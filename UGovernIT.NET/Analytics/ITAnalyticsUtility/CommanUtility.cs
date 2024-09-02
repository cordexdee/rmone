using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Serialization;
namespace ITAnalyticsUtility
{
    public class CommanUtility
    {
        public static string Serializer(object panel, Type type)
        {
            string xmlString = string.Empty;

            StringWriter sWriter = new StringWriter();
            XmlSerializer xSerialize = new XmlSerializer(type);
            xSerialize.Serialize(sWriter, panel);
            xmlString = sWriter.ToString();

            return xmlString;
        }

        public static object DeSerializer(string xmlString, Type type)
        {
            StringReader sReader = new StringReader(xmlString);
            XmlSerializer xSerialize = new XmlSerializer(type);
            object obj = xSerialize.Deserialize(sReader);
            return obj;
        }

        public static long[] ConvertStringToIntArray(string idArray, char[] delimiter)
        {
            List<long> validatedIds = new List<long>();
            if (idArray != null && idArray.Trim() != string.Empty)
            {
                string[] stringArray = idArray.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                int arrayLength = stringArray.Length;
                if (arrayLength > 0)
                {
                    for (int i = 0; i < arrayLength; i++)
                    {
                        try
                        {
                            validatedIds.Add(int.Parse(stringArray[i]));
                        }
                        catch (FormatException)
                        {
                        }
                    }
                }
            }
            return validatedIds.ToArray();
        }

        public static string ConvertIntArrayToString(long[] idArray, string delimiter = ",")
        {
            StringBuilder ids = new StringBuilder();
            int arrayLength = idArray.Length;
            if (delimiter == null)
            {
                delimiter = string.Empty;
            }

            if (arrayLength > 0)
            {
                for (int i = 0; i < arrayLength; i++)
                {
                    ids.Append(Convert.ToString(idArray[i]));
                    if (i != arrayLength - 1)
                    {
                        ids.Append(delimiter);
                    }
                }
            }
            return ids.ToString();
        }


        public static string TransformXmlUsingXsl(IXPathNavigable xPathNavigable, string xslFilePath, XsltArgumentList args)
        {
            TextWriter textWriter = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                XslCompiledTransform xslTransform = new XslCompiledTransform(true);
                xslTransform.Load(xslFilePath);
                textWriter = new StringWriter(sb);
                xslTransform.Transform(xPathNavigable, args, textWriter);
            }
            finally
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                    textWriter.Dispose();
                }
            }
            return sb.ToString();
        }

        public static bool ConvertStringToBoolean(string val)
        {
            if(val == null)
            {
                return false;
            }
            val = val.ToLower();
            if (val == "1" || val == "yes" || val == "on")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Min: 188 and Max: 248
        /// </summary>
        /// <param name="occurance"></param>
        /// <returns></returns>
        public static System.Drawing.Color GetNewColor(int occurance)
        {
            return GetNewColor(248, 188, occurance);
        }

        public static System.Drawing.Color GetNewColor(int max, int min, int occurance)
        {
            
            int reminder = occurance % 4;
            int wholeVal = occurance / 4;

            int maxMinDiff = max - min;
            int scale = wholeVal;
            if (wholeVal > 10)
            {
                int maxOccInScale = maxMinDiff / scale;
                if (maxOccInScale <= wholeVal)
                {
                    scale = scale - maxOccInScale + 10;
                    if (scale <= 0)
                    {
                        scale = scale + maxOccInScale + 10;
                    }
                }
            }
            else
            {
                wholeVal = 10;
            }
            int loopStep = wholeVal % 4;
            if (maxMinDiff < (loopStep * scale))
            {
                scale = 10;
            }


            System.Drawing.Color colr = new System.Drawing.Color();
            int r = max;
            int g = max;
            int b = max;
            switch (reminder)
            {
                case 1:
                    r = max;
                    g = min + (loopStep * scale);
                    b = min;
                    break;
                case 2:

                    r = max - (loopStep * scale);
                    g = max;
                    b = min;
                    break;
                case 3:
                    r = min;
                    g = max;
                    b = min + (loopStep * scale);
                    break;
                default:
                    r = min;
                    g = max - (loopStep * scale);
                    b = min;
                    break;
            }
            colr = System.Drawing.Color.FromArgb(r, g, b);
            return colr;
        }
    }
}
