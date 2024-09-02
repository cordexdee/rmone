using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Manager
{
    public class ExportListToExcel
    {


        /// <summary>
        /// This method return DataTable representation of the SPList, after filtering the updatable fields 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //public static DataTable GetDataTableFromSPList(SPList list)
        //{
        //    DataTable dt = new DataTable();
        //    List<SPField> colList = new List<SPField>();

        //    if (list != null)
        //    {
        //        SPListItemCollection items = list.Items;
        //        SPFieldCollection fc = items.Add().Fields;
        //        foreach (SPField field in fc)
        //        {
        //            if (field.FromBaseType.Equals(false) && !field.Hidden && !field.ReadOnlyField && field.Type != SPFieldType.Attachments && field.InternalName != "ContentType")
        //            {
        //                String type = field.TypeAsString;
        //                Type dataType = null;
        //                switch (type)
        //                {
        //                    case "DateTime":
        //                        dataType = typeof(DateTime);
        //                        break;
        //                    case "Boolean":
        //                        dataType = typeof(Boolean);
        //                        break;
        //                    case "Text":
        //                    case "Note":
        //                        dataType = typeof(String);
        //                        break;

        //                    case "Integer":
        //                        dataType = typeof(Int32);
        //                        break;

        //                    case "Number":
        //                        dataType = typeof(Decimal);
        //                        break;

        //                    case "Currency":
        //                        dataType = typeof(Decimal);
        //                        break;

        //                    case "Lookup":
        //                        dataType = typeof(String);
        //                        break;

        //                    case "User":
        //                        dataType = typeof(String);
        //                        break;
        //                    default:
        //                        dataType = typeof(String);
        //                        break;
        //                }
        //                colList.Add(field);
        //            }


        //        }
        //        String[] fieldsNameArray = new String[colList.Count];
        //        int counter = 0;
        //        foreach (SPField field in colList)
        //        {
        //            fieldsNameArray[counter] = field.InternalName;
        //            counter++;
        //        }

        //        SPListItemCollection filteredItems = list.GetItems(fieldsNameArray);

        //        if (filteredItems.Count == 0)
        //        {
        //            SPListItem li = filteredItems.Add(); //this will add empty item at the top, if list is empty
        //            li.Update();
        //        }
        //        dt = filteredItems.GetDataTable();
        //        if (dt.Columns["Created"] != null)
        //            dt.Columns.Remove("Created");
        //        if (dt.Columns["Modified"] != null)
        //            dt.Columns.Remove("Modified");
        //        if (dt.Columns["ID"] != null)
        //            dt.Columns.Remove("ID");
        //        int rowCounter = 0;
        //        foreach (DataColumn cl in dt.Columns)
        //        {
        //            if (colList.Count == dt.Columns.Count)
        //            {
        //                cl.Caption = colList[rowCounter].Title;
        //                cl.ColumnName = colList[rowCounter].Title;
        //                rowCounter++;
        //            }

        //        }
        //    }
        //    if (dt != null)
        //        return dt;
        //    else
        //        return (new DataTable());
        //}

        /// <summary>
        /// This method uses Response object and creates a xls file, which can be saved on client machine
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            try
            {
                 
                context.Response.ClearContent();
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".xls");
               // context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                string tab = "";
                foreach (DataColumn dc in table.Columns)
                {
                    context.Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                context.Response.Write("\n");

                int i;
                foreach (DataRow dr in table.Rows)
                {
                    tab = "";
                    for (i = 0; i < table.Columns.Count; i++)
                    {
                        context.Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    context.Response.Write("\n");
                }
                //context.Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }


        public static DataTable GetDataTableFromList(string lstName, string Modulename = null)
        {

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DataTable spcoll = GetTableDataManager.GetTableData(lstName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = '{Modulename}'");
            List<DataColumn> SelectedFields = null;
            if (spcoll != null)
            {
                var spfieldList = spcoll.Columns.Cast<DataColumn>();
                SelectedFields = spfieldList.OrderBy(x => x.ColumnName).ToList();
            }

            return GetDataTableFromList(lstName, SelectedFields, context, "");
        }

        public static DataTable GetDataTableFromList(string lstName, List<DataColumn> dttoExport, ApplicationContext context, string query = "")
        {
            DataTable dt = CreateListSchemaDataTable(lstName, dttoExport);

            DataRow[] dataRows = GetTableDataManager.GetTableData(lstName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'", query).Select();
            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            List<FieldConfiguration> listField = new List<FieldConfiguration>();

            foreach (DataRow item in dataRows)
            {
                DataRow trow = dt.NewRow();

                foreach (DataColumn f in item.Table.Columns)
                {
                    if (f.ColumnName != "Attachments")
                    {
                        if (dttoExport.Exists(x => x.ColumnName == f.ColumnName))
                        {
                            FieldConfiguration configField;
                            if (!listField.Any(x => x.FieldName == f.ColumnName))
                            {
                                configField = configFieldManager.GetFieldByFieldName(f.ColumnName);
                                if (configField == null)
                                {
                                    configField = new FieldConfiguration();
                                    configField.FieldName = f.ColumnName;
                                    configField.Datatype = Convert.ToString(f.DataType);
                                }
                                listField.Add(configField);
                            }
                            else
                            {
                                configField = listField.FirstOrDefault(x => x.FieldName == f.ColumnName);
                            }

                            string fieldColumnType = string.Empty;
                            if (configField.Datatype == "UserField")
                            {
                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[f.ColumnName])))
                                    trow[f.ColumnName] = configFieldManager.GetFieldConfigurationData(configField, UGITUtility.ObjectToString(item[f.ColumnName]));

                            }
                            else if (configField.Datatype == "Lookup")
                            {
                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[f.ColumnName])))
                                    trow[f.ColumnName] = configFieldManager.GetFieldConfigurationData(configField, UGITUtility.ObjectToString(item[f.ColumnName]));
                            }

                            else
                            {

                                trow[f.ColumnName] = UGITUtility.ObjectToString(item[f.ColumnName]);
                            }

                        }

                    }

                }
                dt.Rows.Add(trow);
            }
            //Customization for column order
            if (lstName == DatabaseObjects.Tables.RequestType && dt != null && dt.Rows.Count > 0)
            {
                Dictionary<string, Tuple<string, int>> dicSetCustomOrder = new Dictionary<string, Tuple<string, int>>();
                dicSetCustomOrder.Add(DatabaseObjects.Columns.ModuleNameLookup, new Tuple<string, int>(DatabaseObjects.Columns.Module, 0));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.Title, new Tuple<string, int>(DatabaseObjects.Columns.Title, 1));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.Category, new Tuple<string, int>(DatabaseObjects.Columns.Category, 2));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.TicketRequestTypeSubCategory, new Tuple<string, int>("Sub-Category", 3));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.TicketRequestType, new Tuple<string, int>("Request Type", 4));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.RequestCategory, new Tuple<string, int>("RMM Category", 5));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.FunctionalAreaLookup, new Tuple<string, int>("Functional Area", 6));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.WorkflowType, new Tuple<string, int>("Workflow Type", 7));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.RequestTypeOwner, new Tuple<string, int>("Owner", 8));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.PRPGroup, new Tuple<string, int>("PRP Group", 9));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.PRP, new Tuple<string, int>("PRP", 10));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.TicketORP, new Tuple<string, int>("ORP", 11));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.RequestTypeEscalationManager, new Tuple<string, int>("Escalation Manager", 12));
                dicSetCustomOrder.Add(DatabaseObjects.Columns.RequestTypeBackupEscalationManager, new Tuple<string, int>("Backup Escalation Manager", 13));

                //Set Order
                foreach (string key in dicSetCustomOrder.Keys)
                {
                    if (uHelper.IfColumnExists(key, dt))
                    {
                        Tuple<string, int> dicItem = dicSetCustomOrder[key];
                        dt.Columns[key].SetOrdinal(dicItem.Item2);
                        dt.Columns[key].Caption = dicItem.Item1;
                    }
                }

                if (uHelper.IfColumnExists(DatabaseObjects.Columns.Deleted, dt))
                {
                    dt.Columns[DatabaseObjects.Columns.Deleted].Caption = "Deleted";
                    dt.Columns[DatabaseObjects.Columns.Deleted].SetOrdinal(dt.Columns.Count - 1);
                }

                if (uHelper.IfColumnExists(DatabaseObjects.Columns.RequestTypeDescription, dt))
                {
                    dt.Columns[DatabaseObjects.Columns.RequestTypeDescription].Caption = "Description";
                    dt.Columns[DatabaseObjects.Columns.RequestTypeDescription].SetOrdinal(dt.Columns.Count - 1);
                }
            }
            return dt;
        }
        public static DataTable GetDataTableFromList(DataTable dttoExport, ApplicationContext context, string query = "")
        {
            DataTable dt = CreateListSchemaDataTable(dttoExport);

            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            List<FieldConfiguration> listField = new List<FieldConfiguration>();

            foreach (DataRow item in dttoExport.Rows)
            {
                DataRow trow = dt.NewRow();

                foreach (DataColumn f in item.Table.Columns)
                {
                    if (f.ColumnName != "Attachments")
                    {
                        FieldConfiguration configField;
                        if (!listField.Any(x => x.FieldName == f.ColumnName))
                        {
                            configField = configFieldManager.GetFieldByFieldName(f.ColumnName);
                            if (configField == null)
                            {
                                configField = new FieldConfiguration();
                                configField.FieldName = f.ColumnName;
                                configField.Datatype = Convert.ToString(f.DataType);
                            }
                            listField.Add(configField);
                        }
                        else
                        {
                            configField = listField.FirstOrDefault(x => x.FieldName == f.ColumnName);
                        }

                        string fieldColumnType = string.Empty;
                        if (configField.Datatype == "UserField")
                        {
                            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[f.ColumnName])))
                                trow[f.ColumnName] = configFieldManager.GetFieldConfigurationData(configField, UGITUtility.ObjectToString(item[f.ColumnName]));

                        }
                        else if (configField.Datatype == "Lookup")
                        {
                            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[f.ColumnName])))
                                trow[f.ColumnName] = configFieldManager.GetFieldConfigurationData(configField, UGITUtility.ObjectToString(item[f.ColumnName]));
                        }

                        else
                        {

                            trow[f.ColumnName] = UGITUtility.ObjectToString(item[f.ColumnName]);
                        }

                    }

                }
                dt.Rows.Add(trow);
            }
            return dt;
        }
        private static DataTable CreateListSchemaDataTable(string lstName, List<DataColumn> columns)
        {
            DataTable dtSchema = new DataTable();
            //  SPList list = SPListHelper.GetSPList(lstName);

            // Exclude Attachement
            columns = columns.AsEnumerable().Where(x => x.ColumnName != DatabaseObjects.Columns.Attachments).ToList();

            if (columns != null && columns.Count > 0)
            {
                foreach (DataColumn colitem in columns)
                {
                    if (colitem.DataType == typeof(DateTime))
                        dtSchema.Columns.Add(colitem.ColumnName, typeof(DateTime));
                    else if (colitem.DataType == typeof(int))
                        dtSchema.Columns.Add(colitem.ColumnName, typeof(double));
                    else
                        dtSchema.Columns.Add(colitem.ColumnName, typeof(string));
                }
            }

            return dtSchema;
        }
        //private static string GetGroupsAndLoginNamesMultiLookupField(string multiLookupValue)
        //{

        //    SPFieldUserValueCollection ownerspValueCol = new SPFieldUserValueCollection(SPContext.Current.Web, multiLookupValue);
        //    string strValue = string.Empty;

        //    if (ownerspValueCol.Count > 0)
        //    {
        //        foreach (SPFieldUserValue lookup in ownerspValueCol)
        //        {
        //            if (lookup.User != null)
        //            {
        //                SPUser user = UserProfile.GetUserById(lookup.LookupId);
        //                if (user != null)
        //                {
        //                    if (!string.IsNullOrEmpty(strValue))
        //                        strValue += ",";
        //                    strValue += user.LoginName;
        //                }
        //            }
        //            else
        //            {
        //                //SPGroup group = UserProfile.GetGroupByID(lookup.LookupId, SPContext.Current.Web);
        //                //if (group != null)
        //                //{
        //                //    if (!string.IsNullOrEmpty(strValue))
        //                //        strValue += ",";
        //                //    strValue += group.Name;
        //                //}

        //                if (!string.IsNullOrEmpty(strValue))
        //                    strValue += ",";
        //                strValue += lookup.LookupValue;
        //            }
        //        }
        //    }

        //    return strValue;
        //}

        private static DataTable CreateListSchemaDataTable(DataTable dt)
        {

            DataTable dtSchema = new DataTable();
            if (dt != null && dt.Columns.Count > 0)
            {
                foreach (DataColumn colitem in dt.Columns)
                {
                    if (colitem.ColumnName != "Attachments")
                        dtSchema.Columns.Add(colitem.ColumnName, typeof(string));
                }
            }
            return dtSchema;
        }
    }
}
