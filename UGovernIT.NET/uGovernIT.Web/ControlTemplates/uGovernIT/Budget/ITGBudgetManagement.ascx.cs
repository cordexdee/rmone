using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{

    public partial class ITGBudgetManagement : UserControl
    {
        protected int currentYear;
        public int ticketId { get; set; }
        public static DateTime startDate;
        public static DateTime endDate;
        public static DateTime fiscalStartDate;
        public static DateTime fiscalEndDate;
        public bool IsReadOnly;
        public string FrameId;
        private bool isProjectPlanBindinDone;
        private static string[] types = { "Budget", "Staff", "On-Site Consultants", "Off-Site Consultants" };

        private string calendarViewType = "1"; // Default to Calendar Year (Fiscal Year = 2)
        protected DateTime currentYearStartDate = new DateTime(DateTime.Today.Year, 1, 1);
        protected DateTime currentYearEndDate = new DateTime(DateTime.Today.Year, 12, 31);
        protected string importExcelPagePath;
        protected string exportListPagePath;
        List<string> selectedSubCategoriesA = new List<string>();
        List<string> selectedSubCategoriesTextA = new List<string>();
        protected bool IsReadOnlyBudget;
        private DataTable budgets;
        protected bool enableBudgetCategoryType;

        string lastCategoryType = "-1";
        string lastCategory = string.Empty;
        DataTable authorizedCategories;
        public ApplicationContext AppContext;
        public BudgetCategoryViewManager BudgetCategoryManagerObj = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        public ModuleBudgetManager ModuleBudgetManagerObj = new ModuleBudgetManager(HttpContext.Current.GetManagerContext());
        public ModuleMonthlyBudgetManager BudgetMonthlyManagerObj = new ModuleMonthlyBudgetManager(HttpContext.Current.GetManagerContext());
        public ConfigurationVariableManager ConfigVarManagerObj = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        protected void Page_Init(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            List<BudgetCategory> budgetCategories = BudgetCategoryManagerObj.Load(x => x.AuthorizedToView.Contains(AppContext.CurrentUser.Id));

            //SPQuery currentUsersQuery = new SPQuery();
            //currentUsersQuery.Query = string.Format(@"<Where><Or><Or><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='User'>{1}</Value></Eq>
            //                              <Membership Type='CurrentUserGroups'><FieldRef Name='{0}' /></Membership></Or>
            //                              <IsNull><FieldRef Name='{0}' /></IsNull></Or></Where>", DatabaseObjects.Columns.AuthorizedToView, SPContext.Current.Web.CurrentUser.ID);
            //SPListItemCollection budgetCategoriesColl = budgetCategories.GetItems(currentUsersQuery);
            if (budgetCategories.Count == 0)
                budgetCategories = BudgetCategoryManagerObj.Load();
            authorizedCategories = UGITUtility.ToDataTable<BudgetCategory>(budgetCategories);

            if (authorizedCategories != null)
            {
                authorizedCategories.Columns.Add(DatabaseObjects.Columns.BudgetCategoryLookup, typeof(string),
                    string.Format("{0}+'{1}'+{2}", DatabaseObjects.Columns.Id, Constants.Separator, DatabaseObjects.Columns.BudgetSubCategory));
            }


            enableBudgetCategoryType = ConfigVarManagerObj.GetValueAsBool(ConfigurationVariable.EnableBudgetCategoryType);
            pEnableCategoryType.Visible = false;
            if (enableBudgetCategoryType)
            {
                pEnableCategoryType.Visible = true;
            }

            currentYear = DateTime.Now.Year;

            fiscalStartDate = uHelper.GetFiscalStartDateFromConfig(AppContext);

            fiscalEndDate = fiscalStartDate.AddYears(1).Subtract(new TimeSpan(24, 0, 0));


            if (Request["calendarViewType"] != null)
            {
                calendarViewType = Convert.ToString(Request["calendarViewType"]);
            }

            if (Request["calendarYear"] != null)
            {
                int.TryParse(Request["calendarYear"], out currentYear);
            }

            if (currentYear <= 0)
            {
                currentYear = DateTime.Now.Year;
            }

            ChangeStartEndDates(calendarViewType);

            BindNPRBudgetList();


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            currentYearHidden.Value = currentYear.ToString();
            yearType.Attributes.Add("onchange", "changeCalendarViewType(this)");
            ChangeStartEndDates(calendarViewType);
            yearType.SelectedIndex = yearType.Items.IndexOf(yearType.Items.FindByValue(calendarViewType));

            importExcelPagePath = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=importexcel&sourcePage=ITGBudgetManagement");
            exportListPagePath = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=exportlist&sourcePage=ITGBudgetManagement");

            bool enableImportExport = ConfigVarManagerObj.GetValueAsBool("EnableITGBudgetImportExport");
            btImport.Visible = enableImportExport;
            btExport.Visible = enableImportExport;


            foreach (RepeaterItem rItem in rBudgetInfo.Items)
            {
                if (rItem.ItemType == ListItemType.Item || rItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cbSubCategory = (CheckBox)rItem.FindControl("cbSubCategory");
                    HiddenField hfSubCategoryID = (HiddenField)rItem.FindControl("hfSubCategoryID");
                    Label lbSubCategory = (Label)rItem.FindControl("lbSubCategory");
                    if (hfSubCategoryID != null && hfSubCategoryID.Value != string.Empty)
                    {
                        if (cbSubCategory.Checked)
                        {
                            selectedSubCategoriesA.Add(hfSubCategoryID.Value.Trim());
                            selectedSubCategoriesTextA.Add(lbSubCategory.Text.Trim());
                        }
                    }
                }
            }

            IsReadOnlyBudget = true;
            if (selectedSubCategoriesA.Count == 1)
            {
                IsReadOnlyBudget = false;
            }
            currentSubCategoryNameHidden.Value = string.Join(",", selectedSubCategoriesA.ToArray());
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (!isProjectPlanBindinDone)
            {
                BindProjectPlan();
            }
            base.OnPreRender(e);
        }
        protected void ChangeStartEndDates(string changeType)
        {
            switch (changeType)
            {
                case "2":
                    fiscalStartDate = new DateTime(currentYear - 1, fiscalStartDate.Month, 1);
                    fiscalEndDate = fiscalStartDate.AddYears(1).Subtract(new TimeSpan(24, 0, 0));

                    // Adjust fiscal year date if we are already into next fiscal year
                    //if (fiscalEndDate.Year < DateTime.Now.Year)
                    //{
                    //    fiscalStartDate = fiscalStartDate.AddYears(1);
                    //    fiscalEndDate = fiscalEndDate.AddYears(1);
                    //    currentYear += 1;
                    //}

                    startDate = fiscalStartDate;
                    currentStartDate.Value = startDate.ToShortDateString();
                    endDate = fiscalEndDate;
                    currentEndDate.Value = endDate.ToShortDateString();
                    break;
                default:
                    startDate = DateTime.Parse("1/1/" + currentYear);
                    currentStartDate.Value = startDate.ToShortDateString();
                    endDate = DateTime.Parse("12/31/" + currentYear);
                    currentEndDate.Value = endDate.ToShortDateString();
                    break;
            }
        }
        protected void btExport_Click(object sender, EventArgs e)
        {
            //String listName = DatabaseObjects.Lists.ITGMonthlyBudget;
            //SPWeb web = SPContext.Current.Web;
            //SPList list = ExportListToExcel.GetListFromSPServer(listName);

            //if (list != null)
            //{
            //    //This can be used later in case we need all column filters in excel sheet.
            //    //Guid listGuid = list.ID;
            //    //Guid listViewGuid = list.Views[0].ID;
            //    //Response.Redirect(string.Format("{0}/_vti_bin/owssvr.dll?CS=109&Using=_layouts/query.iqy&List={1}&View={2}&CacheControl=1", web.Site.Url, listGuid, listViewGuid));
            //    DataTable table = ExportListToExcel.GetDataTableFromSPList(list);
            //    ExportListToExcel.ExportToSpreadsheet(table, listName);
            //}
        }

        #region Task Sheet Methods
        public void BindProjectPlan()
        {

            DataTable nonProjectBudgetDistn = new DataTable();
            DataTable projectBudgetDistn = new DataTable();

            nonProjectBudgetDistn.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            nonProjectBudgetDistn.Columns.Add("Total", typeof(string));
            projectBudgetDistn.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            projectBudgetDistn.Columns.Add("Total", typeof(string));


            System.Collections.Generic.Dictionary<string, string> monthMap = new System.Collections.Generic.Dictionary<string, string>();
            DateTime tempReportStartDate = DateTime.Parse(startDate.Month + "/" + "1" + "/" + startDate.Year);
            DateTime tempReportEndDate = DateTime.Parse(endDate.Month + "/" + "1" + "/" + endDate.Year);
            int totalMonths = 0;

            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                monthMap.Add(tempReportStartDate.ToString("MMM") + tempReportStartDate.ToString("yy"), "Month" + totalMonths.ToString());
                nonProjectBudgetDistn.Columns.Add("Month" + totalMonths, typeof(string));
                projectBudgetDistn.Columns.Add("Month" + totalMonths, typeof(string));
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }

            //Create first row which contains header for list view
            DataRow nrow = nonProjectBudgetDistn.NewRow();
            DataRow prow = projectBudgetDistn.NewRow();
            nrow["Title"] = prow["Title"] = "";
            nrow["Total"] = prow["Total"] = "Total";

            tempReportStartDate = DateTime.Parse(startDate.Month + "/" + "1" + "/" + startDate.Year);
            totalMonths = 0;
            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                nrow["Month" + totalMonths.ToString()] = tempReportStartDate.ToString("MMM") + " '" + tempReportStartDate.ToString("yy");
                prow["Month" + totalMonths.ToString()] = tempReportStartDate.ToString("MMM") + " '" + tempReportStartDate.ToString("yy");
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }
            nonProjectBudgetDistn.Rows.Add(nrow);
            projectBudgetDistn.Rows.Add(prow);

            //Load distribution data from ITGMonthlyBudget list
            List<string> queryExps = new List<string>();
            //SPQuery rQueryWithCategoryFilter = new SPQuery();
            if (selectedSubCategoriesA.Count > 0)
            {
                List<string> orExpressions = new List<string>();
                foreach (string selectedSubCategory in selectedSubCategoriesA)
                {
                    orExpressions.Add(string.Format("{0}={1}", DatabaseObjects.Columns.BudgetCategoryLookup, selectedSubCategory));
                }

                queryExps.Add(UGITUtility.ConvertListToString(orExpressions, " Or "));
            }
            queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetType, 0));
            // rQueryWithCategoryFilter.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryExps, queryExps.Count - 1, true));

            string sdquery = UGITUtility.ConvertListToString(queryExps, " And ");
            DataTable monthlyBudget = BudgetMonthlyManagerObj.GetDataTable(sdquery);

            //hide data if there is not  authorized category to view
            if (authorizedCategories == null)
            {
                monthlyBudget = null;
            }
            //Show only authorized category data
            if (selectedSubCategoriesA.Count <= 0 && authorizedCategories != null && monthlyBudget != null)
            {
                DataRow[] rows = (from x in monthlyBudget.AsEnumerable()
                                  join y in authorizedCategories.AsEnumerable()
                                      on x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup) equals y.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup)
                                  select x).ToArray();
                if (rows.Length > 0)
                {
                    monthlyBudget = rows.CopyToDataTable();
                }
            }

            //DataTable monthlyBudget = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget).GetItems(rQueryWithCategoryFilter).GetDataTable();


            DataRow nBudgetRow = nonProjectBudgetDistn.NewRow();
            DataRow nActualRow = nonProjectBudgetDistn.NewRow();
            DataRow nVarianceRow = nonProjectBudgetDistn.NewRow();
            DataRow pBudgetRow = projectBudgetDistn.NewRow();
            DataRow pActualRow = projectBudgetDistn.NewRow();
            DataRow pVarianceRow = projectBudgetDistn.NewRow();
            nonProjectBudgetDistn.Rows.Add(nBudgetRow);
            nonProjectBudgetDistn.Rows.Add(nActualRow);
            nonProjectBudgetDistn.Rows.Add(nVarianceRow);
            projectBudgetDistn.Rows.Add(pBudgetRow);
            projectBudgetDistn.Rows.Add(pActualRow);
            projectBudgetDistn.Rows.Add(pVarianceRow);

            nBudgetRow["Title"] = "Planned";
            nActualRow["Title"] = "Actual";
            nVarianceRow["Title"] = "Variance";
            pBudgetRow["Title"] = "Planned";
            pActualRow["Title"] = "Actual";
            pVarianceRow["Title"] = "Variance";

            if (monthlyBudget != null)
            {
                for (int i = 1; i <= totalMonths; i++)
                {
                    nBudgetRow["Month" + i] = "0";
                    nActualRow["Month" + i] = "0";
                    nVarianceRow["Month" + i] = "0";

                    pBudgetRow["Month" + i] = "0";
                    pActualRow["Month" + i] = "0";
                    pVarianceRow["Month" + i] = "0";
                }

                DataRow[] selectTypeRows = monthlyBudget.Select();


                foreach (DataRow monthBudget in selectTypeRows)
                {
                    DateTime date = Convert.ToDateTime(monthBudget[DatabaseObjects.Columns.AllocationStartDate]);


                    if (date >= startDate && date <= endDate)
                    {
                        double plannedBudgets = 0;
                        double actualBudgets = 0;
                        double.TryParse(Convert.ToString(monthBudget[DatabaseObjects.Columns.NonProjectPlannedTotal]), out plannedBudgets);
                        double preBudget = 0;
                        double.TryParse(Convert.ToString(nBudgetRow[monthMap[date.ToString("MMM") + date.ToString("yy")]]), out preBudget);
                        nBudgetRow[monthMap[date.ToString("MMM") + date.ToString("yy")]] = preBudget + plannedBudgets;

                        double.TryParse(Convert.ToString(monthBudget[DatabaseObjects.Columns.NonProjectActualTotal]), out actualBudgets);
                        double.TryParse(Convert.ToString(nActualRow[monthMap[date.ToString("MMM") + date.ToString("yy")]]), out preBudget);
                        nActualRow[monthMap[date.ToString("MMM") + date.ToString("yy")]] = preBudget + actualBudgets;


                        double.TryParse(Convert.ToString(monthBudget[DatabaseObjects.Columns.ProjectPlannedTotal]), out plannedBudgets);
                        double.TryParse(Convert.ToString(pBudgetRow[monthMap[date.ToString("MMM") + date.ToString("yy")]]), out preBudget);
                        pBudgetRow[monthMap[date.ToString("MMM") + date.ToString("yy")]] = preBudget + plannedBudgets;

                        double.TryParse(Convert.ToString(monthBudget[DatabaseObjects.Columns.ProjectActualTotal]), out actualBudgets);
                        double.TryParse(Convert.ToString(pActualRow[monthMap[date.ToString("MMM") + date.ToString("yy")]]), out preBudget);
                        pActualRow[monthMap[date.ToString("MMM") + date.ToString("yy")]] = preBudget + actualBudgets;

                    }
                }


                {
                    double nPlanTotal = 0;
                    double nActualTotal = 0;
                    double nVarianceTotal = 0;
                    double pPlanTotal = 0;
                    double pActualTotal = 0;
                    double pVarianceTotal = 0;

                    double variance = 0;
                    double plannedBudgets = 0;
                    double actualBudgets = 0;
                    for (int i = 1; i <= totalMonths; i++)
                    {
                        double.TryParse(Convert.ToString(nBudgetRow["Month" + i]), out plannedBudgets);
                        double.TryParse(Convert.ToString(nActualRow["Month" + i]), out actualBudgets);

                        nBudgetRow["Month" + i] = string.Format("{0:C}", plannedBudgets);
                        nActualRow["Month" + i] = string.Format("{0:C}", actualBudgets);

                        nPlanTotal += plannedBudgets;
                        nActualTotal += actualBudgets;
                        variance = plannedBudgets - actualBudgets;
                        nVarianceTotal += variance;
                        nVarianceRow["Month" + i] = string.Format("{0:C}", variance);
                        if (variance > 0)
                            nVarianceRow["Month" + i] = string.Format("<b style='font-weight:normal;color:green;'>{0:C}</b>", variance);
                        if (variance < 0)
                            nVarianceRow["Month" + i] = string.Format("<b style='font-weight:normal;color:red;'>{0:C}</b>", variance);


                        double.TryParse(Convert.ToString(pBudgetRow["Month" + i]), out plannedBudgets);
                        double.TryParse(Convert.ToString(pActualRow["Month" + i]), out actualBudgets);
                        pBudgetRow["Month" + i] = string.Format("{0:C}", plannedBudgets);
                        pActualRow["Month" + i] = string.Format("{0:C}", actualBudgets);
                        pPlanTotal += plannedBudgets;
                        pActualTotal += actualBudgets;
                        variance = plannedBudgets - actualBudgets;
                        pVarianceTotal += variance;
                        pVarianceRow["Month" + i] = string.Format("{0:C}", variance);
                        if (variance > 0)
                            pVarianceRow["Month" + i] = string.Format("<b style='font-weight:normal;color:green;'>{0:C}</b>", variance);
                        if (variance < 0)
                            pVarianceRow["Month" + i] = string.Format("<b style='font-weight:normal;color:red;'>{0:C}</b>", variance);
                    }

                    //set total
                    nBudgetRow["Total"] = string.Format("{0:C}", nPlanTotal);
                    nActualRow["Total"] = string.Format("{0:C}", nActualTotal);
                    nVarianceRow["Total"] = string.Format("{0:C}", nVarianceTotal);
                    if (nVarianceTotal > 0)
                        pVarianceRow["Total"] = string.Format("<b style='font-weight:normal;color:green;'>{0:C}</b>", nVarianceTotal);
                    if (nVarianceTotal < 0)
                        pVarianceRow["Total"] = string.Format("<b style='font-weight:normal;color:red;'>{0:C}</b>", nVarianceTotal);

                    pBudgetRow["Total"] = string.Format("{0:C}", pPlanTotal);
                    pActualRow["Total"] = string.Format("{0:C}", pActualTotal);
                    pVarianceRow["Total"] = string.Format("{0:C}", pVarianceTotal);
                    if (pVarianceTotal > 0)
                        pVarianceRow["Total"] = string.Format("<b style='font-weight:normal;color:green;'>{0:C}</b>", pVarianceTotal);
                    if (pVarianceTotal < 0)
                        pVarianceRow["Total"] = string.Format("<b style='font-weight:normal;color:red;'>{0:C}</b>", pVarianceTotal);
                }
            }
            else
            {
                for (int i = 1; i <= totalMonths; i++)
                {
                    nonProjectBudgetDistn.Rows[1]["Month" + i] = string.Format("{0:C}", 0);
                    nonProjectBudgetDistn.Rows[2]["Month" + i] = string.Format("{0:C}", 0);
                    nonProjectBudgetDistn.Rows[3]["Month" + i] = string.Format("{0:C}", 0);

                    projectBudgetDistn.Rows[1]["Month" + i] = string.Format("{0:C}", 0);
                    projectBudgetDistn.Rows[2]["Month" + i] = string.Format("{0:C}", 0);
                    projectBudgetDistn.Rows[3]["Month" + i] = string.Format("{0:C}", 0);
                }

                nonProjectBudgetDistn.Rows[1]["Total"] = string.Format("{0:C}", 0);
                nonProjectBudgetDistn.Rows[2]["Total"] = string.Format("{0:C}", 0);
                nonProjectBudgetDistn.Rows[3]["Total"] = string.Format("{0:C}", 0);
                projectBudgetDistn.Rows[1]["Total"] = string.Format("{0:C}", 0);
                projectBudgetDistn.Rows[2]["Total"] = string.Format("{0:C}", 0);
                projectBudgetDistn.Rows[3]["Total"] = string.Format("{0:C}", 0);

            }

            lvNonProjecdtBudget.DataSource = nonProjectBudgetDistn;
            lvNonProjecdtBudget.DataBind();

            lvProjectBudget.DataSource = projectBudgetDistn;
            lvProjectBudget.DataBind();

            isProjectPlanBindinDone = true;
        }
        private float CalculateDailyBudgetAmount(DateTime startDate, DateTime endDate, float budgetAmount)
        {
            return (budgetAmount / endDate.Subtract(startDate).Days);
        }

        #endregion

        #region Budget categories

        private void BindNPRBudgetList()
        {
            List<DataTable> budgetList = ModuleBudgetManagerObj.LoadBudgetSummary(startDate, endDate);
            budgets = budgetList[0];
            if (budgets != null)
            {
                DataRow[] budgetRows = budgets.Select("", string.Format("{0} asc, {1} asc, {2} asc", DatabaseObjects.Columns.BudgetType, DatabaseObjects.Columns.BudgetCategory, DatabaseObjects.Columns.BudgetSubCategory));
                if (budgetRows != null && budgetRows.Count() > 0)
                    budgets = budgetRows.CopyToDataTable();
            }

            rBudgetInfo.DataSource = budgets;
            rBudgetInfo.DataBind();
        }
        protected void RBudgetInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem rItem = (RepeaterItem)e.Item;
            double grandTotalOfNonProjectVariance = 0;
            double grandTotalOfProjectVariance = 0;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                HtmlTableRow trCategoryType = (HtmlTableRow)rItem.FindControl("trCategoryType");
                if (enableBudgetCategoryType)
                {
                    trCategoryType.Attributes.Add("class", "bitem categoryBackground");
                    trCategoryType.Attributes.Add("level", "1");
                    Label lbCategoryType = (Label)rItem.FindControl("lbCategoryType");
                    if (lastCategoryType.ToLower() != Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]).ToLower())
                    {
                        lastCategoryType = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]);
                        lbCategoryType.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]);
                        if (lbCategoryType.Text.Trim() == string.Empty)
                        {
                            lbCategoryType.Text = "(None)";
                        }
                        trCategoryType.Visible = true;
                        trCategoryType.Attributes.Add("current", string.Format("C-{0}", rowView[DatabaseObjects.Columns.Id]));
                        trCategoryType.Attributes.Add("parent", "0");

                        DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;
                        Label lbCTotalAmount = (Label)trCategoryType.FindControl("lbCTotalAmount");
                        Label lbCTotalActuals = (Label)trCategoryType.FindControl("lbCTotalActuals");
                        Label lbCTotalVariance = (Label)trCategoryType.FindControl("lbCTotalVariance");

                        Label lblProjectPlannedTotal = (Label)trCategoryType.FindControl("lblProjectPlannedTotal");
                        Label lblProjectActualTotal = (Label)trCategoryType.FindControl("lblProjectActualTotal");
                        Label lblProjectVariancelTotal = (Label)trCategoryType.FindControl("lblProjectVariancelTotal");

                        string whereClause = string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetType, Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]));
                        if (string.IsNullOrEmpty(Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType])))
                        {
                            whereClause = string.Format("{0} is null or {0} = ''", DatabaseObjects.Columns.BudgetType);
                        }

                        lbCTotalAmount.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectPlannedTotal), whereClause)).ToString("C");
                        lbCTotalActuals.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectActualTotal), whereClause)).ToString("C");
                        double nonProjectVariance = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectPlannedTotal), whereClause)) - Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectActualTotal), whereClause));
                        lbCTotalVariance.Text = nonProjectVariance.ToString("C");

                        lblProjectPlannedTotal.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectPlannedTotal), whereClause)).ToString("C");
                        lblProjectActualTotal.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectActualTotal), whereClause)).ToString("C");
                        double projectVariance = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectPlannedTotal), whereClause)) - Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectActualTotal), whereClause));
                        lblProjectVariancelTotal.Text = projectVariance.ToString("C");

                    }
                }

                HtmlTableRow trCategory = (HtmlTableRow)rItem.FindControl("trCategory");
                trCategory.Attributes.Add("class", "bitem");
                trCategory.Attributes.Add("current", Convert.ToString(rowView[DatabaseObjects.Columns.BudgetCategory]));
                trCategory.Attributes.Add("budgettype", Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]));
                HtmlTableCell tdCategory = (HtmlTableCell)rItem.FindControl("tdCategory");
                trCategory.Attributes.Add("level", "1");
                if (enableBudgetCategoryType)
                {
                    tdCategory.Style.Add(HtmlTextWriterStyle.PaddingLeft, "20px");
                    trCategory.Attributes.Add("level", "2");
                }
                Label lbCategory = (Label)rItem.FindControl("lbCategory");
                if (lastCategory.ToLower() != Convert.ToString(rowView[DatabaseObjects.Columns.BudgetCategory]).ToLower())
                {
                    lastCategory = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetCategory]);
                    lbCategory.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetCategory]);
                    trCategory.Visible = true;
                    trCategory.Attributes.Add("current", string.Format("C-{0}", rowView[DatabaseObjects.Columns.Id]));
                    trCategory.Attributes.Add("parent", "0");
                    if (enableBudgetCategoryType)
                    {
                        trCategoryType.Attributes.Add("parent", string.Format("CT-{0}", rowView[DatabaseObjects.Columns.Id]));
                    }

                    DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;
                    Label lbSubCTotalAmount = (Label)trCategory.FindControl("lbSubCTotalAmount");
                    Label lbSubCTotalActuals = (Label)trCategory.FindControl("lbSubCTotalActuals");
                    Label lbSubCTotalVariance = (Label)trCategory.FindControl("lbSubCTotalVariance");

                    Label lblSubTotalPlanned = (Label)trCategory.FindControl("lblSubTotalPlanned");
                    Label lblSubTotalActual = (Label)trCategory.FindControl("lblSubTotalActual");
                    Label lblSubTotalVariance = (Label)trCategory.FindControl("lblSubTotalVariance");

                    string whereClause = string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategory, lastCategory);

                    lbSubCTotalAmount.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectPlannedTotal), whereClause)).ToString("C");
                    lbSubCTotalActuals.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectActualTotal), whereClause)).ToString("C");
                    double nonProjectVariance = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectPlannedTotal), whereClause)) - Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectActualTotal), whereClause));
                    lbSubCTotalVariance.Text = nonProjectVariance.ToString("C");

                    lblSubTotalPlanned.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectPlannedTotal), whereClause)).ToString("C");
                    lblSubTotalActual.Text = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectActualTotal), whereClause)).ToString("C");
                    double projectVariance = Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectPlannedTotal), whereClause)) - Convert.ToDouble(budgetTable.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectActualTotal), whereClause));
                    lblSubTotalVariance.Text = projectVariance.ToString("C");
                }

                HtmlTableRow trSubCategory = (HtmlTableRow)rItem.FindControl("trSubCategory");
                trSubCategory.Attributes.Add("class", "bitem subcategory");
                if (e.Item.ItemIndex % 2 != 0)
                {
                    trSubCategory.Attributes.Add("class", "bitem subcategory ms-alternatingstrong");
                }

                trSubCategory.Attributes.Add("level", "2");
                if (enableBudgetCategoryType)
                {
                    trSubCategory.Attributes.Add("level", "3");
                }
                trSubCategory.Visible = true;
                HtmlTableCell tdSubCategory = (HtmlTableCell)rItem.FindControl("tdSubCategory");
                tdSubCategory.Style.Add(HtmlTextWriterStyle.PaddingLeft, "20px");
                if (enableBudgetCategoryType)
                {
                    tdSubCategory.Style.Add(HtmlTextWriterStyle.PaddingLeft, "40px");
                }

                trSubCategory.Attributes.Add("current", Convert.ToString(rowView[DatabaseObjects.Columns.BudgetSubCategory]));
                trSubCategory.Attributes.Add("current", Convert.ToString(rowView[DatabaseObjects.Columns.BudgetSubCategory]));

                trSubCategory.Attributes.Add("current", string.Format("SC-{0}", rowView[DatabaseObjects.Columns.Id]));
                if (enableBudgetCategoryType)
                {
                    trSubCategory.Attributes.Add("parent", string.Format("C-{0}", rowView[DatabaseObjects.Columns.Id]));
                }

                HiddenField hfSubCategoryID = (HiddenField)rItem.FindControl("hfSubCategoryID");
                Label lblCapEx = (Label)rItem.FindControl("lblCapEx");
                Label lbSubCategory = (Label)rItem.FindControl("lbSubCategory");
                Label lbBudgetCOA = (Label)rItem.FindControl("lbBudgetCOA");
                Label lbAmount = (Label)rItem.FindControl("lbAmount");
                Label lbActuals = (Label)rItem.FindControl("lbActuals");
                Label lbVariance = (Label)rItem.FindControl("lbVariance");
                Label lbItemDescription = (Label)rItem.FindControl("lbItemDescription");

                Label lblProjectPlanned = (Label)rItem.FindControl("lblProjectPlanned");
                Label lblProjectActual = (Label)rItem.FindControl("lblProjectActual");
                Label lblProjectVariance = (Label)rItem.FindControl("lblProjectVariance");

                hfSubCategoryID.Value = Convert.ToString(rowView[DatabaseObjects.Columns.Id]);
                lblCapEx.Text = (UGITUtility.StringToBoolean(rowView[DatabaseObjects.Columns.CapitalExpenditure]) ? "C" : "&nbsp;&nbsp;");
                lbSubCategory.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetSubCategory]);
                lbBudgetCOA.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetCOA]);

                lbAmount.Text = Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectPlannedTotal]).ToString("C");
                lbActuals.Text = Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectActualTotal]).ToString("C");
                double nonVariance = Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectPlannedTotal]) - Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectActualTotal]);
                lbVariance.Text = nonVariance.ToString("C");

                lblProjectPlanned.Text = Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectPlannedTotal]).ToString("C");
                lblProjectActual.Text = Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectActualTotal]).ToString("C");
                double pVariance = Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectPlannedTotal]) - Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectActualTotal]);
                lblProjectVariance.Text = pVariance.ToString("C");

                lbItemDescription.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetDescription]);

                // Make link to Project/non-Projects Actuals.
                string parameters = "SubCategoryID=" + Convert.ToString(rowView[DatabaseObjects.Columns.Id]) + "&StartDate=" + startDate + "&EndDate=" + endDate;

                string actualUrlForProject = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=actuals&IsProject=true&" + parameters);
                string actualUrlForNonProject = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=actuals&IsProject=false&" + parameters);
                string title = lbBudgetCOA.Text + ": " + Convert.ToString(rowView[DatabaseObjects.Columns.BudgetSubCategory]);

                if (Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectActualTotal]) != 0)
                {
                    lbActuals.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', '{1}', '80', '80', 0, 0)", actualUrlForNonProject, title));
                    lbActuals.Attributes.Add("class", "clickableLabel");
                }

                if (Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectActualTotal]) != 0)
                {
                    lblProjectActual.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', '{1}', '80', '80', 0, 0)", actualUrlForProject, title));
                    lblProjectActual.Attributes.Add("class", "clickableLabel");
                }

                // Make link to Project/non-Projects Budgets.
                string param = "SubCategoryID=" + Convert.ToString(rowView[DatabaseObjects.Columns.Id]) + "&StartDate=" + startDate + "&EndDate=" + endDate;

                string budgetUrlForProject = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=budgets&IsProject=true&" + parameters);
                string abudgetUrlForNonProject = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=budgets&IsProject=false&" + parameters);

                if (Convert.ToDouble(rowView[DatabaseObjects.Columns.NonProjectPlannedTotal]) != 0)
                {
                    lbAmount.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', '{1}', '80', '80', 0, 0)", abudgetUrlForNonProject, title));
                    lbAmount.Attributes.Add("class", "clickableLabel");
                }

                if (Convert.ToDouble(rowView[DatabaseObjects.Columns.ProjectPlannedTotal]) != 0)
                {
                    lblProjectPlanned.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', '{1}', '80', '80', 0, 0)", budgetUrlForProject, title));
                    lblProjectPlanned.Attributes.Add("class", "clickableLabel");
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DataTable budgetTable1 = (DataTable)rBudgetInfo.DataSource;

                HtmlTableRow trSubTotal = (HtmlTableRow)rItem.FindControl("trTotal");
                trSubTotal.Visible = true;

                Label lbTotalAmount = (Label)rItem.FindControl("lbTotalAmount");
                Label lbTotalActuals = (Label)rItem.FindControl("lbTotalActuals");
                Label lbTotalVariance = (Label)rItem.FindControl("lbTotalVariance");

                Label lblProjectTotal = (Label)rItem.FindControl("lblProjectTotal");
                Label lblProjectActual = (Label)rItem.FindControl("lblProjectActual");
                Label lblProjectVariance = (Label)rItem.FindControl("lblProjectVariance");

                // Caclulate the grand total of Non-Project, Project and Variance.
                if (budgetTable1 != null && budgetTable1.Rows.Count > 0)
                {
                    double gTotalOfNonProjectPlanned = Convert.ToDouble(budgetTable1.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectPlannedTotal), string.Empty));
                    double gTotalOfNonProjectActual = Convert.ToDouble(budgetTable1.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.NonProjectActualTotal), string.Empty));
                    grandTotalOfNonProjectVariance = gTotalOfNonProjectPlanned - gTotalOfNonProjectActual;

                    lbTotalAmount.Text = gTotalOfNonProjectPlanned.ToString("C");
                    lbTotalActuals.Text = gTotalOfNonProjectActual.ToString("C");
                    lbTotalVariance.Text = grandTotalOfNonProjectVariance.ToString("C");

                    double gTotalOfProjectPlanned = Convert.ToDouble(budgetTable1.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectPlannedTotal), string.Empty));
                    double gTotalOfProjectActual = Convert.ToDouble(budgetTable1.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.ProjectActualTotal), string.Empty));
                    grandTotalOfProjectVariance = gTotalOfProjectPlanned - gTotalOfProjectActual;

                    lblProjectTotal.Text = gTotalOfProjectPlanned.ToString("C");
                    lblProjectActual.Text = gTotalOfProjectActual.ToString("C");
                    lblProjectVariance.Text = grandTotalOfProjectVariance.ToString("C");
                }
            }
        }
        protected void RBudgetInfo_PreRender(object sender, EventArgs e)
        {
            DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;
            foreach (RepeaterItem rItem in rBudgetInfo.Items)
            {
                if (rItem.ItemType == ListItemType.Item || rItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cbSubCategory = (CheckBox)rItem.FindControl("cbSubCategory");
                    HiddenField hfSubCategoryID = (HiddenField)rItem.FindControl("hfSubCategoryID");
                    Label lbSubCategory = (Label)rItem.FindControl("lbSubCategory");
                    HtmlTableRow trSubCategory = (HtmlTableRow)rItem.FindControl("trSubCategory");
                    if (cbSubCategory.Checked)
                    {
                        trSubCategory.Attributes.Add("class", trSubCategory.Attributes["class"] + " ms-selectednav");
                    }
                    else
                    {
                        trSubCategory.Attributes.Add("class", trSubCategory.Attributes["class"].Replace(" ms-selectednav", string.Empty));
                    }
                }
            }

            DataRow[] rows = (from x in budgetTable.AsEnumerable()
                              join s in selectedSubCategoriesA on x.Field<int>(DatabaseObjects.Columns.Id).ToString() equals s
                              select x).ToArray();
        }

        #endregion
    }
}
