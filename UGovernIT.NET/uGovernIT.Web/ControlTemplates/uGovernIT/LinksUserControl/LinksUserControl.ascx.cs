using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class LinksUserControl : UserControl
    {        
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public UserProfileManager UserManager = null;
        public ConfigurationVariableManager configurationVariableManager = null;
        LinkViewManager linkViewManager = null;
        public string LinkView { get; set; }
        public string ControlWidth { get; set; }
        public bool HideControlBorder { get; set; }

        DataTable dataTableCategory;
        DataTable dataTableLinkItems;
        DataTable lstLinkCategory;
        DataTable lstLinkItems;
        string analyticUrl = string.Empty;
        protected string editLinkViewPath = string.Empty;

        UserProfile currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            configurationVariableManager = new ConfigurationVariableManager(context);
            linkViewManager = new LinkViewManager(context);
            currentUser = HttpContext.Current.CurrentUser();


            if (string.IsNullOrEmpty(ControlWidth) || Convert.ToUInt32(ControlWidth) < 300)
                ControlWidth = "830";

            editLinkViewPath = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=linkconfig&viewLink={0}", LinkView));
            if (UserManager.IsAdmin(context.CurrentUser) || WikiArticleHelper.IsWikiOwner(context.CurrentUser))
                imgLinkEditMessage.Visible = true;
            else
                imgLinkEditMessage.Visible = false;

            //changes in case of multiple control on same page..
            hdnEditLinkViewPath.Value = editLinkViewPath;

            long linkId = 0;
            if (!string.IsNullOrEmpty(LinkView))
                long.TryParse(LinkView, out linkId);

            if (linkId > 0)
            {
                LinkView splstItemViewLink = linkViewManager.LoadByID(linkId);
                if (splstItemViewLink != null)
                    lblViewHeader.Text = Convert.ToString(splstItemViewLink.Title);
            }

            analyticUrl = UGITUtility.GetAbsoluteURL(configurationVariableManager.GetValue(ConfigConstants.AnalyticUrl));
            string licenseUrl = string.Empty;
            string serviceurl = string.Empty;
            string facttablUrl = string.Empty;
            string cacheurl = string.Empty;
            string dashboardurl = string.Empty;

            lstLinkCategory = GetTableDataManager.GetTableData(DatabaseObjects.Tables.LinkCategory, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");  //SPListHelper.GetSPList(DatabaseObjects.Tables.LinkCategory);
            lstLinkItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.LinkItems, $"{DatabaseObjects.Columns.Deleted} = 0 and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");    //SPListHelper.GetSPList(DatabaseObjects.Tables.LinkItems);

            dataTableLinkItems = lstLinkItems.Clone();
            foreach (DataRow item in lstLinkItems.Rows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView])))
                {
                    dataTableLinkItems.ImportRow(item);
                }
                else if (Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]).Contains(currentUser.Id) || UserManager.IsUserinGroups(Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]), currentUser.UserName) || UserManager.IsAdmin(currentUser))
                {                    
                    dataTableLinkItems.ImportRow(item);
                }                
            }


            DataTable linkItemCategories = null;
            if (dataTableLinkItems != null)
            {
                dataTableLinkItems.DefaultView.RowFilter = string.Format("{0} Is Not null or {0} =''", DatabaseObjects.Columns.LinkCategoryLookup);
                dataTableLinkItems.DefaultView.Sort = DatabaseObjects.Columns.ItemOrder;
                linkItemCategories = dataTableLinkItems.DefaultView.ToTable(true, DatabaseObjects.Columns.LinkCategoryLookup);
                dataTableLinkItems.DefaultView.RowFilter = string.Empty;
            }

            if (lstLinkCategory != null)
            {
                if (linkId > 0)
                    dataTableCategory = lstLinkCategory.Select($"{DatabaseObjects.Columns.LinkViewLookup} = {linkId}").CopyToDataTable();
                else
                    dataTableCategory = lstLinkCategory;
            }

            if (linkItemCategories == null || linkItemCategories.Rows.Count == 0)
                dataTableCategory = null;

            //filter category having item inside
            if (dataTableCategory != null && linkItemCategories != null)
            {
                var categories = (from a in dataTableCategory.AsEnumerable()
                                  join b in linkItemCategories.AsEnumerable() on a.Field<long>(DatabaseObjects.Columns.ID)
                                  equals b.Field<long>(DatabaseObjects.Columns.LinkCategoryLookup)
                                  select a).ToArray();

                if (categories != null && categories.Length > 0)
                    dataTableCategory = categories.CopyToDataTable();
                else
                    dataTableCategory = null;
            }

            //hide black box if there is no links to show and user is not admin
            if (dataTableCategory == null && !UserManager.IsUGITSuperAdmin(context.CurrentUser))
                authorizedPanel.Visible = false;


            RptSubCategory.DataSource = dataTableCategory;
            RptSubCategory.DataBind();
        }

        protected void RptSubCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Label lblSubCategory = (Label)e.Item.FindControl("LblSubCategroy");
                HiddenField hdnCategroyId = (HiddenField)e.Item.FindControl("hdnCategroyId");
                Repeater rptListItem = (Repeater)e.Item.FindControl("RptItemList");
                DataRowView dataView = e.Item.DataItem as DataRowView;

                DataTable linkItems = null;
                if (dataTableLinkItems != null)
                {
                    string Id = Convert.ToString(dataView[DatabaseObjects.Columns.ID]);
                    dataTableLinkItems.DefaultView.RowFilter = string.Format("{0}={1}", DatabaseObjects.Columns.LinkCategoryLookup, Id);
                    linkItems = dataTableLinkItems.DefaultView.ToTable();
                    dataTableLinkItems.DefaultView.RowFilter = string.Empty;
                }
                if (linkItems == null || linkItems.Rows.Count == 0)
                    rptListItem.Visible = false;

                rptListItem.DataSource = linkItems;
                rptListItem.DataBind();
            }
        }

        protected void RptItemList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string absPath = string.Empty;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem gvRow = e.Item;
                DataRowView row = (DataRowView)e.Item.DataItem;
                Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(Convert.ToString(row[DatabaseObjects.Columns.CustomProperties]), Constants.Separator);

                LinkButton link = (LinkButton)e.Item.FindControl("LnkListName");
                string targetType = Convert.ToString(row[DatabaseObjects.Columns.TargetType]);
                string url = string.Empty;
                switch (targetType)
                {
                    case "Control":
                        {
                            KeyValuePair<string, string> pageParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "fileurl");
                            KeyValuePair<string, string> controlParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "control");
                            url = UGITUtility.GetAbsoluteURL(pageParam.Value);
                            url = string.Format(url + "?control={0}&isreadonly=True", controlParam.Value);
                            link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','80',true);", url, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                        }
                        break;
                    case "Dashboard":
                        {
                            KeyValuePair<string, string> modelId = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "modelid");
                            KeyValuePair<string, string> dashBoardId = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "dashboardid");
                            url = string.Format("{0}/runs/QuickSurveyResult?modelID={1}&resultType=True&dashboardId={2}&relativeRunID=0&IsDlg=true&popupId=dashboardRun", analyticUrl, modelId.Value, dashBoardId.Value);
                            link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','80',true);", url, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                        }
                        break;
                    case "Wiki":
                        {
                            url = UGITUtility.GetAbsoluteURL(Convert.ToString(row[DatabaseObjects.Columns.CustomProperties]).Replace("fileurl=", ""));
                            string Strparam = string.Empty;
                            if (UGITUtility.IfColumnExists(row.Row, DatabaseObjects.Columns.DisableLinks))
                                Strparam += "DisableLinks=" + UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.DisableLinks]));

                            if (UGITUtility.IfColumnExists(row.Row, DatabaseObjects.Columns.DisableDiscussion))
                                Strparam += "&DisableDiscussion=" + UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.DisableDiscussion]));

                            if (UGITUtility.IfColumnExists(row.Row, DatabaseObjects.Columns.DisableRelatedItems))
                                Strparam += "&DisableRelatedItems=" + UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.DisableRelatedItems]));

                            if (UGITUtility.IfColumnExists(row.Row, DatabaseObjects.Columns.LeftPaneExpanded))
                                Strparam += "&LeftPaneExpanded=" + UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.LeftPaneExpanded]));

                            if (UGITUtility.IfColumnExists(row.Row, DatabaseObjects.Columns.BottomPaneExpanded))
                                Strparam += "&BottomPaneExpanded=" + UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.BottomPaneExpanded]));

                            link.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','{2}','{1}','90','90',false,'');", url, Convert.ToString(row[DatabaseObjects.Columns.Title]), Strparam));
                        }
                        break;
                    case "List":
                        {
                            KeyValuePair<string, string> fileurlList = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "listname");
                            url = UGITUtility.GetAbsoluteURL(Convert.ToString(row[DatabaseObjects.Columns.CustomProperties]).Replace("fileurl=", "").Replace(string.Format(";#ListName={0}", fileurlList.Value), ""));
                            link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','80',true);", url, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                        }
                        break;
                    case "Document":
                        {
                            url = UGITUtility.GetAbsoluteURL(Convert.ToString(row[DatabaseObjects.Columns.CustomProperties]).Replace("fileurl=", ""));
                            link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','90','80',true);", url, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                            // link.Attributes.Add("href", string.Format( uHelper.GetAbsoluteURL(url)));
                        }
                        break;
                    default:
                        KeyValuePair<string, string> urlParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "fileurl");
                        if (urlParam.Key != null && !string.IsNullOrEmpty(urlParam.Value))
                        {
                            absPath = UGITUtility.GetAbsoluteURL(urlParam.Value);
                            link.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{1}','auto', 'auto','90','80')", absPath, link.Text));
                        }
                        else
                        {
                            link.Attributes.Add("href", "javascript:void(0);");
                        }
                        break;
                }
            }
        }
    }
}

