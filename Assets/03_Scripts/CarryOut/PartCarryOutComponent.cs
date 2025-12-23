using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class PartCarryOutComponent : CarryOutBase, InterfaceUsed, IEventSystemHandler
{
    public enum DisassblyState
    {
        ShowHalf,
        Dissove,
        LightOn,
        LightOff,
        CollierOn,
        CollierOff,
        ActiveOn,
        ActiveOff,
        AddPartBox,
        SubPartBox,
        Restore,
        ToolToHand,
        LinkPart
    }

    public class PartLinkPackage
    {
        public ToolOperateType tooltype;
    }

    public ToolCarryOutComponent tool;

    public string ReallyName;

    public Color OutLine;

    public bool Flash;

    public Transform Parent;

    public Vector3 ToolOffsetPos;

    public Vector3 ToolOffsetEuler;

    public Vector3 ToolDirection;

    protected Vector3 OriPos;

    protected Quaternion OriRota;

    protected Vector3 OriWorldPos;

    [SerializeField]
    protected bool IsDisassemble = true;

    protected QueuePool<UnityAction> pool = new QueuePool<UnityAction>();

    private Color NormalHalfColor = new Color(0f, 0f, 0f, 0f);

    private int points;

    private UnityAction LinkPartsOver;

    public abstract void LinkPartsPlay(UnityAction callback, PartLinkPackage data);

    public override void Init(UnityAction callback)
    {
        AddCollider();
        Debug.Log(base.name);
        Parent = base.transform.parent;
        OriPos = base.transform.localPosition;
        OriRota = base.transform.localRotation;
        OriWorldPos = base.transform.position;
        foreach (CarryOutBase item in LinksCarryOut)
        {
            item.Init(null);
        }
    }

    public override void ResetData(UnityAction callback)
    {
        base.transform.SetParent(Parent);
        base.transform.localPosition = OriPos;
        base.transform.localRotation = OriRota;
        IsDisassemble = true;
        Execution(null, DisassblyState.Restore);
        HighLightOff(null);
        foreach (CarryOutBase item in LinksCarryOut)
        {
            item.ResetData(null);
        }

        if (tool != null)
        {
            tool.ResetData(null);
        }

        LinksCarryOut.Clear();
        callback?.Invoke();
    }

    public virtual void GameObjectActive(bool value, UnityAction callback)
    {
        base.gameObject.SetActive(value);
        callback?.Invoke();
    }

    public virtual void Execution(UnityAction Lastcallback, params DisassblyState[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            pool.Push(GetAction(states[i]));
        }

        pool.Push(Lastcallback);
        pool.Pop()?.Invoke();
    }

    public virtual void ExecutionAdd(params DisassblyState[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            pool.Push(GetAction(states[i]));
        }
    }

    public override void HighLightFlash(Color From, Color To, UnityAction callback)
    {
        Color to = ((!Flash) ? To : NormalHalfColor);
        switch (VEMPattern.Train)
        {
            case VEMPattern.Teach:
                base.HighLightFlash(From, to, callback);
                break;
            case VEMPattern.Train:
                base.HighLightFlash(From, to, callback);
                break;
            case VEMPattern.Exam:
                callback?.Invoke();
                break;
        }
    }

    private UnityAction GetAction(DisassblyState state)
    {
        switch (state)
        {
            case DisassblyState.ActiveOn:
                return delegate
                {
                    GameObjectActive(value: true, pool.Pop());
                };
            case DisassblyState.ActiveOff:
                return delegate
                {
                    GameObjectActive(value: false, pool.Pop());
                };
            case DisassblyState.Dissove:
                return delegate
                {
                    GameObjectEffect.Dissolve.RunDissolve(base.gameObject, 1f, DissolveOrNormal: true, pool.Pop());
                };
            case DisassblyState.ShowHalf:
                return delegate
                {
                    GameObjectEffect.HalfGlass.RunHalf(base.gameObject, pool.Pop());
                };
            case DisassblyState.CollierOn:
                return delegate
                {
                    SetCollider(value: true, pool.Pop());
                };
            case DisassblyState.CollierOff:
                return delegate
                {
                    SetCollider(value: false, pool.Pop());
                };
            case DisassblyState.LightOff:
                return delegate
                {
                    HighLightOff(pool.Pop());
                };
            case DisassblyState.LightOn:
                return delegate
                {
                    HighLightFlash(OutLine, OutLine, pool.Pop());
                };
            case DisassblyState.AddPartBox:
                {
                    Texture2D modelTexture = CameraShootUtil.GetModelTexture(Object.Instantiate(base.gameObject), new Rect(0f, 0f, 128f, 128f));
                    Sprite.Create(modelTexture, new Rect(0f, 0f, modelTexture.width, modelTexture.height), new Vector2(0.5f, 0.5f));
                    return delegate
                    {
                    };
                }
            case DisassblyState.Restore:
                return delegate
                {
                    GameObjectEffect.Restore(base.gameObject, pool.Pop());
                };
            case DisassblyState.ToolToHand:
                return delegate
                {
                    //Singleton<PlatformManager>.Instance.body.RightHand.TemporaryLeaveHand(OutOrIn: false);
                    pool.Pop()?.Invoke();
                };
            case DisassblyState.LinkPart:
                return delegate
                {
                    pool.Pop()?.Invoke();
                };
            default:
                return null;
        }
    }

    private void LinkNum(UnityAction action, int num)
    {
        points = num;
        LinkPartsOver = action;
    }

    private void OverLink()
    {
        points--;
        if (points <= 0)
        {
            LinkPartsOver?.Invoke();
        }
    }

    public void BindTool(ToolCarryOutComponent _tool)
    {
        tool = _tool;
    }

    public void UnBindTool()
    {
        if (tool != null)
        {
            tool.ResetData(null);
            tool = null;
        }
    }

    public abstract void Used(InputEventData game);

    public abstract void Using(InputEventData game);

    public abstract void UnUsed(InputEventData game);
}