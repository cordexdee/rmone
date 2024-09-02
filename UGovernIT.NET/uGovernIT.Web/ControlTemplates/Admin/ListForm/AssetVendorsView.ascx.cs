using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils.Extensions;
using uGovernIT.Util.Cache;
namespace uGovernIT.Web
{
    public partial class AssetVendorsView : UserControl
    {
        // private SPList _SPList;
        //private DataTable _DataTable;
        private string addNewItem = string.Empty;

        #region constant
        private const string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private const string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private const string formTitle = "Asset Vendors";
        private const string viewParam = "assetvendorview";
        private const string newParam = "assetvendornew";
        private const string editParam = "assetvendoredit";
        #endregion
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','600','720',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','600','720',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
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
            BindGrid();
            base.OnLoad(e);
        }

        void BindGrid()
        {
            VendorTypeManager vendorTypeManager = new VendorTypeManager(context);
            AssetVendorViewManager assetVendorManager = new AssetVendorViewManager(context);
            List<AssetVendor> listAssetVendor = assetVendorManager.Load();
            //DataTable dt = assetVendorManager.GetDataTable();
            listAssetVendor.ForEach(x =>
            {
                VendorType item = vendorTypeManager.LoadByID(x.VendorTypeLookup);
                x.VendorType = item != null ? item.Title : string.Empty;
            });
            //if (listAssetVendor != null && listAssetVendor.Count > 0 && !dxShowDeleted.Checked)
            if (listAssetVendor != null && listAssetVendor.Count > 0)
            {
                if (!dxShowDeleted.Checked)
                    listAssetVendor = listAssetVendor.Where(x => !x.Deleted).ToList();

                if (listAssetVendor != null)
                {
                    dx_gridView.DataSource = listAssetVendor;
                    dx_gridView.DataBind();
                    dx_gridView.ExpandAll();
                }
            }
        }

        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }

        protected void dx_gridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "VendorName")
            {
                int index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.VendorName));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, datakeyvalue));
                string url = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{3} - {1}','600','720',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor ahtml = (HtmlAnchor)dx_gridView.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "VendorName")
                {
                    ahtml.InnerText = e.CellValue.ToString();

                }
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            AssetVendorViewManager assetVendorManager = new AssetVendorViewManager(context);
            List<AssetVendor> listAssetVendor = new List<AssetVendor>();
            listAssetVendor = assetVendorManager.Load();
            Util.Cache.CacheHelper<object>.AddOrUpdate(DatabaseObjects.Tables.AssetVendors, context.TenantID, listAssetVendor);

            //foreach (AssetVendor cVariable in listAssetVendor)
            //{
            //    if (string.IsNullOrEmpty(cVariable.Title))
            //        continue;

            //    CacheHelper<AssetVendor>.AddOrUpdate(DatabaseObjects.Tables.AssetVendors, context.TenantID, cVariable);
            //}

        }
    }
}
