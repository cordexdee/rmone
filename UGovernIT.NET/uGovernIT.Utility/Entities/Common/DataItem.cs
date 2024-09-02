using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class DataItem
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public DataItem()
        { 
        }

        public DataItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
