using HTC.UnityPlugin.Vive;
using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.17
//备    注:吸附工具控制器(交互螺丝)
//========================================
public class AdsorptionTool : MonoBehaviour
{
    #region 私有变量

    // 范围内的螺丝
    private List<ScrewInteractable> screwsInRange = new();
    private ScrewInteractable currentScrew;

    private bool IsGrab;

    #endregion

    #region 公共变量

    [Header("顺序拧设置")]
    public bool UseOrder = false;

    // 当前应拧的顺序索引
    public int CurrentOrderIndex = 0;

    //右手手柄
    public Transform RightDeviceTracker;

    //抓取工具模型位置
    //public Transform GrabToolModleTrans;


    #endregion

    #region Mono相关

    private void Start()
    {

    }

    private void Update()
    {
        SelectScrew();
        if (ViveInput.GetTriggerValue(HandRole.RightHand) > 0.1f)
        {
            //if (IsGrab)
            //{ 
            //    OnSnapDriver();
            //}
            if (!IsGrab)
                OnGrab(RightDeviceTracker);
        }
        else
        {
            if (IsGrab)
                OnRelease();
        }
    }

    #endregion

    #region 私有函数/业务逻辑

    void OnTriggerEnter(Collider other)
    {
        var screw = other.GetComponentInParent<ScrewInteractable>();
        if (screw && !screwsInRange.Contains(screw))
            screwsInRange.Add(screw);
    }

    void OnTriggerExit(Collider other)
    {
        var screw = other.GetComponentInParent<ScrewInteractable>();
        if (!screw) return;

        screwsInRange.Remove(screw);

        if (currentScrew == screw)
            DetachCurrentScrew();
    }

    void SelectScrew()
    {
        ScrewInteractable best = null;
        float minDist = float.MaxValue;

        foreach (var screw in screwsInRange)
        {
            // ---------- 顺序限制 ----------
            if (UseOrder && screw.OrderIndex != CurrentOrderIndex)
                continue;

            float d = Vector3.Distance(
                transform.position,
                screw.SnapTarget.position
            );

            if (d < minDist)
            {
                minDist = d;
                best = screw;
            }
        }

        if (best != currentScrew)
        {
            DetachCurrentScrew();

            if (best)
                AttachToScrew(best);
        }
    }

    void AttachToScrew(ScrewInteractable screw)
    {
        currentScrew = screw;
        currentScrew.SnapDriver(transform);
    }

    void DetachCurrentScrew()
    {
        if (currentScrew)
            currentScrew.EndScrew();

        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = transform.parent.rotation;

        currentScrew = null;
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 玩家抓住螺丝刀
    /// </summary>
    public void OnGrab(Transform controller)
    {
        if (currentScrew)
        {
            currentScrew.BeginScrew(controller);
            IsGrab = true;
        }
    }

    /// <summary>
    /// 将工具附着到螺丝上
    /// </summary>
    public void OnSnapDriver()
    {
        if (currentScrew)
        {
            currentScrew.SnapDriver(transform);
        }
    }

    /// <summary>
    /// 玩家松开螺丝刀
    /// </summary>
    public void OnRelease()
    {
        // 如果当前螺丝已拧紧 / 拆卸完成，推进顺序
        if (UseOrder && currentScrew && currentScrew.IsTightened)
            CurrentOrderIndex++;

        DetachCurrentScrew();

        IsGrab = false;
    }

    #endregion
}
