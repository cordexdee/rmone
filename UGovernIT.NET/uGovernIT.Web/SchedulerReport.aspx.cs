
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
namespace uGovernIT
{
    public partial class SchedulerReport : System.Web.UI.Page
    {
        private string ReportName { get; set; }
        private string ControlName { get; set; }
        private string ScheduleFilter = "_SchedulerFilter";
        protected override void OnInit(EventArgs e)
        {
            ReportName = Request["reportName"];
            if (!string.IsNullOrEmpty(ReportName))
            {
                ControlName = ReportName + ScheduleFilter;
            }
            if (!string.IsNullOrEmpty(ControlName))
            {
                Control ctr = Page.LoadControl("~/Reports/DxReport/" + ReportName + "/" + ControlName + ".ascx");
                if (ctr != null)
                {
                    DelegatePanel.Controls.Add(ctr);

                }
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {


        }
    }
}
