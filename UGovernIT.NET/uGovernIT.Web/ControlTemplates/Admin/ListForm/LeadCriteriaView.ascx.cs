using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class LeadCriteriaView : UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LeadCriteriaManager leadCriteriaManager = null;
        public string absoluteUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";

        protected override void OnInit(EventArgs e)
        {
            absoluteUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrl, "LeadCriteriaAddEdit", "Add Lead Criteria"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            leadCriteriaManager = new LeadCriteriaManager(context);
            BindGridView();
        }

        private void BindGridView(bool includeDeleted = false)
        {
            List<LeadCriteria> leadCriterialist = new List<LeadCriteria>();

            if (includeDeleted)
            {
                leadCriterialist = leadCriteriaManager.Load();
            }
            else
            {
                leadCriterialist = leadCriteriaManager.Load().Where(x => x.Deleted != true).ToList();
            }

            grdLeadCriteria.DataSource = leadCriterialist;
            grdLeadCriteria.DataBind();
        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            BindGridView(chkShowDeleted.Checked);
        }
    }
}