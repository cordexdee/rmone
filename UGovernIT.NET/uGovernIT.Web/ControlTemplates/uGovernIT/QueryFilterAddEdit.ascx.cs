using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Text.RegularExpressions;

namespace uGovernIT.Web
{
    public partial class QueryFilterAddEdit : UserControl
    {
        List<WhereInfo> whereClauses;
        public int dashboardID { get; set; }
        public int filterID { get; set; }
        Dashboard uDashboard = null;
        DashboardQuery panel;
        List<Utility.ColumnInfo> columns;
        List<FactTableField> queryTableFields;
        ApplicationContext context = null;
        DashboardManager objDashboardManager = null;
        QueryHelperManager objQueryHelperManager = null;
        ConfigurationVariableManager objConfigurationVariableManager = null;
        UserProfileManager profileManager = null;
        List<Joins> joins;

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objDashboardManager = new DashboardManager(context);
            objQueryHelperManager = new QueryHelperManager(context);
            objConfigurationVariableManager = new ConfigurationVariableManager(context);
            profileManager = new UserProfileManager(context);
            
            
            if (dashboardID > 0)
            {
                uDashboard = objDashboardManager.LoadPanelById(dashboardID, false);
                panel = (DashboardQuery)uDashboard.panel;
                whereClauses = panel.QueryInfo.WhereClauses;
                if (whereClauses == null)
                {
                    whereClauses = new List<WhereInfo>();
                }

                joins = panel.QueryInfo.JoinList;
                if (joins == null)
                    joins = new List<Joins>();

                bool isUnion = false;

                if (joins != null && joins.Count > 0)
                    isUnion = joins.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

                List<string> columnsList = new List<string>();

                //columns = SelectColumns(panel.QueryInfo.Tables,false);
                
                List<Utility.ColumnInfo> columnsForDrpDown = SelectColumns(panel.QueryInfo.Tables,true);
            

                
                if (columnsForDrpDown != null)
                {
                    foreach (Utility.ColumnInfo c in columnsForDrpDown)
                    {
                        if (columnsList.Count > 0 && isUnion)
                        {
                            bool isColSelected = columnsList.Any(x => x.Contains(c.FieldName));

                            if (isColSelected)
                                continue;
                        }

                        string columnWithTableName = c.TableName + "." + c.FieldName + "(" + c.DataType + ")";

                        if (isUnion)
                            drpColumn.Items.Add(new ListItem(c.FieldName + "(" + c.DataType + ")", columnWithTableName));
                        else
                            drpColumn.Items.Add(new ListItem(columnWithTableName));

                        columnsList.Add(c.FieldName);
                    }
                }
                if (!IsPostBack && filterID > 0)
                {
                    WhereInfo wi = whereClauses.Where(m => m.ID == filterID).FirstOrDefault();
                    drpColumn.SelectedIndex = drpColumn.Items.IndexOf(drpColumn.Items.FindByText(wi.ColumnName + "(" + wi.DataType + ")"));
                    drpRelOpt.SelectedValue = wi.RelationOpt.ToString();
                    drpOpt.SelectedValue = wi.Operator.ToString();
                    drpValuetype.SelectedValue = wi.Valuetype.ToString();
                    ddlParameterType.SelectedValue = wi.ParameterType;
                    chkParamater.Checked = wi.ParameterRequired;

                    if (wi.DrpOptions != null)
                    {
                        txtDropdownOptions.Text = wi.DrpOptions.OptionsDropdown;
                        txtDropDownDefaultVal.Text = wi.DrpOptions.DropdownDefaultValue;
                        pDropdownProperties.Visible = true;
                        //pLookupProperties.Visible = true;

                    }

                    if (wi.LookupList != null)
                    {
                        FillLookupListsDropDown();
                        ddlLookupList.SelectedValue = wi.LookupList.LookupListName;
                        ddlLPModule.SelectedValue = wi.LookupList.LookupModuleName;
                        ddlLookupFields.SelectedValue = wi.LookupList.LookupField;
                        DDLLookupList_SelectedIndexChanged(ddlLookupList, e);
                        pLookupProperties.Visible = true;
                    }

                    List<int> ids = whereClauses.Select(m => m.ID).ToList();
                    if (wi.Valuetype == qValueType.Constant)
                    {
                        if (wi.DataType == "DateTime")
                        {
                            dtcValue.Date = Convert.ToDateTime(wi.Value);
                        }
                        else if (wi.DataType == "Boolean")
                        {
                            chkValue.Checked = UGITUtility.StringToBoolean(wi.Value);
                        }
                        else if (wi.DataType == "User" && !string.IsNullOrWhiteSpace(wi.Value))
                        {
                            if (wi.Value.ToLower() == "[$me$]")
                            {
                                dvCurrentUser.Visible = true;
                                chkCurrentUser.Checked = true;
                            }
                            else 
                            {
                                var guidMatch = Regex.Match(wi.Value, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}", RegexOptions.IgnoreCase);

                                if (guidMatch.Success)
                                {
                                    ppeValue.SetValues(wi.Value);
                                }
                                else 
                                {
                                    // load user if it is null then its a group
                                    UserProfile user = profileManager.GetUserByUserName(wi.Value);   //GetUserOrGroupName(wi.Value);

                                    if (user != null) 
                                    {
                                        ppeValue.SetValues(user.Id);
                                    }
                                    else 
                                    {
                                        Role group = profileManager.GetUserRoleByGroupName(wi.Value);
                                        if(group != null)
                                            ppeValue.SetValues(group.Id);
                                    }
                                }
                            }
                        }
                        
                    }
                    txtValue.Text = wi.Valuetype == qValueType.Parameter ? wi.ParameterName : wi.Value;
                    if (ids.Min() == wi.ID)
                    {
                        drpRelOpt.Enabled = false;
                        drpRelOpt.SelectedIndex = 0;
                    }

                    if (wi.Valuetype == qValueType.Variable)
                    {
                        DateVariableCtrl dCtrl = (DateVariableCtrl)dtVariable;
                        dCtrl.SetDateFunction = wi.Value;
                    }

                }
                else
                {
                    drpRelOpt.Enabled = (whereClauses.Count > 0);
                }

            }
            base.OnInit(e);
        }


