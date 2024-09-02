using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class ReportCardView : System.Web.UI.UserControl
    {

        public string reportUrl = string.Empty;
        public string UserId = string.Empty;
        public string selectedCategory = string.Empty;
        public List<Tuple<string, string, string, string>> SubCategoriesTuple;
        public List<Tuple<string, string, string>> AllReports;

        private ApplicationContext _context;
        private DashboardManager _dashboardManager = null;
        //private string dashboardType = "All";

        public List<Dashboard> dashboardTable = new List<Dashboard>();
        public List<ReportMenu> lstReportMenu = new List<ReportMenu>();
        private ReportMenuManager _reportMenuManager = null;
        private ConfigurationVariableManager _configurationVariableManager;
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

        protected ReportMenuManager ReportMenuManager { 
            get 
            {
                if (_reportMenuManager == null)
                {
                    _reportMenuManager = new ReportMenuManager(ApplicationContext);
                }
                return _reportMenuManager;
            } 
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableManager;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
            SubCategoriesTuple = new List<Tuple<string, string, string, string>>();
            AllReports = new List<Tuple<string, string, string>>();
            FillSubCategoriesTuple();
            GetFilteredQueriesBasedOnCategory();
            reportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            UserId = _context.CurrentUser.Id;
            if (!Page.IsPostBack)
            {
                LoadCategories(null);
            }
        }

        private void LoadCategories(List<Dashboard> dashboardTable)
        {
            List<ListItem> items = new List<ListItem>();
            ddlCategoryList.Items.Clear();
            items = GetCategories(dashboardTable);
            foreach (var list in items)
            {
                ddlCategoryList.Items.Add(list);
            }
            ddlCategoryList.Items.Insert(0, new ListItem("--All--", "-1"));
        }

        private List<ListItem> GetCategories(List<Dashboard> dashboardTable)
        {
            if (dashboardTable == null)
            {
                //dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.DashboardType} = '{(int) DashboardType.Query}' and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                dashboardTable = DashboardManager.Load($"{DatabaseObjects.Columns.DashboardType} = {(int)DashboardType.Query} and {DatabaseObjects.Columns.IsActivated} = 1");
            }

            // Bind the Category and subcategory in the same dropdown
            //var groupCategoryData = dashboardTable.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.CategoryName)).OrderBy(x => x.Key);
            var groupCategoryData = dashboardTable.GroupBy(x => x.CategoryName).OrderBy(x => x.Key);
            List<ListItem> items = new List<ListItem>();
            ListItem item;
            string style = string.Empty;

            foreach (var category in groupCategoryData)
            {
                if (category.Key == "Ticketing") //skip ticketing for crm
                    continue;
                if (!string.IsNullOrEmpty(category.Key) && category.Key.ToLower() != "none")
                {
                    /*
                    item = new ListItem(category.Key, category.Key);
                    //ddlCategoryList.Items.Add(item);
                    items.Add(item);
                    */
                    var subCategoriesRow = dashboardTable.Where(x => x.CategoryName == category.Key).Select(x => x.SubCategory).Distinct().ToList(); //dashboardTable.Select(subCategoryFilterExp);

                    if (subCategoriesRow != null && subCategoriesRow.Count > 0)
                    {
                        foreach (var row in subCategoriesRow)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(row)) && Convert.ToString(row).ToLower() != "none")
                            {
                                string gap = string.Empty;
                                item = new ListItem(string.Format("{0}{1}", gap, Convert.ToString(row)), string.Format("SubCategory--{0}", Convert.ToString(row)));
                                //ddlCategoryList.Items.Add(item);
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            

            lstReportMenu = ReportMenuManager.Load(x => x.Deleted != true).ToList();
            lstReportMenu.ForEach(o =>
            {
                string gap = string.Empty;
                item = new ListItem(string.Format("{0}{1}", gap, Convert.ToString(o.Category)), string.Format("SubCategory--{0}", Convert.ToString(o.Category)));
                items.Add(item);
            });
            items = items.Distinct().OrderBy(x => x.Text).ToList();
            return items;
        }

        public void FillSubCategoriesTuple()
        {
            List<ListItem> items = GetCategories(null);
            string reportImageAndBackGroundVal = ConfigurationVariableManager.GetValue(ConfigConstants.ReportImageAndBackGround);
            Dictionary<string, string> colorAndImage = UGITUtility.GetCustomProperties(reportImageAndBackGroundVal, Constants.Separator, true, true);
            foreach (var list in items)
            {
                if (colorAndImage != null && colorAndImage.ContainsKey(list.Text))
                {
                    string[] item = colorAndImage[list.Text].ToString().Split(new string[] { Constants.Separator7 }, StringSplitOptions.RemoveEmptyEntries);
                    if (item.Length == 3)
                    {
                        SubCategoriesTuple.Add(new Tuple<string, string, string, string>(list.Text, item[0], item[1], item[2]));
                    }
                }
                else
                {
                    SubCategoriesTuple.Add(new Tuple<string, string, string, string>(list.Text, "/Content/images/chart3.jpg", "#808080", "0"));
                }
            }

            SubCategoriesTuple.OrderBy(x => x.Item4);
        }

        protected void DdlCategoryList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = ddlCategoryList.SelectedItem.Text;
            GetFilteredQueriesBasedOnCategory();
        }

        private void GetFilteredQueriesBasedOnCategory()
        {
            //dashboardType = "Query";
            //Get dashboard tables
            //DataTable dashboardTable = null;            
            string query = GenerateCategoryQuery(ddlCategoryList);

            dashboardTable.Clear();
            dashboardTable = DashboardManager.Load(query);
            AllReports.Add(new Tuple<string, string, string>("0", "All", "All"));
            dashboardTable?.ForEach(x =>
            {
                AllReports.Add(new Tuple<string, string, string>(x.ID.ToString(), x.Title, "queryreport"));
            });

            lstReportMenu = ReportMenuManager.Load(x => x.Deleted != true).ToList();
            lstReportMenu?.ForEach(o =>
            {
                AllReports.Add(new Tuple<string, string, string>(o.ID.ToString(), o.Title, "customreport"));
            });

            if (dashboardTable != null)
            {
                FillQueries(dashboardTable);
            }
            else
            {
                ddlQueryList.Items.Clear();
            }
        }

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

        private string GenerateCategoryQuery(DropDownList ddlCat)
        {
            string returnQuery = string.Empty, categoryFilter = string.Empty;

            //returnQuery = $"{DatabaseObjects.Columns.DashboardType} = '{(int)DashboardType.Query}' and ({DatabaseObjects.Columns.AuthorizedToView} = '{_context.CurrentUser.Id}' or {DatabaseObjects.Columns.AuthorizedToView} = '' or {DatabaseObjects.Columns.AuthorizedToView} is null ) and ({DatabaseObjects.Columns.IsActivated} = '1' or {DatabaseObjects.Columns.IsActivated} is null)";
            returnQuery = $"{DatabaseObjects.Columns.DashboardType} = '{(int)DashboardType.Query}' and ({DatabaseObjects.Columns.AuthorizedToView} = '{_context.CurrentUser.Id}' or {DatabaseObjects.Columns.AuthorizedToView} = '' or {DatabaseObjects.Columns.AuthorizedToView} is null ) and ({DatabaseObjects.Columns.IsActivated} = '1')";


            if (ddlCat.SelectedValue != "" && ddlCat.SelectedValue != "-1" && ddlCat.SelectedItem.Text.ToLower() != "none")
            {
                if (ddlCat.SelectedValue.Contains("SubCategory--"))
                {
                    categoryFilter = $"{DatabaseObjects.Columns.SubCategory} = '{ddlCat.SelectedItem.Text.Trim()}'";
                }
                else
                {
                    categoryFilter = $"{DatabaseObjects.Columns.CategoryName} = '{ddlCat.SelectedItem.Text.Trim()}'";
                }

                returnQuery = $"{returnQuery} and {categoryFilter}";
            }
            return returnQuery;
        }
    }
}