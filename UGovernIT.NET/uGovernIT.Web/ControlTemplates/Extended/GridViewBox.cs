using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:GridViewBox runat=server></{0}:GridViewBox>")]
    public class GridViewBox : ASPxGridView
    {
        //public string CategoryName { get; set;}
        private DataRow[] moduleColumns;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public GridViewBox()
        {
            this.Theme = DevExpress.Web.ASPxGridView.GlobalTheme;
        }
        protected override void OnInit(EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                this.LoadModuleColumns(CategoryName);
            }
            this.HtmlDataCellPrepared += GridViewBox_HtmlDataCellPrepared;
            this.BeforeHeaderFilterFillItems += grid_HeaderFilterFillItems;
            base.OnInit(e);

        }
        private void grid_HeaderFilterFillItems(object sender, ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
            string fieldName = e.Column.Name;
            //DataRow[] drs = moduleMonitorsTable != null ? moduleMonitorsTable.Where(x => x.Field<string>(DatabaseObjects.Columns.ProjectMonitorName) == fieldName).ToArray() : null;
            //if ((drs != null && drs.Length == 0) || ModuleName != "PMM")
            //{
            //    FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
            //    FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));

            //    e.Values.Insert(0, fvNonBlanks);
            //    e.Values.Insert(0, fvBlanks);
            //}
            List<KeyValuePair<string, string>> nameCollection = new List<KeyValuePair<string, string>>();
            foreach (FilterValue fValue in e.Values)
            {
                if (fValue.ToString() != "(All)" && fValue.ToString() != "(Blanks)" && fValue.ToString() != "(Non Blanks)")
                {
                    string values = Convert.ToString(fValue);
                    LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(context, e.Column.FieldName, values, true);
                    if (lookUPValueCollection != null && lookUPValueCollection.Count > 0)
                    {
                        foreach (LookupValue lookUpValue in lookUPValueCollection)
                        {
                            nameCollection.Add(new KeyValuePair<string, string>(lookUpValue.Value, lookUpValue.ID));

                        }
                    }
                    else
                    {
                        nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                    }
                }
                else
                {
                    nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                }
            }

            e.Values.Clear();
            foreach (KeyValuePair<string, string> s in nameCollection)
            {
                FilterValue v = new FilterValue(s.Key, s.Value);
                e.Values.Add(v);
            }
        }
        private void GridViewBox_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.DataItemTemplate != null)
                return;
            try
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                FieldConfigurationManager fmanger = new FieldConfigurationManager(context);
                if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
                {
                    string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                    e.Cell.Text = value;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
        public void LoadModuleColumns(string CatName)
        {
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context);
            moduleColumns = UGITUtility.ToDataTable<ModuleColumn>(moduleColumnManager.GetModuleColumns()).Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, CatName));
            _GenerateColumns();

        }
        public void _GenerateColumns()
        {
            foreach (DataRow moduleColumn in moduleColumns)
            {
                string fieldColumn = Convert.ToString(moduleColumn["FieldName"]);
                string fieldDisplayName = Convert.ToString(moduleColumn["FieldDisplayName"]);
                GridViewDataTextColumn colId = null;
                GridViewDataColumn grdViewDataColumn = null;
                if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetItem.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.Caption = fieldDisplayName;
                    grdViewDataColumn.FieldName = fieldColumn;

                    this.Columns.Add(grdViewDataColumn);
                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetAmount.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
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
                    colId.PropertiesTextEdit.DisplayFormatString = "MMM-d-yyyy";
                    //this.Columns.Add(grdViewDataColumn);
                    this.Columns.Add(colId);

                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.AllocationEndDate.ToLower())
                {
                    //  grdViewDataColumn = new GridViewDataColumn();
                    colId = new GridViewDataTextColumn();
                    colId.FieldName = fieldColumn;
                    colId.Caption = fieldDisplayName;
                    colId.PropertiesTextEdit.DisplayFormatString = "MMM-d-yyyy";
                    // this.Columns.Add(grdViewDataColumn);
                    this.Columns.Add(colId);

                }
                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.BudgetDescription.ToLower())
                {
                    grdViewDataColumn = new GridViewDataColumn();
                    grdViewDataColumn.FieldName = fieldColumn;
                    grdViewDataColumn.Caption = fieldDisplayName;
                    this.Columns.Add(grdViewDataColumn);
                }
                else
                {
                    colId = new GridViewDataTextColumn();
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.PropertiesEdit.ClientInstanceName = Convert.ToString(moduleColumn["FieldName"]);
                    colId.FieldName = Convert.ToString(moduleColumn["FieldName"]);
                    colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                    this.Columns.Add(colId);
                }
            }
            // GridViewDataTextColumn dataTextColumn = new GridViewDataTextColumn();
            // dataTextColumn.DataItemTemplate = new MyTemplate();
            // dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            // dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            // dataTextColumn.Caption = " ";
            // dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            // dataTextColumn.HeaderStyle.Font.Bold = true;
            // dataTextColumn.Width = new Unit("55px");
            //// dataTextColumn.VisibleIndex = 7;
            // this.Columns.Add(dataTextColumn);
        }
        //class MyTemplate : ITemplate
        //{

        //    public void InstantiateIn(Control container)
        //    {
        //        ASPxImage linkEdit = new ASPxImage();
        //        linkEdit.ID = "imgEdit";
        //        linkEdit.ImageUrl = "/content/images/edit-icon.png";
        //        linkEdit.ToolTip = "Edit";
        //        linkEdit.Style.Value = "float:right;";
        //        GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
        //        container.Controls.Add(linkEdit);
        //        ASPxButton btDelete = new ASPxButton();
        //        btDelete.ID = "imgDelete";
        //        btDelete.RenderMode = ButtonRenderMode.Link;
        //        btDelete.ImageUrl = "/content/images/delete-icon.png";
        //        btDelete.ToolTip = "Delete";
        //        btDelete.Style.Value = "float:right;";
        //        btDelete.TabIndex = 1;
        //        btDelete.ClientSideEvents.Click = "deletedBudget";
        //        btDelete.AutoPostBack = true;
        //        btDelete.CommandArgument = Convert.ToString(gridContainer.KeyValue);
        //        btDelete.CommandName = "Delete";
        //        btDelete.Click += Bt_Click;
        //        container.Controls.Add(btDelete);
        //    }
        //    public void Bt_Click(object sender, EventArgs e)
        //    {
        //        ASPxButton deletebutton = (ASPxButton)sender;
        //        int Id = Convert.ToInt32(deletebutton.CommandArgument);
        //        ModuleBudget objModuleBudget = new ModuleBudget();
        //        objModuleBudget.ID = Id;
        //        bool Deleted = ModuleBudgetManager.Delete(objModuleBudget);
        //       // ModuleBudgetList.Checklabel = false;
        //        //if (Deleted)
        //        //{
        //        //    ModuleBudgetList.Checklabel = true;
        //        //}
        //    }
        //}
    }
}

