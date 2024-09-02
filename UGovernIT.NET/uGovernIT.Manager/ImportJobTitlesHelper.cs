using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
using uGovernIT.Manager.RMM.ViewModel;
using System.Threading;
using System.Web;
using uGovernIT.Helpers;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Runtime.Remoting.Contexts;
using Microsoft.AspNet.Identity;

namespace uGovernIT.Manager
{
    public class ImportJobTitlesHelper
    {
        public static void ImportJobTitles(ApplicationContext context, string listName, string filePath, bool deleteExistingRecords, ref string errorMessage, ref string summary)
        {
            CompanyManager objCompanyManager = new CompanyManager(context);
            DepartmentManager objDepartmentManager = new DepartmentManager(context);
            CompanyDivisionManager objCompanyDivisionManager = new CompanyDivisionManager(context);
            ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(context);
            GlobalRoleManager globalRoleManager = new GlobalRoleManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            UserProfileManager userProfileManager = new UserProfileManager(context);

            int totalRecords = 0, recordsInserted = 0, recordsUpdated = 0, recordsMapped = 0, recordsNotMapped = 0, recordsNotProcessed = 0;
            List<(string, string)> departmentsNotMapped = new List<(string, string)>();

            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadDocument(filePath);
                Worksheet worksheet = workbook.Worksheets[0];
                CellRange dataRange = worksheet.GetDataRange();
                DataTable data = worksheet.CreateDataTable(dataRange, true);
                if (data == null)
                {
                    errorMessage = "No data found in file!";
                    ULog.WriteLog(errorMessage);
                    return;
                }

                List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
                int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;
                List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                List<FieldAliasCollection> objFieldAliasCollection = new List<FieldAliasCollection>();

                if (lstColumn == null || lstColumn.Count == 0 || dataRange.RowCount <= 1)
                {
                    errorMessage = "No data found in file!";
                    ULog.WriteLog(errorMessage);
                    return;
                }

                objFieldAliasCollection = templstColumn.Where(r => r.ListName == listName).ToList();

                if (objFieldAliasCollection == null || objFieldAliasCollection.Count == 0)
                {
                    errorMessage = string.Format("Import not configured for list {0}", listName);
                    ULog.WriteLog(errorMessage);
                    return;
                }

                FieldAliasCollection facItem;
                List<ImportJobTitle> jobTitles = new List<ImportJobTitle>();
                RowCollection rows = worksheet.Rows;
                for (int i = dataRange.TopRowIndex + 1; i <= dataRange.BottomRowIndex; i++)
                {
                    Row rowData = rows[i];

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("Title")).FirstOrDefault();
                    string title = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ShortName").FirstOrDefault();
                    string shortName = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "JobType").FirstOrDefault();
                    string jobType = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "RoleId").FirstOrDefault();
                    string role = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "LowRevenueCapacity").FirstOrDefault();
                    string lowRevenueCapacity = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "HighRevenueCapacity").FirstOrDefault();
                    string highRevenueCapacity = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "LowProjectCapacity").FirstOrDefault();
                    string lowProjectCapacity = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "HighProjectCapacity").FirstOrDefault();
                    string highProjectCapacity = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ResourceLevelTolerance").FirstOrDefault();
                    string resourceLevelTolerance = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "DepartmentId").FirstOrDefault();
                    string department = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Division").FirstOrDefault();
                    string division = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "CostRate").FirstOrDefault();
                    string costRate = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    jobTitles.Add(new ImportJobTitle
                    {
                        Title = title,
                        ShortName = shortName,
                        JobType = jobType,
                        Role = role,
                        LowRevenueCapacity = lowRevenueCapacity,
                        HighRevenueCapacity = highRevenueCapacity,
                        LowProjectCapacity = lowProjectCapacity,
                        HighProjectCapacity = highProjectCapacity,
                        ResourceLevelTolerance = resourceLevelTolerance,
                        Department = department,
                        Division = division,
                        CostRate = costRate
                    });
                }

                totalRecords = jobTitles.Count;

                //List<GlobalRole> globalRoles = globalRoleManager.Load();
                //List<Company> companies = objCompanyManager.GetCompanyData().Where(x => !x.Deleted).ToList();
                //List<Department> departments = objDepartmentManager.GetDepartmentData().Where(x => !x.Deleted).ToList();

                if (deleteExistingRecords)
                {
                    //De-reference user role from Resources
                    List<UserProfile> users = userProfileManager.Load(x => x.TenantID.ToLower() == context.TenantID.ToLower());
                    foreach (UserProfile userProfile in users)
                    {
                        userProfile.JobProfile = null;
                        userProfile.JobTitleLookup = 0;
                        userProfileManager.Update(userProfile);
                        userProfileManager.UpdateIntoCache(userProfile);
                    }
                    context.UserManager.RefreshCache();

                    //Delete Job Titles
                    List<JobTitle> titles = jobTitleManager.Load(x => x.TenantID.ToLower() == context.TenantID.ToLower());
                        jobTitleManager.Delete(titles);
                }

                List<CompanyDivision> divisions = objCompanyDivisionManager.GetCompanyDivisionDataWithAllDepartments().Where(x => !x.Deleted).ToList();

                foreach (var jobTitle in jobTitles)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(jobTitle.Title))
                        {
                            recordsNotProcessed += 1;
                            continue;
                        }

                        //Check JobTitle
                        string roleID = null;
                        if (!string.IsNullOrWhiteSpace(jobTitle.Role))
                        {
                            var foundGlobalRole = globalRoleManager.Get(r => !string.IsNullOrWhiteSpace(r.Name) && r.Name.Trim().ToLower() == jobTitle.Role.ToLower() && !r.Deleted && r.TenantID.ToLower() == context.TenantID.ToLower());
                            if (foundGlobalRole != null)
                                roleID = foundGlobalRole.Id;
                        }
                        JobTitle foundJobTitle = jobTitleManager.Load(x => x.Title.ToLower() == jobTitle.Title.ToLower() && x.Deleted == false && x.TenantID.ToLower() == context.TenantID.ToLower()).FirstOrDefault();

                        Dictionary<string, object> arrParams = new Dictionary<string, object>();
                        arrParams.Add("@Id", foundJobTitle == null ? 0 : foundJobTitle.ID);
                        arrParams.Add("@Title", jobTitle.Title);
                        arrParams.Add("@Shortname", string.IsNullOrWhiteSpace(jobTitle.ShortName) ? jobTitle.Title: jobTitle.ShortName);
                        arrParams.Add("@JobType", string.IsNullOrWhiteSpace(jobTitle.JobType) ? "Overhead" : jobTitle.JobType);
                        arrParams.Add("@LowProjectCapacity", UGITUtility.StringToLong(jobTitle.LowProjectCapacity));
                        arrParams.Add("@HighProjectCapacity", UGITUtility.StringToLong(jobTitle.HighProjectCapacity));
                        arrParams.Add("@LowRevenueCapacity", UGITUtility.StringToLong(jobTitle.LowRevenueCapacity));
                        arrParams.Add("@HighRevenueCapacity", UGITUtility.StringToLong(jobTitle.HighRevenueCapacity));
                        //arrParams.Add("@EmpCostRate", Convert.ToInt32(txtECR.Text));
                        arrParams.Add("@ResourceLevelTolerance", UGITUtility.StringToLong(jobTitle.ResourceLevelTolerance));
                        arrParams.Add("@RoleId", roleID);
                        //arrParams.Add("@DepartmentId", departmentCtr.GetValues()==null ?spitem.DepartmentId: Convert.ToInt64(departmentCtr.GetValues()));
                        arrParams.Add("@Deleted", false);
                        arrParams.Add("@TenantID", context.TenantID);
                        if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertJobtitle", arrParams) > 0)
                        {
                            if (foundJobTitle == null)
                                recordsInserted += 1;
                            else
                            {
                                //if (!string.IsNullOrWhiteSpace(roleID) && ((!string.IsNullOrWhiteSpace(foundJobTitle.RoleId) && foundJobTitle.RoleId != roleID) || string.IsNullOrWhiteSpace(foundJobTitle.RoleId)))
                                //{
                                    List<UserProfile> users = userProfileManager.Load(x => x.JobTitleLookup == foundJobTitle.ID && x.Enabled == true);
                                    foreach (UserProfile usr in users)
                                    {
                                        userProfileManager.UpdateIntoCache(usr);
                                    }
                                //}
                                recordsUpdated += 1;
                            }
                        }

                        JobTitle updatedJobTitle = jobTitleManager.Load(x => x.Title.ToLower() == jobTitle.Title.ToLower() && x.Deleted == false && x.TenantID.ToLower() == context.TenantID.ToLower()).FirstOrDefault();
                        if (updatedJobTitle == null)
                        {
                            recordsNotProcessed += 1;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(jobTitle.Division) || string.IsNullOrWhiteSpace(jobTitle.Department))
                        {
                            //recordsNotProcessed += 1;
                            continue;
                        }

                        var foundDivision = divisions.Where(d => d.TenantID.ToLower() == context.TenantID.ToLower() && !d.Deleted && d.Title.Trim().ToLower() == jobTitle.Division.ToLower()).FirstOrDefault();
                        if (foundDivision != null)
                        {
                            var foundDepartment = foundDivision.Departments.Where(d => d.TenantID.ToLower() == context.TenantID.ToLower() && d.Title.Trim().ToLower() == jobTitle.Department.ToLower()).FirstOrDefault();
                            if (foundDepartment != null)
                            {

                                long deptID = foundDepartment.ID;

                                arrParams.Clear();

                                arrParams.Add("@TenantID", context.TenantID);
                                arrParams.Add("@deptid", deptID);
                                //arrParams.Add("@roleid", Role);
                                arrParams.Add("@JobTitleId", updatedJobTitle.ID);
                                arrParams.Add("@EmpCostRate", UGITUtility.StringToDouble(jobTitle.CostRate));
                                arrParams.Add("@Deleted", false);
                                DataSet dtResultBillings = DAL.uGITDAL.ExecuteDataSet_WithParameters("usp_GetJobTitle", arrParams);

                                int id = 0;
                                if (dtResultBillings != null && dtResultBillings.Tables[0].Rows.Count > 0)
                                {
                                    id = Convert.ToInt32(dtResultBillings.Tables[0].Rows[0]["ID"]);
                                }

                                arrParams.Clear();

                                arrParams.Add("@id", id);
                                arrParams.Add("@JobTitleId", updatedJobTitle.ID);
                                arrParams.Add("@EmpCostRate", UGITUtility.StringToDouble(jobTitle.CostRate));
                                //arrParams.Add("@RoleId", Convert.ToString(Roleid) == "" ? Convert.ToString(ddlRole.SelectedValue) : Roleid);
                                arrParams.Add("@DepartmentId", Convert.ToInt64(deptID));
                                arrParams.Add("@Deleted", false);
                                arrParams.Add("@TenantID", context.TenantID);
                                if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertDepartmentJobtitleMapping", arrParams) > 0)
                                {
                                    recordsMapped += 1;
                                }
                            }
                            else
                            {
                                recordsNotMapped += 1;
                                AddToDepartmentsNotMapped(departmentsNotMapped, jobTitle);
                            }
                        }
                        else
                        {
                            recordsNotMapped += 1;
                            AddToDepartmentsNotMapped(departmentsNotMapped, jobTitle);
                        }
                    }
                    catch (Exception e)
                    {
                        recordsNotProcessed += 1;
                        ULog.WriteException(e, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ULog.WriteLog(errorMessage);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<b>Total Records from Excel:</b> {totalRecords}\n");
            stringBuilder.Append($"<b>Job Titles Created:</b> {recordsInserted}\n");
            stringBuilder.Append($"<b>Job Titles Updated:</b> {recordsUpdated}\n");
            stringBuilder.Append($"<b>Job Titles Mapped with Department:</b> {recordsMapped}\n");
            stringBuilder.Append($"<b>Job Titles not mapped with Department (not found):</b> {recordsNotMapped}\n");
            if (departmentsNotMapped.Count > 0)
            {
                stringBuilder.Append($"<b>Department (not mapped):</b>\n");
            }
            foreach (var item in departmentsNotMapped)
            {
                stringBuilder.Append($"\t{item.Item1} - {item.Item2}\n");
            }
            stringBuilder.Append($"<b>Job Titles not processed (error occurred):</b> {recordsNotProcessed}\n");

            summary = stringBuilder.ToString();
        }

        private static void AddToDepartmentsNotMapped(List<(string, string)> departmentsNotMapped, ImportJobTitle jobTitle)
        {
            if (!departmentsNotMapped.Any(d => d.Item1 == jobTitle.Division && d.Item2 == jobTitle.Department))
                departmentsNotMapped.Add((jobTitle.Division, jobTitle.Department));
        }
    }

    public class ImportJobTitle
    {
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string JobType { get; set; }
        public string Role { get; set; }
        public string LowRevenueCapacity { get; set; }
        public string HighRevenueCapacity { get; set; }
        public string LowProjectCapacity { get; set; }
        public string HighProjectCapacity { get; set; }
        public string ResourceLevelTolerance { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string CostRate { get; set; }
    }
}
