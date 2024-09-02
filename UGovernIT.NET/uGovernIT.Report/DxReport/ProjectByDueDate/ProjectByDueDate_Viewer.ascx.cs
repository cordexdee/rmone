
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Core;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Manager.Helper;
namespace uGovernIT.Report.DxReport
{
    public partial class ProjectByDueDate_Viewer : UserControl
    {
        #region Helping Variable & Properties
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected DataTable resultentTable;
        protected DataTable dtTasks;
        protected DataTable dtCompletedTasks;
        protected DataTable dtIssues;
        DataTable dataSource;
        BusinessStrategyDashboard obj;
        public string FilterExpressionInitiative
        {
            get;
            set;
        }
        public string FilterExpressionProject { get; set; }
        public string FilterExpressionBusinessStrategy { get; set; }
        public string pmmUrl;
        public string nprUrl;
        public string bsTitle;
        protected DataTable dtInitiative;
        UGITModule nprModule;
        UGITModule pmmModule;
        public string DelegateURl = "Layouts/uGovernIT/delegatecontrol.aspx?control=businessstrategygroupeddata";
        public string filterUrl = string.Empty;
        public bool IsBusinessStrategyExist { get; set; }
        ModuleViewManager moduleViewManager;
        #endregion

        #region Methods & Events
        protected override void OnInit(EventArgs e)
        {
            filterUrl = _context.SiteUrl + DelegateURl;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //uGITCache Module
            moduleViewManager = new ModuleViewManager(_context);
            nprModule = moduleViewManager.LoadByName("NPR");
            pmmModule = moduleViewManager.LoadByName("PMM");
            //Details Url
            pmmUrl = UGITUtility.GetAbsoluteURL(pmmModule.DetailPageUrl);
            nprUrl = UGITUtility.GetAbsoluteURL(nprModule.DetailPageUrl);

            obj = new BusinessStrategyDashboard(_context);
            dataSource = obj.CreateSchema();
            dtInitiative = obj.GetIntiative();
            FilterListView(FilterCheckBox_cp, new EventArgs());
            dtTasks = obj.GetTasks();
            dtIssues = obj.GetIssues();
            //LoadGroupData();

        }
        protected void FilterListView(object sender, EventArgs e)
        {
            List<string> filterIdPost = new List<string>();
            if (FilterCheckBox_cp.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.CurrentProjects.ToString());
            }
            if (FilterCheckBox_apr.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.ApprovedNPRs.ToString());
            }
            if (FilterCheckBox_pa.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.PendingApprovalNPRs.ToString());
            }

            resultentTable = obj.FilterById(filterIdPost);
            LoadGroupData();
            hdnkeepFileters.Value = string.Join(",", filterIdPost);
        }
        protected void LoadGroupData()
        {

            dataSource.Clear();
            DateTime? previousDate = DateTime.MinValue;
            previousDate = DateTime.Now;

            DateTime nextDate = DateTime.MinValue;
            int looping = 1;
            int i = 0;
            string days = string.Empty;

            while (looping <= 6)
            {
                DataRow[] drColl = null;
                if (looping == 1)
                {
                    if (resultentTable != null)
                    {

                        drColl = resultentTable.AsEnumerable().Select(x => x).ToArray();
                        if (drColl != null && drColl.Length > 0)
                            days = string.Format("overdue");

                        nextDate = Convert.ToDateTime(previousDate);
                    }
                }
                if (looping != 5 && looping != 6 && (looping == 2 || looping == 3 || looping == 4))
                {
                    nextDate = Convert.ToDateTime(previousDate).AddMonths(1);
                    drColl = GetFilteredData(previousDate, nextDate);
                    if (drColl != null && drColl.Length > 0)
                        days = string.Format("{0}", 30 * (looping - 1));
                }
                else
                {
                    if (looping == 5)
                    {
                        nextDate = Convert.ToDateTime(previousDate).AddMonths(2);
                        drColl = GetFilteredData(previousDate, nextDate);

                        if (drColl != null && drColl.Length > 0)
                            days = string.Format("{0}", 30 * (looping));
                    }
                    if (looping == 6)
                    {

                        drColl = GetFilteredData(previousDate, null);
                        if (drColl != null && drColl.Length > 0)
                            days = "all";
                    }
                }


                if (drColl != null && drColl.Length > 0)
                {
                    string title = string.Format("Next {0} days", 30 * (looping - 1));
                    if (looping == 1)
                    {
                        title = string.Format("Overdue Projects");
                    }
                    else if (looping == 5)
                    {
                        title = string.Format("Next 4 to 6 month");
                    }
                    else if (looping == 6)
                        title = string.Format("Remaining");


                    if (looping == 1)
                    {
                        DataRow[] rowCollBefore = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) < DateTime.Now.Date).ToArray();
                        if (rowCollBefore != null && rowCollBefore.Length > 0)
                        {
                            dataSource = obj.GenerateData(dataSource, rowCollBefore.CopyToDataTable(), true, title);
                            dataSource.Rows[i]["projcount"] = string.Format("{0}", rowCollBefore.Length);
                            dataSource.Rows[i]["days"] = days;
                            dataSource.Rows[i]["previousDate"] = null;
                            dataSource.Rows[i]["nextDate"] = null;
                            i++;
                        }
                    }
                    else
                    {
                        dataSource = obj.GenerateData(dataSource, drColl.CopyToDataTable(), true, title);
                        dataSource.Rows[i]["projcount"] = string.Format("{0}", drColl.Length);
                        dataSource.Rows[i]["days"] = days;
                        dataSource.Rows[i]["previousDate"] = previousDate;
                        dataSource.Rows[i]["nextDate"] = nextDate;
                        i++;
                    }

                    days = string.Empty;
                    drColl = null;

                }
                previousDate = nextDate;
                looping++;
            }

            crdviewgroup1.DataSource = dataSource;
            crdviewgroup1.DataBind();
            looping = 1;
        }
        private DataRow[] GetFilteredData(DateTime? previousDate, DateTime? nextDate = null)
        {
            DateTime preDate = Convert.ToDateTime(previousDate);
            DataRow[] drColl = null;

            if (nextDate != null)
            {
                if (resultentTable != null && resultentTable.Rows.Count > 0)
                    drColl = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) >= preDate.Date && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) < Convert.ToDateTime(nextDate)).ToArray();
            }
            else
            {
                if (resultentTable != null && resultentTable.Rows.Count > 0)
                {
                    DataTable clone = resultentTable.Clone();

                    drColl = resultentTable.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate)).ToArray();
                    if (drColl != null && drColl.Length > 0)
                        clone = drColl.CopyToDataTable();

                    drColl = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) >= preDate.Date).ToArray();
                    if (drColl != null && drColl.Length > 0)
                        clone.Merge(drColl.CopyToDataTable());

                    drColl = clone.AsEnumerable().Select(x => x).ToArray();
                }
            }
            return drColl;
        }
        protected void crdviewgroup1_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = true;
            //if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "BI")
            //    flipview = true;
            if (!flipview)
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level'>Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level'>Risk Level</div>");
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget'><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double budgetamount = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("TotalAmount"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("TotalAmount")), 2);
                    double redthreshold = 0;
                    double yellowthreshold = 0;
                    if (budgetamount > 0)
                    {
                        //Get RedThresholdIn % of budget
                        redthreshold = (budgetamount * obj.AmountLeftRedThreshold) / 100;
                        //Get yellow threshold in % of budget
                        yellowthreshold = (budgetamount * obj.AmountLeftYellowThreshold) / 100;
                    }
                    double amountleft = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(e.Value)))
                    {
                        amountleft = Math.Round(Convert.ToDouble(e.Value), 2);
                    }

                    e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));

                    if (budgetamount == amountleft)
                        e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));
                    else if (amountleft < 0 && budgetamount >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));
                    else if (amountleft <= redthreshold)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));
                    else if (amountleft > redthreshold && amountleft <= yellowthreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(Convert.ToDouble(e.Value), 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration' ><span>{0} Months</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    double allmonths = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("MonthLeft"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("AllMonth")), 1);
                    double leftmonths = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Math.Round(Convert.ToDouble(e.Value), 1);

                    e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));

                    if (leftmonths == allmonths)
                        e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));
                    else if (leftmonths < 0 && allmonths >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));
                    else if (leftmonths <= obj.MonthsLeftRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));
                    else if (leftmonths > obj.MonthsLeftRedThreshold && leftmonths <= obj.MonthsLeftYellowThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(Convert.ToDecimal(e.Value), 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='green' title='Issues'><span>{0}</span></div>", txtissues);

                    int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                    if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                    else if (issues >= obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
                }
            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalPercentage = Math.Round(Convert.ToDouble(e.GetFieldValue("TopNBS")));
                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalPercentage > 100 ? 100 : totalPercentage));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = Convert.ToInt32(e.GetFieldValue("AmountLeftR"));

                    int yellowcount = Convert.ToInt32(e.GetFieldValue("AmountLeftY"));
                    int greencount = Convert.ToInt32(e.GetFieldValue("AmountLeftG"));

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects &#013 Red: High &#013 Yellow: Medium &#013 Green: Minimal'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = Math.Round(Convert.ToDouble(e.GetFieldValue("LongestInstanceBS")), 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = Convert.ToInt32(e.GetFieldValue("MonthLeftR"));
                    int yellowcount = Convert.ToInt32(e.GetFieldValue("MonthLeftY"));
                    int greencount = Convert.ToInt32(e.GetFieldValue("MonthLeftG"));
                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects &#013 Red: Overdue &#013 Yellow: Slightly Late &#013 Green: On Time'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = Convert.ToInt32(e.GetFieldValue("IssuesR"));
                    int yellowcount = Convert.ToInt32(e.GetFieldValue("IssuesY"));
                    int greencount = Convert.ToInt32(e.GetFieldValue("IssuesG"));
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues &#013 Red: High &#013 Yellow: Medium &#013 Green: Minimal'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = Convert.ToInt32(e.GetFieldValue("RiskLevelR"));
                    int yellowcount = Convert.ToInt32(e.GetFieldValue("RiskLevelY"));
                    int greencount = Convert.ToInt32(e.GetFieldValue("RiskLevelG"));
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks &#013 Red: High Risk Probability &#013 Yellow: Medium &#013 Green: Minimal'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
            }
        }
        #endregion
    }
}
