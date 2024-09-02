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

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
	public partial class CustomBudgetControl : System.Web.UI.UserControl
	{
		public int SubCategoryID { get; set; }
		public bool IsProject { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		protected string departmentLevel;

        ApplicationContext context = null;
        DataTable dt = new DataTable();
		protected void Page_Load(object sender, EventArgs e)
		{
            context = HttpContext.Current.GetManagerContext();
			//departmentLevel = uHelper.GetDepartmentLabelName(context, DepartmentLevel.Department);
            LoadProjectBudgets();

        }
        private void LoadProjectBudgets()
        {

            if (IsProject == true)
            {
                pnlProjectBudget.Visible = true;
                pnlNonProjectBudget.Visible = false;
            }
            else
            {
                pnlProjectBudget.Visible = false;
                pnlNonProjectBudget.Visible = true;

            }
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            if (IsProject == true)
                values.Add("@ModuleNameLookup", "");
            else
                values.Add("@ModuleNameLookup", ModuleNames.ITG);
            values.Add("@BudgetCategoryLookup", SubCategoryID);
            values.Add("@AllocationStartDate", Convert.ToDateTime(StartDate));
            values.Add("@AllocationEndDate", Convert.ToDateTime(EndDate));
            dt =  GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudget, values);
            if (IsProject == true)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvProjectBudget.DataSource = dt;
                    gvProjectBudget.DataBind();

                    DataTable dtDistinctModules = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup);
                    if (dtDistinctModules.Rows.Count > 1) //Group the data by Module only when data has more than one module.
                    {
                        gvProjectBudget.SettingsBehavior.AllowGroup = true;
                        ((DevExpress.Web.GridViewDataColumn)gvProjectBudget.Columns[0]).GroupIndex = 0;
                        ((DevExpress.Web.GridViewDataColumn)gvProjectBudget.Columns[0]).Visible = true;
                    }
                    else
                    {
                        gvProjectBudget.SettingsBehavior.AllowGroup = false;
                        ((DevExpress.Web.GridViewDataColumn)gvProjectBudget.Columns[0]).GroupIndex = -1;
                        ((DevExpress.Web.GridViewDataColumn)gvProjectBudget.Columns[0]).Visible = false;
                    }
                }
                else
                    lblProjectMessage.Visible = true;
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvNonProjectBudget.DataSource = dt;
                    gvNonProjectBudget.DataBind();
                }
                else
                    lblNonProjectMessage.Visible = true;
            }
        }
        
    }
}