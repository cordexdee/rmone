using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using uGovernIT.Utility;
using System.Data;
using DevExpress.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
//using uGovernIT.Web.ControlTemplates.Shared;
using System.Globalization;
using System.Threading;
using uGovernIT.Utility.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using uGovernIT.Web.ControlTemplates.Shared;
using System.Text;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public class TicketControls
    {
        public DataTable DataList { get; set; }
        public BatchMode BatchEdit { get; set; }
        public DataRow SourceItem { get; set; }
        private const string varies = "<Value Varies>";
        private string moduleName;
        private DataRow item;
        private UGITModule moduleObj;
        private UserProfileManager UserManager;
        ApplicationContext context;
        FieldConfigurationManager configFieldManager;
        FieldConfiguration configField = null;
        UserProfile currentUser = HttpContext.Current.CurrentUser();
        private DepartmentManager _departmentManager;
        private ConfigurationVariableManager _configurationVariableManager;

        ModuleViewManager ObjModuleViewManager = null;
        LookUpValueBox lookupCtrl = null;
        ASPxTextBox tf = null;
        private bool _enableDivision;

        public TicketControls(UGITModule module, DataRow item)
        {
            context = HttpContext.Current.GetManagerContext();
            ObjModuleViewManager = new ModuleViewManager(context);
            moduleObj = module;
            if (module != null)
                moduleName = module.ModuleName;
            this.item = item;
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            configFieldManager = new FieldConfigurationManager(context);
            _departmentManager = new DepartmentManager(context);
            _configurationVariableManager = new ConfigurationVariableManager(context);
            _enableDivision = _configurationVariableManager.GetValueAsBool(ConfigConstants.EnableDivision);
        }

        public Control GetControls(DataColumn field, ControlMode mode, FieldDesignMode designMode, string parentIdPrefix, ModuleFormLayout formLayout)
        {
           
            string tabId = string.Empty;
            string displayName = string.Empty;
            if (formLayout != null)
            {
                tabId = Convert.ToString(formLayout.TabId);
                displayName = Convert.ToString(formLayout.FieldDisplayName);
            }
            //FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            //FieldConfiguration configField = configFieldManager.GetFieldByFieldName(field.ColumnName);
            string tableName = string.Empty;
            if (moduleObj != null)
                tableName = this.moduleObj.ModuleTable;
            configField = configFieldManager.GetFieldByFieldName(field.ColumnName, tableName);

            string fieldColumnType = string.Empty;
            if (configField != null && !string.IsNullOrWhiteSpace(configField.Datatype))
            {
                if (configField.Datatype.Equals("Lookup", StringComparison.OrdinalIgnoreCase) && UGITUtility.StringToBoolean(configField.Multi))
                    fieldColumnType = "MultiLookup";
                else
                    fieldColumnType = Convert.ToString(configField.Datatype);
            }
            else if (formLayout != null && !string.IsNullOrEmpty(formLayout.ColumnType))//&& (formLayout.ColumnType.Equals("Currency") || formLayout.ColumnType.Equals("Percentage") || formLayout.ColumnType.Equals("Phone"))
                if (formLayout.ColumnType != "Default")
                    fieldColumnType = formLayout.ColumnType;
                else
                {
                    fieldColumnType = Convert.ToString(field.DataType);
                }
            else
                fieldColumnType = Convert.ToString(field.DataType);

            // its a special field to generate attachment control
            if (field.ColumnName == "Attachments")
                fieldColumnType = "Attachments";
            //else if (field.ColumnName == DatabaseObjects.Columns.TicketDescription)
            //    fieldColumnType = "NoteField";

            try
            {
                Control returnControl = null;
                switch (fieldColumnType)
                {
                    case "UserType":
                        returnControl = new Label() { Text = "Working On it", ID = configField.FieldName + '_' + tabId };//CreateUserFieldControl(dt, col, mode);
                        break;
                    case "Lookup":
                        returnControl = CreateLookUpValueControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit, displayName);
                        break;
                    case "System.String":
                        returnControl = CreateTextFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "System.Boolean":
                        returnControl = CreateBooleanField(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit); break;
                    case "System.Double":
                        returnControl = CreateTextFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "NoteField":
                        returnControl = CreateNoteFieldControl1(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "DropDownChoiceField":
                        returnControl = new Label() { Text = "Working On it", ID = configField.FieldName + '_' + tabId }; //CreateDropDownChoiceField(dt, col, mode, colType);
                        break;
                    case "RadioButtonChoiceField":
                        returnControl = new Label() { Text = "Working On it", ID = configField.FieldName + '_' + tabId }; // CreateRadioButtonChoiceField(dt, col, mode, colType);
                        break;
                    case "System.Int32":
                        returnControl = CreateTextFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "System.Int64":
                        returnControl = CreateTextFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "Percentage":
                    case "Currency":
                    case "Integer":
                    case "Double":
                        returnControl = CreateNumberFieldControl(fieldColumnType, configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit, formLayout: formLayout); break;
                    case "System.DateTime":
                        returnControl = CreateDateFieldControl(field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        //returnControl = CreateDateTimeFieldControl(field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "DateTime":
                        returnControl = CreateDateTimeFieldControl(field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "Date":
                        returnControl = CreateDateFieldControl(field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "Choices":
                        returnControl = CreateLookUpValueControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    case "BooleanField":
                        returnControl = CreateBooleanField(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit); break;

                    case "UserField":
                    case "User":
                        returnControl = CreateUserFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit); break;
                    case "Attachments":
                        returnControl = CreateAttachmentControl(field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit); break;
                    case "Phone":
                        returnControl = CreateTextFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit, formLayout: formLayout);
                        break;
                    case "MultiLookup":
                        returnControl = CreateMultipleLookupFieldControl(configField, field, DataList, item, mode, moduleName, designMode, parentIdPrefix, tabId, SourceItem, BatchEdit);
                        break;
                    default:
                        break;
                }

                // show checkbox with edit control
                // NOTE: If edit button is being displayed, that takes precedence!
                // if (!editButton && showWithCheckBox)
                if (designMode == FieldDesignMode.WithCheckbox)
                {
                    string prefixId = GenerateID(field, tabId);
                    Table tableWithCheckBox = new Table();
                    tableWithCheckBox.Attributes.Add("width", "100%");
                    tableWithCheckBox.ID = Guid.NewGuid().ToString();
                    TableRow withCheckButtonRow = new TableRow();

                    TableCell editableControlContainer = new TableCell();
                    editableControlContainer.Style.Add("display", "none");
                    editableControlContainer.ID = prefixId + "_CheckBoxContainer";
                    editableControlContainer.Controls.Add(returnControl);
                    TableCell withCheckButtonCell = new TableCell();
                    withCheckButtonCell.Width = 15;
                    CheckBox checkBox = new CheckBox();
                    checkBox.ID = GenerateID(field, tabId) + "_CheckBox";
                    checkBox.Attributes.Add("onclick", string.Format("showEditItem(this, '{0}');", parentIdPrefix + editableControlContainer.ClientID));
                    withCheckButtonCell.Controls.Add(checkBox);
                    //Checking if there is value in column then checkbox and table column will be visible
                    if (!string.IsNullOrEmpty("" + UGITUtility.GetSPItemValue(item, field.ColumnName)))
                    {
                        editableControlContainer.Style.Add("display", "block");
                        checkBox.Checked = true;
                    }
                    withCheckButtonRow.Cells.Add(withCheckButtonCell);
                    withCheckButtonRow.Cells.Add(editableControlContainer);
                    tableWithCheckBox.Rows.Add(withCheckButtonRow);
                    return tableWithCheckBox;
                }
                return returnControl;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                Label lb = new Label();
                //Log.WriteException(ex, "SPControls");
                //lb.Text = string.Format("Error in GetSharePointControls:<br/>{0}", ex);
                return lb;
            }
        }
        private Control CreateCtrWithType(Control ctr, string moduleName, DataRow item, DataColumn field, string tabId, string parentIdPrefix)
        {

            if (moduleName == null)
                return ctr;

            // string moduleName = Convert.ToString(uGITCache.GetModuleDetails(moduleId)[DatabaseObjects.Columns.ModuleName]);
            // UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(item.Web, moduleName);
            Ticket ticket = new Ticket(context, moduleName);

            int tabNumber = 0;
            int.TryParse(tabId, out tabNumber);
            ModuleFormLayout layoutItem = moduleObj.List_FormLayout.FirstOrDefault(x => x.TabId == tabNumber && x.FieldName == field.ColumnName);

            if (layoutItem != null && layoutItem.ColumnType != null && layoutItem.ColumnType != string.Empty)
            {
                string postfixcontent = UGITUtility.GetPostfixOnColumnType(layoutItem.ColumnType);
                string prefixId = GenerateID(field, tabId);
                Table tablewithpostfix = new Table();
                tablewithpostfix.Style.Add("border-collapse", "collapse!important");
                tablewithpostfix.Style.Add("cellpadding", "0px!important");
                tablewithpostfix.Style.Add("cellspacing", "0px!important");


                //if (tabNumber == 0)
                //    tablewithpostfix.Attributes.Add("width", "100%");
                tablewithpostfix.ID = Guid.NewGuid().ToString();
                tablewithpostfix.CssClass = "clstablewithpostfix";
                TableRow withpostfix = new TableRow();

                TableCell tableControlContainer = new TableCell();
                tableControlContainer.Style.Add("display", "block");
                tableControlContainer.ID = prefixId + "_ColumnTypeContainer";
                tableControlContainer.Controls.Add(ctr);

                TableCell withColumnTypePostFix = new TableCell();
                //withColumnTypePostFix.Style.Add("padding-left", "2px");
                HtmlGenericControl postfixspan = new HtmlGenericControl("span");
                postfixspan.ID = GenerateID(field, tabId) + "_ColumnTypeSpan";
                postfixspan.Attributes.Add("onclick", string.Format("showEditItem(this, '{0}');", parentIdPrefix + tableControlContainer.ClientID));
                postfixspan.InnerText = postfixcontent;
                withColumnTypePostFix.Controls.Add(postfixspan);

                withpostfix.Cells.Add(tableControlContainer);
                withpostfix.Cells.Add(withColumnTypePostFix);

                tablewithpostfix.Rows.Add(withpostfix);
                return tablewithpostfix;
            }
            return ctr;
        }
        private Control CreateRadioButtonChoiceField(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel ctrl = new Panel();
            ctrl.CssClass = "field_" + field.ColumnName.ToLower();
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";

            batchHidden.Value = "True";
            ASPxRadioButton rbcf = new ASPxRadioButton();
            rbcf.Text = field.ColumnName;
            rbcf.ID = GenerateID(field, tabId);
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {

                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.Text = varies;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.ReadOnly = true;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                ctrl.Controls.Add(batchHidden);
                ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, rbcf, batchEditviewControl, mode, batchHidden.ID));

                return ctrl;
            }
            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                Label cView = new Label();
                cView.Text = field.ColumnName;
                cView.ID = GenerateID(field, tabId) + "_view";
                cView.CssClass = viewcss;
                ctrl.Controls.Add(GetControlWithEditButton(cView.ID, rbcf, cView));
                return ctrl;
            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketInitiatorResolved)
                rbcf.CssClass += " initiatorResolved " + viewcss;
            if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
                && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
            {
                rbcf.Value = sourceItem[field.ColumnName];
            }
            else if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                rbcf.Value = sourceItem[field.ColumnName];
            }
            ctrl.Controls.Add(rbcf);
            return ctrl;
        }
        private Control CreateCheckBoxChoiceFieldControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string value = Convert.ToString(item[field.ColumnName]);
            Panel choicePane = new Panel();
            choicePane.CssClass = "field_" + field.ColumnName.ToLower();
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
               && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
            {
                value = Convert.ToString(sourceItem[field.ColumnName]);
            }

            if (batchMode == BatchMode.Edit)
            {
                value = varies;
            }


            Control choiceCtr = null;

            if (mode == ControlMode.Display)
            {
                Label ddcfs = new Label();
                ddcfs.ID = GenerateID(field, tabId) + "_View";
                if (!string.IsNullOrWhiteSpace(value))
                    ddcfs.Text = string.Join("; ", UGITUtility.ConvertStringToList(value, Constants.Separator));
                choiceCtr = ddcfs;
            }
            else
            {
                ASPxListBox ddcfs = new ASPxListBox();
                ddcfs.ID = GenerateID(field, tabId);
                ddcfs.TextField = field.ColumnName;
                //ddcfs.SPListObj = list;
                ddcfs.DataSource = list;
                int Id = Convert.ToInt32(list.Rows[0][DatabaseObjects.Columns.Id]);
                if (Id > 0)
                {
                    ddcfs.Value = value;
                }
                choiceCtr = ddcfs;

                if (field.ColumnName == DatabaseObjects.Columns.UGITIssueType)
                {
                    string splookup = (Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                    //if (splookup != null)
                    //ddcfs.RequestTypeId = splookup.LookupId;
                }
            }


            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.Text = varies;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
            }

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                ASPxCheckBox cView = new ASPxCheckBox();
                cView.Text = field.ColumnName;
                cView.ID = GenerateID(field, tabId) + "_view";
                cView.Enabled = false;
                choicePane.Controls.Add(GetControlWithEditButton(cView.ID, choiceCtr, cView));
                return choicePane;
            }

            if (batchMode == BatchMode.Edit)
            {
                choicePane.Controls.Add(batchHidden);
                choicePane.Controls.Add(GetControlBatchEditButton(field.ColumnName, choiceCtr, batchEditviewControl, mode, batchHidden.ID));
                return choicePane;
            }
            choicePane.Controls.Add(choiceCtr);
            return choicePane;
        }
        private Control GetControlBatchEditButton(string fieldid, Control editControl, Control viewControl, ControlMode mode, string HiddenID)
        {
            Table t = new Table();
            t.Width = Unit.Percentage(100);
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell { ID = fieldid + "tc1", CssClass = "tc1class" };
            TableCell tc2 = new TableCell { ID = fieldid + "tc2", CssClass = "tc2class" };
            HtmlGenericControl controlspan = new HtmlGenericControl("span");
            HtmlGenericControl editbuttonspan = new HtmlGenericControl("span");
            controlspan.ID = fieldid + "_controlspan";
            editbuttonspan.Style.Add("float", "left");
            editbuttonspan.ID = fieldid + "_editbuttonspan";
            controlspan.Style.Add("width", "100px");
            if (mode == ControlMode.Edit)
                editbuttonspan.Controls.Add(new LiteralControl(string.Format("<img style='cursor:pointer;float:left;padding-right:5px;height:16px;' class='editbutton'  src='/Content/images/editNewIcon.png' onclick='javascript:EditBatchControl(this, \"{0}\",\"{1}\",\"{2}\")' alt='Edit'/>", controlspan.ID, editbuttonspan.ID, HiddenID)));
            tc1.Controls.Add(editbuttonspan);
            tc1.Controls.Add(controlspan);

            controlspan.Controls.Add(viewControl);
            tc2.Controls.Add(editControl);
            if (mode == ControlMode.New && moduleName == "TSK")
            {
                tc2.Style.Add("display", "block");
            }
            else
            {
                tc2.Style.Add("display", "none");
            }

            tr.Cells.AddRange(new TableCell[] { tc1, tc2 });
            t.Rows.Add(tr);
            return t;
        }

        private Control CreateTextFieldControl(FieldConfiguration configField, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode, ModuleFormLayout formLayout = null)
        {
            string editcss = "field_" + field.ColumnName + "_edit";
            string viewcss = "field_" + field.ColumnName + "_view";
            Panel paneCtrl = new Panel();
            paneCtrl.CssClass = "field_" + field.ColumnName;
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            string value = Convert.ToString(item[field.ColumnName]);
            if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
               && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
            {
                value = Convert.ToString(sourceItem[field.ColumnName]);
            }
            if (batchMode == BatchMode.Edit)
            {
                value = varies;
            }
            //ASPxTextBox tf = new ASPxTextBox();
            tf = new ASPxTextBox();
            tf.ID = GenerateID(field.ColumnName, tabId);
            tf.CssClass += editcss;
            tf.EnableViewState = true;
            tf.Width = Unit.Percentage(100);
            tf.Text = value;
            if (field.ColumnName.EqualsTo("CRMDuration"))
            {
                tf.ClientSideEvents.TextChanged = "CRMDurationChanged";
            }

            if (field.ColumnName.EqualsTo("ApproxContractValue"))
            {
                tf.ClientSideEvents.TextChanged = "ApproxContractValueChanged";
            }
            if (field.ColumnName.EqualsTo(DatabaseObjects.Columns.ShortName))
            {
                ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(context);
                tf.ClientSideEvents.KeyPress = "ShortNameKeyPress";
                tf.ClientInstanceName = "clnInstanceShortName";
                tf.MaxLength = UGITUtility.StringToInt(objConfigurationVariableManager.GetValue(ConfigConstants.ShortNameCharacters));
            }

            if (field.ColumnName.EqualsTo(DatabaseObjects.Columns.ERPJobIDNC))
            {
                tf.ClientInstanceName = $"{DatabaseObjects.Columns.ERPJobIDNC}";
            }

            if (field.ColumnName.ToLower() == DatabaseObjects.Columns.Title.ToLower())
            {
                tf.TabIndex = 0;
            }
            if (field.ColumnName == "Status")
            {
                tf.ReadOnly = true;
                tf.BackColor = System.Drawing.Color.Transparent;
                tf.Style.Add("border", "none");
            }
            tf.AutoPostBack = false;
            Label lbl = new Label();
            lbl.Text = value;
            lbl.ID = GenerateID(field.ColumnName, tabId);
            lbl.CssClass += "doubleWidth " + field.ColumnName;

            if (formLayout != null && !string.IsNullOrEmpty(formLayout.ColumnType) && formLayout.ColumnType.Equals("Phone"))
            {
                tf.MaskSettings.Mask = "+99999999999999";
                tf.MaskSettings.ErrorText = "Invalid no.";
                tf.ClientSideEvents.Validation = "function(s, e) { e.isValid = true; }";
            }

            if (mode == ControlMode.Display)
            {
                lbl.Text = value;
                lbl.Text = !string.IsNullOrEmpty(value) ? value : "-";
                if (field.ColumnName == DatabaseObjects.Columns.ApproxContractValue || field.ColumnName == "IntUnpaidCosts" || field.ColumnName == "CurrentBudget" || field.ColumnName == "ProjectBillings" || field.ColumnName == "BudgetAmount" || field.ColumnName == "IntClientPaid" || field.ColumnName == "ProjectCost" || field.ColumnName == "ExtUnpaidCosts" || field.ColumnName == "ExtClientPaid")
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        //lbl.Text =  $"$ {value}";
                        lbl.Text = string.Format("{0:c0}", Convert.ToDecimal(value));
                    }
                }

                if (field.ColumnName == DatabaseObjects.Columns.UsableSqFt || field.ColumnName == DatabaseObjects.Columns.RetailSqftNum)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        lbl.Text = string.Format("{0:n0}", Convert.ToDecimal(value));
                    }
                }

                //paneCtrl.Controls.Add(lbl);

                if (mode != ControlMode.New && field.ColumnName == DatabaseObjects.Columns.SuccessChance)
                {
                    string LeadPriorityUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&showSearchOption=false&control=RankingCriteriaView&isreadonly=False&ticketId=");
                    HtmlGenericControl vSpan = new HtmlGenericControl("div");
                    vSpan.Attributes.Add("class", "leadRanking-icon");
                    Image addIcon = new Image();
                    addIcon.ToolTip = "Lead Ranking";
                    addIcon.ImageUrl = "~/Content/images/Ranking.png";
                    //addIcon.Attributes.Add("onclick", "$(document).ready(function () { ShowLeadRanking('" + Convert.ToString(item[DatabaseObjects.Columns.TicketId]) + "') });");
                    addIcon.Attributes.Add("onclick", $"UgitOpenPopupDialog('{LeadPriorityUrl}{Convert.ToString(item[DatabaseObjects.Columns.TicketId])}','', 'Lead Priority', 85, 85, false, escape(window.location.href))");
                    addIcon.Attributes.Add("style", "cursor:pointer");
                    vSpan.Controls.Add(addIcon);
                    paneCtrl.Controls.Add(vSpan);

                    if (!string.IsNullOrEmpty(value))
                    {
                        lbl.Text = $"{value} ({Convert.ToString(item[DatabaseObjects.Columns.RankingCriteriaTotal])})";
                    }
                }

                if (field.ColumnName == DatabaseObjects.Columns.TicketOPMIdLookup || field.ColumnName == DatabaseObjects.Columns.TicketLEMIdLookup)
                {
                    string viewUrl = string.Empty;
                    string title = $"{value} : {item[DatabaseObjects.Columns.Title]}";
                    string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);

                    if (field.ColumnName == DatabaseObjects.Columns.TicketOPMIdLookup)
                        viewUrl = ObjModuleViewManager.LoadByName("OPM").StaticModulePagePath;
                    else if (field.ColumnName == DatabaseObjects.Columns.TicketLEMIdLookup)
                        viewUrl = ObjModuleViewManager.LoadByName("LEM").StaticModulePagePath;

                    lbl.Attributes.Add("onclick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", value), title, sourceURL, 90, 90));
                    lbl.Attributes.Add("style", "cursor:pointer;color:rgb(0, 0, 102);");
                    lbl.Attributes.Add("onmouseover", "this.style.textDecoration='underline'");
                    lbl.Attributes.Add("onmouseout", "this.style.textDecoration=''");
                }
                //if (field.ColumnName == DatabaseObjects.Columns.TicketNPRIdLookup)
                //{
                //    string viewUrl = string.Empty;
                //    DataRow[] dataRow = null;
                //    TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                //    DataTable dtAllTickets = ticketManager.GetAllTickets(ObjModuleViewManager.LoadByName("NPR", true));
                //    dataRow = dtAllTickets.Select($"ID='{value}'");
                //    if (dataRow.Length > 0)
                //    {
                //        string title = $"{dataRow[0]["TicketId"]} : {dataRow[0]["Title"]}";
                //        value = Convert.ToString(dataRow[0]["TicketId"]);
                //        string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                //        if (field.ColumnName == DatabaseObjects.Columns.TicketNPRIdLookup)
                //            viewUrl = ObjModuleViewManager.LoadByName("NPR", true).StaticModulePagePath;
                //        lbl.Text = value;
                //        lbl.Attributes.Add("onclick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", value), title, sourceURL, 90, 90));
                //        lbl.Attributes.Add("style", "cursor:pointer;color:rgb(0, 0, 102);");
                //        lbl.Attributes.Add("onmouseover", "this.style.textDecoration='underline'");
                //        lbl.Attributes.Add("onmouseout", "this.style.textDecoration=''");
                //    }
                    
                //}
                if (field.ColumnName == DatabaseObjects.Columns.TicketPMMIdLookup || field.ColumnName == DatabaseObjects.Columns.TicketNPRIdLookup)
                {
                    string viewUrl = string.Empty;
                    DataRow dataRow = null;
                    TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                    if (field.ColumnName == DatabaseObjects.Columns.TicketPMMIdLookup)
                        dataRow = ticketManager.GetByID(ObjModuleViewManager.LoadByName("PMM"), UGITUtility.StringToLong(value));
                    if (field.ColumnName == DatabaseObjects.Columns.TicketNPRIdLookup)
                        dataRow = ticketManager.GetByID(ObjModuleViewManager.LoadByName("NPR"), UGITUtility.StringToLong(value));
                    if (dataRow!=null)
                    {
                        string title = $"{dataRow["TicketId"]} : {dataRow["Title"]}";
                        value = Convert.ToString(dataRow["TicketId"]);
                        string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                        if (field.ColumnName == DatabaseObjects.Columns.TicketPMMIdLookup)
                            viewUrl = ObjModuleViewManager.LoadByName("PMM", true).StaticModulePagePath;
                        if (field.ColumnName == DatabaseObjects.Columns.TicketNPRIdLookup)
                            viewUrl = ObjModuleViewManager.LoadByName("NPR", true).StaticModulePagePath;
                        lbl.Text = value;
                        lbl.Attributes.Add("onclick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", value), title, sourceURL, 90, 90));
                        lbl.Attributes.Add("style", "cursor:pointer;color:rgb(0, 0, 102);");
                        lbl.Attributes.Add("onmouseover", "this.style.textDecoration='underline'");
                        lbl.Attributes.Add("onmouseout", "this.style.textDecoration=''");
                    }

                }

                //// In case the RelatedTicketId is the field and it is on view mode the Id should be a link to the ticket.
                if (field.ColumnName == DatabaseObjects.Columns.RelatedRequestID && Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]) != "N/A" && Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]) != string.Empty)
                {
                    HyperLink lf = new HyperLink();

                    int moduleId = uHelper.getModuleIdByTicketID(context, Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));
                    lf = uHelper.GetHyperLinkControlForTicketID(context, moduleId, Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));
                    lf.ID = GenerateID(field, tabId);

                    if (!string.IsNullOrEmpty(lf.ToolTip))
                        lf.Text += Constants.Separator7 + " " + UGITUtility.TruncateWithEllipsis(lf.ToolTip, 20);

                    paneCtrl.Controls.Add(lf);
                    return paneCtrl;
                }

                paneCtrl.Controls.Add(lbl);

                return paneCtrl;
            }
            Control ctr = tf;
            if (field.ColumnName == DatabaseObjects.Columns.Comment || field.ColumnName==DatabaseObjects.Columns.TicketNoOfFTEs)
            {
                Label labelComment = new Label();
                labelComment.Text = field.ColumnName;
                labelComment.ID = GenerateID(field.ToString(), tabId) + "_View";
                labelComment.CssClass += " doubleWidth " + viewcss;
                labelComment.EnableViewState = true;
                labelComment.Text = value;
                ctr = labelComment;
                paneCtrl.Controls.Add(ctr);
                return paneCtrl;
            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketDescription || field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
            {
                tf.CssClass += " descExtraHeightWithDoubleWidth " + field.ColumnName;
            }

            if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
            {
                tf.Text = string.Empty;
                tf.EnableViewState = true;
                paneCtrl.Controls.Add(tf);
                return paneCtrl;
            }
            ctr = tf;
            TextBox batchEditviewControl = new TextBox();

            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                paneCtrl.Controls.Add(batchHidden);
                if (field.ColumnName == DatabaseObjects.Columns.Status)
                {
                    paneCtrl.Controls.Add(batchEditviewControl);
                    return paneCtrl;
                }

                tf.Text = "";
                paneCtrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, tf, batchEditviewControl, mode, batchHidden.ID));
                return paneCtrl;
            }

            Label cView = new Label();
            cView.ID = GenerateID(field.ToString(), tabId) + "_view";
            cView.CssClass += " " + viewcss;
            cView.Text = !string.IsNullOrEmpty(value) ? value : "-";

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                if (field.ColumnName == DatabaseObjects.Columns.ApproxContractValue || field.ColumnName == "IntUnpaidCosts" || field.ColumnName == "CurrentBudget" || field.ColumnName == "ProjectBillings" || field.ColumnName == "BudgetAmount" || field.ColumnName == "IntClientPaid" || field.ColumnName == "ProjectCost" || field.ColumnName == "ExtUnpaidCosts" || field.ColumnName == "ExtClientPaid")
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        //cView.Text = $"$ {value}";
                        cView.Text = string.Format("{0:c0}", Convert.ToDecimal(value));
                    }
                }
                if (field.ColumnName == DatabaseObjects.Columns.UsableSqFt || field.ColumnName == DatabaseObjects.Columns.RetailSqftNum)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        cView.Text = string.Format("{0:n0}", Convert.ToDecimal(value));
                    }
                }
                if (field.ColumnName == DatabaseObjects.Columns.RelatedRequestID)
                {
                    HyperLink lnkToTicket = new HyperLink();

                    //Creating hyperlink if ticket id is available in "value" field
                    if (!string.IsNullOrEmpty(value))
                    {
                        ModuleViewManager objmodule = new ModuleViewManager(context);
                        UGITModule o = objmodule.GetByName(moduleName);
                        int moduleId = uHelper.getModuleIdByTicketID(context, Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));
                        lnkToTicket = uHelper.GetHyperLinkControlForTicketID(context, Convert.ToInt32(o.ID), Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));

                        if (!string.IsNullOrEmpty(lnkToTicket.ToolTip))
                            lnkToTicket.Text += Constants.Separator7 + " " + UGITUtility.TruncateWithEllipsis(lnkToTicket.ToolTip, 20);
                    }
                    else
                    {
                        lnkToTicket.Text = value;
                        lnkToTicket.ToolTip = value;
                    }

                    lnkToTicket.ID = GenerateID(field, tabId) + "_view";
                    return GetControlWithEditButton(field.ColumnName, ctr, lnkToTicket, IsRelatedRequestIDInEdit: true);
                }
                paneCtrl.Controls.Add(GetControlWithEditButton(field.ColumnName, ctr, cView));
                return paneCtrl;
            }

            if (field.ColumnName == DatabaseObjects.Columns.RelatedRequestID && (mode == ControlMode.Edit || mode == ControlMode.New))
            {
                HyperLink lnkToTicket = new HyperLink();

                //Creating hyperlink if ticket id is available in "value" field
                if (!string.IsNullOrEmpty(value))
                {
                    int moduleId = uHelper.getModuleIdByTicketID(context, Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));
                    lnkToTicket = uHelper.GetHyperLinkControlForTicketID(context, moduleId, Convert.ToString(item[DatabaseObjects.Columns.RelatedRequestID]));

                    if (!string.IsNullOrEmpty(lnkToTicket.ToolTip))
                        lnkToTicket.Text += Constants.Separator7 + " " + UGITUtility.TruncateWithEllipsis(lnkToTicket.ToolTip, 20);
                }
                else
                {
                    lnkToTicket.Text = value;
                    lnkToTicket.ToolTip = value;
                }

                lnkToTicket.ID = GenerateID(field, tabId) + "_view";
                return GetControlWithEditButton(field.ColumnName, ctr, lnkToTicket, IsRelatedRequestIDInEdit: true);
            }
            paneCtrl.Controls.Add(ctr);
            return paneCtrl;

        }
        private Control CreateAttachmentControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName + "_edit";
            string viewcss = "field_" + field.ColumnName + "_view";
            Panel paneCtrl = new Panel();
            paneCtrl.CssClass = "field_" + field.ColumnName;
            paneCtrl.CssClass = " editTicket_addDoc_wrap";
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            string value = Convert.ToString(item[field.ColumnName]);
            if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
               && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
            {
                value = Convert.ToString(sourceItem[field.ColumnName]);
            }
            if (batchMode == BatchMode.Edit)
            {
                value = varies;
            }
            FileUploadControl tf = new FileUploadControl();
            tf.ID = GenerateID(field.ColumnName, tabId);
            tf.CssClass += editcss;
            tf.CssClass = "fileUploadIcon";
            tf.EnableViewState = true;
            tf.Width = Unit.Percentage(100);
            tf.controlMode = mode;

            tf.SetValue(value);
            if (mode == ControlMode.Display)
            {
                paneCtrl.Controls.Add(tf);
                return paneCtrl;
            }
            Control ctr = tf;
            if (field.ColumnName == DatabaseObjects.Columns.Comment)
            {
                Label labelComment = new Label();
                labelComment.Text = field.ColumnName;
                labelComment.ID = GenerateID(field.ColumnName, tabId) + "_View";
                labelComment.CssClass += " doubleWidth " + viewcss;
                labelComment.EnableViewState = true;
                labelComment.Text = value;
                ctr = labelComment;
                paneCtrl.Controls.Add(ctr);
                return paneCtrl;
            }
            ctr = tf;
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                paneCtrl.Controls.Add(batchHidden);
                paneCtrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, tf, batchEditviewControl, mode, batchHidden.ID));
                return paneCtrl;
            }
            //// Commented below code, related to Attachments, for showing Edit icon &  guid of attachment files, instead of actual file names (27 Sep 2019)
            //if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            //{
            //    Label cView = new Label();
            //    cView.ID = GenerateID(field.ToString(), tabId) + "_view";
            //    cView.CssClass += " " + viewcss;
            //    cView.Text = value;
            //    paneCtrl.Controls.Add(GetControlWithEditButton(field.ColumnName, ctr, cView));
            //    return paneCtrl;
            //}
            paneCtrl.Controls.Add(ctr);
            return paneCtrl;

        }

        public Control GetControlWithEditButton(string fieldId, Control editControl, Control viewControl, bool IsRelatedRequestIDInEdit = false)
        {
            return GetControlWithEditButton(fieldId, editControl, viewControl, true, IsRelatedRequestIDInEdit: IsRelatedRequestIDInEdit);
        }
        public Control GetControlWithEditButton(string fieldId, Control editControl, Control viewControl, bool displayEditButton, bool IsRelatedRequestIDInEdit = false)
        {
            //Created container control for edit and view
            string prefixId = fieldId;
            Table tableWithControls = new Table();
            tableWithControls.ID = Guid.NewGuid().ToString();
            TableRow containerRow = new TableRow();
            TableCell containerCell = new TableCell();
            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlGenericControl viewSpan = new HtmlGenericControl("span");
            HtmlGenericControl valueViewSpan = new HtmlGenericControl("span");
            valueViewSpan.Attributes.Add("class", "labelvalue");
            valueViewSpan.ID = prefixId + "_viewcontainer1";
            viewSpan.ID = prefixId + "_viewcontainer";
            HtmlGenericControl editSpan = new HtmlGenericControl("span");
            editSpan.ID = prefixId + "_editcontainer";
            tableWithControls.Rows.Add(containerRow);
            containerRow.Cells.Add(containerCell);
            containerCell.Controls.Add(div);

            div.Controls.Add(viewSpan);
            div.Controls.Add(editSpan);

            //Attached click event in container to toggle between view and edit container
            if (displayEditButton)
            {
                if (IsRelatedRequestIDInEdit)
                {
                    viewSpan.Controls.Add(new LiteralControl("<img style='cursor:pointer;display:none;float:left;padding-right:5px;' class='editbutton'  src='/Content/images/editNewIcon.png' onclick='javascript:ShowPicker(this);' alt='Edit'/>"));
                }
                else
                {
                    viewSpan.Attributes.Add("ondblclick", string.Format("javascript:displayEditblock(this, '{0}','{1}')", viewSpan.ID, editSpan.ID));
                    viewSpan.Controls.Add(new LiteralControl(string.Format("<img style='cursor:pointer;display:none;float:left;padding-right:5px;width:20px;margin-top:4px;' class='editbutton'  src='/Content/images/editNewIcon.png' onclick='javascript:displayEditblockOnIcon(this, \"{0}\",\"{1}\")' alt='Edit'/>", viewSpan.ID, editSpan.ID)));
                }
                //new line for show edit button on mouseover.
                viewSpan.Attributes.Add("onmouseover", "javascript:showeditbuttonOnhover(this)");
                viewSpan.Attributes.Add("onmouseout", "javascript: hideeditbuttonOnhover(this)");
                viewSpan.Attributes.Add("style", "min-height:11px;");
            }

            //Added edit and view control in their respective container and displayed viewcontainer default

            valueViewSpan.Controls.Add(viewControl);
            viewSpan.Controls.Add(valueViewSpan);
            viewSpan.Style.Add(HtmlTextWriterStyle.Display, "block");
            editSpan.Controls.Add(editControl);
            editSpan.Style.Add(HtmlTextWriterStyle.Display, "none");
            viewSpan.Attributes.Add("class", "viewcontainer spancontainer");
            editSpan.Attributes.Add("class", "editcontainer spancontainer");
            div.Attributes.Add("class", "div-vieweditcontainer");
            div.Style.Add("float", "left");
            div.Style.Add("min-width", "30px");
            div.Style.Add("width", "100%");
            tableWithControls.Width = new Unit("100%");
            return tableWithControls;
        }
        private string GenerateID(DataColumn field, string tabId)
        {
            return field.ColumnName + '_' + tabId;
        }
        private Control CreateDateFieldControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";

            Panel datePanel = new Panel();
            datePanel.ID = GenerateID(field, tabId) + "Panel";
            datePanel.CssClass = "field_" + field.ColumnName;

            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            ASPxDateEdit aspxDateEdit = new ASPxDateEdit();
            //aspxDateEdit.Width = Unit.Pixel(300);// Unit.Percentage(100);
            aspxDateEdit.ID = GenerateID(field.ColumnName, tabId);
            aspxDateEdit.UseMaskBehavior = true;
            aspxDateEdit.EnableViewState = true;
            aspxDateEdit.DropDownButton.Image.Url = "/Content/images/calendarNew.png";
            aspxDateEdit.DropDownButton.Image.Width = 18;
            //aspxDateEdit.CssClass = "CRMDueDate_inputField";
            aspxDateEdit.CalendarProperties.FooterStyle.CssClass = "calenderFooterWrap";
            aspxDateEdit.EditFormat = EditFormat.Date;
            aspxDateEdit.MinDate = UGITUtility.GetDateTimeMinValue();
            aspxDateEdit.ShowOutOfRangeWarning = true;
            aspxDateEdit.ValidationSettings.ValidateOnLeave = true;
            aspxDateEdit.AllowNull = true;
            aspxDateEdit.NullText = "";
            aspxDateEdit.UseMaskBehavior = false;
            aspxDateEdit.Controls[0].Visible = false;
            aspxDateEdit.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            aspxDateEdit.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Top;
            aspxDateEdit.ValidationSettings.Display = Display.Dynamic;
            // aspxDateEdit.EditFormatString = "MMM/d/yyyy";
            aspxDateEdit.EditFormatString = "MM/dd/yyyy";
            aspxDateEdit.ClientSideEvents.DateChanged = "dateChanged";

            if (field.ColumnName.EqualsTo("EstimatedConstructionEnd"))
            {
                aspxDateEdit.ClientSideEvents.DateChanged = "endConstructionDateChanged";
                aspxDateEdit.ClientInstanceName = "estimatedconstructionendclientname";
            }

            if (field.ColumnName.EqualsTo("CloseoutDate"))
            {
                aspxDateEdit.ClientInstanceName = "closeoutdateclientname";
                DateTime closeOutEndDate = UGITUtility.StringToDateTime(item[field.ColumnName]);
                if (closeOutEndDate == DateTime.MinValue && UGITUtility.IfColumnExists(item, DatabaseObjects.Columns.CloseoutStartDate))
                {
                    DateTime closeOutStartDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (closeOutStartDate != DateTime.MinValue)
                    {
                        aspxDateEdit.Value = closeOutStartDate.AddWorkingDays(uHelper.getCloseoutperiod(context));
                    }
                }
            }

            if (field.ColumnName.EqualsTo("EstimatedConstructionStart"))
            {
                aspxDateEdit.ClientSideEvents.DateChanged = "CRMDurationChanged";
                aspxDateEdit.ClientInstanceName = "estimatedconstructionstartclientname";
            }

            if (field.ColumnName.EqualsTo("PreconStartDate"))
            {
                aspxDateEdit.ClientInstanceName = "preconstartdateclientname";
            }

            if (field.ColumnName.EqualsTo("PreconEndDate"))
            {
                aspxDateEdit.ClientInstanceName = "preconenddateclientname";
            }

            if (!string.IsNullOrEmpty(Convert.ToString(item[field.ColumnName])) && UGITUtility.StringToDateTime(item[field.ColumnName]).Date > UGITUtility.GetDateTimeMinValue().Date)
            {
                aspxDateEdit.Date = UGITUtility.StringToDateTime(item[field.ColumnName]);
            }

            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                aspxDateEdit.Value = Convert.ToString(sourceItem[field.ColumnName]);
            }

            if (mode == ControlMode.New && (field.ColumnName.EqualsTo("EstimatedConstructionEnd") || field.ColumnName.EqualsTo("EstimatedConstructionStart")) && (moduleName == "OPM" || moduleName == "LEM" || moduleName == "CPR"))
            {
                aspxDateEdit.Value = null;
            }

            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);

                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                datePanel.Controls.Add(batchHidden);

                datePanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, aspxDateEdit, batchEditviewControl, mode, batchHidden.ID));
                return datePanel;
            }
            if (mode == ControlMode.Display)
            {
                Label date = new Label();
                date.ID = GenerateID(field, tabId) + "_View";
                date.CssClass = viewcss;
                if (!string.IsNullOrEmpty(Convert.ToString((item[field.ColumnName]))) && UGITUtility.StringToDateTime(item[field.ColumnName]).Date > UGITUtility.GetDateTimeMinValue().Date)
                {

                    date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])), false);

                    //if (aspxDateEdit.DisplayFormatString == "d")
                    //{
                    //    date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])));
                    //}
                    //else
                    //    date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])));
                }
                else
                    date.Text = "-";
                datePanel.Controls.Add(date);
                return datePanel;
            }

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                Label date = new Label();
                date.ID = GenerateID(field, tabId) + "_View";
                date.CssClass = viewcss;
                if (!string.IsNullOrEmpty(Convert.ToString((item[field.ColumnName]))) && UGITUtility.GetDateTimeMinValue() != UGITUtility.StringToDateTime(item[field.ColumnName]))
                {
                    //date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])), false);
                    date.Text = Convert.ToString(item[field.ColumnName]);
                    if (aspxDateEdit.DisplayFormatString == "d")
                        date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])));
                    else
                        date.Text = UGITUtility.GetDateStringInFormat(Convert.ToString((item[field.ColumnName])), false);

                }
                else
                    date.Text = "-";
                datePanel.Controls.Add(aspxDateEdit);
                return GetControlWithEditButton(date.ID, datePanel, date);
            }
            datePanel.Controls.Add(aspxDateEdit);
            return datePanel;
        }
        private Control CreateDateTimeFieldControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";

            Panel datePanel = new Panel();
            datePanel.ID = GenerateID(field, tabId) + "Panel";
            datePanel.CssClass = "field_" + field.ColumnName;

            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            ASPxDateEdit aspxDateEdit = new ASPxDateEdit();
            aspxDateEdit.DropDownButton.Image.Url = "/Content/images/calendarNew.png";
            aspxDateEdit.DropDownButton.Image.Width = 20;
            aspxDateEdit.Width = Unit.Percentage(100);
            aspxDateEdit.ID = GenerateID(field.ColumnName, tabId);
            aspxDateEdit.UseMaskBehavior = true;
            aspxDateEdit.EnableViewState = true;
            aspxDateEdit.EditFormat = EditFormat.Custom;
            aspxDateEdit.EditFormatString = "MMM/d/yyyy hh:mm tt";
            aspxDateEdit.TimeSectionProperties.Visible = true;
            aspxDateEdit.MinDate = UGITUtility.GetDateTimeMinValue();
            aspxDateEdit.ShowOutOfRangeWarning = true;
            aspxDateEdit.ValidationSettings.ValidateOnLeave = true;
            aspxDateEdit.AllowNull = true;
            aspxDateEdit.NullText = "";
            aspxDateEdit.UseMaskBehavior = false;
            aspxDateEdit.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            aspxDateEdit.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Top;
            aspxDateEdit.ValidationSettings.Display = Display.Dynamic;
            aspxDateEdit.TimeSectionProperties.TimeEditProperties.EditFormat = EditFormat.Custom;
            aspxDateEdit.TimeSectionProperties.TimeEditProperties.EditFormatString = "hh:mm tt";
            aspxDateEdit.ClientInstanceName = "CustomEditcontrol";
            aspxDateEdit.ClientSideEvents.DateChanged = "dateChanged";

            if (!string.IsNullOrEmpty(Convert.ToString((item[field.ColumnName]))) && UGITUtility.StringToDateTime(item[field.ColumnName]).Date > UGITUtility.GetDateTimeMinValue().Date)
            {
                aspxDateEdit.Date = UGITUtility.StringToDateTime(item[field.ColumnName]);
            }
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                aspxDateEdit.Value = Convert.ToString(sourceItem[field.ColumnName]);
            }
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                datePanel.Controls.Add(batchHidden);

                datePanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, aspxDateEdit, batchEditviewControl, mode, batchHidden.ID));
                return datePanel;
            }
            if (mode == ControlMode.Display)
            {
                Label date = new Label();
                date.ID = GenerateID(field, tabId) + "_view";
                date.CssClass = viewcss;
                if (!string.IsNullOrEmpty(Convert.ToString((item[field.ColumnName]))) && UGITUtility.GetDateTimeMinValue() != UGITUtility.StringToDateTime(item[field.ColumnName]))
                {
                    date.Text = Convert.ToString(((DateTime)item[field.ColumnName]).Date);

                    if (aspxDateEdit.DisplayFormatString == "d")
                        date.Text = Convert.ToString((item[field.ColumnName]));
                    else
                        date.Text = Convert.ToString((item[field.ColumnName]));

                }
                else
                {
                    date.Text = "-";
                }
                datePanel.Controls.Add(date);
                return datePanel;
            }
            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                Label date = new Label();
                date.ID = GenerateID(field, tabId) + "_view";
                date.CssClass = viewcss;
                date.Text = Convert.ToString(item[field.ColumnName]);//   ToString("MMM-d-yyyy");
                if (item[field.ColumnName] == null || item[field.ColumnName] == DBNull.Value)
                {
                    date.Text = "-";
                }
                else
                {
                    if (aspxDateEdit.DisplayFormatString == "d")
                        date.Text = Convert.ToString((item[field.ColumnName]));//.Date.ToString("MMM-d-yyyy");
                    else
                        date.Text = Convert.ToString((item[field.ColumnName]));//.ToString("MMM-d-yyyy hh:mm tt");
                }


                datePanel.Controls.Add(aspxDateEdit);
                return GetControlWithEditButton(date.ID, datePanel, date);

            }
            datePanel.Controls.Add(aspxDateEdit);
            return datePanel;
        }
        private Control CreateBooleanFieldControl(DataTable dt, DataColumn col, ControlMode mode, ModuleFormLayout coltype)
        {

            ASPxCheckBoxList chk = new ASPxCheckBoxList();
            chk.Items.Add(new ListEditItem("Yes"));
            chk.Items.Add(new ListEditItem("No"));
            chk.RepeatDirection = RepeatDirection.Horizontal;
            Control ctr = chk;
            return ctr;
        }
        private Control CreateNoteFieldControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            Panel paneCtrl = new Panel();
            paneCtrl.CssClass = "field_" + field.ColumnName.ToLower();

            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";

            batchHidden.Value = "True";

            ASPxMemo nf = new ASPxMemo();
            nf.ID = GenerateID(field, tabId);
            nf.Width = Unit.Percentage(100);
            nf.HorizontalAlign = HorizontalAlign.NotSet;
            nf.CssClass = "dxeMemo textarea";
            Control ctr = nf;

            if (field.ColumnName == DatabaseObjects.Columns.TicketDescription || field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
            {
                nf.CssClass += " descExtraHeightWithDoubleWidth " + field.ColumnName;
            }
            else
            {
                nf.CssClass += " extraHeightWithDoubleWidth " + field.ColumnName;
            }

            nf.Text = Convert.ToString(UGITUtility.GetSPItemValue(item, field.ColumnName));
            if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
                nf.Text = string.Empty;
            nf.EnableViewState = true;
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                paneCtrl.Controls.Add(batchHidden);
                paneCtrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, nf, batchEditviewControl, mode, batchHidden.ID));
                return paneCtrl;
            }
            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                Label cView = new Label();
                cView.Text = field.ColumnName;
                cView.ID = GenerateID(field.ToString(), tabId) + "_view";
                cView.CssClass += " " + field.ColumnName;
                cView.Text = Convert.ToString(UGITUtility.GetSPItemValue(item, field.ColumnName));
                return GetControlWithEditButton(cView.ID, ctr, cView);
            }
            return ctr;
        }
        private Control CreateDropDownChoiceField(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel ctrl = new Panel();
            ctrl.CssClass = "field_" + field.ColumnName.ToLower();

            HiddenField batchHidden = new HiddenField();

            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            Label lbl = new Label();
            lbl.Text = Convert.ToString(item[field.ColumnName]);
            lbl.CssClass = viewcss;
            lbl.ID = GenerateID(field.ToString(), tabId) + "_view";
            ASPxComboBox combox = new ASPxComboBox();
            combox.ID = GenerateID(field.ToString(), tabId);
            combox.Width = Unit.Percentage(100);
            combox.Text = Convert.ToString(item[field.ColumnName]);
            combox.EnableClientSideAPI = true;
            combox.CssClass = editcss;
            string[] dataRequestSource = UGITUtility.SplitString(configfield.Data, Constants.Separator);
            string[] dataRequest = null;
            if (configfield.FieldName == DatabaseObjects.Columns.TicketResolutionType)
            {
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup, item.Table) && UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    string rvalue = Convert.ToString(GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.ID + "=" + Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup])).Rows[0][DatabaseObjects.Columns.ResolutionTypes]);
                    if (!string.IsNullOrEmpty(rvalue))
                    {
                        dataRequest = UGITUtility.SplitString(rvalue, Constants.Separator);
                    }
                }
            }
            if (configfield.FieldName == DatabaseObjects.Columns.Category)
            {
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Category, item.Table) && UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    string value = Convert.ToString(GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.ID + "=" + Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup])).Rows[0][DatabaseObjects.Columns.IssueTypeOptions]);
                    if (!string.IsNullOrEmpty(value))
                        dataRequest = UGITUtility.SplitString(value, Constants.Separator);
                }
            }
            if (dataRequest != null)
            {
                dataRequestSource = dataRequest;
            }
            combox.DataSource = dataRequestSource.ToList();
            if (configfield.Multi)
            {

            }
            combox.DataBind();
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                ctrl.Controls.Add(batchHidden);
                combox.Value = "";
                ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, combox, batchEditviewControl, mode, batchHidden.ID));
                return ctrl;
            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketInitiatorResolved)
            {
                combox.CssClass = DatabaseObjects.Columns.TicketInitiatorResolved + " " + viewcss;
                combox.Value = Convert.ToString(sourceItem[field.ColumnName]);
            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketInitiatorResolved && (mode == ControlMode.Edit || mode == ControlMode.New))
            {
                combox.ClientSideEvents.TextChanged = "function(s, e) { enableDisableResolution(); }";
            }
            if (field.ColumnName == DatabaseObjects.Columns.NeedReview && (mode == ControlMode.Edit || mode == ControlMode.New))
            {
                combox.ClientSideEvents.TextChanged = "function(s, e) { enableDisableReviewers(); }";
            }
            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                ctrl.Controls.Add(GetControlWithEditButton(field.ColumnName, combox, lbl));
                return ctrl;
            }
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                combox.Value = sourceItem[field.ColumnName];
            }



            if (mode == ControlMode.Display)
            {
                ctrl.Controls.Add(lbl);
                return ctrl;
            }
            else
            {
                ctrl.Controls.Add(combox);
                return ctrl;
            }
        }
        private Control CreateNumberFieldControl(string fieldColumnType, FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode, bool DecimalNotRequired = true, ModuleFormLayout formLayout = null)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel ctrl = new Panel();
            ctrl.CssClass = "field_" + field.ColumnName.ToLower();
            HiddenField batchHidden = new HiddenField();

            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            NumberValueBox txtNumber = new NumberValueBox();
            if (fieldColumnType == "System.Decimal")
            {
                DecimalNotRequired = false;
                txtNumber.DecimalNotRequired = DecimalNotRequired;
            }
            else
            {
                txtNumber.DecimalNotRequired = DecimalNotRequired;

            }

            txtNumber.FieldName = field.ColumnName;
            txtNumber.Width = Unit.Percentage(100);
            txtNumber.ID = GenerateID(field, tabId);
            txtNumber.FormLayout = formLayout;
            txtNumber.SetValue(Convert.ToString(item[field.ColumnName]));
            Label lblNumber = new Label();

            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                batchEditviewControl.ReadOnly = true;
                batchEditviewControl.EnableViewState = true;
                // batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                txtNumber.SetValue("");

            }

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                string value = Convert.ToString(item[field.ColumnName]);
                Label cView = new Label();
                cView.Text = value;

                if (formLayout != null && !string.IsNullOrEmpty(formLayout.ColumnType) && !string.IsNullOrEmpty(value))
                {
                    if (formLayout.ColumnType.Equals("Currency"))
                    {
                        CultureInfo culture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                        cView.Text = string.Format("{0:c0}", Convert.ToDecimal(value));
                    }
                    else if (formLayout.ColumnType.Equals("Percentage"))
                        cView.Text = string.Format("{0:0.##}%", Convert.ToDecimal(value));

                }

                cView.ID = GenerateID(field, tabId) + "_view";
                //cView.ControlMode = ControlMode.Display;               
                cView.CssClass += " " + viewcss;
                ctrl.Controls.Add(GetControlWithEditButton(cView.ID, txtNumber, cView));
                return ctrl;
            }

            if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.TicketBATotalHours)
            {
                txtNumber.CssClass += " ticketBAtotalHours " + editcss;
            }
            if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.TicketDeveloperTotalHours)
            {
                txtNumber.CssClass += " ticketDeveloperTotalHours " + editcss;
            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketActualHours && mode != ControlMode.New && DatabaseObjects.Columns.ModuleName == "ACR")
            {
                Label l = new Label();
                HiddenField hdn = new HiddenField();
                hdn.ID = GenerateID(field, tabId);
                if (null == item[DatabaseObjects.Columns.TicketActualHours])
                {
                    l.Text = "";
                    hdn.Value = "";
                }
                else
                {
                    l.Text = Convert.ToString(item[field.ColumnName]);
                    hdn.Value = Convert.ToString(item[field.ColumnName]);
                }
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Controls.Add(l);
                div.Controls.Add(hdn);
                div.Attributes.Add("Class", "ticketActualHours " + editcss);
                // l.CssClass = "ticketActualHours";
                return div;
            }
            if (batchMode == BatchMode.Edit)
            {
                ctrl.Controls.Add(batchHidden);
                ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, txtNumber, batchEditviewControl, mode, batchHidden.ID));
                return ctrl;
            }


            if (mode == ControlMode.Display)
            {
                string value = Convert.ToString(item[field.ColumnName]);
                lblNumber.Text = value;

                if (formLayout != null && !string.IsNullOrEmpty(formLayout.ColumnType) && !string.IsNullOrEmpty(value))
                {
                    if (formLayout.ColumnType.Equals("Currency"))
                    {
                        CultureInfo culture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                        lblNumber.Text = string.Format("{0:c0}", Convert.ToDecimal(value));
                    }
                    else if (formLayout.ColumnType.Equals("Percentage"))
                        //lblNumber.Text = string.Format("{0}%", Convert.ToInt64(value));
                        lblNumber.Text = string.Format("{0:0.##}%", Convert.ToDecimal(value));

                }

                ctrl.Controls.Add(lblNumber);
                return ctrl;
            }
            ctrl.Controls.Add(txtNumber);
            return ctrl;
        }
        private Control CreateNoteFieldControl1(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel ctrl = new Panel();
            ctrl.CssClass = "field_" + field.ColumnName.ToLower();
            HiddenField batchHidden = new HiddenField();
            string batchValue = "";
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                {
                    batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                    batchValue = Convert.ToString(item[field.ColumnName]);
                }

                batchEditviewControl.EnableViewState = true;
                // batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                ctrl.Controls.Add(batchHidden);

            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments && mode == ControlMode.New)
            {
                ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UGITModule module = ObjModuleViewManager.GetByName(moduleName);
                TextBox tb = new TextBox();
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Width = Unit.Percentage(100);
                tb.ID = GenerateID(field, tabId);
                tb.CssClass += editcss;
                if (item != null && field.ColumnName != DatabaseObjects.Columns.TicketResolutionComments)
                {
                    tb.Text = Convert.ToString(item[field.ColumnName]);
                }
                if (batchMode == BatchMode.Edit)
                {
                    ((TextBox)tb).Text = "";
                    ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, tb, batchEditviewControl, mode, batchHidden.ID));
                    return ctrl;
                }
                ctrl.Controls.Add(tb);
                return ctrl;

            }
            if (mode == ControlMode.Display)
            {
                if (field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
                {
                    ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    UGITModule module = ObjModuleViewManager.GetByName(moduleName);
                    if (module.ActualHoursByUser)
                    {
                        Page dd = new Page();
                        TicketHour thours = (TicketHour)dd.LoadControl("~/ControlTemplates/uGovernIT/TicketHour.ascx");
                        thours.TicketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        thours.ctmode = mode;
                        thours.ModuleName = moduleName;
                        ctrl.Controls.Add(thours);
                        return ctrl;
                    }
                }
                int tabNumber = 0;
                int.TryParse(tabId, out tabNumber);

                Panel pControl = new Panel();
                pControl.CssClass = "spancontainerdesc field_" + field.ColumnName.ToLower();

                Label cView = new Label();
                pControl.Controls.Add(cView);
                cView.ID = GenerateID(field, tabId);
                cView.Style.Add(HtmlTextWriterStyle.WhiteSpace, "pre-wrap");
                if (field.ColumnName != DatabaseObjects.Columns.TicketResolutionComments)
                    cView.Text = uHelper.ConvertTextAreaStringToHtml(Convert.ToString(UGITUtility.GetSPItemValue(item, field.ColumnName)));

                return pControl;
            }

            else
            {
                if (field.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
                {
                    ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    UGITModule module = ObjModuleViewManager.GetByName(moduleName);
                    if (module.ActualHoursByUser)
                    {
                        Page dd = new Page();
                        TicketHour thours = (TicketHour)dd.LoadControl("~/ControlTemplates/uGovernIT/TicketHour.ascx");
                        thours.TicketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        thours.ctmode = mode;
                        thours.ModuleName = moduleName;
                        ctrl.Controls.Add(thours);
                        return ctrl;
                    }
                }
                TextBox nf = new TextBox();
                nf.TextMode = TextBoxMode.MultiLine;
                nf.ID = GenerateID(field, tabId);
                nf.Width = Unit.Percentage(100);
                nf.CssClass += viewcss;
                if (field.ColumnName == DatabaseObjects.Columns.ResolutionComments || field.ColumnName == DatabaseObjects.Columns.TicketDescription)
                {
                    nf.CssClass.Replace(viewcss, " ");
                }
                if (item != null && UGITUtility.IsSPItemExist(item, field.ColumnName))
                {
                    if (mode == ControlMode.Display)
                        nf.Text = uHelper.ConvertTextAreaStringToHtml(Convert.ToString(item[field.ColumnName]));
                    else
                        nf.Text = Convert.ToString(item[field.ColumnName]);
                }

                if (mode == ControlMode.New && !string.IsNullOrEmpty(Convert.ToString(sourceItem[field.ColumnName])))
                    nf.Text = Convert.ToString(sourceItem[field.ColumnName]);
                if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.ResolutionComments)
                    nf.Text = string.Empty;

                nf.EnableViewState = true;
                if (batchMode == BatchMode.Edit)
                {
                    ((TextBox)nf).Text = batchValue;
                    ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, nf, batchEditviewControl, mode, batchHidden.ID));
                    return ctrl;
                }
                if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
                {
                    int tabNumber = 0;
                    int.TryParse(tabId, out tabNumber);

                    Panel pControl = new Panel();
                    Label cView = new Label();
                    cView.Style.Add(HtmlTextWriterStyle.WhiteSpace, "pre-wrap");
                    pControl.Controls.Add(cView);

                    cView.ID = GenerateID(field, tabId) + "_view";
                    if (field.ColumnName != DatabaseObjects.Columns.TicketResolutionComments)
                    {
                        cView.Text = uHelper.ConvertTextAreaStringToHtml(Convert.ToString(UGITUtility.GetSPItemValue(item, field.ColumnName)));
                    }
                    pControl.CssClass = "spancontainerdesc ";

                    ctrl.Controls.Add(GetControlWithEditButton(field.ColumnName, nf, pControl));
                    return ctrl;
                }

                ctrl.Controls.Add(nf);
                return ctrl;
            }
        }
        private Control CreateCurrencyFieldControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            ASPxTextBox txtCurrency = new ASPxTextBox();
            txtCurrency.Width = Unit.Percentage(100);
            return txtCurrency;
        }
        private Control CreateLookUpValueControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode, string displayName = "")
        {
            //written to fix dropdowns like Impact,Severity not visible while creating New Sub tickets from other modules
            if (sourceItem != null && !sourceItem.Table.Columns.Contains(field.ColumnName))
            {
                if (configfield.Datatype == "UserField" || configfield.Datatype == "Choices")
                    sourceItem.Table.Columns.Add(field.ColumnName, typeof(String));
                else if (configfield.Datatype == "Lookup")
                    sourceItem.Table.Columns.Add(field.ColumnName, typeof(Int64));
            }

            string value = "";
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
                value = Convert.ToString(sourceItem[field.ColumnName]);
            else
                value = Convert.ToString(item[field.ColumnName]);
            string text = value;
            if (!string.IsNullOrWhiteSpace(value))
                text = configFieldManager.GetFieldConfigurationData(field.ColumnName, value);

            if (_enableDivision)
            {
                if (!string.IsNullOrWhiteSpace(configfield.TemplateType) && !string.IsNullOrWhiteSpace(text))
                {
                    if (configfield.FieldName == DatabaseObjects.Columns.DepartmentLookup && configfield.TemplateType == "DepartmentDropDownTemplate")
                    {
                        Department department = _departmentManager.LoadByID(Convert.ToInt64(value));
                        if (department != null && department.DivisionIdLookup.HasValue)
                        {
                            string divisionName = configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.DivisionLookup, department.DivisionIdLookup.Value.ToString());
                            text = divisionName + " > " + text;
                        }
                    }
                } 
            }
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel priorityPanel = new Panel();
            priorityPanel.CssClass = "field_" + field.ColumnName.ToLower();
            string batchValue = "";
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            Label lookupLabel = new Label();
            lookupLabel.ID = GenerateID(field, tabId) + "_view";
            lookupLabel.CssClass = viewcss;
            TextBox batchEditviewControl = new TextBox();
            lookupLabel.Text = text;
            batchEditviewControl.Text = text;
            ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(context);
            bool enableStudioDivisionHierarchy = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);

            if (mode == ControlMode.Edit && field.ColumnName == DatabaseObjects.Columns.LeadSource)
            {

                if (!string.IsNullOrEmpty(value))
                {
                    DataRow[] dr = GetTableDataManager.GetTableData(ObjModuleViewManager.GetModuleTableName("CON"), $"{DatabaseObjects.Columns.TicketId} in ('{value}') and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'", $"{DatabaseObjects.Columns.Title}, CRMContactType, {DatabaseObjects.Columns.Telephone}, {DatabaseObjects.Columns.EmailAddress}, {DatabaseObjects.Columns.TicketId}", string.Empty).Select();

                    if (dr.Length > 0)
                    {
                        string viewUrl = string.Empty;
                        string title = text;
                        string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                        viewUrl = ObjModuleViewManager.LoadByName("CON").StaticModulePagePath;
                        lookupLabel.Attributes.Add("onClick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", dr[0][DatabaseObjects.Columns.TicketId]), string.Format("{0}: {1}", dr[0][DatabaseObjects.Columns.TicketId], dr[0][DatabaseObjects.Columns.Title]), sourceURL, 90, 90));

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

                        lookupLabel.Attributes.Add("contactId", value);
                        lookupLabel.CssClass = viewcss + " jqtooltip";
                        lookupLabel.ToolTip = Convert.ToString(sbContact);
                        lookupLabel.Attributes.Add("style", "cursor:pointer");
                    }
                }

            }
            /*
            if (mode == ControlMode.Edit && (field.ColumnName == DatabaseObjects.Columns.CRMCompanyTitleLookup))
            {

                //string viewUrl = string.Empty;
                //DataRow dr = null;
                //string title = text; 
                //string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);

                //if (field.ColumnName == DatabaseObjects.Columns.CRMCompanyTitleLookup)
                //{
                //    dr = GetTableDataManager.GetTableData(ObjModuleViewManager.GetModuleTableName("COM"), $"{DatabaseObjects.Columns.ID} = {value} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'", $"{DatabaseObjects.Columns.TicketId}, {DatabaseObjects.Columns.Telephone}", string.Empty).Select()[0];
                //    viewUrl = ObjModuleViewManager.LoadByName("COM").StaticModulePagePath;
                //}

                //lookupLabel.Attributes.Add("onClick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", dr[DatabaseObjects.Columns.TicketId]), string.Format("{0}: {1}", dr[DatabaseObjects.Columns.TicketId], title), sourceURL, 90, 90));
                //lookupLabel.Attributes.Add("style", "cursor:pointer");

                //if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.Telephone])))
                //{
                //    lookupLabel.CssClass = viewcss + " jqtooltip";
                //    lookupLabel.ToolTip = $"Main Phone: {dr[DatabaseObjects.Columns.Telephone]}";
                //}           
            }
            */
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                    batchEditviewControl.Text = varies;
                else
                {
                    if (configfield.Datatype == "Choices")
                    {
                        batchEditviewControl.Text = Convert.ToString(item[field.ColumnName]);
                        batchValue = Convert.ToString(item[field.ColumnName]);
                    }
                }
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
            }
            priorityPanel.CssClass = "field_" + field.ColumnName.ToLower();
            #region TicketPriority
            if (field.ColumnName == DatabaseObjects.Columns.TicketPriority && !Ticket.PriorityMappingEnabled(moduleObj) &&
                    (mode == ControlMode.Edit || mode == ControlMode.New))
            {
                DropDownList priority = new DropDownList();
                priority.CssClass = "itsmDropDownList aspxDropDownList field_prioritylookup";
                priority.ID = GenerateID(field, tabId);
                priority.Attributes.Add("title", "Priority");
                priority.Attributes.Add("name", "Priority");
                priority.DataSource = moduleObj.List_Priorities;
                priority.EnableViewState = true;
                priority.DataValueField = DatabaseObjects.Columns.ID;
                priority.DataTextField = DatabaseObjects.Columns.Title;
                priority.DataBind();
                priorityPanel.Controls.Add(priority);
                priority.Items.Insert(0, new ListItem("", ""));
                if (mode == ControlMode.Edit && item[DatabaseObjects.Columns.TicketPriorityLookup] != null)
                    priority.SelectedValue = item[DatabaseObjects.Columns.TicketPriorityLookup].ToString().Split(';')[0];

                if (sourceItem != null && !string.IsNullOrWhiteSpace(Convert.ToString(sourceItem[field.ColumnName])))
                {
                    string sourceItemPriority = sourceItem[DatabaseObjects.Columns.TicketPriorityLookup].ToString().Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
                    priority.SelectedValue = sourceItemPriority;

                }
                else if (string.IsNullOrWhiteSpace(Convert.ToString(item[field.ColumnName])))
                {
                    priority.SelectedValue = "";
                }

                if (batchMode == BatchMode.Edit)
                {
                    priority.Items.Insert(0, new ListItem(varies, "-1"));
                    priority.SelectedValue = "-1";
                }

                if (designMode == FieldDesignMode.WithEdit)
                {
                    Panel paneRity = new Panel();
                    paneRity.Controls.Add(batchHidden);
                    paneRity.Controls.Add(GetControlBatchEditButton(field.ColumnName, priorityPanel, lookupLabel, mode, batchHidden.ClientID));
                    return paneRity;
                }
                else
                    return priorityPanel;
            }
            else if (field.ColumnName == DatabaseObjects.Columns.TicketPriority)
            {
                Label priority = new Label();
                priority.ID = GenerateID(field.ColumnName, tabId) + "_view";
                priority.CssClass += "priority " + viewcss;
                if (string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.TicketPriorityLookup])))
                {
                    priority.Text = "No Priority Assigned";
                }
                else
                {
                    priority.Text = text;
                }

                priority.EnableViewState = false;
                priorityPanel.Controls.Add(priority);

                CheckBox chkVIP = new CheckBox();
                chkVIP.ID = GenerateID(field, tabId) + "_elevatecheck";
                chkVIP.Text = "Elevate";
                chkVIP.CssClass = "chkelevatecheck ";
                chkVIP.Attributes.Add("onclick", "onSetPriorityElevated(this)");

                string priorityLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketPriorityLookup));
                List<ModulePrioirty> modulePrioirtyList = this.moduleObj.List_Priorities;
                PrioirtyViewManager objPrioirtyViewManager = new PrioirtyViewManager(context);
                ModulePrioirty elevatedPriority = objPrioirtyViewManager.GetElevatedPriority(modulePrioirtyList);
                if (elevatedPriority == null)
                {
                    chkVIP.Visible = false;
                }
                else if (elevatedPriority != null && priorityLookup != null && priorityLookup == Convert.ToString(elevatedPriority.ID))
                {
                    chkVIP.Checked = true;
                    chkVIP.Visible = true;
                }
                if (mode == ControlMode.New || mode == ControlMode.Edit)
                    priorityPanel.Controls.Add(chkVIP);

                if (batchMode == BatchMode.Edit)
                {
                    Panel paneRity = new Panel();
                    paneRity.Controls.Add(batchHidden);
                    paneRity.Controls.Add(GetControlBatchEditButton(field.ColumnName, priorityPanel, batchEditviewControl, mode, batchHidden.ClientID));
                    return paneRity;
                }

                return priorityPanel;
            }
            #endregion

            /*
            if (field.ColumnName == DatabaseObjects.Columns.TicketOPMIdLookup || field.ColumnName == DatabaseObjects.Columns.TicketLEMIdLookup)
            {
                string viewUrl = string.Empty;
                string title = $"{text} : {item[DatabaseObjects.Columns.Title]}";
                string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);

                if (field.ColumnName == DatabaseObjects.Columns.TicketOPMIdLookup)
                    viewUrl = ObjModuleViewManager.LoadByName("OPM").StaticModulePagePath;
                else if (field.ColumnName == DatabaseObjects.Columns.TicketLEMIdLookup)
                    viewUrl = ObjModuleViewManager.LoadByName("LEM").StaticModulePagePath;

                lookupLabel.Attributes.Add("onclick", string.Format("javascript:openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", text), title, sourceURL, 90, 90));
                lookupLabel.Attributes.Add("style", "cursor:pointer;color:rgb(0, 0, 102);");
                lookupLabel.Attributes.Add("onmouseover", "this.style.textDecoration='underline'");
                lookupLabel.Attributes.Add("onmouseout", "this.style.textDecoration=''");
            }
            */
            if (field.ColumnName == DatabaseObjects.Columns.AssetLookup && (mode == ControlMode.Display))
            {
                string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                string viewUrl = ObjModuleViewManager.LoadByName(ModuleNames.CMDB).StaticModulePagePath;

                Panel mlfPanel = new Panel();
                mlfPanel = AssetHelper.GetAssetLink(value, field, tabId, viewUrl, sourceURL) as Panel;
                priorityPanel.Controls.Add(mlfPanel);
                return priorityPanel;



            }

            if (field.ColumnName == DatabaseObjects.Columns.AssetModelLookup && (mode == ControlMode.Display))
            {
                string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);

                Panel mlfPanel = new Panel();
                if (string.IsNullOrEmpty(value))
                    value = "0";
                mlfPanel = AssetHelper.GetAssetModelLink(value, field, tabId, sourceURL) as Panel;
                priorityPanel.Controls.Add(mlfPanel);
                return priorityPanel;
            }
            //[+][SANKET][24-08-2023][Added condition for Verticals and Geographies]
            if ((field.ColumnName == DatabaseObjects.Columns.TagMultiLookup && configfield.ParentTableName == DatabaseObjects.Tables.ExperiencedTags))
            {
                Page dd = new Page();
                CustomListDropDown mlf = (CustomListDropDown)dd.LoadControl("~/ControlTemplates/Utility/CustomListDropDown.ascx");
                mlf.ID = GenerateID(field, tabId);
                mlf.ControlMode = mode;
                mlf.FieldDesignMode = designMode;
                mlf.ControlList = configfield.ParentTableName;
                mlf.FieldName = field.ColumnName;
                mlf.Value = value;
                mlf.IsMult = true;
                if (mode == ControlMode.Edit)
                    mlf.EnableEditButton = true;
                if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId) && configfield.ParentTableName == DatabaseObjects.Tables.CRMContact)
                {
                    mlf.TicketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                }
                mlf.SetValue(Convert.ToString(item[field.ColumnName]));
                if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
                   && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
                {
                    if (field.ColumnName == DatabaseObjects.Columns.ContactLookup)
                        mlf.SetValue(Convert.ToString(sourceItem[field.ColumnName]));
                }

                if (batchMode == BatchMode.Edit)
                {
                    mlf.SetValue(varies);
                }

                return mlf;
            }


            if (!string.IsNullOrWhiteSpace(configfield.TemplateType))
            {
                Panel pctr = new Panel();
                LookupValueBoxEdit dropDownBox = new LookupValueBoxEdit();

                if (configfield.TemplateType == "DepartmentDropDownTemplate")
                {
                    dropDownBox.CssClass = "nprPrimeBenificiaries_dropDown";
                }
                dropDownBox.dropBox.CssClass = editcss;
                dropDownBox.ModuleName = moduleName;
                if (configField.Multi)
                    dropDownBox.IsMulti = true;
                dropDownBox.FieldName = field.ColumnName;
                dropDownBox.ID = GenerateID(field, tabId);
                if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
                {
                    dropDownBox.Value = Convert.ToString(sourceItem[field.ColumnName]);
                    dropDownBox.dropBox.KeyValue = Convert.ToString(sourceItem[field.ColumnName]);
                    dropDownBox.dropBox.Value = Convert.ToString(sourceItem[field.ColumnName]);
                }
                else
                {
                    dropDownBox.Value = Convert.ToString(item[field.ColumnName]);
                    dropDownBox.dropBox.KeyValue = Convert.ToString(item[field.ColumnName]);
                    dropDownBox.dropBox.Value = Convert.ToString(item[field.ColumnName]);
                }
                //if(field.ColumnName=="AssetLookup" && item.Table.Columns.Contains("AssetMultiLookup"))
                //{
                //    if (item[field.ColumnName] != DBNull.Value)
                //    {
                //        if (Convert.ToInt64(item[field.ColumnName]) > 0)
                //            dropDownBox.Value = Convert.ToString(item[field.ColumnName]);
                //        else
                //            dropDownBox.Value = Convert.ToString(item["AssetMultiLookup"]);
                //    }
                //}
                // else



                dropDownBox.dropBox.Text = lookupLabel.Text;
                if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    dropDownBox.dropBox.ClientSideEvents.ValueChanged = string.Format("function(s,e){{requestTypeSelectionChanged(s,e, '{0}');}}", moduleObj.OwnerBindingChoice);

                dropDownBox.DisplayName = displayName;
                if (batchMode == BatchMode.Edit)
                {
                    dropDownBox.dropBox.Value = "";
                    pctr.Controls.Add(batchHidden);
                    pctr.Controls.Add(GetControlBatchEditButton(field.ColumnName, dropDownBox, batchEditviewControl, mode, batchHidden.ID));
                    return pctr;
                }
                else if (mode == ControlMode.Display)
                {
                    pctr.Controls.Add(lookupLabel);
                    return pctr;
                }
                if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
                {
                    pctr.Controls.Add(GetControlWithEditButton(lookupLabel.ID, dropDownBox, lookupLabel));
                    return pctr;
                }
                pctr.Controls.Add(dropDownBox);
                return pctr;


            }

            //LookUpValueBox lookupCtrl = new LookUpValueBox();
            lookupCtrl = new LookUpValueBox();
            lookupCtrl.devexListBox.CssClass = editcss;
            lookupCtrl.devexListBox.ClientSideEvents.ValueChanged = "rowClick";
            lookupCtrl.FieldName = configfield.FieldName;
            lookupCtrl.ModuleName = moduleObj.ModuleName;
            lookupCtrl.ID = GenerateID(field, tabId);
            lookupCtrl.Mode = mode;
            if (!string.IsNullOrEmpty(Convert.ToString(item[field.ColumnName])))
            {
                lookupCtrl.IsMulti = configfield.Multi;
                lookupCtrl.SetValues(value);
            }
            else if (sourceItem != null && UGITUtility.IfColumnExists(field.ColumnName, sourceItem.Table) && Convert.ToString(sourceItem[field.ColumnName]) != string.Empty)
            {
                string sourceItemPriority = Convert.ToString(value).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
                lookupCtrl.SetValues(sourceItemPriority);
            }
            else if (Convert.ToString(item[field.ColumnName]) == string.Empty)
            {
                lookupCtrl.SetValues(string.Empty);
            }
            if (field.ColumnName == DatabaseObjects.Columns.StudioLookup && enableStudioDivisionHierarchy)
            {
                if (item != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.DivisionLookup, item.Table) && Convert.ToString(item[DatabaseObjects.Columns.DivisionLookup]) != string.Empty)
                    lookupCtrl.FilterExpression = $"{DatabaseObjects.Columns.DivisionLookup} = '{Convert.ToString(item[DatabaseObjects.Columns.DivisionLookup])}'";
            }
            if (batchMode == BatchMode.Edit)
            {
                priorityPanel.Controls.Add(batchHidden);
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                {
                    batchEditviewControl.Text = varies;
                    lookupCtrl.SetValues("");
                }
                else
                {
                    if (configfield.Datatype == "Choices")
                    {
                        batchEditviewControl.Text = value;
                        lookupCtrl.SetValues(value);
                    }
                    else
                    {
                        lookupCtrl.SetValues(value);
                    }
                }

                priorityPanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, lookupCtrl, batchEditviewControl, mode, batchHidden.ID));
                return priorityPanel;
            }
            if (mode == ControlMode.New)
            {
                if (sourceItem != null)
                {
                    if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketImpactLookup))
                    {
                        lookupCtrl.SetValues(Convert.ToString(sourceItem[DatabaseObjects.Columns.TicketImpactLookup]));
                    }
                    if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketSeverityLookup))
                    {
                        lookupCtrl.SetValues(Convert.ToString(sourceItem[DatabaseObjects.Columns.TicketSeverityLookup]));
                    }
                    if (field.ColumnName == DatabaseObjects.Columns.TicketInitiatorResolved)
                    {
                        lookupCtrl.SetValues(Convert.ToString(sourceItem[field.ColumnName]));
                    }
                }
                designMode = FieldDesignMode.Normal;

            }
            if (field.ColumnName == DatabaseObjects.Columns.TicketInitiatorResolved && (mode == ControlMode.Edit || mode == ControlMode.New))
            {
                lookupCtrl.CssClass = DatabaseObjects.Columns.TicketInitiatorResolved + " " + viewcss;

            }
            else if (mode == ControlMode.Display)
            {
                priorityPanel.Controls.Add(lookupLabel);
                return priorityPanel;
            }

            //if (mode != ControlMode.New && field.ColumnName == DatabaseObjects.Columns.SuccessChance)
            //{
            //    HtmlGenericControl vSpan = new HtmlGenericControl("div");
            //    vSpan.Attributes.Add("class", "leadRanking-icon");
            //    Image addIcon = new Image();
            //    addIcon.ToolTip = "Lead Ranking";
            //    addIcon.ImageUrl = "~/Content/images/Ranking.png";
            //    addIcon.Attributes.Add("onclick", "ShowLeadRanking('" + Convert.ToString(item[DatabaseObjects.Columns.TicketId]) + "')");
            //    addIcon.Attributes.Add("style", "cursor:pointer");
            //    vSpan.Controls.Add(addIcon);
            //    lookupCtrl.Controls.Add(vSpan);
            //}

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                priorityPanel.Controls.Add(GetControlWithEditButton(lookupCtrl.ID, lookupCtrl, lookupLabel));
                return priorityPanel;
            }
            lookupCtrl.CssClass = "nprLookuplist";
            priorityPanel.Controls.Add(lookupCtrl);


            return priorityPanel;
        }
        private Control CreateMultipleLookupFieldControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode, string displayName = "")
        {
            #region .Net code
            if (sourceItem != null && !sourceItem.Table.Columns.Contains(field.ColumnName))
            {
                if (configfield.Datatype == "UserField" || configfield.Datatype == "Choices")
                    sourceItem.Table.Columns.Add(field.ColumnName, typeof(String));
                else if (configfield.Datatype == "Lookup")
                    sourceItem.Table.Columns.Add(field.ColumnName, typeof(Int64));
            }

            string value = "";
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
                value = Convert.ToString(sourceItem[field.ColumnName]);
            else
                value = Convert.ToString(item[field.ColumnName]);
            string text = value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                if (field.ColumnName == DatabaseObjects.Columns.TicketBeneficiaries)
                {
                    values.Add("@TenantID", context.TenantID);
                    values.Add("@DeptId", value);
                    DataTable dt = GetTableDataManager.GetData("DepartmentDetails", values);
                    List<string> textvalues = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        textvalues.Add(Convert.ToString(dr["CombinedTitle"]));
                    }
                    text = string.Join(", ", textvalues);
                }
                else
                    text = configFieldManager.GetFieldConfigurationData(field.ColumnName, value);
            }




            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel priorityPanel = new Panel();
            priorityPanel.CssClass = "field_" + field.ColumnName.ToLower();
            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            Label lookupLabel = new Label();
            lookupLabel.ID = GenerateID(field, tabId) + "_view";
            lookupLabel.CssClass = viewcss;
            TextBox batchEditviewControl = new TextBox();
            lookupLabel.Text = text;
            batchEditviewControl.Text = text;

            if (field.ColumnName.Contains(DatabaseObjects.Columns.AssetLookup) && (mode == ControlMode.New || mode == ControlMode.Edit))
            {
                Page dd = new Page();
                AssetLookupDropDownList mlf = (AssetLookupDropDownList)dd.LoadControl("~/controltemplates/uGovernIT/AssetLookupDropDownList.ascx");
                mlf.ID = GenerateID(field, tabId).Replace("-", "");
                mlf.currentModuleName = moduleName;
                mlf.DisableCustomFilter = true;
                if (!string.IsNullOrWhiteSpace(mlf.currentModuleName) && (mlf.currentModuleName == ModuleNames.TSR || mlf.currentModuleName == ModuleNames.PRS))
                    mlf.DisableCustomFilter = false;
                UserProfile user = UserManager.GetUserByUserName(context.CurrentUser.Name);
                mlf.SetDependentFieldValue(context.CurrentUser.Id);

                if (item[DatabaseObjects.Columns.AssetLookup] != null && UGITUtility.ObjectToString(item[DatabaseObjects.Columns.AssetLookup]) !="0")
                {

                    mlf.SetValues(Convert.ToString(item[DatabaseObjects.Columns.AssetLookup]));
                }
                else if (item != null && sourceItem != null)
                {
                    mlf.SetValues(Convert.ToString(item[DatabaseObjects.Columns.AssetLookup]));
                }
                Panel mlfPanel = new Panel();

                priorityPanel.Controls.Add(mlf);
                if (batchMode == BatchMode.Edit)
                {

                    batchEditviewControl.ReadOnly = true;
                    //batchEditviewControl.Width = Unit.Percentage(100);
                    batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                    batchEditviewControl.BackColor = System.Drawing.Color.Transparent;

                    batchEditviewControl.EnableViewState = true;
                    batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                    batchEditviewControl.Attributes.Add("Style", "border:none;");
                    priorityPanel.Controls.Add(batchHidden);
                    DataView view = new DataView(list);
                    DataTable distinctValues = view.ToTable(true, field.ColumnName);
                    if (distinctValues.Rows.Count > 0)
                    {
                        batchEditviewControl.Text = varies;
                        lookupCtrl.SetValues("");
                    }
                    priorityPanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, mlf, batchEditviewControl, mode, batchHidden.ID));
                }
            }
            else if (field.ColumnName.Contains(DatabaseObjects.Columns.AssetLookup) && (mode == ControlMode.Display))
            {
                string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                string viewUrl = ObjModuleViewManager.LoadByName(ModuleNames.CMDB).StaticModulePagePath;

                Panel mlfPanel = new Panel();
                mlfPanel = AssetHelper.GetAssetLink(value, field, tabId, viewUrl, sourceURL) as Panel;
                priorityPanel.Controls.Add(mlfPanel);
            }
            if (!string.IsNullOrWhiteSpace(configfield.TemplateType))
            {
                Panel pctr = new Panel();
                LookupValueBoxEdit dropDownBox = new LookupValueBoxEdit();

                if (configfield.TemplateType == "DepartmentDropDownTemplate")
                {
                    dropDownBox.CssClass = "nprPrimeBenificiaries_dropDown";
                }
                dropDownBox.dropBox.CssClass = editcss;
                dropDownBox.ModuleName = moduleName;

                if (configField.Multi)
                    dropDownBox.IsMulti = true;
                dropDownBox.FieldName = field.ColumnName;
                dropDownBox.ID = GenerateID(field, tabId);
                if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
                {
                    dropDownBox.Value = Convert.ToString(sourceItem[field.ColumnName]);
                    dropDownBox.dropBox.KeyValue = Convert.ToString(sourceItem[field.ColumnName]);
                    dropDownBox.dropBox.Value = Convert.ToString(sourceItem[field.ColumnName]);
                }
                else
                {
                    dropDownBox.Value = Convert.ToString(item[field.ColumnName]);
                    dropDownBox.dropBox.KeyValue = Convert.ToString(item[field.ColumnName]);
                    dropDownBox.dropBox.Value = Convert.ToString(item[field.ColumnName]);
                }

                dropDownBox.dropBox.Text = lookupLabel.Text;
                //if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                //    dropDownBox.dropBox.ClientSideEvents.ValueChanged = string.Format("function(s,e){{requestTypeSelectionChanged(s,e, '{0}');}}", moduleObj.OwnerBindingChoice);

                dropDownBox.DisplayName = displayName;
                if (batchMode == BatchMode.Edit)
                {
                    dropDownBox.dropBox.Value = "";
                    pctr.Controls.Add(batchHidden);
                    pctr.Controls.Add(GetControlBatchEditButton(field.ColumnName, dropDownBox, batchEditviewControl, mode, batchHidden.ID));
                    return pctr;
                }
                else if (mode == ControlMode.Display)
                {
                    lookupLabel.ToolTip = Convert.ToString(text);
                    lookupLabel.Attributes.Add("style", "cursor:pointer");
                    pctr.Controls.Add(lookupLabel);
                    return pctr;
                }
                if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
                {
                    pctr.Controls.Add(GetControlWithEditButton(lookupLabel.ID, dropDownBox, lookupLabel));
                    return pctr;
                }
                pctr.Controls.Add(dropDownBox);
                return pctr;


            }
            else if (mode == ControlMode.Display || mode == ControlMode.Edit)
            {
                lookupLabel.ToolTip = Convert.ToString(text);
                lookupLabel.Attributes.Add("style", "cursor:pointer");
                priorityPanel.Controls.Add(lookupLabel);
                return priorityPanel;
            }
            return priorityPanel;
            #endregion
        }
        private class MyTemplate : ITemplate
        {
            Control ctrl = null;
            public MyTemplate(Control ctrl1)
            {
                ctrl = ctrl1;
            }
            public void InstantiateIn(System.Web.UI.Control container)
            {
                container.Controls.Add(ctrl);
            }
        }
        private Control CreateLookupFieldControl1(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            #region commentedcode
            Label lookupLabel = new Label();
            lookupLabel.ID = GenerateID(field, tabId) + "_view";
            if (Convert.ToString(item[field.ColumnName]) != string.Empty)
            {
                FieldLookupValue lookupVals = new FieldLookupValue(Convert.ToInt32(item[field.ColumnName]), configfield.ParentFieldName, configfield.ParentTableName);
                lookupLabel.Text = lookupVals.Value;
            }
            if (configfield.ParentTableName == DatabaseObjects.Tables.Department)
            {
                Page dd = new Page();
                DepartmentDropdownList mlf = (DepartmentDropdownList)dd.LoadControl("~/ControlTemplates/uGovernIT/DepartmentDropdownList.ascx");
                mlf.SetValue(Convert.ToString(item[field.ColumnName]));
                mlf.ID = GenerateID(field, tabId);
                mlf.IsMulti = false;
                // mlf.ControlMode = mode;
                mlf.EnableHoverEdit = true;

                if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
                   && sourceItem[field.ColumnName] != null && mode != ControlMode.Display)
                {
                    mlf.SetValue(Convert.ToString(sourceItem[field.ColumnName]));
                }

                if (batchMode == BatchMode.Edit)
                {
                    mlf.SetValue(varies);
                }
                return mlf;
            }
            else
            {
                LookUpValueBox lookupCtrl = new LookUpValueBox();
                lookupCtrl.FieldName = configfield.FieldName;
                lookupCtrl.ID = GenerateID(field, tabId);
                lookupCtrl.SetValues(Convert.ToString(item[field.ColumnName]));
                if (mode == ControlMode.Edit && item[field.ColumnName] != null)
                    lookupCtrl.SetValues(Convert.ToString(item[field.ColumnName]));

                if (sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
                {
                    string sourceItemPriority = Convert.ToString(sourceItem[field.ColumnName]).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
                    lookupCtrl.SetValues(sourceItemPriority);
                }
                else if (Convert.ToString(item[field.ColumnName]) == string.Empty)
                {
                    lookupCtrl.SetValues("");
                }

                if (batchMode == BatchMode.Edit)
                {
                    lookupCtrl.SetValues("");
                }
                if (mode == ControlMode.New)
                    designMode = FieldDesignMode.Normal;
                if (designMode == FieldDesignMode.WithEdit)
                {
                    return GetControlWithEditButton(lookupCtrl.ID, lookupCtrl, lookupLabel);
                }
                else
                {
                    return lookupCtrl;
                }
            }
            #endregion
        }

        public void cbx_Load(object sender, EventArgs e)
        {
            ASPxGridLookup cbx = (ASPxGridLookup)sender;
            string selectedSOWID = string.Empty;
            DataTable coll = null;
            if (cbx.DataSource == null)
            {
                if (cbx.GridView.JSProperties.Keys.Contains("cpValue"))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(cbx.GridView.JSProperties["cpValue"])) && cbx.GridView.JSProperties.Count > 0)
                        selectedSOWID = Convert.ToString(cbx.GridView.JSProperties["cpValue"]);
                }

                if (!string.IsNullOrEmpty(cbx.Attributes["selectedValue"]))
                    selectedSOWID = cbx.Attributes["selectedValue"];

                if (cbx.Page.IsPostBack && cbx.Value != null)
                    selectedSOWID = Convert.ToString(cbx.Value);

                if (Convert.ToString(cbx.Attributes["lookupList"]) == DatabaseObjects.Tables.AssetModels)
                {
                    //coll = uGITCache.GetDataTable(DatabaseObjects.Tables.AssetModels);
                    //if (coll != null)
                    //    coll = coll.Copy();
                }
                else
                {
                    string selectQuery = string.Empty;
                    string lookupList = Convert.ToString(cbx.Attributes["LookupList"]);
                    if (lookupList == null)
                        return;

                    DataTable dt = GetTableDataManager.GetTableData(lookupList, "");
                    DataRow[] dr = new DataRow[0];
                    selectQuery = string.Format("({0} = 'True')", DatabaseObjects.Columns.Deleted);


                    if (Convert.ToBoolean(dt.Columns.Contains(DatabaseObjects.Columns.Deleted)) == true)
                        dr = dt.Select(selectQuery);
                    else
                        dr = dt.Select();


                    //query.Query = "<Where><Neq><FieldRef Name='IsDeleted' /><Value Type='Boolean'>1</Value></Neq></Where>";


                    //query.Query = "<Where></Where>";


                    //if (lookupList.Title == DatabaseObjects.Lists.VendorSOW)
                    //{
                    //    string msaID = string.Empty;
                    //    if (!string.IsNullOrEmpty(cbx.Attributes["MSAID"]))
                    //    {
                    //        msaID = cbx.Attributes["MSAID"];
                    //    }
                    //    string msakey = cbx.Request.QueryString.AllKeys.FirstOrDefault(x => x != null && x.StartsWith("js_" + DatabaseObjects.Columns.VendorMSALookup));
                    //    if (!string.IsNullOrEmpty(msakey) && msakey.Trim() != string.Empty)
                    //    {
                    //        string keyValue = cbx.Request.QueryString[msakey];
                    //        query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.VendorMSALookup, keyValue);
                    //    }
                    //    else if (!string.IsNullOrEmpty(msaID) && msaID != "0")
                    //    {
                    //        query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.VendorMSALookup, msaID);
                    //    }

                    //    string sowKey = cbx.Request.QueryString.AllKeys.FirstOrDefault(x => x != null && x.StartsWith("js_" + DatabaseObjects.Columns.VendorSOWLookup));
                    //    if (!string.IsNullOrEmpty(sowKey) && sowKey.Trim() != string.Empty && !cbx.Page.IsPostBack)
                    //    {
                    //        selectedSOWID = cbx.Request.QueryString[sowKey];
                    //    }

                    //}
                    //else if (lookupList.Title == DatabaseObjects.Lists.VendorMSA)
                    //{
                    //    string sowKey = cbx.Request.QueryString.AllKeys.FirstOrDefault(x => x != null && x.StartsWith("js_" + DatabaseObjects.Columns.VendorMSALookup));
                    //    if (!string.IsNullOrEmpty(sowKey) && sowKey.Trim() != string.Empty && !cbx.Page.IsPostBack)
                    //    {
                    //        selectedSOWID = cbx.Request.QueryString[sowKey];
                    //    }
                    //}
                    if (dr.Length > 0)
                        coll = dr.CopyToDataTable();
                }

                //Add empty data for choice none
                if (coll != null && coll.Rows.Count > 0)
                {

                    DataRow r = coll.NewRow();
                    if (cbx.GridView.GroupCount > 0)
                    {
                        r[cbx.GridView.GetGroupedColumns()[0].FieldName] = "(None)";
                    }
                    coll.Rows.InsertAt(r, 0);
                }

                cbx.DataSource = coll;
                cbx.DataBind();
                cbx.Value = selectedSOWID;
                cbx.GridView.Selection.SelectRowByKey(selectedSOWID);


            }
        }
        public void RequestTypeCallBackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataRow modulerow = UGITUtility.ObjectToData(moduleObj).Rows[0];
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            DataTable dtRequestType = moduleViewManager.LoadModuleListByName(Convert.ToString(modulerow[DatabaseObjects.Columns.ModuleName]), DatabaseObjects.Tables.RequestType);
            DataRow[] drRequestType = dtRequestType.Select();
            ASPxCallbackPanel panel = (ASPxCallbackPanel)sender;
            //UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "PRS");
            UGITModule moduleRow = moduleViewManager.GetByName(panel.Attributes["ModuleName"]);
            if (moduleRow == null)
                return;

            string[] paramsData = e.Parameter.Split(new string[] { ";#;" }, StringSplitOptions.None);
            string category = string.Empty;
            string subCategory = string.Empty;

            if (paramsData.Length > 0)
                category = paramsData[0];
            if (paramsData.Length > 1)
                subCategory = paramsData[1];

            foreach (Control ctr in panel.Controls)
            {
                if (ctr is DropDownList)
                {
                    DropDownList requestTypeCtr = (DropDownList)ctr;
                    DropDownList tempdd = uHelper.GetRequestTypesWithCategoriesDropDownOnChange(context, UGITUtility.ObjectToData(moduleRow).Rows[0], category, false, false, subCategory, drRequestType);

                    requestTypeCtr.Items.Clear();
                    foreach (ListItem item in tempdd.Items)
                    {
                        requestTypeCtr.Items.Add(item);
                    }
                }
            }
        }
        private void userSpecificAssets_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)sender;
            DataTable table = null;
            comboBox.Items.Clear();
            UserProfile user = UserManager.GetUserByUserName(Uri.UnescapeDataString(e.Parameter.Trim()));
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule cmdbModule = moduleViewManager.GetByName("CMDB");
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            DataTable result = ticketManager.GetOpenTickets(cmdbModule);
            if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.AssetOwner))
            {
                if (user != null)
                {
                    var xbase = result.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Name);
                    if (xbase.Count() > 0)
                    {
                        result = xbase.CopyToDataTable();
                    }
                    else
                    {
                        result = null;
                    }
                }

            }
            table = result;

            comboBox.ValueField = DatabaseObjects.Columns.Id;
            comboBox.ValueType = typeof(int);
            comboBox.TextField = DatabaseObjects.Columns.AssetTagNum;
            comboBox.DataSource = table;
            comboBox.DataBind();
        }
        public Control GetControlWithHelpAttachmentIcons(Control baseControls, ListItem selectedRequestType)
        {
            Table tableWithControls = new Table();

            string helpText = string.Empty;
            bool isAttachment = false;
            int requestTypeID = 0;
            if (selectedRequestType != null)
            {
                helpText = Convert.ToString(selectedRequestType.Attributes["description"]);
                isAttachment = UGITUtility.StringToBoolean(selectedRequestType.Attributes["isAttachment"]);
                int.TryParse(selectedRequestType.Value, out requestTypeID);
            }

            TableRow containerRow = new TableRow();
            TableCell containerCell = new TableCell();
            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlGenericControl baseControlSpan = new HtmlGenericControl("span");
            HtmlGenericControl helpSpan = new HtmlGenericControl("span");
            HtmlGenericControl attachmentSpan = new HtmlGenericControl("span");


            baseControlSpan.Style.Add("float", "left");
            helpSpan.Style.Add("float", "left");
            attachmentSpan.Style.Add("float", "left");
            attachmentSpan.Style.Add("padding-right", "5px");

            Image helpImage = new Image();
            helpImage.ImageUrl = "~/Content/ButtonImages/comments.png";
            helpImage.ToolTip = helpText;
            helpImage.CssClass = "requestTypeHelp";
            helpImage.Attributes.Add("onclick", "ShowInformation(this)");
            if (helpText == null || helpText.Trim() == string.Empty)
            {
                helpImage.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            Image attachmentImage = new Image();
            attachmentImage.ImageUrl = "~/Content/Images/attach.gif";
            attachmentImage.CssClass = "requestTypeAttachment";
            attachmentImage.Attributes.Add("requesttypeid", requestTypeID.ToString());
            attachmentImage.Attributes.Add("onclick", "ShowAttachments(this)");
            if (!isAttachment)
            {
                attachmentImage.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            helpSpan.Controls.Add(helpImage);
            attachmentSpan.Controls.Add(attachmentImage);

            baseControlSpan.Controls.Add(baseControls);

            div.Controls.Add(baseControlSpan);
            div.Controls.Add(helpSpan);
            div.Controls.Add(attachmentSpan);

            return div;
        }
        private Control CreateBooleanField(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel ctrl = new Panel();
            ctrl.CssClass = "field_" + field.ColumnName.ToLower();

            ASPxCheckBox checkBox = new ASPxCheckBox();
            // checkBox.Text = field.ColumnName;
            checkBox.CssClass = editcss;
            checkBox.ID = GenerateID(field, tabId);

            TextBox batchEditviewControl = new TextBox();
            HiddenField batchHidden = new HiddenField();

            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            if (UGITUtility.IsSPItemExist(item, field.ColumnName) && mode != ControlMode.Display)
            {
                batchHidden.Value = UGITUtility.GetSPItemValueAsString(item, field.ColumnName);
                checkBox.Checked = Convert.ToBoolean(UGITUtility.GetSPItemValue(item, field.ColumnName));
            }
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.Text = varies;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                ctrl.Controls.Add(checkBox);
                ctrl.Controls.Add(GetControlBatchEditButton(field.ColumnName, checkBox, batchEditviewControl, mode, batchHidden.ID));
                return ctrl;
            }

            if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {
                Label cView = new Label();
                cView.Text = UGITUtility.StringToBoolean(item[field.ColumnName]) ? "Yes" : "No";
                checkBox.Checked = UGITUtility.StringToBoolean(item[field.ColumnName]);
                cView.ID = GenerateID(field, tabId) + "_view";
                cView.CssClass = viewcss;
                ctrl.Controls.Add(GetControlWithEditButton(cView.ID, checkBox, cView));
                return ctrl;
            }

            if (mode == ControlMode.Display)
            {
                Label lbl = new Label();
                lbl.Text = UGITUtility.StringToBoolean(item[field.ColumnName]) ? "Yes" : "No";
                //lbl.Text = Convert.ToString(item[field.ColumnName]);
                lbl.ID = GenerateID(field.ColumnName, tabId);
                ctrl.Controls.Add(lbl);
                return ctrl;
            }

            ctrl.Controls.Add(checkBox);
            return ctrl;
        }
        private Control CreateUserFieldControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            UserValueBox userTokenbox = new UserValueBox();
            string value = "";
            if (field.ColumnName == DatabaseObjects.Columns.PRP || field.ColumnName == DatabaseObjects.Columns.TicketORP)
                userTokenbox.UserTokenBoxAdd.ClientSideEvents.TextChanged = "GetComparevalues";
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            bool HideServiceAccountUsers = configurationVariableManager.GetValueAsBool(ConfigConstants.HideServiceAccountUsers);
            userTokenbox.HideServiceAccountUsers = HideServiceAccountUsers;
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                userTokenbox.SetValues(Convert.ToString(sourceItem[field.ColumnName]));
                value = Convert.ToString(sourceItem[field.ColumnName]);

            }
            //Assign requestor  by default(Logged in user) while creating new ticket.
            //else if (mode == ControlMode.New && moduleName.Equals(ModuleNames.TSR) && field.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestor))
            else if (mode == ControlMode.New && field.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestor))
            {
                userTokenbox.SetValues(Convert.ToString(currentUser.Id));
                value = Convert.ToString(currentUser.UserName);
            }
            else
            {
                value = Convert.ToString(item[field.ColumnName]);
                List<string> tempUserlst = UGITUtility.ConvertStringToList(value, Constants.Separator6);
                List<UserProfile> profiles = UserManager.GetUsersProfileWithGroup(context.TenantID, value);
                if (profiles != null && profiles.Count > 0)
                {
                    string values = string.Join(Constants.Separator6, profiles.Select(x => x.Id));
                    userTokenbox.SetValues(values);
                }
            }

            List<UserProfile> users = new List<UserProfile>();
            List<Role> userGroups = new List<Role>();
            string text = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                users = UserManager.GetUserInfosById(Convert.ToString(item[field.ColumnName])).Where(x => x.isRole == false && x.Id != null).ToList();

                userGroups = UserManager.GetUserGroupById(Convert.ToString(item[field.ColumnName]));
                if (userGroups.Count > 0)
                {
                    foreach (var usergroup in userGroups)
                    {
                        users.Add(new UserProfile()
                        {
                            Id = usergroup.Id,
                            Name = usergroup.Title,
                            UserName = usergroup.Name,
                            TenantID = usergroup.TenantID
                        });
                    }
                }
                if (value == "ResetPassword")
                {
                    users.Add(new UserProfile()
                    {
                        Id = "ResetPassword",
                        Name = "ResetPassword",
                        UserName = "ResetPassword",
                        TenantID = "ResetPassword"
                    });

                }

                text = string.Join(Constants.UserInfoSeparator, users.Select(x => x.Name));
                if (users.Count == 0)
                {
                    text = string.Join(Constants.UserInfoSeparator, userGroups.Select(x => x.Name));
                }

                //handle in case if user deleted or still showing guid when do override
                Guid guid;
                if (string.IsNullOrEmpty(text) && Guid.TryParse(value, out guid))
                    userTokenbox.SetValues(string.Empty);
            }

            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel userPanel = new Panel();
            userPanel.ID = GenerateID(field, "Panel");
            userTokenbox.ID = GenerateID(field, tabId);
            userTokenbox.UserTokenBoxAdd.ClientSideEvents.ValueChanged = "UserValueChanged";
            userTokenbox.FieldName = field.ColumnName;
            userTokenbox.UserTokenBoxAdd.CssClass += " " + editcss;

            //code to sset selection set on userfield datasource based on column type from formlayout
            if (moduleObj != null && moduleObj.List_FormLayout != null)
            {
                ModuleFormLayout formLayout = moduleObj.List_FormLayout.FirstOrDefault(x => x.FieldName == field.ColumnName);
                if (formLayout != null)
                    userTokenbox.UserType = formLayout.ColumnType;
            }

            HiddenField batchHidden = new HiddenField();
            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";
            TextBox batchEditviewControl = new TextBox();

            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ReadOnly = true;
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                DataView view = new DataView(list);
                DataTable distinctValues = view.ToTable(true, field.ColumnName);
                if (distinctValues.Rows.Count > 1)
                {
                    batchEditviewControl.Text = varies;
                    userTokenbox.SetValues("");
                }
                else
                {
                    batchEditviewControl.Text = UserManager.NameFromUser(Convert.ToString(item[field.ColumnName]));
                    userTokenbox.SetValues(Convert.ToString(item[field.ColumnName]));
                }
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                userPanel.Controls.Add(batchHidden);
                userPanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, userTokenbox, batchEditviewControl, mode, batchHidden.ID));
                return userPanel;
            }

            else if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketOwner) && (mode == ControlMode.New || mode == ControlMode.Display) && this.moduleObj.OwnerBindingChoice == "Auto" && (moduleName != ModuleNames.CMDB && moduleName != ModuleNames.LEM && moduleName != ModuleNames.CPR && moduleName != ModuleNames.CNS && moduleName != ModuleNames.OPM))
            {
                if (mode == ControlMode.New)
                {
                    Label requestOwner = new Label();
                    requestOwner.ID = GenerateID(field, tabId) + "_view";
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOwner, item.Table)/*.IsSPItemExist(item, DatabaseObjects.Columns.TicketOwner)*/)
                    {
                        requestOwner.Text = "No Owner Assigned";
                        requestOwner.EnableViewState = false;
                        if (!string.IsNullOrWhiteSpace(text))
                            requestOwner.Text = text;
                        requestOwner.CssClass += viewcss + " " + field.ColumnName;
                        userPanel.Controls.Add(requestOwner);
                        return userPanel;
                    }
                }

                var multiUser = CreateOwnerFieldControl(configfield, field, list, item, mode, moduleName, designMode, parentIdPrefix, tabId, sourceItem, batchMode);
                userPanel.Controls.Add(multiUser);
                return userPanel;

            }
            else if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketOwner) && mode == ControlMode.Edit && this.moduleObj.OwnerBindingChoice == "Auto" && (moduleName != "CMDB" && moduleName != ModuleNames.LEM && moduleName != ModuleNames.CPR && moduleName != ModuleNames.CNS && moduleName != ModuleNames.OPM))
            {
                //Label requestOwner = new Label();
                //requestOwner.Text = "No Owner Assigned";
                //requestOwner.ID = GenerateID(field, tabId) + "_view";
                //if (!string.IsNullOrWhiteSpace(text))
                //    requestOwner.Text = text;
                //requestOwner.EnableViewState = false;
                //requestOwner.CssClass += viewcss + " " + field.ColumnName;
                var multiUser = CreateOwnerFieldControl(configfield, field, list, item, mode, moduleName, designMode, parentIdPrefix, tabId, sourceItem, batchMode);
                userPanel.Controls.Add(multiUser);
                return userPanel;
            }

            //Set default value in in businessmanager
            if (field.ColumnName.Contains(DatabaseObjects.Columns.TicketBusinessManager) && mode == ControlMode.New)
            {
                // UGITModule moduleConfig = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, Convert.ToString(uGITCache.GetModuleDetails(moduleId)[DatabaseObjects.Columns.ModuleName]));


                // Get Custom property entry for the BusinessManager Field. 
                // If it has a dependent field add a class with the dependent fields InternalName
                // Business manager field will change using jQuery+Ajax function AjaxHelper.GetBusinessManager if the dependent fields value is changed.
                // Intent is to use as follows:
                //    If custom property set in ModuleUserRoleTypes for business manager, get that user, else get Requestor
                //        and set business manager to that user if manager, else to his manager
                //    Else if loggedin user is manager, set him as manager
                //    Else get loggedin user's manager if set

                ModuleUserType businessManagerUserType = moduleObj.List_ModuleUserTypes.FirstOrDefault(x => x.ColumnName == DatabaseObjects.Columns.TicketBusinessManager);
                UserProfile user = null;
                if (businessManagerUserType != null)
                {
                    string dependentFieldInternalName = DatabaseObjects.Columns.TicketRequestor;
                    if (!string.IsNullOrEmpty(businessManagerUserType.Prop_ManagerOf))
                        dependentFieldInternalName = businessManagerUserType.Prop_ManagerOf;

                    if (UGITUtility.IfColumnExists(dependentFieldInternalName, list) && sourceItem[dependentFieldInternalName] != null)
                    {
                        List<string> fieldUsers = UGITUtility.ConvertStringToList(Convert.ToString(sourceItem[dependentFieldInternalName]), Constants.Separator);    //new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(uHelper.GetSPItemValue(item[ependentFieldInternalName]));
                        if (fieldUsers.Count > 0)
                        {
                            user = UserManager.LoadById(fieldUsers[0]);
                        }
                    }
                }
                else
                {
                    user = UserManager.GetUserByUserName(System.Web.HttpContext.Current.User.Identity.Name);
                }

                if (user != null && (user.IsManager) && user.ManagerID != null)
                {
                    user = UserManager.LoadById(user.ManagerID);
                }

                if (user != null)
                {
                    //Guid outputguid = Guid.NewGuid();
                    //if( Guid.TryParse(user.Id, out outputguid))
                    userTokenbox.SetValues(string.Format("{0}", user.Id)); //string.Format("{0};#{1}", user.ID, user.Name);
                }
            }
            if ((batchMode == BatchMode.Create && field.ColumnName == DatabaseObjects.Columns.TicketRequestor) || (mode == ControlMode.Display || (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)))
            {
                Panel multiUser = new Panel();
                StringBuilder sbUser = new StringBuilder();
                int i = 0;
                if (users != null && users.Count() > 0)
                {
                    foreach (UserProfile usrVal in users)
                    {
                        sbUser.Clear();
                        HyperLink userMailto = new HyperLink();
                        userMailto.CssClass += viewcss + " " + field.ColumnName;
                        userMailto.Attributes.Add("href", "javascript:void(0);");
                        //userMailto.Attributes.Add("onclick", "javascript:mailToUser(this);");
                        userMailto.Attributes.Add("onclick", "javascript:ticketEmail(this);");
                        userMailto.Attributes.Add("ondblclick", "javascript:OpenUserDetails(this);");
                        userMailto.CssClass = userMailto.CssClass + " jqtooltip";
                        userMailto.Style.Add("padding-right", "6px");

                        if (i >= 0 && i < (users.Count - 1) && users.Count > 1)
                            userMailto.Text = $"{usrVal.Name},";
                        else
                            userMailto.Text = usrVal.Name;

                        userMailto.Attributes.Add("UsersID", usrVal.Id);

                        sbUser.Append($"{usrVal.Name} ({usrVal.UserName})");

                        if (!string.IsNullOrEmpty(usrVal.JobProfile))
                            sbUser.Append($"\n{usrVal.JobProfile}");

                        if (!string.IsNullOrEmpty(usrVal.Department))
                            sbUser.Append($"\n{configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.DepartmentLookup, usrVal.Department)}");

                        if (!string.IsNullOrEmpty(usrVal.Location))
                            sbUser.Append($"\nLocation: {configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.LocationLookup, usrVal.Location)}");

                        if (!string.IsNullOrEmpty(usrVal.ManagerID))
                            sbUser.Append($"\nManager: {configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.ManagerID, usrVal.ManagerID)}");

                        userMailto.ToolTip = Convert.ToString(sbUser);
                        //userMailto.ToolTip = string.Format("{0} ({1})", usrVal.Name, usrVal.UserName);
                        multiUser.Controls.Add(userMailto);

                        i++;
                    }
                }

                if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
                {
                    userPanel.Controls.Add(GetControlWithEditButton(field.ColumnName, userTokenbox, multiUser));
                    return userPanel;
                }
                userPanel.Controls.Add(multiUser);
                return userPanel;

            }

            else if (mode != ControlMode.Display && sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName)
                && sourceItem[field.ColumnName] != null)
            {

                if (Convert.ToString(sourceItem[field.ColumnName]) != string.Empty)
                    userTokenbox.SetValues(Convert.ToString(sourceItem[field.ColumnName]));
            }
            else if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                //if (userTokenbox.Tokens.Contains(sourceItem[field.ColumnName]))
                userTokenbox.SetValues(Convert.ToString(sourceItem[field.ColumnName]));
            }

            userPanel.Controls.Add(userTokenbox);
            return userPanel;
        }
        private Control CreateOwnerFieldControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            UserValueBox userTokenbox = new UserValueBox();
            string value = "";
            if (mode == ControlMode.New && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                userTokenbox.SetValues(Convert.ToString(sourceItem[field.ColumnName]));
                value = Convert.ToString(sourceItem[field.ColumnName]);
            }
            else
            {
                userTokenbox.SetValues(Convert.ToString(item[field.ColumnName]));
                value = Convert.ToString(item[field.ColumnName]);
            }
            List<UserProfile> users = new List<UserProfile>();
            List<Role> usersrole = new List<Role>();
            string text = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                users = UserManager.GetUserInfosById(Convert.ToString(item[field.ColumnName]));
                text = string.Join(Constants.UserInfoSeparator, users.Select(x => x.Name));
                if (users.Count == 0)
                {
                    usersrole = UserManager.GetUserGroupById(value);
                }

            }
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel multiUser = new Panel();
            if (users != null && users.Count() > 0)
            {
                foreach (UserProfile usrVal in users)
                {
                    HyperLink userMailto = new HyperLink();
                    userMailto.CssClass += viewcss + " " + field.ColumnName;
                    userMailto.Attributes.Add("href", "javascript:void(0);");
                    //userMailto.Attributes.Add("onclick", "javascript:mailToUser(this);");
                    userMailto.Attributes.Add("onclick", "javascript:ticketEmail(this);");
                    userMailto.Attributes.Add("ondblclick", "javascript:OpenUserDetails(this);");
                    userMailto.CssClass = userMailto.CssClass + " jqtooltip";
                    userMailto.Style.Add("padding-right", "6px");
                    userMailto.Text = usrVal.Name;
                    userMailto.Attributes.Add("UsersID", usrVal.Id);
                    userMailto.ToolTip = string.Format("{0} ({1})", usrVal.Name, usrVal.UserName);
                    multiUser.Controls.Add(userMailto);
                }
            }
            if (usersrole != null && usersrole.Count() > 0)
            {
                foreach (Role usrVal in usersrole)
                {
                    HyperLink userMailto = new HyperLink();
                    userMailto.CssClass += viewcss + " " + field.ColumnName;
                    userMailto.Attributes.Add("href", "javascript:void(0);");
                    //userMailto.Attributes.Add("onclick", "javascript:mailToUser(this);");
                    userMailto.Attributes.Add("onclick", "javascript:ticketEmail(this);");
                    userMailto.Attributes.Add("ondblclick", "javascript:OpenUserDetails(this);");
                    userMailto.CssClass = userMailto.CssClass + " jqtooltip";
                    userMailto.Style.Add("padding-right", "6px");
                    userMailto.Text = usrVal.Name;
                    userMailto.Attributes.Add("UsersID", usrVal.Id);
                    userMailto.ToolTip = string.Format("{0} ({1})", usrVal.Name, usrVal.Title);
                    multiUser.Controls.Add(userMailto);
                }
            }
            return multiUser;
        }
        private Control CreateUrlFieldControl(DataTable dt, DataColumn col, ControlMode mode, ModuleFormLayout coltype)
        {
            ASPxHyperLink hplink = new ASPxHyperLink();
            return hplink;
        }
        private Control CreateUserFieldControl(DataTable dt, DataColumn col, ControlMode mode)
        {
            ASPxTextBox tb = new ASPxTextBox();
            tb.Text = "";
            tb.Width = Unit.Percentage(100);
            tb.Border.BorderColor = System.Drawing.Color.Red;
            Control ctr = tb;
            return ctr;
        }
        private Control CreateNoteFieldControl(FieldConfiguration configfield, DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode) //(SPField field, SPList list, SPListItem item, SPControlMode mode, bool editButton, string tabId, SPListItem sourceItem, int moduleID, bool batchEdit)
        {

            ASPxTextBox tb = new ASPxTextBox();
            tb.Text = "";
            tb.Width = Unit.Percentage(100);
            tb.Border.BorderColor = System.Drawing.Color.Red;
            Control ctr = tb;
            return ctr;
        }
        private Control CreateAttachmentsFieldControl(DataColumn field, DataTable list, DataRow item, ControlMode mode, string moduleName, FieldDesignMode designMode, string parentIdPrefix, string tabId, DataRow sourceItem, BatchMode batchMode)
        {
            string editcss = "field_" + field.ColumnName.ToLower() + "_edit";
            string viewcss = "field_" + field.ColumnName.ToLower() + "_view";
            Panel userPanel = new Panel();
            userPanel.ID = GenerateID(field, "Panel");
            Panel ctrCollection = new Panel();
            Panel pLink = new Panel();
            pLink.CssClass = viewcss;
            pLink.ID = GenerateID(field, tabId) + "_panelLink";
            List<string> files = Convert.ToString(item[field.ColumnName]).Split(',').ToList();
            foreach (string file in files)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    HyperLink userMailto = new HyperLink();
                    userMailto.CssClass = field.ColumnName.ToLower();
                    userMailto.Text = file.Split('\\').Last();
                    userMailto.Target = "_blank";
                    userMailto.ID = field + Guid.NewGuid().ToString().Substring(0, 10).ToString();
                    userMailto.NavigateUrl = "~/content/images/" + file;
                    userMailto.Style.Add("padding-right", "5px");
                    userMailto.ClientIDMode = ClientIDMode.Static;
                    ctrCollection.Controls.Add(userMailto);
                    ctrCollection.Controls.Add(new LiteralControl("<input type='button'  onclick=deleteAttachment('" + Convert.ToString(item[DatabaseObjects.Columns.ID]) + "','" + file + "','" + userMailto.ID + "',this) style='background-image: url(/Content/images/cancel.png); background-repeat: no-repeat; border: none; margin: 0px; padding:0px; width:20px; ' />"));
                    pLink.Controls.Add(userMailto);
                    pLink.Controls.Add(new LiteralControl("<input type='button'  onclick=deleteAttachment('" + Convert.ToString(item[DatabaseObjects.Columns.ID]) + "','" + file + "','" + userMailto.ID + "',this) style='background-image: url(/Content/images/cancel.png); background-repeat: no-repeat; border: none; margin: 0px; padding:0px; width:20px; ' />"));
                }
            }
            Page page = new Page();
            UGITFileUploadManager ufile = (UGITFileUploadManager)page.LoadControl("~/ControlTemplates/Shared/UGITFileUploadManager.ascx");
            ufile.ID = GenerateID(field, tabId);
            //To multiple file select
            ufile.MultiSelect = true;
            ufile.hideWiki = true;
            if (UGITUtility.IsSPItemExist(sourceItem, field.ColumnName))
            {
                ufile.SetImageUrl(Convert.ToString(sourceItem[field.ColumnName]));
            }

            HiddenField batchHidden = new HiddenField();

            batchHidden.ID = GenerateID(field, tabId) + "_batchEditHidden";
            batchHidden.Value = "True";

            TextBox batchEditviewControl = new TextBox();
            if (batchMode == BatchMode.Edit)
            {
                batchEditviewControl.ID = GenerateID(field, tabId) + "batchedit";
                //batchEditviewControl.Width = Unit.Percentage(100);
                batchEditviewControl.Text = varies;
                batchEditviewControl.BackColor = System.Drawing.Color.Transparent;
                batchEditviewControl.ReadOnly = true;
                batchEditviewControl.EnableViewState = true;
                batchEditviewControl.CssClass += "batcheditcustomcontrol " + field.ColumnName + " " + editcss;
                batchEditviewControl.Attributes.Add("Style", "border:none;");
                ufile.SetImageUrl("");
                userPanel.Controls.Add(batchHidden);
                userPanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, page, batchEditviewControl, mode, batchHidden.ID));
                return userPanel;
            }

            if (mode == ControlMode.Display)
            {
                if (batchMode == BatchMode.Edit)
                {
                    userPanel.Controls.Add(GetControlBatchEditButton(field.ColumnName, ufile, batchEditviewControl, mode, batchHidden.ID));
                    return userPanel;
                }
                userPanel.Controls.Add(pLink);
                return userPanel;
            }
            else if (mode == ControlMode.Edit && designMode == FieldDesignMode.WithEdit)
            {

                userPanel.Controls.Add(GetControlWithEditButton(field.ColumnName, ufile, pLink));
                return userPanel;
            }

            else if (mode != ControlMode.Display && sourceItem != null && UGITUtility.IsSPItemExist(sourceItem, field.ColumnName) && sourceItem[field.ColumnName] != null)
            {
                if (Convert.ToString(sourceItem[field.ColumnName]) != string.Empty)
                    ufile.SetImageUrl(Convert.ToString(sourceItem[field.ColumnName]));
            }
            userPanel.Controls.Add(ufile);
            return userPanel;

        }
        protected void btn_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// find the order of the field in the listFields
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GenerateID(string field, string tabId)
        {
            return field + '_' + tabId;
        }
    }
}

