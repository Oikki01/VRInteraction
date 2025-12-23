using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class ClassObjectExpand
{
    public static T JsonCopy<T>(T t) where T : class
    {
        return JsonFullSerializer.LoadJsonText<T>(JsonFullSerializer.ConvertToJson(t));
    }

    public static T DeepCopy<T>(T t) where T : class
    {
        using Stream stream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        ((IFormatter)binaryFormatter).Serialize(stream, (object)t);
        stream.Seek(0L, SeekOrigin.Begin);
        return (T)((IFormatter)binaryFormatter).Deserialize(stream);
    }
}