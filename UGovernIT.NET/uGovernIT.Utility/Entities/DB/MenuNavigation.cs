using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace uGovernIT.Utility
{
    
    [Table("Config_MenuNavigation")]
    public class MenuNavigation:DBBaseEntity
    {
        public long ID { get; set; }
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
        public int? MenuHeight { get; set; } 
        public int? MenuWidth { get; set; } 
        public string MenuBackground { get; set; } 
        public string SubMenuStyle { get; set; } 
        public string MenuFontColor { get; set; } 
        public string MenuItemSeparation { get; set; } 
        [NotMapped]
        public string MenuTextAlignment { get; set; }
        public bool CustomizeFormat { get; set; } 
        public bool IsDisabled { get; set; } 
        public int? SubMenuItemPerRow { get; set; }
        public string SubMenuItemAlignment { get; set; }
        //public string ImageUrl { get; set; }
        public string CustomProperties { get; set; }
        //Properties transferred from MenuNavigationProperties class
        [NotMapped]
        public string MenuFontSize { get; set; }
        [NotMapped]
        public string MenuFontFontFamily { get; set; } 
        [NotMapped]
        public string MenuIconAlignment { get; set; } 
        /// <summary>
        /// ParentID 0 means top item
        /// </summary>
        public long MenuParentLookup { get; set; }  //ParentId renamed according to table name

        [NotMapped]
        public MenuNavigation Parent { get; set; }


        public string NavigationUrl { get; set; }
        public string IconUrl { get; set; }
        [NotMapped]
        public string AttachmentUrl { get; set; }

        public string AuthorizedToView { get; set; }
        //public Dictionary<string, string> CustomProperties { get; set; }

        [NotMapped]
        public List<MenuNavigation> Children { get; set; }
        [NotMapped]
        public int ChildCount { get; set; }
        public int ItemOrder { get; set; }
    }

   

}
