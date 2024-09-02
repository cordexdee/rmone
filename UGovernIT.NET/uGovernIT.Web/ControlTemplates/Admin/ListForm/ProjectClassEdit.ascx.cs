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
using System.Data.SqlClient;
using Utils;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ProjectClassEdit : UserControl
    {
        public int Id { get; set; }
        private ProjectClass _SPListItem;
        private DataTable _DataTable;
        ProjectClassViewManager ProjectClassManager = new ProjectClassViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectClass," id="+ Id);
            _SPListItem = ProjectClassManager.LoadByID(Id);
            Fill();
            base.OnInit(e);
        }

        private void Fill()
        {
            txtTitle.Text = Convert.ToString(_SPListItem.Title);
            txtProjectNote.Text = Convert.ToString(_SPListItem.ProjectNote);
            chkDeleted.Checked = UGITUtility.StringToBoolean(_SPListItem.Deleted);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _SPListItem.Title = txtTitle.Text.Trim();
            _SPListItem.ProjectNote = txtProjectNote.Text.Trim();
            _SPListItem.Deleted = chkDeleted.Checked;
            ProjectClassManager.Update(_SPListItem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           uHelper.ClosePopUpAndEndResponse(Context, true);
        }


    }
}
