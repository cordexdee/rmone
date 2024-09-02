using DevExpress.Web;
using DevExpress.Web.Data;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:UserValueBox runat=server></{0}:UserValueBox>")]
    public class UserValueBox : UGITControl
    {
        //public ASPxGridLookup UserTokenBoxAdd { get; set; }
        public ASPxTokenBox UserTokenBoxAdd { get; set; }

        public bool isMulti { get; set; }
        public bool IsNotPostBack { get; set; }
        public bool IsMandatory { get; set; }
        public bool HideCurrentUser { get; set; }
        UserProfileManager UserManager;
        FieldConfigurationManager fieldManager;
        FieldConfiguration field;
        public string UserType { get; set; }
        public string SelectionSet { get; set; }
        public bool HideServiceAccountUsers { get; set; }
        public string FilterExpression { get; set; }
        public string ValidationGroup { get; set; }
        private string TenantID = TenantHelper.GetTanantID();
        DevExpress.Web.ASPxTokenBox grid = null;
        //private string TenantID = string.Empty;


        ApplicationContext context = HttpContext.Current.GetManagerContext();
        bool isSuperAdmin;

        public UserValueBox()
        {
            isMulti = false;
            UserTokenBoxAdd = new ASPxTokenBox();
            SelectionSet = "";

            UserTokenBoxAdd.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            UserTokenBoxAdd.ShowDropDownOnFocus = ShowDropDownOnFocusMode.Always;
            UserTokenBoxAdd.AllowCustomTokens = false;
            UserTokenBoxAdd.ItemStyle.CssClass = " aspxTokenBox-item";

            //UserTokenBoxAdd.Style.Add("min-width", "100px");
            //UserTokenBoxAdd.Style.Add("max-width", "300px");
            ////UserTokenBoxAdd.ClientSideEvents.DropDown = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";
            ////UserTokenBoxAdd.ClientSideEvents.GotFocus = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";
            ////UserTokenBoxAdd.ClientSideEvents.Init = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";

        }

        protected override void OnInit(System.EventArgs e)
        {
            //TenantID = Convert.ToString(Context.Session["TenantID"]);
            this.UserTokenBoxAdd.ID = this.ID + "LookupSearchValue";
            this.UserTokenBoxAdd.ClientInstanceName = FieldName + "LookupSearchValue";
            this.UserTokenBoxAdd.ClientIDMode = ClientIDMode.Static;
            this.UserTokenBoxAdd.CssClass += " aspxUserTokenBox-control";
            this.UserTokenBoxAdd.SettingsLoadingPanel.Enabled = false;
            this.UserTokenBoxAdd.TokenStyle.CssClass = " tokenBoxStyle";
            //this.UserTokenBoxAdd.CssClass = "text-left all-input form-control btn btn-default dropdown-toggle bg-light-blue close_arrow_edit"; //Remove 'down-arrow' class to align down arrow properly

            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            //DataTable dt = null;

            if (FieldName != null)
            {
                fieldManager = new FieldConfigurationManager(context);
                field = fieldManager.GetFieldByFieldName(FieldName);
                isMulti = field.Multi;
                SelectionSet = field.SelectionSet;
            }

            this.UserTokenBoxAdd.DisplayFormatString = "{0}";
            this.UserTokenBoxAdd.AllowUserInput = true;


            this.UserTokenBoxAdd.IncrementalFilteringMode = IncrementalFilteringMode.Contains;

            this.UserTokenBoxAdd.Load += UserTokenBoxAdd_Load;
            this.UserTokenBoxAdd.ItemTemplate = new LookupTemplate(this.UserTokenBoxAdd.ClientID);

            if (field != null && !string.IsNullOrEmpty(field.Width))
            {
                UserTokenBoxAdd.Width = UGITUtility.StringToUnit(field.Width);
            }
            if (isMulti)
            {
                ////this.UserTokenBoxAdd.SelectionMode = GridLookupSelectionMode.Multiple;       

            }
            else
            {
                UserTokenBoxAdd.ClientSideEvents.ValueChanged = "SingleSelectTokenBox";
                UserTokenBoxAdd.ClientSideEvents.TokensChanged = @"function (s, e) { setTimeout(function() { s.GetInputElement().blur(); }, 200);}";
            }

            if (IsMandatory && !string.IsNullOrEmpty(ValidationGroup))
            {
                this.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
                this.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                this.UserTokenBoxAdd.ValidationSettings.ErrorText = "Value Cannot be Null";
                this.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Value Cannot be Null";
                this.UserTokenBoxAdd.ValidationSettings.ValidationGroup = ValidationGroup;
            }

            this.UserTokenBoxAdd.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;

            Controls.Add(UserTokenBoxAdd);
            base.OnInit(e);
        }

        private string SuperAdminFilter()
        {
            isSuperAdmin = context.UserManager.IsUGITSuperAdmin(context.CurrentUser);

            if (!isSuperAdmin)
            {
                return DatabaseObjects.Columns.Name + "<>'UGIT Super Admin'";
            }
            else
            {
                return string.Empty;
            }
        }

        private void UserTokenBoxAdd_Load(object sender, EventArgs e)
        {
            //DevExpress.Web.ASPxGridLookup grid = sender as DevExpress.Web.ASPxGridLookup;
            grid = sender as DevExpress.Web.ASPxTokenBox;

            if (!string.IsNullOrEmpty(UserType))
            {
                SelectionSet = UserType;
            }
            DataTable dt = new DataTable();
            if (SelectionSet == "User")
            {
                //dt = UGITUtility.ToDataTable<UserProfile>(UserManager.GetUsersProfileWithGroup().Where(x => !x.isRole && x.TenantID == TenantID && x.Enabled).ToList());
                //var userProfiles = UserManager.Load(x => x.Enabled && !x.UserName.EqualsIgnoreCase("SuperAdmin"));
                //dt = UGITUtility.ToDataTable<UserProfile>(userProfiles);
                dt = (DataTable)CacheHelper<object>.Get($"AspNetUsers{context.TenantID}", context.TenantID);
                if (dt == null)
                {
                    dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    CacheHelper<object>.AddOrUpdate($"AspNetUsers{context.TenantID}", context.TenantID, dt);
                }
                string query = string.Format("{0} = {1} And {2} <> {3}", DatabaseObjects.Columns.Enabled, true, DatabaseObjects.Columns.UserName, "'SuperAdmin'");
                DataView dv = new DataView(dt);
                dv.RowFilter = query; // query
                dt = dv.ToTable();
            }
            if(SelectionSet == "UserOnly")
            {
                dt = (DataTable)CacheHelper<object>.Get($"AspNetUsers{context.TenantID}", context.TenantID);
                if (dt == null)
                {
                    dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    CacheHelper<object>.AddOrUpdate($"AspNetUsers{context.TenantID}", context.TenantID, dt);
                }
                string query = string.Format("{0} = {1} And {2} <> {3} And {4} <> {5}", DatabaseObjects.Columns.Enabled, true, DatabaseObjects.Columns.UserName, 
                    "'SuperAdmin'", DatabaseObjects.Columns.isRole, true);
                DataView dv = new DataView(dt);
                dv.RowFilter = query; // query
                dt = dv.ToTable();
            }
            else if (SelectionSet == "Group")
            {
                dt = UGITUtility.ToDataTable<Role>(UserManager.GetUserGroups());
                dt.Columns["Name"].ColumnName = "UserName";
                dt.Columns["Title"].ColumnName = "Name";
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.LocationLookup));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.DepartmentID));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.JobProfile));
                dt.Columns.Add(new DataColumn(DatabaseObjects.Columns.isRole));
            }
            else if (SelectionSet == "User,Manager")
            {
                //dt = UGITUtility.ToDataTable<UserProfile>(UserManager.GetUsersProfileWithGroup().Where(x => !x.isRole && x.IsManager && x.Enabled).ToList());

                var userProfiles = UserManager.Load(x => x.IsManager && x.Enabled && !x.UserName.EqualsIgnoreCase("SuperAdmin"));
                dt = UGITUtility.ToDataTable<UserProfile>(userProfiles);
            }
            else
            {
                ////dt = UGITUtility.ToDataTable<UserProfile>(UserManager.GetUsersProfileWithGroup().Where(x => x.TenantID == TenantID && (x.Enabled || x.isRole)).ToList());
                //dt = UGITUtility.ToDataTable<UserProfile>(UserManager.GetUsersProfileWithGroup().Where(x => !x.isRole && x.TenantID == TenantID && x.Enabled).ToList());

                string cacheName = "UserProfile_" + context.TenantID;

                //if (CacheHelper<object>.IsExists(cacheName))
                //    dt = CacheHelper<object>.Get(cacheName) as DataTable;
                //else
                //{
                var userProfiles = UserManager.Load(x => x.Enabled && !x.UserName.EqualsIgnoreCase("SuperAdmin"));
                dt = UGITUtility.ToDataTable<UserProfile>(userProfiles);

                //var dtUserGroup = UserManager.GetUserGroups().Where(x => x.TenantID == TenantID && x.IsSystem == true).ToList();
                var dtUserGroup = UserManager.GetUserGroups().Where(x => x.TenantID.Equals(TenantID, StringComparison.InvariantCultureIgnoreCase)).ToList();
                DataRow row = null;
                foreach (var userGroup in dtUserGroup)
                {
                    row = dt.NewRow();
                    row["Id"] = userGroup.Id;
                    row["Name"] = userGroup.Title;
                    row["UserName"] = userGroup.Name;
                    row["TenantID"] = userGroup.TenantID;

                    dt.Rows.Add(row);
                }

                CacheHelper<object>.AddOrUpdate(cacheName, dt);
                //}
            }
            //Delegated task should not assign to theirselves by editing own profile.

            if (SelectionSet == "User" && HideCurrentUser && !context.UserManager.IsAdmin(context.CurrentUser))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(context.CurrentUser)))
                {
                    if (!string.IsNullOrEmpty(FilterExpression))

                        FilterExpression = FilterExpression + "and" + DatabaseObjects.Columns.UserName + "<>" + "'" + context.CurrentUser.UserName + "'";

                    else
                        FilterExpression = DatabaseObjects.Columns.UserName + "<>" + "'" + context.CurrentUser.UserName + "'";
                }
            }
            else if (!string.IsNullOrEmpty(FilterExpression))
                FilterExpression = FilterExpression + " and " + SuperAdminFilter();
            else
                FilterExpression = SuperAdminFilter();

            if (dt != null && !string.IsNullOrEmpty(FilterExpression))
            {
                DataRow[] dr = dt.Select(FilterExpression);
                if (dr.Count() > 0)
                    dt = dr.CopyToDataTable();
                else
                    dt = dt.Clone();
            }

            ////string controlID = grid.Attributes["hiddenFieldValues"];
            ////string filterFieldValues = grid.Attributes["filterFieldValues"];
            ////HttpCookie cook = HttpContext.Current.Request.Cookies[controlID];

            ////if (!string.IsNullOrEmpty(controlID))
            ////{
            ////    HiddenField hiddenFields = this.FindControlIterative(controlID) as HiddenField;
            ////    string filterText = hiddenFields.Value;

            ////    if (!string.IsNullOrEmpty(filterText))
            ////    {
            ////        dt = dt.Select(string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.Name, filterText)).CopyToDataTable();
            ////    }
            ////}
            if (HideServiceAccountUsers)
            {
                ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
                bool HideServiceAccountUsersConfigVar = configurationVariableManager.GetValueAsBool(ConfigConstants.HideServiceAccountUsers);
                if (HideServiceAccountUsersConfigVar && dt.Columns.Contains(DatabaseObjects.Columns.IsServiceAccount))
                {
                    DataView dvUser = new DataView(dt);
                    dvUser.RowFilter = string.Format("{0} is null OR {0} = {1}", DatabaseObjects.Columns.IsServiceAccount, false); // query
                    dt = dvUser.ToTable();
                }
            }
            UserTokenBoxAdd.DataSource = dt;
            UserTokenBoxAdd.ValueField = "Id";
            UserTokenBoxAdd.TextField = "Name";
            UserTokenBoxAdd.DataBind();
        }

        public string GetText()
        {
            string value = "";
            value = Convert.ToString(UserTokenBoxAdd.Text.Trim());
            if (isMulti)
            {
                List<string> valuelist = UGITUtility.ConvertStringToList(UserTokenBoxAdd.Text, ",");
                valuelist = valuelist.Select(x => x.Trim()).ToList();
                value = string.Join(",", valuelist);
            }
            return value;
        }

        public string GetValues()
        {
            string value = "";
            /*
            if (UserTokenBoxAdd.SelectionMode == GridLookupSelectionMode.Multiple)
            {
                value = string.Join(",", UserTokenBoxAdd.GridView.GetSelectedFieldValues(UserTokenBoxAdd.KeyFieldName).ToList());
            }
            else
            {
                value = Convert.ToString(UserTokenBoxAdd.Value);
            }
            */

            value = Convert.ToString(UserTokenBoxAdd.Value);

            return value;
        }

        public List<string> GetValuesAsList()
        {
            List<string> values = new List<string>();
            if (!string.IsNullOrEmpty(Convert.ToString(UserTokenBoxAdd.Value)))
            {
                values = UGITUtility.ConvertStringToList(Convert.ToString(UserTokenBoxAdd.Value), ", ");
                return values;
            }
            else
            {
                return values;
            }
        }

        public List<string> GetTextsAsList()
        {
            List<string> values = UGITUtility.ConvertStringToList(UserTokenBoxAdd.Text, ", ");
            return values;
        }

        public void SetValues(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                /*
                string[] keyList = value.Split(',');
                if (keyList.Count() > 1)
                {
                    UserTokenBoxAdd.SelectionMode = GridLookupSelectionMode.Multiple;
                }
                foreach (string key in keyList)
                {
                    UserTokenBoxAdd.GridView.Selection.SelectRowByKey(key);
                }
                */
                UserTokenBoxAdd.Value = value;
            }
            else
            {
                UserTokenBoxAdd.Text = "";
            }
        }

        public void SetText(string value)
        {
            UserTokenBoxAdd.Text = value;
        }
    }

    public class LookupTemplate : IBindableTemplate
    {
        private string gridLookupID = string.Empty;
        private readonly string _tenantID = TenantHelper.GetTanantID();

        public LookupTemplate(string id)
        {
            gridLookupID = id;
        }

        public void InstantiateIn(Control container)
        {
            //GridViewDataRowTemplateContainer tContainer = container as GridViewDataRowTemplateContainer;
            //WebDataRow rowView = tContainer.DataItem as WebDataRow;

            ListEditItemTemplateContainer tContainer = container as ListEditItemTemplateContainer;

            if (tContainer.DataItem.GetType().FullName != "System.Data.DataRowView")
            {
                return;
            }

            DataRow rowView = ((System.Data.DataRowView)tContainer.DataItem).Row;

            var jbLocation = new StringBuilder();
            //if (rowView.Table.Columns.Contains(DatabaseObjects.Columns.LocationLookup))
            //{
            if (!string.IsNullOrEmpty(Convert.ToString(rowView[DatabaseObjects.Columns.LocationLookup])))
            {
                //DataTable managervalue = GetTableDataManager.GetTableData("Location", "id=" + rowView["Location"].ToString());
                //jbLocation += managervalue.Rows.Count > 0 ? Convert.ToString(managervalue.Rows[0]["Title"].ToString()) : null;

                var managerValue = GetTableDataManager.GetSingleValueByIdFromCache(DatabaseObjects.Tables.Location, Convert.ToString(rowView[DatabaseObjects.Columns.LocationLookup]), _tenantID);

                if (!string.IsNullOrEmpty(managerValue))
                    jbLocation.Append(managerValue);
            }
            //}
            //if (!string.IsNullOrEmpty(jbLocation))
            if (jbLocation.Length > 0)
            {
                if (!string.IsNullOrEmpty(rowView["DepartmentId"].ToString()))
                {
                    //DataTable managervalue = GetTableDataManager.GetTableData("Department", "id=" + Convert.ToString(rowView["DepartmentId"]));
                    //jbLocation += managervalue.Rows.Count > 0 ? "," + Convert.ToString(managervalue.Rows[0]["Title"].ToString()) : "";

                    var managerValue = GetTableDataManager.GetSingleValueByIdFromCache(DatabaseObjects.Tables.Department, Convert.ToString(rowView["DepartmentId"]), _tenantID);

                    if (!string.IsNullOrEmpty(managerValue))
                        jbLocation.AppendFormat(",{0}", managerValue);
                }
            }
            else
            {
                //DataTable managervalue;
                if (Convert.ToString(rowView["DepartmentId"]) != "")
                {
                    //var managervalue = GetTableDataManager.GetTableData("Department", "id=" + Convert.ToString(rowView["DepartmentId"]));
                    var managerValue = GetTableDataManager.GetSingleValueByIdFromCache(DatabaseObjects.Tables.Department, Convert.ToString(rowView["DepartmentId"]), _tenantID);

                    if (!string.IsNullOrEmpty(managerValue))
                        jbLocation.Append(managerValue);
                }
                //else
                //{
                //    managervalue = GetTableDataManager.GetTableData("Department", "id=0");
                //}

                ////DataTable managervalue = GetTableDataManager.GetTableData("Department");
                //jbLocation += managervalue.Rows.Count > 0 ? Convert.ToString(managervalue.Rows[0]["Title"].ToString()) : "";    
            }

            //if (!string.IsNullOrEmpty(jbLocation))
            if (jbLocation.Length > 0)
            {
                //jbLocation += "<br/>";
                jbLocation.Append("<br/>");
            }

            List<string> listValue = new List<string>();
            listValue.Add(Convert.ToString(rowView[DatabaseObjects.Columns.Name] + " [" + rowView[DatabaseObjects.Columns.UserName] + "]"));

            if (!string.IsNullOrEmpty(Convert.ToString(rowView[DatabaseObjects.Columns.JobProfile])))
                listValue.Add(Convert.ToString(rowView[DatabaseObjects.Columns.JobProfile]));

            //if (!string.IsNullOrEmpty(jbLocation))
            if (jbLocation.Length > 0)
                listValue.Add(jbLocation.ToString());

            string data = string.Format(@"<table style='width:200px; overflow:hidden; float:left;'>
                                       <tr><td style='' valign='middle'><div style='padding:7px 3px; '>{0}</div></td></tr>
                                       </table>", string.Join(",", listValue.ToArray()));

            bool isRole = UGITUtility.StringToBoolean(rowView[DatabaseObjects.Columns.isRole]);
            //ASPxImage i = new ASPxImage() { ImageUrl = !isRole ? Convert.ToString(rowView[DatabaseObjects.Columns.Picture]) : "/content/images/group.png" };
            //i.Style.Add("float", "left"); i.Style.Add("padding", "3px");             
            //i.Height = 50;
            //i.Width = 50;
            LiteralControl ctd = new LiteralControl(data);
            ctd.EnableViewState = false;
            LiteralControl ct = new LiteralControl("<hr style='margin:2px; padding:0px; display:none;'/>");
            ct.EnableViewState = false;
            //ASPxButton btn = new ASPxButton();
            //btn.ID = "CloseBtn";
            //btn.AutoPostBack = false;
            //btn.Text = "Close";
            //btn.ClientSideEvents.Click = "function(s,e){" + gridLookupID + ".ConfirmCurrentSelection(); " + gridLookupID + ".HideDropDown();}";
            container.Controls.Add(ctd);
            container.Controls.Add(ct);
            //container.Controls.Add(btn);
        }

        public IOrderedDictionary ExtractValues(Control container)
        {
            return new OrderedDictionary(); //throw new NotImplementedException();
        }
    }
}