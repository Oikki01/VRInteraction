using UnityEngine;

/// <summary>
/// 主场景
/// </summary>
public class MainPageScene : BaseScene
{
    /// <summary>
    /// 进入
    /// </summary>
    protected override void OnLoad()
    {
        base.OnLoad();
        Debug.Log("进入主场景");
        //判断登录的是学员还是教员
        if (VEMFacade.CurRole == Role.Teacher)
        {
            UIEntry.UICom.OpenUIForm("Prefabs/UI/UITeacherManagerPanel", UIConfig.Content);
        }
        else
        {
            UIEntry.UICom.OpenUIForm("Prefabs/UI/UITrainTypePanel", UIConfig.Content);
        }
    }

    /// <summary>
    /// 场景卸载
    /// </summary>
    protected override void OnDispose()
    {
        base.OnDispose();
        Debug.Log("离开主场景");
    }
}
