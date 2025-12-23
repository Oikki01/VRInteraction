using System;
using System.Collections.Generic;
using TreeListView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VEMSimTreeView;

/// <summary>
/// Author:lidongwei
/// Create Data:2022.02.24
/// Description:科目列表UI
/// </summary>
public class UISubjectTreeList : MonoBehaviour
{
    #region GameObject引用

    /// <summary>
    /// 搜索输入框
    /// </summary>
    [SerializeField] private InputField inputFieldSearch;

    /// <summary>
    /// 搜索关闭
    /// </summary>
    [SerializeField] private Button btnClose;

    /// <summary>
    /// 右键菜单检测是否点击在自身
    /// </summary>
    [SerializeField] private GameObject checkGobj;

    #endregion

    #region 公有变量

    /// <summary>
    /// 科目列表 管理器
    /// </summary>
    public TreeViewManager SubjectTreeView;

    #endregion

    #region 私有变量

    /// <summary>
    /// Canvas
    /// </summary>
    private Canvas canvas;

    /// <summary>
    /// 初始化完成
    /// </summary>
    private bool isInit = false;

    /// <summary>
    /// 当前选中的科目项
    /// </summary>
    private TreeViewItem curSelectSubjectItem;

    /// <summary>
    /// 数据备份
    /// </summary>
    private List<TreeItemData> dataListCopy;

    /// <summary>
    /// 正在搜索
    /// </summary>
    private bool isSearchIng;

    /// <summary>
    /// 子元素点击事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemEvent<T> : UnityEvent<T>
    {
    }

    /// <summary>
    /// 树形列表添加事件
    /// </summary>
    public ItemEvent<TreeViewItem> OnItemAdded = new ItemEvent<TreeViewItem>();

    // <summary>
    /// 树形列表删除事件
    /// </summary>
    public ItemEvent<List<int>> OnTreeViewItemDelete = new ItemEvent<List<int>>();

    #endregion

    #region Unity生命周期函数

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        inputFieldSearch.onValueChanged.AddListener(InputFieldSearchOnValueChanged);
        inputFieldSearch.onEndEdit.AddListener(InputFieldSearchOnEndEdit);
        btnClose.onClick.AddListener(BtnCloseOnClick);
        SubjectTreeView.TreeListItemOnClick.AddListener(OnSubjectItemClick);
    }

    private void OnDisable()
    {
        inputFieldSearch.onValueChanged.RemoveListener(InputFieldSearchOnValueChanged);
        inputFieldSearch.onEndEdit.RemoveListener(InputFieldSearchOnEndEdit);
        btnClose.onClick.RemoveListener(BtnCloseOnClick);
        SubjectTreeView.TreeListItemOnClick.RemoveListener(OnSubjectItemClick);
    }

    #endregion

    #region 私有函数

    /// <summary>
    /// 搜索输入框 点击
    /// </summary>
    public void InputFieldSearchOnSelect()
    {
        if (isSearchIng == false)
        {
            isSearchIng = true;
            dataListCopy = GetTreeItemDataList();
        }
    }

    /// <summary>
    /// 搜索输入框 失去焦点
    /// </summary>
    private void InputFieldSearchOnEndEdit(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            isSearchIng = false;
        }
    }

    /// <summary>
    /// 关闭搜索
    /// </summary>
    private void BtnCloseOnClick()
    {
        int selectItemId = -1;
        if (curSelectSubjectItem != null && curSelectSubjectItem.UserData != null)
        {
            TreeItemData itemData = curSelectSubjectItem.UserData as TreeItemData;
            selectItemId = itemData.Id;
        }

        inputFieldSearch.text = String.Empty;
        isSearchIng = false;
        if (selectItemId != -1)
        {
            SubjectTreeView.OnItemSelect(selectItemId);
        }
        else
        {
            curSelectSubjectItem = null;
        }
    }

    /// <summary>
    /// 搜索框输入
    /// </summary>
    /// <param name="text"></param>
    private void InputFieldSearchOnValueChanged(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            btnClose.gameObject.SetActive(false);

            if (dataListCopy != null)
            {
                InitData(dataListCopy);
            }
        }
        else
        {
            btnClose.gameObject.SetActive(true);

            List<TreeItemData> newDataList = new List<TreeItemData>();
            for (int i = 0; i < dataListCopy.Count; i++)
            {
                TreeItemData itemData = dataListCopy[i].Copy();
                if (itemData.TextStr.Contains(text))
                {
                    itemData.ParentId = -1;
                    newDataList.Add(itemData);
                }
            }

            InitData(newDataList);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="idList"></param>
    /// <returns></returns>
    private void GetItemIdInChildren(int itemId, ref List<int> idList)
    {
        idList.Add(itemId);
        List<TreeItemData> itemDataList = SubjectTreeView.GetTreeItemDataByParentId(itemId);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            GetItemIdInChildren(itemDataList[i].Id, ref idList);
        }
    }

    /// <summary>
    /// 科目列表点击回调
    /// </summary>
    /// <param name="item"></param>
    private void OnSubjectItemClick(TreeViewItem item)
    {
        if (item.GetComponent<TreeDataItem>().IsSelected)
        {
            curSelectSubjectItem = item;
        }
        else
        {
            curSelectSubjectItem = null;
        }
    }

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="filePath"></param>
    public void InitData(string filePath)
    {
        //载入科目数据 
        InitData(JsonFullSerializer.LoadJsonFile<List<TreeItemData>>(filePath));
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="subjectItemDataList"></param>
    public void InitData(List<TreeItemData> dataList)
    {
        SubjectTreeView.SetData(dataList);
        SubjectTreeView.CollapseAll();
        isInit = true;
    }

    /// <summary>
    /// 清理
    /// </summary>
    public void Clear()
    {
        isInit = false;
        SubjectTreeView.Clear();
    }

    /// <summary>
    /// 返回数据
    /// </summary>
    /// <returns></returns>
    public List<TreeItemData> GetTreeItemDataList()
    {
        return SubjectTreeView.GetTreeItemDataList();
    }

    /// <summary>
    /// 通过id 使某个item 处于点击状态
    /// </summary>
    public void ClickItemById(int id)
    {
        TreeViewItem item = SubjectTreeView.GetTreeItemById(id);
        if (item != null)
        {
            SubjectTreeView.TreeViewItemClick(item);
        }
    }

    #endregion
}
