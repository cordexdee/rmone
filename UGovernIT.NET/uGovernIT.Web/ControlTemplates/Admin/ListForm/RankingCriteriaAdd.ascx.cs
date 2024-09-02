using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class RankingCriteriaAdd : System.Web.UI.UserControl
    {
        public string RankingCriteriaId { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        RankingCriterias RankingCriterias = new RankingCriterias();
        RankingCriteriaManager RankingCriteriaManager = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RankingCriteriaManager = new RankingCriteriaManager(context);

            BindddlRanking();
            if (!IsPostBack)
            {
                long Id = Convert.ToInt64(RankingCriteriaId);
                if (Id > 0)
                {
                    //btnDelete.Visible = true;
                    RankingCriterias = RankingCriteriaManager.LoadByID(Id);
                    txtDescription.Text = RankingCriterias.Description;

                    txtRankingCriteria.Text = RankingCriterias.RankingCriteria;
                    txtWeight.Text = RankingCriterias.Weight.ToString();
                    txtWeightScore.Text = RankingCriterias.WeightedScore.ToString();
                    ddlRanking.SelectedIndex = ddlRanking.Items.IndexOf(ddlRanking.Items.FindByText(RankingCriterias.Ranking.ToString()));
                    chkDeleted.Checked = RankingCriterias.Deleted;
                }
                else
                {
                    //btnDelete.Visible = false;
                    ddlRanking.SelectedIndex = ddlRanking.Items.IndexOf(ddlRanking.Items.FindByText("1"));
                }
            }
        }

        private void BindddlRanking()
        {            
            List<int> listItem = new List<int>();
            listItem.AddRange(Enumerable.Range(1, 5));
            ddlRanking.DataSource = listItem;
            ddlRanking.DataBind();           
        }

        protected void btnSaveRankingCriteria_Click(object sender, EventArgs e)
        {
            long Id = Convert.ToInt64(RankingCriteriaId);
            if (Id > 0)
            {
                RankingCriterias = RankingCriteriaManager.LoadByID(Id);
                RankingCriterias.RankingCriteria = txtRankingCriteria.Text;
                RankingCriterias.Ranking = Convert.ToInt32(ddlRanking.SelectedItem.Value.ToString());
                RankingCriterias.Description = txtDescription.Text;
                RankingCriterias.Weight = Convert.ToDecimal(txtWeight.Text);
                RankingCriterias.WeightedScore = Math.Round((RankingCriterias.Weight * RankingCriterias.Ranking), 1);
                RankingCriterias.Deleted = chkDeleted.Checked;
               // var st = String.Format("{0:0.0}", (RankingCriterias.Weight * RankingCriterias.Ranking));
                RankingCriteriaManager.Update(RankingCriterias);
            }
            else
            {
                RankingCriterias = new RankingCriterias();
                RankingCriterias.RankingCriteria = txtRankingCriteria.Text;
                RankingCriterias.Ranking = Convert.ToInt32(ddlRanking.SelectedItem.Value.ToString());
                RankingCriterias.Description = txtDescription.Text;
                RankingCriterias.Weight = Convert.ToDecimal(txtWeight.Text);
                RankingCriterias.WeightedScore = Math.Round((RankingCriterias.Weight * RankingCriterias.Ranking), 1);
                RankingCriterias.Deleted = chkDeleted.Checked;
                RankingCriteriaManager.Insert(RankingCriterias);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }


        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    long Id = Convert.ToInt64(RankingCriteriaId);
        //    if (Id > 0)
        //    {
        //        RankingCriterias = RankingCriteriaManager.LoadByID(Id);
        //        RankingCriteriaManager.Delete(RankingCriterias);
        //    }
        //    uHelper.ClosePopUpAndEndResponse(Context, true);
        //}
                
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}