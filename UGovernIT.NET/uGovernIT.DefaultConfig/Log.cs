using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.DefaultConfig
{
    public class Log
    {
        public static string lockObj = string.Empty;

        public static void WriteLog(string message, string function)
        {
            string logMessage = string.Format("[{0}] {1} {2}", DateTime.Now.ToLongTimeString(), message, function);

            //Console.WriteLine(logMessage);

            AppendToFile(logMessage);
        }

        public static void WriteLog(string message)
        {
            string logMessage = string.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), message);

            //Console.WriteLine(logMessage);

            AppendToFile(logMessage);
        }

        public static void WriteException(Exception ex, string function)
        {
            string logMessage = string.Format("{0}: [{1}] {2}\n{3}", function, DateTime.Now.ToLongTimeString(), ex.Message, ex.StackTrace);

            //Console.WriteLine(logMessage);

            AppendToFile(logMessage);
        }

        public static void AppendToFile(string logMessage)
        {
            if (Config.WriteToLogFile)
            {
                lock (lockObj)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Config.LogFile, true))
                    {
                        file.WriteLine(logMessage);
                    }
                }
            }
        }        
    }
}
