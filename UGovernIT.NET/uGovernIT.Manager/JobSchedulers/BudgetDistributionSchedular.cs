using DevExpress.CodeParser.Diagnostics;
using DevExpress.UnitConversion;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class BudgetDistributionSchedular : IJobScheduler
    {
        ApplicationContext _context;
        DataTable itgBudgetList = null;
        DataTable itgActualList = null;
        DataTable pmmBudgetList = null;
        DataTable pmmActualList = null;
        DataTable monthlyBudgetList = null;
        DataTable pmmMonthlyBudgetList = null;
        private bool pmmBudgetNeedsApproval = false;
        public string Duration { get; set; }

        public BudgetDistributionSchedular()
        {

        }

        public async Task Execute(string TenantID)//DistributeBudget
        {
            /*
             itgBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGBudget, spWeb);
            itgActualList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGActual, spWeb);
            pmmBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudget, spWeb);
            pmmActualList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudgetActuals, spWeb);
            monthlyBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget, spWeb);
            pmmMonthlyBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMMonthlyBudget, spWeb);
             */
            await Task.FromResult(0);
            _context = ApplicationContext.CreateContext(TenantID);
            pmmBudgetNeedsApproval = _context.ConfigManager.GetValueAsBool(ConfigConstants.PMMBudgetNeedsApproval);
            itgBudgetList = pmmBudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudget, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            itgActualList = pmmActualList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetActuals, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            monthlyBudgetList = pmmMonthlyBudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");


            // Delete all items from list ITGMonthlyBudget.
            //SPListHelper.DeleteAllSPListItems(_context, DatabaseObjects.Tables.ITGMonthlyBudget);
            GetTableDataManager.delete<int>(DatabaseObjects.Tables.ModuleMonthlyBudget, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG);
            // Load all budget categories for monthly distribution.
            BudgetCategoryViewManager objBudgetCategoryViewManager = new BudgetCategoryViewManager(_context);
            DataRow[] categories = objBudgetCategoryViewManager.GetDataTable().Select();

            foreach (DataRow categoryItem in categories)
            {
                int subCategoryID = 0;
                int.TryParse(Convert.ToString(categoryItem[DatabaseObjects.Columns.Id]), out subCategoryID);

                // Update ITG monthly distribution of Planned/Budget.
                UpdateBudgetMonthlyDistributionITG(subCategoryID, _context);

                // Update ITG monthly distribution of Actual.
                UpdateActualMonthlyDistributionITG(subCategoryID, _context);

                // Update PMM monthly distribution of Planned/Budget.
                UpdateBudgetMonthlyDistributionPMM(subCategoryID, _context);

                // Update PMM monthly distribution of Actual.
                UpdateActualMonthlyDistributionPMM(subCategoryID, _context);
            }

            UpdateProjectMonthlyDistribution(_context);
        }
        private void UpdateProjectMonthlyDistribution(ApplicationContext _context)
        {
            string ticketId = string.Empty;
            DataTable pmmProjectes = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            foreach (DataRow pmmProjectItem in pmmProjectes.Rows)
            {
                //Delete all monthly ditribution of pmm from pmm monthly budget list.
                ticketId = Convert.ToString(pmmProjectItem[DatabaseObjects.Columns.TicketId]);
                string pmmMonthlyBudgetQuery = string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.BudgetType, "0");
                DataRow[] pmmBudgetMonthlyCollection = pmmMonthlyBudgetList.Select(pmmMonthlyBudgetQuery);
                while (pmmBudgetMonthlyCollection.Count() > 0)
                {
                    pmmBudgetMonthlyCollection[0].Delete();
                }
                string budgetQuery1 = string.Empty;
                if (pmmBudgetNeedsApproval)
                    budgetQuery1 = string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve);
                else
                    budgetQuery1 = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId);

                DataRow[] pmmBudgetCollection = pmmBudgetList.Select(budgetQuery1);
                ModuleBudgetManager objModuleBudgetManager = new ModuleBudgetManager(_context);

                foreach (DataRow budgetItem in pmmBudgetCollection)
                {
                    int budgetItemID = 0;
                    int.TryParse(Convert.ToString(budgetItem[DatabaseObjects.Columns.Id]), out budgetItemID);
                    if (budgetItemID > 0)
                    {
                        ModuleBudget moduleBudget = objModuleBudgetManager.LoadById(budgetItemID, ticketId, "");
                        UpdateProjectMonthlyDistributionBudget(moduleBudget, ticketId);

                        DataRow[] pmmActualCollection = pmmActualList.Select(string.Format("", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.ModuleBudgetLookup, budgetItemID));
                        foreach (DataRow actualItem in pmmActualCollection)
                        {
                            UpdateProjectMonthlyDistributionActual(actualItem, ticketId);
                        }
                    }
                }
            }
        }
        private void UpdateBudgetMonthlyDistributionITG(int subCategoryID, ApplicationContext _context)
        {
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            // Get all budget items of old sub-category.
            string query = string.Format("{0} = {1} and {2} = '{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG);
            DataView dv = new DataView(itgBudgetList);
            dv.RowFilter = query;
            DataTable budgetTable = dv.ToTable();
            if (budgetTable != null && budgetTable.Rows.Count > 0)
                foreach (DataRow budgetRow in budgetTable.Rows)
                {
                    // Get the start date, end date and amount to distribute within start and end date.
                    DateTime startDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime endDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationEndDate];
                    double totalAmount = Convert.ToDouble(Convert.ToString(budgetRow[DatabaseObjects.Columns.BudgetAmount]));
                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);
                    // Before add/update the key check the items of the category is exist or not.
                    DataView _dv = new DataView(monthlyBudgetList);
                    _dv.RowFilter = query;
                    DataTable oldBudgetDistributionsTable = _dv.ToTable();
                    // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                    foreach (DateTime key in amountDistributions.Keys)
                    {
                        double val = amountDistributions[key];
                        DataRow preItem = null;
                        if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                            preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);

                        ModuleMonthlyBudget item = new ModuleMonthlyBudget();
                        double budgetTotal = 0.0;
                        if (preItem != null)
                        {

                            DataRow oldBudgetItem = monthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0]; //SPListHelper.GetSPListItem(pmmMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
                            double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.BudgetAmount]), out budgetTotal);
                            item = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                            if (item != null)
                            {
                                item.BudgetAmount = budgetTotal + val;
                                item.ModuleName = ModuleNames.ITG;
                            }
                        }
                        else
                        {
                            item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                            item.EstimatedCost = 0;
                            item.ResourceCost = 0;
                            item.ActualCost = 0;
                            item.BudgetType = "";
                            item.BudgetCategoryLookup = subCategoryID;
                            item.NonProjectPlanedTotal = val;
                            item.NonProjectActualTotal = 0;
                            item.ProjectActualTotal = 0;
                            item.ProjectPlanedTotal = 0;
                            item.ModuleName = ModuleNames.ITG;
                        }

                        try
                        {
                            if (item != null)
                                objModuleMonthlyBudgetManager.InsertORUpdateData(item);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating ITG Budget monthly totals for: " + budgetRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
        }
        private void UpdateActualMonthlyDistributionITG(int subCategoryID, ApplicationContext _context)
        {
            //Get all budget items of old sub-category.
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            // Get all budget items of old sub-category.
            string query = string.Format("{0} = {1} and {2} = '{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG);
            DataView dv = new DataView(itgBudgetList);
            dv.RowFilter = query;
            DataTable budgetTable = dv.ToTable();

            //distribute budget amount for old sub category
            if (budgetTable != null && budgetTable.Rows.Count > 0)
            {

                foreach (DataRow budgetRow in budgetTable.Rows)
                {
                    //Get budgets of old sub category
                    query = string.Format("{0} = {1}", DatabaseObjects.Columns.ModuleBudgetLookup, budgetRow[DatabaseObjects.Columns.ID]);
                    DataView _dv = new DataView(itgActualList);
                    _dv.RowFilter = query;
                    DataTable actualTable = _dv.ToTable();
                    //distribute budget amount for old sub category
                    if (actualTable != null && actualTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in actualTable.Rows)
                        {
                            // Get the start date, end date and amount to distribute within start and end date.
                            DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                            DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                            double totalAmount = Convert.ToDouble(Convert.ToString(row[DatabaseObjects.Columns.ActualCost]));

                            // Distribute the amount within specified dates and get the result in month and amount format.
                            Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                            //Get previous monthly distribution of subcategory and set it to 0.
                            DataView dv_ = new DataView(monthlyBudgetList);
                            dv_.RowFilter = string.Format("{0} = {1} and {2} = '{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG);
                            DataTable oldBudgetDistributionsTable = dv_.ToTable();

                            // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                            foreach (DateTime key in amountDistributions.Keys)
                            {
                                double val = amountDistributions[key];
                                DataRow preItem = null;
                                if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                                    preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);

                                ModuleMonthlyBudget item = new ModuleMonthlyBudget();// SPListItem item = null;
                                double actualTotal = 0.0;
                                if (preItem != null)
                                {
                                    DataRow oldBudgetItem = monthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0]; //SPListHelper.GetSPListItem(pmmMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
                                    double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.NonProjectActualTotal]), out actualTotal);
                                    item = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                                    if (item != null)
                                    {
                                        item.NonProjectActualTotal = actualTotal + val;
                                        item.ModuleName = ModuleNames.ITG;
                                    }
                                }
                                else
                                {
                                    item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                    item.EstimatedCost = 0;
                                    item.ResourceCost = 0;
                                    item.ActualCost = 0;
                                    item.BudgetType = "0";
                                    item.BudgetCategoryLookup = subCategoryID;
                                    item.NonProjectPlanedTotal = 0;
                                    item.NonProjectActualTotal = val;
                                    item.ProjectActualTotal = 0;
                                    item.ProjectPlanedTotal = 0;
                                    item.ModuleName = ModuleNames.ITG;
                                }

                                try
                                {
                                    if (item != null)
                                        objModuleMonthlyBudgetManager.InsertORUpdateData(item);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error updating ITG Actual monthly totals for: " + row[DatabaseObjects.Columns.Title]);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void UpdateBudgetMonthlyDistributionPMM(int subCategoryID, ApplicationContext _context)
        {
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            // Get all budget items of old sub-category.
            string bQuery = string.Empty;
            if (pmmBudgetNeedsApproval)
                bQuery = string.Format("{0} = {1} and {2}={3} and {4} ='{5}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM);
            else
                bQuery = string.Format("{0} = {1} and {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM);

            DataView dv = new DataView(itgBudgetList);
            dv.RowFilter = bQuery;
            DataTable budgetTable = dv.ToTable();
            //distribute budget amount for old sub category
            if (budgetTable != null && budgetTable.Rows.Count > 0)
            {
                foreach (DataRow budgetRow in budgetTable.Rows)
                {
                    DateTime startDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime endDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationEndDate];
                    double totalAmount = Convert.ToDouble(Convert.ToString(budgetRow[DatabaseObjects.Columns.BudgetAmount]));

                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);
                    //Get previous monthly distribution of subcategory and set it to 0.
                    // Get the data table object of old distribution collection.
                    bQuery = string.Format("{0} = {1} and {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM);
                    DataView dv_ = new DataView(monthlyBudgetList);
                    dv_.RowFilter = bQuery;
                    DataTable oldBudgetDistributionsTable = dv_.ToTable();

                    // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                    foreach (DateTime key in amountDistributions.Keys)
                    {
                        double val = UGITUtility.StringToDouble(amountDistributions[key]);
                        DataRow preItem = null;
                        if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                        {
                            preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                        }

                        ModuleMonthlyBudget item = new ModuleMonthlyBudget(); //SPListItem item = null;
                        double total = 0.0;
                        if (preItem != null)
                        {
                            DataRow oldBudgetItem = monthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                            item.BudgetCategoryLookup = subCategoryID;
                            double.TryParse(Convert.ToString(item.ProjectPlanedTotal), out total);
                            item = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                            if (item != null)
                                item.ProjectPlanedTotal = total + val;
                        }
                        else
                        {
                            item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                            item.EstimatedCost = 0;
                            item.ResourceCost = 0;
                            item.ActualCost = 0;
                            item.BudgetType = "0";
                            item.BudgetCategoryLookup = subCategoryID;
                            item.NonProjectPlanedTotal = 0;
                            item.NonProjectActualTotal = 0;
                            item.ProjectPlanedTotal = val;
                            item.ProjectActualTotal = 0;
                        }

                        try
                        {
                            objModuleMonthlyBudgetManager.InsertORUpdateData(item);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating PMM Budget monthly totals for: " + budgetRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
            }
        }
        private void UpdateActualMonthlyDistributionPMM(int subCategoryID, ApplicationContext _context)
        {
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            //Get all budget items of old sub-category.
            string bQuery = string.Empty;
            try
            {
                if (pmmBudgetNeedsApproval)
                    bQuery = string.Format("{0} = {1} and {2}={3}", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve);
                else
                    bQuery = string.Format("{0} = {1} and {2}={3}", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID);

                DataView dv = new DataView(pmmBudgetList);
                dv.RowFilter = bQuery;
                DataTable budgetTable = dv.ToTable();

                //distribute budget amount for old sub category
                if (budgetTable != null && budgetTable.Rows.Count > 0)
                {
                    foreach (DataRow budgetRow in budgetTable.Rows)
                    {
                        //Get budgets of old sub category
                        DataView dvpmmActualList = new DataView(pmmActualList);
                        dvpmmActualList.RowFilter = string.Format("{0} = {1} and {2} = '{3}'", DatabaseObjects.Columns.ModuleBudgetLookup, budgetRow[DatabaseObjects.Columns.ID], DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM);
                        DataTable actualTable = dvpmmActualList.ToTable();

                        //distribute budget amount for old sub category
                        if (actualTable != null && actualTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in actualTable.Rows)
                            {
                                // Get the start date, end date and amount to distribute within start and end date.
                                DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                                DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                                double totalAmount = Convert.ToDouble(Convert.ToString(row[DatabaseObjects.Columns.BudgetAmount]));

                                // Distribute the amount within specified dates and get the result in month and amount format.
                                Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                                //Get previous monthly distribution of subcategory and set it to 0.
                                string query = string.Empty;
                                bQuery = string.Format("{0} = {1}", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID);
                                DataView dv_ = new DataView(monthlyBudgetList);
                                dv_.RowFilter = bQuery;
                                DataTable oldBudgetDistributionsTable = dv_.ToTable();
                                // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                                ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                                foreach (DateTime key in amountDistributions.Keys)
                                {
                                    double val = amountDistributions[key];
                                    DataRow preItem = null;
                                    if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                                    {
                                        preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                                    }

                                    double total = 0.0;
                                    if (preItem != null)
                                    {
                                        DataRow oldBudgetItem = monthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                                        double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.ProjectActualTotal]), out total);
                                        objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                                        if (objModuleMonthlyBudget != null)
                                        {
                                            objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                                            objModuleMonthlyBudget.ProjectActualTotal = total + val;
                                        }
                                    }
                                    else
                                    {
                                        objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                        objModuleMonthlyBudget.ProjectActualTotal = val;
                                        //objModuleMonthlyBudget.TicketId = ticketID;
                                        objModuleMonthlyBudget.BudgetAmount = 0;
                                        objModuleMonthlyBudget.BudgetType = "0";
                                        //objModuleMonthlyBudget.ModuleName = modulename;
                                        objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                                    }

                                    try
                                    {
                                        if (objModuleMonthlyBudget != null)
                                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex, "Error updating PMM Actual monthly totals for: " + budgetRow[DatabaseObjects.Columns.Title]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }

        }

        private void UpdateProjectMonthlyDistributionBudget(ModuleBudget newBudget, string ticketID)
        {

            // Query to get all distribution of current Project.
            //SPQuery pmmBudgetQuery = new SPQuery();
            //pmmBudgetQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, pmmId, DatabaseObjects.Columns.BudgetType, "0");
            //SPListItemCollection pmmBudgetCollection = pmmMonthlyBudgetList.GetItems(pmmBudgetQuery);
            string modulename = uHelper.getModuleNameByTicketId(ticketID);
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            DataTable moduleMonthlyBudgetList = objModuleMonthlyBudgetManager.GetDataTable();
            DataRow[] moduleBudgetCollection = moduleMonthlyBudgetList.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.BudgetType, "0"));
            DataTable budgetTable = null;
            if (moduleBudgetCollection.Count() > 0)
            {
                budgetTable = moduleBudgetCollection.CopyToDataTable();
            }

            // Update with new distribution
            if (newBudget != null)
            {
                double newBudgetAmount = newBudget.BudgetAmount;
                DateTime newStartDate = (DateTime)newBudget.AllocationStartDate;
                DateTime newEndDate = (DateTime)newBudget.AllocationEndDate;

                // Distribute the amount within specified dates and get the result in month and amount format.
                Dictionary<DateTime, double> newDistributions = uHelper.DistributeAmount(_context, newStartDate, newEndDate, newBudgetAmount);

                // Update with new distribution
                foreach (DateTime key in newDistributions.Keys)
                {
                    double val = newDistributions[key];
                    double oldValue = 0;
                    DataRow oldItem = null;

                    if (budgetTable != null && budgetTable.Rows.Count > 0)
                    {
                        oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                    }

                    if (oldItem != null)
                    {
                        DataRow oldBudgetItem = pmmMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID])))[0]; //SPListHelper.GetSPListItem(pmmMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
                        double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.BudgetAmount]), out oldValue);
                        ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                        try
                        {
                            if (objModuleMonthlyBudget != null)
                            {
                                objModuleMonthlyBudget.BudgetAmount = oldValue + val;
                                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                            }

                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating PMM Budget monthly totals.");
                        }
                    }
                    else
                    {
                        // SPListItem newMonthItem = pmmMonthlyBudgetList.Items.Add();
                        ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                        objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                        objModuleMonthlyBudget.BudgetAmount = val;
                        objModuleMonthlyBudget.TicketId = ticketID;
                        objModuleMonthlyBudget.ActualCost = 0;
                        objModuleMonthlyBudget.BudgetType = "0";
                        objModuleMonthlyBudget.ModuleName = modulename;
                        try
                        {
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating PMM Budget monthly totals.");
                        }
                    }
                }
            }
        }
        private void UpdateProjectMonthlyDistributionActual(DataRow newActualItem, string ticketID)
        {
            // Query get all monthly distribution of current Project.
            string modulename = uHelper.getModuleNameByTicketId(ticketID);
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);

            DataView dvpmmMonthlyBudgetList = new DataView(pmmMonthlyBudgetList);
            dvpmmMonthlyBudgetList.RowFilter = string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.BudgetType, "0");
            DataTable budgetTable = dvpmmMonthlyBudgetList.ToTable();
            // Update the monthly distribution in case new actual item added or old actual item updated.
            if (newActualItem != null)
            {
                DateTime newStartDate = (DateTime)newActualItem[DatabaseObjects.Columns.AllocationStartDate];
                DateTime newEndDate = (DateTime)newActualItem[DatabaseObjects.Columns.AllocationEndDate];
                double newBudgetAmount = Convert.ToDouble(newActualItem[DatabaseObjects.Columns.BudgetAmount]);

                // Distribute the amount within specified dates and get the result in month and amount format.
                Dictionary<DateTime, double> newDistributions = uHelper.DistributeAmount(_context, newStartDate, newEndDate, newBudgetAmount);

                // Update with new distribution
                foreach (DateTime key in newDistributions.Keys)
                {
                    double val = newDistributions[key];
                    double oldValue = 0;
                    DataRow oldItem = null;
                    ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                    if (budgetTable != null && budgetTable.Rows.Count > 0)
                    {
                        oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                    }

                    if (oldItem != null)
                    {
                        DataRow oldBudgetItem = pmmMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID])))[0];
                        double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.ActualCost]), out oldValue);
                        objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));

                        try
                        {
                            if (objModuleMonthlyBudget != null)
                            {
                                objModuleMonthlyBudget.ActualCost = oldValue + val;
                                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating PMM Actual monthly totals.");
                        }

                    }
                    else
                    {
                        objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                        objModuleMonthlyBudget.ActualCost = val;
                        objModuleMonthlyBudget.TicketId = ticketID;
                        objModuleMonthlyBudget.BudgetAmount = 0;
                        objModuleMonthlyBudget.BudgetType = "0";
                        objModuleMonthlyBudget.ModuleName = modulename;
                        try
                        {
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating PMM Actual monthly distribution.");
                        }
                    }
                }
            }



            //DataTable moduleMonthlyBudgetList = objModuleMonthlyBudgetManager.GetDataTable();
            //DataRow[] moduleBudgetCollection = moduleMonthlyBudgetList.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.BudgetType, "0"));
            //DataTable budgetTable = null;
            //if (moduleBudgetCollection.Count() > 0)
            //{
            //    budgetTable = moduleBudgetCollection.CopyToDataTable();
            //}
            //// Update the monthly distribution in case new actual item added or old actual item updated.
            //if (newActualItem != null)
            //{
            //    DateTime newStartDate = (DateTime)newActualItem[DatabaseObjects.Columns.AllocationStartDate];
            //    DateTime newEndDate = (DateTime)newActualItem[DatabaseObjects.Columns.AllocationEndDate];
            //    double newBudgetAmount = Convert.ToDouble(newActualItem[DatabaseObjects.Columns.BudgetAmount]);

            //    // Distribute the amount within specified dates and get the result in month and amount format.
            //    Dictionary<DateTime, double> newDistributions = uHelper.DistributeAmount(_context, newStartDate, newEndDate, newBudgetAmount);

            //    // Update with new distribution
            //    foreach (DateTime key in newDistributions.Keys)
            //    {
            //        double val = newDistributions[key];
            //        double oldValue = 0;
            //        DataRow oldItem = null;

            //        if (budgetTable != null && budgetTable.Rows.Count > 0)
            //        {
            //            oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
            //        }

            //        if (oldItem != null)
            //        {
            //            // SPListItem oldBudgetItem = SPListHelper.GetSPListItem(pmmMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
            //            DataRow oldBudgetItem = moduleMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID])))[0];
            //            double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.ActualCost]), out oldValue);
            //            //oldBudgetItem[DatabaseObjects.Columns.ActualCost] =
            //            ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
            //            try
            //            {
            //                objModuleMonthlyBudget.ActualCost = oldValue + val;
            //                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
            //                //oldBudgetItem.Update();

            //            }
            //            catch (Exception ex)
            //            {
            //                ULog.WriteException(ex, "Error updating PMM Actual monthly totals.");
            //            }

            //        }
            //        else
            //        {
            //            //SPListItem newMonthItem = pmmMonthlyBudgetList.Items.Add();
            //            //newMonthItem[DatabaseObjects.Columns.AllocationStartDate] = new DateTime(key.Year, key.Month, 1);
            //            //newMonthItem[DatabaseObjects.Columns.ActualCost] = val;
            //            //newMonthItem[DatabaseObjects.Columns.TicketPMMIdLookup] = pmmId;
            //            //newMonthItem[DatabaseObjects.Columns.BudgetAmount] = 0;
            //            //newMonthItem[DatabaseObjects.Columns.BudgetType] = "0";
            //            ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
            //            objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
            //            objModuleMonthlyBudget.ActualCost = val;
            //            objModuleMonthlyBudget.TicketId = ticketID;
            //            objModuleMonthlyBudget.BudgetAmount = 0;
            //            objModuleMonthlyBudget.BudgetType = "0";
            //            objModuleMonthlyBudget.ModuleName = modulename;

            //            try
            //            {
            //                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
            //                //  newMonthItem.Update();

            //            }
            //            catch (Exception ex)
            //            {
            //                ULog.WriteException(ex, "Error updating PMM Actual monthly distribution.");
            //            }
            //        }
            //    }
            //}
        }
    }
}
