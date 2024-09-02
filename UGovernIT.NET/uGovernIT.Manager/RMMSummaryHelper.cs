using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.DAL.Store;
using System.Data.SqlClient;
using System.Web;
using uGovernIT.Manager.RMM;
using uGovernIT.Manager.Managers;
using uGovernIT.DAL;
using System.Drawing;
using Amazon.Runtime;

namespace uGovernIT.Helpers
{
    public class RMMSummaryHelper
    {

        private static void ViewTypeAllocation(string hndYear, string hdndisplayMode, DataTable data, DataRow newRow, DataRow[] dttemp, DataRow newTotalRow, bool Assigned = true)
        {
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

            foreach (DataRow rowitem in dttemp)
            {
                if (hdndisplayMode == "Yearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        yearquaAllocE += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 12;
                        yearquaAllocA += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 12;
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
                else if (hdndisplayMode == "HalfYearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            halfyearquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
                        }

                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6)
                        {
                            halfyearquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
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
                else if (hdndisplayMode == "Quarterly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hndYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 4)
                        {
                            quaterquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 3 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            quaterquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 10)
                        {
                            quaterquaAllocE3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else
                        {
                            quaterquaAllocE4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
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
                else if (hdndisplayMode == "Weekly")
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
                    DateTime rowitemMonthStartDate = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]);
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        DateTime AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        DateTime allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);
                        if (rowitemMonthStartDate >= AllocationMonthStartDate && rowitemMonthStartDate <= allocationEndDate)
                        {
                            if (data.Columns.Contains(rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]);
                                if (newTotalRow != null)
                                    newTotalRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(newTotalRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"]) + Math.Ceiling(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]));
                            }
                        }
                    }
                    if (Assigned)
                    {
                        if (data.Columns.Contains(rowitemMonthStartDate.ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "A"] = UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]);
                        }
                    }

                }
            }

        }



        public static DataTable GetAllocationData(ApplicationContext context, DateTime dateFrom, DateTime dateTo, string hdndisplayMode, string selectedCategory, string selecteddepartment,
    string SelectedUser, string selectedManager, long selectedfunctionalarea, bool viewself, string hndYear, UserProfile CurrentUser,
    bool chkIncludeClosed, bool btnexport, bool forReport = false)
        {
            ConfigurationVariableManager ConfigVariableMGR = new ConfigurationVariableManager(context);
            ResourceWorkItemsManager ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            ResourceAllocationManager ResourceAllocationManager = new ResourceAllocationManager(context);
            DataTable data = new DataTable();

            List<UserProfile> userProfiles = context.UserManager.GetUsersProfile();

            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Name, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Project, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PctEstimatedAllocation, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));
            data.Columns.Add("AllocationType", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationID, typeof(int));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleRelativePagePath, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            DataTable dtlist = new DataTable();
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear))
                {
                    hndYear = DateTime.Now.Year.ToString();
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }
            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;
            if (!string.IsNullOrEmpty(selectedManager) && selectedManager != "0")
            {
                lstUProfile = context.UserManager.GetUserByManager(selectedManager);
                //UserProfile newlstitem = ObjUserProfileManager.LoadById(selectedManager);
                UserProfile newlstitem = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(selectedManager));
                lstUProfile.Add(newlstitem);
                limitedUsers = true;
            }
            else
            {
                lstUProfile = context.UserManager.GetUsersProfile();
            }
            if (!string.IsNullOrEmpty(SelectedUser))
            {
                lstUProfile.Clear();
                UserProfile user = context.UserManager.GetUserById(SelectedUser);
                if (user != null)
                    lstUProfile.Add(user);
                limitedUsers = true;
            }
            bool isResourceAdmin = context.UserManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || context.UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            //allowAllocationForSelf = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
            string allowAllocationForSelf = ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf);
            bool allowAllocationViewAll = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();
            List<UserProfile> userEditPermisionList = null;
            if (!isResourceAdmin)
                userEditPermisionList = context.UserManager.LoadAuthorizedUsers(allowAllocationForSelf);

            if (!isResourceAdmin && !allowAllocationViewAll)
            {

                if (userEditPermisionList != null && userEditPermisionList.Count > 0)
                {
                    userIds.AddRange(userEditPermisionList.Select(x => x.Id));

                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);

                    userIds = userIds.Distinct().ToList();
                }

                if (viewself)
                {
                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);
                }
                if (userIds != null && userIds.Count > 0)
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 1);
                }
            }
            else
            {
                if (limitedUsers)
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();
                    //userIds.Clear();
                    //userIds.Add("a9a6495c-fe3d-4689-94c9-6b18442ac591");
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);
                }
                else
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 4, dateFrom, dateTo);
                    workitems = RMMSummaryHelper.GetOpenworkitems(context, chkIncludeClosed);
                }
            }
            //filter data based on closed check
            if (!chkIncludeClosed)
            {
                // Filter by Open Tickets.
                List<string> LstClosedTicketIds = new List<string>();
                //get closed ticket instead of open ticket and then filter all except closed ticket
                DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(context);
                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                }
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow[] dr = dtResult.AsEnumerable().Where(x => !LstClosedTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (dr != null && dr.Length > 0)
                        dtResult = dr.CopyToDataTable();
                    else
                        dtResult.Rows.Clear();
                }
            }
            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (hdndisplayMode == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView(context, dateFrom, dateTo);
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView(context, dateFrom, dateTo, chkIncludeClosed);

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
            UserProfile userDetails = null;

            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = null;
            //GetUsersProfile()
            DataTable dtAllModuleTickets = ticketManager.GetAllProjectTickets();

            List<string> lstSelectedDepartment = UGITUtility.ConvertStringToList(selecteddepartment, Constants.Separator6);
            List<string> departmentTempUserIds = lstUProfile.Where(a => lstSelectedDepartment.Exists(d => d == a.Department)).Select(x => x.Id).ToList();
            List<string> functionalTempUserIds = lstUProfile.Where(a => a.FunctionalArea != null && a.FunctionalArea == selectedfunctionalarea).Select(x => x.Id).ToList();
            List<string> managerTempUserIds = lstUProfile.Select(x => x.Id).ToList();
            DataTable dtResouceWiseTotals = data.Clone();
            DataRow newRow = null;
            DataRow newTotalRow = null;
            bool isTotalRowNew = false;
            DataRow[] drArray = null;

            foreach (DataRow dr in dtResult.Rows)
            {
                string userid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceId]);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                if (userDetails == null || !userDetails.Enabled)
                    continue;

                //filter...
                if (!string.IsNullOrEmpty(selecteddepartment))
                {
                    if (departmentTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (selectedfunctionalarea > 0)
                {
                    if (functionalTempUserIds.IndexOf(userid) == -1)
                        continue;
                }
                if (!string.IsNullOrEmpty(selectedManager))
                {
                    if (managerTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup])))
                    continue;

                newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;



                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr[DatabaseObjects.Columns.Resource]);
                }
                newRow[DatabaseObjects.Columns.Name] = userDetails.Name;
                if (forReport)
                {
                    //Initial value
                    isTotalRowNew = false;
                    //Find any existing Totals row or create a new one.
                    drArray = dtResouceWiseTotals.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, userid));
                    if (drArray.Length > 0)
                    {
                        newTotalRow = drArray[0];
                    }
                    else
                    {
                        isTotalRowNew = true;
                        newTotalRow = dtResouceWiseTotals.NewRow();
                        newTotalRow[DatabaseObjects.Columns.Id] = userid;
                        newTotalRow[DatabaseObjects.Columns.ItemOrder] = 1;
                        newTotalRow[DatabaseObjects.Columns.Title] = "Total";
                        newTotalRow[DatabaseObjects.Columns.Project] = "Total";
                        newTotalRow[DatabaseObjects.Columns.ModuleName] = "GroupTotalRow";
                    }
                    newTotalRow[DatabaseObjects.Columns.Resource] = newRow[DatabaseObjects.Columns.Resource];
                    newTotalRow[DatabaseObjects.Columns.Name] = userDetails.Name;
                }
                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    string workitemid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]);
                    DataRow[] filterworkitemrow = workitems.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, UGITUtility.GetLookupID(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]))));
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }

                if (drWorkItem != null && drWorkItem[DatabaseObjects.Columns.WorkItem] != null)
                {
                    string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                    string[] arrayModule = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    string moduleName = string.Empty;
                    if (UGITUtility.IsValidTicketID(workItem))
                        moduleName = uHelper.getModuleNameByTicketId(workItem);

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        if (arrayModule.Contains(moduleName) || arrayModule.Length == 0)
                        {

                            module = moduleManager.GetByName(moduleName);
                            if (module == null)
                                continue;

                            //check for active modules.
                            if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                                continue;
                            DataRow dataRow = null;
                            if (tempTicketCollection.ContainsKey(workItem))
                                dataRow = tempTicketCollection[workItem];
                            else
                            {
                                //dataRow = ticketManager.GetByTicketID(module, workItem, viewFields: new List<string>() { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Closed });
                                dataRow = dtAllModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == workItem);
                                if (dataRow != null)
                                {
                                    tempTicketCollection.Add(workItem, dataRow);
                                }
                            }


                            if (dataRow != null)
                            {
                                string ticketID = workItem;
                                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 50);
                                if (!string.IsNullOrEmpty(ticketID))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", ticketID, title);
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0}: {1}", ticketID, title);
                                    if (UGITUtility.StringToBoolean(dataRow["Closed"]))
                                    {
                                        newRow[DatabaseObjects.Columns.Closed] = true;
                                    }
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;// title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                //condition for add new column for breakup gantt chart...
                                if (data != null && data.Rows.Count > 0)
                                {
                                    string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}' AND {DatabaseObjects.Columns.Id}='{userid}'";
                                    DataRow[] row = data.Select(expression);

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];

                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate]) != "01-01-1753 00:00:00")
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' title='{3}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40), title ?? string.Empty);

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;


                                    if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                                        }
                                    }

                                    data.Rows.Add(newRow);
                                    //}
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' title='{3}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40), title ?? string.Empty);

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                                    if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                                            }
                                        }
                                    }
                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (arrayModule.Length > 0 && !arrayModule.Contains(Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType])))
                            continue;

                        if (data != null && data.Rows.Count > 0)
                        {
                            string expression = string.Format("{0}= '{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            DataRow[] row = data.Select(expression);

                            if (row != null && row.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate])))
                                        row[0][DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate])))
                                        row[0][DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                }

                                if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(hndYear, hdndisplayMode, data, row[0], dttemp, newTotalRow);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(hndYear, hdndisplayMode, data, row[0], dttemp, newTotalRow, false);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                                if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                                }

                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow["ProjectNameLink"] = workItem;
                                newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                                newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                    newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                }

                                if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow, false);
                                        }
                                    }
                                }
                                data.Rows.Add(newRow);
                            }
                        }
                        else
                        {
                            string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                            if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                            }
                            else
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            }

                            newRow[DatabaseObjects.Columns.TicketId] = workItem;
                            newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                            newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                            newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);
                            newRow["ProjectNameLink"] = workItem;

                            if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                            }

                            if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                            {
                                DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                if (dttemp != null && dttemp.Length > 0)
                                    ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow);
                            }
                            else
                            {
                                if (dtAllocLookups != null)
                                {
                                    DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                    {
                                        ViewTypeAllocation(hndYear, hdndisplayMode, data, newRow, dttemp, newTotalRow, false);

                                    }
                                }
                            }

                            data.Rows.Add(newRow);
                        }
                    }
                    if (forReport)
                    {
                        if (!string.IsNullOrEmpty(newRow[DatabaseObjects.Columns.AllocationStartDate].ToString()) && (string.IsNullOrEmpty(newTotalRow[DatabaseObjects.Columns.AllocationStartDate].ToString()) ||
                            Convert.ToDateTime(newTotalRow[DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                        {
                            newTotalRow[DatabaseObjects.Columns.AllocationStartDate] = newRow[DatabaseObjects.Columns.AllocationStartDate]; //Capture minimum start date
                        }
                        if (!string.IsNullOrEmpty(newRow[DatabaseObjects.Columns.AllocationEndDate].ToString()) && (string.IsNullOrEmpty(newTotalRow[DatabaseObjects.Columns.AllocationEndDate].ToString()) ||
                            Convert.ToDateTime(newTotalRow[DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate])))
                        {
                            newTotalRow[DatabaseObjects.Columns.AllocationEndDate] = newRow[DatabaseObjects.Columns.AllocationEndDate];
                        }

                        if (isTotalRowNew)
                            dtResouceWiseTotals.Rows.Add(newTotalRow);
                        else
                        {
                            drArray = data.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, userid));
                            if (drArray.Length > 1)
                            {
                                newTotalRow[DatabaseObjects.Columns.ItemOrder] = drArray.Length;
                            }
                        }
                    }
                }


            }
            DataView dvTotals = new DataView(dtResouceWiseTotals);
            dvTotals.RowFilter = DatabaseObjects.Columns.ItemOrder + " > 1";
            if (dtResouceWiseTotals != null && dvTotals.Count > 0)
                data.Merge(dvTotals.ToTable());

            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC, {2} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.Project);

            return data;

        }

        private static DataTable LoadAllocationMonthlyView(ApplicationContext context, DateTime dateFrom, DateTime dateTo, bool chkIncludeClosed)
        {
            try
            {
                string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
                DataTable dtAllocationMonthWise = null;
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                values.Add("@ModuleNames", ModuleNames);
                values.Add("@Fromdate", Convert.ToDateTime(dateFrom));
                values.Add("@Todate", Convert.ToDateTime(dateTo));
                values.Add("@Isclosed", chkIncludeClosed);
                dtAllocationMonthWise = uGITDAL.ExecuteDataSetWithParameters("usp_GetAllocationdata", values);

                return dtAllocationMonthWise;
            }
            catch (Exception)
            { }
            return null;
        }

        private static DataTable LoadAllocationWeeklySummaryView(ApplicationContext context, DateTime dateFrom, DateTime dateTo)
        {
            DateTime dtFrom = dateFrom;
            DateTime dtTo = dateTo;

            dtFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
            dtTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));

            string commQuery = string.Empty;
            ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            //commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo));
            commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo));

            DataTable dtAllocationWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'" + "AND " + commQuery);
            //List<ResourceUsageSummaryWeekWise> allocWeekly = allocationWeekWiseManager.Load(x => x.WeekStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.WeekStartDate.Value.Date <= Convert.ToDateTime(dateTo));
            //DataTable dtAllocationWeekWise = UGITUtility.ToDataTable<ResourceUsageSummaryWeekWise>(allocWeekly);
            return dtAllocationWeekWise;
        }

        public static void DistributionRMMAllocation(ApplicationContext context, double pctAllocation, int workingHours, UserProfile userInfo, DataRow workItem, RResourceAllocation workAllocationTable, ref List<ResourceUsageSummaryWeekWise> tempSummaryWeek, ref List<ResourceUsageSummaryMonthWise> tempSummaryMonth)
        {
            if (workItem == null)
                return;

            DistributionRMMAllocation(context, pctAllocation, workingHours, userInfo, workItem, workAllocationTable, ref tempSummaryWeek, ref tempSummaryMonth, true);
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
        public static void DistributionRMMAllocation(ApplicationContext context, double pctAllocation, int workingHours, UserProfile userInfo, DataRow workItem, RResourceAllocation allocatedWork, 
            ref List<ResourceUsageSummaryWeekWise> tempSummaryWeek, ref List<ResourceUsageSummaryMonthWise> tempSummaryMonth, bool option)
        {
            if (tempSummaryWeek == null)
                tempSummaryWeek = new List<ResourceUsageSummaryWeekWise>();
            if (tempSummaryMonth == null)
                tempSummaryMonth = new List<ResourceUsageSummaryMonthWise>();

            ProjectEstimatedAllocationManager resourceAllocationManager = new ProjectEstimatedAllocationManager(context);

            //double totalActualPctAllocation = 0;

            string monthAllocationQuery = string.Format("{0}='{1}' and {2}='{3}' and {4}='{5}' and {6}='{7}' and {8}='{9}' and {10}='{11}'", DatabaseObjects.Columns.Resource, allocatedWork.Resource, DatabaseObjects.Columns.TenantID, 
                context.TenantID, DatabaseObjects.Columns.ResourceWorkItem, allocatedWork.TicketID, DatabaseObjects.Columns.GlobalRoleID, allocatedWork.RoleId,
                DatabaseObjects.Columns.ActualStartDate, allocatedWork.AllocationStartDate, DatabaseObjects.Columns.ActualEndDate, allocatedWork.AllocationEndDate);
            
            DataRow[] monthDistributionTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocationMonthly, monthAllocationQuery).Select();
            

                DateTime startDate = UGITUtility.StringToDateTime(allocatedWork.AllocationStartDate);
                DateTime endDate = UGITUtility.StringToDateTime(allocatedWork.AllocationEndDate);
                double workPctPlndAlloation = 0;
                double.TryParse(Convert.ToString(allocatedWork.PctPlannedAllocation), out workPctPlndAlloation);


                DateTime tempStartDate = startDate;
                //Fill  weeksummary table for specified workitem
                #region TempSummaryWeek Table
                while (tempStartDate <= endDate)
                {
                    //Gets startdate of the week
                    DateTime weekStartDate = tempStartDate.Date.AddDays(-(int)tempStartDate.DayOfWeek + 1);

                    double pctAllocationForMonth = UGITUtility.StringToDouble(allocatedWork.PctAllocation);
                    double pctAllocationForNextMonth = UGITUtility.StringToDouble(allocatedWork.PctAllocation);
                    //if (monthDistributionTable != null)
                    //{
                    //    List<ProjectEstimatedAllocation> resourceAllocationObj = resourceAllocationManager.Load(x => x.AssignedTo == Convert.ToString(workItem[DatabaseObjects.Columns.Resource]) 
                    //        && x.TicketId == Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]) && x.AllocationStartDate <= tempStartDate 
                    //        && x.AllocationEndDate >= tempStartDate);
                    //    if (resourceAllocationObj != null && resourceAllocationObj.Count > 0)
                    //    {
                    //        double.TryParse(Convert.ToString(resourceAllocationObj.FirstOrDefault().PctAllocation), out totalActualPctAllocation);
                    //    }

                    //    DataRow allocatedForMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(weekStartDate.Year, weekStartDate.Month, 1).Date);
                    //    if (allocatedForMonth != null)
                    //        pctAllocationForMonth = UGITUtility.StringToDouble(Convert.ToString(allocatedForMonth[DatabaseObjects.Columns.PctAllocation]));

                    //    if (weekStartDate.AddDays(7).Month != weekStartDate.Month)
                    //    {
                    //        DataRow allocatedForNxtMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(weekStartDate.AddMonths(1).Year, weekStartDate.AddMonths(1).Month, 1).Date);
                    //        if (allocatedForNxtMonth != null)
                    //            pctAllocationForNextMonth = UGITUtility.StringToDouble(Convert.ToString(allocatedForNxtMonth[DatabaseObjects.Columns.PctAllocation]));
                    //    }
                    //}

                    ResourceUsageSummaryWeekWise summaryRow = new ResourceUsageSummaryWeekWise();


                    summaryRow.AllocationHour = 0;
                    summaryRow.PctAllocation = 0;
                    summaryRow.ActualHour = 0;
                    summaryRow.PctActual = 0;

                    summaryRow.WorkItemID = Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]);
                    summaryRow.WorkItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    summaryRow.WorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]);
                    summaryRow.SubWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                    summaryRow.GlobalRoleID = UGITUtility.ObjectToString(allocatedWork.RoleId);
                    //Fill Userinfo in summary table
                    if (userInfo != null)
                    {
                        summaryRow.Resource = userInfo.Id;
                        if (!string.IsNullOrEmpty(userInfo.ManagerID))
                            summaryRow.ManagerLookup = userInfo.ManagerID;
                        summaryRow.IsManager = userInfo.IsManager;
                        summaryRow.IsIT = userInfo.IsIT;
                        summaryRow.IsConsultant = userInfo.IsConsultant;
                        if (userInfo.FunctionalArea != null)
                            summaryRow.FunctionalAreaTitleLookup = Convert.ToString(userInfo.FunctionalArea);
                    }
                    summaryRow.WeekStartDate = weekStartDate;
                    tempSummaryWeek.Add(summaryRow);
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
                    double.TryParse(Convert.ToString(summaryRow.AllocationHour), out allocationHours);

                    double totalPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow.PctAllocation), out totalPctAllocation);

                    double plndAllocationHrs = 0;
                    double.TryParse(Convert.ToString(summaryRow.PlannedAllocationHour), out plndAllocationHrs);
                    double plndPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow.PctPlannedAllocation), out plndPctAllocation);

                    //Calculates hours in a day based on allocation %.

                    double totalHours = allocationHours;
                    DateTime tempEndDate = weekStartDate.AddDays(6);
                
                    double hoursInDay = (pctAllocation * workingHours) / 100;
                    //Calculates working day within week
                    int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(context, tempStartDate, tempEndDate > endDate.Date ? endDate.Date : tempEndDate);
                    int totalworkingDaysInWeek = uHelper.GetTotalWorkingDaysBetween(context, weekStartDate, tempEndDate);
                    //Calculates total hours allocated in a week.
                    totalHours += (hoursInDay * workingDayInWeek);
                    totalPctAllocation = (totalHours*100)/(totalworkingDaysInWeek * workingHours);

                    double plndAllct = 0;
                    double.TryParse(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), out plndAllct);
                    double plndHrsInDay = (workPctPlndAlloation * workingHours) / 100;
                    plndAllocationHrs += (plndHrsInDay * workingDayInWeek);
                    plndPctAllocation += workPctPlndAlloation;

                    summaryRow.AllocationHour = totalHours;
                    if (totalHours > 0)
                        summaryRow.PctAllocation = Math.Round(totalPctAllocation,2);
                    else
                        summaryRow.PctAllocation = 0;

                    summaryRow.PlannedAllocationHour = Convert.ToInt32(plndAllocationHrs);
                    if (plndAllocationHrs > 0)
                        summaryRow.PctPlannedAllocation = Math.Round(plndPctAllocation,2);
                    else
                        summaryRow.PctPlannedAllocation = 0;

                    //Set Next week start date. if it is greater then enddate then set enddate.
                    if (weekStartDate.Date.AddDays(7).Date > endDate.Date)
                    {
                        // Adding a days as before it was not adding a allocation into week wise table where start and end date both were same.
                        // Same change has been done the while loop, instend of '<' -> '<='
                        tempStartDate = endDate.AddDays(1);
                    }
                    else
                    {
                        tempStartDate = tempStartDate.AddDays(totalDaysInWeek);
                    }
                }

                #endregion

                tempStartDate = startDate;
                #region Fill tempSummaryMonth table
                while (tempStartDate <= endDate)
                {
                    double pctAllocationForMonth = 0;
                    DataRow allocatedForMonth = null;
                    if (monthDistributionTable != null)
                    {
                        allocatedForMonth = monthDistributionTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == new DateTime(tempStartDate.Year, tempStartDate.Month, 1).Date);
                        if (allocatedForMonth != null)
                        {
                            pctAllocationForMonth = UGITUtility.StringToDouble(Convert.ToString(allocatedForMonth[DatabaseObjects.Columns.PctAllocation]));
                        }
                    }

                    //Sets start date of month
                    DateTime monthStartDate = new DateTime(tempStartDate.Year, tempStartDate.Month, 1); ;

                    ResourceUsageSummaryMonthWise summaryRow = new ResourceUsageSummaryMonthWise();
                    tempSummaryMonth.Add(summaryRow);
                    summaryRow.AllocationHour = 0;
                    summaryRow.PctAllocation = 0;
                    summaryRow.ActualHour = 0;
                    summaryRow.PctActual = 0;

                    summaryRow.WorkItemID = Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]);
                    summaryRow.WorkItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    summaryRow.WorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]);
                    summaryRow.SubWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                    summaryRow.GlobalRoleID = UGITUtility.ObjectToString(allocatedWork.RoleId);

                    //Fill Userinfo in summary table
                    if (userInfo != null)
                    {
                        summaryRow.Resource = userInfo.Id;
                        if (userInfo.ManagerID != null)
                            summaryRow.ManagerLookup = userInfo.ManagerID;
                        summaryRow.IsManager = userInfo.IsManager;
                        summaryRow.IsIT = userInfo.IsIT;
                        summaryRow.IsConsultant = userInfo.IsConsultant;
                        if (userInfo.FunctionalArea != null)
                            summaryRow.FunctionalAreaTitleLookup = Convert.ToString(userInfo.FunctionalArea);
                    }

                    summaryRow.MonthStartDate = monthStartDate;

                    //Calculates totaldays in month
                    int totalDaysInMonth = (DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month) - tempStartDate.Day) + 1;
                    if (tempStartDate.Year == endDate.Year && tempStartDate.Month == endDate.Month)
                    {
                        totalDaysInMonth = (endDate.Day - tempStartDate.Day) + 1;
                    }

                    //Calculates working days in month
                    int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(context, monthStartDate, monthStartDate.AddDays(totalDaysInMonth - 1));

                    double allocationHours = 0;
                    double.TryParse(Convert.ToString(summaryRow.AllocationHour), out allocationHours);

                    double totalPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow.PctAllocation), out totalPctAllocation);

                    double plndAllocationHrs = 0;
                    double.TryParse(Convert.ToString(summaryRow.PlannedAllocationHour), out plndAllocationHrs);
                    double plndPctAllocation = 0;
                    double.TryParse(Convert.ToString(summaryRow.PctPlannedAllocation), out plndPctAllocation);

                    //Calculates hours in each days based on % allocation.
                    double hoursInDay = (pctAllocation * workingHours) / 100;

                    //Calculates total hours allocated in a month
                    double totalHours = allocationHours + (hoursInDay * workingDayInMonth);
                    totalPctAllocation = totalPctAllocation + pctAllocationForMonth;

                    double plndAllct = 0;
                    double.TryParse(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]), out plndAllct);
                    double plndHrsInDay = (workPctPlndAlloation * workingHours) / 100;
                    plndAllocationHrs += (plndHrsInDay * workingDayInMonth);
                    plndPctAllocation += workPctPlndAlloation;


                    summaryRow.AllocationHour = totalHours;
                    if (totalHours > 0)
                        summaryRow.PctAllocation = Math.Round(totalPctAllocation, 2);
                    else
                        summaryRow.PctAllocation = 0;

                    summaryRow.PlannedAllocationHour = Convert.ToInt32(plndAllocationHrs);
                    if (plndAllocationHrs > 0)
                        summaryRow.PctPlannedAllocation = Math.Round(plndPctAllocation,2);
                    else
                        summaryRow.PctPlannedAllocation = 0;

                    //Sets next month start date
                    tempStartDate = tempStartDate.AddDays(totalDaysInMonth);
                }
                #endregion
            

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
        public static void DistributionRMMActual(ApplicationContext context, int workingHours, UserProfile userInfo, List<ResourceWorkItems> workItemCollection, DateTime workDate, DataTable actualItemTable, ref DataTable tempSummaryWeek, ref DataTable tempSummaryMonth)
        {
            if (tempSummaryWeek == null)
                tempSummaryWeek = CreateTempSummaryWeek();
            if (tempSummaryMonth == null)
                tempSummaryMonth = CreateTempSummaryMonth();

            string workItemLookupString = string.Empty;

            FunctionalAreasManager objFunctionalAreasManager = new FunctionalAreasManager(context);
            UserProfileManager userManager = new UserProfileManager(context);
            UserProfile mgrInfo = null;

            List<FunctionalArea> functionalAreas = objFunctionalAreasManager.Load();
            FunctionalArea functionalArea = null;

            //Generate summary table for each workitem
            foreach (ResourceWorkItems workItem in workItemCollection)
            {
                workItemLookupString = string.Format("{0}{1}{0}", workItem.ID, Constants.Separator);

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
                    //DataRow[] drCollection = actualItemTable.AsEnumerable().Where(x => x.Field<long>(DatabaseObjects.Columns.ResourceWorkItemLookup) == Convert.ToInt64(workItemLookupString) && x.Field<DateTime>("WorkDate1") >= weekStartDate && x.Field<DateTime>("WorkDate1") <= weekStartDate.AddDays(6)).ToArray();
                    double totalHours = 0.00;
                    if (actualItemTable != null)
                    {
                        DataRow[] drCollection = actualItemTable.Select(string.Format("{0}={1} and {2}>=#{3}# and {2}<=#{4}#", DatabaseObjects.Columns.ResourceWorkItemLookup, UGITUtility.SplitString(workItemLookupString, Constants.Separator)[0], "WorkDate1", weekStartDate.ToString("MM-dd-yyyy"), weekStartDate.AddDays(6).ToString("MM-dd-yyyy")));
                        if (drCollection.Count() > 0)
                        {
                            totalHours = drCollection.Sum(x => x.Field<double>("HoursTaken1"));
                        }
                    }
                    int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(context, weekStartDate, weekStartDate.AddDays(6));

                    DataRow summaryRow = tempSummaryWeek.NewRow();
                    tempSummaryWeek.Rows.Add(summaryRow);
                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.WeekStartDate] = weekStartDate.Date;
                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem.ID;
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem.WorkItemType;
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem.WorkItem;
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem.SubWorkItem;


                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.Id;
                        summaryRow["ResourceNameUser"] = userInfo.Name;

                        if (!string.IsNullOrEmpty(userInfo.ManagerID))
                        {
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                            mgrInfo = userManager.GetUserById(userInfo.ManagerID);
                            if (mgrInfo != null)
                                summaryRow["ManagerName"] = mgrInfo.Name;
                        }

                        if (userInfo.FunctionalArea != null)
                        {
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                            functionalArea = functionalAreas.FirstOrDefault(x => x.ID == userInfo.FunctionalArea.Value);
                            if (functionalArea != null)
                                summaryRow[DatabaseObjects.Columns.FunctionalArea] = functionalArea.Title;
                        }

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
                    // actualItemTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == UGITUtility.SplitString(workItemLookupString, Constants.Separator)[0] && x.Field<DateTime>("WorkDate1") >= monthStartDate && x.Field<DateTime>("WorkDate1") <= monthStartDate.AddDays(DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month) - 1)).Sum(x => x.Field<double>("HoursTaken1"));
                    double totalHours = 0.00;
                    if (actualItemTable != null)
                    {
                        DataRow[] drCollection = actualItemTable.Select(string.Format("{0}={1} and {2}>=#{3}# and {2}<=#{4}#", DatabaseObjects.Columns.ResourceWorkItemLookup, UGITUtility.SplitString(workItemLookupString, Constants.Separator)[0], "WorkDate1", monthStartDate.ToString("MM-dd-yyyy"), monthStartDate.AddDays(DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month) - 1).ToString("MM-dd-yyyy")));
                        if (drCollection.Count() > 0)
                        {
                            totalHours = drCollection.Sum(x => x.Field<double>("HoursTaken1"));
                        }
                    }
                    int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(context, weekStartDate, weekStartDate.AddMonths(1).AddDays(-1));

                    DataRow summaryRow = tempSummaryMonth.NewRow();
                    tempSummaryMonth.Rows.Add(summaryRow);
                    summaryRow[DatabaseObjects.Columns.AllocationHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctAllocation] = 0;
                    summaryRow[DatabaseObjects.Columns.ActualHour] = 0;
                    summaryRow[DatabaseObjects.Columns.PctActual] = 0;

                    summaryRow[DatabaseObjects.Columns.MonthStartDate] = monthStartDate.Date;
                    summaryRow[DatabaseObjects.Columns.WorkItemID] = workItem.ID;
                    summaryRow[DatabaseObjects.Columns.WorkItemType] = workItem.WorkItemType;
                    summaryRow[DatabaseObjects.Columns.WorkItem] = workItem.WorkItem;
                    summaryRow[DatabaseObjects.Columns.SubWorkItem] = workItem.SubWorkItem;

                    if (userInfo != null)
                    {
                        summaryRow[DatabaseObjects.Columns.Resource] = userInfo.Id;
                        summaryRow["ResourceNameUser"] = userInfo.Name;

                        if (!string.IsNullOrEmpty(userInfo.ManagerID))
                        {
                            summaryRow[DatabaseObjects.Columns.Manager] = userInfo.ManagerID;
                            mgrInfo = userManager.GetUserById(userInfo.ManagerID);
                            if (mgrInfo != null)
                                summaryRow["ManagerName"] = mgrInfo.Name;
                        }

                        if (userInfo.FunctionalArea != null)
                        {
                            summaryRow[DatabaseObjects.Columns.FunctionalAreaTitle] = userInfo.FunctionalArea.Value;
                            functionalArea = functionalAreas.FirstOrDefault(x => x.ID == userInfo.FunctionalArea.Value);
                            if (functionalArea != null)
                                summaryRow[DatabaseObjects.Columns.FunctionalArea] = functionalArea.Title;
                        }
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
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            tempSummaryWeek.Columns.Add("ResourceNameUser", typeof(string));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.Manager, typeof(string));
            tempSummaryWeek.Columns.Add("ManagerName", typeof(string));
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.FunctionalAreaTitle);
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.FunctionalArea);
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
            tempSummaryWeek.Columns.Add(DatabaseObjects.Columns.ERPJobID);

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
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            tempSummaryMonth.Columns.Add("ResourceNameUser", typeof(string));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.Manager, typeof(string));
            tempSummaryMonth.Columns.Add("ManagerName", typeof(string));
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.FunctionalAreaTitle);
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.FunctionalArea);
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
            tempSummaryMonth.Columns.Add(DatabaseObjects.Columns.ERPJobID);
            return tempSummaryMonth;
        }

        public static void Delete(ApplicationContext context, DataTable summaryListID, List<int> spItems)
        {

            if (spItems.Count <= 0 || string.IsNullOrEmpty(summaryListID.TableName))
            {
                return;
            }

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Delete</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "</Method>";

            StringBuilder query = new StringBuilder();
            foreach (int itemID in spItems)
            {
                DataRow[] dataRow = summaryListID.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, itemID));
                if (dataRow != null && dataRow.Length > 0)
                    DeleteTnx(context, summaryListID.TableName, dataRow[0]);
                //query.AppendFormat(updateMethodFormat, itemID, summaryListID, itemID);
            }
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = spWeb.ProcessBatchData(batch);
        }

        /// <summary>
        /// Update actual hours in rmm summary list
        /// </summary>
        /// <param name="workItemsID">List of workitemid of which summary need to update</param>
        /// <param name="url">Web url</param>
        /// <param name="wStartDate">start date of week on which timesheet is filled.</param>
        public static void UpdateActualInRMMSummary(ApplicationContext context, List<long> workItemsID, string resourceID, DateTime wStartDate, bool isActualTimeImport = false)
        {
            ResourceUsageSummaryMonthWiseManager objMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            ResourceUsageSummaryWeekWiseManager objWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            FunctionalAreasManager objFunctionalAreasManager = new FunctionalAreasManager(context);
            ResourceAllocationManager resourceAllocationMGR = new ResourceAllocationManager(context);

            if (workItemsID.Count <= 0)
            {
                return;
            }

            //Gets distinct workitenid
            workItemsID = workItemsID.Distinct().ToList();

            //Calculate working dates and working hours/day
            int workingHours = uHelper.GetWorkingHoursInADay(context);

            //Get ResourceInformation from SiteUserInfoList list
            UserProfileManager userManager = new UserProfileManager(context);
            UserProfile userInfo = userManager.GetUserById(resourceID);
            if (userInfo == null)
                return;

            DataRow projectItem = null;
            List<FunctionalArea> functionalAreas = objFunctionalAreasManager.Load();
            FunctionalArea functionalArea = null;

            UserProfile mgrInfo = null;

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
            List<long> workItemsExp = new List<long>();
            List<string> workItemsIDExp = new List<string>();

            foreach (long wID in workItemsID)
            {
                workItemsLookupExp.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ResourceWorkItemLookup, wID));
                workItemsExp.Add(wID);
                workItemsIDExp.Add(string.Format("{0}={1}", DatabaseObjects.Columns.Id, wID));
            }
            string workItemExpression = " ( " + string.Join(" or ", workItemsExp) + " ) ";
            string workItemLookupExpression = " ( " + string.Join(" or ", workItemsLookupExp) + " ) ";
            string workItemIDExpression = " ( " + string.Join(" or ", workItemsIDExp) + " ) ";

            string workItemsQuery = workItemIDExpression;
            ResourceWorkItemsManager ObjresourceWorkItemsManager = new ResourceWorkItemsManager(context);
            List<ResourceWorkItems> workItemCollection = ObjresourceWorkItemsManager.Load().Where(x => workItemsExp.Contains(x.ID)).ToList();//GetTableData(DatabaseObjects.Tables.ResourceWorkItems).Select(workItemsQuery);

            string actuiQuery = string.Format("{0} and {1}>='{2}' and  {1}<='{3}'", workItemLookupExpression, DatabaseObjects.Columns.WorkDate, qStartDate.ToString("MM-dd-yyyy"), qEndDate.ToString("MM-dd-yyyy"));
            DataTable actualItemList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {actuiQuery}");

            DataTable actualItemTable = null;
            try
            {
                actualItemTable = actualItemList;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            DataTable tempSummaryWeek = null;
            DataTable tempSummaryMonth = null;

            RMMSummaryHelper.DistributionRMMActual(context, workingHours, userInfo, workItemCollection, wStartDate, actualItemTable, ref tempSummaryWeek, ref tempSummaryMonth);

            DataTable rmmMonthSummaryList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise, $"{DatabaseObjects.Columns.AllocationHour} <= 0 and  {DatabaseObjects.Columns.ActualHour} <= 0 and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataTable rmmWeekSummaryList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.AllocationHour} <= 0 and  {DatabaseObjects.Columns.ActualHour} <= 0 and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            string summaryMonthQuery = string.Format("{0} and {1}>={2} and {1}<={3}", workItemExpression, DatabaseObjects.Columns.MonthStartDate, monthStartDate.Date, weekEndDate.Date);
            string summaryWeekQuery = string.Format("{0} and {1}={2}", workItemExpression, DatabaseObjects.Columns.WeekStartDate, weekStartDate.Date);

            int rmmMonthSummaryTableCount = objMonthWiseManager.Count();
            int rmmWeekSummaryTableCount = objWeekWiseManager.Count();

            RResourceAllocation objResourceAllocation = null;
            //Update summary for each workItems
            foreach (ResourceWorkItems workItem in workItemCollection)
            {
                //Update Month summary
                if (tempSummaryMonth != null && tempSummaryMonth.Rows.Count > 0)
                {
                    DataRow[] workSummaryRows = tempSummaryMonth.AsEnumerable().Where(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(workItem.ID)).ToArray();
                    for (int i = 0; i < workSummaryRows.Length; i++)
                    {
                        DataRow workSummaryRow = workSummaryRows[i];
                        objResourceAllocation = null; 
                        //As discussed, this code is not required in case of importing Actual time.
                        //We need to insert records in summary table even if there is no corresponding allocation in ResourceAllocation table.
                        if (!isActualTimeImport)
                        {
                            objResourceAllocation = resourceAllocationMGR.Load(x => x.ResourceWorkItemLookup == UGITUtility.StringToLong(workSummaryRow[DatabaseObjects.Columns.WorkItemID])).FirstOrDefault();

                            if (objResourceAllocation == null)
                                continue;
                        }
                        DateTime mStartDate = UGITUtility.StringToDateTime(workSummaryRow[DatabaseObjects.Columns.MonthStartDate]);
                        ResourceUsageSummaryMonthWise monthSummaryItem = null;
                        bool addNewMonthSummary = false;

                        try
                        {

                            if (rmmMonthSummaryTableCount <= 0)
                            {
                                addNewMonthSummary = true;
                            }
                            else
                            {
                                ResourceUsageSummaryMonthWise mRow = objMonthWiseManager.Load(x => x.WorkItemID == Convert.ToInt32(workItem.ID) && x.MonthStartDate == mStartDate).FirstOrDefault();
                                if (mRow != null)
                                    monthSummaryItem = objMonthWiseManager.Load(x => x.ID == Convert.ToInt32(mRow.ID)).FirstOrDefault();
                                else
                                    addNewMonthSummary = true;
                            }

                            if (addNewMonthSummary)
                            {
                                //don't add new entry if allocation and actual hours are zero
                                if (UGITUtility.StringToDouble(workSummaryRow[DatabaseObjects.Columns.AllocationHour]) <= 0 && UGITUtility.StringToDouble(workSummaryRow[DatabaseObjects.Columns.ActualHour]) <= 0)
                                {
                                    continue;
                                }

                                monthSummaryItem = new ResourceUsageSummaryMonthWise();
                                monthSummaryItem.WorkItemID = Convert.ToInt32(workItem.ID);
                                monthSummaryItem.WorkItemType = workItem.WorkItemType;
                                monthSummaryItem.WorkItem = workItem.WorkItem;
                                if (workItem.SubWorkItem != null)
                                    monthSummaryItem.SubWorkItem = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItem.SubWorkItem), "[0-9]+;#", string.Empty);
                                if (userInfo != null)
                                {
                                    monthSummaryItem.Resource = userInfo.Id;
                                    monthSummaryItem.ResourceName = userInfo.Name;

                                    if (!string.IsNullOrEmpty(userInfo.ManagerID))
                                    {
                                        monthSummaryItem.ManagerLookup = userInfo.ManagerID;

                                        mgrInfo = userManager.GetUserById(userInfo.ManagerID);
                                        if (mgrInfo != null)
                                            monthSummaryItem.ManagerName = mgrInfo.Name;
                                    }
                                    monthSummaryItem.IsManager = userInfo.IsManager;
                                    monthSummaryItem.IsIT = userInfo.IsIT;
                                    monthSummaryItem.IsConsultant = userInfo.IsConsultant;
                                    if (userInfo.FunctionalArea != null)
                                    {
                                        monthSummaryItem.FunctionalAreaTitleLookup = Convert.ToString(userInfo.FunctionalArea);
                                        functionalArea = functionalAreas.FirstOrDefault(x => x.ID == userInfo.FunctionalArea);
                                        if (functionalArea != null)
                                            monthSummaryItem.FunctionalArea = functionalArea.Title;
                                    }
                                    if (!string.IsNullOrEmpty(userInfo.GlobalRoleId))
                                        monthSummaryItem.GlobalRoleID = userInfo.GlobalRoleId;
                                }

                                monthSummaryItem.MonthStartDate = mStartDate;
                                monthSummaryItem.PctAllocation = 0;
                                monthSummaryItem.AllocationHour = 0;

                                // For project work items, set title to project ID, else blank
                                monthSummaryItem.Title = string.Empty;
                                string workItemType = Convert.ToString(monthSummaryItem.WorkItemType);
                                if (workItemType == ModuleNames.PMM || workItemType == ModuleNames.TSK || workItemType == ModuleNames.NPR || workItemType == ModuleNames.CPR || workItemType == ModuleNames.OPM || workItemType == ModuleNames.CNS)
                                {
                                    projectItem = Ticket.GetCurrentTicket(context, workItemType, Convert.ToString(monthSummaryItem.WorkItem));
                                    if (projectItem != null)
                                    {
                                        monthSummaryItem.Title = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                                        if (UGITUtility.IfColumnExists(projectItem, DatabaseObjects.Columns.ERPJobID))
                                            monthSummaryItem.ERPJobID = Convert.ToString(projectItem[DatabaseObjects.Columns.ERPJobID]);
                                    }
                                }
                            }

                            monthSummaryItem.ActualHour = UGITUtility.StringToInt(workSummaryRow[DatabaseObjects.Columns.ActualHour]);
                            monthSummaryItem.PctActual = UGITUtility.StringToInt(workSummaryRow[DatabaseObjects.Columns.PctActual]);
                            if (objResourceAllocation != null)
                            {
                                monthSummaryItem.ActualStartDate = objResourceAllocation.AllocationStartDate;
                                monthSummaryItem.ActualEndDate = objResourceAllocation.AllocationEndDate;
                                monthSummaryItem.ActualPctAllocation = objResourceAllocation.PctAllocation;
                            }
                            objMonthWiseManager.Save(monthSummaryItem);

                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    }
                }

                //Update Week summary
                if (tempSummaryWeek != null && tempSummaryWeek.Rows.Count > 0)
                {
                    DataRow workSummaryRow = tempSummaryWeek.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.WorkItemID) == Convert.ToInt32(workItem.ID));
                    if (workSummaryRow != null)
                    {
                        ResourceUsageSummaryWeekWise weekSummaryItem = null;
                        bool addNewWeekSummary = false;

                        try
                        {
                            if (rmmWeekSummaryTableCount <= 0)
                            {
                                addNewWeekSummary = true;
                            }
                            else
                            {
                                ResourceUsageSummaryWeekWise mRow = objWeekWiseManager.Load(x => x.WorkItemID == Convert.ToInt32(workItem.ID) && x.WeekStartDate == weekStartDate).FirstOrDefault();
                                if (mRow != null)
                                    weekSummaryItem = objWeekWiseManager.Load(x => x.ID == Convert.ToInt32(mRow.ID)).FirstOrDefault();
                                else
                                    addNewWeekSummary = true;
                            }

                            if (addNewWeekSummary)
                            {
                                //don't add new entry if allocation and actual hours are zero
                                if (UGITUtility.StringToDouble(workSummaryRow[DatabaseObjects.Columns.AllocationHour]) <= 0 && UGITUtility.StringToDouble(workSummaryRow[DatabaseObjects.Columns.ActualHour]) <= 0)
                                {
                                    continue;
                                }

                                weekSummaryItem = new ResourceUsageSummaryWeekWise();
                                weekSummaryItem.WorkItemID = Convert.ToInt32(workItem.ID);
                                weekSummaryItem.WorkItemType = workItem.WorkItemType;
                                weekSummaryItem.WorkItem = workItem.WorkItem;
                                if (weekSummaryItem.SubWorkItem != null)
                                    weekSummaryItem.SubWorkItem = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(workItem.SubWorkItem), "[0-9]+;#", string.Empty);
                                if (userInfo != null)
                                {
                                    weekSummaryItem.Resource = userInfo.Id;
                                    weekSummaryItem.ResourceName = userInfo.Name;

                                    if (!string.IsNullOrEmpty(userInfo.ManagerID))
                                    {
                                        weekSummaryItem.ManagerLookup = userInfo.ManagerID;

                                        mgrInfo = userManager.GetUserById(userInfo.ManagerID);
                                        if (mgrInfo != null)
                                            weekSummaryItem.ManagerName = mgrInfo.Name;
                                    }
                                    weekSummaryItem.IsManager = userInfo.IsManager;
                                    weekSummaryItem.IsIT = userInfo.IsIT;
                                    weekSummaryItem.IsConsultant = userInfo.IsConsultant;
                                    if (userInfo.FunctionalArea != null)
                                    {
                                        weekSummaryItem.FunctionalAreaTitleLookup = Convert.ToString(userInfo.FunctionalArea);
                                        functionalArea = functionalAreas.FirstOrDefault(x => x.ID == userInfo.FunctionalArea);
                                        if (functionalArea != null)
                                            weekSummaryItem.FunctionalArea = functionalArea.Title;
                                    }
                                    if (!string.IsNullOrEmpty(userInfo.GlobalRoleId))
                                        weekSummaryItem.GlobalRoleID = userInfo.GlobalRoleId;
                                }

                                weekSummaryItem.WeekStartDate = weekStartDate;
                                weekSummaryItem.PctAllocation = 0;
                                weekSummaryItem.AllocationHour = 0;

                                // For project work items, set title to project ID, else blank
                                weekSummaryItem.Title = string.Empty;
                                string workItemType = Convert.ToString(weekSummaryItem.WorkItemType);
                                if (workItemType == ModuleNames.PMM || workItemType == ModuleNames.TSK || workItemType == ModuleNames.NPR || workItemType == ModuleNames.CPR || workItemType == ModuleNames.OPM || workItemType == ModuleNames.CNS)
                                {
                                    projectItem = Ticket.GetCurrentTicket(context, workItemType, Convert.ToString(weekSummaryItem.WorkItem));
                                    if (projectItem != null)
                                    {
                                        weekSummaryItem.Title = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                                        if (UGITUtility.IfColumnExists(projectItem, DatabaseObjects.Columns.ERPJobID))
                                            weekSummaryItem.ERPJobID = Convert.ToString(projectItem[DatabaseObjects.Columns.ERPJobID]);
                                    }
                                }
                            }

                            weekSummaryItem.ActualHour = Convert.ToInt32(workSummaryRow[DatabaseObjects.Columns.ActualHour]);
                            weekSummaryItem.PctActual = Convert.ToInt32(workSummaryRow[DatabaseObjects.Columns.PctActual]);
                            objWeekWiseManager.Save(weekSummaryItem);

                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    }
                }
            }

            //Deletes empty allocation and actual entries from rmm monthly summary list
            List<int> deleteItemList = new List<int>();
            /*
            foreach (ResourceUsageSummaryMonthWise item in rmmMonthSummaryColl)
            {
                if (UGITUtility.StringToDouble(item.AllocationHour) <= 0 && UGITUtility.StringToDouble(item.ActualHour) <= 0)
                {
                    deleteItemList.Add(Convert.ToInt32(item.ID));
                }
            }
            */
            foreach (ResourceUsageSummaryMonthWise item in objMonthWiseManager.Load(x => UGITUtility.StringToDouble(x.AllocationHour) <= 0 && UGITUtility.StringToDouble(x.ActualHour) <= 0))
            {
                deleteItemList.Add(Convert.ToInt32(item.ID));
            }

            if (deleteItemList.Count > 0)
                RMMSummaryHelper.Delete(context, rmmMonthSummaryList, deleteItemList);

            //Deletes empty allocation and actual entries from rmm weekly summary list
            deleteItemList = new List<int>();
            /*
            foreach (ResourceUsageSummaryWeekWise item in rmmWeekSummaryColl)
            {
                if (UGITUtility.StringToDouble(item.AllocationHour) <= 0 && UGITUtility.StringToDouble(item.ActualHour) <= 0)
                {
                    deleteItemList.Add(Convert.ToInt32(item.ID));
                }
            }
            */
            foreach (ResourceUsageSummaryWeekWise item in objWeekWiseManager.Load(x => UGITUtility.StringToDouble(x.AllocationHour) <= 0 && UGITUtility.StringToDouble(x.ActualHour) <= 0))
            {
                deleteItemList.Add(Convert.ToInt32(item.ID));
            }
            if (deleteItemList.Count > 0)
                RMMSummaryHelper.Delete(context, rmmWeekSummaryList, deleteItemList);
        }
        #region Update data of Summary Tables
        private static bool IsProcessActive;

        public static bool ProcessState()
        {
            return IsProcessActive;
        }

        public static void UpdateRMMAllocationSummary(ApplicationContext context)
        {

            if (IsProcessActive)
                return;

            ResourceWorkItemsManager rwManager = new ResourceWorkItemsManager(context);
            List<ResourceWorkItems> lstresourceWorkItems = rwManager.Load();
            ULog.WriteLog($"Summary Data Refresh started.");
            try
            {
                foreach (var workitem in lstresourceWorkItems)
                {
                    IsProcessActive = true;
                    UpdateRMMAllocationSummary(context, workitem.ID);
                }
                ULog.WriteLog($"Summary Data Refresh Completed.");
            }
            catch (Exception ex)
            {
                ULog.WriteException($"UpdateRMMAllocationSummary : {ex}");
            }
            IsProcessActive = false;
        }
        #endregion
        public static void UpdateRMMAllocationSummary(ApplicationContext context, long workItemID)
        {
            //Calculate working dates and working hours/day
            int workingHours = uHelper.GetWorkingHoursInADay(context);

            DataTable workItemDt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.ID}={workItemID} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataRow workItem = null;
            if (workItemDt != null && workItemDt.Rows.Count > 0)
                workItem = workItemDt.Select()[0];
            if (workItem == null)
                return;

            string resourceLookup = Convert.ToString(workItem[DatabaseObjects.Columns.Resource]);
            UserProfile mgrInfo = null, resUserInfo = null;

            UserProfile userProfile = context.UserManager.LoadById(resourceLookup);
            if (userProfile == null)
            {
                userProfile = new UserProfile() { Id = Guid.Empty.ToString(), Name = "Unfilled Roles", UserName = "UnfilledRoles", GlobalRoleId = null, isRole = false, Enabled = true, TenantID = context.TenantID };
            }
            
            FunctionalAreasManager objFunctionalAreasManager = new FunctionalAreasManager(context);
            List<FunctionalArea> functionalAreas = objFunctionalAreasManager.Load();
            FunctionalArea functionalArea = null;

            ResourceUsageSummaryWeekWiseManager objWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            ResourceUsageSummaryMonthWiseManager objMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            
            List<ResourceUsageSummaryWeekWise> oldWeekWiseDate = objWeekWiseManager.Load(x => x.WorkItemID == workItemID);
            if (oldWeekWiseDate != null && oldWeekWiseDate.Count > 0)
            {
                objWeekWiseManager.Delete(oldWeekWiseDate);
            }

            List<ResourceUsageSummaryMonthWise> oldMonthWiseData = objMonthWiseManager.Load(x => x.WorkItemID == workItemID);
            if (oldMonthWiseData != null && oldMonthWiseData.Count > 0)
            {
                objMonthWiseManager.Delete(oldMonthWiseData);
            }

            ResourceAllocationManager resourceAllocationMGR = new ResourceAllocationManager(context);
            List<RResourceAllocation> LstResourceAllocation = resourceAllocationMGR.Load(x => x.ResourceWorkItemLookup == workItemID);
            foreach (var objResourceAllocation in LstResourceAllocation)
            {
                string allocationQuery = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemID, DatabaseObjects.Columns.TenantID, context.TenantID);
                DataRow[] workAllocations = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, allocationQuery).Select();

                string summaryQuery = string.Format("{0}={1}", DatabaseObjects.Columns.WorkItemID, workItemID);
                List<ResourceUsageSummaryMonthWise> rmmMonthSummaryColl = objMonthWiseManager.Load(x => x.Resource == objResourceAllocation.Resource 
                            && x.WorkItem == objResourceAllocation.TicketID && x.GlobalRoleID == objResourceAllocation.RoleId 
                            && x.ActualStartDate == objResourceAllocation.AllocationStartDate && x.ActualEndDate == objResourceAllocation.AllocationEndDate).ToList();
                List<ResourceUsageSummaryWeekWise> rmmWeekSummaryColl = objWeekWiseManager.Load(x => x.Resource == objResourceAllocation.Resource
                            && x.WorkItem == objResourceAllocation.TicketID && x.GlobalRoleID == objResourceAllocation.RoleId
                            && x.ActualStartDate == objResourceAllocation.AllocationStartDate && x.ActualEndDate == objResourceAllocation.AllocationEndDate).ToList();
                List<ResourceUsageSummaryWeekWise> rmmWeekSummaryTable = rmmWeekSummaryColl;
                List<ResourceUsageSummaryMonthWise> rmmMonthSummaryTable = rmmMonthSummaryColl;

                if (rmmMonthSummaryTable != null && rmmMonthSummaryTable.Count > 0)
                {
                    foreach (ResourceUsageSummaryMonthWise mItem in rmmMonthSummaryTable)
                    {
                        mItem.PctAllocation = 0;
                        mItem.AllocationHour = 0;
                    }
                }
                if (rmmWeekSummaryTable != null && rmmWeekSummaryTable.Count > 0)
                {
                    foreach (ResourceUsageSummaryWeekWise wItem in rmmWeekSummaryTable)
                    {
                        wItem.PctAllocation = 0;
                        wItem.AllocationHour = 0;
                    }
                }

                List<ResourceUsageSummaryWeekWise> tempSummaryWeek = new List<ResourceUsageSummaryWeekWise>();
                List<ResourceUsageSummaryMonthWise> tempSummaryMonth = new List<ResourceUsageSummaryMonthWise>();


                DataTable workAllocationTable = new DataTable();
                if (workAllocations.Length > 0)
                    workAllocationTable = workAllocations.CopyToDataTable();

                RMMSummaryHelper.DistributionRMMAllocation(context, objResourceAllocation?.PctAllocation ?? 0, workingHours, userProfile, workItem, objResourceAllocation,
                    ref tempSummaryWeek, ref tempSummaryMonth);

                // For project work items, set title to project ID, else blank
                string workItemTitle = string.Empty;
                string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);

                //Pick title from WorkItem. If it is blank, then fetch the current ticket and use its title
                workItemTitle = Convert.ToString(workItem[DatabaseObjects.Columns.Title]);
                if (string.IsNullOrEmpty(workItemTitle))
                {
                    ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                    UGITModule module = moduleViewManager.LoadByName(workItemType);
                    TicketManager ticketManager = new TicketManager(context);

                    if (module != null)
                    {
                        DataRow projectItem = ticketManager.GetByTicketIdFromCache(workItemType, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
                        if (projectItem != null)
                            workItemTitle = Convert.ToString(projectItem[DatabaseObjects.Columns.Title]);
                    }
                }
                #region Add and edit ResourceUsageSummaryWeekWise List
                if (tempSummaryWeek != null && tempSummaryWeek.Count > 0)
                {
                    bool addNewWeekSummary = false;
                    DateTime updatedRowTime = DateTime.Now;
                    foreach (ResourceUsageSummaryWeekWise row in tempSummaryWeek)
                    {
                        addNewWeekSummary = false;
                        ResourceUsageSummaryWeekWise item = null;
                        if (rmmWeekSummaryTable == null || rmmWeekSummaryTable.Count <= 0)
                        {
                            addNewWeekSummary = true;
                        }
                        else
                        {
                            ResourceUsageSummaryWeekWise mRow = rmmWeekSummaryTable.FirstOrDefault(x => x.WeekStartDate == UGITUtility.StringToDateTime(row.WeekStartDate));
                            if (mRow != null)
                            {
                                item = rmmWeekSummaryColl.FirstOrDefault(x => x.ID == Convert.ToInt32(mRow.ID));
                                if (item == null)
                                    addNewWeekSummary = true;

                                mRow.PctAllocation = row.PctAllocation;
                                mRow.AllocationHour = row.AllocationHour;
                            }
                            else
                                addNewWeekSummary = true;
                        }
                        if (addNewWeekSummary)
                        {
                            item = new ResourceUsageSummaryWeekWise();

                        }
                        item.WorkItemID = row.WorkItemID;
                        item.WorkItemType = row.WorkItemType;
                        item.WorkItem = row.WorkItem;
                        if (row.SubWorkItem != null)
                            item.SubWorkItem = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row.SubWorkItem), "[0-9]+;#", string.Empty);

                        item.Resource = row.Resource;
                        item.ManagerLookup = row.ManagerLookup;
                        item.FunctionalAreaTitleLookup = row.FunctionalAreaTitleLookup;
                        item.IsManager = row.IsManager;
                        item.IsIT = row.IsIT;
                        item.IsConsultant = row.IsConsultant;
                        item.WeekStartDate = row.WeekStartDate;
                        item.ActualHour = 0;
                        item.PctActual = 0;
                        item.Title = workItemTitle;

                        if (UGITUtility.IsValidTicketID(item.WorkItem))
                        {
                            DataRow projectItem = Ticket.GetCurrentTicket(context, workItemType, Convert.ToString(item.WorkItem));
                            if (projectItem != null)
                            {
                                if (UGITUtility.IfColumnExists(projectItem, DatabaseObjects.Columns.ERPJobID))
                                    item.ERPJobID = Convert.ToString(projectItem[DatabaseObjects.Columns.ERPJobID]);
                            }
                        }
                        item.PctAllocation = row.PctAllocation;
                        item.AllocationHour = row.AllocationHour;
                        item.PctPlannedAllocation = row.PctPlannedAllocation;
                        item.PlannedAllocationHour = row.PlannedAllocationHour;
                        item.GlobalRoleID = row.GlobalRoleID;
                        item.ActualStartDate = objResourceAllocation.AllocationStartDate;
                        item.ActualEndDate = objResourceAllocation.AllocationEndDate;
                        item.ActualPctAllocation = objResourceAllocation.PctAllocation;
                        item.SoftAllocation = objResourceAllocation.SoftAllocation;

                        if (!string.IsNullOrEmpty(row.Resource))
                        {
                            resUserInfo = context.UserManager.GetUserById(row.Resource);
                            if (resUserInfo != null)
                            {
                                item.ResourceName = resUserInfo.Name;
                                item.Resource = resUserInfo.Id;
                            }

                            if (!string.IsNullOrEmpty(resUserInfo.ManagerID))
                            {
                                mgrInfo = context.UserManager.GetUserById(resUserInfo.ManagerID);
                                if (mgrInfo != null)
                                {
                                    item.ManagerName = mgrInfo.Name;
                                    item.ManagerLookup = mgrInfo.Id;
                                }
                            }

                            if (resUserInfo.FunctionalArea != null)
                            {
                                functionalArea = functionalAreas.FirstOrDefault(x => x.ID == resUserInfo.FunctionalArea.Value);
                                if (functionalArea != null)
                                    item.FunctionalArea = functionalArea.Title;
                            }
                        }

                        try
                        {
                            objWeekWiseManager.Save(item);
                        }
                        catch (Exception)
                        {
                            // Ignore exception, usually due to write conflict
                        }
                    }
                    tempSummaryWeek.Clear();
                }
                #endregion

                #region Add and edit ResourceUsageSummaryMonthWise List
                if (tempSummaryMonth != null && tempSummaryMonth.Count > 0)
                {
                    DateTime updatedRowTime = DateTime.Now;

                    foreach (ResourceUsageSummaryMonthWise row in tempSummaryMonth)
                    {
                        ResourceUsageSummaryMonthWise item = null;
                        bool addNewMonthSummary = false;
                        if (rmmMonthSummaryTable == null || rmmMonthSummaryTable.Count <= 0)
                        {
                            addNewMonthSummary = true;
                        }
                        else
                        {
                            ResourceUsageSummaryMonthWise mRow = rmmMonthSummaryTable.AsEnumerable().FirstOrDefault(x => x.MonthStartDate == UGITUtility.StringToDateTime(row.MonthStartDate));
                            if (mRow != null)
                            {
                                item = rmmMonthSummaryColl.FirstOrDefault(x => x.ID == Convert.ToInt32(mRow.ID));
                                if (item == null)
                                    addNewMonthSummary = true;

                                mRow.PctAllocation = row.PctAllocation;
                                mRow.AllocationHour = row.AllocationHour;
                            }
                            else
                                addNewMonthSummary = true;
                        }

                        if (addNewMonthSummary)
                        {
                            item = new ResourceUsageSummaryMonthWise();
                            item = row;
                            item.Title = workItemTitle;
                        }
                        if (UGITUtility.IsValidTicketID(item.WorkItem))
                        {
                            DataRow projectItem = Ticket.GetCurrentTicket(context, workItemType, Convert.ToString(item.WorkItem));
                            if (projectItem != null)
                            {
                                if (UGITUtility.IfColumnExists(projectItem, DatabaseObjects.Columns.ERPJobID))
                                    item.ERPJobID = Convert.ToString(projectItem[DatabaseObjects.Columns.ERPJobID]);
                            }
                        }
                        if (!string.IsNullOrEmpty(row.Resource))
                        {
                            resUserInfo = context.UserManager.GetUserById(row.Resource);
                            if (resUserInfo != null)
                            {
                                item.ResourceName = resUserInfo.Name;
                                item.Resource = resUserInfo.Id;
                            }
                            if (!string.IsNullOrEmpty(resUserInfo.ManagerID))
                            {
                                mgrInfo = context.UserManager.GetUserById(resUserInfo.ManagerID);
                                if (mgrInfo != null)
                                {
                                    item.ManagerName = mgrInfo.Name;
                                    item.ManagerLookup = mgrInfo.Id;
                                }
                            }

                            if (resUserInfo.FunctionalArea != null)
                            {
                                functionalArea = functionalAreas.FirstOrDefault(x => x.ID == resUserInfo.FunctionalArea.Value);
                                if (functionalArea != null)
                                    item.FunctionalArea = functionalArea.Title;
                            }
                        }
                        item.ActualStartDate = objResourceAllocation.AllocationStartDate;
                        item.ActualEndDate = objResourceAllocation.AllocationEndDate;
                        item.ActualPctAllocation = objResourceAllocation.PctAllocation;
                        item.SoftAllocation = objResourceAllocation.SoftAllocation;

                        try
                        {
                            objMonthWiseManager.Save(item);
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
                if (rmmMonthSummaryTable != null && rmmMonthSummaryTable.Count > 0)
                {
                    ResourceUsageSummaryMonthWise rItem = null;
                    ResourceUsageSummaryMonthWise dbMonthWiseRecord = null;
                    foreach (ResourceUsageSummaryMonthWise mmItem in rmmMonthSummaryTable)
                    {
                        if (UGITUtility.StringToDouble(mmItem.AllocationHour) <= 0)
                        {
                            if (UGITUtility.StringToDouble(mmItem.ActualHour) <= 0)
                            {
                                dbMonthWiseRecord = objMonthWiseManager.LoadByID(mmItem.ID);
                                if (dbMonthWiseRecord != null) //Delete only if record exists in the DB
                                    objMonthWiseManager.Delete(dbMonthWiseRecord);
                                dbMonthWiseRecord = null;
                            }
                            else
                            {
                                //******* need to check again
                                rItem = rmmMonthSummaryColl.FirstOrDefault(x => x.ID == Convert.ToInt32(mmItem.ID));
                                if (rItem != null)
                                {
                                    rItem.AllocationHour = 0;
                                    rItem.PctAllocation = 0;
                                    objMonthWiseManager.Save(rItem);
                                }
                            }
                        }

                    }
                }

                //Deletes Empty Allocation entry from week summary list.
                if (rmmWeekSummaryTable != null && rmmWeekSummaryTable.Count > 0)
                {
                    ResourceUsageSummaryWeekWise rItem = null;
                    ResourceUsageSummaryWeekWise dbWeekWiseRecord = null;
                    foreach (ResourceUsageSummaryWeekWise wwItem in rmmWeekSummaryTable)
                    {
                        if (UGITUtility.StringToDouble(wwItem.AllocationHour) <= 0)
                        {
                            if (UGITUtility.StringToDouble(wwItem.ActualHour) <= 0)
                            {
                                dbWeekWiseRecord = objWeekWiseManager.LoadByID(wwItem.ID);
                                if (dbWeekWiseRecord != null) //Delete only if record exists in the DB
                                    objWeekWiseManager.Delete(dbWeekWiseRecord);
                                dbWeekWiseRecord = null;
                            }
                            else
                            {
                                //******* need to check again
                                rItem = rmmWeekSummaryColl.FirstOrDefault(x => x.ID == Convert.ToInt32(wwItem.ID));
                                if (rItem != null)
                                {
                                    rItem.AllocationHour = 0;
                                    rItem.PctAllocation = 0;
                                    objWeekWiseManager.Save(rItem);

                                }
                            }
                        }
                    }
                }
                #endregion
            }


        }

        public static void UpdateRMMAllocationSummary(ApplicationContext context, long workItemID, string spWebUrl)
        {
            UpdateRMMAllocationSummary(context, workItemID);

        }

        public static void UpdateRMMAllocationSummary(ApplicationContext context, List<long> workItemsID)
        {
            if (workItemsID.Count <= 0)
            {
                return;
            }

            foreach (int workItemID in workItemsID)
            {
                try
                {
                    UpdateRMMAllocationSummary(context, workItemID);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "UpdateRMMAllocationSummary");
                }
            }

        }

        public static void DeleteRMMAllocationSummary(ApplicationContext context, List<int> workItemsID, string spWebUrl)
        {
            foreach (int workItemID in workItemsID)
            {
                UpdateRMMAllocationSummary(context, workItemID);
            }

        }

        public static void UpdateResourceUsageSummary(ApplicationContext context, DataTable workItems, DataTable allocationItems, DataTable actualItems)
        {
            ConfigurationVariableManager configurationVariable = new ConfigurationVariableManager(context);
            //Calculates total working hours in one day
            DateTime workDayStartTime = UGITUtility.StringToDateTime(configurationVariable.GetValue("WorkdayStartTime"));
            DateTime workDayEndTime = UGITUtility.StringToDateTime(configurationVariable.GetValue("WorkdayEndTime"));
            if (workDayStartTime == DateTime.MinValue)
            {
                workDayStartTime = DateTime.Now.Date;
            }

            if (workDayEndTime == DateTime.MinValue)
            {
                workDayEndTime = DateTime.Now.Date.AddDays(1);
            }
            int workingHours = (int)workDayEndTime.Subtract(workDayStartTime).TotalHours;
            ResourceUsageSummaryWeekWiseManager objWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            ResourceUsageSummaryMonthWiseManager objMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(context);
            ResourceTimeSheetManager objTimeSheetManager = new ResourceTimeSheetManager(context);
            List<ResourceUsageSummaryWeekWise> summaryTableWeek = objWeekWiseManager.Load();
            List<ResourceUsageSummaryMonthWise> summaryTableMonth = objMonthWiseManager.Load();
            UserProfileManager userProfileManager = new UserProfileManager(context);
            List<UserProfile> userInfoes = userProfileManager.GetUsersProfile();

            List<RResourceAllocation> allocationList = objAllocationManager.Load();
            List<ResourceTimeSheet> timeSheetList = objTimeSheetManager.Load();

            List<ResourceUsageSummaryWeekWise> summaryListWeek = summaryTableWeek;// spWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryWeekWise];
            List<ResourceUsageSummaryMonthWise> summaryListMonth = summaryTableMonth;// spWeb.Lists[DatabaseObjects.Lists.ResourceUsageSummaryMonthWise];

            if (workItems == null || workItems.Rows.Count < 0)
            {
                return;
            }

            //group all workitems by resource
            var workItemsByUser = workItems.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.Resource));

            List<ResourceUsageSummaryWeekWise> tempSummaryWeek = new List<ResourceUsageSummaryWeekWise>();// CreateTempSummaryWeek();

            List<ResourceUsageSummaryMonthWise> tempSummaryMonth = new List<ResourceUsageSummaryMonthWise>();// CreateTempSummaryMonth();

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
                // DataRow[] usersInfo = userInfoes.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, userName));
                List<UserProfile> usersInfo = userInfoes.Where(x => x.UserName == userName).ToList();

                if (usersInfo.Count() == 0)
                    continue; // User not found, probably deleted!

                UserProfile userProfile = userProfileManager.LoadById(Convert.ToString(usersInfo[0].Id));
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
                // DataRow[] workSmmaryRowsWeek = new DataRow[0];
                List<ResourceUsageSummaryWeekWise> workSmmaryRowsWeek = new List<ResourceUsageSummaryWeekWise>();
                if (summaryTableWeek != null && summaryTableWeek.Count > 0)
                {
                    workSmmaryRowsWeek = summaryTableWeek.Where(x => x.Resource == userName).ToList();
                }

                //Group month workSummary rows by user
                //DataRow[] workSmmaryRowsMonth = new DataRow[0];
                List<ResourceUsageSummaryMonthWise> workSmmaryRowsMonth = null;
                if (summaryTableMonth != null && summaryTableMonth.Count > 0)
                {
                    workSmmaryRowsMonth = summaryTableMonth.Where(x => x.Resource == userName).ToList();
                }

                foreach (DataRow workItem in userWorkItems)
                {
                    string workItemID = Convert.ToString(workItem[DatabaseObjects.Columns.Id]);

                    // For project work items, set title to project ID, else blank
                    string workItemTitle = string.Empty;
                    string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    if (workItemType == "PMM" || workItemType == "TSK" || workItemType == "NPR")
                    {
                        DataRow projectItem = Ticket.GetCurrentTicket(context, workItemType, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
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

                    //if (workItemAllocations.Length > 0 && userProfile != null)
                    //    RMMSummaryHelper.DistributionRMMAllocation(context, workingHours, userProfile, workItem, workItemAllocations.CopyToDataTable(), ref tempSummaryWeek, ref tempSummaryMonth);

                    //Run loop of all actual work of current workitem and current user 
                    foreach (DataRow actualWork in workItemActuals)
                    {
                        DateTime workDate = UGITUtility.StringToDateTime(actualWork[DatabaseObjects.Columns.WorkDate]);
                        #region TempSummaryWeek Table
                        {
                            // DayOfWeek returns Sun = 0 to Sat = 6
                            //   convert to Mon = 0 to Sun = 6
                            int dayOfWeek = (int)workDate.DayOfWeek - 1;
                            if (dayOfWeek < 0)
                                dayOfWeek = 6;
                            DateTime weekStartDate = workDate.AddDays(-dayOfWeek);

                            ResourceUsageSummaryWeekWise summaryRow = null;
                            if (tempSummaryWeek != null && tempSummaryWeek.Count > 0)
                            {
                                summaryRow = tempSummaryWeek.FirstOrDefault(x => x.WorkItemID == Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]) &&
                                                                                            x.WeekStartDate == weekStartDate.Date);
                            }

                            if (summaryRow == null)
                            {
                                summaryRow = new ResourceUsageSummaryWeekWise();// tempSummaryWeek.NewRow();
                                summaryRow.AllocationHour = 0;
                                summaryRow.PctAllocation = 0;
                                summaryRow.ActualHour = 0;
                                summaryRow.PctActual = 0;
                                tempSummaryWeek.Add(summaryRow);
                            }


                            summaryRow.WeekStartDate = weekStartDate.Date;
                            summaryRow.WorkItemID = Convert.ToInt32(workItem[DatabaseObjects.Columns.ID]);
                            summaryRow.WorkItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                            summaryRow.WorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]);
                            summaryRow.SubWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);

                            summaryRow.Title = workItemTitle;

                            if (userProfile != null)
                            {
                                summaryRow.Resource = userProfile.Id;
                                if (!string.IsNullOrEmpty(userProfile.ManagerID))
                                    summaryRow.ManagerLookup = userProfile.ManagerID;
                                summaryRow.IsManager = userProfile.IsManager;
                                summaryRow.IsIT = userProfile.IsIT;
                                summaryRow.IsConsultant = userProfile.IsConsultant;
                                if (userProfile.FunctionalArea != null)
                                    summaryRow.FunctionalAreaTitleLookup = Convert.ToString(userProfile.FunctionalArea);
                            }

                            double hoursTaken = 0;
                            double.TryParse(Convert.ToString(summaryRow.ActualHour), out hoursTaken);

                            double currentWeekhoursTaken = 0;
                            double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(actualWork, DatabaseObjects.Columns.HoursTaken)), out currentWeekhoursTaken);
                            double totalHours = hoursTaken + currentWeekhoursTaken;
                            int workingDayInWeek = uHelper.GetTotalWorkingDaysBetween(context, weekStartDate, weekStartDate.AddDays(6));
                            summaryRow.ActualHour = Convert.ToInt32(totalHours);
                            if (totalHours > 0)
                                summaryRow.PctActual = Convert.ToInt32(workingDayInWeek > 0 ? (totalHours * 100) / (workingHours * workingDayInWeek) : 100);
                            else
                                summaryRow.PctActual = 0;

                        }
                        #endregion

                        #region Fill tempSummaryMonth table
                        {
                            DateTime monthStartDate = new DateTime(workDate.Year, workDate.Month, 1);
                            int daysInMonth = DateTime.DaysInMonth(monthStartDate.Year, monthStartDate.Month);

                            ResourceUsageSummaryMonthWise summaryRow = null;
                            if (tempSummaryMonth != null && tempSummaryMonth.Count > 0)
                            {
                                summaryRow = tempSummaryMonth.FirstOrDefault(x => x.WorkItemID == Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]) &&
                                                                                            x.MonthStartDate == monthStartDate.Date);
                            }

                            if (summaryRow == null)
                            {
                                summaryRow = new ResourceUsageSummaryMonthWise();
                                summaryRow.AllocationHour = 0;
                                summaryRow.PctAllocation = 0;
                                summaryRow.ActualHour = 0;
                                summaryRow.PctActual = 0;
                                tempSummaryMonth.Add(summaryRow);
                            }

                            summaryRow.MonthStartDate = monthStartDate.Date;
                            summaryRow.WorkItemID = Convert.ToInt32(workItem[DatabaseObjects.Columns.Id]);
                            summaryRow.WorkItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                            summaryRow.WorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]);
                            summaryRow.SubWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                            summaryRow.Title = workItemTitle;

                            if (userProfile != null)
                            {
                                summaryRow.Resource = userProfile.Id;
                                if (!string.IsNullOrEmpty(userProfile.ManagerID))
                                    summaryRow.ManagerLookup = userProfile.ManagerID;
                                summaryRow.IsManager = userProfile.IsManager;
                                summaryRow.IsIT = userProfile.IsIT;
                                summaryRow.IsConsultant = userProfile.IsConsultant;
                                if (userProfile.FunctionalArea != null)
                                    summaryRow.FunctionalAreaTitleLookup = Convert.ToString(userProfile.FunctionalArea);
                            }

                            int workingDayInMonth = uHelper.GetTotalWorkingDaysBetween(context, monthStartDate, monthStartDate.AddDays(daysInMonth));
                            double hoursTaken = 0;
                            double.TryParse(Convert.ToString(summaryRow.ActualHour), out hoursTaken);

                            double currentWeekhoursTaken = 0;
                            double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(actualWork, DatabaseObjects.Columns.HoursTaken)), out currentWeekhoursTaken);
                            double totalHours = hoursTaken + currentWeekhoursTaken;

                            summaryRow.ActualHour = Convert.ToInt32(totalHours);
                            if (totalHours > 0)
                                summaryRow.PctActual = Convert.ToInt32(workingDayInMonth > 0 ? (totalHours * 100) / (workingHours * workingDayInMonth) : 100);
                            else
                                summaryRow.PctActual = 0;
                        }
                        #endregion
                    }
                }

                #region Add and edit ResourceUsageSummaryWeekWise List
                if (tempSummaryWeek != null && tempSummaryWeek.Count > 0)
                {
                    DateTime updatedRowTime = DateTime.Now;
                    foreach (ResourceUsageSummaryWeekWise row in tempSummaryWeek)
                    {
                        ResourceUsageSummaryWeekWise item = null;
                        bool newEntry = false;
                        if (workSmmaryRowsWeek != null)
                        {
                            ResourceUsageSummaryWeekWise summaryRow = workSmmaryRowsWeek.FirstOrDefault(x => x.WorkItemID == Convert.ToInt32(row.WorkItemID) &&
                                                                                            x.WeekStartDate == UGITUtility.StringToDateTime(row.WeekStartDate).Date);
                            if (summaryRow != null)
                            {
                                item = summaryListWeek.FirstOrDefault(x => x.ID == summaryRow.ID);
                                summaryRow.Modified = updatedRowTime;
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
                            item = new ResourceUsageSummaryWeekWise();// summaryListWeek.AddItem();
                            item.WorkItemID = Convert.ToInt32(row.WorkItemID);
                        }

                        item.WorkItemType = row.WorkItemType;
                        item.WorkItem = row.WorkItem;
                        if (row.SubWorkItem != null)
                            item.SubWorkItem = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row.SubWorkItem), "[0-9]+;#", string.Empty);
                        item.Resource = row.Resource;
                        item.ManagerLookup = row.ManagerLookup;
                        item.IsManager = row.IsManager;
                        item.IsIT = row.IsIT;
                        item.IsConsultant = row.IsConsultant;
                        item.WeekStartDate = row.WeekStartDate;
                        item.PctAllocation = row.PctAllocation;
                        item.AllocationHour = row.AllocationHour;
                        item.PctPlannedAllocation = row.PctPlannedAllocation;
                        item.PlannedAllocationHour = row.PlannedAllocationHour;
                        item.ActualHour = row.ActualHour;
                        item.PctActual = row.PctActual;
                        item.FunctionalAreaTitleLookup = row.FunctionalAreaTitleLookup;
                        item.Title = row.Title;
                        objWeekWiseManager.Save(item);
                    }
                    tempSummaryWeek.Clear();

                    // Delete leftover rows with no matching data
                    List<ResourceUsageSummaryWeekWise> crapedSummaryRows = workSmmaryRowsWeek.Where(x => x.Modified < updatedRowTime).ToList();
                    foreach (ResourceUsageSummaryWeekWise crapRow in crapedSummaryRows)
                    {
                        ResourceUsageSummaryWeekWise item = summaryListWeek.FirstOrDefault(x => x.ID == (int)crapRow.ID);
                        objWeekWiseManager.Delete(item);
                    }
                }
                #endregion

                #region Add and edit ResourceUsageSummaryMonthWise List
                if (tempSummaryMonth != null && tempSummaryMonth.Count > 0)
                {
                    DateTime updatedRowTime = DateTime.Now;

                    foreach (ResourceUsageSummaryMonthWise row in tempSummaryMonth)
                    {
                        ResourceUsageSummaryMonthWise item = null;
                        bool newEntry = false;
                        if (workSmmaryRowsMonth.Count > 0)
                        {
                            ResourceUsageSummaryMonthWise summaryRow = workSmmaryRowsMonth.FirstOrDefault(x => x.WorkItemID == Convert.ToInt32(row.WorkItemID) &&
                                                                                         Convert.ToDateTime(x.MonthStartDate).Date == UGITUtility.StringToDateTime(row.MonthStartDate).Date);
                            if (summaryRow != null)
                            {
                                item = summaryListMonth.FirstOrDefault(x => x.ID == Convert.ToInt32(summaryRow.ID));
                                summaryRow.Modified = updatedRowTime;
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
                            item = new ResourceUsageSummaryMonthWise();// summaryListMonth.AddItem();
                            item.WorkItemID = row.WorkItemID;
                        }

                        item.WorkItemType = row.WorkItemType;
                        item.WorkItem = row.WorkItem;
                        if (row.SubWorkItem != null)
                            item.SubWorkItem = System.Text.RegularExpressions.Regex.Replace(Convert.ToString(row.SubWorkItem), "[0-9]+;#", string.Empty);
                        item.Resource = row.Resource;
                        item.ManagerLookup = row.ManagerLookup;
                        item.IsManager = row.IsManager;
                        item.IsIT = row.IsIT;
                        item.IsConsultant = row.IsConsultant;
                        item.MonthStartDate = row.MonthStartDate;
                        item.PctAllocation = row.PctAllocation;
                        item.AllocationHour = row.AllocationHour;
                        item.PctPlannedAllocation = row.PctPlannedAllocation;
                        item.PlannedAllocationHour = row.PlannedAllocationHour;
                        item.ActualHour = row.ActualHour;
                        item.PctActual = row.PctActual;
                        item.FunctionalAreaTitleLookup = row.FunctionalAreaTitleLookup;
                        item.Title = row.Title;
                        objMonthWiseManager.Save(item);
                    }
                    tempSummaryMonth.Clear();

                    // Delete leftover rows with no matching data
                    List<ResourceUsageSummaryMonthWise> crapedSummaryRows = workSmmaryRowsMonth.Where(x => x.Modified < updatedRowTime).ToList();

                    foreach (ResourceUsageSummaryMonthWise crapRow in crapedSummaryRows)
                    {
                        objMonthWiseManager.Delete(crapRow);
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
        public static void DistributeAllocationByMonth(ApplicationContext context, long workItemID = 0, ResourceWorkItems workItem = null, bool updatePlannedOnly = false, List<RResourceAllocation> rResourceAlloc = null)
        {
            if (workItem != null)
                workItemID = workItem.ID;
            else if (workItemID > 0)
            {
                ResourceWorkItemsManager rwManager = new ResourceWorkItemsManager(context);
                workItem = rwManager.Get(workItemID);
            }

            if (workItem == null)
                return;

            List<ResourceDistributionItem> distributions = DistributePercentagesMonthly(context, workItem.ID, updatePlannedOnly, rResourceAlloc: rResourceAlloc);
            DistributeAllocationByMonth(context, workItem, distributions, updatePlannedOnly);
        }

        private static List<ResourceDistributionItem> DistributePercentagesMonthly(ApplicationContext context, long workItemID, bool updatePlanned = false, List<RResourceAllocation> rResourceAlloc = null)
        {
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
            List<RResourceAllocation> rAllocationItems = rResourceAlloc;
            if (rAllocationItems == null)
                rAllocationItems = resourceAllocationManager.Load(x => x.ResourceWorkItemLookup == workItemID && !x.Deleted).ToList();

            rAllocationItems = rAllocationItems.Where(x => x.ResourceWorkItemLookup == workItemID && !x.Deleted).ToList();
            List<ResourceDistributionItem> distItems = new List<ResourceDistributionItem>();
            if (rAllocationItems == null || rAllocationItems.Count <= 0)
                return distItems;

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(context);

            foreach (RResourceAllocation row in rAllocationItems)
            {
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                double pctAllocation = 0;
                if (updatePlanned)
                {
                    startDate = row.PlannedStartDate.HasValue ? row.PlannedStartDate.Value : DateTime.MinValue;
                    endDate = row.PlannedEndDate.HasValue ? row.PlannedEndDate.Value : DateTime.MinValue;
                    pctAllocation = row.PctPlannedAllocation.HasValue ? row.PctPlannedAllocation.Value : 0;
                }
                else
                {
                    startDate = row.AllocationStartDate.HasValue ? row.AllocationStartDate.Value : DateTime.MinValue;
                    endDate = row.AllocationEndDate.HasValue ? row.AllocationEndDate.Value : DateTime.MinValue;
                    pctAllocation = row.PctAllocation.HasValue ? row.PctAllocation.Value : 0;
                }

                DateTime tempStartDate = startDate;
                while (tempStartDate <= endDate)
                {
                    ResourceDistributionItem distItem = new ResourceDistributionItem();

                    DateTime tempS = new DateTime(tempStartDate.Year, tempStartDate.Month, 1);
                    DateTime tempEndDate = new DateTime(tempStartDate.Year, tempStartDate.Month, DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month));
                    if (tempStartDate.Year == endDate.Year && tempStartDate.Month == endDate.Month)
                        tempEndDate = endDate;

                    if (tempStartDate.Day == 1 && tempEndDate.Day == DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month))
                    {
                        if (!distItems.Exists(x => x.Date == tempStartDate.Date && x.AllocationId == row.ID))
                        {
                            distItem.Date = tempStartDate;
                            distItem.PctAllocation = pctAllocation;
                        }
                    }
                    else if (!distItems.Exists(x => x.Date == tempS.Date && x.AllocationId == row.ID))
                    {
                        int workingDays = uHelper.GetTotalWorkingDaysBetween(context, tempStartDate, tempEndDate, false);
                        int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(context, tempS, new DateTime(tempEndDate.Year, tempEndDate.Month, DateTime.DaysInMonth(tempEndDate.Year, tempEndDate.Month)));
                        double allpctPlannedWorkingHrs = (workingDays * workingHrsInDay) * pctAllocation / 100;
                        double pctAllocationInMonth = (allpctPlannedWorkingHrs * 100) / (workingDaysInMonth * workingHrsInDay);

                        distItem.Date = tempS;
                        distItem.PctAllocation = pctAllocationInMonth;
                    }

                    distItem.AllocationId = row.ID;
                    if (distItem.Date != DateTime.MinValue)
                        distItems.Add(distItem);

                    tempStartDate = tempEndDate.AddDays(1);
                }
            }

            return distItems;
        }

        public static void DistributeAllocationByMonth(ApplicationContext context, ResourceWorkItems workItem, List<ResourceDistributionItem> lstDistributions, bool updatePlannedOnly = false)
        {
            ResourceAllocationManager resourceAllocMGR = new ResourceAllocationManager(context);
            List<RResourceAllocation> lstallocation = resourceAllocMGR.Load(x => x.ResourceWorkItemLookup == workItem.ID);
            ResourceAllocationMonthlyManager objResourceAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            List<ResourceAllocationMonthly> oldResourceAllocationMonthlyList = null;
            
            oldResourceAllocationMonthlyList = objResourceAllocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == workItem.ID).ToList();
            if (oldResourceAllocationMonthlyList != null && oldResourceAllocationMonthlyList.Count > 0)
            {
                objResourceAllocationMonthlyManager.Delete(oldResourceAllocationMonthlyList);
            }

            foreach (var allocation in lstallocation)
            {
                string roleId = string.Empty;
                if (allocation != null)
                    roleId = allocation.RoleId;
                List<ResourceAllocationMonthly> resourceAllocationMonthlyList = objResourceAllocationMonthlyManager.Load(x => x.Resource == allocation.Resource
                                    && x.ResourceWorkItem == allocation.TicketID && x.GlobalRoleID == allocation.RoleId && x.ActualStartDate == allocation.AllocationStartDate
                                    && x.ActualEndDate == allocation.AllocationEndDate && !x.Deleted).ToList();
                
                List<ResourceAllocationMonthly> updated = new List<ResourceAllocationMonthly>();
                ResourceAllocationMonthly monthyListItem = null;
                ResourceDistributionItem item = null;
                List<ResourceDistributionItem> distributions = lstDistributions.FindAll(x => x.AllocationId == allocation.ID);

                for (int i = 0; i < distributions.Count; i++)
                {
                    item = distributions[i];
                    if (resourceAllocationMonthlyList != null)
                        monthyListItem = resourceAllocationMonthlyList.FirstOrDefault(x => x.MonthStartDate.Value.Date == item.Date.Date);
                    if (monthyListItem == null)
                        monthyListItem = new ResourceAllocationMonthly();
                    else
                        resourceAllocationMonthlyList.Remove(monthyListItem);

                    monthyListItem.ResourceWorkItemType = workItem.WorkItemType;
                    monthyListItem.Resource = workItem.Resource;
                    monthyListItem.ResourceWorkItemLookup = workItem.ID;
                    monthyListItem.ResourceWorkItem = workItem.WorkItem;
                    monthyListItem.ResourceSubWorkItem = workItem.SubWorkItem;
                    monthyListItem.GlobalRoleID = roleId;
                    monthyListItem.ActualStartDate = allocation.AllocationStartDate;
                    monthyListItem.ActualEndDate = allocation.AllocationEndDate;
                    monthyListItem.ActualPctAllocation = allocation.PctAllocation;

                    monthyListItem.MonthStartDate = item.Date;
                    if (updatePlannedOnly)
                    {
                        monthyListItem.PctPlannedAllocation = Math.Round(item.PctAllocation, 2);
                    }
                    else
                    {
                        monthyListItem.PctAllocation = Math.Round(item.PctAllocation, 2);
                    }

                    objResourceAllocationMonthlyManager.Save(monthyListItem);
                }

                //if (resourceAllocationMonthlyList != null)
                //{
                //    foreach (ResourceAllocationMonthly row in resourceAllocationMonthlyList)
                //    {
                //        monthyListItem = objResourceAllocationMonthlyManager.Get(row.ID);
                //        if (monthyListItem == null)
                //            continue;

                //        if (updatePlannedOnly && monthyListItem.PctAllocation.HasValue && monthyListItem.PctAllocation.Value > 0)
                //        {
                //            monthyListItem.PctPlannedAllocation = 0;
                //            objResourceAllocationMonthlyManager.Save(monthyListItem);
                //        }
                //        else if (!updatePlannedOnly && monthyListItem.PctPlannedAllocation.HasValue && monthyListItem.PctPlannedAllocation.Value > 0)
                //        {
                //            monthyListItem.PctAllocation = 0;
                //            objResourceAllocationMonthlyManager.Save(monthyListItem);
                //        }
                //        else
                //        {
                //            //isPlanned: true, removed entries which are releated to planned percentage only
                //            //isPlanned: false, removed entries which are releated to estimated percentage only
                //            objResourceAllocationMonthlyManager.Delete(monthyListItem);
                //        }
                //    }
                //}
            }
            
        }

        public static DataTable GetMonthlyDistributedAllocations(ApplicationContext context, List<long> workItemIDs)
        {
            DataTable data = null;
            ResourceAllocationMonthlyManager objAllocationMonthly = new ResourceAllocationMonthlyManager(context);
            List<ResourceAllocationMonthly> monthlyAllocationList = objAllocationMonthly.Load();
            List<string> expressions = new List<string>();
            if (workItemIDs.Count > 0)
            {
                foreach (int id in workItemIDs)
                {
                    expressions.Add(id.ToString());
                }
            }
            List<ResourceAllocationMonthly> rawData = monthlyAllocationList.Where(x => expressions.Contains(x.ResourceWorkItemLookup.ToString())).ToList();
            if (rawData == null || rawData.Count <= 0)
                return null;

            ILookup<long, ResourceAllocationMonthly> dataLookups = rawData.ToLookup(x => x.ResourceWorkItemLookup);
            List<DateTime> selectedMonths = new List<DateTime>();
            if (dataLookups.Count > 0)
            {
                data = new DataTable();
                foreach (var dLookup in dataLookups)
                {
                    List<ResourceAllocationMonthly> dataRows = dLookup.ToList();
                    DataRow dtRow = data.NewRow();
                    data.Rows.Add(dtRow);
                    foreach (ResourceAllocationMonthly sRow in dataRows)
                    {
                        DateTime rowDate = UGITUtility.StringToDateTime(sRow.MonthStartDate);
                        if (!selectedMonths.Exists(x => x.Date == rowDate.Date))
                        {
                            selectedMonths.Add(rowDate.Date);
                            data.Columns.Add(rowDate.ToString("MMM/dd/yyyy"), typeof(double));
                        }
                        dtRow[rowDate.ToString("MMM/dd/yyyy")] = Math.Round(UGITUtility.StringToDouble(sRow.PctAllocation), 0);
                    }
                }
            }

            return data;
        }

        public static void UpdateRMMSummaryAndMonthDistribution(ApplicationContext context, long workItemID, bool updatePlanned = false, List<RResourceAllocation> rResourceAlloc = null)
        {
            DistributeAllocationByMonth(context, workItemID, updatePlannedOnly: updatePlanned, rResourceAlloc: rResourceAlloc);
            UpdateRMMAllocationSummary(context, workItemID);
        }

        public static void UpdateRMMSummaryAndMonthDistribution(ApplicationContext context, List<long> workItemIDs, bool updatePlanned = false)
        {
            foreach (int workItemID in workItemIDs)
            {
                try
                {
                    DistributeAllocationByMonth(context, workItemID, updatePlannedOnly: updatePlanned);
                    UpdateRMMAllocationSummary(context, workItemID);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "UpdateRMMSummaryAndMonthDistribution");
                }
            }
        }

        public static void UpdateRMMMonthDistributionForUnassigned(ApplicationContext context, RResourceAllocation rAlloc)
        {

            DistributeUnassignedAllocationByMonth(context, rAlloc);

        }

        public static void DeleteRMMSummaryAndMonthDistribution(ApplicationContext context, List<long> workItemIds)
        {
            foreach (long workitemid in workItemIds)
            {
                DeleteRMMSummaryAndMonthDistribution(context, workitemid, string.Empty);
            }
        }

        /// <summary>
        /// Populate ResourceAllocationMonthly Table to keep allocation by month
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="allocationID"></param>
        public static void DistributeUnassignedAllocationByMonth(ApplicationContext context, RResourceAllocation rAlloc)
        {
            try
            {
                ResourceAllocationManager ObjAllocationManager = new ResourceAllocationManager(context);
                long workItemId = Convert.ToInt64(ObjAllocationManager.Save(rAlloc));
                if (workItemId > 0)
                {
                    rAlloc.ResourceWorkItemLookup = workItemId;
                    //new temp line for testing point of view..
                    // rAlloc.SaveAllocation(spWeb);

                    int workingHrsInDay = uHelper.GetWorkingHoursInADay(context);
                    Dictionary<DateTime, double> dataListByDates = new Dictionary<DateTime, double>();
                    //foreach (DataRow row in rAllocationItems.Rows)
                    //{
                    DateTime startDate = Convert.ToDateTime(rAlloc.AllocationStartDate); // Convert.ToDateTime(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.AllocationStartDate));
                    DateTime endDate = Convert.ToDateTime(rAlloc.AllocationEndDate);  //Convert.ToDateTime(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.AllocationEndDate));
                    double pctAllocation = Convert.ToInt32(rAlloc.PctAllocation); // Convert.ToDouble(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.PctAllocation));

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
                            int workingDays = uHelper.GetTotalWorkingDaysBetween(context, tempStartDate, tempEndDate, false);
                            int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(context, tempS, new DateTime(tempEndDate.Year, tempEndDate.Month, DateTime.DaysInMonth(tempEndDate.Year, tempEndDate.Month)));
                            double allctWorkingHrs = (workingDays * workingHrsInDay) * pctAllocation / 100;
                            double pctAllocationInMonth = (allctWorkingHrs * 100) / (workingDaysInMonth * workingHrsInDay);

                            dataListByDates.Add(tempS, pctAllocationInMonth);
                        }

                        tempStartDate = tempEndDate.AddDays(1);
                    }
                    //}

                    DateTime alldStartDate = dataListByDates.Keys.Min();
                    DateTime alldEndDate = dataListByDates.Keys.Max();

                    List<ResourceAllocationMonthly> tempSummaryMonth = new List<ResourceAllocationMonthly>();// new DataTable();

                    DateTime tempalldStartDate = alldStartDate;
                    while (tempalldStartDate <= alldEndDate)
                    {
                        ResourceAllocationMonthly newRow = new ResourceAllocationMonthly();// tempSummaryMonth.NewRow();
                        tempSummaryMonth.Add(newRow);
                        newRow.MonthStartDate = tempalldStartDate;
                        newRow.PctAllocation = 0;
                        if (dataListByDates.ContainsKey(tempalldStartDate))
                            newRow.PctAllocation = Convert.ToInt32(dataListByDates[tempalldStartDate]);

                        tempalldStartDate = tempalldStartDate.AddDays(DateTime.DaysInMonth(tempalldStartDate.Year, tempalldStartDate.Month));
                    }
                    ResourceAllocationMonthlyManager objAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
                    List<ResourceAllocationMonthly> ResourceAllocationMonthlyList = objAllocationMonthlyManager.Load();
                    List<ResourceAllocationMonthly> collection = ResourceAllocationMonthlyList.Where(x => x.ResourceWorkItemLookup == workItemId).ToList();//.GetSPListItemCollection(DatabaseObjects.Lists.ResourceAllocationMonthly, mQuery, spWeb);

                    int tempCount = 0;

                    for (int i = 0; i < tempSummaryMonth.Count; i++)
                    {
                        ResourceAllocationMonthly item = null;
                        if (i < collection.Count)
                        {
                            item = collection[i];
                            tempCount++;
                        }
                        else
                        {
                            item = new ResourceAllocationMonthly();// ResourceAllocationMonthlyList.AddItem();
                            item.PctPlannedAllocation = 0;
                        }
                        item.Resource = rAlloc.Resource; // rAllocationCollection[0][DatabaseObjects.Columns.Resource];
                        item.ResourceWorkItemLookup = rAlloc.ResourceWorkItemLookup;
                        item.PctAllocation = tempSummaryMonth[i].PctAllocation;
                        item.MonthStartDate = UGITUtility.StringToDateTime(tempSummaryMonth[i].MonthStartDate);
                        objAllocationMonthlyManager.Save(item);
                    }
                    List<long> deleteItemIDs = new List<long>();
                    //Delete extra entries in month view list
                    if (tempCount < collection.Count)
                    {
                        for (int i = tempCount; i < collection.Count; i++)
                        {
                            deleteItemIDs.Add(collection[i].ID);
                        }
                    }

                    // new function for delete junk entry from ResourceAllocationMonhtly
                    List<ResourceAllocationMonthly> dtJunkResourceAllocationMonthly = objAllocationMonthlyManager.Load().Where(x => x.ResourceWorkItemLookup == 0).ToList();
                    if (dtJunkResourceAllocationMonthly != null && dtJunkResourceAllocationMonthly.Count > 0)
                    {
                        foreach (ResourceAllocationMonthly dr in dtJunkResourceAllocationMonthly)
                        {
                            deleteItemIDs.Add(dr.ID);
                        }
                    }

                    if (deleteItemIDs.Count > 0)
                    {
                        deleteItemIDs = deleteItemIDs.Distinct().ToList();
                        deleteItemIDs.ForEach(x =>
                        {
                            ResourceAllocationMonthly allocationMonthly = objAllocationMonthlyManager.LoadByID(x);
                            if (allocationMonthly != null)
                                objAllocationMonthlyManager.Delete(allocationMonthly);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public static void DeleteRMMSummaryAndMonthDistribution(ApplicationContext context, long workItemID, string spWebUrl)
        {
            DeleteMonthlyDistributions(context, workItemID);
            UpdateRMMAllocationSummary(context, workItemID);
        }
        public static void DeleteAllProjectAllocationsByModule(ApplicationContext context, string module, string spWebUrl)
        {

            ResourceAllocationMonthlyManager ObjAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            // For resource monthly
            List<ResourceAllocationMonthly> dtMonthlyAllocation = ObjAllocationMonthlyManager.Load().Where(x => x.ResourceWorkItemType == module).ToList();
            if (dtMonthlyAllocation != null)
            {
                List<long> resourceMonthlyIds = new List<long>();
                foreach (ResourceAllocationMonthly dr in dtMonthlyAllocation)
                {
                    ObjAllocationMonthlyManager.Delete(dr);
                }
            }

            ResourceUsageSummaryMonthWiseManager objMonthlyWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            // For resource monthly summary
            List<ResourceUsageSummaryMonthWise> dtMonthlySummary = objMonthlyWiseManager.Load().Where(x => x.WorkItemType == module).ToList();
            if (dtMonthlySummary != null)
            {
                List<int> resourceMonthlySummaryIds = new List<int>();
                foreach (ResourceUsageSummaryMonthWise dr in dtMonthlySummary)
                {
                    if (dr != null)
                        objMonthlyWiseManager.Delete(dr);
                }
            }


            // For resource weekly summary
            ResourceUsageSummaryWeekWiseManager objWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            List<ResourceUsageSummaryWeekWise> dtWeeklySummary = objWeekWiseManager.Load().Where(x => x.WorkItemType == module).ToList();

            if (dtWeeklySummary != null)
            {
                foreach (ResourceUsageSummaryWeekWise dr in dtWeeklySummary)
                {
                    objWeekWiseManager.Delete(dr);
                }

            }
        }

        public static void DeleteAllAllocations(ApplicationContext context, string spWebUrl)
        {

            // For resource monthly
            ResourceAllocationMonthlyManager objAllocationMonthly = new ResourceAllocationMonthlyManager(context);
            List<ResourceAllocationMonthly> dtMonthlyAllocation = objAllocationMonthly.Load();

            if (dtMonthlyAllocation != null)
            {
                foreach (ResourceAllocationMonthly dr in dtMonthlyAllocation)
                {
                    if (dr != null)
                        objAllocationMonthly.Delete(dr);
                }
            }


            // For resource monthly summary
            ResourceUsageSummaryMonthWiseManager objSummaryMonthWise = new ResourceUsageSummaryMonthWiseManager(context);
            List<ResourceUsageSummaryMonthWise> dtMonthlySummary = objSummaryMonthWise.Load();
            //drs = dtMonthlySummary != null ? dtMonthlySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

            if (dtMonthlySummary != null)
            {
                foreach (ResourceUsageSummaryMonthWise dr in dtMonthlySummary)
                {
                    if (dr != null)
                        objSummaryMonthWise.Delete(dr);
                }

            }


            // For resource weekly summary
            ResourceUsageSummaryWeekWiseManager objSummaryWeekWise = new ResourceUsageSummaryWeekWiseManager(context);
            List<ResourceUsageSummaryWeekWise> dtWeeklySummary = objSummaryWeekWise.Load();// SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWeb);
            // drs = dtWeeklySummary != null ? dtWeeklySummary.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItemType, module)) : null;

            if (dtWeeklySummary != null)
            {

                foreach (ResourceUsageSummaryWeekWise dr in dtWeeklySummary)
                {
                    if (dr != null)
                        objSummaryWeekWise.Delete(dr);
                }

            }
        }

        public static void DeleteAllCRMAllocations(ApplicationContext context, string projectId)
        {
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            ProjectEstimatedAllocationManager cRMProjAllocMGR = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> dbCRMAllocations = cRMProjAllocMGR.Load(x => x.TicketId == projectId);
            cRMProjAllocMGR.Delete(dbCRMAllocations);

            List<RResourceAllocation> allocations = allocationManager.LoadByWorkItem(uHelper.getModuleNameByTicketId(projectId), projectId, null, 4, false, true);
            List<long> lstWorkItems = allocations.Select(x => x.ResourceWorkItemLookup).Distinct().ToList();
            if (lstWorkItems != null && lstWorkItems.Count > 0)
            {
                //Start Thread to update rmm summary list for deleting entry w.r.t current allocation
                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.DeleteRMMSummaryAndMonthDistribution(context, lstWorkItems); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.Start();
            }

            if (allocations != null && allocations.Count > 0)
            {
                allocationManager.Delete(allocations);
                ThreadStart threadStartMethod = delegate () { allocationManager.UpdateIntoCache(allocations[0], false); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.Start();
            }
        }

        public static void BatchDeleteListItems(ApplicationContext context, List<long> ids, string listName)
        {
            if (ids != null && ids.Count > 0)
            {

                DataTable list = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
                //string updateMethodFormat = "<Method ID=\"{0}\">" +
                // "<SetList>{1}</SetList>" +
                // "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                // "<SetVar Name=\"ID\">{2}</SetVar>" +
                // "</Method>";
                StringBuilder queryBuilder = new StringBuilder();
                foreach (int id in ids)
                {
                    DataRow dataRow = list.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, id))[0];
                    DeleteTnx(context, listName, dataRow);
                    //queryBuilder.AppendFormat(updateMethodFormat, id, list.ID, id);
                }

                //string batch = string.Format(batchFormat, queryBuilder.ToString());
                //spWeb.AllowUnsafeUpdates = true;
                // string batchReturn = spWeb.ProcessBatchData(batch);
                // spWeb.AllowUnsafeUpdates = false;
            }

        }
        public static int DeleteTnx(ApplicationContext context, string tableName, DataRow row)
        {
            using (SqlConnection con = new SqlConnection(context.Database))
            {
                row.Delete();
                int effectedRows = 0;
                SqlDataAdapter adp = new SqlDataAdapter(string.Format("select * from {0}", tableName), con);
                adp.UpdateBatchSize = 1;
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                builder.ConflictOption = ConflictOption.OverwriteChanges;
                DataRow[] insertRows = new DataRow[] { row };
                SqlCommand deleteCmd = builder.GetDeleteCommand(true).Clone();
                adp.DeleteCommand = deleteCmd;
                builder.Dispose();
                effectedRows = adp.Update(insertRows);

                if (effectedRows > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }


        public static void DeleteMonthlyDistributions(ApplicationContext context, long workItemID, long allocId = 0)
        {
            ResourceAllocationMonthlyManager objAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            List<ResourceAllocationMonthly> rAllocationMCollection = null;
            
            if (allocId > 0)
            {
                RResourceAllocation allocation = allocationManager.LoadByID(allocId);
                rAllocationMCollection = objAllocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == workItemID
                && x.ActualStartDate == allocation.AllocationStartDate && x.ActualEndDate == allocation.AllocationEndDate && x.ActualPctAllocation == allocation.PctAllocation).GroupBy(x => new { x.MonthStartDate }).Select(g => g.FirstOrDefault()).ToList();
            }
            else
            {
                rAllocationMCollection = objAllocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == workItemID).ToList();
            }

            if (rAllocationMCollection?.Count > 0)
            {
                objAllocationMonthlyManager.Delete(rAllocationMCollection);
            }
        }

        public static void DeleteMonthlyAndWeeklySummaryDistributions(ApplicationContext context, long workItemID, long allocId)
        {
            ResourceUsageSummaryWeekWiseManager objWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            ResourceUsageSummaryMonthWiseManager objMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            List<ResourceUsageSummaryWeekWise> oldWeekWiseDate = null;
            List<ResourceUsageSummaryMonthWise> oldMonthWiseData = null;

            if (allocId > 0)
            {
                RResourceAllocation allocation = allocationManager.LoadByID(allocId);
                
                oldWeekWiseDate = objWeekWiseManager.Load(x => x.WorkItemID == workItemID
                && x.ActualStartDate == allocation.AllocationStartDate && x.ActualEndDate == allocation.AllocationEndDate && x.ActualPctAllocation == allocation.PctAllocation).GroupBy(x => new { x.WeekStartDate }).Select(g => g.FirstOrDefault()).ToList();

                oldMonthWiseData = objMonthWiseManager.Load(x => x.WorkItemID == workItemID
                && x.ActualStartDate == allocation.AllocationStartDate && x.ActualEndDate == allocation.AllocationEndDate && x.ActualPctAllocation == allocation.PctAllocation).GroupBy(x => new { x.MonthStartDate }).Select(g => g.FirstOrDefault()).ToList();
            }
            else
            {
                oldWeekWiseDate = objWeekWiseManager.Load(x => x.WorkItemID == workItemID);
                oldMonthWiseData = objMonthWiseManager.Load(x => x.WorkItemID == workItemID);
            }

            if (oldWeekWiseDate?.Count > 0)
            {
                objWeekWiseManager.Delete(oldWeekWiseDate);
            }

            if (oldMonthWiseData?.Count > 0)
            {
                objMonthWiseManager.Delete(oldMonthWiseData);
            }
        }

        public static void DeleteAllocationsByTasks_old(ApplicationContext context, string ProjectPublicID, bool deleteWorkItems)
        {
            ResourceWorkItemsManager objWorkItemsManager = new ResourceWorkItemsManager(context);
            List<ResourceWorkItems> dt = objWorkItemsManager.Load().Where(x => x.WorkItem == ProjectPublicID).ToList();// (DatabaseObjects.Lists.ResourceWorkItems,spWeb);
                                                                                                                       // DataRow[] drs = dt != null ? dt.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.WorkItem, ProjectPublicID)) : null;
            if (dt != null && dt.Count > 0)
            {
                List<long> workItemIds = new List<long>();
                List<long> allocationIds = new List<long>();
                List<long> allocationMonthlyIds = new List<long>();
                List<long> allocationMonthwiseIds = new List<long>();
                List<long> allocationWeekwiseIds = new List<long>();

                string filterqry = "<Where><In><FieldRef Name=\"ResourceWorkItemLookup\" /><Values>";

                StringBuilder qry = new StringBuilder();
                foreach (ResourceWorkItems dr in dt)
                {
                    workItemIds.Add(UGITUtility.StringToInt(dr.ID));
                    filterqry += "<Value Type=\"Lookup\">" + dr.ID + "</Value>";
                }

                filterqry += "</Values></In></Where>";
                ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(context);
                ResourceTimeSheetManager objTimeSheetManager = new ResourceTimeSheetManager(context);

                List<RResourceAllocation> spListCollection = objAllocationManager.Load();// SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceAllocation, new SPQuery() { Query = filterqry }, spWeb);
                List<ResourceTimeSheet> spTimeSheetCollection = objTimeSheetManager.Load();//.GetSPListItemCollection(DatabaseObjects.Lists.ResourceTimeSheet, new SPQuery() { Query = filterqry }, spWeb);

                if (spTimeSheetCollection != null && spTimeSheetCollection.Count > 0)
                {
                    List<long> lstResourceSheetWorkItemIds = new List<long>();
                    foreach (ResourceTimeSheet itemTimeSheet in spTimeSheetCollection)
                    {
                        lstResourceSheetWorkItemIds.Add(itemTimeSheet.ResourceWorkItemLookup);
                    }

                    workItemIds = workItemIds.Where(x => !lstResourceSheetWorkItemIds.Contains(x)).ToList();
                }

                foreach (RResourceAllocation item in spListCollection)
                {
                    allocationIds.Add(UGITUtility.StringToInt(item.ID));
                }
                ResourceAllocationMonthlyManager objAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
                List<ResourceAllocationMonthly> dtResourceAllocationMonthly = objAllocationMonthlyManager.Load();// SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceAllocationMonthly,spWeb);
                List<ResourceAllocationMonthly> drResourceAllocationMonthly = null;
                if (workItemIds.Count > 0)
                    drResourceAllocationMonthly = dtResourceAllocationMonthly != null ? dtResourceAllocationMonthly.Where(x => workItemIds.Contains(x.ResourceWorkItemLookup)).ToList() : null;
                if (drResourceAllocationMonthly != null)
                {
                    foreach (ResourceAllocationMonthly dr in drResourceAllocationMonthly)
                    {
                        allocationMonthlyIds.Add(dr.ID);
                    }
                }
                ResourceUsageSummaryMonthWiseManager objSummaryMonthWise = new ResourceUsageSummaryMonthWiseManager(context);
                List<ResourceUsageSummaryMonthWise> dtResourceUsageSummaryMonthWise = objSummaryMonthWise.Load();// SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, spWeb);
                List<ResourceUsageSummaryMonthWise> drResourceUsageSummaryMonthWise = null;
                if (workItemIds.Count > 0)
                    drResourceUsageSummaryMonthWise = dtResourceUsageSummaryMonthWise != null ? dtResourceUsageSummaryMonthWise.Where(x => workItemIds.Contains(Convert.ToInt32(x.WorkItemID))).ToList() : null;
                if (drResourceUsageSummaryMonthWise != null)
                {
                    foreach (ResourceUsageSummaryMonthWise dr in drResourceUsageSummaryMonthWise)
                    {
                        allocationMonthwiseIds.Add(dr.ID);
                    }
                }

                ResourceUsageSummaryWeekWiseManager objSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                List<ResourceUsageSummaryWeekWise> dtResourceUsageSummaryWeekWise = objSummaryWeekWiseManager.Load();// SPListHelper.GetDataTable(DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWeb);
                List<ResourceUsageSummaryWeekWise> drResourceUsageSummaryWeekWise = null;
                if (workItemIds.Count > 0)
                    drResourceUsageSummaryWeekWise = dtResourceUsageSummaryWeekWise != null ? dtResourceUsageSummaryWeekWise.Where(x => workItemIds.Contains(Convert.ToInt32(x.WorkItemID))).ToList() : null;
                if (drResourceUsageSummaryWeekWise != null)
                {
                    foreach (ResourceUsageSummaryWeekWise dr in drResourceUsageSummaryWeekWise)
                    {
                        allocationWeekwiseIds.Add(dr.ID);
                    }
                }

                if (deleteWorkItems) // only delete if deleting task
                {
                    workItemIds.ForEach(x =>
                    {
                        ResourceWorkItems workItems = objWorkItemsManager.LoadByID(x);
                        objWorkItemsManager.Delete(workItems);
                    });
                }
                allocationIds.ForEach(x =>
                {
                    RResourceAllocation rResourceAllocation = objAllocationManager.LoadByID(x);
                    objAllocationManager.Delete(rResourceAllocation);
                });

                allocationMonthlyIds.ForEach(x =>
                {
                    ResourceAllocationMonthly rResourceAllocation = objAllocationMonthlyManager.LoadByID(x);
                    objAllocationMonthlyManager.Delete(rResourceAllocation);
                });
                allocationMonthlyIds.ForEach(x =>
                {
                    ResourceUsageSummaryMonthWise rResourceAllocation = objSummaryMonthWise.LoadByID(x);
                    objSummaryMonthWise.Delete(rResourceAllocation);
                });
                allocationWeekwiseIds.ForEach(x =>
                {
                    ResourceUsageSummaryWeekWise rResourceAllocation = objSummaryWeekWiseManager.LoadByID(x);
                    objSummaryWeekWiseManager.Delete(rResourceAllocation);
                });
                //BatchDeleteListItems(allocationIds, DatabaseObjects.Lists.ResourceAllocation, spWeb.Url);
                //BatchDeleteListItems(allocationMonthlyIds, DatabaseObjects.Lists.ResourceAllocationMonthly, spWeb.Url);
                //BatchDeleteListItems(allocationMonthwiseIds, DatabaseObjects.Lists.ResourceUsageSummaryMonthWise, spWeb.Url);
                //BatchDeleteListItems(allocationWeekwiseIds, DatabaseObjects.Lists.ResourceUsageSummaryWeekWise, spWeb.Url);
            }
        }

        public static void DeleteAllocationsByTasks(ApplicationContext context, string ProjectPublicID, bool deleteWorkItems)
        {
            try
            {
                ResourceWorkItemsManager objWorkItemsManager = new ResourceWorkItemsManager(context);
                List<ResourceWorkItems> dt = objWorkItemsManager.Load(x => x.WorkItem == ProjectPublicID);


                if (dt != null && dt.Count > 0)
                {
                    List<long> workItemIds = new List<long>();
                    foreach (ResourceWorkItems dr in dt)
                    {
                        workItemIds.Add(UGITUtility.StringToLong(dr.ID));
                    }
                    //Delete Timesheet allocations
                    ResourceTimeSheetManager TimeSheetManagerMGR = new ResourceTimeSheetManager(context);
                    List<ResourceTimeSheet> lstResourceTimesheets = TimeSheetManagerMGR.Load(x => workItemIds.Contains(x.ResourceWorkItemLookup));
                    if (lstResourceTimesheets != null && lstResourceTimesheets.Count > 0)
                        TimeSheetManagerMGR.Delete(lstResourceTimesheets);

                    //Delete TicketHours 
                    TicketHoursManager TicketHoursMGR = new TicketHoursManager(context);
                    List<ActualHour> lstActualHours = TicketHoursMGR.Load(x => x.TicketID == ProjectPublicID);
                    if(lstActualHours != null && lstActualHours.Count > 0)
                        TicketHoursMGR.Delete(lstActualHours);

                    //Delete Week Wise allocations
                    ResourceUsageSummaryWeekWiseManager SummaryWeekWiseManagerMGR = new ResourceUsageSummaryWeekWiseManager(context);
                    List<ResourceUsageSummaryWeekWise> lstResourceUsageSummaryWeekWise = SummaryWeekWiseManagerMGR.Load(x => x.WorkItem == ProjectPublicID);
                    if (lstResourceUsageSummaryWeekWise != null && lstResourceUsageSummaryWeekWise.Count > 0)
                        SummaryWeekWiseManagerMGR.Delete(lstResourceUsageSummaryWeekWise);

                    //Delete Month Wise allocations
                    ResourceUsageSummaryMonthWiseManager SummaryMonthWiseMGR = new ResourceUsageSummaryMonthWiseManager(context);
                    List<ResourceUsageSummaryMonthWise> lstResourceUsageSummaryMonthWise = SummaryMonthWiseMGR.Load(x => x.WorkItem == ProjectPublicID);
                    if (lstResourceUsageSummaryMonthWise != null && lstResourceUsageSummaryMonthWise.Count > 0)
                        SummaryMonthWiseMGR.Delete(lstResourceUsageSummaryMonthWise);

                    //Delete Resource Allocation Monthly
                    ResourceAllocationMonthlyManager ResourceAllocationMonthlyMGR = new ResourceAllocationMonthlyManager(context);
                    List<ResourceAllocationMonthly> lstResourceAllocationMonthly = ResourceAllocationMonthlyMGR.Load(x => x.ResourceWorkItem == ProjectPublicID);
                    if (lstResourceAllocationMonthly != null && lstResourceAllocationMonthly.Count > 0)
                        ResourceAllocationMonthlyMGR.Delete(lstResourceAllocationMonthly);

                    //Delete Resource Allocation 
                    ResourceAllocationManager ResourceAllocationMGR = new ResourceAllocationManager(context);
                    List<RResourceAllocation> lstResourceAllocation = ResourceAllocationMGR.Load(x => x.TicketID == ProjectPublicID);
                    if (lstResourceAllocation != null && lstResourceAllocation.Count > 0)
                        ResourceAllocationMGR.Delete(lstResourceAllocation);

                    //Delete Resource Estimated Allocations
                    ProjectEstimatedAllocationManager projectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(context);
                    List<ProjectEstimatedAllocation> lstProjectEstimatedAllocation = projectEstimatedAllocationMGR.Load(x => x.TicketId == ProjectPublicID);
                    if (lstProjectEstimatedAllocation != null && lstProjectEstimatedAllocation.Count > 0)
                        projectEstimatedAllocationMGR.Delete(lstProjectEstimatedAllocation);

                    //Delete Project Planned Allocations
                    ProjectAllocationManager ProjectAllocationMGR = new ProjectAllocationManager(context);
                    List<ProjectAllocation> lstProjectAllocation = ProjectAllocationMGR.Load(x => x.TicketID == ProjectPublicID);
                    if(lstProjectAllocation != null && lstProjectAllocation.Count > 0)
                        ProjectAllocationMGR.Delete(lstProjectAllocation);

                    //Delete Resource Workitems
                    ResourceWorkItemsManager ResourceWorkItemManager = new ResourceWorkItemsManager(context);
                    List<ResourceWorkItems> lstWorkItems = ResourceWorkItemManager.Load(x => x.WorkItem == ProjectPublicID);
                    if (lstWorkItems != null && lstWorkItems.Count > 0)
                        ResourceWorkItemManager.Delete(lstWorkItems);
                }
            }catch(Exception ex)
            {
                ULog.WriteException("Method DeleteAllocationsByTasks: " + ex.Message);
            }
        }


        public static void DeleteAllocationsByTask(ApplicationContext context, List<UGITTask> tasks, List<string> users, string moduleName, string projectPublicID)
        {
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            List<RResourceAllocation> allocations = allocationManager.LoadByWorkItem(moduleName, projectPublicID, null, 4, false, true);
            Dictionary<string, RResourceAllocation> updatedRA = new Dictionary<string, RResourceAllocation>();
            users = users.Distinct().ToList();

            DataRow projectItem = Ticket.GetCurrentTicket(context, moduleName, projectPublicID);

            DateTime projectStartDate = DateTime.MinValue;
            DateTime projectEndDate = DateTime.MinValue;

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualStartDate) && projectItem[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                projectStartDate = UGITUtility.StringToDateTime(projectItem[DatabaseObjects.Columns.TicketActualStartDate]);

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualCompletionDate) && projectItem[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                projectEndDate = UGITUtility.StringToDateTime(projectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(context);

            if (users != null)
            {
                #region existing assign users

                foreach (string user in users)
                {
                    List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Split(',').ToList().Exists(y => y == user)).ToList();

                    // double pctWorkingHrs = 0;
                    double totalPorjectWorkingHrs = 0;
                    double totalUserProjectHrs = 0;
                    double pctAllocation = 0;
                    UGITTaskManager taskManager = new UGITTaskManager(context);
                    UserProfileManager profileManager = new UserProfileManager(context);
                    foreach (UGITTask task in userTasks)
                    {
                        double totalPercentage = 0;
                        // bool isCheckPercentage = false;

                        List<UGITAssignTo> listAssignToPct = taskManager.GetUGITAssignPct(task.AssignToPct);

                        if (listAssignToPct.Count > 0)
                            totalPercentage = listAssignToPct.Sum(x => UGITUtility.StringToDouble(x.Percentage));

                        foreach (UGITAssignTo ugitAssignToItem in listAssignToPct)
                        {
                            UserProfile spUser = profileManager.GetUserById(user);
                            if (spUser != null && ugitAssignToItem.LoginName == spUser.UserName && totalPercentage > 0)
                            {
                                DateTime sDate = task.StartDate;
                                DateTime eDate = task.DueDate;
                                if (projectStartDate.Date > sDate.Date)
                                    sDate = projectStartDate.Date;
                                if (projectEndDate.Date < eDate.Date)
                                    eDate = projectEndDate.Date;

                                int taskWorkingDaysPctAllocation = uHelper.GetTotalWorkingDaysBetween(context, sDate, eDate);
                                totalUserProjectHrs += (UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * taskWorkingDaysPctAllocation * workingHrsInDay) / totalPercentage;
                            }
                        }
                    }

                    int taskWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, projectStartDate, projectEndDate);
                    totalPorjectWorkingHrs += taskWorkingDays * workingHrsInDay;

                    RResourceAllocation allocation = allocations.FirstOrDefault(x => x.Resource == user);
                    if (allocation == null)
                    {
                        allocation = new RResourceAllocation();
                        allocation.ResourceWorkItems = new ResourceWorkItems(user);
                        allocation.ResourceWorkItems.WorkItemType = moduleName;
                        allocation.ResourceWorkItems.WorkItem = projectPublicID;
                        allocation.ResourceWorkItems.SubWorkItem = string.Empty;
                        allocation.Resource = user;
                    }

                    // calculate the project allocation for user.
                    pctAllocation = (totalUserProjectHrs * 100) / totalPorjectWorkingHrs;
                    allocation.PctAllocation = (int)pctAllocation;

                    allocation.AllocationStartDate = projectStartDate;
                    allocation.AllocationEndDate = projectEndDate;


                    //calculate the user utilization.
                    double totalProjectLeafTaskHrs = 0;
                    totalProjectLeafTaskHrs = tasks.Where(x => x.ChildCount == 0).Sum(x => x.EstimatedHours);
                    if (totalProjectLeafTaskHrs > 0)
                    {
                        allocation.PctPlannedAllocation = Convert.ToInt32(totalUserProjectHrs * 100 / totalProjectLeafTaskHrs);
                    }

                    if (allocation.PctAllocation == 0 && (allocation.PctPlannedAllocation == 0 || totalProjectLeafTaskHrs == 0))
                        allocationManager.Delete(allocation);
                    else
                    {
                        allocationManager.Save(allocation);
                        updatedRA.Add(string.Format("updated{0}", user), allocation);
                        //Start Thread to update rmm summary list w.r.t current workitem
                        ThreadStart threadStartMethodMonthDistribution = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(context, allocation.ResourceWorkItemLookup); };
                        Thread sThreadMonthDistribution = new Thread(threadStartMethodMonthDistribution);
                        sThreadMonthDistribution.IsBackground = true;
                        sThreadMonthDistribution.Start();
                    }
                }
                #endregion
            }


            #region unassignUser
            ResourceWorkItemsManager ObjResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            RResourceAllocation rAllocation = null;
            rAllocation = new RResourceAllocation();
            List<ResourceWorkItems> wItems = ObjResourceWorkItemsManager.Load().Where(x => x.WorkItemType == projectPublicID).ToList();
            ResourceWorkItems newItems = wItems.FirstOrDefault(x => x.WorkItem == projectPublicID && x.Resource == "");

            if (newItems != null)
            {
                rAllocation.ResourceWorkItems = newItems;
            }
            else
            {
                rAllocation.ResourceWorkItems = new ResourceWorkItems();
                rAllocation.ResourceWorkItems.WorkItemType = moduleName;
                rAllocation.ResourceWorkItems.WorkItem = projectPublicID;
            }

            rAllocation.AllocationStartDate = projectStartDate;
            rAllocation.AllocationEndDate = projectEndDate;

            rAllocation.Resource = "";
            // rAllocation.ResourceName = string.Empty;

            int projectWorkingDay = uHelper.GetTotalWorkingDaysBetween(context, projectStartDate, projectEndDate);
            int totalWorkingHrs = (projectWorkingDay * workingHrsInDay);
            int pctAlloc = totalWorkingHrs > 0 ? (int)(tasks.Where(x => x.ChildCount == 0 && x.AssignToPct == string.Empty).Sum(x => x.EstimatedHours) * 100) / totalWorkingHrs : 0;
            if (rAllocation.PctAllocation != pctAlloc)
            {
                rAllocation.PctAllocation = pctAlloc;
                string webUrl = System.Web.HttpContext.Current.Request.Url.ToString();

                //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                //ParameterizedThreadStart threadstart = new ParameterizedThreadStart(delegate { RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(rAllocation, webUrl); });
                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(context, rAllocation); }; //rAllocation.WorkItemID
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
            #endregion

        }

        public static DataTable GetOpenTickstIds(ApplicationContext context)
        {
            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            List<string> lstmodules = new List<string> { "CPR", "OPM", "CNS", "PMM", "NPR" };
            DataTable dtOpenTickets = new DataTable();
            foreach (string module in lstmodules)
            {
                string moduleTablename = moduleManager.GetModuleTableName(module);
                if (!string.IsNullOrEmpty(moduleTablename))
                {
                    DataTable tmp = ticketManager.GetTickets($"select {DatabaseObjects.Columns.TicketId} from {moduleTablename} where ({DatabaseObjects.Columns.Closed} <> 1 or {DatabaseObjects.Columns.Closed} is null) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                    if (tmp != null && tmp.Rows.Count > 0)
                    {
                        dtOpenTickets.Merge(tmp);
                    }
                }
            }

            return dtOpenTickets;
        }

        public static DataTable GetClosedTicketIds(ApplicationContext context)
        {
            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            //List<string> lstmodules = new List<string> { "CPR", "OPM", "CNS", "PMM", "NPR" };
            string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dtClosedTickets = new DataTable();
            values.Add("@TenantID", context.TenantID);
            values.Add("@ModuleNames", ModuleNames);
            dtClosedTickets = uGITDAL.ExecuteDataSetWithParameters("usp_GetClosedTickets", values);
            //foreach (string module in lstmodules)
            //{
            //    string moduleTablename = moduleManager.GetModuleTableName(module);
            //    if (!string.IsNullOrEmpty(moduleTablename))
            //    {
            //        DataTable tmp = ticketManager.GetTickets($"select {DatabaseObjects.Columns.TicketId} from {moduleTablename} where ({DatabaseObjects.Columns.Closed} = 1) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            //        if (tmp != null && tmp.Rows.Count > 0)
            //        {
            //            dtClosedTickets.Merge(tmp);
            //        }
            //    }
            //}

            return dtClosedTickets;
        }

        public static DataTable GetOpenworkitems(ApplicationContext context, bool Isclosed)
        {
            string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
            DataTable dtopenitems = null;
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            values.Add("@ModuleNames", ModuleNames);
            values.Add("@Isclosed", Isclosed);
            dtopenitems = uGITDAL.ExecuteDataSetWithParameters("usp_GetOpenworkitems", values);
            return dtopenitems;
        }
        #region Clean Allocation

        public static void CleanAllocation(ApplicationContext context, ResourceWorkItems wItem, bool cleanEstimated = false, bool cleanPlanned = false, RResourceAllocation rAllocation = null, ResourceAllocationMonthly rAllocationMonthly = null)
        {
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            List<RResourceAllocation> allocs = allocationManager.LoadByWorkItem(wItem.WorkItemType, wItem.WorkItem, null, 4, false, true);
            allocs = allocs.Where(x => x.ResourceWorkItemLookup == wItem.ID).ToList();

            ResourceAllocationMonthlyManager objAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            List<ResourceAllocationMonthly> monthlyAllocs = objAllocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == wItem.ID);

            List<RResourceAllocation> itemsToDelete = new List<RResourceAllocation>();
            //if both need to clean then delete everything from allocation and monthly then summaries
            if (cleanEstimated && cleanPlanned)
            {
                if (allocs.Count > 0)
                {
                    allocationManager.Delete(allocs);
                    allocationManager.UpdateIntoCache(allocs[0], false);
                }

                if (monthlyAllocs.Count > 0)
                    objAllocationMonthlyManager.Delete(monthlyAllocs);
            }
            else if (cleanPlanned)
            {
                foreach (var aItem in allocs)
                {
                    //cleanup planned allocation then check if estimated is also not there then delete allocation
                    //otherwise update allocation

                    aItem.PctPlannedAllocation = 0;
                    aItem.PlannedStartDate = null;
                    aItem.PlannedEndDate = null;

                    if (UGITUtility.StringToDouble(aItem.PctEstimatedAllocation) > 0 &&
                        UGITUtility.StringToDateTime(aItem.EstStartDate) != DateTime.MinValue)
                    {
                        allocationManager.Update(aItem);
                    }
                    else
                    {
                        itemsToDelete.Add(aItem);
                    }
                }
                //Delete item those added in ItemstoDelete
                foreach (RResourceAllocation dItem in itemsToDelete)
                {

                    allocationManager.Delete(dItem);
                }
                //Cleanup monthly allocation based on workitemid
                DistributeAllocationByMonth(context, wItem.ID, updatePlannedOnly: true);

            }
            else if (cleanEstimated)
            {
                foreach (var aItem in allocs)
                {
                    //cleanup planned allocation then check if estimated is also not there then delete allocation
                    //otherwise update allocation
                    aItem.PctEstimatedAllocation = 0;
                    aItem.EstStartDate = null;
                    aItem.EstEndDate = null;

                    if (UGITUtility.StringToDouble(aItem.PctPlannedAllocation) > 0 &&
                        UGITUtility.StringToDateTime(aItem.PlannedStartDate) != DateTime.MinValue)
                    {
                        //assigned planned start and enddate to allocation start and end. 
                        //we can not leave allocation start and end date empty
                        aItem.PctAllocation = 0;
                        aItem.AllocationStartDate = aItem.PlannedStartDate;
                        aItem.AllocationEndDate = aItem.PlannedEndDate;
                        RResourceAllocation ifObjPersistsaItem = allocationManager.LoadByID(aItem.ID);
                        if (ifObjPersistsaItem != null)
                            allocationManager.Update(aItem);

                        //if we are cleaning estimation then for planned entries we alway keep single allocation againt planned.
                        //so delete other allocatio entries and break loop
                        //if (allocs.Count > i + 1)
                        //{
                        //    for (int d = i + 1; d < allocs.Count; d++)
                        //        itemsToDelete.Add(allocs[d]);
                        //    break;
                        //}
                    }
                    else
                    {
                        itemsToDelete.Add(aItem);
                    }
                }
                //Deleting those item that we need to delete
                foreach (var dItem in itemsToDelete)
                {
                    RResourceAllocation ifObjPersists = allocationManager.LoadByID(dItem.ID);
                    if (ifObjPersists != null)
                        allocationManager.Delete(dItem);
                }
                if (itemsToDelete != null && itemsToDelete.Count > 0)
                    allocationManager.UpdateIntoCache(itemsToDelete[0], true);
                //Cleanup monthly allocation based on workitemid
                DistributeAllocationByMonth(context, wItem.ID, updatePlannedOnly: false);
            }
            
            //Update RMM Allocation Summary After Cleane Allocation
            if (wItem.ID > 0)
            {
                ThreadStart threadStartMethod = delegate () { 
                    UpdateRMMAllocationSummary(context, wItem.ID);
                };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
        }

        public static void CleanAllocation(ApplicationContext context, string type, string workItem, string subWorkItem, string user, bool cleanEstimated = false, bool cleanPlanned = false)
        {
            RMMSummaryHelper.CleanAllocation(context, type, workItem, subWorkItem, new List<string>() { user }, cleanEstimated, cleanPlanned);
        }

        public static void CleanAllocation(ApplicationContext context, string type, string workItem, string subWorkItem, List<string> users, bool cleanEstimated = false, bool cleanPlanned = false)
        {
            if (users == null || users.Count == 0 || (!cleanEstimated && !cleanPlanned))
                return;

            // List<ResourceWorkItems> workItems = ResourceWorkItem.LoadWorkItemsById(context, type, workItem, subWorkItem);
            ResourceWorkItemsManager ObjresourceWorkItemsManager = new ResourceWorkItemsManager(context);
            List<ResourceWorkItems> workItems = ObjresourceWorkItemsManager.LoadWorkItemsById(type, workItem, subWorkItem);//GetTableData(DatabaseObjects.Tables.ResourceWorkItems).Select(workItemsQuery);


            if (workItems == null || workItems.Count == 0)
                return;

            //filter out selected workitems based on resources
            workItems = workItems.Where(x => users.Exists(y => y.ToString() == x.Resource)).ToList();
            if (workItems.Count == 0)
                return;

            //start cleaning up workitem
            foreach (ResourceWorkItems wItem in workItems)
            {
                CleanAllocation(context, wItem, cleanEstimated, cleanPlanned);
            }

        }

        #endregion
        public static string GetSelectedDepartmentsInfo(ApplicationContext context, string selectedDeptIds, bool enableDivision)
        {
            DepartmentManager departmentManager = new DepartmentManager(context);
            CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
            List<Department> departments = departmentManager.GetDepartmentData();
            List<CompanyDivision> companyDivisions = companyDivisionManager.GetCompanyDivisionData();

            string selectedDepartmentText = "";
            string divisionTitle = "";
            List<String> lstDeptIDs = selectedDeptIds.Replace(", ", ",").Split(',').ToList();
            //Utility.Department selectedDepartment = departments.FirstOrDefault(x => x.ID == departmentID && x.Deleted != true);
            List<Department> deptData = departments.Where(x => lstDeptIDs.Any(y => y == x.ID.ToString())).ToList();
            // Add Division
            int newLineIndex = 0;
            foreach (Department dept in deptData)
            {
                if (enableDivision && dept.DivisionLookup != null && dept.DivisionLookup.ID != null &&
                    dept.DivisionLookup.Value != "N/A" && dept.CompanyLookup != null && dept.DivisionLookup.Value != dept.CompanyLookup.Value)
                {
                    //string str = fieldConfigMgr.GetFieldConfigurationData(DatabaseObjects.Columns.DivisionLookup, Convert.ToString(dept.DivisionLookup.ID));
                    divisionTitle = companyDivisions.FirstOrDefault(cd => cd.ID == dept.DivisionIdLookup).Title;
                    selectedDepartmentText = selectedDepartmentText + divisionTitle + " > " + dept.Title + ", ";
                    if (selectedDepartmentText.Substring(newLineIndex).Length > 75) {
                        selectedDepartmentText = selectedDepartmentText + "\n";
                        newLineIndex = selectedDepartmentText.Length;
                    }
                }
            }
            if (!string.IsNullOrEmpty(selectedDepartmentText))
                selectedDepartmentText = selectedDepartmentText.Remove(selectedDepartmentText.Length - 2);
            return selectedDepartmentText;
        }

        //This method is created to pick the black/white font colour dynamically based on background colour of bars.
        public static List<string> SetFontColors(List<string> lstColors)
        {
            string item;
            string Estimatecolor = "";
            string foreColor = "";
            for (int i = 0; i < lstColors.Count; i++)
            {
                item = lstColors[i];
                foreColor = "#000000"; //default font color is set to black
                Estimatecolor = UGITUtility.SplitString(item, Constants.Separator, 1);
                if (!string.IsNullOrEmpty(Estimatecolor))
                {
                    Color backgroundColor = ColorTranslator.FromHtml(Estimatecolor);
                    if (backgroundColor.R * 0.2126 + backgroundColor.G * 0.7152 + backgroundColor.B * 0.0722 < 255 / 2)
                    {
                        foreColor = "#FFFFFF";//when backgroundColor is dark, font color will be white.
                    }
                }
                lstColors[i] = item + Constants.Separator + foreColor;
            }
            return lstColors;
        }

        public static DataSet GetCorruptAllocations(ApplicationContext context, string tabname, bool IncludedClosed)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@TenantID", context.TenantID);
                arrParams.Add("@tabname", tabname);
                arrParams.Add("@IncludedClosed", IncludedClosed);
                ds = uGITDAL.ExecuteDataSet_WithParameters("usp_getCorruptedAllocations", arrParams);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);   
            }
            return ds;
        }

        public static bool FillResourceUtilization(ApplicationContext context)
        {
            bool status = false;
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                values.Add("@url", UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx"));
                uGITDAL.ExecuteDataSetWithParameters("usp_fillResourceUtilization", values);

                Dictionary<string, object> valuesFooter = new Dictionary<string, object>();
                valuesFooter.Add("@TenantID", context.TenantID);
                uGITDAL.ExecuteDataSetWithParameters("usp_fillResourceSummaryUtilization_FooterData", valuesFooter);
                status= true;
            }
            catch (Exception ex)
            {
                status = false;
                ULog.WriteException(ex);
            }
            return status;
        }
        public static DataSet GetDirectorViewData(ApplicationContext context, string sector, int division, int studio, string startDate, string endDate, bool closed)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@tenantID", context.TenantID);
            values.Add("@division", UGITUtility.StringToLong(division));
            values.Add("@studio", UGITUtility.ObjectToString(studio));
            values.Add("@sector", UGITUtility.ObjectToString(sector));
            values.Add("@closed", closed == true ? 1 : 0);
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime aStartdate = UGITUtility.StringToDateTime(startDate);
                values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime aEnddate = UGITUtility.StringToDateTime(endDate);
                values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            DataSet dsData = GetTableDataManager.GetDataSet("ProjectChart", values);
            return dsData;
        }
    }
}
