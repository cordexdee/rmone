using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ProjectClassNew : UserControl
    {
        private ProjectClass _SPListItem;
        //private DataTable _DataTable;
        ProjectClassViewManager ProjectClassManager = new ProjectClassViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _SPListItem = new ProjectClass();

            _SPListItem.Title = txtTitle.Text.Trim();
            _SPListItem.ProjectNote = txtProjectNote.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;
            ProjectClassManager.Insert(_SPListItem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
