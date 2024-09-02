using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Manager
{
    public class Baseline
    {
        public ApplicationContext _applicationContext;

        private TicketManager _ticketManager = null;
        private ModuleViewManager moduleManager = null;
        private UGITTaskManager TaskManager = null;
        private ModuleTaskHistoryManager _moduleTaskHistoryManager = null;
        private ModuleBudgetManager _moduleBudgetManager = null;
        private ModuleBudgetHistoryManager _moduleBudgetHistoryManager = null;
        private BudgetActualsManager _budgetActualsManager = null;
        private ModuleBudgetActualsHistoryManager _moduleBudgetActualsHistoryManager = null;
        private ModuleMonthlyBudgetManager _moduleMonthlyBudgetManager = null;
        private ModuleMonthlyBudgetHistoryManager _moduleMonthlyBudgetHistoryManager = null;
        private PMMCommentManager _pmmCommentManager = null;
        private PMMCommentHistoryManager _pmmCommentHistoryManager = null;
        private DecisionLogManager _decisionLogManager = null;
        private DecisionLogHistoryManager _decisionLogHistoryManager = null;
        private ProjectMonitorStateManager _projectMonitorStateManager = null;
        private ProjectMonitorStateHistoryManager _projectMonitorStateHistoryManager = null;
        private ModuleMonitorOptionHistoryManager _moduleMonitorOptionHistoryManager = null;
        private ModuleMonitorOptionManager _moduleMonitorOptionManager = null;
        


        /// <summary>
        /// Baseline number is version number of ticket item on which base is created.
        /// To get item version use item.Versions.GetVersionFromLabel(baseline number as string)
        /// </summary>

        public string BaselineComment { get; set; }
        public string TicketId { get; set; }
        public long BaselineDetailID { get; set; }
        public int BaselineNum { get; private set; }
        public int parentTaskId = 0;
        public DateTime BaselineDate { get; private set; }

        public ModuleTasksHistory pmmtaskHistory;


        public Baseline(string ticketId, DateTime baselineDate, ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;

            BaselineDate = baselineDate;

            BaselineComment = string.Empty;

            TicketId = ticketId;

            moduleManager = new ModuleViewManager(_applicationContext);

            TaskManager = new UGITTaskManager(_applicationContext);

            _moduleTaskHistoryManager = new ModuleTaskHistoryManager(_applicationContext);

            _moduleBudgetManager = new ModuleBudgetManager(_applicationContext);

            _moduleBudgetHistoryManager = new ModuleBudgetHistoryManager(_applicationContext);

            _budgetActualsManager = new BudgetActualsManager(_applicationContext);

            _moduleBudgetActualsHistoryManager = new ModuleBudgetActualsHistoryManager(_applicationContext);

            

        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(_applicationContext);
                }
                return _ticketManager;
            }
        }

        protected ModuleMonthlyBudgetManager ModuleMonthlyBudgetManager
        {
            get
            {
                if (_moduleMonthlyBudgetManager == null)
                {
                    _moduleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(_applicationContext);

                }
                return _moduleMonthlyBudgetManager;


            }
        }

        protected ModuleMonthlyBudgetHistoryManager ModuleMonthlyBudgetHistoryManager
        {
            get
            {
                if (_moduleMonthlyBudgetHistoryManager == null)
                {
                    _moduleMonthlyBudgetHistoryManager = new ModuleMonthlyBudgetHistoryManager(_applicationContext);

                }
                return _moduleMonthlyBudgetHistoryManager;
            }
        }

        protected PMMCommentManager PMMCommentManager
        {
            get
            {
                if (_pmmCommentManager == null)
                {
                    _pmmCommentManager = new PMMCommentManager(_applicationContext);
                }
                return _pmmCommentManager;
            }
        }

        protected PMMCommentHistoryManager PMMCommentHistoryManager
        {
            get
            {
                if (_pmmCommentHistoryManager == null)
                {
                    _pmmCommentHistoryManager = new PMMCommentHistoryManager(_applicationContext);
                }
                return _pmmCommentHistoryManager;
            }
        }

        protected DecisionLogManager DecisionLogManager
        {
            get
            {
                if (_decisionLogManager == null)
                {
                    _decisionLogManager = new DecisionLogManager(_applicationContext);
                }
                return _decisionLogManager;
            }
        }

        protected DecisionLogHistoryManager DecisionLogHistoryManager
        {
            get
            {
                if (_decisionLogHistoryManager == null)
                {
                    _decisionLogHistoryManager = new DecisionLogHistoryManager(_applicationContext);
                }
                return _decisionLogHistoryManager;
            }
        }


        protected ProjectMonitorStateManager ProjectMonitorStateManager
        {
            get
            {
                if (_projectMonitorStateManager == null)
                {
                    _projectMonitorStateManager = new ProjectMonitorStateManager(_applicationContext);
                }
                return _projectMonitorStateManager;
            }
        }

        protected ProjectMonitorStateHistoryManager ProjectMonitorStateHistoryManager
        {
            get
            {
                if (_projectMonitorStateHistoryManager == null)
                {
                    _projectMonitorStateHistoryManager = new ProjectMonitorStateHistoryManager(_applicationContext);
                }
                return _projectMonitorStateHistoryManager;
            }
        }

        protected ModuleMonitorOptionHistoryManager ModuleMonitorOptionHistoryManager
        {
            get
            {
                if (_moduleMonitorOptionHistoryManager == null)
                {
                    _moduleMonitorOptionHistoryManager = new ModuleMonitorOptionHistoryManager(_applicationContext);
                }
                return _moduleMonitorOptionHistoryManager;
            }
        }

        protected ModuleMonitorOptionManager ModuleMonitorOptionManager
        {
            get
            {
                if (_moduleMonitorOptionManager == null)
                {
                    _moduleMonitorOptionManager = new ModuleMonitorOptionManager(_applicationContext);
                }
                return _moduleMonitorOptionManager;
            }
        }

        



        public void CreateBaseline(DataRow pmmItem, string moduleName,int pmmId=0)
        {

            CreateBaseline(pmmItem, false, moduleName, pmmId);
        }

        public void CreateBaseline(DataRow pmmItem, bool initialVersion, string moduleName, int pmmId=0)
        {
            Ticket tkt = new Ticket(_applicationContext, moduleName);

            TicketDal ticketDal = new TicketDal(_applicationContext);

            UGITModule module = moduleManager.LoadByName(moduleName);

            BaseLineDetailsManager baseLineDetailsManager = new BaseLineDetailsManager(_applicationContext);

            int newBaseLineNum = 0;

            try
            {
                if (initialVersion)
                {
                    // Start with #1
                    BaselineNum = 1;
                }
                else
                {
                    // First do an update to generate a new version

                    tkt.CommitChanges(pmmItem);

                    // Now reload pmmItem so we have latest version info

                    // pmmItem = pmmItem.ParentList.GetItemById(pmmItem.ID);
                    DataRow pmmitem = TicketManager.GetByID(module, Convert.ToInt64(pmmItem[DatabaseObjects.Columns.ID]));

                    // Get the new baseline number from the version label of latest version
                    int baseLineNum = baseLineDetailsManager.Load(x => x.TicketID == Convert.ToString(pmmItem[DatabaseObjects.Columns.TicketId])).Count();

                    if (baseLineNum > 0)
                    {
                        newBaseLineNum = baseLineNum + 1;
                    }
                    else
                        newBaseLineNum = 1;

                    BaselineNum = newBaseLineNum;
                }

               
                #region Create Baseline Detail

                BaseLineDetails baseLineDetails = new BaseLineDetails();

                baseLineDetails.Title = pmmItem[DatabaseObjects.Columns.TicketId].ToString() + " " + BaselineNum.ToString();

                baseLineDetails.TicketID = TicketId;

                baseLineDetails.BaselineId = BaselineNum;

                baseLineDetails.BaselineComment = BaselineComment;

                baseLineDetails.BaselineDate = BaselineDate;

                baseLineDetails.ModuleNameLookup = TicketId.Split('-')[0];

                baseLineDetailsManager.Insert(baseLineDetails);

                #endregion

                #region Create task baseline

                DataTable tempPMMTable = new DataTable();

                tempPMMTable.Columns.Add("PMMID", typeof(long));

                tempPMMTable.Columns.Add(DatabaseObjects.Columns.ParentTaskID);

                tempPMMTable.Columns.Add(DatabaseObjects.Columns.Predecessors);

                tempPMMTable.Columns.Add("PMMHistoryID", typeof(long));

                //SPList pmmTasksList = thisWeb.Lists[DatabaseObjects.Lists.PMMTasks];


                //SPList pmmTasksHistory = thisWeb.Lists[DatabaseObjects.Lists.PMMTasksHistory];

                //SPQuery pmmTaskQuery = new SPQuery();
                //pmmTaskQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, pmmId);
                //SPListItemCollection pmmTasks = pmmTasksList.GetItems(pmmTaskQuery);


                //Pass baseline title.split = ticketid
                var taskList = TaskManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");



                foreach (var pmmTaskItem in taskList)
                {
                    ModuleTasksHistory moduleTaskHistory = new ModuleTasksHistory();
                    // task History

                    moduleTaskHistory.Title = pmmTaskItem.Title;

                    moduleTaskHistory.Priority = pmmTaskItem.Priority;

                    moduleTaskHistory.Status = pmmTaskItem.Status;

                    moduleTaskHistory.PercentComplete = Convert.ToInt32(pmmTaskItem.PercentComplete);

                    moduleTaskHistory.AssignedTo = pmmTaskItem.AssignedTo;

                    //pmmTaskHistory.TaskGroup =  pmmTaskItem.TaskGroup;

                    //pmmTaskHistory.Body =pmmTaskItem.Body;

                    moduleTaskHistory.StartDate = pmmTaskItem.StartDate;

                    moduleTaskHistory.DueDate = pmmTaskItem.DueDate;

                    moduleTaskHistory.ParentTaskID = 0;

                    moduleTaskHistory.ItemOrder = pmmTaskItem.ItemOrder;

                    moduleTaskHistory.EstimatedHours = pmmTaskItem.EstimatedHours;

                    moduleTaskHistory.ActualHours = pmmTaskItem.ActualHours;

                    moduleTaskHistory.Behaviour = pmmTaskItem.Behaviour;

                    moduleTaskHistory.StageStep = pmmTaskItem.StageStep;

                    moduleTaskHistory.IsMileStone = pmmTaskItem.IsMileStone;

                    moduleTaskHistory.BaselineDate = BaselineDate;

                    moduleTaskHistory.BaselineId = BaselineNum;

                    moduleTaskHistory.AssignToPct = pmmTaskItem.AssignToPct;

                    moduleTaskHistory.UserSkillMultiLookup = pmmTaskItem.UserSkillMultiLookup;

                    moduleTaskHistory.TicketId = TicketId;

                    moduleTaskHistory.SubTaskType = pmmTaskItem.SubTaskType;

                    _moduleTaskHistoryManager.Insert(moduleTaskHistory);

                    DataRow row = tempPMMTable.NewRow();

                    row["PMMID"] = pmmTaskItem.ID;

                    row[DatabaseObjects.Columns.ParentTaskID] = pmmTaskItem.ParentTaskID;

                    row[DatabaseObjects.Columns.Predecessors] = pmmTaskItem.Predecessors;

                    row["PMMHistoryID"] = moduleTaskHistory.ID;

                    tempPMMTable.Rows.Add(row);
                }

                //Moved Parent relationship from NPR to PMM and  same to predecessors
                if (tempPMMTable != null && tempPMMTable.Rows.Count > 0)
                {
                    foreach (DataRow taskRow in tempPMMTable.Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors])))
                        {
                            string[] predecessors = Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.Predecessors)).Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);

                            List<string> predecessorsIDH = new List<string>();

                            foreach (string predecessor in predecessors)
                            {
                                int predecessorID = 0;

                                int.TryParse(predecessor, out predecessorID);

                                DataRow[] predecessorsRow = tempPMMTable.Select(string.Format("PMMID={0}", predecessorID));//check pmmid

                                if (predecessorsRow.Length > 0)
                                {
                                    predecessorsIDH.Add(Convert.ToString(UGITUtility.GetSPItemValue(predecessorsRow[0], "PMMHistoryID")));

                                }
                            }

                            taskRow[DatabaseObjects.Columns.Predecessors] = string.Join(Constants.Separator, predecessorsIDH.ToArray());
                        }

                        if (Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTaskID]) != string.Empty)
                        {
                            DataRow[] parentRows = tempPMMTable.Select(string.Format("PMMID={0}", taskRow[DatabaseObjects.Columns.ParentTaskID]));
                            if (parentRows.Length > 0)
                            {
                                taskRow[DatabaseObjects.Columns.ParentTaskID] = parentRows[0]["PMMHistoryID"];
                            }
                        }
                    }
                }

                //Saved Parent relationship from NPR to PMM and  same to predecessors in pmm task history

                //SPQuery pmmHTaskQuery = new SPQuery();
                //pmmHTaskQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}'  /><Value Type='Number'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, pmmId, DatabaseObjects.Columns.BaselineNum, BaselineNum);
                // SPListItemCollection mypmmHTasks = pmmTasksHistory.GetItems(pmmHTaskQuery);

                List<ModuleTasksHistory> pmmTaskList = _moduleTaskHistoryManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}' and {DatabaseObjects.Columns.BaselineId}={BaselineNum} ");

                if (tempPMMTable != null && tempPMMTable.Rows.Count > 0)
                {
                    foreach (DataRow taskRow in tempPMMTable.Rows)
                    {
                        pmmtaskHistory = new ModuleTasksHistory();

                        pmmtaskHistory = pmmTaskList.FirstOrDefault(x => x.ID == Convert.ToInt32(taskRow["PMMHistoryID"]));

                        //SPListItem item = SPListHelper.GetItemByID(mypmmHTasks, Convert.ToInt32(taskRow["PMMHistoryID"]));

                        if (pmmtaskHistory != null)
                        {
                            parentTaskId = Convert.ToInt32(taskRow[DatabaseObjects.Columns.ParentTaskID]);

                            string[] predecessors = Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.PredecessorsID)).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);

                            //SPFieldLookupValueCollection predecessorCollection = new SPFieldLookupValueCollection();

                            List<string> predecessorCollection = new List<string>();

                            foreach (string predecessor in predecessors)
                            {
                                int predecessorID = 0;

                                int.TryParse(predecessor, out predecessorID);
                                //SPFieldLookupValue lookup = new SPFieldLookupValue();
                                if (predecessorID > 0)
                                {
                                    //lookup.LookupId = predecessorID;
                                    predecessorCollection.Add(Convert.ToString(predecessorID));
                                }
                            }

                            // pmmtaskHistory.Predecessors = predecessorCollection;

                            pmmtaskHistory.ParentTaskID = parentTaskId;

                            // _moduleTaskHistoryManager.Insert(pmmtaskHistory);
                            _moduleTaskHistoryManager.Update(pmmtaskHistory);

                        }
                    }
                }


                #endregion

                #region Create budget baseline
                #region Module budget Baseline
                var pmmBudgetsList = _moduleBudgetManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");
                foreach (var pmmBudget in pmmBudgetsList)
                {
                    ModuleBudgetHistory moduleBudgetHistory = new ModuleBudgetHistory();

                    moduleBudgetHistory.TicketId = pmmBudget.TicketId;

                    moduleBudgetHistory.BudgetItem = pmmBudget.BudgetItem;

                    moduleBudgetHistory.AllocationStartDate = pmmBudget.AllocationStartDate;

                    moduleBudgetHistory.AllocationEndDate = pmmBudget.AllocationEndDate;

                    moduleBudgetHistory.BudgetDescription = pmmBudget.BudgetDescription;

                    moduleBudgetHistory.BudgetLookup = pmmBudget.BudgetCategoryLookup;

                    moduleBudgetHistory.BudgetAmount = pmmBudget.BudgetAmount;

                    moduleBudgetHistory.IsAutoCalculated = pmmBudget.IsAutoCalculated;

                    moduleBudgetHistory.BaselineDate = BaselineDate;

                    moduleBudgetHistory.BaselineId = BaselineNum;

                    _moduleBudgetHistoryManager.Insert(moduleBudgetHistory);
                }
                #endregion Module budget Baseline

                #region ModuleBudgetActuals Baseline
                var _budgetActualsList = _budgetActualsManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");
                foreach (var item in _budgetActualsList)
                {
                    ModuleBudgetsActualHistory moduleBudgetsActualHistory = new ModuleBudgetsActualHistory();

                    //Need to check which column is left.

                    moduleBudgetsActualHistory.AllocationEndDate = item.AllocationEndDate;

                    moduleBudgetsActualHistory.AllocationStartDate = item.AllocationStartDate;

                    moduleBudgetsActualHistory.BudgetAmount = item.BudgetAmount;

                    moduleBudgetsActualHistory.BudgetDescription = item.BudgetDescription;

                    moduleBudgetsActualHistory.ModuleName = item.ModuleName;

                    moduleBudgetsActualHistory.TicketId = item.TicketId;

                    moduleBudgetsActualHistory.Title = item.Title;

                    moduleBudgetsActualHistory.VendorName = item.VendorName;

                    moduleBudgetsActualHistory.VendorLookup = item.VendorLookup;

                    moduleBudgetsActualHistory.InvoiceNumber = item.InvoiceNumber;

                    moduleBudgetsActualHistory.ModuleBudgetLookup = item.ModuleBudgetLookup;

                    moduleBudgetsActualHistory.BaselineId = BaselineNum;

                    moduleBudgetsActualHistory.BaselineDate = BaselineDate;

                    _moduleBudgetActualsHistoryManager.Insert(moduleBudgetsActualHistory);

                }
                #endregion ModuleBudgetActuals Baseline
                #endregion Create budget baseline

                #region Create monthly budget baseline
                var ModuleMonthlyBudgetList =ModuleMonthlyBudgetManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");

                foreach (var item in ModuleMonthlyBudgetList)
                {
                    ModuleMonthlyBudgetHistory moduleMonthlyBudgetHistory = new ModuleMonthlyBudgetHistory();

                    moduleMonthlyBudgetHistory.TicketId = TicketId;

                    moduleMonthlyBudgetHistory.BudgetAmount= item.BudgetAmount;

                    moduleMonthlyBudgetHistory.AllocationStartDate = item.AllocationStartDate;

                    moduleMonthlyBudgetHistory.BudgetType = item.BudgetType;

                    moduleMonthlyBudgetHistory.BaselineDate = BaselineDate;

                    moduleMonthlyBudgetHistory.BaselineId = BaselineNum;

                    moduleMonthlyBudgetHistory.ActualCost = item.ActualCost;

                    moduleMonthlyBudgetHistory.ModuleName = item.ModuleName;

                    moduleMonthlyBudgetHistory.BudgetLookup = item.BudgetCategoryLookup;

                    moduleMonthlyBudgetHistory.ProjectActualTotal = item.ProjectActualTotal;

                    ModuleMonthlyBudgetHistoryManager.Insert(moduleMonthlyBudgetHistory);
                    
                }

                #endregion

                #region Create comments baseline

                //  var pmmcommentList=PMMCommentManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}' and {DatabaseObjects.Columns.IsDeletedColumn}=1");
                var pmmcommentList = PMMCommentManager.Load($"{DatabaseObjects.Columns.TicketPMMIdLookup}={pmmId}");

                foreach (var pmmComment in pmmcommentList)
                {
                    PMMCommentHistory pmmCommentHistory = new PMMCommentHistory();

                    pmmCommentHistory.TicketId = TicketId;

                    pmmCommentHistory.PMMIdLookup = pmmId;

                    pmmCommentHistory.ProjectNoteType = pmmComment.ProjectNoteType;

                    pmmCommentHistory.ProjectNote = pmmComment.ProjectNote;

                    pmmCommentHistory.Title = pmmComment.Title;

                    pmmCommentHistory.EndDate = pmmComment.EndDate;

                    pmmCommentHistory.AccomplishmentDate = pmmComment.AccomplishmentDate;

                    pmmCommentHistory.BaselineDate = BaselineDate;

                    pmmCommentHistory.BaselineId = BaselineNum;

                    PMMCommentHistoryManager.Insert(pmmCommentHistory);
                }
                #endregion

                #region Create PMM Project baseline

                pmmItem.SetAdded();                

                pmmItem.Table.Columns.Add(DatabaseObjects.Columns.BaselineDate);
                pmmItem.Table.Columns.Add(DatabaseObjects.Columns.BaselineId);

                pmmItem[DatabaseObjects.Columns.BaselineDate] = BaselineDate;
                pmmItem[DatabaseObjects.Columns.BaselineId] = BaselineNum;

                var pmmHistoryStatus = ticketDal.SaveHistory(DatabaseObjects.Tables.PMMHistory, pmmItem);

                #endregion

                #region Create decisionlog baseline

               var listOfDecisionLog= DecisionLogManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");

                foreach (var itemDecisionLog in listOfDecisionLog)
                {
                    DecisionLogHistory decisionLogHistory = new DecisionLogHistory();

                    decisionLogHistory.Title = itemDecisionLog.Title;

                    decisionLogHistory.ReleaseDate = itemDecisionLog.ReleaseDate;

                    decisionLogHistory.DecisionStatus = itemDecisionLog.DecisionStatus;

                    decisionLogHistory.AssignedTo = itemDecisionLog.AssignedTo;

                    decisionLogHistory.DecisionMaker = itemDecisionLog.DecisionMaker;

                    decisionLogHistory.BaselineId = BaselineNum;

                    decisionLogHistory.BaselineDate = BaselineDate;

                    decisionLogHistory.TicketId = TicketId;

                    DecisionLogHistoryManager.Insert(decisionLogHistory);

                }

                #endregion

                #region  Create monitory baseline
                #region ProjectMonitorState

                var ProjectMonitorStateList = ProjectMonitorStateManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketId}'");

                foreach (var pmmMonitor in ProjectMonitorStateList)
                {

                    ProjectMonitorStateHistory projectMonitorStateHistory = new ProjectMonitorStateHistory();
                    //check thoroughly
                    projectMonitorStateHistory.TicketId = TicketId;

                    projectMonitorStateHistory.Title = pmmMonitor.Title;

                    projectMonitorStateHistory.ModuleName = pmmMonitor.ModuleNameLookup;

                    projectMonitorStateHistory.ModuleMonitorOptionName = pmmMonitor.ModuleMonitorOptionName;

                    projectMonitorStateHistory.ModuleMonitorNameLookup = pmmMonitor.ModuleMonitorNameLookup;

                    projectMonitorStateHistory.ProjectMonitorWeight = pmmMonitor.ProjectMonitorWeight;

                    projectMonitorStateHistory.ProjectMonitorNotes = pmmMonitor.ProjectMonitorNotes;

                    projectMonitorStateHistory.ModuleMonitorOptionLEDClass = pmmMonitor.ModuleMonitorOptionLEDClass;

                    projectMonitorStateHistory.ModuleMonitorOptionIdLookup = pmmMonitor.ModuleMonitorOptionIdLookup;

                    projectMonitorStateHistory.BaselineDate = BaselineDate;

                    projectMonitorStateHistory.BaselineId = BaselineNum;

                    projectMonitorStateHistory.AutoCalculate = pmmMonitor.AutoCalculate;

                    projectMonitorStateHistory.PMMIdLookup = pmmId;


                    ProjectMonitorStateHistoryManager.Insert(projectMonitorStateHistory);
                }
                #endregion

                #region projectMonitorOptions

                //need to add filter

                //var moduleMonitorOptionHistories = ModuleMonitorOptionManager.Load();

                //foreach (var moduleMonitorOption in moduleMonitorOptionHistories)
                //{
                //    ModuleMonitorOptionHistory moduleMonitorOptionHistory = new ModuleMonitorOptionHistory();

                //    moduleMonitorOptionHistory.ModuleMonitorMultiplier = moduleMonitorOption.ModuleMonitorMultiplier;

                //    moduleMonitorOptionHistory.ModuleMonitorOptionName = moduleMonitorOption.ModuleMonitorOptionName;

                //    moduleMonitorOptionHistory.ModuleMonitorOptionLEDClass = moduleMonitorOption.ModuleMonitorOptionLEDClass;

                //    moduleMonitorOptionHistory.IsDefault = moduleMonitorOption.IsDefault;

                //    moduleMonitorOptionHistory.BaselieDate = BaselineDate;

                //    moduleMonitorOptionHistory.BaselineId = BaselineNum;

                //    ModuleMonitorOptionHistoryManager.Insert(moduleMonitorOptionHistory);

                //}

                #endregion
                
                #endregion

                #region Update PMM Ticket
                // Gives an ERROR - column BaselineNum does not exist in PMMProjects list!
                //SPQuery pmmlookupQuery = new SPQuery();
                //pmmlookupQuery.Query = string.Format("<Where><Eq><FieldRef Name='TicketPMMIdLookup' LookupId='TRUE'/><Value Type='Lookup'>{0}</Value></Eq></Where>", pmmId);
                //SPListItemCollection baselineDetails = pmmBaselineList.GetItems(pmmlookupQuery);
                //pmmItem[DatabaseObjects.Columns.BaselineNum] = baselineDetails.Count; // Update # of baselines kept

                //pmmItem.UpdateOverwriteVersion();
                //Ticket tkt = new Ticket(ApplicationContext, "PMM");

                //On condition
                
                   Ticket historyTicket = new Ticket(_applicationContext, moduleName);

                   var pmmItemHistory= TicketManager.GetByTicketID(module, TicketId);

                    string historyTxt = string.Format("{0} Baseline created", Baseline.GetBaselines(Convert.ToString(pmmItem[DatabaseObjects.Columns.TicketId]), _applicationContext).Count);

                    uHelper.CreateHistory(_applicationContext.CurrentUser, historyTxt, pmmItemHistory, false,_applicationContext);

                    historyTicket.CommitChanges(pmmItemHistory);

               
                #endregion

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "CreateBaseline");
            }
        }

        public static List<Baseline> GetBaselines(string ticketId, ApplicationContext context)
        {
            List<Baseline> baselines = new List<Baseline>();

            BaseLineDetailsManager baseLineDetailsManager = new BaseLineDetailsManager(context);

            var pmmDetailsList = baseLineDetailsManager.Load($"{DatabaseObjects.Columns.TicketId}='{ticketId}'").OrderBy(x => x.BaselineId);

            foreach (var item in pmmDetailsList)
            {
                DateTime baseLineDate = Convert.ToDateTime(item.BaselineDate).ToUniversalTime();

                Baseline baseline = new Baseline(ticketId, baseLineDate, context);

                baseline.BaselineDetailID = item.ID;

                baseline.BaselineNum = item.BaselineId;

                baseline.BaselineComment = item.BaselineComment;

                baselines.Add(baseline);
            }

            return baselines;
        }

        public static DataRow LoadProjectBaseline(DataRow pmmItem, double baselineDetailID, ApplicationContext context)
        {

            var pmmProjectData = GetTableDataManager.GetTableData("PMMHistory", $"{DatabaseObjects.Columns.TicketId}='{pmmItem[DatabaseObjects.Columns.TicketId]}' and {DatabaseObjects.Columns.BaselineId}={baselineDetailID} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select();


            if (pmmProjectData != null && pmmProjectData.Count() > 0)
            {
                return pmmProjectData[0];

            }

            return pmmItem;
        }

        public static List<ModuleBudgetHistory> LoadBudgetBaseLine(ApplicationContext applicationContext, int baselineId, string ticketId)
        {
            ModuleBudgetHistoryManager _moduleBudgetHistoryManager = new ModuleBudgetHistoryManager(applicationContext);
            List<ModuleBudgetHistory> moduleBudgetHistories = new List<ModuleBudgetHistory>();

            try
            {

            var moduleBudgetHistoryList = _moduleBudgetHistoryManager.Load($"{DatabaseObjects.Columns.BaselineId}={baselineId} and {DatabaseObjects.Columns.TicketId}='{ticketId}'");


                foreach (var item in moduleBudgetHistoryList)
                {
                    ModuleBudgetHistory moduleBudgetHistory = new ModuleBudgetHistory();

                    moduleBudgetHistory.ID = item.ID;
                    moduleBudgetHistory.AllocationStartDate = item.AllocationStartDate;
                    moduleBudgetHistory.AllocationEndDate = item.AllocationEndDate;
                    moduleBudgetHistory.BudgetItem = item.BudgetItem;
                    moduleBudgetHistory.BudgetAmount = item.BudgetAmount;
                    moduleBudgetHistory.BudgetDescription = item.BudgetDescription;
                    //moduleBudgetHistory.ModuleId = item.PMMId;
                    moduleBudgetHistory.IsAutoCalculated = item.IsAutoCalculated;
                    moduleBudgetHistory.BudgetStatus = (int)Enums.BudgetStatus.Approve;
                    moduleBudgetHistory.UnapprovedAmount = 0;

                    //moduleBudgetHistory.BudgetCategory = item.budgetCategory.Level1;
                   // moduleBudgetHistory.BudgetSubCategory] = item.budgetCategory.Level2;
                    //row["BudgetSubCategoryID"] = item.budgetCategory.Id;

                    //result.Rows.Add(row);
                }
                return moduleBudgetHistoryList;
            }

            catch (Exception ex )
            {
                throw ex;
            }
        }
    }
}

