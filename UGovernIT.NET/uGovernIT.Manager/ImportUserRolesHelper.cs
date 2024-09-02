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
    public class ImportUserRolesHelper
    {
        public static void ImportUserRoles(ApplicationContext context, string listName, string filePath, bool deleteExistingRecords, ref string errorMessage, ref string summary)
        {
            CompanyManager objCompanyManager = new CompanyManager(context);
            DepartmentManager objDepartmentManager = new DepartmentManager(context);
            CompanyDivisionManager objCompanyDivisionManager = new CompanyDivisionManager(context);
            ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(context);
            GlobalRoleManager globalRoleManager = new GlobalRoleManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            UserProfileManager userProfileManager = new UserProfileManager(context);

            int totalRecords = 0, recordsInserted = 0, recordsUpdated = 0, recordsMapped = 0, recordsNotMapped = 0, recordsNotProcessed = 0;
            List<(string,string)> departmentsNotMapped = new List<(string,string)>();
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
                List<ImportUserRole> userRoles = new List<ImportUserRole>();
                RowCollection rows = worksheet.Rows;
                for (int i = dataRange.TopRowIndex + 1; i <= dataRange.BottomRowIndex; i++)
                {
                    Row rowData = rows[i];

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("Name")).FirstOrDefault();
                    string name = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ShortName").FirstOrDefault();
                    string shortName = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Description").FirstOrDefault();
                    string description = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Division").FirstOrDefault();
                    string division = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Department").FirstOrDefault();
                    string department = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "BillingRate").FirstOrDefault();
                    string billingRate = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    userRoles.Add(new ImportUserRole
                    {
                         Name = name,
                         ShortName = shortName,
                         Description = description,
                         Division = division,
                         Department = department,
                         BillingRate = billingRate
                    });
                }

                totalRecords = userRoles.Count;

                //List<GlobalRole> globalRoles = globalRoleManager.Load();
                //List<Company> companies = objCompanyManager.GetCompanyData().Where(x => !x.Deleted).ToList();
                //List<Department> departments = objDepartmentManager.GetDepartmentData().Where(x => !x.Deleted).ToList();
                
                if (deleteExistingRecords)
                {
                    //No UserRoleLookup found in User role mapping

                    //De-reference user role from Job Title
                    List<JobTitle> jobTitles = jobTitleManager.Load(x => x.TenantID.ToLower() == context.TenantID.ToLower());
                    foreach (JobTitle jobTitle in jobTitles)
                        jobTitle.RoleId = null;
                    jobTitleManager.UpdateItems(jobTitles);

                    //De-reference user role from Resources
                    List<UserProfile> users = userProfileManager.Load(x => x.TenantID.ToLower() == context.TenantID.ToLower());
                    foreach (UserProfile userProfile in users)
                    {
                        userProfile.GlobalRoleId = null;
                        userProfile.UserRoleId = null;
                        userProfileManager.Update(userProfile);
                        userProfileManager.UpdateIntoCache(userProfile);
                    }
                    context.UserManager.RefreshCache();
                    
                    //Delete User Roles 
                    List<GlobalRole> roles = globalRoleManager.Load(r => r.TenantID.ToLower() == context.TenantID.ToLower());
                    foreach (string roleID in roles.Select(x => x.Id).ToList())
                        globalRoleManager.MapUserRoles(roleID, true); //Delete User Role Mapping
                    globalRoleManager.Delete(roles);

                    
                }

                List<CompanyDivision> divisions = objCompanyDivisionManager.GetCompanyDivisionDataWithAllDepartments().Where(x => !x.Deleted).ToList();

                foreach (var userRole in userRoles)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(userRole.Name))
                        {
                            recordsNotProcessed += 1;
                            continue;
                        }

                        //Check UserRole

                        var foundRole = globalRoleManager.Get(r => !string.IsNullOrWhiteSpace(r.Name) && r.Name.Trim().ToLower() == userRole.Name.ToLower() && !r.Deleted && r.TenantID.ToLower() == context.TenantID.ToLower());

                        GlobalRole globalRole = foundRole == null ? new GlobalRole() : foundRole;
                        globalRole.Name = userRole.Name;
                        globalRole.ShortName = string.IsNullOrWhiteSpace(userRole.ShortName) ? userRole.Name : userRole.ShortName;
                        string fieldname = Regex.Replace(globalRole.Name, @"[^\w\d]", "");
                        if (!string.IsNullOrEmpty(fieldname))
                            globalRoleManager.addNewUserField(fieldname + "User");
                        globalRole.FieldName = fieldname + "User";
                        globalRole.Description = userRole.Description;
                        globalRole.Deleted = false;

                        if (foundRole == null)
                        {
                            globalRole.Id = UGITUtility.ObjectToString(Guid.NewGuid());
                            globalRoleManager.Insert(globalRole);
                            globalRoleManager.MapUserRoles(globalRole.Id);
                            recordsInserted += 1;
                        }
                        else
                        {
                            globalRoleManager.Update(globalRole);
                            globalRoleManager.MapUserRoles(globalRole.Id);
                            recordsUpdated += 1;
                        }

                        if (string.IsNullOrWhiteSpace(userRole.Division) || string.IsNullOrWhiteSpace(userRole.Department))
                        {
                            //recordsNotProcessed += 1;
                            continue;
                        }

                        var foundDivision = divisions.Where(d => d.TenantID.ToLower() == context.TenantID.ToLower() && !d.Deleted && d.Title.Trim().ToLower() == userRole.Division.ToLower()).FirstOrDefault();
                        if (foundDivision != null)
                        {
                            var foundDepartment = foundDivision.Departments.Where(d => d.TenantID.ToLower() == context.TenantID.ToLower() && d.Title.Trim().ToLower() == userRole.Department.ToLower()).FirstOrDefault();
                            if (foundDepartment != null)
                            {
                                long? deptID = foundDepartment.ID;
                                string roleID = globalRole.Id;

                                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                                arrParams.Add("@TenantID", context.TenantID);
                                arrParams.Add("@deptid", deptID);
                                arrParams.Add("@roleid", roleID);
                                arrParams.Add("@BillingRate", UGITUtility.StringToDouble(userRole.BillingRate));
                                arrParams.Add("@Deleted", false);
                                arrParams.Add("@Id", Int64.MaxValue);
                                DataSet dtResultBillings = DAL.uGITDAL.ExecuteDataSet_WithParameters("usp_GetRole", arrParams);

                                int id = 0;
                                if (dtResultBillings != null && dtResultBillings.Tables[0].Rows.Count > 0)
                                {
                                    id = Convert.ToInt32(dtResultBillings.Tables[0].Rows[0]["ID"]);
                                }

                                arrParams.Clear();

                                arrParams.Add("@id", id);
                                arrParams.Add("@BillingRate", UGITUtility.StringToDouble(userRole.BillingRate));
                                arrParams.Add("@RoleId", roleID);
                                arrParams.Add("@DepartmentId", deptID);
                                arrParams.Add("@Deleted", false);
                                arrParams.Add("@TenantID", context.TenantID);
                                if (DAL.uGITDAL.ExecuteNonQueryWithParameters("usp_insertDepartmentRoleMapping", arrParams) > 0)
                                {
                                    recordsMapped += 1;
                                }
                            }
                            else
                            {
                                recordsNotMapped += 1;
                                AddToDepartmentsNotMapped(departmentsNotMapped, userRole);

                            }
                        }
                        else
                        {
                            recordsNotMapped += 1;
                            AddToDepartmentsNotMapped(departmentsNotMapped, userRole);

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
            stringBuilder.Append($"<b>User Roles Created:</b> {recordsInserted}\n");
            stringBuilder.Append($"<b>User Roles Updated:</b> {recordsUpdated}\n");
            stringBuilder.Append($"<b>User Roles Mapped with Department:</b> {recordsMapped}\n");
            stringBuilder.Append($"<b>User Roles not mapped with Department (not found):</b> {recordsNotMapped}\n");
            if (departmentsNotMapped.Count > 0)
            {
                stringBuilder.Append($"<b>Department (not mapped):</b>\n");
            }
            foreach (var item in departmentsNotMapped)
            {
                stringBuilder.Append($"\t{item.Item1} - {item.Item2}\n");
            }
            stringBuilder.Append($"<b>User Roles not processed (error occurred):</b> {recordsNotProcessed}\n");

            summary = stringBuilder.ToString();
        }

        private static void AddToDepartmentsNotMapped(List<(string, string)> departmentsNotMapped, ImportUserRole userRole)
        {
            if (!departmentsNotMapped.Any(d => d.Item1 == userRole.Division && d.Item2 == userRole.Department))
                departmentsNotMapped.Add((userRole.Division, userRole.Department));
        }
    }

    public class ImportUserRole
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string BillingRate { get; set; }
    }
}
