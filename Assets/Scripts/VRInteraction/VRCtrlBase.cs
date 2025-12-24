using HTC.UnityPlugin.Vive;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.15
//备    注:vr菜单界面基础类
//========================================
public class VRCtrlBase : MonoBehaviour
{
    #region 私有变量
    

    #endregion

    #region 公共变量
	
	

    #endregion

    #region Mono相关
	
	private void Start()
	{
		
	}

    protected virtual void Update()
    {
	    if (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.BKeyTouch) || ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Menu))
	    {
		    //菜单面板
		    Debug.Log("左手 Y 键按下");
            ShowHideMenu();

        }
	    if (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.AKeyTouch))
	    {
		    //关闭菜单面板
		    Debug.Log("左手 X 键按下");
	    }
	    
	    if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.BKeyTouch) || ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Menu))
	    {
		    //工具放下 显示手势
		    Debug.Log("右手 B 键按下");
            DropDownTool();
        }
	    if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.AKeyTouch))
	    {
		    Debug.Log("右手 A 键按下");
	    }
        if (ViveInput.GetTriggerValue(HandRole.RightHand) > 0.1f)
        {
            //按住右手扳机
            HoldRightTrigger();
        }
        else
        {
            //工具放下 显示手势
            Debug.Log("右手 Trigger 键释放");
            ReleaseRightTrigger();
        }
        if (ViveInput.GetTriggerValue(HandRole.LeftHand) > 0.1f)
        {
            //按住左手扳机
            HoldLeftTrigger();
        }
        else
        {
            //工具放下 显示手势
            Debug.Log("左手 Trigger 键释放");
            ReleaseLeftTrigger();
        }
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            //右手 Trigger 键按下瞬间
            PressDownRightTrigger();
        }
    }

    #endregion

    #region 私有函数/业务逻辑


    /// <summary>
    /// 显示隐藏菜单界面
    /// </summary>
    protected virtual void ShowHideMenu()
    {
    }

    /// <summary>
    /// 显示隐藏UI界面
    /// </summary>
    protected virtual void ShowHideUI(Transform UITrans, string handCanvasName, Vector3 localPosition, Vector3 localRotation)
    {
        bool isShowTool = UITrans.gameObject.activeSelf;
        isShowTool = !isShowTool;
        UITrans.gameObject.SetActive(isShowTool);
        if (isShowTool)
        {
            if (GameObject.Find(handCanvasName).transform != null)
            {
                Transform canvasTrans = GameObject.Find(handCanvasName).transform;
                UITrans.SetParent(canvasTrans);
                UITrans.localPosition = localPosition;
                UITrans.localEulerAngles = localRotation;
                UITrans.localScale = Vector3.one;
            }
        }
    }

    /// <summary>
    /// 按住右手扳机
    /// </summary>
    protected virtual void HoldRightTrigger()
    {

    }

    /// <summary>
    /// 按住左手扳机
    /// </summary>
    protected virtual void HoldLeftTrigger()
    {

    }


    /// <summary>
    /// 释放右手扳机  
    /// </summary>
    protected virtual void ReleaseRightTrigger()
    {

    }

    /// <summary>
    /// 释放左手扳机
    /// </summary>
    protected virtual void ReleaseLeftTrigger()
    {

    }

    /// <summary>
    /// 工具放下
    /// </summary>
    protected virtual void DropDownTool()
    { 
    
    }

    /// <summary>
    /// 右手Trigger按下瞬间
    /// </summary>
    protected virtual void PressDownRightTrigger()
    {

    }

    #endregion

    #region 公共函数/业务逻辑

    #endregion
}
