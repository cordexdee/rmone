using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager.Core;
using System.Web;
using uGovernIT.Manager.Managers;
using System.IO;
using System.Configuration;
using uGovernIT.Util.Cache;
using uGovernIT.Web.ControlTemplates;

namespace uGovernIT.Web
{
    public partial class FormLayoutAndPermissionAddEdit : UserControl
    {
        public int ItemID { private get; set; }
        public int CurrentTabIndex { get; set; }
        ModuleFormLayout ObjModuleFormLayout;
        UGITModule ugitModule = null;
        public string Module { get; set; }
        public string SkipConditionUrl { get; set; }
        protected string detailsURL = "/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&";
        protected string showHelpCardURL = "/Layouts/uGovernIT/delegatecontrol.aspx?control=helpcarddisplay&isHelp=true&";
        //private string newWikiParam = "wikilist";   
        private string formTitle = "Wiki Detail(Click On a Row to Select it)";  //added by amar
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string _ModuleTable = string.Empty;
        ModuleFormLayoutManager ObjModuleFormLayoutManager = new ModuleFormLayoutManager(HttpContext.Current.GetManagerContext());
        ModuleFormTabManager ObjModuleFormTabManager = new ModuleFormTabManager(HttpContext.Current.GetManagerContext());
        RequestRoleWriteAccessManager ObjRequestRoleWriteAccessManager = new RequestRoleWriteAccessManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected string oldColumnType = string.Empty;
        List<ModuleFormLayout> lstFormLayoutFields = null;
        private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string newParam = "listpicker";
        private string formTitleHelpCard = "Help Card Picker";

        protected override void OnInit(EventArgs e)
        {
           
            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, formTitleHelpCard, "HLP", string.Empty, "HelpCardList", txtHelpCard.ClientID)); //"TicketWiki"
            aShowHelpCard.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            aShowWiki.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            //aShowWiki.HRef = "javascript:showWiki('" + this.ClientID + "')";
            //aShowHelpCard.HRef = "javascript:showHelpCard('" + this.ClientID + "')";

            ObjModuleFormLayout = ObjModuleFormLayoutManager.LoadByID(ItemID);

            lstFormLayoutFields = ObjModuleFormLayoutManager.Load(x => x.ModuleNameLookup == Module);

