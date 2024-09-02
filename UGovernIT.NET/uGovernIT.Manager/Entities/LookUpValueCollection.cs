using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.DAL;
using uGovernIT.Utility;
using System.Data;

namespace uGovernIT.Manager
{
    public class LookUpValueCollection : ICollection<LookupValue>
    {
        public List<LookupValue> lookupValues;

        string ParentColName { get; set; }
        string tableName { get; set; }
        string Ids { get; set; }

        public LookUpValueCollection(ApplicationContext context, string parentColName, string tableName, string ids)
        {
            lookupValues = new List<LookupValue>();
            DataTable dt = TicketDal.GetLookupValueCollectionData(parentColName, tableName, ids);
            foreach (DataRow item in dt.Rows)
            {
                LookupValue lookup = new LookupValue();
                lookup.IDValue = Convert.ToInt32(item[DatabaseObjects.Columns.Id]);
                lookup.Value = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                lookupValues.Add(lookup);
            }
        }




        public LookUpValueCollection(ApplicationContext context,string fieldName, string values, bool includeDetail = false)
        {
            lookupValues = new List<LookupValue>();
            if (values.Contains(Constants.Separator))
                values = values.Replace(Constants.Separator, Constants.Separator6);
            List<string> lists = UGITUtility.ConvertStringToList(values, Constants.Separator);
            if (includeDetail)
            {
                if (!fieldName.EndsWith("Lookup"))
                {
                    FieldConfigurationManager configManger = new FieldConfigurationManager(context);
                    DataTable dt = configManger.GetFieldDataByFieldName(fieldName);
                    if (dt == null || dt.Rows.Count <= 0)
                        return;
                    FieldConfiguration field = configManger.GetFieldByFieldName(fieldName);
                    bool isMulti = values.IndexOf(Constants.Separator6) != -1 ? true : false;

                    if (field.Datatype == FieldType.UserField.ToString() ||
                        field.Datatype == FieldType.Choices.ToString())
                    {
                        string[] splitVals = values.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        values = string.Join(",", splitVals.Select(x => string.Format("'{0}'", x)));
                    }
                    DataRow[] dataRows = new DataRow[0];
                    if (isMulti)
                    {
                        dataRows = dt.Select(string.Format("{0} in ({1})", "Id", values));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(values))
                        {
                            dataRows = dt.Select(string.Format("{0} = {1}", "Id", values));
                        }

                    }
                    foreach (DataRow item in dataRows)
                    {
                        LookupValue lookup = new LookupValue();
                        lookup.ID = Convert.ToString(item["Id"]);
                        if (field.Datatype == FieldType.UserField.ToString())
                            lookup.Value = Convert.ToString(item[DatabaseObjects.Columns.Name]);
                        else
                            lookup.Value = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                        lookupValues.Add(lookup);
                    }
                }
            }
            else
            {
               lists.ForEach(x =>
                {
                    lookupValues.Add(new LookupValue(x, string.Empty));
                });
               
            }

        }





        public void Add(LookupValue item)
        {
            lookupValues.Add(item);
           
        }

        public void Clear()
        {
            lookupValues.Clear();
          
        }

        public bool Contains(LookupValue item)
        {
            return lookupValues.Exists(x => x.ID == item.ID);
           
        }

        public void CopyTo(LookupValue[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < lookupValues.Count; i++)
            {
                array[i + arrayIndex] = lookupValues[i];
            }
          
        }

        public int Count
        {
            get { return lookupValues.Count; }
          
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
            
        }

        public bool Remove(LookupValue item)
        {
            return lookupValues.Remove(item);
            
        }

        public IEnumerator<LookupValue> GetEnumerator()
        {
            return lookupValues.GetEnumerator();
         
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return lookupValues.GetEnumerator();
           
        }
    }
}
