using System;
using System.Web.UI;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CustomSideBarLink : UserControl
    {

        public DashboardSideProperty dbSideProperty;

        public CustomSideBarLink()
        {
            dbSideProperty = new DashboardSideProperty();
        }
        protected override void OnPreRender(EventArgs e)
        {

            //rDashboardGroup.DataSource = needToViewDashboards;
            //rDashboardGroup.DataBind();
            //string path = @"http://www.c-sharpcorner.com//App_Themes/csharp/images/SiteLogo.gif";
            //imageurl.ImageUrl =uHelper.GetAbsoluteURL(needToViewDashboards.Imapgepath);
            //imageurl.ImageUrl = path;
            imgTile.Visible = false;
            if (dbSideProperty.Imapgepath != null && dbSideProperty.Imapgepath.Trim() != string.Empty)
            {
                imgTile.ImageUrl = UGITUtility.GetAbsoluteURL(dbSideProperty.Imapgepath);
                imgTile.Visible = true;
            }

            previewPanel.Width = 132;

            linkTable.Height = string.Format("{0}px", dbSideProperty.Height > 30 ? dbSideProperty.Height : 30);


            lbTile.Visible = false;
            if (dbSideProperty.Title != null && dbSideProperty.Title.Trim() != string.Empty && (!dbSideProperty.IsHideTitle))
            {
                lbTile.Text = dbSideProperty.Title;
                lbTile.Visible = true;
            }

            if (!dbSideProperty.IsFlat)
            {
                imgTile.BorderWidth = 0;
            }

            string url = UGITUtility.GetAbsoluteURL(dbSideProperty.LinkUrl);
            if (dbSideProperty.NavigationType == 1)
            {
                hypTitleUrl.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog(\"{0}\",'','',90,90,0)", url);
            }
            else if (dbSideProperty.NavigationType == 2)
            {
                hypTitleUrl.NavigateUrl = string.Format("javascript:window.open(\"{0}\",'_blank')", url);
            }
            else
            {
                hypTitleUrl.NavigateUrl = url;
            }


            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
