using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class ScrewManager : Singleton<ScrewManager>
{
    private readonly List<ScrewInteractable> screws =
        new List<ScrewInteractable>();

    // ---------------- 注册 ----------------
    public void Register(ScrewInteractable screw)
    {
        if (!screws.Contains(screw))
            screws.Add(screw);
    }

    public void Unregister(ScrewInteractable screw)
    {
        screws.Remove(screw);
    }

    // ---------------- 查询 ----------------
    public bool AreAllCompleted()
    {
        if (screws.Count == 0)
            return false;

        foreach (var screw in screws)
        {
            if (!screw.IsCompleted)
                return false;
        }
        return true;
    }

    // ---------------- 通知 ----------------
    public void NotifyScrewCompleted()
    {
        if (AreAllCompleted())
        {
            Debug.Log("✅ 所有螺丝已完成！");
            // TODO: 触发下一步
            //拧紧螺丝完毕
            if (ProcessManager.Instance.FlowDataList[ProcessManager.Instance.CurStepIndex].StepId == GlobalManager.Instance.CurStepID)
            {
                ProcessManager.Instance.StepComplete();
            }
        }
    }


    public List<ScrewInteractable> GetAllScrewInteractable()
    { 
        return screws;
    }
}
