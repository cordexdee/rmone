using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using System.Data;

namespace uGovernIT.Manager
{
    public class FieldLookupValueCollection
    {
        string ParentColName { get; set; }
        string tableName { get; set; }
        string Ids { get; set; }
        public List<FieldLookupValue> lookupCollection { get; set; }
        public FieldLookupValueCollection(string parentColName, string tableName, string ids)
        {
            DataTable dt = TicketDal.GetLookupValueCollectionData(parentColName, tableName, ids);
            foreach (DataRow item in dt.Rows)
            {
                FieldLookupValue lookup = new FieldLookupValue();
                lookup.ID = Convert.ToInt32(item[DatabaseObjects.Columns.Id]);
                lookup.Value = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                lookupCollection.Add(lookup);
            }
        }
    }
}
