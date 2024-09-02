using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using static uGovernIT.Web.Controllers.RunInBackgroundServicesController;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Manager.Core;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.DAL;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public class ImportStatus
    {
        public string moduleName;

        public bool succeeded; // true = import succeeded, else failed
        public List<string> errorMessages = new List<string>();

        public int numErrors;
        public int recordsProcessed;
        public int recordsSkipped;
        public int recordsAdded;
        public int recordsUpdated;
        public int totalRecords;
        DataTable table = new DataTable();
        public ImportStatus(string moduleName)
        {
            this.moduleName = moduleName;
        }
    }

    public class ModuleExcelImport
    {
        public static List<ImportStatus> runningImports = new List<ImportStatus>();

        public bool ModuleImportRunning(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return false;

            //return false;
            return (runningImports.Count > 0 && runningImports.Exists(x => !string.IsNullOrEmpty(x.moduleName) && x.moduleName == moduleName));
        }


        public int ModuleImportPercentageComplete(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return -1;

            if (runningImports.Count > 0 && runningImports.Exists(x => !string.IsNullOrEmpty(x.moduleName) && x.moduleName == moduleName))
            {
                ImportStatus currentImportStatus = runningImports.FirstOrDefault(x => x.moduleName == moduleName);

                if (currentImportStatus.recordsProcessed > 0 && currentImportStatus.totalRecords > 0)
                {
                    if (currentImportStatus.recordsProcessed == currentImportStatus.totalRecords)
                        return 99;  // final processing happening, so don't show 100%
                    else
                        return currentImportStatus.recordsProcessed * 100 / currentImportStatus.totalRecords;
                }
                else
                    return 0;
            }
            else
                return -1;

        }
    }

    public partial class ImportExcelFile : UserControl
    {
        bool isSiteAdmin = false;
        bool isCheckMandatoryField = true;
        string mandatoryField = string.Empty;
        private string Module { get; set; }
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        //BudgetCategoryViewManager objBudgetCategoryViewManager = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        CompanyManager CompanyMGR = new CompanyManager(HttpContext.Current.GetManagerContext());
        //DepartmentManager DepartmentMGR = new DepartmentManager(HttpContext.Current.GetManagerContext());
        //GlobalRoleManager GlobalRoleMGR = new GlobalRoleManager(HttpContext.Current.GetManagerContext());
        //JobTitleManager JobTitleMGR = new JobTitleManager(HttpContext.Current.GetManagerContext());
        
        UserProfile User;
        //RequestTypeManager objRequestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
        ApplicationContext AppContextForThread;
        UserProfileManager UserManager;
        private List<LifeCycle> objLifeCycle;
        List<UGITModule> moduleTable = new List<UGITModule>();
        List<UGITModule> listModule = null;
        DataTable olddt = new DataTable();

        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            isSiteAdmin = HttpContext.Current.GetUserManager().IsAdmin(HttpContext.Current.CurrentUser());
            AppContextForThread = HttpContext.Current.GetManagerContext();
            UserManager = HttpContext.Current.GetUserManager();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Module"] != null)
                Module = Request["Module"];

            if (Request["listName"] == "ActualTime")
            {
                ModuleExcelImport excelImport = new ModuleExcelImport();
                string listName = UGITUtility.ObjectToString(Request["listName"]);
                if (excelImport.ModuleImportRunning(listName))
                {
                    lblMessage.Text = "Import already running";
                    lblMessage.Visible = true;
                    Util.Log.ULog.WriteLog(Convert.ToString(lblMessage.Text));
                    btnImport.Enabled=false;
                    return;
                }
            }
            if (Request["listName"] == "MasterImport")
            {
                lnkMasterImport.Visible = true;
                chkDeleteExistingRecord.Visible = false;
                chkRuninBackground.Checked = false;
                chkRuninBackground.Visible = false;
            }
            else
            {
                btnImport.Visible = true;
                if (Request["listName"] == "UserInformationList")
                {
                    if ((ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateNewUser)) && (ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateFBAUser)))
                    {
                        trUserType.Visible = true;
                    }

                    chkDeleteExistingRecord.Visible = false;
                }

                if (Request["listName"] == "PMM")
                {
                    Module = "PMM";
                }

                if (Request["listName"] == "ProjectAllocations" || Request["listName"] == "TimeOffAllocations" || Request["listName"] == "Roles" || Request["listName"] == "JobTitle")
                {
                    chkRuninBackground.Visible = false;
                }
                if (Request["listName"] == "Roles" || Request["listName"] == "JobTitle")
                {
                    chkDeleteExistingRecord.Visible = true;
                }
            }
        }

        private void AssetImportInBackground(ApplicationContext spWeb, string userId, string filePath, bool deleteExistingRecords, ImportStatus status)
        {
            UserProfile user = UserManager.GetUserById(userId);
            string errorMessage = string.Empty;

            try
            {
                AssetImport(spWeb, user, filePath, deleteExistingRecords, status);
                if (status.errorMessages.Count > 0)
                    errorMessage = string.Join<string>("<br />", status.errorMessages);
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("ERROR Importing Assets from file {0}", Path.GetFileName(filePath));
                errorMessage = ex.Message;
            }

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                MailMessenger mail = new MailMessenger(AppContextForThread);

                StringBuilder body = new StringBuilder();
                string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                string signature = ObjConfigurationVariableManager.GetValue("Signature");
                body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);

                body.AppendFormat("The asset import process {0}", status.succeeded ? "completed successfully." : "FAILED!");

                body.AppendFormat("<br /><br />");
                if (status.recordsAdded > 0)
                    body.AppendFormat("{0} asset(s) added<br />", status.recordsAdded);
                if (status.recordsUpdated > 0)
                    body.AppendFormat("{0} asset(s) updated<br />", status.recordsUpdated);
                if (status.recordsSkipped > 0)
                    body.AppendFormat("{0} asset(s) skipped due to errors<br />", status.recordsSkipped);
                if (status.recordsProcessed > 0)
                    body.AppendFormat("______________________________________________<br />");
                body.AppendFormat("{0} total asset(s) processed<br />", status.recordsProcessed);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    body.AppendFormat("<br /><br /><b>ERRORS:</b><br />{0}", errorMessage);

                body.AppendFormat("<br /><br />{0}<br />", signature);

                mail.SendMail(user.Email, status.succeeded ? "Asset Import Complete" : "Asset Import FAILED", string.Empty, body.ToString(), true);
            }
        }

        private void AssetImport(ApplicationContext spWeb, UserProfile user, string filePath, bool deleteExistingRecords, ImportStatus status)
        {
            string errorMessage = string.Empty;
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filePath);

            CellRange range = workbook.Worksheets[0].GetDataRange();
            DataTable data = workbook.Worksheets[0].CreateDataTable(range, true);
            if (data == null)
            {
                errorMessage = "No data found in file!";
                //Log.WriteLog(errorMessage);
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }

            string workSheetName = workbook.Worksheets[0].Name;
            List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
            string listName = DatabaseObjects.Tables.Assets;
            string uniqueFieldName = ObjConfigurationVariableManager.GetValue(ConfigConstants.AssetUniqueTag); // Internal name of unique field
            if (string.IsNullOrWhiteSpace(uniqueFieldName))
                uniqueFieldName = DatabaseObjects.Columns.AssetTagNum;
            List<FieldAliasCollection> listFields = templstColumn.Where(r => r.ListName == listName).ToList();
            DataTable spList = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID} = '{spWeb.TenantID}'");
            List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            Ticket ticket = new Ticket(spWeb, Module);

            // Mandatory checks
            string mandatoryColumnName = string.Empty; // Excel column name of unique mandatory field
            List<FieldAliasCollection> mandatoryFields = new List<FieldAliasCollection>();
            mandatoryFields = listFields.Where(r => r.InternalName == uniqueFieldName).ToList();
            bool checkMandatory = false;
            if (mandatoryFields.Count > 0)
            {
                string[] aliasName = mandatoryFields[0].AliasNames.Split(',');
                foreach (string alias in aliasName)
                {
                    if (lstColumn.Contains(alias))
                    {
                        checkMandatory = true;
                        mandatoryColumnName = alias;
                        break;
                    }
                }
            }

            if (!checkMandatory)
            {
                //Log.WriteLog(string.Format("{0} is mandatory in excel for {1}", uniqueFieldName, listName));
                errorMessage = string.Format("{0} is mandatory in excel", UGITUtility.AddSpaceBeforeWord(uniqueFieldName));
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }

            // Delete the record from the list...if check box is check.
            if (deleteExistingRecords)
            {
                // Log.WriteUGITLog(spWeb, "Deleting existing assets!", UGITLogSeverity.Warning, UGITLogCategory.Import);


                DataRow[] deleteColl = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID}='{spWeb.TenantID}'").Select();
                if (deleteColl != null && deleteColl.Count() > 0)
                {
                    //SPListHelper.DeleteBatch(deleteColl.CopyToDataTable(), deleteColl);
                    //SpDelta 42
                    ////TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                    TicketManager ticketManager = new TicketManager(AppContextForThread);
                    //
                    foreach (DataRow deleteRow in deleteColl)
                    {
                        UGITModule module = ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(Convert.ToString(deleteRow[DatabaseObjects.Columns.ID])));
                        ticketManager.Delete(module, deleteRow);
                    }
                }
            }

            List<string> error = new List<string>();
            List<string> alreadyexist = new List<string>();
            int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;

            status.totalRecords = lastIndex;

            DataColumn assetTagfield = null;

            if (spList.Columns.Contains(uniqueFieldName))
                assetTagfield = spList.Columns[uniqueFieldName];

            int assetTagIndex = -1;
            if (assetTagfield != null)
                assetTagIndex = lstColumn.FindIndex(s => s.Equals(mandatoryColumnName, StringComparison.OrdinalIgnoreCase));

            Hashtable existingAssets = new Hashtable();
            if (assetTagIndex != -1)
            {
                //string query = new SPQuery();
                //query.Query = string.Format("<Where></Where>");
                //query.ViewFieldsOnly = true;
                //query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId, assetTagfield.InternalName);
                DataRow[] assetCollection = spList.Select();
                if (assetCollection != null && assetCollection.Count() > 0)
                {
                    foreach (DataRow item in assetCollection)
                    {
                        string assetTag = Convert.ToString(item[assetTagfield.ColumnName]);
                        if (!existingAssets.ContainsKey(assetTag))
                        {
                            existingAssets.Add(assetTag, Convert.ToString(item[DatabaseObjects.Columns.Id]));
                        }
                        //else
                        //Log.WriteLog("ERROR: Found duplicate asset tag in existing assets - " + assetTag);
                    }
                    //Log.WriteLog(string.Format("Loaded list of {0} existing assets", existingAssets.Count));
                }
            }
            else
            {
                // SHOULD NOT HAPPEN since we are already checking for mandatory column further up!
                //Log.WriteLog(string.Format("{0} is mandatory in excel for {1}", uniqueFieldName, listName));
                errorMessage = string.Format("{0} is mandatory in excel", UGITUtility.AddSpaceBeforeWord(uniqueFieldName));
                status.succeeded = false;
                status.errorMessages.Add(errorMessage);
                status.recordsProcessed = 0;
                return;
            }

            List<string> updatedTickets = new List<string>();
            Dictionary<string, DataTable> lookupLists = GetLookupLists(spWeb, ModuleNames.CMDB);
            Hashtable importedAssets = new Hashtable();
            for (int i = 1; i <= lastIndex; i++)
            {
                status.recordsProcessed++;

                DataRow listItem = null;
                Row row = workbook.Worksheets[0].Rows[i];
                string assetTag = row[assetTagIndex].DisplayText;
                if (string.IsNullOrWhiteSpace(assetTag))
                {
                    errorMessage = string.Format("Line # {0}: Ignoring record with missing {1}", i + 1, assetTagfield.ColumnName);
                    //Log.WriteLog(errorMessage);
                    status.errorMessages.Add(errorMessage);
                    status.recordsSkipped++;
                    continue;
                }

                bool isExistingAsset = false;
                if (importedAssets.ContainsKey(assetTag))
                {
                    errorMessage = string.Format("Line # {0}: Skipping duplicate asset tag {1}", i + 1, assetTag);
                    //Log.WriteLog(errorMessage);
                    status.errorMessages.Add(errorMessage);
                    status.recordsSkipped++;
                    continue;
                }
                else if (existingAssets.ContainsKey(assetTag))
                {
                    int assetID = UGITUtility.StringToInt(existingAssets[assetTag]);
                    listItem = spList.Select("ID=" + assetID)[0];
                    if (listItem == null)
                    {
                        errorMessage = string.Format("ERROR retrieving existing asset with tag # {0}, ID {1}", assetTag, assetID);
                        //Log.WriteLog(errorMessage);
                        status.errorMessages.Add(errorMessage);
                        status.recordsSkipped++;
                        continue;
                    }
                    //Log.WriteLog(string.Format("Updating asset with tag # {0}, ID {1}", assetTag, assetID));
                    isExistingAsset = true;
                }
                else
                {
                    listItem = spList.NewRow();
                    //Log.WriteLog("Importing new asset with tag # " + assetTag);
                }

                List<TicketColumnValue> formValues = new List<TicketColumnValue>();

                uHelper.SetFilledValues(AppContextForThread, lstColumn, row, listItem, listFields, formValues, lookupLists, moduleName: ModuleNames.CMDB);

                //ticket.SetItemValues(listItem, formValues, true, false, User.Id);
                if (UGITUtility.StringToInt(listItem["ID"]) == 0)
                    ticket.Create(listItem, User);

                LifeCycle defaultLifeCycle = ticket.Module.List_LifeCycles.FirstOrDefault();


                if (defaultLifeCycle != null)
                {
                    LifeCycleStage currentStage = null;
                    TicketColumnValue assetStage = null;
                    if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus))
                    {
                        assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus);
                        if (assetStage != null)
                        {
                            currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.Name == Convert.ToString(assetStage.Value));
                            listItem[assetStage.InternalFieldName] = assetStage.Value;
                        }
                    }
                    else if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep))
                    {
                        assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep);
                        if (assetStage != null)
                        {
                            currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(assetStage.Value));
                            listItem[assetStage.InternalFieldName] = assetStage.Value;
                        }
                    }

                    if (currentStage != null)
                    {
                        if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.StageStep))
                        {
                            listItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
                        }
                    }

                    //if (currentStage != null)
                    //    Ticket.SetStageSpecificFields(listItem, currentStage);
                    //else if (!isExistingAsset)
                    //    ticket.QuickClose(spWeb, ticket.Module.ID, listItem, string.Empty);
                }

                string innererror = ticket.CommitChanges(listItem, donotUpdateEscalations: true, stopUpdateDependencies: true);
                if (!string.IsNullOrEmpty(innererror))
                {
                    error.Add(innererror);
                    status.recordsSkipped++;
                }
                else if (isExistingAsset)
                    status.recordsUpdated++;
                else
                    status.recordsAdded++;

                importedAssets.Add(assetTag, listItem["ID"]);
                string ticketID = Convert.ToString(listItem[DatabaseObjects.Columns.TicketId]);
                if (!string.IsNullOrEmpty(ticketID))
                    updatedTickets.Add(Convert.ToString(listItem[DatabaseObjects.Columns.TicketId]));
            }

            string message = string.Format("Asset Import from {0} successful: {1} new assets imported, {2} updated, {3} skipped, {4} processed",
                                            Path.GetFileName(filePath), status.recordsAdded, status.recordsUpdated, status.recordsSkipped, status.recordsProcessed);
            Util.Log.ULog.WriteUGITLog(spWeb.CurrentUser.Id, message, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), spWeb.TenantID);

            // Probably not needed - anyway causes issue when thousands of tickets loaded at once!
            //if (SPContext.Current == null && updatedTickets.Count > 0)
            //    SPListHelper.ReloadTicketsInCache(updatedTickets, spWeb);

            if (status.recordsAdded > 0 || status.recordsUpdated > 0)
                status.succeeded = true;
            else
                status.succeeded = false;

            return;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                
                string listName = string.Empty;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                bool deleteExistingRecords = chkDeleteExistingRecord.Checked;
                if (Request["listName"] != null)
                    listName = Request["listName"];
                if (Request["listName"] != null && Request["listName"] == "PMM")
                {
                    listName = string.Empty;
                }
                if (Request["listName"] != null && Request["listName"] == "RequestType")
                {
                    listName = DatabaseObjects.Tables.RequestType;
                }
                string errorMessage = null;

                if (string.IsNullOrEmpty(listName) && !string.IsNullOrEmpty(this.Module))
                {
                    ImportModuleBasedTickets(this.Module, deleteExistingRecords, ref errorMessage);

                    return;
                }
                if(listName == "Projects")
                {
                    ImportProjects(deleteExistingRecords, ref errorMessage);
                    return;
                }
               
                
                if (flpImport.HasFile)
                {
                    Tuple<string, int, int> message = null;
                    string FileName = Path.GetFileName(flpImport.PostedFile.FileName);
                    string Extension = Path.GetExtension(flpImport.PostedFile.FileName);
                    string FolderPath = flpImport.PostedFile.FileName;

                    if (Extension.ToLower() != ".xlsx" && Extension.ToLower() != ".xls" && Extension.ToLower() != ".csv")
                    {
                        lblMessage.Text = "Only excel format import is supported (.xlsx)";
                        lblMessage.Visible = true;
                        return;
                    }

                    string FilePath = string.Format(@"{0}/{1}", uHelper.GetTempFolderPath(), FileName);
                    flpImport.SaveAs(FilePath);

                    //Log.WriteUGITLog(SPContext.Current.Web, "Starting import from file: " + FileName, UGITLogSeverity.Information, UGITLogCategory.Import);

                    ASPxSpreadsheet1.Document.LoadDocument(FilePath);

                    var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
                    Workbook wb = new Workbook();
                    wb.LoadDocument(FilePath);
                    worksheet.CopyFrom(wb.Worksheets[0]);

                    int colCount = worksheet.Columns.LastUsedIndex;
                    int rowCount = worksheet.Rows.LastUsedIndex;

                    RowCollection rows = worksheet.Rows;
                    Row row = rows[0];

                    //get the columns of excel...

                    List<string> lstColumn = new List<string>();
                    for (int i = 0; i <= colCount; i++)
                    {
                        lstColumn.Add((row.Worksheet.Cells[i].DisplayText).TrimEnd('\n', '\r').Trim());
                    }
                    List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                    List<FieldAliasCollection> objFieldAliasCollection = new List<FieldAliasCollection>();
                    List<string> error = new List<string>();
                    List<string> alreadyexist = new List<string>();
                    Tuple<string, int, int, int> errorObj = new Tuple<string, int, int, int>(string.Empty, 0, 0, 0);
                    string userID = User.Id;
                    if (listName == "UserInformationList")
                    {
                        List<FieldAliasCollection> objFieldAliasCollectionUser = new List<FieldAliasCollection>();
                        objFieldAliasCollectionUser = templstColumn.Where(r => r.ListName == "UserInformationList").ToList();
                        if (chkRuninBackground.Checked)
                        {
                            HttpContext httpContext = Context;
                            ApplicationContext context = HttpContext.Current.GetManagerContext();
                            ThreadStart starter = delegate { ImportUserHelper.ImportUsersElevated(context, User, FilePath, objFieldAliasCollectionUser, userID, ddlUserType.SelectedIndex, FileName); };
                            Thread thread = new Thread(starter);
                            thread.IsBackground = true;
                            thread.Start();
                            uHelper.ClosePopUpAndEndResponse(Context, true);
                        }
                        else
                        {
                            ApplicationContext context = HttpContext.Current.GetManagerContext();
                            errorObj = ImportUserHelper.ImportUsersElevated(context, User, FilePath, objFieldAliasCollectionUser, userID, ddlUserType.SelectedIndex, FileName);
                            StringBuilder seletedParams = new StringBuilder();
                            seletedParams.AppendFormat("ugroup={0}", Request["ugroup"]);
                            seletedParams.AppendFormat("&upage={0}", Request["upage"]);
                            //Context.Cache.Add(string.Format("EditUserInfo-{0}", SPContext.Current.Web.CurrentUser.ID), seletedParams.ToString(), null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        }
                    }
                    else if (listName.EqualsIgnoreCase("ProjectAllocations"))
                    {
                        MailMessenger mail = new MailMessenger(AppContextForThread);
                        string summary = string.Empty;
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        deleteExistingRecords = false;
                        if (Convert.ToString(Request["action"]).EqualsIgnoreCase("newallocations"))
                        {
                            deleteExistingRecords = true;
                        }

                        ImportProjectAllocationHelper.ImportProjectAllocations(AppContextForThread, listName, FilePath, deleteExistingRecords, ref errorMessage, ref summary);

                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"\n<b>{listName} Summary:</b><br />{summary.Replace("\n", "<br>").Replace("\t", "&nbsp;")}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Completed for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = "Import Completed";

                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = errorMessage;
                        lblMessage.Visible = true;

                        //txtSummary.Text = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        dvSummary.InnerHtml = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        SummaryPopup.ShowOnPageLoad = true;

                        if (!string.IsNullOrEmpty(AppContextForThread.CurrentUser.Email))
                        {
                            UserProfile user = AppContextForThread.CurrentUser;
                            StringBuilder body = new StringBuilder();
                            string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                            string signature = ObjConfigurationVariableManager.GetValue("Signature");
                            body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                            body.AppendFormat("Project Allocations Summary:");
                            body.AppendFormat("<br /><br />");
                            body.Append(summary.Replace("\n", "<br>").Replace("\t", "&nbsp;"));

                            body.AppendFormat("<br /><br />{0}<br />", signature);
                            mail.SendMail(user.Email, !string.IsNullOrEmpty(errorMessage) ? "Import Completed" : "Import FAILED", string.Empty, body.ToString(), true);
                        }
                        return;
                    }
                    else if (listName.EqualsIgnoreCase("TimeOffAllocations"))
                    {
                        MailMessenger mail = new MailMessenger(AppContextForThread);
                        string summary = string.Empty;
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        deleteExistingRecords = false;
                        //if (Convert.ToString(Request["action"]).EqualsIgnoreCase("newallocations"))
                        //{
                        //    deleteExistingRecords = true;
                        //}

                        ImportTimeOffAllocationHelper.ImportTimeOffAllocations(AppContextForThread, listName, FilePath, deleteExistingRecords, ref errorMessage, ref summary);

                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"\n<b>{listName} Summary:</b><br />{summary.Replace("\n", "<br>").Replace("\t", "&nbsp;")}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Completed for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = "Import Completed";

                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = errorMessage;
                        lblMessage.Visible = true;

                        //txtSummary.Text = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        dvSummary.InnerHtml = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        SummaryPopup.ShowOnPageLoad = true;

                        if (!string.IsNullOrEmpty(AppContextForThread.CurrentUser.Email))
                        {
                            UserProfile user = AppContextForThread.CurrentUser;
                            StringBuilder body = new StringBuilder();
                            string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                            string signature = ObjConfigurationVariableManager.GetValue("Signature");
                            body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                            body.AppendFormat("TimeOff Allocations Summary:");
                            body.AppendFormat("<br /><br />");
                            body.Append(summary.Replace("\n", "<br>").Replace("\t", "&nbsp;"));

                            body.AppendFormat("<br /><br />{0}<br />", signature);
                            mail.SendMail(user.Email, !string.IsNullOrEmpty(errorMessage) ? "Import Completed" : "Import FAILED", string.Empty, body.ToString(), true);
                        }
                        return;
                    }
                    else if (listName.EqualsIgnoreCase("ActualTime"))
                    {
                        
                        string summary = string.Empty;
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}. <b>File Name:</b> {System.IO.Path.GetFileName(FilePath)}\n", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        deleteExistingRecords = false;

                        ImportStatus status = new ImportStatus("ActualTime");
                        if (chkRuninBackground.Checked)
                        {
                            lblMessage.Text = "File upload process has started in the background. It may take some time to complete.";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Visible = true;
                            try
                            {
                                ModuleExcelImport.runningImports.Add(status);
                                DateTime aDate = DateTime.Now;
                                RunInBackgroundImportStatus backgroundImportStatus = new RunInBackgroundImportStatus("ActualTime", User.Name, "Running", aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"), string.Empty, DateTime.Now.Date);
                                RunInBackgroundModuleExcelImport.runningImports.Add(backgroundImportStatus);
                                ThreadStart starter = delegate
                                {
                                    ImportActualTimeHelper.ImportActualTime(AppContextForThread, listName, FilePath, deleteExistingRecords, ref errorMessage, ref summary);
                                    RunInBackgroundModuleExcelImport.runningImports.Remove(backgroundImportStatus);
                                    ModuleExcelImport.runningImports.Remove(status);
                                    sendsummary(AppContextForThread, summary, listName, errorMessage);
                                };
                                Thread thread = new Thread(starter);
                                thread.IsBackground = true;
                                thread.Start();

                            }
                            catch (Exception ex)
                            {
                                ModuleExcelImport.runningImports.Remove(status);
                                ULog.WriteException(ex);
                            }
                        }
                        else
                        {
                            ImportActualTimeHelper.ImportActualTime(AppContextForThread, listName, FilePath, deleteExistingRecords, ref errorMessage, ref summary);
                            sendsummary(AppContextForThread, summary, listName, errorMessage);
                        }
                        
                        return;
                    }
                     else if (listName.EqualsIgnoreCase("Roles"))
                    {

                        MailMessenger mail = new MailMessenger(AppContextForThread);
                        string summary = string.Empty;
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        ImportUserRolesHelper.ImportUserRoles(AppContextForThread, listName, FilePath, chkDeleteExistingRecord.Checked, ref errorMessage, ref summary);

                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"\n<b>{listName} Summary:</b><br />{summary.Replace("\n", "<br>").Replace("\t", "&nbsp;")}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Completed for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = "Import Completed";

                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = errorMessage;
                        lblMessage.Visible = true;

                        //txtSummary.Text = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        dvSummary.InnerHtml = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        if (!string.IsNullOrWhiteSpace(dvSummary.InnerHtml))
                            SummaryPopup.ShowOnPageLoad = true;

                        if (!string.IsNullOrEmpty(AppContextForThread.CurrentUser.Email))
                        {
                            UserProfile user = AppContextForThread.CurrentUser;
                            StringBuilder body = new StringBuilder();
                            string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                            string signature = ObjConfigurationVariableManager.GetValue("Signature");
                            body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                            body.AppendFormat("Import User Roles Summary:");
                            body.AppendFormat("<br /><br />");
                            body.Append(summary.Replace("\n", "<br>").Replace("\t", "&nbsp;"));

                            body.AppendFormat("<br /><br />{0}<br />", signature);
                            mail.SendMail(user.Email, !string.IsNullOrEmpty(errorMessage) ? "Import Completed" : "Import FAILED", string.Empty, body.ToString(), true);
                        }
                        return;
                    }
                    else if (listName.EqualsIgnoreCase("JobTitle"))
                    {

                        MailMessenger mail = new MailMessenger(AppContextForThread);
                        string summary = string.Empty;
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        ImportJobTitlesHelper.ImportJobTitles(AppContextForThread, listName, FilePath, chkDeleteExistingRecord.Checked, ref errorMessage, ref summary);

                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"\n<b>{listName} Summary:</b><br />{summary.Replace("\n", "<br>").Replace("\t", "&nbsp;")}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                        ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Completed for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = "Import Completed";

                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = errorMessage;
                        lblMessage.Visible = true;

                        //txtSummary.Text = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        dvSummary.InnerHtml = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                        if (!string.IsNullOrWhiteSpace(dvSummary.InnerHtml))
                            SummaryPopup.ShowOnPageLoad = true;

                        if (!string.IsNullOrEmpty(AppContextForThread.CurrentUser.Email))
                        {
                            UserProfile user = AppContextForThread.CurrentUser;
                            StringBuilder body = new StringBuilder();
                            string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                            string signature = ObjConfigurationVariableManager.GetValue("Signature");
                            body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                            body.AppendFormat("Import Job Title Summary:");
                            body.AppendFormat("<br /><br />");
                            body.Append(summary.Replace("\n", "<br>").Replace("\t", "&nbsp;"));

                            body.AppendFormat("<br /><br />{0}<br />", signature);
                            mail.SendMail(user.Email, !string.IsNullOrEmpty(errorMessage) ? "Import Completed" : "Import FAILED", string.Empty, body.ToString(), true);
                        }
                        return;
                    }
                    else
                    {
                        #region import logic

                        if (chkRuninBackground.Checked)
                        {
                            ThreadStart starter = delegate
                            {
                                ImportNonModuleItemInBackground(AppContextForThread, userID, FilePath, listName, deleteExistingRecords);
                            };

                            Thread thread = new Thread(starter);
                            thread.IsBackground = true;
                            thread.Start();

                        }
                        else
                        {
                            message = ImportNonModuleItem(AppContextForThread, User, FilePath, listName, deleteExistingRecords);
                            if (!string.IsNullOrWhiteSpace(message.Item1))
                            {
                                lblMessage.Text = message.Item1.ToString();
                                lblMessage.Visible = true;
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                                return;
                            }
                        }

                        #endregion
                    }

                    if (!string.IsNullOrEmpty(errorObj.Item1))
                    {
                        lblMessage.Text = errorObj.Item1;
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblMessage.Text = "excel file required.";
                    lblMessage.Visible = true;
                }

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Import ERROR");
                lblMessage.Text = string.Format("Unable to import-{0}", ex.Message);
                lblMessage.Visible = true;
            }
        }
        private void sendsummary( ApplicationContext AppContextForThread, string summary, string listName, string errorMessage)
        {
            if (!string.IsNullOrEmpty(summary))
            {
                MailMessenger mail = new MailMessenger(AppContextForThread);
                ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"\n<b>{listName} Summary:</b><br />{summary.Replace("\n", "<br>").Replace("\t", "&nbsp;")}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Completed for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);
                if (string.IsNullOrEmpty(errorMessage))
                    errorMessage = "Import Completed";

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;

                //txtSummary.Text = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                dvSummary.InnerHtml = summary.Replace("\n", "<br>").Replace("\t", "&nbsp;");
                SummaryPopup.ShowOnPageLoad = true;

                if (!string.IsNullOrEmpty(AppContextForThread.CurrentUser.Email))
                {
                    UserProfile user = AppContextForThread.CurrentUser;
                    StringBuilder body = new StringBuilder();
                    string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                    string signature = ObjConfigurationVariableManager.GetValue("Signature");
                    body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                    body.AppendFormat("TimeOff Allocations Summary:");
                    body.AppendFormat("<br /><br />");
                    body.Append(summary.Replace("\n", "<br>").Replace("\t", "&nbsp;"));

                    body.AppendFormat("<br /><br />{0}<br />", signature);
                    mail.SendMail(user.Email, !string.IsNullOrEmpty(errorMessage) ? "Import Completed" : "Import FAILED", string.Empty, body.ToString(), true);
                }

            }
        }
        private void ImportProjects(bool deleteExistingRecords, ref string errorMessage)
        {
            ModuleExcelImport excelImport = new ModuleExcelImport();
            if (excelImport.ModuleImportRunning("GenericTickets"))
            {
                errorMessage = "Import already running";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }

            if (!flpImport.HasFile)
            {
                errorMessage = "Excel file required.";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }
            string FileName = Path.GetFileName(flpImport.PostedFile.FileName);
            string Extension = Path.GetExtension(flpImport.PostedFile.FileName);
            string FolderPath = flpImport.PostedFile.FileName;

            if (Extension.ToLower() != ".xlsx" && Extension.ToLower() != ".csv")
            {
                errorMessage = "Only excel and csv format import is supported (.xlsx, .csv)";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }

            string filePath = string.Format(@"{0}\{1}", uHelper.GetTempFolderPath(), FileName);
            flpImport.SaveAs(filePath);
            string errMsg = errorMessage;


            ImportStatus status = new ImportStatus("GenericTickets");
            if (chkRuninBackground.Checked)
            {
                try
                {
                    ModuleExcelImport.runningImports.Add(status);
                    DateTime aDate = DateTime.Now;
                    RunInBackgroundImportStatus backgroundImportStatus = new RunInBackgroundImportStatus("GenericTickets", User.Name, "Running", aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"), string.Empty, DateTime.Now.Date);

                    RunInBackgroundModuleExcelImport.runningImports.Add(backgroundImportStatus);
                    ThreadStart starter = delegate
                    {
                        ImportProjectsFromFile(ref errMsg, filePath);
                        RunInBackgroundModuleExcelImport.runningImports.Remove(backgroundImportStatus);
                        ModuleExcelImport.runningImports.Remove(status);
                    };

                    Thread thread = new Thread(starter);
                    thread.IsBackground = true;
                    thread.Start();
                }catch(Exception ex)
                {
                    ModuleExcelImport.runningImports.Remove(status);
                    ULog.WriteException(ex);
                }

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else
            {
                ImportProjectsFromFile(ref errMsg, filePath);

                ModuleExcelImport.runningImports.Remove(status);

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = errMsg; // string.Format("Import from {0} successful: {1} new records imported and {2} records skipped", Path.GetFileName(filePath), status.recordsAdded, status.recordsSkipped);
                lblMessage.Visible = true;
                
            }
        }

        private void ImportProjectsFromFile(ref string errorMessage, string filePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filePath);
            CellRange range = workbook.Worksheets[0].GetDataRange();
            DataTable data = workbook.Worksheets[0].CreateDataTable(range, true);

            int numAdded = 0, numUpdated = 0, numSkipped = 0;// counters used to store no of records updated, added or skipped
            List<string> lstSkippedTitles = new List<string>();

            //Validate cell value types. If cell value types in a column are different, the column values are exported as text.
            for (int col = 0; col < range.ColumnCount; col++)
            {
                CellValueType cellType = range[0, col].Value.Type;
                for (int r = 1; r < range.RowCount; r++)
                {
                    if (cellType != range[r, col].Value.Type)
                    {
                        data.Columns[col].DataType = typeof(string);
                        break;
                    }
                }
            }

            // Create the exporter that obtains data from the specified range, 
            // skips the header row (if required) and populates the previously created data table. 
            DataTableExporter exporter = workbook.Worksheets[0].CreateDataTableExporter(range, data, true);
            // Handle value conversion errors.
            exporter.CellValueConversionError += exporter_CellValueConversionError;

            // Perform the export.
            exporter.Export();
            void exporter_CellValueConversionError(object sender, CellValueConversionErrorEventArgs e)
            {
                //MessageBox.Show("Error in cell " + e.Cell.GetReferenceA1());
                e.DataTableValue = null;
                e.Action = DataTableExporterAction.Continue;
            }

            Dictionary<string, DataRow[]> modulePairs = new Dictionary<string, DataRow[]>();
            if (data == null)
            {
                errorMessage = "No data found in file!";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }
            else
            {
                //modulePairs.Add("PMM", data.Select($"Type='Project' "));
                modulePairs.Add("CPR", data.Select($"Type='Project' "));
                modulePairs.Add("OPM", data.Select($"Type='Opportunity' "));
                modulePairs.Add("CNS", data.Select($"Type='Service' "));
            }

            ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, "Import Initiated for Projects", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

            foreach (KeyValuePair<string, DataRow[]> pair in modulePairs)
            {
                string moduleName = pair.Key;
                ImportStatus status = new ImportStatus(moduleName);
                UGITModule moduleObj = ObjModuleViewManager.LoadByName(moduleName);
                if (moduleObj == null)
                    continue;
                List<ModuleRoleWriteAccess> ColumnsList = moduleObj.List_RoleWriteAccess;
                LifeCycle defaultLifeCycle = moduleObj.List_LifeCycles.FirstOrDefault();
                string listName = moduleObj.ModuleTable;
                DataTable spList = GetTableDataManager.GetTableData(listName);
                Ticket ticket = new Ticket(AppContextForThread, moduleObj.ModuleName);

                List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                List<FieldAliasCollection> listFields = templstColumn.Where(r => r.ListName == listName).ToList();
                List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();

                foreach (DataRow row in pair.Value)
                {
                    DataRow listItem = spList.NewRow();
                    List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                    
                    uHelper.SetFilledValuesFromDatarow(AppContextForThread, lstColumn, row, listItem, listFields);
                    ticket.SetItemValues(listItem, formValues, true, false, AppContextForThread.CurrentUser.Id);
                    
                    if (UGITUtility.StringToInt(listItem["ID"]) == 0)
                        ticket.Create(listItem, AppContextForThread.CurrentUser);
                    string error = ticket.CommitChanges(listItem);
                    if(error.StartsWith("Fail"))
                    {
                        numSkipped++;
                        string titleImported = UGITUtility.ObjectToString(listItem[DatabaseObjects.Columns.Title]);
                        if(!string.IsNullOrEmpty(titleImported))
                            lstSkippedTitles.Add(titleImported);
                    }
                    else
                    {
                        numAdded++;
                    }
                }
            }

            //errorMessage = string.Format("Import from {0} successful: {1} new records imported, {2} updated, {3} skipped", Path.GetFileName(filePath), numAdded, numUpdated, numSkipped);
            errorMessage = $"Import from {Path.GetFileName(filePath)} successful: {numAdded} new records imported, {numUpdated} updated, {numSkipped} skipped<BR>";
            if(lstSkippedTitles.Count > 0)
                errorMessage = $"{errorMessage}Items could not be imported are: {UGITUtility.ConvertListToString(lstSkippedTitles, "<br>")}";
        }

        private void ImportModuleBasedTickets(string pModuleName,bool deleteExistingRecords, ref string errorMessage)
        {
            ModuleExcelImport excelImport = new ModuleExcelImport();
            if (excelImport.ModuleImportRunning(pModuleName))
            {
                errorMessage = "Import already running";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }

            if (!flpImport.HasFile)
            {
                errorMessage = "Excel file required.";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }
            string FileName = Path.GetFileName(flpImport.PostedFile.FileName);
            string Extension = Path.GetExtension(flpImport.PostedFile.FileName);
            string FolderPath = flpImport.PostedFile.FileName;

            if (Extension.ToLower() != ".xlsx" && Extension.ToLower() != ".csv")
            {
                errorMessage = "Only excel and csv format import is supported (.xlsx, .csv)";
                Util.Log.ULog.WriteLog(errorMessage);
                lblMessage.Text = errorMessage;
                lblMessage.Visible = true;
                return;
            }

            string filePath = string.Format(@"{0}\{1}", uHelper.GetTempFolderPath(), FileName);
            flpImport.SaveAs(filePath);

            UGITModule module = ObjModuleViewManager.LoadByName(pModuleName);

            ImportStatus status = new ImportStatus(pModuleName);
            ModuleExcelImport.runningImports.Add(status);
            DateTime aDate = DateTime.Now;
            RunInBackgroundImportStatus backgroundImportStatus = new RunInBackgroundImportStatus(pModuleName, User.Name, "Running", aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"), string.Empty, DateTime.Now.Date);
            RunInBackgroundModuleExcelImport.runningImports.Add(backgroundImportStatus);
            if (chkRuninBackground.Checked)
            {
                // make active BackgroundStatus icon In fixedCustom menu bar
                Session["SetBackgroundIcon"] = true;
                string userID = HttpContext.Current.CurrentUser().Id;

                ThreadStart starter = delegate
                {
                    if (pModuleName == ModuleNames.CMDB)
                        AssetImportInBackground(AppContextForThread, userID, filePath, deleteExistingRecords, status);
                    else
                        ImportTicketInBackground(AppContextForThread, userID, filePath, deleteExistingRecords, module, status, backgroundImportStatus);

                    ModuleExcelImport.runningImports.Remove(status);
                };


                Thread thread = new Thread(starter);
                thread.IsBackground = true;
                thread.Start();

                //close popup
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else
            {
                try
                {
                    if (pModuleName == ModuleNames.CMDB)
                    {
                        AssetImport(AppContextForThread, User, filePath, chkDeleteExistingRecord.Checked, status);
                        if (!status.succeeded)
                            errorMessage = string.Format("Asset Import Failed!");
                    }
                    else
                    {
                        ImportTicket(HttpContext.Current.GetManagerContext(), User, filePath, chkDeleteExistingRecord.Checked, module, status);
                        ModuleExcelImport.runningImports.Remove(status);

                        if (!status.succeeded)
                            errorMessage = string.Format("Import Failed!");
                    }
                }
                catch (Exception ex)
                {
                    ModuleExcelImport.runningImports.Remove(status);
                    string logMessage = string.Format("ERROR Importing from file {0}", Path.GetFileName(filePath));
                    Util.Log.ULog.WriteLog(logMessage);
                    errorMessage = ex.Message;
                    Util.Log.ULog.WriteLog(string.Format("{0}: {1}", logMessage, ex.Message));
                }

                ModuleExcelImport.runningImports.Remove(status);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = errorMessage;
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = string.Format("Import from {0} successful: {1} new records imported, {2} updated, {3} skipped, {4} processed", Path.GetFileName(filePath), status.recordsAdded, status.recordsUpdated, status.recordsSkipped, status.recordsProcessed);
                    lblMessage.Visible = true;
                    //uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                ImportSendEmail(status, errorMessage, FileName);
            }
        }

        public void ImportSendEmail(ImportStatus status, string errorMessage, string filename = "")
        {
            string sendEmail = ObjConfigurationVariableManager.GetValue("SendEmail");
            if (sendEmail.Equals("true", StringComparison.InvariantCultureIgnoreCase) && User != null && !string.IsNullOrWhiteSpace(User.Email))
            {
                MailMessenger mail = new MailMessenger(AppContextForThread);
                StringBuilder body = new StringBuilder();
                string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                string signature = ObjConfigurationVariableManager.GetValue("Signature");
                body.AppendFormat("{0} {1},<br /><br />", greeting, User.Name);
                if(!string.IsNullOrEmpty(filename))
                    body.AppendFormat("The import process {0} for {1}.", status.succeeded ? "completed successfully" : "FAILED", filename);
                else
                    body.AppendFormat("The import process {0}.", status.succeeded ? "completed successfully." : "FAILED!");

                body.AppendFormat("<br /><br />");
                if (status.recordsAdded > 0)
                    body.AppendFormat("{0} record(s) added<br />", status.recordsAdded);
                if (status.recordsUpdated > 0)
                    body.AppendFormat("{0} record(s) updated<br />", status.recordsUpdated);
                if (status.recordsSkipped > 0)
                    body.AppendFormat("{0} record(s) skipped due to errors<br />", status.recordsSkipped);
                if (status.recordsProcessed > 0)
                    body.AppendFormat("______________________________________________<br />");
                body.AppendFormat("{0} total record(s) processed<br />", status.recordsProcessed);

                bool errorHeaderAdded = false;
                if (!string.IsNullOrWhiteSpace(errorMessage)) { 
                    body.AppendFormat("<br /><br /><b>ERRORS:</b><br />{0}", errorMessage);
                    errorHeaderAdded = true;
                }
                if (status.errorMessages.Count > 0) { 
                    if(errorHeaderAdded)
                        body.AppendFormat("<br />{0}", string.Join(",", status.errorMessages));
                    else
                        body.AppendFormat("<br /><br /><b>ERRORS:</b><br />{0}", string.Join(",", status.errorMessages));
                }
                body.AppendFormat("<br /><br />{0}<br />", signature);

                mail.SendMail(User.Email, status.succeeded ? "Import Complete" : "Import FAILED", string.Empty, body.ToString(), true);
            }
        }

        ///summary
        ///Batch Delete List Items
        private void DeleteAllListItem(string listName)
        {
            ////delete the record from the list...if check box is check.
            if (chkDeleteExistingRecord.Checked)
            {
                DataTable dt = GetTableDataManager.GetTableData(listName);
                List<long> ids = new List<long>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ids.Add(UGITUtility.StringToLong(dr[DatabaseObjects.Columns.Id]));
                    }
                    RMMSummaryHelper.BatchDeleteListItems(HttpContext.Current.GetManagerContext(), ids, listName);
                }
            }
        }

        // Load all lookup list data needed for Asset Import
        private static Dictionary<string, DataTable> GetLookupLists(ApplicationContext spWeb, string moduleName)
        {
            Dictionary<string, DataTable> data = new Dictionary<string, DataTable>();
            List<string> lookupLists = new List<string>() { DatabaseObjects.Tables.AssetVendors, DatabaseObjects.Tables.AssetModels, DatabaseObjects.Tables.Department,
                                                            DatabaseObjects.Tables.CompanyDivisions, DatabaseObjects.Tables.Location, DatabaseObjects.Tables.RequestType,DatabaseObjects.Tables.TicketPriority,
                                                            DatabaseObjects.Tables.ModuleStages,DatabaseObjects.Tables.TicketImpact,DatabaseObjects.Tables.TicketSeverity,DatabaseObjects.Tables.ACRTypes,
                                                            DatabaseObjects.Tables.FunctionalAreas,DatabaseObjects.Tables.Services,DatabaseObjects.Tables.Applications,DatabaseObjects.Tables.SubLocation,
                                                            DatabaseObjects.Tables.Assets};
            List<string> viewFields = new List<string>();

            foreach (string lookupListName in lookupLists)
            {
                //string query = "";
                // Add all required fields depending on lookup field
                viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id));
                viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title));

                if (lookupListName == DatabaseObjects.Tables.AssetVendors)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.VendorName));
                else if (lookupListName == DatabaseObjects.Tables.AssetModels)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ModelName));
                else if (lookupListName == DatabaseObjects.Tables.RequestType)
                    viewFields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketRequestType));

                //query.ViewFields = string.Join("", viewFields);

                //if (lookupListName == DatabaseObjects.Tables.RequestType)
                //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, moduleName);
                //else
                //    query.Query = string.Format("<Where></Where>");

                //DataTable dataTable = SPListHelper.GetDataTable(lookupListName, query, spWeb);
                //data.Add(lookupListName, dataTable);
            }
            return data;
        }

        private static void SaveValuesToList(Worksheet wsitem, int rowCount, RowCollection rows, List<string> lstColumn, List<FieldAliasCollection> templstColumn, List<FieldAliasCollection> objFieldAliasCollection, string lstName)
        {
            for (int i = 1; i <= rowCount; i++)
            {
                Row rowData = rows[i];
                //string query = "";

                List<FieldAliasCollection> objFieldAliasQuery = new List<FieldAliasCollection>();
                List<FieldAliasCollection> objFieldAliasQuery1 = new List<FieldAliasCollection>();
                if (wsitem.Name.Contains(DatabaseObjects.Tables.Department))
                {
                    objFieldAliasQuery = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Department && r.InternalName == DatabaseObjects.Columns.Title).ToList();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn));
                }

                else if (wsitem.Name.Contains(DatabaseObjects.Tables.Location))
                {
                    objFieldAliasQuery = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Location && r.InternalName == DatabaseObjects.Columns.Title).ToList();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn));
                }

                else if (wsitem.Name.Contains("Budget Categories"))
                {
                    objFieldAliasQuery = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.BudgetCategories && r.InternalName == DatabaseObjects.Columns.BudgetCategory).ToList();
                    objFieldAliasQuery1 = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.BudgetCategories && r.InternalName == DatabaseObjects.Columns.BudgetSubCategory).ToList();
                    //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.BudgetCategory, uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn), DatabaseObjects.Columns.BudgetSubCategory, uHelper.GetValueByColumn(rowData, objFieldAliasQuery1[0], lstColumn));
                }
                else if (wsitem.Name.Contains(DatabaseObjects.Tables.Applications))
                {
                    objFieldAliasQuery = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Applications && r.InternalName == DatabaseObjects.Columns.Title).ToList();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn));
                }

                else if (wsitem.Name.Contains("Request Type"))
                {
                    objFieldAliasQuery = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.RequestType && r.InternalName == DatabaseObjects.Columns.TicketRequestType).ToList();
                    objFieldAliasQuery1 = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.RequestType && r.InternalName == DatabaseObjects.Columns.ModuleNameLookup).ToList();
                    //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketRequestType, uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn), DatabaseObjects.Columns.ModuleNameLookup, uHelper.GetValueByColumn(rowData, objFieldAliasQuery1[0], lstColumn));
                }

                if (string.IsNullOrEmpty(uHelper.GetValueByColumn(rowData, objFieldAliasQuery[0], lstColumn)))
                    continue;

                if (wsitem.Name.Contains("Request Type") && string.IsNullOrEmpty(uHelper.GetValueByColumn(rowData, objFieldAliasQuery1[0], lstColumn)))
                    continue;

                //DataRow[] lstNameCol = lstName.GetItems(query);

                //DataRow listItem;
                //if (lstNameCol != null && lstNameCol.Count() > 0)
                //{
                //    listItem = lstNameCol[0];
                //    uHelper.SetFilledValues(HttpContext.Current.GetManagerContext(),lstColumn, rowData, listItem, objFieldAliasCollection);
                //}
                //else
                //{
                //    listItem = lstName.Items.Add();
                //    uHelper.SetFilledValues(HttpContext.Current.GetManagerContext(), lstColumn, rowData, listItem, objFieldAliasCollection);

                //    if (wsitem.Name.Contains(DatabaseObjects.Tables.Applications))
                //    {
                //        string queryCount = "";
                //        int totalTicketCount = lstName.GetItems(queryCount).Count;
                //        listItem[DatabaseObjects.Columns.TicketId] = "APP-" + DateTime.Now.ToString("yy") + "-" + (totalTicketCount + 1).ToString(new string('0', 6));
                //    }
                //}
                //listItem.AcceptChanges();
            }
        }

        private static bool CheckColumnExistsInExcel(List<string> lstColumn, FieldAliasCollection facItemGroup)
        {
            if (lstColumn.Contains(facItemGroup.InternalName, StringComparer.OrdinalIgnoreCase))
                return true;

            foreach (var anItem in facItemGroup.AliasNames.Split(','))
            {
                if (lstColumn.Contains(anItem, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        protected void lnkMasterImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (flpImport.HasFile)
                {
                    string FileName = Path.GetFileName(flpImport.PostedFile.FileName);
                    string Extension = Path.GetExtension(flpImport.PostedFile.FileName);
                    string FolderPath = flpImport.PostedFile.FileName;

                    if (Extension.ToLower() != ".xlsx")
                    {
                        lblMessage.Text = "Only excel format import is supported (.xlsx)";
                        lblMessage.Visible = true;
                        return;
                    }

                    string FilePath = string.Format(@"{0}\{1}", uHelper.GetTempFolderPath(), FileName);
                    flpImport.SaveAs(FilePath);

                    ASPxSpreadsheet1.Document.LoadDocument(FilePath);

                    foreach (var wsitem in ASPxSpreadsheet1.Document.Worksheets)
                    {
                        var worksheet = wsitem;
                        Workbook wb = new Workbook();
                        wb.LoadDocument(FilePath);
                        worksheet.CopyFrom(wb.Worksheets[wsitem.Index]);

                        int colCount = worksheet.Columns.LastUsedIndex;
                        int rowCount = worksheet.Rows.LastUsedIndex;

                        RowCollection rows = worksheet.Rows;
                        Row row = rows[0];

                        //get the columns of excel...
                        List<string> lstColumn = new List<string>();
                        for (int i = 0; i <= colCount; i++)
                        {
                            lstColumn.Add(row.Worksheet.Cells[i].DisplayText.Trim());
                        }

                        List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                        List<FieldAliasCollection> objFieldAliasCollection = new List<FieldAliasCollection>();

                        // check mandatory field present in excel..
                        #region "Mandatory Check"
                        if (wsitem.Name.Contains(DatabaseObjects.Tables.Location))
                        {
                            List<FieldAliasCollection> objFieldAliasCollectionMandatoryCheck = new List<FieldAliasCollection>();
                            objFieldAliasCollectionMandatoryCheck = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Location && r.InternalName == DatabaseObjects.Columns.Title).ToList();

                            bool isCheckMandatoryField = false;
                            if (objFieldAliasCollectionMandatoryCheck.Count > 0)
                            {
                                string[] aliasName = objFieldAliasCollectionMandatoryCheck[0].AliasNames.Split(',');
                                foreach (var tempName in aliasName)
                                {
                                    if (lstColumn.IndexOf(tempName) > -1)
                                    {
                                        isCheckMandatoryField = true;
                                    }
                                }
                            }
                            if (!isCheckMandatoryField)
                            {
                                //Log.WriteLog("Title fields are mandatory in excel for Location.");
                                lblMessage.Text = "Title fields are mandatory in excel.";
                                lblMessage.Visible = true;
                                continue;
                            }
                        }

                        else if (wsitem.Name.Contains(DatabaseObjects.Tables.Department))
                        {
                            List<FieldAliasCollection> objFieldAliasCollectionMandatoryCheck = new List<FieldAliasCollection>();
                            objFieldAliasCollectionMandatoryCheck = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Department && r.InternalName == DatabaseObjects.Columns.Title).ToList();

                            bool isCheckMandatoryField = false;
                            if (objFieldAliasCollectionMandatoryCheck.Count > 0)
                            {

                                string[] aliasName = objFieldAliasCollectionMandatoryCheck[0].AliasNames.Split(',');
                                foreach (var tempName in aliasName)
                                {
                                    if (lstColumn.IndexOf(tempName) > -1)
                                    {
                                        isCheckMandatoryField = true;
                                    }
                                }
                            }
                            if (!isCheckMandatoryField)
                            {
                                //Log.WriteLog("Title fields are mandatory in excel for Department.");
                                lblMessage.Text += "Title fields are mandatory in excel.";
                                lblMessage.Visible = true;
                                continue;
                            }
                        }
                        // User have sepreate funtion for Import..
                        //else if (wsitem.Name.Contains("Users"))
                        //{
                        //    List<FieldAliasCollection> objFieldAliasCollectionMandatoryCheck = new List<FieldAliasCollection>();
                        //    objFieldAliasCollectionMandatoryCheck = templstColumn.Where(r => r.ListName == "UserInformationList").ToList();
                        //    if (!isSiteAdmin)
                        //    {
                        //        int userID = SPContext.Current.Web.CurrentUser.ID;
                        //        SPSecurity.RunWithElevatedPrivileges(delegate()
                        //        {
                        //            using (SPSite spSite = new SPSite(SPContext.Current.Site.Url))
                        //            {
                        //                using (ApplicationContext context = spSite.OpenWeb())
                        //                {
                        //                    UserProfile currentUser = UserProfile.GetUserById(userID, spWeb);
                        //                    spWeb.AllowUnsafeUpdates = true;
                        //                    ImportUserHelper.ImportUsers(rowCount, rows, lstColumn, objFieldAliasCollectionMandatoryCheck, spWeb, currentUser);
                        //                    StringBuilder seletedParams = new StringBuilder();
                        //                    seletedParams.AppendFormat("ugroup={0}", Request["ugroup"]);
                        //                    seletedParams.AppendFormat("&upage={0}", Request["upage"]);
                        //                    Context.Cache.Add(string.Format("EditUserInfo-{0}", SPContext.Current.Web.CurrentUser.ID), seletedParams.ToString(), null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

                        //                    spWeb.AllowUnsafeUpdates = false;
                        //                }
                        //            }
                        //        });
                        //    }
                        //    else
                        //    {
                        //        ImportUserHelper.ImportUsers(rowCount, rows, lstColumn, objFieldAliasCollectionMandatoryCheck, SPContext.Current.Web, SPContext.Current.Web.CurrentUser);
                        //        StringBuilder seletedParams = new StringBuilder();
                        //        seletedParams.AppendFormat("ugroup={0}", Request["ugroup"]);
                        //        seletedParams.AppendFormat("&upage={0}", Request["upage"]);
                        //        Context.Cache.Add(string.Format("EditUserInfo-{0}", SPContext.Current.Web.CurrentUser.ID), seletedParams.ToString(), null, DateTime.Now.AddSeconds(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        //    }
                        //    continue;
                        //}
                        else if (wsitem.Name.Contains("Budget Categories"))
                        {
                            // no field mandatory
                        }
                        else if (wsitem.Name.Contains("Applications"))
                        {
                            List<FieldAliasCollection> objFieldAliasCollectionMandatoryCheck = new List<FieldAliasCollection>();
                            objFieldAliasCollectionMandatoryCheck = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Applications && r.InternalName == DatabaseObjects.Columns.Title).ToList();

                            bool isCheckMandatoryField = false;
                            if (objFieldAliasCollectionMandatoryCheck.Count > 0)
                            {
                                string[] aliasName = objFieldAliasCollectionMandatoryCheck[0].AliasNames.Split(',');
                                foreach (var tempName in aliasName)
                                {
                                    if (lstColumn.IndexOf(tempName) > -1)
                                    {
                                        isCheckMandatoryField = true;
                                    }
                                }
                            }
                            if (!isCheckMandatoryField)
                            {
                                //Log.WriteLog("Title fields are mandatory in excel for Application.");
                                lblMessage.Text += "Title fields are mandatory in excel.";
                                lblMessage.Visible = true;
                                continue;
                            }
                        }
                        // special case here we combine two sheet and import in our Request Type list..
                        else if (wsitem.Name.Contains("Request Type Categories"))
                        {
                            //get the Request Type Categories datatable form excel.
                            Worksheet worksheetRequestCategory = wsitem;
                            //tables not found in the worksheet that why we pass range manully here..
                            CellRange rangeRequestCategory = worksheetRequestCategory.Range["A:G"];
                            DataTable dataTableRequestCategory = worksheetRequestCategory.CreateDataTable(rangeRequestCategory, true);
                            DataTableExporter exporterRequestCategory = worksheetRequestCategory.CreateDataTableExporter(rangeRequestCategory, dataTableRequestCategory, true);
                            exporterRequestCategory.Export();

                            //get the Request Type datatable from excel.
                            Worksheet worksheetRequestType = wb.Worksheets[wsitem.Index + 1];
                            CellRange rangeRequestType = worksheetRequestType.Tables[0].Range;
                            DataTable dataTableRequestType = worksheetRequestType.CreateDataTable(rangeRequestType, true);
                            DataTableExporter exporterRequestType = worksheetRequestType.CreateDataTableExporter(rangeRequestType, dataTableRequestType, true);
                            exporterRequestType.Export();

                            DataTable dtRequestTypeCategory = exporterRequestCategory.DataTable;
                            DataTable dtRequestType = exporterRequestType.DataTable;

                            // fill the blank entry into request type..(only for category)
                            string tempRequestType = string.Empty;
                            foreach (DataRow rowRequestTypeItem in dtRequestType.Rows)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(rowRequestTypeItem[0])))
                                    tempRequestType = Convert.ToString(rowRequestTypeItem[0]);

                                if (string.IsNullOrEmpty(Convert.ToString(rowRequestTypeItem[0])))
                                {
                                    rowRequestTypeItem[0] = tempRequestType;
                                }
                            }

                            // fill the blank entry into request type Category..(only for category)
                            string tempRequestTypeCategory = string.Empty;
                            foreach (DataRow rowRequestTypeCategoryItem in dtRequestTypeCategory.Rows)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(rowRequestTypeCategoryItem[0])))
                                    tempRequestTypeCategory = Convert.ToString(rowRequestTypeCategoryItem[0]);

                                if (string.IsNullOrEmpty(Convert.ToString(rowRequestTypeCategoryItem[0])))
                                {
                                    rowRequestTypeCategoryItem[0] = tempRequestTypeCategory;
                                }
                            }


                            List<FieldAliasCollection> objFieldAliasCollectionModuleName = new List<FieldAliasCollection>();
                            objFieldAliasCollectionModuleName = templstColumn.Where(r => r.ListName == "RequestType").ToList();
                            FieldAliasCollection facItemGroup = objFieldAliasCollectionModuleName.Where(x => x.InternalName == "ModuleNameLookup").FirstOrDefault();

                            //create addtional columns if required here..
                            DataTable dtFinalRequestType = new DataTable();
                            dtFinalRequestType = dtRequestTypeCategory.Clone();
                            dtFinalRequestType.Columns.Add(dtRequestType.Columns[1].ColumnName, typeof(String));
                            if (!CheckColumnExistsInExcel(lstColumn, facItemGroup))
                            {
                                dtFinalRequestType.Columns.Add("ModuleNameLookup", typeof(String));
                                dtFinalRequestType.Columns["ModuleNameLookup"].DefaultValue = "TSR";
                            }

                            //create final table for request type...
                            int columnCount = dtRequestTypeCategory.Columns.Count;
                            foreach (DataRow rowCategory in dtRequestTypeCategory.Rows)
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(rowCategory[0])))
                                    continue;

                                string expression = string.Format("{0} = '{1}'", dtRequestType.Columns[0].ColumnName, rowCategory[0]);
                                DataRow[] foundRows = dtRequestType.Select(expression);
                                foreach (DataRow foundRowitem in foundRows)
                                {
                                    DataRow newRow = dtFinalRequestType.NewRow();
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        newRow[i] = rowCategory[i];
                                    }
                                    newRow[dtRequestType.Columns[1].ColumnName] = foundRowitem[1];
                                    dtFinalRequestType.Rows.Add(newRow);
                                }
                            }

                            // Import final datatable into worksheet..
                            Workbook newworkbook = new Workbook();
                            Worksheet finalRequestTypeSheet = newworkbook.Worksheets[0];
                            finalRequestTypeSheet.Name = "Request Type";
                            finalRequestTypeSheet.Import(dtFinalRequestType, true, 0, 0);

                            int colCountRequestType = finalRequestTypeSheet.Columns.LastUsedIndex;
                            int rowCountRequestType = finalRequestTypeSheet.Rows.LastUsedIndex;

                            RowCollection rowsRequestType = finalRequestTypeSheet.Rows;
                            Row rowRequestType = rowsRequestType[0];

                            //reset the column of excel..
                            lstColumn.Clear();
                            for (int i = 0; i <= colCountRequestType; i++)
                            {
                                lstColumn.Add(rowRequestType.Worksheet.Cells[i].DisplayText.Trim());
                            }

                            objFieldAliasCollection = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.RequestType).ToList();
                            ///SPList lstNameRequestType =GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType);

                            //// save values into list.
                            //SaveValuesToList(finalRequestTypeSheet, rowCountRequestType, rowsRequestType, lstColumn, templstColumn, objFieldAliasCollection, lstNameRequestType);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                        #endregion

                        DataTable lstName = null;
                        if (wsitem.Name.Contains(DatabaseObjects.Tables.Location))
                        {
                            objFieldAliasCollection = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Location).ToList();
                            lstName = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location);
                        }
                        else if (wsitem.Name.Contains(DatabaseObjects.Tables.Department))
                        {
                            objFieldAliasCollection = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Department).ToList();
                            lstName = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Department);
                        }
                        else if (wsitem.Name.Contains("Budget Categories"))
                        {
                            objFieldAliasCollection = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.BudgetCategories).ToList();
                            lstName = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BudgetCategories);
                        }
                        else if (wsitem.Name.Contains(DatabaseObjects.Tables.Applications))
                        {
                            objFieldAliasCollection = templstColumn.Where(r => r.ListName == DatabaseObjects.Tables.Applications).ToList();
                            lstName = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications);
                        }

                        //if (wsitem.Name.Contains(DatabaseObjects.Tables.Location) || wsitem.Name.Contains(DatabaseObjects.Tables.Department) || wsitem.Name.Contains("Budget Categories") || wsitem.Name.Contains(DatabaseObjects.Tables.Applications))
                        //    SaveValuesToList(wsitem, rowCount, rows, lstColumn, templstColumn, objFieldAliasCollection, lstName);
                    }

                    if (string.IsNullOrEmpty(lblMessage.Text))
                        uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblMessage.Text = "excel file required.";
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //Log.WriteLog(string.Format("Unable to import-{0}", ex.Message));
                lblMessage.Text = string.Format("Unable to import-{0}", ex.Message);
                lblMessage.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true, true);
        }

        /// <summary>
        /// Import ticket in background
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="userId"></param>
        /// <param name="filePath"></param>
        /// <param name="deleteExistingRecords"></param>
        /// <param name="module"></param>
        private void ImportTicketInBackground(ApplicationContext context, string userId, string filePath, bool deleteExistingRecords, UGITModule module, ImportStatus status, RunInBackgroundImportStatus backgroundImportStatus)
        {
            BackgroundProcessStatusManager backgroundProcessStatusManager = new BackgroundProcessStatusManager(context);
            BackgroundProcessStatus backgroundProcessStatus = new BackgroundProcessStatus();


            UserProfile user = context.CurrentUser;
            //UserProfile user = HttpContext.Current.GetUserManager().GetUserById(userId);
            string errorMessage = string.Empty;
            string SDAte = string.Empty;
            try
            {
                SDAte = backgroundImportStatus.startDate;

                ImportTicket(context, user, filePath, deleteExistingRecords, module, status);

                ModuleExcelImport.runningImports.Remove(status);

                #region RunInBackgroundImportStatus
                RunInBackgroundModuleExcelImport.runningImports.Remove(backgroundImportStatus);
                DateTime aDate = DateTime.Now;
                RunInBackgroundImportStatus completedImportStatus = new RunInBackgroundImportStatus(this.Module, User.Name, "Completed", SDAte, aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"), aDate.Date);
                RunInBackgroundModuleExcelImport.runningImports.Add(completedImportStatus);
                if (this.Module == "PMM")
                    backgroundProcessStatus.ServiceName = "Import PMM Projects ";
                else
                    backgroundProcessStatus.ServiceName = string.Format("Import {0} Tickets", this.Module);

                backgroundProcessStatus.Status = "Completed";
                backgroundProcessStatus.StartDate = SDAte.ToDateTime();
                backgroundProcessStatus.EndDate = aDate;
                backgroundProcessStatus.UserName = user.Name;
                backgroundProcessStatusManager.Insert(backgroundProcessStatus);
                #endregion

                if (status.errorMessages.Count > 0)
                    errorMessage = string.Join<string>("<br />", status.errorMessages);
            }
            catch (Exception ex)
            {
                ModuleExcelImport.runningImports.Remove(status);

                #region RunInBackgroundImportStatus
                RunInBackgroundModuleExcelImport.runningImports.Remove(backgroundImportStatus);
                DateTime aDate = DateTime.Now;
                RunInBackgroundImportStatus FailedImportStatus = new RunInBackgroundImportStatus(this.Module, User.Name, "Failed", SDAte, aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"), aDate.Date);
                RunInBackgroundModuleExcelImport.runningImports.Add(FailedImportStatus);
                if (this.Module == "PMM")
                    backgroundProcessStatus.ServiceName = "Import PMM Projects ";
                else
                    backgroundProcessStatus.ServiceName = string.Format("Import {0} Tickets", this.Module);

                backgroundProcessStatus.Status = "Failed";
                backgroundProcessStatus.StartDate = SDAte.ToDateTime();
                backgroundProcessStatus.EndDate = aDate;
                backgroundProcessStatus.UserName = user.Name;
                backgroundProcessStatusManager.Insert(backgroundProcessStatus);
                #endregion

                string logMessage = string.Format("ERROR Importing Tickets from file {0}", Path.GetFileName(filePath));
                //Log.WriteException(ex, logMessage);
                errorMessage = ex.Message;

                //ULog.WriteUGITLog(ApplicationContext context, string.Format("{0}: {1}", logMessage, ex.Message), UGITLogSeverity.Error, UGITLogCategory.Import);

            }

            ImportSendEmail(status, errorMessage, Path.GetFileName(filePath));
        }

        /// <summary>
        /// Import ticket
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="user"></param>
        /// <param name="filePath"></param>
        /// <param name="deleteExistingRecords"></param>
        /// <param name="moduleObj"></param>
        /// <returns></returns>
        public void ImportTicket(ApplicationContext context, UserProfile user, string filePath, bool deleteExistingRecords, UGITModule moduleObj, ImportStatus status)
        {
            int i = 1;
            try
            {
                TicketManager ticketManager = new TicketManager(context);

                string errorMessage = string.Empty;
                if (moduleObj == null)
                {
                    errorMessage = "Module not passed!";
                    Util.Log.ULog.WriteLog(errorMessage);
                    status.succeeded = false;
                    status.errorMessages.Add(errorMessage);
                    status.recordsProcessed = 0;
                    return;
                }

                LifeCycleStageManager objLifeCycleStageManager = new LifeCycleStageManager(context);
                List<ModuleRoleWriteAccess> ColumnsList = moduleObj.List_RoleWriteAccess;
                LifeCycle defaultLifeCycle = moduleObj.List_LifeCycles.FirstOrDefault();
                string listName = moduleObj.ModuleTable;
                DataTable spList = GetTableDataManager.GetTableStructure(listName);
                Ticket ticket = new Ticket(context, moduleObj.ModuleName);

                List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                List<FieldAliasCollection> listFields = templstColumn.Where(r => r.ListName == listName).ToList();
                if (listFields == null || listFields.Count == 0)
                {
                    errorMessage = "Import not configured for this module";
                    Util.Log.ULog.WriteLog(errorMessage);
                    status.succeeded = false;
                    status.errorMessages.Add(errorMessage);
                    status.recordsProcessed = 0;
                    return;
                }

                Workbook workbook = new Workbook();
                workbook.LoadDocument(filePath);
                CellRange range = workbook.Worksheets[0].GetDataRange();
                DataTable data = workbook.Worksheets[0].CreateDataTable(range, true);
                if (data == null)
                {
                    errorMessage = "No data found in file!";
                    Util.Log.ULog.WriteLog(errorMessage);
                    status.succeeded = false;
                    status.errorMessages.Add(errorMessage);
                    status.recordsProcessed = 0;
                    return;
                }

                string workSheetName = workbook.Worksheets[0].Name;
                List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
                int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;
                status.totalRecords = lastIndex;
                string mandatoryColumnName = DatabaseObjects.Columns.Title;
                List<FieldAliasCollection> mandatoryFieldsAlias = new List<FieldAliasCollection>();
                mandatoryFieldsAlias = listFields.Where(r => r.InternalName == DatabaseObjects.Columns.Title).ToList();
                bool checkMandatory = false;
                if (mandatoryFieldsAlias.Count > 0)
                {
                    string[] aliasName = mandatoryFieldsAlias[0].AliasNames.Split(',');
                    foreach (string alias in aliasName)
                    {
                        if (lstColumn.Contains(alias))
                        {
                            checkMandatory = true;
                            mandatoryColumnName = alias;
                            break;
                        }

                    }
                }

                if (!checkMandatory)
                {
                    errorMessage = string.Format("{0} is mandatory in excel", UGITUtility.AddSpaceBeforeWord(mandatoryColumnName));
                    Util.Log.ULog.WriteLog(errorMessage);
                    status.succeeded = false;
                    status.errorMessages.Add(errorMessage);
                    status.recordsProcessed = 0;
                    return;
                }

                List<FieldAliasCollection> ticketFieldAlias = new List<FieldAliasCollection>();
                ticketFieldAlias = listFields.Where(r => r.InternalName == DatabaseObjects.Columns.TicketId).ToList();
                string ticketIdColumnName = string.Empty;
                if (ticketFieldAlias.Count > 0)
                {
                    string[] aliasName = ticketFieldAlias[0].AliasNames.Split(',');
                    foreach (string alias in aliasName)
                    {
                        if (lstColumn.Contains(alias))
                        {
                            ticketIdColumnName = alias;
                            break;
                        }
                    }
                }

                // Delete the record from the list...if check box is check.
                if (deleteExistingRecords)
                {
                    Util.Log.ULog.WriteLog("Deleting existing assets!", "WARN");

                    DataRow[] deleteColl = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select();
                    if (deleteColl != null && deleteColl.Count() > 0)
                    {
                        foreach (DataRow deleteRow in deleteColl)
                        {
                            UGITModule module = ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(Convert.ToString(deleteRow[DatabaseObjects.Columns.ID])));
                            ticketManager.Delete(module, deleteRow);
                        }
                    }
                }

                List<string> recordWiseErrors = new List<string>();

                //DataColumn mandatoryField = null;

                //DataColumn ticketField = null;

                int mandatoryFieldIndex = -1, ticketFieldIndex = -1;
                mandatoryFieldIndex = lstColumn.FindIndex(s => s.Equals(mandatoryColumnName, StringComparison.OrdinalIgnoreCase));

                ticketFieldIndex = lstColumn.FindIndex(s => s.Equals(ticketIdColumnName, StringComparison.OrdinalIgnoreCase));
                //Check with existing

                Hashtable existingTickets = new Hashtable();

                List<string> newTickets = new List<string>();
                List<string> updatedTickets = new List<string>();
                Dictionary<string, DataTable> lookupLists = GetLookupLists(context, moduleObj.ModuleName);
                Hashtable importedTickets = new Hashtable();
                for (i = 1; i <= lastIndex; i++)
                {
                    DataRow listItem = null;
                    Row row = workbook.Worksheets[0].Rows[i];
                    string ticketTitle = row[mandatoryFieldIndex].DisplayText;
                    string ticketPublicId = string.Empty;
                    if (ticketFieldIndex != -1)
                        ticketPublicId = row[ticketFieldIndex].DisplayText;
                    if (string.IsNullOrWhiteSpace(ticketTitle))
                    {
                        errorMessage = string.Format("Title is missing in row {0}", i+1);
                        status.errorMessages.Add(errorMessage);
                        status.errorMessages.Add($"<br>Record Skipped at row {i + 1}.");
                        status.recordsSkipped++;
                        continue;
                    }

                    bool isExistingTicket = false;

                    if (!string.IsNullOrEmpty(ticketPublicId) && importedTickets.ContainsKey(ticketPublicId))
                    {
                        errorMessage = string.Format("Line # {0}: Skipping duplicate ticket ID {1}", i + 1, ticketPublicId);
                        Util.Log.ULog.WriteLog(errorMessage);
                        status.errorMessages.Add(errorMessage);
                        status.recordsSkipped++;
                        continue;
                    }
                    else
                    {
                        listItem = ticketManager.GetByTicketID(moduleObj, ticketPublicId);
                        
                        if (listItem != null)
                        {
                            isExistingTicket = true;
                            Util.Log.ULog.WriteLog("Updating existing ticket with title: " + ticketTitle);
                        }
                        else
                        {
                            listItem = spList.NewRow();
                            Util.Log.ULog.WriteLog("Importing new ticket with title: " + ticketTitle);
                        }
                    }

                    List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                    string errorInRecords = uHelper.SetFilledValues(context, lstColumn, row, listItem, listFields,moduleObj.ModuleName);

                    if (!string.IsNullOrEmpty(errorInRecords))
                    {
                        recordWiseErrors.Add($"{errorInRecords} in {Path.GetFileName(filePath)} at Row number {i + 1}");
                        status.errorMessages.Add($"{errorInRecords} at Row number {i + 1}");
                    }

                    ticket.SetItemValues(listItem, formValues, true, false, context.CurrentUser.Id);
                    if (UGITUtility.StringToInt(listItem["ID"]) == 0)
                        ticket.Create(listItem, context.CurrentUser);

                    if (lstColumn.Contains("StageStep") || lstColumn.Contains("Status") || lstColumn.Contains("ModuleStepLookup"))
                    {
                        var stageList = objLifeCycleStageManager.Load(x => x.ModuleNameLookup == moduleObj.ModuleName);
                        LifeCycleStage currentStage = null;
                        if (stageList != null)
                        {
                            if (lstColumn.Contains("StageStep"))
                            {
                                FieldAliasCollection facItem = new FieldAliasCollection(listItem.Table.TableName, "StageStep", "StageStep");
                                string value = uHelper.GetValueByColumn(row, facItem, lstColumn);
                                currentStage = stageList.Where(x => x.StageStep == UGITUtility.StringToInt(value)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(value) && currentStage == null)
                                {
                                    status.errorMessages.Add($"Invalid StageStep at Row number {i + 1}");
                                }
                                
                            }
                            else if (lstColumn.Contains("Status"))
                            {
                                FieldAliasCollection facItem = new FieldAliasCollection(listItem.Table.TableName, "Status", "Status");
                                string value = uHelper.GetValueByColumn(row, facItem, lstColumn);
                                currentStage = stageList.Where(x => x.StageTitle.Equals(value, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                if (currentStage != null && !string.IsNullOrWhiteSpace(currentStage.StageTypeChoice) && currentStage.StageTypeChoice.Equals(StageType.Closed.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    string comment = "[Closed]: Using Import Tickets";
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.History, listItem.Table))
                                    {
                                        //listItem[DatabaseObjects.Columns.TicketComment] = UGITUtility.GetVersionString(context.CurrentUser.Id, comment, listItem, DatabaseObjects.Columns.TicketComment);
                                        uHelper.CreateHistory(context.CurrentUser, comment, listItem, context);
                                        Ticket.MarkTicketClosed(context, listItem);
                                    }
                                }
                                else
                                {
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, listItem.Table))
                                        listItem[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, listItem.Table))
                                        listItem[DatabaseObjects.Columns.TicketClosed] = 0;
                                }
                                if (currentStage == null)
                                {
                                    status.errorMessages.Add($"Invalid Status at Row number {i + 1}");
                                }
                            }
                            else if (lstColumn.Contains("ModuleStepLookup"))
                            {
                                FieldAliasCollection facItem = new FieldAliasCollection(listItem.Table.TableName, "ModuleStepLookup", "ModuleStepLookup");
                                string value = uHelper.GetValueByColumn(row, facItem, lstColumn);
                                currentStage = stageList.Where(x => x.StageTitle.Equals(value, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(value) && currentStage == null)
                                {
                                    status.errorMessages.Add($"Invalid ModuleStepLookup at Row number {i + 1}");
                                }
                            }

                            if (currentStage != null)
                            {
                                listItem[DatabaseObjects.Columns.TicketStatus] = currentStage.StageTitle;
                                listItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
                                listItem[DatabaseObjects.Columns.ModuleStepLookup] = currentStage.ID;
                            }
                        }
                    }
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Closed, listItem.Table))
                    {
                        if (string.IsNullOrEmpty(UGITUtility.ObjectToString(listItem[DatabaseObjects.Columns.TicketClosed])))
                            listItem[DatabaseObjects.Columns.TicketClosed] = 0;
                    }
                    if (defaultLifeCycle != null)
                    {
                        LifeCycleStage currentStage = null;
                        TicketColumnValue assetStage = null;
                        if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus))
                        {
                            assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketStatus);
                            if (assetStage != null)
                                currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.Name == Convert.ToString(assetStage.Value));
                        }
                        else if (formValues.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep))
                        {
                            assetStage = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.StageStep);
                            if (assetStage != null)
                                currentStage = defaultLifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(assetStage.Value));
                        }

                    }

                    // set stageaction user to set able to edit ticket
                    if (moduleObj.ModuleName == ModuleNames.PMM)
                    {
                        ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
                        listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                    }

                    if (Convert.ToString(listItem[DatabaseObjects.Columns.TicketStatus]) == "Closed")
                    {

                        LifeCycleManager lifeCycleHelper = new LifeCycleManager(context);
                        objLifeCycleStageManager = new LifeCycleStageManager(context);
                        objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
                        var stageList = objLifeCycleStageManager.Load();
                        var workFlow = objLifeCycle.FirstOrDefault();
                        var currentStage = stageList.Where(x => x.StageTypeChoice == "Closed").FirstOrDefault();
                        listItem[DatabaseObjects.Columns.TicketStatus] = "Closed";
                        listItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
                    }

                    string innererror = ticket.CommitChanges(listItem, donotUpdateEscalations: true, stopUpdateDependencies: true);

                    string ticketID = UGITUtility.ObjectToString(listItem[DatabaseObjects.Columns.TicketId]);
                    if (!string.IsNullOrEmpty(innererror))
                    {
                        status.errorMessages.Add($"{innererror} at row number {i + 1}.");
                        status.errorMessages.Add($"<br>Record Skipped at row {i + 1}.");
                        status.recordsSkipped++;
                    }
                    else if (isExistingTicket)
                    {
                        updatedTickets.Add(ticketID);
                        status.recordsUpdated++;
                    }
                    else
                    {
                        newTickets.Add(ticketID);
                        status.recordsAdded++;
                    }

                    status.recordsProcessed++;
                    ////importedTickets.Add(ticketTitle, listItem["ID"]);
                    //string ticketID = UGITUtility.ObjectToString(listItem[DatabaseObjects.Columns.TicketId]);
                    //if (!string.IsNullOrEmpty(ticketID))
                    //    updatedTickets.Add(ticketID);
                }
                if(recordWiseErrors.Count > 0)
                    recordWiseErrors.Add($"in {Path.GetFileName(filePath)}.");

                string message = string.Format("Import from {0} successful: {1} new records imported, {2} updated, {3} skipped, {4} processed", Path.GetFileName(filePath), status.recordsAdded, status.recordsUpdated, status.recordsSkipped, status.recordsProcessed);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, message, UGITLogSeverity.Information.ToString(), UGITLogCategory.Import.ToString(), context.CurrentUser.TenantID);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, string.Join(", ", status.errorMessages), UGITLogSeverity.Information.ToString(), UGITLogCategory.Import.ToString(), context.CurrentUser.TenantID);
                Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Records Processed: {status.recordsProcessed}, Skipped: {status.recordsSkipped}, Imported: ({status.recordsAdded}) {string.Join(", ", newTickets)}, Updated: ({status.recordsUpdated}) {string.Join(", ", updatedTickets)}", UGITLogSeverity.Information.ToString(), UGITLogCategory.Import.ToString(), context.CurrentUser.TenantID);
                Util.Log.ULog.WriteLog($"Records Processed: {status.recordsProcessed}, Skipped: {status.recordsSkipped}, Imported: ({status.recordsAdded}) {string.Join(", ", newTickets)}, Updated: ({status.recordsUpdated}) {string.Join(", ", updatedTickets)}");

                // Probably not needed - causes issues when lots of tickets loaded at once
                //if (SPContext.Current == null && updatedTickets.Count > 0)
                //    SPListHelper.ReloadTicketsInCache(updatedTickets, spWeb);

                if (status.recordsAdded > 0 || status.recordsUpdated > 0)
                    status.succeeded = true;
                else
                    status.succeeded = false;
                return;
            }
            catch (Exception ex)
            {                
                throw new Exception(string.Format("{0} at Row number {1}. Process terminated!", ex.Message, i));
            }
        }

        private void ImportNonModuleItemInBackground(ApplicationContext context, string userId, string filePath, string listName, bool deleteExistingRecords)
        {
            try
            {
                UserProfile user = UserManager.GetUserById(userId);   // UserProfile.GetUserById(userId, spWeb);
                Tuple<string, int, int> message = ImportNonModuleItem(context, user, filePath, listName, deleteExistingRecords);
                List<string> error = new List<string>();
                if (!string.IsNullOrEmpty(message.Item1))
                    error = UGITUtility.ConvertStringToList(message.Item1, ",");

                if (user != null && !string.IsNullOrWhiteSpace(user.Email))
                {
                    MailMessenger mail = new MailMessenger(context);
                    StringBuilder body = new StringBuilder();
                    string greeting = ObjConfigurationVariableManager.GetValue("Greeting");
                    string signature = ObjConfigurationVariableManager.GetValue("Signature");
                    body.AppendFormat("{0} {1},<br /><br />", greeting, user.Name);
                    body.AppendFormat(message.Item1);
                    body.AppendFormat("<br /><br />{0}<br />", signature);
                    mail.SendMail(user.Email, "Import Complete", string.Empty, body.ToString(), true);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Importing items");
            }
        }

        public Tuple<string, int, int> ImportNonModuleItem(ApplicationContext context, UserProfile user, string filePath, string listName, bool deleteExistingRecords)
        {
            ULog.WriteUGITLog(AppContextForThread.CurrentUser.Id, $"Import Initiated for {listName}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContextForThread.TenantID);

            Tuple<string, int, int> message = new Tuple<string, int, int>(string.Empty, 0, 0);
            List<string> error = new List<string>();
            List<string> missedValues = new List<string>();    //store invalid values found for department and roles
            DataTable spList = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            string errorMessage = string.Empty;
            if (listName == DatabaseObjects.Tables.RequestType)
                listModule = ObjModuleViewManager.LoadAllModule().Where(x => x.EnableModule).ToList();
            if (spList == null)
            {
                errorMessage = string.Format("List to be imported into [{0}] not found!", listName);
                ULog.WriteLog(errorMessage);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filePath);
            CellRange range = workbook.Worksheets[0].GetDataRange();
            DataTable data = workbook.Worksheets.ActiveWorksheet.CreateDataTable(range, true);
            RowCollection rows = workbook.Worksheets[0].Rows;
            if (data == null)
            {
                errorMessage = "No data found in file!";
                ULog.WriteLog(errorMessage);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }

            List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;
            List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
            List<FieldAliasCollection> objFieldAliasCollection = new List<FieldAliasCollection>();
            if (lstColumn == null || lstColumn.Count == 0 || range.RowCount <= 1)
            {
                errorMessage = "No data found in file!";
                ULog.WriteLog(errorMessage);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }

            #region Mandatory Check for fields
            List<string> mandatoryFieldAsPerList = new List<string>();
            if (listName == DatabaseObjects.Tables.RequestType)
                mandatoryFieldAsPerList.AddRange(new string[] { DatabaseObjects.Columns.TicketRequestType, "ModuleNameLookup" });
            else if (listName == DatabaseObjects.Tables.Location)
                mandatoryFieldAsPerList.AddRange(new string[] { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.AssetName });
            else if (listName == DatabaseObjects.Tables.Department)
                mandatoryFieldAsPerList.AddRange(new string[] { DatabaseObjects.Columns.Title });
            else if (listName == DatabaseObjects.Tables.ApplModuleRoleRelationship)
                mandatoryFieldAsPerList.AddRange(new string[] { DatabaseObjects.Columns.APPTitleLookup, DatabaseObjects.Columns.ApplicationModulesLookup, DatabaseObjects.Columns.ApplicationRoleLookup });

            objFieldAliasCollection = templstColumn.Where(r => r.ListName == listName || mandatoryFieldAsPerList.Contains(r.InternalName)).ToList();

            if (objFieldAliasCollection == null || objFieldAliasCollection.Count == 0)
            {
                errorMessage = string.Format("Import not configured for list {0}", listName);
                ULog.WriteLog(errorMessage);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }

            foreach (FieldAliasCollection fieldAlias in objFieldAliasCollection)
            {
                List<string> aliasName = fieldAlias.AliasNames.Split(',').ToList();
                if (aliasName == null || aliasName.Count == 0)
                    continue;
                if (!aliasName.Any(x => lstColumn.Contains(x)))
                {
                    mandatoryField = fieldAlias.InternalName;
                    //isCheckMandatoryField = false;
                    break;
                }
            }

            if (!isCheckMandatoryField)
            {
                errorMessage = string.Format("{1} fields are mandatory in excel for {0}", listName, mandatoryField);
                ULog.WriteLog(errorMessage);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }

            #endregion

            #region "Delete Existing Records"
            if (deleteExistingRecords)
            {

            }
            #endregion

            DataTable dtAppModuleRoleRelationShip = null;
            #region App module role relationship related code
            DataRow[] appAccessColl = null;
            if (listName == DatabaseObjects.Tables.ApplModuleRoleRelationship)
            {
                //Get existing access 
                string commonQuery = string.Empty;
                commonQuery = string.Format("<Where></Where>");
                DataRow[] commonColl = GetTableDataManager.GetTableData(listName).Select(commonQuery);
                dtAppModuleRoleRelationShip = new DataTable();
                dtAppModuleRoleRelationShip.Columns.AddRange(new DataColumn[] { new DataColumn("AccessId", typeof(int)), new DataColumn("AppId", typeof(int)), new DataColumn("AppModuleId", typeof(int)), new DataColumn("AppRoleId", typeof(int)), new DataColumn("AppAccessUserId", typeof(int)) });
                if (commonColl != null || commonColl.Count() > 0)
                {
                    appAccessColl = commonColl;
                    dtAppModuleRoleRelationShip = commonColl.CopyToDataTable();
                    dtAppModuleRoleRelationShip.Columns.AddRange(new DataColumn[] { new DataColumn("AccessId", typeof(int)), new DataColumn("AppId", typeof(int)), new DataColumn("AppModuleId", typeof(int)), new DataColumn("AppRoleId", typeof(int)), new DataColumn("AppAccessUserId", typeof(int)) });

                    for (int i = 0; i < dtAppModuleRoleRelationShip.Rows.Count; i++)
                    {
                        dtAppModuleRoleRelationShip.Rows[i]["AccessId"] = UGITUtility.StringToInt(UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.Id));
                        // lookupVal = new SPFieldLookupValue(UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.APPTitleLookup));
                        dtAppModuleRoleRelationShip.Rows[i]["AppId"] = UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.APPTitleLookup);
                        dtAppModuleRoleRelationShip.Rows[i]["AppModuleId"] = UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.ApplicationModulesLookup);// lookupVal.LookupId;
                        dtAppModuleRoleRelationShip.Rows[i]["AppRoleId"] = UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.ApplicationRoleLookup);// lookupVal.LookupId;
                        dtAppModuleRoleRelationShip.Rows[i]["AppAccessUserId"] = UGITUtility.GetSPItemValueAsString(commonColl[i], DatabaseObjects.Columns.ApplicationRoleAssign);// lookupVal.LookupId;
                    }
                }
            }
            #endregion

            Ticket dd = null;
            int numAdded = 0, numUpdated = 0, numSkipped = 0;
            List<string> lstItemSkipped = new List<string>();
            List<string> lstItemUpdated = new List<string>();
            //tuple have 4 item like appid,moduleId,roleId,accessUser 
            List<Tuple<int, int, int, int>> lstKeepCurrentImported = new List<Tuple<int, int, int, int>>();

            for (int i = 1; i <= lastIndex; i++)
            {
                Row rowData = rows[i];

                if (Module == null)
                {
                    DataTable insertDt = spList.Clone();
                    insertDt.CaseSensitive = true;
                    DataRow listItem = insertDt.NewRow();
                    string missedValue = uHelper.SetFilledValues(context, lstColumn, rowData, listItem, objFieldAliasCollection);
                    if (!string.IsNullOrEmpty(missedValue))
                    {
                        string missedvalueMessage = string.Format("Line # {0}: Skipped record with missing value {1}<BR>", i + 1, missedValue);
                        missedValues.Add(missedvalueMessage);
                        numSkipped++;
                        continue;
                    }
                    if (listName == DatabaseObjects.Tables.ApplModuleRoleRelationship)
                    {
                        if (UGITUtility.ObjectToString(spList.Rows[i - 1]["Title"]) == DatabaseObjects.Tables.ApplModuleRoleRelationship)
                        {
                            int appId = 0, appModuleId = 0, appRoleId = 0, appAccessUser = 0;
                            List<string> logMsg = new List<string>();
                            if (string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(listItem, DatabaseObjects.Columns.APPTitleLookup))))
                            {
                                numSkipped++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(listItem, DatabaseObjects.Columns.ApplicationModulesLookup))))
                            {
                                numSkipped++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(listItem, DatabaseObjects.Columns.ApplicationRoleLookup))))
                            {
                                numSkipped++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(listItem, DatabaseObjects.Columns.ApplicationRoleAssign))))
                            {
                                numSkipped++;
                                continue;
                            }

                            int accessId = UGITUtility.StringToInt(dtAppModuleRoleRelationShip.AsEnumerable().Where(x => x.Field<int>("AppId") == appId && x.Field<int>("AppModuleId") == appModuleId && x.Field<int>("AppRoleId") == appRoleId && x.Field<int>("AppAccessUserId") == appAccessUser).Select(y => y.Field<int>(DatabaseObjects.Columns.Id)).FirstOrDefault());
                            if (accessId > 0)
                            {
                                if (appAccessColl != null && appAccessColl.Count() > 0)
                                {
                                    ////DataRow appAccessItem = appAccessColl.Select(DatabaseObjects.Columns.ID + "=" + accessId)[0];
                                    ////if (appAccessItem != null)
                                    ////{
                                    ////    appAccessItem.AcceptChanges();
                                    ////    numUpdated++;
                                    ////}
                                    ////else
                                    ////    numSkipped++;
                                }
                                continue;
                            }

                            if (!lstKeepCurrentImported.Exists(x => x.Item1 == appId && x.Item2 == appModuleId && x.Item3 == appRoleId && x.Item4 == appAccessUser))
                                lstKeepCurrentImported.Add(new Tuple<int, int, int, int>(appId, appModuleId, appRoleId, appAccessUser));
                        }

                    }
                    if (listName == DatabaseObjects.Tables.ModuleMonthlyBudget)
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.TicketId].ToString()) && string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.AllocationStartDate].ToString()))
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing TicketId or Start Date value");
                            numSkipped++;
                            continue;
                        }
                        var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("TicketId") == insertDt.Rows[0][DatabaseObjects.Columns.TicketId].ToString() && r.Field<DateTime>("AllocationStartDate") == UGITUtility.GetObjetToDateTime(insertDt.Rows[0][DatabaseObjects.Columns.AllocationStartDate]));
                        if (firstDataTable.Any())
                        {
                            olddt = firstDataTable.CopyToDataTable();
                            uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                            numUpdated++;
                            continue;
                        }
                        else
                        {
                            GetTableDataManager.bulkupload(insertDt, listName);
                            numAdded++;
                            continue;
                        }
                    }
                    if (listName == DatabaseObjects.Tables.RequestType)
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (!string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.Category].ToString()) && !string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.RequestType].ToString()))
                        {
                            if (listModule.Count() > 0)
                            {
                                moduleTable = listModule.Where(x => x.Title == insertDt.Rows[0][DatabaseObjects.Columns.ModuleNameLookup].ToString() || x.ShortName == insertDt.Rows[0][DatabaseObjects.Columns.ModuleNameLookup].ToString()).ToList();
                                insertDt.Rows[0][DatabaseObjects.Columns.ModuleNameLookup] = moduleTable[0].ModuleName;
                            }
                            string subcat = insertDt.Rows[0][DatabaseObjects.Columns.SubCategory].ToString() == null ? string.Empty : insertDt.Rows[0][DatabaseObjects.Columns.SubCategory].ToString();
                            var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("RequestType") == insertDt.Rows[0][DatabaseObjects.Columns.RequestType].ToString() && r.Field<string>("ModuleNameLookup") == insertDt.Rows[0][DatabaseObjects.Columns.ModuleNameLookup].ToString() && r.Field<string>("Category") == insertDt.Rows[0][DatabaseObjects.Columns.Category].ToString() &&  r.Field<string>("SubCategory") == subcat && r.Field<Boolean>("Deleted")==false);
                           
                            if (firstDataTable.Any())
                            {
                                olddt = firstDataTable.CopyToDataTable();
                                uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                                numUpdated++;
                                continue;
                            }
                            else
                            {
                                GetTableDataManager.bulkupload(insertDt, listName);
                                numAdded++;
                                continue;
                            }
                        }
                        else
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing value");
                            numSkipped++;
                            continue;
                        } 
                        
                    }
                    if (listName == DatabaseObjects.Tables.BudgetCategories)
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.BudgetCategoryName].ToString()) && string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.BudgetSubCategory].ToString()))
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing {DatabaseObjects.Columns.BudgetCategoryName} value");
                            numSkipped++;
                            continue; 
                        }
                        var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("BudgetCategoryName") == insertDt.Rows[0][DatabaseObjects.Columns.BudgetCategoryName].ToString() && r.Field<string>("BudgetSubCategory") == insertDt.Rows[0][DatabaseObjects.Columns.BudgetSubCategory].ToString());
                        if (firstDataTable.Any())
                        {
                            olddt = firstDataTable.CopyToDataTable();
                            uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                            numUpdated++;
                            continue;
                        }
                        else
                        {
                            GetTableDataManager.bulkupload(insertDt, listName);
                            numAdded++;
                            continue;
                        }
                        
                    }
                    if(listName== DatabaseObjects.Tables.Department)
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.Title].ToString()) )
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing Title value");
                            numSkipped++;
                            continue;
                        }
                        
                        var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("Title") == insertDt.Rows[0][DatabaseObjects.Columns.Title].ToString() );
                        if (firstDataTable.Any())
                        {
                            olddt = firstDataTable.CopyToDataTable();
                            uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                            lstItemUpdated.Add(Convert.ToString(insertDt.Rows[0][DatabaseObjects.Columns.Title]));
                            numUpdated++;
                            continue;
                        }
                        else
                        {
                            Company firstCompany = CompanyMGR.Load().FirstOrDefault();
                            if (firstCompany != null)
                                listItem[DatabaseObjects.Columns.CompanyIdLookup] = firstCompany.ID;
                            GetTableDataManager.bulkupload(insertDt, listName);
                            numAdded++;
                            continue;
                        }
                        
                    }
                    if(listName == DatabaseObjects.Tables.GlobalRole)
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.Name].ToString()))
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing Name value");                       
                            numSkipped++;
                            continue;
                        }

                        var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("Name") == insertDt.Rows[0][DatabaseObjects.Columns.Name].ToString());
                        if (firstDataTable.Any())
                        {
                            olddt = firstDataTable.CopyToDataTable();
                            uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                            lstItemUpdated.Add(Convert.ToString(insertDt.Rows[0][DatabaseObjects.Columns.Name]));
                            numUpdated++;
                            continue;
                        }
                        else
                            GetTableDataManager.bulkupload(insertDt, listName);
                    }
                    else
                    {
                        olddt = insertDt.Clone();
                        insertDt.Rows.Add(listItem);
                        insertDt.AcceptChanges();
                        if (string.IsNullOrEmpty(insertDt.Rows[0][DatabaseObjects.Columns.Title].ToString()))
                        {
                            lstItemSkipped.Add($"Line # {i + 1}: Ignoring record with missing Name value");
                            numSkipped++;
                            continue;
                        }

                        var firstDataTable = spList.AsEnumerable().Where(r => r.Field<string>("Title") == insertDt.Rows[0][DatabaseObjects.Columns.Title].ToString());
                        if (firstDataTable.Any())
                        {
                            olddt = firstDataTable.CopyToDataTable();
                            uGITDAL.ExecuteNonQuery(UGITUtility.ReturnUpdateQuery(UGITUtility.UpdateDatatableRow(insertDt, olddt).Select()[0], listName));
                            lstItemUpdated.Add(Convert.ToString(insertDt.Rows[0][DatabaseObjects.Columns.Title]));
                            numUpdated++;
                            continue;
                        }
                        else
                            GetTableDataManager.bulkupload(insertDt, listName);
                    }
                    numAdded++;
                }
                else
                {
                    DataRow listItem = spList.NewRow();
                    List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                    uHelper.SetFilledValues(context, lstColumn, rowData, listItem, objFieldAliasCollection, formValues);
                    ////dd.SetItemValues(listItem, formValues, true, true, 0);
                    dd.SetItemValues(listItem, formValues, true, true, Convert.ToString(context.CurrentUser));
                    // dd.Create(spWeb, dd.Module.ID, listItem);
                    dd.Create(listItem, context.CurrentUser);

                    string innererror = string.Empty;
                    innererror = dd.CommitChanges(listItem, "", Request.Url);

                    if (!string.IsNullOrEmpty(innererror))
                    {
                        innererror = dd.CommitChanges(listItem, "", Request.Url);
                        if (!string.IsNullOrEmpty(innererror))
                            error.Add(innererror);
                    }
                }

                if (i <= lastIndex && i % 100 == 0)
                {

                }

            }
            //Refresh cache
            string cacheName = "Lookup_" + listName + "_" + context.TenantID;
            CacheHelper<object>.ClearWithRegion(cacheName);
            string msg = string.Format("Import from {0} successful: {1} new records imported, {2} updated, {3} skipped", Path.GetFileName(filePath), numAdded, numUpdated, numSkipped);
            if (error != null && error.Count > 0)
            {
                errorMessage = string.Format("{0}", string.Join<string>(",", error));
                ULog.WriteUGITLog(context.CurrentUser.Id, msg, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Import), context.TenantID);
                message = new Tuple<string, int, int>(errorMessage, 0, 0);
                return message;
            }
            else
            {
                message = new Tuple<string, int, int>(msg, 0, 0);
                string itemskippedmsg = string.Empty;
                if (missedValues.Count > 0)
                    msg = $"{msg}<br>Skipped Items are: {UGITUtility.ConvertListToString(missedValues, Constants.Separator6)}";
                message = new Tuple<string, int, int>(msg, 0, 0);
                ULog.WriteUGITLog(context.CurrentUser.Id, itemskippedmsg, Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), context.TenantID);
            }
            return message;


        }


    }
}
