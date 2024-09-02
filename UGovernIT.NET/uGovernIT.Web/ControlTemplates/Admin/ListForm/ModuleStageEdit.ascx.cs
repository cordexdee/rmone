using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Data;
using DevExpress.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using System.Web;
using uGovernIT.Manager.Core;
using System.IO;
using System.Configuration;

namespace uGovernIT.Web
{
    public partial class ModuleStageEdit : UserControl
    {
        public int StageID { get; set; }
        public string Module { get; set; }
        public string LifeCycleName { get; set; }
        private string TenantID = string.Empty;
        LifeCycleManager lcHelper;
        LifeCycle projectLifeCycle;
        LifeCycleStage lifeCycleStage;
        LifeCycleStageManager objLifeCycleStageManager;
        ModuleViewManager objModuleManager;
        public string SkipConditionUrl { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UGITModule ugitModule;
        private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string newParam = "listpicker";
        private string formTitle = "Help Card Picker";
        private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";

        protected override void OnInit(EventArgs e)
        {
            objModuleManager = new ModuleViewManager(context);
            objLifeCycleStageManager = new LifeCycleStageManager(context);
            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, formTitle, "HLP", string.Empty, "HelpCardList", txtHelpCard.ClientID)); //"TicketWiki"
            aAddHelpCard.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, "wiki list picker", "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            TenantID = Convert.ToString(Session["TenantID"]);
            if (Request["Module"] != null)
            {
                Module = Uri.UnescapeDataString(Request["Module"]);
                ugitModule = objModuleManager.GetByName(Module);
            }
            if (Request["LifeCycle"] != null)
            {
                LifeCycleName = Uri.UnescapeDataString(Request["LifeCycle"]);
            }

            lcHelper = new LifeCycleManager(context);
            if(!string.IsNullOrEmpty(LifeCycleName))
                projectLifeCycle = lcHelper.LoadLifeCycleByModule(Module).Where(m => m.Name == LifeCycleName).FirstOrDefault();
            else
                projectLifeCycle = lcHelper.LoadLifeCycleByModule(Module).Where(m => m.ID == 0).FirstOrDefault();

            if (Request["stageid"] != null)
            {
                int stageID = 0;
                int.TryParse(Request["stageid"], out stageID);
                StageID = stageID;
            }

            if (projectLifeCycle != null)
            {
                lifeCycleStage = projectLifeCycle.Stages.FirstOrDefault(x => x.ID == StageID);
                if (lifeCycleStage != null)                
                    lifeCycleStage.SelectedTab = lifeCycleStage.SelectedTabNumber;
            }
			if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0)
            {
                lcsgraphics.Controls.Clear();
                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/ControlTemplates/Shared/LifeCycleGUI.ascx");
                ctr2.ModuleLifeCycle = projectLifeCycle;
                if (lifeCycleStage != null)
                    ctr2.CurrentStep = lifeCycleStage.StageStep;
                lcsgraphics.Controls.Add(ctr2);
            }
			
            ddlStageType.SetText("None");
            FillApproveStage();
            FillRejectStage();
            FillReturnStage();
            BindSelectedTabDropdown();
            BindTargetTypeCategories();

            DataTable actionUsers = GetActionUser();
            glActionUser.DataSource = actionUsers;
            glActionUser.DataBind();
            glActionUser.GridView.Width = glActionUser.Width;
            
            glDataEditors.DataSource = actionUsers;
            glDataEditors.DataBind();
            glDataEditors.GridView.Width = glDataEditors.Width;
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        DataTable GetActionUser()
        {
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("NameRole");
            dtGroups.Columns.Add("Role");
            dtGroups.Columns.Add("Type");

            ModuleUserTypeManager userTypeManager = new ModuleUserTypeManager(context);
            List<ModuleUserType> rows = userTypeManager.Load(x => x.ModuleNameLookup == Module).OrderBy(x => x.ColumnName).ToList();
            if (rows != null && rows.Count() > 0)
            {
                foreach (var uType in rows)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["ID"] = uType.ID;
                    dr["Name"] = uType.ColumnName;
                    dr["Role"] = uType.UserTypes;
                    dr["NameRole"] =  string.Format("{0} ({1})", uType.UserTypes, uType.ColumnName);
                    dr["Type"] = "Role";
                    dtGroups.Rows.Add(dr);
                }
            }

