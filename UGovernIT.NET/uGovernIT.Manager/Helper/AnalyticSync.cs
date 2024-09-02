using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Data;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.Helpers
{
    public class AnalyticSync
    {
        public static void Sync(ApplicationContext context)
        {
            //Load analytic dashboard data from analytic
            string analyticData = string.Empty;
            using (WebClient webClient = new WebClient())
            {
                AnalyticCredential analyticCredential = new AnalyticCredential();

                string credential = context.ConfigManager.GetValue(ConfigConstants.AnalyticAuth);
                string decryptedCredential = uGovernITCrypto.Decrypt(credential, Constants.UGITAPass);
                if (string.IsNullOrEmpty(decryptedCredential))
                    return; // Can't do anything without credentials!

                XmlDocument xmlDocCtnt = new XmlDocument();
                xmlDocCtnt.LoadXml(decryptedCredential);
                analyticCredential = (AnalyticCredential)uHelper.DeSerializeAnObject(xmlDocCtnt, analyticCredential);
                webClient.Credentials = new NetworkCredential(analyticCredential.UserName, analyticCredential.Password, analyticCredential.Domain);

                string analyticUrl = context.ConfigManager.GetValue(ConfigConstants.AnalyticUrl);
                if (!string.IsNullOrEmpty(analyticUrl))
                {
                    Stream stream = webClient.OpenRead(string.Format("{0}/analytics/GetActiveAanalyticDashboards", analyticUrl));
                    StreamReader fd = new StreamReader(stream);
                    analyticData = fd.ReadToEnd();
                }
            }

            //Deserialize data xml to object
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(analyticData);
            List<AnalyticDashboards> dashboards = new List<AnalyticDashboards>();
            dashboards = (List<AnalyticDashboards>)uHelper.DeSerializeAnObject(xmlDoc, dashboards);
            UpdateAnalyticDashboard(context,dashboards);
        }

        public static bool Validate(ApplicationContext context,AnalyticCredential credential)
        {
            return Validate(context, credential, context.ConfigManager.GetValue(ConfigConstants.AnalyticUrl));
        }
        public static bool Validate(ApplicationContext context, AnalyticCredential credential, string analyticUrl)
        {
            try
            {
                //Load analytic dashboard data from analytic
                string analyticData = string.Empty;
                using (WebClient webClient = new WebClient())
                {
                    webClient.Credentials = new NetworkCredential(credential.UserName, credential.Password, credential.Domain);
                    Stream stream = webClient.OpenRead(string.Format("{0}/analytics", analyticUrl));
                    StreamReader fd = new StreamReader(stream);
                    analyticData = fd.ReadToEnd();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Validate error: " + analyticUrl);
                return false;
            }
        }

        public static void UpdateAnalyticDashboard(ApplicationContext context, List<AnalyticDashboards> dashboards)
        {
            AnalyticDashboardManager analyticDashboardManager = new AnalyticDashboardManager(context);
            DataTable dashboadList = analyticDashboardManager.GetDataTable();
            DataView dataView = new DataView(dashboadList);
            DataTable dashboardTable = dataView.ToTable(false, DatabaseObjects.Columns.AnalyticID, DatabaseObjects.Columns.AnalyticName, DatabaseObjects.Columns.AnalyticVID, DatabaseObjects.Columns.DashboardID, DatabaseObjects.Columns.Title);

            if (dashboardTable != null)
            {
                dashboardTable.Columns.Add("Valid", typeof(bool));
            }
            AnalyticDashboards Item = new AnalyticDashboards();
            DataRow lItem = null;
            foreach (AnalyticDashboards analyticD in dashboards)
            {
                Item = analyticD;
                lItem = null;
                if (dashboardTable != null)
                {
                    DataRow row = dashboardTable.AsEnumerable().FirstOrDefault(x => x.Field<double>(DatabaseObjects.Columns.DashboardID) == analyticD.DashboardID);
                    if (row == null)
                    {
                        lItem = dashboadList.NewRow();
                        lItem[DatabaseObjects.Columns.DashboardID] = analyticD.DashboardID;
                    }
                    else
                    {
                       DataRow[] items = dashboadList.Select(string.Format("{0}='{1}'", "ID", UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Id])));
                        if (items.Count() > 0)
                            lItem = items[0];                   
                        row["Valid"] = true;
                    }
                }
                else
                {
                    lItem = dashboadList.NewRow();
                    lItem[DatabaseObjects.Columns.DashboardID] = analyticD.DashboardID;
                }

                lItem[DatabaseObjects.Columns.Title] = analyticD.Title;
                lItem[DatabaseObjects.Columns.AnalyticVID] = analyticD.AnalyticVID;
                lItem[DatabaseObjects.Columns.AnalyticID] = analyticD.AnalyticID;
                lItem[DatabaseObjects.Columns.AnalyticName] = analyticD.AnalyticName;
                //lItem.UpdateOverwriteVersion();
            }

            //Deletes useless datas
            if (dashboardTable != null)
            {
                DataView view = dashboardTable.DefaultView;
                view.RowFilter = string.Format("Valid=0 or Valid is null");
                dashboardTable = view.ToTable();
                foreach (DataRow row in dashboardTable.Rows)
                {
                    Item = analyticDashboardManager.Get(Convert.ToInt32(row[DatabaseObjects.Columns.Id]));
                    Item.Deleted = true;
                    analyticDashboardManager.Update(Item);
                }
            }
        }
    }
}
