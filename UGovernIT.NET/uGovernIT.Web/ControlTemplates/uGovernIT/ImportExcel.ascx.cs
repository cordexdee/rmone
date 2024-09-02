using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class ImportExcel : System.Web.UI.UserControl
    {
        public const String NEW_LIST = "newList";
        public const String OLD_LIST = "oldList";
        public const String APPEND_LIST = "append";
        public const String REPOPULATE_LIST = "rePopulate";
        public const String DELETE_CREATE_LIST = "delete";
        protected String pageName { get; set; }
        protected String idValue { get; set; }
        protected String columnFilter { get; set; }
        ApplicationContext context = null;
        protected String flag;
        BudgetCategoryViewManager budgetCategoryViewManager = null;
        List<BudgetCategory> budgetCategories = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            pageName = Convert.ToString(Request.QueryString["sourcePage"]);
            columnFilter = Convert.ToString(Request.QueryString["columnFilter"]);
            idValue = Convert.ToString(Request.QueryString["idValue"]);

            if (!IsPostBack)
            {
                //operationsDropDown.DataSource = GetAllListName();
                //operationsDropDown.DataBind();
            }
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            ImportFile(sender, e);
        }

        protected void ImportFile(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (FileUploadControl.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || FileUploadControl.PostedFile.ContentType == "application/vnd.ms-excel" || FileUploadControl.PostedFile.ContentType == "text/vnd.ms-excel")
                    {
                        String listName = "";
                        String filename = FileUploadControl.FileName;
                        String tempPath = uHelper.GetTempFolderPath();
                        FileUploadControl.PostedFile.SaveAs(tempPath + filename);
                        listName = Convert.ToString(newListTitle.Value);


                        //string FileName = Path.GetFileName(updExcelFile.UploadedFiles[0].FileName);
                        //string FilePath = string.Format(@"{0}/{1}", uHelper.GetTempFolderPath(), filename);
                        //updExcelFile.UploadedFiles[0].SaveAs(FilePath);

                        if (pageName.Equals("ITGBudgetManagement"))
                        {
                            listName = DatabaseObjects.Tables.ModuleMonthlyBudget;
                            budgetCategoryViewManager = new BudgetCategoryViewManager(context);
                            budgetCategories = budgetCategoryViewManager.Load();
                            DataTable dtExcelSheet = CreateTable();
                            DataRow dr = null;
                            DataTable dt = uHelper.GetDataTableFromExcel(tempPath + filename, "BudgetData");


                            BudgetCategory budgetCategory = null;
                            foreach (DataRow row in dt.Rows)
                            {
                                dr = dtExcelSheet.NewRow();
                                budgetCategory = budgetCategories.FirstOrDefault(x => x.Title == UGITUtility.ObjectToString(row[DatabaseObjects.Columns.BudgetCategoryLookup]));
                                dr[DatabaseObjects.Columns.BudgetCategoryLookup] = budgetCategory.ID;
                                dr[DatabaseObjects.Columns.TicketId] = row[DatabaseObjects.Columns.TicketId];
                                dr[DatabaseObjects.Columns.Title] = row[DatabaseObjects.Columns.Title];
                                dr[DatabaseObjects.Columns.ActualCost] = row[DatabaseObjects.Columns.ActualCost];
                                dr[DatabaseObjects.Columns.NonProjectActualTotal] = row[DatabaseObjects.Columns.NonProjectActualTotal];
                                dr[DatabaseObjects.Columns.NonProjectPlannedTotal] = row[DatabaseObjects.Columns.NonProjectPlannedTotal];
                                dr[DatabaseObjects.Columns.ProjectPlannedTotal] = row[DatabaseObjects.Columns.ProjectPlannedTotal];
                                dr[DatabaseObjects.Columns.ProjectActualTotal] = row[DatabaseObjects.Columns.ProjectActualTotal];
                                dr[DatabaseObjects.Columns.BudgetAmount] = row[DatabaseObjects.Columns.BudgetAmount];
                                dr[DatabaseObjects.Columns.EstimatedCost] = row[DatabaseObjects.Columns.EstimatedCost];
                                dr[DatabaseObjects.Columns.AllocationStartDate] = Convert.ToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]);
                                dr[DatabaseObjects.Columns.ResourceCost] = row[DatabaseObjects.Columns.ResourceCost];
                                dr[DatabaseObjects.Columns.BudgetType] = row[DatabaseObjects.Columns.BudgetType];
                                dr[DatabaseObjects.Columns.ModuleNameLookup] = ModuleNames.ITG;
                                dr[DatabaseObjects.Columns.Created] = DateTime.Now;
                                dr[DatabaseObjects.Columns.Modified] = DateTime.Now;
                                dr[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
                                dr[DatabaseObjects.Columns.TenantID] = context.TenantID;
                                dtExcelSheet.Rows.Add(dr);
                            }
                            dtExcelSheet.AcceptChanges();
                            GetTableDataManager.bulkupload(dtExcelSheet, listName);
                            Label3.Text = "Data has been uploaded successfully !!";
                        }
                        else if (pageName.Equals("PMMBudget"))
                        {
                            listName = DatabaseObjects.Tables.ModuleMonthlyBudget;

                        }
                        else if (Convert.ToString(newListTitle.Value).Equals(String.Empty))
                        {
                            listName = operationsDropDown.SelectedValue;

                        }

                        //flag = CreateSPList(tempPath + filename, operationToBePerformed.Value.ToString(), listName);
                        //if (pageName.Equals("DashBoardFactTables"))
                        //{
                        //    AddListTOFactTable(tempPath + filename, listName);
                        //}

                      
                        //uHelper.ClosePopUpAndEndResponse(Context, false);
                        listName = "";
                    }

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }
        public DataTable CreateTable()
        {
            DataTable result = new DataTable("BudgetData");
            result.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Created, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.Modified, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.CreatedByUser, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TenantID, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ActualCost , typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.NonProjectActualTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.NonProjectPlannedTotal,typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ProjectPlannedTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ProjectActualTotal, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.ResourceCost, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.EstimatedCost, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.BudgetType, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetCategoryLookup, typeof(Int64));
            return result;
        }
        //protected void AddListTOFactTable(String filePath, String listName)
        //{
        //    try
        //    {
        //        bool isListNew = false;
        //        ExcelToSPList importObject = new ExcelToSPList();
        //        SPWeb currentWeb = SPContext.Current.Web;
        //        importObject.filePath = filePath;
        //        isListNew = getListStatus(listName);
        //        if (isListNew)
        //        {
        //            SPListItemCollection listItems = currentWeb.Lists[DatabaseObjects.Tables.DashboardFactTables].Items;
        //            SPListItem item = listItems.Add();
        //            item["Title"] = listName;
        //            item["CacheAfter"] = "0";       // Minutes
        //            item["CacheThreshold"] = "0";
        //            item["ExpiryDate"] = DateTime.Now.ToString("MM/dd/yyyy");
        //            item["CacheTable"] = "1";   // Boolean
        //            item["CacheMode"] = "On-Demand";    // On-Demand or Scheduled
        //            item["RefreshMode"] = "All";  // All or ChangesOnly
        //            item.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex);
        //    }
        //}

        ///// <summary>
        ///// This method checks if the list is present in Dashboard Fact tables or not.
        ///// </summary>
        ///// <param name="listName"></param>
        ///// <returns></returns>
        //protected bool getListStatus(string listName)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        SPWeb currentWeb = SPContext.Current.Web;
        //        SPListItemCollection dashboardListItems = currentWeb.Lists[DatabaseObjects.Lists.DashboardFactTables].Items;

        //        if (dashboardListItems != null)
        //        {
        //            foreach (SPListItem item in dashboardListItems)
        //            {
        //                if (item["Title"].ToString().Equals(listName))
        //                {
        //                    flag = false;
        //                    break;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex, "getListStatus");
        //    }
        //    return flag;
        //}

        //protected string CreateSPList(String filePath, String operation, String listName)
        //{
        //    ExcelToSPList importObject = new ExcelToSPList();
        //    importObject.filePath = filePath;
        //    importObject.newListName = listName;
        //    try
        //    {
        //        switch (operation)
        //        {
        //            case NEW_LIST:
        //                importObject.GetAllSPLists();

        //                break;

        //            case OLD_LIST:

        //                break;

        //            case APPEND_LIST:
        //                importObject.AppendDataToSPList(listName);
        //                break;

        //            case REPOPULATE_LIST:
        //                if (pageName.Equals("PMMBudget"))
        //                    importObject.RepopulateSPList(listName, columnFilter, idValue);
        //                else
        //                    importObject.RepopulateSPList(listName);

        //                break;

        //            case DELETE_CREATE_LIST:
        //                importObject.GetAllSPLists();
        //                break;

        //            default:
        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex, "CreateSPList");
        //    }

        //    return importObject.status;
        //}

        //protected List<string> GetAllListName()
        //{
        //    List<string> listNames = new List<string>();
        //    try
        //    {

        //        SPWeb currentWeb = SPContext.Current.Web;

        //        foreach (SPList list in currentWeb.Lists)
        //        {
        //            listNames.Add(list.Title);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex, "GetAllListName");
        //    }
        //    return listNames;
        //}

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            ImportFile(sender, e);

        }
    }
}