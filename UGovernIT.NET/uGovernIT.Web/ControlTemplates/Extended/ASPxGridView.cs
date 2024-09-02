using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Data;
using System.Web.UI.WebControls;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:ASPxGridView runat=server></{0}:ASPxGridView>")]
    public class ASPxGridView : DevExpress.Web.ASPxGridView
    {
        public string CategoryName { get; set; }
        public string ExCssClass { get; set; }
        private List<ModuleColumn> moduleColumns;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        FieldConfigurationManager fmanger = null;
        public ASPxGridView()
        {
            this.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            this.HtmlDataCellPrepared += GridViewBox_HtmlDataCellPrepared;
            this.BeforeHeaderFilterFillItems += GridViewBox_HeaderFilterFillItems;
            //this.Theme = DevExpress.Web.ASPxGridView.GlobalTheme;
            this.Images.LoadingPanel.Url = "~/Content/Images/AjaxLoader.gif";
        }
        protected override void OnInit(EventArgs e)
        {
            fmanger = new FieldConfigurationManager(context);

            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                this.LoadColumns(CategoryName);
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            this.CssClass += " customgridview ";
            base.OnPreRender(e);
        }
        private void GridViewBox_HeaderFilterFillItems(object sender, ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
            ////string fieldName = e.Column.FieldName;
            //////e.Column.SettingsHeaderFilter.Mode= GridHeaderFilterMode.CheckedList;
            //////DataRow[] drs = moduleMonitorsTable != null ? moduleMonitorsTable.Where(x => x.Field<string>(DatabaseObjects.Columns.ProjectMonitorName) == fieldName).ToArray() : null;
            //////if ((drs != null && drs.Length == 0) || ModuleName != "PMM")
            //////{
            //////    FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
            //////    FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));

            //////    e.Values.Insert(0, fvNonBlanks);
            //////    e.Values.Insert(0, fvBlanks);
            //////}
            ////List<KeyValuePair<string, string>> nameCollection = new List<KeyValuePair<string, string>>();
            ////foreach (FilterValue fValue in e.Values)
            ////{
            ////    if (fValue.ToString() != "(All)" && fValue.ToString() != "(Blanks)" && fValue.ToString() != "(Non Blanks)")
            ////    {
            ////        string values = Convert.ToString(fValue);
            ////        LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(context,e.Column.FieldName, values, true);
            ////        if (lookUPValueCollection != null && lookUPValueCollection.Count > 0)
            ////        {
            ////            foreach (LookupValue lookUpValue in lookUPValueCollection)
            ////            {
            ////                nameCollection.Add(new KeyValuePair<string, string>(lookUpValue.Value, lookUpValue.ID));

            ////            }
            ////        }
            ////        else
            ////        {
            ////            nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
            ////        }
            ////    }
            ////    else
            ////    {
            ////        nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
            ////    }
            ////}

            ////e.Values.Clear();
            ////foreach (KeyValuePair<string, string> s in nameCollection)
            ////{
            ////    FilterValue v = new FilterValue(s.Key, s.Value);
            ////    e.Values.Add(v);
            ////}
        }
        public void LoadColumns(string categoryName = "")
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                categoryName = this.CategoryName;
            }
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context);
            moduleColumns = moduleColumnManager.Load(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, categoryName)).OrderBy(x => x.FieldSequence).ToList();
            GenerateColumns();
        }
        public void GenerateColumns()
        {
            foreach (ModuleColumn moduleColumn in moduleColumns)
            {
                string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                string fieldDisplayName = Convert.ToString(moduleColumn.FieldDisplayName);
                GridViewDataTextColumn colId = null;
                GridViewDataColumn grdViewDataColumn = null;
                HorizontalAlign alignment = HorizontalAlign.Center;
                if (!string.IsNullOrWhiteSpace(moduleColumn.TextAlignment))
                    alignment = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), moduleColumn.TextAlignment);

                if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetItem.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.SortAscending();
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetAmount.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.UnapprovedAmount.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.Visible = false;
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.AllocationStartDate.ToLower())
                {
                    //grdViewDataColumn = new GridViewDataColumn();
                    colId = new GridViewDataTextColumn();
                    // grdViewDataColumn.FieldName = fieldColumn;
                    // grdViewDataColumn.Caption = fieldDisplayName;
                    colId.FieldName = fieldColumn;
                    colId.Caption = fieldDisplayName;
                    colId.PropertiesTextEdit.DisplayFormatString = string.IsNullOrEmpty(context.ConfigManager.GetValue(ConfigConstants.uGovernITDateFormat))? "MMM-d-yyyy": context.ConfigManager.GetValue(ConfigConstants.uGovernITDateFormat);
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    colId.CellStyle.HorizontalAlign = alignment;
                    //this.Columns.Add(grdViewDataColumn);
                    this.Columns.Add(colId);

                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.AllocationEndDate.ToLower())
                {
                    //  grdViewDataColumn = new GridViewDataColumn();
                    colId = new GridViewDataTextColumn();
                    colId.FieldName = fieldColumn;
                    colId.Caption = fieldDisplayName;
                    colId.PropertiesTextEdit.DisplayFormatString = string.IsNullOrEmpty(context.ConfigManager.GetValue(ConfigConstants.uGovernITDateFormat)) ? "MMM-d-yyyy" : context.ConfigManager.GetValue(ConfigConstants.uGovernITDateFormat);
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    colId.CellStyle.HorizontalAlign = alignment;
                    // this.Columns.Add(grdViewDataColumn);
                    this.Columns.Add(colId);

                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetDescription.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.ModuleBudgetLookup.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.VendorLookup.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);

                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.InvoiceNumber.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.UserSkillLookup.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetType.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns._ResourceType.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketNoOfFTEs.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.RequestedResources.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.RoleName.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.CellStyle.HorizontalAlign = alignment;
                    grdViewDataColumn.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    this.Columns.Add(grdViewDataColumn);
                }
                else
                {
                    colId = new GridViewDataTextColumn();
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.PropertiesEdit.ClientInstanceName = Convert.ToString(moduleColumn.FieldName);
                    colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                    colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    colId.CellStyle.HorizontalAlign = alignment;
                    this.Columns.Add(colId);
                }
            }
        }
        private void GridViewBox_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //if (e.DataColumn.DataItemTemplate != null)
            //    return;

            //if (string.IsNullOrWhiteSpace(e.DataColumn.FieldName))
            //    return;

            //try
            //{
            //    // if (!Page.IsPostBack)
            //    {
            //        string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
            //        fmanger = new FieldConfigurationManager(context);

            //        if (!string.IsNullOrEmpty(values))
            //        {
            //            if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
            //            {
            //                string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
            //                e.Cell.Text = value;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ULog.WriteException(ex);
            //}
        }
    }
}

