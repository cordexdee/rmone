using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Utility
{

    [Table(DatabaseObjects.Tables.PageConfiguration)]
    public class PageConfiguration : DBBaseEntity
    {

        public long ID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string LeftMenuName { get; set; }
        public string LeftMenuType { get; set; }
        public bool HideLeftMenu { get; set; }
        public bool HideTopMenu { get; set; }
        public string TopMenuName { set; get; }
        public string TopMenuType { set; get; }
        public bool HideSearch { get; set; }
        public bool HideFooter { get; set; }
        public string RootFolder { get; set; }
        public string ControlInfo { get; set; }
        public string LayoutInfo { get; set; }
        public bool HideHeader { get; set; }
        [NotMapped]
        public List<DockPanelSetting> ControlInfoList { get; set; }

        [NotMapped]
        public Dictionary<string, List<object>> LayoutInfoList { get; set; }
    }
}
