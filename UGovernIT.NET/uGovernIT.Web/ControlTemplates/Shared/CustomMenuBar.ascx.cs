using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Web.Helpers;
using AutoMapper;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class CustomMenuBar : System.Web.UI.UserControl
    {
        protected string filterPage;
        protected string logoUrl = string.Empty;
        public bool UgitHideGlobalSearch { get; set; }
        public bool HideTopMenu { get; set; }
        public bool HideFooter { get; set; }
        public string MenuType { get; set; }
        MenuNavigationCollection eData;
        int menuItemDefaultHeight = 50;
        int menuItemDefaultHeight_SideBySide = 20;
        private ApplicationContext _context = null;
        public string siteurl = "";// SPContext.Current.Web.Url;
        //bool isAdmin = false;// SPContext.Current.Web.CurrentUser.IsSiteAdmin;
                             // protected List<SPGroup> currentUserGroups = SPContext.Current.Web.CurrentUser.Groups.Cast<SPGroup>().ToList();
        int topMenuItemHeight = 0;
        bool isTopBottomAlignment = true;
        protected string menuHighlightColor;
        public UserProfile User = null;
        public UserProfileManager userProfileManager = null;
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
        protected override void OnInit(EventArgs e)
        {

            var str = Session["SelectedGroup"];
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var pageConfig = this.Page.PageConfig();
            if (pageConfig != null)
            {
                MenuType = pageConfig.LeftMenuName;
            }
            User = HttpContext.Current.CurrentUser();
            userProfileManager = new UserProfileManager(ApplicationContext);

            var menuNavigationHelper = new MenuNavigationManager(HttpContext.Current.GetManagerContext());
            List<MenuNavigation> menu = menuNavigationHelper.LoadMenuNavigation(MenuType).Where(x => x.IsDisabled == false).ToList();

            #region filter data authorize to view and disabled menu
            List<MenuNavigation> menuToRemove = new List<MenuNavigation>();
            menuToRemove = menu.Where(x => x.IsDisabled == true || 
            (!string.IsNullOrEmpty(x.AuthorizedToView) && 
            ((UGITUtility.ConvertStringToList(x.AuthorizedToView, Constants.Separator6)).Contains(User != null ? User.Id : "") == false) 
            && userProfileManager.IsUserinGroups(User != null ? User.Id : "", (UGITUtility.ConvertStringToList(x.AuthorizedToView, Constants.Separator6))) == false)).ToList();

            foreach (MenuNavigation mn in menuToRemove)
            {
                if (mn.MenuParentLookup != 0)
                {
                    MenuNavigation parent = mn.Parent;
                    menu.Where(x => x.ID == parent.ID).SingleOrDefault()?.Children.Remove(mn);
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

            //Set max height to all root menu 
            isTopBottomAlignment = eData.Any(x => ((x.MenuTextAlignment == MenuAllignmentType.TopLeft && x.MenuIconAlignment == MenuAllignmentType.BottomLeft)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomLeft && x.MenuIconAlignment == MenuAllignmentType.TopLeft)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopRight && x.MenuIconAlignment == MenuAllignmentType.BottomRight)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomRight && x.MenuIconAlignment == MenuAllignmentType.TopRight)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomRight && x.MenuIconAlignment == MenuAllignmentType.TopLeft)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopLeft && x.MenuIconAlignment == MenuAllignmentType.BottomRight)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomLeft && x.MenuIconAlignment == MenuAllignmentType.TopRight)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopRight && x.MenuIconAlignment == MenuAllignmentType.BottomLeft)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopCenter && x.MenuIconAlignment == MenuAllignmentType.BottomCenter)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopCenter && x.MenuIconAlignment == MenuAllignmentType.BottomLeft)
                 || (x.MenuTextAlignment == MenuAllignmentType.TopCenter && x.MenuIconAlignment == MenuAllignmentType.BottomRight)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomCenter && x.MenuIconAlignment == MenuAllignmentType.TopCenter)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomLeft && x.MenuIconAlignment == MenuAllignmentType.TopCenter)
                 || (x.MenuTextAlignment == MenuAllignmentType.BottomRight && x.MenuIconAlignment == MenuAllignmentType.TopCenter)
                 ));


            int menuItemHeight = 0;
            if (eData.Count > 0)
                menuItemHeight = eData.Max(y => UGITUtility.StringToInt(y.MenuHeight));
            if (menuItemHeight == 0)
            {
                if (isTopBottomAlignment)
                    menuItemHeight = menuItemDefaultHeight;
                else
                    menuItemHeight = menuItemDefaultHeight_SideBySide;
            }
            topMenuItemHeight = menuItemHeight;
            if (eData != null && eData.Count != 0)
            {
                dxNavigationMenuForUsers.DataSource = eData;
                dxNavigationMenuForUsers.DataBind();
            }

        }

        protected void menuTopBar_ItemDataBound(object source, MenuItemEventArgs e)
        {
            //only Execute root level menu item
            if (e.Item.Depth != 0)
                return;

            MenuNavigationItem mItem = e.Item.DataItem as MenuNavigationItem;

            if (mItem.ChildCount > 0)
            {
                SubMenuItem sbMenuItem = new SubMenuItem();
                sbMenuItem.MItem = mItem;
                TopMenuBarSubItem item = (TopMenuBarSubItem)Page.LoadControl("~/ControlTemplates/Shared/TopMenuBarSubItem.ascx");
                item.ParentClass = string.Format("parentmenu-{0}", mItem.ID);
                item.RootMenuItem = mItem;
                sbMenuItem.SubMenuItemCtr = item;

                e.Item.SubMenuTemplate = sbMenuItem;
            }
        }

        private class SubMenuItem : ITemplate
        {
            public MenuNavigationItem MItem { get; set; }
            public TopMenuBarSubItem SubMenuItemCtr { get; set; }

            public void InstantiateIn(System.Web.UI.Control container)
            {
                container.Controls.Add(SubMenuItemCtr);
            }
        }

        private enum MenuIconPosition
        {
            IconTop, IconFirst, IconRight, IconBottom
        }

        protected void anchorRootMenuItem_Load(object sender, EventArgs e)
        {
            HyperLink anchorRootMenuItem = sender as HyperLink;
            // DevExpress.Web.MenuItemTemplateContainer container = anchorRootMenuItem.NamingContainer as DevExpress.Web.MenuItemTemplateContainer;
            DevExpress.Web.NavBarItemTemplateContainer container = anchorRootMenuItem.NamingContainer as DevExpress.Web.NavBarItemTemplateContainer;

            if (container.Item.DataItem == null)
                return;

            MenuNavigationItem mItem = container.Item.DataItem as MenuNavigationItem;
            //DevExpress.Web.MenuItem currentMenuItem = container.Item;
            #region BindNavigation Url
            if (!string.IsNullOrEmpty(mItem.NavigationType))
            {
                if (anchorRootMenuItem != null)
                {
                    anchorRootMenuItem.NavigateUrl = UGITUtility.GetAbsoluteURL(mItem.NavigationUrl);
                    anchorRootMenuItem.ToolTip = mItem.Title;
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
            MenuIconPosition iconPosition = MenuIconPosition.IconFirst;
            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                if (mItem.CustomizeFormat)
                    iconPosition = GetIconPosition(mItem.MenuTextAlignment, mItem.MenuIconAlignment);

                if (iconPosition == MenuIconPosition.IconFirst)
                {
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                }
                else if (iconPosition == MenuIconPosition.IconRight)
                {
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                }
                else if (iconPosition == MenuIconPosition.IconBottom)
                {
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                }
                else
                {
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                }
            }
            else if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly)
            {
                anchorRootMenuItem.Controls.Add(imgIconDiv);
            }
            else
            {
                anchorRootMenuItem.Controls.Add(lblTitleDiv);
            }

            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly || mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                Label lblTitle = new Label();
                lblTitle.Text = mItem.Title.Trim();

                //string txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White));
                string txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Black));
                if (mItem.CustomizeFormat)
                {
                    if (!string.IsNullOrEmpty(mItem.MenuFontColor))
                        txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(UGITUtility.TranslateColorCode(mItem.MenuFontColor, System.Drawing.Color.White)));

                }
                lblTitle.Style.Add(HtmlTextWriterStyle.Color, txtFontColor);
                lblTitleDiv.Controls.Add(lblTitle);

                string titleAligment = MenuAllignmentType.Center;
                if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                {
                    lblTitleDiv.Style.Add("display", "table-cell");
                    anchorRootMenuItem.Style.Add("display", "table-row");
                }
                if (mItem.CustomizeFormat)
                {
                    titleAligment = mItem.MenuTextAlignment;
                    anchorRootMenuItem.Style.Add("height", topMenuItemHeight + "px");
                }
                lblTitleDiv.Style.Add("padding", "2px;");
                SetTitleAlignProp(titleAligment, lblTitleDiv, lblTitle);
            }


            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly || mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                int iconHeight = topMenuItemHeight;

                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both && string.IsNullOrWhiteSpace(mItem.IconUrl))
                {

                    mItem.MenuDisplayType = MenuTitleDisplayType.TitleOnly;
                }

                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
                {
                    if (isTopBottomAlignment)
                        iconHeight = topMenuItemHeight - 20;
                }

                imgIconDiv.Style.Add("height", string.Format("{0}px", iconHeight));
                imgIconDiv.Style.Add("position", "relative");
                imgIconDiv.Style.Add("padding", "3px;");
                if (!string.IsNullOrWhiteSpace(mItem.IconUrl))
                {
                    Image imgIcon = new Image();
                    imgIcon.Style.Add("max-height", string.Format("{0}px", iconHeight));
                    if (!string.IsNullOrEmpty(mItem.MenuWidth))
                        imgIcon.Style.Add("max-width", string.Format("{0}px !important", Convert.ToInt32(mItem.MenuWidth) - 10));
                    imgIcon.ImageUrl = UGITUtility.GetAbsoluteURL(mItem.IconUrl);

                    if (imgIcon.ImageUrl.StartsWith(@"\")) //added to show icons
                    {
                        imgIcon.ImageUrl = imgIcon.ImageUrl.Substring(1);
                    }

                    imgIconDiv.Controls.Add(imgIcon);

                    string iconAligment = MenuAllignmentType.Center;
                    if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                        imgIconDiv.Style.Add("display", "table-cell");
                    if (mItem.CustomizeFormat)//if (mItem.CustomizeFormat && mItem.Properties != null)
                    {
                        iconAligment = mItem.MenuIconAlignment;
                        imgIconDiv.Style.Add("width", string.Format("{0}px", iconHeight));

                    }

                    SetIconAlignProp(iconAligment, imgIconDiv, imgIcon);
                }
            }
            #endregion
        }

        private void SetIconAlignProp(string iconAligment, HtmlGenericControl imgIconDiv, Image icon)
        {
            if (string.IsNullOrWhiteSpace(iconAligment))
                iconAligment = string.Empty;

            iconAligment = iconAligment.ToLower();

            if (iconAligment == MenuAllignmentType.TopLeft.ToLower())
            {
                icon.Style.Add("position", "absolute");
                icon.Style.Add("top", "0px");
                icon.Style.Add("left", "0px");
            }
            else if (iconAligment == MenuAllignmentType.TopCenter.ToLower())
            {
                imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "top");
            }
            else if (iconAligment == MenuAllignmentType.TopRight.ToLower())
            {
                icon.Style.Add("position", "absolute");
                icon.Style.Add("top", "0px");
                icon.Style.Add("right", "0px");
            }
            else if (iconAligment == MenuAllignmentType.BottomLeft.ToLower())
            {
                icon.Style.Add("position", "absolute");
                icon.Style.Add("position", "absolute");
                icon.Style.Add("bottom", "0px");
                icon.Style.Add("left", "0px");
            }
            else if (iconAligment == MenuAllignmentType.BottomRight.ToLower())
            {
                icon.Style.Add("position", "absolute");
                icon.Style.Add("bottom", "0px");
                icon.Style.Add("right", "0px");

            }
            else if (iconAligment == MenuAllignmentType.BottomCenter.ToLower())
            {
                imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "bottom");
            }
            else
            {
                imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "middle");
                imgIconDiv.Style.Add("width", "30px");
            }
        }

        private MenuIconPosition GetIconPosition(string textAligment, string iconAligment)
        {
            MenuIconPosition iconPosition = MenuIconPosition.IconTop;
            if (string.IsNullOrWhiteSpace(textAligment))
                textAligment = string.Empty;
            textAligment = textAligment.ToLower();

            if (string.IsNullOrWhiteSpace(iconAligment))
                iconAligment = string.Empty;

            iconAligment = iconAligment.ToLower();

            if (textAligment == MenuAllignmentType.TopLeft.ToLower() && iconAligment == MenuAllignmentType.TopLeft.ToLower()
                || textAligment == MenuAllignmentType.TopRight.ToLower() && iconAligment == MenuAllignmentType.TopLeft.ToLower()
                || textAligment == MenuAllignmentType.TopCenter.ToLower() && iconAligment == MenuAllignmentType.TopLeft.ToLower()
                || textAligment == MenuAllignmentType.TopCenter.ToLower() && iconAligment == MenuAllignmentType.TopCenter.ToLower()
                || textAligment == MenuAllignmentType.Center.ToLower() && iconAligment == MenuAllignmentType.Center.ToLower()
                || textAligment == MenuAllignmentType.BottomCenter.ToLower() && iconAligment == MenuAllignmentType.BottomCenter.ToLower()
                || textAligment == MenuAllignmentType.BottomLeft.ToLower() && iconAligment == MenuAllignmentType.BottomLeft.ToLower()
                || textAligment == MenuAllignmentType.BottomRight.ToLower() && iconAligment == MenuAllignmentType.BottomLeft.ToLower()
                 || textAligment == MenuAllignmentType.BottomCenter.ToLower() && iconAligment == MenuAllignmentType.BottomLeft.ToLower())
            {
                iconPosition = MenuIconPosition.IconFirst;
            }
            else if (textAligment == MenuAllignmentType.TopRight.ToLower() && iconAligment == MenuAllignmentType.TopRight.ToLower()
                || textAligment == MenuAllignmentType.TopLeft.ToLower() && iconAligment == MenuAllignmentType.TopRight.ToLower()
                || textAligment == MenuAllignmentType.TopCenter.ToLower() && iconAligment == MenuAllignmentType.TopRight.ToLower()
                  || textAligment == MenuAllignmentType.TopLeft.ToLower() && iconAligment == MenuAllignmentType.TopCenter.ToLower()
                || textAligment == MenuAllignmentType.BottomRight.ToLower() && iconAligment == MenuAllignmentType.BottomRight.ToLower()
                 || textAligment == MenuAllignmentType.BottomLeft.ToLower() && iconAligment == MenuAllignmentType.BottomRight.ToLower()
                 || textAligment == MenuAllignmentType.BottomCenter.ToLower() && iconAligment == MenuAllignmentType.BottomRight.ToLower())
            {
                iconPosition = MenuIconPosition.IconRight;
            }
            else if (textAligment == MenuAllignmentType.TopLeft.ToLower() && (iconAligment == MenuAllignmentType.BottomLeft.ToLower() || iconAligment == MenuAllignmentType.BottomRight.ToLower() || iconAligment == MenuAllignmentType.BottomCenter.ToLower())
                || textAligment == MenuAllignmentType.TopRight.ToLower() && (iconAligment == MenuAllignmentType.BottomLeft.ToLower() || iconAligment == MenuAllignmentType.BottomRight.ToLower() || iconAligment == MenuAllignmentType.BottomCenter.ToLower())
                || textAligment == MenuAllignmentType.Center.ToLower() && (iconAligment == MenuAllignmentType.BottomLeft.ToLower() || iconAligment == MenuAllignmentType.BottomRight.ToLower() || iconAligment == MenuAllignmentType.BottomCenter.ToLower())
                || textAligment == MenuAllignmentType.TopCenter.ToLower() && (iconAligment == MenuAllignmentType.BottomLeft.ToLower() || iconAligment == MenuAllignmentType.BottomRight.ToLower() || iconAligment == MenuAllignmentType.BottomCenter.ToLower()))
            {
                iconPosition = MenuIconPosition.IconBottom;
            }

            return iconPosition;
        }

        private void SetTitleAlignProp(string titleAligment, HtmlGenericControl imgTitleDiv, Label lblTitle)
        {
            if (string.IsNullOrWhiteSpace(titleAligment))
                titleAligment = string.Empty;

            titleAligment = titleAligment.ToLower();
            lblTitle.Style.Add("display", "inline-block");
            if (titleAligment == MenuAllignmentType.TopLeft.ToLower())
            {
                lblTitle.Style.Add("text-align", "left");
                imgTitleDiv.Style.Add("vertical-align", "top");
            }
            else if (titleAligment == MenuAllignmentType.TopCenter.ToLower())
            {
                lblTitle.Style.Add("text-align", "center");
                imgTitleDiv.Style.Add("vertical-align", "top");
            }
            else if (titleAligment == MenuAllignmentType.TopRight.ToLower())
            {
                lblTitle.Style.Add("text-align", "right");
                imgTitleDiv.Style.Add("vertical-align", "top");
            }
            else if (titleAligment == MenuAllignmentType.BottomLeft.ToLower())
            {
                lblTitle.Style.Add("text-align", "left");
                imgTitleDiv.Style.Add("vertical-align", "bottom");
            }
            else if (titleAligment == MenuAllignmentType.BottomRight.ToLower())
            {
                lblTitle.Style.Add("text-align", "right");
                imgTitleDiv.Style.Add("vertical-align", "bottom");

            }
            else if (titleAligment == MenuAllignmentType.BottomCenter.ToLower())
            {
                lblTitle.Style.Add("text-align", "center");
                imgTitleDiv.Style.Add("vertical-align", "bottom");
            }
            else
            {
                lblTitle.Style.Add("text-align", "center");
                imgTitleDiv.Style.Add("vertical-align", "middle");
            }
        }

        //start
        protected void navbarLeftMenu_DataBinding(object sender, EventArgs e)
        {
            dxNavigationMenuForUsers.Groups.Clear();
            if (dxNavigationMenuForUsers.DataSource == null)
            {
                dxNavigationMenuForUsers.DataSource = eData;
                dxNavigationMenuForUsers.GroupDataFields.TextField = "Title";
                dxNavigationMenuForUsers.GroupDataFields.HeaderImageUrlField = "IconUrl";
                dxNavigationMenuForUsers.ItemDataFields.TextField = "Title";
                dxNavigationMenuForUsers.ItemDataFields.NameField = "ID";
                dxNavigationMenuForUsers.GroupHeaderImage.Width = 20;
                //navbarLeftMenu.ItemDataFields.ImageUrlField = "IconUrl";
                //navbarLeftMenu.ItemImage.Width = 15;
            }
        }

        protected void navbarLeftMenu_GroupDataBound(object source, NavBarGroupEventArgs e)
        {
            MenuNavigationItem Groupitem = e.Group.DataItem as MenuNavigationItem;
            e.Group.NavigateUrl = UGITUtility.GetAbsoluteURL(Groupitem.NavigationUrl);
            e.Group.Name = Groupitem.Title;
        }

        protected void anchorGroopmenuitem_Load(object sender, EventArgs e)
        {
            HyperLink anchorRootMenuItem = sender as HyperLink;
            DevExpress.Web.NavBarGroupTemplateContainer container = anchorRootMenuItem.NamingContainer as DevExpress.Web.NavBarGroupTemplateContainer;
            if (container.DataItem == null)
                return;

            //MenuNavigation mItem = container.Group.DataItem as MenuNavigation;
            MenuNavigationItem mItem = container.Group.DataItem as MenuNavigationItem;
            if (mItem == null)
                return;

            DevExpress.Web.NavBarGroup currentMenuItem = container.DataItem as NavBarGroup;
            #region BindNavigation Url
            if (!string.IsNullOrEmpty(mItem.NavigationType))
            {
                if (anchorRootMenuItem != null)
                {
                    anchorRootMenuItem.Style.Add("cursor", "pointer");
                    anchorRootMenuItem.NavigateUrl = UGITUtility.GetAbsoluteURL(mItem.NavigationUrl);

                    //if (mItem.NavigationType == "Modal")
                    //{
                    //    anchorRootMenuItem.NavigateUrl = "javascript:showSubMenuPopUp('" + uHelper.GetAbsoluteURL(mItem.LinkUrl) + "','" + uHelper.ReplaceInvalidCharsInURL(mItem.Title) + "')";
                    //}
                    //else if (mItem.NavigationType == "Modeless")
                    //    anchorRootMenuItem.Target = "_blank";
                }
            }
            #endregion

            #region Configure Title Configurations
            HtmlGenericControl lblTitleDiv = new HtmlGenericControl("Div");
            HtmlGenericControl imgIconDiv = new HtmlGenericControl("Div");
            MenuIconPosition iconPosition = MenuIconPosition.IconFirst;
            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                if (mItem.CustomizeFormat)
                    iconPosition = GetIconPosition(mItem.MenuTextAlignment, mItem.MenuIconAlignment);

                if (iconPosition == MenuIconPosition.IconFirst)
                {
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                }
                else if (iconPosition == MenuIconPosition.IconRight)
                {
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                }
                else if (iconPosition == MenuIconPosition.IconBottom)
                {
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                }
                else
                {
                    anchorRootMenuItem.Controls.Add(imgIconDiv);
                    anchorRootMenuItem.Controls.Add(lblTitleDiv);
                }
            }
            else if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly)
            {
                anchorRootMenuItem.Controls.Add(imgIconDiv);
            }
            else
            {
                anchorRootMenuItem.Controls.Add(lblTitleDiv);
            }

            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly || mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                Label lblTitle = new Label();
                lblTitle.Text = mItem.Title.Trim();

                //string txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Black));
                if (mItem.CustomizeFormat)
                {
                    //if (!string.IsNullOrEmpty(mItem.MenuFontColor))
                    //  txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(uHelper.TranslateColorCode(mItem.MenuFontColor, System.Drawing.Color.Black)));

                }
                lblTitle.Style.Add(HtmlTextWriterStyle.Color, "");
                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
                {
                    //lblTitle.Style.Add("padding-left", "4px;");
                }
                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly)
                {
                    lblTitle.Style.Add("text-indent", "-2px;");
                }

                lblTitleDiv.Controls.Add(lblTitle);

                string titleAligment = MenuAllignmentType.Center;
                if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                {
                    lblTitleDiv.Style.Add("display", "table-cell");
                    anchorRootMenuItem.Style.Add("display", "table-row");
                    lblTitle.Style.Add("display", "inline-block !important");
                }
                else
                {
                    lblTitleDiv.Style.Add("height", "24px;");
                    lblTitleDiv.Style.Add("display", "table");
                }
                if (mItem.CustomizeFormat)
                {
                    titleAligment = mItem.MenuTextAlignment;

                    if (!string.IsNullOrEmpty(mItem.MenuHeight.ToString()))
                        anchorRootMenuItem.Style.Add("height", mItem.MenuHeight + "px");

                }
                lblTitleDiv.Style.Add("padding-left", "2px;");
                lblTitleDiv.Style.Add("padding-right", "2px;");
                if (mItem.MenuWidth != null)
                {
                    if (!string.IsNullOrEmpty(mItem.MenuWidth.ToString()))
                    {
                        //if (IsVertical)
                        //    lblTitleDiv.Style.Add("width", string.Format("{0}px !important", topMenuItemWidth));
                        // else
                        lblTitleDiv.Style.Add("width", string.Format("{0}px !important", Convert.ToInt32(mItem.MenuWidth)));
                    }
                }
                SetTitleAlignProp(titleAligment, lblTitleDiv, lblTitle);
            }
            if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly || mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                int iconHeight = 20;
                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both && string.IsNullOrWhiteSpace(mItem.IconUrl))
                {
                    mItem.MenuDisplayType = MenuTitleDisplayType.TitleOnly;
                }
                if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
                {
                    if (isTopBottomAlignment)
                    {
                        iconHeight = 20;
                    }
                }

                imgIconDiv.Style.Add("height", string.Format("{0}px", iconHeight));
                imgIconDiv.Style.Add("position", "relative");
                imgIconDiv.Style.Add("padding", "0px;");
                if (!string.IsNullOrWhiteSpace(mItem.IconUrl))
                {
                    Image imgIcon = new Image();
                    imgIcon.Style.Add("max-width", string.Format("{0}px", iconHeight));
                    if (mItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly)
                    {
                        imgIconDiv.Style.Add("text-indent", "67px;");
                        imgIconDiv.Controls.Add(imgIcon);
                    }
                    //if (!string.IsNullOrEmpty(mItem.MenuWidth))
                    //    imgIcon.Style.Add("max-width", string.Format("{0}px !important", Convert.ToInt32(mItem.MenuWidth) - 10));
                    imgIcon.ImageUrl = UGITUtility.GetAbsoluteURL(mItem.IconUrl);
                    imgIconDiv.Controls.Add(imgIcon);

                    string iconAligment = MenuAllignmentType.Center;
                    if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                        imgIconDiv.Style.Add("display", "table-cell");
                    if (mItem.CustomizeFormat)//if (mItem.CustomizeFormat && mItem.Properties != null)
                    {
                        iconAligment = mItem.MenuIconAlignment;
                        imgIconDiv.Style.Add("width", string.Format("{0}px", iconHeight));

                    }

                    SetIconAlignProp(iconAligment, imgIconDiv, imgIcon);
                }
            }
            #endregion
        }
        //end
    }
}