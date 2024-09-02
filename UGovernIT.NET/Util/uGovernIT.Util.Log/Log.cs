using Common.Logging;
using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using NLog.Targets;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace uGovernIT.Util.Log
{
    public static class LogExtension
    {
        //public static void Debug(this ILog ilog, string message, object debugData)
        //{

        //}
    }

    public class ULog
    {
        static readonly ILogger log;
        static ULog logg;
        static readonly string connectionStringnew = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
        private string AccountName { get; set; }
        private string Username { get; set; }
        public static void Create(string AccountName, string Username)
        {
            logg = new ULog
            {
                AccountName = AccountName,
                Username = Username
            };
        }
        static ULog()
        {
            if (log == null)
            {
                log = NLog.LogManager.GetCurrentClassLogger();
            }
        }

        //public static void SetTenanttID(string tenantID)
        //{
        //    if (log.Factory.Configuration != null)
        //    {
        //        log.Factory.Configuration.Variables["tenantid"] = tenantID;
        //    }
        //}

        public static void WriteLog(string message, string category = null)
        {
            if (log.Factory.Configuration != null)
            {
                log.Factory.Configuration.Variables["category"] = category;
                log.Factory.Configuration.Variables["tenantid"] = logg.AccountName;
                log.Factory.Configuration.Variables["username"] = logg.Username;
            }
            ThreadStart ts = delegate ()
            {
                log.Info(message);
                SaveLogsInSPLogger(message, category, "Info");
            };
            Thread th = new Thread(ts);
            th.IsBackground = true;
            th.Start();
        }

        public static void WriteException(string message, string category = null)
        {
            if (log.Factory.Configuration != null)
            {
                log.Factory.Configuration.Variables["category"] = category;
                log.Factory.Configuration.Variables["tenantid"] = logg.AccountName;
                log.Factory.Configuration.Variables["username"] = logg.Username;
            }
            ThreadStart ts = delegate ()
            {
                log.Error(message);
                SaveLogsInSPLogger(message, category, "Error");
            };
            Thread th = new Thread(ts);
            th.IsBackground = true;
            th.Start();
        }

        public static void WriteException(Exception ex, string message, string category = null)
        {
            if (log.Factory.Configuration != null && log.Factory.Configuration.Variables != null)
            {
                log.Factory.Configuration.Variables["category"] = category;
                //log.Factory.Configuration.Variables["tenantid"] = "0";
            }
            ThreadStart ts = delegate ()
            {
                log.Error(ex.ToString(), message);
                SaveLogsInSPLogger(message, category, "Error", ex);
            };
            Thread th = new Thread(ts);
            th.IsBackground = true;
            th.Start();
        }

        public static void WriteException(Exception ex, string category = null)
        {
            WriteException(ex, "", category);
        }

        public static void WriteUGITLog(string userId, string logMessage, string severity, string category, string TenantId)
        {
            WriteUGITLog(userId, logMessage, severity, category, TenantId, string.Empty, string.Empty);
        }

        public static void WriteUGITLog(string userId, string logMessage, string severity, string category, string TenantId, string moduleName, string ticketID)
        {
            bool EnableUGITLogging = false;
            string UserName = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionStringnew))
            {
                using (SqlCommand cmd = new SqlCommand($"select KeyValue  from Config_ConfigurationVariable where TenantID = '{TenantId}' and KeyName = 'EnableUGITLogging'", con))
                {
                    con.Open();
                    EnableUGITLogging = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand($"select Name from AspNetUsers where Id = '{userId}'", con))
                {
                    con.Open();
                    UserName = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                }
            }

            WriteLog(string.Format("{0}: {1}", UserName, logMessage));

            if (EnableUGITLogging)
            {
                ThreadStart threadStartMethod = delegate () { ULog.WriteToUGITLog(userId, logMessage, severity, category, TenantId, moduleName, ticketID); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
        }

        private static void WriteToUGITLog(string userId, string logMessage, string severity, string category, string TenantId, string moduleName, string ticketID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionStringnew))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_WriteUGITLog", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("UserId", userId);
                        cmd.Parameters.AddWithValue("Description", logMessage);
                        cmd.Parameters.AddWithValue("CategoryName", category);
                        cmd.Parameters.AddWithValue("ModuleNameLookup", moduleName);
                        cmd.Parameters.AddWithValue("Severity", severity);
                        cmd.Parameters.AddWithValue("TicketId", ticketID);
                        cmd.Parameters.AddWithValue("TenantID", TenantId);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteException(ex, "Error writing to UGITLog");
            }
        }

        public static bool AuditTrail(string userName, string message, Uri urlVisited)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(message) || urlVisited == null || urlVisited.Segments == null)
                return false;

            string page = string.Empty;
            string[] segments = urlVisited.Segments;
            if (segments.Length > 0)
                page = segments[segments.Length - 1];

            string fullMessage = string.Format("User {0} {1} at page {2} on {3}", userName, message, page, urlVisited.Host);
            WriteLog(fullMessage);
            return true;
        }
        public static bool AuditTrail(string spUser, Uri urlVisited)
        {
            if (spUser == null || urlVisited == null || urlVisited.Segments == null)
                return false;

            string page = string.Empty;
            string[] segments = urlVisited.Segments;
            if (segments.Length > 0)
                page = segments[segments.Length - 1];

            string message = string.Format("User {0} visited page {1} on {2}", spUser, page, urlVisited.Host);
            WriteLog(message);
            return true;
        }
        public static bool AuditTrail(string message)
        {
            WriteLog(message, "Information");
            return true;
        }
        public static void SaveLogsInSPLogger(string message, string category, string logType, Exception ex = null)
        {
            NLogModel model = new NLogModel();
            model.Category = category;
            model.Exception = "";
            model.Host = System.Net.Dns.GetHostName();
            model.Message = message;
            model.Process = "";
            model.Level = logType;
            model.Type = "";
            model.TenantId = logg.AccountName;
            model.Username = logg.Username;
            if (ex != null)
                model.Stacktrace = ex.StackTrace;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsSPLoggerEnabled"]) && !string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["SpLoggerUrl"])))
            {
                string url = ConfigurationManager.AppSettings["SpLoggerUrl"];
                HttpClient client = new HttpClient();
                var contentData = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                client.PostAsync(url, contentData);
            }
        }
    }
    public class NLogModel
    {
        public DateTime TimeStamp { set; get; }
        public string Host { set; get; }
        public string Type { set; get; }
        public string Process { set; get; }
        public string TenantId { set; get; }
        public string Category { set; get; }
        public string Message { set; get; }
        public string Stacktrace { set; get; }
        public string Exception { set; get; }
        public string Username { set; get; }
        public string Level { set; get; }
    }
}
