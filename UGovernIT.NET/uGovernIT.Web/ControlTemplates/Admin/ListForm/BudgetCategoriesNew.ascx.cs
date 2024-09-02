using System;
using System.Data;
using System.Web;
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
using System.Linq;
namespace uGovernIT.Web
{
    public partial class BudgetCategoriesNew : UserControl
    {
        List<BudgetCategory> dtBGetCat;
        public int BudgetCategoryID { get; set; }
        BudgetCategoryViewManager budgetCataegoryManager = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        BudgetCategory spListItemBudgetCategory;
        protected override void OnInit(EventArgs e)
        {
            spListItemBudgetCategory = new BudgetCategory();
            BindBudgetType();
            base.OnInit(e);
        }

        private void BindBudgetType()
        {
            dtBGetCat = budgetCataegoryManager.Load(x => !string.IsNullOrEmpty(x.BudgetCategoryName));
            if (dtBGetCat != null && dtBGetCat.Count > 0)
            {
                ddlBudgetType.Items.Clear();
                ddlBudgetCategory.Items.Clear();
                ddlBudgetSubCategory.Items.Clear();
                // dxheader.Visible = true;
                List<BudgetCategory> dtBudgetType = dtBGetCat.OrderBy(x => x.BudgetType).ToList();
                List<string> lstCategory = dtBGetCat.OrderBy(x => x.BudgetCategoryName).Select(x => x.BudgetCategoryName).Distinct().ToList();
                List<string> lstSubCategory = dtBGetCat.OrderBy(x => x.BudgetSubCategory).Select(x => x.BudgetSubCategory).Distinct().ToList();
                if (lstCategory != null && lstCategory.Count > 0)
                {
                    ddlBudgetCategory.DataSource = lstCategory;
                    ddlBudgetCategory.DataBind();
                }
                if (lstSubCategory != null && lstSubCategory.Count > 0)
                {
                    ddlBudgetSubCategory.DataSource = lstSubCategory;
                    ddlBudgetSubCategory.DataBind();
                }
                if (dtBudgetType != null && dtBudgetType.Count > 0)
                {
                    foreach (BudgetCategory row in dtBudgetType)
                    {
                        if (Convert.ToString(row.Deleted) == "True")
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                            {
                                if (ddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                    ddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                            }
                            else
                            {
                                if (ddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                    ddlBudgetType.Items.Add("None");
                            }
                        }
                        else
                        {
                            if (Convert.ToString(row.Deleted) == "False")
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)))
                                {
                                    if (ddlBudgetType.Items.FindByTextWithTrim(Convert.ToString(row.BudgetType)) == null)
                                    {
                                        ddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                                    }
                                }
                                else
                                {
                                    if (ddlBudgetType.Items.FindByTextWithTrim("None") == null)
                                        ddlBudgetType.Items.Add("None");
                                }

                            }

                        }
                    }
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            // spListItemBudgetCategory.ID = 0;
            spListItemBudgetCategory.Title = txtBudgetCategory.Text.Trim() + " - " + txtBudgetSubCategory.Text.Trim();
            spListItemBudgetCategory.BudgetCategoryName = string.IsNullOrEmpty(txtBudgetCategory.Text.Trim()) ? ddlBudgetCategory.Text.Trim() : txtBudgetCategory.Text.Trim();
            spListItemBudgetCategory.BudgetSubCategory = string.IsNullOrEmpty(txtBudgetSubCategory.Text.Trim()) ? ddlBudgetSubCategory.Text.Trim() : txtBudgetSubCategory.Text.Trim();
            spListItemBudgetCategory.BudgetDescription = txtDescription.Text.Trim();
            spListItemBudgetCategory.BudgetAcronym = txtAcronym.Text.Trim();
            spListItemBudgetCategory.BudgetCOA = txtCOA.Text.Trim();

            if (ddlBudgetType.SelectedIndex >= 0)
                spListItemBudgetCategory.BudgetType = ddlBudgetType.SelectedItem.Text;
            else
                spListItemBudgetCategory.BudgetType = txtBudgetType.Text.Trim();

            //spListItemBudgetCategory.BudgetType = txtBudgetType.Text.Trim();
            spListItemBudgetCategory.BudgetTypeCOA = txtBudgetTypeCOA.Text.Trim();
            spListItemBudgetCategory.CapitalExpenditure = chkCapEx.Checked;
            spListItemBudgetCategory.Deleted = chkDeleted.Checked;
            spListItemBudgetCategory.IncludesStaffing = chkIncludesStaffing.Checked;

            ///For Authorized To View People Picker
            spListItemBudgetCategory.AuthorizedToView = ppeAuthorizedToView.GetValues();

            ///For Authorized To Edit People Picker
            spListItemBudgetCategory.AuthorizedToEdit = ppeAuthorizedToEdit.GetValues();

            budgetCataegoryManager.Insert(spListItemBudgetCategory);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}
