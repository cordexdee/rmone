
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using System.Text;
using System.Collections;
using DevExpress.Web;
using System.Collections.Generic;
using uGovernIT.Core;
using System.Drawing;
using System.Web;
using uGovernIT.Manager;
using DevExpress.Web.Rendering;
using uGovernIT.Manager.Managers;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using DevExpress.Data;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class Template : UserControl
    {
        public string Module { get; set; }
        public string TicketId { get; set; }
        public DataTable FieldTable { get; set; }
        public int TemplateId { get; set; }
        string[] CheckBoxTypeFields = { "IsPrivate", "IsPerformanceTestingDone", "IsITGApprovalRequired", "IsSteeringApprovalRequired" };
        TicketTemplate spTemplateItem;
        UGITModule ModuleObj;
        DataTable SPTickets;
        DataRow ticket;

        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        TicketTemplateManager TemplateManager = new TicketTemplateManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            this.ID = "ticketTemplates";
            grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            grid.AutoGenerateColumns = false;
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsPager.PageSizeItemSettings.Visible = true;
            grid.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            grid.SettingsBehavior.AllowSort = false;
            grid.SettingsBehavior.AllowSelectByRowClick = false;
            grid.SettingsBehavior.EnableRowHotTrack = true;
            //grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ugitlight1lightest";
            grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");



            if (TemplateId > 0 && Module == null)
            {
                spTemplateItem = TemplateManager.LoadByID(TemplateId);
                if (spTemplateItem != null)
                {
                    Module = spTemplateItem.ModuleNameLookup;
                }
            }
            if (!IsPostBack)
            {
                LoadData();

                if (TemplateId > 0)
                {
                    FillData();
                }
            }

            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ModuleObj = ModuleManager.LoadByName(Module);

            if (ModuleObj != null)
            {
                SPTickets = ticketManager.GetAllTickets(ModuleObj);
                ticket = ticketManager.GetByTicketID(ModuleObj, TicketId);
            }


            BindGridView();
            if (!IsPostBack)
            {
                //LoadControls();
            }

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void FillData()
        {

            ddlTicketType.SelectedIndex = ddlTicketType.Items.IndexOf(ddlTicketType.Items.FindByValue(Convert.ToString(spTemplateItem.TemplateType)));

            txtTemplateName.Text = Convert.ToString(spTemplateItem.Title);
            if (spTemplateItem != null)
            {
                string values = Convert.ToString(spTemplateItem.FieldValues);
                if (!string.IsNullOrEmpty(values))
                {
                    string[] fields = values.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in fields)
                    {
                        string[] parameters = s.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);
                        if (parameters.Length > 0)
                        {
                            grid.Selection.SelectRowByKey(parameters[0]);
                        }
                    }
                }
            }
        }

        #region Data & Binding

        private void LoadData()
        {

            DataTable dtFormLayout = ModuleManager.LoadModuleListByName(Module, DatabaseObjects.Tables.FormLayout);
            DataTable dtTab = ModuleManager.LoadModuleListByName(Module, DatabaseObjects.Tables.ModuleFormTab);
            DataRow[] formLayout = dtFormLayout.Select(string.Format("{0}='{1}' And ({2} is null Or {2} <> 'True')", DatabaseObjects.Columns.ModuleNameLookup, Module, DatabaseObjects.Columns.HideInTicketTemplate));
            //DataRow dr = dtTabs.NewRow();
            //dr["TabId"] = "0";
            //dr["TabName"] = dtTabs.Rows[0]["TabName"];
            //dr["ModuleNameLookup"] = dtTabs.Rows[0]["ModuleNameLookup"];
            //dtTabs.Rows.Add(dr);

            //  DataRow[] tabs = dtTabs.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, Module));

            DataTable dt = new DataTable();
            dt.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            dt.Columns.Add(DatabaseObjects.Columns.TabName, typeof(string));
            dt.Columns.Add(DatabaseObjects.Columns.FieldName, typeof(string));
            dt.Columns.Add(DatabaseObjects.Columns.FieldDisplayName, typeof(string));
            dt.Columns.Add(DatabaseObjects.Columns.FieldSequence, typeof(int));
            dt.Columns.Add(DatabaseObjects.Columns.TabId, typeof(int));

            var result = from dataRows1 in formLayout.AsEnumerable()
                         join dataRows2 in dtTab.AsEnumerable()
                         on dataRows1[DatabaseObjects.Columns.TabId] equals dataRows2[DatabaseObjects.Columns.TabId]
                         select dt.LoadDataRow(new object[]
                        {
                            dataRows1[DatabaseObjects.Columns.Id],
                            dataRows2[DatabaseObjects.Columns.TabName],
                            dataRows1[DatabaseObjects.Columns.FieldName],
                            dataRows1[DatabaseObjects.Columns.FieldDisplayName],
                            dataRows1[DatabaseObjects.Columns.FieldSequence],
                             dataRows2[DatabaseObjects.Columns.TabId]
                        }, false);

            FieldTable = result.CopyToDataTable();

            FieldTable = FieldTable.Select(string.Format("{0} NOT IN ('#GroupEnd#','#GroupStart#','#Control#','#TableStart#','#TableEnd#','#Label#')", DatabaseObjects.Columns.FieldName)).CopyToDataTable();

            if (FieldTable.Rows.Count > 0)
            {
                DataView dataView = FieldTable.DefaultView;
                dataView.Sort = DatabaseObjects.Columns.TabId + " ASC," + DatabaseObjects.Columns.FieldSequence + " ASC";
                FieldTable = dataView.ToTable();
            }
            else
            {
                FieldTable = null;
            }
        }

        private void BindGridView()
        {
            grid.DataSource = FieldTable;
            grid.DataBind();
        }

        #endregion

        #region Events

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (FieldTable == null || FieldTable.Rows.Count == 0)
            {
                LoadData();
            }
            grid.DataSource = FieldTable;
        }

        #endregion

        protected void btSave_Click(object sender, EventArgs e)
        {
            var objList = grid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.FieldName });

            List<string> fields = new List<string>();

            foreach (object obj in objList)
            {
                string value = string.Empty;
                FieldConfigurationManager fieldConfigurationMGR = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                FieldConfiguration spField = fieldConfigurationMGR.GetFieldByFieldName(obj.ToString());
                string fieldtype = string.Empty;
                if (spField != null)
                    fieldtype = spField.Datatype;
                else if (ModuleObj != null && ModuleObj.List_FormLayout != null && ModuleObj.List_FormLayout.Count > 0)
                {
                    ModuleFormLayout formLayOut = ModuleObj.List_FormLayout.Where(x => !string.IsNullOrEmpty(x.ColumnType) && x.FieldName.Equals(obj)).FirstOrDefault();
                    if (formLayOut != null)
                    {
                        fieldtype = formLayOut.ColumnType;
                    }
                }

                string fieldName = UGITUtility.ObjectToString(obj);
                switch (fieldtype)
                {
                    case "UserField":
                        UserValueBox ppeValue = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, spField.FieldName) as UserValueBox;
                        value = Convert.ToString(ppeValue.GetValues());
                        break;
                    case "Date":
                    case "DateTime":
                        DropDownList ddlOperator = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, "ddlOperator") as DropDownList;
                        TextBox txtDays = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, "txtDays") as TextBox;
                        int days = 0;
                        int.TryParse(txtDays.Text, out days);
                        if (ddlOperator.SelectedIndex == 1)
                            days = -days;
                        value = string.Format("f:adddays(Today,{0})", days);

                        break;
                    case "Boolean":
                        CheckBox chkValue = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, fieldName) as CheckBox;
                        value = chkValue.Checked.ToString();
                        break;
                    case "Choice":
                    case "Choices":
                        LookUpValueBox lookupChoice = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, spField.FieldName) as LookUpValueBox;
                        value = lookupChoice.GetValues();
                        break;
                    case "Lookup":

                        if (obj.ToString().Contains(DatabaseObjects.Columns.DepartmentLookup) || obj.ToString().Contains(DatabaseObjects.Columns.TicketBeneficiaries))
                        {
                            Control ctr = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, spField.FieldName) as Control;
                            if (ctr is DepartmentDropdownList)
                            {
                                value = ((DepartmentDropdownList)ctr).GetValues();
                            }
                            else if (ctr is LookupValueBoxEdit)
                            {
                                LookupValueBoxEdit lookUpValueBox = (LookupValueBoxEdit)ctr;
                                if (!string.IsNullOrEmpty(lookUpValueBox.Value))
                                    value = lookUpValueBox.Value;
                                else
                                    value = lookUpValueBox.GetValues();

                            }
                            else
                            {
                                LookUpValueBox lookUpValueBox = (LookUpValueBox)ctr;
                                value = lookUpValueBox.GetValues();
                            }
                        }
                        else if (obj.ToString().Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                        {
                            LookupValueBoxEdit lookupValueBoxEdit = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, spField.FieldName) as LookupValueBoxEdit;
                            if (lookupValueBoxEdit != null)
                            {
                                value = lookupValueBoxEdit.GetValues();
                            }
                        }
                        else
                        {
                            LookUpValueBox lookup = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, spField.FieldName) as LookUpValueBox;
                            value = lookup.GetValues();
                        }
                        break;
                    default:
                        //Added this since making DataType as Boolean for 'IsPrivate' in FieldConfiguration table is making Is Private field invisible in New Ticket popup
                        if (CheckBoxTypeFields.Contains(fieldName))
                        {
                            CheckBox chkValue1 = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, fieldName) as CheckBox;
                            value = chkValue1.Checked.ToString();
                            fieldtype = "System.Boolean";
                        }
                        else
                        {
                            TextBox txtValue = grid.FindRowCellTemplateControl(grid.FindVisibleIndexByKeyValue(obj), grid.Columns[4] as GridViewDataColumn, fieldName) as TextBox;
                            value = txtValue.Text;
                            fieldtype = "System.String";
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(value))
                    fields.Add(string.Format("{0}~#{1}~#{2}", obj.ToString(), fieldtype, value));
            }




            if (TemplateId == 0)
            {
                spTemplateItem = new TicketTemplate();
            }
            else
            {
                TicketTemplate spListItem = TemplateManager.Load(x => x.Title == txtTemplateName.Text).FirstOrDefault();  // SPListHelper.GetListItem(DatabaseObjects.Lists.TicketTemplates, DatabaseObjects.Columns.Title, txtTemplateName.Text, "Text", SPContext.Current.Web);
                if (spListItem != null)
                {
                    if (UGITUtility.StringToInt(spListItem.ID) != TemplateId)
                    {
                        TemplateManager.Delete(spListItem);
                    }
                }
            }

            //SPListItem 
            spTemplateItem.Title = txtTemplateName.Text;
            spTemplateItem.FieldValues = string.Join(Constants.Separator3, fields.ToArray());
            spTemplateItem.ModuleNameLookup = Module;
            spTemplateItem.TemplateType = ddlTicketType.SelectedValue;
            TemplateManager.Update(spTemplateItem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        //private void LoadControls()
        //{
        //    string values = string.Empty;
        //    string[] fields = null;
        //    if (spTemplateItem != null)
        //    {
        //        values = Convert.ToString(spTemplateItem.FieldValues);
        //        if (!string.IsNullOrEmpty(values))
        //        {
        //            fields = values.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);
        //        }
        //    }

        //    for (int i = 0; i <= grid.VisibleRowCount; i++)
        //    {
        //        if (grid.GetRowLevel(i) != 1)
        //        {
        //            continue;
        //        }

        //        DataRow dr = grid.GetDataRow(i);

        //        string key = Convert.ToString(dr[DatabaseObjects.Columns.FieldName]);
        //        if (ticket != null && !ticket.Table.Columns.Contains(key))
        //        {
        //            continue;
        //        }

        //        string value = string.Empty;
        //        if (fields != null)
        //        {
        //            string field = fields.ToList().FirstOrDefault(x => x.StartsWith(key));
        //            value = !string.IsNullOrEmpty(field) && field.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries).Length > 2 ? field.Split(new string[] { "~#" }, StringSplitOptions.RemoveEmptyEntries)[2] : string.Empty;
        //        }
        //        else if (ticket != null)
        //        {
        //            value = Convert.ToString(ticket[key]);
        //        }

        //        //UserValueBox ppeValue2 = grid.FindRowCellTemplateControl(i, grid.Columns[4] as GridViewDataColumn, "ppeValue") as UserValueBox;
        //        //ppeValue2.Style.Add("display", "none");
        //        HtmlGenericControl dvDateTime = grid.FindRowCellTemplateControl(i, grid.Columns[4] as GridViewDataColumn, "dvDateTime") as HtmlGenericControl;
        //        HtmlGenericControl dvComplexControl = grid.FindRowCellTemplateControl(i, grid.Columns[4] as GridViewDataColumn, "dvComplexControl") as HtmlGenericControl;

        //        dvDateTime.Style.Add("display", "none");
        //        DropDownList ddlValue = grid.FindRowCellTemplateControl(i, grid.Columns[4] as GridViewDataColumn, "ddlValue") as DropDownList;
        //        UGITModule moduleConfig = ModuleManager.LoadByName(Module);  // uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, Module);
        //        DepartmentDropdownList dptart = grid.FindRowCellTemplateControl(i, grid.Columns[4] as GridViewDataColumn, "dpmtValue") as DepartmentDropdownList;

        //        //if (SPTickets != null && !SPTickets.con.ContainsField(key))
        //        //    continue;




        //    }
        //}

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string values = string.Empty;
            string[] fields = null;
            if (spTemplateItem != null)
            {
                values = Convert.ToString(spTemplateItem.FieldValues);
                if (!string.IsNullOrEmpty(values))
                {
                    fields = values.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            if (fields == null)
                return;
            string value = string.Empty;
            string field = fields.ToList().FirstOrDefault(x => x.StartsWith(Convert.ToString(e.KeyValue)));
            if (field != null)
                value = !string.IsNullOrEmpty(field) && field.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries).Length > 2 ? field.Split(new string[] { "~#" }, StringSplitOptions.RemoveEmptyEntries)[2] : field.Split(new string[] { "~#" }, StringSplitOptions.RemoveEmptyEntries)[1];
            if (ticket != null && UGITUtility.IsSPItemExist(ticket, UGITUtility.ObjectToString(e.KeyValue)))
            {
                value = UGITUtility.ObjectToString(ticket[UGITUtility.ObjectToString(e.KeyValue)]);
            }
            if (!string.IsNullOrEmpty(value))
            {
                FieldConfigurationManager fieldConfigurationMGR = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                FieldConfiguration spField = fieldConfigurationMGR.GetFieldByFieldName(Convert.ToString(e.KeyValue));
                string fieldtype = string.Empty;
                if (spField != null)
                {
                    fieldtype = spField.Datatype;

                    if (spField.Datatype == "Lookup")
                    {
                        if (!string.IsNullOrWhiteSpace(spField.TemplateType))
                        {
                            LookupValueBoxEdit lookupValueBox = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.DataColumns[3], spField.FieldName) as LookupValueBoxEdit;
                            if (lookupValueBox != null)
                                lookupValueBox.SetValues(value);
                        }
                        else
                        {
                            LookUpValueBox lookupValueBox = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.DataColumns[3], spField.FieldName) as LookUpValueBox;
                            if (lookupValueBox != null)
                                lookupValueBox.SetValues(value);
                        }
                    }
                }
            }
        }

        protected void cbValidate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter) && e.Parameter.Contains("ValidateName"))
            {
                if (!string.IsNullOrEmpty(txtTemplateName.Text))
                {

                    if (!cbValidate.JSProperties.ContainsKey("cpIsValidated"))
                    {
                        cbValidate.JSProperties.Add("cpIsValidated", false);
                    }

                    TicketTemplate spListItem = TemplateManager.Load(x => x.Title == txtTemplateName.Text).FirstOrDefault();  // SPListHelper.GetListItem(DatabaseObjects.Lists.TicketTemplates, DatabaseObjects.Columns.Title, txtTemplateName.Text, "Text", SPContext.Current.Web);
                    if (spListItem != null)
                    {
                        if (TemplateId > 0)
                        {
                            if (UGITUtility.StringToInt(spListItem.ID) != TemplateId)
                            {
                                cbValidate.JSProperties["cpIsValidated"] = true;
                            }
                        }
                        else
                        {
                            cbValidate.JSProperties["cpIsValidated"] = true;
                        }
                    }
                }

            }

        }

        protected void cbbox_Callback(object sender, CallbackEventArgsBase e)
        {

        }

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

        }

        protected void grid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            string values = string.Empty;
            string[] fields = null;
            if (spTemplateItem != null)
            {
                values = Convert.ToString(spTemplateItem.FieldValues);
                if (!string.IsNullOrEmpty(values))
                {
                    fields = values.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);
                }
            }


            GridViewDataColumn column = new GridViewDataColumn();
            column.FieldName = DatabaseObjects.Columns.FieldName;
            MyDataItemTemplate mytemplate = new MyDataItemTemplate();
            mytemplate.ModuleName = Module;
            mytemplate.FieldValues = fields;
            mytemplate.TicketRow = ticket;
            mytemplate.ModuleObj = ModuleObj;
            column.DataItemTemplate = mytemplate;

            grid.Columns.Add(column);
        }
    }

    class MyDataItemTemplate : ITemplate
    {
        public string ModuleName { get; set; }
        public string[] FieldValues { get; set; }
        public DataRow TicketRow { get; set; }
        public UGITModule ModuleObj { get; set; }




        public void InstantiateIn(Control container)
        {

            string[] CheckBoxTypeFields = { "IsPrivate", "IsPerformanceTestingDone", "IsITGApprovalRequired", "IsSteeringApprovalRequired" };
            string[] NumericDataFields = { "ActualHours", "PctComplete", "TotalHoldDuration", "EstimatedHours", "BAAnalysisHours", "BATestingHours", "BATotalHours", "DeveloperCodingHours", "DeveloperSupportHours", "DeveloperSupportHours", "DeveloperTotalHours", "Duration", "EstimatedHours", "TotalHoldDuration", "TotalHours", "ProjectEstDurationMaxDays", "ProjectEstDurationMinDays", "ProjectEstSizeMaxHrs", "ProjectEstSizeMinHrs", "Duration", "EstimatedHours" };
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            FieldConfigurationManager fieldConfigurationMGR = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            string key = UGITUtility.ObjectToString(gridContainer.KeyValue);

            string value = string.Empty;
            if (FieldValues != null)
            {
                string field = FieldValues.ToList().FirstOrDefault(x => x.StartsWith(key));
                if (field != null)
                    value = !string.IsNullOrEmpty(field) && field.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries).Length > 2 ? field.Split(new string[] { "~#" }, StringSplitOptions.RemoveEmptyEntries)[2] : field.Split(new string[] { "~#" }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            try
            {
                if (TicketRow != null)
                {
                    value = UGITUtility.ObjectToString(TicketRow[key]);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            FieldConfiguration spField = fieldConfigurationMGR.GetFieldByFieldName(key);
            string fieldtype = string.Empty;
            if (spField != null)
                fieldtype = spField.Datatype;
            else if (ModuleObj != null && ModuleObj.List_FormLayout != null && ModuleObj.List_FormLayout.Count > 0)
            {
                ModuleFormLayout formLayOut = ModuleObj.List_FormLayout.Where(x => !string.IsNullOrEmpty(x.ColumnType) && x.FieldName.Equals(key)).FirstOrDefault();
                if (formLayOut != null)
                {
                    fieldtype = formLayOut.ColumnType;
                }
            }

            switch (fieldtype)
            {
                case "UserField":
                    UserValueBox ppeValue = new UserValueBox();
                    ppeValue.ID = key;
                    if (!string.IsNullOrEmpty(value))
                    {
                        ppeValue.SetValues(value);
                    }
                    ppeValue.CssClass = "new-template-table";
                    container.Controls.Add(ppeValue);
                    // container.Controls
                    break;
                case "Date":
                case "DateTime":
                    {
                        HtmlGenericControl span1 = new HtmlGenericControl("SPAN");
                        HtmlGenericControl b1 = new HtmlGenericControl("B");
                        b1.InnerText = "Request Creation Date";
                        span1.Controls.Add(b1);
                        DropDownList ddlOperator = new DropDownList();
                        ddlOperator.Items.Add(new ListItem("+", "+"));
                        ddlOperator.Items.Add(new ListItem("-", "-"));
                        ddlOperator.ID = "ddlOperator";
                        ddlOperator.Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
                        TextBox txtDays = new TextBox();
                        txtDays.Width = Unit.Pixel(40);
                        txtDays.ID = "txtDays";
                        txtDays.Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
                        txtDays.Style.Add(HtmlTextWriterStyle.MarginRight, "5px");
                        txtDays.Attributes.Add("onkeypress", "return IsNumeric(event);");
                        if (!string.IsNullOrEmpty(value))
                        {
                            string addedToNowStr = value.Replace("f:adddays(Today,", string.Empty).Replace(")", string.Empty).Trim();
                            int addToNow = 0;
                            int.TryParse(addedToNowStr, out addToNow);

                            if (addToNow >= 0)
                            {
                                ddlOperator.SelectedIndex = 0;
                                txtDays.Text = addToNow.ToString();
                            }
                            else
                            {
                                ddlOperator.SelectedIndex = 1;
                                txtDays.Text = Math.Abs(addToNow).ToString();
                            }
                        }
                        HtmlGenericControl span2 = new HtmlGenericControl("SPAN");
                        HtmlGenericControl b2 = new HtmlGenericControl("B");
                        b2.InnerText = "days";
                        span2.Controls.Add(b1);
                        HtmlGenericControl dvDateTime = new HtmlGenericControl("DIV");
                        dvDateTime.Controls.Add(span2);
                        dvDateTime.Controls.Add(ddlOperator);
                        dvDateTime.Controls.Add(txtDays);
                        dvDateTime.Controls.Add(span1);
                        span1.Controls.Add(b2);
                        container.Controls.Add(dvDateTime);
                        break;
                    }
                case "Boolean":
                    {
                        CheckBox chkValue = new CheckBox();
                        chkValue.ID = key;
                        if (!string.IsNullOrEmpty(value))
                        {
                            chkValue.Checked = UGITUtility.StringToBoolean(value);
                        }
                        chkValue.CssClass = "new-template-table";
                        container.Controls.Add(chkValue);
                        break;
                    }
                case "Choice":
                case "Choices":
                    {
                        LookUpValueBox choice = new LookUpValueBox();
                        choice.ID = key;
                        choice.FieldName = key;
                        if (!string.IsNullOrEmpty(value))
                        {
                            choice.SetValues(value);
                        }
                        choice.CssClass = "new-template-table";
                        container.Controls.Add(choice);
                        break;
                    }
                case "Lookup":
                    {

                        if (!string.IsNullOrWhiteSpace(spField.TemplateType))
                        {
                            LookupValueBoxEdit dropDownBoxEdit = new LookupValueBoxEdit();
                            dropDownBoxEdit.dropBox.AutoPostBack = false;
                            dropDownBoxEdit.dropBox.CssClass = "";
                            dropDownBoxEdit.ModuleName = ModuleName;
                            dropDownBoxEdit.FieldName = key;
                            dropDownBoxEdit.ID = key;
                            if (!string.IsNullOrEmpty(value))
                            {
                                dropDownBoxEdit.SetValues(value);
                                dropDownBoxEdit.Value = value;
                            }
                            dropDownBoxEdit.CssClass = "new-template-table";
                            container.Controls.Add(dropDownBoxEdit);

                        }
                        else
                        {
                            LookUpValueBox lookup = new LookUpValueBox();
                            lookup.FieldName = key;
                            lookup.ModuleName = ModuleName;
                            lookup.ID = key;
                            if (!string.IsNullOrEmpty(value))
                            {
                                lookup.SetValues(value);
                            }
                            lookup.CssClass = "new-template-table";
                            container.Controls.Add(lookup);
                        }
                        break;
                    }
                case "NoteField":
                    {
                        TextBox txtValue = new TextBox();
                        txtValue.TextMode = TextBoxMode.MultiLine;
                        txtValue.Rows = 3;
                        txtValue.Visible = true;
                        txtValue.ID = key;
                        if (!string.IsNullOrEmpty(value))
                        {
                            txtValue.Text = value;
                        }
                        txtValue.CssClass = "new-template-table";
                        container.Controls.Add(txtValue);
                    }
                    break;
                default:
                    {
                        //Added this since making DataType as Boolean for 'IsPrivate' in FieldConfiguration table is making Is Private field invisible in New Ticket popup
                        if (CheckBoxTypeFields.Contains(key))
                        {
                            CheckBox chkValue = new CheckBox();
                            chkValue.ID = key;
                            if (!string.IsNullOrEmpty(value))
                            {
                                chkValue.Checked = UGITUtility.StringToBoolean(value);
                            }
                            chkValue.CssClass = "new-template-table";
                            container.Controls.Add(chkValue);
                        }
                        else
                        {
                            TextBox txtValue = new TextBox();
                            txtValue.Visible = true;
                            txtValue.ID = key;
                            if (!string.IsNullOrEmpty(value))
                            {
                                txtValue.Text = value;
                            }

                            // Allows user to enter only Numeric values
                            if (NumericDataFields.Contains(key))
                            {
                                txtValue.Attributes.Add("onkeypress", "return IsFloat(event,this);");
                            }
                            txtValue.CssClass = "new-template-table";
                            container.Controls.Add(txtValue);
                        }
                        HiddenField hndCCValue = new HiddenField();
                        hndCCValue.ID = "hndCCValue";
                        container.Controls.Add(hndCCValue);

                    }
                    break;
            }
        }
    }
}
