using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Helpers;
using MailKit.Security;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using DevExpress.XtraSpreadsheet.Model;
using System.Windows.Forms;

namespace uGovernIT.Manager
{
    public class Ticket
    {
        private long moduleID;
        private string moduleName;

        public UserProfile User { get; private set; }

        public UGITModule Module { get; private set; }
        private ITicketManager ticketManager;
        public bool? isAddREsolutionTime = false;
        public String strResolutionTime = String.Empty;
        TicketActionType ticketActionType;

        ApplicationContext _context;
        ModuleUserStatisticsManager objModuleUserStatisticsManager;
        ConfigurationVariableManager objConfigurationVariableHelper;
        ModuleWorkflowHistoryManager workflowHistoryMgr;
        ModuleViewManager moduleMgr;
        RequestTypeManager requestTypeManager;
        StateManager stateManager;
        private DataTable moduleInstanceList;
        UGITTaskManager taskManager;
        FieldConfigurationManager configurationManager;
        /// <summary>
        /// Keep skip stages list to send notification of skip stages as well.
        /// All the notification goes out when SendEmailToActionUsers function is called
        /// </summary>
        private List<LifeCycleStage> skipStageNotificationStages;
        public string Comment;
        private StatisticsManager _statisticsManager;
        private StatisticsConfigurationManager _statisticsConfigurationManager;

        public Ticket(ApplicationContext context, long thisModuleID)
        {
            _context = context;
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            objModuleUserStatisticsManager = new ModuleUserStatisticsManager(context);
            workflowHistoryMgr = new ModuleWorkflowHistoryManager(context);
            configurationManager = new FieldConfigurationManager(context);
            ticketManager = new TicketManager(context);
            taskManager = new UGITTaskManager(_context);
            moduleMgr = new ModuleViewManager(_context);
            _statisticsManager = new StatisticsManager(_context);
            _statisticsConfigurationManager = new StatisticsConfigurationManager(_context);
            Module = moduleMgr.GetByID(thisModuleID);
            requestTypeManager = new RequestTypeManager(_context);
            ticketActionType = TicketActionType.None;

            if (Module != null)
            {
                moduleName = Module.ModuleName;
                moduleID = thisModuleID;
                moduleInstanceList = ticketManager.GetTableSchemaDetail(Module.ModuleTable,string.Empty);
            }
            if (context != null)
                User = context.CurrentUser;

            skipStageNotificationStages = new List<LifeCycleStage>();
        }

        public Ticket(ApplicationContext context, string modulename)
        {
            _context = context;
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            objModuleUserStatisticsManager = new ModuleUserStatisticsManager(context);
            workflowHistoryMgr = new ModuleWorkflowHistoryManager(context);
            ticketManager = new TicketManager(context);
            taskManager = new UGITTaskManager(_context);
            moduleMgr = new ModuleViewManager(_context);
            _statisticsManager = new StatisticsManager(_context);
            _statisticsConfigurationManager = new StatisticsConfigurationManager(_context);

            Module = moduleMgr.LoadByName(modulename, true);
            if (Module != null)
            {
                moduleID = Module.ID;
                Module.IconUrl = "/Content/Images/" + Module.ModuleName + "_32x32.png";
                moduleName = Module.ModuleName;
                moduleInstanceList = ticketManager.GetTableSchemaDetail(Module.ModuleTable, string.Empty);
            }
            ticketActionType = TicketActionType.None;
            if (context != null)
                User = context.CurrentUser;

            skipStageNotificationStages = new List<LifeCycleStage>();
        }

        public Ticket(ApplicationContext context, string modulename, UserProfile actionUser)
        {
            _context = context;
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            objModuleUserStatisticsManager = new ModuleUserStatisticsManager(context);
            workflowHistoryMgr = new ModuleWorkflowHistoryManager(context);
            moduleMgr = new ModuleViewManager(_context);
            _statisticsManager = new StatisticsManager(_context);
            _statisticsConfigurationManager = new StatisticsConfigurationManager(_context);
            taskManager = new UGITTaskManager(_context);
            _context = context;
            ticketManager = new TicketManager(_context);
            requestTypeManager = new RequestTypeManager(_context);
            Module = moduleMgr.LoadByName(modulename, true);
            if (Module != null)
            {
                Module.IconUrl = "/Content/Images/" + Module.ModuleName + "_32x32.png";
                moduleName = Module.ModuleName;
                moduleID = Module.ID;

                moduleInstanceList = ticketManager.GetTableSchemaDetail(Module.ModuleTable, string.Empty);
            }
            ticketActionType = TicketActionType.None;
            User = actionUser;
            skipStageNotificationStages = new List<LifeCycleStage>();
        }
        public string GetValue(string values)
        {
            string outputValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(values))
            {
                List<string> lookupIds = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                List<string> listLookupValues = new List<string>();
                lookupIds.ForEach(x =>
                {
                    DataRow dataRow = GetTableDataManager.GetTableData(Module.ModuleTable, "ID=" + x).Select().FirstOrDefault();
                    if (dataRow != null)
                    {
                        if (UGITUtility.IsSPItemExist(dataRow, DatabaseObjects.Columns.AssetTagNum))
                        {
                            listLookupValues.Add(Convert.ToString(dataRow[DatabaseObjects.Columns.AssetTagNum]));

                        }
                        else
                        {
                            listLookupValues.Add(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketId]));
                        }
                    }
                });
                if (listLookupValues != null && listLookupValues.Count > 0)
                {
                    outputValue = string.Join("; ", listLookupValues);
                }
            }
            return outputValue;
        }
        public static bool IsActionUser(ApplicationContext context, DataRow currentTicket, UserProfile user)
        {
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes))
            {
                string[] currentStageActionUserTypes = UGITUtility.SplitString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes), Constants.Separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string currentStageActionUserType1 in currentStageActionUserTypes)
                {
                    if (context.UserManager.IsUserPresentInField(user, currentTicket, currentStageActionUserType1, true))
                        return true;
                }
            }

            return false;
        }
        public static bool IsActionUserNew(ApplicationContext context, DataRow currentTicket, UserProfile user)
        {
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes))
            {
                string stageActionUser = UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes));

                string[] currentStageActionUserTypes = UGITUtility.SplitString(stageActionUser, Constants.Separator11, StringSplitOptions.RemoveEmptyEntries);
                if (!string.IsNullOrEmpty(stageActionUser) && currentStageActionUserTypes != null && currentStageActionUserTypes.Length == 1 && stageActionUser.IndexOf(Constants.Separator11.Trim()) != -1)
                    currentStageActionUserTypes = UGITUtility.SplitString(stageActionUser, Constants.Separator11.Trim(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string currentStageActionUserType1 in currentStageActionUserTypes)
                {
                    if (context.UserManager.IsUserPresentInField(user, currentTicket, currentStageActionUserType1.Trim(), true))
                        return true;
                }
            }

            return false;
        }
        public static bool IsDataEditor(DataRow currentTicket, ApplicationContext context)
        {
            return IsDataEditor(currentTicket, context.CurrentUser, context);
        }
        public static bool IsDataEditor(DataRow currentTicket, UserProfile user, ApplicationContext context)
        {
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.DataEditors))
            {
                string[] dataEditors = UGITUtility.SplitString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.DataEditors), Constants.Separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dataEditor in dataEditors)
                {
                    UserProfileManager userManager = new UserProfileManager(context);
                    if (userManager.IsUserPresentInField(user, currentTicket, dataEditor, true))
                        return true;
                }
            }

            return false;
        }

        //Added the viewfield to return only specific columns 
        //Anand
        public static DataRow GetCurrentTicket(ApplicationContext context, string module, string ticketId, List<string> viewFields = null)
        {
            if (string.IsNullOrEmpty(module) || string.IsNullOrEmpty(ticketId))
                return null;
            DataRow currentTicket = null;
            Ticket t = new Ticket(context, module);
            //TicketStore tStore = new TicketStore(context);

            //try is faster then checking count of items
            try
            {
                int tID = 0;
                if (int.TryParse(ticketId, out tID))
                {
                    currentTicket = t.ticketManager.GetByID(t.Module, tID, viewFields);
                }
                else
                {
                    currentTicket = t.ticketManager.GetByTicketID(t.Module, ticketId, viewFields);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, string.Format("Error getting ticket with ID: {0}", ticketId));
            }

            return currentTicket;

        }
        public static DataRow GetCurrentTicket(ApplicationContext context, long moduleid, string ticketId)
        {
            if (moduleid == 0 || string.IsNullOrEmpty(ticketId))
                return null;

            string moduleName = "";
            string listName = string.Empty;

            UGITModule module = null;
            long moduleID = moduleid;

            if (moduleid > 0)
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context);
                module = moduleMgr.GetByID(moduleid);
                if (module != null)
                    moduleName = module.ModuleName;
            }
            return GetCurrentTicket(context, moduleName, ticketId);
        }
        public void UpdateTicketCache(DataRow dr, string moduleName)
        {
            ticketManager.UpdateTicketCache(dr, moduleMgr.GetByName(moduleName), false);
        }

        public LifeCycle GetTicketLifeCycle(DataRow ticketRow)
        {
            //LifeCycleStore lifecyclestore = new LifeCycleStore(_context);
            //ModuleLifeCycleDAL objLifecycle = new ModuleLifeCycleDAL();
            List<LifeCycle> lifeCycleList = this.Module.List_LifeCycles; //lifecyclestore.LoadByModule(moduleName);  // objLifecycle.LoadByModule("TSR").FirstOrDefault(x => x.ID == 0); //  this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);


            LifeCycle lifeCycle = lifeCycleList.First();
            if (this.Module != null)
            {
                if (this.Module.ModuleName == "PMM")
                {
                    if (ticketRow != null && UGITUtility.IsSPItemExist(ticketRow, DatabaseObjects.Columns.ProjectLifeCycleLookup))
                    {
                        long lifecyleID = 0;
                        long.TryParse(Convert.ToString(ticketRow[DatabaseObjects.Columns.ProjectLifeCycleLookup]), out lifecyleID);

                        if (lifecyleID > 0)
                        {
                            lifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == lifecyleID);
                        }
                    }
                }
                else
                {
                    lifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                }
            }

            return lifeCycle;
        }

        public LifeCycle GetTicketLifeCycle()
        {
            LifeCycleStore lifecyclestore = new LifeCycleStore(_context);
            //ModuleLifeCycleDAL objLifecycle = new ModuleLifeCycleDAL();
            LifeCycle lifeCycle = lifecyclestore.LoadByModule(moduleName).FirstOrDefault(x => x.ID == 1 || x.ID == 0);  // objLifecycle.LoadByModule("TSR").FirstOrDefault(x => x.ID == 0); //  this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            //if (this.Module.ModuleName == "PMM")
            //{
            //    if (saveTicket != null && uHelper.IsSPItemExist(saveTicket, DatabaseObjects.Columns.ProjectLifeCycleLookup))
            //    {
            //        SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(saveTicket, DatabaseObjects.Columns.ProjectLifeCycleLookup)));
            //        if (lookup != null && lookup.LookupId > 0)
            //        {
            //            lifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == lookup.LookupId);
            //        }
            //    }
            //}

            return lifeCycle;
        }

        public LifeCycleStage GetTicketCurrentStage()
        {
            LifeCycle lifeCycle = GetTicketLifeCycle();

            if (lifeCycle == null || lifeCycle.Stages.Count <= 0)
            {
                return null;
            }
            return new LifeCycleStage();
            // return GetTicketCurrentStage(lifeCycle);
        }
        public LifeCycleStage GetTicketCurrentStage(DataRow ticketRow)
        {

            LifeCycle lifeCycle = GetTicketLifeCycle(ticketRow);

            if (lifeCycle == null || lifeCycle.Stages.Count <= 0)
            {
                return null;
            }
            return GetTicketCurrentStage(lifeCycle, ticketRow);
        }
        public LifeCycleStage GetTicketCurrentStage(LifeCycle ticketLifeCycle, DataRow item)
        {
            if (ticketLifeCycle == null || ticketLifeCycle.Stages.Count <= 0)
            {
                return null;
            }

            LifeCycleStage currentStage = null;

            //Fetch current stage detail based on stage step
            if (item.Table.Columns.Contains(DatabaseObjects.Columns.StageStep))
            {
                int step = 0;
                if (int.TryParse(Convert.ToString(item[DatabaseObjects.Columns.StageStep]), out step))
                {
                    currentStage = ticketLifeCycle.Stages.FirstOrDefault(x => x.StageStep == step);
                }
            }
            else if (item.Table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
            {
                //Fetch current stage detail based on modulesteplookup
                string stageLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ModuleStepLookup));
                int sstep = 0;
                if (int.TryParse(stageLookup, out sstep))
                {
                    currentStage = ticketLifeCycle.Stages.FirstOrDefault(x => x.ID == sstep);
                }
            }

            // Safety net to prevent crash
            if (currentStage == null)
                currentStage = ticketLifeCycle.Stages[0];

            return currentStage;
        }

        public List<TicketTemplate> GetTicketTemplate()
        {
            StoreBase<TicketTemplate> tickettemplates = new StoreBase<TicketTemplate>(_context, DatabaseObjects.Tables.TicketTemplates);
            return tickettemplates.Load(string.Empty);

        }


        public bool Validate(List<TicketColumnValue> values, DataRow item, List<TicketColumnError> messages, bool skipMandatoryCheck, bool adminOverride, int requestSource, bool ignoreConstraintValidation = false)
        {
            FieldConfigurationManager fieldconfigMGR = new FieldConfigurationManager(_context);
            FieldConfiguration field = null;
            ModuleRoleWriteAccess roleWriteItem = null;
            ModuleFormLayout moduleFormLayout = null;

            bool noError = true;
            ArrayList mandatoryDependentFieldsSkipped = new ArrayList();
            #region Mandatory Field Validation
            LifeCycleStage currentStage = GetTicketCurrentStage(item);
            bool ticketIsOnhold = false;
            //if ticket is On-hold then don't do any validate checks
            if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketOnHold))
            { ticketIsOnhold = UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketOnHold]); }

            if (!adminOverride && ticketIsOnhold)
                return true;

            //Title is absolutely mandatory field.
            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.Title))
            {
                TicketColumnValue cValue = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.Title);
                if (string.IsNullOrEmpty(Convert.ToString(cValue.Value)) && Convert.ToString(cValue.Value).Trim() == string.Empty)
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddMandatoryError(cValue.InternalFieldName, cValue.DisplayName, string.Empty));
                }

                if (!string.IsNullOrEmpty(Convert.ToString(cValue.Value)) && Module.ModuleName == ModuleNames.COM)
                {
                    long Id = 0;
                    long.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ID]), out Id);
                    DataTable dt = GetTableDataManager.GetTableData(Module.ModuleTable, $"TRIM({DatabaseObjects.Columns.Title}) = '{Convert.ToString(cValue.Value).Replace("'", "''")}' and {DatabaseObjects.Columns.ID} <> {Id} and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'", DatabaseObjects.Columns.Title, "");
                    if (dt.Rows.Count >= 1)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(cValue.InternalFieldName, cValue.DisplayName, "Company with same name already exists"));
                    }
                }
            }

            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.AssetTagNum) && _context.ConfigManager.GetValue(ConfigConstants.AssetUniqueTag) == DatabaseObjects.Columns.AssetTagNum)
            {
                TicketColumnValue cValue = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.AssetTagNum);
                if (!string.IsNullOrEmpty(Convert.ToString(cValue.Value)))
                {
                    long Id = 0;
                    long.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ID]), out Id);
                    DataTable dt = GetTableDataManager.GetTableData(Module.ModuleTable, $"{DatabaseObjects.Columns.AssetTagNum} = '{cValue.Value}' and LEN(TRIM({DatabaseObjects.Columns.AssetTagNum})) > 0 and {DatabaseObjects.Columns.ID} <> {Id} and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'", DatabaseObjects.Columns.AssetTagNum, "");
                    if (dt.Rows.Count >= 1)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(cValue.InternalFieldName, cValue.DisplayName, "Asset with same tag already exists"));
                    }
                }
            }

            if ((moduleName == ModuleNames.CON) && (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.FirstName) || values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.LastName)))
            {
                TicketColumnValue cValueFirstName = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.FirstName);
                TicketColumnValue cValueLastName = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.LastName);
                TicketColumnValue cValueCompanyLookup = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.CRMCompanyLookup);

                if (!string.IsNullOrEmpty(Convert.ToString(cValueFirstName.Value)) || !string.IsNullOrEmpty(Convert.ToString(cValueLastName.Value)))
                {
                    long Id = 0;
                    long.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ID]), out Id);
                    DataTable dt = GetTableDataManager.GetTableData(Module.ModuleTable, $"TRIM({DatabaseObjects.Columns.FirstName}) = '{cValueFirstName.Value}' and TRIM({DatabaseObjects.Columns.LastName}) = '{cValueLastName.Value}' and {DatabaseObjects.Columns.CRMCompanyLookup} = '{cValueCompanyLookup.Value}' and {DatabaseObjects.Columns.ID} <> {Id} and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'", DatabaseObjects.Columns.FirstName, "");
                    if (dt.Rows.Count >= 1)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(cValueFirstName.InternalFieldName, cValueFirstName.DisplayName, "Contact (FirstName LastName) with in the Company already exists"));
                    }
                }
            }

            bool fieldLevelMandate = false;
            foreach (TicketColumnValue cValue in values)
            {
                fieldLevelMandate = false;

                if (cValue.InternalFieldName.ToLower() == "#control#")
                {
                    roleWriteItem = Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName.ToLower() == cValue.DisplayName.ToLower());
                    moduleFormLayout = Module.List_FormLayout.FirstOrDefault(x => x.TabId == cValue.TabNumber && x.FieldName == cValue.InternalFieldName);
                    if (moduleFormLayout != null && !string.IsNullOrEmpty(moduleFormLayout.SkipOnCondition))
                        fieldLevelMandate = FormulaBuilder.EvaluateFormulaExpression(_context, moduleFormLayout.SkipOnCondition, values, item);
                    if (!fieldLevelMandate && !skipMandatoryCheck && roleWriteItem != null && roleWriteItem.FieldMandatory && !UGITUtility.StringToBoolean(cValue.Value))
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddMandatoryError(cValue.InternalFieldName, cValue.ErrorDisplayName, string.Empty));
                    }
                }
                else
                {
                    DataTable parentTable = item.Table;
                    DataColumn f = parentTable.Columns[cValue.InternalFieldName];
                    //SPField f = SPListHelper.GetSPField(item.ParentList, cValue.InternalFieldName);
                    if (f == null)
                        continue;
                    if (string.IsNullOrEmpty(Convert.ToString(currentStage.StageStep)))
                    {
                        currentStage.StageStep = 0;
                    }
                    roleWriteItem = Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName.ToLower() == f.ColumnName.ToLower());
                    moduleFormLayout = Module.List_FormLayout.FirstOrDefault(x => x.TabId == cValue.TabNumber && x.FieldName == cValue.InternalFieldName);
                    if (moduleFormLayout != null && !string.IsNullOrEmpty(moduleFormLayout.SkipOnCondition))
                        fieldLevelMandate = FormulaBuilder.EvaluateFormulaExpression(_context, moduleFormLayout.SkipOnCondition, values, item);

                    if (cValue.InternalFieldName != DatabaseObjects.Columns.Title && roleWriteItem != null &&
                        roleWriteItem.FieldMandatory && string.IsNullOrEmpty(Convert.ToString(cValue.Value)))
                    {
                        bool valueExistInV = false;
                        if (f.ColumnName == DatabaseObjects.Columns.TicketResolutionComments && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.TicketResolutionComments])))
                            valueExistInV = true;

                        if (roleWriteItem.StageStep == 0 || (!fieldLevelMandate && !skipMandatoryCheck && !valueExistInV))
                        {
                            if (UGITUtility.IsDependentMandatoryField(moduleName, f.ColumnName))
                            {
                                if ((IsQuickClose(item, values) || currentStage.StageStep > 1) && !CheckQuickCloseMandatoryField(values, f.ColumnName))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                                }

                                if (IsAquisition(item, values) && !CheckHardwareSoftwareMandatoryField(values, f.ColumnName))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                                }

                                if (!IsNoTest(item, values) && !CheckNoTestField(values, f.ColumnName))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                                }

                                if (IsReviewRequired(item, values) && !CheckReviewMandatoryField(values, f.ColumnName)) // Used for CMT
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                                }

                                if (moduleName == "VCC" && !CheckMSAMandatoryFieldForVCC(values, f.ColumnName))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                                }
                            }
                            else
                            {
                                noError = false;
                                messages.Add(TicketColumnError.AddMandatoryError(f.ColumnName, cValue.DisplayName, string.Empty));
                            }
                        }
                    }
                }
            }

            #endregion

            foreach (TicketColumnValue cValue in values)
            {
                moduleFormLayout = Module.List_FormLayout.FirstOrDefault(x => x.TabId == cValue.TabNumber && x.FieldName == cValue.InternalFieldName);
                if (moduleFormLayout != null && !string.IsNullOrEmpty(moduleFormLayout.SkipOnCondition))
                    fieldLevelMandate = FormulaBuilder.EvaluateFormulaExpression(_context, moduleFormLayout.SkipOnCondition, values, item);

                DataTable fTable = item.Table;
                DataColumn f = fTable.Columns[cValue.InternalFieldName];
                //SPField f = SPListHelper.GetSPField(item.ParentList, cValue.InternalFieldName);
                if (f == null)
                    continue;

                roleWriteItem = Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName.ToLower() == f.ColumnName.ToLower());

                #region Field Specific Validation
                switch (cValue.InternalFieldName)
                {
                    case "DetectionDate":
                    case "OccurrenceDate":
                        // NOTE: These two fields can be set in the future by ModuleTasks code for ACR, etc.
                        //case  "ActualStartDate":
                        //case  "ActualCompletionDate":
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(cValue.Value)))
                            {
                                DateTime incidentDate = DateTime.MinValue;
                                DateTime.TryParse(Convert.ToString(cValue.Value), out incidentDate);
                                if (incidentDate == DateTime.MinValue || incidentDate.Date > DateTime.Today.Date)
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, string.Format("{0} cannot be in the future or blank.", cValue.DisplayName)));
                                }
                            }
                        }
                        break;
                    case "DesiredCompletionDate":
                        {
                            if (_context.ConfigManager.GetValueAsBool(Constants.DesiredCompletionDateValidation) &&
                                !string.IsNullOrEmpty(Convert.ToString(cValue.Value)) && string.IsNullOrEmpty(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketDesiredCompletionDate])))
                            {
                                DateTime cplnDate = DateTime.MinValue;
                                DateTime.TryParse(Convert.ToString(cValue.Value), out cplnDate);
                                if (cplnDate == DateTime.MinValue || cplnDate.Date < DateTime.Today.Date)
                                {

                                    noError = false;
                                    messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, string.Format("{0} cannot be in the past.", cValue.DisplayName)));
                                }
                            }

                            if (string.IsNullOrEmpty(Convert.ToString(cValue.Value)) && adminOverride)
                            {
                                messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, string.Format("{0} cannot be blank.", cValue.DisplayName)));
                            }
                        }
                        break;
                    case DatabaseObjects.Columns.StudioLookup:
                        {
                            if (_context.ConfigManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy) &&
                                !string.IsNullOrEmpty(Convert.ToString(cValue.Value)))
                            {
                                StudioManager objStudioManager = new StudioManager(_context);
                                long divisionID = objStudioManager.GetDivisionIdForStudio(UGITUtility.StringToLong(cValue.Value));
                                TicketColumnValue divisionColumnValue = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.DivisionLookup);
                                if (divisionColumnValue != null && divisionID != 0 && divisionID != UGITUtility.StringToLong(divisionColumnValue.Value))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, string.Format("{0} does not belong to the {1} selected for this item.", cValue.DisplayName, divisionColumnValue.DisplayName)));
                                }
                            }
                        }
                        break;
                    case DatabaseObjects.Columns.RetainageChoice:
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(cValue.Value)) && UGITUtility.ObjectToString(cValue.Value) == "Other")
                            {
                                TicketColumnValue retainageData = values.Where(x => x.InternalFieldName == DatabaseObjects.Columns.Retainage)?.FirstOrDefault();
                                if (retainageData != null && string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(retainageData.Value)))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddMandatoryError(DatabaseObjects.Columns.Retainage, retainageData.DisplayName, string.Empty));
                                }
                            }
                        }
                        break;
                }

                #endregion
                #region Type Specific Validation

                field = fieldconfigMGR.GetFieldByFieldName(f.ColumnName);
                string fieldDataType = string.Empty;
                if (field != null)
                    fieldDataType = field.Datatype;
                else
                    fieldDataType = f.DataType.ToString();

                switch (fieldDataType)
                {
                    case DatabaseObjects.DataTypes.UserField:
                        {
                            bool validUser = CheckUserOrGroupIsITManagerType(field, Convert.ToString(cValue.Value), moduleName);
                            if (!validUser)
                            {
                                noError = false;
                                messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, "Selected user is not a Manager, IT Person or Group Member."));
                            }
                        }
                        break;
                    case DatabaseObjects.DataTypes.UserType:
                        {
                            bool validUser = CheckUserOrGroupIsITManagerType(field, Convert.ToString(cValue.Value), moduleName);
                            if (!validUser)
                            {
                                noError = false;
                                messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, "Selected user is not a Manager, IT Person or Group Member."));
                            }
                        }
                        break;
                    case DatabaseObjects.DataTypes.Number:
                        {
                            double number = 0;
                            if (double.TryParse(Convert.ToString(cValue.Value), out number))
                            {
                                if (number < 0)
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, "Cannot enter negative values."));
                                }
                                else if (f.ColumnName == DatabaseObjects.Columns.TicketPctComplete && (number < 0 || number > 100))
                                {
                                    noError = false;
                                    messages.Add(TicketColumnError.AddError(f.ColumnName, cValue.DisplayName, "Value should be between 0 - 100."));
                                }
                            }
                        }
                        break;
                }
                #endregion
            }

            if (moduleName == "SVC" && Convert.ToInt64(item["ID"]) > 0)
            {
                // If CheckAssigneeToAllTask is set, don't allow approve unless all tasks are either complete or assigned
                bool checkTaskAssignee = currentStage.Prop_CheckAssigneeToAllTask.HasValue ? currentStage.Prop_CheckAssigneeToAllTask.Value : false;
                bool isAllTaskCompleted = UGITUtility.StringToBoolean(DatabaseObjects.Columns.IsAllTaskComplete);
                if (checkTaskAssignee && !isAllTaskCompleted && !taskManager.CheckOwnerIsAssignedToAll(Convert.ToString(item[DatabaseObjects.Columns.TicketId])))
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError("One or more tasks are not assigned, please assign these first!"));
                }
            }

            #region Dependent Validations

            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.DetectionDate) && values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.OccurrenceDate))
            {
                DateTime incidentDetectionDate = DateTime.MinValue;
                TicketColumnValue incidentDetectionDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.DetectionDate);
                if (incidentDetectionDateVal != null)
                    DateTime.TryParse(Convert.ToString(incidentDetectionDateVal.Value), out incidentDetectionDate);

                DateTime incidentOccurenceDate = DateTime.MinValue;
                TicketColumnValue incidentOccurenceDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.OccurrenceDate);
                if (incidentOccurenceDateVal != null)
                    DateTime.TryParse(Convert.ToString(incidentOccurenceDateVal.Value), out incidentOccurenceDate);


                if (incidentOccurenceDate != DateTime.MinValue && incidentDetectionDate != DateTime.MinValue &&
                    incidentOccurenceDate.Date <= DateTime.Now.Date && incidentOccurenceDate.Date <= DateTime.Now.Date &&
                    incidentDetectionDate.Date < incidentOccurenceDate.Date)
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", incidentDetectionDateVal.InternalFieldName, incidentDetectionDateVal.DisplayName),
                        string.Format("{0};#{1}", incidentOccurenceDateVal.InternalFieldName, incidentOccurenceDateVal.DisplayName), "Detection Date cannot be before Occurence Date"));
                }
            }

            if (values.Exists(x => x.InternalFieldName == "TargetStartDate") && values.Exists(x => x.InternalFieldName == "TargetCompletionDate"))
            {
                DateTime targetStartDate = DateTime.MinValue;
                TicketColumnValue targetStartDateVal = values.FirstOrDefault(x => x.InternalFieldName == "TargetStartDate");
                if (targetStartDateVal != null)
                    DateTime.TryParse(Convert.ToString(targetStartDateVal.Value), out targetStartDate);

                DateTime targetCplnDate = DateTime.MinValue;
                TicketColumnValue targetCplnDateVal = values.FirstOrDefault(x => x.InternalFieldName == "TargetCompletionDate");
                if (targetCplnDateVal != null)
                    DateTime.TryParse(Convert.ToString(targetCplnDateVal.Value), out targetCplnDate);


                if (targetStartDate != DateTime.MinValue && targetCplnDate != DateTime.MinValue && targetCplnDate.Date < targetStartDate.Date)
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", targetStartDateVal.InternalFieldName, targetStartDateVal.DisplayName),
                      string.Format("{0};#{1}", targetCplnDateVal.InternalFieldName, targetCplnDateVal.DisplayName), "Target Completion Date cannot be before Start Date"));
                }
            }

            if (values.Exists(x => x.InternalFieldName == "ScheduledStartDateTime") && values.Exists(x => x.InternalFieldName == "ScheduledEndDateTime"))
            {
                DateTime startDate = DateTime.MinValue;
                TicketColumnValue startDateVal = values.FirstOrDefault(x => x.InternalFieldName == "ScheduledStartDateTime");
                if (startDateVal != null)
                    DateTime.TryParse(Convert.ToString(startDateVal.Value), out startDate);

                DateTime endDate = DateTime.MinValue;
                TicketColumnValue endDateVal = values.FirstOrDefault(x => x.InternalFieldName == "ScheduledEndDateTime");
                if (endDateVal != null)
                    DateTime.TryParse(Convert.ToString(endDateVal.Value), out endDate);


                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && endDate.Date < startDate.Date)
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", startDateVal.InternalFieldName, startDateVal.DisplayName),
                      string.Format("{0};#{1}", endDateVal.InternalFieldName, endDateVal.DisplayName), "Scheduled Completion Date cannot be before Start Date"));

                }
            }

            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.PreconStartDate) && values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.PreconEndDate))
            {
                DateTime preconStartDate = DateTime.MinValue;
                TicketColumnValue preconStartDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.PreconStartDate);
                if (preconStartDate != null)
                    DateTime.TryParse(Convert.ToString(preconStartDateVal.Value), out preconStartDate);

                DateTime preconEndDate = DateTime.MinValue;
                TicketColumnValue preconEndDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.PreconEndDate);
                //[+][31-10-2023][SANKET][Added validation condition if end date exists and start date is blank then it's required]
                if ((UGITUtility.ObjectToString(preconEndDateVal.Value) != null && UGITUtility.ObjectToString(preconEndDateVal.Value) != "") && (UGITUtility.ObjectToString(preconStartDateVal.Value) == null || UGITUtility.ObjectToString(preconStartDateVal.Value) == ""))
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(preconStartDateVal.InternalFieldName, preconStartDateVal.DisplayName, "Entry of Precon Start Date is required."));
                }

                if (preconEndDateVal != null)
                    DateTime.TryParse(Convert.ToString(preconEndDateVal.Value), out preconEndDate);


                if (preconEndDate != DateTime.MinValue && preconStartDate != DateTime.MinValue &&
                    preconStartDate.Date > preconEndDate.Date)
                {
                    noError = false;
                    //[+][31-10-2023][SANKET][Update error message]
                    //messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", preconEndDateVal.InternalFieldName, preconEndDateVal.DisplayName),
                    //    string.Format("{0};#{1}", preconEndDateVal.InternalFieldName, preconEndDateVal.DisplayName), "Precon End Date cannot be before Precon Start Date"));
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", preconEndDateVal.InternalFieldName, preconEndDateVal.DisplayName),
                        string.Format("{0};#{1}", preconEndDateVal.InternalFieldName, preconEndDateVal.DisplayName), "Precon End Date should be after the Precon Start Date."));
                }
            }

            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionStart) && values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd))
            {
                DateTime estimatedConstructionStart = DateTime.MinValue;
                TicketColumnValue estimatedConstructionStartDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionStart);
                if (estimatedConstructionStart != null)
                    DateTime.TryParse(Convert.ToString(estimatedConstructionStartDateVal.Value), out estimatedConstructionStart);

                DateTime estimatedConstructionEnd = DateTime.MinValue;
                TicketColumnValue estimatedConstructionEndDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd);
                //[+][31-10-2023][SANKET][Added validation condition if end date exists and start date is blank then it's required]
                if ((UGITUtility.ObjectToString(estimatedConstructionEndDateVal.Value) != null && UGITUtility.ObjectToString(estimatedConstructionEndDateVal.Value) != "") && (UGITUtility.ObjectToString(estimatedConstructionStartDateVal.Value) == null || UGITUtility.ObjectToString(estimatedConstructionStartDateVal.Value) == ""))
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(estimatedConstructionStartDateVal.InternalFieldName, estimatedConstructionStartDateVal.DisplayName, "Entry of Construction Start Date is required."));
                }
                //[+][31-10-2023][SANKET][Added validation condition if start date exists and end date is blank then it's required]
                if ((UGITUtility.ObjectToString(estimatedConstructionStartDateVal.Value) != null && UGITUtility.ObjectToString(estimatedConstructionStartDateVal.Value) != "") && (UGITUtility.ObjectToString(estimatedConstructionEndDateVal.Value) == null || UGITUtility.ObjectToString(estimatedConstructionEndDateVal.Value) == ""))
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(estimatedConstructionStartDateVal.InternalFieldName, estimatedConstructionStartDateVal.DisplayName, "Entry of Construction End Date is required."));
                }
                if (estimatedConstructionEndDateVal != null)
                    DateTime.TryParse(Convert.ToString(estimatedConstructionEndDateVal.Value), out estimatedConstructionEnd);


                if (estimatedConstructionEnd != DateTime.MinValue && estimatedConstructionStart != DateTime.MinValue &&
                    estimatedConstructionStart.Date > estimatedConstructionEnd.Date)
                {
                    noError = false;
                    //[+][31-10-2023][SANKET][Update error message]
                    //messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", estimatedConstructionEndDateVal.InternalFieldName, estimatedConstructionEndDateVal.DisplayName),
                    //    string.Format("{0};#{1}", estimatedConstructionEndDateVal.InternalFieldName, estimatedConstructionEndDateVal.DisplayName), "Construction End Date cannot be before Construction Start Date"));
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", estimatedConstructionEndDateVal.InternalFieldName, estimatedConstructionEndDateVal.DisplayName),
                        string.Format("{0};#{1}", estimatedConstructionEndDateVal.InternalFieldName, estimatedConstructionEndDateVal.DisplayName), "Construction End Date should be after the Construction Start Date."));
                }
            }

            if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.DetectionDate) && values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.OccurrenceDate))
            {
                DateTime incidentDetectionDate = DateTime.MinValue;
                TicketColumnValue incidentDetectionDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.DetectionDate);
                if (incidentDetectionDateVal != null)
                    DateTime.TryParse(Convert.ToString(incidentDetectionDateVal.Value), out incidentDetectionDate);

                DateTime incidentOccurenceDate = DateTime.MinValue;
                TicketColumnValue incidentOccurenceDateVal = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.OccurrenceDate);
                if (incidentOccurenceDateVal != null)
                    DateTime.TryParse(Convert.ToString(incidentOccurenceDateVal.Value), out incidentOccurenceDate);


                if (incidentOccurenceDate != DateTime.MinValue && incidentDetectionDate != DateTime.MinValue &&
                    incidentOccurenceDate.Date <= DateTime.Now.Date && incidentOccurenceDate.Date <= DateTime.Now.Date &&
                    incidentDetectionDate.Date < incidentOccurenceDate.Date)
                {
                    noError = false;
                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", incidentDetectionDateVal.InternalFieldName, incidentDetectionDateVal.DisplayName),
                        string.Format("{0};#{1}", incidentOccurenceDateVal.InternalFieldName, incidentOccurenceDateVal.DisplayName), "Detection Date cannot be before Occurence Date"));
                }
            }

            if (values.Exists(x => x.InternalFieldName == "ActualStartDate") && values.Exists(x => x.InternalFieldName == "ActualCompletionDate"))
            {
                DateTime startDate = DateTime.MinValue;
                TicketColumnValue startDateVal = values.FirstOrDefault(x => x.InternalFieldName == "ActualStartDate");
                if (startDateVal != null)
                    DateTime.TryParse(Convert.ToString(startDateVal.Value), out startDate);

                DateTime endDate = DateTime.MinValue;
                TicketColumnValue endDateVal = values.FirstOrDefault(x => x.InternalFieldName == "ActualCompletionDate");
                if (endDateVal != null)
                    DateTime.TryParse(Convert.ToString(endDateVal.Value), out endDate);


                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue &&
                    startDate.Date <= DateTime.Now.Date && endDate.Date <= DateTime.Now.Date &&
                    endDate.Date < startDate.Date)
                {
                    noError = false;

                    messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", startDateVal.InternalFieldName, startDateVal.DisplayName),
                    string.Format("{0};#{1}", endDateVal.InternalFieldName, endDateVal.DisplayName), "Actual Completion Date cannot be before Actual Start Date"));

                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.ProjectID))
                {
                    TicketColumnValue projectId = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.ProjectID);

                    if (!string.IsNullOrEmpty(Convert.ToString(projectId.Value)) && Convert.ToString(projectId.Value) != "-")
                    {
                        DataTable dt = GetTableDataManager.GetTableData(Module.ModuleTable, $"{DatabaseObjects.Columns.ProjectID} = '{projectId.Value}' and LEN(TRIM({DatabaseObjects.Columns.ProjectID})) > 0 and {DatabaseObjects.Columns.ID} <> {item[DatabaseObjects.Columns.ID]} and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'", DatabaseObjects.Columns.ProjectID, "");

                        if (dt.Rows.Count >= 1)
                        {
                            noError = false;
                            messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", projectId.InternalFieldName, projectId.DisplayName), string.Format("{0};#{1}", projectId.InternalFieldName, projectId.DisplayName), $" {projectId.DisplayName} {projectId.Value}  already exists"));
                        }

                        if (!Regex.IsMatch(Convert.ToString(projectId.Value), GetProjectIdExpression()))
                        {
                            noError = false;
                            string ProjectIDFormat = objConfigurationVariableHelper.GetValue("ProjectIDFormat");
                            messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", projectId.InternalFieldName, projectId.DisplayName), string.Format("{0};#{1}", projectId.InternalFieldName, projectId.DisplayName), $" Invalid {projectId.DisplayName} format (eg. {ProjectIDFormat})"));
                        }
                    }

                    if (Convert.ToString(projectId.Value) == "-")
                    {
                        projectId.Value = null;
                    }
                }


                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.Telephone))
                {
                    TicketColumnValue telephone = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.Telephone);
                    if (!Regex.IsMatch(Convert.ToString(telephone.Value), UGITUtility.GetPhoneRegExpression()) && Convert.ToString(telephone.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), $" Invalid {telephone.DisplayName} format"));
                    }
                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.Mobile))
                {
                    TicketColumnValue telephone = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.Mobile);
                    if (!Regex.IsMatch(Convert.ToString(telephone.Value), UGITUtility.GetPhoneRegExpression()) && Convert.ToString(telephone.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), $" Invalid {telephone.DisplayName} format"));
                    }
                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.MobilePhone))
                {
                    TicketColumnValue telephone = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.MobilePhone);
                    if (!Regex.IsMatch(Convert.ToString(telephone.Value), UGITUtility.GetPhoneRegExpression()) && Convert.ToString(telephone.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), string.Format("{0};#{1}", telephone.InternalFieldName, telephone.DisplayName), $" Invalid {telephone.DisplayName} format"));
                    }
                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.Fax))
                {
                    TicketColumnValue fax = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.Fax);
                    if (!Regex.IsMatch(Convert.ToString(fax.Value), UGITUtility.GetPhoneRegExpression()) && Convert.ToString(fax.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", fax.InternalFieldName, fax.DisplayName), string.Format("{0};#{1}", fax.InternalFieldName, fax.DisplayName), $" Invalid {fax.DisplayName} format"));
                    }
                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.EmailAddress))
                {
                    TicketColumnValue email = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.EmailAddress);
                    if (!Regex.IsMatch(Convert.ToString(email.Value), UGITUtility.GetEmailRegExpression()) && Convert.ToString(email.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", email.InternalFieldName, email.DisplayName), string.Format("{0};#{1}", email.InternalFieldName, email.DisplayName), $" Invalid {email.DisplayName} format"));
                    }
                }

                if (values.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.SecondaryEmail))
                {
                    TicketColumnValue email = values.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.SecondaryEmail);
                    if (!Regex.IsMatch(Convert.ToString(email.Value), UGITUtility.GetEmailRegExpression()) && Convert.ToString(email.Value) != string.Empty)
                    {
                        noError = false;
                        messages.Add(TicketColumnError.AddError(string.Format("{0};#{1}", email.InternalFieldName, email.DisplayName), string.Format("{0};#{1}", email.InternalFieldName, email.DisplayName), $" Invalid {email.DisplayName} format"));
                    }
                }

                #endregion

            }

            #region constraints Validation

            if (skipMandatoryCheck == false && !ignoreConstraintValidation)
            {
                string message = string.Empty;
                //var isConstraintValid = UGITModuleConstraint.GetPendingConstraintsStatus(Convert.ToString(DatabaseObjects.Columns.TicketId), currentStage.StageStep, ref message, _context);
                var isConstraintValid = UGITModuleConstraint.GetPendingConstraintsStatus(Convert.ToString(item["TicketId"]), currentStage.StageStep, ref message, _context);
                if (!isConstraintValid)
                {
                    noError = false;
                    string msg = string.Empty;
                    if (message.LastIndexOf(',') > 0)
                        msg = string.Format("\n These Tasks are not complete : {0}", message);
                    else
                        msg = string.Format("\n This Task is not complete : {0}", message);
                    messages.Add(TicketColumnError.AddError(string.Empty, string.Empty, msg));
                }
            }

            #endregion
            return noError;
        }

        /// <summary>
        /// Save change into listitem
        /// </summary>
        /// <param name="item"></param>
        /// <param name="forceUpdate"></param>
        /// <param name="visitedUrl">used to put entry in audit trail only</param>
        /// <param name="stopTicketUpdate">It stops function calling which are updating same ticket in background thread. 
        /// like updating escalations. You need to set this when your are calling CommitChange function manay time in same request. 
        /// so set this property true in all previous calls.</param>
        /// <returns></returns>
        public string CommitChanges(DataRow item, string senderID = "", Uri visitedUrl = null, bool forceUpdate = false, bool donotUpdateEscalations = false, bool stopUpdateDependencies = false, UserProfile actionUser = null, AgentSummary agentSummary = null)
        {
            bool newTicket = false;
            long id = 0;
            if (item == null || UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]) == 0)
                newTicket = true;
            string error = string.Empty;
            try
            {

                #region Priority notification
                PriorityNotification priorityNotification = new PriorityNotification();
                //Get stage of ticket before save to check stage transition
                List<LifeCycleStage> lifeCycleStages = GetActiveLifeCycleStages(item);
                LifeCycleStage currentLifeCycleStage = GetTicketCurrentStage(item);

                LifeCycleStage stageBeforeSave = null;
                int oldStageStep = 0;
                string oldassetLookup = string.Empty;

                string ticketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                if (Module == null)
                    Module = moduleMgr.LoadByName(uHelper.getModuleNameByTicketId(ticketId));
                string oldTicketPRPId = string.Empty;
                string oldAssetOwner = string.Empty;
                string oldAssetDescription = string.Empty;
                string oldAssetSNo1 = string.Empty;
                string oldAssetSNo2 = string.Empty;
                string oldAssetSNo3 = string.Empty;
                string oldAssetManufacturer = string.Empty;
                string oldAssetModel = string.Empty;
                DataRow oldItem = null;
                if (!newTicket)
                {
                    id = UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]);
                    if (lifeCycleStages != null && lifeCycleStages.Count > 0)
                    {
                        string viewFields = string.Format("<FieldRef Name='{0}' Nullable='True'/><FieldRef Name='{1}' Nullable='True'/><FieldRef Name='{2}' Nullable='True'/><FieldRef Name='{3}' Nullable='True'/><FieldRef Name='{4}' Nullable='True'/><FieldRef Name='{5}' Nullable='True'/><FieldRef Name='{6}' Nullable='True'/><FieldRef Name='{7}' Nullable='True'/><FieldRef Name='{8}' Nullable='True'/><FieldRef Name='{9}' Nullable='True'/><FieldRef Name='{10}' Nullable='True'/><FieldRef Name='{11}' Nullable='True'/><FieldRef Name='{12}' Nullable='True'/>",
                                                           DatabaseObjects.Columns.Id, DatabaseObjects.Columns.StageStep, DatabaseObjects.Columns.AssetLookup, DatabaseObjects.Columns.TicketPRP, DatabaseObjects.Columns.AssetOwner, DatabaseObjects.Columns.TicketPriorityLookup, DatabaseObjects.Columns.TicketStatus, DatabaseObjects.Columns.AssetDescription, DatabaseObjects.Columns.SerialNum1, DatabaseObjects.Columns.SerialNum2, DatabaseObjects.Columns.SerialNum3, DatabaseObjects.Columns.Manufacturer, DatabaseObjects.Columns.AssetModelLookup);

                        oldItem = ticketManager.GetByID(Module, id);
                        oldStageStep = UGITUtility.StringToInt(oldItem[DatabaseObjects.Columns.StageStep]);
                        if (oldItem != null)
                        {
                            stageBeforeSave = lifeCycleStages.FirstOrDefault(x => x.StageStep == oldStageStep);
                            if (UGITUtility.IfColumnExists(oldItem, DatabaseObjects.Columns.AssetLookup))
                                oldassetLookup = Convert.ToString(oldItem[DatabaseObjects.Columns.AssetLookup]);

                            if (UGITUtility.IfColumnExists(oldItem, DatabaseObjects.Columns.TicketPRP) && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketAssignedBy))
                            {
                                string oldTicketPRP = Convert.ToString(oldItem[DatabaseObjects.Columns.TicketPRP]);
                                if (!string.IsNullOrWhiteSpace(oldTicketPRP))
                                    oldTicketPRPId = oldTicketPRP;
                            }

                            if (moduleName == ModuleNames.CMDB)
                            {
                                oldAssetOwner = Convert.ToString(oldItem[DatabaseObjects.Columns.AssetOwner]);
                                oldAssetDescription = Convert.ToString(oldItem[DatabaseObjects.Columns.AssetDescription]);
                                oldAssetSNo1 = Convert.ToString(oldItem[DatabaseObjects.Columns.SerialNum1]);
                                oldAssetSNo2 = Convert.ToString(oldItem[DatabaseObjects.Columns.SerialNum2]);
                                oldAssetSNo3 = Convert.ToString(oldItem[DatabaseObjects.Columns.SerialNum3]);
                                oldAssetManufacturer = Convert.ToString(oldItem[DatabaseObjects.Columns.Manufacturer]);
                                oldAssetModel = Convert.ToString(oldItem[DatabaseObjects.Columns.AssetModelLookup]);
                            }

                            if (UGITUtility.IfColumnExists(oldItem, DatabaseObjects.Columns.TicketPriorityLookup))
                            {
                                long pLookup = UGITUtility.StringToLong(oldItem[DatabaseObjects.Columns.TicketPriorityLookup]);
                                if (pLookup > 0)
                                {
                                    PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(_context);
                                    ModulePrioirty prioirty = prioirtyViewManager.LoadByID(pLookup);
                                    if (prioirty != null)
                                        priorityNotification.OldPriority = prioirty.Title;
                                }
                            }

                            if (UGITUtility.IfColumnExists(oldItem, DatabaseObjects.Columns.TicketStatus))
                            {
                                priorityNotification.OldTicketStatus = Convert.ToString(oldItem[DatabaseObjects.Columns.TicketStatus]);
                            }
                        }
                    }
                }
                // Log ticket event when ticket is being created
                if (newTicket)
                {
                    TicketEventManager eventHelper = new TicketEventManager(_context, this.Module.ModuleName, ticketId);
                    eventHelper.LogEvent(Constants.TicketEventType.Created, currentLifeCycleStage, user: actionUser);
                }
                //set Ticket Assigned By whenever PRP is changed
                if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketPRP) && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketAssignedBy))
                {
                    string newTicketPRP = Convert.ToString(item[DatabaseObjects.Columns.TicketPRP]);
                    if (!string.IsNullOrWhiteSpace(oldTicketPRPId) && oldTicketPRPId != newTicketPRP)
                        item[DatabaseObjects.Columns.TicketAssignedBy] = _context.CurrentUser.Id;
                }
                // For DRQ set TicketRapidRequest to "Yes" or "No" depending on whether DRQRapidTypeLookup has a value
                if (moduleName == ModuleNames.DRQ)
                {
                    long drqRapidTypeLookup = UGITUtility.StringToLong(item[DatabaseObjects.Columns.DRQRapidTypeLookup]);
                    DrqRapidTypesManager drqRapidTypesManager = new DrqRapidTypesManager(_context);
                    DRQRapidType dRQRapidType = null;
                    if (drqRapidTypeLookup > 0)
                        dRQRapidType = drqRapidTypesManager.LoadByID(drqRapidTypeLookup);
                    if (dRQRapidType == null) // Shows up as "0" OR "0;#" when not selected!
                    {
                        // If Rapid Type is not set (null), set to Low Priority & Urgent = No
                        if (Module.List_Priorities != null && Module.List_Priorities.Count > 0)
                        {
                            ModulePrioirty priority = Module.List_Priorities.OrderByDescending(x => x.ItemOrder).FirstOrDefault();  // Normal priority (usually last in list)
                            if (priority != null)
                                item[DatabaseObjects.Columns.TicketPriorityLookup] = priority.ID;
                        }
                        item[DatabaseObjects.Columns.TicketRapidRequest] = "No";
                    }
                    else
                    {
                        // If Rapid Type is set to something, set to High Priority & Urgent = Yes
                        if (Module.List_Priorities != null && Module.List_Priorities.Count > 0)
                        {
                            ModulePrioirty priority = Module.List_Priorities.OrderBy(x => x.ItemOrder).FirstOrDefault(); // Highest priority (first in list)
                            if (priority != null)
                                item[DatabaseObjects.Columns.TicketPriorityLookup] = priority.ID;
                        }
                        item[DatabaseObjects.Columns.TicketRapidRequest] = "Yes";
                    }
                }
                if (currentLifeCycleStage != null && oldStageStep < currentLifeCycleStage.StageStep)
                {
                    TicketEventManager ticketEventManager = new TicketEventManager(_context, moduleName, ticketId);
                    if (currentLifeCycleStage.StageTypeChoice == StageType.Closed.ToString())
                    {
                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketClosedBy))
                            item[DatabaseObjects.Columns.TicketClosedBy] = _context.CurrentUser.Id;

                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketResolvedBy) && string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketResolvedBy])))
                            item[DatabaseObjects.Columns.TicketResolvedBy] = _context.CurrentUser.Id;

                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.ResolutionDate) && string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ResolutionDate])))
                            item[DatabaseObjects.Columns.ResolutionDate] = DateTime.Now;

                        ticketEventManager.LogEvent(Constants.TicketEventType.Closed, currentLifeCycleStage, user: actionUser);
                    }
                    else if (currentLifeCycleStage.StageTypeChoice == StageType.Resolved.ToString())
                    {
                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketResolvedBy))
                            item[DatabaseObjects.Columns.TicketResolvedBy] = _context.CurrentUser.Id;

                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.ResolutionDate))
                            item[DatabaseObjects.Columns.ResolutionDate] = DateTime.Now;

                        ticketEventManager.LogEvent(Constants.TicketEventType.Resolved, currentLifeCycleStage, user: actionUser);
                    }

                    else if (currentLifeCycleStage.StageTypeChoice == StageType.Approved.ToString())
                    {
                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.ApprovedBy))
                            item[DatabaseObjects.Columns.ApprovedBy] = _context.CurrentUser.Id;

                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.ApprovalDate))
                            item[DatabaseObjects.Columns.ApprovalDate] = DateTime.Now;

                        // Note this is also done in SetNextStage
                        ticketEventManager.LogEvent(Constants.TicketEventType.Approved, currentLifeCycleStage, user: actionUser);
                    }

                    else if (currentLifeCycleStage.StageTypeChoice == StageType.Assigned.ToString())
                    {
                        string affectedUsers = string.Empty;
                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketPRP))
                        {
                            affectedUsers = Convert.ToString(item[DatabaseObjects.Columns.TicketPRP]);
                        }
                        //string affectedUsers =Convert.ToString(item[DatabaseObjects.Columns.TicketPRP]);
                        ticketEventManager.LogEvent(Constants.TicketEventType.Assigned, currentLifeCycleStage, affectedUsers: affectedUsers, user: actionUser);
                    }
                }

                //Update ERPJobID in OPM module with ERPJobIDNC
                if (Module.ModuleName == ModuleNames.OPM && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.ERPJobID))
                {
                    string erpjobid = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ERPJobID]);
                    if (!string.IsNullOrEmpty(erpjobid))
                    {
                        item[DatabaseObjects.Columns.ERPJobIDNC] = erpjobid;
                        item[DatabaseObjects.Columns.ERPJobID] = null;
                    }
                }
                // Add or Update record into table
                if (newTicket && item.RowState == DataRowState.Detached)
                {
                    item.Table.Rows.Add(item);
                }

                try
                {
                    int result = ticketManager.Save(Module, item);
                    if (result <= 0)
                        error = "Fail: Ticket Not Saved!";
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    throw new Exception("Fail", new Exception("Not able to save data for: " + ticketId));
                }

                if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketPriorityLookup))
                {
                    long pLookup = UGITUtility.StringToLong(item[DatabaseObjects.Columns.TicketPriorityLookup]);
                    if (pLookup > 0)
                    {
                        PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(_context);
                        ModulePrioirty modulePrioirty = prioirtyViewManager.LoadByID(pLookup);
                        if (modulePrioirty != null)
                            priorityNotification.NewPriority = modulePrioirty.Title;
                    }
                }
                //if AutoCreateDocumentLibrary is checked
                if (newTicket && Module.AutoCreateDocumentLibrary && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.DocumentLibraryName) &&
                    string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.DocumentLibraryName])))
                {
                    string ownerUser = string.Empty;
                    List<string> userList = new List<string>();

                    if (item.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers) && !string.IsNullOrEmpty(item[DatabaseObjects.Columns.TicketStageActionUsers].ToString()))
                    {
                        userList = $"{item[DatabaseObjects.Columns.TicketStageActionUsers]}".Split(new string[] { ";#" }, StringSplitOptions.None).ToList();
                    }
                    if (item.Table.Columns.Contains(DatabaseObjects.Columns.Owner) && !string.IsNullOrEmpty(item[DatabaseObjects.Columns.Owner].ToString()))
                    {
                        userList.Add(item[DatabaseObjects.Columns.Owner].ToString());
                    }
                    ownerUser = string.Join(",", userList.Select(x => x).Distinct());
                    DMSManagerBase dMSManagerBase = new DMSManagerBase(_context);
                    dMSManagerBase.CreatePortalForTicketElevated(_context, ticketId, _context.CurrentUser.Id, ownerUser, "true");
                }
                if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketStatus))
                    priorityNotification.NewTicketStatus = Convert.ToString(item[DatabaseObjects.Columns.TicketStatus]);

                // Save ticket event entry & send notification when Asset Owner Changed
                ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);

                if (moduleName == ModuleNames.CMDB && configurationVariableManager.GetValueAsBool("NotifyAssetOwner"))
                {
                    string assetTag = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.AssetTagNum);
                    string oldOwner = oldAssetOwner;
                    string currentAssetOwner = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.AssetOwner);
                    string currentOwner = currentAssetOwner;
                    UserProfile currentOwnerProfile = null;
                    UserProfileManager userProfileManager = new UserProfileManager(_context);
                    if (!string.IsNullOrWhiteSpace(currentOwner))
                        currentOwnerProfile = userProfileManager.LoadById(currentOwner);

                    string assetDescription = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.AssetDescription);
                    string assetSerialNum1 = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.SerialNum1);
                    string assetSerialNum2 = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.SerialNum2);
                    string assetSerialNum3 = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.SerialNum3);
                    string assetManufacturer = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.Manufacturer);
                    string assetModel = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.AssetModelLookup);
                    string assetType = UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.TicketRequestTypeLookup, true);
                    if (string.IsNullOrWhiteSpace(assetType))
                        assetType = "asset";

                    int changesCounter = 0;

                    // If owner was changed, notify new and old owners
                    if (!string.IsNullOrWhiteSpace(oldAssetOwner) && currentAssetOwner != oldAssetOwner)
                    {
                        TicketEventManager ticketEventManager = new TicketEventManager(_context, moduleName, ticketId);

                        List<string> affectedUsers = new List<string>();
                        if (!string.IsNullOrWhiteSpace(oldOwner))
                            affectedUsers.Add(oldOwner);
                        if (!string.IsNullOrWhiteSpace(currentOwner))
                            affectedUsers.Add(currentOwner);

                        UserProfile oldOwnerProfile = userProfileManager.LoadById(oldOwner);
                        UserProfile newOwnerProfile = userProfileManager.LoadById(currentOwner);
                        string eventReason = string.Format("{0} for Asset: {1} ({2}) from {3} => {4}", Constants.TicketEventType.OwnerChange, ticketId, assetTag, oldOwnerProfile.Name, newOwnerProfile.Name);

                        ticketEventManager.LogEvent(Constants.TicketEventType.OwnerChange, currentLifeCycleStage, eventReason: eventReason, affectedUsers: string.Join(Constants.Separator, affectedUsers));
                        string subject = string.Empty;
                        string body = string.Empty;
                        string emailBody = string.Empty;
                        string greeting = configurationVariableManager.GetValue("Greeting");
                        string signature = configurationVariableManager.GetValue("Signature");
                        string assetOwnerChangeFooter = configurationVariableManager.GetValue("AssetOwnerChangeFooter");

                        // Notify old owner they are no longer assigned the asset
                        if (oldOwnerProfile != null && !string.IsNullOrEmpty(oldOwnerProfile.Email))
                        {
                            subject = "Asset Re-assigned";
                            body = string.Format("{0} <b>{1}</b>,<br/><br/>The {2} with asset tag <b>{3}</b> is no longer assigned to you. This change may have been the result of an equipment upgrade or reassignment.",
                                                 greeting, oldOwnerProfile.Name, assetType, assetTag);

                            // Add appropriate footer if configured
                            if (!string.IsNullOrEmpty(assetOwnerChangeFooter))
                                body += "<br/><br/>" + assetOwnerChangeFooter;

                            emailBody = string.Format(@"{0} <br/><br/>{1}<br/>", body, signature);
                            emailBody += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, item, moduleName, true, false));

                            MailMessenger mail = new MailMessenger(_context);
                            mail.SendMail(oldOwnerProfile.Email, subject, string.Empty, emailBody, true, null, true, false, ticketId);
                        }

                        // Notify new owner of assignment
                        if (currentOwnerProfile != null && !string.IsNullOrEmpty(currentOwnerProfile.Email))
                        {
                            subject = "Asset Owner Assigned";
                            body = string.Format("{0} <b>{1}</b>,<br/><br/>The {2} with asset tag <b>{3}</b> was assigned to you today.",
                                                 greeting, currentOwnerProfile.Name, assetType, assetTag);

                            // Add appropriate footer if configured
                            if (!string.IsNullOrEmpty(assetOwnerChangeFooter))
                                body += "<br/><br/>" + assetOwnerChangeFooter;

                            emailBody = string.Format(@"{0} <br/><br/>{1}<br/>", body, signature);
                            emailBody += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, item, moduleName, true, false));

                            MailMessenger mail = new MailMessenger(_context);
                            mail.SendMail(currentOwnerProfile.Email, subject, string.Empty, emailBody, true, null, true, false, ticketId);
                        }
                    }
                    else if (!newTicket && currentOwnerProfile != null && !string.IsNullOrEmpty(currentOwnerProfile.Email))
                    {
                        // else notify current Owner of any asset detail changes.
                        string subject = "Asset Details Updated";
                        string body = string.Empty;
                        string bodyDetails = string.Empty;
                        string emailBody = string.Empty;
                        string greeting = configurationVariableManager.GetValue("Greeting");
                        string signature = configurationVariableManager.GetValue("Signature");

                        AssetDetailsUpdate(oldAssetDescription, oldAssetSNo1, oldAssetSNo2, oldAssetSNo3, oldAssetManufacturer, oldAssetModel, assetDescription, assetSerialNum1, assetSerialNum2, assetSerialNum3, assetManufacturer, assetModel, ref changesCounter, ref bodyDetails);
                        if (changesCounter > 0)
                        {
                            body = string.Format("{0} <b>{1}</b>,<br><br>The {2} with asset tag <b>{3}</b> has been updated with the following changes:",
                                                 greeting, currentOwnerProfile.Name, assetType, assetTag);
                            body += bodyDetails;

                            string assetDetailsChangeFooter = configurationVariableManager.GetValue("AssetDetailsChangeFooter");
                            if (!string.IsNullOrEmpty(assetDetailsChangeFooter))
                                body += "<br/><br/>" + assetDetailsChangeFooter;

                            emailBody = string.Format(@"{0} <br/><br/>{1}<br/>", body, signature);
                            emailBody += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, item, moduleName, true, false));

                            MailMessenger mail = new MailMessenger(_context);
                            mail.SendMail(currentOwnerProfile.Email, subject, string.Empty, emailBody, true, null, true, false, ticketId);
                        }
                    }
                }

                // Update target completion date based on SLA for expected resolution
                // Need to do it here so we can include this field in notification to Requestor
                if (newTicket && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketTargetCompletionDate))
                {
                    DateTime completionDate = EscalationProcess.GetSLACompletionDate(_context, item, this, Constants.SLACategory.Resolution);
                    if (completionDate != DateTime.MinValue)
                    {
                        item[DatabaseObjects.Columns.TicketTargetCompletionDate] = completionDate;
                        int result = ticketManager.Save(Module, item);
                    }
                }

                #endregion

                objModuleUserStatisticsManager.ADDUpdateModuleUserStatistics(item, Module.ModuleName);

                #region "Update Closeout Start based on Construction End Date"
                if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.EstimatedConstructionEnd) && UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionEnd]) != DateTime.MinValue)
                {
                    if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.CloseoutStartDate))
                    {
                        // if Closeout end date is hide from form layout then it will set the cookie value on changing the const end date. 
                        bool IsConstChanged = HttpContext.Current != null && UGITUtility.GetCookieValue(HttpContext.Current.Request, "ConstEndDateChanged") == "1" ? true : false;
                        if (IsConstChanged) {
                            UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "ConstEndDateChanged");
                        }
                        item[DatabaseObjects.Columns.CloseoutStartDate] = uHelper.GetNextWorkingDateAndTime(_context, UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionEnd]));
                        // update closeoutenddate based on Const.End date
                        if (IsConstChanged || UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutDate]) == DateTime.MinValue
                            || UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutDate]) < UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutStartDate]))
                        {
                            DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context, UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutStartDate]), uHelper.getCloseoutperiod(_context));
                            item[DatabaseObjects.Columns.CloseoutDate] = dates[1];
                        }
                    }
                }
                else if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.CloseoutStartDate))
                {
                    item[DatabaseObjects.Columns.CloseoutStartDate] = DBNull.Value;
                }
                #endregion

                // Skip stages based on skip condition
                if (SetTicketSkipStages(item))
                {
                    int result = ticketManager.Save(Module, item);
                }

                //log stage transition whenever stage is changed
                #region log transition 
                //log stage transition whenever stage is changed
                int newStageStep = UGITUtility.StringToInt(item[DatabaseObjects.Columns.StageStep]);
                if (lifeCycleStages != null && lifeCycleStages.Count > 0)
                {
                    LifeCycleStage newStage = lifeCycleStages.FirstOrDefault(x => x.StageStep == newStageStep);
                    if (stageBeforeSave == null || (stageBeforeSave.StageStep != newStageStep))
                    {
                        if (actionUser != null)
                            LogStageTransition(item, stageBeforeSave, newStage, DateTime.Now, actionUser);
                        else
                            LogStageTransition(item, stageBeforeSave, newStage);
                    }
                }
                #endregion

                //Agent Summary
                if (agentSummary != null && agentSummary.IsAgentActivated == true)
                {
                    ModuleWorkflowHistoryManager workflowHistoryMgr = new ModuleWorkflowHistoryManager(_context);
                    List<ModuleWorkflowHistory> lstmoduleWorkflowHistory = workflowHistoryMgr.Load(x => x.TicketId == item[DatabaseObjects.Columns.TicketId].ToString()).ToList();
                    agentSummary.StageSteps = "";
                    foreach (ModuleWorkflowHistory moduleWorkflowHistory in lstmoduleWorkflowHistory)
                    {
                        LifeCycleStage lifeCyclestage = new LifeCycleStageManager(_context).Load(x => x.ModuleNameLookup == moduleWorkflowHistory.ModuleNameLookup && x.StageStep == moduleWorkflowHistory.StageStep).SingleOrDefault();
                        if (!agentSummary.Stages.Contains(lifeCyclestage))
                        {
                            agentSummary.Stages.Add(lifeCyclestage);
                            agentSummary.StageSteps = agentSummary.StageSteps + lifeCyclestage.StageStep + ";";
                        }
                    }
                    if (item != null)
                    {
                        if (item.Table.Columns.Contains(DatabaseObjects.Columns.AgentSummary))
                        {
                            item[DatabaseObjects.Columns.AgentSummary] = Newtonsoft.Json.JsonConvert.SerializeObject(agentSummary);
                        }

                    }

                    ticketManager.Save(Module, item);
                }

                // Default Role for APP
                id = UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]);
                if (Module.ModuleName.Equals(ModuleNames.APP, StringComparison.OrdinalIgnoreCase) && newTicket && id > 0)
                {
                    ApplicationRoleManager applicationRoleManager = new ApplicationRoleManager(_context);
                    ApplicationRole spitem = new ApplicationRole();
                    spitem.Title = "User";
                    spitem.APPTitleLookup = id;
                    spitem.ItemOrder = 1;
                    spitem.ApplicationRoleModuleLookup = "0";
                    spitem.Description = "Default User Role";
                    applicationRoleManager.Insert(spitem);
                }

                #region Create Asset Relationship
                string currentAssetId = string.Empty;
                if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.AssetLookup))
                {
                    string assetLookupColl = Convert.ToString(item[DatabaseObjects.Columns.AssetLookup]);
                    if (!string.IsNullOrWhiteSpace(assetLookupColl))
                    {
                        AssetsManger assetsManger = new AssetsManger(_context);

                        List<string> lstOfasset = assetLookupColl.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string assetLookup in lstOfasset)
                        {
                            string assetLook = assetLookup;
                            if (UGITUtility.StringToLong(assetLook) > 0)
                            {
                                currentAssetId = assetLook;
                                if (!string.IsNullOrWhiteSpace(oldassetLookup))
                                {
                                    List<string> lstOfOldAssets = oldassetLookup.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    if (!lstOfOldAssets.Any(x => x == currentAssetId))
                                    {
                                        assetsManger.CreateRelationWithIncident(currentAssetId, ticketId);
                                        assetsManger.CreateAssetHistory(assetLookup, item);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                string title = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                if (visitedUrl != null)
                {
                    if (!newTicket)
                        ULog.AuditTrail(_context.CurrentUser.Name, string.Format("saved ticket {0}", ticketId), visitedUrl);
                    else
                        ULog.AuditTrail(_context.CurrentUser.Name, string.Format("saved new {0} ticket", moduleName), visitedUrl);
                }
                else
                {
                    if (!newTicket)
                        ULog.AuditTrail(string.Format("User {0} saved ticket {1}", _context.CurrentUser.Name, ticketId));
                    else
                        ULog.AuditTrail(string.Format("User {0} saved new {1} ticket", _context.CurrentUser.Name, moduleName));
                }
                //Escalation Update
                if (!donotUpdateEscalations)
                {
                    UpdateInBackGround(_context, item);
                }
                if (!newTicket && !stopUpdateDependencies)
                {
                    //Update parent SVC ticket &sibling SVC tasks if ticket Status Changes.
                    if (taskManager == null)
                        taskManager = new UGITTaskManager(_context);
                    taskManager.UpdateDependentSVCTasks(item);
                }
                // Update the Scheduled Email Reminders for DRQ & CMT
                AgentJobHelper agent = new AgentJobHelper(_context);
                if (moduleName == ModuleNames.DRQ)
                    agent.UpdateDRQScheduleActionEmail(item, Module.ModuleId);
                else if (moduleName == ModuleNames.CMT)
                    agent.UpdateContractReminder(item, Module);
                // Schedule auto stage move if needed
                if (!newTicket && !donotUpdateEscalations)
                    agent.ScheduleAutoStage(item, currentLifeCycleStage.CustomProperties, moduleName);


                //update complexity summary table if complexity changes
                if (!newTicket && (moduleName == ModuleNames.CPR || moduleName == ModuleNames.OPM))
                {
                    if (UGITUtility.StringToInt(item[DatabaseObjects.Columns.CRMProjectComplexity]) != UGITUtility.StringToInt(oldItem[DatabaseObjects.Columns.CRMProjectComplexity]) ||
                        UGITUtility.StringToDouble(item[DatabaseObjects.Columns.ApproxContractValue]) != UGITUtility.StringToDouble(oldItem[DatabaseObjects.Columns.ApproxContractValue]) ||
                        UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketClosed]) != UGITUtility.StringToBoolean(oldItem[DatabaseObjects.Columns.TicketClosed]))
                    {
                        ThreadStart threadStartRefreshComplexity = delegate ()
                        {
                            ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(_context);
                            allocationManager.RefreshProjectComplexityForProject(moduleName, ticketId);
                        };
                        Thread threadRefreshComplexity = new Thread(threadStartRefreshComplexity);
                        threadRefreshComplexity.IsBackground = true;
                        threadRefreshComplexity.Start();
                    }
                }
                if (moduleName == ModuleNames.CPR && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TagMultiLookup))
                {
                    //string tags = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TagMultiLookup]);
                    //ThreadStart threadStartRefreshExperienceTags = delegate ()
                    //{
                    //    ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(_context);
                    //    allocationManager.UpdateUserProjectTagExperience(_context, tags, ticketId);
                    //};
                    //Thread threadExperienceTags = new Thread(threadStartRefreshExperienceTags);
                    //threadExperienceTags.IsBackground = true;
                    //threadExperienceTags.Start();
                }
                //Copy OPM related data into CPR/CNS while creating CPR/CNS from OPM
                if (newTicket && (moduleName == "CPR" || moduleName == "CNS") && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.TicketOPMIdLookup) && UGITUtility.StringToInt(item[DatabaseObjects.Columns.TicketOPMIdLookup]) > 0)
                {
                    ThreadStart threadStartRefreshComplexity = delegate ()
                    {
                        ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(_context);
                        allocationManager.CopyDependentDataFromOPM(ticketId, UGITUtility.StringToInt(item[DatabaseObjects.Columns.TicketOPMIdLookup]));
                    };
                    Thread threadRefreshComplexity = new Thread(threadStartRefreshComplexity);
                    threadRefreshComplexity.IsBackground = true;
                    threadRefreshComplexity.Start();
                }

                #region add exit criteria tasks from templates
                if (newTicket)
                {
                    #region exit criteria 
                    //ULog.WriteException("Method ConfigureModuleStageTaskInTicket Called Inside Thread In Event CommitChanges on Class Ticket.cs: ticketid " + ticketId);

                    ThreadStart threadStartcreateModuleStageTaskInTicket = delegate () { ConfigureModuleStageTaskInTicket(Module, ticketId); };
                    Thread sThreadStartcreateModuleStageTaskInTicket = new Thread(threadStartcreateModuleStageTaskInTicket);
                    sThreadStartcreateModuleStageTaskInTicket.IsBackground = true;
                    sThreadStartcreateModuleStageTaskInTicket.Start();
                    #endregion
                }
                #endregion

                #region Process Statistics

                ThreadStart threadStartProcessStatistics = delegate () 
                {
                    _statisticsManager.ProcessStatistics(new List<string> { ticketId }, Module);
                };
                Thread threadProcessStatistics = new Thread(threadStartProcessStatistics);
                threadProcessStatistics.IsBackground = true;
                threadProcessStatistics.Start();
                
                #endregion
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                error = "Fail:" + ex.Message;
            }
            finally
            {

            }

            return error;
        }

        private static void AssetDetailsUpdate(string oldAssetDescription, string oldAssetSNo1, string oldAssetSNo2, string oldAssetSNo3, string oldAssetManufacturer, string oldAssetModel, string assetDescription, string assetSerialNum1, string assetSerialNum2, string assetSerialNum3, string assetManufacturer, string assetModel, ref int changesCounter, ref string bodyDetails)
        {
            bodyDetails += string.Format("<br>");
            if (oldAssetDescription.Trim() != assetDescription.Trim())
            {
                bodyDetails += string.Format("<br> <b>Description: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldAssetDescription) ? "No Value" : oldAssetDescription, string.IsNullOrEmpty(assetDescription) ? "No Value" : assetDescription);
                changesCounter++;
            }
            if (oldAssetSNo1.Trim() != assetSerialNum1.Trim())
            {
                bodyDetails += string.Format("<br> <b>Serial Num1: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldAssetSNo1) ? "No Value" : oldAssetSNo1, string.IsNullOrEmpty(assetSerialNum1) ? "No Value" : assetSerialNum1);
                changesCounter++;
            }
            if (oldAssetSNo2.Trim() != assetSerialNum2.Trim())
            {
                bodyDetails += string.Format("<br> <b>Serial Num2: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldAssetSNo2) ? "No Value" : oldAssetSNo2, string.IsNullOrEmpty(assetSerialNum2) ? "No Value" : assetSerialNum2);
                changesCounter++;
            }
            if (oldAssetSNo3.Trim() != assetSerialNum3.Trim())
            {
                bodyDetails += string.Format("<br> <b>Serial Num3: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldAssetSNo3) ? "No Value" : oldAssetSNo3, string.IsNullOrEmpty(assetSerialNum3) ? "No Value" : assetSerialNum3);
                changesCounter++;
            }
            if (oldAssetManufacturer.Trim() != assetManufacturer.Trim())
            {
                bodyDetails += string.Format("<br> <b>Manufacturer: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldAssetManufacturer) ? "No Value" : oldAssetManufacturer, string.IsNullOrEmpty(assetManufacturer) ? "No Value" : assetManufacturer);
                changesCounter++;
            }
            if (oldAssetModel.Trim() != assetModel.Trim())
            {
                string oldValue = oldAssetModel;
                string newValue = assetModel;
                bodyDetails += string.Format("<br> <b>Asset Model: </b> [{0}] => [{1}]", string.IsNullOrEmpty(oldValue) ? "No Value" : oldValue, string.IsNullOrEmpty(newValue) ? "No Value" : newValue);
                changesCounter++;
            }
        }
        private void UpdateInBackGround(ApplicationContext _context, DataRow item)
        {
            if (!UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, item.Table) || !UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.SLADisabled]))
            {
                UpdateEscalation(_context, item);
            }
            if (Module != null && Convert.ToString(Module.ModuleType) == ModuleType.SMS.ToString())
            {
                PopulateDashboard objPopulateDashboard = new PopulateDashboard(_context);
                objPopulateDashboard.PopulateDashboardItem(Module, item);
            }
            if (Module != null && Module.ModuleName == ModuleNames.APP)
            {
                ApplicationHelper objApplicationHelper = new ApplicationHelper(_context);
                objApplicationHelper.SyncToRequestType(_context, item);
            }

        }


        public bool IsQuickClose(DataRow item)
        {
            if (string.IsNullOrEmpty(moduleName) || item == null)
                return false;

            if (moduleName.ToLower() == "prs" || moduleName.ToLower() == "tsr")
            {
                if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketInitiatorResolved) && UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketInitiatorResolved]))
                    return true;
                else if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == UGITUtility.GetLookupID(DatabaseObjects.Columns.TicketRequestTypeLookup));
                    if (requestType != null)
                        return (requestType.WorkflowType == Constants.QuickClose);
                }
            }

            return false;
        }

        private bool IsQuickClose(DataRow item, List<TicketColumnValue> columnValues)
        {
            try
            {
                if (moduleName.ToLower() == "prs" || moduleName.ToLower() == "tsr")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketInitiatorResolved);
                    if (colVal != null)
                    {
                        return UGITUtility.StringToBoolean(colVal.Value);
                    }
                    else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketInitiatorResolved))
                    {
                        return UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketInitiatorResolved]);
                    }
                    else
                    {
                        TicketColumnValue colVal2 = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeCategory);
                        if (colVal2 != null)
                        {
                            //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(colVal2.Value));
                            //if (lookup != null && lookup.LookupId > 0)
                            //{
                            //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                            //    if (requestType != null)
                            //        return requestType.WorkFlowType == Constants.
                            ;
                            //}
                        }
                        else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeCategory))
                        {
                            //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeCategory]));
                            //if (lookup != null && lookup.LookupId > 0)
                            //{
                            //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                            //    if (requestType != null)
                            //        return requestType.WorkFlowType == Constants.Requisition;
                            //}
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return false;
            }
        }

        private bool IsAquisition(DataRow item, List<TicketColumnValue> columnValues)
        {
            try
            {

                TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup);
                if (colVal != null)
                {
                    //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(colVal.Value));
                    //if (lookup != null && lookup.LookupId > 0)
                    //{
                    //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                    //    if (requestType != null)
                    //        return requestType.WorkFlowType == Constants.Requisition;
                    //}
                }
                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                    //if (lookup != null && lookup.LookupId > 0)
                    //{
                    //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                    //    if (requestType != null)
                    //        return requestType.WorkFlowType == Constants.Requisition;
                    //}
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return false;
        }

        private bool CheckHardwareSoftwareMandatoryField(List<TicketColumnValue> columnValues, string fieldName)
        {
            bool noError = true;
            try
            {
                if (moduleName.ToLower() == "tsr")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == fieldName);
                    if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketGLCode &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                    else if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.DepartmentLookup &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return noError;
        }

        private bool CheckQuickCloseMandatoryField(List<TicketColumnValue> columnValues, string fieldName)
        {
            bool noError = true;
            try
            {
                if (moduleName.ToLower() == "prs" || moduleName.ToLower() == "tsr")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == fieldName);
                    if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketActualHours &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                    if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketResolutionComments &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return noError;
        }

        private bool IsNoTest(DataRow item, List<TicketColumnValue> columnValues)
        {
            try
            {
                TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup);
                if (colVal != null)
                {
                    //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(colVal.Value));
                    //if (lookup != null && lookup.LookupId > 0)
                    //{
                    //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                    //    if (requestType != null)
                    //        return requestType.WorkFlowType == Constants.NoTest;
                    //}
                }
                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                    //if (lookup != null && lookup.LookupId > 0)
                    //{
                    //    ModuleRequestType requestType = this.Module.List_RequestTypes.FirstOrDefault(x => x.ID == lookup.LookupId);
                    //    if (requestType != null)
                    //        return requestType.WorkFlowType == Constants.NoTest;
                    //}
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return false;
        }

        private bool CheckNoTestField(List<TicketColumnValue> columnValues, string fieldName)
        {
            bool noError = true;
            try
            {
                if (moduleName.ToLower() == "prs" || moduleName.ToLower() == "tsr" || moduleName.ToLower() == "acr")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == fieldName);
                    if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketTester &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);

            }
            return noError;
        }

        private bool IsReviewRequired(DataRow item, List<TicketColumnValue> columnValues)
        {
            try
            {
                if (moduleName.ToLower() == "cmt")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.NeedReview);
                    if (colVal != null)
                    {
                        return UGITUtility.StringToBoolean(colVal.Value);
                    }
                    else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.NeedReview))
                    {
                        return UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.NeedReview]);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return false;
            }
        }

        private bool CheckReviewMandatoryField(List<TicketColumnValue> columnValues, string fieldName)
        {
            bool noError = true;
            try
            {
                if (moduleName.ToLower() == "cmt")
                {
                    TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == fieldName);
                    if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketFinanceManager &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                    else if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketPurchasing &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                    else if (colVal != null && colVal.InternalFieldName == DatabaseObjects.Columns.TicketLegal &&
                        Convert.ToString(colVal.Value) == string.Empty)
                    {
                        noError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return noError;
        }

        private bool CheckMSAMandatoryFieldForVCC(List<TicketColumnValue> columnValues, string fieldName)
        {
            bool noError = true;

            TicketColumnValue colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == fieldName);
            if (colVal != null && (colVal.InternalFieldName == DatabaseObjects.Columns.VendorSOWLookup || colVal.InternalFieldName == DatabaseObjects.Columns.VendorSOWNameLookup)
                && string.IsNullOrEmpty(Convert.ToString(colVal.Value)))
            {
                noError = false;
            }

            if (!noError)
            {
                colVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.VendorMSALookup);
                if (colVal != null && string.IsNullOrEmpty(Convert.ToString(colVal.Value)))
                {
                    noError = false;
                }
                else
                {
                    noError = true;
                }
            }
            return noError;
        }

        public void SetItemValues(DataRow item, List<TicketColumnValue> columnValues, bool adminOverride, bool updateChangeInHistory, string userId)
        {
            SetItemValues(item, columnValues, adminOverride, updateChangeInHistory, userId, false);
        }

        public void SetTicketPriority(DataRow currentTicket, string moduleName, bool? setVIP = null)
        {
            DataTable thisList = currentTicket.Table;

            // First check if we have a VIP priority configured
            ModulePrioirty vipPriority = Module.List_Priorities.FirstOrDefault(x => x.IsVIP);

            if (vipPriority != null)
            {
                if (setVIP.HasValue)
                {
                    if (setVIP == true && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ElevatedPriority, thisList))
                    {
                        currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = vipPriority.ID; // new LookupValue(vipPriority.ID, vipPriority.Name);
                        return;
                    }
                }
                else
                {
                    if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.TicketRequestor))
                    {
                        UserValue userLookup = new UserValue(_context, Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketRequestor)));
                        string vipGroup = objConfigurationVariableHelper.GetValue(ConfigConstants.VIPGroup);

                        if (userLookup != null && !string.IsNullOrEmpty(userLookup.ID) && userLookup.userProfile != null && _context.UserManager.CheckUserIsInGroup(vipGroup, userLookup.userProfile))
                        {
                            currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = vipPriority.ID; // new LookupValue(vipPriority.ID, vipPriority.Name);
                            return;
                        }
                    }
                }
            }

            if (!PriorityMappingEnabled())
                return;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketImpactLookup, thisList)
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketSeverityLookup, thisList)
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, thisList))
            {
                if (currentTicket[DatabaseObjects.Columns.TicketImpactLookup] != null && currentTicket[DatabaseObjects.Columns.TicketSeverityLookup] != null)
                {
                    int iLookup = UGITUtility.StringToInt(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketImpactLookup]));
                    int sLookup = UGITUtility.StringToInt(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketSeverityLookup]));

                    if (iLookup > 0 && sLookup > 0)
                    {
                        ModuleImpact impact = Module.List_Impacts.FirstOrDefault(x => x.ID == iLookup);
                        ModuleSeverity severity = Module.List_Severities.FirstOrDefault(x => x.ID == sLookup);
                        if (impact != null && severity != null)
                        {
                            ModulePriorityMap pMap = Module.List_PriorityMaps.FirstOrDefault(x => x.SeverityLookup == severity.ID && x.ImpactLookup == impact.ID);
                            if (pMap != null && pMap.ID > 0 && pMap.PriorityLookup > 0)
                            {
                                ModulePrioirty priority = Module.List_Priorities.FirstOrDefault(x => x.ID == pMap.PriorityLookup && !x.Deleted);
                                if (priority != null && priority.ID > 0)
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priority.ID;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetItemValues(DataRow item, List<TicketColumnValue> columnValues, bool adminOverride, bool updateChangeInHistory, string userId, bool batchEditing, bool isAPIcall = false)
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(_context);
            FieldConfiguration fieldConfig = null;

            List<string> history = new List<string>();
            string oldValue = string.Empty;
            string newValue = string.Empty;


            LifeCycleStage currentStage = GetTicketCurrentStage(item);
            #region get old ticket priority before changes
            string existingPriorityLookup = null;
            bool? isPriorityElevated = null;

            if (item != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, item.Table))
            {
                existingPriorityLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketPriorityLookup));
                isPriorityElevated = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ElevatedPriority));
            }
            #endregion
            ModuleRoleWriteAccess roleAccessField = null;
            AssetsManger assetsManger = new AssetsManger(_context);
            List<Assests> assests = null;
            List<string> updatedFields = new List<string>();
            List<Statistics> statisticsList = new List<Statistics>();
            List<StatisticsConfiguration> statisticsFieldsToTrack = _statisticsConfigurationManager.Load(sc => sc.ModuleName == moduleName);
            DataTable originalItemTable = item.Table.Copy();
            DataRow originalItem = item.Table.Copy().Rows.Count > 0 ? item.Table.Copy().Rows[0] : null;

            bool isScheduleDurationProcessed = false;

            foreach (TicketColumnValue val in columnValues)
            {

                //condition for batch editing..
                if (Convert.ToString(val.Value) == "<Value Varies>")
                    continue;

                //ModuleRoleWriteAccess roleAccessField = Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName == val.InternalFieldName);
                roleAccessField = Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName == val.InternalFieldName);
                bool showInHistory = false;
                bool noHistoryOnFirstEntry = false;


                // Check change value will be logged or not using following logic
                // if field is editable with edit button and checkbox then log it every time.
                // if field is editable directly then only log it when [Old Value] != [New Value]. Do not log when [No Value] != [New Value]
                if (roleAccessField != null)
                {
                    showInHistory = true;
                    noHistoryOnFirstEntry = true;
                    if (roleAccessField.ShowEditButton || roleAccessField.ShowWithCheckbox)
                    {
                        noHistoryOnFirstEntry = false;
                    }
                }

                if (adminOverride || isAPIcall)
                    showInHistory = true;

                DataColumn field = item.Table.Columns[val.InternalFieldName];
                if (field == null || field.ReadOnly)
                    continue;

                // Check for the statistics fields if value changes and record it
                if (originalItemTable.Rows.Count > 0)
                {
                    if (statisticsFieldsToTrack.Any(sf => sf.FieldName == val.InternalFieldName && val.InternalFieldName != DatabaseObjects.Columns.CRMDuration))
                    {
                        Statistics statistics = _statisticsManager.GetChangedFieldValue(Module, originalItem, val, _context.CurrentUser.Id, field);
                        if (statistics != null)
                            statisticsList.Add(statistics);
                    }

                    if (statisticsFieldsToTrack.Any(sf => sf.FieldName == DatabaseObjects.Columns.CRMDuration)
                        && (val.InternalFieldName == DatabaseObjects.Columns.CRMDuration
                        || val.InternalFieldName == DatabaseObjects.Columns.PreconStartDate
                        || val.InternalFieldName == DatabaseObjects.Columns.PreconEndDate
                        || val.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionStart
                        || val.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd
                        || val.InternalFieldName == DatabaseObjects.Columns.CloseoutDate) && !isScheduleDurationProcessed && moduleName == ModuleNames.CPR)
                    {
                        Statistics preconStartDate = _statisticsManager.GetChangedFieldValue(Module, originalItem, 
                            columnValues.Find(x => x.InternalFieldName == DatabaseObjects.Columns.PreconStartDate), _context.CurrentUser.Id, field);
                        Statistics preconEndDate = _statisticsManager.GetChangedFieldValue(Module, originalItem,
                            columnValues.Find(x => x.InternalFieldName == DatabaseObjects.Columns.PreconEndDate), _context.CurrentUser.Id, field);
                        Statistics estimatedConstructionStart = _statisticsManager.GetChangedFieldValue(Module, originalItem,
                            columnValues.Find(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionStart), _context.CurrentUser.Id, field);
                        Statistics estimatedConstructionEnd = _statisticsManager.GetChangedFieldValue(Module, originalItem,
                            columnValues.Find(x => x.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd), _context.CurrentUser.Id, field);
                        Statistics closeoutDate = _statisticsManager.GetChangedFieldValue(Module, originalItem,
                            columnValues.Find(x => x.InternalFieldName == DatabaseObjects.Columns.CloseoutDate), _context.CurrentUser.Id, field);


                        if ((preconStartDate != null && preconStartDate.Value != null)
                            || (preconEndDate != null && preconEndDate.Value != null) 
                            || (estimatedConstructionStart != null && estimatedConstructionStart.Value != null)
                            || (estimatedConstructionEnd != null && estimatedConstructionEnd.Value != null)
                            || (closeoutDate != null && closeoutDate.Value != null))
                        {
                            if (originalItem[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value && originalItem[DatabaseObjects.Columns.CloseoutDate] != DBNull.Value)
                            {
                                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, Convert.ToDateTime(originalItem[DatabaseObjects.Columns.PreconStartDate]), Convert.ToDateTime(originalItem[DatabaseObjects.Columns.CloseoutDate]));
                                int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);
                                statisticsList.Add(new Statistics
                                {
                                    TicketID = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketId]),
                                    FieldName = DatabaseObjects.Columns.CRMDuration,
                                    Value = noOfWeeks.ToString(),
                                    CreatedBy = _context.CurrentUser.Id,
                                    Date = DateTime.Now
                                });
                            }
                        }
                        isScheduleDurationProcessed = true;
                    }
                    
                }
                
                if ((field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments ||
                    field.ColumnName == DatabaseObjects.Columns.TicketComment))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(val.Value)))
                    {
                        item[field.ColumnName] = uHelper.GetVersionString(userId, Convert.ToString(val.Value), item, field.ColumnName);
                    }
                }
                else if (field.ColumnName == DatabaseObjects.Columns.Retainage || field.ColumnName == DatabaseObjects.Columns.RetainageChoice) {

                    if (field.ColumnName == DatabaseObjects.Columns.RetainageChoice)
                    {
                        newValue = UGITUtility.ObjectToString(val.Value);
                        oldValue = UGITUtility.ObjectToString(item[field.ColumnName]);

                        if (string.IsNullOrEmpty(newValue) || newValue == "0")
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue) || oldValue == "0")
                            oldValue = "[No Value]";

                        if (newValue != "Other" && oldValue.ToLower() != newValue.ToLower() && showInHistory)
                        {
                            history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            item[val.InternalFieldName] = val.Value;
                        }
                        oldValue = newValue = string.Empty;
                    }
                    if (field.ColumnName == DatabaseObjects.Columns.Retainage && !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(val.Value)))
                    {
                        TicketColumnValue data = columnValues.Where(x => x.InternalFieldName == DatabaseObjects.Columns.RetainageChoice).FirstOrDefault();
                        if (data != null && UGITUtility.ObjectToString(data.Value) == "Other")
                        {
                            string newVal = UGITUtility.ObjectToString(val.Value);
                            string oldVal = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.RetainageChoice]);

                            if (string.IsNullOrEmpty(newVal) || newVal == "0")
                                newVal = "[No Value]";
                            else
                                newVal = newVal + "%";
                            if (string.IsNullOrEmpty(oldVal) || oldVal == "0")
                                oldVal = "[No Value]";
                            FieldConfiguration configField = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.RetainageChoice, Module.ModuleTable);
                            if (configField != null && !string.IsNullOrWhiteSpace(newVal))
                            {
                                if (!string.IsNullOrWhiteSpace(configField.Data))
                                {
                                    configField.Data = !UGITUtility.SplitString(configField.Data, Constants.Separator).Contains(newVal)
                                            ? configField.Data + Constants.Separator + newVal
                                            : configField.Data;
                                }
                                else
                                {
                                    configField.Data = newVal;
                                }
                                fieldManager.Update(configField);
                                if (oldVal.ToLower() != newVal.ToLower() && showInHistory)
                                {
                                    history.Add(string.Format("{0} ({1} => {2})", data.DisplayName, oldVal, newVal));
                                }
                                item[data.InternalFieldName] = newVal;
                            }
                        }
                    }
                }
                else if (field.ColumnName == DatabaseObjects.Columns.AssetLookup)
                {
                    if (assests == null || assests.Count == 0)
                        assests = assetsManger.GetAssetTicket();
                    string newVal = UGITUtility.ObjectToString(val.Value);
                    string oldVal = Convert.ToString(item[field.ColumnName]);

                    {

                        List<string> newCls = null;
                        if (!string.IsNullOrWhiteSpace(newVal))
                            newCls = newVal.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        List<string> oldCls = null;
                        if (!string.IsNullOrWhiteSpace(oldVal))
                            oldCls = oldVal.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (assests != null && assests.Count > 0)
                        {
                            Assests assest = null;
                            if (newCls != null && newCls.Count > 0)
                            {
                                List<string> lookupVals = newCls;
                                List<string> lstOfNew = new List<string>();
                                lookupVals.ForEach(x =>
                                {
                                    assest = assests.FirstOrDefault(y => y.ID == Convert.ToInt64(x));
                                    if (assest != null)
                                        lstOfNew.Add(assest.AssetTagNum);
                                });
                                assest = null;

                                if (lstOfNew.Count == 0)
                                    newCls = null;

                                if (lstOfNew != null)
                                    newValue = string.Join(", ", lstOfNew);
                            }
                            if (oldCls != null && oldCls.Count > 0)
                            {
                                List<string> lookupVals = oldCls;
                                List<string> lstOfOld = new List<string>();
                                lookupVals.ForEach(x =>
                                {
                                    assest = assests.FirstOrDefault(y => y.ID == Convert.ToInt64(x));
                                    if (assest != null)
                                        lstOfOld.Add(assest.AssetTagNum);
                                });
                                assest = null;

                                if (lstOfOld.Count == 0)
                                    oldCls = null;

                                if (lstOfOld != null)
                                    oldValue = string.Join(", ", lstOfOld);
                            }
                        }



                        if (string.IsNullOrEmpty(newValue))
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue))
                            oldValue = "[No Value]";

                        if (newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            oldValue = newValue = string.Empty;
                        }

                        if (newCls != null && newCls.Count == 0)
                            newCls = null;

                        item[val.InternalFieldName] = newVal;

                    }
                }
                else
                {
                    if (field.DataType.ToString() == DatabaseObjects.DataTypes.UserField)
                    {
                        //new line for batch create...
                    }

                    else if (field.DataType.ToString() == "Boolean" || field.DataType.Name.ToString() == "Boolean")
                    {
                        newValue = "False";
                        oldValue = "False";
                        if (UGITUtility.StringToBoolean(item[field.ColumnName]))
                            oldValue = "True";
                        if (UGITUtility.StringToBoolean(val.Value))
                            newValue = "True";

                        if (string.IsNullOrEmpty(newValue))
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue))
                            oldValue = "[No Value]";

                        if (oldValue.ToLower() != newValue.ToLower() && showInHistory)
                        {
                            history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            oldValue = newValue = string.Empty;
                        }
                        if (Convert.ToString(val.Value) == "No")
                        {
                            val.Value = false;
                        }
                        if (Convert.ToString(val.Value) == "Yes")
                        {
                            val.Value = true;
                        }
                        item[val.InternalFieldName] = val.Value;
                    }
                    else if (field.DataType.ToString() == DatabaseObjects.DataTypes.DateTime)
                    {
                        newValue = UGITUtility.GetDateStringInFormat(Convert.ToString(val.Value), false);
                        oldValue = UGITUtility.GetDateStringInFormat(Convert.ToString(item[field.ColumnName]), false);

                        if (string.IsNullOrEmpty(newValue))
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue))
                            oldValue = "[No Value]";

                        if (oldValue.ToLower() != newValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            oldValue = newValue = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(val.Value)))
                            item[val.InternalFieldName] = UGITUtility.StringToDateTime(val.Value);
                        else
                        {
                            item[val.InternalFieldName] = DBNull.Value;
                        }
                    }
                    else if (field.DataType.ToString() == DatabaseObjects.DataTypes.SMALLNUMBER)
                    {
                        newValue = UGITUtility.StringToInt(val.Value).ToString();
                        oldValue = UGITUtility.StringToInt(Convert.ToString(item[field.ColumnName])).ToString();

                        if (string.IsNullOrEmpty(newValue) || newValue == "0")
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue) || oldValue == "0")
                            oldValue = "[No Value]";


                        if (newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            oldValue = newValue = string.Empty;
                        }
                        if (UGITUtility.StringToInt(val.Value) > 0)
                            item[val.InternalFieldName] = UGITUtility.StringToInt(val.Value);
                        else
                            item[val.InternalFieldName] = DBNull.Value;
                    }
                    else if (field.DataType.ToString() == DatabaseObjects.DataTypes.Number || field.DataType.Name.ToString() == DatabaseObjects.DataTypes.Double || field.DataType.Name.ToString() == DatabaseObjects.DataTypes.Decimal)
                    {
                        newValue = UGITUtility.StringToDouble(val.Value).ToString();
                        oldValue = UGITUtility.StringToDouble(item[field.ColumnName]).ToString();

                        if (string.IsNullOrEmpty(newValue) || newValue == "0")
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue) || oldValue == "0")
                            oldValue = "[No Value]";


                        if (newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            fieldConfig = fieldManager.GetFieldByFieldName(val.InternalFieldName);
                            if (fieldConfig != null)
                            {
                                //fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values)
                                history.Add(string.Format("{0} ({1} => {2})", val.DisplayName,
                                                                            fieldManager.GetFieldConfigurationData(val.InternalFieldName, oldValue),
                                                                            fieldManager.GetFieldConfigurationData(val.InternalFieldName, newValue)));
                            }
                            else
                            {
                                history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValue, newValue));
                            }
                            oldValue = newValue = string.Empty;
                        }
                        if (UGITUtility.StringToDouble(val.Value) > 0)
                            item[val.InternalFieldName] = UGITUtility.StringToDouble(val.Value);
                        else
                            item[val.InternalFieldName] = DBNull.Value;
                    }
                    else if (field.DataType.ToString() == DatabaseObjects.DataTypes.MultiLookup) // IssueType, etc.
                    {
                        // Value is in format ";#Cannot Login to ERP System;#Accounts Payable;#"
                        string[] oldValues = UGITUtility.SplitString(Convert.ToString(item[field.ColumnName]), ";#", StringSplitOptions.RemoveEmptyEntries);
                        string[] newValues = UGITUtility.SplitString(val.Value, ";#", StringSplitOptions.RemoveEmptyEntries);

                        string[] changes = newValues.Except(oldValues).ToArray();
                        if (changes.Length == 0)
                            changes = oldValues.Except(newValues).ToArray();

                        if (changes.Length > 0 && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            oldValue = string.Join("; ", oldValues);
                            newValue = string.Join("; ", newValues);

                            if (string.IsNullOrEmpty(oldValue))
                                oldValue = "[No Value]";

                            if (string.IsNullOrEmpty(newValue))
                                newValue = "[No Value]";

                            history.Add(string.Format("{0} ({1} => {2})", field.ColumnName, oldValue, newValue));
                            oldValue = newValue = string.Empty;
                        }

                        item[val.InternalFieldName] = val.Value;
                    }
                    else
                    {
                        newValue = Convert.ToString(val.Value);
                        oldValue = Convert.ToString(item[field.ColumnName]);

                        if (string.IsNullOrEmpty(newValue))
                            newValue = "[No Value]";
                        if (string.IsNullOrEmpty(oldValue))
                            oldValue = "[No Value]";

                        newValue = newValue.Replace("\r\n", "\n");
                        if (field.ColumnName == DatabaseObjects.Columns.Attachments && newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            fieldConfig = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.Attachments);
                            string[] OldFileVal = oldValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            string[] NewFileVal = newValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            string OldFileName = string.Empty, NewFileName = string.Empty;
                            foreach (string item1 in OldFileVal)
                            {
                                if (!string.IsNullOrWhiteSpace(item1))
                                {
                                    var OldName = fieldManager.GetFieldConfigurationData(fieldConfig, Convert.ToString(item1));
                                    OldFileName = OldFileName + "," + OldName;
                                }
                            }

                            foreach (string item2 in NewFileVal)
                            {
                                if (!string.IsNullOrWhiteSpace(item2))
                                {
                                    var NewName = fieldManager.GetFieldConfigurationData(fieldConfig, Convert.ToString(item2));
                                    NewFileName = NewFileName + "," + NewName;
                                }
                            }


                            //history.Add(string.Format("{0} ({1} => {2})", field.ColumnName, oldValue, newValue));
                            if (NewFileName != ",")
                                history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, OldFileName.TrimStart(new char[] { ',' }), NewFileName.TrimStart(new char[] { ',' })));
                            else
                                history.Add("Attached file(s): -");

                            oldValue = newValue = string.Empty;
                        }
                        else // if (newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                        {
                            newValue = Convert.ToString(val.Value);
                            oldValue = Convert.ToString(item[val.InternalFieldName]);

                            if (string.IsNullOrEmpty(newValue))
                                newValue = "[No Value]";
                            if (string.IsNullOrEmpty(oldValue))
                                oldValue = "[No Value]";

                            oldValue = oldValue.Replace("\r\n", "\n");
                            newValue = newValue.Replace("\r\n", "\n");
                            if (newValue.ToLower() != oldValue.ToLower() && showInHistory && (!noHistoryOnFirstEntry || oldValue.ToLower() != "[no value]"))
                            {
                                fieldConfig = fieldManager.GetFieldByFieldName(val.InternalFieldName);
                                if (fieldConfig != null && (fieldConfig.Datatype == "UserField" || fieldConfig.Datatype == "Lookup"))
                                {
                                    history.Add(string.Format("{0} ({1} => {2})", val.DisplayName,
                                                                               fieldManager.GetFieldConfigurationData(val.InternalFieldName, oldValue).Replace(Constants.Separator, ", "),
                                                                               fieldManager.GetFieldConfigurationData(val.InternalFieldName, newValue).Replace(Constants.Separator, ", ")));
                                }
                                else
                                {
                                    string newValueHistory = UGITUtility.StripHTML(newValue);
                                    string oldValueHistory = UGITUtility.StripHTML(oldValue);
                                    if (newValueHistory.Length > 100)
                                        newValueHistory = UGITUtility.TruncateWithEllipsis(newValueHistory, 100);
                                    if (oldValueHistory.Length > 100)
                                        oldValueHistory = UGITUtility.TruncateWithEllipsis(oldValueHistory, 100);
                                    history.Add(string.Format("{0} ({1} => {2})", val.DisplayName, oldValueHistory, newValueHistory));
                                    oldValue = newValue = string.Empty;
                                }
                            }
                           
                            if (string.IsNullOrWhiteSpace(Convert.ToString(val.Value)))
                                item[val.InternalFieldName] = string.Empty;
                            else
                                item[val.InternalFieldName] = UGITUtility.StripHTML(Convert.ToString(val.Value), true);
                        }

                        item[val.InternalFieldName] = val.Value;
                    }
                }

                #region Save companies and divisions w.r.t selected departments
                //    if (moduleName == "PMM" || moduleName == "NPR" || moduleName == "TSK" &&
                //        uHelper.IfColumnExists(DatabaseObjects.Columns.TicketBeneficiaries, item.ParentList) && lookupField.LookupList == DatabaseObjects.Lists.Department)
                //    {
                //        SPFieldLookupValueCollection departmetnLookups = new SPFieldLookupValueCollection(Convert.ToString(Convert.ToString(item[DatabaseObjects.Columns.TicketBeneficiaries])));

                //        SPFieldLookupValueCollection companyLookups = new SPFieldLookupValueCollection();
                //        SPFieldLookupValueCollection divisionLookups = new SPFieldLookupValueCollection();

                //        List<Department> selectedDpts = uGITCache.LoadDepartments(item.Web).Where(x => departmetnLookups.Exists(y => y.LookupId == x.ID)).ToList();
                //        foreach (Department dpt in selectedDpts)
                //        {
                //            if (dpt.CompanyLookup != null && dpt.CompanyLookup.ID > 0)
                //            {
                //                SPFieldLookupValue cmplkup = new SPFieldLookupValue(dpt.CompanyLookup.ID, dpt.CompanyLookup.Value);
                //                companyLookups.Add(cmplkup);
                //            }

                //            if (dpt.DivisionLookup != null && dpt.DivisionLookup.ID > 0)
                //            {
                //                SPFieldLookupValue divlkup = new SPFieldLookupValue(dpt.DivisionLookup.ID, dpt.DivisionLookup.Value);
                //                divisionLookups.Add(divlkup);
                //            }
                //        }

                //        item[DatabaseObjects.Columns.CompanyMultiLookup] = null;
                //        if (companyLookups.Count > 0)
                //            item[DatabaseObjects.Columns.CompanyMultiLookup] = companyLookups;

                //        item[DatabaseObjects.Columns.DivisionMultiLookup] = null;
                //        if (divisionLookups.Count > 0)
                //            item[DatabaseObjects.Columns.DivisionMultiLookup] = divisionLookups;
                //    }
                #endregion

                if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.MarketSector) && !updatedFields.Contains(DatabaseObjects.Columns.MarketSector))
                {
                    //long.TryParse(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup]), out Id);
                    if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup])))
                    {
                        long Id = 0;
                        string tablename = moduleMgr.GetModuleTableName("COM");
                        var categoryId = GetTableDataManager.GetSingleValueByTicketId(tablename, DatabaseObjects.Columns.TicketRequestTypeLookup, Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup]), Module.TenantID);
                        if (categoryId != null && categoryId != DBNull.Value)
                        {
                            Id = Convert.ToInt64(categoryId);
                            item[DatabaseObjects.Columns.MarketSector] = requestTypeManager.LoadByID(Id)?.Category;
                        }
                    }

                    updatedFields.Add(DatabaseObjects.Columns.MarketSector);
                }

                if ((moduleName == "CON") && item[DatabaseObjects.Columns.FirstName] != null && item[DatabaseObjects.Columns.LastName] != null)
                {
                    item[DatabaseObjects.Columns.Title] = string.Format("{0} {1}", item[DatabaseObjects.Columns.FirstName], item[DatabaseObjects.Columns.LastName]);
                }

                // (*) after Title indicates Master Agreement for that Company
                if (moduleName == "COM" && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.MasterAgreement) && !updatedFields.Contains(DatabaseObjects.Columns.MasterAgreement))
                {
                    if (item[DatabaseObjects.Columns.MasterAgreement] != DBNull.Value && Convert.ToBoolean(item[DatabaseObjects.Columns.MasterAgreement]) == true)
                    {
                        if (!Convert.ToString(item[DatabaseObjects.Columns.Title]).Contains(" (*)"))
                        {
                            item[DatabaseObjects.Columns.Title] = $"{item[DatabaseObjects.Columns.Title]} (*)";
                        }
                    }
                    else if (item[DatabaseObjects.Columns.MasterAgreement] != DBNull.Value && Convert.ToBoolean(item[DatabaseObjects.Columns.MasterAgreement]) == false)
                    {
                        if (Convert.ToString(item[DatabaseObjects.Columns.Title]).Contains(" (*)"))
                        {
                            item[DatabaseObjects.Columns.Title] = Convert.ToString(item[DatabaseObjects.Columns.Title]).Replace(" (*)", string.Empty);
                        }
                    }

                    updatedFields.Add(DatabaseObjects.Columns.MasterAgreement);
                }

                if (moduleName == "OPM" || moduleName == "CPR" && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.Address) && !updatedFields.Contains(DatabaseObjects.Columns.Address))
                {
                    item[DatabaseObjects.Columns.Address] = GetAddress(item);
                    updatedFields.Add(DatabaseObjects.Columns.Address);
                }

                if (moduleName == "OPM" || moduleName == "CPR" && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.AcquisitionCost) && !updatedFields.Contains(DatabaseObjects.Columns.AcquisitionCost))
                {
                    //item[DatabaseObjects.Columns.AcquisitionCost] = GetAcquisitionCost(item);
                    //updatedFields.Add(DatabaseObjects.Columns.AcquisitionCost);

                    DataTable dt = GetForecastAndAcquisitionCosts(item);
                    if (dt.Rows.Count > 0)
                    {
                        item[DatabaseObjects.Columns.AcquisitionCost] = dt.Rows[0]["ForecastedAcquisitionCost"];
                        item[DatabaseObjects.Columns.ActualAcquisitionCost] = dt.Rows[0][DatabaseObjects.Columns.ActualAcquisitionCost];
                        item[DatabaseObjects.Columns.ForecastedProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ForecastedProjectCost];
                        item[DatabaseObjects.Columns.ActualProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ActualProjectCost];
                    }
                    updatedFields.Add(DatabaseObjects.Columns.AcquisitionCost);
                }

                if (moduleName == "CMDB" && (val.InternalFieldName == DatabaseObjects.Columns.Title || val.InternalFieldName == DatabaseObjects.Columns.AssetName))
                {

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AssetName, item.Table) && val.InternalFieldName == DatabaseObjects.Columns.Title)
                    {

                        item[DatabaseObjects.Columns.AssetName] = Convert.ToString(val.Value);

                    }
                    else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Title, item.Table) && val.InternalFieldName == DatabaseObjects.Columns.AssetName)
                    {

                        item[DatabaseObjects.Columns.Title] = Convert.ToString(val.Value);

                    }
                }

                if ((val.InternalFieldName == DatabaseObjects.Columns.CRMDuration || val.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd) &&
                    UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                    UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.CRMDuration) &&
                    UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.EstimatedConstructionEnd) &&
                    item[DatabaseObjects.Columns.EstimatedConstructionStart] != null && item[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value
                    //item[DatabaseObjects.Columns.CRMDuration] != null && item[DatabaseObjects.Columns.CRMDuration] != DBNull.Value &&
                    //(item[DatabaseObjects.Columns.EstimatedConstructionEnd] == null || item[DatabaseObjects.Columns.EstimatedConstructionEnd] == DBNull.Value)
                    )
                {
                    int noOfWorkingDays = 0;
                    if (val.InternalFieldName == DatabaseObjects.Columns.CRMDuration && Convert.ToString(val.Value) != "" && item[DatabaseObjects.Columns.EstimatedConstructionEnd] == DBNull.Value)
                    {
                        DateTime startDateNew = Convert.ToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionStart]); //DateTime.ParseExact(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.EstimatedConstructionStart]), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        noOfWorkingDays = uHelper.GetWorkingDaysInWeeks(_context, Convert.ToInt32(val.Value));
                        DateTime[] calculatedDates = uHelper.GetEndDateByWorkingDays(_context, startDateNew, noOfWorkingDays);
                        item[DatabaseObjects.Columns.EstimatedConstructionEnd] = calculatedDates[1];
                    }
                    else if (val.InternalFieldName == DatabaseObjects.Columns.EstimatedConstructionEnd && Convert.ToString(val.Value) != "")
                    {
                        noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, Convert.ToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionStart]), Convert.ToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionEnd]));
                        int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);
                        if (columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.CRMDuration) != null)
                            columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.CRMDuration).Value = noOfWeeks;
                        item[DatabaseObjects.Columns.CRMDuration] = noOfWeeks;

                        if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.CloseoutStartDate))
                        {
                            item[DatabaseObjects.Columns.CloseoutStartDate] = uHelper.GetNextWorkingDateAndTime(_context, UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionEnd]));
                            // update closeoutenddate based on Const.End date
                            oldValue = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.CloseoutDate]);
                            if (string.IsNullOrEmpty(oldValue))
                                oldValue = "[No Value]";
                            else
                                oldValue = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.CloseoutDate]), false);
                            bool IsConstChanged = HttpContext.Current != null && UGITUtility.GetCookieValue(HttpContext.Current.Request, "ConstEndDateChanged") == "1" ? true : false;
                            if (IsConstChanged)
                            {
                                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "ConstEndDateChanged");
                            }
                            if (IsConstChanged || UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutDate]) == DateTime.MinValue 
                                || UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutDate]) < UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutStartDate]))
                            {
                                DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context, UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutStartDate]), uHelper.getCloseoutperiod(_context));
                                item[DatabaseObjects.Columns.CloseoutDate] = dates[1];
                                history.Add(string.Format("{0} ({1} => {2})", DatabaseObjects.Columns.CloseoutDate, oldValue, UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.CloseoutDate]), false)));
                            }
                        }
                        

                    }

                }

            }
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Attachments, item.Table) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.IsTicketAttachment, item.Table))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.Attachments])))
                {
                    item[DatabaseObjects.Columns.IsTicketAttachment] = true;
                }
                else
                {
                    item[DatabaseObjects.Columns.IsTicketAttachment] = false;
                }
            }
            //Change priority based on impact and sevierty
            #region set Preority
            if (item != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, item.Table))
            {
                TicketColumnValue elevatedPriority = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.ElevatedPriority);
                if (elevatedPriority != null)
                    isPriorityElevated = UGITUtility.StringToBoolean(elevatedPriority.Value);

                SetTicketPriority(item, moduleName, isPriorityElevated);

                string newPriorityLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketPriorityLookup));
                if (!string.IsNullOrWhiteSpace(newPriorityLookup))
                {

                    ModulePrioirty priority = Module.List_Priorities.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(newPriorityLookup));
                    if (priority != null)
                    {
                        newValue = priority.Title;
                        if (!string.IsNullOrWhiteSpace(existingPriorityLookup))
                        {
                            ModulePrioirty oldpriority = Module.List_Priorities.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(existingPriorityLookup));
                            if (oldpriority != null)
                                oldValue = oldpriority.Title;
                        }

                        if (newValue.ToLower() != oldValue.ToLower() && (oldValue.ToLower() != "[no value]"))
                        {
                            if (isPriorityElevated != true)
                                history.Add(string.Format("{0} ({1} => {2})", "Priority", oldValue, newValue));
                            else
                                history.Add(string.Format("{0} Elevated ({1} => {2})", "Priority", oldValue, newValue));
                        }
                    }
                }
            }


            #endregion

            #region Set Department, Division, Company based on Requestor
            //if (uHelper.IsSPItemExist(item, DatabaseObjects.Columns.TicketRequestor) && uHelper.IfColumnExists(DatabaseObjects.Columns.DepartmentLookup, item.ParentList))
            //{
            //    int requestorUserId = 0;
            //    //UserProfile objRequestorUserProfile;

            //    SPFieldUserValueCollection userLookUp = new SPFieldUserValueCollection(_spWeb, Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor]));
            //    requestorUserId = userLookUp[0].LookupId;

            //    SPQuery queryUser = new SPQuery();
            //    queryUser.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", "ID", requestorUserId, "ContentType", "Person");
            //    SPListItemCollection userListCol = _spWeb.SiteUserInfoList.GetItems(queryUser);

            //    int departmentId = 0;
            //    if (userListCol != null && userListCol.Count > 0)
            //    {
            //        if (userListCol[0][DatabaseObjects.Columns.DepartmentLookup] != null)
            //        {
            //            SPFieldLookupValue deptLookup = new SPFieldLookupValue(Convert.ToString(userListCol[0][DatabaseObjects.Columns.DepartmentLookup]));
            //            departmentId = deptLookup.LookupId;
            //        }
            //    }
            //    else
            //    {
            //        SPGroup group = UserProfile.GetGroupByID(requestorUserId, _spWeb);
            //        if (group != null && group.Users.Count != 0)
            //        {
            //            SPQuery queryGroupUser = new SPQuery();
            //            queryGroupUser.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", "ID", group.Users[0].ID);
            //            SPListItemCollection GroupUserListCol = _spWeb.SiteUserInfoList.GetItems(queryGroupUser);
            //            if (GroupUserListCol != null && GroupUserListCol.Count > 0)
            //            {
            //                if (GroupUserListCol[0][DatabaseObjects.Columns.DepartmentLookup] != null)
            //                {
            //                    SPFieldLookupValue deptLookup = new SPFieldLookupValue(Convert.ToString(GroupUserListCol[0][DatabaseObjects.Columns.DepartmentLookup]));
            //                    departmentId = deptLookup.LookupId;
            //                }
            //            }
            //        }
            //    }

            //    if (moduleName == "TSR")
            //    {
            //        // For TSR, the user can override billing department
            //        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.DepartmentLookup])))
            //        {
            //            SPFieldLookupValue departmentLookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.DepartmentLookup]));
            //            departmentId = departmentLookup.LookupId;
            //        }
            //    }

            //    List<Department> listDepartment = uGITCache.LoadDepartments(_spWeb);
            //    Department filteredDepartment = listDepartment.FirstOrDefault(d => d.ID == departmentId);
            //    if (filteredDepartment != null)
            //    {
            //        if (filteredDepartment.ID > 0)
            //        {
            //            item[DatabaseObjects.Columns.DepartmentLookup] = uHelper.StringToInt(filteredDepartment.ID);
            //        }
            //        if (filteredDepartment.CompanyLookup != null && filteredDepartment.CompanyLookup.ID > 0)
            //        {
            //            item[DatabaseObjects.Columns.CompanyTitleLookup] = filteredDepartment.CompanyLookup.ID;
            //        }
            //        if (filteredDepartment.DivisionLookup != null && filteredDepartment.DivisionLookup.ID > 0)
            //        {
            //            item[DatabaseObjects.Columns.DivisionLookup] = filteredDepartment.DivisionLookup.ID;
            //        }
            //    }
            //}
            #endregion
            #region Calculate Actual Hours
            double totalTestingHours = -1;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketTestingTotalHours, item.Table))
                double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.TicketTestingTotalHours]), out totalTestingHours);

            double totalBAHours = -1;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketBATotalHours, item.Table))
                double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.TicketBATotalHours]), out totalBAHours);

            double totalDeveloperHours = -1;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketDeveloperTotalHours, item.Table))
                double.TryParse(Convert.ToString(item[DatabaseObjects.Columns.TicketDeveloperTotalHours]), out totalDeveloperHours);

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketActualHours, item.Table) && (totalTestingHours != -1 || totalBAHours != -1 || totalDeveloperHours != -1))
            {
                if (totalTestingHours == -1)
                    totalTestingHours = 0;
                if (totalBAHours == -1)
                    totalBAHours = 0;
                if (totalDeveloperHours == -1)
                    totalDeveloperHours = 0;

                double totalActualHours = totalTestingHours + totalBAHours + totalDeveloperHours;
                if (totalActualHours > 0)
                    item[DatabaseObjects.Columns.TicketActualHours] = totalActualHours;
            }
            #endregion

            #region VND code
            //if (uHelper.IfColumnExists(DatabaseObjects.Columns.VendorSOWLookup, item.Table) && item[DatabaseObjects.Columns.VendorSOWLookup] != null)
            //{
            //    SPFieldLookupValue lookup = null;
            //    TicketColumnValue sowVal = columnValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.VendorSOWLookup);
            //    if (sowVal != null)
            //    {
            //        if (sowVal.Value != null && !string.IsNullOrEmpty(Convert.ToString(sowVal.Value)))
            //            lookup = new SPFieldLookupValue(Convert.ToString(sowVal.Value));
            //    }
            //    else
            //        lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.VendorSOWLookup]));


            //    if (lookup != null && lookup.LookupId > 0 && uHelper.IfColumnExists(DatabaseObjects.Columns.VendorMSALookup, item.ParentList))
            //    {
            //        string sowID = Convert.ToString(lookup.LookupId);
            //        SPListItem sowItem = Ticket.getCurrentTicket("VSW", sowID);

            //        if (sowItem != null)
            //        {
            //            lookup = new SPFieldLookupValue(Convert.ToString(sowItem[DatabaseObjects.Columns.VendorMSALookup]));
            //            if (lookup != null && lookup.LookupId > 0)
            //                item[DatabaseObjects.Columns.VendorMSALookup] = lookup;
            //        }
            //    }
            //}

            ////link to DMS if any relation is configured
            //DocumentHelper.LinkModuleWithDMS(item, moduleName);
            #endregion

            #region CMDB
            //if (moduleName == "CMDB")
            //{

            //    SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.AssetModelLookup]));
            //    if (lookup != null)
            //    {
            //        if (lookup.LookupId != assetModelOldVal)
            //        {
            //            SPListItem itemAssetVendor = uHelper.GetListItem(DatabaseObjects.Lists.AssetModels, DatabaseObjects.Columns.Id, Convert.ToString(lookup.LookupId));
            //            if (itemAssetVendor != null)
            //            {
            //                lookup = new SPFieldLookupValue(Convert.ToString(itemAssetVendor[DatabaseObjects.Columns.VendorLookup]));
            //                if (lookup != null && lookup.LookupId > 0)
            //                    item[DatabaseObjects.Columns.VendorLookup] = lookup;
            //            }
            //        }

            //    }
            //    else
            //    {
            //        item[DatabaseObjects.Columns.VendorLookup] = null;
            //    }

            //}
            #endregion

            #region [+][7/11/2023][SANKET][PreconDuration, ConstructionDuration]
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.PreconStartDate, item.Table) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.PreconEndDate, item.Table))
            {
                if ((!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.PreconStartDate]))) && (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.PreconEndDate]))))
                {
                    item[DatabaseObjects.Columns.PreconDuration] = uHelper.GetTotalWorkingDaysBetween(_context, Convert.ToDateTime(item[DatabaseObjects.Columns.PreconStartDate]), Convert.ToDateTime(item[DatabaseObjects.Columns.PreconEndDate]), true);
                }
                else
                {
                    item[DatabaseObjects.Columns.PreconDuration] = 0;
                }
            }
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.EstimatedConstructionStart, item.Table) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.EstimatedConstructionEnd, item.Table))
            {
                if ((!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.EstimatedConstructionStart]))) && (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.EstimatedConstructionEnd]))))
                {
                    item[DatabaseObjects.Columns.EstimatedConstructionDuration] = uHelper.GetTotalWorkingDaysBetween(_context, Convert.ToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionStart]), Convert.ToDateTime(item[DatabaseObjects.Columns.EstimatedConstructionEnd]), true);
                }
                else
                {
                    item[DatabaseObjects.Columns.EstimatedConstructionDuration] = 0;
                }
            }
            #endregion

            //Put Changes in history
            if (updateChangeInHistory && history.Count > 0)
            {
                string historyStr = string.Join("<br/>", history.ToArray());
                if (adminOverride)
                {
                    if (batchEditing)
                        historyStr += " using Batch Editing";
                    else
                        historyStr += " using Admin Override";
                }

                if (isAPIcall)
                {
                    historyStr += " using API";
                }
                uHelper.CreateHistory(_context.CurrentUser, historyStr, item, false, _context);
            }

            // Insert Statistics
            _statisticsManager.InsertItems(statisticsList);
        }

        /// <summary>
        /// Gets the list of active lifecycle stages for this ticket filtering out any skipped stages
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public List<LifeCycleStage> GetActiveLifeCycleStages(DataRow ticket)
        {
            LifeCycle lifeCycle = GetTicketLifeCycle(ticket);
            if (lifeCycle == null)
                return null;

            List<LifeCycleStage> lifeCycleStages = new List<LifeCycleStage>();
            //ticket.Fields.ContainsField(DatabaseObjects.Columns.WorkflowSkipStages) && !string.IsNullOrEmpty(Convert.ToString(ticket[DatabaseObjects.Columns.WorkflowSkipStages]))
            if (ticket != null && !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.WorkflowSkipStages))))
            {
                string[] skippedStages = Convert.ToString(ticket[DatabaseObjects.Columns.WorkflowSkipStages]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in lifeCycle.Stages)
                {
                    int pos = Array.IndexOf(skippedStages, Convert.ToString(item.StageStep));
                    if (pos < 0)
                        lifeCycleStages.Add(item);
                }
            }
            else
            {
                lifeCycleStages = lifeCycle.Stages;
            }

            return lifeCycleStages;
        }
        public LifeCycleStage GetTicketCurrentStage(DataRow saveTicket, LifeCycle ticketLifeCycle)
        {
            if (ticketLifeCycle == null || ticketLifeCycle.Stages.Count <= 0)
            {
                return null;
            }

            LifeCycleStage currentStage = null;

            //Fetch current stage detail based on stage step
            if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.StageStep))
            {
                int step = 0;
                if (int.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(saveTicket, DatabaseObjects.Columns.StageStep)), out step))
                {
                    currentStage = ticketLifeCycle.Stages.FirstOrDefault(x => x.StageStep == step);
                }
            }
            else if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.ModuleStepLookup))
            {
                //Fetch current stage detail based on modulesteplookup
                //SPFieldLookupValue stageLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(saveTicket, DatabaseObjects.Columns.ModuleStepLookup)));
                //if (stageLookup.LookupId > 0)
                //{
                //    currentStage = ticketLifeCycle.Stages.FirstOrDefault(x => x.ID == stageLookup.LookupId);
                //}
            }

            // Safety net to prevent crash
            if (currentStage == null)
                currentStage = ticketLifeCycle.Stages[0];

            return currentStage;
        }

        public void Create(DataRow currentTicket, UserProfile CurrentUser, string PRP = null, ResetPasswordAgent resetPasswordAgent = null)
        {
            ticketActionType = TicketActionType.Created;
            // default section
            DataTable thisList = currentTicket.Table;
            List<ModulePriorityMap> moduleRequestPriorityObj = Module.List_PriorityMaps;
            List<LifeCycleStage> objLifeCycleStages = new List<LifeCycleStage>();


            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketCreationDate))
                currentTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now.ToString();
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketInitiator))
                if (currentTicket[DatabaseObjects.Columns.TicketInitiator] == System.DBNull.Value || Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketInitiator]) == String.Empty)
                    currentTicket[DatabaseObjects.Columns.TicketInitiator] = CurrentUser.Id;
            // Get a new ticket ID.
            currentTicket[DatabaseObjects.Columns.TicketId] = GetNewTicketId();

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectID, thisList) && Convert.ToString(currentTicket[DatabaseObjects.Columns.ProjectID]) == "")
            {
                // Condition added, as per Jon's suggestion in mail, 'BCCI Items 12.09.19', to remove ProjectID from LEM.
                if (moduleName != ModuleNames.LEM)
                    currentTicket[DatabaseObjects.Columns.ProjectID] = GetNewProjectId();
            }

            //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AssetTagNum, thisList))
            //{
            //    currentTicket[DatabaseObjects.Columns.AssetTagNum] = currentTicket[DatabaseObjects.Columns.TicketId];
            //}

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.AssetTagNum, thisList) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.AssetName, thisList))
            {
                if (string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.AssetName])))
                    currentTicket[DatabaseObjects.Columns.AssetName] = currentTicket[DatabaseObjects.Columns.AssetTagNum];
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Title, thisList) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.AssetName, thisList))
            {
                if (string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.Title])))
                    currentTicket[DatabaseObjects.Columns.Title] = currentTicket[DatabaseObjects.Columns.AssetName];
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCreationDate, thisList))
                currentTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now.ToString();


            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketInitiator, thisList) && (currentTicket[DatabaseObjects.Columns.TicketInitiator] == System.DBNull.Value || Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketInitiator]) == String.Empty))
                currentTicket[DatabaseObjects.Columns.TicketInitiator] = CurrentUser.Id;


            // set the current stage start date.

            currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now.ToString();

            // If the ticket is creating from related ticket page.
            // Create Relation Ticket code moved from here because in new ticket case till here ticket is not saved in the list, therefore the relation with that ticket can't be stablish.
            // Code moved to the Module web part function after ticket saved.

            //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.DRQRapidTypeLookup, thisList) &&
            //    UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRapidRequest, thisList) &&
            //    UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, thisList))
            //{
            //    string drqRapidTypeLookup = Convert.ToString(currentTicket[DatabaseObjects.Columns.DRQRapidTypeLookup]);
            //    if (drqRapidTypeLookup != string.Empty && drqRapidTypeLookup != "0") // Shows up as 0 when not selected!
            //    {
            //        // If Rapid Type is set to something, set to High Priority & Urgent = Yes
            //        if (moduleRequestPriorityObj.Count > 0)
            //        {
            //            ModulePrioirty priorityRow = Module.List_Priorities.FirstOrDefault(x => x.ID == moduleRequestPriorityObj[0].PriorityLookup);
            //            currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priorityRow.ID;
            //        }
            //            currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "Yes";
            //    }
            //    else
            //    {
            //        if (moduleRequestPriorityObj.Count > 0)
            //        {
            //            // If Rapid Type is not set (null), set to Low Priority & Urgent = No
            //            ModulePrioirty priorityRow = Module.List_Priorities.FirstOrDefault(x => x.ID == moduleRequestPriorityObj[2].PriorityLookup);
            //            if (priorityRow != null)
            //                currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priorityRow.ID;
            //        }
            //        currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "No";
            //    }
            //}
            // For DRQ set TicketRapidRequest to "Yes" or "No" depending on whether DRQRapidTypeLookup has a value
            if (moduleName == "DRQ")
            {
                string drqRapidTypeLookup = Convert.ToString(currentTicket[DatabaseObjects.Columns.DRQRapidTypeLookup]);
                if (string.IsNullOrWhiteSpace(drqRapidTypeLookup) || drqRapidTypeLookup.Contains("None") ||
                    drqRapidTypeLookup.StartsWith("0")) // Shows up as "0" OR "0;#" when not selected!
                {
                    // If Rapid Type is not set (null), set to Low Priority & Urgent = No
                    if (Module.List_Priorities != null && Module.List_Priorities.Count > 0)
                    {
                        ModulePrioirty priority = Module.List_Priorities.OrderByDescending(x => x.ItemOrder).FirstOrDefault();  // Normal priority (usually last in list)
                        if (priority != null)
                            currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priority.ID;
                    }
                    currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "No";
                }
                else
                {
                    // If Rapid Type is set to something, set to High Priority & Urgent = Yes
                    if (Module.List_Priorities != null && Module.List_Priorities.Count > 0)
                    {
                        ModulePrioirty priority = Module.List_Priorities.OrderBy(x => x.ItemOrder).FirstOrDefault(); // Highest priority (first in list)
                        if (priority != null)
                            currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priority.ID;
                    }
                    currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "Yes";
                }
            }
            // In DRQ TicketTargetCompletionDate is used and in PRS, TSR, ACR, TicketDesiredCompletionDate is entered
            //So make both fields the same
            if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate)
                && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate))
            {
                if (currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] == null && currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != null)
                    currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate];
                else if (currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] == null && currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] != null)
                    currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] = currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate];
            }

            if (resetPasswordAgent != null)
            {


                if (resetPasswordAgent.IsResetPasswordAgentActivated)
                {
                    currentTicket[DatabaseObjects.Columns.PRP] = PRP;
                }
                else
                {
                    Boolean AutoFillTicket = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoFillTicket);
                    if (AutoFillTicket)
                    {
                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.PRP, thisList))
                        {
                            currentTicket[DatabaseObjects.Columns.PRP] = PRP;
                        }
                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.EstimatedHours, thisList) && !string.IsNullOrEmpty(PRP) && currentTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                        {
                            currentTicket[DatabaseObjects.Columns.EstimatedHours] = 8;
                        }
                    }
                }

            }

            CheckRequestType(currentTicket, false);

            AssignModuleSpecificDefaults(currentTicket);
            AssignModuleSpecificDefaultUsers(currentTicket);

            ///commented four properties already set by ProjectManager.SetDefaultLifeCycleSettingOnProject method
            // Save default status (Initiated). Will be overwritten in QuickClose later
            // but do this here in case we get a crash before that (between two ticket saves) 
            // which otherwise leaves hanging ticket with blank status
            currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = CurrentUser.Id;
            //Below code execute for every module except PMM to set action user type,dataeditor 
            if (Module != null && Module.ModuleName != "PMM" && Module.List_LifeCycles != null && Module.List_LifeCycles.Count > 0)
            {
                LifeCycle defaultLifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                List<LifeCycleStage> lstOfStages = defaultLifeCycle.Stages;
                if (lstOfStages != null && lstOfStages.Count > 0)
                {
                    currentTicket[DatabaseObjects.Columns.TicketStatus] = lstOfStages.FirstOrDefault().StageTitle;
                    currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = GetStageActionUsers(1);

                    if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.DataEditor))
                        currentTicket[DatabaseObjects.Columns.DataEditor] = GetStageDataEditors(1);
                    currentTicket[DatabaseObjects.Columns.ModuleStepLookup] = Convert.ToString(lstOfStages.FirstOrDefault().ID);
                    currentTicket[DatabaseObjects.Columns.StageStep] = Convert.ToString(lstOfStages.FirstOrDefault().StageStep);
                }
            }


            #region MailQueue
            if (Module.ModuleName.Trim().ToUpper() == "DRQ")
            {
                if (currentTicket[DatabaseObjects.Columns.SLADisabled] == null || currentTicket[DatabaseObjects.Columns.SLADisabled] == DBNull.Value)
                {
                    currentTicket[DatabaseObjects.Columns.SLADisabled] = false;
                }
                ////Ticket.UpdateEMailQueue(currentTicket, SPContext.Current.Web);
                //AgentJobHelper agent = new AgentJobHelper(_spWeb);
                //agent.UpdateDRQScheduleActionEmail(currentTicket, moduleId);
            }
            #endregion
            #region SendReminder
            else if (Module.ModuleName.Trim().ToUpper() == "CMT")
            {
                //AgentJobHelper agentHelper = new AgentJobHelper(_spWeb);
                //agentHelper.UpdateCMTScheduleActionReminder(currentTicket, moduleId);
            }
            #endregion

            if (Module.List_LifeCycles != null && Module.List_LifeCycles.Count > 0)
            {
                objLifeCycleStages = Module.List_LifeCycles[0].Stages;
                if (Module.List_LifeCycles[0].Stages.Count > 0)
                {
                    string historyDescription = Convert.ToString(objLifeCycleStages[0].ApproveActionDescription);
                    if (string.IsNullOrEmpty(historyDescription))
                        historyDescription = "Ticket Initiated";
                    uHelper.CreateHistory(CurrentUser, historyDescription, currentTicket, true, _context);
                }
            }
        }

        public string GetNewTicketId()
        {
            string ticketId = string.Empty;

            if (Module != null)
            {
                var lastSequence = moduleMgr.GetModuleLastSequence(Module.ID);
                // Get next sequence number
                // int seqNum = int.Parse(Convert.ToString(Module.LastSequence)) + 1;
                int seqNum = int.Parse(Convert.ToString(lastSequence)) + 1;

                // Create the ticket id in format: PRS-14-123456, i.e. <modulename>-<2-digit current year>-<6-digit seq num>
                string moduleName = Module.ModuleName;
                ticketId = moduleName + "-" + DateTime.Now.ToString("yy") + "-" + seqNum.ToString(new string('0', 6));

                // Update module entry with latest sequence number & date
                Module.LastSequence = seqNum;
                Module.LastSequenceDate = DateTime.Now;
                moduleMgr.Update(Module);

                CacheHelper<object>.AddOrUpdate(moduleName, _context.TenantID, Module);
            }

            return ticketId;
        }

        public string GetNewProjectId()
        {
            string projectId = string.Empty;
            ConfigurationVariable configVariable = objConfigurationVariableHelper.LoadVaribale("LastSequenceProjectID");
            // Get next sequence number            
            int NoofDigitsInSeq = 0;
            long seqNum = 0;
            string ProjectIDFormat = objConfigurationVariableHelper.GetValue("ProjectIDFormat");
            if (configVariable != null)
                long.TryParse(configVariable.KeyValue, out seqNum);

            seqNum++;
            string[] arrProjectIDFormat = ProjectIDFormat.Split('-');
            string YearSubstring = arrProjectIDFormat.Where(x => x.Contains("Y")).FirstOrDefault() ?? string.Empty;
            string NumSubstring = arrProjectIDFormat.Where(x => x.Contains("N")).FirstOrDefault() ?? string.Empty;

            NoofDigitsInSeq = NumSubstring.Length;
            // Create the Project Id in format set in Configuration Variable.
            projectId = ProjectIDFormat;
            projectId = projectId.Replace(YearSubstring, DateTime.Now.ToString(YearSubstring.ToLower())).Replace(NumSubstring, seqNum.ToString(new string('0', NoofDigitsInSeq)));

            // Update Configuration variable with latest sequence number
            objConfigurationVariableHelper.Save("LastSequenceProjectID", Convert.ToString(seqNum));
            configVariable.KeyValue = Convert.ToString(seqNum);
            CacheHelper<ConfigurationVariable>.AddOrUpdate(configVariable.KeyName, _context.TenantID, configVariable);

            return projectId;
        }

        public bool CheckRequestType(DataRow currentTicket, bool updateOnlyWhenEmpty)
        {
            if (!UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketRequestTypeLookup))
                return true;

            ModuleRequestType request = null;
            long requestTypeID = UGITUtility.StringToLong(currentTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]);
            if (requestTypeID > 0)
                request = this.Module.List_RequestTypes.SingleOrDefault(x => x.ID == requestTypeID);

            if (request == null)
            {
                return false;
            }

            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.TicketRequestTypeCategory))
            {
                currentTicket[DatabaseObjects.Columns.TicketRequestTypeCategory] = request.Category;
            }
            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.TicketRequestTypeSubCategory))
            {
                currentTicket[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = request.SubCategory;
            }
            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.TicketRequestTypeWorkflow))
            {
                currentTicket[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = request.WorkflowType;
            }
            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.SLADisabled))
                currentTicket[DatabaseObjects.Columns.SLADisabled] = request.SLADisabled;


            try
            {
                if (this.Module.OwnerBindingChoice == TicketOwnerBinding.Auto.ToString() || // If auto-binding, always update from request type
              (this.Module.OwnerBindingChoice == TicketOwnerBinding.ClientSide.ToString() &&  // If client-side, update only if left blank!
              string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketOwner]))))
                {

                    if (!updateOnlyWhenEmpty || Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketOwner]) == string.Empty)
                    {
                        //Get ticket location which should be requestor location or user manually set location
                        int ticketLocationID = 0;
                        string ticketLocation = Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.LocationLookup));
                        ticketLocation = UGITUtility.SplitString(ticketLocation, Constants.Separator, 0);
                        int.TryParse(ticketLocation, out ticketLocationID);
                        if (ticketLocationID <= 0)
                        {
                            // If location was not set in ticket, try to get it from the first requestor
                            UserProfile userProfile = null;
                            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketRequestor))
                            {
                                string user = UGITUtility.SplitString(currentTicket[DatabaseObjects.Columns.TicketRequestor], Constants.Separator, 0);
                                if (!string.IsNullOrEmpty(user))
                                {
                                    int userID = 0;
                                    if (int.TryParse(user, out userID) && userID > 0)
                                    {
                                        userProfile = _context.UserManager.LoadById(Convert.ToString(userID));
                                        if (userProfile != null)
                                        {
                                            ticketLocationID = Convert.ToInt32(userProfile.LocationId);
                                            if (userProfile.LocationId > 0 && UGITUtility.IfColumnExists(DatabaseObjects.Columns.LocationLookup, currentTicket.Table) && currentTicket[DatabaseObjects.Columns.LocationLookup] == null)
                                                currentTicket[DatabaseObjects.Columns.LocationLookup] = string.Format("{0};#{1}", ticketLocationID, userProfile.Location);
                                            if (!string.IsNullOrEmpty(userProfile.DeskLocation) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.DeskLocation, currentTicket.Table) && string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.DeskLocation])))
                                                currentTicket[DatabaseObjects.Columns.DeskLocation] = userProfile.DeskLocation;
                                        }
                                    }
                                }
                            }
                        }


                        // Update Owner by Location or Request type and estimated hour if any
                        if (request != null)
                        {
                            DataTable thisList = currentTicket.Table;
                            ModuleRequestTypeLocation reqTypeLoc = null;
                            if (ticketLocationID > 0)
                                reqTypeLoc = this.Module.List_RequestTypeByLocation.FirstOrDefault(x => x.RequestTypeLookup == requestTypeID && x.LocationLookup == ticketLocationID);

                            // If Owner exist in request type by location table use it else use it from request type table only.
                            if (reqTypeLoc != null)
                            {

                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOwner, thisList))
                                {
                                    if (currentTicket[DatabaseObjects.Columns.TicketOwner] == DBNull.Value || String.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketOwner])))
                                        currentTicket[DatabaseObjects.Columns.TicketOwner] = reqTypeLoc.Owner;
                                }

                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.PRPGroup, thisList) &&
                                   request.PRPGroup != null && request.PRPGroup != "0" &&
                                   (this.Module.OwnerBindingChoice == TicketOwnerBinding.Auto.ToString()))
                                {
                                    currentTicket[DatabaseObjects.Columns.PRPGroup] = reqTypeLoc.PRPGroup;
                                }

                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketORP, thisList))
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketORP] = reqTypeLoc.ORP;
                                }
                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPRP, thisList))
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketPRP] = reqTypeLoc.PRPUser;
                                }
                            }
                            else
                            {

                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOwner, thisList))
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketOwner] = UGITUtility.ObjectToString(request.Owner);
                                }

                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.PRPGroup, thisList) &&
                                    request.PRPGroup != null && request.PRPGroup != "0" && (this.Module.OwnerBindingChoice == TicketOwnerBinding.Auto.ToString()))
                                {
                                    currentTicket[DatabaseObjects.Columns.PRPGroup] = request.PRPGroup;
                                }

                                if (!UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketORP) && !string.IsNullOrEmpty(request.ORP))
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketORP] = request.ORP;

                                }
                                if (!UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketPRP) && !string.IsNullOrEmpty(request.PRPUser))
                                {
                                    currentTicket[DatabaseObjects.Columns.TicketPRP] = request.PRPUser;

                                }
                            }

                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.FunctionalAreaLookup, thisList) && request.FunctionalAreaLookup != null)
                            {
                                currentTicket[DatabaseObjects.Columns.FunctionalAreaLookup] = request.FunctionalAreaLookup;
                            }

                            // Update PRP from PRP Group in Request Type By Location or from Request type IF not already assigned
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPRP, thisList) && string.IsNullOrEmpty(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.TicketPRP])))
                            {
                                UserProfile prpGroup = null;

                                // If PRP Group exists in request type by location table use it else use it from request type table only.
                                if (reqTypeLoc != null && reqTypeLoc.PRPGroup != null && reqTypeLoc.PRPGroup != "0")
                                    prpGroup = _context.UserManager.GetUserById(reqTypeLoc.PRPGroup);
                                else
                                    prpGroup = _context.UserManager.GetUserById(request.PRPGroup);

                                if (prpGroup != null && !string.IsNullOrWhiteSpace(prpGroup.Id))
                                {
                                    if (!prpGroup.isRole)
                                    {
                                        // If single user in PRPGroup field, assign it to PRP.
                                        currentTicket[DatabaseObjects.Columns.TicketPRP] = prpGroup; // users.userIds[0];
                                    }
                                    else if (request.AutoAssignPRP.HasValue)
                                    {

                                        // If AutoAssignPRP set to true, auto assign PRP from PRP group
                                        List<UserProfile> users = _context.UserManager.GetUsersByGroupID(prpGroup.Id);
                                        users = users.Where(x => !x.EnableOutofOffice || !(DateTime.Now.Date >= x.LeaveFromDate && DateTime.Now.Date <= x.LeaveToDate)).OrderBy(x => x.Name).ToList();


                                        if (users != null && users.Count == 1)
                                        {
                                            // If only one user in PRP Group, assign it to PRP
                                            //If user out of office not enabled only then assign user as PRP

                                            currentTicket[DatabaseObjects.Columns.TicketPRP] = users[0].Id;

                                        }
                                        else
                                        {
                                            // Else find user in group with least number of assigned tickets, and assign as PRP
                                            string prpUserID = "";

                                            List<ModuleUserStatistic> moduleUserStatsColl = objModuleUserStatisticsManager.Load(x => x.UserRole == "PRP" && x.IsActionUser && x.ModuleNameLookup == this.Module.ModuleName);
                                            //DataTable moduleUserStatsColl =  objModuleUserStatisticsManager.GetDataTable();
                                            // DataRow[] dr = moduleUserStatsColl.Where(x => x.Field<string>(DatabaseObjects.Columns.UserRole) == "PRP" && x.Field<string>(DatabaseObjects.Columns.IsActionUser) == "1" && x.Field<double>(DatabaseObjects.Columns.ModuleId) == Convert.ToDouble(this.Module.ID)).ToArray();

                                            var assignedUser = moduleUserStatsColl.GroupBy(x => x.UserName).Select(x => new { keyValue = x.Key, Count = x.Count() }).OrderBy(z => z.Count);
                                            var unassignedUsers = users.Select(x => x.Id).Except(assignedUser.Select(z => z.keyValue)).ToList();

                                            UserProfile user = null;

                                            if (unassignedUsers.Count == 0)
                                            {
                                                var prpGroupUsersOnly = assignedUser.Select(x => x.keyValue).Intersect(users.Select(x => x.Id));
                                                user = _context.UserManager.GetUserById(prpGroupUsersOnly.FirstOrDefault());
                                            }
                                            else
                                            {
                                                Random randomIndex = new Random();
                                                user = _context.UserManager.GetUserById(unassignedUsers[randomIndex.Next(0, unassignedUsers.Count - 1)]);
                                            }

                                            if (user != null)
                                                prpUserID = user.Id;
                                            else
                                                prpUserID = users[0].Id;

                                            if (!string.IsNullOrEmpty(prpUserID))
                                                currentTicket[DatabaseObjects.Columns.TicketPRP] = prpUserID;
                                        }
                                    }
                                }
                            }

                            // Update estimated hours if field is exist in ticket and requesttype
                            if (request.EstimatedHours > 0 &&
                                UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketEstimatedHours, thisList))
                            {
                                currentTicket[DatabaseObjects.Columns.TicketEstimatedHours] = request.EstimatedHours;
                            }

                            //if (request.TaskTemplateLookup != null && request.TaskTemplateLookup.ID > 0)
                            //{
                            //    OverrideAllTasks(request.ID, uHelper.StringToInt(currentTicket[DatabaseObjects.Columns.Id]));
                            //}
                            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.SLADisabled))
                                currentTicket[DatabaseObjects.Columns.SLADisabled] = request.SLADisabled;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set Default values for current stages
        /// </summary>
        /// <param name="item"></param>
        public void AssignModuleSpecificDefaults(DataRow item)
        {
            LifeCycleStage currentStage = GetTicketCurrentStage(item);
            if (currentStage != null)
            {
                AssignModuleSpecificDefaults(item, currentStage.StageStep);
            }
        }
        public void AssignModuleSpecificDefaults(DataRow item, int step)
        {
            LifeCycle lifeCycle = GetTicketLifeCycle(item);
            if (lifeCycle == null)
                return;

            LifeCycleStage stage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == step);
            if (stage == null)
                return;


            try
            {
                List<ModuleDefaultValue> moduleDefaultValues = this.Module.List_DefaultValues.Where(x => x.ModuleStepLookup != null && Convert.ToInt32(x.ModuleStepLookup) == stage.ID).ToList();

                foreach (ModuleDefaultValue moduleDefaultValue in moduleDefaultValues)
                {
                    string defaultKey = moduleDefaultValue.KeyName;
                    if (UGITUtility.IfColumnExists(defaultKey, item.Table))
                    {
                        bool updateValue = false;
                        if (Convert.ToString(item[defaultKey]) == string.Empty)
                            updateValue = true;
                        else if (moduleDefaultValue.Prop_Override.HasValue && moduleDefaultValue.Prop_Override.Value)
                        {
                            updateValue = true;
                        }

                        if (updateValue)
                        {
                            string defaultValue = Convert.ToString(moduleDefaultValue.KeyValue);
                            switch (defaultValue)
                            {
                                case "TodaysDate":
                                    item[defaultKey] = DateTime.Now.Date;
                                    break;
                                case "TomorrowsDate":
                                    {
                                        DateTime tmrwDate = DateTime.MinValue;
                                        //string UseCalendar = ConfigurationVariableHelper.GetConfigVariableValue(DatabaseObjects.Columns.UseCalendar);
                                        //string Holidays = ConfigurationVariableHelper.GetConfigVariableValue(DatabaseObjects.Columns.Holidays);
                                        //DateTime[] dates = uHelper.GetEndDateByWorkingDays(DateTime.Now, 2, UseCalendar, Holidays);
                                        DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context, DateTime.Now, 2);
                                        //if today is off then pick next start date otherwise pick end date
                                        if (dates[0].Date > DateTime.Now.Date)
                                            tmrwDate = dates[0];
                                        else
                                            tmrwDate = dates[1];

                                        item[defaultKey] = tmrwDate;
                                    }
                                    break;
                                case "LoggedInUser":
                                    {
                                        if (User != null)
                                            item[defaultKey] = string.Format("{0}", User.Id.ToString()); // string.Format("{0};#{1}", User.Id.ToString(), User.UserName);
                                    }
                                    break;
                                case "LoggedInUserLocation":
                                    {
                                        if (User != null)
                                            item[defaultKey] = User.Location; //string.Format("{0}", User.LocationId);  //string.Format("{0};#{1}", User.LocationId, User.Location);
                                    }
                                    break;
                                case "LoggedInUserDeskLocation":
                                    {
                                        if (User != null)
                                            item[defaultKey] = User.DeskLocation;
                                    }
                                    break;
                                case "RequestorLocation":
                                    {
                                        List<UserProfile> userList = _context.UserManager.GetUsersProfile();
                                        UserProfile requestor = userList.Where(x => x.Id == Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor])).FirstOrDefault(); //uHelper.GetUser(item, DatabaseObjects.Columns.TicketRequestor);
                                        if (requestor != null)
                                        {
                                            if (requestor != null)
                                                if (requestor.LocationId > 0)
                                                {
                                                    item[defaultKey] = string.Format("{0}", requestor.LocationId.ToString());  //string.Format("{0};#{1}", requestor.LocationId.ToString(), requestor.Location);
                                                }
                                                else
                                                {
                                                    item[defaultKey] = DBNull.Value;
                                                }
                                        }
                                    }
                                    break;
                                case "RequestorDeskLocation":
                                    {
                                        List<UserProfile> userList = _context.UserManager.GetUsersProfile();
                                        UserProfile requestor = userList.Where(x => x.Id == Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor])).FirstOrDefault(); //uHelper.GetUser(item, DatabaseObjects.Columns.TicketRequestor);
                                        if (requestor != null)
                                        {
                                            if (requestor != null)
                                                item[defaultKey] = requestor.DeskLocation;
                                        }
                                    }
                                    break;
                                case "LoggedInUserManager":
                                    {
                                        if (User != null && User.ManagerID != null)
                                            item[defaultKey] = string.Format("{0}", User.ManagerID.ToString());  // string.Format("{0};#{1}", User.Manager.ID.ToString(), User.Manager.Name);
                                    }
                                    break;
                                case "LoggedInUserManagerIfNotManager":
                                    {
                                        if (User != null)
                                        {
                                            if (Convert.ToBoolean(User.IsManager))
                                                item[defaultKey] = string.Format("{0}", User.Id.ToString());     //string.Format("{0};#{1}", _spWeb.CurrentUser.ID.ToString(), _spWeb.CurrentUser.Name);
                                            else if (User.ManagerID != null)
                                                item[defaultKey] = string.Format("{0}", User.ManagerID.ToString()); //string.Format("{0};#{1}", loggedInUserProfile.Manager.ID.ToString(), loggedInUserProfile.Manager.Name);
                                        }
                                    }
                                    break;
                                case "RequestorManager":
                                    {
                                        List<UserProfile> userList = _context.UserManager.GetUsersProfile();
                                        UserProfile requestor = userList.Where(x => x.Id == Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor])).FirstOrDefault();
                                        if (requestor != null)
                                        {
                                            if (requestor.ManagerID != null)
                                                item[defaultKey] = string.Format("{0}", requestor.ManagerID.ToString());
                                        }
                                    }
                                    break;
                                case "RequestorManagerIfNotManager":
                                    {
                                        List<UserProfile> userList = _context.UserManager.GetUsersProfile();
                                        UserProfile requestor = userList.Where(x => x.Id == Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor])).FirstOrDefault();
                                        if (requestor != null)
                                        {
                                            if (requestor != null)
                                            {
                                                if (Convert.ToBoolean(requestor.IsManager))
                                                    item[defaultKey] = string.Format("{0}", requestor.Id.ToString(), requestor.Name);
                                                else if (requestor.ManagerID != null)
                                                    item[defaultKey] = string.Format("{0}", requestor.ManagerID.ToString());  //, requestor.Manager.Name);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    item[defaultKey] = defaultValue;
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { ULog.WriteException(ex); }
        }
        public void AssignModuleSpecificDefaultUsers(DataRow item)
        {
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(ApplicationContext.Create());
            FieldConfiguration field = null;
            List<ModuleColumn> lstOfModuleColumns = this.Module.List_ModuleColumns;
            ModuleColumn moduleColumn = null;
            foreach (ModuleUserType moduleUserType in this.Module.List_ModuleUserTypes)
            {
                // made changes for: Create ACR is failing when default username is set with lookup value 'ACRTypeTitleLookup' under column
                // Issue was unable to create new ACR ticket if set with lookup default value 'ACRTypeTitleLookup' from admin field type

                field = fieldManager.GetFieldByFieldName(moduleUserType.ColumnName);
                if (lstOfModuleColumns != null)
                    moduleColumn = lstOfModuleColumns.FirstOrDefault(x => x.FieldName == moduleUserType.ColumnName);
                if (field != null && (field.Datatype == "UserField" || field.Datatype == "GroupField"))
                {
                    if (UGITUtility.IfColumnExists(moduleUserType.ColumnName, item.Table)
                    && moduleUserType.DefaultUser != null && !string.IsNullOrWhiteSpace(moduleUserType.DefaultUser))
                    {
                        item[moduleUserType.ColumnName] = moduleUserType.DefaultUser;
                    }
                }
                else if (moduleColumn != null && !string.IsNullOrEmpty(moduleColumn.ColumnType) && (moduleColumn.ColumnType == "UserField" || moduleColumn.ColumnType == "GroupField"))
                {
                    if (UGITUtility.IfColumnExists(moduleUserType.ColumnName, item.Table)
                    && moduleUserType.DefaultUser != null && !string.IsNullOrWhiteSpace(moduleUserType.DefaultUser))
                    {
                        item[moduleUserType.ColumnName] = moduleUserType.DefaultUser;
                    }
                }
            }
        }

        public void convertDataRowtoQuery(DataRow ticket)
        {
            //string column = "insert into TSR(";
            //string values = "(";
            foreach (var item in ticket.ItemArray)
            {

            }
        }

        public void LogStageTransition(DataRow currentTicket, LifeCycleStage preStage, LifeCycleStage newStage)
        {
            LogStageTransition(currentTicket, preStage, newStage, DateTime.Now, _context.CurrentUser);
        }

        public void LogStageTransition(DataRow currentTicket, LifeCycleStage preStage, LifeCycleStage newStage, DateTime stageEndDate, UserProfile actionUser)
        {
            // means there is no stage to log transition
            if (newStage == null)
                return;

            // No transition required in case of same step
            if (preStage != null && preStage.StageStep == newStage.StageStep)
                return;

            if (stageEndDate == DateTime.MinValue)
                stageEndDate = DateTime.Now;

            StoreBase<ModuleWorkflowHistory> store = new StoreBase<ModuleWorkflowHistory>(_context);
            DataTable moduleWorkflowHistory = UGITUtility.ToDataTable<ModuleWorkflowHistory>(store.Load());
            List<ModuleWorkflowHistory> historyCollection = store.Load().Where(x => x.TicketId == currentTicket[DatabaseObjects.Columns.TicketId].ToString()).OrderByDescending(x => x.ID).ToList();

            List<LifeCycleStage> possibleMiddleStages = new List<LifeCycleStage>();

            bool use24x7Calendar = uHelper.IsTicket24x7Enabled(_context, currentTicket);

            if (ticketActionType == TicketActionType.Rejected || ticketActionType == TicketActionType.Returned)
            {
                //reject /return must have previous stage
                if (preStage == null)
                    return;
            }
            else
            {
                //find out possible transitions
                if (preStage != null)
                {
                    LifeCycleStage nextStage = preStage.ApprovedStage;
                    if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.WorkflowSkipStages))
                    {
                        List<string> skipStages = UGITUtility.ConvertStringToList(Convert.ToString(currentTicket[DatabaseObjects.Columns.WorkflowSkipStages]), ",");
                        while (nextStage != null && nextStage.StageStep < newStage.StageStep)
                        {
                            if (!skipStages.Contains(nextStage.StageStep.ToString()))
                                possibleMiddleStages.Add(nextStage);
                            nextStage = nextStage.ApprovedStage;
                        }
                    }
                }
            }

            if (preStage != null)
            {
                // DataRow latestHistoryItem = null;
                ModuleWorkflowHistory latestHistoryItem = null;
                if (historyCollection.Count > 0)
                {
                    latestHistoryItem = historyCollection[0];
                    int hStageStep = UGITUtility.StringToInt(latestHistoryItem.StageStep);
                    DateTime stageStartDate = DateTime.MinValue;
                    DateTime.TryParse(Convert.ToString(latestHistoryItem.StageStartDate), out stageStartDate);
                    if (stageStartDate == DateTime.MinValue)
                    {
                        stageStartDate = Convert.ToDateTime(latestHistoryItem.StageStartDate);
                        latestHistoryItem.StageStartDate = stageStartDate;
                    }

                    if (hStageStep == preStage.StageStep)
                    {

                        latestHistoryItem.StageEndDate = stageEndDate;
                        double totalMinutes = uHelper.GetWorkingMinutesBetweenDates(_context, stageStartDate, stageEndDate, use24x7Calendar: use24x7Calendar, isSLA: true);
                        latestHistoryItem.Duration = Convert.ToInt32(totalMinutes);
                        latestHistoryItem.StageClosedBy = actionUser.Id;
                        latestHistoryItem.ActionUserType = string.Join(Constants.Separator, preStage.ActionUser);
                        latestHistoryItem.StageClosedByName = actionUser.Name;
                        workflowHistoryMgr.Update(latestHistoryItem);
                    }

                    #region stage transition calculate hold duration
                    if (hStageStep == preStage.StageStep)
                    {
                        latestHistoryItem.StageEndDate = stageEndDate;
                        double totalMinutes = 0;
                        double holdDuration = 0;
                        //For svc assigned stage, calculate total minutes based on their tasks completion date where sla is enable.
                        if (this.Module.ModuleName == ModuleNames.SVC && preStage.StageTypeChoice == StageType.Assigned.ToString())
                        {
                            string ticketID = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
                            UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                            List<UGITTask> dependents = uGITTaskManager.LoadByProjectID(Module.ModuleName, ticketID);
                            if (dependents != null && dependents.Count > 0)
                            {
                                dependents = dependents.Where(x => !x.SLADisabled && x.Status == "Completed").ToList();
                                List<Tuple<string, DateTime, DateTime>> holdSlots = new List<Tuple<string, DateTime, DateTime>>();
                                List<Tuple<string, DateTime, DateTime>> totalSlots = new List<Tuple<string, DateTime, DateTime>>();
                                foreach (UGITTask d in dependents)
                                {
                                    DateTime startDate = DateTime.MinValue;
                                    if (d.TaskActualStartDate.HasValue)
                                        startDate = d.TaskActualStartDate.Value;
                                    else if (d.StartDate != DateTime.MinValue)
                                        startDate = d.StartDate;

                                    uHelper.GetHoldUnHoldSlots(_context, d.ID.ToString(), ticketID, ref holdSlots);
                                    totalSlots.Add(new Tuple<string, DateTime, DateTime>(d.ID.ToString(), startDate, d.CompletionDate));
                                }

                                holdDuration = uHelper.GetTotalDurationByTimeSlot(_context, holdSlots, use24x7Calendar);
                                totalMinutes = uHelper.GetTotalDurationByTimeSlot(_context, totalSlots, use24x7Calendar);
                                latestHistoryItem.OnHoldDuration = UGITUtility.StringToInt(holdDuration);
                                latestHistoryItem.Duration = UGITUtility.StringToInt(totalMinutes);
                            }
                        }
                        else
                            totalMinutes = uHelper.GetWorkingMinutesBetweenDates(_context, stageStartDate, stageEndDate, use24x7Calendar: use24x7Calendar, isSLA: true);

                        latestHistoryItem.Duration = UGITUtility.StringToInt(totalMinutes);
                        latestHistoryItem.StageClosedBy = actionUser.Id;
                        latestHistoryItem.ActionUserType = preStage.ActionUser != null ? string.Join(Constants.Separator, preStage.ActionUser) : string.Empty;
                        latestHistoryItem.StageClosedByName = actionUser.Name;
                        workflowHistoryMgr.Update(latestHistoryItem);
                    }
                    #endregion
                }
            }

            // DataRow newHistory = null;
            ModuleWorkflowHistory newHistory = null;
            DateTime currentDate = DateTime.Now;
            foreach (LifeCycleStage middleStage in possibleMiddleStages)
            {
                newHistory = new ModuleWorkflowHistory();
                newHistory.Title = string.Format("{0} {1}", currentTicket[DatabaseObjects.Columns.TicketId], middleStage.StageStep);
                newHistory.TicketId = currentTicket[DatabaseObjects.Columns.TicketId].ToString();
                newHistory.ModuleNameLookup = this.Module.ModuleName;
                newHistory.StageStep = middleStage.StageStep;
                DateTime creationDate = DateTime.MinValue;
                newHistory.StageStartDate = stageEndDate;
                newHistory.StageEndDate = stageEndDate;
                newHistory.Duration = 0;
                newHistory.OnHoldDuration = 0;
                newHistory.StageClosedBy = actionUser.Id; // _spWeb.CurrentUser.ID;
                newHistory.ActionUserType = string.Join(Constants.Separator, middleStage.ActionUser);
                newHistory.StageClosedByName = actionUser.Name; ;
                workflowHistoryMgr.Insert(newHistory);
            }

            //new stage
            newHistory = new ModuleWorkflowHistory();
            newHistory.TicketId = currentTicket[DatabaseObjects.Columns.TicketId].ToString();
            newHistory.ModuleNameLookup = Module.ModuleName;
            newHistory.StageStep = newStage.StageStep;
            newHistory.StageStartDate = stageEndDate;
            newHistory.StageEndDate = null;
            //if (newStage.StageTypeChoice == StageType.Closed.ToString())
            //{
            newHistory.StageEndDate = stageEndDate;
            newHistory.StageClosedBy = actionUser.Id;
            newHistory.ActionUserType = string.Join(Constants.Separator, newStage.ActionUser);
            newHistory.StageClosedByName = actionUser.Name;
            //}
            newHistory.Title = string.Format("{0} {1}", currentTicket[DatabaseObjects.Columns.TicketId], newStage.StageStep);
            newHistory.Duration = 0;
            newHistory.OnHoldDuration = 0;
            workflowHistoryMgr.Insert(newHistory);

        }

        public bool SetTicketSkipStages(DataRow currentTicket)
        {
            bool ticketUpdated = false;

            string currentTicketSkipStages = string.Empty;
            LifeCycle lifeCycle = GetTicketLifeCycle(currentTicket);
            List<string> stagesIds = new List<string>();
            foreach (LifeCycleStage iterativeStage in lifeCycle.Stages)
            {
                if (!string.IsNullOrEmpty(iterativeStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(_context, iterativeStage.SkipOnCondition, currentTicket))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(iterativeStage.StageStep)))
                    {
                        stagesIds.Add(Convert.ToString(iterativeStage.StageStep));
                    }
                }
            }

            if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.WorkflowSkipStages))
            {
                currentTicket[DatabaseObjects.Columns.WorkflowSkipStages] = string.Join(",", stagesIds.ToArray());
                ticketUpdated = true;
            }
            return ticketUpdated;
        }

        /// <summary>
        /// This function Approves the current ticket.
        /// </summary>
        /// <param name="thisWeb"></param>
        /// <param name="moduleId"></param>
        /// <param name="currentTicket"></param>
        /// <returns></returns>
        public string Approve(UserProfile user, DataRow currentTicket)
        {
            return Approve(user, currentTicket, false);
        }
        public string Approve(UserProfile asUser, DataRow currentTicket, bool asAdmin, bool autoStageMove = false)
        {
            if (asUser == null)
                asUser = _context.CurrentUser;


            ticketActionType = TicketActionType.Approved;
            string moduleName;
            int stageStep = 0;
            if (Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.StageStep)) != string.Empty)
                stageStep = UGITUtility.StringToInt(currentTicket[DatabaseObjects.Columns.StageStep]);

            moduleName = this.Module.ModuleName;
            string errorMessage = string.Empty;
            string currentStageActionUserType = string.Empty;
            LifeCycleStage currentStage = GetTicketCurrentStage(currentTicket);

            bool isPMOStage = false;
            if (moduleName == "NPR" && currentStage.Prop_PMOReview.HasValue && currentStage.Prop_PMOReview.Value)
                isPMOStage = UGITUtility.StringToBoolean(currentStage.Prop_PMOReview.Value);


            string ticketActionUserTypes = string.Empty;
            string ticketStageActionUserTypes = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
            string[] currentStageActionUserTypes = UGITUtility.SplitString(ticketStageActionUserTypes, Constants.Separator);

            List<string> currentUserActionUserTypes = new List<string>();
            foreach (string currentStageActionUserType1 in currentStageActionUserTypes)
            {
                if (currentStageActionUserType1 != string.Empty)
                {
                    if (_context.UserManager.IsUserPresentInField(asUser, currentTicket, currentStageActionUserType1, true))
                    {
                        currentUserActionUserTypes.Add(currentStageActionUserType1);
                    }
                }
            }

            // Do we need approval from all ActionUser? Or is the approval done as a SuperAdmin
            bool allApprovalsRequired = currentStage.StageAllApprovalsRequired;
            if (!allApprovalsRequired || asAdmin)
                ticketStageActionUserTypes = string.Empty;

            if (ticketStageActionUserTypes != string.Empty && currentUserActionUserTypes.Count > 0)
            {
                // See if we still need approvals AND approvers are configured in the stage
                List<string> actionUserTypes = new List<string>(currentStageActionUserTypes);
                actionUserTypes = actionUserTypes.Except(currentUserActionUserTypes).ToList();
                string remainingActionUsers = string.Empty;
                if (actionUserTypes.Count == 0)
                    ticketStageActionUserTypes = string.Empty;
                else
                {
                    ticketStageActionUserTypes = string.Join(Constants.Separator, actionUserTypes);
                    remainingActionUsers = uHelper.GetUsersAsString(_context, ticketStageActionUserTypes, currentTicket);
                }

                currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = ticketStageActionUserTypes;
                currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = remainingActionUsers;

                if (string.IsNullOrEmpty(remainingActionUsers))
                    ticketStageActionUserTypes = string.Empty;

                if (!string.IsNullOrWhiteSpace(ticketStageActionUserTypes))
                {
                    // Write history
                    LifeCycleStage currentStageObj = this.GetTicketCurrentStage(currentTicket);
                    string historyDescription = currentStageObj.ApproveActionDescription.Trim();
                    if (string.IsNullOrEmpty(historyDescription))
                        historyDescription = "Approved";
                    uHelper.CreateHistory(asUser, historyDescription, currentTicket, _context);
                }
            }

            if (currentStage.StageApprovedStatus > 0 && (ticketStageActionUserTypes == string.Empty || ticketStageActionUserTypes == Constants.Separator))
            {
                if (isPMOStage && _context.ConfigManager.GetValueAsBool(ConfigConstants.NPRBudgetMandatory))
                {
                    ModuleBudgetManager budgetMgr = new ModuleBudgetManager(_context);
                    List<ModuleBudget> budgets = budgetMgr.Load(x => x.ModuleName == this.Module.ModuleName && x.TicketId == Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                    if (budgets == null || budgets.Count <= 0)
                    {
                        currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = ticketActionUserTypes;
                        errorMessage = Constants.ErrorMsgBudgetItemRequired;
                        return errorMessage;
                    }
                }


                List<LifeCycleStage> lifeCycleStages = GetCurrentLifeCyleStages(currentTicket);
                LifeCycleStage approvedStage = lifeCycleStages.FirstOrDefault(x => x.StageStep == currentStage.StageApprovedStatus);
                // Write history
                string historyDescription = currentStage.ApproveActionDescription;
                if (string.IsNullOrEmpty(historyDescription))
                    historyDescription = currentStage.StageTitle + " => " + approvedStage.StageTitle;

                if (autoStageMove)
                {
                    historyDescription += " (Auto Stage Move)";
                    uHelper.CreateHistory(asUser, historyDescription, currentTicket, true, _context);
                }
                else
                {
                    uHelper.CreateHistory(asUser, historyDescription, currentTicket, false, _context);
                }

                //uHelper.CreateHistory(user, historyDescription, currentTicket, true);
                currentTicket[DatabaseObjects.Columns.TicketStatus] = approvedStage.StageTitle;
                currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = GetStageActionUsers(currentStage.StageApprovedStatus);
                currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes].ToString(), currentTicket);
                currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.UtcNow.ToString();

                LifeCycleStage cycleNextStage = SetNextStage(currentTicket);
                if (moduleName == "SVC" && cycleNextStage != null && cycleNextStage.StageTypeChoice == StageType.Assigned.ToString())
                {

                    // Get tasks out of Waiting status
                    CommitChanges(currentTicket, stopUpdateDependencies: true, donotUpdateEscalations: true);
                    UGITTaskManager taskManager = new UGITTaskManager(_context);
                    taskManager.StartTasks(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                    taskManager.UpdateSVCTicket(currentTicket, false);
                }
            }

            return errorMessage;
        }

        private string GetStageActionUsers(int id)
        {
            LifeCycle defaultLifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            if (defaultLifeCycle == null)
                return string.Empty;
            LifeCycleStage stage = null;
            if (id > 0)
            {
                stage = defaultLifeCycle.Stages.FirstOrDefault(x => x.StageStep == id);
            }
            else { stage = new LifeCycleStage(); }
            if (stage == null)
                return string.Empty;
            if (string.IsNullOrEmpty(stage.ActionUser))
                return string.Empty;

            return string.Join(Constants.Separator, stage.ActionUser.Trim());
        }
        private string GetStageActionUsers(string stageName)
        {
            LifeCycle defaultLifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            if (defaultLifeCycle == null)
                return string.Empty;
            LifeCycleStage stage = null;
            if (!string.IsNullOrEmpty(stageName))
            {
                stage = defaultLifeCycle.Stages.FirstOrDefault(x => x.Name != null && x.Name.ToLower() == stageName.ToLower());
            }
            else { stage = new LifeCycleStage(); }
            if (stage == null)
                return string.Empty;

            return string.Join(Constants.Separator, stage.ActionUser);
        }

        public string GetStageActionUsers(string stageName, string moduleName)
        {
            DataTable ticketStages;
            LifeCycleManager lifeCycleHelper = new Manager.LifeCycleManager(_context);
            List<LifeCycle> lifecyles = lifeCycleHelper.LoadLifeCycleByModule(moduleName);
            if (lifecyles != null && lifecyles[0].Stages.Count > 0)
            {
                ticketStages = UGITUtility.ToDataTable<LifeCycleStage>(lifecyles[0].Stages);
                foreach (DataRow stage in ticketStages.Rows)
                {
                    if (Convert.ToString(stage[DatabaseObjects.Columns.StageTitle]) == stageName)
                        return Convert.ToString(stage[DatabaseObjects.Columns.ActionUser]);
                }
            }

            return string.Empty;
        }

        private string GetStageDataEditors(string stageName)
        {
            LifeCycle defaultLifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            if (defaultLifeCycle == null)
                return string.Empty;

            LifeCycleStage stage = defaultLifeCycle.Stages.FirstOrDefault(x => x.Name.ToLower() == stageName.ToLower());
            if (string.IsNullOrWhiteSpace(stage.DataEditors))
                return string.Empty;

            return string.Join(Constants.Separator, stage.DataEditors);
        }
        private string GetStageDataEditors(int stageStep)
        {
            LifeCycle defaultLifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            if (defaultLifeCycle == null)
                return string.Empty;

            LifeCycleStage stage = defaultLifeCycle.Stages.FirstOrDefault(x => x.StageStep == stageStep);
            if (string.IsNullOrWhiteSpace(stage.DataEditors))
                return string.Empty;

            return string.Join(Constants.Separator, stage.DataEditors);
        }

        public LifeCycleStage SetNextStage(DataRow saveTicket)
        {
            return SetNextStage(saveTicket, User);
        }
        public LifeCycleStage SetNextStage(DataRow saveTicket, UserProfile actionUser)
        {
            LifeCycleStage cycleNextStage = null;
            LifeCycle lifeCycle = GetTicketLifeCycle(saveTicket);

            if (lifeCycle == null && lifeCycle.Stages.Count <= 0)
            {
                return null;
            }

            //Get the current stage "STEP"
            LifeCycleStage currentStage = GetTicketCurrentStage(saveTicket);


            CheckRequestType(saveTicket, false);

            //Check Skip Conditions and see if we want to Skip the default next stage.
            //This should work even if auto approve is off
            if (currentStage != null && currentStage.ApprovedStage != null)
                cycleNextStage = currentStage.ApprovedStage;

            // Check module has AutoApprove enabled
            Boolean moduleAutoApprove = this.Module.ModuleAutoApprove;

            bool skipNextStage = false;
            if (cycleNextStage != null && !string.IsNullOrEmpty(cycleNextStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(_context, cycleNextStage.SkipOnCondition, saveTicket))
                skipNextStage = true;

            // Move ticket forward automatically if we have AutoApprove ON for the current module in Modules table or we need to skip the next stage.
            if (moduleAutoApprove || skipNextStage)
            {
                this.CommitChanges(saveTicket, donotUpdateEscalations: true, stopUpdateDependencies: true, actionUser: actionUser);

                // Start checking from the stage immediately AFTER the current stage
                LifeCycleStage previousStage = currentStage;
                foreach (LifeCycleStage iterativeStage in lifeCycle.Stages)
                {
                    if (iterativeStage.StageStep < currentStage.StageStep + 1)
                    {
                        continue;
                    }
                    // Can auto-approve this step only if both module AND stage have auto-approve enabled
                    bool stageAutoApprove = moduleAutoApprove;
                    //Check Skip Conditions and see if we want to SkipCurrentStage
                    bool skipCurrentStage = false;
                    if (iterativeStage.Prop_AutoApprove.HasValue)
                        stageAutoApprove = iterativeStage.Prop_AutoApprove.Value;
                    if (iterativeStage.DisableAutoApprove.HasValue && iterativeStage.DisableAutoApprove.Value)
                        stageAutoApprove = false;

                    //Check Skip Conditions and see if we want to SkipCurrentStage
                    if (!string.IsNullOrEmpty(iterativeStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(_context, iterativeStage.SkipOnCondition, saveTicket))
                    {
                        skipCurrentStage = true;
                    }

                    if ((iterativeStage.StageStep >= currentStage.StageStep && (stageAutoApprove || skipCurrentStage)) || iterativeStage.StageStep == currentStage.StageStep)
                    {
                        //Get all mandatory fields for this stage.
                        ModuleRoleWriteAccess[] roleWriteRows = this.Module.List_RoleWriteAccess.Where(x => x.StageStep == iterativeStage.StageStep && x.FieldMandatory).ToArray();

                        //Even if we have one mandatory field, we cannot AutoApprove it.
                        bool stageHasMandatoryFields = false;

                        LifeCycleStage nextStage = lifeCycle.Stages.FirstOrDefault(x => iterativeStage.ApprovedStage != null && x.ID == iterativeStage.ApprovedStage.ID);

                        if (roleWriteRows.Length >= 1)
                        {
                            foreach (ModuleRoleWriteAccess roleWriteRow in roleWriteRows)
                            {
                                string columnName = roleWriteRow.FieldName;
                                bool dependentField = UGITUtility.IsDependentMandatoryField(moduleName, columnName);
                                if (UGITUtility.IfColumnExists(columnName, saveTicket.Table) && !dependentField)
                                {
                                    if (Convert.ToString(saveTicket[columnName]) == string.Empty)
                                    {
                                        if (!HasModuleSpecificDefault(saveTicket, iterativeStage.ID, columnName))
                                        {
                                            stageHasMandatoryFields = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //Set variables for the current stage.
                        bool valid = false, overallValid = true;

                        ////Get current stage Action User types. Eg: TicketPRP
                        string[] currentStageActionUserTypes = UGITUtility.SplitString(iterativeStage.ActionUser, Constants.Separator);
                        string[] iterativeStageActionUser = UGITUtility.SplitString(iterativeStage.ActionUser, Constants.Separator);
                        string[] previousStageActionUser = UGITUtility.SplitString(previousStage.ActionUser, Constants.Separator5);


                        //Check if current stage ActionUserTypes match the previous stage ActionUserTypes
                        //If yes then we can auto approve it.
                        if (stageAutoApprove && !stageHasMandatoryFields &&
                            iterativeStage != null && iterativeStageActionUser.Length > 0 &&
                            previousStage != null && previousStageActionUser.Length > 0)
                        {

                            SetTicketStage(saveTicket, iterativeStage);
                            //Set current stage action users & userTypes in the current ticket

                            // Do we need approval from all the users?
                            bool allApprovalsRequired = iterativeStage.StageAllApprovalsRequired;
                            //SpDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                            //Get currentStage action User

                            List<string> currentUserActionUserTypes = new List<string>();
                            foreach (string actionUserType in currentStageActionUserTypes)
                            {
                                if (_context.UserManager.IsUserPresentInField(actionUser, saveTicket, actionUserType, true))
                                    currentUserActionUserTypes.Add(actionUserType);
                            }
                            //
                            //Loop though all UserTypes in current stage. Example :  TicketPRP;#TicketOwner
                            //Start Commented SPDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                            //foreach (string currentStageActionUserType in currentStageActionUserTypes)
                            //{
                            //Get currentStage action User
                            //valid = _context.UserManager.IsUserPresentInField(actionUser, saveTicket, currentStageActionUserType);
                            //End Commented SPDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                            if (allApprovalsRequired)
                            {
                                //Need to do an AND opertaion here, Need to match all users from prev stage to all users of this stage

                                //SPDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                                //overallValid = valid & overallValid;//Commented SPDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                                List<string> actionUserTypes = new List<string>(currentStageActionUserTypes);
                                actionUserTypes = actionUserTypes.Except(currentUserActionUserTypes).ToList();
                                string remainingActionUsers = uHelper.GetUsersAsString(_context, string.Join(Constants.Separator, actionUserTypes), saveTicket);

                                // Approve ticket if there is no remaining action user type or no users present in remaining onces.
                                if (actionUserTypes.Count == 0 || string.IsNullOrWhiteSpace(remainingActionUsers))
                                    valid = true;
                                else
                                {
                                    overallValid = false;

                                    //Clone is required so that cache object would not be impacted.
                                    cycleNextStage = cycleNextStage.Clone() as LifeCycleStage;
                                    cycleNextStage.ActionUser = iterativeStage.ActionUser;

                                    // Log to history if user approve ticket but it still in waiting for other approver to approve.
                                    if (actionUserTypes.Count < currentStageActionUserTypes.Length)
                                    {
                                        string historyDescription;
                                        string approveActionDescription = iterativeStage.ApproveActionDescription;
                                        if (string.IsNullOrEmpty(approveActionDescription))
                                            historyDescription = previousStage.Name + " => " + iterativeStage.ApprovedStage.Name + " (Auto-Approve)";
                                        else
                                            historyDescription = approveActionDescription + " (Auto-Approve)";

                                        //Write History description here - this gets displayed in the History tab
                                        uHelper.CreateHistory(actionUser, historyDescription, saveTicket, false, _context);

                                        //Also create entry in event log to track approval commented SPDelta 13(Fix for auto-approvals not working with "All Approvals Required" is set)
                                        TicketEventManager eventHelper = new TicketEventManager(_context, Module.ModuleName, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                                        eventHelper.LogEvent(Constants.TicketEventType.Approved, iterativeStage, user: actionUser);
                                    }
                                }

                                //End Spdelta 13(Fix for auto-approvals not working with "All Approvals Required" is set).
                            }
                            else
                            {
                                if (currentUserActionUserTypes.Count > 0)
                                    valid = true;
                            }
                        }

                        if (!stageHasMandatoryFields && !valid && iterativeStage.Prop_DoNotWaitForActionUser.HasValue)
                        {
                            valid = iterativeStage.Prop_DoNotWaitForActionUser.Value;
                        }

                        // Check if we need to skip the current stage
                        if (skipCurrentStage && nextStage != null)
                        {
                            //Skipping logic goes here, assign stage specific defaults
                            cycleNextStage = nextStage;

                            SetTicketStage(saveTicket, iterativeStage);

                            //Assign stage defaults since we are going through this stage
                            AssignModuleSpecificDefaults(saveTicket);
                            previousStage = iterativeStage;

                            string historyDescription = iterativeStage.Name + " (Skipped)";
                            uHelper.CreateHistory(actionUser, historyDescription, saveTicket, false, _context);

                            //Add in notification send stages list to send notification when all the notificaiton going out
                            skipStageNotificationStages.Add(iterativeStage);

                            //Skip the logic that follows and move the loop forward
                            continue;
                        }

                        // Check if we need to auto-approve this stage
                        if (stageAutoApprove)
                        {
                            if (overallValid && valid)
                            {
                                //We enter here if we Auto Approve the current stage
                                //Change the current ticket stage step by step and not skip.
                                if (nextStage != null)
                                    cycleNextStage = nextStage;
                                else
                                    cycleNextStage = nextStage = iterativeStage;


                                if (lifeCycle.ID == 0)
                                    saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = iterativeStage.ID;

                                saveTicket[DatabaseObjects.Columns.TicketStatus] = iterativeStage.Name;

                                //Assign new stage defaults - Even if this stage gets skipped in next iteration
                                AssignModuleSpecificDefaults(saveTicket);

                                if (iterativeStage.ApprovedStage != null)
                                {
                                    // Assign stage defaults since we are going to this stage
                                    AssignModuleSpecificDefaults(saveTicket, cycleNextStage.StageStep);
                                    SetTicketStage(saveTicket, iterativeStage.ApprovedStage);
                                    if (moduleName == "SVC" && cycleNextStage != null && cycleNextStage.StageTypeChoice == StageType.Assigned.ToString())
                                    {

                                        // Get tasks out of Waiting status
                                        //CommitChanges(saveTicket, stopUpdateDependencies: true, donotUpdateEscalations: true);
                                        UGITTaskManager taskManager = new UGITTaskManager(_context);
                                        taskManager.StartTasks(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                                        taskManager.UpdateSVCTicket(saveTicket, false);
                                    }
                                    //Log stage movement with timestamps
                                    string historyDescription;
                                    //**LogStageTransition(saveTicket, iterativeStage, iterativeStage.ApprovedStage, actionUserTypeOfCurrentUser);

                                    string approveActionDescription = iterativeStage.ApproveActionDescription;
                                    if (string.IsNullOrEmpty(approveActionDescription))
                                        historyDescription = previousStage.Name + " => " + iterativeStage.ApprovedStage.Name + " (Auto-Approve)";
                                    else
                                        historyDescription = approveActionDescription + " (Auto-Approve)";

                                    //Write History description here - this gets displayed in the History tab
                                    uHelper.CreateHistory(actionUser, historyDescription, saveTicket, false, _context);
                                    skipStageNotificationStages.Add(iterativeStage);
                                }
                            }
                            else
                            {
                                // We enter here if we dont auto approve this stage, so break out of the loop, no point in checking subsequent stages
                                break;
                            }
                        }
                        previousStage = iterativeStage;
                    }

                    // Also break out if at auto-approve is false for any of the stages we are cycling through
                    if (iterativeStage.StageStep > currentStage.StageStep)
                    {
                        if (!stageAutoApprove)
                            break;
                    }

                }
            }

            if (cycleNextStage != null)
            {
                // Set the next stage
                if (lifeCycle.ID == 0)
                    saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = cycleNextStage.ID;

                // Assign stage defaults since we are going to this stage
                AssignModuleSpecificDefaults(saveTicket, cycleNextStage.StageStep);
                SetTicketStage(saveTicket, cycleNextStage);
            }

            // If we are moving to last (i.e. Closed) stage of ticket, then set TicketCloseDate field
            if (cycleNextStage != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, saveTicket.Table))
            {
                LifeCycleStage closeStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());
                if (closeStage != null)
                {
                    if (closeStage.StageStep == cycleNextStage.StageStep)
                    {
                        MarkTicketClosed(saveTicket);
                    }
                    else
                    {
                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, saveTicket.Table))
                            saveTicket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, saveTicket.Table))
                            saveTicket[DatabaseObjects.Columns.TicketClosed] = 0;
                    }
                }
            }

            if (cycleNextStage == null)
                return null;
            else
                return cycleNextStage;

        }

        /// <summary>
        /// Method to set ticket stage and stage action users for current ticket
        /// </summary>
        /// <param name="saveTicket"></param>
        /// <param name="stage"></param>
        private void SetTicketStage(DataRow saveTicket, LifeCycleStage stage)
        {
            saveTicket[DatabaseObjects.Columns.StageStep] = stage.StageStep;
            saveTicket[DatabaseObjects.Columns.TicketStatus] = stage.Name;
            saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = stage.ActionUser;
            saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), saveTicket);
            saveTicket[DatabaseObjects.Columns.DataEditors] = stage.DataEditors;
        }

        private void MarkTicketClosed(DataRow currentTicket)
        {
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketActualCompletionDate, currentTicket.Table))
            {
                // Only overwrite actual completion date if not already set by user
                DateTime actualCompletionDate = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.TicketActualCompletionDate]);
                if (actualCompletionDate == DateTime.MinValue)
                    currentTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = DateTime.Now.Date;
            }
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPctComplete, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.TicketPctComplete] = 100;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CloseDate, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.CloseDate] = DateTime.Now;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ClosedDateOnly, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.ClosedDateOnly] = DateTime.Now;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Closed, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.Closed] = 1;

            // Calculate TicketAge
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketAge, currentTicket.Table))
            {
                int workingHourInADay = uHelper.GetWorkingHoursInADay(_context, true);
                bool ticketAgeExcludesHoldTime = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.TicketAgeExcludesHoldTime);
                currentTicket[DatabaseObjects.Columns.TicketAge] = GetTicketAge(_context, currentTicket, workingHourInADay, ticketAgeExcludesHoldTime);
            }
        }
        public static void MarkTicketClosed(ApplicationContext _context, DataRow currentTicket)
        {
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketActualCompletionDate, currentTicket.Table))
            {
                // Only overwrite actual completion date if not already set by user
                DateTime actualCompletionDate = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.TicketActualCompletionDate]);
                if (actualCompletionDate == DateTime.MinValue)
                    currentTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = DateTime.Now.Date;
            }
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPctComplete, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.TicketPctComplete] = 100;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CloseDate, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.CloseDate] = DateTime.Now;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ClosedDateOnly, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.ClosedDateOnly] = DateTime.Now;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Closed, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.Closed] = 1;

            // Calculate TicketAge
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketAge, currentTicket.Table))
            {
                int workingHourInADay = uHelper.GetWorkingHoursInADay(_context, true);
                bool ticketAgeExcludesHoldTime = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.TicketAgeExcludesHoldTime);
                currentTicket[DatabaseObjects.Columns.TicketAge] = GetTicketAge(_context, currentTicket, workingHourInADay, ticketAgeExcludesHoldTime);
            }
        }

        public bool HasModuleSpecificDefault(DataRow item, long stageId, string columnName)
        {
            List<ModuleDefaultValue> moduleDefaultValues = Module.List_DefaultValues.Where(x => x.KeyName.ToLower() == columnName.ToLower() && x.ModuleStepLookup != null && x.ModuleStepLookup == Convert.ToString(stageId)).ToList();
            if (moduleDefaultValues.Count > 0)
                return true;
            return false;
        }

        public void Reject(DataRow currentTicket, string comment, UserProfile asUser = null)
        {
            if (asUser == null)
                asUser = _context.CurrentUser;
            ticketActionType = TicketActionType.Rejected;
            LifeCycleStage currentStage = GetTicketCurrentStage(currentTicket);

            if (currentStage.RejectStage != null)
            {
                LifeCycleStage rejectStage = currentStage.RejectStage;

                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketComment, currentTicket.Table) && !string.IsNullOrWhiteSpace(comment))
                    currentTicket[DatabaseObjects.Columns.TicketComment] = UGITUtility.GetVersionString(asUser.Id, comment, currentTicket, DatabaseObjects.Columns.TicketComment);

                string historyDescription = currentStage.RejectActionDescription;
                if (string.IsNullOrEmpty(historyDescription))
                    historyDescription = currentStage.Name + " => " + currentStage.RejectStage.Name;

                string currentStep = string.Empty;
                DataRow[] currentStepList = null;
                if (currentStepList != null)
                {
                    currentStepList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.Id + "=" + currentStage.ID).Select();
                }
                else
                {
                    DataTable dtTemp = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"TenantID='{_context.TenantID}'");
                    currentStepList = dtTemp.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, currentStage.ID));
                }

                if (rejectStage != null)
                    currentStep = Convert.ToString(rejectStage.StageStep);

                string rejectActionDescription = currentStage.RejectActionDescription;
                if (string.IsNullOrEmpty(rejectActionDescription) || rejectActionDescription.IndexOf(" ") != -1)
                    rejectActionDescription = "Cancelled";


                if (currentStep == "1" && IsQuickClose(currentTicket))
                {
                    currentTicket[DatabaseObjects.Columns.TicketStatus] = rejectStage.Name;
                    historyDescription += " (Quick Close)";
                }
                else
                {
                    currentTicket[DatabaseObjects.Columns.TicketStatus] = rejectActionDescription;
                    historyDescription += " (Cancelled)";
                }

                uHelper.CreateHistory(asUser, historyDescription, currentTicket, _context);

                currentTicket[DatabaseObjects.Columns.ModuleStepLookup] = rejectStage.ID;
                currentTicket[DatabaseObjects.Columns.StageStep] = rejectStage.StageStep;
                if (!string.IsNullOrWhiteSpace(rejectStage.StageTypeChoice) && rejectStage.StageTypeChoice.Equals(StageType.Closed.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    MarkTicketClosed(currentTicket);
                }
                else
                {
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, currentTicket.Table))
                        currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, currentTicket.Table))
                        currentTicket[DatabaseObjects.Columns.TicketClosed] = 0;
                }

                currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Join(Constants.Separator, rejectStage.ActionUser);
                currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket);
                currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now.ToString();
            }
        }

        public void Return(int moduleId, DataRow currentTicket, string comment)
        {
            Return(moduleId, currentTicket, comment, true, null);
        }

        public void Return(int moduleId, DataRow currentTicket, string comment, bool changeStatus, int? returnStep)
        {
            ticketActionType = TicketActionType.Returned;
            LifeCycle lifeCycle = GetTicketLifeCycle(currentTicket);
            LifeCycleStage currentStage = GetTicketCurrentStage(currentTicket);
            LifeCycleStage closedStage = GetTicketCloseStage(lifeCycle);

            if (currentStage == null)
                return;

            LifeCycleStage returnStage = currentStage.ReturnStage;
            if (returnStep.HasValue && returnStep.Value > 0)
            {
                returnStage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == returnStep);
            }

            if (returnStage == null)
                return;

            string historyDescription = currentStage.ReturnActionDescription;
            if (string.IsNullOrEmpty(historyDescription))
                historyDescription = currentStage.Name + " => " + returnStage.Name + " (Returned)";
            uHelper.CreateHistory(User, historyDescription, currentTicket, _context);

            if (!changeStatus || currentStage.StageTypeChoice == "Closed")
                currentTicket[DatabaseObjects.Columns.TicketStatus] = returnStage.Name;
            else
                currentTicket[DatabaseObjects.Columns.TicketStatus] = "Returned";

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketComment, currentTicket.Table) && !string.IsNullOrWhiteSpace(comment))
                currentTicket[DatabaseObjects.Columns.TicketComment] = UGITUtility.GetVersionString(User.Id, comment, currentTicket, DatabaseObjects.Columns.TicketComment);

            currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = GetStageActionUsers(returnStage.Name);
            currentTicket[DatabaseObjects.Columns.ModuleStepLookup] = returnStage.ID;
            currentTicket[DatabaseObjects.Columns.StageStep] = returnStage.StageStep;
            currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket);// Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
            currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now.ToString();

            if (closedStage.StageStep == returnStage.StageStep)
            {
                MarkTicketClosed(currentTicket);
            }
            else
            {
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, currentTicket.Table))
                    currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, currentTicket.Table))
                    currentTicket[DatabaseObjects.Columns.TicketClosed] = 0;
            }
        }

        public LifeCycleStage GetTicketCloseStage(DataRow saveTicket)
        {
            LifeCycle lifeCycle = GetTicketLifeCycle(saveTicket);
            if (lifeCycle == null || lifeCycle.Stages == null || lifeCycle.Stages.Count <= 0)
            {
                return null;
            }
            return GetTicketCloseStage(lifeCycle);
        }

        public LifeCycleStage GetTicketCloseStage(LifeCycle ticketLifeCycle)
        {
            if (ticketLifeCycle == null || ticketLifeCycle.Stages == null || ticketLifeCycle.Stages.Count <= 0)
            {
                return null;
            }
            return ticketLifeCycle.Stages.Last();
        }

        public LifeCycleStage GetTicketAwardStage(LifeCycle ticketLifeCycle)
        {
            if (ticketLifeCycle == null || ticketLifeCycle.Stages == null || ticketLifeCycle.Stages.Count <= 0)
            {
                return null;
            }
            return ticketLifeCycle.Stages.FirstOrDefault(x => !string.IsNullOrEmpty(x.CustomProperties) && x.CustomProperties.ToLower().Contains("awardstage"));
        }

        private void UpdateEscalation(ApplicationContext context, DataRow item)
        {
            //Update escalations of ticket
            if (item.Table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
            {
                EscalationProcess objEscalationProcess = new EscalationProcess(context);
                objEscalationProcess.GenerateEscalationInBackground(this, Convert.ToString(item[DatabaseObjects.Columns.TicketId]), this.Module.ID, true);
            }
        }

        public void QuickClose(long moduleId, DataRow currentTicket, string buttonId, string performedActionName = "")
        {
            LifeCycle lifeCycle = GetTicketLifeCycle(currentTicket);
            LifeCycleStage firstStage = lifeCycle.Stages.OrderBy(x => x.StageStep).FirstOrDefault();

            bool isQuickClose = IsQuickClose(currentTicket);
            if (isQuickClose || buttonId == "saveAsDraftButton" || buttonId == "quickCloseTicket")
            {
                if (isQuickClose && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketDesiredCompletionDate, currentTicket.Table))
                    currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = DateTime.Now.ToString();

                if (buttonId == "saveAsDraftButton")
                {
                    currentTicket[DatabaseObjects.Columns.TicketStatus] = firstStage.Name;
                    currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = firstStage.ActionUser;
                    currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = User.Id;
                    currentTicket[DatabaseObjects.Columns.StageStep] = firstStage.StageStep;
                    if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
                        currentTicket[DatabaseObjects.Columns.ModuleStepLookup] = firstStage.ID;

                    //No Email sent in this case
                    SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, UGITUtility.ObjectToString(moduleId), performedAction: performedActionName);
                }
                else// QuickClose
                {

                    LifeCycleStage closeStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());

                    if (closeStage != null)
                    {
                        // Create & Close clicked (Quick-Close)
                        AssignModuleSpecificDefaults(currentTicket);

                        string historyDescription;
                        string approveActionDescription = closeStage.ApproveActionDescription;
                        if (string.IsNullOrEmpty(approveActionDescription))
                            historyDescription = firstStage.Name + " => " + closeStage.Name + " (Quick Close)";
                        else
                            historyDescription = approveActionDescription + " (Quick Close)";

                        if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPRP, currentTicket.Table) && currentTicket[DatabaseObjects.Columns.TicketPRP] == null)
                            currentTicket[DatabaseObjects.Columns.TicketPRP] = User.Id;

                        uHelper.CreateHistory(User, historyDescription, currentTicket, _context);

                        if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
                            currentTicket[DatabaseObjects.Columns.ModuleStepLookup] = closeStage.ID;

                        currentTicket[DatabaseObjects.Columns.StageStep] = closeStage.StageStep;
                        currentTicket[DatabaseObjects.Columns.TicketStatus] = closeStage.Name;

                        MarkTicketClosed(currentTicket);

                        SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, UGITUtility.ObjectToString(moduleId), performedAction: performedActionName);
                    }
                }
            }
            else
            {
                //save ticket once to load lookup related dependent fields
                //bool unsaveUpdate = thisWeb.AllowUnsafeUpdates;
                //thisWeb.AllowUnsafeUpdates = true;
                //Commented By Munna! 22-03-2017{
                //CommitChanges(currentTicket, buttonId, donotUpdateEscalations: true)
                //};       // senderid not comming
                //thisWeb.AllowUnsafeUpdates = unsaveUpdate;

                //defaultStartStage is ID;#value
                LifeCycleStage cycleNextStage = SetNextStage(currentTicket);

                //Send Email
                SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, UGITUtility.ObjectToString(moduleId), performedAction: performedActionName);

                //If ticket is just created and landing on some stage, then a notification should be sent for the landing stage approval email notification.
                if (performedActionName == Convert.ToString(TicketActionType.Created))
                    SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, UGITUtility.ObjectToString(moduleId), performedAction: Convert.ToString(TicketActionType.Approved));
            }

            //Assign new stage defaults
            AssignModuleSpecificDefaults(currentTicket);
        }


        /// <summary>
        ///  This function sets the Ticket Status as "Hold" .
        /// </summary>
        /// <param name="thisWeb"></param>
        /// <param name="currentTicket"></param>
        /// <param name="comment"></param>
        /// <param name="thisList"></param>
        /// <param name="moduleId"></param>
        /// <param name="holdTill">This is required. If it is passed as min or max value then fuction add a year in today date.</param>
        public void HoldTicket(DataRow currentTicket, string moduleId, string comment, DateTime holdTill, string holdReason, bool isEnableDeleteOnHold = false)
        {
            string holdComments = string.Format("On Hold Till {0}, Reason: {1}", UGITUtility.GetDateStringInFormat(holdTill, true), holdReason);
            if (!string.IsNullOrWhiteSpace(comment))
                holdComments = string.Format("{0}, Comment: {1}", holdComments, comment);
            if (isEnableDeleteOnHold)
                holdComments = holdComments + ", Ticket set to be deleted when hold time expires";
            holdComments = UGITUtility.WrapComment(holdComments, "hold");
            string oldStatus = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStatus]);

            currentTicket[DatabaseObjects.Columns.TicketOnHold] = "1";
            currentTicket[DatabaseObjects.Columns.TicketStatus] = "On Hold";
            currentTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetVersionString(User.Id, holdComments, currentTicket, DatabaseObjects.Columns.TicketComment);
            if (holdReason == null || holdReason.Trim() == string.Empty)
                holdReason = "Other";

            //currentTicket[DatabaseObjects.Columns.OnHoldReason] = holdReason;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReason, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.OnHoldReason] = holdReason;
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReasonChoice, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.OnHoldReasonChoice] = holdReason;
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldStartDate, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.TicketOnHoldStartDate] = DateTime.Now.ToString();
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldTillDate, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.TicketOnHoldTillDate] = DateTime.Today.AddYears(1);
                if (holdTill != DateTime.MinValue && holdTill != DateTime.MaxValue)
                    currentTicket[DatabaseObjects.Columns.TicketOnHoldTillDate] = holdTill;

                AgentJobHelper agentJob = new AgentJobHelper(_context);
                agentJob.ScheduleUnholdTicket(currentTicket, moduleId, isEnableDeleteOnHold);
            }

            string historyDescription = string.Format("{0} => On Hold Till {1}, Reason: {2}",
                                                        oldStatus,
                                                        Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketOnHoldTillDate]),
                                                        holdReason);
            uHelper.CreateHistory(User, historyDescription, currentTicket, true, _context);

            LifeCycleStage currentStage = GetTicketCurrentStage(currentTicket);
            
            TicketEventManager eventHelper = new TicketEventManager(_context, this.Module.ModuleName, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
            eventHelper.LogEvent(Constants.TicketEventType.Hold, currentStage, comment: comment, eventReason: holdReason, plannedEndDate: holdTill);

            if (Module.ModuleName == "SVC" && currentStage.StageTypeChoice == StageType.Assigned.ToString())
            {
                UGITTaskManager tManager = new UGITTaskManager(_context);
                //Hold in progress task and ticket if any for svc
                tManager.HoldTasks(Module.ModuleName, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), comment, holdTill, holdReason);
            }
        }

        public void UnHoldTicket(DataRow currentTicket, string moduleName, string comment, bool holdExpired = false, bool closeTicketOnHoldExpiration = false)
        {
            currentTicket[DatabaseObjects.Columns.TicketOnHold] = "0";

            // Get status from lifecycle or modulesStepLookup depending on module
            LifeCycleStage lifeCycleStage = null;
            string newStatus = string.Empty;
            lifeCycleStage = GetTicketCurrentStage(currentTicket);
            if (this.Module.ModuleName == "PMM")
            {
                newStatus = lifeCycleStage.Name;
            }
            else
                newStatus = lifeCycleStage.Name;
            currentTicket[DatabaseObjects.Columns.TicketStatus] = newStatus;
            if (!string.IsNullOrEmpty(comment))
                currentTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetVersionString(_context.CurrentUser.Id, UGITUtility.WrapComment(comment, holdExpired ? "HoldExpired" : "UnHold"), currentTicket, DatabaseObjects.Columns.TicketComment); //uHelper.GetVersionString(_context.CurrentUser.Id, comment, currentTicket, DatabaseObjects.Columns.TicketComment);
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldTillDate, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.TicketOnHoldTillDate] = DBNull.Value;
            }

            //currentTicket[DatabaseObjects.Columns.OnHoldReason] = string.Empty;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReason, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.OnHoldReason] = string.Empty;
            }

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReasonChoice, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.OnHoldReasonChoice] = string.Empty;
            }

            //calculate current hold of ticket
            double holdTime = 0;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldStartDate, currentTicket.Table) &&
                currentTicket[DatabaseObjects.Columns.TicketOnHoldStartDate] != null)
            {
                holdTime = uHelper.GetWorkingMinutesBetweenDates(_context, (DateTime)currentTicket[DatabaseObjects.Columns.TicketOnHoldStartDate], DateTime.Now);
            }

            //Set total hold duration of ticket till now.
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldStartDate, currentTicket.Table))
            {
                currentTicket[DatabaseObjects.Columns.TicketTotalHoldDuration] = UGITUtility.StringToDouble(currentTicket[DatabaseObjects.Columns.TicketTotalHoldDuration]) + holdTime;
            }
            LifeCycleStage currentLifeCycleStage = GetTicketCurrentStage(currentTicket);
            
            TicketEventManager eventHelper = new TicketEventManager(_context, this.Module.ModuleName, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
            string eventType = holdExpired ? Constants.TicketEventType.HoldExpired : Constants.TicketEventType.HoldRemoved;
            eventHelper.LogEvent(eventType, currentLifeCycleStage, comment: comment, bySystem: holdExpired);
            //
            if (holdExpired && closeTicketOnHoldExpiration)
            {
                Ticket ticketRequest = new Ticket(_context, moduleID);
                comment = string.Format("{0} closed due to expiration of hold", UGITUtility.moduleTypeName(this.Module.ModuleName));
                ticketRequest.CloseTicket(currentTicket, comment);
            }
            else
            {
                string historyDescription = string.Format("OnHold => {0}: {1}", newStatus, holdExpired ? "Hold Expired" : string.Format("Hold Removed by {0}", _context.CurrentUser.Name));
                uHelper.CreateHistory(_context.CurrentUser, historyDescription, currentTicket, true, _context);
            }
            
            if (Module.ModuleName == "SVC" && currentLifeCycleStage.StageTypeChoice == StageType.Assigned.ToString())
            {
                UGITTaskManager tManager = new UGITTaskManager(_context);
                //Hold in progress task and ticket if any for svc
                tManager.UnHoldTasks(Module.ModuleName, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), comment, holdExpired);
            }
        }

        public void CloseTicket(DataRow saveTicket, string resolution)
        {
            LifeCycleStage currentStage = GetTicketCurrentStage(saveTicket);
            LifeCycleStage closeStage = GetTicketCloseStage(saveTicket);

            if (!string.IsNullOrWhiteSpace(resolution) && UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.TicketResolutionComments))
                saveTicket[DatabaseObjects.Columns.TicketResolutionComments] = resolution;

            string historyDescription;
            string approveActionDescription = Convert.ToString(closeStage.ApproveActionDescription);
            if (string.IsNullOrEmpty(approveActionDescription))
                historyDescription = currentStage.Name + " => " + closeStage.Name + " (Force Close)";
            else
                historyDescription = approveActionDescription + " (Force Close)";

            uHelper.CreateHistory(User, historyDescription, saveTicket, false, _context);

            // Update skip stages
            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.WorkflowSkipStages))
            {
                LifeCycle lifeCycle = GetTicketLifeCycle(saveTicket);
                List<string> skipstep = new List<string>();
                if (!string.IsNullOrWhiteSpace(Convert.ToString(saveTicket[DatabaseObjects.Columns.WorkflowSkipStages])))
                    skipstep = Convert.ToString(saveTicket[DatabaseObjects.Columns.WorkflowSkipStages]).Split(',').ToList();

                LifeCycleStage currentstage = this.GetTicketCurrentStage(saveTicket, lifeCycle);
                foreach (LifeCycleStage stage in lifeCycle.Stages)
                {
                    if (stage.StageStep > currentstage.StageStep && stage.StageStep < closeStage.StageStep)
                        skipstep.Add(stage.StageStep.ToString());
                }
                skipstep.Sort();

                saveTicket[DatabaseObjects.Columns.WorkflowSkipStages] = string.Join(Constants.Separator6, skipstep.Distinct());
            }

            saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = closeStage.ID;
            saveTicket[DatabaseObjects.Columns.StageStep] = closeStage.StageStep;
            saveTicket[DatabaseObjects.Columns.TicketStatus] = closeStage.Name;

            // Assign close stage action user 
            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUserTypes) && saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
            {
                saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Join(Constants.Separator, closeStage.ActionUser);
                saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), saveTicket);
            }
            //if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.DataEditor))
            //{
            //    if (closeStage.DataEditors != null && closeStage.DataEditors.Count > 0)
            //        saveTicket[DatabaseObjects.Columns.DataEditor] = string.Join(Constants.Separator, closeStage.DataEditors.ToArray());
            //}
            MarkTicketClosed(saveTicket);
        }

        public void ClosePMMTicket(DataRow closeTicket, string comment, bool closePMMTasks = true)
        {
            if (UGITUtility.IsSPItemExist(closeTicket, DatabaseObjects.Columns.ProjectLifeCycleLookup))
            {
                long lcLookup = Convert.ToInt64(UGITUtility.GetSPItemValue(closeTicket, DatabaseObjects.Columns.ProjectLifeCycleLookup));
                LifeCycle lifeCycle = this.Module.List_LifeCycles.FirstOrDefault(x => x.ID == lcLookup);
                if (lifeCycle != null)
                {
                    LifeCycleStage closeStage = this.GetTicketCloseStage(lifeCycle);
                    string previousStage = Convert.ToString(closeTicket[DatabaseObjects.Columns.TicketStatus]);

                    if (!string.IsNullOrEmpty(comment))
                        closeTicket[DatabaseObjects.Columns.TicketComment] = comment;
                    closeTicket[DatabaseObjects.Columns.TicketStatus] = "Closed";
                    closeTicket[DatabaseObjects.Columns.StageStep] = closeStage.StageStep;
                    closeTicket[DatabaseObjects.Columns.ModuleStepLookup] = null;

                    UGITTaskManager taskManager = new UGITTaskManager(_context);
                    if (closePMMTasks)
                        taskManager.MarkPMMTaskAsComplete(Convert.ToString(closeTicket[DatabaseObjects.Columns.TicketId]), "PMM", closeTicket);
                    MarkTicketClosed(closeTicket);
                    string historyDescription = previousStage + " => " + closeStage.Name + " (Manual Close)";
                    uHelper.CreateHistory(User, historyDescription, closeTicket, false, _context);
                }
            }
        }


        /// <summary>
        /// Its check field level perfo
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="writeAccessID"></param>
        /// <returns></returns>
        public static bool HasFieldLevelAccess(ApplicationContext context, UGITModule module, DataRow ticket, int writeAccessID, UserProfile user)
        {
            ModuleRoleWriteAccess rwAccess = null;

            bool foundRoleWriteAccessEntry = false;
            if (writeAccessID == 0) // 0 means bypass RoleWriteAccess check
                foundRoleWriteAccessEntry = true;
            else
            {
                rwAccess = module.List_RoleWriteAccess.FirstOrDefault(x => x.ID == writeAccessID);
                foundRoleWriteAccessEntry = (rwAccess != null);
            }

            // If user is action user AND entry in rolewriteaccess for this field & stage, then user has access
            if (foundRoleWriteAccessEntry && (context.UserManager.IsActionUser(ticket, user) || context.UserManager.IsDataEditor(ticket, user)))
                return true;

            // Else user has access if in role that has specific field-level access
            string fieldLevelUserType = string.Empty;
            if (rwAccess != null && UGITUtility.SplitString(rwAccess.ActionUser, Constants.Separator).Count() > 0)
            {
                fieldLevelUserType = string.Join(Constants.Separator, rwAccess.ActionUser.ToArray());
                if (fieldLevelUserType != string.Empty)
                {
                    if (context.UserManager.IsUserPresentInField(user, ticket, fieldLevelUserType, true))
                        return true;
                }
            }

            // Neither condition applies, return false
            return false;
        }
        public static HyperLink GetHyperLinkControlForTicketID(DataRow moduleDetail, string ticketId, bool inIframe, string ticketTitle)
        {
            HyperLink lf = new HyperLink();
            lf.Text = ticketId;
            lf.ToolTip = ticketTitle;
            string navigationUrl = "javascript:";
            if (moduleDetail == null)
            {
                lf.NavigateUrl = navigationUrl;
                return lf;
            }

            string url = string.Empty;
            if (moduleDetail != null && moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
            {
                url = Convert.ToString(moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath]);
            }
            url = UGITUtility.GetAbsoluteURL(url);
            string prefix = "javascript:UgitOpenPopupDialog(";
            if (inIframe)
                prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
            navigationUrl = string.Format(prefix + "\"{0}\",\"TicketId={1}\",\"{2} Ticket: {1}\",\"90\",\"90\")", url, ticketId, moduleDetail[DatabaseObjects.Columns.ModuleName]);
            lf.NavigateUrl = navigationUrl;

            return lf;
        }
        public bool SendEmailToActionUsers(string currentStageId, DataRow currentTicket, string moduleId, List<string> newUsers = null, bool? IsAddREsolutionTime = null, string StrResolutionTime = null, string performedAction = "")
        {
            //isAddREsolutionTime = IsAddREsolutionTime;
            //strResolutionTime = StrResolutionTime;
            return SendEmailToActionUsers(currentStageId, currentTicket, moduleId, string.Empty, string.Empty, newUsers, performedAction);
        }
        public bool SendEmailToActionUsers(string currentStageId, DataRow currentTicket, string moduleId, string appendMsgBeforeBody, string appendMsgAfterBody, List<string> newUsers = null, string performedAction = "")
        {
            List<ModuleTaskEmail> taskEmails = this.Module.List_TaskEmail;
            if (taskEmails == null || taskEmails.Count == 0)
                return false;

            bool isPerformedAction = !string.IsNullOrEmpty(performedAction);
            bool allowSkipStages = false;

            if (isPerformedAction)
                allowSkipStages = performedAction == Convert.ToString(TicketActionType.Approved) || performedAction == Convert.ToString(TicketActionType.Returned);
            else
                allowSkipStages = true;

            //Send notification for skip stages if notification is enabled for skip stages
            //Get Priority from splistitem
            string priority = GetPriorityFromItem(currentTicket);
            if (skipStageNotificationStages != null && skipStageNotificationStages.Count > 0 && allowSkipStages)
            {
                foreach (LifeCycleStage skipStage in skipStageNotificationStages)
                {
                    // If SendEvenIfStageSkipped column in Taskemail true then email notification send for the skip stages.
                    List<ModuleTaskEmail> emailNotifications = null;

                    if (isPerformedAction)
                        emailNotifications = GetEmailNotifications(taskEmails, Convert.ToInt32(skipStage.ID), performedAction); // get action-specific notifications
                    else
                        emailNotifications = taskEmails.Where(x => x.StageStep.HasValue && x.StageStep.Value == skipStage.ID).ToList(); // get all notifications for stage

                    if (emailNotifications != null && emailNotifications.Count > 0)
                    {

                        foreach (ModuleTaskEmail notification in emailNotifications)
                        {
                            if (IsDifferentPriorityNotification(notification, priority))
                                continue;
                            bool notifyInPlainText = notification.NotifyInPlainText;
                            if (notification.SendEvenIfStageSkipped)
                                this.SendEmailToActionUsers(notification, currentTicket, moduleId, string.Empty, string.Empty, isHtmlBody: !notifyInPlainText, isskipped: true);
                        }
                    }
                }
                skipStageNotificationStages = new List<LifeCycleStage>();
            }

            string stageTitle = string.Empty;
            string stageStep = string.Empty;
            if (!string.IsNullOrEmpty(currentStageId)) // currentStageId is empty for On-Hold emails
            {
                if (currentStageId.Contains(Constants.Separator))
                {
                    currentStageId = UGITUtility.SplitString(currentStageId, Constants.Separator, 0);
                }
                // DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup + "='" + moduleId.ToString() + "'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{this.Module.ModuleName}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                DataRow stage = moduleStagesRow.FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.Id) == UGITUtility.StringToInt(currentStageId));

                if (stage != null)
                {
                    stageTitle = Convert.ToString(stage[DatabaseObjects.Columns.StageTitle]);
                    stageStep = UGITUtility.ObjectToString(stage[DatabaseObjects.Columns.ID]);
                }
            }

            #region Sharepoint reference
            List<ModuleTaskEmail> allStageNotifications = null;

            allStageNotifications = taskEmails.AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(Convert.ToString(x.StageStep)) && x.StageStep == UGITUtility.StringToInt(currentStageId)).ToList();

            if (isPerformedAction)
            {
                List<ModuleTaskEmail> actionNotifications = GetEmailNotifications(taskEmails, UGITUtility.StringToInt(stageStep), performedAction);
                // For Reject/Cancel, include the comment
                if (performedAction == Convert.ToString(TicketActionType.Rejected) && !string.IsNullOrWhiteSpace(Comment))
                {
                    if (!string.IsNullOrEmpty(appendMsgAfterBody))
                        appendMsgAfterBody = string.Format("{0}<br/><br/>{1}", appendMsgAfterBody, UGITUtility.WrapCommentForEmail(Comment, "reject"));
                    else
                        appendMsgAfterBody = UGITUtility.WrapCommentForEmail(Comment, "reject");
                }

                if (actionNotifications != null && actionNotifications.Count > 0)
                {
                    foreach (ModuleTaskEmail notification in actionNotifications)
                    {
                        if (IsDifferentPriorityNotification(notification, priority))
                            continue;

                        bool notifyInPlainText = UGITUtility.StringToBoolean(notification.NotifyInPlainText);
                        SendEmailToActionUsers(notification, currentTicket, moduleId, appendMsgBeforeBody, appendMsgAfterBody, newUsers, isHtmlBody: !notifyInPlainText);
                    }
                }
                else if (performedAction == Convert.ToString(TicketActionType.OnHold))
                {
                    List<ModuleTaskEmail> holdNotifications = taskEmails.AsEnumerable().Where(x => string.IsNullOrEmpty(x.Stage)).ToList();
                    if (holdNotifications != null && holdNotifications.Count > 0)
                    {
                        foreach (ModuleTaskEmail notification in holdNotifications)
                        {
                            if (IsDifferentPriorityNotification(notification, priority))
                                continue;

                            bool notifyInPlainText = UGITUtility.StringToBoolean(notification.NotifyInPlainText);
                            SendEmailToActionUsers(notification, currentTicket, moduleId, appendMsgBeforeBody, appendMsgAfterBody, newUsers, isHtmlBody: !notifyInPlainText);
                        }
                    }
                }
            }
            else
            {
                List<ModuleTaskEmail> actionNotifications = GetEmailNotifications(taskEmails, UGITUtility.StringToInt(stageStep), Convert.ToString(TicketActionType.Approved));
                foreach (ModuleTaskEmail email in actionNotifications)
                {
                    if (IsDifferentPriorityNotification(email, priority))
                        continue;

                    bool notifyInPlainText = UGITUtility.StringToBoolean(email.NotifyInPlainText);
                    SendEmailToActionUsers(email, currentTicket, moduleId, appendMsgBeforeBody, appendMsgAfterBody, newUsers, isHtmlBody: !notifyInPlainText);
                }
            }
            #endregion

            return true;
        }
        public bool SendEmailToActionUsers(ModuleTaskEmail moduleEmail, DataRow currentTicket, string moduleId, string appendMsgBeforeBody, string appendMsgAfterBody, List<string> newUsers = null, bool isHtmlBody = true, bool isskipped = false)
        {

            if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.SendEmail) == false)
                return true;

            if (currentTicket == null)
                return false;

            string currentModuleListPagePath = UGITUtility.GetAbsoluteURL(this.Module.ModuleRelativePagePath);
            string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
            string moduleName = this.Module.ModuleName;

            string emailTitle = uHelper.ReplaceTokensWithTicketColumns(_context, currentTicket, moduleEmail.EmailTitle, this, true);
            string emailBody = uHelper.ReplaceTokensWithTicketColumns(_context, currentTicket, moduleEmail.EmailBody, this, false, strResolutionTime);

            // We need both to send out an email
            if (string.IsNullOrEmpty(emailTitle) || string.IsNullOrEmpty(emailBody))
                return false;

            string greeting = _context.ConfigManager.GetValue(ConfigConstants.Greeting);
            string signature = _context.ConfigManager.GetValue(ConfigConstants.Signature);
            string newLine = isHtmlBody ? "<br />" : "\r\n";
            //Get all the email user types from the module emails list
            string[] currentStageEmailUserTypes = UGITUtility.SplitString(moduleEmail.EmailUserTypes, Constants.Separator);
            try
            {
                //Get all unique userEmails
                List<UsersEmail> emailUsers = new List<UsersEmail>();
                List<UserProfile> userList = null;
                if (newUsers != null)
                    userList = _context.UserManager.GetUserInfosById(string.Join(",", newUsers));
                if (newUsers != null)
                {
                    foreach (UserProfile u in userList)
                    {
                        UsersEmail uemail = new UsersEmail();
                        uemail.Email = u.Email;
                        uemail.UserName = u.Name;
                        emailUsers.Add(uemail);
                    }
                }
                else
                {
                    emailUsers = _context.UserManager.GetUsersEmail(currentTicket, currentStageEmailUserTypes, true);
                }

                string emailCC = Convert.ToString(moduleEmail.EmailIDCC);
                bool disableEmailTicketLink = moduleEmail.EmailBody.Contains(DatabaseObjects.Columns.TicketIdWithoutLink);

                string emailBodyTemp = string.Empty;
                //string[] emailList = emailUsers.Select(x => x.Email).ToArray();
                List<string> emailList = new List<string>();
                #region copy code
                List<string> userNames = new List<string>();
                if (emailUsers != null || !string.IsNullOrWhiteSpace(emailCC))
                {
                    //Change last, first name to first, last name
                    if (emailUsers != null)
                    {
                        emailUsers.ForEach(x =>
                        {
                            string email = !string.IsNullOrWhiteSpace(x.NotificationEmail) ? x.NotificationEmail : x.Email;
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                emailList.Add(email);
                                userNames.Add(x.UserName);
                            }
                        });

                        if (userNames != null && userNames.Count > 0)
                            userNames = userNames.Distinct().ToList();
                        if (emailList != null && emailList.Count > 0)
                            emailList = emailList.Distinct().ToList();

                        if (isHtmlBody)
                            emailBodyTemp = greeting + " <b>" + string.Join(", ", userNames) + "</b>";
                        else
                            emailBodyTemp = greeting + " " + string.Join(", ", userNames);
                    }
                    else if (isHtmlBody) // Leave off greeting for plaintext notification
                        emailBodyTemp = greeting;

                    if (!string.IsNullOrEmpty(appendMsgBeforeBody))
                        emailBodyTemp += newLine + newLine + appendMsgBeforeBody;

                    emailBodyTemp += newLine + newLine + emailBody;

                    if (!string.IsNullOrEmpty(appendMsgAfterBody))
                        emailBodyTemp += newLine + newLine + appendMsgAfterBody;

                    if (isHtmlBody) // Leave off greeting for plaintext notification
                        emailBodyTemp += newLine + newLine + signature + newLine;

                    if (isHtmlBody && !UGITUtility.StringToBoolean(moduleEmail.HideFooter))
                        emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, currentTicket, moduleName, true, disableEmailTicketLink, moduleEmail.Status, isskipped));


                    string usersEmail = string.Join(",", emailList);
                    //Create entry of outgoing mail against ticket
                    //DataRow dataRow = null;
                    MailMessenger mail = new MailMessenger(_context);
                    //mail.CreateOutgoingMailInstance(Utility.Constants.InProgress, moduleId, ticketId, usersEmail, moduleEmail.EmailIDCC, emailTitle, emailBodyTemp, ref dataRow);
                    mail.SendMail(usersEmail, emailTitle, emailCC, emailBodyTemp, isHtmlBody, new string[] { }, true, saveToTicketId: ticketId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "SendEmailToActionUsers: Could not send mail");
            }

            return true;
        }
        public string GetActionbuttonforEmail(DataRow item, string textWithTokens, Ticket objTicket)
        {
            StringBuilder emailbtn = new StringBuilder();

            if (objTicket != null)
            {
                LifeCycleStage currentStage = GetTicketCurrentStage(item);
                //SPWeb spWeb = item.ParentList.ParentWeb;

                string lnkUrlApprove = string.Empty;
                string imageUrlApprove = string.Empty;
                string lnkUrlReject = string.Empty;
                string imageUrlReject = string.Empty;
                string lnkUrlReturn = string.Empty;
                string imageUrlRetrun = string.Empty;

                if (currentStage.ApprovedStage != null)
                {
                    imageUrlApprove = UGITUtility.GetAbsoluteURL("/Content/ButtonImages/approve_btn.png", _context.SiteUrl);
                    // lnkUrlApprove = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Approve", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx"), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName);
                    //lnkUrlApprove = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Approve&cStage={3}", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx", spWeb), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.Step);
                    lnkUrlApprove = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Approve&cStage={3}", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx", _context.SiteUrl), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.StageStep);
                    emailbtn.AppendFormat("<a style='text-decoration: none' href='{0}'><img title='Approve' border='0' src='{1}'></a>&nbsp;", lnkUrlApprove, imageUrlApprove);
                }
                if (currentStage.RejectStage != null)
                {
                    imageUrlReject = UGITUtility.GetAbsoluteURL("/Content/ButtonImages/reject_btn.png", _context.SiteUrl);
                    //lnkUrlReject = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Reject", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx"), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName);
                    //lnkUrlReject = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Reject&cStage={3}", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx", spWeb), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.Step);
                    lnkUrlReject = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Reject&cStage={3}", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx", _context.SiteUrl), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.StageStep);
                    emailbtn.AppendFormat("<a style='text-decoration: none' href='{0}'><img title='Reject' border='0' src='{1}'></a>&nbsp;", lnkUrlReject, imageUrlReject);
                }
                if (currentStage.ReturnStage != null)
                {
                    imageUrlRetrun = UGITUtility.GetAbsoluteURL("/Content/ButtonImages/return_btn.png", _context.SiteUrl);
                    //lnkUrlReturn = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Return", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx"), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName);
                    //lnkUrlReturn = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Return&cStage={3}", uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx", spWeb), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.Step);
                    lnkUrlReturn = string.Format("{0}?TicketId={1}&control=approvereject&ModuleName={2}&UserAction=Return&cStage={3}", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx", _context.SiteUrl), Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, currentStage.StageStep);
                    emailbtn.AppendFormat("<a style='text-decoration: none' href='{0}'><img title='Return' border='0' src='{1}'></a>&nbsp;", lnkUrlReturn, imageUrlRetrun);
                }
            }

            return emailbtn.ToString();
        }
        public bool sendEmailToActionUsersOnHoldStage(DataRow currentTicket, string moduleId)
        {
            return sendEmailToActionUsersOnHoldStage(currentTicket, moduleId, string.Empty);
        }
        public bool sendEmailToActionUsersOnHoldStage(DataRow currentTicket, string moduleId, string extraInformationTobeMailed, string currentStageId = null, string performedActionName = "")
        {
            return SendEmailToActionUsers(currentStageId, currentTicket, moduleId, string.Empty, extraInformationTobeMailed, performedAction: performedActionName);
        }

        public void SendEmailToActionUsers(DataRow ticketItem, string subject, string body)
        {
            UserProfile emailUsers = _context.UserManager.GetUserInfo(ticketItem, UGITUtility.SplitString(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), Constants.Separator));
            SendEmailToEmailList(ticketItem, subject, body, emailUsers);
        }
        public void SendEmailToEmailList(DataRow ticketItem, string subject, string body, UserProfile emailUsers)
        {
            if (ticketItem == null || string.IsNullOrWhiteSpace(subject) || emailUsers == null)
            {
                ULog.WriteLog("ERROR: Cannot send email - null ticket, subject or email list!");
                return;
            }

            LifeCycleStage currentStage = GetTicketCurrentStage(ticketItem);
            string greeting = objConfigurationVariableHelper.GetValue("Greeting");
            string signature = objConfigurationVariableHelper.GetValue("Signature");

            //Get all unique userEmails
            string ticketId = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
            string emailBodyTemp = string.Empty;
            if (!string.IsNullOrEmpty(emailUsers.Email))
            {
                //Change last, first name to first, last name
                List<string> userNames = UGITUtility.ConvertStringToList(emailUsers.UserName, ";");
                for (int i = 0; i < userNames.Count; i++)
                {
                    userNames[i] = UGITUtility.ConvertToFirstMLast(userNames[i]);
                }

                emailBodyTemp = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}<br/><br/>
                                                {3}<br/>", greeting, string.Join(", ", userNames), body, signature);

                emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, ticketItem, moduleName, true, false));
                MailMessenger mail = new MailMessenger(_context);
                mail.SendMail(emailUsers.Email, subject, string.Empty, emailBodyTemp, true, new string[] { }, true);
            }
        }
        public void SendEmailToEmailList(DataRow ticketItem, string subject, string body, List<UsersEmail> emailUsers)
        {
            if (ticketItem == null || string.IsNullOrWhiteSpace(subject) || emailUsers == null)
            {
                ULog.WriteLog("ERROR: Cannot send email - null ticket, subject or email list!");
                return;
            }

            LifeCycleStage currentStage = GetTicketCurrentStage(ticketItem);
            string greeting = objConfigurationVariableHelper.GetValue("Greeting");
            string signature = objConfigurationVariableHelper.GetValue("Signature");

            //Get all unique userEmails
            string ticketId = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
            string emailBodyTemp = string.Empty;
            if (emailUsers != null)
            {
                //Change last, first name to first, last name
                List<string> userNames = emailUsers.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName).Distinct().ToList();
                for (int i = 0; i < userNames.Count; i++)
                {
                    userNames[i] = UGITUtility.ConvertToFirstMLast(userNames[i]);
                }

                emailBodyTemp = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}<br/><br/>
                                                {3}<br/>", greeting, string.Join(", ", userNames), body, signature);

                emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, ticketItem, moduleName, true, false));
                MailMessenger mail = new MailMessenger(_context);
                string tomails = string.Join(Constants.Separator6, emailUsers.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email).Distinct().ToList());
                mail.SendMail(tomails, subject, string.Empty, emailBodyTemp, true, new string[] { }, true);
            }
        }
        public void SendEmailToRequestor(DataRow ticketItem, string subject, string body)
        {
            if (UGITUtility.IfColumnExists(ticketItem, DatabaseObjects.Columns.TicketRequestor))
            {
                UserProfile emailUsers = _context.UserManager.GetRequestorUserInfo(ticketItem, UGITUtility.SplitString(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestor]), Constants.Separator));
                SendEmailToEmailList(ticketItem, subject, body, emailUsers);
            }
        }

        public void SendEmailToInitiator(DataRow ticketItem, string subject, string body)
        {
            if (UGITUtility.IfColumnExists(ticketItem, DatabaseObjects.Columns.TicketInitiator))
            {
                UserProfile emailUsers = _context.UserManager.GetUserInfo(ticketItem, UGITUtility.SplitString(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketInitiator]), Constants.Separator));
                SendEmailToEmailList(ticketItem, subject, body, emailUsers);
            }
        }
        public bool ProjectStageMove(DataRow currentTicket, List<UGITTask> tasks, bool updateTicket, ref List<string> messages)
        {
            bool success = false;
            messages = new List<string>();
            LifeCycle lifeCycle = GetTicketLifeCycle(currentTicket);

            if (lifeCycle == null || lifeCycle.Stages.Count <= 0)
            {
                messages.Add("Lifecycle not attached");
                return false;
            }

            if (!tasks.Exists(x => x.IsMileStone))
            {
                messages.Add("No task is assigned to milestone.");
                return false;
            }

            LifeCycleStage nextStage = null;

            foreach (LifeCycleStage iStage in lifeCycle.Stages)
            {
                UGITTask currentStageTask = tasks.FirstOrDefault(x => x.IsMileStone && x.StageStep == iStage.StageStep);
                if (currentStageTask != null)
                {
                    if (currentStageTask.Status.ToLower() == Constants.Completed.ToLower())
                    {
                        if (lifeCycle.Stages.Last().StageStep != iStage.StageStep)
                        {
                            nextStage = lifeCycle.Stages[lifeCycle.Stages.IndexOf(iStage) + 1];
                        }
                        else
                        {
                            nextStage = iStage;
                        }
                    }
                    else
                    {
                        nextStage = iStage;
                        break;
                    }
                }
            }

            if (nextStage != null)
            {
                LifeCycleStage currentStage = GetTicketCurrentStage(currentTicket, lifeCycle);
                LifeCycleStage closeDate = GetTicketCloseStage(currentTicket);

                if (currentStage == null || currentStage.StageStep != nextStage.StageStep)
                {
                    //**LogStageTransition(currentTicket, currentStage, nextStage, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]));

                    currentTicket[DatabaseObjects.Columns.TicketStatus] = nextStage.Name;
                    currentTicket[DatabaseObjects.Columns.StageStep] = nextStage.StageStep;
                    currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now.ToString();

                    //Set ticketclosedate nad ticketclosed project is closed
                    if (closeDate.StageStep == nextStage.StageStep)
                    {
                        UGITTask closedStageTask = tasks.FirstOrDefault(x => x.IsMileStone && x.StageStep == closeDate.StageStep);
                        if (closedStageTask != null && closedStageTask.Status != Constants.Completed)
                        {
                            // if  task attached at closed stage then close ticket only if it is completed
                            currentTicket[DatabaseObjects.Columns.TicketCloseDate] = null;
                            currentTicket[DatabaseObjects.Columns.TicketClosed] = 0;
                        }
                        else
                        {
                            currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DateTime.Now.ToString();
                            currentTicket[DatabaseObjects.Columns.TicketClosed] = 1;
                        }
                    }
                    else
                    {
                        currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value; //null;
                        currentTicket[DatabaseObjects.Columns.TicketClosed] = 0;
                    }

                    success = true;
                }
                else
                {
                    if (nextStage.StageStep == closeDate.StageStep)
                    {
                        UGITTask closedStageTask = tasks.FirstOrDefault(x => x.IsMileStone && x.StageStep == closeDate.StageStep);
                        if (closedStageTask != null && closedStageTask.Status == Constants.Completed)
                        {
                            currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DateTime.Now.ToString();
                            currentTicket[DatabaseObjects.Columns.TicketClosed] = 1;
                        }
                        success = true;
                    }
                }
                if (success)
                {
                    string historyDescription = string.Format("(Empty => {0}", nextStage.Name);
                    if (currentStage != null)
                        historyDescription = string.Format("{0} => {1}", currentStage.Name, nextStage.Name);

                    uHelper.CreateHistory(User, historyDescription, currentTicket, false, _context);

                    if (updateTicket)
                        //currentTicket.UpdateOverwriteVersion();
                        this.CommitChanges(currentTicket);

                    //if (SPContext.Current != null)
                    //{
                    //    //Update change after updating ticket
                    //    uGITCache.ModuleDataCache.UpdateOpenTicketsCache(this.moduleID, currentTicket);
                    //}
                }
            }
            return success;
        }
        // Utility functions to generate ticket urls for popups, emails, etc.
        public static string GenerateTicketURL(ApplicationContext context, string ticketID)
        {
            string moduleName = UGITUtility.SplitString(ticketID, "-", 0);
            if (string.IsNullOrEmpty(moduleName))
                return string.Empty;
            return GenerateTicketURL(context, moduleName, ticketID);
        }
        public static string GenerateTicketURL(ApplicationContext context, string moduleName, string ticketID)
        {
            return GenerateTicketURL(context, moduleName, ticketID, false);
        }
        public static string GenerateTicketURL(ApplicationContext context, string moduleName, string ticketID, bool forEmail)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.GetByName(moduleName);
            return GenerateTicketURL(context, module, ticketID, forEmail);
        }
        public static string GenerateTicketURL(ApplicationContext context, UGITModule moduleRow, string ticketID, bool forEmail)
        {
            string ticketURL = string.Empty;
            string moduleName = string.Empty;
            if (moduleRow != null)
            {
                moduleName = Convert.ToString(moduleRow.ModuleName);
                string colName = forEmail ? DatabaseObjects.Columns.ModuleRelativePagePath : DatabaseObjects.Columns.StaticModulePagePath;
                string detailPage = UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow.GetType().GetProperty(colName).GetValue(moduleRow)));// moduleRow[colName]));
                ticketURL = detailPage;

                if (ticketURL != null)
                {
                    if (ticketURL.Contains("?"))
                        ticketURL = string.Format("{0}&TicketId={1}", ticketURL, ticketID);
                    else
                    {
                        ticketURL = string.Format("{0}&TicketId={1}", ticketURL, ticketID);
                        var regex = new Regex(Regex.Escape("&"));
                        ticketURL = regex.Replace(ticketURL, "?", 1);
                    }
                }
            }
            return ticketURL;
        }
        public static async Task TicketFromMailAsync(ApplicationContext _context)
        {


            if (!_context.ConfigManager.GetValueAsBool(ConfigConstants.EnableEmailToTicket))
                return;

            // Fetch credential of mail account
            EmailToTicketConfiguration obj = new EmailToTicketConfiguration();
            string credential = _context.ConfigManager.GetValue(ConfigConstants.EmailToTicketCredentials);
            bool useOAuth2 = _context.ConfigManager.GetValueAsBool(ConfigConstants.EmailToTicketUsesOAuth2);
            if (!string.IsNullOrEmpty(credential))
            {
                XmlDocument xmlDocCtnt = new XmlDocument();
                xmlDocCtnt.LoadXml(credential);

                obj = (EmailToTicketConfiguration)uHelper.DeSerializeAnObject(xmlDocCtnt, obj);
            }
            else
                return;

            // Decrypt password
            string decryptPassword = uGovernITCrypto.Decrypt(obj.Password, Constants.UGITAPass);

            // Return if either one is not exist
            if (obj == null || string.IsNullOrEmpty(obj.ServerName) || string.IsNullOrEmpty(obj.UserName) || string.IsNullOrEmpty(decryptPassword) || obj.PortNo != 993)
                return;

            ULog.WriteLog("*** Started Email-to-Ticket Job Processing ..");

            SecureSocketOptions ssl = MailKit.Security.SecureSocketOptions.StartTls;
            if (obj.SSL)
                ssl = SecureSocketOptions.SslOnConnect;

            bool isdelete = obj.IsDelete;
            using (var client = new ImapClient())
            {
                using (var cancel = new CancellationTokenSource())
                {
                    try
                    {
                        // Connect to Imap server
                        int connectionAttempts = 0;
                        int maxAttempts = 3; // Try connecting 3 times
                        while (connectionAttempts < maxAttempts)
                        {
                            try
                            {
                                client.Connect(obj.ServerName.Trim(), obj.PortNo, ssl, cancel.Token);
                                if (client.IsConnected)
                                    break;
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "ERROR Connecting to IMAP server");
                            }
                            connectionAttempts++;
                        }

                        if (!client.IsConnected)
                        {
                            ULog.WriteLog(string.Format("*** Email-to-ticket job giving up after {0} failed connection attempts", maxAttempts));
                            return;
                        }

                        // Authenticate user
                        try
                        {

                            if (useOAuth2)
                            {
                                // OAUTH2 Authentication - Required as of 1/1/2023
                                string token = GetAccessToken(obj);
                                SaslMechanismOAuth2 oauth2 = new SaslMechanismOAuth2(obj.UserName, token);
                                client.Authenticate(oauth2);
                            }
                            else
                            {
                                // Basic Authentication - Deprecated
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                client.AuthenticationMechanisms.Remove("NTLM");
                                client.Authenticate(obj.UserName.Trim(), decryptPassword.Trim(), cancel.Token);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "ERROR Authenticating to IMAP server");
                            return;
                        }

                        // Query unread emails from Inbox folder which is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadWrite);
                        List<string> moduleTickets = new List<string>();
                        var items = inbox.Search(SearchQuery.NotSeen);
                        foreach (var id in items)
                        {
                            string subject = string.Empty;

                            try
                            {
                                var message = inbox.GetMessage(id, cancel.Token);

                                StringBuilder CC = new StringBuilder();
                                StringBuilder mailTo = new StringBuilder();
                                string emailFrom = string.Empty;
                                List<string> blockedListOfSenders = new List<string>();
                                string blockedSenders = _context.ConfigManager.GetValue(ConfigConstants.BlockedEmailToTicketSenders);
                                if (!string.IsNullOrEmpty(blockedSenders))
                                {
                                    blockedListOfSenders = blockedSenders.Split(new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                }

                                if (blockedListOfSenders.Count > 0)
                                {
                                    bool blockedSender = false;
                                    foreach (MailboxAddress fromAddress in message.From)
                                    {
                                        if (blockedListOfSenders.Count == 0 || blockedListOfSenders.Exists(x => x.ToLower() == fromAddress.Address.ToLower()))
                                        {
                                            emailFrom = fromAddress.Address;
                                            break;
                                        }
                                    }

                                    //return if email from is not allowed to create ticket
                                    if (blockedSender)
                                    {
                                        ULog.WriteLog(string.Format("Email sender [{0}] in blocked list, skipping", emailFrom));
                                        inbox.SetFlags(id, MessageFlags.Seen, true);
                                        continue;
                                    }
                                }
                                else
                                {
                                    foreach (MailboxAddress fromAddress in message.From)
                                    {
                                        if (blockedListOfSenders.Count == 0 || blockedListOfSenders.Exists(x => x.ToLower() == fromAddress.Address.ToLower()))
                                        {
                                            emailFrom = fromAddress.Address;
                                            break;
                                        }
                                    }
                                }


                                foreach (MailboxAddress toAddress in message.To.Mailboxes)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(mailTo)))
                                    {
                                        mailTo.Append(";");
                                        mailTo.Append(toAddress.Address);
                                    }
                                    else
                                        mailTo.Append(toAddress.Address);


                                }

                                foreach (MailboxAddress ccAddress in message.Cc.Mailboxes)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(CC)))
                                    {
                                        CC.Append(";");
                                        CC.Append(ccAddress.Address);
                                    }
                                    else
                                        CC.Append(ccAddress.Address);
                                }

                                string mailto = UGITUtility.RemoveDuplicateEmails(Convert.ToString(mailTo));
                                string cc = UGITUtility.RemoveDuplicateEmails(Convert.ToString(CC));

                                UserProfile sender = await _context.UserManager.FindByEmailAsync(emailFrom);

                                subject = string.IsNullOrEmpty(message.Subject) ? string.Empty : message.Subject.Trim();

                                // If HTML format, convert to text, else use text version as-is
                                string body = message.HtmlBody != null ? UGITUtility.StripHTML(message.HtmlBody, false, 5) : message.TextBody;

                                ULog.WriteLog(string.Format("*** Email ticket from: {0} ({1}): {2}",
                                                            (sender != null ? sender.Name : "unknown"), emailFrom, subject));

                                if (message.Body.ContentType.Parameters != null &&
                                            message.Body.ContentType.Parameters.Contains("report-type") &&
                                            message.Body.ContentType.Parameters["report-type"].ToLower().Contains("delivery-status"))
                                {
                                    ULog.WriteLog("Delivery status notification, skipping");
                                    inbox.SetFlags(id, MessageFlags.Seen, true);
                                    continue;
                                }

                                // See if email subject contains a ticket ID
                                string TicketId = string.Empty;
                                string pat = @"[A-Z]{3}-[0-9]{2}-[0-9]{6}";
                                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                                Match m = r.Match(subject);
                                if (m.Success)
                                {
                                    TicketId = m.Value;
                                    ULog.WriteLog(string.Format("-- Matched email to existing ticket: {0}", TicketId));
                                }

                                EmailsManager objEmailsManager = new EmailsManager(_context);
                                Email objEmail = null;

                                // Check ticket is already created or not
                                if (!string.IsNullOrEmpty(TicketId) && message.InReplyTo != null)
                                {
                                    objEmail = objEmailsManager.Get(x => x.MessageId == message.InReplyTo);
                                    if (objEmail != null)
                                    {
                                        TicketId = objEmail.TicketId;
                                        ULog.WriteLog(string.Format("-- Matched email to existing ticket: {0}", TicketId));
                                    }
                                }
                                else
                                {
                                    objEmail = new Email();
                                }

                                // If ticket ID found, we want to add to that ticket's email entries so get that module properties
                                // else we will want to create a new TSR ticket
                                string moduleName = string.Empty;

                                if (!string.IsNullOrEmpty(TicketId))
                                    moduleName = uHelper.getModuleNameByTicketId(TicketId);
                                else
                                {
                                    List<List<string>> tokenArray = uHelper.GetTokenValueArray(_context, uHelper.GetAllTokens(_context, body));
                                    moduleName = uHelper.GetModuleNameByToken(tokenArray).Trim();
                                    TicketId = uHelper.GetTicketbyToken(tokenArray).Trim();
                                    subject = uHelper.GetTitlebyToken(tokenArray).Trim();
                                    moduleName = uHelper.getModuleNameByTicketId(TicketId);
                                    if (string.IsNullOrWhiteSpace(moduleName))
                                        moduleName = "TSR";
                                }

                                Ticket objTicket = new Ticket(_context, moduleName);
                                int moduleId = uHelper.getModuleIdByModuleName(_context, moduleName);

                                // Create new Entry in TicketEmail
                                // Get email to, cc, etc.
                                string EmailIdTo = string.Empty;
                                string EmailIdCC = string.Empty;
                                string patEmailId = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                                Regex rEmailId = new Regex(patEmailId, RegexOptions.IgnoreCase);
                                if (!string.IsNullOrEmpty(mailto))
                                {
                                    MatchCollection matchesEmailIdTo = Regex.Matches(mailto, patEmailId);
                                    foreach (Match match in matchesEmailIdTo)
                                    {
                                        if (EmailIdTo != string.Empty)
                                            EmailIdTo += ";";
                                        EmailIdTo = EmailIdTo + match + ";";
                                    }
                                }

                                if (!string.IsNullOrEmpty(cc))
                                {
                                    MatchCollection matchesEmailIdCC = Regex.Matches(cc, patEmailId);
                                    foreach (Match match in matchesEmailIdCC)
                                    {
                                        if (EmailIdCC != string.Empty)
                                            EmailIdCC += ";";
                                        EmailIdCC += match + ";";
                                    }
                                }

                                subject = UGITUtility.TruncateWithEllipsis(subject, 255, null, ellipsis: ".."); // Title cannot be more than 255 characters!

                                objEmail.Title = subject;

                                objEmail.EmailIDTo = EmailIdTo;
                                objEmail.EmailIDCC = EmailIdCC;
                                objEmail.MailSubject = subject;
                                objEmail.ModuleNameLookup = moduleName;
                                objEmail.TicketId = TicketId;
                                objEmail.EmailIDFrom = Convert.ToString(emailFrom);
                                objEmail.IsIncomingMail = true;
                                objEmail.MessageId = message.MessageId;
                                objEmail.EmailStatus = Constants.EmailStatus.Delivered;

                                string bodyMessage = message.HtmlBody ?? message.TextBody;

                                //Handle inline attachments
                                foreach (var bodyPart in message.BodyParts)
                                {
                                    if (bodyPart is TextPart)
                                        continue;

                                    MimePart mPart = bodyPart as MimePart;
                                    if (mPart == null)
                                        continue;
                                    //we are handling attachment in differnt way. 
                                    //Inline attachment has isattachment false and has contentID value
                                    if (mPart.IsAttachment || string.IsNullOrWhiteSpace(mPart.ContentId))
                                        continue;

                                    try
                                    {
                                        string docStr = string.Empty;
                                        using (MemoryStream stream = new MemoryStream())
                                        {
                                            mPart.Content.WriteTo(stream);
                                            docStr = Encoding.UTF8.GetString(stream.ToArray());
                                            docStr = docStr.Replace("\n\r", "");
                                            docStr = docStr.Replace("\r\n", "");
                                        }

                                        StringBuilder contentString = new StringBuilder();
                                        contentString.Append("data:");
                                        contentString.AppendFormat("{0};", mPart.ContentType.MimeType);
                                        contentString.AppendFormat("{0},", mPart.ContentTransferEncoding.ToString().ToLower());
                                        contentString.Append(docStr);
                                        string contentID = string.Format("cid:{0}", mPart.ContentId);
                                        bodyMessage = bodyMessage.Replace(contentID, contentString.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex, "Inline attachment is being failed");
                                    }
                                }

                                objEmail.EscalationEmailBody = bodyMessage;

                                if (_context.ConfigManager.GetValueAsBool(ConfigConstants.EmailToTicketIgnoreAttachments))
                                {
                                    int attachmentCount = message.Attachments.Count();
                                    if (attachmentCount > 0)
                                        ULog.WriteLog(string.Format("  IGNORING {0} attachment(s)!", attachmentCount));
                                }
                                else
                                {
                                    List<string> attachments = new List<string>();
                                    foreach (MimeEntity attachment in message.Attachments)
                                    {
                                        string origFilename = string.Empty;
                                        string fileNameWithoutExt = string.Empty;
                                        string extension = string.Empty;

                                        MemoryStream ms = new MemoryStream();
                                        if (attachment is MimePart)
                                        {
                                            MimePart mPart = attachment as MimePart;
                                            mPart.Content.DecodeTo(ms);
                                            origFilename = mPart.FileName;
                                            fileNameWithoutExt = Path.GetFileNameWithoutExtension(mPart.FileName);
                                            //byte[] imageData = ms.ToArray();

                                            string allowedExtensions = _context.ConfigManager.GetValue(ConfigConstants.EmailToTicketAllowedAttachmentsExtn).ToLower();
                                            extension = Path.GetExtension(mPart.FileName);
                                            string returnExtn = string.Empty;
                                            if (!string.IsNullOrWhiteSpace(extension))
                                                returnExtn = extension.TrimStart(new char[] { '.' });
                                            if (!string.IsNullOrWhiteSpace(allowedExtensions) && !(allowedExtensions.Split(';').ToList()).Contains(returnExtn.ToLower()))
                                            {
                                                ULog.WriteLog(string.Format("   IGNORING Disallowed attachment with extension {0} for file {1}", returnExtn, mPart.FileName));
                                                continue;
                                            }

                                        }
                                        else if (attachment is MessagePart)
                                        {
                                            MessagePart msgPart = attachment as MessagePart;
                                            msgPart.WriteTo(ms);
                                            origFilename = fileNameWithoutExt = msgPart.Message.Subject;
                                            extension = ".eml";
                                        }

                                        byte[] imageData = ms.ToArray();
                                        fileNameWithoutExt = UGITUtility.ReplaceInvalidCharsInFolderName(fileNameWithoutExt, extension.Length);
                                        if (string.IsNullOrWhiteSpace(fileNameWithoutExt))
                                            fileNameWithoutExt = string.Format("{0}", Guid.NewGuid().ToString().Substring(0, 8));
                                        if (attachments.Exists(x => x.Contains(fileNameWithoutExt)))
                                            fileNameWithoutExt = string.Format("{0}_{1}", fileNameWithoutExt, Guid.NewGuid().ToString().Substring(0, 8));
                                        string fullFileName = string.Format("{0}{1}", fileNameWithoutExt, extension);

                                        ULog.WriteLog(string.Format("  Adding attachment: {0}", fullFileName));

                                        attachments.Add(fullFileName);
                                    }
                                }

                                // Need to save here to preserve attachments
                                if (objEmail.ID > 0)
                                    objEmailsManager.Update(objEmail);
                                else
                                    objEmailsManager.Insert(objEmail);

                                if (!string.IsNullOrEmpty(TicketId))
                                {
                                    // If the email was regarding an existing ticket, no need to create a new ticket, just attach to existing ticket (done above) and send notification to action user
                                    // SPListItem listItem = uHelper.GetTicket(TicketId);
                                    DataRow listItem = GetCurrentTicket(_context, moduleName, TicketId);
                                    if (listItem != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketStageActionUserTypes, listItem.Table))
                                    {
                                        bool isActionUser = false;
                                        string[] actionUserTypes = UGITUtility.SplitString(Convert.ToString(listItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), Constants.Separator);
                                        UserProfile actionUsers = _context.UserManager.GetUserInfo(listItem, actionUserTypes); //UserProfile.GetUserInfo(listItem, actionUserTypes);
                                        if (actionUsers != null && actionUsers.Email != null && !string.IsNullOrWhiteSpace(emailFrom) && actionUsers.Email.ToLower().IndexOf(emailFrom.ToLower()) != -1)
                                            isActionUser = true;

                                        // If action user replied with uppercase "APPROVE" or "REJECT" in subject line, then take appropriate action
                                        // ONLY IF email approval is allowed for current stage (allow if no mandatory data entry needed)
                                        bool foundEmailApproval = false;
                                        if (isActionUser)
                                        {
                                            if (subject.Contains("REJECT") || body.StartsWith("REJECT"))
                                            {
                                                LifeCycleStage currentStage = objTicket.GetTicketCurrentStage(listItem);
                                                if (currentStage.Prop_AllowEmailApproval)
                                                {
                                                    ULog.WriteLog("***   Found REJECT by action user for ticket " + TicketId);
                                                    string comment = subject.Substring(subject.IndexOf(TicketId) + TicketId.Length, subject.IndexOf("REJECT") - (subject.IndexOf(TicketId) + TicketId.Length));
                                                    List<TicketColumnError> errors = new List<TicketColumnError>();
                                                    objTicket.RejectTicket(errors, listItem, comment, sender);

                                                    foundEmailApproval = true;
                                                    moduleTickets.Add(TicketId);
                                                }
                                            }
                                            else if (subject.Contains("APPROVE") || body.StartsWith("APPROVE"))
                                            {
                                                LifeCycleStage currentStage = objTicket.GetTicketCurrentStage(listItem);
                                                if (currentStage.Prop_AllowEmailApproval)
                                                {
                                                    ULog.WriteLog("***   Found APPROVE by action user for ticket " + TicketId);
                                                    List<TicketColumnError> errors = new List<TicketColumnError>();
                                                    objTicket.ApproveTicket(errors, listItem, false, sender);
                                                    foundEmailApproval = true;
                                                    moduleTickets.Add(TicketId);
                                                }
                                            }
                                        }

                                        if (!foundEmailApproval) // check if from requestor
                                        {
                                            bool isRequestor = false;
                                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestor, listItem.Table))
                                            {
                                                string[] requestorsuseertype = UGITUtility.SplitString(Convert.ToString(listItem[DatabaseObjects.Columns.TicketRequestor]), Constants.Separator);
                                                UserProfile requestors = _context.UserManager.GetUserInfo(listItem, requestorsuseertype);
                                                if (requestors != null && sender != null && requestors.UserName.Contains(sender.Id.ToString()))
                                                    isRequestor = true;
                                            }

                                            // If email is from requestor, then send notification to action users
                                            if (isRequestor)
                                            {
                                                string emailSubject = string.Format("Requestor Response Received for ticket {0}", TicketId);
                                                StringBuilder emailBody = new StringBuilder();
                                                emailBody.AppendFormat("Follow-up email received from requestor for ticket <b>{0} [{1}]</b>:<br /><br />", TicketId, Convert.ToString(listItem[DatabaseObjects.Columns.Title]));
                                                emailBody.AppendFormat("Email From: {0}<br />", (sender != null ? sender.Name : emailFrom));
                                                emailBody.AppendFormat("Email To: {0}<br />", EmailIdTo);
                                                if (!string.IsNullOrWhiteSpace(EmailIdCC))
                                                    emailBody.AppendFormat("Email cc: {0}<br />", EmailIdCC);
                                                emailBody.AppendFormat("Subject: {0}<br /><br />", subject);
                                                emailBody.Append(uHelper.ConvertTextAreaStringToHtml(body));
                                                objTicket.SendEmailToEmailList(listItem, emailSubject, emailBody.ToString(), actionUsers);
                                            }
                                        }


                                        objTicket.UpdateTicketFromEmail(subject, body, _context.CurrentUser, objEmail, moduleName, listItem, emailFrom);
                                        //objTicket.CommitChanges(listItem);
                                    }
                                }
                                else
                                {
                                    // Create New ticket & add entry in in TicketEmail     
                                    DataRow TSRListItem = null;
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(subject))
                                            TSRListItem = objTicket.CreateTicketFromEmail(subject, body, _context.CurrentUser, objEmail, moduleName, emailFrom);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }

                                    //If TSRListItem null delete from TicketEmail
                                    if (TSRListItem == null)
                                    {
                                        objEmailsManager.Delete(objEmail);
                                        continue;
                                    }

                                    TicketId = Convert.ToString(TSRListItem[DatabaseObjects.Columns.TicketId]);
                                    moduleTickets.Add(TicketId);

                                    // Update TicketId (just generated in Create() above into saveTicketEmail entry so its tied to ticket
                                    objEmail.TicketId = TicketId;
                                    objEmailsManager.Update(objEmail);

                                }

                                if (isdelete)
                                {
                                    inbox.AddFlags(new MailKit.UniqueId[] { id }, MessageFlags.Deleted, true);
                                    inbox.Expunge();
                                }
                                else
                                    inbox.SetFlags(id, MessageFlags.Seen, true);
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "ERROR processing email with subject: " + subject);
                            }
                        }

                        client.Disconnect(true, cancel.Token);

                        // SPListHelper.ReloadTicketsInCache(moduleTickets, spWeb);

                        ULog.WriteLog(string.Format("*** Email-to-Ticket job done, processed {0} email(s), created/updated {1} ticket(s)", items.Count, moduleTickets.Count));
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                }
            }
        }
        private static string GetAccessToken(EmailToTicketConfiguration config)
        {


            string endpointurl = "https://login.microsoftonline.com/" + config.TenantId + "/oauth2/v2.0/token";
            string scope = "https://outlook.office365.com/.default";
            string grant_type = "client_credentials";
            string token = string.Empty;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;



            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointurl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string jsondata = "grant_type=" + grant_type + "&Client_id=" + config.ClientId + "&Client_Secret=" + config.SecretId + "&Scope=" + scope + "";
            byte[] data = Encoding.ASCII.GetBytes(jsondata);
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }



            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            if (response.StatusCode.ToString() == "OK")
            {
                using (Stream stream = dataStream)
                {
                    StreamReader reader = new StreamReader(stream);
                    Newtonsoft.Json.Linq.JToken resl = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JToken>(reader.ReadToEnd());
                    token = resl["access_token"].ToString();
                }
            }
            return token;
        }
        public DataRow CreateTicketFromEmail(string title, string description, UserProfile user, Email ticketEmailItem, string modulename, string from = "")
        {
            if (string.IsNullOrEmpty(modulename))
                return null;

            DataTable oList = null;
            oList = ticketManager.GetAllTickets(moduleMgr.LoadByName(modulename, true));
            int moduleId = uHelper.getModuleIdByModuleName(_context, modulename);
            DataRow moduleDetail;
            DataRow[] moduleStagesRow;
            DataTable modulesTable = moduleMgr.LoadAllModules();
            moduleDetail = modulesTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, modulename))[0];
            LifeCycleManager objLifeCycleHelper = new LifeCycleManager(_context);
            moduleStagesRow = UGITUtility.ToDataTable<LifeCycleStage>(objLifeCycleHelper.LoadLifeCycleByModule(modulename)[0].Stages).Select();
            DataRow firstStage = moduleStagesRow[0];
            DataRow saveTicket = oList.NewRow();
            MatchCollection mColl = uHelper.GetAllTokens(_context, description);
            uHelper.InsertTokenInItem(_context, modulename, uHelper.GetTokenValueArray(_context, mColl), saveTicket);

            //To Remove tokens from description when save in ticket table
            string descAfterRemoveToken = description;
            foreach (Match token in mColl)
            {
                descAfterRemoveToken = descAfterRemoveToken.Replace(token.Value, "");
            }
            descAfterRemoveToken = descAfterRemoveToken.Trim();

            saveTicket[DatabaseObjects.Columns.Title] = string.IsNullOrEmpty(Convert.ToString(saveTicket[DatabaseObjects.Columns.Title])) == true ? title : saveTicket[DatabaseObjects.Columns.Title];
            //saveTicket[DatabaseObjects.Columns.TicketDescription] = string.IsNullOrEmpty(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketDescription])) == true ? descAfterRemoveToken : saveTicket[DatabaseObjects.Columns.TicketDescription];
            saveTicket[DatabaseObjects.Columns.TicketStatus] = Convert.ToString(firstStage[DatabaseObjects.Columns.StageTitle]);
            saveTicket[DatabaseObjects.Columns.StageStep] = Convert.ToString(firstStage[DatabaseObjects.Columns.StageStep]);
            saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = firstStage[DatabaseObjects.Columns.Id];
            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.RequestSource))
                saveTicket[DatabaseObjects.Columns.RequestSource] = "E-mail";
            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate))
                saveTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = DateTime.Now.AddDays(1).ToString();
            saveTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now.ToString();

            if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.IsTicketAttachment))
                saveTicket[DatabaseObjects.Columns.IsTicketAttachment] = false;

            // Set requestor, requestor location & business manager
            UserProfile userInfoItem = user;


            if (user != null)
            {
                saveTicket[DatabaseObjects.Columns.TicketRequestor] = user.Id;

                if (userInfoItem != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.LocationLookup, saveTicket.Table) &&
                    string.IsNullOrWhiteSpace(Convert.ToString(saveTicket[DatabaseObjects.Columns.LocationLookup])))
                {
                    saveTicket[DatabaseObjects.Columns.LocationLookup] = userInfoItem.LocationId;
                }

                // If email sender is valid site user, then set business manager to user's manager (if not already set)
                if (userInfoItem != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketBusinessManager, saveTicket.Table) &&
                    string.IsNullOrWhiteSpace(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketBusinessManager])) &&
                    !string.IsNullOrWhiteSpace(Convert.ToString(userInfoItem.ManagerID)))
                {
                    saveTicket[DatabaseObjects.Columns.TicketBusinessManager] = Convert.ToString(userInfoItem.ManagerID);
                }
            }

            if (ticketEmailItem != null)
            {
                //foreach (string fileName in ticketEmailItem.Attachments)
                //{
                //    SPFile file = ticketEmailItem.ParentList.ParentWeb.GetFile(ticketEmailItem.Attachments.UrlPrefix + fileName);
                //    byte[] imageData = file.OpenBinary();
                //    saveTicket.Attachments.Add(fileName, imageData);
                //}
            }

            // Set the RequestTypeLookup according to the "Keywords" available in the mail body or in title.
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup, saveTicket.Table) && saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] == null)
            {
                // Get Request Types where keywords field has values
                //SPQuery query = new SPQuery();
                //query.Query = @"<Where><And><Eq><FieldRef Name='ModuleNameLookup' LookupId='TRUE' /><Value Type='Lookup'>" + moduleId.ToString() + "</Value></Eq>" +
                //                          @"<Or><IsNotNull><FieldRef Name='KeyWords' /></IsNotNull><IsNotNull><FieldRef Name='EmailToTicketSender' /></IsNotNull></Or>" +
                //              @"</And></Where>";
                //query.ViewFields = string.Concat(
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.Id),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.Title),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.ModuleNameLookup),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.TicketRequestType),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.EmailToTicketSender),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.KeyWords),
                //      string.Format("<FieldRef Name='{0}' Nullable='True' />", DatabaseObjects.Columns.MatchAllKeywords)
                //      );
                //query.ViewFieldsOnly = true;
                DataTable requestTypeData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"TenantID='{_context.TenantID}'");
                DataRow[] requestTypeDatacoll = requestTypeData.Select(string.Format("{0}='{1}' And ({2}<>isnull Or {3}<>isnull)", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.KeyWords, DatabaseObjects.Columns.EmailToTicketSender));
                //SPListHelper.GetDataTable(DatabaseObjects.Lists.RequestType, query, oWeb);

                string titleStr = title.ToLower();
                string descriptionStr = description.ToLower();
                DataRow requestTypeRow = null;

                if (requestTypeData != null)
                {
                    if (!string.IsNullOrWhiteSpace(from))
                    {
                        from = from.ToLower();
                        foreach (DataRow requestType in requestTypeData.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(Convert.ToString(requestType[DatabaseObjects.Columns.EmailToTicketSender])))
                                continue;

                            if (Convert.ToString(requestType[DatabaseObjects.Columns.EmailToTicketSender]).ToLower().IndexOf(from) != -1)
                            {
                                requestTypeRow = requestType;
                                ULog.WriteLog(string.Format("Matched configured sender [{0}] in email, matched to request type {1}", from, requestType[DatabaseObjects.Columns.Title]));
                                break;
                            }
                        }
                    }

                    if (requestTypeRow == null)
                    {
                        foreach (DataRow requestType in requestTypeData.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(Convert.ToString(requestType[DatabaseObjects.Columns.KeyWords])))
                                continue;
                            bool matchall = UGITUtility.StringToBoolean(requestType[DatabaseObjects.Columns.KeyWords]);
                            string[] keyWordsList = UGITUtility.SplitString(requestType[DatabaseObjects.Columns.KeyWords], ";", StringSplitOptions.RemoveEmptyEntries);

                            if (keyWordsList != null && keyWordsList.Length > 0)
                            {
                                if (matchall == true)
                                {
                                    bool valid = false;

                                    foreach (string keyword in keyWordsList)
                                    {

                                        if (titleStr.Contains(keyword.ToLower()) || descriptionStr.Contains(keyword.ToLower()))
                                            valid = true;
                                        else
                                        {
                                            valid = false;
                                            break;
                                        }
                                    }
                                    if (valid)
                                    {
                                        requestTypeRow = requestType;
                                        ULog.WriteLog(string.Format("Found all keywords in email, matched to request type {0}", requestType[DatabaseObjects.Columns.Title]));
                                    }

                                }
                                else
                                {
                                    foreach (string keyword in keyWordsList)
                                    {
                                        if (titleStr.Contains(keyword.ToLower()) || descriptionStr.Contains(keyword.ToLower()))
                                        {
                                            requestTypeRow = requestType;
                                            ULog.WriteLog(string.Format("Found keyword [{0}] in email, matched to request type {1}", keyword, requestType[DatabaseObjects.Columns.Title]));
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                if (requestTypeRow != null)
                {
                    saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = requestTypeRow[DatabaseObjects.Columns.Id];

                    // Set business manager field value if not already set
                    // NOTE: we want to do this only when request type has been set to allow the ticket to move forward.
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketBusinessManager, saveTicket.Table) &&
                        string.IsNullOrWhiteSpace(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketBusinessManager])))
                    {
                        string managerLookup = null;

                        // Use system account's (current user's) manager
                        UserProfile systemAccItem = user;//SPListHelper.GetSPListItem(oWeb.SiteUserInfoList, oWeb.CurrentUser.ID);
                        if (systemAccItem != null && !string.IsNullOrWhiteSpace(Convert.ToString(systemAccItem.ManagerID)))
                            managerLookup = Convert.ToString(systemAccItem.ManagerID);

                        // else set to system account (current user)
                        if (managerLookup == null)
                            managerLookup = string.Format("{0}{1}{2}", user.Id, Constants.Separator, User.Name);

                        saveTicket[DatabaseObjects.Columns.TicketBusinessManager] = user.ManagerID;
                    }
                }
            }

            // Create ticket.
            Create(saveTicket, _context.CurrentUser);

            // Need to set Priority before we update the ticket (Emails were showing priority Id instead of the value)
            SetTicketPriority(saveTicket, modulename);
            string query = $"{DatabaseObjects.Columns.ModuleNameLookup}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";
            DataRow[] moduleRoleWriteAccessRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestRoleWriteAccess, query).Select();
            //Get all mandatory fields for this stage.
            DataRow[] roleWriteRows = moduleRoleWriteAccessRow.Where(x => x.Field<Int32>(DatabaseObjects.Columns.StageStep) == 1 && x.Field<Boolean>(DatabaseObjects.Columns.FieldMandatory) == true).ToArray();
            //Even if we have one mandatory field, we cannot AutoApprove it.
            bool stageHasMandatoryFields = false;
            if (roleWriteRows.Length >= 1)
            {
                foreach (DataRow roleWriteRow in roleWriteRows)
                {
                    string columnName = Convert.ToString(roleWriteRow[DatabaseObjects.Columns.FieldName]);
                    bool dependentField = UGITUtility.IsDependentMandatoryField(modulename, columnName);
                    if (UGITUtility.IfColumnExists(columnName, saveTicket.Table) && !dependentField)
                    {
                        if (Convert.ToString(saveTicket[columnName]) == string.Empty)
                        {
                            stageHasMandatoryFields = true;
                            break;
                        }
                    }
                }
            }

            if (!stageHasMandatoryFields)
                QuickClose(uHelper.getModuleIdByModuleName(_context, modulename), saveTicket, "Create");

            // If we are still in first stage (because no keyword match to request type, missing mandatory fields, etc.), then set action user to help desk
            if (Convert.ToString(saveTicket[DatabaseObjects.Columns.StageStep]) == "1")
            {
                string helpDeskGroup = _context.ConfigManager.GetValue("HelpDeskGroup");
                saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = helpDeskGroup;
                saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = helpDeskGroup;
            }

            this.CommitChanges(saveTicket);

            return saveTicket;
        }

        public List<LifeCycleStage> GetCurrentLifeCyleStages(DataRow ticket)
        {
            LifeCycle currentLf = GetTicketLifeCycle(ticket);
            if (currentLf == null)
                return new List<LifeCycleStage>();

            return currentLf.Stages;
        }

        public bool IsNextStageSkip(DataRow item)
        {
            LifeCycleStage currentLifeCycle = this.GetTicketCurrentStage(item);
            if (currentLifeCycle.ApprovedStage != null && !string.IsNullOrWhiteSpace(currentLifeCycle.ApprovedStage.SkipOnCondition))
            {
                if (FormulaBuilder.EvaluateFormulaExpression(_context, currentLifeCycle.ApprovedStage.SkipOnCondition, item))
                {
                    return true;
                }
            }
            return false;
        }


        public DataRow CreateModuleInstance(Dictionary<string, object> ticketData)
        {
            DataRow currentTicket = null;
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_context);
            currentTicket = moduleInstanceList.NewRow();
            List<LifeCycleStage> stages = GetCurrentLifeCyleStages(currentTicket);
            LifeCycleStage stageRow = Module.List_LifeCycles[0].Stages.FirstOrDefault(x => x.StageStep == 1);
            //Returns null if not stage exist for module
            if (stageRow == null)
            {
                return null;
            }
            List<ModuleRoleWriteAccess> startingStageFormFields = Module.List_RoleWriteAccess.Where(x => x.StageStep == 0 || x.StageStep == 1).ToList();
            List<ModuleFormLayout> startingFormLayoutFields = Module.List_FormLayout.Where(x => x.TabId == 0).ToList();
            currentTicket[DatabaseObjects.Columns.StageStep] = stageRow.StageStep;
            if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketStatus) && !string.IsNullOrEmpty(Convert.ToString(ticketData[DatabaseObjects.Columns.TicketStatus])))
            {
                currentTicket[DatabaseObjects.Columns.TicketStatus] = Convert.ToString(ticketData[DatabaseObjects.Columns.TicketStatus]);
            }
            else
            {
                currentTicket[DatabaseObjects.Columns.TicketStatus] = stageRow.StageTitle;
            }


            foreach (ModuleRoleWriteAccess formField in startingStageFormFields)
            {
                if (ticketData.ContainsKey(formField.FieldName) && !string.IsNullOrEmpty(Convert.ToString(ticketData[formField.FieldName])))
                {
                    if (moduleInstanceList.Columns[formField.FieldName].DataType.FullName == "System.Boolean")
                    {
                        if (ticketData[formField.FieldName].ToString() == "1")
                            currentTicket[formField.FieldName] = true;
                        else
                            currentTicket[formField.FieldName] = false;
                    }
                    else if (moduleInstanceList.Columns[formField.FieldName].DataType.FullName == "System.Int64")
                    {
                        long value = 0;
                        if (Int64.TryParse(Convert.ToString(ticketData[formField.FieldName]), out value))
                        {
                            currentTicket[formField.FieldName] = value;
                        }
                    }
                    else
                    {
                        currentTicket[formField.FieldName] = ticketData[formField.FieldName];
                    }
                }
            }

            currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = GetStageActionUsers(stageRow.StageApprovedStatus);
            currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket);

            // Get a new ticket ID.
            String newTicketId;
            newTicketId = GetNewTicketId();
            currentTicket[DatabaseObjects.Columns.TicketId] = newTicketId;

            if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.ProjectID) && Convert.ToString(currentTicket[DatabaseObjects.Columns.ProjectID]) == "")
            {
                // Condition added, as per Jon's suggestion in mail, 'BCCI Items 12.09.19', to remove ProjectID from LEM.
                if (moduleName != ModuleNames.LEM)
                    currentTicket[DatabaseObjects.Columns.ProjectID] = GetNewProjectId();
            }

            if ((moduleName == "CON") && currentTicket[DatabaseObjects.Columns.FirstName] != null && currentTicket[DatabaseObjects.Columns.LastName] != null)
            {
                currentTicket[DatabaseObjects.Columns.Title] = string.Format("{0} {1}", currentTicket[DatabaseObjects.Columns.FirstName], currentTicket[DatabaseObjects.Columns.LastName]);
            }

            // (*) after Title indicates Master Agreement for that Company
            if (moduleName == "COM" && UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.MasterAgreement))
            {
                if (currentTicket[DatabaseObjects.Columns.MasterAgreement] != DBNull.Value && Convert.ToBoolean(currentTicket[DatabaseObjects.Columns.MasterAgreement]) == true)
                {
                    if (!Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]).Contains(" (*)"))
                    {
                        currentTicket[DatabaseObjects.Columns.Title] = $"{currentTicket[DatabaseObjects.Columns.Title]} (*)";
                    }
                }
                else if (currentTicket[DatabaseObjects.Columns.MasterAgreement] != DBNull.Value && Convert.ToBoolean(currentTicket[DatabaseObjects.Columns.MasterAgreement]) == false)
                {
                    if (Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]).Contains(" (*)"))
                    {
                        currentTicket[DatabaseObjects.Columns.Title] = Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]).Replace(" (*)", string.Empty);
                    }
                }
            }

            if (moduleName == "OPM" || moduleName == "CPR" && UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.Address))
            {
                currentTicket[DatabaseObjects.Columns.Address] = GetAddress(currentTicket);
            }

            if (moduleName == "OPM" || moduleName == "CPR" && UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.AcquisitionCost))
            {
                //currentTicket[DatabaseObjects.Columns.AcquisitionCost] = GetAcquisitionCost(currentTicket);
                DataTable dt = GetForecastAndAcquisitionCosts(currentTicket);
                if (dt.Rows.Count > 0)
                {
                    currentTicket[DatabaseObjects.Columns.AcquisitionCost] = dt.Rows[0]["ForecastedAcquisitionCost"];
                    currentTicket[DatabaseObjects.Columns.ActualAcquisitionCost] = dt.Rows[0][DatabaseObjects.Columns.ActualAcquisitionCost];
                    currentTicket[DatabaseObjects.Columns.ForecastedProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ForecastedProjectCost];
                    currentTicket[DatabaseObjects.Columns.ActualProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ActualProjectCost];
                }
            }

            if (moduleName == "CMDB")
            {
                currentTicket[DatabaseObjects.Columns.AssetTagNum] = newTicketId;

                if (ticketData.ContainsKey(DatabaseObjects.Columns.AssetDescription) && ticketData.ContainsKey(DatabaseObjects.Columns.AssetDescription))
                {
                    currentTicket[DatabaseObjects.Columns.AssetDescription] = ticketData[DatabaseObjects.Columns.AssetDescription];
                }
            }
            else
            {
                if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    currentTicket[DatabaseObjects.Columns.TicketCreationDate] = DateTime.Now;
                }
                if (currentTicket[DatabaseObjects.Columns.TicketInitiator] == DBNull.Value || (String)currentTicket[DatabaseObjects.Columns.TicketInitiator] == String.Empty)
                {
                    currentTicket[DatabaseObjects.Columns.TicketInitiator] = _context.CurrentUser.Id;
                }
            }
            // set the current stage start date.
            currentTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now;

            //Set Request type and columns which are drived from request type like (TicketOwner, FunctionalAreaLookup)
            if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketRequestTypeLookup))
            {
                if (ticketData[DatabaseObjects.Columns.TicketRequestTypeLookup] != DBNull.Value && ticketData[DatabaseObjects.Columns.TicketRequestTypeLookup] != null && Convert.ToString(ticketData[DatabaseObjects.Columns.TicketRequestTypeLookup]) != string.Empty)
                    currentTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = UGITUtility.SplitString(ticketData[DatabaseObjects.Columns.TicketRequestTypeLookup], Constants.Separator)[0];
                if (ticketData.ContainsKey(DatabaseObjects.Columns.LocationLookup) && ticketData[DatabaseObjects.Columns.LocationLookup] != DBNull.Value && ticketData[DatabaseObjects.Columns.LocationLookup] != null)
                {
                    string locationValue = Convert.ToString(ticketData[DatabaseObjects.Columns.LocationLookup]);
                    if (locationValue.Contains(Constants.Separator6))
                        locationValue = UGITUtility.SplitString(locationValue, Constants.Separator6)[0];

                    /*
                     *  prior to converting location value to long, we need to check that location value
                     *  is "Numeric" only, since it has been observed that it may contians alpha numeric values also
                     */
                    long result = 0;
                    if (UGITUtility.IsNumber(locationValue, out result))
                    {
                        currentTicket[DatabaseObjects.Columns.LocationLookup] = Convert.ToInt64(locationValue);
                    }

                }

                CheckRequestType(currentTicket, false);
            }

            // Hardcode request source to "Wizard" for all tickets
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.RequestSource, moduleInstanceList))
            {
                currentTicket[DatabaseObjects.Columns.RequestSource] = "Wizard";
            }

            //Do specific for the modules
            if (moduleName == "SVC")
            {
                FieldConfiguration serviceField = fieldConfigurationManager.GetFieldByFieldName(DatabaseObjects.Columns.ServiceTitleLookup);
                if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketOwner) && ticketData[DatabaseObjects.Columns.TicketOwner] != DBNull.Value)
                {
                    currentTicket[DatabaseObjects.Columns.TicketOwner] = ticketData[DatabaseObjects.Columns.TicketOwner];
                }

                if (ticketData.ContainsKey(DatabaseObjects.Columns.OwnerApprovalRequired) && ticketData[DatabaseObjects.Columns.OwnerApprovalRequired] != null)
                {
                    currentTicket[DatabaseObjects.Columns.OwnerApprovalRequired] = ticketData[DatabaseObjects.Columns.OwnerApprovalRequired];
                }

                if (ticketData.ContainsKey(DatabaseObjects.Columns.ServiceTitleLookup) && ticketData[DatabaseObjects.Columns.ServiceTitleLookup] != DBNull.Value)
                {

                    currentTicket[DatabaseObjects.Columns.ServiceTitleLookup] = Convert.ToInt64(ticketData[DatabaseObjects.Columns.ServiceTitleLookup]);
                }
            }

            //Set TicketImpact and Severity and columns which are drived from them. like TicketPriorityLookup
            if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketImpactLookup) && ticketData.ContainsKey(DatabaseObjects.Columns.TicketSeverityLookup))
            {
                FieldConfiguration impactField = fieldConfigurationManager.GetFieldByFieldName(DatabaseObjects.Columns.TicketImpactLookup);
                FieldConfiguration severityField = fieldConfigurationManager.GetFieldByFieldName(DatabaseObjects.Columns.TicketSeverityLookup);
                if (currentTicket[DatabaseObjects.Columns.TicketImpactLookup] != DBNull.Value && currentTicket[DatabaseObjects.Columns.TicketImpactLookup] != null)
                {

                    currentTicket[DatabaseObjects.Columns.TicketImpactLookup] = Convert.ToInt32(ticketData[DatabaseObjects.Columns.TicketImpactLookup]);
                }
                if (ticketData[DatabaseObjects.Columns.TicketSeverityLookup] != DBNull.Value && ticketData[DatabaseObjects.Columns.TicketSeverityLookup] != null)
                {
                    currentTicket[DatabaseObjects.Columns.TicketSeverityLookup] = Convert.ToInt32(ticketData[DatabaseObjects.Columns.TicketSeverityLookup]);
                }
                SetPriority(currentTicket);
            }

            // Update priority if value is not set from service.
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, currentTicket.Table) &&
                (!ticketData.ContainsKey(DatabaseObjects.Columns.TicketPriorityLookup) || UGITUtility.StringToDouble(ticketData[DatabaseObjects.Columns.TicketPriorityLookup]) <= 0))
            {
                SetTicketPriority(currentTicket, moduleName);
            }

            // If the ticket is creating from related ticket page.
            // Create Relation Ticket code moved from here because in new ticket case till here ticket is not saved in the list, therefore the relation with that ticket can't be stablish.
            // Code moved to the Module web part function after ticket saved.
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.DRQRapidTypeLookup, moduleInstanceList) &&
                UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRapidRequest, moduleInstanceList) &&
                UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, moduleInstanceList))
            {
                string drqRapidTypeLookup = Convert.ToString(currentTicket[DatabaseObjects.Columns.DRQRapidTypeLookup]);
                if (drqRapidTypeLookup != string.Empty && drqRapidTypeLookup != "0") // Shows up as 0 when not selected!
                {
                    // If Rapid Type is set to something, set to High Priority & Urgent = Yes
                    //ModulePrioirty priorityRow = Module.List_Priorities.FirstOrDefault(x => x.uPriority== Convert.ToString(moduleRequestPriorityRow[0][DatabaseObjects.Columns.TicketPriorityLookup]));// modulePrioritiesRow.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.UPriority) == Convert.ToString(moduleRequestPriorityRow[0][DatabaseObjects.Columns.TicketPriorityLookup]));
                    ModulePrioirty priorityRow = Module.List_Priorities.FirstOrDefault(x => x.ID == Module.List_PriorityMaps[0].PriorityLookup);
                    currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priorityRow.ID;
                    currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "Yes";
                }
                else
                {
                    // If Rapid Type is not set (null), set to Low Priority & Urgent = No
                    ModulePrioirty priorityRow = Module.List_Priorities.FirstOrDefault(x => x.ID == Module.List_PriorityMaps[2].PriorityLookup);
                    //DataRow priorityRow = modulePrioritiesRow.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.UPriority) == Convert.ToString(moduleRequestPriorityRow[2][DatabaseObjects.Columns.TicketPriorityLookup]));
                    currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = priorityRow.ID;
                    currentTicket[DatabaseObjects.Columns.TicketRapidRequest] = "No";
                }
            }

            // In DRQ TicketTargetCompletionDate is used and in PRS, TSR, ACR, TicketDesiredCompletionDate is entered
            //So make both fields the same
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketTargetCompletionDate, moduleInstanceList)
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketDesiredCompletionDate, moduleInstanceList))
            {
                if (currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] == DBNull.Value && currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                    currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate];
                else if (currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] == DBNull.Value && currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] != DBNull.Value)
                    currentTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] = currentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate];
            }
            if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketDescription) && ticketData.ContainsKey(DatabaseObjects.Columns.TicketDescription))
            {
                currentTicket[DatabaseObjects.Columns.TicketDescription] = ticketData[DatabaseObjects.Columns.TicketDescription];
            }

            //Assign module specific defaults which are assigned during module configuration in ModuleDefaultValues list
            AssignModuleSpecificDefaults(currentTicket);

            //Assign module specific defaults user which are assigned during module configuration in ModuleUserTypes list
            AssignModuleSpecificDefaultUsers(currentTicket);

            // Set department, division & company based on requestor's department
            SetRequestorDepartment(currentTicket);
            string historyDescription = Module.List_LifeCycles[0].Stages[0].ApproveActionDescription;

            if (moduleName != "SVC")
                historyDescription = historyDescription + " (Service Wizard)";

            if (string.IsNullOrEmpty(historyDescription))
                historyDescription = "Ticket Initiated";
            uHelper.CreateHistory(User, historyDescription, currentTicket, false, _context);

            SetTicketSkipStages(currentTicket);

            ticketActionType = TicketActionType.Created;
            return currentTicket;
        }

        private void SetPriority(DataRow currentTicket)
        {
            try
            {
                long impactLookup = UGITUtility.StringToLong(currentTicket[DatabaseObjects.Columns.TicketImpactLookup]);
                long severityLookup = UGITUtility.StringToLong(currentTicket[DatabaseObjects.Columns.TicketSeverityLookup]);



                if (Module.List_Impacts.Count > 0 && Module.List_Severities.Count > 0)
                {
                    ModuleImpact impact = Module.List_Impacts.FirstOrDefault(x => x.ID == impactLookup);
                    ModuleSeverity severity = Module.List_Severities.FirstOrDefault(x => x.ID == severityLookup);
                    if (impact != null && severity != null)
                    {
                        ModulePriorityMap map = Module.List_PriorityMaps.FirstOrDefault(x => x.ImpactLookup == impact.ID && x.SeverityLookup == severity.ID);
                        if (map != null && map.PriorityLookup > 0)
                        {
                            currentTicket[DatabaseObjects.Columns.TicketPriorityLookup] = map.PriorityLookup;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR setting priority based on impact & severity");
            }
        }

        private void SetRequestorDepartment(DataRow item)
        {
            if (!UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketRequestor) || !UGITUtility.IfColumnExists(DatabaseObjects.Columns.DepartmentLookup))
                return;

            List<UserProfile> userLookUp = _context.UserManager.GetUserInfosById(Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor]));  // new SPFieldUserValueCollection(_spWeb, Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor]));
            string requestorUserId = string.Empty;   // (userLookUp != null && userLookUp.Count > 0 ? userLookUp[0].LookupId : 0);

            int departmentId = 0;
            if (userLookUp != null && userLookUp.Count > 0)
                requestorUserId = userLookUp[0].Id;

            if (User != null)
            {
                UserProfile requestor = _context.UserManager.GetUserInfoById(requestorUserId);
                if (requestor != null)
                    departmentId = requestor.DepartmentId;
                else
                    return;
            }
            else
            {
                List<UserProfile> userListCol = _context.UserManager.GetUsersProfileWithGroup();
                if (userListCol != null)
                    userListCol = userListCol.Where(x => x.Id == requestorUserId).ToList();
                if (userListCol != null && userListCol.Count > 0)
                {
                    departmentId = userListCol[0].DepartmentId;
                }
            }
            //if (SPContext.Current != null)
            //{
            //    UserProfile requestor = UserProfile.LoadById(requestorUserId, _spWeb);
            //    if (requestor != null)
            //        departmentId = requestor.DepartmentId;
            //    else
            //        return;
            //}
            //else
            //{
            //    SPQuery queryUser = new SPQuery();
            //    queryUser.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}' Nullable='True'/>", DatabaseObjects.Columns.Id),
            //                                         string.Format("<FieldRef Name='{0}' Nullable='True'/>", DatabaseObjects.Columns.DepartmentLookup));
            //    queryUser.ViewFieldsOnly = true;
            //    queryUser.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", "ID", requestorUserId, "ContentType", "Person");
            //    SPListItemCollection userListCol = _spWeb.SiteUserInfoList.GetItems(queryUser);

            //    if (userListCol != null && userListCol.Count > 0)
            //    {
            //        if (userListCol[0][DatabaseObjects.Columns.DepartmentLookup] != null)
            //        {
            //            SPFieldLookupValue deptLookup = new SPFieldLookupValue(Convert.ToString(userListCol[0][DatabaseObjects.Columns.DepartmentLookup]));
            //            if (deptLookup != null)
            //                departmentId = deptLookup.LookupId;
            //        }
            //    }
            //    else
            //    {
            //        SPGroup group = UserProfile.GetGroupByID(requestorUserId, _spWeb);
            //        if (group != null && group.Users.Count != 0)
            //        {
            //            SPQuery queryGroupUser = new SPQuery();
            //            queryGroupUser.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}' Nullable='True'/>", DatabaseObjects.Columns.Id),
            //                                                      string.Format("<FieldRef Name='{0}' Nullable='True'/>", DatabaseObjects.Columns.DepartmentLookup));
            //            queryGroupUser.ViewFieldsOnly = true;
            //            queryGroupUser.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", "ID", group.Users[0].ID);
            //            SPListItemCollection GroupUserListCol = _spWeb.SiteUserInfoList.GetItems(queryGroupUser);
            //            if (GroupUserListCol != null && GroupUserListCol.Count > 0)
            //            {
            //                if (GroupUserListCol[0][DatabaseObjects.Columns.DepartmentLookup] != null)
            //                {
            //                    SPFieldLookupValue deptLookup = new SPFieldLookupValue(Convert.ToString(GroupUserListCol[0][DatabaseObjects.Columns.DepartmentLookup]));
            //                    if (deptLookup != null)
            //                        departmentId = deptLookup.LookupId;
            //                }
            //            }
            //        }
            //    }
            //}

            if (departmentId > 0)
            {
                DepartmentManager departmentManager = new DepartmentManager(_context);
                List<Department> listDepartment = departmentManager.Load(); // uGITCache.LoadDepartments(_spWeb);
                Department filteredDepartment = listDepartment.FirstOrDefault(d => d.ID == departmentId);
                // Only set if not already set in front-end
                if (filteredDepartment != null)
                {
                    if (item[DatabaseObjects.Columns.DepartmentLookup] == null && filteredDepartment.ID > 0)
                        item[DatabaseObjects.Columns.DepartmentLookup] = UGITUtility.StringToInt(filteredDepartment.ID);

                    if (item[DatabaseObjects.Columns.CompanyTitleLookup] == null && filteredDepartment.CompanyLookup != null && filteredDepartment.CompanyIdLookup > 0)
                        item[DatabaseObjects.Columns.CompanyTitleLookup] = filteredDepartment.CompanyLookup.ID;

                    if (item[DatabaseObjects.Columns.DivisionLookup] == null && filteredDepartment.DivisionLookup != null && filteredDepartment.DivisionLookup != null)
                        item[DatabaseObjects.Columns.DivisionLookup] = filteredDepartment.DivisionLookup.ID;
                }
            }
        }
        public void NotifyOnElevateTicket(DataRow ticketItem, string newPriority, string oldPriority)
        {
            if (string.IsNullOrWhiteSpace(newPriority))
                return;

            // Check if any notifications configured for this priority value           
            ModulePrioirty priority = this.Module.List_Priorities.FirstOrDefault(x => x.ID == Convert.ToInt64(newPriority) && !string.IsNullOrWhiteSpace(x.EmailIDTo));
            if (priority == null)
                return;

            List<string> emails = UGITUtility.ConvertStringToList(priority.EmailIDTo, new string[] { ",", ";" });
            if (emails.Count > 0)
            {
                string subject = string.Format("{0} Priority Ticket Created: {1}", priority.Title, ticketItem[DatabaseObjects.Columns.Title]);
                string url = string.Format("{0}?TicketId={1}&ModuleName={2}", UGITUtility.GetAbsoluteURL(Constants.HomePage), Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]), moduleName);
                url = string.Format("<a href='{0}'>{1}</a>", url, ticketItem[DatabaseObjects.Columns.Title]);
                string body = string.Empty;
                string ticketId = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
                string title = Convert.ToString(ticketItem[DatabaseObjects.Columns.Title]);
                string emailBodyTemp = string.Empty;
                string mails = string.Join("; ", emails.ToArray());

                if (priority.NotifyInPlainText)
                {
                    string lineBreak = Environment.NewLine;
                    subject = string.Format("{0} Priority Ticket Created", priority.Title);
                    body = string.Format("A ticket with priority {0} has been created.{1}{1} ", priority.Title, lineBreak);
                    body += string.Format("{0}: {1}{2} ", ticketId, title, lineBreak);
                    body += string.Format("Requestor: {0}{1} ", UGITUtility.GetSPItemValueAsString(ticketItem, DatabaseObjects.Columns.TicketRequestor, true), lineBreak);
                    body += string.Format("Location: {0}{1} ", UGITUtility.GetSPItemValueAsString(ticketItem, DatabaseObjects.Columns.LocationLookup, true), lineBreak);
                    body += string.Format("Request Type: {0} ", uHelper.FormatRequestType(_context, ticketItem));
                    emailBodyTemp = body;
                }
                else
                {
                    string greeting = objConfigurationVariableHelper.GetValue("Greeting");
                    string signature = objConfigurationVariableHelper.GetValue("Signature");
                    body = string.Format("A ticket with priority <b>{0}</b> has been created: {1}.<br><br> Please see details below and take any appropriate action to close the ticket.", newPriority, url);
                    emailBodyTemp = string.Format(@"{0} <br/><br/>
                                                {1}<br/><br/>
                                                {2}<br/>", greeting, body, signature);
                    emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(_context, ticketItem, this.Module.ModuleName, true, false));
                }
                MailMessenger mail = new MailMessenger(_context);
                mail.SendMail(mails, subject, string.Empty, emailBodyTemp, true, new string[] { }, true);
            }
        }

        public void Recycle(DataRow dataRow)
        {

            if (UGITUtility.IsSPItemExist(dataRow, DatabaseObjects.Columns.Deleted))
                dataRow[DatabaseObjects.Columns.Deleted] = true;
            //if (UGITUtility.IsSPItemExist(dataRow, DatabaseObjects.Columns.IsDeleted))
            //    dataRow[DatabaseObjects.Columns.IsDeleted] = true;

        }
        public void ArchiveTickets(DataRow dataRow)
        {
            UGITModule uModule = null;
            if (UGITUtility.IsSPItemExist(dataRow, DatabaseObjects.Columns.TicketId))
            {
                string module = uHelper.getModuleNameByTicketId(UGITUtility.GetSPItemValueAsString(dataRow, DatabaseObjects.Columns.TicketId));
                uModule = moduleMgr.GetByName(module);
            }
            if (uModule != null)
            {
                long oldTicketId = Convert.ToInt64(dataRow[DatabaseObjects.Columns.ID]);
                dataRow.AcceptChanges();
                dataRow.SetAdded();
                ticketManager.SaveArchive(uModule, dataRow);

                DataRow oldTicket = ticketManager.GetByID(uModule, oldTicketId);
                ticketManager.Delete(uModule, dataRow);
            }
        }

        private Boolean CheckUserOrGroupIsITManagerType(FieldConfiguration f, string value, string moduleName)
        {
            Boolean valid = true;
            Boolean IsITManagerValid = false;
            //FieldConfigurationManager fieldManager = new FieldConfigurationManager(_context);
            //FieldConfiguration field = fieldManager.GetFieldByFieldName(f.FieldName);
            if (!string.IsNullOrEmpty(value) && value.Trim() != string.Empty)
            {
                ModuleUserTypeManager userTypeManager = new ModuleUserTypeManager(_context);
                DataTable moduleUserTypesTable = userTypeManager.GetDataTable();  // uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserTypes);



                string query = string.Format("{0} = '{1}' And ( {2} = '{3}' OR {2} = '{4}' )", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.ColumnName, f.FieldName, f.FieldName);
                DataRow[] moduleUserTypes = moduleUserTypesTable.Select(query);
                if (moduleUserTypes != null && moduleUserTypes.Length > 0)
                {
                    if (Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.ITOnly]) == "True" ||
                        Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.ManagerOnly]) == "True")
                    {
                        IsITManagerValid = true;
                    }
                }

                UserProfileManager userManager = new UserProfileManager(_context);
                UserProfile spUser = null;
                UserProfile spGroup = null;
                List<UserProfile> uCollection = userManager.GetUserInfosById(value);  // new SPFieldUserValueCollection(spWeb, value);
                if (uCollection != null && uCollection.Count > 0)
                {
                    //int uID = uCollection[0].LookupId;
                    spUser = uCollection[0];

                    if (spUser.isRole == true)
                        spGroup = spUser;
                }

                if (spUser != null && IsITManagerValid)
                {
                    //UserProfile user = UserProfile.LoadById(spUser.ID, spWeb);

                    if (moduleUserTypes.Length > 0)
                    {
                        if (moduleUserTypes[0][DatabaseObjects.Columns.Groups] != null
                            && Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]) != string.Empty)
                        {
                            string[] groups = Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]).Split(';');

                            foreach (string s in groups)
                            {
                                if (groups.Length > 0 && spUser != null && userManager.CheckUserIsInGroup(s, spUser))
                                {
                                    if (spUser.IsManager == true)
                                        valid = true;
                                }
                                else
                                {
                                    valid = false;
                                }
                            }
                        }
                        else
                        {
                            valid = spUser.IsManager;  // UserProfile.CheckUserISManager(moduleUserTypes[0], user);
                        }
                    }
                }
                else if (spGroup != null)
                {
                    List<UserProfile> userlist = userManager.GetUsersByGroupID(spGroup.Id);
                    if (moduleUserTypes.Length > 0)
                    {
                        if (moduleUserTypes[0][DatabaseObjects.Columns.Groups] != null
                            && Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]) != string.Empty)
                        {
                            string[] groups = Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]).Split(';');
                            if (groups.Length > 0 && groups.Contains(spGroup.Name))
                            {
                                valid = true;
                            }
                            else
                            {
                                valid = false;
                            }
                        }

                    }
                }
                else if (!IsITManagerValid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return valid;
        }

        public static bool PriorityMappingEnabled(UGITModule module)
        {
            return (module.List_PriorityMaps != null && module.List_PriorityMaps.Count > 0);
        }

        public bool PriorityMappingEnabled()
        {
            return (Module.List_PriorityMaps != null && Module.List_PriorityMaps.Count > 0);
        }

        public bool CheckMandatoryFieldForStage(LifeCycleStage currentStage, DataRow currenticket)
        {
            bool stageHasMandatoryFields = false;
            ModuleRoleWriteAccess[] roleWriteRows = this.Module.List_RoleWriteAccess.Where(x => x.StageStep == currentStage.StageStep && x.FieldMandatory).ToArray();
            if (roleWriteRows.Length >= 1)
            {
                foreach (ModuleRoleWriteAccess roleWriteRow in roleWriteRows)
                {
                    string columnName = roleWriteRow.FieldName;
                    bool dependentField = UGITUtility.IsDependentMandatoryField(moduleName, columnName);
                    if (uHelper.IfColumnExists(columnName, currenticket.Table) && !dependentField)
                    {
                        if (Convert.ToString(currenticket[columnName]) == string.Empty)
                        {
                            if (!HasModuleSpecificDefault(currenticket, currentStage.ID, columnName))
                            {
                                stageHasMandatoryFields = true;
                                break;
                            }
                        }
                    }
                }
            }
            return stageHasMandatoryFields;
        }


        public void RejectTicket(List<TicketColumnError> errors, DataRow saveTicket, string comment, UserProfile asUser = null, string performedActionName = "")
        {
            if (asUser == null)
                asUser = _context.CurrentUser;
            Comment = comment;

            //string rejectComments = UGITUtility.WrapComment(comment, "reject");

            // Perform reject action on main ticket
            Reject(saveTicket, comment, asUser);
            string error = CommitChanges(saveTicket);

            if (string.IsNullOrEmpty(error))
            {
                // Perform reject/cancel on SVC sub-tickets
                if (moduleName == "SVC")
                {
                    LifeCycleStage currentStage = GetTicketCurrentStage(saveTicket);
                    if (currentStage != null && currentStage.StageTypeChoice != null && currentStage.StageTypeChoice.Equals(StageType.Closed.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        string resolutionType = UGITUtility.GetSPItemValueAsString(saveTicket, DatabaseObjects.Columns.TicketResolutionType);

                        UserProfile prp = uHelper.GetUser(_context, saveTicket, DatabaseObjects.Columns.TicketPRP);
                        TicketRelationManager ctont = new TicketRelationManager(_context);
                        TicketRelationshipHelper rHelper = new TicketRelationshipHelper(_context);
                        //the value 3 tells us that it is parent or child, now child value is 3 instead of 2 because of Sibling Item(s) enhancement
                        rHelper.CloseTickets(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), 3, comment, prp, resolutionType);
                    }
                }

                SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), saveTicket, Convert.ToString(Module.ModuleId), performedAction: performedActionName);

                // Delete any pending escalations for this ticket
                // Got to do it here since escalations block below may not be executed if not valid
                EscalationProcess esProcess = new EscalationProcess(_context);
                esProcess.DeleteEscalation(saveTicket);
            }
            else
            {
                errors.Add(TicketColumnError.AddError(error));
            }
        }


        public void ApproveTicket(List<TicketColumnError> errors, DataRow saveTicket, bool adminOverride, UserProfile asUser = null, bool autoStageMove = false)
        {
            LifeCycleStage oldCurrentStage = GetTicketCurrentStage(saveTicket);
            if (asUser == null)
                asUser = _context.CurrentUser;

            string error = Approve(asUser, saveTicket, adminOverride, autoStageMove);
            if (string.IsNullOrEmpty(error))
            {
                AssignModuleSpecificDefaults(saveTicket);
                error = CommitChanges(saveTicket);
                if (string.IsNullOrEmpty(error))
                {
                    LifeCycleStage currentStage = GetTicketCurrentStage(saveTicket);

                    //Send Email notification
                    bool sendNotification = true;
                    if (currentStage == oldCurrentStage && currentStage.StageAllApprovalsRequired)
                        sendNotification = false;
                    if (sendNotification)
                        SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), saveTicket, moduleName, performedAction: Convert.ToString(TicketActionType.Approved));

                    //This will ensure start date and end date are updated if they have previous dates from template
                    UGITModuleConstraint.ConfigureCurrentModuleStageTask(_context, currentStage.StageStep + 1, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                }
            }

            if (!string.IsNullOrEmpty(error))
                errors.Add(TicketColumnError.AddError(error));
        }

        // NOT USED ANY MORE: Cancel/Reject Notifications are based on "Reject" event type only going forward
        /// <summary>
        ///Default Reject notifiction format, Send notification
        /// </summary>
        //protected void RejectNotification(DataRow saveTicket, string comment)
        //{
        //    string ticketId = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
        //    string title = Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]);
        //    string moduleType = UGITUtility.moduleTypeName(this.Module.ModuleName);
        //    string subject = string.Format("{0} Cancelled: {1}", moduleType, title);
        //    string mailBody = string.Format("The {0} <b>{1}</b> was cancelled. <br/><br/>{2}", moduleType, title, UGITUtility.WrapCommentForEmail(comment, "reject"));
        //    if (Module.ActionUserNotificationOnCancel)
        //        SendEmailToActionUsers(saveTicket, subject, mailBody);
        //    if (Module.RequestorNotificationOnCancel)
        //        SendEmailToRequestor(saveTicket, subject, mailBody);
        //    if (Module.InitiatorNotificationOnCancel)
        //        SendEmailToInitiator(saveTicket, subject, mailBody);
        //}

        #region Method to fetch data for a particular Email Notification
        /// <summary>
        /// This method is used to get the data row from Email Notifications for a particular Module on the basis of Stage Title and Performed Actions
        /// </summary>
        /// <param name="taskEmails"></param>
        /// <param name="stageTitle"></param>
        /// <param name="performedAction"></param>
        /// <returns></returns>
        protected List<ModuleTaskEmail> GetEmailNotifications(List<ModuleTaskEmail> taskEmails, int stageStep, string performedAction)
        {
            List<ModuleTaskEmail> notifications = null;
            List<ModuleTaskEmail> moduleStageNotifications = taskEmails.Where(x => x.StageStep.HasValue && x.StageStep.Value == stageStep).ToList();

            if (performedAction == Convert.ToString(TicketActionType.Approved) || performedAction == Convert.ToString(TicketActionType.Returned))
            {
                // See if we have any entries for specific event type
                notifications = moduleStageNotifications.Where(x => x.EmailEventType == performedAction).ToList();

                // Else use entry with null event type (for backwards compatibility)
                if (notifications == null || notifications.Count == 0)
                    notifications = moduleStageNotifications.Where(x => string.IsNullOrEmpty(x.EmailEventType)).ToList();

                // Else default to entry of type "Approved"
                if (notifications == null || notifications.Count == 0)
                    notifications = moduleStageNotifications.Where(x => x.EmailEventType == Convert.ToString(TicketActionType.Approved)).ToList();
            }
            else if (performedAction == Convert.ToString(TicketActionType.Rejected))
            {
                // For Cancel/Reject we want specific event type notifications for that stage only
                notifications = moduleStageNotifications.Where(x => x.EmailEventType == performedAction).ToList();
            }
            else if (performedAction == Convert.ToString(TicketActionType.Created) || performedAction == Convert.ToString(TicketActionType.OnHold) ||
                     performedAction == Convert.ToString(TicketActionType.Elevated))
            {
                // We don't create Email Notifications for Created, Rejected & for OnHold at Stage Level, instead we create single Default Entries for them. 
                notifications = taskEmails.AsEnumerable().Where(x => x.EmailEventType == performedAction).ToList();
            }

            return notifications;
        }

        #endregion Method to fetch data for a particular Email Notification

        /// <summary>
        /// This method is used to calculate TicketAge for a Ticket
        /// </summary>
        /// <param name="dRow">Data row containing the details to calculate TicketAge</param>
        /// <param name="ticketAgeExcludesHoldTime">A boolean value from the config variable TicketAgeExcludesHoldTime</param>
        /// <param name="workingHoursInDay">indicates total working hours in day</param>
        /// <returns></returns>
        public static double GetTicketAge(ApplicationContext context, DataRow ticket, int workingHoursInADay, bool ticketAgeExcludesHoldTime = false, string colNamePrefix = "")
        {

            // Get Ticket creation date
            DateTime? ticketCreationDate;
            if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketCreationDate, ticket.Table))
                ticketCreationDate = UGITUtility.StringToDateTime(ticket[colNamePrefix + DatabaseObjects.Columns.TicketCreationDate]);
            else
                ticketCreationDate = UGITUtility.StringToDateTime(ticket[colNamePrefix + DatabaseObjects.Columns.Created]);

            // Get Ticket close date if it exists
            DateTime? ticketCloseDate = null;
            if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketClosed, ticket.Table) && UGITUtility.StringToBoolean(ticket[colNamePrefix + DatabaseObjects.Columns.TicketClosed]))
            {
                if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketCloseDate, ticket.Table))
                    ticketCloseDate = UGITUtility.StringToDateTime(ticket[colNamePrefix + DatabaseObjects.Columns.TicketCloseDate]);
                else if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.ClosedDate, ticket.Table))
                    ticketCloseDate = UGITUtility.StringToDateTime(ticket[colNamePrefix + DatabaseObjects.Columns.ClosedDate]);
            }

            // Get Ticket Age
            double ticketAge = 0;
            if (ticketCreationDate.HasValue && ticketCreationDate.Value != DateTime.MinValue)
            {
                if (ticketCloseDate.HasValue && ticketCloseDate.Value != DateTime.MinValue)
                    ticketAge = uHelper.GetTotalWorkingDaysBetween(context, ticketCreationDate.Value.Date, ticketCloseDate.Value.Date);
                else
                    ticketAge = uHelper.GetTotalWorkingDaysBetween(context, ticketCreationDate.Value.Date, DateTime.Today);

                // Adjust so it counts same-day closure as 0 days instead of 1
                if (ticketAge > 0)
                    ticketAge -= 1;
            }

            // Recalculate TicketAge column values if following condition is true
            if (ticketAgeExcludesHoldTime)
            {
                double totalHoldDuration = GetTotalHoldTime(context, ticket, false);

                if (totalHoldDuration <= 0)
                    return ticketAge;

                double holdDurationInDays = totalHoldDuration / (60 * workingHoursInADay);

                if (holdDurationInDays > 0)
                    ticketAge = Math.Round(ticketAge - holdDurationInDays, 0);
            }

            return ticketAge;
        }

        /// <summary>
        /// This method is used to get Total hold time including any current hold time
        /// </summary>
        /// <param name="dRow">Data row containing the details to calculate TotalHoldTime</param>
        /// <param name="inHours">Return in hours if set to true, else in minutes</param>
        /// <param name="colNamePrefix">Optional parameter which is needed when this function is invoked from Query Wizard where column names have TableName as prefix</param>
        /// <returns></returns>
        public static double GetTotalHoldTime(ApplicationContext context, DataRow dRow, bool inHours, string colNamePrefix = "")
        {
            double totalHoldTime = 0;

            if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketTotalHoldDuration, dRow.Table))
            {
                totalHoldTime = UGITUtility.StringToDouble(dRow[colNamePrefix + DatabaseObjects.Columns.TicketTotalHoldDuration]);

                if (uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketOnHold, dRow.Table) && UGITUtility.StringToBoolean(dRow[colNamePrefix + DatabaseObjects.Columns.TicketOnHold]) && uHelper.IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketOnHoldStartDate, dRow.Table))
                {
                    double currentHoldMinutes = uHelper.GetWorkingMinutesBetweenDates(context, UGITUtility.StringToDateTime(dRow[colNamePrefix + DatabaseObjects.Columns.TicketOnHoldStartDate]), DateTime.Now, isSLA: true);
                    totalHoldTime += currentHoldMinutes;
                }
            }

            if (inHours)
                return TimeSpan.FromMinutes(totalHoldTime).TotalHours;

            return totalHoldTime;
        }


        private string GetAddress(DataRow item)
        {
            string Address = string.Empty;

            if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.StreetAddress1) && item[DatabaseObjects.Columns.StreetAddress1] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.StreetAddress1])))
            {
                Address = $"{item[DatabaseObjects.Columns.StreetAddress1]} ";
            }

            if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.City) && item[DatabaseObjects.Columns.City] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.City])))
            {
                Address += $"{item[DatabaseObjects.Columns.City]}, ";
            }

            if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.StateLookup) && item[DatabaseObjects.Columns.StateLookup] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.StateLookup])))
            {
                stateManager = new StateManager(_context);
                var state = stateManager.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.StateLookup]));
                if (state != null)
                {
                    Address += $"{state.StateCode} ";
                }
            }

            if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.Zip) && item[DatabaseObjects.Columns.Zip] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.Zip])))
            {
                Address += $"{item[DatabaseObjects.Columns.Zip]}";
            }

            return Address;
        }
        private string GetProjectIdExpression()
        {
            string ProjectIDFormat = objConfigurationVariableHelper.GetValue("ProjectIDFormat");

            if (string.IsNullOrEmpty(ProjectIDFormat))
                return string.Empty;

            string[] arrProjectIDFormat = ProjectIDFormat.Split('-');
            string YearSubstring = arrProjectIDFormat.Where(x => x.Contains("Y")).FirstOrDefault();
            string NumSubstring = arrProjectIDFormat.Where(x => x.Contains("N")).FirstOrDefault();

            string regex = string.Empty;

            if (arrProjectIDFormat.FirstOrDefault().EqualsIgnoreCase(YearSubstring))
                regex = @"^\d{" + $"{1},{YearSubstring.Length}" + "}\\-?\\d{" + $"{1},{NumSubstring.Length}" + "}$";
            else
                regex = @"^\d{" + $"{1},{NumSubstring.Length}" + "}\\-?\\d{" + $"{1},{YearSubstring.Length}" + "}$";

            return regex;
        }
        private string GetPriorityFromItem(DataRow item)
        {
            string priority = string.Empty;
            if (item != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, item.Table))
            {
                long priortyLookup = UGITUtility.StringToLong(item[DatabaseObjects.Columns.TicketPriorityLookup]);
                if (priortyLookup > 0)
                {
                    ModulePrioirty modulePrioirty = this.Module.List_Priorities.FirstOrDefault(x => x.ID == priortyLookup);
                    if (modulePrioirty != null)
                        priority = modulePrioirty.Title;
                }
            }
            return priority;
        }
        private bool IsDifferentPriorityNotification(ModuleTaskEmail row, string priority)
        {
            string itemPriority = Convert.ToString(row.TicketPriorityLookup);
            if ((!string.IsNullOrEmpty(itemPriority) && itemPriority != "0") && !priority.Equals(itemPriority))
                return true;

            return false;
        }
        public DataRow UpdateTicketFromEmail(string title, string description, UserProfile user, Email ticketEmailItem, string modulename, DataRow item, string from = "")
        {
            if (string.IsNullOrEmpty(modulename))
                return null;

            //DataRow saveTicket = null;
            MatchCollection mColl = uHelper.GetAllTokens(_context, description);
            uHelper.InsertTokenInItem(_context, modulename, uHelper.GetTokenValueArray(_context, mColl), item);

            //To Remove tokens from description when save in ticket table
            string descAfterRemoveToken = description;
            foreach (Match token in mColl)
            {
                descAfterRemoveToken = descAfterRemoveToken.Replace(token.Value, "");
            }
            descAfterRemoveToken = descAfterRemoveToken.Trim();

            item[DatabaseObjects.Columns.Title] = title;
            item[DatabaseObjects.Columns.TicketDescription] = descAfterRemoveToken;
            if (UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.IsTicketAttachment))
                item[DatabaseObjects.Columns.IsTicketAttachment] = false;

            // Set requestor, requestor location & business manager
            UserProfile userInfoItem = user;


            if (user != null)
            {
                item[DatabaseObjects.Columns.TicketRequestor] = user.Id;

                if (userInfoItem != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.LocationLookup, item.Table) &&
                    string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.LocationLookup])))
                {
                    item[DatabaseObjects.Columns.LocationLookup] = userInfoItem.LocationId;
                }

                // If email sender is valid site user, then set business manager to user's manager (if not already set)
                if (userInfoItem != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketBusinessManager, item.Table) &&
                    string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.TicketBusinessManager])) &&
                    !string.IsNullOrWhiteSpace(Convert.ToString(userInfoItem.ManagerID)))
                {
                    item[DatabaseObjects.Columns.TicketBusinessManager] = Convert.ToString(userInfoItem.ManagerID);
                }
            }
            // Set the RequestTypeLookup according to the "Keywords" available in the mail body or in title.
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup, item.Table) && item[DatabaseObjects.Columns.TicketRequestTypeLookup] == null)
            {
                DataTable requestTypeData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"TenantID='{_context.TenantID}'");
                DataRow[] requestTypeDatacoll = requestTypeData.Select(string.Format("{0}='{1}' And ({2}<>isnull Or {3}<>isnull)", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.KeyWords, DatabaseObjects.Columns.EmailToTicketSender));
                string titleStr = title.ToLower();
                string descriptionStr = description.ToLower();
                DataRow requestTypeRow = null;
                if (requestTypeData != null)
                {
                    if (!string.IsNullOrWhiteSpace(from))
                    {
                        from = from.ToLower();
                        foreach (DataRow requestType in requestTypeData.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(Convert.ToString(requestType[DatabaseObjects.Columns.EmailToTicketSender])))
                                continue;
                            if (Convert.ToString(requestType[DatabaseObjects.Columns.EmailToTicketSender]).ToLower().IndexOf(from) != -1)
                            {
                                requestTypeRow = requestType;
                                ULog.WriteLog(string.Format("Matched configured sender [{0}] in email, matched to request type {1}", from, requestType[DatabaseObjects.Columns.Title]));
                                break;
                            }
                        }
                    }
                    if (requestTypeRow == null)
                    {
                        foreach (DataRow requestType in requestTypeData.Rows)
                        {
                            if (string.IsNullOrWhiteSpace(Convert.ToString(requestType[DatabaseObjects.Columns.KeyWords])))
                                continue;
                            bool matchall = UGITUtility.StringToBoolean(requestType[DatabaseObjects.Columns.KeyWords]);
                            string[] keyWordsList = UGITUtility.SplitString(requestType[DatabaseObjects.Columns.KeyWords], ";", StringSplitOptions.RemoveEmptyEntries);
                            if (keyWordsList != null && keyWordsList.Length > 0)
                            {
                                if (matchall == true)
                                {
                                    bool valid = false;
                                    foreach (string keyword in keyWordsList)
                                    {
                                        if (titleStr.Contains(keyword.ToLower()) || descriptionStr.Contains(keyword.ToLower()))
                                            valid = true;
                                        else
                                        {
                                            valid = false;
                                            break;
                                        }
                                    }
                                    if (valid)
                                    {
                                        requestTypeRow = requestType;
                                        ULog.WriteLog(string.Format("Found all keywords in email, matched to request type {0}", requestType[DatabaseObjects.Columns.Title]));
                                    }
                                }
                                else
                                {
                                    foreach (string keyword in keyWordsList)
                                    {
                                        if (titleStr.Contains(keyword.ToLower()) || descriptionStr.Contains(keyword.ToLower()))
                                        {
                                            requestTypeRow = requestType;
                                            ULog.WriteLog(string.Format("Found keyword [{0}] in email, matched to request type {1}", keyword, requestType[DatabaseObjects.Columns.Title]));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (requestTypeRow != null)
                {
                    item[DatabaseObjects.Columns.TicketRequestTypeLookup] = requestTypeRow[DatabaseObjects.Columns.Id];

                    // Set business manager field value if not already set
                    // NOTE: we want to do this only when request type has been set to allow the ticket to move forward.
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketBusinessManager, item.Table) &&
                        string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.TicketBusinessManager])))
                    {
                        string managerLookup = null;

                        // Use system account's (current user's) manager
                        UserProfile systemAccItem = user;//SPListHelper.GetSPListItem(oWeb.SiteUserInfoList, oWeb.CurrentUser.ID);
                        if (systemAccItem != null && !string.IsNullOrWhiteSpace(Convert.ToString(systemAccItem.ManagerID)))
                            managerLookup = Convert.ToString(systemAccItem.ManagerID);

                        // else set to system account (current user)
                        if (managerLookup == null)
                            managerLookup = string.Format("{0}{1}{2}", user.Id, Constants.Separator, User.Name);

                        item[DatabaseObjects.Columns.TicketBusinessManager] = user.ManagerID;
                    }
                }
            }



            // Need to set Priority before we update the ticket (Emails were showing priority Id instead of the value)
            SetTicketPriority(item, modulename);
            string query = $"{DatabaseObjects.Columns.ModuleNameLookup}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";
            DataRow[] moduleRoleWriteAccessRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestRoleWriteAccess, query).Select();
            //Get all mandatory fields for this stage.
            DataRow[] roleWriteRows = moduleRoleWriteAccessRow.Where(x => x.Field<Int32>(DatabaseObjects.Columns.StageStep) == 1 && x.Field<Boolean>(DatabaseObjects.Columns.FieldMandatory) == true).ToArray();
            //Even if we have one mandatory field, we cannot AutoApprove it.
            bool stageHasMandatoryFields = false;
            if (roleWriteRows.Length >= 1)
            {
                foreach (DataRow roleWriteRow in roleWriteRows)
                {
                    string columnName = Convert.ToString(roleWriteRow[DatabaseObjects.Columns.FieldName]);
                    bool dependentField = UGITUtility.IsDependentMandatoryField(modulename, columnName);
                    if (UGITUtility.IfColumnExists(columnName, item.Table) && !dependentField)
                    {
                        if (Convert.ToString(item[columnName]) == string.Empty)
                        {
                            stageHasMandatoryFields = true;
                            break;
                        }
                    }
                }
            }

            if (!stageHasMandatoryFields)
                QuickClose(uHelper.getModuleIdByModuleName(_context, modulename), item, "Create");

            // If we are still in first stage (because no keyword match to request type, missing mandatory fields, etc.), then set action user to help desk
            if (Convert.ToString(item[DatabaseObjects.Columns.StageStep]) == "1")
            {
                string helpDeskGroup = _context.ConfigManager.GetValue("HelpDeskGroup");
                item[DatabaseObjects.Columns.TicketStageActionUsers] = helpDeskGroup;
                item[DatabaseObjects.Columns.TicketStageActionUserTypes] = helpDeskGroup;
            }

            this.CommitChanges(item);

            return item;
        }

        public object GetAcquisitionCost(DataRow currentTicket)
        {
            object AcquisitionCost = null;
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("TenantID", _context.TenantID);
            arrParams.Add("TicketID", currentTicket[DatabaseObjects.Columns.TicketId]);
            arrParams.Add("Module", moduleName);
            DataTable dt = uGITDAL.ExecuteDataSetWithParameters("GetAcquisitionCost", arrParams);
            if (dt != null && dt.Rows.Count > 0)
            {
                AcquisitionCost = dt.Rows[0]["AcquisitionCost"];
            }

            return AcquisitionCost;
        }

        public DataTable GetForecastAndAcquisitionCosts(DataRow currentTicket)
        {
            DataTable data = new DataTable();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("TenantID", _context.TenantID);
            arrParams.Add("TicketID", currentTicket[DatabaseObjects.Columns.TicketId]);
            DataTable dt = uGITDAL.ExecuteDataSetWithParameters("GetForecastAndAcquisitionCosts", arrParams);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt;
            }

            return data;
        }

        public static void SetStageSpecificFields(ApplicationContext context, DataRow listItem, LifeCycleStage currentStage)
        {
            if (currentStage == null)
                return;

            string actionUserTypes = string.Join(Constants.Separator, currentStage.ActionUser.ToArray());
            listItem[DatabaseObjects.Columns.TicketStatus] = currentStage.Name;
            listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = actionUserTypes;
            listItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetActionUsersList(context, listItem);
            listItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
            if (listItem.Table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
                listItem[DatabaseObjects.Columns.ModuleStepLookup] = currentStage.ID;

            if (currentStage.StageTypeChoice != null && currentStage.StageTypeChoice == StageType.Closed.ToString())
                MarkTicketClosed(context, listItem);
            else
                MarkTicketNotClosed(listItem);
        }
        public static void MarkTicketNotClosed(DataRow currentTicket)
        {
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.TicketClosed] = 0;
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketRejected, currentTicket.Table))
                currentTicket[DatabaseObjects.Columns.TicketRejected] = 0;
        }
        /// <summary>
        /// Keep priority change to send notification 
        /// </summary>
        public class PriorityNotification
        {
            public string OldTicketStatus { get; set; }
            public string NewTicketStatus { get; set; }
            public string OldPriority { get; set; }
            public string NewPriority { get; set; }
        }
        private void ConfigureModuleStageTaskInTicket(UGITModule uGITModule, string currentTicketPublicID)
        {
            DataRow ticket = ticketManager.GetByTicketID(uGITModule, currentTicketPublicID);
            UGITModuleConstraint.CreateModuleStageTasksInTicket(_context, currentTicketPublicID, uGITModule.ModuleName, Convert.ToString(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.WorkflowSkipStages)));
            UGITModuleConstraint.ConfigureCurrentModuleStageTask(_context, ticket);
        }
    }
}


