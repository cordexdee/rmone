using System.Data;
using System.Linq;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Manager.PMM;
using System;
using System.Collections.Generic;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
using uGovernIT.Util.ImportExportMPP;
using System.Threading;

namespace uGovernIT.Manager.Managers
{
    //public interface IProjectManager : ITicketStore
    //{
    //    DataTable GetAllTickets(UGITModule module);
    //    DataTable GetTableSchemaDetail(string tableName, string moduleName);
    //    DataTable GetColumnDetail(string tableName, string moduleName);
    //}

    public class ProjectManager : TicketStore  //, IProjectManager
    {
        private ApplicationContext applicationContext;
        ConfigurationVariableManager configManager;
        UserProfileManager UserManager;

        public ProjectManager(ApplicationContext context) : base(context)
        {
            applicationContext = context;
            configManager = new ConfigurationVariableManager(context);
            UserManager = context.UserManager;
        }


        /// <summary>
        /// This Method is used to create PMM ticket from given options
        /// </summary>
        /// <param name="projectRequest"></param>
        /// <returns></returns>
        public string CreatePMM(NewProjectRequest projectRequest, TicketManager ticketManager)
        {
            string ticketId = string.Empty;
            string error = string.Empty;
            Ticket objTicket = new Ticket(applicationContext, ModuleNames.PMM, applicationContext.CurrentUser);

            if (projectRequest.Mode == PMMMode.Scratch)
            {
                error = CreateDefaultPMM(projectRequest, ticketManager, objTicket, ref ticketId);
                if (!string.IsNullOrWhiteSpace(ticketId))
                {
                    UGITTaskManager taskManager = new UGITTaskManager(applicationContext);
                    taskManager.ImportAllTasksUnderPMM(projectRequest.Mode, string.Empty, ticketId);
                }
            }
            else if (projectRequest.Mode == PMMMode.NPR || projectRequest.Mode == PMMMode.PMM)
            {
                ModuleViewManager moduleManager = new ModuleViewManager(applicationContext);
                UGITModule module = moduleManager.GetByName(projectRequest.Mode.ToString());

                // Get Table schema from existing ticket table and load the existing ticket
                DataTable dtTickets = ticketManager.GetTableSchemaDetail(module.ModuleTable, string.Empty);
                DataRow drow = dtTickets.NewRow();
                drow = Ticket.GetCurrentTicket(applicationContext, module.ModuleName, projectRequest.SelectedItem);

                if (drow == null)
                {
                    error = "Selected Ticket is not found.";
                    ULog.WriteLog(error);
                    return error;
                }

                bool isNPRToPMM = projectRequest.Mode == PMMMode.NPR;
                error = CreateProject(applicationContext, ticketManager, objTicket, drow, projectRequest, ref ticketId, isNPRToPMM);

                if (!string.IsNullOrEmpty(error))
                    ULog.WriteLog(error);

            }
            else if (projectRequest.Mode == PMMMode.MPP || projectRequest.Mode == PMMMode.Template)
            {
                // Create a default PMM project
                error = CreateDefaultPMM(projectRequest, ticketManager, objTicket, ref ticketId);

                if (!string.IsNullOrEmpty(ticketId))
                {
                    if (projectRequest.Mode == PMMMode.MPP)
                    {
                        // Added 27 jan 2020
                        ImportTask_MSProject(projectRequest.SelectedItem, ticketId, false, false, false);
                    }
                    else
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(applicationContext);
                        taskManager.ImportAllTasksUnderPMM(projectRequest.Mode, projectRequest.SelectedItem, ticketId);
                    }
                }
            }

            if (string.IsNullOrEmpty(ticketId))
                return error;

            return ticketId;
        }
        
