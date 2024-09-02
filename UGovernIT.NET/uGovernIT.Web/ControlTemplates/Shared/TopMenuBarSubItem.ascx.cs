using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Web;
using DevExpress.Web;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web
{
    public partial class TopMenuBarSubItem : UserControl
    {
        public MenuNavigationItem RootMenuItem { get; set; }
        protected string SubMenuStyle { get; set; }
       // protected List<SPGroup> currentUserGroups = SPContext.Current.Web.CurrentUser.Groups.Cast<SPGroup>().ToList();
        int subMenuDefaultHeight = 50;
        List<MenuNavigationItem> lstSubMenuItems = new List<MenuNavigationItem>();
        double subItemsPerRow = 0;
        public TopMenuBarSubItem()
        {

        }
        private enum MenuIconPosition
        {
            IconTop, IconFirst, IconRight, IconBottom
        }
        public string ParentClass { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            SubMenuStyle = "vertical";
            if (!string.IsNullOrWhiteSpace(RootMenuItem.SubMenuStyle))
                SubMenuStyle = RootMenuItem.SubMenuStyle.ToLower();
                

            double subMenuContainerCount = 1;
            subItemsPerRow = RootMenuItem.SubMenuItemPerRow;
            double childCount = RootMenuItem.ChildCount;
            if (subItemsPerRow != 0)
                subMenuContainerCount = Math.Ceiling(childCount / subItemsPerRow);

            List<string> lstSubMenuDivIds = new List<string>();
            for (int i = 1; i <= subMenuContainerCount; i++)
            {
                lstSubMenuDivIds.Add("divSubMenuContainer" + i);
            }
            lstSubMenuItems = RootMenuItem.Children.Where(x => x != null && x.IsDisabled == false).ToList();// && (Convert.ToInt32( x.AuthorizedToView) == 0 || x.AuthorizedToView.ContainsKey(SPContext.Current.Web.CurrentUser.ID) || x.AuthorizedToView.FirstOrDefault(y => currentUserGroups.Exists(z => z.ID == y.Key)).Value != null)).OrderBy(x => x.ItemOrder).ToList(); ;
            subMenuRepeater.DataSource = lstSubMenuDivIds;
            subMenuRepeater.DataBind();
        }

        protected void subMenuRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            Repeater subMenuItemRepeater = e.Item.FindControl("subMenuItemRepeater") as Repeater;
            if (subMenuItemRepeater != null)
            {
                int startIndex = 0; //
                int endIndex = lstSubMenuItems.Count;// 
                if (Convert.ToInt16(subItemsPerRow) != 0)
                {
                    startIndex = (e.Item.ItemIndex * Convert.ToInt16(subItemsPerRow));
                    endIndex = (startIndex + Convert.ToInt16(subItemsPerRow)) ;
                    if (endIndex >= lstSubMenuItems.Count)
                        endIndex = lstSubMenuItems.Count;
                }

                if (lstSubMenuItems.Count > 0)
                {
                    List<MenuNavigationItem> lstSubItemsPerRow = new List<MenuNavigationItem>();
                    for (int j = startIndex; j < endIndex; j++)
                    {
                        lstSubItemsPerRow.Add(lstSubMenuItems[j]);
                    }
                    // List<MenuNavigation> lstSubItemsPerRow = lstSubMenuItems.OrderBy(x => x.ItemOrder).Where(x => x.ItemOrder >= startIndex && x.ItemOrder <= endIndex).ToList();
                    if (lstSubItemsPerRow.Count > 0)
                    {
                        subMenuItemRepeater.DataSource = lstSubItemsPerRow;
                        subMenuItemRepeater.DataBind();
                    }
                }
            }

        }
        protected void subMenuItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            MenuNavigationItem subItem = e.Item.DataItem as MenuNavigationItem;
            Panel divSubMenuitem = e.Item.FindControl("divSubMenuitem") as Panel;
            HyperLink subMenuLink = e.Item.FindControl("subMenuLink") as HyperLink;
            #region subMenuCreation
            HtmlGenericControl lblTitleDiv = new HtmlGenericControl("Div");
            HtmlGenericControl imgIconDiv = new HtmlGenericControl("Div");
            MenuIconPosition iconPosition = MenuIconPosition.IconFirst;
            bool isTopBottomAlignment = false;
            if ((subItem.MenuTextAlignment == MenuAllignmentType.TopLeft && subItem.MenuIconAlignment == MenuAllignmentType.BottomLeft)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomLeft && subItem.MenuIconAlignment == MenuAllignmentType.TopLeft)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopRight && subItem.MenuIconAlignment == MenuAllignmentType.BottomRight)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomRight && subItem.MenuIconAlignment == MenuAllignmentType.TopRight)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomRight && subItem.MenuIconAlignment == MenuAllignmentType.TopLeft)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopLeft && subItem.MenuIconAlignment == MenuAllignmentType.BottomRight)
                || (subItem.MenuTextAlignment == MenuAllignmentType.TopLeft && subItem.MenuIconAlignment == MenuAllignmentType.BottomCenter)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomLeft && subItem.MenuIconAlignment == MenuAllignmentType.TopRight)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopRight && subItem.MenuIconAlignment == MenuAllignmentType.BottomLeft)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopRight && subItem.MenuIconAlignment == MenuAllignmentType.BottomCenter)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopCenter && subItem.MenuIconAlignment == MenuAllignmentType.BottomCenter)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopCenter && subItem.MenuIconAlignment == MenuAllignmentType.BottomLeft)
               || (subItem.MenuTextAlignment == MenuAllignmentType.TopCenter && subItem.MenuIconAlignment == MenuAllignmentType.BottomRight)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomCenter && subItem.MenuIconAlignment == MenuAllignmentType.TopCenter)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomLeft && subItem.MenuIconAlignment == MenuAllignmentType.TopCenter)
               || (subItem.MenuTextAlignment == MenuAllignmentType.BottomRight && subItem.MenuIconAlignment == MenuAllignmentType.TopCenter))
            {
                isTopBottomAlignment = true;
            }

            if (string.IsNullOrWhiteSpace(subItem.MenuDisplayType))
                subItem.MenuDisplayType = MenuTitleDisplayType.TitleOnly;

            if (subItem.MenuDisplayType.ToLower() != MenuTitleDisplayType.Both || !isTopBottomAlignment)
                subMenuDefaultHeight = 25;

            int subMenuHeight = subMenuDefaultHeight;
            if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                if (subItem.CustomizeFormat)
                    iconPosition = GetIconPosition(subItem.MenuTextAlignment, subItem.MenuIconAlignment);
                else if (subItem.Parent.CustomizeFormat)
                    iconPosition = GetIconPosition(subItem.Parent.MenuTextAlignment, subItem.Parent.MenuIconAlignment);

                if (iconPosition == MenuIconPosition.IconFirst)
                {
                    subMenuLink.Controls.Add(imgIconDiv);
                    subMenuLink.Controls.Add(lblTitleDiv);
                }
                else if (iconPosition == MenuIconPosition.IconRight)
                {
                    subMenuLink.Controls.Add(lblTitleDiv);
                    subMenuLink.Controls.Add(imgIconDiv);
                }
                else if (iconPosition == MenuIconPosition.IconBottom)
                {
                    subMenuLink.Controls.Add(lblTitleDiv);
                    subMenuLink.Controls.Add(imgIconDiv);
                }
                else
                {
                    subMenuLink.Controls.Add(imgIconDiv);
                    subMenuLink.Controls.Add(lblTitleDiv);
                }
            }
            else if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly)
            {
                subMenuLink.Controls.Add(imgIconDiv);
            }
            else
            {
                subMenuLink.Controls.Add(lblTitleDiv);
            }

            if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly || subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                Label lblTitle = new Label();
                lblTitle.Text = subItem.Title.Trim();

                //string txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.White));
                string txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Black));
                string titleAligment = MenuAllignmentType.Center;
                string fontSize = Constants.DefaultMenuFontSize;
                string fontFamily = string.Empty;
                if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                {
                    lblTitleDiv.Style.Add("display", "table-cell");
                    subMenuLink.Style.Add("display", "table !important");
                }
                if (subItem.CustomizeFormat)
                {
                    if (!string.IsNullOrEmpty(subItem.MenuFontColor))
                        txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(UGITUtility.TranslateColorCode(subItem.MenuFontColor, System.Drawing.Color.White)));
                    titleAligment = subItem.MenuTextAlignment;

                    if (!string.IsNullOrEmpty(subItem.MenuHeight))
                        subMenuHeight = UGITUtility.StringToInt(subItem.MenuHeight);
                    subMenuLink.Style.Add("height", subMenuHeight + "px");

                    if (!string.IsNullOrEmpty(subItem.MenuWidth))
                        subMenuLink.Style.Add("width", UGITUtility.StringToInt(subItem.MenuWidth) + "px");

                    if (!string.IsNullOrEmpty(subItem.MenuFontSize))
                        fontSize = subItem.MenuFontSize;

                    if (!string.IsNullOrEmpty(subItem.MenuFontFontFamily))
                    {
                        fontFamily = subItem.MenuFontFontFamily;

                    }
                }
                else if (subItem.Parent.CustomizeFormat)
                {
                    if (!string.IsNullOrEmpty(subItem.Parent.MenuFontColor))
                        txtFontColor = string.Format("{0}", System.Drawing.ColorTranslator.ToHtml(UGITUtility.TranslateColorCode(subItem.Parent.MenuFontColor, System.Drawing.Color.White)));
                    if (!string.IsNullOrEmpty(subItem.Parent.MenuHeight))
                        subMenuHeight = UGITUtility.StringToInt(subItem.Parent.MenuHeight);
                    subMenuLink.Style.Add("height", subMenuHeight + "px");

                    if (!string.IsNullOrEmpty(subItem.Parent.MenuWidth))
                        subMenuLink.Style.Add("width", UGITUtility.StringToInt(subItem.Parent.MenuWidth) + "px");


                    //if (!string.IsNullOrEmpty(subItem.Parent.Properties.MenuFontSize))
                    //    fontSize = subItem.Parent.Properties.MenuFontSize;

                    //if (!string.IsNullOrEmpty(subItem.Parent.Properties.MenuFontFontFamily))
                    //{
                    //    fontFamily = subItem.Properties.MenuFontFontFamily;

                    //}
                }
                lblTitle.Style.Add(HtmlTextWriterStyle.Color, txtFontColor);                
                lblTitle.Style.Add(HtmlTextWriterStyle.FontSize, fontSize + "pt");
                if (!string.IsNullOrEmpty(fontFamily) && fontFamily.ToLower() != "default")
                {
                    lblTitle.Style.Add(HtmlTextWriterStyle.FontSize, fontFamily);
                }
                lblTitleDiv.Controls.Add(lblTitle);
                lblTitleDiv.Style.Add("padding", "0px;");
                SetTitleAlignProp(titleAligment, lblTitleDiv, lblTitle);
            }
            if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.IconOnly || subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
            {
                int iconHeight = subMenuHeight;

                if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both && string.IsNullOrWhiteSpace(subItem.IconUrl))
                {
                    subItem.MenuDisplayType = MenuTitleDisplayType.TitleOnly;
                }

                if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.Both)
                {
                    if (isTopBottomAlignment)
                        iconHeight = iconHeight - 20;
                }

                imgIconDiv.Style.Add("height", string.Format("{0}px", iconHeight));
                imgIconDiv.Style.Add("position", "relative");
                imgIconDiv.Style.Add("padding", "0px;");

                if (!string.IsNullOrWhiteSpace(subItem.IconUrl))
                {
                    Image imgIcon = new Image();

                    imgIcon.ImageUrl = UGITUtility.GetAbsoluteURL(subItem.IconUrl);
                    imgIconDiv.Controls.Add(imgIcon);

                    string iconAligment = MenuAllignmentType.Center;

                    if (iconPosition != MenuIconPosition.IconTop && iconPosition != MenuIconPosition.IconBottom)
                    {
                        // 
                        imgIconDiv.Style.Add("padding-right", "10px ;");
                        imgIconDiv.Style.Add("width", string.Format("{0}px", subMenuDefaultHeight + 10));
                        imgIconDiv.Style.Add("display", "table-cell");
                    }
                    else
                    {
                        int imgIconDivWidth = 0;
                        if (!string.IsNullOrEmpty(subItem.MenuWidth))
                        {
                            imgIconDivWidth = Convert.ToInt32(subItem.MenuWidth);
                        }
                        else if (!string.IsNullOrEmpty(subItem.Parent.MenuWidth))
                            imgIconDivWidth = Convert.ToInt32(subItem.Parent.MenuWidth);
                        else
                            imgIconDivWidth = 80;

                        imgIconDiv.Style.Add("width", string.Format("{0}px", imgIconDivWidth));
                        imgIcon.Style.Add("max-width", string.Format("{0}px !important", imgIconDivWidth - 20));

                        imgIcon.Style.Add("max-height", string.Format("{0}px !important", iconHeight));

                    }
                    if (subItem.CustomizeFormat)//if (subItem.CustomizeFormat && subItem.Properties != null)
                    {
                        iconAligment = subItem.MenuIconAlignment;


                    }
                    SetIconAlignProp(iconAligment, imgIconDiv, imgIcon);
                }
            }
            #region Navigation Link
            //if (!string.IsNullOrEmpty(subItem.NavigationType))
            if (!string.IsNullOrEmpty(subItem.NavigationType) && subItem.Deleted == false) //Added to disable link based on Deleted flag in DB.
            {
                if (subMenuLink != null && !string.IsNullOrEmpty(subItem.NavigationUrl))
                    subMenuLink.NavigateUrl = UGITUtility.GetAbsoluteURL(subItem.NavigationUrl);
                if (subItem.NavigationType == "Modal")
                {
                    subMenuLink.NavigateUrl = "javascript:void(0)";
                    subMenuLink.Attributes.Add("onClick", "showSubMenuPopUp('" + UGITUtility.GetAbsoluteURL(subItem.NavigationUrl) + "','" + UGITUtility.ReplaceInvalidCharsInURL(subItem.Title) + "')");
                }
                else if (subItem.NavigationType == "Modeless")
                    subMenuLink.Target = "_blank";
            }

            if (SubMenuStyle.ToLower() == "vertical")
                subMenuLink.CssClass += " verticalSubMenu";
            #endregion
            if (subItem.CustomizeFormat == false)
            {
                SetMenuConfig(subItem.Parent, divSubMenuitem, subMenuLink, lblTitleDiv, imgIconDiv, iconPosition);
                string currentDevExTheme = DevExpress.Web.ASPxWebControl.GlobalTheme;
                if (currentDevExTheme.ToLower() != "ugitclassic" && currentDevExTheme.ToLower() != "ugitclassicdevex") 
                    divSubMenuitem.CssClass += string.Format("{0} {1}", " divSubMenuitem menu-item",ParentClass);

            }
            else
            {
                SetMenuConfig(subItem, divSubMenuitem, subMenuLink, lblTitleDiv, imgIconDiv, iconPosition);
            }

            #endregion
        }

        private void SetMenuConfig(MenuNavigationItem subItem, Panel divSubMenuitem, HyperLink subMenuLink, HtmlGenericControl lblTitleDiv, HtmlGenericControl imgIconDiv, MenuIconPosition iconPosition)
        {

            #region add sepration between sub menus

            if (!string.IsNullOrEmpty(subItem.MenuItemSeparation))
            {
                if (!Convert.ToString(subItem.MenuItemSeparation).Contains("px"))
                {
                    divSubMenuitem.Style.Add(HtmlTextWriterStyle.Margin, Convert.ToString(subItem.MenuItemSeparation) + "px");
                }
                else
                {
                    divSubMenuitem.Style.Add(HtmlTextWriterStyle.Margin, Convert.ToString(subItem.MenuItemSeparation));
                }
            }
            else
            {
                divSubMenuitem.Style.Add(HtmlTextWriterStyle.Margin, "0px");
            }
            #endregion sepration between sub menus

            #region Set sub menu background  layer
            if (subMenuLink != null)
            {
                if (!string.IsNullOrEmpty(subItem.MenuBackground) && subItem.CustomizeFormat == true)
                {
                    if (!Convert.ToString(subItem.MenuBackground).Contains("."))
                    {
                        divSubMenuitem.BackColor = UGITUtility.TranslateColorCode(subItem.MenuBackground, System.Drawing.Color.Black);
                        divSubMenuitem.Style.Add("background-color", UGITUtility.TranslateColorCode(subItem.MenuBackground, System.Drawing.Color.Black).Name + " !important");
                    }
                    else
                    {
                        divSubMenuitem.BackImageUrl = UGITUtility.GetAbsoluteURL(subItem.MenuBackground);
                    }
                }
            }
            #endregion Set sub menu background  layer
        }

        private void BindSubMenuIcon(MenuNavigationItem subItem, Image sbMenIcon)
        {
            if (!string.IsNullOrEmpty(subItem.IconUrl) && sbMenIcon != null)
            {
                sbMenIcon.Visible = true;
                sbMenIcon.ImageUrl = UGITUtility.GetAbsoluteURL(subItem.IconUrl);
            }
            else
                sbMenIcon.Visible = false;
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
                  || textAligment == MenuAllignmentType.TopRight.ToLower() && iconAligment == MenuAllignmentType.TopCenter.ToLower()
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
                   || textAligment == MenuAllignmentType.TopRight.ToLower() && iconAligment == MenuAllignmentType.TopCenter.ToLower()
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
                // imgIconDiv.Style.Add("display", "table-cell");
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
                icon.Style.Add("bottom", "0px");
                icon.Style.Add("left", "0px");
                // imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "bottom");
            }
            else if (iconAligment == MenuAllignmentType.BottomRight.ToLower())
            {
                icon.Style.Add("position", "absolute");
                icon.Style.Add("bottom", "0px");
                icon.Style.Add("right", "0px");
                // imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "bottom");

            }
            else if (iconAligment == MenuAllignmentType.BottomCenter.ToLower())
            {
                imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "bottom");
            }
            else
            {
                // imgIconDiv.Style.Add("display", "table-cell");
                imgIconDiv.Style.Add("vertical-align", "middle");
            }
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

        private void BindSubMenuTitle(MenuNavigationItem subItem, Label mTopLbl, Label mbottomLbl, Panel subMenuItem, HyperLink subMenuLink)
        {
            string align = "Left";
            string menuFontSize = Constants.DefaultMenuFontSize;
            string menuFontFamily = string.Empty;
            string menuIconAlignment = string.Empty;

            //if (subItem.CustomProperties != null)
            //{
            //    subItem.CustomProperties.TryGetValue("MenuFontSize", out menuFontSize);
            //    subItem.CustomProperties.TryGetValue("MenuFontFontFamily", out menuFontFamily);
            //    subItem.CustomProperties.TryGetValue("MenuIconAlignment", out menuIconAlignment);
            //}
            //set menu Font Size
            if (!string.IsNullOrEmpty(menuFontSize))
            {
                mTopLbl.Style.Add(HtmlTextWriterStyle.FontSize, string.Format("{0}pt", menuFontSize));
                mbottomLbl.Style.Add(HtmlTextWriterStyle.FontSize, string.Format("{0}pt", menuFontSize));
            }
            //check text  alignment
            if (subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.TopLeft.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomLeft.ToLower())
            {
                align = "Left";
            }
            else if (subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.TopRight.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomRight.ToLower())
            {
                align = "Right";
            }
            else if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.Center.ToLower())
            {
                align = "center";
            }

            #region MenuAllignment
            //set alignment
            if (subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.TopLeft.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.TopCenter.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.TopRight.ToLower())
            {
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, align);
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                //  if (subItem.DisplayType.ToLower() == MenuTitleDisplayType.TitleOnly.ToLower())
                subMenuItem.Style.Add("vertical-align", "top");
            }
            else if (subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomCenter.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomLeft.ToLower() || subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomRight.ToLower())
            {
                mbottomLbl.Style.Add(HtmlTextWriterStyle.TextAlign, align);
                mbottomLbl.Text = subItem.Title;
                mTopLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                //  if (subItem.DisplayType.ToLower() == MenuTitleDisplayType.TitleOnly.ToLower())
                subMenuItem.Style.Add("vertical-align", "bottom");
            }
            else if (subItem.MenuTextAlignment.ToLower() == MenuAllignmentType.Center.ToLower())
            {
                //it will show menu title in center but there should be no icon it
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                subMenuItem.Style.Add("vertical-align", "middle");
            }
            else
            {
                //it will show menu title in  top-center
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                subMenuItem.Style.Add("vertical-align", "middle");
            }

            if (align != "Left")
            {
                subMenuLink.Style.Add("width", "100%");
            }

            #endregion
            //set font color

            if (!string.IsNullOrEmpty(subItem.MenuFontColor))
                subMenuItem.ForeColor = UGITUtility.TranslateColorCode(subItem.MenuFontColor, System.Drawing.Color.Black);
            else
                subMenuItem.ForeColor = System.Drawing.Color.Black;
        }

        private void InheritSubMenuformating(MenuNavigationItem subItem, Label mTopLbl, Label mbottomLbl, Panel subMenuItem, HyperLink subMenuLink)
        {
            string align = "Left";
            string menuFontSize = Constants.DefaultMenuFontSize;
            string menuFontFamily = string.Empty;

            //if (subItem.Parent.CustomProperties != null && subItem.Parent.CustomizeFormat == true)
            //{
            //    subItem.Parent.CustomProperties.TryGetValue("MenuFontSize", out menuFontSize);
            //    subItem.Parent.CustomProperties.TryGetValue("MenuFontFontFamily", out menuFontFamily);
            //}
            //set menu Font Size
            if (!string.IsNullOrEmpty(menuFontSize))
            {
                mTopLbl.Style.Add(HtmlTextWriterStyle.FontSize, string.Format("{0}pt", menuFontSize));
                mbottomLbl.Style.Add(HtmlTextWriterStyle.FontSize, string.Format("{0}pt", menuFontSize));
            }
            //set menu font Family
            if (!string.IsNullOrEmpty(menuFontFamily) && menuFontFamily.ToLower() != "default")
            {
                mTopLbl.Style.Add(HtmlTextWriterStyle.FontFamily, menuFontFamily);
                mbottomLbl.Style.Add(HtmlTextWriterStyle.FontFamily, menuFontFamily);
            }
            //check text  alignment
            if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopLeft.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomLeft.ToLower())
            {
                align = "Left";
            }
            else if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopRight.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomRight.ToLower())
            {
                align = "Right";
            }
            else if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.Center.ToLower())
            {
                align = "center";
            }
            #region MenuAllignment
            //set alignment

            if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopLeft.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.TopRight.ToLower())
            {
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, align);
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly.ToLower())
                    subMenuItem.Style.Add("vertical-align", "top");
            }
            else if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomCenter.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomLeft.ToLower() || subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.BottomRight.ToLower())
            {
                mbottomLbl.Style.Add(HtmlTextWriterStyle.TextAlign, align);
                mbottomLbl.Text = subItem.Title;
                mTopLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                if (subItem.MenuDisplayType.ToLower() == MenuTitleDisplayType.TitleOnly.ToLower())
                    subMenuItem.Style.Add("vertical-align", "bottom");
            }
            else if (subItem.Parent.MenuTextAlignment.ToLower() == MenuAllignmentType.Center.ToLower())
            {
                //it will show menu title in center but there should be no icon it
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
                subMenuItem.Style.Add("vertical-align", "middle");
            }
            else
            {
                //it will show menu title in  top-center
                mTopLbl.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                mTopLbl.Text = subItem.Title;
                mbottomLbl.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            if (align != "Left")
            {
                subMenuLink.Style.Add("width", "100%");
            }
            #endregion


            //set font color

            if (!string.IsNullOrEmpty(subItem.Parent.MenuFontColor) && subItem.Parent.MenuFontColor != "#0" && subItem.Parent.CustomizeFormat == true)
                subMenuItem.ForeColor = UGITUtility.TranslateColorCode(subItem.Parent.MenuFontColor, System.Drawing.Color.White);
            else
                subMenuItem.ForeColor = System.Drawing.Color.White;

        }


    }
}