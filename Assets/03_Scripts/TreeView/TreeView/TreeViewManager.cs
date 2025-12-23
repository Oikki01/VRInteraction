using System.Collections.Generic;
using TreeListView;
using UnityEngine;
using UnityEngine.Events;


namespace VEMSimTreeView
{
    public class TreeViewManager : MonoBehaviour
    {
        #region GameObject引用

        [SerializeField] private TreeView mTreeView;

        #endregion

        #region 子元素事件

        //子元素点击事件
        public class ItemClickedEvent<T> : UnityEvent<T>
        {
        }

        /// <summary>
        /// 树形列表点击事件
        /// </summary>
        public ItemClickedEvent<TreeViewItem> TreeListItemOnClick = new ItemClickedEvent<TreeViewItem>();

        #endregion

        #region 私有变量

        /// <summary>
        /// 当前选中的项 id
        /// </summary>
        int mCurSelectedItemId = 0;

        /// <summary>
        /// UI 字典
        /// </summary>
        private Dictionary<int, TreeViewItem> mTreeViewItemDict;

        #endregion

        void Awake()
        {
            mTreeViewItemDict = new Dictionary<int, TreeViewItem>();

            mTreeView.OnTreeListAddOneItem = OnTreeListAddOneItem;
            mTreeView.OnTreeListDeleteOneItem = OnTreeListDeleteOneItem;
            mTreeView.OnItemExpandBegin = OnItemExpandBegin;
            mTreeView.OnItemCollapseBegin = OnItemCollapseBegin;
            mTreeView.OnItemCustomEvent = OnItemCustomEvent;
            mTreeView.InitView();
        }

        void OnTreeListAddOneItem(TreeList treeList)
        {
            int count = treeList.ItemCount;
            TreeViewItem parentTreeItem = treeList.ParentTreeItem;
            if (count > 0 && parentTreeItem != null)
            {
                TreeDataItem st = parentTreeItem.GetComponent<TreeDataItem>();
                st.SetExpandBtnVisible(true);
                st.SetExpandStatus(parentTreeItem.IsExpand);
            }
        }

        void OnTreeListDeleteOneItem(TreeList treeList)
        {
            int count = treeList.ItemCount;
            TreeViewItem parentTreeItem = treeList.ParentTreeItem;
            if (count == 0 && parentTreeItem != null)
            {
                TreeDataItem st = parentTreeItem.GetComponent<TreeDataItem>();
                st.SetExpandBtnVisible(false);
            }
        }

        void OnItemExpandBegin(TreeViewItem item)
        {
            TreeDataItem st = item.GetComponent<TreeDataItem>();
            st.SetExpandStatus(true);
        }

        void OnItemCollapseBegin(TreeViewItem item)
        {
            TreeDataItem st = item.GetComponent<TreeDataItem>();
            st.SetExpandStatus(false);
        }

        void OnItemCustomEvent(TreeViewItem item, CustomEvent customEvent, System.Object param)
        {
            if (customEvent == CustomEvent.ItemClicked)
            {
                TreeDataItem st = item.GetComponent<TreeDataItem>();
                if (mCurSelectedItemId != item.ItemId)
                {
                    //关闭当前选中的项
                    TreeViewItem curItem = GetTreeItemById(mCurSelectedItemId);
                    if (curItem != null && curItem.GetComponent<TreeDataItem>().IsSelected)
                    {
                        curItem.GetComponent<TreeDataItem>().IsSelected = false;
                    }

                    mCurSelectedItemId = item.ItemId;
                    st.IsSelected = true;
                }
                else
                {
                    st.IsSelected = false;
                    mCurSelectedItemId = 0;
                }

                // Debug.Log("实现单击" + mCurSelectedItemId);
                TreeListItemOnClick.Invoke(item);
            }
        }

