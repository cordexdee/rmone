using System;
using System.Linq;
using System.Xml;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Collections.Generic;
using uGovernIT.Util.Cache;
using Newtonsoft.Json;
using System.Data;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Manager.Managers;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public interface IStatisticsManager : IManagerBase<Statistics>
    {
        bool ProcessStatistics(List<string> ticketIDs, UGITModule module);
        Statistics GetChangedFieldValue(UGITModule module, DataRow item, TicketColumnValue val, string userID, DataColumn field);
    }

    public class StatisticsManager : ManagerBase<Statistics>, IStatisticsManager
    {
        private TicketManager _ticketManager;
        private ProjectEstimatedAllocationManager _estimationAllocationManager;
        private StatisticsConfigurationManager _statisticsConfigurationManager;
        private ApplicationContext _context;
        public StatisticsManager(ApplicationContext context) : base(context)
        {
            store = new StatisticsStore(this.dbContext);

            _context = context;
            _ticketManager = new TicketManager(context);
            _estimationAllocationManager = new ProjectEstimatedAllocationManager(context);
            _statisticsConfigurationManager = new StatisticsConfigurationManager(context);
        }

        public override bool InsertItems(List<Statistics> itemList)
        {
            return base.InsertItems(itemList);
        }

        public Statistics GetChangedFieldValue(UGITModule module, DataRow item, TicketColumnValue val, string userID, DataColumn field)
        {
            Statistics statistics = null;

            if (module.ModuleName != ModuleNames.CPR)
                return null;

            if (val == null)
                return null;

            string ticketID = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketId]);
            if (string.IsNullOrEmpty(ticketID))
                return null;

            try
            {
                string newValue = "";
                string oldValue = "";

                if (field.DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                {
                    newValue = UGITUtility.GetDateStringInFormat(Convert.ToString(val.Value), false);
                    oldValue = UGITUtility.GetDateStringInFormat(Convert.ToString(item[val.InternalFieldName]), false);
                }
                else
                {
                    newValue = UGITUtility.ObjectToString(val.Value);
                    oldValue = UGITUtility.ObjectToString(item[val.InternalFieldName]);
                }
                if (string.IsNullOrEmpty(oldValue))
                {
                    statistics = new Statistics { TicketID = ticketID, FieldName = val.InternalFieldName, Value = null, CreatedBy = userID, Date = DateTime.Now };
                }
                if ((!string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
                    || (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue) && oldValue != newValue))
                {
                    statistics = new Statistics { TicketID = ticketID, FieldName = val.InternalFieldName, Value = oldValue, CreatedBy = userID, Date = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                ULog.WriteLog($"Failed Statistics for Ticket - {ticketID}.");
                ULog.WriteException(ex);
            }
            return statistics;
        }

        public bool ProcessStatistics(List<string> ticketIDs, UGITModule module)
        {
            if (module.ModuleName != ModuleNames.CPR)
                return false;

            bool isSucceed = true;

            DataTable dataTable = _ticketManager.GetByTicketIDs(module, ticketIDs);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                ULog.WriteLog($"Processing statistics for {dataTable.Rows.Count} {ModuleNames.CPR} projects");
                foreach (DataRow projectRow in dataTable.Rows)
                {
                    string ticketID = Convert.ToString(projectRow[DatabaseObjects.Columns.TicketId]);

                    // NumTags
                    var tags = new List<ProjectTag>();
                    var tagStr = Convert.ToString(projectRow[DatabaseObjects.Columns.TagMultiLookup]);
                    if (!string.IsNullOrEmpty(tagStr))
                    {
                        try
                        {
                            tags = JsonConvert.DeserializeObject<List<ProjectTag>>(tagStr);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {tagStr}.");
                            tags = new List<ProjectTag>();
                        }
                    }
                    projectRow[DatabaseObjects.Columns.NumTags] = tags == null ? 0 : tags.Count;

                    // NumAllocations
                    var allocations = _estimationAllocationManager.Load(x => x.TicketId == ticketID && x.Deleted != true);
                    projectRow[DatabaseObjects.Columns.NumAllocations] = allocations.Count;

                    // NumUniqueRoles
                    var noOfUniqueRoles = allocations.Where(a => !string.IsNullOrEmpty(a.Type)).Select(a => a.Type).Distinct().Count();
                    projectRow[DatabaseObjects.Columns.NumUniqueRoles] = noOfUniqueRoles;

                    // NumContractAmountChanges & PerContractAmountChanges
                    var configuredFields = _statisticsConfigurationManager.Load(sc => sc.FieldName == DatabaseObjects.Columns.ApproxContractValue && sc.ModuleName == module.ModuleName);
                    if (configuredFields.Count > 0)
                    {
                        var contractAmountHistory = this.Load(sc => sc.TicketID == ticketID && sc.FieldName == DatabaseObjects.Columns.ApproxContractValue).OrderByDescending(sc => sc.Date);
                        projectRow[DatabaseObjects.Columns.NumContractAmountChanges] = contractAmountHistory.Count();

                        if (contractAmountHistory.Count() > 0 && !string.IsNullOrEmpty(contractAmountHistory.Last().Value) && !string.IsNullOrEmpty(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.ApproxContractValue])))
                        {
                            var oldValue = Convert.ToInt64(contractAmountHistory.Last().Value);
                            var currentValue = Convert.ToDecimal(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.ApproxContractValue]));

                            projectRow[DatabaseObjects.Columns.PerContractAmountChange] = $"{Math.Round(((currentValue - oldValue) / oldValue) * 100, MidpointRounding.AwayFromZero)}%";
                        }
                        else
                        {
                            projectRow[DatabaseObjects.Columns.PerContractAmountChange] = "0%";
                        }
                    }
                    else
                    {
                        projectRow[DatabaseObjects.Columns.NumContractAmountChanges] = 0;
                        projectRow[DatabaseObjects.Columns.PerContractAmountChange] = "0%";
                    }

                    // NumScheduleChanges & PerScheduleChanges
                    configuredFields = _statisticsConfigurationManager.Load(sc => sc.FieldName == DatabaseObjects.Columns.CRMDuration && sc.ModuleName == module.ModuleName);
                    if (configuredFields.Count > 0)
                    {
                        var crmDurationHistory = this.Load(sc => sc.TicketID == ticketID && sc.FieldName == DatabaseObjects.Columns.CRMDuration).OrderByDescending(sc => sc.Date);
                        projectRow[DatabaseObjects.Columns.NumScheduleChanges] = crmDurationHistory.Count();

                        if (crmDurationHistory.Count() > 0 && !string.IsNullOrEmpty(crmDurationHistory.Last().Value) 
                            && projectRow[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value && projectRow[DatabaseObjects.Columns.CloseoutDate] != DBNull.Value)
                        {
                            int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, Convert.ToDateTime(projectRow[DatabaseObjects.Columns.PreconStartDate]), Convert.ToDateTime(projectRow[DatabaseObjects.Columns.CloseoutDate]));
                            int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);
                            var oldValue = Convert.ToInt64(crmDurationHistory.Last().Value);
                            var currentValue = Convert.ToDecimal(noOfWeeks);

                            projectRow[DatabaseObjects.Columns.PerScheduleChanges] = $"{Math.Round(((currentValue - oldValue) / oldValue) * 100, MidpointRounding.AwayFromZero)}%";
                        }
                        else
                        {
                            projectRow[DatabaseObjects.Columns.PerScheduleChanges] = "0%";
                        }
                    }
                    else
                    {
                        projectRow[DatabaseObjects.Columns.NumScheduleChanges] = 0;
                        projectRow[DatabaseObjects.Columns.PerScheduleChanges] = "0%";
                    }

                    // Save Record
                    var isSucceeded = _ticketManager.Save(module, projectRow) > 0;
                    if (isSucceeded)
                        ULog.WriteLog($" -- Completed project - {ticketID} - Updated Statistics.");
                    else
                    {
                        isSucceed = false;
                        ULog.WriteLog($" -- Failed project - {ticketID}");
                        break;
                    }
                }
            }


            return isSucceed;
        }
    }
}
