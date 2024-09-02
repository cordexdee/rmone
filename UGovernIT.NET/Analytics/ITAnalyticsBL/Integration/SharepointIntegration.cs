using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using System.Net;
using Microsoft.SharePoint.Client;
using ClientOM = Microsoft.SharePoint.Client;
using System.Data;
using uGovernIT.Util.Log;
using uGovernIT.DefaultConfig;

namespace ITAnalyticsBL.Integration
{
    internal class SharepointIntegration : IETIntegration
    {
       
        public List<ListDetail> LoadAllList(DataIntegration config, bool includeFields, string integratToList, bool showSpecifiedListOnly)
        {
            List<ListDetail> lists = new List<ListDetail>();
            try
            {
                using (ClientOM.ClientContext ctx = new ClientOM.ClientContext(config.ConnectionString))
                {
                    ctx.AuthenticationMode = ClientAuthenticationMode.Default;

                    string userid = "kundan";
                    string password = "QWERTY@123";
                    string domain = "ugovernit";

                    if (System.Configuration.ConfigurationManager.AppSettings["userid"] != null)
                    {
                        userid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["userid"]);
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["password"] != null)
                    {
                        password = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["password"]);
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["domain"] != null)
                    {
                        domain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["domain"]);
                    }

                    NetworkCredential credentials = new NetworkCredential(userid, password, domain);
                    ctx.Credentials = credentials;

                    ctx.Load(ctx.Web.Lists);
                    ctx.ExecuteQuery();
                    //ClientOM.List moduleList = ctx.Web.Lists.GetByTitle(config.ListName);
                    //ClientOM.CamlQuery query = new ClientOM.CamlQuery();

                    //if (!showSpecifiedListOnly || includeFields)
                    //{
                    //    query.ViewXml = "<View/>";
                    //}
                    //else
                    //{
                    //    query.ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where></Query></View>", config.FieldName, integratToList);
                    //}

                    if (includeFields && showSpecifiedListOnly && integratToList != string.Empty)
                    {
                        ClientOM.List mList = ctx.Web.Lists.GetByTitle(integratToList);
                        ctx.Load(mList);
                        ctx.Load(mList.Fields);
                        ctx.ExecuteQuery();
                        ListDetail list = new ListDetail();
                        list.ListName = mList.Title;
                        List<FieldDetail> fields = new List<FieldDetail>();
                        foreach (ClientOM.Field mField in mList.Fields)
                        {
                            FieldDetail field = new FieldDetail();
                            if (!mField.FromBaseType)
                            {
                                field.DisplayName = mField.StaticName;
                                field.InternalName = mField.InternalName;
                                field.DataType = ParseSPType(mField.FieldTypeKind.ToString());
                                field.DisplayNameWithType = string.Format("{0}({1})", field.DisplayName, field.DataType);
                                field.InternalNameWithType = string.Format("{0}({1})", field.InternalName, field.DataType);
                                field.RefDisplayName = string.Format("{0}__{1}__{2}", config.Name, list.ListName, field.DisplayName);
                                field.RefInternalName = string.Format("{0}__{1}__{2}", config.DataIntegrationID, list.ListName, field.InternalName);
                                fields.Add(field);
                            }
                        }
                        list.Fields = fields;
                        lists.Add(list);
                    }
                    else
                    {
                        foreach (List spList in ctx.Web.Lists)
                        {
                            if (spList.BaseType == BaseType.GenericList)
                            {
                                ListDetail list = new ListDetail();
                                list.ListName = spList.Title;
                                lists.Add(list);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e,"LoadAllList from sharepoint is crashing");
            }
            return lists;
        }

