public class KeyValue
{
    public string Key;

    public PerformPanelType Value;

    public PerformBehaviour PerformBehaviour;

    public KeyValue(string key, PerformPanelType value, PerformBehaviour performBehaviour)
    {
        Key = key;
        Value = value;
        PerformBehaviour = performBehaviour;
    }
}