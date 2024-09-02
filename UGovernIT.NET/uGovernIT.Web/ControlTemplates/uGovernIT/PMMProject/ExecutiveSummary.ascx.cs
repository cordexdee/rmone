using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ExecutiveSummary : UserControl
    {
        public int PMMID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }
        public double BaselineId { get; set; }
        public string ticketId { get; set; }

        protected DataRow pmmItem = null;
        protected string currentModulePagePath;

        Ticket ticketRequest = null;
        //LifeCycle projectLifeCycle = null;
        //LifeCycleStage currentLifeCycleStage = null;
        HtmlEditorControl htmlEditor = null;
        UserProfile User;
        private ApplicationContext _context = null;

        
       

        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        TicketManager TicketManagerObj = new TicketManager(HttpContext.Current.GetManagerContext());
        Ticket TicketObj = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

       protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            htmlEditor = (HtmlEditorControl)HtmlEditorControl;
            htmlEditor.OnSubmit +=htmlEditor_OnSubmit;
          
        }

        void htmlEditor_OnSubmit(object sender, EventArgsSubmitEventHandler e)
        {

            if (e.Value != string.Empty)
            {
                pmmItem[DatabaseObjects.Columns.ProjectSummaryNote] = uHelper.GetVersionString(User.Name, e.Value, pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                TicketObj.CommitChanges(pmmItem);
                htmlEditor.HtmlData = UGITUtility.ObjectToString(e.Value);
                //htmlEditor.Html = string.Empty;
                lbReadOnlyStatusSummary.Text = UGITUtility.ObjectToString(e.Value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");

            UGITModule module = ModuleManager.GetByName(ModuleNames.PMM);
            pmmItem = TicketManagerObj.GetByID(module, PMMID);
            if (IsShowBaseline)
                htmlEditor.IsShowBaseline = true;

            /// code moved to page load from pre render
            if (IsReadOnly)
            {
                trShowLatestSummary.Visible = false;
                trAddSummaryBox.Visible = false;
                if (PMMID > 0)
                {
                    List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                    if (historyList.Count > 0)
                    {
                        lbReadOnlyStatusSummary.Text = historyList.First().entry;
                    }
                }
            }
            //Baseline to show ProjectSummaryNote

            else if (IsShowBaseline)
            {
                lbReadOnlyStatusSummary.Visible = false;
                DataRow[] pmmhistoryCollection = GetTableDataManager.GetTableData("pmmhistory", $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.BaselineId}={BaselineId} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'").Select();
                if (pmmhistoryCollection != null && pmmhistoryCollection.Count() > 0)
                {
                    pmmItem = pmmhistoryCollection[0];
                    if (pmmItem != null)
                    {
                        List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);

                        txtProjectSummary.Text = historyList.First().entry;
                    }
                }
            }
            else
            {
                trShowLatestSummary.Visible = true;
                trAddSummaryBox.Visible = false;
                string historyUrlPath = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=projectAllStatusSummary"); ;
                btProjectSummaryHistory.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', 'ticketid={1}', '{1}: Executive Summary History', '800px', '500px', 0)", historyUrlPath, pmmItem[DatabaseObjects.Columns.TicketId]));
                List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                if (historyList.Count > 0)
                {
                    //htmlEditor.CurrentHtml = htmlEditor.Html;
                   //htmlEditor.Html = historyList.First().entry;
                    htmlEditor.HtmlData = historyList.First().entry;

                    lbReadOnlyStatusSummary.Text = historyList.First().entry;
                    txtProjectSummary.Text = historyList.First().entry;
                }


            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            

        }

        #region Project Summary
        protected void btShowAddProjectSummary_Click(object sender, EventArgs e)
        {
            trAddSummaryBox.Visible = true;
            trShowLatestSummary.Visible = false;
        }

        protected void btAddProjectSummary_Click1(object sender, EventArgs e)
        {
            if (txtProjectSummary.Text.Trim() != string.Empty)
            {
                pmmItem[DatabaseObjects.Columns.ProjectSummaryNote] = UGITUtility.GetVersionString(User.UserName, htmlEditor.Html.Trim(), pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                TicketObj.CommitChanges(pmmItem);   //pmmItem.UpdateOverwriteVersion();
                htmlEditor.Html = string.Empty;

                trAddSummaryBox.Visible = false;
                trShowLatestSummary.Visible = true;
            }
        }

        protected void btCancelAddSummary_Click(object sender, EventArgs e)
        {
            trAddSummaryBox.Visible = false;
            trShowLatestSummary.Visible = true;
        }
        #endregion
    }
}