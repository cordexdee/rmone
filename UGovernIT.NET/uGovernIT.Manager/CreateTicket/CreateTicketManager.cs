using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using System.Web.UI;
using System.Web;

namespace uGovernIT.Manager
{
   
    public class CreateTicketManager
    {
        public string currentModuleName;
        public string currentTicketPublicID;
        public string strResolutionTime = string.Empty;
        public string error = string.Empty;

        public bool? isAddREsolutionTime = false;

        public UserProfile user;
        public AgentSummary agentSummary;



        private LifeCycleStage currentStage;
        public LifeCycle lifeCycle;
        private ConfigurationVariableManager _configurationVariableManager=null;
        private ApplicationContext _context = null;

        protected DataRow saveTicket;

        protected string PRP = string.Empty;

        protected bool adminOverride;
        protected bool confirmChildTicketsClose;

        protected Ticket TicketRequest;

        public CreateTicketManager(ApplicationContext context)
        {
            _context = context;
           
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {

            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(_context);
                }
                return _configurationVariableManager;
            }
        }




        private void NewCreateTicket(bool valid, string senderID, List<TicketColumnError> errors, List<TicketColumnValue> formValues, bool updateChangesInHistory, string userID)
        {
            //if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate))
            //{
            //    if (saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
            //        oldtargetcompletiondate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;
            //}
            //if (currentModuleName != ModuleNames.CMDB)
            //{

            //    resetPasswordAgent = PasswordResetAgent(formValues);

            //    if (resetPasswordAgent.IsResetPasswordAgentActivated == false)
            //    {
            //        Boolean AutoFillTicket = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoFillTicket);
            //        if (AutoFillTicket)
            //        {
            //            AutoUpdateTicket(formValues);
            //        }
            //    }

            //}


            // TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, userID);

            //if (currentModuleName != ModuleNames.CMDB)
            //{
            //    if (resetPasswordAgent.IsResetPasswordAgentActivated == true)
            //    {
            //        if (resetPasswordAgent.IsRequestorIsGroup == true || resetPasswordAgent.IsInitiatorEqualRequestor == true)
            //        {
            //            string restpasswordagentmessage = string.Empty;
            //            if (resetPasswordAgent.IsInitiatorEqualRequestor == true)
            //            {
            //                restpasswordagentmessage = $"Initiator and Requestor cannot be same for Password Reset Agent. Please select valid user as requestor.";
            //                ResetPasswordCloseMessage = $"New Password is not reset";
            //            }
            //            else
            //            {
            //                restpasswordagentmessage = $"Password Reset Agent does not work with group as Requestor. Please select valid user as requestor.";
            //                ResetPasswordCloseMessage = $"New Password is not reset";
            //            }


            //            saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, restpasswordagentmessage, saveTicket, DatabaseObjects.Columns.TicketComment, false);
            //        }
            //        else
            //        {
            //            ResetPasswordCloseMessage = $"New password was sent to {resetPasswordAgent.requestors}";
            //        }

            //    }
            //}



            //TicketRequest.Create(saveTicket, user, PRP, resetPasswordAgent);

            //            PasswordResetMail(resetPasswordUserList, saveTicket[DatabaseObjects.Columns.TicketId].ToString());

            //          string error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: true, agentSummary: agentSummary);

            #region Dependent Code
            #region Save Asset mapping if needed
            //TicketColumnValue assetLookupVal = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.AssetLookup);
            //if (assetLookupVal != null)
            //{
            //    if (!string.IsNullOrEmpty(Convert.ToString(assetLookupVal.Value)))
            //    {
            //        string[] valuechek = UGITUtility.SplitString(assetLookupVal.Value,",");

            //        if (valuechek.Count() > 1)
            //        {
            //            if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.AssetMultiLookup))
            //            saveTicket[DatabaseObjects.Columns.AssetMultiLookup] = Convert.ToString(assetLookupVal.Value);
            //            saveTicket[DatabaseObjects.Columns.AssetLookup] = 0;
            //        }
            //        else
            //        {
            //            saveTicket[DatabaseObjects.Columns.AssetLookup] = Convert.ToString(assetLookupVal.Value);
            //            if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.AssetMultiLookup))
            //                saveTicket[DatabaseObjects.Columns.AssetMultiLookup] = string.Empty;
            //        }

            //            //Assets.CreateRelationWithIncident(assetLookup.LookupId.ToString(), saveTicket[DatabaseObjects.Columns.TicketId].ToString());
            //            //AssetTicketRelationship.CreateAssetHistory(assetLookup.LookupId.ToString(), saveTicket);
            //         //.LookupId.ToString(); 
            //    }
            //    if(string.IsNullOrEmpty(Convert.ToString(assetLookupVal.Value)))
            //    {
            //        saveTicket[DatabaseObjects.Columns.AssetLookup] = 0;
            //        if(UGITUtility.IsSPItemExist(saveTicket,DatabaseObjects.Columns.AssetMultiLookup))
            //        saveTicket[DatabaseObjects.Columns.AssetMultiLookup] = string.Empty;
            //    }                
            //}
            #endregion
            //#region QuickClose and close
            //TicketRequest.QuickClose(moduleId, saveTicket, senderID);

            //if (resetPasswordAgent != null)
            //{
            //    if (resetPasswordAgent.IsResetPasswordAgentActivated)
            //    {

            //        TicketRequest.CloseTicket(saveTicket, uHelper.GetCommentString(user, ResetPasswordCloseMessage, saveTicket, DatabaseObjects.Columns.TicketResolutionComments, false));

            //    }
            //}
            //#endregion
            //currentTicketId = UGITUtility.StringToInt(Convert.ToString(saveTicket["ID"]));
            // currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
            //saveTicket = SPListHelper.GetSPListItem(thisList, currentTicketId);
            // currentTicketIdHidden.Value = Convert.ToString(saveTicket["ID"]); //saveTicket.ID.ToString();
            //currentTicket = saveTicket;
            #endregion

            //ModuleWorkflowHistory moduleWorkflowHistory = new ModuleWorkflowHistory();


            //Commented By Munna! 22-03-2017
            error = TicketRequest.CommitChanges(saveTicket, senderID, HttpContext.Current.Request.Url, agentSummary: agentSummary);

            if (!string.IsNullOrEmpty(error))
            {
                errors.Add(TicketColumnError.AddError(error));
                valid = false;
            }

            if (!string.IsNullOrEmpty(PRP) && !saveTicket.IsNull(DatabaseObjects.Columns.TicketRequestTypeLookup) && currentModuleName != ModuleNames.CMDB)
            {
                isAddREsolutionTime = true;

                ApproveTicketRequest( saveTicket, errors, agentSummary, "approvebuttonhidden", true);
            }


            //if (valid)
            //{
            //    panelNewTicket.Style.Add("display", "block");
            //    panelDetail.Style.Add("display", "none");
            //    string moduleName = Convert.ToString(currentModuleName);
            //    if (userID != null)
            //    {
            //        #region Show popup with new ticket ID
            //        //keep ticket open when keepticketopen is enable for module
            //        if (TicketRequest.Module.KeepItemOpen && senderID == "saveAsDraftButton")
            //        {
            //            UGITUtility.CreateCookie(Response, currentModuleName + "-" + SELECTEDTABCOOKIE, hdnActiveTab.Value);
            //            var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            //            qs.Set("TicketId", Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
            //            string url = Request.Url.AbsoluteUri.Split('?')[0];
            //            Response.Redirect(string.Format("{0}?{1}", UGITUtility.GetAbsoluteURL(url), qs.ToString()));
            //        }
            //        else
            //        {
            //            //if (!TicketRequest.Module.DisableNewConfirmation &&  /*!UGITUtility.StringToBoolean(Request["hpac"])&&*/ !TicketRequest.IsQuickClose(currentTicket))
            //            if (!TicketRequest.Module.DisableNewConfirmation && !TicketRequest.IsQuickClose(currentTicket))
            //            {
            //                // var dataExist = GetTableDataManager.IsLookaheadTicketExists(moduleName, TenantID, Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]), Convert.ToString(saveTicket[DatabaseObjects.Columns.ID]), Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]));
            //                var list = (List<string>)Session["relatedTicket"];
            //                if (list != null && list.Count > 0)
            //                {
            //                    foreach (string val in list)
            //                    {
            //                        //if (Module != null && Module.Trim() != string.Empty && !string.IsNullOrEmpty(val))
            //                        //{
            //                        TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), val);
            //                        int rowEffected = tRelation.CreateRelation(context);
            //                        //}
            //                    }
            //                }

            //                Session.Clear();


            //                string newModuleTicketTitle = string.Format("{0}: {1}", saveTicket[DatabaseObjects.Columns.TicketId], UGITUtility.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]), 100, string.Empty)));
            //                string newModuleTicketLink = string.Format("<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{1}','ticketId={0}','{2}', 90, 90, 0, '{3}' );  \">{0}</a>", saveTicket[DatabaseObjects.Columns.TicketId], currentModuleListPagePath, newModuleTicketTitle, Request["source"]);
            //                string typeName = UGITUtility.newTicketTitle(moduleName);
            //                string informationMsg = string.Format("<div class='informationMsg'><div class='infoMsgSuccess-title'>{0} Created: <div class='cteatedTicketNum'>{1}</div></div></div>",
            //                                                        typeName, newModuleTicketLink);
            //                string header = string.Format("{0} Created", typeName);
            //                TicketRequest.UpdateTicketCache(currentTicket, currentModuleName);
            //                uHelper.InformationPopup(Context, Server.UrlEncode(informationMsg), header);
            //                uHelper.ClosePopUpAndEndResponse(Context, false, Convert.ToString(Request.Url));
            //                //}
            //            }
            //            else
            //            {
            //                uHelper.ClosePopUpAndEndResponse(Context);
            //            }
            //        }
            //        #endregion
            //    }
            //    #region RelatedTicket
            //    if (!UGITUtility.StringToBoolean(Request["OnlyCopy"]))
            //    {
            //        // If the ticket is creating from related ticket page.
            //        string realtedTicketId = HttpContext.Current.Request["ParentId"];
            //        if (!string.IsNullOrEmpty(realtedTicketId))
            //        {
            //            TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, realtedTicketId, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
            //            int rowEffected = tRelation.CreateRelation(HttpContext.Current.GetManagerContext());
            //        }
            //        // Used for Ticket if it has the Related ticket id as parent ticket id in its New Ticket Form (Like In DRQ Module).
            //        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.RelatedRequestID, currentTicket.Table) && Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]) != "N/A" && Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]) != string.Empty)
            //        {
            //            TicketRelationshipHelper t1 = new TicketRelationshipHelper(context, Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]), Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
            //            t1.CreateRelation(HttpContext.Current.GetManagerContext());
            //        }
            //    }
            //    #endregion

            //    #region add exit criteria tasks from templates
            //    UGITModuleConstraint.CreateModuleStageTasksInTicket(context, currentTicketPublicID, moduleName);
            //    UGITModuleConstraint.ConfigureCurrentModuleStageTask(context, saveTicket);
            //    #endregion
            //}

            ////new line of code for change ticket type....
            //ArchiveOldTicket();
            ////return valid;
            //TicketRequest.UpdateTicketCache(currentTicket, currentModuleName);
        }

