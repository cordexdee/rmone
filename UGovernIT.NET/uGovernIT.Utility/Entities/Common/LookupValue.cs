using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    [Serializable]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class LookupValue
    {
        public string Value { get; set; }
        public string ID { get; set; }
        public long IDValue { get; set; }
        public LookupValue()
        {
            Value = string.Empty;
        }

        public LookupValue(long id, string value)
        {
            ID = id.ToString();
            Value = value;
        }
        public LookupValue(string id, string value)
        {
            ID = id;
            Value = value;
        }


        public static string ConcatenateValues(List<LookupValue> lookups, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                values = string.Join(separator, lookups.Select(x => x.Value).ToArray());
            }
            return values;
        }

        public static string ConcatenateIDs(List<LookupValue> lookups, string separator)
        {
            string ids = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                ids = string.Join(separator, lookups.Select(x => x.IDValue.ToString()).ToArray());
            }
            return ids;
        }

        public static string ConcatenateBoth(List<LookupValue> lookups, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                values = string.Join(separator, lookups.Where(x => x.IDValue > 0).Select(x => string.Format("{0}{1}{2}", x.IDValue, separator, x.Value)).ToArray());
            }
            return values;
        }

        public static string ConcatenateBoth(LookupValue lookup, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (lookup != null && lookup.IDValue > 0)
            {
                values = string.Format("{0}{1}{2}", lookup.IDValue, separator, lookup.Value);
            }
            return values;
        }


        public static string GetValue(LookupValue user)
        {
            if (user != null)
                return user.Value;
            return string.Empty;
        }

        public static long GetID(LookupValue user)
        {
            if (user != null)
                return user.IDValue;

            return 0;
        }
    }
}
