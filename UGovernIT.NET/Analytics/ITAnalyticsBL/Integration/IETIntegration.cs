using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using ITAnalyticsUtility;
using System.Data;


namespace ITAnalyticsBL.Integration
{
    internal interface IETIntegration
    {
        List<ListDetail> LoadAllList(DataIntegration config, bool includeFields, string integratToList, bool showSpecifiedListOnly);
        List<IDOutput> GetFieldValuesByParam(DataIntegration config, string selectionCriteria, List<IDInputParam> parms);
        string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd);
        List<string> GetFieldValues(DataIntegration config, string etTable, string column);
        DataTable GetETTable(DataIntegration config, string etTable);
       
    }
}
