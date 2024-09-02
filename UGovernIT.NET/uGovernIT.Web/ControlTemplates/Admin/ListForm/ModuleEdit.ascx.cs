using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager.Managers;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ModuleEdit : UserControl
    {
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager ticketManager = null;

        public int Id { get; set; }
        private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        //DataTable dt;
        // private SPListItem _SPListItem;

        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&&ControlId={5}";

        //ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
		private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        //private string formTitle = "Help Card Picker";
        UGITModule Module;

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _moduleViewManager;
            }
        }
        protected TicketManager TicketManager
        {
            get
            {
                if (ticketManager == null)
                {
                    ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                }
                return ticketManager;
            }
        }
        
        
        protected override void OnInit(EventArgs e)
        {
            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, formTitle, "HLP", string.Empty, "HelpCardList", txtHelpCard.ClientID)); //"TicketWiki"
            aAddHelpCard.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','80','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
			BindTargetTypeCategories();
            Module = ModuleViewManager.LoadByID(Id);
            if(!IsPostBack)
                Fill();
            BindThemeColor();
            FillFieldNames();
            base.OnInit(e);
        }

        private void BindThemeColor()
        {   
            ddlThemeColor.Items.Insert(0, new ListItem("None", "0"));
        }

        private void Fill()
        {

            string value = Convert.ToString(Module.ModuleType);
            string moduletypevalue = Convert.ToString(Module.ModuleType);
            txtModuleName.Text = Convert.ToString(Module.ModuleName);
            txtModuleId.Text = Convert.ToString(Module.ID);
            txtLastSequence.Text = Convert.ToString(Module.LastSequence);
            dtcLastSequenceDate.Date = Convert.ToDateTime(Module.LastSequenceDate);
            chkAuroApprove.Checked = UGITUtility.StringToBoolean(Module.ModuleAutoApprove);
            txtHoldMaxStage.Text = Convert.ToString(Module.ModuleHoldMaxStage);
            cmbFieldName.Value=Module.AltTicketIdField;
            ddlTargetType.SelectedIndex = ddlTargetType.Items.IndexOf(ddlTargetType.Items.FindByValue(Convert.ToString(Module.NavigationType)));
            SetTargetTypeDependency();
            if (ddlTargetType.SelectedValue == "Wiki")
            {
                txtWiki.Text = Convert.ToString(Module.NavigationUrl);
            }
            if (ddlTargetType.SelectedValue == "Link")
            {
                txtFileLink.Text = Convert.ToString(Module.NavigationUrl);
            }
            if (ddlTargetType.SelectedValue == "File")
            {
                var attachments = Module.Attachments;

                string fileName = Path.GetFileName(Convert.ToString(Module.NavigationUrl));

                lblUploadedFile.Text = fileName;

            }
            if (ddlTargetType.SelectedValue == "HelpCard")
            {
                txtHelpCard.Text = Convert.ToString(Module.NavigationUrl);
            }
            //txtHelp.SetImageUrl(Convert.ToString(Module.NavigationUrl));
            txtTicketTable.Text = Convert.ToString(Module.ModuleTable);
            txtRelativePath.Text = Convert.ToString(Module.ModuleRelativePagePath);
            txtStaticModule.Text = Convert.ToString(Module.StaticModulePagePath);
            txtDescription.Text = Convert.ToString(Module.ModuleDescription);
            //txtOpentTicketChart.Text = Convert.ToString(Module.OpenTicketChart);
            //txtCloseTicketChart.Text = Convert.ToString(Module.CloseTicketChart);

            Enum moduleType = (ModuleType)Convert.ToInt32(Module.ModuleType);
            ddlmoduletype.SetValues(moduleType.ToString());
            //ddlThemeColor.SelectedValue = (Convert.ToString(Module.ThemeColor));

            ppeAuthorizedToView.SetValues(Convert.ToString(Module.AuthorizedToView));
            ppeAuthorizedToCreate.SetValues(Convert.ToString(Module.AuthorizedToCreate));
            chkAllowDraftMode.Checked = UGITUtility.StringToBoolean(Module.AllowDraftMode);
            chkEnableEventReceivers.Checked = UGITUtility.StringToBoolean(Module.EnableEventReceivers);
            chkEnableNewButtonOnHomePage.Checked = UGITUtility.StringToBoolean(Module.EnableNewsOnHomePage);
            txtItemOrder.Text = Convert.ToString(Module.ItemOrder);
            chkEnable.Checked = UGITUtility.StringToBoolean(Module.EnableModule);
            if (!string.IsNullOrEmpty(Convert.ToString(Module.KeepItemOpen))) { chkKeepItemOpen.Checked = UGITUtility.StringToBoolean(Module.KeepItemOpen); }
            chkReturnCommentOptional.Checked = UGITUtility.StringToBoolean(Module.ReturnCommentOptional);
            txtCustomProperties.Text = Convert.ToString(Module.CustomProperties);
            txtShortName.Text = Convert.ToString(Module.ShortName);
            txtCategoryName.Text = Convert.ToString(Module.CategoryName);
            chkEnableCache.Checked =  UGITUtility.StringToBoolean(Module.EnableCache);
            chkStoreTicketEmail.Checked = UGITUtility.StringToBoolean(Module.StoreEmails);
            chkUseInGlobalSearch.Checked = UGITUtility.StringToBoolean(Module.UseInGlobalSearch);
            chkAutoCreateLibrary.Checked = UGITUtility.StringToBoolean(Module.AutoCreateDocumentLibrary);
            chkEnableLayout.Checked = UGITUtility.StringToBoolean(Module.EnableLayout);
            chkEnableQuickTicket.Checked = UGITUtility.StringToBoolean(Module.EnableQuick);
            chkEnableWorkflow.Checked = UGITUtility.StringToBoolean(Module.EnableWorkflow);
            chkShowComment.Checked = UGITUtility.StringToBoolean(Module.ShowComment);
            chkShowNextSLA.Checked = UGITUtility.StringToBoolean(Module.ShowNextSLA);
            chkSyncAppsToRequestType.Checked = UGITUtility.StringToBoolean(Module.SyncAppsToRequestType);
            ddlownerbinding.SetValues(Convert.ToString(Module.OwnerBindingChoice));
            chkAllowChangeTicketType.Checked = UGITUtility.StringToBoolean(Module.AllowChangeType);
            chkAllowBatchEdit.Checked = UGITUtility.StringToBoolean(Module.AllowBatchEditing);
            chkAllowBatchCreate.Checked = UGITUtility.StringToBoolean(Module.AllowBatchCreate);
            chkAllowReassign.Checked = UGITUtility.StringToBoolean(Module.AllowReassignFromList);
            chkAllowEscalation.Checked = UGITUtility.StringToBoolean(Module.AllowEscalationFromList);
            chkShowTicketSummary.Checked = UGITUtility.StringToBoolean(Module.ShowSummary);

            chkNotifyActionUsersOnComment.Checked = UGITUtility.StringToBoolean(Module.ActionUserNotificationOnComment);
            chkNotifyRequestorOnComment.Checked = UGITUtility.StringToBoolean(Module.RequestorNotificationOnComment);
            chkNotifyInitiatorOnComment.Checked = UGITUtility.StringToBoolean(Module.InitiatorNotificationOnComment);

            chkWaitingOnMeIncludeGroups.Checked = UGITUtility.StringToBoolean(Module.WaitingOnMeIncludesGroups);
            chkWaitingOnMeExcludeResolved.Checked = UGITUtility.StringToBoolean(Module.WaitingOnMeExcludesResolved);
            chkShowBottleNeckChart.Checked = UGITUtility.StringToBoolean(Module.ShowBottleNeckChart);
            chkAllowBatchClose.Checked = UGITUtility.StringToBoolean(Module.AllowBatchClose);
            chkAllowTicketDelete.Checked = UGITUtility.StringToBoolean(Module.AllowDelete);
            chkHideWorkFlow.Checked = UGITUtility.StringToBoolean(Module.HideWorkFlow);
            chkEnableRMMAllocation.Checked = UGITUtility.StringToBoolean(Module.EnableRMMAllocation);
            chkPreloadTabs.Checked = UGITUtility.StringToBoolean(Module.PreloadAllModuleTabs);
            chkModuleAgent.Checked = UGITUtility.StringToBoolean(Module.EnableModuleAgent);
            chkEnableCloseonHoldExpiration.Checked = UGITUtility.StringToBoolean(Module.EnableCloseOnHoldExpire);
            chkShowBaseline.Checked = UGITUtility.StringToBoolean(Module.EnableBaseLine);
            chkActualHourUser.Checked = UGITUtility.StringToBoolean(Module.ActualHoursByUser);
            chkEnableAddNewButton.Checked = UGITUtility.StringToBoolean(Module.EnableAddNewButton);

            IsAllocationTypeHard_Soft.Checked = UGITUtility.StringToBoolean(Module.IsAllocationTypeHard_Soft);

            //SpDelta 42(Implementation of asset/ticket import)
            chkEnableTicketImport.Checked= UGITUtility.StringToBoolean(Module.EnableTicketImport);
            spinedit.Text = UGITUtility.ObjectToString(Module.AutoRefreshListFrequency);
            chkShowTasksInProjectTasks.Checked = UGITUtility.StringToBoolean(Module.ShowTasksInProjectTasks);
            chkEnableLinkSimilarTickets.Checked = UGITUtility.StringToBoolean(Module.EnableLinkSimilarTickets);
            chkEnableIcon.Checked = UGITUtility.StringToBoolean(Module.EnableIcon);
            if (Module.ModuleName == ModuleNames.RMM)
                dvKeepTicketCount.Visible = chkKeepTicketCount.Checked = false;
            else
                chkKeepTicketCount.Checked = UGITUtility.StringToBoolean(Module.KeepTicketCounts);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string moduleName = txtModuleName.Text.Trim();
            string shortName = txtShortName.Text.Trim();
            if (moduleName == string.Empty || shortName == string.Empty)
                return;
            
            Module.Title= shortName + " (" + moduleName + ")";
            Module.ModuleName= moduleName;
            Module.ModuleId= UGITUtility.StringToInt(txtModuleId.Text==string.Empty?null:txtModuleId.Text);
            Module.LastSequence= UGITUtility.StringToInt(txtLastSequence.Text==string.Empty?null:txtLastSequence.Text);
            Module.LastSequenceDate= DateTime.Today;
            Module.ModuleAutoApprove= chkAuroApprove.Checked;
            Module.ModuleHoldMaxStage= UGITUtility.StringToInt(txtHoldMaxStage.Text==string.Empty?"0":txtHoldMaxStage.Text);
            if (cmbFieldName.SelectedItem != null)
                Module.AltTicketIdField = cmbFieldName.SelectedItem.Text;
            else
                Module.AltTicketIdField = DatabaseObjects.Columns.TicketId;

            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    if (fileUploadControl.HasFile)
                    {
                        string AssetFolder = ConfigurationManager.AppSettings["AssetFolder"];
                        string finalPath = AssetFolder + "/" + Module.TenantID;
                        string folderPath = Server.MapPath(finalPath);

                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        //Save the File to the Directory (Folder).
                        fileUploadControl.SaveAs(folderPath + "/" + Path.GetFileName(fileUploadControl.FileName));

                        Module.NavigationUrl = finalPath + "/" + Path.GetFileName(fileUploadControl.FileName);
                        Module.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "Link":
                    Module.NavigationUrl = txtFileLink.Text.Trim();
                    Module.NavigationType = ddlTargetType.SelectedValue;
                    break;
                case "Wiki":
                    {
                        if (txtWiki.Visible)
                        {
                            Module.NavigationUrl = txtWiki.Text.Trim();
                        }
                        Module.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "HelpCard":
                    {
                        if (txtHelpCard.Visible)
                        {
                            Module.NavigationUrl = txtHelpCard.Text.Trim();
                        }
                        Module.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                default:
                    break;
            }

            //Module.NavigationUrl = txtHelp.GetImageUrl();
            Module.ModuleRelativePagePath= txtRelativePath.Text.Trim();
            Module.StaticModulePagePath= txtStaticModule.Text.Trim();
            Module.ModuleDescription= txtDescription.Text.Trim();
            Module.KeepItemOpen= chkKeepItemOpen.Checked;
            Module.ReturnCommentOptional= chkReturnCommentOptional.Checked;
            Module.EnableNewsOnHomePage = chkEnableNewButtonOnHomePage.Checked;
            //Module.ThemeColor = ddlThemeColor.SelectedValue;
            ModuleType moduleType;
            Enum.TryParse(ddlmoduletype.GetValues(), out moduleType);
           

            Module.ModuleType= moduleType;

            Module.AuthorizedToView= ppeAuthorizedToView.GetValues();
            Module.AuthorizedToCreate= ppeAuthorizedToCreate.GetValues();
            Module.AllowDraftMode= chkAllowDraftMode.Checked;
            Module.EnableEventReceivers= chkEnableEventReceivers.Checked;
            Module.ItemOrder = UGITUtility.StringToInt(txtItemOrder.Text==string.Empty?"0":txtItemOrder.Text);
            Module.EnableModule= chkEnable.Checked;
            Module.CustomProperties= txtCustomProperties.Text.Trim();
            Module.ShortName= txtShortName.Text.Trim();
            Module.StoreEmails= chkStoreTicketEmail.Checked;
            Module.EnableQuick= chkEnableQuickTicket.Checked;
            Module.AllowChangeType= chkAllowChangeTicketType.Checked;
            Module.ShowSummary= chkShowTicketSummary.Checked;
            Module.AllowDelete= chkAllowTicketDelete.Checked;
            Module.ModuleTable= txtTicketTable.Text.Trim();
            //Module.OpenTicketChart= txtOpentTicketChart.Text.Trim();
            //Module.CloseTicketChart= txtCloseTicketChart.Text.Trim();
            Module.CategoryName= txtCategoryName.Text.Trim();
            Module.EnableCache= chkEnableCache.Checked;
            Module.UseInGlobalSearch= chkUseInGlobalSearch.Checked;
            Module.AutoCreateDocumentLibrary= chkAutoCreateLibrary.Checked;
            Module.SyncAppsToRequestType= chkSyncAppsToRequestType.Checked;
            Module.ShowNextSLA= chkShowNextSLA.Checked;
            Module.ShowComment= chkShowComment.Checked;
            Module.EnableWorkflow= chkEnableWorkflow.Checked;
            Module.EnableLayout= chkEnableLayout.Checked;
            Module.OwnerBindingChoice= ddlownerbinding.GetValues();
            Module.AllowBatchEditing= chkAllowBatchEdit.Checked;
            Module.AllowBatchCreate= chkAllowBatchCreate.Checked;
            Module.AllowReassignFromList= chkAllowReassign.Checked;
            Module.AllowEscalationFromList= chkAllowEscalation.Checked;
            Module.RequestorNotificationOnComment= chkNotifyRequestorOnComment.Checked;
            Module.ActionUserNotificationOnComment= chkNotifyActionUsersOnComment.Checked;
            Module.InitiatorNotificationOnComment= chkNotifyInitiatorOnComment.Checked;
            Module.WaitingOnMeIncludesGroups = chkWaitingOnMeIncludeGroups.Checked;
            Module.WaitingOnMeExcludesResolved = chkWaitingOnMeExcludeResolved.Checked;
            Module.ShowBottleNeckChart= chkShowBottleNeckChart.Checked;
            Module.AllowBatchClose= chkAllowBatchClose.Checked;
            Module.HideWorkFlow= chkHideWorkFlow.Checked;
            Module.EnableRMMAllocation= chkEnableRMMAllocation.Checked;
            Module.PreloadAllModuleTabs= chkPreloadTabs.Checked;
            Module.EnableModuleAgent= chkModuleAgent.Checked;
            Module.EnableCloseOnHoldExpire = chkEnableCloseonHoldExpiration.Checked;
            Module.EnableBaseLine = chkShowBaseline.Checked;
            Module.AutoRefreshListFrequency= UGITUtility.StringToInt( spinedit.Value);
            Module.ShowTasksInProjectTasks = chkShowTasksInProjectTasks.Checked;
            Module.KeepTicketCounts = chkKeepTicketCount.Checked;
            Module.EnableLinkSimilarTickets = chkEnableLinkSimilarTickets.Checked;
            Module.EnableIcon = chkEnableIcon.Checked;
            Module.EnableTicketImport = chkEnableTicketImport.Checked;
            Module.ActualHoursByUser = chkActualHourUser.Checked;
            Module.EnableAddNewButton = chkEnableAddNewButton.Checked;


            Module.IsAllocationTypeHard_Soft = IsAllocationTypeHard_Soft.Checked;

            ModuleViewManager.Update(Module);
            //uHelper.ClosePopUpAndEndResponse(Context, true);
            
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated module: {Module.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            // Save changes to module
            btnSave_Click(btnSave, new EventArgs());
            Module = ModuleViewManager.LoadByID(Id);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            CacheHelper<UGITModule>.AddOrUpdate(string.Format(Module.ModuleName), context.TenantID, Module);
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
        protected void cmbFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFieldNames();
        }
        void FillFieldNames()
        {
            cmbFieldName.Items.Clear();
            List<string> fields = new List<string>();
            FieldConfigurationManager fieldColl = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            UGITModule ugitModule = _moduleViewManager.GetByID(Id);
            if (ugitModule !=null)
            {
                DataTable dt = TicketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, UGITUtility.ObjectToString(ugitModule.ModuleTable));
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("{0} in ({1}) and {2} not like {3} and {2} not like {4} and {2} not in ('AltTicketIdField','Attachments','TenantID') ", DatabaseObjects.Columns.DATA_TYPE,"'int','nvarchar','varchar'",DatabaseObjects.Columns.ColumnNameSchema,"'%User%'", "'%Lookup%'");
                dt.DefaultView.Sort = "COLUMN_NAME ASC";
                dt = dv.ToTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbFieldName.DataSource = dt;
                    cmbFieldName.TextField = DatabaseObjects.Columns.ColumnNameSchema;
                    cmbFieldName.ValueField = DatabaseObjects.Columns.ColumnNameSchema;
                    cmbFieldName.DataBind();
                }
            }
        }
    }
}
