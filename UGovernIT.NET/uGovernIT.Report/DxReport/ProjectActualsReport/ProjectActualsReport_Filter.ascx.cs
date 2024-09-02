using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Text;
using uGovernIT.Report.Helpers;

namespace uGovernIT.Report.DxReport
{
    public partial class ProjectActualsReport : UserControl
    {
        public int PMMId { get; set; }
        public string TicketID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Category { get; set; }

        private DataTable PMMRePlanBudgetTable = null;
        private DataTable budgetTable = new DataTable();
        private DataTable subCategoryTable = null;

        private double subCategoryTotal = 0.0;
        private double categoryTotal = 0.0;
        private double budgetTypeTotal = 0.0;
        private double itemTotal = 0.0;

        private double grandTotalActual = 0.0;
        public string minusURL = string.Empty;
        public string plusURL = string.Empty;

        bool enableBudgetCategoryType = false;
        List<BudgetCategory> authorizedCategories = null;

        private string reportURL = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();

        private List<BudgetCategory>  categoryList = null;
        protected bool printReport = false;
        ConfigurationVariableManager ConfigVariableManager;
        BudgetCategoryViewManager BudgetCategoryManager;
        ModuleBudgetManager ModuleBudgetMGR;
        BudgetActualsManager BudgetActualsMGR;
        ApplicationContext AppContext;
        protected void Page_Load(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            ModuleBudgetMGR = new ModuleBudgetManager(AppContext);
            BudgetCategoryManager = new BudgetCategoryViewManager(AppContext);
            ConfigVariableManager = new ConfigurationVariableManager(AppContext);
            BudgetActualsMGR = new BudgetActualsManager(AppContext);
            //Load category list.
            categoryList = BudgetCategoryManager.Load();
            minusURL = UGITUtility.GetImageUrlForReport("/Report/Content/images/minus.png");
            plusURL = UGITUtility.GetImageUrlForReport("/Report/Content/images/plus.png");

            if (Request["alltickets"] != null)
            {
                TicketID = UGITUtility.SplitString(UGITUtility.ObjectToString(Request["alltickets"]), Constants.Separator6)[0];
            }
            // Load all authorized Category and SubCategories.
            authorizedCategories = BudgetCategoryManager.LoadCategories(AppContext.CurrentUser.Id, 1);
            if (authorizedCategories != null && authorizedCategories.Count > 0)
                authorizedCategories = authorizedCategories.GroupBy(x => x.BudgetCategoryName).Select(x => x.FirstOrDefault()).ToList();
            // Get the configuration from config whether we have to display Budget Type in report or not.
            enableBudgetCategoryType = ConfigVariableManager.GetValueAsBool(ConfigConstants.EnableBudgetCategoryType);

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
                    byte[] bytes = convert.GetReportFromHTML(html, AppContext.SiteUrl);

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
            Category = new List<string>();
            string categories = string.Empty;
            foreach (ListItem categoryItem in chkBoxCategoryList.Items)
            {
                if (categoryItem.Selected)
                {
                    Category.Add(categoryItem.Text);
                    reportURL = reportURL + "&Cat=" + categoryItem.Text;
                }
            }

            //Getnerate the report according to selected criteria.
            GenerateReport(StartDate, EndDate, Category);
        }

        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.AbsoluteUri));
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

        private void GenerateReport(DateTime startdate, DateTime enddate, List<string> selectedcategory)
        {
            // Show hide the report components and headings.
            string Title = string.Empty;
            DataRow ticketRow = Ticket.GetCurrentTicket(AppContext, ModuleNames.PMM, TicketID);
            if (ticketRow != null)
                Title = UGITUtility.ObjectToString(ticketRow[DatabaseObjects.Columns.Title]);
            lblHeading.Text = $"Project Actuals Report<BR> {TicketID}: {Title}";
            if (dtDateFrom.Value == null && dtDateTo.Value == null)
                lblSubHeading.Text = string.Empty;
            else
                lblSubHeading.Text = "Report from: " + (string.IsNullOrEmpty( dtDateFrom.Date.ToString() ) ? "Earliest" : UGITUtility.GetDateStringInFormat(dtDateFrom.Date, false)) +
                                              " to " + (string.IsNullOrEmpty(dtDateTo.Date.ToString()) ? "Latest" : UGITUtility.GetDateStringInFormat(dtDateTo.Date, false));
             
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

                // if configuration enableBudgetCategoryType is false insert a blank row in budgetTypeTable so that we can find and bind the category repeater in the itemDataBound event().
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
                    
                    // Show Butget type total only when Budget Type is enabled.
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
                Label lblCategory =  (Label)e.Item.FindControl("lblCategory");
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

                // Hide the Category if category total is 0.
                if (categoryTotal <= 0)
                {
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
                 Repeater itemRepeater = (Repeater)item.FindControl("ItemRepeater");
                 Label lblSubCategory = (Label)e.Item.FindControl("lblSubCategory");
                 Label lblSubCategoryTotal = (Label)(e.Item.FindControl("lblSubCategoryTotal"));
                 DataTable itemTable = GetItems(lblSubCategory.Text);

                 if (itemTable != null && itemTable.Rows.Count > 0)
                 {
                     itemTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetItem;
                     itemRepeater.DataSource = itemTable.DefaultView.Table;
                     itemRepeater.DataBind();
                     lblSubCategoryTotal.Text = string.Format("{0:C}", Convert.ToDouble(subCategoryTotal)); //Convert.ToString(subCategoryTotal);
                     categoryTotal += subCategoryTotal;
                 }

                 if (subCategoryTotal <= 0)
                 {
                     // Hide the Subcategory the subcategory total is 0.
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
                HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
                Label lblActualTotal = (Label)item.FindControl("lblActualTotal");
                Label lblBudgetItem = (Label)item.FindControl("lblBudgetItem");
                
                // Get actuals data of an item.
                DataTable actualTable = GetActuals(Convert.ToInt32(hdnItemId.Value));

                // Bind the actuals of an item.
                if (actualTable != null && actualTable.Rows.Count > 0)
                {
                    // Get the total from 0th row of the table.
                    Repeater itemDataRepeater = (Repeater)item.FindControl("ItemDataRepeater");
                    // Bind the table with repeater.
                    actualTable.DefaultView.Sort = DatabaseObjects.Columns.AllocationStartDate;
                    itemDataRepeater.DataSource = actualTable.DefaultView.Table;
                    itemDataRepeater.DataBind();

                    lblActualTotal.Text = string.Format("{0:C}", Convert.ToDouble(itemTotal));
                    subCategoryTotal += Convert.ToDouble(itemTotal);
                }
                else
                {
                    // Hide the budgetItem if there is no actual for that item.
                    e.Item.Visible = false;
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

        private void CreateSelectedCategoryBudgetTable(List<string> selectedCategory)
        {
            PMMRePlanBudgetTable = ModuleBudgetMGR.GetDataTable($"{DatabaseObjects.Columns.TicketId} = '{TicketID}' ");

            DataColumn budgetType = new DataColumn(DatabaseObjects.Columns.BudgetType, typeof(string));
            DataColumn category = new DataColumn(DatabaseObjects.Columns.BudgetCategory, typeof(string));
            DataColumn subCategory = new DataColumn(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            DataColumn budgetItem = new DataColumn(DatabaseObjects.Columns.BudgetItem, typeof(string));
            DataColumn itemID = new DataColumn(DatabaseObjects.Columns.Id, typeof(Int32));

            budgetTable.Columns.Add(budgetType);
            budgetTable.Columns.Add(category);
            budgetTable.Columns.Add(subCategory);
            budgetTable.Columns.Add(budgetItem);
            budgetTable.Columns.Add(itemID);

            //Load the budget category type table
            List<BudgetCategory> categoryTable = categoryList;

            if (PMMRePlanBudgetTable != null)
            {
                foreach (DataRow dr in PMMRePlanBudgetTable.Rows)
                {
                    if (selectedCategory.Contains(Convert.ToString(dr[DatabaseObjects.Columns.BudgetCategory])))
                    {
                        DataRow row = budgetTable.NewRow();
                        BudgetCategory categoryRow = categoryTable.FirstOrDefault(x=>x.BudgetCategoryName == UGITUtility.ObjectToString( dr[DatabaseObjects.Columns.BudgetCategory]));

                       if (categoryRow != null)
                            row[DatabaseObjects.Columns.BudgetType] = categoryRow.BudgetType;

                        row[DatabaseObjects.Columns.BudgetCategory] = dr[DatabaseObjects.Columns.BudgetCategory];
                        row[DatabaseObjects.Columns.BudgetSubCategory] = dr[DatabaseObjects.Columns.BudgetSubCategory];
                        row[DatabaseObjects.Columns.BudgetItem] = dr[DatabaseObjects.Columns.BudgetItem];
                        row[DatabaseObjects.Columns.Id] = dr[DatabaseObjects.Columns.Id];
                        budgetTable.Rows.Add(row);
                    }
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

        private DataTable GetItems(string subCategory)
        {
            DataTable tempTable = null;
            DataRow[] budgetItems = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetSubCategory, subCategory));

            if (budgetItems.Length > 0)
            {
                tempTable = budgetItems.CopyToDataTable();
                tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetItem,DatabaseObjects.Columns.Id);
            }
            return tempTable;
        }
                
        private DataTable GetActuals(int itemId)
        {
            DataRow[] items = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, itemId));
            if (items.Length > 0)
                return items.CopyToDataTable();
            else
                return null;
        }

        private DataTable CreateBudgetTable()
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
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.AllocationStartDate].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.AllocationEndDate].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Actuals, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Actuals].DefaultValue = string.Format("{0:C}", 0.0);
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetDescription].DefaultValue = string.Empty;
           
            return projectBudgetTable;
        }

        private void BindLevel1Category()
        {
            //Get the catgeory, to which user is authorized to view.
            if (authorizedCategories != null && authorizedCategories.Count > 0)
            {
                List<BudgetCategory> categoryTable = authorizedCategories;  //authorizedCategories.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategory);
                chkBoxCategoryList.DataSource = categoryTable;
                chkBoxCategoryList.DataTextField = DatabaseObjects.Columns.BudgetCategoryName;
                chkBoxCategoryList.DataValueField = DatabaseObjects.Columns.ID;
                chkBoxCategoryList.DataBind();

                foreach (ListItem item in chkBoxCategoryList.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private DataTable GetCurrentPlannedData()
        {

            ////Execute the query.
            List<BudgetActual> budgetCollectionTable = BudgetActualsMGR.Load(x => ((x.AllocationStartDate >= StartDate && x.AllocationStartDate <= EndDate) || (x.AllocationEndDate >= StartDate && x.AllocationEndDate <= EndDate)) && x.TicketId == TicketID); // oWeb.GetSiteData(childQuery);

            ////Load the budget category list.
            List<BudgetCategory> categoryList = BudgetCategoryManager.Load();

            //// Create the target table template.
            DataTable budgetSummaryTable = CreateBudgetTable();

            if (budgetCollectionTable.Count > 0)
            {
                DataTable plannedTable = UGITUtility.ToDataTable<BudgetActual>(budgetCollectionTable);

                foreach (DataRow dr in plannedTable.Rows)
                {
                    ModuleBudget budgetItem = ModuleBudgetMGR.LoadByID(UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ModuleBudgetLookup]));   //SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMBudget, Convert.ToInt32(UGITUtility.SplitString(Convert.ToString(dr[DatabaseObjects.Columns.PMMBudgetLookup]), ";#", 0)));

                    if (budgetItem != null)
                    {
                        DataRow summaryRow = budgetSummaryTable.NewRow();
                        summaryRow[DatabaseObjects.Columns.Id] = UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.ModuleBudgetLookup]);
                        summaryRow[DatabaseObjects.Columns.BudgetItem] = UGITUtility.SplitString(Convert.ToString(dr[DatabaseObjects.Columns.ModuleBudgetLookup]), ";#", 1);
                        summaryRow[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        summaryRow[DatabaseObjects.Columns.Actuals] = dr[DatabaseObjects.Columns.BudgetAmount];
                        summaryRow[DatabaseObjects.Columns.BudgetSubCategory] = UGITUtility.SplitString(Convert.ToString(budgetItem.BudgetCategoryLookup), ";#", 1);
                        summaryRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.AllocationStartDate];
                        summaryRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.AllocationEndDate];

                        // Get category and Budget Type
                        string category = string.Empty;
                        string budgetType = string.Empty;
                        string budgetSubCategory = string.Empty;

                        BudgetCategory categoryItem = categoryList.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(budgetItem.BudgetCategoryLookup));

                        if (categoryItem != null)
                        {
                            category = UGITUtility.ObjectToString(categoryItem.BudgetCategoryName);
                            budgetType = UGITUtility.ObjectToString(categoryItem.BudgetType);
                            budgetSubCategory = UGITUtility.ObjectToString(categoryItem.BudgetSubCategory);
                        }
                        summaryRow[DatabaseObjects.Columns.BudgetType] = budgetType;
                        summaryRow[DatabaseObjects.Columns.BudgetCategory] = category;
                        summaryRow[DatabaseObjects.Columns.BudgetSubCategory] = budgetSubCategory;
                        budgetSummaryTable.Rows.Add(summaryRow);
                    }
                }
            }

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
                        List<BudgetCategory> subcategories = categoryList.Where(x => x.BudgetCategoryName == category && x.BudgetSubCategory == UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.BudgetSubCategory])).ToList(); //categoryList.GetItems(query);

                        if (subcategories.Count > 0)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(subcategories.First().AuthorizedToView)) || subcategories.First().AuthorizedToView.Contains(AppContext.CurrentUser.Id)) //code need to convert: || AppContext.UserManager.IsUserPresentInField(AppContext.CurrentUser, subcategories[0], DatabaseObjects.Columns.AuthorizedToView)
                            {
                                filteredTable.ImportRow(dr);
                            }
                        }
                    }
                }
            }
            budgetTable = filteredTable;
            return budgetTable;
        }
    }
}
