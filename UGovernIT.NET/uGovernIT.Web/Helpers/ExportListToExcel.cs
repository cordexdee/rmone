using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;

namespace uGovernIT.Web.Helpers
{
    public class ExportListToExcel
    {
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
                context.Response.End();

            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }


        public static DataTable GetDataTableFromList(string lstName)
        {
            DataTable spcoll = GetTableDataManager.GetTableData(lstName);
            if (spcoll != null)
            {
                var spfieldList = spcoll.Columns.Cast<DataColumn>();
                List<DataColumn> SelectedFields = spfieldList.OrderBy(x => x.ColumnName).ToList();
            }

            return GetDataTableFromList(lstName);
        }

        public static DataTable GetDataTableFromList(DataTable dttoExport, ApplicationContext context)
        {
            DataTable dt = CreateListSchemaDataTable(dttoExport);
            //SPQuery query = new SPQuery();
            //query.Query = "<Where></Where>";
            // SPListItemCollection splstcol = SPListHelper.GetSPListItemCollection(lstName, query);
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

