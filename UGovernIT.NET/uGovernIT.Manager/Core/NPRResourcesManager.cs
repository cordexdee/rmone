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
    public class NPRResourcesManager: ManagerBase<NPRResource>, INPRResourcesManager
    { 
        ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager;
        ApplicationContext _context;
        public NPRResourcesManager(ApplicationContext context):base(context)
        {
            _context = context;
        
           // objNPRResources = new NPRResourcesManager(this.dbContext);
            store = new NPRResourcesStore(this.dbContext);
        }
        public  DataTable LoadNprResources()
        {
            
            List<NPRResource> configModuleNPRResourcesList = store.Load();
            return UGITUtility.ToDataTable<NPRResource>(configModuleNPRResourcesList);
        }
       
        public  void RemoveNPRMonthlyDistributionResource(string TickedId, string type)
        {
            objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            DataTable nprResourceList = LoadNprResources();  //SPListHelper.GetSPList(DatabaseObjects.Lists.NPRResources);
            DataTable MonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget); //SPListHelper.GetSPList(DatabaseObjects.Lists.NPRMonthlyBudget);
            if (TickedId != null)
            {

                DataRow[] moudleBudgetCollection = MonthlyBudgetList.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, TickedId, DatabaseObjects.Columns.BudgetType, type));
                if (moudleBudgetCollection.Count() > 0)
                {
                    foreach (DataRow item in moudleBudgetCollection)
                    {
                        ModuleMonthlyBudget objModuleMonthlyBUdget = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(item[DatabaseObjects.Columns.ID]));
                        objModuleMonthlyBudgetManager.Delete(objModuleMonthlyBUdget);

                    }
                    //for (int i = moudleBudgetCollection.Length - 1; i >= 0; i--)
                    //{
                    //    GetTableDataManager.delete<int>(DatabaseObjects.Tables.ModuleMonthlyBudget, DatabaseObjects.Columns.ID, Convert.ToString(moudleBudgetCollection[i][DatabaseObjects.Columns.ID]));
                    //    //nprBudgetCollection.Delete(i);
                    //}
                }
            }
        }
        public NPRResource InsertORUpdateData(NPRResource objNPRResources)
        {
            return (store as NPRResourcesStore).InsertORUpdateData(objNPRResources);
        }
        public  void AddNPRMonthlyDistributionResource(string TickedId, string type,string ModuleName)
        {
            objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_context);
            DataTable nprResourceList = LoadNprResources();
            DataTable MonthlyBudgetList = objModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();  //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget);

            if (TickedId != null)
            {
                DataRow[] nprResourceItemCollection = nprResourceList.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, TickedId, DatabaseObjects.Columns.BudgetTypeChoice, type));
                if (nprResourceItemCollection.Length > 0)
                {
                    foreach (DataRow ritem in nprResourceItemCollection)
                    {
                        // SPListItemCollection nprBudgetCollection = nprMonthlyBudgetList.GetItems(nprBudgetQuery);
                        DataRow[] nprBudgetCollection = MonthlyBudgetList.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, TickedId, DatabaseObjects.Columns.BudgetType, type));
                        DataTable budgetTable = null;
                        if (nprBudgetCollection.Length > 0)
                        {
                            budgetTable = nprBudgetCollection.CopyToDataTable();
                        }

                        DateTime startDate = (DateTime)ritem[DatabaseObjects.Columns.AllocationStartDate];
                        DateTime endDate = (DateTime)ritem[DatabaseObjects.Columns.AllocationEndDate];
                        double totalFTEs = Convert.ToDouble(ritem[DatabaseObjects.Columns.TicketNoOfFTEs]);

                        // Distribute the amount within specified dates and get the result in month and amount format.
                        Dictionary<DateTime, double> distributions = uHelper.MonthlyDistributeFTEs(_context,startDate, endDate, totalFTEs);

                        // Add new Distribution
                        foreach (DateTime key in distributions.Keys)
                        {
                            double val = distributions[key];
                            double oldValue = 0;
                            DataRow oldItem = null;

                            if (budgetTable != null && budgetTable.Rows.Count > 0)
                            {
                                oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                            }

                            if (oldItem != null)
                            {
                                ModuleMonthlyBudget _moduleMonthlyBudget = new ModuleMonthlyBudget();
                                DataRow oldBudgetItem = MonthlyBudgetList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, oldItem[DatabaseObjects.Columns.ID]))[0];   //UGITUtility.GetSPListItem(nprMonthlyBudgetList, (int)oldItem[DatabaseObjects.Columns.Id]);
                                if (oldBudgetItem != null)
                                {
                                    double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.BudgetAmount]), out oldValue);
                                    _moduleMonthlyBudget.BudgetAmount = oldValue + val;
                                    objModuleMonthlyBudgetManager.InsertORUpdateData(_moduleMonthlyBudget);
                                }
                            }
                            else
                            {
                                ModuleMonthlyBudget _moduleMonthlyBudget = new ModuleMonthlyBudget();
                                _moduleMonthlyBudget.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                _moduleMonthlyBudget.BudgetAmount = val;
                                _moduleMonthlyBudget.TicketId = TickedId;
                                _moduleMonthlyBudget.BudgetType = type;
                                _moduleMonthlyBudget.ModuleName = ModuleName;
                                objModuleMonthlyBudgetManager.InsertORUpdateData(_moduleMonthlyBudget);
                            }
                        }
                    }
                }
            }
        }
       
    }
    public interface INPRResourcesManager : IManagerBase<NPRResource>
    {
        void AddNPRMonthlyDistributionResource(string TickedId, string type, string ModuleName);
        void RemoveNPRMonthlyDistributionResource(string TickedId, string type);
    }
}
