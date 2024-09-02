using DevExpress.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL.Store;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Web
{
    public partial class CustomGroupsAndUsersInfo : UserControl
    {
        private DataTable resultedTable;
        private int resourceCount = 0;
        string departmentLabel;
        private string absoluteUrlImport = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&listName={1}";
        bool isResourceAdmin = false;
        public string FrameId { get; set; }
        public bool limitExceed = false;
        protected string dd;
        StringBuilder seletedParams = new StringBuilder();
        public string userInfoPageUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx"));
        public string BulkuserInfoPageUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=automateuserinfo"));
        public bool HidePopup;

        protected string editGroupUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=editspgroup"));
        protected string sourceUrl = string.Empty;
        protected string popupSourceUrl = string.Empty;
        protected string importUrl;
        protected string changeNameProcessUrl = "";

        UserProfileManager ObjUserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        UserRoleManager ObjRoleManager = null;
        //UserRolesManager userRoles = new UserRolesManager(HttpContext.Current.GetManagerContext());
        LandingPagesManager landingPages = null;

        Microsoft.AspNet.Identity.RoleManager<Role> UserRoleManager = new Microsoft.AspNet.Identity.RoleManager<Role>(new RoleStore<Role>(HttpContext.Current.GetManagerContext()));
        JsonUserList jUserList = new JsonUserList();
        UserOrganizationChart userOrgChart = new UserOrganizationChart();

        //DataRow[] selectedColumnsForGridView = null;
        //DataRow[] selectedColumnsForCardView = null;

        ConfigurationVariableManager objConfigurationVariableHelper;
        ModuleColumnManager objModuleColumnManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        FieldConfigurationManager fmanger = null;
        List<string> sortedQuery = new List<string>();
        private List<ModuleColumn> gridViewModuleColumns;
        private List<ModuleColumn> cardViewModuleColumns;
        bool enableDivision = false;
        public CustomGroupsAndUsersInfo()
        {
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            objModuleColumnManager = new ModuleColumnManager(context);
            gridViewModuleColumns = CacheHelper<object>.Get($"ModuleColumnslistview{context.TenantID}") as List<ModuleColumn>;
            if (gridViewModuleColumns == null)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context);
                gridViewModuleColumns = moduleColumnManager.Load();
                CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{context.TenantID}", gridViewModuleColumns);
            }
            cardViewModuleColumns = gridViewModuleColumns.Where(x => x.CategoryName == Utility.Constants.ResourceCardView).ToList();
            gridViewModuleColumns = gridViewModuleColumns.Where(x => x.CategoryName == Utility.Constants.ResourceTab).ToList();

            //selectedColumnsForGridView = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, Utility.Constants.ResourceTab));
            //selectedColumnsForCardView = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, Utility.Constants.ResourceCardView));
        }

        protected override void OnInit(EventArgs e)
        {
            TenantValidation tenantValidation = new TenantValidation(context);
            fmanger = new FieldConfigurationManager(context);
            ObjRoleManager = new UserRoleManager(context);
            landingPages = new LandingPagesManager(context);
            //BTS-22-000975: Set enableDivision and Populate the lists of Department and Division initially for later use.
            enableDivision = context.ConfigManager.GetValueAsBool(ConfigConstants.EnableDivision);

            if (tenantValidation.IsUserLimitExceed())
            {
                limitExceed = true;
            }

            //userBoxList.UserTokenBoxAdd.ClientSideEvents.ValueChanged = "function(s,e){  showloadingpanel();CallbackPanelGroup.PerformCallback('');}";
            userBoxList.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
            userBoxList.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Select User(s)";
            userBoxList.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.Text;
            userBoxList.UserTokenBoxAdd.ValidationSettings.ValidationGroup = "ValidateAddUserToGroup";

            rolesListBox.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
            rolesListBox.devexListBox.ValidationSettings.RequiredField.ErrorText = "Select Group(s)";
            rolesListBox.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.Text;
            rolesListBox.devexListBox.ValidationSettings.ValidationGroup = "ValidateAddUserToGroup";
            rolesListBox.devexListBox.Columns["Title"].Caption = "Choose Groups";
            rolesListBox.devexListBox.CssClass = "lookup-inputWrap";
            rolesListBox.devexListBox.GridViewProperties.Settings.VerticalScrollableHeight = 130;

            pplUserAccount.devexListBox.ValidationSettings.RequiredField.IsRequired = false;
            pplUserAccount.devexListBox.ValidationSettings.RequiredField.ErrorText = "Select Group(s)";
            pplUserAccount.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.Text;
            pplUserAccount.devexListBox.ValidationSettings.ValidationGroup = "ValidateAddUserToGroup";
            pplUserAccount.devexListBox.Columns["Title"].Caption = "Choose Groups";
            pplUserAccount.devexListBox.CssClass = "lookup-inputWrap";
            pplUserAccount.devexListBox.GridViewProperties.Settings.VerticalScrollableHeight = 130;

            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            aspxGridviewFiltered.SettingsPager.Position = PagerPosition.Bottom;
            //grid.Settings.GridLines = GridLines.Vertical;
            //aspxGridviewFiltered.Theme = "CustomMaterial";

            aspxGridviewFiltered.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            aspxGridviewFiltered.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            aspxGridviewFiltered.Settings.GridLines = GridLines.Horizontal;

            aspxGridviewFiltered.Styles.Header.Border.BorderStyle = BorderStyle.None;
            aspxGridviewFiltered.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            //grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            //           grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
            aspxGridviewFiltered.SettingsBehavior.AllowDragDrop = false;
            aspxGridviewFiltered.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
            aspxGridviewFiltered.Settings.ShowHeaderFilterBlankItems = false;
            aspxGridviewFiltered.SettingsPager.Position = PagerPosition.Bottom;
            aspxGridviewFiltered.SettingsPopup.HeaderFilter.Width = 300;
            aspxGridviewFiltered.SettingsPopup.HeaderFilter.Height = 300;
            aspxGridviewFiltered.StylesPopup.HeaderFilter.Content.CssClass = "SearchFilter_content";
            aspxGridviewFiltered.StylesPopup.HeaderFilter.ButtonPanel.CancelButton.CssClass = "Filter_cancelBtn";
            aspxGridviewFiltered.StylesPopup.HeaderFilter.ButtonPanel.OkButton.CssClass = "Filter_okBtn";
            aspxGridviewFiltered.StylesPopup.HeaderFilter.Footer.CssClass = "FilterFooter_btnWrap";
            BindRoles();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string mailParameter = string.Empty;
            string frmTrailUser = string.Empty;
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.Split('?').Length > 1)
            {
                var requestFromMail = (Request.UrlReferrer.AbsoluteUri.Split('?')[1]).Split('&');
                if (requestFromMail.Count() > 1)
                {
                    mailParameter = Convert.ToString(requestFromMail[0].ToString());
                    frmTrailUser = Convert.ToString(requestFromMail[1].ToString());
                }
            }

            string username = HttpContext.Current.User.Identity.Name.ToString();
            changeNameProcessUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=accountchangeprocess"));
            isResourceAdmin = ObjUserManager.IsRole("Admin", username) || ObjUserManager.IsRole("ResourceAdmin", username) || ObjUserManager.IsRole("UGITSuperAdmin", username);
            bool enableUserCreation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateNewUser);
            newUserDiv.Visible = enableUserCreation && isResourceAdmin;
            newUserDiv.Visible = isResourceAdmin;


            scriptLetral.Text = "";
            aspxGridviewFiltered.EnableRowsCache = false;
            // ugitUserProfileCardView.Settings.ShowCustomizationPanel = false;
            ugitUserProfileCardView.SettingsBehavior.EnableCustomizationWindow = false;

            if (isResourceAdmin)
                div1replaceuser.Visible = true;
            if (ASPxCallbackPanelEnableUser.IsCallback)
            {
                //get the id of selected users
                List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
                List<string> usersRoleList = new List<string>();
                if (objList != null && objList.Count > 0)
                {
                    foreach (string uID in objList)
                    {
                        List<string> iRoleList = ObjUserManager.GetRoles(uID).ToList();
                        foreach (string role in iRoleList)
                        {
                            //if List does not contain role the add it.
                            if (!usersRoleList.Contains(role))
                            {
                                usersRoleList.Add(role);
                            }
                        }
                    }
                }
                pplUserAccount.SetText(string.Join(" ,", usersRoleList.ToArray()));
            }
            if (ASPxCallbackPanelGroupAdd.IsCallback)
            {
                //get the id of selected users
                List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
                userBoxList.SetValues("");
                rolesListBox.SetValues(ddlUserGroups.SelectedValue.ToString());
            }

            if (!Page.IsPostBack)
            {
                
                //Keeps filter, when request comes after update user info
                object cacheVal = Context.Cache.Get(string.Format("EditUserInfo-{0}", ObjUserManager.GetUsersProfile().SingleOrDefault(x => x.UserName == username).Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("EditUserInfo-{0}", ObjUserManager.GetUsersProfile().SingleOrDefault(x => x.UserName == username).Id));

                    Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                    if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("ugroup"))
                    {
                        ddlUserGroups.SelectedIndex = ddlUserGroups.Items.IndexOf(ddlUserGroups.Items.FindByValue(cacheParams["ugroup"]));
                    }
                }

                if (!string.IsNullOrEmpty(mailParameter) && mailParameter.ToLower() == "frommail" && Request["ismail"] != "1")
                {
                    string edititem = "";
                    if (!string.IsNullOrEmpty(frmTrailUser))
                    {
                        // edititem = "/ControlTemplates/RMM/userinfo.aspx?uID=0&newUser=1&ismail=0";
                        edititem = BulkuserInfoPageUrl;
                    }
                    else
                    {
                        edititem = "/ControlTemplates/RMM/userinfo.aspx?uID=0&newUser=1&ismail=1";
                    }
                    string script = string.Format("JavaScript:window.parent.UgitOpenPopupDialog('{0}','','Add Multiple Users','750px','90',false)", edititem);
                    // string script = string.Format("JavaScript:UgitOpenPopupDialog('{0}','','Add New User','625px','700',false)");

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
                    if (!string.IsNullOrEmpty(frmTrailUser))
                    {

                        //Server.Transfer("~/Pages/RMM");
                    }
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>btnCreateUser_Click();</Script>", false);
                    // mailParameter = null;
                }

            }
            else
            {

            }

            string alternet = hdntogglevalue.Value;
            //To Show and Hide
            ShowAndHideControl(alternet);

            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "UserInformationList"));
            if (ddlUserGroups.Items.IndexOf(ddlUserGroups.Items.FindByText("Disabled Users")) != -1 && ddlUserGroups.SelectedItem.Text.ToLower() == "disabled users")
            {
                // divenable.Visible = true;
                divenable.Visible = isResourceAdmin;
                divdisable.Visible = false;
            }
            else
            {
                divdisable.Visible = isResourceAdmin;
                divenable.Visible = false;
            }
            //Rename the "Resource Tab" columns to match with the dataset columns for DataBind. 
            foreach(ModuleColumn col in gridViewModuleColumns)
            {
                if (col.FieldName.EndsWith("Lookup") || col.FieldName.EndsWith("User"))
                {
                    col.FieldName = col.FieldName + "$";
                }
                if (col.FieldName == "GlobalRoleID")
                    col.FieldName = DatabaseObjects.Columns.GlobalRoleID + "$";
            }

        }

        protected void DdlUserGroups_Init(object sender, EventArgs e)
        {
            UserRoleManager roleManager = new UserRoleManager(context);
            List<Role> sRoles = roleManager.GetRoleList().OrderBy(x => x.Title).ToList();
            UserProfile user = context.CurrentUser;
            ObjUserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            bool isSuperAdmin = ObjUserManager.IsUGITSuperAdmin(user);
            if (!isSuperAdmin)
            {
                sRoles = sRoles.Where(x => x.Name != "UGITSuperAdmin").ToList();
            }

            if (ddlUserGroups.Items.Count <= 0 && sRoles != null)
            {
                foreach (Role group in sRoles)
                {
                    ddlUserGroups.Items.Add(new ListItem(group.Title, group.Id.ToString()));
                }
                ddlUserGroups.Items.Insert(0, new ListItem("All Groups", "0"));
                ddlUserGroups.Items.Insert(1, new ListItem("Disabled Users", "00"));
            }

            ShowHideGroupButtons();
        }

        protected void ddlUserGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideGroupButtons();
        }

        // show/hide 'Add in Group' and 'Delete From Group' buttons when 'All Groups' & 'Disabled Users' items are seleted in 'User Group'.
        private void ShowHideGroupButtons()
        {
            if (ddlUserGroups.SelectedValue == "0" || ddlUserGroups.SelectedValue == "00")
            {
                btnAddInGroup.Visible = false;
                btnDeleteFromGroup.Visible = false;
            }
            else
            {
                btnAddInGroup.Visible = true;
                btnDeleteFromGroup.Visible = true;
            }
        }

        private void SetResultedData(bool setJson = false)
        {
            DataTable userTable = new DataTable();
            DataRow[] drUsers = null;

            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            values.Add("@EnableDivision", enableDivision);
            //Call SP to fetch data
            DataTable newTable = GetTableDataManager.GetData(DatabaseObjects.Tables.AspNetUsers, values);
            DataTable dtRawUserData = newTable.Clone();
            foreach (DataColumn dc in dtRawUserData.Columns)
            {
                if (dc.DataType == typeof(System.DateTime))
                {
                    dc.DataType = typeof(string);
                }
            }
            //newTable = dtRawUserData;
            dtRawUserData.Merge(newTable,false,MissingSchemaAction.Ignore);
            List<ModuleColumn> cols = gridViewModuleColumns.Where(x => x.ColumnType.EqualsIgnoreCase("Date") || x.ColumnType.EqualsIgnoreCase("DateTime")).ToList();
            foreach (ModuleColumn _item in cols)
            {
                foreach (DataRow item in dtRawUserData.Rows)
                {
                    if (UGITUtility.IfColumnExists(item, _item.FieldName))
                    {
                        DateTime dt;
                        if (DateTime.TryParse(Convert.ToString(item[_item.FieldName]), out dt))
                        {
                            var columnType = gridViewModuleColumns.Where(x => x.FieldName == _item.FieldName).FirstOrDefault();
                            if (columnType.ColumnType.EqualsIgnoreCase("date"))
                                item[_item.FieldName] = uHelper.GetDateStringInFormat(context, dt, false);
                            else if (columnType.ColumnType.EqualsIgnoreCase("datetime"))
                                item[_item.FieldName] = uHelper.GetDateStringInFormat(context, dt, true);
                        }
                    }
                }
            }
            
            if (seletedParams.ToString() == string.Empty)
            {
                seletedParams.AppendFormat("&ugroup={0}", ddlUserGroups.SelectedValue);
            }
            string groupId = null;
            //Filter Enabled or Disabled users
            if (ddlUserGroups.Items.IndexOf(ddlUserGroups.Items.FindByText("Disabled Users")) != -1 && ddlUserGroups.SelectedItem.Text.ToLower() == "disabled users")
            {
                if (isResourceAdmin)
                    divenable.Visible = true;
                drUsers = dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Enabled, "0"));
            }
            else
            {
                if (isResourceAdmin)
                    divdisable.Visible = true;
                drUsers = dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Enabled, "1"));
            }
            //Further filter the data if any group is selected.
            if (ddlUserGroups.SelectedIndex > 1)
            {
                groupId = ddlUserGroups.SelectedValue;
                List<UserRole> userRoles = ObjUserManager.GetUserRoleListByRole(groupId);
                drUsers = drUsers.Where(x => userRoles.Any(y => y.UserId.Equals(x[DatabaseObjects.Columns.ID]))).ToArray();
            }
            if (drUsers.Length > 0)
                dtRawUserData = drUsers.CopyToDataTable();
            else
                dtRawUserData.Clear();
            //dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsIT + "bool", "True")).ToList<DataRow>().ForEach(r => r[DatabaseObjects.Columns.IsIT] = "Yes");
            //dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsIT + "bool", "False")).ToList<DataRow>().ForEach(r => r[DatabaseObjects.Columns.IsIT] = "No");
            //dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsManager + "bool", "True")).ToList<DataRow>().ForEach(r => r[DatabaseObjects.Columns.IsManager] = "Yes");
            //dtRawUserData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.IsManager + "bool", "False")).ToList<DataRow>().ForEach(r => r[DatabaseObjects.Columns.IsManager] = "No");
            //SPFieldUrlValue value = new SPFieldUrlValue();
            //List<UserProfile> uProfiles = null;
            //create json if parameter is true for Org chart creation
            if (setJson)
            {
                foreach (DataRow dr in drUsers)
                {
                    JsonUser jUser = new JsonUser
                    { 
                        id = dr[DatabaseObjects.Columns.Id].ToString(),
                        parentId = dr[DatabaseObjects.Columns.ManagerID].ToString(),
                        name = dr[DatabaseObjects.Columns.Name].ToString(),
                        title = dr[DatabaseObjects.Columns.JobProfile].ToString(),
                        email = dr[DatabaseObjects.Columns.EmailID].ToString(),
                        phone = dr[DatabaseObjects.Columns.MobilePhone].ToString(),
                        picture = dr[DatabaseObjects.Columns.Picture].ToString()
                    };
                    jUserList.Users.Add(jUser);
                }
            }

            sourceUrl = Request.Url.PathAndQuery + seletedParams.ToString();
            popupSourceUrl = string.Format("{0}?frameObjId={1}", Request.Url.AbsolutePath, Request["frameObjId"]);
            if (dtRawUserData != null)
            {
                //fetch columns which have SortOrder value to sort the final data
                List<ModuleColumn> sortedColumns = gridViewModuleColumns.Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
                foreach (ModuleColumn column in sortedColumns)
                {
                    sortedQuery.Add(column.FieldName + " " + (column.IsAscending == true ? "ASC" : "DESC"));
                }
                dtRawUserData.DefaultView.Sort = string.Join(", ", sortedQuery);
                //userTable = dtRawUserData.DefaultView.ToTable();
            }
            resultedTable = dtRawUserData;
            resourceCount = dtRawUserData.Rows.Count;
        }
        protected void ExportToCSV()
        {
            // In below functionality, RoleNme column is used for 'User Group' (Groups) and
            // UserRoleId column is for Role like SMS User, PMO for redirecting to particular page.

            string value = string.Empty;
            DataTable newcsvtable = resultedTable.Clone();
            string[] arrRequiredColumns = null;
            List<string> columnList = new List<string>();
            List<string> sortedQuery = new List<string>();
            List<ModuleColumn> filterCols = gridViewModuleColumns.Where(x => x.FieldName != DatabaseObjects.Columns.Id && x.IsDisplay).OrderBy(x => x.FieldSequence).ToList();
            if (filterCols.Count() == 0) //Default export data 
            {
                columnList.Add(DatabaseObjects.Columns.Name);
                columnList.Add(DatabaseObjects.Columns.JobProfile);
                columnList.Add(DatabaseObjects.Columns.Manager);
                columnList.Add("Department");
                columnList.Add("Location");
                columnList.Add(DatabaseObjects.Columns.MobilePhone);
                columnList.Add(DatabaseObjects.Columns.IsIT);
                columnList.Add(DatabaseObjects.Columns.IsConsultant);
                columnList.Add(DatabaseObjects.Columns.IsManager);
                columnList.Add("RoleName");
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.DepartmentLookup))
                    resultedTable.Columns[DatabaseObjects.Columns.DepartmentLookup].ColumnName = "Department";
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.LocationLookup))
                    resultedTable.Columns[DatabaseObjects.Columns.LocationLookup].ColumnName = "Location";
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.GlobalRoleID))
                    resultedTable.Columns[DatabaseObjects.Columns.GlobalRoleID].ColumnName = "RoleName";
            }
            //Add the columns using their FieldDisplayName to the Column list.
            //If FieldDisplayName column does not exist and FieldName exists in the resultedTable, then rename FieldName column as FieldDisplayName.
            //This has been done so that the table can be sent directly to excel.
            //Ex: If Department column doesnt exist, rename DepartmentLookUp as Department.
            foreach (ModuleColumn modulecolumn in filterCols)
            {
                columnList.Add(modulecolumn.FieldDisplayName);
                if (!resultedTable.Columns.Contains(modulecolumn.FieldDisplayName) && resultedTable.Columns.Contains(modulecolumn.FieldName))
                    resultedTable.Columns[modulecolumn.FieldName].ColumnName = modulecolumn.FieldDisplayName;
            }
            arrRequiredColumns = columnList.ToArray();
            newcsvtable = new DataView(resultedTable).ToTable(false, arrRequiredColumns);
            newcsvtable.DefaultView.Sort = string.Join(", ", sortedQuery);          

            string csvData = UGITUtility.ConvertTableToCSV(newcsvtable);
            string attachment = string.Format("attachment; filename={0}.csv", "Export");
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.Write(csvData.ToString());
            Response.Flush();
            Response.End();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Show hide action button according to user role.
            if (isResourceAdmin)
            {
                //btnDeleteFromGroup.Visible = true;
                //btnAddInGroup.Visible = true;
                ShowHideGroupButtons();
                btnDeleteFromSite.Visible = true;
                btnCreateUser.Visible = true;
                lbCreateGroup.Visible = true;
                btndisableuser.Visible = true;
                if (ddlUserGroups.SelectedIndex <= 1)
                {
                    lbEditGroup.Visible = false;
                }
                if (ddlUserGroups.SelectedIndex > 1)
                { lbEditGroup.Visible = true; }
                onlyExcelExport.Visible = true;

                if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableImportUsers))
                    btnImportUsers.Visible = true;
            }
            BindAspxGridView();
            importUrl += seletedParams.ToString();
            base.OnPreRender(e);
        }

        #region ASPxGridView Control Events
        private void BindAspxGridView()
        {
            // Added below lines to change Header from  'User Role' to 'Role'.
            if (aspxGridviewFiltered.Columns["User Role"] != null)
                    aspxGridviewFiltered.Columns["User Role"].Caption = DatabaseObjects.Columns.Role;

            aspxGridviewFiltered.DataBind();
            ugitUserProfileCardView.DataBind();
        }

        #endregion

        public DataTable getDataSource(string loadGrid)
        {
            if (loadGrid == "Card View")
            {
                SetResultedData();
                CreateDvxCarddView();
                aspxGridviewFiltered.SettingsPager.Visible = (resultedTable != null && resultedTable.Rows.Count > 0);
                return resultedTable;
            }
            else
            {
                SetResultedData();
                CreateAspxGridView();
                aspxGridviewFiltered.SettingsPager.Visible = (resultedTable != null && resultedTable.Rows.Count > 0);
                return resultedTable;

            }
        }

        protected void btnDeleteFromGroup_Click(object sender, EventArgs e)
        {
            if (ddlUserGroups.SelectedIndex > 1)
            {
                DeleteFromGroup();
                //BindAspxGridView();
            }
            else { }
        }

        private void DeleteFromGroup()
        {
            List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            if (objList != null && objList.Count > 0)
            {
                foreach (string item in objList)
                {
                    try
                    {
                        UserProfile u = ObjUserManager.GetUserById(item);
                        Role group = UserRoleManager.FindById(Convert.ToString(ddlUserGroups.SelectedItem.Value));
                        ObjUserManager.DeleteFromGroupById(item, group.Name);
                        Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Removed user: {u.Name} from group: {group.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.UserProfile), context.TenantID);
                    }
                    catch (Exception ex)
                    {
                        Util.Log.ULog.WriteException(ex);
                    }
                }
            }
            BindAspxGridView();
        }

        private void DeleteFromSite()
        {
            string Message = "";
            string multiName = string.Empty;
            UserProfile u = null;
            UserRoleManager uRole = new UserRoleManager(context);
            // Delete the selected user(s) from site.
            List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            if (objList != null && objList.Count > 0)
            {
                foreach (string item in objList)
                {
                    try
                    {
                        u = ObjUserManager.GetUserById(item);
                        List<UserProfile> delegateTaskFor = ObjUserManager.GetUserInfosById(u.DelegateUserFor);

                        var qu = (from ord in uRole.GetRoleList().Where(y => y.IsSystem).ToList() join detail in ObjUserManager.GetUserRoles(item) on ord.Id equals detail.Id select new { ord.Name });

                        if (qu.ToList().Count > 0)
                        {
                            Message += u.Name + " : This is system type user so you can't delete it<br/>";
                        }
                        else if (!string.IsNullOrEmpty(u.DelegateUserFor))
                        {
                            foreach (var user in delegateTaskFor)
                            {
                                multiName = multiName != "" ? multiName = multiName + " and " + user.Name : multiName + user.Name;
                            }

                            Message += multiName + " assign their task to " + u.UserName + " so you can't delete it<br/>";
                        }
                        else
                        {
                            if (!ObjUserManager.isCurrentUser(item))
                            {
                                ObjUserManager.DeleteUserById(item); Message += u.Name + " : has been deleted;<br/>";
                                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Removed user: {u.Name}.", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.UserProfile), context.TenantID);
                            }
                            else { Message += u.Name + " : is current user;<br/>"; }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log.ULog.WriteException(ex);
                    }
                }
                ObjUserManager.UpdateIntoCache(u);
                BindAspxGridView();
            }
            lblMessage.Text = Message;
            msgPopup.ShowOnPageLoad = true;
        }

        protected void btnDeleteFromSite_Click(object sender, EventArgs e)
        {
            DeleteFromSite();
            BindAspxGridView();
        }

        protected void aspxGridViewCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            // Un comment below lines when required.
            //string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
            //FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            //string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);

            if (e.DataColumn.FieldName == "Name")
            {
                e.DataColumn.PropertiesEdit.EncodeHtml = true;
                string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1", e.GetValue(DatabaseObjects.Columns.Id)));
                string TitleLink = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false,\"{2}\")'>{3}</a>",
                                                userLinkUrl, e.GetValue("Name").ToString().Replace("'", string.Empty), Uri.EscapeDataString(sourceUrl), Convert.ToString(e.GetValue(DatabaseObjects.Columns.Name)));
                e.Cell.Text = TitleLink;
            }
            //if(e.DataColumn.FieldName==DatabaseObjects.Columns.Created)
            //{
            //    DateTime dt;
            //    if (DateTime.TryParse(Convert.ToString(e.CellValue), out dt))
            //    {
            //        e.DataColumn.PropertiesEdit.EncodeHtml = true;
            //        var columnType = gridViewModuleColumns.Where(x => x.FieldName == DatabaseObjects.Columns.Created).FirstOrDefault();
            //        if (columnType.ColumnType.EqualsIgnoreCase("date"))
            //            e.Cell.Text = uHelper.GetDateStringInFormat(context, dt, false);
            //        else if (columnType.ColumnType.EqualsIgnoreCase("datetime"))
            //            e.Cell.Text = uHelper.GetDateStringInFormat(context, dt, true);
            //    }
            //}

            if (e.DataColumn.FieldName == "ManagerID")
            {
                e.DataColumn.PropertiesEdit.EncodeHtml = true;
                var managervalue = ObjUserManager.GetUserById(e.GetValue("ManagerID").ToString());
                if (managervalue != null)
                {
                    string managerLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1", managervalue.Id));
                    string ManagerLink = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false, \"{2}\")'>{3}</a>",
                                                managerLinkUrl, Convert.ToString(managervalue.Name).Replace("'", string.Empty), Uri.EscapeDataString(sourceUrl), Convert.ToString(managervalue.Name));

                    e.Cell.Text = ManagerLink;
                }
            }
        }

        protected void aspxGridviewFiltered_DataBinding(object sender, EventArgs e)
        {
            if (ddlViews.SelectedValue == "List View")
                aspxGridviewFiltered.DataSource = getDataSource("List View");
            else if (ddlViews.SelectedValue == "Card View")
            {
                ugitUserProfileCardView.DataSource = getDataSource("Card View");
                HideButtonForChartViewAndOrg();
            }

            else
                HideButtonForChartViewAndOrg();
        }

        protected void aspxGridviewFiltered_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            string values = Convert.ToString(e.GetFieldValue(e.Column.FieldName));
            //FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            string value = string.Empty;
            if (e.Column.FieldName == DatabaseObjects.Columns.Location || e.Column.FieldName == DatabaseObjects.Columns.LocationLookup)
            {
                value = fmanger.GetFieldConfigurationData(DatabaseObjects.Columns.LocationLookup, values);
                e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                e.DisplayText = value;
            }
            if (e.Column.FieldName == DatabaseObjects.Columns.ManagerID)
            {
                e.Column.PropertiesEdit.EncodeHtml = true;
                var managervalue = ObjUserManager.GetUserById(values);
                if (managervalue != null)
                {
                    string managerLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}", managervalue.Id));
                    string ManagerLink = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false, \"{2}\")'>{3}</a>",
                                                managerLinkUrl, Convert.ToString(managervalue.Name).Replace("'", string.Empty), Uri.EscapeDataString(sourceUrl), Convert.ToString(managervalue.Name));
                    e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    e.DisplayText = managervalue.Name;
                }
            }
            if (e.Column.FieldName == DatabaseObjects.Columns.IsIT)
            {
                e.Column.PropertiesEdit.EncodeHtml = true;
                value = values == "False" ? "No" : "Yes";
                e.DisplayText = value;
            }

            if (e.Column.FieldName == "UserRoleId" || e.Column.FieldName == DatabaseObjects.Columns.UserRoleIdLookup)
            {
                if (!string.IsNullOrEmpty(values))
                {
                    LandingPages userDetails = umanager.GetUserRoleById(values);
                    if (userDetails != null)
                        e.DisplayText = userDetails.Name;
                }
                else
                {
                    e.DisplayText = values;
                }
            }
            if (e.Column.FieldName == "GlobalRoleID" || e.Column.FieldName == DatabaseObjects.Columns.GlobalRoleID)
            {
                value = fmanger.GetFieldConfigurationData(DatabaseObjects.Columns.GlobalRoleID, values);
            }

        }

        void AddUserOrganizationChart()
        {
            SetResultedData(true);
            userOrgChart = (UserOrganizationChart)Page.LoadControl("~/CONTROLTEMPLATES/RMM/UserOrganizationChart.ascx");
            userOrgChart.jsonString = ProcessUserList(jUserList);
            orgChartdiv.Controls.Add(userOrgChart);
        }

        string ProcessUserList(JsonUserList userList)
        {
            JsonUserList jusers = new JsonUserList();
            List<JsonUser> juserlist = new List<JsonUser>();
            userList.Users = userList.Users.OrderBy(x => x.parentId).ThenBy(x => x.name).ToList();
            foreach (JsonUser user in userList.Users)
            {

                if (user.name == "System Account")
                    continue;

                if (!juserlist.Exists(x => x != null && x.id == user.id))
                {

                    if (string.IsNullOrEmpty(user.parentId))
                    {
                        if (!juserlist.Exists(x => x.parentId == user.parentId))
                        {
                            juserlist.Insert(0, user);
                        }
                        else
                        {
                            juserlist.Add(user);
                        }
                    }
                    else
                    {
                        if (!userList.Users.Exists(x => x.id == user.parentId) || user.id == user.parentId)
                        {
                            user.parentId = "";
                        }
                        juserlist.Add(user);
                        int index = juserlist.Count - 1;
                        juserlist = ReorderUsers(juserlist, userList.Users, user.parentId, index);
                    }
                }
            }

            List<JsonUser> juserl = juserlist.FindAll(x => x.parentId == "");
            juserlist.RemoveAll(x => x.parentId == "");
            juserlist.InsertRange(0, juserl.OrderBy(x => x.name));

            string jsonString = jusers.Serialized(juserlist);
            return jsonString;
        }

        List<JsonUser> ReorderUsers(List<JsonUser> juserlist, List<JsonUser> juserlistorg, string parentid, int index)
        {
            var user2 = juserlistorg.Find(x => x != null && x.id == parentid);
            if (user2 != null)
            {
                if (juserlist.Exists(x => x.id == parentid))
                {
                    return juserlist;
                }
                if (!juserlistorg.Exists(x => x.id == user2.parentId) || user2.id == user2.parentId)
                {
                    user2.parentId = "";
                }

                juserlist.Insert(index, user2);
                juserlist = ReorderUsers(juserlist, juserlistorg, user2.parentId, index);
            }

            return juserlist;
        }

        protected void aspxGridviewFiltered_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            bool needToHandleMultiValue = false;
            if (e.Column.FieldName == "UserSkill")
            {
                needToHandleMultiValue = true;
            }

            if (e.Values != null && e.Values.Count > 0)
            {
                e.Values.RemoveAll(filterValue => Guid.TryParse(filterValue.Value, out Guid result));
            }

            if (needToHandleMultiValue)
            {
                List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
                List<string> values = new List<string>(); // List in string format used to keep out duplicates
                foreach (FilterValue fValue in e.Values)
                {
                    if (fValue.Value.Contains(";"))
                    {
                        // Found multiple semi-colon separated values
                        string[] vals = fValue.Value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in vals)
                        {
                            // Add to filter list only if not already in it
                            string trimmedVal = val.Trim();
                            if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                            {
                                temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                                values.Add(trimmedVal);
                            }
                        }
                    }
                    else
                    {
                        // Single value, add to list if not already in it
                        string trimmedVal = fValue.Value.Trim();
                        if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                        {
                            temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                            values.Add(trimmedVal);
                        }
                    }
                }

                // Add to filter list in order
                temp = temp.OrderBy(o => o.Value).ToList();
                e.Values.Clear();
                foreach (FilterValue fVal in temp)
                {
                    e.Values.Add(fVal);
                }
            }
        }

        private void CreateDvxCarddView()
        {
            CardViewLayoutGroup CardViewLay = new CardViewLayoutGroup();
            if (ugitUserProfileCardView.Columns.Count > 0)
                return;
            if (cardViewModuleColumns.Count() > 0) //From MyModuleColumns
            {
                //DataRow[] filterRows = selectedColumnsForCardView.Where(x => x.Field<string>(DatabaseObjects.Columns.FieldName) != DatabaseObjects.Columns.Id && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "1").OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                List<ModuleColumn> filterCols = cardViewModuleColumns.Where(x => x.FieldName != DatabaseObjects.Columns.Id && x.IsDisplay).OrderBy(x => x.FieldSequence).ToList();

                foreach (ModuleColumn modulecolumn in filterCols)
                {

                    if (modulecolumn.FieldName == DatabaseObjects.Columns.Picture)
                    {
                        CardViewImageColumn Col = new CardViewImageColumn();
                        Col.FieldName = modulecolumn.FieldName;
                        Col.Caption = "";
                        CardViewColumnLayoutItem CitemLay = new CardViewColumnLayoutItem();
                        CitemLay.ColumnName = DatabaseObjects.Columns.Picture;
                        CitemLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
                        CitemLay.VerticalAlign = FormLayoutVerticalAlign.Top;
                        ugitUserProfileCardView.CardLayoutProperties.Items.AddColumnItem(CitemLay);

                        if (resultedTable != null && resultedTable.Columns.Contains(modulecolumn.FieldName))
                            ugitUserProfileCardView.Columns.Add(Col);
                    }
                    else
                    {
                        CardViewColumn Ccol = new CardViewColumn();
                        Ccol.FieldName = modulecolumn.FieldName;

                        CardViewLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
                        CardViewLay.VerticalAlign = FormLayoutVerticalAlign.Middle;
                        CardViewLay.SettingsItemCaptions.Location = LayoutItemCaptionLocation.Top;
                        CardViewLay.GroupBoxDecoration = GroupBoxDecoration.None;

                        CardViewColumnLayoutItem CitemLay = new CardViewColumnLayoutItem();
                        CitemLay.ColumnName = modulecolumn.FieldName;

                        CardViewLay.Items.AddColumnItem(CitemLay);

                        if (resultedTable != null && resultedTable.Columns.Contains(modulecolumn.FieldName))
                            ugitUserProfileCardView.Columns.Add(Ccol);
                    }

                }
                ugitUserProfileCardView.CardLayoutProperties.ColCount = 2;
                ugitUserProfileCardView.CardLayoutProperties.Items.AddGroup(CardViewLay);

            }
            else
            {
                CardViewImageColumn Col = new CardViewImageColumn();
                Col.FieldName = DatabaseObjects.Columns.Picture;
                Col.Caption = "";
                Col.PropertiesImage.ImageHeight = Unit.Pixel(75);
                Col.PropertiesImage.ImageWidth = Unit.Pixel(75);

                CardViewColumnLayoutItem CitemLay = new CardViewColumnLayoutItem();
                CitemLay.ColumnName = DatabaseObjects.Columns.Picture;
                CitemLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
                CitemLay.VerticalAlign = FormLayoutVerticalAlign.Top;

                ugitUserProfileCardView.CardLayoutProperties.Items.AddColumnItem(CitemLay);
                ugitUserProfileCardView.Columns.Add(Col);


                //For Layout Group
                CardViewLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
                CardViewLay.VerticalAlign = FormLayoutVerticalAlign.Middle;
                CardViewLay.SettingsItemCaptions.Location = LayoutItemCaptionLocation.Left;
                CardViewLay.GroupBoxDecoration = GroupBoxDecoration.None;


                CardViewColumn ccol = new CardViewColumn();
                ccol.FieldName = DatabaseObjects.Columns.Name;
                //ccol.Caption = DatabaseObjects.Columns.Name;
                ccol.Caption = "";

                CardViewLay.Items.AddColumnItem(ccol.FieldName);
                ugitUserProfileCardView.Columns.Add(ccol);

                ccol = new CardViewColumn();
                ccol.FieldName = DatabaseObjects.Columns.JobProfile;
                ccol.Caption = "Job Title";
                //ccol.Caption = "";

                CardViewLay.Items.AddColumnItem(ccol.FieldName);
                ugitUserProfileCardView.Columns.Add(ccol);


                ccol = new CardViewColumn();
                ccol.FieldName = "Email";
                ccol.Caption = "Email";
                //ccol.Caption = "";

                CardViewLay.Items.AddColumnItem(ccol.FieldName);
                ugitUserProfileCardView.Columns.Add(ccol);

                ccol = new CardViewColumn();
                ccol.FieldName = DatabaseObjects.Columns.PhoneNumber;
                ccol.Caption = DatabaseObjects.Columns.Phone;
                //ccol.Caption = "";

                CardViewLay.Items.AddColumnItem(ccol.FieldName);
                ugitUserProfileCardView.Columns.Add(ccol);

                ugitUserProfileCardView.CardLayoutProperties.ColCount = 2;
                ugitUserProfileCardView.CardLayoutProperties.Items.AddGroup(CardViewLay);

            }
        }

        private void CreateAspxGridView()
        {
            if (aspxGridviewFiltered.Columns.Count > 0)
                return;

            if (gridViewModuleColumns.Count() > 0) //From MyModuleColumns
            {
                GridViewCommandColumn ccol = new GridViewCommandColumn();
                ccol.Caption = string.Empty;
                ccol.ShowSelectCheckbox = true;
                aspxGridviewFiltered.Columns.Add(ccol);

                //DataRow[] filterRows = selectedColumnsForGridView.Where(x => x.Field<string>(DatabaseObjects.Columns.FieldName) != DatabaseObjects.Columns.Id && (x.Field<bool>(DatabaseObjects.Columns.IsDisplay))).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                List<ModuleColumn> filterCols = gridViewModuleColumns.Where(x => x.FieldName != DatabaseObjects.Columns.Id && x.IsDisplay).OrderBy(x => x.FieldSequence).ToList();

                foreach (ModuleColumn modulecolumn in filterCols)
                {
                    GridViewDataTextColumn col = new GridViewDataTextColumn();

                    if (modulecolumn.FieldName == DatabaseObjects.Columns.Name || modulecolumn.FieldName == DatabaseObjects.Columns.Manager)
                    {
                        col.PropertiesTextEdit.EncodeHtml = false;
                        col.Settings.FilterMode = ColumnFilterMode.Value;
                        col.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    }
                    else if (modulecolumn.IsDisplay && modulecolumn.FieldName != DatabaseObjects.Columns.Name || modulecolumn.FieldName != DatabaseObjects.Columns.Manager)
                        col.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    col.Caption = modulecolumn.FieldDisplayName;
                    col.FieldName = modulecolumn.FieldName;
                    if (col.FieldName == "GlobalRoleID")
                        col.FieldName = DatabaseObjects.Columns.GlobalRoleID;
                    if (col.FieldName == DatabaseObjects.Columns.ID)
                        col.FieldName = DatabaseObjects.Columns.Id;
                    col.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    if (resultedTable != null && resultedTable.Columns.Contains(modulecolumn.FieldName))
                        aspxGridviewFiltered.Columns.Add(col);
                }
            }
            else //Default Case
            {
                GridViewCommandColumn col = new GridViewCommandColumn();
                col.Caption = string.Empty;
                col.ShowSelectCheckbox = true;
                aspxGridviewFiltered.Columns.Add(col);

                GridViewDataColumn dcol = new GridViewDataColumn();
                dcol.FieldName = DatabaseObjects.Columns.Id;
                dcol.Caption = DatabaseObjects.Columns.Id;
                dcol.Visible = false;

                aspxGridviewFiltered.Columns.Add(dcol);

                GridViewDataTextColumn ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.TitleLink;
                ccol.Caption = DatabaseObjects.Columns.Name;
                ccol.PropertiesTextEdit.EncodeHtml = false;
                ccol.Settings.FilterMode = ColumnFilterMode.Value;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);
                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.Manager;
                ccol.PropertiesTextEdit.EncodeHtml = false;
                ccol.Settings.FilterMode = ColumnFilterMode.Value;
                aspxGridviewFiltered.Columns.Add(ccol);


                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.JobProfile;
                ccol.Caption = DatabaseObjects.Columns.Title;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = ObjUserManager.GetUserById(DatabaseObjects.Columns.ManagerID).UserName.ToString();
                ccol.Caption = "Manager Name";
                ccol.PropertiesTextEdit.EncodeHtml = false;
                ccol.Settings.FilterMode = ColumnFilterMode.Value;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = "Department";
                ccol.Caption = departmentLabel;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = "Location";
                ccol.Caption = "Location";
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                //ccol = new GridViewDataTextColumn();
                //ccol.FieldName = DatabaseObjects.Columns.DeskLocation;
                //ccol.Caption = "Desk";
                //ccol.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                //aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.PhoneNumber;
                ccol.Caption = "Phone #";
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.IsIT;
                ccol.Caption = DatabaseObjects.Columns.IT;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.IsConsultant;
                ccol.Caption = "Cons";
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = DatabaseObjects.Columns.IsManager;
                ccol.Caption = "Mgr";
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);

                ccol = new GridViewDataTextColumn();
                ccol.FieldName = "RoleName";
                ccol.Caption = DatabaseObjects.Columns.Role;
                ccol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                aspxGridviewFiltered.Columns.Add(ccol);
            }
        }

        protected void onlyExcelExport_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        protected void ShowAndHideControl(string alternet)
        {
            if (alternet == ResourceViews.OrgView)
            {
                gridviewdiv.Visible = false;
                orgChartdiv.Visible = true;
                ugitUserProfileCardViewDiv.Visible = false;
                //Bind Data
                AddUserOrganizationChart();

            }
            if (alternet == ResourceViews.ListView || alternet == string.Empty)
            {
                gridviewdiv.Visible = true;
                orgChartdiv.Visible = false;
                ugitUserProfileCardViewDiv.Visible = false;
            }
            if (alternet == ResourceViews.CardView)
            {
                gridviewdiv.Visible = false;
                orgChartdiv.Visible = false;
                ugitUserProfileCardViewDiv.Visible = true;
            }
        }

        protected void btnDevExDisable_Click(object sender, EventArgs e)
        {
            List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            string rolesRemoved;
            if (objList != null && objList.Count > 0)
            {
                List<string> existingRoles = new List<string>();
                foreach (string user in objList)
                {
                    rolesRemoved= ObjUserManager.DisableUserById(user);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Removed user: {user} from group: {rolesRemoved}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.UserProfile), context.TenantID);
                    //existingRoles.Clear();
                    //existingRoles = umanager.GetRoles(user).ToList();
                    //existingRoles.ForEach(x => { umanager.DeleteFromGroupById(user, x); });
                }
            }
            if (hdnkeepAction.Contains("action") && hdnkeepAction.Get("action").ToString() == "yes" && ddlUserGroups.SelectedIndex != -1 && !(ddlUserGroups.SelectedValue == "0" || ddlUserGroups.SelectedValue == "00"))
            {
                btnDeleteFromGroup_Click(null, null);
            }
            confirmDisablePopup.ShowOnPageLoad = false;
            resultedTable = null;
            BindAspxGridView();
        }

        protected void btnDevExEnable_Click(object sender, EventArgs e)
        {
            List<object> objList = aspxGridviewFiltered.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            foreach (string uid in objList)
            {
                if (!string.IsNullOrEmpty(pplUserAccount.GetText()))
                {
                    List<string> existingRoles = ObjUserManager.GetRoles(uid).ToList();
                    List<string> valueList = ObjRoleManager.GetRoleList().Where(x => pplUserAccount.GetValuesAsList().Contains(x.Id)).Select(c => c.Name).ToList();
                    List<string> deleteList = existingRoles.Except(valueList).ToList();
                    List<string> addList = valueList.Except(existingRoles).ToList();
                    if (deleteList.Count > 0)
                    {
                        ObjUserManager.RemoveFromRoles(uid, deleteList.ToArray());
                    }
                    if (addList.Count > 0)
                    {
                        ObjUserManager.AddToRoles(uid, addList.ToArray());
                    }
                }
                else
                {
                    Role role = ObjRoleManager.GetRoleByName("uGovernITMembers");
                    if (role != null)
                    {
                        ObjUserManager.AddToRole(uid, role.Name);
                    }

                }
                ObjUserManager.EnableUserById(uid);


            }
            //aspxuserpopup.ShowOnPageLoad = false;
            aspxenableuserpopup.ShowOnPageLoad = false;
            resultedTable = null;
            BindAspxGridView();
        }

        protected void btnAddUserToGroup_Click(object sender, EventArgs e)
        {
            AddUserToGroup();
        }

        protected void AddUserToGroup()
        {
            string[] uList = userBoxList.GetValues().ToString().Split(',');
            UserProfile user = null;
            foreach (string uid in uList)
            {
                if (!string.IsNullOrEmpty(rolesListBox.GetText().ToString()) && !string.IsNullOrEmpty(uid))
                {
                    List<string> rolesList = rolesListBox.GetValuesAsList();
                    List<string> valueList = ObjRoleManager.GetRoleList().Where(x => rolesList.Contains(x.Id)).Select(c => c.Name).ToList();

                    List<string> existingRoles = ObjUserManager.GetRoles(uid.Trim()).ToList();

                    /*Previous group was removed while adding new Group*/
                    //List<string> deleteList = existingRoles.Except(valueList).ToList();
                    List<string> addList = valueList.Except(existingRoles).ToList();
                    //if (deleteList.Count > 0)
                    //{
                    //    ObjUserManager.RemoveFromRoles(uid.Trim(), deleteList.ToArray());
                    //}
                    if (addList.Count > 0)
                    {
                        ObjUserManager.AddToRoles(uid.Trim(), addList.ToArray());
                    }

                    if (ddlUserRole.SelectedValue != "1") // Existing Role
                    {
                        user = umanager.FindById(uid);
                        if (user != null)
                        {
                            if (ddlUserRole.SelectedValue == "0")
                                user.UserRoleId = null;
                            else
                                user.UserRoleId = ddlUserRole.SelectedValue;

                            IdentityResult result = umanager.Update(user);
                        }
                    }
                }
            }
            aspxuserpopup.ShowOnPageLoad = false;
            BindAspxGridView();

        }

        protected void ASPxCallbackPanelGroupAdd_Callback(object sender, CallbackEventArgsBase e)
        {
            string value = userBoxList.GetValues();
            if (!string.IsNullOrEmpty(value))
            {
                List<string> roleList = ObjUserManager.GetUserGroups(value);
                rolesListBox.SetValues(string.Join(",", roleList));
            }
        }

        protected void HideButtonForChartViewAndOrg()
        {
            btnDeleteFromGroup.Visible = false;
            btnAddInGroup.Visible = false;
            btnDeleteFromSite.Visible = false;
            btnCreateUser.Visible = false;
            //lbCreateGroup.Visible = false;
            btndisableuser.Visible = false;
            btnChangeNameProcess.Visible = false;
        }

        private void BindRoles()
        {
            try
            {
                List<LandingPages> dtUserRole = landingPages.GetLandingPages().Where(x => x.Deleted == false).OrderBy(x => x.Name).ToList();
                if (dtUserRole != null && dtUserRole.Count > 0)
                {
                    ddlUserRole.DataTextField = "Name";
                    ddlUserRole.DataValueField = "Id";
                    ddlUserRole.DataSource = dtUserRole;
                    ddlUserRole.DataBind();
                }

                ddlUserRole.Items.Insert(0, new ListItem("(None)", "0"));
                ddlUserRole.Items.Insert(1, new ListItem("(Existing Role)", "1"));
                ddlUserRole.Items.FindByValue("1").Selected = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
public struct ResourceViews
{
    public const string ListView = "List View";
    public const string CardView = "Card View";
    public const string OrgView = "Org Chart";
}