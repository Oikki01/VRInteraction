using UnityEngine.UI;

namespace UIWidgets
{
    using UIWidgets.l10n;
    using UnityEngine;

    /// <summary>
    /// TreeView component.
    /// </summary>
    public class TreeViewComponent : TreeViewComponentBase<TreeViewItem>
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Toggle toggleIcon;

        TreeViewItem item;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (toggleIcon != null)
            {
                toggleIcon.onValueChanged.AddListener(ToggleIconOnValueChanged);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (toggleIcon != null)
            {
                toggleIcon.onValueChanged.RemoveListener(ToggleIconOnValueChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        private void ToggleIconOnValueChanged(bool isOn)
        {
            SetTextColor();
            Owner.ItemsEvents.IconEnable.Invoke(isOn, this);
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public TreeViewItem Item
        {
            get { return item; }

            set
            {
                if (item != null)
                {
                    item.OnChange -= UpdateView;
                }

                item = value;

                if (item != null)
                {
                    item.OnChange += UpdateView;
                }

                UpdateView();
            }
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="depth">Depth.</param>
        public override void SetData(TreeNode<TreeViewItem> node, int depth)
        {
            Node = node;
            base.SetData(Node, depth);

            Item = (Node == null) ? null : Node.Item;
        }

        /// <inheritdoc/>
        public override void LocaleChanged()
        {
            UpdateName();
        }

        /// <summary>
        /// Update display name.
        /// </summary>
        protected virtual void UpdateName()
        {
            if (Item == null)
            {
                return;
            }

            TextAdapter.text = Item.LocalizedName ?? Localization.GetTranslation(Item.Name);
            if (TextIndex != null)
            {
                TextIndex.text = item.Index.ToString();
            }
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        protected virtual void UpdateView()
        {
            if ((Icon == null) || (TextAdapter == null))
            {
                return;
            }

            if (Item == null)
            {
                Icon.sprite = null;
                TextAdapter.text = string.Empty;
            }
            else
            {
                Icon.sprite = Item.Icon;
                UpdateName();
            }

            if (SetNativeSize)
            {
                Icon.SetNativeSize();
            }

            // set transparent color if no icon
            Icon.color = (Icon.sprite == null) ? Color.clear : Color.white;

            // if (toggleIcon != null)
            // {
            //     VEMTreeViewItem vemItem = Item as VEMTreeViewItem;
            //     if (vemItem != null && vemItem.Trs != null)
            //     {
            //         toggleIcon.SetIsOnWithoutNotify(vemItem.Trs.gameObject.activeSelf);
            //         SetTextColor();
            //     }
            //
            //     //todo
            //     TheoryTreeViewItem theoryItem = Item as TheoryTreeViewItem;
            //     if (theoryItem != null)
            //     {
            //         toggleIcon.SetIsOnWithoutNotify(theoryItem.IsSelected);
            //         SetTextColor();
            //     }
            // }
        }

        /// <summary>
        /// Called when item moved to cache, you can use it free used resources.
        /// </summary>
        public override void MovedToCache()
        {
            if (Icon != null)
            {
                Icon.sprite = null;
            }
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            if (item != null)
            {
                item.OnChange -= UpdateView;
            }

            base.OnDestroy();
        }

        private void SetTextColor()
        {
            Color color = TextAdapter.color;
            color.a = toggleIcon.isOn ? 1 : 0.5f;
            TextAdapter.color = color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="isNotify"></param>
        public void SetToggleIsOn(bool isOn,bool isNotify = false)
        {
            if (isNotify)
            {
                toggleIcon.isOn = isOn;
            }
            else
            {
                toggleIcon.SetIsOnWithoutNotify(isOn);
            }
        }
    }
}