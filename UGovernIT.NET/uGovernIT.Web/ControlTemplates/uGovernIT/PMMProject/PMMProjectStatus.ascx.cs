using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.uGovernIT.PMMProject
{
    public partial class PMMProjectStatus : UserControl
    {
        public string FrameId { get; set; }
        public string TicketId { get; set; }

        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }

        public double BaselineId { get; set; }

        public int PMMID { get; set; }

        private const string SELECTEDTABCOOKIE = "TicketSelectedTab";

        //private bool isAccomplishmentsDone;
        //private bool isImmediatePlansDone;
        //private bool isIssuesDone;
        //private bool isRisksDone;

        //DataTable projectMonitorState;
        //DataTable projectMonitorOptions;    
       
        Ticket ticketRequest = null;
        LifeCycle projectLifeCycle = null;
        LifeCycleStage currentLifeCycleStage = null;
       // HtmlEditorControl htmlEditor = null;

        protected DataRow pmmItem = null;

        protected string pmmPublicId;

        protected string ajaxHelperURL = string.Empty;
        protected string currentModulePagePath;
        protected string editIssueFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=pmmissuesedit");
        protected string newIssueFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=pmmissuesnew");
        protected string newRiskFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=PMMRisksNew");
        protected string itemFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=pmmstatusitem");
        protected string editRiskFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=PMMRisksEdit");
        protected string newDecisionLogFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=PMMDecisionLogNew");
        protected string editDecisionLogFormUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=pmmdecisionlogedit");
        protected string moduleName = "PMM";

        protected long closeStageId = 0;


        public UGITModule module = null;
        
        UserProfile User;

        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());

        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/_Layouts/15/ugovernit/ajaxhelper.aspx");
            //PMMID
            ExecutiveSummary.PMMID = PMMID;
            ProjectMonitors.PMMID = PMMID;
            PhaseChanges.PMMID = PMMID;
            Accomplishments.PMMID = PMMID;
            ImmediatePlans.PMMID = PMMID;
            ProjectRisks.PMMID = PMMID;
            Issues.PMMID = PMMID;
            DecisionLogList.PMMID = PMMID;

            //ShowBaseline
            Accomplishments.IsShowBaseline = IsShowBaseline;           
            ImmediatePlans.IsShowBaseline = IsShowBaseline;
            ProjectRisks.IsShowBaseline = IsShowBaseline;
            Issues.IsShowBaseline = IsShowBaseline;
            DecisionLogList.IsShowBaseline = IsShowBaseline;
            ProjectMonitors.IsShowBaseline = IsShowBaseline;
            PhaseChanges.IsShowBaseline = IsShowBaseline;
            ExecutiveSummary.IsShowBaseline = IsShowBaseline;

            //ExecutiveSummary = PMMID;
            ProjectMonitors.PMMID = PMMID;
            PhaseChanges.PMMID = PMMID;

            //BaselineId
            Accomplishments.BaselineId = BaselineId;
            ImmediatePlans.BaselineId = BaselineId;
            ProjectRisks.BaselineId = BaselineId;
            Issues.BaselineId = BaselineId;
            DecisionLogList.BaselineId = BaselineId;
            ProjectMonitors.BaselineId = BaselineId;
            ExecutiveSummary.BaselineId = BaselineId;
            

            module = ModuleManager.GetByName(moduleName);

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {

            

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");
            UGITModule module = ModuleManager.GetByName("PMM");
            pmmItem = ticketManager.GetByID(module, PMMID); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);
            projectLifeCycle = ticketRequest.GetTicketLifeCycle(pmmItem);
            LifeCycleStage closeStage = ticketRequest.GetTicketCloseStage(pmmItem);
            if (closeStage != null)
                closeStageId = closeStage.ID;

            currentLifeCycleStage = ticketRequest.GetTicketCurrentStage(pmmItem, projectLifeCycle);
            pmmPublicId = Convert.ToString(pmmItem[DatabaseObjects.Columns.TicketId]);

            ProjectMonitors.ticketId = pmmPublicId;
            ExecutiveSummary.ticketId = pmmPublicId;

            // Check whether current logged in user is authorise to edit item or not.
            if (!IsReadOnly && !Ticket.IsActionUser(HttpContext.Current.GetManagerContext(), pmmItem, User) && !UserManager.IsDataEditor(pmmItem, User))
            {
                IsReadOnly = true;
            }

            if (IsReadOnly)
            {
                ExecutiveSummary.IsReadOnly = true;
                ProjectMonitors.IsReadOnly = true;
                PhaseChanges.IsReadOnly = true;
                Accomplishments.IsReadOnly = true;
                ImmediatePlans.IsReadOnly = true;
                ProjectRisks.IsReadOnly = true;
                Issues.IsReadOnly = true;
                DecisionLogList.IsReadOnly = true;
                if (PMMID > 0)
                {
                    List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                    if (historyList.Count > 0)
                    {
                        // lbReadOnlyStatusSummary.Text = historyList.First().entry;
                    }

                    ////Fill project stage dropdown
                    //lbProjectStatus.Text = UGITUtility.SplitString(pmmItem[DatabaseObjects.Columns.ModuleStepLookup], Constants.Separator, 1);

                    //addIssueRVPanel.Visible = true;
                    //if (Request["enablePrint"] != null)
                    //    addIssueRVPanel.Visible = false;
                }
            }
            else if (IsShowBaseline)
            {
                #region "commentedBaselineCode"
                //projectStatusEditMode.Visible = false;
                //projectStatusReadMode.Visible = true;
                //SPQuery pmmQuery = new SPQuery();
                //pmmQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE' /><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, PMMID, DatabaseObjects.Columns.BaselineNum, BaselineNum);

                //// Bind baseline
                //projectMonitorState = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectMonitorStateHistory);
                //projectMonitorOptions = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleMonitorOptions);
                //SPListItemCollection monitors = projectMonitorState.GetItems(pmmQuery);
                //rReadOnlyMonitors.DataSource = monitors.GetDataTable();
                //rReadOnlyMonitors.DataBind();

                //// Get item version
                //SPListItemVersion itemVersion = pmmItem.Versions.GetVersionFromLabel(Convert.ToString(BaselineNum));
                //if (itemVersion != null)
                //{
                //    //Fill project stage dropdown
                //    lbProjectStatus.Text = Convert.ToString(itemVersion[DatabaseObjects.Columns.TicketStatus]);
                //}

                //List<HistoryEntry> historyList = UGITUtility.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                //if (historyList.Count > 0)
                //{
                //    lbReadOnlyStatusSummary.Text = historyList.First().entry;
                //}

                //// Bind Accomplishments
                //SPQuery accompleshmentQuery = new SPQuery();
                //accompleshmentQuery.Query = string.Format("<Where><And><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Choice'>{3}</Value></Eq></And><Eq><FieldRef Name='{4}' LookupId='TRUE' /><Value Type='Lookup'>{5}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, PMMID, DatabaseObjects.Columns.ProjectNoteType, "Accomplishments", DatabaseObjects.Columns.BaselineNum, BaselineNum);
                //SPListItemCollection accomplishments = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMCommentsHistory, accompleshmentQuery);
                //lvReadOnlyAccomplishments.DataSource = accomplishments.GetDataTable();
                //lvReadOnlyAccomplishments.DataBind();

                ////Bind Immediate plans
                //SPQuery immediatePlanQuery = new SPQuery();
                //immediatePlanQuery.Query = string.Format("<Where><And><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Choice'>{3}</Value></Eq></And><Eq><FieldRef Name='{4}' LookupId='TRUE' /><Value Type='Lookup'>{5}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, PMMID, DatabaseObjects.Columns.ProjectNoteType, "Immediate Plans", DatabaseObjects.Columns.BaselineNum, BaselineNum);
                //SPListItemCollection immediatePlans = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMCommentsHistory, immediatePlanQuery);
                //lvReadOnlyImmediatePlans.DataSource = immediatePlans.GetDataTable();
                //lvReadOnlyImmediatePlans.DataBind();

                ////Bind Issues
                //SPQuery issueQuery = new SPQuery();
                //SPListItemCollection issues = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMIssuesHistory, pmmQuery);
                //DataTable issueData = issues.GetDataTable();
                //if (issueData != null)
                //{
                //    foreach (DataRow row in issueData.Rows)
                //    {
                //        row[DatabaseObjects.Columns.AssignedTo] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));
                //    }
                //}
                //lvReadOnlyIssues.DataSource = issueData;
                //lvReadOnlyIssues.DataBind();
                #endregion
            }
        }

        protected string SetStyle_Archive(PMMProjectStatus task)
        {
            string style = string.Empty;
            bool isDeleted = UGITUtility.StringToBoolean(Eval(DatabaseObjects.Columns.Deleted));
            if (isDeleted)
            {
                style = "archived-item";
            }
            return style;
        }

        protected void headerFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {

            if (e.Column.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
                List<string> values = new List<string>(); // List in string format used to keep out duplicates
                foreach (FilterValue fValue in e.Values)
                {
                    if (fValue.Value.Contains(";"))
                    {
                        // Found multiple semi-colon separated values
                        string[] vals = fValue.Value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in vals)
                        {
                            // Add to filter list only if not already in it
                            string trimmedVal = val.Trim();
                            if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                            {
                                temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                                values.Add(trimmedVal);
                            }
                        }
                    }
                    else
                    {
                        // Single value, add to list if not already in it
                        string trimmedVal = fValue.Value.Trim();
                        if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                        {
                            temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                            values.Add(trimmedVal);
                        }
                    }
                }

                // Add to filter list in order
                temp = temp.OrderBy(o => o.Value).ToList();
                e.Values.Clear();
                foreach (FilterValue fVal in temp)
                {
                    e.Values.Add(fVal);
                }
            }
        }

        protected void btCancelAddSummary_Click(object sender, EventArgs e)
        {
            //trAddSummaryBox.Visible = false;
            //trShowLatestSummary.Visible = true;
        }

        protected void btAddProjectSummary_Click1(object sender, EventArgs e)
        {
            //if (txtProjectSummary.Text.Trim() != string.Empty)
            //{
                //pmmItem[DatabaseObjects.Columns.ProjectSummaryNote] = uHelper.GetVersionString(SPContext.Current.Web.CurrentUser, htmlEditor.Html.Trim(), pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                //pmmItem.UpdateOverwriteVersion();
                //htmlEditor.Html = string.Empty;

                //trAddSummaryBox.Visible = false;
                //trShowLatestSummary.Visible = true;
            //}
        }
    }
}
