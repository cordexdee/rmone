using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CustomListDropDown : UserControl
    {

        public string Value { get; set; }
        public string TicketId { get; set; }
        public string absoluteUrlEdit = "layouts/ugovernit/DelegateControl.aspx?&disableReloadParent=true&control={0}&ID={1}&ticketID={2}&ControlId={3}";
        public string ControlList { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationGroup { get; set; }

        public ControlMode ControlMode { get; set; }
        public FieldDesignMode FieldDesignMode { get; set; }

        public bool IsMandatory { get; set; }
        public bool ShowSearch { get; set; }
        public bool ShowAdd { get; set; }
        public bool EnableEditButton { get; set; }
        public bool IsMult { get; set; }

        // private int selectedId = 0;
        private string ModuleName { get; set; }
        private string selectedId = string.Empty;
        private int rowCount = 20;

        private ApplicationContext _context = null;
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager _ticketManager = null;

        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        public void SetValue(string value)
        {
            Value = value;
            if (!string.IsNullOrEmpty(Value))
            {
                if (Value == "<Value Varies>")
                {
                    if (IsMult)
                    {
                        GridLookup.Text = Value;
                        GridLookup.GridView.Selection.SelectRowByKey("-1");
                    }
                    else
                    {
                        ddlList.Items.Insert(0, new ListEditItem(Value, -1));
                        //ddlList.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (IsMult)
                    {
                        string[] selectedKeys = value.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in selectedKeys)
                        {
                            GridLookup.GridView.Selection.SelectRowByKey(val);
                        }

                        hlnk.Text = GridLookup.Text;
                        if (ControlList == DatabaseObjects.Tables.CRMContact)
                        {
                            //mailTo.Attributes.Add("contactId", string.Join(",", selectedKeys));
                            hlnk.Attributes.Add("contactId", string.Join(",", selectedKeys));
                            //hlnk.ToolTip = GridLookup.Text;
                        }
                        //hlnk.ToolTip = GridLookup.Text;

                        ddlList.Value = Value;
                    }
                    else
                    {
                        if (Value.Contains(Constants.Separator))
                        {
                            //ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByValue(UGITUtility.GetLookupID(Value).ToString()));
                            ddlList.Value = Value;
                        }
                        else
                        {
                            //ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByValue(Value));
                            ddlList.Value = Value;
                        }

                        hlnk.Text = ddlList.Text;

                        //if (ddlList.SelectedItem != null)
                        //    hlnk.Attributes.Add("contactId", ddlList.SelectedItem.Value.ToString());
                        //mailTo.Attributes.Add("contactId", ddlList.SelectedItem.Value.ToString());

                        if (ControlList == DatabaseObjects.Tables.CRMContact)
                        {
                            /*
                            if (ddlList.SelectedItem != null)
                                hlnk.Attributes.Add("contactId", ddlList.SelectedItem.Value.ToString());
                            */

                            if (!string.IsNullOrEmpty(Convert.ToString(ddlList.Value)))
                                hlnk.Attributes.Add("contactId", Convert.ToString(ddlList.Value));

                            //hlnk.ToolTip = ddlList.Text;
                        }
                        //hlnk.ToolTip = ddlList.Text;

                        if (FieldName == DatabaseObjects.Columns.CRMCompanyLookup)
                        {
                            if (ddlList.Items.Count > 0 && ddlList.Items.FindByValue(ddlList.Value) != null)
                            {
                                var spListItem = _ticketManager.GetByTicketIdFromCache(ModuleNames.COM, UGITUtility.ObjectToString(ddlList.Value));
                                //var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.TicketId}='{ddlList.Value}' and TenantID='{ApplicationContext.TenantID}'").Select()[0];
                                if (spListItem != null)
                                {
                                    hndCompany.Visible = true;
                                    hndCompany.Value = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetValue()
        {
            string val = string.Empty;
            if (IsMult)
            {
                if (GridLookup.Text == "<Value Varies>")
                {
                    val = GridLookup.Text;
                }
                else
                {
                    List<LookupValue> lookups = new List<LookupValue>();
                    List<object> multiKeys = GridLookup.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title);
                    foreach (object obj in multiKeys)
                    {
                        //lookups.Add(new LookupValue(Convert.ToInt32(((object[])obj)[0]), Convert.ToString(((object[])obj)[1])));
                        lookups.Add(new LookupValue(Convert.ToString(((object[])obj)[0]), Convert.ToString(((object[])obj)[1])));
                    }
                    val = string.Join(Constants.Separator6, lookups.Select(x => x.ID.ToString()).ToArray());
                }
            }
            else
            {
                if (ddlList.Text == "<Value Varies>")
                    val = ddlList.Text;
                else
                {
                    if (FieldName == DatabaseObjects.Columns.ContactLookup)
                    {
                        val = hndContact.Value;
                    }
                    else
                    {
                        val = Convert.ToString(ddlList.Value);
                    }
                }
            }

            return val;
        }

        protected override void OnInit(EventArgs e)
        {
            //rowCount = IsPostBack ? 100 : 1;
            ShowAdd = false;
            ShowSearch = false;
            // imgAdd.Visible = ShowAdd;
            //if (FieldName == DatabaseObjects.Columns.CRMCompanyTitleLookup)
            if (ControlList == DatabaseObjects.Tables.CRMCompany)
            {
                //imgAdd.Visible = true;
                /*
                 if (FieldName == "Broker")
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(BrokerContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); BrokerContact.PerformCallback('broker');  }}}";
                 }
                 else if (FieldName == DatabaseObjects.Columns.CompanyEngineer)
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(CompanyEngineerContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); CompanyEngineerContact.PerformCallback('CompanyEngineer');  }}}";
                 }
                 else if (FieldName == "BuildingOwnerCompany")
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(BuildingOwnerCompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); BuildingOwnerCompanyContact.PerformCallback('BuildingOwnerCompany');  }}}";
                 }
                 else if (FieldName == DatabaseObjects.Columns.ConstructionManagementCompany)
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(ConstructionManagementCompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); ConstructionManagementCompanyContact.PerformCallback('ConstructionManagementCompany');  }}}";
                 }
                 else if (FieldName == DatabaseObjects.Columns.CompanyArchitect)
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(CompanyArchitectContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  CompanyArchitectContact.PerformCallback('Architect');  }}}";
                 }
                 else if (FieldName == DatabaseObjects.Columns.CRMCompanyTitleLookup)
                 {
                     ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){   if(typeof(CompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  CompanyContact.PerformCallback('CompanyTitle');} if(typeof(ProjectTeamContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  ProjectTeamContact.PerformCallback('CompanyTitle1');}}  $.cookie(\"dataChanged\", 1, { path: \"/\" }); }";
                     dvHiddenFields.Attributes.Add("class", FieldName);
                 }           
                 */

                if (FieldName == "Broker")
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(BrokerContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); BrokerContact.PerformCallback('broker');   }}}";
                }
                else if (FieldName == DatabaseObjects.Columns.CompanyEngineer)
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(CompanyEngineerContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); CompanyEngineerContact.PerformCallback('CompanyEngineer');  }}}";
                }
                else if (FieldName == "BuildingOwnerCompany")
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(BuildingOwnerCompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); BuildingOwnerCompanyContact.PerformCallback('BuildingOwnerCompany');  }}}";
                }
                else if (FieldName == DatabaseObjects.Columns.ConstructionManagementCompany)
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(ConstructionManagementCompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show(); ConstructionManagementCompanyContact.PerformCallback('ConstructionManagementCompany');  }}}";
                }
                else if (FieldName == DatabaseObjects.Columns.CompanyArchitect)
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){ if(typeof(CompanyArchitectContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  CompanyArchitectContact.PerformCallback('Architect');  }}}";
                }
                else if (FieldName == DatabaseObjects.Columns.CRMCompanyLookup)
                {
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) { if(s.GetValue() != s.GetText()){   if(typeof(CompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  CompanyContact.PerformCallback('CompanyTitle');} if(typeof(ProjectTeamContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  ProjectTeamContact.PerformCallback('CompanyTitle1');}} setTimeout(function(){ SingleSelectTokenBox(s, e); }, 2000); $.cookie(\"dataChanged\", 1, { path: \"/\" }); }";
                    dvHiddenFields.Attributes.Add("class", FieldName);
                }
            }

            if (ControlList == DatabaseObjects.Tables.CRMContact)
            {
                //imgAdd.Visible = true;
                if (FieldName == "BrokerContact")
                {
                    ASPxCallbackPanel1.ClientInstanceName = "BrokerContact";
                }
                else if (FieldName == DatabaseObjects.Columns.EngineerContact)
                {
                    ASPxCallbackPanel1.ClientInstanceName = "CompanyEngineerContact";
                }
                else if (FieldName == DatabaseObjects.Columns.BuildingOwner)
                {
                    ASPxCallbackPanel1.ClientInstanceName = "BuildingOwnerCompanyContact";
                }
                else if (FieldName == DatabaseObjects.Columns.ConstructionManagerContact)
                {
                    ASPxCallbackPanel1.ClientInstanceName = "ConstructionManagementCompanyContact";
                }
                else if (FieldName == DatabaseObjects.Columns.Architect)
                {
                    ASPxCallbackPanel1.ClientInstanceName = "CompanyArchitectContact";
                }
                else if (FieldName == DatabaseObjects.Columns.ContactLookup)
                {
                    ASPxCallbackPanel1.ClientInstanceName = "CompanyContact";
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) { setTimeout(function(){ ContactSingleSelectTokenBox(s, e); }, 1000);   }";
                    dvHiddenFields.Attributes.Add("class", FieldName);
                }
                else if (FieldName == "ProjectTeamLookup")
                {
                    ASPxCallbackPanel1.ClientInstanceName = "ProjectTeamContact";
                }
                else if (FieldName == DatabaseObjects.Columns.LeadSource)
                {
                    // ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) {if(s.GetValue() != s.GetText()){updateLeadSourceCompany(s.GetValue());}}";
                    ddlList.ClientSideEvents.ValueChanged = "function ddlList_SelectedIndexChanged(s, e) { setTimeout(function(){ SingleSelectTokenBox(s, e); }, 2000);   }";
                }
            }

            if (IsMult)
            {
                if (FieldName == DatabaseObjects.Columns.CRMCompanyLookup || FieldName == DatabaseObjects.Columns.LeadSource || FieldName == DatabaseObjects.Columns.TagMultiLookup || FieldName == DatabaseObjects.Columns.ContactLookup)
                {
                    IsMult = false;
                    pnlContactMult.Visible = false;
                    pnlContact.Visible = true;

                }
            }

            lblContact.CssClass = FieldName;
            if (hlnk != null)
                hlnk.CssClass = FieldName + " jqtooltip";
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            string filter = UGITUtility.GetCookieValue(Request, FieldName);
            //if (!IsPostBack)
            {
                LoadDataOnDemand(0, ddlList, Value, filter);
            }

            if (!string.IsNullOrEmpty(Value) && Value != "<Value Varies>")
            {
                if (Value.Contains(Constants.Separator))
                {
                    selectedId = Convert.ToString(UGITUtility.GetLookupID(Value));
                }
                else
                {
                    selectedId = Convert.ToString(Value);
                }
            }

            if (ControlList == DatabaseObjects.Tables.CRMCompany)
            {
                string keyPrefix = "";
                if (FieldName == "Broker")
                {
                    keyPrefix = FieldName;
                }
                else if (FieldName == DatabaseObjects.Columns.CompanyEngineer)
                {
                    keyPrefix = "Engineer";
                }
                else if (FieldName == "BuildingOwnerCompany")
                {
                    keyPrefix = "BuildingOwner";
                }
                else if (FieldName == DatabaseObjects.Columns.ConstructionManagementCompany)
                {
                    keyPrefix = "ConstructionManagement";
                }
                else if (FieldName == DatabaseObjects.Columns.CompanyArchitect)
                {
                    keyPrefix = "Architect";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(ddlList.Value)))
                {
                    if (ddlList.Tokens.Count > 1)
                    {
                        ddlList.Tokens.RemoveAt(0);
                    }
                    selectedId = Convert.ToString(ddlList.Value);
                }
                else
                {
                    Context.Cache.Remove(string.Format("{1}CompanyId-{0}", ApplicationContext.CurrentUser.Id, keyPrefix));
                }

                if (!string.IsNullOrEmpty(selectedId))
                {
                    Context.Cache.Remove(string.Format("{1}CompanyId-{0}", ApplicationContext.CurrentUser.Id, keyPrefix));
                    Context.Cache.Add(string.Format("{1}CompanyId-{0}", ApplicationContext.CurrentUser.Id, keyPrefix), string.Format("compID={0}", selectedId), null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

                    // For displaying Market sector based on company change                    
                    string viewFields = $"{DatabaseObjects.Columns.Id}={selectedId} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'";
                    //var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany,viewFields).Select()[0];//Check selectedid

                    //if (spListItem != null)
                    //{

                    //    hndCompany.Visible = true;
                    //    hndCompany.Value = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                    //}
                }
            }

            if (ControlList == DatabaseObjects.Tables.CRMCompany)
            {
                ModuleName = "COM";
            }
            else
            {
                ModuleName = "CON";
                // imgAdd.Visible = false;
            }


            if (ControlMode == ControlMode.Display)
            {
                if (!ddlList.IsCallback && !IsPostBack)
                {
                    LoadDataOnDemand(0, ddlList, selectedId, filter);
                    SetValue(Value);
                }
                pnlContact.Style.Add("display", "none");
                pnlContactMult.Style.Add("display", "none");

                hlnk.Visible = true;
                if (ControlList == DatabaseObjects.Tables.CRMContact && !IsMult && !string.IsNullOrEmpty(selectedId))
                {
                    divEditContact.Attributes.Add("onmouseover", string.Format("javascript:showmailbuttonOnhover( this);"));
                    divEditContact.Attributes.Add("onmouseout", string.Format("javascript:hidemailbuttonOnhover(this);"));
                }

                pnlContactMult.Style.Add("display", "none");
            }
            else if (ControlMode == ControlMode.Edit && EnableEditButton)
            {
                if (FieldDesignMode == FieldDesignMode.WithEdit)
                {
                    pnlContact.Style.Add("display", "none");
                    pnlContactMult.Style.Add("display", "none");
                    hlnk.Visible = true;
                    editContact.Visible = true;
                }
                else if (FieldDesignMode == FieldDesignMode.Normal)
                {
                    pnlContact.Style.Add("display", "block");
                    pnlContactMult.Style.Add("display", "block");
                    hlnk.Visible = false;
                    editContact.Visible = false;
                }

                //pnlContact.Style.Add("display", "none");
                //pnlContactMult.Style.Add("display", "none");
                if (!ddlList.IsCallback && !IsPostBack)
                {
                    LoadDataOnDemand(0, ddlList, selectedId, filter);
                    SetValue(Value);
                }

                hlnk.Text = ddlList.Text;
                //hlnk.Visible = true;

                if (ControlList == DatabaseObjects.Tables.CRMContact && !IsMult && !string.IsNullOrEmpty(selectedId))
                {
                    divEditContact.Attributes.Add("onmouseover", string.Format("javascript:showcontacteditbuttonOnhover(this);showmailbuttonOnhover( this);"));
                    divEditContact.Attributes.Add("onmouseout", string.Format("javascript:hidecontacteditbuttonOnhover(this);hidemailbuttonOnhover(this);"));
                }
                else
                {
                    divEditContact.Attributes.Add("onmouseover", string.Format("javascript:showcontacteditbuttonOnhover(this);"));
                    divEditContact.Attributes.Add("onmouseout", string.Format("javascript:hidecontacteditbuttonOnhover(this);"));
                }
            }

            if (Request.Form["__EVENTTARGET"] != null && (Convert.ToString(Request.Form["__EVENTTARGET"]).Contains("$superAdminEditButton"))) // || Convert.ToString(Request.Form["__EVENTTARGET"]).Contains("ddlList")
            {
                if (IsMult)
                {
                    GridLookup.DataBind();
                }
                else
                {
                    LoadDataOnDemand(0, ddlList, selectedId, filter);
                }
                SetValue(Value);
            }
            /*
            else if (IsPostBack)
            {
                //if (FieldName == DatabaseObjects.Columns.ContactLookup)
                if (FieldName == DatabaseObjects.Columns.ContactLookup || FieldName == DatabaseObjects.Columns.BrokerContact || FieldName == DatabaseObjects.Columns.EngineerContact || FieldName == DatabaseObjects.Columns.ConstructionManagerContact || FieldName == DatabaseObjects.Columns.Architect || FieldName == DatabaseObjects.Columns.BuildingOwner)
                {
            //        LoadDataOnDemand(0, ddlList, selectedId, filter);
                    if (ddlList.Items.Count == 0 || ddlList.Items.FindByValue(ddlList.Value) == null)
                    {
                        //ddlList.SelectedIndex = -1;                       
                    }
                }
            }
            */
            // Multi contact lookup issue fix: Bind multilookup control incase first time load & company dropdown postback(CustomPostback)
            if ((!GridLookup.IsCallback && !IsPostBack && IsMult))
            {
                GridLookup.DataBind();
            }
            else if (Request.Form["__EVENTARGUMENT"] != null && Convert.ToString(Request.Form["__EVENTARGUMENT"]) == "CustomPostback" && IsMult)
            {
                GridLookup.DataBind();
            }

            if (IsMult)
            {
                GridLookup.GridView.Width = GridLookup.Width;
            }

            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Response.HeadersWritten)
                UGITUtility.CreateCookie(Response, FieldName, "");

            //if (uHelper.getModuleNameByTicketId(TicketId) == "CPP")
            //    imgAdd.Visible = false;

            //int Id = 0;
            string Id = string.Empty;
            if (!string.IsNullOrEmpty(Value) && Value != "<Value Varies>")
            {
                if (Value.Contains(Constants.Separator6))
                {
                    //Id =Convert.ToString( UGITUtility.GetLookupID(Value));
                    Id = Value;
                }
                else
                {
                    Id = Value;
                }
            }

            try
            {


                if (ControlList == DatabaseObjects.Tables.CRMCompany)
                {
                    ModuleName = "COM";

                    string compID = string.Empty;
                    object cacheVal = Context.Cache.Get(string.Format("CompanyId-{0}", ApplicationContext.CurrentUser.Id));
                    if (cacheVal != null)
                    {

                        Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                        if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                        {
                            compID = Convert.ToString(cacheParams["compid"]);
                        }
                    }

                    //var moduleDetail = ModuleViewManager.LoadByName(ModuleName,true);

                    //string absoluteUrlAdd = string.Format("{0}?ticketID=0&ControlId={1}&hpac=true", UGITUtility.GetAbsoluteURL(moduleDetail.ModuleRelativePagePath.ToString()), dvReload.ClientID);
                    //imgAdd.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2}','100','90',0,'{1}')", absoluteUrlAdd, Server.UrlEncode(Request.Url.AbsolutePath), UGITUtility.newTicketTitle(ModuleName)));

                    if (!string.IsNullOrEmpty(Id))
                    {
                        //string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title);
                        //SPListItem spListItem = SPListHelper.GetSPListItem(Convert.ToString(moduleDetail[DatabaseObjects.Columns.ModuleTicketTable]), Id, ApplicationContext, viewFields);

                        string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                        var dr = _ticketManager.GetByTicketIdFromCache(ModuleNames.COM, Value);
                        //var dr = GetTableDataManager.GetTableData(ModuleViewManager.GetModuleTableName("COM"), $"{DatabaseObjects.Columns.TicketId} = '{Value}' and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'", $"{DatabaseObjects.Columns.TicketId}, {DatabaseObjects.Columns.Telephone},{DatabaseObjects.Columns.Title}", string.Empty).Select()[0];
                        var viewUrl = ModuleViewManager.LoadByName("COM").StaticModulePagePath;

                        if (dr != null)
                        {
                            hlnk.Attributes.Add("onClick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", dr[DatabaseObjects.Columns.TicketId], dr[DatabaseObjects.Columns.Title]), string.Format("{0}: {1}", dr[DatabaseObjects.Columns.TicketId], dr[DatabaseObjects.Columns.Title]), sourceURL, 90, 90));
                            hlnk.Attributes.Add("style", "cursor:pointer");

                            if (UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.Telephone) &&  !string.IsNullOrEmpty(UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Telephone])))
                            {
                                hlnk.CssClass = " jqtooltip";
                                hlnk.ToolTip = $"Main Phone: {dr[DatabaseObjects.Columns.Telephone]}";
                            }
                        }
                    }
                }
                else
                {
                    ModuleName = "CON";
                    // imgAdd.Visible = false;

                    string compID = string.Empty;
                    object cacheVal = Context.Cache.Get(string.Format("CompanyId-{0}", ApplicationContext.CurrentUser.Id));
                    if (cacheVal != null)
                    {
                        Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                        if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                        {
                            compID = Convert.ToString(cacheParams["compid"]);
                        }
                    }

                    //DataRow moduleDetail = uGITCache.GetModuleDetails(ModuleName);
                    //var moduleDetail = ModuleViewManager.LoadByName(ModuleName);

                    //string absoluteUrlAdd = string.Format("{0}?ticketID=0&CompanyId={2}&ControlId={1}&hpac=true", UGITUtility.GetAbsoluteURL(moduleDetail.ModuleRelativePagePath.ToString()), dvReload.ClientID, compID);
                    //imgAdd.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2}','100','90',0,'{1}')", absoluteUrlAdd, Server.UrlEncode(Request.Url.AbsolutePath), UGITUtility.newTicketTitle(ModuleName)));
                    if (!string.IsNullOrEmpty(Id))
                    {
                        DataRow[] dr = GetTableDataManager.GetTableData(ModuleViewManager.GetModuleTableName("CON"), $"{DatabaseObjects.Columns.TicketId} in ('{Value}') and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'", $"{DatabaseObjects.Columns.Title}, CRMContactType, {DatabaseObjects.Columns.Telephone}, {DatabaseObjects.Columns.EmailAddress}, {DatabaseObjects.Columns.TicketId}", string.Empty).Select();

                        if (dr.Length > 0)
                        {
                            string viewUrl = string.Empty;
                            //string title = text;
                            string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                            viewUrl = ModuleViewManager.LoadByName("CON").StaticModulePagePath;
                            hlnk.Attributes.Add("onClick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", dr[0][DatabaseObjects.Columns.TicketId]), string.Format("{0}: {1}", dr[0][DatabaseObjects.Columns.TicketId], dr[0][DatabaseObjects.Columns.Title]), sourceURL, 90, 90));
                            hlnk.Attributes.Add("style", "cursor:pointer");

                            StringBuilder sbContact = new StringBuilder();

                            for (int i = 0; i < dr.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr[i][DatabaseObjects.Columns.Title])))
                                    sbContact.Append(dr[i][DatabaseObjects.Columns.Title]);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[i]["CRMContactType"])))
                                    sbContact.Append($"\nContact Type: {dr[i]["CRMContactType"]}");

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[i][DatabaseObjects.Columns.Telephone])))
                                    sbContact.Append($"\nPhone: {dr[i][DatabaseObjects.Columns.Telephone]}");

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[i][DatabaseObjects.Columns.EmailAddress])))
                                    sbContact.Append($"\nEmail: {dr[i][DatabaseObjects.Columns.EmailAddress]}");

                                if (i < dr.Length - 1)
                                    sbContact.Append($"\n\n");
                            }

                            hlnk.Attributes.Add("contactId", Value);
                            hlnk.CssClass = " jqtooltip";
                            hlnk.ToolTip = Convert.ToString(sbContact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteLog(string.Format("Error in Customdropdown list {1} {0}", ex.StackTrace.ToString(), FieldName));
            }

            if (IsMult && !IsPostBack)
            {
                GridLookup.DataBind();
            }
        }

        protected void btnReloadDropDown_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hndValue.Value))
            {
                string value = string.Empty;
                if (hndValue.Value.Contains(Constants.Separator))
                {
                    value = Convert.ToString(UGITUtility.GetLookupID(hndValue.Value));
                }
                else
                {
                    value = hndValue.Value;
                }
                if (FieldName == DatabaseObjects.Columns.CRMCompanyLookup)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        //DataRow moduleDetail = uGITCache.GetModuleDetails("COM");
                        var moduleDetail = ModuleViewManager.LoadByName("COM");
                        absoluteUrlEdit = string.Format("{0}?ticketID=0&ControlId={1}&hpac=true", UGITUtility.GetAbsoluteURL(moduleDetail.ModuleRelativePagePath.ToString()), dvReload.ClientID);
                        imgAdd.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2}','100','90',0,'{1}')", absoluteUrlEdit, Server.UrlEncode(Request.Url.AbsolutePath), UGITUtility.newTicketTitle(ModuleName)));
                        //hlnk.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2}','100','90',0,'{1}')", absoluteUrlEdit, Server.UrlEncode(Request.Url.AbsolutePath), uHelper.newTicketTitle(ModuleName)));

                        Context.Cache.Remove(string.Format("CompanyId-{0}", ApplicationContext.CurrentUser.Id));
                        Context.Cache.Add(string.Format("CompanyId-{0}", ApplicationContext.CurrentUser.Id), string.Format("compID={0}", value), null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
                LoadDataOnDemand(0, ddlList, value, string.Empty);
                SetValue(hndValue.Value);
            }
        }

        protected void ddlList_ItemsRequestedByFilterCondition(object source, DevExpress.Web.ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //if (ControlMode == SPControlMode.New || IsPostBack || selectedId > 0)
            if (ControlMode == ControlMode.New || IsPostBack || !string.IsNullOrEmpty(selectedId))
            {
                ASPxTokenBox comboBox = (ASPxTokenBox)source;
                int startIndex = comboBox.Items.Count > e.EndIndex ? e.EndIndex + 1 : 0;
                LoadDataOnDemand(startIndex, comboBox, selectedId, e.Filter);
                SetValue(Value); // It is used to show prefilled values for new ticket or project
                if (ControlMode == ControlMode.Display)
                {
                    pnlContact.Style.Add("display", "none");
                    hlnk.Text = ddlList.Text;
                    hlnk.Visible = true;
                }
            }
        }

        //private void LoadDataOnDemand(int startIndex, ASPxComboBox comboBox, string value, string filter)
        private void LoadDataOnDemand(int startIndex, ASPxTokenBox comboBox, string value, string filter)
        {
            //try
            //{
            DataTable dtResults = null;
            string displayColumn = string.Empty;

            if (FieldName == DatabaseObjects.Columns.TagMultiLookup)
            {
                displayColumn = DatabaseObjects.Columns.Title;
                dtResults = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ExperiencedTags, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
            }
            else
            {
                if (string.IsNullOrEmpty(ModuleName))
                {
                    if (ControlList == DatabaseObjects.Tables.CRMContact)
                    {
                        ModuleName = "CON";
                        // imgAdd.Visible = false;
                    }
                    else
                    {
                        ModuleName = "COM";
                        // imgAdd.Visible = true;
                    }
                }

                displayColumn = DatabaseObjects.Columns.Title;

                //dtResults = TicketManager.GetOpenTickets(uHelper.getModuleIdByModuleName(ApplicationContext,ModuleName));
                dtResults = TicketManager.GetOpenTickets(ModuleViewManager.GetByName(ModuleName));


                //imgAdd.Visible = true;

                // DataRow selectRow = null;
                if (dtResults != null)
                {
                    // due to crash put it here.
                    //selectRow = dtResults.AsEnumerable().FirstOrDefault(dr => dr.Field<Int64>(DatabaseObjects.Columns.Id) ==Convert.ToInt64(value));//check this out
                    // selectRow = dtResults.Select($"ID in ({value})")[0];

                    DataRow[] drs = null;
                    if (FieldName == DatabaseObjects.Columns.ContactLookup || FieldName == DatabaseObjects.Columns.BrokerContact || FieldName == DatabaseObjects.Columns.EngineerContact || FieldName == DatabaseObjects.Columns.ConstructionManagerContact || FieldName == DatabaseObjects.Columns.Architect || FieldName == DatabaseObjects.Columns.BuildingOwner)
                    {
                        string keyPrefix = string.Empty;

                        if (FieldName == DatabaseObjects.Columns.BrokerContact)
                        {
                            keyPrefix = "Broker";
                        }
                        else if (FieldName == DatabaseObjects.Columns.EngineerContact)
                        {
                            keyPrefix = "Engineer";
                        }
                        else if (FieldName == DatabaseObjects.Columns.BuildingOwner)
                        {
                            keyPrefix = "BuildingOwner";
                        }
                        else if (FieldName == DatabaseObjects.Columns.ConstructionManagerContact)
                        {
                            keyPrefix = "ConstructionManagement";
                        }
                        else if (FieldName == DatabaseObjects.Columns.Architect)
                        {
                            keyPrefix = "Architect";
                        }

                        string compID = string.Empty;
                        object cacheVal = Context.Cache.Get(string.Format("{1}CompanyId-{0}", ApplicationContext.CurrentUser.Id, keyPrefix));
                        if (cacheVal != null)
                        {
                            comboBox.Items.Clear();
                            Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                            if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                            {
                                compID = cacheParams["compid"];
                            }
                        }

                        if (!string.IsNullOrEmpty(TicketId) && uHelper.getModuleNameByTicketId(TicketId) == "COM")
                        {
                            //var spListItem = Ticket.GetCurrentTicket(ApplicationContext,"COM", TicketId, new List<string>() { DatabaseObjects.Columns.Title }, true);
                            var spListItem = Ticket.GetCurrentTicket(ApplicationContext, "COM", TicketId);
                            string company = Convert.ToString(spListItem[DatabaseObjects.Columns.Title]);
                            company = company.Replace("'", "''");
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyLookup, spListItem[DatabaseObjects.Columns.TicketId])).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(DatabaseObjects.Columns.CRMCompanyLookup) && dr.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup) == company.Trim() && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else if (!string.IsNullOrEmpty(compID))
                        {
                            //var viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Title, compID);
                            //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CRMCompany, Convert.ToInt32(compID), SPContext.Current.Web, string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title));
                            //var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.Id}={compID} and TenantID='{ApplicationContext.TenantID}'").Select()[0];//Check selectedid

                            //if (spListItem != null)
                            //{
                            //    string company = Convert.ToString(spListItem[DatabaseObjects.Columns.Title]);
                            //    company = company.Replace("'", "''");
                            //    if (string.IsNullOrEmpty(filter))
                            //    {
                            //        //drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, company.Trim())).ToArray();
                            //        drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, compID)).ToArray();
                            //    }
                            //    else
                            //    {
                            //        drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(DatabaseObjects.Columns.CRMCompanyTitleLookup) && dr.Field<Int64>(DatabaseObjects.Columns.CRMCompanyTitleLookup) == Convert.ToInt64(compID) && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        string relationshipTypeColumn = DatabaseObjects.Columns.RelationshipType;
                        if (dtResults.Columns.Contains(DatabaseObjects.Columns.RelationshipTypeLookup))
                            relationshipTypeColumn = DatabaseObjects.Columns.RelationshipTypeLookup;

                        if (FieldName == DatabaseObjects.Columns.CompanyArchitect)
                        {
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains(DatabaseObjects.Columns.Architect)).Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains(DatabaseObjects.Columns.Architect) && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else if (FieldName == DatabaseObjects.Columns.CompanyEngineer)
                        {
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Engineer")).Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Engineer") && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else if (FieldName == DatabaseObjects.Columns.ConstructionManagementCompany)
                        {
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​")).Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​") && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else if (FieldName == "BuildingOwnerCompany")
                        {
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.Select(string.Format("{0} LIKE '%Building Rep%'", relationshipTypeColumn)).Skip(startIndex).Take(rowCount).ToArray();
                                //drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​")).Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.Select(string.Format("{0} LIKE '%Building Rep%' AND Title Like '%" + filter + "%'", relationshipTypeColumn));
                                //drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​") && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else if (FieldName == "Broker")
                        {
                            if (string.IsNullOrEmpty(filter))
                            {
                                drs = dtResults.Select(string.Format("{0} LIKE '%Broker%'", relationshipTypeColumn)).Skip(startIndex).Take(rowCount).ToArray();
                                //drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​")).Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            }
                            else
                            {
                                drs = dtResults.Select(string.Format("{0} LIKE '%Broker%' AND Title Like '%" + filter + "%'", relationshipTypeColumn));
                                //drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(relationshipTypeColumn) && dr.Field<string>(relationshipTypeColumn).Contains("Construction Manager​") && !dr.IsNull(displayColumn) && dr.Field<string>(displayColumn).ToLower().Contains(filter.ToLower())).ToArray();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(filter))
                                drs = dtResults.Rows.Cast<System.Data.DataRow>().Skip(startIndex).Take(rowCount).ToArray();
                            //drs = dtResults.Rows.Cast<System.Data.DataRow>().ToArray();
                            else
                                drs = dtResults.AsEnumerable().Where(dr => !dr.IsNull(displayColumn) && dr.Field<string>(DatabaseObjects.Columns.Title).ToLower().Contains(filter.ToLower())).ToArray();
                        }
                    }

                    if (drs != null && drs.Length > 0)
                    {                                  //   DataRow[] row = dtResults.Select("ID = " + value);
                        if (!string.IsNullOrEmpty(value))
                        {
                            var row = dtResults.Select($"{DatabaseObjects.Columns.TicketId} in ('{value}')");
                            dtResults = drs.CopyToDataTable();

                            if (row.Count() > 0 && dtResults.Select($"{DatabaseObjects.Columns.TicketId} in ('{value}')").Count() == 0)
                                dtResults.Merge(row.CopyToDataTable());
                        }
                        else
                            dtResults = drs.CopyToDataTable();
                    }
                }

                if (FieldName == DatabaseObjects.Columns.LeadSource)
                    if (string.IsNullOrEmpty(value) || value == "-1")
                        mailTo.Visible = false;


                //if ((ModuleName == "COM" || FieldName == DatabaseObjects.Columns.LeadSource) && dtResults != null && selectRow != null)
                //{
                //    if (value > 0 && dtResults.AsEnumerable().Where(dr => dr.Field<int>(DatabaseObjects.Columns.Id) == value).ToArray().Length == 0)
                //    {
                //        dtResults.Rows.Add(selectRow.ItemArray);
                //    }
                //}
            }
            if (dtResults != null && dtResults.Rows.Count > 0)
            {
                dtResults.DefaultView.Sort = displayColumn + " ASC";
                dtResults = dtResults.DefaultView.ToTable();

                dtResults = dtResults.AsEnumerable().Any(dr => !string.IsNullOrWhiteSpace(dr.Field<string>(displayColumn)))
                    ? dtResults.AsEnumerable().Where(dr => !string.IsNullOrWhiteSpace(dr.Field<string>(displayColumn))).CopyToDataTable()
                    : null;

                comboBox.TextField = displayColumn;
                //comboBox.ValueField = DatabaseObjects.Columns.Id;
                if (FieldName == DatabaseObjects.Columns.TagMultiLookup)
                {
                    comboBox.ValueField = DatabaseObjects.Columns.ID;
                }
                else
                {
                    comboBox.ValueField = DatabaseObjects.Columns.TicketId;
                }
                //comboBox.ValueField = DatabaseObjects.Columns.TicketId;
                comboBox.DataSource = dtResults;
                comboBox.DataBind();
            }
            //comboBox.TextField = displayColumn;
            //comboBox.ValueField = DatabaseObjects.Columns.Id;
            //comboBox.DataSource = dtResults;
            //comboBox.DataBind();

            //}
            //catch (Exception ex)
            //{
            //  Log.WriteException(ex, "LoadDataOnDemand(int startIndex, ASPxComboBox comboBox, int value, string filter)");
            //}
        }

        private DataTable LoadData(int value)
        {
            DataTable dtResults = null;
            if (string.IsNullOrEmpty(ModuleName))
            {
                if (ControlList == DatabaseObjects.Tables.CRMContact)
                {
                    ModuleName = "CON";
                }
                else
                {
                    ModuleName = "COM";
                }
            }

            // dtResults = TicketManager.GetOpenTickets(uHelper.getModuleIdByModuleName(ApplicationContext,ModuleName));
            dtResults = TicketManager.GetOpenTickets(ModuleViewManager.GetByName(ModuleName));
            if (dtResults != null && dtResults.Rows.Count > 0)
            {
                //dtResults = mStatistics.ResultedData;

                string compID = string.Empty;
                DataRow[] drs = null;

                if (FieldName == "ProposalRecipient" || FieldName == "AdditionalRecipients")
                {
                    //string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/><FieldRef Name='{6}'/><FieldRef Name='{7}'/><FieldRef Name='{8}'/>",
                    //  DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CRMCompanyTitleLookup, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CustomProperties,
                    //  DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ContactLookup, DatabaseObjects.Columns.RelationshipTypeLookup, DatabaseObjects.Columns.CostCodeLookup, DatabaseObjects.Columns.ItemOrder);

                    //SPQuery spQuery = new SPQuery();
                    //var Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>",
                    //                                        DatabaseObjects.Columns.TicketId, TicketId);
                    var Query = $"{DatabaseObjects.Columns.TicketId}='{TicketId}'";

                    //  Query = viewFields;
                    //spQuery.ViewFieldsOnly = true;

                    //SPListItemCollection collection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.RelatedCompanies, Query);
                    var collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, Query);

                    List<string> ids = new List<string>();
                    if (collection != null && collection.Rows.Count > 0)
                    {
                        foreach (DataTable spListItem in collection.Rows)
                        {
                            //SPFieldLookupValueCollection values = new SPFieldLookupValueCollection(Convert.ToString(spListItem[DatabaseObjects.Columns.ContactLookup]));//Need to convert code @Chetan
                            var values = new List<string>();
                            // values = Convert.ToString(spListItem[DatabaseObjects.Columns.ContactLookup]);
                            foreach (var val in values)
                            {
                                //if (val != null && val.LookupId > 0)
                                //    ids.Add(Convert.ToString(val.LookupId));
                            }
                        }
                    }

                    if (ids != null && ids.Count > 0)
                    {
                        ids = ids.Where(str => !String.IsNullOrEmpty(str)).Distinct().ToList();

                        string strIds = string.Join(",", ids.Select(x => string.Format("'{0}'", x)).ToArray());
                        try
                        {
                            drs = dtResults.Select(string.Format("{0} In ({1})", DatabaseObjects.Columns.Id, strIds)).ToArray();
                        }
                        catch (Exception ex) { ULog.WriteException(ex); }
                    }

                    if (drs != null && drs.Length > 0)
                    {
                        DataView dView1 = drs.CopyToDataTable().AsDataView();
                        dView1.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                        return dView1.ToTable();
                    }

                    return null;
                }
                else if (FieldName == "ProjectTeamLookup")
                {
                    object cacheVal = Context.Cache.Get(string.Format("CompanyId-{0}", ApplicationContext.CurrentUser.Id));
                    if (cacheVal != null)
                    {
                        Context.Cache.Remove(string.Format("CompanyIdForMult-{0}", ApplicationContext.CurrentUser.Id));
                        Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                        if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                        {
                            compID = cacheParams["compid"];
                        }
                    }

                    if (!string.IsNullOrEmpty(compID))
                    {
                        var viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Id);
                        //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CRMCompany, Convert.ToInt32(compID), SPContext.Current.Web, string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title));
                        var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'").Select()[0];  //ask about ID @ Chetan

                        if (spListItem != null)
                        {
                            string company = Convert.ToString(spListItem[DatabaseObjects.Columns.Title]);
                            company = company.Replace("'", "''");
                            //drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, company)).ToArray();
                            drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyLookup, company)).ToArray();
                        }
                    }

                    if (drs != null && drs.Length > 0)
                    {
                        DataView dView1 = drs.CopyToDataTable().AsDataView();
                        dView1.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                        return dView1.ToTable();
                    }
                    else
                        return null;

                }
                else if (FieldName == DatabaseObjects.Columns.ContactLookup || FieldName == DatabaseObjects.Columns.BrokerContact || FieldName == DatabaseObjects.Columns.EngineerContact || FieldName == DatabaseObjects.Columns.ConstructionManagerContact || FieldName == DatabaseObjects.Columns.Architect || FieldName == DatabaseObjects.Columns.BuildingOwner)
                {
                    string keyPrefix = string.Empty;

                    if (FieldName == DatabaseObjects.Columns.BrokerContact)
                    {
                        keyPrefix = "Broker";
                    }
                    else if (FieldName == DatabaseObjects.Columns.EngineerContact)
                    {
                        keyPrefix = "Engineer";
                    }
                    else if (FieldName == DatabaseObjects.Columns.BuildingOwner)
                    {
                        keyPrefix = "BuildingOwner";
                    }
                    else if (FieldName == DatabaseObjects.Columns.ConstructionManagerContact)
                    {
                        keyPrefix = "ConstructionManagement";
                    }
                    else if (FieldName == DatabaseObjects.Columns.Architect)
                    {
                        keyPrefix = "Architect";
                    }
                    //else if (FieldName == DatabaseObjects.Columns.ContactLookup)
                    //{
                    //    keyPrefix = "MCompany";
                    //}

                    string cmpID = string.Empty;
                    object cacheVal = Context.Cache.Get(string.Format("{1}CompanyId-{0}", ApplicationContext.CurrentUser.Id, keyPrefix));
                    if (cacheVal != null)
                    {
                        //comboBox.Items.Clear();
                        //Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                        Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "Ʃ");
                        if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                        {
                            compID = cacheParams["compid"];
                            compID = compID.Replace("'", "''");
                            //long id = 0;
                            //if (!long.TryParse(compID, out id))
                            if (!string.IsNullOrEmpty(compID) && !compID.Contains("COM-"))
                            {
                                DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.Title} = '{compID}' and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'");

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    compID = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.TicketId]);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(TicketId) && uHelper.getModuleNameByTicketId(TicketId) == "COM")
                    {
                        // var spListItem = Ticket.GetCurrentTicket(ApplicationContext,"COM", TicketId, new List<string>() { DatabaseObjects.Columns.Title }, true);
                        var spListItem = Ticket.GetCurrentTicket(ApplicationContext, "COM", TicketId);
                        string company = Convert.ToString(spListItem[DatabaseObjects.Columns.Title]);
                        company = company.Replace("'", "''");
                        //drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, spListItem[DatabaseObjects.Columns.Id])).Take(rowCount).ToArray();
                        drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyLookup, spListItem[DatabaseObjects.Columns.TicketId])).Take(rowCount).ToArray();

                    }
                    else if (!string.IsNullOrEmpty(compID))
                    {
                        //var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.Id}={Convert.ToInt64(compID)}").Select()[0];//Pass id colun
                        //var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.TicketId}='{compID}' and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'").Select()[0];
                        var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.TicketId}='{compID}' and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                        if (spListItem != null && spListItem.Rows.Count > 0)
                        {
                            string company = Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Title]);
                            company = company.Replace("'", "''");
                            drs = dtResults.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyLookup, compID)).ToArray();
                        }
                    }
                    if (drs != null && drs.Length > 0)
                    {
                        DataView dView1 = drs.CopyToDataTable().AsDataView();
                        dView1.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                        dtResults = dView1.ToTable();
                    }
                    else
                    {
                        dtResults = null;
                    }
                }
            }

            if (dtResults != null && dtResults.Rows.Count > 0)
            {
                if (Value == "<Value Varies>")
                {
                    DataRow dr = dtResults.NewRow();
                    dr[DatabaseObjects.Columns.Id] = -1;
                    dr[DatabaseObjects.Columns.Title] = Value;

                    dtResults.Rows.Add(dr);
                }
                DataView dView = dtResults.AsDataView();
                dView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                dtResults = dView.ToTable();
            }
            else
            {
                if (Value == "<Value Varies>")
                {
                    dtResults = new DataTable();
                    dtResults.Columns.Add(DatabaseObjects.Columns.Id);
                    dtResults.Columns.Add(DatabaseObjects.Columns.Title);
                    dtResults.Rows.Add(new object[] { -1, Value });
                }
            }

            return dtResults;
        }

        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ddlList.ClientSideEvents.SelectedIndexChanged = "function ddlList_SelectedIndexChanged(s, e) { debugger;if(s.GetValue() != s.GetText()){   if(typeof(CompanyContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  CompanyContact.PerformCallback('CompanyTitle');} if(typeof(ProjectTeamContact) !='undefined'){ LoadingPanel.SetText('Loading Contacts ...');LoadingPanel.Show();  ProjectTeamContact.PerformCallback('CompanyTitle1');}}}";
        }

        protected void ddlList_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
        }

        protected void GridLookup_DataBinding(object sender, EventArgs e)
        {
            GridLookup.DataSource = LoadData(0);
            if (!string.IsNullOrEmpty(Value))
            {
                SetValue(Value);
            }
        }

        protected void ddlList_Callback(object sender, CallbackEventArgsBase e)
        {
            LoadDataOnDemand(0, ddlList, selectedId, string.Empty);
            SetValue(Value);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            /*
            object cacheVal = Context.Cache.Get(string.Format("CompanyId-{0}",ApplicationContext.CurrentUser.Id));

            if (cacheVal != null)
            {
                Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("compid"))
                {
                    long Id = Convert.ToInt64(cacheParams["compid"]);

                    // For displaying Market sector based on company change
                    //SPList lstCRMCompany = SPListHelper.GetSPList(DatabaseObjects.Tables.CRMCompany);
                   // var lstCRMCompany = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.TenantID}={ApplicationContext.TenantID}");

                    //string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketRequestTypeCategory);
                    //SPListItem spListItem = SPListHelper.GetSPListItem(lstCRMCompany, Id, viewFields, true);
                    var spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, $"{DatabaseObjects.Columns.Id}={Id} and TenantID='{ApplicationContext.TenantID}'");  //ask about ID @ Chetan
                    if (spListItem != null)
                    {
                        // string marketSector = uHelper.GetLookupValue(spListItem, DatabaseObjects.Columns.TicketRequestTypeCategory);
                        string marketSector = string.Empty;
                        if (ASPxCallbackPanel1.JSProperties.ContainsKey("cpMarketSector"))
                        {
                            ASPxCallbackPanel1.JSProperties["cpMarketSector"] = marketSector;
                        }
                        else
                        {
                            ASPxCallbackPanel1.JSProperties.Add("cpMarketSector", marketSector);
                        }
                    }
                }
            }
            */
            GridLookup.DataBind();
        }

        protected void ddlList_CustomFiltering(object source, ListEditCustomFilteringEventArgs e)
        {
            //if (ControlMode == SPControlMode.New || IsPostBack || selectedId > 0)
            if (ControlMode == ControlMode.New || IsPostBack || !string.IsNullOrEmpty(selectedId))
            {
                ASPxTokenBox comboBox = (ASPxTokenBox)source;
                //int startIndex = comboBox.Items.Count > e.EndIndex ? e.EndIndex + 1 : 0;
                //int startIndex = 0;
                //LoadDataOnDemand(startIndex, comboBox, selectedId, e.Filter);
                UGITUtility.CreateCookie(Response, FieldName, e.Filter);
                SetValue(Value); // It is used to show prefilled values for new ticket or project
                if (ControlMode == ControlMode.Display)
                {
                    pnlContact.Style.Add("display", "none");
                    hlnk.Text = ddlList.Text;
                    hlnk.Visible = true;
                }
            }

        }
    }
}