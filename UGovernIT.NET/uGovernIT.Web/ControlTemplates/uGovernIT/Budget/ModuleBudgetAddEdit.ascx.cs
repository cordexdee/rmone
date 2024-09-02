
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class ModuleBudgetAddEdit : UserControl
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public string ModuleName { get; set; }
        public bool pmmBudgetNeedsApproval { get; set; }
        StringBuilder linkFile = new StringBuilder();
        
        private ModuleBudget Budget { get; set; }
        private ApplicationContext _context = null;
        private ModuleBudgetManager _moduleBudgetManager = null;
        private ConfigurationVariableManager _configurationVariableHelper = null;
        private BudgetCategoryViewManager _budgetCategoryViewManager = null;
        private BudgetActualsManager _budgetActualsManager = null;
        private DMSManagerService _dmsManagerService = null;

 

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected ModuleBudgetManager ModuleBudgetManager
        {
            get
            {
                if (_moduleBudgetManager == null)
                {
                    _moduleBudgetManager = new ModuleBudgetManager(ApplicationContext);
                }
                return _moduleBudgetManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableHelper == null)
                {
                    _configurationVariableHelper = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableHelper;
            }
        }

        protected BudgetCategoryViewManager BudgetCategoryViewManager
        {
            get
            {
                if (_budgetCategoryViewManager == null)
                {
                    _budgetCategoryViewManager = new BudgetCategoryViewManager(ApplicationContext);
                }
                return _budgetCategoryViewManager;
            }
        }

        protected BudgetActualsManager BudgetActualsManager
        {
            get
            {
                if (_budgetActualsManager == null)
                {
                    _budgetActualsManager = new BudgetActualsManager(ApplicationContext);
                }
                return _budgetActualsManager;
            }
        }

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(ApplicationContext);
                }
                return _dmsManagerService;
            }
        }

        //ApplicationContext _context;

        protected override void OnInit(EventArgs e)
        {

            // Get config setting for PMM budget for approval/rejection and the mode so that user can edit the approved budget or not.
            pmmBudgetNeedsApproval = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.PMMBudgetNeedsApproval);
            if (string.IsNullOrEmpty(ModuleName))
                ModuleName = ModuleName.ToUpper();

            if (Id > 0)
            {
                Budget = ModuleBudgetManager.LoadByID(Id);
            }

            FillEditForm();
            base.OnInit(e);
        }

        protected void FillEditForm()
        {
            FillCategory();

            if (Id > 0)
            {
                ddlBudgetCategories.SelectedIndex = ddlBudgetCategories.Items.IndexOf(ddlBudgetCategories.Items.FindByValue(Convert.ToString(Budget.BudgetCategoryLookup)));

                txtBudgetItemVal.Text = Budget.BudgetItem;

                double budgetAmount = 0;

                double.TryParse(UGITUtility.ObjectToString(Budget.BudgetAmount), out budgetAmount);

                txtBudgetAmountf.SetValue(Convert.ToString(budgetAmount));

                if (Budget.ModuleName != ModuleNames.NPR)
                {
                    if (Budget.BudgetStatus == (int)Enums.BudgetStatus.PendingApproval)
                    {
                        txtBudgetAmountf.SetValue(Convert.ToString(Budget.UnapprovedAmount));
                    }
                    else
                    {
                        txtBudgetAmountf.SetValue(Convert.ToString(Budget.BudgetAmount));
                    }
                }

                dtcBudgetStartDate.Date = Budget.AllocationStartDate;

                dtcBudgetEndDate.Date = Budget.AllocationEndDate;

                txtBudgetDescription.Text = Budget.BudgetDescription;

                txtBudgetComment.Text = Budget.Comment;

                if (!string.IsNullOrEmpty(Budget.Attachments))
                {
                    FileUploadControl.SetValue(Budget.Attachments);
                }
                //BindDocuments.AttchDocument()


            }
        }

        private void FillCategory()
        {
            DataTable resultedTable = BudgetCategoryViewManager.LoadCategories();

            List<ListItem> items = BudgetCategoryViewManager.LoadCategoriesDropDownItems(resultedTable);

            foreach (ListItem item in items)
            {
                ddlBudgetCategories.Items.Add(item);
            }
            ListItem itemSelect = new ListItem("--Select--", "0");
            ddlBudgetCategories.Items.Insert(0, itemSelect);
        }

        protected void Save()
        {
            ModuleBudget _moduleBudget = new ModuleBudget();
            double amount;
            _moduleBudget.TicketId = TicketId;
            TicketId = TicketId.ToUpper();
            _moduleBudget.ModuleName = ModuleName;
            if (txtBudgetAmountf != null && double.TryParse(txtBudgetAmountf.GetValue(), out amount))
            {
                _moduleBudget.BudgetAmount = amount;
            }
            if (txtBudgetDescription != null)
            {
                _moduleBudget.BudgetDescription = txtBudgetDescription.Text;
            }
            if (txtBudgetItemVal != null)
            {
                _moduleBudget.BudgetItem = txtBudgetItemVal.Text;
                _moduleBudget.Title = txtBudgetItemVal.Text;
            }
            if (dtcBudgetStartDate.Date != null)
            {
                _moduleBudget.AllocationStartDate = dtcBudgetStartDate.Date;
            }

            if (dtcBudgetEndDate.Date != null)
            {
                _moduleBudget.AllocationEndDate = dtcBudgetEndDate.Date;
            }
            _moduleBudget.BudgetCategoryLookup = int.Parse(ddlBudgetCategories.SelectedValue);

            // Update the busget status according to setting whether budget needs approval or not.
            if (ModuleName == ModuleNames.PMM)
            {
                if (pmmBudgetNeedsApproval)
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.PendingApproval);
                    _moduleBudget.BudgetAmount = 0;
                    _moduleBudget.UnapprovedAmount = double.Parse(txtBudgetAmountf.GetValue());
                }
                else
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.Approve);
                    _moduleBudget.BudgetAmount = double.Parse(txtBudgetAmountf.GetValue());
                    _moduleBudget.UnapprovedAmount = 0;
                }
            }
            if (Id > 0)
            {
                ModuleBudget budgetItem = ModuleBudgetManager.LoadById(Id, TicketId, ModuleName);
                ModuleBudget oldBudgetItem = ModuleBudgetManager.Clone(budgetItem);
                if (ModuleName == ModuleNames.PMM)
                {
                    UpdateAllocation(Id);
                }
                else
                {
                    _moduleBudget.ID = Id;
                    ModuleBudgetManager.InsertORUpdateData(_moduleBudget);
                    
                    if (ModuleName == ModuleNames.NPR)
                    {

                        ModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(oldBudgetItem, budgetItem);

                        BudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketId, ModuleName);

                        ModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(budgetItem.budgetCategory.ID);

                        BudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(budgetItem.budgetCategory.ID);
                    }
                }
                uHelper.ClosePopUpAndEndResponse(Context, true, "control=modulebudget");
            }
            else
            {
                ModuleBudgetManager.InsertORUpdateData(_moduleBudget);

            }


            // No need to update project/Non-project monthly distribution budget when budget will be in "Pending" mode.
            if (ModuleName == ModuleNames.NPR)
            {
                ////ModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(oldBudgetItem, budgetItem);

                //objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketId, ModuleName);



                //objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(_moduleBudget.BudgetLookup, TicketId);
            }
            //update monthly distribution budget 
            if (_moduleBudget != null)
                ModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(_moduleBudget.BudgetCategoryLookup, TicketId);

            if (ModuleName == ModuleNames.PMM || ModuleName == ModuleNames.ITG)
            {
                if (pmmBudgetNeedsApproval)
                {
                    // Update Monthly distribution in Itg Monthly budget list.
                    //objModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(_moduleBudget.BudgetLookup);

                    // Update Monthly distribution in Itg Monthly budget list.u
                    ModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(null, _moduleBudget);
                }
                else
                {
                    // Send mail notification to approver as inserted budget is in pending approval.
                    //SendMailToBudgetApprover(pmmBudget);
                }
                // Make an history entry
                DataTable dtPmm = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                DataRow drpmmitem = dtPmm.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == TicketId).FirstOrDefault();
                //SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);
                string historyTxt = string.Format("Budget item {0} Added", _moduleBudget.BudgetItem);
                uHelper.CreateHistory(ApplicationContext.CurrentUser, historyTxt, drpmmitem, ApplicationContext);

            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnBudgetSave_Click(object sender, EventArgs e)
        {
            ModuleBudget _moduleBudget = new ModuleBudget();
            double amount;
            TicketId = TicketId.ToUpper();
            _moduleBudget.TicketId = TicketId;
            _moduleBudget.ModuleName = ModuleName;
            if (txtBudgetAmountf != null && double.TryParse(txtBudgetAmountf.GetValue(), out amount))
                _moduleBudget.BudgetAmount = amount;
            if (txtBudgetDescription != null)
                _moduleBudget.BudgetDescription = txtBudgetDescription.Text;
            if (txtBudgetItemVal != null)
            {
                _moduleBudget.BudgetItem = txtBudgetItemVal.Text;
                _moduleBudget.Title = txtBudgetItemVal.Text;
            }
            if (dtcBudgetStartDate.Date != null)
                _moduleBudget.AllocationStartDate = dtcBudgetStartDate.Date;

            if (dtcBudgetEndDate.Date != null)
                _moduleBudget.AllocationEndDate = dtcBudgetEndDate.Date;
            _moduleBudget.BudgetCategoryLookup = int.Parse(ddlBudgetCategories.SelectedValue);

            // Update the busget status according to setting whether budget needs approval or not.
            if (ModuleName == ModuleNames.PMM)
            {
                if (pmmBudgetNeedsApproval && _moduleBudget.BudgetStatus == (int)(Enums.BudgetStatus.PendingApproval))
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.PendingApproval);
                    _moduleBudget.BudgetAmount = 0;
                    _moduleBudget.UnapprovedAmount = double.Parse(txtBudgetAmountf.GetValue());
                }
                else
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.Approve);
                    _moduleBudget.BudgetAmount = double.Parse(txtBudgetAmountf.GetValue());
                    _moduleBudget.UnapprovedAmount = 0;
                }
            }

            // Add Attachments to item
            if(!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                _moduleBudget.Attachments = FileUploadControl.GetValue();
            if (Id > 0)
                ModuleBudgetManager.UpdateAllocation(Id, _moduleBudget, pmmBudgetNeedsApproval);
            else
                ModuleBudgetManager.NewAllocation(_moduleBudget, pmmBudgetNeedsApproval);

            uHelper.ClosePopUpAndEndResponse(Context, true, "control=modulebudget");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);

        }

        private void UpdateAllocation(int budgetID)
        {
            //int autoID = int.Parse(lvBudgetList.DataKeys[lvBudgetList.EditIndex].Value.ToString());
            ModuleBudget budgetItem = ModuleBudgetManager.LoadById(budgetID, TicketId, ModuleName);
            ModuleBudget oldBudgetItem = ModuleBudgetManager.Clone(budgetItem);

            if (budgetItem != null)
            {
                // Catch the old data of budget
                long oldSubCategoryID = budgetItem.BudgetCategoryLookup;
                if (budgetItem.budgetCategory != null && oldSubCategoryID == 0)
                    oldSubCategoryID = budgetItem.budgetCategory.ID;
                double oldAmount = budgetItem.BudgetAmount;
                DateTime oldStartDate = budgetItem.AllocationStartDate;
                DateTime oldEndDate = budgetItem.AllocationEndDate;
                string oldTitle = budgetItem.BudgetItem;

                // Set the new data of budget
                budgetItem.budgetCategory = new BudgetCategory();
                budgetItem.budgetCategory.ID = int.Parse(ddlBudgetCategories.SelectedValue);
                budgetItem.IsAutoCalculated = true;
                budgetItem.BudgetDescription = txtBudgetDescription.Text.Trim();
                budgetItem.Comment = txtBudgetComment.Text.Trim();
                budgetItem.BudgetItem = txtBudgetItemVal.Text.Trim();
                budgetItem.AllocationStartDate = dtcBudgetStartDate.Date;
                budgetItem.AllocationEndDate = dtcBudgetEndDate.Date;
                budgetItem.BudgetCategoryLookup = int.Parse(ddlBudgetCategories.SelectedValue);
                //budgetItem.budgetCategory = int.Parse(ddlBudgetCategories.SelectedValue);

                budgetItem.Title = budgetItem.BudgetItem;

                double newBudgetAmount = double.Parse(txtBudgetAmountf.GetValue());

                //bool sendApprovalNotification = false;
                bool enableMonthDistribution = false;
                //Enable monthly distribution of data if any one is changed
                if (oldBudgetItem.BudgetCategoryLookup != budgetItem.BudgetCategoryLookup || oldBudgetItem.AllocationStartDate.Date != budgetItem.AllocationStartDate.Date || oldBudgetItem.AllocationEndDate.Date != budgetItem.AllocationEndDate.Date)
                    enableMonthDistribution = true;

                double preUnapproveAmount = budgetItem.UnapprovedAmount;
                // Update the budget status as "Pending" when category, item name, budget amount, start date or end date is changed.
                #region BudgetNeedsApproval
                if (pmmBudgetNeedsApproval && budgetItem.ModuleName != ModuleNames.NPR)
                {
                    // If budget need approval then only approve budget need distribution in monthly list.
                    if (budgetItem.BudgetStatus == (int)Enums.BudgetStatus.Approve)
                    {
                        //Change the status of budget item.
                        if (newBudgetAmount > oldAmount)
                        {
                            budgetItem.UnapprovedAmount = newBudgetAmount - budgetItem.BudgetAmount;
                            budgetItem.BudgetStatus = (int)Enums.BudgetStatus.PendingApproval;
                            //sendApprovalNotification = true;
                        }
                        else if (newBudgetAmount == oldAmount)
                        {
                            budgetItem.UnapprovedAmount = 0;
                            budgetItem.BudgetAmount = newBudgetAmount;
                        }
                        else
                        {
                            budgetItem.UnapprovedAmount = 0;
                            budgetItem.BudgetAmount = newBudgetAmount;
                            enableMonthDistribution = true;
                        }
                    }
                    else if (budgetItem.BudgetStatus == (int)Enums.BudgetStatus.Reject)
                    {
                        budgetItem.UnapprovedAmount = newBudgetAmount;
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.PendingApproval;
                        if (budgetItem.UnapprovedAmount == 0)
                            budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Approve;
                    }
                    else if (budgetItem.BudgetStatus == (int)Enums.BudgetStatus.PendingApproval)
                    {
                        //Change the status of budget item.
                        budgetItem.UnapprovedAmount = newBudgetAmount;
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.PendingApproval;
                        if (budgetItem.UnapprovedAmount == 0)
                            budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Approve;
                    }
                }
                #endregion
                #region BudgetDoesNotNeedApproval
                else
                {
                    budgetItem.BudgetAmount = double.Parse(txtBudgetAmountf.GetValue());
                    budgetItem.UnapprovedAmount = 0;
                    if (budgetItem.BudgetAmount != oldBudgetItem.BudgetAmount)
                        enableMonthDistribution = true;
                }
                #endregion

                ModuleBudgetManager.InsertORUpdateData(budgetItem);

                if (enableMonthDistribution)
                {
                    // Make an history entry
                    DataRow[] drColl = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.PMMProjects, DatabaseObjects.Columns.TicketId, TicketId);
                    if (drColl != null && drColl.Length > 0)
                    {
                        DataRow drpmmitem = drColl.FirstOrDefault();
                        string historyTxt = string.Format("Budget item {0} Added", budgetItem.BudgetItem);
                        uHelper.CreateHistory(_context.CurrentUser, historyTxt, drpmmitem, _context);
                    }

                    // Revoke budget distribution of budget item and update new data
                    ModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(oldBudgetItem, budgetItem);

                    // Revoke all actuals distribution of budget item.
                    if (budgetItem.ModuleName != ModuleNames.NPR)
                        BudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketId, ModuleName);

                    if (oldBudgetItem.budgetCategory.ID != budgetItem.budgetCategory.ID)
                    {
                        // Update old subcategory monthly distribution.
                        ModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(oldSubCategoryID);
                        // Update old subcategory monthly distribution.
                        if (budgetItem.ModuleName != ModuleNames.NPR)
                            BudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(oldSubCategoryID);
                    }

                    // Update new subcategory monthly distribution if category is changed.
                    ModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(budgetItem.BudgetCategoryLookup);
                }
            }
        }

        private void NewAllocation()
        {
            ModuleBudget _moduleBudget = new ModuleBudget();
            _moduleBudget.budgetCategory = new BudgetCategory();
            _moduleBudget.budgetCategory.ID = int.Parse(ddlBudgetCategories.SelectedValue);
            _moduleBudget.BudgetItem = txtBudgetItemVal.Text;
            _moduleBudget.Title = txtBudgetItemVal.Text;
            _moduleBudget.IsAutoCalculated = true;
            _moduleBudget.TicketId = TicketId;
            _moduleBudget.BudgetDescription = txtBudgetDescription.Text.Trim();
            _moduleBudget.BudgetItem = txtBudgetItemVal.Text.Trim();
            _moduleBudget.AllocationStartDate = dtcBudgetStartDate.Date;
            _moduleBudget.AllocationEndDate = dtcBudgetEndDate.Date;
            _moduleBudget.Comment = txtBudgetComment.Text.Trim();
            _moduleBudget.BudgetCategoryLookup = int.Parse(ddlBudgetCategories.SelectedValue);
            _moduleBudget.ModuleName = ModuleName;
            double amount;
            if (txtBudgetAmountf != null && double.TryParse(txtBudgetAmountf.GetValue(), out amount))
            {
                _moduleBudget.BudgetAmount = amount;
            }
            // Update the busget status according to setting whether budget needs approval or not.
            if (ModuleName != ModuleNames.NPR)
            {
                if (pmmBudgetNeedsApproval)
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.PendingApproval);
                    _moduleBudget.BudgetAmount = 0;
                    _moduleBudget.UnapprovedAmount = double.Parse(txtBudgetAmountf.GetValue().Trim());
                }
                else
                {
                    _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.Approve);
                    _moduleBudget.BudgetAmount = double.Parse(txtBudgetAmountf.GetValue().Trim());
                    _moduleBudget.UnapprovedAmount = 0;
                }
            }
            ModuleBudgetManager.InsertORUpdateData(_moduleBudget);
            {
                // No need to update project/Non-project monthly distribution budget when budget will be in "Pending" mode.
                if (!pmmBudgetNeedsApproval)
                {
                    // Update Monthly distribution in Itg Monthly budget list.
                    ModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(_moduleBudget.BudgetCategoryLookup);



                    // Update Monthly distribution in Itg Monthly budget list.
                    ModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(null, _moduleBudget);
                }
                else
                {
                    // Send mail notification to approver as inserted budget is in pending approval.
                    //SendMailToBudgetApprover(_moduleBudget);
                }



                // Make an history entry
                DataRow[] drColl = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.PMMProjects, DatabaseObjects.Columns.TicketId, TicketId);
                if (drColl != null && drColl.Length > 0)
                {
                    DataRow drpmmitem = drColl.FirstOrDefault();
                    string historyTxt = string.Format("Budget item {0} Added", _moduleBudget.BudgetItem);
                    uHelper.CreateHistory(_context.CurrentUser, historyTxt, drpmmitem, _context);
                }
            }
        }
    }
}
