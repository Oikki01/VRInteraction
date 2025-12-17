using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class VRPaintBrushManager : MonoBehaviour
{
    #region 私有变量

    //右手Trigger是否按下
    private bool RightTrigger;

    //左右Trigger是否按下
    private bool LeftTrigger;

    #endregion

    #region 公共变量
	
	

    #endregion

    #region Mono相关
	
	private void Start()
	{
		
	}

    private void Update()
    {

    }

    protected virtual void LateUpdate()
    {
        UpdateTools();
    }
    #endregion

    #region 私有函数/业务逻辑

    private void UpdateTools()
    { 
        
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 设置左手Trigger是否按下
    /// </summary>
    /// <param name="value">true 撤销上一次绘制的图案</param>
    public void SetLeftTrigger(bool value)
    { 
        LeftTrigger = value;
    }

    /// <summary>
    /// 设置右手Trigger是否按下
    /// </summary>
    /// <param name="value">true 开始绘制图案</param>
    public void SetRightTrigger(bool value)
    {
        LeftTrigger = value;
    }

    #endregion
}
