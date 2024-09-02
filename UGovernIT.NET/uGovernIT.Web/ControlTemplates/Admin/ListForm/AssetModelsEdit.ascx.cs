using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    //Changes by Munna Singh 
    public partial class AssetModelsEdit : UserControl
    {
        public int Id { get; set; }
        //private SPListItem _SPListItem;
        //private DataTable dt;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        BudgetCategoryViewManager budgetCategoryViewManager;
        AssetModelViewManager assetModelViewManager;
        AssetVendorViewManager assetVendorViewManager;
        AssetModel assetModel;
        protected override void OnInit(EventArgs e)
        {
            assetModel = new AssetModel();
            assetModelViewManager = new AssetModelViewManager(_context);
            budgetCategoryViewManager = new BudgetCategoryViewManager(_context);
            assetModel = assetModelViewManager.LoadByID(Convert.ToInt64(Id));
            BindVendor();
            BindBudget();
            Fill();
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
            assetVendorViewManager = new AssetVendorViewManager(_context);
            List<AssetVendor> dtVendor = assetVendorViewManager.Load().Where(x => x.Deleted == false).OrderBy(x => x.VendorName).ToList();
            if (dtVendor != null && dtVendor.Count > 0)
            {
                //foreach (AssetVendor row in dtVendor)
                //{
                //    ddlVendor.Items.Add(new ListItem(Convert.ToString(row.Title), Convert.ToString(row.ID)));
                //}
                ddlVendor.DataTextField = "Title";
                ddlVendor.DataValueField = "ID";
                ddlVendor.DataSource = dtVendor;
                ddlVendor.DataBind();
            }
            else
            {
                ddlVendor.Items.Insert(0, new ListItem("None", "0"));
            }
        }


        private void Fill()
        {
            if (assetModel != null)
            {
                txtModelName.Text = assetModel.ModelName;
                if (!string.IsNullOrEmpty(assetModel.VendorLookup.ToString()))
                {
                    ddlVendor.SelectedValue = assetModel.VendorLookup.ToString();
                }
                if (!string.IsNullOrEmpty(assetModel.BudgetLookup.ToString()))
                {
                    ddlBudgetItem.SelectedValue = assetModel.BudgetLookup.ToString();
                }
                txtDescription.Text = assetModel.ModelDescription;
                chkDeleted.Checked = assetModel.Deleted;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            assetModel.Title = txtModelName.Text.Trim();
            assetModel.ModelName = txtModelName.Text.Trim();
            assetModel.VendorLookup = Convert.ToInt64(ddlVendor.SelectedValue.Trim());
            assetModel.BudgetLookup = Convert.ToInt64(ddlBudgetItem.SelectedValue.Trim());
            assetModel.ModelDescription = txtDescription.Text.Trim();
            assetModel.Deleted = chkDeleted.Checked;
            assetModelViewManager.Update(assetModel);
            uHelper.ClosePopUpAndEndResponse(Context, true);


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
