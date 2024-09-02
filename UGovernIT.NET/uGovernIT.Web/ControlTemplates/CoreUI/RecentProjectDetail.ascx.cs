using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class RecentProjectDetail : System.Web.UI.UserControl
    {
        public ApplicationContext AppContext;
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string TicketPath { get; set; }
        ModuleViewManager _moduleViewManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            _moduleViewManager = new ModuleViewManager(AppContext);
            TicketManager ticketManagerObj = new TicketManager(AppContext);
            List<UGITModule> moduleTable = new List<UGITModule>();
            List<string> tabs = new List<string>() { "myproject" };
            moduleTable = _moduleViewManager.Load(x => x.ModuleType == ModuleType.Project && x.EnableModule == true).ToList();
            DataTable moduleUserStatisticsListTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{AppContext.TenantID}'");
            //ModuleStatisticResponse moduleStatResult;
            DataTable dtOpenTickets = new DataTable();
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            Dictionary<string, string> modulePaths = new Dictionary<string, string>();
            foreach (UGITModule module in moduleTable)
            {
                modulePaths.Add(module.ModuleName, module.StaticModulePagePath);
                DataRow[] myProjectRows = moduleUserStatisticsListTable.Select(DatabaseObjects.Columns.ModuleNameLookup + "='" + module.ModuleName + "'");
                string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.UserName, UGITUtility.ObjectToString(AppContext.CurrentUser?.Id));
                
                //myProjectRows = myProjectRows.CopyToDataTable().Select(selectQuery);
                DataTable dt = new DataTable();
                if (myProjectRows.Length > 0)
                {
                    myProjectRows = myProjectRows.CopyToDataTable().Select(selectQuery);
                    if (myProjectRows.Length > 0)
                        dt = myProjectRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                }
                //Ticket ticketManager = new Ticket(AppContext, module.ModuleName);
                //TicketManager ticketManagerObj = new TicketManager(AppContext);
                DataTable dtallopentickets = ticketManagerObj.GetOpenTickets(module);
                string ticketList = string.Join("','", dt.AsEnumerable().Select(r => r["TicketId"].ToString()));
                if (!string.IsNullOrEmpty(ticketList))
                {
                    DataRow[] filteredtickets = dtallopentickets.Select($"TicketId IN ('{ticketList}')");
                    if (filteredtickets != null && filteredtickets.Count() > 0)
                    {
                        DataTable copyTable = filteredtickets.CopyToDataTable();
                        dtOpenTickets.Merge(copyTable);
                    }
                }
            }

            DataRow recentProject = dtOpenTickets.AsEnumerable().OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.Modified)).FirstOrDefault();
            if (recentProject != null)
            {
                string sectorText = string.Empty;
                UserProfile projectManager = AppContext.UserManager.GetUserById(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketProjectManager]));
                if (projectManager != null)
                    imgProjectManager.ImageUrl = projectManager.Picture;
                else
                    imgProjectManager.ImageUrl = "~/Assets/Projector.png";

                string StartDate = "--";
                string EndDate = "--";
                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.TicketTargetStartDate))
                {
                    StartDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketTargetStartDate]));
                }

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.EstimatedConstructionStart))
                {
                    StartDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.EstimatedConstructionStart]));
                }

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.ContractStartDate))
                {
                    StartDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.ContractStartDate]));
                }

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.TicketCreationDate))
                    StartDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketCreationDate]));

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    EndDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.EstimatedConstructionEnd]));

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.ContractExpirationDate))
                {
                    EndDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.ContractExpirationDate]));
                }

                if (UGITUtility.IsSPItemExist(recentProject, DatabaseObjects.Columns.TicketDesiredCompletionDate))
                    EndDate = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketDesiredCompletionDate]));


                lblDateRange.Text = $"{StartDate} to {EndDate}";
                lblProjectTitle.Text = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.Title]);
                lblStatus.Text = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.Status]);
                
                if (UGITUtility.IfColumnExists(recentProject, DatabaseObjects.Columns.BCCISector))
                    lblSector.Text = sectorText = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.BCCISector]);
                if (recentProject[DatabaseObjects.Columns.IconBlob] != DBNull.Value)
                    imgProjectIcon.ImageUrl = "data:image;base64," + Convert.ToBase64String((byte[])recentProject[DatabaseObjects.Columns.IconBlob]);
                else
                {
                    ConfigurationVariableManager fieldManager = new ConfigurationVariableManager(AppContext);
                    string defaultIconUrl = fieldManager.GetValue(Constants.ProjectIcon);
                    if (!string.IsNullOrEmpty(defaultIconUrl))
                        imgProjectIcon.ImageUrl = defaultIconUrl;
                    else
                        imgProjectIcon.ImageUrl = "/Content/Images/newUpload.png";
                }
                TicketId = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketId]);
                Title = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.Title]);
                if (!string.IsNullOrEmpty(Title) && Title.Length > 50)
                {
                    lblProjectTitle.Text = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.Title]).Substring(0, 50) + "...";
                    lblProjectTitle.ToolTip = Title;
                }
                if (!string.IsNullOrEmpty(sectorText) && sectorText.Length > 25)
                {
                    lblSector.Text = UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.BCCISector]).Substring(0, 25) + "...";
                    lblSector.ToolTip = sectorText;
                }
                TicketPath = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(recentProject[DatabaseObjects.Columns.TicketId]))];
            }
        }
    }
}