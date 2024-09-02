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
    public partial class ProjectComplexityView : UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectComplexityManager projectComplexityManager = null;
        public string absoluteUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";

        protected override void OnInit(EventArgs e)
        {
            absoluteUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrl, "ProjectComplexityAddEdit", "Add Project Complexity"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            projectComplexityManager = new ProjectComplexityManager(context);
            BindGridView();
        }

        private void BindGridView(bool includeDeleted = false)
        {
            List<ProjectComplexity> projectComplexityList = new List<ProjectComplexity>();

            if (includeDeleted)
            {
                projectComplexityList = projectComplexityManager.Load();
            }
            else
            {
                projectComplexityList = projectComplexityManager.Load().Where(x => x.Deleted != true).ToList();
            }

            grdProjectComplexity.DataSource = projectComplexityList;
            grdProjectComplexity.DataBind();
        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            BindGridView(chkShowDeleted.Checked);
        }
    }
}