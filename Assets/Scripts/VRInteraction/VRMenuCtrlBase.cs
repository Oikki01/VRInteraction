using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.15
//备    注:vr菜单界面基础类
//========================================
public class VRMenuCtrlBase : MonoBehaviour
{
    #region 私有变量
    

    #endregion

    #region 公共变量
	
	

    #endregion

    #region Mono相关
	
	private void Start()
	{
		
	}

    private void Update()
    {
	    if (ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.BKeyTouch))
	    {
		    //菜单面板
		    Debug.Log("左手 Y 键按下");
	    }
	    if (ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.AKeyTouch))
	    {
		    //菜单面板返回
		    Debug.Log("左手 X 键按下");
	    }
	    
	    if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.BKeyTouch))
	    {
		    //工具放下 显示手势
		    Debug.Log("右手 B 键按下");
	    }
	    if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.AKeyTouch))
	    {
		    Debug.Log("右手 A 键按下");
	    }
    }

    #endregion

    #region 私有函数/业务逻辑



    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 菜单按钮按下
    /// </summary>
    protected virtual void MenuKeyOnClick()
    {
    }

    /// <summary>
    /// 界面回退
    /// </summary>
    protected virtual void InterfaceBackOnClick()
    {
	    
    }

    #endregion
}
