using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class PerformExpand
{
    public static CarryOutBase Bind(Transform Partent, DataBase data, UnityAction<CarryOutBase> action)
    {
        //PerformTypeAttribute perform = data.Perform.GetPerform();
        CarryOutBase carryOutBase = null;
        //carryOutBase = ((!(Partent == null)) ? ((CarryOutBase)Partent.Find(data.ModelFullName).gameObject.AddComponent(perform.PerformType)) : ((CarryOutBase)GameObject.Find(data.ModelFullName).AddComponent(perform.PerformType)));
        //carryOutBase.perform = ClassObjectExpand.JsonCopy(data.Perform);
        //action?.Invoke(carryOutBase);
        return carryOutBase;
    }

    public static CarryOutBase Bind(Transform Partent, string FullName, PerformBase perform, UnityAction<CarryOutBase> action)
    {
        PerformTypeAttribute perform2 = perform.GetPerform();
        CarryOutBase carryOutBase = null;
        carryOutBase = ((!(Partent == null)) ? ((CarryOutBase)Partent.Find(FullName).gameObject.AddComponent(perform2.PerformType)) : ((CarryOutBase)GameObject.Find(FullName).AddComponent(perform2.PerformType)));
        carryOutBase.perform = ClassObjectExpand.JsonCopy(perform);
        action?.Invoke(carryOutBase);
        return carryOutBase;
    }
}