            UserRoleManager roleManager = new UserRoleManager(context);
            List<Role> sRoles = roleManager.GetRoleList().OrderBy(x => x.Name).ToList();
            foreach (Role oGroup in sRoles)
            {
                DataRow dr = dtGroups.NewRow();
                dr["ID"] = oGroup.Id;
                dr["Name"] = oGroup.Name;
                dr["Role"] = oGroup.Title;
                dr["NameRole"] = oGroup.Title;
                dr["Type"] = "Group";
                dtGroups.Rows.Add(dr);
            }

            return dtGroups;
        }

        private void FillData()
        {
            if (lifeCycleStage != null)
            {
                txtTitle.Text = lifeCycleStage.Name;
                txtStep.Text = Convert.ToString(lifeCycleStage.StageStep);
                txtWeight.Text = lifeCycleStage.StageWeight.ToString("0");
                txtUserPrompt.Text = Convert.ToString(lifeCycleStage.UserPrompt);
                txtAction.Text = lifeCycleStage.Action;
                if (!string.IsNullOrEmpty(lifeCycleStage.IconUrl))
                {
                    UGITFileUploadManager1.SetImageUrl(lifeCycleStage.IconUrl);
                }

                ddlTargetType.SelectedIndex = ddlTargetType.Items.IndexOf(ddlTargetType.Items.FindByValue(Convert.ToString(lifeCycleStage.NavigationType)));
                SetTargetTypeDependency();
                if (ddlTargetType.SelectedValue == "Wiki")
                {
                    txtWiki.Text = Convert.ToString(lifeCycleStage.NavigationUrl);
                }
                if (ddlTargetType.SelectedValue == "Link")
                {
                    txtFileLink.Text = Convert.ToString(lifeCycleStage.NavigationUrl);
                }
                if (ddlTargetType.SelectedValue == "File")
                {
                    var attachments = lifeCycleStage.Attachments;

                    string fileName = Path.GetFileName(Convert.ToString(lifeCycleStage.NavigationUrl));

                    lblUploadedFile.Text = fileName;

                }
                if (ddlTargetType.SelectedValue == "HelpCard")
                {
                    txtHelpCard.Text = Convert.ToString(lifeCycleStage.NavigationUrl);
                }

                DataTable dataTable = (DataTable)glActionUser.DataSource;
                string[] users = null;
                if (lifeCycleStage.ActionUser!=null && lifeCycleStage.ActionUser.IndexOf(Constants.Separator10) != -1)
                    lifeCycleStage.ActionUser = lifeCycleStage.ActionUser.Replace(Constants.Separator10, Constants.Separator);

                users= UGITUtility.SplitString(lifeCycleStage.ActionUser, Constants.Separator);
                List<string> actionUsers = new List<string>();
                foreach (string au in users)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Name") == au.Trim());
                    string groupStr = au;
                    if (row == null)
                    {
                       DataRow groupRow= dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == au.Trim());
                        if (groupRow != null)
                            groupStr = Convert.ToString(groupRow["Role"]);
                    }
                    if (row != null)
                    {
                        actionUsers.Add(Convert.ToString(row["Role"]));
                    }
                    else
                    {
                        actionUsers.Add(groupStr);
                    }
                }

                //glActionUser.Text = string.Join(Constants.Separator, actionUsers.ToArray());
                glActionUser.Text = string.Join(Constants.UserInfoSeparator, actionUsers.ToArray());

                dataTable = (DataTable)glDataEditors.DataSource;
                string[] dataEditorUsers = null;
                if (lifeCycleStage.DataEditors!=null && lifeCycleStage.DataEditors.IndexOf(Constants.Separator10) != -1)
                    lifeCycleStage.DataEditors = lifeCycleStage.DataEditors.Replace(Constants.Separator10, Constants.Separator);
                dataEditorUsers = UGITUtility.SplitString(lifeCycleStage.DataEditors, Constants.Separator);
                List<string> dataEditors = new List<string>();
                foreach (string au in dataEditorUsers)
                {
                    DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Name") == au.Trim());
                    string groupStr = au;
                    if (row == null)
                    {
                        DataRow groupRow = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == au.Trim());
                        if (groupRow != null)
                            groupStr = Convert.ToString(groupRow["Role"]);
                    }
                    if (row != null)
                        dataEditors.Add(Convert.ToString(row["Role"]));
                    else
                        dataEditors.Add(groupStr);
                }
                //glDataEditors.Text = string.Join(Constants.Separator, dataEditors.ToArray());
                glDataEditors.Text = string.Join(Constants.UserInfoSeparator, dataEditors.ToArray());

                chkAllApporvalRequired.Checked = UGITUtility.StringToBoolean(lifeCycleStage.StageAllApprovalsRequired);
                txtApproveActionDesc.Text = lifeCycleStage.ApproveActionDescription;
                txtApproveBtnName.Text = lifeCycleStage.StageApproveButtonName;
                txtRejectActionDesc.Text = lifeCycleStage.RejectActionDescription;
                txtRejectBtnName.Text = lifeCycleStage.StageRejectedButtonName;
                txtReturnActionDesc.Text = lifeCycleStage.ReturnActionDescription;
                txtReturnBtnName.Text = lifeCycleStage.StageReturnButtonName;
                txtShortStageTitle.Text = lifeCycleStage.ShortStageTitle;
                txtUserWorkflowStatus.Text = lifeCycleStage.UserWorkflowStatus;
                txtCustomProperties.Text = lifeCycleStage.CustomProperties;
                //new lines.
                txtApprovedIcon.Text = lifeCycleStage.ApproveIcon;
                txtReturnIcon.Text = lifeCycleStage.ReturnIcon;
                txtRejectIcon.Text = lifeCycleStage.RejectIcon;
                txtCapacityNormal.Text = lifeCycleStage.StageCapacityNormal.ToString("0");
                txtCapacityMax.Text = lifeCycleStage.StageCapacityMax.ToString("0");

                string strskiponcondition = FormulaBuilder.GetSkipConditionExpression(Convert.ToString(lifeCycleStage.SkipOnCondition), Module, true);

                lblSkipOnCondition.Text = strskiponcondition;
                hdnSkipOnCondition.Set("SkipCondition", strskiponcondition);
            
                ddlStageType.SetText(lifeCycleStage.StageTypeChoice);

                ddlApprovedStatus.SelectedValue = Convert.ToString(lifeCycleStage.StageApprovedStatus);

                ddlReturnStatus.SelectedValue = Convert.ToString(lifeCycleStage.StageReturnStatus);

                ddlRejectedStatus.SelectedValue = Convert.ToString(lifeCycleStage.StageRejectedStatus);

                chkReturnAnyStage.Checked = lifeCycleStage.EnableCustomReturn;
                ddlSelectedTabs.SelectedIndex = ddlSelectedTabs.Items.IndexOf(ddlSelectedTabs.Items.FindByText(Convert.ToString(lifeCycleStage.SelectedTab)));
                txtApproveActionTooltip.Text = lifeCycleStage.ApproveButtonTooltip;
                txtReturnActionTooltip.Text = lifeCycleStage.ReturnButtonTooltip;
                txtRejectActionTooltip.Text = lifeCycleStage.ReturnButtonTooltip;
                chkDisableAutoApprove.Checked =Convert.ToBoolean(lifeCycleStage.DisableAutoApprove);
                chkAllowReassignFromList.Checked = lifeCycleStage.AllowReassignFromList;

                chkAutoApproveOnStageTasks.Checked = UGITUtility.StringToBoolean(lifeCycleStage.AutoApproveOnStageTasks);
            }

          
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                    // Only show stage-level "Disable Auto-Approve" checkbox if Auto-Approve is enabled at the module level
                    if (ugitModule != null && ugitModule.ModuleAutoApprove)
                        trdisablelbl.Visible = trdisablechk.Visible = true;
                    else
                        trdisablelbl.Visible = trdisablechk.Visible = false;
                    FillData();    
            }
            SkipConditionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagerule&moduleName={0}&controlId={1}&stageID={2}", Module, lblSkipOnCondition.ClientID, StageID));
            if (lifeCycleStage == null)
            {
                LnkbtnDelete.Visible = false;
            }
        }

        private void FillApproveStage()
        {
            if (projectLifeCycle != null)
            {
                foreach (LifeCycleStage lcs in projectLifeCycle.Stages)
                {
                    if (lifeCycleStage == null)
                    {
                        ddlApprovedStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.StageTitle), lcs.StageStep.ToString()));

                    }
                    else
                    {
                        if (lcs.StageStep > lifeCycleStage.StageStep)
                        {
                            ddlApprovedStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.StageTitle), lcs.StageStep.ToString()));
                        }
                    }

                }
            }

           ddlApprovedStatus.Items.Insert(0, "None");
        }

        private void FillReturnStage()
        {
            if (projectLifeCycle != null)
            {
                foreach (LifeCycleStage lcs in projectLifeCycle.Stages)
                {
                    if (lifeCycleStage == null)
                    {
                        ddlReturnStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.StageTitle), lcs.StageStep.ToString()));
                    }
                    else
                    {
                        if (lcs.StageStep < lifeCycleStage.StageStep)
                        {
                            ddlReturnStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.StageTitle), lcs.StageStep.ToString()));
                        }
                    }
                }
            }

            ddlReturnStatus.Items.Insert(0, "None");
        }

        private void FillRejectStage()
        {
            if (projectLifeCycle != null)
            {
                foreach (LifeCycleStage lcs in projectLifeCycle.Stages)
                {
                    if (lifeCycleStage == null)
                    {
                        ddlRejectedStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.Name), lcs.StageStep.ToString()));
                    }
                    else
                    {
                        if (lcs.StageStep != lifeCycleStage.StageStep)
                        {
                            ddlRejectedStatus.Items.Add(new ListItem(string.Format("{0} - {1}", lcs.StageStep, lcs.Name), lcs.StageStep.ToString()));
                        }
                    }
                }
            }

            ddlRejectedStatus.Items.Insert(0, "None");
        }

        protected void btSaveLifeCycleStage_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            DataRow item = null;
            if (lifeCycleStage == null)
            {
                lifeCycleStage = new LifeCycleStage();
                item = UGITUtility.ObjectToData(lifeCycleStage).Select()[0];
            }
            else
            {
                item = UGITUtility.ObjectToData(lifeCycleStage).Rows[0];
            }
            if (string.IsNullOrEmpty(LifeCycleName))
                lifeCycleStage.LifeCycleName = null;
            else
                lifeCycleStage.LifeCycleName = lcHelper.LoadLifeCycleByModule(Module).Where(x => x.Name == LifeCycleName).First().ID;

            lifeCycleStage.Name = txtTitle.Text.Trim();
            lifeCycleStage.StageTitle = txtTitle.Text.Trim();
            double weight = 0;
            double.TryParse(txtWeight.Text.Trim(), out weight);
            lifeCycleStage.StageWeight = Convert.ToInt32( Math.Round(weight, 2));

            int step = 0;
            int.TryParse(txtStep.Text.Trim(), out step);
            lifeCycleStage.StageStep = step;

            lifeCycleStage.UserPrompt = Server.HtmlDecode(txtUserPrompt.Text);
            lifeCycleStage.DisableAutoApprove = chkDisableAutoApprove.Checked;
            DataTable dataTable = (DataTable)glActionUser.DataSource;
            List<string> sActionUrs = new List<string>();
            List<string> selectedActionUrs = Convert.ToString(glActionUser.Text).Split(new string[] { Constants.UserInfoSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string sau in selectedActionUrs)
            {
                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Role") == sau);
                string groupStr = sau;
                if (row == null)
                {
                    DataRow groupRow = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == sau.Trim());
                    if (groupRow != null)
                        groupStr = Convert.ToString(groupRow["Role"]);
                }
                if (row != null)
                {
                    sActionUrs.Add(Convert.ToString(row["Name"]));
                }
                else
                {
                    sActionUrs.Add(sau);
                }
            }

            lifeCycleStage.ActionUser = string.Join<string>(Constants.Separator, sActionUrs);

            if (!string.IsNullOrEmpty(lifeCycleStage.ActionUser))            
                lifeCycleStage.ActionUser = lifeCycleStage.ActionUser.Replace(Constants.UserInfoSeparator, Constants.Separator);

            sActionUrs = new List<string>();
            dataTable = (DataTable)glDataEditors.DataSource;
            selectedActionUrs = Convert.ToString(glDataEditors.Text).Split(new string[] { Constants.UserInfoSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string sau in selectedActionUrs)
            {
                DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Role") == sau);
                string groupStr = sau;
                if (row == null)
                {
                    DataRow groupRow = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == sau.Trim());
                    if (groupRow != null)
                        groupStr = Convert.ToString(groupRow["Role"]);
                }
                if (row != null)
                    sActionUrs.Add(Convert.ToString(row["Name"]));
                else
                    sActionUrs.Add(sau);
            }

            lifeCycleStage.DataEditors = string.Join<string>(Constants.Separator, sActionUrs);

            if (!string.IsNullOrEmpty(lifeCycleStage.DataEditors))
                lifeCycleStage.DataEditors = lifeCycleStage.DataEditors.Replace(Constants.UserInfoSeparator, Constants.Separator);

            lifeCycleStage.Action = txtAction.Text;
            lifeCycleStage.StageAllApprovalsRequired = chkAllApporvalRequired.Checked;
            lifeCycleStage.ApproveActionDescription = txtApproveActionDesc.Text;
            lifeCycleStage.StageApproveButtonName = txtApproveBtnName.Text;
            lifeCycleStage.RejectActionDescription = txtRejectActionDesc.Text;
            lifeCycleStage.StageRejectedButtonName = txtRejectBtnName.Text;
            lifeCycleStage.ReturnActionDescription = txtReturnActionDesc.Text;
            lifeCycleStage.StageReturnButtonName = txtReturnBtnName.Text;
            lifeCycleStage.ShortStageTitle = txtShortStageTitle.Text;
            lifeCycleStage.UserWorkflowStatus = txtUserWorkflowStatus.Text;
            lifeCycleStage.CustomProperties = txtCustomProperties.Text;
            //new lines
            lifeCycleStage.ApproveIcon = txtApprovedIcon.Text;
            lifeCycleStage.ReturnIcon = txtReturnIcon.Text;
            lifeCycleStage.RejectIcon = txtRejectIcon.Text;

            step = 0;
            int.TryParse(txtCapacityNormal.Text.Trim(), out step);
            lifeCycleStage.StageCapacityNormal = step;

            step = 0;
            int.TryParse(txtCapacityMax.Text.Trim(), out step);
            lifeCycleStage.StageCapacityMax = step;

            lifeCycleStage.ApproveActionTooltip = txtApproveActionTooltip.Text.Trim();
            lifeCycleStage.RejectActionTooltip = txtRejectActionTooltip.Text.Trim();
            lifeCycleStage.ReturnActionToolip = txtReturnActionTooltip.Text.Trim();

            lifeCycleStage.ApproveButtonTooltip = txtApproveActionTooltip.Text.Trim();
            lifeCycleStage.RejectButtonTooltip = txtRejectActionTooltip.Text.Trim();
            lifeCycleStage.ReturnButtonTooltip = txtReturnActionTooltip.Text.Trim();

            int selectedTab = 0;
            if (ddlSelectedTabs.Items.Count > 0)
                int.TryParse(ddlSelectedTabs.SelectedValue, out selectedTab);
            lifeCycleStage.SelectedTab = selectedTab;
            lifeCycleStage.SelectedTabNumber = selectedTab;

            lifeCycleStage.SkipOnCondition = FormulaBuilder.GetSkipConditionExpression(hdnSkipOnCondition.Contains("SkipCondition") ? Convert.ToString(hdnSkipOnCondition.Get("SkipCondition")) : null, Module, false);

            // Baseline value, only for PMM.
            if (Module.ToLower() == "pmm")
            {
                lifeCycleStage.Prop_BaseLine = true;
            }
            lifeCycleStage.StageTypeChoice = ddlStageType.GetValues();

            if (ddlApprovedStatus.SelectedItem != null && ddlApprovedStatus.SelectedValue != "None")
            {
                lifeCycleStage.ApprovedStage = projectLifeCycle.Stages.Where(m => m.StageStep == Convert.ToInt16(ddlApprovedStatus.SelectedValue)).FirstOrDefault();
                lifeCycleStage.StageApprovedStatus = lifeCycleStage.ApprovedStage.StageStep;
            }
            else
            {
                lifeCycleStage.ApprovedStage = null;
            }

            if (ddlReturnStatus.SelectedItem != null && ddlReturnStatus.SelectedValue != "None")
            {
                lifeCycleStage.ReturnStage = projectLifeCycle.Stages.Where(m => m.StageStep == Convert.ToInt16(ddlReturnStatus.SelectedValue)).FirstOrDefault();
                //lifeCycleStage.StageReturnStatus = lifeCycleStage.ReturnStage.StageStep;
            }
            else
            {
                lifeCycleStage.ReturnStage = null;
            }

            lifeCycleStage.StageReturnStatus = UGITUtility.StringToInt(lifeCycleStage.ReturnStage != null ? lifeCycleStage.ReturnStage.StageStep : 0);

            if (ddlRejectedStatus.SelectedItem != null && ddlRejectedStatus.SelectedValue != "None")
            {
                lifeCycleStage.RejectStage = projectLifeCycle.Stages.Where(m => m.StageStep == Convert.ToInt16(ddlRejectedStatus.SelectedValue)).FirstOrDefault();
                lifeCycleStage.StageRejectedStatus = lifeCycleStage.RejectStage.StageStep;
            }
            else
            {
                lifeCycleStage.RejectStage = null;
                lifeCycleStage.StageRejectedStatus = 0;
            }

            lifeCycleStage.ModuleNameLookup = Module;
            lifeCycleStage.IconUrl = UGITFileUploadManager1.GetImageUrl();
            lifeCycleStage.EnableCustomReturn = chkReturnAnyStage.Checked;

            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    if (fileUploadControl.HasFile)
                    {
                        string AssetFolder = ConfigurationManager.AppSettings["AssetFolder"];
                        string finalPath = AssetFolder + "/" + lifeCycleStage.TenantID;
                        string folderPath = Server.MapPath(finalPath);

                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        //Save the File to the Directory (Folder).
                        fileUploadControl.SaveAs(folderPath + "/" + Path.GetFileName(fileUploadControl.FileName));

                        lifeCycleStage.NavigationUrl = finalPath + "/" + Path.GetFileName(fileUploadControl.FileName);
                        lifeCycleStage.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "Link":
                    lifeCycleStage.NavigationUrl = txtFileLink.Text.Trim();
                    lifeCycleStage.NavigationType = ddlTargetType.SelectedValue;
                    break;
                case "Wiki":
                    {
                        if (txtWiki.Visible)
                        {
                            lifeCycleStage.NavigationUrl = txtWiki.Text.Trim();
                        }
                        lifeCycleStage.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "HelpCard":
                    {
                        if (txtHelpCard.Visible)
                        {
                            lifeCycleStage.NavigationUrl = txtHelpCard.Text.Trim();
                        }
                        lifeCycleStage.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                default:
                    break;
            }
            lifeCycleStage.AllowReassignFromList = chkAllowReassignFromList.Checked;

            lifeCycleStage.AutoApproveOnStageTasks = chkAutoApproveOnStageTasks.Checked;

            if (lifeCycleStage.ID == 0)
                objLifeCycleStageManager.Insert(lifeCycleStage);
            else
                objLifeCycleStageManager.Update(lifeCycleStage);

            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added/Updated module stage: {lifeCycleStage.StageTitle} ({Module})", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void UpdateStage(LifeCycleStage lifeCycleStage, DataRow item)
        {
            if (item != null)
            {
                item[DatabaseObjects.Columns.Title] = lifeCycleStage.Name;
                //item[DatabaseObjects.Columns.ModuleNameLookup] = uHelper.getModuleIdByModuleName(Module);
                item[DatabaseObjects.Columns.StageTitle] = lifeCycleStage.Name;
                item[DatabaseObjects.Columns.ModuleStep] = lifeCycleStage.StageStep;
                item[DatabaseObjects.Columns.StageWeight] = lifeCycleStage.StageWeight;
                item[DatabaseObjects.Columns.UserPrompt] = lifeCycleStage.UserPrompt;
                item[DatabaseObjects.Columns.Action] = lifeCycleStage.Action;
                item[DatabaseObjects.Columns.ActionUser] = lifeCycleStage.ActionUser;
                item[DatabaseObjects.Columns.StageAllApprovalsRequired] = Convert.ToInt16(lifeCycleStage.StageAllApprovalsRequired);
                item[DatabaseObjects.Columns.ApproveActionDescription] = lifeCycleStage.ApproveActionDescription;

                item[DatabaseObjects.Columns.StageApproveButtonName] = lifeCycleStage.StageApproveButtonName;
                item[DatabaseObjects.Columns.RejectActionDescription] = lifeCycleStage.RejectActionDescription;
                item[DatabaseObjects.Columns.StageRejectedButtonName] = lifeCycleStage.StageRejectedButtonName;

                item[DatabaseObjects.Columns.ReturnActionDescription] = lifeCycleStage.ReturnActionDescription;
                item[DatabaseObjects.Columns.StageReturnButtonName] = lifeCycleStage.StageReturnButtonName;
                item[DatabaseObjects.Columns.ShortStageTitle] = lifeCycleStage.ShortStageTitle;

                item[DatabaseObjects.Columns.UserWorkflowStatus] = lifeCycleStage.UserWorkflowStatus;
                item[DatabaseObjects.Columns.CustomProperties] = lifeCycleStage.CustomProperties;
                item[DatabaseObjects.Columns.SkipOnCondition] = FormulaBuilder.GetSkipConditionExpression(lifeCycleStage.SkipOnCondition, Module, false);
                item[DatabaseObjects.Columns.ShowBaselineButtons] = lifeCycleStage.Prop_BaseLine;
                item[DatabaseObjects.Columns.StageType] = ddlStageType.GetValues(); //ddlStageType.Items.FindByText(lifeCycleStage.StageType) != null ? ddlStageType.Items.FindByText(lifeCycleStage.StageType).Value : string.Empty;
                item[DatabaseObjects.Columns.StageApprovedStatus] = lifeCycleStage.ApprovedStage != null ? lifeCycleStage.ApprovedStage.ID : 0;
                item[DatabaseObjects.Columns.StageReturnStatus] = lifeCycleStage.ReturnStage != null ? lifeCycleStage.ReturnStage.ID : 0;
                item[DatabaseObjects.Columns.StageRejectedStatus] = lifeCycleStage.RejectStage != null ? lifeCycleStage.RejectStage.ID : 0;
                item[DatabaseObjects.Columns.EnableCustomReturn] = lifeCycleStage.EnableCustomReturn;

                //new lines
                item[DatabaseObjects.Columns.ApproveIcon] = lifeCycleStage.ApproveIcon;
                item[DatabaseObjects.Columns.ReturnIcon] = lifeCycleStage.ReturnIcon;
                item[DatabaseObjects.Columns.RejectIcon] = lifeCycleStage.RejectIcon;
                item[DatabaseObjects.Columns.SelectedTabNumber] = lifeCycleStage.SelectedTab;

                item[DatabaseObjects.Columns.ApproveButtonTooltip] = lifeCycleStage.ApproveActionTooltip;
                item[DatabaseObjects.Columns.RejectButtonTooltip] = lifeCycleStage.RejectActionTooltip;
                item[DatabaseObjects.Columns.ReturnButtonTooltip] = lifeCycleStage.ReturnActionToolip;
                item[DatabaseObjects.Columns.StageCapacityNormal] = lifeCycleStage.StageCapacityNormal;
                item[DatabaseObjects.Columns.StageCapacityMax] = lifeCycleStage.StageCapacityMax;

                item[DatabaseObjects.Columns.AllowReassignFromList] = lifeCycleStage.AllowReassignFromList;
                item[DatabaseObjects.Columns.DataEditors] = lifeCycleStage.DataEditors;
            }
            //DataTable rowTable = item.Table;
            //lifeCycleStage
            //lcHelper.UpdateLifeCycleStage(item);
            //item.UpdateOverwriteVersion();
        }

        protected void cvTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0 &&
                projectLifeCycle.Stages.Exists(x => (lifeCycleStage == null || x.ID != lifeCycleStage.ID) && !string.IsNullOrEmpty(x.Name) && x.Name.ToLower() == txtTitle.Text.Trim().ToLower()))
            {
                args.IsValid = false;
            }
        }

        protected void cvStep_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int stepNumber = 0;
            int.TryParse(txtStep.Text.Trim(), out stepNumber);

            if (stepNumber <= 0)
            {
                args.IsValid = false;
            }
            else if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0 && projectLifeCycle.Stages.Exists(x => x.StageStep == stepNumber && x.ID != StageID))
            {
                args.IsValid = false;
            }
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (lifeCycleStage != null)
                // objLifeCycleStageManager.Delete(lifeCycleStage);
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted module stage: {lifeCycleStage.StageTitle} ({Module})", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            objLifeCycleStageManager.DeleteLifeCycleStage(Module,lifeCycleStage.ID);

                uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void BindSelectedTabDropdown()
        {
            if (ddlSelectedTabs.Items.Count == 0)
            {
                FormLayoutManager formManager = new FormLayoutManager(context);
                //SPQuery query = new SPQuery();
                //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
                //query.ViewFieldsOnly = true;
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, Module);

                //List<ModuleFormTab> collection = formManager.GetConfigClientAdminCategoryData().Where(x=>x.ModuleNameLookup == Module).ToList(); //SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.ModuleFormTab, query);
                //int count = collection.Count();
                int count = formManager.GetConfigClientAdminCategoryData().Where(x => x.ModuleNameLookup == Module).Count();

                if (count <= 0)
                    return;                

                for (int i = 1; i <= count; i++)
                {
                    ddlSelectedTabs.Items.Add(i.ToString());
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);  // UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestageaddedit"));
        }


        private void BindTargetTypeCategories()
        {
            ddlTargetType.Items.Add(new ListItem("File", "File"));
            ddlTargetType.Items.Add(new ListItem("Link", "Link"));
            ddlTargetType.Items.Add(new ListItem("Wiki", "Wiki"));
            ddlTargetType.Items.Add(new ListItem("Help Card", "HelpCard"));
            ddlTargetType.DataBind();
        }

        protected void ddlTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTargetTypeDependency();
        }

        private void SetTargetTypeDependency()
        {
            trLink.Visible = false;
            trFileUpload.Visible = false;
            trWiki.Visible = false;
            trHelpCard.Visible = false;
            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    trFileUpload.Visible = true;
                    break;
                case "Link":
                    trLink.Visible = true;
                    break;
                case "Wiki":
                    trWiki.Visible = true;
                    break;
                case "HelpCard":
                    trHelpCard.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
}
