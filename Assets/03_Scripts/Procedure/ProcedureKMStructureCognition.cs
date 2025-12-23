using HuaRuXR.FSM;
using HuaRuXR.Procedure;

/// <summary>
/// 流程操作
/// </summary>
public class ProcedureKMStructureCognition : ProcedureBase
{
    
    protected override void OnEnter(IFsm<IProcedureManager> fsm)
    {
        base.OnEnter(fsm);
        //ModuleMgr.Instance.SceneMod.LoadScene("StructureCognitionScene");
    }

    protected override void OnLeave(IFsm<IProcedureManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        
    }
}