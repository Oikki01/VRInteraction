using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.22
//备    注:VR菜单控制器
//========================================
public class VRMenuCtrl : VRCtrlBase
{
    #region 私有变量

    /// <summary>
    /// VR Canvas
    /// </summary>
    [SerializeField] private Canvas VRCanvas;

    /// <summary>
    /// VR 菜单
    /// </summary>
    [Header("生成预制体相关")]
    [SerializeField] private VRUIMenu m_VRUIMenuPrefab;

    /// <summary>
    /// 工具列表
    /// </summary>
    [SerializeField] private UIToolListPanel m_ToolListPanelPrefab;

    /// <summary>
    /// 步骤列表
    /// </summary>
    [SerializeField] private UIStepListPanel m_StepListPanelPrefab;

    /// <summary>
    /// 按钮名称字典
    /// </summary>
    [Header("生成按钮相关")]
    public Dictionary<MenuBtnType, string> m_BtnNameDict;

    /// <summary>
    /// 按钮位置字典
    /// </summary>
    public Dictionary<string, Vector3> m_BtnPosDict;

    /// <summary>
    /// 菜单实例
    /// </summary>
    private VRUIMenu m_VRUIMenuInstance;

    /// <summary>
    /// 工具列表实例
    /// </summary>
    private UIToolListPanel m_ToolListPanelInstance;

    /// <summary>
    /// 步骤列表实例
    /// </summary>
    private UIStepListPanel m_SetpListPanelInstance;


    private ProcessManager processManager;

    #endregion

    #region 公共变量



    #endregion

    #region Mono相关

    private void Awake()
    {
        GlobalManager.Instance.OperationType = OperationType.None;


        m_BtnNameDict = new Dictionary<MenuBtnType, string>();
        m_BtnNameDict.Add(MenuBtnType.BtnToolList, "工具箱");
        m_BtnNameDict.Add(MenuBtnType.BtnStepList, "步骤列表");
        m_BtnNameDict.Add(MenuBtnType.BtnExit, "退出");

        m_BtnPosDict = new Dictionary<string, Vector3>();
        m_BtnPosDict.Add("工具箱", new Vector3(-147, 0, 0));
        m_BtnPosDict.Add("步骤列表", new Vector3(-147, -110, 0));
        m_BtnPosDict.Add("退出", new Vector3(-147, -220, 0));

        CreateVRUIMenu();
        CreateToolListPanel();
        CreateStepListPanel();

        m_VRUIMenuInstance.SetOnClickCallBack(m_BtnNameDict, OnClickCallBack, m_BtnPosDict);
    }

    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region 私有函数/业务逻辑

    /// <summary>
    /// 创建菜单
    /// </summary>
    private void CreateVRUIMenu()
    {
        m_VRUIMenuInstance = Instantiate(m_VRUIMenuPrefab, VRCanvas.transform);
        m_VRUIMenuInstance.gameObject.SetActive(false);
    }

    //显示隐藏菜单
    protected override void ShowHideMenu()
    {
        ShowHideUI(m_VRUIMenuInstance.transform, "LeftCanvas", new Vector3(-6, 91, 134), new Vector3(62, 0, 0));
        m_ToolListPanelInstance.gameObject.SetActive(false);
    }

    private void CreateToolListPanel()
    {
        //初始化工具数据
        DataManager.Instance.Init(VEMFacade.VEMSimPlatformPath);
        m_ToolListPanelInstance = Instantiate(m_ToolListPanelPrefab, VRCanvas.transform);
        m_ToolListPanelInstance.Init();
        m_ToolListPanelInstance.gameObject.SetActive(false);
    }

    private void CreateStepListPanel()
    {
        //初始化步骤列表数据

        //流程管理器
        VEMFacade.CurTrainType = TrainType.Teach;
        GameObject processManagerGobj = new GameObject("ProcessManager");
        processManagerGobj.name = "ProcessManager";
        processManager = processManagerGobj.AddComponent<ProcessManager>();
        QuoteResourcesManager quoteResourcesManager = processManagerGobj.AddComponent<QuoteResourcesManager>();
        GameObject modelScreenPrefab = Resources.Load<GameObject>("Prefabs/ModelScreenCanvas");
        VEMFacade.CurSubjectFolder = VEMFacade.VEMSimPlatformPath + "/Subject";
        quoteResourcesManager.InitData(VEMFacade.CurSubjectFolder, modelScreenPrefab);
        processManager.Init(VEMFacade.CurSubjectFolder);

        m_SetpListPanelInstance = Instantiate(m_StepListPanelPrefab, VRCanvas.transform);
        m_SetpListPanelInstance.InitScrollView(ProcessManager.Instance.FlowDataList);
        m_SetpListPanelInstance.gameObject.SetActive(false);
    }

    private void ShowHideToolListPanel()
    {
        ShowHideUI(m_ToolListPanelInstance.transform, "LeftCanvas", new Vector3(-6, 91, 134), new Vector3(62, 0, 0));
    }

    private void ShowHideStepListPanel()
    {
        ShowHideUI(m_SetpListPanelInstance.transform, "LeftCanvas", new Vector3(-6, 91, 134), new Vector3(62, 0, 0));
    }

    private void OnClickCallBack(MenuBtnType menuBtnType)
    {
        switch (menuBtnType)
        {
            case MenuBtnType.BtnStepList:
                ShowHideStepListPanel();
                break;
            case MenuBtnType.BtnToolList:
                ShowHideToolListPanel();
                break;
            case MenuBtnType.BtnExit:
                Application.Quit();
                break;
        }
    }

    #endregion

    #region 公共函数/业务逻辑



    #endregion
}
