using DevExpress.UnitConversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class CustomActualControl : System.Web.UI.UserControl
    {
        public int SubCategoryID { get; set; }
        public bool IsProject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        ApplicationContext context = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsProject == true)
                LoadProjectActuals();
            else
                LoadNonProjectActuals();
        }

        // Date Condition:  (ActualStart <= FiscalEnd) AND (ActualEnd >= FiscalStart)
        private void LoadProjectActuals()
        {
            pnlProjectActual.Visible = true;
            pnlNonProjectActual.Visible = false;
            context = HttpContext.Current.GetManagerContext();


            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            values.Add("@ModuleNameLookup", "PMM");
            DataTable pmmBudget = GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudget, values);
            DataTable pmmActuals = GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudgetActuals, values);
            string budgetQuery = string.Format("{0}={1}", DatabaseObjects.Columns.BudgetCategoryLookup, SubCategoryID);
            DataRow[] budgetItems = pmmBudget.Select(budgetQuery);
            DataTable actualTable = null;
            foreach (DataRow budgetItem in budgetItems)
            {
                int budgetItemId = Convert.ToInt32(budgetItem[DatabaseObjects.Columns.ID]);
                string actualQuery = string.Format("{0}={1} and {2}<='{5}' and {3}>='{4}'", DatabaseObjects.Columns.ModuleBudgetLookup, budgetItemId, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                DataRow[] actualItems = pmmActuals.Select(actualQuery);
                if (actualItems.Count() > 0)
                {
                    if (actualTable == null)
                        actualTable = actualItems.CopyToDataTable();
                    else
                        actualTable.Merge(actualItems.CopyToDataTable());
                }
            }

            if (actualTable == null)
                actualTable = new DataTable();
            if (actualTable != null)
            {
                gvProjectActual.DataSource = actualTable;
                gvProjectActual.DataBind();
                DataTable dtDistinctModules = actualTable.DefaultView.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup);
                if (dtDistinctModules.Rows.Count > 1) //Group the data by Module only when data has more than one module.
                {
                    gvProjectActual.SettingsBehavior.AllowGroup = true;
                    ((DevExpress.Web.GridViewDataColumn)gvProjectActual.Columns[0]).GroupIndex = 0;
                    ((DevExpress.Web.GridViewDataColumn)gvProjectActual.Columns[0]).Visible = true;
                }
                else
                {
                    gvProjectActual.SettingsBehavior.AllowGroup = false;
                    ((DevExpress.Web.GridViewDataColumn)gvProjectActual.Columns[0]).GroupIndex = -1;
                    ((DevExpress.Web.GridViewDataColumn)gvProjectActual.Columns[0]).Visible = false;
                }

            }
        }
        private void LoadNonProjectActuals()
        {
            pnlProjectActual.Visible = false;
            pnlNonProjectActual.Visible = true;
            DataTable itgBudget = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudget, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");
            DataTable itgActuals = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetActuals, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");

            string budgetQuery = string.Format("{0}={1}", DatabaseObjects.Columns.BudgetCategory, SubCategoryID);
            DataRow[] budgetItems = itgBudget.Select(budgetQuery);

            DataTable actualTable = null;
            foreach (DataRow budgetItem in budgetItems)
            {
                int budgetItemId = Convert.ToInt32(budgetItem[DatabaseObjects.Columns.Id]);

                string actualQuery = string.Format("{0}={1} and {2}<={3} and {4}>={5}", DatabaseObjects.Columns.ModuleBudgetLookup, budgetItemId, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                DataRow[] actualItems = itgActuals.Select(actualQuery);

                if (actualItems.Count() > 0)
                {
                    if (actualTable == null)
                        actualTable = actualItems.CopyToDataTable();
                    else
                        actualTable.Merge(actualItems.CopyToDataTable());
                }
            }

            if (actualTable == null)
                actualTable = new DataTable();
        }

    }
}