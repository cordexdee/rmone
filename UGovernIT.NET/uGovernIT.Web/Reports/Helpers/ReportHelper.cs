using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace uGovernIT.Helpers
{
    public static class ReportHelper
    {
        public static  bool CheckFilterValues(NameValueCollection nameValueCollection, List<string> list)
        {
            Dictionary<string, string> items = ConstructQueryString(nameValueCollection, list);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.Value))
                        return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        public static Dictionary<string, string> ConstructQueryString(NameValueCollection parameters, List<string> list)
        {

            Dictionary<string, string> items = new Dictionary<string, string>();
            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(parameters.GetKey(i)) && list.Contains(parameters.GetKey(i)))
                {
                    items.Add(parameters.GetKey(i), parameters.Get(i));
                }
            }
            return items;
        }
        public static string ExportFiles(XtraReport report, string attachFormat, string filePath, string title)
        {
            string fileName = string.Empty;
            if (attachFormat == "xls")
            {
                fileName = title + ".xls";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                report.ExportToXls(Path.Combine(filePath, fileName));
            }
            else if (attachFormat == "pdf")
            {
                fileName = title + ".pdf";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                report.ExportToPdf(Path.Combine(filePath, fileName));
            }

            return Path.Combine(filePath, fileName);
        }
        
        public static string GetReportSubFolder()
        {
            string Folder = new Uri(ConfigurationManager.AppSettings["ReportUrl"].ToString()).AbsolutePath;            
            return Folder;
        }
    }
}