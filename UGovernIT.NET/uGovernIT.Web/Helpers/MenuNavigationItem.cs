using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web.Helpers
{
    public class MenuNavigationItem : IHierarchyData
    {
        public int ID { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// TitleOnly, IconOnly, Both
        /// </summary>
        public string MenuDisplayType { get; set; }
        /// <summary>
        /// Navigate, Modeless, modal
        /// Navigate:- Open in same window
        /// Modeless:- Open in New tab
        /// Modal:- Open in popup
        /// </summary>
        public string NavigationType { get; set; }
        public string MenuName { get; set; }
        public string MenuHeight { get; set; }
        public string MenuWidth { get; set; }
        public string MenuBackground { get; set; }
        public string SubMenuStyle { get; set; }
        public string MenuFontColor { get; set; }
        public string MenuItemSeparation { get; set; }
        public string MenuTextAlignment { get; set; }
        public bool CustomizeFormat { get; set; }
        public bool IsDisabled { get; set; }
        public int SubMenuItemPerRow { get; set; }
        public string SubMenuItemAlignment { get; set; }
        public string CustomProperties { get; set; }
        public string MenuFontSize { get; set; }
        public string MenuFontFontFamily { get; set; }
        public string MenuIconAlignment { get; set; }
        public string TenantID { get; set; }
        public bool Deleted { get; set; }
        /// <summary>
        /// ParentID 0 means top item
        /// </summary>
        public int MenuParentLookup { get; set; }  //ParentId renamed according to table name

        public MenuNavigationItem Parent { get; set; }


        public string NavigationUrl { get; set; }
        public string IconUrl { get; set; }
        public string AttachmentUrl { get; set; }

        public string AuthorizedToView { get; set; }
        //public Dictionary<string, string> CustomProperties { get; set; }

        public List<MenuNavigationItem> Children { get; set; }
        public int ChildCount { get; set; }
        public int ItemOrder { get; set; }


        public IHierarchicalEnumerable GetChildren()
        {
            MenuNavigationCollection collection = new MenuNavigationCollection();
            foreach (MenuNavigationItem item in Children)
                if (item != null && item.IsDisabled == false)
                    collection.Add(item);
                //collection.AddRange(Children.Where(x=>x.IsDisabled==false));
            return collection;

        }

        public IHierarchyData GetParent()
        {
            return Parent as IHierarchyData;
        }

        public bool HasChildren
        {
            get
            {
                if (ChildCount > 0)
                    return true;

                return false;
            }
        }

        public object Item
        {
            get { return this; }
        }

        public string Path
        {
            get
            {
                return string.Empty;
            }
        }

        public string Type
        {
            get { return "MenuNavigationItem"; }
        }
    }

    public class MenuNavigationCollection : List<MenuNavigationItem>, IHierarchicalEnumerable
    {
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }
    }
    
}
