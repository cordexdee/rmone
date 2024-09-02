using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.DashboardPanels)]
    public class Dashboard:DBBaseEntity
    {
        public readonly DashboardType PanelType;

        [Key]
        public long ID { get; set; }
        [NotMapped]
        public int SecondryID { get; set; }
        [NotMapped]
        public DashboardPanel panel { get; set; }
        public string DashboardPanelInfo { get; set; }
        public int ItemOrder { get; set; }
        public string Title { get; set; }
        public string DashboardDescription { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [Column(DatabaseObjects.Columns.DashboardPermission)]
        public string DashboardPermission { get; set; }
        //public UserLookupValue Permissions { get; set; }
        public DashboardType DashboardType { get; set; }

        public bool? IsShowInSideBar { get; set; }
        public string Icon { get; set; }
        public bool? IsHideTitle { get; set; }
        public bool? IsHideDescription { get; set; }
        public int PanelWidth { get; set; }
        public int PanelHeight { get; set; }
        [NotMapped]
        public int Top { get; set; }
        [NotMapped]
        public int Left { get; set; }
        public string ThemeColor { get; set; }
        public string CategoryName { get; set; }
        public string SubCategory { get; set; }
        public bool? IsActivated { get; set; }
        public string FontStyle { get; set; }
        public string HeaderFontStyle { get; set; }
        [NotMapped]
        public string DashboardSubType { get; set; }
        //[System.Xml.Serialization.XmlIgnore]
        public string AuthorizedToView { get; set; }

       // [System.Xml.Serialization.XmlIgnore]
        public string DashboardModuleMultiLookup { get; set; }
        
        public Dashboard()
        {
            Title = string.Empty;
            DashboardDescription = string.Empty;
            Icon = string.Empty;
            ThemeColor = "Accent1";
        }
    }
}
