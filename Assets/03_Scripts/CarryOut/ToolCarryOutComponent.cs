using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ToolCarryOutComponent : CarryOutBase, InterfaceGrip, IEventSystemHandler
{
    public PartCarryOutComponent part;

    public ToolOperateType toolOperateType;

    protected Transform Parent;

    protected Vector3 OriPos;

    protected Quaternion OriRota;

    protected string Gesture;

    protected Vector3 OnHandPosOffset;

    protected Vector3 OnHandRotaOffset;

    protected virtual void Update()
    {
    }

    public void BindPart(PartCarryOutComponent partCarry)
    {
        part = partCarry;
        partCarry.BindTool(this);
    }

    public void UnBindPart()
    {
        part.UnBindTool();
        part = null;
    }

    public virtual void ChangeTool()
    {
    }

    public override void Init(UnityAction callback)
    {
        Parent = base.transform.parent;
        OriPos = base.transform.localPosition;
        OriRota = base.transform.localRotation;
    }

    public override void ResetData(UnityAction callback)
    {
        base.transform.SetParent(Parent);
        base.transform.localPosition = OriPos;
        base.transform.localRotation = OriRota;
        callback?.Invoke();
    }

    protected bool CheckPart()
    {
        if (part == null)
        {
            return false;
        }

        return true;
    }

    public abstract void AdsorbentCorrectionPosition(UnityAction callback);

    public virtual void GripOn(InputEventData game)
    {
        base.gameObject.SetActive(value: true);
    }

    public virtual void GripDown(InputEventData game)
    {
        base.gameObject.SetActive(value: false);
    }

    public void Griping(InputEventData game)
    {
        throw new NotImplementedException();
    }

    public override void EditorPlay(UnityAction callback)
    {
        throw new NotImplementedException();
    }
}