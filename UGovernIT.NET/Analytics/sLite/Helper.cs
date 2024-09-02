using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace sLite
{
    public static class Helper
    {

        public static bool ConvertToBoolean(string val)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(val))
            {
                return result;
            }
            val = val.ToLower();
            if (val == "false")
            {
                result = false;
            }
            else if (val == "true" || val != "0")
            {
                 result = true;
            }

            return result;
        }




        public static string Serializer(object panel, Type type)
        {
            string xmlString= string.Empty;
           
                StringWriter sWriter = new StringWriter();
                XmlSerializer xSerialize = new XmlSerializer(type);
                xSerialize.Serialize(sWriter, panel);
                xmlString= sWriter.ToString();
           
            return xmlString;
        }

        public static string SerializerSupressDeclaration(object panel, Type type)
        {
            string xmlString = string.Empty;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            XmlSerializer cs = new XmlSerializer(type);

            cs.Serialize(writer, panel, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            xmlString = sr.ReadToEnd();

            
            return xmlString;
        }

        public static object DeSerializer(string xmlString, Type type)
        {

            StringReader sReader = new StringReader(xmlString);
            XmlSerializer xSerialize = new XmlSerializer(type);
            object obj = xSerialize.Deserialize(sReader);
            return obj;
        }

        public static object DeSerializer(string xmlString)
        {
            Analytic aa = new Analytic();
            StringReader sReader = new StringReader(xmlString);
            XmlSerializer xSerialize = new XmlSerializer(aa.GetType());
            aa = (Analytic)xSerialize.Deserialize(sReader);
            return aa;
        }
    }
}
