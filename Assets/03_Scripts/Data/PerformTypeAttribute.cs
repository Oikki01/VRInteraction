using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PerformTypeAttribute : Attribute
{
    public Type PerformType;

    public string PerformName;

    public PerformTypeAttribute(string Name, Type type)
    {
        PerformName = Name;
        PerformType = type;
    }
}