using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ITAnalyticsBL.Core;

namespace ITAnalyticsBL.ImportExport
{
    static class ExportImportHelper
    {
        public static string GetExportTemplate()
        {
            return System.Web.HttpContext.Current.Server.MapPath("/Content/Template/anlayticExportForInput.xlsx");
        }
       

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
        

       
    }
}
