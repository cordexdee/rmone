using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Collections.Generic;
using uGovernIT.Util.Log;

/// <summary>
/// Summary description for StringUtils
/// </summary>
public class StringUtils
{
    public const string STRINGLIST_DELIM = ", ";
    public const string STRINGLIST_START = "(";     // don't expect it to be used, but adding for completeness
    public const string STRINGLIST_END = ")";

    public static string getValue(Object o)
    {
        if (o == null || o.Equals(DBNull.Value))
            return "";
        else
            return o.ToString();
    }

    public static bool isNumberValid(string univId)
    {
        if (univId == null || univId.Trim().Equals(""))
        {
            return false;
        }
        else
        {
            try
            {
                Int64.Parse(univId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public static bool isStringNonEmpty(string var)
    {
        if ((var == null) || var.Trim().Equals(""))
        {
            return false;
        }
        return true;
    }

    public static bool isRealNumberValid(string univId)
    {
        if (univId == null || univId.Trim().Equals(""))
        {
            return false;
        }
        else
        {
            try
            {
                Double.Parse(univId);
                return true;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return false;
            }
        }
    }

    public static long getNumberFromString(string univId)
    {
        if (isNumberValid(univId))
        {
            return Int64.Parse(univId);
        }
        else
            return -1;
    }

    public static long getNumberFromString(string univId, long def)
    {
        if (isNumberValid(univId))
        {
            return Int64.Parse(univId);
        }
        else
            return def;
    }

    public static double getRealNumberFromString(string univId)
    {
        if (isRealNumberValid(univId))
        {
            return Double.Parse(univId);
        }
        else
            return -1;
    }

    /**
     * Tokenize by space
     * */
    public static string[] getTokens(string value)
    {
        return value.Split(new char[] { ' ' });
    }

    public static string getStringFromObject(object p)
    {
        if (p == null || p == DBNull.Value || p.ToString().Trim().Equals(""))
            return null;
        else
            return p.ToString();
    }

    public static long getNumberFromObject(object o)
    {
        string numString = getStringFromObject(o);
        return getNumberFromString(numString);
    }

    public static bool getBooleanFromObject(object p)
    {
        if (p == null)
            return false;
        else
        {
            if (p.ToString().ToLower().Equals("true") || p.ToString().Trim().Equals("1"))
                return true;
            else
                return false;
        }

    }

    public static string[] getTokens(string value, char p)
    {
        if (value != null)
        {
            return value.Split(new char[] { p });
        }
        else
        {
            return null;
        }
    }

    public static string sanitizeForSql(string sqlValue)
    {
        if (sqlValue != null)
            return sqlValue.Replace("'", "''").Replace("--", "").Replace(";", "").Replace("=", "").Replace("\"", "");
        else
            return null;
    }

    public static string sqlRemoveSingleQuote(string sqlValue)
    {
        return sqlValue.Replace("--", "");
    }

    public static string sqlEscape(string sqlValue)
    {
        return sqlValue.Replace("'", "''");
    }

    public static string removeTrailingSeparator(string the_string, string separator)
    {
        if (the_string != null && the_string.Length != 0)
        {
            if (the_string.EndsWith(separator))
            {
                return the_string.Remove(the_string.Length - separator.Length);
            }
        }
        return the_string;
    }

    public static string replaceSpecialChars(string toReplace, char replaceWith)
    {
        char[] specials = { '{', '}', '[', ']', '"', '\'' };
        foreach (char special in specials)
        {
            toReplace = toReplace.Replace(special, replaceWith);
        }
        return toReplace;
    }

    public static string toTitleCase(string word)
    {
        TextInfo ti = new CultureInfo("en-US", false).TextInfo;
        return ti.ToTitleCase(word);
    }

    public static string getCommaSeparatedList(long[] list)
    {
        string value = "";
        if (list == null)
            return value;

        foreach (long o in list)
        {
            value += o + ",";
        }
        value = removeTrailingSeparator(value, ",");
        return value;
    }

    public static string getCommaSeparatedList(List<object> objectList)
    {
        string value = "";
        if (objectList == null)
            return value;

        foreach (object o in objectList)
        {
            value += o.ToString() + ",";
        }
        value = removeTrailingSeparator(value, ",");
        return value;
    }

    public static string formatDate(DateTime currentDate, string format)
    {
        return currentDate.ToString(format);
    }

    public static string formatDate(object dateString, string format)
    {
        DateTime dt = DateTime.Parse(dateString.ToString());
        return dt.ToString(format);
    }

    public static string newLineToBR(string paragraph)
    {
        if (paragraph == null)
            return null;
        else
        {
            return paragraph.Replace(Environment.NewLine, "<br/>");
        }
    }

    public static string convertToString(byte[] rawResponse)
    {
        return System.Text.Encoding.ASCII.GetString(rawResponse);
    }

    public static string getCommaSeparatedList(List<string> stringList)
    {
        if (stringList == null)
            return "";
        else
        {
            string final = "";
            foreach (string item in stringList)
            {
                final += item + ",";
            }
            final = removeTrailingSeparator(final, ",");
            return final;
        }
    }

    /**
     * Template string = Hello #name, Welcome...from #sender
     * values = {"name":"chetan,"sender":"higherstudy"}
     * take all keys in the values.
     * find #+key = (#name) in template string
     * replace #+key =  (#name) with values[key] = "chetan"
     * continue to replace all values.
     * return updated string
     * */
    public static string fillTemplate(string templateString, Dictionary<string, string> values)
    {
        string prefix = "#";
        foreach (string placeholder in values.Keys)
        {
            if (templateString.Contains(prefix + placeholder))
                templateString = templateString.Replace(prefix + placeholder, values[placeholder]);
        }
        return templateString;
    }

    public static string formatMoney(string money)
    {
        if (money != null && !money.Equals(""))
        {
            if (money.Length > 3)
            {
                int totalLength = money.Length;
                money = reverseString(money);
                for (int i = 3; i < money.Length; i += 4)
                {
                    money = money.Insert(i, ",");
                }
                return reverseString(money);
            }
            return money;
        }
        else
            return "";
    }

    public static string reverseString(string straight)
    {
        if (straight != null)
        {
            char[] characters = straight.ToCharArray();
            Array.Reverse(characters);
            return new String(characters);
        }
        return null;
    }

    public static string decodeBase64(string text)
    {
        if (text != null)
        {
            byte[] raw = System.Convert.FromBase64String(text);
            return convertToString(raw);
        }
        else
            return null;
    }

    public static string encodeBase64(string text)
    {
        if (text != null)
        {
            byte[] raw = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            return System.Convert.ToBase64String(raw);
        }
        else
            return null;
    }

    public static string getSqlDateString(DateTime date)
    {
        if (date != null)
            return date.ToString("yyyyMMdd hh:mm:ss.mmm");
        else
            return null;
    }

    public static string Trim(string str, int maxChars)
    {
        if (string.IsNullOrEmpty(str))
            return "";
        string sStr = str;
        if (sStr.Length > maxChars)
        {
            sStr = sStr.Substring(0, maxChars) + "...";
        }
        return sStr;
    }
}
