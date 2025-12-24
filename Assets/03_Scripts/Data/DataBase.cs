using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataBase
{
    public int StepId;

    public string StepName;

    public string ModelFullName;

    public string StepDescribe;

    public ControlType control;

    public Color OutLineColor = Color.blue;

    public PerformBase Perform;

    public int ToolId;

    public float DelayTime;

    public float ZOffset;

    public string MoveModelFullName;

    public bool IsDoMove;

    public bool IsScrew;

    public virtual string ToJson()
    {
        return JsonFullSerializer.ConvertToJson(this);
    }

    public void SetPerformFullName()
    {
        Perform.SetFullName(ModelFullName);
        //foreach (KeyValuePair<string, PerformBase> item in ExtendActionDict)
        //{
        //    item.Value.SetFullName(item.Key);
        //}
    }

    public static T ToData<T>(string str) where T : DataBase
    {
        return JsonFullSerializer.LoadJsonText<T>(str);
    }

    public static PerformBase SetModelType(string Name, string TypeName)
    {
        Type type = Type.GetType(TypeName + ",VEM_CarryOut");
        if (type == null)
        {
            Debug.Log("No This Type : " + TypeName);
            return null;
        }

        return type.Assembly.CreateInstance(TypeName) as PerformBase;
    }
}