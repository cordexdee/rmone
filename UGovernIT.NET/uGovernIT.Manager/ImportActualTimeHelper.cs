using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ImportActualTimeHelper
    {
        public static void ImportActualTime(ApplicationContext context, string listName, string filePath, bool deleteExistingRecords, ref string errorMessage, ref string summary)
        {
            int totalRecords = 0, recordsImported = 0, missingUsersCount = 0;
            string missingUsers = string.Empty;
            StringBuilder sbMissingItems = new StringBuilder();
            StringBuilder sbInvalidDateItems = new StringBuilder();
            DateTime dt;

            List<string> lstMissingUsers = new List<string>();
            List<string> lstInvalidProjects = new List<string>();

            ResourceWorkItemsManager objResourceAvailbility = new ResourceWorkItemsManager(context);
            UserProfileManager profileManager = new UserProfileManager(context);
            UserProfile user = null;

            List<ActualSummary> actualSummaries = new List<ActualSummary>();
            List<RMMActualSummary> rmmActualSummaries = new List<RMMActualSummary>();

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

                //List<UserProfile> userProfiles = profileManager.GetUsersProfile().ToList();

                List<UserProfile> userProfiles = profileManager.LoadWithoutGroup().ToList();

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
                List<ActualTime> actualTimes = new List<ActualTime>();
                RowCollection rows = worksheet.Rows;
                for (int i = dataRange.TopRowIndex + 1; i <= dataRange.BottomRowIndex; i++)
                {
                    Row rowData = rows[i];

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("ProjectId")).FirstOrDefault();
                    string ProjectId = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (string.IsNullOrEmpty(ProjectId))
                        continue;

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("ERPJobId")).FirstOrDefault();
                    string ERPJobId = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (string.IsNullOrEmpty(ERPJobId))
                        continue;

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("ERPJobIdNC")).FirstOrDefault();
                    string ERPJobIdNC = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (string.IsNullOrEmpty(ERPJobIdNC))
                        continue;

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Dept").FirstOrDefault();
                    string Dept = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (string.IsNullOrEmpty(Dept) || (!Dept.Contains("JOB") && !Dept.Contains("NCO")))
                        continue;

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ResourceUser").FirstOrDefault();
                    string ResourceUser = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "EmployeeId").FirstOrDefault();
                    string EmployeeId = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Date").FirstOrDefault();
                    string Date = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (DateTime.TryParse(Date, out dt) == false)
                    {
                        sbInvalidDateItems.AppendLine($"{ProjectId}");
                        continue;
                    }


                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "JobCompany").FirstOrDefault();
                    string JobCompany = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Title").FirstOrDefault();
                    string Title = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "NWHR").FirstOrDefault();
                    //int NWHR = UGITUtility.StringToInt(uHelper.GetValueByColumn(rowData, facItem, lstColumn));
                    float NWHR = UGITUtility.StringToFloat(uHelper.GetValueByColumn(rowData, facItem, lstColumn));

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "OT").FirstOrDefault();
                    //int OT = UGITUtility.StringToInt(uHelper.GetValueByColumn(rowData, facItem, lstColumn));
                    float OT = UGITUtility.StringToFloat(uHelper.GetValueByColumn(rowData, facItem, lstColumn));

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "DOT").FirstOrDefault();
                    //int DOT = UGITUtility.StringToInt(uHelper.GetValueByColumn(rowData, facItem, lstColumn));
                    float DOT = UGITUtility.StringToFloat(uHelper.GetValueByColumn(rowData, facItem, lstColumn));

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "OTHR").FirstOrDefault();
                    //int OTHR = UGITUtility.StringToInt(uHelper.GetValueByColumn(rowData, facItem, lstColumn));
                    float OTHR = UGITUtility.StringToFloat(uHelper.GetValueByColumn(rowData, facItem, lstColumn));

                    //int Hours = NWHR + OT + DOT + OTHR;
                    float Hours = NWHR + OT + DOT + OTHR;
                    actualTimes.Add(new ActualTime { ResourceUser = ResourceUser, ProjectId = ProjectId, ERPJobId = ERPJobId, ERPJobIdNC = ERPJobIdNC, Title = Title, Date = Date, Dept = Dept, Hours = Hours, EmployeeId = EmployeeId });
                }

                totalRecords = actualTimes.Count;

                Dictionary<string, object> arrParams = new Dictionary<string, object>();

                //if (userProfiles != null)
                //{
                //    foreach (var item in userProfiles)
                //    {
                //        string userCache = "Name:- " + item.Name + " \n " + "EmployeeId:- " + item.EmployeeId + " \n " + "Email:- " + item.Email + " \n " + "Enabled:- " + item.Enabled;
                //        ULog.WriteLog(userCache);
                //        ULog.WriteException(userCache);
                //    }
                //}

                foreach (ActualTime item in actualTimes)
                {
                    user = null;

                    if (!string.IsNullOrEmpty(item.EmployeeId))
                        user = userProfiles.FirstOrDefault(x => x.EmployeeId.EqualsIgnoreCase(item.EmployeeId));

                    if (user == null)
                    {
                        //string missing = ":::MISSING USERS::: Name:- " + item.ResourceUser + " \n " + "ProjectId:- " + item.ProjectId + " \n " + "ErPJobId:- " + item.ERPJobId + " \n " + "ERPJobIdNC:- " + item.ERPJobIdNC + " \n " + "EmployeeId:- " + item.EmployeeId;
                        //ULog.WriteLog(missing);
                        //ULog.WriteException(missing);
                        lstMissingUsers.Add(item.ResourceUser);
                        continue;
                    }

                    arrParams.Clear();
                    arrParams.Add("@ResourceUser", user.Id);
                    arrParams.Add("@ProjectId", item.ProjectId);
                    arrParams.Add("@ERPJobId", item.ERPJobId);
                    arrParams.Add("@ERPJobIdNC", item.ERPJobIdNC);
                    arrParams.Add("@Dept", item.Dept);
                    arrParams.Add("@Date", item.Date);
                    arrParams.Add("@Hours", item.Hours);
                    arrParams.Add("@TenantID", context.TenantID);
                    arrParams.Add("@CreatedBy", context.CurrentUser.Id);
                    DataTable val = DAL.uGITDAL.ExecuteDataSetWithParameters("usp_insertActualHours", arrParams);
                    if (!UGITUtility.IfColumnExists("Result", val))
                    {
                        if (!lstInvalidProjects.Contains(item.ProjectId))
                            lstInvalidProjects.Add(item.ProjectId);

                        continue;
                    }

                    if (Convert.ToString(val.Rows[0]["Result"]) == "1" || Convert.ToString(val.Rows[0]["Result"]) == "2" || Convert.ToString(val.Rows[0]["Result"]) == "3")
                    {
                        if (Convert.ToString(val.Rows[0]["Result"]) != "")
                            recordsImported++;

                        actualSummaries.Add(new ActualSummary { Resource = Convert.ToString(val.Rows[0]["Resource"]), WeekStartDate = Convert.ToString(val.Rows[0]["WeekStartDate"]), WorkItemId = Convert.ToInt64(val.Rows[0]["WorkItemId"]) });

                        if (rmmActualSummaries.Where(x => x.Resource == Convert.ToString(val.Rows[0]["Resource"]) && x.WeekStartDate == Convert.ToString(val.Rows[0]["WeekStartDate"])).Count() == 0)
                        {
                            rmmActualSummaries.Add(new RMMActualSummary { Resource = Convert.ToString(val.Rows[0]["Resource"]), WeekStartDate = Convert.ToString(val.Rows[0]["WeekStartDate"]) });
                        }

                    }
                    else if (Convert.ToString(val.Rows[0]["Result"]) == "0" || Convert.ToString(val.Rows[0]["Result"]) == "-1")
                    {
                        if (!lstInvalidProjects.Contains(item.ProjectId))
                            lstInvalidProjects.Add(item.ProjectId);
                    }
                }

                if (lstMissingUsers.Count > 0)
                {
                    missingUsers = string.Join(",\n", lstMissingUsers.Distinct());
                    missingUsersCount = lstMissingUsers.Distinct().Count();
                }
            }
            catch (Exception ex)
            {
                errorMessage = System.IO.Path.GetFileName(filePath) + ": " + ex.Message;
                ULog.WriteLog(errorMessage);
                ULog.WriteException(ex);
            }
            finally
            {
                objResourceAvailbility.UpdateIntoCache(null, true, true);
                List<long> lstWorkItems = new List<long>();
                foreach (var item in rmmActualSummaries)
                {
                    lstWorkItems.Clear();
                    lstWorkItems = actualSummaries.Where(x => x.Resource == item.Resource && x.WeekStartDate == item.WeekStartDate).Select(x => x.WorkItemId).Distinct().ToList();
                    //item.WorkItemId = lstWorkItems.Distinct();

                    RMMSummaryHelper.UpdateActualInRMMSummary(context, lstWorkItems, item.Resource, Convert.ToDateTime(item.WeekStartDate), true);
                }
            }

            summary = $"<b>File Name:</b> {System.IO.Path.GetFileName(filePath)}\n<b>Total Records from Excel:</b> {totalRecords}\n<b>Records Processed:</b> {recordsImported}\n<b>Missing Users count: </b>{missingUsersCount}\n<b>Missing Users: </b>\n{missingUsers}" +
            $"\n<b>Invalid Date Items:</b>\n{Convert.ToString(sbInvalidDateItems)}\n<b>Invalid Projects:</b>\n{string.Join(",\n", lstInvalidProjects)}\n";
        }

    }

    public class ActualTime
    {
        public string ProjectId { get; set; }
        public string ERPJobId { get; set; }
        public string ERPJobIdNC { get; set; }
        public string Title { get; set; }
        public string JobCompany { get; set; }
        public string Date { get; set; }
        public string EmployeeId { get; set; }
        public string ResourceUser { get; set; }
        public string Dept { get; set; }
        public string AcctCode { get; set; }
        public float NWHR { get; set; }
        public float OT { get; set; }
        public float DOT { get; set; }
        public float OTHR { get; set; }
        public float Hours { get; set; }
    }

    public class ActualSummary
    {
        public string Resource { get; set; }
        public string WeekStartDate { get; set; }
        public long WorkItemId { get; set; }
    }

    public class RMMActualSummary
    {
        public string Resource { get; set; }
        public string WeekStartDate { get; set; }
        //public List<long> WorkItemId { get; set; }
    }
}
