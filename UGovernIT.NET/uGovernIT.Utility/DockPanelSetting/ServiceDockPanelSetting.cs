using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Utility.DockPanels
{
    public class ServiceDockPanelSetting : DockPanelSetting
    {

        public string WelcomeHeading { get; set; }
        public string WelcomeDesc { get; set; }
        public bool HideWaitingOnMeTab { get; set; }
        public string UpdateWaitingOnMeTab { get; set; }
        public bool HideMyDepartmentTickets { get; set; }
        public string UpdateMyDepartmentTickets { get; set; }
        public bool HideMyDivisionTickets { get; set; }
        public string UpdateMyDivisionTickets { get; set; }
        public bool HideMyPendingApprovals { get; set; }
        public string UpdateMyPendingApprovals { get; set; }
        public bool HideMyProject { get; set; }
        public string UpdateMyProject { get; set; }
        public bool HideMyTasks { get; set; }
        public string UpdateMyTasks { get; set; }
        public bool HideMyTickets { get; set; }
        public string UpdateMyTickets { get; set; }
        public bool HideMyClosedTickets { get; set; }
        public string UpdateMyClosedTickets { get; set; }
        public bool HideSMSModules { get; set; }
        public bool HideGovernanceModules { get; set; }
        public bool ShowServiceCatalog { get; set; }
        public bool ShowIcons { get; set; }
        public string ServiceViewType { get; set; }
        public bool EnableNewButton { get; set; }
        public int NoOfPreviewTickets { get; set; }
        public int ModulePanelOrder { get; set; }
        public int MyTicketPanelOrder { get; set; }
        public int ServiceCatalogOrder { get; set; }
        public bool IsServiceDocPanel { get; set; }

        public bool ShowPanel { get; set; }
        public long PanelId { get; set; }
        public int IconSize { get; set; }

    }
}
