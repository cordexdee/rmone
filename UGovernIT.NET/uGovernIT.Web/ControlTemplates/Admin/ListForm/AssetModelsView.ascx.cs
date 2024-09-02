using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Util.Cache;
namespace uGovernIT.Web
{
    public partial class AssetModelsView : UserControl
    {
        private string addNewItem = string.Empty;
        #region constant
        private const string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private const string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}";
        private const string formTitle = "Asset Models";
        private const string viewParam = "assetmodelview";
        private const string newParam = "assetmodelnew";
        private const string editParam = "assetmodeledit";
        #endregion
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            //changes by MKS
            addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=" + newParam);
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','400',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
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
            BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(context);
            AssetModelViewManager assetModelViewManager= new AssetModelViewManager(context);
            AssetVendorViewManager assetVendorViewManager = new AssetVendorViewManager(context);
            BudgetCategory budgetItem = null;
            List<AssetModel> _DataTable = assetModelViewManager.Load().OrderBy(x => x.VendorLookup).ToList();
            _DataTable.ForEach(x =>
            {
                AssetVendor item = assetVendorViewManager.LoadByID(x.VendorLookup);
                if (item != null)
                    x.VendorName = item.VendorName;
                else
                    x.VendorName = "";

                budgetItem = budgetCategoryViewManager.LoadByID(x.BudgetLookup);
                x.BudgetItem = budgetItem != null ? budgetItem.BudgetSubCategory : string.Empty;                
            });
            //if (_DataTable != null && _DataTable.Count>0 && !dxShowDeleted.Checked)
            if (_DataTable != null && _DataTable.Count > 0)
            {
                if (!dxShowDeleted.Checked)
                    _DataTable = _DataTable.Where(x => !x.Deleted).ToList();
                
                if (_DataTable != null && _DataTable.Count > 0)
                {
                    dx_SPGrid.DataSource = _DataTable;
                    dx_SPGrid.DataBind();
                    dx_SPGrid.ExpandAll();
                }
            }
        }
        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showdelete));
            Response.Redirect(url);
        }

        protected void dx_SPGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "ModelName")
            {
                int index = e.VisibleIndex;
                int datakeyvalue = Convert.ToInt32(e.KeyValue);
                string Title = (string)e.GetValue(DatabaseObjects.Columns.ModelName);
                string edititem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=assetmodeledit&ID=" + datakeyvalue + " ");
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','400',0,'{2}','true')", edititem, Title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor ahtml = (HtmlAnchor)dx_SPGrid.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "ModelName")
                {
                    ahtml.InnerText = e.CellValue.ToString();
                }
            }
        }


        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            AssetModelViewManager assetModelViewManager = new AssetModelViewManager(context);
            List<AssetModel> listAssetModel = assetModelViewManager.Load();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.AssetModels + "_" + context.TenantID;
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, listAssetModel);
            //foreach (AssetModel cVariable in listAssetModel)
            //{
            //    if (string.IsNullOrEmpty(cVariable.Title))
            //        continue;

            //    CacheHelper<AssetModel>.AddOrUpdate(DatabaseObjects.Tables.AssetModels, context.TenantID, cVariable);
            //}

        }
    }
}
