using System;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class ITGBudgetTimeSheet : UserControl
    {
        public int nprId { get; set; }
        private  int BudgetSubCategory { get; set; }
        private int Year { get; set; }
        public int BudgetSubCategory1 { get; set; }
        public int Year1 { get; set; }
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsReadOnly;
        public string FrameId;

        protected DateTime fiscalYearStartDate; 
        protected DateTime fiscalYearEndDate; 

        private static string[] types = { "Budget", "# of Staff", "# of On-Site Consultants", "# of Off-Site Consultants" };
        protected List<ModuleBudget> budgets;
        public ApplicationContext AppContext;
        protected void Page_Load(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            //Gets the company fiscal year start and end date.
            fiscalYearStartDate = uHelper.GetFiscalStartDateFromConfig(AppContext);
            fiscalYearEndDate = Convert.ToDateTime(fiscalYearStartDate).AddYears(1).Subtract(new TimeSpan(1, 0, 0, 0));
           
            
            if (currentYearHidden.Value != null && currentYearHidden.Value != string.Empty)
            {
                Year = Year1 = int.Parse(currentYearHidden.Value);
            }
            if (!string.IsNullOrEmpty(currentStartDate.Value))
            {
                StartDate = DateTime.Parse(currentStartDate.Value.Split(',')[0]);
            }
            if (!string.IsNullOrEmpty(currentEndDate.Value))
            {
                EndDate = DateTime.Parse(currentEndDate.Value.Split(',')[0]);
            }
            if (subCategoryIdHidden.Value.Replace(",", "").Trim() != string.Empty)
            {
                BudgetSubCategory = int.Parse(subCategoryIdHidden.Value.Replace(",", "").Trim());
            }
            BindProjectPlan();
        }

        public void BindProjectPlan()
        {
            if (BudgetSubCategory1 > 0)
            {
                subCategoryIdHidden.Value = BudgetSubCategory1.ToString();
                BudgetSubCategory = BudgetSubCategory1;
                Year = Year1;
                currentYearHidden.Value = Year1.ToString();
                BudgetSubCategory1 = 0;
                Year1 = 0;
            }

            DataTable projectPlan = new DataTable();
            projectPlan.Columns.Add("NprId", typeof(string));
            projectPlan.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            projectPlan.Columns.Add(DatabaseObjects.Columns.Category, typeof(string));
            projectPlan.Columns.Add("Total", typeof(string));
            projectPlan.Columns.Add("BudgetDate", typeof(string));

            System.Collections.Generic.Dictionary<string,string> monthMap = new System.Collections.Generic.Dictionary<string,string>();

            DateTime tempReportStartDate = DateTime.Parse(StartDate.Month + "/" + "1" + "/" + StartDate.Year);
            DateTime tempReportEndDate = DateTime.Parse(EndDate.Month + "/" + "1"+ "/" + EndDate.Year);
            int totalMonths = 0;

            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                monthMap.Add(tempReportStartDate.ToString("MMM") + tempReportStartDate.ToString("yy"), "Month" + totalMonths.ToString());
                projectPlan.Columns.Add("Month"+totalMonths, typeof(string));
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }

            DataRow row = projectPlan.NewRow();
            row["NprId"] = "";
            row["Category"] = "";
            row["Title"] = "";
            row["Total"] = "Total";
            tempReportStartDate = DateTime.Parse(StartDate.Month + "/" + "1" + "/" + StartDate.Year);
            totalMonths = 0;
            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                row["Month" + totalMonths.ToString()] = tempReportStartDate.ToString("MMM") +" '" + tempReportStartDate.ToString("yy");
                row["BudgetDate"] += tempReportStartDate.ToShortDateString() + "#";
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }
            projectPlan.Rows.Add(row);

            int counter = 0;
            {
                foreach (string type in types)
                {
                    row = projectPlan.NewRow();
                    row["NprId"] = nprId;
                    row["Category"] = counter;
                    row["Title"] = type;
                    row["BudgetDate"] = projectPlan.Rows[0]["BudgetDate"];
                    for (int i = 1; i <= totalMonths; i++)
                    {
                        row["Month" + i] = "0";
                    }
                   
                    ModuleMonthlyBudgetManager moduleMonthlyBudgetManagerObj = new ModuleMonthlyBudgetManager(AppContext);
                    List<ModuleMonthlyBudget> monthlyBudget = moduleMonthlyBudgetManagerObj.Load(x=>x.BudgetCategoryLookup == BudgetSubCategory && x.BudgetType ==UGITUtility.ObjectToString(counter)); 

                    foreach (ModuleMonthlyBudget monthBudget in monthlyBudget)
                    {
                        DateTime date = (DateTime)monthBudget.AllocationStartDate;
                        if (date >= StartDate && date <= EndDate)
                        {
                            row[monthMap[date.ToString("MMM") + date.ToString("yy")]] = monthBudget.BudgetAmount;
                        }
                    }
                    counter++;
                    projectPlan.Rows.Add(row);
                }
            }

            projectPlanSheet.DataSource = projectPlan;
            projectPlanSheet.DataBind();
        }

        protected void projectPlanSheet_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            projectPlanSheet.EditIndex = e.NewEditIndex;
            BindProjectPlan();
        }
        
        private float CalculateDailyBudgetAmount(DateTime startDate, DateTime endDate, float budgetAmount)
        {
            return (budgetAmount / endDate.Subtract(startDate).Days);
        }

        protected void projectPlanSheet_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            projectPlanSheet.EditIndex = -1;
            BindProjectPlan();
        }
        protected void projectPlanSheet_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = projectPlanSheet.Items[e.ItemIndex];
            //ModuleBudget itgBudgets = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget);
            //string category = ((ImageButton)item.FindControl("btnUpdate")).CommandArgument;
            //string [] budgetDates = ((HiddenField)item.FindControl("budgetDates")).Value.Split('#');
            //for (int i = 1; i <= 12; i++)
            //{
            //    try
            //    {
            //        float monthBudget = float.Parse(((TextBox)item.FindControl("txtMonth" + i)).Text);
            //        if (monthBudget != 0)
            //        {
            //            SPQuery query = new SPQuery();
            //            query.Query = string.Format(@"<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'  /><Value Type='Lookup'>{1}</Value></Eq>" +
            //                "<Eq><FieldRef Name='{2}' /><Value IncludeTimeValue='FALSE' Type='DateTime'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.BudgetLookup, BudgetSubCategory, 
            //                DatabaseObjects.Columns.AllocationStartDate, Year + "-" + i + "-01");
            //            SPListItemCollection filteredBudgets = itgBudgets.GetItems(query);
            //            if (filteredBudgets.Count > 0)
            //            {
            //                SPListItem budget = filteredBudgets[0];
            //                budget[DatabaseObjects.Columns.BudgetAmount] = monthBudget;

            //                budget.Update();
            //            }
            //            else
            //            {
            //                SPListItem newBudget = itgBudgets.Items.Add();
            //                DateTime monthStartDate = DateTime.Parse(budgetDates[i-1]);
            //                newBudget[DatabaseObjects.Columns.AllocationStartDate] = monthStartDate;
            //                newBudget[DatabaseObjects.Columns.BudgetAmount] = monthBudget;
            //                newBudget[DatabaseObjects.Columns.BudgetLookup] = BudgetSubCategory;
            //                newBudget[DatabaseObjects.Columns.BudgetType] = category;
            //                newBudget.Update();
            //            }
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}
            //projectPlanSheet.EditIndex = -1;
            BindProjectPlan();
        }
        
    }
}
