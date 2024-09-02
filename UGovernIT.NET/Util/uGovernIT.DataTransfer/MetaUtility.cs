using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DataTransfer
{
    public class MetaUtility
    {
        public static string GenerateInsertObjects<T>(List<T> objectList, List<string> excludeProperties = null)
        {
            string strObject = string.Empty;
            if (objectList == null || objectList.Count == 0)
                return strObject;
            
            Type type = objectList.FirstOrDefault().GetType();
            System.Reflection.PropertyInfo[] props = type.GetProperties().Where(x => x.CustomAttributes.FirstOrDefault(y => y.AttributeType.Name == "NotMappedAttribute") == null).ToArray();
            if (props.Length == 0)
                return strObject;

            List<string> strList = new List<string>();

            strList.Add($"List<{type.Name}> list = new List<{type.Name}>();");

            List<string> propStrList = new List<string>();

            string strVal = null;
            foreach (T page in objectList)
            {
                propStrList = new List<string>();
                foreach (System.Reflection.PropertyInfo prop in props)
                {
                    if (excludeProperties != null && excludeProperties.Contains(prop.Name))
                    {
                        continue;
                    }

                    if (prop.PropertyType == typeof(bool))
                    {
                        propStrList.Add($"{prop.Name}={ Convert.ToString(prop.GetValue(page)).ToLower()}");
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(int) || prop.PropertyType == typeof(float) || prop.PropertyType == typeof(long))
                    {
                        propStrList.Add($"{prop.Name}={prop.GetValue(page)}");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        propStrList.Add($"{prop.Name}= Convert.ToDateTime(\"{prop.GetValue(page)}\")");
                    }
                    else if (prop.PropertyType == typeof(bool?))
                    {
                        if(prop.GetValue(page) == null)
                            propStrList.Add($"{prop.Name}=null");
                        else
                            propStrList.Add($"{prop.Name}={ Convert.ToString(prop.GetValue(page)).ToLower()}");
                    }
                    else if (prop.PropertyType == typeof(double?) || prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(float?) || prop.PropertyType == typeof(long?))
                    {
                        if (prop.GetValue(page) == null)
                            propStrList.Add($"{prop.Name}=null");
                        else
                            propStrList.Add($"{prop.Name}={ Convert.ToString(prop.GetValue(page))}");
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        if (prop.GetValue(page) == null)
                            propStrList.Add($"{prop.Name}=null");
                        else
                            propStrList.Add($"{prop.Name}= Convert.ToDateTime(\"{prop.GetValue(page)}\")");
                    }
                    else
                    {
                        strVal = Convert.ToString(prop.GetValue(page));
                        if (strVal == null)
                        {
                            propStrList.Add($"{prop.Name}=null");
                        }
                        else
                        {
                            strVal = strVal.Replace("\"", "\\\"");
                            propStrList.Add($"{prop.Name}=\"{strVal}\"");
                        }
                    }
                }
                strList.Add($"list.Add(new {type.Name}() {{{string.Join(", ", propStrList)}}});");

            }
            strObject = string.Join("\n", strList);

            return strObject;
        }
    }
}
