/*
 * 树形列表数据实体类
 * Author lidongwei
 * 2021.2.20
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TreeListView
{
    public class TreeItemData
    {
        /// <summary>
        /// 自身id
        /// </summary>
        public int Id;

        /// <summary>
        /// 标题
        /// </summary>
        public string TextStr;

        /// <summary>
        /// 父节点
        /// </summary>
        public int ParentId;

        /// <summary>
        /// 自身节点的层级 级别
        /// </summary>
        public int SelfLevel;

        public TreeItemData(int id, string titleStr, int parentId = -1, int selfLevel = 0)
        {
            this.Id = id;
            this.TextStr = titleStr;
            this.ParentId = parentId;
            this.SelfLevel = selfLevel;
        }

        public TreeItemData()
        {
            
        }

        public TreeItemData Copy()
        {
            return MemberwiseClone() as TreeItemData;
        }
    }
}