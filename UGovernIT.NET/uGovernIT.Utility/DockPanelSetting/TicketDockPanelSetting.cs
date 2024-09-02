using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Utility.DockPanels
{
    public class TicketDockPanelSetting:DockPanelSetting
    {
        public string ModuleName { get; set; }
        public int PageSize { get; set; }         
        public bool HideStatusOverProgressBar { get; set; }
        public bool HideModuleLogo { get; set; }
        public bool HideModuleDescription { get; set; }
        public bool HideNewbutton { get; set; }
        public bool HideFilteredTabs { get; set; }
        public bool HideGlobalSearch { get; set; }

        //new property for show/hide the tab in customfilter ticket.
        
        public string DataFilterExpression { get; set; }    
    }
}