using HuaRuXR.FSM;
using HuaRuXR.Procedure;
using HuaRuXR.UI;
using ProcedureOwner = HuaRuXR.FSM.IFsm<HuaRuXR.Procedure.IProcedureManager>;

/// <summary>
/// 登陆
/// </summary>
public class ProcedureKMLogin : ProcedureBase
{
    /// <summary>
    /// 登陆UI ID
    /// </summary>
    private int loginPanelId;
    
    protected override void OnEnter(IFsm<IProcedureManager> fsm)
    {
        base.OnEnter(fsm);

        loginPanelId = UIEntry.UICom.OpenUIForm("Prefabs/UI/UILoginPanel", UIConfig.Content);
        UIForm uiForm = UIEntry.UICom.GetUIForm(loginPanelId);
        //uiForm.GetComponent<UILoginPanel>().LoginCallback = LoginCallBack;
    }

    /// <summary>
    /// 流程入口 流程 初始化 相当于MonoBehaviour的Awake()方法。
    /// *需要注意的是，所有勾选的流程会在程序启动时一同初始化,
    /// </summary>
    /// <param name="procedureOwner"></param>
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }
    
    protected override void OnLeave(IFsm<IProcedureManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    
    /// <summary>
    /// 登录成功回调
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="roleType"></param>
    /// <param name="userName"></param>
    //private void LoginCallBack(bool isSuccess, RoleType roleType, string userName)
    //{
    //    if (isSuccess)
    //    {
    //        UIEntry.UICom.CloseUIForm(loginPanelId);
    //        ProcedureEntry.ProcedureManager.ChangeProcedure("ProcedureKMMainPage");
    //        // UIEntry.UICom.OpenUIForm("Prefabs/UI/UIHJMainTitle",UIConfig.Content,null);
    //        // UIEntry.UICom.OpenUIForm("Prefabs/UI/UIPatternPanel", UIConfig.Content);
    //        // UIEntry.UICom.OpenUIForm("Prefabs/UI/UIBack", UIConfig.Background);
    //        // UIEntry.UICom.OpenUIForm("Prefabs/UI/UIMachineState", UIConfig.Dialog);
    
    //        // //默认读取项目
    //        // ProjectData projectData = JsonFullSerializer.LoadJsonFile<ProjectData>(VEMFacade.ProjectDataPath);
    //        // projectData.FullPath = VEMFacade.VEMSimConfigPath + "/" + projectData.FullPath;
    //        // PlatformFacade.Instance.SetProjectData(projectData);
    //        //
    //        // //初始化资源库 表现数据管理器
    //        ResourcesRepertoryDataManager.Instance.Init(VEMFacade.VEMSimPlatformPath);
    //        PerformDataManager.Instance.Init(VEMFacade.VEMSimPlatformPath);
    //    }
    //}
    
}