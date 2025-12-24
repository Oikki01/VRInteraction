using UnityEngine;
using Valve.VR;
using HighlightingSystem;
using Unity.VisualScripting;

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
    //当前工具
    private Transform m_CurTool;
    // 当前吸附中的工具
    private Transform snapDriver;

    #endregion

    #region 公共变量

    [Header("工具限制")]
    public ToolType RequiredToolType;

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
    //螺丝头朝前(0, 0, 1)
    //螺丝头朝后(0, 0, -1)
    //螺丝头朝上(0, 1, 0)
    //螺丝头朝下(0, -1, 0)
    //螺丝头朝右(1, 0, 0)
    //螺丝头朝左(-1, 0, 0)
    public Vector3 LocalAxis = new Vector3(0, 0, 1);

    [Header("工具相对旋转偏移")]
    public Quaternion ToolRotationOffset = Quaternion.identity;


    [Header("吸附状态")]
    public SnapState SnapStatus = SnapState.None;

    [Header("吸附平滑参数")]
    public float SnapMoveSpeed = 12f;        // 位置平滑速度
    public float SnapRotateSpeed = 12f;      // 旋转平滑速度
    public float SnapPosThreshold = 0.001f;  // 1mm 认为到位
    public float SnapAngleThreshold = 1f;    // 1° 认为稳定

    [Header("初始化螺丝状态")]
    public ScrewInitialState InitialState = ScrewInitialState.Installed;

    public bool IsCompleted;

    [Header("交互状态")]
    [SerializeField]
    private bool isInteractable = false;

    public bool IsInteractable { get { return isInteractable; } set { isInteractable = value; } }

    public bool IsTightened => currentDepth >= MaxDepth - 0.0001f;
    public bool IsRemoved => currentDepth <= MinDepth + 0.0001f;

    #endregion

    #region Mono相关

    private void Start()
    {
        switch (InitialState)
        {
            case ScrewInitialState.Installed:
                currentDepth = MaxDepth;   // 刚放进孔
                break;

            case ScrewInitialState.Tightened:
                currentDepth = MinDepth;   // 已拧紧
                break;
        }
    }

    private void Update()
    {
        if (!isInteractable) return;
        if (!controller) return;
        if (SnapStatus == SnapState.None)
            return;

        // ---------- 吸附过程 ----------
        if (SnapStatus == SnapState.Snapping && snapDriver)
        {
            UpdateSnapDriver();
            return; // ❗吸附过程中，不允许拧
        }

        if (!snapDriver) return;

        Quaternion targetRot =
           SnapTarget.rotation * ToolRotationOffset;

        snapDriver.rotation = Quaternion.Slerp(
            snapDriver.rotation,
            targetRot,
            Time.deltaTime * SnapRotateSpeed
        );

        // ---------- 1. 对准检测 ----------
        //float dot = Vector3.Dot(controller.forward, transform.up);
        //if (dot < AlignDot) return;

        // ---------- 2. 计算手柄旋转变化 ----------
        Quaternion delta = controller.rotation * Quaternion.Inverse(lastRotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);

        // 判断是否绕螺丝轴旋转
        float axisDot = Vector3.Dot(axis, LocalAxis);
        float signedAngle = angle * Mathf.Sign(axisDot) * RotateSensitivity;

        if (Mathf.Abs(signedAngle) < 0.1f)
            return;

        // ---------- 3. 累计旋转角度（多圈） ----------
        //angleAccumulator += signedAngle;

        // ---------- 4. 根据旋转推进或拉出螺丝 ----------
        float move = signedAngle / 360f * MovePerTurn;

        // 判断是否还能动（防无限旋转）
        bool canRotate =
            (move > 0 && currentDepth < MaxDepth) ||   // 正在拧紧
            (move < 0 && currentDepth > MinDepth);     // 正在拆卸

        if (!canRotate)
        {
            PlayHaptic(0.8f, 200f); // 卡死反馈
            return;
        }

        // ---------- 5. 允许后才真正旋转 ----------
        Vector3 worldAxis = Screw.TransformDirection(LocalAxis);

        Screw.Rotate(worldAxis, signedAngle, Space.World);
        Screw.position -= worldAxis * move;

        currentDepth = Mathf.Clamp(currentDepth + move, MinDepth, MaxDepth);
        if (currentDepth == MinDepth)
        {
            MarkCompleted();
        }
        Debug.LogError(currentDepth);

        lastRotation = controller.rotation;
    }

    private void OnEnable()
    {
        ScrewManager.Instance.Register(this);
    }


    private void OnDisable()
    {
        ScrewManager.Instance.Unregister(this);
    }
    #endregion

    #region 私有函数/业务逻辑

    private void MarkCompleted()
    {
        IsCompleted = true;
        isInteractable = false;
        Highlighter highlighter = GetComponent<Highlighter>();
        if (highlighter != null)
        {
            highlighter.constant = false;
            highlighter.tween = false;
        }
        ScrewManager.Instance.NotifyScrewCompleted();
    }

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

    void UpdateSnapDriver()
    {
        // 平滑位置
        snapDriver.position = Vector3.Lerp(
            snapDriver.position,
            SnapTarget.position,
            Time.deltaTime * SnapMoveSpeed
        );

        // 计算目标旋转
        //Quaternion targetRot = Quaternion.LookRotation(ToolForwardAxis, ToolUpAxis);

        Quaternion targetRot =
                   SnapTarget.rotation * ToolRotationOffset;

        snapDriver.rotation = Quaternion.Slerp(
            snapDriver.rotation,
            targetRot,
            Time.deltaTime * SnapRotateSpeed
        );

        // 判断是否到位
        float posError = Vector3.Distance(
            snapDriver.position,
            SnapTarget.position
        );

        float rotError = Quaternion.Angle(
            snapDriver.rotation,
            targetRot
        );

        if (posError < SnapPosThreshold &&
            rotError < SnapAngleThreshold)
        {
            SnapStatus = SnapState.Locked;
        }
    }


    /// <summary>
    /// 启动平滑吸附（不会立刻到位）
    /// </summary>
    private void SnapDriver(Transform driver)
    {
        snapDriver = driver;
        m_CurTool = driver;

        SnapStatus = SnapState.Snapping;
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 螺丝刀开始拧此螺丝
    /// </summary>
    public void BeginScrew(Transform ctrl, Transform tool)
    {
        if (!isInteractable) return;

        // 吸附上和正在吸附不执行
        if (SnapStatus != SnapState.None)
            return;

        // 当前工具类型不匹配 → 不能拧
        ToolType toolType = tool.GetComponent<AdsorptionTool>().CurrentToolType;
        if (toolType != RequiredToolType)
        {
            PlayHaptic(0.6f, 100f); // 错误工具反馈
            return;
        }

        controller = ctrl;
        lastRotation = controller.rotation;
        angleAccumulator = 0f;

        SnapDriver(tool);
    }

    /// <summary>
    /// 停止拧（松手 / 切换目标）
    /// </summary>
    public void EndScrew()
    {
        controller = null;
        SnapStatus = SnapState.None;
        snapDriver = null;
    }

    #endregion
}

/// <summary>
/// 吸附状态
/// </summary>
public enum SnapState
{
    None,        // 未吸附
    Snapping,   // 正在平滑吸附
    Locked      // 已吸附到位（允许拧）
}

public enum ScrewInitialState
{
    Installed,   // 已安装（未拧）
    Tightened    // 已拧紧
}