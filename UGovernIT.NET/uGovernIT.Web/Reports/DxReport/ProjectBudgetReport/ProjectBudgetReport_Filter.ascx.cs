using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using uGovernIT.Helpers;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Text;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
namespace uGovernIT.DxReport
{
    public partial class ProjectBudgetReport_Filter : UserControl
    {
        #region Property
        public int PMMId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Category { get; set; }
        public string TicketId { get; set; }
        private DataTable budgetTable = new DataTable();
        private DataTable subCategoryTable = null;

        private double actualTotalOnBudgetType = 0.0;
        private double actualTotalOnSubCategory = 0.0;
        private double actualTotalOnCategory = 0.0;

        private double plannedTotalOnBudgetType = 0.0;
        private double plannedTotalOnCategory = 0.0;
        private double plannedTotalOnSubCategory = 0.0;

        private double varianceTotalOnBudgetType = 0.0;
        private double varianceTotalOnCategory = 0.0;
        private double varianceTotalOnSubCategory = 0.0;

        private double grandTotalBudget = 0.0;
        private double grandTotalVariance = 0.0;
        private double grandTotalActual = 0.0;

        protected string ajaxPageURL = string.Empty;

        bool enableBudgetCategoryType = false;
        DataTable authorizedCategories = null;
        

        private string reportURL = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();

        public string minusURL = string.Empty;
        public string plusURL = string.Empty;

        protected bool printReport = false;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        BudgetCategoryViewManager budgetCategoryViewManager = null;
        ConfigurationVariableManager configurationVariableManager = null;
        TicketManager _ticketManager = null;
        UserProfileManager userProfileManager = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _ticketManager = new TicketManager(context);
            budgetCategoryViewManager = new BudgetCategoryViewManager(context);
            configurationVariableManager = new ConfigurationVariableManager(context);
            userProfileManager = new UserProfileManager(context);
            if (Request["TicketId"] != null)
                TicketId = Request["TicketId"];
            //Load category list.
            //categoryList = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);

                //Load all authorized Category and SubCategories.
                //authorizedCategories = BudgetCategory.LoadCategoties(SPContext.Current.Web.CurrentUser.ID, 1);
            authorizedCategories = budgetCategoryViewManager.LoadCategories();
            // Get the configuration from config whether we have to display Budget Type in report or not.
            enableBudgetCategoryType = configurationVariableManager.GetValueAsBool(ConfigConstants.EnableBudgetCategoryType);
            //enableBudgetCategoryType = uHelper.StringToBoolean(ConfigurationVariable.GetValue(ConfigurationVariable.EnableBudgetCategoryType));
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/AjaxHelper.aspx");
            ajaxPageURL = ajaxPageURL.Replace("/report", string.Empty);
            minusURL = UGITUtility.GetImageUrlForReport("/Content/images/minus.png");
            plusURL = UGITUtility.GetImageUrlForReport("/Content/images/plus.png");

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
            urlBuilder.Append(Request.Url.PathAndQuery);
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
                    byte[] bytes = convert.GetReportFromHTML(html, string.Empty);

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

                    excelTable.Columns[DatabaseObjects.Columns.Actuals].ColumnName = "Actual";

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
            if (dtDateFrom.Value!=null)
                StartDate = dtDateFrom.Date;
            else
                StartDate = new DateTime(1999, 1, 1);
            reportURL = reportURL + "StartDate=" + StartDate.ToString();

