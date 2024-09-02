using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class DRQSystemAreaView : UserControl
    {
        private string addNewItem = string.Empty;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private string formTitle = "DRQ System Areas";
        private string viewParam = "drqsystemarea";
        private string newParam = "drqsystemareasnew";
        private string editParam = "drqsystemareasedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle));
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    dxchkShowDeleted.Checked = showdelete == "0" ? false : true;
                }
                BindGriview();
            }
            else
            {
                BindGriview();
            }
            base.OnLoad(e);
        }

        private void BindGriview()
        {
            DrqSyetemAreaViewManager drqSystemAreaManager = new DrqSyetemAreaViewManager(context);
            List<DRQSystemArea> lstDRQSystemArea = drqSystemAreaManager.Load();
            if (lstDRQSystemArea != null && !dxchkShowDeleted.Checked)
            {
                lstDRQSystemArea= lstDRQSystemArea.Where(x => !x.Deleted).ToList();
                //DataRow[] dataRows = dt.Select(string.Format("{0}='{1}' or {0} IS NULL", DatabaseObjects.Columns.IsDeleted, "False"));
                //if (dataRows.Length > 0)
                //{
                //    dt = null;
                //    dt = dataRows.CopyToDataTable();
                //}
            }
            dxGvDSAreas.DataSource = lstDRQSystemArea;
            dxGvDSAreas.DataBind();

        }
        protected void dxchkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxchkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }
        protected void dxGvDSAreas_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','500','300',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), this.formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxGvDSAreas.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.DRQSystemAreas + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DRQSystemAreas, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
