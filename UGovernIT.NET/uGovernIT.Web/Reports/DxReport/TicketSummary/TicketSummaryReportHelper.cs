using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DxReport
{
    public class TicketSummaryReportHelper
    {
        #region Private Variable
        private string _fromDate;
        private string _toDate;
        private string _module;
        private string _sortType;
        private string _isModuleSort;
        private string[] _selectedModules;

        private TicketStatus _tStatus;
        private UserProfile _CurrentUser = null;

        private DataTable resultedTable = null;
        private DataTable dtDateWiseData = new DataTable();
        private DataTable dtTickets = null;
        private DataTable dtOpenTickets = null;
        private DataTable dtAlltickets = null;
        private DataTable dtClosedTickets = null;

          private DataRow[] moduleColumns = null;
        #endregion
        ApplicationContext _context = null;
        ModuleColumnManager moduleColumnManager;

        public TicketSummaryReportHelper(ApplicationContext context, string module, string sortType, string isModuleSort, TicketStatus tStatus, string fromDate, string toDate)
        {
            _context = context;
            this._module = module;
            this._sortType = sortType;
            this._isModuleSort = isModuleSort;
            this._tStatus = tStatus;
            this._fromDate = fromDate;
            this._toDate = toDate;
            //  this._spWeb = SPContext.Current.Web;
            this._CurrentUser = context.CurrentUser;
            if (module.Contains(','))
                this._selectedModules = module.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            else
                this._selectedModules = new string[] { _module };
        }

        //public TicketSummaryReportHelper(ApplicationContext context,string module, string sortType, string isModuleSort, TicketStatus tStatus, string fromDate, string toDate)
        //{
        //    this._module = module;
        //    this._sortType = sortType;
        //    this._isModuleSort = isModuleSort;
        //    this._tStatus = tStatus;
        //    this._fromDate = fromDate;
        //    this._toDate = toDate;

        //    this._CurrentUser = currentUser;
        //    if (module.Contains(','))
        //        this._selectedModules = module.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //    else
        //        this._selectedModules = new string[] { _module };
        //}

        public XtraReport GetTicketSummaryReport()
        {
            XtraReport report;

            if (_selectedModules.Length > 1 || _module.EqualsIgnoreCase("All"))
            {
                string UserType = "all";

                DataTable mytable = new DataTable();
                int openTicketsCount = 0; int UnassignedTicketsCount = 0; int closedTicketsCount = 0;

                mytable = CreateOpenTicketDataTable(_tStatus, UserType, ref openTicketsCount, ref UnassignedTicketsCount, ref closedTicketsCount);
                resultedTable = mytable;
                if (_tStatus == TicketStatus.Closed)
                {
                    dtOpenTickets = CreateOpenTicketDataTable(TicketStatus.Open, UserType, ref openTicketsCount, ref UnassignedTicketsCount, ref closedTicketsCount);
                }
                else
                {
                    dtOpenTickets = mytable;
                }
                GetClosedTicketsData(_tStatus, mytable);
                report = GetReportData(openTicketsCount, UnassignedTicketsCount, closedTicketsCount, _tStatus);
            }
            else
            {
                report = CreateTicketReport(_tStatus, _module);
            }
            return report;
        }

        private XtraReport CreateTicketReport(TicketStatus MTicketStatus, string module)
        {
            //string UserType = "all";
            // ModuleStatistics stat = null;

            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            ModuleStatistics moduleUserStatisticsManager = new ModuleStatistics(_context);
            mRequest.UserID = _CurrentUser != null ? _CurrentUser.Id : "";  //_CurrentUser.Id;
            mRequest.CurrentTab = Convert.ToString(CustomFilterTab.OpenTickets);
            mRequest.ModuleName = module;
            mRequest.Tabs = new List<string>();
            mRequest.Tabs.Add(Convert.ToString(CustomFilterTab.OpenTickets));
            mRequest.Tabs.Add(Convert.ToString(CustomFilterTab.UnAssigned));
            mRequest.Tabs.Add(Convert.ToString(CustomFilterTab.CloseTickets));


            ModuleStatisticResponse statOpen = moduleUserStatisticsManager.Load(mRequest);
            dtOpenTickets = statOpen.ResultedData;
            resultedTable = statOpen.ResultedData;
            mRequest.Tabs = new List<string>();
            mRequest.CurrentTab = Convert.ToString(CustomFilterTab.CloseTickets);
            ModuleStatisticResponse statClosed = moduleUserStatisticsManager.Load(mRequest);
            dtClosedTickets = statClosed.ResultedData;
            if (MTicketStatus == TicketStatus.All)
            {
                //stat = ModuleStatistics.Load(Module, _spWeb.CurrentUser.ID, TicketStatus.Open, false, true, UserType);
                resultedTable = statOpen.ResultedData;
                if (resultedTable != null && resultedTable.Rows.Count > 0)
                {
                    resultedTable.Merge(statClosed.ResultedData);
                }
                else
                {
                    resultedTable = statClosed.ResultedData;
                }
            }
            //else
            //{
            //    mRequest.Tabs = new List<string>();
            //    mRequest.CurrentTab = CustomFilterTab.MyOpenTickets.ToString();
            //    ModuleStatisticResponse stat = moduleUserStatisticsManager.Load(mRequest, MTicketStatus, UserType);
            //    resultedTable = stat.ResultedData;
            //}


            return GetReportData(statOpen.TabCounts[CustomFilterTab.OpenTickets.ToString()], statOpen.TabCounts[FilterTab.unassigned.ToString()], statOpen.TabCounts[CustomFilterTab.CloseTickets.ToString()], MTicketStatus);
        }

        private XtraReport GetReportData(int OpenTicketsCount, int UnassignedTicketsCount, int ClosedTicketsCount, TicketStatus MTicketStatus)
        {
            //TicketSummary_Report openTicketsReport = null;
            DataTable dtRequestsSummary = null;
            Dictionary<string, string> dicTicketStatus = new Dictionary<string, string>();
            List<ChartEntity> dicChartUrls = new List<ChartEntity>();
            List<DateTime> lstDates = uHelper.GetPreviousWorkingDates(_context, DateTime.Now, 2);
            if (lstDates != null && lstDates.Count == 2)
            {
                DateTime dtPreviousDate = lstDates[1];
                DateTime dtCurrentDate = lstDates[0];
                CreateDatatable(dtDateWiseData, dtCurrentDate.ToString("MMM-dd-yyyy"), dtPreviousDate.ToString("MMM-dd-yyyy"));
                if (dtOpenTickets != null && dtOpenTickets.Rows.Count > 0)
                {
                    DataRow[] drCurrentData = dtOpenTickets.AsEnumerable().Where(r => r.Field<object>(DatabaseObjects.Columns.Created) != null && r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtCurrentDate.Date).ToArray();
                    DataRow[] drPreviousData = dtOpenTickets.AsEnumerable().Where(r => r.Field<object>(DatabaseObjects.Columns.Created) != null && r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtPreviousDate.Date).ToArray();
                    DataTable dtPreviousTickets = null;
                    if (drPreviousData.Length > 0)
                        dtPreviousTickets = drPreviousData.CopyToDataTable();

                    DataTable dtCurrentOpenTickets = drCurrentData.CopyToDataTable();
                    AddTicketAge(ref dtPreviousTickets, dtPreviousDate);
                    AddTicketAge(ref dtCurrentOpenTickets, dtCurrentDate);
                    SetDataDateWise(dtCurrentOpenTickets, dtCurrentDate.ToString("MMM-dd-yyyy"));
                    SetDataDateWise(dtPreviousTickets, dtPreviousDate.ToString("MMM-dd-yyyy"));
                }

                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    //Code Added for total number of ticket closed
                    DataRow[] drCurrentClosedData = dtClosedTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtCurrentDate.Date).ToArray();
                    DataRow[] drPreviousClosedData = dtClosedTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtPreviousDate.Date).ToArray();
                    if (drCurrentClosedData.Length > 0)
                        dtDateWiseData.Rows[0]["ClosedTicketsCount"] = drCurrentClosedData.Length;
                    if (drPreviousClosedData.Length > 0)
                        dtDateWiseData.Rows[1]["ClosedTicketsCount"] = drPreviousClosedData.Length;
                    if (drCurrentClosedData.Length > 0 && drPreviousClosedData.Length > 0)
                        dtDateWiseData.Rows[2]["ClosedTicketsCount"] = drCurrentClosedData.Length - drPreviousClosedData.Length;

                    if (dtClosedTickets.Columns[DatabaseObjects.Columns.TicketInitiatorResolved] != null)
                    {
                        DataRow[] drQuickClosedTickets = dtClosedTickets.Select(string.Format("{0}='yes'", DatabaseObjects.Columns.TicketInitiatorResolved));
                        if (drQuickClosedTickets != null && drQuickClosedTickets.Length > 0)
                        {
                            DataTable dtQuickClosedTickets = drQuickClosedTickets.CopyToDataTable();
                            DataRow[] drCurrentQuickClosedData = dtQuickClosedTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtCurrentDate.Date).ToArray();
                            DataRow[] drPreviousQuickClosedData = dtQuickClosedTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.Created).Date <= dtPreviousDate.Date).ToArray();
                            if (drCurrentQuickClosedData.Length > 0)
                                dtDateWiseData.Rows[0]["QuickClosedTicketsCount"] = drCurrentQuickClosedData.Length;
                            if (drPreviousQuickClosedData.Length > 0)
                                dtDateWiseData.Rows[1]["QuickClosedTicketsCount"] = drPreviousQuickClosedData.Length;
                            if (drCurrentQuickClosedData.Length > 0 && drPreviousQuickClosedData.Length > 0)
                                dtDateWiseData.Rows[2]["QuickClosedTicketsCount"] = drCurrentQuickClosedData.Length - drPreviousQuickClosedData.Length;
                        }
                    }
                    else
                    {
                        dtDateWiseData.Rows[0]["QuickClosedTicketsCount"] = dtDateWiseData.Rows[1]["QuickClosedTicketsCount"] = dtDateWiseData.Rows[2]["QuickClosedTicketsCount"] = 0;
                    }
                }

                if (resultedTable != null && resultedTable.Rows.Count > 0)
                {
                    moduleColumnManager = new ModuleColumnManager(_context);
                    moduleColumns = moduleColumnManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.CategoryName, _module));
                    //SetTable();
                    dtTickets = resultedTable;
                    //SetTable();
                    AddTicketAge(ref dtTickets, dtCurrentDate);

                    if (this._module == "CPR")
                        SetCurrency(ref dtTickets);

                    FilterReportDatabyDates(ref dtTickets);
                    SortReportData();
                }

                dtRequestsSummary = GetRequestsSummary();
                dicTicketStatus = BindTicketStatus(OpenTicketsCount, UnassignedTicketsCount, ClosedTicketsCount);
                CreateTicketDialogUrl();
                string panelId = string.Empty;

                dicChartUrls = GetChartsInformation();

                if (this._module == "CPR")
                {
                    // Refer column names in ProjectSummary_Report.cs -> CreateDynamicTables(), to add/Remove Columns
                    if (dtTickets != null)
                    {
                        string[] selectedColumns = new[] { "ID", "ProjectManagerUser", "EstimatorUser", "ProjectedBudget", "SuperintendentUser", "EstimatedConstructionStart", "EstimatedConstructionEnd","ProjectId", "ERPJobID", "ERPJobIDNC", "TicketDialogUrl", "Title" };
                        dtTickets = new DataView(dtTickets).ToTable(false, selectedColumns);
                        dtTickets.AcceptChanges();
                    }
                }
                else
                {
                    if (dtTickets != null)
                    { 
                        // Refer column names in TicketSummary_Report.cs -> CreateDynamicTables(), to add/Remove Columns
                        string[] commonColumns = new[] { "ID", "Age", "RequestTypeCategory", "PriorityLookup", "Status", "RequestorUser", "DesiredCompletionDate", "TicketId", "TicketDialogUrl", "Title", "StageActionUsersUser", "DepartmentLookup" };
                        if (dtAlltickets.Columns.Contains("LocationMultLookup"))
                            commonColumns = commonColumns.Concat(new[] { "LocationMultLookup" }).ToArray();
                        else if (dtAlltickets.Columns.Contains("LocationLookup"))
                            commonColumns = commonColumns.Concat(new[] { "LocationLookup" }).ToArray();
                        dtTickets = new DataView(dtTickets).ToTable(false, commonColumns);
                        dtTickets.AcceptChanges();
                    }
                }
                if (this._module == "CPR")
                    return new ProjectSummary_Report(MTicketStatus.ToString(), dtTickets, dtDateWiseData, dicTicketStatus, dicChartUrls, dtRequestsSummary);
                else
                    return new TicketSummary_Report(MTicketStatus.ToString(), dtTickets, dtDateWiseData, dicTicketStatus, dicChartUrls, dtRequestsSummary);
            }
            return null;
        }

        private void FilterReportDatabyDates(ref DataTable dtTickets)
        {
            if (dtTickets != null && dtTickets.Rows.Count > 0)
            {
                string filterCriteria = string.Empty;
                if (!string.IsNullOrEmpty(_fromDate) && _fromDate != "NaN")
                {
                    DateTime dtFrom = UGITUtility.StringToDateTime(_fromDate);
                    DataRow[] drSelectedTickets = dtTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate).Date >= dtFrom.Date).ToArray();
                    if (drSelectedTickets.Length > 0)
                    {
                        dtTickets = drSelectedTickets.CopyToDataTable();
                    }
                    else
                    {
                        dtTickets = null;
                    }
                }
                if (!string.IsNullOrEmpty(_toDate) && _toDate != "NaN" && dtTickets != null && dtTickets.Rows.Count > 0)
                {
                    DateTime dtTo = UGITUtility.StringToDateTime(_toDate);
                    DataRow[] drSelectedTickets = dtTickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate).Date <= dtTo.Date).ToArray();
                    if (drSelectedTickets.Length > 0)
                    {
                        dtTickets = drSelectedTickets.CopyToDataTable();
                    }
                    else
                    {
                        dtTickets = null;
                    }
                }

            }
        }

        private void AddTicketAge(ref DataTable dtTickets, DateTime selectedDate)
        {
            if (dtTickets != null && dtTickets.Rows.Count > 0)
            {
                if (!dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketAge) &&
                    dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    dtTickets.Columns.Add(DatabaseObjects.Columns.TicketAge, typeof(int));
                }
                int age = 0;
                foreach (DataRow row in dtTickets.Rows)
                {
                    if (dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketAge) && Convert.ToString(row[DatabaseObjects.Columns.TicketCreationDate]) != string.Empty)
                    {
                        age = (selectedDate - (DateTime)row[DatabaseObjects.Columns.TicketCreationDate]).Days;
                        if (age < 0)
                        {
                            age = 0;
                        }
                        row[DatabaseObjects.Columns.TicketAge] = age;
                    }
                }
            }
        }

        private void SetCurrency(ref DataTable dtTickets)
        {
            if (dtTickets != null && dtTickets.Rows.Count > 0)
            {
                if (!dtTickets.Columns.Contains(DatabaseObjects.Columns.ApproxContractValueRpt))
                {
                    dtTickets.Columns.Add(DatabaseObjects.Columns.ApproxContractValueRpt, typeof(string));
                }

                foreach (DataRow row in dtTickets.Rows)
                {
                    if (row[DatabaseObjects.Columns.ApproxContractValueRpt] != DBNull.Value)
                        row[DatabaseObjects.Columns.ApproxContractValueRpt] = string.Format("{0:C0}", row[DatabaseObjects.Columns.ApproxContractValueRpt]);
                }
            }
        }

        private void SetTable()
        {
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_context);
            List<string> selectedColumns = new List<string>();
            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                DataView view = new DataView(resultedTable);
                //  dv.Sort = "Column1 ASC";
                if (view != null)
                {
                    dtTickets = ((DataView)view).ToTable();
                    foreach (DataColumn column in dtTickets.Columns)
                    {

                        DataRow row = moduleColumns.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == column.ColumnName);
                        if (row != null && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName])))
                        {
                            column.ColumnName = Convert.ToString(row[DatabaseObjects.Columns.FieldName]);
                            column.Caption = Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName]);
                        }
                        //foreach (DataRow rownew in dtTickets.Rows)
                        //{
                        //    FieldConfiguration  checkType = fieldConfigurationManager.GetFieldByFieldName(column.ColumnName);
                        //    if (checkType != null &&!(checkType.Datatype == "Lookup"))
                        //    {
                        //        string type = fieldConfigurationManager.GetFieldConfigurationData(column.ColumnName, Convert.ToString(rownew[column.ColumnName]));
                        //        if (!string.IsNullOrEmpty(type))
                        //            rownew[column.ColumnName] = type;
                        //    }

                        //}
                    }
                }
            }
        }

        private void CreateDatatable(DataTable dtSelectedData, string currentDate, string previousDate)
        {
            dtSelectedData.Columns.Add(new DataColumn("SelectedDate"));
            dtSelectedData.Columns.Add(new DataColumn("OpenTicketsCount", typeof(int)));
            dtSelectedData.Columns.Add(new DataColumn("OldestIssue"));
            dtSelectedData.Columns.Add(new DataColumn("MaxDaysOpen", typeof(int)));
            dtSelectedData.Columns.Add(new DataColumn("AvgDaysOpen", typeof(int)));
            dtSelectedData.Columns.Add(new DataColumn("ClosedTicketsCount", typeof(int)));
            dtSelectedData.Columns.Add(new DataColumn("QuickClosedTicketsCount", typeof(int)));
            DataRow drCurrent = dtSelectedData.NewRow();
            drCurrent["SelectedDate"] = currentDate;
            dtSelectedData.Rows.Add(drCurrent);

            DataRow drPrevious = dtSelectedData.NewRow();
            drPrevious["SelectedDate"] = previousDate;
            dtSelectedData.Rows.Add(drPrevious);

            DataRow drVariance = dtSelectedData.NewRow();
            drVariance["SelectedDate"] = "Variance";
            dtSelectedData.Rows.Add(drVariance);

            foreach (DataRow dr in dtSelectedData.Rows)
            {
                foreach (DataColumn dc in dtSelectedData.Columns)
                {
                    if (dc.ColumnName != "SelectedDate" && dc.ColumnName != "OldestIssue")
                    {
                        dr[dc] = 0;
                    }
                }
            }
        }

        private void SetDataDateWise(DataTable dtTicketsData, string selectedDate)
        {
            if (_tStatus == TicketStatus.All && dtTicketsData != null && dtTicketsData.Rows.Count > 0)
            {
                DataRow[] dr = dtTicketsData.Select(string.Format("{0} = 'False'  or {0} is null", DatabaseObjects.Columns.TicketClosed));
                if (dr != null && dr.Length > 0)
                {
                    dtTicketsData = dr.CopyToDataTable();
                }
            }
            if (dtTicketsData != null && dtTicketsData.Rows.Count > 0)
            {
                dtTicketsData.DefaultView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.TicketCreationDate);
                dtTicketsData = dtTicketsData.DefaultView.ToTable();
                DataRow[] selectedRows = dtDateWiseData.Select(string.Format("SelectedDate='{0}'", selectedDate));
                DataRow dr = selectedRows[0];
                // dr["SelectedDate"] = selectedDate;
                dr["OpenTicketsCount"] = dtTicketsData.Rows.Count;
                string ticketID = DatabaseObjects.Columns.TicketId;
                if (this._module == "CPR")
                {
                    ticketID = DatabaseObjects.Columns.CRMProjectID;
                }
                dr["OldestIssue"] = dtTicketsData.Rows[0][ticketID];

                if (UGITUtility.IsSPItemExist(dtTicketsData.Rows[0], DatabaseObjects.Columns.TicketAge))
                {
                    DataColumn colTicketAge = dtTicketsData.Columns[DatabaseObjects.Columns.TicketAge];
                    if (colTicketAge.DataType == typeof(int))
                    {
                        var max = dtTicketsData.AsEnumerable()
                                .Where(x => x[colTicketAge] != DBNull.Value)
                                .Max(x => x.Field<int>(colTicketAge));
                        dr["MaxDaysOpen"] = max;

                        var avg = dtTicketsData.AsEnumerable()
                                 .Where(x => x[colTicketAge] != DBNull.Value)
                                 .Average(x => x.Field<int>(colTicketAge));
                        dr["AvgDaysOpen"] = avg;
                    }
                    else if (colTicketAge.DataType == typeof(long))
                    {
                        var max = dtTicketsData.AsEnumerable()
                                .Where(x => x[colTicketAge] != DBNull.Value)
                                .Max(x => x.Field<long>(colTicketAge));
                        dr["MaxDaysOpen"] = max;

                        var avg = dtTicketsData.AsEnumerable()
                                 .Where(x => x[colTicketAge] != DBNull.Value)
                                 .Average(x => x.Field<long>(colTicketAge));
                        dr["AvgDaysOpen"] = avg;
                    }
                }
                // dtDateWiseData.Rows.Add(dr);
            }
            else
            {
                DataRow dr = dtDateWiseData.NewRow();
                dr["SelectedDate"] = selectedDate;
                dr["OpenTicketsCount"] = 0;
                dr["OldestIssue"] = string.Empty;
                dr["MaxDaysOpen"] = 0;
                dr["AvgDaysOpen"] = 0;
                dtDateWiseData.Rows.Add(dr);
            }
            if (dtDateWiseData.Rows.Count == 3)
            {
                DataRow drVariance = dtDateWiseData.Rows[2];
                drVariance["OpenTicketsCount"] = UGITUtility.StringToInt(dtDateWiseData.Rows[0]["OpenTicketsCount"]) - UGITUtility.StringToInt(dtDateWiseData.Rows[1]["OpenTicketsCount"]);
                drVariance["OldestIssue"] = string.Empty;
                drVariance["MaxDaysOpen"] = UGITUtility.StringToInt(dtDateWiseData.Rows[0]["MaxDaysOpen"]) - UGITUtility.StringToInt(dtDateWiseData.Rows[1]["MaxDaysOpen"]);
                drVariance["AvgDaysOpen"] = UGITUtility.StringToInt(dtDateWiseData.Rows[0]["AvgDaysOpen"]) - UGITUtility.StringToInt(dtDateWiseData.Rows[1]["AvgDaysOpen"]);
            }
        }

        private Dictionary<string, string> BindTicketStatus(int OpenTicketsCount, int UnassignedTicketsCount, int ClosedTicketsCount)
        {
            int numTicketsOnHold = 0;
            if (dtAlltickets != null && dtAlltickets.Rows.Count > 0)
            {
                DataRow[] drOnHoldTickets = dtAlltickets.Select(string.Format("{0}=1", DatabaseObjects.Columns.TicketOnHold));
                numTicketsOnHold = drOnHoldTickets.Length;
            }

            Dictionary<string, string> dicTicketStatus = new Dictionary<string, string>();

            if (OpenTicketsCount == UnassignedTicketsCount)
                UnassignedTicketsCount = 0;

            dicTicketStatus.Add("Unassigned", Convert.ToString(UnassignedTicketsCount));
            dicTicketStatus.Add("In Progress", Convert.ToString(OpenTicketsCount - (UnassignedTicketsCount + numTicketsOnHold)));
            dicTicketStatus.Add("On-Hold", Convert.ToString(numTicketsOnHold));
            dicTicketStatus.Add("Closed", Convert.ToString(ClosedTicketsCount));

            return dicTicketStatus;
        }

        private void SortReportData()
        {
            if (dtTickets != null && dtTickets.Rows.Count > 0)
            {
                string strSort = string.Empty;
                if (_isModuleSort.ToLower() == "true" && dtTickets.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                {
                    strSort = string.Format("{0} ASC", DatabaseObjects.Columns.ModuleName);
                }
                if (_isModuleSort.ToLower() == "true" && dtTickets.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    strSort = string.Format("{0} ASC", DatabaseObjects.Columns.ModuleNameLookup);
                }
                if (_sortType.ToLower() == "waitingon" && dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
                {
                    strSort += ((strSort == null || strSort == "") ? string.Empty : ",") + string.Format(" {0} ASC", DatabaseObjects.Columns.TicketStageActionUsers);
                }
                else if (_sortType.ToLower() == "newesttooldest" && dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    strSort += ((strSort == null || strSort == "") ? string.Empty : ",") + string.Format("{0} DESC", DatabaseObjects.Columns.TicketCreationDate);
                }
                else if (_sortType.ToLower() == "oldesttonewest" && dtTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    strSort += ((strSort == null || strSort == "") ? string.Empty : ",") + string.Format("{0} ASC", DatabaseObjects.Columns.TicketCreationDate);
                }
                dtTickets.DefaultView.Sort = strSort;
                dtTickets = dtTickets.DefaultView.ToTable();
            }
        }

        private DataTable GetRequestsSummary()
        {
            ModuleStatistics moduleUserStatisticsManager = new ModuleStatistics(_context);
            if (_selectedModules.Length == 1 && _selectedModules.Select(x => !x.EqualsIgnoreCase("all")).FirstOrDefault())
            {
                ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                //mRequest.UserID = this._CurrentUser.Id;
                mRequest.UserID = _CurrentUser != null ? _CurrentUser.Id : "";
                mRequest.CurrentTab = CustomFilterTab.OpenTickets.ToString();
                mRequest.ModuleName = _module;
                mRequest.Tabs = new List<string>();

                ModuleStatisticResponse statOpen = moduleUserStatisticsManager.Load(mRequest);

                mRequest.CurrentTab = CustomFilterTab.CloseTickets.ToString();
                ModuleStatisticResponse statClosed = moduleUserStatisticsManager.Load(mRequest);
                dtAlltickets = statOpen.ResultedData;
                if (dtAlltickets != null && dtAlltickets.Rows.Count > 0 && statClosed.ResultedData != null && statClosed.ResultedData.Rows.Count > 0)
                {
                    dtAlltickets.Merge(statClosed.ResultedData);
                }
                else if (statClosed.ResultedData != null && statClosed.ResultedData.Rows.Count > 0)
                {
                    dtAlltickets = statClosed.ResultedData.Copy();
                }

            }
            else
            {
                int openTicketsCount = 0; int UnassignedTicketsCount = 0; int closedTicketsCount = 0;
                dtAlltickets = CreateOpenTicketDataTable(TicketStatus.Open, "all", ref openTicketsCount, ref UnassignedTicketsCount, ref closedTicketsCount);
                DataTable dtClosedTickets = CreateOpenTicketDataTable(TicketStatus.Closed, "all", ref openTicketsCount, ref UnassignedTicketsCount, ref closedTicketsCount);
                if (dtAlltickets != null && dtAlltickets.Rows.Count > 0)
                {
                    dtAlltickets.Merge(dtClosedTickets);
                }
                else
                {
                    dtAlltickets = dtClosedTickets.Copy();
                }
                if (dtAlltickets != null)
                {
                    var res = dtAlltickets.AsEnumerable().Where(x => _selectedModules.Contains(x.Field<string>(DatabaseObjects.Columns.ModuleName)) || _selectedModules.Contains(x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup)));
                    if (res.Count() > 0)
                        dtAlltickets = res.CopyToDataTable();
                    else dtAlltickets = null;
                }
            }

            DataTable dtRequestsSummary = new DataTable();
            if (dtAlltickets != null && dtAlltickets.Rows.Count > 0)
            {
                dtRequestsSummary.Columns.Add(new DataColumn("ReuestTitle"));
                dtRequestsSummary.Columns.Add(new DataColumn("RequestsCount"));
                DataRow drToday = dtRequestsSummary.NewRow();
                DataRow drWeek = dtRequestsSummary.NewRow();
                DataRow drMonth = dtRequestsSummary.NewRow();
                DataRow drOlderMonth = dtRequestsSummary.NewRow();

                int requestsToday = dtAlltickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date == DateTime.Now.Date).Count();
                int requestsThisWeek = dtAlltickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date <= DateTime.Now.Date && r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date >= DateTime.Now.AddDays(-7).Date).Count();
                int requestsThisMonth = dtAlltickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date <= DateTime.Now.Date && r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date >= DateTime.Now.AddMonths(-1).Date).Count();
                int requestsOlderThanMonth = dtAlltickets.AsEnumerable().Where(r => r.Field<DateTime>(DatabaseObjects.Columns.CreationDate).Date < DateTime.Now.AddMonths(-1).Date && (r.Field<Boolean?>(DatabaseObjects.Columns.TicketClosed) != true || r.Field<Boolean?>(DatabaseObjects.Columns.TicketClosed) == null)).Count();
                drToday[0] = string.Format("New {0} Created Today", GetRecordType());
                drToday[1] = requestsToday;
                dtRequestsSummary.Rows.Add(drToday);
                drWeek[0] = string.Format("New {0} in Past Week", GetRecordType());
                drWeek[1] = requestsThisWeek;
                dtRequestsSummary.Rows.Add(drWeek);
                drMonth[0] = string.Format("New {0} in Past Month", GetRecordType());
                drMonth[1] = requestsThisMonth;
                dtRequestsSummary.Rows.Add(drMonth);
                drOlderMonth[0] = string.Format("{0} Older than a Month", GetRecordType());
                drOlderMonth[1] = requestsOlderThanMonth;
                dtRequestsSummary.Rows.Add(drOlderMonth);
            }
            return dtRequestsSummary;
        }

        private string GetRecordType()
        {
            string value = "Tickets";
            if (this._module == "CPR")
            {
                value = "Projects";
            }
            return value;
        }

        private DataTable CreateOpenTicketDataTable(TicketStatus MTicketStatus, string UserType, ref int openTicketsCount, ref int UnassignedTicketsCount, ref int closedTicketsCount)
        {
            ModuleStatistics moduleUserStatisticsManager = new ModuleStatistics(_context);
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            //mRequest.UserID =this._CurrentUser.Id;
            mRequest.UserID = _CurrentUser != null ? _CurrentUser.Id : "";
            mRequest.ModuleName = string.Empty;
            mRequest.ModuleType = ModuleType.SMS;
            mRequest.Tabs = new List<string>();
            mRequest.Tabs.Add(CustomFilterTab.OpenTickets.ToString());
            mRequest.Tabs.Add(CustomFilterTab.UnAssigned.ToString());
            mRequest.Tabs.Add(CustomFilterTab.CloseTickets.ToString());

            List<ModuleStatisticResponse> stats = moduleUserStatisticsManager.LoadAll(mRequest, MTicketStatus, UserType);
            stats = stats.Where(x => _selectedModules.Contains(x.ModuleName)).ToList();
            // ModuleStatistics moduleStatistics = new ModuleStatistics();
            DataTable mytable = new DataTable();
            if (stats != null && stats.Count > 0)
            {
                openTicketsCount = stats.Sum(x => x.TabCounts[CustomFilterTab.OpenTickets.ToString()]);
                UnassignedTicketsCount = stats.Sum(x => x.TabCounts[CustomFilterTab.UnAssigned.ToString()]);
                closedTicketsCount = stats.Sum(x => x.TabCounts[CustomFilterTab.CloseTickets.ToString()]);
                foreach (ModuleStatisticResponse mStat in stats)
                {
                    if (mStat.ResultedData != null /* && uHelper.IsUserAuthorizedToViewModule(_CurrentUser, mStat.ModuleName)*/)
                    {

                        if (!mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                        {
                            mStat.ResultedData.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
                            mStat.ResultedData.AsEnumerable().ToList().ForEach(c => c.SetField(DatabaseObjects.Columns.ModuleName, mStat.ModuleName));
                        }
                        if (mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                        {
                            if (mytable != null && mytable.Rows.Count > 0)
                            {
                                mytable.Merge(mStat.ResultedData.Copy());
                            }
                            else
                            {
                                mytable = mStat.ResultedData.Copy();
                            }

                        }

                    }
                }
            }
            return mytable;
        }

        private void CreateTicketDialogUrl()
        {
            if (dtTickets != null && dtTickets.Rows.Count > 0)
            {
                dtTickets.Columns.Add(new DataColumn("TicketDialogUrl"));

                var url = System.Web.HttpContext.Current.Request.Url;
                var port = url.Port != 80 ? (":" + url.Port) : string.Empty;
                //ModuleViewManager moduleManager = new ModuleViewManager(_context);
                //UGITModule module = null;
                foreach (DataRow dr in dtTickets.Rows)
                {
                    string moduleName = string.Empty;
                    string title = string.Empty;
                    string TicketId = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                    if (!string.IsNullOrEmpty(TicketId))
                    {
                        string tID = TicketId.Trim();
                        if (tID.Split('-').Length > 0)
                        {
                            moduleName = tID.Split('-')[0];
                        }
                    }

                    //ModuleViewManager moduleManager = new ModuleViewManager(_context);
                    //module = moduleManager.LoadByName(moduleName);
                    //string viewUrl = UGITUtility.GetAbsoluteURL(module.ModuleRelativePagePath);// uHelper.GetAbsoluteURL(ugitModule.ListPageUrl, _spWeb);
                    string viewUrl = string.Format("{0}://{1}{2}{3}?TicketId={4}&ModuleName={5}", url.Scheme, url.Host, port, uGovernIT.Utility.Constants.HomePagePath, TicketId, moduleName);
                    if (!string.IsNullOrEmpty(viewUrl))
                    {
                        if (TicketId != string.Empty)
                        {
                            string ticketTitle = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.Title])))
                                ticketTitle = UGITUtility.TruncateWithEllipsis(Convert.ToString(dr[DatabaseObjects.Columns.Title]), 100, string.Empty);

                            if (!string.IsNullOrEmpty(TicketId))
                                title = string.Format("{0}: ", TicketId);

                            title = string.Format("{0}{1}", title, ticketTitle);
                            title = uHelper.ReplaceInvalidCharsInURL(title);
                        }

                        dr["TicketDialogUrl"] = string.Format("{0}&pageTitle={1}", viewUrl, title);
                    }
                }
            }
        }

        private void GetClosedTicketsData(TicketStatus ticketStatus, DataTable dtAllTickets)
        {
            string UserType = "all";
            int openCount = 0;
            int unassignedCount = 0;
            int closedCount = 0;

            if (ticketStatus == TicketStatus.Open)
            {
                dtClosedTickets = CreateOpenTicketDataTable(TicketStatus.Closed, UserType, ref openCount, ref unassignedCount, ref closedCount);
            }
            else if (ticketStatus == TicketStatus.Closed)
            {
                dtClosedTickets = dtAllTickets;
            }
            else if (ticketStatus == TicketStatus.All)
            {
                if (dtAllTickets != null && dtAllTickets.Rows.Count > 0)
                {
                    // DataRow[] dr = dtAllTickets.Select(string.Format("{0}='1'", DatabaseObjects.Columns.TicketClosed));

                    DataRow[] dr = dtAllTickets.Select(string.Format("{0}=1", DatabaseObjects.Columns.TicketClosed));

                    if (dr.Length > 0)
                    {
                        dtClosedTickets = dr.CopyToDataTable();
                    }
                    else
                        dtClosedTickets = dtAllTickets;
                }
            }
        }

        private List<ChartEntity> GetChartsInformation()
        {
            DashboardPanelViewManager dashboardPanelViewManager = new DashboardPanelViewManager(_context);
            DashboardManager dashboardManager = new DashboardManager(_context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            System.Collections.ArrayList arrViewIds = new System.Collections.ArrayList();

            if (_tStatus == TicketStatus.Open || _tStatus == TicketStatus.All)
            {
                string viewIds = string.Empty;
                if (_selectedModules.Length > 1)
                {
                    if (this._module != "CPR")
                        viewIds = _context.ConfigManager.GetValue(ConfigConstants.OpenTicketsChart);
                    else
                        viewIds = _context.ConfigManager.GetValue(ConfigConstants.OpenProjectsChart);
                }
                else
                {
                    UGITModule moduleRow = moduleViewManager.GetByName(_module); //uGITCache.GetModuleDetails(_module);
                    if (moduleRow != null)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(moduleRow.OpenChart)))
                        {
                            if (this._module != "CPR")
                                viewIds = _context.ConfigManager.GetValue(ConfigConstants.OpenTicketsChart);
                            else
                                viewIds = _context.ConfigManager.GetValue(ConfigConstants.OpenProjectsChart);
                        }
                        else
                            viewIds = Convert.ToString(moduleRow.OpenChart);
                    }
                }

                string[] viewIdList = UGITUtility.SplitString(viewIds, Constants.Separator);
                if (viewIdList != null && viewIdList.Length > 0)
                {
                    foreach (string view in viewIdList)
                    {
                        if (!string.IsNullOrEmpty(view))
                            arrViewIds.Add(view);
                    }
                }
            }

            if (_tStatus == TicketStatus.Closed || _tStatus == TicketStatus.All)
            {
                string viewIds = string.Empty;
                if (_selectedModules.Length > 1)
                {
                    if (this._module != "CPR")
                        viewIds = _context.ConfigManager.GetValue(ConfigConstants.ClosedTicketsChart);
                    else
                        viewIds = _context.ConfigManager.GetValue(ConfigConstants.ClosedProjectsChart);

                }
                else
                {
                    UGITModule moduleRow = moduleViewManager.GetByName(_module); //uGITCache.GetModuleDetails(_module, _spWeb);
                    if (moduleRow != null)
                    {
                        if (string.IsNullOrEmpty(moduleRow.CloseChart))
                        {
                            if (this._module != "CPR")
                                viewIds = _context.ConfigManager.GetValue(ConfigConstants.ClosedTicketsChart);
                            else
                                viewIds = _context.ConfigManager.GetValue(ConfigConstants.ClosedProjectsChart);
                        }
                        else
                            viewIds = Convert.ToString(moduleRow.CloseChart);
                    }
                }

                string[] viewIdList = UGITUtility.SplitString(viewIds, Constants.Separator);
                if (viewIdList != null && viewIdList.Length > 0)
                {
                    foreach (string view in viewIdList)
                    {
                        if (!string.IsNullOrEmpty(view) && !arrViewIds.Contains(view))
                            arrViewIds.Add(view);
                    }
                }
            }

            List<ChartEntity> dic = new List<ChartEntity>();

            if (arrViewIds != null && arrViewIds.Count > 0)
            {
                foreach (string item in arrViewIds)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int viewId = UGITUtility.StringToInt(item);
                        DashboardPanelView dashboardView = dashboardPanelViewManager.LoadViewByID(viewId);
                        if (dashboardView != null)
                        {
                            List<ChartEntity> lstChartEntity = new List<ChartEntity>();
                            switch (dashboardView.ViewType.ToLower())
                            {
                                case "indivisible dashboards":
                                    IndivisibleDashboardsView indiView = (dashboardView.ViewProperty as IndivisibleDashboardsView);
                                    var Dashborad = indiView.Dashboards;
                                    Dashborad.OrderBy(m => m.ItemOrder);
                                    List<Dashboard> indivDashboards = dashboardManager.LoadDashboardsByNames(Dashborad.Select(x => x.DashboardName).ToList(), false);
                                    List<Dashboard> selectedDashboards = indivDashboards.Where(c => c.DashboardType == DashboardType.Chart).ToList();
                                    for (int i = 0; i < selectedDashboards.Count; i++)
                                    {
                                        var currentDashboard = Dashborad.FirstOrDefault(c => c.DashboardName == selectedDashboards[i].Title);
                                        if (currentDashboard != null)
                                            CreateChart(ref lstChartEntity, selectedDashboards[i].panel, currentDashboard.DisplayName, currentDashboard.Width, currentDashboard.Height, currentDashboard.ItemOrder, currentDashboard.StartFromNewLine, viewId);
                                    }
                                    break;
                                case "super dashboards":
                                    SuperDashboardsView superView = (dashboardView.ViewProperty as SuperDashboardsView);
                                    var SuperDashboards = superView.DashboardGroups.OrderBy(t => t.ItemOrder).ToList();
                                    for (int i = 0; i < SuperDashboards.Count; i++)
                                    {
                                        List<Dashboard> superDashboards = dashboardManager.LoadDashboardsByNames(SuperDashboards[i].Dashboards.Select(x => x.DashboardName).ToList(), false);
                                        for (int j = 0; j < SuperDashboards[i].Dashboards.Count; j++)
                                        {
                                            DashboardPanelProperty dashboardProperty = SuperDashboards[i].Dashboards[j];
                                            Dashboard uDashboard = superDashboards.FirstOrDefault(x => x.Title == dashboardProperty.DashboardName);
                                            if (uDashboard != null && uDashboard.DashboardType == DashboardType.Chart)
                                            {
                                                CreateChart(ref lstChartEntity, uDashboard.panel, dashboardProperty.DisplayName, dashboardProperty.Width, dashboardProperty.Height, dashboardProperty.ItemOrder, dashboardProperty.StartFromNewLine, viewId);
                                            }
                                        }
                                    }

                                    break;
                            }

                            lstChartEntity = lstChartEntity.OrderBy(c => c.Order).ToList();
                            int k = 1;
                            foreach (ChartEntity chart in lstChartEntity)
                            {
                                chart.Order = k;
                                k++;
                            }
                            lstChartEntity.ForEach(l => dic.Add(l));
                        }
                    }
                }
            }
            return CalculatePositions(ref dic);
        }

        private void CreateChart(ref List<ChartEntity> lstChartEntity, DashboardPanel dashboardPanel, string title, int width, int height, int order, bool isNewLine, int ViewId)
        {
            ChartEntity chartEntity = new ChartEntity();
            ChartSetting chartSetting = dashboardPanel as ChartSetting;
            if (chartSetting.FactTable == "DashboardSummary")
                chartSetting.FactTable = DatabaseObjects.Tables.DashboardSummary;
            ChartHelper cHelper = new ChartHelper(chartSetting, _context);
            cHelper.UseAjax = true;
            cHelper.ChartTitle = title;
            Chart cPChart = cHelper.CreateChart(true, Convert.ToString(width), Convert.ToString(height), false);
            cPChart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            cPChart.Style.Add("width", "100px");
            cPChart.Style.Add("height", "100px");
            string tempPath = uHelper.GetTempFolderPathNew();
            string fileName = Guid.NewGuid() + "chart.png";
            tempPath = tempPath + "" + fileName;
            try
            {
                cPChart.SaveImage(tempPath, ChartImageFormat.Png);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            string chartUrl = string.Format("/Content/IMAGES/ugovernit/upload/{0}", fileName);
            chartEntity.ChartUrl = UGITUtility.GetImageUrlForReport(chartUrl);
            chartEntity.Title = cHelper.ChartTitle;

            chartEntity.Width = width;
            chartEntity.Height = height;
            chartEntity.Order = order;
            chartEntity.IsNewLine = isNewLine;
            chartEntity.ViewId = 27;
            lstChartEntity.Add(chartEntity);
        }

        private List<ChartEntity> CalculatePositions(ref List<ChartEntity> lstChartEntity)
        {
            List<ChartEntity> tempLstChartEntity = new List<ChartEntity>();
            int cnt = 0;
            int calwidth = 20;
            int calHeight = 0;
            int row = 1;
            int pos = 0;
            while (cnt < lstChartEntity.Count)
            {
                ChartEntity chartEntity = lstChartEntity[cnt];
                ChartEntity tempChartEntity = chartEntity;
                bool isAdjusted = false;
                bool isIncremented = false;
                if (tempLstChartEntity.Count == 0)
                {
                    tempChartEntity.Left = 10;
                    tempChartEntity.Right = chartEntity.Width + chartEntity.Left;
                    tempChartEntity.Top = 10;
                    tempChartEntity.Bottom = chartEntity.Height + chartEntity.Top;
                    tempChartEntity.Row = new List<int> { row };
                    calwidth += tempChartEntity.Right;
                    pos = 0;
                    isAdjusted = true;
                }

                if (!isAdjusted && !tempChartEntity.IsNewLine)
                {
                    ChartEntity entity = tempLstChartEntity.FirstOrDefault(c => c.Order == (tempChartEntity.Order - 1) && c.ViewId == chartEntity.ViewId);
                    if (entity != null)
                    {
                        int i = entity.Row.Max();
                        int maxHeight = tempLstChartEntity.Where(c => c.Row.Contains(i)).Max(c => c.Height);

                        int index = tempLstChartEntity.FindIndex(c => c.Height == maxHeight && c.Row.Contains(i));
                        ChartEntity chart = null;
                        ChartEntity chartObj = null;
                        if (index + 1 < tempLstChartEntity.Count)
                        {
                            chart = tempLstChartEntity.FirstOrDefault(c => c.Height == maxHeight && c.Row.Contains(i));
                            chartObj = tempLstChartEntity.ElementAt(index + 1);
                        }
                        int maxRight = tempLstChartEntity.Where(c => c.Row.Contains(i)).Max(c => c.Right);
                        var selectedIitem = tempLstChartEntity.FirstOrDefault(c => c.Right == maxRight && c.Row.Contains(i));
                        var selectedItemIndex = tempLstChartEntity.FindIndex(c => c.Right == maxRight && c.Row.Contains(i));

                        if ((1080 - maxRight) > chartEntity.Width)
                        {

                            tempLstChartEntity.Add(new ChartEntity());
                            pos = selectedItemIndex + 1;
                            for (int j = tempLstChartEntity.Count - 1; j > pos; j--)
                            {
                                ChartEntity item = tempLstChartEntity[j - 1];
                                tempLstChartEntity[j] = item;
                            }

                            tempChartEntity.Left = tempLstChartEntity[pos - 1].Right + 10;
                            tempChartEntity.Right = tempChartEntity.Width + tempChartEntity.Left;
                            tempChartEntity.Top = tempLstChartEntity[pos - 1].Top;
                            tempChartEntity.Bottom = tempChartEntity.Height + tempChartEntity.Top;
                            tempChartEntity.Row = new List<int> { i };
                            calHeight = tempChartEntity.Bottom;
                            isAdjusted = false;
                            isIncremented = true;
                            tempLstChartEntity[pos] = tempChartEntity;
                        }
                        else if (chart != null && chartObj != null && chart.Row.Contains(i) && chartObj.Row.Contains(i))
                        {

                            int maxBottom = tempLstChartEntity.Where(c => c.Left == chartObj.Left && c.Row.Intersect(chart.Row).Any()).Max(c => c.Bottom);
                            int maxWidth = tempLstChartEntity.Where(c => c.Left == chartObj.Left && c.Row.Intersect(chart.Row).Any()).Max(c => c.Width);
                            if (chart.Bottom >= (chartEntity.Height + maxBottom + 10) && maxWidth >= chartEntity.Width)
                            {
                                tempLstChartEntity.Add(new ChartEntity());
                                for (int j = tempLstChartEntity.Count - 1; j > index + 2; j--)
                                {
                                    ChartEntity item = tempLstChartEntity[j - 1];
                                    List<int> SelectedRows = item.Row;
                                    SelectedRows = SelectedRows.Select(m => m + 1).ToList();
                                    item.Row = SelectedRows;
                                    tempLstChartEntity[j] = item;
                                }
                                pos = index + 2;
                                isIncremented = true;
                                tempChartEntity.Left = chartObj.Left;
                                tempChartEntity.Right = tempChartEntity.Width + tempChartEntity.Left;
                                tempChartEntity.Top = maxBottom + 10;
                                tempChartEntity.Bottom = tempChartEntity.Height + tempChartEntity.Top;
                                tempChartEntity.Row = new List<int> { chartObj.Row.Max() + 1 };
                                calHeight = tempChartEntity.Bottom;
                                isAdjusted = false;
                                if (!chart.Row.Contains(chartObj.Row.Max() + 1))
                                {
                                    List<int> previousRows = chart.Row;
                                    previousRows.Add(chartObj.Row.Max() + 1);
                                    chart.Row = previousRows;

                                }
                                tempLstChartEntity[pos] = tempChartEntity;
                            }
                        }


                    }
                }
                if (!isAdjusted && !isIncremented)
                {
                    row++;
                    int maxBottom = tempLstChartEntity.Where(c => c.Left == 10).Max(c => c.Bottom);
                    int height = tempLstChartEntity[tempLstChartEntity.Count - 1].Height;
                    if ((830 - height - 10) >= chartEntity.Height)
                    {
                        tempChartEntity.Left = 10;
                        tempChartEntity.Right = chartEntity.Width + chartEntity.Left;
                        tempChartEntity.Top = maxBottom + 10;
                        tempChartEntity.Bottom = chartEntity.Height + chartEntity.Top;
                        tempChartEntity.Row = new List<int> { row };
                        isAdjusted = true;
                        calwidth = 20 + tempChartEntity.Right;
                        pos++;
                    }
                    else
                    {
                        tempChartEntity.Left = 10;
                        tempChartEntity.Right = chartEntity.Width + chartEntity.Left;
                        tempChartEntity.Top = tempLstChartEntity[tempLstChartEntity.Count - 1].Bottom + (830 - height - 10);
                        tempChartEntity.Bottom = chartEntity.Height + chartEntity.Top;
                        tempChartEntity.Row = new List<int> { row };
                        isAdjusted = true;
                        calwidth = 20 + tempChartEntity.Right;
                        pos++;
                    }
                }
                if (isAdjusted)
                {
                    tempLstChartEntity.Insert(pos, tempChartEntity);
                }
                row = tempLstChartEntity[tempLstChartEntity.Count - 1].Row.Max();
                cnt++;
            }
            return tempLstChartEntity;
        }
    }
}
