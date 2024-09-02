
using DevExpress.Web;
using DevExpress.Web.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    class ColCheckBoxTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            CheckBox _chkBox = new CheckBox();
            _chkBox.ID = "chkbox";
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            container.Controls.Add(_chkBox);
        }
    }

    class ColCheckBoxRoleName : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            CheckBox _chkBox = new CheckBox();
            _chkBox.ID = "chkboxSelectAll";
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            container.Controls.Add(_chkBox);
        }
    }

    public partial class ServiceMatrix : UserControl
    {
        ApplicationRoleManager appRoleManager = new ApplicationRoleManager(HttpContext.Current.GetManagerContext());

        ApplicationModuleManager appModuleManager = new ApplicationModuleManager(HttpContext.Current.GetManagerContext());// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationModules);

        ApplicationHelperManager objApplicationHelperManager = new ApplicationHelperManager(HttpContext.Current.GetManagerContext());
        #region mandatory properties
        /// <summary>
        /// Use ACCESSTYPE_ADD, ACCESSTYPE_REMOVE, ACCESSTYPE_REMOVEALL constant to set value of this property
        /// </summary>
        public string AccessType { get; set; }

        /// <summary>
        /// It is required only in case add/change access.
        /// </summary>
        public List<int> Applications { get; set; }

        public string _RoleAssignee;
        public string _MirrorAccessFromUser;
        public string RoleAssignee
        {
            get
            {
                return _RoleAssignee;
            }
            set
            {
                if (_RoleAssignee != value)
                {
                    _RoleAssignee = value;
                    userAccessData = objApplicationHelperManager.InitializeDataByUser(value);
                }
            }
        }
        public string MirrorAccessFromUser
        {
            get
            {
                return _MirrorAccessFromUser;
            }
            set
            {
                if (_MirrorAccessFromUser != value)
                {
                    _MirrorAccessFromUser = value;
                    InitializeDataByUserMirror(value);
                }
            }
        }
        public string MandatoryMessage { get; set; }
        public string ValidationGroup { get; set; }
        public string ParentControl { get; set; }
        public string ControlIDPrefix { get; set; }
        public List<string> LstAccessRequestModes = new List<string>();

        public bool IsMobile { get; set; }
        public bool MandatoryCheck { get; set; }       
        public bool IsReadOnly { get; set; }
        public bool IsNoteEnabled { get; set; }
        public bool ShowAccessDescription { get; set; }
        public bool DependentToExistingUser { get; set; }
        //needed to check if selected application is greater than show application in config variable on client side
        public int SelectedApplicationCount { get; set; }
        public int ShowApplications { get; set; }
        public bool DisableAllCheckBox { get; set; }
        public bool ShowBasedOnAccessAdmin { get; set; }
        public string AccessAdmin { get; set; }
        public const string ACCESSTYPE_ADD = "add";
        public const string ACCESSTYPE_REMOVE = "remove";
        public const string ACCESSTYPE_REMOVEALL = "removeall";

        private DataTable userAccessData;
        private DataTable mirrorAccessData;
        private DataTable appRoles;
        private DataTable applicationData;

        private List<Tuple<string, string, int>> lstAccessChanges = new List<Tuple<string, string, int>>();
        #region variables
        public List<ServiceMatrixData> ServiceMatrixDataList { get; set; }

        UGITModule appModuleConfig;

        #endregion
        #endregion
        ApplicationContext applicationContext = null;
        TicketManager objTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        public bool isRoleOrderExist;
        public bool isModuleOrderExist;
        #region Events
        protected override void OnInit(EventArgs e)
        {
            applicationContext = HttpContext.Current.GetManagerContext();
            appTabPage.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            appTabPage.EnableViewState = false;

            appModuleConfig = ObjModuleViewManager.GetByName("APP");
            if (string.IsNullOrWhiteSpace(AccessType))
                AccessType = ACCESSTYPE_ADD;

            csChkChangedData.Visible = MandatoryCheck;

            //only create access options in case of more then 1 access mode
            if (LstAccessRequestModes != null && LstAccessRequestModes.Count > 1)
            {
                LstAccessRequestModes = LstAccessRequestModes.OrderBy(x => x).ToList();
                divAccessRequestModes.Visible = true;
                lblAccessRequestType.Visible = false;
                foreach (var item in LstAccessRequestModes)
                {
                    if (item.ToLower() == ACCESSTYPE_ADD)
                        rdblstAccessReqMode.Items.Add(new ListItem("Add/Change", ACCESSTYPE_ADD));
                    else if (item.ToLower() == ACCESSTYPE_REMOVE)
                        rdblstAccessReqMode.Items.Add(new ListItem("Remove from Specific Application(s)", ACCESSTYPE_REMOVE));
                    else if (item.ToLower() == ACCESSTYPE_REMOVEALL)
                        rdblstAccessReqMode.Items.Add(new ListItem("Remove from All Application(s)", ACCESSTYPE_REMOVEALL));
                }
                if (!string.IsNullOrEmpty(AccessType))
                    rdblstAccessReqMode.SelectedIndex = rdblstAccessReqMode.Items.IndexOf(rdblstAccessReqMode.Items.FindByValue(AccessType.ToLower()));
            }

            if (ParentControl != null && ParentControl.ToLower() == "application")
                chkbxExistingAccess.Visible = true;
            else
                chkbxExistingAccess.Visible = false;

            Reload();
        }

        protected void rdblstAccessReqMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }
        #endregion

        #region Helpers
        public void Reload()
        {
            if (rdblstAccessReqMode.Items.Count > 0 && !string.IsNullOrWhiteSpace(Request[rdblstAccessReqMode.UniqueID]))
                AccessType = Request[rdblstAccessReqMode.UniqueID];

            if (AccessType.ToLower() == ACCESSTYPE_ADD)
            {
                DataTable allApplications = objTicketManager.GetOpenTickets(appModuleConfig);

                //return if no application exist
                if (allApplications == null || allApplications.Rows.Count == 0)
                    return;

                if (Applications == null || Applications.Count == 0)
                {
                    applicationData = allApplications;
                    Applications = applicationData.AsEnumerable().Select(x => Convert.ToInt32(x.Field<long>(DatabaseObjects.Columns.Id))).ToList();
                }
                else
                {
                    DataRow[] selectedApps = new DataRow[0];
                    selectedApps = (from apps in allApplications.AsEnumerable()
                                    join p in Applications on apps.Field<long>(DatabaseObjects.Columns.ID) equals p
                                    select apps).ToArray();
                    if (selectedApps.Length > 0)
                    {
                        applicationData = selectedApps.CopyToDataTable();
                    }
                }
                if (applicationData == null || applicationData.Rows.Count == 0)
                    return;

                if (UGITUtility.StringToBoolean(Request[chkbxExistingAccess.UniqueID]))
                {
                    chkbxExistingAccess.Checked = true;
                    chkbxExistingAccess_CheckedChanged(chkbxExistingAccess, new EventArgs());
                }
                else
                {
                    this.ServiceMatrixDataList = CreateServiceMatrix(applicationData);
                    CreateTable();
                }
            }
            else if (AccessType.ToLower() == ACCESSTYPE_REMOVE)
            {
                this.ServiceMatrixDataList = GetExistingAccess(true);
                CreateTable();
            }
            else
            {
                this.ServiceMatrixDataList = GetExistingAccess();
                CreateTable();
            }
        }

        public void LoadOnState(List<ServiceMatrixData> matric)
        {
            if (matric != null && matric.Count > 0)
            {
                RoleAssignee = matric[0].RoleAssignee;
                AccessType = matric[0].AccessRequestMode;
                MirrorAccessFromUser = matric[0].MirrorAccessFromUser;
                Applications = matric.Select(x => Convert.ToInt32(x.ID)).ToList();
                if (appModuleConfig == null)
                    appModuleConfig = ObjModuleViewManager.LoadByName("APP");
                
                DataTable allApplications =  objTicketManager.GetAllTickets(appModuleConfig);
                //return if no application exist
                if (allApplications == null || allApplications.Rows.Count == 0)
                    return;


                DataRow[] selectedApps = new DataRow[0];
                selectedApps = (from apps in allApplications.AsEnumerable()
                                join p in Applications on apps.Field<long>(DatabaseObjects.Columns.ID) equals p
                                select apps).ToArray();
                if (selectedApps.Length > 0)
                {
                    applicationData = selectedApps.CopyToDataTable();
                }

                if (applicationData == null || applicationData.Rows.Count == 0)
                    return;
            }

            this.ServiceMatrixDataList = matric;
            CreateTable();

        }

        private List<ServiceMatrixData> CreateServiceMatrixForPreview(DataTable appsData)
        {
            string userID = GetRoleAssignee();
            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
            foreach (DataRow item in appsData.Rows)
            {
                ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                serviceMatrixData.Name = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                serviceMatrixData.ID = Convert.ToString(item[DatabaseObjects.Columns.Id]);
                if (!string.IsNullOrWhiteSpace(userID))
                    serviceMatrixData.RoleAssignee = Convert.ToString(userID);
                serviceMatrixData.AccessRequestMode = AccessType;
                serviceMatrixData.lstGridData = objApplicationHelperManager.GetExistingAccessOfUser(item, userAccessData);
                serviceMatrixData.lstMirrorAccess = GetExistingAccessOfMirrorUser(item);
                string moduleLookupCollection =Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleLookup]);
                if (serviceMatrixData.lstGridData == null)
                    serviceMatrixData.lstGridData = new List<ServiceData>();
                if (serviceMatrixData.lstRowsNames != null && serviceMatrixData.lstRowsNames.Count > 0)
                    serviceMatrixDataList.Add(serviceMatrixData);
                serviceMatrixDataList.ForEach(x => { x.lstRowsNames = x.lstRowsNames.OrderBy(y => y).ToList(); });
                serviceMatrixDataList.ForEach(x => { x.lstColumnsNames = x.lstColumnsNames.OrderBy(y => y).ToList(); });
            }
            return serviceMatrixDataList;
        }

        private List<ServiceMatrixData> CreateServiceMatrix(DataTable appsData)
        {
            string userID = GetRoleAssignee();

            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();

            foreach (DataRow item in appsData.Rows)
            {
                ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                serviceMatrixData.Name = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                serviceMatrixData.ID = Convert.ToString(item[DatabaseObjects.Columns.Id]);
                if (!string.IsNullOrWhiteSpace(userID))
                    serviceMatrixData.RoleAssignee = Convert.ToString(userID);

                int MirrorAccessFromID = 0;
                if (!string.IsNullOrEmpty(MirrorAccessFromUser))
                    MirrorAccessFromID = UGITUtility.StringToInt(MirrorAccessFromUser);
                if (MirrorAccessFromID > 0)
                    serviceMatrixData.MirrorAccessFromUser = Convert.ToString(MirrorAccessFromID);
                serviceMatrixData.AccessRequestMode = AccessType;

                serviceMatrixData.lstGridData = objApplicationHelperManager.GetExistingAccessOfUser(item,userAccessData);
                serviceMatrixData.lstMirrorAccess = GetExistingAccessOfMirrorUser(item);

                if (serviceMatrixData.lstGridData != null && serviceMatrixData.lstMirrorAccess != null && serviceMatrixData.lstMirrorAccess.Count > 0)
                {
                    var newAccess = serviceMatrixData.lstMirrorAccess.Select(x => new { x.ColumnName, x.RowName }).Except(serviceMatrixData.lstGridData.Select(x => new { x.ColumnName, x.RowName })).ToList();
                    var existingAccess = serviceMatrixData.lstMirrorAccess.Select(x => new { x.ColumnName, x.RowName }).Intersect(serviceMatrixData.lstGridData.Select(x => new { x.ColumnName, x.RowName })).ToList();
                    if ((newAccess != null && newAccess.Count > 0) || (existingAccess != null && existingAccess.Count > 0))
                        serviceMatrixData.lstGridData = new List<ServiceData>();
                    foreach (var newAcs in newAccess)
                    {
                        ServiceData d = new ServiceData();
                        d.RowName = newAcs.RowName;
                        d.ColumnName = newAcs.ColumnName;
                        serviceMatrixData.lstGridData.Add(d);
                    }
                    foreach (var extAcs in existingAccess)
                    {
                        ServiceData d = new ServiceData();
                        d.RowName = extAcs.RowName;
                        d.ColumnName = extAcs.ColumnName;
                        serviceMatrixData.lstGridData.Add(d);
                    }
                }
                // 
                if (serviceMatrixData.lstGridData == null)
                    serviceMatrixData.lstGridData = new List<ServiceData>();

                GetAppRolesModules(ref serviceMatrixData);

                //only show those applications which are having module list
                if (serviceMatrixData.lstRowsNames != null && serviceMatrixData.lstRowsNames.Count > 0)
                    serviceMatrixDataList.Add(serviceMatrixData);

            }

            serviceMatrixDataList.ForEach(x => { x.lstRowsNames = x.lstRowsNames.OrderBy(y => y).ToList(); });
            serviceMatrixDataList.ForEach(x => { x.lstColumnsNames = x.lstColumnsNames.OrderBy(y => y).ToList(); });

            return serviceMatrixDataList.OrderBy(x => x.Name).ToList();
        }

       

        public List<ServiceData> GetExistingAccessOfMirrorUser(DataRow app)
        {
            List<ServiceData> lstServiceData = new List<ServiceData>();

            if (mirrorAccessData == null || mirrorAccessData.Rows.Count == 0)
                return lstServiceData;

            DataRow[] accessDataRows = new DataRow[0];
            if (app == null)
                accessDataRows = mirrorAccessData.Select();
            else
                accessDataRows = mirrorAccessData.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.APPTitleLookup, UGITUtility.StringToLong(app[DatabaseObjects.Columns.Id])));

            if (accessDataRows.Length == 0)
                return lstServiceData;

            foreach (DataRow spItem in accessDataRows)
            {
                ServiceData serviceData = new ServiceData();
                serviceData.ID = Convert.ToString(spItem[DatabaseObjects.Columns.Id]);
                string spFieldLookupValueRoles =Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationRoleLookup]);
                string spFieldLookupValueApp = Convert.ToString(spItem[DatabaseObjects.Columns.APPTitleLookup]);
                serviceData.RowName = spFieldLookupValueRoles;
                if (string.IsNullOrEmpty(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup])))
                    serviceData.ColumnName = spFieldLookupValueApp;
                else
                {
                    string spFieldLookupValueModules = (Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup]));
                    serviceData.ColumnName = spFieldLookupValueModules;
                }
                serviceData.State = 0;
                lstServiceData.Add(serviceData);
            }

            return lstServiceData;
        }

        private List<ServiceMatrixData> GetExistingAccess(bool selectedAppsOnly = false)
        {
            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
            DataTable allApplications = objTicketManager.GetAllTickets(appModuleConfig);
            if (allApplications == null || allApplications.Rows.Count == 0)
                return serviceMatrixDataList;

            string userID = GetRoleAssignee();

            if (allApplications == null || allApplications.Rows.Count == 0 || userAccessData == null || userAccessData.Rows.Count == 0)
                return serviceMatrixDataList;

            DataTable userAccessApps = userAccessData.DefaultView.ToTable(true, DatabaseObjects.Columns.APPTitleLookup);
            applicationData = allApplications.Clone();
            Applications = new List<int>();
            foreach (DataRow uAccess in userAccessApps.Rows)
            {
                ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                string spFieldApplLookup = (Convert.ToString(uAccess[DatabaseObjects.Columns.APPTitleLookup]));

                //No application for access
                if (string.IsNullOrWhiteSpace(spFieldApplLookup))
                    continue;
                //don't show application if those are not selected during question setup
                if (selectedAppsOnly && Applications.Count > 0 && !Applications.Exists(x => x ==UGITUtility.StringToInt(spFieldApplLookup)))
                    continue;

                DataRow[] applicationRows = allApplications.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.Id, spFieldApplLookup));
                DataRow app = null;
                if (applicationRows.Length > 0)
                {
                    app = applicationRows[0];
                    applicationData.Rows.Add(app.ItemArray);
                    Applications.Add(UGITUtility.StringToInt(app[DatabaseObjects.Columns.Id]));
                }
                //no application found
                if (app == null)
                    continue;

                serviceMatrixData.Name = spFieldApplLookup;
                serviceMatrixData.ID = spFieldApplLookup.ToString();
                serviceMatrixData.RoleAssignee = Convert.ToString(userID);
                serviceMatrixData.AccessRequestMode = AccessType;
                serviceMatrixData.lstGridData = objApplicationHelperManager.GetExistingAccessOfUser(app, userAccessData);
                serviceMatrixData.lstMirrorAccess = GetExistingAccessOfMirrorUser(app);
                DataTable userAccessAppData = userAccessData.DefaultView.ToTable(true, DatabaseObjects.Columns.APPTitleLookup, DatabaseObjects.Columns.ApplicationRoleLookup, DatabaseObjects.Columns.ApplicationModulesLookup);
                DataRow[] accessDataRows = userAccessData.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.APPTitleLookup, UGITUtility.StringToLong(spFieldApplLookup)));
                foreach (DataRow item in accessDataRows)
                {
                    serviceMatrixData = GetExistingRoleModules(item, serviceMatrixData);
                }
                serviceMatrixDataList.Add(serviceMatrixData);
            }

            serviceMatrixDataList.ForEach(x => { x.lstRowsNames = x.lstRowsNames.OrderBy(y => y).ToList(); });
            serviceMatrixDataList.ForEach(x => { x.lstColumnsNames = x.lstColumnsNames.OrderBy(y => y).ToList(); });

            return serviceMatrixDataList.OrderBy(x => x.Name).ToList();
        }

        public void SaveState()
        {
            SaveState(GetSavedState());
        }
        public void SaveState(List<ServiceMatrixData> serviceMatrixList)
        {
            ApplicationModuleManager applicationModuleManager = new ApplicationModuleManager(applicationContext);
            foreach (TabPage tabItem in appTabPage.TabPages)
            {
                List<ServiceData> lstServiceEnableData = new List<ServiceData>();
                List<ServiceData> lstServiceData = new List<ServiceData>();
                HiddenField hndApplicationId = (HiddenField)tabItem.FindControl("hndApplicationId" + tabItem.Name);
                if (hndApplicationId != null)
                {
                    Label lblNotes = tabItem.FindControl("lblNotes" + hndApplicationId.Value) as Label;
                    Label lblNotesText = tabItem.FindControl("lblNotesText" + hndApplicationId.Value) as Label;
                    TextBox txtNotes = tabItem.FindControl("txtNotes" + hndApplicationId.Value) as TextBox;
                    ASPxGridView grdApplModuleRoleMap = (ASPxGridView)tabItem.FindControl("grdApplModuleRoleMapAppId" + hndApplicationId.Value);
                    string userId = GetRoleAssignee();
                    DataRow[] userAccessList = null;
                    List<ApplicationModule> spItemApplicationModules = null;
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        spItemApplicationModules = applicationModuleManager.Load(x => x.APPTitleLookup == UGITUtility.StringToInt(hndApplicationId.Value));
                        string where = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.APPTitleLookup, UGITUtility.StringToInt(hndApplicationId.Value), DatabaseObjects.Columns.ApplicationRoleAssign, userId);
                        DataTable dataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, where);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                            userAccessList = dataTable.AsEnumerable().ToArray();
                    }

                    for (int i = 0; i < grdApplModuleRoleMap.VisibleRowCount; i++)
                    {
                        DataRow dr = grdApplModuleRoleMap.GetDataRow(i);

                        string key = dr["RoleName"].ToString();
                        int cellsCount = grdApplModuleRoleMap.Columns.Count;
                        int cntr = 1;
                        while (cntr < cellsCount)
                        {
                            GridViewDataColumn qtyColumn = (GridViewDataColumn)grdApplModuleRoleMap.Columns[cntr];
                            string id = string.Format("{0}_{1}_{2}{3}", ControlIDPrefix, hndApplicationId.Value, i, cntr);
                            CheckBox chkApplModuleRole = (CheckBox)grdApplModuleRoleMap.FindControlRecursive(id);
                            if (chkApplModuleRole == null)
                                chkApplModuleRole = new CheckBox();

                            bool addAccess = false;
                            bool changeAccess = false;

                            if (userAccessList != null && userAccessList.Count() > 0)
                            {
                                bool check = false;

                                if (spItemApplicationModules != null && spItemApplicationModules.Count == 0)
                                    check = userAccessList.Where(x => UGITUtility.SplitString(x[DatabaseObjects.Columns.ApplicationRoleLookup], Constants.Separator, 1) == key).ToList().Count > 0;
                                else
                                    check = userAccessList.Where(x => UGITUtility.SplitString(x[DatabaseObjects.Columns.ApplicationModulesLookup], Constants.Separator, 1) == qtyColumn.FieldName &&
                                                                      UGITUtility.SplitString(x[DatabaseObjects.Columns.ApplicationRoleLookup], Constants.Separator, 1) == key).ToList().Count > 0;
                                if (check)
                                    changeAccess = (check == chkApplModuleRole.Checked) ? false : true;
                                else
                                    addAccess = chkApplModuleRole.Checked;
                            }
                            else if(chkApplModuleRole!=null)
                            {
                                addAccess = chkApplModuleRole.Checked;
                            }

                            if (addAccess || changeAccess)
                            {
                                ServiceData serviceData = new ServiceData();
                                serviceData.ColumnName = chkApplModuleRole.Attributes["ItemText"];
                                serviceData.RowName = chkApplModuleRole.Attributes["ItemValue"];
                                serviceData.ID = chkApplModuleRole.Attributes["ItemId"];
                                serviceData.Value = Convert.ToString(chkApplModuleRole.Checked);
                                lstServiceEnableData.Add(serviceData);
                            }
                            cntr++;
                        }
                    }

                    // While saving access, Skip if below condition met.
                    bool changeNoXml = ShowBasedOnAccessAdmin && IsReadOnly;
                    if (!changeNoXml && ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0)
                    {
                        var items = ServiceMatrixDataList.Where(c => c.ID == hndApplicationId.Value).Select(c => c).ToList();
                        items[0].lstGridData = lstServiceEnableData;
                        items[0].AccessRequestMode = rdblstAccessReqMode.SelectedValue;
                        items[0].RoleAssignee = !string.IsNullOrEmpty(ServiceMatrixDataList[0].RoleAssignee) ? ServiceMatrixDataList[0].RoleAssignee : !string.IsNullOrEmpty(RoleAssignee) ? RoleAssignee : string.Empty;
                        items[0].MirrorAccessFromUser = !string.IsNullOrEmpty(ServiceMatrixDataList[0].MirrorAccessFromUser) ? ServiceMatrixDataList[0].MirrorAccessFromUser : !string.IsNullOrEmpty(MirrorAccessFromUser) ? MirrorAccessFromUser : string.Empty;

                        if (!string.IsNullOrEmpty(txtNotes.Text) && txtNotes.Text != "Any Special Instructions")
                            items[0].Note = txtNotes.Text;
                        else
                            items[0].Note = string.Empty;
                    }
                }
            }
        }

        public List<ServiceMatrixData> GetSavedState()
        {
            return ServiceMatrixDataList;
        }

        public void InitializeDataByUserMirror(string userID)
        {
            string dateQuery = string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='User'>{1}</Value></Eq>", DatabaseObjects.Columns.ApplicationRoleAssign, userID);
            //SPSiteDataQuery childQuery = new SPSiteDataQuery();
            DataTable wfhList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");

            wfhList = wfhList.DefaultView.ToTable(true);

            mirrorAccessData = wfhList;

        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (Applications != null && Applications.Count > 0)
            {
                ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                SelectedApplicationCount = Applications.Count;
                ShowApplications = UGITUtility.StringToInt(objConfigurationVariableManager.GetValue(ConfigConstants.ApplicationAccessPageSize), 10);
            }

            foreach (TabPage tabItem in appTabPage.TabPages)
            {
                HiddenField hndApplicationId = (HiddenField)tabItem.FindControl("hndApplicationId" + tabItem.Name);
                if (hndApplicationId != null)
                {
                    ASPxGridView grdApplModuleRoleMap = (ASPxGridView)tabItem.FindControl("grdApplModuleRoleMapAppId" + hndApplicationId.Value);


                }
            }

        }

        private void CreateAccessChangesDescription()
        {
            divAccessChanges.Visible = false;
            if (lstAccessChanges != null && lstAccessChanges.Count > 0)
            {
                divAccessChanges.Visible = true;
                var lstAcess = lstAccessChanges.GroupBy(z => z.Item3).OrderBy(x => x.Key);
                List<string> readData = new List<string>();
                foreach (var ctr in lstAcess)
                {
                    StringBuilder str = new StringBuilder();
                    if (ctr.Key == 1)
                        str.Append("<b>Add access to</b>");
                    else if (ctr.Key == 2)
                        str.Append("<b>Remove access from</b>");
                    else if (ctr.Key == 3)
                        str.Append("<b>No Change in Access</b>");
                    var moduleGruops = ctr.ToArray().GroupBy(x => x.Item2);
                    foreach (var mg in moduleGruops)
                    {
                        str.AppendFormat("&nbsp;<b>{0}:</b> as {1}; ", mg.Key, string.Join(", ", mg.ToArray().Select(x => x.Item1)));
                    }
                    readData.Add(str.ToString());
                }
                divAccessChanges.InnerHtml = string.Join("<br/>", readData);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (rdblstAccessReqMode.Visible && rdblstAccessReqMode.Items.Count > 0)
                AccessType = rdblstAccessReqMode.SelectedValue;

            if (!string.IsNullOrEmpty(AccessType))
                rdblstAccessReqMode.SelectedIndex = rdblstAccessReqMode.Items.IndexOf(rdblstAccessReqMode.Items.FindByValue(AccessType.ToLower()));
            if (!IsReadOnly && !ShowAccessDescription)
            {
                lblAccessRequestType.Visible = true;
                if (!string.IsNullOrEmpty(AccessType))
                {
                    if (AccessType == ACCESSTYPE_ADD)
                        lblAccessRequestType.Text = "Add/Change Access for User";
                    else if (AccessType == ACCESSTYPE_REMOVE)
                        lblAccessRequestType.Text = "Remove Specific Access from Application";
                    else if (AccessType == ACCESSTYPE_REMOVEALL)
                        lblAccessRequestType.Text = "Remove All Access from Application";
                }
                else
                {

                }
            }
            else if (ShowAccessDescription)
            {
                CreateAccessChangesDescription();
            }
            if (rdblstAccessReqMode.Items.Count > 0 || (ParentControl != null && ParentControl.ToLower() != "service"))
                lblAccessRequestType.Visible = false;
        }

        protected void cvMandatoryData_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (MandatoryCheck)
            {
                SaveState();
                string userId = GetRoleAssignee();
                if (!string.IsNullOrWhiteSpace(userId) && txtHidden.Text != "[$skip$]")
                {
                    if (ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0)
                    {

                        ServiceRequestBL serviceRequestBL = new ServiceRequestBL(HttpContext.Current.GetManagerContext());
                        foreach (ServiceMatrixData serviceMatrixData in ServiceMatrixDataList)
                        {
                            if (string.IsNullOrEmpty(serviceMatrixData.RoleAssignee) && !string.IsNullOrEmpty(RoleAssignee))
                                serviceMatrixData.RoleAssignee = RoleAssignee;
                            if (serviceRequestBL.CheckIsRelationChanged(serviceMatrixData))
                            {
                                args.IsValid = true;
                                break;
                            }
                            else
                            {
                                MandatoryMessage = " No changes made from previous request";
                                args.IsValid = false;
                            }
                        }
                    }
                    else
                    {
                        args.IsValid = false;
                    }

                }
                else if (string.IsNullOrWhiteSpace(userId) && txtHidden.Text != "[$skip$]")
                {
                    args.IsValid = false;
                    MandatoryMessage = "Please Specify";
                    if (ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0 &&
                        ServiceMatrixDataList.Any(x => x.lstGridData != null && x.lstGridData.Count > 0))
                    {
                        args.IsValid = true;
                    }
                }
                else if (string.IsNullOrWhiteSpace(userId) && txtHidden.Text == "[$skip$]")
                { args.IsValid = true; }
                csChkChangedData.ErrorMessage = MandatoryMessage;
            }

        }

        protected void devexGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string userId = GetRoleAssignee();
            string appID = string.Empty;
            ASPxGridView grdApplModuleRoleMap = (ASPxGridView)sender;
            if (!string.IsNullOrEmpty(grdApplModuleRoleMap.Attributes["gvId"]))
                appID = grdApplModuleRoleMap.Attributes["gvId"];
            grdApplModuleRoleMap.CssClass += " appID_" + appID;
            if (ServiceMatrixDataList == null || ServiceMatrixDataList.Count == 0)
                return;
            var selectedApp = ServiceMatrixDataList.FirstOrDefault(c => c.ID == appID);
            if (e.RowType == GridViewRowType.Data)
            {
                if (e.Row.Cells[0].GetType().ToString() == "DevExpress.Web.Rendering.GridViewTableNoColumnsCell")
                    return;
                GridViewTableDataCell editCellValue = (GridViewTableDataCell)e.Row.Cells[0];
                string rowText = string.Empty;
                if (((GridViewDataColumn)editCellValue.Column).FieldName.ToLower() != "")
                    rowText = Convert.ToString(grdApplModuleRoleMap.GetRowValues(e.VisibleIndex, ((GridViewDataColumn)editCellValue.Column).FieldName));
                DataRow currentRow = grdApplModuleRoleMap.GetDataRow(e.VisibleIndex);
                string key = currentRow["RoleName"].ToString();
                for (int Index = 0; Index < e.Row.Cells.Count; Index++)
                {

                    if (e.Row.Cells[Index] is GridViewTableDataCell)
                    {
                        GridViewTableDataCell editCell = (GridViewTableDataCell)e.Row.Cells[Index];
                        GridViewDataColumn qtyColumn = (GridViewDataColumn)editCell.Column;
                        string columnValue = Convert.ToString(grdApplModuleRoleMap.GetRowValues(e.VisibleIndex, ((GridViewDataColumn)editCell.Column).FieldName));
                        string cellText = string.Empty;
                        if (qtyColumn.FieldName.ToLower() != "")
                        {
                            string columnsName = ((GridViewDataColumn)editCell.Column).FieldName;
                            if (columnsName.ToLower() == "rolename")
                            {
                                CheckBox chkCheckBox = new CheckBox();
                                chkCheckBox.ID = "chkbox" + rowText;
                                chkCheckBox.Attributes.Add("onclick", "selectAllModules(this);");
                                chkCheckBox.Attributes.Add("ischecked", "false");
                                chkCheckBox.Text = rowText;
                                chkCheckBox.CssClass += " selectallCheckbox";
                                chkCheckBox.Attributes.Add("ItemText", columnsName);
                                chkCheckBox.Attributes.Add("ItemValue", rowText);
                                chkCheckBox.Attributes.Add("ItemId", appID);
                                chkCheckBox.Style.Add("float", "left");
                                if (DisableAllCheckBox || AccessType == ACCESSTYPE_REMOVEALL)
                                    chkCheckBox.Enabled = false;
                                ServiceData serviceData = new ServiceData();
                                serviceData.ColumnName = chkCheckBox.Attributes["ItemText"];
                                serviceData.RowName = chkCheckBox.Attributes["ItemValue"];
                                serviceData.ID = chkCheckBox.Attributes["ItemId"];
                                if (!IsReadOnly)
                                {
                                    editCell.Text = string.Empty;
                                    Label lblRowText = new Label();
                                    lblRowText.Text = rowText;
                                    lblRowText.CssClass = "lblRoleName";
                                    editCell.Controls.Add(chkCheckBox);
                                   // editCell.Controls.Add(lblRowText);
                                }
                                   
                            }
                            else if (columnsName.ToLower() != rowText.ToLower())
                            {
                                DataTable dt = new DataTable();
                                Dictionary<string, object> values = new Dictionary<string, object>();
                                values.Add("@TenantID", applicationContext.TenantID);
                                values.Add("@ApplicationId", appID);
                                dt = GetTableDataManager.GetData(DatabaseObjects.Tables.ApplicationRole, values);
                                string ass = string.Format("{0}='{1}' AND ( {2} like'%{3}%' OR {4}='{5}' )", DatabaseObjects.Columns.Title, rowText, "ApplicationRoleModuleLookupName",columnsName, "ApplicationRoleModuleLookup","0");
                                DataRow[] drRoleLookups_ = dt.Select(ass);

                                CheckBox chkCheckBox = (CheckBox)grdApplModuleRoleMap.FindRowCellTemplateControlByKey(key, qtyColumn, "chkbox");
                                if (drRoleLookups_.Count() > 0)
                                {
                                    var items_ = selectedApp.lstRoleModuleMap.Where(c => c.Role == rowText && (c.Module == drRoleLookups_[0]["ApplicationRoleModuleLookup"].ToString() || c.Module == "0" || c.Module == UGITUtility.ObjectToString(drRoleLookups_[0]["ApplicationRoleModuleLookupName"]))).Select(c => c).ToList();
                                    
                                    if (items_.Count > 0)
                                    {
                                        chkCheckBox.Enabled = true;
                                        if (chkCheckBox == null)
                                            return;
                                        chkCheckBox.Attributes.Add("onclick", "toggleAccess(this);");
                                        chkCheckBox.Attributes.Add("ischecked", "false");
                                        chkCheckBox.ID = string.Format("{0}_{1}_{2}{3}", ControlIDPrefix, appID, e.VisibleIndex, Index);
                                        chkCheckBox.Attributes.Add("ItemText", columnsName);
                                        chkCheckBox.CssClass += " moduleCheckbox";
                                        chkCheckBox.Attributes.Add("ItemValue", rowText);
                                        chkCheckBox.Attributes.Add("ItemId", appID);
                                        ServiceData serviceData = new ServiceData();
                                        serviceData.ColumnName = chkCheckBox.Attributes["ItemText"];
                                        serviceData.RowName = chkCheckBox.Attributes["ItemValue"];
                                        serviceData.ID = chkCheckBox.Attributes["ItemId"];
                                        DataRow[] dr = new DataRow[0];
                                        if (userAccessData != null && userAccessData.Rows.Count > 0)
                                        {
                                            //dr = userAccessData.Select(string.Format("{0} like '%;#{1}' And ({2} like '%;#{3}' or {4} like '%;#{5}')",
                                            //                            DatabaseObjects.Columns.ApplicationRoleLookup, serviceData.RowName.Replace("'", "''"),
                                            //                            DatabaseObjects.Columns.ApplicationModulesLookup, serviceData.ColumnName.Replace("'", "''"),
                                            //                            DatabaseObjects.Columns.APPTitleLookup, serviceData.ColumnName.Replace("'", "''")));
                                            DataRow[] drRoleLookups = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationRole).Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, serviceData.RowName.ToString()));
                                            DataRow[] drAppModuleLookups = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationModules).Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, serviceData.ColumnName.ToString()));
                                            DataRow[] drAppTitleLookups = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications).Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, serviceData.ID.ToString()));

                                            int ApplicationRoleLookup = drRoleLookups.Count() > 0 ? Convert.ToInt32(drRoleLookups[0][DatabaseObjects.Columns.ID]) : 0;
                                            int ApplicationModulesLookup = drAppModuleLookups.Count() > 0 ? Convert.ToInt32(drAppModuleLookups[0][DatabaseObjects.Columns.ID]) : 0;
                                            int APPTitleLookup = drAppTitleLookups.Count() > 0 ? Convert.ToInt32(drAppTitleLookups[0][DatabaseObjects.Columns.ID]) : 0;
                                            dr = userAccessData.Select(string.Format("{0}={1} And ({2}={3} and {4}={5})",
                                                                      DatabaseObjects.Columns.ApplicationRoleLookup, ApplicationRoleLookup,
                                                                      DatabaseObjects.Columns.ApplicationModulesLookup, ApplicationModulesLookup,
                                                                      DatabaseObjects.Columns.APPTitleLookup, APPTitleLookup));
                                        }
                                        if (!IsReadOnly)
                                        {
                                            if (!string.IsNullOrEmpty(columnValue) && columnValue.ToLower() == "true")
                                            {
                                                chkCheckBox.Checked = true;
                                                chkCheckBox.Attributes.Add("ischecked", "true");
                                                if (dr == null || dr.Length == 0)//condition for new access
                                                {
                                                    editCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#ffff9e");
                                                    chkCheckBox.Attributes.Add("ischeckedbckgrnd", "#ffff9e");
                                                    lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 1));
                                                }
                                                else
                                                {
                                                    lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 3));//condition for no change
                                                }
                                            }
                                            else
                                            {
                                                if (dr != null && dr.Length > 0)
                                                {
                                                    if (AccessType == ACCESSTYPE_REMOVEALL)
                                                    {
                                                        Image imgControl = new Image();
                                                        imgControl.ImageUrl = "/content/images/uGovernIT/crossicon.jpg";
                                                        imgControl.Visible = true;
                                                        imgControl.Style.Add("vertical-align", "middle");
                                                        chkCheckBox.Visible = false;
                                                        editCell.Controls.Add(imgControl);
                                                    }
                                                    else
                                                    {
                                                        editCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFE7E4");
                                                        chkCheckBox.Attributes.Add("ischeckedbckgrnd", "#FFE7E4");
                                                        lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 2));
                                                        editCell.Controls.Add(chkCheckBox);
                                                    }
                                                }
                                                else//no value existing access
                                                {
                                                    var items = selectedApp.lstRoleModuleMap.Where(c => c.Role == Convert.ToString(serviceData.RowName) && (c.Module == Convert.ToString(serviceData.ID) || c.Module == "All")).Select(c => c).ToList();
                                                    if (items != null && items.Count > 0)
                                                    {
                                                        if (!string.IsNullOrEmpty(AccessType) && AccessType != ACCESSTYPE_ADD)//if remove option then show checkbox for existing access only
                                                        {
                                                            var item = selectedApp.lstGridData.Where(c => c.RowName == Convert.ToString(serviceData.RowName) && (c.ColumnName == Convert.ToString(serviceData.ID))).Select(c => c).ToList();
                                                            if (item != null && item.Count == 0)
                                                            {
                                                                chkCheckBox.Visible = false;
                                                            }
                                                        }
                                                    }
                                                    //else
                                                    //    chkCheckBox.Visible = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Image imgControl = new Image();

                                            if (!string.IsNullOrEmpty(columnValue) && columnValue.ToLower() == "true")
                                            {
                                                imgControl.ImageUrl = "/content/images/uGovernIT/message_good.png";
                                                imgControl.Visible = true;
                                                imgControl.Style.Add("vertical-align", "middle");
                                                if ((dr == null) || dr.Length == 0)
                                                {
                                                    editCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#ffff9e");
                                                    lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 1));
                                                }
                                                else
                                                {
                                                    lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 3));
                                                }
                                            }
                                            else
                                            {
                                                if ((dr != null && dr.Length != 0))
                                                {
                                                    imgControl.ImageUrl = "/Content/images/uGovernIT/crossicon.jpg";
                                                    imgControl.Visible = true;
                                                    imgControl.Style.Add("vertical-align", "middle");
                                                    editCell.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFE7E4");
                                                    lstAccessChanges.Add(new Tuple<string, string, int>(serviceData.RowName, serviceData.ColumnName, 2));
                                                }
                                                else
                                                    imgControl.Visible = false;
                                            }
                                            chkCheckBox.Visible = false;
                                            editCell.Controls.Add(imgControl);
                                        }
                                    }
                                    else
                                    {
                                        chkCheckBox.Enabled = false;
                                    }
                                }
                                else
                                {
                                    chkCheckBox.Enabled = false;
                                }
                                
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }

        protected void devexGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Header)
            {

            }
        }

        private void CreateTable()
        {
            string userId = GetRoleAssignee();

            ulAppTab.Controls.Clear();

            appTabPage.Controls.Clear();
            appTabPage.TabPages.Clear();

            if (ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0)
            {
                string strAppTabs = "";
                int i = 0;
                foreach (ServiceMatrixData appMtx in ServiceMatrixDataList)
                {
                    DataRow app = applicationData.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID).ToString() == appMtx.ID);
                    if (app == null)
                        continue;

                    int appID = Convert.ToInt16(app[DatabaseObjects.Columns.Id]);
                    string appTitle = Convert.ToString(app[DatabaseObjects.Columns.Title]);

                    TextBox txtNotes = new TextBox();
                    GridView grdApplModuleRoleMap = new GridView();
                    grdApplModuleRoleMap.EmptyDataText = "No Access Found";

                    Label lblNotes = new Label();
                    Label lblNotesText = new Label();
                    HiddenField hndApplicationId = new HiddenField();

                    txtNotes.ID = "txtNotes" + appID;
                    txtNotes.CssClass = "txtNotes";
                    txtNotes.TextMode = TextBoxMode.MultiLine;

                    lblNotes.ID = "lblNotes" + appID;
                    lblNotesText.ID = "lblNotesText" + appID;
                    hndApplicationId.Value = Convert.ToString(appID);

                    ASPxGridView devexGrid = new ASPxGridView();
                    devexGrid.ID = "grdApplModuleRoleMap" + "AppId" + appID;
                    devexGrid.HtmlRowCreated += devexGrid_HtmlRowCreated;
                    devexGrid.ClientInstanceName = "devexGrid";
                    devexGrid.ClientSideEvents.Init += "function(s, e) {SetApplicationGridWidth(s,e);}";

                    devexGrid.Styles.AlternatingRow.CssClass = "tablerow alternateBackground";
                    devexGrid.Styles.Row.CssClass = "tablerow subCategoryBackground";
                    devexGrid.Styles.Header.CssClass = "titleHeaderBackground";
                    //devexGrid.Styles.Header.Font.Bold = true;
                    devexGrid.Styles.Header.Wrap = DevExpress.Utils.DefaultBoolean.True;
                    devexGrid.CssClass = "gvResourceClass applAccess_content";
                    //devexGrid.CssClass = "gvResourceClass";
                    devexGrid.SettingsPager.AlwaysShowPager = false;
                   
                    devexGrid.AutoGenerateColumns = false;
                    devexGrid.Styles.Cell.HorizontalAlign = HorizontalAlign.Center;
                    devexGrid.SettingsBehavior.AllowSort = false;
                    devexGrid.Width = new Unit(589);
                    devexGrid.Settings.VerticalScrollableHeight = 240;
                    devexGrid.SettingsText.EmptyDataRow = "Please specify user to show current access";
                    devexGrid.SettingsText.EmptyHeaders = "&nbsp;";
                    devexGrid.KeyFieldName = "RoleName";
                    if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
                    {
                        
                        devexGrid.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCells;
                        devexGrid.SettingsAdaptivity.AllowOnlyOneAdaptiveDetailExpanded = true;
                        //devexGrid.SettingsAdaptivity.HideDataCellsAtWindowInnerWidth = 1000;
                        devexGrid.SettingsAdaptivity.AdaptiveColumnPosition = GridViewAdaptiveColumnPosition.Right;
                        devexGrid.SettingsCommandButton.ShowAdaptiveDetailButton.Text = "+";
                        devexGrid.SettingsCommandButton.HideAdaptiveDetailButton.Text = "x";
                        devexGrid.SettingsCommandButton.ShowAdaptiveDetailButton.RenderMode = GridCommandButtonRenderMode.Button;
                        devexGrid.SettingsCommandButton.HideAdaptiveDetailButton.RenderMode = GridCommandButtonRenderMode.Button;
                        devexGrid.Width = Unit.Percentage(100);
                        devexGrid.SettingsCommandButton.ShowAdaptiveDetailButton.Styles.Style.CssClass = "applAccessReg_expandBtn";
                        devexGrid.SettingsCommandButton.HideAdaptiveDetailButton.Styles.Style.CssClass = "applAccessReg_collapseBtn";
                    }
                    else
                    {
                        devexGrid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        devexGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                        devexGrid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        devexGrid.Width =  new Unit(587);

                    }

                    
                   


                    TabPage tabPage = new TabPage();
                    tabPage.Controls.Add(hndApplicationId);
                    tabPage.Controls.Add(devexGrid);
                    tabPage.Controls.Add(txtNotes);
                    tabPage.Controls.Add(hndApplicationId);
                    tabPage.Controls.Add(lblNotes);
                    tabPage.Controls.Add(lblNotesText);
                    appTabPage.TabPages.Insert(i, tabPage);
                    int cntrlCount = tabPage.Controls.Count;
                    tabPage.Text = appTitle;
                    tabPage.Name = "tab_AppId" + appID;

                    if (i == 0)
                    {
                        strAppTabs += "<li class='active' onclick='javascript:tabclick(" + i + ",\"" + appTabPage.ClientID + "\")'>" + appTitle + "</li>";
                    }
                    else
                    {
                        strAppTabs += "<li onclick='javascript:tabclick(" + i + ",\"" + appTabPage.ClientID + "\")'>" + appTitle + "</li>";
                    }

                    hndApplicationId.ID = "hndApplicationId" + tabPage.Name;
                    if (!string.IsNullOrWhiteSpace(userId)|| !DependentToExistingUser)
                    {
                        LoadControlByAppId(Convert.ToString(appID), hndApplicationId, txtNotes, lblNotes, lblNotesText, devexGrid);
                    }
                    i++;
                }
                ulAppTab.Controls.Add(new LiteralControl(strAppTabs));

                divGrd.Style.Add("width", "auto");
                divGrd.Style.Add("height", "auto");
                divGrd.Style.Add("min-height", "auto !important");

                if (Applications.Count == 1)
                {
                    divAppTab.Visible = false;
                    spanApplicationName.Visible = true;
                    lblApplicationName.Text = ServiceMatrixDataList[0].Name;
                    lblNoAccess.Visible = false;
                }
                else if (Applications.Count == 0)
                {
                    spanApplicationName.Visible = false;
                    divAppTab.Visible = false;
                    lblNoAccess.Visible = true;
                    lblNoAccess.Text = "No Existing Access";
                }
                else // multiple applications
                {
                    spanApplicationName.Visible = false;
                    divAppTab.Visible = true;
                    lblNoAccess.Visible = false;
                }
            }
            else
            {
                spanApplicationName.Visible = false;
                divAppTab.Visible = false;
                lblNoAccess.Visible = true;
                if (IsReadOnly)
                    lblNoAccess.Text = "No Change in Access";
                else
                    lblNoAccess.Text = "No Existing Access";
            }
        }

        protected void devexGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

        }

        public void LoadControlByAppId(string id, HiddenField hndApplicationId, TextBox txtNotes, Label lblNotes, Label lblNotesText, ASPxGridView devexGrid)
        {
            if (ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0)
            {
                ServiceMatrixData serviceMatrixData = ServiceMatrixDataList.FirstOrDefault(x => x.ID == id);
                DataTable dtMatrix = new DataTable();

              //  dtMatrix.Columns.Add(new DataColumn("SelectAll"));
                dtMatrix.Columns.Add(new DataColumn("RoleName"));
                if (serviceMatrixData.lstRowsNames != null && serviceMatrixData.lstRowsNames.Count() > 0)
                {
                    serviceMatrixData.lstRowsNames = serviceMatrixData.lstRowsNames.Distinct().ToList();
                    foreach (string rowItem in serviceMatrixData.lstRowsNames)
                    {
                        DataRow dr = dtMatrix.NewRow();
                        dr["RoleName"] = rowItem;
                        dtMatrix.Rows.Add(dr);
                    }
                }
              
                if (serviceMatrixData.lstColumnsNames != null && serviceMatrixData.lstColumnsNames.Count() > 0)
                {
                    serviceMatrixData.lstColumnsNames = serviceMatrixData.lstColumnsNames.Distinct().ToList();
                    foreach (string columnItem in serviceMatrixData.lstColumnsNames)
                    {
                        dtMatrix.Columns.Add(new DataColumn(columnItem));
                    }
                }
               
                if (serviceMatrixData.lstGridData != null && serviceMatrixData.lstGridData.Count() > 0)
                {
                    foreach (DataRow dr in dtMatrix.Rows)
                    {
                        foreach (DataColumn dc in dtMatrix.Columns)
                        {
                            string columnName = dc.ColumnName;
                            List<ServiceData> serviceData = serviceMatrixData.lstGridData;
                            foreach(ServiceData sd in serviceData)
                            {
                                if (Int64.TryParse(sd.ColumnName, out long result))
                                {
                                    ApplicationModule appModule = appModuleManager.LoadByID(Convert.ToInt64(sd.ColumnName));
                                    if (appModule != null)
                                        sd.ColumnName = appModule.Title;
                                    ApplicationRole appRole = appRoleManager.LoadByID(Convert.ToInt64(sd.RowName));
                                    if (appRole != null)
                                        sd.RowName = appRole.Title;
                                }
                            }
                            ServiceData serviceDataItem = new ServiceData();
                            serviceDataItem.RowName = Convert.ToString(dr["RoleName"]);
                            serviceDataItem.ColumnName = Convert.ToString(dc.ColumnName);
                            var items = serviceData.Where(c => c.ColumnName == Convert.ToString(dc.ColumnName) && c.RowName == Convert.ToString(dr["RoleName"])).Select(c => c).ToList();
                            if (items != null && items.Count() > 0)
                            {
                                if (AccessType == ACCESSTYPE_REMOVEALL)
                                    dr[dc] = false;
                                else
                                {
                                    dr[dc] = true;
                                }
                            }
                        }
                    }
                }
                hndApplicationId.Value = id.ToString();
                devexGrid.Attributes.Add("gvId", Convert.ToString(id));
                string appID = string.Empty;
                appID = Convert.ToString(id);
                if (ServiceMatrixDataList == null || ServiceMatrixDataList.Count == 0)
                    return;
                var selectedApp = ServiceMatrixDataList.FirstOrDefault(c => c.ID == appID);
                foreach (DataColumn dc in dtMatrix.Columns)
                {
                    GridViewDataColumn column = new GridViewDataColumn(dc.ColumnName);
                    //if (dc.ColumnName.ToLower() == "selectall")
                    //{
                    //    column.DataItemTemplate = new ColCheckBoxRoleName();
                    //}
                    //else 
                    if (dc.ColumnName.ToLower() != "rolename")
                    {
                        column.DataItemTemplate = new ColCheckBoxTemplate();
                       
                    }
                    else
                    {
                        if (selectedApp.lstRowsNames != null)
                        {
                           
                             column.FixedStyle = GridViewColumnFixedStyle.Left;
                            column.HeaderStyle.CssClass = "appMatrixTitle";
                            string divHtml = string.Empty;
                            if (selectedApp.lstColumnsNames != null && selectedApp.lstColumnsNames.Count == 1 && selectedApp.lstColumnsNames[0] == selectedApp.Name)
                            {
                                divHtml = @"<div><span style='margin-left: 36px;font-size:12px;margin-top:0px;position:relative; color:white;'>Role</span></div>";
                            }
                            else
                            {
                                column.Width = 110;
                                divHtml = @"<div><span style='font-size: 12px;margin-left: 6px;margin-top: 0px;float:left;position:relative; color:white;'>Role \ Module</span></div>";
                            }
                            column.Caption = divHtml;

                        }
                    }
                    devexGrid.Columns.Add(column);
                }
                devexGrid.KeyFieldName = "RoleName";
                devexGrid.DataSource = dtMatrix;
                devexGrid.DataBind();

                if (IsNoteEnabled == true && IsReadOnly == false)
                {
                    txtNotes.Style.Add("display", "block");
                    lblNotes.Style.Add("display", "none");
                    lblNotesText.Style.Add("display", "none");
                    txtNotes.Text = lblNotesText.Text = serviceMatrixData.Note;
                }
                else if (IsNoteEnabled == true && IsReadOnly == true)
                {
                    txtNotes.Style.Add("display", "none");
                    if (!string.IsNullOrEmpty(serviceMatrixData.Note))
                    {
                        lblNotes.Style.Add("display", "inline");
                        lblNotesText.Style.Add("display", "inline");
                        lblNotesText.Text = serviceMatrixData.Note;
                    }
                    else
                    {
                        lblNotes.Style.Add("display", "none");
                        lblNotesText.Style.Add("display", "none");
                    }
                }
                else if (IsNoteEnabled == false)
                {
                    txtNotes.Style.Add("display", "none");
                    lblNotes.Style.Add("display", "none");
                    lblNotesText.Style.Add("display", "none");
                }
                if (ShowAccessDescription)
                {
                    lblAssignee.Style.Add("display", "inline");
                    UserProfile  arrRoleAssignee = HttpContext.Current.GetUserManager().GetUserById(Convert.ToString(ServiceMatrixDataList[0].RoleAssignee));
                    if (arrRoleAssignee != null)
                        lblAssignee.Text = "User whose access needs to be updated:" + arrRoleAssignee.Name + " (" + arrRoleAssignee.UserName.Replace("i:0#.w|", "") + ")";
                    else
                        lblAssignee.Style.Add("display", "none");
                }
                else if (IsReadOnly == false)
                {

                    lblAssignee.Style.Add("display", "none");
                    UserProfile arrRoleAssignee =HttpContext.Current.GetUserManager().GetUserById(Convert.ToString(serviceMatrixData.RoleAssignee));
                    if (arrRoleAssignee != null )
                    {
                        lblAssignee.Text = arrRoleAssignee.Name + "(" + arrRoleAssignee.UserName.Replace("i:0#.w|", "") + ")";
                    }
                }
            }
        }

        private string GetRoleAssignee()
        {
            string UserId = "";
            if (!string.IsNullOrWhiteSpace(RoleAssignee))
            {
                UserId = Convert.ToString(RoleAssignee);
            }
            if (string.IsNullOrWhiteSpace(UserId) && ServiceMatrixDataList != null && ServiceMatrixDataList.Count > 0 && !string.IsNullOrEmpty(ServiceMatrixDataList[0].RoleAssignee))
            {
                UserId =ServiceMatrixDataList[0].RoleAssignee;
            }
            return UserId;
        }

        public ServiceMatrixData GetAppRolesModules(ref ServiceMatrixData serviceMatrixData)
        {
            string query = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.APPTitleLookup, serviceMatrixData.ID,DatabaseObjects.Columns.TenantID, HttpContext.Current.GetManagerContext().TenantID);
            DataTable splistAppRolesDt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationRole,query);
            List<string> lstRowNames = new List<string>();
            List<RoleModuleData> lstRoleModuleName = new List<RoleModuleData>();
            if (splistAppRolesDt != null)
            {
                DataRow[] splistAppRoles = splistAppRolesDt.Select(query);
                if (splistAppRoles != null && splistAppRoles.Count() > 0)
                {
                    appRoles = splistAppRoles.CopyToDataTable();
                    lstRowNames = splistAppRoles.CopyToDataTable().AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.Title)).ToList();
                    foreach (DataRow item in splistAppRoles)
                    {
                        List<string> moduleLookupCollection = UGITUtility.ConvertStringToList(Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleModuleLookup]), Constants.Separator);
                        if (moduleLookupCollection != null && moduleLookupCollection.Count > 0)
                        {
                            foreach (string moduleLookup in moduleLookupCollection)
                            {
                                RoleModuleData roleModule = new RoleModuleData();
                                roleModule.Role = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                                if (!string.IsNullOrWhiteSpace(moduleLookup))
                                    roleModule.Module = moduleLookup; //== "0" ? "All" : moduleLookup;
                                lstRoleModuleName.Add(roleModule);
                            }
                        }
                        else
                        {
                            RoleModuleData roleModule = new RoleModuleData();
                            roleModule.Role = Convert.ToString(item[DatabaseObjects.Columns.Title]);
                            roleModule.Module = "All";
                            lstRoleModuleName.Add(roleModule);
                        }
                    }
                }
            }

            string queryModule = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.APPTitleLookup, serviceMatrixData.ID, DatabaseObjects.Columns.TenantID, HttpContext.Current.GetManagerContext().TenantID);
            DataTable splistAppModulesDt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationModules, queryModule);
            List<string> lstColumnNames = new List<string>();
            if (splistAppRolesDt != null)
            {
                DataRow[] splistAppModules = splistAppModulesDt.Select(queryModule);
                if (splistAppModules == null || splistAppModules.Count() == 0)
                    lstColumnNames.Add(serviceMatrixData.Name);
                else
                    lstColumnNames = splistAppModules.CopyToDataTable().AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.Title)).ToList();
            }

            serviceMatrixData.lstRoleModuleMap = lstRoleModuleName;
            serviceMatrixData.lstRowsNames = lstRowNames;
            serviceMatrixData.lstColumnsNames = lstColumnNames;
            return serviceMatrixData;
        }

        private ServiceMatrixData GetExistingRoleModules(DataRow item, ServiceMatrixData serviceMatrixData)
        {
            string roleLookupCollection =(Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleLookup]));
            string moduleLookupCollection =(Convert.ToString(item[DatabaseObjects.Columns.ApplicationModulesLookup]));
            if (moduleLookupCollection != null)
            {
                if (serviceMatrixData.lstColumnsNames == null)
                    serviceMatrixData.lstColumnsNames = new List<string>();
                if (!string.IsNullOrWhiteSpace(moduleLookupCollection))
                {
                    ApplicationModule role = appModuleManager.LoadByID(UGITUtility.StringToLong(moduleLookupCollection));
                    if(role != null)
                        serviceMatrixData.lstColumnsNames.Add(role.Title);
                }
                else
                {
                    string appLookup = (Convert.ToString(item[DatabaseObjects.Columns.APPTitleLookup]));
                    if (appLookup != null)
                        serviceMatrixData.lstColumnsNames.Add(appLookup);
                }
            }
            if (roleLookupCollection != null)
            {
                //DataRow drAppRow = appRoles.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == roleLookupCollection.LookupId);
                if (serviceMatrixData.lstRowsNames == null)
                    serviceMatrixData.lstRowsNames = new List<string>();
                if (!string.IsNullOrEmpty(roleLookupCollection))
                {
                    ApplicationRole role = appRoleManager.LoadByID(UGITUtility.StringToLong(roleLookupCollection));
                    roleLookupCollection = role.Title;
                }
                serviceMatrixData.lstRowsNames.Add(Convert.ToString(roleLookupCollection));
                List<string> roleModuleLookupCollection = UGITUtility.ConvertStringToList(Convert.ToString(DatabaseObjects.Columns.ApplicationRoleModuleLookup),Constants.Separator);
                if (serviceMatrixData.lstRoleModuleMap == null)
                    serviceMatrixData.lstRoleModuleMap = new List<RoleModuleData>();
                if (roleModuleLookupCollection != null && roleModuleLookupCollection.Count > 0)
                {

                    foreach (string moduleLookup in roleModuleLookupCollection)
                    {
                        RoleModuleData roleModule = new RoleModuleData();
                        roleModule.Role = Convert.ToString(roleLookupCollection);
                        if (!string.IsNullOrWhiteSpace(moduleLookup))
                            roleModule.Module = moduleLookup;
                        serviceMatrixData.lstRoleModuleMap.Add(roleModule);
                    }
                }
                else
                {
                    RoleModuleData roleModule = new RoleModuleData();
                    roleModule.Role = Convert.ToString(roleLookupCollection);
                    roleModule.Module = "All";
                    serviceMatrixData.lstRoleModuleMap.Add(roleModule);
                }

            }
            return serviceMatrixData;
        }

        private List<ServiceMatrixData> BindExistingRolesModules()
        {
            int i = userAccessData.Rows.Count;
            string userID = GetRoleAssignee();
            DataRow app = applicationData.AsEnumerable().FirstOrDefault();
            DataRow[] accessDataRows = null;
            if (app == null)
                accessDataRows = userAccessData.Select();
            else
                accessDataRows = userAccessData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.APPTitleLookup, app[DatabaseObjects.Columns.Id]));


            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
            List<string> lstColumnNames = new List<string>();
            List<string> lstRowNames = new List<string>();
            List<RoleModuleData> lstRoleModuleName = new List<RoleModuleData>();
            ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
            serviceMatrixData.Name = Convert.ToString(app[DatabaseObjects.Columns.Title]);
            serviceMatrixData.ID = Convert.ToString(app[DatabaseObjects.Columns.Id]);
            if (!string.IsNullOrWhiteSpace(userID))
                serviceMatrixData.RoleAssignee = Convert.ToString(userID);
            serviceMatrixData.AccessRequestMode = AccessType;
            serviceMatrixData.lstGridData = objApplicationHelperManager.GetExistingAccessOfUser(app,  userAccessData);
            serviceMatrixData.lstMirrorAccess = GetExistingAccessOfMirrorUser(app);
            if (accessDataRows.Length != 0)
            {
                DataTable dt = accessDataRows.CopyToDataTable();
                foreach (DataRow item in dt.Rows)
                {
                    serviceMatrixData = GetExistingRoleModules(item, serviceMatrixData);
                }
            }

            if (serviceMatrixData.lstGridData == null)
                serviceMatrixData.lstGridData = new List<ServiceData>();
            // 
            serviceMatrixDataList.Add(serviceMatrixData);
            if (serviceMatrixData.lstRowsNames != null && serviceMatrixData.lstRowsNames.Count > 0)
            {
                serviceMatrixDataList.ForEach(x => { x.lstRowsNames = x.lstRowsNames.OrderBy(y => y).ToList(); });
                serviceMatrixDataList.ForEach(x => { x.lstColumnsNames = x.lstColumnsNames.OrderBy(y => y).ToList(); });
            }
            return serviceMatrixDataList.OrderBy(x => x.Name).ToList();
        }

        protected void chkbxExistingAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxExistingAccess.Checked)
            {
                this.ServiceMatrixDataList = BindExistingRolesModules();
                CreateTable();
            }
            else
            {
                this.ServiceMatrixDataList = CreateServiceMatrix(applicationData);
                CreateTable();
            }
        }
    }
}