        // Added 27 jan 2020
        public void ImportTask_MSProject(string strFileName, string newprojectID, bool dontimportpredecessors, bool dontImportAssignees, bool calculateEstmHrs)
        {
            ConfigSetiingMpp mppSetting = new ConfigSetiingMpp()
            {
                dontImportAssignee = dontImportAssignees,
                importDatesOnly= dontimportpredecessors,
                calculateEstimatedHrs= calculateEstmHrs,
                EnableExportImport = true,
                UseMSProject = true
            };
            string Modulename = uHelper.getModuleNameByTicketId(newprojectID);
            string filePath = uHelper.GetTempFolderPath();
            MPXJClass mPXJClass = new MPXJClass();
            Project proj = mPXJClass.GetMPXJResult(strFileName, mppSetting, filePath);
            UGITTaskManager taskManager = new UGITTaskManager(applicationContext);
            List<UGITTask> tasklist = taskManager.LoadByProjectID(newprojectID);

            if (proj.taskList != null && proj.taskList.Count > 0)
            {
                if (tasklist != null && tasklist.Count > 0)
                {
                    foreach (UGITTask task in tasklist)
                    {
                        taskManager.Delete(task);
                    }
                }
                List<UGITTask> tasks =  MapProjectTaskWithUGITTasks(proj);

                // Load project to get some datail like start and endDate
                DataRow project = Ticket.GetCurrentTicket(applicationContext, Modulename, newprojectID);
                DateTime projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate] == DBNull.Value ? DateTime.MinValue : project[DatabaseObjects.Columns.TicketActualStartDate]);

                if (projectStartDate == DateTime.MinValue)
                    projectStartDate = DateTime.Now;

                // Fetch lifecycle of PMM project
                LifeCycleManager lifeCycleHelper = new LifeCycleManager(applicationContext);
                LifeCycle lifeCycle = lifeCycleHelper.LoadLifeCycleByModule(Modulename)[0];

                // update Start/Due dates of all tasks based on the project start date
                //tasks = taskManager.SetTaskDatesFromProjectStartDate(lifeCycle, projectStartDate, tasks);

                // Remanage all tasks i.e. estimate durations, update Successor & Parent Dates
                taskManager.ReManageTasks(ref tasks);

