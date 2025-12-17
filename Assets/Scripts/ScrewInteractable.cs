using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//========================================
//作    者:HJK
//创建时间:2025.12.17
//备    注:螺丝的交互逻辑
//========================================
public class ScrewInteractable : MonoBehaviour
{
    #region 私有变量

    // 当前正在拧的手柄
    private Transform controller;
    // 上一帧手柄旋转
    private Quaternion lastRotation;
    // 当前拧入深度
    private float currentDepth;
    // 旋转角度累计（多圈）
    private float angleAccumulator;
    //螺丝旋转轴向
    private Vector3 m_RotateAxis;
    //当前工具
    private Transform m_CurTool;
    //螺丝是否拆卸完成
    private bool m_IsOverScrew = false;

    #endregion

    #region 公共变量

    [Header("螺丝组件")]
    //螺丝模型（旋转和移动）
    public Transform Screw;
    //工具吸附位置
    public Transform SnapTarget;

    [Header("螺纹参数")]
    //每旋转一周，螺丝前进的距离
    public float MovePerTurn = 0.01f;
    //最大拧入深度（拧紧）
    public float MaxDepth = 0.05f;
    //最小深度（完全拆卸）
    public float MinDepth = 0f;

    [Header("旋转灵敏度")]
    public float RotateSensitivity = 1f;

    [Header("对准要求")]
    //工具与螺丝轴的夹角要求
    public float AlignDot = 0.9f;

    [Header("顺序拧（可选）")]
    //-1 表示不限制顺序
    public int OrderIndex = -1;

    [Header("震动反馈")]
    public SteamVR_Action_Vibration Haptic;

    [Header("螺丝旋转轴向")]
    public RotateAxis RotateAxisType = RotateAxis.X;

    public bool IsTightened => currentDepth >= MaxDepth - 0.0001f;
    public bool IsRemoved => currentDepth <= MinDepth + 0.0001f;

    #endregion

    #region Mono相关

    private void Start()
    {
        switch (RotateAxisType)
        {
            case RotateAxis.X:
                m_RotateAxis = transform.right;
                break;
            case RotateAxis.Y:
                m_RotateAxis = transform.up;
                break;
            case RotateAxis.Z:
                m_RotateAxis = transform.forward;
                break;
        }
    }

    private void Update()
    {
        if (!controller) return;
        //if (m_IsOverScrew)
        //{
        //    PlayHaptic(0.8f, 200f); // 强震动提示“卡住”
        //    return;
        //} 

        // ---------- 1. 对准检测 ----------
        //float dot = Vector3.Dot(controller.forward, transform.up);
        //if (dot < AlignDot) return;

        // ---------- 2. 计算手柄旋转变化 ----------
        Quaternion delta = controller.rotation * Quaternion.Inverse(lastRotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);

        // 判断是否绕螺丝轴旋转
        float axisDot = Vector3.Dot(axis, m_RotateAxis);
        float signedAngle = angle * Mathf.Sign(axisDot) * RotateSensitivity;

        if (Mathf.Abs(signedAngle) < 0.1f) 
            return;

        // ---------- 3. 累计旋转角度（多圈） ----------
        angleAccumulator += signedAngle;

        // ---------- 4. 旋转螺丝模型 ----------
        Screw.Rotate(m_RotateAxis, signedAngle, Space.World);

        // ---------- 5. 根据旋转推进或拉出螺丝 ----------
        //螺丝转一圈在unity世界中前进的距离
        float move = signedAngle / 360f * MovePerTurn;
        //比如MovePerTurn = 0.01f则为前进1cm，MaxDepth = 0.02则需要两圈完成
        float nextDepth = Mathf.Clamp(currentDepth + move, MinDepth, MaxDepth);

        // 到达极限（拧到底 / 拆完）
        if (Mathf.Approximately(nextDepth, currentDepth))
        {
            PlayHaptic(0.8f, 200f); // 强震动提示“卡住”
            return;
        }

        currentDepth = nextDepth;
        Screw.position -= m_RotateAxis * move;

        // ---------- 6. 每半圈震动反馈 ----------
        if (Mathf.Abs(angleAccumulator) >= 180f)
        {
            PlayHaptic(0.3f, 150f);
            angleAccumulator = 0f;
        }

        lastRotation = controller.rotation;
    }

    #endregion

    #region 私有函数/业务逻辑

    /// <summary>
    /// 震动
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    void PlayHaptic(float amplitude, float frequency)
    {
        if (Haptic == null || controller == null) return;

        Haptic.Execute(
            0,
            0.05f,
            frequency,
            amplitude,
            SteamVR_Input_Sources.Any
        );
    }

    //螺丝拆卸完毕
    void OverScrew()
    {
        controller = null;
        m_CurTool.GetComponent<AdsorptionTool>().OnRelease();
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 螺丝刀开始拧此螺丝
    /// </summary>
    public void BeginScrew(Transform ctrl)
    {
        controller = ctrl;
        lastRotation = controller.rotation;
        angleAccumulator = 0f;
    }

    /// <summary>
    /// 停止拧（松手 / 切换目标）
    /// </summary>
    public void EndScrew()
    {
        controller = null;
    }

    /// <summary>
    /// 螺丝刀吸附到位（只负责自己，不做选择）
    /// </summary>
    public void SnapDriver(Transform driver)
    {
        driver.position = SnapTarget.position;
        driver.rotation = Quaternion.LookRotation(transform.up, -transform.forward);
        m_CurTool = driver;
    }

    #endregion
}


public enum RotateAxis
{
    X,
    Y,
    Z
}
