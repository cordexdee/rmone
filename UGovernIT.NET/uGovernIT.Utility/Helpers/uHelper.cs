using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Web.UI.WebControls;

namespace uGovernIT.Utility
{
    public static class uHelper
    {
        
        public static bool StringToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            string strValue = value.Trim().ToLower();
            return (strValue == "1" || strValue == "true" || strValue == "yes" || strValue == "on");
        }
        public static bool StringToBoolean(object value)
        {
            if (value == null)
                return false;

            return StringToBoolean(Convert.ToString(value));
        }
        
        public static int getModuleIdByModuleName(string moduleName)
        {
            return getModuleIdByModuleName();
        }
        public static int getModuleIdByModuleName()
        {
            int moduleID = 0;

            DataRow moduleRow = null;
            if (SPContext.Current != null)
                moduleRow = uGITCache.GetModuleDetails(moduleName);
            else
                moduleRow = uGITCache.GetModuleDetails(moduleName, spWeb);

            if (moduleRow != null)
                int.TryParse(Convert.ToString(moduleRow[DatabaseObjects.Columns.Id]), out moduleID);

            return moduleID;
        }

        //public static SPListItem getModuleItemByTicketID(SPWeb oWeb, string ticketID)
        //{
        //    //string moduleName = getModuleNameByTicketId(ticketID);
        //    //if (!string.IsNullOrEmpty(moduleName))
        //    //{
        //    //    SPList modules = oWeb.Lists["Modules"];
        //    //    SPQuery query = new SPQuery();
        //    //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleName, moduleName);
        //    //    SPListItemCollection moduleCollection = modules.GetItems(query);
        //    //    if (moduleCollection != null && moduleCollection.Count > 0)
        //    //        return moduleCollection[0];
        //    //}

        //    return null;
        //}

    }
}
