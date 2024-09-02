using DevExpress.Web;
using DevExpress.Web.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace uGovernIT.Web
{
    public partial class GlobalSearchPage : System.Web.UI.UserControl
    {
        private DashboardManager _dashboardManager = null;
        public bool IsDashboard { get; set; }
        public string CategoryName { get; set; }
        public string MyHomeTab { get; set; }
        public bool IsHomePage { get; set; }
        protected bool isWildCardFiltered;
        protected string wildCardWaterMark = "Search String";
        private List<ModuleColumn> moduleColumns;
        protected ModuleStatisticResponse moduleStatResult;
        private ApplicationContext _context;
        protected DataTable resultedTable;
        public UserProfile User;
        public string UserType { get; set; }
        List<WikiArticles> articles = new List<WikiArticles>();
        List<Services> services = new List<Services>();
        public string searchText { get; set; }
        public string newServiceURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID="));
        private List<ModuleColumn> gridViewModuleColumns;
        ConfigurationVariableManager objConfigurationVariableHelper;
        ModuleColumnManager objModuleColumnManager;
        public ModuleColumn moduleColumn = null;
        public FieldConfiguration field = null;
        private FieldConfigurationManager _fmanger = null;
        public UGITModule currentUGITModule = null;
        public ConfigurationVariableManager _configurationVariableManager;
        private TicketManager _ticketManager = null;
        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(_context);
                }
                return _ticketManager;
            }
        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fmanger == null)
                {
                    _fmanger = new FieldConfigurationManager(_context);
                }
                return _fmanger;
            }
        }



        public GlobalSearchPage()
        {
            _context = HttpContext.Current.GetManagerContext();
            _ticketManager = new TicketManager(_context);
            UserType = "all";
            objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            objModuleColumnManager = new ModuleColumnManager(_context);
            gridViewModuleColumns = CacheHelper<object>.Get($"ModuleColumnslistview{_context.TenantID}") as List<ModuleColumn>;
            if (gridViewModuleColumns == null)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_context);
                gridViewModuleColumns = moduleColumnManager.Load();
                CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{_context.TenantID}", gridViewModuleColumns);
            }
            gridViewModuleColumns = gridViewModuleColumns.Where(x => x.CategoryName == Utility.Constants.MyHomeTicketTab).ToList();
        }

        protected DashboardManager DashboardManager
        {
            get
            {
                if (_dashboardManager == null)
                {
                    _dashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());
                }
                return _dashboardManager;
            }
        }
        private ModuleViewManager _moduleViewManager = null;
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _moduleViewManager;
            }
        }
        private WikiArticlesManager _wikiArticlesManager = null;

        protected WikiArticlesManager wikiArticlesManager
        {
            get
            {
                if (_wikiArticlesManager == null)
                {
                    _wikiArticlesManager = new WikiArticlesManager(HttpContext.Current.GetManagerContext());
                }
                return _wikiArticlesManager;
            }
        }

        private ServicesManager _servicesManager = null;
        //private long moduleName;

        protected ServicesManager servicesManager
        {
            get
            {
                if (_servicesManager == null)
                {
                    _servicesManager = new ServicesManager(HttpContext.Current.GetManagerContext());
                }
                return _servicesManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadModules();
            LoadModuleColumns();
            if (!IsPostBack)
            {
                txtWildCard.Text = Convert.ToString(Request.QueryString["globalSearchString"]);

            }
            searchText = txtWildCard.Text;

            User = HttpContext.Current.CurrentUser();

            globalSearchPage.DataSource = GetWikiArticles();
            globalSearchPage.DataBind();

            globalServiceCatalog.DataSource = GetServices();
            //globalServiceCatalog.DataSource = null;

            globalServiceCatalog.DataBind();

            globalServiceUserRequest.DataSource = GetFilteredData();
            globalServiceUserRequest.DataBind();

            userRequestLink.Attributes.Add("class", "active");
            /*
            if (services.Count == 0 && resultedTable == null && articles.Count > 0)
            {
                wikiList.Attributes.Add("class", "active");
            }
            else if (resultedTable != null)
            {
                if (services.Count == 0 && resultedTable.Rows.Count > 0 && articles.Count == 0)
                {
                    userRequestLink.Attributes.Add("class", "active");
                }
            }
            */

            #region if no search found for all area

            if (resultedTable != null)
            {
                if (resultedTable.Rows.Count == 0 && articles.Count == 0 && services.Count == 0)
                {
                    NodataFound.Visible = true;
                    //ClearFilters();
                }
                else
                {
                    NodataFound.Visible = false;
                }
            }
            else
            {
                NodataFound.Visible = false;
            }

            #endregion
        }

        private void ClearFilters()
        {
            ddlModuleName.SelectedIndex = 0;
            lstFilteredFields.SelectedIndex = 0;
            dtFrom.Text = string.Empty;
            dtTo.Text = string.Empty;
        }

        private void LoadModules()
        {
            if (Request.QueryString["isGlobalSearch"] == "true")
            {
                //globalfilterByModule.Visible = true;
                //pGlobalFiltersTable.Attributes.Add("width", "950px");
                if (ddlModuleName.Items.Count <= 0)
                {

                    DataTable dtModuleList = ModuleViewManager.LoadAllModules();  //uGITCache.GetModuleList(ModuleType.All);
                    if (dtModuleList.Columns.Contains(DatabaseObjects.Columns.UseInGlobalSearch))
                    {
                        DataRow[] modulesRows = dtModuleList.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.UseInGlobalSearch, true));
                        foreach (DataRow row in modulesRows)
                        {
                            ddlModuleName.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                        }

                        ddlModuleName.Items.Insert(0, new ListItem("--All--", "0"));
                        // set the "All" in Field Drop Down.
                        lstFilteredFields.Items.Insert(0, new ListItem("--All--", "0"));
                    }
                }

                if (!IsPostBack)
                {
                    txtWildCard.Text = Convert.ToString(Request.QueryString["globalSearchString"]);
                }
            }
        }

        public string ModuleName
        {
            get
            {
                if (ViewState["modulename"] == null)
                    return string.Empty;
                else
                    return Convert.ToString(ViewState["modulename"]);
            }
            set { ViewState["modulename"] = value; }
        }

        private void CreateAspxGridView()
        {
            if (globalServiceUserRequest.Columns.Count > 0)
                return;

            if (gridViewModuleColumns.Count() > 0) //From MyModuleColumns
            {
                //GridViewCommandColumn ccol = new GridViewCommandColumn();
                //ccol.Caption = string.Empty;
                //ccol.ShowSelectCheckbox = true;
                //globalServiceUserRequest.Columns.Add(ccol);

                List<ModuleColumn> filterCols = gridViewModuleColumns.Where(x => x.FieldName != DatabaseObjects.Columns.Id && x.IsDisplay).OrderBy(x => x.FieldSequence).ToList();

                foreach (ModuleColumn modulecolumn in filterCols)
                {
                    GridViewDataTextColumn col = new GridViewDataTextColumn();

                    if (modulecolumn.FieldName == DatabaseObjects.Columns.Name || modulecolumn.FieldName == DatabaseObjects.Columns.Manager)
                    {
                        col.PropertiesTextEdit.EncodeHtml = false;
                        col.Settings.FilterMode = ColumnFilterMode.Value;
                        col.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    }
                    else if (modulecolumn.IsDisplay && modulecolumn.FieldName != DatabaseObjects.Columns.Name || modulecolumn.FieldName != DatabaseObjects.Columns.Manager)
                        col.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    col.Caption = modulecolumn.FieldDisplayName;
                    col.FieldName = modulecolumn.FieldName;
                    if (col.FieldName == "GlobalRoleID")
                        col.FieldName = DatabaseObjects.Columns.GlobalRoleID;
                    if (col.FieldName == DatabaseObjects.Columns.ID)
                        col.FieldName = DatabaseObjects.Columns.Id;
                    col.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    if (resultedTable != null && resultedTable.Columns.Contains(modulecolumn.FieldName))
                        globalServiceUserRequest.Columns.Add(col);
                }
            }
            else //Default Case
            {
                GridViewCommandColumn col = new GridViewCommandColumn();
                col.Caption = string.Empty;
                col.ShowSelectCheckbox = true;
                globalServiceUserRequest.Columns.Add(col);

                GridViewDataColumn dcol = new GridViewDataColumn();
                dcol.FieldName = DatabaseObjects.Columns.TicketId;
                dcol.Caption = DatabaseObjects.Columns.Title;
                dcol.Caption = DatabaseObjects.Columns.Description;
                dcol.Caption = DatabaseObjects.Columns.Created;
                dcol.Visible = false;

                globalServiceUserRequest.Columns.Add(dcol);

            }
        }
        private DataTable GetFilteredData()
        {

            GetBaseFilteredData();
            GetGlobalFilteredData();

            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                //GenerateColumns();
            }

            if (resultedTable != null && resultedTable.Columns.Contains(DatabaseObjects.Columns.IsPrivate))
            {
                DataTable dt = null;
                EnumerableRowCollection<DataRow> dc = resultedTable.AsEnumerable().Where(x => UGITUtility.StringToBoolean(x.Field<object>(DatabaseObjects.Columns.IsPrivate)));
                if (dc != null && dc.Count() > 0)
                    dt = dc.CopyToDataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                        string ticketModuleName = ModuleName;
                        if (string.IsNullOrEmpty(ticketModuleName))
                            ticketModuleName = uHelper.getModuleNameByTicketId(ticketId);
                    }
                }
            }
            // filtering result set by TenantID            
            //resultedTable = resultedTable.AsEnumerable().Where(x => x.Field<string>("TenantID") != null && x.Field<string>("TenantID").Equals(TenantID, StringComparison.InvariantCultureIgnoreCase)).CopyToDataTable();
            if (resultedTable == null || resultedTable.Rows.Count == 0)
            {
                //ServiceCatalogsearch.Style.Add("display", "none");
                // UserRequestsSearch.Style.Add("display", "none");
                ////userRequestLink.Style.Add("display", "none");
            }
            else
            {
                ServiceCatalogsearch.Style.Add("display", "block");
                UserRequestsSearch.Style.Add("display", "block");
                userRequestLink.Style.Add("display", "block");
            }

            CreateAspxGridView();

            return resultedTable;
        }

        private void GetBaseFilteredData()
        {
            //if (FilteredTable != null)
            //{
            //    IsFilteredTableExist = true;
            //}

            //if (IsFilteredTableExist)
            //{
            //    resultedTable = FilteredTable;
            //}
            //else if (!IsDashboard)
            //{
            LoadFilteredTickets();
            //}
            //else if (IsDashboard)
            //{
            // GetModuleNameByDashboardId();
            //}

            // CustomGridModifications();

        }


        private void LoadFilteredTickets()
        {
            ModuleStatistics moduleStatistics = new ModuleStatistics(_context);
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            //mRequest.SPWebObj = "";// SPContext.Current.Web;
            mRequest.UserID = User.Id;
            mRequest.Tabs = new List<string>();

            //if (cModuleFilteredLinksPanel.Visible)
            //{
            //    foreach (Tab t in filterTab.Tabs)
            //    {

            //        mRequest.Tabs.Add(t.Name);
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(UserType))
                UserType = UserType.Trim().ToLower();


            //if (cModuleFilteredLinksPanel.Visible == true && ModuleName != string.Empty && !IsPostBack)
            //{
            //if (UGITUtility.GetCookie(Request, ModuleName + "_tabChoice") != null)
            //{
            //    UserType = UGITUtility.GetCookieValue(Request, ModuleName + "_tabChoice");
            //}

            //try
            //{
            //    HttpCookie cookie = UGITUtility.GetCookie(Request, ModuleName + "_MTicketStatus");
            //    if (cookie != null)
            //        MTicketStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), cookie.Value);
            //    else
            //        MTicketStatus = TicketStatus.Open;
            //}
            //catch
            //{
            //    MTicketStatus = TicketStatus.Open;
            //}

            //if (tabType.Value == string.Empty)
            //{
            //    if (UserType == "my" && MTicketStatus == TicketStatus.Open)
            //    {
            //        tabType.Value = "myopentickets";
            //    }
            //    else if (UserType == "mygroup" && MTicketStatus == TicketStatus.Open)
            //    {
            //        tabType.Value = "mygrouptickets";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.Open)
            //    {
            //        tabType.Value = "allopentickets";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.Closed)
            //    {
            //        tabType.Value = "allclosedtickets";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.WaitingOnMe)
            //    {
            //        tabType.Value = "waitingonme";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.Unassigned)
            //    {
            //        tabType.Value = "unassigned";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.All)
            //    {
            //        tabType.Value = "alltickets";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.Department)
            //    {
            //        tabType.Value = "departmentticket";
            //    }
            //    else if (UserType == "all" && MTicketStatus == TicketStatus.Approved)
            //    {
            //        tabType.Value = "allresolvedtickets";
            //    }
            //    else if (UserType == "my" && MTicketStatus == TicketStatus.Closed)
            //    {
            //        tabType.Value = "myclosedtickets";
            //    }
            //}
            // }



            //if (tabType.Value.Trim().Equals("myopentickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    //if (UserType.Equals("my", StringComparison.CurrentCultureIgnoreCase))
            //    //{
            //    UserType = "my";
            //    //}
            //    MTicketStatus = TicketStatus.Open;
            //    mRequest.CurrentTab = FilterTab.myopentickets;//filterTab.Tabs.FindByName(FilterTab.myopentickets) != null ? filterTab.Tabs.FindByName(FilterTab.myopentickets) : new Tab(FilterTab.myopentickets, FilterTab.myopentickets);

            //}
            //if (tabType.Value.Trim().Equals("mygrouptickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "mygroup";
            //    MTicketStatus = TicketStatus.Open;
            //    mRequest.CurrentTab = FilterTab.mygrouptickets; //filterTab.Tabs.FindByName(FilterTab.mygrouptickets) != null ? filterTab.Tabs.FindByName(FilterTab.mygrouptickets) : new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets); // CustomFilterTab.MyGroupTickets;
            //}
            //else if (tabType.Value.Trim().Equals("allopentickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.Open;
            //    mRequest.CurrentTab = FilterTab.allopentickets; //filterTab.Tabs.FindByName(FilterTab.allopentickets) != null ? filterTab.Tabs.FindByName(FilterTab.allopentickets) : new Tab(FilterTab.allopentickets, FilterTab.allopentickets); //CustomFilterTab.OpenTickets;
            //}
            //else if (tabType.Value.Trim().Equals("allclosedtickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.Closed;
            //    mRequest.CurrentTab = FilterTab.allclosedtickets;//filterTab.Tabs.FindByName(FilterTab.allclosedtickets) != null ? filterTab.Tabs.FindByName(FilterTab.allclosedtickets) : new Tab(FilterTab.allclosedtickets, FilterTab.allclosedtickets); //CustomFilterTab.CloseTickets;
            //}
            //else if (tabType.Value.Trim().Equals("waitingonme", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.WaitingOnMe;
            //    mRequest.CurrentTab = FilterTab.waitingonme; //filterTab.Tabs.FindByName(FilterTab.waitingonme) != null ? filterTab.Tabs.FindByName(FilterTab.waitingonme) : new Tab(FilterTab.waitingonme, FilterTab.waitingonme); // new Tab(FilterTab.waitingonme, FilterTab.waitingonme); // CustomFilterTab.WaitingOnMe;

            //}
            //else if (tabType.Value.Trim().Equals("unassigned", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.Unassigned;
            //    mRequest.CurrentTab = FilterTab.unassigned; //filterTab.Tabs.FindByName(FilterTab.unassigned) != null ? filterTab.Tabs.FindByName(FilterTab.unassigned) : new Tab(FilterTab.unassigned, FilterTab.unassigned); //CustomFilterTab.UnAssigned;

            //}
            //else if (tabType.Value.Trim().Equals("alltickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.All;
            //    mRequest.CurrentTab = FilterTab.alltickets; //filterTab.Tabs.FindByName(FilterTab.alltickets) != null ? filterTab.Tabs.FindByName(FilterTab.alltickets) : new Tab(FilterTab.alltickets, FilterTab.alltickets); // CustomFilterTab.AllTickets;
            //}
            //else if (tabType.Value.Trim().Equals("departmentticket", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "my";
            //    MTicketStatus = TicketStatus.Department;
            //    mRequest.CurrentTab = FilterTab.departmentticket;//filterTab.Tabs.FindByName(FilterTab.departmentticket) != null ? filterTab.Tabs.FindByName(FilterTab.departmentticket) : new Tab(FilterTab.departmentticket, FilterTab.departmentticket);  //CustomFilterTab.MyDepartmentTickets;
            //}
            //else if (tabType.Value.Trim().Equals("allresolvedtickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "all";
            //    MTicketStatus = TicketStatus.Approved;
            //    mRequest.CurrentTab = FilterTab.allresolvedtickets;//filterTab.Tabs.FindByName(FilterTab.allresolvedtickets) != null ? filterTab.Tabs.FindByName(FilterTab.allresolvedtickets) : new Tab(FilterTab.allresolvedtickets, FilterTab.allresolvedtickets); // CustomFilterTab.ResolvedTickets;
            //}
            //else if (tabType.Value.Trim().Equals("myclosedtickets", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    UserType = "my";
            //    MTicketStatus = TicketStatus.Closed;

            //    mRequest.CurrentTab = FilterTab.myclosedtickets; //filterTab.Tabs.FindByName("myclosedtickets") != null ? filterTab.Tabs.FindByName("myclosedtickets") : new Tab("myclosedtickets", "myclosedtickets"); // CustomFilterTab.MyCloseTickets;
            //}

            //if (IsPostBack && ModuleName != string.Empty && cModuleFilteredLinksPanel.Visible == true)
            //{
            //    UGITUtility.CreateCookie(Response, ModuleName + "_tabChoice", UserType);
            //    UGITUtility.CreateCookie(Response, ModuleName + "_MTicketStatus", MTicketStatus.ToString());
            //}


            //if (UserType == "my" && MTicketStatus == TicketStatus.Open)
            //{
            //    mRequest.CurrentTab = FilterTab.myopentickets; //filterTab.Tabs.FindByName(FilterTab.myopentickets) != null ? filterTab.Tabs.FindByName(FilterTab.myopentickets) : new Tab(FilterTab.myopentickets, FilterTab.myopentickets); // CustomFilterTab.MyOpenTickets;
            //}
            //else if (UserType == "mygroup" && MTicketStatus == TicketStatus.Open)
            //{
            //    mRequest.CurrentTab = FilterTab.mygrouptickets; //filterTab.Tabs.FindByName(FilterTab.mygrouptickets) != null ? filterTab.Tabs.FindByName(FilterTab.mygrouptickets) : new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets); // CustomFilterTab.MyGroupTickets;
            //}
            //else if (UserType == "all" && MTicketStatus == TicketStatus.Open)
            //{
            //    mRequest.CurrentTab = FilterTab.allopentickets;//filterTab.Tabs.FindByName(FilterTab.allopentickets) != null ? filterTab.Tabs.FindByName(FilterTab.allopentickets) : new Tab(FilterTab.allopentickets, FilterTab.allopentickets); // CustomFilterTab.OpenTickets;
            //}
            //else if (UserType == "all" && MTicketStatus == TicketStatus.Closed)
            //{
            //    mRequest.CurrentTab = FilterTab.allclosedtickets; //filterTab.Tabs.FindByName(FilterTab.allclosedtickets) != null ? filterTab.Tabs.FindByName(FilterTab.allclosedtickets) : new Tab(FilterTab.allclosedtickets, FilterTab.allclosedtickets); // CustomFilterTab.CloseTickets;
            //}
            //else if (UserType == "all" && MTicketStatus == TicketStatus.WaitingOnMe)
            //{
            //    mRequest.CurrentTab = FilterTab.waitingonme; //filterTab.Tabs.FindByName(FilterTab.waitingonme) != null ? filterTab.Tabs.FindByName(FilterTab.waitingonme) : new Tab(FilterTab.waitingonme, FilterTab.waitingonme); //new Tab(FilterTab.waitingonme, FilterTab.waitingonme); // CustomFilterTab.WaitingOnMe;
            //}
            //else if (UserType == "all" && MTicketStatus == TicketStatus.Unassigned)
            //{
            //    mRequest.CurrentTab = FilterTab.unassigned;//filterTab.Tabs.FindByName(FilterTab.unassigned) != null ? filterTab.Tabs.FindByName(FilterTab.unassigned) : new Tab(FilterTab.unassigned, FilterTab.unassigned); // CustomFilterTab.UnAssigned;
            //}
            //if (UserType == "all" && MTicketStatus == TicketStatus.All)
            if (UserType == "all")
            {
                mRequest.CurrentTab = FilterTab.alltickets;//filterTab.Tabs.FindByName(FilterTab.alltickets) != null ? filterTab.Tabs.FindByName(FilterTab.alltickets) : new Tab(FilterTab.alltickets, FilterTab.alltickets); // CustomFilterTab.AllTickets;
            }
            //else if (MTicketStatus == TicketStatus.Department)
            //{
            //    mRequest.CurrentTab = FilterTab.departmentticket; //filterTab.Tabs.FindByName(FilterTab.departmentticket) != null ? filterTab.Tabs.FindByName(FilterTab.departmentticket) : new Tab(FilterTab.departmentticket, FilterTab.departmentticket); // CustomFilterTab.MyDepartmentTickets;
            //}
            //else if (UserType == "all" && MTicketStatus == TicketStatus.Approved)
            //{
            //    mRequest.CurrentTab = FilterTab.allresolvedtickets;//filterTab.Tabs.FindByName(FilterTab.allresolvedtickets) != null ? filterTab.Tabs.FindByName(FilterTab.allresolvedtickets) : new Tab(FilterTab.allresolvedtickets, FilterTab.allresolvedtickets); // CustomFilterTab.ResolvedTickets;
            //}
            //else if (UserType == "my" && MTicketStatus == TicketStatus.Closed)
            //{
            //    mRequest.CurrentTab = FilterTab.myclosedtickets; //filterTab.Tabs.FindByName("myclosedtickets") != null ? filterTab.Tabs.FindByName("myclosedtickets") : new Tab("myclosedtickets", "myclosedtickets"); // CustomFilterTab.MyCloseTickets;
            //}

            if (mRequest.CurrentTab != null && (!mRequest.Tabs.Contains(mRequest.CurrentTab)) && mRequest.Tabs.Count > 0)
            {
                mRequest.CurrentTab = mRequest.Tabs.First();//new Tab(mRequest.Tabs.First(), mRequest.Tabs.First()); //mRequest.Tabs.First();
            }





            if (ModuleName == string.Empty)
            {
                // Home page case 
                //cModuleGlobalFilterPanel.Visible = !HideGlobalSearch;
                //cModuleFilteredLinksPanel.Visible = !HideGlobalSearch;

                List<ModuleStatisticResponse> stats = null;

                //if (MTicketStatus == TicketStatus.WaitingOnMe)
                //{
                //    mRequest.CurrentTab = FilterTab.waitingonme;//new Tab(FilterTab.waitingonme, FilterTab.waitingonme); // filterTab.Tabs.FindByName("waitingonme");
                //    mRequest.ModuleType = ModuleType.SMS;
                //    stats = moduleStatistics.LoadAll(mRequest);
                //}
                //else if (MTicketStatus == TicketStatus.Department)
                //{
                //    mRequest.CurrentTab = FilterTab.departmentticket; //new Tab(FilterTab.departmentticket, FilterTab.departmentticket);
                //    mRequest.ModuleType = ModuleType.SMS;
                //    stats = moduleStatistics.LoadAll(mRequest);
                //}
                if (Request["isGlobalSearch"] == "true")
                {
                    mRequest.CurrentTab = FilterTab.alltickets; //new Tab(FilterTab.alltickets, FilterTab.alltickets); // filterTab.Tabs.FindByName("alltickets");
                    mRequest.ModuleType = ModuleType.All;
                    mRequest.IsGlobalSearch = true;
                    stats = moduleStatistics.LoadAll(mRequest, chkIncludeClosed.Checked);
                }
                else
                {
                    if (UserType != "" && UserType != "all" && UserType != "my" && UserType != "mygroup")
                    {
                        resultedTable = moduleStatistics.GetMyOpenTicketDataByRole(User, UserType);
                    }
                    else
                    {
                        mRequest.ModuleType = ModuleType.SMS;
                        stats = moduleStatistics.LoadAll(mRequest);
                    }
                }
                if (stats == null)
                    return;

                moduleStatResult = new ModuleStatisticResponse();

                DataTable mytable = new DataTable();
                foreach (ModuleStatisticResponse mStat in stats)
                {
                    if (mStat.ResultedData != null && uHelper.IsUserAuthorizedToViewModule(mStat.ModuleName))
                    {
                        if (!mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                        {
                            mStat.ResultedData.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                            mStat.ResultedData.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = ""; //string.Format("'{0}'", mStat.ModuleName);
                        }
                        if (mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                        {
                            if (mytable.Rows.Count <= 0)
                            {
                                mytable = mStat.ResultedData;
                            }
                            else
                            {
                                mytable.Merge(mStat.ResultedData);
                            }
                        }

                        foreach (string tab in mRequest.Tabs)
                        {
                            if (mStat.TabCounts.ContainsKey(tab))
                                moduleStatResult.TabCounts[tab] = mStat.TabCounts[tab];
                            else
                                moduleStatResult.TabCounts[tab] = 0;
                        }
                    }
                }
                //enableTicketListExport = false;
                resultedTable = mytable;
            }

        }

        private void GetGlobalFilteredData()
        {
            // pGlobalFilters.CssClass = "globalfilter";
            //pGlobalFiltersTable.Attributes.Add("class", "bordercolps");
            //allTicketLink.CssClass = "tickettypetab";

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            if (dtFrom.Text != string.Empty)
            {
                startDate = dtFrom.Date;
            }

            if (dtTo.Text != string.Empty)
            {
                endDate = dtTo.Date;
            }

            //if from date empty and to date is not empty then set to date in from
            if (dtFrom.Text == string.Empty && dtTo.Text != string.Empty)
            {
                startDate = dtTo.Date;
            }

            //if to date empty and from date is not empty then set from date in to
            if (dtFrom.Text != string.Empty && dtTo.Text == string.Empty)
            {
                endDate = dtFrom.Date;
            }

            string globalFilter = string.Empty;
            string fieldName = string.Empty;

            if (txtWildCard.Text.Trim() != string.Empty && !txtWildCard.Text.Equals(wildCardWaterMark, StringComparison.CurrentCultureIgnoreCase))
            {
                globalFilter = txtWildCard.Text.Trim();
            }


            bool isModuleSelected = false;
            string moduleName = ModuleName;

            if (ddlModuleName.SelectedItem != null && ddlModuleName.SelectedItem.Text != "--All--")
            {
                moduleName = ddlModuleName.SelectedItem.Text;
                isModuleSelected = true;
            }

            if (!string.IsNullOrEmpty(globalFilter))
            {
                if (!globalFilter.Contains("*"))
                {
                    globalFilter = string.Format("*{0}*", globalFilter);
                }
                globalFilter = globalFilter.Replace("*", "%");
            }

            //if the field name is selected for search
            if (lstFilteredFields.SelectedItem != null && lstFilteredFields.SelectedItem.Text != "--All--")
            {
                // store the internal name of the field.
                fieldName = (lstFilteredFields.SelectedValue);
            }

            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue || !string.IsNullOrEmpty(globalFilter) || isModuleSelected)
            {
                isWildCardFiltered = true;
                //pGlobalFilters.CssClass = "globalfilter";
                //pGlobalFiltersTable.Attributes.Add("class", "bordercolps");
                //allTicketLink.CssClass = "tickettypetab";
                if (resultedTable != null)
                    resultedTable = WildCardFilter(resultedTable, globalFilter, moduleName, startDate, endDate, fieldName);
            }
            else if (Request.QueryString["isGlobalSearch"] == "true")
            {
                // If called from global search, only show results when we have valid search parameters
                resultedTable = null;
            }
        }


        private DataTable WildCardFilter(DataTable table, string wildCard, string moduleName, DateTime startDate, DateTime endDate, string fieldName)
        {
            if (table == null)
                return null;

            try
            {
                FieldConfigurationManager configFieldManager = new FieldConfigurationManager(_context);
                FieldConfiguration configField = null;
                DataRow[] localFilteredRows = table.Select();
                if (!string.IsNullOrEmpty(moduleName))
                {
                    if (table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                        localFilteredRows = table.Select(DatabaseObjects.Columns.TicketId + " Like '" + moduleName + "%'");
                }

                if (!string.IsNullOrEmpty(wildCard))
                {
                    // DataTable columns =  GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns);  //uGITCache.ModuleConfigCache.LoadModuleListByName(ModuleName, DatabaseObjects.Tables.ModuleColumns);// uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                    List<string> selectedColumns = new List<string>();

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        //selectedColumns = moduleColumns.Where(x => x.CategoryName == moduleName && x.IsUseInWildCard).Select(x => x.FieldName).Distinct().ToList();
                        // added below line for if search from Global filter popup, when module is selected.
                        selectedColumns = new ModuleColumnManager(_context).Load(x => x.CategoryName == moduleName && x.IsUseInWildCard).Select(x => x.FieldName).Distinct().ToList();
                    }
                    else
                    {
                        selectedColumns = moduleColumns.Select(x => x.FieldName).Distinct().ToList();
                    }

                    //To search in a field.
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        selectedColumns = selectedColumns.Where(x => x == fieldName).ToList();
                    }

                    StringBuilder queryExpression = new StringBuilder();
                    if (selectedColumns.Count > 0)
                    {
                        for (int i = 0; i < selectedColumns.Count; i++)
                        {
                            string column = selectedColumns[i];
                            if (table.Columns.Contains(column))
                            {
                                configField = configFieldManager.GetFieldByFieldName(column);

                                if (configField == null)
                                {
                                    if (table.Columns[column].DataType == typeof(string))
                                    {
                                        if (queryExpression.Length > 0)
                                            queryExpression.Append(" or ");

                                        queryExpression.AppendFormat("{0} like '{1}'", column, wildCard);
                                    }
                                }
                                else
                                {
                                    string value = configFieldManager.GetFieldConfigurationIdByName(configField.FieldName, Convert.ToString(wildCard.Replace("%", "")));

                                    if (queryExpression.Length > 0 && !string.IsNullOrEmpty(value) && value != "''")
                                    {
                                        if (configField.Datatype.Equals("Date", StringComparison.InvariantCultureIgnoreCase) || configField.Datatype.Equals("DateTime", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                        }
                                        else if (configField.Datatype.Equals("Choices", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            queryExpression.Append(" or ");
                                            queryExpression.AppendFormat("{0} like {1}", column, value);
                                        }
                                        else
                                        {
                                            string[] Values = value.Replace("'", "").Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);

                                            foreach (var item in Values)
                                            {
                                                queryExpression.Append(" or ");
                                                if (table.Columns[configField.FieldName].DataType.Name.Equals("String", StringComparison.InvariantCultureIgnoreCase))
                                                    queryExpression.AppendFormat("{0} like '%{1}%'", column, item);
                                                else
                                                    queryExpression.AppendFormat("{0} in ({1})", column, item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Array.Clear(localFilteredRows, 0, localFilteredRows.Count());
                    }
                    //Fiter data by global string
                    if (localFilteredRows != null && localFilteredRows.Length > 0)
                    {
                        localFilteredRows = localFilteredRows.CopyToDataTable().Select(Convert.ToString(queryExpression));
                    }
                }

                //Fiter data between start and end date
                if (startDate.Date != DateTime.MinValue || endDate.Date != DateTime.MinValue)
                {
                    string dateColumnName = string.Empty;
                    ModuleColumn[] multiuserColumns = new ModuleColumn[0];
                    if (moduleColumns.Count > 0)
                    {
                        multiuserColumns = moduleColumns.Where(x => !string.IsNullOrWhiteSpace(x.CustomProperties) && (x.CustomProperties.Contains("fieldtype=datetime") || x.CustomProperties.Contains("useforglobaldatefilter=true") || x.CustomProperties.Contains("useforglobaldatefilter=1"))).ToArray();
                    }

                    if (multiuserColumns.Length > 0 && multiuserColumns[0].FieldName != null && table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    {
                        dateColumnName = Convert.ToString(multiuserColumns[0].FieldName).Trim();
                    }
                    else if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    {
                        dateColumnName = DatabaseObjects.Columns.TicketCreationDate;
                    }
                    else if (table.Columns.Contains(DatabaseObjects.Columns.Created))
                    {
                        dateColumnName = DatabaseObjects.Columns.Created;
                    }

                    if (dateColumnName != string.Empty && localFilteredRows.Length > 0)
                    {
                        localFilteredRows = localFilteredRows.Where(x => x.IsNull(dateColumnName) != true && x.Field<DateTime>(dateColumnName) != null && x.Field<DateTime>(dateColumnName).ToString() != string.Empty && x.Field<DateTime>(dateColumnName).Date >= startDate.Date && x.Field<DateTime>(dateColumnName).Date <= endDate.Date).ToArray();
                    }
                }

                if (localFilteredRows != null && localFilteredRows.Length > 0)
                {
                    return localFilteredRows.CopyToDataTable();
                }
                else
                {
                    return table.Clone();
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
                return null;
            }
        }

        void LoadModuleColumns()
        {
            // GetModuleNameByDashboardId();
            CategoryName = "MyHomeTab";
            if (IsHomePage && !string.IsNullOrEmpty(MyHomeTab))
                CategoryName = CategoryName;
            else if (string.IsNullOrEmpty(ModuleName))
                CategoryName = "MyHomeTab";
            else { CategoryName = ModuleName; }
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_context);
            moduleColumns = moduleColumnManager.Load(x => x.CategoryName == CategoryName);
            if (IsDashboard)
            {
                if (!string.IsNullOrEmpty(Request["dID"]))
                {
                    int dID = 0;
                    int.TryParse(Request["dID"], out dID);
                    Guid linkID = Guid.Empty;
                    if (Request["kID"] != null)
                    {
                        try
                        {
                            linkID = new Guid(Request["kID"]);
                        }
                        catch { }
                    }
                    Dashboard dashboard = DashboardManager.LoadPanelById(dID);
                    PanelSetting panel = dashboard.panel as PanelSetting;
                    DashboardPanelLink panelLink = panel.Expressions.FirstOrDefault(x => x.LinkID == linkID);
                    if (panelLink != null && !string.IsNullOrEmpty(panelLink.ShowColumns_Category))
                        moduleColumns = moduleColumnManager.Load(x => x.CategoryName == panelLink.ShowColumns_Category);
                    else
                        moduleColumns = moduleColumnManager.Load(x => x.CategoryName == Constants.MyDashboardTicket);
                }
                else
                {
                    moduleColumns = moduleColumnManager.Load(x => x.CategoryName == Constants.MyDashboardTicket);
                }

            }
            else
            {
                if (moduleColumns == null)
                    moduleColumns = moduleColumnManager.Load(x => x.CategoryName == Constants.MyHomeTicketTab);
            }

            if (moduleColumns != null)
            {
                moduleColumns = moduleColumns.OrderBy(x => x.FieldSequence).ToList();
            }
        }



        #region wiki 

        private List<WikiArticles> GetWikiArticles()
        {
            // WikiArticles = wikiArticlesManager.Load(x => x.IsDeleted == false, null, skip, pageSize, null);
            string query = String.Format("Title like '%" + txtWildCard.Text + "%'");

            //string query = string.Format("") 
            articles = wikiArticlesManager.Load(query).ToList();
            if (articles.Count == 0)
            {
                wikisearch.Style.Add("display", "none");
                wikiList.Style.Add("display", "none");
                globalSearchPage.Visible = false;
            }
            else
            {
                wikisearch.Style.Add("display", "block");
                wikiList.Style.Add("display", "block");
                globalSearchPage.Visible = true;
            }

            return articles;
        }

        protected void globalSearchPage_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            string func = string.Empty;
            string ticketTitle = string.Empty;
            string ticketID = string.Empty;
            string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            sourceURL = "/default.aspx";

            var row = globalSearchPage.GetRow(e.VisibleIndex);

            if (row != null)
            {
                WikiArticles article = (WikiArticles)row;
                ticketID = article.TicketId;
                ticketTitle = article.Title;
                ticketTitle = string.Format("{0}:{1}", ticketID, ticketTitle);

            }
            var viewUrl = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails";

            // Window.parent.UgitOpenPopupDialog(viewUrl, "", "View Wiki Article", "98", "100", false, "");

            func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), ticketTitle, sourceURL, 90, 90);
            e.Row.Attributes.Add("onClick", func);
            e.Row.Style.Add("cursor", "pointer");
        }

        #endregion

        #region services
        public List<Services> GetServices()
        {
            string query = String.Format("Title like '%" + txtWildCard.Text + "%' AND IsActivated = 1");

            services = servicesManager.Load(query);

            if (services != null)
            {
                if (services.Count == 0)
                {
                    ServiceCatalogsearch.Style.Add("display", "none");
                    serviceCatlogLink.Style.Add("display", "none");
                }
                else
                {
                    ServiceCatalogsearch.Style.Add("display", "block");
                    serviceCatlogLink.Style.Add("display", "block");
                } 
            }

            return services;
        }



        protected void globalServiceCatalog_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            string viewUrl = string.Empty;
            string title = string.Empty;
            string func = string.Empty;
            string ticketTitle = string.Empty;
            string ticketID = string.Empty;
            string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            sourceURL = "/default.aspx";
            var row = globalServiceCatalog.GetRow(e.VisibleIndex);
            if (row != null)
            {
                Services service = (Services)row;
                ticketID = Convert.ToString(service.ID);
                ticketTitle = service.Title;
            }
            string module = uHelper.getModuleNameByTicketId(ticketID);
            DataRow moduleDetail = null;
            if (!string.IsNullOrEmpty(module))
            {
                DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(module));
                if (moduledt.Rows.Count > 0)
                    moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
            }

            if (moduleDetail != null)
            {
                viewUrl = string.Empty;
                if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                {
                    viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                }


                if (!string.IsNullOrEmpty(ticketTitle))
                {
                    ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                }

                if (!string.IsNullOrEmpty(ticketID))
                {
                    title = string.Format("{0}: ", ticketID);
                }
                title = string.Format("{0}{1}", title, ticketTitle);

            }

            func = string.Format("servicecatalog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", newServiceURL, ticketID, ticketTitle, sourceURL, 90, 90);
            //e.Row.Attributes.Add("onClick", func);
            //e.Row.Style.Add("cursor", "pointer");
        }

        #endregion

        protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item;
            DataTable moduleColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            lstFilteredFields.Items.Clear();
            if (Convert.ToInt32(ddlModuleName.SelectedValue) != 0)
            {
                string selectQuery = string.Format("{0} = '{1}' and {2}= {3}", DatabaseObjects.Columns.CategoryName, ddlModuleName.SelectedItem.Text, DatabaseObjects.Columns.IsUseInWildCard, "True");
                DataRow[] moduleColumnRows = moduleColumns.Select(selectQuery);

                if (moduleColumnRows.Length > 0)
                {
                    foreach (DataRow dataRow in moduleColumnRows.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldDisplayName)).ToArray())
                    {
                        item = new ListItem(Convert.ToString(dataRow[DatabaseObjects.Columns.FieldDisplayName]), Convert.ToString(dataRow[DatabaseObjects.Columns.FieldName]));
                        lstFilteredFields.Items.Add(item);
                    }
                }
            }
            item = new ListItem("--All--");
            lstFilteredFields.Items.Insert(0, item);
        }

        protected void globalServiceUserRequest_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Created" || e.DataColumn.FieldName == "DesiredCompletionDate")
            {
                DateTime createdDate;
                if (DateTime.TryParse(Convert.ToString(e.CellValue), out createdDate))
                    e.Cell.Text = createdDate.ToString("MMM-d-yyyy");
                else
                    e.Cell.Text = "";

            }

            if (e.DataColumn.FieldName == "TicketId" || e.DataColumn.FieldName == "Title" || e.DataColumn.FieldName == "Description" || e.DataColumn.FieldName == "Created")
            {
                string viewUrl = string.Empty;
                string title = string.Empty;
                string func = string.Empty;
                string ticketTitle = string.Empty;
                string ticketID = string.Empty;
                string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                sourceURL = "/default.aspx";
                DataRow row = globalServiceUserRequest.GetDataRow(e.VisibleIndex);
                if (row != null)
                {
                    ticketID = Convert.ToString(row[DatabaseObjects.Columns.TicketId]);
                    ticketTitle = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                }
                string module = uHelper.getModuleNameByTicketId(ticketID);
                DataRow moduleDetail = null;
                if (!string.IsNullOrEmpty(module))
                {
                    DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(module));
                    if (moduledt.Rows.Count > 0)
                        moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                }

                if (moduleDetail != null)
                {
                    viewUrl = string.Empty;
                    if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                    {
                        viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                    }


                    if (!string.IsNullOrEmpty(ticketTitle))
                    {
                        ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                    }

                    if (!string.IsNullOrEmpty(ticketID))
                    {
                        title = string.Format("{0}: ", ticketID);
                    }
                    title = string.Format("{0}{1}", title, ticketTitle);

                    func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), title, sourceURL, 90, 90);
                    e.Cell.Attributes.Add("onClick", func);
                    e.Cell.Style.Add("cursor", "pointer");
                }
            }
        }


        protected void globalServiceUserRequest_HtmlRowPrepared1(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string viewUrl = string.Empty;
            string title = string.Empty;
            string func = string.Empty;
            string ticketId = string.Empty;
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            //In case of ResourceTab/AspNetUsers Dashboards Gridview should not be clickable like in Demo Sharepoint site.
            if (ModuleName == Utility.Constants.ResourceTab && !string.IsNullOrEmpty(Request["dID"]))
                return;
            //because the code will not find TicketId column for every chart when CategoryName is MyHomeTab
            if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab && !string.IsNullOrEmpty(Request["dashboardID"]))
                return;

            string moduleName = ModuleName;

            DataRow currentRow = globalServiceUserRequest.GetDataRow(e.VisibleIndex);
            if (currentRow == null)
                return; // No rows in table, nothing to do!


            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                moduleName = Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]);
            }

            if (string.IsNullOrEmpty(moduleName))
            {
                ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                if (ticketId.Length > 0)
                    moduleName = ticketId.Substring(0, ticketId.IndexOf("-"));

            }
            //start

            if (currentRow != null)
            {

                //Get Row ID
                //string ticketId = string.Empty;
                ticketId = string.Empty;

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketId) && Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]) != string.Empty)
                {
                    ticketId = currentRow[DatabaseObjects.Columns.TicketId].ToString().Trim();
                }
                else if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id))
                {
                    ticketId = currentRow[DatabaseObjects.Columns.Id].ToString();
                }


                title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
                title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening


                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && currentRow[DatabaseObjects.Columns.Id] != null)
                    e.Row.Attributes.Add("ticketId", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                e.Row.Style.Add("cursor", "pointer");

            }

            Ticket objTicket = new Ticket(_context, moduleName);

            //bool enableTicketReopenByRequestor = true; //objConfigurationVariableHelper.GetConfigVariableValueAsBool(ConfigConstants.EnableTicketReopenByRequestor);
            GridViewTableDataCell editCell = null;
            GridViewDataColumn cellColumn = null;
            int cellIndex = 0;
            int truncateTextTo = 0;
            //bool allowExe = false;
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    editCell = (GridViewTableDataCell)cell;
                    cellColumn = editCell.Column as GridViewDataColumn;

                    cellIndex = cellColumn.VisibleIndex;

                    if (cellIndex == 1)
                    {
                        editCell.CssClass = "edit-ticket-cell";
                        //cellColumn.AdaptivePriority = 1;
                    }
                    // adjust cell alignment with header alignment for left aligned column
                    if (cellColumn.CellStyle.HorizontalAlign == HorizontalAlign.Left || cellColumn.CellStyle.HorizontalAlign == HorizontalAlign.Right)
                    {
                        editCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "7px");
                    }

                    string fieldName = cellColumn.FieldName;
                    moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldName == fieldName);
                    string columnType = string.Empty;

                    if (moduleColumn != null && !string.IsNullOrWhiteSpace(moduleColumn.ColumnType))
                        columnType = moduleColumn.ColumnType.ToLower();

                    if (columnType == "string" && UGITUtility.StringToInt(moduleColumn.TruncateTextTo) > 0)
                        truncateTextTo = UGITUtility.StringToInt(moduleColumn.TruncateTextTo);

                    
                    field = FieldConfigurationManager.GetFieldByFieldName(fieldName);

                    string fieldColumnType = string.Empty;
                    if (field != null)
                    {
                        fieldColumnType = Convert.ToString(field.Datatype);
                    }
                    else if (!string.IsNullOrEmpty(columnType))
                        fieldColumnType = columnType;



                    if (fieldName == DatabaseObjects.Columns.TicketPriorityLookup)
                    {
                        if(fieldName== DatabaseObjects.Columns.TicketPriorityLookup)
                        {
                            fieldName = "PriorityLookup";
                        }
                        #region SP Code
                        string priority = Convert.ToString(e.GetValue(fieldName));
                        if (!string.IsNullOrEmpty(priority) && !string.IsNullOrEmpty(moduleName) && _moduleViewManager != null)
                        {
                            UGITModule uModule = _moduleViewManager.LoadByName(moduleName, true);
                            ModulePrioirty currentpriority = null;
                            if (uModule != null && uModule.List_Priorities.Count > 0)
                                currentpriority = uModule.List_Priorities.FirstOrDefault(x => x.ModuleNameLookup == moduleName && (x.Title == priority || x.ID == UGITUtility.StringToLong(priority)));
                            string priorityTitle = currentpriority == null ? priority : currentpriority.Title;
                            string priorityText = "<div style='white-space:nowrap; border-radius:4px; padding:6px 2px; text-align:center; '>" + priorityTitle + "</div>";
                            if (currentpriority != null)
                            {
                                Color colorName = ColorTranslator.FromHtml(currentpriority.Color);
                                Color foreColor = new Color();
                                if (!string.IsNullOrWhiteSpace(currentpriority.Color))
                                {
                                    foreColor = UGITUtility.IdealTextColor(colorName);
                                }

                                priorityText = "<div style='white-space:nowrap;border-radius:4px; padding:6px 2px; text-align:center;color:" + foreColor.Name + ";background-color:" + currentpriority.Color + "'>" + currentpriority.Title + "</div>";
                                priority = currentpriority.Title;
                            }

                            editCell.Text = priorityText;
                        }
                        if (!string.IsNullOrEmpty(priority) &&  priority.Contains("Critical"))
                        {
                            editCell.Style.Add(HtmlTextWriterStyle.Color, "#e24a7a");
                            editCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        else if(!string.IsNullOrEmpty(priority) && priority.Contains("High")) 
                        {
                            editCell.Style.Add(HtmlTextWriterStyle.Color, "#FFA533"); 
                            editCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        else if (!string.IsNullOrEmpty(priority) && priority.Contains("Medium"))
                        {
                            editCell.Style.Add(HtmlTextWriterStyle.Color, "#43C55F");
                            //editCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        else if (!string.IsNullOrEmpty(priority) && priority.Contains("Low"))
                        {
                            editCell.Style.Add(HtmlTextWriterStyle.Color, "#3AA8DC");
                            //editCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        //33FF7D
                        #endregion
                    }



                    if (fieldName == DatabaseObjects.Columns.TicketStatus || fieldName == DatabaseObjects.Columns.ModuleStepLookup)
                    {
                        // Check if we need to show field as a progress bar
                        if (columnType == "progressbar" && !string.IsNullOrEmpty(moduleName))
                        {
                            LifeCycleStage currentStage = null;
                            LifeCycle defaultLifeCycle = objTicket.GetTicketLifeCycle(currentRow);
                            if (defaultLifeCycle != null)
                            {
                                double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                                currentStage = objTicket.GetTicketCurrentStage(currentRow);
                                if (currentStage != null)
                                {
                                    double tillStageWeight = defaultLifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                                    int pctComplete = (int)(tillStageWeight / totalWeight * 100);
                                    editCell.Text = UGITUtility.GetProgressBar(defaultLifeCycle, currentStage, currentRow, fieldName, false, false);


                                }
                            }
                        }




                    }

                    if (fieldName == DatabaseObjects.Columns.TicketAge)
                    {
                        // ageColorByTargetCompletion = _configurationVariableManager.GetValueAsBool(ConfigConstants.AgeColorByTargetCompletion);
                        int ticketAge = UGITUtility.StringToInt(e.GetValue(DatabaseObjects.Columns.TicketAge));
                        editCell.Text = UGITUtility.GetAgeBar(currentRow, true, ticketAge);
                    }
                    if (fieldName == DatabaseObjects.Columns.CRMCompanyLookup && !string.IsNullOrEmpty(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup])))
                    {
                        string modulename = uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup]));
                        editCell.Text = UGITUtility.ObjectToString(_ticketManager.GetByTicketIdFromCache(modulename, UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup]))[DatabaseObjects.Columns.Title]);
                    }
                    if (field != null && !string.IsNullOrEmpty(UGITUtility.ObjectToString(e.GetValue(fieldName))) && (field.Datatype == FieldType.Date.ToString() || field.Datatype == FieldType.DateTime.ToString()))
                    {
                        editCell.Text = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(e.GetValue(fieldName)), columnType == FieldType.DateTime.ToString().ToLower());
                    }
                } 
            }
        }

        protected void globalSearchPage_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Created")
            {
                DateTime createdDate;
                if (DateTime.TryParse(Convert.ToString(e.CellValue), out createdDate))
                    e.Cell.Text = createdDate.ToString("MMM-d-yyyy");
                else
                    e.Cell.Text = "";

            }
        }
    }
}
