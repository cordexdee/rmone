using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager.Helper
{
    public class DashboardHelper
    {        
        public static string GetGlobalFilter(ApplicationContext context,string filterString, DashboardPanelView view, DataTable data = null)
        {
            Dictionary<string, string> filters = GetGlobalFilterList(context,filterString, view, data);
            return string.Join(" And ", filters.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Value).ToArray());
        }

        public static Dictionary<string, string> GetGlobalFilterList(ApplicationContext context,string filterString, DashboardPanelView view, DataTable data = null)
        {
            Dictionary<string, string> filterList = new Dictionary<string, string>();

            //if (view == null)
            //    return filterList;

            List<DashboardFilterProperty> viewfilters = new List<DashboardFilterProperty>();
            if (view != null && ( view.ViewProperty is IndivisibleDashboardsView || view.ViewProperty is SuperDashboardsView || view.ViewProperty is CommonDashboardsView ))
            {
                if (view.ViewProperty is IndivisibleDashboardsView)
                {
                    IndivisibleDashboardsView indiView = view.ViewProperty as IndivisibleDashboardsView;
                    viewfilters = indiView.GlobalFilers;
                }
                else if (view.ViewProperty is CommonDashboardsView)
                {
                    CommonDashboardsView indiView = view.ViewProperty as CommonDashboardsView;
                    viewfilters = indiView.GlobalFilers;
                }
                else
                {
                    SuperDashboardsView sView = view.ViewProperty as SuperDashboardsView;
                    viewfilters = sView.GlobalFilers;
                }
            }

            //create where clause using global filter of chart
            if (viewfilters.Count <= 0)
                return filterList;

            
            Dictionary<string, string> filters = new Dictionary<string, string>();
            string externalFilter = string.Empty;

            //get filter data from user input
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = Uri.UnescapeDataString(filterString);

                string[] filterOpts = UGITUtility.SplitString(filterString, ";*;");
                if (filterOpts.Length > 0)
                {
                    filterString = Uri.UnescapeDataString(filterOpts[0]);
                    if (filterOpts.Length > 1)
                    {
                        externalFilter = Uri.UnescapeDataString(filterOpts[1]);
                    }
                }
                filterString = filterString.Replace("~", "=");
                filters = UGITUtility.GetCustomProperties(filterString, ";#");
            }

            //get filter from hidden filters
            if (viewfilters != null && viewfilters.Count > 0 && viewfilters.Exists(x => x.Hidden))
            {
                if (filters == null)
                    filters = new Dictionary<string, string>();

                List<DashboardFilterProperty> fts = viewfilters.Where(x => x.Hidden).ToList();
                foreach (DashboardFilterProperty item in fts)
                {
                    if (item.DefaultValues != null && item.DefaultValues.Count > 0
                        && !filters.ContainsKey(item.ID.ToString()))
                    {
                        filters.Add(item.ID.ToString(), string.Join(Constants.Separator, item.DefaultValues));
                    }
                }
            }

            if (filters == null || filters.Count == 0)
                return filterList;


            //return empty when not filter comes in parameter
            if (filters == null || filters.Count <= 0)
            {
                return filterList;
            }

            StringBuilder query = new StringBuilder();
            //Loop all global filter to create formula expression
            foreach (DashboardFilterProperty filterP in viewfilters)
            {
                string filterKey = filterP.ID.ToString();
                if (!filters.ContainsKey(filterKey))
                {
                    filterList.Add(filterKey, string.Empty);
                    continue;
                }

                if (filters[filterKey] == null || filters[filterKey].Trim() == string.Empty)
                {
                    filterList.Add(filterKey, string.Empty);
                    continue;
                }

                DataTable dasbhoardTable = DashboardCache.GetCachedDashboardData(context,filterP.ListName);
                if (dasbhoardTable == null || dasbhoardTable.Rows.Count <= 0)
                {
                    filterList.Add(filterKey, string.Empty);
                    continue;
                }

                if (!dasbhoardTable.Columns.Contains(filterP.ColumnName))
                {
                    filterList.Add(filterKey, string.Empty);
                    continue;
                }

                query = new StringBuilder();


                DataColumn column = dasbhoardTable.Columns[filterP.ColumnName];
                if (data != null && !data.Columns.Contains(column.ColumnName))
                {
                    filterList.Add(filterKey, string.Empty);
                    continue;
                }


                string[] values = filters[filterKey].Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length > 1)
                    query.Append(" (");
                if (column.DataType == typeof(String))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i != 0)
                        {
                            query.Append(" OR ");
                        }
                        if (values[i] == "[Me]")
                            values[i] = GetUserPropertyValueByColumnName(context, context.CurrentUser.Id, column.ColumnName, filterP.ListName);
                        query.AppendFormat("{0} = '{1}' ", column.ColumnName, values[i]);

                    }

                }
                else if (column.DataType == typeof(DateTime))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i != 0)
                        {
                            query.Append(" OR ");
                        }
                        string[] dateRange = values[i].Split(new string[] { "to" }, StringSplitOptions.None);
                        if (dateRange.Length == 1)
                        {
                            string expression = "(";
                            DateTime date = Convert.ToDateTime(values[i]);
                            expression += string.Format("{0}>=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                            expression += " AND ";
                            expression += string.Format("{0}<=#{1}# ", column.ColumnName, date.AddMonths(1).ToString("MM/dd/yyyy"));
                            // query.AppendFormat("{0}=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                            expression += ")";
                            query.AppendFormat("{0} ", expression);
                        }
                        else
                        {
                            string expression = "(";
                            if (dateRange[0].Trim().Length == 7)
                            {
                                DateTime date = Convert.ToDateTime(dateRange[0]);
                                expression += string.Format("{0}>=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                                if (dateRange[1].Trim().Length == 7)
                                {
                                    expression += " AND ";
                                }
                            }
                            if (dateRange[1].Trim().Length == 7)
                            {
                                DateTime date = Convert.ToDateTime(dateRange[1]);
                                expression += string.Format("{0}<=#{1}# ", column.ColumnName, date.ToString("MM/dd/yyyy"));
                            }
                            expression += ")";
                            query.AppendFormat("{0} ", expression);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i != 0)
                        {
                            query.Append(" OR ");
                        }
                        query.AppendFormat("{0}={1} ", column.ColumnName, values[i]);
                    }
                }
                if (values.Length > 1)
                    query.Append(") ");

                filterList.Add(filterKey, query.ToString());
            }

            //Append External query with filter
            if (externalFilter != null && externalFilter.Trim() != string.Empty)
            {
                filterList.Add("ExternalQuery", string.Format("({0})", externalFilter));
            }
            return filterList;
        }

        public DataTable ApplyGlobalFilter(DataTable inputTable, string globalfilter)
        {
            //return if input table is null or empty
            if (inputTable == null)
            {
                return null;
            }

            if (inputTable.Rows.Count <= 0)
            {
                return inputTable.Clone();
            }

            // return if there is no global filter
            if (globalfilter == null || globalfilter.Trim() == string.Empty)
            {
                return inputTable.Copy();
            }

            //Apply global filter on input table
            try
            {
                DataRow[] filteredRows = inputTable.Copy().Select(globalfilter);
                if (filteredRows.Length > 0)
                {
                    return filteredRows.CopyToDataTable();
                }
                else
                {
                    return inputTable.Clone();
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return inputTable.Clone();
            }
        }

        public static bool IsFilterFieldRelatedToUser(ApplicationContext context, string columnName,string tableName)
        {
            DataTable dt = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (!dt.Columns.Contains(columnName))
                return false;

            bool isExist = false;
            
            DataColumn field = dt.Columns[columnName];
            FieldConfigurationManager objfdManager = new FieldConfigurationManager(context);
            FieldConfiguration objfield = objfdManager.GetFieldByFieldName(field.ColumnName);
            if (objfield!=null && objfield.Datatype.ToLower() == "lookup")
            {
                
                DataTable lList = GetTableDataManager.GetTableData(objfield.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (lList != null)
                {
                    List<string> urList = new List<string>();
                    urList.Add(DatabaseObjects.Tables.Company);
                    urList.Add(DatabaseObjects.Tables.CompanyDivisions);
                    urList.Add(DatabaseObjects.Tables.Department);
                    urList.Add(DatabaseObjects.Tables.FunctionalAreas);
                    if (urList.Exists(x => x == objfield.FieldName))
                    {
                        isExist = true;
                    }
                }
            }
            else if (objfield != null && objfield.Datatype.ToLower() == "userfield")
            {
                isExist = true;
            }
            return isExist;
        }

        public static string GetUserPropertyValueByColumnName(ApplicationContext context, string currentUser, string columnName,string tableName)
        {
            DataTable dt = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (!IsFilterFieldRelatedToUser(context, columnName,tableName) || !dt.Columns.Contains(columnName))
                return string.Empty;

            string value = string.Empty;
            DataColumn field = dt.Columns[columnName];
            UserProfileManager objuserManager = new UserProfileManager(context);
           UserProfile user=objuserManager.LoadById(currentUser);
            FieldConfigurationManager objFdManager = new FieldConfigurationManager(context);
            FieldConfiguration objField = objFdManager.GetFieldByFieldName(field.ColumnName);
            if (objField.Datatype.ToLower() == "lookup")
            {
             
                DataTable lList = GetTableDataManager.GetTableData(objField.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (lList != null)
                {
                    if (lList.TableName == DatabaseObjects.Tables.Company)
                    {
                        DepartmentManager dpHelper = new DepartmentManager(context);
                        Department dp = dpHelper.LoadByID(user.DepartmentId);
                        if (dp == null || dp.CompanyLookup == null)
                            value = string.Empty;
                        else
                        {
                            value = dp.CompanyLookup.Value;
                        }
                    }
                    else if (lList.TableName == DatabaseObjects.Tables.CompanyDivisions)
                    {
                        DepartmentManager dpHelper = new DepartmentManager(context);
                        Department dp = dpHelper.LoadByID(user.DepartmentId);
                        if (dp == null || dp.DivisionLookup == null)
                            value = string.Empty;
                        else
                        {
                            value = dp.DivisionLookup.Value;
                        }
                    }
                    else if (lList.TableName == DatabaseObjects.Tables.Department)
                    {
                        DepartmentManager dpHelper = new DepartmentManager(context);
                        Department dp = dpHelper.LoadByID(user.DepartmentId);
                        if (dp == null)
                            value = string.Empty;
                        else
                        {
                            value = dp.Title;
                        }
                    }
                    else if (lList.TableName == DatabaseObjects.Tables.FunctionalAreas)
                    {
                        if (user.FunctionalArea != null)
                            value =Convert.ToString(user.FunctionalArea);
                    }
                }
            }
            else if (objField.Datatype.ToLower() =="userfield")
            {
                value = user.Name;
            }
            return value;
        }
    }
}
