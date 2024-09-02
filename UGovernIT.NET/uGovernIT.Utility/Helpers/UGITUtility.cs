using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using uGovernIT.Util.Log;

namespace uGovernIT.Utility
{
    public static class UGITUtility
    {
        #region Do Not Change Without Discussion with Manish Sir
        //public enum UserType { User, Role };
        //public enum RoleType { SAdmin=1, Admin=2,ResourceAdmin=3, TicketAdmin=4 };
        #endregion

        public static Boolean CheckType(String value, Type type)
        {
            try
            {
                var obj = Convert.ChangeType(value, type);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public static bool StringToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            else
            {
                string strValue = value.Trim().ToLower();
                return (strValue == "1" || strValue == "true" || strValue == "yes" || strValue == "on");
            }
        }

        public static DataRow SetRowValues(DataRow dr)
        {
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName])))
                {
                    dr.Table.Columns[dc.ColumnName].AllowDBNull = true;
                    dr[dc.ColumnName] = DBNull.Value;
                }
            }
            return dr;
        }

        public static string RemoveWildCardFromQuery(string filterValue)
        {
            string lb = "~~LeftBracket~~";
            string rb = "~~RightBracket~~";
            return filterValue.Replace("[", lb).Replace("]", rb).Replace("*", "[*]").Replace("%", "[%]").Replace("'", "''");
        }

        public static bool StringToBoolean(object value)
        {
            if (value == null)
                return false;
            else
                return StringToBoolean(Convert.ToString(value));
        }

        public static string ObjectToString(object p)
        {
            if (p == null || p == DBNull.Value || p.ToString().Trim().Equals(""))
                return null;
            else
                return p.ToString();
        }

        public static string GetNullOrDefaultString(object str, string defaultValue = "No")
        {
            if (!string.IsNullOrWhiteSpace(ObjectToString(str)))
            {
                return ObjectToString(str);
            }
            return defaultValue;
        }

        public static string GetFormattedHistoryString(string rawData, bool needHTML)
        {
            return GetFormattedHistoryString(rawData, needHTML, false);
        }

        public static string GetFormattedHistoryString(string rawData, bool needHTML, bool orderAsc)
        {
            return GetFormattedHistoryString(rawData, needHTML, false, true);
        }

        public static string GetFormattedHistoryString(string rawData, bool needHTML, bool orderAsc, bool newLineSeparator)
        {
            string[] versionsDelim = { Constants.SeparatorForVersions };
            string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

            // Original data is in ascending chronological order, so reverse if need descending
            if (!orderAsc)
                versions = versions.Reverse().ToArray();

            string formattedData = string.Empty;
            foreach (string version in versions)
            {
                // Assume <version1>$;#$<version2>$;#$<version3>
                string[] versionDelim = { Constants.Separator };
                string[] versionData = version.Split(versionDelim, StringSplitOptions.None);

                DateTime createdDate;
                if (versionData.GetLength(0) == 3)
                {
                    // Assume <userID>;#<timestamp>;#<text>
                    string createdBy = versionData[0];

                    string created;
                    if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate);
                        created = createdDate.ToLocalTime().ToString("MMM-d-yyyy hh:mm tt");
                    }
                    else
                    {
                        DateTime.TryParse(versionData[1], out createdDate);
                        created = createdDate.ToString("MMM-d-yyyy hh:mm tt");
                    }

                    string entry = versionData[2];
                    if (formattedData != string.Empty)
                    {
                        if (newLineSeparator)
                            formattedData += (needHTML ? "<br>" : "\r\n");
                        else
                            formattedData += Constants.CommentSeparator;
                    }
                    string prefix = string.Format("{0} {1}", created, createdBy);
                    if (needHTML)
                        prefix = string.Format("<b>{0}</b>", prefix);
                    formattedData += string.Format("{0}: {1}", prefix, entry);
                }
                else
                {
                    // Assume whole data is one string
                    if (formattedData != string.Empty)
                    {
                        if (newLineSeparator)
                            formattedData += (needHTML ? "<br>" : "\r\n");
                        else
                            formattedData += Constants.CommentSeparator;
                    }
                    formattedData += version;
                }
            }

            return formattedData;
        }

        public static string ConvertTableToCSV(DataTable table, Dictionary<string, string> dcColDataType = null)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                return string.Empty;
            }

            StringBuilder csvData = new StringBuilder();
            string colDataType = string.Empty;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn column = table.Columns[i];
                if (i != 0)
                {
                    csvData.Append(",");
                }
                csvData.AppendFormat("\"{0}\"", AddSpaceBeforeWord(column.Caption));
                if (i == table.Columns.Count - 1)
                {
                    csvData.Append("\r\n");
                }
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    DataColumn column = table.Columns[i];
                    if (i != 0)
                    {
                        csvData.Append(",");
                    }

                    string value = Convert.ToString(row[column]);

                    //BTS-23-001177: Query Export CSV or Excel not recognized as Date format.
                    if (column.DataType == typeof(DateTime) && !string.IsNullOrEmpty(value))
                    {
                        colDataType = "Date";
                        if (dcColDataType != null)
                        { colDataType = dcColDataType.FirstOrDefault(x => x.Key == column.ColumnName).Value; }
                        if (colDataType == "DateTime")
                            value = ChangeDateFormat(Convert.ToDateTime(row[column]), "yyyy-MM-dd HH:mm");
                        else
                            value = ChangeDateFormat(Convert.ToDateTime(row[column]), "yyyy-MM-dd");
                    }

                    if (column.ColumnName == DatabaseObjects.Columns.TicketResolutionComments ||
                        column.ColumnName == DatabaseObjects.Columns.ProjectSummaryNote)
                    {
                        // Just get the latest entry
                        string[] versionsDelim = { Constants.SeparatorForVersions };
                        string[] versions = value.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

                        if (versions != null && versions.Length != 0)
                        {
                            string latestEntry = versions[versions.Length - 1];

                            // Assume <version1>$;#$<version2>$;#$<version3>
                            string[] versionDelim = { Constants.Separator };
                            string[] versionData = latestEntry.Split(versionDelim, StringSplitOptions.None);

                            if (versionData.GetLength(0) == 3)
                            {
                                // Assume <userID>;#<timestamp>;#<text>
                                string createdBy = versionData[0];
                                DateTime createdDate;
                                string created = string.Empty;
                                if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate))
                                        created = getDateStringInFormat(createdDate.ToLocalTime(), false);// createdDate.ToLocalTime().ToString("MMM-d-yyyy");
                                }
                                else
                                {
                                    if (DateTime.TryParse(versionData[1], out createdDate))
                                        created = getDateStringInFormat(createdDate, false);//createdDate.ToString("MMM-d-yyyy");
                                }
                                value = versionData[2];
                            }
                            else
                            {
                                value = latestEntry;
                            }
                        }

                        value = StripHTML(value);
                    }
                    else if (column.ColumnName == DatabaseObjects.Columns.TicketComment ||
                             column.ColumnName == DatabaseObjects.Columns.History)
                    {
                        value = GetFormattedHistoryString(value, false);
                    }
                    else if (value.Contains(Constants.Separator))
                    {
                        // Get value and strip IDs if lookup field
                        value = RemoveIDsFromLookupString(value);
                    }

                    value = value.Replace("\"", "\"\""); // Replace " with "" to avoid csv formatting errors
                    csvData.AppendFormat("\"{0}\"", value);
                    if (i == table.Columns.Count - 1)
                    {
                        csvData.Append("\r\n");
                    }
                }
            }

            return csvData.ToString();
        }

        /// <summary>
        /// Uses HTML Agility Pack to parse and convert HTML into readable text preserving newlines & spaces
        /// https://htmlagilitypack.codeplex.com/
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripHTML(string source, bool stripScriptsOnly = false, int numAllowedErrors = 0, bool stripErrors = false)
        {
            // First - remove all the existing newlines from HTML
            // they mean nothing in HTML, but break our logic
            string html = source;
            if (!stripScriptsOnly)
                html = source.Replace("\r", "").Replace("\n", " ");

            // Now create an Html Agile Doc object (using HTML Agility Pack
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);

            // If not able to parse as HTML, send back as-is since it may be plain-text
            // Should return even if one error but sometimes Outlook has a few malformed tags
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                if (!stripErrors && htmlDoc.ParseErrors.Count() > numAllowedErrors)
                    return source;
            }

            // Remove comments, head, style and script tags
            HtmlNodeCollection htmlNodes = htmlDoc.DocumentNode.SelectNodes("//comment() | //script | //style | //head");
            if (htmlNodes != null)
            {
                foreach (HtmlNode node in htmlNodes)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }

            string tmpString = string.Empty;

            if (!stripScriptsOnly)
            {
                string newLine = Environment.NewLine; // "\r\n"
                // Convert block-elements to line-breaks
                if (htmlDoc.DocumentNode.SelectNodes("//p | //div") != null)
                    foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//p | //div")) //you could add more tags here
                    {
                        node.ParentNode.InsertBefore(htmlDoc.CreateTextNode(newLine), node);
                    }

                // Replace BR tags with newlines
                if (htmlDoc.DocumentNode.SelectNodes("//br") != null)
                    foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//br"))
                    {
                        node.ParentNode.ReplaceChild(htmlDoc.CreateTextNode(newLine), node);
                    }

                // Replace HR tags
                if (htmlDoc.DocumentNode.SelectNodes("//hr") != null)
                    foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//hr"))
                    {
                        node.ParentNode.ReplaceChild(htmlDoc.CreateTextNode("________________________________________" + newLine), node);
                    }

                // Process LI tags
                if (htmlDoc.DocumentNode.SelectNodes("//li") != null)
                    foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//li"))
                    {
                        node.ParentNode.InsertBefore(htmlDoc.CreateTextNode(newLine + " - "), node);
                    }
                tmpString = htmlDoc.DocumentNode.InnerText.Trim();

                // Finally - return the inner text which will have our inserted line-breaks in it minus all the tags
                // Decode &nbsp; and other html-encoded strings with actual spaces
                tmpString = HttpUtility.HtmlDecode(tmpString);

                //remove html from inner text of body if user puts html as text
                //if (Regex.IsMatch(tmpString, "<.*?>"))
                //    tmpString = StripHTML(tmpString);

                // Replace instances of three or more sequential newlines
                tmpString = Regex.Replace(tmpString, @"[\r\n]\s{3,}", "\r\n\r\n", RegexOptions.IgnoreCase);

                // Replace newline+space with newline (extra spaces coming from CreateTextNode?)
                tmpString = tmpString.Replace("\r\n ", "\r\n").Trim();
            }
            else
                tmpString = htmlDoc.DocumentNode.InnerHtml.Trim();

            return tmpString;
        }

        /// <summary>
        /// When we convert a multiuser field into a DataTable, Sharepoint does a stupid thing in taking out the first Id
        /// for example if we have 1;#userName1;#2;#userName2 Sharepoint converts into userName1;#2;#userName2 since its a multi-lookup
        /// We need to manually parse this to get the actual usernames.
        /// Use this or we can also use a heavy operation UserProfile.GetUserInfo
        /// </summary>
        /// <param name="valuesWithIDs">user1Name;#user2Id;#user2Name;#user3Id;#user3Name.....</param>
        /// <returns></returns>
        public static string RemoveIDsFromLookupString(string valuesWithIDs)
        {
            // Replace leading number (ID) before ;#
            if (!string.IsNullOrEmpty(valuesWithIDs))
            {
                valuesWithIDs = Regex.Replace(valuesWithIDs, "^[0-9]+;#", string.Empty);
                // Replace any numbers (IDs) embedded between two ";#"
                return Regex.Replace(valuesWithIDs, ";#[0-9]+;#", "; ");
            }
            return null;

        }

        /// <summary>
        /// It get column value from datarow 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static object GetSPItemValue(DataRow item, string columnValue)
        {
            try
            {
                if (item != null && item.Table.Columns.Contains(columnValue) && item[columnValue] != null)
                {
                    return item[columnValue];
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///It checks whether column exist in the datarow or not. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static bool IsSPItemExist(DataRow item, string columnValue)
        {
            try
            {
                if (item != null && item.Table.Columns.Contains(columnValue) && !string.IsNullOrEmpty(Convert.ToString(item[columnValue])))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsSPItemExist(DataRow item, string[] fieldname)
        {
            bool val = false;
            foreach (string columnValue in fieldname)
            {
                try
                {
                    if (item != null && item.Table.Columns.Contains(columnValue) && !string.IsNullOrEmpty(Convert.ToString(item[columnValue])))
                    {
                        val = true;
                    }

                }
                catch (Exception)
                {
                    val = false;
                }
            }
            return val;
        }

        public static string GetTaskTypeImage(string taskType)
        {
            string fileName = string.Empty;
            if (string.IsNullOrEmpty(taskType))
                return fileName;

            switch (taskType)
            {
                case Constants.TaskType.Deliverable:
                    return "/Content/images/uGovernIT/document_down.png";
                case Constants.TaskType.Receivable:
                    return "/Content/images/uGovernIT/document_up.png";
                case Constants.TaskType.Milestone:
                    return "/Content/images/uGovernIT/milestone_icon.png";
            }

            return fileName;
        }

        public static string GetJsonForDictionary(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("'{0}': '{1}'", d.Key, d.Value));
            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        public static object DeSerializeAnObject(XmlDocument doc, object obj1)
        {
            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
            Object objType = new object();
            XmlSerializer ser = new XmlSerializer(obj1.GetType());
            object obj = ser.Deserialize(reader);
            return obj;
        }

        public static string SerializeXML<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static Dictionary<string, string> GetCustomProperties(string compositeString)
        {
            return GetCustomProperties(compositeString, "#", false, true);
        }

        public static Dictionary<string, string> GetCustomProperties(string compositeString, string sperator)
        {
            return GetCustomProperties(compositeString, sperator, false, true);
        }

        public static Dictionary<string, string> GetCustomProperties(string compositeString, string sperator, bool dontlowerCaseKey, bool removeEmptyPair)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(compositeString))
            {
                string[] pros = compositeString.Split(new string[] { sperator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string pro in pros)
                {
                    string[] oneProperty = pro.Split(new char[] { '=' });
                    if (oneProperty.Length > 0 && !properties.ContainsKey(oneProperty[0]))
                    {
                        if (dontlowerCaseKey)
                        {
                            properties.Add(oneProperty[0].Trim(), oneProperty.Length > 1 ? oneProperty[1].Trim() : string.Empty);
                        }
                        else
                        {
                            properties.Add(oneProperty[0].Trim().ToLower(), oneProperty.Length > 1 ? oneProperty[1].Trim() : string.Empty);
                        }
                    }
                }
            }
            return properties;
        }

        public static string SerializeDicObject(Dictionary<string, object> dic)
        {
            List<DataItem> tempdataitems = new List<DataItem>(dic.Count);
            foreach (string key in dic.Keys)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dic[key])))
                {
                    tempdataitems.Add(new DataItem(key, Convert.ToString(dic[key])));
                }

            }

            return tempdataitems.Serialize<List<DataItem>>();
        }

        public static Dictionary<string, object> DeserializeDicObject(string serialized)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            List<DataItem> tempdataitems = serialized.Deserialize<List<DataItem>>();

            foreach (DataItem d in tempdataitems)
            {
                dic.Add(d.Key, d.Value);
            }
            return dic;
        }

        public static Dictionary<string, string> DeserializeDicObjects(string serialized, string pairSeprator = null, string keyvalueseprator = null)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try

            {
                if (!string.IsNullOrEmpty(serialized))
                {
                    if (string.IsNullOrEmpty(pairSeprator))
                        pairSeprator = Constants.Separator;
                    if (string.IsNullOrEmpty(keyvalueseprator))
                        keyvalueseprator = "=";
                    string[] keyvalueList = UGITUtility.SplitString(serialized, pairSeprator);
                    if (keyvalueList != null && keyvalueList.Count() > 0)
                    {
                        foreach (string keyval in keyvalueList)
                        {
                            if (!string.IsNullOrEmpty(keyval))
                            {
                                string[] keypair = UGITUtility.SplitString(keyval, keyvalueseprator);
                                if (keypair != null && keypair.Count() > 0)
                                {
                                    dic.Add(keypair[0], UGITUtility.ObjectToString(keypair[1]));
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return dic;
        }

        public static string ConvertToFirstMLast(string input)
        {
            // Convert "Last, First M" or "Last, First" or "Last, First, M" format to "First M Last"
            string name = input.Trim();
            if (name.Contains(","))
            {
                string[] comps = name.Split(new string[] { "," }, StringSplitOptions.None);
                if (comps.Length == 2)
                    name = comps[1].Trim() + " " + comps[0].Trim();
                else if (comps.Length == 3)
                    name = comps[1].Trim() + " " + comps[2].Trim() + " " + comps[0].Trim();
            }
            return name;
        }

        public static System.Drawing.Color TranslateColorCode(string colorCode, System.Drawing.Color defaultColor)
        {
            if (string.IsNullOrWhiteSpace(colorCode))
                return defaultColor;

            System.Drawing.KnownColor knowColor = KnownColor.Transparent;
            bool isKnowColor = Enum.TryParse<System.Drawing.KnownColor>(colorCode, out knowColor);
            System.Drawing.Color translatedColor = new System.Drawing.Color();
            if (isKnowColor)
            {
                translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode);
            }
            else
            {
                if (!colorCode.Contains("#"))
                    colorCode = "#" + colorCode;
                else
                {
                    if (colorCode.ToArray().Length < 7)
                        colorCode = defaultColor.Name;
                }

                try
                {
                    translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                    try
                    {
                        translatedColor = System.Drawing.ColorTranslator.FromHtml(colorCode.Replace("#", ""));
                    }
                    catch (Exception exp)
                    {
                        ULog.WriteException(exp);
                        //Log.WriteException(exp);
                        translatedColor = defaultColor;
                    }
                }
            }
            return translatedColor;
        }

        public static int StringToInt(string value)
        {
            return StringToInt(value, 0);
        }

        public static float StringToFloat(string value)
        {
            return StringToFloat(value, 0);
        }

        public static Unit StringToUnit(string value)
        {
            try
            {
                return Unit.Parse(value);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Unit.Parse("0px");
            }

        }

        public static int StringToInt(string value, int defaultValue)
        {
            int dblValue = defaultValue;
            if (!string.IsNullOrEmpty(value) && !int.TryParse(value.Trim(), out dblValue))
            {
                double val = 0;
                double.TryParse(value.Trim(), out val);
                dblValue = Convert.ToInt32(val);
            }
            return dblValue;
        }

        public static float StringToFloat(string value, float defaultValue)
        {
            float dblValue = defaultValue;
            if (!string.IsNullOrEmpty(value) && !float.TryParse(value.Trim(), out dblValue))
            {
                double val = 0;
                double.TryParse(value.Trim(), out val);
                dblValue = Convert.ToInt32(val);
            }
            return dblValue;
        }

        public static int StringToInt(object value)
        {
            if (value == null)
                return 0;

            return StringToInt(Convert.ToString(value));
        }
        public static float StringToFloat(object value)
        {
            if (value == null)
                return 0;

            return StringToFloat(Convert.ToString(value));
        }

        public static long StringToLong(string value)
        {
            return StringToLong(value, 0);
        }

        public static long StringToLong(string value, int defaultValue)
        {
            long dblValue = defaultValue;
            if (!string.IsNullOrEmpty(value) && !long.TryParse(value.Trim(), out dblValue))
            {
                double val = 0;
                double.TryParse(value.Trim(), out val);
                dblValue = Convert.ToInt64(val);
            }
            return dblValue;
        }

        public static long StringToLong(object value)
        {
            if (value == null)
                return 0;

            return StringToLong(Convert.ToString(value));
        }

        public static short StringToShort(object value)
        {
            if (value == null)
                return 0;

            return StringToShort(Convert.ToString(value));
        }

        public static short StringToShort(string value)
        {
            return StringToShort(value, 0);
        }

        public static short StringToShort(string value, short defaultValue)
        {
            short shortValue = defaultValue;
            try
            {
                shortValue = Convert.ToInt16(value);
            }
            catch (Exception) { }
            return shortValue;
        }

        /// <summary>
        /// Translates a relative URL into an absolute URL.
        /// If running in user context, uses SPContext.Current
        /// else uses the SP Web Application's Alt Access Mappings.
        /// </summary>
        /// <param name="url">URL to translate</param>
        /// <param name="spWeb">Site SPWeb</param>
        /// <returns>Absolute URL</returns>
        public static string GetAbsoluteURL(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (IsAbsoluteUrl(relativeUrl))
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static string GetImageUrlForReport(string relativeURL)
        {
            if (string.IsNullOrEmpty(relativeURL))
                return string.Empty;
            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;
            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, relativeURL);
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        /// <summary>
        /// Translates a relative URL into an absolute URL.
        /// If running in user context, uses SPContext.Current
        /// else uses the SP Web Application's Alt Access Mappings.
        /// </summary>
        /// <param name="url">URL to translate</param>
        /// <param name="spWeb">Site SPWeb</param>
        /// <returns>Absolute URL</returns>
        public static string GetAbsoluteURL(string relativeUrl, string siteUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (IsAbsoluteUrl(relativeUrl))
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = new Uri(siteUrl);
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        /// <summary>
        /// To prevent erors in URLs, replaces any of these characters with an underscore: \ # ' "
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceInvalidCharsInURL(string input)
        {
            return Regex.Replace(input, "[\\\\#'\"]", string.Empty);
        }

        public static string GetCookieValue(HttpRequest request, string cookieName)
        {
            HttpCookie cookie = GetCookie(request, cookieName);
            return cookie != null ? cookie.Value : string.Empty;
        }

        public static int GetScreenSizeFromCookie(HttpRequest request, string cookieName)
        {
            HttpCookie cookie = GetCookie(request, cookieName);
            var screenWidth = cookie != null ? cookie.Value : string.Empty;
            if (!string.IsNullOrEmpty(screenWidth) && int.TryParse(screenWidth, out int result))
                return result;
            else
                return 0;
        }

        public static bool IsMobileView(HttpRequest request)
        {
            var width = GetScreenSizeFromCookie(request, "screenWidth");
            if (width != 0 && width <= 480)
                return true;
            else
                return false;
        }

        public static bool IsMobileWithLandscapeView(HttpRequest request)
        {
            var width = GetScreenSizeFromCookie(request, "screenWidth");
            if (width != 0 && width <= 915)
                return true;
            else
                return false;
        }

        public static bool IsGridCollapsed(HttpRequest request)
        {
            bool isCollapsed = false;

            HttpCookie cookie = GetCookie(request, Constants.Cookie.IsGanttViewExpanded);
            string value = cookie != null ? cookie.Value : string.Empty;
            if (!string.IsNullOrWhiteSpace(value) && value == "0")
                isCollapsed = true;
            else
                isCollapsed = false;

            return isCollapsed;
        }

        public static List<string> GetGanttViewCollapsedUsers(HttpRequest request)
        {
            HttpCookie cookie = GetCookie(request, Constants.Cookie.GanttViewCollapsedUsers);
            if (cookie == null)
                return null;

            List<string> users = new List<string>();

            string value = cookie != null ? cookie.Value : string.Empty;
            if (!string.IsNullOrWhiteSpace(cookie.Value))
                users = cookie.Value.Split(',').ToList();

            return users;
        }

        public static List<string> GetGanttViewExpandedUsers(HttpRequest request)
        {
            HttpCookie cookie = GetCookie(request, Constants.Cookie.GanttViewExpandedUsers);
            if (cookie == null)
                return null;

            List<string> users = new List<string>();

            string value = cookie != null ? cookie.Value : string.Empty;
            if (!string.IsNullOrWhiteSpace(cookie.Value))
                users = cookie.Value.Split(',').ToList();

            return users;
        }

        public static HttpCookie GetCookie(HttpRequest request, string cookieName)
        {
            // Check if cookie exists before trying to access value, else browser creates empty cookie!
            if (request.Cookies.AllKeys.Contains(cookieName))
                return request.Cookies[cookieName];
            else
                return null;
        }

        public static void DeleteCookie(HttpRequest request, HttpResponse response, string cookieName)
        {
            // First check if Request contains cookie with this name (note need to look in request not response!)
            if (request.Cookies.AllKeys.Contains(cookieName))
            {
                // Delete from request
                request.Cookies[cookieName].Value = string.Empty;
                request.Cookies.Remove(cookieName);

                // Must create new cookie with same name & expire it to delete
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Path = VirtualPathUtility.MakeRelative("~", HttpContext.Current.Request.Url.PathAndQuery).ToString();  // VirtualPathUtility.MakeRelative(HttpContext.Current.Request.Url.PathAndQuery, UriKind.Relative.ToString());
                //cookie.Value = "";
                cookie.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Add(cookie);
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in propertyInfos)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                string proName = prop.Name;
                if (prop.CustomAttributes != null && prop.CustomAttributes.Count() > 0 && prop.CustomAttributes.Any(x => x.AttributeType.Name == "ColumnAttribute") && prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "ColumnAttribute").ConstructorArguments.Count > 0)
                    proName = Convert.ToString(prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "ColumnAttribute").ConstructorArguments[0].Value);
                if (type.Name == "DateTime" || type.Name == "Int32")
                    dataTable.Columns.Add(proName, type);
                else
                    dataTable.Columns.Add(proName);
            }

            if (items == null) return dataTable;

            try
            {

                foreach (T item in items)
                {
                    var values = new object[propertyInfos.Length];
                    for (var i = 0; i < propertyInfos.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = propertyInfos[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                //ULog.WriteException(ex, errorMessage);
                // Util.Log.ULog.WriteException(ex, MessageCategory.DocumentManagement);

            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ObjectToData(object o)
        {
            DataTable dt = new DataTable("OutputData");

            if (o == null)
                return dt;

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    dt.Columns.Add(f.Name, Nullable.GetUnderlyingType(f.PropertyType) ?? f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o) ?? DBNull.Value;
                }
                catch { }
            });
            return dt;
        }

        public static string moduleTypeName(string moduleName)
        {
            if (moduleName == "NPR")
            {
                return "Request";
            }
            else if (moduleName == "PMM" || moduleName == "CPR" || moduleName == "CNS")
            {
                return "Project";
            }
            else if (moduleName == "CMDB")
            {
                return "Asset";
            }
            else if (moduleName == "TSK")
            {
                return "Tasklist";
            }
            else if (moduleName == "INC")
            {
                return "Outage";
            }
            else if (moduleName == "APP")
            {
                return "Application";
            }
            else if (moduleName == "CMT")
            {
                return "Contract";
            }
            else if (moduleName == "VND")
            {
                return "Vendor MSA";
            }
            else if (moduleName == "VPM")
            {
                return "Performance Report";
            }
            else if (moduleName == "VFM")
            {
                return "Vendor Invoice";
            }
            else if (moduleName == "VSW")
            {
                return "Vendor SOW";
            }
            else if (moduleName == "VSL")
            {
                return "Vendor SLA";
            }
            else if (moduleName == "VCC")
            {
                return "Contract Change";
            }
            else if (moduleName == "OPM")
            {
                return "Opportunity";
            }
            else if (moduleName == "SVCConfig")
            {
                return "Service";
            }
            else if (moduleName == "SVC")
            {
                return "Ticket";
            }

            else if (moduleName == "TSR")
            {
                return "Ticket";
            }
            else if (moduleName == "ACR")
            {
                return "Ticket";//Application Change Request //BTS-22-000880
            }
            else if (moduleName == "DRQ")
            {
                return "Ticket";//Change    //BTS-22-000880
            }
            else if (moduleName == "CMT")
            {
                return "Contract";
            }
            else if (moduleName == "BTS")
            {
                return "Ticket";
            }
            else if (moduleName == "RCA")
            {
                return "Ticket";
            }
            else if (moduleName == "COM")
            {
                return "Company";
            }
            else if (moduleName == "CON")
            {
                return "Contact";
            }
            else if (moduleName == "LEM")
            {
                return "Lead";
            }
            else if (moduleName == "OPM")
            {
                return "Opportunity";
            }
            else
            {
                return "Item";//Ticket  //BTS-22-000880
            }
        }

        public static string moduleTypeName(int moduleID)
        {
            if (moduleID == 6) // NPR
            {
                return "Request";
            }
            else if (moduleID == 7) // PMM
            {
                return "Project";
            }
            else if (moduleID == 10) // CMDB
            {
                return "Asset";
            }
            else if (moduleID == 12) // TSL
            {
                return "Tasklist";
            }
            else if (moduleID == 11) // INC
            {
                return "Outage";
            }
            else if (moduleID == 16) // APP
            {
                return "Application";
            }
            else if (moduleID == 19) // VND
            {
                return "Vendor MSA";
            }
            else if (moduleID == 20) // VPM
            {
                return "Performance Report";
            }
            else if (moduleID == 21) // VFM
            {
                return "Vendor Invoice";
            }
            else if (moduleID == 22) // VSW
            {
                return "Vendor SOW";
            }
            else if (moduleID == 23) // VSL
            {
                return "Vendor SLA";
            }
            else if (moduleID == 24)
            {
                return "Contract Change";
            }
            else if (moduleID == 25)
            {
                return "Opportunity";
            }
            else
                return "Ticket";
        }

        public static string newTicketTitle(string moduleName)
        {
            if (moduleName == "CMDB")
            {
                return "New Asset";
            }
            else if (moduleName == "INC")
            {
                return "New Outage";
            }
            else if (moduleName == "TSK")
            {
                return "New Tasklist";
            }
            else if (moduleName == "APP")
            {
                return "New Application";
            }
            else if (moduleName == "CMT")
            {
                return "New Contract";
            }
            else if (moduleName == "VND")
            {
                return "New Vendor MSA";
            }
            else if (moduleName == "VPM")
            {
                return "New Performance Report";
            }
            else if (moduleName == "VFM")
            {
                return "New Vendor Invoice";
            }
            else if (moduleName == "VSW")
            {
                return "New Vendor SOW";
            }
            else if (moduleName == "VSL")
            {
                return "New Vendor SLA";
            }
            else if (moduleName == "VCC")
            {
                return "New Contract Change";
            }
            else if (moduleName == "OPM")
            {
                return "New Opportunity";
            }
            else if (moduleName == "TSR")
            {
                return "New Service Prime Ticketing System";
            }
            else if (moduleName == "SVC")
            {
                return "New Services Tickets";
            }
            else if (moduleName == "ACR")
            {
                return "New Application Change Request";
            }
            else if (moduleName == "DRQ")
            {
                return "New Change Management ";
            }
            else if (moduleName == "CMT")
            {
                return "New Contract Management";
            }
            else if (moduleName == "BTS")
            {
                return "New Bug Tracking System";
            }
            else if (moduleName == "RCA")
            {
                return "New Root cause Analysis";
            }
            else if (moduleName == "LEM")
            {
                return "New Lead";
            }
            else if (moduleName == "COM")
            {
                return "New Company";
            }
            else if (moduleName == "CON")
            {
                return "New Contact";
            }
            else if (moduleName == "CPR")
            {
                return "New Construction Project";
            }
            else if (moduleName == "CNS")
            {
                return "New Construction Service";
            }
            else
            {
                return string.Format("New {0} {1}", moduleName.ToUpper(), moduleTypeName(moduleName));
            }
        }

        public static string[] SplitString(object stringToBeSplit, string delimitor)
        {
            return SplitString(stringToBeSplit, delimitor, StringSplitOptions.None);
        }

        public static string[] SplitString(object stringToBeSplit, string[] delimitor)
        {
            if (stringToBeSplit == null || delimitor == null)
                return null;

            return Convert.ToString(stringToBeSplit).Split(delimitor, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitString(object stringToBeSplit, string[] delimitor, StringSplitOptions options)
        {
            if (stringToBeSplit == null || delimitor == null)
                return null;

            return Convert.ToString(stringToBeSplit).Split(delimitor, options);
        }

        public static List<string> ConvertStringToList(string inputValue, string separators)
        {
            return ConvertStringToList(inputValue, new string[] { separators });
        }

        public static string ConvertListToString(List<string> list, string seperator)
        {
            StringBuilder sValue = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sValue.AppendFormat("{0}", list[i]);
                    if (i != list.Count - 1)
                    {
                        sValue.Append(seperator);
                    }
                }
            }
            return sValue.ToString();
        }

        public static List<string> ConvertStringToList(string inputValue, string[] separators)
        {
            var outputList = new List<string>();
            if (string.IsNullOrEmpty(inputValue)) return outputList;

            var valueArray = inputValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            outputList.AddRange(from value in valueArray where !string.IsNullOrEmpty(value) select value.Trim());

            return outputList;
        }

        public static string[] SplitString(object stringToBeSplit, string delimitor, StringSplitOptions splitOption)
        {
            if (stringToBeSplit == null || string.IsNullOrEmpty(delimitor))
                return new string[0];

            return Convert.ToString(stringToBeSplit).Split(new string[] { delimitor }, splitOption);
        }

        public static string SplitString(object stringToBeSplit, string delimitor, int position)
        {
            string[] stringParts = SplitString(stringToBeSplit, delimitor);
            if (stringParts != null && position < stringParts.Length)
                return stringParts[position];
            else
                return null;
        }

        // 
        // Used to de-dupe email lists of the form a@b.com;c@b.com;a@b.com ...
        // 1) Splits input email list
        // 2) Copies list into new list while excluding dupes
        // 3) Joins list back into delimited string
        public static string RemoveDuplicateEmails(string emailList, string Separator = ",")
        {
            string[] list = SplitString(emailList, new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (list == null || list.Length == 0)
                return emailList; // Something went wrong!

            string[] listWithNoDupes = list.Distinct().ToArray();

            return String.Join(Separator, listWithNoDupes);
        }

        // 
        // Used to de-dupe delimited strings lists:
        // 1) Splits input string
        // 2) Copies list into new list while excluding dupes
        // 3) Joins list back into delimited string using first delimitor passed in
        public static string RemoveDuplicates(string inputList, string[] delimitors)
        {
            string[] list = SplitString(inputList, delimitors, StringSplitOptions.RemoveEmptyEntries);
            if (list == null || list.Length == 0)
                return inputList; // Something went wrong!

            string[] listWithNoDupes = list.Distinct().ToArray();

            return String.Join(delimitors[0], listWithNoDupes);
        }

        /// <summary>
        /// Remove Duplicate Cc emails that already exist in To list.
        /// </summary>        
        public static string RemoveDuplicateCcFromToEmails(string ToEmailList, string CcEmailList, string Separator = ",")
        {
            string[] ToList = SplitString(ToEmailList, new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries);

            string[] CcList = SplitString(CcEmailList, new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (CcEmailList == null || CcEmailList.Length == 0)
                return CcEmailList;

            CcList = CcList.Distinct().ToArray();

            string[] listWithNoDuplicates = CcList.Except(ToList).ToArray(); //CcList.Where(p => !ToList.Any(p2 => p2 == p));

            return String.Join(Separator, listWithNoDuplicates);
        }

        public static string GetProgressBar(string title, string bgColor, double maxWidth, double width, double barWidth)
        {
            /*double v1 = 9 * 100 / 9 = 100;
            double v2 = 0 * 100 / 9 = 100;
            double v3 = 1 * 100 / 9 = 11;*/
            if (bgColor == null || bgColor.Trim() == string.Empty)
            {
                bgColor = "#EAEAEA";
            }
            else if (!bgColor.Trim().StartsWith("#"))
            {
                bgColor = "#" + bgColor.Trim();
            }

            double acWidth = width * barWidth / maxWidth;
            if (acWidth > 100)
                acWidth = 100;
            double maxAcWidth = barWidth;
            return string.Format(@"<div style='position:relative;'><strong style='position:absolute;font-size:12px;left:2px;text-align:center;top:1px;'>{2}</strong><div style='float:left; width:{0}%;'><div style='float:left; width:{1}%;background:{3};height:20px;'>&nbsp;</div></div></div>", maxAcWidth, acWidth, title, bgColor);
        }

        public static string GetSPItemValueAsString(DataRow item, string columnName, bool removeIDs = false)
        {
            string value = Convert.ToString(GetSPItemValue(item, columnName));
            if (removeIDs)
                return RemoveIDsFromLookupString(value);
            else
                return value;
        }

        public static string GetSelfAssignButton()
        {
            string className = "usertick";
            string bar = string.Format(@"<div>
                                            <div class='{0}' title='You can self-assign this ticket' style='text-align: center; width:16px'> 
                                                &nbsp;
                                            </div>
                                         </div>  
                                        ", className);
            return bar;
        }

        /// <summary>
        /// To prevent erors in folder names, replaces any of these characters with an underscore: \ # ' "
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceInvalidCharsInFolderName(string input, int extensionLength = 0)
        {
            string fileName = Regex.Replace(input, @"[~#%&*{}\<>?/+|"":]", string.Empty, RegexOptions.IgnoreCase);
            Regex r = new Regex("[.][.]+");
            fileName = r.Replace(fileName, ".");

            //remove dot from end of file name if it is
            if (fileName.LastOrDefault() == '.')
                fileName = fileName.Remove(fileName.Length - 1);
            if (fileName.FirstOrDefault() == '.')
                fileName = fileName.Remove(0, 1);

            //SharePoint only supports file name upto 128 characters
            if (fileName.Length + extensionLength > 128)
            {
                string extension = Path.GetExtension(fileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                fileName = name.Substring(0, 128 - (extension.Length + extensionLength)) + extension;
            }

            // Remove any other invalid characters.
            var invalidChars = Path.GetInvalidFileNameChars();
            invalidChars = invalidChars.ToArray();
            if (fileName.IndexOfAny(invalidChars) > -1)
            {
                foreach (char item in invalidChars)
                {
                    fileName = fileName.Replace(item.ToString(), string.Empty);
                }
            }
            return fileName;
        }

        public static string Truncate(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Length <= maxLength ? input : input.Substring(0, maxLength);
        }

        public static string TruncateWithEllipsis(string input, int lengthRequired)
        {
            return TruncateWithEllipsis(input, lengthRequired, string.Empty);
        }

        public static string TruncateWithEllipsis(string input, int lengthRequired, string stopBefore, string ellipsis = "")
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string output = string.Empty;
            if (!string.IsNullOrEmpty(stopBefore))
                output = input.Split(new string[] { stopBefore }, StringSplitOptions.None)[0];
            else
                output = input;

            if (string.IsNullOrWhiteSpace(ellipsis))
                ellipsis = "...";

            if (output.Length > lengthRequired)
            {
                if (lengthRequired > ellipsis.Length)
                    output = string.Format("{0}{1}", output.Substring(0, lengthRequired - ellipsis.Length), ellipsis);
                else
                    output = output.Substring(0, lengthRequired);
            }
            return output;
        }

        public static string TruncateWithEllipsis(string input, int lengthRequired, string stopBefore)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string output = string.Empty;
            if (!string.IsNullOrEmpty(stopBefore))
                output = input.Split(new string[] { stopBefore }, StringSplitOptions.None)[0];
            else
                output = input;

            if (output.Length > lengthRequired && lengthRequired > 2)
                output = string.Format("{0}..", output.Substring(0, lengthRequired - 2));

            return output;
        }

        public static double StringToDouble(string value, double defaultValue)
        {
            double dblValue = StringToDouble(value);
            if (dblValue == 0)
                dblValue = defaultValue;

            return dblValue;
        }

        public static double StringToDouble(string value)
        {
            double dblValue = 0;
            if (!string.IsNullOrEmpty(value))
                double.TryParse(value.Trim(), out dblValue);

            return dblValue;
        }

        public static double StringToDouble(object value)
        {
            if (value == null)
                return 0;

            return StringToDouble(Convert.ToString(value));
        }

        //public static string GetModuleNameFromId(int moduleID)
        //{
        //    string moduleName = string.Empty;
        //    moduleName = 
        //    return moduleName;
        //}

        /// <summary>
        /// It get column value from spitem "columnName either be display name or internal name"
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnName">It takes either displayname or internal name of column</param>
        /// <returns></returns>
        public static object GetSPItemValue(string columnName)
        {
            try
            {
                //if (item != null && uHelper.IfColumnExists(columnName, item.ParentList))
                //{
                //    SPField spf = item.Fields.GetField(columnName);
                //    if (spf != null && item[spf.Title] != null)
                //    {
                //        return item[spf.Title];
                //    }
                //    else if (spf != null && item[columnName] != null)
                //    {
                //        return item[columnName];
                //    }
                //}
                return string.Empty;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                //string msg = string.Format("Error getting value of column {0} from list {1}", columnName, item.ParentList.Title);
                //Log.WriteException(ex, msg);
                return string.Empty;
            }
        }

        //public static string GetSPItemValueAsString(string columnName)
        //{
        //    return Convert.ToString(GetSPItemValue(row, columnName));
        //}

        /// <summary>
        /// Returns true if column exists in the spitem and is not null. columnName can be either display name or internal name.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columnName">It takes either displayname or internal name of column </param>
        /// <returns></returns>
        public static bool IsSPItemExist(string columnName)
        {
            try
            {
                //if (item != null && UGITUtility.IfColumnExists(columnName, item.Table))
                //{
                //    SPField spf = item.Fields.GetField(columnName);
                //    if (spf != null && item[spf.InternalName] != null)
                //        return true;
                //}
                return true;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                //Log.WriteException(ex, "Error getting value for column - " + columnName);
                return false;
            }
        }

        public static bool IfColumnExists(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return false;

            //if (thisList.Fields.ContainsField(columnName))
            //    return true;

            //try
            //{
            //    SPField spf = thisList.Fields.GetField(columnName);
            //    if (spf != null)
            //        return true;
            //}
            //catch (Exception)
            //{
            //    // Column does not exist in list
            //    //Log.WriteException(exp);
            //}

            return true;
        }

        public static bool IfColumnExists(DataRow row, string columnName)
        {
            return IfColumnExists(columnName, row.Table);
        }

        public static bool IfColumnExists(string columnName, DataTable thisList)
        {
            if (thisList == null || string.IsNullOrEmpty(columnName))
                return false;

            return thisList.Columns.Contains(columnName);
        }

        public static bool IfColumnExists(string columnName, string TableName)
        {
            string sql = "SELECT TOP (1) 1 FROM   INFORMATION_SCHEMA.COLUMNS (NOLOCK) WHERE  TABLE_NAME = '" + TableName + "'  AND COLUMN_NAME = '" + columnName + "'";
            return DBConnection.ExecuteReader(sql).HasRows;
        }

        public static string GetShortDateFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }

        public static string GetShortTimeFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        }

        public static string GetLongDateFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
        }
        public static string ChangeDateFormat(DateTime date, string dateFormat = "MM-dd-yyyy")
        {
            return (date.ToString(dateFormat));
        }
        public static DateTime GetObjetToDateTime(object date)
        {
            DateTime dateTime2;
            if (date == null)
                return dateTime2 = DateTime.MinValue;

            if (DateTime.TryParse(date.ToString(), out dateTime2))
            {
                return dateTime2;

            }
            else
            {
                dateTime2 = DateTime.MinValue;
            }
            return dateTime2;
        }
        public static string GetSystemDateTime()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        }

        // Convert date string to standard format
        public static string GetDateStringInFormat(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return string.Empty;

            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
                return GetDateStringInFormat(dateTime, !dateTime.TimeOfDay.Equals(TimeSpan.Zero)); // include time if not set to midnight :-)
            else
                return string.Empty;
        }

        public static string GetDateStringInFormat(string dateString, bool includeTime)
        {
            if (string.IsNullOrEmpty(dateString))
                return string.Empty;

            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
                return GetDateStringInFormat(dateTime, includeTime);
            else
                return string.Empty;
        }

        public static string GetDateStringInFormat(DateTime datetime, bool includeTime)
        {
            if (includeTime)
            {
                return datetime.ToString("MMM d, yyyy hh:mm tt");
            }
            else
            {
                return datetime.ToString("MMM d, yyyy");
            }
        }
        public static string GetCurrentTimestamp()
        {
            return DateTime.Now.ToString("MMM-dd-yyyy_HHmmss_ffff");
        }

        public static string FindAndConvertToAnchorTag(string convertMeToAnchor)
        {
            if (String.IsNullOrWhiteSpace(convertMeToAnchor))
                return string.Empty;

            string toachortag = string.Empty;
            string pattern = @"\b(https?|ftp|file)://[-A-Z0-9+&@/%?=~_|!:,.;]*[A-Z0-9+&@/%=~_|]";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Get a collection of matches.
            MatchCollection matches = Regex.Matches(convertMeToAnchor, pattern, RegexOptions.IgnoreCase);
            if (matches != null)
            {
                List<string> lstMatches = matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
                // Use foreach-loop.
                foreach (string match in lstMatches)
                {
                    toachortag = string.Format("<a href='{0}'>{0}</a>", match);
                    convertMeToAnchor = convertMeToAnchor.Replace(match, toachortag);
                }
            }
            return convertMeToAnchor;
        }

        public static int GetLookupID(string lookupField)
        {
            if (string.IsNullOrEmpty(lookupField))
                return -1;

            string[] delim = { Constants.Separator };
            try
            {
                return StringToInt(lookupField.Split(delim, StringSplitOptions.None)[0]);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                // Log.WriteException(ex);
                return -1;
            }
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="ContainerCtl"></param>
        /// <param name="IdToFind"></param>
        /// <returns></returns>
        public static Control FindControlRecursive(this Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);

                if (FoundCtl != null)
                    return FoundCtl;

            }
            return null;
        }

        public static Control FindControlIterative(this Control control, string id)
        {
            Control ctl = control;
            LinkedList<Control> controls = new LinkedList<Control>();
            try
            {
                if (ctl.ID == id)
                {
                    return ctl;
                }
                foreach (Control child in ctl.Controls)
                {
                    if (child.ID == id)
                    {
                        return child;
                    }
                    if (child.HasControls())
                    {
                        controls.AddLast(child);
                    }
                }

                if (controls != null && controls.Count > 0)
                {
                    Control userControl = controls.FirstOrDefault(x => x.ID == id);
                    if (userControl != null)
                        return userControl;
                    else
                    {
                        foreach (Control childControl in controls)
                        {
                            if (childControl != null && childControl.HasControls() && childControl.Controls.Count > 0)
                            {
                                foreach (Control lastchildControl in childControl.Controls)
                                {
                                    if (lastchildControl.ID == id)
                                        return lastchildControl;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
            return null;
        }

        public static bool IsDependentMandatoryField(string moduleName, string fieldInternalName)
        {
            switch (moduleName.ToLower())
            {
                case "prs":
                    if (fieldInternalName == DatabaseObjects.Columns.TicketActualHours
                        || fieldInternalName == DatabaseObjects.Columns.TicketResolutionComments
                        || fieldInternalName == DatabaseObjects.Columns.TicketTester)
                    { return true; }
                    break;
                case "tsr":
                    //if (fieldInternalName == DatabaseObjects.Columns.TicketActualHours
                    //    || fieldInternalName == DatabaseObjects.Columns.TicketResolutionComments
                    //    || fieldInternalName == DatabaseObjects.Columns.TicketTester
                    //    || fieldInternalName == DatabaseObjects.Columns.TicketGLCode
                    //    || fieldInternalName == DatabaseObjects.Columns.DepartmentLookup)
                    if (fieldInternalName == DatabaseObjects.Columns.TicketActualHours
                        || fieldInternalName == DatabaseObjects.Columns.TicketResolutionComments
                        || fieldInternalName == DatabaseObjects.Columns.TicketTester)
                    { return true; }
                    break;
                case "acr":
                    if (fieldInternalName == DatabaseObjects.Columns.TicketTester) // For "NoTest" workflow type
                    { return true; }
                    break;
                case "cmt":
                    if (fieldInternalName == DatabaseObjects.Columns.TicketFinanceManager
                        || fieldInternalName == DatabaseObjects.Columns.TicketLegal
                        || fieldInternalName == DatabaseObjects.Columns.TicketPurchasing)
                    { return true; }
                    break;
                case "vcc":
                    if (fieldInternalName == DatabaseObjects.Columns.VendorSOWLookup
                        || fieldInternalName == DatabaseObjects.Columns.VendorSOWNameLookup)
                    { return true; }
                    break;
                default:
                    return false;
            }
            return false;
        }

        public static string GetVersionString(string UserName, string versionDescription, DataRow versionFor, string internalName)
        {
            string oldVersionString = string.Empty;
            // Check for non-versioned value
            string prevValue = Convert.ToString(versionFor[internalName]);
            // In case the non-versioned value is empty we need to Check for versioned value
            if (prevValue == string.Empty && Convert.ToInt32(versionFor["ID"]) != 0 && versionFor.ItemArray.Count() != 0)
            {
                //prevValue = Convert.ToString(versionFor.Versions[0][internalName]);
            }
            if (prevValue != string.Empty)
                oldVersionString = prevValue + Constants.SeparatorForVersions;

            return oldVersionString + UserName + Constants.Separator
                        + Constants.UTCPrefix + DateTime.UtcNow.ToString() + Constants.Separator
                        + versionDescription;
        }

        public static string GetVersionString(string UserName, string versionDescription, string internalName)
        {
            string oldVersionString = string.Empty;
            // Check for non-versioned value
            string prevValue = Convert.ToString(internalName);
            // In case the non-versioned value is empty we need to Check for versioned value
            if (prevValue == string.Empty)//&& Convert.ToInt32(versionFor["ID"]) != 0 && versionFor.ItemArray.Count() != 0)
            {
                //prevValue = Convert.ToString(versionFor.Versions[0][internalName]);
            }
            if (prevValue != string.Empty)
                oldVersionString = prevValue + Constants.SeparatorForVersions;

            return oldVersionString + UserName + Constants.Separator
                        + Constants.UTCPrefix + DateTime.UtcNow.ToString() + Constants.Separator
                        + versionDescription;
        }

        public static string GetVersionString(string versionDescription, DataRow versionFor, string internalName)
        {
            string oldVersionString = string.Empty;

            // Check for non-versioned value
            string prevValue = Convert.ToString(versionFor[internalName]);
            if (prevValue != string.Empty)
                oldVersionString = prevValue + Constants.SeparatorForVersions;

            return oldVersionString + Constants.UTCPrefix + DateTime.UtcNow.ToString() + Constants.Separator
                        + versionDescription;
        }

        public static DateTime StringToDateTime(string value)
        {
            DateTime dateTimeValue = DateTime.MinValue;
            if (!string.IsNullOrEmpty(value))
                DateTime.TryParse(value.Trim(), out dateTimeValue);

            return dateTimeValue;
        }

        public static DateTime StringToDateTime(object value)
        {
            if (value == null)
                return DateTime.MinValue;

            return StringToDateTime(Convert.ToString(value));
        }

        public static DateTime GetDateTimeMinValue()
        {
            DateTime dateTime = DateTime.ParseExact("01/01/1753", "dd/MM/yyyy", null);
            return dateTime;
        }

        public static DateTime GetFirstMondayOfWeek(DateTime currentDate)
        {
            if (currentDate == null)
                return DateTime.MinValue;

            return currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
        }

        public static DateTime GetFirstDayOfPreviousMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
        }

        public static DateTime GetFirstDayOfPreviousMonth(DateTime currentDate)
        {
            if (currentDate == null)
                return DateTime.MinValue;

            return new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
        }

        public static DateTime GetLastDayOfNextMonth()
        {

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1);
        }

        public static DateTime GetLastDayOfNextMonth(DateTime currentDate)
        {
            if (currentDate == null)
                return DateTime.MinValue;

            return new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(2).AddDays(-1);
        }
        // NOTE: Cookie value is set in Response, but retreived from Request
        #region CookieHandling

        public static HttpCookie CreateCookie(HttpResponse response, string cookieName)
        {
            return CreateCookie(response, cookieName, null);
        }

        public static HttpCookie CreateCookie(HttpResponse response, string cookieName, string cookieValue)
        {
            // Create cookie with value
            HttpCookie cookie;
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Cookies.AllKeys.Contains(cookieName) || request.Cookies[cookieName].Value != cookieValue)
            {
                if (string.IsNullOrEmpty(cookieValue))
                    cookie = new HttpCookie(cookieName);    // empty cookie
                else
                    cookie = new HttpCookie(cookieName, cookieValue);
                // Set cookie path to distinguish between multiple sites in same domain
                cookie.Path = VirtualPathUtility.MakeRelative("~", HttpContext.Current.Request.Url.PathAndQuery).ToString();// SPContext.Current.Web.ServerRelativeUrl;
                // Used to enforce creation of session cookie
                cookie.Expires = DateTime.MinValue;

                response.Cookies.Add(cookie);
            }
            else
            {
                cookie = request.Cookies[cookieName];
            }
            return cookie;
        }

        #endregion CookieHandling

        /// <summary>
        /// Add space before word IF there are no embedded spaces already
        /// e.g. input: ModuleName, Output: Module Name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddSpaceBeforeWord(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (!value.Trim().Contains(' '))
                return Regex.Replace(value.Trim(), "((?<=\\p{Ll})\\p{Lu})|((?!\\A)\\p{Lu}(?>\\p{Ll}))", " $0");
            else
                return value.Trim();
        }

        public static Dictionary<DateTime, double> MonthlyDistributeFTEs(DateTime startDate, DateTime endDate, double FTE)
        {
            Dictionary<DateTime, double> monthlyDistribution = new Dictionary<DateTime, double>();
            do
            {
                DateTime firstDayOfMonth = UGITUtility.FirstDayOfMonth(startDate);
                DateTime lastDayOfMonth = UGITUtility.LastDayOfMonth(startDate);

                ///data creating for monthly Allocation for tempary basis
                DateTime monthEndDate = UGITUtility.LastDayOfMonth(startDate) >= endDate ? endDate : UGITUtility.LastDayOfMonth(startDate);
                //double workingdaysinmonth = UGITUtility.GetTotalWorkingDaysBetween(startDate, monthEndDate,"");
                //double totaldays = uHelper.GetTotalWorkingDaysBetween(firstDayOfMonth, lastDayOfMonth,"");
                // double pctAllocation = (workingdaysinmonth / totaldays) * FTE * 100;

                monthlyDistribution.Add(UGITUtility.FirstDayOfMonth(startDate), 12 / 100);

                startDate = UGITUtility.FirstDayOfMonth(startDate.AddMonths(1));

            } while (startDate <= endDate);

            return monthlyDistribution;
        }

        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static string WrapComment(string comment, string type)
        {
            if (string.IsNullOrEmpty(comment))
                return string.Empty;

            switch (type.ToLower())
            {
                case "reject":
                case "cancel":
                    comment = "[Cancel]: " + comment;
                    break;
                case "return":
                    comment = "[Return]: " + comment;
                    break;
                case "hold":
                    comment = "[Hold]: " + comment;
                    break;
                case "unhold":
                    comment = "[Remove Hold]: " + comment;
                    break;
                case "reopen":
                    comment = "[Re-Open]: " + comment;
                    break;
                default:
                    break;
            }
            return comment;
        }

        public static string WrapCommentForEmail(string comment, string type)
        {
            return WrapCommentForEmail(comment, type, DateTime.MinValue, string.Empty);
        }

        public static string WrapCommentForEmail(string comment, string type, DateTime holdTillDate, string holdReason)
        {
            if (string.IsNullOrEmpty(comment) && type.ToLower() != "hold")
                return string.Empty;

            switch (type.ToLower())
            {
                case "reject":
                case "cancel":
                    comment = "<b>Cancel Comment</b>: " + comment;
                    break;
                case "return":
                    comment = "<b>Return Comment</b>: " + comment;
                    break;
                case "hold":
                    if (string.IsNullOrEmpty(comment))
                        comment = string.Format("<b>On Hold Till</b>: {0}<br/><b>Hold Reason</b>: {1}", holdTillDate.ToString("MMM-dd-yyyy"), holdReason);
                    else
                        comment = string.Format("<b>On Hold Till</b>: {0}<br/><b>Hold Reason</b>: {1}<br/><b>Comment</b>: {2}", holdTillDate.ToString("MMM-dd-yyyy"), holdReason, comment);
                    break;
                case "unhold":
                    comment = "<b>Remove Hold Comment</b>: " + comment;
                    break;
                case "reopen":
                    comment = "<b>Re-Open Comment</b>: " + comment;
                    break;
                default:
                    break;
            }
            return comment;
        }

        public static string ReturnInsertQuery(DataRow item, string tableName)
        {
            string ColumnString = "Insert into " + tableName + "(";
            string ValueString = " output INSERTED.* Values(";

            DataTable dtSaveTicket = item.Table.Clone();
            dtSaveTicket.Rows.Add(item.ItemArray);
            for (int i = 0; i < dtSaveTicket.Columns.Count; i++)
            {
                string columnName = (dtSaveTicket.Columns[i]).ColumnName;
                string value = Convert.ToString(dtSaveTicket.Rows[0][i]);
                if (value != string.Empty && columnName != DatabaseObjects.Columns.Id)
                {
                    if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.String" || (dtSaveTicket.Columns[i]).DataType.ToString() == "System.DateTime")
                        ValueString = ValueString + "'" + value + "',";
                    else if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.Int64" || (dtSaveTicket.Columns[i]).DataType.ToString() == "System.Int32")
                        ValueString = ValueString + value + ",";
                    //else if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.DateTime" || (dtSaveTicket.Columns[i]).DataType.ToString() == "System.Int32")
                    //    ValueString = ValueString + value + ",";

                    ColumnString += columnName + ",";
                }
            }
            string QueryString = ColumnString.Remove(ColumnString.Length - 1, 1) + ")" + ValueString.Remove(ValueString.Length - 1, 1) + ")";
            return QueryString;
        }

        public static string ReturnUpdateQuery(DataRow item, string tableName)
        {
            string innerQuery = string.Empty;
            string fullQuery = string.Empty;
            DataTable dtSaveTicket = item.Table.Clone();
            dtSaveTicket.Rows.Add(item.ItemArray);
            for (int i = 0; i < dtSaveTicket.Columns.Count; i++)
            {
                string columnName = (dtSaveTicket.Columns[i]).ColumnName;
                string value = Convert.ToString(dtSaveTicket.Rows[0][i]);
                if (value != string.Empty && columnName != "ID")
                {

                    if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.String")
                        innerQuery = innerQuery + ", " + columnName + " = " + "'" + value + "'";
                    else if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.Int64" || (dtSaveTicket.Columns[i]).DataType.ToString() == "System.Int32"
                        || (dtSaveTicket.Columns[i]).DataType.ToString() == "System.Double")
                        innerQuery = innerQuery + ", " + columnName + " = " + value;
                    else if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.DateTime")
                        innerQuery = innerQuery + ", " + columnName + " = " + "Convert(varchar,'" + value + "',105)";
                    else if ((dtSaveTicket.Columns[i]).DataType.ToString() == "System.Boolean")
                        innerQuery = innerQuery + ", " + columnName + " = " + "'" + value + "'";
                }
            }
            innerQuery = innerQuery.Remove(0, 1);
            if (tableName == DatabaseObjects.Tables.GlobalRole)
                fullQuery = "Update " + tableName + " Set " + innerQuery + " output INSERTED.* Where Id = '" + item[DatabaseObjects.Columns.Id] + "'";
            else
                fullQuery = "Update " + tableName + " Set " + innerQuery + " output INSERTED.* Where Id = " + item[DatabaseObjects.Columns.Id];
            return fullQuery;
        }

        public static string GetAbsoluteUrltForModule(string moduleName)
        {
            //DataTable mTable = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules);
            //DataRow[] mRows = mTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, moduleName));
            //if (mRows.Length > 0)
            //    return uHelper.GetAbsoluteURL(mRows[0][DatabaseObjects.Columns.ModuleRelativePagePath].ToString());
            //else
            //    return SPContext.Current.Web.Url.ToString();
            return string.Empty;
        }

        public static string GetPostfixOnColumnType(string columntype)
        {
            string retString = string.Empty;
            switch (columntype)
            {
                case "Percentage":
                    retString = "%";
                    break;
                default:
                    break;

            }
            return retString;
        }

        public static string GetDueIn(DateTime desiredCompletionDate)
        {
            string className = "progressbar";
            int dueIn = ((desiredCompletionDate - DateTime.Now).Days + 1);
            if (dueIn < 0)
            {
                className = "progressbarhold";
            }
            else if (dueIn > 0 && dueIn < 5)
            {

            }
            else if (dueIn > 5)
            {

            }
            string bar = string.Format(@"<div style='width:25px' title='Due Date: " + desiredCompletionDate.ToString("MMM-d-yyyy") + @"'>
                                            <div class='{0}' style='font-size:90%; text-align: center;'> 
                                                {1}
                                            </div>
                                         </div>  
                                        ", className, dueIn);
            return bar;
        }

        public static int GetDueInValue(DateTime desiredCompletionDate)
        {
            string className = string.Empty;
            className = "progressbar";
            // add above line to remove warning message 
            if (desiredCompletionDate == DateTime.MinValue || desiredCompletionDate == System.Data.SqlTypes.SqlDateTime.MinValue)
                desiredCompletionDate = DateTime.Now;
            int dueIn = ((DateTime.Now - desiredCompletionDate).Days + 1);
            if (dueIn < 0)
            {
                className = "progressbarhold";
            }
            else if (dueIn > 0 && dueIn < 5)
            {

            }
            else if (dueIn > 5)
            {

            }

            return dueIn;
        }



        /// <summary>
        /// Create Age bar for ticket. It calculates the difference between createdate and current date.
        /// If desiredcopmletiondate is less then current then the makes it red otherwise makes it green
        /// </summary>
        /// <param name="creationDate"></param>
        /// <param name="desiredCompletionDate"></param>
        /// <returns></returns>
        public static string GetValueBar(DateTime creationDate, DateTime desiredCompletionDate)
        {
            string className = "progressbar";
            int age = 0;

            if (creationDate != null)
                age = (DateTime.Now - creationDate).Days;

            if (desiredCompletionDate != null && desiredCompletionDate != DateTime.MinValue && desiredCompletionDate.Date < DateTime.Now.Date)
                className = "progressbarhold";

            string bar = string.Format(@"<div class='homeGrid_ageWrap' title='Created On: " + creationDate.ToString("MMM-d-yyyy") + @"'>
                                            <div class='{0}' style='font-size:90%; text-align: center;'> 
                                                {1}
                                            </div>
                                         </div>  
                                        ", className, age);
            return bar;
        }

        /// <summary>
        /// This function is used to create Age bar for tickets. It accepts the ticket age as a parameter which is already calculated.
        /// If desiredcopmletiondate is less then current then the makes it red otherwise makes it green
        /// </summary>
        /// <param name="dRow">Data row containing the details to calculate TicketAge</param>
        /// <param name="ticketAge">contains TicketAge</param>
        /// <returns></returns>
        public static string GetAgeBar(DataRow dRow, bool ageColorByTargetCompletion, double ticketAge = -1, string colNamePrefix = "")
        {
            // Find ticket creation date, close date and desired completion date
            DateTime desiredCompletionDate = DateTime.MinValue;

            if (ageColorByTargetCompletion)
            {
                if (dRow.Table.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketTargetCompletionDate))
                    DateTime.TryParse(Convert.ToString(dRow[colNamePrefix + DatabaseObjects.Columns.TicketTargetCompletionDate]), out desiredCompletionDate);
            }
            else
            {
                if (dRow.Table.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketDesiredCompletionDate))
                    DateTime.TryParse(Convert.ToString(dRow[colNamePrefix + DatabaseObjects.Columns.TicketDesiredCompletionDate]), out desiredCompletionDate);
            }

            DateTime creationDate = DateTime.MinValue;
            if (dRow.Table.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketCreationDate))
                DateTime.TryParse(Convert.ToString(dRow[colNamePrefix + DatabaseObjects.Columns.TicketCreationDate]), out creationDate);

            DateTime? closedDate = null;
            if (dRow.Table.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketClosed) && dRow.Table.Columns.Contains(colNamePrefix + DatabaseObjects.Columns.TicketCloseDate) &&
                StringToBoolean(dRow[colNamePrefix + DatabaseObjects.Columns.TicketClosed]))
            {
                closedDate = UGITUtility.StringToDateTime(dRow[colNamePrefix + DatabaseObjects.Columns.TicketCloseDate]);
            }

            // Set the css class to show the Age bar color
            string className = "progressbar";
            DateTime endDate = DateTime.Now;

            if (creationDate != null && creationDate != DateTime.MinValue && creationDate != DateTime.MaxValue)
            {
                if (closedDate.HasValue && closedDate != DateTime.MinValue && closedDate != DateTime.MaxValue)
                    endDate = Convert.ToDateTime(closedDate);
            }

            if (desiredCompletionDate != null && desiredCompletionDate != DateTime.MinValue && desiredCompletionDate.Date < endDate.Date)
                className = "progressbarhold";

            string bar = string.Format(@"<div style='width:27px' title='Created On: " + creationDate.ToString("MMM-d-yyyy") + @"'>
                                            <div class='{0}' style='font-size:90% !important; text-align: center; padding-top: 6px;'> 
                                                {1}
                                            </div>
                                         </div>", className, ticketAge);
            return bar;
        }

        /// <summary>
        /// Create Progress bar for ticket. Calculate progress based on stageweight. 
        /// if ticket isOnHold field is true then shows red bar otherwise shos green bar.
        /// </summary>
        /// <param name="stageCollection"></param>
        /// <param name="currentStage"></param>
        /// <param name="isOnHold"></param>
        /// <param name="hideStatusOverProgressBar"></param>
        /// <param name="stageTitle"></param>
        /// <param name="requiredFixWidth"></param>
        /// <returns></returns>
        public static string GetProgressBar(LifeCycle lifeCycle, LifeCycleStage currentStage, DataRow dataRow, string fieldName, bool hideStatusOverProgressBar, bool requiredFixWidth)
        {
            if (lifeCycle == null || lifeCycle.Stages.Count <= 0)
                return string.Empty;

            if (currentStage == null)
                return string.Empty;

            string status = string.Empty;
            if (fieldName == DatabaseObjects.Columns.TicketStatus && dataRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
                status = Convert.ToString(dataRow[DatabaseObjects.Columns.TicketStatus]); // TicketStatus field passed in
            else
                status = currentStage.Name; // ModuleStepLookup field passed in

            // Show short stage title if configured AND if we don't have an artificial status like "Returned"
            if (!string.IsNullOrEmpty(currentStage.ShortStageTitle) && currentStage.Name.ToLower() == status.ToLower())
                status = currentStage.ShortStageTitle;

            double totalWeight = lifeCycle.Stages.Sum(x => x.StageWeight);
            double completeStageWeight = lifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
            if (totalWeight <= 0)
            {
                totalWeight = lifeCycle.Stages.Count();
                completeStageWeight = currentStage.StageStep;
            }

            string percCompleteDiv = string.Empty;
            double percentage = 0;
            string progressBarClass = "progress-bar";
            //string emptyProgressBarClass = "emptyProgressBar";
            string widthStyle = "width:99%; min-width:150px;";
            if (requiredFixWidth)
                widthStyle = "width:150px;";

            //if (dataRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketOnHold) &&
            //    StringToBoolean(dataRow[DatabaseObjects.Columns.TicketOnHold]))
            //    progressBarClass = "progressbarhold";

            bool onHold = dataRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketOnHold) && UGITUtility.StringToBoolean(dataRow[DatabaseObjects.Columns.TicketOnHold]);
            bool cancelled = (dataRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketRejected) && UGITUtility.StringToBoolean(dataRow[DatabaseObjects.Columns.TicketRejected])) || status == Constants.Cancelled;

            if (onHold)
            {
                progressBarClass = "progressbar-hold";
                status = Constants.OnHoldStatus;
            }
            else if (cancelled)
                progressBarClass = "progressbar-hold"; // Leave status alone in case we have a custom reject status configured in workflow

            percentage = Math.Round((completeStageWeight / totalWeight) * 100, 2);
            /*
            percCompleteDiv = @"<div class='" + emptyProgressBarClass + @"' style='float:left; position:relative; " + widthStyle + @"'>
                                        <div class='" + progressBarClass + "' style='float:left; position:absolute; width:" + percentage + @"%;'>&nbsp;</div>";
            if (!hideStatusOverProgressBar)
            {
                percCompleteDiv += "<div style='float:left; font-size:90%; position:absolute;'>&nbsp;" + status + @"</div>";
            }
            percCompleteDiv += "</div>";
            */

            if (!hideStatusOverProgressBar)
            {
                percCompleteDiv = $"<span style='float:left;font-size:100%;'>{status}</span><br>";
            }

            percCompleteDiv += $"<div style='{widthStyle}'><div class='progress xs'>" +
                                $"<div class='{progressBarClass}' role='progressbar' aria-valuenow='20' aria-valuemin='0' aria-valuemax='100' title='{percentage}%' style='width: {percentage}%;'>" +
                                $"</div>" +
                                $"</div></div>";
            return percCompleteDiv;
        }

        public static string GetProgressBar(string title, int pctComplete, bool isRed)
        {
            return GetProgressBar(title, pctComplete, isRed, new Unit("100%"));
        }

        public static string GetProgressBar(string title, int pctComplete, bool isRed, Unit width)
        {
            StringBuilder bar = new StringBuilder();

            float percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "emptyProgressBar";

            if (width != null && width.Value < 0)
            {
                width = new Unit("100%");
            }

            if (percentage > 100)
            {
                percentage = 100;
            }

            if (isRed)
            {
                progressBarClass = "progressbarhold";
            }
            bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:{4};text-align:center;top:1px;'>{3}</strong><div class='{0}' style='float:left; width:{4};'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, pctComplete, title, width.ToString());

            return bar.ToString();
        }

        public static string GetPriorityStatus(string status, HorizontalAlign horizontalAlign)
        {
            string align = Convert.ToString(horizontalAlign);
            if (horizontalAlign == HorizontalAlign.Center)
                align = "none";
            string statusDiv = "";
            var splitstatus = status.Split('-');
            var index = splitstatus.Count() > 1 ? 1 : 0;
            switch (splitstatus[index]?.ToLower())
            {
                case "critical":
                    string classCritical = "";
                    if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
                    {
                        classCritical = "mobilePriorityCritical";
                    }
                    statusDiv = $@"<div class='priorityCritical " + classCritical + "'" + " style='float:" + align + "; min-width:85%;'></div>";
                    break;
                case "high":
                    string classHigh = "";
                    if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
                    {
                        classHigh = "mobilePriorityHigh";
                    }
                    statusDiv = $@"<div class='priorityHigh  " + classHigh + " ' " + " style='float:" + align + "; min-width:85%;'></div>";
                    break;
                case "medium":
                    string classMedium = "";
                    if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
                    {
                        classMedium = "mobilePriorityMedium";
                    }
                    statusDiv = $@"<div class='priorityMedium " + classMedium + " ' " + " style='float:" + align + "; min-width:85%;'></div>";
                    break;
                case "low":
                    string classLow = "";
                    if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
                    {
                        classLow = "mobilePriorityLow";
                    }
                    statusDiv = $@"<div class='priorityLow " + classLow + " ' " + " style='float:" + align + "; min-width:85%;'></div>";
                    break;
                default:
                    statusDiv = status;
                    break;
            }
            return statusDiv;
        }

        public static void OpenPopup(HttpContext context, string url, string title, bool stopRefresh)
        {
            StringBuilder script = new StringBuilder();
            string sourceUrl = string.Empty;
            if (context.Request["source"] != null && context.Request["source"].Trim() != string.Empty)
            {
                sourceUrl = context.Request["source"].Trim();
            }
            script.AppendFormat("window.parent.setTimeout(\"window.parent.UgitOpenPopupDialog('{0}','','{1}','90','90','{2}', {3})\", 500);", url, title, sourceUrl, stopRefresh ? "true" : "false");
            script.Append("window.frameElement.commitPopup(\"" + sourceUrl + "\");");
            context.Response.Write(string.Format("<script type='text/javascript'>{0}</script>", script.ToString()));
            context.Response.Flush();
            //crash here... so i have modify for change ticket type. 
            //context.Response.End();
            context.ApplicationInstance.CompleteRequest();
        }

        public static void AppendWithSeparator(ref string outputText, string textToAppend, string separator)
        {
            if (separator == null)
                separator = string.Empty;

            if (string.IsNullOrEmpty(outputText))
                outputText = textToAppend;
            else if (!string.IsNullOrEmpty(textToAppend))
                outputText += separator + textToAppend;
        }

        public static string getDateStringInFormat(DataRow item, string fieldName, bool withTime)
        {
            if (withTime)
            {
                return ((DateTime)item[fieldName]).ToString("MMM-d-yyyy hh:mm tt");
            }
            else
            {
                return ((DateTime)item[fieldName]).ToString("MMM-d-yyyy");
            }
        }

        public static string getDateStringInFormat(DateTime date, bool withTime)
        {
            if (withTime)
            {
                return date.ToString("MMM-d-yyyy hh:mm tt");
            }
            else
            {
                return date.ToString("MMM-d-yyyy");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"> Dictionary keys: ProjectID, Title, Description, StartDate, EndDate, TaskEstimatedHours</param>
        /// <param name="ticketUrl"></param>
        /// <param name="HTML"></param>
        /// <param name="disableEmailTicketLink"></param>
        /// <returns></returns>
        public static string GetTaskDetailsForEmailFooter(Dictionary<string, string> task, string ticketUrl, bool HTML, bool disableEmailTicketLink, string tennantID, string ShowTab=null)
        {
            string projectID = string.Empty;
            if (task.Keys.Contains("ProjectID") && !string.IsNullOrEmpty(task["ProjectID"]))
                projectID = task["ProjectID"];

            bool isIssue = false;
            if (task.Keys.Contains("IsIssue") && !string.IsNullOrEmpty(task["IsIssue"]))
                isIssue = UGITUtility.StringToBoolean(task["IsIssue"]);

            string type = "Project";
            if (task.Keys.Contains("IsService") && UGITUtility.StringToBoolean(task["IsService"]))
                type = "Service";

            string typeKey = task.Keys.FirstOrDefault(x => x.ToLower() == "type");
            if (!string.IsNullOrEmpty(typeKey))
                type = task[typeKey];

            StringBuilder formattedTaskDetails = new StringBuilder();
            string htmlBoldStart = HTML ? "<b>" : string.Empty;
            string htmlBoldEnd = HTML ? "</b>" : string.Empty;
            string htmlBreak = HTML ? "<br>" : "\r\n";
            string fieldFormat = "{0}:{1} \n\r";
            if (HTML)
            {
                fieldFormat = "<tr><td style='background:none repeat scroll 0 0 #E8F5F8; font-weight:bold; text-align:right; width:190px; vertical-align:top;'>{0}</td><td style='background:none repeat scroll 0 0 #FBFBFB; padding:3px 6px 4px; vertical-align:top;'>{1}</td></tr>";
            }

            formattedTaskDetails.Append(htmlBreak);
            if (HTML)
                formattedTaskDetails.Append("<hr>");
            else
                formattedTaskDetails.AppendFormat("_________________________________________________________{0}", htmlBreak);

            //Check for null because not all modules have all the fields we want.
            string projectTitle = projectID;
            if (task.Keys.Contains("ProjectTitle") && !string.IsNullOrEmpty(task["ProjectTitle"]))
                projectTitle += " " + task["ProjectTitle"];

            string moduleName = string.Empty;
            if (!string.IsNullOrEmpty(projectTitle) && projectTitle.Contains('-'))
            {
                moduleName = projectTitle.Split('-')[0];
            }
            string projectUrl = string.Format("{0}?TicketId={1}&ModuleName={2}&Tid={3}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), projectID, moduleName, tennantID);

            if (HTML && !string.IsNullOrEmpty(projectUrl))
                projectTitle = string.Format("<a target='_blank' href='{1}'>{0}</a>", projectTitle, projectUrl);
            else
                projectTitle = ReplaceInvalidCharsInURL(projectTitle);

            formattedTaskDetails.AppendFormat("{0}{4} Details for {5}: {3}{1}{2}{2}",
                                                htmlBoldStart, htmlBoldEnd, htmlBreak, projectTitle, isIssue ? "Issue" : "Task", type);

            if (HTML)
                formattedTaskDetails.AppendFormat("<Table style='border: 1px solid #A5A5A5;'>");

            if (task.Keys.Contains("Title") && !string.IsNullOrEmpty(task["Title"]))
            {
                string title = task["Title"];

                string taskIssueURL = string.Empty;
                if (type == "Service")
                    taskIssueURL = string.Format("{0}&showTab={1}", ticketUrl, "1");
                else if (isIssue)
                    taskIssueURL = string.Format("{0}&showTab={1}", ticketUrl, "3");
                else if(ShowTab != null)
                    taskIssueURL=string.Format("{0}&showTabFromEmail={1}", ticketUrl, ShowTab);
                else
                    taskIssueURL = string.Format("{0}&showTab={1}", ticketUrl, "2");

                if (HTML && !string.IsNullOrEmpty(ticketUrl))
                    title = string.Format("<a target='_blank' href='{1}'>{0}</a>", title, taskIssueURL);
                else
                    title = ReplaceInvalidCharsInURL(title);

                formattedTaskDetails.AppendFormat(fieldFormat, isIssue ? "Issue " : "Task " + "Title", title, string.Empty);
            }

            if (task.Keys.Contains("StartDate") && !string.IsNullOrEmpty(task["StartDate"]))
            {
                string dateString = Convert.ToDateTime(task["StartDate"]).ToString("MMM-d-yyyy");
                formattedTaskDetails.AppendFormat(fieldFormat, "Start Date", dateString, string.Empty);
            }

            if (task.Keys.Contains("DueDate") && !string.IsNullOrEmpty(task["DueDate"]))
            {
                string dateString = Convert.ToDateTime(task["DueDate"]).ToString("MMM-d-yyyy");
                formattedTaskDetails.AppendFormat(fieldFormat, "Due Date", dateString, string.Empty);
            }

            if (task.Keys.Contains("ProposedDueDate") && !string.IsNullOrEmpty(task["ProposedDueDate"]))
            {
                string dateString = Convert.ToDateTime(task["ProposedDueDate"]).ToString("MMM-d-yyyy");
                formattedTaskDetails.AppendFormat(fieldFormat, "Proposed Due Date", dateString, string.Empty);
            }

            if (task.Keys.Contains("EstimatedHours") && !string.IsNullOrEmpty(task["EstimatedHours"]))
            {
                double estimatedHours = Convert.ToDouble(task["EstimatedHours"]);
                formattedTaskDetails.AppendFormat(fieldFormat, "Estimated Hours", estimatedHours, string.Empty);
            }

            if (task.Keys.Contains("Status") && !string.IsNullOrEmpty(task["Status"]))
            {
                string status = task["Status"];
                string pctComplete = string.Empty;
                if (task.Keys.Contains("% Complete") && !string.IsNullOrEmpty(task["% Complete"]))
                    pctComplete = " (" + task["% Complete"] + "%)";

                if (!HTML)
                    status = ReplaceInvalidCharsInURL(status);

                formattedTaskDetails.AppendFormat(fieldFormat, "Status", status, string.Empty);
            }

            if (task.Keys.Contains("Description") && !string.IsNullOrEmpty(task["Description"]))
            {
                string description = task["Description"];
                if (!HTML)
                    description = ReplaceInvalidCharsInURL(description);

                formattedTaskDetails.AppendFormat(fieldFormat, "Detailed Description", description, string.Empty);
            }
            if (HTML)
                formattedTaskDetails.AppendFormat("</Table>");

            return formattedTaskDetails.ToString();
        }

        public static string GetMonitorsGraphic(DataRow monitor)
        {
            StringBuilder monitorHtml = new StringBuilder();
            monitorHtml.Append("<div>");
            monitorHtml.AppendFormat("<span style='margin: 0 8px 0 8px;' class='{0} monitoricon'>", UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup));
            monitorHtml.AppendFormat("<span style='display:none;' class='info'>{0} - {1}{2}</span>",
                UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ModuleMonitorName), UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ModuleMonitorOptionNameLookup),
                UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ProjectMonitorNotes).ToString() != string.Empty ? string.Format("<br>Note: {0}", UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ProjectMonitorNotes)) : string.Empty);

            monitorHtml.Append("</span>");
            monitorHtml.Append("</div>");

            return monitorHtml.ToString();
        }

        /// <summary>
        /// Returns a static version number for each CS or JS file. 
        /// Manually increment the version number here whenever the file is changed 
        /// to force browsers to reload the file instead of using the cached version.
        /// </summary>
        /// <param name="pathName">Filename, optionally including path. Only filename portion is used.</param>
        /// <returns>Static version</returns>
        public static string GetFileVersion(string pathName)
        {
            string fileName = Path.GetFileName(pathName).ToLower();
            string version;

            switch (fileName)
            {
                // CSS Files
                case "dmcommon.css":
                    version = "1";
                    break;
                case "ugitcommon.css":
                    version = "1";
                    break;
                case "ugitstagegraphic.css":
                    version = "1";
                    break;
                case "ugitwizard.css":
                    version = "1";
                    break;

                // JS Files
                case "edmcommon.js":
                    version = "1";
                    break;
                case "ugitcommon.js":
                    version = "1";
                    break;
                case "ugitdashboard.js":
                    version = "1";
                    break;

                // Default value if not found
                default:
                    version = "1";
                    break;
            }

            //Log.WriteLog(string.Format("GetFileVersion('{0}') = '{1}'", fileName, version));

            return version;
        }

        public static string ReplaceImageSrcWithAbsoluteUrl(string contentText)
        {
            string html = contentText;
            // Now create an Html Agile Doc object (using HTML Agility Pack
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // If not able to parse as HTML, send back as-is since it may be plain-text
            // Should return even if one error but sometimes Outlook has a few malformed tags
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                return contentText;

            string temString = string.Empty;

            var imgNode = htmlDoc.DocumentNode.SelectNodes("//img");
            // Replace img src to absolute url
            if (imgNode != null)
            {
                foreach (HtmlNode node in imgNode)
                {
                    HtmlAttribute srcAttr = node.Attributes["src"];
                    if (srcAttr != null && !string.IsNullOrWhiteSpace(srcAttr.Value))
                        node.SetAttributeValue("src", GetAbsoluteURL(srcAttr.Value));
                }
            }
            temString = htmlDoc.DocumentNode.InnerHtml;

            // Finally - return the inner html which will have our inserted line-breaks in it minus all the tags
            // Decode &nbsp; and other html-encoded strings with actual spaces
            temString = HttpUtility.HtmlDecode(temString);

            return temString;
        }

        public static bool IsNumber(string inputValue, out long result)
        {
            return Int64.TryParse(inputValue, out result);
        }

        public static bool IsPropertyExists(object obj, string PropertyName)
        {
            return ((Type)obj.GetType()).GetProperties().Where(p => p.Name.Equals(PropertyName)).Any();
        }

        public static void SetPropertyValue(object obj, string PropertyName, string Value)
        {
            PropertyInfo pp = obj.GetType().GetProperty(PropertyName);
            if (pp != null)
                pp.SetValue(obj, Value);
        }

        public static string GetPredecessors(DataTable moduleList, string PredIds)
        {
            string PredItemOrders = string.Empty;
            string ItemOrder = string.Empty;

            if (!string.IsNullOrEmpty(PredIds) && moduleList != null && moduleList.Rows.Count > 0)
            {
                string[] values = UGITUtility.SplitString(PredIds, Constants.Separator6, StringSplitOptions.RemoveEmptyEntries);
                DataTable dt = new DataTable();
                DataRow[] dataRows = null;
                foreach (var item in values.OrderBy(x => x))
                {
                    dataRows = moduleList.Select($"{DatabaseObjects.Columns.ID}={item}");
                    if (dataRows != null && dataRows.Length > 0)
                        dt = dataRows.CopyToDataTable();
                    //Convert.ToString(moduleList.AsEnumerable().Where(x =>x.Field<long>(DatabaseObjects.Columns.ID) == UGITUtility.StringToLong(item)).Select(x => x.Field<int>("ItemOrder")).FirstOrDefault());
                    if (dt != null && dt.Rows.Count > 0)
                        ItemOrder = UGITUtility.ObjectToString(dt.Rows[0][DatabaseObjects.Columns.ItemOrder]);
                    PredItemOrders = PredItemOrders + ItemOrder + ",";
                }
                PredItemOrders = PredItemOrders.TrimEnd(',');
            }

            return PredItemOrders;
        }

        public static string GetPredecessorValues(List<UGITTask> Tasks, string PredIds)
        {
            string PredLabels = string.Empty;
            string Item = string.Empty;
            if (!string.IsNullOrEmpty(PredIds) && Tasks != null && Tasks.Count > 0)
            {
                string[] values = UGITUtility.SplitString(PredIds, Constants.Separator6, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in values)
                {
                    Item = Tasks.AsEnumerable().Where(x => x.ID == Convert.ToInt64(item)).Select(x => x.ItemOrder + " - " + x.Title).FirstOrDefault();
                    PredLabels = PredLabels + Item + System.Environment.NewLine;
                }
            }
            return PredLabels;
        }

        public static bool IsOnboardingUIRequest()
        {
            try
            {
                if (HttpContext.Current.Request["requestPage"] != null)
                {
                    return HttpContext.Current.Request["requestPage"].Trim().ToLower() == Convert.ToString(ConfigurationManager.AppSettings["OnBoardingLoggingURL"]);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return false;
        }


        public static bool IsOnboardingUIRequestType()
        {
            try
            {
                if (HttpContext.Current.Request["type"] != null)
                {
                    return HttpContext.Current.Request["type"].Trim().ToLower() == Convert.ToString(ConfigurationManager.AppSettings["OnBoadingLoggingType"]);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return false;
        }

        /// <summary>
        /// Generate XML of where expressions either in "Aad" OR in "Or"
        /// </summary>
        /// <param name="queryExpression">List of string containing query expresssion</param>
        /// <param name="useAnd">Use "And" or "Or"</param>
        /// <returns></returns>
        public static string GenerateWhereQueryWithAndOr(List<string> queryExpression, bool useAnd)
        {
            return GenerateWhereQueryWithAndOr(queryExpression, queryExpression.Count - 1, useAnd);
        }

        /// <summary>
        /// Generate XML of where expressions either in "Aad" OR in "Or"
        /// </summary>
        /// <param name="queryExpression">List of string containing query expresssion</param>
        /// <param name="startIndex">start index of list</param>
        /// <param name="useAnd">Use "And" or "Or"</param>
        /// <returns></returns>
        public static string GenerateWhereQueryWithAndOr(List<string> queryExpression, int endIndex, bool useAnd)
        {
            string type = "Or";
            if (useAnd)
            {
                type = "And";
            }
            StringBuilder query = new StringBuilder();
            if (queryExpression.Count > 0)
            {
                if (queryExpression.Count > 1)
                {
                    query.AppendFormat(queryExpression[endIndex]);
                    if (endIndex != 0) { query.AppendFormat(" {0} ", type); }
                    endIndex = endIndex - 1;
                    if (endIndex >= 0)
                    {
                        query.Append(GenerateWhereQueryWithAndOr(queryExpression, endIndex, useAnd));
                    }
                }
                else
                {
                    query.AppendFormat(queryExpression[endIndex]);
                }
                //if (endIndex != 0) { query.AppendFormat("</{0}>", type); }
            }
            return query.ToString();
        }

        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue(Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static string GetDelegateUrl(string ctrl, bool forPopup = true, string title = "")
        {
            string popup = "&isdlg=1&isudlg=1";
            if (!forPopup)
                popup = string.Empty;

            if (!string.IsNullOrWhiteSpace(title))
                title = $"&pageTitle={title}";
            return GetAbsoluteURL($"/Layouts/ugovernit/DelegateControl.aspx?ctrl={ctrl}{title}{popup}");
        }

        public static string FormatNumber(double value, string labelFormat)
        {
            string localizedValue = value.ToString();
            if (string.IsNullOrWhiteSpace(labelFormat))
            {
                localizedValue = value.ToString("#,0.##");
                return localizedValue;
            }


            labelFormat = labelFormat.ToLower();
            if (labelFormat == "currency")
            {
                //if (value >= 10000000000)
                //    localizedValue = (value / 1000000000D).ToString("#,0.00") + "B"; // Billions
                //else 
                if (value >= 1000000)
                    localizedValue = (value / 1000000D).ToString("#,0.0") + "M"; // Millions
                else if (value >= 10000)
                    localizedValue = (value / 1000D).ToString("#,0.0") + "K"; // Thousands

                localizedValue = "$" + localizedValue; // Need to generalize at some point to support other currencies!
            }
            else if (labelFormat == "mintodays")
            {
                localizedValue = value.ToString("#,0.##") + "d";
            }
            else if (labelFormat == "withdollaronly")
            {
                localizedValue = string.Format("${0}", value.ToString("#,0.0"));
            }
            else
            {
                localizedValue = value.ToString("#,0.##");
            }

            return localizedValue;
        }

        /// <summary>
        /// Returns Telephone Regular Expression Pattern
        /// </summary>        
        public static string GetPhoneRegExpression()
        {
            return "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";
        }

        /// <summary>
        /// Returns Email Regular Expression Pattern
        /// </summary>
        public static string GetEmailRegExpression()
        {
            return "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
        }

        public static List<T> MapToList<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    T obj = new T();

                    foreach (DataColumn col in table.Columns)
                    {
                        var property = typeof(T).GetProperty(col.ColumnName);
                        if (property != null && row[col] != DBNull.Value)
                        {
                            property.SetValue(obj, row[col]);
                        }
                    }

                    list.Add(obj);
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e);
            }
            return list;
        }
        public static List<T> ConvertCustomDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetCustomItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetCustomItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        catch (Exception ex) { ULog.WriteException(ex); }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            List<PropertyInfo> prolist = temp.GetProperties().ToList();

            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo prop = prolist.FirstOrDefault(x => x.Name == column.ColumnName);
                if (prop != null)
                {
                    try
                    {
                        if (column.DataType == typeof(DateTime))
                        {
                            prop.SetValue(obj, Convert.ToDateTime(dr[column.ColumnName]), null);
                        }
                        else if (prop.PropertyType == typeof(Boolean))
                        {
                            prop.SetValue(obj, UGITUtility.StringToBoolean(dr[column.ColumnName]), null);
                        }
                        else if (prop.PropertyType == typeof(double))
                        {
                            prop.SetValue(obj, UGITUtility.StringToDouble(dr[column.ColumnName]), null);
                        }
                        else if (prop.PropertyType == typeof(Int32))
                        {
                            prop.SetValue(obj, UGITUtility.StringToInt(dr[column.ColumnName]), null);
                        }
                        else if (prop.PropertyType == typeof(Int64))
                        {
                            prop.SetValue(obj, UGITUtility.StringToLong(dr[column.ColumnName]), null);
                        }
                        else if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(obj, UGITUtility.StringToInt(dr[column.ColumnName]), null);
                        }
                        else
                            prop.SetValue(obj, UGITUtility.ObjectToString(dr[column.ColumnName]), null);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                }
                //foreach (PropertyInfo pro in temp.GetProperties())
                //{
                //    if (pro.Name == column.ColumnName)
                //    {
                //        try
                //        {
                //            if (column.DataType == typeof(DateTime))
                //            {
                //                pro.SetValue(obj, Convert.ToDateTime(dr[column.ColumnName]), null);
                //            }
                //            else
                //                pro.SetValue(obj, dr[column.ColumnName], null);
                //        }
                //        catch (Exception ex) { ULog.WriteException(ex); }
                //    }
                //    else
                //        continue;
                //}
            }
            return obj;
        }

        public static string GenerateHashPassword()
        {
            string Passwd = Guid.NewGuid().ToString().Substring(0, 10).ToString();
            for (int i = 0; i < Passwd.Length; i++)
            {
                if (char.IsLetter(Passwd[i]))
                {
                    Passwd = Passwd.Replace(Passwd[i], char.ToUpper(Passwd[i]));
                    break;
                }
            }
            // Regular expression to check Auto Generated password, for alteast 1 small, 1 capital letter, 1 digit, 1 special character.
            String PasswdRegex = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%-]).{6,10})"; ///@"^\d{5}$";
            if (Regex.IsMatch(Passwd, PasswdRegex))
            {
                return Passwd;
            }
            else
            {
                return GenerateHashPassword();
            }
        }


        public static void CheckMaxSize(FileStream stream)
        {
            if (stream.Length > 1024 * 400000)
                throw new Exception("File is too large");
        }

        public static void AppendContentToFile(string path, HttpPostedFile content)
        {
            using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.InputStream.CopyTo(stream);
                CheckMaxSize(stream);
            }
        }

        /// <summary>
        /// Returns bytes to B, KB, MB...
        /// </summary>
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return $"0 {suf[0]}";
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return $"{(Math.Sign(byteCount) * num)} {suf[place]}";
        }

        /// <summary>
        /// Get statandard width of column,Pass List of object
        /// </summary>
        /// <param name="columns">List of Columns</param>
        /// <param name="changeWidthForcely"> if true then existing width override with standard width</param>
        /// <returns>Same list which we passed to get standard width</returns>
        public static List<Utility.ColumnInfo> GetColumnStandardWidth(List<Utility.ColumnInfo> columns, bool changeWidthForcely = false)
        {
            columns.ForEach(column =>
            {
                column = GetColumnStandardWidth(column, changeWidthForcely);
            });
            return columns;
        }

        /// <summary>
        /// Get column standard width based on datatype
        /// </summary>
        /// <param name="column">Pass ColumnInfo object to get width of it</param>
        /// <param name="changeWidthForcely"> if true then existing width override with standard width</param>
        /// <returns></returns>
        public static Utility.ColumnInfo GetColumnStandardWidth(Utility.ColumnInfo column, bool changeWidthForcely = false)
        {
            if (changeWidthForcely)
                column.Width = GetColumnStandardWidth(column.FieldName, column.DataType);
            else
                column.Width = column.Width > 0 ? column.Width : GetColumnStandardWidth(column.FieldName, column.DataType);
            return column;
        }

        /// <summary>
        /// Get column standard width, passing fieldname and datatype
        /// </summary>
        /// <param name="FieldName">Name of Field</param>
        /// <param name="DataType">Field dataType</param>
        /// <returns>integer : width of column</returns>
        public static int GetColumnStandardWidth(string FieldName, string DataType)
        {
            int width = 0;
            switch (DataType)
            {
                case "Currency":
                    width = 70;
                    break;
                case "DateTime":
                case "System.DateTime": // Time part will wrap, but that's okay!
                case "Date":
                    width = 90;
                    break;
                case "Time":
                    width = 75;
                    break;
                case "Double":
                    width = 50;
                    break;
                case "Percent":
                    width = 50;
                    break;
                case "Percent*100":
                    width = 70;
                    break;
                case "Boolean":
                    width = 40;
                    break;
                case "Integer":
                    width = 70;
                    break;
                case "Progress":
                    width = 100;
                    break;
                default:
                    if (FieldName.Equals(DatabaseObjects.Columns.Title) || FieldName.Equals(DatabaseObjects.Columns.TicketId) || FieldName.Equals(DatabaseObjects.Columns.ProjectID))
                        width = 130;
                    else if (FieldName.Equals(DatabaseObjects.Columns.History) || FieldName.Equals(DatabaseObjects.Columns.Comments) || FieldName.Equals(DatabaseObjects.Columns.UGITComment) || FieldName.Equals(DatabaseObjects.Columns.TicketResolutionComments))
                        width = 200;
                    else if (FieldName.Equals(DatabaseObjects.Columns.TicketStatus) || FieldName.Equals(DatabaseObjects.Columns.TicketProjectManager))
                        width = 130;
                    break;
            }
            return width;
        }

        /// <summary>
        /// This method is used to 
        /// </summary>
        /// <param name="ZipFolderName"></param>
        public static void DeleteFolder(string ZipFolderName)
        {
            bool isExists = false;
            try
            {
                if (ZipFolderName.EndsWith(".zip") && File.Exists(ZipFolderName))
                {
                    using (FileStream stream = new FileStream(ZipFolderName, FileMode.Open))
                    {
                        using (ZipArchive zipArchive = new ZipArchive(stream, ZipArchiveMode.Update))
                        {
                            if (zipArchive == null)
                                return;

                            foreach (var entry in zipArchive.Entries.ToList())
                            {
                                entry.Delete();
                            }
                            zipArchive.Dispose();
                            File.Delete(ZipFolderName);
                        }
                    }
                }
                else
                {
                    isExists = Directory.Exists(ZipFolderName);
                    if (isExists)
                    {
                        DirectoryInfo serviceFolder = new DirectoryInfo(ZipFolderName);
                        foreach (FileInfo file in serviceFolder.GetFiles())
                        {
                            file.Delete();
                        }
                        serviceFolder.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteLog(string.Format("Folder not exists,Exception: {0}", ex.Message));
            }
        }
        public static string TenantAccount { get; set; }
        public static string AuthenticationType { get; set; }
        /// <summary>
        /// To validate ticket CPR-21-0000333
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public static bool IsValidTicketID(string ticketId)
        {
            string ticketPattern = @"(^[a-zA-Z]{3}-[0-9]{2}-[0-9]{6})";
            return Regex.IsMatch(ticketId, ticketPattern);
        }

        /// <summary>
        /// Text Color For Background-color
        /// </summary>
        /// <param name="bg"></param>
        /// <returns></returns>
        public static Color IdealTextColor(Color bg)
        {
            int nThreshold = 105;
            int bgDelta = Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) +
                                          (bg.B * 0.114));

            Color foreColor = (255 - bgDelta < nThreshold) ? Color.Black : Color.White;
            return foreColor;
        }

        /// <summary>
        /// Simulates a SQL 'Where In' clause in CAML
        /// </summary>
        /// <param name="lookupId">Specify whether to use the Lookup column's Id or Value.</param>
        /// <returns>Nested 'Or' elements portion of CAML query</returns>
        public static string CamlIn<T>(string internalFieldName, bool lookupId, params T[] values)
        {
            XDocument doc = new XDocument();
            XElement prev = null;
            int index = 0;

            while (index < values.Length)
            {
                XElement element =
                    new XElement("Or",
                        new XElement("Eq",
                            new XElement("FieldRef",
                                new XAttribute("Name", internalFieldName),
                                lookupId ? new XAttribute("LookupId", "TRUE") : null),
                            new XElement("Value",
                                new XAttribute("Type", "Lookup"),
                                values[index++].ToString())));

                if (index == values.Length - 1)
                {
                    element.AddFirst(
                        new XElement("Eq",
                            new XElement("FieldRef",
                                new XAttribute("Name", internalFieldName),
                                lookupId ? new XAttribute("LookupId", "TRUE") : null),
                            new XElement("Value",
                                new XAttribute("Type", "Lookup"),
                                values[index++].ToString())));
                }

                if (prev != null)
                    prev.AddFirst(element);
                else
                    doc.Add(element);

                prev = element;
            }

            if (values.Length == 1)
            {
                XElement newRoot = doc.Descendants("Eq").Single();
                doc.RemoveNodes();
                doc.Add(newRoot);
            }

            return doc.ToString(SaveOptions.DisableFormatting);
        }

        public static void GetParentTaskID(UGITTask task, List<string> returnids)
        {
            if (task == null)
                return;
            else if (task.ParentTaskID == 0)
                return;
            else
            {
                returnids.Add(task.ParentTaskID.ToString());
                GetParentTaskID(task.ParentTask, returnids);
            }

        }

        public static DataTable UpdateDatatableRow(DataTable NewDt, DataTable OldDt)
        {
            for (int j = 0; j < NewDt.Rows.Count; j++)
            {
                for (int k = 1; k < NewDt.Columns.Count; k++)
                {
                    // Referencing the column in the new row by number, starting from 0.
                    OldDt.Rows[j][k] = NewDt.Rows[j][k];
                }
            }
            OldDt.AcceptChanges();
            return OldDt;
        }

        public static Dictionary<string, string> GetTabNames()
        {
            // Creating a dictionary for tab names and key values
            Dictionary<string, string> dictTabs = new Dictionary<string, string>();
            dictTabs.Add("allopentickets", "Open");
            dictTabs.Add("allclosedtickets", "Closed");
            dictTabs.Add("waitingonme", "Waiting on Me");
            dictTabs.Add("myopentickets", "My Tickets");
            dictTabs.Add("unassigned", "Unassigned");
            dictTabs.Add("allresolvedtickets", "Resolved");
            dictTabs.Add("onhold", "On-Hold");
            dictTabs.Add("departmentticket", "My Department");
            dictTabs.Add("mygrouptickets", "My Group");

            return dictTabs;
        }
        public static string AssemblyVersion
        {
            get
            {
                return Assembly.Load("uGovernIT.Web").GetName().Version.ToString();
            }
        }

        public static string LoadScript(string url)
        {
            return "\n<script language='javaScript' type='text/javascript' src='" + url + "?v=" + AssemblyVersion + "'></script>";
        }

        public static string LoadStyleSheet(string url)
        {
            return "\n<link href='" + url + "?v=" + AssemblyVersion + "' rel='stylesheet' type='text/css' />";
        }

        public static string GetDefualtProfilePictureIfEmpty(string picture)
        {
            string defaultpicture = "/Content/Images/useravtar64x64.png";
            if (picture.EndsWith("userNew.png"))
                defaultpicture = "/Content/Images/useravtar64x64.png";
            if (!string.IsNullOrEmpty(picture))
                defaultpicture = "/Content/Images/useravtar64x64.png";
            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(picture)))
                return picture;

            return defaultpicture;
        }

        public static string GetHrefFromATagString(string aTag)
        {
            string htmlString = aTag;

            // Load the HTML string into an HtmlDocument object
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            // Get the anchor (a) element
            HtmlNode anchorNode = doc.DocumentNode.SelectSingleNode("//a");

            // Get the value of the href attribute
            if (anchorNode == null)
                return string.Empty;
            string hrefValue = anchorNode.GetAttributeValue("href", "");

            return hrefValue;
        }

        public static List<string> GetModuleNamesList()
        {
            List<string> moduleNamesList = new List<string>();

            foreach (var field in typeof(ModuleNames).GetFields())
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    moduleNamesList.Add((string)field.GetRawConstantValue());
                }
            }

            return moduleNamesList;
        }

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static List<DateTime> ConvertStringToDateList(string input)
        {
            List<DateTime> dateList = new List<DateTime>();
            string[] dateStrings = input.Split(',');

            foreach (string dateString in dateStrings)
            {
                dateList.Add(UGITUtility.StringToDateTime(dateString));
            }

            return dateList;
        }

        public static bool CanShowGanttViewInFullScreen(NameValueCollection queryString)
        {
            bool fullScreen = false;
            if (queryString != null)
            {
                string controlValue = queryString.Get("control");
                if (!string.IsNullOrWhiteSpace(controlValue) && controlValue.ToLower() == "customresourceallocation")
                {
                    fullScreen = true;
                }
            }
            return fullScreen;
        }
    }


    public static class SerializationExtensions
    {
        public static string Serialize<T>(this T obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var writer = new StringWriter())
            using (var stm = new XmlTextWriter(writer))
            {
                serializer.WriteObject(stm, obj);
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(this string serialized)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = new StringReader(serialized))
            using (var stm = new XmlTextReader(reader))
            {
                return (T)serializer.ReadObject(stm);
            }
        }

        public static string JsonSerialize<T>(this T obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, obj);
            return sb.ToString();
        }
        public static T JsonDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string TruncateWithEllipsis(string input, int lengthRequired)
        {
            return TruncateWithEllipsis(input, lengthRequired, string.Empty);
        }

        public static string TruncateWithEllipsis(string input, int lengthRequired, string stopBefore, string ellipsis = "")
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string output = string.Empty;
            if (!string.IsNullOrEmpty(stopBefore))
                output = input.Split(new string[] { stopBefore }, StringSplitOptions.None)[0];
            else
                output = input;

            if (string.IsNullOrWhiteSpace(ellipsis))
                ellipsis = "...";

            if (output.Length > lengthRequired)
            {
                if (lengthRequired > ellipsis.Length)
                    output = string.Format("{0}{1}", output.Substring(0, lengthRequired - ellipsis.Length), ellipsis);
                else
                    output = output.Substring(0, lengthRequired);
            }
            return output;
        }

        public static DateTime ToDateTime(this string date)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.MinValue.ToString();
                }
                dt = Convert.ToDateTime(date);
            }
            catch (Exception)
            {
                dt = DateTime.MinValue;
            }
            return dt;
        }

        public static string GetJSONString(DataTable table)
        {
            StringBuilder headStrBuilder = new StringBuilder(table.Columns.Count * 5); //pre-allocate some space, default is 16 bytes
            for (int i = 0; i < table.Columns.Count; i++)
            {
                headStrBuilder.AppendFormat("\"{0}\" : \"{0}{1}¾\",", table.Columns[i].Caption, i);
            }
            headStrBuilder.Remove(headStrBuilder.Length - 1, 1); // trim away last ,

            StringBuilder sb = new StringBuilder(table.Rows.Count * 5); //pre-allocate some space
            //sb.Append("{\"");
            // sb.Append(table.TableName);
            // sb.Append("\" : [");
            sb.Append("[");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string tempStr = headStrBuilder.ToString();
                sb.Append("{");
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    table.Rows[i][j] = table.Rows[i][j].ToString().Replace("'", "");
                    tempStr = tempStr.Replace(table.Columns[j] + j.ToString() + "¾", table.Rows[i][j].ToString());
                }
                sb.Append(tempStr + "},");
            }
            sb.Remove(sb.Length - 1, 1); // trim last ,
            sb.Append("]");
            //sb.Append("}");

            return sb.ToString();
        }

    }

    public class ReportQueryFormat
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public bool ShowCompanyLogo { get; set; }
        public string AdditionalInfo { get; set; }
        public string AdditionalFooterInfo { get; set; }
        public bool ShowDateInFooter { get; set; }
        public Dictionary<string, string> Legend { get; set; }
    }
}

public enum UserType
{
    User,
    Role
};

public enum RoleType
{
    User,
    //SAdmin,
    Admin,
    TicketAdmin,
    ResourceAdmin,
    UGITSuperAdmin
}