            FillFieldName();
            FillTab();
            FillDisplayWidth();
            BindTargetTypeCategories(); //added by amar 
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ObjModuleFormLayout != null )
                {
                    Fill();
                }
                else
                {
                    lnkbtndelete.Visible = false;
                    SetTargetTypeDependency(); //It is used to set value of dlltype when Add new Item
                }
            }
            SkipConditionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagerule&moduleName={0}&controlId={1}&stageID={2}&IID={3}", Module, lblSkipOnCondition.ClientID, "", ItemID));
        }
        void FillFieldName()
        {
            TicketManager objTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ugitModule = ObjModuleManager.GetByName(Module);

            if (ugitModule != null)
                _ModuleTable = ugitModule.ModuleTable;
            else
                _ModuleTable = Module;

            DataTable dt = objTicketManager.GetTableSchemaDetail(DatabaseObjects.Tables.InformationSchema, _ModuleTable);
            List<string> fields = new List<string>();
            List<ModuleFormLayout> tabFields = lstFormLayoutFields.Where(x => x.TabId == CurrentTabIndex).ToList();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    
                    if (Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "ContentType" || Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]) != "Title")
                    {
                        if (tabFields.Exists(x => x.FieldName == Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema])))
                            continue;
                        else
                            fields.Add(Convert.ToString(dr[DatabaseObjects.Columns.ColumnNameSchema]));
                    }
                }

                // Add attachments field placeholder
                fields.Add(DatabaseObjects.Columns.Attachments);

                if (Module == "PMM")
                {
                    fields.Add("ProjectMonitors");
                }

                fields.Sort();
            }

            cmbFieldName.DataSource = fields;
            cmbFieldName.DataBind();
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#TableStart#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#TableEnd#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#PlaceHolder#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#Label#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#GroupStart#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#GroupEnd#"));
            cmbFieldName.Items.Insert(0, new DevExpress.Web.ListEditItem("#Control#"));
        }

        void FillDisplayWidth()
        {
            cmbDisplayWidth.Items.Add("1");
            cmbDisplayWidth.Items.Add("2");
            cmbDisplayWidth.Items.Add("3");
            cmbDisplayWidth.SelectedIndex = 0;
        }

        void FillTab()
        {
            List<ModuleFormTab> drTabs = ObjModuleFormTabManager.Load(x=>x.ModuleNameLookup==Module); 
            if (drTabs != null && drTabs.Count > 0)
            {
                ddlTab.DataSource = drTabs;
            }
            ddlTab.DataTextField = DatabaseObjects.Columns.TabName;
            ddlTab.DataValueField = DatabaseObjects.Columns.TabId;
            ddlTab.DataBind();

            ddlTab.Items.Insert(0, new ListItem("New", "0"));
            ddlTab.SelectedIndex = ddlTab.Items.IndexOf(ddlTab.Items.FindByValue(Convert.ToString(CurrentTabIndex)));
        }

        void Fill()
        {
            cmbFieldName.Text = Convert.ToString(ObjModuleFormLayout.FieldName);
            txtDisplayName.Text = Convert.ToString(ObjModuleFormLayout.FieldDisplayName);
            txtFieldSequence.Text = Convert.ToString(ObjModuleFormLayout.FieldSequence);
            cmbDisplayWidth.Text = Convert.ToString(ObjModuleFormLayout.FieldDisplayWidth);
            txtCustomProperties.Text = Convert.ToString(ObjModuleFormLayout.CustomProperties);
            txtToolTips.Text = Convert.ToString(ObjModuleFormLayout.Tooltip);
            ddlTab.SelectedValue = Convert.ToString(ObjModuleFormLayout.TabId);
            chkShowInMobile.Checked = UGITUtility.StringToBoolean(ObjModuleFormLayout.ShowInMobile);
            string strskiponcondition = FormulaBuilder.GetSkipConditionExpression(Convert.ToString(ObjModuleFormLayout.SkipOnCondition), Module, true);
            lblSkipOnCondition.Text = strskiponcondition;
            hdnSkipOnCondition.Set("SkipCondition", strskiponcondition);
            hideCheckTemplate.Checked = Convert.ToBoolean(ObjModuleFormLayout.HideInTemplate);
            
            if (Convert.ToString(ObjModuleFormLayout.ColumnType) == "Default" || Convert.ToString(ObjModuleFormLayout.ColumnType) == string.Empty)
                ddlType.SelectedIndex = 0;
            else
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(Convert.ToString(ObjModuleFormLayout.ColumnType)));


            ddlTargetType.SelectedIndex = ddlTargetType.Items.IndexOf(ddlTargetType.Items.FindByValue(Convert.ToString(ObjModuleFormLayout.TargetType)));


            SetTargetTypeDependency(); //added by amar 

            if (ddlTargetType.SelectedValue == "Wiki")
            {
                txtWiki.Text = Convert.ToString(ObjModuleFormLayout.TargetURL);
            }


            if (ddlTargetType.SelectedValue == "Link")
            {
                txtFileLink.Text = Convert.ToString(ObjModuleFormLayout.TargetURL);
            }


            if (ddlTargetType.SelectedValue == "File")
            {
                var attachments = ObjModuleFormLayout.Attachments;

                string fileName = Path.GetFileName(Convert.ToString(ObjModuleFormLayout.TargetURL));
                
                lblUploadedFile.Text = fileName;
                
            }

            if (ddlTargetType.SelectedValue == "HelpCard")
            {                
                txtHelpCard.Text = Convert.ToString(ObjModuleFormLayout.TargetURL);                
            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ObjModuleFormLayout == null)
                ObjModuleFormLayout = new ModuleFormLayout();
            List<string> stringList = new List<string>{"#Control#","#GroupEnd#","#GroupStart#","#Label#","#PlaceHolder#","#TableEnd#","#TableStart#"};

            if (!string.IsNullOrEmpty(txtDisplayName.Text.Trim()) && !stringList.Contains(cmbFieldName.Text.Trim()))
            {
                //[+][SANKET][14-08-2023][Added code for FormLayout add new item save and update]
                //int DuplicateCount = lstFormLayoutFields.Where(x => x.ModuleNameLookup == Module && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower()).Count();
                int DuplicateCount = lstFormLayoutFields.Where(x => x.ModuleNameLookup == Module && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower() && x.TabId == UGITUtility.StringToInt(ddlTab.SelectedValue)).Count();
                ModuleFormLayout _ModuleFormLayout = lstFormLayoutFields.Where(x => x.ModuleNameLookup == Module && x.FieldDisplayName.ToLower() == txtDisplayName.Text.Trim().ToLower() && x.TabId == UGITUtility.StringToInt(ddlTab.SelectedValue)).FirstOrDefault();
                if (ItemID == 0 && _ModuleFormLayout == null)
                    ObjModuleFormLayout.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else if (ItemID > 0 && _ModuleFormLayout != null && ItemID == _ModuleFormLayout.ID && _ModuleFormLayout.TabId == ObjModuleFormLayout.TabId)
                    ObjModuleFormLayout.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else if (DuplicateCount == 0)
                    ObjModuleFormLayout.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else if (DuplicateCount == 1 && _ModuleFormLayout.TabId != UGITUtility.StringToInt(ddlTab.SelectedValue))
                    ObjModuleFormLayout.FieldDisplayName = string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? "" : txtDisplayName.Text.Trim();
                else
                {
                    lblerrormsg.Text = "Display Name already exist";
                    return;
                }
            }
            ObjModuleFormLayout.FieldName= cmbFieldName.Text.Trim();//txtFieldName.Text.Trim();
            ObjModuleFormLayout.FieldDisplayName= string.IsNullOrEmpty(txtDisplayName.Text.Trim()) ? cmbFieldName.Text.Trim() : txtDisplayName.Text.Trim();
            ObjModuleFormLayout.FieldSequence =UGITUtility.StringToInt(string.IsNullOrEmpty(txtFieldSequence.Text.Trim()) ? "0" : txtFieldSequence.Text);
            ObjModuleFormLayout.Title= txtDisplayName.Text.Trim();
            ObjModuleFormLayout.ModuleNameLookup= Module;
            ObjModuleFormLayout.FieldDisplayWidth= UGITUtility.StringToInt(cmbDisplayWidth.Text.Trim());
            ObjModuleFormLayout.CustomProperties= txtCustomProperties.Text.Trim();
            ObjModuleFormLayout.ShowInMobile=chkShowInMobile.Checked;
            ObjModuleFormLayout.TabId= UGITUtility.StringToInt(ddlTab.SelectedValue);
            ObjModuleFormLayout.SkipOnCondition= FormulaBuilder.GetSkipConditionExpression(hdnSkipOnCondition.Contains("SkipCondition") ? Convert.ToString(hdnSkipOnCondition.Get("SkipCondition")) : string.Empty, Module, false);
            //Set Type of Field
            oldColumnType = string.IsNullOrEmpty(ObjModuleFormLayout.ColumnType) ? string.Empty : ObjModuleFormLayout.ColumnType;
            ObjModuleFormLayout.ColumnType = ddlType.SelectedItem.Value == "" ? "Default" : ddlType.SelectedItem.Value;
            // ObjModuleFormLayout.TargetURL= UGITFileUploadManager1.GetImageUrl(); //commented by amar

            //start added by amar
            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    if (fileUploadControl.HasFile)
                    {
                        

                        string AssetFolder = ConfigurationManager.AppSettings["AssetFolder"];
                        string finalPath = AssetFolder + "/" + ObjModuleFormLayout.TenantID;
                        string folderPath = Server.MapPath(finalPath);

                        

                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        
                        //Save the File to the Directory (Folder).
                        fileUploadControl.SaveAs(folderPath + "/" + Path.GetFileName(fileUploadControl.FileName));

                        ObjModuleFormLayout.TargetURL = finalPath + "/" + Path.GetFileName(fileUploadControl.FileName);
                        ObjModuleFormLayout.TargetType = ddlTargetType.SelectedValue;


                    }
                    break;
                case "Link":
                    ObjModuleFormLayout.TargetURL = txtFileLink.Text.Trim();
                    ObjModuleFormLayout.TargetType = ddlTargetType.SelectedValue;
                    break;
                case "Wiki":
                    {
                        if (txtWiki.Visible)
                        {
                            ObjModuleFormLayout.TargetURL = txtWiki.Text.Trim();
                        }
                        ObjModuleFormLayout.TargetType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "HelpCard":
                    {
                        if (txtHelpCard.Visible)
                        {
                            ObjModuleFormLayout.TargetURL = txtHelpCard.Text.Trim();
                        }
                        ObjModuleFormLayout.TargetType = ddlTargetType.SelectedValue;
                    }
                    break;
                default:
                    break;
            }

          
            ObjModuleFormLayout.Tooltip= txtToolTips.Text;
            ObjModuleFormLayout.HideInTemplate=hideCheckTemplate.Checked;
            ObjModuleFormLayoutManager.AddOrUpdate(ObjModuleFormLayout);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Add/Updated Formlayout: {ObjModuleFormLayout.Title}; Module: {ObjModuleFormLayout.ModuleNameLookup}", 
                                                            Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            
            if (!string.IsNullOrEmpty(txtFieldSequence.Text))
            {
                ModuleFormLayout drSecondItem = ObjModuleFormLayoutManager.Load(x => x.FieldSequence == UGITUtility.StringToInt(txtFieldSequence.Text.Trim()) && x.ID != ObjModuleFormLayout.ID && x.ModuleNameLookup == Module && x.TabId == UGITUtility.StringToLong(ddlTab.SelectedValue)).FirstOrDefault();
              
                if (drSecondItem != null)
                {
                    string firstId = UGITUtility.ObjectToString(ObjModuleFormLayout.ID);
                    string secondId = UGITUtility.ObjectToString(drSecondItem.ID);
                    ObjModuleFormLayoutManager.UpdateSequence(firstId, secondId, Module, UGITUtility.ObjectToString(ddlTab.SelectedValue));
                }
            }

            Context.Cache.Add(string.Format("CURRENTTABINTEX-{0}", Context.CurrentUser().Id), CurrentTabIndex == -1 ? UGITUtility.StringToInt(ddlTab.SelectedValue) : CurrentTabIndex, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            uHelper.ClosePopUpAndEndResponse(Context, true);
            
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {               
                if (ObjModuleFormLayout != null )
                {
                    //Delete From RequestRoleWriteAccess
                    string SPFormLayout_FieldName = Convert.ToString(ObjModuleFormLayout.FieldName);
                    string SPFormLayout_ModuleName = Convert.ToString(ObjModuleFormLayout.ModuleNameLookup);
                    List<ModuleRoleWriteAccess> itemsToDelete = ObjRequestRoleWriteAccessManager.Load(x=>x.FieldName.Equals(SPFormLayout_FieldName) && x.ModuleNameLookup.Equals(SPFormLayout_ModuleName));
                
                    if (itemsToDelete.ToList().Count > 0)
                    {                   
                        foreach (ModuleRoleWriteAccess dr in itemsToDelete)
                        {
                            ObjRequestRoleWriteAccessManager.Delete(dr);
                        }
                    }
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Delete Formlayout item: {ObjModuleFormLayout.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    ObjModuleFormLayoutManager.Delete(ObjModuleFormLayout);

                    List<ModuleFormLayout> drFormLayout = ObjModuleFormLayoutManager.Load(x => x.ModuleNameLookup == SPFormLayout_ModuleName && x.TabId == Convert.ToInt32(ObjModuleFormLayout.TabId));
                    List<ModuleFormLayout> dataView = drFormLayout.OrderBy(x => x.FieldSequence).ToList();
                   
                    int ctr = 0;
                    foreach (ModuleFormLayout dr in dataView)
                    {
                        dr.FieldSequence = UGITUtility.StringToInt(++ctr);
                        ObjModuleFormLayoutManager.Update(dr);
                    }
                }
                Context.Cache.Add(string.Format("CURRENTTABINTEX-{0}", Context.CurrentUser().Id), CurrentTabIndex == -1 ? UGITUtility.StringToInt(ddlTab.SelectedValue) : CurrentTabIndex, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Cache.Add(string.Format("CURRENTTABINTEX-{0}", Context.CurrentUser().Id), CurrentTabIndex == -1 ? UGITUtility.StringToInt(ddlTab.SelectedValue) : CurrentTabIndex, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            uHelper.ClosePopUpAndEndResponse(Context, true);
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

        private void BindTargetTypeCategories()
        {
            ddlTargetType.Items.Add(new ListItem("File", "File"));
            ddlTargetType.Items.Add(new ListItem("Link", "Link"));
            ddlTargetType.Items.Add(new ListItem("Wiki", "Wiki"));
            ddlTargetType.Items.Add(new ListItem("Help Card", "HelpCard"));
            ddlTargetType.DataBind();
        }


        protected void cb_Callback(object sender, CallbackEventArgsBase e)
        {
            // hdnPopupContentInfo["controlkey"] = e.Parameter;

            string LinkCallBackType = e.Parameter;
            if (e.Parameter.EqualsIgnoreCase("aShowHelpCard"))
            {
                HelpCardSelect.Controls.Clear();
                LoadHelpCardPickerList();
            }
            WikiSelect.Controls.Clear();
            // txtHelp.Text = string.Empty;
            //  fileUploadManager.Controls.Add(hdnPopupContentInfo);
            LoadControl();
        }


        private void LoadHelpCardPickerList()
        {
            HelpCardListPicker helpCardListPicker = (HelpCardListPicker)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/HelpCardListPicker.ascx");
            helpCardListPicker.ExcludedTickets = new List<string>();
            helpCardListPicker.CallBackWiki = "HelpcardCallBack";
            //helpCardListPicker.EnableModuleDropdown = true;
            //customWikiListPicker.ExcludedTickets = GetExcludedTickets();
            helpCardListPicker.Module ="HLP";
            helpCardListPicker.ParentTicketId = Request["TicketId"] == null ? string.Empty : Convert.ToString(Request["TicketId"]);
            helpCardListPicker.ID = "helpCardListPicker";
            HelpCardSelect.Controls.Add(helpCardListPicker);
        }

        private void LoadControl()
        {


            WikiListPicker customWikiListPicker = (WikiListPicker)Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/WikiListPicker.ascx");
            customWikiListPicker.ExcludedTickets = new List<string>();
            customWikiListPicker.CallBackWiki = "WikiCallBack";
            customWikiListPicker.EnableModuleDropdown = true;
            //customWikiListPicker.ExcludedTickets = GetExcludedTickets();
            customWikiListPicker.Module = "WIKI";
            customWikiListPicker.ParentTicketId = Request["TicketId"] == null ? string.Empty : Convert.ToString(Request["TicketId"]);
            customWikiListPicker.ID = "wikiListPicker";
            WikiSelect.Controls.Add(customWikiListPicker);


        }


        protected void UpdateColumnInFieldConfiguration(string fieldName) 
        {
            FieldConfigurationManager configurationManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            FieldConfiguration field = configurationManager.Load(x => x.FieldName == fieldName && x.TableName == _ModuleTable).FirstOrDefault();

            if (field == null)
                field = new FieldConfiguration();

            field.FieldName = ObjModuleFormLayout.FieldName;
            field.Datatype = ObjModuleFormLayout.ColumnType.ToLower() == "default" ? "" : ObjModuleFormLayout.ColumnType;
            field.TableName = _ModuleTable;
            field.Notation = field.Datatype.ToLower() == "currency" ? "en-US" : string.Empty;

            // Save/update field in field configuration and refresh cache
            configurationManager.Save(field);
            configurationManager.RefreshCache();
        }

    }
}
