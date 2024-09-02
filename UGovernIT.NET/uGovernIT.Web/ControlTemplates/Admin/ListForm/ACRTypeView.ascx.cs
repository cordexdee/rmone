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
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ACRTypeView : UserControl
    {
        // private SPList _SPList;
        private string addNewItem = string.Empty;
        ACRTypeManager acrType = new ACRTypeManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=acrtypenew");
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','ACR Types - New Itemss','500','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','ACR Types - New Item','500','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
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

            }
            BindGriview();
            base.OnLoad(e);
        }

        private void BindGriview()
        {
           List<ACRType> lstAcrType= acrType.Load();

            if (lstAcrType != null && !dxchkShowDeleted.Checked)
            {
                lstAcrType = lstAcrType.Where(x=>!x.Deleted).ToList();
            }
            dxGvACR.DataSource = lstAcrType;
            dxGvACR.DataBind();
        }
        protected void dxchkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxchkShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=acrtypes&pageTitle=ACR Types&isdlg=1&isudlg=1&showdelete=" + showdelete);
            Response.Redirect(url);

        }
        protected void dxGvACR_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=acrtypeedit&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','ACR Types - {1}','500','300',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)dxGvACR.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
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
            string cacheName = "Lookup_" + DatabaseObjects.Tables.ACRTypes + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ACRTypes, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