        public void ApproveTicketRequest(DataRow saveTicket,List<TicketColumnError> errors, AgentSummary agentSummary, string senderId, bool checkChildTickets = false,string moduleName=null)
        {
            try
            {

                TicketRequest = new Ticket(_context, moduleName);

                lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault();
                currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);

                if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                {
                    currentStage = lifeCycle.Stages[0];
                }


             LifeCycleStage oldCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);

            string error = TicketRequest.Approve(user, saveTicket, adminOverride);

            if (!taskValidation(oldCurrentStage,saveTicket))
            {
                error = "Complete all linked tasks of current stage";
            }
            LifeCycleStage newCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);
            if (string.IsNullOrEmpty(error) && checkChildTickets && newCurrentStage.StageTypeChoice == Convert.ToString(StageType.Closed) && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCloseChildTickets))
            {
                TicketRelationshipHelper tHelper = new TicketRelationshipHelper(_context);
                List<TicketRelation> ticketChilds = tHelper.GetTicketChildList(currentTicketPublicID);
                if (ticketChilds != null && ticketChilds.Count > 0)
                {
                    confirmChildTicketsClose = true;
                    error = "closechildtickets";
                }
            }

            if (string.IsNullOrEmpty(error))
            {
                TicketRequest.AssignModuleSpecificDefaults(saveTicket);

                error = TicketRequest.CommitChanges(saveTicket, senderId, HttpContext.Current.Request.Url, agentSummary: agentSummary);

                // Update Ticket.
                if (string.IsNullOrEmpty(error))
                {
                    //AutoSetResolutionTime(saveTicket[DatabaseObjects.Columns.ModuleNameLookup].ToString(), saveTicket[DatabaseObjects.Columns.Title].ToString());
                    //Send Email notification.
                    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), saveTicket, moduleName, null, isAddREsolutionTime, strResolutionTime);

                    //This will ensure start date and end date are updated if they have previous dates from template
                    UGITModuleConstraint.ConfigureCurrentModuleStageTask(_context, currentStage.StageStep + 1, currentTicketPublicID);
                }
                else
                {
                    errors.Add(TicketColumnError.AddError(error));
                }
            }
            else
            {
                errors.Add(TicketColumnError.AddError(error));
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create funtion for use in approve and update button.
        /// </summary>
        /// <param name="thisWeb"></param>
        /// <returns></returns>
        /// 
        private bool taskValidation(LifeCycleStage oldCurrentStage,DataRow saveTicket)
        {
            //LifeCycleStage oldCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);
            UGITTaskManager taskManager = new UGITTaskManager(_context);


            List<UGITTask> depncies = taskManager.LoadByProjectID(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));

            List<UGITTask> currentStageTask = depncies.Where(x => x.StageStep == oldCurrentStage.StageStep && x.Status != "Completed").ToList();

            if (currentStageTask.Count > 0)
            {
                return false;
            }
            return true;
        }


    }
}
