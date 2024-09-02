using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Core;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ApplicationsRelatedAssetsView : UserControl
    {
        ApplicationServer spitem;
        public int AssetID { get; set; }
        private string absoluteUrlEdit = "layouts/ugovernit/DelegateControl.aspx?control={0}&ItemId={1}&AssetId={2}";
        private string addNewItem = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DataTable spList = null;

        protected override void OnInit(EventArgs e)
        {
            //spList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationServers, context);
            if (AssetID > 0)
                BindGrid();
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "ApplicationsRelatedAssetsEdit", "0", AssetID));
            aAddNew.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','990','450',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Related Applications"));
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BindGrid()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            values.Add("@AssetsTitleLookup", AssetID);
            spList = GetTableDataManager.GetData(DatabaseObjects.Tables.ApplicationServers, values);
            appServerGrid.DataSource = spList;
            appServerGrid.DataBind();

        }
        protected void appServerGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Keys.Count <= 0)
                return;
            ApplicationServersManager appServerMGR = new ApplicationServersManager(context);
            long id = Convert.ToInt64(e.Keys[DatabaseObjects.Columns.ID]);
            spitem = appServerMGR.Load().FirstOrDefault(x => x.ID == id);
            if (spitem != null)
            {
                appServerMGR.Delete(spitem);
            }
            e.Cancel = true;
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}