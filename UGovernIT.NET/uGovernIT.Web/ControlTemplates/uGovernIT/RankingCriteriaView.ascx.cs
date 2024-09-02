using DevExpress.Data;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Data;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class RankingCriteriaView : System.Web.UI.UserControl
    {
        public string ticketID { get; set; }
        public Decimal Ranking = 0;
        public Decimal WeightScore = 0;
        public Decimal Weight = 0;
        public string RCT;
        public string FrameId;
        public bool ReadOnly;
        public string ControlId { get; set; }
        //DataRow spTicket = null;
        public string ModuleName { get; set; }
        public string absoluteUrlImportCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&module={3}&isdlg=1&isudlg=1";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LeadRanking leadRanking = null;
        LeadRankingManager leadRankingManager = null;
        RankingCriteriaManager RankingCriteriaManager = null;
        public string absoluteUrlCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&module={3}&isdlg=1&isudlg=1";

        protected override void OnInit(EventArgs e)
        {
            absoluteUrlImportCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImportCheckList, "importrankingcriteria", "Import Ranking Criteria Template", ticketID, ModuleName));
            absoluteUrlCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckList, "leadrankingaddedit", "Edit Ranking Criteria", ticketID, ModuleName));

            leadRanking = new LeadRanking();
            leadRankingManager = new LeadRankingManager(context);
            RankingCriteriaManager = new RankingCriteriaManager(context);

            if ((Request["ticketId"]) != null)
            {
                ModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(Request["ticketId"]));
            }            
        }

        protected void txtRanking_Init(object sender, EventArgs e)
        {
            ASPxTextBox textBox = sender as ASPxTextBox;
            GridViewDataItemTemplateContainer container = textBox.NamingContainer as GridViewDataItemTemplateContainer;

            // textBox.ClientSideEvents.TextChanged = String.Format("function(s, e) {{ TextBox_TextChanged(s, {0}, e); }}", container.VisibleIndex);
            textBox.ClientSideEvents.LostFocus = String.Format("function(s, e) {{ SetScore(s, {0}, e); }}", container.VisibleIndex);
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            long Id = 0;
            foreach (var args in e.UpdateValues)
            {
                Id = Convert.ToInt64(args.Keys["ID"]);
                if (Id > 0)
                {
                    LeadRanking leadRanking = new LeadRanking();
                    leadRanking = leadRankingManager.LoadByID(Id);
                    leadRanking.Ranking = Convert.ToInt32(args.NewValues["Ranking"]);
                    leadRanking.WeightedScore = Convert.ToDecimal(args.NewValues["WeightedScore"]);
                    leadRankingManager.Update(leadRanking);                    
                }
            }

            e.Handled = true;
            grdRankingCriteria.CancelEdit();
            BindGridView();
            ResetLeadPriority(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            List<LeadRanking> lstleadRanking = leadRankingManager.Load(x => x.TicketId == ticketID);
            if (lstleadRanking.Count == 0)
            {
                //LinkSaveBtn.Visible = false;     
                List<RankingCriterias> lstrankingCriterias = new List<RankingCriterias>();
                lstrankingCriterias = RankingCriteriaManager.Load(x => x.Deleted == false);

                foreach (RankingCriterias rankingCriterias in lstrankingCriterias)
                {
                    leadRanking = new LeadRanking();
                    leadRanking.Ranking = rankingCriterias.Ranking;
                    leadRanking.RankingCriteria = rankingCriterias.RankingCriteria;
                    leadRanking.Weight = rankingCriterias.Weight;
                    leadRanking.WeightedScore = rankingCriterias.WeightedScore;
                    leadRanking.Description = rankingCriterias.Description;
                    leadRanking.TicketId = ticketID;
                    leadRankingManager.Insert(leadRanking);
                }
                lstleadRanking = leadRankingManager.Load(x => x.TicketId == ticketID);
                grdRankingCriteria.Visible = true;
            }
            else
            {
                //LinkSaveBtn.Visible = true;
                grdRankingCriteria.Visible = true;
            }

            if (lstleadRanking != null && lstleadRanking.Count != 0)
            {
                Ranking = 0;
                Weight = 0;
                WeightScore = 0;
                foreach (LeadRanking LR in lstleadRanking)
                {
                    Ranking = LR.Ranking + Ranking;
                    Weight = LR.Weight + Weight;
                    WeightScore = LR.WeightedScore + WeightScore;
                }

                RCT = (Math.Round((WeightScore / lstleadRanking.Count), 1)).ToString();

                grdRankingCriteria.DataSource = lstleadRanking;
                grdRankingCriteria.DataBind();

                //ResetLeadPriority();
            }

        }

        protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Ticket TicketRequest = new Ticket(context, ModuleName);
            DataRow dr = Ticket.GetCurrentTicket(context, ModuleName, ticketID);
            if(dr[DatabaseObjects.Columns.RankingCriteriaTotal] == DBNull.Value || Convert.ToString(dr[DatabaseObjects.Columns.RankingCriteriaTotal]) == "0")
                ResetLeadPriority(true);
        }

        protected void LinkSaveBtn_Click(object sender, EventArgs e)
        {
            leadRankingManager = new LeadRankingManager(context);
            long ID = 0;
            Int32 Ranking = 0;
            for (int i = 0; i < grdRankingCriteria.VisibleRowCount; i++)
            {
                ID = Convert.ToInt64(grdRankingCriteria.GetRowValues(i, "ID"));
                string RankingCriteria = Convert.ToString(grdRankingCriteria.GetRowValues(i, "RankingCriteria"));
                string Description = Convert.ToString(grdRankingCriteria.GetRowValues(i, "Description"));

                ASPxComboBox cb = grdRankingCriteria.FindRowCellTemplateControl(i, null, "cbRanking") as ASPxComboBox;
                Ranking = Convert.ToInt32(cb.SelectedItem.Value);
                Decimal Weight = Convert.ToDecimal(grdRankingCriteria.GetRowValues(i, "Weight"));
                Decimal WeightedScore = Convert.ToDecimal(grdRankingCriteria.GetRowValues(i, "WeightedScore"));

                leadRanking = leadRankingManager.LoadByID(ID);
                leadRanking.RankingCriteria = RankingCriteria;
                leadRanking.Description = Description;
                leadRanking.Ranking = Ranking;
                leadRanking.Weight = Weight;
                leadRanking.WeightedScore = WeightedScore;
                leadRanking.WeightedScore = Math.Round((leadRanking.Weight * leadRanking.Ranking), 1);
                leadRanking.TicketId = ticketID;
                leadRankingManager.Update(leadRanking);
            }
        }

        protected void grdRankingCriteria_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
        }

        protected void grdRankingCriteria_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                if (e.Item.FieldName == "WeightedScore")
                {
                    e.Text = RCT;
                }

                if (e.Item.FieldName == "Ranking")
                {
                    e.Text = Ranking.ToString();
                }

                if (e.Item.FieldName == "Weight")
                {
                    e.Text = Weight.ToString();
                }
            }
        }

        private void ResetLeadPriority(bool UpdateHistory = false)
        {
            Ticket TicketRequest = new Ticket(context, ModuleName);

            DataRow dr =  Ticket.GetCurrentTicket(context, ModuleName, ticketID);
            decimal TotalWeight = Convert.ToDecimal(RCT);

            string PreviousRankingCriteriaTotal = Convert.ToString(dr[DatabaseObjects.Columns.RankingCriteriaTotal]);
            string PreviousSuccessChance = dr[DatabaseObjects.Columns.SuccessChance] != DBNull.Value ? Convert.ToString(dr[DatabaseObjects.Columns.SuccessChance]) : "[No Value]";

            dr[DatabaseObjects.Columns.RankingCriteriaTotal] = TotalWeight;
            /*
            if (TotalWeight < 2)
                dr[DatabaseObjects.Columns.SuccessChance] = "Cold";
            else if (TotalWeight >= 2 && TotalWeight < 4)
                dr[DatabaseObjects.Columns.SuccessChance] = "Warm";
            else if (TotalWeight >= 4)
                dr[DatabaseObjects.Columns.SuccessChance] = "Hot";
            */

            LeadCriteriaManager leadCriteriaManager = new LeadCriteriaManager(context);
            List<LeadCriteria> lstCriteria = leadCriteriaManager.Load().Where(x => x.Deleted != true).ToList();

            foreach (var item in lstCriteria)
            {
                if (item.MinValue <= TotalWeight && item.MaxValue >= TotalWeight)
                {
                    dr[DatabaseObjects.Columns.SuccessChance] = item.Priority;
                    break;
                }
            }

            if (UpdateHistory == true)
            {
                uHelper.CreateHistory(HttpContext.Current.CurrentUser(), $"Lead priority ({PreviousSuccessChance} {PreviousRankingCriteriaTotal} => {dr[DatabaseObjects.Columns.SuccessChance]} {TotalWeight})", dr, context);
            }            

            TicketRequest.CommitChanges(dr);
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}