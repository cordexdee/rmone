using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility.Entities;
using System.Web.UI.WebControls;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.DashboardPanelView)]
    public class DashboardPanelView:DBBaseEntity
    {
        public long ID { get; set; }
        public string DashboardPanelInfo { get; set; }
        [NotMapped]
        public DashboardViewProperties ViewProperty { get; set; }
        public string AuthorizedToViewUsers { get; set; }
        [NotMapped]
        public List<UserProfile> AuthorizedToView { get; set; }
        public string ViewName { get; set; }
        public string ViewType { get; set; }
        public string Title { get; set; }
        //public DashboardPanelView(string viewName)
        //{
        //    ViewName = viewName;
        //}
    }


    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(IndivisibleDashboardsView))]
    [System.Xml.Serialization.XmlInclude(typeof(SuperDashboardsView))]
    [System.Xml.Serialization.XmlInclude(typeof(SideDashboardView))]
    [System.Xml.Serialization.XmlInclude(typeof(CommonDashboardsView))]
    public class DashboardViewProperties
    {

    }

    public class IndivisibleDashboardsView : DashboardViewProperties
    {
        public List<DashboardPanelProperty> Dashboards { get; set; }
        public List<DashboardFilterProperty> GlobalFilers { get; set; }
    }

    public class CommonDashboardsView : DashboardViewProperties
    {
        public List<DashboardPanelProperty> Dashboards { get; set; }
        public List<DashboardFilterProperty> GlobalFilers { get; set; }
        public int ViewHeight { get; set; }
        public int ViewWidth { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingBottom { get; set; }
        public string ViewBackground { get; set; }
        public double Opacity { get; set; }
        public string BorderWidth { get; set; }
        public string BorderColor { get; set; }
        public DashboardLayoutType LayoutType { get; set; }
        public string ViewBackgroundColor { get; set; }
        public bool IsThemable { get; set; }
    }

    public class SuperDashboardsView : DashboardViewProperties
    {
        public List<DashboardGroupProperty> DashboardGroups { get; set; }
        public List<DashboardFilterProperty> GlobalFilers { get; set; }
    }


    public class SideDashboardView : DashboardViewProperties
    {
        public double Width { get; set; }
        public List<DashboardSideProperty> DashboardSideList { get; set; }
        public List<DashboardGroupProperty> DashboardGroups { get; set; }
        public List<DashboardPanelProperty> Dashboards { get; set; }
    }

    public class DashboardSideProperty
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string Imapgepath { get; set; }
        public int Height { get; set; }
        public bool IsFlat { get; set; }
        public short NavigationType { get; set; }
        public int ItemOrder { get; set; }
        public bool IsHideTitle { get; set; }
    }
    public class DashboardGroupProperty
    {
        public string DashboardGroup { get; set; }
        public int Width { get; set; }
        public List<DashboardPanelProperty> Dashboards { get; set; }
        public int ItemOrder { get; set; }
        public string GroupTheme { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundIcon { get; set; }
    }

    public class DashboardFilterProperty
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public int ItemOrder { get; set; }
        public string ListName { get; set; }
        public string ColumnName { get; set; }
        public bool Hidden { get; set; }
        public List<string> DefaultValues { get; set; }
        public DashboardFilterProperty()
        {
            ID = Guid.NewGuid();
            Title = string.Empty;
            ListName = string.Empty;
            ColumnName = string.Empty;
            DefaultValues = new List<string>();
        }
    }


    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(DashbordChartProperty))]
    [System.Xml.Serialization.XmlInclude(typeof(DashbordKPIoperty))]
    public class DashboardPanelProperty
    {
        public bool DisableInheritDefault { get; set; }
        public string DashboardName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public UnitType WidthUnitType { get; set; }
        public UnitType LeftUnitType { get; set; }
        public string Theme { get; set; }
        public int ItemOrder { get; set; }
        public string DisplayName { get; set; }
        public string IconUrl { get; set; }
        public string BackGroundUrl { get; set; }
        public string GroupTheme { get; set; }
        public string DashboardUrl { get; set; }
        public short NavigationType { get; set; }
        public bool StartFromNewLine { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Zindex { get; set; }
        public bool IsLink { get; set; }
        public string LinkUrl { get; set; }
        public string QueryParameter { get; set; }
        public int iconLeft { get; set; }
        public int iconTop { get; set; }
        public int iconWidth { get; set; }
        public int iconHeight { get; set; }
        public string LinkType { get; set; }
        public string LinkDetails { get; set; }
        public int PanelTop { get; set; }
        public int PanelLeft { get; set; }
        public string FontStyle { get; set; }
        public string HeaderFontStyle { get; set; }
        public int TitleTop { get; set; }
        public int TitleLeft { get; set; }
        public string BorderColor { get; set; }
        public string DashboardType { get; set; }
        public string IconShape { get; set; }
        public string DashboardSubType { get; set; }
        public int PageSize { get; set; }
        public string Module { get; set; }
        public bool IsHideTitle { get; set; }
        public string Priority { get; set; }
        public bool IsCritical { get; set; }
        public string DueDate { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public bool EnableFilter { get; set; }
        public bool EnableFilterPredictBacklog { get; set; }
        public bool EnableFilterTicketCreatedByWeek { get; set; }
        public bool EnableFilterScoreCard { get; set; }
        public string WeeklyAverage { get; set; }
        public DateTime ScoreCardStartDate { get; set; }
        public DateTime ScoreCardEndDate { get; set; }
        public DateTime TicketFlowStartDate { get; set; }
        public DateTime TicketFlowEndDate { get; set; }
        public bool EnableFilterTicketFlow { get; set; }
        public bool HideSLATabular { get; set; }
        public string ShowMode { get; set; }
        public string DisplayUnit { get; set; }// stand for Days & Hours
        public string CreatedOn { get; set; }// Created on filter
        public string CompletedOn { get; set; }//Completed on filter
        public bool ShowTotal { get; set; }
        public bool IncludeOpenTickets { get; set; }
        /// <summary>
        /// -1 for Current User
        ///  0 for All
        ///  > 0 Specific User
        /// </summary>
        public int UserID { get; set; }
        public List<KPIDetail> KPIList { get; set; }
        public bool ShowKPIDetail { get; set; }
        public string HelpCards { get; set; }
        public string WelcomeMessage { get; set; }
        public bool ShowProjectCountOnWelcomeScreen { get; set; }
        public bool HideResourceAllocationFilter  { get;set; }
        public bool ShowCurrentUserDetailsOnly { get; set; }
        public bool HideAllocationType { get; set; }
        //public string UnfilledAllocationType { get; set; }
        public bool ShowByUsersDivision { get; set; }

        public DashboardPanelProperty()
        {
            DisplayName = string.Empty;
            DashboardName = string.Empty;
            IconUrl = string.Empty;
            Theme = string.Empty;
            DisableInheritDefault = true;
            BackGroundUrl = string.Empty;
            GroupTheme = "default";
            DashboardUrl = string.Empty;
            NavigationType = 0;
            QueryParameter = string.Empty;
            BorderColor = string.Empty;
            IsCritical = true;
            Priority = "high";
            DueDate = "both";
            UserID = -1; // For Current user
            Status = string.Empty;
            DisplayUnit = "d";//"d" & "h" stand for Days & Hours
        }
    }

    public class DashbordChartProperty : DashboardPanelProperty
    {
        public bool HideZoomAction { get; set; }
        public bool HideDownloadAction { get; set; }
        public bool HideTableAction { get; set; }
    }

    public class DashbordKPIoperty : DashboardPanelProperty
    {

    }

    public class KPIDetail
    {
        public string KpiName { get; set; }
        public string KpiDisplayName { get; set; }
        public int Order { get; set; }
        public bool HideIcon { get; set; }
    }
}
