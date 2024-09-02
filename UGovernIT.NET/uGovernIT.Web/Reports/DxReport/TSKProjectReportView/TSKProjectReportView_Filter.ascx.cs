
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager.Reports;
using System.Linq;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
namespace uGovernIT.DxReport
{
    public partial class TSKProjectReportView_Filter : UserControl
    {
        public int TSKid { get; set; }
        public int projectYear { get; set; }

        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        protected override void OnInit(EventArgs e)
        {
            SetDefault();
            base.OnInit(e);
        }

        private void SetDefault()
        {
            TSKid = UGITUtility.StringToInt(Request.Params["TSKid"]);

            chkStatus.Checked = true;
            chkProjectRoles.Checked = true;
            chkProjectDescription.Checked = true;
            chkKeyReceivables.Checked = false;
            chkKeyDeliverables.Checked = false;
            chkShowMilestone.Checked = true;
            chkShowAllTasks.Checked = false;
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void lnkBuild_Click(object sender, EventArgs e)
        {
            string url = delegateControl + "?reportName=TSKProjectReport";

            url = url + string.Format("&TSKIds={0}", TSKid);
            url = url + string.Format("&projectYear={0}", projectYear);
            url = url + string.Format("&Status={0}", chkStatus.Checked);
            url = url + string.Format("&SGC={0}", chkGanttChart.Checked);
            url = url + string.Format("&SAT={0}", chkShowAllTasks.Checked);
            url = url + string.Format("&SMS={0}", chkShowMilestone.Checked);
            url = url + string.Format("&SKD={0}", chkKeyDeliverables.Checked);
            url = url + string.Format("&SKR={0}", chkKeyReceivables.Checked);
            url = url + string.Format("&ProjectRoles={0}", chkProjectRoles.Checked);
            url = url + string.Format("&ProjectDesc={0}", chkProjectDescription.Checked);
            Response.Redirect((url));
        }
    }
}
