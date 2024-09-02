using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Util.Log;
using uGovernIT.Helpers;

namespace uGovernIT.Web
{
    public partial class GovernanceConfiguratorEdit : UserControl
    {
        private string absoluteUrlEdit = "/layouts/ugovernit/delegateControl.aspx?control={0}&ID={1}&categoryID={2}&categoryType={3}";
        private string absoluteUrlNew = "/layouts/ugovernit/delegateControl.aspx?control={0}&ID={1}&itemID={2}&categoryType={3}";
        List<GovernanceLinkItem> splstCategoryItems;
        List<GovernanceLinkCategory> splstCategory;
        public string itemID;
        public string categoryType;
        private string newParam = "governanceCategoryAdd";
        private string editParam = "governanceCategoryEdit";
        string addCategoryLink = string.Empty;
        string editCategoryLink = string.Empty;
        string categoryId = string.Empty;
        GovernanceLinkItem item = null;
        GovernanceLinkItemManager ObjGovernanceLinkItemManager = new GovernanceLinkItemManager(HttpContext.Current.GetManagerContext());
        GovernanceLinkCategoryManager ObjGovernanceLinkCategoryManager = new GovernanceLinkCategoryManager(HttpContext.Current.GetManagerContext());
        AnalyticDashboardManager ObjAnalyticDashboardManager = new AnalyticDashboardManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            btAnalyticSync.Visible = false;
            if (ddlTargetType.SelectedValue == "Dashboard")
            {
                btAnalyticSync.Visible = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            //Getting the list data from sharepoint lists
            splstCategoryItems = ObjGovernanceLinkItemManager.Load();
            splstCategory = ObjGovernanceLinkCategoryManager.Load();        
            //Getting the selected item to edit.
           item = ObjGovernanceLinkItemManager.LoadByID(UGITUtility.StringToLong(this.itemID));          
            BindTargetTypeCategories();
            //Fill all the data from selected item in respective fields.
            txtItemName.Text = item.Title;          
            txtDescription.Text = item.Description;
            txtSequence.Text = Convert.ToString(item.TabSequence);
            txtImageUrl.Text = Convert.ToString(item.ImageUrl);
            string target = Convert.ToString(item.TargetType);
            ddlTargetType.SelectedIndex = ddlTargetType.Items.IndexOf(ddlTargetType.Items.FindByValue(Convert.ToString(item.TargetType)));

            //based on targettype, view of the edit pop up is adjusted.
            SetTargetDependency(item);

            BindControlList(item);
            BindModels();
            Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(Convert.ToString(item.CustomProperties), Constants.Separator);

            KeyValuePair<string, string> pageParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "fileurl");
            if (pageParam.Key != null)
            {
                txtFileLink.Text = pageParam.Value;
            }
            if (ddlTargetType.SelectedValue == "Dashboard")
            {
                KeyValuePair<string, string> modelkey = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "modelid");
                KeyValuePair<string, string> dashboardkey = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "dashboardid");
                if (!string.IsNullOrEmpty(modelkey.Key))
                {
                    ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByValue(modelkey.Value));
                    ddlModel_SelectedIndexChanged(ddlModel, new EventArgs());
                    if (!string.IsNullOrEmpty(dashboardkey.Key))
                    {
                        ddlDashbaord.SelectedIndex = ddlDashbaord.Items.IndexOf(ddlDashbaord.Items.FindByValue(dashboardkey.Value));
                    }
                }
            }

            if (this.categoryType.Equals("config"))
                trImage.Visible = false;
            else
                trImage.Visible = true;
           //Category id is needed to select the respective category of an item.
            string lookupValue = Convert.ToString(item.GovernanceLinkCategoryLookup);
            if (item.GovernanceLinkCategoryLookup>0)
                this.categoryId = lookupValue;
            BindCategories();
            
            //url for adding another item
            addCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, 0, 0, "config"));
            aAddCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','500','250',0,'{1}','true')", addCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
            if (!this.categoryId.Equals(string.Empty))
            {
                editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, 0, this.categoryId, "config"));
            }
            else
            {
                editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, 0, ddlCategory.SelectedValue, "config"));
            }
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','500','250',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));

            base.OnInit(e);
        }

        private void BindTargetTypeCategories()
        {
            ddlTargetType.Items.Add(new ListItem("Control", "Control"));
            ddlTargetType.Items.Add(new ListItem("File", "File"));
            ddlTargetType.Items.Add(new ListItem("Analytic Dashboard", "Dashboard"));
            ddlTargetType.Items.Add(new ListItem("Link", "Link"));
            ddlTargetType.DataBind();

        }

        private void BindControlList(GovernanceLinkItem item)
        {
            ddlControls.Items.Add(new ListItem("Project Portfolio", "ProjectPortfolio"));
            ddlControls.Items.Add(new ListItem("Budget Management", "BudgetManagement"));
            ddlControls.Items.Add(new ListItem("Non Project Budget Management", "NonProjectBudgetManagement"));
            //Some values are stored in customproperties hence need to fetch them.
            Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(Convert.ToString(item.CustomProperties), Constants.Separator);
            KeyValuePair<string, string> param = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "controltitle");
            if(!string.IsNullOrEmpty(param.Value))
            ddlControls.Items.FindByValue(param.Value).Selected = true;
            ddlControls.DataBind();

        }

       

        private void BindModels()
        {
            ddlModel.SelectedIndex = -1;
            DataTable dashboards = ObjAnalyticDashboardManager.GetDataTable();// SPListHelper.GetDataTable(DatabaseObjects.Lists.AnalyticDashboards, SPContext.Current.Web);
            DataTable resultTable = null;
            if (dashboards != null && dashboards.Rows.Count > 0)
            {
                resultTable = dashboards.DefaultView.ToTable(true, DatabaseObjects.Columns.AnalyticVID, DatabaseObjects.Columns.AnalyticName);
            }

            if (resultTable != null)
            {
                ddlModel.Items.Clear();
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
            if (splstCategory.Count > 0)
            {
                //Bind Category
                DataTable dtCategoryList = UGITUtility.ToDataTable(splstCategory);
                DataRow[] foundRows = null;
                DataTable dtCategory = null;
                string categoryName = String.Empty;
                categoryName = "Governance Dashboard Buttons";
                if (this.categoryType.Equals("config"))
                {

                    foundRows = dtCategoryList.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.CategoryName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title }).Select(string.Format("{0}<>'{1}'", DatabaseObjects.Columns.CategoryName, categoryName));
                    trTitle.Visible = true;
                    if (foundRows != null && foundRows.Length > 0)
                    {
                        dtCategory = foundRows.CopyToDataTable();
                        dtCategory.DefaultView.Sort = DatabaseObjects.Columns.CategoryName + " ASC";
                        ddlCategory.DataSource = dtCategory;
                        ddlCategory.DataTextField = DatabaseObjects.Columns.CategoryName;
                        ddlCategory.DataValueField = DatabaseObjects.Columns.Id;
                        ddlCategory.DataBind();
                        if (!string.IsNullOrEmpty(this.categoryId))
                            if (splstCategory.FirstOrDefault(x => x.ID == Convert.ToInt64(this.categoryId)) != null)
                                ddlCategory.Items.FindByValue(this.categoryId).Selected = true;
                    }

                }
                else
                {
                    trTitle.Visible = false;
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            GovernanceLinkItem item = ObjGovernanceLinkItemManager.LoadByID(UGITUtility.StringToInt(this.itemID));
            if (item != null)
            {
                item.TabSequence = Convert.ToInt32(txtSequence.Text);
                item.Title = txtItemName.Text;
                item.Description= txtDescription.Text;
                if (!this.categoryType.Equals("config"))
                    item.ImageUrl = txtImageUrl.Text;
                item.TargetType = ddlTargetType.SelectedValue;
                
                switch (ddlTargetType.SelectedValue)
                {
                    case "File":
                       
                            item.CustomProperties = string.Format("fileurl={0}", fileUploadControl.GetValue());
                     
                        break;
                    case "Control":
                        {
                            string pageLink = "/_layouts/15/uGovernIT/ProjectManagement.aspx";
                           

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
                        item.CustomProperties = string.Format("fileurl={0}",  txtFileLink.Text.Trim());
                        break;
                    default:
                        break;
                }    
               
                if (this.categoryType.Equals("config"))
                {
                    item.GovernanceLinkCategoryLookup = Convert.ToInt32(ddlCategory.SelectedValue);
                }
                ObjGovernanceLinkItemManager.Update(item);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            GovernanceLinkItem item = ObjGovernanceLinkItemManager.LoadByID(UGITUtility.StringToInt(this.itemID));
            if (item != null)
            {
                item.Deleted = true;                
                ObjGovernanceLinkItemManager.Update(item);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
     

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, 0, ddlCategory.SelectedValue, "config"));
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','600',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
        }

        private void SetTargetDependency(GovernanceLinkItem linkItem )
        {
            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = true;
                    fileUploadControl.SetValue(item.CustomProperties.Replace("fileurl=", ""));
                    break;
                case "Control":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = false;
                    trControlsList.Visible = true;
                    trFileUpload.Visible = false;
                    break;
                case "Dashboard":
                    trModel.Visible = true;
                    trDashboard.Visible = true;
                    trLink.Visible = false;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    break;
                case "Link":
                    trModel.Visible = false;
                    trDashboard.Visible = false;
                    trLink.Visible = true;
                    trControlsList.Visible = false;
                    trFileUpload.Visible = false;
                    break;
                default:
                    break;
            }    
        }
        protected void ddlTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTargetDependency(item);
        }

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDashbaord.SelectedIndex = -1;
            ddlDashbaord.Items.Clear();
            if (ddlModel.SelectedValue != string.Empty)
            {
                DataTable dashboards = ObjAnalyticDashboardManager.GetDataTable();//
                if (ddlModel.SelectedValue != string.Empty)
                {
                    DataView view = dashboards.DefaultView;
                    view.RowFilter = string.Format("{0}={1}", DatabaseObjects.Columns.AnalyticVID, ddlModel.SelectedValue);
                    dashboards = view.ToTable();
                }

                if (dashboards != null)
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
                AnalyticSync.Sync(HttpContext.Current.GetManagerContext());
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
