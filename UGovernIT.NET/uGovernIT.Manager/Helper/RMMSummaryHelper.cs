using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Manager
{
    public class RMMSummaryHelper
    {
        public static void DistributionRMMAllocation(ApplicationContext context,int workingHours, UserProfile userInfo, DataRow workItem, DataTable workAllocationTable, ref DataTable tempSummaryWeek, ref DataTable tempSummaryMonth)
        {
            if (workItem == null)
                return;

            DataTable workItemTable = new DataTable();
            workItemTable.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            workItemTable.Columns.Add(DatabaseObjects.Columns.WorkItemType, typeof(string));
            workItemTable.Columns.Add(DatabaseObjects.Columns.WorkItem, typeof(string));
            workItemTable.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            workItemTable.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));

            DataRow workItemRow = workItemTable.NewRow();
            workItemTable.Rows.Add(workItemRow);
            workItemRow[DatabaseObjects.Columns.Id] = workItem["ID"];
            workItemRow[DatabaseObjects.Columns.Title] = Convert.ToString(workItem[DatabaseObjects.Columns.Title]);
            workItemRow[DatabaseObjects.Columns.WorkItemType] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
            workItemRow[DatabaseObjects.Columns.WorkItem] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]);
            workItemRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);

            DistributionRMMAllocation(context, workingHours, userInfo, workItemTable.Rows[0], workAllocationTable, ref tempSummaryWeek, ref tempSummaryMonth);
        }

        /// <summary>
        /// Distributes RMM Allocation for specified workitem.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="workingHours"></param>
        /// <param name="userInfo"></param>
        /// <param name="workItem"></param>
        /// <param name="workAllocationTable"></param>
        /// <param name="tempSummaryWeek"></param>
        /// <param name="tempSummaryMonth"></param>
        public static void DistributionRMMAllocation(ApplicationContext spWeb, int workingHours, UserProfile userInfo, DataRow workItem, DataTable workAllocationTable, ref DataTable tempSummaryWeek, ref DataTable tempSummaryMonth)
        {
            if (tempSummaryWeek == null)
                tempSummaryWeek = CreateTempSummaryWeek();
            if (tempSummaryMonth == null)
                tempSummaryMonth = CreateTempSummaryMonth();

            if (workAllocationTable == null)
            {
                return;
            }

            string monthAllocationQuery =string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, workItem[DatabaseObjects.Columns.Id]);
            DataTable monthDistributionTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly).Select(monthAllocationQuery).CopyToDataTable();

            foreach (DataRow allocatedWork in workAllocationTable.Rows)
            {
                DateTime startDate = Convert.ToDateTime(allocatedWork[DatabaseObjects.Columns.AllocationStartDate]);
                DateTime endDate = Convert.ToDateTime(allocatedWork[DatabaseObjects.Columns.AllocationEndDate]);
                double workPctPlndAlloation = 0;
                double.TryParse(Convert.ToString(allocatedWork[DatabaseObjects.Columns.PctPlannedAllocation]), out workPctPlndAlloation);
               

                DateTime tempStartDate = startDate;

                //Fill  weeksummary table for specified workitem
                #region TempSummaryWeek Table
                while (tempStartDate < endDate)
                {
                    //Gets startdate of the week
                    DateTime weekStartDate = tempStartDate.Date.AddDays(-(int)tempStartDate.DayOfWeek + 1);

                    double pctAllocationForMonth = 0;
                    double pctAllocationForNextMonth = 0;
                    if (monthDistributionTable != null)
                    {
                        DataRow allocatedForMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(weekStartDate.Year, weekStartDate.Month, 1).Date);
                        if (allocatedForMonth != null)
                            pctAllocationForMonth = Convert.ToDouble(Convert.ToString(allocatedForMonth[DatabaseObjects.Columns.PctAllocation]));

                        if (weekStartDate.AddDays(7).Month != weekStartDate.Month)
                        {
                            DataRow allocatedForNxtMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(weekStartDate.AddMonths(1).Year, weekStartDate.AddMonths(1).Month, 1).Date);
                            if (allocatedForNxtMonth != null)
                                pctAllocationForNextMonth = Convert.ToDouble(Convert.ToString(allocatedForNxtMonth[DatabaseObjects.Columns.PctAllocation]));
                        }
                    }

                    DataRow summaryRow = tempSummaryWeek.NewRow();
                    tempSummaryWeek.Rows.Add(summaryRow);

                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];

                    //Fill Userinfo in summary table
                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.ManagerID;
                        if (userInfo.ManagerID != null)
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                        summaryRow[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                        summaryRow[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                        summaryRow[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                        if (userInfo.FunctionalArea != null)
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                    }
                    summaryRow[DatabaseObjects.Columns.WeekStartDate] = weekStartDate;
                    //Calculates total days in a week
                    int totalDaysInWeek = 7 - (int)tempStartDate.DayOfWeek;
                    if (weekStartDate.Date.AddDays(totalDaysInWeek).Date > endDate.Date)
                    {
                        totalDaysInWeek = (int)endDate.DayOfWeek;
                    }

                    //Monday is first day of week and Sunday is the last day of week
                    if (totalDaysInWeek == 7)
                        totalDaysInWeek = 1;
                    else
                        totalDaysInWeek += 1;

                 

                    double allocationHours = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.AllocationHour]), out allocationHours);

                    double totalPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PctAllocation]), out totalPctAllocation);

                    double plndAllocationHrs = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PlannedAllocationHour]), out plndAllocationHrs);
                    double plndPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PctPlannedAllocation]), out plndPctAllocation);

                    //Calculates hours in a day based on allocation %.
                  
                    double totalHours = allocationHours;
                    DateTime tempEndDate = weekStartDate.AddDays(6);
                    if (weekStartDate.Month != tempEndDate.Month)
                    {
                        double hoursInDay = (pctAllocationForMonth * workingHours) / 100;
                        double hoursInDayForNxtMonth = (pctAllocationForNextMonth * workingHours) / 100;
                        int daysInCtMonth = DateTime.DaysInMonth(weekStartDate.Year, weekStartDate.Month) - weekStartDate.Day + 1;
                        int daysInNxtMonth = tempEndDate.Day;

                        //Calculates working day within week
                        int workingDayInWeekCtMonth = uHelper.GetTotalWorkingDaysBetween(spWeb,weekStartDate, weekStartDate.AddDays(daysInCtMonth-1));
                        int workingDayInWeekNxtMonth = uHelper.GetTotalWorkingDaysBetween(spWeb,tempEndDate.AddDays(-daysInNxtMonth+1), tempEndDate);

                        double weekWorkingHrs = (hoursInDay * workingDayInWeekCtMonth) + (hoursInDayForNxtMonth * workingDayInWeekNxtMonth);
                        totalHours += weekWorkingHrs;

                        int totalWorkingHrs = (workingDayInWeekCtMonth + workingDayInWeekNxtMonth) * workingHours;
                        totalPctAllocation = totalPctAllocation + (weekWorkingHrs * 100) / totalWorkingHrs;


                        double plndAllct = 0;
                        double.TryParse(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), out plndAllct);
                        double plndHrsInDay = (workPctPlndAlloation * workingHours) / 100;
                        plndAllocationHrs += (plndHrsInDay * (workingDayInWeekCtMonth + workingDayInWeekNxtMonth));
                        plndPctAllocation += workPctPlndAlloation;
                    }
                    else
                    {
                        double hoursInDay = (pctAllocationForMonth * workingHours) / 100;
                        //Calculates working day within week
                        int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(spWeb, weekStartDate, weekStartDate.AddDays(totalDaysInWeek - 1));
                        //Calculates total hours allocated in a week.
                        totalHours += (hoursInDay * workingDayInWeek);
                        totalPctAllocation = totalPctAllocation + pctAllocationForMonth;

                        double plndAllct = 0;
                        double.TryParse(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), out plndAllct);
                        double plndHrsInDay = (workPctPlndAlloation * workingHours) / 100;
                        plndAllocationHrs += (plndHrsInDay * workingDayInWeek);
                        plndPctAllocation += workPctPlndAlloation;
                    }

                    summaryRow[DatabaseObjects.Columns.AllocationHour] = totalHours;
                    if (totalHours > 0)
                        summaryRow[DatabaseObjects.Columns.PctAllocation] = totalPctAllocation;
                    else
                        summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;

                    summaryRow[DatabaseObjects.Columns.PlannedAllocationHour] = plndAllocationHrs;
                    if (plndAllocationHrs > 0)
                        summaryRow[DatabaseObjects.Columns.PctPlannedAllocation] = plndPctAllocation;
                    else
                        summaryRow[DatabaseObjects.Columns.PctPlannedAllocation] = 0;

                    //Set Next week start date. if it is greater then enddate then set enddate.
                    if (weekStartDate.Date.AddDays(7).Date > endDate.Date)
                    {
                        tempStartDate = endDate;
                    }
                    else
                    {
                        tempStartDate = tempStartDate.AddDays(totalDaysInWeek);
                    }
                }

                #endregion

                tempStartDate = startDate;
                #region Fill tempSummaryMonth table
                while (tempStartDate < endDate)
                {
                    double pctAllocationForMonth = 0;
                    DataRow allocatedForMonth = null;
                    if (monthDistributionTable != null)
                    {
                        allocatedForMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(tempStartDate.Year, tempStartDate.Month, 1).Date);
                        if (allocatedForMonth != null)
                        {
                            pctAllocationForMonth = Convert.ToDouble(Convert.ToString(allocatedForMonth[DatabaseObjects.Columns.PctAllocation]));
                        }
                    }

                    //Sets start date of month
                    DateTime monthStartDate = new DateTime(tempStartDate.Year, tempStartDate.Month, 1); ;

                    DataRow summaryRow = tempSummaryMonth.NewRow();
                    tempSummaryMonth.Rows.Add(summaryRow);
                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];

                    //Fill Userinfo in summary table
                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.Id;
                        if (userInfo.ManagerID != null)
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                        summaryRow[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                        summaryRow[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                        summaryRow[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                        if (userInfo.FunctionalArea != null)
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                    }

                    summaryRow[DatabaseObjects.Columns.MonthStartDate] = monthStartDate;

                    //Calculates totaldays in month
                    int totalDaysInMonth = (DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month) - tempStartDate.Day) + 1;
                    if (tempStartDate.Year == endDate.Year && tempStartDate.Month == endDate.Month)
                    {
                        totalDaysInMonth = (endDate.Day - tempStartDate.Day) + 1;
                    }

                    //Calculates working days in month
                    int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(spWeb, monthStartDate, monthStartDate.AddDays(totalDaysInMonth - 1));

                    double allocationHours = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.AllocationHour]), out allocationHours);

                    double totalPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PctAllocation]), out totalPctAllocation);

                    double plndAllocationHrs = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PlannedAllocationHour]), out plndAllocationHrs);
                    double plndPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow[DatabaseObjects.Columns.PctPlannedAllocation]), out plndPctAllocation);

                    //Calculates hours in each days based on % allocation.
                    //double allocationPct = 0;
                    //double.TryParse(Convert.ToString(allocatedWork[DatabaseObjects.Columns.PctAllocation]), out allocationPct);
                    //double hoursInDay = (allocationPct * workingHours) / 100;
                    double hoursInDay = (pctAllocationForMonth * workingHours) / 100;

                    //Calculates total hours allocated in a month
                    double totalHours = allocationHours + (hoursInDay * workingDayInMonth);
                    totalPctAllocation = totalPctAllocation + pctAllocationForMonth;

                    double plndAllct = 0;
                    double.TryParse(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), out plndAllct);
                    double plndHrsInDay = (workPctPlndAlloation * workingHours) / 100;
                    plndAllocationHrs += (plndHrsInDay * workingDayInMonth);
                    plndPctAllocation += workPctPlndAlloation;
                 

                    summaryRow[DatabaseObjects.Columns.AllocationHour] = totalHours;
                    if (totalHours > 0)
                        summaryRow[DatabaseObjects.Columns.PctAllocation] = totalPctAllocation;
                    else
                        summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;

                    summaryRow[DatabaseObjects.Columns.PlannedAllocationHour] = plndAllocationHrs;
                    if (plndAllocationHrs > 0)
                        summaryRow[DatabaseObjects.Columns.PctPlannedAllocation] = plndPctAllocation;
                    else
                        summaryRow[DatabaseObjects.Columns.PctPlannedAllocation] = 0;

                    //Sets next month start date
                    tempStartDate = tempStartDate.AddDays(totalDaysInMonth);
                }
                #endregion
            }

        }

        /// <summary>
        /// Distributes RMM Actual for specified workitem and workdate
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="workingHours"></param>
        /// <param name="userInfo"></param>
        /// <param name="workItem"></param>
        /// <param name="workDate"></param>
        /// <param name="actualItemTable">Timesheet datatable which is queried using SiteDataQuery</param>
        /// <param name="tempSummaryWeek"></param>
        /// <param name="tempSummaryMonth"></param>
        public static void DistributionRMMActual(ApplicationContext spWeb, int workingHours, UserProfile userInfo, DataRow[] workItemCollection, DateTime workDate, DataTable actualItemTable, ref DataTable tempSummaryWeek, ref DataTable tempSummaryMonth)
        {
            if (tempSummaryWeek == null)
                tempSummaryWeek = CreateTempSummaryWeek();
            if (tempSummaryMonth == null)
                tempSummaryMonth = CreateTempSummaryMonth();

            string workItemLookupString = string.Empty;

            //Generate summary table for each workitem
            foreach (DataRow workItem in workItemCollection)
            {
                workItemLookupString = string.Format("{0}{1}{0}", workItem[DatabaseObjects.Columns.Id], Constants.Separator);

                //Add WorkDate1 and HoursTaken1 column in actuat table so type cast WorkDate and HoursTaken
                if (actualItemTable != null && actualItemTable.Rows.Count > 0 && !actualItemTable.Columns.Contains("WorkDate1") && !actualItemTable.Columns.Contains("HoursTaken1"))
                {
                    actualItemTable.Columns.Add("WorkDate1", typeof(DateTime), string.Format("{0}", DatabaseObjects.Columns.WorkDate));
                    actualItemTable.Columns.Add("HoursTaken1", typeof(double), string.Format("{0}", DatabaseObjects.Columns.HoursTaken));
                }

                //Sets weekStart dates
                DateTime weekStartDate = workDate.Date.AddDays(-(int)workDate.DayOfWeek + 1);
                DateTime weekEndDate = weekStartDate.Date.AddDays(6);
               

                //Fill WeekSummary table 
                {
                    double totalHours = actualItemTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == workItemLookupString && x.Field<DateTime>("WorkDate1") >= weekStartDate && x.Field<DateTime>("WorkDate1") <= weekStartDate.AddDays(6)).Sum(x => x.Field<double>("HoursTaken1"));
                    int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(spWeb, weekStartDate, weekStartDate.AddDays(6));

                    DataRow summaryRow = tempSummaryWeek.NewRow();
                    tempSummaryWeek.Rows.Add(summaryRow);
                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.WeekStartDate] = weekStartDate.Date;
                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];

                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.Id;
                        if (userInfo.ManagerID != null)
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                        if (userInfo.FunctionalArea != null)
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                        summaryRow[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                        summaryRow[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                        summaryRow[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                    }

                    summaryRow[DatabaseObjects.Columns.ActualHour] = totalHours;

                    //Calculates % actual in week based on total hours work
                    if (totalHours > 0)
                        summaryRow[DatabaseObjects.Columns.PctActual] = workingDayInWeek > 0 ? (totalHours * 100) / (workingHours * workingDayInWeek) : 100;
                    else
                        summaryRow[DatabaseObjects.Columns.PctActual] = 0;
                }

                //Fill MonthSummary table
                int totalMonthInWeek = (weekEndDate.Month - weekStartDate.Month) + 1;
                for (int i = 0; i < totalMonthInWeek; i++)
                {
                    int month = weekStartDate.Month + i;
                    DateTime monthStartDate = new DateTime(workDate.Year, month, 1);

                    double totalHours = actualItemTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == workItemLookupString && x.Field<DateTime>("WorkDate1") >= monthStartDate && x.Field<DateTime>("WorkDate1") <= monthStartDate.AddDays(DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month) - 1)).Sum(x => x.Field<double>("HoursTaken1"));
                    int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(spWeb, weekStartDate, weekStartDate.AddMonths(1).AddDays(-1));

                    DataRow summaryRow = tempSummaryMonth.NewRow();
                    tempSummaryMonth.Rows.Add(summaryRow);
                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.MonthStartDate] = monthStartDate.Date;
                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];

                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.Id;
                        if (userInfo.ManagerID != null)
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                        if (userInfo.FunctionalArea != null)
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                        summaryRow[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                        summaryRow[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                        summaryRow[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                    }

                    summaryRow[DatabaseObjects.Columns.ActualHour] = totalHours;

                    //Calculates % actual in month based on total hours work
                    if (totalHours > 0)
                        summaryRow[DatabaseObjects.Columns.PctActual] = workingDayInMonth > 0 ? (totalHours * 100) / (workingHours * workingDayInMonth) : 100;
                    else
                        summaryRow[DatabaseObjects.Columns.PctActual] = 0;
                }
            }
        }

        public static DataTable CreateTempSummaryWeek()
        {
            DataTable tempSummaryWeek = new DataTable();
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.Title);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(int));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.WorkItemType);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.WorkItem);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.SubWorkItem);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.Resource, typeof(int));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.Manager, typeof(int));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.FunctionalAreaTitle);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.WeekStartDate, typeof(DateTime));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.PctAllocation, typeof(double));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.AllocationHour, typeof(double));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.PctActual, typeof(double));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.ActualHour, typeof(double));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.IsIT, typeof(bool));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.IsManager, typeof(bool));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.IsConsultant, typeof(bool));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(double));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.PlannedAllocationHour, typeof(double));
            return tempSummaryWeek;
        }

        public static DataTable CreateTempSummaryMonth()
        {
            DataTable tempSummaryMonth = new DataTable();
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.Title);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(int));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.WorkItemType);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.WorkItem);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.SubWorkItem);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.Resource, typeof(int));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.Manager, typeof(int));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.FunctionalAreaTitle);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.MonthStartDate, typeof(DateTime));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PctAllocation, typeof(double));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.AllocationHour, typeof(double));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PctActual, typeof(double));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.ActualHour, typeof(double));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.IsIT, typeof(bool));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.IsManager, typeof(bool));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.IsConsultant, typeof(bool));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(double));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PlannedAllocationHour, typeof(double));
            return tempSummaryMonth;
        }

        public static void Delete(ApplicationContext spWeb, Guid summaryListID, List<int> spItems)
        {
            if (spItems.Count <= 0)
            {
                return;
            }

            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            string updateMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Delete</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "</Method>";

            StringBuilder query = new StringBuilder();
            foreach (int itemID in spItems)
            {
                query.AppendFormat(updateMethodFormat, itemID, summaryListID, itemID);
            }
            string batch = string.Format(batchFormat, query.ToString());
            string batchReturn = spWeb.ProcessBatchData(batch);
        }

        /// <summary>
        /// Update actual hours in rmm summary list
        /// </summary>
        /// <param name="workItemsID">List of workitemid of which summary need to update</param>
        /// <param name="url">Web url</param>
        /// <param name="wStartDate">start date of week on which timesheet is filled.</param>
        public static void UpdateActualInRMMSummary(ApplicationContext context,List<int> workItemsID, string resourceID, string url, DateTime wStartDate)
        {
            if (workItemsID.Count <= 0)
            {
                return;
            }
            
                    //Gets distinct workitenid
                    workItemsID = workItemsID.Distinct().ToList();

                    //Calculate working dates and working hours/day
                    int workingHours = uHelper.GetWorkingHoursInADay();

                    //Get ResourceInformation from SiteUserInfoList list
                    UserProfile userInfo = context.UserManager.GetUserById(resourceID);
                    if (userInfo == null)
                        return;

                    //Set week start date and month start date
                    DateTime weekStartDate = wStartDate.Date.AddDays(-(int)wStartDate.DayOfWeek + 1); // Not strictly needed since the start date passed in is always the week start (Mon=1)
                    DateTime weekEndDate = weekStartDate.Date.AddDays(6);
                    DateTime monthStartDate = new DateTime(wStartDate.Year, wStartDate.Month, 1);

                    DateTime qStartDate = monthStartDate;
                    DateTime qEndDate = monthStartDate.AddDays(DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month));
                    if (weekStartDate < qStartDate)
                    {
                        qStartDate = weekStartDate;
                    }
                    else if (weekEndDate > qEndDate)
                    {
                        qEndDate = new DateTime(weekEndDate.Year, weekEndDate.Month, DateTime.DaysInMonth(weekEndDate.Year, weekEndDate.Month));
                    }

                    //Generate workitemexpression using workitem id
                    List<string> workItemsLookupExp = new List<string>();
                    List<string> workItemsExp = new List<string>();
                    List<string> workItemsIDExp = new List<string>();

                    foreach (int wID in workItemsID)
                    {
                        workItemsLookupExp.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, wID));
                        workItemsExp.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemID, wID));
                        workItemsIDExp.Add(string.Format("{0}='{1}'>", DatabaseObjects.Columns.Id, wID));
                    }
                    string workItemExpression = string.Join(" Or",workItemsExp);
                    string workItemLookupExpression = string.Join(" Or",workItemsLookupExp);
            string workItemIDExpression = string.Join(" Or", workItemsIDExp);

                    string workItemsQuery =workItemIDExpression;
                    DataRow[] workItemCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems).Select(workItemsQuery);

                    //Get Actual from RMMTimeSheet between qStartDate and qEndDate
                    DataTable actualItemList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet);
                   
                   string actuiQuery = string.Format("{0} and {1}>={2} and {1}<={3}"
                        , workItemLookupExpression, DatabaseObjects.Columns.WorkDate, qStartDate, qEndDate);
                    DataTable actualItemTable = null;
                    try
                    {
                        actualItemTable = actualItemList.Select(actuiQuery).CopyToDataTable();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                    }

                    DataTable tempSummaryWeek = null;
                    DataTable tempSummaryMonth = null;

                    RMMSummaryHelper.DistributionRMMActual(context, workingHours, userInfo, workItemCollection, wStartDate, actualItemTable, ref tempSummaryWeek, ref tempSummaryMonth);

                    //Loads summary data(Month, Week) against workitemid and StartDate
                    DataTable rmmMonthSummaryList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise);
            DataTable rmmWeekSummaryList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise);
                    string summaryMonthQuery =  string.Format("{0} and {1}>={2} and {1}<={3}",
                        workItemExpression, DatabaseObjects.Columns.MonthStartDate,monthStartDate.Date,weekEndDate.Date);
                    string summaryWeekQuery = string.Format(" {0} and {1}={2}",
                       workItemExpression, DatabaseObjects.Columns.WeekStartDate, weekStartDate.Date);
                    DataRow[] rmmMonthSummaryColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise).Select(summaryMonthQuery);
                    DataRow[] rmmWeekSummaryColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise).Select(summaryWeekQuery);
                    DataTable rmmMonthSummaryTable = rmmMonthSummaryColl.CopyToDataTable();
                    DataTable rmmWeekSummaryTable = rmmWeekSummaryColl.CopyToDataTable();
                    if (rmmMonthSummaryTable != null && !rmmMonthSummaryTable.Columns.Contains("WorkItemID1"))
                    {
                        rmmMonthSummaryTable.Columns.Add("WorkItemID1", typeof(int), string.Format("{0}", DatabaseObjects.Columns.WorkItemID));
                    }
                    if (rmmWeekSummaryTable != null && !rmmWeekSummaryTable.Columns.Contains("WorkItemID1"))
                    {
                        rmmWeekSummaryTable.Columns.Add("WorkItemID1", typeof(int), string.Format("{0}", DatabaseObjects.Columns.WorkItemID));
                    }

                    //Update summary for each workItems
                    foreach (DataRow workItem in workItemCollection)
                    {
                        //Update Month summary
                        if (tempSummaryMonth != null && tempSummaryMonth.Rows.Count > 0)
                        {
                            DataRow[] workSummaryRows = tempSummaryMonth.AsEnumerable().Where(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == workItem.ID).ToArray();
                            for (int i = 0; i < workSummaryRows.Length; i++)
                            {

                                DataRow workSummaryRow = workSummaryRows[i];
                                DateTime mStartDate = Convert.ToDateTime(workSummaryRow[DatabaseObjects.Columns.MonthStartDate]);
                                DataRow monthSummaryItem = null;
                                bool addNewMonthSummary = false;
                                if (rmmMonthSummaryTable == null || rmmMonthSummaryTable.Rows.Count <= 0)
                                {
                                    addNewMonthSummary = true;
                                }
                                else
                                {
                                    DataRow mRow = rmmMonthSummaryTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("WorkItemID1") == workItem.ID && x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate) == mStartDate);
                                    if (mRow != null)
                                        monthSummaryItem = rmmMonthSummaryColl.CopyToDataTable().Select("ID="+Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]))[0];
                                    else
                                        addNewMonthSummary = true;
                                }

                                if (addNewMonthSummary)
                                {
                                    //don't add new entry if allocation and actual hours are zero
                                    if (Convert.ToDouble(workSummaryRow[DatabaseObjects.Columns.AllocationHour]) <= 0 && Convert.ToDouble(workSummaryRow[DatabaseObjects.Columns.ActualHour]) <= 0)
                                    {
                                        continue;
                                    }

                                    monthSummaryItem = rmmMonthSummaryList.NewRow();
                                    monthSummaryItem[DatabaseObjects.Columns.WorkItemID] = workItem["ID"];
                                    monthSummaryItem[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                                    monthSummaryItem[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                                    monthSummaryItem[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                                    if (userInfo != null)
                                    {
                                        monthSummaryItem[DatabaseObjects.Columns.Resource] = userInfo.Id;
                                        if (userInfo.ManagerID != null)
                                            monthSummaryItem[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                                        monthSummaryItem[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                                        monthSummaryItem[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                                        monthSummaryItem[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                                        if (userInfo.FunctionalArea != null)
                                            monthSummaryItem[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                                    }

                                    monthSummaryItem[DatabaseObjects.Columns.MonthStartDate] = mStartDate;
                                    monthSummaryItem[DatabaseObjects.Columns.PctAllocation] = 0;
                                    monthSummaryItem[DatabaseObjects.Columns.AllocationHour] = 0;

                                    // For project work items, set title to project ID, else blank
                                    monthSummaryItem[DatabaseObjects.Columns.Title] = string.Empty;
                                    string workItemType = Convert.ToString(monthSummaryItem[DatabaseObjects.Columns.WorkItemType]);
                                    if (workItemType == "PMM" || workItemType == "TSK" || workItemType == "NPR")
                                    {
                                        DataRow projectItem = Ticket.getCurrentTicket(workItemType, Convert.ToString(monthSummaryItem[DatabaseObjects.Columns.WorkItem]), spWeb);
                                        if (projectItem != null)
                                            monthSummaryItem[DatabaseObjects.Columns.Title] = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                                    }
                                }

                                monthSummaryItem[DatabaseObjects.Columns.ActualHour] = workSummaryRow[DatabaseObjects.Columns.ActualHour];
                                monthSummaryItem[DatabaseObjects.Columns.PctActual] = workSummaryRow[DatabaseObjects.Columns.PctActual];
                                monthSummaryItem.UpdateOverwriteVersion();
                            }
                        }

                        //Update Week summary
                        if (tempSummaryWeek != null && tempSummaryWeek.Rows.Count > 0)
                        {
                            DataRow workSummaryRow = tempSummaryWeek.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == workItem.ID);
                            if (workSummaryRow != null)
                            {
                                DataRow weekSummaryItem = null;
                                bool addNewWeekSummary = false;
                                if (rmmWeekSummaryTable == null || rmmWeekSummaryTable.Rows.Count <= 0)
                                {
                                    addNewWeekSummary = true;
                                }
                                else
                                {
                                    DataRow mRow = rmmWeekSummaryTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("WorkItemID1") == workItem.ID && x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate) == weekStartDate);
                                    if (mRow != null)
                                        weekSummaryItem = rmmWeekSummaryColl.CopyToDataTable().Select("ID="+Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]))[0];
                                    else
                                        addNewWeekSummary = true;
                                }

                                if (addNewWeekSummary)
                                {
                                    //don't add new entry if allocation and actual hours are zero
                                    if (Convert.ToDouble(workSummaryRow[DatabaseObjects.Columns.AllocationHour]) <= 0 && Convert.ToDouble(workSummaryRow[DatabaseObjects.Columns.ActualHour]) <= 0)
                                    {
                                        continue;
                                    }

                                    weekSummaryItem = rmmWeekSummaryList.NewRow();
                                    weekSummaryItem[DatabaseObjects.Columns.WorkItemID] = workItem["ID"];
                                    weekSummaryItem[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                                    weekSummaryItem[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                                    weekSummaryItem[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                                    if (userInfo != null)
                                    {
                                        weekSummaryItem[DatabaseObjects.Columns.Resource] = userInfo.Id;
                                        if (userInfo.ManagerID != null)
                                            weekSummaryItem[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                                        weekSummaryItem[DatabaseObjects.Columns.IsManager] = userInfo.IsManager;
                                        weekSummaryItem[DatabaseObjects.Columns.IsIT] = userInfo.IsIT;
                                        weekSummaryItem[DatabaseObjects.Columns.IsConsultant] = userInfo.IsConsultant;
                                        if (userInfo.FunctionalArea != null)
                                            weekSummaryItem[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                                    }

                                    weekSummaryItem[DatabaseObjects.Columns.WeekStartDate] = weekStartDate;
                                    weekSummaryItem[DatabaseObjects.Columns.PctAllocation] = 0;
                                    weekSummaryItem[DatabaseObjects.Columns.AllocationHour] = 0;

                                    // For project work items, set title to project ID, else blank
                                    weekSummaryItem[DatabaseObjects.Columns.Title] = string.Empty;
                                    string workItemType = Convert.ToString(weekSummaryItem[DatabaseObjects.Columns.WorkItemType]);
                                    if (workItemType == "PMM" || workItemType == "TSK" || workItemType == "NPR")
                                    {
                                        DataRow projectItem = Ticket.getCurrentTicket( context, workItemType, Convert.ToString(weekSummaryItem[DatabaseObjects.Columns.WorkItem]));
                                        if (projectItem != null)
                                            weekSummaryItem[DatabaseObjects.Columns.Title] = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                                    }
                                }

                                weekSummaryItem[DatabaseObjects.Columns.ActualHour] = workSummaryRow[DatabaseObjects.Columns.ActualHour];
                                weekSummaryItem[DatabaseObjects.Columns.PctActual] = workSummaryRow[DatabaseObjects.Columns.PctActual];
                                weekSummaryItem.UpdateOverwriteVersion();
                            }
                        }
                    }

                    //Deletes empty allocation and actual entries from rmm monthly summary list
                    List<int> deleteItemList = new List<int>();
                    foreach (DataRow item in rmmMonthSummaryColl)
                    {
                        if (Convert.ToDouble(item[DatabaseObjects.Columns.AllocationHour]) <= 0 && Convert.ToDouble(item[DatabaseObjects.Columns.ActualHour]) <= 0)
                        {
                            deleteItemList.Add(Convert.ToInt32(item["ID"]));
                        }
                    }
                    if (deleteItemList.Count > 0)
                        RMMSummaryHelper.Delete(context, rmmMonthSummaryList["ID"], deleteItemList);

                    //Deletes empty allocation and actual entries from rmm weekly summary list
                    deleteItemList = new List<int>();
                    foreach (DataRow item in rmmWeekSummaryColl)
                    {
                        if (Convert.ToDouble(item[DatabaseObjects.Columns.AllocationHour]) <= 0 && Convert.ToDouble(item[DatabaseObjects.Columns.ActualHour]) <= 0)
                        {
                            deleteItemList.Add(Convert.ToInt32(item["ID"]));
                        }
                    }
                    if (deleteItemList.Count > 0)
                        RMMSummaryHelper.Delete(context, rmmWeekSummaryList["ID"], deleteItemList);

                    //spWeb.AllowUnsafeUpdates = false;
               
           
        }

        public static void UpdateRMMAllocationSummary(int workItemID, ApplicationContext spWeb)
        {
            //Calculate working dates and working hours/day
            int workingHours = uHelper.GetWorkingHoursInADay(spWeb);

            DataRow workItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems).Select("ID="+workItemID)[0];
            if (workItem == null)
                return;          
            UserProfile userProfile = spWeb.UserManager.LoadById(Convert.ToString(workItem[DatabaseObjects.Columns.Resource]));
            if (userProfile == null)
                return;

            string allocationQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID);
            DataRow[] workAllocations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation).Select(allocationQuery);

            string summaryQuery =  string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemID, workItemID); ;
            DataRow[] rmmMonthSummaryColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise).Select(summaryQuery) ;
            DataRow[] rmmWeekSummaryColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise).Select(summaryQuery);
            DataTable rmmWeekSummaryTable = rmmWeekSummaryColl.CopyToDataTable();
            DataTable rmmMonthSummaryTable = rmmMonthSummaryColl.CopyToDataTable();

            if (rmmMonthSummaryTable != null && rmmMonthSummaryTable.Rows.Count > 0)
            {
                foreach (DataRow mItem in rmmMonthSummaryTable.Rows)
                {
                    mItem[DatabaseObjects.Columns.PctAllocation] = 0;
                    mItem[DatabaseObjects.Columns.AllocationHour] = 0;
                }
            }
            if (rmmWeekSummaryTable != null && rmmWeekSummaryTable.Rows.Count > 0)
            {
                foreach (DataRow wItem in rmmWeekSummaryTable.Rows)
                {
                    wItem[DatabaseObjects.Columns.PctAllocation] = 0;
                    wItem[DatabaseObjects.Columns.AllocationHour] = 0;
                }
            }

            DataTable tempSummaryWeek = RMMSummaryHelper.CreateTempSummaryWeek();
            DataTable tempSummaryMonth = RMMSummaryHelper.CreateTempSummaryMonth();
            DataTable workAllocationTable = workAllocations.CopyToDataTable();
            RMMSummaryHelper.DistributionRMMAllocation(spWeb, workingHours, userProfile, workItem, workAllocationTable, ref tempSummaryWeek, ref tempSummaryMonth);

            DataTable summaryListWeek = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise);
            DataTable summaryListMonth = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise);

            // For project work items, set title to project ID, else blank
            string workItemTitle = string.Empty;
            string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
            if (workItemType == "PMM" || workItemType == "TSK" || workItemType == "NPR")
            {
                DataRow projectItem = Ticket.getCurrentTicket(spWeb,workItemType, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
                if (projectItem != null)
                    workItemTitle = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
            }

           // spWeb.AllowUnsafeUpdates = true;

            #region Add and edit ResourceUsageSummaryWeekWise List
            if (tempSummaryWeek != null && tempSummaryWeek.Rows.Count > 0)
            {
                bool addNewWeekSummary = false;
                DateTime updatedRowTime = DateTime.Now;
                foreach (DataRow row in tempSummaryWeek.Rows)
                {
                    addNewWeekSummary = false;
                    DataRow item = null;
                    if (rmmWeekSummaryTable == null || rmmWeekSummaryTable.Rows.Count <= 0)
                    {
                        addNewWeekSummary = true;
                    }
                    else
                    {
                        DataRow mRow = rmmWeekSummaryTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate) == Convert.ToDateTime(row[DatabaseObjects.Columns.WeekStartDate]));
                        if (mRow != null)
                        {
                            mRow = rmmWeekSummaryTable.Rows[rmmWeekSummaryTable.Rows.IndexOf(mRow)];
                            item = rmmWeekSummaryColl.CopyToDataTable().Select("ID="+Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]))[0];
                            if (item == null)
                                addNewWeekSummary = true;

                            mRow[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                            mRow[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                        }
                        else
                            addNewWeekSummary = true;
                    }
                    if (addNewWeekSummary)
                    {
                        item = summaryListWeek.NewRow();
                        item[DatabaseObjects.Columns.WorkItemID] = row[DatabaseObjects.Columns.WorkItemID];
                        item[DatabaseObjects.Columns.WorkItemType] = row[DatabaseObjects.Columns.WorkItemType];
                        item[DatabaseObjects.Columns.WorkItem] = row[DatabaseObjects.Columns.WorkItem];
                        item[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                        item[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                        item[DatabaseObjects.Columns.Manager] = row[DatabaseObjects.Columns.Manager];
                        item[DatabaseObjects.Columns.FunctionalAreaTitle] = row[DatabaseObjects.Columns.FunctionalAreaTitle];
                        item[DatabaseObjects.Columns.IsManager] = row[DatabaseObjects.Columns.IsManager];
                        item[DatabaseObjects.Columns.IsIT] = row[DatabaseObjects.Columns.IsIT];
                        item[DatabaseObjects.Columns.IsConsultant] = row[DatabaseObjects.Columns.IsConsultant];
                        item[DatabaseObjects.Columns.WeekStartDate] = row[DatabaseObjects.Columns.WeekStartDate];
                        item[DatabaseObjects.Columns.ActualHour] = 0;
                        item[DatabaseObjects.Columns.PctActual] = 0;
                        item[DatabaseObjects.Columns.Title] = workItemTitle;
                    }
                    item[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                    item[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                    item[DatabaseObjects.Columns.PctPlannedAllocation] = row[DatabaseObjects.Columns.PctPlannedAllocation];
                    item[DatabaseObjects.Columns.PlannedAllocationHour] = row[DatabaseObjects.Columns.PlannedAllocationHour];
                    try
                    {
                        item.Update();
                    }
                    catch (Exception)
                    {
                        // Ignore exception, usually due to write conflict
                    }
                }
                tempSummaryWeek.Rows.Clear();
            }
            #endregion

            #region Add and edit ResourceUsageSummaryMonthWise List
            if (tempSummaryMonth != null && tempSummaryMonth.Rows.Count > 0)
            {
                DateTime updatedRowTime = DateTime.Now;

                foreach (DataRow row in tempSummaryMonth.Rows)
                {
                    DataRow item = null;
                    bool addNewMonthSummary = false;
                    if (rmmMonthSummaryTable == null || rmmMonthSummaryTable.Rows.Count <= 0)
                    {
                        addNewMonthSummary = true;
                    }
                    else
                    {
                        DataRow mRow = rmmMonthSummaryTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate) == Convert.ToDateTime(row[DatabaseObjects.Columns.MonthStartDate]));
                        if (mRow != null)
                        {
                            mRow = rmmMonthSummaryTable.Rows[rmmMonthSummaryTable.Rows.IndexOf(mRow)];
                            item = rmmMonthSummaryColl.CopyToDataTable().Select("ID="+Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]))[0];
                            //item = rmmMonthSummaryColl, Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]));
                            if (item == null)
                                addNewMonthSummary = true;

                            mRow[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                            mRow[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                        }
                        else
                            addNewMonthSummary = true;
                    }

                    if (addNewMonthSummary)
                    {
                        item = summaryListMonth.NewRow();
                        item[DatabaseObjects.Columns.WorkItemID] = row[DatabaseObjects.Columns.WorkItemID];
                        item[DatabaseObjects.Columns.WorkItemType] = row[DatabaseObjects.Columns.WorkItemType];
                        item[DatabaseObjects.Columns.WorkItem] = row[DatabaseObjects.Columns.WorkItem];
                        item[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                        item[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                        item[DatabaseObjects.Columns.Manager] = row[DatabaseObjects.Columns.Manager];
                        item[DatabaseObjects.Columns.FunctionalAreaTitle] = row[DatabaseObjects.Columns.FunctionalAreaTitle];
                        item[DatabaseObjects.Columns.IsManager] = row[DatabaseObjects.Columns.IsManager];
                        item[DatabaseObjects.Columns.IsIT] = row[DatabaseObjects.Columns.IsIT];
                        item[DatabaseObjects.Columns.IsConsultant] = row[DatabaseObjects.Columns.IsConsultant];
                        item[DatabaseObjects.Columns.MonthStartDate] = row[DatabaseObjects.Columns.MonthStartDate];
                        item[DatabaseObjects.Columns.ActualHour] = 0;
                        item[DatabaseObjects.Columns.PctActual] = 0;
                        item[DatabaseObjects.Columns.Title] = workItemTitle;
                    }

                    item[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                    item[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                    item[DatabaseObjects.Columns.PctPlannedAllocation] = row[DatabaseObjects.Columns.PctPlannedAllocation];
                    item[DatabaseObjects.Columns.PlannedAllocationHour] = row[DatabaseObjects.Columns.PlannedAllocationHour];
                    try
                    {
                        item.Update();
                    }
                    catch (Exception)
                    {
                        // Ignore exception, usually due to write conflict
                    }
                }
            }
            #endregion

            #region Deletes Empty Allocations from month and week summary list
            //Deletes Empty Allocation entry from month summary list.
            List<int> deleteItemList = new List<int>();
            if (rmmMonthSummaryTable != null && rmmMonthSummaryTable.Rows.Count > 0)
            {
                SPListItem rItem = null;
                foreach (DataRow mmItem in rmmMonthSummaryTable.Rows)
                {
                    if (Convert.ToDouble(mmItem[DatabaseObjects.Columns.AllocationHour]) <= 0)
                    {
                        if (Convert.ToDouble(mmItem[DatabaseObjects.Columns.ActualHour]) <= 0)
                        {
                            deleteItemList.Add(Convert.ToInt32(mmItem[DatabaseObjects.Columns.Id]));
                        }
                        else
                        {
                            //******* need to check again
                            rItem = SPListHelper.GetItemByID(rmmMonthSummaryColl, Convert.ToInt32(mmItem[DatabaseObjects.Columns.Id]));
                            if (rItem != null)
                            {
                                rItem[DatabaseObjects.Columns.AllocationHour] = 0;
                                rItem[DatabaseObjects.Columns.PctAllocation] = 0;
                                rItem.Update();
                            }
                        }
                    }

                }
                if (deleteItemList.Count > 0)
                    RMMSummaryHelper.Delete(spWeb, summaryListMonth.ID, deleteItemList);
            }

            //Deletes Empty Allocation entry from week summary list.
            if (rmmWeekSummaryTable != null && rmmWeekSummaryTable.Rows.Count > 0)
            {
                SPListItem rItem = null;
                deleteItemList = new List<int>();
                foreach (DataRow wwItem in rmmWeekSummaryTable.Rows)
                {
                    if (Convert.ToDouble(wwItem[DatabaseObjects.Columns.AllocationHour]) <= 0)
                    {
                        if (Convert.ToDouble(wwItem[DatabaseObjects.Columns.ActualHour]) <= 0)
                        {
                            deleteItemList.Add(Convert.ToInt32(wwItem[DatabaseObjects.Columns.Id]));
                        }
                        else
                        {
                            //******* need to check again
                            rItem = SPListHelper.GetItemByID(rmmWeekSummaryColl, Convert.ToInt32(wwItem[DatabaseObjects.Columns.Id]));
                            if (rItem != null)
                            {
                                rItem[DatabaseObjects.Columns.AllocationHour] = 0;
                                rItem[DatabaseObjects.Columns.PctAllocation] = 0;
                                rItem.Update();
                            }
                        }
                    }
                }
                if (deleteItemList.Count > 0)
                    RMMSummaryHelper.Delete(spWeb, summaryListWeek.ID, deleteItemList);
            }
            #endregion

            //spWeb.AllowUnsafeUpdates = false;
        }

        public static void UpdateRMMAllocationSummary(int workItemID, string spWebUrl,ApplicationContext applicationContext)
        {
            
                    UpdateRMMAllocationSummary(workItemID, applicationContext);
               
        }

        public static void DeleteRMMAllocationSummary(List<int> workItemsID, string spWebUrl,ApplicationContext spWeb)
        {
            foreach (int workItemID in workItemsID)
            {
                UpdateRMMAllocationSummary(workItemID, spWeb);
            }

        }

        public static void UpdateResourceUsageSummary(ApplicationContext spWeb, DataTable workItems, DataTable allocationItems, DataTable actualItems)
        {
            //Calculates total working hours in one day
            DateTime workDayStartTime = Convert.ToDateTime(ConfigurationVariable.GetValue(spWeb, "WorkdayStartTime"));
            DateTime workDayEndTime = Convert.ToDateTime(ConfigurationVariable.GetValue(spWeb, "WorkdayEndTime"));
            if (workDayStartTime == DateTime.MinValue)
            {
                workDayStartTime = DateTime.Now.Date;
            }

            if (workDayEndTime == DateTime.MinValue)
            {
                workDayEndTime = DateTime.Now.Date.AddDays(1);
            }
            int workingHours = (int)workDayEndTime.Subtract(workDayStartTime).TotalHours;

            DataTable summaryTableWeek = uGITCache.LoadTable(DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, null, spWeb, 20000);
            DataTable summaryTableMonth = uGITCache.LoadTable(DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, null, spWeb, 20000);
            DataTable userInfoes = spWeb.SiteUserInfoList.Items.GetDataTable();
            SPList allocationList = spWeb.Lists[DatabaseObjects.Lists.ResourceAllocation];
            SPList timeSheetList = spWeb.Lists[DatabaseObjects.Lists.ResourceTimeSheet];

            SPList summaryListWeek = spWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryWeekWise];
            SPList summaryListMonth = spWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryMonthWise];

            if (workItems == null || workItems.Rows.Count < 0)
            {
                return;
            }

            //group all workitems by resource
            var workItemsByUser = workItems.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource));

            DataTable tempSummaryWeek = CreateTempSummaryWeek();

            DataTable tempSummaryMonth = CreateTempSummaryMonth();

            foreach (var userWork in workItemsByUser)
            {
                //Continue if resource is empty (user must exist)
                if (string.IsNullOrEmpty(userWork.Key) || userWork.Key.Trim() == string.Empty)
                {
                    continue;
                }

                string userName = userWork.Key.Replace("'", "''");

                //Workitems of user
                DataRow[] userWorkItems = userWork.ToArray();

                //User info of user
                DataRow[] usersInfo = userInfoes.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, userName));
                if (usersInfo.Length == 0)
                    continue; // User not found, probably deleted!

                UserProfile userProfile = UserProfile.LoadById(Convert.ToInt32(usersInfo[0][DatabaseObjects.Columns.Id]), spWeb);
                if (userProfile == null)
                    return;

                //Group workallocations of user by workitem
                ILookup<string, DataRow> workAllocationByWorkItem = null;
                if (allocationItems != null)
                {
                    workAllocationByWorkItem = allocationItems.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userName)).ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup));
                }

                //Group actual work of user by workitem
                ILookup<string, DataRow> actualWorkByWorkItem = null;
                if (actualItems != null)
                {
                    actualWorkByWorkItem = actualItems.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userName)).ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup));
                }

                //Group week workSummary rows by user
                DataRow[] workSmmaryRowsWeek = new DataRow[0];
                if (summaryTableWeek != null && summaryTableWeek.Rows.Count > 0)
                {
                    workSmmaryRowsWeek = summaryTableWeek.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userName));
                }

                //Group month workSummary rows by user
                DataRow[] workSmmaryRowsMonth = new DataRow[0];
                if (summaryTableMonth != null && summaryTableMonth.Rows.Count > 0)
                {
                    workSmmaryRowsMonth = summaryTableMonth.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userName));
                }

                foreach (DataRow workItem in userWorkItems)
                {
                    string workItemID = Convert.ToString(workItem[DatabaseObjects.Columns.Id]);

                    // For project work items, set title to project ID, else blank
                    string workItemTitle = string.Empty;
                    string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    if (workItemType == "PMM" || workItemType == "TSK" || workItemType == "NPR")
                    {
                        SPListItem projectItem = Ticket.getCurrentTicket(workItemType, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]), spWeb);
                        if (projectItem != null)
                            workItemTitle = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                    }

                    DataRow[] workItemAllocations = new DataRow[0];
                    DataRow[] workItemActuals = new DataRow[0];

                    //Allocations for current workitem
                    if (workAllocationByWorkItem != null)
                    {
                        var workAlloations = workAllocationByWorkItem.FirstOrDefault(x => x.Key == workItemID);
                        if (workAlloations != null)
                        {
                            workItemAllocations = workAlloations.ToArray();
                        }
                    }

                    //Actuals for current workitem
                    if (actualWorkByWorkItem != null)
                    {
                        var workActuals = actualWorkByWorkItem.FirstOrDefault(x => x.Key == workItemID);
                        if (workActuals != null)
                        {
                            workItemActuals = workActuals.ToArray();
                        }
                    }

                    if (workItemAllocations.Length > 0 && userProfile != null)
                        RMMSummaryHelper.DistributionRMMAllocation(spWeb, workingHours, userProfile, workItem, workItemAllocations.CopyToDataTable(), ref tempSummaryWeek, ref tempSummaryMonth);
                   
                    //Run loop of all actual work of current workitem and current user 
                    foreach (DataRow actualWork in workItemActuals)
                    {
                        DateTime workDate = Convert.ToDateTime(actualWork[DatabaseObjects.Columns.WorkDate]);
                        #region TempSummaryWeek Table
                        {
                            // DayOfWeek returns Sun = 0 to Sat = 6
                            //   convert to Mon = 0 to Sun = 6
                            int dayOfWeek = (int)workDate.DayOfWeek - 1;
                            if (dayOfWeek < 0)
                                dayOfWeek = 6;
                            DateTime weekStartDate = workDate.AddDays(-dayOfWeek);

                            DataRow summaryRow = null;
                            if (tempSummaryWeek != null && tempSummaryWeek.Rows.Count > 0)
                            {
                                summaryRow = tempSummaryWeek.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]) &&
                                                                                            x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate).Date == weekStartDate.Date);
                            }

                            if (summaryRow == null)
                            {
                                summaryRow = tempSummaryWeek.NewRow();
                                summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                                summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                                summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                                summaryRow[DatabaseObjects.Columns.PctActual] = 0;
                                tempSummaryWeek.Rows.Add(summaryRow);
                            }

                            summaryRow[DatabaseObjects.Columns.WeekStartDate] = weekStartDate.Date;
                            summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                            summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                            summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                            summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];
                           
                            summaryRow[DatabaseObjects.Columns.Title] = workItemTitle;

                            if (userProfile != null)
                            {
                                summaryRow[DatabaseObjects.Columns.Resource] = userProfile.ID;
                                if(userProfile.Manager != null)
                                summaryRow[DatabaseObjects.Columns.Manager] = userProfile.Manager.ID;
                                summaryRow[DatabaseObjects.Columns.IsManager] = userProfile.IsManager;
                                summaryRow[DatabaseObjects.Columns.IsIT] = userProfile.IsIT;
                                summaryRow[DatabaseObjects.Columns.IsConsultant] = userProfile.IsConsultant;
                                if (userProfile.FunctionalArea != null)
                                    summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userProfile.FunctionalArea.Value;
                            }

                            double hoursTaken = 0;
                            double.TryParse(Convert.ToString(uHelper.GetSPItemValue(summaryRow, DatabaseObjects.Columns.ActualHour)), out hoursTaken);

                            double currentWeekhoursTaken = 0;
                            double.TryParse(Convert.ToString(uHelper.GetSPItemValue(actualWork, DatabaseObjects.Columns.HoursTaken)), out currentWeekhoursTaken);
                            double totalHours = hoursTaken + currentWeekhoursTaken;
                            int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(weekStartDate, weekStartDate.AddDays(6), spWeb);
                            summaryRow[DatabaseObjects.Columns.ActualHour] = totalHours;
                            if (totalHours > 0)
                                summaryRow[DatabaseObjects.Columns.PctActual] = workingDayInWeek > 0 ? (totalHours * 100) / (workingHours * workingDayInWeek) : 100;
                            else
                                summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                        }
                        #endregion

                        #region Fill tempSummaryMonth table
                        {
                            DateTime monthStartDate = new DateTime(workDate.Year, workDate.Month, 1);
                            int daysInMonth = DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month);

                            DataRow summaryRow = null;
                            if (tempSummaryMonth != null && tempSummaryMonth.Rows.Count > 0)
                            {
                                summaryRow = tempSummaryMonth.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]) &&
                                                                                            x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == monthStartDate.Date);
                            }

                            if (summaryRow == null)
                            {
                                summaryRow = tempSummaryMonth.NewRow();
                                summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                                summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                                summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                                summaryRow[DatabaseObjects.Columns.PctActual] = 0;
                                tempSummaryMonth.Rows.Add(summaryRow);
                            }

                            summaryRow[DatabaseObjects.Columns.MonthStartDate] = monthStartDate.Date;
                            summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem[DatabaseObjects.Columns.Id];
                            summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem[DatabaseObjects.Columns.WorkItemType];
                            summaryRow[DatabaseObjects.Columns.WorkItem] = workItem[DatabaseObjects.Columns.WorkItem];
                            summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];
                            summaryRow[DatabaseObjects.Columns.Title] = workItemTitle;

                            if (userProfile != null)
                            {
                                summaryRow[DatabaseObjects.Columns.Resource] = userProfile.ID;
                                if (userProfile.Manager != null)
                                    summaryRow[DatabaseObjects.Columns.Manager] = userProfile.Manager.ID;
                                summaryRow[DatabaseObjects.Columns.IsManager] = userProfile.IsManager;
                                summaryRow[DatabaseObjects.Columns.IsIT] = userProfile.IsIT;
                                summaryRow[DatabaseObjects.Columns.IsConsultant] = userProfile.IsConsultant;
                                if (userProfile.FunctionalArea != null)
                                    summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userProfile.FunctionalArea.Value;
                            }

                            int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(monthStartDate, monthStartDate.AddDays(daysInMonth), spWeb);
                            double hoursTaken = 0;
                            double.TryParse(Convert.ToString(uHelper.GetSPItemValue(summaryRow, DatabaseObjects.Columns.ActualHour)), out hoursTaken);

                            double currentWeekhoursTaken = 0;
                            double.TryParse(Convert.ToString(uHelper.GetSPItemValue(actualWork, DatabaseObjects.Columns.HoursTaken)), out currentWeekhoursTaken);
                            double totalHours = hoursTaken + currentWeekhoursTaken;

                            summaryRow[DatabaseObjects.Columns.ActualHour] = totalHours;
                            if (totalHours > 0)
                                summaryRow[DatabaseObjects.Columns.PctActual] = workingDayInMonth > 0 ? (totalHours * 100) / (workingHours * workingDayInMonth) : 100;
                            else
                                summaryRow[DatabaseObjects.Columns.PctActual] = 0;
                        }
                        #endregion
                    }
                }

                #region Add and edit ResourceUsageSummaryWeekWise List
                if (tempSummaryWeek != null && tempSummaryWeek.Rows.Count > 0)
                {
                    DateTime updatedRowTime = DateTime.Now;
                    foreach (DataRow row in tempSummaryWeek.Rows)
                    {
                        SPListItem item = null;
                        bool newEntry = false;
                        if (workSmmaryRowsWeek.Length > 0)
                        {
                            DataRow summaryRow = workSmmaryRowsWeek.FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(row[DatabaseObjects.Columns.WorkItemID]) &&
                                                                                            x.Field<DateTime>(DatabaseObjects.Columns.WeekStartDate).Date == Convert.ToDateTime(row[DatabaseObjects.Columns.WeekStartDate]).Date);
                            if (summaryRow != null)
                            {
                                item = SPListHelper.GetSPListItem(summaryListWeek, (int)summaryRow[DatabaseObjects.Columns.Id]);
                                summaryRow[DatabaseObjects.Columns.Modified] = updatedRowTime;
                            }
                            else
                            {
                                newEntry = true;
                            }
                        }
                        else
                        {
                            newEntry = true;
                        }

                        if (newEntry)
                        {
                            item = summaryListWeek.AddItem();
                            item[DatabaseObjects.Columns.WorkItemID] = row[DatabaseObjects.Columns.WorkItemID];
                        }

                        item[DatabaseObjects.Columns.WorkItemType] = row[DatabaseObjects.Columns.WorkItemType];
                        item[DatabaseObjects.Columns.WorkItem] = row[DatabaseObjects.Columns.WorkItem];
                        item[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                        item[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                        item[DatabaseObjects.Columns.Manager] = row[DatabaseObjects.Columns.Manager];
                        item[DatabaseObjects.Columns.IsManager] = row[DatabaseObjects.Columns.IsManager];
                        item[DatabaseObjects.Columns.IsIT] = row[DatabaseObjects.Columns.IsIT];
                        item[DatabaseObjects.Columns.IsConsultant] = row[DatabaseObjects.Columns.IsConsultant];
                        item[DatabaseObjects.Columns.WeekStartDate] = row[DatabaseObjects.Columns.WeekStartDate];
                        item[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                        item[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                        item[DatabaseObjects.Columns.PctPlannedAllocation] = row[DatabaseObjects.Columns.PctPlannedAllocation];
                        item[DatabaseObjects.Columns.PlannedAllocationHour] = row[DatabaseObjects.Columns.PlannedAllocationHour];
                        item[DatabaseObjects.Columns.ActualHour] = row[DatabaseObjects.Columns.ActualHour];
                        item[DatabaseObjects.Columns.PctActual] = row[DatabaseObjects.Columns.PctActual];
                        item[DatabaseObjects.Columns.FunctionalAreaTitle] = row[DatabaseObjects.Columns.FunctionalAreaTitle];
                        item[DatabaseObjects.Columns.Title] = row[DatabaseObjects.Columns.Title];

                        item.Update();
                    }
                    tempSummaryWeek.Rows.Clear();

                    // Delete leftover rows with no matching data
                    DataRow[] crapedSummaryRows = workSmmaryRowsWeek.Where(x => x.Field<DateTime>(DatabaseObjects.Columns.Modified) < updatedRowTime).ToArray();
                    foreach (DataRow crapRow in crapedSummaryRows)
                    {
                        SPListItem item = SPListHelper.GetSPListItem(summaryListWeek, (int)crapRow[DatabaseObjects.Columns.Id]);
                        item.Delete();
                    }
                }
                #endregion

                #region Add and edit ResourceUsageSummaryMonthWise List
                if (tempSummaryMonth != null && tempSummaryMonth.Rows.Count > 0)
                {
                    DateTime updatedRowTime = DateTime.Now;

                    foreach (DataRow row in tempSummaryMonth.Rows)
                    {
                        SPListItem item = null;
                        bool newEntry = false;
                        if (workSmmaryRowsMonth.Length > 0)
                        {
                            DataRow summaryRow = workSmmaryRowsMonth.FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(row[DatabaseObjects.Columns.WorkItemID]) &&
                                                                                           x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == Convert.ToDateTime(row[DatabaseObjects.Columns.MonthStartDate]).Date);
                            if (summaryRow != null)
                            {
                                item = SPListHelper.GetSPListItem(summaryListMonth, Convert.ToInt32(summaryRow[DatabaseObjects.Columns.Id]));
                                summaryRow[DatabaseObjects.Columns.Modified] = updatedRowTime;
                            }
                            else
                            {
                                newEntry = true;
                            }
                        }
                        else
                        {
                            newEntry = true;
                        }

                        if (newEntry)
                        {
                            item = summaryListMonth.AddItem();
                            item[DatabaseObjects.Columns.WorkItemID] = row[DatabaseObjects.Columns.WorkItemID];
                        }

                        item[DatabaseObjects.Columns.WorkItemType] = row[DatabaseObjects.Columns.WorkItemType];
                        item[DatabaseObjects.Columns.WorkItem] = row[DatabaseObjects.Columns.WorkItem];
                        item[DatabaseObjects.Columns.SubWorkItem] = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]), "[0-9]+;#", string.Empty);
                        item[DatabaseObjects.Columns.Resource] = row[DatabaseObjects.Columns.Resource];
                        item[DatabaseObjects.Columns.Manager] = row[DatabaseObjects.Columns.Manager];
                        item[DatabaseObjects.Columns.IsManager] = row[DatabaseObjects.Columns.IsManager];
                        item[DatabaseObjects.Columns.IsIT] = row[DatabaseObjects.Columns.IsIT];
                        item[DatabaseObjects.Columns.IsConsultant] = row[DatabaseObjects.Columns.IsConsultant];
                        item[DatabaseObjects.Columns.MonthStartDate] = row[DatabaseObjects.Columns.MonthStartDate];
                        item[DatabaseObjects.Columns.PctAllocation] = row[DatabaseObjects.Columns.PctAllocation];
                        item[DatabaseObjects.Columns.AllocationHour] = row[DatabaseObjects.Columns.AllocationHour];
                        item[DatabaseObjects.Columns.PctPlannedAllocation] = row[DatabaseObjects.Columns.PctPlannedAllocation];
                        item[DatabaseObjects.Columns.PlannedAllocationHour] = row[DatabaseObjects.Columns.PlannedAllocationHour];
                        item[DatabaseObjects.Columns.ActualHour] = row[DatabaseObjects.Columns.ActualHour];
                        item[DatabaseObjects.Columns.PctActual] = row[DatabaseObjects.Columns.PctActual];
                        item[DatabaseObjects.Columns.FunctionalAreaTitle] = row[DatabaseObjects.Columns.FunctionalAreaTitle];
                        item[DatabaseObjects.Columns.Title] = row[DatabaseObjects.Columns.Title];

                        item.Update();
                    }
                    tempSummaryMonth.Rows.Clear();

                    // Delete leftover rows with no matching data
                    DataRow[] crapedSummaryRows = workSmmaryRowsMonth.Where(x => x.Field<DateTime>(DatabaseObjects.Columns.Modified) < updatedRowTime).ToArray();
                    foreach (DataRow crapRow in crapedSummaryRows)
                    {
                        SPListItem item = SPListHelper.GetSPListItem(summaryListMonth, (int)crapRow[DatabaseObjects.Columns.Id]);
                        item.Delete();
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Populate ResourceAllocationMonthly Table to keep allocation by month
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="allocationID"></param>
        public static void DistributeAllocationByMonth(ApplicationContext spWeb, int workItemID)
        {
            string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID);
            DataRow[] rAllocationCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation).Select(query);
            DataTable rAllocationItems= rAllocationCollection.CopyToDataTable();

            if (rAllocationItems == null)
                return;

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(spWeb);
            Dictionary<DateTime, double> dataListByDates = new Dictionary<DateTime, double>();
            foreach (DataRow row in rAllocationItems.Rows)
            {
                DateTime startDate = Convert.ToDateTime(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.AllocationStartDate));
                DateTime endDate = Convert.ToDateTime(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.AllocationEndDate));
                double pctAllocation = Convert.ToDouble(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.PctAllocation));

                DateTime tempStartDate = startDate;
                while (tempStartDate <= endDate)
                {
                    DateTime tempS = new DateTime(tempStartDate.Year, tempStartDate.Month,1);
                    DateTime tempEndDate = new DateTime(tempStartDate.Year, tempStartDate.Month, DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month));
                    if (tempStartDate.Year == endDate.Year && tempStartDate.Month == endDate.Month)
                        tempEndDate = endDate;

                    if (tempStartDate.Day == 1 && tempEndDate.Day == DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month))
                    {
                        if (!dataListByDates.ContainsKey(tempStartDate.Date))
                            dataListByDates.Add(tempStartDate, pctAllocation);
                    }
                    else if (!dataListByDates.ContainsKey(tempS.Date))
                    {
                        int workingDays = uHelper.GetTotalWorkingDaysBetween(tempStartDate, tempEndDate, false ,spWeb);
                        int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(tempS, new DateTime(tempEndDate.Year, tempEndDate.Month, DateTime.DaysInMonth(tempEndDate.Year, tempEndDate.Month)), spWeb);
                        double allctWorkingHrs = (workingDays * workingHrsInDay) * pctAllocation / 100;
                        double pctAllocationInMonth = (allctWorkingHrs * 100) / (workingDaysInMonth * workingHrsInDay);

                        dataListByDates.Add(tempS, pctAllocationInMonth);
                    }

                    tempStartDate = tempEndDate.AddDays(1);
                }
            }

            DateTime alldStartDate = dataListByDates.Keys.Min();
            DateTime alldEndDate = dataListByDates.Keys.Max();

            DataTable tempSummaryMonth = new DataTable();
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.MonthStartDate, typeof(DateTime));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PctAllocation, typeof(double));
            DateTime tempalldStartDate = alldStartDate;
            while (tempalldStartDate <= alldEndDate)
            {
                DataRow newRow = tempSummaryMonth.NewRow();
                tempSummaryMonth.Rows.Add(newRow);
                newRow[DatabaseObjects.Columns.MonthStartDate] = tempalldStartDate;
                newRow[DatabaseObjects.Columns.PctAllocation] = 0;
                if (dataListByDates.ContainsKey(tempalldStartDate))
                    newRow[DatabaseObjects.Columns.PctAllocation] = dataListByDates[tempalldStartDate];

                tempalldStartDate = tempalldStartDate.AddDays(DateTime.DaysInMonth(tempalldStartDate.Year, tempalldStartDate.Month));
            }

            
            DataTable ResourceAllocationMonthlyList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly);
            SPQuery mQuery = new SPQuery();
            mQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID);
            SPListItemCollection collection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceAllocationMonthly, mQuery, spWeb);

            int tempCount = 0;
            bool allowUnsafe = spWeb.AllowUnsafeUpdates;
            spWeb.AllowUnsafeUpdates = true;
            for (int i = 0; i < tempSummaryMonth.Rows.Count;i++)
            {
                SPListItem item = null;
                if (i < collection.Count)
                {
                    item = collection[i];
                    tempCount++;
                }
                else
                {
                    item = ResourceAllocationMonthlyList.AddItem();
                    item[DatabaseObjects.Columns.PctPlannedAllocation] = 0;
                }
                item[DatabaseObjects.Columns.Resource] = rAllocationCollection[0][DatabaseObjects.Columns.Resource];
                item[DatabaseObjects.Columns.ResourceWorkItemLookup] = workItemID;
                item[DatabaseObjects.Columns.PctAllocation] = tempSummaryMonth.Rows[i][DatabaseObjects.Columns.PctAllocation];
                item[DatabaseObjects.Columns.MonthStartDate] = Convert.ToDateTime(tempSummaryMonth.Rows[i][DatabaseObjects.Columns.MonthStartDate]);

                try
                {
                    item.Update();
                }
                catch (Exception)
                {
                    // Ignore known exception due to event receiver getting fired multiple times
                    //Log.WriteException(ex);
                }
            }

            spWeb.AllowUnsafeUpdates = allowUnsafe;

            List<int> deleteItemIDs = new List<int>();
            //Delete extra entries in month view list
            if (tempCount < collection.Count)
            {
                for (int i = tempCount; i < collection.Count; i++)
                {
                    deleteItemIDs.Add(collection[i].ID);
                }
            }

            // new function for delete junk entry from ResourceAllocationMonhtly
            SPQuery ramQuery = new SPQuery();
            ramQuery.Query = string.Format("<Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>", DatabaseObjects.Columns.ResourceWorkItemLookup);
            DataTable dtJunkResourceAllocationMonthly = SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceAllocationMonthly, ramQuery, spWeb);
            if (dtJunkResourceAllocationMonthly != null && dtJunkResourceAllocationMonthly.Rows.Count > 0)
            {
                foreach (DataRow dr in dtJunkResourceAllocationMonthly.Rows)
                {
                    deleteItemIDs.Add(Convert.ToInt32(dr[DatabaseObjects.Columns.Id]));
                }
            }

            if (deleteItemIDs.Count > 0)
            {
                deleteItemIDs = deleteItemIDs.Distinct().ToList();
                BatchDeleteListItems(deleteItemIDs, DatabaseObjects.Tables.ResourceAllocationMonthly, spWeb.Url);
            }
        }

        public static DataTable GetMonthlyDistributedAllocations(ApplicationContext spWeb, List<int> workItemIDs)
        {
            DataTable data = null;
            SPList monthlyAllocationList = SPListHelper.GetSPList(DatabaseObjects.Tables.ResourceAllocationMonthly, spWeb);

            SPSiteDataQuery query = new SPSiteDataQuery();
            List<string> expressions = new List<string>();
            if (workItemIDs.Count > 0)
            {
                foreach (int id in workItemIDs)
                {
                    expressions.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.ResourceWorkItemLookup, id));
                }
                string mergeExp = uHelper.GenerateWhereQueryWithAndOr(expressions, expressions.Count - 1, false);
                expressions = new List<string>();
                expressions.Add(mergeExp);
            }
            SPQuery dd = new SPQuery();
     
            //expressions.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'><Value Type='User'>{1}</Value></Eq>", DatabaseObjects.Columns.Resource, userID));
            query.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(expressions, expressions.Count - 1, true));
            List<string> viewFields = new List<string>();
            viewFields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.ResourceWorkItemLookup));
            viewFields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Resource));
            viewFields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.MonthStartDate));
            viewFields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.PctAllocation));
            viewFields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Title));
            query.ViewFields = string.Join("", viewFields.ToArray());
            query.Lists = string.Format("<Lists Hidden='True'><List ID='{0}' /></Lists>", monthlyAllocationList.ID);

            DataTable rawData =  spWeb.GetSiteData(query);
            if (rawData == null || rawData.Rows.Count <= 0)
                return null;

            ILookup<string, DataRow>  dataLookups = rawData.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup));
            List<DateTime> selectedMonths = new List<DateTime>();
            if (dataLookups.Count > 0)
            {
                data = new DataTable();
                foreach (var dLookup in dataLookups)
                {
                    DataRow[] dataRows = dLookup.ToArray();
                    DataRow dtRow = data.NewRow();
                    data.Rows.Add(dtRow);
                    foreach (DataRow sRow in dataRows)
                    {
                        DateTime rowDate = Convert.ToDateTime(sRow[DatabaseObjects.Columns.MonthStartDate]);
                        if (!selectedMonths.Exists(x => x.Date == rowDate.Date))
                        {
                            selectedMonths.Add(rowDate.Date);
                            data.Columns.Add(rowDate.ToString("MMM/dd/yyyy"), typeof(double));
                        }
                        dtRow[rowDate.ToString("MMM/dd/yyyy")] = Math.Round(Convert.ToDouble(sRow[DatabaseObjects.Columns.PctAllocation]), 0);
                    }
                }
            }

            return data;
        }

        public static void UpdateRMMSummaryAndMonthDistribution(int workItemID, string spWebUrl,ApplicationContext spWeb)
        {
          
                    DistributeAllocationByMonth(spWeb, workItemID);
                    UpdateRMMAllocationSummary(workItemID, spWeb);
               
        }

        public static void UpdateRMMMonthDistributionForUnassigned(ResourceAllocation rAlloc, string spWebUrl, ApplicationContext spWeb)
        {
            //rAlloc.SaveWorkItem(spWeb);
            DistributeUnassignedAllocationByMonth(spWeb, rAlloc);
        }

        /// <summary>
        /// Populate ResourceAllocationMonthly Table to keep allocation by month
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="allocationID"></param>
        public static void DistributeUnassignedAllocationByMonth(ApplicationContext spWeb, ResourceAllocation rAlloc)
        {
            try
            {
                int workItemId = rAlloc.SaveWorkItem(spWeb);
                if (workItemId > 0)
                {
                    rAlloc.WorkItemID = workItemId;
                    //new temp line for testing point of view..
                   // rAlloc.SaveAllocation(spWeb);

                    int workingHrsInDay = uHelper.GetWorkingHoursInADay(spWeb);
                    Dictionary<DateTime, double> dataListByDates = new Dictionary<DateTime, double>();
                    //foreach (DataRow row in rAllocationItems.Rows)
                    //{
                    DateTime startDate = rAlloc.StartDate; // Convert.ToDateTime(uHelper.GetSPItemValue(row, DatabaseObjects.Columns.AllocationStartDate));
                    DateTime endDate = rAlloc.EndDate;  //Convert.ToDateTime(uHelper.GetSPItemValue(row, DatabaseObjects.Columns.AllocationEndDate));
                    double pctAllocation = rAlloc.PctAllocation; // Convert.ToDouble(uHelper.GetSPItemValue(row, DatabaseObjects.Columns.PctAllocation));

                    DateTime tempStartDate = startDate;
                    while (tempStartDate <= endDate)
                    {
                        DateTime tempS = new DateTime(tempStartDate.Year, tempStartDate.Month, 1);
                        DateTime tempEndDate = new DateTime(tempStartDate.Year, tempStartDate.Month, DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month));
                        if (tempStartDate.Year == endDate.Year && tempStartDate.Month == endDate.Month)
                            tempEndDate = endDate;

                        if (tempStartDate.Day == 1 && tempEndDate.Day == DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month))
                        {
                            if (!dataListByDates.ContainsKey(tempStartDate.Date))
                                dataListByDates.Add(tempStartDate, pctAllocation);
                        }
                        else if (!dataListByDates.ContainsKey(tempS.Date))
                        {
                            int workingDays = uHelper.GetTotalWorkingDaysBetween(tempStartDate, tempEndDate, false, spWeb);
                            int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(tempS, new DateTime(tempEndDate.Year, tempEndDate.Month, DateTime.DaysInMonth(tempEndDate.Year, tempEndDate.Month)), spWeb);
                            double allctWorkingHrs = (workingDays * workingHrsInDay) * pctAllocation / 100;
                            double pctAllocationInMonth = (allctWorkingHrs * 100) / (workingDaysInMonth * workingHrsInDay);

                            dataListByDates.Add(tempS, pctAllocationInMonth);
                        }

                        tempStartDate = tempEndDate.AddDays(1);
                    }
                    //}

                    DateTime alldStartDate = dataListByDates.Keys.Min();
                    DateTime alldEndDate = dataListByDates.Keys.Max();

                    DataTable tempSummaryMonth = new DataTable();
                    tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.MonthStartDate, typeof(DateTime));
                    tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.PctAllocation, typeof(double));
                    DateTime tempalldStartDate = alldStartDate;
                    while (tempalldStartDate <= alldEndDate)
                    {
                        DataRow newRow = tempSummaryMonth.NewRow();
                        tempSummaryMonth.Rows.Add(newRow);
                        newRow[DatabaseObjects.Columns.MonthStartDate] = tempalldStartDate;
                        newRow[DatabaseObjects.Columns.PctAllocation] = 0;
                        if (dataListByDates.ContainsKey(tempalldStartDate))
                            newRow[DatabaseObjects.Columns.PctAllocation] = dataListByDates[tempalldStartDate];

                        tempalldStartDate = tempalldStartDate.AddDays(DateTime.DaysInMonth(tempalldStartDate.Year, tempalldStartDate.Month));
                    }


                    SPList ResourceAllocationMonthlyList = SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceAllocationMonthly, spWeb);
                    SPQuery mQuery = new SPQuery();
                    mQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ResourceWorkItemLookup, rAlloc.WorkItemID);
                    SPListItemCollection collection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceAllocationMonthly, mQuery, spWeb);

                    int tempCount = 0;
                    bool allowUnsafe = spWeb.AllowUnsafeUpdates;
                    spWeb.AllowUnsafeUpdates = true;
                    for (int i = 0; i < tempSummaryMonth.Rows.Count; i++)
                    {
                        SPListItem item = null;
                        if (i < collection.Count)
                        {
                            item = collection[i];
                            tempCount++;
                        }
                        else
                        {
                            item = ResourceAllocationMonthlyList.AddItem();
                            item[DatabaseObjects.Columns.PctPlannedAllocation] = 0;
                        }
                        item[DatabaseObjects.Columns.Resource] = rAlloc.ResourceId; // rAllocationCollection[0][DatabaseObjects.Columns.Resource];
                        item[DatabaseObjects.Columns.ResourceWorkItemLookup] = rAlloc.WorkItemID;
                        item[DatabaseObjects.Columns.PctAllocation] = tempSummaryMonth.Rows[i][DatabaseObjects.Columns.PctAllocation];
                        item[DatabaseObjects.Columns.MonthStartDate] = Convert.ToDateTime(tempSummaryMonth.Rows[i][DatabaseObjects.Columns.MonthStartDate]);
                        item.Update();
                    }

                    spWeb.AllowUnsafeUpdates = allowUnsafe;

                    List<int> deleteItemIDs = new List<int>();
                    //Delete extra entries in month view list
                    if (tempCount < collection.Count)
                    {
                        for (int i = tempCount; i < collection.Count; i++)
                        {
                            deleteItemIDs.Add(collection[i].ID);
                        }
                    }

                    // new function for delete junk entry from ResourceAllocationMonhtly
                    SPQuery ramQuery = new SPQuery();
                    ramQuery.Query = string.Format("<Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>", DatabaseObjects.Columns.ResourceWorkItemLookup);
                    DataTable dtJunkResourceAllocationMonthly = SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceAllocationMonthly, ramQuery, spWeb);
                    if (dtJunkResourceAllocationMonthly != null && dtJunkResourceAllocationMonthly.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtJunkResourceAllocationMonthly.Rows)
                        {
                            deleteItemIDs.Add(Convert.ToInt32(dr[DatabaseObjects.Columns.Id]));
                        }
                    }

                    if (deleteItemIDs.Count > 0)
                    {
                        deleteItemIDs = deleteItemIDs.Distinct().ToList();
                        BatchDeleteListItems(deleteItemIDs, DatabaseObjects.Lists.ResourceAllocationMonthly, spWeb.Url);
                    }
                }
            }
            catch { }
        }

        public static void DeleteRMMSummaryAndMonthDistribution(int workItemID, string spWebUrl, ApplicationContext spWeb)
        {
            DeleteMonthlyDistributions(spWeb, workItemID);
            UpdateRMMAllocationSummary(workItemID, spWeb);
        }
        public static void DeleteAllProjectAllocationsByModule(string module, string spWebUrl,ApplicationContext spWeb)
        {  // For resource monthly
            DataTable dtMonthlyAllocation = SPListHelper.GetDataTable(DatabaseObjects.Tables.ResourceAllocationMonthly, spWeb);
            DataRow[] drs = dtMonthlyAllocation != null ? dtMonthlyAllocation.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

            if (drs != null)
            {
                List<int> resourceMonthlyIds = new List<int>();
                foreach (DataRow dr in drs)
                {
                    resourceMonthlyIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                }
                if (resourceMonthlyIds.Count > 0)
                {
                    BatchDeleteListItems(resourceMonthlyIds, DatabaseObjects.Lists.ResourceAllocationMonthly, spWebUrl);
                }
            }


            // For resource monthly summary
            DataTable dtMonthlySummary = SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, spWeb);
            drs = dtMonthlySummary != null ? dtMonthlySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

            if (drs != null)
            {
                List<int> resourceMonthlySummaryIds = new List<int>();
                foreach (DataRow dr in drs)
                {
                    resourceMonthlySummaryIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                }
                if (resourceMonthlySummaryIds.Count > 0)
                {
                    BatchDeleteListItems(resourceMonthlySummaryIds, DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, spWebUrl);
                }
            }
            // For resource weekly summary
            DataTable dtWeeklySummary = SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWeb);
            drs = dtWeeklySummary != null ? dtWeeklySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;
            if (drs != null)
            {
                List<int> resourceWeeklySummaryIds = new List<int>();
                foreach (DataRow dr in drs)
                {
                    resourceWeeklySummaryIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                }
                if (resourceWeeklySummaryIds.Count > 0)
                {
                    BatchDeleteListItems(resourceWeeklySummaryIds, DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWebUrl);
                }
            }


        }

        public static void DeleteAllAllocations(string spWebUrl,ApplicationContext spWeb)
        {
            

                    // For resource monthly
                    DataTable dtMonthlyAllocation =GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly);

                    if (dtMonthlyAllocation != null)
                    {
                        List<int> resourceMonthlyIds = new List<int>();
                        foreach (DataRow dr in dtMonthlyAllocation.Rows)
                        {
                            resourceMonthlyIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                        }
                        if (resourceMonthlyIds.Count > 0)
                        {
                            BatchDeleteListItems(resourceMonthlyIds, DatabaseObjects.Lists.ResourceAllocationMonthly, spWebUrl);
                        }
                    }


                    // For resource monthly summary
                    DataTable dtMonthlySummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise);
                    //drs = dtMonthlySummary != null ? dtMonthlySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

                    if (dtMonthlySummary != null)
                    {
                        List<int> resourceMonthlySummaryIds = new List<int>();
                        foreach (DataRow dr in dtMonthlySummary.Rows)
                        {
                            resourceMonthlySummaryIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                        }
                        if (resourceMonthlySummaryIds.Count > 0)
                        {
                            BatchDeleteListItems(resourceMonthlySummaryIds, DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, spWebUrl);
                        }
                    }


                    // For resource weekly summary
                    DataTable dtWeeklySummary = SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWeb);
                   // drs = dtWeeklySummary != null ? dtWeeklySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

                    if (dtWeeklySummary != null)
                    {
                        List<int> resourceWeeklySummaryIds = new List<int>();
                        foreach (DataRow dr in dtWeeklySummary.Rows)
                        {
                            resourceWeeklySummaryIds.Add(uHelper.StringToInt(dr[DatabaseObjects.Columns.Id]));
                        }
                        if (resourceWeeklySummaryIds.Count > 0)
                        {
                            BatchDeleteListItems(resourceWeeklySummaryIds, DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWebUrl);
                        }
                    }

               
        }

        public static void BatchDeleteListItems(List<int> ids, string listName, string spWebUrl)
        {
            if (ids != null && ids.Count > 0)
            {
                DataTable list = GetTableDataManager.GetTableData(listName);
                string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
                string updateMethodFormat = "<Method ID=\"{0}\">" +
                 "<SetList>{1}</SetList>" +
                 "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                 "<SetVar Name=\"ID\">{2}</SetVar>" +
                 "</Method>";

                StringBuilder queryBuilder = new StringBuilder();
                foreach (int id in ids)
                {
                    queryBuilder.AppendFormat(updateMethodFormat, id, list.ID, id);
                }

                string batch = string.Format(batchFormat, queryBuilder.ToString());
                //spWeb.AllowUnsafeUpdates = true;
                string batchReturn = spWeb.ProcessBatchData(batch);
                //spWeb.AllowUnsafeUpdates = false;
            }       
        }

        public static void DeleteMonthlyDistributions(ApplicationContext spWeb, int workItemID)
        {
           string query =  string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID);
            DataRow[] rAllocationMCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly).Select(query);
            DataTable rAllocationMItems = rAllocationMCollection.CopyToDataTable();
            if (rAllocationMItems == null || rAllocationMItems.Rows.Count == 0)
                return;

            DataTable questionList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly);
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            string updateMethodFormat = "<Method ID=\"{0}\">" +
             "<SetList>{1}</SetList>" +
             "<SetVar Name=\"Cmd\">Delete</SetVar>" +
             "<SetVar Name=\"ID\">{2}</SetVar>" +
             "</Method>";

            StringBuilder queryBuilder = new StringBuilder();
            foreach (DataRow row in rAllocationMItems.Rows)
            {
                queryBuilder.AppendFormat(updateMethodFormat, row[DatabaseObjects.Columns.Id], questionList.ID, row[DatabaseObjects.Columns.Id]);
            }

            string batch = string.Format(batchFormat, queryBuilder.ToString());
            string batchReturn = spWeb.ProcessBatchData(batch);

        }

        public static void DeleteAllocationsByTasks(ApplicationContext spWeb,string ProjectPublicID, bool deleteWorkItems)
        {
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems);
            DataRow[] drs = dt != null ? dt.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItem, ProjectPublicID)) : null;
            if (drs != null && drs.Length > 0)
            {
                List<int> workItemIds = new List<int>();
                List<int> allocationIds = new List<int>();
                List<int> allocationMonthlyIds = new List<int>();
                List<int> allocationMonthwiseIds = new List<int>();
                List<int> allocationWeekwiseIds = new List<int>();

                string filterqry = "<Where><In><FieldRef Name=\"ResourceWorkItemLookup\" /><Values>";
              
                StringBuilder qry = new StringBuilder();


                foreach (DataRow dr in drs)
                {
                    workItemIds.Add(UGITUtility.StringToInt(dr[DatabaseObjects.Columns.Id]));
                    filterqry += "<Value Type=\"Lookup\">" + dr[DatabaseObjects.Columns.Id] + "</Value>";
                }

                filterqry += "</Values></In></Where>";
                DataRow[] spListCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation).Select(filterqry);
                DataRow[] spTimeSheetCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet).Select( filterqry);

                if (spTimeSheetCollection != null && spTimeSheetCollection.Count() > 0)
                {
                    List<int> lstResourceSheetWorkItemIds = new List<int>();
                    foreach (DataRow itemTimeSheet in spTimeSheetCollection)
                    {
                        lstResourceSheetWorkItemIds.Add(Convert.ToInt32(UGITUtility.GetSPItemValue(itemTimeSheet,DatabaseObjects.Columns.ResourceWorkItemLookup)));
                    }

                    workItemIds = workItemIds.Where(x => !lstResourceSheetWorkItemIds.Contains(x)).ToList();
                }
                foreach (DataRow item in spListCollection)
                {
                    allocationIds.Add(UGITUtility.StringToInt(item[DatabaseObjects.Columns.Id]));
                }

               


                DataTable dtResourceAllocationMonthly = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly);
                DataRow[] drResourceAllocationMonthly = dtResourceAllocationMonthly != null ? dtResourceAllocationMonthly.Select(string.Format("{0} IN ('{1}')", DatabaseObjects.Columns.ResourceWorkItemLookup, string.Join("','", workItemIds.ToArray()))) : null;
                if (drResourceAllocationMonthly != null)
                {
                    foreach (DataRow dr in drResourceAllocationMonthly)
                    {
                        allocationMonthlyIds.Add(UGITUtility.StringToInt(dr[DatabaseObjects.Columns.Id]));
                    }
                }
              
              

                DataTable dtResourceUsageSummaryMonthWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise);
                DataRow[] drResourceUsageSummaryMonthWise = dtResourceUsageSummaryMonthWise != null ? dtResourceUsageSummaryMonthWise.Select(string.Format("{0} IN ('{1}')", DatabaseObjects.Columns.WorkItemID, string.Join("','", workItemIds.ToArray()))) : null;
                if (drResourceUsageSummaryMonthWise != null)
                {
                    foreach (DataRow dr in drResourceUsageSummaryMonthWise)
                    {
                        allocationMonthwiseIds.Add(UGITUtility.StringToInt(dr[DatabaseObjects.Columns.Id]));
                    }
                }

                DataTable dtResourceUsageSummaryWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise);
                DataRow[] drResourceUsageSummaryWeekWise = dtResourceUsageSummaryWeekWise != null ? dtResourceUsageSummaryWeekWise.Select(string.Format("{0} IN ('{1}')", DatabaseObjects.Columns.WorkItemID, string.Join("','", workItemIds.ToArray()))) : null;
                if (drResourceUsageSummaryWeekWise != null)
                {
                    foreach (DataRow dr in drResourceUsageSummaryWeekWise)
                    {
                        allocationWeekwiseIds.Add(UGITUtility.StringToInt(dr[DatabaseObjects.Columns.Id]));
                    }
                }

                if (deleteWorkItems) // only delete if deleting task
                {
                    BatchDeleteListItems(workItemIds, DatabaseObjects.Tables.ResourceWorkItems,HttpContext.Current.Request.Url.ToString());
                }

                BatchDeleteListItems(allocationIds, DatabaseObjects.Tables.ResourceAllocation, HttpContext.Current.Request.Url.ToString());
                BatchDeleteListItems(allocationMonthlyIds, DatabaseObjects.Tables.ResourceAllocationMonthly, HttpContext.Current.Request.Url.ToString());
                BatchDeleteListItems(allocationMonthwiseIds, DatabaseObjects.Tables.ResourceUsageSummaryMonthWise, HttpContext.Current.Request.Url.ToString());
                BatchDeleteListItems(allocationWeekwiseIds, DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, HttpContext.Current.Request.Url.ToString());

            }
        }

        public static void DeleteAllocationsByTask(ApplicationContext spWeb,List<UGITTask> tasks, List<int> users, string moduleName, string projectPublicID)
        {
            List<ResourceAllocation> allocations = new List<ResourceAllocation>(); // ResourceAllocation.LoadByWorkItem(spWeb, moduleName, projectPublicID, null, 4, false, true);
            Dictionary<string, ResourceAllocation> updatedRA = new Dictionary<string, ResourceAllocation>();
            users = users.Distinct().ToList();

            DataRow projectItem = Ticket.getCurrentTicket( spWeb, moduleName, projectPublicID);

            DateTime projectStartDate = DateTime.MinValue;
            DateTime projectEndDate = DateTime.MinValue;

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualStartDate) && projectItem[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                projectStartDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualStartDate]);

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualCompletionDate) && projectItem[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                projectEndDate = Convert.ToDateTime(projectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(spWeb);

            if (users != null)
            {
                #region existing assign users

                foreach (int user in users)
                {
                    List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Exists(y => y.LookupId == user)).ToList();

                    // double pctWorkingHrs = 0;
                    double totalPorjectWorkingHrs = 0;
                    double totalUserProjectHrs = 0;
                    double pctAllocation = 0;
                    foreach (UGITTask task in userTasks)
                    {
                        double totalPercentage = 0;
                        // bool isCheckPercentage = false;
                        List<UGITAssignTo> listAssignToPct = UGITTaskHelper.GetUGITAssignPct(task.AssignToPct);

                        if (listAssignToPct.Count > 0)
                            totalPercentage = listAssignToPct.Sum(x => Convert.ToDouble(x.Percentage));

                        foreach (UGITAssignTo ugitAssignToItem in listAssignToPct)
                        {
                            SPUser spUser = UserProfile.GetUserById(user, spWeb);
                            if (spUser != null && ugitAssignToItem.LoginName == spUser.LoginName && totalPercentage > 0)
                            {
                                DateTime sDate = task.StartDate;
                                DateTime eDate = task.DueDate;
                                if (projectStartDate.Date > sDate.Date)
                                    sDate = projectStartDate.Date;
                                if (projectEndDate.Date < eDate.Date)
                                    eDate = projectEndDate.Date;

                                int taskWorkingDaysPctAllocation = uHelper.GetTotalWorkingDaysBetween(sDate, eDate, spWeb);
                                totalUserProjectHrs += (Convert.ToDouble(ugitAssignToItem.Percentage) * taskWorkingDaysPctAllocation * workingHrsInDay) / totalPercentage;
                            }
                        }
                    }

                    int taskWorkingDays = uHelper.GetTotalWorkingDaysBetween(projectStartDate, projectEndDate, spWeb);
                    totalPorjectWorkingHrs += taskWorkingDays * workingHrsInDay;

                    ResourceAllocation allocation = allocations.FirstOrDefault(x => x.ResourceId == user);
                    if (allocation == null)
                    {
                        allocation = new ResourceAllocation();
                        allocation.WorkItem = new ResourceWorkItem(user);
                        allocation.WorkItem.Level1 = moduleName;
                        allocation.WorkItem.Level2 = projectPublicID;
                        allocation.WorkItem.Level3 = string.Empty;
                        allocation.ResourceId = user;
                    }

                    // calculate the project allocation for user.
                    pctAllocation = (totalUserProjectHrs * 100) / totalPorjectWorkingHrs;
                    allocation.PctAllocation = (int)pctAllocation;

                    allocation.StartDate = projectStartDate;
                    allocation.EndDate = projectEndDate;


                    //calculate the user utilization.
                    double totalProjectLeafTaskHrs = 0;
                    totalProjectLeafTaskHrs = tasks.Where(x => x.ChildCount == 0).Sum(x => x.EstimatedHours);
                    if (totalProjectLeafTaskHrs > 0)
                    {
                        allocation.PlannedPctAllocation = totalUserProjectHrs * 100 / totalProjectLeafTaskHrs;
                    }

                    if (allocation.PctAllocation == 0 && (allocation.PlannedPctAllocation == 0 || totalProjectLeafTaskHrs ==0))
                        allocation.Delete(true,spWeb);
                    else
                    {
                        allocation.SaveAllocation(spWeb);
                        updatedRA.Add(string.Format("updated{0}", user), allocation);
                        //Start Thread to update rmm summary list w.r.t current workitem
                        ThreadStart threadStartMethodMonthDistribution = delegate() { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(allocation.WorkItemID, spWeb.Url); };
                        Thread sThreadMonthDistribution = new Thread(threadStartMethodMonthDistribution);
                        sThreadMonthDistribution.IsBackground = true;
                        sThreadMonthDistribution.Start();
                    }
                }
                #endregion
            }


            #region unassignUser

            ResourceAllocation rAllocation = null;
            rAllocation = new ResourceAllocation();
            List<ResourceWorkItem> wItems = ResourceWorkItem.LoadWorkItemsById(spWeb, moduleName, projectPublicID, string.Empty);
            ResourceWorkItem newItems = wItems.FirstOrDefault(x => x.Level2 == projectPublicID && x.ResourceId == 0);

            if (newItems != null)
            {
                rAllocation.WorkItem = newItems;
            }
            else
            {
                rAllocation.WorkItem = new ResourceWorkItem(0);
                rAllocation.WorkItem.Level1 = moduleName;
                rAllocation.WorkItem.Level2 = projectPublicID;
            }

            rAllocation.StartDate = projectStartDate;
            rAllocation.EndDate = projectEndDate;

            rAllocation.ResourceId = 0;
            rAllocation.ResourceName = string.Empty;

            int projectWorkingDay = uHelper.GetTotalWorkingDaysBetween(projectStartDate, projectEndDate, spWeb);
            int totalWorkingHrs = (projectWorkingDay * workingHrsInDay);
            int pctAlloc = totalWorkingHrs > 0 ? (int)(tasks.Where(x => x.ChildCount == 0 && x.AssignToPct == string.Empty).Sum(x => x.EstimatedHours) * 100) / totalWorkingHrs : 0;
            if (rAllocation.PctAllocation != pctAlloc)
            {
                rAllocation.PctAllocation = pctAlloc;
                string webUrl = spWeb.Url;

                //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                //ParameterizedThreadStart threadstart = new ParameterizedThreadStart(delegate { RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(rAllocation, webUrl); });
                ThreadStart threadStartMethod = delegate() { RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(rAllocation, webUrl); }; //rAllocation.WorkItemID
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
            #endregion

        }
    }
}