        TreeViewItem CurSelectedItem
        {
            get
            {
                if (mCurSelectedItemId <= 0)
                {
                    return null;
                }

                TreeViewItem item = GetTreeItemById(mCurSelectedItemId);
                if (item == null)
                {
                    mCurSelectedItemId = 0;
                    return null;
                }

                return item;
            }
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        public void Clear()
        {
            //删除所有
            mTreeView.Clear();
            mTreeViewItemDict = new Dictionary<int, TreeViewItem>();
            mCurSelectedItemId = 0;
        }

        public int SetData(List<TreeItemData> dateList)
        {
            Clear();
            int curId = 0;
            for (int i = 0; i < dateList.Count; i++)
            {
                mCurSelectedItemId = dateList[i].ParentId;
                CreateItem(dateList[i]);

                if (curId < dateList[i].Id)
                {
                    curId = dateList[i].Id;
                }
            }

            mCurSelectedItemId = 0;
            return curId;
        }

        /// <summary>
        /// 返回树形列表数据
        /// </summary>
        /// <returns></returns>
        public List<TreeItemData> GetTreeItemDataList()
        {
            List<TreeItemData> treeItemDataList = new List<TreeItemData>();

            foreach (var item in mTreeViewItemDict)
            {
                treeItemDataList.Add(item.Value.UserData as TreeItemData);
            }

            return treeItemDataList;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public TreeItemData GetTreeItemData(int itemId)
        {
            if (mTreeViewItemDict.ContainsKey(itemId))
            {
                TreeViewItem item = mTreeViewItemDict[itemId];
                return item.UserData as TreeItemData;
            }

            return null;
        }

        /// <summary>
        /// 根据父节点Id 获取子节点数据列表 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<TreeItemData> GetTreeItemDataByParentId(int parentId)
        {
            List<TreeItemData> itemDataList = new List<TreeItemData>();
            foreach (var item in mTreeViewItemDict)
            {
                TreeItemData data = item.Value.UserData as TreeItemData;
                if (data.ParentId == parentId)
                {
                    itemDataList.Add(data);
                }
            }

            return itemDataList;
        }

        public void OpenItem(string itemName)
        {
            TreeViewItem item = GetTreeItemByName(itemName);
            if (item != null)
            {
                item.GetComponent<TreeDataItem>().OnItemClicked();
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void CreateItem(TreeItemData itemData)
        {
            TreeViewItem item = CurSelectedItem;
            if (item == null)
            {
                TreeViewItem childItem = mTreeView.AppendItem("ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("", itemData.TextStr);
                childItem.UserData = itemData;
                childItem.ItemId = itemData.Id;
                TreeViewItemDictAdd(itemData.Id, childItem);
            }
            else
            {
                item.Expand();
                TreeViewItem childItem = item.ChildTree.AppendItem("ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("", itemData.TextStr);
                childItem.UserData = itemData;
                childItem.ItemId = itemData.Id;
                TreeViewItemDictAdd(itemData.Id, childItem);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteItem(TreeViewItem item)
        {
            if (item == null)
            {
                Debug.Log("Please Select a Item First");
                return;
            }

            DeleteChildItem(item);
            item.ParentTreeList.DeleteItem(item);
        }

        public void DeleteChildItem(TreeViewItem item)
        {
            TreeViewItemDictDelete(item.ItemId);
            for (int i = 0; i < item.ChildTree.ItemCount; i++)
            {
                DeleteChildItem(item.ChildTree.GetItemByIndex(i));
            }
        }

        /// <summary>
        /// 通过id获得项
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public TreeViewItem GetTreeItemById(int itemId)
        {
            TreeViewItem item = null;
            if (mTreeViewItemDict.TryGetValue(itemId, out item))
            {
                return item;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void TreeViewItemClick(TreeViewItem item)
        {
            mTreeView.OnItemCustomEvent.Invoke(item, CustomEvent.ItemClicked, null);
        }

        /// <summary>
        /// 折叠所有
        /// </summary>
        public void CollapseAll()
        {
            foreach (var item in mTreeViewItemDict)
            {
                item.Value.CollapseAll();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnItemSelect(int itemId)
        {
            TreeViewItem item = GetTreeViewItem(itemId);
            TreeItemData itemData = item.UserData as TreeItemData;
            if (itemData.Id == itemId)
            {
                ParentExpand(itemData.ParentId);
            }

            item.GetComponent<TreeDataItem>().OnItemClicked();

            //计算高度
            int hightIndex = 0;
            float itemHight = item.GetComponent<RectTransform>().sizeDelta.y + mTreeView.ItemPadding;
            CalHightIndex(itemId, ref hightIndex);
            Vector2 pos = mTreeView.GetComponent<RectTransform>().anchoredPosition;
            pos.y = hightIndex * itemHight;
            mTreeView.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        private void CalHightIndex(int itemId, ref int hightIndex)
        {
            TreeViewItem item = GetTreeViewItem(itemId);
            hightIndex += item.transform.GetSiblingIndex();
            TreeItemData itemData = item.UserData as TreeItemData;
            if (itemData.ParentId != -1)
            {
                hightIndex++;
                CalHightIndex(itemData.ParentId, ref hightIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ParentExpand(int itemId)
        {
            if (itemId == -1)
            {
                return;
            }

            TreeViewItem item = GetTreeViewItem(itemId);
            item.Expand();
            TreeItemData itemData = item.UserData as TreeItemData;
            ParentExpand(itemData.ParentId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private TreeViewItem GetTreeViewItem(int itemId)
        {
            foreach (var item in mTreeViewItemDict)
            {
                TreeItemData itemData = item.Value.UserData as TreeItemData;
                if (itemData.Id == itemId)
                {
                    return item.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过名称获得项
        /// </summary>
        /// <param name="itemTitle"></param>
        /// <returns></returns>
        private TreeViewItem GetTreeItemByName(string itemTitle)
        {
            TreeViewItem item = null;

            foreach (KeyValuePair<int, TreeViewItem> viewItem in mTreeViewItemDict)
            {
                TreeDataItem treeItem = viewItem.Value.GetComponent<TreeDataItem>();
                if (treeItem.mLabelText.text.Equals(itemTitle))
                {
                    return viewItem.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// 数据增加
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="item"></param>
        private void TreeViewItemDictAdd(int itemId, TreeViewItem item)
        {
            mTreeViewItemDict.Add(itemId, item);
        }

        /// <summary>
        ///  数据删除
        /// </summary>
        /// <param name="itemId"></param>
        private void TreeViewItemDictDelete(int itemId)
        {
            mTreeViewItemDict.Remove(itemId);
        }
    }
}