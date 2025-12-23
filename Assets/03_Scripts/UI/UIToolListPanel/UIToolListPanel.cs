using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToolListPanel : MonoBehaviour
{
    #region GameObject引用

    /// <summary>
    /// 滑动列表
    /// </summary>
    [SerializeField] private ScrollRect scrollView;

    /// <summary>
    /// 预制体
    /// </summary>
    [SerializeField] private UIToolItem toolItemPrefab;

    #endregion

    #region 私有函数

    /// <summary>
    /// 
    /// </summary>
    private List<UIToolItem> toolItemList = new List<UIToolItem>();


    private VRInteraction m_VRInteraction;

    #endregion

    #region Mono
    private void Start()
    {

    }
    #endregion


    #region 私有函数

    private void ClearScrollView()
    {
        for (int i = 0; i < toolItemList.Count; i++)
        {
            Destroy(toolItemList[i].gameObject);
        }

        toolItemList.Clear();
    }

    private void InitScrollView()
    {
        ClearScrollView();
        List<string> toolTypeList = DataManager.Instance.GetAllToolType();
        for (int i = 0; i < toolTypeList.Count; i++)
        {
            UIToolItem item = Instantiate(toolItemPrefab, scrollView.content);
            item.gameObject.SetActive(true);
            item.Init(toolTypeList[i], ToolItemOnClick);
            toolItemList.Add(item);
        }

        if (GameObject.Find("VRRig"))
        {
            m_VRInteraction = GameObject.Find("VRRig").GetComponent<VRInteraction>();
        }
    }

    /// <summary>
    /// 工具点击
    /// </summary>
    /// <param name="toolType"></param>
    private void ToolItemOnClick(string toolType)
    {
        Debug.Log("工具点击 == " + toolType);
        //TODO:HJK生成工具 并且当前手中有工具则不生成
        if (toolType == "徒手")
            m_VRInteraction.CreatePaintTool();
        if (toolType == "一字螺丝刀")
            m_VRInteraction.CreateAdsorptionTool();
        gameObject.SetActive(false);
    }

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        InitScrollView();
    }

    #endregion
}