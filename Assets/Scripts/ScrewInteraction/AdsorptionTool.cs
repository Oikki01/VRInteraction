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

    [Header("工具类型")]
    public ToolType CurrentToolType;

    // 当前应拧的顺序索引
    public int CurrentOrderIndex = 0;

    public Transform RightHandAdsorptionTool;

    #endregion

    #region Mono相关

    private void Start()
    {

    }

    private void Update()
    {
        SelectScrew();
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

        //if (currentScrew == screw)
        //    DetachCurrentScrew();
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
            //DetachCurrentScrew();

            if (best)
                AttachToScrew(best);
        }
    }

    void AttachToScrew(ScrewInteractable screw)
    {
        currentScrew = screw;
        //currentScrew.SnapDriver(transform);
    }

    void DetachCurrentScrew()
    {
        if (currentScrew)
            currentScrew.EndScrew();

        transform.SetParent(RightHandAdsorptionTool);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(-90, 0, 0);

        currentScrew = null;
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 玩家抓住螺丝刀
    /// </summary>
    public void OnGrab(Transform controller)
    {
        if (!IsGrab) 
        {
            if (currentScrew)
            {
                currentScrew.BeginScrew(controller, transform);
                transform.SetParent(currentScrew.SnapTarget);
                IsGrab = true;
            }
        }
    }

    /// <summary>
    /// 玩家松开螺丝刀
    /// </summary>
    public void OnRelease()
    {
        if (IsGrab)
        {
            // 如果当前螺丝已拧紧 / 拆卸完成，推进顺序
            if (UseOrder && currentScrew && currentScrew.IsTightened)
                CurrentOrderIndex++;

            DetachCurrentScrew();

            IsGrab = false;
        }
    }

    #endregion
}


public enum ToolType
{
    Screwdriver,    // 螺丝刀
    Wrench,         // 扳手
    SquareDriver    // 外四角工具
}