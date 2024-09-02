using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class BudgetCategoriesEdit : UserControl
    {
        public int BudgetCategoryID { get; set; }
        //SPListItem spListItemBudgetCategory;
        BudgetCategory spListItemBudgetCategory;
        BudgetCategoryViewManager budgetCataegoryManager = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            spListItemBudgetCategory = (budgetCataegoryManager.LoadByID(BudgetCategoryID));
            BindBudgetType();
            Fill();
            base.OnInit(e);
        }

        private void BindBudgetType()
        {
            List<BudgetCategory> dtBGetCat = budgetCataegoryManager.Load(x => !string.IsNullOrEmpty(x.BudgetType));
            if (dtBGetCat != null && dtBGetCat.Count > 0)
            {
                List<BudgetCategory> dtBudgetType = dtBGetCat.OrderBy(x => x.BudgetType).ToList();
                if (dtBudgetType != null && dtBudgetType.Count > 0)
                {
                    foreach (BudgetCategory row in dtBudgetType)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(row.BudgetType)) && ddlBudgetType.Items.FindByText(Convert.ToString(row.BudgetType)) == null)
                        {
                            ddlBudgetType.Items.Add(Convert.ToString(row.BudgetType));
                        }
                    }
                }
                ddlBudgetType.Items.Insert(0, "");
            }

        }
        private void Fill()
        {
            if (spListItemBudgetCategory != null)
            {
                txtBudgetCategory.Text = Convert.ToString(spListItemBudgetCategory.BudgetCategoryName);
                txtBudgetSubCategory.Text = Convert.ToString(spListItemBudgetCategory.BudgetSubCategory);
                txtDescription.Text = Convert.ToString(spListItemBudgetCategory.BudgetDescription);
                txtAcronym.Text = Convert.ToString(spListItemBudgetCategory.BudgetAcronym);
                txtCOA.Text = Convert.ToString(spListItemBudgetCategory.BudgetCOA);
                ddlBudgetType.SelectedValue = Convert.ToString(spListItemBudgetCategory.BudgetType);
                txtBudgetTypeCOA.Text = Convert.ToString(spListItemBudgetCategory.BudgetTypeCOA);
                chkCapEx.Checked = Convert.ToBoolean(spListItemBudgetCategory.CapitalExpenditure);
                chkDeleted.Checked = Convert.ToBoolean(spListItemBudgetCategory.Deleted);
                chkIncludesStaffing.Checked = Convert.ToBoolean(spListItemBudgetCategory.IncludesStaffing);

                ///Authorized to View Field multiUser field fill
                ppeAuthorizedToView.SetValues(Convert.ToString(spListItemBudgetCategory.AuthorizedToView)); //ppeAuthorizedToView.UpdateEntities(uHelper.getUsersListFromCollection(AuthorizedToViewColl));

                ///Authorized to Edit Field multiUser field fill
                ppeAuthorizedToEdit.SetValues(Convert.ToString(spListItemBudgetCategory.AuthorizedToEdit));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BudgetCategory budgetCategoryObj = new BudgetCategory();
            budgetCategoryObj.ID = Convert.ToInt64(spListItemBudgetCategory.ID);
            budgetCategoryObj.Title = txtBudgetCategory.Text.Trim() + " - " + txtBudgetSubCategory.Text.Trim();
            budgetCategoryObj.BudgetCategoryName = txtBudgetCategory.Text.Trim();
            budgetCategoryObj.BudgetSubCategory = txtBudgetSubCategory.Text.Trim();
            budgetCategoryObj.BudgetDescription = txtDescription.Text.Trim();
            budgetCategoryObj.BudgetAcronym = txtAcronym.Text.Trim();
            budgetCategoryObj.BudgetCOA = txtCOA.Text.Trim();

            budgetCategoryObj.BudgetType = hdnTextFieldVisible.Value == "true" ? txtBudgetType.Text.Trim() : ddlBudgetType.SelectedValue;
            budgetCategoryObj.BudgetTypeCOA = txtBudgetTypeCOA.Text.Trim();
            budgetCategoryObj.CapitalExpenditure = chkCapEx.Checked;
            budgetCategoryObj.Deleted = chkDeleted.Checked;
            budgetCategoryObj.IncludesStaffing = chkIncludesStaffing.Checked;

            ///For Authorized To View People Picker

            budgetCategoryObj.AuthorizedToView = ppeAuthorizedToView.GetValues();

            ///For Authorized To Edit People Pickerb);
            budgetCategoryObj.AuthorizedToEdit = ppeAuthorizedToEdit.GetValues();


            budgetCataegoryManager.Update(budgetCategoryObj);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}
