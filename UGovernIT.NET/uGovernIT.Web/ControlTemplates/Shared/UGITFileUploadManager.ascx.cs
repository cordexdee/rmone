using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Web.Helpers;
using AutoMapper;
using System.Data;
using System.Configuration;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class UGITFileUploadManager : System.Web.UI.UserControl
    {
        
        public string Module { get; set; }
        public string ParentTicketId { get; set; }
        public string Type { get; set; }
        public DataTable FilteredTable { get; set; }
        public string selectedTicketId { get; set; }
        public string ParentModule { get; set; }
        public string ControlId { get; set; }
        public bool hideWiki { get; set; }
        public bool hideDoc { get; set; }
        public string AnchorLabel { get; set; }
        public bool MultiSelect = false;
        
       // private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&TicketId={3}&Type={4}&&ControlId={5}";
        //private string newParam = "listpicker";
        //private string formTitle = "Picker List";
        public string fileExtension { get; set; }
        protected string controlID = string.Empty;
        protected string detailsURL = "/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&";

        ASPxFileManager fileManger = null;
        public UGITFileUploadManager()
        {
            fileExtension = string.Empty;
            AnchorLabel = "Upload Document";
        }
        protected override void OnInit(System.EventArgs e)
        {
            //string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIK", string.Empty, "WikiHelp", ""));
            //aShowWiki.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            if (!string.IsNullOrEmpty(fileExtension))
            {
               // ASPxFileManager1.Settings.AllowedFileExtensions = fileExtension.Split(',');
            }

            if (hdnPopupContentInfo.Contains("controlkey"))
            {
                LoadControl();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (hideWiki)
            {
                aShowWiki.Visible = false;
                apipe.Visible = false;
               
            }
            if (hideDoc)
            {
                aUploadDocuments.Visible = false;
                apipe.Visible = false;
               
            }
            aShowWiki.HRef = "javascript:showWiki('" + this.ClientID + "')";
            aUploadDocuments.HRef = "javascript:showUploadControl('" + this.ClientID + "')";

        }
        public string GetImageUrl()
        {
            /*
            string value = string.Empty;
            value = Convert.ToString(txtHelp.Text.Trim());
            if (!value.StartsWith(@"\"))
                value = @"\" + value;
            return value;
            */
            return Convert.ToString(txtHelp.Text.Trim());
        }
        public void SetImageUrl(string value)
        {
            txtHelp.Text = value;
        }

        protected void cb_Callback(object sender, CallbackEventArgsBase e)
        {
            hdnPopupContentInfo["controlkey"] = e.Parameter;
            fileUploadManager.Controls.Clear();
            txtHelp.Text = string.Empty;
            fileUploadManager.Controls.Add(hdnPopupContentInfo);
            LoadControl();
        }
            
        

        private void LoadControl()
        {

            //string fileExtentsion=".jpg,.rtf,.txt,.avi,.png,.mp3,.xml";
            string param = Convert.ToString(hdnPopupContentInfo["controlkey"]);
            if (param == "aUploadDocuments")
            {
                string AssetFolder = ConfigurationManager.AppSettings["AssetFolder"];
                fileManger = new ASPxFileManager();
                fileManger.ID = "ASPxFileManager1";
                fileManger.ClientInstanceName = "ASPxFileManager1";
                fileManger.Settings.EnableMultiSelect = MultiSelect;
                fileManger.Height = 560;
                fileManger.Width = 480;
                fileManger.SettingsAdaptivity.Enabled = true;
                fileManger.SettingsAdaptivity.EnableCollapseFolderContainer = true;
                fileManger.SettingsFolders.Visible = false;
                if (MultiSelect)
                    fileManger.ClientSideEvents.SelectionChanged = "fileManager_SelectionChanged";
                else
                    fileManger.ClientSideEvents.SelectedFileChanged = "function(s,e){FindImageName(s,e);}";
                                
                fileManger.Settings.RootFolder = AssetFolder;
                fileManger.Settings.InitialFolder = "Assets";
                fileManger.Settings.ThumbnailFolder = AssetFolder + "/Thumb";
                fileManger.SettingsFolders.ShowExpandButtons = true;
                fileManger.SettingsEditing.AllowCopy = true;
                fileManger.SettingsEditing.AllowDelete = true;
                fileManger.SettingsEditing.AllowDownload = true;
                fileManger.SettingsEditing.AllowMove = true;
                fileManger.SettingsEditing.AllowRename = true;

                FileManagerToolbarRefreshButton RefreshB = new FileManagerToolbarRefreshButton();
                RefreshB.BeginGroup = false;
                fileManger.SettingsToolbar.Items.Add(RefreshB);

                FileManagerToolbarDeleteButton DelButton = new FileManagerToolbarDeleteButton();                
                fileManger.SettingsToolbar.Items.Add(DelButton);

                FileManagerToolbarDownloadButton DownloadB= new FileManagerToolbarDownloadButton();
                fileManger.SettingsToolbar.Items.Add(DownloadB);

                //fileManger.ClientSideEvents.CustomCommand = "ChangeView";
                //fileManger.SettingsContextMenu.Enabled = true;

                FileManagerToolbarCustomButton FmanagerCB = new FileManagerToolbarCustomButton();
                FmanagerCB.GroupName = "ViewMode";
                FmanagerCB.ToolTip = "Details View";
                FmanagerCB.CommandName = "ChangeView-Details";
                FmanagerCB.Image.IconID = "grid_grid_16x16";
                fileManger.SettingsToolbar.Items.Add(FmanagerCB);
                
                FileManagerToolbarCustomButton FmanagerCustB = new FileManagerToolbarCustomButton();
                FmanagerCustB.GroupName = "ViewMode";
                FmanagerCustB.ToolTip = "Thumbnails View";
                FmanagerCustB.CommandName = "ChangeView-Thumbnails";
                FmanagerCustB.Image.IconID = "grid_cards_16x16";
                fileManger.SettingsToolbar.Items.Add(FmanagerCustB);

                fileManger.ClientSideEvents.CustomCommand = "ChangeView";
                fileManger.CustomCallback += ASPxFileManager1_CustomCallback;


                //fileManger.Styles.FileAreaFolder.Width = new Unit("50px");
                if (!string.IsNullOrEmpty(fileExtension))
                {
                    fileManger.Settings.AllowedFileExtensions = fileExtension.Split(',');
                }
                fileUploadManager.Controls.Add(fileManger);
               
               
            }
            else if (param == "aShowWiki")
            {
                WikiListPicker customWikiListPicker = (WikiListPicker)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/WikiListPicker.ascx");
                customWikiListPicker.ExcludedTickets = new List<string>();
                customWikiListPicker.CallBackWiki = "WikiCallBack";
                customWikiListPicker.EnableModuleDropdown = true;
                customWikiListPicker.ExcludedTickets = GetExcludedTickets();
                customWikiListPicker.Module = "WIKI";
                customWikiListPicker.ParentTicketId = Request["TicketId"] == null ? string.Empty : Convert.ToString(Request["TicketId"]);
                customWikiListPicker.ID = "wikiListPicker";
                fileUploadManager.Controls.Add(customWikiListPicker);
               
            }
        }

        private List<string> GetExcludedTickets()
        {
            List<string> selectedTicket = new List<string>();
            // bool isOwner = false;
            //bool isCreator = false;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DataTable dtWikiTickets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.WikiArticles, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");    //SPListHelper.GetDataTable(DatabaseObjects.Lists.WikiArticles);
            if (dtWikiTickets != null && dtWikiTickets.Rows.Count > 0)
            {
                //isOwner = WikiArticleHelper.IsWikiOwner();
                // isCreator = WikiArticleHelper.GetPermissions("add", "");
                //DataTable dt = ModuleInstanceDependency.LoadTableByPublicID(ParentTicketId);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataRow[] dr = dt.Select(string.Format("ModuleName= '{0}'", "WIK"));
                //    if (dr != null && dr.Length > 0)
                //    {
                //        dt = dr.CopyToDataTable();

                //        string ticketIds = string.Empty;
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            ticketIds = ticketIds + "'" + Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]) + "',";
                //            selectedTicket.Add(Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]));
                //        }
                //    }
                //}
            }

            return selectedTicket;
        }

        protected void ASPxFileManager1_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "Details")
            {               
                fileManger.SettingsFileList.View = DevExpress.Web.FileListView.Details;
            }
            else if (e.Parameter == "Thumbnails")
            {
                fileManger.SettingsFileList.View = DevExpress.Web.FileListView.Thumbnails;
            }
        }
    }
}