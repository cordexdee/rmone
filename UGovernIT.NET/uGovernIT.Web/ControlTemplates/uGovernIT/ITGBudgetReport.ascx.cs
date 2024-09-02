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
using uGovernIT.Utility;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web
{
    public partial class ITGBudgetReport : System.Web.UI.UserControl
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Category { get; set; }

        bool enableBudgetCategoryType = false;

        private double actualTotalOnBudgetType = 0.0;
        private double actualTotalOnSubCategory = 0.0;
        private double actualTotalOnCategory = 0.0;

        private double plannedTotalOnBudgetType = 0.0;
        private double plannedTotalOnCategory = 0.0;
        private double plannedTotalOnSubCategory = 0.0;

        private double varianceTotalOnBudgetType = 0.0;
        private double varianceTotalOnCategory = 0.0;
        private double varianceTotalOnSubCategory = 0.0;

        protected string ajaxPageURL = string.Empty;

        private DataTable budgetTable = null;

        private double grandTotalBudget = 0.0;
        private double grandTotalVariance = 0.0;
        private double grandTotalActual = 0.0;
        ApplicationContext context = null;
        DataTable authorizedCategories = null;
        BudgetCategoryViewManager objmgr;
        ConfigurationVariableManager objconfigvar;
       // BudgetActualsManager objBudgetActualsmgr;
        private string reportURL = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();
        private List<BudgetCategory> categoryList = null;
 

        protected bool printReport = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Load category list.
            context = HttpContext.Current.GetManagerContext();
            objmgr = new BudgetCategoryViewManager(context);
            //objBudgetActualsmgr = new BudgetActualsManager(context);
            objconfigvar = new ConfigurationVariableManager(context);
            //categoryList = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);
            categoryList = objmgr.Load();
            // Load all authorized Category and SubCategories.
            //authorizedCategories = BudgetCategory.LoadCategoties(SPContext.Current.Web.CurrentUser.ID, 1);
            authorizedCategories = objmgr.LoadCategories();
            // Get the configuration from config whether we have to display Budget Type in report or not.
            enableBudgetCategoryType = UGITUtility.StringToBoolean(objconfigvar.GetValue(ConfigurationVariable.EnableBudgetCategoryType));

            // Get the refrence of ajax helper page.
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/AjaxHelper.aspx");

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

        protected void BudgetTypeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            plannedTotalOnBudgetType = 0.0;
            actualTotalOnBudgetType = 0.0;
            varianceTotalOnBudgetType = 0.0;

            // Select the distince category from budget table.
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater categoryRepeater = (Repeater)item.FindControl("categoryRepeater");
                DataTable categoryTable = null;
                Label lblBudgetType = (Label)e.Item.FindControl("lblBudgetType");
                Label lblPlannedTotalOnBudgetType = (Label)e.Item.FindControl("lblPlannedTotalOnBudgetType");
                Label lblVarianceTotalOnBudgetType = (Label)e.Item.FindControl("lblVarianceTotalOnBudgetType");
                Label lblActualTotalOnBudgetType = (Label)e.Item.FindControl("lblActualTotalOnBudgetType");

                // If configuration enableBudgetCategoryType is false bind all the category at once else bind the categories according to the BudgetType.
                if (!enableBudgetCategoryType)
                {
                    categoryTable = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                    HtmlImage img = (HtmlImage)e.Item.FindControl("collapseImage");
                    img.Visible = false;
                }
                else
                {
                    DataRow[] categoryRows;

                    if (lblBudgetType.Text != string.Empty)
                        categoryRows = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetType, lblBudgetType.Text));
                    else
                        categoryRows = budgetTable.Select(string.Format("{0} is null", DatabaseObjects.Columns.BudgetType));

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

                    // Show Butget type total only when Budget Type is enabled.
                    if (enableBudgetCategoryType)
                    {
                        lblPlannedTotalOnBudgetType.Text = string.Format("{0:C}", Convert.ToDouble(plannedTotalOnBudgetType));
                        lblVarianceTotalOnBudgetType.Text = string.Format("{0:C}", Convert.ToDouble(varianceTotalOnBudgetType));
                        lblActualTotalOnBudgetType.Text = string.Format("{0:C}", Convert.ToDouble(actualTotalOnBudgetType));
                    }
                }

                if (plannedTotalOnBudgetType <= 0)
                {
                    e.Item.Visible = false;
                }
            }

            if (item.ItemType == ListItemType.Footer)
            {
                Label lblGTotalBudget = (Label)e.Item.FindControl("lblGTotalBudget");
                Label lblGTotalActual = (Label)e.Item.FindControl("lblGTotalActual");
                Label lblGTotalVariance = (Label)e.Item.FindControl("lblGTotalVariance");

                lblGTotalBudget.Text = string.Format("{0:C}", Convert.ToDouble(grandTotalBudget));
                lblGTotalVariance.Text = string.Format("{0:C}", Convert.ToDouble(grandTotalVariance));
                lblGTotalActual.Text = string.Format("{0:C}", Convert.ToDouble(grandTotalActual));
            }
        }

        protected void CategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            plannedTotalOnCategory = 0.0;
            actualTotalOnCategory = 0.0;
            varianceTotalOnCategory = 0.0;

            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater subCategoryRepeater = (Repeater)item.FindControl("subCategoryRepeater");
                Label lblCategory = (Label)e.Item.FindControl("lblCategory");
                Label lblPlannedTotalOnCategory = (Label)e.Item.FindControl("lblPlannedTotalOnCategory");
                Label lblVarianceTotalOncategory = (Label)e.Item.FindControl("lblVarianceTotalOncategory");
                Label lblActualTotalOnCategory = (Label)e.Item.FindControl("lblActualTotalOnCategory");

                DataTable subCategoryTable = GetSubCategory(lblCategory.Text);

                if (subCategoryTable != null && subCategoryTable.Rows.Count > 0)
                {
                    subCategoryTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetSubCategory;
                    subCategoryRepeater.DataSource = subCategoryTable.DefaultView.Table;
                    subCategoryRepeater.DataBind();
                    lblPlannedTotalOnCategory.Text = string.Format("{0:C}", Convert.ToDouble(plannedTotalOnCategory));
                    lblVarianceTotalOncategory.Text = string.Format("{0:C}", Convert.ToDouble(varianceTotalOnCategory));
                    lblActualTotalOnCategory.Text = string.Format("{0:C}", Convert.ToDouble(actualTotalOnCategory));

                    plannedTotalOnBudgetType += plannedTotalOnCategory;
                    actualTotalOnBudgetType += actualTotalOnCategory;
                    varianceTotalOnBudgetType += varianceTotalOnCategory;

                    // calculate the Grand total
                    grandTotalBudget += plannedTotalOnCategory;
                    grandTotalVariance += varianceTotalOnCategory;
                    grandTotalActual += actualTotalOnCategory;

                }

                if (plannedTotalOnCategory <= 0)
                {
                    // Hide the Subcategory the subcategory total is 0.
                    e.Item.Visible = false;
                }

            }
        }

        protected void SubCategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            actualTotalOnSubCategory = 0.0;
            plannedTotalOnSubCategory = 0.0;
            varianceTotalOnSubCategory = 0.0;

            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater itemRepeater = (Repeater)item.FindControl("ItemRepeater");
                Label lblSubCategory = (Label)e.Item.FindControl("lblSubCategory");
                Label lblSubCategoryTotal = (Label)(e.Item.FindControl("lblSubCategoryTotal"));
                Label lblVarianceTotalOnSubcategorybel = (Label)(e.Item.FindControl("lblVarianceTotalOnSubcategorybel"));
                Label lblPlannedTotalOnSubcategory = (Label)(e.Item.FindControl("lblPlannedTotalOnSubcategory"));

                // DataTable itemTable = GetCurrentPlannedData(lblSubCategory.Text);

                DataTable itemTable = GetItemData(lblSubCategory.Text);

                if (itemTable != null && itemTable.Rows.Count > 0)
                {
                    itemTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetItem;
                    itemRepeater.DataSource = itemTable.DefaultView.Table;
                    itemRepeater.DataBind();
                    lblSubCategoryTotal.Text = string.Format("{0:C}", Convert.ToDouble(actualTotalOnSubCategory));
                    lblVarianceTotalOnSubcategorybel.Text = string.Format("{0:C}", Convert.ToDouble(varianceTotalOnSubCategory));
                    lblPlannedTotalOnSubcategory.Text = string.Format("{0:C}", Convert.ToDouble(plannedTotalOnSubCategory));

                    actualTotalOnCategory += actualTotalOnSubCategory;
                    plannedTotalOnCategory += plannedTotalOnSubCategory;
                    varianceTotalOnCategory += varianceTotalOnSubCategory;
                }

                if (plannedTotalOnSubCategory <= 0)
                {
                    // Hide the Subcategory the subcategory total is 0.
                    e.Item.Visible = false;
                }
            }
        }

        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                // Get the item id from hidden field.
                HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
                Label lblActualTotal = (Label)item.FindControl("lblActualTotal");

                Label lblPlanedAmt = (Label)item.FindControl("lblPlanedAmt");
                Label lblActualAmt = (Label)item.FindControl("lblActualAmt");
                Label lblVarianceAmt = (Label)item.FindControl("lblVarianceAmt");

                actualTotalOnSubCategory += Convert.ToDouble(lblActualAmt.Text);
                plannedTotalOnSubCategory += Convert.ToDouble(lblPlanedAmt.Text);
                varianceTotalOnSubCategory += Convert.ToDouble(lblVarianceAmt.Text);

                lblPlanedAmt.Text = string.Format("{0:C}", Convert.ToDouble(lblPlanedAmt.Text));
                lblVarianceAmt.Text = string.Format("{0:C}", Convert.ToDouble(lblVarianceAmt.Text));
                lblActualAmt.Text = string.Format("{0:C}", Convert.ToDouble(lblActualAmt.Text));
            }
        }

        private DataTable GetCurrentPlannedData()
        {
           
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@BudgetStartDate", StartDate.ToString("yyyy-MM-dd"));
            values.Add("@BudgetEndDate", EndDate.ToString("yyyy-MM-dd"));
            values.Add("@TenantID", context.TenantID);
            DataTable budgetSummaryTable = CreateBudgetTableTemplate();
            budgetSummaryTable = GetTableDataManager.GetData("RPT_ModuleBudget", values);
            return budgetSummaryTable;
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
                    excelTable.Columns[DatabaseObjects.Columns.DepartmentLookup].ColumnName = "Department";
                    excelTable.Columns[DatabaseObjects.Columns.Actuals].ColumnName = "Actual";
                    excelTable.Columns[DatabaseObjects.Columns.BudgetAmount].ColumnName = "Planned";

                    // Remove the unnecessary column.
                    excelTable.Columns.Remove(DatabaseObjects.Columns.BudgetStartDate);
                    excelTable.Columns.Remove(DatabaseObjects.Columns.BudgetEndDate);

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

        private void GenerateReport(DateTime startdate, DateTime enddate, List<string> selectedcategory)
        {
            // Show hide the report components and headings.
            lblHeading.Text = "Non-Project Budget Report";
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

        private DataTable CreateBudgetTableTemplate()
        {
            DataTable projectBudgetTable = new DataTable();

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Id, typeof(Int32));
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetType, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetType].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetCategory].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetSubCategory].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetItem].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.GLCode, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.GLCode].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.DepartmentLookup, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.DepartmentLookup].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetAmount].DefaultValue = 0.0;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Actuals, typeof(double));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Actuals].DefaultValue = 0.0;

            projectBudgetTable.Columns.Add("Variance", typeof(double));
            projectBudgetTable.Columns["Variance"].DefaultValue = 0.0;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetStartDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetStartDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetEndDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetEndDate].DefaultValue = string.Empty;

            return projectBudgetTable;
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

        private DataTable GetSubCategory(string category)
        {
            DataRow[] subCategories = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategory, category));
            DataTable filteredTable = null;

            if (subCategories.Length > 0)
            {
                DataTable tempTable = subCategories.CopyToDataTable();
                tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetSubCategory);

                filteredTable = tempTable;
                //foreach (DataRow dr in tempTable.Rows)
                //{
                //    DataRow[] subCategoryRow = authorizedCategories.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetSubCategory, dr[DatabaseObjects.Columns.BudgetSubCategory]));
                //    if (subCategoryRow.Length > 0)
                //    {
                //        filteredTable.ImportRow(dr);
                //    }
                //}
            }
            return filteredTable;
        }

        private DataTable GetItemData(string subCategory)
        {
            DataRow[] items = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetSubCategory, subCategory));
            if (items.Length > 0)
                return items.CopyToDataTable();
            else
                return null;
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
                        string query = string.Empty;
                        //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.BudgetCategory, category, DatabaseObjects.Columns.BudgetSubCategory, dr[DatabaseObjects.Columns.BudgetSubCategory]);
                        //SPListItemCollection subcategories = categoryList.GetItems(query);
                        var subcategories = categoryList.Where(x=>x.BudgetCategoryName== category && x.BudgetSubCategory== UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.BudgetSubCategory]));

                        if (subcategories!=null)
                        {
                            //if (string.IsNullOrEmpty(Convert.ToString(subcategories[0][DatabaseObjects.Columns.AuthorizedToView])) || context.UserManager.IsUserPresentInField(context.CurrentUser, subcategories[0], DatabaseObjects.Columns.AuthorizedToView))
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