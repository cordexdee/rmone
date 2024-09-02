using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
     public class Log
     {
        public static void WriteException(Exception ex)
        {
            string filePath = @"E:\ErrorLog.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                writer.Close();
            }
            
        }
        public static bool WriteLog(string logMessage,string traceSeverity, string eventSeverity)
        {
            try
            {
                 string filePath = @"E:\ErrorLog.txt";

                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + logMessage + "<br/>" + Environment.NewLine + "traceSeverity : " + traceSeverity+ "eventSeverity : " + eventSeverity +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    writer.Close();
                }
                Debug.WriteLine(logMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool WriteLog(string logMessage)
        {
            //return WriteLog(logMessage, TraceSeverity.Medium, EventSeverity.Information);
            return WriteLog(logMessage, "", "");
        }

        public static bool WriteLog(string logMessage, string functionDetails)
        {
            return WriteLog(functionDetails + ": \n" + logMessage);
        }
    }
}
