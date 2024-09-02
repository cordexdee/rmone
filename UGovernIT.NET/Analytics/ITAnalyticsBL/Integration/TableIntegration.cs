using ITAnalyticsBL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;

namespace ITAnalyticsBL.Integration
{
    public class TableIntegration : IETIntegration
    {
        public string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd)
        {
            return string.Empty;
        }

        public DataTable GetETTable(DataIntegration config, string etTable)
        {
            TicketManager tManager = new TicketManager(config.Context);
            return uGITDAL.GetTable(etTable, string.Format("[{0}]='{1}'", DatabaseObjects.Columns.TenantID, config.Context.TenantID)); 
        }

        public List<string> GetFieldValues(DataIntegration config, string etTable, string column)
        {
            TicketManager tManager = new TicketManager(config.Context);
            DataTable data = uGITDAL.GetTable(etTable, string.Format("[{0}]='{1}'", DatabaseObjects.Columns.TenantID, config.Context.TenantID), column);

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
            ModuleViewManager mManager = new ModuleViewManager(config.Context);
            List<ListDetail> lists = new List<ListDetail>();

            ListDetail list = new ListDetail();
            List<string> tables = new List<string>();
            if (showSpecifiedListOnly && !string.IsNullOrWhiteSpace(integratToList))
                tables.Add(integratToList);
            else
                tables = mManager.LoadAllModule().Select(x => x.ModuleTable).Distinct().ToList();

            FieldDetail field = null;
            foreach (string table in tables)
            {
                list = new ListDetail();
                list.ListName = table;
                list.ListId = table;
                ModelDB modelDb = new ModelDB(config.Context);
                if (includeFields)
                {
                    list.Fields = new List<FieldDetail>();
                    DataTable fields = uGITDAL.GetTableSchema(config.Context.Database, table);
                    foreach (DataRow row in fields.Rows)
                    {
                        field = new FieldDetail();
                        field.InternalName = field.DisplayName = Convert.ToString(row[DatabaseObjects.Columns.ColumnNameSchema]);
                        field.DataType = Convert.ToString(row[DatabaseObjects.Columns.DATA_TYPE]);
                        field.DisplayNameWithType = string.Format("{0}({1})", field.DisplayName.Trim(), field.DataType);
                        field.InternalNameWithType = string.Format("{0}({1})", field.InternalName.Trim(), field.DataType);
                        field.RefDisplayName = string.Format("{0}__{1}__{2}", config.Name, list.ListName, field.DisplayName);
                        field.RefInternalName = string.Format("{0}__{1}__{2}", config.DataIntegrationID, list.ListName, field.InternalName);
                        list.Fields.Add(field);
                    }
                }
                lists.Add(list);
            }
            return lists;
        }

        
    }
}
