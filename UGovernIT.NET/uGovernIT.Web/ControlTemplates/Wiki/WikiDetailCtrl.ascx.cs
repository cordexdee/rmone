using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Web.Controllers;

namespace uGovernIT.Web.ControlTemplates.Wiki
{
    public partial class WikiDetailCtrl : System.Web.UI.UserControl
    {
        public string TicketId { get; set; }
        public long? wikiLikesCount { get; set; }
        public long? wikiDislikesCount { get; set; }
        WikiReviewsManager reviewsManager = new WikiReviewsManager(HttpContext.Current.GetManagerContext());
        WikiArticlesController WikiArticlesController = new WikiArticlesController();
        WikiArticlesManager WikiArticlesManager = new WikiArticlesManager(HttpContext.Current.GetManagerContext());
        public string AssemblyVersion = string.Empty;
        protected bool IsExpandBottomContainer;
        protected bool IsExpandLeftContainer;
        public void Page_Load(object sender, EventArgs e)
        {
            ASPxSplitterWiki.GetPaneByName("wikiLinksContainer").Collapsed = true;
            ASPxSplitterWiki.GetPaneByName("bottomContainer").Collapsed = true;
            BindLinks();
            if (!string.IsNullOrEmpty(Request["Width"]))
            {
                string width = Request["Width"];
                int iWidth = 0;
                if (width.Contains("px"))
                {
                    width = width.Substring(0, width.IndexOf("px"));
                }

                iWidth = Convert.ToInt32(Convert.ToDouble(width));
                iWidth = iWidth - 25;
                width = iWidth + "px";
                tabServiceDetails.Style.Add("width", width);
            }

            double height = UGITUtility.StringToDouble(Request["height"]);
            if (height <= 0)
                height = 400;
            else
                height = height - 100;
            ASPxSplitterWiki.Height = new Unit(height);
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            List<WikiReviews> WikiReviews = new List<WikiReviews>();
            //string getQuery = "where WikiTicketID = 'WIKI-19-000003'";
            //string getQuery = "where TicketId = '" + TicketId + "' ";
            string getQuery = $"where TicketId = '{TicketId}' and {DatabaseObjects.Columns.TenantID}= '{HttpContext.Current.GetManagerContext().TenantID}'";


            //WikiDiscussions = wikiDiscussionManager.Load(query);
            //WikiLinks = wikiLinksManager.Load(query);

            WikiReviews = reviewsManager.Load(getQuery);
            //check
            var article = WikiArticlesManager.Get(getQuery);

            if (article != null)
            {
                if (article.WikiLikesCount == null)
                    wikiLikesCount = 0;
                else
                    wikiLikesCount = article.WikiLikesCount;
                if (article.WikiDislikesCount == null)
                    wikiDislikesCount = 0;
                else
                wikiDislikesCount = article.WikiDislikesCount;
            }
            else
            {
                wikiLikesCount = 0;
                wikiDislikesCount = 0;
            }

            if (WikiReviews != null && WikiReviews.Count > 0)
            {
                foreach (var item in WikiReviews)
                {
                    if (item.ReviewStatus == ReviewStatus.likeWiki && item.ReviewType == ReviewType.Like)
                    {
                        //case "liked":
                        imgDislikebtn.ImageUrl = "/content/Images/reject.png";
                        imgLikebtn.ImageUrl = "/content/Images/approve.png";
                        imgDislikebtn.Attributes["isdisliked"] = "false";
                        imgLikebtn.Attributes["isliked"] = "true";
                    }

                    else if (item.ReviewStatus == ReviewStatus.likeWiki && item.ReviewType == ReviewType.Dislike)
                    {

                        // case "disliked":
                        imgDislikebtn.ImageUrl = "/content/Images/reject.png";
                        imgLikebtn.ImageUrl = "/content/Images/approve.png";
                        imgDislikebtn.Attributes["isdisliked"] = "true";
                        imgLikebtn.Attributes["isliked"] = "false";
                        //break;


                    }
                    else if (item.ReviewStatus == ReviewStatus.FavoriteWiki && item.ReviewType == ReviewType.Favorite)

                    {

                        //case "favorite":
                        imgFavorite.ImageUrl = "/content/ButtonImages/Favorite.png";
                        imgFavorite.Attributes["isfavorite"] = "true";
                       // break;

                    }
                    else if ((item.ReviewStatus == ReviewStatus.FavoriteWiki && item.ReviewType == ReviewType.UnFavorite))

                    {
                        // case "unfavorite":
                        imgFavorite.ImageUrl = "/content/ButtonImages/UnFavorite.png";
                        imgFavorite.Attributes["isfavorite"] = "false";
                        //break;

                    }
                }
            }


            //else
            //{
            //    imgLikebtn.ImageUrl = "/content/ButtonImages/thumbs_up_gray.png";
            //    imgDislikebtn.ImageUrl = "/content/ButtonImages/thumbs_down_gray.png";
            //    imgFavorite.ImageUrl = "/content/ButtonImages/UnFavorite.png";
            //}

        }

        protected void imgLikebtn_Click(object sender, EventArgs e)
        {
            imgDislikebtn.ImageUrl = "/content/Images/reject.png";
            imgLikebtn.ImageUrl = "/content/Images/approve.png";
            imgDislikebtn.Attributes["isdisliked"] = "false";
            imgLikebtn.Attributes["isliked"] = "true";
            wikiLikesCount++;
            _ = WikiArticlesController.LikeWikiArticle(TicketId);
            //WikiArticlesController.updateWikiReviews(TicketId, ReviewType.Like, ReviewStatus.likeWiki);
        }

        protected void imgDislikebtn_Click(object sender, EventArgs e)
        {

            imgDislikebtn.ImageUrl = "/content/Images/reject.png";
            imgLikebtn.ImageUrl = "/content/Images/approve.png";
            imgDislikebtn.Attributes["isdisliked"] = "true";
            imgLikebtn.Attributes["isliked"] = "false";
            wikiDislikesCount++;
            _ = WikiArticlesController.DisLikeWikiArticle(TicketId);
            //WikiArticlesController.updateWikiReviews(TicketId, ReviewType.Dislike, ReviewStatus.likeWiki);
        }

        protected void imgFavorite_Click(object sender, EventArgs e)
        {
            if (imgFavorite.Attributes["isfavorite"] == "false")
            {
                imgFavorite.ImageUrl = "/content/ButtonImages/Favorite.png";
                //WikiArticlesController.FavoriteWikiArticle(TicketId, ReviewType.Favorite);
                WikiArticlesController.updateWikiReviews(TicketId, ReviewType.Favorite, ReviewStatus.FavoriteWiki);
                imgFavorite.Attributes["isfavorite"] = "true";
            }
            else
            {
                imgFavorite.ImageUrl = "/content/ButtonImages/UnFavorite.png";
                //WikiArticlesController.FavoriteWikiArticle(TicketId, ReviewType.UnFavorite);
                WikiArticlesController.updateWikiReviews(TicketId, ReviewType.UnFavorite, ReviewStatus.FavoriteWiki);
                imgFavorite.Attributes["isfavorite"] = "false";

            }
        }
        private void BindLinks()
        {
            WikiLink wikiLinks = (WikiLink)Page.LoadControl("~/CONTROLTEMPLATES/WIKI/WikiLink.ascx");
            wikiLinks.TicketId = TicketId;
            wikiLinks.IsHeader = true;
            // wikiLinks.width = "205px";
            pnlWikiLinks.Controls.Add(wikiLinks);

        }
    }
}