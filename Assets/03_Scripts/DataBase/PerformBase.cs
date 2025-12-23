using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public abstract class PerformBase : IDataFiles
{
    public int PartId;

    protected Dictionary<string, HashSet<string>> relatedDict = new Dictionary<string, HashSet<string>>();

    private List<MemberInfo> HasPerformMember;

    public string FullName { get; private set; }

    public List<MemberInfo> PerformMembers => HasPerformMember;

    public void SetFullName(string Value)
    {
        FullName = Value;
    }

    public int GetPartId()
    {
        return PartId;
    }

    public virtual Dictionary<string, HashSet<string>> GetRelatedDict()
    {
        return relatedDict;
    }

    protected void AddRelatedToDict(string keyStr, string relatKeyStr)
    {
        if (relatedDict.ContainsKey(keyStr))
        {
            relatedDict[keyStr].Add(relatKeyStr);
            return;
        }

        HashSet<string> hashSet = new HashSet<string>();
        hashSet.Add(relatKeyStr);
        relatedDict.Add(keyStr, hashSet);
    }

    public PerformBase()
    {
        FieldInfo[] fields = GetType().GetFields();
        PropertyInfo[] properties = GetType().GetProperties();
        HasPerformMember = new List<MemberInfo>();
        FieldInfo[] array = fields;
        foreach (FieldInfo fieldInfo in array)
        {
            if (fieldInfo.GetCustomAttribute<PerformFieldAttribute>() != null)
            {
                HasPerformMember.Add(fieldInfo);
            }
        }

        PropertyInfo[] array2 = properties;
        foreach (PropertyInfo propertyInfo in array2)
        {
            if (propertyInfo.GetCustomAttribute<PerformFieldAttribute>() != null)
            {
                HasPerformMember.Add(propertyInfo);
            }
        }
    }

    public virtual object GetTureValue(string name, object param)
    {
        return null;
    }

    public List<KeyValue> GetFiles()
    {
        List<KeyValue> list = new List<KeyValue>();
        foreach (MemberInfo item in HasPerformMember)
        {
            PerformFieldAttribute customAttribute = item.GetCustomAttribute<PerformFieldAttribute>();
            if (customAttribute != null)
            {
                list.Add(new KeyValue(customAttribute.FieldName, customAttribute.FielType, customAttribute.PerformBehaviour));
            }
        }

        return list;
    }

    public void SetValue(string MarkName, object value)
    {
        foreach (MemberInfo item in HasPerformMember)
        {
            PerformFieldAttribute customAttribute = item.GetCustomAttribute<PerformFieldAttribute>();
            if (customAttribute != null && customAttribute.FieldName == MarkName && customAttribute.FieldName == MarkName)
            {
                if (item is FieldInfo)
                {
                    (item as FieldInfo).SetValue(this, value);
                }
                else if (item is PropertyInfo)
                {
                    (item as PropertyInfo).SetValue(this, value);
                }
            }
        }
    }

    public object GetValue(string MarkName)
    {
        foreach (MemberInfo item in HasPerformMember)
        {
            PerformFieldAttribute customAttribute = item.GetCustomAttribute<PerformFieldAttribute>();
            if (customAttribute != null && customAttribute.FieldName == MarkName && customAttribute.FieldName == MarkName)
            {
                if (item is FieldInfo)
                {
                    return (item as FieldInfo).GetValue(this);
                }

                if (item is PropertyInfo)
                {
                    return (item as PropertyInfo).GetValue(this);
                }
            }
        }

        return null;
    }

    public bool TryGetValue<T>(string MarkName, ref T value)
    {
        object value2 = GetValue(MarkName);
        if (value2 == null)
        {
            return false;
        }

        if (!(value2 is T))
        {
            return false;
        }

        value = (T)value2;
        return true;
    }

    public PerformTypeAttribute GetPerform()
    {
        return GetType().GetCustomAttribute<PerformTypeAttribute>(inherit: true);
    }

    public void CopyValueByOther(PerformBase OtherPerform)
    {
        foreach (MemberInfo item in OtherPerform.HasPerformMember)
        {
            PerformFieldAttribute customAttribute = item.GetCustomAttribute<PerformFieldAttribute>();
            PerformNoConfigAttribute customAttribute2 = item.GetCustomAttribute<PerformNoConfigAttribute>();
            if (customAttribute != null && customAttribute2 == null)
            {
                SetValue(customAttribute.FieldName, OtherPerform.GetValue(customAttribute.FieldName));
            }
        }
    }

    public static CarryOutBase BindCarryOut(Transform ModleRoot, PerformBase perform, UnityAction<CarryOutBase> action)
    {
        PerformTypeAttribute perform2 = perform.GetPerform();
        CarryOutBase carryOutBase = null;
        carryOutBase = ((!(ModleRoot == null)) ? ((CarryOutBase)ModleRoot.Find(perform.FullName).gameObject.GetOrAddComponent(perform2.PerformType)) : ((CarryOutBase)GameObject.Find(perform.FullName).GetOrAddComponent(perform2.PerformType)));
        carryOutBase.perform = ClassObjectExpand.JsonCopy(perform);
        action?.Invoke(carryOutBase);
        return carryOutBase;
    }

    public virtual PerformBase Copy()
    {
        return MemberwiseClone() as PerformBase;
    }
}