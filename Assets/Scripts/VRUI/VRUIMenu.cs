using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.22
//备    注:VR菜单
//========================================
public class VRUIMenu : MonoBehaviour
{
    #region 私有变量

    [SerializeField] private VRUIMenuBtn m_BtnPrefab;

    [SerializeField] private Transform m_BtnPanel;

    private Action<MenuBtnType> m_OnClickAction;
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

    #endregion

    #region 私有函数/业务逻辑

    /// <summary>
    /// 按钮点击回调
    /// </summary>
    /// <param name="menuBtnType"></param>
    private void BtnOnClick(MenuBtnType menuBtnType)
    { 
        m_OnClickAction?.Invoke(menuBtnType);
        gameObject.SetActive(false);
    }

    #endregion

    #region 公共函数/业务逻辑

    /// <summary>
    /// 设置按钮点击回调
    /// </summary>
    /// <param name="btnNameDict"></param>
    /// <param name="onClickAction"></param>
    /// <param name="btnPosDict"></param>
    public void SetOnClickCallBack(Dictionary<MenuBtnType, string> btnNameDict, Action<MenuBtnType> onClickAction, Dictionary<string, Vector3> btnPosDict)
    { 
        this.m_OnClickAction = onClickAction;

        foreach (KeyValuePair<MenuBtnType, string> item in btnNameDict)
        {
            VRUIMenuBtn btn = Instantiate(m_BtnPrefab, m_BtnPanel);
            btn.gameObject.SetActive(true);
            btn.transform.localPosition = btnPosDict[item.Value];
            btn.Init(item.Key, item.Value, BtnOnClick);
        }
    }

    #endregion
}
