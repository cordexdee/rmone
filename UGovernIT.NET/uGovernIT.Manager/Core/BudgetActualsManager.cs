using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class BudgetActualsManager:ManagerBase<BudgetActual>,IBudgetActualsManager
    {
        ModuleBudgetManager objModuleBudgetManager;
        ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager;
        ApplicationContext _context;
        public BudgetActualsManager(ApplicationContext context):base(context)
        {
            _context = context;
            store = new BudgetActualsStore(this.dbContext);
          
            objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(this.dbContext);
        }
        public BudgetActual InsertORUpdateData(BudgetActual objBudgetActuals)
        {
            return (store as BudgetActualsStore).InsertOrUpdateData(objBudgetActuals);
        }
        public  DataTable LoadModuleBudgetActuals()
        {
           //return GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetActuals);
            List<BudgetActual> configModuleBudgetActualList = store.Load();

            return UGITUtility.ToDataTable<BudgetActual>(configModuleBudgetActualList);
        }
        /// <summary>
        ///  function picked from pmm budget class in share point project
        /// </summary>
        /// <param name="TickedId"></param>
        public  void UpdateProjectMonthlyDistributionActual( string TicketId,string ModuleName)
        {
            objModuleBudgetManager = new ModuleBudgetManager(_context);
            // Open all three related list to get budget item actuals.
            
            DataTable dtActualList =LoadModuleBudgetActuals();
            DataTable dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            DataTable dtActualTable = new DataTable();
            DataRow[] dractulasCollection = dtActualList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketId));
            if (dractulasCollection.Count() > 0)
            {
                dtActualTable = dractulasCollection.CopyToDataTable();

            }

            // Open all three related list to get budget item actuals.
            DataRow[] drBudgetCollection = dtMonthlyBudgetList.Select(string.Format("{0}='{1}'And {2}='{3}'", DatabaseObjects.Columns.TicketId, TicketId, DatabaseObjects.Columns.BudgetType, "0"));
            DataTable budgetTable = null;
            if (drBudgetCollection.Count() > 0)
            {
                foreach (DataRow item in drBudgetCollection)
                {
                    ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(item[DatabaseObjects.Columns.ID]));
                    objModuleMonthlyBudget.ActualCost = 0;
                    objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                }
                dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
                drBudgetCollection = dtMonthlyBudgetList.Select(string.Format("{0}='{1}'And {2}='{3}'", DatabaseObjects.Columns.TicketId, TicketId, DatabaseObjects.Columns.BudgetType, "0"));
                budgetTable = drBudgetCollection.CopyToDataTable();
            }

            if (dtActualTable != null && dtActualTable.Rows.Count > 0)
            {
                foreach (DataRow actualRow in dtActualTable.Rows)
                {
                    DateTime newStartDate = (DateTime)actualRow[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime newEndDate = (DateTime)actualRow[DatabaseObjects.Columns.AllocationEndDate];
                    double newBudgetAmount = 0;
                    double.TryParse(Convert.ToString(actualRow[DatabaseObjects.Columns.BudgetAmount]), out newBudgetAmount);
                    ModuleBudget budgetItem = objModuleBudgetManager.LoadByID(UGITUtility.StringToLong(actualRow[DatabaseObjects.Columns.ModuleBudgetLookup]));

                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> newDistributions = uHelper.DistributeAmount(_context,newStartDate, newEndDate, newBudgetAmount);

                    // Update with new distribution
                    foreach (DateTime key in newDistributions.Keys)
                    {
                        double val = newDistributions[key];
                        double oldValue = 0;
                        DataRow oldItem = null;

                        if (budgetTable != null && budgetTable.Rows.Count > 0)
                        {
                            if(budgetItem != null)
                                oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date
                                && x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup) == UGITUtility.ObjectToString(budgetItem.BudgetCategoryLookup));
                        }

                        if (oldItem != null)
                        {
                            ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                            double.TryParse(Convert.ToString(objModuleMonthlyBudget.ActualCost), out oldValue);
                            objModuleMonthlyBudget.ActualCost = val;
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                        else
                        {
                            ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                            objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                            objModuleMonthlyBudget.ActualCost = val;
                            objModuleMonthlyBudget.TicketId = TicketId;
                            objModuleMonthlyBudget.BudgetType = "0";
                            objModuleMonthlyBudget.ModuleName = ModuleName;
                            objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                        }
                    }
                }
            }
        }
        public  void UpdateNonProjectMonthlyDistributionActual(long subCategoryID)
        {
            objModuleBudgetManager = new ModuleBudgetManager(_context);
            // Open all three related list to get budget item actuals.
            DataTable dtModuleBudgetList = objModuleBudgetManager.LoadModuleBudget();
            DataTable dtModuleBudgetTable = new DataTable();
            DataTable dtActualList = LoadModuleBudgetActuals();
            DataTable dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            DataRow[] drModuleBudgetColl = dtModuleBudgetList.Select(string.Format("{0}={1} And {2}={3} And {4}='{5}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.BudgetStatus, (int)Enums.BudgetStatus.Approve,DatabaseObjects.Columns.ModuleNameLookup,ModuleNames.PMM));
            if (drModuleBudgetColl.Count() > 0)
            {
                dtModuleBudgetTable = drModuleBudgetColl.CopyToDataTable();
            }
            DataRow[] droldBudgetDistributionsColl = dtMonthlyBudgetList.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID,DatabaseObjects.Columns.ModuleNameLookup,ModuleNames.ITG));
            if (droldBudgetDistributionsColl.Count() > 0)
            {
                dtMonthlyBudgetList = droldBudgetDistributionsColl.CopyToDataTable();

            }
            foreach (DataRow oldItem in droldBudgetDistributionsColl)
            {
                ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                objModuleMonthlyBudget.ProjectActualTotal = 0;
                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
            }

            dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
            droldBudgetDistributionsColl = dtMonthlyBudgetList.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.ITG));
            //distribute budget amount for old sub category
            if (dtModuleBudgetTable != null && dtModuleBudgetTable.Rows.Count > 0)
            {
                foreach (DataRow budgetRow in dtModuleBudgetTable.Rows)
                {
                    DataRow[] dractualCollection = dtActualList.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.ModuleBudgetLookup, budgetRow[DatabaseObjects.Columns.ID],DatabaseObjects.Columns.ModuleNameLookup,ModuleNames.PMM));
                    if (dractualCollection.Count() > 0)
                    {
                        dtActualList = dractualCollection.CopyToDataTable();
                    }

                    //distribute budget amount for old sub category
                    if (dtActualList != null && dtActualList.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtActualList.Rows)
                        {
                            // Get the start date, end date and amount to distribute within start and end date.
                            DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                            DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                            double totalAmount = 0;
                            double.TryParse(Convert.ToString(row[DatabaseObjects.Columns.BudgetAmount]), out totalAmount);

                            // Distribute the amount within specified dates and get the result in month and amount format.
                            Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                            // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                            foreach (DateTime key in amountDistributions.Keys)
                            {
                                double val = amountDistributions[key];
                                DataRow preItem = null;
                                if (dtMonthlyBudgetList != null && dtMonthlyBudgetList.Rows.Count > 0)
                                {
                                    preItem = dtMonthlyBudgetList.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                                }

                                DataRow item = null;
                                double total = 0.0;
                                if (preItem != null)
                                {
                                    item = dtMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                                    ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(preItem[DatabaseObjects.Columns.ID]));
                                    double.TryParse(Convert.ToString(objModuleMonthlyBudget.ProjectActualTotal), out total);
                                    objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                                    objModuleMonthlyBudget.ProjectActualTotal = total + val;
                                    objModuleMonthlyBudget.ModuleName = ModuleNames.ITG;
                                    objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                                }
                                else
                                {

                                    ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                                    objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                    objModuleMonthlyBudget.EstimatedCost = 0;
                                    objModuleMonthlyBudget.ResourceCost = 0;
                                    objModuleMonthlyBudget.ActualCost = 0;
                                    objModuleMonthlyBudget.BudgetType = "0";
                                    objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                                    objModuleMonthlyBudget.NonProjectActualTotal = 0;
                                    objModuleMonthlyBudget.NonProjectPlanedTotal = 0;
                                    objModuleMonthlyBudget.ProjectActualTotal = val;
                                    objModuleMonthlyBudget.ProjectPlanedTotal = 0;
                                    objModuleMonthlyBudget.ModuleName = ModuleNames.ITG;
                                    objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateNonProjectMonthlyDistributionActual(long subCategoryID, string TicketID)
        {
            objModuleBudgetManager = new ModuleBudgetManager(_context);
            // Open all three related list to get budget item actuals.
            DataTable dtModuleBudgetList = objModuleBudgetManager.LoadModuleBudget();
            DataTable dtActualList = LoadModuleBudgetActuals();
            DataTable dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            DataRow[] drModuleBudgetColl = dtModuleBudgetList.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.TicketId, TicketID));
            if (drModuleBudgetColl.Count() > 0)
            {
                dtModuleBudgetList = drModuleBudgetColl.CopyToDataTable();
            }
            DataRow[] droldBudgetDistributionsColl = dtMonthlyBudgetList.Select(string.Format("{0}={1} And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryLookup, subCategoryID, DatabaseObjects.Columns.TicketId, TicketID));
            if (droldBudgetDistributionsColl.Count() > 0)
            {
                dtMonthlyBudgetList = droldBudgetDistributionsColl.CopyToDataTable();

            }
            foreach (DataRow oldItem in droldBudgetDistributionsColl)
            {
                ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldItem[DatabaseObjects.Columns.ID]));
                objModuleMonthlyBudget.NonProjectActualTotal = 0;
                objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
            }
            dtMonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();
            //distribute budget amount for old sub category
            if (dtModuleBudgetList != null && dtModuleBudgetList.Rows.Count > 0)
            {
                foreach (DataRow budgetRow in dtModuleBudgetList.Rows)
                {
                    DataRow[] dractualCollection = dtActualList.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleBudgetLookup, budgetRow[DatabaseObjects.Columns.ID]));
                    if (dractualCollection.Count() > 0)
                    {
                        dtActualList = dractualCollection.CopyToDataTable();
                    }

                    //distribute budget amount for old sub category
                    if (dtActualList != null && dtActualList.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtActualList.Rows)
                        {
                            // Get the start date, end date and amount to distribute within start and end date.
                            DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                            DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                            double totalAmount = 0;
                            double.TryParse(Convert.ToString(row[DatabaseObjects.Columns.BudgetAmount]), out totalAmount);

                            // Distribute the amount within specified dates and get the result in month and amount format.
                            Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(_context, startDate, endDate, totalAmount);

                            // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                            foreach (DateTime key in amountDistributions.Keys)
                            {
                                double val = amountDistributions[key];
                                DataRow preItem = null;
                                if (dtMonthlyBudgetList != null && dtMonthlyBudgetList.Rows.Count > 0)
                                {
                                    preItem = dtMonthlyBudgetList.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                                }

                                DataRow item = null;
                                double total = 0.0;
                                if (preItem != null)
                                {


                                    item = dtMonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(preItem[DatabaseObjects.Columns.ID])))[0];
                                    double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.NonProjectActualTotal]), out total);
                                    ModuleMonthlyBudget objModuleMonthlyBudget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(preItem[DatabaseObjects.Columns.ID]));
                                    objModuleMonthlyBudget.BudgetCategoryLookup = subCategoryID;
                                    objModuleMonthlyBudget.NonProjectActualTotal = total + val;
                                    objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                                }
                                else
                                {

                                    //ModuleMonthlyBudget objModuleMonthlyBudget = new ModuleMonthlyBudget();
                                    //objModuleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                    //objModuleMonthlyBudget.EstimatedCost = 0;
                                    //objModuleMonthlyBudget.ResourceCost = 0;
                                    //objModuleMonthlyBudget.ActualCost = 0;
                                    //objModuleMonthlyBudget.BudgetType = "0";
                                    //objModuleMonthlyBudget.BudgetLookup = subCategoryID;
                                    //objModuleMonthlyBudget.NonProjectActualTotal = 0;
                                    //objModuleMonthlyBudget.NonProjectPlanedTotal = 0;
                                    //objModuleMonthlyBudget.ProjectActualTotal = val;
                                    //objModuleMonthlyBudget.ProjectPlanedTotal = 0;
                                    //objModuleMonthlyBudgetManager.InsertORUpdateData(objModuleMonthlyBudget);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    public interface IBudgetActualsManager:IManagerBase<BudgetActual>
    {
        DataTable LoadModuleBudgetActuals();
        void UpdateProjectMonthlyDistributionActual(string TickedId, string ModuleName);
		void UpdateNonProjectMonthlyDistributionActual(long subCategoryID, string TicketID);
        void UpdateNonProjectMonthlyDistributionActual(long subCategoryID);
        BudgetActual InsertORUpdateData(BudgetActual objBudgetActuals);
    }
}
