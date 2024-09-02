using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class DashboardQueryDisplay : UserControl
    {
        //public int ApplicationId { get; set; }

        //private string dashboardType = "All";

        private ApplicationContext _context = null;
        private DashboardManager _dashboardManager = null;

        protected string delegatePageUrl;
        protected string sourceUrl = string.Empty;
        protected string pageName { get; set; }

        protected int dashboardID;

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

        protected DashboardManager DashboardManager
        {
            get
            {
                if (_dashboardManager == null)
                {
                    _dashboardManager = new DashboardManager(ApplicationContext);
                }
                return _dashboardManager;
            }
        }

        Dashboard uDashboard = null;
        UserProfile currentUser;

        List<Dashboard> dashboardTable = new List<Dashboard>();

        protected override void OnInit(EventArgs e)
        {
            currentUser = HttpContext.Current.CurrentUser();
            sourceUrl = UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery);

            if (!string.IsNullOrEmpty(Request["dashboardId"]))
            {
                dashboardID = UGITUtility.StringToInt(Request["dashboardId"]);
                if (dashboardID > 0)
                {
                    filterTable.Visible = false;
                    ExecuteQuery();
                }
            }
            else
            {
                filterTable.Visible = true;
                //dashboardType = "Query";
                //Get dashboard tables and bind it with listview 
                //DataTable dashboardTable = null;

                //string query = $"{DatabaseObjects.Columns.DashboardType} = '{(int)DashboardType.Query}' and ({DatabaseObjects.Columns.IsActivated} = '0' or {DatabaseObjects.Columns.IsActivated} is null) and ({DatabaseObjects.Columns.AuthorizedToView} = '{HttpContext.Current.CurrentUser().Id}' or {DatabaseObjects.Columns.AuthorizedToView} is null ) ";
                // Uncomment above query later

                string query = $"{DatabaseObjects.Columns.DashboardType} = {(int)DashboardType.Query} and {DatabaseObjects.Columns.IsActivated} = 0 and ({DatabaseObjects.Columns.AuthorizedToView} = '{currentUser.Id}' or {DatabaseObjects.Columns.AuthorizedToView} is null )";

                //dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{query} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                dashboardTable.Clear();
                dashboardTable = DashboardManager.Load(query);

                LoadCategories(null);
                FillQueries(dashboardTable);
                ddlQueryList.Attributes.Add("onChange", "setFlag();");
                ddlCategoryList.Attributes.Add("onChange", "validate(this);");
                isTableLoaded.Value = "false";
                if (Request[ddlQueryList.UniqueID] == null)
                    dashboardID = UGITUtility.StringToInt(ddlQueryList.SelectedValue);
                else
                    dashboardID = UGITUtility.StringToInt(Request[ddlQueryList.UniqueID]);

                if (!string.IsNullOrEmpty(Request[hdnbuttonClick.UniqueID]))
                {
                    dashboardID = UGITUtility.StringToInt(Request[hdnbuttonClick.UniqueID]);
                    if (dashboardID > 0)
                        ExecuteQuery();
                }

                if (Request["queryId"] != null)
                {
                    dashboardID = UGITUtility.StringToInt(Request["queryId"]);
                    if (dashboardID > 0)
                    {
                        ddlQueryList.SelectedIndex = ddlQueryList.Items.IndexOf(ddlQueryList.Items.FindByValue(Convert.ToString(dashboardID)));
                        uDashboard = DashboardManager.LoadPanelById(dashboardID);
                        if (uDashboard != null)
                            ddlCategoryList.SelectedIndex = ddlCategoryList.Items.IndexOf(ddlCategoryList.Items.FindByText(uDashboard.CategoryName));

                        ExecuteQuery();
                    }
                }
            }

            base.OnInit(e);
        }

        /// <summary>
        /// It fills the category dropdown
        /// </summary>
        private void LoadCategories(List<Dashboard> dashboardTable)
        {
            if (dashboardTable == null)
            {
                //dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.DashboardType} = '{(int) DashboardType.Query}' and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                dashboardTable = DashboardManager.Load($"{DatabaseObjects.Columns.DashboardType} = {(int)DashboardType.Query} and {DatabaseObjects.Columns.IsActivated} = 0");
            }

            ddlCategoryList.Items.Clear();

            // Bind the Category and subcategory in the same dropdown
            //var groupCategoryData = dashboardTable.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.CategoryName)).OrderBy(x => x.Key);
            var groupCategoryData = dashboardTable.GroupBy(x => x.CategoryName).OrderBy(x => x.Key);
            ListItem item;
            string style = string.Empty;

            foreach (var category in groupCategoryData)
            {
                if (category.Key == "Ticketing") //skip ticketing for crm
                    continue;
                if (!string.IsNullOrEmpty(category.Key) && category.Key.ToLower() != "none")
                {
                    
                    item = new ListItem(category.Key, category.Key);
                    //ddlCategoryList.Items.Add(item);

                    //string subCategoryFilterExp = string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, category.Key);
                    var subCategoriesRow = dashboardTable.Where(x => x.CategoryName == category.Key).Select(x => x.SubCategory).Distinct().OrderBy(x=>x).ToList(); //dashboardTable.Select(subCategoryFilterExp);

                    if (subCategoriesRow != null && subCategoriesRow.Count > 0)
                    {
                        foreach (var row in subCategoriesRow)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(row)) && Convert.ToString(row).ToLower() != "none")
                            {
                                string gap = string.Empty;
                                item = new ListItem($"{category.Key} > {row}");
                                //item = new ListItem(string.Format("{0}{1}", gap, Convert.ToString(row)), string.Format("SubCategory--{0}{1}", Convert.ToString(row), category.Key));
                                ddlCategoryList.Items.Add(item);
                            }
                        }
                    }
                }
            }

            ddlCategoryList.Items.Insert(0, new ListItem("--All--", "-1"));
        }


        /// <summary>
        /// It fills the query dropdown
        /// </summary>
        /// <param name="dashboardTable"></param>
        private void FillQueries(List<Dashboard> dashboardTable)
        {
            if (dashboardTable != null)
            {
                ddlQueryList.Items.Clear();
                ddlQueryList.SelectedIndex = -1;
                ddlQueryList.DataTextField = DatabaseObjects.Columns.Title;
                ddlQueryList.DataValueField = DatabaseObjects.Columns.Id;
                ddlQueryList.DataSource = dashboardTable;
                ddlQueryList.DataBind();

                SortDDL(ref ddlQueryList);
            }
        }

        private void SortDDL(ref DropDownList objDDL)
        {
            ArrayList textList = new ArrayList();
            ArrayList valueList = new ArrayList();

            if (objDDL != null)
            {
                foreach (ListItem li in objDDL.Items)
                {
                    textList.Add(li.Text);
                }

                textList.Sort();

                foreach (object item in textList)
                {
                    string value = objDDL.Items.FindByText(item.ToString()).Value;
                    valueList.Add(value);
                }
                objDDL.Items.Clear();

                for (int i = 0; i < textList.Count; i++)
                {
                    ListItem objItem = new ListItem(textList[i].ToString(), valueList[i].ToString());
                    objDDL.Items.Add(objItem);
                }
            }
        }

        private void GetFilteredQueriesBasedOnCategory()
        {
            //dashboardType = "Query";
            //Get dashboard tables
            //DataTable dashboardTable = null;            
            string query = GenerateCategoryQuery(ddlCategoryList);

            //dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{query} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
            dashboardTable.Clear();
            dashboardTable = DashboardManager.Load(query);

            if (dashboardTable != null)
            {
                FillQueries(dashboardTable);
            }
            else
            {
                ddlQueryList.Items.Clear();
            }
        }

        private void GetFilteredDataBasedOnModules()
        {
            //dashboardType = "Query";
            //Get dashboard tables
            //DataTable dashboardTable = null;            
            //dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
            dashboardTable.Clear();
            dashboardTable = DashboardManager.Load();

            if (dashboardTable != null)
            {
                LoadCategories(dashboardTable);
                FillQueries(dashboardTable);
            }
            else
            {
                ddlCategoryList.Items.Clear();
                ddlQueryList.Items.Clear();
            }
        }

        /// <summary>
        /// This functions executes the query in case there are no parameters to be passed.
        /// </summary>
        private void ExecuteQuery()
        {
            reportPanel.Controls.Clear();
            QueryWizardPreview preview = (QueryWizardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryWizardPreview.ascx");
            preview.Id = dashboardID;
            reportPanel.Controls.Add(preview);
        }

        private void AddQueryToFactTable(String queryId, String queryTitle)
        {

        }

        private string GenerateCategoryQuery(DropDownList ddlCat)
        {
            string returnQuery = string.Empty, categoryFilter = string.Empty;

            //returnQuery= $"{DatabaseObjects.Columns.DashboardType} = '{(int)DashboardType.Query}' and ({DatabaseObjects.Columns.IsActivated} = '0' or {DatabaseObjects.Columns.IsActivated} is null) and ({DatabaseObjects.Columns.AuthorizedToView} = '{HttpContext.Current.CurrentUser().Id}' or {DatabaseObjects.Columns.AuthorizedToView} is null ) ";
            // Uncomment above query later

            returnQuery = $"{DatabaseObjects.Columns.DashboardType} = '{(int)DashboardType.Query}' and ({DatabaseObjects.Columns.AuthorizedToView} = '{currentUser.Id}' or {DatabaseObjects.Columns.AuthorizedToView} = '' or {DatabaseObjects.Columns.AuthorizedToView} is null ) and ({DatabaseObjects.Columns.IsActivated} = '1' or {DatabaseObjects.Columns.IsActivated} is null)";


            if (ddlCat.SelectedValue != "" && ddlCat.SelectedValue != "-1" && ddlCat.SelectedItem.Text.ToLower() != "none")
            {
                if (ddlCat.SelectedValue.Contains("SubCategory--"))
                {
                    categoryFilter = $"{DatabaseObjects.Columns.SubCategory} = '{ddlCat.SelectedItem.Text.Trim()}'";
                }
                else
                {
                    List<string> categorylst = UGITUtility.ConvertStringToList(ddlCat.SelectedItem.Text.Trim(), ">");
                    
                    if (categorylst.Count > 1)
                    {
                        categoryFilter = $"{DatabaseObjects.Columns.CategoryName} = '{categorylst[0].Trim()}' And {DatabaseObjects.Columns.SubCategory} = '{categorylst[1].Trim()}'";
                    }
                    else if(categorylst.Count > 0)
                    {
                        categoryFilter = $"{DatabaseObjects.Columns.CategoryName} = '{categorylst[0].Trim()}'";
                    }
                }

                returnQuery = $"{returnQuery} and {categoryFilter}";
            }
            return returnQuery;
        }

        protected void DdlCategoryList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hdnbuttonClick.Value = string.Empty;
            GetFilteredQueriesBasedOnCategory();
        }

        protected void DdlModules_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetFilteredDataBasedOnModules();
        }

        protected void BtSaveAndRun_OnClick(object sender, EventArgs e)
        {
            dashboardID = Convert.ToInt32(ddlQueryList.SelectedValue);
            if (dashboardID > 0)
            {
                hdnbuttonClick.Value = Convert.ToString(dashboardID);
                ExecuteQuery();
            }
        }

    }
}
