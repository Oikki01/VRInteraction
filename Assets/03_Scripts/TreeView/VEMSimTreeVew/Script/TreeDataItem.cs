//=================================
//创建作者：刘明坤
//创建时间：2021-12-20 9:33:20
//创建说明：1、处理Item  UI数据展现  
//          2、处理单击、选中、展开等展现交互
//          3、后续可根据需求进行修改
//=================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace VEMSimTreeView
{
    public class TreeDataItem : MonoBehaviour
    {
        public Button mExpandBtn;
        public Image mIcon;
        public Image mSelectImg;
        public Button mClickBtn;
        public InputField mOverrideNameInput;
        public Image enterImage;
        public Text mLabelText;
        public Color normalColor;
        public Color selectColor;
        string mData = "";

        public string Data
        {
            get
            {
                return mData;
            }
            set
            {
                mData = value;
            }
        }

        void Start()
        {
            mExpandBtn.onClick.AddListener(OnExpandBtnClicked);
            mClickBtn.onClick.AddListener(OnItemClicked);
        }

        public void Init()
        {
            SetExpandBtnVisible(false);
            SetExpandStatus(true);
            IsSelected = false;
        }

        void OnExpandBtnClicked()
        {
            TreeViewItem item = GetComponent<TreeViewItem>();
            item.DoExpandOrCollapse();
        }


        public void SetItemInfo(string iconSpriteName,string labelTxt,string data = "")
        {
            Init();
            //mIcon.sprite = ResManager.Instance.GetSpriteByName(iconSpriteName);
            mLabelText.text = labelTxt;
            mData = data;
        }

        public void OnItemClicked()
        {
            TreeViewItem item = GetComponent<TreeViewItem>();
            item.RaiseCustomEvent(CustomEvent.ItemClicked, null);
            
        }


        public void SetExpandBtnVisible(bool visible)
        {
            if (visible)
            {
                mExpandBtn.gameObject.SetActive(true);
            }
            else
            {
                mExpandBtn.gameObject.SetActive(false);
            }
        }

        public bool IsSelected
        {
            get
            {
                return mSelectImg.gameObject.activeSelf;
            }
            set
            {
                mSelectImg.gameObject.SetActive(value);
                if (value)
                {
                    mLabelText.color = selectColor;
                }
                else
                {
                    mLabelText.color = normalColor;
                }
            }
        }
        public void SetExpandStatus(bool expand)
        {
            if (expand)
            {
                mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, 0);

            }
        }


    }

}