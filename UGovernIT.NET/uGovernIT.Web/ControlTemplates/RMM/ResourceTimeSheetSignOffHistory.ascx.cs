using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class ResourceTimeSheetSignOffHistory : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();

        public string ListItemID { get; set; }
        List<ResourceTimeSheetEntry> datalist;
        DataTable signOffListItem;

        protected override void OnInit(EventArgs e)
        {
            GetSignOffHistory();
            historyGrid.DataSource = GetHistoryData(datalist);
            historyGrid.DataBind();

            //Enable checkbox on filter popup
            GridHeaderFilterMode headerFilterMode = GridHeaderFilterMode.CheckedList;
            foreach (GridViewDataColumn column in historyGrid.Columns)
            {
                column.SettingsHeaderFilter.Mode = headerFilterMode;
            }

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GetSignOffHistory()
        {
            //Get SpListItem from a SPList by using ItemID
            signOffListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheetSignOff, $"{DatabaseObjects.Columns.ID}={ListItemID} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'" );

            //Get list of SignOff History data
            datalist = GetHistoryEntries(Convert.ToString(signOffListItem.Rows[0][DatabaseObjects.Columns.History]));
        }

        private DataTable GetHistoryData(List<ResourceTimeSheetEntry> history)
        {
            DataTable data = new DataTable();
            data.Columns.Add("Date");
            data.Columns.Add("Action By");
            data.Columns.Add("Action");
            data.Columns.Add("Comment");

            if (history != null && history.Count > 0)
            {
                foreach (ResourceTimeSheetEntry item in history)
                {
                    data.Rows.Add(item.created, item.createdBy, item.status, item.entry);
                }
            }

            return data;
        }

        private List<ResourceTimeSheetEntry> GetHistoryEntries(string data)
        {
            List<ResourceTimeSheetEntry> dataList = new List<ResourceTimeSheetEntry>();

            string[] versionsDelim = { Constants.SeparatorForVersions };
            string[] versions = data.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

            foreach (string version in versions)
            {
                // Assume <version1>$;#$<version2>$;#$<version3>
                string[] versionDelim = { Constants.Separator };
                string[] versionData = version.Split(versionDelim, StringSplitOptions.None);

                if (versionData.Length >= 3)
                {
                    // Assume <userID>;#<timestamp>;#<text>
                    //entry.createdBy = uHelper.GetUserById(int.Parse(splitString[0])).Name;
                    ResourceTimeSheetEntry entry = new ResourceTimeSheetEntry();
                    entry.createdBy = versionData[0];
                    if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DateTime createdDate = UGITUtility.StringToDateTime(versionData[1].Substring(Constants.UTCPrefix.Length));
                        entry.created = createdDate.ToLocalTime().ToString("MMM-d-yyyy hh:mm tt");
                    }
                    else
                    {
                        DateTime createdDate = UGITUtility.StringToDateTime(versionData[1]);
                        entry.created = createdDate.ToString("MMM-d-yyyy hh:mm tt");
                    }

                    entry.status = versionData[2];

                    entry.entry = versionData.Length > 3 ? versionData[3].Replace("\r\n", "<br>") : string.Empty;

                    dataList.Add(entry);
                }
                else
                    Util.Log.ULog.WriteLog("Invalid timesheet history entry found: " + data);
            }

            return dataList;
        }
    }
}