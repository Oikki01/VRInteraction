using System.Xml;
using HuaRuXR.FSM;
using HuaRuXR.Procedure;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using ProcedureOwner = HuaRuXR.FSM.IFsm<HuaRuXR.Procedure.IProcedureManager>;

/// <summary>
/// 启动流程
/// </summary>
public class ProcedureKMLaunch : ProcedureBase
{
    protected override void OnEnter(IFsm<IProcedureManager> fsm)
    {
        base.OnEnter(fsm);
        XRSettings.enabled = false;
        
        //TrilibLoad trilibLoad = new TrilibLoad();
        //ModuleMgr.Instance.ResourcesMod.InitTrilibLoadClass(trilibLoad);
        
       // ProcedureEntry.ProcedureManager.ChangeProcedure("ProcedureKMLogin");
    }

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
    }
    
    protected override void OnLeave(IFsm<IProcedureManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    
}