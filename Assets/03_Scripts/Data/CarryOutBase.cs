using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.Events;

public abstract class CarryOutBase : MonoBehaviour
{
    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }

    public class CarryMessageInfo
    {
        public string OutValueInfo;

        public Type ValueType;

        public object Value;
    }

    public int ID;

    public bool IsExtend;

    public int StepID;

    public HashSet<CarryOutBase> LinksCarryOut = new HashSet<CarryOutBase>();

    [SerializeField]
    public PerformBase perform;

    protected List<CarryMessageInfo> CarryMessages = new List<CarryMessageInfo>();

    protected List<FieldInfo> MessageInfos = new List<FieldInfo>();

    protected Collider ColliderCom;

    protected Highlighter Highlightable;

    private GradientColorKey[] color;

    private GradientAlphaKey[] alpha = new GradientAlphaKey[2]
    {
        new GradientAlphaKey(1f, 0f),
        new GradientAlphaKey(1f, 1f)
    };

    public ColliderType colliderType { get; private set; }

    protected virtual void Awake()
    {
        FieldInfo[] fields = GetType().GetFields();
        foreach (FieldInfo fieldInfo in fields)
        {
            if (fieldInfo.GetCustomAttribute<CarryOutOutValueAttribute>() != null)
            {
                MessageInfos.Add(fieldInfo);
            }
        }
    }

    public abstract void Init(UnityAction callback);

    public abstract void Play(UnityAction callback);

    public abstract void EditorPlay(UnityAction callback);

    public abstract void ResetData(UnityAction callback);

    public void SendLinkTragetData()
    {
        GetCarryMessageData();
        foreach (CarryOutBase item in LinksCarryOut)
        {
            item.Play(null);
        }
    }

    private void GetCarryMessageData()
    {
        CarryMessages.Clear();
        foreach (FieldInfo messageInfo in MessageInfos)
        {
            CarryMessageInfo carryMessageInfo = new CarryMessageInfo();
            CarryOutOutValueAttribute customAttribute = messageInfo.GetCustomAttribute<CarryOutOutValueAttribute>();
            carryMessageInfo.OutValueInfo = customAttribute.OutValueInfo;
            carryMessageInfo.ValueType = customAttribute.ValueType;
            carryMessageInfo.Value = messageInfo.GetValue(this);
            CarryMessages.Add(carryMessageInfo);
        }
    }

    public void AddCollider()
    {
        if ((bool)GetComponent<MeshRenderer>())
        {
            ColliderCom = base.gameObject.GetOrAddComponent<MeshCollider>();
        }
        else
        {
            ColliderCom = base.gameObject.GetOrAddComponent<BoxCollider>();
            MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
            if (componentsInChildren.Length != 0)
            {
                Bounds trsBounds = TransformUtil.GetTrsBounds(componentsInChildren);
                (ColliderCom as BoxCollider).size = trsBounds.size;
                (ColliderCom as BoxCollider).center = base.transform.position - trsBounds.center;
            }
        }

        ColliderCom.enabled = false;
        colliderType = ColliderType.Mesh;
    }

    protected void AddHighLight()
    {
        if ((UnityEngine.Object)(object)Highlightable == null)
        {
            Highlightable = base.gameObject.GetOrAddComponent<Highlighter>();
        }
    }

    public virtual void HighLightFlash(Color From, Color To, UnityAction callback)
    {
        callback?.Invoke();
        Highlightable = base.gameObject.GetOrAddComponent<Highlighter>();
        if (From == To)
        {
            Highlightable.tween = false;
            Highlightable.constant = true;
            StartCoroutine(LightFlashDelay(To));
        }
        else
        {
            Highlightable.tween = true;
            Highlightable.constant = false;
            Highlightable.tweenDuration = 0.5f;
            StartCoroutine(LightFlashDelay(From, To));
        }
    }

    protected IEnumerator LightFlashDelay(Color From, Color To)
    {
        yield return new WaitForFixedUpdate();
        color = new GradientColorKey[2]
        {
            new GradientColorKey(From, 0f),
            new GradientColorKey(To, 1f)
        };
        Highlightable.tweenGradient.SetKeys(color, alpha);
    }

    protected IEnumerator LightFlashDelay(Color To)
    {
        yield return new WaitForFixedUpdate();
        Highlightable.constantColor = To;
    }

    public void HighLightOff(UnityAction callback)
    {
        if ((UnityEngine.Object)(object)Highlightable != null)
        {
            Highlightable.constant = false;
            Highlightable.tween = false;
        }

        callback?.Invoke();
    }

    public void SetCollider(bool value, UnityAction callback)
    {
        if (ColliderCom == null)
        {
            AddCollider();
        }

        ColliderCom.enabled = value;
        callback?.Invoke();
    }
}