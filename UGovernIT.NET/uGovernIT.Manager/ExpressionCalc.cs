using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Manager.Helper;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
 

namespace uGovernIT.Manager
{
    public class ExpressionCalc
    {
        public string FactTableName { get; private set; }
        public DataTable FactTable { get; private set; }
        private readonly ApplicationContext applicationContext;
        private readonly ConfigurationVariableManager configurationVariableHelper;

        public ExpressionCalc(string factTableName, ApplicationContext context)
        {
            applicationContext = context;
            configurationVariableHelper = new ConfigurationVariableManager(applicationContext);

            var table = DashboardCache.GetCachedDashboardData(applicationContext, factTableName);

            if (table == null) return;

            FactTableName = factTableName;
            FactTable = table.Copy();

            if (!FactTable.Columns.Contains("today"))
            {
                FactTable.Columns.Add("today", typeof(DateTime), $"'{System.DateTime.Now.ToLongDateString()}'");
            }

            if (!FactTable.Columns.Contains("me"))
            {
                FactTable.Columns.Add("me", typeof(string),
                    $"'{applicationContext.CurrentUser.Name.Replace("'", "''")}'"); // Handle names like Peter O'Toole
            }
        }

        public  string GetParsedDateExpression(string filter)
        {
            if (filter == null || filter.Trim() == string.Empty)
            {
                return filter;
            }

            var ftTable = new DataTable();
            ftTable.Columns.Add("Expression", typeof(string));

            var row = ftTable.NewRow();
            ftTable.Rows.Add(row);

            var tokenRegx = "f:[a-zA-Z]*\\([a-zA-Z0-9,~'\\/:\\]\\[# ]*\\)";
            var matchedTokens = Regex.Matches(filter, tokenRegx, RegexOptions.IgnoreCase);

            foreach (Match m in matchedTokens)
            {
                var func = m.ToString().Replace("f:", string.Empty);
                var fDetail = func.Split('(');
                var funcName = fDetail[0];
                var arguments = new List<string>();

                if (fDetail.Length > 1)
                {
                    arguments = UGITUtility.ConvertStringToList(fDetail[1].Replace(")", string.Empty), new string[] { ",", "~" });
                    for (var i = 0; i < arguments.Count; i++)
                    {
                        arguments[i] = arguments[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
                    }
                }

                switch (funcName.Trim().ToLower())
                {
                    case "adddays":
                        //var colnName3 = Guid.NewGuid().ToString();
                        var date5 = DateTime.MinValue;
                        if (arguments.Count > 0)
                        {
                            arguments[0] = arguments[0].Replace("Today", Convert.ToString(DateTime.Now.Date, CultureInfo.InvariantCulture));
                            DateTime.TryParse(arguments[0], out date5);
                        }
                        if (date5 != DateTime.MinValue)
                        {
                            int.TryParse(arguments[1], out var addDays);

                            //Add one more day to exactly add days
                            if (addDays < 0)
                                addDays = addDays - 1;
                            else
                                addDays = addDays + 1;

                            var dates = uHelper.GetEndDateByWorkingDays(applicationContext, date5, addDays);

                            return Regex.Replace(filter, tokenRegx, dates[1].ToString("dd-MM-yyy"));
                        }
                        else
                        {
                            return string.Empty;
                        }

                    default:
                        break;
                }
            }
            return filter;
        }

        public ExpressionCalc(ApplicationContext context)
        {
            applicationContext = context;
            configurationVariableHelper = new ConfigurationVariableManager(applicationContext);
        }

        public ExpressionCalc(DataTable factTable, bool keepReference)
        {
            if (factTable != null)
            {
                if (keepReference)
                    FactTable = factTable;
                else
                    FactTable = factTable.Copy();

                if (!FactTable.Columns.Contains("today"))
                {
                    FactTable.Columns.Add("today", typeof(DateTime), string.Format("'{0}'", System.DateTime.Now.ToLongDateString()));
                }
                if (!FactTable.Columns.Contains("me"))
                {
                    FactTable.Columns.Add("me", typeof(string), string.Format("'{0}'", "Administrator"/*SPContext.Current.Web.CurrentUser.Name*/));
                }
            }
        }

        public string GetKPIUrl(long dashboardID, DashboardPanelLink link)
        {
            string url = string.Empty;

            if (link == null)
            {
                return url;
            }

            if (!link.DefaultLink && link.LinkUrl != string.Empty)
            {
                url = UGITUtility.GetAbsoluteURL(string.Format("{2}&dID={0}&kID={1}", dashboardID, link.LinkID, link.LinkUrl));
            }
            else
            {
                url = UGITUtility.GetAbsoluteURL(string.Format("{2}&dID={0}&kID={1}", dashboardID, link.LinkID, configurationVariableHelper.GetValue("FilterTicketsPageUrl")));
            }


            return url;
        }

        public string GetDateRangeClause(string field, string dateView)
        {
            if (field == null || field.Trim() == string.Empty || dateView == string.Empty)
            {
                return string.Empty;
            }

            var whereClause = string.Empty;
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MinValue;
            //var range = string.Empty;

            //uHelper.GetStartEndDateFromDateView(dateView, ref startDate, ref endDate, ref range);
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                endDate = endDate.AddDays(1);
                whereClause = string.Format("([{0}] >= #{1:MM/dd/yyyy}# AND [{0}] < #{2:MM/dd/yyyy}#)", field,
                    startDate, endDate);
            }

            return whereClause;
        }

        public string ResolveFunctions(ApplicationContext context,string filter)
        {
            if (filter == null || filter.Trim() == string.Empty)
            {
                return filter;
            }

            string tokenRegx = "f:[a-zA-Z]*\\([a-zA-Z0-9,'\\]\\[# ]*\\)";
            MatchCollection matchedTokens = Regex.Matches(filter, tokenRegx, RegexOptions.IgnoreCase);

            foreach (Match m in matchedTokens)
            {
                string func = m.ToString().Replace("f:", string.Empty);
                string[] fDetail = func.Split('(');
                string funcName = fDetail[0];
                List<string> arguments = new List<string>();
                if (fDetail.Length > 1)
                {
                    arguments = UGITUtility.ConvertStringToList(fDetail[1].Replace(")", string.Empty), new string[] { "," });
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        arguments[i] = arguments[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
                    }
                }

                switch (funcName.Trim().ToLower())
                {
                    case "daysdiff":
                        {
                            string colnName1 = Guid.NewGuid().ToString();
                            FactTable.Columns.Add(colnName1, typeof(double));
                            bool isWorkingDays = false;
                            if (arguments.Count > 2 && arguments[2].Trim().ToLower() == "1")
                            {
                                isWorkingDays = true;
                            }

                            if (isWorkingDays)
                            {
                                FactTable.Select(string.Format("[{0}] is not null and [{1}] is not null", arguments[0], arguments[1])).ToList().ForEach(x => x.SetField<double>(colnName1, uHelper.GetTotalWorkingDaysBetween(context,x.Field<DateTime>(arguments[1]), x.Field<DateTime>(arguments[0]))));
                            }
                            else
                            {
                                FactTable.Select().ToList().ForEach(x => x.SetField<double>(colnName1, uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(x.Field<DateTime>(arguments[1])), Convert.ToDateTime(x.Field<DateTime>(arguments[0])))));
                            }
                            filter = filter.Replace(m.ToString(), string.Format(" [{0}] ", colnName1));
                        }
                        break;
                    case "yearsdiff":
                        {
                            string colnName2 = Guid.NewGuid().ToString();
                            FactTable.Columns.Add(colnName2, typeof(double));
                            FactTable.Select(string.Format("[{0}] is not null and [{1}] is not null", arguments[0], arguments[1])).ToList().ForEach(x => x.SetField<double>(colnName2, (x.Field<DateTime>(arguments[1]).Year - x.Field<DateTime>(arguments[0]).Year)));
                            filter = filter.Replace(m.ToString(), string.Format(" [{0}] ", colnName2));
                        }
                        break;
                    case "matchuser":
                        {
                            string colnName2 = Guid.NewGuid().ToString();
                            FactTable.Columns.Add(colnName2, typeof(double));
                            List<string> likeExps = new List<string>();
                            if (arguments.Count == 2 && FactTable.Columns.Contains(arguments[0]))
                            {
                                DataColumn specifiedColumn = FactTable.Columns[arguments[0]];

                                if (arguments[1].ToLower() == "me")
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], context.CurrentUser.Name.Replace("'", "''")));
                                    List<Role> groups = context.UserManager.GetUserRoles(context.CurrentUser.Id);
                                    foreach (Role group in groups)
                                    {
                                        likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], group.Name.Replace("'", "''")));
                                    }
                                }
                                else if (arguments[1].ToLower() == "meonly")
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], context.CurrentUser.Name.Replace("'", "''")));
                                }
                                else
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], arguments[1].Replace("'", "''")));
                                }
                            }
                            filter = filter.Replace(m.ToString(), string.Format(" ({0}) ", string.Join("OR", likeExps.ToArray())));
                        }
                        break;
                    case "matchcase":
                        {
                            string colnName2 = Guid.NewGuid().ToString();
                            FactTable.Columns.Add(colnName2, typeof(double));
                            List<string> likeExps = new List<string>();
                            if (arguments.Count == 2 && FactTable.Columns.Contains(arguments[0]))
                            {
                                DataColumn specifiedColumn = FactTable.Columns[arguments[0]];

                                if (arguments[1].ToLower() == "me")
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], context.CurrentUser.Name.Replace("'", "''")));
                                }
                                else if (arguments[1].ToLower() == "meonly")
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], context.CurrentUser.Name.Replace("'", "''")));
                                }
                                else
                                {
                                    likeExps.Add(string.Format(" [{0}] Like '%{1}%' ", arguments[0], arguments[1].Replace("'", "''")));
                                }
                            }
                            filter = filter.Replace(m.ToString(), string.Format(" ({0}) ", string.Join("OR", likeExps.ToArray())));
                        }
                        break;
                    default:
                        break;
                }
            }

            var wildTokenRegx = "\\[[a-zA-Z0-9]*\\]";
            matchedTokens = Regex.Matches(filter, wildTokenRegx, RegexOptions.IgnoreCase);

            foreach (Match m in matchedTokens)
            {
                switch (m.ToString().ToLower())
                {
                    case "[me]":
                        //filter = filter.Replace(m.ToString(), "administrator" /*SPContext.Current.Web.CurrentUser.Name*/);
                        filter = filter.Replace(m.ToString(), context.CurrentUser.UserName);
                        break;
                    case "[today]":
                        filter = filter.Replace(m.ToString(), $"{DateTime.Now:dd/MM/yyyy}");
                        break;
                }
            }

            // filter = filter.Replace("[me]", SPContext.Current.Web.CurrentUser.Name);
            // filter = filter.Replace("[today]", DateTime.Now.ToString("dd/MM/yyyy"));
            return filter;
        }

        public static string ExecuteFunctions(ApplicationContext pContext, string filter, bool businessDays = true)
        {
            if (filter == null || filter.Trim() == string.Empty)
            {
                return filter;
            }

            DataTable ftTable = new DataTable();
            ftTable.Columns.Add("Expression", typeof(string));

            DataRow row = ftTable.NewRow();
            ftTable.Rows.Add(row);

            //string tokenRegx = "f:[a-zA-Z]*\\([a-zA-Z0-9,\\+\\-'\\/:\\]\\[# ]*\\)";
            string tokenRegx = "f:[a-zA-Z]*\\([a-zA-Z0-9~,\\+\\-'\\/:\\]\\[# ]*\\)";
            MatchCollection matchedTokens = Regex.Matches(filter, tokenRegx, RegexOptions.IgnoreCase);
            foreach (Match m in matchedTokens)
            {

                string func = m.ToString().Replace("f:", string.Empty);
                string[] fDetail = func.Split('(');
                string funcName = fDetail[0];
                List<string> arguments = new List<string>();
                if (fDetail.Length > 1)
                {
                    // Added below replace to ", " with ";", as fDetails is getting value in format (Jan 01, 2000, +1) & split is splitting as Jan 01/2000/+1 & No. of days are added to year instead of days. (BTS-21-000674)
                    //arguments = UGITUtility.ConvertStringToList(fDetail[1].Replace(")", string.Empty), new string[] { "," });
                    arguments = UGITUtility.ConvertStringToList(fDetail[1].Replace(", ", ";").Replace(")", string.Empty), new string[] { ",", "~" });
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        arguments[i] = arguments[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty).Replace(";", ", ");
                    }
                }

                switch (funcName.Trim().ToLower())
                {
                    case "adddays":
                        string colnName3 = Guid.NewGuid().ToString();
                        DateTime date5 = DateTime.MinValue;
                        if (arguments != null && arguments.Count > 0)
                        {
                            if (arguments[0].IndexOf("DayOfMonth") != -1)
                            {
                                DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                                arguments[0] = arguments[0].Replace("DayOfMonth", Convert.ToString(firstDate));
                            }
                            else
                            {
                                arguments[0] = arguments[0].Replace("Today", Convert.ToString(DateTime.Now.Date));
                                DateTime.TryParse(arguments[0], out date5);
                            }
                        }

                        if (date5 != DateTime.MinValue)
                        {
                            int addDays = 0;
                            if (arguments.Count >= 2)
                            {
                                //if user not specified any value then keep date as is
                                if (!int.TryParse(arguments[1].Replace(")", " ").Trim(), out addDays))
                                {
                                    return date5.ToString("yyyy-MM-ddThh:mm:ss");
                                }
                            }

                            if (businessDays)
                            {
                                //Add one more day to exactly add days
                                if (addDays < 0)
                                {
                                    addDays = addDays - 1;
                                    List<DateTime> previousDates = uHelper.GetPreviousWorkingDates(pContext, date5, addDays);
                                    if (previousDates != null)
                                        return previousDates.Last().ToString("yyyy-MM-ddThh:mm:ss");
                                    else
                                        return string.Empty;
                                }
                                else
                                {
                                    addDays = addDays + 1;
                                    DateTime[] dates = uHelper.GetEndDateByWorkingDays(pContext, date5, addDays);
                                    return dates[1].ToString("yyyy-MM-ddThh:mm:ss");
                                }
                            }
                            else
                            {
                                string timeUnit = "Days";
                                int frequency = UGITUtility.StringToInt(arguments[1].Replace("(", string.Empty).Replace(")", string.Empty).Trim());
                                if (arguments.Count == 3)
                                    timeUnit = arguments[2].Replace(")", string.Empty).Trim();
                                switch (timeUnit)
                                {
                                    case "Months":
                                        date5 = date5.AddMonths(frequency);
                                        break;
                                    case "Weeks":
                                        date5 = date5.AddDays(frequency * 7);
                                        break;
                                    case "Days":
                                        date5 = date5.AddDays(frequency);
                                        break;
                                    default:
                                        break;
                                }
                                return date5.ToString("yyyy-MM-ddThh:mm:ss");
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }

                    default:
                        break;
                }
            }

            string wildTokenRegx = "\\[[a-zA-Z0-9]*\\]";
            matchedTokens = Regex.Matches(filter, wildTokenRegx, RegexOptions.IgnoreCase);
            foreach (Match m in matchedTokens)
            {
                if (m.ToString().ToLower() == "[me]")
                {
                    filter = filter.Replace(m.ToString(), pContext.CurrentUser.Name);
                }
                else if (m.ToString().ToLower() == "[today]")
                {
                    filter = filter.Replace(m.ToString(), string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy")));
                }
            }
            return filter;
        }

        public static string GetParsedDateExpression(ApplicationContext context, string filter)
        {
            if (filter == null || filter.Trim() == string.Empty)
            {
                return filter;
            }

            DataTable ftTable = new DataTable();
            ftTable.Columns.Add("Expression", typeof(string));

            DataRow row = ftTable.NewRow();
            ftTable.Rows.Add(row);

            string tokenRegx = "f:[a-zA-Z]*\\([a-zA-Z0-9,'\\/:\\]\\[# ]*\\)";
            MatchCollection matchedTokens = Regex.Matches(filter, tokenRegx, RegexOptions.IgnoreCase);
            foreach (Match m in matchedTokens)
            {

                string func = m.ToString().Replace("f:", string.Empty);
                string[] fDetail = func.Split('(');
                string funcName = fDetail[0];
                List<string> arguments = new List<string>();
                if (fDetail.Length > 1)
                {
                    arguments = UGITUtility.ConvertStringToList(fDetail[1].Replace(")", string.Empty), new string[] { "," });
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        arguments[i] = arguments[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
                    }
                }

                switch (funcName.Trim().ToLower())
                {
                    case "adddays":
                        string colnName3 = Guid.NewGuid().ToString();
                        DateTime date5 = DateTime.MinValue;
                        if (arguments != null && arguments.Count > 0)
                        {
                            arguments[0] = arguments[0].Replace("Today", Convert.ToString(DateTime.Now.Date));
                            DateTime.TryParse(arguments[0], out date5);
                        }
                        if (date5 != DateTime.MinValue)
                        {
                            int addDays = 0;
                            int.TryParse(arguments[1], out addDays);
                            //Add one more day to exactly add days
                            if (addDays < 0)
                                addDays = addDays - 1;
                            else
                                addDays = addDays + 1;

                            DateTime[] dates = uHelper.GetEndDateByWorkingDays(context, date5, addDays);
                            return Regex.Replace(filter, tokenRegx, dates[1].ToString("dd-MM-yyy"));
                        }
                        else
                        {
                            return string.Empty;
                        }

                    default:
                        break;
                }
            }


            return filter;
        }

        public double Aggragate(string function, string expression, string filter, string dateRangeFilter, string globalfilter)
        {
            return Aggragate(function, expression, filter, dateRangeFilter, false, globalfilter);
        }

        public double Aggragate(string function, string expression, string filter, string dateRangeFilter, bool findPct, string globalfilter)
        {
            if (FactTable == null || FactTable.Rows.Count <= 0)
            {
                return 0;
            }

            //Resolve custom funtions in filter and expression
            filter = ResolveFunctions(applicationContext,filter);
            expression = ResolveFunctions(applicationContext,expression);

            string colnName = Guid.NewGuid().ToString().Replace("-", string.Empty);
            Type colnType = typeof(double);
            string col = expression.Replace("[", " ").Replace("]", " ").Trim();

            if (FactTable.Columns.Contains(col))
            {
                colnType = FactTable.Columns[col].DataType;
            }

            DataTable table = FactTable;
            DashboardHelper chartHelper = new DashboardHelper();
            table = chartHelper.ApplyGlobalFilter(table, globalfilter);

            //if aggragate function is empty then filter out data first
            if (string.IsNullOrEmpty(function))
            {
                DataRow[] filteredRow = FactTable.Select(filter);
                if (filteredRow.Length > 0)
                {
                    table = filteredRow.CopyToDataTable();
                }
                else
                {
                    table = FactTable.Clone();
                }
            }

            //Apply date range filter if exist
            if (dateRangeFilter != null && dateRangeFilter.Trim() != string.Empty)
            {
                DataRow[] filteredRows = FactTable.Select(dateRangeFilter);
                if (filteredRows.Length > 0)
                {
                    table = filteredRows.CopyToDataTable();
                }
                else
                {
                    table = FactTable.Clone();
                }
            }

            //Add column to calculate express of aggragate expression
            table.Columns.Add(colnName, colnType);
            table.Columns[colnName].Expression = expression;

            double result = 0;
            try
            {
                if (table != null && table.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(function))
                    {
                        if (findPct)
                        {
                            double.TryParse(Convert.ToString(table.Compute(string.Format("{0}([{1}])", function, colnName), filter)), out result);
                            double total = 0;
                            double.TryParse(Convert.ToString(table.Compute(string.Format("{0}([{1}])", function, colnName), string.Empty)), out total);
                            if (total > 0)
                            {
                                result = (result * 100) / total;
                            }
                            else
                            {
                                result = 0;
                            }
                        }
                        else
                        {
                            double.TryParse(Convert.ToString(table.Compute(string.Format("{0}([{1}])", function, colnName), filter)), out result);
                        }
                    }
                    else
                    {
                        if (findPct)
                        {
                            double.TryParse(Convert.ToString(table.Rows[0][colnName]), out result);
                            result = result * 100;
                        }
                        else
                        {
                            double.TryParse(Convert.ToString(table.Rows[0][colnName]), out result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error parsing expression " + expression + " for dashboard");
            }

            result = Math.Round(result, 0);
            if (double.IsNaN(result))
                result = 0; // to prevent crashes when this goes to the denominator

            return result;
        }

        public string past6month(string dd)
        {
            return string.Empty;
        }

        public bool EvalLogicalExp(string expression)
        {
            expression = expression.Replace("!=", "<>");
            bool result = false;
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            result = UGITUtility.StringToBoolean(row["expression"]);
            return result;
        }

        public DataTable Select(string filter, string orderBy)
        {
            if (FactTable == null || FactTable.Rows.Count <= 0)
            {
                return null;
            }
            //Resolves custom functions in filter
            filter = ResolveFunctions(applicationContext, filter);

            DataTable resultedTable = null;
            DataRow[] rows = FactTable.Select(filter, orderBy);
            resultedTable = FactTable.Clone();
            if (rows.Length > 0)
            {
                resultedTable = rows.CopyToDataTable();
            }
            return resultedTable;
        }

        /// <summary>
        /// Evaluate all eval[x] format within string.  like Budget $eval[20/100]k
        /// </summary>
        /// <param name="evalExp"></param>
        /// <returns></returns>
        public string EvaluateEvalFunt(string evalExp)
        {
            string tokenRegx = "eval\\[[0-9.-/*+]*\\]";
            string exp = evalExp;
            MatchCollection matchedTokens = Regex.Matches(evalExp, tokenRegx, RegexOptions.IgnoreCase);
            string eval = string.Empty;
            foreach (Match m in matchedTokens)
            {
                eval = m.ToString().Replace("eval[", string.Empty).Replace("]", string.Empty);
                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(double), eval);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                double result = 0;
                double.TryParse(Convert.ToString(row["expression"]), out result);
                result = Math.Round(result, 0);
                exp = exp.Replace(m.ToString(), result.ToString());
            }
            return exp;
        }

        public static string GetKPIUrl(ApplicationContext context, long dashboardID, DashboardPanelLink link)
        {
            string url = string.Empty;

            if (link == null)
            {
                return url;
            }

            if (!link.DefaultLink && link.LinkUrl != string.Empty)
            {
                url = UGITUtility.GetAbsoluteURL(string.Format("{2}&dID={0}&kID={1}", dashboardID, link.LinkID, link.LinkUrl));
            }
            else
            {
                ConfigurationVariableManager objCVManager = new ConfigurationVariableManager(context);
                url = UGITUtility.GetAbsoluteURL(string.Format("{2}&dID={0}&kID={1}", dashboardID, link.LinkID, objCVManager.GetValue("FilterTicketsPageUrl")));
            }


            return url;
        }

        public DataTable GetFilteredData(DashboardPanelLink kpi)
        {
            string dateRangeFilter = GetDateRangeClause(kpi.DateFilterStartField, kpi.DateFilterDefaultView);
            string filter = kpi.Filter;
            if (FactTable == null)
            {
                return null;
            }

            filter = ResolveFunctions(applicationContext, filter);
            DataView view = FactTable.DefaultView;
            try
            {
                if (dateRangeFilter != null && dateRangeFilter.Trim() != string.Empty)
                {
                    view.RowFilter = string.Format("{0} AND ({1})", filter, dateRangeFilter);
                }
                else
                {
                    view.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error evaluating KPI " + kpi.Title);
            }
            return view.ToTable();
        }

        public static DataTable RemoveExpressionTempColumns(DataTable table)
        {
            if (table == null)
                return table;

            if (table.Columns.Contains("today"))
            {
                table.Columns.Remove(table.Columns["today"]);
            }
            if (table.Columns.Contains("me"))
            {
                table.Columns.Remove(table.Columns["me"]);
            }

            return table;
        }

        public void ApplyFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return;
            DashboardHelper dHelper = new DashboardHelper();
            FactTable = dHelper.ApplyGlobalFilter(FactTable, filter);
        }
    }


   


    [Serializable]
    public class FormulaExpression
    {
        public DataRow Filter;
        public string Variable;
        public string Operator;
        public string Value;
        public string AppendWith;
    }

}
