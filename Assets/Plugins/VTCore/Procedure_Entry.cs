using System.Collections.Generic;
using ProcedureOwner = HuaRuXR.FSM.IFsm<HuaRuXR.Procedure.IProcedureManager>;
using UnityEngine;
using HuaRuXR.Procedure;
using System.IO;

/// <summary>
/// 流程入口
/// </summary>
public class Procedure_Entry : ProcedureBase
{
    /// <summary>
    /// 是否加载UI资源
    /// </summary>
    public bool isLoadUIAssets;

    /// <summary>
    /// 当前流程可打开的界面及可切换的流程，在BattleSim/UI/UIConfig.config中配置
    /// </summary>
    Dictionary<string, CssSettingData> CssSettingDataDic = new Dictionary<string, CssSettingData>();

    /// <summary>
    /// 流程入口 流程 初始化 相当于MonoBehaviour的Awake()方法。
    /// *需要注意的是，所有勾选的流程会在程序启动时一同初始化,
    /// </summary>
    /// <param name="procedureOwner"></param>
    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
        UIEntry.UICom.AddUIGroup(UIConfig.Background);
        UIEntry.UICom.AddUIGroup(UIConfig.Content);
        UIEntry.UICom.AddUIGroup(UIConfig.Control);
        UIEntry.UICom.AddUIGroup(UIConfig.Dialog);
        CssSettingDataDic = CoreEntry.BaseComponent.GetUIDic(this.ToString());
    }

    /// <summary>
    /// 进入 流程入口 流程 想定于相当于MonoBehaviour的Start()方法，进入流程时调用
    /// </summary>
    /// <param name="procedureOwner"></param>
    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        isLoadUIAssets = false;
    }

    /// <summary>
    /// 流程入口 流程 OnUpdate()方法，相当于MonoBehaviour的Update()方法
    /// </summary>
    /// <param name="procedureOwner"></param>
    /// <param name="elapseSeconds"></param>
    /// <param name="realElapseSeconds"></param>
    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (isLoadUIAssets)
        {
            return;
        }

        if (ResourceEntry.Resources != null && ResourceEntry.Resources.SimPathResource != null)
        {
            if (ResourceEntry.Resources.SimPathResource.IsReady) //等框架资源加载完成
            {
                if ( /*!CoreEntry.BaseComponent.EditorResourceMode&&*/File.Exists(Application.streamingAssetsPath + @"/BSimCoreVersion.dat"))
                {
                    //初始化UI资源
                    ResourceEntry.Resources.InitResources(Application.streamingAssetsPath + @"/BSimCoreVersion.dat", OnInitResourcesComplete);
                }
                else
                {
                    OnInitResourcesComplete();
                }

                isLoadUIAssets = true;
            }
        }
    }

    /// <summary>
    ///  离开 流程入口 流程 离开流程，相当于MonoBehaviour的OnDestory()
    /// </summary>
    /// <param name="procedureOwner"></param>
    /// <param name="isShutdown"></param>
    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }

    /// <summary>
    /// 销毁 流程入口 流程 销毁流程，相当于MonoBehaviour的OnDestory()
    /// </summary>
    /// <param name="procedureOwner"></param>
    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    /// <summary>
    /// UI资源初始化完成回调
    /// </summary>
    private void OnInitResourcesComplete()
    {
        Dictionary<string, CssSettingData> dic = CoreEntry.BaseComponent.GetUIDic(ToString());
        foreach (var item in dic.Values)
        {
            if (item.IsEnable && item.UIType == "Procedure")
            {
                ProcedureEntry.ProcedureManager.ChangeProcedure(item.UIName);
            }
        }
    }
}

/// <summary>
/// UI基础参数
/// </summary>
public class UIConfig
{
    #region 层级

    /// <summary>
    /// 背景层
    /// </summary>
    public const string Background = "Background";

    /// <summary>
    /// 内容层
    /// </summary>
    public const string Content = "Content";

    /// <summary>
    /// 全局控制层
    /// </summary>
    public const string Control = "Control";

    /// <summary>
    /// 临时对话层
    /// </summary>
    public const string Dialog = "Dialog";

    #endregion

    #region BattleSim下的相对路径

    /// <summary>
    /// BattleSim/UI下的排行图片文件
    /// </summary>
    public const string RankingList = "/UI/Texture/RankingList";

    /// <summary>
    /// BattleSim/UI下的人物头像图片文件
    /// </summary>
    public const string PersonIcon = "/UI/Texture/PersonIcon";

    #endregion

    #region 通用界面预制路径

    /// <summary> 
    /// 通用信息提示界面  Dialog
    /// </summary>
    public static readonly string InfoTipViewPath = "Assets/SelfModules/GameModules/Common/InfoTipView/Form/InfoTipView.prefab";

    /// <summary>
    /// 个人信息界面 content
    /// </summary>
    public static readonly string SelfInfoViewPath = "Assets/SelfModules/GameModules/Common/SelfInfoView/Form/SelfInfoView.prefab";

    /// <summary>
    ///  标题栏 全局控制层 control
    /// </summary>
    public static readonly string TitleBarPanelPath = "Assets/SelfModules/GameModules/Common/TitleBarPanel/Form/TitleBarPanel.prefab";

    #endregion
}