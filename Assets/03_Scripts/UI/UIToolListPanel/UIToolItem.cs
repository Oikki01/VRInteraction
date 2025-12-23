using System;
using UnityEngine;
using UnityEngine.UI;

public class UIToolItem : MonoBehaviour
{
    #region GameObject引用

    /// <summary>
    /// 工具图片
    /// </summary>
    [SerializeField] private Image imgTool;

    /// <summary>
    /// 工具文本
    /// </summary>
    [SerializeField] private Text textTool;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Toggle toggle;

    #endregion

    #region 私有变量

    /// <summary>
    /// 工具类型
    /// </summary>
    private string toolType;

    /// <summary>
    /// 点击回调
    /// </summary>
    private Action<string> clickCallback;

    #endregion

    #region Mono

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(ToggleOnClick);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(ToggleOnClick);
    }

    #endregion

    #region 私有函数

    /// <summary>
    /// 点击
    /// </summary>
    private void ToggleOnClick(bool isOn)
    {
        if (isOn)
        {
            clickCallback?.Invoke(toolType);
        }
    }

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="toolType"></param>
    /// <param name="clickCallback"></param>
    public void Init(string toolType, Action<string> clickCallback)
    {
        this.toolType = toolType;
        this.clickCallback = clickCallback;

        ToolInfoData toolInfoData = DataManager.Instance.GetFirstToolByName(toolType);
        FileIOUtil.LoadSpriteByIO(VEMFacade.VEMSimPlatformPath + "/" + toolInfoData.ThumbnailImg, imgTool, 0, 0);
        textTool.text = toolType;
    }

    #endregion
}