using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using ITAnalyticsBL.Core;
using ITAnalyticsBL.ET;
using System.Net;
using System.Data;

namespace ITAnalyticsBL.Integration
{
    internal class ETTableIntegration : IETIntegration
    {

        
        public List<ListDetail> LoadAllList(DataIntegration config, bool includeFields, string integratToList, bool showSpecifiedListOnly)
        {
            List<ListDetail> list = new List<ListDetail>();
            ModelDB modelDb = new ModelDB(config.Context);
            if (showSpecifiedListOnly && !string.IsNullOrWhiteSpace(integratToList))
            {
              
                //ETTable etTableObj = context.ETTables.FirstOrDefault(x => x.TableName == integratToList);
                List<ETTable> etTableObj = modelDb.ETTables.Where(x => x.TableName.ToLower() == integratToList.ToLower()).ToList();

                foreach (ETTable ettb in etTableObj)
                {
                    if (ettb != null)
                    {
                        ListDetail lstDetail = new ListDetail();

                        string etTable = string.Format(ETContext.TABLEFORMAT, config.Context.TenantID, ettb.TableName);
                        DataTable dt = ETContext.GetTableSchema(etTable);
                        lstDetail.ListName = ettb.TableName;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lstDetail.Fields = new List<FieldDetail>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                FieldDetail field = new FieldDetail();
                                field.DisplayName = Convert.ToString(dr["COLUMN_NAME"]).Trim();
                                field.InternalName = Convert.ToString(dr["COLUMN_NAME"]).Trim();
                                string dataType = Convert.ToString(dr["DATA_TYPE"]).ToLower();
                                if (dataType == "decimal" || dataType == "double" || dataType == "float" || dataType == "money" || dataType == "real" || dataType == "smallmoney" || dataType == "numeric")
                                {
                                    field.DataType = "Double";
                                }
                                else if (dataType == "bigint")
                                {
                                    field.DataType = "Long";
                                }
                                else if (dataType == "datetime" || dataType == "datetime2" || dataType == "datetimeoffset" || dataType == "date" || dataType == "smalldatetime" || dataType == "timestamp")
                                {
                                    field.DataType = "DateTime";
                                }
                                else if (dataType == "bit")
                                {
                                    field.DataType = "Boolean";
                                }
                                else if (dataType == "integer" || dataType == "int" || dataType == "tinyint")
                                {
                                    field.DataType = "Integer";
                                }
                                else
                                {
                                    field.DataType = "String";
                                }

                                field.DisplayNameWithType = string.Format("{0}({1})", field.DisplayName.Trim(), field.DataType);
                                field.InternalNameWithType = string.Format("{0}({1})", field.InternalName.Trim(), field.DataType);
                                field.RefDisplayName = string.Format("{0}__{1}__{2}", config.Name, lstDetail.ListName, field.DisplayName);
                                field.RefInternalName = string.Format("{0}__{1}__{2}", config.DataIntegrationID, lstDetail.ListName, field.InternalName);
                                lstDetail.Fields.Add(field);
                            }
                        }

                        list.Add(lstDetail);
                    }
                }
               
            }
            else
            {
                var tableList = from tbl in modelDb.ETTables
                                orderby tbl.TableName
                                select new { tbl.TableName,tbl.ETTableID
                                };
                foreach (var tbl in tableList)
                {
                    ListDetail dd = new ListDetail();
                    dd.ListName = tbl.TableName;
                    dd.ListId =Convert.ToString(tbl.ETTableID);
                    list.Add(dd);
                }

            }

            return list;
        }

        public List<IDOutput> GetFieldValuesByParam(DataIntegration config, string selectionCriteria, List<IDInputParam> parms)
        {
            List<IDOutput> lstOutput = new List<IDOutput>();
            if (parms == null || parms.Count <= 0)
            {
                return lstOutput;
            }

            string etTable = parms[0].Listname;

            List<FormulaExpression> expressions = FormulaExpression.ParseFormulaExpressions(selectionCriteria);
            string whereClose = FormulaExpression.ConvertSQLWhereClause(expressions);
            DataTable filteredTable = ETContext.GetDatatableByCriteria(config.Context.TenantID, etTable, whereClose);
            if (filteredTable != null && filteredTable.Rows.Count > 0)
            {
                foreach (IDInputParam param in parms)
                {
                    IDOutput output = new IDOutput();
                    output.FieldName = param.FieldName;
                    if (param.FieldName != null && filteredTable.Columns.Contains(param.FieldName))
                    {
                        for (int i = 0; i < filteredTable.Rows.Count;i++ )
                        {
                            output.FieldValue = Convert.ToString(filteredTable.Rows[i][param.FieldName]);
                        }
                    }
                    output.Info = param.Info;
                    lstOutput.Add(output);
                }
            }

            return lstOutput;
        }

        public string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd)
        {
            //throw new NotImplementedException();
            return string.Empty;
        }


        public List<string> GetFieldValues(DataIntegration config, string etTable, string column)
        {
            etTable = string.Format(ETContext.TABLEFORMAT, config.Context.TenantID, etTable);
            List<string> lstFieldDetails = new List<string>();
            DataTable dt = ETContext.GetAssociatedColumnValues(etTable, column);
            if (dt != null)
            {
                if (dt.Columns[column].DataType == typeof(DateTime))
                {
                    lstFieldDetails = dt.DefaultView.ToTable(true, column).AsEnumerable().Select(x => x.Field<DateTime>(column).ToString("dd MMM yyyy")).ToList();
                    
                }
                else if (dt.Columns[column].DataType == typeof(int))
                {
                    lstFieldDetails = dt.DefaultView.ToTable(true, column).AsEnumerable().Select(x => x.Field<int>(column).ToString()).ToList();
                }
                else if (dt.Columns[column].DataType == typeof(decimal))
                {
                    lstFieldDetails = dt.DefaultView.ToTable(true, column).AsEnumerable().Select(x => x.Field<decimal>(column).ToString()).ToList();
                }
                else if (dt.Columns[column].DataType == typeof(double))
                {
                    lstFieldDetails = dt.DefaultView.ToTable(true, column).AsEnumerable().Select(x => x.Field<double>(column).ToString()).ToList();
                }
                else
                {
                    lstFieldDetails = dt.DefaultView.ToTable(true, column).AsEnumerable().Select(x => x.Field<string>(column)).ToList();
                }
            }
            return lstFieldDetails;
        }


        public DataTable GetETTable(DataIntegration config, string etTable)
        {
            DataTable dt = ETContext.GetDatatableByCriteria(config.Context.TenantID, etTable);
            return dt;
        }
    }
}