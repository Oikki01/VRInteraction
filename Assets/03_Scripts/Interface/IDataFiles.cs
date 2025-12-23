using System.Collections.Generic;

public interface IDataFiles
{
    List<KeyValue> GetFiles();

    void SetValue(string MarkName, object value);

    object GetValue(string MarkName);

    int GetPartId();

    Dictionary<string, HashSet<string>> GetRelatedDict();
}