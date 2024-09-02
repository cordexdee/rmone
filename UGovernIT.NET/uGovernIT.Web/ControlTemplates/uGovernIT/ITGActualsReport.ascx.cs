using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web
{
    public partial class ITGActualsReport : System.Web.UI.UserControl
    {
        public int PMMId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Category { get; set; }

        private DataTable budgetTable = new DataTable();
        private DataTable subCategoryTable = null;

        private double grandTotalActual = 0.0;

        private double subCategoryTotal = 0.0;
        private double categoryTotal = 0.0;
        private double budgetTypeTotal = 0.0;
        private double itemTotal = 0.0;

        bool enableBudgetCategoryType = false;
        DataTable authorizedCategories = null;

        private string reportURL = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();
        private List<BudgetCategory> categoryList = null;
        protected bool printReport = false;
        ApplicationContext context = null;
        BudgetCategoryViewManager objmgr;
        ConfigurationVariableManager objconfigvar;
        BudgetActualsManager objBudgetActualsmgr;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objmgr = new BudgetCategoryViewManager(context);
            objBudgetActualsmgr = new BudgetActualsManager(context);
            objconfigvar = new ConfigurationVariableManager(context);
            //Load category list.
            //categoryList = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);
            categoryList =  objmgr.Load();
            //Load all authorized Category and SubCategories.
            //authorizedCategories = BudgetCategory.LoadCategoties(SPContext.Current.Web.CurrentUser.ID, 1);
            authorizedCategories = objmgr.LoadCategories();
            // Get the configuration from config whether we have to display Budget Type in report or not.
            enableBudgetCategoryType = UGITUtility.StringToBoolean(objconfigvar.GetValue(ConfigurationVariable.EnableBudgetCategoryType));

            if (!IsPostBack)
            {
                // If all the parameters are coming in url then generate the report.
                if (Request["ExportReport"] != null)
                {
                    if (Request["StartDate"] != null)
                    {
                        StartDate = Convert.ToDateTime(Request["StartDate"]);
                    }
                    if (Request["EndDate"] != null)
                    {
                        EndDate = Convert.ToDateTime(Request["EndDate"]);
                    }

                    if (Request["Cat"] != null)
                    {
                        string[] categories = Request["Cat"].Split(',');
                        Category = new List<string>();
                        foreach (string category in categories)
                        {
                            Category.Add(category);
                        }
                    }

                    GenerateReport(StartDate, EndDate, Category);
                    exportPanel.Visible = false;
                    btnClose.Visible = false;
                }
                else
                {
                    BindLevel1Category();
                }
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
            if (string.IsNullOrEmpty(Request.Url.Query))
            {
                urlBuilder.Append("?");
                urlBuilder.Append(reportURL);
            }
            else
            {
                urlBuilder.Append("&");
                urlBuilder.Append(reportURL);
            }
            base.OnPreRender(e);
        }
        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (dtDateFrom.Value != null)
                StartDate = dtDateFrom.Date;
            else
                StartDate = new DateTime(1999, 1, 1);
            reportURL = reportURL + "StartDate=" + StartDate.ToString();

            if (dtDateTo.Value != null)
                EndDate = dtDateTo.Date;
            else
                EndDate = new DateTime(2099, 12, 31);
            reportURL = reportURL + "&EndDate=" + EndDate.ToString();

            // Make a list of selected category.
            List<string> selectedCategory = new List<string>();
            string categories = string.Empty;
            foreach (ListItem categoryItem in chkBoxCategoryList.Items)
            {
                if (categoryItem.Selected)
                {
                    selectedCategory.Add(categoryItem.Value);
                    reportURL = reportURL + "&Cat=" + categoryItem.Value;
                }
            }

            //Getnerate the report according to selected criteria.
            GenerateReport(StartDate, EndDate, selectedCategory);
        }

        private void GenerateReport(DateTime startdate, DateTime enddate, List<string> selectedcategory)
        {
            // Show hide the report components and headings.
            lblHeading.Text = "Non-Project Actuals Report";
            if (dtDateFrom.Value == null && dtDateTo.Value == null)
                lblSubHeading.Text = string.Empty;
            else
                lblSubHeading.Text = "Report from: " + (string.IsNullOrEmpty(dtDateFrom.Value.ToString()) ? "Earliest" : UGITUtility.GetDateStringInFormat(dtDateFrom.Date, false)) +
                                              " to " + (string.IsNullOrEmpty(dtDateTo.Value.ToString()) ? "Latest" : UGITUtility.GetDateStringInFormat(dtDateTo.Date, false));

            PnlBudgetReportPopup.Style.Add("display", "none");
            pnlBudgetComponent.Style.Add("display", "block");

            // Get Budget Data.
            budgetTable = GetCurrentPlannedData();

            // Filter the row by selected category.
            FilterSelectedCategory(selectedcategory);

            // Filter budget table for authorized to view category/subcategory. 
            FilterCategoriesToView();

            // Bind The Budget Type repeater with budget types.
            if (budgetTable != null && budgetTable.Rows.Count > 0)
            {
                DataTable budgetTypeTable = budgetTable.Clone();

                // If configuration enableBudgetCategoryType is false insert a blank row in budgetTypeTable so that we can find and bind the category repeater in the itemDataBound event().
                if (enableBudgetCategoryType)
                    budgetTypeTable = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetType);
                else
                {
                    DataRow emptyRow = budgetTypeTable.NewRow();
                    emptyRow[DatabaseObjects.Columns.BudgetType] = "";
                    emptyRow[DatabaseObjects.Columns.BudgetCategory] = "";

                    budgetTypeTable.Rows.InsertAt(emptyRow, 0);
                    budgetTypeTable.AcceptChanges();
                }

                budgetTypeRepeater.DataSource = budgetTypeTable;
                budgetTypeRepeater.DataBind();
            }
        }
        protected void BudgetTypeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            budgetTypeTotal = 0.0;
            // Select the distince category from budget table.
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater categoryRepeater = (Repeater)item.FindControl("categoryRepeater");
                DataTable categoryTable = null;

                Label lblBudgetType = (Label)e.Item.FindControl("lblBudgetType");
                Label lblBudgetTypeTotal = (Label)e.Item.FindControl("lblBudgetTypeTotal");

                // If configuration enableBudgetCategoryType is false bind all the category at once else bind the categories according to the BudgetType.
                if (!enableBudgetCategoryType)
                {
                    categoryTable = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                    HtmlImage img = (HtmlImage)e.Item.FindControl("collapseImage");
                    img.Visible = false;
                }
                else
                {
                    DataRow[] categoryRows = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetType, lblBudgetType.Text));

                    if (categoryRows.Length > 0)
                    {
                        categoryTable = categoryRows.CopyToDataTable();
                        categoryTable = categoryTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                    }
                }

                if (categoryTable != null)
                {
                    categoryTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetCategory;
                    categoryRepeater.DataSource = categoryTable.DefaultView.Table;
                    categoryRepeater.DataBind();

                    //  Show Butget type total only when Budget Type is enabled.
                    if (enableBudgetCategoryType)
                    {
                        lblBudgetTypeTotal.Text = string.Format("{0:C}", Convert.ToDouble(budgetTypeTotal));
                    }
                }

                // Hide the Category if category total is 0.
                if (budgetTypeTotal <= 0)
                {
                    e.Item.Visible = false;
                }
            }

            if (item.ItemType == ListItemType.Footer)
            {
                Label lblGTotalActual = (Label)e.Item.FindControl("lblGTotalActual");
                lblGTotalActual.Text = string.Format("{0:C}", Convert.ToDouble(grandTotalActual));
            }
        }

        protected void CategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            categoryTotal = 0.0;
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater subCategoryRepeater = (Repeater)item.FindControl("subCategoryRepeater");
                Label lblCategory = (Label)e.Item.FindControl("lblCategory");
                Label lblCategoryTotal = (Label)e.Item.FindControl("lblCategoryTotal");

                subCategoryTable = GetSubCategory(lblCategory.Text);

                if (subCategoryTable != null && subCategoryTable.Rows.Count > 0)
                {
                    subCategoryTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetSubCategory;
                    subCategoryRepeater.DataSource = subCategoryTable.DefaultView.Table;
                    subCategoryRepeater.DataBind();
                    lblCategoryTotal.Text = string.Format("{0:C}", Convert.ToDouble(categoryTotal));
                    budgetTypeTotal += categoryTotal;

                    //calculate the grand total.
                    grandTotalActual += categoryTotal;
                }

                if (categoryTotal <= 0)
                {
                    // Hide the Category if category total is 0.
                    e.Item.Visible = false;
                }
            }
        }

        protected void SubCategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            subCategoryTotal = 0.0;
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater itemRepeater = (Repeater)item.FindControl("itemRepeater");
                Label lblSubCategory = (Label)e.Item.FindControl("lblSubCategory");
                Label lblSubCategoryTotal = (Label)(e.Item.FindControl("lblSubCategoryTotal"));
                DataTable itemTable = GetBudgetItems(lblSubCategory.Text);

                if (itemTable != null && itemTable.Rows.Count > 0)
                {
                    itemTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetItem;
                    itemRepeater.DataSource = itemTable.DefaultView.Table;
                    itemRepeater.DataBind();
                    lblSubCategoryTotal.Text = string.Format("{0:C}", Convert.ToDouble(subCategoryTotal)); //Convert.ToString(subCategoryTotal);
                    categoryTotal += subCategoryTotal;
                }

                // Hide the Subcategory the subcategory total is 0.
                if (subCategoryTotal <= 0)
                {
                    e.Item.Visible = false;
                }
            }
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            itemTotal = 0.0;
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                // Get the item id from hidden field.
                HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
                Label lblActualTotal = (Label)item.FindControl("lblActualTotal");
                Label lblBudgetItem = (Label)item.FindControl("lblBudgetItem");

                DataTable actualTable = GetActuals(Convert.ToInt32(hdnItemId.Value));

                if (actualTable != null && actualTable.Rows.Count > 0)
                {
                    Repeater itemDataRepeater = (Repeater)item.FindControl("ItemDataRepeater");
                    itemDataRepeater.DataSource = actualTable.DefaultView.Table;
                    itemDataRepeater.DataBind();
                    lblActualTotal.Text = string.Format("{0:C}", Convert.ToDouble(itemTotal));
                    subCategoryTotal += Convert.ToDouble(itemTotal);
                }
            }
        }

        protected void ItemDataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Label lblActualAmount = (Label)item.FindControl("lblActual");

                Label lblStartDate = (Label)item.FindControl("lblStartDate");
                Label lblEndDate = (Label)item.FindControl("lblEndDate");
                lblStartDate.Text = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(lblStartDate.Text), false);
                lblEndDate.Text = UGITUtility.GetDateStringInFormat(Convert.ToDateTime(lblEndDate.Text), false);
                itemTotal += Convert.ToDouble(lblActualAmount.Text);
                lblActualAmount.Text = string.Format("{0:C}", Convert.ToDouble(lblActualAmount.Text));
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // If request is for export the report in given form like pdf,excel etc.
            if (Request["ExportReport"] != null)
            {
                string exportType = Request["exportType"];

                if (exportType == "pdf" || exportType == "image")
                {
                    string headerTitle = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter(sb));
                    //Render the page to the new HtmlTextWriter which actually writes to the stringbuilder
                    base.Render(tw);
                    //Get the rendered content
                    string sContent = sb.ToString();

                    //Now output it to the page, if you want
                    writer.Write(sContent);
                    string html = sb.ToString();

                    ExportReport convert = new ExportReport();
                    convert.ScriptsEnabled = true;
                    convert.ShowFooter = true;
                    convert.ShowHeader = true;
                    int reportType = 0;
                    string reportTypeString = "pdf";
                    string contentType = "Application/pdf";
                    if (exportType == "IMAGE")
                    {
                        reportType = 1;
                        reportTypeString = "png";
                        contentType = "image/png";
                    }
                    convert.ReportType = reportType;
                    html = string.Format(@"<html><head></head><body>{0}</body></html>", html);
                    byte[] bytes = convert.GetReportFromHTML(html, "");

                    string fileName = string.Format("export.{0}", reportTypeString);
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = contentType;
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
                else if (exportType == "excel")
                {
                    // Filter && Copy the budget table in temporary table.
                    DataTable excelTable = FilterCategoriesToView();

                    // Rename column name to display in excel sheet.
                    excelTable.Columns[DatabaseObjects.Columns.DepartmentLookup].ColumnName = "Vendor";
                    excelTable.Columns[DatabaseObjects.Columns.GLCode].ColumnName = "PO/Invoice #";
                    excelTable.Columns[DatabaseObjects.Columns.Actuals].ColumnName = "Actual";

                    //// Remove the unnecessary column.
                    //excelTable.Columns.Remove(DatabaseObjects.Columns.BudgetAmount);
                    //excelTable.Columns.Remove(DatabaseObjects.Columns.Variance);

                    if (!enableBudgetCategoryType)
                        excelTable.Columns.Remove(DatabaseObjects.Columns.BudgetType);
                    else
                        excelTable.Columns[DatabaseObjects.Columns.BudgetType].ColumnName = "Budget Type";

                    // Convert the data in csv format.
                    string csvData = UGITUtility.ConvertTableToCSV(excelTable);
                    string attachment = string.Format("attachment; filename={0}.csv", "Export");
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "text/csv";
                    Response.Write(csvData.ToString());
                    Response.Flush();
                    Response.End();
                }
                // In case of print is called from report
                else if (exportType == "print")
                {
                    printReport = true;
                }
            }

            base.Render(writer);
        }

        private DataTable GetActuals(int itemId)
        {
            DataRow[] items = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, itemId));
            if (items.Length > 0)
                return items.CopyToDataTable();
            else
                return null;
        }

        private DataTable GetSubCategory(string category)
        {
            DataRow[] subCategories = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategory, category));
            DataTable filteredTable = null;

            if (subCategories.Length > 0)
            {
                DataTable tempTable = subCategories.CopyToDataTable();
                tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetSubCategory);
                filteredTable = tempTable;
            }
            return filteredTable;
        }

        private DataTable GetBudgetItems(string subCategory)
        {
            DataTable tempTable = null;
            DataRow[] budgetItems = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetSubCategory, subCategory));

            if (budgetItems.Length > 0)
            {
                tempTable = budgetItems.CopyToDataTable();
                tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetItem, DatabaseObjects.Columns.Id);
            }
            return tempTable;
        }

        private void BindLevel1Category()
        {
            //Get the catgeory, to which user is authorized to view.
            if (authorizedCategories != null && authorizedCategories.Rows.Count > 0)
            {
                DataTable categoryTable = authorizedCategories.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                chkBoxCategoryList.DataSource = categoryTable;
                chkBoxCategoryList.DataTextField = DatabaseObjects.Columns.BudgetCategoryName;
                chkBoxCategoryList.DataValueField = DatabaseObjects.Columns.BudgetCategoryName;
                chkBoxCategoryList.DataBind();

                foreach (ListItem item in chkBoxCategoryList.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private DataTable GetCurrentPlannedData()
        {
            ///Generate the query.
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@BudgetStartDate", StartDate.ToString("yyyy-MM-dd"));
            values.Add("@BudgetEndDate", EndDate.ToString("yyyy-MM-dd"));
            values.Add("@TenantID", context.TenantID);
            DataTable budgetSummaryTable = CreateBudgetTableTemplate();
            budgetSummaryTable = GetTableDataManager.GetData("RPT_ModuleBudgetActual", values);
            return budgetSummaryTable;
        }

        private void FilterSelectedCategory(List<string> selectedCategory)
        {
            DataTable tempTable = budgetTable.Clone();

            // Filter the selected category.
            if (budgetTable != null && budgetTable.Rows.Count > 0)
            {
                foreach (string item in selectedCategory)
                {
                    DataRow[] categoryRow = budgetTable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategory, item));

                    foreach (DataRow dr in categoryRow)
                    {
                        tempTable.ImportRow(dr);
                    }
                }
            }
            // oveerwrite the budget table.
            budgetTable = tempTable;
        }

        private DataTable CreateBudgetTableTemplate()
        {
            DataTable projectBudgetTable = new DataTable();

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Id, typeof(Int32));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Id].DefaultValue = null;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetType, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetType].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetCategory].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetSubCategory].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetItem].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Title].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetStartDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetStartDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetEndDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetEndDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.DepartmentLookup, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.DepartmentLookup].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.GLCode, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.GLCode].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Actuals, typeof(double));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Actuals].DefaultValue = 0.0;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetAmount].DefaultValue = 0.0;

            projectBudgetTable.Columns.Add("Variance", typeof(double));
            projectBudgetTable.Columns["Variance"].DefaultValue = 0.0;

            return projectBudgetTable;
        }

        private DataTable FilterCategoriesToView()
        {
            DataTable budgetCategory = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory);
            DataTable filteredTable = budgetTable.Clone();

            foreach (DataRow categoryRow in budgetCategory.Rows)
            {
                string category = Convert.ToString(categoryRow[DatabaseObjects.Columns.BudgetCategory]);

                DataRow[] subCategories = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategory, category));

                if (subCategories.Length > 0)
                {
                    DataTable tempTable = subCategories.CopyToDataTable();
                    // tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetSubCategory);

                    foreach (DataRow dr in tempTable.Rows)
                    {
                        //SPQuery query = new SPQuery();
                        //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.BudgetCategory, category, DatabaseObjects.Columns.BudgetSubCategory, dr[DatabaseObjects.Columns.BudgetSubCategory]);
                        //SPListItemCollection subcategories = categoryList.GetItems(query);
                        var subcategories = categoryList.Where(x => x.BudgetCategoryName == category && x.BudgetSubCategory == UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.BudgetSubCategory]));
                        if (subcategories!=null)
                        {
                            //if (string.IsNullOrEmpty(Convert.ToString(subcategories[0][DatabaseObjects.Columns.AuthorizedToView])) || UserProfile.IsUserPresentInField(SPContext.Current.Web.CurrentUser, subcategories[0], DatabaseObjects.Columns.AuthorizedToView))
                            //{
                                filteredTable.ImportRow(dr);
                            //}
                        }
                    }
                }
            }
            budgetTable = filteredTable;
            return budgetTable;
        }
    }
}