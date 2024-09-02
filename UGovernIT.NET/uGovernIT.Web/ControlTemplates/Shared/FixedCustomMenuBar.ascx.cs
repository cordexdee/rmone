using AutoMapper;
using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class FixedCustomMenuBar : UserControl
    {
        private ApplicationContext _context = null;
        private LandingPagesManager _LandingPagesManager = null;
        protected bool IsTrailClient = false;
        protected string newTask = UGITUtility.GetAbsoluteURL("/Pages/HomeTasks");
        protected string userHomePage = UGITUtility.GetAbsoluteURL("/Pages/UserHomePage");
        protected bool IsTrailAdminUser = false;
        protected string myProject = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=myproject");
        protected string globalLandingUserHomePage = string.Empty;

        protected string NewTask = UGITUtility.GetAbsoluteURL("~/Admin/NewAdminUI.aspx");

        public string backgroundStaus = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=runInBackgroundServicesView");
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

        protected LandingPagesManager LandingPagesManager
        {
            get
            {
                if (_LandingPagesManager == null)
                {
                    _LandingPagesManager = new LandingPagesManager(ApplicationContext);
                }
                return _LandingPagesManager;
            }
        }
        UserProfileManager userProfileManager;
        public bool isMasterTenant = false;

        public string MenuType { get; set; }
        MenuNavigationCollection eData;
        protected void Page_Load(object sender, EventArgs e)
        {

            #region menu binding from admin side
            var User = HttpContext.Current.CurrentUser();
            userProfileManager = new UserProfileManager(ApplicationContext);
            var pageConfig = this.Page.PageConfig();
            if (pageConfig != null)
            {
                MenuType = pageConfig.LeftMenuName;
                if (pageConfig.HideLeftMenu)
                {
                    menuNewPresentation.Visible = false;
                }
            }
            var menuNavigationHelper = new MenuNavigationManager(HttpContext.Current.GetManagerContext());
            List<MenuNavigation> menu = menuNavigationHelper.LoadMenuNavigation(MenuType).Where(x => x.IsDisabled == false).ToList();

            #region filter data authorize to view and disabled menu
            List<MenuNavigation> menuToRemove = new List<MenuNavigation>();
            menuToRemove = menu.Where(x => x.IsDisabled == true || (!string.IsNullOrEmpty(x.AuthorizedToView) && ((UGITUtility.ConvertStringToList(x.AuthorizedToView, Constants.Separator6)).Contains(User != null ? User.Id : "") == false) && userProfileManager.IsUserinGroups(User != null ? User.Id : "", (UGITUtility.ConvertStringToList(x.AuthorizedToView, Constants.Separator6))) == false)).ToList();

            foreach (MenuNavigation mn in menuToRemove)
            {
                if (mn.MenuParentLookup != 0)
                {
                    MenuNavigation parent = mn.Parent;
                    menu.Where(x => x.ID == parent.ID).SingleOrDefault().Children.Remove(mn);
                }
                else
                {
                    menu.Remove(mn);
                }
            }
            #endregion
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuNavigation, MenuNavigationItem>();
            });
            var mapper = config.CreateMapper();
            List<MenuNavigationItem> menuList = mapper.Map<List<MenuNavigationItem>>(menu);
            List<MenuNavigationItem> rootmenu = menuList == null ? null : menuList.Where(x => x.MenuParentLookup == 0).OrderBy(x => x.ItemOrder).ToList();
            eData = new MenuNavigationCollection();
            eData.AddRange(rootmenu);
            if (eData != null && eData.Count != 0)
            {
                menuNewPresentation.DataSource = eData;
                menuNewPresentation.DataBind();
            }

            #endregion

        }


        protected void anchorRootMenuItem_Load(object sender, EventArgs e)
        {
            HyperLink anchorRootMenuItem = sender as HyperLink;
            DevExpress.Web.MenuItemTemplateContainer container = anchorRootMenuItem.NamingContainer as DevExpress.Web.MenuItemTemplateContainer;


            if (container.Item.DataItem == null)
                return;
            MenuNavigationItem mItem = container.Item.DataItem as MenuNavigationItem;
            #region BindNavigation Url
            if (!string.IsNullOrEmpty(mItem.NavigationType))
            {
                if (anchorRootMenuItem != null)
                {
                    if (string.IsNullOrEmpty(mItem.IconUrl))
                    {
                        HtmlGenericControl lblDiv = new HtmlGenericControl("Div");
                        lblDiv.Attributes.Add("class", "fixedMenu-itemlabel");
                        anchorRootMenuItem.Controls.Add(lblDiv);
                        anchorRootMenuItem.ToolTip = mItem.Title; ;
                        string collapseNoIconTitle = string.IsNullOrEmpty(mItem.Title) ? "M" : mItem.Title.Substring(0, 1).ToUpper();
                        lblDiv.Attributes.Add("title", mItem.Title);
                        lblDiv.InnerHtml = collapseNoIconTitle;
                        anchorRootMenuItem.NavigateUrl = mItem.NavigationUrl;
                    }
                    anchorRootMenuItem.NavigateUrl = mItem.NavigationUrl;
                    if (mItem.NavigationType == "Modal")
                    {
                        anchorRootMenuItem.NavigateUrl = "javascript:showSubMenuPopUp('" + UGITUtility.GetAbsoluteURL(mItem.NavigationUrl) + "','" + UGITUtility.ReplaceInvalidCharsInURL(mItem.Title) + "')";
                    }
                    else if (mItem.NavigationType == "Modeless")
                        anchorRootMenuItem.Target = "_blank";

                }
            }
            #endregion

            #region Configure Title Configurations
            HtmlGenericControl lblTitleDiv = new HtmlGenericControl("Div");
            HtmlGenericControl imgIconDiv = new HtmlGenericControl("Div");
            
            if (!string.IsNullOrWhiteSpace(mItem.IconUrl))
            {
                anchorRootMenuItem.Controls.Add(imgIconDiv);
                Image imgIcon = new Image();
                
                if (!string.IsNullOrEmpty(mItem.MenuWidth))
                    imgIcon.ImageUrl = UGITUtility.GetAbsoluteURL(mItem.IconUrl);
                
                if(mItem.Parent == null)
                    imgIcon.Attributes.Add("title", mItem.Title);

                imgIcon.Width = Unit.Pixel(25);
                imgIconDiv.Controls.Add(imgIcon);
                imgIconDiv.Attributes.Add("class", "fleft");
            }

            // Add title to the Sub-Menu items
            if (mItem != null && mItem.Parent != null) 
            {
                anchorRootMenuItem.Controls.Add(lblTitleDiv);
                anchorRootMenuItem.ToolTip = mItem.Title.Trim();
                Label lblTitle = new Label();
                lblTitle.Text = mItem.Title.Trim();

                lblTitleDiv.Controls.Add(lblTitle);
                lblTitleDiv.Attributes.Add("style", "padding-top:7px; vertical-align: middle;");
                lblTitle.Attributes.Add("style", "display:inline-block; text-align:center; color:white; padding-left:2px");
            }
            #endregion
        }


        protected void anchorGroopmenuitem_Load(object sender, EventArgs e)
        {

        }

        protected void menuNewPresentation_ItemDataBound(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            ASPxMenu aSPxMenu = (ASPxMenu)source;
            if (e.Item.DataItem != null)
            {
                DevExpress.Web.MenuItem menuItem = e.Item as DevExpress.Web.MenuItem;
                MenuNavigationItem mItem = e.Item.DataItem as MenuNavigationItem;
                //menuItem.NavigateUrl = mItem.NavigationUrl;
                //if (mItem.Title.Equals("Dashboards & Reports",StringComparison.CurrentCultureIgnoreCase))
                //{
                //    menuItem.NavigateUrl = "javascript:UgitOpenPopupDialog('" + $"{System.Configuration.ConfigurationManager.AppSettings["SiteUrl"]}/report/buildreport.aspx?reportName=companysalesreport&userId={HttpContext.Current.CurrentUser().Id}" + "', '', 'Company Sales Report', 90, 95)";
                //}

                //menuItem.Image.Url = mItem.IconUrl;
                menuItem.SubMenuItemStyle.CssClass = "fixedMenu-subMenuItem";
                //menuItem.SubMenuItemImage.Width = Unit.Pixel(22);
                //menuItem.Image.Width = Unit.Pixel(25);
                menuItem.Image.ToolTip = mItem.Title;

            }

        }
  
    }
}