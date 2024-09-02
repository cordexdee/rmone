using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class GanttView_Filter : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        public string delegateControl;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configVariableMGR = null;
        protected override void OnInit(EventArgs e)
        {
            configVariableMGR = new ConfigurationVariableManager(context);
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            ModuleName = Request["Module"];
            BindGanttGroupBy();
            BindGridView();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void BindGanttGroupBy()
        {
            string initiativeLevel2Name = string.IsNullOrEmpty(configVariableMGR.GetValue(ConfigConstants.InitiativeLevel2Name)) ? "Business Initiative" : configVariableMGR.GetValue(ConfigConstants.InitiativeLevel2Name);

            ddlGroupBy.Items.Add(new ListItem("--None--", "0"));
            ddlGroupBy.Items.Add(new ListItem("Priority", "1"));
            ddlGroupBy.Items.Add(new ListItem("Project Type", "2"));
            ddlGroupBy.Items.Add(new ListItem(initiativeLevel2Name, "3"));
        }

        private void BindGridView()
        {
            // Fetch all roles of selected module
            
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule pmmModule = moduleManager.GetByName(ModuleName);
            List<ModuleColumn> moduleColumns = pmmModule.List_ModuleColumns;

            if (moduleColumns == null || moduleColumns.Count == 0)
                return;

            List<ModuleColumn> selectedColumns = null;
            if (moduleColumns != null && moduleColumns.Count > 0)
            {
                selectedColumns = moduleColumns.Where(x => x.DisplayForReport == true).ToList();

                if (selectedColumns != null && selectedColumns.Count > 0)
                {
                    grid.DataSource = selectedColumns;
                    grid.DataBind();
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }
            }
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            string moduleName = string.Empty;

            DataRow currentRow = grid.GetDataRow(e.VisibleIndex);
            if (currentRow == null)
                return; // No rows in table, nothing to do!


            if (Convert.ToString(UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.FieldName)) == DatabaseObjects.Columns.Attachments)
                e.Row.Visible = false;

        }

        protected void grid_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                bool selected = UGITUtility.StringToBoolean(Convert.ToString(grid.GetRowValues(i, "DisplayForReport")));
                if (selected)
                    grid.Selection.SelectRow(i);
            }
        }
        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        
        private void SwapDropDownValue(DropDownList ddl1, DropDownList ddl2, int remSum)
        {
            int tempValue = Math.Abs( 15 - remSum - UGITUtility.StringToInt(ddl2.SelectedValue));
            ddl2.SelectedValue = ddl1.SelectedValue;
            ddl1.SelectedValue = tempValue.ToString();
        }
        protected void ddlProjectSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrioritySortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue); ;
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProjectSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectSortOrder, remSum);
            }
        }

        protected void ddlPrioritySortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProgressSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlTargetDateSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlProjectRank.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectRank, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue));
            }
        }

        protected void ddlProjectRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectRank, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectRank, remSum);
            }
        }

        protected void ddlProgressSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProgressSortOrder, remSum);
            }
        }


        protected void ddlTargetDateSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlTargetDateSortOrder, remSum);
            }
        }
    }
}