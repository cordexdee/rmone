using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class FindResourceAvailability : System.Web.UI.UserControl
    {
        #region variables
        public string ControlId { get; set; }
        public string FrameId;
        public bool ReadOnly;
        protected static string peopleGroupName;
        protected static string ticketId;
        string varUseCalendar = string.Empty;
        DataTable resultTable;
        ////SPListItem spProjectItem;
         DataRow spProjectItem;

        List<string> lstFontColor = null;
        private ApplicationContext _context = null;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        UserProfileManager UserProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ResourceAllocationMonthlyManager ObjResourceAllocationMonthlyManager = new ResourceAllocationMonthlyManager(HttpContext.Current.GetManagerContext());
        ResourceAllocationManager ObjResourceAllocationManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager ObjConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        TicketManager TicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
        #endregion

        #region pageEvents
        protected override void OnInit(EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
            //set font color on the base of configuration variable.
            string strResourceAllocationColor = ObjConfigManager.GetValue(ConfigConstants.ResourceAllocationFontColor);        // ConfigurationVariable.GetValue(SPContext.Current.Web, ConfigConstants.ResourceAllocationFontColor);

            if (!string.IsNullOrEmpty(strResourceAllocationColor))
            {
                lstFontColor = strResourceAllocationColor.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            varUseCalendar = ObjConfigManager.GetValue(DatabaseObjects.Columns.UseCalendar);       // ConfigurationVariable.GetValue(SPContext.Current.Web, DatabaseObjects.Columns.UseCalendar);
            ticketId = Request["ticketId"];

            string moduleName = uHelper.getModuleNameByTicketId(ticketId);
            if (moduleName == ModuleNames.CPR || moduleName == ModuleNames.OPM)
                divComplexityType.Visible = true;
            spProjectItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketId), ticketId);

            
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["pStartDate"]) && Request["pStartDate"] != "NaN")
                    hdndtfrom.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pStartDate"]).Year, Convert.ToDateTime(Request["pStartDate"]).Month, 1));
                else
                    hdndtfrom.Value = Convert.ToString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

                if (!string.IsNullOrEmpty(Request["pStartDate"]) && Request["pStartDate"] != "NaN")
                    hdndtto.Value = Convert.ToString(new DateTime(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month, System.DateTime.DaysInMonth(Convert.ToDateTime(Request["pEndDate"]).Year, Convert.ToDateTime(Request["pEndDate"]).Month)));
                else
                    hdndtto.Value = Convert.ToString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)));

                hdndisplayMode.Value = DisplayMode.Monthly.ToString();


                if (!gvResourceAvailablity.IsCallback)
                {
                    gvResourceAvailablity.ExpandAll();
                    PrepareAllocationGrid();

                    gvResourceAvailablity.DataSource = GetAllocationData();
                    gvResourceAvailablity.DataBind();
                }
            }
            

        }

        protected override void OnPreRender(EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            if (chklstComplexitytype.Items != null && chklstComplexitytype.Items.FindByText("Capacity").Selected == false)
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = false;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = false;
            }
            else
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = true;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = true;
            }
            base.OnPreRender(e);
        }
        #endregion

        #region gridevent
        protected void gvResourceAvailablity_DataBinding(object sender, EventArgs e)
        {
            //if (gvResourceAvailablity.DataSource == null)
            //{
            //    gvResourceAvailablity.DataSource = GetAllocationData();
            //}
        }

        protected void gvResourceAvailablity_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            //{
            //    DataRow currentRow = gvResourceAvailablity.GetDataRow(e.VisibleIndex);

            //    if (currentRow != null && e.Row.Cells.Count > 1)
            //    {
            //        e.Row.Attributes.Add("onmouseover", string.Format("showTaskActions(this,{0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
            //        e.Row.Attributes.Add("onmouseout", string.Format("hideTaskActions(this,{0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
            //    }
            //}

            if (e.RowType == GridViewRowType.Group)
            {
                DataRow row = gvResourceAvailablity.GetDataRow(e.VisibleIndex);

                e.Row.Cells[0].Text = string.Format("{0} ({1})", row[DatabaseObjects.Columns.GroupName], gvResourceAvailablity.GetChildRowCount(e.VisibleIndex));
            }
            if(e.RowType == GridViewRowType.Data)
            {
                DataRow row = gvResourceAvailablity.GetDataRow(e.VisibleIndex);
                JobTitleManager jobTitleManager = new JobTitleManager(HttpContext.Current.GetManagerContext());
                string rowUser = Convert.ToString(row[DatabaseObjects.Columns.Id]);
                UserProfile aUser = UserProfileManager.LoadById(rowUser);
                if (chklstComplexitytype.Items != null && chklstComplexitytype.Items.FindByText("Complexity").Selected == true)
                {                     
                    //List<SummaryResourceProjectComplexity> userComplexities = complexityManager.Load(x => x.UserId == rowUser);
                    //int projectCount = userComplexities.Sum(x => x.Count);
                    //JobTitle jobTitle = jobTitleManager.LoadByID(aUser.JobTitleLookup);
                    //if (jobTitle != null)
                    //{
                    //    if (projectCount < jobTitle.LowProjectCapacity)
                    //    {
                    //        e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFF200");
                    //    }
                    //    else if (projectCount >= jobTitle.LowProjectCapacity && projectCount <= jobTitle.HighProjectCapacity)
                    //    {
                    //        e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#4CBB17");
                    //    }
                    //    else
                    //    {
                    //        e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FF2400");
                    //    }
                    //}
                }

                if(chklstComplexitytype.Items != null && chklstComplexitytype.Items.FindByText("Capacity").Selected == true)
                {
                    ProjectEstimatedAllocationManager allocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
                    
                    JobTitle jobTitle = jobTitleManager.LoadByID(aUser.JobTitleLookup);
                    List<SummaryResourceProjectComplexity> userComplexities = complexityManager.Load(x => x.UserId == rowUser);
                    if (userComplexities != null && userComplexities.Count > 0)
                    {
                        double totalProjectCost = userComplexities.Sum(x=>x.HighProjectCapacity);
                        int totalProjectCount = userComplexities.Sum(x => x.Count);
                        if (jobTitle != null)
                        {
                            //code to color project capacity column
                            e.Row.Cells[2].Text = UGITUtility.ObjectToString(totalProjectCount);
                            if (totalProjectCount <= jobTitle.LowProjectCapacity)
                            {
                                e.Row.Cells[2].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#baf0d7");
                            }
                            else if (totalProjectCount > jobTitle.HighProjectCapacity)
                            {
                                e.Row.Cells[2].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcf7b5");
                            }
                            else
                            {
                                e.Row.Cells[2].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#f8ac4a");
                            }


                            e.Row.Cells[3].Text = UGITUtility.FormatNumber(UGITUtility.StringToDouble(totalProjectCost), "currency");
                            //code to color Revenue column
                            if (totalProjectCost < jobTitle.LowRevenueCapacity)
                            {
                                e.Row.Cells[3].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#baf0d7");
                            }
                            else if (totalProjectCost > jobTitle.HighRevenueCapacity)
                            {
                                e.Row.Cells[3].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fcf7b5");
                            }
                            else
                            {
                                e.Row.Cells[3].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#f8ac4a");
                            }

                        }
                    }
                }

            }
        }

        protected void gvResourceAvailablity_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            
            if (e.CellValue == null)
                return;

            if (e.DataColumn.FieldName != "#" && e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != DatabaseObjects.Columns.GroupName && e.DataColumn.FieldName != DatabaseObjects.Columns.ItemOrder)
            {
                if (!e.DataColumn.FieldName.Contains("ALL"))
                {
                    string ColFontColor = "#000000";
                    int colValue;
                    if (rbtnProject.Checked)
                        colValue = UGITUtility.StringToInt(e.CellValue.ToString());
                    else
                        colValue = Convert.ToInt32(UGITUtility.ObjectToString(gvResourceAvailablity.GetRowValues(e.VisibleIndex, e.DataColumn.FieldName + "ALL")));

                    if (lstFontColor != null)
                    {
                        if (lstFontColor.Count == 1)
                        {
                            string[] coloritem = lstFontColor[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                            if (colValue >= UGITUtility.StringToInt(coloritem[0]))
                            {
                                ColFontColor = coloritem[1];
                            }
                        }
                        if (lstFontColor.Count == 2)
                        {
                            string[] coloritem = lstFontColor[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                            string[] coloritem1 = lstFontColor[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);

                            if (colValue >= UGITUtility.StringToInt(coloritem[0]))
                            {
                                ColFontColor = coloritem[1];
                            }

                            if (colValue < UGITUtility.StringToInt(coloritem[0]) && colValue > UGITUtility.StringToInt(coloritem1[0]))
                            {
                                ColFontColor = coloritem1[1];
                            }
                        }

                    }

                    if (rbtnProject.Checked)
                    {
                        e.Cell.Text = string.Format("{0}%", e.CellValue.ToString());
                        e.Cell.ForeColor = System.Drawing.ColorTranslator.FromHtml(ColFontColor);
                    }
                    if (rbtnTotal.Checked)
                    {
                        e.Cell.Text = string.Format("{0}%", Convert.ToInt32(UGITUtility.ObjectToString(gvResourceAvailablity.GetRowValues(e.VisibleIndex, e.DataColumn.FieldName + "ALL"))));
                        e.Cell.ForeColor = System.Drawing.ColorTranslator.FromHtml(ColFontColor);
                    }
                    if (rbtnProjectTotal.Checked)
                    {
                        e.Cell.Text = string.Format("{0}% / <span style='color:{2}'>{1}%<span>", e.CellValue.ToString().Trim(), Convert.ToInt32(UGITUtility.ObjectToString(gvResourceAvailablity.GetRowValues(e.VisibleIndex, e.DataColumn.FieldName + "ALL"))), ColFontColor);
                    }

                }

            }
        }
        #endregion

        #region helper method
        private int GetDaysForDisplayMode(string dMode, DateTime dt)
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
                default:
                    break;
            }
            return days;
        }

        public DateTime FirstDayOfWeek(DateTime date)
        {
            var candidateDate = date;
            while (candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                candidateDate = candidateDate.AddDays(-1);
            }
            return candidateDate;
        }

        private int GetISOWeek(DateTime d)
        {
            return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private DataTable SummarizeDataTable(DataTable dtStaffSheet, string ColToSum)
        {
            var rows = dtStaffSheet.AsEnumerable();
            var columns = dtStaffSheet.Columns.Cast<DataColumn>();
            string columnToGroup = ColToSum;
            DataColumn colToGroup = columns.First(c => c.ColumnName.Equals(columnToGroup, StringComparison.OrdinalIgnoreCase));
            //var colsToSum = columns.Where(c => c != colToGroup && c.Caption != "ItemOrder" && c.Caption != "ID" && c.Caption != "FullAllocation" && c.Caption != "GroupName");
            var colsToSum = columns.Where(c => c != colToGroup && c.Caption != "ItemOrder" && c.Caption != "Id" && c.Caption != "FullAllocation" && c.Caption != "GroupName" && c.Caption != "Resource");
            var columnsToSum = new HashSet<DataColumn>(colsToSum);

            resultTable = dtStaffSheet.Clone(); // empty table, same schema
            foreach (var group in rows.GroupBy(r => r[colToGroup]))
            {
                DataRow row = resultTable.Rows.Add();
                foreach (var col in columns)
                {
                    if (columnsToSum.Contains(col))
                    {
                        double sum = group.Sum(r => r.Field<string>(col) == null ? 0 : Convert.ToDouble(r.Field<string>(col)));
                        row[col.ColumnName] = Convert.ToInt32(sum); //sum.ToString("N2");
                    }
                    else
                        row[col.ColumnName] = group.First()[col];
                }
            }


            //foreach (var group in rows.GroupBy(row => new { Resource = row.Field<string>("Resource"), GroupName = row.Field<string>("GroupName") }))
            //{
            //    DataRow row = resultTable.Rows.Add();
            //    foreach (var col in columns)
            //    {
            //        if (columnsToSum.Contains(col))
            //        {
            //            double sum = group.Sum(r => r.Field<string>(col) == null ? 0 : Convert.ToDouble(r.Field<string>(col)));
            //            row[col.ColumnName] = Convert.ToInt32(sum); //sum.ToString("N2");
            //        }
            //        else
            //            row[col.ColumnName] = group.First()[col];
            //    }
            //}
            return resultTable;
        }
        #endregion

        #region GetData
        private DataTable GetAllocationData()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.GroupName, typeof(string));
            data.Columns.Add("UserIdGroup", typeof(string));

            for (DateTime dt = Convert.ToDateTime(hdndtfrom.Value); Convert.ToDateTime(hdndtto.Value) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                if (!data.Columns.Contains(dt.ToString("MMM-dd-yy")))
                    data.Columns.Add(dt.ToString("MMM-dd-yy"), typeof(string));

                if (!data.Columns.Contains(dt.ToString("MMM-dd-yy") + "ALL"))
                    data.Columns.Add(dt.ToString("MMM-dd-yy") + "ALL", typeof(string));
            }
            data.Columns.Add("FullAllocation", typeof(string));


            DataTable dtAllResouceAllocation = null;
            DataTable dtAllProjectResouceAllocation = null;
            DataTable dtAllAllocation = null;
            #region weekly data
            //new block for performance related changes ...
            if (hdndisplayMode.Value == "Weekly" || hdndisplayMode.Value == "Daily")
            {
                //get all resource allocation
                dtAllResouceAllocation = LoadRawTableByResource();
                //get the workitem for the particular ticket.
                DataTable dtWorkitem = GetAllWorkItemforTicket();

                //get the all workitem
                DataTable dtAllWorkitem = GetAllWorkItem();


                // DataTable dtAllProjectResourceAllocation = dtAllResouceAllocation

                // get project data.. for partical ticket form the all data..
                if (dtWorkitem != null && dtAllResouceAllocation != null)
                {
                    var JoinResult = (from amw in dtAllResouceAllocation.AsEnumerable()
                                      from up in dtWorkitem.AsEnumerable()
                                      where amw.Field<Int64>(DatabaseObjects.Columns.ResourceWorkItemLookup) == Convert.ToInt64(up.Field<Int64>(DatabaseObjects.Columns.Id))
                                      select new
                                      {
                                          Id = up.Field<Int64>(DatabaseObjects.Columns.Id),
                                          Resource = amw.Field<string>(DatabaseObjects.Columns.Resource),
                                          //ResourceWorkItemType = amw.Field<string>(DatabaseObjects.Columns.ResourceWorkItemType),
                                          AllocationStartDate = amw.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate),
                                          AllocationEndDate = amw.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate),
                                          PctAllocation = amw.Field<double>(DatabaseObjects.Columns.PctAllocation),
                                          SubWorkItem = up.Field<string>(DatabaseObjects.Columns.SubWorkItem)
                                      }).ToList();

                      dtAllProjectResouceAllocation = UGITUtility.LINQResultToDataTable(JoinResult);
                    
                }

                // get All data..
                if (dtAllWorkitem != null && dtAllResouceAllocation != null)
                {
                    var JoinResult = (from amw in dtAllResouceAllocation.AsEnumerable()
                                      from up in dtAllWorkitem.AsEnumerable()
                                      where amw.Field<Int64>(DatabaseObjects.Columns.ResourceWorkItemLookup) == up.Field<Int64>(DatabaseObjects.Columns.Id)
                                      select new
                                      {
                                          Id = up.Field<Int64>(DatabaseObjects.Columns.Id),
                                          Resource = amw.Field<string>(DatabaseObjects.Columns.Resource),
                                          //ResourceWorkItemType = amw.Field<string>(DatabaseObjects.Columns.ResourceWorkItemType),
                                          AllocationStartDate = amw.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate),
                                          AllocationEndDate = amw.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate),
                                          PctAllocation = amw.Field<double>(DatabaseObjects.Columns.PctAllocation),
                                          SubWorkItem = up.Field<string>(DatabaseObjects.Columns.SubWorkItem)
                                      }).ToList();

                    dtAllAllocation = UGITUtility.LINQResultToDataTable(JoinResult);
                }
            }
            #endregion

            //new block..  
            DataTable dttempnew = data.Clone();

            if (Request["pGlobalRoleID"] != null)
            {
                //group wise distribution.
                string[] arrayGroupName = null;
                string groups = Convert.ToString(Request["pGlobalRoleID"]);
                if (!string.IsNullOrEmpty(groups))
                {
                    if (groups.Contains(";"))
                    {
                        arrayGroupName = groups.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else if (groups.Contains(","))
                    {
                        arrayGroupName = groups.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                        arrayGroupName = new string[] { groups };
                }

                DataTable dtMonthlyAllocation = null;
                DataTable dtProjectMonthlyAllocation = null;

                if (hdndisplayMode.Value == "Monthly")
                {
                    dtMonthlyAllocation = LoadAllocationMonthlyView(null);
                    dtProjectMonthlyAllocation = LoadAllocationMonthlyView(ticketId);
                }
                //convert role display name to name
                List<GlobalRole> groupNames = new List<GlobalRole>();
                GlobalRoleManager roleMgr = new GlobalRoleManager(_context);
                foreach (string itemGroupName in arrayGroupName)
                {
                    GlobalRole role = roleMgr.Get(x => x.Name == itemGroupName);
                    if (role != null)
                        groupNames.Add(role);
                }

                foreach (GlobalRole globalRole in groupNames)
                {
                    List<UserProfile> lstUProfile = UserProfileManager.GetUsersByGlobalRoleID(globalRole.Id);

                    if (lstUProfile == null)
                        continue;

                    if (chklstComplexitytype.SelectedItem != null && chklstComplexitytype.SelectedItem.Text == "Complexity")
                    {
                        int projectComplexity = Convert.ToInt32(spProjectItem[DatabaseObjects.Columns.ProjectComplexity]);
                        ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
                        List<SummaryResourceProjectComplexity> complexityAboveCurrentCRP = complexityManager.Load(x => Convert.ToInt32(x.Complexity) >= projectComplexity);
                        lstUProfile = lstUProfile.FindAll(x => complexityAboveCurrentCRP.Any(y => y.UserId == x.Id)).ToList();
                    }
                    
                    int ctr = 1;
                    foreach (UserProfile userProfile in lstUProfile)
                        {
                            DataRow newRow = null;
                            #region weekly and daily
                            if (hdndisplayMode.Value == "Weekly" || hdndisplayMode.Value == "Daily")
                            {
                                newRow = data.NewRow();
                                newRow[DatabaseObjects.Columns.Id] = userProfile.Id;
                                newRow[DatabaseObjects.Columns.ItemOrder] = ctr++;
                                newRow[DatabaseObjects.Columns.Resource] = userProfile.Name;
                                newRow[DatabaseObjects.Columns.GroupName] = globalRole;

                                newRow["UserIdGroup"] = userProfile.Id + Constants.Separator + globalRole.Id;

                                DataTable table = null;

                                if (dtAllAllocation != null && dtAllAllocation.Rows.Count > 0)
                                {
                                    var resultAllResouceAllocation = from amw in dtAllAllocation.AsEnumerable()
                                                                     where amw.Field<string>(DatabaseObjects.Columns.Resource) == Convert.ToString(userProfile.Id)
                                                                     where amw.Field<string>(DatabaseObjects.Columns.SubWorkItem) == globalRole.Name.Trim()
                                                                     select amw;


                                    if (resultAllResouceAllocation != null && resultAllResouceAllocation.Count() > 0)
                                        table = resultAllResouceAllocation.CopyToDataTable();
                                }

                                DataTable tableProject = null;
                                if (dtAllProjectResouceAllocation != null && dtAllProjectResouceAllocation.Rows.Count > 0)
                                {
                                    var resultAllProjectResouceAllocation = from apra in dtAllProjectResouceAllocation.AsEnumerable()
                                                                            where apra.Field<string>(DatabaseObjects.Columns.Resource) == Convert.ToString(userProfile.Id)
                                                                            where apra.Field<string>(DatabaseObjects.Columns.SubWorkItem) == globalRole.Name.Trim()
                                                                            select apra;

                                    if (resultAllProjectResouceAllocation != null && resultAllProjectResouceAllocation.Count() > 0)
                                        tableProject = resultAllProjectResouceAllocation.CopyToDataTable();

                                }


                                for (DateTime dt = Convert.ToDateTime(hdndtfrom.Value); Convert.ToDateTime(hdndtto.Value) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                                {
                                    newRow[dt.ToString("MMM-dd-yy") + "ALL"] = string.Format("{0}", ObjResourceAllocationManager.AllocationPercentageWithProjectType(dtAllResouceAllocation, userProfile.Id, 4, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), false, null, tableProject, false, 0));
                                    //newRow[dt.ToString("MMM-dd-yy")] = string.Format("{0} ", ResourceAllocation.ProjectAllocationPercentage(userProfile.ID, 4, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), ticketId, false));

                                    // newRow[dt.ToString("MMM-dd-yy") + "ALL"] = string.Format("{0} ", ResourceAllocation.AllocationPercentageWithProjectType(tableProject, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), false, null));
                                    //newRow[dt.ToString("MMM-dd-yy") + "ALL"] = string.Format("{0} ", ResourceAllocation.AllocationPercentageWithProjectType(table, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), false, null));
                                    newRow[dt.ToString("MMM-dd-yy")] = string.Format("{0}", ObjResourceAllocationManager.ProjectAllocationPercentage(userProfile.Id, 4, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), ticketId));

                                }
                            }
                            #endregion

                            #region Monthly allocation
                            if (hdndisplayMode.Value == "Monthly")
                            {
                                if (dtMonthlyAllocation != null && dtMonthlyAllocation.Rows.Count > 0)
                                {
                                    resultTable = data.Clone();
                                    foreach (DataRow row in dtMonthlyAllocation.Rows)
                                    {
                                        string valueResource = Convert.ToString(row[DatabaseObjects.Columns.Resource]);
                                        if (valueResource == null)
                                            continue;

                                        string valueResourceSubWorkItem = Convert.ToString(row[DatabaseObjects.Columns.ResourceSubWorkItem]);
                                        if (valueResourceSubWorkItem == null)
                                            continue;

                                        if (valueResource == userProfile.Id && valueResourceSubWorkItem == globalRole.Name.Trim())
                                        {
                                            if (rbtnAllResource.Checked || rbtnPartiallyAvailable.Checked)
                                            {
                                                if (globalRole.Name.Trim() == valueResourceSubWorkItem)
                                                {
                                                    DataRow tRow = dttempnew.NewRow();
                                                    tRow[DatabaseObjects.Columns.Id] = valueResource;
                                                    tRow[DatabaseObjects.Columns.ItemOrder] = ctr++;
                                                    tRow[DatabaseObjects.Columns.Resource] = userProfile.Name;
                                                    double monthlyAllocation = Convert.ToDouble(row[DatabaseObjects.Columns.PctAllocation]);
                                                    tRow[Convert.ToDateTime(row[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "ALL"] = monthlyAllocation;
                                                    tRow[DatabaseObjects.Columns.GroupName] = globalRole;
                                                    tRow["UserIdGroup"] = valueResource + Constants.Separator + globalRole.Id;
                                                    dttempnew.Rows.Add(tRow);
                                                }
                                            }
                                        else
                                            goto Outer;
                                    }
                                    }
                                }


                                //if (chkAll.Checked || chkPartial.Checked)
                                if (rbtnAllResource.Checked || rbtnPartiallyAvailable.Checked)
                                {
                                    if (dtProjectMonthlyAllocation != null && dtProjectMonthlyAllocation.Rows.Count > 0)
                                    {
                                        foreach (DataRow prow in dtProjectMonthlyAllocation.Rows)
                                        {
                                            string valuePResource = Convert.ToString(prow[DatabaseObjects.Columns.Resource]);
                                            string valuePResourceSubWorkItem = Convert.ToString(prow[DatabaseObjects.Columns.ResourceSubWorkItem]);
                                            if (valuePResourceSubWorkItem != null && valuePResourceSubWorkItem == globalRole.Name.Trim())
                                            {
                                                string strExp = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.MonthStartDate, prow[DatabaseObjects.Columns.MonthStartDate], DatabaseObjects.Columns.Resource, Convert.ToString(valuePResource).Replace("'", "''"), DatabaseObjects.Columns.ResourceSubWorkItem, valuePResourceSubWorkItem);
                                                DataRow[] resultrow = dtProjectMonthlyAllocation.Select(strExp);

                                                if (resultrow != null && resultrow.Length > 0)
                                                {
                                                    string colName = Convert.ToDateTime(prow[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy");
                                                    DataRow[] presultrow = dttempnew.Select(string.Format("{0}='{1}' AND {2}='{3}'", DatabaseObjects.Columns.Id, valuePResource, DatabaseObjects.Columns.GroupName, globalRole));
                                                    if (presultrow != null && presultrow.Length > 0)
                                                        presultrow[0][string.Format("{0}", colName)] = resultrow.CopyToDataTable().AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.PctAllocation));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (hdndisplayMode.Value == "Weekly" || hdndisplayMode.Value == "Daily")
                            {
                                data.Rows.Add(newRow);
                            }
                            else
                            {
                                DataRow tRownew = dttempnew.NewRow();
                                tRownew[DatabaseObjects.Columns.Id] = userProfile.Id;
                                tRownew[DatabaseObjects.Columns.ItemOrder] = ctr++;
                                tRownew[DatabaseObjects.Columns.Resource] = userProfile.Name;
                                tRownew[DatabaseObjects.Columns.GroupName] = globalRole.Name;

                                tRownew["UserIdGroup"] = userProfile.Id + Constants.Separator + globalRole.Id;
                                dttempnew.Rows.Add(tRownew);
                            }
                            Outer:
                            continue;
                        }
                    

                }

                if (hdndisplayMode.Value == "Monthly")
                    data = SummarizeDataTable(dttempnew, "UserIdGroup");

                bool tempCounterCheck = false;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (tempCounterCheck)
                    {
                        i--;
                        tempCounterCheck = false;
                    }

                    double tempnumber = 0;
                    int colIndex = 0;
                    int colCount = 0;
                    DataRow selectedrow = data.Rows[i];
                    foreach (DataColumn colitem in data.Columns)
                    {

                        //add checkbox in resource column.

                        if (colitem.ColumnName != DatabaseObjects.Columns.ItemOrder && colitem.ColumnName != DatabaseObjects.Columns.Resource && colitem.ColumnName != DatabaseObjects.Columns.Id && colitem.ColumnName != "GroupName" && colitem.ColumnName != "UserIdGroup" && colitem.ColumnName != "FullAllocation" && colitem.ColumnName.Contains("ALL"))
                        {
                            tempnumber += UGITUtility.StringToDouble(Convert.ToString(selectedrow[colIndex]));
                            colCount++;
                        }
                        colIndex++;
                    }

                    // if (chkPartial.Checked)
                    if (rbtnPartiallyAvailable.Checked)
                    {
                        if (tempnumber / colCount >= 100)
                        {
                            data.Rows.Remove(selectedrow);
                            tempCounterCheck = true;
                        }
                    }

                    if (hdndisplayMode.Value == "Weekly" || hdndisplayMode.Value == "Daily")
                    {
                        //if (!chkAll.Checked && !chkPartial.Checked)
                        if (!rbtnAllResource.Checked && !rbtnPartiallyAvailable.Checked)
                        {
                            if (tempnumber > 0)
                            {
                                data.Rows.Remove(selectedrow);
                                tempCounterCheck = true;
                            }
                        }
                    }

                }

                //data.DefaultView.Sort = string.Format("{0}, {1} ASC", DatabaseObjects.Columns.GroupName, DatabaseObjects.Columns.Resource);
                data.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Resource);
                data = data.DefaultView.ToTable();
                return data;

            }
            else
                return null;

        }

        private void PrepareAllocationGrid()
        {
            gvResourceAvailablity.Columns.Clear();
            if (gvResourceAvailablity.Columns.Count <= 0)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                //colId.Caption = "#";
                //colId.FieldName = DatabaseObjects.Columns.ItemOrder;
                //colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                //colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                //colId.HeaderStyle.Font.Bold = true;
                //colId.Width = new Unit("50px");
                //gvResourceAvailablity.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.GroupName;
                colId.Caption = "Group Name";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.HeaderStyle.Font.Bold = true;
                colId.GroupIndex = 0;
                gvResourceAvailablity.Columns.Add(colId);


                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Resource;
                colId.Caption = DatabaseObjects.Columns.Resource;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                //colId.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                //
                //if (Request["pStartDate"] == null && Request["pEndDate"] == null)
                if (rbtnFullyAvailable.Checked)
                    colId.DataItemTemplate = new HoverMenuDataTemplate();
                colId.Width = new Unit("200px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                gvResourceAvailablity.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ProjectCapacity;
                colId.Caption = "# Capacity";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("80px");
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                gvResourceAvailablity.Columns.Add(colId);


                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.RevenueCapacity;
                colId.Caption = "$ Capacity";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("80px");
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                gvResourceAvailablity.Columns.Add(colId);

                GridViewBandColumn bdCol = new GridViewBandColumn();
                string currentDate = string.Empty;
                for (DateTime dt = Convert.ToDateTime(hdndtfrom.Value); Convert.ToDateTime(hdndtto.Value) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    if (hdndisplayMode.Value == "Daily")
                    {
                        if (FirstDayOfWeek(dt).ToString("MMM-dd-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                        {
                            gvResourceAvailablity.Columns.Add(bdCol);
                            bdCol = new GridViewBandColumn();
                        }
                        if (FirstDayOfWeek(dt).ToString("MMM-dd-yy") != currentDate)
                        {
                            if (FirstDayOfWeek(dt).Month == dt.Month)
                            {
                                bdCol.Caption = FirstDayOfWeek(dt).ToString("MMM-dd-yy");
                                currentDate = FirstDayOfWeek(dt).ToString("MMM-dd-yy");
                            }
                            else
                            {
                                bdCol.Caption = dt.ToString("MMM-dd-yy");
                                currentDate = dt.ToString("MMM-dd-yy");
                            }
                            bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bdCol.HeaderStyle.Font.Bold = true;
                            bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                        }
                    }
                    else if (hdndisplayMode.Value == "Weekly")
                    {
                        if (dt.ToString("MMM-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                        {
                            gvResourceAvailablity.Columns.Add(bdCol);
                            bdCol = new GridViewBandColumn();
                        }

                        if (dt.ToString("MMM-yy") != currentDate)
                        {
                            bdCol.Caption = dt.ToString("MMM-yy");
                            bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bdCol.HeaderStyle.Font.Bold = true;

                            bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                            currentDate = dt.ToString("MMM-yy");
                        }
                    }
                    else
                    {
                        if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                        {
                            gvResourceAvailablity.Columns.Add(bdCol);
                            bdCol = new GridViewBandColumn();
                        }

                        if (dt.ToString("yyyy") != currentDate)
                        {
                            bdCol.Caption = dt.ToString("yyyy");
                            bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            bdCol.HeaderStyle.Font.Bold = true;

                            bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol);
                            currentDate = dt.ToString("yyyy");
                        }
                    }


                    //added new col for project allocation
                    GridViewDataTextColumn colIdA = new GridViewDataTextColumn();

                    colId = new GridViewDataTextColumn();
                    if (hdndisplayMode.Value == "Monthly")
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("MMM");
                        colId.HeaderTemplate = new CommandColumnHeaderTemplate(colId);

                        colIdA.FieldName = dt.ToString("MMM-dd-yy") + "ALL";
                        colIdA.Caption = dt.ToString("MMM");

                    }
                    else if (hdndisplayMode.Value == "Weekly")
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("MMM-dd");

                        colId.HeaderTemplate = new CommandColumnHeaderTemplate(colId);

                        colIdA.FieldName = dt.ToString("MMM-dd-yy") + "ALL";
                        colIdA.Caption = dt.ToString("MMM-dd");
                    }
                    else
                    {
                        colId.FieldName = dt.ToString("MMM-dd-yy");
                        colId.Caption = dt.ToString("dd");

                        colIdA.FieldName = dt.ToString("MMM-dd-yy") + "ALL";
                        colIdA.Caption = dt.ToString("dd");
                    }

                    colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colId.PropertiesTextEdit.EncodeHtml = false;
                    colId.HeaderStyle.Font.Bold = true;
                    colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;

                    //new col for porject allocaion for project.
                    colIdA.UnboundType = DevExpress.Data.UnboundColumnType.String;
                    colIdA.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    colIdA.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colIdA.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                    colIdA.PropertiesTextEdit.EncodeHtml = false;
                    colIdA.HeaderStyle.Font.Bold = true;
                    colIdA.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    colIdA.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    colIdA.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                    colIdA.SetColVisible(false);
                    //colIdA.SetColVisibleIndex(-1);

                    if (hdndisplayMode.Value == "Monthly")
                    {
                        if (Request["pStartDate"] != null && Request["pEndDate"] != null)
                        {
                            colId.Width = new Unit("80px");
                            colIdA.Width = new Unit("80px");
                        }
                        else
                            colId.Width = new Unit("50px");
                    }
                    else if (hdndisplayMode.Value == "Daily")
                    {
                        colId.Width = new Unit("80px");
                    }
                    else
                    {
                        colId.Width = new Unit("80px");
                    }

                    bdCol.Columns.Add(colId);
                    //bdCol.Columns.Add(colIdA);
                }

                if (currentDate == bdCol.Caption)
                {
                    gvResourceAvailablity.Columns.Add(bdCol);
                }
            }
        }

        private DataTable LoadAllocationMonthlyView(string ticketId)
        {
            string commQuery = string.Empty;
            string dtfrom = Convert.ToDateTime(hdndtfrom.Value).ToString("yyyy-MM-dd");
            string dtto = Convert.ToDateTime(hdndtto.Value).ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(ticketId))
            {
                commQuery = string.Format("{3} = '{4}' AND {0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.MonthStartDate, dtfrom, dtto, DatabaseObjects.Columns.ResourceWorkItem, Request["ticketId"]);
            }
            else
            {
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.MonthStartDate, dtfrom, dtto);
            }

            //string childQuery = "";

            List<ResourceAllocationMonthly> resourceAllocationMonthlies = ObjResourceAllocationMonthlyManager.Load(commQuery);
           
            DataTable dtAllocationMonthWise = UGITUtility.ToDataTable<ResourceAllocationMonthly>(resourceAllocationMonthlies);   // ObjResourceAllocationMonthlyManager.GetDataTable(commQuery);   // SPContext.Current.Web.GetSiteData(query);
            return dtAllocationMonthWise;
            
        }


        #endregion

        #region Events

        protected void btnDrilDown_Click(object sender, EventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Weekly.ToString();

                //modification related to grid header.
                if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Mon")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime());
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Tue")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(6));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Wed")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(5));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Thu")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(4));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Fri")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(3));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sat")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(2));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sun")
                    hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(1));

                DateTime enddateweekly = hdnSelectedDate.Value.ToDateTime().AddMonths(1).AddDays(-1);
                if (enddateweekly.ToString("ddd") == "Mon")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(6));
                else if (enddateweekly.ToString("ddd") == "Tue")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(5));
                else if (enddateweekly.ToString("ddd") == "Wed")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(4));
                else if (enddateweekly.ToString("ddd") == "Thu")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(3));
                else if (enddateweekly.ToString("ddd") == "Fri")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(2));
                else if (enddateweekly.ToString("ddd") == "Sat")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(1));
                else if (enddateweekly.ToString("ddd") == "Sun")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(0));
            }
            else if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Daily.ToString();
                hdndtfrom.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime());

                if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Mon")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(7));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Tue")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(6));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Wed")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(5));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Thu")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(4));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Fri")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(3));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sat")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(2));
                else if (hdnSelectedDate.Value.ToDateTime().ToString("ddd") == "Sun")
                    hdndtto.Value = Convert.ToString(hdnSelectedDate.Value.ToDateTime().AddDays(1));
            }

            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();

            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void btnDrilUp_Click(object sender, EventArgs e)
        {
            if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Monthly.ToString();

                if (Request["pStartDate"] != null && Request["pEndDate"] != null)
                {

                    DateTime tempstartDate = Convert.ToDateTime(Request["pStartDate"]);
                    DateTime tempendDate = Convert.ToDateTime(Request["pEndDate"]);
                    DateTime tempDate = DateTime.ParseExact(hdnSelectedDate.Value, "MMM-yy", null);

                    hdndtfrom.Value = Convert.ToString(new DateTime(tempstartDate.Year, tempstartDate.Month, 1));
                    hdndtto.Value = Convert.ToString(new DateTime(tempendDate.Year, tempendDate.Month, System.DateTime.DaysInMonth(tempendDate.Year, tempendDate.Month)));

                    //if (tempstartDate.Year == tempDate.Year)
                    //{
                    //    hdndtfrom.Value = Convert.ToString(tempstartDate);
                    //    hdndtto.Value = Convert.ToString(tempendDate);
                    //}
                    //else
                    //{
                    //    hdndtfrom.Value = Convert.ToString(new DateTime(tempDate.Year, tempstartDate.Month, tempstartDate.Day));
                    //    hdndtto.Value = Convert.ToString(new DateTime(tempDate.Year, tempendDate.Month, tempendDate.Day));
                    //}

                }
                //else
                //{
                //    DateTime tempDate = DateTime.ParseExact(hdnSelectedDate.Value, "MMM-yy", null);
                //    hdndtfrom.Value = Convert.ToString(new DateTime(tempDate.Year, 1, 1));
                //    hdndtto.Value = Convert.ToString(new DateTime(tempDate.Year, 12, 31));
                //}
            }
            else if (hdndisplayMode.Value == DisplayMode.Daily.ToString())
            {
                hdndisplayMode.Value = DisplayMode.Weekly.ToString();

                DateTime tempfromDate = new DateTime(Convert.ToDateTime(hdnSelectedDate.Value).Year, Convert.ToDateTime(hdnSelectedDate.Value).Month, 1);

                if (tempfromDate.ToString("ddd") == "Mon")
                    hdndtfrom.Value = Convert.ToString(tempfromDate);
                else if (tempfromDate.ToString("ddd") == "Tue")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(6));
                else if (tempfromDate.ToString("ddd") == "Wed")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(5));
                else if (tempfromDate.ToString("ddd") == "Thu")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(4));
                else if (tempfromDate.ToString("ddd") == "Fri")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(3));
                else if (tempfromDate.ToString("ddd") == "Sat")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(2));
                else if (tempfromDate.ToString("ddd") == "Sun")
                    hdndtfrom.Value = Convert.ToString(tempfromDate.AddDays(1));

                DateTime enddateweekly = tempfromDate.AddMonths(1).AddDays(-1);

                if (enddateweekly.ToString("ddd") == "Mon")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(6));
                else if (enddateweekly.ToString("ddd") == "Tue")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(5));
                else if (enddateweekly.ToString("ddd") == "Wed")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(4));
                else if (enddateweekly.ToString("ddd") == "Thu")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(3));
                else if (enddateweekly.ToString("ddd") == "Fri")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(2));
                else if (enddateweekly.ToString("ddd") == "Sat")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(1));
                else if (enddateweekly.ToString("ddd") == "Sun")
                    hdndtto.Value = Convert.ToString(enddateweekly.AddDays(0));
            }

            gvResourceAvailablity.ExpandAll();

            PrepareAllocationGrid();

            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        #endregion

        public class CommandColumnHeaderTemplate : ITemplate
        {
            GridViewDataTextColumn colID = null;
            public CommandColumnHeaderTemplate(GridViewDataTextColumn coID)
            {
                this.colID = coID;
            }

            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                HyperLink hnkButton = new HyperLink();
                hnkButton.Text = this.colID.Caption;
                container.Controls.Add(hnkButton);
                string func = string.Format("ClickOnDrillDown(this,'{0}','{1}')", colID.FieldName, colID.Caption);
                hnkButton.Attributes.Add("onclick", func);
            }

            #endregion
        }

        public class CommandGridViewBandColumn : ITemplate
        {
            GridViewBandColumn colBDC = null;

            public CommandGridViewBandColumn(GridViewBandColumn coID)
            {
                this.colBDC = coID;
            }

            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                //HtmlGenericControl HContainer = new HtmlGenericControl("Div");
                HyperLink hnkBDButton = new HyperLink();
                //hnkBDButton.Style.Add("vertical-align", "top");
                hnkBDButton.Text = this.colBDC.Caption;
                container.Controls.Add(hnkBDButton);
                string func = string.Format("ClickOnDrillUP(this,'{0}')", colBDC.Caption);
                hnkBDButton.Attributes.Add("onclick", func);

                //HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;\" src=\"/_layouts/15/images/ugovernit/Previous16x16.png\" onclick=\"ClickOnPrevious()\"  />"));
                //HContainer.Controls.Add(hnkBDButton);
                //HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;\" src=\"/_layouts/15/images/ugovernit/Next16x16.png\" onclick=\"ClickOnNext()\"  />"));
                //container.Controls.Add(HContainer);
            }

            #endregion
        }

        public class HoverMenuDataTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;

                string[] strkey = Convert.ToString(gridContainer.KeyValue).Split('-');


                HtmlGenericControl divContainer = new HtmlGenericControl("Div");
                if (strkey.Length > 1)
                    divContainer.ID = string.Format("div_title_{0}", strkey[0]);
                else
                    divContainer.ID = string.Format("div_title_{0}", gridContainer.KeyValue);
                divContainer.Style.Add("float", "left");
                divContainer.Style.Add("width", "100%");
                divContainer.Style.Add("position", "relative");

                HtmlGenericControl innerDivContainer = new HtmlGenericControl("Div");
                if (strkey.Length > 1)
                    innerDivContainer.ID = string.Format("actionButtons{0}", strkey[0]);
                else
                    innerDivContainer.ID = string.Format("actionButtons{0}", gridContainer.KeyValue);
                //innerDivContainer.Style.Add("display", "none");
                innerDivContainer.Style.Add("width", "15px");
                innerDivContainer.Style.Add("position", "absolute");
                innerDivContainer.Style.Add("right", "0px");
                innerDivContainer.Style.Add("float", "right");
                if (DataBinder.Eval(container, string.Format("DataItem.{0}", DatabaseObjects.Columns.Resource)) != null)
                {
                    string userName = DataBinder.Eval(container, string.Format("DataItem.{0}", DatabaseObjects.Columns.Resource)).ToString();

                    innerDivContainer.Controls.Add(new LiteralControl("<input type=\"checkbox\" Id=\"checkboxcss_" + gridContainer.VisibleIndex + "\"  onclick=\"FillAllocation(this,'" + gridContainer.VisibleIndex + "')\"  />"));
                    divContainer.Controls.Add(new LiteralControl(string.Format("<span style='float:left;'>{0}</span>", userName)));
                    divContainer.Controls.Add(innerDivContainer);
                    container.Controls.Add(divContainer);
                }
            }
        }

        protected void gvResourceAvailablity_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            //string webUrl = SPContext.Current.Web.Url;
            string strModuleName = uHelper.getModuleNameByTicketId(ticketId);

            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();

            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            foreach (var args in e.UpdateValues)
            {
                var sortedDictionary = args.NewValues.Cast<System.Collections.DictionaryEntry>()
                     .OrderBy(r => Convert.ToDateTime(r.Key))
                     .ToDictionary(c => c.Key, d => d.Value);

                string[] arrayUser = Convert.ToString(args.Keys[0]).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);

                DateTime tempStartDate = DateTime.MinValue;
                DateTime tempEndDate = DateTime.MinValue;
                double tempPercentage = -1;



                //SPQuery crmQuery = new SPQuery();
                ////its delete all cprAllocation so modify its on the base user its delete the CPR Allocation 
                //// crmQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, ticketId);
                //crmQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{4}'/><Value Type='Text'>{5}</Value></Eq><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></And></Where>", 
                //    DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.UGITAssignedTo, Convert.ToInt32(arrayUser[0]), DatabaseObjects.Columns.Type, arrayUser[1]);
                //crmQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/>",
                //               DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Type, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate);

                List<ProjectEstimatedAllocation> dtCPRAllocation = cRMProjectAllocationManager.Load(x => x.TicketId == ticketId && x.AssignedTo == arrayUser[0] && x.Type == arrayUser[1]);      //  SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMProjectAllocation, crmQuery);
               

                foreach (var item in sortedDictionary)
                {
                    if (tempStartDate == DateTime.MinValue)
                    {
                        if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
                        {
                            if (!string.IsNullOrEmpty(Request["pStartDate"]) && Request["pStartDate"] != "NaN")
                                tempStartDate = Convert.ToDateTime(Request["pStartDate"]);
                            else
                                tempStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        }
                        else
                        {
                            tempStartDate = Convert.ToDateTime(item.Key);
                        }
                    }

                    if (tempEndDate == DateTime.MinValue)
                    {
                        if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
                        {
                            // tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                            if (!string.IsNullOrEmpty(Request["pEndDate"]) && Request["pEndDate"] != "NaN")
                                tempEndDate = Convert.ToDateTime(Request["pEndDate"]);
                            else
                                tempEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                        }
                        else if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(6);
                        else
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(-1);
                    }

                    if (tempPercentage == -1)
                        tempPercentage = Convert.ToDouble(item.Value);

                    if (tempPercentage != Convert.ToDouble(item.Value))
                    {
                        //if (tempPercentage != 0)

                        if (dtCPRAllocation != null && dtCPRAllocation.Count > 0)
                        {
                            List<ProjectEstimatedAllocation> row = dtCPRAllocation.Where(x=>(x.AllocationStartDate <= tempStartDate && x.AllocationEndDate >= tempStartDate) || (x.AllocationStartDate <= tempEndDate && x.AllocationEndDate >= tempEndDate) || (x.AllocationStartDate >= tempStartDate && x.AllocationEndDate <= tempEndDate)).ToList();
                            //DataRow[] row = dtCPRAllocation.Select(string.Format("({0}<='{2}' AND {1}>='{2}') OR ({0}<='{3}' AND {1}>='{3}') OR ({0}>='{2}' AND {1}<='{3}')", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, tempStartDate, tempEndDate));
                            if (row != null && row.Count > 0)
                            {
                                if (gvResourceAvailablity.JSProperties.ContainsKey("cpfindResourceValidation"))
                                {
                                    gvResourceAvailablity.JSProperties["cpfindResourceValidation"] = Constants.ErrorMsgRMMOverlappingDates;
                                }
                                else
                                {
                                    gvResourceAvailablity.JSProperties.Add("cpfindResourceValidation", Constants.ErrorMsgRMMOverlappingDates);
                                }

                                //lblMessage.Text = Constants.ErrorMsgRMMOverlappingDates;
                                //lblMessage.Visible = true;
                                // return;
                                continue;
                            }
                            else
                            {
                                //sortedOldDictionary.
                                if (tempPercentage != 0)
                                    lstUserWithPercetage.Add(new UserWithPercentage() { UserId = arrayUser[0], Percentage = tempPercentage, StartDate = tempStartDate, EndDate = tempEndDate, CustomProperty = arrayUser[1], RoleTitle = arrayUser[1] });
                            }
                        }
                        else
                        {
                            if (tempPercentage != 0)
                                lstUserWithPercetage.Add(new UserWithPercentage() { UserId = arrayUser[0], Percentage = tempPercentage, StartDate = tempStartDate, EndDate = tempEndDate, CustomProperty = arrayUser[1], RoleTitle = arrayUser[1] });
                        }

                        //// lstUserWithPercetage.Add(new UserWithPercentage() { UserId = Convert.ToInt32(arrayUser[0]), Percentage = tempPercentage, StartDate = tempStartDate, EndDate = tempEndDate, CustomProperty = arrayUser[1] });

                        tempStartDate = Convert.ToDateTime(item.Key);
                        //tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);

                        if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
                        {
                            if (!string.IsNullOrEmpty(Request["pEndDate"]) && Request["pEndDate"] != "NaN")
                            {
                                if (Convert.ToDateTime(Request["pEndDate"]).Month == Convert.ToDateTime(item.Key).Month && Convert.ToDateTime(Request["pEndDate"]).Year == Convert.ToDateTime(item.Key).Year)
                                {
                                    tempEndDate = Convert.ToDateTime(Request["pEndDate"]);
                                }
                                else
                                    tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                            }
                            else
                                tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                        }
                        else if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(6);
                        else
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(-1);

                        tempPercentage = Convert.ToDouble(item.Value);
                    }
                    else
                    {
                        //tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                        if (hdndisplayMode.Value == DisplayMode.Monthly.ToString())
                        {
                            if (!string.IsNullOrEmpty(Request["pEndDate"]) && Request["pEndDate"] != "NaN")
                            {
                                if (Convert.ToDateTime(Request["pEndDate"]).Month == Convert.ToDateTime(item.Key).Month && Convert.ToDateTime(Request["pEndDate"]).Year == Convert.ToDateTime(item.Key).Year)
                                {
                                    tempEndDate = Convert.ToDateTime(Request["pEndDate"]);
                                }
                                else
                                    tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                            }
                            else
                                tempEndDate = Convert.ToDateTime(item.Key).AddMonths(1).AddDays(-1);
                        }
                        else if (hdndisplayMode.Value == DisplayMode.Weekly.ToString())
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(6);
                        else
                            tempEndDate = Convert.ToDateTime(item.Key).AddDays(-1);
                    }
                }

                if (dtCPRAllocation != null && dtCPRAllocation.Count > 0)
                {
                    List<ProjectEstimatedAllocation> row = dtCPRAllocation.Where(x => (x.AllocationStartDate <= tempStartDate && x.AllocationEndDate >= tempStartDate) || (x.AllocationStartDate <= tempEndDate && x.AllocationEndDate >= tempEndDate) || (x.AllocationStartDate >= tempStartDate && x.AllocationEndDate <= tempEndDate)).ToList();
                    //DataRow[] row = dtCPRAllocation.Select(string.Format("({0}<='{2}' AND {1}>='{2}') OR ({0}<='{3}' AND {1}>='{3}') OR ({0}>='{2}' AND {1}<='{3}')", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, tempStartDate, tempEndDate));
                    if (row != null && row.Count > 0)
                    {
                        if (gvResourceAvailablity.JSProperties.ContainsKey("cpfindResourceValidation"))
                        {
                            gvResourceAvailablity.JSProperties["cpfindResourceValidation"] = Constants.ErrorMsgRMMOverlappingDates;
                        }
                        else
                        {
                            gvResourceAvailablity.JSProperties.Add("cpfindResourceValidation", Constants.ErrorMsgRMMOverlappingDates);
                        }
                        //lblMessage.Text = Constants.ErrorMsgRMMOverlappingDates;
                        //lblMessage.Visible = true;
                        //return;
                        continue;

                    }
                    else
                    {
                        if (tempPercentage != 0)
                            lstUserWithPercetage.Add(new UserWithPercentage() { UserId = arrayUser[0], Percentage = tempPercentage, StartDate = tempStartDate, EndDate = tempEndDate, CustomProperty = arrayUser[1], RoleTitle = arrayUser[1] });
                    }
                }
                else
                {
                    if (tempPercentage != 0)
                        lstUserWithPercetage.Add(new UserWithPercentage() { UserId = arrayUser[0], Percentage = tempPercentage, StartDate = tempStartDate, EndDate = tempEndDate, CustomProperty = arrayUser[1], RoleTitle = arrayUser[1] });
                }


            }

            if (strModuleName == "CPR" || strModuleName == "OPM" || strModuleName == "CNS")
            {
                UserRoleManager roleManager = new UserRoleManager(_context);
                // create new entry in CPR Allocation...
                foreach (UserWithPercentage itemUWP in lstUserWithPercetage)
                {
                    if (itemUWP.Percentage != 0)
                    {
                        //SPFieldUserValueCollection spUsers = new SPFieldUserValueCollection();
                        //SPFieldUserValue fuv = new SPFieldUserValue(SPContext.Current.Web, itemUWP.UserId, "");

                        //spUsers.Add(fuv);

                        //SPListItem spListItem = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMProjectAllocation).AddItem();
                        ProjectEstimatedAllocation spListItem = new ProjectEstimatedAllocation();
                        spListItem.TicketId = ticketId;

                        spListItem.PctAllocation = itemUWP.Percentage;
                        spListItem.AssignedTo = itemUWP.UserId;
                        spListItem.Type = itemUWP.CustomProperty;
                        spListItem.Title = "";
                        spListItem.AllocationStartDate = itemUWP.StartDate;
                        spListItem.AllocationEndDate = itemUWP.EndDate;

                        cRMProjectAllocationManager.Insert(spListItem);

                        Role role = roleManager.Get(x => x.Id == itemUWP.RoleTitle);
                        if (role != null)
                            itemUWP.RoleTitle = role.Name;
                    }
                }
            }

            //ObjResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(ticketId), ticketId, lstUserWithPercetage);
            
            ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(ticketId), ticketId, lstUserWithPercetage); };
            Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
            sThreadStartMethodUpdateCPRProjectAllocation.Start();

            // gvResourceAvailablity.ExpandAll();
            // PrepareAllocationGrid();

            PrepareAllocationGrid();

            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();

            gvResourceAvailablity.ExpandAll();

            e.Handled = true;

            //DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback(Request.Url.AbsoluteUri);
            // Response.Redirect(Request.RawUrl);
        }

        private static void DeleteCPRAllocation(string webUrl, string strModuleName, string userId, string type)
        {
            ////SPQuery crmQuery = new SPQuery();
            //////its delete all cprAllocation so modify its on the base user its delete the CPR Allocation 
            ////// crmQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, ticketId);
            ////crmQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{4}'/><Value Type='Text'>{5}</Value></Eq><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></And></Where>", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.UGITAssignedTo, Convert.ToInt32(userId), DatabaseObjects.Columns.Type, type);
            ////crmQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/>",
            ////               DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Type, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate);

            ////SPListItemCollection spColCRMProjectAllocation = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CRMProjectAllocation, crmQuery);


            ////for (int i = 0; i < spColCRMProjectAllocation.Count; i++)
            ////{
            ////    SPListItem splistItemCPRAllocation = spColCRMProjectAllocation[i];
            ////    List<ResourceAllocation> allocations = ResourceAllocation.LoadByWorkItem(SPContext.Current.Web, strModuleName, ticketId, null, 4, false, true);
            ////    //allocations = allocations.Where(x => x.ResourceId == Convert.ToInt16(spUsers[0].LookupId)).ToList();

            ////    if (allocations != null)
            ////    {
            ////        foreach (ResourceAllocation rAlloc in allocations)
            ////        {
            ////            if (rAlloc.StartDate == Convert.ToDateTime(splistItemCPRAllocation[DatabaseObjects.Columns.AllocationStartDate]) && rAlloc.EndDate == Convert.ToDateTime(splistItemCPRAllocation[DatabaseObjects.Columns.AllocationEndDate]))
            ////            {
            ////                rAlloc.Delete(true, SPContext.Current.Web);
            ////                if (rAlloc.WorkItemID > 0)
            ////                {
            ////                    //string webUrl = SPContext.Current.Web.Url;

            ////                    ////Start Thread to update rmm summary list for deleting entry w.r.t current allocation
            ////                    //ThreadStart threadStartMethod = delegate() { RMMSummaryHelper.DeleteRMMSummaryAndMonthDistribution(rAlloc.WorkItemID, webUrl); };
            ////                    //Thread sThread = new Thread(threadStartMethod);
            ////                    //sThread.Start();

            ////                    RMMSummaryHelper.DeleteRMMSummaryAndMonthDistribution(rAlloc.WorkItemID, webUrl);
            ////                }
            ////            }

            ////        }
            ////    }

            ////    splistItemCPRAllocation.Delete();
            ////    i--;

            ////}
        }

        protected void gvResourceAvailablity_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

        }

        protected void rbtnProject_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void rbtnTotal_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void rbtnProjectTotal_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void rbtnFullyAvailable_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void rbtnPartiallyAvailable_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void rbtnAllResource_CheckedChanged(object sender, EventArgs e)
        {
            gvResourceAvailablity.ExpandAll();
            PrepareAllocationGrid();
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }


        public static DataTable LoadRawTableByResource()
        {
            DataTable resultTable = null;

            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
           //// rQuery = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Boolean'>0</Value></Eq></Where>", DatabaseObjects.Columns.IsDeleted);
            string rQuery = string.Format("{0} = 0 ", DatabaseObjects.Columns.Deleted);


            //Guid listID = uGITCache.GetListID(DatabaseObjects.Tables.ResourceAllocation);
            //rQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", listID);
            resultTable = resourceAllocationManager.GetDataTable(rQuery);
            
            return resultTable;
        }

        public static DataTable GetAllWorkItemforTicket()
        {
            DataTable resultTable = null;

            string rQuery = string.Format("{0} = 0 And {1} = '{3}' and {2} = '{4}'", 
                DatabaseObjects.Columns.Deleted, DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem, uHelper.getModuleNameByTicketId(ticketId), ticketId);
            
            ResourceWorkItems ResourceWorkItems = new ResourceWorkItems();
            ResourceWorkItemsManager ResourceWorkItemsManager = new ResourceWorkItemsManager(HttpContext.Current.GetManagerContext());
            resultTable =  ResourceWorkItemsManager.GetDataTable(rQuery);
            return resultTable;
        }


        //get all workitems.
        public static DataTable GetAllWorkItem()
        {
            DataTable resultTable = null;

            ////rQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Boolean'>0</Value></Eq></Where>", DatabaseObjects.Columns.IsDeleted);
            string rQuery = string.Format("{0} = 0", DatabaseObjects.Columns.Deleted); 
            ////Guid listID = uGITCache.GetListID(DatabaseObjects.Tables.ResourceWorkItems);
            ////rQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", listID);
            ////resultTable = SPContext.Current.Web.GetSiteData(rQuery);
            ResourceWorkItems ResourceWorkItems = new ResourceWorkItems();
            ResourceWorkItemsManager ResourceWorkItemsManager = new ResourceWorkItemsManager(HttpContext.Current.GetManagerContext());
            resultTable = ResourceWorkItemsManager.GetDataTable(rQuery);
            return resultTable;
        }

        protected void chklstComplexitytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chklstComplexitytype.SelectedItems != null && chklstComplexitytype.Items.FindByText("Capacity").Selected == false)
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = false;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = false;
            }
            else
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = true;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = true;
            }
            gvResourceAvailablity.DataSource = GetAllocationData();
            gvResourceAvailablity.DataBind();
        }

        protected void gvResourceAvailablity_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (chklstComplexitytype.Items != null && chklstComplexitytype.Items.FindByText("Capacity").Selected == false)
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = false;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = false;
            }
            else
            {
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.ProjectCapacity].Visible = true;
                gvResourceAvailablity.Columns[DatabaseObjects.Columns.RevenueCapacity].Visible = true;
            }
        }
    }
}