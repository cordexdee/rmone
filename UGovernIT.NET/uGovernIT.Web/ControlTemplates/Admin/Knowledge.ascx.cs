using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility.Entities.DB;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class Knowledge : System.Web.UI.UserControl
    {
        private ApplicationContext _context = null;
        public string documenttype = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=DocumentTypeView"));
        public string doctypeinfo = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=todo"));
        protected string wikiArticle = UGITUtility.GetAbsoluteURL("/pages/wikiarticles");
        UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        ConfigurationVariableManager configurationVariableManager = null;
        private WikiArticlesManager wikiArticlesManager = null;
        public  bool permission = false;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSlider.DataSource = Catagories();
            ImageSlider.DataBind();

            ASPxImageSlider1.DataSource = Documents();
            ASPxImageSlider1.DataBind();

            ASPxImageSlider2.DataSource = Wikis();
            ASPxImageSlider2.DataBind();

            permission = GetPermissionsForWiki("add", "");
        }

        public DataTable Catagories()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));




            DataRow dr = dt.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";
            dt.Rows.Add(dr3);
            return dt;
        }

        public DataTable Documents()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));




            DataRow dr = dt.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";
            dt.Rows.Add(dr3);
            return dt;
        }

        public DataTable Wikis()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Image", typeof(string));
            dt.Columns.Add("Link", typeof(string));
            dt.Columns.Add("HrClass", typeof(string));




            DataRow dr = dt.NewRow();
            dr["Text"] = "Add";
            dr["Image"] = "/Content/Images/plus-blue.png";
            dr["Link"] = "";
            dt.Rows.Add(dr);



            DataRow dr2 = dt.NewRow();
            dr2["Text"] = "Edit";
            dr2["Image"] = "/Content/Images/editNewIcon.png";
            dr2["Link"] = "";
            dt.Rows.Add(dr2);



            DataRow dr3 = dt.NewRow();
            dr3["Text"] = "Links";
            dr3["Image"] = "/Content/Images/linkBlue.png";
            dr3["Link"] = "";
            dr3["HrClass"] = "lastHr";
            dt.Rows.Add(dr3);
            return dt;
        }


        public bool GetPermissionsForWiki(string Action, string TicketId)
        {
            // Didn't find right permissions
            bool IsAuthorizedUser = false;
            bool AuthorizedToView = false;
            string getQuery = string.Empty;
            _context = HttpContext.Current.GetManagerContext();
            wikiArticlesManager = new WikiArticlesManager(_context);
            configurationVariableManager = new ConfigurationVariableManager(_context);
            if (!string.IsNullOrEmpty(TicketId))
            {
                getQuery = "where TicketId = '" + TicketId + "' ";

            }



            // If current user is super admin, just return true
            
            if (userManager.IsUGITSuperAdmin(_context.CurrentUser))
                AuthorizedToView = true;


            if (!string.IsNullOrEmpty(Action))
            {
                if (IsWikiOwner(_context.CurrentUser))
                    AuthorizedToView = true;

                if (Action == "add")
                {
                    string wikiCreators = configurationVariableManager.GetValue(ConfigConstants.WikiCreators);
                    // bool isCreator = userManager.CheckUserIsInGroup(wikiCreators, _applicationContext.CurrentUser);
                    if (wikiCreators == _context.CurrentUser.Id)
                        IsAuthorizedUser = true;
                    bool isCreator = userManager.CheckIfUserGroup(_context.CurrentUser.Id, wikiCreators);

                    if (isCreator == true || IsAuthorizedUser)
                        AuthorizedToView = true;
                }
                else // action == "edit-delete"
                {
                    WikiArticles wikiArticle = new WikiArticles();
                    wikiArticle.TicketId = TicketId;
                    // WikiArticleHelper wikiHelper = new WikiArticleHelper();
                    var article = wikiArticlesManager.Get(getQuery);
                    string WikiUserId = string.Empty;
                    if (article != null)
                    {
                        // SPFieldUserValueCollection spFieldAuthor = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(spColl[0][DatabaseObjects.Columns.Author]));
                        if (article.CreatedBy != null)
                            WikiUserId = article.CreatedBy;
                    }

                    if (WikiUserId == _context.CurrentUser.Id)
                        AuthorizedToView = true;

                }
            }


            return AuthorizedToView;
        }


        public bool IsWikiOwner(UserProfile user)
        {
            // ApplicationContext context = HttpContext.Current.GetManagerContext();
            string wikiOwners = configurationVariableManager.GetValue(ConfigConstants.WikiOwners);
            //string wikiOwners = ConfigurationVariable.GetValue(SPContext.Current.Web, ConfigConstants.WikiOwners);
            return userManager.CheckUserIsInGroup(wikiOwners, user);
        }

    }
}