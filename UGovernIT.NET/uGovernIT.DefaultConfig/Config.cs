using System;

namespace uGovernIT.DefaultConfig
{
    public class Config
    {
        public static bool WriteToLogFile;
        public static string SPSiteUrl;
        public static string LogFile;
        public static int DefaultUserID;
        public static int BatchSize;

        public static void LoadConfig(string spSite, bool writeInLogFile)
        {
            WriteToLogFile = writeInLogFile;
            SPSiteUrl = spSite;
            LogFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\uGovernITDefaults_Script.log";
            DefaultUserID = 2;
            BatchSize = 10;
        }
    }
}
