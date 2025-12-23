//=================================
//创建作者：刘明坤
//创建时间：2021-12-20 9:47:20
//创建说明：树形结构测试脚本（主要测试自动布局、前插入、后插入、展开、收缩、增加子节点、删除节点、整体展开、整体收缩等功能）
//=================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace VEMSimTreeView
{
    public class DemoSceneScript : MonoBehaviour
    {
        public TreeView mTreeView;

        int mCurSelectedItemId = 0;

        int mNewItemCount = 0;

        void Start()
        {
            ResManager rm = ResManager.Instance;
            mTreeView.OnTreeListAddOneItem = OnTreeListAddOneItem;
            mTreeView.OnTreeListDeleteOneItem = OnTreeListDeleteOneItem;
            mTreeView.OnItemExpandBegin = OnItemExpandBegin;
            mTreeView.OnItemCollapseBegin = OnItemCollapseBegin;
            mTreeView.OnItemCustomEvent = OnItemCustomEvent;
            mTreeView.InitView();
            TreeViewItem item1 = mTreeView.AppendItem("ItemPrefab1");
            item1.GetComponent<TreeDataItem>().SetItemInfo("Home", "Home");
            

        }
            IEnumerator TreeCreate(List<Transform> listTrans)
    {
        yield return new WaitForSeconds(2);
        Debug.Log(listTrans.Count);
        foreach (var item in listTrans)
        {
            Debug.Log(item.name + "++++++++++++++++++++");
            TreeViewItem mTreeViewItem = mTreeView.AppendItem(item.name);
            Debug.Log(mTreeViewItem.transform.name);
            mTreeViewItem.GetComponent<TreeDataItem>().SetItemInfo("Home", item.name, "1");
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
                if (mCurSelectedItemId > 0)
                {
                    if (item.ItemId == mCurSelectedItemId)
                    {
                        return;
                    }
                    TreeViewItem curSelectedItem = mTreeView.GetTreeItemById(mCurSelectedItemId);
                    if (curSelectedItem != null)
                    {
                        curSelectedItem.GetComponent<TreeDataItem>().IsSelected = false;
                    }
                    mCurSelectedItemId = 0;
                }
                st.IsSelected = true;
                mCurSelectedItemId = item.ItemId;
                Debug.Log("实现单击" + mCurSelectedItemId);
            }
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

        TreeViewItem CurSelectedItem
        {
            get
            {
                if (mCurSelectedItemId <= 0)
                {
                    return null;
                }
                TreeViewItem item = mTreeView.GetTreeItemById(mCurSelectedItemId);
                if (item == null)
                {
                    mCurSelectedItemId = 0;
                    return null;
                }
                return item;
            }
        }

        public void OnExpandAllBtnClicked()
        {
            mTreeView.ExpandAllItem();
        }
        public void OnCollapseAllBtnClicked()
        {
            mTreeView.CollapseAllItem();
        }

        public void OnExpandBtnClicked()
        {
            TreeViewItem item = CurSelectedItem;
            if (item == null)
            {
                Debug.Log("Please Select a Item First");
                return;
            }
            item.Expand();
        }

        public void OnCollapseBtnClicked()
        {
            TreeViewItem item = CurSelectedItem;
            if (item == null)
            {
                Debug.Log("Please Select a Item First");
                return;
            }
            item.Collapse();
        }


        public void OnInsertBeforeBtnClicked()
        {
            mNewItemCount++;
            if (mTreeView.IsEmpty)
            {
                TreeViewItem childItem = mTreeView.InsertItem(0, "ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }
            else
            {
                TreeViewItem item = CurSelectedItem;
                if (item == null)
                {
                    Debug.Log("Please Select a Item First");
                    return;
                }
                TreeViewItem childItem = item.ParentTreeList.InsertItem(item.ItemIndex, "ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }

        }

        public void OnInsertAfterBtnClicked()
        {
            mNewItemCount++;
            if (mTreeView.IsEmpty)
            {
                TreeViewItem childItem = mTreeView.InsertItem(0, "ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }
            else
            {
                TreeViewItem item = CurSelectedItem;
                if (item == null)
                {
                    Debug.Log("Please Select a Item First");
                    return;
                }
                TreeViewItem childItem = item.ParentTreeList.InsertItem(item.ItemIndex + 1, "ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }

        }

        public void OnAddChildBtnClicked()
        {
            mNewItemCount++;
            if (mTreeView.IsEmpty)
            {
                TreeViewItem childItem = mTreeView.AppendItem("ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }
            else
            {
                TreeViewItem item = CurSelectedItem;
                if (item == null)
                {
                    Debug.Log("Please Select a Item First");
                    return;
                }
                TreeViewItem childItem = item.ChildTree.AppendItem("ItemPrefab1");
                childItem.GetComponent<TreeDataItem>().SetItemInfo("Movie", "Movie" + mNewItemCount);
            }

        }

        public void OnDeleteBtnClicked()
        {
            TreeViewItem item = CurSelectedItem;
            if (item == null)
            {
                Debug.Log("Please Select a Item First");
                return;
            }
            item.ParentTreeList.DeleteItem(item);
        }

        public void OnBackBtnClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

    }
}