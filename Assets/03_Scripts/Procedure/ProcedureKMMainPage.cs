using HuaRuXR.FSM;
using HuaRuXR.Procedure;
using HuaRuXR.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 主页
/// </summary>
public class ProcedureKMMainPage : ProcedureBase
{
    
    protected override void OnEnter(IFsm<IProcedureManager> fsm)
    {
        base.OnEnter(fsm);
        //ModuleMgr.Instance.SceneMod.LoadScene("MainPageScene");
    }

    protected override void OnLeave(IFsm<IProcedureManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }
    
}