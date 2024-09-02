using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class ITGBudgetEditor : UserControl
    {
        public string FrameId { get; set; }
        public int BudgetSubCategoryID { get; set; }
        public bool IsReadOnly { get; set; }

        private bool isBindBudgetDone;
        protected bool enableBudgetCategoryType;
        protected DataTable itgBudgetTable;
        protected DataTable itgBudgetActualTable;
        protected bool showCompanyInBudgetList;
        private DataTable authorizedCategoriesTable;
        string lastCategoryType = "-1";
        string lastCategory = string.Empty;
        string lastSubCategory = string.Empty;

        public string budgetReportUrl = string.Empty;
        public string actualsReportUrl = string.Empty;
        LookupValueBoxEdit departmentCtrActual = null;
        LookupValueBoxEdit departmentCtrBudget = null;
        public string departmentLabel;
        protected string currentYear = DateTime.Now.Year.ToString();
        private string yearType = string.Empty;

        private DateTime yearStartDate = DateTime.Now;
        private DateTime yearEndDate = DateTime.Now;
        public ApplicationContext AppContext;
        BudgetCategoryViewManager BudgetCategoryManagerObj = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        ModuleBudgetManager BudgetManagerObj = new ModuleBudgetManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ConfigVarManagerObj = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        BudgetActualsManager BudgetActualsManagerObj = new BudgetActualsManager(HttpContext.Current.GetManagerContext());
        ModuleMonthlyBudgetManager BudgetMonthlyManagerObj = new ModuleMonthlyBudgetManager(HttpContext.Current.GetManagerContext());

        CompanyManager CompanyManagerObj = new CompanyManager(HttpContext.Current.GetManagerContext());
        DepartmentManager DepartmentManagerObj = new DepartmentManager(HttpContext.Current.GetManagerContext());
        FieldConfigurationManager FieldManagerObj = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);

            departmentCtrActual = (LookupValueBoxEdit)ctDepartment_Actual;
            departmentCtrBudget = (LookupValueBoxEdit)ctDepartment_Budget;
            //  lblSelectedYear.Text = DateTime.Now.Date.Year.ToString();
            authorizedCategoriesTable = BudgetCategoryManagerObj.LoadCategories();
            lblSelectedYear.Text = DateTime.Now.Date.Year.ToString();
            // itgGrid.DataBind();
            //  rBudgetInfo.DataBind();
            // updatepanelAcutal.Update();

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["expandedState"] != null)
                {
                    string visibleRows = UGITUtility.GetCookieValue(Request, "expandedState");
                    hfTreeState.Value = Server.UrlDecode(visibleRows);
                }
            }
            if (ddlYear.SelectedIndex != -1 && ddlYear.SelectedValue != "0")
                spancalenderNonbudget.Visible = true;

            //Get the PMM Budget report url
            budgetReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=itgbudgetreport");
            actualsReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=itgactualsreport");
            LoadReportOption();

            if (ConfigVarManagerObj.GetValueAsBool(ConfigurationVariable.EnableBudgetCategoryType))
            {
                divBudgetType.Visible = true;
                enableBudgetCategoryType = true;
            }


            if (enableBudgetCategoryType)
            {
                pEnableCategoryType.Visible = true;
            }

            /// rBudgetInfo.DataBind();
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Check whether current logged in user is authorise to edit item or not.
            if (!IsReadOnly)
            {
                //SPListItem pmmItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMTicket, PMMID);
                //if (!Ticket.IsAuthorizated(pmmItem))
                //{
                //    IsReadOnly = true;
                //}
            }

            spancalenderNonbudget.Visible = false;
            if (ddlYear.SelectedIndex != -1 && ddlYear.SelectedValue != "0")
                spancalenderNonbudget.Visible = true;

            yearType = ddlYear.SelectedItem.Text;
            if (!string.IsNullOrEmpty(hdnCurrentYear.Value))
                currentYear = hdnCurrentYear.Value;
            else
                currentYear = Convert.ToString(DateTime.Now.Year);
            if (yearType.ToLower() == "fiscal year")
            {
                yearStartDate = new DateTime(UGITUtility.StringToInt(currentYear), 4, 1);
                yearEndDate = new DateTime(UGITUtility.StringToInt(currentYear) + 1, 3, 31);
            }
            else if (yearType.ToLower() == "calendar year")
            {
                yearStartDate = new DateTime(UGITUtility.StringToInt(currentYear), 1, 1);
                yearEndDate = new DateTime(UGITUtility.StringToInt(currentYear), 12, 31);
            }
            // Moved from pre-render to on-load.
            itgBudgetTable = GetBudgetData();
            itgBudgetActualTable = GetBudgetActualList();
            // rBudgetInfo.DataBind();
            if (!isBindBudgetDone)
            {
                BindBudgetList();
            }


            ddlBudgetCategories.Items.Clear();
            FillDropDownLevel1(ddlBudgetCategories, new EventArgs());

            //ddlBudgetDepartment.Items.Clear();
            // DDLDepartment_Load(ddlBudgetDepartment, new EventArgs());

            ddlActualCategory.Items.Clear();
            FillDropDopwnLevel1ForActual(ddlActualCategory, new EventArgs());


            ddlActualBudget.Items.Clear();
            DDLBudgetItems_Load(ddlActualBudget, new EventArgs());

            ddlActualVender.Items.Clear();
            DDLVendorActual_Load(ddlActualVender, new EventArgs());

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }
        #region Budget Filter
        protected void CategoryListPanel_Load(object sender, EventArgs e)
        {
            StringBuilder categoryTypeList = new StringBuilder();
            StringBuilder categoryList = new StringBuilder();
            StringBuilder subCategoryList = new StringBuilder();

            if (authorizedCategoriesTable != null)
            {

                string glCode = string.Empty;

                var categoryTypeLookup = authorizedCategoriesTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetType));
                foreach (var categoryType in categoryTypeLookup)
                {
                    DataRow[] categories = categoryType.ToArray();

                    glCode = string.Empty;
                    if (Convert.ToString(UGITUtility.GetSPItemValue(categories[0], DatabaseObjects.Columns.BudgetTypeCOA)) != string.Empty)
                    {
                        glCode = string.Format("{0}: ", UGITUtility.GetSPItemValue(categories[0], DatabaseObjects.Columns.BudgetTypeCOA));
                    }
                    categoryTypeList.AppendFormat("<span class='subcategory-itemspan'><input type='checkbox' value='{1}' cId='{0}' />{2}{0}</span>", string.IsNullOrEmpty(categoryType.Key) ? categoryType.Key : "(none)", categoryType.Key, glCode);
                }

                var categoryLookup = authorizedCategoriesTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategoryName));
                string bType = string.Empty;
                foreach (var category in categoryLookup)
                {
                    DataRow[] subCategories = category.ToArray();
                    if (enableBudgetCategoryType)
                    {
                        bType = Convert.ToString(subCategories[0][DatabaseObjects.Columns.BudgetType]);
                    }

                    glCode = string.Empty;
                    if (Convert.ToString(UGITUtility.GetSPItemValue(subCategories[0], DatabaseObjects.Columns.BudgetCOA)) != string.Empty)
                    {
                        glCode = string.Format("{0}: ", UGITUtility.GetSPItemValue(subCategories[0], DatabaseObjects.Columns.BudgetAcronym));
                    }
                    categoryList.AppendFormat("<span class='subcategory-itemspan'><input type='checkbox' value='{0}' cId='{0}' bType='{2}' />{1}{0}</span>", category.Key, glCode, bType);

                    foreach (DataRow subCategory in subCategories)
                    {
                        glCode = string.Empty;
                        if (Convert.ToString(UGITUtility.GetSPItemValue(subCategory, DatabaseObjects.Columns.BudgetCOA)) != string.Empty)
                        {
                            glCode = string.Format("{0}: ", UGITUtility.GetSPItemValue(subCategory, DatabaseObjects.Columns.BudgetCOA));
                        }
                        subCategoryList.AppendFormat("<span class='subcategory-itemspan' style='display:none;'><input type='checkbox' value='{0}' pId='{2}' sId='{1}' bType='{4}'/>{3}{0}</span>", subCategory[DatabaseObjects.Columns.BudgetSubCategory],
                        subCategory[DatabaseObjects.Columns.Id], category.Key, glCode, bType);
                    }
                    bType = string.Empty;
                }
            }

            subCategoryListPanel.Controls.Add(new LiteralControl(subCategoryList.ToString()));
            categoryListPanel.Controls.Add(new LiteralControl(categoryList.ToString()));
            categoryTypeListPanel.Controls.Add(new LiteralControl(categoryTypeList.ToString()));
        }
        protected void DepartmentListPanel_Load(object sender, EventArgs e)
        {
            Panel departments = (Panel)sender;
            StringBuilder departmentList = new StringBuilder();
            CompanyManager companyManagerObj = new CompanyManager(AppContext);
            List<Company> companies = companyManagerObj.LoadAllHierarchy();  // uGITCache.LoadCompanies(SPContext.Current.Web);

            if (companies.Count > 1)
            {
                showCompanyInBudgetList = UGITUtility.StringToBoolean(ConfigVarManagerObj.GetValue("ShowCompanyNameInBudgetList"));
            }
            string glCode = string.Empty;
            if (companies != null && companies.Count > 0)
            {
                string prefix = "&nbsp;&nbsp;";
                if (companies.Count <= 1)
                {
                    prefix = string.Empty;
                }
                foreach (Company comp in companies)
                {
                    if (companies.Count > 1)
                    {
                        if (!string.IsNullOrEmpty(comp.GLCode))
                        {
                            glCode = string.Format("{0}: ", comp.GLCode);
                        }
                        departmentList.AppendFormat("<span class='company-itemspan'><input type='checkbox' value='{0}' cid='{1}' />{2}{0}</span>", comp.Title, comp.ID, glCode);
                    }
                    foreach (Department depart in comp.Departments)
                    {
                        string divisiontitle = string.Empty;
                        if (comp.CompanyDivisions != null)
                        {
                            CompanyDivision divisionObj = comp.CompanyDivisions.FirstOrDefault(x => x.ID == depart.DivisionIdLookup);
                            if (divisionObj != null)
                                divisiontitle = divisionObj.Title + " > ";
                        }
                        if (!string.IsNullOrEmpty(depart.GLCode))
                        {
                            glCode = string.Format("{0}: ", depart.GLCode);
                        }
                        departmentList.AppendFormat("<span class='department-itemspan'>{3}<input type='checkbox' value='{0}' pid='{2}' did='{1}' />{4}{5}{0}</span>", depart.Title, depart.ID, comp.ID, prefix, glCode, divisiontitle);
                    }
                }
                departments.Controls.Add(new LiteralControl(departmentList.ToString()));
            }
        }
        #endregion

        #region Project Budget
        private DataTable GetBudgetData()
        {
            DataTable table = new DataTable();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", AppContext.TenantID);
            values.Add("@ModuleNameLookup", "ITG");
            if (!string.IsNullOrEmpty(hfSelectedCategories.Text))
            {
                values.Add("@BudgetCategoryLookup", hfSelectedCategories.Text);
                table = GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudget, values);
                //table = BudgetManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup} IN ({hfSelectedCategories.Text})"); 
                // ITGBudget.Load(GetSelectedSubCategories(), SPContext.Current.Web);
            }
            else
            {
                //table = BudgetManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup} > 0 and ModuleNameLookup='ITG'");
                table = GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudget, values);
            }
            //BudgetSubCategoryID
            DataRow[] selectedItems = new DataRow[0];
            if (authorizedCategoriesTable != null && authorizedCategoriesTable.Rows.Count > 0 && table != null && table.Rows.Count > 0)
            {
                selectedItems = (from ac in authorizedCategoriesTable.AsEnumerable()
                                 join b in table.AsEnumerable() on UGITUtility.StringToLong(ac.Field<string>(DatabaseObjects.Columns.ID)) equals UGITUtility.StringToLong(b.Field<Int64>("BudgetCategoryLookup"))
                                 select b).ToArray();
            }

            if (selectedItems.Length > 0)
            {
                table = selectedItems.CopyToDataTable();
            }

            int[] departments = GetSelectedDepartments();
            if (departments.Length > 0 && table != null)
            {
                List<string> expressions = new List<string>();
                foreach (int dID in departments)
                {
                    expressions.Add(string.Format(" DepartmentLookup ={0} ", dID));
                }

                DataRow[] rows = table.Select(string.Join(" or ", expressions.ToArray()));
                if (rows.Length > 0)
                {
                    table = rows.CopyToDataTable();
                }
                else
                {
                    table = table.Clone();
                }
            }
            if (table != null)
            {
                table.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.BudgetCategory);
            }

            DataRow[] filteredData = new DataRow[0];
            if (yearType.ToLower() == "all" && table != null && table.Rows.Count > 0)
                filteredData = table.Select();
            else
                filteredData = table.AsEnumerable().Where(x => yearStartDate.Date <= x.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate).Date && yearEndDate.Date >= x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date).ToArray();

            if (filteredData != null && filteredData.Length > 0)
                table = filteredData.CopyToDataTable();
            else
            {
                table = table.Clone();
                DataRow dr = table.NewRow();
                table.Rows.Add(dr);
            }
            return table;
        }
        protected void FillDropDownLevel1(object sender, EventArgs e)
        {
            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                if (authorizedCategoriesTable != null)
                {
                    List<ListItem> items = BudgetCategoryManagerObj.LoadCategoriesDropDownItems(authorizedCategoriesTable);
                    foreach (ListItem item in items)
                    {
                        level1.Items.Add(item);
                    }
                }
                level1.Items.Insert(0, new ListItem("--Select--", ""));
                level1.SelectedIndex = 0;
            }

            int[] selectedSubCategories = GetSelectedSubCategories();
            if (level1.SelectedIndex <= 0 && selectedSubCategories.Length > 0)
            {
                level1.SelectedIndex = level1.Items.IndexOf(level1.Items.FindByValue(selectedSubCategories[0].ToString()));
            }


        }
        protected void DDLDepartment_Load(object sender, EventArgs e)
        {
            CompanyManager companyManagerObj = new CompanyManager(AppContext);
            DropDownList departments = (DropDownList)sender;
            if (departments.Items.Count <= 0)
            {
                List<Company> companies = companyManagerObj.LoadAllHierarchy();
                if (companies != null && companies.Count > 0)
                {
                    string prefix = "— ";
                    if (companies.Count <= 1)
                    {
                        prefix = string.Empty;
                    }
                    companies = companies.OrderBy(x => x.Name).ToList();
                    foreach (Company comp in companies)
                    {
                        if (companies.Count > 1)
                        {
                            ListItem ddlItem = new ListItem(comp.Name + " " + comp.GLCode, Convert.ToString(comp.ID));
                            ddlItem.Attributes.Add("disabled", "disabled");
                            ddlItem.Attributes.Add("glcode", comp.GLCode);
                            ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "black");
                            ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold");
                            ddlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontStyle, "italic");
                            departments.Items.Add(ddlItem);
                        }
                        comp.Departments = comp.Departments.OrderBy(x => x.Title).ToList();
                        foreach (Department depart in comp.Departments)
                        {
                            string glCode = comp.GLCode;
                            if (!string.IsNullOrEmpty(comp.GLCode))
                            {
                                glCode += "-";
                            }
                            glCode += depart.GLCode;
                            ListItem subCDdlItem = new ListItem(string.Format("{1}{2} {0}", depart.Title, prefix, glCode), Convert.ToString(depart.ID));
                            subCDdlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "10px");
                            subCDdlItem.Attributes.Add("glcode", glCode);
                            departments.Items.Add(subCDdlItem);
                        }
                    }

                    departments.Items.Insert(0, new ListItem("--Select--", ""));
                }
                else
                {
                    departments.Visible = false;
                }
            }

            int[] selectedDepartments = GetSelectedDepartments();
            if (departments.SelectedIndex <= 0 && selectedDepartments.Length > 0)
            {
                departments.SelectedIndex = departments.Items.IndexOf(departments.Items.FindByValue(selectedDepartments[0].ToString()));
            }


        }

        private void DDLActualDepartment_Load(object sender, EventArgs e)
        {

            DropDownList departments = (DropDownList)sender;

            if (departments.Items.Count <= 0)
            {
                itgBudgetTable = GetBudgetData();
                DataTable companyCount = itgBudgetTable.DefaultView.ToTable(true, "Company");
                DataTable tblDepartments = itgBudgetTable.DefaultView.ToTable(true, "DepartmentLookup");

                DataTable companyTable = CompanyManagerObj.GetDataTable(); //.LoadTable(DatabaseObjects.Lists.Company);
                DataTable departmentTable = DepartmentManagerObj.GetDataTable(); // uGITCache.LoadTable(DatabaseObjects.Lists.Department);

                foreach (DataRow dr in tblDepartments.Rows)
                {

                    DataRow[] deptRowCollection = departmentTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, Convert.ToInt32(dr["DepartmentLookup"])));

                    if (deptRowCollection.Length > 0)
                    {
                        DataRow deptRow = deptRowCollection[0];

                        DataRow[] companyRowCollection = companyTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, Convert.ToString(deptRow[DatabaseObjects.Columns.CompanyTitleLookup])));
                        string compGLCode = Convert.ToString(companyRowCollection[0][DatabaseObjects.Columns.GLCode]);

                        string prefix = "— ";
                        if (companyCount.Rows.Count <= 1)
                        {
                            prefix = string.Empty;
                        }

                        string glCode = compGLCode;
                        if (!string.IsNullOrEmpty(compGLCode))
                        {
                            glCode += "-";
                        }
                        glCode += deptRow[DatabaseObjects.Columns.GLCode];

                        ListItem subCDdlItem = new ListItem(string.Format("{1}{2} {0}", deptRow[DatabaseObjects.Columns.Title], prefix, glCode), Convert.ToString(deptRow[DatabaseObjects.Columns.Id]));
                        subCDdlItem.Attributes.CssStyle.Add(HtmlTextWriterStyle.PaddingLeft, "10px");
                        subCDdlItem.Attributes.Add("glcode", glCode);
                        departments.Items.Add(subCDdlItem);
                    }
                }

                departments.Items.Insert(0, new ListItem("--Select--", ""));

                int[] selectedDepartments = GetSelectedDepartments();
                if (departments.SelectedIndex <= 0 && selectedDepartments.Length > 0)
                {
                    departments.SelectedIndex = departments.Items.IndexOf(departments.Items.FindByValue(selectedDepartments[0].ToString()));
                }
            }
        }

        private void BindBudgetList()
        {

            rBudgetInfo.DataSource = itgBudgetTable;
            rBudgetInfo.DataBind();
        }

        private void UpdateMonthlyBudget(long oldSubCategoryID, long newSubCategoryID)
        {

            //re distribute budget for oldsubcategoryid if it is not same
            if (oldSubCategoryID != -1 && oldSubCategoryID != newSubCategoryID)
            {
                ////Get budgets of old sub category
                //SPQuery bQuery = new SPQuery();
                //bQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, oldSubCategoryID);
                //DataTable budgetTable = itgBudgetList.GetItems(bQuery).GetDataTable();
                DataTable budgetTable = BudgetManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup}={oldSubCategoryID} And {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");

                ////Gets previous monthly distribution of old subcategory
                ////otherwise set budgetamount to 0
                //SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, oldSubCategoryID);
                //SPListItemCollection oldBudgetDistributions = monthlyBudgetList.GetItems(query);
                List<ModuleMonthlyBudget> oldBudgetDistributions = BudgetMonthlyManagerObj.Load(x => x.BudgetCategoryLookup == oldSubCategoryID && x.ModuleName == ModuleNames.ITG);
                foreach (ModuleMonthlyBudget oldItem in oldBudgetDistributions)
                {
                    oldItem.BudgetAmount = 0;
                    oldItem.NonProjectPlanedTotal = 0;
                    //  oldItem[DatabaseObjects.Columns.NonProjectActualTotal] = 0;
                    BudgetMonthlyManagerObj.Update(oldItem);
                }

                //distribute budget amount for old sub category
                if (budgetTable != null && budgetTable.Rows.Count > 0)
                {
                    foreach (DataRow row in budgetTable.Rows)
                    {
                        //SPQuery queryT = new SPQuery();
                        //queryT.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, oldSubCategoryID);
                        //SPListItemCollection oldBudgetDistributionsC = monthlyBudgetList.GetItems(queryT);
                        DataTable oldBudgetDistributionsTable = BudgetMonthlyManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup}={oldSubCategoryID} And {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");

                        DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                        DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                        double totalAmount = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.BudgetAmount)));
                        Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(AppContext, startDate, endDate, totalAmount);
                        foreach (DateTime key in amountDistributions.Keys)
                        {
                            double val = amountDistributions[key];
                            DataRow preItem = null;
                            if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                            {
                                preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                            }

                            ModuleMonthlyBudget item = null;
                            if (preItem != null)
                            {
                                item = BudgetMonthlyManagerObj.LoadByID(UGITUtility.StringToLong(preItem[DatabaseObjects.Columns.Id])); // SPListHelper.GetSPListItem(monthlyBudgetList, (int)preItem[DatabaseObjects.Columns.Id]);
                                item.BudgetCategoryLookup = newSubCategoryID;
                                item.BudgetType = "0";
                                item.ModuleName = ModuleNames.ITG;
                                item.BudgetAmount = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(preItem, DatabaseObjects.Columns.BudgetAmount))) + val;
                                item.NonProjectPlanedTotal = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(preItem, DatabaseObjects.Columns.NonProjectPlannedTotal))) + val;
                            }
                            else
                            {
                                item = new ModuleMonthlyBudget();
                                item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                item.EstimatedCost = 0;
                                item.ResourceCost = 0;
                                item.ActualCost = 0;
                                item.BudgetType = "0";
                                item.ModuleName = ModuleNames.ITG;
                                item.BudgetCategoryLookup = newSubCategoryID;
                                item.BudgetAmount = val;
                                item.NonProjectPlanedTotal = val;
                            }

                            try
                            {
                                BudgetMonthlyManagerObj.Update(item);
                            }
                            catch (Exception ex)
                            {
                                uGovernIT.Util.Log.ULog.WriteException(ex, "Error updating ITG Actual monthly totals from actual: " + row[DatabaseObjects.Columns.Title]);
                            }
                        }
                    }
                }
            }

            // When we adding or update a new Item in existing subcategory.
            //SPQuery mbQuery = new SPQuery();
            //mbQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, newSubCategoryID);
            //SPListItemCollection newBudgetDistributions = monthlyBudgetList.GetItems(mbQuery);
            List<ModuleMonthlyBudget> newBudgetDistributions = BudgetMonthlyManagerObj.Load(x => x.BudgetCategoryLookup == newSubCategoryID && x.ModuleName == ModuleNames.ITG);
            foreach (ModuleMonthlyBudget nItem in newBudgetDistributions)
            {
                nItem.BudgetAmount = 0;
                nItem.NonProjectPlanedTotal = 0;
                BudgetMonthlyManagerObj.Update(nItem);
                //nItem.Update();
            }

            //SPQuery budgetQuery = new SPQuery();
            //budgetQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, newSubCategoryID);
            DataTable budgetCollection = BudgetManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup}={newSubCategoryID} And {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");
            if (budgetCollection == null || budgetCollection.Rows.Count <= 0)
            {
                return;
            }

            foreach (DataRow row in budgetCollection.Rows)
            {
                //SPQuery queryT = new SPQuery();
                //queryT.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, newSubCategoryID);
                //SPListItemCollection oldBudgetDistributionsC = monthlyBudgetList.GetItems(queryT);
                DataTable oldBudgetDistributionsTable = BudgetMonthlyManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup}={newSubCategoryID} And {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");

                DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                double totalAmount = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.BudgetAmount)));
                Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(AppContext, startDate, endDate, totalAmount);

                foreach (DateTime key in amountDistributions.Keys)
                {
                    double val = amountDistributions[key];
                    DataRow preItem = null;
                    if (oldBudgetDistributionsTable != null && oldBudgetDistributionsTable.Rows.Count > 0)
                    {
                        preItem = oldBudgetDistributionsTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date && x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);
                    }

                    ModuleMonthlyBudget item = null;
                    if (preItem != null)
                    {
                        item = BudgetMonthlyManagerObj.LoadByID(UGITUtility.StringToLong(preItem[DatabaseObjects.Columns.ID])); // SPListHelper.GetSPListItem(monthlyBudgetList, (int)preItem[DatabaseObjects.Columns.Id]);
                        item.BudgetCategoryLookup = newSubCategoryID;
                        item.BudgetType = "0";
                        item.ModuleName = ModuleNames.ITG;
                        item.BudgetAmount = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(preItem, DatabaseObjects.Columns.BudgetAmount))) + val;
                        item.NonProjectPlanedTotal = Convert.ToDouble(Convert.ToString(UGITUtility.GetSPItemValue(preItem, DatabaseObjects.Columns.BudgetAmount))) + val;
                        BudgetMonthlyManagerObj.Update(item);
                    }
                    else
                    {
                        item = new ModuleMonthlyBudget();
                        item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                        item.EstimatedCost = 0;
                        item.ResourceCost = 0;
                        item.ActualCost = 0;
                        item.BudgetType = "0";
                        item.ModuleName = ModuleNames.ITG;
                        item.BudgetCategoryLookup = newSubCategoryID;
                        item.BudgetAmount = val;
                        item.NonProjectPlanedTotal = val;
                        BudgetMonthlyManagerObj.Update(item);
                    }
                }
            }
        }

        protected void BtDeleteBudget_Click(object sender, EventArgs e)
        {
            ModuleBudgetActualsHistoryManager budgetAcutalHistoryManagerObj = new ModuleBudgetActualsHistoryManager(AppContext);

            long budgetItemID = 0;
            long.TryParse(hfEditBudgetID.Value, out budgetItemID);
            if (budgetItemID > 0)
            {
                ModuleBudget budget = BudgetManagerObj.LoadByID(budgetItemID);

                List<ModuleBudgetsActualHistory> lstBudgetActualHistory = budgetAcutalHistoryManagerObj.Load(x => x.ModuleBudgetLookup == budgetItemID);
                foreach (ModuleBudgetsActualHistory actualHistory in lstBudgetActualHistory)
                    budgetAcutalHistoryManagerObj.Delete(actualHistory);

                List<BudgetActual> budgetActuals = BudgetActualsManagerObj.Load(x => x.ModuleBudgetLookup == budgetItemID);
                foreach (BudgetActual actual in budgetActuals)
                {
                    // actual.ITGBudgetItem = null;
                    //UpdateActualAllocation(actual);
                    BudgetActualsManagerObj.Delete(actual);
                }

                string historyDescription = string.Format("Deleted budget Item: {0}(GLCode:{1})", budget.BudgetItem, budget.GLCode);

                if (BudgetManagerObj.Delete(budget))
                {
                    //create history
                    //SPListItem itg = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ITGovernance, 1);
                    //uHelper.CreateHistory(AppContext.CurrentUser, historyDescription, itg, AppContext);

                    //Update monthly distribution after deleting budget item
                    UpdateMonthlyBudget(-1, budget.BudgetCategoryLookup);
                    UpdateMonthlyDistribution(budget.BudgetCategoryLookup, budgetItemID);
                    budgetMessage.Text = "Budget Item deleted";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    budgetMessage.Text = "Error in deleting";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
                updatepanelAcutal.Update();
            }
        }

        string categoryTypeWhereClause = string.Empty;
        string categoryWhereClause = string.Empty;
        string subCategoryWhereClause = string.Empty;

        protected void RBudgetInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem rItem = (RepeaterItem)e.Item;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                if (string.IsNullOrEmpty(Convert.ToString(rowView["BudgetCategoryLookup"])))
                    return;
                int budgetSubCategoryID = Convert.ToInt32(rowView["BudgetCategoryLookup"]);
                int budgetItemID = Convert.ToInt32(rowView[DatabaseObjects.Columns.Id]);
                BudgetCategory currentbudgetCategory = BudgetCategoryManagerObj.LoadByID(budgetSubCategoryID);
                if (currentbudgetCategory == null)
                    return;

                //Process Category Type if enabled
                HtmlTableRow trCategoryType = (HtmlTableRow)rItem.FindControl("trCategoryType");
                if (enableBudgetCategoryType)
                {
                    Label lbCategoryType = (Label)rItem.FindControl("lbCategoryType");
                    if (lastCategoryType.ToLower() != Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]).ToLower())
                    {
                        lastCategoryType = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]);
                        lbCategoryType.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetType]);
                        if (lbCategoryType.Text.Trim() == string.Empty)
                        {
                            lbCategoryType.Text = "(None)";
                        }
                        trCategoryType.Visible = true;
                        trCategoryType.Attributes.Add("parent", "0");
                        trCategoryType.Attributes.Add("current", string.Format("CT-{0}", budgetSubCategoryID));

                        DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;

                        categoryTypeWhereClause = string.Format(" {0}='{1}' ", DatabaseObjects.Columns.BudgetType, lastCategoryType);
                        if (string.IsNullOrEmpty(lastCategoryType))
                        {
                            categoryTypeWhereClause = string.Format(" ({0} is null or {0} = '') ", DatabaseObjects.Columns.BudgetType);
                        }
                        DataRow[] budgetRows = budgetTable.Select(categoryTypeWhereClause);

                        //Calculate categorytype Planned Amount
                        Label lbCategoryTypeAmount = (Label)rItem.FindControl("lbCategoryTypeAmount");
                        lbCategoryTypeAmount.Text = string.Format("{0:C}", budgetRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount)));

                        //Calculate Categorytype Axtual Amount
                        Label lbCategoryTypeActualAmount = (Label)rItem.FindControl("lbCategoryTypeActualAmount");
                        lbCategoryTypeActualAmount.Text = string.Format("{0:C}", 0);
                        if (itgBudgetActualTable != null && itgBudgetActualTable.Rows.Count > 0 && budgetRows.Length > 0)
                        {
                            DataRow[] actualRows = (from ib in itgBudgetActualTable.AsEnumerable()
                                                    join ia in budgetRows on ib.Field<string>("ModuleBudgetLookup") equals ia.Field<string>(DatabaseObjects.Columns.Id)
                                                    select ib).ToArray();
                            lbCategoryTypeActualAmount.Text = string.Format("{0:C}", actualRows.Sum(x => x.Field<double>(DatabaseObjects.Columns.ActualCost)));
                        }
                    }
                }

                //Category
                HtmlTableRow trCategory = (HtmlTableRow)rItem.FindControl("trCategory");
                trCategory.Attributes.Add("current", Convert.ToString(rowView["BudgetCategoryLookup"]));
                trCategory.Attributes.Add("budgettype", UGITUtility.ObjectToString(currentbudgetCategory?.BudgetType));
                HtmlTableCell tdCategory = (HtmlTableCell)rItem.FindControl("tdCategory");

                Label lbCategory = (Label)rItem.FindControl("lbCategory");
                if (lastCategory.ToLower() != Convert.ToString(currentbudgetCategory.BudgetCategoryName).ToLower())
                {
                    lastCategory = Convert.ToString(currentbudgetCategory.BudgetCategoryName);
                    lbCategory.Text = Convert.ToString(currentbudgetCategory.BudgetCategoryName);
                    trCategory.Visible = true;
                    trCategory.Attributes.Add("parent", "0");
                    if (enableBudgetCategoryType)
                    {
                        trCategory.Attributes.Add("parent", string.Format("CT-{0}", budgetSubCategoryID));
                    }
                    trCategory.Attributes.Add("current", string.Format("C-{0}", budgetSubCategoryID));


                    DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;
                    //categoryWhereClause = string.Format("{0}='{1}'", "BudgetCategoryLookup", budgetSubCategoryID);
                    categoryWhereClause = string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategory, lastCategory);
                    //categoryWhereClause = string.Format("{0}='{1}' and {2}='{3}'", "BudgetCategoryLookup", budgetSubCategoryID, DatabaseObjects.Columns.BudgetCategory, lastCategory);
                    if (categoryTypeWhereClause != string.Empty)
                    {
                        categoryWhereClause = string.Format(" {0} and {1}='{2}' ", categoryTypeWhereClause, DatabaseObjects.Columns.BudgetCategory, lastCategory);
                    }
                    DataRow[] categoryPlannedRows = budgetTable.Select(categoryWhereClause);

                    //Calculate Category Planned Amount
                    Label lbSubTotalAmount = (Label)rItem.FindControl("lbSubTotalAmount");
                    lbSubTotalAmount.Text = string.Format("{0:C}", 0);
                    if (categoryPlannedRows.Length > 0)
                    {
                        lbSubTotalAmount.Text = string.Format("{0:C}", categoryPlannedRows.Sum(x => UGITUtility.StringToDouble(x.Field<double>(DatabaseObjects.Columns.BudgetAmount))));
                    }

                    //Calculate Category Actual Amount
                    Label lbSubTotalActualAmount = (Label)rItem.FindControl("lbSubTotalActualAmount");
                    lbSubTotalActualAmount.Text = string.Format("{0:C}", 0);
                    if (itgBudgetActualTable != null && itgBudgetActualTable.Rows.Count > 0 && categoryPlannedRows.Length > 0)
                    {
                        DataRow[] actualRows = (from ib in itgBudgetActualTable.AsEnumerable()
                                                join ia in categoryPlannedRows on UGITUtility.ObjectToString(ib.Field<Int64>("ModuleBudgetLookup")) equals UGITUtility.ObjectToString(ia.Field<Int64>(DatabaseObjects.Columns.ID))
                                                select ib).ToArray();
                        lbSubTotalActualAmount.Text = string.Format("{0:C}", actualRows.Sum(x => UGITUtility.StringToDouble(x.Field<double>(DatabaseObjects.Columns.ActualCost))));
                    }


                }


                //SubCategory
                HtmlTableRow trSubCategory = (HtmlTableRow)rItem.FindControl("trSubCategory");
                trSubCategory.Visible = false;
                Label lbSubCategory = (Label)rItem.FindControl("lbSubCategory");
                if (lastSubCategory.ToLower() != Convert.ToString(currentbudgetCategory.BudgetSubCategory).ToLower())
                {
                    lastSubCategory = Convert.ToString(currentbudgetCategory.BudgetSubCategory);
                    lbSubCategory.Text = Convert.ToString(currentbudgetCategory.BudgetSubCategory);
                    trSubCategory.Visible = true;
                    trSubCategory.Attributes.Add("parent", string.Format("C-{0}", budgetSubCategoryID));
                    trSubCategory.Attributes.Add("current", string.Format("SC-{0}", budgetSubCategoryID));



                    DataTable budgetTable = (DataTable)rBudgetInfo.DataSource;
                    subCategoryWhereClause = string.Format(" {0}='{1}' ", DatabaseObjects.Columns.BudgetCategoryLookup, budgetSubCategoryID);
                    if (categoryWhereClause != string.Empty)
                    {
                        //subCategoryWhereClause = string.Format("{0} or {1}='{2}'", categoryWhereClause, DatabaseObjects.Columns.BudgetCategory, lastSubCategory);
                    }
                    DataRow[] subCategoryPlannedRows = budgetTable.Select(subCategoryWhereClause);

                    //Calculate Category Planned Amount
                    Label lbSubCategoryAmount = (Label)rItem.FindControl("lbSubCategoryAmount");
                    lbSubCategoryAmount.Text = string.Format("{0:C}", 0);
                    if (subCategoryPlannedRows.Length > 0)
                    {
                        lbSubCategoryAmount.Text = string.Format("{0:C}", subCategoryPlannedRows.Sum(x => UGITUtility.StringToDouble(x.Field<double>(DatabaseObjects.Columns.BudgetAmount))));
                    }

                    //Calculate Category Actual Amount
                    Label lbSubCategoryActualAmount = (Label)rItem.FindControl("lbSubCategoryActualAmount");
                    lbSubCategoryActualAmount.Text = string.Format("{0:C}", 0);
                    if (itgBudgetActualTable != null && itgBudgetActualTable.Rows.Count > 0 && subCategoryPlannedRows.Length > 0)
                    {
                        DataRow[] actualRows = (from ib in itgBudgetActualTable.AsEnumerable()
                                                join ia in subCategoryPlannedRows on UGITUtility.ObjectToString(ib.Field<Int64>(DatabaseObjects.Columns.ModuleBudgetLookup)) equals UGITUtility.ObjectToString(ia.Field<Int64>(DatabaseObjects.Columns.ID))
                                                select ib).ToArray();
                        lbSubCategoryActualAmount.Text = string.Format("{0:C}", actualRows.Sum(x => UGITUtility.StringToDouble(x.Field<double>(DatabaseObjects.Columns.ActualCost))));
                    }
                }


                //BudgetItems
                HtmlTableRow trBudgetItem = (HtmlTableRow)rItem.FindControl("trBudgetItem");
                trBudgetItem.Visible = true;
                trBudgetItem.Attributes.Add("parent", string.Format("SC-{0}", budgetSubCategoryID));
                trBudgetItem.Attributes.Add("current", string.Format("B-{0}", budgetItemID));

                if (e.Item.ItemIndex % 2 == 0)
                {
                    trBudgetItem.Attributes.Add("class", string.Format("{0} bugdet-itemalt", trBudgetItem.Attributes["class"]));
                }

                HiddenField hfSubCategoryID = (HiddenField)rItem.FindControl("hfSubCategoryID");
                Label lbItemInfo = (Label)rItem.FindControl("lbItemInfo");
                //Label lbSubCategory = (Label)rItem.FindControl("lbSubCategory");
                Label lbDepartment = (Label)rItem.FindControl("lbDepartment");
                Label lbBudgetCOA = (Label)rItem.FindControl("lbBudgetCOA");
                Label lbBudgetItem = (Label)rItem.FindControl("lbBudgetItem");
                Label lbAmount = (Label)rItem.FindControl("lbAmount");
                Label lbStartDate = (Label)rItem.FindControl("lbStartDate");
                Label lbEndDate = (Label)rItem.FindControl("lbEndDate");
                Label lbActualAmount = (Label)rItem.FindControl("lbActualAmount");
                // CheckBox cbBudgetItem = (CheckBox)rItem.FindControl("cbBudgetItem");

                HtmlImage expcollImg3 = (HtmlImage)rItem.FindControl("expcollImg3");
                expcollImg3.Visible = false;
                Repeater rBudgetActuals = (Repeater)rItem.FindControl("rBudgetActuals");
                lbActualAmount.Text = String.Format("{0:C}", 0);

                if (itgBudgetActualTable != null && itgBudgetActualTable.Rows.Count > 0)
                {
                    DataRow[] rows = itgBudgetActualTable.AsEnumerable().Where(x => UGITUtility.StringToLong(x.Field<Int64>("ModuleBudgetLookup")) == UGITUtility.StringToLong(rowView[DatabaseObjects.Columns.ID])).ToArray();
                    if (rows.Length > 0)
                    {
                        if (itgBudgetActualTable.Columns.Contains("DepartmentLookup"))
                            rows.AsEnumerable().ToList().ForEach(x => UpdateDepartmentLookup(x, Convert.ToString(rowView["DepartmentLookup"])));

                        rBudgetActuals.DataSource = rows.OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate)).ThenByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate)).ToArray();
                        rBudgetActuals.DataBind();
                        lbActualAmount.Text = String.Format("{0:C}", rows.Sum(x => UGITUtility.StringToDouble(x.Field<double>(DatabaseObjects.Columns.ActualCost))));
                        expcollImg3.Visible = true;
                    }
                }

                hfSubCategoryID.Value = Convert.ToString(rowView[DatabaseObjects.Columns.Id]);
                //  lbSubCategory.Text = Convert.ToString(rowView[DatabaseObjects.Columns.BudgetSubCategory]);
                //FieldConfiguration departmentField = FieldManagerObj.GetFieldByFieldName(DatabaseObjects.Columns.DepartmentLookup);
                string departmentText = FieldManagerObj.GetFieldConfigurationData(DatabaseObjects.Columns.DepartmentLookup, Convert.ToString(rowView[DatabaseObjects.Columns.DepartmentLookup]));
                lbDepartment.Text = departmentText;
                lbBudgetCOA.Text = Convert.ToString(rowView[DatabaseObjects.Columns.GLCode]);
                lbBudgetItem.Text = Convert.ToString(rowView[DatabaseObjects.Columns.Title]);
                lbAmount.Text = Convert.ToString(String.Format("{0:C}", UGITUtility.StringToDouble(rowView[DatabaseObjects.Columns.BudgetAmount])));
                lbStartDate.Text = ((DateTime)rowView[DatabaseObjects.Columns.AllocationStartDate]).ToString("MMM-d-yyyy");
                lbEndDate.Text = ((DateTime)rowView[DatabaseObjects.Columns.AllocationEndDate]).ToString("MMM-d-yyyy");

                string itemInfoJson = string.Format("{0}\"itemid\":\"{2}\",\"cid\":\"{3}\",\"did\":\"{4}\",\"title\":\"{5}\",\"glcode\":\"{6}\",\"amount\":\"{7}\",\"startdate\":\"{8}\",\"enddate\":\"{9}\"{1}",
                    "{", "}", UGITUtility.ObjectToString(rowView[DatabaseObjects.Columns.ID]),
                   UGITUtility.ObjectToString(currentbudgetCategory.ID),
                   UGITUtility.ObjectToString(rowView["DepartmentLookup"]),
                   UGITUtility.ObjectToString(rowView[DatabaseObjects.Columns.BudgetItem]),
                   UGITUtility.ObjectToString(rowView[DatabaseObjects.Columns.GLCode]),
                   UGITUtility.ObjectToString(rowView[DatabaseObjects.Columns.BudgetAmount]),
                   ((DateTime)rowView[DatabaseObjects.Columns.AllocationStartDate]).ToString("MM/dd/yyyy"),
                   ((DateTime)rowView[DatabaseObjects.Columns.AllocationEndDate]).ToString("MM/dd/yyyy"));
                lbItemInfo.Text = itemInfoJson;

                // HtmlImage btEdit = (HtmlImage)rItem.FindControl("btEdit");
                // btEdit.Attributes.Add("itemID", Convert.ToString(rowView[DatabaseObjects.Columns.Id]));

                //if (selectedBudgets.Length > 0 && selectedBudgets.FirstOrDefault(x=>x == int.Parse(hfSubCategoryID.Value)) > 0)
                //{
                //    cbBudgetItem.Checked = true;
                //}

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DataTable budgetTable1 = (DataTable)rBudgetInfo.DataSource;
                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(budgetTable1.Rows[0][DatabaseObjects.Columns.ID])))
                    return;
                //Calcuate Grand totals
                HtmlTableRow trSubTotal = (HtmlTableRow)rItem.FindControl("trTotal");
                trSubTotal.Visible = true;

                Label lbTotalAmount = (Label)rItem.FindControl("lbTotalAmount");
                if (budgetTable1 != null && budgetTable1.Rows.Count > 0)
                {
                    lbTotalAmount.Text = string.Format("{0:C}", budgetTable1.AsEnumerable().Sum(r => UGITUtility.StringToDouble(r.Field<double>(DatabaseObjects.Columns.BudgetAmount))));
                    //lbTotalAmount.Text = string.Format("{0:C}", budgetTable1.Compute(string.Format("Sum(Convert({0},'System.Double'))", DatabaseObjects.Columns.BudgetAmount),string.Empty));
                }

                Label lbTotalActualAmount = (Label)rItem.FindControl("lbTotalActualAmount");
                lbTotalActualAmount.Text = string.Format("{0:C}", 0);
                if (itgBudgetActualTable != null && itgBudgetActualTable.Rows.Count > 0)
                {
                    lbTotalActualAmount.Text = string.Format("{0:C}", itgBudgetActualTable.AsEnumerable().Sum(r => UGITUtility.StringToDouble(r.Field<double>(DatabaseObjects.Columns.ActualCost))));
                    //lbTotalActualAmount.Text = string.Format("{0:C}", itgBudgetActualTable.Compute(string.Format("Sum(Convert({0},'System.Double'))", DatabaseObjects.Columns.BudgetAmount),
                    //    string.Format("{0} is not null AND {0} > 0", "ModuleBudgetLookup")));
                }
            }
        }

        protected void UpdateDepartmentLookup(DataRow updationRow, String updateWith)
        {
            updationRow["DepartmentLookup"] = updateWith;
        }

        protected void RBudgetActuals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            RepeaterItem rItem = (RepeaterItem)e.Item;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DataRow rowView = (DataRow)e.Item.DataItem;
                long budgetItemID = UGITUtility.StringToLong(rowView["ModuleBudgetLookup"]);
                long actualItemID = UGITUtility.StringToLong(rowView[DatabaseObjects.Columns.ID]);

                HtmlTableRow trBudgetActualItem = (HtmlTableRow)rItem.FindControl("trBudgetActualItem");
                if (e.Item.ItemIndex % 2 != 0)
                {
                    trBudgetActualItem.Attributes.Add("Class", "bitem actual-item ms-alternatingstrong");
                }
                trBudgetActualItem.Attributes.Add("parent", string.Format("B-{0}", budgetItemID));
                trBudgetActualItem.Attributes.Add("current", string.Format("A-{0}", actualItemID));

                Label lbBATitle = (Label)rItem.FindControl("lbBATitle");
                Label lbBAVendorLookup = (Label)rItem.FindControl("lbBAVendorLookup");
                Label lbBAInvoiceNumber = (Label)rItem.FindControl("lbBAInvoiceNumber");
                Label lbBAActualCost = (Label)rItem.FindControl("lbBAActualCost");
                Label lbBABudgetStartDate = (Label)rItem.FindControl("lbBABudgetStartDate");
                Label lbBABudgetEndDate = (Label)rItem.FindControl("lbBABudgetEndDate");

                lbBATitle.Text = string.Format("{0}", rowView[DatabaseObjects.Columns.Title]);

                lbBAVendorLookup.Text = "-None-";
                if (!string.IsNullOrEmpty(Convert.ToString(rowView[DatabaseObjects.Columns.VendorLookup])))
                {
                    FieldConfiguration fieldconfigObj = FieldManagerObj.GetFieldByFieldName(DatabaseObjects.Columns.VendorLookup);
                    lbBAVendorLookup.Text = FieldManagerObj.GetFieldConfigurationData(fieldconfigObj, Convert.ToString(rowView[DatabaseObjects.Columns.VendorLookup]));
                }

                lbBAInvoiceNumber.Text = string.Format("{0}", rowView[DatabaseObjects.Columns.InvoiceNumber]);
                lbBAActualCost.Text = String.Format("{0:C}", UGITUtility.StringToDouble(rowView[DatabaseObjects.Columns.ActualCost]));


                lbBABudgetStartDate.Text = string.Format("{0}", ((DateTime)rowView[DatabaseObjects.Columns.AllocationStartDate]).ToString("MMM-d-yyyy"));
                lbBABudgetEndDate.Text = string.Format("{0}", ((DateTime)rowView[DatabaseObjects.Columns.AllocationEndDate]).ToString("MMM-d-yyyy"));


                Label budgetActualInfo = (Label)rItem.FindControl("lbBudgetActualInfo");
                string itemInfoJson = string.Format("{0}\"itemid\":\"{2}\",\"bid\":\"{3}\",\"did\":\"{10}\",\"vid\":\"{4}\",\"invoiceno\":\"{5}\",\"title\":\"{6}\",\"amount\":\"{7}\",\"startdate\":\"{8}\",\"enddate\":\"{9}\"{1}",
                     "{", "}", rowView[DatabaseObjects.Columns.Id], rowView["ModuleBudgetLookup"], rowView["VendorLookup"], rowView[DatabaseObjects.Columns.InvoiceNumber], rowView[DatabaseObjects.Columns.Title], rowView[DatabaseObjects.Columns.ActualCost], ((DateTime)rowView[DatabaseObjects.Columns.AllocationStartDate]).ToString("MM/dd/yyyy"), ((DateTime)rowView[DatabaseObjects.Columns.AllocationEndDate]).ToString("MM/dd/yyyy"), rowView["DepartmentLookup"]);
                budgetActualInfo.Text = itemInfoJson;
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                RepeaterItem parentItem = ((RepeaterItem)e.Item.Parent.Parent);
                DataRowView rowView = (DataRowView)parentItem.DataItem;

                HtmlTableRow trBudgetActualItemHead = (HtmlTableRow)rItem.FindControl("trBudgetActualItemHead");
                trBudgetActualItemHead.Attributes.Add("parent", string.Format("B-{0}", rowView[DatabaseObjects.Columns.Id]));
                trBudgetActualItemHead.Attributes.Add("current", string.Format("A-header"));

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                RepeaterItem parentItem = ((RepeaterItem)e.Item.Parent.Parent);
                DataRowView rowView = (DataRowView)parentItem.DataItem;

                HtmlTableRow trBudgetActualItemFooter = (HtmlTableRow)rItem.FindControl("trBudgetActualItemFooter");
                trBudgetActualItemFooter.Attributes.Add("parent", string.Format("B-{0}", rowView[DatabaseObjects.Columns.Id]));
                trBudgetActualItemFooter.Attributes.Add("current", string.Format("A-footer"));
            }
        }

        protected void BtBudgetSave_Click(object sender, EventArgs e)
        {
            long budgetID = 0;
            long.TryParse(hfEditBudgetID.Value, out budgetID);

            ModuleBudget budgetItem = null;
            if (budgetID > 0)
            {
                budgetItem = BudgetManagerObj.LoadByID(budgetID);
            }

            if (budgetItem != null)
            {
                UpdateAllocation(budgetItem);
            }
            else
            {
                NewAllocation();
            }
            ClearBudgetForm();
        }

        private void NewAllocation()
        {
            ModuleBudget budget = new ModuleBudget();

            if (ValidateBudgetForm())
            {
                budget.budgetCategory = new BudgetCategory();
                long budgetCategoryId = 0;
                long.TryParse(ddlBudgetCategories.SelectedValue, out budgetCategoryId);
                budget.budgetCategory = BudgetCategoryManagerObj.GetBudgetCategoryById(budgetCategoryId);
                budget.BudgetCategoryLookup = budgetCategoryId;

                budget.BudgetAmount = double.Parse(txtBudgetAmountf.Text.Trim());
                budget.BudgetItem = txtBudgetItemVal.Text.Trim();
                budget.ModuleName = ModuleNames.ITG;

                budget.AllocationStartDate = dtcBudgetStartDate.Date;
                if (dtcActualStartDate.Date != DateTime.MinValue)
                    budget.AllocationStartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

                budget.AllocationEndDate = dtcBudgetEndDate.Date;
                if (dtcActualStartDate.Date != DateTime.MinValue)
                    budget.AllocationEndDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));


                long departmentID = 0;
                if (!string.IsNullOrEmpty(departmentCtrBudget.GetValues()))
                {
                    departmentID = UGITUtility.StringToLong(departmentCtrBudget.GetValues());
                }
                budget.DepartmentLookup = departmentID;

                CompanyManager companyManagerObj = new CompanyManager(AppContext);
                List<Company> lstCompanies = companyManagerObj.LoadAllHierarchy();
                DepartmentManager dHelper = new DepartmentManager(AppContext);
                Department departmentObj = dHelper.LoadByID(departmentID);
                if (departmentObj != null)
                    budget.GLCode = uHelper.GenerateGLCode(budget.budgetCategory, departmentObj, lstCompanies, ConfigVarManagerObj.GetValueAsBool(ConfigConstants.EnableDivision));
                if (BudgetManagerObj.Insert(budget) > 0)
                {
                    UpdateMonthlyBudget(-1, budget.BudgetCategoryLookup);
                    budgetMessage.Text = "Budget Inserted Successfully.";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;

                    BindBudgetList();
                    budgetPopupBox.ShowOnPageLoad = false;
                }
                else
                {
                    BindBudgetList();
                    budgetMessage.Text = "Error creating Budget";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private bool UpdateAllocation(ModuleBudget budgetItem)
        {
            bool isUpdated = false;

            if (ValidateBudgetForm())
            {

                long oldSubCategoryID = budgetItem.BudgetCategoryLookup;
                BudgetCategory savedbudgetCategory = BudgetCategoryManagerObj.LoadByID(oldSubCategoryID);
                string oldGLCode = budgetItem.GLCode;
                double oldAmount = budgetItem.BudgetAmount;
                DateTime oldStartDate = budgetItem.AllocationStartDate;
                DateTime oldEndDate = budgetItem.AllocationEndDate;
                string oldTitle = budgetItem.BudgetItem;
                string oldSubCategory = savedbudgetCategory.BudgetSubCategory;

                long budgetCategoryId = 0;
                long.TryParse(ddlBudgetCategories.SelectedValue, out budgetCategoryId);
                budgetItem.budgetCategory = BudgetCategoryManagerObj.GetBudgetCategoryById(budgetCategoryId);
                budgetItem.BudgetAmount = double.Parse(txtBudgetAmountf.Text.Trim());

                budgetItem.BudgetItem = txtBudgetItemVal.Text.Trim();
                budgetItem.AllocationStartDate = dtcBudgetStartDate.Date;
                budgetItem.AllocationEndDate = dtcBudgetEndDate.Date;
                long departmentID = 0;
                if (!string.IsNullOrEmpty(departmentCtrBudget.GetValues()))
                {
                    departmentID = UGITUtility.StringToLong(departmentCtrBudget.GetValues());
                }

                //DepartmentHelper dHelper = new DepartmentHelper(SPContext.Current.Web);
                //budgetItem.DepartmentV = dHelper.Load(departmentID);

                //budgetItem.GLCode = ITGBudget.GenerateGLCode(budgetItem.budgetCategory, budgetItem.DepartmentV, uGITCache.LoadCompanies(SPContext.Current.Web), uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableDivision));


                if (BudgetManagerObj.Update(budgetItem))
                {
                    //Calls updatemonthbudget only in case of change in amount, start, enddate, category
                    if (oldAmount != budgetItem.BudgetAmount || oldSubCategoryID != budgetItem.BudgetCategoryLookup || oldStartDate.Date != budgetItem.AllocationStartDate || oldEndDate.Date != budgetItem.AllocationEndDate)
                    {
                        UpdateMonthlyBudget(oldSubCategoryID, budgetItem.BudgetCategoryLookup);
                    }

                    //Create history
                    if (oldTitle != budgetItem.BudgetItem || oldAmount != budgetItem.BudgetAmount || oldGLCode != budgetItem.GLCode || oldStartDate.Date != budgetItem.AllocationStartDate || oldEndDate.Date != budgetItem.AllocationEndDate)
                    {
                        // SPListItem itg = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ITGovernance, 1);
                        StringBuilder historyDescription = new StringBuilder();
                        historyDescription.Append("Budget Item Updated<br/>");
                        if (oldTitle != budgetItem.BudgetItem)
                        {
                            historyDescription.AppendFormat("Budget Item({0}=>{1})<br/>", oldTitle, budgetItem.BudgetItem);
                        }
                        if (oldAmount != budgetItem.BudgetAmount)
                        {
                            historyDescription.AppendFormat("Budget Amount({0}=>{1})<br/>", oldAmount, budgetItem.BudgetAmount);
                        }
                        if (oldGLCode != budgetItem.GLCode)
                        {
                            historyDescription.AppendFormat("GL Code({0}=>{1})<br/>", oldGLCode, budgetItem.GLCode);
                        }
                        if (oldStartDate.Date != budgetItem.AllocationStartDate)
                        {
                            historyDescription.AppendFormat("Budget Item({0}=>{1})<br/>", oldStartDate.ToString("MMM-d-yyyy"), budgetItem.AllocationStartDate.ToString("MMM-d-yyyy"));
                        }
                        if (oldEndDate.Date != budgetItem.AllocationEndDate)
                        {
                            historyDescription.AppendFormat("Budget Item({0}=>{1})", oldEndDate.ToString("MMM-d-yyyy"), budgetItem.AllocationEndDate.ToString("MMM-d-yyyy"));
                        }
                        //uHelper.CreateHistory(AppContext.CurrentUser, historyDescription.ToString(), itg, AppContext);
                    }


                    BindBudgetList();
                    budgetPopupBox.ShowOnPageLoad = false;
                    budgetMessage.Text = "Budget Item Updated Successfully.";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;
                    isUpdated = true;
                }
                else
                {
                    budgetMessage.Text = "Error Saving Budget";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            return isUpdated;
        }
        private void ClearBudgetForm()
        {
            txtBudgetItemVal.Text = "";
            txtBudgetAmountf.Text = "";
            lbBudgetGLCode.Text = "";
            ddlBudgetCategories.SelectedIndex = 0;
            //dtcBudgetStartDate.date;
            //dtcBudgetEndDate.ClearSelection();
        }
        private bool ValidateBudgetForm()
        {
            bool isError = false;
            StringBuilder messages = new StringBuilder();
            messages.Append("<ol style='color:red;'>");
            DropDownList level1 = ddlBudgetCategories;
            TextBox txtBudgetItem = txtBudgetItemVal;
            TextBox txtBudgetAmount = txtBudgetAmountf;
            ASPxDateEdit startDate = dtcBudgetStartDate;
            ASPxDateEdit endDate = dtcBudgetEndDate;
            if (txtBudgetItem.Text.Trim() == string.Empty)
            {
                isError = true;
                messages.Append("<li>Please enter budget item name</li>");
            }
            if (level1.SelectedItem == null || level1.SelectedItem.Text.Trim() == string.Empty || level1.SelectedItem.Text.Trim() == "--Select--")
            {
                isError = true;
                messages.Append("<li>Please select a budget category</li>");
            }

            if (string.IsNullOrEmpty(departmentCtrBudget.GetValues()))
            {
                isError = true;
                messages.Append("<li>Please select a Department</li>");
            }
            if (txtBudgetAmount.Text.Trim() != string.Empty)
            {
                try
                {
                    double.Parse(txtBudgetAmount.Text.Trim());
                }
                catch
                {
                    isError = true;
                    messages.AppendFormat("<li>Amount entered  \"{0}\" is incorrect format, please use format \"12345.99\"</li>", txtBudgetAmount.Text.Trim());
                }
            }
            else
            {
                isError = true;
                messages.Append("<li>Please enter budget amount</li>");
            }

            if (startDate.Date.CompareTo(endDate.Date) > 0)
            {
                isError = true;
                messages.Append("<li>Start date cannot be greater than end date</li>");
            }
            messages.Append("</ol>");
            if (isError)
            {
                budgetMessage.Text = messages.ToString();

                string selectedCategory = level1.SelectedValue;
                level1.Items.Clear();
                FillDropDownLevel1(level1, new EventArgs());
                level1.SelectedIndex = level1.Items.IndexOf(level1.Items.FindByValue(selectedCategory));

                //List<int> selectedDepts = departmentCtrBudget.GetSelectedDepartmentIDs();
                //string selectedDepartment = string.Empty;
                //if (selectedDepts.Count > 0)get
                //    selectedDepartment = Convert.ToString(selectedDepts[0]);

                isBindBudgetDone = true;
            }

            return !isError;
        }
        #endregion

        #region Project Budget Actuals
        /// <summary>
        /// Bind Budget list
        /// </summary>

        private DataTable GetBudgetActualList()
        {
            int[] selectedCategoies = GetSelectedSubCategories();
            //List<string> lstSelectedCategories = selectedCategoies.Select(x => "ModuleBudgetLookup=" + x.ToString()).ToList();
            //string query = UGITUtility.ConvertListToString(lstSelectedCategories, " Or ");
            string query = string.Join(",", selectedCategoies);


            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", AppContext.TenantID);
            values.Add("@ModuleNameLookup", "ITG");
            values.Add("@BudgetCategoryLookup", query);

            DataTable table = GetTableDataManager.GetData(DatabaseObjects.Tables.ModuleBudgetActuals, values);//BudgetActualsManagerObj.GetDataTable(query); // ITGAMoctual.LoadBySubCategory(selectedCategoies);

            //Gets unallocationed  actuals
            DataRow[] unallocatedActuals = table.AsEnumerable().Where(ac => ac.Field<Int64>("ModuleBudgetLookup") != 0).ToArray();

            //BudgetSubCategoryID
            if (itgBudgetTable == null)
            {
                BindBudgetList();
            }
            DataRow[] selectedItems = new DataRow[0];
            if (itgBudgetTable != null && itgBudgetTable.Rows.Count > 0 && table != null && table.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(itgBudgetTable.Rows[0][DatabaseObjects.Columns.Id])))
                {
                    selectedItems = (from ac in itgBudgetTable.AsEnumerable()
                                     join b in table.AsEnumerable() on UGITUtility.StringToLong(ac.Field<Int64>(DatabaseObjects.Columns.ID)) equals
                                     UGITUtility.StringToLong(b.Field<Int64>("ModuleBudgetLookup"))
                                     select b).ToArray();
                }

            }


            if (selectedItems.Length > 0)
            {
                table = selectedItems.CopyToDataTable();
            }
            else
            {
                table = table.Clone();
            }

            if (unallocatedActuals.Length > 0)
            {
                //table.Merge(unallocatedActuals.CopyToDataTable());
            }

            return table;
        }
        protected void DDLBudgetItems_Load(object sender, EventArgs e)
        {
            BulletedList ddlBudgets = (BulletedList)sender;
            string selectedVal = ddlBudgets.SelectedValue;
            ddlBudgets.Items.Clear();

            int[] selectedDepartments = GetSelectedDepartments();
            int departmentID = 0;
            if (selectedDepartments.Length > 0)
            {
                departmentID = selectedDepartments[0];
            }

            DataTable budgets = null;
            if (departmentID > 0)
            {
                //DepartmentHelper dHelper = new DepartmentHelper(SPContext.Current.Web);
                //Department dept = dHelper.Load(departmentID);
                //string company = null;
                //if (dept.CompanyLookup != null)
                //    company = dept.CompanyLookup.Value;

                budgets = BudgetManagerObj.GetDataTable($"DepartmentLookup = {departmentID}");
            }
            else
            {
                budgets = BudgetManagerObj.GetDataTable();
            }

            //BudgetSubCategoryID
            DataRow[] selectedItems = new DataRow[0];
            if (authorizedCategoriesTable != null && authorizedCategoriesTable.Rows.Count > 0 && budgets != null && budgets.Rows.Count > 0)
            {
                selectedItems = (from ac in authorizedCategoriesTable.AsEnumerable()
                                 join b in budgets.AsEnumerable() on UGITUtility.StringToLong(ac.Field<string>(DatabaseObjects.Columns.ID))
                                 equals UGITUtility.StringToLong(b.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup))
                                 select b).ToArray();
            }

            if (selectedItems.Length > 0)
            {
                selectedItems = selectedItems.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.BudgetItem)).ToArray();
                ListItem lItem = null;
                foreach (DataRow row in selectedItems)
                {
                    lItem = new ListItem(Convert.ToString(row[DatabaseObjects.Columns.BudgetItem]), Convert.ToString(row[DatabaseObjects.Columns.Id]));
                    lItem.Attributes.Add("glcode", Convert.ToString(row[DatabaseObjects.Columns.GLCode]));
                    lItem.Attributes.Add("value", Convert.ToString(row[DatabaseObjects.Columns.ID]));
                    ddlBudgets.Items.Add(lItem);
                }
            }
            ListItem lItem1 = new ListItem("--Select-- ", "");
            ddlBudgets.Items.Insert(0, lItem1);
        }
        protected void DDLVendorActual_Load(object sender, EventArgs e)
        {
            DropDownList vendorDropDown = (DropDownList)sender;
            if (vendorDropDown.Items.Count <= 0)
            {
                AssetVendorViewManager assetVentorManager = new AssetVendorViewManager(HttpContext.Current.GetManagerContext());
                DataTable vendors = assetVentorManager.GetDataTable(); // uGITCache.GetDataTable(DatabaseObjects.Lists.AssetVendors);
                if (vendors != null)
                {
                    DataRow[] rows = vendors.Select("", string.Format("{0} asc", DatabaseObjects.Columns.Title));
                    foreach (DataRow vendor in rows)
                    {
                        vendorDropDown.Items.Add(new ListItem(Convert.ToString(UGITUtility.GetSPItemValue(vendor, DatabaseObjects.Columns.Title)), Convert.ToString(UGITUtility.GetSPItemValue(vendor, DatabaseObjects.Columns.Id))));
                    }
                }
                vendorDropDown.Items.Insert(0, new ListItem("--None--", ""));
            }
        }
        protected void BtDeleteActual_Click(object sender, EventArgs e)
        {
            long budgetActualItemID = 0;
            long.TryParse(hfEditBudgetActualID.Value, out budgetActualItemID);
            if (budgetActualItemID > 0)
            {
                BudgetActualsManager budgetManager = new BudgetActualsManager(HttpContext.Current.GetManagerContext());
                BudgetActual budgetActual = budgetManager.LoadByID(budgetActualItemID);
                string historyDescription = string.Format("Deleted budget Actual: {0}(Invoice:{1})", budgetActual.Title, budgetActual.InvoiceNumber);

                if (budgetManager.Delete(budgetActual))
                {
                    //////create history
                    //DataRow actualrow = UGITUtility.ObjectToData(budgetActual).Select()[0];
                    //uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyDescription, itg);

                    budgetMessage.Text = "Budget Actual deleted";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;

                    // Update Monthly distribution
                    ModuleBudget moduleBudget = BudgetManagerObj.LoadByID(budgetActual.ModuleBudgetLookup);
                    if (moduleBudget != null && moduleBudget.BudgetCategoryLookup > 0)
                    {
                        List<ModuleMonthlyBudget> lstmonthlybudgetlist = BudgetMonthlyManagerObj.Load(x => x.BudgetCategoryLookup == moduleBudget.BudgetCategoryLookup);
                        if (lstmonthlybudgetlist != null && lstmonthlybudgetlist.Count > 0)
                        {
                            UpdateITGMonthlyBudget(-1, moduleBudget.BudgetCategoryLookup, budgetActual.ModuleBudgetLookup);
                        }
                    }
                    //SPList itgBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGBudget);
                    //SPQuery bQuery = new SPQuery();
                    //bQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Number'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, budgetActual.ITGBudgetItem.LookupId);
                    //SPListItemCollection budgetItemCollection = itgBudgetList.GetItems(bQuery);

                    //if (budgetItemCollection.Count > 0)
                    //{
                    //    int subCategoryId = Convert.ToInt32(UGITUtility.SplitString(budgetItemCollection[0][DatabaseObjects.Columns.BudgetLookup], ";#", 0));
                    //    UpdateITGMonthlyBudget(-1, subCategoryId);
                    //}
                }
                else
                {
                    budgetMessage.Text = "Error in deleting";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

        }
        protected void FillDropDopwnLevel1ForActual(object sender, EventArgs e)
        {
            DropDownList ddlLevel1 = (DropDownList)sender;
            if (ddlLevel1.Items.Count <= 0)
            {
                BindLevel1CategoryForActual(ddlLevel1);
            }
        }
        protected void BtBudgetActualSave_Click(object sender, EventArgs e)
        {
            int budgetID = 0;
            int.TryParse(hfEditBudgetActualID.Value, out budgetID);

            int budgetActionID = 0;
            int.TryParse(hfEditActualID.Value, out budgetActionID);

            BudgetActual actualItem = null;
            if (budgetActionID > 0)
            {
                actualItem = BudgetActualsManagerObj.LoadByID(budgetActionID);
            }

            if (actualItem != null)
            {
                if (UpdateActualAllocation(actualItem))
                {
                    ////Update Monthly distribution if actual passed validation and was updated successfully
                    budgetMessage.Text = "Budget Actual Updated!";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;

                    // Update Monthly distribution
                    ModuleBudget moduleBudget = BudgetManagerObj.LoadByID(actualItem.ModuleBudgetLookup);
                    if (moduleBudget != null && moduleBudget.BudgetCategoryLookup > 0)
                    {
                        List<ModuleMonthlyBudget> lstmonthlybudgetlist = BudgetMonthlyManagerObj.Load(x => x.BudgetCategoryLookup == moduleBudget.BudgetCategoryLookup);
                        if (lstmonthlybudgetlist != null && lstmonthlybudgetlist.Count > 0)
                        {
                            UpdateITGMonthlyBudget(-1, moduleBudget.BudgetCategoryLookup, actualItem.ModuleBudgetLookup);
                        }
                    }
                    //SPList itgBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGBudget);
                    //SPQuery bQuery = new SPQuery();
                    //bQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Number'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, actualItem.ITGBudgetItem.LookupId);
                    //SPListItemCollection budgetItemCollection = itgBudgetList.GetItems(bQuery);

                    //if (budgetItemCollection.Count > 0)
                    //{
                    //    int oldCategoryId = Convert.ToInt32(UGITUtility.SplitString(budgetItemCollection[0][DatabaseObjects.Columns.BudgetLookup], ";#", 0));
                    //    UpdateITGMonthlyBudget(oldCategoryId, Convert.ToInt32(ddlActualCategory.SelectedValue));
                    //}
                }
            }
            else
            {
                NewActualAllocation();
            }
        }
        private bool ValidateBudgetActualForm(bool validateall = false)
        {
            bool isError = false;
            StringBuilder messages = new StringBuilder();
            messages.Append("<ol style='color:red;'>");
            BulletedList ddlBudgetItem = ddlActualBudget;
            TextBox txtTitle = txtActualTitle;
            TextBox txtBudgetAmount = txtActualAmount;
            TextBox txtBudgetInvoiceNo = txtActualInvoice;
            ASPxDateEdit startDate = dtcActualStartDate;
            ASPxDateEdit endDate = dtcActualEndDate;

            if (validateall && (ddlActualCategory.SelectedItem == null || ddlActualCategory.SelectedItem.Text.Trim() == string.Empty || ddlActualCategory.SelectedItem.Text.Trim() == "--Select--"))
            {
                isError = true;
                messages.Append("<li>Please select a budget category</li>");
            }
            if (hfEditBudgetActualID.Value == "" || hfEditBudgetActualID.Value == "0")
            {
                isError = true;
                messages.Append("<li>Please select a budget item</li>");
            }

            if (txtTitle.Text.Trim() == string.Empty)
            {
                isError = true;
                messages.Append("<li>Please enter title</li>");
            }

            if (txtBudgetAmount.Text.Trim() != string.Empty)
            {
                try
                {
                    double.Parse(txtBudgetAmount.Text.Trim());
                }
                catch
                {
                    isError = true;
                    messages.AppendFormat("<li>Amount entered  \"{0}\" is incorrect format, please use format \"12345.99\"</li>", txtBudgetAmount.Text.Trim());
                }
            }
            else
            {
                isError = true;
                messages.Append("<li>Please enter budget amount</li>");
            }

            if (startDate.Date.CompareTo(endDate.Date) > 0)
            {
                isError = true;
                messages.Append("<li>Start date cannot be greater than end date</li>");
            }
            messages.Append("</ol>");
            if (isError)
            {

                budgetMessage.Text = messages.ToString();

                ddlActualCategory.Items.Clear();
                FillDropDownLevel1(ddlActualCategory, new EventArgs());

                ddlActualBudget.Items.Clear();
                DDLBudgetItems_Load(ddlActualBudget, new EventArgs());

                ddlActualVender.Items.Clear();
                DDLVendorActual_Load(ddlActualVender, new EventArgs());

            }

            return !isError;
        }
        private void NewActualAllocation()
        {
            TextBox txtTitle = txtActualTitle;
            TextBox txtInvoiceNoActual = txtActualInvoice;
            ASPxDateEdit startDate = dtcActualStartDate;
            ASPxDateEdit endDate = dtcActualEndDate;
            DropDownList ddlVendors = ddlActualVender;


            long categoryId = 0;
            long.TryParse(ddlActualCategory.SelectedValue, out categoryId);

            BudgetActual budgetActual = new BudgetActual();
            ModuleBudget budgetItemObj = null;
            if (ValidateBudgetActualForm())
            {
                long igtBugetItemID = 0;
                long.TryParse(hfEditBudgetActualID.Value, out igtBugetItemID);
                if (igtBugetItemID > 0)
                {
                    budgetActual.ModuleBudgetLookup = igtBugetItemID;
                    budgetItemObj = BudgetManagerObj.LoadByID(igtBugetItemID);
                }
                budgetActual.Title = txtTitle.Text.Trim();
                budgetActual.InvoiceNumber = txtInvoiceNoActual.Text.Trim();
                budgetActual.ModuleName = ModuleNames.ITG;
                budgetActual.AllocationStartDate = startDate.Date;
                if (dtcActualStartDate.Date == DateTime.MinValue)
                    budgetActual.AllocationStartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

                budgetActual.AllocationEndDate = endDate.Date;
                if (dtcActualStartDate.Date == DateTime.MinValue)
                    budgetActual.AllocationEndDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));

                budgetActual.BudgetAmount = double.Parse(txtActualAmount.Text.Trim());
                budgetActual.BudgetItem = UGITUtility.ObjectToString(budgetItemObj?.BudgetItem);

                long vendorID = 0;
                long.TryParse(ddlVendors.SelectedValue, out vendorID);
                if (vendorID > 0)
                {
                    budgetActual.VendorLookup = vendorID;
                }
                else
                {
                    budgetActual.VendorLookup = null;
                }

                long departmentID = 0;
                long.TryParse(departmentCtrActual.GetValues(), out departmentID);
                if (departmentID > 0)
                    budgetActual.DepartmentLookup = departmentID;
                else
                    budgetActual.DepartmentLookup = null;

                if (BudgetActualsManagerObj.Insert(budgetActual) > 0)
                {

                    UpdateITGMonthlyBudget(-1, categoryId, budgetActual.ModuleBudgetLookup);
                    budgetMessage.Text = "Budget Inserted Successfully.";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;

                    // Upate ITG Monthly budget in case a new category added in ITG actual.

                    BindBudgetList();

                    budgetActualPopupBox.ShowOnPageLoad = false;
                }
                else
                {
                    BindBudgetList();
                    budgetMessage.Text = "Error creating Budget";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        private bool UpdateActualAllocation(BudgetActual budgetActual)
        {
            bool isUpdated = false;

            TextBox txtTitle = txtActualTitle;
            TextBox txtInvoiceNoActual = txtActualInvoice;
            ASPxDateEdit startDate = dtcActualStartDate;
            ASPxDateEdit endDate = dtcActualEndDate;
            DropDownList ddlVendors = ddlActualVender;

            if (ValidateBudgetActualForm())
            {
                try
                {
                    string oldTitle = budgetActual.Title;
                    string oldBudget = string.Empty;
                    long oldBudgetID = 0;
                    if (budgetActual.ModuleBudgetLookup > 0)
                    {
                        oldBudgetID = budgetActual.ModuleBudgetLookup;
                    }

                    long? oldVenderID = 0;
                    if (budgetActual.VendorLookup > 0)
                    {
                        oldVenderID = budgetActual.VendorLookup;
                    }

                    DateTime oldStartDate = budgetActual.AllocationStartDate;
                    DateTime oldEndDate = budgetActual.AllocationEndDate;
                    string oldInvoice = budgetActual.InvoiceNumber;
                    double oldActualAmount = budgetActual.BudgetAmount;


                    //        int igtBugetItemID = 0;
                    //        int.TryParse(hfEditBudgetActualID.Value, out igtBugetItemID);
                    //        if (igtBugetItemID > 0)
                    //        {
                    //            //budgetActual.ITGBudgetItem = new SPFieldLookupValue();
                    //            budgetActual.ITGBudgetItem.LookupId = igtBugetItemID;
                    //        }
                    //        else
                    //        {
                    //            budgetActual.ITGBudgetItem = null;
                    //        }

                    long vendorID = 0;
                    long.TryParse(ddlVendors.SelectedValue, out vendorID);
                    budgetActual.VendorLookup = vendorID;

                    budgetActual.Title = txtTitle.Text.Trim();
                    budgetActual.BudgetDescription = txtInvoiceNoActual.Text.Trim();

                    budgetActual.AllocationStartDate = startDate.Date;
                    budgetActual.AllocationEndDate = endDate.Date;
                    budgetActual.BudgetAmount = double.Parse(txtActualAmount.Text.Trim());
                    budgetActual.InvoiceNumber = txtInvoiceNoActual.Text.Trim();


                    if (BudgetActualsManagerObj.Update(budgetActual))
                    {

                        ////Create history
                        //if (oldTitle != budgetActual.Title || oldActualAmount != budgetActual.ActualCost || (budgetActual.ITGBudgetItem != null && oldBudgetID != budgetActual.ITGBudgetItem.LookupId) ||
                        //    oldInvoice != budgetActual.InvoiceNumber || oldStartDate.Date != budgetActual.StartDate.Date || oldEndDate.Date != budgetActual.EndDate.Date ||
                        //    oldVenderID != vendorID)
                        //{
                        //    //SPListItem itg = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ITGovernance, 1);
                        //    StringBuilder historyDescription = new StringBuilder();
                        //    historyDescription.Append("Budget Actual Updated<br/>");
                        //    if (oldTitle != budgetActual.Title)
                        //    {
                        //        historyDescription.AppendFormat("Title({0}=>{1})<br/>", oldTitle, budgetActual.Title);
                        //    }

                        //    if (budgetActual.ITGBudgetItem != null && oldBudgetID != budgetActual.ITGBudgetItem.LookupId)
                        //    {
                        //        historyDescription.AppendFormat("Budget Item({0}=>{1})<br/>", oldBudget, budgetActual.ITGBudgetItem.LookupValue);
                        //    }

                        //    if (oldVenderID != vendorID)
                        //    {
                        //        if (budgetActual.Vendor != null)
                        //        {
                        //            historyDescription.AppendFormat("Vendor({0}=>{1})<br/>", oldVender, budgetActual.Vendor.LookupValue);
                        //        }
                        //        else
                        //        {
                        //            historyDescription.AppendFormat("Vendor({0}=>\"\")<br/>", oldVender);
                        //        }
                        //    }

                        //    if (oldActualAmount != budgetActual.ActualCost)
                        //    {
                        //        historyDescription.AppendFormat("Actual Amount({0}=>{1})<br/>", oldActualAmount, budgetActual.ActualCost);
                        //    }

                        //    if (oldInvoice != budgetActual.InvoiceNumber)
                        //    {
                        //        historyDescription.AppendFormat("Invoice No.({0}=>{1})<br/>", oldInvoice, budgetActual.InvoiceNumber);
                        //    }
                        //    if (oldStartDate.Date != budgetActual.StartDate.Date)
                        //    {
                        //        historyDescription.AppendFormat("Budget Item({0}=>{1})<br/>", oldStartDate.ToString("MMM-d-yyyy"), budgetActual.StartDate.ToString("MMM-d-yyyy"));
                        //    }
                        //    if (oldEndDate.Date != budgetActual.EndDate.Date)
                        //    {
                        //        historyDescription.AppendFormat("Budget Item({0}=>{1})", oldEndDate.ToString("MMM-d-yyyy"), budgetActual.EndDate.ToString("MMM-d-yyyy"));
                        //    }
                        //    uHelper.CreateHistory(AppContext.CurrentUser, historyDescription.ToString(), itg, AppContext);
                        //}
                        budgetMessage.Text = "Actual Updated Successfully.";
                        budgetMessage.ForeColor = System.Drawing.Color.Blue;
                        isUpdated = true;

                        budgetActualPopupBox.ShowOnPageLoad = false;
                    }
                    else
                    {
                        budgetMessage.Text = "Error creating Budget";
                        budgetMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch
                {
                    budgetMessage.Text = "Error in saving";
                    budgetMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            return isUpdated;
        }
        private void BindLevel1CategoryForActual(DropDownList ddlLevel1)
        {
            ddlLevel1.Items.Clear();

            DataTable collection = BudgetCategoryManagerObj.GetDataTable();

            if (collection != null && collection.Rows.Count > 0)
            {
                DataTable actualsCategoryTable = collection.Clone();

                DataTable budgetTable = GetBudgetData();
                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(budgetTable.Rows[0][DatabaseObjects.Columns.ID])))
                    return;
                var actualsCategoryJoin = (from category in collection.AsEnumerable()
                                           join budget in budgetTable.AsEnumerable()
                                           on category.Field<string>(DatabaseObjects.Columns.ID) equals UGITUtility.ObjectToString(budget.Field<Int64>(DatabaseObjects.Columns.BudgetCategoryLookup))
                                           select new
                                           {
                                               BudgetCategory = category.Field<string>(DatabaseObjects.Columns.BudgetCategoryName),
                                               BudgetSubCategory = category.Field<string>(DatabaseObjects.Columns.BudgetSubCategory)
                                           }).ToArray();
                if (actualsCategoryJoin.Count() <= 0)
                    return;
                DataTable actualsCategory = UGITUtility.LINQResultToDataTable(actualsCategoryJoin);

                //Get the distinct category and subcategory from budget table.
                DataTable budgetActulCategoryTable = actualsCategory.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory, DatabaseObjects.Columns.BudgetSubCategory);

                if (budgetActulCategoryTable != null)
                {
                    // Get the Category-SubCategory Row from categories table and import the hole row in a new table so that we can pass that table in the function to build the hiearchical drop down.
                    foreach (DataRow budgetRow in budgetActulCategoryTable.Rows)
                    {
                        DataRow[] categoryRows = collection.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.BudgetCategoryName, budgetRow[DatabaseObjects.Columns.BudgetCategory], DatabaseObjects.Columns.BudgetSubCategory, budgetRow[DatabaseObjects.Columns.BudgetSubCategory]));

                        if (categoryRows.Length > 0)
                            actualsCategoryTable.ImportRow(categoryRows[0]);
                    }

                    if (actualsCategoryTable.Rows.Count > 0)
                    {
                        List<ListItem> items = BudgetCategoryManagerObj.LoadCategoriesDropDownItems(actualsCategoryTable);
                        foreach (ListItem item in items)
                        {
                            ddlLevel1.Items.Add(item);
                        }
                        ListItem itemSelect = new ListItem("--Select--", "");
                        ddlLevel1.Items.Insert(0, itemSelect);
                    }
                }
            }
        }

        private void UpdateITGMonthlyBudget(long oldSubCategoryID, long newSubCategoryID, long budgetLookup)
        {
            // Case: when category is changed then update the old category distribution.
            if (oldSubCategoryID != -1 && oldSubCategoryID != newSubCategoryID)
            {
                UpdateMonthlyDistribution(oldSubCategoryID, budgetLookup);
            }
            else
            {
                // Case: Add/Update the new category distribution.
                UpdateMonthlyDistribution(newSubCategoryID, budgetLookup);
            }
        }
        private void UpdateMonthlyDistribution(long subCategoryID, long budgetLookup)
        {
            //// Open all three related list to get budget item actuals.
            //SPList itgBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGBudget);
            //SPList itgActualList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGActual);
            //SPList monthlyBudgetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ITGMonthlyBudget);

            //Get all budget items of old sub-category.
            //SPQuery bQuery = new SPQuery();
            //bQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, subCategoryID);
            //DataTable budgetTable = itgBudgetList.GetItems(bQuery).GetDataTable();
            DataTable budgetTable = BudgetManagerObj.GetDataTable($"{DatabaseObjects.Columns.BudgetCategoryLookup}='{subCategoryID}'");

            //Get previous monthly distribution of subcategory and set it to 0.
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.BudgetLookup, subCategoryID);
            List<ModuleMonthlyBudget> oldBudgetDistributions = BudgetMonthlyManagerObj.Load(x => x.BudgetCategoryLookup == subCategoryID && x.ModuleName == ModuleNames.ITG);

            foreach (ModuleMonthlyBudget oldItem in oldBudgetDistributions)
            {
                oldItem.NonProjectActualTotal = 0;
                BudgetMonthlyManagerObj.Update(oldItem);
            }

            // Get the data table object of old distribution collection.
            //DataTable oldBudgetDistributionsTable = oldBudgetDistributions.GetDataTable();

            //distribute budget amount for old sub category
            if (oldBudgetDistributions != null && oldBudgetDistributions.Count > 0)
            {
                foreach (ModuleMonthlyBudget budgetRow in oldBudgetDistributions)
                {
                    ////Get budgets of old sub category
                    //SPQuery aQuery = new SPQuery();
                    //aQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ITGBudgetLookup, budgetRow[DatabaseObjects.Columns.Id]);
                    DataTable actualTable = BudgetActualsManagerObj.GetDataTable($"{DatabaseObjects.Columns.ModuleBudgetLookup}={budgetLookup} And {DatabaseObjects.Columns.ModuleNameLookup}='ITG'");

                    //distribute budget amount for old sub category
                    if (actualTable != null && actualTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in actualTable.Rows)
                        {
                            // Get the start date, end date and amount to distribute within start and end date.
                            DateTime startDate = (DateTime)row[DatabaseObjects.Columns.AllocationStartDate];
                            DateTime endDate = (DateTime)row[DatabaseObjects.Columns.AllocationEndDate];
                            double totalAmount = Convert.ToDouble(Convert.ToString(row[DatabaseObjects.Columns.BudgetAmount]));

                            // Distribute the amount within specified dates and get the result in month and amount format.
                            Dictionary<DateTime, double> amountDistributions = uHelper.DistributeAmount(AppContext, startDate, endDate, totalAmount);

                            // Check the month entry is already exist in MonthlyDistribution table, if it is then update else add it.
                            foreach (DateTime key in amountDistributions.Keys)
                            {
                                double val = amountDistributions[key];
                                ModuleMonthlyBudget preItem = null;
                                if (oldBudgetDistributions != null && oldBudgetDistributions.Count > 0)
                                {
                                    preItem = oldBudgetDistributions.FirstOrDefault(x => x.AllocationStartDate == key.Date && x.AllocationStartDate == key.Date);
                                }

                                ModuleMonthlyBudget item = null;
                                double actualTotal = 0.0;
                                if (preItem != null)
                                {
                                    item = BudgetMonthlyManagerObj.LoadByID(UGITUtility.StringToLong(preItem.ID)); // SPListHelper.GetSPListItem(monthlyBudgetList, (int)preItem[DatabaseObjects.Columns.Id]);
                                    item.BudgetCategoryLookup = subCategoryID;
                                    double.TryParse(Convert.ToString(item.NonProjectActualTotal), out actualTotal);
                                    item.NonProjectActualTotal = val;
                                    BudgetMonthlyManagerObj.Update(item);
                                }
                                else
                                {
                                    item = new ModuleMonthlyBudget();
                                    item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                    item.EstimatedCost = 0;
                                    item.ResourceCost = 0;
                                    item.ActualCost = 0;
                                    item.BudgetType = "0";
                                    item.BudgetCategoryLookup = subCategoryID;
                                    item.NonProjectActualTotal = val;
                                    item.NonProjectPlanedTotal = 0;
                                    item.ProjectActualTotal = 0;
                                    item.ProjectPlanedTotal = 0;
                                    item.ModuleName = ModuleNames.ITG;
                                    BudgetMonthlyManagerObj.Update(item);
                                }
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region Utility
        public int[] GetSelectedSubCategories()
        {
            string selectedSubCategory = hfSelectedCategories.Text.Trim();
            List<int> selectedSubCategoryIDs = new List<int>();
            if (!string.IsNullOrEmpty(selectedSubCategory) && selectedSubCategory.Trim() != string.Empty)
            {
                List<string> subCategories = UGITUtility.ConvertStringToList(selectedSubCategory.Trim(), new string[] { "," });
                foreach (string subCategory in subCategories)
                {
                    int id = 0;
                    int.TryParse(subCategory, out id);
                    if (id > 0)
                    {
                        selectedSubCategoryIDs.Add(id);
                    }
                }
            }
            return selectedSubCategoryIDs.ToArray();
        }

        public int[] GetSelectedDepartments()
        {
            string selectedDepartments = hfSelectedDepartments.Text.Trim();
            List<int> selectedSubCategoryIDs = new List<int>();
            if (!string.IsNullOrEmpty(selectedDepartments) && selectedDepartments.Trim() != string.Empty)
            {
                List<string> subCategories = UGITUtility.ConvertStringToList(selectedDepartments.Trim(), new string[] { "," });
                foreach (string subCategory in subCategories)
                {
                    int id = 0;
                    int.TryParse(subCategory, out id);
                    if (id > 0)
                    {
                        selectedSubCategoryIDs.Add(id);
                    }
                }
            }
            return selectedSubCategoryIDs.ToArray();
        }

        private void LoadReportOption()
        {
            //Label l1 = new Label();
            //l1.Text = "CIO Report";
            //l1.Attributes.Add("onclick", "javascript:showCIOReport(this);return false;");
            //l1.CssClass = "ugitlinkbg reportitem";
            //l1.Attributes.Add("onmouseover", "reportItemMouseOver(this)");
            //l1.Attributes.Add("onmouseout", "reportItemMouseOut(this)");
            //pnlReportList.Controls.Add(l1);

            Label l3 = new Label();
            l3.Text = "Budget Report";
            l3.Attributes.Add("onclick", "javascript:OpenBudgetReportPopup();return false;");
            l3.CssClass = "ugitlinkbg reportitem";
            l3.Attributes.Add("onmouseover", "reportItemMouseOver(this)");
            l3.Attributes.Add("onmouseout", "reportItemMouseOut(this)");
            pnlReportList.Controls.Add(l3);

            Label l4 = new Label();
            l4.Text = "Actuals Report";
            l4.Attributes.Add("onclick", "javascript:OpenActualsReportPopup();return false;");
            l4.CssClass = "ugitlinkbg reportitem";
            l4.Attributes.Add("onmouseover", "reportItemMouseOver(this)");
            l4.Attributes.Add("onmouseout", "reportItemMouseOut(this)");
            pnlReportList.Controls.Add(l4);
        }

        #endregion


        protected void previousYrs_Click(object sender, ImageClickEventArgs e)
        {
            lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) - 1);
            if (hdnCurrentYear.Value != lblSelectedYear.Text)
                hdnCurrentYear.Value = lblSelectedYear.Text;
        }
        protected void nextYrs_Click(object sender, ImageClickEventArgs e)
        {
            lblSelectedYear.Text = Convert.ToString(Convert.ToInt32(lblSelectedYear.Text) + 1);
            if (hdnCurrentYear.Value != lblSelectedYear.Text)
                hdnCurrentYear.Value = lblSelectedYear.Text;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdnCurrentYear.Value != lblSelectedYear.Text)
                hdnCurrentYear.Value = lblSelectedYear.Text;
        }
    }
}
