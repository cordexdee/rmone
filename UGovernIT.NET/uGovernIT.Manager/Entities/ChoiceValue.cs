using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Manager
{

    public class ChoiceValue
    {
        public string Value { get; set; }
        public int ID { get; set; }
        public ChoiceValue()
        { 

        }
        public ChoiceValue(int id, string value)
        {
            Value = value;
            id = ID;
        }
        public static string ConcatenateValues(List<ChoiceValue> choicevalues, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Constants.Separator;
            if (choicevalues != null && choicevalues.Count > 0)
            {
                values = string.Join(separator, choicevalues.Select(x => x.Value).ToArray());
            }
            return values;
        }
    }
}
