using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class ImportRankingCriteria : System.Web.UI.UserControl
    {
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LeadRanking leadRanking = null;
        LeadRankingManager leadRankingManager = null;
        RankingCriteriaManager rankingCriteriaManager = null;
        LeadCriteriaManager leadCriteriaManager = null;
        //RankingCriterias rankingCriterias = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            leadCriteriaManager = new LeadCriteriaManager(context);
        }

        protected void btnImportRankingCriteria_Click(object sender, EventArgs e)
        {
            rankingCriteriaManager = new RankingCriteriaManager(context);
            leadRankingManager = new LeadRankingManager(context);
            List<LeadRanking> lstleadRanking = leadRankingManager.Load(x => x.TicketId == TicketID);
            if (lstleadRanking.Count == 0)
            {
                List<RankingCriterias> lstrankingCriterias = new List<RankingCriterias>();
                lstrankingCriterias = rankingCriteriaManager.Load(x => x.Deleted == false);

                foreach (RankingCriterias rankingCriterias in lstrankingCriterias)
                {
                    leadRanking = new LeadRanking();
                    leadRanking.Ranking = rankingCriterias.Ranking;
                    leadRanking.RankingCriteria = rankingCriterias.RankingCriteria;
                    leadRanking.Weight = rankingCriterias.Weight;
                    leadRanking.WeightedScore = rankingCriterias.WeightedScore;
                    leadRanking.Description = rankingCriterias.Description;
                    leadRanking.TicketId = TicketID;
                    leadRankingManager.Insert(leadRanking);
                }
            }
            else
            {
                leadRankingManager.Delete(lstleadRanking);

                List<RankingCriterias> lstrankingCriterias = new List<RankingCriterias>();
                lstrankingCriterias = rankingCriteriaManager.Load(x => x.Deleted == false);
                foreach (RankingCriterias rankingCriterias in lstrankingCriterias)
                {
                    leadRanking = new LeadRanking();
                    leadRanking.Ranking = rankingCriterias.Ranking;
                    leadRanking.RankingCriteria = rankingCriterias.RankingCriteria;
                    leadRanking.Weight = rankingCriterias.Weight;
                    leadRanking.WeightedScore = rankingCriterias.WeightedScore;
                    leadRanking.Description = rankingCriterias.Description;
                    leadRanking.TicketId = TicketID;
                    leadRankingManager.Insert(leadRanking);
                }
            }

            DataTable CurrentTicket = null;
            string tableName = string.Empty;
            tableName = DatabaseObjects.Tables.Lead;
            string query2 = string.Format("{0}='{1}' ",
                   DatabaseObjects.Columns.TicketId, TicketID);
            ModuleName = uHelper.getModuleNameByTicketId(Request["ticketId"]);
            if (ModuleName == "LEM")
            {
                CurrentTicket = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Lead, $"{query2} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);
                tableName = DatabaseObjects.Tables.Lead;
            }
            else if (ModuleName == "OPM")
            {
                CurrentTicket = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $"{query2} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);
                tableName = DatabaseObjects.Tables.Opportunity;
            }
            Decimal Ranking = 0;
            Decimal Weight = 0;
            Decimal RCT = 0;
            if (CurrentTicket != null)
            {
                long DbTicketId = Convert.ToInt64(CurrentTicket.Rows[0]["ID"].ToString());

                lstleadRanking = leadRankingManager.Load(x => x.TicketId == TicketID);
                if (lstleadRanking != null && lstleadRanking.Count != 0)
                {

                    foreach (LeadRanking LR in lstleadRanking)
                    {
                        Ranking = LR.Ranking + Ranking;
                        Weight = LR.Weight + Weight;
                    }
                    RCT = (Math.Round((Ranking / Weight), 1));

                }

                //List<LeadCriteria> lstCriteria = leadCriteriaManager.Load().Where(x => x.Deleted != true).ToList();
                List<LeadCriteria> lstCriteria = leadCriteriaManager.Load(x => x.Deleted != true).ToList();

                Dictionary<String, object> values = new Dictionary<string, object>();
                values.Add(DatabaseObjects.Columns.RankingCriteriaTotal, RCT.ToString());


                foreach (var item in lstCriteria)
                {
                    if (item.MinValue <= RCT && item.MaxValue >= RCT)
                    {                        
                        values.Add(DatabaseObjects.Columns.SuccessChance, item.Priority);
                        break;
                    }
                }

                int success = (int)GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.Lead, Convert.ToInt64(DbTicketId), values);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}