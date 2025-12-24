using DG.Tweening;
using HighlightingSystem;
using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class CarryOutComponent : MonoBehaviour, IColliderEventHoverEnterHandler,
    IColliderEventHoverExitHandler
{
    #region 私有变量

    private GradientColorKey[] color;

    private GradientAlphaKey[] alpha = new GradientAlphaKey[2]
    {
        new GradientAlphaKey(1f, 0f),
        new GradientAlphaKey(1f, 1f)
    };

    private bool IsTriggerEnter;

    private string[] Models;

    #endregion

    #region 公共变量

    public int StepID;

    public Color OutLine;

    public Collider ColliderCom;

    public Highlighter Highlightable;

    public bool IsDoMove;

    public string ModelFullName;

    public bool IsScrew;

    public ColliderType colliderType { get; private set; }

    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }

    public float ZOffset;

    public string MoveModelFullName;

    #endregion

    #region Mono相关

    private void Start()
	{

    }

    private void Update()
    {

    }

    #endregion

    #region 私有函数/业务逻辑



    #endregion

    #region 公共函数/业务逻辑

    public void Init()
    {
        GlobalManager.Instance.CurStepID = StepID;
        Models = ModelFullName.Split(";");
        if (Models.Length > 1)
        {
            for (int i = 0; i < Models.Length; i++)
            {
                GameObject go = GameObject.Find(Models[i]);
                if (go)
                {
                    go.GetOrAddComponent<Highlighter>();
                    HighLightFlash(go.GetComponent<Highlighter>(), Color.blue, Color.red, null);
                }
            }
        }

        if (IsScrew)
        {
            List<ScrewInteractable> screwInteractables = ScrewManager.Instance.GetAllScrewInteractable();
            foreach (var item in screwInteractables)
            {
                item.IsInteractable = true;
            }
        }
    }

    public void Play(UnityAction callback)
    {
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


    public void HighLightOff()
    {
        if (Models.Length > 1)
        {
            for (int i = 0; i < Models.Length; i++)
            {
                GameObject go = GameObject.Find(Models[i]);
                if (go)
                {
                    if (go.GetComponent<Highlighter>())
                    {
                        go.GetComponent<Highlighter>().constant = false;
                        go.GetComponent<Highlighter>().tween = false;
                    }
                }
            }
        }   
    }

    public void AddHighLight()
    {
        if ((UnityEngine.Object)(object)Highlightable == null)
        {
            Highlightable = base.gameObject.GetOrAddComponent<Highlighter>();
        }
    }

    public void HighLightFlash(Color From, Color To, UnityAction callback)
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

    public void HighLightFlash(Highlighter highlighter, Color From, Color To, UnityAction callback)
    {
        callback?.Invoke();
            highlighter.tween = true;
            highlighter.constant = false;
            highlighter.tweenDuration = 0.5f;
            StartCoroutine(LightFlashDelay(highlighter,From, To));
    }

    public IEnumerator LightFlashDelay(Highlighter highlighter,Color From, Color To)
    {
        yield return new WaitForFixedUpdate();
        color = new GradientColorKey[2]
        {
            new GradientColorKey(From, 0f),
            new GradientColorKey(To, 1f)
        };
        highlighter.tweenGradient.SetKeys(color, alpha);
    }

    public IEnumerator LightFlashDelay(Color From, Color To)
    {
        yield return new WaitForFixedUpdate();
        color = new GradientColorKey[2]
        {
            new GradientColorKey(From, 0f),
            new GradientColorKey(To, 1f)
        };
        Highlightable.tweenGradient.SetKeys(color, alpha);
    }

    public IEnumerator LightFlashDelay(Color To)
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

        GlobalManager.Instance.CurStepID = 0;
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        //如果可以移动则进入 不可以移动则退出
        if (!IsDoMove) return;
        if (IsTriggerEnter) return;
        if (ProcessManager.Instance.FlowDataList[ProcessManager.Instance.CurStepIndex].StepId == StepID)
        {
            //播放动画 动画完成开启下一步
            GameObject go = GameObject.Find(MoveModelFullName);
            if (go)
            {
                IsTriggerEnter = true;
                go.transform.DOLocalMoveZ(ZOffset, 1f).OnComplete(() => {
                    ProcessManager.Instance.StepComplete();
                });
            }
        }
    }

    #endregion
}
