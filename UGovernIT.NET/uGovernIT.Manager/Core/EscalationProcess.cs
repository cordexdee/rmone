using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using uGovernIT.Utility;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class EscalationProcess
    {
        ScheduleActionsManager objScheduleActionsManager;
        ConfigurationVariableManager objConfigurationVariableHelper;
        UserProfileManager UserManager;
        ApplicationContext context = null;
        ModuleViewManager ObjModuleViewManager = null;
        public EscalationProcess(ApplicationContext _context)
        {
              context = _context;
              UserManager = _context.UserManager;
              objScheduleActionsManager = new ScheduleActionsManager(_context);
              objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
              ObjModuleViewManager = new ModuleViewManager(_context);

        }
        public void DeleteEscalation(DataRow ticket)
        {
            DeleteEscalation(ticket, false);
        }
        public void DeleteEscalation(DataRow ticket, bool matchingOnly)
        {
            string ticketId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            string condition = $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.ActionType}='{ScheduleActionType.EscalationEmail}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
            DataTable scheduleAction = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SchedulerActions, condition);

            if (scheduleAction.Rows.Count > 0)
            {
                List<int> deleteIds = new List<int>();
                foreach (DataRow scheduleActionItem in scheduleAction.Rows)
                {
                    if (matchingOnly)
                    {
                        int SLAId = 0;
                        Dictionary<string, string> customProperty = UGITUtility.GetCustomProperties(Convert.ToString(scheduleActionItem[DatabaseObjects.Columns.CustomProperties]));

                        if (customProperty.ContainsKey(CustomProperties.SLARuleId))
                        {
                            SLAId = Convert.ToInt32(customProperty[CustomProperties.SLARuleId]);
                        }

                        condition = $"{DatabaseObjects.Columns.Id}={SLAId} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
                        DataRow slaRuleItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, condition).Select().First();
                        if (slaRuleItem != null)
                        {
                            int endStageId = !string.IsNullOrEmpty(Convert.ToString(slaRuleItem[DatabaseObjects.Columns.EndStageTitleLookup])) ? Convert.ToInt32(slaRuleItem[DatabaseObjects.Columns.EndStageTitleLookup]): 0;
                            int stageId = !string.IsNullOrEmpty(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup])) ? Convert.ToInt32(ticket[DatabaseObjects.Columns.ModuleStepLookup]) : 0;
                            
                            if (stageId >= endStageId)
                            {
                                deleteIds.Add(Convert.ToInt32(Convert.ToString(scheduleActionItem[DatabaseObjects.Columns.ID])));
                            }
                        }
                    }
                    else
                        deleteIds.Add(Convert.ToInt32(Convert.ToString(scheduleActionItem[DatabaseObjects.Columns.ID])));
                }

                // Delete selected id(s) from collection
                if (deleteIds.Count > 0)
                {
                   try
                    {
                        foreach (int id in deleteIds)
                        {
                            if (id > 0)
                                GetTableDataManager.delete<int>(DatabaseObjects.Tables.SchedulerActions, DatabaseObjects.Columns.ID,Convert.ToString(id));
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "ERROR Deleting Escalation for ticket ID " + ticketId);
                    }
                }
            }
        }

        public  void GenerateEscalationInBackground(Ticket ticketRequest, string ticketId, long moduleId)
        {
            GenerateEscalationInBackground(ticketRequest, ticketId, moduleId, false);
        }
        public  void GenerateEscalationInBackground( Ticket ticketRequest, string ticketId, long moduleId, bool deleteExisting)
        {
            string webUrl = string.Empty;

            if (HttpContext.Current != null)
                webUrl = HttpContext.Current.Request.Url.AbsoluteUri;       
            Thread escThread = new Thread(delegate () { GenerateEscalation(ticketRequest, ticketId, moduleId, webUrl, deleteExisting); });
            escThread.Start();
        }

        public  void GenerateEscalation(Ticket ticketRequest, string ticketId, long moduleId, string webUrl)
        {
            GenerateEscalation(ticketRequest, ticketId, moduleId, webUrl, false);
        }
        public  void GenerateEscalation(Ticket ticketRequest, string ticketId, long moduleId, string webUrl, bool deleteExisting)
        {
            DataRow ticketItem = Ticket.GetCurrentTicket(context, moduleId, ticketId);
            try
            {
                if (deleteExisting)
                {
                    DeleteEscalation(ticketItem);
                }
                if (!UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.SLADisabled) && Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketStatus]) != "Waiting")
                    GenerateEscalation(ticketRequest, ticketItem, moduleId);
            }
            catch (Exception ex)
            {
               ULog.WriteException(ex);
            }

        }
        public void GenerateEscalation(Ticket ticketRequest, DataRow ticketItem, long moduleId)
        {
            // NOTE: We want to go through this even if escalations are OFF just so we update the NextSLATime on tickets.
            //AddUpdateScheduleAction () & AddUpdateRequestTypeScheduleAction() will skip actual escalations processing if escalations not enabled
            if (ticketItem == null)
                return;

            // Get the current statge and life cycle of ticket
            LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(ticketItem);
            LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(ticketItem);
            if (lifeCycle == null || currentStage == null)
                return;
            
            string ticketID = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
            string currentModuleName = uHelper.getModuleNameByTicketId(ticketID);
            bool isSLADisabled = true;

            if (currentModuleName == ModuleNames.SVC)
                isSLADisabled = Convert.ToBoolean(ticketItem[DatabaseObjects.Columns.SLADisabled]);

            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
            bool updateNextSLAFields = UGITUtility.IfColumnExists(DatabaseObjects.Columns.NextSLAType, ticketItem.Table) &&
                                       UGITUtility.IfColumnExists(DatabaseObjects.Columns.NextSLATime, ticketItem.Table);

            string prevSLAType = null;
            DateTime? prevSLATime = null;
            string newSLAType = null;
            DateTime? newSLATime = null;

            // Clear out Next SLA fields, will be set correctly if needed below
            if (updateNextSLAFields)
            {
                prevSLAType = Convert.ToString(ticketItem[DatabaseObjects.Columns.NextSLAType]);
                prevSLATime =!string.IsNullOrEmpty(Convert.ToString(ticketItem[DatabaseObjects.Columns.NextSLATime])) ? Convert.ToDateTime(Convert.ToString(ticketItem[DatabaseObjects.Columns.NextSLATime])) :DateTime.MinValue;
            }

            bool ticketOnHold = UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketOnHold]);
            DateTime holdStartDate = DateTime.MinValue;
            DateTime holdTillDate = DateTime.MinValue;
            if (ticketOnHold && UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketOnHoldStartDate))
                holdStartDate = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketOnHoldStartDate]);
            if (ticketOnHold && UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketOnHoldTillDate))
                holdTillDate = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketOnHoldTillDate]);

            #region Request Type Escalation Override Process
            // SPFieldLookupValue ticketReqTypeLookupValue = new SPFieldLookupValue(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]));
            DataRow requestTypeItem = null;
            double assignmentSLA = 0;
            double resolutionSLA = 0;
            double closeSLA = 0;
            double requestorContactSLA = 0;
            int requestTypeSLAWarningTime = 2 * 60; // defaults to 2 hrs before SLA expiration
            int escalationFrequency = 0;
            DateTime ticketCreationTime = DateTime.UtcNow;
            LifeCycleStage assignedStage = null;
            LifeCycleStage resolvedStage = null;
            LifeCycleStage closedStage = null;

            //pick creation date of ticket as start time
            if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketCreationDate))
                ticketCreationTime = Convert.ToDateTime(ticketItem[DatabaseObjects.Columns.TicketCreationDate]);
            else
                ticketCreationTime = Convert.ToDateTime(ticketItem[DatabaseObjects.Columns.Created]);

            int requestTypeID = UGITUtility.StringToInt(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
            bool use24x7Calendar = false;
            int whInADay = workingHoursInADay;
            if (requestTypeID > 0)
            {
                requestTypeItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.ID}={requestTypeID} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select().First();
                // Get value of Use24x7Calendar field and take it as highest priority to calculate the SLA values
                use24x7Calendar = UGITUtility.StringToBoolean(requestTypeItem[DatabaseObjects.Columns.Use24x7Calendar]);

                // Update WorkingHoursInADay if Use24x7Calendar is enabled
                if (use24x7Calendar)
                    workingHoursInADay = 24;
                //Get ticket location which should be requestor location or user manually set location
                int ticketLocationID = UGITUtility.StringToInt(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.LocationLookup));

                // Get RequestTypeByLocation entry for this location if it exists
                string condition = $"{DatabaseObjects.Columns.TicketRequestTypeLookup}={requestTypeID} and {DatabaseObjects.Columns.LocationLookup}={ticketLocationID} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
                DataTable requestTypeLocationColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestTypeByLocation, condition);
                if (requestTypeLocationColl != null && requestTypeLocationColl.Rows.Count > 0)
                {
                    // Found RequestTypeByLocation entry for this location, use values from here
                    DataRow requestTypeByLocation = requestTypeLocationColl.Rows[0];
                    requestorContactSLA = UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.RequestorContactSLA]);
                    assignmentSLA = UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.AssignmentSLA]);
                    resolutionSLA = UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.ResolutionSLA]);
                    closeSLA = UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.CloseSLA]);
                }
                else
                {
                    // else use values from main request type entry
                    requestorContactSLA = UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.RequestorContactSLA]);
                    assignmentSLA = UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.AssignmentSLA]);
                    resolutionSLA = UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.ResolutionSLA]);
                    closeSLA = UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.CloseSLA]);
                }

                string strWarningTime = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAWarningTime);
                if (Int32.TryParse(strWarningTime.Trim(), out requestTypeSLAWarningTime))
                    requestTypeSLAWarningTime *= 60; // Convert from hours to minutes

                string strRecurInterval = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAEscalationRecurrance);
                if (Int32.TryParse(strRecurInterval.Trim(), out escalationFrequency))
                    escalationFrequency *= 60; 

                // Convert from hours to minutes
                assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Assigned");
                resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved");
                closedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Closed");

                if (resolutionSLA != 0 && resolvedStage == null && closedStage != null)
                {
                    resolvedStage = closedStage;
                    closedStage = null;
                }
            }

            bool requestorContactDue = false;

            double totalHoldTime = 0;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketTotalHoldDuration, ticketItem.Table))
                totalHoldTime = UGITUtility.StringToDouble(ticketItem[DatabaseObjects.Columns.TicketTotalHoldDuration]);

            requestTypeSLAWarningTime += Convert.ToInt32(totalHoldTime);
            double requestorContactSLAWithoutHold = requestorContactSLA;
            double assignmentSLAAWithoutHold = assignmentSLA;
            double resolutionSLAWithoutHold = resolutionSLA;
            double closeSLAWithoutHold = closeSLA;

            // First see if requestor contact SLA needs to be checked, this is done regardless of everything else 
            if (requestorContactSLA != 0 && resolvedStage != null && currentStage.StageStep < resolvedStage.StageStep &&
                UGITUtility.IfColumnExists(DatabaseObjects.Columns.RequestorContacted, ticketItem.Table))
            {
                requestorContactSLA += totalHoldTime;

                // We need to enforce this SLA, so check if requestor has been contacted
                bool requestorContacted = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.RequestorContacted));
                if (!requestorContacted)
                {
                    requestorContactDue = true;

                    // Get name of requestor contact SLA - support for alternate names like "Customer Schedule"
                    string requestorContactSLAName = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestorContactSLAName);
                    if (string.IsNullOrEmpty(requestorContactSLAName))
                        requestorContactSLAName = "Requestor Contact";

                    // Get expiration time for ticket for SLA RequestType wise for Requestor Contact
                    if (updateNextSLAFields)
                    {
                        if (use24x7Calendar)
                            newSLATime = ticketCreationTime.AddMinutes(requestorContactSLA);
                        else
                            newSLATime = uHelper.GetWorkingEndDate(context, ticketCreationTime, requestorContactSLA, isSLA: true);

                        string SLATime = GetFormattedTime(requestorContactSLAWithoutHold, workingHoursInADay);
                        newSLAType = string.Format("{0} SLA ({1})", requestorContactSLAName, SLATime);
                    }

                    // Generate SLA Warning Notification if time not already past
                    if (requestorContactSLA > requestTypeSLAWarningTime)
                    {
                        DateTime slaWarningTime = DateTime.MinValue;
                        if (use24x7Calendar)
                            slaWarningTime = ticketCreationTime.AddMinutes(requestorContactSLA - requestTypeSLAWarningTime);
                        else
                            slaWarningTime = uHelper.GetWorkingEndDate(context, ticketCreationTime, requestorContactSLA - requestTypeSLAWarningTime, isSLA: true);
                        
                        if (!ticketOnHold && slaWarningTime > DateTime.Now)
                            AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, slaWarningTime, moduleId, requestorContactSLAName, 0, 0, use24x7Calendar);
                    }

                    // Generate SLA Expiration Notification if time in future, or recurrence configured
                    
                    DateTime nextEscalationTime = CalculateEscalationTime(ticketCreationTime, requestorContactSLA, escalationFrequency, isUse24x7Calendar: use24x7Calendar);
                    if (nextEscalationTime > DateTime.Now)
                        AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, nextEscalationTime, moduleId, requestorContactSLAName, 1, escalationFrequency, use24x7Calendar);
                }
            }

            // Next if any of the SLAs at the request type level are active for current stage, they take precedence
            if (assignmentSLA != 0 && assignedStage != null && currentStage.StageStep < assignedStage.StageStep)
            {
                assignmentSLA += totalHoldTime;


                if (updateNextSLAFields && !requestorContactDue)
                {
                    // Get expiration time for ticket for SLA RequestType wise for Assignment
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.NextSLATime, ticketItem.Table))
                    {
                        if (use24x7Calendar)
                            newSLATime = ticketCreationTime.AddMinutes(assignmentSLA);
                        else
                            newSLATime = uHelper.GetWorkingEndDate(context, ticketCreationTime, assignmentSLA, isSLA: true);
                    }

                    string SLATime = GetFormattedTime(assignmentSLAAWithoutHold, workingHoursInADay);
                    newSLAType = string.Format("Assignment SLA ({0})", SLATime);
                }

                // Generate SLA Warning Notification if time not already past
                if (assignmentSLA > requestTypeSLAWarningTime)
                {
                    DateTime slaWarningTime = DateTime.MinValue;

                    if (use24x7Calendar)
                        slaWarningTime = ticketCreationTime.AddMinutes(assignmentSLA - requestTypeSLAWarningTime);
                    else
                        slaWarningTime = uHelper.GetWorkingEndDate(context, ticketCreationTime, assignmentSLA - requestTypeSLAWarningTime, isSLA: true);
                    
                    if (!ticketOnHold && slaWarningTime > DateTime.Now)
                        AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, slaWarningTime, moduleId, "Assignment", 0, 0, use24x7Calendar);
                }

                // Generate SLA Expiration Notification if time in future, or recurrence configured
                DateTime nextEscalationTime = CalculateEscalationTime(ticketCreationTime, assignmentSLA, escalationFrequency);
                if (nextEscalationTime > DateTime.Now)
                    AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, nextEscalationTime, moduleId, "Assignment", 1, escalationFrequency, use24x7Calendar);
            }
            else if (resolutionSLA != 0 && resolvedStage != null && currentStage.StageStep < resolvedStage.StageStep)
            {
                resolutionSLA += totalHoldTime;

                if (updateNextSLAFields && !requestorContactDue)
                {
                    // Get expiration time for ticket for SLA RequestType wise for Resolution
                    if (use24x7Calendar)
                        newSLATime = ticketCreationTime.AddMinutes(resolutionSLA);
                    else
                        newSLATime = uHelper.GetWorkingEndDate(context, ticketCreationTime, resolutionSLA, isSLA: true);
                    string SLATime = GetFormattedTime(resolutionSLAWithoutHold, workingHoursInADay);
                    newSLAType = string.Format("Resolution SLA ({0})", SLATime);
                }

                // Generate SLA Warning Notification if time not already past
                if (resolutionSLA > requestTypeSLAWarningTime)
                {
                    DateTime slaWarningTime = DateTime.MinValue;
                    if (use24x7Calendar)
                        slaWarningTime = ticketCreationTime.AddMinutes(resolutionSLA - requestTypeSLAWarningTime);
                    else
                        slaWarningTime = uHelper.GetWorkingEndDate(context, ticketCreationTime, resolutionSLA - requestTypeSLAWarningTime, isSLA: true);
                    
                    if (!ticketOnHold && slaWarningTime > DateTime.Now)
                        AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, slaWarningTime, moduleId, "Resolution",  0, 0, use24x7Calendar);
                }

                // Generate SLA Expiration Notification if time in future, or recurrence configured
                DateTime nextEscalationTime = CalculateEscalationTime(ticketCreationTime, resolutionSLA, escalationFrequency, isUse24x7Calendar: use24x7Calendar);
                if (nextEscalationTime > DateTime.Now)
                    AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, nextEscalationTime, moduleId, "Resolution",  1, escalationFrequency, use24x7Calendar);
            }
            else if (closeSLA != 0 && closedStage != null && currentStage.StageStep < closedStage.StageStep)
            {
                closeSLA += totalHoldTime;

                if (updateNextSLAFields && !requestorContactDue)
                {
                    // Get expiration time for ticket for SLA RequestType wise for close
                    if (use24x7Calendar)
                        newSLATime = ticketCreationTime.AddMinutes(closeSLA);
                    else
                        newSLATime = uHelper.GetWorkingEndDate(context, ticketCreationTime, closeSLA, isSLA: true);
                    string SLATime = GetFormattedTime(closeSLAWithoutHold, workingHoursInADay);
                    newSLAType = string.Format("Close SLA ({0})", SLATime);
                }

                // Generate SLA Warning Notification if time not already past
                if (closeSLA > requestTypeSLAWarningTime)
                {
                    DateTime slaWarningTime = DateTime.MinValue;

                    if (use24x7Calendar)
                        slaWarningTime = ticketCreationTime.AddMinutes(closeSLA - requestTypeSLAWarningTime);
                    else
                        slaWarningTime = uHelper.GetWorkingEndDate(context, ticketCreationTime, closeSLA - requestTypeSLAWarningTime, isSLA: true);
                    if (!ticketOnHold && slaWarningTime > DateTime.Now)
                        AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, slaWarningTime, moduleId, "Close",  0, 0, use24x7Calendar);
                }

                // Generate SLA Expiration Notification if time in future, or recurrence configured
                DateTime nextEscalationTime = CalculateEscalationTime(ticketCreationTime, closeSLA, escalationFrequency, isUse24x7Calendar: use24x7Calendar);
                if (nextEscalationTime > DateTime.Now)
                    AddUpdateRequestTypeScheduleAction(ticketItem, requestTypeItem, newSLATime, nextEscalationTime, moduleId, "Close", 1, escalationFrequency, use24x7Calendar);
            }

            #endregion

            #region Get resolutionSLA from SVC config if Module is SVC
            

            else if (currentModuleName == ModuleNames.SVC && !isSLADisabled)
            {
                SLAConfiguration slaconfig = null;
                use24x7Calendar = false;
                bool startResolutionSLAFromAssigned = false;
                ServicesManager servicesManager = new ServicesManager(context);
                long serviceLookup = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.ServiceTitleLookup]);

                // Get Resolution SLA and startResolutionSLAFromAssigned from Service list
                Services service = servicesManager.LoadByID(serviceLookup);
                if (service != null)
                {
                    if (!string.IsNullOrEmpty(service.SLAConfiguration))
                        slaconfig = servicesManager.GetSLAConfiguration(service.SLAConfiguration);

                    use24x7Calendar = service.Use24x7Calendar;
                    if (use24x7Calendar)
                        workingHoursInADay = 24;

                    resolutionSLA = service.ResolutionSLA;
                    startResolutionSLAFromAssigned = service.StartResolutionSLAFromAssigned;
                }

                DateTime creationTime = DateTime.MinValue;
                DateTime slaCompletionDate = DateTime.MinValue;
                int currentStageStepID = currentStage.StageStep;

                // Assigned stage will initiated if SLA is not set to startResolutionSLAFromAssigned
                if (startResolutionSLAFromAssigned)
                    assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Assigned");
                else
                    assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Initiated");

                resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved");
                closedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Closed");

                if (resolutionSLA != 0 && resolvedStage == null && closedStage != null)
                {
                    resolvedStage = closedStage;
                    closedStage = null;
                }

                if (resolutionSLA > 0 && currentStage.StageStep >= assignedStage.StageStep && resolvedStage != null && currentStage.StageStep < resolvedStage.StageStep)
                {
                    resolutionSLAWithoutHold = resolutionSLA;
                    resolutionSLA += totalHoldTime;

                    if (startResolutionSLAFromAssigned)
                        creationTime = GetTicketAssignedDate(context, ticketItem, assignedStage.StageStep);

                    if (creationTime == DateTime.MinValue)
                    {
                        if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketCreationDate))
                            creationTime = Convert.ToDateTime(ticketItem[DatabaseObjects.Columns.TicketCreationDate]);
                        else
                            creationTime = Convert.ToDateTime(ticketItem[DatabaseObjects.Columns.Created]);
                    }

                    if (updateNextSLAFields)
                    {
                        if (use24x7Calendar)
                            slaCompletionDate = creationTime.AddMinutes(resolutionSLA);
                        else
                            slaCompletionDate = uHelper.GetWorkingEndDate(context, creationTime, resolutionSLA, isSLA: true);

                        if (slaCompletionDate != DateTime.MinValue)
                        {
                            newSLATime = slaCompletionDate;

                            string SLATime = GetFormattedTime(resolutionSLAWithoutHold, workingHoursInADay);

                            newSLAType = string.Format("Resolution SLA ({0})", SLATime);
                        }
                    }

                    if (!ticketOnHold && slaconfig != null && slaconfig.EnableEscalation)//Escalation will generate irrespective the value of escation after in case of zero too
                    {
                        double escalationDuration = resolutionSLA + slaconfig.EscalationAfter;
                        DateTime nextEscalationTime = CalculateEscalationTime(creationTime, escalationDuration, slaconfig.EscalationFrequency, use24x7Calendar);
                       
                        if (nextEscalationTime > DateTime.Now)
                            AddUpdateSVCScheduleAction(context, ticketItem, slaconfig, newSLATime, nextEscalationTime, moduleId, "Resolution", 1, use24x7Calendar);
                    }
                }
            }
            #endregion Get resolutionSLA from SVC config if Module is SVC

            #region Module-Priority Escalation

            else if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketPriorityLookup))
            {
                // If none of the RequestType-level SLAs are active, then the module-priority level SLAs take effect
                int priorityId = UGITUtility.StringToInt(ticketItem[DatabaseObjects.Columns.TicketPriorityLookup]);// priorityLookupValue.LookupId;

                // Get all the SLA Rules of the ticket's module and ticket priority
                string condition = $"{DatabaseObjects.Columns.TicketPriorityLookup}={priorityId} and {DatabaseObjects.Columns.ModuleNameLookup}='{ObjModuleViewManager.LoadByID(moduleId).ModuleName}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
                DataTable slaRuleColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, condition);
                DataTable moduleStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                DataTable escalationRules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.EscalationRule, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                DateTime minSLAexpirationTime = DateTime.MaxValue;
                DataTable ticketHistoryData = null;
                if (slaRuleColl.Rows.Count > 0)
                {
                    //condition = DatabaseObjects.Columns.ModuleNameLookup + "='" + ObjModuleViewManager.LoadByID(moduleId).ModuleName + "' and " + DatabaseObjects.Columns.TicketId + "='" + ticketItem[DatabaseObjects.Columns.TicketId] + "'";
                    condition = $"{DatabaseObjects.Columns.ModuleNameLookup}='{ObjModuleViewManager.LoadByID(moduleId).ModuleName}' and {DatabaseObjects.Columns.TicketId}='{ticketItem[DatabaseObjects.Columns.TicketId]}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
                    ticketHistoryData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory, condition);
                }

                // Check the rules one by one.
                foreach (DataRow  SLAItem in slaRuleColl.Rows)
                {
                    // Get the SLA Rule start & end stage.
                    LifeCycleStage startStage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(UGITUtility.GetSPItemValue(SLAItem, DatabaseObjects.Columns.StartStageStep)));
                    LifeCycleStage endStage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(UGITUtility.GetSPItemValue(SLAItem, DatabaseObjects.Columns.EndStageStep)));

                    if (startStage == null || endStage == null)
                    {
                        continue;
                    }

                    // Check if the SLA rule Start stageid is equevalent to ticket's stageid(actual).
                    if (currentStage.StageStep >= startStage.StageStep && currentStage.StageStep < endStage.StageStep)
                    {
                        int holdTime = 0;
                        if (ticketHistoryData != null && ticketHistoryData.Rows.Count > 0)
                        {
                            holdTime = UGITUtility.StringToInt(ticketHistoryData.Compute(string.Format("sum({0})", DatabaseObjects.Columns.OnHoldDuration), string.Format("{0} >= {1} And {0} < {2}", currentStage.StageStep, startStage.StageStep, endStage.StageStep)));
                        }

                        // Get start time of current stage
                        DateTime startStageDateTime = DateTime.MinValue;
                        if (startStage.StageStep == 1)
                        {   //pick creation date of ticket if rule startstage is first
                            if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketCreationDate))
                                startStageDateTime = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketCreationDate]);
                            else
                                startStageDateTime = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.Created]);
                        }
                        else
                        {   // Else get date from ModuleWorkflowHistory
                            startStageDateTime = this.GetStageStartDate( lifeCycle, startStage, ticketID);
                        }

                        if (startStageDateTime == DateTime.MinValue)
                        {
                            ULog.WriteLog(string.Format("ERROR: Could not get stage start time for stage {0} of ticket {1}", startStage.StageStep, ticketID));
                            continue;
                        }

                        // Get expiration time for SLA
                        double SLAHours = UGITUtility.StringToDouble(SLAItem[DatabaseObjects.Columns.SLAHours]);
                        // SLAHours is store based on working hours in days, eg 8 hours, so we need to convert hours into days and convert it into minutes based on 24 hours
                        if (use24x7Calendar && (SLAHours % whInADay == 0))
                            SLAHours = SLAHours / whInADay * 24;
                        if (SLAHours > 0 && updateNextSLAFields)
                        {
                            int slaMinutesWithoutHold = Convert.ToInt32(SLAHours * 60);
                            int SLAMinutes = slaMinutesWithoutHold + holdTime;

                            DateTime slaExpirationTime = DateTime.MinValue;
                            if (use24x7Calendar)
                                slaExpirationTime = startStageDateTime.AddMinutes(SLAMinutes);
                            else
                                slaExpirationTime = uHelper.GetWorkingEndDate(context, startStageDateTime, SLAMinutes, isSLA: true);

                            if (slaExpirationTime < minSLAexpirationTime)
                            {
                                newSLATime = slaExpirationTime;
                                string SLATime = GetFormattedTime(slaMinutesWithoutHold, workingHoursInADay);
                                newSLAType = string.Format("{0} ({1})", Convert.ToString(SLAItem[DatabaseObjects.Columns.SLACategory]).Replace("Time", "SLA"), SLATime);
                                minSLAexpirationTime = slaExpirationTime;
                            }
                        }

                        //Get all the escalation depending on above SLA rule.
                      
                        DataRow[] escalationRuleColl = escalationRules.Select(DatabaseObjects.Columns.SlaRuleIdLookup+"="+Convert.ToString(SLAItem[DatabaseObjects.Columns.Id]));
                        if (escalationRuleColl != null && escalationRuleColl.Count() > 0)
                        {
                            //Enter the each escalation time in EscalationQueue table.
                            foreach (DataRow  escalationRule in escalationRuleColl)
                            {
                                DateTime nextEscalationTime = CalculateEscalationTime(startStageDateTime, ticketItem, escalationRule);
                                AddUpdateScheduleAction(ticketItem, nextEscalationTime, SLAItem, escalationRule, moduleId, use24x7Calendar);
                            }
                        }
                    }
                }
            }
            #endregion

            if (updateNextSLAFields)
            {
                // For tickets on hold with non-expired SLA, also add current hold window to SLA expiration time
                bool isSLAExpired = (newSLATime != DateTime.MinValue && newSLATime < DateTime.Now);
                if (ticketOnHold && !isSLAExpired)
                {
                    if (use24x7Calendar)
                        newSLATime += (holdTillDate - holdStartDate);
                    else
                    {
                        double holdMinutes = uHelper.GetWorkingMinutesBetweenDates(context, holdStartDate, holdTillDate);
                        newSLATime = uHelper.GetWorkingEndDate(context, (DateTime)newSLATime, holdMinutes, isSLA: true);
                    }
                }
                if (newSLATime != prevSLATime || newSLAType != prevSLAType)
                {
                    ticketItem[DatabaseObjects.Columns.NextSLAType] = newSLAType;
                    if (newSLATime != null)
                        ticketItem[DatabaseObjects.Columns.NextSLATime] = newSLATime;
                    else
                        ticketItem[DatabaseObjects.Columns.NextSLATime] = DBNull.Value;
                    
                    try
                    {
                        // Update ticket
                        ticketRequest.CommitChanges(ticketItem, donotUpdateEscalations: true,stopUpdateDependencies: true);
                        
                    }
                    catch (Exception ex)
                    {
                        // Ignore known exception due to TicketRequest.CommitChanges() called multiple times in new ticket creation code
                        ULog.WriteException(ex);
                    }
                    finally
                    {
                    }
                }
            }
        }

        // Returns minutes formatted to nearest round unit such as: 2 days, or 3 hours or 30 mins
        private string GetFormattedTime(double minutes, int workingHoursInADay)
        {
            string formattedTime;
            double hours = minutes / 60;
            if (hours % workingHoursInADay == 0)
            {
                int days = Convert.ToInt32(hours / workingHoursInADay);
                formattedTime = string.Format("{0:0.##} {1}", days, days == 1 ? "day" : "days");
            }
            else if (minutes % 60 == 0)
                formattedTime = string.Format("{0:0.##} {1}", hours, hours == 1.0d ? "hr" : "hrs");
            else
                formattedTime = string.Format("{0:0.##} {1}", minutes, minutes == 1.0d ? "min" : "mins");

            return formattedTime;
        }

        /// <summary>
        /// Calculates the next escalation time from startTime based on escalation rule using Working days & working hours from Calendar/Config. 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="ticketItem"></param>
        /// <param name="escalationRule"></param>
        /// <param name="spWeb"></param>
        /// <returns>Next Escalation Time</returns>
        private DateTime CalculateEscalationTime(DateTime startTime, DataRow ticketItem, DataRow escalationRule)
        {
            double escalationMinutes = Convert.ToDouble(escalationRule[DatabaseObjects.Columns.EscalationMinutes]);

            //// Use Desired completion date if chosen on escalation rule and if it is later than escalation time.
            //if (UGITUtility.IsSPItemExist(escalationRule, DatabaseObjects.Columns.UseDesiredCompletionDate))
            //{
            //    bool useDesiredCompletionDate = UGITUtility.StringToBoolean(escalationRule[DatabaseObjects.Columns.UseDesiredCompletionDate]);
            //    if (useDesiredCompletionDate && UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketDesiredCompletionDate))
            //    {
            //        DateTime desiredCompletionDate = (DateTime)ticketItem[DatabaseObjects.Columns.TicketDesiredCompletionDate];
            //        if (desiredCompletionDate < startTime) // Sanity check in case desired date was in the past
            //            desiredCompletionDate = startTime;
            //        int workingHrsInDay = UGITUtility.GetWorkingHoursInADay(spWeb);
            //        int desiredCompletionMinutes = UGITUtility.GetTotalWorkingDaysBetween(startTime, desiredCompletionDate, spWeb) * workingHrsInDay * 60;
            //        if (desiredCompletionMinutes > escalationMinutes)
            //        {
            //            escalationMinutes = desiredCompletionMinutes;
            //        }
            //    }
            //}

            //Escalation frequency minutes which required to send mail again if required
            double escalationFrequencyMinutes = 0;
            double.TryParse(Convert.ToString(escalationRule[DatabaseObjects.Columns.EscalationFrequency]), out escalationFrequencyMinutes);

            // Get next escalation time
            return CalculateEscalationTime(startTime, escalationMinutes, escalationFrequencyMinutes);
        }

        /// <summary>
        /// Calculates the next escalation time from startTime using Working days & working hours from Calendar/Config. 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="escalationMinutes"></param>
        /// <param name="escalationFrequencyMinutes"></param>
        /// <param name="spWeb"></param>
        /// <returns>Next Escalation Time</returns>
        private DateTime CalculateEscalationTime(DateTime startTime, double escalationMinutes, double escalationFrequencyMinutes, bool isUse24x7Calendar = false)
        {
            // Get next escalation time, and repeat till escalation time in future
            DateTime nextEscalationTime = DateTime.MinValue;
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);

            if (isUse24x7Calendar)
            {
                if ((escalationMinutes / 60) % workingHoursInADay == 0)
                    escalationMinutes = escalationMinutes / workingHoursInADay * 24;
                nextEscalationTime = startTime.AddMinutes(escalationMinutes);
            }
            else
                nextEscalationTime = uHelper.GetWorkingEndDate(context, startTime, escalationMinutes);

            if (nextEscalationTime < DateTime.Now && escalationFrequencyMinutes > 0)
            {
                while (nextEscalationTime < DateTime.Now)
                {
                    if (isUse24x7Calendar)
                    {
                        if ((escalationFrequencyMinutes / 60) % workingHoursInADay == 0)
                            escalationFrequencyMinutes = escalationFrequencyMinutes / workingHoursInADay * 24;
                        nextEscalationTime = nextEscalationTime.AddMinutes(escalationFrequencyMinutes);
                    }
                    else
                        nextEscalationTime = uHelper.GetWorkingEndDate(context, nextEscalationTime, escalationFrequencyMinutes);
                }
            }
            return nextEscalationTime;
        }

        private void AddUpdateScheduleAction(DataRow ticket, DateTime nextEscalationTime, DataRow SLARule, DataRow escalationRule, long moduleId, bool use24x7Calendar)
        {
            string ticketId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            string mailTo = GetAllEmailTo(escalationRule, ticket, moduleId);

            // Add Ticket Action User in cc field if not in emailTo
            string mailCC = string.Empty;
            if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(escalationRule, DatabaseObjects.Columns.IncludeActionUsers)))
            {
                StringBuilder mailCCList = new StringBuilder();
                string stageActionUser = getStageActionUsers(ticket);
               
                List<UserProfile> actionUseruserInfo = uHelper.GetActionUsersList(context, stageActionUser, ticket);
                string usersEmails = "";
                if (actionUseruserInfo.Count()>0)
                  usersEmails = string.Join(Constants.UserInfoSeparator, actionUseruserInfo.AsEnumerable().Select(x => x.Email).ToList());
                string[] separator = { Constants.UserInfoSeparator };
                if (!string.IsNullOrEmpty(usersEmails))
                {
                    string[] actionUserEmails = usersEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < actionUserEmails.Length; i++)
                    {
                        string actionUser = Convert.ToString(actionUserEmails[i]);
                        if (actionUser != string.Empty && !mailTo.Contains(actionUser))
                        {
                            if (mailCCList.Length != 0)
                                mailCCList.Append(";");
                            mailCCList.Append(actionUser);
                        }
                    }
                }
                mailCC = mailCCList.ToString();
            }

            if (string.IsNullOrEmpty(mailTo) && string.IsNullOrEmpty(mailCC))
            {
                return; // No one to send to
            }

            //to get email subject 
            string subject = Convert.ToString(escalationRule[DatabaseObjects.Columns.EscalationMailSubject]);
            string[] tokens = uHelper.GetMyTokens(subject);
            subject = ReplaceMyTokensWithTicketValues(ticket, subject, tokens);

            ///to get email body
            StringBuilder emailBody = new StringBuilder();
            string body = Convert.ToString(escalationRule[DatabaseObjects.Columns.EscalationEmailBody]);
            tokens = uHelper.GetMyTokens(body);
            emailBody.Append(ReplaceMyTokensWithTicketValues(ticket, body, tokens));

           //Adding expiration time in Mail body
            string slaRequestType = Convert.ToString(SLARule[DatabaseObjects.Columns.SLACategory]).Replace("Time", "");
            string Footer = uHelper.GetTicketDetailsForEmailFooter(context,ticket, uHelper.getModuleNameByTicketId(Convert.ToString(ticket[DatabaseObjects.Columns.TicketId])), true, false);
            emailBody.Append(Footer);
            SchedulerAction scheduleDic = new SchedulerAction();
            //old pattern
            // Dictionary<string, object> scheduleDic = new Dictionary<string, object>();
            //scheduleDic.Add(DatabaseObjects.Columns.StartTime, nextEscalationTime);
            scheduleDic.StartTime = nextEscalationTime;
            scheduleDic.Title = ticketId.ToString() + " " + nextEscalationTime.ToString();
            scheduleDic.TicketId = ticketId;
            scheduleDic.ActionType =""+ ScheduleActionType.EscalationEmail;
            scheduleDic.ModuleNameLookup = ObjModuleViewManager.LoadByID(moduleId).ModuleName;
            scheduleDic.Recurring = true;
            scheduleDic.RecurringInterval = Convert.ToInt32(escalationRule[DatabaseObjects.Columns.EscalationFrequency]);
            scheduleDic.EmailIDTo = mailTo;
            scheduleDic.EmailIDCC = mailCC;
            scheduleDic.MailSubject = subject;
            scheduleDic.EmailBody = emailBody.ToString();
            scheduleDic.RecurringEndDate = null;
            StringBuilder customProperties = new StringBuilder();
            customProperties.Append(string.Format("{0}={1}", CustomProperties.EscalationRuleId, Convert.ToString(escalationRule[DatabaseObjects.Columns.Id])));
            customProperties.Append("#");
            customProperties.Append(string.Format("{0}={1}", CustomProperties.SLARuleId, Convert.ToString(SLARule[DatabaseObjects.Columns.Id])));
            scheduleDic.CustomProperties = customProperties.ToString();
            if (use24x7Calendar)
                customProperties.AppendFormat(";#BusinessHours=false");

            objScheduleActionsManager.AddUpdate(scheduleDic);
            //AgentJobHelper agent = new AgentJobHelper(spWeb);
            //agent.CreateSchedule(scheduleDic);
        }

        // new funtion for get  updated request type schedule...// slarequesttype show the SLA Type i.e assinged close..// slaType 1 means after expire the ticket.
        private  void AddUpdateRequestTypeScheduleAction( DataRow ticket, DataRow requestType, DateTime? slaExpirationTime, DateTime nextEscalationTime, long moduleId, string slaRequestType, int slaType, int escalationFrequency, bool use24x7Calendar)
        {
            string ticketId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            List<UsersEmail> usersEmail = GetRequestTypeAllEmailTo(requestType, ticket, moduleId,  slaType);
            string emailIds = string.Join(",", usersEmail.AsEnumerable().Select(x => x.Email).ToList());
            string usersNames = string.Join(",", usersEmail.AsEnumerable().Select(x => x.UserName).ToList());

            if (usersEmail == null || string.IsNullOrEmpty(emailIds))
                return; // No one to send to

            string mailTo = emailIds;
            mailTo= UGITUtility.RemoveDuplicateEmails(mailTo);
            string subject = string.Empty;
            string[] tokens = null;

            // email body
            StringBuilder emailBody = new StringBuilder();
            string greeting = objConfigurationVariableHelper.GetValue(ConfigConstants.Greeting);
            string signature = objConfigurationVariableHelper.GetValue(ConfigConstants.Signature);
            string expirationTimeMessage = string.Empty;
           if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.NextSLATime))
               expirationTimeMessage = string.Format("<b>Expiration Time:</b> {0:MMM-dd-yyyy hh:mm tt}<br/><br/>", slaExpirationTime != null ? slaExpirationTime : Convert.ToDateTime(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.NextSLATime)));

            string body = string.Empty;
            if (slaType == 1) // SLA Expiration
           {
                subject = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAEscalationSubject);
                tokens = uHelper.GetMyTokens(subject);
                if (!string.IsNullOrEmpty(subject))
                    subject = ReplaceMyTokensWithTicketValues(ticket, subject, tokens);
                else
                    subject = string.Format("Ticket {0} Escalation", ticketId);

                body = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAEscalationBody);
                if (!string.IsNullOrEmpty(body))
                {
                    // Replace SLA type token in case we get that from configured body, other standard tokens replaced in ReplaceMyTokensWithTicketValues()
                    body = body.Replace("[$SLAType$]", slaRequestType.ToLower());
                    tokens = uHelper.GetMyTokens(body);
                    body = ReplaceMyTokensWithTicketValues(ticket, body, tokens);
                }
                else
                {
                    // Default body
                    body = string.Format("The ticket {1} SLA for Ticket ID <b>{0}</b> has <b>EXPIRED</b> and needs your attention.",
                                                    ticketId, slaRequestType.ToLower());
                }
            }
           else // SLA Pre-expiration Warning
            {
                subject = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAWarningSubject);
                tokens = uHelper.GetMyTokens(subject);
                if (!string.IsNullOrEmpty(subject))
                    subject = ReplaceMyTokensWithTicketValues(ticket, subject, tokens);
                else
                    subject = string.Format("Ticket {0} Escalation", ticketId);
                body = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAWarningBody);
                if (!string.IsNullOrEmpty(body))
                {
                    //// Replace SLA type token in case we get that from configured body, other standard tokens replaced in ReplaceMyTokensWithTicketValues()
                    body = body.Replace("[$SLAType$]", slaRequestType.ToLower());
                    tokens = uHelper.GetMyTokens(body);
                    body = ReplaceMyTokensWithTicketValues(ticket, body, tokens);
                }
               else
                {
                    // Default body
                    body = string.Format("The ticket {1} SLA for Ticket ID <b>{0}</b> is about to expire and needs your attention.",
                                                    ticketId, slaRequestType.ToLower());
                }
            }
            emailBody.Append(string.Format(@"{0} {1}<br/><br/>
                                             {2}<br/><br/>
                                             {3}
                                             {4}<br/>", greeting, usersNames, body, expirationTimeMessage, signature));

            string Footer = uHelper.GetTicketDetailsForEmailFooter(context, ticket, uHelper.getModuleNameByTicketId(Convert.ToString(ticket[DatabaseObjects.Columns.TicketId])), true, false);
            emailBody.Append(Footer);
            SchedulerAction scheduleAction = new SchedulerAction();
            scheduleAction.StartTime = nextEscalationTime;
            scheduleAction.Title = ticketId.ToString() + " " + nextEscalationTime.ToString();
            scheduleAction.TicketId = ticketId;
            scheduleAction.ActionType =""+ ScheduleActionType.EscalationEmail;
            scheduleAction.ModuleNameLookup = ObjModuleViewManager.LoadByID(moduleId).ModuleName;
            scheduleAction.EmailIDTo = mailTo;
            scheduleAction.EmailIDCC = string.Empty;
            scheduleAction.MailSubject = subject;
            scheduleAction.EmailBody = emailBody.ToString();
            scheduleAction.RecurringEndDate = null;   
            if (slaType == 1 && escalationFrequency > 0)
            {
                scheduleAction.Recurring = true;
                scheduleAction.RecurringInterval = escalationFrequency;            
            }
            else
                scheduleAction.Recurring = false;

            string customProperties = string.Empty;
            if (slaType == 1) // SLA Expiration post-expiration
                customProperties = slaRequestType + " Request Type SLA Post-Expiration";
            else // SLA Warning pre-expiration
                customProperties = slaRequestType + " Request Type SLA Pre-Expiration";

            //set BusinessHours=false in customproperties enable logic to set next recuring escalation based 24x7 not working hours
            if (use24x7Calendar)
                customProperties = string.Format("{0}{1}BusinessHours=false", customProperties, Constants.Separator);
            scheduleAction.CustomProperties = customProperties;
            objScheduleActionsManager.AddUpdate(scheduleAction);

        }
        //new funtion to get all emailto for requesttpe.
        private  List<UsersEmail> GetRequestTypeAllEmailTo(DataRow requestType, DataRow ticketItem, long moduleid, int slaType)
        {
            StringBuilder mailTo = new StringBuilder();
            List<UsersEmail> userInfo = new List<UsersEmail>();

            if (slaType == 1)
            {
                // SLA Expiration email, also include Escalation roles from RequestType
                string RequestTypeSLAEscalationRoles = objConfigurationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAEscalationRoles);
                string[] actionUserFields = RequestTypeSLAEscalationRoles.Split(';');

                foreach (string actionUserField in actionUserFields)
                {
                    if (actionUserField == "PRPManager")
                    {
                        List<UserProfile> manager = GetTicketPRPORPManagers(ticketItem, moduleid, 0, UserManager);
                        if (manager != null & manager.Count()>0)
                        {
                            foreach (UserProfile user in manager)
                            {                                
                                if (!string.IsNullOrEmpty(user.Email))
                                {
                                    UsersEmail uEmail = new UsersEmail();
                                    uEmail.ID = user.Id;
                                    uEmail.UserName = user.Name;
                                    uEmail.Email = user.Email;
                                    userInfo.Add(uEmail);
                                }
                            }
                            UserManager.AddUsersFromFieldUserValue(ref userInfo,  false, false);
                        }
                    }
                    else if (actionUserField == "ORPManager")
                    {
                        List<UserProfile> manager = GetTicketPRPORPManagers(ticketItem, moduleid, 1, UserManager);
                        if (manager != null)
                        {
                            foreach (UserProfile user in manager)
                            {
                                if (!string.IsNullOrEmpty(user.Email))
                                {
                                    UsersEmail uEmail = new UsersEmail();
                                    uEmail.ID = user.Id;
                                    uEmail.UserName = user.Name;
                                    uEmail.Email = user.Email;
                                    userInfo.Add(uEmail);
                                }
                            }
                            UserManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
                        }
                    }
                    else
                    {
                        UserManager.AddUsersFromItemField(ref userInfo, requestType, actionUserFields,  false, false);
                    }
                }
            }

            // Get all action users
            string actionUserList = getStageActionUsers(ticketItem);
            string[] actionUsers = UGITUtility.SplitString(actionUserList, Constants.Separator, StringSplitOptions.RemoveEmptyEntries);
            UserManager.AddUsersFromItemField(ref userInfo, ticketItem, actionUsers, false, false);         
            return userInfo;
        }
        private List<UserProfile> GetTicketPRPORPManagers(DataRow ticketItem, long moduleid, int managertype,UserProfileManager UserManager)
        {
          List<UserProfile> manager = new List<UserProfile>();
            if (managertype == 0) // PRP Manager
            {
                if (ticketItem.ItemArray.Contains(DatabaseObjects.Columns.TicketPRP))
                {
                    string spfield =Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketPRP]);
                    if (!string.IsNullOrEmpty(spfield))
                    {
                        UserProfile prp = UserManager.GetUserById(spfield);                     
                        if (prp!=null && !string.IsNullOrEmpty(prp.ManagerID))
                        {
                            string[] managerIdList = prp.ManagerID.Split(',').ToArray();
                            foreach (string managerId in managerIdList)
                            {
                                UserProfile prpManager = UserManager.GetUserById(managerId);
                                if (prpManager != null)
                                    manager.Add(prpManager);
                            }
                        }
                    }
                }
            }
            else if (managertype == 1) // ORP Manager
            {
                if (ticketItem.ItemArray.Contains(DatabaseObjects.Columns.TicketORP))
                {
                    string spfield =Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketORP]);
                    UserProfile orp = UserManager.GetUserById(spfield);
                    if (orp != null && !string.IsNullOrEmpty(orp.ManagerID))
                    {
                        string[] managerIdList = orp.ManagerID.Split(',').ToArray();
                        foreach (string managerId in managerIdList)
                        {
                            UserProfile orpManager = UserManager.GetUserById(managerId);
                            if (orpManager != null)
                                manager.Add(orpManager);
                        }
                    }
                }
            }

            return manager;
        }
        public string ReplaceMyTokensWithTicketValues(DataRow ticket, string emailBody)
        {
            string[] tokens = uHelper.GetMyTokens(emailBody);
            return ReplaceMyTokensWithTicketValues(ticket, emailBody, tokens);
        }
        public  string ReplaceMyTokensWithTicketValues(DataRow ticket, string emailBody, string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string ticketColumn = tokens[i].ToString().Remove(0, 2);
                ticketColumn = ticketColumn.Remove(ticketColumn.Length - 2);
                //foreach (SPField field in ticket.Fields)
                //{
                //    if (ticketColumn == DatabaseObjects.Columns.NextSLATime)
                //    {
                //        emailBody = emailBody.Replace(tokens[i].ToString(), Convert.ToDateTime(ticket[ticketColumn]).ToString("{0:MMM-dd-yyyy hh:mm tt}"));
                //    }
                //    else if (field.InternalName == ticketColumn)
                //    {
                //        emailBody = emailBody.Replace(tokens[i].ToString(), ticket[ticketColumn].ToString());
                //    }
                //}
            }

            return emailBody;
        }

        private string GetAllEmailTo(DataRow escalationRule, DataRow ticketItem, long moduleid)
        {
            StringBuilder mailTo = new StringBuilder();
            List<UserProfile> userInfo = new List<UserProfile>();
            //UserProfile.UsersInfo userInfo = new UserProfile.UsersInfo();

            mailTo.Append(GetAllEmailTo(Convert.ToString(escalationRule[DatabaseObjects.Columns.EscalationToRoles]), ticketItem, moduleid));

            // Append escalationTo field in mailTo if it is not exist previously.
            string escalationTo = Convert.ToString(escalationRule[DatabaseObjects.Columns.EscalationToEmails]);
            if (escalationTo != string.Empty && !Convert.ToString(mailTo).Contains(escalationTo))
            {
                if (mailTo.Length != 0)
                    mailTo.Append(";");
                mailTo.Append(escalationTo);
            }

            return mailTo.ToString();
        }

        public string GetAllEmailTo(string escalationsRoles, DataRow ticketItem, long moduleid)
        {
            StringBuilder mailTo = new StringBuilder();
            List<UsersEmail> userInfo = new List<UsersEmail>();
            //UserProfile.UsersInfo userInfo = new UserProfile.UsersInfo();
            //Get all mailTo  
            string[] escalationToRoles = escalationsRoles.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
            foreach (string escalationRole in escalationToRoles)
            {
                if (escalationRole == "EscalationManager" || escalationRole == "RequestTypeEscalationManager")
                {
                    List<UserProfile> managers = GetTicketEscalationManagers(ticketItem, moduleid, 0);
                    if (managers != null && managers.Count > 0)
                    {
                        foreach (UserProfile userVal in managers)
                        {
                            UsersEmail uEmail = new UsersEmail();
                            uEmail.ID = userVal.Id;
                            uEmail.UserName = userVal.Name;
                            uEmail.Email = userVal.Email;
                            userInfo.Add(uEmail);
                        }
                        UserManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
                    }
                }
                else if (escalationRole == "BackupEscalationManagerUser" || escalationRole == "RequestTypeBackupEscalationManager")
                {
                    List<UserProfile> managers = GetTicketEscalationManagers(ticketItem, moduleid, 1);
                    if (managers != null && managers.Count > 0)
                    {
                        foreach (UserProfile userVal in managers)
                        {
                            UsersEmail uEmail = new UsersEmail();
                            uEmail.ID = userVal.Id;
                            uEmail.UserName = userVal.Name;
                            uEmail.Email = userVal.Email;
                            userInfo.Add(uEmail);
                        }
                        UserManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
                    }
                }
                else if (escalationRole == "PRPManager")
                {
                    List<UserProfile> managers = GetTicketPRPORPManagers(ticketItem, moduleid, 0, UserManager);
                    if (managers != null && managers.Count > 0)
                    {
                        foreach (UserProfile userVal in managers)
                        {
                            UsersEmail uEmail = new UsersEmail();
                            uEmail.ID = userVal.Id;
                            uEmail.UserName = userVal.Name;
                            uEmail.Email = userVal.Email;
                            userInfo.Add(uEmail);
                        }
                        UserManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
                    }
                }
                else if (escalationRole == "ORPManager")
                {
                    List<UserProfile> managers = GetTicketPRPORPManagers(ticketItem, moduleid, 1, UserManager);
                    if (managers != null && managers.Count > 0)
                    {
                        foreach (UserProfile userVal in managers)
                        {
                            UsersEmail uEmail = new UsersEmail();
                            uEmail.ID = userVal.Id;
                            uEmail.UserName = userVal.Name;
                            uEmail.Email = userVal.Email;
                            userInfo.Add(uEmail);
                        }
                        UserManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
                    }
                }
                else
                {
                    UserManager.AddUsersFromItemField(ref userInfo, ticketItem, new[] { escalationRole }, false, false);
                }
            }
            string emailList = string.Join(",", userInfo.AsEnumerable().Select(x => x.Email).Distinct().ToList());
            if (userInfo != null && !string.IsNullOrEmpty(emailList))
            {
                mailTo.Append(emailList);
            }

            return mailTo.ToString();
        }

        ////new funtion to get all emailto for requesttpe.
        //private UserProfile.UsersInfo GetRequestTypeAllEmailTo(SPListItem requestType, SPListItem ticketItem, int moduleid, SPWeb spWeb, int slaType)
        //{
        //    StringBuilder mailTo = new StringBuilder();
        //    UserProfile.UsersInfo userInfo = new UserProfile.UsersInfo();

        //    if (slaType == 1)
        //    {
        //        // SLA Expiration email, also include Escalation roles from RequestType
        //        string RequestTypeSLAEscalationRoles = ConfigurationVariable.GetValue(spWeb, ConfigConstants.RequestTypeSLAEscalationRoles);
        //        string[] actionUserFields = RequestTypeSLAEscalationRoles.Split(';');

        //        foreach (string actionUserField in actionUserFields)
        //        {
        //            if (actionUserField == "PRPManager")
        //            {
        //                SPFieldUserValue manager = GetTicketPRPORPManagers(ticketItem, moduleid, 0, spWeb);
        //                if (manager != null)
        //                {
        //                    UserProfile.AddUsersFromFieldUserValue(ref userInfo, manager, spWeb, false, false);
        //                }
        //            }
        //            else if (actionUserField == "ORPManager")
        //            {
        //                SPFieldUserValue manager = GetTicketPRPORPManagers(ticketItem, moduleid, 1, spWeb);
        //                if (manager != null)
        //                {
        //                    UserProfile.AddUsersFromFieldUserValue(ref userInfo, manager, spWeb, false, false);
        //                }
        //            }
        //            else
        //            {
        //                UserProfile.AddUsersFromItemField(ref userInfo, requestType, actionUserField, spWeb, false, false);
        //            }
        //        }
        //    }

        //    // Get all action users
        //    string actionUserList = getStageActionUsers(ticketItem, spWeb);
        //    string[] actionUsers = UGITUtility.SplitString(actionUserList, Constants.Separator, StringSplitOptions.RemoveEmptyEntries);

        //    foreach (string actionUserField in actionUsers)
        //    {
        //        UserProfile.AddUsersFromItemField(ref userInfo, ticketItem, actionUserField, spWeb, false, false);
        //    }

        //    return userInfo;
        //}

        // to get prp/orp manager spfiledUservalue.


        /// <summary>
        /// Get ticket escalation managers using location and requesttype of ticket
        /// ManagerType: 
        /// Escalation Manager=0, Backup Escalation Manager=1
        /// </summary>
        /// <param name="ticketItem"></param>
        /// <param name="moduleId"></param>
        /// <param name="managerType">Escalation Manager=0, Backup Escalation Manager=1</param>
        /// <param name="spWeb"></param>
        /// <returns>return groups and users</returns>
        private List<UserProfile> GetTicketEscalationManagers(DataRow ticketItem, long moduleId, int managerType)
        {
            if (ticketItem == null)
                return null;

            List<UserProfile> escalationManagers = null;

            //Get ticket location which should be requestor location or user manually set location
            int ticketLocationID = Convert.ToInt32(Convert.ToString(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.LocationLookup)));

            //Get Ticket Request type id
            int requestTypeID = Convert.ToInt32(Convert.ToString(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketRequestTypeLookup)));

            //Get escalation manager using location and requestype.
            if (ticketLocationID > 0 && requestTypeID > 0)
            {
                string query = string.Format(" {0}={1} and {2}={3} and {4}='{5}'", DatabaseObjects.Columns.TicketRequestTypeLookup, requestTypeID, DatabaseObjects.Columns.LocationLookup, ticketLocationID, DatabaseObjects.Columns.TenantID, context.TenantID);
                DataRow[] requestTypeLocationColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestTypeByLocation, query).Select();
                
                if (requestTypeLocationColl.Count() > 0)
                {
                    if (managerType == 0)
                    {
                        escalationManagers = UserManager.GetUserInfosById(Convert.ToString(UGITUtility.GetSPItemValue(requestTypeLocationColl[0], DatabaseObjects.Columns.RequestTypeEscalationManager)));
                    }
                    else if (managerType == 1)
                    {
                        escalationManagers = UserManager.GetUserInfosById(Convert.ToString(UGITUtility.GetSPItemValue(requestTypeLocationColl[0], DatabaseObjects.Columns.RequestTypeBackupEscalationManager)));
                    }
                }
            }

            //Fetch escalation manager from default if escalation is not found for ticket location.
            if (escalationManagers == null || escalationManagers.Count <= 0)
            {
                DataTable moduleRequestTypeList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                string moduleName =  ObjModuleViewManager.LoadByID(moduleId).ModuleName;
                string query = string.Format(" {0}='{1}' and {2}={3} ", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.Id, requestTypeID);

                DataRow[] requestTypeCollection = moduleRequestTypeList.Select(query);
                if (requestTypeCollection.Count() > 0)
                {
                    if (managerType == 0)
                    {
                        escalationManagers = UserManager.GetUserInfosById(Convert.ToString(UGITUtility.GetSPItemValue(requestTypeCollection[0], DatabaseObjects.Columns.RequestTypeEscalationManager)));
                    }
                    else if (managerType == 1)
                    {
                        escalationManagers = UserManager.GetUserInfosById( Convert.ToString(UGITUtility.GetSPItemValue(requestTypeCollection[0], DatabaseObjects.Columns.RequestTypeBackupEscalationManager)));
                    }
                }
            }

            return escalationManagers;
        }


        public string getStageActionUsers(DataRow ticketItem)
        {
            string stageActionUser = string.Empty;

             //Get the ticket stage id.
            int ticketCurrentStageId = Convert.ToInt32(ticketItem[DatabaseObjects.Columns.ModuleStepLookup].ToString().Split(new char[] { ';' })[0]);

            //SPQuery moduleStepQuery = new SPQuery();
            // Get the current statge Id of ticket.
            string stageId = UGITUtility.SplitString(ticketItem[DatabaseObjects.Columns.ModuleStepLookup].ToString(), Constants.Separator, 0);

            //// Get the actual stage from module stage table.
            DataTable moduleStage = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' AND {DatabaseObjects.Columns.ID} = {ticketCurrentStageId}");

            if (moduleStage != null && moduleStage.Rows.Count>0)
            {
                stageActionUser = Convert.ToString(moduleStage.Rows[0][DatabaseObjects.Columns.ActionUser]);
            }
            return stageActionUser;
        }

        /// <summary>
        /// Add minutes to start time keeping in mind working / business hours
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="startTime">Start Time from which to calculate</param>
        /// <param name="minutesToAdd">Minutes to add to start time</param>
        /// <returns></returns>
        public DateTime AddTimeInWorkingHours(DateTime startTime, double minutesToAdd)
        {
            DateTime nextTime = startTime;

            // Get the working time stamp of currentStagedate from config/calendar list depends on configuration.
            string startDayWorkingHours = uHelper.GetWorkingHourOfWorkingDate(context , startTime);

            // split out the working Start time and End time.
            string[] WorkingdateTime = startDayWorkingHours.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
            DateTime WorkingStartDateTime = Convert.ToDateTime(WorkingdateTime[0]);
            DateTime WorkingEndDateTime = Convert.ToDateTime(WorkingdateTime[1]);

            // If ticket created before working hours, set start time to start of day
            if (startTime < WorkingStartDateTime)
                startTime = WorkingStartDateTime;

            // Get the remaining time of of start time's working day
            TimeSpan todaysRemainingTime = WorkingEndDateTime - startTime;

            // If adding escalation time stays within today's working hours, we are done!
            if (startTime.AddMinutes(minutesToAdd) <= WorkingEndDateTime)
            {
                nextTime = startTime.AddMinutes(minutesToAdd);
                return nextTime;
            }

            // In case the schedular runs AFTER the office closed time todays remaining time will be set to 0:0:0.
            if (todaysRemainingTime.TotalMinutes < 0)
                todaysRemainingTime = new TimeSpan(0, 0, 0);

            // get the remaining time after subtracting the today's remaining time from escalation time.
            TimeSpan escalationTime = new TimeSpan(0, Convert.ToInt32(minutesToAdd), 0);
            TimeSpan remainingTime = escalationTime - todaysRemainingTime;

            // now add up the remaining time in next working days untill it remains 0.
            while (remainingTime.TotalMinutes > 0)
            {
                // Get next working day's start, end and duration
                string nextWorkingDay = uHelper.GetNextWorkingDateAndTime(context,startTime);
                string[] nextWorkingDayStartDateTime = nextWorkingDay.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                DateTime nextWorkingStartDateTime = Convert.ToDateTime(nextWorkingDayStartDateTime[0]);
                //changed by munna
                DateTime nextWorkingEndDateTime = Convert.ToDateTime(nextWorkingDayStartDateTime[0]);//Convert.ToDateTime(nextWorkingDayStartDateTime[1]);
                TimeSpan workingDayDuration = nextWorkingEndDateTime - nextWorkingStartDateTime; // Note: can change day-to-day

                // If total remaining time is less than this day's duration, we should be done!
                // Just set next time by adding remaining time to day's start
                if (remainingTime <= workingDayDuration)
                {
                    nextTime = nextWorkingStartDateTime.Add(remainingTime);
                    break;
                }
                else
                {
                    // Else set start time to next working days's start so we can continue
                    startTime = Convert.ToDateTime(nextWorkingStartDateTime);

                    // And reduce remaining time by this day's duration
                    remainingTime = remainingTime - workingDayDuration;
                }
            }

            return nextTime;
        }

        /// <summary>
        /// It fetch stage start from module workflow history list
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <param name="requiredStage"></param>
        /// <returns></returns>
        public DateTime GetStageStartDate(LifeCycle lifeCycle, LifeCycleStage requiredStage, string ticketID)
        {
            DateTime stageStartTime = DateTime.MinValue;
            DataTable dtCollectionTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (dtCollectionTable != null && dtCollectionTable.Rows.Count > 0)
            {
                DataRow[] collection = dtCollectionTable.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.StageStep, requiredStage.StageStep));
                if (collection != null && collection.Count() > 0)
                {
                    dtCollectionTable = null;
                    dtCollectionTable = collection.CopyToDataTable();
                    dtCollectionTable.DefaultView.Sort = DatabaseObjects.Columns.StageStartDate;
                    dtCollectionTable = dtCollectionTable.DefaultView.ToTable();
                    stageStartTime = Convert.ToDateTime(dtCollectionTable.Rows[0][DatabaseObjects.Columns.StageStartDate]);
                }
            }
            return stageStartTime;
        }


        /// <summary>
        /// This method is used to get Assigned StageStartDate for a Ticket from ModuleWorkFlowHistory list
        /// </summary>
        /// <param name="ticketItem"></param>
        /// <param name="assignedStepId"></param>
        /// <param name="spWeb"></param>
        /// <returns>StageStartDate</returns>
        protected static DateTime GetTicketAssignedDate(ApplicationContext context, DataRow ticketItem, int assignedStepId)
        {
            DateTime ticketAssignedDate = new DateTime();
            ModuleWorkflowHistoryManager workflowHistoryManager = new ModuleWorkflowHistoryManager(context);
            List<ModuleWorkflowHistory> lstWorkFlowHistory = workflowHistoryManager.Load(x => x.TicketId == Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]) && x.StageStep == assignedStepId);

            if (lstWorkFlowHistory != null && lstWorkFlowHistory.Count > 0)
            {
                ticketAssignedDate = lstWorkFlowHistory[0].StageStartDate;
            }

            return ticketAssignedDate;
        }

        private void AddUpdateSVCScheduleAction(ApplicationContext context, DataRow ticket, SLAConfiguration slaconfig, DateTime? slaExpirationTime, DateTime nextEscalationTime, long moduleId, string slaRequestType, int slaType, bool use24x7Calendar = false)
        {
            string ticketId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            ConfigurationVariableManager obcConfiguationVariableHelper = new ConfigurationVariableManager(context);
            DateTime workDayStartTime = Convert.ToDateTime(obcConfiguationVariableHelper.GetValue("WorkdayStartTime"));

            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule module = moduleViewManager.LoadByID(moduleId);
            UserProfileManager profileManager = new UserProfileManager(context);
            List<UsersEmail> userInfo = new List<UsersEmail>();
            string mailTo = string.Empty;

            if (slaconfig != null && slaconfig.EscalationTo != null && module != null && module.ModuleName == ModuleNames.SVC)
            {
                List<UserProfile> lstUsers = profileManager.GetUserInfosById(slaconfig.EscalationTo);
                foreach (UserProfile user in lstUsers)
                {
                    if (string.IsNullOrEmpty(user.Email))
                        continue;

                    UsersEmail uEmail = new UsersEmail();
                    uEmail.ID = user.Id;
                    uEmail.UserName = user.Name;
                    uEmail.Email = user.Email;
                    userInfo.Add(uEmail);
                }
                profileManager.AddUsersFromFieldUserValue(ref userInfo, false, false);
            }
            if ((userInfo == null || !userInfo.Exists(x=> !string.IsNullOrEmpty(x.Email))) && string.IsNullOrEmpty(slaconfig.EscalationEmailTo))
            {
                return; // No one to send to
            }

            mailTo = string.Join(",", userInfo.AsEnumerable().Select(x => x.Email).Distinct().ToList());
            
            if (!string.IsNullOrEmpty(slaconfig.EscalationEmailTo))
                mailTo += "," + slaconfig.EscalationEmailTo;

            /// email subject 
            string subject = string.Format("Service {0} Escalation", ticketId);
            string[] tokens = uHelper.GetMyTokens(subject);
            subject = ReplaceMyTokensWithTicketValues(ticket, subject, tokens);

            /// email body
            StringBuilder emailBody = new StringBuilder();
            string greeting = obcConfiguationVariableHelper.GetValue(ConfigConstants.Greeting);
            string signature = obcConfiguationVariableHelper.GetValue(ConfigConstants.Signature);
            string expirationTimeMessage = string.Empty;
            
            if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.NextSLATime))
                expirationTimeMessage = string.Format("<b>SLA Expiration:</b> {0:MMM-dd-yyyy hh:mm tt}<br/><br/>", slaExpirationTime != null ? slaExpirationTime : Convert.ToDateTime(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.NextSLATime)));

            string body = string.Empty;
            if (slaType == 1) // SLA Expiration
            {
                body = obcConfiguationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAEscalationBody);
                if (!string.IsNullOrEmpty(body))
                {
                    // Replace SLA type token in case we get that from configured body, other standard tokens replaced in ReplaceMyTokensWithTicketValues()
                    body = body.Replace("[$SLAType$]", slaRequestType.ToLower());
                    tokens = uHelper.GetMyTokens(body);
                    body = ReplaceMyTokensWithTicketValues(ticket, body, tokens);
                }
                else
                {
                    // Default body
                    body = string.Format("The {1} SLA for Service <b>{0}</b> has <b>EXPIRED</b> and needs your attention.",
                                                    ticketId, slaRequestType.ToLower());
                }
            }
            else // SLA Pre-expiration Warning
            {
                body = obcConfiguationVariableHelper.GetValue(ConfigConstants.RequestTypeSLAWarningBody);
                if (!string.IsNullOrEmpty(body))
                {
                    // Replace SLA type token in case we get that from configured body, other standard tokens replaced in ReplaceMyTokensWithTicketValues()
                    body = body.Replace("[$SLAType$]", slaRequestType.ToLower());
                    tokens = uHelper.GetMyTokens(body);
                    body = ReplaceMyTokensWithTicketValues(ticket, body, tokens);
                }
                else
                {
                    // Default body
                    body = string.Format("The {1} SLA for Service <b>{0}</b> is about to expire and needs your attention.",
                                                    ticketId, slaRequestType.ToLower());
                }
            }

            string userNames = string.Empty;

            if (userInfo != null && userInfo.Exists(x => !string.IsNullOrEmpty(x.UserName)))
                userNames = string.Join(",", userInfo.Where(y => !string.IsNullOrEmpty(y.UserName)).Select(x => x.UserName).Distinct().ToList());

            emailBody.Append(string.Format(@"{0} {1}<br/><br/>
                                             {2}<br/><br/>
                                             {3}
                                             {4}<br/>", greeting, userNames, body, expirationTimeMessage, signature));

            string Footer = uHelper.GetTicketDetailsForEmailFooter(context, ticket, uHelper.getModuleNameByTicketId(Convert.ToString(ticket[DatabaseObjects.Columns.TicketId])), true, false);
            emailBody.Append(Footer);

            Dictionary<string, object> scheduleDic = new Dictionary<string, object>();

            scheduleDic.Add(DatabaseObjects.Columns.StartTime, nextEscalationTime);
            scheduleDic.Add(DatabaseObjects.Columns.Title, ticketId.ToString() + " " + uHelper.GetDateStringInFormat(context, nextEscalationTime, true));
            scheduleDic.Add(DatabaseObjects.Columns.TicketId, ticketId);
            scheduleDic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.EscalationEmail);
            scheduleDic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleId);
            scheduleDic.Add(DatabaseObjects.Columns.EmailIDTo, mailTo);
            scheduleDic.Add(DatabaseObjects.Columns.EmailIDCC, string.Empty);
            scheduleDic.Add(DatabaseObjects.Columns.MailSubject, subject);
            scheduleDic.Add(DatabaseObjects.Columns.EmailBody, emailBody.ToString());

            if (slaconfig != null && slaconfig.EscalationFrequency > 0)
            {
                scheduleDic.Add(DatabaseObjects.Columns.Recurring, true);
                scheduleDic.Add(DatabaseObjects.Columns.RecurringInterval, slaconfig.EscalationFrequency);
            }
            else
                scheduleDic.Add(DatabaseObjects.Columns.Recurring, false);

            string customProperties = string.Empty;
            if (slaType == 1) // SLA Expiration post-expiration
                customProperties = string.Format("title={0} Service SLA Post-Expiration", slaRequestType);
            else // SLA Warning pre-expiration
                customProperties = string.Format("title={0} Service SLA Pre-Expiration", slaRequestType);


            //set BusinessHours=false in customproperties enable logic to set next recuring escalation based 24x7 not working hours
            if (use24x7Calendar)
                customProperties = string.Format("{0}{1}BusinessHours=false", customProperties, Constants.Separator);

            scheduleDic.Add(DatabaseObjects.Columns.CustomProperties, customProperties);

            AgentJobHelper agent = new AgentJobHelper(context);
            agent.CreateSchedule(scheduleDic);
        }
        public static DateTime GetSLACompletionDate(ApplicationContext spWeb, DataRow ticketItem, Ticket ticketRequest, string escalationType)
        {
            DateTime slaCompletionDate = DateTime.MinValue;
            //pick creation date of ticket as start time
            DateTime ticketCreationTime = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketCreationDate))
                ticketCreationTime = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketCreationDate]);
            else
                ticketCreationTime = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.Created]);

            // Find value of Use24x7Calendar field if RequesType is enabled for current ticket
            bool use24x7Calendar = false;
            long ticketReqTypeLookupValue = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);

            if (ticketReqTypeLookupValue > 0)
            {

                ModuleRequestType requestType = ticketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == ticketReqTypeLookupValue);
                if (requestType != null)
                    use24x7Calendar = requestType.Use24x7Calendar;
            }

            //Find sla completion date based on its duration
            double duration = GetSALRuleDuration(spWeb, ticketItem, ticketRequest, escalationType);

            if (duration > 0 && !use24x7Calendar)
                slaCompletionDate = uHelper.GetWorkingEndDate(spWeb, ticketCreationTime, duration, isSLA: true);
            else if (duration > 0 && use24x7Calendar)
                slaCompletionDate = ticketCreationTime.AddMinutes(duration);

            return slaCompletionDate;
        }

        private static double GetSALRuleDuration(ApplicationContext spWeb, DataRow ticketItem, Ticket ticketRequest, string escalationType)
        {
            double slaDuration = 0;
            long ticketReqTypeLookupValue = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
            if (ticketReqTypeLookupValue > 0)
            {
                bool lRequestTypeExist = false;
                if (UGITUtility.IfColumnExists(ticketItem, DatabaseObjects.Columns.LocationLookup))
                {
                    long location = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.LocationLookup]);
                    if (location > 0)
                    {
                        ModuleRequestTypeLocation locationRequestType = ticketRequest.Module.List_RequestTypeByLocation.FirstOrDefault(x => x.RequestType != null && x.RequestTypeLookup == ticketReqTypeLookupValue
                        && x.Location != null && x.LocationLookup == location);
                        if (locationRequestType != null)
                        {
                            if (escalationType == Constants.SLACategory.Resolution)
                                slaDuration = locationRequestType.ResolutionSLA;
                            else if (escalationType == Constants.SLACategory.Assignment)
                                slaDuration = locationRequestType.AssignmentSLA;
                            else if (escalationType == Constants.SLACategory.RequestorContact)
                                slaDuration = locationRequestType.RequestorContactSLA;
                            else if (escalationType == Constants.SLACategory.Close)
                                slaDuration = locationRequestType.CloseSLA;

                            lRequestTypeExist = true;
                        }
                    }
                }

                if (!lRequestTypeExist)
                {
                    ModuleRequestType requestType = ticketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == ticketReqTypeLookupValue);
                    if (requestType != null)
                    {
                        if (escalationType == Constants.SLACategory.Resolution)
                            slaDuration = requestType.ResolutionSLA;
                        else if (escalationType == Constants.SLACategory.Assignment)
                            slaDuration = requestType.AssignmentSLA;
                        else if (escalationType == Constants.SLACategory.RequestorContact)
                            slaDuration = requestType.RequestorContactSLA;
                        else if (escalationType == Constants.SLACategory.Close)
                            slaDuration = requestType.CloseSLA;
                    }
                }
            }

            // If none of the RequestType-level SLAs are active, then the module-priority level SLAs take effect
            if (slaDuration <= 0)
            {
                SlaRulesManager slaRulesManager = new SlaRulesManager(spWeb);
                List<ModuleSLARule> slaRuleTable = slaRulesManager.Load(x => x.TenantID == spWeb.TenantID);
                
                if (slaRuleTable == null || slaRuleTable.Count==0)
                    return 0;

                long priorityLookupValue = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.TicketPriorityLookup]);
                if (priorityLookupValue > 0)
                {
                    ModuleSLARule slaRow = slaRuleTable.FirstOrDefault(x => x.ModuleNameLookup == ticketRequest.Module.ModuleName &&
                x.SLACategoryChoice == escalationType && x.PriorityLookup == priorityLookupValue);
                    if (slaRow == null)
                        return 0;

                    //sla in minutes
                    slaDuration = UGITUtility.StringToDouble(slaRow.SLAHours) * 60;
                }
            }

            return slaDuration;
        }
    }
}
