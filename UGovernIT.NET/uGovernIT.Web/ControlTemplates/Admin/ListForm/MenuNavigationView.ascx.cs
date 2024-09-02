using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class MenuNavigationView : UserControl
    {
        public string editMenuNavigation = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/uGovernITConfiguration.aspx?control=editmenunavigation");
        public string requestUrl = string.Empty;
        string listName = string.Empty;
        public bool isShowHomeTab = true;
        public bool isShowAdminTab = true;
        private DataTable webMenuData;
        protected bool isDefaultMenuHighlighter = true;
        protected string ajaxHelperPage = string.Empty;
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
            requestUrl = Request.Url.AbsoluteUri;
            //chkbxHideHomeTab.Checked = uGITCache.GetConfigVariableValueAsBool(ConfigConstants.HideHomeMenuItem, SPContext.Current.Web);

            //SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.MenuName);
            //query.Query = string.Format("<Where></Where>");
            webMenuData = menuNavigationMGR.GetDataTable();  // SPListHelper.GetDataTable(DatabaseObjects.Lists.MenuNavigation, query, SPContext.Current.Web);
            ConfigurationVariable objUgit = configMGR.LoadVaribale(ConfigConstants.UgitTheme);   //.Load(SPContext.Current.Web, ConfigConstants.UgitTheme);
            if (objUgit != null)
            {
                string uGitTheme = objUgit.KeyValue;
                if (!string.IsNullOrEmpty(uGitTheme))
                {
                    XmlDocument obj = new XmlDocument();
                    obj.LoadXml(uGitTheme);
                    UGITTheme objUgitTheme = new UGITTheme();
                    objUgitTheme = (UGITTheme)uHelper.DeSerializeAnObject(obj, objUgitTheme);
                    if (objUgitTheme != null)
                    {
                        if (!string.IsNullOrEmpty(objUgitTheme.MenuHighlightColor))
                        {
                            isDefaultMenuHighlighter = false;
                            objUgitTheme.MenuHighlightColor = System.Drawing.ColorTranslator.ToHtml(uHelper.TranslateColorCode(objUgitTheme.MenuHighlightColor, System.Drawing.Color.Orange));
                            menuHightlightColorPicker.Color = uHelper.TranslateColorCode(objUgitTheme.MenuHighlightColor, System.Drawing.Color.Orange);
                        }
                    }
                }
            }
           // menuHightlightColorPicker.CssClass = "textss";
          
        }

        private void BindMenuGrid(string menuName)
        {
            string cbxselectedval = Convert.ToString(cbxMenuNavigationType.Value);
            DataTable dtMenu = new DataTable();

            //SPQuery queryMainMenu = new SPQuery();
            //List<string> queryEx = new List<string>();
            ////if (cbxselectedval == "Default")
            //{
            //    listName = DatabaseObjects.Lists.MenuNavigation;
            //    queryEx.Add(string.Format("<Or><IsNull><FieldRef Name='{0}'/></IsNull><Eq><FieldRef Name='{0}'/><Value Type='Text'></Value></Eq></Or>", DatabaseObjects.Columns.MenuName));
            //    queryEx.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'></Value></Eq>", DatabaseObjects.Columns.MenuName));
            //    queryMainMenu.Query = string.Format("<Where><And>{0}<IsNull><FieldRef Name='{2}'/></IsNull></And></Where><OrderBy><FieldRef Name ='{1}' Ascending='TRUE'/></OrderBy>", uHelper.GenerateWhereQueryWithAndOr(queryEx, false), DatabaseObjects.Columns.ItemOrder, DatabaseObjects.Columns.MenuParentLookup);

            //}
            ////else if (cbxselectedval == "Mobile")
            //{
            //    listName = DatabaseObjects.Lists.MenuNavigationMobile;
            //    queryMainMenu.Query = string.Format("<OrderBy><FieldRef Name ='{1}' Ascending='TRUE'/></OrderBy><Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>", DatabaseObjects.Columns.MenuParentLookup, DatabaseObjects.Columns.ItemOrder);
            //}
            //else
            //{
            //    listName = DatabaseObjects.Lists.MenuNavigation;
            //    queryEx.Add(string.Format("<Eq><FieldRef Name='{1}'/><Value Type='Text'>{0}</Value></Eq>", menuName, DatabaseObjects.Columns.MenuName));
            //    queryMainMenu.Query = string.Format("<Where><And>{0}<IsNull><FieldRef Name='{2}'/></IsNull></And></Where><OrderBy><FieldRef Name ='{1}' Ascending='TRUE'/></OrderBy>", uHelper.GenerateWhereQueryWithAndOr(queryEx, true), DatabaseObjects.Columns.ItemOrder, DatabaseObjects.Columns.MenuParentLookup);
            //}

            List<MenuNavigation> lstMenuNavigation = menuNavigationMGR.LoadMenuNavigation(cbxselectedval).OrderBy(x => x.ItemOrder).ToList(); //SPListItemCollection spMainMenuListItemCol = SPListHelper.GetSPListItemCollection(listName, queryMainMenu);
            DataTable dtMainMenu = menuNavigationMGR.GetDataTable();
            if (lstMenuNavigation != null && lstMenuNavigation.Count > 0)
            {
                //DataTable dtMainMenu = spMainMenuListItemCol.GetDataTable();
                dtMainMenu.Columns.Add("ParentID", typeof(int));
                dtMenu = dtMainMenu.Clone();

                foreach (DataRow row in dtMainMenu.Rows)
                {
                    row["ParentID"] = 0;
                    dtMenu.ImportRow(row);
                    //queryEx = new List<string>();
                    //SPQuery querySubMenu = new SPQuery();
                    //queryEx.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.MenuParentLookup, row["Title"]));
                    //if (cbxselectedval.ToLower()!="mobile")
                    //{
                    // if (cbxselectedval.ToLower() != "default")
                    //    queryEx.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.MenuName, menuName));
                    //else
                    //    queryEx.Add(string.Format("<Or><IsNull><FieldRef Name='{0}'/></IsNull><Eq><FieldRef Name='{0}'/><Value Type='Text'></Value></Eq></Or>", DatabaseObjects.Columns.MenuName));
                    
                    //}
                   
                    //querySubMenu.Query = string.Format("<OrderBy><FieldRef Name = '{0}' Ascending='TRUE'/></OrderBy><Where>{1}</Where>", DatabaseObjects.Columns.ItemOrder, uHelper.GenerateWhereQueryWithAndOr(queryEx, true));
                    //SPListItemCollection spSubMenuListItemCol = SPListHelper.GetSPListItemCollection(listName, querySubMenu);

                    //DataTable dtSubMenu = spSubMenuListItemCol.GetDataTable();
                    //if (dtSubMenu != null)
                    //{
                    //    dtSubMenu.Columns.Add("ParentID");
                    //    foreach (DataRow subrow in dtSubMenu.Rows)
                    //    {
                    //        subrow["ParentID"] = row["ID"];
                    //        dtMenu.ImportRow(subrow);
                    //    }
                    //}
                }
            }

            treeList.DataSource = lstMenuNavigation;
            treeList.DataBind();
            treeList.ExpandAll();
            //
        }

        protected override void OnLoad(EventArgs e)
        {
            ajaxHelperPage = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ajaxHelperPage);
            string mName = Convert.ToString(cbxMenuNavigationType.Value);
            if (!IsPostBack)
            {
                BindMenuNames();
                cbxMenuNavigationType.Value = "Default";
                string cacheVal = Convert.ToString(Context.Cache.Get(string.Format("MenuTypEdit_{0}", User.Id)));
                if (!string.IsNullOrWhiteSpace(cacheVal))
                {
                    Context.Cache.Remove(string.Format("MenuTypEdit_{0}", User.Id));
                    cbxMenuNavigationType.Value = cacheVal;
                }

               
                
            }
            BindMenuGrid(Convert.ToString(cbxMenuNavigationType.Value));
            divMenuType.Visible = true;
            

            //base.OnLoad(e);
        }

        void UpdateSequence(string firstID, string secondID)
        {
            if (secondID == "")
                return;

            //DataTable dtMainMenu = SPListHelper.GetDataTable(listName);
            //DataRow[] drFirstItem = dtMainMenu.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, Convert.ToInt32(firstID.Trim())));
            //DataRow[] drSecondItem = dtMainMenu.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, Convert.ToInt32(secondID.Trim())));

            //if (Convert.ToString(drFirstItem[0][DatabaseObjects.Columns.MenuParentLookup]) != "")
            //{
            //    SPQuery queryMainMenu = new SPQuery();
            //    queryMainMenu.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Numeric'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, firstID);
            //    SPListItemCollection spMainMenuListItemCol = SPListHelper.GetSPListItemCollection(listName, queryMainMenu);
            //    SPListItem spmenulistitem = SPListHelper.GetItemByID(spMainMenuListItemCol, Convert.ToInt32(firstID));

            //    if (spmenulistitem != null)
            //    {
            //        if (Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]) != "")
            //        {
            //            SPQuery queryParentMainMenu = new SPQuery();
            //            queryParentMainMenu.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]));
            //            SPListItemCollection spParentMainMenuListItemCol = SPListHelper.GetSPListItemCollection(listName, queryParentMainMenu);

            //            //AllowUnsafeUpdates
            //            SPContext.Current.Web.AllowUnsafeUpdates = true;
            //            spmenulistitem[DatabaseObjects.Columns.MenuParentLookup] = spParentMainMenuListItemCol[0].ID;
            //            if (Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]) != "")
            //                if (Convert.ToInt32(drSecondItem[0][DatabaseObjects.Columns.ItemOrder]) < Convert.ToInt32(drFirstItem[0][DatabaseObjects.Columns.ItemOrder]))
            //                    spmenulistitem[DatabaseObjects.Columns.ItemOrder] = Convert.ToInt32(drSecondItem[0][DatabaseObjects.Columns.ItemOrder]) - 1;
            //                else
            //                    spmenulistitem[DatabaseObjects.Columns.ItemOrder] = Convert.ToInt32(drSecondItem[0][DatabaseObjects.Columns.ItemOrder]) + 1;
            //            else
            //                spmenulistitem[DatabaseObjects.Columns.ItemOrder] = 1;
            //            spmenulistitem.Update();
            //            SPContext.Current.Web.AllowUnsafeUpdates = false;
            //            //}
            //        }
            //        else
            //        {
            //            SPContext.Current.Web.AllowUnsafeUpdates = true;
            //            spmenulistitem[DatabaseObjects.Columns.MenuParentLookup] = secondID;
            //            spmenulistitem[DatabaseObjects.Columns.ItemOrder] = 0;
            //            spmenulistitem.Update();
            //            SPContext.Current.Web.AllowUnsafeUpdates = false;
            //        }
            //    }

            //    //update itemorder seq...
            //    SPQuery queryfirstChildMenu = new SPQuery();
            //    queryfirstChildMenu.Query = string.Format("<OrderBy><FieldRef Name ='{2}' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuParentLookup, Convert.ToString(drFirstItem[0][DatabaseObjects.Columns.MenuParentLookup]), DatabaseObjects.Columns.ItemOrder);
            //    SPListItemCollection spfirstchildListItemcol = SPListHelper.GetSPListItemCollection(listName, queryfirstChildMenu);
            //    DataTable dtfirstchild = spfirstchildListItemcol.GetDataTable();

            //    if (dtfirstchild != null && dtfirstchild.Rows.Count > 0)
            //    {
            //        ArrangeItemOrder(dtfirstchild, listName);
            //    }

            //    if (Convert.ToString(drFirstItem[0][DatabaseObjects.Columns.MenuParentLookup]) != Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]))
            //    {
            //        SPListItemCollection spchildListItemcol;
            //        if (Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]) != "")
            //        {
            //            SPQuery queryChildMenu = new SPQuery();
            //            queryChildMenu.Query = string.Format("<OrderBy><FieldRef Name ='{2}' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuParentLookup, Convert.ToString(drSecondItem[0][DatabaseObjects.Columns.MenuParentLookup]), DatabaseObjects.Columns.ItemOrder);
            //            spchildListItemcol = SPListHelper.GetSPListItemCollection(listName, queryChildMenu);
            //        }
            //        else
            //        {
            //            SPQuery queryChildMenu = new SPQuery();
            //            queryChildMenu.Query = string.Format("<OrderBy><FieldRef Name ='{2}' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuParentLookup, secondID, DatabaseObjects.Columns.ItemOrder);
            //            spchildListItemcol = SPListHelper.GetSPListItemCollection(listName, queryChildMenu);
            //        }
            //        DataTable dtchild = spchildListItemcol.GetDataTable();
            //        if (dtchild != null && dtchild.Rows.Count > 0)
            //        {
            //            ArrangeItemOrder(dtchild, listName);
            //        }
            //    }
            //}
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

        protected void treeList_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            string mName = Convert.ToString(cbxMenuNavigationType.Value);

            //if (Object.Equals(e.GetValue("MenuParentLookup"), 0))
            //{
            //    e.Row.Font.Bold = true;
            //    e.Row.Cells[2].Text = string.Empty;

            //}

            var tree = sender as ASPxTreeList;
            if (tree == null) return;

            var hyperLink = tree.FindDataCellTemplateControl(e.NodeKey, null, "titleLinkButton") as HyperLink;
            var editHyperLink = tree.FindDataCellTemplateControl(e.NodeKey, null, "editLinkButton") as HyperLink;
            if (e.RowKind == TreeListRowKind.Data)
            {
                if (hyperLink != null)
                {
                    string editItem = string.Empty;
                    if (mName == "Mobile")
                    {
                        editItem = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=editmenunavigation&type={0}&ItemId=" + e.NodeKey, "Mobile"));
                    }
                    else
                    {
                        editItem = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=editmenunavigation&type={0}&ItemId=" + e.NodeKey, mName));
                    }
                    string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','Menu - {2}','600','800',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsoluteUri), "Edit Item");

                    hyperLink.NavigateUrl = Url;
                    hyperLink.Text = Convert.ToString(e.GetValue("Title"));
                }

                if (editHyperLink != null)
                {
                    string editItem = string.Empty;
                    if (mName == "Mobile")
                    {
                        editItem = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=editmenunavigation&type={0}&ItemId=" + e.NodeKey, "Mobile"));
                    }
                    else
                    {
                        editItem = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=editmenunavigation&type={0}&ItemId=" + e.NodeKey, mName));
                    }
                    string Url = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Menu - {2}','600','865',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsoluteUri), "Edit Item");

                    editHyperLink.NavigateUrl = Url;
                }

            }

            if (Convert.ToInt64(e.GetValue("MenuParentLookup")) == 0)
            {
                e.Row.Font.Bold = true;
                //e.Row.Cells[2].Text = string.Empty;
                hyperLink.Font.Bold = true;
            }
        }

        protected void treeList_ProcessDragNode(object sender, DevExpress.Web.ASPxTreeList.TreeListNodeDragEventArgs e)
        {
            UpdateSequence(Convert.ToString(e.Node["ID"]), Convert.ToString(e.NewParentNode["ID"]));
            e.Handled = true;
            string mtype = Convert.ToString(cbxMenuNavigationType.Value);
            BindMenuGrid(mtype);
        }

        protected void rbtnWeb_CheckedChanged(object sender, EventArgs e)
        {
            string showType = "web";
            string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showtype={2}", "menunavigationview", "Menu Navigation", showType));
            Response.Redirect(url);
        }

        protected void rbtnMobile_CheckedChanged(object sender, EventArgs e)
        {
            string showType = "mob";
            string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showtype={2}", "menunavigationview", "Menu Navigation", showType));
            Response.Redirect(url);
        }

        //protected void chkbxHideHomeTab_CheckedChanged(object sender, EventArgs e)
        //{


        //    ConfigurationVariable updateADUserCredentail = ConfigurationVariable.Load(ConfigConstants.HideHomeMenuItem);
        //    updateADUserCredentail.KeyValue = Convert.ToString(chkbxHideHomeTab.Checked);
        //    updateADUserCredentail.Save();

        //    //uGITCache.RefreshList(DatabaseObjects.Lists.ConfigurationVariable);
        //}


        private void BindMenuNames()
        {
            //SPList menuNavigationList = SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation, spWeb);
            if (webMenuData != null)
            {
                webMenuData.DefaultView.RowFilter = string.Format("{0} is not null and {0} <> ''", DatabaseObjects.Columns.MenuName);
                webMenuData = webMenuData.DefaultView.ToTable(true, DatabaseObjects.Columns.MenuName);
                cbxMenuNavigationType.DataSource = webMenuData;
                cbxMenuNavigationType.TextField = DatabaseObjects.Columns.MenuName;
                cbxMenuNavigationType.ValueField = DatabaseObjects.Columns.MenuName;
                cbxMenuNavigationType.DataBind();
            }

            cbxMenuNavigationType.Items.Insert(0, new ListEditItem("Mobile", "Mobile"));
            if(cbxMenuNavigationType.Items.FindByText("Default") == null)
                cbxMenuNavigationType.Items.Insert(0, new ListEditItem("Default", "Default"));
        }

        protected void cbxMenuNavigationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mName = Convert.ToString(cbxMenuNavigationType.SelectedItem.Value);
            Util.Cache.CacheHelper<object>.Clear();
            BindMenuGrid(mName);
        }

        protected void btCreateMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMenuName.Text.Trim()))
                return;

            string oldChoice = Convert.ToString(cbxMenuNavigationType.Value);
            string newValue = txtMenuName.Text.Trim();

            if (newValue.ToLower() == "default" || newValue.ToLower() == "mobile")
            {
                spnerror.Visible = true;
                return;
            }

            //SPList menuList = SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation);
            //SPFieldChoice spChoice = (SPFieldChoice)menuList.Fields.GetField(DatabaseObjects.Columns.MenuName);
            //if (spChoice.Choices.Contains(txtMenuName.Text.Trim()))
            //{
            //    spnerror.Visible = true;
            //    return;
            //}

            BindMenuNames();
        }

        protected void BtSaveMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMenuName.Text.Trim()))
                return;

            if (Convert.ToString(cbxMenuNavigationType.Value).ToLower() == txtMenuName.Text.Trim().ToLower())
            {
                navigationMenuPopup.ShowOnPageLoad = false;
                return;
            }


            string oldChoice = Convert.ToString(cbxMenuNavigationType.Value);
            string newValue = txtMenuName.Text.Trim();

            if (newValue.ToLower() == "default" || newValue.ToLower() == "mobile")
            {
                spnerror.Visible = true;
                return;
            }

            //SPList menuList = SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation);
            //SPFieldChoice spChoice = (SPFieldChoice)menuList.Fields.GetField(DatabaseObjects.Columns.MenuName);
            //if (spChoice.Choices.Contains(txtMenuName.Text.Trim()))
            //{
            //    spnerror.Visible = true;
            //    return;
            //}


            //menuList = SPListHelper.GetSPList(DatabaseObjects.Lists.MenuNavigation);
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.MenuName, oldChoice);
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.MenuName);
            //SPListItemCollection collection = SPListHelper.GetSPListItemCollection(menuList.Title, query, SPContext.Current.Web);
            //foreach (SPListItem item in collection)
            //{
            //    item[DatabaseObjects.Columns.MenuName] = newValue;
            //    item.SystemUpdate();
            //}

            string menunavigationtype = Convert.ToString(cbxMenuNavigationType.SelectedItem.Value);

            BindMenuGrid(menunavigationtype);
        }

        public void btnRefreshCache_Click(object sender, EventArgs e)
        {
            //uGITCache.RefreshMenu();
            string menunavigationtype = Convert.ToString(cbxMenuNavigationType.SelectedItem.Value);
            BindMenuGrid(menunavigationtype);
            string uGitTheme = configMGR.GetValue(ConfigConstants.UgitTheme);
            if (!string.IsNullOrEmpty(uGitTheme))
            {
                XmlDocument obj = new XmlDocument();
                obj.LoadXml(uGitTheme);
                UGITTheme objUgitTheme = new UGITTheme();
                objUgitTheme = (UGITTheme)uHelper.DeSerializeAnObject(obj, objUgitTheme);
                if (objUgitTheme != null)
                {
                    if (!string.IsNullOrEmpty(objUgitTheme.MenuHighlightColor))
                    {
                        isDefaultMenuHighlighter = false;
                        objUgitTheme.MenuHighlightColor = System.Drawing.ColorTranslator.ToHtml(uHelper.TranslateColorCode(objUgitTheme.MenuHighlightColor, System.Drawing.Color.Orange));
                        menuHightlightColorPicker.Color = uHelper.TranslateColorCode(objUgitTheme.MenuHighlightColor, System.Drawing.Color.Orange);
                    }
                    else
                    {
                        objUgitTheme.MenuHighlightColor = System.Drawing.ColorTranslator.ToHtml(menuHightlightColorPicker.Color);
                        //bool unsafeUpdate = SPContext.Current.Web.AllowUnsafeUpdates;

                        ConfigurationVariable cnfigVariable = configMGR.LoadVaribale(ConfigConstants.UgitTheme);
                        cnfigVariable.KeyName = ConfigConstants.UgitTheme;
                        cnfigVariable.KeyValue = uHelper.SerializeObject(objUgitTheme).OuterXml;
                        configMGR.Update(cnfigVariable);
                    }
                }
            }
            Util.Cache.CacheHelper<object>.Delete($"MenuNavigation", context.TenantID);
        }
    }
}
