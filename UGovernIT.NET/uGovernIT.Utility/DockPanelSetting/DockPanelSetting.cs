using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Utility.DockPanels
{
    [System.Xml.Serialization.XmlInclude(typeof(TicketDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(DashboardDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(ModuleWebpartDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(HomeDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(MessageboardDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(RMMDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(DashboardSLADockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(DashboardReportPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(TaskDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(LandingPageDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(HomeCardPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(SuperAdminDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(ServiceDockPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(TicketCountTrendsDocPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(HomeCardGridPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(LinkPanelSetting))]
    [System.Xml.Serialization.XmlInclude(typeof(Applicationuptimesetting))]

    public class DockPanelSetting
    {
        public string AssemblyName { get; set; }
        public string ControlID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool ShowTitle { get; set; }
        public string Icon { get; set; }
        public int PanelOrder { get; set; }
        public bool ShowCompactRows { get; set; }
        public bool ShowBandedRows { get; set; }
        
    }
}
