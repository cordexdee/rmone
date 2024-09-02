using ITAnalyticsBL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace ITAnalyticsBL.Integration
{
    public class QueryIntegration : IETIntegration
    {
        public string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd)
        {
            return string.Empty;
        }

        public DataTable GetETTable(DataIntegration config, string etTable)
        {
            QueryHelperManager qMgr = new QueryHelperManager(config.Context);
            return qMgr.GetReportData(etTable);
        }

        public List<string> GetFieldValues(DataIntegration config, string etTable, string column)
        {
            QueryHelperManager qMgr = new QueryHelperManager(config.Context);
            DataTable data = qMgr.GetReportData(etTable);

            if (data == null)
                return new List<string>();

            return data.AsEnumerable().Select(x => Convert.ToString(x[column])).Distinct().ToList();
        }

        public List<IDOutput> GetFieldValuesByParam(DataIntegration config, string selectionCriteria, List<IDInputParam> parms)
        {
            return new List<IDOutput>();
        }

        public List<ListDetail> LoadAllList(DataIntegration config, bool includeFields, string integratToList, bool showSpecifiedListOnly)
        {
            DashboardManager dashboardManager = new DashboardManager(config.Context);
            List<ListDetail> lists = new List<ListDetail>();

            ListDetail list = new ListDetail();
            List<uGovernIT.Utility.Dashboard> queryDashboards = new List<uGovernIT.Utility.Dashboard>();
            if (showSpecifiedListOnly && !string.IsNullOrWhiteSpace(integratToList))
                queryDashboards = dashboardManager.Load(x => x.DashboardType == DashboardType.Query && x.IsActivated.HasValue && x.IsActivated.Value && x.Title.ToString() == integratToList).Take(1).ToList();
            else
                queryDashboards = dashboardManager.Load(x => x.DashboardType == DashboardType.Query && x.IsActivated.HasValue && x.IsActivated.Value);

            FieldDetail field = null;
            foreach (uGovernIT.Utility.Dashboard ds in queryDashboards)
            {
                list = new ListDetail();
                list.ListName = ds.Title;
                list.ListId = ds.ID.ToString();

                ModelDB modelDb = new ModelDB(config.Context);
                if (includeFields)
                {
                    list.Fields = new List<FieldDetail>();
                    List<uGovernIT.Utility.Dashboard> dashboardsDetail = dashboardManager.LoadDashboardPanels(new List<long>() { ds.ID }, false);
                    if (dashboardsDetail.Count > 0 && dashboardsDetail[0].panel is DashboardQuery)
                    {
                        DashboardQuery dQuery = dashboardsDetail[0].panel as DashboardQuery;
                        if (dQuery.QueryInfo.Tables != null)
                        {
                            foreach (TableInfo tInfor in dQuery.QueryInfo.Tables)
                            {
                                if (tInfor.Columns != null)
                                {
                                    foreach (uGovernIT.Utility.ColumnInfo cInfo in tInfor.Columns)
                                    {
                                        field = new FieldDetail();
                                        field.InternalName = field.DisplayName = cInfo.DisplayName;
                                        field.DataType = cInfo.DataType;
                                        field.DisplayNameWithType = string.Format("{0}({1})", field.DisplayName.Trim(), field.DataType);
                                        field.InternalNameWithType = string.Format("{0}({1})", field.InternalName.Trim(), field.DataType);
                                        field.RefDisplayName = string.Format("{0}__{1}__{2}", config.Name, list.ListName, field.DisplayName);
                                        field.RefInternalName = string.Format("{0}__{1}__{2}", config.DataIntegrationID, list.ListName, field.InternalName);
                                        list.Fields.Add(field);
                                    }
                                }
                            }
                        }
                    }
                }
                lists.Add(list);
            }
            return lists;
        }
    }
}