            if (dtDateTo.Value!=null)
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
                    Category.Add(categoryItem.Value);
                    reportURL = reportURL + "&Cat=" + categoryItem.Value;
                }
            }

            //Getnerate the report according to selected criteria.
            GenerateReport(StartDate, EndDate, Category);
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
                      categoryTable = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                      HtmlImage img = (HtmlImage)e.Item.FindControl("collapseImage");
                      img.Visible = false;
                      
                  }
                  else
                  {
                       DataRow[] categoryRows;

                      if(lblBudgetType.Text != string.Empty)
                        categoryRows = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetType, lblBudgetType.Text));
                      else
                        categoryRows = budgetTable.Select(string.Format("{0} is null", DatabaseObjects.Columns.BudgetType));

                      if (categoryRows.Length > 0)
                      {
                          categoryTable = categoryRows.CopyToDataTable();
                          categoryTable = categoryTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
                      }
                  }


                  if (categoryTable != null)
                  {
                      categoryTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetCategoryName;
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
              }

              // Display the Grand total.
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
                //HtmlImage imgminusparent = (HtmlImage)e.Item.FindControl("imgminusparent");
                //imgminusparent.Src = UGITUtility.GetImageUrlForReport("/Report/Content/images/minus.png");

                //HtmlImage imgminuschild = (HtmlImage)e.Item.FindControl("imgminuschild");
                //imgminusparent.Src = UGITUtility.GetImageUrlForReport("/Report/Content/images/minus.png");

                subCategoryTable = GetSubCategory(lblCategory.Text);

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

        private void GenerateReport(DateTime startdate, DateTime enddate, List<string> selectedcategory)
        {
            // Show hide the report components and headings.
            string Title = string.Empty;
            DataRow ticketRow = Ticket.GetCurrentTicket(context, ModuleNames.PMM, TicketId);
            if (ticketRow != null)
                Title = UGITUtility.ObjectToString(ticketRow[DatabaseObjects.Columns.Title]);
            lblHeading.Text = $"Project Budget Report<BR> {TicketId}: {Title}";
            if (startdate == null && enddate == null)
                lblSubHeading.Text = string.Empty;
            else
                lblSubHeading.Text = "Report from: " + (startdate == null ? "Earliest" : UGITUtility.GetDateStringInFormat(startdate, false)) + 
                                              " to " + (enddate == null ? "Latest" : UGITUtility.GetDateStringInFormat(enddate, false));

            PnlBudgetReportPopup.Style.Add("display", "none");
            pnlBudgetComponent.Style.Add("display", "block");

            // Load the Current Planned Data.
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
                    emptyRow[DatabaseObjects.Columns.BudgetCategoryName] = "";

                    budgetTypeTable.Rows.InsertAt(emptyRow, 0);
                    budgetTypeTable.AcceptChanges();
                }

                budgetTypeRepeater.DataSource = budgetTypeTable;
                budgetTypeRepeater.DataBind();
            }
        }

        private void FilterSelectedCategory(List<string> selectedCategory)
        {
            DataTable tempTable = budgetTable.Clone();

            // Filter the selected category.
            if (budgetTable != null && budgetTable.Rows.Count > 0)
            {
                foreach (string item in selectedCategory)
                {
                    DataRow[] categoryRow = budgetTable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.BudgetCategoryName, item));

                    foreach (DataRow dr in categoryRow)
                    {
                        tempTable.ImportRow(dr);
                    }
                }
            }

            // oveerwrite the budget table.
            budgetTable = tempTable;
        }
        
        private DataTable GetSubCategory(string category)
        {
            DataRow[] subCategories = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategoryName, category));
            DataTable filteredTable = null;

            if (subCategories.Length > 0)
            {
                DataTable tempTable = subCategories.CopyToDataTable();
                tempTable = tempTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetSubCategory);

                filteredTable = tempTable;
            }
            return filteredTable;
        }

        /// <summary>
        /// This Function calculates the Planed and Actual data of 
        /// </summary>
        /// <param name="subCategory"></param>
        /// <returns></returns>
        private DataTable GetItemData(string subCategory)
        {
            DataRow[] items = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetSubCategory, subCategory));
            if (items.Length > 0)
                return items.CopyToDataTable();
            else
                return null;
        }
      
        private DataTable CreateBudgetTable()
        {
            DataTable projectBudgetTable = new DataTable();

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Id, typeof(Int32));
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetType, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetType].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetCategoryName, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetCategoryName].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetSubCategory, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetSubCategory].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetItem, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetItem].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("Planned", typeof(double));
            projectBudgetTable.Columns["Planned"].DefaultValue = 0.0;
            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.Actuals, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.Actuals].DefaultValue = (0.0);
            projectBudgetTable.Columns.Add("Variance", typeof(double));
            projectBudgetTable.Columns["Variance"].DefaultValue = 0.0;
            

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

        private DataTable GetCurrentPlannedData()
        {
            List<string> lstOfQuery = new List<string>();
            lstOfQuery.Add(string.Format("({0}>='{1}' AND {0}<='{2}')",DatabaseObjects.Columns.AllocationStartDate,StartDate.ToString(),EndDate.ToString()));
            //lstOfQuery.Add(string.Format("({0}>0)", DatabaseObjects.Columns.BudgetAmount));

            string dateQuery = string.Empty;
            if(lstOfQuery.Count>0)
                dateQuery = "(" + string.Join(" AND ", lstOfQuery) + ")";

            lstOfQuery.Clear();
            lstOfQuery.Add(dateQuery);
            lstOfQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketId));
            if(lstOfQuery.Count>0)
                dateQuery = "(" + string.Join(" AND ", lstOfQuery) + ")";

            ModuleBudgetManager budgetManager = new ModuleBudgetManager(context);
            List<ModuleBudget> budgetList = budgetManager.LoadBudgetByTicketId(TicketId);

            ModuleMonthlyBudgetManager moduleBudgetManager = new ModuleMonthlyBudgetManager(context);
            List<ModuleMonthlyBudget> moduleBudgets=  moduleBudgetManager.Load(dateQuery);
            DataTable budgetSummaryTable = CreateBudgetTable();

            //Load the budget category type table
            string projectfilter = string.Format("{0}='{1}' AND {2} > 0", DatabaseObjects.Columns.TicketId, TicketId, DatabaseObjects.Columns.BudgetCategoryLookup);
            DataTable budgetActual = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, projectfilter);

            if (moduleBudgets!=null && moduleBudgets.Count>0)
            {
                DataTable plannedTable = UGITUtility.ToDataTable<ModuleMonthlyBudget>(moduleBudgets); 
                string expression = DatabaseObjects.Columns.BudgetAmount;
                       
                // Create a double type column and copy all the values of column "BudgetAmount" to this column for further calculation because returning col is string type.
                plannedTable.Columns.Add("BudgetAmt", typeof(double), expression);

                // Group the items by sub category.
                var BudgetBySubcategories = plannedTable.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup));

                foreach (var objSubcategory in BudgetBySubcategories)
                {
                    DataRow[] subCategoryRow = objSubcategory.ToArray();
                    DataTable tblSubCategory = subCategoryRow.CopyToDataTable();

                    // Group the items by budget item Id.
                    var BudgetItems = tblSubCategory.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup));
                  
                    foreach (var obj in BudgetItems)
                    {
                        DataRow[] rows = obj.ToArray();
                        ModuleBudget row = budgetList.FirstOrDefault(x => x.TicketId == UGITUtility.ObjectToString( rows[0][DatabaseObjects.Columns.TicketId]) && x.BudgetCategoryLookup == UGITUtility.StringToLong(rows[0][DatabaseObjects.Columns.BudgetCategoryLookup]));
                        DataRow summaryRow = budgetSummaryTable.NewRow();
                        if (row != null)
                        {
                            summaryRow[DatabaseObjects.Columns.BudgetItem] = row.BudgetItem;

                            double plannedAmt = obj.Sum(x => x.Field<double>("BudgetAmt"));
                            summaryRow["Planned"] = Math.Round(plannedAmt, 2);
                            summaryRow[DatabaseObjects.Columns.BudgetSubCategory] = UGITUtility.SplitString(row.BudgetCategoryLookup, ";#", 1);

                            // Get category and Budget Type
                            string category = string.Empty;
                            string budgetType = string.Empty;
                            string budgetSubCategory = string.Empty;

                            int subCategoryId = Convert.ToInt32(UGITUtility.SplitString(row.BudgetCategoryLookup, ";#", 0));
                            string filter = string.Format("{0}='{1}' AND {2}={3}", DatabaseObjects.Columns.TenantID, context.TenantID, DatabaseObjects.Columns.ID, subCategoryId);
                            DataRow categoryItem = authorizedCategories.Select(filter)[0];

                            if (categoryItem != null)
                            {
                                category = Convert.ToString(categoryItem[DatabaseObjects.Columns.BudgetCategoryName]);
                                budgetType = Convert.ToString(categoryItem[DatabaseObjects.Columns.BudgetType]);
                                budgetSubCategory = UGITUtility.ObjectToString(categoryItem[DatabaseObjects.Columns.BudgetSubCategory]);
                            }
                            summaryRow[DatabaseObjects.Columns.BudgetType] = budgetType;
                            summaryRow[DatabaseObjects.Columns.BudgetCategoryName] = category;
                            summaryRow[DatabaseObjects.Columns.BudgetSubCategory] = budgetSubCategory;

                            string strActualFilter = string.Format("{0}={1} AND {2}", DatabaseObjects.Columns.BudgetCategoryLookup, rows[0][DatabaseObjects.Columns.BudgetCategoryLookup], dateQuery);
                            DataTable actualBudgetTable = new DataTable();
                            if (budgetActual != null && budgetActual.Rows.Count > 0)
                            {
                                DataRow[] rowColl = budgetActual.Select(strActualFilter);
                                if (rowColl != null && rowColl.Length > 0)
                                    actualBudgetTable = rowColl.CopyToDataTable();
                            }

                            if (actualBudgetTable.Rows.Count > 0)
                            {
                                // Create a double type column and copy all the values of column "BudgetAmount" to this column for further calculation because returning col is string type.
                                //actualBudgetTable.Columns.Add(DatabaseObjects.Columns.ActualCost, typeof(double), expression);

                                var actualItems = (from i in actualBudgetTable.AsEnumerable()
                                                   group i by i.Field<Int64>(DatabaseObjects.Columns.BudgetCategoryLookup) into p

                                                   select new
                                                   {
                                                       BudgetItem = p.Key,
                                                       actualAmt = p.Sum(x => x.Field<double>(DatabaseObjects.Columns.ActualCost)),
                                                   }).ToList();

                                foreach (var obj1 in actualItems)
                                {
                                    summaryRow[DatabaseObjects.Columns.Id] = Convert.ToInt32(UGITUtility.SplitString(obj1.BudgetItem, ";#", 0));
                                    summaryRow[DatabaseObjects.Columns.Actuals] = obj1.actualAmt;
                                    summaryRow["Variance"] = plannedAmt - obj1.actualAmt;
                                }
                            }
                            else
                            {
                                summaryRow[DatabaseObjects.Columns.Id] = 0;
                                summaryRow[DatabaseObjects.Columns.Actuals] = 0.0;
                                summaryRow["Variance"] = plannedAmt;
                            }
                            budgetSummaryTable.Rows.Add(summaryRow);
                        }
                    }
                }
            }
            return budgetSummaryTable;
        }

        private DataTable FilterCategoriesToView()
        {
            DataTable budgetCategory = budgetTable.DefaultView.ToTable(true, DatabaseObjects.Columns.BudgetCategoryName);
            DataTable filteredTable = budgetTable.Clone();

            foreach (DataRow categoryRow in budgetCategory.Rows)
            {
                string category = Convert.ToString(categoryRow[DatabaseObjects.Columns.BudgetCategoryName]);

                DataRow[] subCategories = budgetTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BudgetCategoryName, category));

                if (subCategories.Length > 0)
                {
                    DataTable tempTable = subCategories.CopyToDataTable();

                    foreach (DataRow dr in tempTable.Rows)
                    {
                        string innerFilter = string.Format("{0}='{1}' AND {2}='{3}'",DatabaseObjects.Columns.BudgetCategoryName,category,DatabaseObjects.Columns.BudgetSubCategory, dr[DatabaseObjects.Columns.BudgetSubCategory]);
                        DataRow[] rowColl= authorizedCategories.Select(innerFilter);

                        if (rowColl!=null && rowColl.Length > 0)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(rowColl[0][DatabaseObjects.Columns.AuthorizedToView])) || userProfileManager.IsUserPresentInField(context.CurrentUser, rowColl.FirstOrDefault(), DatabaseObjects.Columns.AuthorizedToView))
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
