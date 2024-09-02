using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Linq;
using DevExpress.Web.Rendering;
using System.Web;

namespace uGovernIT.Web
{
    public partial class ProjectClassView : UserControl
    {
        //private SPList _SPList;
        //private DataTable _DataTable;
        private string addNewItem = string.Empty;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private string formTitle = "Project Class";
        private string viewParam = "projectclass";
        private string newParam = "projectclassnew";
        private string editParam = "projectclassedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','350',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','350',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    dxShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }
            }
            BindGriview();

            base.OnLoad(e);
        }

        private void BindGriview()
        {
            //SPQuery qry = new SPQuery();
            //qry.Query = "<Where></Where>";
            //qry.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ProjectNote),
            //                               string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.IsDeleted));
            //qry.ViewFieldsOnly = true;

            ProjectClassViewManager projectClassViewManager= new ProjectClassViewManager(context);

            DataTable dt = projectClassViewManager.GetDataTable();       //SPListHelper.GetDataTable(DatabaseObjects.Lists.ProjectClass, qry);
            if (dt != null && !dxShowDeleted.Checked)
            {
                DataRow[] dataRows = dt.Select(string.Format("{0}='False' or {0} = '' or {0} is null", DatabaseObjects.Columns.Deleted));
                if (dataRows.Length > 0)
                {
                    dt = null;
                    dt = dataRows.CopyToDataTable();
                }
            }

            //if (dt != null)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        row[DatabaseObjects.Columns.ProjectNote] = Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]).Length >= 50 ?
            //                                                   Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]).Substring(0, 50) + "..." :
            //                                                   Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]).Substring(0, Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]).Length);
            //    }
            //}
            
            dxgridview.DataSource = dt;
            dxgridview.DataBind();
        }

        protected void dxgridview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if(e.DataColumn.Name=="aEdit"||e.DataColumn.FieldName== "Title")
            {
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                int Index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, datakeyvalue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','350',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor ahtml = (HtmlAnchor)dxgridview.FindRowCellTemplateControl(Index, e.DataColumn, "editlink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    ahtml.InnerText = e.CellValue.ToString();
                }
            }

        }

        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.ProjectClass + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectClass, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
