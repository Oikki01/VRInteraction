using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//========================================
//作    者:HJK
//创建时间:2025.12.22
//备    注:VR菜单按钮
//========================================
public class VRUIMenuBtn : MonoBehaviour
{
    #region 私有变量

    [SerializeField] private Button m_Btn;
    [SerializeField] private Image m_BgImg;
    [SerializeField] private Text m_Text;

    private Action<MenuBtnType> m_ClickAction;
    private MenuBtnType m_MuneBtnType;
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

    private void OnEnable()
    {
        m_Btn.onClick.AddListener(BtnOnClick);
    }

    private void OnDisable()
    {
        m_Btn.onClick.RemoveListener(BtnOnClick);
    }

    #endregion

    #region 私有函数/业务逻辑

    private void BtnOnClick()
    { 
        m_ClickAction?.Invoke(m_MuneBtnType);
    }

    #endregion

    #region 公共函数/业务逻辑

    public void Init(MenuBtnType btnType, string btnText, Action<MenuBtnType> clickAction)
    {
        m_MuneBtnType = btnType;
        m_Text.text = btnText;
        m_ClickAction = clickAction;

        Texture2D loadedSprite = Resources.Load<Texture2D>("Texture\\VrUIMenu\\" + btnText);
        if (loadedSprite != null)
        {
            m_BgImg.sprite = Sprite.Create(loadedSprite, new Rect(0, 0, loadedSprite.width, loadedSprite.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.Log("无法加载图片 : " + "Texture\\VrUIMenu\\" + btnText);
        }
    }

    #endregion
}
