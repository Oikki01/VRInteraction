using System.Collections.Generic;

public abstract class FlowDataBase : DataBase
{
    public List<int> PerviousStepList;

    public List<int> NextStepList;

    public StepOperateType StepOperateType;

    public float Score;

    public FlowDataBase Copy()
    {
        FlowDataBase flowDataBase = MemberwiseClone() as FlowDataBase;
        if (Perform == null)
        {
            flowDataBase.Perform = null;
        }
        else
        {
            flowDataBase.Perform = Perform.Copy();
        }

        if (ExtendActionDict == null)
        {
            flowDataBase.ExtendActionDict = null;
        }
        else
        {
            flowDataBase.ExtendActionDict = new Dictionary<string, PerformBase>();
            foreach (KeyValuePair<string, PerformBase> item in ExtendActionDict)
            {
                flowDataBase.ExtendActionDict.Add(item.Key, item.Value.Copy());
            }
        }

        return flowDataBase;
    }
}