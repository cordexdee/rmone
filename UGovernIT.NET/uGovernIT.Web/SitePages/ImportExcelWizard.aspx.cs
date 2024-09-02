using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using uGovernIT.Util.Cache;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity;
using System.Threading;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ImportExcelWizard : System.Web.UI.Page
    {
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        CompanyManager companyManager = new CompanyManager(HttpContext.Current.GetManagerContext());
        DepartmentManager departmentManager = new DepartmentManager(HttpContext.Current.GetManagerContext());
        CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(HttpContext.Current.GetManagerContext());
        StudioManager studioManagerObj = new StudioManager(HttpContext.Current.GetManagerContext());

        JobTitleManager jobTitleManagerObj = new JobTitleManager(HttpContext.Current.GetManagerContext());
        GlobalRoleManager globalroleManagerObj = new GlobalRoleManager(HttpContext.Current.GetManagerContext());
        
        
        public string moduletypejson { get; set; }
        public string TenantAccountID { get; set; }
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            TenantAccountID = AppContext.TenantAccountId;
            string configModuleTypes = configManager.GetValue("ModuleTypes");
            Dictionary<string, string> lstmodules = UGITUtility.DeserializeDicObjects(configModuleTypes, ";", ":");
            //moduletypejson = UGITUtility.GetJsonForDictionary(lstmodules);
            moduletypejson = JsonConvert.SerializeObject(lstmodules.Select(x => new { id = x.Key, name=x.Value }));
            MainMaster mainMasterPage = Page.Master as MainMaster;
            if(mainMasterPage != null)
                mainMasterPage.IsHideMenuBar = true;

        }

        protected void btnImportExcel_Click(object sender, EventArgs e)
        {
            if(updExcelFile.UploadedFiles.Length > 0)
            {
                // string CurrentFilePath = Server.MapPath(updExcelFile.UploadedFiles[0].FileName);
                string FileName = Path.GetFileName(updExcelFile.UploadedFiles[0].FileName);
                string FilePath = string.Format(@"{0}/{1}", uHelper.GetTempFolderPath(), FileName);
                updExcelFile.UploadedFiles[0].SaveAs(FilePath);
                DataTable dtExcelSheet = GetDataTableFromExcel(FilePath, "Project Categories");
                if(dtExcelSheet != null && dtExcelSheet.Rows.Count > 1)
                    UpdateDivisonandStudio(dtExcelSheet);
                try
                {
                    DataTable dtProjectMetaData = GetDataTableFromExcel(FilePath, "ProjectMetaData");
                    if(dtProjectMetaData != null && dtProjectMetaData.Rows.Count > 0)
                    {
                        string sqlConnectionString = UGITUtility.ObjectToString(ConfigurationManager.ConnectionStrings["cnn"]);
                        SqlBulkCopy bulkInsert = new SqlBulkCopy(sqlConnectionString);
                        bulkInsert.DestinationTableName = "ProjectMetaData$";
                        bulkInsert.WriteToServer(dtProjectMetaData);
                        //MsgAlert.Text = “Product uploaded successfully”;

                        //Create Tickets from "ProjectMetaData$" into module tables
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        values.Add("@tenantID", AppContext.TenantID);
                        DAL.uGITDAL.ExecuteDataSetWithParameters("UpdateProjectMetaData", values);

                        RegisterCache.ReloadTicketsCache(AppContext);

                        lblProjectData.Text = "Excel Upload Successful!";
                    }
                                     
                }
                catch (Exception ex)
                {
                    uGovernIT.Util.Log.ULog.WriteException(ex, "Import Excel Failed!" );
                }
            }
        }

        protected void btnImportcompanyinfo_Click(object sender, EventArgs e)
        {
            JobTitleManager jobTitleManager = new JobTitleManager(AppContext);
            GlobalRoleManager roleManager = new GlobalRoleManager(AppContext);
            DepartmentManager deptManager = new DepartmentManager(AppContext);
            if (updCompanyInfo.UploadedFiles.Length > 0)
            {
                try
                {
                    string FileName = Path.GetFileName(updCompanyInfo.UploadedFiles[0].FileName);
                    string FilePath = string.Format(@"{0}{1}", uHelper.GetTempFolderPath(), FileName);
                    updCompanyInfo.UploadedFiles[0].SaveAs(FilePath);
                    DataTable dtExcelSheet = GetDataTableFromExcel(FilePath, "ResourceProfile");
                    if (dtExcelSheet != null && dtExcelSheet.Rows.Count > 0)
                    {
                        //Clear exiting data for job title
                        List<JobTitle> lstJobTitles = new List<JobTitle>();
                        lstJobTitles = jobTitleManager.Load();
                        if(lstJobTitles != null && lstJobTitles.Count > 0)
                        {
                            jobTitleManager.Delete(lstJobTitles);
                        }
                        Company companyObj = companyManager.Load().FirstOrDefault();
                        foreach (DataRow row in dtExcelSheet.Rows)
                        {
                            JobTitle newJobTitle = new JobTitle();
                            newJobTitle.Title = UGITUtility.ObjectToString(row["Title"]);
                            newJobTitle.LowRevenueCapacity = UGITUtility.StringToDouble(row["LowRevenueCapacity"]);
                            newJobTitle.HighRevenueCapacity = UGITUtility.StringToDouble(row["HighRevenueCapacity"]);
                            newJobTitle.LowProjectCapacity = UGITUtility.StringToInt(row["LowProjectCapacity"]);
                            newJobTitle.HighProjectCapacity = UGITUtility.StringToInt(row["HighProjectCapacity"]);
                            newJobTitle.BillingLaborRate = UGITUtility.StringToDouble(row["BillingLaborRate"]);
                            newJobTitle.EmployeeCostRate = UGITUtility.StringToDouble(row["EmployeeCostRate"]);
                            newJobTitle.ResourceLevelTolerance = UGITUtility.StringToInt(row["ResourceLevelTolerance"]);
                            newJobTitle.JobType = UGITUtility.ObjectToString(row["JobType"]);

                            string rolename = UGITUtility.ObjectToString(row["RoleName"]);
                            if (!string.IsNullOrEmpty(rolename))
                            {
                                GlobalRole roleObj = roleManager.Load(x => x.Name == rolename).FirstOrDefault();
                                if (roleObj != null)
                                    newJobTitle.RoleId = roleObj.Name;
                                else
                                {
                                    roleObj = new GlobalRole() { Name = rolename, Description = rolename };
                                    roleManager.Insert(roleObj);
                                    newJobTitle.RoleId = roleObj.Id;
                                }
                            }

                            string department = UGITUtility.ObjectToString(row["DepartmentName"]);
                            if (!string.IsNullOrEmpty(department))
                            {
                                List<Department> departmentLst = deptManager.Load(x => x.Title == department);
                                if(department != null && departmentLst.Count > 0)
                                {
                                    foreach (Department item in departmentLst)
                                    {
                                        JobTitle copyJobTitleObj = newJobTitle;
                                        copyJobTitleObj.DepartmentId = item.ID;
                                        copyJobTitleObj.ID = 0;
                                        jobTitleManager.Insert(copyJobTitleObj);
                                    }
                                }
                                else
                                {
                                    jobTitleManager.Insert(newJobTitle);
                                }
                                
                            }

                        }
                    }

                    string defaultPassword = configManager.GetValue(ConfigConstants.DefaultPassword);
                    DataTable dtResourceUsers = GetDataTableFromExcel(FilePath, "Resource");

                    List<UserProfile> lstUserProfiles = AppContext.UserManager.GetUsersProfile();
                    foreach(UserProfile deleteUser in lstUserProfiles)
                    {
                        if(deleteUser.UserName != "Administrator_Platinum")
                        {
                            AppContext.UserManager.DeleteUserById(deleteUser.Id);
                        }
                    }
                    List<JobTitle> JobTitlesLst = jobTitleManagerObj.loadJobTitles();

                    DateTime minDate = new DateTime(2014, 01, 01);
                    DateTime maxDate = new DateTime(8900, 12, 31);
                    List<string> successfullNames = new List<string>();
                    List<string> unsuccessfullNames = new List<string>();
                    foreach (DataRow row in dtResourceUsers.Rows)
                    {
                        JobTitle jobTitleObj = JobTitlesLst.Where(x => x.Title == UGITUtility.ObjectToString(row["Title"]) && x.DepartmentDescription.Contains(UGITUtility.ObjectToString(row["Division"]))).FirstOrDefault();
                        GlobalRole globalRoleObj = globalroleManagerObj.Load(x => x.Name == UGITUtility.ObjectToString(row["Preferred Role"])).FirstOrDefault();
                        Studio studioObj = studioManagerObj.Load(x => x.Description == UGITUtility.ObjectToString(row["Division"]) + " > " + UGITUtility.ObjectToString(row["Studio"])).FirstOrDefault();
                        UserProfile userCreated = new UserProfile() {
                            UserName = UGITUtility.ObjectToString(row["ID"]),
                            //Email = UGITUtility.ObjectToString(row[""]), 
                            Enabled = true,
                            Name = UGITUtility.ObjectToString(row["Resource"]),
                            EmployeeId = UGITUtility.ObjectToString(row["ID"]),
                            JobProfile = jobTitleObj?.Title,
                            JobTitleLookup = jobTitleObj == null ? 0 : jobTitleObj.ID,
                            Department = jobTitleObj == null ? "0" : UGITUtility.ObjectToString(jobTitleObj.DepartmentId),
                            UGITStartDate = minDate,
                            UGITEndDate = maxDate,
                            GlobalRoleId = globalRoleObj?.Id,
                            StudioLookup = studioObj == null ? 0 : studioObj.ID,
                            TenantID = AppContext.TenantID 
                        };

                        IdentityResult
                            result = AppContext.UserManager.Create(userCreated, defaultPassword);
                        
                        if (result.Succeeded == false)
                        {
                            unsuccessfullNames.Add(userCreated.Name);
                        }
                        if (result.Succeeded == true)
                        {
                            successfullNames.Add(userCreated.Name);
                        }
                    }
                    

                    lblcompanyinfo.Text = "Excel Upload Successful!";

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }

        private void UpdateDivisonandStudio(DataTable dtExcelSheet)
        {

            FieldConfigurationManager fieldManager = new FieldConfigurationManager(AppContext);

            List<string> lstDivision = dtExcelSheet.AsEnumerable().Select(x => x.Field<string>("Division")).ToList();
            lstDivision.RemoveAll(x => x == null);
            List<string> lstStudio = dtExcelSheet.AsEnumerable().Select(x => x.Field<string>("Studio")).ToList();
            lstStudio.RemoveAll(x => x == null);
            List<string> lstDepartments = dtExcelSheet.AsEnumerable().Select(x => x.Field<string>("Department")).ToList();
            lstDepartments.RemoveAll(x => x == null);
            List<string> lstProjectType = dtExcelSheet.AsEnumerable().Select(x => x.Field<string>("Project Type")).ToList();
            lstProjectType.RemoveAll(x => x == null);
            List<string> lstProjectGroup = dtExcelSheet.AsEnumerable().Select(x => x.Field<string>("Project Group")).ToList();
            lstProjectGroup.RemoveAll(x => x == null);

            if(lstDivision != null && lstDivision.Count > 0)
            {
                DepartmentManager departmentManager = new DepartmentManager(AppContext);
                CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(AppContext);
                StudioManager studioManager = new StudioManager(AppContext);
                //Clear existing data if any 
                List<Department> departments = departmentManager.Load();
                List<CompanyDivision> divisions = companyDivisionManager.Load();
                List<Studio> studios = studioManager.Load();

                //clear department, division and studios before entering new records
                if (divisions != null && studios != null && departments != null)
                {
                    foreach (Studio s in studios)
                        studioManager.Delete(s);
                    foreach (CompanyDivision d in divisions)
                        companyDivisionManager.Delete(d);
                    foreach (Department dept in departments)
                        departmentManager.Delete(dept);
                }

                Company companyObj = new Company() { Name = AppContext.TenantAccountId, Title = AppContext.TenantAccountId, Description = AppContext.TenantAccountId };
                companyManager.Insert(companyObj);
                foreach (string item in lstDivision)
                {
                    /*
                     * The platinum company has divisions.  Each division has studios.  Each division also has departments. 
                        For simplicity  each division having identical studios and each division having identical departments.  
                     */
                    CompanyDivision divisionObj = new CompanyDivision() { CompanyIdLookup = companyObj.ID, Description = item, Title = item };
                    long updatedDivision = companyDivisionManager.Insert(divisionObj);
                    if(updatedDivision > 0 && lstStudio != null && lstStudio.Count > 0)
                    {
                        foreach (string studio in lstStudio)
                        {
                            Studio studioObj = new Studio() { Title = studio, Description = $"{item} > {studio}", DivisionLookup = divisionObj.ID };
                            long updatedStudio = studioManager.Insert(studioObj);
                        }
                    }

                    if(updatedDivision > 0 && lstDepartments != null && lstDepartments.Count > 0)
                    {
                        foreach (string dept in lstDepartments)
                        {
                            Department departmentObj = new Department() { Title = dept, DepartmentDescription = item + " > " + dept, CompanyIdLookup = companyObj.ID, DivisionIdLookup = divisionObj.ID };
                            long updatedDepartment = departmentManager.Insert(departmentObj);
                        }
                    }
                }
            }

            if(lstProjectType != null && lstProjectType.Count > 0)
            {
                List<FieldConfiguration> lstSectorFields = fieldManager.Load(x => x.FieldName == DatabaseObjects.Columns.BCCISector);
                if(lstSectorFields != null && lstSectorFields.Count > 0)
                {
                    foreach(FieldConfiguration sectorField in lstSectorFields)
                    {
                        sectorField.Data = UGITUtility.ConvertListToString(lstProjectType, uGovernIT.Utility.Constants.Separator);
                    }
                    fieldManager.UpdateItems(lstSectorFields);
                }
            }

            if(lstProjectGroup != null && lstProjectGroup.Count > 0)
            {
                List<FieldConfiguration> lstProjectGroupObjs = fieldManager.Load(x => x.FieldName == "OpportunityTypeChoice");
                if (lstProjectGroupObjs != null && lstProjectGroupObjs.Count > 0)
                {
                    foreach (FieldConfiguration projectGroup in lstProjectGroupObjs)
                    {
                        projectGroup.Data = UGITUtility.ConvertListToString(lstProjectGroup, uGovernIT.Utility.Constants.Separator);
                    }
                    fieldManager.UpdateItems(lstProjectGroupObjs);
                }
            }
        }

        private static DataTable GetDataTableFromExcel(string FilePath, string sheetName)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(FilePath);
            Worksheet worksheet = workbook.Worksheets[sheetName];
            CellRange dataRange = worksheet.GetDataRange();
            DataTable data = worksheet.CreateDataTable(dataRange, true);

            CellRange range = workbook.Worksheets[sheetName].GetDataRange();
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

            return data;
        }

        protected void btnAllocationInfo_Click(object sender, EventArgs e)
        {
            ProjectEstimatedAllocationManager EstimatedAllocationManager = new ProjectEstimatedAllocationManager(AppContext);
            DataTable dtProjects = new DataTable();
            dtProjects.Columns.Add("Title", typeof(string));
            dtProjects.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            if (updAllocationInfo.UploadedFiles.Length > 0)
            {
                string FileName = Path.GetFileName(updAllocationInfo.UploadedFiles[0].FileName);
                string FilePath = string.Format(@"{0}/{1}", uHelper.GetTempFolderPath(), FileName);
                updAllocationInfo.UploadedFiles[0].SaveAs(FilePath);
                DataTable dtExcelSheet = GetDataTableFromExcel(FilePath, "Allocation");
                List<UserProfile> lstUserprofiles = AppContext.UserManager.GetUsersProfile();

                Ticket ticketManager = new Ticket(AppContext, "CPR");
                TicketManager ticketManagerObj = new TicketManager(AppContext);
                DataTable dtProjectTickets = ticketManagerObj.GetAllTicketsByModuleName(ModuleNames.CPR);
                DataTable dtOpportunityTickets = ticketManagerObj.GetAllTicketsByModuleName(ModuleNames.OPM);
                dtProjects.Merge(dtProjectTickets);
                dtProjects.Merge(dtOpportunityTickets);
                List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();

                foreach  (DataRow row in dtExcelSheet.Rows)
                {
                    UserProfile user = lstUserprofiles.Where(x => x.EmployeeId == UGITUtility.ObjectToString(row["Resource ID"])).FirstOrDefault();
                    DataRow[] projectlist = dtProjects.Select($"Title = '{UGITUtility.ObjectToString(row["Project"])}'");
                    string ticketID = string.Empty;
                    if (projectlist != null && projectlist.Count() > 0)
                        ticketID = UGITUtility.ObjectToString( projectlist[0][DatabaseObjects.Columns.TicketId]);
                    GlobalRole role = globalroleManagerObj.Load(x=>x.Name == UGITUtility.ObjectToString(row["Role"])).FirstOrDefault();
                    double pct = UGITUtility.StringToDouble(row["Allocation"]);
                    DateTime StartDate = UGITUtility.StringToDateTime(row["Start"]);
                    DateTime EndDate = UGITUtility.StringToDateTime(row["End"]);

                    if(user != null && ticketID != null && role != null && pct > 0 && StartDate >= DateTime.MinValue && EndDate >= DateTime.MinValue)
                    {
                        ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                        crmAllocation.AllocationStartDate = StartDate;
                        crmAllocation.AllocationEndDate = EndDate;

                        int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(AppContext, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                        int noOfWeeks = uHelper.GetWeeksFromDays(AppContext, noOfWorkingDays);

                        crmAllocation.AssignedTo = user.Id;
                        crmAllocation.PctAllocation = pct;
                        crmAllocation.Type = role.Id;
                        crmAllocation.Duration = noOfWeeks;
                        //crmAllocation.Title = allocation.Title;
                        crmAllocation.TicketId = ticketID;
                        EstimatedAllocationManager.Insert(crmAllocation);

                        lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue, StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue, Percentage = crmAllocation.PctAllocation, UserId = crmAllocation.AssignedTo, RoleTitle = role.Name, ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID), RoleId = crmAllocation.Type });

                    }
                }

            }
        }

        protected void btnUpdateResourceToProjectAllocInfo_Click(object sender, EventArgs e)
        {
            try
            {
                TicketManager ticketManagerObj = new TicketManager(AppContext);
                DataTable dtProjectTickets = ticketManagerObj.GetAllTicketsByModuleName(ModuleNames.CPR);
                DataTable dtOpportunityTickets = ticketManagerObj.GetAllTicketsByModuleName(ModuleNames.OPM);
                DataTable dtServiceTickets = ticketManagerObj.GetAllTicketsByModuleName(ModuleNames.CNS);

                ThreadStart threadStart = delegate () {
                    if (dtProjectTickets.Rows.Count > 0)
                        UpdateProjectGroups(dtProjectTickets, ModuleNames.CPR);

                    if (dtOpportunityTickets.Rows.Count > 0)
                        UpdateProjectGroups(dtOpportunityTickets, ModuleNames.OPM);

                    if (dtServiceTickets.Rows.Count > 0)
                        UpdateProjectGroups(dtServiceTickets, ModuleNames.CNS);

                    Util.Log.ULog.WriteUGITLog(AppContext.CurrentUser.Id, $"Update Resource To Projects, completed.", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Import), AppContext.TenantID);
                };
                Thread thread = new Thread(threadStart);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        private void UpdateProjectGroups(DataTable dtTickets, string module)
        {
            ProjectEstimatedAllocationManager EstimatedAllocationManager = new ProjectEstimatedAllocationManager(AppContext);
            ProjectEstimatedAllocationManager crmManager = new ProjectEstimatedAllocationManager(AppContext);
            List<ProjectEstimatedAllocation> alloc = new List<ProjectEstimatedAllocation>();

            foreach (DataRow item in dtTickets.Rows)
            {
                alloc = EstimatedAllocationManager.Load(x => x.TicketId == Convert.ToString(item[DatabaseObjects.Columns.TicketId]));
                if (alloc != null && alloc.Count > 0)
                {
                    crmManager.UpdateProjectGroups(module, Convert.ToString(item[DatabaseObjects.Columns.TicketId]), alloc);
                }
            }
        }
    }
}