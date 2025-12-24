using HTC.UnityPlugin.Vive;
using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:HJK    
//创建时间:2025.12.22
//备    注:VR交互类
//========================================
public class VRInteraction : VRCtrlBase
{
    #region 私有变量

    /// <summary>
    /// 工具实例
    /// </summary>
    private GameObject m_ToolInstance;

    private AdsorptionTool m_AdsorptionTool;

    #endregion

    #region 公共变量

    public GameObject PaintTool;

    public GameObject InteractTool;

    public CwHitBetween VrHitBetween;

    //右手手柄
    public Transform RightDeviceTracker;

    public float RayDistance = 2f;
    public LayerMask DetectLayer;

    public GameObject RightHandModel;

    #endregion

    #region Mono相关

    private void Start()
    {
        m_AdsorptionTool = InteractTool.GetComponent<AdsorptionTool>();

    }

    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region 私有函数/业务逻辑

    protected override void DropDownTool()
    {
        if (GlobalManager.Instance.ToolInteractionType == ToolInteratType.Paint)
        {
            if (ProcessManager.Instance.FlowDataList[ProcessManager.Instance.CurStepIndex].StepId == GlobalManager.Instance.CurStepID)
            {
                ProcessManager.Instance.StepComplete();
            }
        }
        GlobalManager.Instance.ToolInteractionType = ToolInteratType.None;
        GlobalManager.Instance.OperationType = OperationType.None;
        //放下工具
        if (m_ToolInstance != null)
        {
            RightHandModel.SetActive(true);
            m_ToolInstance.SetActive(false);
        }
    }

    protected override void HoldRightTrigger()
    {
        if (GlobalManager.Instance.OperationType != OperationType.Tool)
            return;
        switch (GlobalManager.Instance.ToolInteractionType)
        {
            case ToolInteratType.Paint:
                VrHitBetween.CurPaintState = CwHitBetween.PaintState.Painting;
                break;
            case ToolInteratType.Interact:
                m_AdsorptionTool.OnGrab(RightDeviceTracker);
                break;
        }
    }


    protected override void ReleaseRightTrigger()
    {
        if (GlobalManager.Instance.OperationType != OperationType.Tool)
            return;
        switch (GlobalManager.Instance.ToolInteractionType)
        {
            case ToolInteratType.Paint:
                VrHitBetween.CurPaintState = CwHitBetween.PaintState.None;
                break;
            case ToolInteratType.Interact:
                m_AdsorptionTool.OnRelease();
                break;
        }
    }

    protected override void PressDownRightTrigger()
    {

    }
    #endregion

    #region 公共函数/业务逻辑

    public void CreateAdsorptionTool()
    {
        DropDownTool();
        GlobalManager.Instance.OperationType = OperationType.Tool;
        GlobalManager.Instance.ToolInteractionType = ToolInteratType.Interact;
        m_ToolInstance = InteractTool;
        InteractTool.SetActive(true);
        RightHandModel.SetActive(false);
    }

    public void CreatePaintTool()
    {
        DropDownTool();
        GlobalManager.Instance.OperationType = OperationType.Tool;
        GlobalManager.Instance.ToolInteractionType = ToolInteratType.Paint;
        m_ToolInstance = PaintTool;
        PaintTool.SetActive(true);
        RightHandModel.SetActive(false);
    }

    #endregion
}
