using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Manager.Helper
{
    public class ITG
    {
        protected ApplicationContext AppContext;
        protected ModuleViewManager ModuleManagerObj;
        public ITG(ApplicationContext Context)
        {
            AppContext = Context;
            ModuleManagerObj = new ModuleViewManager(Context);
        }

        public class Portfolio
        {
            protected string Title { get; set; }
            protected string Priority { get; set; }
            protected string TicketStatus { get; set; }
            protected float BudgetAmount { get; set; }
            protected string Actual { get; set; }
            protected float CostToCompletion { get; set; }
            protected string TicketActualStartDate { get; set; }
            protected string TicketActualCompletionDate { get; set; }
            protected string ProjectStatus { get; set; }
            protected static DateTime yearStartDate = DateTime.Now;
            protected static DateTime yearEndDate = DateTime.Now;

            public static DataTable LoadAll(ApplicationContext _context, List<Constants.ProjectType> projectType)
            {

                ModuleViewManager moduleManager = new ModuleViewManager(_context);
                DataTable result = CreateTable();
                UGITModule nprModule = moduleManager.GetByName("NPR");
                UGITModule pmmModule = moduleManager.GetByName("PMM");

                bool queryNPR = false;
                bool queryPMM = false;

                //Builds query based on type
                List<string> requiredQuery = new List<string>();
                List<string> nprRequestQuery = new List<string>();

                if (projectType.Contains(Constants.ProjectType.All) || projectType.Contains(Constants.ProjectType.CurrentProjects) || projectType.Contains(Constants.ProjectType.CompletedProjects))
                {
                    queryPMM = true;
                }

                if (projectType.Contains(Constants.ProjectType.All) || projectType.Contains(Constants.ProjectType.ApprovedNPRs) || projectType.Contains(Constants.ProjectType.OnHold) || projectType.Contains(Constants.ProjectType.PendingApprovalNPRs))
                {
                    queryNPR = true;
                }

                //Add this query only when pending approval view is not added. if pending approval view is added then we are adding approved view inside it.
                if (projectType.Contains(Constants.ProjectType.ApprovedNPRs))
                {
                    //Fetchs NPRs whose current stage is approved
                    LifeCycleStage approvedStage = nprModule.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.Prop_ReadyToImport.HasValue && x.Prop_ReadyToImport.Value);
                    nprRequestQuery.Add($"{DatabaseObjects.Columns.StageStep}={approvedStage.StageStep}");
                }

                if (projectType.Contains(Constants.ProjectType.PendingApprovalNPRs))
                {
                    //Fetchs NPRs whose current stage is ITGReview and SCReview
                    // First add all NPR steps before Approved stage
                    LifeCycleStage approvedStage = nprModule.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.Prop_ReadyToImport.HasValue && x.Prop_ReadyToImport.Value);
                    List<string> stepExps = new List<string>();
                    List<LifeCycleStage> approvalStages = nprModule.List_LifeCycles.FirstOrDefault().Stages.Where(x => x.StageStep < approvedStage.StageStep).ToList();
                    foreach (LifeCycleStage stage in approvalStages)
                    {
                        stepExps.Add($"{DatabaseObjects.Columns.StageStep} = {stage.StageStep}");
                    }

                    // Then exclude all projects on hold
                    if (!projectType.Contains(Constants.ProjectType.OnHold))
                    {
                        //nprRequestQuery.Add(string.Format("<And>{2}<Neq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Neq></And>",
                        //   DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus, UGITUtility.GenerateWhereQueryWithAndOr(stepExps, stepExps.Count - 1, false)));
                        nprRequestQuery.Add($"{DatabaseObjects.Columns.TicketStatus}='{Constants.OnHoldStatus}' and {UGITUtility.ConvertListToString(stepExps, " Or ")}");
                    }
                    else
                        nprRequestQuery.Add(UGITUtility.ConvertListToString(stepExps, " OR "));
                }

                if (projectType.Contains(Constants.ProjectType.OnHold))
                {
                    //Shows Only onhold tickets
                    //nprRequestQuery.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>",
                    //  DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus));
                    nprRequestQuery.Add($"{DatabaseObjects.Columns.TicketStatus}='{Constants.OnHoldStatus}'");
                }

                string pmmQuery = string.Empty;
                List<string> pmmQueryStr = new List<string>();
                if (requiredQuery.Count > 0)
                {
                    pmmQueryStr.Add(UGITUtility.ConvertListToString(requiredQuery, " And "));
                }

                if (!(projectType.Contains(Constants.ProjectType.CompletedProjects) && projectType.Contains(Constants.ProjectType.CurrentProjects)))
                {
                    if (projectType.Contains(Constants.ProjectType.CompletedProjects) && !projectType.Contains(Constants.ProjectType.CurrentProjects))
                        pmmQueryStr.Add($"{DatabaseObjects.Columns.Closed}='True'");
                    else
                        pmmQueryStr.Add($"{DatabaseObjects.Columns.Closed}<>'True'");
                }
                pmmQuery = UGITUtility.ConvertListToString(pmmQueryStr, " And ");

                List<string> nprQueryStr = new List<string>();
                if (nprRequestQuery.Count > 0)
                {
                    nprQueryStr.Add(UGITUtility.ConvertListToString(nprRequestQuery, " And "));
                }
                nprQueryStr.Add($"{DatabaseObjects.Columns.Closed}<>'True'");
                //nprQueryStr.Add(string.Format("<Neq><FieldRef Name='{0}'/><Value Type='Boolean'>1</Value></Neq>", DatabaseObjects.Columns.TicketClosed));
                //nprQueryStr.Add(string.Format("<Or><IsNull><FieldRef Name='{0}'/></IsNull><Eq><FieldRef Name='{0}'/><Value Type='Lookup'></Value></Eq></Or>", DatabaseObjects.Columns.TicketPMMIdLookup));

                string nprQuery = string.Empty;
                nprQuery = UGITUtility.ConvertListToString(nprQueryStr, " And ");
                //nprQuery.Query = string.Format("<Where>{0}</Where>", UGITUtility.GenerateWhereQueryWithAndOr(nprQueryStr, true));

                TicketManager ticketManagerObj = new TicketManager(_context);
                if (queryNPR)
                {
                    DataTable nprAllData = ticketManagerObj.GetAllTickets(nprModule);
                    DataRow[] nprRData = nprAllData.Select(nprQuery);
                    if (nprRData != null && nprRData.Count() > 0)
                    {
                        DataTable nprRDataTable = nprRData.CopyToDataTable();
                        result = GeneratePortfolioTable(_context, nprModule, nprRDataTable);
                    }
                }

                if (queryPMM)
                {
                    DataTable pmmAllData = ticketManagerObj.GetAllTickets(pmmModule);
                    DataRow[] pmmRData = pmmAllData.Select(pmmQuery);
                    if (pmmRData != null && pmmRData.Count() > 0)
                    {
                        DataTable pmmRDataTable = pmmRData.CopyToDataTable();
                        DataTable table = GeneratePortfolioTable(_context, pmmModule, pmmRDataTable);
                        if (result == null || result.Rows.Count <= 0)
                            result = table;
                        else if (table != null && table.Rows.Count > 0)
                            result.Merge(table);
                    }
                }

                return result;
            }

            private static DataTable GeneratePortfolioTable(ApplicationContext _context, UGITModule module, DataTable rawData)
            {
                DataTable result = CreateTable();
                if (rawData == null)
                    return result;

                string absoluteURL = UGITUtility.GetAbsoluteURL(module.StaticModulePagePath);
                rawData.DefaultView.Sort = DatabaseObjects.Columns.Title; // sort by title
                foreach (DataRowView rowView in rawData.DefaultView)
                {
                    DataRow rRow = rowView.Row;
                    DataRow newRow = result.NewRow();
                    result.Rows.Add(newRow);

                    newRow[DatabaseObjects.Columns.ModuleName] = module.ModuleName;

                    foreach (DataColumn dc in result.Columns)
                    {
                        object value = UGITUtility.GetSPItemValue(rRow, dc.ColumnName);
                        if (dc.DataType.FullName == "System.Single"
                                       || dc.DataType.FullName == "System.Double")
                        {
                            if (Convert.ToString(value) != string.Empty)
                                newRow[dc.ColumnName] = UGITUtility.StringToDouble(value);
                            else
                                newRow[dc.ColumnName] = 0;
                        }
                        else if (dc.DataType.FullName == "System.String" && !dc.ColumnName.ToLower().Contains("lookup"))
                        {
                            if (Convert.ToString(value) != string.Empty)
                                newRow[dc.ColumnName] = value.ToString();
                            else
                                newRow[dc.ColumnName] = " none";
                        }
                        else if (dc.DataType.FullName == "System.DateTime")
                        {
                            if (rRow[dc.ColumnName] != null && Convert.ToString(rRow[dc.ColumnName]) != string.Empty)
                                newRow[dc.ColumnName] = Convert.ToString(rRow[dc.ColumnName]);
                            else
                                newRow[dc.ColumnName] = DBNull.Value;
                        }
                        else
                        {
                            string htmlvalue = UGITUtility.StripHTML(Convert.ToString(rRow[dc.ColumnName]));
                            if (!string.IsNullOrEmpty(htmlvalue))
                                newRow[dc.ColumnName] = UGITUtility.StripHTML(Convert.ToString(rRow[dc.ColumnName]));
                            else
                                newRow[dc.ColumnName] = " none";
                        }
                    }

                    float estimatedCost = 0;
                    float.TryParse(Convert.ToString(rRow[DatabaseObjects.Columns.TicketTotalCost]), out estimatedCost);

                    float actualCost = 0;
                    float.TryParse(Convert.ToString(rRow[DatabaseObjects.Columns.ProjectCost]), out actualCost);
                    newRow["Actual"] = actualCost;
                    //newRow[DatabaseObjects.Columns.CostToCompletion] = estimatedCost;
                    newRow[DatabaseObjects.Columns.BudgetAmount] = estimatedCost;
                    newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("{0:C}", 0);

                    string ticketTitle = UGITUtility.TruncateWithEllipsis(newRow[DatabaseObjects.Columns.Title].ToString(), 100, string.Empty);
                    string title = string.Empty;
                    if (newRow[DatabaseObjects.Columns.TicketId] != null)
                    {
                        title = string.Format("{0}: ", newRow[DatabaseObjects.Columns.TicketId]);
                    }
                    title = string.Format("{0}{1}", title, ticketTitle);
                    title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!

                    if (estimatedCost > 0)
                    {
                        if (module.ModuleName == "PMM")
                        {
                            string url = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ProjectManagement.aspx");
                            newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=ProjectBudgetDetail&IsReadOnly=true&pmmid={1}&isdlg=1&isudlg=1','{3}', 90, 80);\">{2}</a>", url, rRow[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), title);
                        }
                        else
                        {
                            string url = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/newprojectmanagement.aspx");
                            newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=nprbudget&IsReadOnly=true&NPRID={1}','{3}', 90, 80);\">{2}</a>", url, rRow[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), title);
                        }
                    }

                    //added hidden span block to perform sorting correctly when we show data in gridview.
                    newRow[DatabaseObjects.Columns.TitleLink] = string.Format("<span style='display:none;'>{2}</span><a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','TicketId={1}','{3}', 90, 80);\">{2}</a>", absoluteURL, rRow[DatabaseObjects.Columns.TicketId], rRow[DatabaseObjects.Columns.Title], title);
                    newRow[DatabaseObjects.Columns.ModuleName] = module.ModuleName;
                    newRow[DatabaseObjects.Columns.Title] = rRow[DatabaseObjects.Columns.Title].ToString();
                }

                return result;
            }

            private static DataTable CreateTable()
            {
                DataTable result = new DataTable("ITGConstraints");
                result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                result.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketStatus, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.BudgetAmountWithLink, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.IsPrivate);
                DataColumn dateColumn = result.Columns.Add(DatabaseObjects.Columns.TicketActualStartDate, typeof(DateTime));
                dateColumn.AllowDBNull = true;

                dateColumn = result.Columns.Add(DatabaseObjects.Columns.TicketActualCompletionDate, typeof(DateTime));
                dateColumn.AllowDBNull = true;
                result.Columns.Add("Actual", typeof(double));
                //result.Columns.Add(DatabaseObjects.Columns.CostToCompletion, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
                //Measures
                result.Columns.Add(DatabaseObjects.Columns.TicketNoOfFTEs, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketNoOfFTEsNotes, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketNoOfConsultants, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketNoOfConsultantsNotes, typeof(string));
                //Constraints
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalCost, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalCostsNotes, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalStaffHeadcount, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalOnSiteConsultantHeadcount, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketTotalOffSiteConsultantHeadcount, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.TitleLink, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.ProjectInitiativeLookup, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.ProjectRank, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeCategory, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeLookup, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeSubCategory, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.ProjectCost, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.TicketRiskScore, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.TicketPctComplete, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.TicketProjectManager);
                result.Columns.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate, typeof(DateTime));
                return result;
            }

            public static void BindResourceAllocationData(ApplicationContext context, DataTable budgetData, string yearType, string allocationType, int year, bool isApprovedProjectRequests, bool isPendingApproval)
            {
                budgetData.Columns.Add("Q1", typeof(double));
                budgetData.Columns.Add("Q2", typeof(double));
                budgetData.Columns.Add("Q3", typeof(double));
                budgetData.Columns.Add("Q4", typeof(double));
                foreach (DataRow d in budgetData.Rows)
                {
                    d["Q1"] = 0;
                    d["Q2"] = 0;
                    d["Q3"] = 0;
                    d["Q4"] = 0;
                }
                if (yearType.ToLower() == "fiscal year")
                {
                    yearStartDate = new DateTime(year, 4, 1);
                    yearEndDate = new DateTime(year + 1, 3, 31);
                }
                else
                {
                    yearStartDate = new DateTime(year, 1, 1);
                    yearEndDate = new DateTime(year, 12, 31);
                }
                DataTable dtResourceAllocation = CreateResourceAllocationSchema(yearStartDate, yearEndDate);
                string query = GetQueryByWorkItemType(context, isApprovedProjectRequests, isPendingApproval);

                ResourceUsageSummaryMonthWiseManager monthwiseallocationManagerObj = new ResourceUsageSummaryMonthWiseManager(context);
                DataTable dtAllocationMonthWise = monthwiseallocationManagerObj.GetDataTable(query); // SPContext.Current.Web.GetSiteData(query);
                double monthlyAllocation = 0;
                if (dtAllocationMonthWise == null || dtAllocationMonthWise.Rows.Count == 0)
                    return;
                var drAllocationMonthWise = dtAllocationMonthWise.AsEnumerable().OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate)).ToArray();
                if (drAllocationMonthWise.Length == 0)
                    return;
                dtAllocationMonthWise = drAllocationMonthWise.CopyToDataTable();

                if (dtAllocationMonthWise != null && dtAllocationMonthWise.Rows.Count > 0)
                {
                    string prevMonth = string.Empty;
                    foreach (DataRow row in dtAllocationMonthWise.Rows)
                    {
                        string monthDate = Convert.ToDateTime(row[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM") + "-" + Convert.ToDateTime(row[DatabaseObjects.Columns.MonthStartDate]).ToString("yy");
                        DataRow tRow = dtResourceAllocation.NewRow();
                        //SPFieldLookupValue value = new SPFieldLookupValue(Convert.ToString(row[DatabaseObjects.Columns.Resource]));
                        monthlyAllocation = (UGITUtility.StringToDouble(row[DatabaseObjects.Columns.PctAllocation]) / 100);

                        tRow[DatabaseObjects.Columns.TicketId] = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.WorkItem]);
                        tRow[monthDate] = monthlyAllocation.ToString("N2");
                        dtResourceAllocation.Rows.Add(tRow);
                    }
                }
                int i = 1;

                List<UserProfile> lstAllUserProfile = context.UserManager.GetUsersProfile();
                //DataTable dttempUserProfile = lstAllUserProfile.ToDataTable();

                //List<TempUserInfo> lstUserProfile = dttempUserProfile.AsEnumerable()
                //                  .Select(s => new TempUserInfo
                //                  {
                //                      UGITStartDate = s.Field<DateTime>(DatabaseObjects.Columns.UGITStartDate),
                //                      UGITEndDate = s.Field<DateTime>(DatabaseObjects.Columns.UGITEndDate),
                //                      IsConsultant = s.IsNull(DatabaseObjects.Columns.IsConsultant) ? false : s.Field<Boolean>(DatabaseObjects.Columns.IsConsultant),
                //                      IsIT = s.IsNull(DatabaseObjects.Columns.IsIT) ? false : s.Field<Boolean>(DatabaseObjects.Columns.IsIT)
                //                  }).ToList<TempUserInfo>();

                for (DateTime startDate = yearStartDate; startDate <= yearEndDate; startDate = startDate.AddMonths(3))
                {
                    if (!dtResourceAllocation.Columns.Contains("Q" + i))
                        dtResourceAllocation.Columns.Add("Q" + i, typeof(double));
                    string month1 = Convert.ToDateTime(startDate).ToString("MMM") + "-" + Convert.ToDateTime(startDate).ToString("yy");
                    string month2 = Convert.ToDateTime(startDate.AddMonths(1)).ToString("MMM") + "-" + Convert.ToDateTime(startDate.AddMonths(1)).ToString("yy");
                    string month3 = Convert.ToDateTime(startDate.AddMonths(2)).ToString("MMM") + "-" + Convert.ToDateTime(startDate.AddMonths(2)).ToString("yy");

                    if (allocationType.ToLower() == "percentage")
                    {
                        int monthCounter = 0;
                        double availableQWorkingDays = 0;
                        while (monthCounter < 3)
                        {
                            DateTime monthStartDate = startDate.AddMonths(monthCounter);
                            DateTime monthEndDate = new DateTime(monthStartDate.Month == 12 ? monthStartDate.AddYears(1).Year : monthStartDate.Year, monthStartDate.AddMonths(1).Month, 1).AddDays(-1);
                            List<DateTime> monthWorkingDates = uHelper.GetTotalWorkingDateBetween(context, monthStartDate, monthEndDate);
                            int totalMWorkingDays = monthWorkingDates.Count();
                            if (totalMWorkingDays != 0)
                            {
                                double availableMWorkingDays = lstAllUserProfile.Select(x => (monthWorkingDates.Count(y => (x.UGITStartDate.Date == DateTime.MinValue.Date && x.UGITEndDate.Date == DateTime.MinValue.Date || x.UGITStartDate.Date <= y.Date && x.UGITEndDate >= y.Date))) / totalMWorkingDays).Sum();
                                availableQWorkingDays += availableMWorkingDays;
                                monthCounter++;
                            }
                        }
                        if (!dtResourceAllocation.Columns.Contains("TotalAvailableQ" + i))
                            dtResourceAllocation.Columns.Add("TotalAvailableQ" + i, typeof(double));

                        foreach (DataRow item in dtResourceAllocation.Rows)
                        {
                            item["TotalAvailableQ" + i] = availableQWorkingDays;
                        }
                        dtResourceAllocation.Columns["Q" + i].Expression = string.Format("([{0}]+[{1}]+[{2}])/[{3}]", month1, month2, month3, "TotalAvailableQ" + i);
                    }
                    else
                        dtResourceAllocation.Columns["Q" + i].Expression = string.Format("[{0}]+[{1}]+[{2}]", month1, month2, month3);
                    i++;
                }
                if (dtResourceAllocation == null || dtResourceAllocation.Rows.Count == 0)
                    return;
                var drResorceAllocation = (dtResourceAllocation.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).Select(y => new
                {
                    TicketId = y.Key,
                    Q1 = (y.Sum(s => s.Field<double>("Q1"))),
                    Q2 = y.Sum(s => s.Field<double>("Q2")),
                    Q3 = y.Sum(s => s.Field<double>("Q3")),
                    Q4 = y.Sum(s => s.Field<double>("Q4"))
                }).OrderBy(x => x.TicketId).ToArray());

                DataTable dtFinal = dtResourceAllocation.Clone();
                if (drResorceAllocation.Length == 0)
                    return;
                //dtFinal = drResorceAllocation.ToDataTable();
                var JoinResult = (from amw in dtResourceAllocation.AsEnumerable()
                                  join up in budgetData.AsEnumerable()
                                  on amw.Field<string>(DatabaseObjects.Columns.TicketId) equals up.Field<string>(DatabaseObjects.Columns.TicketId)
                                  select new { amw, up });
                foreach (var values in JoinResult)
                {
                    i = 1;
                    while (i <= 4)
                    {
                        if (allocationType.ToLower() == "percentage")
                            values.up["Q" + i] = Math.Round(Convert.ToDouble(values.amw["Q" + i]) * 100, 1);
                        else
                            values.up["Q" + i] = Math.Floor(Convert.ToDouble(values.amw["Q" + i]) * 100) / 100;
                        i++;
                    }
                }
            }

            private static DataTable CreateResourceAllocationSchema(DateTime yearStartDate, DateTime yearEndDate)
            {
                DataTable resourcePlanningSchema = new DataTable();
                resourcePlanningSchema.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));

                for (DateTime startDate = yearStartDate; startDate <= yearEndDate; startDate = startDate.AddMonths(1))
                {
                    string month = Convert.ToDateTime(startDate).ToString("MMM") + "-" + Convert.ToDateTime(startDate).ToString("yy");
                    resourcePlanningSchema.Columns.Add(month, typeof(double));
                    resourcePlanningSchema.Columns[month].DefaultValue = 0;
                }
                return resourcePlanningSchema;
            }


            private static string GetQueryByWorkItemType(ApplicationContext _context, bool isApprovedProjectRequests, bool isPendingApproval)
            {
                ModuleViewManager moduleManagerObj = new ModuleViewManager(_context);
                TicketManager ticketManager = new TicketManager(_context);

                LifeCycle lc = new LifeCycle();
                Ticket TicketRequest;
                TicketRequest = new Ticket(_context, "NPR");
                lc = TicketRequest.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                int approvestep = lc.Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved").StageStep;
                int closeStep = lc.Stages.FirstOrDefault(x => x.StageTypeChoice == "Closed").StageStep;
                List<string> expressionList = new List<string>();
                expressionList.Add($"{DatabaseObjects.Columns.WorkItemType}='PMM' ");

                if (isApprovedProjectRequests)
                {
                    UGITModule moduleRow = moduleManagerObj.LoadByName("NPR");
                    if (moduleRow != null)
                    {
                        DataTable tblNPRTicket = ticketManager.GetOpenTickets(moduleRow);   // uGITCache.ModuleDataCache.GetOpenTickets(Convert.ToInt32(moduleRow[DatabaseObjects.Columns.Id]));
                        if (tblNPRTicket != null && tblNPRTicket.Rows.Count > 0)
                        {
                            List<string> ids = new List<string>();
                            foreach (DataRow rowItem in tblNPRTicket.Rows)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.StageStep])) && Convert.ToInt32(rowItem[DatabaseObjects.Columns.StageStep]) == approvestep)
                                    ids.Add(Convert.ToString(rowItem[DatabaseObjects.Columns.TicketId]));
                            }
                            if (ids.Count > 0)
                                expressionList.Add(string.Format(UGITUtility.CamlIn(DatabaseObjects.Columns.ResourceWorkItem, false, ids.ToArray())));
                            else
                                expressionList.Add(string.Format(UGITUtility.CamlIn(DatabaseObjects.Columns.Id, false, 0)));
                        }

                    }
                }

                expressionList.Add($"{DatabaseObjects.Columns.MonthStartDate} >= '{yearStartDate}' And {DatabaseObjects.Columns.MonthStartDate} <= '{yearEndDate}'");

                return UGITUtility.ConvertListToString(expressionList, " And ");
            }

            public static DataTable GetAllocationData(string module, string ProjectPublicID, DateTime startDate, DateTime endDate)
            {
                DataTable data = new DataTable();
                data.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                data.Columns.Add(DatabaseObjects.Columns.ResourceId, typeof(int));
                data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
                data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));

                return data;
            }
        }
        public class Review
        {
            protected string Title { get; set; }
            protected string TicketSponsors { get; set; }
            protected string TicketBeneficiaries { get; set; }
            protected string TicketPriorityLookup { get; set; }
            protected string TicketStatus { get; set; }
            protected string BudgetAmount { get; set; }
            protected DateTime TicketActualStartDate { get; set; }
            protected DateTime TicketActualCompletionDate { get; set; }
            protected string ProjectStatus { get; set; }
            public string ITGReviewApproval { get; set; }


            //public static DataTable LoadByStageType(ApplicationContext context, string stageType)
            //{
                //DataTable nprList = new DataTable();
                //TicketManager ticketManager = new TicketManager(context);
                //ModuleViewManager ObjModuleViewManager = new ModuleViewManager(context);
                //UserProfileManager ObjUserProfileManager = new UserProfileManager(context);
                //ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
                //UGITModule module = ObjModuleViewManager.LoadByName(ModuleNames.NPR);
                //DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{ModuleNames.NPR}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                ////DataRow[] moduleStagesRow = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, "NPR").OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                //DataRow stage = moduleStagesRow.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.CustomProperties) != null && x.Field<string>(DatabaseObjects.Columns.CustomProperties).Contains(stageType));
                //if (stage == null)
                //    return CreateTable(); // return empty table

                //string stageId = stage[DatabaseObjects.Columns.Id].ToString();
                //DataTable result = CreateTable();
                //nprList = ticketManager.GetAllTickets(module);
                ////SPQuery rQuery = new SPQuery();
                ////rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleStepLookup, stageId);
                //string rQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleStepLookup, stageId);
                //DataRow[] nprListColl = nprList.Select(rQuery);

                ////SPListItem nprModule = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Modules, 6);
                //string nprAbsoluteURL = UGITUtility.GetAbsoluteURL(Convert.ToString(module.ModuleRelativePagePath));

                //// Check authorization to approve
                //bool authorizedToApprove = false;
                //string nprApprover = null;
                //if (stageType == "ITGReview")
                //    nprApprover = configurationVariableManager.GetValue(ConfigConstants.NPRITGovApprover);
                //else if (stageType == "ITSCReview")
                //    nprApprover = configurationVariableManager.GetValue(ConfigConstants.NPRITSCApprover);
                //if (!string.IsNullOrEmpty(nprApprover))
                //    authorizedToApprove = ObjUserProfileManager.CheckUserIsInGroup(nprApprover, context.CurrentUser);
                //result = nprListColl.CopyToDataTable();
                //foreach (DataRow npr in nprListColl)
                //{
                //    DataRow row = result.NewRow();
                //    foreach (DataColumn dc in row.Table.Columns)
                //    {
                //        if (!npr.Table.Columns.Contains(dc.ColumnName))
                //            continue;

                //        try
                //        {
                //            if (dc.ColumnName.Contains("Date"))
                //            {
                //                if (Convert.ToString(npr[dc.ColumnName]) != string.Empty)
                //                    row[dc.ColumnName] = ((DateTime)npr[dc.ColumnName]).ToString("MMM-d-yyyy");
                //                else
                //                    row[dc.ColumnName] = " - ";
                //            }
                //            else if (npr.Fields.GetField(dc.ColumnName).Type == SPFieldType.Lookup)
                //            {
                //                SPFieldLookupValueCollection values = new SPFieldLookupValueCollection(Convert.ToString(npr[dc.ColumnName]));
                //                if (values != null)
                //                {
                //                    row[dc.ColumnName] = string.Join("; ", values.Select(x => x.LookupValue));
                //                }
                //            }
                //            else if (npr.Fields.GetField(dc.ColumnName).Type == SPFieldType.User)
                //            {
                //                SPFieldUserValueCollection values = new SPFieldUserValueCollection(npr.Web, Convert.ToString(npr[dc.ColumnName]));
                //                if (values != null)
                //                {
                //                    row[dc.ColumnName] = string.Join("; ", values.Select(x => x.LookupValue));
                //                }
                //            }
                //            else
                //            {
                //                row[dc.ColumnName] = npr.GetFormattedValue(dc.ColumnName);
                //            }
                //        }
                //        catch
                //        {
                //            //row[dc.ColumnName] = string.Empty;
                //        }
                //    }

                //    SPList nprBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.NPRBudget);
                //    rQuery = new SPQuery();
                //    rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='true' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketNPRIdLookup, npr[DatabaseObjects.Columns.Id].ToString());

                //    SPListItemCollection budgetListFiltered = nprBudgetList.GetItems(rQuery);
                //    float estimatedCost = 0;
                //    foreach (SPListItem budget in budgetListFiltered)
                //    {
                //        estimatedCost += float.Parse(Convert.ToString(budget[DatabaseObjects.Columns.BudgetAmount]));
                //    }

                //    if (estimatedCost > 0)
                //    {
                //        string url = uHelper.GetAbsoluteURL("/_layouts/15/ugovernit/newprojectmanagement.aspx");
                //        row[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=nprbudget&IsReadOnly=true&NPRID={1}','NPR Request: {3}', 80, 50);\">{2}</a>",
                //                                                                            url, npr[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), npr[DatabaseObjects.Columns.TicketId]);
                //    }
                //    else
                //        row[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("{0:C}", 0);

                //    row[DatabaseObjects.Columns.BudgetAmount] = estimatedCost;
                //    row[DatabaseObjects.Columns.TitleLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','TicketId={1}','NPR Request: {1}', 90, 90);\">{2}</a>",
                //                                                        nprAbsoluteURL, npr[DatabaseObjects.Columns.TicketId], npr[DatabaseObjects.Columns.Title]);
                //    row[DatabaseObjects.Columns.ModuleName] = "NPR";
                //    row[DatabaseObjects.Columns.Title] = npr[DatabaseObjects.Columns.Title].ToString();
                //    row[DatabaseObjects.Columns.StageStep] = npr[DatabaseObjects.Columns.StageStep].ToString();
                //    row[DatabaseObjects.Columns.AuthorizedToApprove] = authorizedToApprove;
                //    result.Rows.Add(row);
                //}

                //return result;
           // }

            public static DataTable CreateTable()
            {
                DataTable result = new DataTable("ITGReview");
                result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                result.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketStatus, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
                result.Columns.Add(DatabaseObjects.Columns.BudgetAmountWithLink, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketActualStartDate, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketActualCompletionDate, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketSponsors, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.TicketBeneficiaries, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.AuthorizedToApprove, typeof(bool));
                result.Columns.Add(DatabaseObjects.Columns.TitleLink, typeof(string));
                result.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(string));

                return result;
            }
        }
        public void UpdateGroup(string firstID, string secondID, string groupby)
        {
            string moduleSource = uHelper.getModuleNameByTicketId(firstID.Trim());
            string moduleTarget = uHelper.getModuleNameByTicketId(secondID.Trim());
            UGITModule sourceModule = ModuleManagerObj.GetByName(moduleSource); // uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, moduleSource);
            //UGITModule targetModule = ModuleManagerObj.GetByName(moduleTarget); // uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, moduleTarget);

            //DataTable sourceList = null;
            //DataTable targetList = null;
            DataRow sourceItem = null;
            DataRow targetItem = null;
            //string querySource = null;
            if (moduleSource.ToLower() == moduleTarget.ToLower())
            {

                sourceItem = Ticket.GetCurrentTicket(AppContext, moduleSource, firstID);   // sourceColl.Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.TicketId]) == firstID);
                targetItem = Ticket.GetCurrentTicket(AppContext, moduleSource, secondID);  // sourceColl.Cast<SPListItem>().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.TicketId]) == secondID);
                UpdateGroup(groupby, sourceItem, targetItem, sourceModule.ModuleName, true);

            }
            else
            {
                sourceItem = Ticket.GetCurrentTicket(AppContext, moduleSource, firstID);
                targetItem = Ticket.GetCurrentTicket(AppContext, moduleTarget, secondID);
                UpdateGroup(groupby, sourceItem, targetItem, sourceModule.ModuleName);

            }
        }
        public void UpdateGroup(string groupby, DataRow sourceItem, DataRow targetItem, string moduleName, bool sourceOrTargetSame = false)
        {
            Ticket ticketManagerObj = new Ticket(AppContext, moduleName);
            switch (groupby)
            {
                case "0":
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, sourceItem.Table) && (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPriorityLookup, targetItem.Table) || sourceOrTargetSame))
                    {
                        sourceItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetItem[DatabaseObjects.Columns.TicketPriorityLookup];
                    }
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectRank, sourceItem.Table) && (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectRank, targetItem.Table) || sourceOrTargetSame))
                    {
                        sourceItem[DatabaseObjects.Columns.ProjectRank] = targetItem[DatabaseObjects.Columns.ProjectRank];
                    }
                    ticketManagerObj.CommitChanges(sourceItem);
                    return;
                case "1":
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup, sourceItem.Table) && (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestTypeLookup, targetItem.Table) || sourceOrTargetSame))
                    {
                        sourceItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup];
                    }
                    ticketManagerObj.CommitChanges(sourceItem);

                    return;
                case "2":
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectInitiativeLookup, sourceItem.Table) && (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectInitiativeLookup, targetItem.Table) || sourceOrTargetSame))
                    {
                        sourceItem[DatabaseObjects.Columns.ProjectInitiativeLookup] = targetItem[DatabaseObjects.Columns.ProjectInitiativeLookup];
                    }
                    ticketManagerObj.CommitChanges(sourceItem);

                    return;
                default:
                    return;
            }

        }
    }

}