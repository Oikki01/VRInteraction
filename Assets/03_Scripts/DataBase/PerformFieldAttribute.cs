using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class PerformFieldAttribute : Attribute
{
    public string FieldName;

    public PerformPanelType FielType;

    public PerformBehaviour PerformBehaviour;

    public PerformFieldAttribute(string Name, PerformPanelType type, PerformBehaviour PerformBehaviour = PerformBehaviour.None)
    {
        FieldName = Name;
        FielType = type;
        this.PerformBehaviour = PerformBehaviour;
    }
}