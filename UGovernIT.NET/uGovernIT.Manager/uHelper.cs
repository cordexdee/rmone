using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using uGovernIT.Utility;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml.Serialization;
using System.Xml;
using DevExpress.XtraScheduler;
using System.Globalization;
using uGovernIT.Utility.Entities;
using System.Data.SqlClient;
using System.Drawing;
using DevExpress.Web;
using DevExpress.Spreadsheet;
using uGovernIT.Util.Log;
using uGovernIT.Manager.Managers;
using System.Configuration;
using uGovernIT.Manager.Core;
using DevExpress.Spreadsheet.Export;
using System.Runtime.Remoting.Contexts;
using System.Runtime.InteropServices.WindowsRuntime;
using DevExpress.Xpo.Helpers;
using System.Threading;
using DevExpress.XtraRichEdit.Import.Html;
using uGovernIT.DAL;
using DevExpress.XtraRichEdit.Model;
using System.Runtime.InteropServices.ComTypes;
using uGovernIT.Manager.RMM;
using System.Reflection;

namespace uGovernIT.Manager
{
    public class uHelper
    {
        public static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public static ColorEditItemCollection CreatePalette()
        {
            ColorEditItemCollection colorEditItemCollection = new ColorEditItemCollection();
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#000000"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#993300"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#333300"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#003300"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#003366"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#000080"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#333399"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#333333"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#800000"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FF6600"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#808000"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#008000"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#008080"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#0000FF"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#666699"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#808080"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FF0000"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#99CC00"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#339966"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#33CCCC"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#3366FF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#800080"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#999999"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FF00FF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFCC00"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFFF00"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FF00FF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFCC00"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFFF00"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#00FF00"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#00FFFF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#00CCFF"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#993366"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#C0C0C0"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FF99CC"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFCC99"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFFF99"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#CCFFCC"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#CCFFFF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#99CCFF"));

            colorEditItemCollection.Add(ColorTranslator.FromHtml("#CC99FF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#FFFFFF"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#4A6EE2"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#E24A7A"));
            colorEditItemCollection.Add(ColorTranslator.FromHtml("#5DE9BF"));
            return colorEditItemCollection;
        }
        public static double GetOverlappingDays(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            DateTime maxStart = firstStart > secondStart ? firstStart : secondStart;
            DateTime minEnd = firstEnd < secondEnd ? firstEnd : secondEnd;
            TimeSpan interval = minEnd - maxStart;
            double returnValue = interval > TimeSpan.FromSeconds(0) ? interval.TotalDays : 0;
            return returnValue;
        }
        public static bool IsProjectDatesOverLappingOrInValid(DataRow ticketRow)
        {
            bool result  = false;
            try
            {
                DateTime preconStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconStartDate]);
                DateTime preconEnd = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconEndDate]);
                DateTime constStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                DateTime constEnd = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                DateTime closeoutStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                DateTime closeoutEnd = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]);

                if (preconStart == DateTime.MinValue || preconEnd == DateTime.MinValue || constStart == DateTime.MinValue ||
                    constEnd == DateTime.MinValue || closeoutStart == DateTime.MinValue || closeoutEnd == DateTime.MinValue)
                    return true;

                double a = GetOverlappingDays(preconStart, preconEnd, constStart, constEnd);
                double b = GetOverlappingDays(constStart, constEnd, closeoutStart, closeoutEnd);
                if (a + b > 0)
                    return true;
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
                return true;
            }
            return result;
        }
        public static DataTable GetGeneralGroupsFromConfig(ApplicationContext context)
        {
            string groups = context.ConfigManager.GetValue(ConfigConstants.GeneralGroups);
            List<string> lstgroups = new List<string>();
            if (!string.IsNullOrEmpty(groups))
            {
                lstgroups = UGITUtility.SplitString(groups.ToLower(), Constants.Separator6).Select(x => x.Trim()).ToList();
            }
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("NameRole");
            dtGroups.Columns.Add("Role");
            dtGroups.Columns.Add("Type");

            UserRoleManager roleManager = new UserRoleManager(context);
            List<Role> sRoles = roleManager.GetRoleList();

            foreach (Role oGroup in sRoles)
            {
                if (!string.IsNullOrEmpty(groups) && !lstgroups.Contains(oGroup.Title.ToLower()))
                    continue;
                DataRow dr = dtGroups.NewRow();
                dr["ID"] = oGroup.Id;
                dr["Name"] = oGroup.Title;
                dr["Role"] = oGroup.Name;
                dr["NameRole"] = oGroup.Name;
                dr["Type"] = "Group";
                dtGroups.Rows.Add(dr);

            }

            return dtGroups;
        }

        public static List<GlobalRole> GetGlobalRoles(ApplicationContext context, bool includeDeleted = true)
        {
            GlobalRoleManager globalRoleManager = new GlobalRoleManager(context);
            List<GlobalRole> roles = includeDeleted ? globalRoleManager.Load()
                : globalRoleManager.Load(x => !x.Deleted);
            return roles;
        }

        public static string GetUsersAsString(ApplicationContext context, string actionsUserTypes, DataRow currentTicket)
        {
            string actionUser = "";
            if (currentTicket != null && !string.IsNullOrEmpty(actionsUserTypes))
            {
                List<UserProfile> users = GetActionUsersList(context, actionsUserTypes.Trim(), currentTicket);
                if (users.Count() > 0)
                {
                    actionUser = string.Join(Constants.Separator, users.AsEnumerable().Select(x => x.Id).ToList());
                }
            }
            return actionUser;
        }

        public static List<UserProfile> GetActionUsersList(ApplicationContext context, string actionsUserTypes, DataRow currentTicket)
        {
            List<UserProfile> users = new List<UserProfile>();
            string[] actionUserTypes = UGITUtility.SplitString(actionsUserTypes, Constants.Separator);
            foreach (string fieldName in actionUserTypes)
            {
                if (UGITUtility.IsSPItemExist(currentTicket, fieldName))
                {
                    string[] multiuser = null;
                    if (Convert.ToString(currentTicket[fieldName]).Contains(Constants.Separator5))
                        multiuser = UGITUtility.SplitString(Convert.ToString(currentTicket[fieldName]), Constants.Separator5);
                    else
                        multiuser = UGITUtility.SplitString(Convert.ToString(currentTicket[fieldName]), Constants.Separator);
                    UserProfile user = null;
                    foreach (string userid in multiuser)
                    {
                        user = context.UserManager.GetUserById(userid.Trim());
                        //UserProfile userInfo = UserProfileManager.GetUserInfo(currentTicket, actionUserTypes, true);
                        if (user != null)
                            users.Add(user);
                    }
                }
                else
                {
                    Role role = context.UserManager.GetUserRoleByGroupName(fieldName);
                    if (role != null)
                        users.Add(context.UserManager.GetUserProfileFromRole(role));
                }
            }
            return users;
        }
        public static List<string> GetActionUsersList(ApplicationContext context, DataRow currentTicket)
        {
            List<string> users = new List<string>();
            string actionsUserTypes = string.Empty;
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketStageActionUserTypes, currentTicket.Table))
            {
                actionsUserTypes = UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                string[] actionUserTypes = UGITUtility.SplitString(actionsUserTypes, Constants.Separator);
                foreach (string fieldName in actionUserTypes)
                {
                    if (UGITUtility.IsSPItemExist(currentTicket, fieldName))
                    {
                        string user = UGITUtility.ObjectToString(currentTicket[fieldName]);
                        if (!string.IsNullOrWhiteSpace(user))
                        {
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
        /// <summary>
        /// Get Stage action user by stage name
        /// </summary>
        /// <param name="stageName"></param>
        /// <returns></returns>
        /// 

        public static string GetDepartmentLabelName(DepartmentLevel level)
        {
            string val = string.Empty;
            switch (level)
            {
                case DepartmentLevel.Company:
                    {
                        val = "";//uGITCache.GetConfigVariableValue(ConfigConstants.DepartmentLevel1Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Company";

                    }
                    break;
                case DepartmentLevel.Division:
                    {
                        val = "";// uGITCache.GetConfigVariableValue(ConfigConstants.DepartmentLevel2Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Division";
                    }
                    break;
                case DepartmentLevel.Department:
                    {
                        val = ""; //uGITCache.GetConfigVariableValue(ConfigConstants.DepartmentLevel3Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Department";
                    }
                    break;
            }

            return val;

        }

        //public static DataTable LoadAssetsForParticularUser(string userid)
        //{
        //    var spList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets);
        //    SPQuery query = new SPQuery();
        //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='User'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.AssetOwner, userid);

        //    DataTable dt = spList.GetItems(query).GetDataTable();
        //    return dt;
        //}

        private static double CalculateDailyBudgetAmount(ApplicationContext context, DateTime startDate, DateTime endDate, double budgetAmount)
        {
            int totalDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate);
            if (totalDays > 0)
            {
                return (budgetAmount / totalDays);
            }
            else
            {
                return 0;
            }
        }

        public static DateTime[] GetEndDateByHours(ApplicationContext context, int hours, DateTime startDate)
        {
            int workingHours = GetWorkingHoursInADay(context);
            DateTime[] dates = null;
            int workingDays = 0;
            if (workingHours != 0)
            {
                workingDays = hours / workingHours;
                if (hours % workingHours != 0)
                    workingDays += 1;
            }
            dates = GetEndDateByWorkingDays(context, startDate, workingDays);
            return dates;
        }

        public static string GetStageActionUsersByStageTitle(ApplicationContext context, string stageName, string currentModuleName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            DataRow[] moduleStagesRow = moduleViewManager.LoadModuleListByName(currentModuleName, DatabaseObjects.Tables.ProjectLifeCycles).Select(); //uGITCache.GetDataTable(DatabaseObjects.Columns.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, currentModuleName).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();
            DataRow stage = moduleStagesRow.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTitle) == stageName);
            if (stage != null && stage.Table.Columns.Contains(DatabaseObjects.Columns.ActionUser))
            {
                return Convert.ToString(stage[DatabaseObjects.Columns.ActionUser]);
            }
            return string.Empty;
        }

        //public static DataTable GetModuleList(ModuleType moduleType)
        //{
        //    DataTable moduleList = null;

        //    moduleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules);

        //    if (moduleList == null)
        //        return null;

        //    DataTable filteredModules = null;
        //    if (moduleType == ModuleType.All)
        //        filteredModules = moduleList.Copy();
        //    else
        //    {
        //        string query = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleType, (int)moduleType);
        //        DataRow[] rows = moduleList.Select(query);
        //        if (rows.Length > 0)
        //            filteredModules = rows.CopyToDataTable();
        //        else
        //            filteredModules = moduleList.Clone();
        //    }

        //    return filteredModules;
        //}

        public static DateTime[] GetNewEndDateForExistingDuration(ApplicationContext context, DateTime startDate, DateTime endDate, DateTime newStartDate)
        {
            return GetNewEndDateForExistingDuration(context, startDate, endDate, newStartDate, true);
        }

        public static DateTime[] GetNewEndDateForExistingDuration(ApplicationContext context, DateTime startDate, DateTime endDate, DateTime newStartDate, bool startFromNextDays)
        {
            int duration = GetTotalWorkingDaysBetween(context, startDate, endDate);
            if (startFromNextDays)
            {
                newStartDate = newStartDate.AddDays(1);
            }
            return GetEndDateByWorkingDays(context, newStartDate, duration);
        }

        public static string GetWorkingHourOfWorkingDate(ApplicationContext context, DateTime SourceDateTime)
        {
            string workingDateTime = string.Empty;
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            string WorkdayStartTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayStartTime);
            string WorkdayEndTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayEndTime);
            string TimeFormat = UGITUtility.GetShortTimeFormat();
            // int workingHoursInDay = DateTime.ParseExact(WorkdayEndTime, TimeFormat, CultureInfo.InvariantCulture).Hour - DateTime.ParseExact(WorkdayStartTime, TimeFormat, CultureInfo.InvariantCulture).Hour;  //, DateTime.ParseExact(WorkdayEndTime, "h:mm tt", CultureInfo.InvariantCulture));
            int workingHoursInDay = DateTime.Parse(WorkdayEndTime).Hour - DateTime.Parse(WorkdayStartTime).Hour;  //, DateTime.ParseExact(WorkdayEndTime, "h:mm tt", CultureInfo.InvariantCulture));
            workingDateTime = WorkdayStartTime + ";#" + WorkdayEndTime;
            return workingDateTime;
        }

        public static string GetWorkingDayFromConfigTable(ApplicationContext dbContext, DateTime SourceDateTime, Boolean isForNextWorkingDay)
        {
            string nextWorkingDateTime = string.Empty;
            AppointmentsManager AppManager = new AppointmentsManager(dbContext);
            List<DevExpress.XtraScheduler.Appointment> appointments = AppManager.Pattern;
            OccurrenceCalculator calc = AppManager.Calc;

            foreach (DevExpress.XtraScheduler.Appointment item in appointments)
            {
                if (item.Type == AppointmentType.Pattern)
                {
                    calc = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);

                    TimeSpan duration = item.Duration;
                    DateTime start = calc.CalcOccurrenceStartTime(0);
                    int length = item.RecurrenceInfo.OccurrenceCount;
                    for (int i = 0; i < length; i++)
                    {
                        AppManager.Scheduler.WorkDays.AddHoliday(start.AddDays(i).Date, item.Description);
                    }
                }
                if (item.Type == AppointmentType.Normal)
                {
                    AppManager.Scheduler.WorkDays.AddHoliday(item.Start.Date, item.Description);
                }
            }

            if (isForNextWorkingDay)
                SourceDateTime = SourceDateTime.AddDays(1);
            else
            {
                nextWorkingDateTime = Convert.ToString(SourceDateTime.AddDays(1));
                return nextWorkingDateTime;
            }

            do
            {
                List<DateTime> workingDays = new List<DateTime>();

                if (AppManager.Scheduler.WorkDays.IsWorkDay(SourceDateTime))
                {

                    nextWorkingDateTime = Convert.ToString(SourceDateTime);
                    break;
                }
                SourceDateTime = SourceDateTime.AddDays(1);
            } while (true);

            return nextWorkingDateTime;
        }

        public static DateTime[] GetEndDateByWorkingDays(ApplicationContext dbContext, DateTime startDate, int noOfWorkingDays)
        {
            var originalStartDate = startDate;
            //var daysAdded = 0;

            if (noOfWorkingDays == 0)
            {
                noOfWorkingDays = 1;
            }

            var appointmentsManager = new AppointmentsManager(dbContext);
            var appointments = appointmentsManager.Pattern;

            foreach (DevExpress.XtraScheduler.Appointment item in appointments)
            {
                switch (item.Type)
                {
                    case AppointmentType.Pattern:
                        {
                            var oCalculator = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);
                            //var tsDuration = item.Duration;
                            var startTime = oCalculator.CalcOccurrenceStartTime(0);
                            var length = item.RecurrenceInfo.OccurrenceCount;

                            for (var i = 0; i < length; i++)
                            {
                                appointmentsManager.Scheduler.WorkDays.AddHoliday(startTime.AddDays(i).Date, item.Description);
                            }

                            break;
                        }
                    case AppointmentType.Normal:
                        appointmentsManager.Scheduler.WorkDays.AddHoliday(item.Start.Date, item.Description);
                        break;
                }
            }

            var workingDays = new List<DateTime>();
            var date = startDate;
            bool reverseDayAddition = noOfWorkingDays < 0 ? true : false;
            noOfWorkingDays = noOfWorkingDays < 0 ? (-noOfWorkingDays) : noOfWorkingDays;
            do
            {
                if (appointmentsManager.Scheduler.WorkDays.IsWorkDay(date))
                    workingDays.Add(date);

                date = date.AddDays(reverseDayAddition ? -1 : 1);

            } while (workingDays.Count < noOfWorkingDays);

            var originalEndDate = workingDays[workingDays.Count - 1];

            ////If nothing found consider all days as working
            //else
            //{
            //    orignalStartDate = startDate;
            //    orignalEndDate = startDate.AddDays(noOfWorkingDays - 1);
            //}


            DateTime[] dates = { originalStartDate, originalEndDate };
            return dates;
        }

        public static DataTable GetWorkingHoursInMonth(DataTable holidayList, int month, int year)
        {
            //DateTime startDate = new DateTime(year, month, 1);
            //SPQuery calendarQuery = new SPQuery();
            //calendarQuery.CalendarDate = startDate;
            ////Expands a Multi day event to consider all dates.
            //calendarQuery.ExpandRecurrence = true;
            ////Query gets all events within the start and end dates and which have Category = Holiday
            //calendarQuery.ViewFields = string.Concat(
            //     string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title),
            //    string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.EventDate),
            //    string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.EndDate),
            //    string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Category),
            //    string.Format("<FieldRef Name='{0}'/>", "RecurrenceID"),
            //    string.Format("<FieldRef Name='{0}'/>", "fRecurrence"),
            //       string.Format("<FieldRef Name='{0}' Nullable='True'/>", "RecurrenceData")
            // );
            //// calendarQuery.ViewFieldsOnly = true;
            //calendarQuery.Query = string.Format(@"<Where><And><Eq><FieldRef Name='Category' /><Value Type='Choice'>Work hours</Value></Eq>
            //                                              <DateRangesOverlap>
            //                                              <FieldRef Name='EventDate' />
            //                                              <FieldRef Name='EndDate' />
            //                                              <FieldRef Name='RecurrenceID' />
            //                                              <Value  Type='DateTime'><Month /></Value>
            //                                              </DateRangesOverlap></And></Where>");


            //SPQuery deletedEventsQuery = new SPQuery();
            //deletedEventsQuery.CalendarDate = startDate;

            //deletedEventsQuery.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}' />", DatabaseObjects.Columns.EventDate), string.Format("<FieldRef Name='{0}'/>", "RecurrenceID"));
            //deletedEventsQuery.Query = string.Format(@"<Where>
            //                                                <And>
            //                                                    <Eq><FieldRef Name='Category' /><Value Type='Choice'>Work hours</Value></Eq>
            //                                                    <Eq><FieldRef Name='EventType' /><Value Type='Number'>3</Value></Eq>
            //                                               </And>
            //                                            </Where>");

            ////Get all Holiday events for the given time period
            ////DataRow[] calendarEvents = GetTableDataManager.GetTableData();
            //SPListItemCollection calendarEvents = holidayList.GetItems(calendarQuery);
            //DataTable dtCalenderEvents = calendarEvents.GetDataTable();

            ////Get all deleted events for the given time period
            //SPListItemCollection deletedEvents = holidayList.GetItems(deletedEventsQuery);
            //DataTable dtdeletedEvents = deletedEvents.GetDataTable();

            ////Remove deleted events from the calender events
            //if ((dtCalenderEvents != null && dtCalenderEvents.Rows.Count > 0) && (dtdeletedEvents != null && dtdeletedEvents.Rows.Count > 0))
            //{
            //    DateTime[] drCalenderEvents = dtCalenderEvents.AsEnumerable().Select(x => x.Field<DateTime>(DatabaseObjects.Columns.EventDate).Date).Distinct().ToArray();
            //    DateTime[] drDeletedEvents = dtdeletedEvents.AsEnumerable().Select(x => x.Field<DateTime>(DatabaseObjects.Columns.EventDate).Date).Distinct().ToArray();
            //    if (drCalenderEvents.Length > 0 && drDeletedEvents.Length > 0)
            //    {
            //        drCalenderEvents = drCalenderEvents.Except(drDeletedEvents).ToArray();
            //        if (drCalenderEvents.Length > 0)
            //        {
            //            dtCalenderEvents = drCalenderEvents.ToDataTable();
            //            dtCalenderEvents.Columns["Date"].ColumnName = DatabaseObjects.Columns.EventDate;
            //        }

            //    }
            //}
            DataTable dtCalenderEvents = new DataTable();
            return dtCalenderEvents;
        }

        /// <summary>
        /// Return # of working hours in a day for use in either SLA or task/allocaton hours calculations
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isSLA"></param>
        /// <returns></returns>
        public static int GetWorkingHoursInADay(ApplicationContext context, bool isSLA)
        {
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            if (isSLA)
            {
                // Get hours used to calculate SLAs - can be longer than 8 hours if multiple shifts working
                DateTime workDayStartTime = UGITUtility.StringToDateTime(objConfigurationVariableHelper.GetValue("WorkdayStartTime"));
                DateTime workDayEndTime = UGITUtility.StringToDateTime(objConfigurationVariableHelper.GetValue("WorkdayEndTime"));

                if (workDayStartTime == DateTime.MinValue)
                    workDayStartTime = DateTime.Now.Date;

                if (workDayEndTime == DateTime.MinValue)
                    workDayEndTime = DateTime.Now.Date.AddDays(1);

                return (int)workDayEndTime.Subtract(workDayStartTime).TotalHours;
            }
            else
            {
                // Get standard working hours used for task hour & resource allocation calculations - usually 8 hours per day
                int resourceWorkingHour = UGITUtility.StringToInt(objConfigurationVariableHelper.GetValue("ResourceWorkingHours"), 8);
                return resourceWorkingHour;
            }
        }

        #region Method to Get Duration in Minutes
        /// <summary>
        /// This method is used to Get Duration in Minutes on the basis of working hours 
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="selectedUnit"></param>
        /// <param name="use24x7Calendar"></param>
        /// <returns></returns>
        public static double GetWorkingMinutes(ApplicationContext context, string duration, string selectedUnit, bool use24x7Calendar = false)
        {
            double value = 0.0;
            int workingHoursInADay = use24x7Calendar ? 24 : GetWorkingHoursInADay(context, false);

            // Converting days, hours into minutes
            if (selectedUnit == Constants.SLAConstants.Days)
                value = Convert.ToDouble(duration) * workingHoursInADay * 60;
            else if (selectedUnit == Constants.SLAConstants.Hours)
                value = Convert.ToDouble(duration) * 60;
            else
                value = Convert.ToDouble(duration);

            return value;
        }
        #endregion Method to Get Duration in Minutes

        public static DataTable GetWorkingHoursInMonth(ApplicationContext context, int month, int year)
        {
            AppointmentsManager AppManager = new AppointmentsManager(context);
            List<DevExpress.XtraScheduler.Appointment> appointments = AppManager.Pattern;
            OccurrenceCalculator calc = AppManager.Calc;

            foreach (DevExpress.XtraScheduler.Appointment item in appointments)
            {
                if (item.Type == AppointmentType.Pattern)
                {
                    calc = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);

                    TimeSpan duration = item.Duration;
                    DateTime start = calc.CalcOccurrenceStartTime(0);
                    int length = item.RecurrenceInfo.OccurrenceCount;
                    for (int i = 0; i < length; i++)
                    {
                        AppManager.Scheduler.WorkDays.AddHoliday(start.AddDays(i).Date, item.Description);
                    }
                }
                if (item.Type == AppointmentType.Normal)
                {
                    AppManager.Scheduler.WorkDays.AddHoliday(item.Start.Date, item.Description);
                }
            }

            List<DateTime> workingDays = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                DateTime date = new DateTime(year, month, i);
                if (AppManager.Scheduler.WorkDays.IsWorkDay(date))
                    workingDays.Add(date);
            }
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            string WorkdayStartTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayStartTime);
            string WorkdayEndTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayEndTime);
            string TimeFormat = UGITUtility.GetShortTimeFormat();
            DateTime WorkStartTime = WorkdayStartTime == string.Empty ? DateTime.Today.Date : DateTime.ParseExact(WorkdayStartTime, TimeFormat, CultureInfo.InvariantCulture);
            DateTime WorkEndTime = WorkdayEndTime == string.Empty ? DateTime.Today.Date : DateTime.ParseExact(WorkdayEndTime, TimeFormat, CultureInfo.InvariantCulture);

            TimeSpan workingHoursInDay = WorkEndTime.Subtract(WorkStartTime);
            double totalWoringHoursInMonth = workingHoursInDay.Hours * workingDays.Count + (workingHoursInDay.Minutes * workingDays.Count) / 60;


            return new DataTable();
        }

        public static string GetNextWorkingDateAndTime(ApplicationContext context, DateTime SourceDateTime)
        {
            string nextWorkingDateTime = string.Empty;

            nextWorkingDateTime = GetWorkingDayFromConfigTable(context, SourceDateTime, true);

            return nextWorkingDateTime;
        }

        public static string[] GetMultiLookupValue(string multiLookupField)
        {
            List<string> lstMultilookupValues = new List<string>();
            string[] delim = { Constants.Separator };
            try
            {
                string[] multiLookupValue = multiLookupField.ToString().Split(delim, StringSplitOptions.None);
                for (int i = 0; i < multiLookupValue.Length; i++)
                {
                    if (i % 2 == 0)
                        lstMultilookupValues.Add(multiLookupValue[i]);
                }
                return lstMultilookupValues.ToArray();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return lstMultilookupValues.ToArray();
            }
        }

        public static List<string> GetMultiLookupText(string multiLookupField, string separator)
        {
            List<string> lstMultilookupValues = new List<string>();
            string[] delim = { separator };

            try
            {
                lstMultilookupValues = multiLookupField.ToString().Split(delim, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return lstMultilookupValues;
        }

        public static double GetWorkingMinutesBetweenDates(ApplicationContext context, DateTime startDate, DateTime endDate, bool use24x7Calendar = false, bool isSLA = true)
        {
            if (startDate > endDate)
                return 0;

            if (use24x7Calendar)
                return endDate.Subtract(startDate).TotalMinutes;

            TimeSpan workingHoursInDay;
            int workingMinutesInDay = uHelper.GetWorkingHoursInADay(context, isSLA);
            double workingMinutes = 0;

            AppointmentsManager AppManager = new AppointmentsManager(context);
            List<DevExpress.XtraScheduler.Appointment> appointments = AppManager.Pattern;
            OccurrenceCalculator calc = AppManager.Calc;

            foreach (DevExpress.XtraScheduler.Appointment item in appointments)
            {
                if (item.Type == AppointmentType.Pattern)
                {
                    calc = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);

                    TimeSpan duration = item.Duration;
                    DateTime start = calc.CalcOccurrenceStartTime(0);
                    int length = item.RecurrenceInfo.OccurrenceCount;
                    for (int i = 0; i < length; i++)
                    {
                        AppManager.Scheduler.WorkDays.AddHoliday(start.AddDays(i).Date, item.Description);
                    }
                }
                if (item.Type == AppointmentType.Normal)
                {
                    AppManager.Scheduler.WorkDays.AddHoliday(item.Start.Date, item.Description);
                }
            }

            List<DateTime> workingDays = new List<DateTime>();

            DateTime date = startDate;
            do
            {
                if (AppManager.Scheduler.WorkDays.IsWorkDay(date))
                    workingDays.Add(date);

                date = date.AddDays(1);

            } while (date <= endDate);
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            string WorkdayStartTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayStartTime);
            string WorkdayEndTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayEndTime);

            string TimeFormat = UGITUtility.GetShortTimeFormat();
            //Changed datetime string format(h:mm tt) to (d/M/yyyy h:mm:ss tt) by Munna because our date is in string and  is in this format (d/M/yyyy h:mm:ss tt) 
            //DateTime WorkStartTime = (WorkdayStartTime == string.Empty ? DateTime.Today.Date : DateTime.ParseExact(WorkdayStartTime, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));


            //same as above comment
            //DateTime WorkEndTime = (WorkdayEndTime == string.Empty ? DateTime.Today.Date : DateTime.ParseExact(WorkdayEndTime, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            //BTS-20-000084: Fixed the calculation of TicketTotalHoldDuration 
            DateTime WorkStartTime = string.IsNullOrEmpty(WorkdayStartTime) ? DateTime.Today.Date : UGITUtility.StringToDateTime(WorkdayStartTime);
            DateTime WorkEndTime = string.IsNullOrEmpty(WorkdayEndTime) ? DateTime.Today.Date : UGITUtility.StringToDateTime(WorkdayEndTime);


            workingHoursInDay = WorkEndTime.Subtract(WorkStartTime);
            double wMinuteInWorkingDates = (workingDays.Count * workingHoursInDay.Hours * 60) + (workingHoursInDay.Minutes * workingDays.Count) / 60;
            workingMinutes = wMinuteInWorkingDates;

            return Math.Round(workingMinutes, 2);

        }

        public static int GetWorkingHoursInADay(DateTime workdayStartTime, DateTime workdayEndTime)
        {
            //Calculates total working hours in one day
            DateTime workDayStartTime = workdayStartTime;
            DateTime workDayEndTime = workdayEndTime;
            if (workDayStartTime == DateTime.MinValue)
            {
                workDayStartTime = DateTime.Now.Date;
            }

            if (workDayEndTime == DateTime.MinValue)
            {
                workDayEndTime = DateTime.Now.Date.AddDays(1);
            }
            return (int)workDayEndTime.Subtract(workDayStartTime).TotalHours;
        }

        public static int GetWorkingHoursInADay(ApplicationContext context)
        {
            //Calculates total working hours in one day
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            // Get standard working hours used for task hour & resource allocation calculations - usually 8 hours per day
            int resourceWorkingHour = UGITUtility.StringToInt(objConfigurationVariableHelper.GetValue("ResourceWorkingHours"));
            if (resourceWorkingHour > 0)
                return resourceWorkingHour;
            string WorkdayStartTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayStartTime);
            string WorkdayEndTime = objConfigurationVariableHelper.GetValue(ConfigConstants.WorkdayEndTime);
            DateTime workDayStartTime = string.IsNullOrEmpty(WorkdayStartTime) ? DateTime.Today.Date : UGITUtility.StringToDateTime(WorkdayStartTime);
            DateTime workDayEndTime = string.IsNullOrEmpty(WorkdayEndTime) ? DateTime.Today.Date : UGITUtility.StringToDateTime(WorkdayEndTime);
            if (workDayStartTime == DateTime.MinValue)
            {
                workDayStartTime = DateTime.Now.Date;
            }

            if (workDayEndTime == DateTime.MinValue)
            {
                workDayEndTime = DateTime.Now.Date.AddDays(1);
            }
            return (int)workDayEndTime.Subtract(workDayStartTime).TotalHours;
        }

        public static List<DateTime> GetTotalWorkingDateBetween(ApplicationContext context, DateTime startDate, DateTime endDate)
        {
            List<DateTime> workingDateList = new List<DateTime>();

            AppointmentsManager AppManager = new AppointmentsManager(context);
            List<DevExpress.XtraScheduler.Appointment> appointments = AppManager.Pattern;
            OccurrenceCalculator calc = AppManager.Calc;

            foreach (DevExpress.XtraScheduler.Appointment item in appointments)
            {
                if (item.Type == AppointmentType.Pattern)
                {
                    calc = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);

                    TimeSpan duration = item.Duration;
                    DateTime start = calc.CalcOccurrenceStartTime(0);
                    int length = item.RecurrenceInfo.OccurrenceCount;
                    for (int i = 0; i < length; i++)
                    {
                        AppManager.Scheduler.WorkDays.AddHoliday(start.AddDays(i).Date, item.Description);
                    }
                }
                if (item.Type == AppointmentType.Normal)
                {
                    AppManager.Scheduler.WorkDays.AddHoliday(item.Start.Date, item.Description);
                }
            }

            DateTime date = startDate;
            do
            {
                if (AppManager.Scheduler.WorkDays.IsWorkDay(date))
                    workingDateList.Add(date);

                date = date.AddDays(1);

            } while (date <= endDate);

            return workingDateList;
        }

        public static List<DateTime> GetPreviousWorkingDates(ApplicationContext context, DateTime dtCurrentDate, int datesCount)
        {
            List<DateTime> lstPreviousDates = new List<DateTime>();
            DateTime dtEndDate = dtCurrentDate;
            datesCount = Math.Abs(datesCount);
            DateTime dtStartDate = dtCurrentDate.AddDays(-10);
            int selectedDates = datesCount;
            int count = 0;
            while (count < datesCount)
            {
                List<DateTime> lstDates = GetTotalWorkingDateBetween(context, dtStartDate, dtEndDate);
                if (lstDates != null && lstDates.Count > 0)
                {
                    // SelectedDates = lstDates.Count;
                    selectedDates = (lstDates.Count > selectedDates) ? selectedDates : lstDates.Count;
                    lstDates = lstDates.OrderByDescending(x => x).Take(selectedDates).ToList();
                    lstPreviousDates.AddRange(lstDates);
                    selectedDates = datesCount - selectedDates;
                    count = count + selectedDates;
                }
                if (lstPreviousDates.Count < datesCount)
                {
                    dtEndDate = dtStartDate;
                    dtStartDate = dtStartDate.AddDays(-10);
                }
            }
            return lstPreviousDates;
        }

        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static int GetTotalWorkingDaysBetween(ApplicationContext context, DateTime startDate, DateTime endDate)
        {
            return GetTotalWorkingDaysBetween(context, startDate, endDate, true);
        }

        public static double GetTotalWorkingDayTimeMinuteBetween(ApplicationContext context, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                //Log.WriteLog("ERROR: Start Date greater than end Date in GetTotalWorkingDaysBetween");
                return 0.0;
            }

            if (startDate.Date == DateTime.MinValue.Date || endDate.Date == DateTime.MaxValue.Date)
            {
                ULog.WriteLog("ERROR: Invalid dates passed in to GetTotalWorkingDaysBetween");
                return 0.0;
            }



            var calculator = new Calculation(new List<DateTime>(), new OpenHours("09:00;17:00"));
            var result = calculator.getElapsedMinutes(startDate, endDate);


            return result;
        }

        public static int GetWeeksFromDays(ApplicationContext context, int days)
        {
            int daysPerWeek = 7;
            int calculatedWeeks = 0;
            int remainder = 0;
            ConfigurationVariableManager obcConfiguationVariableHelper = new ConfigurationVariableManager(context);
            if (days > 0)
            {
                string[] weekEndDays = UGITUtility.SplitString(obcConfiguationVariableHelper.GetValue(ConfigConstants.WeekendDays), ",");
                if (weekEndDays != null && weekEndDays.Length > 0)
                {
                    daysPerWeek = daysPerWeek - weekEndDays.Length;
                }
                ////if (days % daysPerWeek == 0)

                //calculatedWeeks =  days )/ daysPerWeek;
                calculatedWeeks = Math.DivRem(days, daysPerWeek, out remainder);


            }
            return remainder == 0 ? calculatedWeeks : calculatedWeeks + 1;
            //return calculatedWeeks;
        }

        public static int GetWorkingDaysInWeeks(ApplicationContext context, int week)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            int daysPerWeek = 7;
            int calculatedDays = 0;
            if (week > 0)
            {
                string[] weekEndDays = UGITUtility.SplitString(configurationVariableManager.GetValue(ConfigConstants.WeekendDays).Trim(), ",");
                if (weekEndDays != null && weekEndDays.Length > 0)
                {
                    daysPerWeek = daysPerWeek - weekEndDays.Length;
                }
                calculatedDays = week * daysPerWeek;
            }
            return calculatedDays;
        }

        public static string GetProjectComplexity(ApplicationContext context, double appxContractValue)
        {
            ProjectComplexityManager projectComplexityManager = new ProjectComplexityManager(context);
            List<ProjectComplexity> lstCriteria = projectComplexityManager.Load(x => x.Deleted != true).ToList();
            string projectComplexity = string.Empty;

            foreach (var item in lstCriteria)
            {
                if (item.MinValue <= appxContractValue && item.MaxValue >= appxContractValue)
                {
                    projectComplexity = item.CRMProjectComplexity;
                    break;
                }
            }

            return projectComplexity;
        }

        public static string GetTempFolderPath()
        {
            return HttpContext.Current.Server.MapPath("~/Content/IMAGES/ugovernit/upload/");

        }

        public static string GetTempFolderPathNew()
        {
            // return context.Server.MapPath("~/Content/IMAGES/ugovernittemp/");
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Content/IMAGES/ugovernit/upload/");

        }

        public static string GetUploadFolderPath()
        {
            return HttpContext.Current.Server.MapPath("~/Content/IMAGES/ugovernit/upload/");
        }

        //Add function for get default service xml data
        public static string GetDefaultServicesPath()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/uGovernITServices/");
        }

        public static string GetDataMigrationTemplate()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/DataMigration/demo.settings.json");
        }

        public static string ReplaceInvalidCharsInURL(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return Regex.Replace(input, "[\\\\#'\"]", string.Empty);
            else
                return string.Empty;
        }

        public static string[] GetMyTokens(string TokenString)
        {
            string tokenRejX = Constants.TokenRegx;
            MatchCollection matchedTokens = Regex.Matches(TokenString, tokenRejX, RegexOptions.IgnoreCase);
            Match[] tokenList = new Match[matchedTokens.Count];
            matchedTokens.CopyTo(tokenList, 0);
            return tokenList.Select(x => x.Value).ToArray();
        }

        public static DateTime GetWorkingEndDate(ApplicationContext context, DateTime startDate, double minute, bool isSLA = false)
        {
            DateTime endDate = DateTime.MinValue;
            ConfigurationVariableManager obcConfiguationVariableHelper = new ConfigurationVariableManager(context);
            DateTime workDayStartTime = Convert.ToDateTime(obcConfiguationVariableHelper.GetValue("WorkdayStartTime"));
            DateTime workDayEndTime = Convert.ToDateTime(obcConfiguationVariableHelper.GetValue("WorkdayEndTime"));
            double workingMinutesInDay = uHelper.GetWorkingHoursInADay(context, isSLA) * 60;

            //check start date is working on not
            DateTime[] arr = uHelper.GetEndDateByWorkingDays(context, startDate, 1);
            if (arr.Length > 0 && arr[0].Date > startDate.Date)
            {
                startDate = arr[0].Date;
            }

            if (startDate.TimeOfDay < workDayStartTime.TimeOfDay)
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, workDayStartTime.Hour, workDayStartTime.Minute, workDayStartTime.Second);
            if (startDate.TimeOfDay > workDayEndTime.TimeOfDay)
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, workDayEndTime.Hour, workDayEndTime.Minute, workDayEndTime.Second); ;


            double startDateRemainingWorkingMinutes = workDayEndTime.TimeOfDay.TotalMinutes - startDate.TimeOfDay.TotalMinutes;
            if (startDateRemainingWorkingMinutes >= minute)
            {
                endDate = startDate.AddMinutes(minute);
            }
            else
            {
                minute = minute - startDateRemainingWorkingMinutes;
                startDate = startDate.AddDays(1);
                double totalWorkingDays = minute / workingMinutesInDay;
                int completeWorkingDays = (int)totalWorkingDays;
                if (minute % workingMinutesInDay != 0)
                    completeWorkingDays += 1;

                DateTime[] nextWorkingDates = uHelper.GetEndDateByWorkingDays(context, startDate, completeWorkingDays);
                endDate = nextWorkingDates[1];

                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, workDayStartTime.Hour, workDayStartTime.Minute, workDayStartTime.Second);

                minute = (totalWorkingDays - Math.Floor(totalWorkingDays)) * workingMinutesInDay;
                endDate = endDate.AddMinutes(minute);
            }

            return endDate;
        }

        public static int GetTotalWorkingDaysBetween(ApplicationContext context, DateTime startDate, DateTime endDate, bool minOneDay)
        {
            if (startDate > endDate)
            {
                //Log.WriteLog("ERROR: Start Date greater than end Date in GetTotalWorkingDaysBetween");
                return 0;
            }

            if (startDate.Date == DateTime.MinValue.Date || endDate.Date == DateTime.MaxValue.Date)
            {
                ULog.WriteLog("ERROR: Invalid dates passed in to GetTotalWorkingDaysBetween");
                return 0;
            }

            int noOfWorkingDays = 0;

            //Check for a holiday calendar defined in the configuration
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            if (objConfigurationVariableHelper.GetValueAsBool(DatabaseObjects.Columns.UseCalendar))
            {
                DataTable calendarList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.HolidaysAndWorkDaysCalendar, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");  //SPListHelper.GetSPList(DatabaseObjects.Lists.HolidaysAndWorkDaysCalendar, spWeb);
                if (calendarList != null && calendarList.Rows.Count > 0)
                {
                    // Start from 1st of the month of start date to account for case when endDate day is less than startDate day
                    DateTime currentStartDate = new DateTime(startDate.Year, startDate.Month, 1);
                    while (currentStartDate <= endDate)
                    {
                        //Get working days list of specified month and year
                        DataTable workingDates = GetWorkingHoursInMonth(context, currentStartDate.Month, currentStartDate.Year);

                        //Get working days for this month between start and end date
                        if (workingDates != null)
                        {
                            DateTime[] existingWorkingDates = workingDates.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.EventDate).Date >= startDate.Date && x.Field<DateTime>(DatabaseObjects.Columns.EventDate).Date <= endDate.Date &&
                                                                                                x.Field<DateTime>(DatabaseObjects.Columns.EventDate).Month == currentStartDate.Month).Select(x => x.Field<DateTime>(DatabaseObjects.Columns.EventDate)).Distinct().ToArray();
                            noOfWorkingDays += existingWorkingDates.Length;
                        }
                        currentStartDate = currentStartDate.AddMonths(1);
                    }
                }
            }
            //If no calendar found check for the weekly holiday entries in the configuration
            else if (objConfigurationVariableHelper.GetValue(DatabaseObjects.Columns.Holidays) != string.Empty)
            {
                string[] Holidays = objConfigurationVariableHelper.GetValue(DatabaseObjects.Columns.Holidays).ToString().Split(',');
                while (startDate <= endDate)
                {
                    if (Holidays.FirstOrDefault(x => x.ToLower() == startDate.DayOfWeek.ToString().ToLower()) == null)
                    {
                        noOfWorkingDays++;
                    }
                    startDate = startDate.AddDays(1);
                }
            }
            //If nothing found consider all days as working
            else
            {
                noOfWorkingDays = (endDate - startDate).Days + 1;
            }

            // If minimum working days have to be 1 (assuming start date is less than or equal to end date), adjust zero up to 1
            if (minOneDay && noOfWorkingDays == 0)
                noOfWorkingDays = 1;

            return noOfWorkingDays;
        }

        public static DateTime FirstDayOfWeek(ApplicationContext context, DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public static int GetRecurringIntervalInMinutes(ApplicationContext context, string repeatInterval, DateTime currentReminderTime)
        {
            int recurringInterval = 0;
            DateTime newDateTime = new DateTime();
            switch (repeatInterval)
            {
                case "Every 1 Week":
                    newDateTime = currentReminderTime.AddDays(7);
                    break;
                case "Every 2 Weeks":
                    newDateTime = currentReminderTime.AddDays(14);
                    break;
                case "Every 1 Month":
                    newDateTime = currentReminderTime.AddMonths(1);
                    break;
                case "Every Quarter":
                case "Every Quarterly": // Old typo
                    newDateTime = currentReminderTime.AddMonths(3);
                    break;
                case "Every 6 Months":
                    newDateTime = currentReminderTime.AddMonths(6);
                    break;
                case "Every Year":
                    newDateTime = currentReminderTime.AddYears(1);
                    break;
                default:
                    break;
            }
            TimeSpan outresult = newDateTime.Subtract(currentReminderTime);
            recurringInterval = (int)outresult.TotalMinutes;
            return recurringInterval;
        }

        public static Dictionary<DateTime, double> DistributeAmount(ApplicationContext context, DateTime startDate, DateTime endDate, double amount)
        {
            Dictionary<DateTime, double> distributeA = new Dictionary<DateTime, double>();

            // Sanity check JUST in case dates are wrong, else get NaN for daily amounts causing errors down the chain!
            if (startDate > endDate)
            {
                string errorMsg = string.Format("ERROR in UpdateMonthlyDistribution - found start date {0} > end date {1} for amount {2}", startDate, endDate, amount);
                ULog.WriteLog(errorMsg);
                return distributeA;
            }

            int totalDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate, true);
            double oneDayAmount = amount / totalDays;
            int totalMonth = 0;
            if (startDate.Year == endDate.Year)
                totalMonth = endDate.Month - startDate.Month + 1;
            else
                totalMonth = (12 - startDate.Month + 1) + (12 * (endDate.Year - startDate.Year - 1)) + endDate.Month;

            DateTime tempDate = new DateTime(startDate.Year, startDate.Month, 1);
            int yearDiff = endDate.Year - startDate.Year;
            double totalAmount = 0;
            for (int m = 0; m < totalMonth; m++)
            {
                int daysInMonth = 0;
                double monthlyAmount = 0;

                if (tempDate.Year == startDate.Year && tempDate.Month == startDate.Month && tempDate.Year == endDate.Year && tempDate.Month == endDate.Month)
                {
                    // If start & end dates are in same actual year and month
                    // just get diff in days between two dates
                    daysInMonth = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate, true);
                }
                else if (tempDate.Year == startDate.Year && tempDate.Month == startDate.Month)
                {
                    // we are in first (start) month
                    // get num days from start date to month end
                    daysInMonth = uHelper.GetTotalWorkingDaysBetween(context, startDate, new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)), true);
                }
                else if (tempDate.Year == endDate.Year && tempDate.Month == endDate.Month)
                {
                    // we are in last (end) month
                    // get num days till month start to end date
                    daysInMonth = uHelper.GetTotalWorkingDaysBetween(context, tempDate, endDate, true);
                }
                else
                {
                    // Else we need the whole of current month
                    daysInMonth = uHelper.GetTotalWorkingDaysBetween(context, tempDate, new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month)), true);
                }

                monthlyAmount = oneDayAmount * daysInMonth;
                distributeA.Add(tempDate, monthlyAmount);
                tempDate = tempDate.AddMonths(1);
                totalAmount += monthlyAmount;
            }

            if (Math.Round(totalAmount, 2) != Math.Round(amount, 2))
            {
                // Sanity check :-)
                ULog.WriteLog("ERROR: distributed amount " + Math.Round(totalAmount, 2).ToString() + " != input amount " + Math.Round(amount, 2).ToString());
            }

            return distributeA;
        }

        public static Dictionary<DateTime, double> MonthlyDistributeFTEs(ApplicationContext context, DateTime startDate, DateTime endDate, double FTE)
        {
            Dictionary<DateTime, double> monthlyDistribution = new Dictionary<DateTime, double>();
            do
            {
                DateTime firstDayOfMonth = uHelper.FirstDayOfMonth(startDate);
                DateTime lastDayOfMonth = uHelper.LastDayOfMonth(startDate);

                ///data creating for monthly Allocation for tempary basis
                DateTime monthEndDate = uHelper.LastDayOfMonth(startDate) >= endDate ? endDate : uHelper.LastDayOfMonth(startDate);
                double workingdaysinmonth = uHelper.GetTotalWorkingDaysBetween(context, startDate, monthEndDate);
                double totaldays = uHelper.GetTotalWorkingDaysBetween(context, firstDayOfMonth, lastDayOfMonth);
                double pctAllocation = (workingdaysinmonth / totaldays) * FTE * 100;

                monthlyDistribution.Add(uHelper.FirstDayOfMonth(startDate), pctAllocation / 100);

                startDate = uHelper.FirstDayOfMonth(startDate.AddMonths(1));

            } while (startDate <= endDate);

            return monthlyDistribution;
        }

        /// <summary>
        /// Routine that returns a list of history entries.
        /// Each HistoryEntry has the user, timestamp and text in string format
        /// Assumes each versioned entry in the column in stored in one of these three formats:
        /// Assume <version1>$;#$<version2>$;#$<version3>
        /// <userID>;#<timestamp>;#<text>
        /// <userID>;#<text>
        /// <text>
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="colName"></param>
        /// <param name="oldestFirst"></param>
        /// <returns></returns>
        /// 

        public static List<HistoryEntry> GetHistory(DataRow ticket, string colName)
        {
            return GetHistory(ticket, colName, false);
        }

        public static List<HistoryEntry> GetHistory(DataRow ticket, string colName, bool oldestFirst)
        {
            List<HistoryEntry> dataList = new List<HistoryEntry>();
            //List<string> versionEntries = new List<string>();  // BTS-23-001023 . commented as repeated allocations getting removed
            string rowValue = string.Empty;
            if (UGITUtility.IsSPItemExist(ticket, colName))
                rowValue = UGITUtility.ObjectToString(ticket[colName]);
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(rowValue.Trim());
            //dataList = (List<HistoryEntry>)uHelper.DeSerializeAnObject(doc, dataList);
            //HistoryEntry entry = new HistoryEntry();
            //entry.entry = ticket[colName].ToString();
            //entry.created = DateTime.Now.ToString();
            //entry.createdBy = "Naveen";
            //entry.IsPrivate = true;
            //dataList.Add(entry);
            // return dataList;


            if (ticket.HasVersion(DataRowVersion.Current))
            {
                // Get all Versions for the specified column (from oldest to newest)
                //for (int i = ticket.Table.Columns.Count - 1; i >= 0; i--)
                //{
                // Use case: <version1><userID>;#<timestamp>;#<text></version1>$;#$<version2><userID>;#<timestamp>;#<text></version2>$;#$<version3>                
                string rawData = Convert.ToString(rowValue);
                if (rawData != string.Empty)
                {
                    string[] versionsDelim = { Constants.SeparatorForVersions };
                    string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string version in versions)
                    {
                        // Should be in format <userID>;#<timestamp>;#<text>
                        string[] versionDelim = { Constants.Separator };
                        string[] versionData = version.Split(versionDelim, StringSplitOptions.None);

                        HistoryEntry entry = new HistoryEntry();
                        DateTime createdDate;

                        if (versionData.GetLength(0) == 3 || versionData.GetLength(0) == 4)
                        {
                            // Assume <userID>;#<timestamp>;#<text>
                            //entry.createdBy = uHelper.GetUserById(int.Parse(splitString[0])).Name;
                            entry.createdBy = versionData[0];
                            if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                            {
                                DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate);
                                entry.created = createdDate.ToLocalTime().ToString("MMM-d-yyyy hh:mm:ss tt");
                            }
                            else
                            {
                                DateTime.TryParse(versionData[1], out createdDate);
                                entry.created = createdDate.ToString("MMM-d-yyyy hh:mm:ss tt");
                            }
                            entry.entry = versionData[2].Replace("\r\n", "<br>");
                            //to add private comment field to versionData
                            if (versionData.GetLength(0) == 4)
                                entry.IsPrivate = UGITUtility.StringToBoolean(versionData[3]);
                        }
                        else
                        {
                            // Assume whole data is one string
                            // NOTE: ticket.Versions[i].CreatedBy.User.Name will crash if user doesn't exist any more!
                            //entry.createdBy = ticket.Versions[i].CreatedBy.LookupValue;
                            //entry.created = ticket.Versions[i].Created.ToLocalTime().ToString("MMM-d-yyyy hh:mm:ss tt");
                            entry.entry = version.Replace("\r\n", "<br>");
                        }

                        if (!string.IsNullOrEmpty(entry.entry))
                            dataList.Add(entry);
                    }
                }

                // If we want newest to oldest, reverse the list
                if (oldestFirst == false)
                    dataList.Reverse();
            }

            return dataList;
        }

        public static List<HistoryEntry> GetHistory(string ticket)
        {
            return GetHistory(ticket, false);
        }

        public static List<HistoryEntry> GetHistory(string ticket, bool oldestFirst)
        {
            List<HistoryEntry> dataList = new List<HistoryEntry>();
            List<string> versionEntries = new List<string>();
            string rowValue = ticket;
            string rawData = Convert.ToString(ticket);
            if (rawData != string.Empty)
            {
                string[] versionsDelim = { Constants.SeparatorForVersions };
                string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

                foreach (string version in versions)
                {
                    // Keep track of entries to prevent duplicates
                    // (that were unfortunately caused by versioning + updates from backend)
                    if (versionEntries.Contains(version))
                        continue;
                    versionEntries.Add(version);
                    // Should be in format <userID>;#<timestamp>;#<text>
                    string[] versionDelim = { Constants.Separator };
                    string[] versionData = version.Split(versionDelim, StringSplitOptions.None);
                    HistoryEntry entry = new HistoryEntry();
                    DateTime createdDate;
                    if (versionData.GetLength(0) == 3 || versionData.GetLength(0) == 4)
                    {
                        // Assume <userID>;#<timestamp>;#<text>
                        //entry.createdBy = uHelper.GetUserById(int.Parse(splitString[0])).Name;
                        entry.createdBy = versionData[0];
                        if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                        {
                            DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate);
                            entry.created = createdDate.ToLocalTime().ToString("MMM-d-yyyy hh:mm tt");
                        }
                        else
                        {
                            DateTime.TryParse(versionData[1], out createdDate);
                            entry.created = createdDate.ToString("MMM-d-yyyy hh:mm tt");
                        }
                        entry.entry = versionData[2].Replace("\r\n", "<br>");
                        //to add private comment field to versionData
                        if (versionData.GetLength(0) == 4)
                            entry.IsPrivate = UGITUtility.StringToBoolean(versionData[3]);
                    }
                    else
                    {
                        // Assume whole data is one string
                        // NOTE: ticket.Versions[i].CreatedByUser.User.Name will crash if user doesn't exist any more!
                        //entry.createdBy = ticket.Versions[i].CreatedByUser.LookupValue;
                        //entry.created = ticket.Versions[i].Created.ToLocalTime().ToString("MMM-d-yyyy hh:mm tt");
                        entry.entry = version.Replace("\r\n", "<br>");
                    }
                    dataList.Add(entry);
                }
            }
            // If we want newest to oldest, reverse the list
            if (oldestFirst == false)
                dataList.Reverse();
            return dataList;
        }

        public static string GetCommentsbyDataList(List<HistoryEntry> dataList)
        {
            string strComment = string.Empty;
            foreach (var item in dataList)
            {
                if (!string.IsNullOrEmpty(strComment))
                    strComment += Constants.SeparatorForVersions;
                if (item.IsPrivate)
                {
                    strComment += item.createdBy + Constants.Separator + Constants.UTCPrefix + TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(item.created)) + Constants.Separator + item.entry + Constants.Separator + item.IsPrivate;
                }
                else
                {
                    strComment += item.createdBy + Constants.Separator + Constants.UTCPrefix + TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(item.created)) + Constants.Separator + item.entry;
                }
                
            }
            return strComment;
        }

        public static bool CreateHistory(UserProfile user, string historyDescription, DataRow historyFor, ApplicationContext context)
        {
            return CreateHistory(user, historyDescription, historyFor, true, context);
        }

        public static bool CreateHistory(UserProfile user, string historyDescription, DataRow historyFor, bool updateItem, ApplicationContext context)
        {
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.History, historyFor.Table))
            {
                try
                {
                    historyFor[DatabaseObjects.Columns.History] = GetVersionString(user.Id, historyDescription, historyFor, DatabaseObjects.Columns.History);
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Exception through from CreateHistory " + ex.Message);
                }

                //if (updateItem)

                return true;
            }

            return false;
        }

        public static string GetVersionString(string user, string versionDescription, DataRow versionFor, string internalName)
        {
            string oldVersionString = string.Empty;
            // Check for non-versioned value
            string prevValue = Convert.ToString(versionFor[internalName]);
            // In case the non-versioned value is empty we need to Check for versioned value
            if (prevValue == string.Empty /*&& Convert.ToInt32( versionFor["ID"]) != 0 && Convert.ToInt32( versionFor["Versions"]) != 0*/)
            {
                prevValue = Convert.ToString(versionFor[internalName]);
            }
            if (prevValue != string.Empty)
                oldVersionString = prevValue + Constants.SeparatorForVersions;

            return oldVersionString + user + Constants.Separator
                        + Constants.UTCPrefix + DateTime.UtcNow.ToString() + Constants.Separator
                        + versionDescription;
        }

        public static DropDownList GetRequestTypesWithCategoriesDropDownOnChange(ApplicationContext context, DataRow moduleRow, string requestCategories, bool hidePrefix, bool includeServiceTypes, string requestSubCategory, DataRow[] drRequestType, List<string> selectedRequesttypes = null)
        {
            DropDownList ddl = new DropDownList();
            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
            DataRow[] selectedRTS = drRequestType; // uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            DataTable requestTypeTable = null;
            if (selectedRTS == null || selectedRTS.Length == 0)
                selectedRTS = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{moduleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select();
            //if module requestTypes are not exist then return empty dropdownlist
            if (selectedRTS == null || selectedRTS.Length <= 0)
                return ddl;

            requestTypeTable = selectedRTS.CopyToDataTable();

            DataRow[] requestTypes = new DataRow[0];

            string selectQuery = string.Empty;
            string includeServiceTypeQuery = string.Empty;
            if (!includeServiceTypes)
                includeServiceTypeQuery = string.Format(" and ({0} = 'False' or {0} is null)", DatabaseObjects.Columns.ServiceWizardOnly);


            string categoryQuery = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestCategories))
            {
                if (requestCategories.IndexOf(";#") == -1)
                {
                    categoryQuery = string.Format("And ({0}='{1}'", DatabaseObjects.Columns.Category, requestCategories);

                    if (!string.IsNullOrWhiteSpace(requestSubCategory))
                    {
                        categoryQuery += string.Format("And {0}='{1}'", DatabaseObjects.Columns.SubCategory, requestSubCategory);
                    }
                    categoryQuery += ")";
                }
                else
                {
                    string[] selectedArray = requestCategories.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> categoryExps = new List<string>();
                    foreach (string sCategory in selectedArray)
                    {
                        categoryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Category, sCategory));
                    }
                    categoryQuery = string.Format("And ({0})", string.Join(" OR ", categoryExps));
                }
            }

            selectQuery = string.Format("({0} = 'False' or {0} is null){2}{1}",
                                                           DatabaseObjects.Columns.Deleted, includeServiceTypeQuery, categoryQuery);

            requestTypes = requestTypeTable.Select(selectQuery);
            //new block for order by request type on the base of sort to bottom column....
            DataTable subCategorydt = new DataTable();
            if (requestTypes.Length > 0)
            {
                if (selectedRequesttypes != null && selectedRequesttypes.Count > 0)
                {
                    requestTypes = (from s in requestTypes
                                    join
                                        p in selectedRequesttypes on
                                        s.Field<int>(DatabaseObjects.Columns.Id).ToString() equals p
                                    select s).ToArray();
                }

                if (requestTypes.Length > 0)
                {
                    subCategorydt = requestTypes.CopyToDataTable();
                    if (!subCategorydt.Columns.Contains("SortRequestTypeCol"))
                        subCategorydt.Columns.Add("SortRequestTypeCol", typeof(int));
                    subCategorydt.Columns["SortRequestTypeCol"].Expression = string.Format("IIF([{0}] = 1, 1, 0)", DatabaseObjects.Columns.SortToBottom);
                    subCategorydt.DefaultView.Sort = "SortRequestTypeCol ASC, " + DatabaseObjects.Columns.TicketRequestType + " ASC";
                }
            }

            if (subCategorydt == null || subCategorydt.Rows.Count == 0)
                return ddl;

            DataRow[] drnewRequestType = subCategorydt.Select();
            return GetRequestTypeDropDown(context, moduleRow, drnewRequestType, hidePrefix);
        }

        public static DropDownList GetRequestTypesWithCategoriesDropDown(ApplicationContext context, long moduleId)
        {
            DataRow moduleRow = null;
            DataTable module = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ID} = {moduleId}");
            if (module != null && module.Rows.Count > 0)
            {
                moduleRow = module.NewRow();
                moduleRow = module.Select()[0];
            }

            if (moduleRow == null)
                return new DropDownList();

            return GetRequestTypesWithCategoriesDropDown(context, moduleRow);
        }

        public static DropDownList GetRequestTypesWithCategoriesDropDown(ApplicationContext context, string moduleName)
        {
            DataRow moduleRow = null;
            DataTable module = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ModuleName} = '{moduleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (module != null && module.Rows.Count > 0)
            {
                moduleRow = module.NewRow();
                moduleRow = module.Select()[0];
            }

            if (moduleRow == null)
                return new DropDownList();

            return GetRequestTypesWithCategoriesDropDown(context, moduleRow);
        }

        public static DropDownList GetRequestTypesWithCategoriesDropDown(ApplicationContext context, DataRow moduleRow)
        {
            return GetRequestTypesWithCategoriesDropDown(context, moduleRow, null, false, false, null);
        }

        public static DropDownList GetRequestTypesWithCategoriesDropDown(ApplicationContext context, DataRow moduleRow, List<string> selectedRequesttypes, bool hidePrefix, bool includeServiceTypes, DataRow[] drRequestType)
        {
            DropDownList ddl = new DropDownList();
            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);

            DataRow[] selectedRTS = drRequestType;

            if (selectedRTS == null)
            {
                selectedRTS = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{moduleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select();
            }

            DataTable requestTypeTable = null;
            //if module requestTypes are not exist then return empty dropdownlist
            if (selectedRTS == null || selectedRTS.Length <= 0)
                return ddl;

            requestTypeTable = selectedRTS.CopyToDataTable();

            DataRow[] requestTypes = new DataRow[0];

            string selectQuery = string.Empty;
            string includeServiceTypeQuery = string.Empty;
            if (!includeServiceTypes)
                includeServiceTypeQuery = string.Format(" and ({0} = 'False' or {0} is null)", DatabaseObjects.Columns.ServiceWizardOnly);


            selectQuery = string.Format("({0} = 'False' or {0} is null){1}",
                                                           DatabaseObjects.Columns.Deleted, includeServiceTypeQuery);
            requestTypes = requestTypeTable.Select(selectQuery);

            //Show only selected Request types
            if (selectedRequesttypes != null && selectedRequesttypes.Count > 0)
            {
                requestTypes = requestTypes.Where(x => selectedRequesttypes.Contains(x.Field<long>(DatabaseObjects.Columns.Id).ToString())).ToArray();
            }

            return GetRequestTypeDropDown(context, moduleRow, requestTypes, hidePrefix);
        }
        public static DropDownList GetCategoryWithSubCategoriesDropDown(ApplicationContext context, DataRow moduleRow)
        {
            return GetCategoryWithSubCategoriesDropDown(context, moduleRow, null, false);
        }
        public static DropDownList GetCategoryWithSubCategoriesDropDown(ApplicationContext context, DataRow moduleRow, DataRow[] drRequestType, bool includeServiceTypes, List<string> selectedRequesttypes = null)
        {
            DropDownList ddl = new DropDownList();
            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
            RequestTypeManager requestTypeManager = new RequestTypeManager(context);
            List<ModuleRequestType> moduleRequestTypes = requestTypeManager.Load(x => x.ModuleNameLookup.EqualsIgnoreCase(moduleName));
            List<ModuleRequestType> selectedRTS = moduleRequestTypes;// uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            DataTable requestTypeTable = null;
            //if module requestTypes are not exist then return empty dropdownlist
            if (selectedRTS == null || selectedRTS.Count <= 0)
                return ddl;
            requestTypeTable = UGITUtility.ToDataTable(selectedRTS);
            //requestTypeTable = selectedRTS.CopyToDataTable();

            DataRow[] requestTypes = new DataRow[0];

            string selectQuery = string.Empty;
            string includeServiceTypeQuery = string.Empty;
            if (!includeServiceTypes)
                includeServiceTypeQuery = string.Format(" and ({0} = 'False' or {0} is null)", DatabaseObjects.Columns.ServiceWizardOnly);

            selectQuery = string.Format("({0} = 'False' or {0} is null){1}",
                                                           DatabaseObjects.Columns.Deleted, includeServiceTypeQuery);

            requestTypes = requestTypeTable.Select(selectQuery);

            //Show only selected Request types
            if (selectedRequesttypes != null && selectedRequesttypes.Count > 0)
            {
                requestTypes = requestTypes.Where(x => selectedRequesttypes.Contains(x.Field<int>(DatabaseObjects.Columns.Id).ToString())).ToArray();
            }

            var groupData = requestTypes.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category)).OrderBy(x => x.Key);

            ListItem item;
            string style = string.Empty;
            foreach (var category in groupData)
            {
                //Provide "--select--" option in case of single categeory otherwise make dropdown visibility false
                var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory)).OrderBy(x => x.Key);
                if (groupData.Count() == 1 && (true/*!uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory)*/
                    || (subCategories.Count() == 1 && string.IsNullOrWhiteSpace(subCategories.First().Key))))
                {
                    ddl.Visible = false;
                    break;
                }

                if (groupData.Count() > 1)
                {
                    item = new ListItem(category.Key, category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }
                else if (groupData.Count() == 1 && true /*uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory)*/)
                {
                    item = new ListItem("--Select--", category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }

                foreach (var subCategory in subCategories)
                {
                    if (!string.IsNullOrEmpty(subCategory.Key))
                    {
                        item = new ListItem(string.Format("{0}{1}", HttpUtility.HtmlDecode(HttpUtility.HtmlDecode("&nbsp;&nbsp;")), subCategory.Key), string.Format("{0};#;{1}", category.Key, subCategory.Key));
                        item.Attributes.Add("style", string.Format("float:left;padding-left:10px;"));
                        ddl.Items.Add(item);
                    }
                }
            }

            if (groupData.Count() > 1)
                ddl.Items.Insert(0, new ListItem("All Categories", string.Empty));

            return ddl;
        }
        public static DropDownList GetCategoryWithSubCategoriesDropDown(UGITModule moduleRow, DataRow[] drRequestType, bool includeServiceTypes, List<string> selectedRequesttypes = null)
        {
            DropDownList ddl = new DropDownList();
            string moduleName = Convert.ToString(moduleRow.ModuleName);
            DataRow[] selectedRTS = drRequestType;// uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            DataTable requestTypeTable = null;
            //if module requestTypes are not exist then return empty dropdownlist
            if (selectedRTS == null || selectedRTS.Length <= 0)
                return ddl;

            requestTypeTable = selectedRTS.CopyToDataTable();

            DataRow[] requestTypes = new DataRow[0];

            string selectQuery = string.Empty;
            string includeServiceTypeQuery = string.Empty;
            if (!includeServiceTypes)
                includeServiceTypeQuery = string.Format(" and ({0} = 'False' or {0} is null)", DatabaseObjects.Columns.ServiceWizardOnly);

            selectQuery = string.Format("({0} = 'False' or {0} is null){1}",
                                                           DatabaseObjects.Columns.Deleted, includeServiceTypeQuery);

            requestTypes = requestTypeTable.Select(selectQuery);

            //Show only selected Request types
            if (selectedRequesttypes != null && selectedRequesttypes.Count > 0)
            {
                requestTypes = requestTypes.Where(x => selectedRequesttypes.Contains(x.Field<int>(DatabaseObjects.Columns.Id).ToString())).ToArray();
            }

            var groupData = requestTypes.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category)).OrderBy(x => x.Key);

            ListItem item;
            string style = string.Empty;
            foreach (var category in groupData)
            {
                //Provide "--select--" option in case of single categeory otherwise make dropdown visibility false
                var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory)).OrderBy(x => x.Key);
                if (groupData.Count() == 1 && (true/*!uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory)*/
                    || (subCategories.Count() == 1 && string.IsNullOrWhiteSpace(subCategories.First().Key))))
                {
                    ddl.Visible = false;
                    break;
                }

                if (groupData.Count() > 1)
                {
                    item = new ListItem(category.Key, category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }
                else if (groupData.Count() == 1 && true /*uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory)*/)
                {
                    item = new ListItem("--Select--", category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }

                foreach (var subCategory in subCategories)
                {
                    if (!string.IsNullOrEmpty(subCategory.Key))
                    {
                        item = new ListItem(string.Format("{0}{1}", HttpUtility.HtmlDecode(HttpUtility.HtmlDecode("&nbsp;&nbsp;")), subCategory.Key), string.Format("{0};#;{1}", category.Key, subCategory.Key));
                        item.Attributes.Add("style", string.Format("float:left;padding-left:10px;"));
                        ddl.Items.Add(item);
                    }
                }
            }

            if (groupData.Count() > 1)
                ddl.Items.Insert(0, new ListItem("All Categories", string.Empty));

            return ddl;
        }
        public static DropDownList GetCategoryWithSubCategoriesDropDown(ApplicationContext context, UGITModule moduleRow)
        {
            return GetCategoryWithSubCategoriesDropDown(context, moduleRow, false);
        }
        public static DropDownList GetCategoryWithSubCategoriesDropDown(ApplicationContext context, UGITModule moduleRow, bool includeServiceTypes, List<string> selectedRequesttypes = null)
        {
            DropDownList ddl = new DropDownList();
            string moduleName = moduleRow.ModuleName;

            RequestTypeManager requestTypeManager = new RequestTypeManager(context);

            List<ModuleRequestType> selectedRTS = requestTypeManager.Load(x => x.ModuleNameLookup == moduleName);
            //DataRow[] selectedRTS = uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            //if module requestTypes are not exist then return empty dropdownlist
            if (selectedRTS == null || selectedRTS.Count <= 0)
                return ddl;

            //requestTypeTable = selectedRTS selectedRTS.CopyToDataTable();

            List<ModuleRequestType> requestTypes = new List<ModuleRequestType>();

            string selectQuery = string.Empty;
            string includeServiceTypeQuery = string.Empty;

            if (!includeServiceTypes)
            {
                requestTypes = selectedRTS.Where(x => !UGITUtility.StringToBoolean(x.Deleted) && !x.ServiceWizardOnly.HasValue).ToList();
            }
            else
                requestTypes = selectedRTS.Where(x => !UGITUtility.StringToBoolean(x.Deleted)).ToList();
            //includeServiceTypeQuery = string.Format(" && ({0} == '0' || {0} == null || {0} == '')", DatabaseObjects.Columns.ServiceWizardOnly);
            //includeServiceTypeQuery = string.Format(" and ({0} = '0' or {0} is null or {0} = '')", DatabaseObjects.Columns.ServiceWizardOnly);

            //selectQuery = string.Format("({0} = '0' or {0} is null or {0} = ''){1}",
            //                                               DatabaseObjects.Columns.IsDeleted, includeServiceTypeQuery);
            //selectQuery = string.Format("({0} == '0' || {0} == null || {0} == ''){1}",
            //DatabaseObjects.Columns.IsDeleted, includeServiceTypeQuery);

            //requestTypes = selectedRTS.Where().Select(selectQuery);

            //Show only selected Request types
            if (selectedRequesttypes != null && selectedRequesttypes.Count > 0)
            {
                requestTypes = requestTypes.Where(x => selectedRequesttypes.Contains(UGITUtility.ObjectToString(x.ID))).ToList();
            }

            var groupData = requestTypes.AsEnumerable().GroupBy(x => x.Category).OrderBy(x => x.Key);

            ListItem item;
            string style = string.Empty;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            foreach (var category in groupData)
            {
                //Provide "--select--" option in case of single categeory otherwise make dropdown visibility false
                var subCategories = category.GroupBy(x => x.SubCategory).OrderBy(x => x.Key);
                if (groupData.Count() == 1 && (!configurationVariableManager.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory)
                    || (subCategories.Count() == 1 && string.IsNullOrWhiteSpace(subCategories.First().Key))))
                {
                    ddl.Visible = false;
                    break;
                }

                if (groupData.Count() > 1)
                {
                    item = new ListItem(category.Key, category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }
                else if (groupData.Count() == 1 && configurationVariableManager.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                {
                    item = new ListItem("--Select--", category.Key);
                    item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    ddl.Items.Add(item);
                }

                foreach (var subCategory in subCategories)
                {
                    if (!string.IsNullOrEmpty(subCategory.Key))
                    {
                        item = new ListItem(string.Format("{0}{1}", HttpUtility.HtmlDecode("&nbsp;&nbsp;"), subCategory.Key), string.Format("{0};#;{1}", category.Key, subCategory.Key));
                        item.Attributes.Add("style", string.Format("float:left;padding-left:10px;"));
                        ddl.Items.Add(item);
                    }
                }
            }

            if (groupData.Count() > 1)
                ddl.Items.Insert(0, new ListItem("--All Categories", string.Empty));

            return ddl;
        }
        public static string ConvertTextAreaStringToHtml(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            data = uHelper.FindAndConvertToAnchorTag(data);
            data = data.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
            return data;
        }

        public static string FindAndConvertToAnchorTag(string convertMeToAnchor)
        {
            if (String.IsNullOrWhiteSpace(convertMeToAnchor))
                return string.Empty;

            string toachortag = string.Empty;
            string pattern = @"\b(https?|ftp|file)://[-A-Z0-9+&@/%?=~_|!:,.;]*[A-Z0-9+&@/%=~_|]";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Get a collection of matches.
            MatchCollection matches = Regex.Matches(convertMeToAnchor, pattern, RegexOptions.IgnoreCase);
            if (matches != null)
            {
                List<string> lstMatches = matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
                // Use foreach-loop.
                foreach (string match in lstMatches)
                {
                    toachortag = string.Format("<a href='{0}'>{0}</a>", match);
                    convertMeToAnchor = convertMeToAnchor.Replace(match, toachortag);
                }
            }
            return convertMeToAnchor;
        }

        private static DropDownList GetRequestTypeDropDown(ApplicationContext context, DataRow moduleRow, DataRow[] requestTypes, bool hidePrefix)
        {

            DropDownList ddl = new DropDownList();


            DataRow[] requestTypesNoCSRows = requestTypes.Where(x => x.IsNull(DatabaseObjects.Columns.SortToBottom) || Convert.ToString(x.Field<bool>(DatabaseObjects.Columns.SortToBottom)) == "False").ToArray();
            DataRow[] sortToBottomRows = requestTypes.Where(x => !x.IsNull(DatabaseObjects.Columns.SortToBottom) && Convert.ToString(x.Field<bool>(DatabaseObjects.Columns.SortToBottom)) == "True").ToArray();
            DataTable sortToBottomTable = null;
            if (sortToBottomRows.Length > 0)
            {
                sortToBottomTable = sortToBottomRows.CopyToDataTable();
            }

            var groupData = requestTypesNoCSRows.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category)).OrderBy(x => x.Key).ToList();
            ListItem item;
            string style = string.Empty;
            foreach (var category in groupData)
            {
                if (groupData.Count() > 1)
                {
                    item = new ListItem(category.Key, "0");
                    //item.Attributes.Add("style", string.Format("float:left;color:black;font-weight:bold;"));
                    item.Attributes.Add("disabled", "disabled");
                    ddl.Items.Add(item);
                }

                var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory)).OrderBy(x => x.Key);
                bool subCategoryFound = false;
                ConfigurationVariableManager ConfigurationVariableHelper = new ConfigurationVariableManager(context);
                foreach (var subCategory in subCategories)
                {
                    subCategoryFound = false;
                    if (!string.IsNullOrEmpty(subCategory.Key) && ConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                    {
                        subCategoryFound = true;
                        item = new ListItem(string.Format("{0}", subCategory.Key), "0");
                        item.Attributes.Add("disabled", "disabled");
                        //item.Attributes.Add("style", string.Format("float:left;padding-left:10px;font-weight:bold;"));
                        ddl.Items.Add(item);
                    }

                    string title = string.Empty;
                    DataRow[] rq = subCategory.ToArray().OrderBy(x => x.Field<string>(DatabaseObjects.Columns.TicketRequestType)).ToArray();
                    string intents = "";
                    foreach (var requestty in rq)
                    {
                        title = Convert.ToString(requestty[DatabaseObjects.Columns.TicketRequestType]);
                        if (!hidePrefix)
                        {
                            switch (Convert.ToString(requestty[DatabaseObjects.Columns.WorkflowType]))
                            {
                                case "Quick":
                                    title = ">> " + title;
                                    break;
                                case "Requisition":
                                    title = "$$ " + title;
                                    break;
                                default:
                                    title = "— " + title;
                                    break;
                            }
                        }

                        if (subCategoryFound)
                            intents = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode("&nbsp;&nbsp;"));

                        item = new ListItem(string.Format("{1}{0}", title, intents), string.Format("{0}", Convert.ToString(requestty[DatabaseObjects.Columns.Id])));
                        item.Attributes.Add("style", string.Format("float:left;padding-left:12px;"));
                        item.Attributes.Add("description", Convert.ToString(requestty[DatabaseObjects.Columns.RequestTypeDescription]));
                        //if (uHelper.StringToBoolean(requestty[DatabaseObjects.Columns.Attachments]))
                        //{
                        //    item.Attributes.Add("isattachment", "1");
                        //}
                        ddl.Items.Add(item);
                    }

                    if (sortToBottomTable != null && sortToBottomTable.Rows.Count > 0)
                    {
                        string exp = string.Empty;
                        if (category.Key == null)
                            exp = string.Format("{0} is null", DatabaseObjects.Columns.Category);
                        else
                            exp = string.Format("{0} ='{1}'", DatabaseObjects.Columns.Category, category.Key);

                        if (subCategory.Key == null)
                            exp += string.Format("and {0} is null", DatabaseObjects.Columns.SubCategory);
                        else
                            exp += string.Format("and {0} ='{1}'", DatabaseObjects.Columns.SubCategory, subCategory.Key);

                        DataRow[] stbRows = sortToBottomTable.Select(exp, string.Format("{0}", DatabaseObjects.Columns.TicketRequestType));
                        foreach (DataRow r in stbRows)
                        {
                            title = Convert.ToString(r[DatabaseObjects.Columns.TicketRequestType]);
                            if (!hidePrefix)
                            {
                                switch (Convert.ToString(r[DatabaseObjects.Columns.WorkflowType]))
                                {
                                    case "Quick":
                                        title = ">> " + title;
                                        break;
                                    case "Requisition":
                                        title = "$$ " + title;
                                        break;
                                    default:
                                        title = "— " + title;
                                        break;
                                }
                            }

                            item = new ListItem(string.Format("{1}{0}", title, intents), string.Format("{0}", Convert.ToString(r[DatabaseObjects.Columns.Id])));
                            item.Attributes.Add("style", string.Format("float:left;padding-left:12px;"));
                            item.Attributes.Add("description", Convert.ToString(r[DatabaseObjects.Columns.RequestTypeDescription]));
                            //if (uHelper.StringToBoolean(r[DatabaseObjects.Columns.Attachments]))
                            //{
                            //    item.Attributes.Add("isattachment", "1");
                            //}
                            ddl.Items.Add(item);
                        }
                    }
                }
            }

            string ticketOwnerBinding = Convert.ToString(moduleRow[DatabaseObjects.Columns.TicketOwnerBinding]);
            if (ticketOwnerBinding == TicketOwnerBinding.Disabled.ToString()) return ddl;
            ddl.Attributes.Add("onchange", string.Format("changeRequestOwner(this, true, \"{0}\")", ticketOwnerBinding));

            int sIndex = -1;
            foreach (ListItem lItem in ddl.Items)
            {
                if (lItem.Value != "0")
                {
                    sIndex = ddl.Items.IndexOf(lItem);
                    break;
                }
            }

            if (sIndex >= 0)
            {
                ddl.SelectedIndex = sIndex;
            }

            return ddl;
        }

        public static bool IsUserAuthorizedToViewModule(string moduleName)
        {
            //DataRow moduleRow = uGITCache.GetModuleDetails(moduleName);
            return true;
        }

        public static string getModuleNameByTicketId(string ticketID)
        {
            string moduleName = string.Empty;
            if (!string.IsNullOrEmpty(ticketID) && ticketID.Contains('-'))
            {
                moduleName = ticketID.Split('-')[0];
            }
            return moduleName;
        }


        public static void ClosePopUpAndEndResponse(HttpContext context)
        {
            ClosePopUpAndEndResponse(context, true);
        }

        public static void ClosePopUpAndEndResponse(HttpContext context, bool refreshPage, bool refreshDataOnly = false)
        {
            string sourceUrl = string.Empty;
            if (context.Request["source"] != null && context.Request["source"].Trim() != string.Empty)
            {
                sourceUrl = context.Request["source"].Trim();
            }

            ClosePopUpAndEndResponse(context, refreshPage, sourceUrl, refreshDataOnly);
        }

        public static void ClosePopUpAndEndResponse(HttpContext context, bool refreshPage, string sourceUrl, bool refreshDataOnly = false)
        {
            if (refreshPage)
            {
                if (refreshDataOnly)
                    sourceUrl += "**refreshDataOnly";

                context.Response.Write("<script type='text/javascript'> window.parent.CloseWindowCallback(1, \"" + sourceUrl + "\")</script>");

            }
            else
            {
                sourceUrl += "**stoprefreshpage";
                context.Response.Write("<script type='text/javascript'> window.parent.CloseWindowCallback(1,  \"" + sourceUrl + "\")</script>");
            }
            context.ApplicationInstance.CompleteRequest();
        }
        public static void PerformAjaxPanelCallBack(Page Page, HttpContext context)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "refreshAjaxPanelCallBack", "window.parent.PerformAjaxPanelCallBack(true);", true);
        }

        public static void ClosePopUpAndRedirect(HttpContext context, string sourceUrl)
        {

            context.Response.Write("<script type='text/javascript'> window.top.location.href = \"" + sourceUrl + "\" </script>");
            context.ApplicationInstance.CompleteRequest();
        }


        public static bool IsUserAuthorizedToViewModule(ApplicationContext context, UserProfile user, string moduleName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule ugitModule = moduleViewManager.LoadByName(moduleName);
            DataTable aResultDT = UGITUtility.ObjectToData(ugitModule);
            return IsUserAuthorizedToViewModule(context, user, aResultDT.Rows[0]);
        }

        public static bool IsUserAuthorizedToViewModule(ApplicationContext context, UserProfile user, DataRow moduleRow)
        {

            if (moduleRow == null)
                return false; // Should never happen!

            // If Super-Admin, then always authorized!
            //if (context.UserManager.IsRole(RoleType.SAdmin, user.UserName))
            if (context.UserManager.IsRole(RoleType.UGITSuperAdmin, user.UserName))
                return true;

            // If module is disabled, then no one is authorized!
            if (!UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.EnableModule]))
                return false;

            string authorizedToView = Convert.ToString(moduleRow[DatabaseObjects.Columns.AuthorizedToView]).Trim();
            if (string.IsNullOrEmpty(authorizedToView))
                return true;    // If column is empty, that means EVERYONE is authorized!

            try
            {
                string[] users = UGITUtility.SplitString(authorizedToView, Constants.Separator);
                if (users.Contains(user.Name))
                {
                    return true;
                }
                else
                {
                    // In case column value had group names
                    foreach (string groupName in users)
                    {
                        if (context.UserManager.CheckUserIsInGroup(groupName, user))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return false;
        }

        public static int getModuleIdByTicketID(ApplicationContext context, string ticketID)
        {
            int moduleId = 0;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule uGITModule = moduleViewManager.LoadByName(uHelper.getModuleNameByTicketId(ticketID));
            if (uGITModule != null)
                moduleId = Convert.ToInt32(uGITModule.ID);

            return moduleId;
        }

        public static int getModuleIdByModuleName(ApplicationContext context, string moduleName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            int moduleID = 0;
            /*
                DataRow moduleRow = null;
                UGITModule ugitModule = moduleViewManager.LoadByName(moduleName); // ModuleDAL.getModuleConfigData(moduleName);
                DataTable aResultDT = UGITUtility.ObjectToData(ugitModule);
                moduleRow = aResultDT.Rows[0];
                if (moduleRow != null)
                    int.TryParse(Convert.ToString(moduleRow[DatabaseObjects.Columns.Id]), out moduleID);
            */

            moduleID = Convert.ToInt32(moduleViewManager.LoadByName(moduleName, true).ID);

            return moduleID;
        }

        public static string getModuleNameByModuleId(ApplicationContext context, int moduleID)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            string moduleName = string.Empty;
            UGITModule uGITModule = moduleViewManager.LoadByID(moduleID);
            if (uGITModule != null)
                moduleName = uGITModule.ModuleName;
            return moduleName;
        }

        public static DataRow getModuleItemByTicketID(string ticketID)
        {
            string moduleName = getModuleNameByTicketId(ticketID);
            string where = string.Format("{0}='{1}' and Tenantid='{2}'", DatabaseObjects.Columns.ModuleName, moduleName, HttpContext.Current.GetManagerContext().TenantID);
            if (!string.IsNullOrEmpty(moduleName))
            {
                DataRow[] moduleCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, where).Select();
                //SPList modules = oWeb.Lists["Modules"];
                //SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleName, moduleName);
                //SPListItemCollection moduleCollection = modules.GetItems(query);
                if (moduleCollection != null && moduleCollection.Count() > 0)
                    return moduleCollection[0];
            }

            return null;
        }

        public static DataRow getModuleItemByTicketID(string ticketID, string TenantId)
        {
            string moduleName = getModuleNameByTicketId(ticketID);
            if (!string.IsNullOrEmpty(moduleName))
            {
                //DataRow[] moduleCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, DatabaseObjects.Columns.ModuleName + "='" + moduleName + "'").Select();
                DataRow[] moduleCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ModuleName}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{TenantId}'").Select();

                //SPList modules = oWeb.Lists["Modules"];
                //SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleName, moduleName);
                //SPListItemCollection moduleCollection = modules.GetItems(query);
                if (moduleCollection != null && moduleCollection.Count() > 0)
                    return moduleCollection[0];
            }

            return null;
        }

        public static bool IsUserAuthorizedToViewModule(UserProfile user, UGITModule module)
        {
            if (user == null || module == null)
                return false; // Should never happen!!

            if (string.IsNullOrEmpty(module.AuthorizedToView))
                return true;

            if (module.AuthorizedToView.Split(',').Any(x => x == user.Id))
            {
                return true;
            }
            else
            {
                //IEnumerable<int> groupIDs = module.AuthorizedToView.Where(x => x.IsGroup).Select(x => x.ID);
                //foreach (int groupID in groupIDs)
                //{
                //    if (UserProfile.CheckUserIsInGroup(groupID, user))
                //        return true;
                //}
            }
            return false;
        }

        public static string GetCommentString(UserProfile user, string versionDescription, DataRow versionFor, string internalName, bool isPrivateComment)
        {
            string oldVersionString = string.Empty;
            // Check for non-versioned value
            string prevValue = Convert.ToString(versionFor[internalName]);
            // In case the non-versioned value is empty we need to Check for versioned value

            if (prevValue == string.Empty/* && versionFor.ID != 0 && versionFor.Versions.Count != 0*/)
            {
                //prevValue = Convert.ToString(versionFor.Versions[0][internalName]);
            }
            if (prevValue != string.Empty)
                oldVersionString = prevValue + Constants.SeparatorForVersions;
            return oldVersionString + user.Id + Constants.Separator
                        + Constants.UTCPrefix + DateTime.UtcNow.ToString() + Constants.Separator
                        + versionDescription + Constants.Separator
                        + Convert.ToString(isPrivateComment);
        }

        public static void InformationPopup(HttpContext context, string html, string title)
        {
            InformationPopup(context, html, title, false);
        }

        public static void PickerListPopup(HttpContext context, string url)
        {
            bool isPickerList = true;
            InformationPopup(context, url, null, false, isPickerList);
        }

        public static void InformationPopup(HttpContext context, string html, string title, bool stopRefresh, bool isPickerList = false)
        {
            StringBuilder sbScript = new StringBuilder();

            string sourceUrl = string.Empty;
            if (context.Request["source"] != null && context.Request["source"].Trim() != string.Empty && !isPickerList)
            {
                sourceUrl = context.Request["source"].Trim();// Pages / TSRTickets
                sbScript.AppendFormat("window.parent.UgitOpenHTMLPopupDialog('{0}','{1}','{2}', {3});", html.Replace("\"", "\\\"").Replace("'", "~"), title, sourceUrl, stopRefresh ? "true" : "false");
            }
            else if (isPickerList)
                sbScript.AppendFormat("window.parent.UgitOpenPopupDialog('{0}', '', '{2}', '95', '95', 0, '{1}')", html, context.Request["source"], "Picker List");
            else

                sbScript.AppendFormat("window.parent.setTimeout(\"window.parent.UgitOpenHTMLPopupDialog('{0}','{1}','{2}', {3})\", 500);", html.Replace("\"", "\\\"").Replace("'", "~"), title, sourceUrl, stopRefresh ? "true" : "false");


            //script.Append("window.frameElement.commitPopup(\"" + sourceUrl + "\");");
            context.Response.Write(string.Format("<script type='text/javascript'>{0}</script>", sbScript.ToString()));
            context.Response.Flush();
            ////crash here... so i have modify for change ticket type. 
            //context.Response.End();
            context.ApplicationInstance.CompleteRequest();
        }

        public static XmlDocument SerializeObject(Object objToSerialize)
        {
            XmlSerializer ser = new XmlSerializer(objToSerialize.GetType());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            ser.Serialize(writer, objToSerialize);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            return doc;
        }

        public static object DeSerializeAnObject(XmlDocument doc, object obj1)
        {
            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
            Object objType = new object();
            XmlSerializer ser = new XmlSerializer(obj1.GetType());
            object obj = ser.Deserialize(reader);
            return obj;
        }

        public static object DeSerializeAnObject(XmlDocument doc, Type type)
        {
            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
            Object objType = new object();
            XmlSerializer ser = new XmlSerializer(type);
            object obj = ser.Deserialize(reader);
            return obj;
        }

        public static DevExpress.Web.MenuItem AddAgentItem(ApplicationContext context, string moduleName, string currentStageName)
        {
            DevExpress.Web.MenuItem agentItem = new DevExpress.Web.MenuItem("Agent", "Agent", "/Content/ButtonImages/self-assign.png");
            agentItem.ClientVisible = false;
            ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(context);
            ServicesManager objServicesManager = new ServicesManager(context);
            ServiceCategory serviceCategory = objServiceCategoryManager.LoadCategoryByType(Constants.ModuleAgent).FirstOrDefault();
            string TicketManualEscalationUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ManualEscalation");
            if (serviceCategory != null && !string.IsNullOrEmpty(serviceCategory.CategoryName))
            {
                // change
                List<Services> servicesModuleAgent = objServicesManager.Load(x => x.ServiceType == "ModuleAgent").ToList();
                foreach (Services item in servicesModuleAgent)
                {
                    servicesModuleAgent = servicesModuleAgent.Where(x => x.IsActivated == true && x.ServiceType == "ModuleAgent" && x.ModuleNameLookup == moduleName).ToList();
                }
                // SPQuery query = new SPQuery();
                // query.Query = string.Format("<Where><And><Eq><FieldRef Name='{4}'/><Value Type='Boolean'>{5}</Value></Eq><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Text'>{3}</Value></Eq></And></And></Where>", DatabaseObjects.Columns.ServiceCategoryNameLookup, serviceCategory.CategoryName, DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.IsActivated, 1);
                // query.ViewFieldsOnly = true;
                // query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStageMultiLookup);
                // DataTable dt = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.Services, query).GetDataTable();
                if (servicesModuleAgent != null && servicesModuleAgent.Count > 0)
                {
                    foreach (Services dr in servicesModuleAgent)
                    {
                        List<string> stageName = null;
                        List<string> modulestages = UGITUtility.ConvertStringToList(dr.ModuleStage, Constants.Separator);
                        LifeCycleManager objLifeCycleHelper = new LifeCycleManager(context);
                        LifeCycle obj = objLifeCycleHelper.LoadLifeCycleByModule(moduleName)[0];
                        if (obj != null)
                        {
                            stageName = new List<string>();
                            modulestages.ForEach(x => stageName.Add(obj.Stages.FirstOrDefault(y => y.StageStep == Convert.ToInt32(x)).StageTitle));

                        }
                        //uHelper.GetMultiLookupValue(Convert.ToString(dr[DatabaseObjects.Columns.ModuleStageMultiLookup]));
                        if (modulestages.Count > 0)
                        {
                            string agentStages = string.Join(Constants.Separator, stageName);
                            string agentId = Convert.ToString(dr.ID);
                            DevExpress.Web.MenuItem agentSubItem = new DevExpress.Web.MenuItem(Convert.ToString(dr.Title), "AgentSubItem" + "#" + agentId + "#" + Convert.ToString(dr.Title) + "#" + agentStages);
                            //current stage is empty when opening from customfilter
                            if (!string.IsNullOrEmpty(currentStageName))
                            {
                                if (agentStages.IndexOf(currentStageName) != -1)
                                {
                                    agentSubItem.ClientVisible = true;
                                    agentItem.ClientVisible = true;
                                    agentSubItem.Items.Add("Run Now", "AgentSubItem" + "#" + agentId + "#" + Convert.ToString(dr.Title) + "#" + agentStages);
                                    agentSubItem.Items.Add("Send Link to Action User", "SendAgentLink#" + agentId + "#" + Convert.ToString(dr.Title));
                                    agentItem.Items.Add(agentSubItem);
                                }
                            }
                            else
                            {
                                agentSubItem.ClientVisible = false;
                                agentItem.ClientVisible = false;
                                agentSubItem.Items.Add("Run Now", "AgentSubItem" + "#" + agentId + "#" + Convert.ToString(dr.Title) + "#" + agentStages);
                                agentSubItem.Items.Add("Send Link to Action User", "SendAgentLink#" + agentId + "#" + Convert.ToString(dr.Title));
                                agentItem.Items.Add(agentSubItem);

                            }
                        }
                    }
                    return agentItem;
                }
            }
            return null;
        }

        public static bool IsProjectApproved(ApplicationContext context, DataRow ticketItem)
        {
            Ticket ticket = new Ticket(context, "NPR");
            LifeCycle lifeCycle = ticket.GetTicketLifeCycle(ticketItem);
            if (lifeCycle == null)
                return true;

            LifeCycleStage currentStage = ticket.GetTicketCurrentStage(ticketItem);
            if (currentStage == null)
                return true;

            LifeCycleStage resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());
            if (resolvedStage == null)
                return false;

            if (currentStage.StageStep >= resolvedStage.StageStep)
                return true;

            return false;
        }

        public static string GetTicketDetailsForEmailFooter(ApplicationContext context, DataRow item, string moduleName, bool HTML)
        {
            return GetTicketDetailsForEmailFooter(context, item, moduleName, HTML, false);
        }

        public static string GetTicketDetailsForEmailFooter(ApplicationContext context, DataRow item, string moduleName, bool HTML, bool disableEmailTicketLink, string status = null, bool isskipped = false)
        {
            if (item == null || string.IsNullOrEmpty(moduleName))
                return string.Empty;

            DataTable ticketEmailFooterList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.EmailFooter, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            ticketEmailFooterList = ticketEmailFooterList.DefaultView.ToTable(false, DatabaseObjects.Columns.FieldName,
                                                               DatabaseObjects.Columns.FieldDisplayName,
                                                               DatabaseObjects.Columns.ItemOrder,
                                                               DatabaseObjects.Columns.ModuleNameLookup);
            DataRow[] ticketEmailFooterCollItems = null;
            if (ticketEmailFooterList.Rows.Count > 0)
            {
                ticketEmailFooterCollItems = ticketEmailFooterList.Select(string.Format("Where {0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            }
            else
            {
                ticketEmailFooterCollItems = ticketEmailFooterList.Select();
            }
            List<Tuple<string, string>> emailFields = new List<Tuple<string, string>>();
            if (ticketEmailFooterCollItems != null && ticketEmailFooterCollItems.Count() > 0)
            {
                foreach (DataRow ticketEmailFooterItem in ticketEmailFooterCollItems)
                {
                    emailFields.Add(new Tuple<string, string>(Convert.ToString(ticketEmailFooterItem[DatabaseObjects.Columns.FieldName]),
                        Convert.ToString(ticketEmailFooterItem[DatabaseObjects.Columns.FieldDisplayName])));
                }
            }
            else
            {
                // If no footer fields configured, use default list
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketRequestor, "Requestor"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketInitiator, "Initiator"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketDesiredCompletionDate, "Desired Completion Date"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketPriorityLookup, "Priority"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketStatus, "Status"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketRequestTypeLookup, "Request Type"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketOwner, "Owner"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.OwnerUser, "Owner"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketDescription, "Detailed Description"));
                emailFields.Add(new Tuple<string, string>(DatabaseObjects.Columns.TicketResolutionComments, "Resolution"));
            }

            StringBuilder formattedTicketDetails = new StringBuilder();
            string ticketId = string.Empty;
            string ticketTitle = string.Empty;
            if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId))
            {
                ticketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                ticketTitle = Convert.ToString(item[DatabaseObjects.Columns.Title]);
            }

            string htmlBoldStart = HTML ? "<b>" : string.Empty;
            string htmlBoldEnd = HTML ? "</b>" : string.Empty;
            string htmlBreak = HTML ? "<br/>" : "\r\n";
            string fieldFormat = "{0}:{1} \n\r";
            string requesterFieldFormat = "{0}:{1}{2} \n\r";
            string userSummaryFormat = string.Empty;
            if (HTML)
            {
                formattedTicketDetails.AppendFormat("<Table>");
                fieldFormat = "<tr><td style='background:none repeat scroll 0 0 #E8F5F8; font-weight:bold; text-align:right; width:190px; vertical-align:top;'>{0}</td><td style='background:none repeat scroll 0 0 #FBFBFB; padding:3px 6px 4px; vertical-align:top;'>{1}</td></tr>";
                requesterFieldFormat = "<tr><td style='background:none repeat scroll 0 0 #E8F5F8; font-weight:bold; text-align:right; width:190px; vertical-align:top;'>{0}</td><td style='background:none repeat scroll 0 0 #FBFBFB; padding:3px 6px 4px; vertical-align:top;'>{1}{2}</td></tr>";
                userSummaryFormat = "<tr><td colspan='2' style='background:none repeat scroll 0 0 #E8F5F8;'>{0}</td></tr>";
            }
            string titleWithLink = string.Empty;
            if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId))
            {
                string title = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                if (ticketId != null)
                {
                    if (HTML)
                    {
                        if (disableEmailTicketLink)
                            titleWithLink = string.Format("{0}: {1}", ticketId, title);
                        else
                        {
                            string url = string.Format("{3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", Constants.HomePagePath,
                                    Convert.ToString(item[DatabaseObjects.Columns.TicketId]), moduleName, ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), context.TenantID);
                            titleWithLink = string.Format("<a href='{0}'>{1}: {2}</a>", url, ticketId, title);
                        }

                    }
                    else
                    {
                        title = ReplaceInvalidCharsInURL(title); // Replace characters that break mailto: URL
                        string url = string.Format("Ticket Link: {3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", Constants.HomePagePath,
                            Convert.ToString(item[DatabaseObjects.Columns.TicketId]), moduleName, ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), context.TenantID);
                        titleWithLink = string.Format("{0}: {1}{2}{3}", ticketId, title, htmlBreak, url);
                    }
                }
            }
            // Create header for footer details
            formattedTicketDetails.Append(htmlBreak);
            if (HTML)
                formattedTicketDetails.Append("<hr>");
            else
                formattedTicketDetails.AppendFormat("_________________________________________________________{0}", htmlBreak);
            formattedTicketDetails.AppendFormat("{0}{1}{2}{3}{4}", htmlBoldStart, titleWithLink, htmlBoldEnd, htmlBreak, htmlBreak);
            if (HTML)
                formattedTicketDetails.AppendFormat("<Table style='border: 1px solid #A5A5A5;'>");
            // Add footer fields          
            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            FieldConfiguration configField = null;
            foreach (Tuple<string, string> eField in emailFields)
            {
                if (UGITUtility.IsSPItemExist(item, eField.Item1))
                {
                    DataColumn spField = item.Table.Columns[eField.Item1];
                    //  SPField spField = UGITUtility. item.GetFieldByInternalName(eField.Item1);

                    configField = configFieldManager.GetFieldByFieldName(spField.ColumnName);

                    string fieldColumnType = string.Empty;
                    if (configField != null)
                    {
                        fieldColumnType = Convert.ToString(configField.Datatype);
                    }
                    else
                        fieldColumnType = Convert.ToString(spField.DataType);
                    bool isQuestionSummary = false;
                    string value = string.Empty;
                    if (fieldColumnType == "UserField")
                    {
                        if (spField.ColumnName == DatabaseObjects.Columns.TicketRequestor)
                        {
                            string userLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, eField.Item1));
                            if (!string.IsNullOrWhiteSpace(userLookup))
                            {
                                UserProfile userProfile = context.UserManager.GetUserById(userLookup);
                                if (userProfile != null)
                                {
                                    List<string> requesterDetail = new List<string>();
                                    StringBuilder requesterDetails = new StringBuilder();
                                    if (!string.IsNullOrEmpty(userProfile.Email))
                                        requesterDetail.Add(userProfile.Email);
                                    if (!string.IsNullOrEmpty(userProfile.MobilePhone))
                                        requesterDetail.Add(userProfile.MobilePhone);
                                    if (!string.IsNullOrEmpty(userProfile.Department))
                                        requesterDetail.Add(userProfile.Department);
                                    if (!string.IsNullOrEmpty(userProfile.Location))
                                        requesterDetail.Add(userProfile.Location);

                                    if (requesterDetail.Count > 0)
                                        formattedTicketDetails.AppendFormat(requesterFieldFormat, "Requestor", userProfile.Name, " [" + string.Join(", ", requesterDetail) + "]");
                                    else
                                        formattedTicketDetails.AppendFormat(requesterFieldFormat, "Requestor", userProfile.Name, string.Empty);
                                }
                            }
                        }
                        else if (spField.ColumnName == DatabaseObjects.Columns.TicketInitiator)
                        {
                            string userLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, eField.Item1));
                            if (!string.IsNullOrWhiteSpace(userLookup))
                            {
                                UserProfile userProfile = context.UserManager.GetUserById(userLookup);
                                if (userProfile != null)
                                {
                                    List<string> requesterDetail = new List<string>();
                                    StringBuilder requesterDetails = new StringBuilder();
                                    if (!string.IsNullOrEmpty(userProfile.Email))
                                        requesterDetail.Add(userProfile.Email);
                                    if (!string.IsNullOrEmpty(userProfile.MobilePhone))
                                        requesterDetail.Add(userProfile.MobilePhone);
                                    if (!string.IsNullOrEmpty(userProfile.Department))
                                        requesterDetail.Add(userProfile.Department);
                                    if (!string.IsNullOrEmpty(userProfile.Location))
                                        requesterDetail.Add(userProfile.Location);

                                    if (requesterDetail.Count > 0)
                                        formattedTicketDetails.AppendFormat(requesterFieldFormat, "Initiator", userProfile.Name, " [" + string.Join(", ", requesterDetail) + "]");
                                    else
                                        formattedTicketDetails.AppendFormat(requesterFieldFormat, "Initiator", userProfile.Name, string.Empty);
                                }
                            }
                        }
                        else
                        {
                            /*
                                string[] field = { "spField.ColumnName" };
                                UserProfile user = context.UserManager.GetUserInfo(item, field, true);
                                if (user != null)
                                    value = user.UserName;
                            */
                            var userIds = Convert.ToString(item[spField.ColumnName]);
                            if (userIds != null)
                            {
                                var userIdList = UGITUtility.ConvertStringToList(userIds, Constants.Separator6);
                                var users = context.UserManager.GetUserOrGroupName(userIdList);

                                if (users != null)
                                    value = users;
                            }
                        }
                    }
                    else if (spField.ColumnName == DatabaseObjects.Columns.UserQuestionSummary)
                    {
                        string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.UserQuestionSummary));
                        if (emailFields.Exists(x => x.Item1 == DatabaseObjects.Columns.UserQuestionSummary) && !string.IsNullOrEmpty(questionInputs))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(questionInputs.Trim());
                            //ServiceInput inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                            //value = GenerateSummary(inputObj, spWeb);
                            isQuestionSummary = true;
                        }
                    }
                    else if (spField.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
                    {
                        List<string> resolutionDetail = new List<string>();
                        List<HistoryEntry> historyList = uHelper.GetHistory(item, DatabaseObjects.Columns.TicketResolutionComments);
                        if (historyList.Count > 0)
                        {
                            foreach (HistoryEntry historyEntry in historyList)
                            {
                                if (!string.IsNullOrEmpty(historyEntry.created))
                                    resolutionDetail.Add(string.Format("({0})", historyEntry.created));

                                if (!string.IsNullOrEmpty(historyEntry.createdBy))
                                    resolutionDetail.Add(string.Format(" {0}", uHelper.GetUserNameBasedOnId(context, historyEntry.createdBy)));

                                if (!string.IsNullOrEmpty(historyEntry.entry))
                                {
                                    if (!HTML)
                                        resolutionDetail.Add(string.Format(" : {0}", ReplaceInvalidCharsInURL(historyEntry.entry))); // Replace characters that break mailto: URL
                                    else
                                        resolutionDetail.Add(string.Format(" : {0}", uHelper.ConvertTextAreaStringToHtml(historyEntry.entry)));
                                }

                                if (resolutionDetail.Count > 0)
                                    value = string.Join("", resolutionDetail);
                            }
                        }
                    }
                    else if (fieldColumnType == "NoteField")
                    {
                        value = ConvertTextAreaStringToHtml(Convert.ToString(item[spField.ColumnName]));
                    }
                    else if (fieldColumnType == "Lookup")
                    {
                        value = Convert.ToString(UGITUtility.GetSPItemValue(item, eField.Item1));
                        string aLookupvalue = configFieldManager.GetFieldConfigurationData(eField.Item1, value);
                        value = aLookupvalue;
                    }
                    else if (fieldColumnType == "DateTime")
                    {
                        value = UGITUtility.GetDateStringInFormat(Convert.ToString(item[DatabaseObjects.Columns.TicketDesiredCompletionDate]), false);
                    }
                    else if (spField.ColumnName == DatabaseObjects.Columns.Status && !string.IsNullOrEmpty(status))
                    {
                        value = status;
                    }
                    else
                    {
                        value = Convert.ToString(UGITUtility.GetSPItemValue(item, eField.Item1));
                    }

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (isQuestionSummary)
                            formattedTicketDetails.AppendFormat(userSummaryFormat, value);
                        else
                            formattedTicketDetails.AppendFormat(fieldFormat, eField.Item2, value);
                    }

                }
            }

            if (HTML)
            {
                formattedTicketDetails.AppendFormat("</Table>");
            }

            return formattedTicketDetails.ToString();
        }

        public static string GetUserNameBasedOnId(ApplicationContext context, string userid)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(userid))
            {
                UserProfileManager userManager = new UserProfileManager(context);
                UserProfile user = userManager.GetUserById(userid);
                if (user != null)
                    result = user.Name;
            }
            return result;
        }

        public static string GetModuleTitle(string module)
        {
            if (module.Contains("(CPR)"))
                module = ModuleNames.CPR;
            else if (module.Contains("(CNS)"))
                module = ModuleNames.CNS;
            else if (module.Contains("(OPM)"))
                module = ModuleNames.OPM;

            ModuleViewManager objMgr = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            string title = string.Empty;
            UGITModule ugitModule = objMgr.LoadByName(module);
            if (ugitModule != null)
            {
                title = ugitModule.Title;
            }
            return title;
        }

        public static string GetModuleStageId(DataRow[] moduleStages, DataRow[] moduleStageTypes, StageType stageType)
        {
            DataRow stageTypeRow = moduleStageTypes.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleStageType) == stageType.ToString());
            if (stageTypeRow != null && Convert.ToString(stageTypeRow[DatabaseObjects.Columns.Title]) != string.Empty)
            {
                DataRow stage = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageType) == stageTypeRow[DatabaseObjects.Columns.Title].ToString());
                if (stage != null)
                {
                    return Convert.ToString(stage[DatabaseObjects.Columns.Id]);
                }
            }
            return string.Empty;
        }

        public static string GetModuleStageId(ApplicationContext context,DataRow moduleItem, StageType stageType)
        {
            string stageId = "0";

            if (moduleItem != null)
            {
                string queryStageType = string.Format("{1}='{0}'", stageType.ToString(), DatabaseObjects.Columns.ModuleStageType);
                DataRow[] stageTypeCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.StageType, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(queryStageType);

                if (stageTypeCollection != null && stageTypeCollection.Count() > 0)
                {
                    DataTable moduleStageList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    string queryModuleStages = string.Format("{1}='{0}' and {3}='{2}'", moduleItem[DatabaseObjects.Columns.ModuleName], DatabaseObjects.Columns.ModuleNameLookup, stageTypeCollection[0][DatabaseObjects.Columns.ModuleStageType], DatabaseObjects.Columns.StageType);
                    DataRow[] closedStages = moduleStageList.Select(queryModuleStages);
                    if (closedStages != null && closedStages.Count() > 0)
                    {
                        stageId = closedStages[0][DatabaseObjects.Columns.Id].ToString();
                    }
                }
            }

            return stageId;
        }

        /// <summary>
        /// Token replacement.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="textWithTokens"></param>
        /// <param name="objTicket">objTicket is not madatory may be null and its used for genrating suverylink ,Ticket URL and Action Button.</param>
        /// <param name="disableLinks">its diable all links in Email</param>
        /// <returns></returns>
        /// 
        public static DataRow[] GetServices(string servicecategory, string moduleName, List<string> lstviewfields = null)
        {
            DataTable olist = null;
            // SPQuery query = new SPQuery();
            string query = "";
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ServiceType, servicecategory.Replace(Constants.Separator2, string.Empty), DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            }
            else
            {
                query = string.Format("{0}='{1}',{2} is null", DatabaseObjects.Columns.ServiceType, servicecategory.Replace(Constants.Separator2, string.Empty), DatabaseObjects.Columns.ModuleNameLookup, moduleName);
                //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq><IsNull><FieldRef Name='{2}' /></IsNull></And></Where>",
                //DatabaseObjects.Columns.ServiceCategoryNameLookup, servicecategory, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            }

            string viewfields = string.Empty;
            if (lstviewfields != null && lstviewfields.Count > 0)
            {
                lstviewfields = lstviewfields.Distinct().ToList();

                foreach (string str in lstviewfields)
                {
                    if (UGITUtility.IfColumnExists(str, olist))
                    {
                        viewfields = string.Concat(viewfields,
                                                   string.Format(str));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(viewfields))
            {
                //query.ViewFields = viewfields;
                //query.ViewFieldsOnly = true;
            }

            return GetTableDataManager.GetTableData(DatabaseObjects.Tables.Services, query).Select();
        }

        public static string ReplaceTokensWithTicketColumns(ApplicationContext context, DataRow item, string textWithTokens, Ticket objTicket, bool disableLinks, string strResolutionTime = null)
        {
            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            FieldConfiguration configField = null;

            string textWithoutTokens = textWithTokens;

            string[] tokens = uHelper.GetMyTokens(textWithTokens).Distinct().ToArray();
            for (int i = 0; i < tokens.Length; i++)
            {
                bool disableEmailTicketLink = false;

                string ticketColumn = tokens[i];
                ticketColumn = ticketColumn.Replace(Constants.TokenStart, string.Empty).Replace(Constants.TokenEnd, string.Empty);
                string tokenName = ticketColumn;
                if (ticketColumn.ToLower() == "logo")
                    continue;
                //Replaces surveylink to with actual survey link if survey form in configure for current module
                if (ticketColumn.ToLower().Contains("surveylink") && objTicket != null && !disableLinks)
                {
                    // If token is in format [$SurveyLink|<hyperlink text>$], extract hyperlink text
                    string agentLinkText = string.Empty;
                    if (ticketColumn.Contains("|"))
                        agentLinkText = ticketColumn.Split('|')[1];
                    else
                        agentLinkText = "Please click here to provide your feedback";

                    //Load Survey detail for current module
                    List<string> lstviewfields = new List<string>();
                    lstviewfields.Add(DatabaseObjects.Columns.IsActivated);
                    lstviewfields.Add(DatabaseObjects.Columns.ModuleNameLookup);
                    DataRow[] survey = uHelper.GetServices(Constants.ModuleFeedback, objTicket.Module.ModuleName, lstviewfields);

                    if (survey != null && survey.Count() > 0 && UGITUtility.StringToBoolean(survey[0][DatabaseObjects.Columns.IsActivated]))
                    {
                        string ticketid = string.Empty;
                        if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId))
                        {
                            ticketid = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        }
                        string tValue = string.Format("<br/><br/><a href='{0}?control=serviceswizard&module=Generic&ticketid=Generic&ServiceID={3}'>{4}</a>", UGITUtility.ToAbsoluteUrl("/layouts/uGovernIT/uGovernITConfiguration.aspx"), objTicket.Module.ModuleName, ticketid, Convert.ToString(survey[0][DatabaseObjects.Columns.ID]), agentLinkText);
                        textWithoutTokens = textWithoutTokens.Replace(tokens[i], tValue);
                    }
                    else
                    {
                        textWithoutTokens = textWithoutTokens.Replace(tokens[i], string.Empty);
                    }
                    continue;
                }

                else if (ticketColumn.Contains("|") && (ticketColumn.Split('|')[0]).ToLower() == "moduleagent" && objTicket != null && !disableLinks)
                {
                    string agentName = ticketColumn.Split('|')[1];
                    string agentLinkText = string.Empty;
                    if (ticketColumn.Split('|').Length > 2)
                        agentLinkText = ticketColumn.Split('|')[2];
                    if (string.IsNullOrEmpty(agentLinkText))
                        agentLinkText = "Please click here to enter the required data";

                    List<string> lstviewfields = new List<string>();
                    lstviewfields.Add(DatabaseObjects.Columns.ModuleNameLookup);
                    lstviewfields.Add(DatabaseObjects.Columns.ModuleStageMultiLookup);
                    lstviewfields.Add(DatabaseObjects.Columns.Id);
                    lstviewfields.Add(DatabaseObjects.Columns.IsActivated);
                    lstviewfields.Add(DatabaseObjects.Columns.Title);
                    DataRow[] agents = uHelper.GetServices(Constants.ModuleAgent, objTicket.Module.ModuleName, lstviewfields);
                    DataRow agentItem = null;
                    if (agents != null && agents.Count() > 0)
                    {
                        var agent = agents.CopyToDataTable().AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.Title]).ToLower() == HttpUtility.HtmlDecode(agentName).ToLower() && UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.IsActivated]));
                        if (agent != null)
                            agentItem = agents.CopyToDataTable<DataRow>().Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, UGITUtility.StringToInt(agent[DatabaseObjects.Columns.Id])))[0];
                    }

                    if (agentItem != null)
                    {
                        string[] moduleStages = uHelper.GetMultiLookupValue(Convert.ToString(agentItem[DatabaseObjects.Columns.ModuleStageMultiLookup]));
                        string agentStages = string.Join(Constants.Separator5, moduleStages);
                        if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.ModuleStepLookup))
                        {
                            //SPFieldLookupValue ticketStage = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ModuleStepLookup]));
                            if (Convert.ToString(item[DatabaseObjects.Columns.ModuleStepLookup]) != null && !string.IsNullOrEmpty(agentStages))
                            {
                                if (agentStages.IndexOf(Convert.ToString(item[DatabaseObjects.Columns.ModuleStepLookup])) != -1)//REPLACE MODULE AGENT TOKEN IF TICKET STAGE IS EQUAL TO AGENT STAGES
                                {
                                    string ticketid = string.Empty;
                                    if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TicketId))
                                    {
                                        ticketid = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                                    }
                                    string param = "TicketId=" + ticketid + "&ModuleName=" + objTicket.Module.ModuleName + "";
                                    string newAgentURL = UGITUtility.GetAbsoluteURL(string.Format("/SitePages/ServiceWizard.aspx?isdlg=1&serviceID={0}&{1}", Convert.ToString(agentItem[DatabaseObjects.Columns.Id]), param));
                                    string tValue = string.Format("<a href='{0}'>{1}</a>", newAgentURL, agentLinkText);
                                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], tValue);
                                }
                            }
                        }
                    }
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], string.Empty);
                    continue;
                }
                else if (ticketColumn.ToLower() == "userquestionsummary" && item != null && !disableLinks)
                {
                    string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.UserQuestionSummary));
                    if (!string.IsNullOrEmpty(questionInputs))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(questionInputs.Trim());
                        // ServiceInput inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                        //textWithoutTokens = textWithoutTokens.Replace(tokens[i], GenerateSummary(inputObj, item.Web));
                    }
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], string.Empty);
                    continue;
                }

                else if (ticketColumn.ToLower() == "includeactionbuttons" && !disableLinks)
                {
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], objTicket.GetActionbuttonforEmail(item, textWithoutTokens, objTicket));
                    continue;
                }

                if (ticketColumn == DatabaseObjects.Columns.TicketIdWithoutLink)
                {
                    ticketColumn = DatabaseObjects.Columns.TicketId;
                    disableEmailTicketLink = true;
                }

                if (tokenName.Contains("|"))
                    ticketColumn = tokenName.Split('|')[0];
                // if tokencolumn is not exist then replace it with empty string
                if (!UGITUtility.IsSPItemExist(item, ticketColumn))
                {
                    string tokenValue = uHelper.GetDefaultTokenValue(context, ticketColumn, true);
                    if (tokenValue == null)
                        tokenValue = string.Empty;

                    ticketColumn = ticketColumn.ToLower();
                    if (ticketColumn == "today")
                    {
                        DateTime dateVal = UGITUtility.StringToDateTime(tokenValue);
                        //add calendar days in token value
                        if (dateVal != DateTime.MinValue && !string.IsNullOrEmpty(tokenName) &&
                            tokenName.Split('|').Length == 3)
                        {
                            string dType = tokenName.Split('|')[1].ToLower().Trim();
                            if (dType == "days")
                            {
                                int days = UGITUtility.StringToInt(tokenName.Split('|')[2]);
                                dateVal = dateVal.AddDays(days);
                            }
                            else if (dType == "bdays")
                            {
                                int days = UGITUtility.StringToInt(tokenName.Split('|')[2]);
                                dateVal = AddWorkingDays(dateVal, days, context);
                            }
                            if (dateVal != DateTime.MinValue)
                                tokenValue = UGITUtility.GetDateStringInFormat(dateVal, false);
                        }
                    }
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], string.Empty);
                }
                else if (ticketColumn.ToLower() == DatabaseObjects.Columns.TicketResolutionComments.ToLower() || ticketColumn.ToLower() == DatabaseObjects.Columns.TicketComment.ToLower())
                {
                    string tokenValue = string.Empty;
                    List<string> resolutionDetail = new List<string>();
                    List<HistoryEntry> historyList = uHelper.GetHistory(item, ticketColumn);
                    if (historyList != null && historyList.Count > 0)
                    {
                        // Only need latest comment
                        HistoryEntry historyEntry = historyList[0];
                        if (!string.IsNullOrEmpty(historyEntry.created))
                            resolutionDetail.Add(string.Format("[{0}]", historyEntry.created));

                        if (!string.IsNullOrEmpty(historyEntry.createdBy))
                            resolutionDetail.Add(string.Format("{0}:", historyEntry.createdBy));

                        if (!string.IsNullOrEmpty(historyEntry.entry))
                            resolutionDetail.Add(string.Format("{0}", uHelper.ConvertTextAreaStringToHtml(historyEntry.entry)));

                        if (resolutionDetail.Count > 0)
                            tokenValue = string.Join(" ", resolutionDetail);
                    }
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], tokenValue);
                }
                else
                {
                    string tokenValue = Convert.ToString(item[ticketColumn]);

                    if (!string.IsNullOrEmpty(tokenValue))
                    {
                        configField = configFieldManager.GetFieldByFieldName(ticketColumn);
                        if (configField != null)
                        {
                            tokenValue = configFieldManager.GetFieldConfigurationData(ticketColumn, tokenValue);
                        }
                    }


                    if (tokenValue.Contains(Constants.Separator))
                    {
                        //FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
                        //FieldConfiguration configField = configFieldManager.Get(ticketColumn);

                        string fieldColumnType = string.Empty;
                        if (configField != null)
                        {
                            fieldColumnType = Convert.ToString(configField.Datatype);
                        }
                        if (ticketColumn == "UserField")
                        {
                            // User field
                            tokenValue = UGITUtility.RemoveIDsFromLookupString(tokenValue);
                        }
                        else if (tokenValue.Contains(Constants.Separator))
                        {
                            // Other lookup field
                            string[] components = UGITUtility.SplitString(tokenValue, Constants.Separator);
                            if (components.Length > 1)
                            {
                                tokenValue = string.Empty;
                                foreach (string component in components)
                                {
                                    int id = 0;
                                    if (!int.TryParse(component, out id))
                                    {
                                        if (tokenValue != string.Empty)
                                            tokenValue += ";";
                                        tokenValue += component;
                                    }
                                }
                            }
                        }
                    }

                    if (ticketColumn.Contains(DatabaseObjects.Columns.TicketId) && !disableEmailTicketLink && objTicket != null && !disableLinks)
                    {
                        string url = string.Format("{0}?TicketId={1}&ModuleName={2}&Tid={3}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath, context.SiteUrl),
                            Convert.ToString(item[DatabaseObjects.Columns.TicketId]), objTicket.Module.ModuleName, context.TenantID);
                        tokenValue = "<a href='" + url + "'>" + tokenValue + "</a>";
                    }
                    textWithoutTokens = textWithoutTokens.Replace(tokens[i], tokenValue);
                }
            }
            return textWithoutTokens;
        }

        public static UserProfile GetUser(ApplicationContext context, DataRow item, string fieldName)
        {
            try
            {
                string fieldValue = Convert.ToString(UGITUtility.GetSPItemValue(item, fieldName));
                if (string.IsNullOrEmpty(fieldValue))
                    return null;
                string id = fieldValue.Split(',')[0];
                UserProfile user = context.UserManager.GetUserById(id);
                return user;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        // service code
        public static string GetDefaultTokenValue(ApplicationContext context, string token, bool valueOnly)
        {
            return GetDefaultTokenValue(context, token, valueOnly, null);
        }

        public static string GetDefaultTokenValue(ApplicationContext context, string token, bool valueOnly, Services service)
        {
            DepartmentManager objDepartmentManager = new DepartmentManager(context);
            CompanyManager objCompanyManager = new CompanyManager(context);
            string val = string.Empty;
            if (token.StartsWith("[$"))
            {
                token = token.Replace("[$", string.Empty).Replace("$]", string.Empty);
            }

            switch (token.ToLower())
            {
                case "initiator":
                    if (valueOnly)
                        val = context.CurrentUser.Name;
                    else
                        val = context.CurrentUser.Id; // string.Format("{0}{1}{2}", context.CurrentUser.Id, Constants.Separator, context.CurrentUser.Name);
                    break;
                case "initiatormanager":
                    UserProfile userProfile = context.CurrentUser;
                    if (userProfile.ManagerID != null)
                    {
                        UserProfile userManger = context.UserManager.GetUserById(userProfile.ManagerID);
                        if (valueOnly)
                            val = userManger.Name;
                        else
                            val = userProfile.ManagerID; // string.Format("{0}{1}{2}", userProfile.ManagerID, Constants.Separator, userManger.Name);

                    }
                    break;
                case "today":
                    val = DateTime.Now.ToString();
                    break;
                case "initiatorlocation":
                    UserProfile userProfile1 = context.UserManager.GetUserById(context.CurrentUser.Id);
                    if (userProfile1.Location != "0")
                    {
                        if (valueOnly)
                            val = userProfile1.Location;
                        else
                            val = userProfile1.Location; // string.Format("{0}{1}{2}", userProfile1.LocationId, Constants.Separator, userProfile1.Location);
                    }
                    break;
                case "initiatordepartment":
                    UserProfile userProfile2 = context.CurrentUser;
                    if (userProfile2.Department != "0")
                    {
                        if (valueOnly)
                            val = userProfile2.Department;
                        else
                            val = Convert.ToString(userProfile2.Department); // string.Format("{0}{1}{2}", userProfile2.DepartmentId, Constants.Separator, userProfile2.Department);
                    }
                    break;
                case "initiatordepartmentmanager":
                    {
                        UserProfile requestorProfile = context.CurrentUser;
                        if (requestorProfile == null || requestorProfile.DepartmentId == 0)
                            break;

                        List<Department> departments = objDepartmentManager.Load();  //uGITCache.LoadDepartments(spWeb);
                        if (departments == null || departments.Count == 0)
                            break;

                        //Department dpt = departments.FirstOrDefault(x => x.ID == requestorProfile.DepartmentId);
                        //if (dpt == null || dpt.ManagerID == b)
                        //    break;

                        //UserProfile manager = UserProfile.LoadById(dpt.ManagerID, spWeb.Url);
                        //if (manager == null)
                        //    break;

                        //val = string.Format("{0};#{1}", manager.Id, manager.Name);
                    }
                    break;
                case "initiatordivisionmanager":
                    {
                        UserProfile requestorProfile = context.CurrentUser; //UserProfile.LoadById(spWeb.CurrentUser.ID, spWeb);
                        if (requestorProfile == null || requestorProfile.DepartmentId == 0)
                            break;

                        List<Department> departments = objDepartmentManager.Load();//uGITCache.LoadDepartments(spWeb);
                        if (departments == null && departments.Count == 0)
                            break;

                        Department dpt = departments.FirstOrDefault(x => x.ID == requestorProfile.DepartmentId);
                        if (dpt == null)
                            break;

                        List<Company> cmpies = objCompanyManager.Load(); //uGITCache.LoadCompanies(spWeb);
                        if (cmpies == null || cmpies.Count == 0)
                            break;

                        //Company cmp = cmpies.FirstOrDefault(x => x.CompanyDivisions != null && x.CompanyDivisions.Exists(y => y.ID == dpt.DivisionLookup.ID));
                        //if (cmp == null || cmp.CompanyDivisions == null || cmp.CompanyDivisions.Count == 0)
                        //    break;

                        //CompanyDivision division = cmp.CompanyDivisions.FirstOrDefault(x => x.ID == dpt.DivisionLookup.ID);
                        //if (division == null || division.ManagerID == 0)
                        //    break;

                        UserProfile manager = context.CurrentUser;
                        if (manager == null)
                            break;

                        val = string.Format("{0};#{1}", manager.Id.ToString(), manager.Name);
                    }
                    break;
                case "serviceowner":
                    {
                        if (service != null)
                        {
                            if (valueOnly)
                                val = service.OwnerUser;
                        }
                        else
                            val = null;
                    }
                    break;
                //SpDelta 70
                case "dayofmonth":
                    DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    val = Convert.ToString(firstDate);
                    break;
                //
                default:
                    val = null;
                    break;
            }

            return val;
        }

        public static string GetUserEmailList(List<string> users, ApplicationContext context)
        {
            if (users == null)
                return null;

            string userEmailList = string.Empty;
            foreach (string user in users)
            {
                string email = context.UserManager.GetUserById(user).Email;
                if (!string.IsNullOrEmpty(email))
                {
                    if (userEmailList != string.Empty)
                        userEmailList += ",";
                    userEmailList += email;
                }
            }

            return userEmailList;
        }

        public static List<List<string>> GetTokenValueArray(ApplicationContext context, MatchCollection tokens)
        {
            List<List<string>> tokenValue = new List<List<string>>();
            string separator = context.ConfigManager.GetValue(Constants.MailTokenValueSeparator);
            if (string.IsNullOrEmpty(separator))
                separator = ":";
            foreach (Match token in tokens)
            {
                string[] tempSplit = UGITUtility.SplitString(token.ToString(), separator);
                List<string> temp = new List<string>();
                temp.Add(tempSplit[0]);
                temp.Add(tempSplit[1]);

                tokenValue.Add(temp);
            }
            return tokenValue;
        }

        public static MatchCollection GetAllTokens(ApplicationContext context, string inputString)
        {
            string tokenStart = context.ConfigManager.GetValue(Constants.MailTokenStart);
            if (string.IsNullOrEmpty(tokenStart))
                tokenStart = "<$";
            string tokenEnd = context.ConfigManager.GetValue(Constants.MailTokenEnd);
            if (string.IsNullOrEmpty(tokenEnd))
                tokenEnd = "$>";
            string tokenStartRegex = string.Empty, tokenEndRegex = string.Empty;
            for (int i = 0; i < tokenStart.Length; i++)
                tokenStartRegex += "\\" + tokenStart[i];
            for (int i = 0; i < tokenEnd.Length; i++)
                tokenEndRegex += "\\" + tokenEnd[i];
            inputString = HttpUtility.HtmlDecode(inputString);
            MatchCollection matchedTokens = Regex.Matches(inputString, tokenStartRegex + "(.+?)" + tokenEndRegex, RegexOptions.IgnoreCase);
            return matchedTokens;
        }

        public static string GetModuleNameByToken(List<List<string>> tokenArray)
        {
            string moduleName = string.Empty;
            if (tokenArray != null && tokenArray.Count() > 0)
            {
                List<string> moduleToken = tokenArray.FirstOrDefault(x => x[0].ToLower() == string.Format("<${0}", DatabaseObjects.Columns.ModuleName.ToLower()));
                if (moduleToken != null && moduleToken.Count > 1)
                {
                    moduleName = moduleToken[1];
                    moduleName = moduleName.Replace("$>", string.Empty);
                }
            }
            return moduleName;
        }


        public static bool InsertTokenInItem(ApplicationContext context, string moduleName, List<List<string>> tokens, DataRow item)
        {
            string tokenStart = context.ConfigManager.GetValue(Constants.MailTokenStart);
            if (string.IsNullOrEmpty(tokenStart))
                tokenStart = "$>";
            string tokenEnd = context.ConfigManager.GetValue(Constants.MailTokenEnd);
            if (string.IsNullOrEmpty(tokenEnd))
                tokenEnd = "$>";

            foreach (List<string> token in tokens)
            {
                token[0] = token[0].Replace(tokenStart, "");
                token[1] = token[1].Replace(tokenEnd, "");

                if (token.Count >= 2)
                    ULog.WriteLog(string.Format("    Token: [{0}] - [{1}]", token[0], token[1]));
                else
                {
                    ULog.WriteLog("INVALID TOKEN!");
                    continue;
                }

                string tokenInternalName = GetInternalNameFromToken(token[0], context.TenantID);
                if (!string.IsNullOrEmpty(tokenInternalName) && UGITUtility.IfColumnExists(tokenInternalName, item.Table))
                {
                    //SPField field = item.Fields.GetField(tokenInternalName);
                    DataColumn field = item.Table.Columns[tokenInternalName];
                    FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
                    FieldConfiguration configField = configFieldManager.GetFieldByFieldName(field.ColumnName);
                    string fieldColumnType = string.Empty;
                    if (configField != null)
                    {
                        fieldColumnType = Convert.ToString(configField.Datatype);
                    }
                    else
                        fieldColumnType = Convert.ToString(field.DataType);
                    switch (fieldColumnType)
                    {
                        case "System.DateTime":
                            DateTime date = DateTime.Now;
                            DateTime.TryParse(token[1], out date);
                            item[tokenInternalName] = date.ToString();
                            break;
                        case "System.Boolean":
                            item[tokenInternalName] = UGITUtility.StringToBoolean(token[1]);
                            break;
                        case "Lookup":
                            {
                                try
                                {
                                    //SPFieldLookupValue lookupVal = GetLookupFieldValue(moduleName, item, tokenInternalName, token[1]);
                                    // if (lookupVal != null)
                                    DataTable dt = GetTableDataManager.GetTableData(configField.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                                    DataRow[] dr = null;
                                    if (dt != null && dt.Columns.Contains(DatabaseObjects.Columns.ID) && dt.Columns.Contains(DatabaseObjects.Columns.Title))
                                    {
                                        if (configField.FieldName == "RequestTypeLookup")
                                            dr = dt.Select(string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.RequestType, token[1], DatabaseObjects.Columns.ModuleNameLookup, moduleName));
                                        else
                                            dr = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, token[1]));

                                    }
                                    if (dr.Count() > 0)
                                    {
                                        item[tokenInternalName] = dr[0][DatabaseObjects.Columns.ID];
                                    }
                                    else
                                    {
                                        item[tokenInternalName] = "";
                                    }


                                    // else
                                    //Log.WriteLog(string.Format("ERROR - Lookup value not found for token {0}: {1}", tokenInternalName, token[1]));
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Problem in finding lookup in EmailFromTicket");
                                }
                            }
                            break;
                        case "UserField":
                            UserProfile user = context.UserManager.GetUserByUserName(token[1]);
                            if (user != null)
                                item[tokenInternalName] = user.Id;
                            else
                                ULog.WriteLog(string.Format("ERROR - User not found: {0}", token[1]));
                            break;
                        case "System.Int32":
                        case "System.Int64":
                            int number = 0;
                            int.TryParse(token[1], out number);
                            item[tokenInternalName] = number;
                            break;
                        default:
                            item[tokenInternalName] = token[1];
                            break;
                    }
                }
            }

            return true;
        }

        public static string GetInternalNameFromToken(string tokenName, string tenantId)
        {
            try
            {
                DataRow[] tokenColumnNames = GetTableDataManager.GetTableData(DatabaseObjects.Tables.MailTokenColumnName, string.Format("  {0} Like '%{1}%' and tenantid='{2}'", DatabaseObjects.Columns.KeyName, tokenName.Trim() + ";#", tenantId)).Select();
                if (tokenColumnNames.Count() > 0)
                {
                    return tokenColumnNames[0][DatabaseObjects.Columns.KeyValue].ToString();
                }
            }
            catch
            { }

            return string.Empty;
        }

        public static string GetDepartmentLabelName(ApplicationContext spWeb, DepartmentLevel level)
        {
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(spWeb);
            string val = string.Empty;
            switch (level)
            {
                case DepartmentLevel.Company:
                    {
                        val = configManager.GetValue(ConfigConstants.DepartmentLevel1Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Company";

                    }
                    break;
                case DepartmentLevel.Division:
                    {
                        val = configManager.GetValue(ConfigConstants.DepartmentLevel2Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Division";
                    }
                    break;
                case DepartmentLevel.Department:
                    {
                        val = configManager.GetValue(ConfigConstants.DepartmentLevel3Name);
                        if (string.IsNullOrWhiteSpace(val))
                            val = "Department";
                    }
                    break;
            }

            return val;

        }

        public static string FormatDepartment(ApplicationContext context, string value, bool showDeptHierarchy, string separator = "; ")
        {
            CompanyManager company = new CompanyManager(context);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            DepartmentManager deptManager = new DepartmentManager(context);
            List<string> formatedValues = new List<string>();
            List<Company> companies = company.Load();
            bool enableDivision = Convert.ToBoolean(configManager.GetValue(ConfigConstants.EnableDivision));
            bool showDepartmentdetail = showDeptHierarchy || Convert.ToBoolean(configManager.GetValue(ConfigConstants.ShowDepartmentDetail));
            List<string> lookups = UGITUtility.ConvertStringToList(value, Constants.Separator6);
            if (value.Contains(Constants.Separator))
            {
                lookups = UGITUtility.ConvertStringToList(value, Constants.Separator);
            }
            if (lookups != null && lookups.Count > 0)
            {
                List<string> format = new List<string>();
                foreach (string lookup in lookups)
                {
                    format = new List<string>();
                    Department dpt = deptManager.LoadByID(Convert.ToInt64(lookup));
                    if (dpt != null)
                    {
                        if (lookups.Count == 1 || showDepartmentdetail)
                        {
                            // Show Company if exists and we have more than one company
                            if (dpt.CompanyLookup != null && companies.Count > 1)
                            {
                                format.Add(dpt.CompanyLookup.Value);
                            }

                            // Show division if we have a division, divisions enabled AND division is not just a placeholder
                            if (dpt.DivisionLookup != null && enableDivision &&
                                dpt.DivisionLookup.Value != "N/A" && dpt.DivisionLookup.Value != dpt.CompanyLookup.Value)
                            {
                                format.Add(dpt.DivisionLookup.Value);
                            }

                            // Show Department
                            format.Add(dpt.Title);

                            formatedValues.Add(string.Join(" > ", format.ToArray()));
                        }
                        else
                        {
                            formatedValues.Add(dpt.Title);
                        }
                    }
                }
            }
            else
                return value; // Probably not in correct ID;#Value;#ID;#Value... format

            return string.Join(separator, formatedValues.ToArray());

        }

        /// <summary>
        /// Formats a department with the hierarchy as Company > Division > Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public static string FormatDepartment(ApplicationContext context, Department department)
        {
            List<string> deptList = new List<string>();
            CompanyManager companyManager = new CompanyManager(context);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            // Show Company if exists and we have more than one company
            List<Company> companies = companyManager.Load();
            if (department.CompanyLookup != null && companies.Count > 1)
                deptList.Add(department.CompanyLookup.Value);

            // Show division if we have a division, divisions enabled AND division is not just a placeholder
            bool enableDivision = Convert.ToBoolean(configManager.GetValue(ConfigConstants.EnableDivision));
            if (enableDivision && department.DivisionLookup != null &&
                department.DivisionLookup.Value != "N/A" && department.CompanyLookup != null && department.DivisionLookup.Value != department.CompanyLookup.Value)
            {
                deptList.Add(department.DivisionLookup.Value);
            }

            // Add Department
            deptList.Add(department.Title);

            string formattedDepartment = string.Join(" > ", deptList.ToArray());
            formattedDepartment = formattedDepartment.Replace("'", "\'").Replace("\"", "\\\"");

            return formattedDepartment;
        }

        public static bool SaveNewUserOld(ApplicationContext context, string user, string defaultPassword, string displayName, string email, Enums.UserType userType)
        {
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            bool newUserCreated = false;

            // Generate randow password if blank passed in
            //if (string.IsNullOrWhiteSpace(defaultPassword))
            //    defaultPassword = UserProfile.GenerateRandomPassword();

            if (userType == Enums.UserType.NewADUser)
            {


                // If Admin creds saved, decrypt them
                string ADAdminCredential = configManager.GetValue("ADAdminCredential");
                if (!string.IsNullOrEmpty(ADAdminCredential))
                {
                    //ConfigurationVariable updateADUserCredentail = configManager.Load("ADAdminCredential");
                    //string credential = updateADUserCredentail.KeyValue;
                    //string decryptedCredential = uGovernITCrypto.Decrypt(credential, Constants.UGITAPass);
                    //string[] credentaildetails = decryptedCredential.Split(',');
                    //useDefaultCredentials = Convert.ToBoolean(credentaildetails[3]);
                    //if (!useDefaultCredentials)
                    //{
                    //    domain = credentaildetails[0].Replace("LDAP://i:0#.w|", "");
                    //    // domain = credentaildetails[0].Replace("LDAP://", "");
                    //    userName = credentaildetails[1];
                    //    password = credentaildetails[2];
                    //}
                }

                try
                {
                    //SPSecurity.RunWithElevatedPrivileges(delegate ()
                    //{
                    //    ContextType contextType = IsConnectedToDomain() ? ContextType.Domain : ContextType.Machine;

                    //    PrincipalContext principalContext = new PrincipalContext(contextType, domain, userName, password);
                    //    UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, user);
                    //    if (up == null)
                    //    {
                    //        UserPrincipal userPrincipal = new UserPrincipal(principalContext, user, defaultPassword, true);
                    //        userPrincipal.Name = user;
                    //        userPrincipal.DisplayName = displayName;
                    //        if (contextType != ContextType.Machine)
                    //        {
                    //            userPrincipal.UserPrincipalName = user;
                    //            if (!string.IsNullOrEmpty(email))
                    //                userPrincipal.EmailAddress = email;
                    //        }
                    //        userPrincipal.PasswordNeverExpires = true;
                    //        userPrincipal.Enabled = true;
                    //        userPrincipal.Save();
                    //        newUserCreated = true;
                    //    }
                    //});
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Error creating user " + user);
                    newUserCreated = false;
                }
            }
            else if (userType == Enums.UserType.NewFBAUser)
            {
                try
                {
                    //MembershipUser usr;
                    //MembershipCreateStatus createStatus;

                    //MembershipUserCollection users = Membership.FindUsersByName(user);
                    //if (users.Count == 0)
                    //{
                    //    if (email != string.Empty)
                    //    {
                    //        usr = Membership.CreateUser(user, defaultPassword, email, Constants.passwordQuestion, Constants.passwordAnswer, true, out createStatus);
                    //        if (createStatus == MembershipCreateStatus.Success)
                    //            newUserCreated = true;
                    //    }
                    //    else
                    //    {
                    //        usr = Membership.CreateUser(user, defaultPassword);
                    //        usr.IsApproved = true;
                    //        Membership.UpdateUser(usr);
                    //        newUserCreated = true;
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Error creating user " + user);
                    newUserCreated = false;
                }
            }

            return newUserCreated;

        }

        public static List<string> GetAllTableFromDatabase()
        {
            List<string> listTable = new List<string>();
            string sql = "exec GetAllTableList";
            SqlDataReader reader = DBConnection.ExecuteReader(sql);
            while (reader.Read())
            {
                listTable.Add(reader["TABLE_NAME"].ToString());
            }
            return listTable;
        }

        public static void GetStartEndDateFromDateView(string dateView, ref DateTime startDate, ref DateTime endDate, ref string range)
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
            DateTime currentDate = DateTime.Now;
            range = string.Empty;
            switch (dateView.ToLower())
            {
                case "current month":
                    startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    range = string.Format("{0} {1}", startDate.ToString("MMM"), startDate.Year);
                    break;
                case "last 30 days":
                    startDate = currentDate.AddDays(-30).Date;
                    endDate = currentDate.Date;
                    range = string.Format("Last 30 Days");
                    break;
                case "current year":
                    startDate = new DateTime(currentDate.Year, 1, 1);
                    endDate = new DateTime(currentDate.Year, 12, DateTime.DaysInMonth(currentDate.Year, 12));
                    range = string.Format("{0}", startDate.Year);
                    break;
                case "last month":
                    int month1 = currentDate.Month;
                    int year1 = currentDate.Year;
                    if (currentDate.Month == 1)
                    {
                        month1 = 12;
                        year1 = currentDate.Year - 1;
                    }
                    else
                    {
                        month1 = currentDate.Month - 1;
                    }
                    startDate = new DateTime(year1, month1, 1);
                    endDate = new DateTime(year1, month1, DateTime.DaysInMonth(year1, month1));
                    range = string.Format("{0} {1}", startDate.ToString("MMM"), startDate.Year);
                    break;
                case "last year":
                    startDate = new DateTime(currentDate.Year - 1, 1, 1);
                    endDate = new DateTime(currentDate.Year - 1, 12, DateTime.DaysInMonth(currentDate.Year - 1, 12));
                    range = string.Format("{0}", startDate.Year);
                    break;
                case "last 6 months":
                    startDate = new DateTime(currentDate.AddMonths(-6).Year, currentDate.AddMonths(-6).Month, 1);
                    endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    if (startDate.Year == endDate.Year)
                        range = string.Format("{0}-{1} {2}", startDate.ToString("MMM"), endDate.ToString("MMM"), startDate.Year);
                    else
                        range = string.Format("{0} {1}-{2} {3}", startDate.ToString("MMM"), startDate.Year, endDate.ToString("MMM"), endDate.Year);
                    break;
                case "last 3 months":
                    startDate = new DateTime(currentDate.AddMonths(-3).Year, currentDate.AddMonths(-3).Month, 1);
                    endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    if (startDate.Year == endDate.Year)
                        range = string.Format("{0}-{1} {2}", startDate.ToString("MMM"), endDate.ToString("MMM"), startDate.Year);
                    else
                        range = string.Format("{0} {1}-{2} {3}", startDate.ToString("MMM"), startDate.Year, endDate.ToString("MMM"), endDate.Year);
                    break;
                case "last 12 months":
                    startDate = new DateTime(currentDate.AddMonths(-12).Year, currentDate.AddMonths(-12).Month, 1);
                    endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    if (startDate.Year == endDate.Year)
                        range = string.Format("{0}-{1} {2}", startDate.ToString("MMM"), endDate.ToString("MMM"), startDate.Year);
                    else
                        range = string.Format("{0} {1}-{2} {3}", startDate.ToString("MMM"), startDate.Year, endDate.ToString("MMM"), endDate.Year);
                    break;
                case "last 24 hours":
                    startDate = currentDate.AddDays(-1);
                    endDate = currentDate;
                    range = string.Format("24 Horus");
                    break;
                case "last 7 days":
                    startDate = currentDate.AddDays(-((int)currentDate.DayOfWeek + 7)).Date;// currentDate.AddDays(-1);
                    endDate = startDate.AddDays(7).Date;
                    range = string.Format("{0} - {1}", startDate.ToString("dd-MMM"), endDate.ToString("dd-MMM"));
                    break;
                case "current week":
                    startDate = currentDate.AddDays(-((int)currentDate.DayOfWeek)).Date;
                    endDate = currentDate;
                    range = string.Format("{0} - {1}", startDate.ToString("dd-MMM"), endDate.ToString("dd-MMM"));
                    break;
            }

            string view = dateView.ToLower();
            if (view == "current quarter" || view == "last quarter")
            {
                int month = currentDate.Month;
                int remander = month % 3;
                int result = month / 3;
                int startMonth = 0;
                if (remander == 0)
                {
                    startMonth = month - 2;
                }
                else
                {
                    startMonth = (result * 3) + 1;
                }

                if (view == "current quarter")
                {
                    startDate = new DateTime(currentDate.Year, startMonth, 1);
                    endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else if (view == "last quarter")
                {
                    int month1 = startMonth;
                    int year1 = currentDate.Year;
                    if (startMonth == 1)
                    {
                        month1 = 10;
                        year1 = currentDate.Year - 1;
                    }
                    else
                    {
                        month1 = month1 - 3;
                    }

                    startDate = new DateTime(year1, month1, 1);
                    endDate = new DateTime(year1, month1 + 2, DateTime.DaysInMonth(year1, month1 + 2));
                }
                if (startDate.Year == endDate.Year)
                    range = string.Format("{0}-{1} {2}", startDate.ToString("MMM"), endDate.ToString("MMM"), startDate.Year);
                else
                    range = string.Format("{0} {1}-{2} {3}", startDate.ToString("MMM"), startDate.Year, endDate.ToString("MMM"), endDate.Year);
            }

            if (string.IsNullOrEmpty(range))
                range = "All";
        }

        public static string FormatNumber(double value, string labelFormat)
        {
            string localizedValue = value.ToString();
            if (string.IsNullOrWhiteSpace(labelFormat))
            {
                localizedValue = value.ToString("#,0.##");
                return localizedValue;
            }


            labelFormat = labelFormat.ToLower();
            if (labelFormat == "currency")
            {
                //if (value >= 10000000000)
                //    localizedValue = (value / 1000000000D).ToString("#,0.00") + "B"; // Billions
                //else 
                if (value >= 10000000)
                    localizedValue = (value / 1000000D).ToString("#,0.00") + "M"; // Millions
                else if (value >= 10000)
                    localizedValue = (value / 1000D).ToString("#,0.00") + "K"; // Thousands

                localizedValue = "$" + localizedValue; // Need to generalize at some point to support other currencies!
            }
            else if (labelFormat == "mintodays")
            {
                localizedValue = value.ToString("#,0.##") + "d";
            }
            else if (labelFormat == "withdollaronly")
            {
                localizedValue = string.Format("${0}", value.ToString("#,0.00"));
            }
            else if (labelFormat == "currencywithoutdecimal")
            {
                if (value >= 10000000)
                    localizedValue = (value / 1000000D).ToString("#,0") + "M"; // Millions
                else if (value >= 10000)
                    localizedValue = (value / 1000D).ToString("#,0") + "K"; // Thousands

                localizedValue = "$" + localizedValue;
            }
            else
            {
                localizedValue = value.ToString("#,0.##");
            }

            return localizedValue;
        }

        public static System.Drawing.Color TranslateColorCode(string colorCode, System.Drawing.Color defaultColor)
        {
            if (string.IsNullOrWhiteSpace(colorCode))
                return defaultColor;

            System.Drawing.KnownColor knowColor = KnownColor.Transparent;
            bool isKnowColor = Enum.TryParse<System.Drawing.KnownColor>(colorCode, out knowColor);
            System.Drawing.Color translatedColor = new System.Drawing.Color();
            if (isKnowColor)
            {
                translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode);
            }
            else
            {
                if (!colorCode.Contains("#"))
                    colorCode = "#" + colorCode;
                else
                {
                    if (colorCode.ToArray().Length < 7)
                        colorCode = defaultColor.Name;
                }

                try
                {
                    translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    try
                    {
                        translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode.Replace("#", ""));
                    }
                    catch (Exception exp)
                    {
                        ULog.WriteException(exp);
                        translatedColor = defaultColor;
                    }
                }
            }
            return translatedColor;
        }

        public static string ReplaceTokenWithValue(ApplicationContext context, string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return string.Empty;

            MatchCollection matchedTokens = Regex.Matches(expression, "\\[\\$(.+?)\\$\\]", RegexOptions.IgnoreCase);
            foreach (Match token in matchedTokens)
            {
                expression = expression.Replace(token.ToString(), GetDefaultTokenValue(context, token.ToString(), true));
            }

            return expression;
        }

        public static List<HistoryEntry> GetProjectSummaryNote(string data)
        {
            List<HistoryEntry> histories = new List<HistoryEntry>();

            string entryseprator = "<;#>";
            string dataseprator = ";#";
            string[] entries = UGITUtility.SplitString(data, entryseprator);
            foreach (string entry in entries)
            {
                HistoryEntry historyentry = new HistoryEntry();
                string[] datas = UGITUtility.SplitString(entry, dataseprator);
                historyentry.createdBy = datas[0];
                historyentry.created = datas[1];
                historyentry.entry = datas[2];
                histories.Add(historyentry);
            }
            return histories;
        }

        public static string GetCurrentTimestamp()
        {
            return DateTime.Now.ToString("MMM-dd-yyyy_HHmmss_ffff");
        }

        public static Dictionary<string, object> ReportScheduleDict
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constants.ReportScheduleDict] == null)
                    return new Dictionary<string, object>();
                else
                    return (Dictionary<string, object>)HttpContext.Current.Session[Constants.ReportScheduleDict];
            }
            set
            {
                HttpContext.Current.Session[Constants.ReportScheduleDict] = value;
            }
        }

        public static string DataTypeMapping(string dateType)
        {
            string type = "String";
            switch (dateType)
            {
                case "System.Int32":
                case "System.Int64":
                    type = "Integer"; break;
                default:
                    break;
            }
            return type;


        }

        public static Control GetParentControl(Control childControl, string ParentCtrID)
        {
            Control parent = childControl.Parent;
            while (parent.ID != ParentCtrID)
            {
                parent = parent.Parent;
            }
            if (parent.ID == ParentCtrID)
                return parent;

            return null;
        }

        public static void BindOrderDropDown(ASPxComboBox comboBox, ApplicationContext context)
        {
            comboBox.Items.Clear();
            int itemCount = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows.Count, currentCount = itemCount;
            List<int> lst = new List<int>();
            int i = 1;
            do
            {
                if (i > 0)
                    comboBox.Items.Add(new ListEditItem(Convert.ToString(i), i));
                i++;
                --currentCount;
            } while (currentCount > 0);

            comboBox.Items.Add(new ListEditItem(Convert.ToString(i), i));
            comboBox.Value = i;
        }

        public static string GetValueByColumn(Row row, FieldAliasCollection fieldAliasName, List<string> lstColumn)
        {
            if (row == null || fieldAliasName == null || lstColumn == null)
            {
                ULog.WriteLog("ERROR: Null values passed into GetValueByColumn!");
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(fieldAliasName.InternalName))
            {
                int index = lstColumn.FindIndex(s => s.Equals(fieldAliasName.InternalName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                if (index != -1)
                {
                    string value = row[index].DisplayText;
                    if (!string.IsNullOrEmpty(value))
                        value = value.Trim();
                    if (fieldAliasName.InternalName == "SortToBottom")
                    {
                        if (value == "False")
                        {
                            value = "false";
                        }
                        else if (value == "True")
                        {
                            value = "true";
                        }
                        else if (String.IsNullOrEmpty(value))
                        {
                            value = "false";
                        }
                    }
                    return value;
                }
            }

            if (!string.IsNullOrEmpty(fieldAliasName.AliasNames))
            {
                string[] aliasNames = fieldAliasName.AliasNames.Split(',');
                foreach (var tempName in aliasNames)
                {
                    int index = lstColumn.FindIndex(s => s.Equals(tempName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                    if (index != -1)
                    {
                        string value = row[index].DisplayText;
                        if (!string.IsNullOrEmpty(value))
                            value = value.Trim();
                        return value;
                    }
                }
            }

            return string.Empty; // Not found!
        }

        public static string GetValueByColumn(DataRow row, FieldAliasCollection fieldAliasName, List<string> lstColumn)
        {
            if (row == null || fieldAliasName == null || lstColumn == null)
            {
                ULog.WriteLog("ERROR: Null values passed into GetValueByColumn!");
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(fieldAliasName.InternalName))
            {
                int index = lstColumn.FindIndex(s => s.Equals(fieldAliasName.InternalName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                if (index != -1)
                {
                    string value = Convert.ToString(row[index]);
                    if (!string.IsNullOrEmpty(value))
                        value = value.Trim();
                    return value;
                }
            }

            if (!string.IsNullOrEmpty(fieldAliasName.AliasNames))
            {
                string[] aliasNames = fieldAliasName.AliasNames.Split(',');
                foreach (var tempName in aliasNames)
                {
                    int index = lstColumn.FindIndex(s => s.Equals(tempName, StringComparison.OrdinalIgnoreCase)); // Do case-insensitive comparison
                    if (index != -1)
                    {
                        string value = Convert.ToString(row[index]);
                        if (!string.IsNullOrEmpty(value))
                            value = value.Trim();
                        return value;
                    }
                }
            }

            return string.Empty; // Not found!
        }

        public static string SetFilledValues(ApplicationContext context, List<string> lstColumn, Row rowDate, DataRow listItem, List<FieldAliasCollection> lstFieldAliasCollection, string ModuleName="")
        {
            string returnMsg = string.Empty;
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            DepartmentManager deptManager = new DepartmentManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
            ModuleViewManager moduleViewManager  = new ModuleViewManager(context);
            StudioManager studioManager = new StudioManager(context);
            FieldConfiguration field = null;
            FieldAliasCollection facItem = null;
            DataColumn spField = null;
            bool isDivisionAlreadySet = false;
            bool enableStudioDivisionHierarchy = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);

            
            List<string> columnsToSeparate = new List<string>();
            List<FieldAliasCollection> filteredItems = new List<FieldAliasCollection>();

            //Check If Separate Department and Division columns exists
            bool departmentColumnFound = false;
            bool divisionColumnFound = false;

            foreach (string ColItem in lstColumn)
            {
                if (!departmentColumnFound)
                {
                    FieldAliasCollection departmentItem = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.DepartmentLookup && x.AliasNames.ToLower().Split(',').Contains(ColItem.ToLower())).FirstOrDefault();
                    if (departmentItem != null)
                    {
                        departmentColumnFound = true;
                        columnsToSeparate.Add(ColItem);
                        filteredItems.Add(departmentItem);
                        continue;
                    } 
                }
                if (!divisionColumnFound)
                {
                    FieldAliasCollection divisionItem = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.DivisionLookup && x.AliasNames.ToLower().Split(',').Contains(ColItem.ToLower())).FirstOrDefault();
                    if (divisionItem != null)
                    {
                        divisionColumnFound = true;
                        columnsToSeparate.Add(ColItem);
                        filteredItems.Add(divisionItem);
                        continue;
                    }
                }
                if (departmentColumnFound && divisionColumnFound)
                    break;
            } 
            
            if (departmentColumnFound && divisionColumnFound)
            {
                FieldAliasCollection departmentItem = filteredItems.Where(x => x.InternalName == DatabaseObjects.Columns.DepartmentLookup).FirstOrDefault();
                FieldAliasCollection divisionItem = filteredItems.Where(x => x.InternalName == DatabaseObjects.Columns.DivisionLookup).FirstOrDefault();

                string divisionValue = "";
                string departmentValue = "";
                departmentValue = uHelper.GetValueByColumn(rowDate, departmentItem, lstColumn);
                divisionValue = uHelper.GetValueByColumn(rowDate, divisionItem, lstColumn);

                if (!string.IsNullOrWhiteSpace(divisionValue) && !string.IsNullOrWhiteSpace(departmentValue))
                {
                    List<CompanyDivision> divisions = companyDivisionManager.GetCompanyDivisionDataWithAllDepartments().Where(x => !x.Deleted).ToList();
                    CompanyDivision divison = divisions.Where(x => x.Title.ToLower() == divisionValue.ToLower()).FirstOrDefault();
                    if (divison != null)
                    {
                        Department dept = divison.Departments.Where(x => x.Title.ToLower() == departmentValue.ToLower()).FirstOrDefault();
                        if (dept != null)
                        {
                            listItem[DatabaseObjects.Columns.DivisionLookup] = divison.ID.ToString();
                            listItem[DatabaseObjects.Columns.DepartmentLookup] = dept.ID.ToString();
                        }
                        else
                            returnMsg = $"{returnMsg}<br>Invalid combination: Division - {divisionValue} and Department - {departmentValue}";
                    }
                    else
                        returnMsg = $"{returnMsg}<br>Invalid combination: Division - {divisionValue} and Department - {departmentValue}";
                }
                else if (string.IsNullOrWhiteSpace(divisionValue) && string.IsNullOrWhiteSpace(departmentValue)) { }
                else
                    returnMsg = $"{returnMsg}<br>Invalid combination: Division - {divisionValue} and Department - {departmentValue}";
            }

            //Check If Separate Request Category, Sub-category, Type column exists
            bool requestCategoryFound = false;
            bool requestSubCategoryFound = false;
            bool requestTypeFound = false;
            filteredItems = new List<FieldAliasCollection>();

            foreach (string ColItem in lstColumn)
            {
                if (!requestCategoryFound)
                {
                    FieldAliasCollection category = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.Category && x.AliasNames.ToLower().Split(',').Contains(ColItem.ToLower())).FirstOrDefault();
                    if (category != null)
                    {
                        requestCategoryFound = true;
                        columnsToSeparate.Add(ColItem);
                        filteredItems.Add(category);
                        continue;
                    }
                }
                if (!requestSubCategoryFound)
                {
                    FieldAliasCollection subCategory = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.SubCategory && x.AliasNames.ToLower().Split(',').Contains(ColItem.ToLower())).FirstOrDefault();
                    if (subCategory != null)
                    {
                        requestSubCategoryFound = true;
                        columnsToSeparate.Add(ColItem);
                        filteredItems.Add(subCategory);
                        continue;
                    }
                }
                if (!requestTypeFound)
                {
                    FieldAliasCollection requestType = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.RequestType && x.AliasNames.ToLower().Split(',').Contains(ColItem.ToLower())).FirstOrDefault();
                    if (requestType != null)
                    {
                        requestTypeFound = true;
                        columnsToSeparate.Add(ColItem);
                        filteredItems.Add(requestType);
                        continue;
                    }
                }
                if (requestCategoryFound && requestSubCategoryFound && requestTypeFound)
                    break;
            }

            if (requestCategoryFound && requestSubCategoryFound && requestTypeFound)
            {
                FieldAliasCollection requestCategoryItem = filteredItems.Where(x => x.InternalName == DatabaseObjects.Columns.Category).FirstOrDefault();
                FieldAliasCollection requestSubCategoryItem = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.SubCategory).FirstOrDefault();
                FieldAliasCollection requestTypeItem = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.RequestType).FirstOrDefault();
                string requestCategoryItemValue = uHelper.GetValueByColumn(rowDate, requestCategoryItem, lstColumn);
                string requestSubCategoryItemValue = uHelper.GetValueByColumn(rowDate, requestSubCategoryItem, lstColumn);
                string requestTypeItemValue = uHelper.GetValueByColumn(rowDate, requestTypeItem, lstColumn);
                
                if (!string.IsNullOrWhiteSpace(requestCategoryItemValue) && !string.IsNullOrWhiteSpace(requestSubCategoryItemValue) && !string.IsNullOrWhiteSpace(requestTypeItemValue))
                {

                    RequestTypeManager RequestTypeMgr = new RequestTypeManager(context);
                    DataTable dtRequestType = RequestTypeMgr.GetDataTable();
                    DataRow[] datarows = dtRequestType.Select(string.Format("{0}='{1}' AND {2}=False AND {3}='{4}' AND {5}='{6}' AND {7}='{8}'",
                        DatabaseObjects.Columns.ModuleNameLookup, ModuleName, DatabaseObjects.Columns.Deleted, DatabaseObjects.Columns.Category, requestCategoryItemValue.Trim(),
                        DatabaseObjects.Columns.SubCategory, requestSubCategoryItemValue.Trim(), DatabaseObjects.Columns.RequestType, requestTypeItemValue.Trim()));
                    if (datarows != null && datarows.Length == 1)
                    {
                        listItem[DatabaseObjects.Columns.RequestTypeLookup] = UGITUtility.ObjectToString(datarows[0][DatabaseObjects.Columns.ID]);
                    }
                    else
                        returnMsg = $"{returnMsg}<br>Invalid combination: Request Category - {requestCategoryItemValue}, Request Sub-Category  - {requestSubCategoryItemValue} and Request Type - {requestTypeItemValue}";
                }
                else if (string.IsNullOrWhiteSpace(requestCategoryItemValue) && string.IsNullOrWhiteSpace(requestSubCategoryItemValue) && string.IsNullOrWhiteSpace(requestTypeItemValue)) { }
                else
                    returnMsg = $"{returnMsg}<br>Invalid combination: Request Category - {requestCategoryItemValue}, Request Sub-Category  - {requestSubCategoryItemValue} and Request Type - {requestTypeItemValue}";
            }

            foreach (string ColItem in lstColumn)
            {
                if ((departmentColumnFound && divisionColumnFound) || (requestCategoryFound && requestSubCategoryFound && requestTypeFound))
                {
                    if (columnsToSeparate.Contains(ColItem))
                        continue;
                }

                string value = string.Empty;
                string columnName = ColItem.ToLower();
                facItem = lstFieldAliasCollection.Where(x => x.InternalName == ColItem || x.AliasNames.ToLower().Split(',').Contains(columnName)).FirstOrDefault();
                if (facItem == null)
                {
                    if (UGITUtility.IfColumnExists(ColItem, listItem.Table))
                        facItem = new FieldAliasCollection(listItem.Table.TableName, ColItem, ColItem); // Create a dummy entry so we can use internal name to access
                    else
                    {
                        if (columnName == Constants.DivisionGLCode.ToLower())
                        {
                            facItem = new FieldAliasCollection(listItem.Table.TableName, DatabaseObjects.Columns.DivisionLookup, ColItem); // Create a dummy entry so we can use internal name to access
                        }
                        else if (columnName == Constants.StudioTitle.ToLower())
                        {
                            facItem = new FieldAliasCollection(listItem.Table.TableName, DatabaseObjects.Columns.StudioLookup, ColItem); // Create a dummy entry so we can use internal name to access
                        }
                        else
                            continue;
                    }
                }

                spField = listItem.Table.Columns[facItem.InternalName];
                if (spField != null)
                {
                    if (spField.ColumnName.Equals("ContentType") || spField.ColumnName.Equals("Attachments"))
                        continue;
                }

                field = fieldConfigurationManager.GetFieldByFieldName(facItem.InternalName);
                if (field == null)
                {
                    // "ProjectLifeCycleLookup"
                    if (ColItem == "Lifecycle" || facItem.InternalName == "ProjectLifeCycleLookup")
                    {
                        string lyfCycleName = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        LifeCycle lifeCycle = null;
                        List<LifeCycle> objLifeCycle = new List<LifeCycle>();
                        LifeCycleManager lifeCycleHelper = new LifeCycleManager(context);
                        LifeCycleStageManager objLifeCycleStageManager = new LifeCycleStageManager(context);
                        objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
                        var stageList = objLifeCycleStageManager.Load();
                        // Select the first LifeCycle from selected if exits otherwise load first lifecyle
                        if (!string.IsNullOrEmpty(lyfCycleName))
                        {
                            lifeCycle = objLifeCycle.FirstOrDefault(x => x.Name == lyfCycleName);
                            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                            {
                                listItem[DatabaseObjects.Columns.ModuleStepLookup] = null;
                                // listItem[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                                // listItem[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                                listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                                listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToInt64(lifeCycle.ID);

                            }
                            else if (lifeCycle == null)
                            {
                                // setting first lifecycle kind of default if 
                                lifeCycle = objLifeCycle.FirstOrDefault();

                                if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                                {
                                    //ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);

                                    listItem[DatabaseObjects.Columns.ModuleStepLookup] = lifeCycle.Stages[0].ID;
                                    listItem[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                                    listItem[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                                    listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                                    listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToInt64(lifeCycle.ID);
                                }
                            }
                        }
                        else
                        {
                            listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = DBNull.Value;
                        }

                        listItem[DatabaseObjects.Columns.ScrumLifeCycle] = false;      //keep false as default setting

                    }
                    else if (facItem.InternalName == "CloseDate")
                    {
                        if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.Closed) && UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.CloseDate) && !string.IsNullOrEmpty(Convert.ToString(uHelper.GetValueByColumn(rowDate, facItem, lstColumn))))
                        {
                            listItem[DatabaseObjects.Columns.Closed] = true;
                            listItem[DatabaseObjects.Columns.TicketStatus] = "Closed";
                        }

                        else
                        {
                            if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.Closed))
                                listItem[DatabaseObjects.Columns.Closed] = false;
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            listItem[facItem.InternalName] = value.ToDateTime();

                        }

                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.BudgetCategoryLookup)
                        listItem[DatabaseObjects.Columns.BudgetCategoryLookup] = 0;
                    else if (facItem.InternalName == "ServiceWizardOnly" || facItem.InternalName == "StagingId")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                        {
                            listItem[facItem.InternalName] = value;
                        }
                    }
                    else if (facItem.InternalName == "TaskTemplateLookup" || facItem.InternalName == "TaskTemplateLookup")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                            listItem[facItem.InternalName] = value;
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.AutoAssignPRP)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                            listItem[facItem.InternalName] = value;
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.SLADisabled || facItem.InternalName == DatabaseObjects.Columns.Use24x7Calendar
                        || facItem.InternalName == DatabaseObjects.Columns.OutOfOffice)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = false;
                        }
                        else
                            listItem[facItem.InternalName] = UGITUtility.StringToBoolean(value);
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.Id)
                    {
                        listItem[facItem.InternalName] = Guid.NewGuid();
                    }
                    else if (facItem.InternalName == "DepartmentId")
                    {
                        string oldvalue = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        Department departmentObj = deptManager.Load(x => x.Title == oldvalue).FirstOrDefault();

                        if (departmentObj != null)
                        {
                            listItem[facItem.InternalName] = UGITUtility.StringToLong(departmentObj.ID);
                        }
                        else
                        {
                            returnMsg = $"{returnMsg}<br>Department {oldvalue}";
                        }
                    }
                    else if (facItem.InternalName == "RoleId")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        GlobalRole role = roleManager.Load(x => x.Name == value).FirstOrDefault();
                        if (role != null)
                        {
                            listItem[facItem.InternalName] = role.Id;
                        }
                        else
                        {
                            returnMsg = $"{returnMsg}<br>Role {value}";
                        }
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.ShortName)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            int shortNameLimit = 50;
                            string shortNameLengthString = objConfigurationVariableHelper.GetValue(ConfigConstants.ShortNameCharacters);
                            if (!string.IsNullOrEmpty(shortNameLengthString))
                                int.TryParse(shortNameLengthString, out shortNameLimit);
                         
                            listItem[facItem.InternalName] = value.Length > shortNameLimit ? value.Substring(0, shortNameLimit) : value;
                        }
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.ApproxContractValue)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (double.TryParse(value.Replace(",", ""), out double contractValue))
                                listItem[facItem.InternalName] = contractValue.ToString();
                            else
                                returnMsg = $"{returnMsg}<br>Invalid {ColItem} - {value}";
                        }
                    }
                    else
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (listItem.Table.Columns[facItem.InternalName].DataType == typeof(System.Boolean))
                                listItem[facItem.InternalName] = UGITUtility.StringToBoolean(value);
                            else
                                listItem[facItem.InternalName] = value;
                        }
                    }

                }
                else
                {
                    if (field.Datatype == "Lookup")
                    {

                        if (lstFieldAliasCollection[0].ListName == "RequestType" && facItem.InternalName != "ModuleNameLookup" && facItem.InternalName != "FunctionalAreaLookup" && facItem.InternalName != "APPTitleLookup" && facItem.InternalName != "BudgetIdLookup")
                        {
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            listItem[facItem.InternalName] = UGITUtility.StringToLong(value);
                            if (string.IsNullOrEmpty(value))
                                returnMsg = $"{returnMsg}<br>Invalid {field.FieldName} {value}";
                        }
                        else if (facItem.InternalName == "ModuleNameLookup")
                        {

                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            List<string> lstwords = UGITUtility.ConvertStringToList(value, ")");
                            //string[] words = value.Split(new[] { ")" }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> lstwords1 = UGITUtility.ConvertStringToList(lstwords[0], "(");
                            //words = words[0].Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries);

                            listItem[facItem.InternalName] = UGITUtility.ObjectToString(lstwords1.FirstOrDefault());
                        }
                        else if (facItem.InternalName == DatabaseObjects.Columns.CRMCompanyLookup || facItem.InternalName == DatabaseObjects.Columns.ContactLookup ||
                            facItem.InternalName == DatabaseObjects.Columns.TicketOPMIdLookup || facItem.InternalName == DatabaseObjects.Columns.TicketLEMIdLookup)
                        {
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            value = fieldConfigurationManager.GetFieldConfigurationIdByName(facItem.InternalName, value, facItem.ListName);
                            if (!string.IsNullOrEmpty(value))
                            {
                                var companyData = GetTableDataManager.GetTableData(field.ParentTableName, $"{DatabaseObjects.Columns.Id}='{value}' and TenantID='{context.TenantID}'").Select()[0];
                                if (companyData != null)
                                {
                                    listItem[facItem.InternalName] = Convert.ToString(companyData[DatabaseObjects.Columns.TicketId]);
                                }
                            }
                            else
                                returnMsg = $"{returnMsg}<br>Invalid {field.FieldName} {value}";
                        }
                        else if (columnName == Constants.DivisionGLCode.ToLower())
                        {
                            if (isDivisionAlreadySet)
                                continue;

                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            CompanyDivision division = companyDivisionManager.Load(x => x.GLCode == value).FirstOrDefault();
                            if (division != null)
                            {
                                listItem[facItem.InternalName] = UGITUtility.StringToLong(division.ID);
                            }
                            else
                                returnMsg = $"{returnMsg}<br>Invalid {field.FieldName} {value}";
                        }
                        else if (columnName == Constants.StudioTitle.ToLower())
                        {
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            List<Studio> lstStudios = studioManager.Load(x => x.Title == value);

                            if (lstStudios.Count > 0)
                            {
                                listItem[facItem.InternalName] = lstStudios[0].ID;
                                //Set Division based on Studio.
                                if (listItem.Table.Columns.Contains(DatabaseObjects.Columns.DivisionLookup) && enableStudioDivisionHierarchy)
                                {
                                    listItem[DatabaseObjects.Columns.DivisionLookup] = lstStudios[0].DivisionLookup;
                                    isDivisionAlreadySet = true;
                                }
                            }
                        }
                        else if (columnName == DatabaseObjects.Columns.RequestTypeLookup.ToLower() || facItem.InternalName == DatabaseObjects.Columns.RequestTypeLookup)
                        {
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            value = fieldConfigurationManager.GetFieldConfigurationIdByName(facItem.InternalName, value, ModuleName);
                            if (!string.IsNullOrEmpty(value))
                                listItem[facItem.InternalName] = UGITUtility.StringToLong(value);
                        }
                        else
                        {
                            if (facItem.InternalName == DatabaseObjects.Columns.DivisionLookup && isDivisionAlreadySet && enableStudioDivisionHierarchy)
                                continue;
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (string.IsNullOrEmpty(value))
                                continue;
                            value = fieldConfigurationManager.GetFieldConfigurationIdByName(facItem.InternalName, value, facItem.ListName);

                            if (!string.IsNullOrEmpty(value))
                            {
                                listItem[facItem.InternalName] = UGITUtility.StringToLong(value);
                                //Set Division based on Studio.
                                if (facItem.InternalName == DatabaseObjects.Columns.StudioLookup && enableStudioDivisionHierarchy)
                                {
                                    long divisionID = studioManager.GetDivisionIdForStudio(UGITUtility.StringToLong(value));
                                    if (divisionID != 0 && listItem.Table.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                                    {
                                        listItem[DatabaseObjects.Columns.DivisionLookup] = divisionID;
                                        isDivisionAlreadySet = true;
                                    }
                                }
                            }
                            else
                                returnMsg = $"{returnMsg}<br>Invalid {field.FieldName} {value}";
                        }
                    }

                    else if (field.Datatype == "Percentage")
                    {
                        Double percentValue = 0;
                        Double.TryParse(value, out percentValue);
                        double PctValue = percentValue * 100;
                        // int pctComplete = 0;

                        listItem[facItem.InternalName] = Convert.ToInt32(PctValue);
                    }

                    else if (field.Datatype == "UserField")
                    {
                        if (field.Multi)
                        {
                            List<string> ccUserValueCollection = new List<string>();
                            string[] arrayUser = uHelper.GetValueByColumn(rowDate, facItem, lstColumn).Split(';');
                            foreach (string userName in arrayUser)
                            {
                                if (!string.IsNullOrWhiteSpace(userName))
                                {
                                    UserProfile user = context.UserManager.GetUserByBothUserNameandDisplayName(userName); //.FirstOrDefault(x => x.UserName == userName || x.Name == userName || x.LoginName == userName);

                                    //UserProfile user = context.UserManager.GetUserByUserName(userName); // Check existing users only, do not create!
                                    if (user != null)
                                    {
                                        ccUserValueCollection.Add(user.Id);
                                    }
                                }
                            }
                            if (ccUserValueCollection != null && ccUserValueCollection.Count() > 0)
                                listItem[facItem.InternalName] = string.Join(" ,", ccUserValueCollection);
                        }
                        else
                        {
                            string userName = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (!string.IsNullOrWhiteSpace(userName))
                            {
                                UserProfile user = context.UserManager.GetUserByBothUserNameandDisplayName(userName); // Check existing users only, do not create!
                                if (user != null)
                                    listItem[facItem.InternalName] = user.Id;
                                else
                                    listItem[facItem.InternalName] = context.CurrentUser.Id;
                            }
                        }
                    }

                    else if (field.Datatype == "Date")
                    { 
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrEmpty(value))
                            listItem[facItem.InternalName] = UGITUtility.StringToDateTime(value);
                    }
                    else if (field.Datatype == "NoteField")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        listItem[facItem.InternalName] = value;
                    }
                    else if (field.Datatype == "Choices")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (string.IsNullOrEmpty(value))
                            continue;
                        string data = fieldConfigurationManager.GetFieldByFieldName(facItem.InternalName, moduleViewManager.GetByName(ModuleName).ModuleTable)?.Data;
                        List<string> choiceValues = UGITUtility.SplitString(data, Constants.Separator).ToList();
                        if (choiceValues.Contains(value))
                        {
                            listItem[facItem.InternalName] = value;
                        }
                        else
                            returnMsg = $"{returnMsg}<br>Invalid {field.FieldName} {value}";
                    }
                    else if (listItem.Table.Columns[facItem.InternalName].DataType == typeof(System.Boolean))
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        listItem[facItem.InternalName] = UGITUtility.StringToBoolean(value);
                    }
                    else
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        listItem[facItem.InternalName] = value;
                    }
                }
            }
            //set default columns
            listItem[DatabaseObjects.Columns.TenantID] = context.TenantID;
            listItem[DatabaseObjects.Columns.Created] = DateTime.Now;
            listItem[DatabaseObjects.Columns.Modified] = DateTime.Now;
            listItem[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
            listItem[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
            if(spField.ColumnName.EqualsIgnoreCase(DatabaseObjects.Columns.TicketId))
                listItem[DatabaseObjects.Columns.Closed] = listItem[DatabaseObjects.Columns.Closed].ToString() == "" ? 0 : listItem[DatabaseObjects.Columns.Closed];
            if (listItem.Table.Columns.Contains(DatabaseObjects.Columns.BudgetCategoryLookup))
                listItem[DatabaseObjects.Columns.BudgetCategoryLookup] = 0;
            if (listItem.Table.Columns.Contains(DatabaseObjects.Columns.Id) && listItem.Table.Columns[DatabaseObjects.Columns.Id].DataType.FullName != "System.Int64")
                listItem[DatabaseObjects.Columns.Id] = Guid.NewGuid();

            return returnMsg;
        }

        public static void SetFilledValues(ApplicationContext context, List<string> lstColumn, Row rowDate, DataRow listItem, List<FieldAliasCollection> lstFieldAliasCollection, List<TicketColumnValue> ctrValues, Dictionary<string, DataTable> lookupLists = null, string moduleName = "")
        {
            UserProfileManager userManager = new UserProfileManager(context);
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            FieldConfiguration fieldConfiguration = null;
            foreach (string ColItem in lstColumn)
            {
                string columnName = ColItem.ToLower().Trim();
                FieldAliasCollection facItem = lstFieldAliasCollection.Where(x => x.InternalName == ColItem || x.AliasNames.ToLower().Split(',').Contains(columnName)).FirstOrDefault();
                if (facItem == null)
                {
                    if (UGITUtility.IfColumnExists(columnName, listItem.Table))
                        facItem = new FieldAliasCollection(listItem.Table.TableName, columnName, columnName); // Create a dummy entry so we can use internal name to access
                    else
                        continue;
                }

                DataColumn spField = listItem.Table.Columns[facItem.InternalName];
                //if (spField == null || spField.ColumnName.Equals("ContentType") || spField.ColumnName.Equals("Attachments") || !spField.ColumnName.Equals(DatabaseObjects.Columns.SubLocationTagLookup))
                //    continue;
                if (spField != null)
                {
                    if (spField.ColumnName.Equals("ContentType") || spField.ColumnName.Equals("Attachments"))
                        continue;
                }

                TicketColumnValue rctrValues = new TicketColumnValue();
                fieldConfiguration = fieldConfigurationManager.GetFieldByFieldName(facItem.InternalName);
                if (fieldConfiguration != null && fieldConfiguration.Datatype == "UserField")
                {
                    if (fieldConfiguration.Multi)
                    {
                        List<string> ccUserValueCollection = new List<string>();

                        string[] arrayUser = uHelper.GetValueByColumn(rowDate, facItem, lstColumn).Split(','); //(rowDate[indexData].DisplayText).Split(',');
                        foreach (string userName in arrayUser)
                        {
                            if (!string.IsNullOrWhiteSpace(userName))
                            {
                                //UserProfile user = userManager.GetUserByUserName(userName); // Check existing users only, do not create!
                                UserProfile user = userManager.GetUserInfoByIdOrName(userName);
                                if (user != null)
                                {
                                    ccUserValueCollection.Add(user.Id);
                                }
                                else
                                {
                                    UserProfile group = userManager.GetUserByUserName(userName);
                                    if (group != null)
                                        ccUserValueCollection.Add(group.Id);
                                }
                            }
                        }
                        if (ccUserValueCollection != null && ccUserValueCollection.Count > 0)
                        {
                            try
                            {
                                rctrValues.DisplayName = facItem.AliasNames;
                                rctrValues.InternalFieldName = facItem.InternalName;
                                rctrValues.Value = ccUserValueCollection;
                                ctrValues.Add(rctrValues);
                                listItem[facItem.InternalName] = rctrValues.Value;
                            }
                            catch (Exception) { }
                        }
                    }
                    else
                    {
                        string userName = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrWhiteSpace(userName))
                        {
                            //UserProfile user = context.UserManager.GetUserByUserName(userName); // Check existing users only, do not create!
                            UserProfile user = userManager.GetUserInfoByIdOrName(userName);
                            if (user != null)
                            {
                                rctrValues.DisplayName = facItem.AliasNames;
                                rctrValues.InternalFieldName = facItem.InternalName;
                                rctrValues.Value = user.Id;
                                ctrValues.Add(rctrValues);
                                listItem[facItem.InternalName] = rctrValues.Value;
                            }
                        }
                    }
                }
                else if (fieldConfiguration != null && fieldConfiguration.Datatype == "Lookup")
                {
                    List<string> valcoll = new List<string>();
                    DataTable lookupList = new DataTable();
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, fieldConfiguration.ParentTableName))
                        lookupList = GetTableDataManager.GetTableData(fieldConfiguration.ParentTableName, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{moduleName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    else
                        lookupList = GetTableDataManager.GetTableData(fieldConfiguration.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                    string lookupListName = fieldConfiguration.ParentTableName;
                    string elxFieldValue = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                    bool isMultiple = fieldConfiguration.Multi;
                    if (!string.IsNullOrWhiteSpace(elxFieldValue))
                    {
                        // Get lookup data for this field from saved values if present
                        DataTable lookupData = null;
                        if (lookupLists != null)
                            lookupData = lookupLists.FirstOrDefault(x => x.Key == lookupList.TableName).Value;

                        valcoll = GetLookValueFromExcel(lookupData, lookupList, listItem, fieldConfiguration.FieldName, valcoll, elxFieldValue);

                        if (!isMultiple && (valcoll == null || valcoll.Count == 0))
                        {
                            // Don't have this lookup list in memory, so run a query to translate value to ID 
                            string querylookUpFiled = "";
                            if (fieldConfiguration.FieldName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                                fieldConfiguration.ParentFieldName = DatabaseObjects.Columns.RequestType;
                            if (!string.IsNullOrWhiteSpace(moduleName) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, lookupList))
                                querylookUpFiled = string.Format("{0}='{1}' and {2}='{3}'", fieldConfiguration.ParentFieldName, elxFieldValue, DatabaseObjects.Columns.ModuleNameLookup, moduleName);
                            else
                                querylookUpFiled = string.Format("{0}='{1}'", fieldConfiguration.ParentFieldName, elxFieldValue);

                            DataRow[] collection = GetTableDataManager.GetTableData(lookupListName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(querylookUpFiled);
                            if (collection != null && collection.Count() > 0)
                            {

                                valcoll.Add(UGITUtility.ObjectToString(collection[0]["ID"]));
                            }
                            string LookupId = "";
                            // If we are importing into Assets list, and the field in AssetModelLookup, AND its a new model then add it to the SPList
                            if ((valcoll == null || valcoll.Count == 0) && listItem.Table.TableName == DatabaseObjects.Tables.Assets && spField.ColumnName == DatabaseObjects.Columns.AssetModelLookup &&
                                UGITUtility.IfColumnExists(spField.ColumnName, listItem.Table))
                            {
                                // Update vendor lookup if vendor is exist in imported file and it is also exist in database
                                FieldAliasCollection vendorFacItem = lstFieldAliasCollection.Where(x => x.AliasNames.ToLower().Split(',').Contains(DatabaseObjects.Columns.VendorLookup)).FirstOrDefault();
                                if (vendorFacItem != null)
                                {
                                    string elxVendorValue = uHelper.GetValueByColumn(rowDate, vendorFacItem, lstColumn);
                                    if (!string.IsNullOrWhiteSpace(elxVendorValue))
                                    {
                                        DataTable vendorData = null;
                                        if (lookupLists != null)
                                            vendorData = lookupLists.FirstOrDefault(x => x.Key == DatabaseObjects.Tables.AssetVendors).Value;

                                        if (vendorData != null)
                                        {
                                            DataRow rData = vendorData.AsEnumerable().FirstOrDefault(x => !x.IsNull(fieldConfiguration.FieldName) && x.Field<string>(fieldConfiguration.FieldName).ToLower() == elxVendorValue.ToLower());
                                            if (rData != null)
                                                LookupId = UGITUtility.ObjectToString(rData[DatabaseObjects.Columns.Id]);
                                        }
                                        else
                                        {
                                            string vquery = "";
                                            vquery = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, elxVendorValue);
                                            DataRow[] vColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AssetVendors, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(vquery);
                                            if (vColl != null && vColl.Count() > 0)
                                            {
                                                LookupId = UGITUtility.ObjectToString(vColl[0]["ID"]);
                                            }
                                        }
                                    }
                                }

                                ULog.WriteLog("Found new asset model, adding to list: " + elxFieldValue);

                                // Create new asset model entry in SPList
                                DataRow newItem = lookupList.NewRow();
                                newItem[DatabaseObjects.Columns.ModelName] = newItem[DatabaseObjects.Columns.Title] = elxFieldValue;
                                if (!string.IsNullOrEmpty(LookupId))
                                    newItem[DatabaseObjects.Columns.VendorLookup] = LookupId;
                                newItem.AcceptChanges();

                                // Add new entry to lookup data so we have it next time, else will keep adding same entry in DB each time :-)
                                if (lookupData != null)
                                {
                                    DataRow newLookupEntry = lookupData.NewRow();
                                    newLookupEntry[DatabaseObjects.Columns.Id] = newItem["ID"];
                                    newLookupEntry[DatabaseObjects.Columns.Title] = newLookupEntry[DatabaseObjects.Columns.ModelName] = elxFieldValue;
                                    lookupData.Rows.Add(newLookupEntry);
                                }

                                //newItem.Web.AllowUnsafeUpdates = allowunsafe;
                            }
                        }
                    }

                    if (valcoll != null && valcoll.Count > 0)
                    {
                        if (facItem.InternalName == DatabaseObjects.Columns.SubLocationTagLookup)
                        {
                            string subLocCol = DatabaseObjects.Columns.SubLocationLookup.ToLower();
                            FieldAliasCollection subLocationAlias = lstFieldAliasCollection.Where(x => x.InternalName == DatabaseObjects.Columns.SubLocationLookup || x.AliasNames.ToLower().Split(',').Contains(subLocCol)).FirstOrDefault();
                            rctrValues.DisplayName = subLocationAlias.AliasNames;
                            rctrValues.InternalFieldName = subLocationAlias.InternalName;
                            rctrValues.Value = valcoll[0];
                            ctrValues.Add(rctrValues);
                        }
                        else
                        {
                            rctrValues.DisplayName = facItem.AliasNames;
                            rctrValues.InternalFieldName = facItem.InternalName;
                            rctrValues.Value = valcoll[0];
                            if (isMultiple)
                                rctrValues.Value = valcoll;
                            ctrValues.Add(rctrValues);
                            listItem[facItem.InternalName] = rctrValues.Value;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(uHelper.GetValueByColumn(rowDate, facItem, lstColumn)))
                    {
                        rctrValues.DisplayName = facItem.AliasNames;
                        rctrValues.InternalFieldName = facItem.InternalName;
                        rctrValues.Value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        ctrValues.Add(rctrValues);
                        listItem[facItem.InternalName] = rctrValues.Value;
                    }
                }
            }

            //Default Columns
            ctrValues.Add(new TicketColumnValue()
            {
                DisplayName = DatabaseObjects.Columns.TenantID,
                InternalFieldName = DatabaseObjects.Columns.TenantID,
                Value = context.TenantID
            });
            ctrValues.Add(new TicketColumnValue()
            {
                DisplayName = DatabaseObjects.Columns.Created,
                InternalFieldName = DatabaseObjects.Columns.Created,
                Value = DateTime.Now
            });
            ctrValues.Add(new TicketColumnValue()
            {
                DisplayName = DatabaseObjects.Columns.Modified,
                InternalFieldName = DatabaseObjects.Columns.Modified,
                Value = DateTime.Now
            });
            ctrValues.Add(new TicketColumnValue()
            {
                DisplayName = DatabaseObjects.Columns.CreatedByUser,
                InternalFieldName = DatabaseObjects.Columns.CreatedByUser,
                Value = context.CurrentUser.Id
            });
            ctrValues.Add(new TicketColumnValue()
            {
                DisplayName = DatabaseObjects.Columns.ModifiedByUser,
                InternalFieldName = DatabaseObjects.Columns.ModifiedByUser,
                Value = context.CurrentUser.Id
            });
        }
        public static void SetFilledValuesFromDatarow(ApplicationContext context, List<string> lstColumn, DataRow rowDate, DataRow listItem, List<FieldAliasCollection> lstFieldAliasCollection)
        {
            //SPWeb oWeb = listItem.Web;
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            FieldConfiguration field = null;
            FieldAliasCollection facItem = null;
            DataColumn spField = null;
            foreach (string ColItem in lstColumn)
            {
                string value = string.Empty;
                string columnName = ColItem.ToLower();
                facItem = lstFieldAliasCollection.Where(x => x.InternalName == ColItem || x.AliasNames.ToLower().Split(',').Contains(columnName)).FirstOrDefault();
                if (facItem == null)
                {
                    if (UGITUtility.IfColumnExists(ColItem, listItem.Table))
                        facItem = new FieldAliasCollection(listItem.Table.TableName, ColItem, ColItem); // Create a dummy entry so we can use internal name to access
                    else
                        continue;
                }

                spField = listItem.Table.Columns[facItem.InternalName];
                if (spField != null)
                {
                    if (spField.ColumnName.Equals("ContentType") || spField.ColumnName.Equals("Attachments"))
                        continue;
                }

                field = fieldConfigurationManager.GetFieldByFieldName(facItem.InternalName);
                if (field == null)
                {
                    // "ProjectLifeCycleLookup"
                    if (ColItem == "Lifecycle" || facItem.InternalName == "ProjectLifeCycleLookup")
                    {
                        string lyfCycleName = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        LifeCycle lifeCycle = null;
                        List<LifeCycle> objLifeCycle = new List<LifeCycle>();
                        LifeCycleManager lifeCycleHelper = new LifeCycleManager(context);
                        LifeCycleStageManager objLifeCycleStageManager = new LifeCycleStageManager(context);
                        objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
                        var stageList = objLifeCycleStageManager.Load();
                        // Select the first LifeCycle from selected if exits otherwise load first lifecyle
                        if (!string.IsNullOrEmpty(lyfCycleName))
                        {
                            lifeCycle = objLifeCycle.FirstOrDefault(x => x.Name == lyfCycleName);
                            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                            {
                                listItem[DatabaseObjects.Columns.ModuleStepLookup] = null;
                                // listItem[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                                // listItem[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                                listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                                listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToInt64(lifeCycle.ID);

                            }
                            else if (lifeCycle == null)
                            {
                                // setting first lifecycle kind of default if 
                                lifeCycle = objLifeCycle.FirstOrDefault();

                                if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                                {
                                    //ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);

                                    listItem[DatabaseObjects.Columns.ModuleStepLookup] = lifeCycle.Stages[0].ID;
                                    listItem[DatabaseObjects.Columns.TicketStatus] = lifeCycle.Stages[0].Name;
                                    listItem[DatabaseObjects.Columns.StageStep] = lifeCycle.Stages[0].StageStep;
                                    listItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Format("{0}{1}{2}", DatabaseObjects.Columns.TicketProjectManager, Constants.Separator, objConfigurationVariableHelper.GetValue(ConfigConstants.PMOGroup), Constants.Separator, DatabaseObjects.Columns.TicketProjectCoordinators);
                                    listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToInt64(lifeCycle.ID);
                                }
                            }
                        }
                        else
                        {
                            listItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = DBNull.Value;
                        }

                        listItem[DatabaseObjects.Columns.ScrumLifeCycle] = false;      //keep false as default setting

                    }
                    else if (facItem.InternalName == "CloseDate")
                    {
                        if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.Closed) && UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.CloseDate) && !string.IsNullOrEmpty(Convert.ToString(uHelper.GetValueByColumn(rowDate, facItem, lstColumn))))
                        {
                            listItem[DatabaseObjects.Columns.Closed] = true;
                            listItem[DatabaseObjects.Columns.TicketStatus] = "Closed";
                        }

                        else
                        {
                            if (UGITUtility.IfColumnExists(listItem, DatabaseObjects.Columns.Closed))
                                listItem[DatabaseObjects.Columns.Closed] = false;
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            listItem[facItem.InternalName] = value.ToDateTime();

                        }

                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.BudgetCategoryLookup)
                        listItem[DatabaseObjects.Columns.BudgetCategoryLookup] = 0;
                    else if (facItem.InternalName == "ServiceWizardOnly" || facItem.InternalName == "StagingId")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                        {
                            listItem[facItem.InternalName] = value;
                        }
                    }
                    else if (facItem.InternalName == "TaskTemplateLookup" || facItem.InternalName == "TaskTemplateLookup")
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                            listItem[facItem.InternalName] = value;
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.AutoAssignPRP)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = DBNull.Value;
                        }
                        else
                            listItem[facItem.InternalName] = value;
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.SLADisabled || facItem.InternalName == DatabaseObjects.Columns.Use24x7Calendar
                        || facItem.InternalName == DatabaseObjects.Columns.OutOfOffice)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (value == "")
                        {
                            listItem[facItem.InternalName] = false;
                        }
                        else
                            listItem[facItem.InternalName] = UGITUtility.StringToBoolean(value);
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.Id)
                    {
                        listItem[facItem.InternalName] = Guid.NewGuid();
                    }
                    else if (facItem.InternalName == DatabaseObjects.Columns.EstimatedConstructionStart || facItem.InternalName == DatabaseObjects.Columns.EstimatedConstructionEnd)
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (!string.IsNullOrWhiteSpace(value))
                            listItem[facItem.InternalName] = UGITUtility.StringToDateTime(value);
                        else
                            listItem[facItem.InternalName] = UGITUtility.StringToDateTime(DateTime.Now);
                    }
                    else
                    {
                        value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                        if (listItem.Table.Columns[facItem.InternalName].DataType == typeof(System.Boolean))
                            listItem[facItem.InternalName] = UGITUtility.StringToBoolean(value);
                        else
                            listItem[facItem.InternalName] = value;
                    }

                }
                else
                {
                    if (field != null)
                    {

                        if (field.Datatype == "Lookup")
                        {

                            if (lstFieldAliasCollection[0].ListName == "RequestType" && facItem.InternalName != "ModuleNameLookup" && facItem.InternalName != "FunctionalAreaLookup" && facItem.InternalName != "APPTitleLookup" && facItem.InternalName != "BudgetIdLookup")
                            {
                                value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                                listItem[facItem.InternalName] = UGITUtility.StringToLong(value);
                            }
                            else if (facItem.InternalName == "ModuleNameLookup")
                            {

                                value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                                List<string> lstwords = UGITUtility.ConvertStringToList(value, ")");
                                //string[] words = value.Split(new[] { ")" }, StringSplitOptions.RemoveEmptyEntries);
                                List<string> lstwords1 = UGITUtility.ConvertStringToList(lstwords[0], "(");
                                //words = words[0].Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries);

                                listItem[facItem.InternalName] = UGITUtility.ObjectToString(lstwords1.FirstOrDefault());
                            }
                            else
                            {
                                value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                                value = fieldConfigurationManager.GetFieldConfigurationIdByName(facItem.InternalName, value, facItem.ListName);

                                if (!string.IsNullOrEmpty(value))
                                {
                                    listItem[facItem.InternalName] = UGITUtility.StringToLong(value);
                                }
                            }
                        }

                        else if (field.Datatype == "Percentage")
                        {
                            Double percentValue = 0;
                            Double.TryParse(value, out percentValue);
                            double PctValue = percentValue * 100;
                            // int pctComplete = 0;

                            listItem[facItem.InternalName] = Convert.ToInt32(PctValue);
                        }

                        else if (field.Datatype == "UserField")
                        {
                            if (field.Multi)
                            {
                                List<string> ccUserValueCollection = new List<string>();
                                string[] arrayUser = uHelper.GetValueByColumn(rowDate, facItem, lstColumn).Split(';');
                                foreach (string userName in arrayUser)
                                {
                                    if (!string.IsNullOrWhiteSpace(userName))
                                    {
                                        UserProfile user = context.UserManager.GetUserByBothUserNameandDisplayName(userName); //.FirstOrDefault(x => x.UserName == userName || x.Name == userName || x.LoginName == userName);

                                        //UserProfile user = context.UserManager.GetUserByUserName(userName); // Check existing users only, do not create!
                                        if (user != null)
                                        {
                                            ccUserValueCollection.Add(user.Id);
                                        }
                                        //else
                                        //{
                                        //    UserProfile group = context.UserManager.GetUserByUserName(userName);
                                        //    if (group != null)
                                        //        ccUserValueCollection.Add(group.Id);
                                        //}
                                    }
                                }
                                if (ccUserValueCollection != null && ccUserValueCollection.Count() > 0)
                                    listItem[facItem.InternalName] = string.Join(" ,", ccUserValueCollection);
                            }
                            else
                            {
                                string userName = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                                if (!string.IsNullOrWhiteSpace(userName))
                                {
                                    UserProfile user = context.UserManager.GetUserByBothUserNameandDisplayName(userName); // Check existing users only, do not create!
                                    if (user != null)
                                        listItem[facItem.InternalName] = user.Id;
                                    else
                                        listItem[facItem.InternalName] = context.CurrentUser.Id;
                                }
                            }
                        }
                        else if (field.Datatype == "Date")
                        {
                            value = uHelper.GetValueByColumn(rowDate, facItem, lstColumn);
                            if (!string.IsNullOrEmpty(value))
                                listItem[facItem.InternalName] = UGITUtility.StringToDateTime(value);
                        }

                    }

                }
            }
            //set default columns
            listItem[DatabaseObjects.Columns.TenantID] = context.TenantID;
            listItem[DatabaseObjects.Columns.Created] = DateTime.Now;
            listItem[DatabaseObjects.Columns.Modified] = DateTime.Now;
            listItem[DatabaseObjects.Columns.CreatedByUser] = context.CurrentUser.Id;
            listItem[DatabaseObjects.Columns.ModifiedByUser] = context.CurrentUser.Id;
            if (listItem.Table.Columns.Contains(DatabaseObjects.Columns.BudgetCategoryLookup))
                listItem[DatabaseObjects.Columns.BudgetCategoryLookup] = 0;
            if (listItem[DatabaseObjects.Columns.Closed] == null || listItem[DatabaseObjects.Columns.Closed] == DBNull.Value)
                listItem[DatabaseObjects.Columns.Closed] = false;
        }

        public static string GetTrimmedRowValue(DevExpress.Spreadsheet.Row row, int index)
        {
            string value = row[index] != null ? row[index].DisplayText : string.Empty;
            if (value != null)
                return value.Trim();

            return string.Empty;
        }
        public static string CreateAllocationBar(double pctAllocation)
        {
            StringBuilder bar = new StringBuilder();
            string progressBarClass = "progressbar";
            string emptyProgressBarClass = "emptyProgressBar";

            if (pctAllocation > 100.0f)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:1px;'>{2:0.##}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", emptyProgressBarClass, progressBarClass, pctAllocation);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:1px;'>{2:0.##}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", emptyProgressBarClass, progressBarClass, pctAllocation);
            }

            return bar.ToString();
        }
        //
        public static List<string> GetLookValueFromExcel(DataTable lookupData, DataTable lookupList, DataRow listItem, string lookupField, List<string> valcoll, string elxFieldValue)
        {
            if (lookupData != null && lookupData.Columns.Contains(lookupField))
            {
                List<string> lstelxFieldValue = elxFieldValue.Split(',').ToList();
                string lookVal = "";
                valcoll = new List<string>();
                DataRow rData = null;
                lstelxFieldValue.ForEach(pr =>
                {
                    rData = lookupData.AsEnumerable().FirstOrDefault(x => !x.IsNull(lookupField) && x.Field<string>(lookupField).ToLower() == pr.ToLower());
                    if (rData != null)
                        lookVal = UGITUtility.ObjectToString(rData[DatabaseObjects.Columns.Id]);
                    if (!string.IsNullOrEmpty(lookVal))
                        valcoll.Add(lookVal);
                });
            }

            return valcoll;
        }

        public static HyperLink GetHyperLinkControlForTicketID(ApplicationContext context, int moduleId, string ticketId)
        {
            return GetHyperLinkControlForTicketID(context, moduleId, ticketId, false);
        }

        public static HyperLink GetHyperLinkControlForTicketID(ApplicationContext context, int moduleId, string ticketId, bool inIframe)
        {
            TicketManager ticket = new TicketManager(context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            string title = string.Empty;
            DataRow row = null;
            if (moduleViewManager.GetByID(moduleId) != null)
            {
                DataTable ticketTable = ticket.GetOpenTickets(moduleViewManager.GetByID(moduleId));
                if (ticketTable != null)
                {
                    DataRow[] ticketRow = ticketTable.Select(string.Format("{0}= '{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                    if (ticketRow.Length > 0)
                    {
                        title = Convert.ToString(ticketRow[0][DatabaseObjects.Columns.Title]);
                    }
                }

                DataTable dataTable = UGITUtility.ObjectToData(moduleViewManager.GetByID(moduleId));
                if (dataTable != null && dataTable.Rows.Count > 0)
                    row = dataTable.Rows[0];
            }
            return GetHyperLinkControlForTicketID(row, ticketId, inIframe, title);
        }

        public static HyperLink GetHyperLinkControlForTicketID(DataRow moduleDetail, string ticketId, bool inIframe, string ticketTitle)
        {
            HyperLink lf = new HyperLink();
            lf.Text = ticketId;
            lf.ToolTip = ticketTitle;
            string navigationUrl = "javascript:";
            if (moduleDetail == null)
            {
                lf.NavigateUrl = navigationUrl;
                return lf;
            }

            string url = string.Empty;
            if (moduleDetail != null && moduleDetail[DatabaseObjects.Columns.StaticModulePagePath] != null)
            {
                url = Convert.ToString(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath]);
            }
            url = UGITUtility.GetAbsoluteURL(url);
            string prefix = "javascript:UgitOpenPopupDialog(";
            if (inIframe)
                prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
            navigationUrl = string.Format(prefix + "\"{0}\",\"TicketId={1}\",\"{1}: {2}\",\"90\",\"90\")",
                              url, ticketId, uHelper.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(ticketTitle, 100)));
            lf.NavigateUrl = navigationUrl;

            return lf;
        }

        public static HyperLink GetHyperLinkControlForTicketID(UGITModule moduleDetail, string ticketId, bool inIframe, string ticketTitle)
        {
            HyperLink lf = new HyperLink();
            lf.Text = ticketId;
            lf.ToolTip = ticketTitle;
            string navigationUrl = "javascript:";
            if (moduleDetail == null)
            {
                lf.NavigateUrl = navigationUrl;
                return lf;
            }

            string url = string.Empty;
            if (moduleDetail != null && moduleDetail.StaticModulePagePath != null)
            {
                url = Convert.ToString(moduleDetail.StaticModulePagePath);
            }
            url = UGITUtility.GetAbsoluteURL(url);
            string prefix = "javascript:UgitOpenPopupDialog(";
            if (inIframe)
                prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
            navigationUrl = string.Format(prefix + "\"{0}\",\"TicketId={1}\",\"{1}: {2}\",\"90\",\"90\")",
                              url, ticketId, uHelper.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(ticketTitle, 100)));
            lf.NavigateUrl = navigationUrl;

            return lf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public static Dictionary<string, DateTime> GetReportScheduleDates(ApplicationContext context, string dateRange)
        {
            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
            DateTime DateFrom = new DateTime();
            DateTime DateTo = new DateTime();
            string[] arrDateRange = dateRange.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
            if (arrDateRange.Length > 0)
            {
                string unit = arrDateRange[1];
                string[] arrDates = arrDateRange[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
                if (arrDates.Length > 0)
                {
                    int fromDays = 0;
                    int toDays = 0;
                    switch (unit)
                    {
                        case "Days":
                            {
                                fromDays = UGITUtility.StringToInt(arrDates[0]);
                                toDays = UGITUtility.StringToInt(arrDates[1]);
                                DateFrom = DateTime.Now.AddDays(UGITUtility.StringToInt(arrDates[0]));
                                DateTo = DateTime.Now.AddDays(UGITUtility.StringToInt(arrDates[1]));
                            }
                            break;
                        case "Weeks":
                            {
                                fromDays = (UGITUtility.StringToInt(arrDates[0]) * 7);
                                toDays = (UGITUtility.StringToInt(arrDates[1]) * 7);
                                DateFrom = DateTime.Now.AddDays(fromDays);
                                DateTo = DateTime.Now.AddDays(toDays);
                            }
                            break;
                        case "Months":
                            {
                                fromDays = (UGITUtility.StringToInt(arrDates[0]));
                                toDays = (UGITUtility.StringToInt(arrDates[1]));
                                DateFrom = DateTime.Now.AddMonths(fromDays);
                                DateTo = DateTime.Now.AddMonths(toDays);
                            }
                            break;
                    }
                }
            }
            dic.Add("DateFrom", DateFrom);
            dic.Add("DateTo", DateTo);
            return dic;
        }

        public static string GetHealthIndicatorImageUrl(ApplicationContext context, double score)
        {
            string oapScoreImageUrl = string.Empty;
            double red = 50, yellow = 90;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            string overallProjectScoreColor = configurationVariableManager.GetValue(ConfigConstants.OverallProjectScoreColor);
            if (!string.IsNullOrEmpty(overallProjectScoreColor))
            {
                Dictionary<string, string> cpoverallProjectScoreColor = UGITUtility.GetCustomProperties(overallProjectScoreColor, Constants.Separator);

                if (cpoverallProjectScoreColor.ContainsKey(Constants.Red))
                {
                    Double.TryParse(cpoverallProjectScoreColor[Constants.Red], out red);
                }
                if (cpoverallProjectScoreColor.ContainsKey(Constants.Yellow))
                {
                    Double.TryParse(cpoverallProjectScoreColor[Constants.Yellow], out yellow);
                }
            }
            if (score > red && score <= yellow)
            {
                oapScoreImageUrl = GetHealthIndicatorImageUrl("Yellow");
            }
            else if (score <= red)
            {
                oapScoreImageUrl = GetHealthIndicatorImageUrl("Red");
            }
            else
            {
                oapScoreImageUrl = GetHealthIndicatorImageUrl("Green");
            }

            return oapScoreImageUrl;
        }

        public static string GetHealthIndicatorImageUrl(string className)
        {
            string fileName = string.Empty;
            switch (className)
            {
                case "Green":
                case "GreenLED":
                    fileName = "/Content/images/LED_Green.png";
                    break;
                case "Yellow":
                case "YellowLED":
                    fileName = "/Content/images/LED_Yellow.png";
                    break;
                case "Red":
                case "RedLED":
                    fileName = "/Content/images/LED_Red.png";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        public static string FormatRequestType(ApplicationContext context, DataRow ticketItem)
        {
            string value = string.Empty;
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeCategory, Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeCategory]));
            string category = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeCategory, Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeCategory]));//UGITUtility.GetLookupValue(ticketItem, DatabaseObjects.Columns.TicketRequestTypeCategory);
            string subCategory = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeSubCategory, Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory]));//uHelper.GetLookupValue(ticketItem, DatabaseObjects.Columns.TicketRequestTypeSubCategory);
            if (!string.IsNullOrWhiteSpace(category))
                value = category + " > ";
            if (!string.IsNullOrWhiteSpace(subCategory))
                value += subCategory + " > ";
            value += fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeLookup, Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]));// uHelper.GetLookupValue(ticketItem, DatabaseObjects.Columns.TicketRequestTypeLookup);
            return value;
        }

        public static DataTable SortCollection(DataTable dt, string sortedBy)
        {
            if (dt == null || dt.Rows.Count == 0 || !IfColumnExists(sortedBy, dt))
                return dt;

            DataView view = dt.DefaultView;
            view.Sort = string.Format("{0} asc", sortedBy);
            dt = view.ToTable(true);
            return dt;
        }

        public static bool IfColumnExists(string columnName, DataTable thisList)
        {
            if (thisList == null || string.IsNullOrEmpty(columnName))
                return false;

            return thisList.Columns.Contains(columnName);
        }

        public static string GetReportLogoURL(ApplicationContext applicationContext)
        {
            string logoUrl = applicationContext.ConfigManager.GetValue(ConfigConstants.ReportLogo);
            if (!string.IsNullOrEmpty(logoUrl))
            {
                try
                {
                    logoUrl = UGITUtility.GetAbsoluteURL(logoUrl);

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Cannot load Company Logo file");
                }
            }
            return logoUrl;
        }
        public static Tuple<string, System.Drawing.Image> GetReportLogo(ApplicationContext applicationContext)
        {
            Tuple<string, System.Drawing.Image> logo = null;
            // Get company logo to show on report header
            System.Drawing.Image img = null;
            // Get custom company logo if configured
            string logoUrl = applicationContext.ConfigManager.GetValue(ConfigConstants.ReportLogo);
            if (!string.IsNullOrEmpty(logoUrl))
            {
                try
                {
                    logoUrl = UGITUtility.GetAbsoluteURL(logoUrl);

                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Cannot load Company Logo file");
                    img = null;
                }
            }

            // If custom logo not configured OR cannot load custom logo, then default to standard uGovernIT logo
            if (img == null)
            {
                logoUrl = "/Content/images/uGovernIT_Logo.png";
                string fileName = logoUrl.Substring(logoUrl.LastIndexOf("/"));
                string filePath = uHelper.GetTempFolderPathNew(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath("TEMPLATE\\IMAGES\\uGovernIT\\", SPUtility.CompatibilityLevel15);
                try
                {
                    img = Bitmap.FromFile(filePath + fileName);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Cannot load Company Logo file");
                    img = null;
                }
            }

            if (img != null)
            {
                logo = new Tuple<string, System.Drawing.Image>(logoUrl, img);
            }

            return logo;
        }

        public static string GetBehaviourIcon(string behaviour)
        {
            if (string.IsNullOrEmpty(behaviour))
                return string.Empty;

            string fileName = string.Empty;
            switch (behaviour)
            {
                case Constants.TaskType.Milestone:
                    fileName = "/Content/images/uGovernIT/milestone_icon.png";
                    break;
                case Constants.TaskType.Deliverable:
                    fileName = "/Content/images/uGovernIT/document_down.png";
                    break;
                case Constants.TaskType.Receivable:
                    fileName = "/Content/images//uGovernIT/document_up.png";
                    break;
            }
            return fileName;
        }

        public static void DisplayHelpTextLink(ApplicationContext context, string moduleName, Control helpTextContainer, Control HelpTextNewTicket = null)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            if (!string.IsNullOrEmpty(moduleName))
            {
                UGITModule spListItem = moduleViewManager.GetByName(moduleName);
                string helpUrl = Convert.ToString(spListItem.NavigationUrl);

                if (!string.IsNullOrEmpty(helpUrl) && helpUrl != "\\")
                {
                    HyperLink helpTextImage = new HyperLink();
                    helpTextImage.ImageUrl = "~/content/images/help_22x22.png";
                    helpTextImage.CssClass = "ugit-helpicon";
                    helpTextImage.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    helpTextImage.ToolTip = "Help";

                    HyperLink helpTextNewTicketImage = new HyperLink();
                    helpTextNewTicketImage.ImageUrl = "~/content/images/help_22x22.png";
                    helpTextNewTicketImage.CssClass = "ugit-helpicon";
                    helpTextNewTicketImage.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    helpTextImage.ToolTip = "Help";

                    string absoluteUrl = UGITUtility.GetAbsoluteURL(helpUrl);
                    string Title = "Help Documentation";
                    helpTextImage.Attributes.Add("onclick", "UgitOpenPopupDialog('" + absoluteUrl + "' , '', '" + Title + "', '95', '100', 0, 0)");
                    helpTextNewTicketImage.Attributes.Add("onclick", "UgitOpenPopupDialog('" + absoluteUrl + "' , '', '" + Title + "', '1000px', '100', 0, 0)");
                    if (spListItem.NavigationType.EqualsIgnoreCase("HelpCard"))
                    {
                        helpTextImage.Attributes.Add("onclick", string.Format(@"javascript:openHelpCard('{0}','{1}')", helpUrl, moduleName));
                        helpTextNewTicketImage.Attributes.Add("onclick", string.Format(@"javascript:openHelpCard('{0}','{1}')", helpUrl, moduleName));

                    }
                    if (HelpTextNewTicket != null)
                    {
                        HelpTextNewTicket.Controls.Add(helpTextNewTicketImage);
                        HelpTextNewTicket.Visible = true;
                    }

                    helpTextContainer.Controls.Add(helpTextImage);
                    helpTextContainer.Visible = true;
                }
                else
                {
                    helpTextContainer.Visible = false;
                }
            }
        }

        public static void DisplayHelpTextLink(ApplicationContext context, Control helpTextContainer)
        {
            string helpUrl = context.ConfigManager.GetValue("HomePageHelpUrl");

            if (!string.IsNullOrEmpty(helpUrl))
            {
                HyperLink helpTextImage = new HyperLink();
                helpTextImage.ImageUrl = "~/Content/images/uGovernIT/help_22x22.png";
                helpTextImage.CssClass = "ugit-helpicon";
                helpTextImage.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                helpTextImage.ToolTip = "Help";

                string absoluteUrl = UGITUtility.GetAbsoluteURL(helpUrl);
                string Title = "Help Documentation";
                helpTextImage.Attributes.Add("onclick", "UgitOpenPopupDialog('" + absoluteUrl + "' , '', '" + Title + "', '1000px', '90', 0, 0)");
                helpTextContainer.Controls.Add(helpTextImage);
                helpTextContainer.Visible = true;
            }
        }

        public static DataTable ConvertTableLookupValues(ApplicationContext context, DataTable table)
        {
            //DataTable aResultTable = table.Clone();
            DataTable aResultTable;
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            FieldConfiguration field;
            ModuleColumn moduleColumn = new ModuleColumn();
            List<UserProfile> listUser = null;
            DataRow newRowobj = null;
            if (table == null)
            {
                aResultTable = new DataTable();
                return aResultTable;
            }
            else
            {
                aResultTable = table.Clone();
            }

            foreach (DataColumn col in aResultTable.Columns)
                aResultTable.Columns[col.ColumnName].DataType = typeof(string);

            foreach (DataRow row in table.Rows)
            {
                newRowobj = aResultTable.NewRow();
                foreach (DataColumn col in table.Columns)
                {
                    if (row[col.ColumnName] == DBNull.Value)
                        continue;

                    newRowobj[col.ColumnName] = Convert.ToString(row[col.ColumnName]);

                    field = fieldManager.GetFieldByFieldName(col.ColumnName, string.Empty);

                    if (field != null && field.Datatype == "Lookup")
                    {
                        string value = Convert.ToString(row[col.ColumnName]);

                        if (!string.IsNullOrEmpty(value))
                        {
                            moduleColumn.ColumnType = "Lookup";
                            string lookupValue = fieldManager.GetFieldConfigurationData(field, value, null, null, moduleColumn);
                            if (!string.IsNullOrEmpty(lookupValue))
                            {
                                newRowobj[col.ColumnName] = lookupValue;
                            }
                        }
                    }
                    if (field != null && field.Datatype == "UserField")
                    {
                        string value = Convert.ToString(row[col.ColumnName]);

                        if (!string.IsNullOrEmpty(value))
                        {
                            listUser = context.UserManager.GetUserInfosById(value);
                            if (listUser != null && listUser.Count > 0)
                            {
                                string userValue = string.Join($"{Constants.Separator6} ", listUser.Select(x => x.Name));
                                newRowobj[col.ColumnName] = userValue;
                            }
                            else
                                newRowobj[col.ColumnName] = string.Empty;
                        }
                    }
                    if (field != null && field.Datatype == "Date")
                    {
                        string value = Convert.ToString(row[col.ColumnName]);
                        if (!string.IsNullOrEmpty(value))
                        {
                            newRowobj[col.ColumnName] = string.Format("{0:MMM-dd-yyyy}", value.ToDateTime());
                        }
                    }
                }
                aResultTable.Rows.Add(newRowobj);
            }

            return aResultTable;
        }

        public static DataTable ExcludeIsPrivateMarked(ApplicationContext context, DataTable inputDt, UserProfile currentUser)
        {
            UserProfileManager userManager = new UserProfileManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            TicketManager ticketManager = new TicketManager(context);
            DataTable _DataTable = inputDt;
            if (_DataTable != null && _DataTable.Rows.Count > 0 && _DataTable.Columns.Contains(DatabaseObjects.Columns.IsPrivate) && !userManager.IsUGITSuperAdmin(currentUser))
            {
                DataTable dt = null;
                EnumerableRowCollection<DataRow> dc = _DataTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.IsPrivate) == "1" || x.Field<string>(DatabaseObjects.Columns.IsPrivate) == "True");
                if (dc != null && dc.Count() > 0)
                    dt = dc.CopyToDataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                        string ticketModuleName = uHelper.getModuleNameByTicketId(ticketId);
                        if (string.IsNullOrEmpty(ticketModuleName))
                            continue; // Should never happen!!


                        UGITModule ugitModule = moduleManager.LoadByName(ticketModuleName);  // uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, ticketModuleName);
                        DataRow currentTicket = null;
                        if (ugitModule != null)
                        {
                            List<string> userTypeColumnColl = ugitModule.List_ModuleUserTypes.Select(x => x.FieldName).ToList();
                            userTypeColumnColl.Add(DatabaseObjects.Columns.IsPrivate);
                            currentTicket = ticketManager.GetByTicketID(ugitModule, ticketId);  //.getCurrentTicket(ticketModuleName, ticketId, viewFields: userTypeColumnColl, viewFieldsOnly: true);
                            if (currentTicket != null && !userManager.IsUserPresentInModuleUserTypes(currentUser, currentTicket, ticketModuleName))
                            {
                                // Hide if ticket marked private (and user is not named user)
                                if (uHelper.IfColumnExists(DatabaseObjects.Columns.IsPrivate, currentTicket.Table) && UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsPrivate]))
                                {
                                    _DataTable.AsEnumerable().FirstOrDefault(s => s.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId).Delete();
                                    _DataTable.AcceptChanges(); // Need this otherwise get crash in global search
                                }
                            }
                        }
                        else
                        {
                            // Should never get here!!!
                            currentTicket = ticketManager.GetByTicketID(ugitModule, ticketId);
                            if (currentTicket != null && !userManager.IsUserPresentInModuleUserTypes(currentUser, currentTicket, ticketModuleName))
                            {
                                // Hide if ticket marked private (and user is not named user)
                                if (uHelper.IfColumnExists(DatabaseObjects.Columns.IsPrivate, currentTicket.Table) && UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsPrivate]))
                                {
                                    _DataTable.AsEnumerable().FirstOrDefault(s => s.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId).Delete();
                                    _DataTable.AcceptChanges();
                                }
                            }
                        }
                    }
                }
            }
            return _DataTable;
        }

        public static DataTable GetSLAAndTabularDashboardData(ApplicationContext context, string moduleName, List<Tuple<string, DateTime, DateTime>> lstFilter, string dUnit = "d", bool includeOpen = false)
        {
            DataTable slaTable = uHelper.GetClosedTicketSLASummaryData(context, moduleName, lstFilter, includeOpen: includeOpen);
            DataTable timeLineData = new DataTable();
            timeLineData.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            timeLineData.Columns.Add("SLATargetX1", typeof(double));
            timeLineData.Columns.Add("SLATargetX2", typeof(double));
            timeLineData.Columns.Add("SLAActualX1", typeof(double));
            timeLineData.Columns.Add("SLAActualX2", typeof(double));
            timeLineData.Columns.Add(DatabaseObjects.Columns.StartStageStep, typeof(int));
            timeLineData.Columns.Add(DatabaseObjects.Columns.EndStageStep, typeof(int));
            timeLineData.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));

            if (slaTable != null && slaTable.Rows.Count > 0)
            {
                var slaGroupby = slaTable.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SLARuleName));
                int workingHoursInDay = uHelper.GetWorkingHoursInADay(context);
                foreach (var slatitle in slaGroupby)
                {

                    DataRow[] dr = slatitle.ToArray();
                    DataRow drInnerFirst = dr.FirstOrDefault();
                    double averageOfTargetDays = dr.Average(x => x.Field<int>(DatabaseObjects.Columns.TargetTime));
                    double averageOfActualDays = dr.Average(x => x.Field<int>(DatabaseObjects.Columns.ActualTime));

                    DataRow slarow = timeLineData.NewRow();
                    if (dUnit.Equals("h"))
                    {
                        averageOfTargetDays = (averageOfTargetDays / 60);
                        averageOfActualDays = (averageOfActualDays / 60);
                    }
                    else
                    {
                        averageOfTargetDays = (averageOfTargetDays / 60) / workingHoursInDay;
                        averageOfActualDays = (averageOfActualDays / 60) / workingHoursInDay;
                    }

                    slarow[DatabaseObjects.Columns.Title] = slatitle.Key;
                    slarow["SLATargetX1"] = 0;
                    slarow["SLATargetX2"] = Math.Round(averageOfTargetDays, 2);
                    slarow["SLAActualX1"] = 0;
                    slarow["SLAActualX2"] = Math.Round(averageOfActualDays, 2);
                    slarow[DatabaseObjects.Columns.StartStageStep] = drInnerFirst[DatabaseObjects.Columns.StartStageStep];
                    slarow[DatabaseObjects.Columns.EndStageStep] = drInnerFirst[DatabaseObjects.Columns.EndStageStep];
                    slarow[DatabaseObjects.Columns.ModuleNameLookup] = drInnerFirst[DatabaseObjects.Columns.ModuleNameLookup];
                    timeLineData.Rows.Add(slarow);
                }
            }


            if (timeLineData != null)
            {
                timeLineData.DefaultView.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.StartStageStep, DatabaseObjects.Columns.EndStageStep);
                timeLineData = timeLineData.DefaultView.ToTable(true);
            }
            return timeLineData;
        }

        public static DataTable GetClosedTicketSLASummaryData(ApplicationContext context, string moduleName, List<Tuple<string, DateTime, DateTime>> lstFilter, string slaName = null, bool includeOpen = false, string criteria = "SLARuleName", int ruleLookupId = 0, int svcId = 0)
        {
            //return if module name is not exist
            if (string.IsNullOrEmpty(moduleName))
            {
                return null;
            }

            SlaRulesManager slaMgr = new SlaRulesManager(context);

            DataTable slaTable = new DataTable();
            DataTable icSPList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketWorkflowSLASummary, $" {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            StringBuilder sbQuery = new StringBuilder();


            List<string> queryExps = new List<string>();
            queryExps.Add($"{DatabaseObjects.Columns.ModuleNameLookup} = '{moduleName}'");

            if (svcId > 0)
                queryExps.Add($"and {DatabaseObjects.Columns.ServiceTitleLookup} = {svcId}");

            if (criteria.Equals(DatabaseObjects.Columns.RuleNameLookup) && ruleLookupId > 0)
            {
                ModuleSLARule slaRule = slaMgr.LoadByID(ruleLookupId);
                if (slaRule != null)
                    slaName = Convert.ToString(slaRule.Title);
            }

            if (slaName != null)
            {
                queryExps.Add($" and ({DatabaseObjects.Columns.SLARuleName}='{slaName}' or {DatabaseObjects.Columns.RuleNameLookup} = {ruleLookupId})");
            }

            DateTime cStartDate = DateTime.MinValue, cEndDate = DateTime.MaxValue;
            DateTime comStartDate = DateTime.MinValue, comEndDate = DateTime.MaxValue;

            if (lstFilter != null)
            {
                Tuple<string, DateTime, DateTime> tup = lstFilter.AsEnumerable().FirstOrDefault(x => x.Item1.Equals("CreatedOn"));
                if (tup != null)
                {
                    cStartDate = tup.Item2;
                    cEndDate = tup.Item3;
                }

                tup = lstFilter.AsEnumerable().FirstOrDefault(x => x.Item1.Equals("CompletedOn"));
                if (tup != null)
                {
                    comStartDate = tup.Item2;
                    comEndDate = tup.Item3;
                }
            }

            if (cStartDate != DateTime.MinValue && cStartDate != DateTime.MaxValue)
                queryExps.Add($" and {DatabaseObjects.Columns.Created} >= '{cStartDate.ToString("yyyy-MM-dd")}'");

            if (cEndDate != DateTime.MinValue && cEndDate != DateTime.MaxValue)
                queryExps.Add($" and {DatabaseObjects.Columns.Created} <= '{cEndDate.ToString("yyyy-MM-dd")}'");

            queryExps.Add($" and {DatabaseObjects.Columns.StageStartDate} is not null"); // used to filter out cancelled requests        

            if (!includeOpen)
            {
                queryExps.Add($" and {DatabaseObjects.Columns.TicketClosed} = true");

                if (comStartDate != DateTime.MinValue && comStartDate != DateTime.MaxValue)
                    queryExps.Add($" and {DatabaseObjects.Columns.TicketCloseDate} >= '{comStartDate.ToString("yyyy-MM-dd")}'");

                if (comEndDate != DateTime.MinValue && comEndDate != DateTime.MaxValue)
                    queryExps.Add($" and {DatabaseObjects.Columns.TicketCloseDate} <= '{comEndDate.ToString("yyyy-MM-dd")}'");
            }

            if (queryExps.Count > 0)
            {
                foreach (var item in queryExps)
                {
                    sbQuery.Append(item);
                }
            }

            DataRow[] dr = icSPList.Select(Convert.ToString(sbQuery));

            if (dr.Count() > 0)
                slaTable = dr.CopyToDataTable();

            if (slaTable == null || slaTable.Rows.Count == 0)
                return null;

            return slaTable;
        }

        public static string GetFromEmailId(ApplicationContext context, bool SendMailFromLoggedInUser = false)
        {
            if (SendMailFromLoggedInUser)
            {
                return context.CurrentUser.Email;
            }
            else
            {
                SmtpConfiguration smtpSettings = context.ConfigManager.GetValueAsClassObj(ConfigConstants.SmtpCredentials, typeof(SmtpConfiguration)) as SmtpConfiguration;
                if (smtpSettings != null && !string.IsNullOrEmpty(smtpSettings.SmtpFrom))
                {
                    return smtpSettings.SmtpFrom;
                }
            }

            // need to fix
            return string.Empty;
        }

        /// <summary>
        /// Gets the ID of the post back control.
        /// 
        /// </summary>
        /// <param name = "page">The page.</param>
        /// <returns></returns>
        public static string GetPostBackControlId(Page page)
        {
            if (!page.IsPostBack)
                return string.Empty;

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                // ReSharper disable TooWideLocalVariableScope
                string controlId;
                Control foundControl;
                // ReSharper restore TooWideLocalVariableScope

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        /// <summary>
        /// Get week start date (Monday) corresponding to any date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetWeekStartDate(DateTime date)
        {
            date = date.Date;
            return date.AddDays(1 - (date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek));
        }

        /// <summary>
        /// Get month start date corresponding to any date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMonthStartDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static string CreateApplicationAccessTable(List<ServiceMatrixData> serviceMatrixDataList, ApplicationContext context)
        {
            if (serviceMatrixDataList == null || serviceMatrixDataList.Count == 0)
                return string.Empty;

            string html = string.Empty;
            UserProfileManager UserManager = new UserProfileManager(context);
            UserProfile user = UserManager.GetUserById(serviceMatrixDataList[0].RoleAssignee);

            if (user != null)
                html += string.Format("Update access for user: <b>{0}</b> ({1}) as noted below.<br/>", user.Name, user.LoginName.Split('|').LastOrDefault());

            foreach (ServiceMatrixData serviceMatrixData in serviceMatrixDataList)
            {
                if (serviceMatrixData.lstGridData.Count == 0)
                    continue; // No changes for this application!

                List<ServiceData> newAccess = serviceMatrixData.lstGridData.Where(p => p.Value == "True").ToList();
                List<ServiceData> removeAccess = serviceMatrixData.lstGridData.Where(p => p.Value == "False").ToList();

                List<Tuple<string, string, int>> lstAccessChanges = new List<Tuple<string, string, int>>();
                foreach (ServiceData item in newAccess)
                    lstAccessChanges.Add(new Tuple<string, string, int>(item.RowName, item.ColumnName, 1));
                foreach (ServiceData item in removeAccess)
                    lstAccessChanges.Add(new Tuple<string, string, int>(item.RowName, item.ColumnName, 2));

                if (lstAccessChanges.Count > 0)
                    html += "<hr />Application: <b>" + serviceMatrixData.Name + "</b><br /><br />" + CreateAccessChangesDescription(lstAccessChanges) + "<br />";
            }
            html += "<hr />";

            return html;
        }

        public static string CreateAccessChangesDescription(List<Tuple<string, string, int>> lstAccessChanges)
        {
            string html = "";
            if (lstAccessChanges != null && lstAccessChanges.Count > 0)
            {
                var lstAccess = lstAccessChanges.GroupBy(z => z.Item3).OrderBy(x => x.Key);
                List<string> readData = new List<string>();
                foreach (var ctr in lstAccess)
                {
                    StringBuilder str = new StringBuilder();
                    if (ctr.Key == 1)
                        str.Append("<b>ADD</b> access to");
                    else if (ctr.Key == 2)
                        str.Append("<b>REMOVE</b> access from");
                    else if (ctr.Key == 3)
                        str.Append("<b>NO CHANGE</b> in access for");

                    var moduleGroups = ctr.ToArray().GroupBy(x => x.Item2).OrderBy(x => x.Key);
                    if (moduleGroups.Count() > 1)
                        str.Append(":<br />");
                    foreach (var mg in moduleGroups)
                    {
                        str.AppendFormat("&nbsp;&nbsp;<b>{0}:</b> as {1}<br />", mg.Key, string.Join(", ", mg.ToArray().Select(x => x.Item1)));
                    }
                    readData.Add(str.ToString());
                }
                html = string.Join("<br/>", readData);
            }
            return html;
        }

        /// <summary>
        /// Check if the request type tied to the ticket uses 24x7 calendar
        /// </summary>
        /// <param name="ticketItem"></param>
        /// <returns></returns>
        public static bool IsTicket24x7Enabled(ApplicationContext context, DataRow ticketItem)
        {
            string modulename = uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(ticketItem[DatabaseObjects.Columns.TicketId]));
            if (modulename == ModuleNames.SVC)
            {
                // SVC Ticket: get boolean from service configuration
                //SPFieldLookupValue serviceLookup = new SPFieldLookupValue(uHelper.GetSPItemValueAsString(ticketItem, DatabaseObjects.Columns.ServiceTitleLookup));
                long servicelookup = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.ServiceTitleLookup]);
                if (servicelookup > 0)
                {

                    //SPQuery sQuery = new SPQuery();
                    //sQuery.ViewFields = string.Format("<FieldRef Name= '{0}' />", DatabaseObjects.Columns.Use24x7Calendar);
                    //sQuery.ViewFieldsOnly = true;
                    //sQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Number'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, serviceLookup.LookupId);
                    //SPListItemCollection resultCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.Services, sQuery, ticketItem.Web);
                    //if (resultCollection != null && resultCollection.Count > 0)
                    //{
                    //    SPListItem serviceItem = resultCollection[0];
                    //    return uHelper.StringToBoolean(serviceItem[DatabaseObjects.Columns.Use24x7Calendar]);
                    //}
                }
            }
            else
            {
                // TSR or other ticket: get boolean from request type configuration
                RequestTypeManager requestTypeManager = new RequestTypeManager(context);

                long ticketReqTypeLookupValue = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                if (ticketReqTypeLookupValue > 0)
                {
                    // Get from cache if available
                    ModuleRequestType requestType = requestTypeManager.LoadByID(ticketReqTypeLookupValue);  // uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.Id, ticketReqTypeLookupValue.LookupId);
                    if (requestType != null)
                        return UGITUtility.StringToBoolean(requestType.Use24x7Calendar);
                }
            }

            return false;
        }

        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public static string GetDateStringInFormat(ApplicationContext context, DateTime datetime, bool includeTime)
        {
            string defaultdateFormat = "MMM d, yyyy";
            string dateFormat = context.ConfigManager.GetValue(ConfigConstants.uGovernITDateFormat);
            if (!string.IsNullOrEmpty(dateFormat))
                defaultdateFormat = dateFormat;
            if (includeTime)
            {
                return datetime.ToString(defaultdateFormat + " hh:mm tt");
            }
            else
            {
                return datetime.ToString(defaultdateFormat);
            }
        }

        public static DataTable GetAllocationData(ApplicationContext context)
        {
            DateTime dateFrom;
            DateTime dateTo;
            UserProfileManager ObjUserProfileManager;
            //UserProfile CurrentUser;
            String DisplayMode;
            ResourceAllocationManager ResourceAllocationManager = new ResourceAllocationManager(context);
            ResourceWorkItemsManager ResourceWorkItemsManager = new ResourceWorkItemsManager(context);

            DisplayMode = "Monthly";
            ObjUserProfileManager = context.UserManager;
            string syear = DateTime.Now.Year.ToString();
            dateFrom = new DateTime(UGITUtility.StringToInt(syear), 1, 1);
            dateTo = dateFrom.AddMonths(12);

            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add("id", typeof(int));
            data.Columns.Add("parent", typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add("text", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Project, typeof(string));
            data.Columns.Add("start_date", typeof(DateTime));
            data.Columns.Add("end_date", typeof(DateTime));
            data.Columns.Add("AllocationType", typeof(string));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add("ProjectManager", typeof(string));
            data.Columns.Add("APM", typeof(string));
            data.Columns.Add("Estimator", typeof(string));
            data.Columns.Add("Supritendent", typeof(string));
            data.Columns.Add("ProjectExecutive", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add("duration", typeof(string));
            data.Columns.Add("ProjectStage", typeof(string));
            data.Columns.Add("color", typeof(string));

            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));



            int IDCounter = 0;
            //int ParentIDCounter = 0;
            //bool containsModules = false;

            //List<UserProfile> lstUProfile = new List<UserProfile>();

            //lstUProfile = ObjUserProfileManager.GetUsersProfile();


            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();


            dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
            workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 1);
            dtResult = dtResult.Rows.Cast<DataRow>().Take(100).CopyToDataTable();

            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (DisplayMode == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView(context, dateFrom, dateTo);
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView(context, dateFrom, dateTo);

            if (dtResult == null)
                return data;

            ILookup<object, DataRow> dtAllocLookups = null;
            ILookup<object, DataRow> dtWeeklyLookups = null;
            //Grouping on AllocationMonthly datatable based on ResourceWorkItemLookup
            if (dtAllocationMonthly != null && dtAllocationMonthly.Rows.Count > 0)
                dtAllocLookups = dtAllocationMonthly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.ResourceWorkItemLookup]);
            //Grouping on AllocationWeekly datatable based on WorkItemID
            if (dtAllocationWeekly != null && dtAllocationWeekly.Rows.Count > 0)
                dtWeeklyLookups = dtAllocationWeekly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.WorkItemID]);

            Dictionary<string, DataRow> tempTicketCollection = new Dictionary<string, DataRow>();

            LifeCycleManager lifecycleManager = new LifeCycleManager(context);
            List<LifeCycle> lifecyels = lifecycleManager.LoadLifeCycleByModule(ModuleNames.CPR);
            int resolvedStageStep = 0;
            string resolvedStageName;
            if (lifecyels != null && lifecyels.Count > 0)
            {
                if (lifecyels[0].Stages != null && lifecyels[0].Stages.Count > 0)
                {
                    LifeCycleStage stage = lifecyels[0].Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved");
                    if (stage != null)
                    {
                        resolvedStageName = stage.StageTitle;
                        resolvedStageStep = stage.StageStep;
                    }
                }
            }
            #region data creating
            foreach (DataRow dr in dtResult.Rows)
            {
                string userid = Convert.ToString(dr[DatabaseObjects.Columns.Resource]);
                if (string.IsNullOrEmpty(userid))
                    continue;

                UserProfile userDetails = ObjUserProfileManager.LoadById(userid);
                if (userDetails == null || !userDetails.Enabled)
                    continue;

                if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup])))
                    continue;

                DataRow newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userDetails.Name;
                newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr[DatabaseObjects.Columns.Resource]);
                newRow["id"] = ++IDCounter;
                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    // drWorkItem = workitems.AsEnumerable().FirstOrDefault(x => x.Field<Int32>(DatabaseObjects.Columns.Id) == uHelper.GetLookupID(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup])));
                    DataRow[] filterworkitemrow = workitems.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, UGITUtility.GetLookupID(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]))));
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }

                if (drWorkItem != null && drWorkItem[DatabaseObjects.Columns.WorkItem] != null)
                {
                    string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                    string[] arrayModule = { "CPR", "OPM" };   // selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    string moduleName = uHelper.getModuleNameByTicketId(workItem);
                    TicketManager ticketManager = new TicketManager(context);
                    ModuleViewManager moduleManager = new ModuleViewManager(context);

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        if (arrayModule.Contains(moduleName) || arrayModule.Length == 0)
                        {

                            UGITModule module = moduleManager.GetByName(moduleName);
                            if (module == null)
                                continue;

                            //check for active modules.
                            if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                                continue;
                            DataRow dataRow = null;

                            dataRow = ticketManager.GetByTicketID(module, workItem);

                            if (dataRow != null)
                            {
                                string ticketID = workItem;
                                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 50);
                                string pm = uHelper.GetUserNameBasedOnId(context, Convert.ToString(dataRow["ProjectManager"]));
                                string apm = uHelper.GetUserNameBasedOnId(context, Convert.ToString(dataRow["APM"]));
                                string estimator = uHelper.GetUserNameBasedOnId(context, Convert.ToString(dataRow["Estimator"]));
                                string super = uHelper.GetUserNameBasedOnId(context, Convert.ToString(dataRow["Superintendent"]));
                                string px = uHelper.GetUserNameBasedOnId(context, Convert.ToString(dataRow["ProjectExecutive"]));
                                string duration = UGITUtility.ObjectToString(dataRow[DatabaseObjects.Columns.CRMDuration]);
                                int stagestep = UGITUtility.StringToInt(dataRow[DatabaseObjects.Columns.StageStep]);
                                string stagename = string.Empty;

                                if (stagestep < resolvedStageStep)
                                    newRow["color"] = "blue";  //pre construction
                                else
                                    newRow["color"] = "orange"; // in progress
                                if (module.ModuleName == ModuleNames.OPM)
                                    newRow["color"] = "red";


                                if (!string.IsNullOrEmpty(ticketID))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", ticketID, title);
                                    newRow["text"] = string.Format("{0}", title);
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0}: {1}", ticketID, title);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;
                                    newRow["text"] = title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow["ProjectStage"] = stagestep;
                                newRow["ProjectExecutive"] = px;
                                newRow["ProjectStage"] = stagename;
                                newRow["ProjectManager"] = pm;
                                newRow["APM"] = apm;
                                newRow["Estimator"] = estimator;
                                newRow["Supritendent"] = super;
                                newRow["duration"] = duration;
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;

                                //condition for add new column for breakup gantt chart...
                                if (data != null && data.Rows.Count > 0)
                                {
                                    string expression = string.Format("{0}= '{1}' AND {2}='{3}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid);
                                    DataRow[] row = data.Select(expression);
                                    if (row != null && row.Count() > 0)
                                    {

                                        if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                        {

                                            if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate])))
                                                row[0]["start_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]).ToShortDateString();


                                            if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate])))
                                                row[0]["end_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]).ToShortDateString();
                                        }
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                        {
                                            newRow["start_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]).ToShortDateString();
                                            newRow["end_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate]).ToShortDateString();
                                        }

                                        data.Rows.Add(newRow);
                                    }
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                    {
                                        newRow["start_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]).ToShortDateString();
                                        newRow["end_date"] = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate]).ToShortDateString(); ;
                                    }


                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;

                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }

                }
            }
            //create child parent relationship
            {
                //DataTable groupTable = data.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Resource))
                //    .Select(y =>
                //    {
                //        var row = data.NewRow();
                //        row["Resource"] = y.Min(r => r.Field<string>("Resource"));
                //        row["parent"] = y.Min(r => r.Field<int>("id"));
                //        return row;
                //    }).CopyToDataTable();

                //var col = data.Columns["parent"];
                //var colResource = data.Columns["Resource"];
                //foreach (DataRow row in data.Rows)
                //{
                //    object parentvalue = groupTable.Select("Resource = '" + Convert.ToString(row[colResource]) + "'")[0]["parent"];
                //    if (Convert.ToInt32(row["id"]) == Convert.ToInt32(parentvalue))
                //        row[col] = 0;
                //    else
                //        row[col] = groupTable.Select("Resource = '" + Convert.ToString(row[colResource]) + "'")[0]["parent"];
                //}
            }
            #endregion
            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC", DatabaseObjects.Columns.Resource, DatabaseObjects.Columns.Project);
            data = data.DefaultView.ToTable();

            return data;
        }

        public static int GetDaysForDisplayMode(string dMode, DateTime dt)
        {
            int days = 30;
            switch (dMode)
            {
                case "Daily":
                    days = 1;
                    break;
                case "Weekly":
                    {
                        if (dt.ToString("ddd") == "Mon")
                            days = 7;
                        else if (dt.ToString("ddd") == "Tue")
                            days = 6;
                        else if (dt.ToString("ddd") == "Wed")
                            days = 5;
                        else if (dt.ToString("ddd") == "Thu")
                            days = 4;
                        else if (dt.ToString("ddd") == "Fri")
                            days = 3;
                        else if (dt.ToString("ddd") == "Sat")
                            days = 2;
                        else if (dt.ToString("ddd") == "Sun")
                            days = 1;

                        break;
                    }
                case "Monthly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(1)) - dt).Days;
                    break;
                case "Quarterly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(3)) - dt).Days;
                    break;
                case "HalfYearly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(6)) - dt).Days;
                    break;
                case "Yearly":
                    days = 365;
                    break;
                default:
                    break;
            }
            return days;
        }

        public static DataTable LoadAllocationWeeklySummaryView(ApplicationContext context, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string commQuery = string.Empty;
                ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, dateFrom, dateTo);

                List<ResourceUsageSummaryWeekWise> allocWeekly = allocationWeekWiseManager.Load(x => x.WeekStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.WeekStartDate.Value.Date <= Convert.ToDateTime(dateTo));
                DataTable dtAllocationWeekWise = UGITUtility.ToDataTable<ResourceUsageSummaryWeekWise>(allocWeekly);
                return dtAllocationWeekWise;
            }
            catch (Exception)
            { }
            return null;
        }
        public static DataTable LoadAllocationMonthlyView(ApplicationContext context, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
                List<ResourceAllocationMonthly> allocMonthly = allocationMonthlyManager.Load(x => x.MonthStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.MonthStartDate.Value.Date <= Convert.ToDateTime(dateTo)).ToList();
                DataTable dtAllocationMonthWise = UGITUtility.ToDataTable<ResourceAllocationMonthly>(allocMonthly);
                return dtAllocationMonthWise;
            }
            catch (Exception)
            { }
            return null;
        }

        public static void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, bool Assigned = true)
        {
            string DisplayMode = "Monthly";
            double yearquaAllocE = 0;
            double yearquaAllocA = 0;

            double halfyearquaAllocE1 = 0;
            double halfyearquaAllocE2 = 0;
            double halfyearquaAllocA1 = 0;
            double halfyearquaAllocA2 = 0;

            double quaterquaAllocE1 = 0;
            double quaterquaAllocE2 = 0;
            double quaterquaAllocE3 = 0;
            double quaterquaAllocE4 = 0;
            double quaterquaAllocA1 = 0;
            double quaterquaAllocA2 = 0;
            double quaterquaAllocA3 = 0;
            double quaterquaAllocA4 = 0;

            string hndYear = "2019";


            foreach (DataRow rowitem in dttemp)
            {
                if (DisplayMode == "Yearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        yearquaAllocE += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                        yearquaAllocA += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                    }

                    DateTime yearColumn = new DateTime(UGITUtility.StringToInt(hndYear), 1, 1);
                    if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(yearColumn).ToString("MMM-dd-yy") + "E"] = Math.Round(yearquaAllocE, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(yearColumn).ToString("MMM-dd-yy") + "A"] = Math.Round(yearquaAllocA, 2);
                        }
                    }
                }
                else if (DisplayMode == "HalfYearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            halfyearquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            halfyearquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }

                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6)
                        {
                            halfyearquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            halfyearquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                    }

                    DateTime halfyearColumn1 = new DateTime(UGITUtility.StringToInt(hndYear), 1, 1);
                    DateTime halfyearColumn2 = new DateTime(UGITUtility.StringToInt(hndYear), 7, 1);
                    if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE2, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA2, 2);
                        }
                    }

                }
                else if (DisplayMode == "Quarterly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 4)
                        {
                            quaterquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            quaterquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 3 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            quaterquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            quaterquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 10)
                        {
                            quaterquaAllocE3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            quaterquaAllocA3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                        else
                        {
                            quaterquaAllocE4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                            quaterquaAllocA4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                    }

                    DateTime quaterColumn1 = new DateTime(UGITUtility.StringToInt(hndYear), 1, 1);
                    DateTime quaterColumn2 = new DateTime(UGITUtility.StringToInt(hndYear), 4, 1);
                    DateTime quaterColumn3 = new DateTime(UGITUtility.StringToInt(hndYear), 7, 1);
                    DateTime quaterColumn4 = new DateTime(UGITUtility.StringToInt(hndYear), 10, 1);

                    if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE2, 2);
                    }

                    if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn3).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE3, 2);
                    }

                    if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn4).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE4, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA2, 2);
                        }

                        if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn3).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA3, 2);
                        }

                        if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn4).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA4, 2);
                        }
                    }

                }
                else if (DisplayMode == "Weekly")
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }
                }
                else
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Adds numBusinessDays to startDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numBusinessDays"></param>
        /// <param name="spWeb"></param>
        /// <returns>New Date</returns>
        public static DateTime AddWorkingDays(DateTime startDate, int numBusinessDays, ApplicationContext context)
        {
            int workingHoursInADay = GetWorkingHoursInADay(context, false);
            int addMinutes = numBusinessDays * workingHoursInADay * 60;
            DateTime endDate = GetWorkingEndDate(context, startDate, addMinutes);
            return endDate;
        }


        public static DataRow[] GetMultipleLookupValueExistData(DataTable table, string columnName, string value)
        {
            return GetMultipleLookupValueExistData(table, columnName, value, Constants.Separator6);
        }

        public static DataRow[] GetMultipleLookupValueExistData(DataTable table, string columnName, string value, string separator)
        {
            DataRow[] rows = new DataRow[0];
            if (table != null && table.Rows.Count > 0)
            {
                DataTable table1 = table.Copy();
                table1.Columns.Add(string.Format("{0}", columnName));
                table1.Columns[string.Format("{0}", columnName)].Expression = string.Format("'{0}'", separator, columnName);
                DataRow[] panels = table1.Select(string.Format("{0} LIKE '%{1}%'", columnName, value.Replace("'", "''")));
                if (panels.Length > 0)
                {
                    rows = panels;
                }
            }
            return rows;
        }

        public static DataRow[] GetMultipleLookupMultiValueExistData(DataTable table, string columnName, List<string> values, string separator)
        {
            DataRow[] rows = new DataRow[0];
            if (table != null && table.Rows.Count > 0)
            {
                DataTable table1 = table.Copy();
                //table1.Columns.Add(string.Format("{0}", columnName));
                //table1.Columns[string.Format("{0}", columnName)].Expression = string.Format("'{0}'", separator, columnName);

                string exp = string.Join(" Or ", values.Select(x => string.Format("{0} LIKE '%{1}%'", columnName, x.Replace("'", "''"))));
                DataRow[] panels = table1.Select(exp);
                if (panels.Length > 0)
                {
                    rows = panels;
                }
                table1.Columns.Remove(string.Format("{0}", columnName));
            }


            return rows;
        }

        public static List<HistoryEntry> GetHistorywithusername(DataRow ticket, string colName, bool oldestFirst)
        {
            List<HistoryEntry> dataList = new List<HistoryEntry>();
            List<string> versionEntries = new List<string>();
            string rowValue = ticket[colName].ToString();
            if (ticket.HasVersion(DataRowVersion.Current))
            {
                string rawData = Convert.ToString(ticket[colName]);
                if (rawData != string.Empty)
                {
                    string[] versionsDelim = { Constants.SeparatorForVersions };
                    string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string version in versions)
                    {
                        if (versionEntries.Contains(version))
                            continue;
                        versionEntries.Add(version);
                        string[] versionDelim = { Constants.Separator };
                        string[] versionData = version.Split(versionDelim, StringSplitOptions.None);
                        HistoryEntry entry = new HistoryEntry();
                        DateTime createdDate;
                        if (versionData.GetLength(0) == 3 || versionData.GetLength(0) == 4)
                        {
                            entry.createdBy = uHelper.GetUserNameBasedOnId(versionData[0]);
                            if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                            {
                                entry.created = versionData[1].Substring(Constants.UTCPrefix.Length);
                            }
                            else
                            {
                                DateTime.TryParse(versionData[1], out createdDate);
                                entry.created = createdDate.ToString("MMM-d-yyyy hh:mm tt");
                            }
                            entry.entry = versionData[2].Replace("\r\n", "<br>");
                            if (versionData.GetLength(0) == 4)
                                entry.IsPrivate = UGITUtility.StringToBoolean(versionData[3]);
                        }
                        else
                        {
                            entry.entry = version.Replace("\r\n", "<br>");
                        }
                        dataList.Add(entry);
                    }
                }

                if (oldestFirst == false)
                    dataList.Reverse();
            }

            return dataList;
        }
        public static string GetUserNameBasedOnId(string userid)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(userid))
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, DatabaseObjects.Columns.ID + "='" + userid + "'").Select();
                if (drModules != null && drModules.Length > 0)
                {
                    result = Convert.ToString(drModules[0][DatabaseObjects.Columns.Name]);
                }
                return result;
            }
            return result;
        }

        public static string GetComments(List<HistoryEntry> dataList)
        {
            string strComment = string.Empty;
            foreach (var item in dataList)
            {
                if (!string.IsNullOrEmpty(strComment))
                    strComment += Constants.SeparatorForVersions;
                strComment += item.createdBy + Constants.Separator + item.created + Constants.Separator + item.entry;
            }
            return strComment;
        }
        public static string GetTitlebyToken(List<List<string>> tokenArray)
        {
            string Title = string.Empty;
            if (tokenArray != null && tokenArray.Count() > 0)
            {
                List<string> moduleToken = tokenArray.FirstOrDefault(x => x[0].ToLower() == string.Format("<${0}", DatabaseObjects.Columns.Title.ToLower()));
                if (moduleToken != null && moduleToken.Count > 1)
                {
                    Title = moduleToken[1];
                    Title = Title.Replace("$>", string.Empty);
                }
            }
            return Title;
        }
        public static string GetTicketbyToken(List<List<string>> tokenArray)
        {
            string TicketId = string.Empty;
            if (tokenArray != null && tokenArray.Count() > 0)
            {
                List<string> TicketIdToken = tokenArray.FirstOrDefault(x => x[0].ToLower() == string.Format("<${0}", DatabaseObjects.Columns.TicketId.ToLower()));
                if (TicketIdToken != null && TicketIdToken.Count > 1)
                {
                    TicketId = TicketIdToken[1];
                    TicketId = TicketId.Replace("$>", string.Empty);
                }
            }
            return TicketId;
        }
        public static string GetRoleNameBasedOnId(string userid)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(userid))
            {
                DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetRoles, DatabaseObjects.Columns.ID + "='" + userid + "'").Select();
                if (drModules != null && drModules.Length > 0)
                {
                    result = Convert.ToString(drModules[0][DatabaseObjects.Columns.Name]);
                }
                return result;
            }
            return result;
        }

        public static void GetHoldUnHoldSlots(ApplicationContext applicationContext, string TaskId, string TicketId, ref List<Tuple<string, DateTime, DateTime>> slots)
        {
            if (string.IsNullOrEmpty(TaskId))
                return;

            string queryExpress = string.Empty;
            //$"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'"
            queryExpress = $"{DatabaseObjects.Columns.SubTaskId}='{TaskId}' and {DatabaseObjects.Columns.TicketId}='{TicketId}' and {DatabaseObjects.Columns.TicketEventType} in {string.Format("('{0}','{1}','{2}')", "Hold", "Hold Removed", "Hold Expired")} order by {DatabaseObjects.Columns.TicketEventTime} asc";
            string viewFields = string.Join(Constants.Separator6, new List<string>() { DatabaseObjects.Columns.TicketEventTime, DatabaseObjects.Columns.TicketEventType });
            DataTable eventCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketEvents, queryExpress, viewFields, string.Empty);

            //query.ViewFields = string.Format("<FieldRef Name='{0}' /><FieldRef Name='{1}' />", DatabaseObjects.Columns.TicketEventTime, DatabaseObjects.Columns.TicketEventType);
            //List<string> queryExpress = new List<string>();
            //queryExpress.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.SubTaskId, TaskId));
            //queryExpress.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketId, TicketId));
            //queryExpress.Add(string.Format("<In><FieldRef Name='{0}'/><Values><Value Type='Text'>Hold</Value><Value Type='Text'>Hold Removed</Value><Value Type='Text'>Hold Expired</Value></Values></In>", DatabaseObjects.Columns.TicketEventType));
            //query.Query = string.Format("<Where>{0}</Where><OrderBy><FieldRef Name='TicketEventTime' /></OrderBy>", uHelper.GenerateWhereQueryWithAndOr(queryExpress, true));
            //SPListItemCollection eventCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.TicketEvents, query, spweb);
            if (eventCollection != null && eventCollection.Rows.Count > 0)
            {
                DataTable dt = eventCollection;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string lookupid = Convert.ToString(TaskId);
                    bool findNextHold = false;
                    DateTime timestamp = DateTime.MinValue;
                    do
                    {
                        findNextHold = false;
                        DataRow holdRow = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketEventType) == "Hold" && x.Field<DateTime>(DatabaseObjects.Columns.TicketEventTime) > timestamp);
                        if (holdRow != null)
                        {
                            DataRow unHoldRow = dt.AsEnumerable().FirstOrDefault(x => (x.Field<string>(DatabaseObjects.Columns.TicketEventType) == "Hold Expired" || x.Field<string>(DatabaseObjects.Columns.TicketEventType) == "Hold Removed") && UGITUtility.StringToDateTime(x[DatabaseObjects.Columns.TicketEventTime]) > UGITUtility.StringToDateTime(timestamp));
                            if (unHoldRow != null)
                            {
                                //find next hold and unload row based on previous unhold event date. 
                                //because next hold and unhold must be greater then previous unhold time.
                                timestamp = UGITUtility.StringToDateTime(unHoldRow[DatabaseObjects.Columns.TicketEventTime]);
                                slots.Add(new Tuple<string, DateTime, DateTime>(lookupid, UGITUtility.StringToDateTime(holdRow[DatabaseObjects.Columns.TicketEventTime]), timestamp));

                                findNextHold = true;
                            }
                        }

                    } while (findNextHold);
                }
            }
        }

        public static double GetTotalDurationByTimeSlot(ApplicationContext applicationContext, List<Tuple<string, DateTime, DateTime>> slots, bool use24x7Calendar = false)
        {
            double totalHours = 0;
            if (slots == null || slots.Count <= 0)
                return 0;

            //remove overlap time slots between
            slots = slots.OrderBy(x => x.Item2).ThenBy(x => x.Item3).ToList();

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            // 10/1/2018 9:00AM  - 10/1/2018 5:00PM  8hr
            // first: 
            // previousStartDate = 10/1/2018 9:00AM, 
            // previousEndDate = 10/1/2018 5:00PM;
            // Hours: 8hr

            // 10/1/2018 12:00PM - 10/1/2018 2:00PM  2hr
            // Second:
            // previousStartDate = 10/1/2018 2:00PM, 
            // previousEndDate = 10/1/2018 5:00PM;
            // Hours: 0hr

            // 10/1/2018 2:00PM  - 10/1/2018 4:00PM  2hr
            // Third:
            // previousStartDate = 10/1/2018 4:00PM, 
            // previousEndDate = 10/1/2018 5:00PM;
            // Hours: 0hr 

            // 10/2/2018 9:00AM  - 10/2/2018 2:00PM  5hr
            // Forth:
            // previousStartDate = 10/2/2018 9:00AM, 
            // previousEndDate = 10/2/2018 2:00PM;
            // Hours: 5hr
            // Total service horus: 8+0+0+5= 13hr

            foreach (var item in slots)
            {
                DateTime currentStartDate = item.Item2;
                DateTime currentEndDate = item.Item3;

                if (startDate == DateTime.MinValue)
                {
                    //This is for first iteration only, it start with first element start and end date.
                    startDate = currentStartDate;
                    endDate = currentEndDate;
                }
                else if (currentStartDate >= endDate)
                {
                    //when current slot is not overriding with previous slot then it sets start and end date again
                    startDate = currentStartDate;
                    endDate = currentEndDate;
                }
                else if (currentStartDate < endDate)
                {
                    //when current slot overlapping with previous slot then it set dates accordingly.
                    startDate = endDate;
                    endDate = currentEndDate;
                }

                if (endDate >= startDate)
                    totalHours += GetWorkingMinutesBetweenDates(applicationContext, startDate, endDate, use24x7Calendar, isSLA: true);
                else
                {
                    //if end date is less then start date then reverse date assignments
                    DateTime endDate_old = endDate;
                    endDate = startDate;
                    startDate = endDate_old;
                }
            }

            return Math.Round(totalHours, 2);
        }
        /// <summary>
        /// Create formValue TicketColumnValue list for further operation
        /// </summary>
        /// <param name="listItem"></param>
        /// <param name="formValues"></param>
        public static void SetFormValues(DataRow listItem, List<TicketColumnValue> formValues)
        {
            TicketColumnValue colVal = null;
            foreach (DataColumn field in listItem.Table.Columns)
            {
                colVal = new TicketColumnValue();

                if (field == null || field.ColumnName.Equals("Attachments"))
                    continue;


                TicketColumnValue rctrValues = new TicketColumnValue();
                try
                {

                    rctrValues.DisplayName = field.Caption; // facItem.AliasNames;
                    rctrValues.InternalFieldName = field.ColumnName;
                    rctrValues.Value = UGITUtility.GetSPItemValue(listItem, field.ColumnName);
                    formValues.Add(rctrValues);
                }
                catch (Exception) { }
            }
        }
        public static string GetUserEmail(UserProfile user, ApplicationContext _context)
        {
            if (!user.isRole)
            {
                // Simple user, just return email
                return user.Email;
            }
            else
            {
                // Check if this is a group

                if (user != null)
                {
                    string userEmailList = string.Empty;
                    UserProfileManager userProfileManager = new UserProfileManager(_context);
                    List<UserProfile> userProfiles = userProfileManager.GetUsersByGroupID(user.Id);
                    foreach (UserProfile groupMember in userProfiles)
                    {
                        if (!string.IsNullOrEmpty(groupMember.Email))
                        {
                            if (userEmailList != string.Empty)
                                userEmailList += ";";
                            userEmailList += groupMember.Email;
                        }
                    }
                    return userEmailList;
                }
            }

            return string.Empty;
        }

        public static DateTime GetFiscalStartDateFromConfig(ApplicationContext context)
        {
            //Get Fiscal startdate from configuration
            //If its not exist then make default fiscal date (currentyear 1st april)
            DateTime fiscalStartDate;

            ConfigurationVariableManager configManagerObj = new ConfigurationVariableManager(context);
            DateTime.TryParse(configManagerObj.GetValue(Constants.FiscalStartDate), out fiscalStartDate);
            if (fiscalStartDate == DateTime.MinValue)
            {
                fiscalStartDate = new DateTime(DateTime.Now.Year - 1, 4, 1);
            }
            else
            {
                fiscalStartDate = new DateTime(DateTime.Now.Year - 1, fiscalStartDate.Month, 1);
            }

            return fiscalStartDate;
        }

        public static string GenerateGLCode(BudgetCategory budgetCategory, Department department, List<Company> companies, bool enableDivision)
        {
            StringBuilder glCode = new StringBuilder();
            if (department != null)
            {
                if (department.CompanyLookup != null && department.CompanyIdLookup > 0)
                {
                    Company cmp = companies.FirstOrDefault(x => x.ID == department.CompanyIdLookup);
                    if (cmp != null && !string.IsNullOrEmpty(cmp.GLCode))
                    {
                        glCode.Append(cmp.GLCode);
                    }
                }

                if (enableDivision && department.DivisionLookup != null)
                {
                    Company cmp = companies.FirstOrDefault(x => x.CompanyDivisions != null && x.CompanyDivisions.Exists(y => y.ID == department.DivisionIdLookup));
                    CompanyDivision division = null;
                    if (cmp != null)
                    {
                        division = cmp.CompanyDivisions.FirstOrDefault(x => x.ID == department.CompanyIdLookup);
                        if (division != null && division.GLCode != string.Empty)
                        {
                            if (glCode.ToString() != string.Empty)
                            {
                                glCode.Append("-");
                            }
                            glCode.Append(division.GLCode);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(department.GLCode))
                {
                    if (glCode.ToString() != string.Empty)
                    {
                        glCode.Append("-");
                    }
                    glCode.Append(department.GLCode);
                }
            }

            if (budgetCategory != null && !string.IsNullOrEmpty(budgetCategory.BudgetCOA))
            {
                if (glCode.ToString() != string.Empty)
                {
                    glCode.Append("-");
                }
                glCode.Append(budgetCategory.BudgetCOA);
            }

            return glCode.ToString();
        }

        public static bool IsDataAllSet(ApplicationContext context)
        {
            UserProfileManager userProfileManager = new UserProfileManager(context);
            bool result = true;
            if (!userProfileManager.IsAdmin(context.CurrentUser))
                return result;

            DepartmentManager deptManager = new DepartmentManager(context);
            List<Department> lstDeparments = deptManager.GetDepartmentData();
            if (lstDeparments == null || lstDeparments.Count <= 0)
                return false;

            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<GlobalRole> lstroles = roleManager.Load();
            if (lstroles == null || lstroles.Count <= 0)
                return false;

            JobTitleManager jobTitleManager = new JobTitleManager(context);
            List<JobTitle> lstJobTitles = jobTitleManager.Load();
            if (lstJobTitles == null || lstJobTitles.Count <= 0)
                return false;

            List<UserProfile> lstUsers = context.UserManager.GetUsersProfile();
            if (lstUsers == null || lstUsers.Count < 2)
                return false;


            return result;
        }

        public static string GetDefaultLandingPage(ApplicationContext context, UserProfile user, string param = "")
        {
            UserProfileManager userProfileManager = new UserProfileManager(context);
            DataTable dt = GetTableDataManager.GetTableData($"{DatabaseObjects.Tables.ConfigurationVariable}", $"{DatabaseObjects.Columns.KeyName}='ClientType'", $"{DatabaseObjects.Columns.KeyValue}", null);
            string url = "/Pages/UserHomePage";
            string ClientType = (dt != null && dt.Rows.Count > 0) ? UGITUtility.ObjectToString(dt.Rows[0][0]) : string.Empty;
            if (user != null && ClientType.EqualsIgnoreCase("GC"))
            {
                if (user.IsManager)
                    url = "/Pages/gcmanagerui";
                else if (userProfileManager.IsAdmin(user))
                    url = "/Admin/NewAdminUI.aspx";
                else
                    url = "/Pages/gcstaffui";

                if (!string.IsNullOrEmpty(param))
                    url = $"{url}?{param}";
            }

            return url;
        }
        public static DataTable GetMonthlyBudget(ApplicationContext context, DateTime startDate, DateTime endDate, int budgetSubCategory, BudgetType budgetType, int ticketId, uGovernIT.Utility.ColumnStyle columnStyle)
        {
            return GetMonthlyBudget(context, startDate, endDate, budgetSubCategory, budgetType, ticketId, columnStyle, 0);
        }
        public static DataTable GetMonthlyBudget(ApplicationContext context, DateTime startDate, DateTime endDate, int budgetSubCategory, BudgetType budgetType, int ticketId, uGovernIT.Utility.ColumnStyle columnStyle, double baselineDetailID)
        {
            #region Get Budget Table
            //SPQuery query = new SPQuery();
            string query = string.Empty;
            List<string> requiredQuery = new List<string>();
            if (budgetSubCategory != 0)
            {
                //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                //    DatabaseObjects.Columns.BudgetIdLookup, budgetSubCategory));

                requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.BudgetIdLookup, budgetSubCategory));

            }
            string targetListName = string.Empty;

            //Decide which Table to use based on the BudgetType passed and ticketId.
            switch (budgetType)
            {
                case BudgetType.NPR:
                    if (ticketId != 0)
                    {
                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                        //    DatabaseObjects.Columns.TicketNPRIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketNPRIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.NPR));
                    }
                    targetListName = DatabaseObjects.Tables.ModuleBudget;
                    break;
                case BudgetType.PMM:
                    if (ticketId != 0)
                    {
                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                        //    DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM));
                    }
                    targetListName = DatabaseObjects.Tables.ModuleBudget;
                    break;
                case BudgetType.ITG:
                    ModuleBudgetManager objbudgetmgr = new ModuleBudgetManager(context);
                    return objbudgetmgr.LoadBudgetSummary(startDate, endDate)[0];
                case BudgetType.PMMActuals:
                    if (ticketId != 0)
                    {
                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                        //    DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));

                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                        //    DatabaseObjects.Columns.Id, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM));
                    }
                    targetListName = DatabaseObjects.Tables.ModuleBudgetActuals;
                    break;
                case BudgetType.PMMBudgetBaseline:
                    if (ticketId != 0)
                    {
                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>",
                        //    DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        //requiredQuery.Add(string.Format(@"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Number'>{1}</Value></Eq>",
                        //  DatabaseObjects.Columns.BaselineNum, baselineDetailID));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.BaselineNum, baselineDetailID));
                        requiredQuery.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM));
                    }
                    targetListName = DatabaseObjects.Tables.PMMBudgetHistory;
                    break;
            }
            //query.Query = string.Format("<Where>{0}</Where>", UGITUtility.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
            query = UGITUtility.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true);
            //SPListItemCollection budgets = SPListHelper.GetSPList(targetListName).GetItems(query);
            DataTable budgets = GetTableDataManager.GetTableData(targetListName, query);
            #endregion

            #region Create Return table
            DataTable monthlyBudget = new DataTable();
            monthlyBudget.Columns.Add("Title", typeof(string));
            monthlyBudget.Columns.Add("Category", typeof(string));
            monthlyBudget.Columns.Add("Total", typeof(string));
            monthlyBudget.Columns.Add("BudgetDate", typeof(string));

            System.Collections.Generic.Dictionary<string, string> monthMap = new System.Collections.Generic.Dictionary<string, string>();

            DateTime tempReportStartDate = startDate.AddDays(-startDate.Day + 1);
            DateTime tempReportEndDate = endDate.AddDays(-endDate.Day + 1);
            int totalMonths = 0;

            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                monthMap.Add(tempReportStartDate.ToString("MMM") + tempReportStartDate.ToString("yy"), "Month" + totalMonths.ToString());
                monthlyBudget.Columns.Add("Month" + totalMonths, typeof(string));
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }
            #endregion

            #region Enter First Row telling the Month and Year (Jan '11, Feb '11)
            DataRow firstRow = monthlyBudget.NewRow();
            DataRow row = monthlyBudget.NewRow();
            firstRow["Category"] = "";
            firstRow["Title"] = "";
            firstRow["Total"] = "Total";
            tempReportStartDate = startDate.AddDays(-startDate.Day + 1);
            totalMonths = 0;
            while (tempReportStartDate <= tempReportEndDate)
            {
                totalMonths++;
                firstRow["Month" + totalMonths.ToString()] = tempReportStartDate.ToString("MMM") + " '" + tempReportStartDate.ToString("yy");
                row["Month" + totalMonths.ToString()] = 0;
                firstRow["BudgetDate"] += tempReportStartDate.ToShortDateString() + "#";
                tempReportStartDate = tempReportStartDate.AddMonths(1);
            }
            monthlyBudget.Rows.Add(firstRow);
            #endregion

            double totalBudgetForTheTimePeriod = 0;

            foreach (DataRow budget in budgets.Rows)
            {
                DateTime budgetStartDate = (DateTime)budget[DatabaseObjects.Columns.AllocationStartDate];
                DateTime budgetEndDate = (DateTime)budget[DatabaseObjects.Columns.AllocationEndDate];

                double totalBudget = float.Parse(budget[DatabaseObjects.Columns.BudgetAmount].ToString());
                double perDayAmount = CalculateDailyBudgetAmount(context, budgetStartDate, budgetEndDate, totalBudget);

                if (budgetStartDate < endDate && budgetEndDate > startDate)
                {
                    //The actual time period we need to distribute the budget for
                    DateTime higherStartDate = budgetStartDate > startDate ? budgetStartDate : startDate;
                    DateTime lowerEndDate = budgetEndDate > endDate ? endDate : budgetEndDate;

                    //Calculate total number of budget days in the first month and last month as they can start and end in the middle of the month

                    // int firstMonthDays = DateTime.DaysInMonth(higherStartDate.Year, higherStartDate.Month) - higherStartDate.Day + 1;
                    int firstMonthDays = uHelper.GetTotalWorkingDaysBetween(context, higherStartDate, new DateTime(higherStartDate.Year, higherStartDate.Month, DateTime.DaysInMonth(higherStartDate.Year, higherStartDate.Month)));
                    int lastMonthDays = 0;

                    if (higherStartDate.Year == lowerEndDate.Year && higherStartDate.Month == lowerEndDate.Month)
                    {
                        lastMonthDays = 0;
                        //firstMonthDays = lowerEndDate.Day - higherStartDate.Day + 1;
                        firstMonthDays = uHelper.GetTotalWorkingDaysBetween(context, higherStartDate, lowerEndDate);
                    }
                    else
                    {
                        lastMonthDays = uHelper.GetTotalWorkingDaysBetween(context, new DateTime(lowerEndDate.Year, lowerEndDate.Month, 1), lowerEndDate);
                        // lastMonthDays = lowerEndDate.Day;
                    }
                    //Add start and end month budget to the distribution
                    double firstMonthBudget = double.Parse(row[monthMap[higherStartDate.ToString("MMM") + higherStartDate.ToString("yy")]].ToString()) + perDayAmount * firstMonthDays;
                    row[monthMap[higherStartDate.ToString("MMM") + higherStartDate.ToString("yy")]] = Math.Round(firstMonthBudget, 2);

                    double lastMonthBudget = double.Parse(row[monthMap[lowerEndDate.ToString("MMM") + lowerEndDate.ToString("yy")]].ToString()) + perDayAmount * lastMonthDays;
                    row[monthMap[lowerEndDate.ToString("MMM") + lowerEndDate.ToString("yy")]] = Math.Round(lastMonthBudget, 2);

                    totalBudgetForTheTimePeriod += Math.Round(((perDayAmount * firstMonthDays) + (perDayAmount * lastMonthDays)), 2);

                    //Since we have already calculated the budget for the first and last month, recalculate the remaining time period
                    DateTime nextMonth = higherStartDate.AddMonths(1);
                    higherStartDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    DateTime lastMonthSubtraction = new DateTime(lowerEndDate.Year, lowerEndDate.Month, 1);
                    TimeSpan timeSpan = lowerEndDate - lastMonthSubtraction;
                    lowerEndDate = lowerEndDate.Subtract(timeSpan);

                    //Parse through the remaining months, if any.
                    while (lowerEndDate > higherStartDate)
                    {
                        double monthBudget = perDayAmount * uHelper.GetTotalWorkingDaysBetween(context, higherStartDate, new DateTime(higherStartDate.Year, higherStartDate.Month, DateTime.DaysInMonth(higherStartDate.Year, higherStartDate.Month)));
                        row[monthMap[higherStartDate.ToString("MMM") + higherStartDate.ToString("yy")]] = Math.Round(double.Parse(row[monthMap[higherStartDate.ToString("MMM") + higherStartDate.ToString("yy")]].ToString()) + monthBudget, 2);
                        higherStartDate = higherStartDate.AddMonths(1);
                        totalBudgetForTheTimePeriod += monthBudget;
                    }
                }
            }
            row["Total"] = Math.Round(totalBudgetForTheTimePeriod, 2);
            monthlyBudget.Rows.Add(row);
            return monthlyBudget;
        }

        public static Boolean IsTableExist(string etTable)
        {
            Boolean messageCode = false;

            SqlConnection conn = DBConnection.GetSqlConnection();
            SqlCommand commnd = null;
            try
            {
                commnd = new SqlCommand(string.Format(" select * from sysobjects where xtype = 'u' and name = '{0}'", etTable), conn);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(commnd);
                DataSet resultedSet = new DataSet();

                adapter.Fill(resultedSet);
                if (resultedSet != null && resultedSet.Tables[0].Rows.Count > 0)
                {
                    messageCode = true;
                }


            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (commnd != null)
                {
                    commnd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return messageCode;
        }

        public static DataTable GetDataTableFromExcel(string FilePath, string sheetName)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(FilePath);
            Worksheet worksheet = workbook.Worksheets[0];
            CellRange dataRange = worksheet.GetDataRange();
            DataTable data = worksheet.CreateDataTable(dataRange, true);

            CellRange range = workbook.Worksheets[0].GetDataRange();
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

        public static bool IsCPRModuleEnabled(ApplicationContext context)
        {
            bool result = false;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule module = moduleViewManager.LoadByName(ModuleNames.CPR);
            if (module != null && module.EnableModule)
                result = true;
            return result;
        }

        public static bool IsValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
        public static void SendJobStatusEmail(ApplicationContext _context, string jobName, ImportStatus status)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            string mailTo = configurationVariableManager.GetValue("JobStatusEmailTo");
            if (string.IsNullOrWhiteSpace(mailTo))
            {
                ULog.WriteLog("Skipping job status email since no To account configured!");
                return;
            }

            string subject = string.Format("{0} {1}", jobName, status.succeeded ? "completed successfully" : "FAILED!");
            string greeting = configurationVariableManager.GetValue("Greeting");
            string signature = configurationVariableManager.GetValue("Signature");

            StringBuilder body = new StringBuilder();
            body.AppendFormat("{0} {1},<br /><br />", greeting, mailTo);

            body.AppendFormat(subject);

            if (status.errorMessages.Count > 0)
                body.AppendFormat("<br /><br />{0}", string.Join<string>("<br />", status.errorMessages));

            body.AppendFormat("<br /><br />{0}<br />", signature);

            MailMessenger mail = new MailMessenger(_context);
            mail.SendMail(mailTo, subject, string.Empty, body.ToString(), true, false);
        }

        public static string GetHomeUrlByUser(ApplicationContext _context, UserProfile userProfile)
        {
            // Should default to null to prevent unnecessary redirects from uGovernITHomeUserControl
            string homePage = null;
            if (_context == null || userProfile == null)
                return homePage;

            // Redirect on the landing home page according to the user role.

            if (userProfile != null && !string.IsNullOrEmpty(userProfile.GlobalRoleId))
            {
                LandingPagesManager ObjLandingPagesManager = new LandingPagesManager(_context);
                LandingPages landingPages = new LandingPages();
                landingPages = ObjLandingPagesManager.GetUserRoleById(userProfile.GlobalRoleId);
                if (landingPages != null)
                    homePage = landingPages.LandingPage;
            }

            return homePage;
        }
        public static string GetFormattedHoldTime(ApplicationContext spWeb, DataRow ticket)
        {
            // Get total hold time in minutes
            double totalHoldMinutes = GetTotalHoldTime(ticket, false, spWeb);
            return GetFormattedHoldTime(totalHoldMinutes, spWeb);
        }
        public static double GetTotalHoldTime(DataRow ticket, bool inHours, ApplicationContext spWeb)
        {
            double totalHoldTime = 0;

            if (IfColumnExists(DatabaseObjects.Columns.TicketTotalHoldDuration, ticket.Table))
            {
                totalHoldTime = UGITUtility.StringToDouble(ticket[DatabaseObjects.Columns.TicketTotalHoldDuration]);

                if (UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.TicketOnHold]) && UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.TicketOnHoldStartDate))
                {
                    double currentHoldMinutes = GetWorkingMinutesBetweenDates(spWeb, UGITUtility.StringToDateTime(ticket[DatabaseObjects.Columns.TicketOnHoldStartDate]), DateTime.Now, isSLA: true);
                    totalHoldTime += currentHoldMinutes;
                }
            }

            if (inHours)
                return TimeSpan.FromMinutes(totalHoldTime).TotalHours;

            return totalHoldTime;
        }
        public static string GetFormattedHoldTime(DataRow dRow, ApplicationContext spWeb, string colNamePrefix = "")
        {
            // Get total hold time in minutes
            double totalHoldMinutes = GetTotalHoldTime(dRow, false, spWeb, colNamePrefix);
            return GetFormattedHoldTime(totalHoldMinutes, spWeb);
        }
        public static double GetTotalHoldTime(DataRow dRow, bool inHours, ApplicationContext spWeb, string colNamePrefix = "")
        {
            double totalHoldTime = 0;

            if (IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketTotalHoldDuration, dRow.Table))
            {
                totalHoldTime = UGITUtility.StringToDouble(dRow[colNamePrefix + DatabaseObjects.Columns.TicketTotalHoldDuration]);

                if (IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketOnHold, dRow.Table) && UGITUtility.StringToBoolean(dRow[colNamePrefix + DatabaseObjects.Columns.TicketOnHold]) && IfColumnExists(colNamePrefix + DatabaseObjects.Columns.TicketOnHoldStartDate, dRow.Table))
                {
                    double currentHoldMinutes = GetWorkingMinutesBetweenDates(spWeb, UGITUtility.StringToDateTime(dRow[colNamePrefix + DatabaseObjects.Columns.TicketOnHoldStartDate]), DateTime.Now, isSLA: true);
                    totalHoldTime += currentHoldMinutes;
                }
            }

            if (inHours)
                return TimeSpan.FromMinutes(totalHoldTime).TotalHours;

            return totalHoldTime;
        }
        /// <summary>
        /// Format hold time as days, hrs, minutes
        /// </summary>
        /// <param name="totalHoldMinutes">Total Hold time in minutes</param>
        /// <param name="spWeb"></param>
        /// <returns></returns>
        public static string GetFormattedHoldTime(double totalHoldMinutes, ApplicationContext spWeb)
        {
            if (totalHoldMinutes == 0)
                return "0";

            int workingMinutesInDay = GetWorkingHoursInADay(spWeb, true) * 60;

            TimeSpan timeSpan = TimeSpan.FromMinutes(totalHoldMinutes);

            int days = 0, hours = 0, minutes = 0;
            if (totalHoldMinutes < 60) // Less than 1 hour
                minutes = (int)totalHoldMinutes;
            else if (totalHoldMinutes < workingMinutesInDay) // Less than one day
            {
                hours = (int)Math.Floor(timeSpan.TotalHours);
                minutes = timeSpan.Minutes;
            }
            else // More than 1 day
            {
                days = (int)Math.Floor(totalHoldMinutes / workingMinutesInDay);
                hours = (int)Math.Floor((totalHoldMinutes % workingMinutesInDay) / 60);
                minutes = timeSpan.Minutes;
            }

            string output = "-";
            if (days > 0)
                output = string.Format("{0} {1}", days, days == 1 ? "day" : "days");
            if (hours > 0)
            {
                if (output == "-")
                    output = string.Format("{0} {1}", hours, hours == 1 ? "hr" : "hrs");
                else
                    output += string.Format(", {0} {1}", hours, hours == 1 ? "hr" : "hrs");
            }
            if (minutes > 0)
            {
                if (output == "-")
                    output = string.Format("{0} {1}", minutes, minutes == 1 ? "min" : "mins");
                else
                    output += string.Format(", {0} {1}", minutes, minutes == 1 ? "min" : "mins");
            }
            return output;
        }

        public static string GetGanttCellBackGroundColor(ApplicationContext context, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd, DateTime dt, DateTime? dtEnd = null, bool aSoftAllocation = false)
        {
            string backgroundColor = string.Empty;
            if (uHelper.IsCPRModuleEnabled(context))
            {
                dtEnd = dtEnd == null ? new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)) : dtEnd;
                ////classes to set color based on dates
                if (aSoftAllocation)
                    backgroundColor = "softallocationbgcolor";
                else if ((dt.Date >= aPreconStart.Date && dt.Date <= aPreconEnd.Date) && (dtEnd >= aConstStart.Date && dtEnd <= aConstEnd.Date))
                    backgroundColor = "preconbgcolor-constbgcolor";
                else if ((dt.Date >= aConstStart.Date && dt.Date <= aConstEnd.Date) && (aConstEnd.Date != DateTime.MinValue.Date && dtEnd > aConstEnd.Date))
                    backgroundColor = "constbgcolor-closeoutbgcolor";
                else if (dt.Date >= aPreconStart.Date && dt.Date <= aPreconEnd.Date)
                    backgroundColor = "preconbgcolor";
                else if (dt.Date >= aConstStart.Date && dt.Date <= aConstEnd.Date)
                    backgroundColor = "constbgcolor";
                else if (aConstEnd.Date != DateTime.MinValue.Date && dt.Date > aConstEnd.Date)
                    backgroundColor = "closeoutbgcolor";
                else if (dt.Date < aPreconStart.Date && dtEnd > aPreconStart.Date)
                    backgroundColor = "nobgcolor-preconbgcolor";
                else
                    backgroundColor = "nobgcolor"; // if allocation does not falls in any stage consider it as precon stage

            }
            else
                backgroundColor = "itsmassignedbgcolor";
            return backgroundColor;
        }

        public static string GetTooltipString(ApplicationContext context, List<(DateTime StartDate, DateTime EndDate)> datePairs)
        {
            StringBuilder tooltipBuilder = new StringBuilder();
            foreach (var item in datePairs)
            {
                string tooltipItem = $"{uHelper.GetDateStringInFormat(context, item.StartDate, false)} to {uHelper.GetDateStringInFormat(context, item.EndDate, false)}</br>";
                tooltipBuilder.Append(tooltipItem);
            }

            return tooltipBuilder.ToString();
        }

        public static int getCloseoutperiod(ApplicationContext context)
        {
            ConfigurationVariableManager configMGR = new ConfigurationVariableManager(context);
            int days = UGITUtility.StringToInt(configMGR.GetValue(ConfigConstants.CloseoutPeriod));
            return days == 0 ? 10 : days;
        }

        public static double GetAllocationOverbookingFactor(ApplicationContext context)
        {
            ConfigurationVariableManager configMGR = new ConfigurationVariableManager(context);
            double factor = UGITUtility.StringToDouble(configMGR.GetValue(ConfigConstants.AllocationOverbookingFactor));
            return factor == 0 ? 10 : factor;
        }
        public static bool HideAllocationTemplate(ApplicationContext context)
        {
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            return configManager.GetValueAsBool(ConfigConstants.HideAllocationTemplate);
        }
        public static string getAltTicketIdField(ApplicationContext context, string moduleName)
        {
            string prefixcolumn = string.Empty;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule module = moduleViewManager.LoadByName(moduleName, true);
            if (module != null)
            {
                prefixcolumn = module.AltTicketIdField;
                if (string.IsNullOrEmpty(prefixcolumn))
                    prefixcolumn = DatabaseObjects.Columns.TicketId;
            }
            return prefixcolumn;
        }


        public static void UpdateEstimatedAllocationTable(ResourceAllocationManager objAllocationManager, ProjectEstimatedAllocationManager ObjEstimatedAllocationManager, RResourceAllocation rAllocation, string workitemtype)
        {
            if (workitemtype == "PMM" || workitemtype == "NPR" || workitemtype == "CPR" ||
                                            workitemtype == "CNS" || workitemtype == "OPM" || workitemtype == "TSK" || workitemtype == "Current Projects (PMM)" || workitemtype == "Project Management Module (PMM)")
            {
                ProjectEstimatedAllocation estimatedAllocation = ObjEstimatedAllocationManager.MapIntoEstAllocationFromResourceObject(rAllocation);
                ObjEstimatedAllocationManager.Insert(estimatedAllocation);
                if (estimatedAllocation.ID > 0)
                {
                    rAllocation.ProjectEstimatedAllocationId = UGITUtility.ObjectToString(estimatedAllocation.ID);
                }
            }
        }
        public static bool HasAccessToAddExpTags(ApplicationContext context)
        {
            UserProfile user = context.CurrentUser;
            UserProfileManager userProfileManager = new UserProfileManager(context);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            string adminGroup = configManager.GetValue(ConfigConstants.ExperienceTagAdminGroup);

            if (!string.IsNullOrEmpty(adminGroup))
            {
                return userProfileManager.CheckUserIsInResourceGroup(adminGroup, user);
            }

            return false;
        }

        public static bool HasExperienceTagAllowMultiselect(ApplicationContext context)
        {
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            return configManager.GetValueAsBool(ConfigConstants.ExperienceTagAllowMultiselect);
        }
        public static bool IsExperienceTagAllowGroupFilter(ApplicationContext context)
        {
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            return configManager.GetValueAsBool(ConfigConstants.ExperienceTagAllowGroupFilter);
        }
        public static string GenerateSummaryIcon(ApplicationContext context, DataRow currentTicket, string projectID)
        {
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.Load(x => x.TicketId == projectID && x.Deleted != true);
            bool isAllocInPrecon = false, isAllocInConst = false, isAllocInCloseOut = false;
            string oText = string.Empty;
            if (currentTicket != null && projectAllocations != null)
            {
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconEndDate))
                {
                    DateTime preconStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconStartDate]);
                    DateTime preconEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconEndDate]);

                    if (preconEnd != DateTime.MinValue && preconStart != DateTime.MinValue)
                    {
                        isAllocInPrecon = projectAllocations.Any(o => o.AllocationStartDate <= preconEnd && o.AllocationEndDate >= preconStart);
                    }
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    DateTime constStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    DateTime constEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (constStart != DateTime.MinValue && constEnd != DateTime.MinValue)
                    {
                        isAllocInConst = projectAllocations.Any(o => o.AllocationStartDate <= constEnd && o.AllocationEndDate >= constStart);
                    }
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutDate))
                {
                    DateTime closesoutEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutDate]);
                    DateTime closesoutStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (closesoutEnd == DateTime.MinValue && closesoutStart != DateTime.MinValue)
                    {
                        closesoutEnd = closesoutStart.AddWorkingDays(uHelper.getCloseoutperiod(context));
                    }
                    if (closesoutEnd != DateTime.MinValue && closesoutStart != DateTime.MinValue)
                    {
                        isAllocInCloseOut = projectAllocations.Any(o => o.AllocationStartDate <= closesoutEnd && o.AllocationEndDate >= closesoutStart);
                    }
                }
            }
            oText = string.Format("<div class='alloctype'><i class='{0}' style='margin-right:5px;font-size: 17px; color:#52BED9'></i>" +
                                "<i class='{1}' style='margin-right:5px;font-size: 17px; color:#005C9B'></i>" +
                                "<i class='{2}' style='font-size: 17px; color:#351B82'></i></div>",
                                isAllocInPrecon ? "fa fa-circle" : "far fa-circle",
                                isAllocInConst ? "fa fa-circle" : "far fa-circle",
                                isAllocInCloseOut ? "fa fa-circle" : "far fa-circle");
            return oText;
        }
        public static bool Updateshortname(ApplicationContext context)
        {
            bool status = false;
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                uGITDAL.ExecuteDataSetWithParameters("usp_update_shortname", values);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                ULog.WriteException(ex);
            }
            return status;
        }

        public static DataRow FillShortNameIfExists(DataRow TicketRow, ApplicationContext context)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            if (UGITUtility.IfColumnExists(TicketRow, DatabaseObjects.Columns.ShortName) && string.IsNullOrEmpty(UGITUtility.ObjectToString(TicketRow[DatabaseObjects.Columns.ShortName])))
            {
                int ShortNameLength = UGITUtility.StringToInt(configurationVariableManager.GetValue(ConfigConstants.ShortNameCharacters));
                string titleString = UGITUtility.ObjectToString(TicketRow[DatabaseObjects.Columns.Title]);
                TicketRow[DatabaseObjects.Columns.ShortName] = titleString.Substring(0, Math.Min(titleString.Length, ShortNameLength));
            }
            return TicketRow;
        }
        public static List<long> SplitAllocationIdIntoTuple(string input, DateTime actualStartDate, DateTime actualEndDate)
        {
            List<long> resultList = new List<long>();
            if (string.IsNullOrEmpty(input))
                return resultList;

            string[] segments = input.Split(',');

            foreach (var segment in segments)
            {
                string[] parts = segment.Split(':');
                if (parts.Length == 3)
                {
                    long id = long.Parse(parts[0]);

                    DateTime startDate = UGITUtility.StringToDateTime(parts[1]);
                    DateTime endDate = UGITUtility.StringToDateTime(parts[2]);
                    DateTime newStartDate = new DateTime(startDate.Year, startDate.Month, 01);
                    // Filter based on the target date
                    if (actualStartDate.Date == startDate.Date && actualEndDate.Date == endDate.Date)
                    {
                        resultList.Add(id);
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Used to fetch weekwise %allocation of the users.
        /// </summary>
        /// <param name="resourceUsageSummaryWeekWises"></param>
        /// <returns></returns>
        public static List<ResourceWeekWiseAvailabilityResponse> GetWeekWiseAveragePctAllocation(List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWises, List<AllocationData> allocationDates, List<string> userIds)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            double overLappingFactor = GetAllocationOverbookingFactor(context);
            List<ResourceWeekWiseAvailabilityResponse> lstResourceWeekWiseAvailability = new List<ResourceWeekWiseAvailabilityResponse>();
            if (resourceUsageSummaryWeekWises?.Count > 0)
            {
                userIds.ForEach(user =>
                {
                    double totalPct = 0, totalDays = 0, postTotalPct = 0;
                    ResourceWeekWiseAvailabilityResponse resourceWeekWiseAvailability = new ResourceWeekWiseAvailabilityResponse();
                    resourceWeekWiseAvailability.WeekWiseAllocations = new List<ResourceWeekWiseAvailabilityModel>();
                    allocationDates.ForEach(date =>
                    {
                        List<ResourceUsageSummaryWeekWise> tempWeekWiseSummary = resourceUsageSummaryWeekWises.Where(x => x.ActualEndDate.Value >= date.StartDate
                            && x.ActualStartDate.Value <= date.EndDate && x.Resource == user).ToList();

                        DateTime startDate = date.StartDate.Date;
                        DateTime endDate = date.EndDate.Date;
                        DateTime tempStartData = startDate.StartOfWeek(DayOfWeek.Monday).Date, tempEndDate = endDate;
                        while (tempStartData <= endDate)
                        {
                            tempEndDate = tempStartData.EndOfWeek(DayOfWeek.Friday);
                            if (tempEndDate > endDate)
                            {
                                tempEndDate = endDate;
                            }
                            if (tempEndDate >= DateTime.Now)
                            {
                                List<ResourceUsageSummaryWeekWise> lstWeekData = tempWeekWiseSummary?.Where(x => x.WeekStartDate == tempStartData)?.ToList();
                                double? totalPercentAllocatedInRange = 0;
                                double searchPeriodDays = 5;
                                List<Tuple<DateTime, DateTime>> lstDates = new List<Tuple<DateTime, DateTime>>();
                                List<WeekdetailedSummary> weekdetailedSummaries = new List<WeekdetailedSummary>();
                                if (lstWeekData?.Count > 0)
                                {
                                    double? totalAllocOverlapDays = 0;
                                    foreach (ResourceUsageSummaryWeekWise rallocWeek in lstWeekData)
                                    {
                                        var startDt = tempStartData.Date < date.StartDate.Date ? date.StartDate.Date : tempStartData.Date;
                                        var endDt = tempEndDate.Date > date.EndDate.Date ? date.EndDate.Date : tempEndDate.Date;

                                        var startDtW = tempStartData.Date < rallocWeek.ActualStartDate.Value ? rallocWeek.ActualStartDate.Value : tempStartData.Date;
                                        var endDtW = tempEndDate.Date > rallocWeek.ActualEndDate.Value ? rallocWeek.ActualEndDate.Value : tempEndDate.Date;
                                        double workingDays = 0;
                                        if (rallocWeek.WorkItemType == "Time Off")
                                        {
                                            workingDays = uHelper.GetTotalWorkingDaysBetween(context, startDtW.Date, endDtW.Date);
                                            lstDates.Add(new Tuple<DateTime, DateTime>(startDtW, endDtW));
                                        }
                                        else
                                        {
                                            workingDays = uHelper.GetTotalWorkingDaysBetween(context, startDt.Date, endDt.Date);
                                            lstDates.Add(new Tuple<DateTime, DateTime>(startDt, endDt));
                                        }
                                        //lstDates.Add(new Tuple<DateTime, DateTime>(startDt, endDt));
                                        double? pctAlloc = 0;
                                        if (rallocWeek.PctAllocation.HasValue)
                                            pctAlloc = rallocWeek.WorkItemType == "Time Off" ? 100 : rallocWeek.PctAllocation;
                                        totalAllocOverlapDays += workingDays * pctAlloc;

                                        weekdetailedSummaries.Add(new WeekdetailedSummary
                                        {
                                            TicketId = rallocWeek.WorkItem,
                                            Title = string.IsNullOrWhiteSpace(rallocWeek.Title) ? rallocWeek.WorkItem : rallocWeek.Title,
                                            Role = rallocWeek.SubWorkItem
                                        });
                                        //searchPeriodDays += workingDays;
                                    }
                                    searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(context, lstDates.Min(x => x.Item1), lstDates.Max(x => x.Item2));
                                    totalPercentAllocatedInRange = totalAllocOverlapDays > 0 && searchPeriodDays > 0 ? totalAllocOverlapDays / searchPeriodDays : 0;
                                }
                                else
                                {
                                    searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(context, tempStartData.Date < date.StartDate.Date ? date.StartDate.Date : tempStartData.Date
                                        , tempEndDate.Date > date.EndDate.Date ? date.EndDate.Date : tempEndDate.Date);
                                }
                                ResourceWeekWiseAvailabilityModel weekWiseAvailabilityModel = new ResourceWeekWiseAvailabilityModel();
                                weekWiseAvailabilityModel.WeekStartDate = lstWeekData?.Count > 0 ? lstDates.Min(x => x.Item1) : tempStartData < date.StartDate ? date.StartDate.Date : tempStartData.Date;
                                weekWiseAvailabilityModel.PctAllocation = Math.Round(totalPercentAllocatedInRange.Value, 2);
                                weekWiseAvailabilityModel.PostPctAllocation = Math.Round(totalPercentAllocatedInRange.Value + date.RequiredPctAllocation, 2);
                                weekWiseAvailabilityModel.PctAvailability = 100 - (totalPercentAllocatedInRange.Value - overLappingFactor);
                                weekWiseAvailabilityModel.NoOfDays = searchPeriodDays;
                                weekWiseAvailabilityModel.WeekdetailedSummaries = weekdetailedSummaries;
                                weekWiseAvailabilityModel.IsAvailable = date.RequiredPctAllocation <= weekWiseAvailabilityModel.PctAvailability ? true : false;
                                if (searchPeriodDays > 0)
                                    resourceWeekWiseAvailability.WeekWiseAllocations.Add(weekWiseAvailabilityModel);
                                totalPct += (totalPercentAllocatedInRange.Value * searchPeriodDays);
                                postTotalPct += ((totalPercentAllocatedInRange.Value + date.RequiredPctAllocation) * searchPeriodDays);
                                totalDays += searchPeriodDays;
                            }
                            tempStartData = tempStartData.EndOfWeek(DayOfWeek.Sunday).AddDays(1);
                        }
                    });
                    resourceWeekWiseAvailability.UserId = user;
                    resourceWeekWiseAvailability.AverageUtilization = totalDays > 0 && totalPct > 0 ? Math.Round(totalPct / totalDays) : 0;
                    resourceWeekWiseAvailability.PostAverageUtilization = (totalDays > 0 && postTotalPct > 0 ? Math.Round(postTotalPct / totalDays) : 0) - overLappingFactor;
                    resourceWeekWiseAvailability.AvailabilityType = resourceWeekWiseAvailability.WeekWiseAllocations.All(x => x.IsAvailable)
                        ? Availability.FullyAvailable
                        : resourceWeekWiseAvailability.WeekWiseAllocations.Any(x => x.IsAvailable)
                            ? Convert.ToDouble((Convert.ToDouble(resourceWeekWiseAvailability.WeekWiseAllocations.Where(x => x.IsAvailable).Count()) / Convert.ToDouble(resourceWeekWiseAvailability.WeekWiseAllocations.Count())) * 100) > 60
                                ? Availability.NearToFullyAvailable
                                : Availability.PartiallyAvailable
                            : Availability.NotAvailable;
                    lstResourceWeekWiseAvailability.Add(resourceWeekWiseAvailability);
                });
            }
            return lstResourceWeekWiseAvailability;
        }

        public static List<string> GetProjectIdsBySector(string sector)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            TicketManager ticketManager = new TicketManager(context);
            DataTable dt = ticketManager.GetAllTicketsByModuleName(ModuleNames.CPR);
            List<string> ids = dt?.AsEnumerable()?.Where(o => o.Field<string>(DatabaseObjects.Columns.BCCISector) == sector)?.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId))?.ToList() ?? null;
            return ids;
        }
        public static List<string> GetProjectIdsByContractType(string contractType)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            TicketManager ticketManager = new TicketManager(context);
            DataTable dt = ticketManager.GetAllTicketsByModuleName(ModuleNames.CPR);
            List<string> ids = dt?.AsEnumerable()?.Where(o => o.Field<string>(DatabaseObjects.Columns.OwnerContractTypeChoice) == contractType)?.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId))?.ToList() ?? null;
            return ids;
        }

        public static List<string> GetProjectIdsByClient(string projectId)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            TicketManager ticketManager = new TicketManager(context);
            DataTable dt = ticketManager.GetAllTicketsByModuleName(ModuleNames.CPR);
            List<string> ids = dt?.AsEnumerable()?.Where(o => o.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup) == projectId)?.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId))?.ToList() ?? null;
            return ids;
        }

        public static List<string> GetProjectIdsByChannelPartners(string projectId)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            RelatedCompanyManager relatedCompanyManager = new RelatedCompanyManager(context);
            List<string> partnerCompany = relatedCompanyManager.Load(x => x.TicketID == projectId).Select(x => x.CRMCompanyLookup)?.Distinct()?.ToList() ?? null;
            if (partnerCompany?.Count > 0)
            {
                return relatedCompanyManager.Load(x => partnerCompany.Contains(x.CRMCompanyLookup))?.Select(x => x.TicketID)?.Distinct()?.ToList() ?? null;
            }
            return null;
        }

        public static List<string> GetChannelPartnersTitleByType(string projectId, string type)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            RelatedCompanyManager relatedCompanyManager = new RelatedCompanyManager(context);
            CRMRelationshipTypeManager cRMRelationshipTypeManager = new CRMRelationshipTypeManager(context);
            long id = cRMRelationshipTypeManager.Load(x => x.Title == type && x.TenantID == context.TenantID)?.FirstOrDefault()?.ID ?? 0;
            List<RelatedCompany> partnerCompany = relatedCompanyManager.Load(x => x.TicketID == projectId) ?? null;
            if (id > 0)
            {
                return partnerCompany?.Where(x => x.RelationshipTypeLookup.Value == id)?.Count() > 0 
                    ? partnerCompany?.Where(x => x.RelationshipTypeLookup.Value == id)?.Select(x => x.CRMCompanyLookup)?.Distinct().ToList()
                    : null;
            }
            return null;
        }

        public static List<string> GetProjectResourceIds(string projectId) {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(context);
            List<string> users = allocationManager.Load(x => x.TicketId == projectId && x.AssignedTo != "00000000-0000-0000-0000-000000000000")
                ?.Select(x => x.AssignedTo)?.Distinct()?.ToList() ?? null;
            return users;
        }

        /// <summary>
        /// Get common projects id where users have worked together.
        /// </summary>
        /// <param name="userIds"></param>
        public static List<string> GetUsersCommonProjects(List<string> userIds)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            List<string> projectIds = new List<string>();
            ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(context);
            var allocations = allocationManager.Load(x => x.AssignedTo != "00000000-0000-0000-0000-000000000000" 
                && userIds.Contains(x.AssignedTo) && x.TicketId.StartsWith(ModuleNames.CPR))?.GroupBy(y => y.TicketId) ?? null;
            if (allocations?.Count() > 0)
            {
                foreach (var item in allocations)
                {
                    List<string> users = item.Select(z => z.AssignedTo)?.Distinct()?.ToList() ?? null;
                    if (users?.Count() > 0 && userIds?.Intersect(users)?.Count() == userIds?.Count())
                    {
                        projectIds.Add(item.Key);
                    }
                }
            }
            return projectIds;
        }

        public static bool HasAnyPastAllocation(string projectId)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            List<string> projectIds = new List<string>();
            ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(context);
            var allocations = allocationManager.Load(x => x.TicketId == projectId) ?? null;
            return allocations?.Any(x => x.AllocationStartDate < DateTime.Now) ?? false;
        }

        public static string GetFormattedChanceOfSuccess(string chanceOfSuccess)
        {
            string retValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(chanceOfSuccess))
            {
                int choice = UGITUtility.StringToInt(chanceOfSuccess.Trim().Substring(1, 1));
                if (choice == 4)
                {
                    retValue = "Chance Of Success: >80%";
                }
                else if (choice == 3)
                {
                    retValue = "Chance Of Success: 50 to 80%";
                }
                else if (choice == 2)
                {
                    retValue = "Chance Of Success: 30 to 50%";
                }
                else if (choice == 1)
                {
                    retValue = "Chance Of Success: <30%";
                }
            }
            return retValue;
        }

        public static bool IsDateRangeOverlapping(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            if (startDate1 != DateTime.MinValue && endDate1 != DateTime.MinValue && startDate2 != DateTime.MinValue && endDate2 != DateTime.MinValue)
            {
                if (startDate1 <= endDate2 && endDate1 >= startDate2)
                {
                    return true;
                }
            }
            return false;
        }

        public static string EscapeApostrophe(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.Replace("'", "''");
            else
                return value;
        }

        public static string GetERPJobIDName(ApplicationContext context)
        {
            return context.ConfigManager.GetValue(ConfigConstants.ERPJobIDName);
        }
        public static string GetERPJobIDNCName(ApplicationContext context)
        {
            return context.ConfigManager.GetValue(ConfigConstants.ERPJobIDNCName);
        }
        public static GanttFormat GetGanttFormat(ApplicationContext context)
        {
            string ganttFormat = context.ConfigManager.GetValue(ConfigConstants.AdminGanttFormat);
            if (Enum.TryParse(ganttFormat, out GanttFormat retValue))
                return retValue;
            else
                return GanttFormat.Days;
        }
    }
    

    public static class Extensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = dt.DayOfWeek - endOfWeek;
            if (diff > 0)
                diff -= 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime AddWorkingDays(this DateTime startDate, int numBusinessDays)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DateTime endDate = uHelper.AddWorkingDays(startDate, numBusinessDays, context);
            return endDate;
        }
        public static DataTable GetTable(ApplicationContext context, DataTable dt)
        {
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            foreach (DataColumn column in dt.Columns)
            {
                FieldConfiguration fieldConfiguration = fieldConfigurationManager.GetFieldByFieldName(column.ColumnName);
                if (fieldConfiguration != null)
                {
                    if (fieldConfiguration.Datatype.ToLower() == "lookup")
                    {
                        // column.Expression = "IIF("+column.ColumnName+ "<> null, "+fieldConfigurationManager.GetFieldConfigurationData(column.ColumnName,"")+" , '')";.
                    }
                    else if (fieldConfiguration.Datatype.ToLower() == "userfield")
                    {

                    }
                }
            }
            return dt;
        }

        public static bool EqualsTo(this string value, string toCompare)
        {
            return string.Equals(value, toCompare, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Case Insensitive string comparision.
        /// </summary>        
        public static bool EqualsIgnoreCase(this string value, string toCompare)
        {
            return string.Equals(value, toCompare, StringComparison.InvariantCultureIgnoreCase);
        }
        
    }
}
