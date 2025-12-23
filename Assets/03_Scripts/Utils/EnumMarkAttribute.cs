using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumMarkAttribute : Attribute
{
    public string Mark;

    public EnumMarkAttribute(string mark)
    {
        Mark = mark;
    }
}