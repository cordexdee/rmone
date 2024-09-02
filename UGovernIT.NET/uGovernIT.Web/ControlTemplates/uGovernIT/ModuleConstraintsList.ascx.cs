using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Web
{
    public partial class ModuleConstraintsList : UserControl
    {
        protected string ConstraintTaskUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagetask");
        protected string ConstraintRuleUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagerule");
        protected DataTable DtConditions;
        public string TicketPublicID = string.Empty;
        public string ModuleName = string.Empty;
        public string ModuleStageId = string.Empty;
        public string ModuleStepId = string.Empty;
        protected DataRow CurrentTicket;
        protected bool IsLastModuleStage = false;
        public bool enablePrint;
        protected int viewType = 0;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleStageConstraintsManager SplstConditions;
        //ModuleViewManager moduleViewManager;
        UGITModule uGITModule;
        ConfigurationVariableManager objConfigurationVariableManager;
        UserProfileManager userProfileManager;
        ModuleStageConstraints moduleStageConstraint = null;
        #region page events
        protected override void OnInit(EventArgs e)
        {
            userProfileManager = new UserProfileManager(context);
            objConfigurationVariableManager = new ConfigurationVariableManager(context);
            SplstConditions = new ModuleStageConstraintsManager(context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            uGITModule = moduleViewManager.LoadByName(ModuleName);
            //Add a check if all public variables are loaded, if not load them else return.
            CurrentTicket = Ticket.GetCurrentTicket(context, this.ModuleName, TicketPublicID);
            lblMessage.Text = string.Format("The {0} will not move to the next stage till these tasks are complete.", UGITUtility.moduleTypeName(this.ModuleName).ToLower());
            string javaScript = string.Format("spGridViewExpandViewGroup('{0}',{1});", spGridConstraintList.ID, 0);
            Page.ClientScript.RegisterStartupScript(typeof(string), "expandviewgroup", javaScript, true);
            ModuleStepId = UGITModuleConstraint.GetModuleStepIdFromStage(context, this.ModuleStageId, this.ModuleName);
            SetActionButtonConfig();
            if (Request["enablePrint"] != null && Convert.ToBoolean(Request["enablePrint"]))
            {
                newConstraint.Visible = false;
                enablePrint = Convert.ToBoolean(Request["enablePrint"]);
            }
            var source = Convert.ToString(Request["Source"]);
            ConstraintTaskUrl = string.Format("{0}&source={1}", ConstraintTaskUrl, source);
            BindGridView();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // BindGridView();
        }

        #endregion

        #region spgridView actions
        protected void spGridConstraintList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "MarkAsComplete")
            {
                int taskID = int.Parse(e.CommandArgument.ToString());
                if (taskID > 0)
                {
                    MarkTaskAsComplete(int.Parse(e.CommandArgument.ToString()));
                }
                BindGridView();
                //TaskCache.ReloadProjectTasks(Constants.ExitCriteria, TicketPublicID);
            }
            else if (e.CommandName == "Delete")
            {

            }

        }


        protected void spGridConstraintList_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            ASPxGridView view = sender as ASPxGridView;
            DataTable dt = UGITUtility.ObjectToData(view.GetRow(e.VisibleIndex));
            if (dt.Rows.Count > 0)
            {
                DataRow row = UGITUtility.ObjectToData(view.GetRow(e.VisibleIndex)).Rows[0];// view.GetDataRow(e.VisibleIndex);
                HiddenField hiddenContentType = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "hiddenContentType") as HiddenField; //(HiddenField)e.Row.FindControl("hiddenContentType");
                HtmlImage image = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "taskType") as HtmlImage;//e.Row.FindControl("taskType");
                if (hiddenContentType.Value.Equals(DatabaseObjects.ContentType.ModuleTaskCT))
                    image.Src = "/content/images/ittask.png";
                else if (hiddenContentType.Value.Equals(DatabaseObjects.ContentType.ModuleRuleCT))
                    image.Src = "/content/images/ittask.png";
                else if (hiddenContentType.Value.Equals(DatabaseObjects.ContentType.DocumentApproveTaskCT))
                    image.Src = "/content/images/ittask.png";
                bool showActionSection = false;
                HtmlContainerControl actionSection = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "actionButtonsSection") as HtmlContainerControl;// e.Row.FindControl("actionButtonsSection");
                                                                                                                                                                           //string assignedTo = e.GetValue("AssignedTo") as string; //Convert.ToString(rowView.Row[DatabaseObjects.Columns.AssignedTo]);
                                                                                                                                                                           // Commented above line, as assignedTo is getting Names instead of UserIds.
                long taskId = Convert.ToInt64(e.GetValue("ID"));
                moduleStageConstraint = SplstConditions.LoadByID(taskId);
                string assignedTo = string.Empty;
                if (moduleStageConstraint != null)
                    assignedTo = moduleStageConstraint.AssignedTo;

                if (!string.IsNullOrEmpty(assignedTo))
                {
                    string[] users = UGITUtility.SplitString(assignedTo, Constants.Separator6);
                    //foreach (string user in users)
                    //{

                    //    UserProfile assignedToUser = userProfileManager.GetUserById(user);
                    //    if (assignedToUser != null && assignedToUser.Name == "Admin")
                    //    {
                    //        showActionSection = true;
                    //        break;
                    //    }
                    //}

                    if (assignedTo.Contains(context.CurrentUser.Id) || userProfileManager.IsUserinGroups(context.CurrentUser.Id, users.ToList()) || userProfileManager.IsAdmin(context.CurrentUser))
                        showActionSection = true;
                }

                if (UGITUtility.IsSPItemExist(CurrentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes))
                {
                    if (userProfileManager.IsUserPresentInField(context.CurrentUser, CurrentTicket, Convert.ToString(UGITUtility.GetSPItemValue(CurrentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes)), true) || userProfileManager.IsUGITSuperAdmin(context.CurrentUser))
                        showActionSection = true;
                }
                if (showActionSection)
                {
                    actionSection.Visible = true;
                }
                else
                    viewType = -1;

                e.Row.Attributes.Add("onmouseover", String.Format("showTaskActions(this,{0});", Convert.ToString(row[DatabaseObjects.Columns.ID])));
                e.Row.Attributes.Add("onmouseout", String.Format("hideTaskActions(this ,{0});", Convert.ToString(row[DatabaseObjects.Columns.ID])));
                HyperLink lnkEditbtn = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkEdit") as HyperLink;// e.Row.FindControl("lnkEdit");
                lnkEditbtn.Attributes.Add("OnClick", string.Format("editTask({0},\"{1}\" ,\"{2}\",\"{3}\")", Convert.ToString(row[DatabaseObjects.Columns.ID]), "ModuleTaskCT", Convert.ToString(row[DatabaseObjects.Columns.Title]), viewType));
                ImageButton btMarkComplete = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "btMarkComplete") as ImageButton;// e.Row.FindControl("btMarkComplete");
                HyperLink btNewPMMTask = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "btNewPMMTask") as HyperLink;//e.Row.FindControl("btNewPMMTask");
                ImageButton lnkDelete = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkDelete") as ImageButton; //e.Row.FindControl("lnkDelete");

                // Take out Stage Exit Criteria delete button if so configured
                lnkDelete.Visible = true;
                if (objConfigurationVariableManager.GetValueAsBool(DatabaseObjects.Columns.DisableStageExitCriteriaDelete))
                    lnkDelete.Visible = false;

                HyperLink anchrTitle = spGridConstraintList.FindRowCellTemplateControl(e.VisibleIndex, null, "anchrTitle") as HyperLink;// e.Row.FindControl("anchrTitle");
                anchrTitle.Attributes.Add("OnClick", string.Format("editTask({0},\"{1}\" ,\"{2}\",\"{3}\")", Convert.ToString(row[DatabaseObjects.Columns.ID]), "ModuleTaskCT", Convert.ToString(row[DatabaseObjects.Columns.Title]), viewType));
                if (e.Row.Cells[3].Text.ToLower() == "completed")
                    btMarkComplete.Visible = false;
                if (IsLastModuleStage)
                {
                    actionSection.Visible = false;
                    ConstraintTaskUrl = string.Format("{0}&viewType=-1", ConstraintTaskUrl);
                }
                //  }
            }
        }




        #endregion

        #region Helper Functions
        /// <summary>
        /// This will bind the gridview 
        /// </summary>
        private void BindGridView()
        {
            //filter data on the basis of stage id as well
            List<ModuleStageConstraints> items = SplstConditions.Load(x => x.TicketId.Equals(TicketPublicID) && x.Deleted==false);
            LifeCycle lifeCycle = null; string name = string.Empty;
            if (items != null && items.Count > 0)
            {
                if (uGITModule != null)
                    lifeCycle = uGITModule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);

                //DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{this.ModuleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                //DataRow[] moduleStagesRow = null;
                //if (dt != null && dt.Rows.Count > 0)
                //    moduleStagesRow = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, this.ModuleName)).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.StageStep)).ToArray(); //GetTableDataManager.GetDataTable(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, this.ModuleName).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();

                //DataRow currentStageRow = null;
                
                UserProfileManager MGRuserProfile = new UserProfileManager(context);
                List<UserProfile> userProfiles = MGRuserProfile.GetUsersProfile();
                //int rowCounter = 0;
                foreach (ModuleStageConstraints row in items)
                {
                    List<string> lstMultiuserValues = new List<string>();
                    name = string.Empty;
                    UserProfile uProfile = null;
                    Role uRoles = null;
                    row.AssignedTo = row.AssignedTo ?? string.Empty;
                    string[] multiLookupValue = row.AssignedTo.ToString().Split(',');
                    for (int i = 0; i < multiLookupValue.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(multiLookupValue[i])))
                        {
                            if (i == 0)
                            {
                                uProfile= userProfiles.Where(x => x.Id == UGITUtility.ObjectToString(multiLookupValue[i])).FirstOrDefault();
                                if(uProfile != null)
                                {
                                    name = uProfile.Name;
                                }
                                else
                                {
                                    uRoles = MGRuserProfile.GetUserGroups().Where(x => x.TenantID.Equals(context.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(multiLookupValue[i])).FirstOrDefault();
                                    if( uRoles != null)
                                    {
                                        name = uRoles.Name;
                                    }
                                }
                                
                            }
                            else
                            {
                                uProfile = userProfiles.Where(x => x.Id == UGITUtility.ObjectToString(multiLookupValue[i])).FirstOrDefault();
                                if (uProfile != null)
                                {
                                    name += ", " + uProfile.Name;
                                }
                                else
                                {
                                    uRoles = MGRuserProfile.GetUserGroups().Where(x => x.TenantID.Equals(context.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(multiLookupValue[i])).FirstOrDefault();
                                    if (uRoles != null)
                                    {
                                        name += ", " + uRoles.Title;
                                    }
                                }

                                //name += ", " + userProfiles.Where(x => x.Id == UGITUtility.ObjectToString(multiLookupValue[i])).FirstOrDefault().Name;
                            }
                        }                            
                        if (string.IsNullOrEmpty(name))
                        {
                            lstMultiuserValues.Add(uHelper.GetRoleNameBasedOnId(multiLookupValue[i]));
                        }
                            
                    }
                    lstMultiuserValues.ToArray();
                    if (!string.IsNullOrEmpty(name))
                    {
                        row.AssignedTo = name;
                    }
                    else
                    {
                        row.AssignedTo = string.Join(",", lstMultiuserValues); //string.Join(",", uHelper.GetUserNameBasedOnId(Convert.ToString(row.AssignedTo)));
                    }
                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(row.CompletedBy)))
                        row.CompletedBy = userProfiles.Where(x => x.Id == UGITUtility.ObjectToString(row.CompletedBy)).FirstOrDefault().Name;
                    //currentStageRow = moduleStagesRow.FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.ModuleStep]).Equals(Convert.ToString(row.ModuleStep)));
                    if (lifeCycle != null)
                    {
                        hiddenGroupName.Value = Convert.ToString(lifeCycle.Stages.Where(x => x.StageStep == Convert.ToInt32(row.ModuleStep)).FirstOrDefault().StageTitle);
                        row.Stage= Convert.ToString(hiddenGroupName.Value);
                    }

                }

                if (lifeCycle != null && lifeCycle.Stages.Count() == UGITUtility.StringToInt(this.ModuleStepId))
                    IsLastModuleStage = true;

                //if (moduleStagesRow.Count() == Convert.ToInt32(this.ModuleStepId))
                //    IsLastModuleStage = true;

                if (items != null && items.Count > 0)
                    items = items.OrderBy(x => x.ItemOrder).ThenBy(x => x.Title).ThenBy(x => x.ModuleStep).ToList();

                //currentStageRow = moduleStagesRow.FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.ModuleStep]).Equals(Convert.ToString(UGITModuleConstraint.GetModuleStepIdFromStage(this.ModuleStageId))));
                //if (currentStageRow != null)
                //{
                //    hiddenGroupName.Value = Convert.ToString(currentStageRow[DatabaseObjects.Columns.StageTitle]);

                    //}
                spGridConstraintList.DataSource = items;
                spGridConstraintList.DataBind();
            }
            else
            {
                spGridConstraintList.DataSource = null;
                spGridConstraintList.DataBind();

            }
        }

        /// <summary>
        /// This will show or hide the action buttons for task, based on user, and Module stage. Actions not visible in Last stage
        /// </summary>
        private void SetActionButtonConfig()
        {
            if (userProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || Ticket.IsActionUser(context, CurrentTicket, HttpContext.Current.CurrentUser())) //|| Ticket.IsDataEditor(CurrentTicket, SPContext.Current.Web.CurrentUser))
                newConstraint.Visible = true;

            
            
            if (uGITModule != null)
            {
                LifeCycle lifeCycle = uGITModule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                if (lifeCycle != null && lifeCycle.Stages.Count() == UGITUtility.StringToInt(ModuleStepId))
                    newConstraint.Visible = false;
            }

            // Hide if last stage

            //DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages);
            //DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{this.ModuleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            //DataRow[] moduleStagesRow = null;
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    moduleStagesRow = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, this.ModuleName)).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.ModuleStep)).ToArray();
            //    newConstraint.Visible = true;
            //}

            //if (moduleStagesRow != null && moduleStagesRow.Count() == UGITUtility.StringToInt(ModuleStepId))
            //    newConstraint.Visible = false;
        }


        /// <summary>
        /// this will mark the task Complete.
        /// </summary>
        /// <param name="taskID"></param>
        private void MarkTaskAsComplete(int taskID)
        {
            if (taskID > 0)
            {
                UGITModuleConstraint constraint = UGITModuleConstraint.MarkStageTaskAsComplete(taskID, context);

                //completed on..
                string message = string.Empty;
                bool flag = UGITModuleConstraint.GetPendingConstraintsStatus(TicketPublicID, Convert.ToInt32(ModuleStepId), ref message, context);

                // Do not approve if AutoApproveOnStageTasks set false at stage level
                Ticket ticketObj = new Ticket(context, uHelper.getModuleIdByTicketID(context, this.TicketPublicID));
                LifeCycle lifeCycle = null;
                LifeCycleStage stage = new LifeCycleStage();
                if (ticketObj != null)
                {
                    lifeCycle = ticketObj.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                    stage = lifeCycle.Stages.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(this.ModuleStageId));
                }

                if (flag && stage != null && stage.AutoApproveOnStageTasks) //stage.AutoApproveOnStageTasks)
                {
                    UGITModuleConstraint taskConstraint = new UGITModuleConstraint();
                    string errormessage = taskConstraint.AutoApproveTicket(taskConstraint, context);
                    if (string.IsNullOrEmpty(errormessage))
                    {
                        refreshPage.Value = "true";
                        approveErrorMessage.Value = string.Empty;
                    }
                    else
                    {
                        approveErrorMessage.Value = errormessage;
                        refreshPage.Value = "false";
                    }
                }
            }
        }

        /// <summary>
        /// this will delete the item
        /// </summary>
        private void DeleteConstraint(int id)
        {
            ModuleStageConstraints constraint = null;
            if (id > 0)
            {
                constraint = SplstConditions.LoadByID(id);
                constraint.Deleted = true;
                SplstConditions.Update(constraint);
                //SplstConditions.Delete(constraint);

            }
        }

        #endregion




        protected void spGridConstraintList_RowCommand1(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            if (e.CommandArgs.CommandName == "MarkAsComplete")
            {
                int taskID = int.Parse(e.CommandArgs.CommandArgument.ToString());
                if (taskID > 0)
                {
                    MarkTaskAsComplete(int.Parse(e.CommandArgs.CommandArgument.ToString()));
                }
                BindGridView();
                //TaskCache.ReloadProjectTasks(Constants.ExitCriteria, TicketPublicID);
            }
            //else if (e.CommandName == "Delete")
            //{

            //}

        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lnkDelete = sender as ImageButton;
            int taskID = Convert.ToInt32(lnkDelete.CommandArgument);
            if (taskID > 0)
            {
                DeleteConstraint(taskID);
            }
            //BindGridView();
            var tabvalue = uGITModule.List_FormTab.Where(m => m.TabName == "Tasks").Select(m => m.TabId).FirstOrDefault();
            UGITUtility.CreateCookie(Response, "TicketSelectedTab", tabvalue.ToString());
            Response.Write("<script type='text/javascript'> window.parent.location.reload(\"" + ConstraintTaskUrl + "\")</script>");            
        }

        protected void spGridConstraintList_CustomColumnSort(object sender, DevExpress.Web.CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == "Stage")
            {
                Int32 val1 = GetStageOrder(Convert.ToString(e.Value1));
                Int32 val2 = GetStageOrder(Convert.ToString(e.Value2));
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(val1, val2);
            }
        }

        private int GetStageOrder(string value)
        {
            var record = (spGridConstraintList.DataSource as List<ModuleStageConstraints>).Where(x => x.Stage == value).FirstOrDefault();
            if (record != null)
                return UGITUtility.StringToInt(record.ModuleStep);

            return 0;
        }
    }
}

