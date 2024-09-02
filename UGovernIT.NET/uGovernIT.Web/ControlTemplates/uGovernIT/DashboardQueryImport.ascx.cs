using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using DevExpress.Web.Rendering;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web.UI;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class DashboardQueryImport : UserControl
    {
        Dashboard Dashboard;
        List<Dashboard> allDashBoards = null;
        UserProfileManager uprofilemanager = null;
        public string UserId;
        DashboardManager objDashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        QueryHelperManager queryHelper = new QueryHelperManager(HttpContext.Current.GetManagerContext());
        Dashboard dashboardError;
        Dictionary<string, List<string>> dictMissingColumns = null;
        bool hasMissingColumns;

        protected void Page_Load(object sender, EventArgs e)
        {
            uprofilemanager = new UserProfileManager(context);
            allDashBoards = objDashboardManager.Load();
            //allDashBoards = Dashboard.(false, context);
        }

        protected void btnCreateDashboardQuery_Click(object sender, EventArgs e)
        {
            List<string> lstFiles = new List<string>();
            foreach (GridViewRow row in gvDashboardQuery.Rows)
            {
                Label lblDashboardName = (Label)row.Cells[0].FindControl("lblDashboardText");
                string dashboardName = lblDashboardName.Text;
                lstFiles.Add(dashboardName);
            }
            List<Dashboard> lstDashboard = new List<Dashboard>();
            if (lstFiles != null && lstFiles.Count > 0)
            {
                foreach (string fileName in lstFiles)
                {
                    Dashboard uDashboard = new Dashboard();
                    string text = System.IO.File.ReadAllText(System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(text);
                        
                        uDashboard = (Dashboard)UGITUtility.DeSerializeAnObject(xmlDoc, uDashboard);
                        if (uDashboard == null)
                            continue;
                        
                        lstDashboard.Add(uDashboard);
                    }
                    catch (Exception ex)
                    {
                        Util.Log.ULog.WriteException(ex, string.Format("File {0} not imported", fileName));
                    }
                }
            }
            if (lstDashboard != null && lstDashboard.Count > 0)
            {
                foreach (Dashboard dash in lstDashboard)
                {
                    Dashboard = dash;
                    dashboardError = new Dashboard();
                    CreateDashboardQuery();
                }

                //Refresh cache
                //uGITCache.RefreshList(DatabaseObjects.Tables.DashboardPanels);

                if (!hasMissingColumns)
                    uHelper.ClosePopUpAndEndResponse(Context, true);

            }
           
        }

        protected void btImportFiles_Click(object sender, EventArgs e)
        {
            int numFiles = Request.Files.Count;
            if (numFiles < 1)
            {
                lblErrorMessage.Visible = true;
            }
            else
            {
                List<string> lstFiles = new List<string>();

                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFile postedfile = Request.Files[i];
                    if ((!string.IsNullOrEmpty(postedfile.FileName) && postedfile.FileName.EndsWith(".zip")) || !postedfile.FileName.EndsWith(".xml"))
                        continue;

                    if (!string.IsNullOrEmpty(postedfile.FileName))
                    {
                        System.IO.Stream inStream = postedfile.InputStream;
                        byte[] fileData = new byte[postedfile.ContentLength];
                        inStream.Read(fileData, 0, postedfile.ContentLength);
                        string fileName = Path.GetFileName(postedfile.FileName);
                        postedfile.SaveAs(System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                        lstFiles.Add(fileName);
                    }
                }
                if (lstFiles != null && lstFiles.Count > 0)
                {
                    lstFiles = lstFiles.Distinct().ToList();
                    trServices.Style.Add("visibility", "visible");
                    gvDashboardQuery.DataSource = lstFiles;
                    gvDashboardQuery.DataBind();
                }
                else
                {
                    trServices.Style.Add("visibility", "hidden");
                }
            }
        }

        private void CreateDashboardQuery()
        {
            if (Dashboard != null)
            {
                Dashboard newDashboard = Dashboard;
                newDashboard.ID = 0;

                string newDashBoardTitle = newDashboard.Title;
                if (allDashBoards.Exists(x => x.Title == newDashBoardTitle))//if query or dashboard not exist first time
                    newDashBoardTitle = newDashboard.Title + "-Copy";

                int i = 1;
                while (allDashBoards.Exists(x => x.Title == newDashBoardTitle))
                {
                    newDashBoardTitle = newDashboard.Title + "-Copy" + i;
                    i++;
                }
                newDashboard.Title = newDashBoardTitle;
                newDashboard.TenantID = context.TenantID;
                if (newDashboard.panel != null)
                {
                    newDashboard.panel.ContainerTitle = newDashBoardTitle;
                    newDashboard.panel.DashboardID = Guid.NewGuid();
                }
                if (newDashboard.DashboardType == DashboardType.Chart)
                {
                    ChartSetting chart = newDashboard.panel as ChartSetting;
                    if (chart != null)
                    {
                        chart.ChartId = Guid.NewGuid();
                        newDashboard.panel = chart;
                    }
                }
                else if (newDashboard.DashboardType == DashboardType.Panel)
                {
                    PanelSetting panel = newDashboard.panel as PanelSetting;
                    
                    if (panel != null)
                    {
                        panel.ContainerTitle = newDashboard.Title;
                        panel.PanelID = Guid.NewGuid();
                        foreach (DashboardPanelLink link in panel.Expressions)
                        {
                            link.LinkID = Guid.NewGuid();
                        }
                        newDashboard.panel = panel;
                    }
                }
                else if (newDashboard.DashboardType == DashboardType.Query)
                {
                    DashboardQuery query = newDashboard.panel as DashboardQuery;
                    if (query != null)
                    {
                        query.QueryId = Guid.NewGuid();

                        // Update table name for each column in where clause if it is null or empty i.e. for old queries.
                        List<WhereInfo> lstWhereClause = QueryHelperManager.UpdateTableNameInWhereClause(query);

                        if (lstWhereClause != null && lstWhereClause.Count > 0)
                            query.QueryInfo.WhereClauses = lstWhereClause;

                        // Find the columns details from the query which aren't available in Request Lists
                        dictMissingColumns = queryHelper.GetMissingModuleColumns(query);

                        // Check if the query has missing columns and assign the value to QueryInfo.MissingColumns on it's basis
                        hasMissingColumns = dictMissingColumns != null && dictMissingColumns.Any(x => x.Value.Count > 0);

                        if (hasMissingColumns)
                            query.QueryInfo.MissingColumns = queryHelper.GetMissingColInString(dictMissingColumns);
                        else
                            query.QueryInfo.MissingColumns = string.Empty;

                        newDashboard.panel = query;
                    }
                }

                allDashBoards.Add(newDashboard);
                byte[] iconContents = new byte[0];

                // Save/Update dashboard
                objDashboardManager.SaveDashboardPanel(iconContents, string.Empty, true, newDashboard);

                //Bind Missing columns grid if missing columns are found
                if (hasMissingColumns)
                {
                    missingColumnsGrid.DataBind();
                    missingColumnsContainer.ShowOnPageLoad = true;
                }
            }
        }

        /// <summary>
        /// Data Binding Method of Missing Module Column grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void missingColumnsGrid_DataBinding(object sender, EventArgs e)
        {
            missingColumnsGrid.DataSource = queryHelper.GetMissingModuleColumns(dictMissingColumns);
        }

        /// <summary>
        /// Method to Close Missing Column Popup and Proceed the Functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            missingColumnsContainer.ShowOnPageLoad = false;
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
        
    
    