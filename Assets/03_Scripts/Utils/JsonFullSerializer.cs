using System.IO;
using FullSerializer;
using UnityEngine;

public class JsonFullSerializer : fsSerializer
{
    public static string SaveJsonFile<T>(string path, T data, bool isCompressed = false) where T : class
    {
        new fsSerializer().TrySerialize(data, out var data2).AssertSuccess();
        StreamWriter streamWriter = new StreamWriter(path);
        string text = null;
        text = ((!isCompressed) ? fsJsonPrinter.PrettyJson(data2) : fsJsonPrinter.CompressedJson(data2));
        streamWriter.WriteLine(text);
        streamWriter.Close();
        return text;
    }

    public static string ConvertToJson<T>(T data, bool isCompressed = false) where T : class
    {
        new fsSerializer().TrySerialize(data, out var data2).AssertSuccess();
        string text = null;
        if (isCompressed)
        {
            return fsJsonPrinter.CompressedJson(data2);
        }

        return fsJsonPrinter.PrettyJson(data2);
    }

    public static T LoadJsonText<T>(string text) where T : class
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        object result = null;
        fsData data = fsJsonParser.Parse(text);
        new fsSerializer().TryDeserialize(data, typeof(T), ref result).AssertSuccess();
        return result as T;
    }

    public static T LoadJsonFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        object result = null;
        using (StreamReader streamReader = new StreamReader(path))
        {
            fsData data = fsJsonParser.Parse(streamReader.ReadToEnd());
            new fsSerializer().TryDeserialize(data, typeof(T), ref result).AssertSuccess();
            streamReader.Close();
        }

        return result as T;
    }

    public static T LoadJsonFromRes<T>(string path) where T : class
    {
        string text = Resources.Load<TextAsset>(path).text;
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        object result = null;
        fsData data = fsJsonParser.Parse(text);
        new fsSerializer().TryDeserialize(data, typeof(T), ref result).AssertSuccess();
        return result as T;
    }
}