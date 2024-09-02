using DevExpress.Web;
using DevExpress.Web.Rendering;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Spreadsheet;
using System.IO;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ApplModuleRoleMappingCtrl : UserControl
    {
        public UserProfileManager ObjUserManager;
        public string ddlViewTypeValue
        {
            get
            {
                if (Session["ddlViewTypeValue"] != null)
                    return Convert.ToString(Session["ddlViewTypeValue"]);
                else
                    return "";
            }

            set
            {
                Session["ddlViewTypeValue"] = value;
            }
        }
        public string grdvUserListSelection
        {
            get
            {
                if (Session["grdvUserListSelection"] != null)
                    return Convert.ToString( Session["grdvUserListSelection"]);
                else
                {
                    return "";
                }
            }
            set
            {
                Session["grdvUserListSelection"] = value;
            }
        }
        
        public string grdvModuleListSelection
        {
            get
            {
                if (Session["grdvModuleListSelection"] != null)
                    return Convert.ToString(Session["grdvModuleListSelection"]);
                else
                {
                    return "";
                }
            }
            set
            {
                Session["grdvModuleListSelection"] = value;
            }
        }
        public int ApplicationId { get; set; }
        public string TicketId { get; set; }
        List<ApplicationAccess> spListItemColl = null;
        DataTable dt = new DataTable();
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ItemID={1}&AppId={2}&Mode={3}&UserId={4}";
        string addNewItem = string.Empty;
        public bool isUserExists { get; set; }
        DataRow spItem = null;
        string selectedUserId;
        string selectedModuleId;
        //Import Variable
        private string absoluteUrlImport = "/layouts/ugovernit/DelegateControl.aspx?control=importexcelfile&listName=ApplModuleRoleRelationship";
        protected string importUrl;
        ApplicationAccessManager appAccessManager;
        UserProfile User;
        ConfigurationVariableManager configManager;
        ApplicationContext _context;
        ApplicationModuleManager applicationModuleManager;
        ApplicationRoleManager applicationRoleManager;
        protected override void OnInit(EventArgs e)
        {
            #region default initialization
            _context = HttpContext.Current.GetManagerContext();
            applicationModuleManager = new ApplicationModuleManager(_context);
            applicationRoleManager = new ApplicationRoleManager(_context);
            ObjUserManager = new UserProfileManager(_context);
            appAccessManager = new ApplicationAccessManager(_context);
            User = HttpContext.Current.CurrentUser();
            configManager = new ConfigurationVariableManager(_context);
            #endregion
            spItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, ApplicationId)[0]; 
            isUserExists = Ticket.IsActionUserNew(HttpContext.Current.GetManagerContext(), spItem, User);
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulerolemapedit", "0", ApplicationId, "ADD", ""));
            string url = string.Format("window.parent.UgitOpenPopupDialog('{0}','','Add Permissions','90','90',0,'{1}')", addNewItem, "/?xyz");
            aAddNew.ClientSideEvents.Click = "function(){ " + url + " }"; //used /?xyz to refresh parent tab

            string reportUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ApplicationId={1}&TicketId={2}", "applicationaccessreportviewer", ApplicationId, TicketId));
            lnkReport.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','1200','800',0,'{1}','true')", reportUrl, Server.UrlEncode(Request.Url.AbsolutePath), "Application Permission Report"));
            aAddNew.Visible = isUserExists;

            CreateDataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtUsers = dt.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ApplicationRoleAssign, "UserId" });
                dtUsers.DefaultView.Sort = DatabaseObjects.Columns.ApplicationRoleAssign + " ASC";
                grdvUserList.DataSource = dtUsers.DefaultView;
                grdvUserList.DataBind();
                DataTable dtModules = dt.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ApplicationModulesLookup, "ModuleId" });
                dtModules.DefaultView.Sort = DatabaseObjects.Columns.ApplicationModulesLookup + " ASC";
                grdvModuleList.DataSource = dtModules.DefaultView;
                grdvModuleList.DataBind();
            }

           
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            importUrl = absoluteUrlImport = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport));
            onlyExcelImport.Visible = false;
            if (configManager.GetValueAsBool(ConfigConstants.EnableAPPAccessImport))
                onlyExcelImport.Visible = true;

            if (!IsPostBack)
            {
                if(!string.IsNullOrEmpty(ddlViewTypeValue))
                   ddlViewType.SelectedValue = ddlViewTypeValue;

                if (!string.IsNullOrEmpty(grdvUserListSelection))
                    grdvUserList.Selection.SetSelectionByKey(grdvUserListSelection, true);
                else
                {
                    if (grdvUserList.DataSource != null)
                    {
                        grdvUserList.Selection.SetSelection(0, true);
                    }
                }

                if (!string.IsNullOrEmpty(grdvModuleListSelection))
                    grdvModuleList.Selection.SetSelectionByKey(grdvModuleListSelection, true);
                else
                {
                    if (grdvModuleList.DataSource != null)
                    {
                        grdvModuleList.Selection.SetSelection(0, true);
                    }
                }
            }

            if (HttpContext.Current.Request.Form["__CALLBACKPARAM"] != null)
            {
                if (HttpContext.Current.Request.Form["__CALLBACKPARAM"].ToString().Contains("SelectedModuleId"))
                {
                    string[] val = HttpContext.Current.Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    selectedModuleId = val[val.Length - 1].Replace(";", string.Empty);
                    if (!string.IsNullOrEmpty(selectedModuleId))
                        BindGrid(selectedModuleId);
                }
                else if (HttpContext.Current.Request.Form["__CALLBACKPARAM"].ToString().Contains("SelectedUserId"))
                {
                    string[] val = HttpContext.Current.Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    selectedUserId = val[val.Length - 1].Replace(";", string.Empty);
                    if (!string.IsNullOrEmpty(selectedUserId))
                        BindApplModuleRoleMapByUserGrid(selectedUserId);
                }
                else
                {
                    if (grdvUserList.DataSource != null && grdvUserList.Selection!=null && grdvUserList.Selection.Count>0)
                    {
                        selectedUserId = Convert.ToString(grdvUserList.GetSelectedFieldValues("UserId")[0]);
                        if (!string.IsNullOrEmpty(selectedUserId))
                            BindApplModuleRoleMapByUserGrid(selectedUserId);
                    }
                    else if (grdvModuleList.DataSource != null && grdvModuleList.Selection != null && grdvModuleList.Selection.Count > 0)
                    {
                        selectedModuleId = Convert.ToString(grdvModuleList.GetSelectedFieldValues("ModuleId")[0]);
                        if (!string.IsNullOrEmpty(selectedModuleId))
                            BindGrid(selectedModuleId);
                    }
                }
            }
            else
            {
                if (grdvUserList.DataSource != null && grdvUserList.Selection != null && grdvUserList.Selection.Count > 0)
                {
                    selectedUserId = Convert.ToString(grdvUserList.GetSelectedFieldValues("UserId")[0]);
                    if (!string.IsNullOrEmpty(selectedUserId))
                        BindApplModuleRoleMapByUserGrid(selectedUserId);
                }
                if (grdvModuleList.DataSource != null && grdvModuleList.Selection != null && grdvModuleList.Selection.Count > 0)
                {
                    selectedModuleId = Convert.ToString(grdvModuleList.GetSelectedFieldValues("ModuleId")[0]);
                    if (!string.IsNullOrEmpty(selectedModuleId))
                        BindGrid(selectedModuleId);
                }
            }
           



            foreach (GridViewDataColumn column in gvApplModuleRoleMapByUser.Columns)
            {

                if (column.FieldName == DatabaseObjects.Columns.ApplicationModulesLookup) // column.Name.StartsWith("projecthealth") ||
                {
                    column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                {
                    column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

        private void BindGrid(string moduleId)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drRows = dt.Select(string.Format("{0}='{1}'", "ModuleId", moduleId));
                if (drRows.Length > 0)
                {
                    DataTable dtRow = drRows.CopyToDataTable();
                    dtRow.DefaultView.Sort = DatabaseObjects.Columns.ApplicationRoleAssign + " ASC";
                    grdApplModuleRoleMap.DataSource = dtRow.DefaultView;
                    grdApplModuleRoleMap.DataBind();
                }
                
            }
            else
            {
                grdApplModuleRoleMap.DataSource = dt;
            }
        }

        private void BindApplModuleRoleMapByUserGrid(string userId)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drRows = dt.Select(string.Format("{0}='{1}'", "UserId", userId));
                if (drRows.Length > 0)
                {
                    DataTable dtRow = drRows.CopyToDataTable();
                    dtRow.DefaultView.Sort = DatabaseObjects.Columns.ApplicationModulesLookup + " ASC";
                    gvApplModuleRoleMapByUser.DataSource = dtRow.DefaultView;
                    gvApplModuleRoleMapByUser.DataBind();
                }


            }
            else
            {
                gvApplModuleRoleMapByUser.DataSource = dt;
            }
        }


        private void CreateDataTable()
        {
            
            spListItemColl = appAccessManager.Load(x => x.APPTitleLookup == ApplicationId).ToList();
            string query = string.Format("{0}='{1}' and {2}={3}", DatabaseObjects.Columns.TenantID, _context.TenantID, DatabaseObjects.Columns.ID, ApplicationId);
            DataTable appRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, query);
            if (spListItemColl != null && spListItemColl.Count > 0)
            {
                #region reduce multiple hit to get common data
                List<long?> mappedAppModule = spListItemColl.Select(x => x.ApplicationModulesLookup).Distinct().ToList();
                List<long?> mappedAppRoles = spListItemColl.Select(x => x.ApplicationRoleLookup).Distinct().ToList();
                List<string> lstOfRoleAssign = spListItemColl.Select(x => x.ApplicationRoleAssign).Distinct().ToList();
                List<ApplicationModule> lstOfFilterModules = applicationModuleManager.Load(x => mappedAppModule.Any(y => x.ID == y));
                List<ApplicationRole> lstOfFilterRoles = applicationRoleManager.Load(x => mappedAppRoles.Any(y => x.ID == y));
                #endregion

                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.Id));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.ApplicationModulesLookup));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.ApplicationRoleAssign));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.ApplicationRoleLookup));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.APPTitleLookup));
                dt.Columns.Add(new DataColumn("ModuleId"));
                dt.Columns.Add(new DataColumn("UserId"));
                dt.Columns.Add(new DataColumn("ApplicationId"));
                dt.Columns.Add(new DataColumn("RoleId"));
                ApplicationRole applicationRole = null;
                ApplicationModule applicationModule = null;
                List<UserProfile> lstOfFilterUser = new List<UserProfile>();
                if (lstOfRoleAssign != null && lstOfRoleAssign.Count > 0)
                {
                    lstOfFilterUser= ObjUserManager.Load(x => lstOfRoleAssign.Contains(x.Id));
                }

                UserProfile userProfile = null;
                foreach (ApplicationAccess item in spListItemColl)
                {
                    bool isNew = true;

                    string moduleLookup = Convert.ToString(item.ApplicationModulesLookup);
                    string appTitleLookup = Convert.ToString(item.APPTitleLookup);
                    string roleLookup = Convert.ToString(item.ApplicationRoleLookup);
                    applicationRole=lstOfFilterRoles.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(roleLookup));
                    string roleAssigneeLookup = Convert.ToString(item.ApplicationRoleAssign);
                    userProfile = lstOfFilterUser.FirstOrDefault(x => x.Id == roleAssigneeLookup);
                    DataRow drSelected = null;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] drSelectedRows = dt.Select(string.Format("{0}='{1}' AND {2}='{3}'",
                                                                            "ModuleId", moduleLookup.ToString(),
                                                                            "UserId", roleAssigneeLookup.ToString()));
                        if (drSelectedRows != null && drSelectedRows.Length > 0)
                        {
                            drSelected = drSelectedRows[0];
                            isNew = false;
                            applicationRole = lstOfFilterRoles.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(roleLookup));
                            if(applicationRole!=null)
                            drSelected[DatabaseObjects.Columns.ApplicationRoleLookup] = string.Format("{0}, {1}", drSelected[DatabaseObjects.Columns.ApplicationRoleLookup], applicationRole.Title);
                        }
                    }
                    if (isNew)
                    {
                        drSelected = dt.NewRow();
                        if (string.IsNullOrEmpty(moduleLookup))
                        {
                            if (appRow != null && appRow.Rows.Count > 0)
                            {
                                drSelected[DatabaseObjects.Columns.ApplicationModulesLookup] = UGITUtility.ObjectToString(appRow.Rows[0][DatabaseObjects.Columns.Title]);// "(No Modules)";
                            }
                        }
                        else
                        {
                            applicationModule = lstOfFilterModules.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(moduleLookup));
                            if (applicationModule != null)
                            {
                                drSelected[DatabaseObjects.Columns.ApplicationModulesLookup] = applicationModule.Title;
                                drSelected["ModuleId"] = applicationModule.ID;
                            }
                        }

                        drSelected["ApplicationId"] = appTitleLookup;
                        drSelected[DatabaseObjects.Columns.APPTitleLookup] = appTitleLookup;
                        drSelected[DatabaseObjects.Columns.ApplicationRoleAssign] = roleAssigneeLookup;
                        if (userProfile != null)
                            drSelected[DatabaseObjects.Columns.ApplicationRoleAssign] = userProfile.Name;
                        if (applicationRole != null)
                            drSelected[DatabaseObjects.Columns.ApplicationRoleLookup] = applicationRole.Title;
                        drSelected[DatabaseObjects.Columns.Id] = item.ID;
                        drSelected["ModuleId"] = moduleLookup;
                        drSelected["UserId"] = roleAssigneeLookup;
                        drSelected["RoleId"] = roleLookup;
                        dt.Rows.Add(drSelected);
                    }
                }
                dt.AcceptChanges();
            }
        }

        protected void ddlViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlViewTypeValue = ddlViewType.SelectedItem.Value;
            if (dt == null || dt.Rows.Count == 0)
            {
                CreateDataTable();
            }
            if (ddlViewType.SelectedItem.Value == "bymodule")
            {
                if (grdvModuleList.DataSource != null)
                {
                    grdvModuleList.Selection.SetSelection(0, true);
                    selectedModuleId = Convert.ToString(grdvModuleList.GetSelectedFieldValues("ModuleId")[0]);
                    if (!string.IsNullOrEmpty(selectedModuleId))
                        BindGrid(selectedModuleId);
                }
            }
            else if (ddlViewType.SelectedItem.Value == "byuser")
            {
                if (grdvUserList.DataSource != null)
                {
                    grdvUserList.Selection.SetSelection(0, true);
                    selectedUserId = Convert.ToString(grdvUserList.GetSelectedFieldValues("UserId")[0]);
                    if (!string.IsNullOrEmpty(selectedUserId))
                        BindApplModuleRoleMapByUserGrid(selectedUserId);
                }
            }
        }

        protected void grdApplModuleRoleMap_HtmlDataCellPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            UserProfileManager userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string editItem;
                ASPxHyperLink anchorEdit = grdApplModuleRoleMap.FindRowCellTemplateControl(e.VisibleIndex, null, "aRoleAssignee") as ASPxHyperLink;
                if(!string.IsNullOrEmpty(lsDataKeyValue))
                    anchorEdit.Text = userManager.GetUserInfoById(lsDataKeyValue).UserName;
                string Title = anchorEdit.Text;  // Convert.ToString(e.GetValue(DatabaseObjects.Columns.ApplicationRoleAssign));

                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulerolemapedit", "0", ApplicationId, "EDIT", lsDataKeyValue));
                string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Edit permission for {2}','100','100',0,'{1}')", editItem, "/layouts/ugovernit/delegatecontrol", Title);//user /?pqr to refresh parent tab

                anchorEdit.Attributes.Add("href", jsFunc);
                ASPxImage imgedit = grdApplModuleRoleMap.FindRowCellTemplateControl(e.VisibleIndex, null, "Imgedit") as ASPxImage;
                imgedit.ClientSideEvents.Click = "function (s, e) {" + jsFunc + "}";
                imgedit.Visible = isUserExists;
            }
        }

        protected void gvApplModuleRoleMapByUser_HtmlDataCellPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Group)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string editItem;
                string Title = HttpContext.Current.GetUserManager().GetUserById(Convert.ToString(e.GetValue(DatabaseObjects.Columns.ApplicationRoleAssign))).UserName;
                editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulerolemapedit", "0", ApplicationId, "EDIT", lsDataKeyValue));
                string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Edit Permission for {2}','100','100',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
                ASPxHyperLink anchorEdit = gvApplModuleRoleMapByUser.FindGroupRowTemplateControl(e.VisibleIndex, "aRoleAssignee") as ASPxHyperLink;
                anchorEdit.Attributes.Add("href", jsFunc);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (ddlViewType.SelectedItem.Value == "bymodule")
            {
                grdApplModuleRoleMap.Visible = true;
                gvApplModuleRoleMapByUser.Visible = false;
                grdvUserList.Visible = false;
                grdvModuleList.Visible = true;
            }
            else if (ddlViewType.SelectedItem.Value == "byuser")
            {
                grdApplModuleRoleMap.Visible = false;
                gvApplModuleRoleMapByUser.Visible = true;
                grdvUserList.Visible = true;
                grdvModuleList.Visible = false;
            }

            base.OnPreRender(e);
        }

        protected void grdvUserList_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                string editItem;
                ASPxImage imgedit = grdvUserList.FindRowCellTemplateControl(e.VisibleIndex, null, "ImgEditUser") as ASPxImage;
                if(imgedit!=null)
                {
                    string Title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ApplicationRoleAssign));
                    editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "modulerolemapedit", "0", ApplicationId, "EDIT", lsDataKeyValue));
                    string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Edit Permission for {2}','100','100',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
                    imgedit.ClientSideEvents.Click = "function (s, e) {" + jsFunc + "}";
                    imgedit.Visible = isUserExists;
                }
                 
                if (!string.IsNullOrWhiteSpace(lsDataKeyValue))
                {
                    //e.Row.Cells[1].ToolTip = lsDataKeyValue;
                    
                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", lsDataKeyValue));
                    e.Row.Cells[1].Text = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{2}\")'>{3}</a>",
                                                        userLinkUrl,
                                                        Convert.ToString(e.GetValue("ApplicationRoleAssignUser")).Replace("'", string.Empty),
                                                        Uri.EscapeDataString(Request.Url.PathAndQuery),
                                                        Convert.ToString(e.GetValue("ApplicationRoleAssignUser")));
                }
            }
        }

        protected void grdvUserList_SelectionChanged(object sender, EventArgs e)
        {
            List<object> obj = grdvUserList.GetSelectedFieldValues("UserId");
            if(obj != null)
                grdvUserListSelection = Convert.ToString(obj[0]);
        }

        protected void grdvModuleList_SelectionChanged(object sender, EventArgs e)
        {
            List<object> obj = grdvModuleList.GetSelectedFieldValues("ModuleId");
            if (obj != null)
                grdvModuleListSelection = Convert.ToString(obj[0]);
        }

        protected void btnExportAppAccessCtr_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void ExportToExcel()
        {
            //Tuple<string,string, string> first item1=Sheet name,item2=module name to get module columns,item3=list name
            List<Tuple<string, string, string>> lstSPLists = new List<Tuple<string, string, string>>() {
                new Tuple<string,string,string>("Modules",string.Empty,DatabaseObjects.Tables.ApplicationModules),
                new Tuple<string,string,string>("Roles",string.Empty,DatabaseObjects.Tables.ApplicationRole),
                new Tuple<string,string,string>("Users",string.Empty,DatabaseObjects.Tables.ApplModuleRoleRelationship)
            };

            DataTable table = null;

            List<Tuple<string, string>> lstCommonDisplayNames = new List<Tuple<string, string>>() {
                new Tuple<string, string>(DatabaseObjects.Columns.APPTitleLookup, "Application"),
                new Tuple<string, string>(DatabaseObjects.Columns.ApplicationModulesLookup, "Module"),
                new Tuple<string, string>(DatabaseObjects.Columns.ApplicationRoleLookup, "Role"),
                new Tuple<string, string>(DatabaseObjects.Columns.ApplicationRoleModuleLookup, "Module"),
            };

            int counter = 0;
          //  string queryExpress = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.APPTitleLookup, ApplicationId);
            string queryExpress = string.Format("{0}={1}", DatabaseObjects.Columns.APPTitleLookup, ApplicationId);

            string fileName = string.Empty;
            foreach (Tuple<string, string, string> tuple in lstSPLists)
            {
                ApplicationModuleManager moduleManager = new ApplicationModuleManager(HttpContext.Current.GetManagerContext());
                // SPList list = SPListHelper.GetSPList(tuple.Item3);//Get splist
                DataTable dt = GetTableDataManager.GetTableData(tuple.Item3,string.Format("{0}='{1}'",DatabaseObjects.Columns.TenantID, HttpContext.Current.GetManagerContext().TenantID));
                if (dt == null)
                    continue;
                List<DataColumn> spfieldList = new List<DataColumn>();
                List<ModuleColumn> moduleColumn = new List<ModuleColumn>();

                spfieldList = ExcelGenerateColumns(dt);

                table = ExportListToExcel.GetDataTableFromList(tuple.Item3, spfieldList, HttpContext.Current.GetManagerContext(), queryExpress);
                Worksheet worksheet;
                if (table == null || table.Rows.Count == 0)
                {
                    table = SheetWithDefaultSchema(table, tuple);
                    if (counter == 0)
                    {
                        worksheet = aspxSpreadSheetCtr.Document.Worksheets[0];
                        worksheet.Name = tuple.Item1;
                    }
                    else
                        worksheet = aspxSpreadSheetCtr.Document.Worksheets.Add(tuple.Item1);

                    worksheet.Import(table, true, 0, 0);
                    counter++;
                    continue;
                }

                if (string.IsNullOrEmpty(fileName) && table.Columns.Contains(DatabaseObjects.Columns.APPTitleLookup))
                    fileName = string.Format("{0} - Access", Convert.ToString(table.Rows[0][DatabaseObjects.Columns.APPTitleLookup]));

                //Sort by required cols
                DataView sortView = table.DefaultView;
                if (tuple.Item3 == DatabaseObjects.Tables.ApplicationModules || tuple.Item3 == DatabaseObjects.Tables.ApplicationRole)
                    sortView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                else
                    sortView.Sort = string.Format("{0} asc,{1} asc,{2} asc", DatabaseObjects.Columns.ApplicationModulesLookup, DatabaseObjects.Columns.ApplicationRoleLookup, DatabaseObjects.Columns.ApplicationRoleAssign);
                List<Tuple<string, string>> lstDisplayName = new List<Tuple<string, string>>();
                if (string.IsNullOrEmpty(tuple.Item2))
                    lstDisplayName = GetDisplayName(tuple.Item3);

                foreach (DataColumn col in table.Columns)
                {
                    DataColumn f = spfieldList.FirstOrDefault(x => x.ColumnName == col.ColumnName);

                    if (f != null && lstCommonDisplayNames.Any(x => x.Item1 == f.ColumnName))
                    {
                        Tuple<string, string> tup = lstCommonDisplayNames.FirstOrDefault(x => x.Item1 == f.ColumnName);
                        if (!uHelper.IfColumnExists(tup.Item2, table))
                            col.ColumnName = tup.Item2;

                        continue;
                    }

                    if (f != null && lstDisplayName.Any(x => x.Item1 == f.ColumnName))
                    {
                        Tuple<string, string> tup = lstDisplayName.FirstOrDefault(x => x.Item1 == f.ColumnName);
                        if (!uHelper.IfColumnExists(tup.Item2, table))
                            col.ColumnName = tup.Item2;

                        continue;
                    }

                    if (f != null && moduleColumn.Count > 0 && moduleColumn.Any(x => x.FieldName == col.ColumnName))
                    {
                        ModuleColumn mCol = moduleColumn.FirstOrDefault(x => x.FieldName == col.ColumnName);
                        table.AsEnumerable().ToList().ForEach(x => UpdateRowsForCustomization(x, f, modCol: mCol));

                        if (!uHelper.IfColumnExists(mCol.FieldDisplayName, table))
                            col.ColumnName = mCol.FieldDisplayName;
                        continue;
                    }
                    if (f == null)
                        continue;
                    table.AsEnumerable().ToList().ForEach(x => UpdateRowsForCustomization(x, f));

                    if (!uHelper.IfColumnExists(f.ColumnName, table))
                        col.ColumnName = f.ColumnName;
                }

                table.AcceptChanges();

                DataView dataView = table.DefaultView;
                if (tuple.Item3 == DatabaseObjects.Tables.ApplicationModules)
                {
                    if (table.Columns.Contains("Application"))
                        table.Columns["Application"].SetOrdinal(0);
                    if (table.Columns.Contains("Item Order"))
                        table.Columns["Item Order"].SetOrdinal(1);
                    if (table.Columns.Contains("Module"))
                        table.Columns["Module"].SetOrdinal(2);
                }
                else if (tuple.Item3 == DatabaseObjects.Tables.ApplicationRole)
                {
                    if (table.Columns.Contains("Application"))
                        table.Columns["Application"].SetOrdinal(0);
                    if (table.Columns.Contains("Item Order"))
                        table.Columns["Item Order"].SetOrdinal(1);
                    if (table.Columns.Contains("Module"))
                        table.Columns["Module"].SetOrdinal(2);
                    if (table.Columns.Contains("Role"))
                        table.Columns["Role"].SetOrdinal(3);
                }
                else if (tuple.Item3 == DatabaseObjects.Tables.ApplModuleRoleRelationship)
                {
                    if (table.Columns.Contains("Application"))
                        table.Columns["Application"].SetOrdinal(0);
                    if (table.Columns.Contains("Module"))
                        table.Columns["Module"].SetOrdinal(1);
                    if (table.Columns.Contains("Role"))
                        table.Columns["Role"].SetOrdinal(3);
                    if (table.Columns.Contains("User"))
                        table.Columns["User"].SetOrdinal(4);
                }


                if (counter == 0)
                {
                    worksheet = aspxSpreadSheetCtr.Document.Worksheets[0];
                    worksheet.Name = tuple.Item1;
                }
                else
                    worksheet = aspxSpreadSheetCtr.Document.Worksheets.Add(tuple.Item1);
                counter++;

                worksheet.Import(table, true, 0, 0);
            }

            if (string.IsNullOrEmpty(fileName))
                fileName = "Application - Access";
            MemoryStream st = new MemoryStream();
            IWorkbook book = aspxSpreadSheetCtr.Document;
            if (book.Worksheets.Count > 0)
                book.Worksheets.ActiveWorksheet = book.Worksheets[0];
            aspxSpreadSheetCtr.Document.SaveDocument(st, DocumentFormat.OpenXml);
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();

        }
        private List<Tuple<string, string>> GetDisplayName(string listName)
        {
            List<Tuple<string, string>> lstDPNames = new List<Tuple<string, string>>();
            switch (listName)
            {
                case "ApplicationModules":
                    lstDPNames = new List<Tuple<string, string>>()
                                        {
                                            new Tuple<string,string>(DatabaseObjects.Columns.APPTitleLookup,"Application"),
                                              new Tuple<string,string>(DatabaseObjects.Columns.ItemOrder,"Item Order"),
                                             new Tuple<string,string>(DatabaseObjects.Columns.Title,"Module"),
                                        };
                    break;
                case "ApplicationRole":
                    lstDPNames = new List<Tuple<string, string>>()
                                        {
                                            new Tuple<string,string>(DatabaseObjects.Columns.APPTitleLookup,"Application"),
                                             new Tuple<string,string>(DatabaseObjects.Columns.ItemOrder,"Item Order"),
                                              new Tuple<string,string>(DatabaseObjects.Columns.ApplicationRoleModuleLookup,"Module"),
                                               new Tuple<string,string>(DatabaseObjects.Columns.Title,"Role")
                                        };
                    break;
                case "ApplModuleRoleRelationship":
                    lstDPNames = new List<Tuple<string, string>>()
                                        {
                                            new Tuple<string,string>(DatabaseObjects.Columns.APPTitleLookup,"Application"),
                                             new Tuple<string,string>(DatabaseObjects.Columns.ApplicationRoleModuleLookup,"Module"),
                                              new Tuple<string,string>(DatabaseObjects.Columns.ApplicationRoleLookup,"Role"),
                                              new Tuple<string,string>(DatabaseObjects.Columns.ApplicationRoleAssign,"User")
                                        };
                    break;
                default:
                    break;
            }
            return lstDPNames;
        }
        private List<DataColumn> ExcelGenerateColumns(DataTable list)
        {
            List<DataColumn> spfieldList = new List<DataColumn>();
            var fieldColl = list.Columns;//Datacolumn collection
            foreach (DataColumn field in fieldColl)
            {
                if (!field.ReadOnly && !field.ColumnName.Equals(DatabaseObjects.Columns.ContentType) && field.ColumnName != DatabaseObjects.Columns.Attachments)
                    spfieldList.Add(field);
            }
            return spfieldList;
        }

        private DataTable SheetWithDefaultSchema(DataTable table, Tuple<string, string, string> tuple)
        {
            if (table == null)
                table = new DataTable();

            if (tuple.Item3 == DatabaseObjects.Tables.ApplicationModules)
            {
                table.Columns.Add("Application");
                table.Columns.Add("Item Order");
                table.Columns.Add("Module");


            }
            else if (tuple.Item3 == DatabaseObjects.Tables.ApplicationRole)
            {
                table.Columns.Add("Application");
                table.Columns.Add("Item Order");
                table.Columns.Add("Module");
                table.Columns.Add("Role");
            }
            else if (tuple.Item3 == DatabaseObjects.Tables.ApplModuleRoleRelationship)
            {
                table.Columns.Add("Application");
                table.Columns.Add("Module");
                table.Columns.Add("Role");
                table.Columns.Add("User");
            }

            DataView dataView = table.DefaultView;
            if (tuple.Item3 == DatabaseObjects.Tables.ApplicationModules)
                table = dataView.ToTable(true, new string[] { "Application", "Item Order", "Module" });
            else if (tuple.Item3 == DatabaseObjects.Tables.ApplicationRole)
                table = dataView.ToTable(true, new string[] { "Application", "Item Order", "Module", "Role" });
            else if (tuple.Item3 == DatabaseObjects.Tables.ApplModuleRoleRelationship)
                table = dataView.ToTable(true, new string[] { "Application", "Module", "Role", "User" });
            return table;
        }

        private void UpdateRowsForCustomization(DataRow dRow, DataColumn f, ModuleColumn modCol = null)
        {
            if (f.DataType == System.Type.GetType("System.DateTime") && modCol != null && (string.IsNullOrEmpty(modCol.ColumnType) || modCol.ColumnType == "Date"))
            {
                DateTime date = UGITUtility.StringToDateTime(dRow[f.ColumnName]);
                if (date != DateTime.MinValue)
                    dRow[f.ColumnName] = date.ToString("MM/dd/yyyy");
            }
            else if (f.DataType == System.Type.GetType("System.DateTime"))
            {
                DateTime date = UGITUtility.StringToDateTime(dRow[f.ColumnName]);
                if (date != DateTime.MinValue)
                    dRow[f.ColumnName] = date.ToString("MM/dd/yyyy");
            }
            else if (f.DataType == System.Type.GetType("System.DateTime"))
            {
                string[] userList = UGITUtility.SplitString(Convert.ToString(dRow[f.ColumnName]), ",", StringSplitOptions.RemoveEmptyEntries);
                if (userList.Length > 0)
                {
                    List<string> users = new List<string>();
                    foreach (string userName in userList)
                    {
                        UserProfileManager userProfile = new UserProfileManager(HttpContext.Current.GetManagerContext());
                        UserProfile user = userProfile.GetUserByUserName(userName);
                        if (user != null)
                            users.Add(user.Name);
                    }

                    if (users.Count > 0)
                        dRow[f.ColumnName] = String.Join("; ", users);
                }
            }
        }
    }
}
