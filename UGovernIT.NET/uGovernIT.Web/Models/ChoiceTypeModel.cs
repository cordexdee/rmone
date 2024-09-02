using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class ChoiceTypeModel
    {
        public int requesttypeid { get; set; }
        public string value { get; set; }
        public string item { get; set; }
        public string moduleName { get; set; }
        public string fieldName { get; set; }

    }
    public class TicketModel
    {
        public string id { get; set; }
        public string image { get; set; }
        public string moduleName { get; set; }
        public int currentStep { get; set; }
    }
    public class DashbaordPanelKPIFilter
    {
        public string viewID { get; set; }
        public string panelID { get; set; }
        public string kpiIDs { get; set; }
        public string globalFilter { get; set; }
    }
    public class DashboardChartFilter
    {
        public string viewID { get; set; }
        public string panelID { get; set; }
        public string sidebar { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string viewType { get; set; }
        public string datapointFilter { get; set; }
        public string drillDown { get; set; }
        public string localFilter { get; set; }
        public string globalFilter { get; set; }
        public string whoTriggered { get; set; }
        public string zoom { get; set; }
    }
    public class DashboardFilters
    {
        public string viewID { get; set; }
        public string globalFilter { get; set; }
    }
    public class CommonModel
    {
        public string name { get; set; }
        public string id { get; set; }
        
    }

}