using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{    
    public partial class uGovernITDashboardSLAUserControl : UserControl
    {
        private int legendWithinSLA;

        public string stringText = string.Empty;
        public string slaMetricsDrillDownUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=slametricsdrilldown");

        public List<Tuple<string, DateTime, DateTime>> lstFilter = new List<Tuple<string, DateTime, DateTime>>();

        //uGovernITDashboardSLA slaCom;
        //uGovernITDashboardSLA wepPartObj;
        public string ruleEditUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=slaruleedit");
        //public bool ShowSLAName;
        public string LegendSetting { get; set; }
        public string Module { get; set; }

        public string FilterView { get; set; }
        public bool IncludeOpen { get; set; }
        public bool ShowSLAName { get; set; }
        public string SLAEnableModules { get; set; }
        public string StringOfSelectedModule { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        FieldConfigurationManager fmanger = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RequestType_Load(requestType, null);
                ReportingPeriod_Load(reportingPeriod, null);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            fmanger = new FieldConfigurationManager(context);

            legendWithinSLA = UGITUtility.StringToInt(LegendSetting);
            if (legendWithinSLA < 1 || legendWithinSLA > 99)
                legendWithinSLA = 5;

            slaXPctMeet.Text = legendWithinSLA.ToString();

            try
            {
                //uHelper.DisplayHelpTextLink(wepPartObj, helpTextContainer);
                uHelper.DisplayHelpTextLink(context, Module, helpTextContainer);
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                if (!IsPostBack)
                {
                    chkShowOpen.Checked = IncludeOpen;
                    string defaultSelectedModule = string.Empty;
                    if (!string.IsNullOrEmpty(StringOfSelectedModule))
                    {
                        //Here [0] means "Technical Service Resolution (TSR)"
                        defaultSelectedModule = StringOfSelectedModule.Split(';').ToList()[0];
                        defaultSelectedModule = defaultSelectedModule.Split('(')[1].Replace(')', ' ').Trim();
                    }
                    else if (!string.IsNullOrEmpty(Module))//as per old code if calling done from other code
                        defaultSelectedModule = Module;
                    else if (requestType.Items.Count > 0)
                    {
                        defaultSelectedModule = requestType.Items[0].Value;
                    }
                    requestType.SelectedIndex = requestType.Items.IndexOf(requestType.Items.FindByValue(defaultSelectedModule));
                    reportingPeriod.SelectedIndex = reportingPeriod.Items.IndexOf(reportingPeriod.Items.FindByValue(FilterView));
                    if (reportingPeriod.SelectedIndex != -1 && !Convert.ToString(reportingPeriod.Value).Equals("Custom"))
                    {
                        uHelper.GetStartEndDateFromDateView(Convert.ToString(reportingPeriod.Value), ref startDate, ref endDate, ref stringText);
                    }
                    else if (reportingPeriod.SelectedIndex != -1)
                    {
                        startDate = UGITUtility.StringToDateTime(lblmetricdatecStartDate.Text);
                        endDate = UGITUtility.StringToDateTime(lblmetricdatecEndDate.Text);
                    }
                }

                BindData(startDate, endDate);
            }
            catch (Exception ex)
            {
                //Log.WriteException(ex, "ERROR Loading SLA Metrics");
                Util.Log.ULog.WriteException(ex, "ERROR Loading SLA Metrics");
            }
        }

        protected void SlaTable_PreRender(object sender, EventArgs e)
        {
        }

        private void GenerateSLATable(DataTable selectedlist)
        {
            sLATable.Rows.Clear();
            string moduleName = requestType.SelectedValue;

            //string query = string.Format("<Where><Or><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Or></Where>",
            //                            DatabaseObjects.Columns.ModuleNameLookup, moduleName);

            string query = $"{DatabaseObjects.Columns.ModuleNameLookup} = '{moduleName}'";

            DataTable slaRules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {query}");

            if (slaRules == null || slaRules.Rows.Count == 0)
                return;

            slaRules.DefaultView.Sort = DatabaseObjects.Columns.TicketPriorityLookup;
            DataTable priorities = slaRules.DefaultView.ToTable(true, DatabaseObjects.Columns.TicketPriorityLookup);

            #region SLA Table Header
            TableRow headerRow = new TableRow();
            headerRow.CssClass = "ms-viewheadertr";
            for (int i = -1; i < priorities.Rows.Count; i++)
            {
                TableCell cell = new TableCell();
                cell.Style.Add("font-weight", "bold");
                cell.Height = 25;
                if (i == -1)
                {
                    cell.Text = @"SLA Category \ Priority";
                    cell.Attributes.Add("align", "center");
                    cell.ColumnSpan = 2;
                }
                else
                {
                    //cell.Text = string.Format("{0}", priorities.Rows[i][DatabaseObjects.Columns.TicketPriorityLookup]);
                    cell.Text = fmanger.GetFieldConfigurationData(DatabaseObjects.Columns.TicketPriorityLookup, Convert.ToString(priorities.Rows[i][DatabaseObjects.Columns.TicketPriorityLookup]));    //string.Format("{0}", priorities.Rows[i][DatabaseObjects.Columns.TicketPriorityLookup]);
                    cell.Attributes.Add("align", "center");
                    cell.Attributes.Add("width", ShowSLAName ? "220px" : "150px");
                }

                headerRow.Cells.Add(cell);
            }
            sLATable.Rows.Add(headerRow);
            #endregion

            // SLA Table data rows start
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context);
            DataTable selectedDataTable = selectedlist; //selectedlist.GetDataTable();

            slaRules.DefaultView.Sort = DatabaseObjects.Columns.SLACategory;
            DataTable categories = slaRules.DefaultView.ToTable(true, DatabaseObjects.Columns.SLACategory);
            if (categories == null || categories.Rows.Count == 0)
                return;

            slaRules.DefaultView.Sort = string.Format("{0} ASC, {1} ASC, {2} ASC, {3} ASC", DatabaseObjects.Columns.SLACategory, DatabaseObjects.Columns.StartStageStep, DatabaseObjects.Columns.EndStageStep, DatabaseObjects.Columns.SLAHours);
            slaRules = slaRules.DefaultView.ToTable();

            foreach (DataRow category in categories.Rows)
            {
                string categoryChoice = Convert.ToString(category[DatabaseObjects.Columns.SLACategory]);
                TableRow contentRow = new TableRow();
                TableCell contentCell = new TableCell();
                contentCell.Text = categoryChoice;
                var groupQuery =
                       from product in slaRules.AsEnumerable()
                       where product.Field<string>(DatabaseObjects.Columns.SLACategory) == categoryChoice &&
                        product.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == requestType.SelectedValue
                       group product by product.Field<long>(DatabaseObjects.Columns.TicketPriorityLookup) into g
                       select new { priority = g.Key, priorityCount = g.Count() };

                foreach (var product in groupQuery)
                {
                    contentCell.RowSpan = contentCell.RowSpan < product.priorityCount * 2 ? product.priorityCount * 2 : contentCell.RowSpan;
                    contentCell.Style.Add("font-weight", "bold");
                    contentCell.Attributes.Add("width", "150px");
                    contentCell.Attributes.Add("align", "center");
                }

                contentRow.CssClass = "ms-viewheadertr";
                contentRow.Cells.Add(contentCell);
                int countRowSpan = contentCell.RowSpan;

                for (int i = 0; i < countRowSpan; i++)
                {
                    contentRow.CssClass = "ms-viewheadertr";
                    if (i % 2 == 0)
                    {
                        TableCell slaCell = new TableCell();
                        slaCell.Text = "SLA";
                        slaCell.Height = ShowSLAName ? 50 : 35;
                        slaCell.Attributes.Add("align", "center");
                        slaCell.Attributes.Add("width", "100px");
                        contentRow.Cells.Add(slaCell);
                    }
                    else
                    {
                        TableCell slaAchievedCell = new TableCell();
                        slaAchievedCell.Text = "Achieved";
                        slaAchievedCell.Height = 35;
                        slaAchievedCell.Attributes.Add("align", "center");
                        slaAchievedCell.Attributes.Add("width", "100px");
                        contentRow.Cells.Add(slaAchievedCell);
                    }

                    foreach (DataRow row in priorities.Rows)
                    {
                        long priority = Convert.ToInt64(row[DatabaseObjects.Columns.TicketPriorityLookup]);

                        int headerCount = (i / 2);
                        var ruleQuery = from rule in slaRules.AsEnumerable()
                                        where rule.Field<long>(DatabaseObjects.Columns.TicketPriorityLookup) == priority && rule.Field<string>(DatabaseObjects.Columns.SLACategory) == categoryChoice &&
                                              rule.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == requestType.SelectedValue
                                        select rule;
                        DataRow sltRow = null;
                        if (ruleQuery.ToList().Count >= (headerCount + 1))
                            sltRow = ruleQuery.ToList()[headerCount];

                        if (i % 2 == 0)
                        {
                            TableCell cell = new TableCell();
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#e2e0e0"); // very light gray
                            if (sltRow != null)
                            {
                                double SLAHours = Math.Round(UGITUtility.StringToDouble(sltRow[DatabaseObjects.Columns.SLAHours]), 2);
                                string txtSLAHours = string.Empty;
                                string SLAUnit = "hours";
                                if (SLAHours % workingHoursInADay == 0)
                                {
                                    txtSLAHours = string.Format("{0:0.##}", SLAHours / workingHoursInADay);
                                    SLAUnit = "days";
                                }
                                else if (SLAHours % 1 == 0)
                                {
                                    txtSLAHours = string.Format("{0:0.##}", SLAHours);
                                    SLAUnit = "hours";
                                }
                                else
                                {
                                    txtSLAHours = string.Format("{0:0.##}", SLAHours * 60);
                                    SLAUnit = "minutes";
                                }
                                string strFun = string.Format("OpenSLA(\"{0}\",\"{1}\");", Convert.ToString(sltRow[DatabaseObjects.Columns.Title]), UGITUtility.StringToInt(sltRow[DatabaseObjects.Columns.Id]));
                                cell.Text = string.Format("<div onclick='{4}' style='cursor: pointer;'><div class='clsShowSLARule' style='font-size:10px;font-weight:bold;padding-bottom:5px;'>{0}</div><div>{1}% within {2} {3}</div></div>",
                                                           Convert.ToString(sltRow[DatabaseObjects.Columns.Title]), Convert.ToString(sltRow[DatabaseObjects.Columns.SLATarget]), txtSLAHours, SLAUnit, strFun);
                            }
                            else
                            {
                                cell.Text = "----";
                            }

                            cell.Attributes.Add("align", "center");

                            contentRow.Cells.Add(cell);
                        }
                        else
                        {
                            TableCell cell = new TableCell();
                            if (selectedlist == null || selectedlist.Rows.Count == 0)
                            {
                                cell.Text = "-----";
                                cell.Attributes.Add("align", "center");
                                contentRow.Cells.Add(cell);
                                continue;
                            }

                            DataTable SelectedPTickets = selectedDataTable; //selectedDataTable.Clone();
                            ////SelectedPTickets.Columns.Add(DatabaseObjects.Columns.SlaRuleIdLookup, typeof(int));

                            ////selectedlist.Cast<SPListItem>().ToList().ForEach(x =>
                            ////{
                            ////    SPFieldLookupValue slaLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(x, DatabaseObjects.Columns.RuleNameLookup)));
                            ////    DataRow innerRow = SelectedPTickets.NewRow();
                            ////    SelectedPTickets.Columns.Cast<DataColumn>().ToList().ForEach(ic =>
                            ////    {
                            ////        if (x.Fields.ContainsField(ic.ColumnName))
                            ////        {
                            ////            SPField f = x.Fields.GetField(ic.ColumnName);
                            ////            if (f != null)
                            ////            {
                            ////                string type = f.GetType().Name;
                            ////                switch (type)
                            ////                {
                            ////                    case "SPFieldDateTime":
                            ////                        innerRow[ic.ColumnName] = UGITUtility.StringToDateTime(x[ic.ColumnName]);
                            ////                        break;
                            ////                    default:
                            ////                        innerRow[ic.ColumnName] = x[ic.ColumnName];
                            ////                        break;
                            ////                }
                            ////            }

                            ////        }
                            ////    });

                            ////    if (slaLookup != null && slaLookup.LookupId > 0)
                            ////    {
                            ////        innerRow[DatabaseObjects.Columns.RuleNameLookup] = slaLookup.LookupValue;
                            ////        innerRow[DatabaseObjects.Columns.SlaRuleIdLookup] = slaLookup.LookupId;
                            ////    }

                            ////    SelectedPTickets.Rows.Add(innerRow);
                            ////});


                            
                            if (SelectedPTickets != null && SelectedPTickets.Rows.Count > 0 && sltRow != null)
                            {

                                int countPTickets = (from l in SelectedPTickets.AsEnumerable()
                                                     where l.Field<long>(DatabaseObjects.Columns.RuleNameLookup) == UGITUtility.StringToInt(sltRow[DatabaseObjects.Columns.Id])
                                                     && l.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == Convert.ToString(sltRow[DatabaseObjects.Columns.ModuleNameLookup])
                                                     select l).Count();

                                int countSLAMTickets = 0;
                                if (chkShowOpen.Checked)
                                {

                                    countSLAMTickets = (from l in SelectedPTickets.AsEnumerable()
                                                        where l.Field<DateTime?>(DatabaseObjects.Columns.StageEndDate) != DateTime.MinValue && l.Field<long>(DatabaseObjects.Columns.RuleNameLookup) == UGITUtility.StringToInt(sltRow[DatabaseObjects.Columns.Id]) &&
                                                        l.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == Convert.ToString(sltRow[DatabaseObjects.Columns.ModuleNameLookup]) &&
                                                        l.Field<int>(DatabaseObjects.Columns.TargetTime) >= l.Field<int>(DatabaseObjects.Columns.ActualTime)
                                                        select l).Count();

                                    countSLAMTickets = countSLAMTickets + (from l in SelectedPTickets.AsEnumerable()
                                                                           where l.Field<DateTime?>(DatabaseObjects.Columns.StageEndDate) == DateTime.MinValue && l.Field<DateTime>(DatabaseObjects.Columns.UGITDueDate) <= DateTime.Today && l.Field<long>(DatabaseObjects.Columns.RuleNameLookup) == UGITUtility.StringToInt(sltRow[DatabaseObjects.Columns.Id]) &&
                                                                           l.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == Convert.ToString(sltRow[DatabaseObjects.Columns.ModuleNameLookup]) &&
                                                                           l.Field<int>(DatabaseObjects.Columns.TargetTime) >= l.Field<int>(DatabaseObjects.Columns.ActualTime)
                                                                           select l).Count();
                                }
                                else
                                {

                                    countSLAMTickets = (from l in SelectedPTickets.AsEnumerable()
                                                        where l.Field<long>(DatabaseObjects.Columns.RuleNameLookup) == UGITUtility.StringToInt(sltRow[DatabaseObjects.Columns.Id]) &&
                                                        l.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == Convert.ToString(sltRow[DatabaseObjects.Columns.ModuleNameLookup]) &&
                                                        l.Field<int>(DatabaseObjects.Columns.TargetTime) >= l.Field<int>(DatabaseObjects.Columns.ActualTime)
                                                        select l).Count();
                                }

                                if (countPTickets > 0)
                                {
                                    float preDifineMeet = float.Parse(sltRow[DatabaseObjects.Columns.SLATarget].ToString());

                                    int meet = countSLAMTickets;
                                    float total = countPTickets;
                                    float pctMet = (meet / total) * 100;
                                    string filter = string.Empty;
                                    if (Convert.ToString(reportingPeriod.Value) != "Custom")
                                        filter = string.Format("{0}~#{1}", Convert.ToString(reportingPeriod.Value), chkShowOpen.Checked);
                                    else
                                    {
                                        filter = string.Format("{0}~#{1}", Convert.ToString(reportingPeriod.Value), chkShowOpen.Checked);
                                        string customDateFilter = string.IsNullOrEmpty(lblmetricdatecStartDate.Text) ? DateTime.Today.ToString("yyyy-MM-dd") : lblmetricdatecStartDate.Text;
                                        customDateFilter = string.Format("{0}!#{1}", customDateFilter, string.IsNullOrEmpty(lblmetricdatecEndDate.Text) ? DateTime.Today.ToString("yyyy-MM-dd") : lblmetricdatecEndDate.Text);
                                        filter = string.Format("{0}~#{1}", filter, customDateFilter);
                                    }


                                    //priority = Convert.ToString(sltRow[DatabaseObjects.Columns.Title]);
                                    string title = Convert.ToString(sltRow[DatabaseObjects.Columns.Title]);
                                    string strFun = string.Format("ShowDrillDown(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\");", title, categoryChoice, moduleName, filter, sltRow[DatabaseObjects.Columns.Id]);
                                    cell.Text = string.Format("<span style='width:100%;cursor:pointer;' onclick='{3}'>{0}% ({1} of {2})</span>", Math.Round(pctMet, 1), meet, total, strFun);
                                    // UInt64 targetDiff = Convert.ToUInt64(legendWithinSLA - preDifineMeet);
                                    if (pctMet < preDifineMeet && pctMet > (preDifineMeet - legendWithinSLA))
                                    {
                                        cell.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else if (pctMet < preDifineMeet)
                                    {
                                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ce2f2f");
                                        cell.ForeColor = System.Drawing.Color.White;
                                    }
                                    else
                                    {
                                        cell.BackColor = System.Drawing.Color.LightGreen;
                                    }
                                }
                                else
                                {
                                    cell.Text = "-----";
                                }

                                cell.Attributes.Add("align", "center");
                                contentRow.Cells.Add(cell);
                            }
                            else
                            {
                                cell.Text = "-----";
                                cell.Attributes.Add("align", "center");
                                contentRow.Cells.Add(cell);
                            }
                        }
                    }

                    sLATable.Rows.Add(contentRow);
                    contentRow = new TableRow();
                }

            }
            // SLA Table data rows end

        }

        protected void RequestType_Load(object sender, EventArgs e)
        {
            if (requestType.Items.Count <= 0)
            {
                if (!string.IsNullOrEmpty(StringOfSelectedModule))// if page editor setting are done
                {
                    List<string> lstOfModule = StringOfSelectedModule.Split(';').ToList();
                    foreach (string key in lstOfModule)
                    {
                        string[] textval = key.Split('(');
                        string text = textval[0];
                        string val = textval[1].Replace(')', ' ').Trim();
                        requestType.Items.Add(new ListItem() { Text = string.Format("{0}", key), Value = val });
                    }
                }
                else //In case first time used but no page editor settings done
                {
                    //SPList spModuleList = SPListHelper.GetSPList(DatabaseObjects.Lists.Modules);
                    //SPList SLARule = SPListHelper.GetSPList(DatabaseObjects.Lists.SLARule);

                    ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                    SlaRulesManager slaRulesManager = new SlaRulesManager(context);

                    DataTable dtModule = moduleViewManager.LoadAllModules();
                    DataTable dtSLARule = slaRulesManager.GetDataTable();

                    //if (SLARule.ItemCount > 0)
                    if (dtSLARule != null && dtSLARule.Rows.Count > 0)
                    {
                        //DataTable dt = SLARule.Items.GetDataTable();
                        //DataTable dtModule = spModuleList.Items.GetDataTable();
                        dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                        dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule });
                        DataRow[] moduleRows = dtModule.Select(string.Format("{0}='1'", DatabaseObjects.Columns.EnableModule));
                        DataRow[] dr = null;
                        foreach (DataRow moduleRow in moduleRows)
                        {
                            string moduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
                            dr = dtSLARule.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
                            if (dr != null && dr.Length > 0)
                            {
                                requestType.Items.Add(new ListItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleName });
                            }

                            if (requestType.Items.IndexOf(requestType.Items.FindByValue(ModuleNames.SVC)) == -1 && moduleName == ModuleNames.SVC)
                                requestType.Items.Add(new ListItem(Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), ModuleNames.SVC));
                        }
                    }
                }
            }
        }

        protected void SlaTable_Load(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
        }

        protected void RequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (reportingPeriod.SelectedIndex != -1 && !Convert.ToString(reportingPeriod.Value).Equals("Custom"))
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(reportingPeriod.Value), ref startDate, ref endDate, ref stringText);
                //BindData(startDate, endDate);
            }
            else if (reportingPeriod.SelectedIndex != -1)
            {
                startDate = UGITUtility.StringToDateTime(lblmetricdatecStartDate.Text);
                endDate = UGITUtility.StringToDateTime(lblmetricdatecEndDate.Text);
            }

            BindData(startDate, endDate);
        }

        protected void ReportingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (reportingPeriod.SelectedIndex > 0 && "Custom" != Convert.ToString(reportingPeriod.Value))
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(reportingPeriod.Value), ref startDate, ref endDate, ref stringText);
            }
            else if (reportingPeriod.SelectedIndex > 0) // Custom
            {
                startDate = UGITUtility.StringToDateTime(lblmetricdatecStartDate.Text);
                endDate = UGITUtility.StringToDateTime(lblmetricdatecEndDate.Text);
            }

            if (requestType.SelectedValue == ModuleNames.SVC)
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate); //FilterDashboardSummaryList(requestType.SelectedValue, startDate, endDate);
                GenerateSVCLayout(selectedlist);
            }
            else
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);
                GenerateSLATable(selectedlist);
            }
        }
                

        private DataTable FilterDashboardList(string module, DateTime startDate, DateTime endDate)
        {
            DataTable selectedList = new DataTable();
            DataTable spDashboardList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketWorkflowSLASummary, $" {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");            
            string query = GenerateSLAFilterQuery(startDate, endDate, module, chkShowOpen.Checked);

            DataRow[] dr = spDashboardList.Select(Convert.ToString(query));

            if (dr.Count() > 0)
                selectedList = dr.CopyToDataTable();
            
            return selectedList;
        }

        protected void RequestType_PreRender(object sender, EventArgs e)
        {

        }

        protected void ReportingPeriod_Load(object sender, EventArgs e)
        {
            BindPeriodFilter(reportingPeriod);
        }

        protected void Filter_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            if (aFilterStartDate.SelectedDate != null)
            {
                try
                {
                    startDate = aFilterStartDate.SelectedDate.Date;
                }
                catch { }
            }
            if (aFilterEndDate.SelectedDate != null)
            {
                try
                {
                    endDate = aFilterEndDate.SelectedDate.Date;
                }
                catch { }
            }
            BindData(startDate, endDate);
        }

        private string GenerateSLAFilterQuery(DateTime startDate, DateTime endDate, string module, bool showOpenClosed = false)
        {
            StringBuilder inputQuery = new StringBuilder();
            List<string> generateQuery = new List<string>();

            generateQuery.Add($" {DatabaseObjects.Columns.StageStartDate} is not null");

            if (module != string.Empty)
                generateQuery.Add($" and {DatabaseObjects.Columns.ModuleNameLookup} = '{module}'");

            if (startDate != DateTime.MinValue && startDate != DateTime.MaxValue)
                generateQuery.Add($" and {DatabaseObjects.Columns.Created} >= '{startDate.ToString("yyyy-MM-dd")}'");

            if (endDate != DateTime.MinValue && endDate != DateTime.MaxValue)
                generateQuery.Add($" and {DatabaseObjects.Columns.Created} <= '{endDate.ToString("yyyy-MM-dd")}'");

            if (module != ModuleNames.SVC)
                generateQuery.Add($" and {DatabaseObjects.Columns.RuleNameLookup} is not null");

            if (!showOpenClosed)
                generateQuery.Add($" and {DatabaseObjects.Columns.TicketClosed} = true");

            if (generateQuery.Count > 0)
            {
                foreach (var item in generateQuery)
                {
                    inputQuery.Append(item);
                }
            }

            return inputQuery.ToString();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtStartdate.Date == DateTime.MinValue ? DateTime.Today : dtStartdate.Date;
            DateTime endDate = dtEndDate.Date == DateTime.MinValue ? DateTime.Today : dtEndDate.Date;

            if (reportingPeriod.SelectedIndex > 0 && Convert.ToString(reportingPeriod.Value).Equals("Custom"))
            {
                lblmetricdatecStartDate.Text = startDate.Date.ToString("yyyy-MM-dd");
                lblmetricdatecEndDate.Text = endDate.Date.ToString("yyyy-MM-dd");
            }

            slametricscustompopup.ShowOnPageLoad = false;
            if (requestType.SelectedValue == ModuleNames.SVC)
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);//FilterDashboardSummaryList(requestType.SelectedValue, startDate, endDate);
                GenerateSVCLayout(selectedlist);
            }
            else
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);
                GenerateSLATable(selectedlist);
            }
        }

        private void BindPeriodFilter(ASPxComboBox ddl)
        {
            if (ddl.Items.Count <= 0)
            {
                List<string> dateViews = DashboardCache.GetDateViewList();
                foreach (string item in dateViews)
                {
                    ListEditItem itm = new ListEditItem(item, item);
                    ddl.Items.Add(new ListEditItem(item, item));
                }

                ddl.Items.Insert(0, new ListEditItem("All Requests", string.Empty));
                if (ddl.Items.Count > 0)
                {
                    string itemText = "Custom";
                    ddl.Items.Insert(ddl.Items.Count, new ListEditItem(itemText, itemText));
                }
            }
        }

        protected void chkShowOpen_CheckedChanged(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            if (reportingPeriod.SelectedIndex != -1 && "Custom" != Convert.ToString(reportingPeriod.Value))
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(reportingPeriod.Value), ref startDate, ref endDate, ref stringText);
            }
            else if (reportingPeriod.SelectedIndex != -1)
            {
                startDate = UGITUtility.StringToDateTime(lblmetricdatecStartDate.Text);
                endDate = UGITUtility.StringToDateTime(lblmetricdatecEndDate.Text);
            }

            BindData(startDate, endDate);
        }

        protected void ddlCreatedOn_ValueChanged(object sender, EventArgs e)
        {
            ASPxComboBox cmbddl = sender as ASPxComboBox;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            if (cmbddl.SelectedIndex != -1 && !Convert.ToString(cmbddl.Value).Equals("Custom"))
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(reportingPeriod.Value), ref startDate, ref endDate, ref stringText);
                BindData(startDate, endDate);
            }
        }

        private void BindData(DateTime startDate, DateTime endDate)
        {
            if (requestType.SelectedValue == ModuleNames.SVC)
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);//FilterDashboardSummaryList(requestType.SelectedValue, startDate, endDate);
                GenerateSVCLayout(selectedlist);
            }
            else
            {
                DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);
                GenerateSLATable(selectedlist);
            }
        }

        protected void slametriccallbackpanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter) && e.Parameter == "slametricokcreatedon")
            {
                DateTime startDate = dtStartdate.Date == DateTime.MinValue ? DateTime.Today : dtStartdate.Date;
                DateTime endDate = dtEndDate.Date == DateTime.MinValue ? DateTime.Today : dtEndDate.Date;

                if (reportingPeriod.SelectedIndex > 0 && Convert.ToString(reportingPeriod.Value).Equals("Custom"))
                {
                    lblmetricdatecStartDate.Text = startDate.Date.ToString("yyyy-MM-dd");
                    lblmetricdatecEndDate.Text = endDate.Date.ToString("yyyy-MM-dd");
                }
                slametricscustompopup.ShowOnPageLoad = false;
                if (requestType.SelectedValue == ModuleNames.SVC)
                {
                    DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);// FilterDashboardSummaryList(requestType.SelectedValue, startDate, endDate);
                    GenerateSVCLayout(selectedlist);
                }
                else
                {
                    DataTable selectedlist = FilterDashboardList(requestType.SelectedValue, startDate, endDate);
                    GenerateSLATable(selectedlist);

                }
            }
        }
        /// <summary>
        /// SVC SLA Metrics block
        /// </summary>
        /// <param name="filterItemColl"></param>
        protected void GenerateSVCLayout(DataTable filterItemColl)//SPListItemCollection ticketWorkFlowSummary
        {
            svcslaTable.Rows.Clear();

            //Dashboard Summary
            DataTable dashboardCollForService = filterItemColl;
            if (filterItemColl == null || filterItemColl.Rows.Count == 0)
                return;

            bool openTicketsOnly = chkShowOpen.Checked;

            // SLA Table Header Start
            TableRow headerRow = new TableRow();
            headerRow.CssClass = "ms-viewheadertr";
            string[] headerText = new string[] { "Service", "Resolution SLA", "% Achieved" };
            for (int i = 0; i < headerText.Length; i++)
            {
                TableCell cell = new TableCell();
                cell.Style.Add("font-weight", "bold");
                cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7b7f84");
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F5");
                cell.Height = 25;
                cell.Attributes.Add("align", "center");
                cell.Text = headerText[i];
                headerRow.Cells.Add(cell);
            }

            svcslaTable.Rows.Add(headerRow);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context);
            float slaTarget = 100;

            var gColl = filterItemColl.AsEnumerable().GroupBy(x => UGITUtility.StringToInt(x[DatabaseObjects.Columns.ServiceTitleLookup]));
            ServicesManager servicesManager = new ServicesManager(context);
            foreach (var item in gColl)
            {
                if (item.Key == 0)
                    continue;

                DataRow[] itemArr = item.ToArray();
                Services service = servicesManager.LoadByID(UGITUtility.StringToLong(itemArr[0][DatabaseObjects.Columns.ServiceTitleLookup]));
                if (service == null || string.IsNullOrEmpty(service.Title))
                    continue;

                int serviceId = item.Key;
                string title = service.Title;

                double resolution = Math.Round(UGITUtility.StringToDouble(itemArr[0][DatabaseObjects.Columns.TargetTime]), 2);
                if (resolution == 0)
                    continue;

                TableRow contentRow = new TableRow();
                TableCell contentCell = new TableCell();
                contentCell.Height = 35;
                contentCell.Text = title;
                string str = contentCell.Text;

                //Items against service from dashboardsummary
                contentCell.Style.Add("font-weight", "bold");
                contentCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7b7f84");
                contentCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F5");
                contentCell.Attributes.Add("width", "150px");
                contentCell.Attributes.Add("align", "center");
                contentCell.CssClass = "ms-viewheadertr";
                contentRow.Cells.Add(contentCell);//Title 

                contentCell = new TableCell();
                contentCell.Height = 35;

                string txtSLAHours = string.Empty;
                string SLAUnit = "hours";
                double slahours = resolution / 60;
                if (slahours % workingHoursInADay == 0)
                {
                    txtSLAHours = string.Format("{0:0.##}", slahours / workingHoursInADay);
                    SLAUnit = "days";
                }
                else if (slahours % 1 == 0)
                {
                    txtSLAHours = string.Format("{0:0.##}", slahours);
                    SLAUnit = "hours";
                }
                else
                {
                    txtSLAHours = string.Format("{0:0.##}", slahours * 60);
                    SLAUnit = "minutes";
                }

                contentCell.Text = string.Format("100% within {0} {1}", txtSLAHours, SLAUnit);
                contentCell.Attributes.Add("width", "150px");
                contentCell.Attributes.Add("align", "center");
                contentCell.CssClass = "ms-viewheadertr";
                contentRow.Cells.Add(contentCell);//SLA

                contentCell = new TableCell();
                contentCell.Height = 35;
                contentCell.Attributes.Add("width", "150px");
                contentCell.Attributes.Add("align", "center");
                contentCell.CssClass = "ms-viewheadertr";

                if (itemArr == null || itemArr.Length == 0)
                {
                    contentCell.Text = "----";
                }
                else
                {
                    float totalPItems = itemArr.Length;
                    float metItems = 0;
                    if (!openTicketsOnly)
                    {
                        // For closed tickets, just get # of items where target time >= actual time
                        metItems = itemArr.Where(x => Convert.ToDouble(x[DatabaseObjects.Columns.TargetTime]) >= Convert.ToDouble(x[DatabaseObjects.Columns.ActualTime])).Count();
                    }
                    else
                    {
                        // For open tickets, need to update actual time upto now, and then check target time vs. actual time
                        foreach (DataRow dataItem in itemArr)
                        {
                            double targetTime = UGITUtility.StringToDouble(dataItem[DatabaseObjects.Columns.TargetTime]);
                            double actualTime = UGITUtility.StringToDouble(dataItem[DatabaseObjects.Columns.ActualTime]);

                            if (!UGITUtility.StringToBoolean(dataItem[DatabaseObjects.Columns.TicketOnHold]) &&
                                 UGITUtility.StringToDateTime(dataItem[DatabaseObjects.Columns.StageStartDate]) != DateTime.MinValue &&
                                 UGITUtility.StringToDateTime(dataItem[DatabaseObjects.Columns.StageEndDate]) == DateTime.MinValue)
                            {
                                DateTime stageStartDate = UGITUtility.StringToDateTime(dataItem[DatabaseObjects.Columns.StageStartDate]);
                                double holdDuration = UGITUtility.StringToDouble(dataItem[DatabaseObjects.Columns.TicketTotalHoldDuration]);
                                bool use24x7Calendar = UGITUtility.StringToBoolean(dataItem[DatabaseObjects.Columns.Use24x7Calendar]);
                                double span = uHelper.GetWorkingMinutesBetweenDates(context, stageStartDate, DateTime.Now, use24x7Calendar: use24x7Calendar, isSLA: true);

                                if (span > holdDuration)
                                    actualTime = span - holdDuration;
                                else
                                    actualTime = 0;
                            }

                            if (targetTime >= actualTime)
                                metItems++;
                        }
                    }

                    float pctMet = (metItems / totalPItems) * 100;
                    string filter = string.Empty;
                    if (Convert.ToString(reportingPeriod.Value) != "Custom")
                        filter = string.Format("{0}~#{1}", Convert.ToString(reportingPeriod.Value), openTicketsOnly);
                    else
                    {
                        filter = string.Format("{0}~#{1}", Convert.ToString(reportingPeriod.Value), openTicketsOnly);
                        string customDateFilter = string.IsNullOrEmpty(lblmetricdatecStartDate.Text) ? DateTime.Today.ToString("yyyy-MM-dd") : lblmetricdatecStartDate.Text;
                        customDateFilter = string.Format("{0}!#{1}", customDateFilter, string.IsNullOrEmpty(lblmetricdatecEndDate.Text) ? DateTime.Today.ToString("yyyy-MM-dd") : lblmetricdatecEndDate.Text);
                        filter = string.Format("{0}~#{1}", filter, customDateFilter);
                    }

                    string strFun = string.Format("ShowDrillDown(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\");", "Resolution", "", ModuleNames.SVC, filter, "", service.ID);
                    contentCell.Text = string.Format("<span style='width:100%;cursor:pointer;' onclick='{3}'>{0}% ({1} of {2})</span>", Math.Round(pctMet, 1), metItems, totalPItems, strFun);

                    if (pctMet < slaTarget && pctMet > (slaTarget - legendWithinSLA))
                    {
                        // SLA Target not met, but within SLA tolerance (typically 5%): Yellow Background
                        contentCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#e06e37");
                    }
                    else if (pctMet < slaTarget)
                    {
                        // SLA Target not met AND not within tolerance: Red background
                        contentCell.ForeColor = System.Drawing.Color.DarkRed;
                    }
                    else
                    {
                        // SLA Target met: Green background
                        contentCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#129812");
                    }
                }

                contentRow.Cells.Add(contentCell);  //SLA
                svcslaTable.Rows.Add(contentRow);
            }
        }
    }  
}