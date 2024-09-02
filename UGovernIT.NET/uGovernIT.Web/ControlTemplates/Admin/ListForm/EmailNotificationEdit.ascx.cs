
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class EmailNotificationEdit : UserControl
    {
        //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
        public string module = string.Empty;
        public int EmailNotificationID { get; set; }
        ModuleTaskEmail TaskEmail = null;
        List<UGITModule> spModuleList;
        HtmlEditorControl htmlBody;
        TaskEmailViewManager taskEmailViewManager = new TaskEmailViewManager(HttpContext.Current.GetManagerContext());
        LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        //Added by mudassir 18 march 2020
        //UGITModule ugitModule = null;
        protected override void OnInit(EventArgs e)
        {
            htmlBody = htmlEditor;
            htmlBody.EnablePickToken = true;
            BindModuleName();
            ppeSPGroup.Visible = chkUserSPGroup.Checked;
            BindEmailUserType(ddlModule.SelectedValue);
            BindModuleStep(ddlModule.SelectedValue);
            TaskEmail = taskEmailViewManager.LoadByID(EmailNotificationID); //SPListHelper.GetSPListItem(DatabaseObjects.Tables.TaskEmails, EmailNotificationID);
            if (TaskEmail != null)
            {
                LnkbtnDelete.Visible = true;
                //txtTitle.Text = TaskEmail.Title]);
                txtStatus.Text = TaskEmail.Status;
                htmlBody.Html = TaskEmail.EmailBody;
                txtEmailTitle.Text = TaskEmail.EmailTitle;
                hdnEmailUserTypes.Value = TaskEmail.EmailUserTypes;
                txtEmailToCC.Text = TaskEmail.EmailIDCC;
                txtCustomProperties.Text = TaskEmail.CustomProperties;
                chkSendEvenIfStageSkipped.Checked = UGITUtility.StringToBoolean(TaskEmail.SendEvenIfStageSkipped);
                chkShowTicketFooter.Checked = !UGITUtility.StringToBoolean(TaskEmail.HideFooter);
                ddlModule.SelectedValue = TaskEmail.ModuleNameLookup;
                BindEmailUserType(ddlModule.SelectedValue);
                BindModuleStep(ddlModule.SelectedValue);
                ddlModuleStep.SelectedValue = Convert.ToString(TaskEmail.StageStep);
                //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(ddlModule.SelectedValue));
                //Added by mudassir 19 march 2020
                chkAllowPlainTxt.Checked = UGITUtility.StringToBoolean(TaskEmail.NotifyInPlainText);
                //Added by mudassir 23 march 2020
                ddlEmailEventType.SelectedIndex = ddlEmailEventType.Items.IndexOf(ddlModule.Items.FindByValue(ddlEmailEventType.SelectedValue)); //ddlEmailEventType.Items.IndexOf(ddlEmailEventType.Items.FindByValue(Convert.ToString(TaskEmail[DatabaseObjects.Columns.EmailEventType])));
                SelectEmailUserTypesValue();
            }
            else
            {
                LnkbtnDelete.Visible = false;
            }
            //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
            //UGITModule ugitModule = moduleManager.LoadByName(ddlModule.SelectedValue);
            //
            BindModulePriority();
            //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
            if (TaskEmail != null)
                ddlPriority.SelectedValue = Convert.ToString(TaskEmail.TicketPriorityLookup);
            //
            //Added by mudassir 23 march 2020
            BindEmailEventType();
            if (TaskEmail != null)
                ddlEmailEventType.SelectedValue = Convert.ToString(TaskEmail.EmailEventType);
            //
            htmlBody.ModuleName = ddlModule.SelectedValue;
            htmlBody.ModuleStage = ddlModuleStep.SelectedValue;
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            showHideDropdowns();
            base.OnLoad(e);
        }
        //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
        protected void BindModulePriority()
        {
            ddlPriority.Items.Clear();
            ListItem item = new ListItem("-- All Priorities --", "0");
            ddlPriority.Items.Insert(0, item);
            UGITModule ugitModule = moduleManager.LoadByName(ddlModule.SelectedValue);
            if (ugitModule != null)
            {
                List<ModulePrioirty> mPriority = ugitModule.List_Priorities;
                if (mPriority != null || mPriority.Count > 0)
                {
                    mPriority.ForEach(x =>
                    {
                        ddlPriority.Items.Add(new ListItem(x.Title, Convert.ToString(x.ID)));
                    });
                }
            }
        }
        //
        //Added by mudassir 23 march 2020
        protected void ddlEmailEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show hide module step and priority dropdowns based on event type
            showHideDropdowns();
        }
        /// <summary>
        /// This method is used to Show/Hide Module Step and Priority drop-downs based on selected Event Type
        /// </summary>
        private void showHideDropdowns()
        {
            // Module Step does not apply for Created, Elevated & OnHold events
            trModuleStep.Visible = !(ddlEmailEventType.SelectedValue == Convert.ToString(TicketActionType.Created) ||
                                     ddlEmailEventType.SelectedValue == Convert.ToString(TicketActionType.Elevated) ||
                                     ddlEmailEventType.SelectedValue == Convert.ToString(TicketActionType.OnHold));

            // Priority does not apply for Elevated event
            trPriority.Visible = ddlEmailEventType.SelectedValue != Convert.ToString(TicketActionType.Elevated);
        }
        private void BindEmailUserType(string selectModule)
        {
            chklstEmailUserTypes.Items.Clear();
            ModuleUserTypeManager userTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            List<ModuleUserType> dtUserEmail = userTypeManager.Load(x => x.ModuleNameLookup.Equals(selectModule, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(x => x.UserTypes).ToList();
            if (dtUserEmail.Count > 0)
            {
                chklstEmailUserTypes.DataSource = dtUserEmail;
                chklstEmailUserTypes.DataValueField = DatabaseObjects.Columns.ColumnName;
                chklstEmailUserTypes.DataTextField = DatabaseObjects.Columns.UserTypes;
                chklstEmailUserTypes.DataBind();
            }
        }
        private void SelectEmailUserTypesValue()
        {
            string[] emailUsertype = UGITUtility.SplitString(hdnEmailUserTypes.Value, Constants.Separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in emailUsertype)
            {
                int index = chklstEmailUserTypes.Items.IndexOf(chklstEmailUserTypes.Items.FindByValue(item));
                if (index >= 0)
                {
                    chklstEmailUserTypes.Items[index].Selected = true;
                }
                else
                {
                    chkUserSPGroup.Checked = true;
                    //txtUserSPGroup.Text = item;
                    ArrayList entityArrayList = new ArrayList();
                    //PickerEntity entity = new PickerEntity();
                    //entity.Key = item;
                    //entity = ppeSPGroup.ValidateEntity(entity);
                    //entityArrayList.Add(entity);
                    //ppeSPGroup.UpdateEntities(entityArrayList);
                    ppeSPGroup.SetText(item);
                }
            }
            ppeSPGroup.Visible = chkUserSPGroup.Checked;
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModuleStep(ddlModule.SelectedValue);
            BindEmailUserType(ddlModule.SelectedValue);
            //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
            module = ddlModule.SelectedValue;
            BindModulePriority();
        }
        void BindModuleStep(string selectedModule)
        {
            ddlModuleStep.ClearSelection();
            ddlModuleStep.Items.Clear();
            List<LifeCycle> spListModuleStep = lifeCycleHelper.LoadLifeCycleByModule(selectedModule);
            List<LifeCycleStage> rows = spListModuleStep[0].Stages.Where(x => x.ModuleNameLookup.Equals(selectedModule, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(x => x.StageStep).ToList();
            rows.Select(x => x.ID);
            if (rows != null && rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    string title = $"{row.StageStep}" + " - " + $"{row.StageTitle}";
                    // ddlModuleStep.Items.Add(new ListItem(title, Convert.ToString(row.ID)));
                    ddlModuleStep.Items.Add(new ListItem(title, Convert.ToString(row.ID)));
                }
            }
            ddlModuleStep.Items.Insert(0, new ListItem("(None)", "0"));
            ddlModuleStep.SelectedIndex = 0;

        }
        private void BindModuleName()
        {
            spModuleList = moduleManager.Load();
            ddlModule.Items.Clear();
            if (spModuleList.Count > 0)
            {
                var dtModule = spModuleList.Select(x => new { x.ModuleName, x.ID, x.Title, x.EnableModule }).Where(x => x.EnableModule == true).OrderBy(x => x.ModuleName).ToList();
                foreach (var moduleRow in dtModule)
                {
                    List<LifeCycle> lifecyclestages = lifeCycleHelper.LoadLifeCycleByModule(moduleRow.ModuleName);
                    if (lifecyclestages != null && lifecyclestages.Count > 0 && lifecyclestages[0].Stages.Count > 1)
                    {
                        string moduleTitle = Convert.ToString(moduleRow.Title);
                        if (string.IsNullOrEmpty(moduleTitle))
                            moduleTitle = moduleRow.ModuleName;
                        ddlModule.Items.Add(new ListItem { Text = moduleTitle, Value = moduleRow.ModuleName });
                    }
                }
            }
            ddlModule.DataBind();
            //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
            ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(ddlModule.SelectedValue));
            //
        }
        private void Save()
        {
            if (EmailNotificationID == 0)
            {
                TaskEmail = new ModuleTaskEmail();
            }
            TaskEmail.Status = txtStatus.Text.Trim();
            TaskEmail.EmailBody = htmlBody.Html;
            TaskEmail.EmailTitle = txtEmailTitle.Text.Trim();
            TaskEmail.EmailIDCC = txtEmailToCC.Text.Trim();
            TaskEmail.CustomProperties = txtCustomProperties.Text.Trim();
            TaskEmail.SendEvenIfStageSkipped = chkSendEvenIfStageSkipped.Checked;
            List<UGITModule> moduleList = spModuleList.Where(x => x.ModuleName.Equals(ddlModule.SelectedValue)).ToList();
            if (moduleList.Count > 0)
            {
                TaskEmail.ModuleNameLookup = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), Convert.ToInt32(moduleList[0].ID));
            }
            TaskEmail.Title = TaskEmail.ModuleNameLookup + " - " + txtStatus.Text.Trim();
            if (ddlModuleStep.SelectedIndex > 0)
            {
                TaskEmail.StageStep = Convert.ToInt32(ddlModuleStep.SelectedValue);
            }
            else
            {
                TaskEmail.StageStep = null;
            }

            TaskEmail.HideFooter = !chkShowTicketFooter.Checked;
            //Added by mudassir  19 march 2020 SPDelta 206(Email notification - Check box to control Email body Plain Text/html)
            TaskEmail.NotifyInPlainText = chkAllowPlainTxt.Checked;
            //
            //Added by mudassir 18 march 2020 SPDelta 6(Priority-based & plain-text notifications: You can now configure email notifications from Admin > Email Notifications to go out for ticket of specific priorities. If priority field left blank, the notification will apply to tickets of all priorities as before. You can also specify a notification should be sent as plain-text which will strim any HTML before sending.)
            TaskEmail.TicketPriorityLookup = Convert.ToInt32(ddlPriority.SelectedValue);
            //Added by mudassir 23 march 2020
            TaskEmail.EmailEventType = ddlEmailEventType.SelectedValue;
            //
            // Get selected user types
            if (GetUserTypes())
            {
                TaskEmail.EmailUserTypes = hdnEmailUserTypes.Value;
                //TaskEmail.Update();
                if (TaskEmail.ID > 0)
                    taskEmailViewManager.Update(TaskEmail);
                else
                    taskEmailViewManager.Insert(TaskEmail);

                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated Email Notification: {TaskEmail.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        //Added 23 march 2020
        protected void BindEmailEventType()
        {

           var allActionType = Enum.GetValues(typeof(TicketActionType)).Cast<TicketActionType>().ToArray();
            var sortTicketActionType = allActionType.OrderBy(l => l.ToString()).ToList();
            foreach(var i in sortTicketActionType)
            {
                if (i.ToString() != "None")
                    ddlEmailEventType.Items.Add(Enum.GetName(typeof(TicketActionType), i));
            }
            
            //foreach (int i in Enum.GetValues(typeof(TicketActionType)))
            //{
            //    ddlEmailEventType.Items.Add(Enum.GetName(typeof(TicketActionType), i));
            //}

            ddlEmailEventType.Items.Cast<ListItem>().Where(x => x.Text == Convert.ToString(TicketActionType.Approved)).ToList().OrderBy(p=>p.Text).ToList().ForEach(y =>
            {
                y.Text = Constants.ApprovedDefault;
                y.Value = Convert.ToString(TicketActionType.Approved);
            });
        }
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(htmlBody.Html))
            {
                if (!lblEmailBodyErrorMessae.Visible)
                {
                    lblEmailBodyErrorMessae.Visible = true;
                }
                lblEmailBodyErrorMessae.Text = "Email Body is Required";
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('');", true);
                return;
            }
            else
            {
                lblEmailBodyErrorMessae.Text = "";
                lblEmailBodyErrorMessae.Visible = false;
            }
            if (!Page.IsValid)
                return;
               
            Save();
        }
        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (TaskEmail != null)
            {
                //TaskEmail.Delete();
                TaskEmail.Deleted = true;
                taskEmailViewManager.Update(TaskEmail);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Email Notification: {TaskEmail.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void chkUserSPGroup_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.ID == "chkUserSPGroup")
            {
                ppeSPGroup.Visible = checkBox.Checked;
            }
        }
        bool GetUserTypes()
        {
            hdnEmailUserTypes.Value = string.Empty;
            List<string> userValues = new List<string>();
            if (chklstEmailUserTypes.SelectedIndex > -1)
            {
                foreach (ListItem item in chklstEmailUserTypes.Items)
                {
                    if (item.Selected)
                        userValues.Add(item.Value);
                }
            }
            if (chkUserSPGroup.Checked)
            {
                var usrVals = ppeSPGroup.GetTextsAsList();

                if (usrVals != null && usrVals.Count > 0)
                {
                    userValues.Add(usrVals[0]);
                }
                else
                {
                    return false;
                }
            }
            hdnEmailUserTypes.Value = String.Join(Constants.Separator, userValues.ToArray());
            return true;
        }
    }
}
