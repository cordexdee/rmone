using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public static class DicExtensions
    {
        public static Dictionary<string, TValue> ReplaceInKeys<TValue>(this Dictionary<string, TValue> oldDictionary, string replaceIt, string withIt)
        {
            // Do all the works with just one line of code:
            return oldDictionary
                   .Select(x => new KeyValuePair<string, TValue>(x.Key.Replace(replaceIt, withIt), x.Value))
                   .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
