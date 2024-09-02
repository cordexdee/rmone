using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager.Managers
{
    public class DashboardManager : ManagerBase<Dashboard>, IDashboardManager
    {
        ApplicationContext _context;

        public DashboardManager(ApplicationContext context) : base(context)
        {
            _context = this.dbContext;
            store = new DashboardStore(this.dbContext);
        }

        public List<Dashboard> LoadDashboardPanels(int dashboardGroupId)
        {
            return LoadDashboardPanels(dashboardGroupId, true);
        }

        public List<Dashboard> LoadDashboardPanels(int dashboardGroupId, bool useCache)
        {
            var dashboardPanelViewManager = new DashboardPanelViewManager(_context);
            var dashboards = new List<Dashboard>();

            if (useCache)
            {
                DataRow[] panelGroups = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanelGroup, string.Format("{0}={1}", DatabaseObjects.Columns.Id, dashboardGroupId)).Select();
                if (panelGroups.Length > 0)
                {
                    DataTable dashboardPanelTable = dashboardPanelViewManager.GetDataTable();  // GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels);
                    if (dashboardPanelTable != null)
                    {
                        DataTable dashbaordPanelTable1 = dashboardPanelTable.Copy();
                        dashbaordPanelTable1.Columns.Add("DashboardMultiLookup1");
                        dashbaordPanelTable1.Columns["DashboardMultiLookup1"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.DashboardMultiLookup);
                        if (dashbaordPanelTable1 != null && dashbaordPanelTable1.Rows.Count > 0)
                        {
                            DataRow[] panels = dashbaordPanelTable1.Select(string.Format("DashboardMultiLookup1 LIKE '%{0}{1}{0}%'", Constants.Separator, panelGroups[0][DatabaseObjects.Columns.Title]));
                            foreach (DataRow item in panels)
                            {
                                dashboards.Add(GetDashboardObj(item));
                            }
                            dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
                        }
                    }
                }
            }
            else
            {
                string query = string.Format("{0}={1}", DatabaseObjects.Columns.DashboardMultiLookup, dashboardGroupId);
                DataRow[] collection = dashboardPanelViewManager.GetDataTable().Select(query);  // GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels).Select(query);

                if (collection.Count() > 0)
                {
                    foreach (DataRow item in collection)
                    {
                        dashboards.Add(GetDashboardObj(item));
                    }
                }
                dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
            }

            return dashboards;
        }

        public List<Dashboard> LoadDashboardPanels(List<long> panels)
        {
            return LoadDashboardPanels(panels, true);
        }

        public List<Dashboard> LoadDashboardPanels(List<long> panels, bool useCache)
        {
            var dashboards = new List<Dashboard>();

            if (panels.Count <= 0)
            {
                return dashboards;
            }

            if (useCache)
            {
                List<string> dNames = new List<string>();
                foreach (int dID in panels)
                {
                    Dashboard dashboard = CacheHelper<Dashboard>.Get(dID.ToString(), dbContext.TenantID);
                    if (dashboard == null)
                    {
                        dashboard = store.Get(x => x.ID == dID);
                        if (dashboard != null)
                        {
                            LoadProperty(dashboard);
                            CacheHelper<Dashboard>.AddOrUpdate(dID.ToString(), dbContext.TenantID, dashboard);
                        }
                    }

                    if (dashboard != null)
                        dashboards.Add(dashboard);
                }
            }
            else
            {
                dashboards = store.Load(x => panels.Contains(x.ID));
                foreach (Dashboard d in dashboards)
                {
                    LoadProperty(d);
                }
            }

            return dashboards;
        }

        public List<Dashboard> LoadDashboardsByNames(List<string> panels)
        {
            return LoadDashboardsByNames(panels, true);
        }

        public List<Dashboard> LoadDashboardsByNames(List<string> panels, bool useCache)
        {
            var dashboards = new List<Dashboard>();

            if (panels.Count <= 0)
            {
                return dashboards;
            }

            if (useCache)
            {
                List<string> dNames = new List<string>();
                foreach (string dName in panels)
                {
                    Dashboard dashboard = CacheHelper<Dashboard>.Get(dName, dbContext.TenantID);
                    if (dashboard == null)
                    {
                        dashboard = store.Get(x => x.Title == dName);
                        if (dashboard != null)
                        {
                            LoadProperty(dashboard);
                            CacheHelper<Dashboard>.AddOrUpdate(dName, dbContext.TenantID, dashboard);
                        }
                    }

                    if (dashboard != null)
                        dashboards.Add(dashboard);
                }
            }
            else
            {
                dashboards = store.Load(x => panels.Contains(x.Title));
                foreach (Dashboard d in dashboards)
                {
                    LoadProperty(d);
                }
            }



            //// For loading analytic dashboards
            //StringBuilder queryAnalytic = new StringBuilder();
            //for (int i = 0; i < panels.Count; i++)
            //{
            //    if (i != 0)
            //    {
            //        queryAnalytic.Append(" OR ");
            //    }
            //    queryAnalytic.AppendFormat("{0}='{1}'", DatabaseObjects.Columns.Title, panels[i].Replace("'", "''"));
            //}

            //DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AnalyticDashboards);
            //DataView dv = new DataView(dt);
            //DataTable dtAnalyitc = dv.ToTable(false, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.AnalyticName, DatabaseObjects.Columns.DashboardID, DatabaseObjects.Columns.AnalyticVID);
            //if (dtAnalyitc != null && dtAnalyitc.Rows.Count > 0)
            //{
            //    DataRow[] dashboardAnalytics = dtAnalyitc.Select(queryAnalytic.ToString());
            //    foreach (DataRow dr in dashboardAnalytics)
            //    {
            //        Dashboard uDashboard = new Dashboard();
            //        uDashboard.Title = Convert.ToString(dr[DatabaseObjects.Columns.Title]);
            //        uDashboard.DashboardDescription = Convert.ToString(dr[DatabaseObjects.Columns.AnalyticName]);
            //        uDashboard.ID = Convert.ToInt16(dr[DatabaseObjects.Columns.DashboardID]);
            //        uDashboard.SecondryID = Convert.ToInt16(dr[DatabaseObjects.Columns.AnalyticVID]);
            //        uDashboard.DashboardType = DashboardType.Analytic;
            //        dashboards.Add(uDashboard);
            //    }
            //}

            return dashboards;
        }

        //public List<Dashboard> LoadDashboardPanelsByType(DashboardType dashboardType)
        //{
        //    return LoadDashboardPanelsByType(dashboardType, true);
        //}

        public List<Dashboard> LoadAll(bool useCache)
        {
            List<Dashboard> dashboards = new List<Dashboard>();

            DashboardManager dashboardManager = new DashboardManager(_context);
            if (useCache)
            {
                DataTable dashboardPanelTable = dashboardManager.GetDataTable();
                if (dashboardPanelTable != null)
                {
                    foreach (DataRow item in dashboardPanelTable.Rows)
                    {
                        dashboards.Add(GetDashboardObj(item));
                    }

                    if (dashboards.Count > 0)
                        dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
                }
            }
            else
            {
                DataRow[] collection = dashboardManager.GetDataTable().Select();
                if (collection != null && collection.Count() > 0)
                {
                    foreach (DataRow item in collection)
                    {
                        dashboards.Add(GetDashboardObj(item));
                    }
                }

                if (dashboards.Count > 0)
                    dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
            }

            // For loading analytic dashboards
            DataTable dtAnalyitc = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AnalyticDashboards, $"TenantID='{_context.TenantID}'");
            if (dtAnalyitc != null && dtAnalyitc.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAnalyitc.Rows)
                {
                    Dashboard uDashboard = new Dashboard();
                    uDashboard.Title = Convert.ToString(dr[DatabaseObjects.Columns.Title]);
                    uDashboard.DashboardDescription = Convert.ToString(dr[DatabaseObjects.Columns.AnalyticName]);
                    uDashboard.ID = Convert.ToInt16(dr[DatabaseObjects.Columns.DashboardID]);
                    uDashboard.SecondryID = Convert.ToInt16(dr[DatabaseObjects.Columns.AnalyticVID]);
                    uDashboard.DashboardType = DashboardType.Analytic;
                    dashboards.Add(uDashboard);
                }
            }
            return dashboards;
        }

        /// <summary>
        /// Load Dashboard panels by type
        /// </summary>
        /// <param name="dashboardType"></param>
        /// <param name="useCache"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Dashboard> LoadDashboardPanelsByType(DashboardType dashboardType, bool useCache, string TenantId)
        {
            List<Dashboard> dashboards = new List<Dashboard>();
            if (useCache)
            {
                DataRow[] dashboardPanelTable = null;

                dashboardPanelTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, string.Format("{0}='{1}' AND {2}={3} AND Deleted=0", DatabaseObjects.Columns.TenantID, TenantId, DatabaseObjects.Columns.DashboardType, (int)dashboardType)).Select();

                if (dashboardPanelTable != null)
                {
                    foreach (DataRow item in dashboardPanelTable)
                    {
                        dashboards.Add(GetDashboardObj(item));
                    }
                    dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();


                }
            }
            else
            {
                string query = string.Format("'{0}='{1}'", DatabaseObjects.Columns.DashboardType, dashboardType.ToString());
                DataRow[] collection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.TenantID} = '{this.dbContext.TenantID}'").Select(query);
                if (collection.Count() > 0)
                {
                    foreach (DataRow item in collection)
                    {
                        dashboards.Add(GetDashboardObj(item));
                    }
                }
                dashboards = dashboards.OrderBy(x => x.ItemOrder).ToList();
            }
            return dashboards;
        }

        private void LoadProperty(Dashboard dashboard)
        {
            List<string> listAttachedFile = UGITUtility.ConvertStringToList(dashboard.Attachments, Constants.Separator);
            if (listAttachedFile.Count > 0)
            {
                dashboard.Icon = UGITUtility.GetAbsoluteURL(listAttachedFile[0]);

            }

            if (!string.IsNullOrEmpty(dashboard.DashboardPanelInfo))
                dashboard.panel = DeSerializerPanel(dashboard.DashboardPanelInfo, dashboard.DashboardType);
        }

        private Dashboard GetDashboardObj(object itemObj)
        {
            Dashboard dashboard = new Dashboard();
            if (itemObj is Dashboard)
            {
                DataRow item = (DataRow)itemObj;
                dashboard.ID = int.Parse(item[DatabaseObjects.Columns.Id].ToString());
                int order = 0;
                if (int.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ItemOrder]), out order))
                    dashboard.ItemOrder = order;
                else
                    dashboard.ItemOrder = 0;
                dashboard.DashboardPermission = Convert.ToString(item[DatabaseObjects.Columns.DashboardPermission]);
                dashboard.Title = item[DatabaseObjects.Columns.Title] != null ? item[DatabaseObjects.Columns.Title].ToString() : string.Empty;
                dashboard.DashboardDescription = item[DatabaseObjects.Columns.DashboardDescription] != null ? item[DatabaseObjects.Columns.DashboardDescription].ToString() : string.Empty;
                dashboard.panel = DeSerializerPanel(item[DatabaseObjects.Columns.DashboardPanelInfo].ToString(), (DashboardType)Enum.Parse(typeof(DashboardType), item[DatabaseObjects.Columns.DashboardType].ToString()));
                dashboard.DashboardType = (DashboardType)Enum.Parse(typeof(DashboardType), item[DatabaseObjects.Columns.DashboardType].ToString());
                dashboard.IsShowInSideBar = item[DatabaseObjects.Columns.IsShowInSideBar] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsShowInSideBar]) : false;
                dashboard.IsHideTitle = item[DatabaseObjects.Columns.IsHideTitle] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsHideTitle]) : false;
                dashboard.IsHideDescription = item[DatabaseObjects.Columns.IsHideDescription] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsHideDescription]) : false;
                dashboard.PanelWidth = item[DatabaseObjects.Columns.PanelWidth] != null ? Convert.ToInt32(item[DatabaseObjects.Columns.PanelWidth].ToString()) : 200;
                dashboard.PanelHeight = item[DatabaseObjects.Columns.PanelHeight] != null ? Convert.ToInt32(item[DatabaseObjects.Columns.PanelHeight].ToString()) : 150;
                dashboard.ThemeColor = item[DatabaseObjects.Columns.ThemeColor] != null ? item[DatabaseObjects.Columns.ThemeColor].ToString() : "Accent1";

                dashboard.CategoryName = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.CategoryName));
                dashboard.SubCategory = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.SubCategory));
                dashboard.IsActivated = UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.IsActivated) ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsActivated]) : false;

                dashboard.HeaderFontStyle = item[DatabaseObjects.Columns.HeaderFontStyle] != null ? item[DatabaseObjects.Columns.HeaderFontStyle].ToString() : string.Empty;
                dashboard.FontStyle = item[DatabaseObjects.Columns.FontStyle] != null ? item[DatabaseObjects.Columns.FontStyle].ToString() : string.Empty;
                dashboard.TenantID = item[DatabaseObjects.Columns.TenantID] != null ? item[DatabaseObjects.Columns.TenantID].ToString() : string.Empty;
                // LookUpValueCollection moduleCollection = new LookUpValueCollection(this.dbContext, DatabaseObjects.Columns.DashboardModuleMultiLookup, Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.DashboardModuleMultiLookup)));

                string moduleCollection = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.DashboardModuleMultiLookup));

                if (!string.IsNullOrWhiteSpace(moduleCollection))
                {
                    dashboard.DashboardModuleMultiLookup = moduleCollection;
                }

                ///Authorized to View Field.
                string AuthorizedToViews = Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]);

                if (!string.IsNullOrWhiteSpace(AuthorizedToViews))
                {
                    dashboard.AuthorizedToView = AuthorizedToViews;
                }
                string Attachments = Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]);
                List<string> listAttachedFile = UGITUtility.ConvertStringToList(Attachments, Constants.Separator);

                if (!string.IsNullOrWhiteSpace(dashboard.Attachments) && listAttachedFile.Count > 0)
                {
                    dashboard.Icon = UGITUtility.GetAbsoluteURL(listAttachedFile[0]);
                }

            }
            else if (itemObj is DataRow)
            {
                DataRow item = (DataRow)itemObj;
                dashboard.ID = int.Parse(item[DatabaseObjects.Columns.Id].ToString());
                int order = 0;
                if (int.TryParse(Convert.ToString(item[DatabaseObjects.Columns.ItemOrder]), out order))
                    dashboard.ItemOrder = order;
                else
                    dashboard.ItemOrder = 0;
                // dashboard.Permissions = (SPFieldUserValueCollection)item[DatabaseObjects.Columns.DashboardPermission];
                dashboard.Title = item[DatabaseObjects.Columns.Title] != null ? Convert.ToString(item[DatabaseObjects.Columns.Title]) : string.Empty;
                dashboard.DashboardDescription = item[DatabaseObjects.Columns.DashboardDescription] != null ? Convert.ToString(item[DatabaseObjects.Columns.DashboardDescription]) : string.Empty;
                dashboard.panel = DeSerializerPanel(item[DatabaseObjects.Columns.DashboardPanelInfo].ToString(), (DashboardType)Enum.Parse(typeof(DashboardType), item[DatabaseObjects.Columns.DashboardType].ToString()));
                dashboard.DashboardType = (DashboardType)Enum.Parse(typeof(DashboardType), item[DatabaseObjects.Columns.DashboardType].ToString());
                dashboard.IsShowInSideBar = item[DatabaseObjects.Columns.IsShowInSideBar] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsShowInSideBar]) : false;
                dashboard.IsHideTitle = item[DatabaseObjects.Columns.IsHideTitle] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsHideTitle]) : false;
                dashboard.IsHideDescription = item[DatabaseObjects.Columns.IsHideDescription] != null ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsHideDescription]) : false;
                dashboard.PanelWidth = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.PanelWidth)) != string.Empty ? Convert.ToInt32(item[DatabaseObjects.Columns.PanelWidth]) : 200;
                dashboard.PanelHeight = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.PanelHeight)) != string.Empty ? Convert.ToInt32(item[DatabaseObjects.Columns.PanelHeight]) : 150;
                dashboard.ThemeColor = item[DatabaseObjects.Columns.ThemeColor] != null ? item[DatabaseObjects.Columns.ThemeColor].ToString() : "Accent1";
                dashboard.FontStyle = item[DatabaseObjects.Columns.FontStyle] != null ? item[DatabaseObjects.Columns.FontStyle].ToString() : string.Empty;
                dashboard.HeaderFontStyle = item[DatabaseObjects.Columns.HeaderFontStyle] != null ? item[DatabaseObjects.Columns.HeaderFontStyle].ToString() : string.Empty;
                dashboard.TenantID = item[DatabaseObjects.Columns.TenantID] != null ? item[DatabaseObjects.Columns.TenantID].ToString() : string.Empty;
                dashboard.CategoryName = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.CategoryName));
                dashboard.SubCategory = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.SubCategory));
                dashboard.IsActivated = UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.IsActivated) ? UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsActivated]) : false;
                dashboard.DashboardModuleMultiLookup = item[DatabaseObjects.Columns.DashboardModuleMultiLookup] != null ? Convert.ToString(item[DatabaseObjects.Columns.DashboardModuleMultiLookup]) : string.Empty;
                /*
                string modules = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.DashboardModuleMultiLookup));
                StringBuilder moduleCollection = new StringBuilder();
                if (!string.IsNullOrEmpty(modules))
                {
                    modules = System.Text.RegularExpressions.Regex.Replace(modules, string.Format("{0}[0-9]*{0}", Constants.Separator), ",");
                    List<string> moduleList = UGITUtility.ConvertStringToList(modules, new string[] { "," });
                    ModuleViewManager module = new ModuleViewManager(this.dbContext);
                    DataTable moduleTable = module.GetDataTable();
                    foreach (DataRow row in moduleTable.Rows)
                    {
                        if (moduleCollection.ToString() != string.Empty)
                        {
                            moduleCollection.Append(Constants.Separator);
                        }
                        if (moduleList.Exists(x => x == Convert.ToString(row[DatabaseObjects.Columns.ModuleName])))
                        {
                            moduleCollection.AppendFormat("{0}{1}{2}", row[DatabaseObjects.Columns.Id], Constants.Separator, row[DatabaseObjects.Columns.ModuleName]);
                        }
                    }

                    string moduleValueCollection = moduleCollection.ToString();
                    if (!string.IsNullOrWhiteSpace(moduleValueCollection))// != null && moduleValueCollection.Count > 0)
                    {
                        dashboard.DashboardModuleMultiLookup = moduleValueCollection;
                    }
                }
                */
                //Commented Preivously how we imported from sharepoint

                /////Authorized to View Field.
                //SPFieldUserValueCollection AuthorizedToViewColl = new SPFieldUserValueCollection(, Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]));
                string Attachments = Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]);
                List<string> listAttachedFile = UGITUtility.ConvertStringToList(Attachments, Constants.Separator);
                if (!string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView])))
                {
                    dashboard.AuthorizedToView = Convert.ToString(item[DatabaseObjects.Columns.AuthorizedToView]);
                }

                if (listAttachedFile.Count > 0)
                {
                    dashboard.Icon = UGITUtility.GetAbsoluteURL(listAttachedFile[0]);
                }

            }

            return dashboard;
        }

        public long SaveDashboardPanel(byte[] iconContents, string fileName, bool removeIcon, Dashboard objUDashboard)
        {
            long rowEffected = 0;

            if (objUDashboard != null)
            {
                if (removeIcon)
                {
                    if (!string.IsNullOrWhiteSpace(objUDashboard.Attachments))
                    {
                        objUDashboard.Attachments = "";
                    }
                }
                if (fileName != null && fileName.Trim() != string.Empty && iconContents.Count() > 0)
                {
                    //if (item.Attachments.Count > 0)
                    //{
                    //    item.Attachments.Delete(item.Attachments[0].ToString());
                    //}
                    //item.Attachments.Add(fileName, iconContents);
                    objUDashboard.Attachments = ""; //path of attched icons
                }
                objUDashboard.DashboardPanelInfo = SerializerPanel(objUDashboard.panel);
                rowEffected = (store as DashboardStore).InsertORUpdateData(objUDashboard).ID;
                CacheHelper<Dashboard>.AddOrUpdate(objUDashboard.Title, this.dbContext.TenantID, objUDashboard);
                CacheHelper<Dashboard>.AddOrUpdate(objUDashboard.ID.ToString(), this.dbContext.TenantID, objUDashboard);
            }
            else
            {
                rowEffected = 0;
            }
            return rowEffected;
        }

        public long SaveDashboardPanel(byte[] iconContents, string fileName)
        {
            return SaveDashboardPanel(iconContents, fileName, false, null);
        }

        public long SaveDashboardPanel(bool removeIcon)
        {
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            return SaveDashboardPanel(iconContents, fileName, removeIcon, null);
        }

        public long SaveDashboardPanel()
        {
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            return SaveDashboardPanel(iconContents, fileName, false, null);
        }

        public Dashboard LoadPanelById(long ID)
        {
            return LoadPanelById(ID, false);
        }

        public Dashboard LoadPanelById(long id, bool useCache)
        {
            if (id <= 0)
            {
                return null;
            }

            Dashboard dashboard = null;
            var query = $"{DatabaseObjects.Columns.ID}='{id}' And {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";

            if (useCache)
            {
                ////DataRow[] dashboardPanels = this.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, ID));                
                ////var _dashboardPanels = this.GetDataTable(query).Select();
                //var dashboardPanels = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, query).Select();
                //if (dashboardPanels.Any())
                //{
                //    dashboard = GetDashboardObj(dashboardPanels[0]);
                //}
                 dashboard = CacheHelper<Dashboard>.Get(id.ToString(), _context.TenantID);
                if (dashboard == null)
                {
                    dashboard = store.Get(x => x.ID == id);
                    if (dashboard != null)
                    {
                        LoadProperty(dashboard);
                        CacheHelper<Dashboard>.AddOrUpdate(id.ToString(), _context.TenantID, dashboard);
                    }
                }
            }
            else
            {
                dashboard = new Dashboard();
                //var item = this.GetDataTable().Select(query);
                var item = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, query).Select();
                if (item.Any())
                    dashboard = GetDashboardObj(item[0]);

                //Added By Munna
                //this.LoadByID(ID);
                //dashboard.panel = DeSerializerPanel(dashboard.DashboardPanelInfo, (DashboardType)Enum.Parse(typeof(DashboardType), dashboard.DashboardType.ToString()));
            }

            return dashboard;
        }

        public int DeleteDashboard(long ID)
        {
            Dashboard item = this.LoadByID(ID);//.GetDataTable().Select(string.Format("{0}={1}",DatabaseObjects.Columns.ID, ID))[0];
            if (item != null)
            {
                this.Delete(item);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Check whether panel has permission or not
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool HasPermission(/*SPUser user*/ UserProfile user, Dashboard objDashboard)
        {
            bool exist = false;
            if (objDashboard.DashboardPermission != null)
            {
                if (objDashboard.DashboardPermission.Contains(user.Id))
                {
                    exist = true;
                }
                else
                {
                    //foreach (SPGroup group in user.Groups)
                    //{
                    //    foreach (SPFieldUserValue usrval in this.Permissions)
                    //    {
                    //        if (usrval.LookupValue == group.Name)
                    //        {
                    //            exist = true;
                    //            break;
                    //        }
                    //    }

                    //    if (exist)
                    //    {
                    //        break;
                    //    }
                    //}
                }
            }
            else
            {
                exist = true;
            }
            return exist;
        }

        private static DashboardPanel DeSerializerPanel(string panelString, DashboardType type)
        {
            DashboardPanel panel = null;
            StringReader sReader = new StringReader(panelString);
            try
            {
                if (type == DashboardType.Chart)
                {
                    ChartSetting cSetting = new ChartSetting();
                    XmlSerializer xSerialize = new XmlSerializer(cSetting.GetType());
                    cSetting = (ChartSetting)xSerialize.Deserialize(sReader);
                    panel = cSetting;
                }
                else if (type == DashboardType.Panel)
                {
                    PanelSetting pSetting = new PanelSetting();
                    XmlSerializer xSerialize = new XmlSerializer(typeof(PanelSetting));
                    pSetting = (PanelSetting)xSerialize.Deserialize(sReader);
                    panel = pSetting;
                }
                else if (type == DashboardType.Query)
                {
                    DashboardQuery dSetting = new DashboardQuery();
                    XmlSerializer xSerialize = new XmlSerializer(typeof(DashboardQuery));
                    dSetting = (DashboardQuery)xSerialize.Deserialize(sReader);
                    panel = dSetting as DashboardPanel;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return panel;
        }

        private static string SerializerPanel(DashboardPanel panel)
        {
            string panelString = string.Empty;
            try
            {
                StringWriter sWriter = new StringWriter();
                XmlSerializer xSerialize = new XmlSerializer(panel.GetType());
                xSerialize.Serialize(sWriter, panel);
                panelString = sWriter.ToString();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return panelString;
        }

        public void RefreshCache()
        {
            List<Dashboard> lstDashboard = this.Load();
            if (lstDashboard == null || lstDashboard.Count == 0)
                return;

            foreach (Dashboard dashboard in lstDashboard)
            {
                LoadProperty(dashboard);
                CacheHelper<Dashboard>.AddOrUpdate(dashboard.Title, this.dbContext.TenantID, dashboard);
                CacheHelper<Dashboard>.AddOrUpdate(dashboard.ID.ToString(), this.dbContext.TenantID, dashboard);
            }
        }

    }

    public interface IDashboardManager : IManagerBase<Dashboard>
    {

    }
}
