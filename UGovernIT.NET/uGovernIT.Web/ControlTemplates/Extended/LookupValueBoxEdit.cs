using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:LookupValueBoxEdit runat=server></{0}:LookupValueBoxEdit>")]
    public class LookupValueBoxEdit : UGITControl, INamingContainer
    {
        public object CustomParameters { get; set; }

        FieldConfigurationManager fieldManager;
        FieldConfiguration field;

        public ASPxDropDownEdit dropBox { get; set; }
        public string ModuleName { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        //public string value { get; set; }
        public string controlType = "";

        public ASPxGridView gridView;
        public ASPxListBox listBox;
        public List<CustomCollumn> listCollumns { get; set; }

        public bool isRequestType { get; set; }
        public bool IsMulti { get; set; }
        public bool AllowNull { get; set; }
        public bool AllowVaries { get; set; }

        protected const string Varies = "<Value Varies>";

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        
        public DataTable data;
       
        ASPxCallbackPanel panel;
        ASPxHiddenField hdnmodulename;
        public string JsCallbackEvent { get; set; }

        public LookupValueBoxEdit()
        {
            // ModuleName = ModuleName;
            /// panel used to send callback on another control click
            panel = new ASPxCallbackPanel();
            panel.ID = "BoxEditCallBack";
            panel.CssClass = "nprDropDown";
            panel.Callback += pnlRequestTypeCustom_Callback;
            panel.ClientInstanceName = "pnlRequestTypeCustom";
            //panel.Style.Add("min-width", "200px");
            //panel.Style.Add("max-width", "300px");
            /// hidden field to store module name on callback cycle added to callback panel
            hdnmodulename = new ASPxHiddenField();
            hdnmodulename.ID = "hdnmodulename";
            hdnmodulename.ClientInstanceName = "hdnmodulename";
            IsMulti = false;
            FieldName = "";
            dropBox = new ASPxDropDownEdit();
            gridView = new ASPxGridView();
            gridView.ID = "gridview";
            dropBox.ID = "customdropdownedit";
            dropBox.CssClass = dropBox.CssClass +" form-group department";
            gridView.DataBinding += DevexListBox_DataBinding;
            gridView.SettingsBehavior.AllowSelectByRowClick = true;
            // gridView.Width = Unit.Percentage(100);
            gridView.KeyFieldName = DatabaseObjects.Columns.ID;
            gridView.SettingsPager.PageSize = 15;
            gridView.SettingsBehavior.EnableRowHotTrack = true;
            dropBox.Style.Add("min-width", "200px");
            dropBox.Style.Add("max-width", "300px");
            //dropBox.Width = Unit.Pixel(650);
            //Value = "";
            AllowNull = false;
            AllowVaries = false;
            panel.Controls.Add(dropBox);
            panel.Controls.Add(hdnmodulename);
            Controls.Add(panel);
        }

        private void pnlRequestTypeCustom_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Contains("ValueChanged"))
            {
                List<string> values = UGITUtility.ConvertStringToList(e.Parameter, Constants.Separator2);
                //ASPxHiddenField hdnModule = this.FindControlRecursive("hdnmodulename") as ASPxHiddenField;

                if (values.Count == 1)
                    values.Add(string.Empty);

                if (string.IsNullOrEmpty(values[1]) || values[1] == "null")
                    values[1] = "0";

                if (values.Count > 1)
                    ModuleName = uHelper.getModuleNameByModuleId(context, Convert.ToInt32(values[1]));
                IAspxDropdownEditWindowTemplateBase template = this.dropBox.DropDownWindowTemplate as IAspxDropdownEditWindowTemplateBase;
                template.DisplayName = DisplayName;
                template.ModuleName = ModuleName;
                hdnmodulename.Set("ModuleName", ModuleName);
                template.CustomParameters = CustomParameters;
                template.IsMulti = IsMulti;
                template.Value = Value;
                template.DropDownEdit = dropBox;
                dropBox.DropDownWindowTemplate = template;
                //SetValues(string.Empty);
                //SetText(string.Empty);
            }

        }

        private void DropBox_Load(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

        }

        protected override void OnLoad(EventArgs e)
        {
            //SetValues(Value);
            //base.OnLoad(e);
        }

        private void DevexListBox_DataBinding(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxGridView gridview = (DevExpress.Web.ASPxGridView)sender;
            if (data == null)
                data = (DataTable)gridview.DataSource;
            else
            {
                gridView.DataSource = data;
                gridView.AutoGenerateColumns = false;
            }

            DataTable dt = data;

            if (dt!=null)
            {
                //if (fieldManager == null)
                //    fieldManager = new FieldConfigurationManager(context);

                //DataTable dt = fieldManager.GetFieldDataByFieldName(FieldName, ModuleName, "IsDeleted = False");
                string query = HttpContext.Current.Session[gridview.ID] as string;
                if (!string.IsNullOrEmpty(query))
                {
                    DataRow[] dr = dt.Select(query);
                    if (dr.Count() > 0)
                    {
                        dt = dr.CopyToDataTable();
                    }
                    else
                    {
                        dt.Rows.Clear();
                    }
                }
                if (AllowNull == true)
                {
                    DataRow newRow = dt.NewRow();
                    newRow[0] = "0";
                    newRow[1] = "None";
                    dt.Rows.InsertAt(newRow, 0);
                }
                if (dt.Rows.Count > 1)
                {

                    gridView.Settings.ShowColumnHeaders = true;
                }
                //data = dt;

                if (dt.Columns.Count <= 2)
                {
                    dropBox.DisplayFormatString = "{0}";
                }
                gridView.AutoGenerateColumns = false;
                gridView.DataSource = dt;
            }

        }

        protected override void OnInit(System.EventArgs e)
        {
            if (isRequestType)
                controlType = "GridView";
            if (FieldName != null)
            {
                fieldManager = new FieldConfigurationManager(context);
                field = fieldManager.GetFieldByFieldName(FieldName);
                if (data == null &&  ModuleName != ModuleNames.CMDB)
                {
                    if (!string.IsNullOrEmpty(ModuleName) && ModuleName == ModuleNames.CMDB)
                    {
                        data = (DataTable)CacheHelper<object>.Get($"OpenTicket_{ModuleName}", context.TenantID);
                        if (data == null)
                        {
                            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                            keyValuePairs.Add(DatabaseObjects.Columns.TenantID, context.TenantID);
                            data = GetTableDataManager.GetData(DatabaseObjects.Tables.Assets, keyValuePairs);
                        }
                    }
                    else
                        data = fieldManager.GetFieldDataByFieldName(FieldName, ModuleName, "Deleted = False");
                }
                    
                if (field != null && field.Multi)
                {
                    IsMulti = field.Multi;
                    if (!string.IsNullOrWhiteSpace(field.Width) && UGITUtility.StringToUnit(field.Width).Value > 0)
                        this.dropBox.Width = UGITUtility.StringToUnit(field.Width);
                }


                if (field != null && !string.IsNullOrWhiteSpace(field.TemplateType))
                {
                    Type t = typeof(IAspxDropdownEditWindowTemplateBase);
                    try
                    {

                        Type templateType = Type.GetType(t.Namespace + "." + field.TemplateType);
                        var template = Activator.CreateInstance(templateType);
                        if (template != null)
                        {
                            IAspxDropdownEditWindowTemplateBase tpl = template as IAspxDropdownEditWindowTemplateBase;
                            tpl.DisplayName = DisplayName;
                            if (hdnmodulename.Contains("ModuleName"))
                                tpl.ModuleName = Convert.ToString(hdnmodulename.Get("ModuleName"));
                            else
                                tpl.ModuleName = ModuleName;
                            tpl.CustomParameters = CustomParameters;
                            tpl.IsMulti = IsMulti;
                            if (!string.IsNullOrEmpty(Value))
                                tpl.Value = Value;
                            else
                                tpl.Value = DefaultValue;
                            tpl.DropDownEdit = dropBox;
                            tpl.JsCallBackEvent = JsCallbackEvent;
                            dropBox.DropDownWindowTemplate = tpl;

                            // dropBox.AutoPostBack = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                        //loggin
                    }
                }

                if (dropBox.DropDownWindowTemplate == null)
                    dropBox.DropDownWindowTemplate = new MyTemplate(gridView);
            }
            
            if (controlType == "ListBox")
            {
                // listBox.ID = "listBoxDropDownEdit";
                if (IsMulti)
                    listBox.SelectionMode = ListEditSelectionMode.CheckColumn;
                listBox.ValueField = DatabaseObjects.Columns.ID;
                if (data.Columns.Contains(DatabaseObjects.Columns.Title))
                    listBox.TextField = DatabaseObjects.Columns.Title;
                if (data.Columns.Contains(DatabaseObjects.Columns.Name))
                    listBox.TextField = DatabaseObjects.Columns.Name;
                listBox.DataSource = data;

            }
            else
            {

                if (isRequestType)
                {
                    gridView.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Category });
                    gridView.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.SubCategory });
                    gridView.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Title });

                }
                else
                {
                    if (IsMulti)
                    {
                        GridViewCommandColumn dataTextColumn = new GridViewCommandColumn();
                        dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        dataTextColumn.Caption = " ";
                        dataTextColumn.ShowSelectCheckbox = true;
                        gridView.Columns.Add(dataTextColumn);
                    }

                    if (listCollumns != null)
                    {
                        foreach (CustomCollumn x in listCollumns)
                        {
                            string fieldName = x.FieldName;
                            if (data != null)
                            {
                                if ((fieldName.EndsWith("lookup", StringComparison.OrdinalIgnoreCase) || fieldName.EndsWith("user", StringComparison.OrdinalIgnoreCase)) && data.Columns.Contains(string.Format("{0}$", x.FieldName)))
                                    fieldName = string.Format("{0}$", x.FieldName);
                                if (data.Columns.Contains(fieldName))
                                {
                                    gridView.Columns.Add(new GridViewDataColumn() { FieldName = fieldName, Caption = x.DisplayName });
                                }
                            }
                            else
                                gridView.Columns.Add(new GridViewDataColumn() { FieldName = fieldName, Caption = x.DisplayName });
                        }

                    }
                    else
                        gridView.Columns.Add(new GridViewDataColumn() { FieldName = DatabaseObjects.Columns.Title });

                }

                if (!string.IsNullOrEmpty(Convert.ToString(dropBox.KeyValue)))
                {
                    gridView.Selection.SelectRowByKey(dropBox.KeyValue);
                }
            }

            SetValues(Value);
            if (string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(DefaultValue))
                SetValues(DefaultValue);

            base.OnInit(e);

        }

        private void GridView_Init(object sender, EventArgs e)
        {

        }

        private void DevexListBox_Init(object sender, EventArgs e)
        {
            gridView.DataBind();
        }

        public string GetText()
        {
            string value = "";
            value = Convert.ToString(dropBox.Text.Trim());
            if (IsMulti)
            {
                List<string> valuelist = UGITUtility.ConvertStringToList(dropBox.Text, ",");
                valuelist = valuelist.Select(x => x.Trim()).ToList();
                value = string.Join(",", valuelist);
            }
            return value;
        }

        public string GetValues()
        {
            IAspxDropdownEditWindowTemplateBase template = this.dropBox.DropDownWindowTemplate as IAspxDropdownEditWindowTemplateBase;

            string value = "";
            if (IsMulti)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(dropBox.Value)))
                {
                    if (template != null)
                    {
                        value = UGITUtility.ObjectToString(this.dropBox.KeyValue);
                    }
                    else
                    {
                        value = string.Join(",", this.gridView.GetSelectedFieldValues(gridView.KeyFieldName).ToList());
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dropBox.Value)))
                {
                    if (template != null)
                        value = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(this.dropBox.KeyValue)) 
                            ? UGITUtility.ObjectToString(this.dropBox.KeyValue) 
                            : !string.IsNullOrWhiteSpace(template.Value) && Int64.TryParse(template.Value, out long result) ? UGITUtility.ObjectToString(result) 
                            : "";
                    else
                        value = string.Join(",", this.gridView.GetSelectedFieldValues(gridView.KeyFieldName).ToList());
                }
            }
            return value;
        }

        public List<string> GetValuesAsList()
        {
            List<string> values = new List<string>();
            if (IsMulti)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dropBox.Value)))
                {
                    foreach (string s in this.gridView.GetSelectedFieldValues(this.gridView.KeyFieldName).ToList())
                    {
                        values.Add(s);
                    }
                }
            }
            else
            {
                string value = Convert.ToString(this.gridView.GetSelectedFieldValues(gridView.KeyFieldName).ToString());
                if (!string.IsNullOrWhiteSpace(value))
                    values.Add(value);
            }
            return values;
        }

        public List<string> GetTextsAsList()
        {
            List<string> values = UGITUtility.ConvertStringToList(dropBox.Text, ", ");
            return values;
        }

        public void SetValues(string value)
        {
            IAspxDropdownEditWindowTemplateBase template = this.dropBox.DropDownWindowTemplate as IAspxDropdownEditWindowTemplateBase;
            if (gridView.DataSource == null)
                gridView.DataBind();

            if (IsMulti)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (template != null)
                    {
                        template.SetValues(value);
                    }
                    else
                    {
                        string[] keyList = value.Split(',');
                        foreach (string key in keyList)
                        {
                            this.gridView.Selection.SetSelectionByKey(key, true);
                        }
                    }
                }
            }
            else
            {
                if (template != null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        //dropBox.Value = value;        // commented lines to prevent showing department Id & value in dropdown.
                        dropBox.KeyValue = value;       // Uncommented this line, as opening user profile is not setting dept value, while saving record. 
                        template.SetValues(value);
                    }

                }
                else
                {
                    //devexListBox.Value = value;
                    this.gridView.Selection.SetSelectionByKey(value, true);
                }
                //RequestTypeDropDownTemplate control = this.dropBox.DropDownWindowTemplate as RequestTypeDropDownTemplate;
                //if (control != null)
                //    control.SetValues(value);


            }

        }

        public void SetText(string value)
        {
            if (IsMulti)
            {
                dropBox.Text = value;
            }
            else if (AllowVaries)
            {
                dropBox.NullText = Varies;
                dropBox.NullTextStyle.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                dropBox.Text = value;

            }

        }

        public override void DataBind()
        {
            data = null;
            dropBox.DataBind();
            base.DataBind();
        }
    }
    class MyTemplate : ITemplate
    {
        //static int itemcount;
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
    //class RequestTypeTemplate : ITemplate
    //{
    //    RequestTypeCustom ctrl = null;
    //    public string DisplayName { get; set; }
    //    public RequestTypeTemplate()
    //    {
    //    }
    //    public void InstantiateIn(System.Web.UI.Control container)
    //    {
    //        ctrl = (RequestTypeCustom)container.Page.LoadControl("~/ControlTemplates/Shared/RequestTypeCustom.ascx");
    //        ctrl.DisplayName = DisplayName;
    //        container.Controls.Add(ctrl);
    //    }
    //    public string GetValues()
    //    {
    //        return ctrl.GetValues();
    //    }
    //    public void SetValues(string value)
    //    {
    //        ctrl.SetValues(value);
    //    }
    //}
}