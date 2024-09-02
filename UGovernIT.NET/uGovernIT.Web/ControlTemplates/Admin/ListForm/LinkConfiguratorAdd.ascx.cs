using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using uGovernIT.Util.Log;
using System.IO;

namespace uGovernIT.Web
{
    public partial class LinkConfiguratorAdd : UserControl
    {
        private string absoluteUrlNew = "/layouts/ugovernit/DelegateControl.aspx?control={0}&linkCategoryID={1}";
        List<LinkItems> splstLinkItems;
        List<LinkCategory> splstLinkCategory;
        public string linkItemID;
        private string newParam = "linkcategoryedit";
        string addCategoryLink = string.Empty;
        string editCategoryLink = string.Empty;
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string newWikiParam = "listpicker";
        private string formTitle = "Picker List";
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LinkItemManager ObjLinkItemManager = null;
        LinkCategoryManager ObjLinkCategoryManager = null;
        AnalyticDashboardManager ObjAnalyticDashboardManager = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newWikiParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            AddWikiItem.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            btAnalyticSync.Visible = false;
            if (ddlTargetType.SelectedValue == "Dashboard")
            {
                btAnalyticSync.Visible = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ObjLinkItemManager = new LinkItemManager(context);
            ObjLinkCategoryManager = new LinkCategoryManager(context);
            ObjAnalyticDashboardManager = new AnalyticDashboardManager(context);

            splstLinkItems = ObjLinkItemManager.Load();// SPListHelper.GetSPList(DatabaseObjects.Lists.LinkItems);
            splstLinkCategory = ObjLinkCategoryManager.Load();// SPListHelper.GetSPList(DatabaseObjects.Lists.LinkCategory);
            
            BindLists();
            BindCategories();
            BindTargetTypeCategories();
            BindControlList();
            SetTargetTypeDependency();
            BindModels();
            ddlModel_SelectedIndexChanged(ddlModel, new EventArgs());
            addCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, 0));
            aAddCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Section','500','300',0,'{1}','true')", addCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
            editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, ddlCategory.SelectedValue));
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Edit Section','500','300',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));

            base.OnInit(e);
        }


        private void BindLists()
        {
            string listNames = HttpContext.Current.GetManagerContext().ConfigManager.GetValue(ConfigConstants.LinkViewLists);
            if (!string.IsNullOrEmpty(listNames))
            {
                string[] lists = listNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in lists)
                {
                    ddlListName.Items.Add(new ListItem(item, item));
                }
            }
        }

        private void BindTargetTypeCategories()
        {
            ddlTargetType.Items.Add(new ListItem("Control", "Control"));
            ddlTargetType.Items.Add(new ListItem("File", "File"));
            ddlTargetType.Items.Add(new ListItem("Analytic Dashboard", "Dashboard"));
            ddlTargetType.Items.Add(new ListItem("Link", "Link"));
            ddlTargetType.Items.Add(new ListItem("Wiki", "Wiki"));
            ddlTargetType.Items.Add(new ListItem("List", "List"));
            ddlTargetType.DataBind();
        }

        private void BindControlList()
        {
            ddlControls.Items.Add(new ListItem("Project Portfolio", "ProjectPortfolio"));
            ddlControls.Items.Add(new ListItem("Budget Management", "BudgetManagement"));
            ddlControls.Items.Add(new ListItem("Non Project Budget Management", "NonProjectBudgetManagement"));
            ddlControls.DataBind();
        }

        private void BindModels()
        {
            
            DataTable dashboards = ObjAnalyticDashboardManager.GetDataTable();
            DataTable resultTable = null;
            if (dashboards != null && dashboards.Rows.Count > 0)
            {
                resultTable = dashboards.DefaultView.ToTable(true, DatabaseObjects.Columns.AnalyticVID, DatabaseObjects.Columns.AnalyticName);
            }

            if (resultTable != null)
            {
                ddlModel.AppendDataBoundItems = false;
                ddlModel.DataSource = resultTable;
                ddlModel.DataTextField = DatabaseObjects.Columns.AnalyticName;
                ddlModel.DataValueField = DatabaseObjects.Columns.AnalyticVID;
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
        }

        private void BindCategories()
        {
            splstLinkCategory = ObjLinkCategoryManager.Load(x => !x.Deleted);
            if (splstLinkCategory != null && splstLinkCategory.Count > 0)
            {
                List<LinkCategory> dtLinkCategory = splstLinkCategory.OrderBy(x => x.Title).ToList();
                if (dtLinkCategory != null && dtLinkCategory.Count > 0)
                {
                    ddlCategory.DataSource = dtLinkCategory;
                    ddlCategory.DataTextField = DatabaseObjects.Columns.Title;
                    ddlCategory.DataValueField = DatabaseObjects.Columns.ID;
                    ddlCategory.DataBind();
                }
                ddlCategory.Items.Insert(0, new ListItem("--Please Select--", "0"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool HasError = false;
            LinkItems item = new LinkItems();
            if (item != null)
            {
                item.ItemOrder = UGITUtility.StringToInt(txtSequence.Text);
                item.Title = txtItemName.Text;
                item.Description = txtDescription.Text;
                item.TargetType = ddlTargetType.SelectedValue;
                item.AuthorizedToView = ppeAuthorizedToView.GetValues();
                switch (ddlTargetType.SelectedValue)
                {
                    case "File":
                        /*
                            ObjLinkItemManager.Save(item);
                        item.CustomProperties = string.Format("fileurl={0}",fileUploadControl.GetValue());
                        */

                        if (fileUploadControl.HasFile)
                        {
                            //new condition file becasue its crach on attahcments.urlprefix.
                            //ObjLinkItemManager.Save(item);
                            string SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
                            string filePath = Server.MapPath($"/Content/images/ugovernit/upload/{context.TenantID}/");
                            string fileNamePath = Server.MapPath($"/Content/images/ugovernit/upload/{context.TenantID}/{fileUploadControl.FileName}");

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            if (File.Exists(fileNamePath))
                            {
                                lblMessage.Text = $"{fileUploadControl.FileName} already exists.";
                                lblMessage.Visible = true;
                                HasError = true;
                            }
                            else
                            {
                                fileUploadControl.SaveAs(fileNamePath);
                                item.CustomProperties = $"fileurl={SiteUrl}/Content/images/ugovernit/upload/{context.TenantID}/{fileUploadControl.FileName}";
                            }
                        }

                        break;

                    case "Control":
                        {
                            string pageLink = "/layouts/uGovernIT/ProjectManagement.aspx";
                            switch (ddlControls.SelectedValue)
                            {
                                case "ProjectPortfolio":
                                    item.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGPortfolio", "ProjectPortfolio");
                                    break;
                                case "BudgetManagement":
                                    item.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGBudgetManagement", "BudgetManagement");
                                    break;
                                case "NonProjectBudgetManagement":
                                    item.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGBudgetEditor", "NonProjectBudgetManagement");
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "Dashboard":
                        {
                            item.CustomProperties = string.Format("modelid={0};#dashboardid={1}", ddlModel.SelectedValue, ddlDashbaord.SelectedValue);
                        }
                        break;
                    case "Link":
                        item.CustomProperties = string.Format("fileurl={0}", txtFileLink.Text.Trim());
                        break;
                    case "Wiki":
                        {
                            if (txtWiki.Visible)
                            {
                                item.CustomProperties = string.Format("fileurl={0}", txtWiki.Text.Trim());
                            }
                        }
                        break;
                    case "List":
                        {
                            if (ddlListName.Visible)
                            {
                                string listPageLink = string.Format("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=listcontrolview&listName={0}&helplink=true", ddlListName.SelectedValue);
                                item.CustomProperties = string.Format("fileurl={0};#ListName={1}", listPageLink, ddlListName.SelectedValue);
                            }
                        }
                        break;
                    default:
                        break;
                }

                item.LinkCategoryLookup = UGITUtility.StringToInt(ddlCategory.SelectedValue);

                if (!HasError)
                {
                    ObjLinkItemManager.Save(item);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
            //uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, ddlCategory.SelectedValue));
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Edit Category','500','250',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
        }

        private void SetTargetTypeDependency()
        {
            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = true;
                    trWiki.Visible = false;
                    trList.Visible = false;
                    break;
                case "Control":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = true;
                    trFileUpload.Visible = false;
                    trWiki.Visible = false;
                    trList.Visible = false;
                    break;
                case "Dashboard":
                    trModel.Visible = true;
                    trDashboard.Visible = true;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    trWiki.Visible = false;
                    trList.Visible = false;
                    break;
                case "Link":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = true;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    trWiki.Visible = false;
                    trList.Visible = false;
                    break;
                case "Wiki":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    trWiki.Visible = true;
                    trList.Visible = false;
                    break;
                case "List":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    trWiki.Visible = false;
                    trList.Visible = true;
                    break;
                default:
                    break;
            }
        }

        protected void ddlTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTargetTypeDependency();
        }

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDashbaord.Items.Clear();
            if (ddlModel.SelectedValue != string.Empty)
            {
                DataTable dashboards = ObjAnalyticDashboardManager.GetDataTable();// SPListHelper.GetDataTable(DatabaseObjects.Lists.AnalyticDashboards, SPContext.Current.Web);
                if (dashboards != null)
                {
                    DataView view = dashboards.DefaultView;
                    view.RowFilter = string.Format("{0}={1}", DatabaseObjects.Columns.AnalyticVID, ddlModel.SelectedValue);
                    dashboards = view.ToTable();
                    if (dashboards != null && dashboards.Rows.Count > 0)
                    {
                        ddlDashbaord.AppendDataBoundItems = false;
                        ddlDashbaord.DataSource = dashboards;
                        ddlDashbaord.DataTextField = DatabaseObjects.Columns.Title;
                        ddlDashbaord.DataValueField = DatabaseObjects.Columns.DashboardID;
                        ddlDashbaord.DataBind();
                        ddlDashbaord.Items.Insert(0, new ListItem("--Please Select--", ""));
                    }
                }
            }

        }

        protected void ddlDashbaord_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlControls_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTargetType.SelectedValue)
            {
                case "ProjectPortfolio":

                    break;
                case "BudgetManagement":

                    break;
                case "NonProjectBudgetManagement":

                    break;
                default:
                    break;
            }
        }

        protected void BtAnalyticSync_Click(object sender, EventArgs e)
        {
            try
            {
                AnalyticSync.Sync(context);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            if (ddlTargetType.SelectedValue == "Dashboard")
            {
                BindModels();
                ddlModel_SelectedIndexChanged(ddlModel, new EventArgs());
            }
        }

    }
}
