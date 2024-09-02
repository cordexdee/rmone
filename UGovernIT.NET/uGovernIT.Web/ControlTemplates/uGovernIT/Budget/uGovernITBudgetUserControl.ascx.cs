using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT.Budget
{
    public partial class uGovernITBudgetUserControl : System.Web.UI.UserControl
    {
        public string ITGPortfolioURL;
        public string ITGBudgetManagementURL;
        public string ITGBudgetEditorURL;
        public string GovernanceReviewURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid newPortfolioFrameId = Guid.NewGuid();
            ITGPortfolioURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=ITGPortfolio",
                                                                                        newPortfolioFrameId));
            Guid newBudgetManagementFrameId = Guid.NewGuid();
            ITGBudgetManagementURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=ITGBudgetManagement",
                                                                                        newBudgetManagementFrameId));
            Guid newBudgetEditorFrameId = Guid.NewGuid();
            ITGBudgetEditorURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=ITGBudgetEditor",
                                                                                        newBudgetEditorFrameId));
            Guid newGovernanceReviewFrameId = Guid.NewGuid();
            GovernanceReviewURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=GovernanceReview",
                                                                                        newGovernanceReviewFrameId));

        }
    }
}