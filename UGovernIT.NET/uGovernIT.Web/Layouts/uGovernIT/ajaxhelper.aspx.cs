using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class ajaxhelper : UPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetDurationInHours(string startDate, string endDate)
        {
            try
            {
                DateTime sDate = UGITUtility.StringToDateTime(startDate);
                DateTime eDate = UGITUtility.StringToDateTime(endDate);
                if (eDate.TimeOfDay.Hours == 0)
                    eDate = eDate.AddMinutes(1439);

                double duration = Math.Round(uHelper.GetWorkingMinutesBetweenDates(HttpContext.Current.GetManagerContext(), sDate, eDate) / 60, 0);
                return string.Format("{1}\"messagecode\":2,\"message\":\"success\",\"duration\":{0:0.##}{2}", duration, "{", "}");
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetProjectComplexity(String ContractValue)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            double appxContractValue = 0;
            string projectComplexity = string.Empty;

            try
            {
                double.TryParse(ContractValue, out appxContractValue);

                projectComplexity = uHelper.GetProjectComplexity(context, appxContractValue);

                if (!string.IsNullOrEmpty(projectComplexity))
                {
                    return string.Format("{1}\"messagecode\":\"2\",\"message\":\"success\",\"projectComplexity\":\"{0}\"{2}", projectComplexity, "{", "}");
                }
                else
                {
                    return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                }
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetEndDateByWorkingDays(DateTime startDate, int noOfWorkingDays)
        {
            try
            {
                DateTime[] calculatedDates = uHelper.GetEndDateByWorkingDays(HttpContext.Current.GetManagerContext(), startDate, noOfWorkingDays);
                if (calculatedDates != null && calculatedDates.Length > 1)
                {
                    string duration = CalculateDuration(calculatedDates[0], calculatedDates[1]);
                    return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("MM/dd/yyyy").Replace("-", "/"), calculatedDates[1].ToString("MM/dd/yyyy").Replace("-", "/"), "{", "}", duration);
                }
                else
                {
                    return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                }
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string CalculateDuration(DateTime startDate, DateTime endDate)
        {
            string duration = string.Empty;
            try
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startDate, endDate);
                int week = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                if (week > 0)
                {
                    duration = string.Format("{0} week(s)", week);

                }
                else
                {
                    duration = string.Format("{0} days(s)", noOfWorkingDays);
                }
            }
            catch
            {

            }
            return duration;
        }

        [WebMethod]
        public static string GetTitleRelatedTicketCount(string tableName, string titleName, string requestType)
        {
            var ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            var countForRelatedLink = 0;
            //private string TenantID = TenantHelper.GetTanantID();
            try
            {
                var dataExist = GetTableDataManager.IsLookaheadTicketExists(tableName, TenantHelper.GetTanantID(), titleName, requestType);
                if (dataExist)
                {
                    countForRelatedLink = ticketManager.GetTitleRelatedTicketCount(tableName, titleName, requestType);
                    // return " { 2}\"messagecode\":\"2\",\"message\":\"success\"}; convert into json
                    return Convert.ToString(countForRelatedLink);
                }
                else
                    return Convert.ToString(countForRelatedLink);
            }
            catch (Exception ex)
            {
                // return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                return ex.Message;
            }
        }


        [WebMethod]
        public static string ResolveUsers(string assignedTo)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string resolvedUserName = string.Empty;
            string error = string.Empty;
            List<string> lstUser = assignedTo.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string user in lstUser)
            {
                string userName = user.Trim();
                string assignedPct = string.Empty;
                if (!string.IsNullOrEmpty(userName) && userName.Contains("["))
                {
                    string[] userComponents = UGITUtility.SplitString(userName, "[", StringSplitOptions.RemoveEmptyEntries);
                    userName = userComponents[0].Trim();
                    double s = 0;
                    if (userComponents.Length > 1)
                    {
                        assignedPct = "[" + userComponents[1];
                        s = UGITUtility.StringToDouble(userComponents[1].Split('%').FirstOrDefault());
                    }
                    if (s == 0)
                    {
                        if (!string.IsNullOrEmpty(error))
                            error += "; Allocation Percentage not in correct format";
                        else
                            error += "Allocation Percentage not in correct format";
                    }
                    else if (s > 100)
                    {
                        assignedPct = "[100%]";
                    }

                }
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    UserProfile spUser = null;
                    UserProfileManager userProfile = new UserProfileManager(context);
                    if (context != null)
                    {
                        // List<UserProfile> profiles = uGITCache.UserProfileCache.GetEnabledUsers(context);
                        List<UserProfile> profiles = userProfile.GetUsersProfile();
                        if (profiles != null)
                            spUser = profiles.FirstOrDefault(x => (x.Email.ToLower() == userName.ToLower() || x.Name.ToLower() == userName.ToLower() || x.UserName.ToLower() == userName.ToLower()));
                        //spUser = profiles.FirstOrDefault(x => (x.LoginName.ToLower().EndsWith("\\" + userName.ToLower())) || (x.LoginName.ToLower().EndsWith("|" + userName.ToLower())) || x.Email.ToLower() == userName.ToLower() || x.Name.ToLower() == userName.ToLower());
                    }
                    if (resolvedUserName != string.Empty)
                        resolvedUserName += " ";
                    if (spUser != null)
                        resolvedUserName += spUser.Name + assignedPct + ";";
                    else
                    {
                        error += "User " + "'" + userName + "' cannot be resolved";
                        resolvedUserName += userName + assignedPct + ";";
                    }
                }
            }

            StringBuilder data = new StringBuilder();
            string status = error == string.Empty ? "resolvedwithouterrors" : "resolvedwitherrors";
            data.AppendFormat("{0}\"resolvedUsers\":\"{2}\",\"errors\":\"{3}\",\"status\":\"{4}\"{1}", "{", "}", resolvedUserName, error, status);
            return data.ToString();
        }


        [WebMethod]
        public static string GetEndDateByWeeks(String startDate, int noOfWeeks)
        {
            try
            {

                DateTime startDateNew = DateTime.ParseExact(startDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                int noOfWorkingDays = uHelper.GetWorkingDaysInWeeks(HttpContext.Current.GetManagerContext(), noOfWeeks);
                DateTime[] calculatedDates = uHelper.GetEndDateByWorkingDays(HttpContext.Current.GetManagerContext(), startDateNew, noOfWorkingDays);
                if (calculatedDates != null && calculatedDates.Length > 1)
                {
                    string duration = CalculateDuration(calculatedDates[0], calculatedDates[1]);

                    return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("MMM/d/yyyy").Replace("-", "/"), calculatedDates[1].ToString("MMM/d/yyyy").Replace("-", "/"), "{", "}", duration);
                    //return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("yyyy-MM-dd"), calculatedDates[1].ToString("yyyy-MM-dd"), "{", "}", duration);
                }
                else
                {
                    return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                }
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetEndDateByWeeksProject(String startDate, int noOfWeeks)
        {
            try
            {

                DateTime startDateNew = DateTime.ParseExact(startDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                int noOfWorkingDays = uHelper.GetWorkingDaysInWeeks(HttpContext.Current.GetManagerContext(), noOfWeeks);
                DateTime[] calculatedDates = uHelper.GetEndDateByWorkingDays(HttpContext.Current.GetManagerContext(), startDateNew, noOfWorkingDays);
                if (calculatedDates != null && calculatedDates.Length > 1)
                {
                    string duration = CalculateDuration(calculatedDates[0], calculatedDates[1]);

                    return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("MM/dd/yyyy").Replace("-", "/"), calculatedDates[1].ToString("MM/dd/yyyy").Replace("-", "/"), "{", "}", duration);
                    //return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("yyyy-MM-dd"), calculatedDates[1].ToString("yyyy-MM-dd"), "{", "}", duration);
                }
                else
                {
                    return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                }
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }



        [WebMethod]
        public static string GetDurationInWeeks(DateTime startDate, DateTime endDate)
        {
            try
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startDate, endDate);
                int noOfWeeks = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                return string.Format("{1}\"messagecode\":\"2\",\"message\":\"success\",\"duration\":\"{0}\"{2}", noOfWeeks, "{", "}");
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetTotalWorkingDaysBetween(DateTime startDate, DateTime endDate)
        {
            try
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startDate, endDate);
                return string.Format("{1}\"messagecode\":\"2\",\"message\":\"success\",\"noOfWorkingDays\":\"{0}\"{2}", noOfWorkingDays, "{", "}");
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetNextWorkingDateForTasks(string startDateRaw, string endDateRaw, string newStartDateRaw, string addOneMoreDay, int taskid, string ticketId, string moduleName)
        {
            try
            {
                DateTime startDate = UGITUtility.StringToDateTime(startDateRaw);
                DateTime endDate = UGITUtility.StringToDateTime(endDateRaw);
                DateTime newStartDate = UGITUtility.StringToDateTime(newStartDateRaw);

                //If start and due date are empty the assign newstartdate + 1 day to start and due date 
                if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    if (newStartDate != DateTime.MinValue)
                    {
                        startDate = newStartDate.AddDays(1);
                        endDate = startDate;
                    }
                    else
                        startDate = DateTime.Now;
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                    endDate = startDate;
                else if (endDate != DateTime.MinValue && startDate == DateTime.MinValue)
                    startDate = endDate;

                bool addOneDayInNewSDate = UGITUtility.StringToBoolean(addOneMoreDay);

                ApplicationContext context = HttpContext.Current.GetManagerContext();

                if (taskid > 0)
                {
                    UGITTaskManager TaskManager = new UGITTaskManager(context);
                    List<UGITTask> ptasks = TaskManager.LoadByProjectID(moduleName, ticketId);
                    ptasks = UGITTaskManager.MapRelationalObjects(ptasks);

                    var task = ptasks.FirstOrDefault(x => x.ID == taskid);
                    if (task.ParentTaskID > 0)
                    {
                        if (task.PredecessorTasks != null && !task.PredecessorTasks.Exists(x => x.ParentTaskID == task.ParentTaskID))
                        {
                            var ptask = task.ParentTask;
                            if (ptask.StartDate > newStartDate &&
                                ptask.PredecessorTasks != null && ptask.PredecessorTasks.Count > 0)
                            {
                                newStartDate = ptask.StartDate;
                                addOneDayInNewSDate = false;
                            }
                        }
                    }
                }

                DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(context, startDate, endDate, newStartDate, addOneDayInNewSDate);
                int workingDays = uHelper.GetTotalWorkingDaysBetween(context, dates[0], dates[1]);
                int workingHoursInADay = uHelper.GetWorkingHoursInADay(context);
                int totalHours = workingDays * workingHoursInADay;
                return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"workinghours\":\"{4}\"{3}", dates[0].ToString("MM/dd/yyyy").Replace("-", "/"), dates[1].ToString("MM/dd/yyyy").Replace("-", "/"), "{", "}", totalHours);
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetNextWorkingDate(string startDateRaw, string endDateRaw, string newStartDateRaw, string addOneMoreDay)
        {
            try
            {
                DateTime startDate = UGITUtility.StringToDateTime(startDateRaw);
                DateTime endDate = UGITUtility.StringToDateTime(endDateRaw);
                DateTime newStartDate = UGITUtility.StringToDateTime(newStartDateRaw);

                //If start and due date are empty the assign newstartdate + 1 day to start and due date 
                if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    if (newStartDate != DateTime.MinValue)
                    {
                        startDate = newStartDate.AddDays(1);
                        endDate = startDate;
                    }
                    else
                        startDate = DateTime.Now;
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                    endDate = startDate;
                else if (endDate != DateTime.MinValue && startDate == DateTime.MinValue)
                    startDate = endDate;

                bool addOneDayInNewSDate = UGITUtility.StringToBoolean(addOneMoreDay);
                ApplicationContext context = HttpContext.Current.GetManagerContext();

                DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(context, startDate, endDate, newStartDate, addOneDayInNewSDate);
                int workingDays = uHelper.GetTotalWorkingDaysBetween(context, dates[0], dates[1]);
                int workingHoursInADay = uHelper.GetWorkingHoursInADay(context);
                int totalHours = workingDays * workingHoursInADay;
                return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"workinghours\":\"{4}\"{3}", dates[0].ToString("MM/dd/yyyy").Replace("-", "/"), dates[1].ToString("MM/dd/yyyy").Replace("-", "/"), "{", "}", totalHours);
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        [WebMethod]
        public static string GetToolTip(string cAssetId, string listName)
        {
            List<string> tooltipCols = null;
            if (listName.Equals(DatabaseObjects.Tables.Assets))
            {

                tooltipCols = new List<string>() {DatabaseObjects.Columns.Id,DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.AssetTagNum, DatabaseObjects.Columns.AssetName,
                                                  DatabaseObjects.Columns.Owner,DatabaseObjects.Columns.TicketRequestTypeLookup,
                                                  string.Format("{0};~{1}",DatabaseObjects.Columns.VendorLookup,DatabaseObjects.Columns.AssetModelLookup),
                                                  string.Format("{0};~{1}",DatabaseObjects.Columns.TicketStatus,DatabaseObjects.Columns.AssetDispositionChoice),
                                                  DatabaseObjects.Columns.HostName,DatabaseObjects.Columns.IPAddress,
                                                  DatabaseObjects.Columns.DepartmentLookup,DatabaseObjects.Columns.LocationLookup};
            }
            else if (listName.Equals(DatabaseObjects.Tables.AssetVendors))
            {
                tooltipCols = new List<string>() {DatabaseObjects.Columns.Id,DatabaseObjects.Columns.VendorName, DatabaseObjects.Columns.ContactName, DatabaseObjects.Columns.Location,
                                                  DatabaseObjects.Columns.VendorPhone,DatabaseObjects.Columns.VendorEmail, DatabaseObjects.Columns.HostName,DatabaseObjects.Columns.VendorAddress};
            }
            else if (listName.Equals(DatabaseObjects.Tables.AssetModels))
            {
                tooltipCols = new List<string>() {DatabaseObjects.Columns.Id,DatabaseObjects.Columns.VendorLookup, DatabaseObjects.Columns.ModelName,
                                                    //DatabaseObjects.Columns.ExternalType,
                                                    DatabaseObjects.Columns.ModelDescription,DatabaseObjects.Columns.BudgetCategoryLookup};
            }

            if (tooltipCols == null)
                return string.Empty;

            string viewFieldString = string.Empty;
            List<string> lstViewField = new List<string>();
            tooltipCols.ForEach(x =>
            {
                string[] colArray = x.Split(new string[] { ";~" }, StringSplitOptions.RemoveEmptyEntries);
                if (colArray.Length > 1)
                {
                    colArray.ToList().ForEach(y =>
                    {
                        lstViewField.Add(y);
                    });
                }
                else
                    lstViewField.Add(x);
            });

            viewFieldString = string.Join(uGovernIT.Utility.Constants.Separator6, lstViewField);

            DataRow asset = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.ID} = {cAssetId}", viewFieldString, null).Select()[0];
            if (asset == null && asset.ItemArray.Length == 0)
                return string.Empty;

            FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            FieldConfiguration field = null;
            StringBuilder jsonStr = new StringBuilder();
            string colStr = string.Empty;
            foreach (string colInternalName in tooltipCols)
            {
                string value = string.Empty;
                object output = string.Empty;
                string[] combineColStr = colInternalName.Split(new string[] { ";~" }, StringSplitOptions.RemoveEmptyEntries);
                if (combineColStr.Length > 1)
                {
                    string innerVal = string.Empty;
                    List<string> lstColVal = new List<string>();
                    foreach (string currentCol in combineColStr)
                    {
                        output = asset[currentCol];
                        if (string.IsNullOrEmpty(Convert.ToString(output)) || colInternalName == DatabaseObjects.Columns.Id)
                            continue;

                        field = fmanger.GetFieldByFieldName(currentCol);
                        if (field != null)
                        {
                            innerVal = fmanger.GetFieldConfigurationData(field, Convert.ToString(output));
                        }
                        else
                        {
                            innerVal = Convert.ToString(output);
                        }

                        if (!string.IsNullOrEmpty(innerVal))
                            lstColVal.Add(innerVal);
                    }
                    value = string.Join(" > ", lstColVal);
                }
                else
                {
                    output = asset[colInternalName];
                    if (string.IsNullOrEmpty(Convert.ToString(output)) || colInternalName == DatabaseObjects.Columns.Id)
                        continue;

                    field = fmanger.GetFieldByFieldName(colInternalName);
                    if (field != null)
                    {
                        value = fmanger.GetFieldConfigurationData(field, Convert.ToString(output));
                    }
                    else
                    {
                        value = Convert.ToString(output);
                    }
                }

                if (string.IsNullOrEmpty(value))
                    continue;

                value = jsonEncode(value);

                if (jsonStr.Length == 0)
                    jsonStr.Append(string.Format("{{\"{0}\":\"{1}\"", colInternalName, value));
                else
                    jsonStr.Append(string.Format(",\"{0}\":\"{1}\"", colInternalName, value));
            }

            if (jsonStr.Length > 0)
            {
                jsonStr.AppendFormat("}}");
                return jsonStr.ToString();
            }
            else
                return string.Empty;
        }

        [WebMethod]
        public static string GetOpenTasksCount(string ticketID)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                DataTable dt = new DataTable();
                int Count = 0;
                dt = GetTableDataManager.ExecuteQuery($"select count(*) from {DatabaseObjects.Tables.ModuleStageConstraints} where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and ({DatabaseObjects.Columns.TaskStatus} != 'Completed' or {DatabaseObjects.Columns.TaskStatus} is null) and {DatabaseObjects.Columns.Deleted} = '0'");
                if (dt != null && dt.Rows.Count > 0)
                    Count += Convert.ToInt32(dt.Rows[0][0]);

                dt = GetTableDataManager.ExecuteQuery($"select count(*) from {DatabaseObjects.Tables.ModuleTasks} where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and ({DatabaseObjects.Columns.Status} != 'Completed' or {DatabaseObjects.Columns.Status} is null)");
                if (dt != null && dt.Rows.Count > 0)
                    Count += Convert.ToInt32(dt.Rows[0][0]);

                return Convert.ToString(Count);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public static string GetOpenTasksCountById(string ModuleNameLookUp, string ID)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                string ticketID = string.Empty;
                if (ModuleNameLookUp == ModuleNames.LEM)
                    ticketID = Convert.ToString(GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.Lead, $"{DatabaseObjects.Columns.TicketId}", ID, context.TenantID));
                else if (ModuleNameLookUp == ModuleNames.OPM)
                    ticketID = Convert.ToString(GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.Opportunity, $"{DatabaseObjects.Columns.TicketId}", ID, context.TenantID));

                DataTable dt = new DataTable();
                int Count = 0;
                dt = GetTableDataManager.ExecuteQuery($"select count(*) from {DatabaseObjects.Tables.ModuleStageConstraints} where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and ({DatabaseObjects.Columns.TaskStatus} != 'Completed' or {DatabaseObjects.Columns.TaskStatus} is null)");
                if (dt != null && dt.Rows.Count > 0)
                    Count += Convert.ToInt32(dt.Rows[0][0]);

                dt = GetTableDataManager.ExecuteQuery($"select count(*) from {DatabaseObjects.Tables.ModuleTasks} where {DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and ({DatabaseObjects.Columns.Status} != 'Completed' or {DatabaseObjects.Columns.Status} is null)");
                if (dt != null && dt.Rows.Count > 0)
                    Count += Convert.ToInt32(dt.Rows[0][0]);

                return Convert.ToString(Count);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string jsonEncode(string val)
        {
            return val.Replace("\r\n", " ")
                      .Replace("\r", " ")
                      .Replace("\n", " ")
                      .Replace("\\", "\\\\")
                      .Replace("\"", "\\\"");
        }

        [WebMethod]
        public static string GetPmmActualsData(string startDate, string itemId, string endDate, string pmmTicketID)
        {
            int messageCode = 0;
            string message = string.Empty;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            BudgetActualsManager actualManager = new BudgetActualsManager(context);

            DateTime StartDate = Convert.ToDateTime(startDate);
            DateTime EndDate = Convert.ToDateTime(endDate);

            DataTable projectBudgetTable = new DataTable();

            projectBudgetTable.Columns.Add("BudgetItem", typeof(string));
            projectBudgetTable.Columns["BudgetItem"].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.AllocationStartDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.AllocationEndDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add("Actual", typeof(string));
            projectBudgetTable.Columns["Actual"].DefaultValue = string.Format("{0:C}", 0.0);

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetDescription].DefaultValue = string.Empty;

            //string startDateQuery = string.Format("<And><Geq><FieldRef Name='{0}' /><Value  IncludeTimeValue='FALSE' Type='DateTime'>{1}</Value></Geq><Leq><FieldRef Name='{0}' /><Value  IncludeTimeValue='FALSE' Type='DateTime'>{2}</Value></Leq></And>", DatabaseObjects.Columns.AllocationStartDate, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
            //string endDateQuery = string.Format("<And><Geq><FieldRef Name='{0}' /><Value  IncludeTimeValue='FALSE' Type='DateTime'>{1}</Value></Geq><Leq><FieldRef Name='{0}' /><Value  IncludeTimeValue='FALSE' Type='DateTime'>{2}</Value></Leq></And>", DatabaseObjects.Columns.AllocationEndDate, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
            //string dateQuery = string.Format("<Or>{0}{1}</Or>", startDateQuery, endDateQuery);

            //SPQuery childQuery = new SPQuery();
            //childQuery.Query = string.Format("<Where><And><And><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True' /><Value Type='Lookup'>{3}</Value></Eq></And>{4}</And></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, pmmTicketID, DatabaseObjects.Columns.PMMBudgetLookup, itemId, dateQuery);

            List<BudgetActual> actuals = actualManager.Load(x => x.TicketId == pmmTicketID);

            if (actuals.Count > 0)
            {
                // New Code Only Actuals of per budget item is needed.
                double itemTotal = 0.0;

                foreach (BudgetActual actualRow in actuals)
                {
                    DataRow row = projectBudgetTable.NewRow();
                    row[DatabaseObjects.Columns.BudgetItem] = Convert.ToString(actualRow.ModuleBudgetLookup);
                    row[DatabaseObjects.Columns.AllocationStartDate] = uHelper.GetDateStringInFormat(context, Convert.ToDateTime(actualRow.AllocationStartDate), false);
                    row[DatabaseObjects.Columns.AllocationEndDate] = uHelper.GetDateStringInFormat(context, Convert.ToDateTime(actualRow.AllocationEndDate), false);
                    row["Actual"] = Convert.ToDouble(actualRow.BudgetAmount);
                    row[DatabaseObjects.Columns.BudgetDescription] = actualRow.BudgetDescription;
                    itemTotal += Convert.ToDouble(actualRow.BudgetAmount);
                    projectBudgetTable.Rows.Add(row);
                }
                // To store the item in the first row only so that it looks like a group at the time of display.
                //DataRow groupRow = projectBudgetTable.NewRow();
                //groupRow[DatabaseObjects.Columns.BudgetItem] = PMMActualBudgetTable.Rows[0]["PMMBudgetLookup"];
                //groupRow["Actual"] = itemTotal;
                //projectBudgetTable.Rows.InsertAt(groupRow, 0);
                projectBudgetTable.AcceptChanges();

            }

            if (projectBudgetTable.Rows.Count > 0)
            {
                message = SerializationExtensions.GetJSONString(projectBudgetTable);
                return message;
            }

            return "{\"messagecode\":" + messageCode + ", \"message\":\"" + message + "\"}";
        }

        [WebMethod]
        public static string GetItgActualsData(string startDate, string itemId, string endDate)
        {
            int messageCode = 0;
            string message = string.Empty;
            DateTime StartDate = Convert.ToDateTime(startDate);
            DateTime EndDate = Convert.ToDateTime(endDate);

            DataTable projectBudgetTable = new DataTable();

            projectBudgetTable.Columns.Add("BudgetItem", typeof(string));
            projectBudgetTable.Columns["BudgetItem"].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetStartDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetStartDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetEndDate, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetEndDate].DefaultValue = string.Empty;

            projectBudgetTable.Columns.Add("Actual", typeof(string));
            projectBudgetTable.Columns["Actual"].DefaultValue = string.Format("{0:C}", 0.0);

            projectBudgetTable.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            projectBudgetTable.Columns[DatabaseObjects.Columns.BudgetDescription].DefaultValue = string.Empty;
            
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@itemid", itemId);
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);

            projectBudgetTable =GetTableDataManager.GetData("RPT_ModuleBudget", values);
            if (projectBudgetTable.Rows.Count > 0)
            {
                message = SerializationExtensions.GetJSONString(projectBudgetTable);
                return message;
            }

            return "{\"messagecode\":" + messageCode + ", \"message\":\"" + message + "\"}";
        }
    }
}