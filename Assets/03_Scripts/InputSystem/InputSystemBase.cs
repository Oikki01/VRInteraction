using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InputSystemBase : BaseInputModule
{
    //[HideInInspector]
    //public Dictionary<string, CarryOutBase> CurrtCarryOuts = new Dictionary<string, CarryOutBase>();

    //public float MaxRayDis = 10f;

    //[HideInInspector]
    //public BodyCarryOut Body;

    //protected Dictionary<int, Dictionary<OutType, int>> KeyMaps;

    //protected Dictionary<OutType, int> NowKeyMap = new Dictionary<OutType, int>();

    //protected RaycastHit raycast;

    //protected void BindKey(int KeyMapIndex, OutType OutHandle, int EnumValue)
    //{
    //    if (KeyMaps == null)
    //    {
    //        KeyMaps = new Dictionary<int, Dictionary<OutType, int>>();
    //    }

    //    if (!KeyMaps.ContainsKey(KeyMapIndex))
    //    {
    //        KeyMaps.Add(KeyMapIndex, new Dictionary<OutType, int>());
    //    }

    //    Dictionary<OutType, int> dictionary = KeyMaps[KeyMapIndex];
    //    if (dictionary == null)
    //    {
    //        dictionary = new Dictionary<OutType, int>();
    //    }

    //    if (dictionary.ContainsKey(OutHandle))
    //    {
    //        dictionary[OutHandle] = EnumValue;
    //    }
    //    else
    //    {
    //        dictionary.Add(OutHandle, EnumValue);
    //    }
    //}

    //public abstract void BindKeyByRealize(OutType OutHandle, int EnumValue);

    //protected virtual Dictionary<OutType, int> SelectKeyMap(int KeyMapIndex)
    //{
    //    if (KeyMaps == null)
    //    {
    //        return null;
    //    }

    //    if (!KeyMaps.ContainsKey(KeyMapIndex))
    //    {
    //        return null;
    //    }

    //    return NowKeyMap = KeyMaps[KeyMapIndex];
    //}

    //protected virtual void Update()
    //{
    //}

    //protected void SendInput(OutType outType, InputEventData eventData)
    //{
    //    switch (outType)
    //    {
    //        case OutType.Grip:
    //            Body?.GripEvent(eventData);
    //            break;
    //        case OutType.Used:
    //            Body?.UseEvent(eventData);
    //            break;
    //        case OutType.Touch:
    //            Body?.TouchEvent(eventData);
    //            break;
    //        case OutType.MoveForward:
    //            Body?.GetComponent<InterfaceMoveForward>()?.MoveForward(eventData);
    //            break;
    //        case OutType.MoveBack:
    //            Body?.GetComponent<InterfaceMoveBack>()?.MoveBack(eventData);
    //            break;
    //        case OutType.MoveLeft:
    //            Body?.GetComponent<InterfaceMoveLeft>()?.MoveLeft(eventData);
    //            break;
    //        case OutType.MoveRight:
    //            Body?.GetComponent<InterfaceMoveRight>()?.MoveRight(eventData);
    //            break;
    //        case OutType.MoveUp:
    //            Body?.GetComponent<InterfaceMoveUp>()?.MoveUp(eventData);
    //            break;
    //        case OutType.MoveDown:
    //            Body?.GetComponent<InterfaceMoveDown>()?.MoveDown(eventData);
    //            break;
    //        case OutType.AskewLeft:
    //            Body?.GetComponent<InterfaceAskewLeft>()?.AskewLeft(eventData);
    //            break;
    //        case OutType.AskewRight:
    //            Body?.GetComponent<InterfaceAskewRight>()?.AskewRight(eventData);
    //            break;
    //        case OutType.AskewUp:
    //            Body?.GetComponent<InterfaceAskewUp>()?.AskewUp(eventData);
    //            break;
    //        case OutType.AskewDown:
    //            Body?.GetComponent<InterfaceAskewDown>()?.AskewDown(eventData);
    //            break;
    //        case OutType.BesselRay:
    //            Body?.GetComponent<InterfaceBesselRayCast>()?.BesselRayCast(eventData);
    //            break;
    //        case OutType.LineRay:
    //            Body?.GetComponent<InterfaceLineRayCast>()?.LineRayCast(eventData);
    //            break;
    //        case OutType.UICtrl:
    //            Body?.GetComponent<InterfaceUICtrl>()?.UICtrl(eventData);
    //            break;
    //        case OutType.MoveBink:
    //            break;
    //    }
    //}

    //public void SetSelectGame(string table, CarryOutBase carryOutBase)
    //{
    //    if (CurrtCarryOuts.ContainsKey(table))
    //    {
    //        CurrtCarryOuts[table] = carryOutBase;
    //    }
    //    else
    //    {
    //        CurrtCarryOuts.Add(table, carryOutBase);
    //    }
    //}

    //public CarryOutBase GetSelectGame(string table)
    //{
    //    if (CurrtCarryOuts.ContainsKey(table))
    //    {
    //        return CurrtCarryOuts[table];
    //    }

    //    return null;
    //}

    //public static T GetInputSystem<T>() where T : InputSystemBase
    //{
    //    return Object.FindObjectOfType<T>();
    //}
}