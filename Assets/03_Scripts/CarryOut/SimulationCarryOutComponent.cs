using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class SimulationCarryOutComponent : CarryOutBase, InterfaceUsed, IEventSystemHandler, InterfaceGrip
{
    public class SimulationLinkPackage
    {
        public bool BoolData;

        public string StrData;
    }

    public enum SimulationState
    {
        LightOn,
        LightOff
    }

    public List<int> TragetValueList = new List<int>();

    public Color OutLine;

    public bool Flash;

    public string ReallyName;

    private Color NormalHalfColor = new Color(0f, 0f, 0f, 0f);

    private int points;

    private UnityAction LinkPartsOver;

    protected QueuePool<UnityAction> pool = new QueuePool<UnityAction>();

    public abstract void GripDown(InputEventData game);

    public abstract void GripOn(InputEventData game);

    public override void Init(UnityAction callback)
    {
        AddCollider();
        foreach (CarryOutBase item in LinksCarryOut)
        {
            item.Init(null);
        }
    }

    public override void ResetData(UnityAction callback)
    {
        HighLightOff(null);
        foreach (CarryOutBase item in LinksCarryOut)
        {
            item.ResetData(null);
        }

        LinksCarryOut.Clear();
        callback?.Invoke();
    }

    public abstract void UnUsed(InputEventData game);

    public abstract void Used(InputEventData game);

    public override void HighLightFlash(Color From, Color To, UnityAction callback)
    {
        Color to = ((!Flash) ? To : NormalHalfColor);
        base.HighLightFlash(From, to, callback);
    }

    public abstract bool TragetOver();

    public abstract void AddTragets(PerformBase traget);

    public void TragetsClear()
    {
        TragetValueList.Clear();
    }

    public virtual void Using(InputEventData game)
    {
    }

    public virtual void Griping(InputEventData game)
    {
    }

    public virtual void Execution(UnityAction Lastcallback, params SimulationState[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            pool.Push(GetAction(states[i]));
        }

        pool.Push(Lastcallback);
        pool.Pop()?.Invoke();
    }

    public virtual void ExecutionAdd(params SimulationState[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            pool.Push(GetAction(states[i]));
        }
    }

    private UnityAction GetAction(SimulationState state)
    {
        return state switch
        {
            SimulationState.LightOff => delegate
            {
                HighLightOff(pool.Pop());
            }
            ,
            SimulationState.LightOn => delegate
            {
                HighLightFlash(OutLine, OutLine, pool.Pop());
            }
            ,
            _ => null,
        };
    }
}