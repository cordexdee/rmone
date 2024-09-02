using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Linq;
using System.Collections.Generic;
namespace uGovernIT.Web
{
    public partial class DRQRapidTypesView : UserControl
    {
        private string addNewItem = string.Empty;
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private string formTitle = "DRQ Rapid Types";
        private string viewParam = "drqrapidtypes";
        private string newParam = "drqrapidtypesnew";
        private string editParam = "drqrapidtypesedit";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','500','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','500','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    dxchkShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }
            }
            BindGriview();
            base.OnLoad(e);
        }

        private void BindGriview()
        {
            DrqRapidTypesManager drqTypeManager = new DrqRapidTypesManager(context);
            List<DRQRapidType> lstDRQRapidType = drqTypeManager.Load();
            if (lstDRQRapidType != null && !dxchkShowDeleted.Checked)
            {
                lstDRQRapidType = lstDRQRapidType.Where(x=> !x.Deleted).ToList(); //dt.Select(string.Format("{0}='{1}' or {0} IS NULL", DatabaseObjects.Columns.IsDeleted, "False"));
                //if (dataRows.Length > 0)
                //{
                //    dt = null;
                //    dt = dataRows.CopyToDataTable();
                //}

            }
            dxGvDRQ.DataSource = lstDRQRapidType;
            dxGvDRQ.DataBind();
        }

        protected void dxchkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxchkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, this.viewParam, this.formTitle, showdelete));
            Response.Redirect(url);
        }
        protected void dxGvDRQ_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','500','300',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)dxGvDRQ.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
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
            string cacheName = "Lookup_" + DatabaseObjects.Tables.DRQRapidTypes + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DRQRapidTypes, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
