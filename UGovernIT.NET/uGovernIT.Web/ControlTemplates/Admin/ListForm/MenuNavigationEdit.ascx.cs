using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class MenuNavigationEdit : UserControl
    {
        public string Id { get; set; }
        public string MenuType { get; set; }
        List<MenuNavigation> menuNavigationList;
        MenuNavigation item;
        DataTable webMenuData;
        protected bool isCustomizationClicked { get; set; }
        private string absPath = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        //MenuNavigationManager menuNavigationMGR = new MenuNavigationManager(HttpContext.Current.GetManagerContext());
        //ConfigurationVariableManager configMGR = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        MenuNavigationManager menuNavigationMGR = null;
        ConfigurationVariableManager configMGR = null;
        UserProfile User;
        protected override void OnInit(EventArgs e)
        {
            menuNavigationMGR = new MenuNavigationManager(context);
            configMGR = new ConfigurationVariableManager(context);

            User = HttpContext.Current.CurrentUser();
            //SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.MenuName);
            //query.Query = string.Format("<Where></Where>");
            webMenuData = menuNavigationMGR.GetDataTable().AsDataView().ToTable(DatabaseObjects.Columns.MenuName);  // SPListHelper.GetDataTable(DatabaseObjects.Lists.MenuNavigation, query, SPContext.Current.Web);
            BindMenuNames();
            BindMenuTextAlignmentTypes();

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
           // lnkbtnPickAssets.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            //absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromLibrary);
            //lnkbackgroundimage.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','900px','600px','','')", absPath, "Pick From Library"));

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UGITTheme objUgitTheme = UGITTheme.GetDefaultTheme();

            WebBkTr.Style.Add(HtmlTextWriterStyle.Display, "none");
            WebSbMenuStyleTr.Style.Add(HtmlTextWriterStyle.Display, "none");
            trSubMenuItemsPerRow.Style.Add(HtmlTextWriterStyle.Display, "none");
            WebMenuTextAlignmentTr.Visible = false;
            WebMenuSeprationTr.Visible = false;
            WebMenuFontPropTr.Visible = false;
            webMenuHeightWidthTr.Visible = false;
            webcustomizeformateTr.Visible = false;
            webformattingDIV.Visible = false;
            webisdiabledDiv.Visible = false;
            WebMenuIconAlignment.Visible = false;

            if (MenuType.ToLower() == "mobile" || Convert.ToString(ddlMenuName.Value).ToLower() == "mobile")
            {
                menuNavigationList = menuNavigationMGR.Load(); // SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigationMobile);
            }
            else
            {
                trMenuName.Visible = true;
                WebBkTr.Style.Remove(HtmlTextWriterStyle.Display);
                webMenuHeightWidthTr.Visible = true;
                WebMenuTextAlignmentTr.Visible = true;
                WebMenuSeprationTr.Visible = true;
                WebMenuFontPropTr.Visible = true;
                webcustomizeformateTr.Visible = true;
                webformattingDIV.Visible = true;
                webisdiabledDiv.Visible = true;
                WebMenuIconAlignment.Visible = true;
                menuNavigationList = menuNavigationMGR.Load();  // SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation);
            }

            //if(webformattingDIV.Visible==true)
            //BindMenuTextAlignmentTypes(SPContext.Current.Web);

            if (!IsPostBack)
            {
                ddlMenuName.SelectedIndex = ddlMenuName.Items.IndexOf(ddlMenuName.Items.FindByText(MenuType));
                if (string.IsNullOrWhiteSpace(MenuType))
                {
                    ddlMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    txtMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "block");
                    addMenuName.Style.Add(HtmlTextWriterStyle.Display, "none");
                    editMenuName.Style.Add(HtmlTextWriterStyle.Display, "none");
                    cancelMenuName.Style.Add(HtmlTextWriterStyle.Display, "block");
                    hdnMenuName.Value = "1";
                }
                BindParent();

            }
            if (UGITUtility.StringToInt(Id) == 0)
            {
                item = new MenuNavigation();  // menuNavigationList.AddItem();
                                              //if (menuNavigationList.Title != "Mobile")
                                              //{
                rListBackground.SelectedValue = "1";
                ceBkColor.CssClass = ceBkColor.CssClass.Replace("hide", "");
                WebSbMenuStyleTr.Style.Remove(HtmlTextWriterStyle.Display);
                if (cbxMenuStyle.SelectedIndex != 0)
                    trSubMenuItemsPerRow.Style.Remove(HtmlTextWriterStyle.Display);
                else
                    trSubMenuItemsPerRow.Style.Add(HtmlTextWriterStyle.Display, "none");

                //}
                webformattingDIV.Visible = false;
                lnkbtnDelete.Visible = false;
            }
            else
            {
                lnkbtnDelete.Visible = true;
                item = menuNavigationMGR.LoadByID(Convert.ToInt64(Id));  // SPListHelper.GetSPListItem(menuNavigationList, Convert.ToInt32(Id));
                if (!IsPostBack)
                {
                    if (item != null)
                    {
                        txtTitle.Text = Convert.ToString(item.Title);
                        if (!string.IsNullOrEmpty(Convert.ToString(DatabaseObjects.Columns.ItemOrder)))
                            txtItemOrder.Text = Convert.ToString(item.ItemOrder);
                        txtNavigationUrl.Text = Convert.ToString(item.NavigationUrl);
                        //SPFieldLookupValue menuParentLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.MenuParentLookup)));
                        ddlParent.SelectedValue = Convert.ToString(item.MenuParentLookup);
                        ddlDisplayType.SelectedIndex = ddlDisplayType.Items.IndexOf(ddlDisplayType.Items.FindByValue(Convert.ToString(item.MenuDisplayType).ToLower())); ;
                        ddlNavigationType.SelectedValue = Convert.ToString(item.NavigationType);
                        ddlDisplayType_SelectedIndexChanged(null, null);

                        if(item.IconUrl.StartsWith(@"\")) //added on 12-07-2018
                        {
                            item.IconUrl = item.IconUrl.Substring(1);
                        }
                        UGITFileUploadManager1.SetImageUrl(item.IconUrl);
                       // txtImageUrl.Text = Convert.ToString(item.IconUrl);
                        try
                        {
                            if (ddlParent.SelectedIndex == 0)
                            {
                                WebSbMenuStyleTr.Style.Remove(HtmlTextWriterStyle.Display);
                                trSubMenuItemsPerRow.Style.Remove(HtmlTextWriterStyle.Display);
                                spanMenuSeparationMsg.Style.Add("display", "block");
                            }
                            else
                                spanMenuSeparationMsg.Style.Add("display", "none");
                            ceFontcolor.Color = uHelper.TranslateColorCode(Convert.ToString(item.MenuFontColor), System.Drawing.Color.White);
                            if (!string.IsNullOrEmpty(Convert.ToString(item.MenuBackground)) && !Convert.ToString(item.MenuBackground).Contains("."))
                            {
                                rListBackground.SelectedValue = "1";
                                ceBkColor.CssClass = ceBkColor.CssClass.Replace("hide", "");
                                ceBkColor.Color = uHelper.TranslateColorCode(Convert.ToString(item.MenuBackground), System.Drawing.Color.Black);
                            }
                            else
                            {
                                rListBackground.SelectedValue = "2";
                                //txtBkImgUrl.CssClass = txtBkImgUrl.CssClass.Replace("hide", "");
                                //fileUpload.CssClass = fileUpload.CssClass.Replace("hide", "");
                                //lnkbackgroundimage.CssClass = lnkbackgroundimage.CssClass.Replace("hide", "");
                                //txtBkImgUrl.Text = Convert.ToString(item.MenuBackground);
                                ugitFileUpload.SetImageUrl(Convert.ToString(item.MenuBackground));
                            }
                            if (!(item.MenuHeight == null || Convert.ToString(item.MenuHeight) == ""))
                                spnBtnMenuHeight.Value = UGITUtility.StringToInt(item.MenuHeight);

                            if (!(item.MenuWidth == null || Convert.ToString(item.MenuWidth) == ""))
                                spnBtnMenuWidth.Value = UGITUtility.StringToInt(item.MenuWidth);
                            cbxMenuStyle.Value = Convert.ToString(item.SubMenuStyle);

                            cbxMenuTextAlignment.Value = Convert.ToString(item.MenuTextAlignment);

                            if (cbxMenuStyle.SelectedIndex != 0)
                                trSubMenuItemsPerRow.Style.Remove(HtmlTextWriterStyle.Display);
                            else
                                trSubMenuItemsPerRow.Style.Add(HtmlTextWriterStyle.Display, "none");

                            if (item.SubMenuItemPerRow != null && item.SubMenuItemPerRow != 0)
                                seSubMenuItemsPerRow.Value = UGITUtility.StringToInt(item.SubMenuItemPerRow);

                            if (!string.IsNullOrEmpty(item.MenuItemSeparation))
                                spnBtnMenuSeparation.Value = Convert.ToInt32(item.MenuItemSeparation);

                            if (!string.IsNullOrEmpty(item.SubMenuItemAlignment))
                                cbxSubMenuTextAlignment.Value = Convert.ToString(item.SubMenuItemAlignment);


                            if (!string.IsNullOrEmpty(Convert.ToString(item.CustomProperties)))
                            {
                                XmlDocument obj = new XmlDocument();
                                obj.LoadXml(Convert.ToString(item.CustomProperties));
                                MenuNavigationProperties menuNavigationprop = new MenuNavigationProperties();
                                menuNavigationprop = (MenuNavigationProperties)uHelper.DeSerializeAnObject(obj, menuNavigationprop);
                                if (menuNavigationprop != null)
                                {
                                    if (menuNavigationprop.MenuFontSize != "")
                                        spnBtnMenuFontSize.Value = Convert.ToUInt16(menuNavigationprop.MenuFontSize);

                                    if (menuNavigationprop.MenuFontFontFamily != "")
                                        cbxMenuFontFamily.Value = Convert.ToString(menuNavigationprop.MenuFontFontFamily);

                                    if (menuNavigationprop.MenuIconAlignment != "")
                                        cbxMenuIconAlignment.Value = Convert.ToString(menuNavigationprop.MenuIconAlignment);
                                }
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(item.CustomizeFormat)))
                            {
                                CustomizeMenuFormat.Checked = Convert.ToBoolean(item.CustomizeFormat);

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item.IsDisabled)))
                                isdiabledchkbx.Checked = Convert.ToBoolean(item.IsDisabled);

                        }
                        catch (Exception ex)
                        {
                            Util.Log.ULog.WriteException(ex);
                        }
                        ddlMenuName.SelectedIndex = ddlMenuName.Items.IndexOf(ddlMenuName.Items.FindByValue(MenuType));

                        pEditorUserGroup.SetValues(item.AuthorizedToView);
                    }
                }

            }
            string currentDevExTheme = DevExpress.Web.ASPxWebControl.GlobalTheme;
            if (currentDevExTheme.ToLower() == "ugitclassic" || currentDevExTheme.ToLower() == "ugitclassicdevex")
            {
                CustomizeMenuFormat.Checked = false;
                CustomizeMenuFormat.Enabled = false;
                lblCustomizeMenuFormat.Visible = true;
                webformattingDIV.Visible = false;
            }
            else
            {
                CustomizeMenuFormat.Enabled = true;
                lblCustomizeMenuFormat.Visible = false;
                if (CustomizeMenuFormat.Checked == true)
                {
                    webformattingDIV.Visible = true;
                }
                else
                {
                    webformattingDIV.Visible = false;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            editMenuName.Visible = true;
            if (ddlMenuName.SelectedItem != null && (ddlMenuName.SelectedItem.Text.ToLower() == "default" || ddlMenuName.SelectedItem.Text.ToLower() == "mobile"))
            {
                editMenuName.Visible = false;
            }

            base.OnPreRender(e);
        }

        private void BindParent()
        {
            ddlParent.Items.Clear();
            string menuName = Convert.ToString(ddlMenuName.Value);

            if (menuName == "Default")
            {
                var queryEx = menuNavigationList.Where(x => x.MenuParentLookup == 0 && x.MenuName == MenuType).Select(x => new { x.Title, x.ID }).ToArray();
                
                foreach (var item in queryEx)
                {
                    ddlParent.Items.Add(new ListItem(item.Title, Convert.ToString(item.ID)));
                }
            }
            else if (menuName == "Mobile")
            {
                //queryMainMenu.Query = string.Format("<OrderBy><FieldRef Name ='{1}' Ascending='TRUE'/></OrderBy><Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>", DatabaseObjects.Columns.MenuParentLookup, DatabaseObjects.Columns.ItemOrder);
            }
            else
            {
                var queryEx = menuNavigationList.Where(x => x.MenuParentLookup == 0 && x.MenuName == MenuType).Select(x => new { x.Title, x.ID }).ToArray();
                foreach (var item in queryEx)
                {
                    ddlParent.Items.Add(new ListItem(item.Title, Convert.ToString(item.ID)));
                }
            }

            ddlParent.Items.Insert(0, new ListItem("-- Parent --", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (ddlParent.SelectedValue == "0")
            {
                lblError.Visible = false;
                foreach (ListItem lItem in ddlParent.Items)
                    if (lItem.Text.ToLower() == txtTitle.Text.Trim().ToLower() && lItem.Value != Id)
                    {
                        lblError.Visible = true;
                        return;
                    }

            }

            //SPFieldUserValueCollection usrVals = uHelper.GetFieldUserValueCollection(pEditorUserGroup.ResolvedEntities, SPContext.Current.Web);
            item.AuthorizedToView = pEditorUserGroup.GetValues();  // usrVals;
            item.Title = txtTitle.Text.Trim();
            if (!string.IsNullOrEmpty(txtItemOrder.Text))
                item.ItemOrder = Convert.ToInt32(txtItemOrder.Text);
            item.IconUrl = UGITFileUploadManager1.GetImageUrl();

            if (item.IconUrl.StartsWith(@"\")) //added to show icons
            {
                item.IconUrl = item.IconUrl.Substring(1);
            }

            //item.IconUrl = txtImageUrl.Text;
            //if (fileUploadIconImage.HasFile)
            //{
            //    SPSecurity.CodeToRunElevated uploadBGfiles = new SPSecurity.CodeToRunElevated(delegate
            //    {
            //        string fileName = fileUploadIconImage.FileName;
            //        string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
            //        string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
            //        while (File.Exists(path))
            //        {
            //            File.Delete(path);
            //        }
            //        fileUploadIconImage.SaveAs(path);
            //        txtImageUrl.Text = uploadFileURL;
            //        item[DatabaseObjects.Columns.UGITImageUrl] = uploadFileURL;
            //    });
            //    SPSecurity.RunWithElevatedPrivileges(uploadBGfiles);

            //}
            //else
            //{
            ///if (!string.IsNullOrEmpty(txtImageUrl.Text.Trim()))
          //  item.IconUrl = txtImageUrl.Text;
            //}

            item.NavigationUrl = txtNavigationUrl.Text;
            if (ddlParent.SelectedIndex != 0)
            {
                item.MenuParentLookup = Convert.ToInt64(ddlParent.SelectedValue);
            }
            else
                item.MenuParentLookup = 0; // string.Empty;
            item.MenuDisplayType = ddlDisplayType.SelectedItem.Text;
            item.NavigationType = ddlNavigationType.SelectedValue;

            string oldMenuName = string.Empty;
            string menuName = string.Empty;

            oldMenuName = Convert.ToString(item.MenuName);
            menuName = Convert.ToString(ddlMenuName.Value);
            item.MenuName = menuName;

            if (hdnMenuName.Value == "1" || hdnMenuName.Value == "2")
            {
                menuName = txtMenuName.Text.Trim();
            }

            if (menuName.ToLower() != "mobile" && menuName.ToLower() != "default")
                item.MenuName = menuName;


            if (ddlParent.SelectedIndex == 0)
            {
                item.SubMenuStyle = Convert.ToString(cbxMenuStyle.Value);
                if (cbxMenuStyle.SelectedIndex != 0)
                {
                    item.SubMenuItemPerRow = UGITUtility.StringToInt(seSubMenuItemsPerRow.Value);
                    item.SubMenuItemAlignment = Convert.ToString(cbxSubMenuTextAlignment.Value);
                }
                else
                {
                    item.SubMenuItemPerRow = 0;
                    item.SubMenuItemAlignment = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(ceFontcolor.Color.Name) && ceFontcolor.Color.Name != "0")
            {
                if (ceFontcolor.Color.IsKnownColor == false)
                    item.MenuFontColor = uHelper.TranslateColorCode(("#" + ceFontcolor.Color.Name).Trim(), System.Drawing.Color.White).Name;
                else
                    item.MenuFontColor = uHelper.TranslateColorCode((ceFontcolor.Color.Name).Trim(), System.Drawing.Color.White).Name;
            }
            else
                item.MenuFontColor = (uHelper.TranslateColorCode("#ffffff", System.Drawing.Color.Black)).Name; // System.Drawing.ColorTranslator.FromHtml("#ffffff");
            if (rListBackground.SelectedValue == "1")
            {
                if (!string.IsNullOrEmpty(ceBkColor.Color.Name) && ceBkColor.Color.Name != "0")
                {
                    if (ceBkColor.Color.IsKnownColor == false)
                        item.MenuBackground = uHelper.TranslateColorCode(("#" + ceBkColor.Color.Name).Trim(), System.Drawing.Color.Black).Name;
                    else
                        item.MenuBackground = uHelper.TranslateColorCode((ceBkColor.Color.Name).Trim(), System.Drawing.Color.Black).Name;
                }
                else
                    item.MenuBackground = uHelper.TranslateColorCode("#000000", System.Drawing.Color.Black).Name; //System.Drawing.ColorTranslator.FromHtml("#000000");
            }
            else
            {
                //if (fileUpload.HasFile)
                //{
                //    SPSecurity.CodeToRunElevated uploadBGfiles = new SPSecurity.CodeToRunElevated(delegate
                //    {
                //        string fileName = fileUpload.FileName;
                //        string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //        string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //        while (File.Exists(path))
                //        {
                //            File.Delete(path);
                //        }
                //        fileUpload.SaveAs(path);
                //        txtBkImgUrl.Text = uploadFileURL;
                //        item[DatabaseObjects.Columns.MenuBackground] = uploadFileURL;
                //    });
                //    SPSecurity.RunWithElevatedPrivileges(uploadBGfiles);

                //}
                //else
                //{
                //if (!string.IsNullOrEmpty(txtBkImgUrl.Text.Trim()))
                item.MenuBackground = ugitFileUpload.GetImageUrl();  // txtBkImgUrl.Text.Trim();
                //}
            }
            item.MenuHeight = Convert.ToInt32(spnBtnMenuHeight.Value);
            item.MenuWidth = Convert.ToInt32(spnBtnMenuWidth.Value);
            //item.MenuItemSeparation = txtMenuSeparation.Text.Trim();
            item.MenuItemSeparation = Convert.ToString(spnBtnMenuSeparation.Value);

            if (!(cbxMenuTextAlignment.Value == null || cbxMenuTextAlignment.SelectedIndex == -1))
                item.MenuTextAlignment = Convert.ToString(cbxMenuTextAlignment.Value);


            MenuNavigationProperties menuNavigation = new MenuNavigationProperties();
            menuNavigation.MenuFontFontFamily = Convert.ToString(cbxMenuFontFamily.Value);
            menuNavigation.MenuFontSize = Convert.ToString(spnBtnMenuFontSize.Value);
            menuNavigation.MenuIconAlignment = Convert.ToString(cbxMenuIconAlignment.Value);

            item.CustomProperties = uHelper.SerializeObject(menuNavigation).OuterXml;
            item.CustomizeFormat = CustomizeMenuFormat.Checked;
            item.IsDisabled = isdiabledchkbx.Checked;


            if (item.ID > 0)
            {
                menuNavigationMGR.Update(item);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated menu navigation: {item.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }
            else
            {
                menuNavigationMGR.Insert(item);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Added menu navigation: {item.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
            }

            if (ddlParent.SelectedIndex != 0)
            {
                //SPQuery queryChildMenu = new SPQuery();
                //queryChildMenu.Query = string.Format("<OrderBy><FieldRef Name ='{2}' Ascending='TRUE'/><FieldRef Name ='{3}' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuParentLookup, Convert.ToInt32(ddlParent.SelectedValue), DatabaseObjects.Columns.ItemOrder, DatabaseObjects.Columns.Modified);
                //SPListItemCollection spchildListItemcol = SPListHelper.GetSPListItemCollection(Convert.ToString(menuNavigationList), queryChildMenu);
                //DataTable dtchild = spchildListItemcol.GetDataTable();

                //if (dtchild != null || dtchild.Rows.Count > 0)
                //{
                //    ArrangeItemOrder(dtchild, Convert.ToString(menuNavigationList));
                //}
            }

            //if (menuNavigationList.Title == DatabaseObjects.Lists.MenuNavigation && hdnMenuName.Value == "2" && menuName != oldMenuName)
            //{
            //    SPQuery query = new SPQuery();
            //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuName, oldMenuName);
            //    SPListItemCollection spchildListItemcol = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.MenuNavigation, query);
            //    foreach (SPListItem mItem in spchildListItemcol)
            //    {
            //        mItem[DatabaseObjects.Columns.MenuName] = menuName;
            //        mItem.Update();
            //    }
            //}
            if (string.IsNullOrEmpty(MenuType))
                MenuType = !string.IsNullOrEmpty(txtMenuName.Text) ? txtMenuName.Text : (ddlMenuName != null && ddlMenuName.SelectedItem != null) ? ddlMenuName.SelectedItem.Text : "Default";

            Context.Cache.Add(string.Format("MenuTypEdit_{0}", User.Id), MenuType, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            Util.Cache.CacheHelper<object>.Clear();
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

        private static void ArrangeItemOrder(DataTable dtchild, string menuListName)
        {
            //SPList spList = SPListHelper.GetSPList(menuListName);
            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemOrder\">{3}</SetVar>" +
            // "</Method>";
            //StringBuilder query = new StringBuilder();
            //int ctr = 0;
            //foreach (DataRow dr in dtchild.Rows)
            //{
            //    query.AppendFormat(updateMethodFormat, dr[DatabaseObjects.Columns.Id], spList.ID, dr[DatabaseObjects.Columns.Id], ++ctr);
            //}

            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }

        //private int ValidateUser(PickerEntity entity, string Field)
        //{
        //    int userID = 0;
        //    int.TryParse(Convert.ToString(entity.EntityData["SPUserID"]), out userID);

        //    if (userID == 0)
        //    {
        //        SPUser user = UserProfile.GetUserByName(Convert.ToString(entity.EntityData["AccountName"]));
        //        if (user == null)
        //        {
        //            cvAuthorized.ErrorMessage = "Please enter a valid site user.";
        //            cvAuthorized.IsValid = false;

        //            return userID;
        //        }
        //        userID = user.ID;
        //    }
        //    return userID;
        //}

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            item = menuNavigationMGR.LoadByID(Convert.ToInt64(Id));
            if (item != null)
            {
                List<MenuNavigation> listMenuNavigation = menuNavigationMGR.Load(x=> x.MenuParentLookup==item.ID);
                if (listMenuNavigation != null && listMenuNavigation.Count > 0 )
                {
                    menuNavigationMGR.Delete(listMenuNavigation);
                }
                string MenuTitle = item.Title;
                menuNavigationMGR.Delete(item);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted menu navigation: {MenuTitle}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                Util.Cache.CacheHelper<object>.Clear();
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
        private void BindMenuNames()
        {
            //SPList menuNavigationList = SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation, spWeb);
            if (webMenuData != null)
            {
                webMenuData.DefaultView.RowFilter = string.Format("{0} is not null and {0} <> ''", DatabaseObjects.Columns.MenuName);
                webMenuData = webMenuData.DefaultView.ToTable(true, DatabaseObjects.Columns.MenuName);
                ddlMenuName.DataSource = webMenuData;
                ddlMenuName.TextField = DatabaseObjects.Columns.MenuName;
                ddlMenuName.ValueField = DatabaseObjects.Columns.MenuName;
                ddlMenuName.DataBind();
            }

            ddlMenuName.Items.Insert(0, new ListEditItem("Mobile", "Mobile"));
            ddlMenuName.Items.Insert(0, new ListEditItem("Default", "Default"));
        }
        private void BindMenuTextAlignmentTypes()
        {
            List<MenuNavigation> mlist = menuNavigationMGR.Load();  // spWeb.Lists[DatabaseObjects.Lists.MenuNavigation];
            List<string> mAlignment = mlist.Select(x => x.MenuTextAlignment).ToList(); // mlist.Fields.GetFieldByInternalName(DatabaseObjects.Columns.MenuTextAlignment);

            if (mAlignment.Count == 0)
                return;

            cbxMenuTextAlignment.Items.Clear();
            cbxMenuIconAlignment.Items.Clear();

            foreach (string choice in mAlignment)
            {
                cbxMenuTextAlignment.Items.Add(choice, choice);
                cbxMenuIconAlignment.Items.Add(choice, choice);
            }
        }

        protected void cvMenuName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hdnMenuName.Value == "1" || hdnMenuName.Value == "2")
            {
                if (txtMenuName.Text.Trim() == string.Empty)
                {
                    args.IsValid = false;
                    cvMenuName.ErrorMessage = "Please specify menu name.";
                }
                else if (txtMenuName.Text.Trim().ToLower() == "default" || txtMenuName.Text.Trim().ToLower() == "mobile")
                {
                    args.IsValid = false;
                    cvMenuName.ErrorMessage = "You can't enter default or mobile.";
                }

                if (!args.IsValid)
                {
                    ddlMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    txtMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "block");
                    addMenuName.Style.Add(HtmlTextWriterStyle.Display, "none");
                    editMenuName.Style.Add(HtmlTextWriterStyle.Display, "none");
                    cancelMenuName.Style.Add(HtmlTextWriterStyle.Display, "block");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(ddlMenuName.Value)))
                {
                    args.IsValid = false;
                    cvMenuName.ErrorMessage = "Please select menu name.";

                    ddlMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "block");
                    txtMenuNameDiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    addMenuName.Style.Add(HtmlTextWriterStyle.Display, "block");
                    editMenuName.Style.Add(HtmlTextWriterStyle.Display, "block");
                    cancelMenuName.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
            }
        }

        protected void ddlMenuName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindParent();
        }

        protected void CustomizeMenuFormat_CheckedChanged(object sender, EventArgs e)
        {
            isCustomizationClicked = CustomizeMenuFormat.Checked;
            if (CustomizeMenuFormat.Checked)
            {
                if (ddlParent.SelectedIndex > 0)
                {

                    int parentId = Convert.ToInt32(ddlParent.SelectedValue);
                    //SPQuery queryMainMenu = new SPQuery();

                    //List<string> queryEx = new List<string>();
                    //queryEx.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.Id, parentId));
                    //queryMainMenu.Query = string.Format("<Where>{0}</Where><OrderBy><FieldRef Name ='{1}' Ascending='TRUE'/></OrderBy>", uHelper.GenerateWhereQueryWithAndOr(queryEx, true), DatabaseObjects.Columns.ItemOrder);
                    DataTable spMainMenuListItemCol = menuNavigationMGR.GetDataTable(); // SPListHelper.GetSPListItemCollection(Convert.ToString(menuNavigationList), queryMainMenu);

                    foreach (DataRow item in spMainMenuListItemCol.Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.MenuFontColor])))
                        {
                            ceFontcolor.Value = uHelper.TranslateColorCode(Convert.ToString(item[DatabaseObjects.Columns.MenuFontColor]), System.Drawing.Color.White); //System.Drawing.ColorTranslator.FromHtml(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.MenuFontColor)));
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.MenuHeight])))
                        {
                            spnBtnMenuHeight.Value = Convert.ToString(item[DatabaseObjects.Columns.MenuHeight]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.MenuWidth])))
                        {
                            spnBtnMenuWidth.Value = Convert.ToString(item[DatabaseObjects.Columns.MenuWidth]);
                        }

                        if (!Convert.ToString(item[DatabaseObjects.Columns.MenuBackground]).Contains("."))
                        {
                            rListBackground.SelectedValue = "1";
                            ceBkColor.CssClass = ceBkColor.CssClass.Replace("hide", "");
                            ceBkColor.Color = uHelper.TranslateColorCode(Convert.ToString(item[DatabaseObjects.Columns.MenuBackground]), System.Drawing.Color.Black);
                            if (UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.CustomizeFormat]))
                                isCustomizationClicked = false;
                        }
                        else
                        {
                            rListBackground.SelectedValue = "2";
                            //txtBkImgUrl.CssClass = txtBkImgUrl.CssClass.Replace("hide", "");
                            //fileUpload.CssClass = fileUpload.CssClass.Replace("hide", "");
                            //lnkbackgroundimage.CssClass = lnkbackgroundimage.CssClass.Replace("hide", "");
                            //txtBkImgUrl.Text = Convert.ToString(item[DatabaseObjects.Columns.MenuBackground]);
                            ugitFileUpload.SetImageUrl(Convert.ToString(item[DatabaseObjects.Columns.MenuBackground]));
                        }

                        if (!(item[DatabaseObjects.Columns.MenuItemSeparation] == null || Convert.ToString(item[DatabaseObjects.Columns.MenuItemSeparation]) == ""))
                            spnBtnMenuSeparation.Value = Convert.ToInt32(item[DatabaseObjects.Columns.MenuItemSeparation]);

                        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.CustomProperties])))
                        {
                            XmlDocument obj = new XmlDocument();
                            obj.LoadXml(Convert.ToString(item[DatabaseObjects.Columns.CustomProperties]));
                            MenuNavigationProperties menuNavigationprop = new MenuNavigationProperties();
                            menuNavigationprop = (MenuNavigationProperties)uHelper.DeSerializeAnObject(obj, menuNavigationprop);
                            if (menuNavigationprop != null)
                            {
                                if (menuNavigationprop.MenuFontSize != "")
                                    spnBtnMenuFontSize.Value = Convert.ToUInt16(menuNavigationprop.MenuFontSize);
                                if (menuNavigationprop.MenuFontFontFamily != "")
                                    cbxMenuFontFamily.Value = Convert.ToString(menuNavigationprop.MenuFontFontFamily);

                                if (menuNavigationprop.MenuIconAlignment != "")
                                    cbxMenuIconAlignment.Value = Convert.ToString(menuNavigationprop.MenuIconAlignment);

                            }
                        }
                        //if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.MenuTextAlignment])))
                        //    cbxMenuTextAlignment.Value = Convert.ToString(item[DatabaseObjects.Columns.MenuTextAlignment]);
                        if (ddlDisplayType.SelectedValue.ToLower() == MenuTitleDisplayType.IconOnly.ToLower())
                            WebMenuTextAlignmentTr.Visible = false;
                        if (ddlDisplayType.SelectedValue.ToLower() == MenuTitleDisplayType.TitleOnly.ToLower())
                            WebMenuIconAlignment.Visible = false;
                    }
                }
                else
                {
                    //bind for parent
                    WebSbMenuStyleTr.Style.Remove(HtmlTextWriterStyle.Display);
                    trSubMenuItemsPerRow.Style.Remove(HtmlTextWriterStyle.Display);
                    spnBtnMenuSeparation.Value = 1;
                    spnBtnMenuFontSize.Value = UGITUtility.StringToInt(Constants.DefaultMenuFontSize);
                    cbxMenuFontFamily.Value = Convert.ToString("Default");
                    cbxMenuTextAlignment.Value = Convert.ToString("Center");
                    cbxMenuIconAlignment.Value = Convert.ToString("Center");
                }
            }
            else
            {
                spnBtnMenuHeight.Value = null;
                spnBtnMenuWidth.Value = null;
                spnBtnMenuSeparation.Value = 1;
                spnBtnMenuFontSize.Value = UGITUtility.StringToInt(Constants.DefaultMenuFontSize);
                cbxMenuFontFamily.Value = Convert.ToString("Default");
                cbxMenuTextAlignment.Value = Convert.ToString("Center");
                cbxMenuIconAlignment.Value = Convert.ToString("Center");
            }
        }

        protected void ddlDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            trIconUrl.Visible = false;
            WebMenuTextAlignmentTr.Visible = false;
            if (ddlParent.SelectedIndex == 0)
            {
                WebSbMenuStyleTr.Style.Remove(HtmlTextWriterStyle.Display);
                trSubMenuItemsPerRow.Style.Remove(HtmlTextWriterStyle.Display);
            }
            else
            {
                WebSbMenuStyleTr.Style.Add(HtmlTextWriterStyle.Display, "none");
                trSubMenuItemsPerRow.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (ddlDisplayType.SelectedValue == "icononly")
            {
                trIconUrl.Visible = true;
                WebMenuTextAlignmentTr.Visible = false;
                WebMenuIconAlignment.Visible = true;

            }
            else if (ddlDisplayType.SelectedValue == "both")
            {
                trIconUrl.Visible = true;
                WebMenuTextAlignmentTr.Visible = true;
                WebMenuIconAlignment.Visible = true;
            }
            else if (ddlDisplayType.SelectedValue == "titleonly")
            {
                trIconUrl.Visible = false;
                WebMenuTextAlignmentTr.Visible = true;
                WebMenuIconAlignment.Visible = false;
            }
        }



    }
}
