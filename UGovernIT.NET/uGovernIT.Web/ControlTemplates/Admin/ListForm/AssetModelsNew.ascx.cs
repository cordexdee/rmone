using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Util.Cache;
namespace uGovernIT.Web
{
    //Changes Made by Munna Singh 
    public partial class AssetModelsNew : UserControl
    {
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        BudgetCategoryViewManager budgetCategoryViewManager;
        AssetModelViewManager assetModelViewManager;
        AssetVendorViewManager assetVendorViewManager;
        protected override void OnInit(EventArgs e)
        {
            assetModelViewManager = new AssetModelViewManager(_context);
            budgetCategoryViewManager = new BudgetCategoryViewManager(_context);
            BindVendor();
            BindBudget();
            base.OnInit(e);
        }

        private void BindBudget()
        {
            List<BudgetCategory> lstBudgetCategory = budgetCategoryViewManager.Load();
            if (lstBudgetCategory != null && lstBudgetCategory.Count > 0)
            {
                foreach (BudgetCategory row in lstBudgetCategory)
                {
                    ddlBudgetItem.Items.Add(new ListItem(Convert.ToString(row.BudgetSubCategory), Convert.ToString(row.ID)));
                }
                ddlBudgetItem.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        private void BindVendor()
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            assetVendorViewManager = new AssetVendorViewManager(_context);
            List<AssetVendor> items = new List<AssetVendor>();
            items = (List<AssetVendor>)CacheHelper<object>.Get(DatabaseObjects.Tables.AssetVendors, this._context.TenantID);
            if (items == null || items.Count == 0)
            {
                items  = assetVendorViewManager.Load().Where(x => x.Deleted == false).OrderBy(x => x.VendorName).ToList();
                Util.Cache.CacheHelper<object>.AddOrUpdate(DatabaseObjects.Tables.AssetVendors, context.TenantID, items);
            }
            if (items != null && items.Count > 0)
            {
                if (items != null && items.Count > 0)
                {
                    ddlVendor.DataTextField = "Title";
                    ddlVendor.DataValueField = "ID";
                    ddlVendor.DataSource = UGITUtility.ToDataTable<AssetVendor>(items);
                    ddlVendor.DataBind();

                }
                else
                {
                    ddlVendor.Items.Insert(0, new ListItem("None", "0"));
                }
            }
            

            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AssetModel assetModel = new AssetModel();
            assetModel.Title= txtModelName.Text.Trim();
            assetModel.ModelName= txtModelName.Text.Trim();
            assetModel.VendorLookup =Convert.ToInt64(ddlVendor.SelectedValue.Trim());
            assetModel.BudgetLookup = Convert.ToInt64(ddlBudgetItem.SelectedValue.Trim());
            assetModel.ModelDescription = txtDescription.Text.Trim();
            assetModel.Deleted= chkDeleted.Checked;
            assetModelViewManager.Insert(assetModel);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
