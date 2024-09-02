using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class WikiPermissionControl : UserControl
    {
        public string TicketId { get; set; }
        WikiArticles article = new WikiArticles();

        WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(HttpContext.Current.GetManagerContext());

        protected void Page_Load(object sender, EventArgs e)
        {
            string getQuery = "where TicketId = '" +TicketId +"' ";

            article = wikiArticlesManager.Get(getQuery);

            if (!IsPostBack)
            {
                FillDetails();
            }
        }


        protected void FillDetails()
        {
            //SPFieldUserValueCollection UserGroupsColl = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(splistCollectionitem[0][DatabaseObjects.Columns.AuthorizedToView]));
            //ppeWikiUser.UpdateEntities(uHelper.getUsersListFromCollection(UserGroupsColl));
            ppeWikiUser.SetValues(Convert.ToString(article.AuthorizedToView));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ///User Groups For MultiUser
          //  SPFieldUserValueCollection UserGroupCollection = new SPFieldUserValueCollection();
            //UserGroupCollection = ppeWikiUser.GetUserValueCollection();
            //listItem[DatabaseObjects.Columns.AuthorizedToView] = UserGroupCollection;
            article.AuthorizedToView = ppeWikiUser.GetValues();
            wikiArticlesManager.Update(article);
           // listItem.Update();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }


    }
}