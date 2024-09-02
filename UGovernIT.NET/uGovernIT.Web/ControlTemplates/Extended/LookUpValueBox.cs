using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using uGovernIT.Utility;
using System.Data;
using DevExpress.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Linq.Expressions;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:LookUpValueBox runat=server></{0}:LookUpValueBox>")]
    public class LookUpValueBox : UGITControl
    {
        public ASPxGridLookup devexListBox { get; set; }
        FieldConfigurationManager fieldManager;
        public string ModuleName { get; set; }
        FieldConfiguration field;
        public string Value { get; set; }
        /// <summary>
        /// Allow for Multiple Selection Eq. IsMulti=true, IsMulti=false;
        /// </summary>
        public bool IsMulti { get; set; }
        public bool AllowNull { get; set; }
        public bool AllowVaries { get; set; }
        public bool Paging { get; set; }
        public int PageSize { get; set; }
        public ControlMode Mode { get; set; }
        public string ClientInstanceName { get; set; }
        public bool IsSearch { get; set; }
        public List<CustomCollumn> customCollumns { get; set; }
        protected const string Varies = "<Value Varies>";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ModuleViewManager _moduleViewManager = null;
        private RelatedCompanyManager _relatedCompanyManager = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        public bool IsMandatory { get; set; }
        public bool ShowSelectAllCheckbox { get; set; }
        public bool Adddefaultvalue { get; set; }
        public string ValidationGroup { get; set; }

        public string deletedChoices = string.Empty;
        public string FilterClosedFields = string.Empty;
        /// <summary>
        /// Insert comment for filter in datatable eq. DataTable.Select("ID=4")
        /// DataTable.Select(string expression)
        /// </summary>
        public string FilterExpression { get; set; }
        private DataTable data;
        DevExpress.Web.ASPxGridLookup gridview = null;

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

        protected RelatedCompanyManager RelatedCompanyManager
        {
            get
            {
                if (_relatedCompanyManager == null)
                {
                    _relatedCompanyManager = new RelatedCompanyManager(HttpContext.Current.GetManagerContext());
                }
                return _relatedCompanyManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                }
                return _configurationVariableManager;
            }
        }

        public LookUpValueBox()
        {
            ModuleName = "";
            IsMulti = false;
            ShowSelectAllCheckbox = true;
            FieldName = "";

            devexListBox = new ASPxGridLookup();

            devexListBox.DataBinding += DevexListBox_DataBinding;
            devexListBox.GridView.CustomCallback += new ASPxGridViewCustomCallbackEventHandler(GridViewCustomCallback);
            devexListBox.GridView.SettingsBehavior.EnableRowHotTrack = true;
            devexListBox.GridViewStyles.Row.CssClass = "lookupValueBox-drpDwnRow";
            //devexListBox.Style.Add("min-width", "100px");
            //devexListBox.Style.Add("max-width", "300px");
            devexListBox.AutoPostBack = false;
            if (!Paging)
            {
                devexListBox.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                devexListBox.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                devexListBox.GridViewStyles.Row.CssClass = "lookupValueBox-drpDwnRow";
                devexListBox.ClientSideEvents.Init = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";
                //devexListBox.ClientSideEvents.GotFocus = "function(s,e){ s.GetGridView().SetWidth(s.GetWidth()); var height= $('#'+s.GetGridView().mainElement.id+'_DXMainTable').height(); if(height<200 && ($('#'+s.mainElement.id+'_DDD_PW-1').height()>(height+2))){$('#'+s.mainElement.id+'_DDD_PW-1')[0].style.top=$('#'+s.mainElement.id+'_DDD_PW-1').position().top+(200-height)+'px'; s.GetGridView().SetHeight(height); }   var oldgrdHeight;var assignOldHeight=true;var offSetHeight=$('#'+s.mainElement.id).offset().top;var docHeight=Math.max(0, document.documentElement.clientHeight); var bottomHeight=docHeight-offSetHeight; var grdHeight=s.GetGridView().GetHeight();if(assignOldHeight){oldgrdHeight=grdHeight;assignOldHeight=false;} if(oldgrdHeight>grdHeight){grdHeight=oldgrdHeight;}var settableHeight=offSetHeight;if(offSetHeight<bottomHeight){settableHeight=bottomHeight;}if(grdHeight<settableHeight){settableHeight=grdHeight;}else {settableHeight=settableHeight-40;}  s.GetGridView().SetHeight(settableHeight);s.GetGridView().Refresh();}";
                //devexListBox.ClientSideEvents.DropDown = "function(s,e){ var height= $('#'+s.GetGridView().mainElement.id+'_DXMainTable').height(); if(height<200 && ($('#'+s.mainElement.id+'_DDD_PW-1').height()>(height+2))){$('#'+s.mainElement.id+'_DDD_PW-1')[0].style.top=$('#'+s.mainElement.id+'_DDD_PW-1').position().top+(200-height)+'px'; s.GetGridView().SetHeight(height); }   var oldgrdHeight;var assignOldHeight=true;var offSetHeight=$('#'+s.mainElement.id).offset().top;var docHeight=Math.max(0, document.documentElement.clientHeight); var bottomHeight=docHeight-offSetHeight; var grdHeight=s.GetGridView().GetHeight();if(assignOldHeight){oldgrdHeight=grdHeight;assignOldHeight=false;} if(oldgrdHeight>grdHeight){grdHeight=oldgrdHeight;}var settableHeight=offSetHeight;if(offSetHeight<bottomHeight){settableHeight=bottomHeight;}if(grdHeight<settableHeight){settableHeight=grdHeight;}else {settableHeight=settableHeight-40;}  s.GetGridView().SetHeight(settableHeight);  s.GetGridView().SetWidth(s.GetWidth());}";
                devexListBox.ClientSideEvents.DropDown = "function(s,e){var oldgrdHeight;var assignOldHeight=true;var offSetHeight=$('#'+s.mainElement.id).offset().top;var docHeight=Math.max(0, document.documentElement.clientHeight); var bottomHeight=docHeight-offSetHeight; var grdHeight=s.GetGridView().GetHeight();if(assignOldHeight){oldgrdHeight=grdHeight;assignOldHeight=false;} if(oldgrdHeight>grdHeight){grdHeight=oldgrdHeight;}var settableHeight=offSetHeight;if(offSetHeight<bottomHeight){settableHeight=bottomHeight;}if(grdHeight<settableHeight){settableHeight=grdHeight;}else {settableHeight=settableHeight-40;}  s.GetGridView().SetHeight(settableHeight);  s.GetGridView().SetWidth(s.GetWidth());}";
                //devexListBox.ClientSideEvents.GotFocus = "function(s,e){var oldgrdHeight;var assignOldHeight=true;var offSetHeight=$('#'+s.mainElement.id).offset().top;var docHeight=Math.max(0, document.documentElement.clientHeight); var bottomHeight=docHeight-offSetHeight; var grdHeight=s.GetGridView().GetHeight();if(assignOldHeight){oldgrdHeight=grdHeight;assignOldHeight=false;} if(oldgrdHeight>grdHeight){grdHeight=oldgrdHeight;}var settableHeight=offSetHeight;if(offSetHeight<bottomHeight){settableHeight=bottomHeight;}if(grdHeight<settableHeight){settableHeight=grdHeight;}else {settableHeight=settableHeight-40;}  s.GetGridView().SetHeight(settableHeight);  s.GetGridView().SetWidth(s.GetWidth());}";

            }
            else
            {
                devexListBox.ClientSideEvents.DropDown = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";
                devexListBox.ClientSideEvents.GotFocus = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";
                devexListBox.ClientSideEvents.Init = "function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} ";

                if (PageSize > 0)
                    devexListBox.GridViewProperties.SettingsPager.PageSize = PageSize;
                else
                    devexListBox.GridViewProperties.SettingsPager.PageSize = 10;
            }
            Value = "";
            AllowNull = false;
            AllowVaries = false;
            deletedChoices = ConfigurationVariableManager.GetValue("DeletedChoices");
            FilterClosedFields = ConfigurationVariableManager.GetValue("FilterClosedFields");
        }

        private void DevexListBox_DataBinding(object sender, EventArgs e)
        {
            //DevExpress.Web.ASPxGridLookup gridview = (DevExpress.Web.ASPxGridLookup)sender;
            gridview = (DevExpress.Web.ASPxGridLookup)sender;
            data = (DataTable)gridview.DataSource;
            BindGrid(data);
        }
        private void BindGrid(DataTable data)
        {
            if (data == null || !string.IsNullOrEmpty(FilterExpression))
            {
                if (fieldManager == null)
                    fieldManager = new FieldConfigurationManager(context);

                //if (Mode == ControlMode.New && (FieldName == DatabaseObjects.Columns.CRMCompanyTitleLookup || FieldName == DatabaseObjects.Columns.LeadSource))
                if (Mode == ControlMode.New && FilterClosedFields.Contains(FieldName))
                {
                    if (String.IsNullOrEmpty(FilterExpression))
                        FilterExpression = $"{DatabaseObjects.Columns.Closed}  <> 1";
                    else
                        FilterExpression = $"{FilterExpression} and {DatabaseObjects.Columns.Closed} <> 1";
                }

                if (FieldName == DatabaseObjects.Columns.ContactLookup)
                {
                    string TicketId = Convert.ToString(HttpContext.Current.Request["TicketId"]);

                    if (Mode == ControlMode.New)
                    {
                        // Added below code, as some tables using IsDeleted & others Deleted; Need to look into this
                        var field = fieldManager.GetFieldByFieldName(FieldName);
                        if (field != null)
                        {
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Deleted, field.ParentTableName))
                                FilterExpression = DatabaseObjects.Columns.Deleted + " <> 1";
                            //else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Deleted, field.ParentTableName))
                            //    FilterExpression = DatabaseObjects.Columns.Deleted + " <> 1";
                        }
                    }
                    if (!string.IsNullOrEmpty(TicketId))
                    {
                        //if (String.IsNullOrEmpty(FilterExpression))
                            //FilterExpression = $"{DatabaseObjects.Columns.CRMCompanyLookup} = '{TicketId}'";
                        //else
                            //FilterExpression = $"{FilterExpression} and {DatabaseObjects.Columns.CRMCompanyLookup} = '{TicketId}'";
                    }

                }

                bool enableStudioDivisionHierarchy = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
                if (FieldName == DatabaseObjects.Columns.StudioLookup)
                {
                    if (enableStudioDivisionHierarchy)
                    {
                        string division = UGITUtility.GetCookieValue(HttpContext.Current.Request, "ticketDivision");
                        if (!string.IsNullOrEmpty(division))
                            FilterExpression = $"{DatabaseObjects.Columns.DivisionLookup} = '{division}'";
                    }
                }
                if (FieldName == DatabaseObjects.Columns.ProposalRecipient || FieldName == DatabaseObjects.Columns.AdditionalRecipients)
                {
                    string TicketId = Convert.ToString(HttpContext.Current.Request["TicketId"]);

                    if (!string.IsNullOrEmpty(TicketId))
                    {
                        var relatedContacts = RelatedCompanyManager.Load(x => x.TicketID == TicketId && !string.IsNullOrEmpty(x.ContactLookup))
                            .Select(x => x.ContactLookup)
                            .ToList();

                        if (relatedContacts.Count > 0)  // Added condition to fix open Opportunity for Editing, issue
                        {
                            string values = String.Join(Constants.Separator6, relatedContacts);
                            values = string.Join(",", values.Split(',').Select(x => $"'{x}'").ToList());
                            if (String.IsNullOrEmpty(FilterExpression))
                                FilterExpression = $"{DatabaseObjects.Columns.TicketId} in ({values})";
                            else
                                FilterExpression = $"{FilterExpression} and {DatabaseObjects.Columns.TicketId} in ({values})";
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(FilterExpression))
                                FilterExpression = $"{DatabaseObjects.Columns.ID} in (0)";
                            else
                                FilterExpression = $"{FilterExpression} and {DatabaseObjects.Columns.ID} in (0)";
                        }
                    }

                    devexListBox.KeyFieldName = DatabaseObjects.Columns.TicketId;
                }

                DataTable dt = fieldManager.GetFieldDataByFieldName(FieldName, ModuleName, FilterExpression, context.TenantID);


                if (Mode == ControlMode.New && dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    if (dt.Columns.Contains(DatabaseObjects.Columns.Deleted))
                    {
                        dv.RowFilter = $"'{DatabaseObjects.Columns.Deleted}' <> '1'";
                    }
                    //else if (dt.Columns.Contains(DatabaseObjects.Columns.Deleted))
                    //{
                    //    dv.RowFilter = $"'{DatabaseObjects.Columns.Deleted}' <> 1";
                    //}
                    dt = dv.ToTable();
                }

                // Remove/Hide Deleted Items from displaying in Dropdown for Choices.
                if (dt.Rows.Count > 0)
                {
                    if (deletedChoices.Contains(FieldName))
                    {
                        string TicketId = Convert.ToString(HttpContext.Current.Request["TicketId"]);
                        if (!string.IsNullOrEmpty(TicketId))
                        {
                            string moduleTable = ModuleViewManager.GetModuleTableName(ModuleName);
                            bool closed = UGITUtility.StringToBoolean(GetTableDataManager.GetSingleValueByTicketId(moduleTable, DatabaseObjects.Columns.Closed, TicketId, context.TenantID));

                            if (!closed)
                            {
                                String[] Choices = deletedChoices.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                                string value = Choices.Where(x => x.Contains(FieldName)).FirstOrDefault();
                                Choices = value.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var item in Choices)
                                {
                                    dt = dt.AsEnumerable().Where(x => !x.Field<string>(DatabaseObjects.Columns.Title).Contains(item)).CopyToDataTable();
                                }
                            }
                        }
                    }
                }

                if (dt != null && dt.Columns.Count > 0)
                {
                    // dt.DefaultView.Sort = DatabaseObjects.Columns.Title;

                    //if (IsMulti)
                    //{
                    //    devexListBox.Columns.Add(new GridViewCommandColumn() { ShowSelectCheckbox = true, Width = 40, SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page });
                    //}
                    if (dt.Columns.Contains(DatabaseObjects.Columns.Title) && (!FieldName.EqualsIgnoreCase(DatabaseObjects.Columns.CRMUrgency) && !FieldName.EqualsIgnoreCase(DatabaseObjects.Columns.StudioLookup) && !FieldName.EqualsIgnoreCase(DatabaseObjects.Columns.AssetModelLookup)))
                    {
                        dt.DefaultView.Sort = DatabaseObjects.Columns.Title;
                    }

                    data = dt.DefaultView.ToTable();
                    if (AllowNull)
                    {
                        DataRow newRow = data.NewRow();
                        newRow[0] = "0";
                        newRow[1] = "<None>";
                        data.Rows.InsertAt(newRow, 0);
                    }
                    if (Adddefaultvalue)
                    {
                        DataRow newRow = data.NewRow();
                        newRow[0] = "0";
                        newRow[DatabaseObjects.Columns.Title] = "All";
                        data.Rows.InsertAt(newRow, 0);
                    }
                    if (dt.Columns.Count <= 2)
                    {
                        devexListBox.DisplayFormatString = "{0}";
                    }
                }
                devexListBox.DataSource = data;

            }

            if (FieldName == DatabaseObjects.Columns.ProposalRecipient || FieldName == DatabaseObjects.Columns.AdditionalRecipients || FieldName == DatabaseObjects.Columns.CRMCompanyLookup || FieldName == DatabaseObjects.Columns.ContactLookup || FieldName == DatabaseObjects.Columns.LeadSource)
            {
                devexListBox.KeyFieldName = DatabaseObjects.Columns.TicketId;
            }

            if (!string.IsNullOrEmpty(Value))
            {
                if (IsMulti)
                {
                    if (string.IsNullOrEmpty(devexListBox.KeyFieldName))
                        devexListBox.KeyFieldName = DatabaseObjects.Columns.ID;
                    string[] keyList = Value.Split(',');
                    devexListBox.GridView.Selection.UnselectAll();

                    devexListBox.Value = Value;
                    foreach (string key in keyList)
                    {
                        devexListBox.GridView.Selection.SetSelectionByKey(key, true);
                    }
                }
                else
                {
                    devexListBox.GridView.Selection.SelectRowByKey(Value);
                    devexListBox.Value = Value;
                }
            }
            if (data != null)
            {
                if (data.Rows.Count > 10)
                    devexListBox.GridView.Settings.VerticalScrollableHeight = 220;
                else
                    devexListBox.GridView.Settings.VerticalScrollableHeight = data.Rows.Count * 22;
            }
        }
        private string getDivisionID(string data, string typeOfData)
        {
            if (typeOfData == DatabaseObjects.Columns.DivisionIdLookup)
            {
                return data;
            }
            else if (typeOfData == DatabaseObjects.Columns.DivisionLookup)
            {
                if (string.IsNullOrEmpty(data))
                    data = FilterExpression;
                List<CompanyDivision> companyDivisions = null;
                CompanyDivisionManager objCompanyDivisionManager = new CompanyDivisionManager(context);
                companyDivisions = objCompanyDivisionManager.Load(x => x.Title.ToLower() == data.Trim().ToLower());
                //companyDivisions = objCompanyDivisionManager.Load(string.Format("Where Title = {0}", divisionName.Trim()));
                if (companyDivisions.Count > 0)
                    return companyDivisions[0].ID.ToString();
            }
            else
            {
                if (typeOfData == DatabaseObjects.Columns.DepartmentID)
                {
                    DepartmentManager objDepartmentManager = new DepartmentManager(context);
                    Department department = objDepartmentManager.LoadByID(UGITUtility.StringToLong(data));
                    if (department != null)
                        return UGITUtility.ObjectToString(department.DivisionIdLookup);
                }
            }
            return "";
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(ClientInstanceName))
                this.devexListBox.ClientInstanceName = ClientInstanceName;
            if (FieldName != null)
            {
                fieldManager = new FieldConfigurationManager(context);
                field = fieldManager.GetFieldByFieldName(FieldName);
                if (field != null && field.Multi)
                {
                    IsMulti = field.Multi;
                    if (!string.IsNullOrEmpty(field.Width) && UGITUtility.StringToUnit(field.Width).Value > 0)
                    {
                        devexListBox.Width = UGITUtility.StringToUnit(field.Width);
                    }
                }
            }
            if (field != null)
            {
                if (field.FieldName == DatabaseObjects.Columns.TicketResolutionType || field.FieldName == DatabaseObjects.Columns.Category || field.FieldName == DatabaseObjects.Columns.RelationshipTypeLookup)
                {
                    this.devexListBox.GridView.CustomCallback += new ASPxGridViewCustomCallbackEventHandler(GridViewCustomCallback);
                }
                if (field.ParentTableName == DatabaseObjects.Tables.RequestType)
                {
                    this.devexListBox.TextFormatString = "{0} > {1} > {2}";
                    this.devexListBox.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                    this.devexListBox.GridViewProperties.Settings.ShowFilterRow = true;

                    this.devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Category });
                    this.devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.SubCategory });
                }
            }
            if (IsSearch)
            {
                devexListBox.GridView.Settings.ShowFilterRow = true;
            }
            if (IsMandatory && !string.IsNullOrEmpty(ValidationGroup))
            {
                this.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
                this.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                this.devexListBox.ValidationSettings.ErrorText = "Value Cannot be Null";
                this.devexListBox.ValidationSettings.RequiredField.ErrorText = "Value Cannot be Null";
                this.devexListBox.ValidationSettings.ValidationGroup = ValidationGroup;

            }

            if (!IsMulti)
                devexListBox.GridViewProperties.Settings.ShowColumnHeaders = false;
            devexListBox.KeyFieldName = DatabaseObjects.Columns.ID;
            devexListBox.ID = this.ID + "_ListBox";
            if (IsMulti)
            {
                devexListBox.CssClass += " dropdownctr";
                devexListBox.Init += DevexListBox_Init;
                devexListBox.SelectionMode = GridLookupSelectionMode.Multiple;
                devexListBox.Columns.Clear();
                if (ShowSelectAllCheckbox)   // Show or Hide Select All Checkbox
                    devexListBox.Columns.Add(new GridViewCommandColumn() { ShowSelectCheckbox = true, Width = 40, SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages });
                else
                    devexListBox.Columns.Add(new GridViewCommandColumn() { ShowSelectCheckbox = true, Width = 40, SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None });

                if (field != null)
                    devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = (!string.IsNullOrEmpty(field.ParentFieldName) ? field.ParentFieldName : DatabaseObjects.Columns.Title) });
                else
                    devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Title });
                devexListBox.MultiTextSeparator = ",";
                if (field.FieldName == DatabaseObjects.Columns.AssetLookup)
                {
                    
                    devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.AssetModelLookup, Caption = "Asset Model" });
                    devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Status, Caption = "Status" });
                    devexListBox.GridView.Settings.ShowColumnHeaders = true;
                    devexListBox.GridView.Settings.ShowHeaderFilterButton = true;
                    this.devexListBox.TextFormatString = "{0} > {1}";

                }
            }
            else
            {
                devexListBox.SelectionMode = GridLookupSelectionMode.Single;
                if (field != null)
                {
                    if (field.FieldName == DatabaseObjects.Columns.AssetModelLookup)
                    {
                        devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.VendorLookup, Caption = "Vendor" });
                        devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = (!string.IsNullOrEmpty(field.ParentFieldName) ? field.ParentFieldName : DatabaseObjects.Columns.Title), Caption = "Model" });
                        
                        devexListBox.GridView.Settings.ShowColumnHeaders = true;
                        devexListBox.GridView.Settings.ShowHeaderFilterButton = true;
                        this.devexListBox.TextFormatString = "{0} > {1}";

                    }
                    else
                    {
                        devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = (!string.IsNullOrEmpty(field.ParentFieldName) ? field.ParentFieldName : DatabaseObjects.Columns.Title) });                        
                    }
                    devexListBox.DataBind();
                }
                else
                    devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Title });
                devexListBox.ClearButton.DisplayMode = ClearButtonDisplayMode.Always;
            }
            if (customCollumns != null && customCollumns.Count > 0)
            {
                devexListBox.Columns.Clear();
                customCollumns.ForEach(x => { if (data != null) { if (data.Columns.Contains(x.FieldName)) { devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = x.FieldName, Caption = x.DisplayName }); } } else { devexListBox.Columns.Add(new GridViewDataColumn() { FieldName = x.FieldName, Caption = x.DisplayName }); } });


            }

            if (devexListBox.Columns.Count <= 1)
                devexListBox.GridView.Settings.ShowColumnHeaders = false;

            Controls.Add(devexListBox);

            base.OnInit(e);

        }

        private void GridViewCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DevExpress.Web.ASPxGridView glookup = (DevExpress.Web.ASPxGridView)sender;
            var param = e.Parameters.Split(',').ToArray();
            if (param.Length >= 3)
            {
                if (field.FieldName == DatabaseObjects.Columns.StudioLookup)
                {
                    ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(context);
                    bool enableStudioDivisionHierarchy = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
                    if (enableStudioDivisionHierarchy && !string.IsNullOrEmpty(param[2])) 
                    { 
                        FilterExpression = $"{DatabaseObjects.Columns.DivisionLookup} = '{getDivisionID(param[2], param[1])}'";
                        BindGrid(null);
                        devexListBox.DataBind();
                        //if (data.Rows.Count > 10)
                        //    devexListBox.GridView.Settings.VerticalScrollableHeight = 220;
                        //else
                        //    devexListBox.GridView.Settings.VerticalScrollableHeight = data.Rows.Count * 22;
                        return;
                    }
                }
                else 
                {
                    string[] dataRequest = null;
                    DataTable dt = null;
                    fieldManager = new FieldConfigurationManager(context);
                field = fieldManager.GetFieldByFieldName(Convert.ToString(param[1]));
                string rvalue = "";
                if (field.FieldName == DatabaseObjects.Columns.TicketResolutionType)
                {
                    if (!string.IsNullOrEmpty(param[0].ToString()))
                    {
                        rvalue = Convert.ToString(GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.ID + "=" + Convert.ToString(param[0].ToString())).Rows[0][DatabaseObjects.Columns.ResolutionTypes]);

                    }
                    else
                    {
                        rvalue = "";
                    }
                }
                else if (field.FieldName == DatabaseObjects.Columns.Category)
                {
                    if (!string.IsNullOrEmpty(param[0].ToString()))
                    {
                        rvalue = Convert.ToString(GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.ID + "=" + Convert.ToString(param[0].ToString())).Rows[0][DatabaseObjects.Columns.IssueTypeOptions]);
                    }
                    else
                    {
                        rvalue = "";
                    }
                }

                if (!string.IsNullOrEmpty(rvalue))
                {
                    dt = new DataTable();
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(DatabaseObjects.Columns.ID);
                        dt.Columns.Add(DatabaseObjects.Columns.Title);
                    }
                    dataRequest = UGITUtility.SplitString(rvalue, Constants.Separator);
                    foreach (string val in dataRequest)
                    {
                        DataRow dr = dt.NewRow();
                        dr[DatabaseObjects.Columns.ID] = val;
                        dr[DatabaseObjects.Columns.Title] = val;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    dt = fieldManager.GetFieldDataByFieldName(FieldName, ModuleName);
                }
                data = dt;

                glookup.DataBind();
                for (int i = 2; i < param.Length; i++)
                {
                    glookup.Selection.SelectRowByKey(param[i]);
                }
            }
            }
            if (e.Parameters.Contains("ValueChanged"))
            {
                var moduleid = e.Parameters.Split('~')[1];
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                ModuleName = moduleViewManager.GetByID(Convert.ToInt32(moduleid)).ModuleName;
                devexListBox.DataBind();
            }
        }

        private void DevexListBox_Init(object sender, EventArgs e)
        {
            devexListBox.DataBind();
        }
        public string GetText()
        {
            string value = "";
            value = Convert.ToString(devexListBox.Text.Trim());
            if (IsMulti)
            {
                List<string> valuelist = UGITUtility.ConvertStringToList(devexListBox.Text, ",");
                valuelist = valuelist.Select(x => x.Trim()).ToList();
                value = string.Join(",", valuelist);
            }
            return value;
        }
        public string GetValues()
        {
            string value = "";
            if (IsMulti)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(devexListBox.Value)))
                {

                    value = string.Join(",", devexListBox.GridView.GetSelectedFieldValues(devexListBox.KeyFieldName).ToList());
                }

            }
            else
            {

                value = Convert.ToString(devexListBox.Value);

                //value = devexListBox.GridView.GetSelectedFieldValues(devexListBox.KeyFieldName).SingleOrDefault().ToString();

            }
            return value;
        }

        public List<string> GetValuesAsList()
        {
            List<string> values = new List<string>();
            if (IsMulti)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(devexListBox.Value)))
                {
                    foreach (string s in devexListBox.GridView.GetSelectedFieldValues(devexListBox.KeyFieldName).ToList())
                    {
                        values.Add(s);
                    }
                }
            }
            else
            {
                string value = Convert.ToString(devexListBox.GridView.GetSelectedFieldValues(devexListBox.KeyFieldName).ToString());
                if (!string.IsNullOrWhiteSpace(value))
                    values.Add(value);
            }
            return values;
        }

        public List<string> GetTextsAsList()
        {
            List<string> values = UGITUtility.ConvertStringToList(devexListBox.Text, ", ");
            return values;
        }

        public void SetValues(string value)
        {
            devexListBox.GridView.Selection.UnselectAll();
            if (string.IsNullOrEmpty(value))
            {
                devexListBox.Text = string.Empty;
                return;
            }

            if (IsMulti)
            {
                if (string.IsNullOrEmpty(devexListBox.KeyFieldName))
                    devexListBox.KeyFieldName = DatabaseObjects.Columns.ID;
                string[] keyList = value.Split(',');
                devexListBox.GridView.Selection.UnselectAll();
                Value = value;
                devexListBox.Value = value;
                foreach (string key in keyList)
                {
                    devexListBox.GridView.Selection.SetSelectionByKey(key, true);
                }

            }
            else
            {
                Value = value;

                devexListBox.GridView.Selection.SelectRowByKey(value);
            }
        }
        public void SetText(string value)
        {
            if (IsMulti)
            {
                devexListBox.Text = value;
            }
            else if (AllowVaries)
            {
                devexListBox.NullText = Varies;
                devexListBox.NullTextStyle.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                devexListBox.Text = value;

            }

        }

        public override void DataBind()
        {
            data = null;
            devexListBox.DataBind();
            base.DataBind();
        }
    }
}
