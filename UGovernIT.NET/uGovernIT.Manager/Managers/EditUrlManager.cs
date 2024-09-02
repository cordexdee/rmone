using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class EditUrlManager
    {
        static Dictionary<string, EditUrlEntity> dicEditUrl;
        static EditUrlManager()
        {
            Dictionary<string, EditUrlEntity> assigneeDic = new Dictionary<string, EditUrlEntity>();
            EditUrlEntity obj = new EditUrlEntity();
            obj.Url = "/_layouts/15/ugovernit/delegatecontrol.aspx?control=vendorissueedit&isudlg=1&pageTitle=Edit Issue";
            obj.Width = 600;
            obj.Height = 575;
            Dictionary<string, string> dicParamMapping = new Dictionary<string, string>();
            dicParamMapping.Add("projectID",DatabaseObjects.Columns.VendorMSALookup);
            dicParamMapping.Add("taskid", DatabaseObjects.Columns.Id);

            obj.ParamMapping = dicParamMapping;

            assigneeDic.Add(DatabaseObjects.Tables.VendorIssues, obj);
            dicEditUrl = assigneeDic;
        }

        /// <summary>
        /// Get edit url for passed table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static EditUrlEntity GetEditUrl(string tableName)
        {
            EditUrlEntity returnobj = null;
            if (dicEditUrl != null && dicEditUrl.Keys.Count > 0)
            {
                returnobj = dicEditUrl.AsEnumerable().Where(x => x.Key == tableName).Select(y => y.Value).FirstOrDefault();
            }
            return returnobj;
        }
        public static DataTable UpdateEditUrlForEach(DataTable dt, EditUrlEntity entity, Dictionary<string, string> currentlyMapping)
        {
            DataTable temp = dt;

            if (entity == null)
                return temp;

            string url = string.Format("{0}&Width={1}&Height={2}", entity.Url, entity.Width, entity.Height);

            Dictionary<string, string> dicMappedParam = entity.ParamMapping;
            Dictionary<string, string> lstExistParamInTable = new Dictionary<string, string>();

            foreach (var current in dicMappedParam)
            {
                if (!currentlyMapping.ContainsKey(current.Value))
                    continue;

                string mappedColumn = currentlyMapping[current.Value];
                if (temp.Columns.Contains(mappedColumn) && !lstExistParamInTable.Keys.Contains(current.Key))
                {

                    lstExistParamInTable.Add(current.Key, mappedColumn);
                }
            }


            if (lstExistParamInTable != null && lstExistParamInTable.Count > 0)
            {
                if (!temp.Columns.Contains("EnableEditUrl"))
                    temp.Columns.Add("EnableEditUrl");

                temp.AsEnumerable().ToList().ForEach(x => UpdateEditUrl(x, lstExistParamInTable, url));

            }

            return temp;
        }

        public static void UpdateEditUrl(DataRow x, Dictionary<string, string> lstExistParamInTable, string url)
        {
            lstExistParamInTable.ToList().ForEach(y => url = url + string.Format("&{0}={1}", y.Key, Convert.ToString(x[y.Value])));
            x["EnableEditUrl"] = url;
        }
    }
}
