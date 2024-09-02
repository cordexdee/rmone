using DevExpress.XtraScheduler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using static uGovernIT.Utility.DatabaseObjects;

namespace uGovernIT.Manager
{
    public class QueryHelperManager
    {
        ApplicationContext context = null;
        DashboardManager dManager;
        FieldConfigurationManager fmanger;

        public QueryHelperManager(ApplicationContext _context)
        {
            context = _context;
            dManager = new DashboardManager(context);
            fmanger = new FieldConfigurationManager(context);
        }

        public CustomQuery QueryInfo { get; set; }

        public DataTable GetReportData(string queryTitle)
        {
            string query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.Title, queryTitle.Trim(), DatabaseObjects.Columns.TenantID, context.TenantID);

            DataRow[] collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, query).Select();
            int dashboardID = 0;
            DataTable mainTable = new DataTable();
            if (collection != null && collection.Count() > 0)
            {
                DataRow item = collection[0];
                dashboardID = Convert.ToInt32(item[DatabaseObjects.Columns.Id]);
                mainTable = GetReportData(dashboardID);
            }

            return mainTable;
        }

        private int GetQueryID(string queryTitle)
        {
            string query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.Title, queryTitle.Trim(), DatabaseObjects.Columns.TenantID, context.TenantID);
            DataRow[] collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, query).Select();
            int dashboardID = -1;
            DataTable mainTable = new DataTable();
            if (collection != null && collection.Count() > 0)
            {
                DataRow item = collection[0];
                dashboardID = Convert.ToInt32(item[DatabaseObjects.Columns.Id]);
            }
            return dashboardID;
        }

        public DataTable GetReportFromReportData(string queryReportTitle)
        {
            int dashboardID = -1;
            if (queryReportTitle.EndsWith("-Report"))
            {
                string title = queryReportTitle.Split('-')[0];
                dashboardID = GetQueryID(title);
            }
            if (dashboardID > 0)
                return (GetReportData(dashboardID));
            else
                return new DataTable();
        }

        public DataTable GetReportData(int queryID)
        {
            Dashboard uDashboard = dManager.LoadPanelById(queryID,true);
            if (uDashboard == null || !(uDashboard.panel is DashboardQuery))
                return null;

            DashboardQuery dashboard = uDashboard.panel as DashboardQuery;
            DataTable mainTable = new DataTable();
            DataTable totalsFakeTable = new DataTable();
            mainTable = GetReportData(dashboard.QueryInfo, string.Empty, ref totalsFakeTable, false);
            return mainTable;
        }

        public DataTable GetReportData(int queryID, bool useCache)
        {
            Dashboard uDashboard = dManager.LoadPanelById(queryID, useCache);
            if (uDashboard == null || !(uDashboard.panel is DashboardQuery))
            {
                return null;
            }
            DashboardQuery dashboard = uDashboard.panel as DashboardQuery;
            DataTable mainTable = new DataTable();
            DataTable totalsFakeTable = new DataTable();
            mainTable = GetReportData(dashboard.QueryInfo, string.Empty, ref totalsFakeTable, false);
            return mainTable;
        }

        public DataTable GetReportData(int queryID, string whereFilter, ref DataTable totalsTable)
        {
            Dashboard uDashboard = dManager.LoadPanelById(queryID);
            if (uDashboard == null || !(uDashboard.panel is DashboardQuery))
                return null;

            DataTable mainTable = new DataTable();
            DashboardQuery dashboard = uDashboard.panel as DashboardQuery;
            mainTable = GetReportData(dashboard.QueryInfo, whereFilter, ref totalsTable, false);
            return mainTable;
        }

        public DataTable GetReportData(long queryID, string whereFilter, ref DataTable totalsTable, bool isdrilldown)
        {
            Dashboard uDashboard = dManager.LoadPanelById(queryID);
            if (uDashboard == null || !(uDashboard.panel is DashboardQuery))
                return null;

            DataTable mainTable = new DataTable();
            DashboardQuery dashboard = uDashboard.panel as DashboardQuery;
            mainTable = GetReportData(dashboard.QueryInfo, whereFilter, ref totalsTable, isdrilldown);
            return mainTable;
        }

        public DataTable GetReportData(CustomQuery customQueryInfo, string whereFilter, ref DataTable totalsTable)
        {
            return GetReportData(customQueryInfo, whereFilter, ref totalsTable, false);
        }

        /// <summary>
        /// This method is used to generate data for dashboard query
        /// </summary>
        /// <param name="customQueryInfo"></param>
        /// <param name="whereFilter"></param>
        /// <param name="totalsTable"></param>
        /// <param name="isdrilldown"></param>
        /// <param name="sourceUrl"></param>
        /// <returns></returns>
        public DataTable GetReportData(CustomQuery customQueryInfo, string whereFilter, ref DataTable totalsTable, bool isdrilldown, string sourceUrl = null)
        {
            DataTable dt = new DataTable();
            try
            {
                DataTable tempTable = new DataTable();
                DataTable temp = null;
                DataSet tempds = new DataSet();
                var tables = customQueryInfo.Tables;
                bool isUnion = false;
                ModuleViewManager objModuleViewManager = new ModuleViewManager(context);
                if (customQueryInfo.JoinList != null && customQueryInfo.JoinList.Count > 0)
                    isUnion = customQueryInfo.JoinList.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

                // Get All Tables;
                #region QUERY WITH LINQ EXECUTION
                foreach (TableInfo table in customQueryInfo.Tables)
                {
                    // Note: Try to get data from cache if available, else from DB
                    tempTable = DashboardCache.GetCachedDashboardData(context, table.Name);
                    string modulename = objModuleViewManager.GetModuleByTableName(table.Name);
                    if (string.IsNullOrEmpty(modulename))
                    {
                        modulename = table.Name.Split('-')[0];
                        modulename = objModuleViewManager.GetModuleTableName(modulename);
                    }
                    
                    if (tempTable == null || tempTable.Rows.Count == 0)
                    {
                        DataTable spList = GetTableDataManager.GetTableData(table.Name, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                        if (spList != null && spList.Rows.Count > 0)
                        {
                            spList = DashboardCache.UpdateLookupAndUserFieldValues(context, spList);
                            tempTable = GetDataFromList(spList, customQueryInfo.JoinList);
                        }
                    }
                    if (!string.IsNullOrEmpty(modulename) || table.Name==DatabaseObjects.Tables.AspNetUsers || table.Name == DatabaseObjects.Tables.DashboardSummary || table.Name== DatabaseObjects.Tables.ResourceUsageSummaryMonthWise || table.Name == DatabaseObjects.Tables.ResourceUsageSummaryWeekWise)
                    {// this code will be only for moduletable
                        
                        foreach (DataColumn column in tempTable.Columns)
                        {
                            try
                            {
                                if (table.Name == DatabaseObjects.Tables.AspNetUsers)
                                {
                                    if (Convert.ToString(column.ColumnName).EndsWith("GlobalRoleID"))
                                        column.ColumnName = column.ColumnName + "$";
                                    if (Convert.ToString(column.ColumnName).EndsWith("GlobalRoleId$"))
                                        column.ColumnName = Convert.ToString(column.ColumnName).Remove(Convert.ToString(column.ColumnName).Length - 1);
                                    if (Convert.ToString(column.ColumnName).EndsWith("EmployeeType"))
                                        column.ColumnName = column.ColumnName + "$Id";
                                    if (Convert.ToString(column.ColumnName).EndsWith("EmployeeType$"))
                                        column.ColumnName = Convert.ToString(column.ColumnName).Remove(Convert.ToString(column.ColumnName).Length - 1);
                                }
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);

                            }
                        }

                        foreach (DataColumn column in tempTable.Columns)
                        {
                            if (dt.Columns.Contains(column.ColumnName))
                                if (column.ColumnName.EndsWith("User$Id"))
                                    dt.Columns.Remove(column.ColumnName);
                        }
                        tempTable.AcceptChanges();
                    }
                    if (tempTable != null)
                    {
                        tempTable.TableName = table.Name.Replace(" ", "_");
                        tempds.Tables.Add(tempTable.Copy());
                    }
                    else
                        return null;
                }

                if (tempds == null || tempds.Tables.Count == 0)
                    return null;

                ///handling Joins 
                ///if multiple table with join
                if (customQueryInfo.JoinList != null && customQueryInfo.JoinList.Count > 0)
                {
                    foreach (Joins join in customQueryInfo.JoinList)
                    {
                        DataTable targetTable = null;
                        string firstTable, secondTable, firstColumn, secondColumn;
                        ///if space in table Name to Replace with "_"
                        firstTable = join.FirstTable.Replace(" ", "_");
                        secondTable = join.SecondTable.Replace(" ", "_");

                        firstColumn = (temp == null) ? UGITUtility.SplitString(join.FirstColumn, ".")[1] : join.FirstColumn;
                        //if (firstColumn.EndsWith("Lookup"))
                        //    firstColumn = firstColumn + "$";
                        secondColumn = UGITUtility.SplitString(join.SecondColumn, ".")[1];
                        //if (secondColumn.EndsWith("Lookup"))
                        //    secondColumn = secondColumn + "$";
                        DataTable table1 = (temp == null) ? tempds.Tables[firstTable].Copy() : temp;
                        DataTable table2 = tempds.Tables[secondTable].Copy();

                        if (isUnion)
                        {
                            if (temp == null)
                            {
                                var dt1Columns = table1.Columns.OfType<DataColumn>().Select(dc =>
                                   new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));
                                targetTable = new DataTable();
                                targetTable.Columns.AddRange(dt1Columns.ToArray());
                            }
                            else
                            {
                                targetTable = table1.Clone();
                            }

                            if (targetTable != null && targetTable.Columns.Count > 0)
                            {
                                DataColumn[] columnArray = table2.Columns.OfType<DataColumn>().AsEnumerable().Where(x => !targetTable.Columns.Contains(x.ColumnName)).Select(dc =>
                                 new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping)).ToArray();

                                if (columnArray != null && columnArray.Length > 0)
                                    targetTable.Columns.AddRange(columnArray);
                            }
                        }
                        else
                        {
                            if (temp == null)
                            {
                                var dt1Columns = table1.Columns.OfType<DataColumn>().Select(dc =>
                                   new DataColumn(firstTable + "." + dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));
                                targetTable = new DataTable();
                                targetTable.Columns.AddRange(dt1Columns.ToArray());
                            }
                            else
                            {
                                targetTable = table1.Clone();
                            }

                            var dt2Columns = table2.Columns.OfType<DataColumn>().Select(dc =>
                                new DataColumn(secondTable + "." + dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));

                            targetTable.Columns.AddRange(dt2Columns.ToArray());
                        }

                        if (join.JoinType == JoinType.INNER)
                        {
                            if (table1.Columns[firstColumn].DataType != table2.Columns[secondColumn].DataType)
                            {
                                //if innner join column data type if different then type cast with string and then do inner join.
                                var rowsData = from a in table1.AsEnumerable()
                                               join b in table2.AsEnumerable()
                                               on Convert.ToString(a[firstColumn]) equals Convert.ToString(b[secondColumn])
                                               select a.ItemArray.Concat(b.ItemArray).ToArray();

                                if (rowsData == null || rowsData.Count() == 0)
                                    return null;

                                foreach (object[] values in rowsData)
                                    targetTable.Rows.Add(values);

                                temp = targetTable.Copy();
                            }
                            else
                            {
                                var rowsData = from a in table1.AsEnumerable()
                                               join b in table2.AsEnumerable()
                                               on a[firstColumn] equals b[secondColumn]
                                               select a.ItemArray.Concat(b.ItemArray).ToArray();

                                if (rowsData == null || rowsData.Count() == 0)
                                    return null;

                                foreach (object[] values in rowsData)
                                    targetTable.Rows.Add(values);

                                temp = targetTable.Copy();
                            }

                        }
                        else if (join.JoinType == JoinType.OUTER)
                        {
                            if (table1.Columns[firstColumn].DataType != table2.Columns[secondColumn].DataType)
                            {
                                var rowsData = from a in table1.AsEnumerable()
                                               join b in table2.AsEnumerable()
                                               on Convert.ToString(a[firstColumn]) equals Convert.ToString(b[secondColumn])
                                               into merged
                                               from sub in merged.DefaultIfEmpty()
                                               select new { RowA = a, RowB = sub };

                                if (rowsData == null || rowsData.Count() == 0)
                                    return null;

                                foreach (var v in rowsData)
                                {

                                    object[] values = null;
                                    if (v.RowB != null)
                                        values = v.RowA.ItemArray.Concat(v.RowB.ItemArray).ToArray();
                                    else
                                        values = v.RowA.ItemArray.ToArray();

                                    targetTable.Rows.Add(values);
                                }
                                temp = targetTable.Copy();
                            }
                            else
                            {
                                var rowsData = from a in table1.AsEnumerable()
                                               join b in table2.AsEnumerable()
                                               on a[firstColumn] equals b[secondColumn]
                                               into merged
                                               from sub in merged.DefaultIfEmpty()
                                               select new { RowA = a, RowB = sub };

                                if (rowsData == null || rowsData.Count() == 0)
                                    return null;

                                foreach (var v in rowsData)
                                {

                                    object[] values = null;
                                    if (v.RowB != null)
                                        values = v.RowA.ItemArray.Concat(v.RowB.ItemArray).ToArray();
                                    else
                                        values = v.RowA.ItemArray.ToArray();

                                    targetTable.Rows.Add(values);
                                }
                                temp = targetTable.Copy();
                            }
                        }
                        else if (join.JoinType == JoinType.UNION)
                        {
                            if (targetTable != null && targetTable.Columns.Count > 0)
                            {
                                if (table1 != null && table1.Rows.Count > 0)
                                    targetTable.Merge(table1);

                                if (table2 != null && table2.Rows.Count > 0)
                                    targetTable.Merge(table2);
                            }
                            temp = targetTable.Copy();
                        }
                    }
                }
                /// If single table
                else
                {
                    if (tables != null && tables.Count > 0)
                    {
                        ///Expression Columns.
                        List<Utility.ColumnInfo> expColumns = new List<Utility.ColumnInfo>();

                        foreach (TableInfo table in customQueryInfo.Tables)
                        {
                            expColumns.AddRange(table.Columns.Where(x => x.IsExpression).OrderBy(c => c.Sequence).ToArray());
                        }

                        string firstTable = tables[0].Name.Replace(" ", "_");
                        DataTable table1 = tempds.Tables[firstTable].Copy();

                        foreach (Utility.ColumnInfo cInfo in expColumns)
                        {
                            string colVal = cInfo.FieldName.Replace(string.Format("{0}.", firstTable), "");
                            if (!table1.Columns.Contains(colVal))
                                table1.Columns.Add(colVal);
                        }

                        List<DataColumn> dt1Columns = new List<DataColumn>();
                        string expression = string.Empty;

                        foreach (DataColumn column in table1.Columns)
                        {
                            expression = string.Empty;
                            if (!string.IsNullOrWhiteSpace(column.Expression))
                                expression = column.Expression.Replace("[", string.Format("[{0}", firstTable));

                            dt1Columns.Add(new DataColumn(firstTable + "." + column.ColumnName, column.DataType, expression, column.ColumnMapping));
                        }

                        var targetTable = new DataTable();
                        targetTable.Columns.AddRange(GetColumnsWithTableName(table1, firstTable));
                        foreach (DataRow row in table1.Rows)
                        {
                            foreach (Utility.ColumnInfo cInfo in expColumns)
                            {
                                string colVal = cInfo.FieldName.Replace(string.Format("{0}.", firstTable), "");
                                ExpressionInfo expInfo = GetExpressionInfo(cInfo.Expression);

                                if (GetDataType(cInfo.DataType) == typeof(int))
                                {
                                    try
                                    {
                                        if (expInfo.DataType1.Trim().Equals("datetime", StringComparison.OrdinalIgnoreCase) && expInfo.DataType2.Trim().Equals("datetime", StringComparison.OrdinalIgnoreCase) && expInfo.Operator.Trim() == "-")
                                        {
                                            TimeSpan timeSpan = (row[expInfo.Column1] != null && row[expInfo.Column2] != null) ? Convert.ToDateTime(row[expInfo.Column1]).Subtract(Convert.ToDateTime(row[expInfo.Column2])) : new TimeSpan(0);

                                            if (cInfo.DataType.ToLower() == "hours")
                                                row[colVal] = timeSpan.Days * 24 + timeSpan.Hours;
                                            else if (cInfo.DataType.ToLower() == "minutes")
                                                row[colVal] = row[colVal] = timeSpan.Days * 24 + timeSpan.Hours * 60 + timeSpan.Minutes;
                                            else // (cInfo.DataType.ToLower() == "days")
                                                row[colVal] = timeSpan.Days;
                                        }
                                        else
                                        {
                                            if (expInfo.Operator.Trim() == "-")
                                                row[colVal] = Convert.ToInt32(row[expInfo.Column1]) - Convert.ToInt32(row[expInfo.Column2]);
                                            else if (expInfo.Operator.Trim() == "+")
                                                row[colVal] = Convert.ToInt32(row[expInfo.Column1]) + Convert.ToInt32(row[expInfo.Column2]);
                                            else if (expInfo.Operator.Trim() == "/")
                                                row[colVal] = Convert.ToInt32(row[expInfo.Column1]) / Convert.ToInt32(row[expInfo.Column2]);
                                            else if (expInfo.Operator.Trim() == "*")
                                                row[colVal] = Convert.ToInt32(row[expInfo.Column1]) * Convert.ToInt32(row[expInfo.Column2]);
                                        }
                                    }
                                    catch { row[colVal] = 0; }
                                }
                                else if (GetDataType(cInfo.DataType) == 0.000.GetType())
                                {
                                    try
                                    {
                                        if (expInfo.Operator.Trim() == "-")
                                            row[colVal] = Convert.ToDouble(row[expInfo.Column1]) - Convert.ToDouble(row[expInfo.Column2]);
                                        else if (expInfo.Operator.Trim() == "+")
                                            row[colVal] = Convert.ToDouble(row[expInfo.Column1]) + Convert.ToDouble(row[expInfo.Column2]);
                                        else if (expInfo.Operator.Trim() == "/")
                                            row[colVal] = Convert.ToDouble(row[expInfo.Column1]) / Convert.ToDouble(row[expInfo.Column2]);
                                        else if (expInfo.Operator.Trim() == "*")
                                            row[colVal] = Convert.ToDouble(row[expInfo.Column1]) * Convert.ToDouble(row[expInfo.Column2]);
                                    }
                                    catch { row[colVal] = 0; }
                                }
                                else if (GetDataType(cInfo.DataType) == typeof(string))
                                {
                                    try
                                    {
                                        if (cInfo.DataType == "Boolean")
                                        {
                                            if (expInfo.Operator.Trim() == "And")
                                                row[colVal] = Convert.ToBoolean(UGITUtility.StringToBoolean(row[expInfo.Column1])) && Convert.ToBoolean(UGITUtility.StringToBoolean(row[expInfo.Column2]));
                                            else if (expInfo.Operator.Trim() == "Or")
                                                row[colVal] = Convert.ToBoolean(UGITUtility.StringToBoolean(row[expInfo.Column1])) || Convert.ToBoolean(UGITUtility.StringToBoolean(row[expInfo.Column2]));
                                            else
                                                row[colVal] = false;

                                            row[colVal] = Convert.ToBoolean(row[colVal]) ? "1" : "0";
                                        }
                                        else
                                        {
                                            if (expInfo.Operator.Trim() == "+")
                                                row[colVal] = Convert.ToString(row[expInfo.Column1]) + " " + Convert.ToString(row[expInfo.Column2]);
                                            else if (expInfo.Operator.Trim() == "Substring")
                                            {
                                                row[colVal] = Convert.ToString(row[expInfo.Column1]).Substring(Convert.ToInt32(expInfo.StartIndex), Convert.ToInt32(expInfo.Length));
                                            }
                                            else if (expInfo.Operator.Trim() == "Year Month")
                                            {
                                                row[colVal] = Convert.ToString(UGITUtility.StringToDateTime(row[expInfo.Column1]).ToString("yyyy", CultureInfo.InvariantCulture) + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(UGITUtility.StringToDateTime(row[expInfo.Column1]).Month));
                                            }
                                            else if (expInfo.Operator.Trim() == "Month Year")
                                            {
                                                row[colVal] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(row[expInfo.Column1]).Month) + " " + Convert.ToString(Convert.ToDateTime(row[expInfo.Column1]).ToString("yyyy", CultureInfo.InvariantCulture));
                                            }
                                            else if (expInfo.Operator.Trim() == "Month")
                                            {
                                                row[colVal] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(row[expInfo.Column1]).Month);
                                            }
                                            else if (expInfo.Operator.Trim() == "Year")
                                            {
                                                row[colVal] = Convert.ToString(Convert.ToDateTime(row[expInfo.Column1]).ToString("yyyy", CultureInfo.InvariantCulture));
                                            }
                                            else
                                                row[colVal] = "";
                                        }
                                    }
                                    catch { row[colVal] = ""; }
                                }
                            }
                            targetTable.Rows.Add(row.ItemArray);
                        }

                        temp = targetTable.Copy();
                    }
                    else { return null; }
                }



                ///Handling Where Condition
                DataTable dtRowIdTable = temp.Clone();
                var whereClauses = customQueryInfo.WhereClauses;
                if (whereFilter != string.Empty)
                {
                    string[] param_values = whereFilter.Split(new string[] { "," }, StringSplitOptions.None);
                    int j = 0;
                    for (int i = 0; i < whereClauses.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(whereClauses[i].ParameterName) && j < param_values.Length)
                        {
                            whereClauses[i].Value = param_values[j];
                            j++;
                        }
                    }
                }

                StringBuilder rootWheresb = new StringBuilder();

                List<WhereInfo> rootWhere = whereClauses.Where(x => x.ParentID == 0).ToList();
                UserProfileManager profileManager = new UserProfileManager(context);
                foreach (WhereInfo rWhere in rootWhere)
                {
                    RelationalOperator initialOperator = rWhere.RelationOpt;
                    //if (rWhere.ColumnName.EndsWith("Lookup") && !rWhere.ColumnName.EndsWith("CRMCompanyLookup"))
                    //    rWhere.ColumnName = rWhere.ColumnName + "$";
                    List<WhereInfo> subWhere = new List<WhereInfo>();
                    rWhere.RelationOpt = RelationalOperator.None;
                    subWhere.Add(rWhere);
                    subWhere.AddRange(whereClauses.Where(x => x.ParentID == rWhere.ID));
                    StringBuilder Wheresb = new StringBuilder();

                    foreach (WhereInfo where in subWhere)
                    {
                        if (where.Valuetype == qValueType.Parameter && string.IsNullOrEmpty(where.Value))
                            continue;

                        if (where.DataType == "DateTime" || where.DataType == "Date")
                        {
                            if (where.Valuetype == qValueType.Variable || where.Valuetype == qValueType.Parameter)
                            {
                                where.Value = ExpressionCalc.ExecuteFunctions(context, uHelper.ReplaceTokenWithValue(context, where.Value), businessDays: false);
                            }

                            DateTime dtValue = DateTime.MinValue;
                            DateTime.TryParseExact(where.Value.Replace("-", "/"), "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
                            if (dtValue == DateTime.MinValue)
                                DateTime.TryParse(where.Value.Replace("-", "/"), out dtValue);
                            dtValue = dtValue.Date; // Only compare using date portion (not time)

                            Wheresb.Append(Wheresb.ToString() == string.Empty ? "" : where.RelationOpt.ToString());

                            if (where.Operator == OperatorType.Equal)
                            {
                                // >= date (at 0:00am)
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");
                                Wheresb.Append(" >= ");
                                Wheresb.Append("'" + dtValue.ToString() + "' ");

                                Wheresb.Append(" And ");

                                // AND < date+1 (at 0:00am)
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");
                                Wheresb.Append(" < ");
                                Wheresb.Append("'" + dtValue.AddDays(1).ToString() + "' ");
                            }
                            else if (where.Operator == OperatorType.GreaterThan || where.Operator == OperatorType.GreaterThanEqualTo)
                            {
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");

                                if (where.Operator == OperatorType.GreaterThanEqualTo)
                                {
                                    // >= date (at 0:00am)
                                    Wheresb.Append(" >= ");
                                    Wheresb.Append("'" + dtValue.ToString() + "' ");
                                }
                                else
                                {
                                    // > date+1 (at 0:00am)
                                    Wheresb.Append(" > ");
                                    Wheresb.Append("'" + dtValue.AddDays(1).ToString() + "' ");
                                }
                            }
                            else if (where.Operator == OperatorType.LessThan || where.Operator == OperatorType.LessThanEqualTo)
                            {
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");
                                Wheresb.AppendFormat(" < ");
                                if (where.Operator == OperatorType.LessThanEqualTo)
                                {
                                    // < date+1 (at 0:00am)
                                    Wheresb.Append("'" + dtValue.AddDays(1).ToString() + "' ");
                                }
                                else
                                {
                                    // < date (at 0:00am)
                                    Wheresb.Append("'" + dtValue.ToString() + "' ");
                                }
                            }
                            else // Not Equal
                            {
                                // < date (at 0:00am)
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");
                                Wheresb.Append(" < ");
                                Wheresb.Append("'" + dtValue.ToString() + "' ");

                                Wheresb.Append(" OR ");

                                // >= date+1 (at 0:00am)
                                Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");
                                Wheresb.Append(" >= ");
                                Wheresb.Append("'" + dtValue.AddDays(1).ToString() + "' ");
                            }
                        }
                        else
                        {
                            #region Calculation of 'MemberOf' operator
                            string selectedUserNames = string.Empty;

                            // Check if datatype is User for MemberOf operator and create data to query on temp table
                            if (where.DataType == "User" && where.Operator == OperatorType.MemberOf)
                            {
                                UserProfile selectedUser = profileManager.GetUserByUserName(where.Value);
                                List<string> selectedUsers = new List<string>();
                                List<string> usersInDataColumn = new List<string>();
                                List<string> commonUsers = new List<string>();

                                //If selectedUser is null then given value is either a group of system admin
                                if (selectedUser == null)
                                {
                                    Role group = profileManager.GetUserRoleByGroupName(where.Value);
                                    List<UserProfile> groupUsersList = profileManager.GetUserProfilesByGroupName(where.Value);

                                    // Add group users name
                                    if (groupUsersList != null && groupUsersList.Count > 0)
                                        selectedUsers = groupUsersList.Where(y => !string.IsNullOrEmpty(y.Name)).Select(x => x.Name).ToList();

                                    // Add group name
                                    if (group != null && !string.IsNullOrEmpty(group.Title))
                                        selectedUsers.Add(group.Title);
                                }
                                // given user is a SPUser
                                else
                                {
                                    if (!string.IsNullOrEmpty(selectedUser.Name))
                                        selectedUsers.Add(selectedUser.Name);
                                }

                                if (temp == null || temp.Rows.Count == 0)
                                {
                                    ULog.WriteLog("Resulting table is null or has no rows.");
                                    continue;
                                }

                                // Select distinct users from temp table for the particular column
                                usersInDataColumn = temp.AsEnumerable().Where(y => !string.IsNullOrEmpty(y.Field<string>(where.ColumnName))).Select(x => x.Field<string>(where.ColumnName)).Distinct().ToList();

                                bool containsMultiUser = false;
                                //Check if usersInDataColumn has multiple users in temp table
                                if (usersInDataColumn != null && usersInDataColumn.Count > 0)
                                    containsMultiUser = usersInDataColumn.Any(x => x.Contains(Constants.Separator6) || x.Contains(Constants.Separator));

                                // Manipulate users data in case of multiple users available in temp table
                                if (containsMultiUser && selectedUsers != null && selectedUsers.Count > 1)
                                {
                                    // Remove multiple users from list
                                    usersInDataColumn = usersInDataColumn.Where(x => !x.Contains(Constants.Separator6) && !x.Contains(Constants.Separator)).Distinct().ToList();

                                    // Fetch rows which have multiple users in given column
                                    DataRow[] multiUsersData = temp.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>(where.ColumnName))
                                        && (x.Field<string>(where.ColumnName).Contains(Constants.Separator6) || x.Field<string>(where.ColumnName).Contains(Constants.Separator))).ToArray();

                                    if (multiUsersData != null && multiUsersData.Length > 0)
                                    {
                                        int counter = 1;
                                        foreach (DataRow drow in multiUsersData)
                                        {
                                            string colSpecificValues = Convert.ToString(drow[where.ColumnName]);
                                            colSpecificValues = colSpecificValues.Replace(Constants.Separator, Constants.Separator6 + " ");

                                            // Remove lookup id from column value and get users in an array
                                            string[] multiUsersList = colSpecificValues.Split(',').Select(x => x.Trim()).ToArray();

                                            // Check in all users of the column value are available in selected users list
                                            bool allUsersUnmatched = true;
                                            allUsersUnmatched = multiUsersList.ToList().Exists(x => !selectedUsers.Contains(x));

                                            // Insert separate rows for each user in temp table and remove related old row
                                            if (!allUsersUnmatched)
                                            {
                                                // Add a column called 'Row_Id' if temp table and its clone table 'dtRowIdTable' don't have
                                                if (counter == 1 && !uHelper.IfColumnExists("Row_Id", temp) && !uHelper.IfColumnExists("Row_Id", dtRowIdTable))
                                                {
                                                    temp.Columns.Add("Row_Id");
                                                    dtRowIdTable.Columns.Add("Row_Id");
                                                }

                                                // Add a new row for each user in temp table
                                                DataRow row = null;
                                                for (int i = 0; i < multiUsersList.Length; i++)
                                                {
                                                    row = temp.NewRow();
                                                    row = drow;
                                                    row[where.ColumnName] = multiUsersList[i];
                                                    row["Row_Id"] = where.ColumnName + "_" + counter;
                                                    temp.Rows.Add(row.ItemArray);

                                                    // Add each user separately in usersInDataColumn list
                                                    if (!usersInDataColumn.Contains(multiUsersList[i]))
                                                        usersInDataColumn.Add(multiUsersList[i]);
                                                }

                                                // Insert the main data row in dtRowIdTable table for further manipulation
                                                DataRow rowIdRow = dtRowIdTable.NewRow();
                                                rowIdRow = drow;
                                                rowIdRow[where.ColumnName] = colSpecificValues;
                                                rowIdRow["Row_Id"] = where.ColumnName + "_" + counter;
                                                dtRowIdTable.Rows.Add(rowIdRow.ItemArray);

                                                // Remove the main row from temp table
                                                drow.Delete();
                                                temp.AcceptChanges();
                                                counter++;
                                            }
                                        }
                                    }
                                }
                                // Find the common users from usersInDataColumn and selectedUsers lists
                                if (usersInDataColumn != null && usersInDataColumn.Count > 0)
                                {
                                    var result = selectedUsers.Intersect(usersInDataColumn);
                                    if (result != null && result.Count() > 0)
                                        commonUsers = result.ToList();
                                }

                                // Add users in a string to be used in query
                                if (commonUsers != null && commonUsers.Count > 0)
                                    selectedUserNames = string.Join("','", commonUsers.Select(x => x).ToArray());
                            }
                            #endregion Calculation of 'MemberOf' operator

                            // Calculation for user field
                            else if (where.DataType == "User" && where.Operator != OperatorType.MemberOf)
                            {
                                UserProfile selectedUser = profileManager.GetUserByUserName(where.Value);
                                if (selectedUser != null)
                                {
                                    if (!string.IsNullOrEmpty(selectedUser.Name))
                                        where.Value = selectedUser.Name;
                                }
                                else
                                {
                                    Role group = profileManager.GetUserRoleByGroupName(where.Value);
                                    if (group != null && !string.IsNullOrEmpty(group.Title))
                                        where.Value = group.Title;
                                }
                            }

                            Wheresb.Append(Wheresb.ToString() == string.Empty ? "" : where.RelationOpt.ToString());

                            if (string.IsNullOrWhiteSpace(where.Value))
                            {
                                Wheresb.Append(" (");
                                Wheresb.AppendFormat("[{0}] is null OR ", where.ColumnName.Replace(" ", "_"));
                            }

                            Wheresb.Append(" [" + where.ColumnName.Replace(" ", "_") + "] ");

                            // Add equal to operator if the orerator is MemberOf but selected users are empty
                            if (where.Operator == OperatorType.MemberOf && string.IsNullOrEmpty(selectedUserNames))
                            {
                                Wheresb.AppendFormat(" {0} ", GetOperatorFromType(OperatorType.Equal));
                            }
                            else
                                Wheresb.AppendFormat(" {0} ", GetOperatorFromType(where.Operator));

                            if (where.DataType == "Boolean")
                            {
                                string value = UGITUtility.StringToBoolean(where.Value) ? "'1'" : "'0'";
                                if (temp != null && temp.Columns.Contains(where.ColumnName) && temp.Columns[where.ColumnName].DataType.ToString() == "System.Boolean")
                                    value = UGITUtility.StringToBoolean(where.Value) ? "1" : "0";
                                string tempWhereb = Wheresb.ToString();
                                Wheresb.Append(value);
                                Wheresb.Append(" OR " + tempWhereb + " '" + where.Value + "' ");
                            }
                            else if (where.DataType == "Double" || where.DataType == "Integer" || where.DataType == "Number")
                            {
                                Wheresb.Append(where.Value);
                            }
                            else
                            {
                                if (where.Value.ToLower() == "[$me$]")
                                {
                                    where.Value = context.CurrentUser.Name;
                                }
                                else if (where.Value.ToLower() == "[$mydepartment$]")
                                {
                                    DepartmentManager deptManager = new DepartmentManager(context);
                                    Department department = deptManager.LoadByID(context.CurrentUser.DepartmentId);

                                    if (department != null && !string.IsNullOrEmpty(department.Title))
                                        where.Value = department.Title;
                                }

                                if (where.Operator == OperatorType.like)
                                {
                                    Wheresb.Append(" '%" + where.Value + "%' ");
                                }
                                else if (where.Operator == OperatorType.MemberOf && !string.IsNullOrEmpty(selectedUserNames))
                                {
                                    Wheresb.Append(" ('" + selectedUserNames + "') ");
                                }
                                else if (where.Operator == OperatorType.NotEqual && (where.Value).Contains('1') || (where.Value).Contains('0'))
                                {
                                    if(where.Value=="1")
                                        where.Value = "True";
                                    else
                                        where.Value = "False";
                                    Wheresb.Append(" '" + where.Value + "' ");
                                }
                                else
                                {
                                    Wheresb.Append(" '" + where.Value + "' ");
                                }
                            }

                            if (string.IsNullOrWhiteSpace(where.Value))
                            {
                                Wheresb.Append(") ");
                            }
                        }
                    }

                    if (Wheresb.ToString().Trim() != string.Empty)
                    {
                        if (initialOperator != RelationalOperator.None && !string.IsNullOrEmpty(rootWheresb.ToString()))
                        {
                            rootWheresb.AppendFormat(" {0} ", initialOperator.ToString());
                        }
                        rootWheresb.AppendFormat(" ({0}) ", Wheresb.ToString());
                    }
                }

                if (rootWheresb.ToString().Trim() != string.Empty)
                {
                    DataRow[] drows = temp.Select(rootWheresb.ToString());

                    if (drows != null && drows.Count() > 0)
                    {
                        temp = drows.CopyToDataTable();

                        // Check if 'Row_Id' is available in temp table which is added for the calculation of 'MemberOf' operator, 
                        // if yes then manipulate temp table for the required result
                        if (uHelper.IfColumnExists("Row_Id", temp) && dtRowIdTable != null && dtRowIdTable.Rows.Count > 0)
                        {
                            // Get Distinct users from Row_Id column from temp table
                            List<string> distinctRowID = new List<string>();
                            distinctRowID = temp.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("Row_Id"))).Select(y => y.Field<string>("Row_Id")).Distinct().ToList();

                            // Remove rows from temp table which have values for Row_Id column
                            var tempResult = temp.AsEnumerable().Where(x => string.IsNullOrEmpty(x.Field<string>("Row_Id"))).ToArray();

                            if (tempResult != null && tempResult.Length > 0)
                                temp = tempResult.CopyToDataTable();
                            else
                                temp.Rows.Clear();

                            // Find rows from dtRowIdTable which has same Row_Id as in distinctRowID list and instert them in temp table
                            DataRow[] resultRowIdData = dtRowIdTable.AsEnumerable().Where(x => distinctRowID.Exists(y => y == x.Field<string>("Row_Id"))).ToArray();

                            if (resultRowIdData != null && resultRowIdData.Length > 0)
                            {
                                foreach (DataRow row in resultRowIdData)
                                {
                                    temp.Rows.Add(row.ItemArray);
                                }
                            }

                            // Remove Row_Id column from temp table which was added for the calculation of 'MemberOf' operator
                            temp.Columns.Remove("Row_Id");
                            temp.AcceptChanges();
                        }
                    }
                    else { return null; }
                }

                List<TableInfo> queryTables = isdrilldown ? customQueryInfo.DrillDownTables : customQueryInfo.Tables;
                List<Utility.ColumnInfo> qColumnsList = new List<Utility.ColumnInfo>();
                foreach (TableInfo table in queryTables)
                {
                    // Removed DataType None, condition, to prevent Query Report crash
                    //qColumnsList.AddRange(table.Columns.Where(x => x.DataType != "none").OrderBy(c => c.Sequence).ToArray());
                    qColumnsList.AddRange(table.Columns.OrderBy(c => c.Sequence).ToArray());
                }

                qColumnsList = qColumnsList.OrderBy(c => c.Sequence).ToList();
                string[] arrColumns = null;
                if (isUnion)
                {
                    qColumnsList = qColumnsList.AsEnumerable().GroupBy(x => new { field = x.FieldName }).Select(z => z.FirstOrDefault()).ToList();
                    arrColumns = qColumnsList.Select(c => c.FieldName).Distinct().ToArray();
                }
                else
                {
                    arrColumns = qColumnsList.Select(c => c.TableName.Replace(" ", "_") + "." + c.FieldName).Distinct().ToArray();
                }


                // Calculation for TicketTotalHoldDuration column
                string[] totalHoldDurationColumns = arrColumns.Where(x => x.EndsWith(DatabaseObjects.Columns.TicketTotalHoldDuration)).ToArray();
                if (totalHoldDurationColumns != null && totalHoldDurationColumns.Length > 0)
                {
                    foreach (string totalHoldDurationColName in totalHoldDurationColumns)
                    {
                        string tableName = isUnion ? string.Empty : totalHoldDurationColName.Split('.')[0];
                        string colNamePrefix = string.IsNullOrEmpty(tableName) ? string.Empty : tableName + ".";

                        // Update TicketTotalHoldDuration 
                        foreach (DataRow dRow in temp.Rows)
                        {
                            if (temp.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketOnHold) && UGITUtility.StringToBoolean(dRow[colNamePrefix + DatabaseObjects.Columns.TicketOnHold]))
                                dRow[totalHoldDurationColName] = Math.Round(Ticket.GetTotalHoldTime(context, dRow, false, colNamePrefix), 0);
                            else
                                dRow[totalHoldDurationColName] = Math.Round(UGITUtility.StringToDouble(dRow[totalHoldDurationColName]), 0);
                        }
                    }
                }

                ///Remove Columns from table which are not selected.
                List<Utility.ColumnInfo> _columns = new List<Utility.ColumnInfo>();
                string[] col = null;
                List<TableInfo> qTables = isdrilldown ? customQueryInfo.DrillDownTables : customQueryInfo.Tables;
                foreach (TableInfo table in qTables)
                {
                    // Removed DataType None, condition, to prevent Query Report crash
                    //_columns.AddRange(table.Columns.Where(x => x.DataType != "none").OrderBy(c => c.Sequence).ToArray());
                    _columns.AddRange(table.Columns.OrderBy(c => c.Sequence).ToArray());
                }
                //_columns= _columns.GroupBy(elem => elem.FieldName).Select(group => group.First()).OrderBy(c => c.Sequence).ToList();
                _columns = _columns.OrderBy(c => c.Sequence).ToList();
                if (isUnion)
                {
                    _columns = _columns.AsEnumerable().GroupBy(x => new { field = x.FieldName }).Select(z => z.FirstOrDefault()).ToList();
                    col = _columns.Select(c => c.FieldName).Distinct().ToArray();
                }
                else
                    col = _columns.Select(c => c.TableName.Replace(" ", "_") + "." + c.FieldName).Distinct().ToArray();


                //for (int i = 0; i < col.Length; i++)
                //{
                //    if (Convert.ToString(col[i]).EndsWith("User") || Convert.ToString(col[i]).EndsWith("Lookup") && !Convert.ToString(col[i]).EndsWith("CRMCompanyLookup"))
                //        col[i] = Convert.ToString(col[i]) + "$";

                //}
                
                #endregion
                if (customQueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects) && col.Contains(DatabaseObjects.Tables.PMMProjects + "." + DatabaseObjects.Columns.ProjectHealth))
                {
                    if (!uHelper.IfColumnExists(DatabaseObjects.Columns.ProjectHealth, temp))
                        temp.Columns.Add(DatabaseObjects.Tables.PMMProjects+"."+DatabaseObjects.Columns.ProjectHealth);
                }
                if (customQueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects) && col.Contains(DatabaseObjects.Tables.PMMProjects + "." + "GanttView"))
                {
                    if (!uHelper.IfColumnExists("GanttView", temp))
                        temp.Columns.Add(DatabaseObjects.Tables.PMMProjects+"."+ "GanttView");
                }

                //if (temp != null && temp.Columns.Count > 0 && temp.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                //{
                //    List<string> lstColmns = col.ToList();
                //    if (lstColmns != null)
                //        lstColmns.Add(DatabaseObjects.Columns.ModuleName);
                //    col = lstColmns.ToArray();

                //    Utility.ColumnInfo columnInfo = new Utility.ColumnInfo();
                //    columnInfo.DisplayName = DatabaseObjects.Columns.ModuleName;
                //    columnInfo.FieldName = DatabaseObjects.Columns.ModuleName;
                //    _columns.Add(columnInfo);
                //}
                temp = temp.DefaultView.ToTable(false, col);
                #region Modify field data
                foreach (TableInfo tInfo in customQueryInfo.Tables)
                {
                    if (tInfo.Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.Attachments))
                    {
                        Utility.ColumnInfo columnInfo = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.Attachments);
                        DataTable list = GetTableDataManager.GetTableData(tInfo.Name, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                        if (list != null)
                        {
                            Utility.ColumnInfo idColumn = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.Id);
                            if (idColumn != null)
                            {
                                string attachmentColumnName = string.Empty;
                                string idColumnName = string.Empty;
                                if (isUnion)
                                {
                                    attachmentColumnName = string.Format("{0}", columnInfo.FieldName);
                                    idColumnName = string.Format("{0}", idColumn.FieldName);
                                }
                                else
                                {
                                    attachmentColumnName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), columnInfo.FieldName);
                                    idColumnName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), idColumn.FieldName);
                                }

                                DataRow[] collection = list.Select();
                                foreach (DataRow row in temp.Rows)
                                {
                                    DataRow item = list.Select(string.Format("{0}={1}", "ID", Convert.ToInt32(row[idColumnName])))[0];
                                    row[attachmentColumnName] = string.Empty;
                                    if (item != null)
                                    {
                                        // test this in below line of code - Convert.ToString(item["Attachments"]).ToArray() 
                                        row[attachmentColumnName] = string.Join("; ", item["Attachments"]);
                                    }
                                }
                            }
                        }
                    }

                    // To remove special character from multi value field.
                    if (tInfo.Columns.Exists(x => x.DataType != null && x.DataType.ToLower().Contains("multi")))
                    {
                        List<Utility.ColumnInfo> multiValColumns = tInfo.Columns.Where(x => x.DataType != null && x.DataType.ToLower().Contains("multi")).ToList();
                        foreach (Utility.ColumnInfo cInfo in multiValColumns)
                        {
                            string rTCName = string.Empty;
                            if (cInfo != null)
                            {
                                if (isUnion)
                                    rTCName = string.Format("{0}", cInfo.FieldName);
                                else
                                    rTCName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), cInfo.FieldName);
                            }

                            if (string.IsNullOrEmpty(rTCName))
                            {
                                continue;
                            }
                            foreach (DataRow row in temp.Rows)
                            {
                                if (row[rTCName] == null)
                                    continue;
                                string value = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[rTCName]));
                                row[rTCName] = string.Join("; ", UGITUtility.SplitString(value, Constants.Separator, StringSplitOptions.RemoveEmptyEntries));
                            }
                        }
                    }

                    if (tInfo.Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketComment) ||
                        tInfo.Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.UGITComment) ||
                        tInfo.Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.TicketResolutionComments) ||
                        tInfo.Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.History))
                    {
                        Utility.ColumnInfo commentColumn = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.TicketComment);
                        if (commentColumn == null)
                            commentColumn = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.UGITComment);
                        Utility.ColumnInfo resolutionColumn = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.TicketResolutionComments);
                        Utility.ColumnInfo historyColumn = tInfo.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.History);

                        string commntCName = string.Empty;
                        string historyCName = string.Empty;
                        string resolutionCName = string.Empty;

                        if (commentColumn != null)
                        {
                            if (isUnion)
                                commntCName = string.Format("{0}", commentColumn.FieldName);
                            else
                                commntCName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), commentColumn.FieldName);
                        }

                        if (resolutionColumn != null)
                        {
                            if (isUnion)
                                resolutionCName = string.Format("{0}", resolutionColumn.FieldName);
                            else
                                resolutionCName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), resolutionColumn.FieldName);
                        }

                        if (historyColumn != null)
                        {
                            if (isUnion)
                                historyCName = string.Format("{0}", historyColumn.FieldName);
                            else
                                historyCName = string.Format("{0}.{1}", tInfo.Name.Replace(" ", "_"), historyColumn.FieldName);
                        }

                        foreach (DataRow row in temp.Rows)
                        {
                            if (commentColumn != null)
                            {
                                if (commentColumn.DataType == "Last Entry")
                                {
                                    List<HistoryEntry> lstComments = uHelper.GetHistory(Convert.ToString(row[commntCName]), false);
                                    if (lstComments != null && lstComments.Count > 0)
                                        row[commntCName] = lstComments[0].entry;
                                }
                                else
                                    row[commntCName] = UGITUtility.GetFormattedHistoryString(Convert.ToString(row[commntCName]), false, false, false);
                            }

                            if (resolutionColumn != null)
                            {
                                if (resolutionColumn.DataType == "Last Entry")
                                {
                                    List<HistoryEntry> lstResolution = uHelper.GetHistory(Convert.ToString(row[resolutionCName]), false);
                                    if (lstResolution != null && lstResolution.Count > 0)
                                        row[resolutionCName] = lstResolution[0].entry;
                                }
                                else
                                    row[resolutionCName] = UGITUtility.GetFormattedHistoryString(Convert.ToString(row[resolutionCName]), false, false, false);
                            }

                            if (historyColumn != null)
                            {
                                if (historyColumn.DataType == "Last Entry")
                                {
                                    List<HistoryEntry> lstHistory = uHelper.GetHistory(Convert.ToString(row[historyCName]), false);
                                    if (lstHistory != null && lstHistory.Count > 0)
                                        row[historyCName] = lstHistory[0].entry;
                                }
                                else
                                    row[historyCName] = UGITUtility.GetFormattedHistoryString(Convert.ToString(row[historyCName]), false, false, false);

                            }
                        }
                    }
                }
                #endregion

                // Add a column called TicketUrl to keep the url of Tickets
                if (temp != null && temp.Rows.Count > 0)
                {
                    
                    string ticketIdColName = string.Empty;

                    // Get TicketId column name if TicketId column exists
                    bool isTicketIdColExist = temp.Columns.Cast<DataColumn>().Any(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.TicketId));

                    if (isTicketIdColExist)
                        ticketIdColName = temp.Columns.Cast<DataColumn>().Where(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.TicketId)).Select(y => y).FirstOrDefault().ToString();

                    if (!string.IsNullOrEmpty(ticketIdColName))
                    {
                        if (!uHelper.IfColumnExists("TicketUrl", temp))
                            temp.Columns.Add("TicketUrl");

                        ModuleViewManager moduleManager = new ModuleViewManager(context);
                        UGITModule moduleDetail = null;
                        // Save ticketUrl for each row
                        foreach (DataRow row in temp.Rows)
                        {
                            string viewUrl = string.Empty;
                            string title = string.Empty;
                            string func = string.Empty;
                            string ticketId = Convert.ToString(row[ticketIdColName]);

                            if (string.IsNullOrEmpty(ticketId))
                                continue;

                            string moduleName = uHelper.getModuleNameByTicketId(ticketId);

                            if (string.IsNullOrEmpty(moduleName))
                                continue;

                            //UGITModule moduleDetail = moduleManager.LoadByName(moduleName);
                            moduleDetail = moduleManager.LoadByName(moduleName);
                            if (moduleDetail == null)
                                continue;

                            if (!string.IsNullOrEmpty(moduleDetail.StaticModulePagePath))
                                viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath);

                            string ticketTitle = string.Empty;
                            if (moduleName == "CMDB")
                            {
                                string assetNameColTitle = string.Empty;
                                bool isAssetColExist = temp.Columns.Cast<DataColumn>().Any(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.AssetName));

                                if (isAssetColExist)
                                    assetNameColTitle = temp.Columns.Cast<DataColumn>().Where(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.AssetName)).Select(y => y).FirstOrDefault().ToString();

                                if (!string.IsNullOrEmpty(assetNameColTitle))
                                    ticketTitle = UGITUtility.TruncateWithEllipsis(Convert.ToString(row[assetNameColTitle]), 100);
                            }
                            else
                            {
                                string titleColumn = string.Empty;
                                bool isTitleColExist = temp.Columns.Cast<DataColumn>().Any(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.Title));

                                if (isTitleColExist)
                                    titleColumn = temp.Columns.Cast<DataColumn>().Where(x => Convert.ToString(x).Contains(DatabaseObjects.Columns.Title)).Select(y => y).FirstOrDefault().ToString();

                                if (!string.IsNullOrEmpty(titleColumn))
                                    ticketTitle = UGITUtility.TruncateWithEllipsis(Convert.ToString(row[titleColumn]), 100);
                            }

                            if (!string.IsNullOrWhiteSpace(ticketTitle))
                                title = string.Format("{0}: {1}", ticketId, ticketTitle);
                            else
                                title = ticketId;

                            // Remove special characters like # ' " from the title
                            title = uHelper.ReplaceInvalidCharsInURL(title);

                            // Save ticketUrl if viewUrl is not empty
                            if (!string.IsNullOrEmpty(viewUrl))
                            {
                                string width = "90";
                                string height = "90";
                                func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceUrl, width, height);
                                row["TicketUrl"] = func;
                            }
                        }
                    }
                }

                ///Columns Field Name change into Display Name.
                tempTable = new DataTable();
                if (temp != null && temp.Rows.Count > 0)
                {
                    string colname = string.Empty;
                    int i = 1;
                    if (_columns != null)
                    {
                        foreach (var _c in _columns)
                        {
                            colname = _c.DisplayName;
                            i = 1;

                            if (!isUnion)
                            {
                                while (tempTable.Columns.Contains(colname))
                                {
                                    colname = _c.DisplayName + (i++).ToString();
                                }
                            }
                            tempTable.Columns.Add(new DataColumn(colname, GetDataType(_c.DataType)));
                        }
                    }

                    // Add TicketUrl column in tempTable if it is available in temp 
                    if (uHelper.IfColumnExists("TicketUrl", temp))
                        tempTable.Columns.Add("TicketUrl");

                    foreach (DataRow dr in temp.Rows)
                    {
                        var row = dr.ItemArray;
                        tempTable.Rows.Add(row);
                    }
                }

                ///Column Functions if any
                IEnumerable<IGrouping<string, DataRow>> groupedDataRow = null;
                if (_columns != null && _columns.Exists(x => !string.IsNullOrEmpty(x.Function) && x.Function != "none"))
                {
                    DataTable groupDataTable = tempTable.Clone();

                    groupedDataRow = GetGroupByData(_columns, tempTable);
                    foreach (var keys in groupedDataRow)
                    {
                        DataRow dr = groupDataTable.NewRow();
                        foreach (var c in _columns)
                        {
                            switch (c.Function)
                            {
                                case "Count":
                                    dr[c.DisplayName] = keys
                                                .Where(r => r[c.DisplayName] != null)
                                                .Select(x => x[c.DisplayName])
                                                .Count();
                                    break;
                                case "DistinctCount":
                                    dr[c.DisplayName] = keys
                                                .Where(r => r[c.DisplayName] != null)
                                                .Select(x => x[c.DisplayName])
                                                .Distinct()
                                                .Count();
                                    break;
                                case "Avg":

                                    double sum = keys
                                                 .Where(r => r[c.DisplayName] != null)
                                                 .Sum(x => UGITUtility.StringToDouble(x[c.DisplayName]));
                                    dr[c.DisplayName] = sum / keys.Count();
                                    break;
                                case "Sum":
                                    dr[c.DisplayName] = keys
                                                 .Where(r => r[c.DisplayName] != null)
                                                 .Sum(x => (double)x[c.DisplayName]);
                                    break;
                                case "Max":
                                    if (c.DataType != "DateTime")
                                    {
                                        dr[c.DisplayName] = keys
                                                     .Where(r => r[c.DisplayName] != null)
                                                     .Max(x => (double)x[c.DisplayName]);
                                    }
                                    else
                                    {
                                        dr[c.DisplayName] = (DateTime)keys
                                                       .Where(r => r[c.DisplayName] != null)
                                                       .OrderByDescending(x => (DateTime)x[c.DisplayName]).FirstOrDefault()[c.DisplayName];
                                    }
                                    break;
                                case "Min":
                                    if (c.DataType != "DateTime")
                                    {
                                        dr[c.DisplayName] = keys
                                                     .Where(r => r[c.DisplayName] != null)
                                                     .Min(x => (double)x[c.DisplayName]);
                                    }
                                    else
                                    {
                                        dr[c.DisplayName] = (DateTime)keys
                                                       .Where(r => r[c.DisplayName] != null)
                                                       .OrderBy(x => (DateTime)x[c.DisplayName]).FirstOrDefault()[c.DisplayName];
                                    }
                                    break;
                                default:
                                    dr[c.DisplayName] = keys
                                                 .Where(r => r[c.DisplayName] != null)
                                                 .Select(x => x[c.DisplayName]).FirstOrDefault();
                                    break;
                            }
                        }
                        groupDataTable.Rows.Add(dr);
                    }
                    tempTable = groupDataTable.Copy();
                }

                ///Order by if any
                string orderexpre = string.Empty;
                if (customQueryInfo.OrderBy != null && customQueryInfo.OrderBy.Count > 0)
                {
                    var orderbys = customQueryInfo.OrderBy;
                    foreach (var orderby in orderbys)
                    {
                        if (tempTable.Columns.Contains(orderby.Column.DisplayName))
                            orderexpre += string.Format("[{0}] {1},", orderby.Column.DisplayName, orderby.orderBy.ToString());
                    }

                    if (!string.IsNullOrEmpty(orderexpre))
                    {
                        orderexpre = orderexpre.Substring(0, orderexpre.Length - 1);
                        tempTable.DefaultView.Sort = orderexpre;
                    }
                }

                ///Totals on Records from Table.
                if (customQueryInfo.Totals != null && customQueryInfo.Totals.Count > 0)
                {
                    totalsTable = new DataTable();
                    foreach (DataColumn c in tempTable.Columns)
                        totalsTable.Columns.Add(c.ColumnName);

                    DataRow dr = totalsTable.NewRow();
                    foreach (var ci in customQueryInfo.Totals)
                    {

                        double val = 0;
                        string value = string.Empty;
                        switch (ci.Function)
                        {
                            case "Count":
                                val = tempTable.AsEnumerable()
                                    .Where(r => r[ci.DisplayName] != null)
                                    .Select(r => r[ci.DisplayName])
                                    .Count();
                                value = "Count: " + val.ToString();
                                break;
                            case "DistinctCount":
                                val = tempTable.AsEnumerable()
                                    .Where(r => r[ci.DisplayName] != null)
                                    .Select(r => r[ci.DisplayName]).Distinct()
                                    .Count();
                                value = "Count: " + val.ToString();
                                break;
                            case "Avg":
                                val = tempTable.AsEnumerable()
                                    .Where(r => !string.IsNullOrEmpty(Convert.ToString(r[ci.DisplayName])))
                                    .Select(r => UGITUtility.StringToDouble(r[ci.DisplayName]))
                                    .Sum();
                                val = val / tempTable.Rows.Count;
                                if (ci.DataType == "Currency")
                                {
                                    value = "Avg: " + string.Format("{0:C}", val);
                                }
                                else
                                {
                                    value = "Avg: " + val.ToString();
                                }
                                break;
                            case "Sum":
                                val = tempTable.AsEnumerable()
                                    .Where(r => !string.IsNullOrEmpty(Convert.ToString(r[ci.DisplayName])))
                                    .Select(r => UGITUtility.StringToDouble(r[ci.DisplayName]))
                                    .Sum();
                                if (ci.DataType == "Currency")
                                {
                                    value = "Sum: " + string.Format("{0:C}", val);
                                }
                                else
                                {
                                    value = "Sum: " + val.ToString();
                                }
                                break;
                            case "Max":
                                if (ci.DataType != "DateTime")
                                {
                                    val = tempTable.AsEnumerable()
                                        .Where(r => !string.IsNullOrEmpty(Convert.ToString(r[ci.DisplayName])))
                                        .Select(r => UGITUtility.StringToDouble(r[ci.DisplayName]))
                                        .Max();
                                    if (ci.DataType == "Currency")
                                    {
                                        value = "Max: " + string.Format("{0:C}", val);
                                    }
                                    else
                                    {
                                        value = "Max: " + val.ToString();
                                    }
                                }
                                else
                                {
                                    var myval = tempTable.AsEnumerable()
                                                .Where(r => r[ci.DisplayName] != null)
                                                .OrderByDescending(r => (DateTime)r[ci.DisplayName]).FirstOrDefault();
                                    value = "Max: " + (DateTime)myval[ci.DisplayName];
                                }
                                break;
                            case "Min":
                                if (ci.DataType != "DateTime")
                                {
                                    val = tempTable.AsEnumerable()
                                        .Where(r => !string.IsNullOrEmpty(Convert.ToString(r[ci.DisplayName])))
                                        .Select(r => UGITUtility.StringToDouble(r[ci.DisplayName]))
                                        .Min();
                                    if (ci.DataType == "Currency")
                                    {
                                        value = "Min: " + string.Format("{0:C}", val);
                                    }
                                    else
                                    {
                                        value = "Min: " + val.ToString();
                                    }
                                }
                                else
                                {
                                    var myval = tempTable.AsEnumerable()
                                                .Where(r => r[ci.DisplayName] != null)
                                                .OrderBy(r => (DateTime)r[ci.DisplayName]).FirstOrDefault();
                                    value = "Min: " + (DateTime)myval[ci.DisplayName];
                                }
                                break;
                            default:
                                break;
                        }
                        dr[ci.DisplayName] = value;
                    }
                    totalsTable.Rows.Add(dr);
                }
              
                ///Remove Ids from Lookup string.
                temp = tempTable.DefaultView.ToTable();
                foreach (DataRow row in temp.Rows)
                {
                    foreach (DataColumn dc in temp.Columns)
                    {
                        string val = Convert.ToString(row[dc]);
                        if (!string.IsNullOrEmpty(val) && val.Contains(Constants.Separator))
                        {
                            row[dc] = UGITUtility.RemoveIDsFromLookupString(val);
                        }

                        // Test below code 
                        //var fieldName = _columns.Where(x => x.DisplayName == dc.ColumnName).ToList();
                        //if (fieldName != null)
                        //{
                        //    foreach (var field in fieldName)
                        //    {
                        //        var configVal = field.FieldName;
                        //        if (fmanger.GetFieldByFieldName(configVal) != null && !string.IsNullOrEmpty(val) && (field.DataType != "Date" || field.DataType != "Datetime"))
                        //        {
                        //            string value = fmanger.GetFieldConfigurationData(configVal, val);//Need to remove
                        //            row[dc] = value;
                        //        }
                        //    }
                        //}
                    }
                }

                ///Add Special Field PMMProjects ProjectHealth(Critical Issue, Risk, etc..) .
                if (customQueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
                {
                    var table = customQueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects);
                    if (table != null)
                    {
                        var columnInfo = table.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth);
                        if (columnInfo != null)
                        {
                            ///Load All Monitor in case of pmm projects
                            var projectMonitorsStateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                            string monitorQuery = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM, DatabaseObjects.Columns.TenantID, context.TenantID);
                            var moduleMonitorsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, monitorQuery);

                            if (moduleMonitorsTable != null && moduleMonitorsTable.Rows.Count > 0)
                            {
                                foreach (DataRow monitor in moduleMonitorsTable.Rows)
                                {
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.MonitorName) && !temp.Columns.Contains(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString()))
                                    {
                                        temp.Columns.Add(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString());
                                    }
                                }
                            }

                            ///Adding Values to Special Field PMMProjects
                            DataTable dtFinalMonitor = new DataTable();
                            dtFinalMonitor.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup);
                            dtFinalMonitor.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup);
                            dtFinalMonitor.Columns.Add(DatabaseObjects.Columns.ModuleMonitorName);
                            dtFinalMonitor.Columns.Add(DatabaseObjects.Columns.ProjectMonitorNotes);

                            foreach (DataRow row in temp.Rows)
                            {
                                DataTable moduleMonitorOptions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitorOptions, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                                foreach (DataRow monitor in moduleMonitorsTable.Rows)
                                {
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.MonitorName) && temp.Columns.Contains(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString()))
                                    {
                                        string monitorName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                                        var ticketIdColumn = table.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.TicketId);
                                        string ticketID = Convert.ToString(row[ticketIdColumn.DisplayName]);
                                        string monitorid = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ID).ToString();
                                        DataRow[] projectMonitorState = projectMonitorsStateTable.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.ModuleMonitorNameLookup, monitorid));
                                        if (projectMonitorState.Length > 0)
                                        {
                                            DataRow commRow = dtFinalMonitor.NewRow();
                                            DataRow monitorOptionRow = moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == UGITUtility.StringToLong(monitorid) && x.Field<long>(DatabaseObjects.Columns.ID) == UGITUtility.StringToLong(projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                            //moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup == Convert.ToString(monitorId) && x.Field<string>(DatabaseObjects.Columns.ID)== Convert.ToString(projectMonitorState[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                            if (monitorOptionRow != null)
                                            {
                                                commRow[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionName];
                                                commRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClass];
                                            }
                                            commRow[DatabaseObjects.Columns.ModuleMonitorName] = monitor[DatabaseObjects.Columns.MonitorName];
                                            commRow[DatabaseObjects.Columns.ProjectMonitorNotes] = projectMonitorState[0][DatabaseObjects.Columns.ProjectMonitorNotes];
                                            dtFinalMonitor.Rows.Add(commRow);
                                            row[monitorName] = UGITUtility.GetMonitorsGraphic(commRow);
                                        }
                                    }
                                }
                            }
                        }

                        var projectSummaryNoteColumn = table.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.ProjectSummaryNote);
                        if (projectSummaryNoteColumn != null)
                        {
                            var ticketidcolumn = table.Columns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.TicketId);
                            if (ticketidcolumn != null)
                            {
                                foreach (DataRow dr in temp.Rows)
                                {
                                    if (Convert.ToString(dr[projectSummaryNoteColumn.DisplayName]) != string.Empty)
                                    {
                                        string ticketId = Convert.ToString(dr[ticketidcolumn.DisplayName]);
                                        DataRow myTicket = Ticket.GetCurrentTicket(context, ModuleNames.PMM, ticketId);
                                        List<HistoryEntry> projectNotes = uHelper.GetHistory(myTicket, DatabaseObjects.Columns.ProjectSummaryNote, false);
                                        if (projectNotes != null && projectNotes.Count > 0)
                                        {
                                            HistoryEntry projectNote = projectNotes.First();
                                            dr[projectSummaryNoteColumn.DisplayName] = UGITUtility.StripHTML(projectNote.entry);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in temp.Rows)
                                {
                                    if (Convert.ToString(dr[projectSummaryNoteColumn.DisplayName]) != string.Empty)
                                    {
                                        var projectNotes = uHelper.GetProjectSummaryNote(Convert.ToString(dr[projectSummaryNoteColumn.DisplayName]));
                                        HistoryEntry projectNote = projectNotes.First();
                                        dr[projectSummaryNoteColumn.DisplayName] = UGITUtility.StripHTML(projectNote.entry);
                                    }
                                }
                            }
                        }
                    }
                }
                dt = temp;
                tempTable = null;
                temp = null;
                tempds = null;

                return dt;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error getting report data for query table");
                return null;
            }
        }

        #region Get Group by Data by Linq Statement
        static StringBuilder stringBuilder = new StringBuilder();

        static List<string> columnName = null;

        public static String GroupData(DataRow dataRow)
        {
            String[] columnNames = columnName.ToArray();
            stringBuilder.Remove(0, stringBuilder.Length);
            foreach (String column in columnNames)
            {
                stringBuilder.Append(dataRow[column].ToString());
            }
            return stringBuilder.ToString();
        }

        private IEnumerable<IGrouping<string, DataRow>> GetGroupByData(List<Utility.ColumnInfo> columns, DataTable dt)
        {

            columnName = new List<string>();
            if (columns != null && columns.Exists(x => !string.IsNullOrEmpty(x.Function)))
            {
                foreach (var item in columns.Where(x => x.Function == "none"))
                {
                    //columnName.Add(string.Format("{0}.{1}", item.TableName, item.FieldName));
                    columnName.Add(item.DisplayName);
                }
            }

            EnumerableDataRowList<DataRow> enumerableRowCollection = new EnumerableDataRowList<DataRow>(dt.Rows);

            Func<DataRow, String> groupingFunction = GroupData;
            var groupedDataRow = enumerableRowCollection.GroupBy(groupingFunction);
            return groupedDataRow;
        }

        private static IEnumerable<IGrouping<string, DataRow>> GetGroupByData(List<GroupByInfo> groupby, DataTable dt)
        {

            columnName = new List<string>();
            if (groupby != null && groupby.Count > 0)
            {
                foreach (var item in groupby)
                {
                    columnName.Add(item.Column.DisplayName);
                }
            }

            EnumerableDataRowList<DataRow> enumerableRowCollection = new EnumerableDataRowList<DataRow>(dt.Rows);

            Func<DataRow, String> groupingFunction = GroupData;
            var groupedDataRow = enumerableRowCollection.GroupBy(groupingFunction);
            return groupedDataRow;
        }

        #endregion

        private static Type GetDataType(string dataType)
        {
            Type t = typeof(string);
            switch (dataType)
            {
                case "Double":
                case "Currency":
                case "Percent":
                case "Percent*100":
                    t = 0.000.GetType();
                    break;
                case "Integer":
                case "Days":
                case "Hours":
                case "Minutes":
                    t = typeof(int);
                    break;
                case "DateTime":
                case "Date":
                case "Time":
                    t = typeof(DateTime);
                    break;
                case "Boolean":
                //t = typeof(bool);
                //break;
                case "String":
                case "User":
                case "MultiUser":
                    t = typeof(string);
                    break;
                default:
                    break;
            }
            return t;
        }

        /// <summary>
        /// Returns where filter as a string expression from where list.
        /// </summary>
        /// <param name="whereList"></param>
        /// <returns></returns>
        public static String GetWhereExpression(List<Where> whereList)
        {
            StringBuilder queryString = new StringBuilder();
            if (whereList.Count > 0)
            {
                foreach (Where whr in whereList)
                {
                    if (whr.DataType.Equals("Number"))
                        queryString.Append(whr.StartWith + " " + whr.Column + " " + whr.OperatorType + " " + whr.Value + " ");
                    else if (whr.DataType.Equals("DateTime"))
                        queryString.Append(whr.StartWith + " " + whr.Column + " " + whr.OperatorType + " " + " " + "#" + whr.Value + "#" + " ");
                    else if (whr.DataType.Equals("DateTime"))
                        queryString.Append(whr.StartWith + " " + whr.Column + " " + whr.OperatorType + " " + "#" + whr.Value + "#" + " ");
                    else
                        queryString.Append(whr.StartWith + " " + whr.Column + " " + whr.OperatorType + " " + " '" + whr.Value + " '" + " ");
                }
            }

            return queryString.ToString();
        }

        public static string GetOperatorFromType(OperatorType type)
        {
            switch (type)
            {
                case OperatorType.NotEqual:
                    return "<>";
                case OperatorType.GreaterThan:
                    return ">";
                case OperatorType.GreaterThanEqualTo:
                    return ">=";
                case OperatorType.LessThan:
                    return "<";
                case OperatorType.LessThanEqualTo:
                    return "<=";
                case OperatorType.like:
                    return "LIKE";
                case OperatorType.MemberOf:
                    return "IN";
                default:
                    return "=";
            }
        }

        public static string GetOperatorDisplayFormatFromType(OperatorType type)
        {
            switch (type)
            {
                case OperatorType.NotEqual:
                    return "!=";
                case OperatorType.GreaterThan:
                    return ">";
                case OperatorType.GreaterThanEqualTo:
                    return ">=";
                case OperatorType.LessThan:
                    return "<";
                case OperatorType.LessThanEqualTo:
                    return "<=";
                case OperatorType.like:
                    return "like";
                case OperatorType.MemberOf:
                    return "member of";
                default:
                    return "=";
            }
        }

        /// <summary>
        /// Get Data From SPList
        /// It also includes lookup Ref ID if requires in joins
        /// </summary>
        /// <param name="list"></param>
        /// <param name="joins"></param>
        /// <returns></returns>
        public DataTable GetDataFromList(DataTable list, List<Joins> joins)
        {
            DataTable data = null;
            DataRow[] collection = null;
            collection = list.Select();
            data = collection.CopyToDataTable();

            if (joins == null || joins.Count <= 0 || data == null || data.Rows.Count <= 0)
                return data;

            List<string> columns = joins.Where(x => x.FirstTable == list.TableName && x.FirstColumn.IndexOf("_RefID") != -1).Select(x => x.FirstColumn.Replace(list.TableName + ".", string.Empty)).ToList();
            columns.AddRange(joins.Where(x => x.SecondTable == list.TableName && x.SecondColumn.IndexOf("_RefID") != -1).Select(x => x.SecondColumn.Replace(list.TableName + ".", string.Empty)));

            if (columns == null || columns.Count <= 0)
                return data;


            columns = columns.Distinct().ToList();
            foreach (string cln in columns)
            {
                if (!data.Columns.Contains(cln))
                {
                    data.Columns.Add(cln, typeof(int));
                }
            }


            columns = columns.Select(x => x.Replace("_RefID", string.Empty)).ToList();
            DataRow[] sRows = new DataRow[0];
            if (columns.Count > 1)
            {
                string lookup = null;
                foreach (DataRow item in collection)
                {
                    sRows = data.Select("ID=" + item["ID"]);
                    if (sRows.Length > 0)
                    {
                        foreach (string cln in columns)
                        {
                            lookup = Convert.ToString(item[cln]);
                            sRows[0][cln + "_RefID"] = 0;
                            if (lookup != null)
                            {
                                sRows[0][cln + "_RefID"] = lookup;
                            }
                        }
                    }
                }
            }
            else if (columns.Count == 1)
            {
                string lookup = null;
                foreach (DataRow item in collection)
                {
                    sRows = data.Select("ID=" + item["ID"]);
                    if (sRows.Length > 0)
                    {
                        lookup = Convert.ToString(item[columns[0]]);
                        sRows[0][columns[0] + "_RefID"] = 0;
                        if (lookup != null)
                        {
                            sRows[0][columns[0] + "_RefID"] = lookup;
                        }
                    }
                }
            }

            return data;
        }

        public ExpressionInfo GetExpressionInfo(string expression)
        {
            ExpressionInfo info = new ExpressionInfo();
            if (expression.Contains("Substring"))
            {
                Regex pattern = new Regex(@"(?<Table1>\w+).(?<Col1>\w+)\((?<DataType1>\w+)\)");
                Match match = pattern.Match(expression);

                info.Table1 = match.Groups["Table1"].Value;
                info.Column1 = match.Groups["Col1"].Value;
                info.DataType1 = match.Groups["DataType1"].Value;
                info.Operator = "Substring";

                string[] ExpressionArray = expression.Split(new string[] { "Substring" }, StringSplitOptions.None);
                string[] indexarray = ExpressionArray[1].Replace('(', ' ').Replace(')', ' ').Split(',');

                info.StartIndex = indexarray[0];
                info.Length = indexarray[1];
            }
            else if (expression.Contains("Month Year") || expression.Contains("Year Month") || expression.Contains("Year") || expression.Contains("Month"))
            {
                Regex pattern = new Regex(@"(?<Table1>\w+).(?<Col1>\w+)\((?<DataType1>\w+)\)");
                Match match = pattern.Match(expression);

                string[] str = expression.Split(new string[] { "-", "." }, StringSplitOptions.RemoveEmptyEntries);
                List<string> tablePattern = new List<string>(){ "openticket", "closedticket", "All" };
                if (str != null && str.Length > 0)
                {
                    if (tablePattern.Any(s => expression.ToUpper().Contains(s.ToUpper())))
                    {
                        info.Operator = str[0];
                        info.Table1 = str[1] + "-" + str[2];
                        info.Column1 = match.Groups["Col1"].Value;
                        info.DataType1 = match.Groups["DataType1"].Value;
                    }
                    else
                    {
                        info.Operator = str[0];
                        info.Table1 = str[1];
                        info.Column1 = match.Groups["Col1"].Value;
                        info.DataType1 = match.Groups["DataType1"].Value;
                    }
                }
            }
            else
            {
                Regex pattern = new Regex(@"(?<Table1>\w+).(?<Col1>\w+)\((?<DataType1>\w+)\) (?<Operator>[+-/*]|And|Or) (?<Table2>\w+).(?<Col2>\w+)\((?<DataType2>\w+)\)");
                Match match = pattern.Match(expression);

                info.Table1 = match.Groups["Table1"].Value;
                info.Column1 = match.Groups["Col1"].Value;
                info.DataType1 = match.Groups["DataType1"].Value;
                info.Operator = match.Groups["Operator"].Value;
                info.Table2 = match.Groups["Table2"].Value;
                info.Column2 = match.Groups["Col2"].Value;
                info.DataType2 = match.Groups["DataType2"].Value;
            }

            return info;
        }

        /// <summary>
        /// This method is used to update table name for each column in where clause if it is null or empty i.e. for old queries.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<WhereInfo> UpdateTableNameInWhereClause(DashboardQuery query)
        {
            List<WhereInfo> lstWhereClause = new List<WhereInfo>();

            if (query == null)
                return lstWhereClause;

            bool updateTableName = query.QueryInfo.WhereClauses != null && query.QueryInfo.WhereClauses.Count > 0 && query.QueryInfo.WhereClauses.Any(x => string.IsNullOrEmpty(x.TableName));
            if (!updateTableName)
                return lstWhereClause;

            bool isUnion = false;

            if (query.QueryInfo.JoinList != null && query.QueryInfo.JoinList.Count > 0)
                isUnion = query.QueryInfo.JoinList.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

            List<Utility.ColumnInfo> selectedCols = null;
            if (isUnion)
            {
                selectedCols = new List<Utility.ColumnInfo>();

                foreach (TableInfo table in query.QueryInfo.Tables)
                {
                    selectedCols.AddRange(table.Columns.Where(x => x.Selected && !x.Hidden).ToList());
                }
            }

            // Update table name
            foreach (WhereInfo wInfo in query.QueryInfo.WhereClauses)
            {
                if (!string.IsNullOrEmpty(wInfo.TableName))
                    continue;

                if (isUnion)
                    wInfo.TableName = selectedCols.Where(x => x.FieldName == wInfo.ColumnName).Select(y => y.TableName).ToString();
                else
                    wInfo.TableName = wInfo.ColumnName.Split('.')[0];
            }

            lstWhereClause = query.QueryInfo.WhereClauses;
            return lstWhereClause;
        }

        /// <summary>
        /// This method is used to get the standard type of a Field.
        /// </summary>
        /// <param name="spDataType"></param>
        /// <returns></returns>
        public static string GetStandardDataType(string dataType)
        {
            string standardType = "";
            dataType = dataType.Replace("System.", "");
            switch (dataType)
            {
                case "DateTime":
                    standardType = "DateTime";
                    break;
                case "Lookup":
                    standardType = "String";
                    break;
                case "Currency":
                    standardType = "Currency";
                    break;
                case "Text":
                    standardType = "String";
                    break;
                case "Note":
                    standardType = "String";
                    break;
                case "Boolean":
                    standardType = "Boolean";
                    break;
                case "Choice":
                    standardType = "String";
                    break;
                case "Counter":
                case "Int64":
                case "Int32":
                    standardType = "Integer";
                    break;
                case "User":
                    standardType = "User";
                    break;
                case "Number":
                case "Double":
                    standardType = "Double";
                    break;
                default:
                    standardType = "String";
                    break;
            }
            return standardType;
        }

        private static DataColumn[] GetColumnsWithTableName(DataTable table, string tableName)
        {
            List<DataColumn> dt1Columns = new List<DataColumn>();
            string expression = string.Empty;
            foreach (DataColumn column in table.Columns)
            {
                expression = string.Empty;
                if (!string.IsNullOrWhiteSpace(column.Expression))
                    expression = column.Expression.Replace("[", string.Format("[{0}.", tableName));

                if (column.ColumnName == DatabaseObjects.Columns.ModuleName &&
                    (tableName.ToLower() == "allopensmstickets" ||
                    tableName.ToLower() == "allopentickets" ||
                    tableName.ToLower() == "allclosedtickets" ||
                    tableName.ToLower() == "allclosedsmstickets" ||
                    tableName.ToLower() == "alltickets" ||
                    tableName.ToLower() == "allsmstickets"))
                {

                    dt1Columns.Add(new DataColumn(tableName + "." + column.ColumnName, column.DataType, expression, column.ColumnMapping));
                }
                else
                {
                    dt1Columns.Add(new DataColumn(tableName + "." + column.ColumnName, column.DataType, expression, column.ColumnMapping));
                }
            }

            return dt1Columns.ToArray();
        }

        #region Method to Get Missing Columns data in a dictionary
        /// <summary>
        /// This method is used to fetch column details which are missing in Module Columns/Request Lists
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Dictionary</returns>
        public Dictionary<string, List<string>> GetMissingModuleColumns(DashboardQuery query)
        {
            // List that will keep all the columns of module list
            List<string> moduleColumnsList = new List<string>();

            // Dictionary that will keep all the details of missing columns
            Dictionary<string, List<string>> dictMissingColumns = new Dictionary<string, List<string>>();

            if (query == null || query.QueryInfo.Tables == null || query.QueryInfo.Tables.Count == 0)
                return dictMissingColumns;

            #region check and find column details from whereClause if exist

            bool hasWhereClause = false;
            bool isUnion = false;
            List<string> columnsPresentInWhereClause = new List<string>();

            if (query.QueryInfo.WhereClauses != null && query.QueryInfo.WhereClauses.Count > 0)
                hasWhereClause = true;

            if (query.QueryInfo.JoinList != null && query.QueryInfo.JoinList.Count > 0)
                isUnion = query.QueryInfo.JoinList.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

            if (hasWhereClause && isUnion)
                columnsPresentInWhereClause = query.QueryInfo.WhereClauses.AsEnumerable().Where(y => !string.IsNullOrEmpty(y.TableName) && IsModuleTable(y.TableName)).Select(x => x.ColumnName).ToList();
            else if (hasWhereClause && !isUnion)
                columnsPresentInWhereClause = query.QueryInfo.WhereClauses.AsEnumerable().Where(y => !string.IsNullOrEmpty(y.TableName) && IsModuleTable(y.TableName)).Select(x => x.ColumnName.Split('.')[1]).ToList();

            #endregion check and find column details from whereClause if exist

            // Fetching details of miss-matched columns by comparing the selected columns with Module list
            foreach (TableInfo tInfo in query.QueryInfo.Tables)
            {
                // Check if current table is a Fact Table
                bool isFactTable = DashboardCache.DashboardFactTables(context).Any(x => x == tInfo.Name);
                bool isModuleTable = IsModuleTable(tInfo.Name);

                if (!isModuleTable)
                    continue;

                // Get all the fields of current table
                List<uGovernIT.FactTableField> tableFields = new List<uGovernIT.FactTableField>();

                if (isFactTable)
                    tableFields = DashboardCache.GetFactTableFields(context, tInfo.Name);
                else
                    tableFields = GetTableFields(tInfo.Name);

                // Get Module Name of the Fact Table
                string moduleName = GetModuleNameByTableName(tInfo.Name);

                if (!string.IsNullOrEmpty(moduleName))
                    moduleName = "Module: " + moduleName;
                else
                    moduleName = "Table: " + tInfo.Name;

                // Get a list of selected columns that are not available in Module list and add this list to a dictionary called "dictMissingColumns"
                List<string> missingColList = tInfo.Columns.AsEnumerable().Where(x => !x.IsExpression && !tableFields.Exists(y => y.FieldName == x.FieldName)).Select(z => z.FieldName).ToList();

                // Find missing columns from Expressions columns if available
                bool isExpressionColExist = tInfo.Columns.AsEnumerable().Any(x => x.IsExpression);

                if (isExpressionColExist)
                {
                    ExpressionInfo expressInfo = null;
                    List<string> expressionColumns = new List<string>();

                    tInfo.Columns.ForEach(x =>
                    {
                        if (x.IsExpression)
                        {
                            expressInfo = GetExpressionInfo(x.Expression);

                            if (expressInfo != null)
                            {
                                if (!string.IsNullOrEmpty(expressInfo.Column1))
                                    expressionColumns.Add(expressInfo.Column1);

                                if (!string.IsNullOrEmpty(expressInfo.Column2))
                                    expressionColumns.Add(expressInfo.Column2);
                            }
                        }
                    });

                    if (expressionColumns != null && expressionColumns.Count > 0)
                    {
                        expressionColumns = expressionColumns.Where(x => !tableFields.Exists(y => y.FieldName == x)).Select(z => z).ToList();

                        if (expressionColumns != null && expressionColumns.Count > 0)
                        {
                            missingColList.AddRange(expressionColumns);
                            missingColList.Select(x => x).Distinct().ToList();
                        }
                    }
                }

                // Remove below columns from missingColList on the basis of table name
                if (tInfo.Name == DatabaseObjects.Tables.PMMProjects && missingColList != null && missingColList.Count > 0)
                {
                    var result = missingColList.Where(x => x != DatabaseObjects.Columns.ProjectHealth && x != "GanttView");

                    if (result != null && result.Count() > 0)
                        missingColList = result.ToList();
                    else
                        missingColList.Clear();
                }
                else if ((tInfo.Name == DatabaseObjects.Tables.NPRRequest || tInfo.Name == DatabaseObjects.Tables.TSKProjects) && missingColList != null && missingColList.Count > 0)
                {
                    var result = missingColList.Where(x => x != "GanttView");

                    if (result != null && result.Count() > 0)
                        missingColList = result.ToList();
                    else
                        missingColList.Clear();
                }

                if (missingColList != null && missingColList.Count > 0 && !(missingColList.Count == 1 && missingColList[0].ToLower() == DatabaseObjects.Columns.Id.ToLower()))
                    dictMissingColumns.Add(moduleName, missingColList);

                // Get details of all module columns if the query has where clause i.e. it will be used to find missing columns from whereClause
                if (!hasWhereClause)
                    continue;

                // Get all the columns of the Fact Table in a list and add them to a common list called "moduleColumnsList"
                List<string> completeListOfModuleColumns = tableFields.Select(x => x.FieldName).ToList();

                if (completeListOfModuleColumns != null && completeListOfModuleColumns.Count > 0)
                    moduleColumnsList.AddRange(completeListOfModuleColumns);
            }

            // Return missing column dictionary if query doesn't has where clause or modulecolumns list is empty
            if (!hasWhereClause || moduleColumnsList == null || moduleColumnsList.Count == 0)
                return dictMissingColumns;

            // Fetching details of miss-matched columns by comparing columns in where clause(if exist) with columns in modulecolumns list
            moduleColumnsList = moduleColumnsList.Select(x => x).Distinct().ToList();
            columnsPresentInWhereClause = columnsPresentInWhereClause.Where(x => !moduleColumnsList.Exists(y => y == x)).ToList();

            if (columnsPresentInWhereClause == null || columnsPresentInWhereClause.Count == 0)
                return dictMissingColumns;

            // Remove columns from columnsPresentInWhereClause list which are already added to dictionary
            foreach (KeyValuePair<string, List<string>> item in dictMissingColumns)
            {
                columnsPresentInWhereClause = columnsPresentInWhereClause.Where(x => !item.Value.Exists(y => y == x)).ToList();
            }

            // Add columns to dictionary which are available in columnsPresentInWhereClause list
            if (columnsPresentInWhereClause.Count > 0)
                dictMissingColumns.Add("Where Clause", columnsPresentInWhereClause);

            return dictMissingColumns;
        }
        #endregion Method to Get Missing Columns data in a dictionary

        #region Method to Get Missing Columns data in DataTable from a Dictionary
        /// <summary>
        /// This method is used to create a DataTable from a dictionary having the details of missing columns in Module Columns/Request Lists
        /// </summary>
        /// <param name="dictMissingColumns"></param>
        /// <returns>DataTable</returns>
        public DataTable GetMissingModuleColumns(Dictionary<string, List<string>> dictMissingColumns)
        {
            DataTable dtMissingCol = new DataTable();
            dtMissingCol.Columns.Add("#");
            dtMissingCol.Columns.Add(DatabaseObjects.Columns.ColumnName);

            if (dictMissingColumns == null || dictMissingColumns.Count == 0)
                return dtMissingCol;

            foreach (KeyValuePair<string, List<string>> item in dictMissingColumns)
            {
                string columns = string.Empty;
                columns = string.Join(", ", item.Value.AsEnumerable().OrderBy(z => z));
                dtMissingCol.Rows.Add(item.Key, columns);
            }

            return dtMissingCol;
        }
        #endregion Method to Get Missing Columns data in DataTable from a Dictionary

        #region Method to Get Module Name on the Basis of Table Name
        /// <summary>
        /// Method to get Module Name on the basis of given Table Name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetModuleNameByTableName(string tableName)
        {
            string moduleName = string.Empty;

            if (string.IsNullOrEmpty(tableName))
                return moduleName;

            if (tableName.Contains('-'))
            {
                moduleName = tableName.Split('-')[0];
            }
            else
            {
                ModuleViewManager viewManager = new ModuleViewManager(context);
                List<UGITModule> lstModules = viewManager.Load(x => x.ModuleTable == tableName);

                if (lstModules == null || lstModules.Count == 0)
                    return moduleName;

                moduleName = lstModules[0].ModuleName;
            }
            return moduleName;
        }
        #endregion Method to Get Module Name on the Basis of Fact Table Name

        #region Method to Get the Missing Columns in a delimited String from a Dictionary
        /// <summary>
        /// Method to Get the Missing Columns in a delimited String from a Dictionary
        /// </summary>
        /// <param name="dictMissingColumns"></param>
        /// <returns></returns>
        public string GetMissingColInString(Dictionary<string, List<string>> dictMissingColumns)
        {
            string missingColumns = string.Empty;

            if (dictMissingColumns == null || !dictMissingColumns.Any(x => x.Value.Count > 0))
                return missingColumns;

            missingColumns = string.Join(";", dictMissingColumns.Select(x => x.Key + "=" + string.Join(",", x.Value)).ToArray());
            return missingColumns;
        }
        #endregion Method to Get the Missing Columns in a delimited String from a Dictionary

        #region Method to Get the Missing Columns in a Dictionary from a delimited String
        /// <summary>
        /// Method to Get the Missing Columns in a Dictionary from a delimited String
        /// </summary>
        /// <param name="missingColumns"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetMissingColInDictionary(string missingColumns)
        {
            Dictionary<string, List<string>> dictMissingColumns = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(missingColumns) || !missingColumns.Contains('='))
                return dictMissingColumns;

            var resultDict = missingColumns.Split(';').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1].Split(',').Select(y => y).ToList());

            if (resultDict == null || resultDict.Count == 0 || !resultDict.Any(x => x.Value.Count > 0))
                return dictMissingColumns;

            dictMissingColumns = resultDict;
            return dictMissingColumns;
        }
        #endregion Method to Get the Missing Columns in a Dictionary from a delimited String

        /// <summary>
        /// This method is used to check whether the current table is a module table or not
        /// </summary>
        /// <param name="tableName"></param>
        private bool IsModuleTable(string tableName)
        {
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            if (dt == null || dt.Rows.Count == 0)
                return false;

            string moduleName = GetModuleNameByTableName(tableName);
            DataRow[] moduleRow = dt.Select(string.Format("ModuleName='{0}'", moduleName));

            if (moduleRow == null || moduleRow.Length == 0)
                return false;

            return true;
        }

        /// <summary>
        /// This method is used to get all the fields of table as List of FactTableFields
        /// </summary>
        /// <param name="table"></param>
        /// <param name="spWeb"></param>
        /// <returns></returns>
        private List<uGovernIT.FactTableField> GetTableFields(string tableName)
        {
            List<uGovernIT.FactTableField> innerFields = null;
            DataColumnCollection fieldCollection = null;
            DataTable inputList = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            string[] essentialFields = new string[] { DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Author, DatabaseObjects.Columns.Editor, DatabaseObjects.Columns.Created, DatabaseObjects.Columns.Modified, DatabaseObjects.Columns.ContentType };

            if (inputList != null)
            {
                innerFields = new List<uGovernIT.FactTableField>();
                fieldCollection = inputList.Columns;

                foreach (DataColumn field in fieldCollection)
                {
                    if (!field.ReadOnly || Convert.ToString(field.DataType) == "Lookup" || essentialFields.Contains(field.ColumnName))
                    {
                        innerFields.Add(new uGovernIT.FactTableField(tableName, field.ColumnName, GetStandardDataType(Convert.ToString(field.DataType)), field.ColumnName));
                    }
                }
            }
            return innerFields;
        }

        public List<uGovernIT.FactTableField> GetColsListWithDataType(string queryTable, string typeFilter)
        {
            DataColumnCollection fieldCollection = null;
            List<uGovernIT.FactTableField> queryTableFields = new List<uGovernIT.FactTableField>();
            string[] essentialFields = new string[] { DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Author, DatabaseObjects.Columns.Editor, DatabaseObjects.Columns.Created, DatabaseObjects.Columns.Modified, DatabaseObjects.Columns.ContentType };

            if (queryTable != null && queryTable.Trim() != string.Empty)
            {
                queryTableFields = DashboardCache.GetFactTableFields(context, queryTable);
                if (queryTableFields == null)
                    queryTableFields = new List<uGovernIT.FactTableField>();

                if (queryTableFields != null && queryTableFields.Count > 0)
                {
                    foreach (uGovernIT.FactTableField fld in queryTableFields)
                    {
                        if (!string.IsNullOrEmpty(fld.FieldDisplayName) && fld.FieldDisplayName.EndsWith("$"))
                            fld.FieldDisplayName = Convert.ToString(fld.FieldDisplayName).Remove(Convert.ToString(fld.FieldDisplayName).Length - 1);
                        fld.DataType = fld.DataType.Replace("System.", "");
                    }
                }
                else
                {
                    string module = string.Empty, moduleTable = string.Empty;
                    module = queryTable.Split('-')[0];
                    ModuleViewManager moduleManager = new ModuleViewManager(context);
                    // moduleTable = moduleManager.GetModuleTableName(module);
                    moduleTable = moduleManager.GetModuleByTableName(module);
                    //string modulename = moduleManager.GetModuleByTableName(queryTable);


                    if (string.IsNullOrEmpty(moduleTable))
                    {
                        DataTable dtTables = GetTableDataManager.GetTableData("INFORMATION_SCHEMA.TABLES", $"Table_Name='{module}'");
                        if (dtTables != null && dtTables.Rows.Count > 0)
                            moduleTable = module;
                    }

                    queryTableFields = new List<uGovernIT.FactTableField>();

                    if (string.IsNullOrEmpty(moduleTable))
                        return queryTableFields;


                    //DataTable inputList = GetTableDataManager.GetTableData(queryTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    DataTable inputList = null;
                    TicketManager objTicketManager = new TicketManager(context);
                    UGITModule modname = moduleManager.GetByName(moduleTable);
                    if (modname != null)
                    {
                        List<ModuleColumn> lstModuleColumn = modname.List_ModuleColumns;
                        DataTable listFields = uGITDAL.GetTableStructure(modname.ModuleTable);
                        if (lstModuleColumn != null && lstModuleColumn.Count > 0)
                        {
                            foreach (DataColumn column in listFields.Columns)
                            {
                                if (lstModuleColumn.Exists(x => x != null && x.FieldName == column.ColumnName))
                                {
                                    ModuleColumn moduleColumn = lstModuleColumn.Where(x => x.FieldName == column.ColumnName).FirstOrDefault();
                                    queryTableFields.Add(new uGovernIT.FactTableField(queryTable, column.ColumnName, QueryHelperManager.GetStandardDataType(Convert.ToString(column.DataType)), moduleColumn.FieldDisplayName));
                                }
                            }
                        }
                    }

                    else
                    {
                        inputList = GetTableDataManager.GetTableData(moduleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                        if (inputList != null)
                        {
                            fieldCollection = inputList.Columns;

                            foreach (DataColumn field in fieldCollection)
                            {
                                if (!field.ReadOnly || (Convert.ToString(field.DataType) == "Lookup") || essentialFields.Contains(field.ColumnName))
                                {
                                    queryTableFields.Add(new uGovernIT.FactTableField(queryTable, field.ColumnName, GetStandardDataType(Convert.ToString(field.DataType)), field.ColumnName));
                                }
                            }
                        }
                    }
                }
            }
            return queryTableFields;
        }
    }

    internal class EnumerableDataRowList<T> : IEnumerable<T>, IEnumerable
    {
        IEnumerable dataRows;
        internal EnumerableDataRowList(IEnumerable items)
        {
            dataRows = items;
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (T dataRow in dataRows)
                yield return dataRow;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable<T> iEnumerable = this;
            return iEnumerable.GetEnumerator();
        }
    }
}
