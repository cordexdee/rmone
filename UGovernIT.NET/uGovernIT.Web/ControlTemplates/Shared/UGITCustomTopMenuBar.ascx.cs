using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Web.Helpers;
using AutoMapper;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class UGITCustomTopMenuBar : System.Web.UI.UserControl
    {
        protected string filterPage;
        protected string logoUrl = string.Empty;
        public bool UgitHideGlobalSearch { get; set; }
        public bool HideTopMenu { get; set; }
        public bool HideFooter { get; set; }
        public string MenuType { get; set; }

        int menuItemDefaultHeight = 50;
        int menuItemDefaultHeight_SideBySide = 20;

        public string siteurl = "";// SPContext.Current.Web.Url;
       // bool isAdmin = false;// SPContext.Current.Web.CurrentUser.IsSiteAdmin;
       // protected List<SPGroup> currentUserGroups = SPContext.Current.Web.CurrentUser.Groups.Cast<SPGroup>().ToList();
        int topMenuItemHeight = 0;
        bool isTopBottomAlignment = true;
        protected string menuHighlightColor;

        protected override void OnInit(EventArgs e)
        {
            // get filterpage
            //string userInfoPageUrl =  UGITUtility.GetAbsoluteURL(string.Format("/_layouts/15/ugovernit/userinfo.aspx"));
            //var param = "uID=" + SPContext.Current.Web.CurrentUser.ID;

            base.OnInit(e);
        }
        MenuNavigationManager objMenuNavigationHelper = new MenuNavigationManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            //MenuType = "MenuBar";
            //if (!Page.IsPostBack)
            {
                List<MenuNavigation> menu = objMenuNavigationHelper.LoadMenuNavigation(MenuType);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MenuNavigation, MenuNavigationItem>().MaxDepth(2);
                });
                var mapper = config.CreateMapper();
                List<MenuNavigationItem> menuList = mapper.Map<List<MenuNavigationItem>>(menu);

                List<MenuNavigationItem> rootmenu = menuList.Where(x => x.MenuParentLookup == 0).ToList();


                MenuNavigationCollection eData = new MenuNavigationCollection();
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

                dxNavigationMenuForUsers.DataSource = eData;
                dxNavigationMenuForUsers.TextField = "Title";
                dxNavigationMenuForUsers.NameField = "ID";
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

            //DevExpress.Web.MenuItem menuItem = e.Item;

            //menuItem.SubMenuStyle.CssClass = string.Format("submenu-{0} parentmenu-{0} ", mItem.ID);
            //menuItem.ItemStyle.CssClass += string.Format(" parentmenu-{0} submenuitemalignment-{1}", mItem.ID, mItem.SubMenuItemAlignment);

            //menuItem.ItemStyle.Font.Size = new FontUnit(new Unit(Convert.ToDouble(Constants.DefaultMenuFontSize), UnitType.Point));

            //if (!string.IsNullOrEmpty(mItem.MenuWidth))
            //    menuItem.ItemStyle.Width = new Unit(mItem.MenuWidth);

            //menuItem.ItemStyle.Height = new Unit(topMenuItemHeight);

            //menuTopBar.AutoSeparators = AutoSeparatorMode.RootOnly;
            //menuTopBar.SeparatorColor = System.Drawing.Color.White;

            //if (mItem.CustomizeFormat == false)
            //{
            //    menuTopBar.ItemSpacing = new Unit(0);
            //    string currentDevExTheme = DevExpress.Web.ASPxWebControl.GlobalTheme;
            //    if (currentDevExTheme.ToLower() != "ugitclassic" && currentDevExTheme.ToLower() != "ugitclassicdevex")
            //        menuItem.ItemStyle.CssClass += " menu-item";
            //}
            //else
            //{
            //    #region MenuFontSettings
            //    string menuFontSize = string.Empty;
            //    string menuFontFamily = string.Empty;
            //    string menuIconAlignment = string.Empty;
            //    //if (mItem.CustomProperties != null)
            //    //{
            //    //    mItem.CustomProperties.TryGetValue("MenuFontSize", out menuFontSize);
            //    //    mItem.CustomProperties.TryGetValue("MenuFontFontFamily", out menuFontFamily);
            //    //    mItem.CustomProperties.TryGetValue("MenuIconAlignment", out menuIconAlignment);
            //    //}

            //    if (!string.IsNullOrEmpty(menuFontSize))
            //        menuItem.ItemStyle.Font.Size = new FontUnit(new Unit(Convert.ToDouble(menuFontSize), UnitType.Point));
            //    else
            //        menuItem.ItemStyle.Font.Size = new FontUnit(new Unit(Convert.ToDouble(Constants.DefaultMenuFontSize), UnitType.Point));

            //    if (!string.IsNullOrEmpty(menuFontFamily))
            //    {
            //        if (menuFontFamily != "Default")
            //            menuItem.ItemStyle.Font.Name = menuFontFamily;
            //    }
            //    #endregion

            //    if (!string.IsNullOrEmpty(mItem.MenuItemSeparation))
            //        menuTopBar.ItemSpacing = new Unit(mItem.MenuItemSeparation);
            //    if (!string.IsNullOrEmpty(mItem.MenuBackground))
            //    {
            //        if (!Convert.ToString(mItem.MenuBackground).Contains("."))
            //            menuItem.ItemStyle.BackColor = UGITUtility.TranslateColorCode(mItem.MenuBackground, System.Drawing.Color.Black);
            //        else
            //            menuItem.ItemStyle.BackgroundImage.ImageUrl = UGITUtility.GetAbsoluteURL(mItem.MenuBackground);
            //    }
            //}
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
            DevExpress.Web.MenuItemTemplateContainer container = anchorRootMenuItem.NamingContainer as DevExpress.Web.MenuItemTemplateContainer;
            if (container.Item.DataItem == null)
                return;

            MenuNavigationItem mItem = container.Item.DataItem as MenuNavigationItem;
            DevExpress.Web.MenuItem currentMenuItem = container.Item;
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
    }
}