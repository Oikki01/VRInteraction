using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 步骤列表
/// </summary>
public class UIStepListPanel : MonoBehaviour
{
    #region GameObject引用

    /// <summary>
    /// 滑动列表
    /// </summary>
    [SerializeField] private ScrollRect scrollView;

    /// <summary>
    /// 项目 预制体
    /// </summary>
    [SerializeField] private UIStepItem itemPrefab;

    #endregion

    #region 私有变量

    /// <summary>
    /// UI列表
    /// </summary>
    private List<UIStepItem> itemList = new List<UIStepItem>();

    #endregion

    #region Unity生命周期函数

    private void OnEnable()
    {
        GameEvent.GameEventCenter.OnTaskProgressChanged.AddListener(OnTaskProgressChanged);
    }

    private void OnDisable()
    {
        GameEvent.GameEventCenter.OnTaskProgressChanged.RemoveListener(OnTaskProgressChanged);
    }

    #endregion

    #region 私有函数

    /// <summary>
    /// 清空
    /// </summary>
    private void ClearScrollView()
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            Destroy(itemList[i].gameObject);
            itemList.RemoveAt(i);
        }
    }

    /// <summary>
    /// 进度变更
    /// </summary>
    private void OnTaskProgressChanged()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].UpdateStateBg();
        }
    }

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化列表数据
    /// </summary>
    /// <param name="flowDataBaseList"></param>
    public void InitScrollView(List<FlowDataBase> flowDataBaseList)
    {
        //初始化方法
        //stepListPanel.InitScrollView(ProcessManager.Instance.FlowDataList);

        ClearScrollView();
        for (int i = 0; i < flowDataBaseList.Count; i++)
        {
            UIStepItem item = Instantiate(itemPrefab, scrollView.content);
            item.gameObject.SetActive(true);
            item.Init(flowDataBaseList[i], i);
            itemList.Add(item);
        }
    }

    #endregion
}