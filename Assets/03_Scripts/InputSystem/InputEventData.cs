using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputEventData : BaseEventData
{
    public EuipmentType euipment;

    public string InputEuipment;

    public Vector2 TouchPadAixs;

    public KeyState state;

    public CarryOutBase SelectGame;

    public Camera EventCamera;

    public UseHand Hand;

    public GameObject SelectUIGame;

    public Vector3 RayOriPos;

    public Vector3 RayCastPos;

    public Vector3 RayCastNormal;

    public List<Vector3> RayPoints;

    public int UIEnum;

    public Vector3 LastPos;

    public Vector3 ToPos;

    public Vector3 LastEular;

    public Vector3 ToEular;

    protected EventSystem BaseInputSystem;

    public InputEventData(EventSystem eventSystem)
        : base(eventSystem)
    {
        TouchPadAixs = Vector2.zero;
        RayPoints = new List<Vector3>();
        BaseInputSystem = eventSystem;
    }

    public static PointerEventData InputEventDataToPointerEventData(InputEventData input)
    {
        return new PointerEventData(input.BaseInputSystem);
    }
}