using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class LeadRankingAddEdit : System.Web.UI.UserControl
    {
        public string LeadRankingId { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }

        LeadRanking leadRanking = new LeadRanking();
        LeadRankingManager leadRankingManager = new LeadRankingManager(HttpContext.Current.GetManagerContext());
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindddlRanking();
            if (!IsPostBack)
            {
                long Id = Convert.ToInt64(LeadRankingId);
                if (Id > 0)
                {
                    leadRanking = leadRankingManager.LoadByID(Id);
                    txtDescription.Text = leadRanking.Description;

                    txtRankingCriteria.Text = leadRanking.RankingCriteria;
                    txtWeight.Text = leadRanking.Weight.ToString();
                    txtWeightScore.Text = leadRanking.WeightedScore.ToString();

                    ddlRanking.SelectedIndex = ddlRanking.Items.IndexOf(ddlRanking.Items.FindByText(leadRanking.Ranking.ToString()));
                }
            }
        }

        private void BindddlRanking()
        {
            try
            {
                List<Int32> listItem = new List<Int32>();

                listItem.Add(1);
                listItem.Add(2);
                listItem.Add(3);
                listItem.Add(4);
                listItem.Add(5);
                listItem.Sort();
                ddlRanking.DataSource = listItem.Distinct();
                ddlRanking.DataBind();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        protected void btnSaveRankingCriteria_Click(object sender, EventArgs e)
        {
            long Id = Convert.ToInt64(LeadRankingId);
            if (Id > 0)
            {
                leadRanking = leadRankingManager.LoadByID(Id);
                leadRanking.RankingCriteria = txtRankingCriteria.Text;
                leadRanking.Ranking = Convert.ToInt32(ddlRanking.SelectedItem.Value.ToString());
                leadRanking.Description = txtDescription.Text;
                leadRanking.Weight = Convert.ToDecimal(txtWeight.Text);
                leadRanking.WeightedScore = Math.Round((leadRanking.Weight * leadRanking.Ranking), 1);
                leadRankingManager.Update(leadRanking);



                DataTable CurrentTicket = null;
                string tableName = DatabaseObjects.Tables.Lead;
                string query2 = string.Format("{0}='{1}' ",
                     DatabaseObjects.Columns.TicketId, TicketID);
               
                ModuleName = uHelper.getModuleNameByTicketId(Request["ticketId"]);
                if(ModuleName == "LEM")
                {
                     CurrentTicket = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Lead, $"{query2.ToString()} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);
                    tableName = DatabaseObjects.Tables.Lead;
                }
                else if (ModuleName == "OPM")
                {
                    CurrentTicket = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $"{query2.ToString()} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);
                    tableName = DatabaseObjects.Tables.Opportunity;
                }
                //Decimal Ranking = 0;
                //Decimal Weight = 0;
                Decimal RCT = 0;
                Decimal WeightScore = 0;
                if (CurrentTicket != null)
                {
                    long DbTicketId = Convert.ToInt64(CurrentTicket.Rows[0]["ID"].ToString());

                    List<LeadRanking> lstleadRanking = leadRankingManager.Load(x => x.TicketId == TicketID);
                    if (lstleadRanking != null && lstleadRanking.Count != 0)
                    {

                        //foreach (LeadRanking LR in lstleadRanking)
                        //{
                        //    Ranking = LR.Ranking + Ranking;
                        //    Weight = LR.Weight + Weight;
                        //}
                        //RCT = (Math.Round((Ranking / Weight), 1));

                        foreach (LeadRanking LR in lstleadRanking)
                        {
                           
                            WeightScore = LR.WeightedScore + WeightScore;
                        }
                        RCT = (Math.Round((WeightScore / lstleadRanking.Count), 1));

                    }
                    Dictionary<String, object> values = new Dictionary<string, object>();
                    values.Add(DatabaseObjects.Columns.RankingCriteriaTotal, RCT.ToString());

                    int success = (int)GetTableDataManager.UpdateItem<int>(tableName, Convert.ToInt64(DbTicketId), values);

                }

            }
            else
            {
                //RankingCriterias = new RankingCriterias();
                //RankingCriterias.RankingCriteria = txtRankingCriteria.Text;
                //RankingCriterias.Ranking = Convert.ToInt32(ddlRanking.SelectedItem.Value.ToString());
                //RankingCriterias.Description = txtDescription.Text;
                //RankingCriterias.Weight = Convert.ToDecimal(txtWeight.Text);
                //RankingCriterias.WeightedScore = Math.Round((RankingCriterias.Weight * RankingCriterias.Ranking), 1);
                //RankingCriteriaManager.Update(RankingCriterias);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}