                if (tasks != null && tasks.Count > 0)
                {
                    List<UGITTask> tasksList = UGITTaskManager.MapRelationalObjects(tasks);
                    taskManager.ImportTasks(Modulename, tasksList, false, newprojectID);

                    bool autoCreateRMMProjectAllocation = configManager.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                    if (autoCreateRMMProjectAllocation)
                    {
                        ResourceAllocationManager allocationManager = new ResourceAllocationManager(applicationContext);
                        allocationManager.UpdateProjectPlannedAllocationByUser(tasksList, Modulename, Convert.ToString(project[DatabaseObjects.Columns.TicketId]), false);
                        //removed thread code because internal code already working in thread
                        //ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { allocationManager.UpdateProjectPlannedAllocationByUser( tasksList, Modulename, Convert.ToString(project[DatabaseObjects.Columns.TicketId]), false); };
                        //Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                        //sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                        //sThreadUpdateProjectPlannedAllocation.Start();

                    }
                }
            }
        }

        public List<UGITTask> MapProjectTaskWithUGITTasks(Project proj)
        {
            List<UGITTask> tasklist = new List<UGITTask>();
            foreach(TaskList task in proj.taskList)
            {
                UGITTask uTask = new UGITTask();
                uTask.ID = task.Id;
                uTask.Title = task.Name;
                if (!string.IsNullOrEmpty(task.strMainAssignToPct))
                {
                    List<string> lstAssignedToUsers = new List<string>();
                    string[] lstAssignToPct = UGITUtility.SplitString(task.strMainAssignToPct, Constants.Separator);
                    foreach(string s in lstAssignToPct)
                    {
                        string[] lstAssignToUser = UGITUtility.SplitString(s, Constants.Separator1);
                        if (!string.IsNullOrEmpty(lstAssignToUser[0]))
                        {
                            UserProfile userP = UserManager.GetUserByBothUserNameandDisplayName(lstAssignToUser[0]);
                            if (userP != null)
                                lstAssignedToUsers.Add(userP.Id);
                        }
                    }
                    uTask.AssignedTo = UGITUtility.ConvertListToString(lstAssignedToUsers, Constants.Separator6);
                }
                
                uTask.AssignToPct = task.strMainAssignToPct;
                uTask.EstimatedHours = task.strmainCalculateEstHrsToPct;
                
                uTask.StartDate = task.Start;
                uTask.DueDate = task.Finish;
                uTask.Description = task.Notes;
                uTask.Duration = task.Duration;
                uTask.EndDate = task.Finish;
                uTask.PercentComplete = task.PercentageComplete;
                uTask.Status = task.Status;
                uTask.ParentTaskID = Convert.ToInt64(task.ParentTask);
                if (task.taskIds != null && task.taskIds.Count > 0)
                    uTask.Predecessors = string.Join(Constants.Separator6, task.taskIds);
                tasklist.Add(uTask);

            }
            return tasklist;
        }

        /// <summary>
        /// This method is used to create PMM from Scratch
        /// </summary>
        /// <param name="projectRequest"></param>
        /// <returns></returns>
        protected string CreateDefaultPMM(NewProjectRequest projectRequest, TicketManager ticketManager, Ticket objTicket, ref string ticketId)
        {
            DataTable dtPMMTickets = ticketManager.GetTableSchemaDetail(DatabaseObjects.Tables.PMMProjects, string.Empty);
            DataRow drow = dtPMMTickets.NewRow();

            drow[DatabaseObjects.Columns.Title] = projectRequest.Title;

            //Check if StartDate i.e. TicketTargetStartDate is provided 
            bool isValidStartDate = projectRequest.StartDate != null && projectRequest.StartDate.HasValue && projectRequest.StartDate.Value != DateTime.MinValue
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCreationDate, drow.Table);

            if (isValidStartDate)
                drow[DatabaseObjects.Columns.TicketTargetStartDate] = projectRequest.StartDate.Value;
            else
                drow[DatabaseObjects.Columns.TicketTargetStartDate] = DateTime.Now;

            SetDefaultLifeCycleSettingOnProject(applicationContext, drow, projectRequest.LifeCycle);

            objTicket.Create(drow, applicationContext.CurrentUser);
            string error = objTicket.CommitChanges(drow);

            if (!string.IsNullOrEmpty(error))
            {
                ULog.WriteLog(error);
                return error;
            }

            ticketId = Convert.ToString(drow[DatabaseObjects.Columns.TicketId]);
            //create default monitors
            CreateProjectDefaultMonitors(drow, objTicket);
            return ticketId;
        }

        /// <summary>
        /// This method is used to Create PMM from selected NPR/PMM Ticket
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="item"></param>
        /// <param name="projectRequest"></param>
        /// <param name="ticketId"></param>
        /// <param name="isNPRToPMM"></param>
        /// <returns></returns>
        public string CreateProject(ApplicationContext applicationContext, TicketManager ticketManager, Ticket pmmTicketRequest, DataRow item, NewProjectRequest projectRequest, ref string ticketId, bool isNPRToPMM)
        {
            string error = string.Empty;
            DataTable pmmTickets = ticketManager.GetTableSchemaDetail(DatabaseObjects.Tables.PMMProjects, string.Empty);
            ModuleViewManager moduleManager = new ModuleViewManager(applicationContext);

            #region Set all fields and create PMM ticket

            Ticket nprTicket = null;
            DataRow pmmNewTicket = pmmTickets.NewRow();

            pmmNewTicket[DatabaseObjects.Columns.TicketId] = pmmTicketRequest.GetNewTicketId();

            // Copied basic info
            string pmmProjectName = !string.IsNullOrEmpty(projectRequest.Title) ? projectRequest.Title : Convert.ToString(item[DatabaseObjects.Columns.Title]);
            pmmNewTicket[DatabaseObjects.Columns.Title] = pmmProjectName;
            pmmNewTicket[DatabaseObjects.Columns.TicketDescription] = item[DatabaseObjects.Columns.TicketDescription];
            SetDefaultLifeCycleSettingOnProject(applicationContext, pmmNewTicket, projectRequest.LifeCycle);

            pmmNewTicket[DatabaseObjects.Columns.TicketProjectManager] = item[DatabaseObjects.Columns.TicketProjectManager];
            pmmNewTicket[DatabaseObjects.Columns.TicketSponsors] = item[DatabaseObjects.Columns.TicketSponsors];
            pmmNewTicket[DatabaseObjects.Columns.TicketStakeHolders] = item[DatabaseObjects.Columns.TicketStakeHolders];
            pmmNewTicket[DatabaseObjects.Columns.TicketBeneficiaries] = item[DatabaseObjects.Columns.TicketBeneficiaries];
            pmmNewTicket[DatabaseObjects.Columns.CompanyMultiLookup] = item[DatabaseObjects.Columns.CompanyMultiLookup];
            pmmNewTicket[DatabaseObjects.Columns.DivisionMultiLookup] = item[DatabaseObjects.Columns.DivisionMultiLookup];
            pmmNewTicket[DatabaseObjects.Columns.TicketDescription] = item[DatabaseObjects.Columns.TicketDescription];

            // Copy Project Measures and contraints
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalCost] = item[DatabaseObjects.Columns.TicketTotalCost];
            pmmNewTicket[DatabaseObjects.Columns.ProjectCost] = 0;
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalStaffHeadcount] = item[DatabaseObjects.Columns.TicketTotalStaffHeadcount];
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcount];
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcount];
            pmmNewTicket[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = item[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketRiskScore] = item[DatabaseObjects.Columns.TicketRiskScore];
            pmmNewTicket[DatabaseObjects.Columns.TicketRiskScoreNotes] = item[DatabaseObjects.Columns.TicketRiskScoreNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketArchitectureScore] = item[DatabaseObjects.Columns.TicketArchitectureScore];
            pmmNewTicket[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = item[DatabaseObjects.Columns.TicketArchitectureScoreNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketProjectScoreNotes] = item[DatabaseObjects.Columns.TicketProjectScoreNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketNoOfFTEs] = item[DatabaseObjects.Columns.TicketNoOfFTEs];
            pmmNewTicket[DatabaseObjects.Columns.TicketNoOfConsultants] = item[DatabaseObjects.Columns.TicketNoOfConsultants];
            pmmNewTicket[DatabaseObjects.Columns.TicketNoOfConsultantsNotes] = item[DatabaseObjects.Columns.TicketNoOfConsultantsNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketNoOfFTEsNotes] = item[DatabaseObjects.Columns.TicketNoOfFTEsNotes];
            pmmNewTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[DatabaseObjects.Columns.TicketDesiredCompletionDate];

            pmmNewTicket[DatabaseObjects.Columns.UGITDaysToComplete] = pmmNewTicket[DatabaseObjects.Columns.TicketDuration];
            pmmNewTicket[DatabaseObjects.Columns.TicketPctComplete] = 0;
            pmmNewTicket[DatabaseObjects.Columns.IsPrivate] = item[DatabaseObjects.Columns.IsPrivate];
            pmmNewTicket[DatabaseObjects.Columns.ProjectRank] = item[DatabaseObjects.Columns.ProjectRank];
            pmmNewTicket[DatabaseObjects.Columns.ProjectRank2] = item[DatabaseObjects.Columns.ProjectRank2];
            pmmNewTicket[DatabaseObjects.Columns.ProjectRank3] = item[DatabaseObjects.Columns.ProjectRank3];
            pmmNewTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now;

            // Set Priority and Request Types fields acoording to below condition
            if (isNPRToPMM)
            {
                nprTicket = new Ticket(applicationContext, ModuleNames.NPR);
                pmmNewTicket[DatabaseObjects.Columns.TicketNPRIdLookup] = item[DatabaseObjects.Columns.Id];

                // Get priority in NPR and set in pmm project
                string nprPriority = UGITUtility.SplitString(item[DatabaseObjects.Columns.TicketPriorityLookup], Constants.Separator, 1);

                if (!string.IsNullOrEmpty(nprPriority))
                {
                    PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(applicationContext);
                    List<ModulePrioirty> modulePrioirties = prioirtyViewManager.LoadByModule(ModuleNames.PMM);

                    if (modulePrioirties != null && modulePrioirties.Count > 0)
                    {
                        ModulePrioirty modulePrioirty = modulePrioirties.Where(x => x.uPriority == nprPriority).FirstOrDefault();

                        if (modulePrioirty != null)
                            pmmNewTicket[DatabaseObjects.Columns.TicketPriorityLookup] = modulePrioirty.ID;
                    }
                }

                //Import NPR Request Type equivalent request type from PMM
                string nprRequestTypeLookup = UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketRequestTypeLookup));

                if (!string.IsNullOrEmpty(nprRequestTypeLookup))
                {
                    int nprRequesttypeid = UGITUtility.StringToInt(nprRequestTypeLookup);
                    ModuleRequestType nprRequestType = nprTicket.Module.List_RequestTypes.FirstOrDefault(x => x.ID == nprRequesttypeid);

                    if (nprRequestType != null)
                    {
                        ModuleRequestType pmmRequestType = pmmTicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.Category == nprRequestType.Category); //&& x.ID == nprRequestTypeLookup need discusion for check

                        if (pmmRequestType != null)
                            pmmNewTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = pmmRequestType.ID;
                    }
                }
            }
            else
            {
                pmmNewTicket[DatabaseObjects.Columns.TicketPriorityLookup] = item[DatabaseObjects.Columns.TicketPriorityLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = item[DatabaseObjects.Columns.TicketRequestTypeLookup];
            }

            pmmNewTicket[DatabaseObjects.Columns.FunctionalAreaLookup] = item[DatabaseObjects.Columns.FunctionalAreaLookup];
            pmmNewTicket[DatabaseObjects.Columns.ProjectInitiativeLookup] = item[DatabaseObjects.Columns.ProjectInitiativeLookup];
            pmmNewTicket[DatabaseObjects.Columns.TicketOwner] = item[DatabaseObjects.Columns.TicketOwner];
            pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFE] = item[DatabaseObjects.Columns.TicketApprovedRFE];
            pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFEAmount] = item[DatabaseObjects.Columns.TicketApprovedRFEAmount];
            pmmNewTicket[DatabaseObjects.Columns.TicketApprovedRFEType] = item[DatabaseObjects.Columns.TicketApprovedRFEType];
            try
            {
                pmmNewTicket[DatabaseObjects.Columns.TicketRequestor] = item[DatabaseObjects.Columns.TicketRequestor];
                pmmNewTicket[DatabaseObjects.Columns.PRPGroup] = item[DatabaseObjects.Columns.PRPGroup];
                pmmNewTicket[DatabaseObjects.Columns.LocationMultLookup] = item[DatabaseObjects.Columns.LocationMultLookup];
                pmmNewTicket[DatabaseObjects.Columns.APPTitleLookup] = item[DatabaseObjects.Columns.APPTitleLookup];
                pmmNewTicket[DatabaseObjects.Columns.ModuleNameLookup] = item[DatabaseObjects.Columns.ModuleNameLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectScope] = item[DatabaseObjects.Columns.TicketProjectScope];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectAssumptions] = item[DatabaseObjects.Columns.TicketProjectAssumptions];
                pmmNewTicket[DatabaseObjects.Columns.TicketProjectBenefits] = item[DatabaseObjects.Columns.TicketProjectBenefits];
                pmmNewTicket[DatabaseObjects.Columns.ProblemBeingSolved] = item[DatabaseObjects.Columns.ProblemBeingSolved];
                pmmNewTicket[DatabaseObjects.Columns.ProjectRiskNotes] = item[DatabaseObjects.Columns.ProjectRiskNotes];
                pmmNewTicket[DatabaseObjects.Columns.ProjectClassLookup] = item[DatabaseObjects.Columns.ProjectClassLookup];
                pmmNewTicket[DatabaseObjects.Columns.TicketBusinessManager] = item[DatabaseObjects.Columns.TicketBusinessManager];
                pmmNewTicket[DatabaseObjects.Columns.TicketClassification] = item[DatabaseObjects.Columns.TicketClassification];
                pmmNewTicket[DatabaseObjects.Columns.TicketClassificationType] = item[DatabaseObjects.Columns.TicketClassificationType];
                pmmNewTicket[DatabaseObjects.Columns.TicketClassificationImpact] = item[DatabaseObjects.Columns.TicketClassificationImpact];
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            //New PMO Assessment Fields
            try
            {
                pmmNewTicket["BreakEvenIn"] = item["BreakEvenIn"];
                pmmNewTicket["EliminatesHeadcount"] = item["EliminatesHeadcount"];
                pmmNewTicket["ClassificationNotes"] = item["ClassificationNotes"];
                pmmNewTicket["ImpactBusinessGrowthChoice"] = item["ImpactBusinessGrowthChoice"];
                pmmNewTicket["ImpactDecisionMakingChoice"] = item["ImpactDecisionMakingChoice"];
                pmmNewTicket["ImpactIncreasesProductivityChoice"] = item["ImpactIncreasesProductivityChoice"];
                pmmNewTicket["ImpactReducesExpensesChoice"] = item["ImpactReducesExpensesChoice"];
                pmmNewTicket["ImpactReducesRiskChoice"] = item["ImpactReducesRiskChoice"];
                pmmNewTicket["ImpactRevenueIncreaseChoice"] = item["ImpactRevenueIncreaseChoice"];
                pmmNewTicket["IsITGApprovalRequired"] = item["IsITGApprovalRequired"];
                pmmNewTicket["OtherDescribe"] = item["OtherDescribe"];
                pmmNewTicket["ContributionToStrategyChoice"] = item["ContributionToStrategyChoice"];
                pmmNewTicket["PaybackCostSavingsChoice"] = item["PaybackCostSavingsChoice"];
                pmmNewTicket["CustomerBenefitChoice"] = item["CustomerBenefitChoice"];
                pmmNewTicket["RegulatoryChoice"] = item["RegulatoryChoice"];
                pmmNewTicket["ITLifecycleRefreshChoice"] = item["ITLifecycleRefreshChoice"];
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            //Anaylsis Fields
            try
            {
                pmmNewTicket[DatabaseObjects.Columns.ProjectComplexity] = item[DatabaseObjects.Columns.ProjectComplexity];
                pmmNewTicket["ClassificationScopeChoice"] = item["ClassificationScopeChoice"];
                pmmNewTicket["ProjectEstDurationMaxDays"] = item["ProjectEstDurationMaxDays"];
                pmmNewTicket["ProjectEstDurationMinDays"] = item["ProjectEstDurationMinDays"];
                pmmNewTicket["ProjectEstSizeMaxHrs"] = item["ProjectEstSizeMaxHrs"];
                pmmNewTicket["ProjectEstSizeMinHrs"] = item["ProjectEstSizeMinHrs"];
                pmmNewTicket["AdoptionRiskChoice"] = item["AdoptionRiskChoice"];
                pmmNewTicket["VendorSupportChoice"] = item["VendorSupportChoice"];
                pmmNewTicket["InternalCapabilityChoice"] = item["InternalCapabilityChoice"];
                pmmNewTicket["TechnologySecurityChoice"] = item["TechnologySecurityChoice"];
                pmmNewTicket["TechnologyUsabilityChoice"] = item["TechnologyUsabilityChoice"];
                pmmNewTicket["TechnologyReliabilityChoice"] = item["TechnologyReliabilityChoice"];
                pmmNewTicket["OrganizationalImpactChoice"] = item["OrganizationalImpactChoice"];
                pmmNewTicket["ROI"] = item["ROI"];
                pmmNewTicket["ClassificationSizeChoice"] = item["ClassificationSizeChoice"];
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            // Copy Custom fields
            int numFields = 10;
            for (int i = 1; i <= numFields; i++)
            {
                string textFieldName = string.Format("CustomUGText{0}", i.ToString("D2"));
                string dateFieldName = string.Format("CustomUGDate{0}", i.ToString("D2"));
                string userFieldName = string.Format("CustomUGUser{0}", i.ToString("D2"));
                string multiuserFieldName = string.Format("CustomUGUserMulti{0}", i.ToString("D2"));

                if (UGITUtility.IfColumnExists(textFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, textFieldName))
                    pmmNewTicket[textFieldName] = item[textFieldName];

                if (UGITUtility.IfColumnExists(dateFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, dateFieldName))
                    pmmNewTicket[dateFieldName] = item[dateFieldName];

                if (UGITUtility.IfColumnExists(userFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, userFieldName))
                    pmmNewTicket[userFieldName] = item[userFieldName];

                if (UGITUtility.IfColumnExists(multiuserFieldName, pmmTickets) && UGITUtility.IsSPItemExist(item, multiuserFieldName))
                    pmmNewTicket[multiuserFieldName] = item[multiuserFieldName];
            }

            //Check if StartDate i.e. TicketTargetStartDate is provided 
            bool isValidStartDate = projectRequest.StartDate != null && projectRequest.StartDate.HasValue && projectRequest.StartDate.Value != DateTime.MinValue
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCreationDate, pmmNewTicket.Table);

            if (isValidStartDate)
                pmmNewTicket[DatabaseObjects.Columns.TicketTargetStartDate] = projectRequest.StartDate.Value;
            else
                pmmNewTicket[DatabaseObjects.Columns.TicketTargetStartDate] = DateTime.Now;

            error = pmmTicketRequest.CommitChanges(pmmNewTicket);

            if (!string.IsNullOrEmpty(error))
            {
                ULog.WriteLog(error);
                return error;
            }

            // Set TicketID to ref variable
            ticketId = Convert.ToString(pmmNewTicket[DatabaseObjects.Columns.TicketId]);

            #endregion Set all fields and create PMM ticket

            if (isNPRToPMM)
            {
                // Store PMM ref in NPR so that NPR knowd that PMM is created
                item[DatabaseObjects.Columns.TicketPMMIdLookup] = pmmNewTicket[DatabaseObjects.Columns.ID];

                // Close NPR ticket - consider it as default setting
                nprTicket.Approve(applicationContext.CurrentUser, item, true);
                error = nprTicket.CommitChanges(item);

                if (!string.IsNullOrEmpty(error))
                    ULog.WriteLog(error);
            }

            #region Create default monitors on project
            CreateProjectDefaultMonitors(pmmNewTicket, pmmTicketRequest);

            #endregion Create default monitors on project

            // Copy all Tasks from NPR to PMM project
            if (projectRequest.ImportDataOption != null && (projectRequest.ImportDataOption.Contains("Task") || projectRequest.ImportDataOption.Contains("Complete")))
            {
                UGITTaskManager taskManager = new UGITTaskManager(applicationContext);
                taskManager.ImportAllTasksUnderPMM(projectRequest.Mode, UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketId]), ticketId, UGITUtility.StringToLong(projectRequest.NPRTemplateID));
            }

            //Import module budgets
            if (projectRequest.ImportDataOption != null && (projectRequest.ImportDataOption.Contains("Budget") || projectRequest.ImportDataOption.Contains("Complete")))
            {
                ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(applicationContext);
                moduleBudgetManager.ImportBudgets(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketId]), ticketId);
            }

            return error;
        }

        private static void SetDefaultLifeCycleSettingOnProject(ApplicationContext applicationContext, DataRow pmmNewTicket, string selectedLifeCycleID)
        {
            LifeCycle lifeCycle = null;
            List<LifeCycle> objLifeCycle = new List<LifeCycle>();
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(applicationContext);
            objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);

            // Select the first LifeCycle from selected if exits otherwise load first lifecyle
            long lifecyleid = 0;
            lifecyleid = UGITUtility.StringToInt(selectedLifeCycleID);
            if (lifecyleid > 0)
                lifeCycle = objLifeCycle.FirstOrDefault(x => x.ID == lifecyleid);
            else
                lifeCycle = objLifeCycle.FirstOrDefault();

            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
            {
                ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(applicationContext);

                pmmNewTicket[DatabaseObjects.Columns.ModuleStepLookup] = null;
                pmmNewTicket[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                pmmNewTicket[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                pmmNewTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = $"{DatabaseObjects.Columns.TicketProjectManager};#{DatabaseObjects.Columns.TicketProjectCoordinators};#OwnerUser;#PRPGroupUser;#{objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup)}"; 
                //string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                pmmNewTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup] = lifeCycle.ID;
            }
            else
            {
                pmmNewTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup] = DBNull.Value;
            }

            pmmNewTicket[DatabaseObjects.Columns.ScrumLifeCycle] = false;      //keep false as default setting
        }

        protected void CreateProjectDefaultMonitors(DataRow projectTicket, Ticket pmmTicketRequest)
        {
            ModuleMonitorOptionManager MonitorOptionManagerObj = new ModuleMonitorOptionManager(applicationContext);
            ProjectMonitorStateManager MonitorStateManagerObj = new ProjectMonitorStateManager(applicationContext);

            List<ModuleMonitorOption> pmmMonitorOptions = MonitorOptionManagerObj.Load();
            List<ProjectMonitorState> projectMonitors = MonitorStateManagerObj.Load();

            ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(applicationContext);
            List<ModuleMonitor> lstModuleMonitors = moduleMonitorManager.Load();

            int totalMonitorsSelected = 5;       // we have 5 default monitors & by default all are considered as selected
            double projectScore = 0;
            double riskScore = 0;
            ProjectMonitorStateManager monitorStateManager = null;
            ProjectMonitorState newPMMMonitor = null;

            foreach (ModuleMonitor dModuleMonitor in lstModuleMonitors)
            {
                if (pmmMonitorOptions != null && pmmMonitorOptions.Count > 0)
                {
                    List<ModuleMonitorOption> lstOfModuleMonitor = pmmMonitorOptions.Where(x => x.ModuleMonitorNameLookup == UGITUtility.StringToLong(dModuleMonitor.ID)).ToList();

                    if (lstOfModuleMonitor != null && lstOfModuleMonitor.Count > 0)
                    {
                        ModuleMonitorOption moduleMonitorOption = lstOfModuleMonitor.FirstOrDefault(x => x.IsDefault || x.ModuleMonitorOptionLEDClass.StartsWith("GreenLED"));

                        if (moduleMonitorOption != null)
                        {
                            monitorStateManager = new ProjectMonitorStateManager(applicationContext);
                            newPMMMonitor = new ProjectMonitorState();
                            newPMMMonitor.Title = string.Format("{0} - {1}", Convert.ToString(projectTicket[DatabaseObjects.Columns.TicketId]), dModuleMonitor.MonitorName);
                            newPMMMonitor.PMMIdLookup = Convert.ToInt64(projectTicket[DatabaseObjects.Columns.ID]);
                            newPMMMonitor.TicketId = Convert.ToString(projectTicket[DatabaseObjects.Columns.TicketId]);
                            newPMMMonitor.ModuleMonitorNameLookup = Convert.ToInt64(dModuleMonitor.ID);
                            newPMMMonitor.ModuleNameLookup = uHelper.getModuleNameByTicketId(newPMMMonitor.TicketId);
                            newPMMMonitor.ModuleMonitorOptionName = moduleMonitorOption.ModuleMonitorOptionName;
                            newPMMMonitor.ProjectMonitorWeight = (100 / totalMonitorsSelected);
                            newPMMMonitor.ModuleMonitorOptionLEDClass = moduleMonitorOption.ID;
                            newPMMMonitor.ModuleMonitorOptionLEDName = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                            newPMMMonitor.ModuleMonitorOptionIdLookup = Convert.ToInt64(moduleMonitorOption.ID);
                            projectScore += (100 / totalMonitorsSelected) * (float.Parse(moduleMonitorOption.ModuleMonitorMultiplier.ToString()) / 100);

                            if (dModuleMonitor.MonitorName == "On Budget" || dModuleMonitor.MonitorName == "On Time")
                            {
                                newPMMMonitor.AutoCalculate = true;
                            }

                            string monitorname = Convert.ToString(moduleMonitorOption.ModuleMonitorNameLookup);

                            if (!string.IsNullOrEmpty(monitorname) && monitorname == "Risk Level")
                            {
                                float weightage = 0;
                                float.TryParse(Convert.ToString(newPMMMonitor.ProjectMonitorWeight), out weightage);
                                float multiplier = 0;
                                float.TryParse(Convert.ToString(moduleMonitorOption.ModuleMonitorMultiplier), out multiplier);
                                float score = weightage * (multiplier / 100);
                                //pmmNewTicket[DatabaseObjects.Columns.TicketRiskScore] = Math.Round(100 - score);
                                //saving riskscore as total like project score before its saving last risk on ticket
                                riskScore += Math.Round(100 - score);
                            }

                            monitorStateManager.Insert(newPMMMonitor);
                            //error = pmmTicketRequest.CommitChanges(projectTicket);
                            // this code commented bcz no need to save ticket in loop
                            //if (!string.IsNullOrEmpty(error))
                            //    ULog.WriteLog(error);
                        }
                    }
                }
            }

            // update prject socre and risk score in PMM list
            projectTicket[DatabaseObjects.Columns.TicketProjectScore] = projectScore;
            projectTicket[DatabaseObjects.Columns.TicketRiskScore] = riskScore;
            string error = pmmTicketRequest.CommitChanges(projectTicket);

            if (!string.IsNullOrEmpty(error))
                ULog.WriteLog(error);
        }
    }
}
