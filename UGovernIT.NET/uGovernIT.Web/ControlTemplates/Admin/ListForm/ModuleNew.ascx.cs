using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ModuleNew : UserControl
    {
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&&ControlId={5}";
        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
		private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        UGITModule Module;
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, formTitle, "HLP", string.Empty, "HelpCardList", txtHelpCard.ClientID)); //"TicketWiki"
            aAddHelpCard.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            BindTargetTypeCategories();
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string moduleName = txtModuleName.Text.Trim();
            string shortName = txtShortName.Text.Trim();
            if (moduleName == string.Empty || shortName == string.Empty)
                return;
            Module = new UGITModule();
            Module.Title= shortName + " (" + moduleName + ")";
            Module.ModuleName= moduleName;
            Module.ModuleId= Convert.ToInt32(txtModuleId.Text);
            Module.LastSequence=0;
            Module.LastSequenceDate= DateTime.Today;
            Module.ModuleAutoApprove= chkAuroApprove.Checked;
            Module.ModuleHoldMaxStage= Convert.ToInt32(txtHoldMaxStage.Text==string.Empty?"0":txtHoldMaxStage.Text);
            //Module.NavigationUrl= UGITFileUploadManager1.GetImageUrl();

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
            
            Module.ModuleRelativePagePath= txtRelativePath.Text.Trim();
            Module.StaticModulePagePath= txtStaticModule.Text.Trim();
            Module.ModuleDescription= txtDescription.Text.Trim();
            Module.KeepItemOpen= chkKeepItemOpen.Checked;
            Module.ReturnCommentOptional= chkReturnCommentOptional.Checked;

            ModuleType moduleType;
            Enum.TryParse(ddlmoduletype.GetValues(), out moduleType);

            Module.ModuleType= moduleType;

            Module.AuthorizedToView = ppeAuthorizedToView.GetValues();
            Module.AuthorizedToCreate = ppeAuthorizedToCreate.GetValues();
            Module.AllowDraftMode = chkAllowDraftMode.Checked;
            Module.EnableEventReceivers = chkEnableEventReceivers.Checked;
            Module.ItemOrder = UGITUtility.StringToInt(txtItemOrder.Text==string.Empty?"0":txtHoldMaxStage.Text);
            Module.EnableModule= chkEnable.Checked;
            Module.CustomProperties= txtCustomProperties.Text.Trim();
            Module.ShortName= txtShortName.Text.Trim();
            Module.StoreEmails= chkStoreTicketEmail.Checked;
            Module.EnableQuick= chkEnableQuickTicket.Checked;
            Module.AllowChangeType= chkAllowChangeTicketType.Checked;
            Module.ShowSummary= chkShowTicketSummary.Checked;
            Module.AllowDelete= chkAllowTicketDelete.Checked;
            Module.ModuleTable= txtTicketTable.Text.Trim();
            //Module.OpenChart= txtOpentTicketChart.Text.Trim();
            //Module.CloseChart= txtCloseTicketChart.Text.Trim();
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
            
            Module.ShowBottleNeckChart= chkShowBottleNeckChart.Checked;
            Module.AllowBatchClose = chkAllowBatchClose.Checked;
            Module.HideWorkFlow = chkHideWorkFlow.Checked;
            Module.EnableRMMAllocation = chkEnableRMMAllocation.Checked;
            Module.PreloadAllModuleTabs = chkPreloadTabs.Checked;
            //Module.AllowActualHoursByUser= chkActualHourUser.Checked;
            Module.ShowTasksInProjectTasks = chkShowTasksInProjectTasks.Checked;
            Module.EnableLinkSimilarTickets = chkEnableLinkSimilarTickets.Checked;

            Module.IsAllocationTypeHard_Soft = IsAllocationTypeHard_Soft.Checked;

            //Added below to fix exception throwing in Admin -> Page Editor on 30-03-2018
            Module.DisableNewConfirmation = false;
            Module.EnableModuleAgent = false;
            Module.ReloadCache = false;
            Module.WaitingOnMeIncludesGroups = false;
            Module.WaitingOnMeExcludesResolved = false;
            ModuleManager.Insert(Module);

            //Util.Log.ULog.WriteLog($"Added module: {Module.Title}");
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added module: {Module.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
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
