using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class CarryOutOutValueAttribute : Attribute
{
    public string OutValueInfo;

    public Type ValueType;
}