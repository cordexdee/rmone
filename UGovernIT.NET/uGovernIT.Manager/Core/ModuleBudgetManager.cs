using DevExpress.CodeParser.Diagnostics;
using DevExpress.UnitConversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public interface IModuleBudgetManager : IManagerBase<ModuleBudget>
    {
        DataTable LoadModuleBudget();

        ModuleBudget GetBudgetByResourceId(int resourceId);

        ModuleBudget LoadById(long id, string TicketID, string ModuleName);

        void UpdateNonProjectMonthlyDistributionBudget(long subCategoryID);

        void UpdateNonProjectMonthlyDistributionBudget(long subCategoryID, string TicketID);

        void UpdateProjectMonthlyDistributionBudget(ModuleBudget oldBudget, ModuleBudget newBudget);

        ModuleBudget LoadItemObj(DataRow item);
    }

    public class ModuleBudgetManager : ManagerBase<ModuleBudget>, IModuleBudgetManager
    {
        public BudgetCategoryViewManager budgetCategory { get; set; }

        BudgetCategoryViewManager objBudgetCategoryViewManager;
        BudgetActualsManager objBudgetActualsManager;
        ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager;
        ApplicationContext _context;
        ModuleBudgetHistoryManager _moduleBudgetHistoryManager = null;
        ModuleBudgetActualsHistoryManager _moduleBudgetActualsHistoryManager = null;

        public ModuleBudgetManager(ApplicationContext context) : base(context)
        {
            _context = context;

            store = new ModuleBudgetStore(this.dbContext);

            objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(this.dbContext);

            objBudgetCategoryViewManager = new BudgetCategoryViewManager(this.dbContext);

            _moduleBudgetHistoryManager = new ModuleBudgetHistoryManager(this.dbContext);

            _moduleBudgetActualsHistoryManager = new ModuleBudgetActualsHistoryManager(this.dbContext);

            objBudgetActualsManager = new BudgetActualsManager(this.dbContext);

        }

        public DataTable LoadModuleBudget()
        {
            List<ModuleBudget> configModuleBudgetList = store.Load();
            return UGITUtility.ToDataTable<ModuleBudget>(configModuleBudgetList);
        }

        public DataTable LoadBudgetByTicketID(string TickedId, bool baseline = false, int baselineId = 0)
        {
            return Load(TickedId, true, baseline, baselineId);
        }

        public List<ModuleBudget> LoadBudgetByTicketId(string ticketId)
        {
            List<ModuleBudget> moduleBudgets = store.Load(x => x.TicketId == ticketId).ToList();
            return moduleBudgets;
        }

        public DataTable Load(string TickedId, bool updateTotalCost, bool baseline = false, int baselineId = 0)
        {
            DataTable result;

            DataRow[] budgets;

            string ModuleName = uHelper.getModuleNameByTicketId(TickedId);

            result = CreateTable();



            if (baseline)
            {
                budgets = _moduleBudgetHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{TickedId}' and {DatabaseObjects.Columns.BaselineId}={baselineId}").Select();
            }
            else
            {
                DataTable moduleBudget = LoadModuleBudget();

                budgets = moduleBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TickedId));

            }
            double total = 0;

            foreach (DataRow budget in budgets)
            {
                DataRow row = result.NewRow();
                foreach (DataColumn dc in row.Table.Columns)
                {
                    if (UGITUtility.IsSPItemExist(budget, dc.ColumnName))
                        row[dc.ColumnName] = budget[dc.ColumnName];
                }
                //check budget category for baseline

                BudgetCategory budgetCategory = objBudgetCategoryViewManager.GetBudgetCategoryById(int.Parse((budget[DatabaseObjects.Columns.BudgetCategoryLookup].ToString())));
                if (budgetCategory != null)
                {
                    row[DatabaseObjects.Columns.BudgetCategory] = budgetCategory.BudgetCategoryName;
                    row[DatabaseObjects.Columns.BudgetSubCategory] = budgetCategory.BudgetSubCategory;
                    row["BudgetSubCategoryID"] = budgetCategory.ID;
                }

                double budgetAmount = 0;
                double.TryParse(Convert.ToString(row[DatabaseObjects.Columns.BudgetAmount]), out budgetAmount);
                total += budgetAmount;
                if (ModuleName == "NPR")
                {
                    row["Total"] = total.ToString();
                }
                result.Rows.Add(row);
            }
            if (updateTotalCost)
            {
                SaveTotalCost(TickedId, total);
            }
            return result;
        }

        private void SaveTotalCost(string TickedId, double total)
        {
            //DataTable nprTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            //DataRow nprTicket = nprTable.Select(string.Format("{0}", TickedId))[0];
            //nprTicket[DatabaseObjects.Columns.TicketTotalCost] = total;
            string modulename = uHelper.getModuleNameByTicketId(TickedId);
            DataRow project = Ticket.GetCurrentTicket(dbContext, modulename, TickedId);
            if (project != null)
            {
                project[DatabaseObjects.Columns.TicketTotalCost] = total;
                Ticket ticketRequest = new Ticket(dbContext, modulename);
                ticketRequest.CommitChanges(project);
            }
        }

        private static DataTable CreateTable()
        {
            DataTable result = new DataTable("Budget");
            DataTable budgettable = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.ModuleBudget);
            if (budgettable != null)
                result = budgettable.Clone();
            result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            result.Columns.Add("BudgetSubCategoryID", typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            //result.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            //result.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            //result.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.BudgetStatus, typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.UnapprovedAmount, typeof(double));
            result.Columns.Add("Total", typeof(float));
            return result;
        }

        private static DataTable CreateBudgetSummaryTable()
        {
            DataTable result = new DataTable("BudgetSummary");
            result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.BudgetType, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetCOA, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(double));
            result.Columns.Add("ActualBudgetNotes", typeof(string));
            result.Columns.Add("EstimatedCostNotes", typeof(string));
            result.Columns.Add("BudgetCostNotes", typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Actuals, typeof(double));
            result.Columns.Add("ResourceCost", typeof(double));
            result.Columns.Add("Variance", typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));

            result.Columns.Add(DatabaseObjects.Columns.NonProjectPlannedTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.NonProjectActualTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ProjectPlannedTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ProjectActualTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.CapitalExpenditure, typeof(bool));
            return result;
        }

        private static DataTable CreateTablePMM()
        {
            DataTable result = new DataTable("Budget");
            result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            result.Columns.Add("BudgetSubCategoryID", typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsAutoCalculated, typeof(bool));
            result.Columns.Add(DatabaseObjects.Columns.BudgetStatus, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.UnapprovedAmount, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ModuleId, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.UGITComment, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.VendorName, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.VendorLookup, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.InvoiceNumber, typeof(double));
            result.Columns.Add("CommentTrail", typeof(string));
            return result;
        }

        public ModuleBudget GetBudgetByResourceId(int resourceId)
        {
            ModuleBudget nprBudget = new ModuleBudget();
            DataTable budgets = LoadModuleBudget();
            DataRow budget = null;
            if (budget != null)
            {
                nprBudget.AllocationStartDate = Convert.ToDateTime(budget[DatabaseObjects.Columns.AllocationStartDate]);
                nprBudget.AllocationEndDate = Convert.ToDateTime(budget[DatabaseObjects.Columns.AllocationEndDate]);
                nprBudget.BudgetItem = Convert.ToString(budget[DatabaseObjects.Columns.BudgetItem]);
                nprBudget.BudgetAmount = Convert.ToDouble(budget[DatabaseObjects.Columns.BudgetAmount]);
                nprBudget.BudgetDescription = Convert.ToString(budget[DatabaseObjects.Columns.BudgetDescription]);
                nprBudget.ID = Convert.ToInt16(budget[DatabaseObjects.Columns.Id]);
                nprBudget.budgetCategory = new BudgetCategory();
                nprBudget.budgetCategory.ID = Convert.ToInt32(budget[DatabaseObjects.Columns.BudgetCategoryLookup]);
            }
            return nprBudget;
        }

        public ModuleBudget LoadById(long id, string TicketID, string ModuleName)
        {
            ModuleBudget budgetItem = null;
            DataTable dtModuleBudget = LoadModuleBudget();
            DataRow[] moduleBudgets = dtModuleBudget.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, TicketID, DatabaseObjects.Columns.ID, id));
            if (moduleBudgets.Count() > 0)
            {
                budgetItem = LoadItemObj(moduleBudgets[0]);
            }
            return budgetItem;
        }

        public ModuleBudget LoadItemObj(DataRow item)
        {
            ModuleBudget budgetItem = null;
            if (item != null)
            {
                budgetItem = new ModuleBudget();
                budgetItem.ModuleName = Convert.ToString(item[DatabaseObjects.Columns.ModuleNameLookup]);
                budgetItem.ID = int.Parse(item[DatabaseObjects.Columns.Id].ToString());
                budgetItem.BudgetItem = item[DatabaseObjects.Columns.BudgetItem] != null ? item[DatabaseObjects.Columns.BudgetItem].ToString() : string.Empty;
                string budgetLookup = item[DatabaseObjects.Columns.BudgetCategoryLookup] != null ? UGITUtility.SplitString(item[DatabaseObjects.Columns.BudgetCategoryLookup].ToString(), Constants.Separator, 0) : string.Empty;
                int budgetCategoryId = 0;
                int.TryParse(budgetLookup, out budgetCategoryId);
                budgetItem.BudgetCategoryLookup = budgetCategoryId;
                budgetItem.budgetCategory = objBudgetCategoryViewManager.GetBudgetCategoryById(budgetCategoryId);   //BudgetCategoryViewManager.GetBudgetCategoryById(budgetCategoryId);
                budgetItem.BudgetDescription = item[DatabaseObjects.Columns.BudgetDescription] != null ? item[DatabaseObjects.Columns.BudgetDescription].ToString() : string.Empty;
                double budgetAmmount = 0;
                if (item[DatabaseObjects.Columns.BudgetAmount] != null)
                {
                    double.TryParse(item[DatabaseObjects.Columns.BudgetAmount].ToString(), out budgetAmmount);
                }
                budgetItem.BudgetAmount = budgetAmmount;
                budgetItem.AllocationStartDate = (DateTime)item[DatabaseObjects.Columns.AllocationStartDate];
                budgetItem.AllocationEndDate = (DateTime)item[DatabaseObjects.Columns.AllocationEndDate];
                budgetItem.TicketId = (UGITUtility.SplitString(item[DatabaseObjects.Columns.TicketId].ToString(), Constants.Separator, 0));
                budgetItem.BudgetStatus = (item[DatabaseObjects.Columns.BudgetStatus] == null ? 0 : int.Parse(item[DatabaseObjects.Columns.BudgetStatus].ToString()));

                double unapprovedAmount = 0;
                if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.UnapprovedAmount))
                {
                    double.TryParse(item[DatabaseObjects.Columns.UnapprovedAmount].ToString(), out unapprovedAmount);
                }
                budgetItem.UnapprovedAmount = unapprovedAmount;

                //    if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.UGITComment))
                //        budgetItem.CommentHistory = uHelper.GetHistory(Convert.ToString(item[DatabaseObjects.Columns.UGITComment]), false);

            }

            return budgetItem;
        }

        public void UpdateProjectMonthlyDistributionBudget(ModuleBudget oldBudget, ModuleBudget newBudget)
        {
            //int pmmID = 0;
            string TickedId = string.Empty;
            string ModuleName = string.Empty;
            if (oldBudget != null)
            {
                TickedId = oldBudget.TicketId;
                ModuleName = oldBudget.ModuleName;
            }
            else if (newBudget != null)
            {
                TickedId = newBudget.TicketId;
                ModuleName = newBudget.ModuleName;
            }
            // Open all three related list to get budget item actuals.
            //DataTable dtModuleBuget = LoadBudgetByTicketID(TickedId); //ModuleBudgetManager.LoadModuleBudget();
            DataTable dtMonthlyBudget = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
            DataRow[] drMonthlyBudgetColl = dtMonthlyBudget.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, TickedId, DatabaseObjects.Columns.BudgetType, "0"));
            DataTable budgetTable = null;
            if (drMonthlyBudgetColl.Count() > 0)
            {
                budgetTable = drMonthlyBudgetColl.CopyToDataTable();
            }

            if (oldBudget != null)
            {
                DateTime startDate = (DateTime)oldBudget.AllocationStartDate;
                DateTime endDate = (DateTime)oldBudget.AllocationEndDate;
                double totalAmount = 0;
                double.TryParse(Convert.ToString(oldBudget.BudgetAmount), out totalAmount);

                // Distribute the amount within specified dates and get the result in month and amount format.
                Dictionary<DateTime, double> oldDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                // Update with old distribution
                foreach (DateTime key in oldDistributions.Keys)
                {
                    double val = oldDistributions[key];
                    double oldValue = 0;
                    DataRow oldItem = null;
                    if (budgetTable != null && budgetTable.Rows.Count > 0)
                    {
                        oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                        if (oldItem != null)
                        {
                            ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt64(oldItem[DatabaseObjects.Columns.ID]));
                            double.TryParse(Convert.ToString(oldItem[DatabaseObjects.Columns.BudgetAmount]), out oldValue);
                            objModuleMonthlyBudget.BudgetAmount = val;
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                    }
                }
            }

            // Update with new distribution
            if (newBudget != null)
            {
                double newBudgetAmount = 0;
                double.TryParse(Convert.ToString(newBudget.BudgetAmount), out newBudgetAmount);
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
                        oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date
                        && x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup) == UGITUtility.ObjectToString(newBudget.BudgetCategoryLookup));
                    }

                    if (oldItem != null)
                    {
                        ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                        double.TryParse(Convert.ToString(objModuleMonthlyBudget.BudgetAmount), out oldValue);
                        objModuleMonthlyBudget.BudgetAmount = val;
                        objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                    }
                    else
                    {
                        ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                        objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                        objModuleMonthlyBudget.BudgetAmount = val;
                        objModuleMonthlyBudget.TicketId = TickedId;
                        objModuleMonthlyBudget.ModuleName = ModuleName;
                        objModuleMonthlyBudget.BudgetType = "0";
                        objModuleMonthlyBudget.BudgetCategoryLookup = newBudget.BudgetCategoryLookup;
                        objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                    }
                }
            }
        }

        public void UpdateNonProjectMonthlyDistributionBudget(long subCategoryID)
        {
            // Open all the related list to get budget item actuals.
            DataTable dtModuleBudget = LoadModuleBudget();
            DataTable dtMonthlyBudget = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            //Get all budget items of old sub-category.
            DataRow[] drModuleBudgetCollection = dtModuleBudget.Select(string.Format("{0}={1} And {2}={3} And {4}='{5}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM));
            if (drModuleBudgetCollection.Count() > 0)
            {
                dtModuleBudget = drModuleBudgetCollection.CopyToDataTable();

            }

            //Get previous monthly distribution of subcategory and set it to 0.
            DataRow[] droldBudgetDistributions = dtMonthlyBudget.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG));
            if (droldBudgetDistributions.Count() > 0)
            {
                dtMonthlyBudget = droldBudgetDistributions.CopyToDataTable();
            }

            // Get all distribution of currrent project;
            foreach (DataRow oldItem in droldBudgetDistributions)
            {
                ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                objModuleMonthlyBudget.ProjectPlanedTotal = 0;
                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
            }

            dtMonthlyBudget = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
            droldBudgetDistributions = dtMonthlyBudget.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG));
            DataTable dtMonthlyBudgetTable = new DataTable();
            if (droldBudgetDistributions != null && droldBudgetDistributions.Length > 0)
                dtMonthlyBudgetTable = droldBudgetDistributions.CopyToDataTable();
            //distribute budget amount for old sub category
            if (dtMonthlyBudgetTable != null && dtMonthlyBudgetTable.Rows.Count > 0)
            {
                foreach (DataRow budgetRow in dtMonthlyBudgetTable.Rows)
                {
                    DateTime startDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime endDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationEndDate];
                    double totalAmount = 0;
                    double.TryParse(Convert.ToString(budgetRow[DatabaseObjects.Columns.BudgetAmount]), out totalAmount);

                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                    // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                    foreach (DateTime key in amountDistributions.Keys)
                    {
                        double val = amountDistributions[key];
                        if (double.IsNaN(val))
                            val = 0;

                        DataRow preItem = null;
                        if (dtMonthlyBudget != null && dtMonthlyBudget.Rows.Count > 0)
                        {
                            preItem = dtMonthlyBudget.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                        }

                        DataRow item = null;
                        double total = 0.0;
                        if (preItem != null)
                        {
                            item = dtMonthlyBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                            double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ProjectPlannedTotal]), out total);
                            ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(preItem[DatabaseObjects.Columns.ID]));
                            objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                            objModuleMonthlyBudget.ProjectPlanedTotal = total + val;
                            objModuleMonthlyBudget.ModuleName = ModuleNames.ITG;
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                        else
                        {
                            ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                            objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                            objModuleMonthlyBudget.EstimatedCost = 0;
                            objModuleMonthlyBudget.ActualCost = 0;
                            objModuleMonthlyBudget.ResourceCost = 0;
                            objModuleMonthlyBudget.BudgetType = "0";
                            objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                            objModuleMonthlyBudget.NonProjectActualTotal = 0;
                            objModuleMonthlyBudget.NonProjectPlanedTotal = 0;
                            objModuleMonthlyBudget.ProjectActualTotal = 0;
                            objModuleMonthlyBudget.ProjectPlanedTotal = val;
                            objModuleMonthlyBudget.ModuleName = "ITG";
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                    }
                }
            }
        }

        public void UpdateNonProjectMonthlyDistributionBudget(long subCategoryID, string TicketID)
        {
            // Open all the related list to get budget item actuals.
            // SPList pmmBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudget);
            DataTable dtModuleBudget = LoadModuleBudget();  //ModuleBudgetManager.LoadModuleBudget();

            // SPList pmmActualList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudgetActuals);
            // SPList itgMonthlyBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget);
            DataTable dtMonthlyBudget = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();   //ModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            //Get all budget items of old sub-category.
            DataRow[] drModuleBudgetCollection = dtModuleBudget.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.TicketId, TicketID));
            if (drModuleBudgetCollection.Count() > 0)
            {
                dtModuleBudget = drModuleBudgetCollection.CopyToDataTable();

            }

            //Get previous monthly distribution of subcategory and set it to 0.
            DataRow[] droldBudgetDistributions = dtMonthlyBudget.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.TicketId, TicketID));
            if (droldBudgetDistributions.Count() > 0)
            {
                dtMonthlyBudget = droldBudgetDistributions.CopyToDataTable();
            }

            // Get all distribution of currrent project;
            foreach (DataRow oldItem in droldBudgetDistributions)
            {
                ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                objModuleMonthlyBudget.NonProjectPlanedTotal = 0;
                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);

                // oldItem[DatabaseObjects.Columns.ProjectPlannedTotal] = 0;
                //oldItem.Update();
            }
            //dtMonthlyBudget = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();   //ModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
            // Get the data table object of old distribution collection.
            //DataTable oldBudgetDistributionsTable = oldBudgetDistributions.GetDataTable();

            //distribute budget amount for old sub category
            if (dtModuleBudget != null && dtModuleBudget.Rows.Count > 0)
            {
                foreach (DataRow budgetRow in dtModuleBudget.Rows)
                {
                    DateTime startDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime endDate = (DateTime)budgetRow[DatabaseObjects.Columns.AllocationEndDate];
                    double totalAmount = 0;
                    double.TryParse(Convert.ToString(budgetRow[DatabaseObjects.Columns.BudgetAmount]), out totalAmount);

                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                    // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                    foreach (DateTime key in amountDistributions.Keys)
                    {
                        double val = amountDistributions[key];
                        if (double.IsNaN(val))
                            val = 0;

                        DataRow preItem = null;
                        if (dtMonthlyBudget != null && dtMonthlyBudget.Rows.Count > 0)
                        {
                            preItem = dtMonthlyBudget.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                        }

                        DataRow item = null;
                        double total = 0.0;
                        if (preItem != null)
                        {
                            item = dtMonthlyBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                            //item = SPListHelper.GetSPListItem(itgMonthlyBudgetList, (int)preItem[DatabaseObjects.Columns.Id]);
                            double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.NonProjectPlannedTotal]), out total);
                            ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(preItem[DatabaseObjects.Columns.ID]));
                            objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                            objModuleMonthlyBudget.NonProjectPlanedTotal = val;
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                        else
                        {
                            //ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                            //objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                            //objModuleMonthlyBudget.EstimatedCost = 0;
                            //objModuleMonthlyBudget.ActualCost = 0;
                            //objModuleMonthlyBudget.ResourceCost = 0;
                            //objModuleMonthlyBudget.BudgetType = "0";
                            //objModuleMonthlyBudget.BudgetLookup = subCategoryID;
                            //objModuleMonthlyBudget.NonProjectActualTotal = 0;
                            //objModuleMonthlyBudget.NonProjectPlanedTotal = 0;
                            //objModuleMonthlyBudget.ProjectActualTotal = 0;
                            //objModuleMonthlyBudget.ProjectPlanedTotal = val;
                            //objModuleMonthlyBudget.ModuleName = "ITG";
                            //// objModuleMonthlyBudget.TicketId = TicketId;
                            //objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                    }
                }
            }
        }

        public static ModuleBudget Clone(ModuleBudget obj)
        {
            ModuleBudget budgetItem = new ModuleBudget();
            budgetItem.ID = obj.ID;
            budgetItem.BudgetItem = obj.BudgetItem;
            budgetItem.budgetCategory = obj.budgetCategory;
            budgetItem.BudgetDescription = obj.BudgetDescription;
            budgetItem.BudgetAmount = obj.BudgetAmount;
            budgetItem.ModuleName = obj.ModuleName;

            budgetItem.AllocationStartDate = obj.AllocationStartDate;
            budgetItem.AllocationEndDate = obj.AllocationEndDate;
            budgetItem.TicketId = obj.TicketId;
            budgetItem.BudgetStatus = obj.BudgetStatus;
            budgetItem.UnapprovedAmount = obj.UnapprovedAmount;
            return budgetItem;
        }

        public ModuleBudget Save(ModuleBudget obj)
        {
            ModuleBudget budgetItem = new ModuleBudget();
            budgetItem.AllocationStartDate = obj.AllocationStartDate;
            budgetItem.AllocationEndDate = obj.AllocationEndDate;
            budgetItem.BudgetItem = obj.BudgetItem;
            budgetItem.BudgetAmount = obj.BudgetAmount;
            budgetItem.BudgetDescription = obj.BudgetDescription;
            budgetItem.BudgetCategoryLookup = obj.budgetCategory.ID;
            budgetItem.IsAutoCalculated = obj.IsAutoCalculated;
            budgetItem.BudgetStatus = obj.BudgetStatus;
            budgetItem.UnapprovedAmount = obj.UnapprovedAmount;
            budgetItem.ModuleName = obj.ModuleName;
            budgetItem.TicketId = obj.TicketId;
            budgetItem.budgetCategory = objBudgetCategoryViewManager.GetBudgetCategoryById(obj.budgetCategory.ID); // BudgetCategoryViewManager.GetBudgetCategoryById(obj.budgetCategory.ID);
            budgetItem.budgetCategory.BudgetCategoryName = obj.budgetCategory.BudgetCategoryName;
            budgetItem.budgetCategory.BudgetSubCategory = obj.budgetCategory.BudgetSubCategory;
            budgetItem.ID = obj.ID;
            budgetItem.Title = obj.BudgetItem;
            budgetItem.Comment = obj.Comment;
            // ModuleBudgetManager.InsertORUpdateData(budgetItem);
            InsertORUpdateData(budgetItem);
            //if (budgetItem.Comment)
            //{
            //    if (obj.Comment != null && obj.Comment.Trim() != string.Empty)
            //    {
            //        string comment = uHelper.GetVersionString("", obj.Comment, item, DatabaseObjects.Columns.UGITComment);
            //        item[DatabaseObjects.Columns.UGITComment] = comment;
            //        this.CommentHistory = uHelper.GetHistory(comment, false);
            //    }
            //}
            return budgetItem;

        }

        public ModuleBudget InsertORUpdateData(ModuleBudget objModuleBudgetStore)
        {
            return (store as ModuleBudgetStore).InsertORUpdateData(objModuleBudgetStore);
        }

        public DataTable GetPMMBudgetActualList(string TicketID, int baselineId = 0, bool isBaseline = false)
        {
            objBudgetActualsManager = new BudgetActualsManager(_context);

            DataRow[] drActualsCollection;

            //BudgetActual baseline

            if (isBaseline)
            {
                drActualsCollection = _moduleBudgetActualsHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{TicketID}' and {DatabaseObjects.Columns.BaselineId}={baselineId} ").Select();
            }
            else
            {
                var dtProjuctActuals = objBudgetActualsManager.LoadModuleBudgetActuals();

                drActualsCollection = dtProjuctActuals.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID));
            }

            if (drActualsCollection == null || drActualsCollection.Length == 0)
                return null;
            return drActualsCollection.CopyToDataTable();
        }

        public float AutoCalculateBudgetMonitorState(string TicketID, string ModuleName)
        {

            var dtMonitors = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");    //SPListHelper.GetDataTable(DatabaseObjects.Lists.ModuleMonitors);
            DataRow drMonitor = dtMonitors.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ProjectMonitorName) == "On Budget").FirstOrDefault();
            int onTimeMonitorId = Convert.ToInt32(drMonitor[DatabaseObjects.Columns.Id]);
            DataTable dtProjectMonitors = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            DataRow[] projectMoniterStateCollection = dtProjectMonitors.Select(string.Format("{0}={1} And {2}={3}", DatabaseObjects.Columns.TicketId, TicketID, DatabaseObjects.Columns.ModuleMonitorNameLookup, onTimeMonitorId));
            DataRow monitorState = null;

            //  SPListItemCollection spitemcollMonitorState = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ProjectMonitorState, spQuery);
            if (projectMoniterStateCollection.Count() > 0)
            {
                monitorState = projectMoniterStateCollection[0];
                if (Convert.ToBoolean(monitorState[DatabaseObjects.Columns.AutoCalculate]))
                {
                    DataTable monitoroptions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitorOptions, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                    DataRow[] monitorsoptionsCollection = monitoroptions.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleMonitorNameLookup, onTimeMonitorId));
                    DataTable budgetActuals = GetPMMBudgetActualList(TicketID);
                    DataTable budgets = LoadBudgetByTicketID(TicketID);
                    var totalactualamount = budgetActuals.AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount));
                    var totalbudgetamount = budgets.AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount));
                    double budgetVariance;
                    if (totalbudgetamount > 0)
                    {
                        budgetVariance = ((totalactualamount - totalbudgetamount) / totalbudgetamount) * 100;
                    }
                    else
                    {
                        budgetVariance = 0;
                    }

                    if (budgetVariance < 10)
                    {
                        DataRow row = monitoroptions.AsEnumerable().FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.ModuleMonitorMultiplier) == 100);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                    }
                    else if (budgetVariance >= 10 && budgetVariance <= 20)
                    {
                        DataRow row = monitoroptions.AsEnumerable().FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.ModuleMonitorMultiplier) == 50);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                    }
                    else if (budgetVariance > 20)
                    {
                        DataRow row = monitoroptions.AsEnumerable().FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.ModuleMonitorMultiplier) == 0);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                        monitorState[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = Convert.ToInt32(row[DatabaseObjects.Columns.Id]);
                    }
                    //SPWeb thisWeb = SPContext.Current.Web;
                    //thisWeb.AllowUnsafeUpdates = true;
                    //  monitorState.Update();
                    TicketDal.SaveTickettemp(monitorState, DatabaseObjects.Tables.ProjectMonitorState, false);
                }
            }
            return 0;
        }

        public void NewAllocation(ModuleBudget moduleBudget, bool pmmBudgetNeedsApproval = false)
        {
            ModuleBudget _moduleBudget = new ModuleBudget();
            _moduleBudget.budgetCategory = new BudgetCategory();
            _moduleBudget.budgetCategory.ID = moduleBudget.BudgetCategoryLookup;
            _moduleBudget.BudgetItem = moduleBudget.BudgetItem;
            _moduleBudget.Title = moduleBudget.Title;
            _moduleBudget.IsAutoCalculated = true;
            _moduleBudget.TicketId = moduleBudget.TicketId;
            _moduleBudget.BudgetDescription = moduleBudget.BudgetDescription;
            _moduleBudget.BudgetItem = moduleBudget.BudgetItem;
            _moduleBudget.AllocationStartDate = moduleBudget.AllocationStartDate;
            _moduleBudget.AllocationEndDate = moduleBudget.AllocationEndDate;
            _moduleBudget.Comment = moduleBudget.Comment;
            _moduleBudget.BudgetCategoryLookup = moduleBudget.BudgetCategoryLookup;
            _moduleBudget.ModuleName = moduleBudget.ModuleName;
            _moduleBudget.Attachments = moduleBudget.Attachments;
            // Update the busget status according to setting whether budget needs approval or not.
            if (moduleBudget.ModuleName == ModuleNames.PMM && pmmBudgetNeedsApproval)
            {
                _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.PendingApproval);
                _moduleBudget.UnapprovedAmount = moduleBudget.BudgetAmount;
                _moduleBudget.BudgetAmount = 0;
            }
            else
            {
                _moduleBudget.BudgetStatus = (int)(Enums.BudgetStatus.Approve);
                _moduleBudget.BudgetAmount = moduleBudget.BudgetAmount;
                _moduleBudget.UnapprovedAmount = 0;
            }

            //Insert into Module Budget
            InsertORUpdateData(_moduleBudget);

            // No need to update project/Non-project monthly distribution budget when budget will be in "Pending" mode.
            if (!pmmBudgetNeedsApproval)
            {
                // Update Monthly distribution in Itg Monthly budget list.
                UpdateNonProjectMonthlyDistributionBudget(_moduleBudget.budgetCategory.ID);

                // Update Monthly distribution in Itg Monthly budget list.
                UpdateProjectMonthlyDistributionBudget(null, _moduleBudget);
            }
            else
            {
                // Send mail notification to approver as inserted budget is in pending approval.
                SendMailToBudgetApprover(_moduleBudget);
            }

            // Make an history entry
            DataRow[] drColl = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.PMMProjects, DatabaseObjects.Columns.TicketId, _moduleBudget.TicketId);
            if (drColl != null && drColl.Length > 0)
            {
                DataRow drpmmitem = drColl.FirstOrDefault();
                string historyTxt = string.Format("Budget item {0} Added", _moduleBudget.BudgetItem);
                uHelper.CreateHistory(_context.CurrentUser, historyTxt, drpmmitem, _context);
            }
        }
        private void SendMailToBudgetApprover(ModuleBudget budgetItem)
        {
            try
            {
                ModuleViewManager ObjmoduleViewManager = new ModuleViewManager(_context);
                ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(_context);
                UserProfileManager ObjUserProfileManager = new UserProfileManager(_context);
                UGITModule moduleDetail = ObjmoduleViewManager.LoadByName("PMM");
                string currentModuleListPagePath = UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath);
                string landingHomePage = UGITUtility.GetAbsoluteURL(uHelper.GetHomeUrlByUser(_context, _context.CurrentUser));

                //Get all unique userEmails
                StringBuilder mailTo = new StringBuilder();
                StringBuilder mailToNames = new StringBuilder();
                string pmmBudgetApprover = ObjConfigurationVariableManager.GetValue(ConfigConstants.PMMBudgetApprover);
                List<UserProfile> emailUsers = ObjUserProfileManager.GetUserInfosById(pmmBudgetApprover);

                foreach (UserProfile user in emailUsers)
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        if (mailTo.Length != 0)
                            mailTo.Append(";");
                        mailTo.Append(user.Email);
                        if (mailToNames.Length != 0)
                            mailToNames.Append(", ");
                        mailToNames.Append(user.Name);
                    }
                }

                //string emailSubject = "Budget Approval";
                string emailLink = string.Empty;

                string emailBody = string.Empty;
                if (!string.IsNullOrEmpty(mailTo.ToString()))
                {
                    //int tabNo = uHelper.GetPMMBudgetTabNumber(_context, ModuleNames.PMM);

                    //string url = landingHomePage + "?TicketId=" + pmmTicketId + "&showTab=" + tabNo + "&ModuleName=" + ModuleNames.PMM;
                    //emailBody = "Hi " + mailToNames.ToString() + "<br /><br />";
                    //emailBody += "The following project has a budget line item that has been changed/added and is pending your review/approval:<br />";
                    //emailBody += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<a href='{2}'>{0}</a>: {1}<br /><br />", pmmTicketId, pmmItem[DatabaseObjects.Columns.Title], url);
                    //emailBody += string.Format("Budget Line Item is:<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href='{4}'>{0}</a> from  <b>{1}</b> to  <b>{2}</b>, Unapproved Amount: <b>{3}</b>",
                    //    budgetItem.BudgetItem, uHelper.GetDateStringInFormat(budgetItem.StartDate, false),
                    //    uHelper.GetDateStringInFormat(budgetItem.EndDate, false), string.Format("{0:c}", budgetItem.UnapprovedAmount), url);
                    //emailBody += string.Format("<br /><br />{0}", ConfigurationVariable.GetValue(ConfigConstants.Signature));

                    //MailMessenger mail = new MailMessenger();
                    //mail.SendMail(mailTo.ToString(), emailSubject, string.Empty, emailBody, true, SPContext.Current.Web, new string[] { }, true);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "PMM Budget : could not send mail");
            }
        }
        public void UpdateAllocation(long budgetID, ModuleBudget moduleBudget, bool pmmBudgetNeedsApproval)
        {
            moduleBudget.budgetCategory = objBudgetCategoryViewManager.GetBudgetCategoryById(moduleBudget.BudgetCategoryLookup);
            ModuleBudget budgetItem = moduleBudget; // LoadById(budgetID, moduleBudget.TicketId, moduleBudget.ModuleName);
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
                budgetItem.ID = budgetID;
                budgetItem.budgetCategory = new BudgetCategory();
                budgetItem.budgetCategory.ID = moduleBudget.BudgetCategoryLookup;
                budgetItem.IsAutoCalculated = true;
                budgetItem.BudgetDescription = moduleBudget.BudgetDescription;
                budgetItem.Comment = moduleBudget.Comment;
                budgetItem.BudgetItem = moduleBudget.BudgetItem;
                budgetItem.AllocationStartDate = moduleBudget.AllocationStartDate;
                budgetItem.AllocationEndDate = moduleBudget.AllocationEndDate;
                budgetItem.BudgetCategoryLookup = moduleBudget.BudgetCategoryLookup;
                budgetItem.Attachments = moduleBudget.Attachments;
                //budgetItem.budgetCategory = int.Parse(ddlBudgetCategories.SelectedValue);

                budgetItem.Title = budgetItem.BudgetItem;

                double newBudgetAmount = moduleBudget.BudgetAmount;

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
                        //budgetItem.UnapprovedAmount = b;
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.PendingApproval;
                        if (budgetItem.UnapprovedAmount == 0)
                            budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Approve;
                    }
                }
                #endregion
                #region BudgetDoesNotNeedApproval
                else
                {
                    budgetItem.BudgetAmount = moduleBudget.BudgetAmount;
                    budgetItem.UnapprovedAmount = 0;
                    if (budgetItem.BudgetAmount != oldBudgetItem.BudgetAmount)
                        enableMonthDistribution = true;
                }
                #endregion

                InsertORUpdateData(budgetItem);

                if (enableMonthDistribution)
                {
                    // Make an history entry
                    DataRow[] drColl = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.PMMProjects, DatabaseObjects.Columns.TicketId, moduleBudget.TicketId);
                    if (drColl != null && drColl.Length > 0)
                    {
                        DataRow drpmmitem = drColl.FirstOrDefault();
                        string historyTxt = string.Format("Budget item {0} Added", budgetItem.BudgetItem);
                        uHelper.CreateHistory(_context.CurrentUser, historyTxt, drpmmitem, _context);
                    }

                    // Revoke budget distribution of budget item and update new data
                    UpdateProjectMonthlyDistributionBudget(oldBudgetItem, budgetItem);

                    // Revoke all actuals distribution of budget item.
                    if (budgetItem.ModuleName != ModuleNames.NPR)
                        if (objBudgetActualsManager != null)
                            objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(moduleBudget.TicketId, moduleBudget.ModuleName);

                    if (oldBudgetItem.budgetCategory.ID != budgetItem.budgetCategory.ID && budgetItem.ModuleName != ModuleNames.NPR)
                    {
                        // Update old subcategory monthly distribution.
                        //UpdateNonProjectMonthlyDistributionBudget(oldSubCategoryID);
                        // Update old subcategory monthly distribution.
                        //objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(oldSubCategoryID);
                    }

                    // Update new subcategory monthly distribution if category is changed.
                    //UpdateNonProjectMonthlyDistributionBudget(budgetItem.BudgetCategoryLookup);
                }
            }
        }

        /// <summary>
        /// Import budget
        /// </summary>
        /// <param name="fromTicketId"></param>
        /// <param name="toTicketId"></param>
        public void ImportBudgets(string fromTicketId, string toTicketId)
        {
            List<ModuleBudget> lstOfModuleBudget = LoadBudgetByTicketId(fromTicketId);
            string moduleName = uHelper.getModuleNameByTicketId(toTicketId);
            List<string> errors = new List<string>();
            ModuleBudget newModuleBudget = null;
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            bool pmmBudgetNeedsApproval = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.PMMBudgetNeedsApproval);
            double budgetedCost = 0;

            if (lstOfModuleBudget != null && lstOfModuleBudget.Count > 0)
            {
                foreach (ModuleBudget moduleBudget in lstOfModuleBudget)
                {
                    newModuleBudget = new ModuleBudget();
                    newModuleBudget.ModuleName = moduleName;
                    newModuleBudget.TicketId = toTicketId;
                    newModuleBudget.AllocationStartDate = moduleBudget.AllocationStartDate;
                    newModuleBudget.AllocationEndDate = moduleBudget.AllocationEndDate;
                    newModuleBudget.BudgetItem = moduleBudget.BudgetItem;
                    newModuleBudget.BudgetDescription = moduleBudget.BudgetDescription;
                    newModuleBudget.BudgetCategoryLookup = moduleBudget.BudgetCategoryLookup;
                    newModuleBudget.BudgetAmount = moduleBudget.BudgetAmount;
                    newModuleBudget.IsAutoCalculated = false;
                    newModuleBudget.BudgetStatus = (int)(Enums.BudgetStatus.Approve);

                    budgetedCost += Convert.ToDouble(moduleBudget.BudgetAmount);

                    NewAllocation(newModuleBudget, false);
                }
            }

            Ticket ticket = new Ticket(_context, moduleName);
            DataRow currentTicket = Ticket.GetCurrentTicket(_context, moduleName, toTicketId);
            currentTicket[DatabaseObjects.Columns.TicketTotalCost] = budgetedCost;
            ticket.CommitChanges(currentTicket);
        }

        public List<DataTable> LoadBudgetSummary(DateTime startDate, DateTime endDate)
        {
            List<DataTable> resultTables = new List<DataTable>();
            DataTable result = CreateBudgetSummaryTable();

            //SPList budgetCategories = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);
            //SPList itgMonthluBudget = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget);

            //SPQuery currentUsersQuery = new SPQuery();
            //currentUsersQuery.Query = string.Format(@"<Where><Or><Or><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='User'>{1}</Value></Eq>
            //                              <Membership Type='CurrentUserGroups'><FieldRef Name='{0}' /></Membership></Or>
            //                              <IsNull><FieldRef Name='{0}' /></IsNull></Or></Where>", DatabaseObjects.Columns.AuthorizedToView, SPContext.Current.Web.CurrentUser.ID);

            List<BudgetCategory> budgetCategoriesColl = objBudgetCategoryViewManager.Load(x => x.AuthorizedToView.Contains(dbContext.CurrentUser.Id)); //.GetItems(currentUsersQuery);
            if (budgetCategoriesColl.Count == 0)
                budgetCategoriesColl = objBudgetCategoryViewManager.Load();
            foreach (BudgetCategory budgetCategory in budgetCategoriesColl)
            {
                //// Get the Sub-Category wise total from itgMontlhlyBudget list. 
                //SPQuery itgMonthlyItemQuery = new SPQuery();
                //itgMonthlyItemQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='true' /><Value Type='Lookup'>{1}</Value></Eq><And><Geq><FieldRef Name='{2}' /><Value Type='DateTime'>{3}</Value></Geq><Leq><FieldRef Name='{2}' /><Value Type='DateTime'>{4}</Value></Leq></And></And></Where>",
                //DatabaseObjects.Columns.BudgetLookup, budgetCategory[DatabaseObjects.Columns.Id].ToString(), DatabaseObjects.Columns.AllocationStartDate, SPUtility.CreateISO8601DateTimeFromSystemDateTime(startDate), SPUtility.CreateISO8601DateTimeFromSystemDateTime(endDate));
                List<ModuleMonthlyBudget> itgMonthlyItems = objModuleMonthlyBudgetManager.Load(x => x.BudgetCategoryLookup == budgetCategory.ID &&
                                                                            x.AllocationStartDate >= startDate && x.AllocationStartDate <= endDate);

                double nonProjectPlannedTotal = 0.0;
                double nonProjectActualTotal = 0.0;
                double projectPlannedTotal = 0.0;
                double projectActualTotal = 0.0;

                foreach (ModuleMonthlyBudget mItem in itgMonthlyItems)
                {
                    DateTime bStartDate = mItem.AllocationStartDate;

                    //if (bStartDate.Month >= startDate.Month && bStartDate.Month <= endDate.Month)
                    //{
                    //double nonProjectPlanned = 0.0;
                    //double nonProjectActual = 0.0;
                    //double projectPlanned = 0.0;
                    //double projectActual = 0.0;

                    nonProjectPlannedTotal += mItem.NonProjectPlanedTotal;
                    nonProjectActualTotal += mItem.NonProjectActualTotal;
                    projectPlannedTotal += mItem.ProjectPlanedTotal;
                    projectActualTotal += mItem.ProjectActualTotal;
                    // }
                }

                #region Calculate total Resource cost

                #endregion

                DataRow row = result.NewRow();
                row[DatabaseObjects.Columns.Id] = budgetCategory.ID;
                row[DatabaseObjects.Columns.BudgetType] = budgetCategory.BudgetType;

                row[DatabaseObjects.Columns.BudgetCategory] = budgetCategory.BudgetCategoryName;
                row[DatabaseObjects.Columns.BudgetSubCategory] = budgetCategory.BudgetSubCategory;

                row[DatabaseObjects.Columns.BudgetCOA] = budgetCategory.BudgetCOA;
                row[DatabaseObjects.Columns.ResourceCost] = 0; // = totalResouceCost;

                row[DatabaseObjects.Columns.BudgetAmount] = nonProjectPlannedTotal;
                row[DatabaseObjects.Columns.BudgetDescription] = budgetCategory.BudgetDescription;

                // Add sub-category wise total in table.
                row[DatabaseObjects.Columns.NonProjectPlannedTotal] = nonProjectPlannedTotal;
                row[DatabaseObjects.Columns.NonProjectActualTotal] = nonProjectActualTotal;
                row[DatabaseObjects.Columns.ProjectPlannedTotal] = Math.Round(projectPlannedTotal, 2);
                row[DatabaseObjects.Columns.ProjectActualTotal] = projectActualTotal;
                row[DatabaseObjects.Columns.CapitalExpenditure] = budgetCategory.CapitalExpenditure;

                result.Rows.Add(row);
            }
            resultTables.Add(result);
            return resultTables;
        }


        public void UpdateProjectMonthlyDistribution(ApplicationContext _context, bool pmmBudgetNeedsApproval, ModuleBudget moduleBudget, string ticketId, string ModuleName)
        {
            DataTable BudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudget, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}'");
            DataTable MonthlyBudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'  and {DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}'");
            DataTable pmmActualList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetActuals, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}'");
            //Delete all monthly ditribution of pmm from pmm monthly budget list.
            string budgetQuery1 = string.Empty;
            string pmmMonthlyBudgetQuery = string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.BudgetType, "0");
            //DataRow[] pmmBudgetMonthlyCollection = MonthlyBudgetList.Select(pmmMonthlyBudgetQuery);
            //while (pmmBudgetMonthlyCollection.Count() > 0)
            //{
            //    pmmBudgetMonthlyCollection[0].Delete();
            //}
            DateTime newStartDate = (DateTime)moduleBudget.AllocationStartDate;
            DateTime newEndDate = (DateTime)moduleBudget.AllocationEndDate;
            if (pmmBudgetNeedsApproval)
                budgetQuery1 = string.Format("{0}='{1}' And {2}='{3}' and {4}='{5}' and {6}='{7}' ", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve, DatabaseObjects.Columns.AllocationStartDate,newStartDate,DatabaseObjects.Columns.AllocationEndDate, newEndDate);
            else
                budgetQuery1 = string.Format("{0}='{1}' And {2}='{3}' and {4}='{5}'", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.AllocationStartDate, newStartDate, DatabaseObjects.Columns.AllocationEndDate, newEndDate);

            DataRow[] pmmBudgetCollection = BudgetList.Select(budgetQuery1);
            foreach (DataRow budgetItem in pmmBudgetCollection)
            {
                int budgetItemID = 0;
                int.TryParse(Convert.ToString(budgetItem[DatabaseObjects.Columns.Id]), out budgetItemID);
                if (budgetItemID > 0)
                {
                    UpdateProjectMonthlyDistributionBudgetByTicketId(ticketId, moduleBudget);
                    DataRow[] pmmActualCollection = pmmActualList.Select(string.Format("{0}='{1}' and {2}={3}", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.ModuleBudgetLookup, budgetItemID));
                    foreach (DataRow actualItem in pmmActualCollection)
                    {
                        UpdateProjectMonthlyDistributionActual(actualItem, ticketId);
                    }
                }
            }
        }

        public void UpdateProjectMonthlyDistributionBudgetByTicketId(string ticketID, ModuleBudget newBudget)
        {
            // Query to get all distribution of current Project.
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
                        DataRow oldBudgetItem = moduleMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID])))[0]; //SPListHelper.GetSPListItem(pmmMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
                        double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.BudgetAmount]), out oldValue);
                        ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                        try
                        {
                            if (objModuleMonthlyBudget != null)
                            {
                                objModuleMonthlyBudget.BudgetAmount = oldValue + val;
                                objModuleMonthlyBudget.ProjectPlanedTotal = oldValue + val;
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
                        objModuleMonthlyBudget.BudgetCategoryLookup = newBudget.BudgetCategoryLookup;
                        objModuleMonthlyBudget.BudgetType = "0";
                        objModuleMonthlyBudget.ProjectPlanedTotal = val;
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
        public void UpdateProjectMonthlyDistributionActual(DataRow newActualItem, string ticketID)
        {
            // Query get all monthly distribution of current Project.
            string modulename = uHelper.getModuleNameByTicketId(ticketID);
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            DataTable moduleMonthlyBudgetList = objModuleMonthlyBudgetManager.GetDataTable();
            DataView dvpmmMonthlyBudgetList = new DataView(moduleMonthlyBudgetList);
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
                        DataRow oldBudgetItem = moduleMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID])))[0];
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
        }
    }

}
