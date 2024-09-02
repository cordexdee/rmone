using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Report.Entities;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Reports;

namespace uGovernIT.Report.DxReport.ResourceSchedule
{
    public partial class ResourceScheduler_Viewer : System.Web.UI.UserControl
    {
        public string jsonReportData { get; set; }
        public string jsonGroupsData { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationContext context = Session["context"] as ApplicationContext;
            DataTable reportDatatable = uHelper.GetAllocationData(context);
            
            List<ResourceSchedulerReport> entity = UGITUtility.ConvertDataTable<ResourceSchedulerReport>(reportDatatable);
            jsonReportData = "{ data:" + JsonConvert.SerializeObject(entity) + "}";

            List<string> groupvalues = entity.Select(x => x.Id).Distinct().ToList();
            List<GroupsKeyLabel> keys = new List<GroupsKeyLabel>();
            foreach (string s in groupvalues)
            {
                keys.Add(new GroupsKeyLabel() { key = s, label = s });
            }
            jsonGroupsData = JsonConvert.SerializeObject(keys);
        }

    }

    public class GroupsKeyLabel
    {
        public string key { get; set; }
        public string label { get; set; }
    }
}
