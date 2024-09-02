using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class GovernanceConfiguratorAdd : UserControl
    {
        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&categoryID={2}&categoryType={3}";
        private string absoluteUrlNew = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&itemID={2}&categoryType={3}";
        public List<GovernanceLinkCategory> dtCategory;
        public string itemID;
        public string categoryType;
        private string newParam = "governanceCategoryAdd";
        private string editParam = "governanceCategoryEdit";
        string addCategoryLink = string.Empty;
        string editCategoryLink = string.Empty;
        GovernanceLinkCategoryManager ObjGovernanceLinkCategoryManager = new GovernanceLinkCategoryManager(HttpContext.Current.GetManagerContext());
        AnalyticDashboardManager ObjAnalyticDashboardManager = new AnalyticDashboardManager(HttpContext.Current.GetManagerContext());
        GovernanceLinkItemManager ObjGovernanceLinkItemManager = new GovernanceLinkItemManager(HttpContext.Current.GetManagerContext());
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
            // dtCategoryItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GovernanceLinkItems);
            dtCategory = ObjGovernanceLinkCategoryManager.Load();
            //splstCategoryItems = SPListHelper.GetSPList(DatabaseObjects.Lists.GovernanceLinkItems);
            //splstCategory = SPListHelper.GetSPList(DatabaseObjects.Lists.GovernanceLinkCategory);
            BindCategories();
            BindTargetTypeCategories();
            BindControlList();
            SetTargetTypeDependency();
            BindModels();
            ddlModel_SelectedIndexChanged(ddlModel, new EventArgs());
            addCategoryLink =UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlNew, this.newParam, 0, 0, "config"));
            aAddCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','500','250',0,'{1}','true')", addCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
            editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, 0, ddlCategory.SelectedValue, "config"));
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','500','250',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
            if (this.categoryType.Equals("config"))
                trImage.Visible = false;
            else
                trImage.Visible = true;

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
        private void BindControlList()
        {
            ddlControls.Items.Add(new ListItem("Project Portfolio", "ProjectPortfolio"));
            ddlControls.Items.Add(new ListItem("Budget Management", "BudgetManagement"));
            ddlControls.Items.Add(new ListItem("Non Project Budget Management", "NonProjectBudgetManagement"));
            ddlControls.DataBind();

        }
       

        private void BindModels()
        {          
            List<AnalyticDashboards> dashboards = ObjAnalyticDashboardManager.Load();
            List<AnalyticDashboards> resultTable = null;
            if (dashboards != null && dashboards.Count > 0)
            {
                // resultTable = dashboards.DefaultView.ToTable(true, DatabaseObjects.Columns.AnalyticVID, DatabaseObjects.Columns.AnalyticName);
                resultTable = dashboards.Distinct().ToList();
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
            
            //Bind Category
            List<GovernanceLinkCategory> dtCategoryList = ObjGovernanceLinkCategoryManager.Load();
            List<GovernanceLinkCategory> foundRows = null;
            List<GovernanceLinkCategory> dtCategory = null;
                string categoryName = String.Empty;
                if (dtCategoryList != null && dtCategoryList.Count > 0)
                {
                    if (this.categoryType.Equals("config"))
                    {
                        categoryName = "Governance Dashboard Buttons";
                        foundRows = dtCategoryList.Where(x=> !x.CategoryName.ToLower().Equals(categoryName.ToLower())).ToList();
                        trTitle.Visible = true;
                        if (foundRows != null && foundRows.Count > 0)
                        {
                            dtCategory = foundRows.OrderBy(x=>x.CategoryName).ToList();
                            ddlCategory.DataSource = dtCategory;
                            ddlCategory.DataTextField = DatabaseObjects.Columns.CategoryName;
                            ddlCategory.DataValueField = DatabaseObjects.Columns.ID;
                            ddlCategory.DataBind();
                        }
                    }
                    else
                    {

                        trTitle.Visible = false;
                    }
                }
               
           // }
        }

        private string GetCategoryID(String category)
        {
            string categoryName = String.Empty;
            string categoryId = String.Empty;
            categoryName = "Governance Dashboard Buttons";
            dtCategory = ObjGovernanceLinkCategoryManager.Load();
           List<GovernanceLinkCategory> dr = dtCategory.Where(y=>y.CategoryName==categoryName).ToList();
            if (dr != null && dr.Count > 0)
                categoryId = Convert.ToString(dr[0].ID);
            return categoryId;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //SPListItem item = splstCategoryItems.Items.Add();
            string categoryName = String.Empty;
            string categoryId = String.Empty;
            categoryName = "Governance Dashboard Buttons";
            GovernanceLinkItem governanceLinkItem = new GovernanceLinkItem();

            governanceLinkItem.TabSequence = Convert.ToInt32(txtSequence.Text);
            governanceLinkItem.Title = txtItemName.Text;
            governanceLinkItem.Description = txtDescription.Text;


            if (!this.categoryType.Equals("config"))
                governanceLinkItem.ImageUrl = txtImageUrl.Text;
            governanceLinkItem.TargetType = ddlTargetType.SelectedValue;


            switch (ddlTargetType.SelectedValue)
            {
                case "File":                  
                        governanceLinkItem.CustomProperties= fileUploadControl.GetValue();                   
                    break;
                case "Control":
                    {
                        string pageLink = "/layouts/uGovernIT/ProjectManagement.aspx";


                        switch (ddlControls.SelectedValue)
                        {
                            case "ProjectPortfolio":
                                governanceLinkItem.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGPortfolio", "ProjectPortfolio");
                                break;
                            case "BudgetManagement":
                                governanceLinkItem.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGBudgetManagement", "BudgetManagement");
                                break;
                            case "NonProjectBudgetManagement":
                                governanceLinkItem.CustomProperties = string.Format("fileurl={0};#control={1};#controlTitle={2}", pageLink, "ITGBudgetEditor", "NonProjectBudgetManagement");
                                break;
                            default:
                                break;
                        }

                    }
                    break;
                case "Dashboard":
                    {

                        governanceLinkItem.CustomProperties = string.Format("modelid={0};#dashboardid={1}", ddlModel.SelectedValue, ddlDashbaord.SelectedValue);
                    }
                    break;
                case "Link":
                    governanceLinkItem.CustomProperties = string.Format("fileurl={0}", txtFileLink.Text.Trim());
                    break;
                default:
                    break;
            }


            if (this.categoryType.Equals("config"))
            {
                governanceLinkItem.GovernanceLinkCategoryLookup = Convert.ToInt64(ddlCategory.SelectedValue);
            }
            else
            {
                categoryId = GetCategoryID(categoryName);
                if (categoryId.Equals(String.Empty))
                    governanceLinkItem.GovernanceLinkCategoryLookup = 0;
                else
                    governanceLinkItem.GovernanceLinkCategoryLookup = Convert.ToInt64(categoryId);

            }
            ObjGovernanceLinkItemManager.Insert(governanceLinkItem);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            editCategoryLink = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, this.editParam, 0, ddlCategory.SelectedValue, "config"));
            aEditCategory.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','New Item','600','200',0,'{1}','true')", editCategoryLink, Server.UrlEncode(Request.Url.AbsolutePath)));
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
            SetTargetTypeDependency();            
        }

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDashbaord.Items.Clear();
            if (ddlModel.SelectedValue != string.Empty)
            {
                List<AnalyticDashboards> dashboards = ObjAnalyticDashboardManager.Load(x => x.AnalyticVID.Equals(Convert.ToInt32(ddlModel.SelectedValue)));

                if (dashboards != null && dashboards.Count > 0)
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
               // AnalyticSync.Sync(SPContext.Current.Web);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
            if (ddlTargetType.SelectedValue == "Dashboard")
            {
                BindModels();
                ddlModel_SelectedIndexChanged(ddlModel, new EventArgs());
            }
        }
     
    }
}
