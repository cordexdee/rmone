using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class RankingCriteriaViewAdmin : UserControl
    {
        public string RCT;
        public Decimal Ranking = 0;
        public Decimal WeightScore = 0;
        public Decimal Weight = 0;
        RankingCriteriaManager RankingCriteriaManager = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public string absoluteUrlCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";
        protected override void OnInit(EventArgs e)
        {
            absoluteUrlCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckList, "addrankingcriteria", "Add Ranking Criteria"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RankingCriteriaManager = new RankingCriteriaManager(context);
            BindGridView();
        }

        private void BindGridView(bool includeDeleted = false)
        {
            List<RankingCriterias> RankingCriteriaManagerlist = new List<RankingCriterias>();

            if (includeDeleted)
            {
                RankingCriteriaManagerlist = RankingCriteriaManager.Load();
            }
            else
            {
                RankingCriteriaManagerlist = RankingCriteriaManager.Load().Where(x => x.Deleted != true).ToList();
            }

            if (RankingCriteriaManagerlist != null && RankingCriteriaManagerlist.Count != 0)
            {
                foreach (RankingCriterias LR in RankingCriteriaManagerlist)
                {
                    Ranking = LR.Ranking + Ranking;
                    Weight = LR.Weight + Weight;
                    WeightScore = LR.WeightedScore + WeightScore;
                }
                RCT = (Math.Round((WeightScore / RankingCriteriaManagerlist.Count), 1)).ToString();

                grdRankingCriteria.DataSource = RankingCriteriaManagerlist;
                grdRankingCriteria.DataBind();
            }
        }

       
        protected void grdRankingCriteria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //DataTable dtPrioriyFilter = null;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int index = 0;
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    if (index == 0)
                    {
                        GridView grd = (GridView)sender;
                        Label lblCheckListName = (Label)grd.Parent.FindControl("lblCheckListName");
                        HiddenField hdnCheckListId = (HiddenField)grd.Parent.FindControl("hdnCheckListId");

                        string divHtml = String.Format("<a class='divcell_0' href='javascript:void();' onclick='OpenEditCheckListPopup({2})' title='{1}'>{0}</a>", UGITUtility.TruncateWithEllipsis(lblCheckListName.Text, 25), lblCheckListName.Text, hdnCheckListId.Value);
                        cell.Text = divHtml;
                        cell.CssClass = "cell_0_0";

                    }
                    else if (index == e.Row.Cells.Count - 1)
                    {
                        cell.CssClass = "addiconheader";
                        cell.Text = Context.Server.HtmlDecode(cell.Text);
                    }
                    else
                    {
                        cell.CssClass = "header";
                        cell.Text = Context.Server.HtmlDecode(cell.Text);
                    }
                    index++;
                }
            }

           
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

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            BindGridView(chkShowDeleted.Checked);
        }
    }
}