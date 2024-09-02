using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class QueryExpression : UserControl
    {
        public long DashboardID { get; set; }
        Dashboard uDashboard = null;
        DashboardQuery panel;
        DashboardManager objDashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        QueryHelperManager objQueryHelperManager = new QueryHelperManager(HttpContext.Current.GetManagerContext());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DashboardID > 0)
            {
                uDashboard = objDashboardManager.LoadPanelById(DashboardID, false);
                panel = (DashboardQuery)uDashboard.panel;

                if (!IsPostBack)
                    BindData();
            }
        }

        private void BindData()
        {
            lnkDelete.Visible = false;

            List<Utility.ColumnInfo> columns = SelectColumns(panel.QueryInfo.Tables);
            if (columns != null)
            {
                foreach (Utility.ColumnInfo c in columns)
                {
                    ddlFirstColumn.Items.Add(new ListItem(c.TableName + "." + c.FieldName + "(" + c.DataType + ")"));
                    ddlSecondColumn.Items.Add(new ListItem(c.TableName + "." + c.FieldName + "(" + c.DataType + ")"));
                }
            }

            BindExpressionColumns(panel.QueryInfo.Tables);
        }

        private void BindExpressionColumns(List<TableInfo> tables)
        {
            cbExpName.Items.Clear();
            List<Utility.ColumnInfo> colInfo = new List<Utility.ColumnInfo>();
            if (tables == null)
                colInfo = null;
            else
            {
                foreach (TableInfo table in tables)
                {
                    colInfo.AddRange(table.Columns.Where(x => x.IsExpression).OrderBy(c => c.Sequence).ToArray());
                }

                if (colInfo != null && colInfo.Count() > 0)
                {
                    cbExpName.DataSource = colInfo;
                    cbExpName.TextField = "FieldName";
                    cbExpName.ValueField = "Expression";
                    cbExpName.DataBind();
                }
                else
                {
                    cbExpName.DataSource = null;
                    cbExpName.DataBind();
                }
            }
        }

        private List<Utility.ColumnInfo> SelectColumns(List<TableInfo> tables)
        {
            int Id = 0;
            List<FactTableField> columnData = new List<FactTableField>();
            List<Utility.ColumnInfo> columns = new List<Utility.ColumnInfo>();
            if (tables == null) return columns;

            foreach (TableInfo table in tables)
            {
                columnData = GetColsListWithDataType(table.Name);
                if (columnData == null)
                    continue;

                columnData = columnData.OrderBy(cd => cd.FieldName).ToList();

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

            //changing order of 'TableName' & Sequence to solve grouping issue on columns tab.
            columns = columns.OrderByDescending(c => c.Selected)
                             .ThenBy(c => c.TableName)
                             .ThenBy(c => c.Sequence)
                             .ToList();
            return columns;
        }

        private List<FactTableField> GetColsListWithDataType(string queryTable)
        {
            return objQueryHelperManager.GetColsListWithDataType(queryTable, string.Empty);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Utility.ColumnInfo cInfo = new Utility.ColumnInfo();
            cInfo.IsExpression = true;
            cInfo.FieldName = cbExpName.Text;
            cInfo.DisplayName = txtExpDisplayName.Text;

            if (ddlOperator.SelectedItem.Text == "Month Year" || ddlOperator.SelectedItem.Text == "Year Month" || ddlOperator.SelectedItem.Text == "Year" || ddlOperator.SelectedItem.Text == "Month")
            {
                cInfo.Expression = string.Format("{0}-{1}", ddlOperator.SelectedItem.Text, ddlFirstColumn.SelectedValue);
            }
            else
                cInfo.Expression = string.Format("{0} {1} {2}", ddlFirstColumn.SelectedValue, ddlOperator.SelectedValue, ddlSecondColumn.SelectedValue);

            //cInfo.Expression = string.Format("{0} {1} {2}", ddlFirstColumn.SelectedValue, ddlOperator.SelectedValue, ddlSecondColumn.SelectedValue);

            cInfo.Selected = true;
            cInfo.DataType = ddlDataType.SelectedValue;
            cInfo.TableName = panel.QueryInfo.Tables[0].Name;

            List<Utility.ColumnInfo> cols = panel.QueryInfo.Tables[0].Columns.Where(x => x.FieldName.Equals(cbExpName.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            if (cols != null && cols.Count() > 0)
            {
                foreach (var item in cols)
                {
                    panel.QueryInfo.Tables[0].Columns.Remove(item);
                }
            }
            if (panel.QueryInfo.Tables[0].Columns.Count > 1)
                cInfo.Sequence = panel.QueryInfo.Tables[0].Columns.Count - 1;

            panel.QueryInfo.Tables[0].Columns.Add(cInfo);
            uDashboard.panel = panel;
            Context.Cache.Add(string.Format("QueryTabColumn-{0}", context.CurrentUser.Id), "", null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            
            //update dashboard
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            objDashboardManager.SaveDashboardPanel(iconContents,fileName,false,uDashboard);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);  
        }

        protected void cbExpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbExpName.Text))
            {
                List<Utility.ColumnInfo> columns = new List<Utility.ColumnInfo>();
                foreach (TableInfo table in panel.QueryInfo.Tables)
                {
                    columns.AddRange(table.Columns.Where(x => x.IsExpression).OrderBy(c => c.Sequence).ToArray());
                }

                if (columns != null && columns.Count() > 0)
                {
                    lnkDelete.Visible = true;

                    Utility.ColumnInfo expCol = columns.Where(x => x.FieldName.Equals(cbExpName.Text, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (expCol != null)
                    {
                        ExpressionInfo expInfo = objQueryHelperManager.GetExpressionInfo(expCol.Expression);

                        txtExpDisplayName.Text = expCol.DisplayName;
                        ddlDataType.SelectedIndex = ddlDataType.Items.IndexOf(ddlDataType.Items.FindByText(expCol.DataType));

                        ddlFirstColumn.SelectedIndex = ddlFirstColumn.Items.IndexOf(ddlFirstColumn.Items.FindByText(expInfo.Table1 + "." + expInfo.Column1 + "(" + expInfo.DataType1 + ")"));
                        ddlOperator.SelectedIndex = ddlOperator.Items.IndexOf(ddlOperator.Items.FindByText(expInfo.Operator));
                        ddlSecondColumn.SelectedIndex = ddlSecondColumn.Items.IndexOf(ddlSecondColumn.Items.FindByText(expInfo.Table2 + "." + expInfo.Column2 + "(" + expInfo.DataType2 + ")"));

                        if (expInfo.Operator == "Month Year" || expInfo.Operator == "Year Month" || expInfo.Operator == "Year" || expInfo.Operator == "Month")
                        {
                            trSecondColumn.Style.Add("visibility", "hidden");
                        }
                        else if (expInfo.Operator != "Substring")
                        {
                            ddlSecondColumn.SelectedIndex = ddlSecondColumn.Items.IndexOf(ddlSecondColumn.Items.FindByText(expInfo.Table2 + "." + expInfo.Column2 + "(" + expInfo.DataType2 + ")"));
                            trSecondColumn.Style.Add("visibility", "visible");
                        }
                        else
                        {
                            trSecondColumn.Style.Add("visibility", "hidden");
                        }
                    }
                    else
                    {
                        lnkDelete.Visible = false;
                        txtExpDisplayName.Text = "";
                        ddlDataType.SelectedIndex = 0;
                        ddlFirstColumn.SelectedIndex = 0;
                        ddlOperator.SelectedIndex = 0;
                        ddlSecondColumn.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                txtExpDisplayName.Text = "";
                ddlDataType.SelectedIndex = 0;
                ddlFirstColumn.SelectedIndex = 0;
                ddlOperator.SelectedIndex = 0;
                ddlSecondColumn.SelectedIndex = 0;
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            List<Utility.ColumnInfo> cols = panel.QueryInfo.Tables[0].Columns.Where(x => x.FieldName.Equals(cbExpName.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            if (cols != null && cols.Count() > 0)
            {
                foreach (var item in cols)
                {
                    panel.QueryInfo.Tables[0].Columns.Remove(item);
                }
                
                uDashboard.panel = panel;
                Context.Cache.Add(string.Format("QueryTabColumn-{0}", context.CurrentUser.Id), "", null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                
                //update dashboard
                byte[] iconContents = new byte[0];
                string fileName = string.Empty;
                objDashboardManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
    }
}
