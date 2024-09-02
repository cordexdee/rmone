using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.IO;
using System.Text;
using System.Xml;
using System.IO.Compression;
using DevExpress.Web;
using System.Configuration;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class ConfigDashboard : UserControl
    {
        private string orderBy = "ASC";
        private string dashboardType = "All";
        public string delegatePageUrl;
        public string newDashboardUIurl;
        protected string importUrl;
        protected string FactTableUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboardfacttable");
        ApplicationContext context;
        DashboardManager objDashboard = new DashboardManager(HttpContext.Current.GetManagerContext());
        ChartTemplatesManager objChartTemplatesManager = new ChartTemplatesManager(HttpContext.Current.GetManagerContext());

        public ConfigDashboard()
        {
            context = HttpContext.Current.GetManagerContext();
        }

        protected override void OnInit(EventArgs e)
        {
            Bindgrid();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            delegatePageUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx");
            newDashboardUIurl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx");

            string importUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=dashboardimport");
            btImport.ClientSideEvents.Click = "function(s,e){" + string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}' , '', 'Import Dashboard & Query', '90', '90', 0, '{1}')", importUrl, Server.UrlEncode(Request.Url.AbsolutePath)) + "}";
            btImport.PostBackUrl = "javascript:void(0)";
            string analyticUrl = ConfigurationManager.AppSettings["AnalyticUrl"];
            if (!string.IsNullOrWhiteSpace(analyticUrl))
            {
                btnAnalytics.Visible = true;
                btnAnalytics.ClientSideEvents.Click = $"function(s,e){{window.parent.UgitOpenPopupDialog('{analyticUrl}/analytics' , '', 'Analytics', '100', '100', 0, '{Server.UrlEncode(Request.Url.AbsolutePath)}')}}";
            }

            if (!Page.IsPostBack)
            {
                ddlQueryTables.DataSource = uHelper.GetAllTableFromDatabase();
                ddlQueryTables.DataBind();
                ddlQueryTables.Items.Insert(0, new ListItem("--Select--", "-1"));
                lvDashbaords.DataBind();
                if (UGITUtility.GetCookie(Request, "orderBy") != null)
                    orderBy = UGITUtility.GetCookieValue(Request, "orderBy");

                if (UGITUtility.GetCookie(Request, "dashboardType") != null)
                    dashboardType = UGITUtility.GetCookieValue(Request, "dashboardType");
            }

            if (lvDashbaords.DataSource != null && lvDashbaords.Columns.Count > 0)
            {
                lvDashbaords.Columns[DatabaseObjects.Columns.ID].Width = Unit.Percentage(8);
                lvDashbaords.Columns[DatabaseObjects.Columns.ItemOrder].Width = Unit.Percentage(7);
                lvDashbaords.Columns[DatabaseObjects.Columns.DashboardType + "$"].Width = Unit.Percentage(8);
                lvDashbaords.Columns[DatabaseObjects.Columns.CategoryName].Width = Unit.Percentage(12);
                lvDashbaords.Columns[DatabaseObjects.Columns.SubCategory].Width = Unit.Percentage(13);
                lvDashbaords.Columns[DatabaseObjects.Columns.Title].Width = Unit.Percentage(15);
                lvDashbaords.Columns[DatabaseObjects.Columns.ModifiedByUser + "$"].Width = Unit.Percentage(12);
                lvDashbaords.Columns[DatabaseObjects.Columns.Modified].Width = Unit.Percentage(13);

                // Action buttons column
                GridViewDataColumn commandColumn = (GridViewDataColumn)lvDashbaords.Columns[lvDashbaords.Columns.Count - 1];
                if (commandColumn != null)
                    commandColumn.Width = Unit.Percentage(7);
                lvDashbaords.DataBind();
            }

            if (Request["__EVENTTARGET"] != null)
            {
                if (Convert.ToString(Request["__EVENTTARGET"]).Contains("DeleteQuery"))
                {
                    long ID = 0;
                    UGITUtility.IsNumber(Request["__EVENTARGUMENT"], out ID);
                    if (ID != 0)
                        DeleteQuery(Convert.ToInt64(ID));
                }
            }
            
        }

        // Added below method, as Queries/Panels/Charts that are displayed after scrolling down, are not deleted.
        private void DeleteQuery(long panelId)
        {
            objDashboard.DeleteDashboard(panelId);
            Bindgrid();
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Get dashboard tables and bind it with ASPXGridview            
            //lvDashbaords.DataBind();
            // fill template in template dropdowns 
            ddlChartPanelTemplates.Items.Clear();
            ddlKPIPanelTemplates.Items.Clear();
            ddlQueryPanelTemplates.Items.Clear();
            DataTable templateTable = objChartTemplatesManager.GetDataTable();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ChartTemplates);

            if (templateTable != null && templateTable.Rows.Count > 0)
            {
                foreach (DataRow item in templateTable.Rows)
                {
                    if (Convert.ToString(item[DatabaseObjects.Columns.TemplateType]) == ((int)DashboardType.Panel).ToString())
                    {
                        ddlKPIPanelTemplates.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                    else if (Convert.ToString(item[DatabaseObjects.Columns.TemplateType]) == ((int)DashboardType.Chart).ToString())
                    {
                        ddlChartPanelTemplates.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                    else if (Convert.ToString(item[DatabaseObjects.Columns.TemplateType]) == ((int)DashboardType.Query).ToString())
                    {
                        ddlQueryPanelTemplates.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                }
            }

            ddlKPIPanelTemplates.Items.Insert(0, new ListItem("New Panel", "0"));
            ddlChartPanelTemplates.Items.Insert(0, new ListItem("New Chart", "0"));
            ddlQueryPanelTemplates.Items.Insert(0, new ListItem("New Query", "0"));
            ddlKPIPanelTemplates.Attributes.Add("onChange", "customChangeOnTemplate(this)");
            ddlChartPanelTemplates.Attributes.Add("onChange", "customChangeOnTemplate(this)");
            ddlQueryPanelTemplates.Attributes.Add("onChange", "customChangeOnTemplate(this)");

            //select table for dashboard
            //ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            // DataTable modules = moduleViewManager.LoadAllModules();

            if (!ddlDashboardTables.Items.Contains(new ListItem("--Select--", "")))
                ddlDashboardTables.Items.Insert(0, new ListItem("--Select--", ""));

            base.OnPreRender(e);
        }

        protected void DdlDashboardTables_Load(object sender, EventArgs e)
        {
            if (ddlDashboardTables.Items.Count <= 0)
            {

                List<string> factTables = DashboardCache.DashboardFactTables(HttpContext.Current.GetManagerContext());
                if (factTables.Count > 0)
                {
                    foreach (string factTable in factTables)
                    {
                        ddlDashboardTables.Items.Add(new ListItem(factTable, factTable));
                        ddlCachedData.Items.Add(new ListItem(factTable, factTable));
                    }
                }
            }
        }

        //protected void BtTicketDelete_Click(object sender, EventArgs e)
        //{   
        //    long panelId = 0;
        //    long.TryParse(Convert.ToString(hdIdToDelete.Value), out panelId);
        //    objDashboard.DeleteDashboard(panelId);            
        //}

        protected void treeList_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {

            }
        }

        protected void btMigrateInNewDesign_Click(object sender, EventArgs e)
        {
            List<Dashboard> chartDashboards = objDashboard.LoadDashboardPanelsByType(DashboardType.Chart,true,context.TenantID);
            foreach (Dashboard dashboard in chartDashboards)
            {
                ChartSetting chart = dashboard.panel as ChartSetting;
                if (chart == null)
                    continue;

                foreach (ChartExpression expression in chart.Expressions)
                {
                    if (chart.Dimensions == null || chart.Dimensions.Count == 0)
                        break;

                    expression.LabelText = chart.LabelText.Replace("$Exp$", "{V}");
                }

                foreach (ChartDimension dimension in chart.Dimensions)
                {
                    dimension.AxisLabelStyleAngle = chart.AxisLabelStyleAngle;
                }

                objDashboard.SaveDashboardPanel();
            }
        }

        protected void lvDashbaords_DataBound(object sender, EventArgs e)
        {
            Bindgrid();
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            var selectedIds = lvDashbaords.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            List<Dashboard> allDashBoards = objDashboard.LoadAll(false);

            if (allDashBoards == null || allDashBoards.Count == 0)
                return;

            Dashboard uDashboard = null;
            foreach (long dashboardID in selectedIds)
            {
                //long dashboardID;
                //long.TryParse(yID, out dashboardID);

                if (dashboardID > 0)
                {
                    uDashboard = allDashBoards.FirstOrDefault(x => x.ID == dashboardID);
                    if (uDashboard == null)
                        return;
                    Dashboard newDashboard = uDashboard;
                    newDashboard.ID = 0;

                    string newDashBoardTitle = newDashboard.Title + "-Copy";
                    int i = 1;
                    while (allDashBoards.Exists(x => x.Title == newDashBoardTitle))
                    {
                        newDashBoardTitle = newDashboard.Title + "-Copy" + i;
                        i++;
                    }
                    newDashboard.Title = newDashBoardTitle;
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
                        panel.ContainerTitle = newDashboard.Title;
                        if (panel != null)
                        {
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
                            newDashboard.panel = query;
                        }
                    }
                    byte[] iconContents = new byte[0];
                    string fileName = string.Empty;
                    objDashboard.SaveDashboardPanel(iconContents, fileName, false, newDashboard);
                }
                
            }
            Bindgrid();
        }

        /// <summary>
        /// Fills chart template list which will be used to override template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlTemplateList_PreRender(object sender, EventArgs e)
        {
            if (ddlTemplateList.Items.Count <= 0)
            {
                LoadTemplateList();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var selectedIds = lvDashbaords.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            Dashboard uDashboard = null;
            int dashboardID = UGITUtility.StringToInt(selectedIds.FirstOrDefault());

            if (dashboardID > 0)
                uDashboard = objDashboard.LoadPanelById(dashboardID, false);

            if (uDashboard != null)
            {
                bool createNew = true;
                ChartTemplate item = null;
                List<ChartTemplate> chartTemplate = objChartTemplatesManager.Load();

                if (saveTemplate.Checked)
                {
                    var templates = chartTemplate.Where(x => x.Title == templateName.Text.Trim());
                    if (templates != null && templates.Count() > 0)
                    {
                        lblmsg.Style.Add("display", "");

                        lblmsg.Text = "Template already exists! please enter different name.";
                        return;
                    }
                    item = new ChartTemplate();
                    item.Title = templateName.Text;
                }
                else if (overrideTemplate.Checked)
                {
                    int templateID = 0;
                    int.TryParse(ddlTemplateList.SelectedValue.Trim(), out templateID);
                    if (templateID > 0)
                    {
                        item = chartTemplate.FirstOrDefault(x => x.ID == templateID);
                        item.Title = ddlTemplateList.SelectedItem.Text;
                        createNew = false;
                    }
                    else
                    {
                        item = new ChartTemplate();
                        item.Title = uDashboard.Title;
                    }
                }

                uDashboard.ID = 0;
                System.Xml.XmlDocument xmlDoc = uHelper.SerializeObject((object)uDashboard);


                item.ChartObject = xmlDoc.OuterXml;
                if (uDashboard.DashboardType == DashboardType.Panel)
                    item.TemplateType = "0";
                else if (uDashboard.DashboardType == DashboardType.Chart)
                    item.TemplateType = "1";
                else if (uDashboard.DashboardType == DashboardType.Query)
                    item.TemplateType = "2";
                if (createNew)
                {
                    objChartTemplatesManager.Insert(item);
                    LoadTemplateList();
                }
                else
                    objChartTemplatesManager.Update(item);


                ListItem templateItem = ddlTemplateList.Items.FindByValue(item.ID.ToString());
                if (templateItem != null)
                {
                    ddlTemplateList.SelectedIndex = ddlTemplateList.Items.IndexOf(templateItem);
                }

            }

            pcTemplate.ShowOnPageLoad = false;
            var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
            if (!string.IsNullOrWhiteSpace(refreshParentID))
            {
                UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
            }
        }

        private void LoadTemplateList()
        {
            List<ChartTemplate> templateTable = objChartTemplatesManager.Load();
            ddlTemplateList.Items.Clear();
            if (templateTable != null && templateTable.Count > 0)
            {
                foreach (ChartTemplate item in templateTable)
                {
                    ddlTemplateList.Items.Add(new ListItem(item.Title, Convert.ToString(item.ID)));
                }
            }
            ddlTemplateList.Items.Insert(0, new ListItem("--New Template--", "0"));
        }

        protected void btExport_Click(object sender, EventArgs e)
        {
            var selectedIds = lvDashbaords.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            if (selectedIds == null || selectedIds.Count == 0)
                return;

            List<Dashboard> allDashBoards = objDashboard.LoadAll(false);
            if (allDashBoards == null || allDashBoards.Count == 0)
                return;
            var lstKnownType = new List<Type>();
            lstKnownType.Add(typeof(ChartSetting));
            Dashboard uDashboard = null;
            if (selectedIds.Count == 1)
            {
                uDashboard = allDashBoards.FirstOrDefault(x => x.ID == Convert.ToInt32(selectedIds[0]));
                if (uDashboard == null)
                    return;

                string dashboardNameForFile = UGITUtility.ReplaceInvalidCharsInFolderName(uDashboard.Title).Replace(",", " ");
                string tempFolderPath = uHelper.GetTempFolderPath();
                string filePath = Path.Combine(tempFolderPath, string.Format("{0}.xml", dashboardNameForFile));
                try
                {
                    if (!Directory.Exists(tempFolderPath))
                        Directory.CreateDirectory(tempFolderPath);

                    using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        XmlDocument xmlDoc = uHelper.SerializeObject(uDashboard);
                        byte[] bytes = Encoding.Default.GetBytes(xmlDoc.OuterXml);
                        writer.Write(bytes, 0, Encoding.Default.GetByteCount(xmlDoc.OuterXml));
                        writer.Close();
                    }

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/xml";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + string.Format("{0}.xml", dashboardNameForFile));
                    Response.TransmitFile(filePath);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex, "Error exporting selected dashboards");
                }
                finally
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            else if (selectedIds.Count > 1)
            {
                string sourecFileName = string.Format("DashboardQuery_{0}", DateTime.Now.ToString("yyyyMMdd"));
                string sourceUrl = System.IO.Path.Combine(uHelper.GetTempFolderPath(), sourecFileName);
                string ZipFolderName = System.IO.Path.Combine(uHelper.GetTempFolderPath(), string.Format("DashboardQuery_{0}.zip", DateTime.Now.ToString("yyyyMMdd")));
                try
                {
                    if (!Directory.Exists(sourceUrl))
                        Directory.CreateDirectory(sourceUrl);

                    foreach (var id in selectedIds)
                    {
                        uDashboard = allDashBoards.FirstOrDefault(x => x.ID == Convert.ToInt64(id));
                        if (uDashboard == null)
                            return;

                        string dashboardNameForFile = UGITUtility.ReplaceInvalidCharsInFolderName(uDashboard.Title).Replace(",", " ");
                        string fileName = System.IO.Path.Combine(sourceUrl, string.Format("{0}.xml", dashboardNameForFile));
                        using (FileStream writer = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                        {
                            XmlDocument xmlDoc = uHelper.SerializeObject(uDashboard);
                            try
                            {
                                byte[] bytesArr = Encoding.Default.GetBytes(xmlDoc.OuterXml);
                                writer.Write(bytesArr, 0, Encoding.Default.GetByteCount(xmlDoc.OuterXml));
                            }
                            catch (Exception ex)
                            {
                                Util.Log.ULog.WriteException(ex, "Error exporting selected dashboards");
                            }
                        }
                    }

                    UGITUtility.DeleteFolder(ZipFolderName);
                    if (Directory.Exists(sourceUrl))        // Create .zip file
                        ZipFile.CreateFromDirectory(sourceUrl, ZipFolderName, CompressionLevel.Fastest, true);

                    FileStream fs = new FileStream(ZipFolderName, FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/zip";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(string.Format("{0}.zip", sourecFileName), System.Text.Encoding.UTF8));
                    Response.TransmitFile(ZipFolderName);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex, "Error in export");
                }
                finally
                {
                    if (File.Exists(ZipFolderName))
                        File.Delete(ZipFolderName);

                    if (Directory.Exists(sourceUrl))
                        Directory.Delete(sourceUrl, true);
                }
            }
        }
        private void Bindgrid()
        {
            DataTable dashboardTable = null;
            lvDashbaords.DataSource = null;
            if (dashboardTable == null)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                dashboardTable = GetTableDataManager.GetData(DatabaseObjects.Tables.DashboardPanels, values);
            }
            if (dashboardTable != null)
            {
                DataView view = dashboardTable.DefaultView;
                view.Sort = string.Format("{0} {1}", DatabaseObjects.Columns.ItemOrder, orderBy);
                if (dashboardType != "All")
                {
                    view.RowFilter = string.Format("{0}='{1}'", DatabaseObjects.Columns.DashboardType, dashboardType);
                }
                lvDashbaords.DataSource = view;
            }
            else
            {
                lvDashbaords.DataSource = null;
            }
            lvDashbaords.DataBind();
        }

        protected void AspxCallbackDashboardPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            Bindgrid();
        }

        protected void lvDashbaords_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string func = string.Format("javascript:editThisDashboard('{0}', '{1}', '{2}');", e.KeyValue, e.GetValue(DatabaseObjects.Columns.Title), e.GetValue(DatabaseObjects.Columns.DashboardType + "$"));
            for (int i = 1; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].Attributes.Add("onClick", func);
                e.Row.Cells[i].Attributes.Add("style", "cursor:pointer");
            }

        }
    }
}

