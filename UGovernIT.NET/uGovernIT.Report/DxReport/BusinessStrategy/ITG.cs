using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Report.DxReport
{
    class ITG
    {

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
            //public SPFieldChoice ITGReviewApproval { get; set; }


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

            public static DataTable LoadAll(ApplicationContext context, List<Constants.ProjectType> projectType)
            {
                DataTable result = CreateTable();
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                UGITModule nprModule = moduleManager.LoadByName(ModuleNames.NPR);    // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, "NPR");
                UGITModule pmmModule = moduleManager.LoadByName(ModuleNames.PMM);    // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, "PMM");

                bool queryNPR = false;
                bool queryPMM = false;

                //Builds query based on type
                List<string> requiredQuery = new List<string>();
                List<string> nprRequestQuery = new List<string>();
                string _nprRequestQuery = string.Empty;

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
                    nprRequestQuery.Add(string.Format("{0} = {1}", DatabaseObjects.Columns.StageStep, approvedStage.StageStep));
                    _nprRequestQuery = string.Format("{0} = {1}", DatabaseObjects.Columns.StageStep, approvedStage.StageStep);
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
                        stepExps.Add(string.Format("{0} = {1}", DatabaseObjects.Columns.StageStep, stage.StageStep));
                    }

                    // Then exclude all projects on hold
                    if (!projectType.Contains(Constants.ProjectType.OnHold))
                    {
                        //nprRequestQuery.Add(string.Format("<And>{2}<Neq>{0} = {1}",
                        //   DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus, UGITUtility.GenerateWhereQueryWithAndOr(stepExps, stepExps.Count - 1, false)));
                        _nprRequestQuery += $" [StageStep] > 1 AND [StageStep] < 6 AND [{DatabaseObjects.Columns.TicketStatus}]!='{Constants.OnHoldStatus}' ";
                    }
                    else
                    {
                       // nprRequestQuery.Add(UGITUtility.GenerateWhereQueryWithAndOr(stepExps, stepExps.Count - 1, false));
                        _nprRequestQuery += " [StageStep] > 1 AND [StageStep] < 6 ";

                    }
                }

                if (projectType.Contains(Constants.ProjectType.OnHold))
                {
                    //Shows Only onhold tickets
                   // nprRequestQuery.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus));
                    _nprRequestQuery += $" and {DatabaseObjects.Columns.TicketStatus}='{Constants.OnHoldStatus}' ";
                }

                StringBuilder pmmQuery = new StringBuilder();
                List<string> pmmQueryStr = new List<string>();
                if (requiredQuery.Count > 0)
                {
                    pmmQueryStr.Add(UGITUtility.GenerateWhereQueryWithAndOr(requiredQuery, false));
                }

                if (!(projectType.Contains(Constants.ProjectType.CompletedProjects) && projectType.Contains(Constants.ProjectType.CurrentProjects)))
                {
                    if (projectType.Contains(Constants.ProjectType.CompletedProjects) && !projectType.Contains(Constants.ProjectType.CurrentProjects))
                        pmmQueryStr.Add(string.Format("{0} = 'True'", DatabaseObjects.Columns.TicketClosed));
                    else
                        pmmQueryStr.Add(string.Format("{0} <> 'True'", DatabaseObjects.Columns.TicketClosed));
                    pmmQueryStr.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, context.TenantID));
                }
                pmmQuery.Append(string.Format(UGITUtility.GenerateWhereQueryWithAndOr(pmmQueryStr, true)));

                //List<string> nprQueryStr = new List<string>();
                //if (nprRequestQuery.Count > 0)
                //{
                //    nprQueryStr.Add(UGITUtility.GenerateWhereQueryWithAndOr(nprRequestQuery, false));

                //}
                StringBuilder nprQuery = new StringBuilder();
                //_nprRequestQuery += $" And Closed!=1 and TenantID='{context.TenantID}'";
                nprRequestQuery.Add(string.Format("{0}! = {1} and {2} = '{3}'", DatabaseObjects.Columns.Closed, 1, DatabaseObjects.Columns.TenantID, context.TenantID));
                //nprQueryStr.Add(string.Format("<Neq><FieldRef Name='{0}'/><Value Type='Boolean'>1</Value></Neq>", DatabaseObjects.Columns.TicketClosed));
                //nprQueryStr.Add(string.Format("<Or><IsNull><FieldRef Name='{0}'/></IsNull><Eq><FieldRef Name='{0}'/><Value Type='Lookup'></Value></Eq></Or>", DatabaseObjects.Columns.TicketPMMIdLookup));
                nprQuery.Append(string.Format(UGITUtility.GenerateWhereQueryWithAndOr(nprRequestQuery, true)));
                _nprRequestQuery=nprQuery.ToString();
                //  StringBuilder nprQuery = new StringBuilder();
                //  nprQuery.Append(string.Format("({0})", UGITUtility.GenerateWhereQueryWithAndOr(nprQueryStr, true)));
                //nprQuery.Append(string.Format("<Where>{0}</Where>", UGITUtility.GenerateWhereQueryWithAndOr(nprQueryStr, true)));


                TicketManager ticketManager = new TicketManager(context);
                if (queryNPR)
                {
                    DataRow[] nprItemCollection = ticketManager.GetAllTickets(nprModule).Select(); // SPListHelper.GetSPListItemCollection(nprModule.ModuleTicketTable, nprQuery, spWeb);
                    DataTable nprRData = ticketManager.GetAllTickets(nprModule);
                    //DataTable nprRData = GetTableDataManager.GetTableData(nprModule.ModuleName, _nprRequestQuery);
                    if (nprRData != null)
                    {
                        result = GeneratePortfolioTable(nprModule, nprRData);
                    }
                }

                if (queryPMM)
                {
                    //SPListItemCollection pmmItemCollection = SPListHelper.GetSPListItemCollection(pmmModule.ModuleTicketTable, pmmQuery, spWeb);
                    DataTable pmmRData = ticketManager.GetOpenTickets(pmmModule); //GetTableDataManager.GetTableData(pmmModule.ModuleName, pmmQuery);
                    if (pmmRData != null)
                    {
                        DataTable table = GeneratePortfolioTable(pmmModule, pmmRData);
                        if (result == null || result.Rows.Count <= 0)
                            result = table;
                        else if (table != null && table.Rows.Count > 0)
                            result.Merge(table);
                    }
                }

                return result;
            }

            private static DataTable GeneratePortfolioTable(UGITModule module, DataTable rawData)
            {
                DataTable result = CreateTable();
                if (rawData == null)
                    return result;

                string absoluteURL = UGITUtility.GetAbsoluteURL(module.DetailPageUrl);
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
                                newRow[dc.ColumnName] = string.Empty;
                        }
                        else if (dc.DataType.FullName == "System.DateTime")
                        {
                            if (rRow[dc.ColumnName] != null && Convert.ToString(rRow[dc.ColumnName]) != string.Empty)
                                newRow[dc.ColumnName] = Convert.ToString(rRow[dc.ColumnName]);
                            else
                                newRow[dc.ColumnName] = DBNull.Value;
                        }
                        else if (dc.ColumnName == DatabaseObjects.Columns.TicketPriorityLookup)
                        {
                            newRow[dc.ColumnName] = Convert.ToString(rRow[dc.ColumnName + "$"]);
                        }
                        else
                            newRow[dc.ColumnName] = UGITUtility.StripHTML(Convert.ToString(rRow[dc.ColumnName]));
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
                    title = uHelper.ReplaceInvalidCharsInURL(title);// # ' " cause issues!

                    if (estimatedCost > 0)
                    {
                        if (module.ModuleName == "PMM")
                        {
                            string url = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ProjectManagement.aspx");
                            newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=ProjectBudgetDetail&IsReadOnly=true&pmmid={1}&isdlg=1&isudlg=1','{3}', 90, 80);\">{2}</a>", url, rRow[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), title);
                        }
                        else
                        {
                            string url = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/newprojectmanagement.aspx");
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
            public static DataTable GetAllocationData(ApplicationContext context, string module, string ProjectPublicID, DateTime startDate, DateTime endDate)
            {
                DataTable data = new DataTable();
                data.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                data.Columns.Add(DatabaseObjects.Columns.ResourceId, typeof(string));
                data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
                data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));
                int workingHrs = uHelper.GetWorkingHoursInADay(context, false);
                UserProfileManager userProfileManager = new UserProfileManager(context);
                ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
                string level1Type = module == "PMM" ? Constants.RMMLevel1PMMProjectsType : Constants.RMMLevel1NPRProjectsType;
                List<RResourceAllocation> resAlloctions = resourceAllocationManager.LoadByWorkItem(level1Type, ProjectPublicID,null, 4, false, true);
                if (resAlloctions == null || resAlloctions.Count <= 0)
                    return data;

                List<RResourceAllocation> projectResources = new List<RResourceAllocation>();
                List<string> userIds = resAlloctions.Select(x => x.Resource).Distinct().ToList();
                 
                foreach (string ur in userIds)
                {
                    DataRow newRow = data.NewRow();
                    newRow[DatabaseObjects.Columns.ResourceId] = ur;
                    newRow[DatabaseObjects.Columns.Resource] = userProfileManager.GetUserById(ur).Name;
                    //newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(Math.Round(rAllocation.PctAllocation, 0));
                    data.Rows.Add(newRow);


                   // RResourceAllocation allc = null;
                   // List<RResourceAllocation> multiAllc = resAlloctions.Where(x => x.Resource == ur).ToList();
                   // if (multiAllc != null && multiAllc.Count >= 0)
                   // {
                   //     //allc = (ResourceAllocation)multiAllc.First().Clone();
                   //     projectResources.Add(allc);
                   // }

                   // /* Combine multiple entries and calculate %age allocated using A/B where
                   //   B = total working days for all allocations that fall within the range
                   //   A = total allocation for the total of that time period
                   //*/
                   // if (multiAllc.Count > 1)
                   // {
                   //     allc.AllocationStartDate = multiAllc.Min(x => x.AllocationStartDate);
                   //     allc.AllocationEndDate = multiAllc.Max(x => x.AllocationEndDate);
                   //     double projectWrkHrs = 0;
                   //     int totalWrkHrs = 0;
                   //     foreach (RResourceAllocation sAllc in multiAllc)
                   //     {
                   //         //DateTime sDate = sAllc.AllocationStartDate;
                   //         //DateTime eDate = sAllc.AllocationEndDate;
                   //         //if (startDate.Date > sDate.Date)
                   //         //    sDate = startDate.Date;
                   //         //if (endDate.Date < eDate.Date)
                   //         //    eDate = endDate.Date;

                   //         //int workDays = uHelper.GetTotalWorkingDaysBetween(context,sDate, eDate);
                   //         //projectWrkHrs += (workDays * workingHrs * sAllc.PctAllocation) / 100;
                   //         //totalWrkHrs += workDays * workingHrs;
                   //     }

                   //     if (totalWrkHrs == 0)
                   //         allc.PctAllocation = 0;
                   //     else
                   //         allc.PctAllocation = (projectWrkHrs * 100) / totalWrkHrs;
                   // }
                }

                //List<RResourceAllocation> selectAllocations = resourceAllocationManager.LoadByResource(userIds, 4);
                //selectAllocations = selectAllocations.Where(x => x.StartDate.Date <= endDate.Date && x.EndDate.Date >= startDate.Date).ToList();

                foreach (RResourceAllocation rAllocation in projectResources)
                {
                    //DataRow newRow = data.NewRow();
                    //newRow[DatabaseObjects.Columns.Id] = rAllocation.ProjectEstimatedAllocationId;
                    //newRow[DatabaseObjects.Columns.ResourceId] = rAllocation.Resource;
                    ////newRow[DatabaseObjects.Columns.Resource] = rAllocation.ResourceName;
                    ////newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(Math.Round(rAllocation.PctAllocation, 0));
                    //data.Rows.Add(newRow);
                }
                return data;
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
        }
        public class Constraints
        {
            protected string Constraint { get; set; }
            protected string Value { get; set; }
            protected string Note { get; set; }

            private static string[] types = { "NPR", "PMM" };
            private static string[][] constraintFields = { new string[] {"No of FTEs", "TicketNoOfFTEs","TicketNoOfFTEsNotes"},
                                                            new string[] {"No of Consultants", "TicketNoOfConsultants","TicketNoOfConsultantsNotes"}};
            public static DataTable LoadById(List<string[]> ids, DataTable table)
            {
                DataTable result = CreateTable();

                foreach (string[] constraintField in constraintFields)
                {
                    DataRow row = result.NewRow();
                    float totalValue = 0;
                    string totalNotes = string.Empty;
                    foreach (string[] id in ids)
                    {
                        DataRow ticket = table.Select("TicketID ='" + id[1] + "'")[0];

                        float ticketTotalCost = 0;
                        float.TryParse(Convert.ToString(ticket[constraintField[1]]), out ticketTotalCost);
                        totalValue += ticketTotalCost;

                        string note = Convert.ToString(ticket[constraintField[2]]);
                        if (note != string.Empty)
                            totalNotes += note + "<br /><br />";
                    }
                    row["Constraint"] = constraintField[0];
                    row["Value"] = totalValue;
                    row["Note"] = totalNotes;
                    result.Rows.Add(row);

                }
                return result;
            }
            private static DataTable CreateTable()
            {
                DataTable result = new DataTable("ITGPortfolio");
                result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                result.Columns.Add("Constraint", typeof(string));
                result.Columns.Add("Value", typeof(string));
                result.Columns.Add("Note", typeof(string));
                return result;
            }
        }
        public class Measures
        {
            protected string Constraint { get; set; }
            protected string Value { get; set; }
            protected string Note { get; set; }

            private static string[] types = { "NPR", "PMM" };
            private static string[][] measureFields = {
                    new string[] {"Total Cost", "BudgetAmount", "TicketTotalCostsNotes"},
                    new string[] {"Total Staff Headcount", "TicketTotalStaffHeadcount", "TicketTotalStaffHeadcountNotes"},
                    new string[] {"Total OnSite Consultant Headcount", "TicketTotalOnSiteConsultantHeadcount", "TicketTotalConsultantHeadcountNotes"},
                    new string[] {"Total OffSite Consultant Headcount", "TicketTotalOffSiteConsultantHeadcount", "TicketTotalConsultantHeadcountNotes"},
                                                        };
            public static DataTable LoadById(List<string[]> ids, DataTable table)
            {
                DataTable result = CreateTable();
                foreach (string[] measureField in measureFields)
                {
                    DataRow row = result.NewRow();
                    float totalValue = 0;
                    string totalNotes = string.Empty;
                    foreach (string[] id in ids)
                    {
                        if (id.Length == 2)
                        {
                            DataRow ticket = table.Select("TicketID ='" + id[1] + "'")[0];

                            float ticketTotalCost = 0;
                            float.TryParse(Convert.ToString(ticket[measureField[1]]), out ticketTotalCost);
                            totalValue += ticketTotalCost;
                            string note = string.Empty; ;
                            note = Convert.ToString(ticket[measureField[2]]);
                            if (note != string.Empty)
                                totalNotes += note + "<br /><br />";
                        }
                    }
                    row["Measure"] = measureField[0];
                    row["Value"] = totalValue;
                    row["Note"] = totalNotes;
                    result.Rows.Add(row);

                }
                return result;
            }
            private static DataTable CreateTable()
            {
                DataTable result = new DataTable("ITGMeasures");
                result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
                result.Columns.Add("Measure", typeof(string));
                result.Columns.Add("Value", typeof(string));
                result.Columns.Add("Note", typeof(string));
                return result;
            }

        }

    }

}