        private void FillLookupListsDropDown()
        {
            if (ddlLookupList.Items.Count <= 0)
            {
                string lists = objConfigurationVariableManager.GetValue(ConfigurationVariable.ServiceLookupLists);
                string[] listArray = lists.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                listArray = listArray.OrderBy(x => x).ToArray();
                foreach (string item in listArray)
                {
                    ddlLookupList.Items.Add(new ListItem(item, item));
                }
                ddlLookupList.Items.Insert(0, new ListItem("--Select List--", ""));
            }
        }
        
        protected void DDLLookupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlLookupFields.Items.Clear();
            DataTable list = GetTableDataManager.GetTableData(ddlLookupList.SelectedValue, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (list != null)
            {
                var fields = list.Columns.Cast<DataColumn>();
                // List<SPField> selectedFields = fields.Where(x => !x.FromBaseType && !x.Hidden && !x.ReadOnlyField && (x.Type == SPFieldType.Text || x.Type == SPFieldType.Note || x.Type == SPFieldType.Choice)).OrderBy(x => x.Title).ToList();
                // Remove where condition from below line if could not find anystring type or choice type field.
                List<DataColumn> selectedFields = fields.Where(y=> !y.ReadOnly && (Convert.ToString(y.DataType) == "String" || Convert.ToString(y.DataType) == "Note" || Convert.ToString(y.DataType) == "Choice")).OrderBy(x => x.ColumnName).ToList();

                ddlLookupFields.Items.Add(new ListItem(DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Title));
                foreach (var field in selectedFields)
                {
                    ddlLookupFields.Items.Add(new ListItem(field.ColumnName, field.ColumnName));
                }

                DataTable moduleList = GetModulesInfoFromLookup(ddlLookupList.SelectedValue);
                lpModuletr.Visible = false;
                ddlLPModule.Items.Clear();
                
                if (moduleList.Rows.Count > 0)
                {
                    lpModuletr.Visible = true;
                    foreach (DataRow row in moduleList.Rows)
                    {
                        ddlLPModule.Items.Add(new ListItem(Convert.ToString(row["Name"]), Convert.ToString(row["Name"])));
                    }
                }
            }
        }
        private DataTable GetModulesInfoFromLookup(string lookupList)
        {
            DataTable modules = new DataTable();
            modules.Columns.Add("Name");
            DataTable lList = GetTableDataManager.GetTableData(lookupList, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            
            if (lList != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, lList))
            {
                DataTable table = lList;
                modules = table.DefaultView.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup);
                modules.Columns[DatabaseObjects.Columns.ModuleNameLookup].ColumnName = "Name";
            }
            return modules;
        }

        private List<FactTableField> GetColsListWithDataType(String queryTable)
        {
            return GetColsListWithDataType(queryTable, string.Empty);
        }

        private List<FactTableField> GetColsListWithDataType(string queryTable, string typeFilter)
        {
            DataColumnCollection fieldCollection = null;
            queryTableFields = new List<FactTableField>();
            
            if (queryTable != null && queryTable.Trim() != string.Empty)
            {
                queryTableFields = DashboardCache.GetFactTableFields(context, queryTable);
                if (queryTableFields == null)
                    queryTableFields = new List<FactTableField>();

                if (queryTableFields != null && queryTableFields.Count > 0)
                {
                    foreach (FactTableField fld in queryTableFields)
                    {
                        fld.FieldDisplayName = UGITUtility.AddSpaceBeforeWord(fld.FieldName);
                        if (fld.FieldDisplayName.EndsWith("$"))
                            fld.FieldDisplayName = Convert.ToString(fld.FieldDisplayName).Remove(Convert.ToString(fld.FieldDisplayName).Length - 1);
                        fld.DataType = fld.DataType.Replace("System.", "");
                    }
                }
                else
                {
                    queryTableFields = new List<FactTableField>();
                    DataTable inputList = GetTableDataManager.GetTableData(queryTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                    if (inputList != null)
                    {
                        fieldCollection = inputList.Columns;

                        foreach (DataColumn field in fieldCollection)
                        {
                            if (!field.ReadOnly || (Convert.ToString(field.DataType) == "Lookup"))
                            {
                                queryTableFields.Add(new FactTableField(queryTable, field.ColumnName, QueryHelperManager.GetStandardDataType(Convert.ToString(field.DataType)), field.ColumnName));
                            }
                        }
                    }
                }
            }
            return queryTableFields;
        }

        private List<Utility.ColumnInfo> SelectColumns(List<TableInfo> tables, bool sortByFieldName)
        {
            List<FactTableField> columnData = new List<FactTableField>();
            //int sque = 0;
            int Id = 0;
            columns = new List<Utility.ColumnInfo>();
            if (tables == null) return columns;
            ModuleViewManager objModuleViewManager = new ModuleViewManager(context);
            foreach (TableInfo table in tables)
            {
                columnData = GetColsListWithDataType(table.Name);
                string moduleTable = objModuleViewManager.GetModuleByTableName(table.Name);
                moduleTable = table.Name.Split('-')[0];
                moduleTable = objModuleViewManager.GetModuleTableName(moduleTable);
                if (!string.IsNullOrEmpty(moduleTable))
                {
                    columnData.RemoveAll(x => x.FieldName.EndsWith("$Id"));
                }
                if (columnData != null)
                {
                    columnData = columnData.Where(cd => cd.FieldName !=DatabaseObjects.Columns.TenantID).OrderBy(cd => cd.FieldName).ToList();
                    if (table.Name == "PMMProjects")
                    {
                        columnData.Add(new FactTableField("PMMProjects", "ProjectHealth", "none", "ProjectHealth"));
                    }
                    foreach (FactTableField item in columnData)
                    {
                        bool selected = table.Columns.Exists(c => c.FieldName == item.FieldName);

                        if (selected)
                        {
                            var column = table.Columns.Find(c => c.FieldName == item.FieldName);
                            column.ID = ++Id;
                            columns.Add(column);
                            continue;
                        }
                        columns.Add(new Utility.ColumnInfo
                        {
                            ID = ++Id,
                            FieldName = item.FieldName,
                            DisplayName = item.FieldDisplayName,
                            DataType = item.DataType,
                            Function = "none",
                            TableName = table.Name,
                            Selected = selected
                        });
                    }
                }
            }

            //changing order of 'TableName' & Sequence to solve grouping issue on columns tab.
            if(sortByFieldName)
            {
                columns = columns.OrderByDescending(c => c.Selected)
                          .ThenBy(c => c.TableName)
                           .ThenBy(c => c.FieldName)
                          .ThenBy(c => c.Sequence)
                          .ToList();
            }
            else
            {
                columns = columns.OrderByDescending(c => c.Selected)
                           .ThenBy(c => c.TableName)
                           .ThenBy(c => c.Sequence)
                           .ThenBy(c => c.FieldName)

                           .ToList();
                //foreach (var ci in columns)
                //{
                //    ci.Sequence = ++sque;
                //}
            }
            return columns;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (drpValuetype.SelectedValue == "Parameter" && string.IsNullOrEmpty(txtValue.Text))
            {
                lbl_Value.Style.Remove("display");
                return;
            }
            else
            {
                lbl_Value.Style.Add("display", "block");
            }

            AddWhereClause();
            Context.Cache.Add(string.Format("QUERYFILTER-{0}", context.CurrentUser.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void AddWhereClause()
        {
            string columnName = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[0];
            string DataType = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[1];
            OperatorType optType = (OperatorType)Enum.Parse(typeof(OperatorType), drpOpt.SelectedValue);
            RelationalOperator RelationOpt = (RelationalOperator)Enum.Parse(typeof(RelationalOperator), drpRelOpt.SelectedValue);
            qValueType vtype = (qValueType)Enum.Parse(typeof(qValueType), drpValuetype.SelectedValue);

            string columnType = string.Empty;

            if (columnName.Contains('.'))
            {
                string tableName = columnName.Split('.')[0];
                string fieldName = columnName.Split('.')[1];
                columnType = columns.Where(x => x.TableName == tableName && x.FieldName == fieldName).Select(y => y.DataType).FirstOrDefault();
            }
            else
            {
                columnType = columns.Where(x => x.FieldName == columnName).Select(y => y.DataType).FirstOrDefault();
            }

            string value = txtValue.Text;
            string parameterType = ddlParameterType.SelectedItem.Text;
            int Id = 1;
            if (whereClauses != null && whereClauses.Count > 0)
                Id = whereClauses.Max(x => x.ID) + 1;

            FilterOptionsDropdown optDropdown = null;
            FilterLookupList lookUpList = null;
            if (vtype == qValueType.Parameter)
            {
                if (parameterType == "DropDown")
                {
                    optDropdown = new FilterOptionsDropdown();
                    optDropdown.DropdownDefaultValue = txtDropDownDefaultVal.Text;
                    optDropdown.OptionsDropdown = txtDropdownOptions.Text;
                }
                else if (parameterType == "Lookup")
                {
                    lookUpList = new FilterLookupList();
                    lookUpList.LookupListName = ddlLookupList.SelectedValue;
                    lookUpList.LookupModuleName = ddlLPModule.SelectedValue;
                    lookUpList.LookupField = ddlLookupFields.SelectedValue;
                }
            }
            var dataType = DataType.Replace("System.", "");
            if (vtype == qValueType.Constant)
            {
                switch (dataType)
                {
                    case "Number":
                    case "Counter":
                    case "Currency":
                        value = txtValue.Text;
                        break;
                    case "DateTime":
                        value = Convert.ToString(dtcValue.Date);
                        break;
                    case "Boolean":
                        value = chkValue.Checked.ToString();
                        break;
                    case "User":
                        if (chkCurrentUser.Checked)
                        {
                            value = "[$me$]"; // current logged in user at runtime
                        }
                        else
                        {
                            value = string.Empty;

                            List<UserProfile> usrVals = profileManager.GetUserInfosById(ppeValue.GetValues());
                            if (usrVals != null && usrVals.Count > 0)
                            {
                                // set name to value because we want to show name in query instead of user/group id // Convert.ToString(usrVals[0].Id);
                                if (!string.IsNullOrEmpty(columnType) && columnType == "User")
                                {
                                    // check if it is a user or a group
                                    if (usrVals[0].Id != null && !string.IsNullOrEmpty(usrVals[0].UserName))
                                    {
                                        value = usrVals[0].UserName;
                                    }
                                    else
                                    {
                                        List<Role> usrGroupVals = profileManager.GetUserGroupById(ppeValue.GetValues());
                                        if (usrGroupVals != null && usrGroupVals.Count > 0)
                                            value = usrGroupVals[0].Name;
                                    }
                                }
                                else
                                {
                                    value = Convert.ToString(usrVals[0].Id);
                                }
                            }
                        }
                       
                        break;
                    default:
                        value = txtValue.Text;
                        break;
                }
            }
            else if (vtype == qValueType.Variable)
            {
                DateVariableCtrl dCtrl = (DateVariableCtrl)dtVariable;
                value = dCtrl.GetDateFunction;
            }
            else
            {
                value = txtValue.Text;
            }

            if (filterID > 0)
            {
                whereClauses.Where(w => w.ID == filterID).ToList().
                          ForEach(wi =>
                          {
                              wi.DataType = DataType;
                              wi.ColumnName = columnName;
                              wi.Operator = optType;
                              wi.RelationOpt = Id == 1 ? RelationalOperator.None : RelationOpt;
                              wi.Valuetype = vtype;
                              // wi.Value = ((vtype == qValueType.Constant || vtype == qValueType.Variable) ? (value.Contains("me")?SPContext.Current.Web.CurrentUser.Name:value) : "");
                              wi.Value = ((vtype == qValueType.Constant || vtype == qValueType.Variable) ? (value) : "");
                              wi.ParameterName = (vtype == qValueType.Parameter ? value : "");
                              wi.ParameterType = parameterType;
                              wi.ParameterRequired = chkParamater.Checked;
                              wi.LookupList = lookUpList;
                              wi.DrpOptions = optDropdown;
                              wi.TableName = columnName.Contains('.') ? columnName.Split('.')[0] : drpColumn.SelectedValue.Split('.')[0];
                          });
                panel.QueryInfo.WhereClauses = whereClauses;
            }
            else
            {
                whereClauses.Add(new WhereInfo
                {
                    ID = Id,
                    DataType = DataType,
                    ColumnName = columnName,
                    Operator = optType,
                    RelationOpt = whereClauses.Count == 0 ? RelationalOperator.None : RelationOpt,
                    Valuetype = vtype,
                    ParameterType = parameterType,
                    Value = ((vtype == qValueType.Constant || vtype == qValueType.Variable) ? value : ""),
                    ParameterName = (vtype == qValueType.Parameter ? value : ""),
                    ParameterRequired = chkParamater.Checked,
                    LookupList = lookUpList,
                    DrpOptions = optDropdown,
                    TableName = columnName.Contains('.') ? columnName.Split('.')[0] : drpColumn.SelectedValue.Split('.')[0]
                });
            }
            panel.QueryInfo.WhereClauses = whereClauses;
            SaveQueryData();
        }

        public void SaveQueryData()
        {
            panel.QueryInfo.WhereClauses = whereClauses;
            uDashboard.panel = panel;
            objDashboardManager.SaveDashboardPanel(new byte[0],"",false,uDashboard);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVal = ddlParameterType.SelectedValue;
            switch (selectedVal)
            {
                case "DropDown":
                    pDropdownProperties.Visible = true;
                    pLookupProperties.Visible = false;
                    break;
                case "Lookup":
                    pDropdownProperties.Visible = false;
                    pLookupProperties.Visible = true;
                    ddlLookupList.Items.Clear();
                    FillLookupListsDropDown();
                    break;
                default:
                    pDropdownProperties.Visible = false;
                    pLookupProperties.Visible = false;
                    break;
            }

        }

        protected void drpValuetype_Load(object sender, EventArgs e)
        {

        }

    }
}