        public List<IDOutput> GetFieldValuesByParam(DataIntegration config,string selectionCriteria , List<IDInputParam> parms)
        {
            List<IDOutput> outputs = new List<IDOutput>();
            try
            {
                using (ClientOM.ClientContext ctx = new ClientOM.ClientContext(config.ConnectionString))
                {
                    ctx.AuthenticationMode = ClientAuthenticationMode.Default;
                    NetworkCredential credentials = new NetworkCredential("manish", "SharePo!nt1", "ugovernit");
                    ctx.Credentials = credentials;

                    ctx.Load(ctx.Web.Lists);
                    ClientOM.List moduleList = ctx.Web.Lists.GetByTitle(config.ListName);
                    string[] IDLists = parms.Select(x => x.Listname).Distinct().ToArray();

                    List<string> expressions = new List<string>();
                    foreach (string item in IDLists)
                    {
                        expressions.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", config.FieldName, item));
                    }
                    string expQuery = GenerateWhereQueryWithAndOr(expressions, expressions.Count - 1, false);
                    CamlQuery cQuery = new CamlQuery();
                    cQuery.ViewXml = string.Format("<View><Query><Where>{0}</Where></Query></View>", expQuery);
                    ListItemCollection collection = moduleList.GetItems(cQuery);
                    ctx.Load(moduleList);
                    ctx.Load(collection);
                    ctx.ExecuteQuery();
                    

                    List<ListItemCollection> integratedList = new List<ListItemCollection>();
                    foreach (ClientOM.ListItem item in collection)
                    {

                        CamlQuery tQuery = new CamlQuery();
                        //tQuery.ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where></Query></View>", queryStringVariable, queryStringparam);
                        List selectedList = ctx.Web.Lists.GetByTitle(item[config.FieldName].ToString());
                        ListItemCollection selectedCollection = selectedList.GetItems(tQuery);
                        ctx.Load(selectedList);
                        ctx.Load(selectedCollection);
                        ctx.Load(selectedList.Fields);
                        ctx.ExecuteQuery();
                        integratedList.Add(selectedCollection);
                    }

                    foreach (IDInputParam param in parms)
                    {
                        foreach (ListItemCollection item in integratedList)
                        {
                            if (item != null && item.Count > 0 && item[0][param.FieldName] != null)
                            {
                                IDOutput output = new IDOutput();
                                output.FieldName = param.FieldName;



                                if (item[0][param.FieldName].GetType().Name == "FieldLookupValue")
                                {
                                    output.FieldValue = ((ClientOM.FieldLookupValue)item[0][param.FieldName]).LookupValue;
                                }
                                else if (item[0][param.FieldName].GetType().Name == "FieldUserValue")
                                {
                                    output.FieldValue = ((ClientOM.FieldUserValue)item[0][param.FieldName]).LookupValue;
                                }
                                else
                                {
                                    output.FieldValue = item[0][param.FieldName].ToString();
                                }

                                output.Info = param.Info;
                                outputs.Add(output);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e,"GetFieldValues from sharepoint is crashing");
            }
            return outputs;
        }

        /// <summary>
        /// Generate XML of where expressions either in "Aan" OR in "Or"
        /// </summary>
        /// <param name="queryExpression">List of string containing query expresssion</param>
        /// <param name="startIndex">start index of list</param>
        /// <param name="useAnd">Use "And" or "Or"</param>
        /// <returns></returns>
        public string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd)
        {
            string type = "Or";
            if (useAnd)
            {
                type = "And";
            }
            StringBuilder query = new StringBuilder();
            if (queryExpression.Count > 0)
            {
                if (queryExpression.Count > 1)
                {
                    if (endIndex != 0) { query.AppendFormat("<{0}>", type); }
                    query.AppendFormat(queryExpression[endIndex]);
                    endIndex = endIndex - 1;
                    if (endIndex >= 0)
                    {
                        query.Append(GenerateWhereQueryWithAndOr(queryExpression, endIndex, useAnd));
                    }
                }
                else
                {
                    query.AppendFormat(queryExpression[endIndex]);
                }
                if (endIndex != 0) { query.AppendFormat("</{0}>", type); }
            }
            return query.ToString();
        }




        public List<string> GetFieldValues(DataIntegration config, string etTable, string column)
        {
            throw new NotImplementedException();
        }


        public System.Data.DataTable GetETTable(DataIntegration config, string etTable)
        {
            DataTable spTable = new DataTable();
            try
            {
                using (ClientOM.ClientContext ctx = new ClientOM.ClientContext(config.ConnectionString))
                {
                    ////ctx.AuthenticationMode = ClientAuthenticationMode.Default;
                    ////NetworkCredential credentials = new NetworkCredential("manish", "SharePo!nt1", "ugovernit");
                    ////ctx.Credentials = credentials;

                    ctx.AuthenticationMode = ClientAuthenticationMode.Default;

                    string userid = "kundan";
                    string password = "QWERTY@123";
                    string domain = "ugovernit";

                    if (System.Configuration.ConfigurationManager.AppSettings["userid"] != null)
                    {
                        userid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["userid"]);
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["password"] != null)
                    {
                        password = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["password"]);
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["domain"] != null)
                    {
                        domain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["domain"]);
                    }

                    NetworkCredential credentials = new NetworkCredential(userid, password, domain);
                    ctx.Credentials = credentials;

                    //ctx.Load(ctx.Web.Lists);
                    ClientOM.List specifiedList = ctx.Web.Lists.GetByTitle(etTable);
                    CamlQuery cQuery = new CamlQuery();
                    cQuery.ViewXml = string.Format("<View><Query><Where></Where></Query></View>");
                    ListItemCollection collection = specifiedList.GetItems(cQuery);
                    ctx.Load(specifiedList.Fields);
                    ctx.Load(collection);
                    ctx.ExecuteQuery();

                    spTable = GetDataTable(specifiedList.Fields, collection);

                    if (spTable != null && spTable.Rows.Count > 0 && !spTable.Columns.Contains("RowID"))
                    {
                        DataTable dtNew = spTable.Clone();
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.Int32");
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;
                        column.ColumnName = "RowID";
                        dtNew.Columns.Add(column);

                        // Addtional merge action to generate autogenerated id
                        dtNew.Merge(spTable, true);

                        return dtNew;
                       
                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e,"GetFieldValues from sharepoint is crashing");
            }

            return spTable;
        }
        // <summary>
        // It creates a new data table from existing rows and columns of the selected share point table.
        // </summary>
        // <param name="fieldCollection">column collection of the table. </param>
        // <param name="collection"> Rows collection of the table.</param>
        // <returns></returns>
        private DataTable GetDataTable(FieldCollection fieldCollection, ListItemCollection collection)
        {
            DataTable spTable = new DataTable();
           // DataRow dr = null;
            try
            {
                DataColumn dc = null;
                //it iterates for every column of the table and add the same column name and data type in the spTable(DataTable).
                for (int i = 0; i < fieldCollection.Count; i++)
                {
                    dc = new DataColumn();
                    string dataType = string.Empty;
                    dc.ColumnName = fieldCollection[i].InternalName;// name of the column
                    dataType = ParseSPType(fieldCollection[i].FieldTypeKind.ToString());//get the data type of the column
                    dc.DataType = Type.GetType(string.Format("System.{0}", dataType));//set the data type of the column
                    spTable.Columns.Add(dc);//adds the column in the spTable(dataTable).
                }


                if (collection == null || collection.Count <= 0)
                {
                    return null;
                }

                //it iterates for every row of the table and add the same row in the spTable(DataTable).
                for (int i = 0; i < collection.Count; i++)
                {
                    DataRow dr = spTable.NewRow();
                    //it ignores those column which are not defined in the collections of rows.
                    foreach (DataColumn dtColumn in spTable.Columns)
                    {
                        if (!collection[i].FieldValues.ContainsKey(dtColumn.ColumnName))
                        {
                            continue;
                        }
                        // it checks for the null value and assign the default value for different data types.
                        if (collection[i].FieldValues[dtColumn.ColumnName] == null)
                        {
                            if (dtColumn.DataType == typeof(DateTime))
                            {
                                dr[dtColumn] = DateTime.Now;
                            }
                            else if(dtColumn.DataType==typeof(Decimal))
                            {
                                dr[dtColumn] = 0.0;
                            }
                            else if(dtColumn.DataType == typeof(Int32))
                            {
                                dr[dtColumn] = 0;
                            }
                        }
                        else
                        { // assign
                            if (collection[i][dtColumn.ColumnName].GetType().Name == "FieldLookupValue")
                            {
                                dr[dtColumn] = ((ClientOM.FieldLookupValue)collection[i][dtColumn.ColumnName]).LookupValue;
                            }
                            else if (collection[i][dtColumn.ColumnName].GetType().Name == "FieldUserValue")
                            {
                                dr[dtColumn] = ((ClientOM.FieldUserValue)collection[i][dtColumn.ColumnName]).LookupValue;
                            }
                            else
                            {
                                dr[dtColumn] = collection[i][dtColumn.ColumnName].ToString();
                            }
                        }
                    }

                    spTable.Rows.Add(dr);// after assinging values to each column row is added to the spTable(dataTable).
                }
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return spTable;
        }
        // <summary>
        // parsing the share point data field type into dot net data type.
        // </summary>
        // <param name="spFieldType"></param>
        // <returns></returns>
        private string ParseSPType(string spFieldType)
        {
            spFieldType = spFieldType.ToLower();
            if (spFieldType == "boolean")
            {
                return "Boolean";
            }
            else if (spFieldType == "counter" || spFieldType == "integer")
            {
                return "Int32";
            }
            else if (spFieldType == "currency" || spFieldType == "number")
            {
               return "Decimal";
            }
            else if (spFieldType == "datetime")
            {
                return "DateTime";
            }
            else
            {
                return "String";
            }
        }
    }
}










