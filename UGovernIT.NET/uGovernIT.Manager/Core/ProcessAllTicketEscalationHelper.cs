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
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class ProcessAllTicketEscalationHelper
    {
        private static bool IsProcessActive;
        EscalationProcess objEscalationProcess;
        ApplicationContext _context;
        ModuleViewManager ObjModuleManager;
        TicketManager ObjTicketManager;
        public ProcessAllTicketEscalationHelper(ApplicationContext context)
        {
            _context = context;
            objEscalationProcess = new EscalationProcess(context);
            ObjModuleManager = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
        }
        public bool ProcessState()
        {
            return IsProcessActive;
        }
         
        /// <summary>
        /// Call function to re-generate escalations for tickets (Only SMS)
        /// Check process state before call it because function will not perform any operation if it is already in running state.
        /// </summary>
        public void ReGenerateAllEscalations()
        {
            if (IsProcessActive)
                return;

            ULog.WriteLog("Regenerating all escalations...");
            int numEscalationsCreated = 0;

            try
            {
                IsProcessActive = true;

                // Get list of modules with escalations configured
                List<string> lstModuleNames = GetInvolvedModules();

                // Delete existing escalations for these modules
                ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
                List<SchedulerAction> lstScheduleActions = scheduleActionsManager.Load(x => x.ActionType == ScheduleActionType.EscalationEmail.ToString()
                    && !string.IsNullOrEmpty(x.ModuleNameLookup) && lstModuleNames.Contains(x.ModuleNameLookup));

                if (lstScheduleActions != null && lstScheduleActions.Count > 0)
                {
                    ULog.WriteLog(string.Format("Deleting {0} existing escalations...", lstScheduleActions.Count));
                    scheduleActionsManager.Delete(lstScheduleActions);
                }

                // Recreate escalations for these modules
                List<UGITModule> lstUGITModules = ObjModuleManager.Load(x => !string.IsNullOrEmpty(x.ModuleName) && lstModuleNames.Contains(x.ModuleName) 
                    && !string.IsNullOrEmpty(x.ModuleTable)).ToList();              // x.ModuleType == ModuleType.SMS

                foreach (UGITModule module in lstUGITModules)
                {
                    // NOTE: Escalations are for open tickets only, so get list of all open tickets for current module
                    DataTable openTickets = ObjTicketManager.GetOpenTickets(module);
                    
                    if (openTickets == null || openTickets.Rows.Count == 0)
                        continue;

                    int numModuleEscalations = 0;
                    ULog.WriteLog(string.Format("Recreating escalations for {0} open {1} tickets ...", openTickets.Rows.Count, module.ModuleName));
                    Ticket ticketRequest = new Ticket(_context, module.ModuleName);

                    // Generate escalation for each open ticket of current module
                    foreach (DataRow ticketItem in openTickets.Rows)
                    {
                        string ticketId = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
                        try
                        {
                            objEscalationProcess.GenerateEscalation(ticketRequest, ticketItem, module.ID);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error generating escalation for ticket " + ticketId);
                        }
                        numModuleEscalations++;
                    }

                    ULog.WriteLog(string.Format("Created {0} escalations for module {1}", numModuleEscalations, module.ModuleName));
                    numEscalationsCreated += numModuleEscalations;
                }

                IsProcessActive = false;
            }
            catch (Exception ex)
            {
               ULog.WriteException(ex);
            }
            finally
            {
                IsProcessActive = false;
                ULog.WriteLog(string.Format("Finished Regenerating all escalations, {0} total escalations created", numEscalationsCreated));
            }
        }

        /// <summary>
        /// This method is used to get a list of Modules with escalations configured
        /// </summary>
        /// <returns></returns>
        private List<string> GetInvolvedModules()
        {
            List<string> lstModuleNames = new List<string>();
            
            // Find modules involved in Priority-based SLA escalations
            SlaRulesManager rulesManager = new SlaRulesManager(_context);
            List<ModuleSLARule> lstSLARules = rulesManager.Load();

            ModuleEscalationRuleManager escalationRuleManager = new ModuleEscalationRuleManager(_context);
            List<ModuleEscalationRule> lstEscalationRules = escalationRuleManager.Load();

            if (lstSLARules != null && lstEscalationRules != null)
            {
                lstModuleNames = (from a in lstSLARules
                                  join b in lstEscalationRules on a.ID equals b.SLARuleIdLookup
                                  select a.ModuleNameLookup).ToList();
            }

            // Find modules involved in Request Type based escalations
            RequestTypeManager requestTypeManager = new RequestTypeManager(_context);
            List<ModuleRequestType> lstRequestType = requestTypeManager.Load(x => !string.IsNullOrEmpty(x.ModuleNameLookup) 
                && (x.AssignmentSLA > 0 || x.CloseSLA > 0 || x.ResolutionSLA > 0 || x.RequestorContactSLA > 0));

            if (lstRequestType != null)
                lstModuleNames.AddRange(lstRequestType.Select(y => y.ModuleNameLookup));

            RequestTypeByLocationManager typeByLocationManager = new RequestTypeByLocationManager(_context);
            List<ModuleRequestTypeLocation> lstReqTypeByLocation = typeByLocationManager.Load(x => !string.IsNullOrEmpty(x.ModuleNameLookup) 
                && (x.AssignmentSLA > 0 || x.CloseSLA > 0 || x.ResolutionSLA > 0 || x.RequestorContactSLA > 0));

            if(lstReqTypeByLocation != null)
                lstModuleNames.AddRange(lstReqTypeByLocation.Select(y => y.ModuleNameLookup));

            // Check if we have any services with escalations configured, if so add SVC to the module list
            ServicesManager servicesManager = new ServicesManager(_context);
            List<Services> lstServices = servicesManager.Load(x => !x.SLADisabled);
            if (lstServices != null && lstServices.Count > 0)
                lstModuleNames.Add(ModuleNames.SVC);

            // Make the list to have distinct values
            lstModuleNames = lstModuleNames.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

            return lstModuleNames;
        }
    }
}
