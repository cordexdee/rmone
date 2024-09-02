using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class CRMProjectAllocationAddEdit : UserControl
    {
        public int Id { private get; set; }

        public string ticketID { get; set; }
        public string ControlList { get; set; }
        public string moduleName = string.Empty;
        public string allocationStartDate { get; set; }
        public string allocationEndDate { get; set; }
        public string absoluteUrlSearch = "/Layouts/uGovernIT/DelegateControl.aspx?disableReloadParent=true&control={0}&ID={1}&ticketID={2}&ControlId=AllocationAddEdit&AllocationViewType=CRMProjectAuto";
       
        private string editParam = "resourceavailability";
        private ApplicationContext _context = null;
        private ProjectEstimatedAllocationManager _cRMProjectAllocationManager = null;
        private ResourceAllocationManager _resourceAllocationManager = null;
        private UserProfileManager _userProfileManager = null;

        ConfigurationVariableManager objConfigurationVariableHelper = null;
        DataTable spListItem;
        DataRow spProjectItem;
        UserProfile UserProfile = new UserProfile();
        ProjectEstimatedAllocation CRMProjectAllocation = new ProjectEstimatedAllocation();

        //public string absoluteUrlSearch = "_layouts/15/ugovernit/DelegateControl.aspx?disableReloadParent=true&control={0}&ID={1}&ticketID={2}&ControlId={3}&AllocationViewType=CRMProjectAuto";
        protected string findResourceUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=findresourceavailability");      
        protected string cprprojectId = string.Empty;
        protected string projectTitle = string.Empty;
        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ajaxhelper.aspx");  
        
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        protected ProjectEstimatedAllocationManager CRMProjectAllocationManager
        {
            get
            {
                if (_cRMProjectAllocationManager == null)
                {
                    _cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(ApplicationContext);
                }
                return _cRMProjectAllocationManager;
            }
        }

        protected ResourceAllocationManager ResourceAllocationManager
        {
            get
            {
                if (_resourceAllocationManager == null)
                {
                    _resourceAllocationManager = new ResourceAllocationManager(ApplicationContext);
                }
                return _resourceAllocationManager;
            }
        }

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(ApplicationContext);
                }
                return _userProfileManager;
            }
        }    

        protected override void OnInit(EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();

            //disable control in case of ReAllocation.
            if (Request["ModeType"] == "ReAllocation")
            {
                txtSubProject.Enabled = false;
                ddlUserType.Enabled = false;
                LnkbtnDelete.Visible = false;
            }

            if (Request["Mode"] != "RMMProjectAgent")
            {
                if (string.IsNullOrEmpty(ticketID))
                {
                    return;
                }
                else
                {
                    spProjectItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);

                    if (moduleName == "CPR" || moduleName == "CNS")
                        cprprojectId = Convert.ToString(spProjectItem[DatabaseObjects.Columns.CRMProjectID]);

                    projectTitle = Convert.ToString(spProjectItem[DatabaseObjects.Columns.Title]);
                }
            }

            if (Request["Mode"] == "ProjectAgent")
            {
                FillGroupName();
                LnkbtnDelete.Visible = false;
                trUser.Visible = false;
                trAllocation.Visible = false;
                //btnSave.Visible = false;
                lnkAllocationSave.Visible = false;
                trUserGroup.Visible = false;
                trType.Visible = false;
                trWorkItem.Visible = false;
                trSubItem.Visible = false;
                trCPRDuration.Visible = false;
            }
            else if (Request["Mode"] == "RMMProjectAgent")
            {

                FillGroupName();
                LnkbtnDelete.Visible = false;
                trUser.Visible = false;
                trAllocation.Visible = false;
                // btnSave.Visible = false;
                lnkAllocationSave.Visible = false;
                trUserGroup.Visible = false;
                trSubItem.Visible = false;
                trCPRDuration.Visible = false;

                if (Request["filterMode"] == "CPRTeamAllocation")
                {
                    trType.Visible = false;
                    trWorkItem.Visible = false;
                    trStartDate.Visible = false;
                    trCPRDuration.Visible = false;
                    trEndDate.Visible = false;

                    ////moduleName = UGITUtility.getModuleNameByTicketId(ticketID);
                    ////spProjectItem = Ticket.getCurrentTicket(UGITUtility.getModuleNameByTicketId(ticketID), ticketID);
                    spProjectItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);
                    projectTitle = Convert.ToString(spProjectItem[DatabaseObjects.Columns.Title]);

                    if (UGITUtility.IsSPItemExist(spProjectItem, DatabaseObjects.Columns.EstimatedConstructionStart))
                    {
                        allocationStartDate = Convert.ToString(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    }
                    else if (UGITUtility.IsSPItemExist(spProjectItem, DatabaseObjects.Columns.PreconStartDate))
                    {
                        allocationStartDate = Convert.ToString(spProjectItem[DatabaseObjects.Columns.PreconStartDate]);
                    }
                    else if (UGITUtility.IsSPItemExist(spProjectItem, DatabaseObjects.Columns.ContractStartDate))
                    {
                        allocationStartDate = Convert.ToString(spProjectItem[DatabaseObjects.Columns.ContractStartDate]);
                    }


                    if (UGITUtility.IsSPItemExist(spProjectItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                        allocationEndDate = Convert.ToString(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    else if (UGITUtility.IsSPItemExist(spProjectItem, DatabaseObjects.Columns.ContractExpirationDate))
                    {
                        allocationEndDate = Convert.ToString(spProjectItem[DatabaseObjects.Columns.ContractExpirationDate]);
                    }
                }
            }
            else
            {
                trAdditionalUserGroup.Visible = false;
                trUserGroup.Visible = true;
                btnFind.Visible = false;
                trType.Visible = false;
                trWorkItem.Visible = false;
                trSubItem.Visible = true;
            }

            if (Id > 0)
            {
                //spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMProjectAllocation, Id, SPContext.Current.Web, ViewFields);
                string spQuery = string.Format("ID = '{0}'", Id);
                spListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMProjectAllocation, $"{spQuery.ToString()} and TenantID='{ApplicationContext.TenantID}'");
                CRMProjectAllocation = CRMProjectAllocationManager.LoadByID(Id);

                pAuditInformataion.Visible = true;
                LnkbtnDelete.Visible = true;
            }
            else
            {
                ////spListItem = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMProjectAllocation).AddItem();

                pAuditInformataion.Visible = false;
                LnkbtnDelete.Visible = false;
            }

            if (!IsPostBack)
            {
                DdlUserGroups();
                FillData();
            }

            base.OnInit(e);
        }

        protected void DdlUserGroups()
        {

            ddlUserType.Items.Clear();
            ////foreach (SPGroup group in SPContext.Current.Web.SiteGroups)
            ////{
            ////    string groups = ConfigurationVariable.GetValue(ConfigConstants.GeneralGroups);

            ////    if (string.IsNullOrEmpty(groups) || groups.ToLower().Contains(group.Name.ToLower()))
            ////        ddlUserType.Items.Add(new ListEditItem(group.Name, group.Name));
            ////}

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //string groups = context.ConfigManager.GetValue(ConfigConstants.GeneralGroups);
            //DataTable dtGroups = new DataTable();
            //dtGroups.Columns.Add("ID");
            //dtGroups.Columns.Add("Name");
            //dtGroups.Columns.Add("NameRole");
            //dtGroups.Columns.Add("Role");
            //dtGroups.Columns.Add("Type");

            //UserRoleManager roleManager = new UserRoleManager(_context);
            //List<Role> sRoles = roleManager.GetRoleList();

            //foreach (Role oGroup in sRoles)
            //{
            //    if (!string.IsNullOrEmpty(groups) && !groups.ToLower().Contains(oGroup.Title.ToLower()))
            //        continue;
            //    DataRow dr = dtGroups.NewRow();
            //    dr["ID"] = oGroup.Id;
            //    dr["Name"] = oGroup.Title;
            //    dr["Role"] = oGroup.Name;
            //    dr["NameRole"] = oGroup.Name;
            //    dr["Type"] = "Group";
            //    dtGroups.Rows.Add(dr);

            //}

            // UserProfileManager.GetUserGroups

            ddlUserType.DataSource = uHelper.GetGeneralGroupsFromConfig(context);
            ddlUserType.TextField = "Name";
            ddlUserType.ValueField = "ID";


            ddlUserType.DataBind();

        }

        protected override void OnLoad(EventArgs e)
        {
            if (Request["Mode"] == "ProjectAgent" || Request["Mode"] == "RMMProjectAgent")
            {
                if (!IsPostBack)
                {
                    DataTable dataTable = (DataTable)glUserGroup.DataSource;
                    List<string> usergroup = new List<string>();
                    glUserGroup.Text = string.Join("; ", usergroup.ToArray());
                }
                if (Request["Mode"] == "ProjectAgent")
                    SetDeaultDates();

            }
            else
            {
                if (!IsPostBack)
                {
                    if (Id < 1)
                    {
                        SetDeaultDates();
                    }
                }
                ////absoluteUrlSearch = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlSearch, editParam, "0", ticketID, dvReload.ClientID));
                absoluteUrlSearch = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlSearch, editParam, "0", ticketID));
                imgAdd.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}','pStartDate={3}&pEndDate={4}&pGlobalRoleID={5}','{2}','80','90',0)", absoluteUrlSearch, Server.UrlEncode(Request.Url.AbsolutePath), "Resource Availability", startDate.Date, endDate.Date, ddlUserType.Value));
            }
        }

        private void SetDeaultDates()
        {
            DateTime pStartDate = DateTime.MinValue;
            DateTime pEndDate = DateTime.MinValue;
            moduleName = uHelper.getModuleNameByTicketId(ticketID);
            if (moduleName == ModuleNames.CPR || moduleName == ModuleNames.OPM || moduleName == ModuleNames.CNS)
            {

                //new line for new requirement.
                if (spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart] != null && spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value)
                {
                    if (spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart] != null && Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart]) != DateTime.MinValue)
                    {
                        if (spProjectItem[DatabaseObjects.Columns.PreconStartDate] != null && spProjectItem[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value)
                        {
                            if (spProjectItem[DatabaseObjects.Columns.PreconStartDate] != null && Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.PreconStartDate]) != DateTime.MinValue)
                            {
                                if (Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart]) < Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.PreconStartDate]))
                                    pStartDate = Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                else
                                    pStartDate = Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.PreconStartDate]);
                            }
                            else
                            {
                                pStartDate = Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                            }

                        }
                    }
                    else if (spProjectItem[DatabaseObjects.Columns.PreconStartDate] != null)
                    {
                        pStartDate = Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.PreconStartDate]);
                    }

                    if (spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd] != null && spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value)
                    {
                        if (spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd] != null && Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd]) != DateTime.MinValue)
                        {
                            pEndDate = Convert.ToDateTime(spProjectItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        }
                    }
                }
            }

            if (pStartDate != DateTime.MinValue)
            {
                if (Request["Mode"] == "RMMProjectAgent")
                {
                    ((TextBox)(startDate.Controls[0])).Text = pStartDate.ToShortDateString();

                }
                else
                {
                    ////startDate.SelectedDate = pStartDate;
                    startDate.Date = pStartDate;
                }
            }

            if (pEndDate != DateTime.MinValue)
            {
                if (Request["Mode"] == "RMMProjectAgent")
                {

                    ((TextBox)(endDate.Controls[0])).Text = pEndDate.ToShortDateString();
                }
                else
                {

                    endDate.Date = pEndDate;
                }
            }

            // absoluteUrlSearch = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlSearch, editParam, "0", ticketID, dvReload.ClientID));
            absoluteUrlSearch = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlSearch, editParam, "0", ticketID));
            imgAdd.Attributes.Add("onclick", string.Format("OpenAutoAllocationPopup(this)"));
        }

        private void FillData()
        {
            if (Id > 0)
            {


                Guid TypeId = Guid.Empty;
                Guid.TryParse(Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Type]), out TypeId);

                if (TypeId != Guid.Empty)
                    ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Type])));
                else
                    ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByText(Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Type])));

                BindUserGroup();

                ////if (spListItem.Rows[0][DatabaseObjects.Columns.UGITAssignedTo] != null && spListItem.Rows[0][DatabaseObjects.Columns.UGITAssignedTo].ToString().Contains(Constants.Separator))
                if (spListItem.Rows[0][DatabaseObjects.Columns.UGITAssignedTo] != null)
                    ASPxComboBoxGroupUser.SelectedItem = ASPxComboBoxGroupUser.Items.FindByValue((spListItem.Rows[0][DatabaseObjects.Columns.UGITAssignedTo]));

                txtAlloc.Text = Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.PctAllocation]);

                DateTime dtcstartDate = (spListItem.Rows[0][DatabaseObjects.Columns.AllocationStartDate] != null && spListItem.Rows[0][DatabaseObjects.Columns.AllocationStartDate] != DBNull.Value) ? Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.AllocationStartDate]) : DateTime.Now;
                DateTime dtcendDate = (spListItem.Rows[0][DatabaseObjects.Columns.AllocationEndDate] != null && spListItem.Rows[0][DatabaseObjects.Columns.AllocationEndDate] != DBNull.Value) ? Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.AllocationEndDate]) : DateTime.Now;

                if (Request["ModeType"] == "ReAllocation")
                {

                    if (dtcstartDate > DateTime.Now)
                        startDate.Date = dtcstartDate;
                    else
                        startDate.Date = DateTime.Now;

                    if (dtcendDate > DateTime.Now)
                        endDate.Date = dtcendDate;
                    else
                        endDate.Date = startDate.Date.AddDays(1);
                }
                else
                {
                    startDate.Date = dtcstartDate;
                    endDate.Date = dtcendDate;
                }

                txtSubProject.Text = Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Title]);

                ////SPFieldUserValue userAuthorValue = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(spListItem[DatabaseObjects.Columns.Author]));
                ////SPFieldUserValue userEditorValue = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(spListItem[DatabaseObjects.Columns.Editor]));

                ////lbCreatedInfo.Text = string.Format("Created at {0} by {1}", UGITUtility.GetDateStringInFormat(Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.Created]), true), userAuthorValue.LookupValue);
                ////lbModifiedInfo.Text = string.Format("Modified at {0} by {1}", UGITUtility.GetDateStringInFormat(Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.Modified]), true), userEditorValue.LookupValue);

                if (!string.IsNullOrEmpty(Convert.ToString(spListItem.Rows[0][DatabaseObjects.Columns.Duration])))
                    txtCPRDuration.Text = Convert.ToString(Convert.ToInt32(spListItem.Rows[0][DatabaseObjects.Columns.Duration]));
            }
            else
            {
                if (Convert.ToString(Request["FromSTP"]) == "true")
                {
                    trSTPMessage.Visible = true;

                }
            }
        }

        private List<UserWithPercentage> UpdateProjectWithTeamMembers(ProjectEstimatedAllocation CRMProjectAllocation)
        {
            string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.UGITAssignedTo, DatabaseObjects.Columns.Type, DatabaseObjects.Columns.AllocationEndDate, DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.PctAllocation, DatabaseObjects.Columns.Title);


            var ti = ticketID;
            ////SPListItemCollection finalCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CRMProjectAllocation, DatabaseObjects.Columns.TicketId, ticketID, "Text", SPContext.Current.Web, viewFields, true);

            string where = string.Format("{0} = '{1}'  ", DatabaseObjects.Columns.TicketId, ticketID);
            List<ProjectEstimatedAllocation> finalCollection = CRMProjectAllocationManager.Load(where);

            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            ////SPFieldUserValueCollection users = null;
            UserProfile users = new UserProfile();


            ////if (spCurrentItem != null)
            ////{
            ////    users = (SPFieldUserValueCollection)spCurrentItem[DatabaseObjects.Columns.UGITAssignedTo];
            ////}

            if (CRMProjectAllocation != null)
            {
                ////users = (SPFieldUserValueCollection)spCurrentItem[DatabaseObjects.Columns.UGITAssignedTo];
                users.Id = CRMProjectAllocation.AssignedTo;
            }
            ////SPFieldUserValueCollection estimators = new SPFieldUserValueCollection();
            ////SPFieldUserValueCollection pms = new SPFieldUserValueCollection();
            ////SPFieldUserValueCollection apms = new SPFieldUserValueCollection();
            ////SPFieldUserValueCollection projectExecutives = new SPFieldUserValueCollection();
            ////SPFieldUserValueCollection superintendent = new SPFieldUserValueCollection();

            List<UserProfile> estimators = null;
            //List<UserProfile> pms = null;
            List<UserProfile> apms = null;
            //List<UserProfile> projectExecutives = null;
            //List<UserProfile> superintendent = null;


            if (finalCollection != null)
            {

                foreach (ProjectEstimatedAllocation spLItem in finalCollection)
                {

                    ////switch (Convert.ToString(spLItem[DatabaseObjects.Columns.Type]))
                    switch (Convert.ToString(spLItem.Type))
                    {
                        case "Assistant Project Manager":
                            //// apms.AddRange((SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo]);
                            ////apms.AddRange((IEnumerable)spLItem.AssignedTo);
                            break;
                        case "Estimator":
                            ////estimators.AddRange((SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo]);
                            break;
                        case "Project Executive":
                            ////projectExecutives.AddRange((SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo]);
                            break;
                        case "Project Manager":
                            ////pms.AddRange((SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo]);
                            break;
                        case "Superintendent":
                            ////superintendent.AddRange((SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo]);
                            break;
                    }
                    ////if (users != null && users.Count > 0)
                    ////{
                    ////    SPFieldUserValueCollection assignTo = (SPFieldUserValueCollection)spLItem[DatabaseObjects.Columns.UGITAssignedTo];
                    ////    if (assignTo != null && assignTo.Count > 0 && assignTo[0].LookupId == users[0].LookupId)
                    ////    {
                    ////        // new condition for reallocation. for allocation distribution
                    ////        if (Request["ModeType"] != "ReAllocation")
                    ////            lstUserWithPercetage.Add(new UserWithPercentage() { UserId = users[0].LookupId, Percentage = Convert.ToInt16(spLItem[DatabaseObjects.Columns.PctAllocation]), StartDate = Convert.ToDateTime(spLItem[DatabaseObjects.Columns.AllocationStartDate]), EndDate = Convert.ToDateTime(spLItem[DatabaseObjects.Columns.AllocationEndDate]), Title = Convert.ToString(spLItem[DatabaseObjects.Columns.Type]) });
                    ////    }
                    ////}

                    if (users != null)
                    {
                        users.Id = CRMProjectAllocation.AssignedTo;
                        var assignTo = spLItem.AssignedTo.ToString();
                        var usergroupId = spLItem.Type.ToString();
                        // UserProfile spUsers = UserProfileManager.GetUserById(usergroupId);

                        // for Groupname 
                        UserRoleManager roleManager = new UserRoleManager(ApplicationContext);
                        List<Role> sRoles = roleManager.GetRoleList();

                        foreach (Role oGroup in sRoles)
                        {
                            if (oGroup.Id == usergroupId)
                            {
                                spLItem.Title = oGroup.Name;
                            }

                        }
                        if (users.Id != null && assignTo == users.Id)
                        {
                            // new condition for reallocation. for allocation distribution
                            if (Request["ModeType"] != "ReAllocation")
                                lstUserWithPercetage.Add(new UserWithPercentage() { UserId = users.Id, Percentage = spLItem.PctAllocation, StartDate = (DateTime)spLItem.AllocationStartDate, EndDate = (DateTime)spLItem.AllocationEndDate, RoleTitle = spLItem.Title });
                        }
                    }

                }
            }

            else
            {
                if (Request["ModeType"] != "ReAllocation") { }
                // lstUserWithPercetage.Add(new UserWithPercentage() { UserId = users[0].LookupId, Percentage = Convert.ToInt16(txtAlloc.Text), StartDate = startDate.SelectedDate, EndDate = endDate.SelectedDate, Title = ddlUserType.Value.ToString() });
                // lstUserWithPercetage.Add(new UserWithPercentage() { UserId = users[0].LookupId, Percentage = Convert.ToInt16(txtAlloc.Text), StartDate = startDate.SelectedDate, EndDate = endDate.SelectedDate, Title = ddlUserType.Value.ToString() });

            }

            if (UGITUtility.IfColumnExists(spProjectItem, DatabaseObjects.Columns.AssistantProjectManager))
                spProjectItem[DatabaseObjects.Columns.AssistantProjectManager] = apms;

            if (UGITUtility.IfColumnExists(spProjectItem, DatabaseObjects.Columns.Estimator))
                spProjectItem[DatabaseObjects.Columns.Estimator] = estimators;

            ////if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectExecutive, spProjectItem))
            ////    spProjectItem[DatabaseObjects.Columns.ProjectExecutive] = projectExecutives;

            ////if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketProjectManager, spProjectItem))
            ////    spProjectItem[DatabaseObjects.Columns.TicketProjectManager] = pms;

            ////if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Superintendent, spProjectItem))
            ////    spProjectItem[DatabaseObjects.Columns.Superintendent] = superintendent;

            ////spProjectItem.Update();
            ////uGITCache.ModuleDataCache.UpdateOpenTicketsCache(UGITUtility.getModuleIdByTicketID(ticketID), spProjectItem);

            return lstUserWithPercetage;
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (spListItem != null)
            {
                CRMProjectAllocation = CRMProjectAllocationManager.LoadByID(Id);
                UserProfile spUsers = UserProfileManager.GetUserById(ASPxComboBoxGroupUser.SelectedItem.Value.ToString());

                if (spUsers == null && UGITUtility.IsSPItemExist(spListItem.Rows[0], DatabaseObjects.Columns.UGITAssignedTo))
                {
                    spUsers = UserProfileManager.GetUserById(CRMProjectAllocation.AssignedTo);
                }

                List<RResourceAllocation> allocations = ResourceAllocationManager.LoadByWorkItem(uHelper.getModuleNameByTicketId(ticketID), ticketID, null, 4, false, true);
                allocations = allocations.Where(x => x.Resource == spUsers.Id).ToList();

                if (allocations != null)
                {
                    foreach (RResourceAllocation rAlloc in allocations)
                    {
                        if (rAlloc.AllocationStartDate == CRMProjectAllocation.AllocationStartDate && rAlloc.AllocationEndDate == CRMProjectAllocation.AllocationEndDate)
                        {
                            ResourceAllocationManager.Delete(rAlloc);
                            if (rAlloc.ResourceWorkItemLookup > 0)
                            {
                                string webUrl = _context.SiteUrl;
                                //Start Thread to update rmm summary list for deleting entry w.r.t current allocation
                                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.DeleteRMMSummaryAndMonthDistribution(_context, rAlloc.ResourceWorkItemLookup, webUrl); };
                                Thread sThread = new Thread(threadStartMethod);
                                sThread.Start();
                            }
                        }
                    }
                }

                CRMProjectAllocation.Deleted = true;
                CRMProjectAllocationManager.Delete(CRMProjectAllocation);


            }
            uHelper.CreateHistory(UserProfile, string.Format("Deleted allocation for user no: {0}", ASPxComboBoxGroupUser.Text), Ticket.GetCurrentTicket(_context, uHelper.getModuleNameByTicketId(ticketID), ticketID), true, _context);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        //protected void cvUser_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    ArrayList userList = peUsers.ResolvedEntities;
        //    if (userList.Count == 0)
        //    {
        //        args.IsValid = false;
        //    }
        //}

        protected void FillGroupName()
        {

            DataTable dtUserGroup = new DataTable();
            //dtUserGroup.Columns.Add("GroupName", typeof(string));
            ////string groups = ConfigurationVariable.GetValue(ConfigConstants.GeneralGroups);
            //string groups = _context.ConfigManager.GetValue(ConfigConstants.GeneralGroups);

            //UserRoleManager roleManager = new UserRoleManager(_context);
            //List<Role> sRoles = roleManager.GetRoleList();

            //foreach (Role oGroup in sRoles)
            //{
            //    if (string.IsNullOrEmpty(groups) || groups.ToLower().Contains(oGroup.Name.ToLower()))
            //    {
            //        DataRow row = dtUserGroup.NewRow();
            //        row["GroupName"] = oGroup.Name;

            //        dtUserGroup.Rows.Add(row);
            //    }
            //    //ddlUserType.Items.Add(new ListEditItem(oGroup.Name, group.Name));
            //}

            //dtUserGroup.DefaultView.Sort = "GroupName ASC";

            //UserProfileManager UserManager;
            //UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            //List<string> groupList = UserManager.Users.Where(x => x.isRole == true).Select(x => x.Id).ToList();   // SPContext.Current.Web.SiteGroups.Cast<SPGroup>().Select(x => x.Name).ToList();
            //List<string> groupListName = UserManager.Users.Where(x => x.isRole == true).Select(x => x.Name).ToList();
            //DataTable data = new DataTable();
            ////data.Columns.Add("Group", typeof(string));
            //data.Columns.Add("GroupName", typeof(string));
            //foreach (string gr in groupListName)
            //{
            //    data.Rows.Add(gr);
            //}

            dtUserGroup = uHelper.GetGeneralGroupsFromConfig(HttpContext.Current.GetManagerContext());
            glUserGroup.DataSource = dtUserGroup;
            glUserGroup.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {

        }

        protected void FillDropDownLevel1(object sender, EventArgs e)
        {
            ////DropDownList level1 = (DropDownList)sender;
            ////if (level1.Items.Count <= 0)
            ////{
            ////    DataTable resultedTable = AllocationType.LoadLevel1();
            ////    if (resultedTable != null)
            ////    {
            ////        level1.Items.Add(new ListItem("--Select--", "--Select--"));
            ////        foreach (DataRow row in resultedTable.Rows)
            ////        {
            ////            if (row["LevelTitle"] != null && row["LevelTitle"].ToString() != string.Empty)
            ////            {
            ////                DataRow[] drModules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, DatabaseObjects.Columns.Title, Convert.ToString(row["LevelTitle"]));
            ////                if (drModules.Length > 0)
            ////                    level1.Items.Add(new ListItem(row["LevelTitle"].ToString(), row["LevelTitle"].ToString()));
            ////            }
            ////        }
            ////    }

            ////    DataRow[] drSelectedModules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, DatabaseObjects.Columns.ModuleName, "CPR");
            ////    level1.SelectedIndex = level1.Items.IndexOf(level1.Items.FindByText(Convert.ToString(drSelectedModules[0][DatabaseObjects.Columns.Title])));

            ////    cbLevel2.TextFormatString = "{1}";
            ////}
        }

        protected void FillDropDownLevel2(object sender, EventArgs e)
        {
            cbLevel2.Value = null;
            cbLevel2_Load(sender, e);
            //if (cbLevel2.Items.Count == 1)
            //{
            // If just one item, leave it pre-selected by default and get the level 3 items
            cbLevel2.SelectedIndex = 0;
            ////if (cbLevel2.SelectedItem != null)
            ////{
            ////    moduleName = UGITUtility.getModuleNameByTicketId(cbLevel2.SelectedItem.Value.ToString());
            ////    spProjectItem = Ticket.getCurrentTicket(moduleName, cbLevel2.SelectedItem.Value.ToString());

            ////    if (moduleName == "CPR" || moduleName == "CNS")
            ////        cprprojectId = Convert.ToString(spProjectItem[DatabaseObjects.Columns.CRMProjectID]);

            ////    projectTitle = Convert.ToString(spProjectItem[DatabaseObjects.Columns.Title]);

            ////    ticketID = Convert.ToString(spProjectItem[DatabaseObjects.Columns.TicketId]);
            ////    SetDeaultDates();


            ////}
            //}

            if (moduleName == "CPR" || moduleName == "CNS")
            {
                cbLevel2.TextFormatString = "{1}";
            }
            else
            {
                cbLevel2.TextFormatString = "{0}";
            }

        }

        protected void cbLevel2_Load(object sender, EventArgs e)
        {
            ////if (ddlLevel1.SelectedItem != null && ddlLevel1.SelectedItem.Text != "--Select--")
            ////{
            ////    DataRow[] drModules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, DatabaseObjects.Columns.Title, ddlLevel1.SelectedItem.Text.Trim());
            ////    bool fromModule = drModules != null && drModules.Length > 0;


            ////    DataTable resultedTable = AllocationType.LoadLevel2(ddlLevel1.SelectedItem.Text, fromModule);
            ////    if (resultedTable != null)
            ////    {
            ////        if (fromModule)
            ////        {
            ////            cbLevel2.Columns.Clear();

            ////            if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) == "CPR" || Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) == "CNS")
            ////            {
            ////                //resultedTable.Columns.Add("EstPrjExpression", typeof(string));

            ////                foreach (DataRow rowitem in resultedTable.Rows)
            ////                {

            ////                    if (!string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.CRMProjectID])))
            ////                    {
            ////                        rowitem[DatabaseObjects.Columns.CRMProjectID] = Convert.ToString(rowitem[DatabaseObjects.Columns.CRMProjectID]);
            ////                    }
            ////                    else
            ////                    {
            ////                        rowitem[DatabaseObjects.Columns.CRMProjectID] = Convert.ToString(rowitem[DatabaseObjects.Columns.EstimateNo]);
            ////                    }

            ////                }
            ////                cbLevel2.Columns.Add(DatabaseObjects.Columns.CRMProjectID);
            ////                // cbLevel2.Columns[0].Caption = "Project #";
            ////                // cbLevel2.Columns.Add("EstPrjExpression");
            ////                cbLevel2.Columns[0].Caption = "Est./Prj. #";
            ////                cbLevel2.Columns[0].Width = new Unit("60px");
            ////            }
            ////            else if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) == "OPM")
            ////            {
            ////                cbLevel2.Columns.Add("LevelTitle");
            ////                cbLevel2.Columns[0].Caption = "Opportunity";
            ////                cbLevel2.Columns[0].Width = new Unit("95px");
            ////            }
            ////            else if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) == "LEM")
            ////            {
            ////                cbLevel2.Columns.Add("LevelTitle");
            ////                cbLevel2.Columns[0].Caption = "Lead";
            ////                cbLevel2.Columns[0].Width = new Unit("95px");
            ////            }
            ////            else
            ////            {
            ////                cbLevel2.Columns.Add("LevelTitle");
            ////                cbLevel2.Columns[0].Caption = "Ticket ID";
            ////                cbLevel2.Columns[0].Width = new Unit("95px");
            ////            }



            ////            if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "CPR" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "OPM" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "LEM" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "CNS")
            ////            {
            ////                cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
            ////                cbLevel2.Columns[1].Width = new Unit("306px");
            ////            }
            ////            else
            ////            {
            ////                cbLevel2.Columns.Add(DatabaseObjects.Columns.Title);
            ////                cbLevel2.Columns[1].Caption = "Project Name";
            ////                cbLevel2.Columns[1].Width = new Unit("306px");
            ////            }




            ////            if (Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "CPR" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "OPM" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "LEM" && Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]) != "CNS")
            ////            {
            ////                cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketStatus);
            ////                cbLevel2.Columns[2].Caption = "Status";
            ////                cbLevel2.DropDownWidth = Unit.Empty;
            ////            }
            ////            else
            ////            {

            ////                resultedTable.Columns.Add("ExpressionDate", typeof(string));

            ////                foreach (DataRow rowitem in resultedTable.Rows)
            ////                {
            ////                    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.PreconStartDate))
            ////                    {
            ////                        if (!string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.EstimatedConstructionStart])))
            ////                        {
            ////                            if (string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.PreconStartDate])))
            ////                                rowitem["ExpressionDate"] = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.EstimatedConstructionStart]).ToString("MMM-dd-yyyy");
            ////                            else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.EstimatedConstructionStart]) < Convert.ToDateTime(rowitem[DatabaseObjects.Columns.PreconStartDate]))
            ////                            {
            ////                                rowitem["ExpressionDate"] = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.EstimatedConstructionStart]).ToString("MMM-dd-yyyy");
            ////                            }
            ////                            else
            ////                            {
            ////                                rowitem["ExpressionDate"] = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.PreconStartDate]).ToString("MMM-dd-yyyy");
            ////                            }

            ////                        }
            ////                        else
            ////                        {
            ////                            if (!string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.PreconStartDate])))
            ////                                rowitem["ExpressionDate"] = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.PreconStartDate]).ToString("MMM-dd-yyyy");
            ////                        }

            ////                    }
            ////                    else
            ////                    {
            ////                        rowitem["ExpressionDate"] = Convert.ToString(rowitem[DatabaseObjects.Columns.TicketActualStartDate]);
            ////                    }
            ////                }

            ////                //if (resultedTable.Columns.Contains(DatabaseObjects.Columns.PreconStartDate))
            ////                //{
            ////                //    cbLevel2.Columns.Add(DatabaseObjects.Columns.PreconStartDate);
            ////                //}
            ////                //else
            ////                //{
            ////                //    cbLevel2.Columns.Add(DatabaseObjects.Columns.TicketActualStartDate);
            ////                //}

            ////                cbLevel2.Columns.Add("ExpressionDate");
            ////                cbLevel2.Columns[2].Caption = "Start Date";
            ////                cbLevel2.Columns[2].Width = new Unit("70px");

            ////                cbLevel2.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd);
            ////                cbLevel2.Columns[3].Caption = "End Date";
            ////                cbLevel2.Columns[3].Width = new Unit("70px");

            ////                // test purpose may be this is user further for calculation.
            ////                cbLevel2.Columns.Add("LevelTitle");
            ////                cbLevel2.Columns[4].Caption = "Ticket ID";
            ////                cbLevel2.Columns[4].Width = new Unit("95px");
            ////                cbLevel2.Columns[4].Visible = false;
            ////            }

            ////        }
            ////        else
            ////        {
            ////            cbLevel2.Columns.Clear();
            ////            cbLevel2.DropDownWidth = cbLevel2.Width;
            ////        }


            ////        if (resultedTable.Columns.Contains(DatabaseObjects.Columns.PreconStartDate) && resultedTable.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
            ////        {
            ////            foreach (DataRow rowItem in resultedTable.Rows)
            ////            {
            ////                if (!string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreconStartDate])))
            ////                    rowItem[DatabaseObjects.Columns.PreconStartDate] = Convert.ToDateTime(rowItem[DatabaseObjects.Columns.PreconStartDate]).ToString("MMM-dd-yyyy");
            ////                if (!string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.EstimatedConstructionEnd])))
            ////                    rowItem[DatabaseObjects.Columns.EstimatedConstructionEnd] = Convert.ToDateTime(rowItem[DatabaseObjects.Columns.EstimatedConstructionEnd]).ToString("MMM-dd-yyyy");
            ////            }
            ////        }

            ////        cbLevel2.DataSource = resultedTable;
            ////        cbLevel2.ValueField = "LevelTitle";
            ////        cbLevel2.ValueType = typeof(string);
            ////        cbLevel2.TextField = "LevelTitle";
            ////        cbLevel2.DataBind();

            ////        if (cbLevel2.Items.Count > 1 && !fromModule)
            ////        {
            ////            // Else add the dummy item on top
            ////            cbLevel2.Items.Insert(0, new ListEditItem("--Select--", ""));
            ////        }
            ////    }


            ////}
        }

        protected void cbLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////if (Request["Mode"] == "RMMProjectAgent")
            ////{

            ////    //string str = cbLevel2.SelectedItem.Value.ToString();
            ////    if (cbLevel2.SelectedItem != null)
            ////    {
            ////        moduleName = UGITUtility.getModuleNameByTicketId(cbLevel2.SelectedItem.Value.ToString());
            ////        spProjectItem = Ticket.getCurrentTicket(moduleName, cbLevel2.SelectedItem.Value.ToString());

            ////        if (moduleName == "CPR" || moduleName == "CNS")
            ////        {
            ////            if (!string.IsNullOrEmpty(Convert.ToString(spProjectItem[DatabaseObjects.Columns.CRMProjectID])))
            ////                cprprojectId = Convert.ToString(spProjectItem[DatabaseObjects.Columns.CRMProjectID]);
            ////            else
            ////                cprprojectId = Convert.ToString(spProjectItem[DatabaseObjects.Columns.EstimateNo]);
            ////        }

            ////        projectTitle = Convert.ToString(spProjectItem[DatabaseObjects.Columns.Title]);


            ////        if (moduleName == "CPR" || moduleName == "CNS")
            ////        {
            ////            if (string.IsNullOrEmpty(cprprojectId))
            ////                cbLevel2.TextFormatString = "{1}";
            ////            else
            ////                cbLevel2.TextFormatString = "{0}: {1}";
            ////        }
            ////        else
            ////            cbLevel2.TextFormatString = "{0}";


            ////        SetDeaultDates();

            ////        ticketID = Convert.ToString(spProjectItem[DatabaseObjects.Columns.TicketId]);


            ////    }


            ////}
        }

        protected void lnkAllocationSave_Click(object sender, EventArgs e)
        {
            bool IsNewLastEntry = false;
            DateTime? tempEndDate = CRMProjectAllocation.AllocationEndDate;
            int selectedAllocationId = 0;

            if (ASPxComboBoxGroupUser.SelectedItem == null)
            {
                lblMessage.Text = "User Required.";
                lblMessage.Visible = true;
                return;
            }
            
            if (startDate.Date > endDate.Date)
            {
                lblMessage.Text = "End Date must be greater then Start Date";
                lblMessage.Visible = true;
                return;
            }

            if (txtCPRDuration.Text == "0" || txtCPRDuration.Text == null || txtCPRDuration.Text == string.Empty)
            {
                txtCPRDuration.Text = GetDurationInWeeks(startDate.Date, endDate.Date);
            }
            

            UserProfile = UserProfileManager.GetUserById(ASPxComboBoxGroupUser.SelectedItem.Value.ToString().Trim());
            String query = "";

            if (Id > 0)
            {
                query = string.Format("{0}='{1}' and {2} = '{3}' and {4} = '{5}' and {6} = '{7}' ",
                    DatabaseObjects.Columns.UGITAssignedTo, UserProfile.Id, DatabaseObjects.Columns.Id, Id, DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.Type, ddlUserType.Value.ToString());
            }
            else
            {
                query = string.Format("{0}='{1}' and {2} = '{3}' and {4} = '{5}'",
                    DatabaseObjects.Columns.UGITAssignedTo, UserProfile.Id, DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.Type, ddlUserType.Value.ToString());
                CRMProjectAllocation.ID = 0;
            }

            DataTable dtCPRAllocation;
            dtCPRAllocation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMProjectAllocation, $"{query.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);

            if (dtCPRAllocation.Rows.Count == 0)
            {
                string query2 = string.Format("{0}='{1}' and {2} = '{3}' and {4} = '{5}'",
               DatabaseObjects.Columns.UGITAssignedTo, UserProfile.Id, DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.Type, ddlUserType.Value.ToString());

                dtCPRAllocation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMProjectAllocation, $"{query2.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                
            }

            if (dtCPRAllocation != null && dtCPRAllocation.Rows.Count > 0)
            {
                string query1 = string.Format("({0}<='{2}' AND {1}>='{2}') OR ({0}<='{3}' AND {1}>='{3}') OR ({0}>='{2}' AND {1}<='{3}') AND ({4} <> '{5}')", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate, startDate.Date, endDate.Date, DatabaseObjects.Columns.ID, Id);
                DataRow[] row = dtCPRAllocation.Select(query1);

                var editId = "";
                foreach (var item in row)
                {
                    editId = item[DatabaseObjects.Columns.ID].ToString();
                    if (editId != Id.ToString())
                    {   
                        if (row != null && row.Length > 0)
                        {
                            lblMessage.Text = Constants.ErrorMsgRMMOverlappingDates;
                            lblMessage.Visible = true;
                            return;
                        }
                    }

                }
            }


            //////condition for reallocation
            if (Request["ModeType"] == "ReAllocation")
            {
                if (Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.AllocationStartDate]) != startDate.Date || Convert.ToDateTime(spListItem.Rows[0][DatabaseObjects.Columns.AllocationEndDate]) != endDate.Date)
                {
                    if (tempEndDate > endDate.Date)
                    {
                        IsNewLastEntry = true;
                        selectedAllocationId = Convert.ToInt32(spListItem.Rows[0][DatabaseObjects.Columns.Id]);
                    }
                    
                    if (startDate.Date != null)
                    {
                        CRMProjectAllocation.AllocationEndDate = startDate.Date;
                    }
                    else
                    {
                        CRMProjectAllocation.AllocationEndDate = DateTime.Now;
                    }
                    
                    CRMProjectAllocationManager.Update(CRMProjectAllocation);
                    CRMProjectAllocation = new ProjectEstimatedAllocation();
                    
                }
            }
            
            CRMProjectAllocation.TicketId = ticketID;

            if (string.IsNullOrEmpty(txtAlloc.Text.Trim()))
            {
                double DefaultPercentageAllocation = UGITUtility.StringToDouble(objConfigurationVariableHelper.GetValue(ConfigConstants.DefaultPercentageAllocation));
                txtAlloc.Text = Convert.ToString(DefaultPercentageAllocation);
            }
            
            CRMProjectAllocation.PctAllocation = UGITUtility.StringToInt(txtAlloc.Text.Trim());
            CRMProjectAllocation.AssignedTo = ASPxComboBoxGroupUser.SelectedItem.Value.ToString(); 
            CRMProjectAllocation.AssignedTo = ASPxComboBoxGroupUser.Value.ToString();
            CRMProjectAllocation.Type = ddlUserType.Value.ToString();
            CRMProjectAllocation.Title = txtSubProject.Text;
            
            if (startDate.Date != null)
            {
                CRMProjectAllocation.AllocationStartDate = startDate.Date;
            }
            
            if (endDate.Date != null)
            {
                CRMProjectAllocation.AllocationEndDate = endDate.Date;
            }


            if (!string.IsNullOrEmpty(txtCPRDuration.Text))
                CRMProjectAllocation.Duration = Convert.ToInt32(txtCPRDuration.Text);
            else
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate.Date, endDate.Date);
                int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);
                CRMProjectAllocation.Duration = noOfWeeks;
            }
            
            CRMProjectAllocationManager.Update(CRMProjectAllocation);

            List<UserWithPercentage> lstUserWithPercetage = UpdateProjectWithTeamMembers(CRMProjectAllocation);
            //here is block for update allocation in case of reallocation.
            if (Request["ModeType"] == "ReAllocation")
            {
                UserProfile users = UserProfileManager.LoadById(CRMProjectAllocation.AssignedTo);
                ProjectEstimatedAllocation newTempspListItem = new ProjectEstimatedAllocation();
                newTempspListItem = CRMProjectAllocationManager.LoadByID(Id);
                
                UserProfile assignToUser = UserProfileManager.LoadById(newTempspListItem.AssignedTo);
                lstUserWithPercetage.Add(new UserWithPercentage() { UserId = assignToUser.Id, Percentage = Convert.ToInt16(txtAlloc.Text), StartDate = Convert.ToDateTime(newTempspListItem.AllocationStartDate), EndDate = startDate.Date, RoleTitle = ddlUserType.Text.ToString() });
                lstUserWithPercetage.Add(new UserWithPercentage() { UserId = users.Id, Percentage = Convert.ToInt16(txtAlloc.Text), StartDate = startDate.Date, EndDate = endDate.Date, RoleTitle = ddlUserType.Text.ToString() });

                if (IsNewLastEntry && selectedAllocationId > 0)
                {;
                    ProjectEstimatedAllocation newTempLastspListItem = new ProjectEstimatedAllocation();
                    newTempLastspListItem = CRMProjectAllocationManager.LoadByID(selectedAllocationId);
                    ProjectEstimatedAllocation CRMProjectAllocation = new ProjectEstimatedAllocation();
                    CRMProjectAllocation.TicketId = ticketID;
                    CRMProjectAllocation.AssignedTo = newTempLastspListItem.AssignedTo;
                    CRMProjectAllocation.PctAllocation = newTempLastspListItem.PctAllocation;
                    CRMProjectAllocation.Type = newTempLastspListItem.Type;
                    CRMProjectAllocation.Title = newTempLastspListItem.Title;
                    CRMProjectAllocation.Duration = newTempLastspListItem.Duration;
                    CRMProjectAllocation.AllocationStartDate = endDate.Date;
                    CRMProjectAllocation.AllocationEndDate = tempEndDate;

                    CRMProjectAllocationManager.Update(CRMProjectAllocation);
                    
                    lstUserWithPercetage.Add(new UserWithPercentage() { UserId = CRMProjectAllocation.AssignedTo, Percentage = CRMProjectAllocation.PctAllocation, StartDate = (DateTime)CRMProjectAllocation.AllocationStartDate, EndDate = (DateTime)CRMProjectAllocation.AllocationEndDate, RoleTitle = ddlUserType.Text.ToString() });
                }
            }


            if (CRMProjectAllocation != null)
            {
                ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(_context);
                bool autoCreateRMMProjectAllocation = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                if (autoCreateRMMProjectAllocation)
                {;
                    if (lstUserWithPercetage != null && lstUserWithPercetage.Count > 0)
                    {
                        var taskManager = new UGITTaskManager(_context);
                        List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(ticketID), ticketID);
                        List<string> lstUsers = lstUserWithPercetage.Select(a => a.UserId).ToList();
                        var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                        // Only create allocation enties if user is not in schedule
                        if (res == null || res.Count == 0)
                        {
                            ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(ticketID), ticketID, lstUserWithPercetage); };
                            Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                            sThreadStartMethodUpdateCPRProjectAllocation.Start();
                        }
                    }
                }
            }
            

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        //private void addUserInGroup()
        //{
        //    using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
        //    using (SPWeb spweb = spSite.OpenWeb())
        //    {
        //        SPFieldUserValueCollection spUsers = GetSelectedUsers(spweb);

        //        spweb.AllowUnsafeUpdates = true;
        //        SPGroup userGroup = UserProfile.GetGroupByName(ddlUserType.Value.ToString(), spweb);
        //        userGroup.AddUser(spUsers[0].User);
        //        userGroup.Update();
        //        spweb.AllowUnsafeUpdates = false;
        //    }
        //}

        protected void ASPxComboBoxGroupUser_Load(object sender, EventArgs e)
        {
            BindUserGroup();
        }

        private void BindUserGroup()
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("Id"))
                dt.Columns.Add("Id");

            if (!dt.Columns.Contains("Name"))
                dt.Columns.Add("Name");

            if (ddlUserType.SelectedItem != null)
            {

                //List<UserProfile> userList = UserProfile.GetGroupUsers(ddlUserType.SelectedItem.Text.Trim(), SPContext.Current.Web);
                ////List<Role> lstUsers = new List<Role>();
                //// lstUsers =  UserProfileManager.GetUserGroupById(ddlUserType.SelectedItem.Value.ToString());
                List<UserProfile> lstUProfile;
                lstUProfile = UserProfileManager.GetUsersByGroupID(ddlUserType.SelectedItem.Value.ToString());

                foreach (UserProfile user in lstUProfile)
                {
                    dt.Rows.Add(new object[] { user.Id, user.Name });
                }
            }

            ASPxComboBoxGroupUser.DataSource = dt;
            ASPxComboBoxGroupUser.DataBind();
        }

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUserGroup();

            ASPxComboBoxGroupUser.Text = "";
        }

        public static string GetEndDateByWeeksProject(String startDate, int noOfWeeks)
        {
            try
            {

                DateTime startDateNew = DateTime.ParseExact(startDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                int noOfWorkingDays = uHelper.GetWorkingDaysInWeeks(HttpContext.Current.GetManagerContext(), noOfWeeks);
                DateTime[] calculatedDates = uHelper.GetEndDateByWorkingDays(HttpContext.Current.GetManagerContext(), startDateNew, noOfWorkingDays);
                if (calculatedDates != null && calculatedDates.Length > 1)
                {
                    string duration = CalculateDuration(calculatedDates[0], calculatedDates[1]);

                    return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("MM/dd/yyyy").Replace("-", "/"), calculatedDates[1].ToString("MM/dd/yyyy").Replace("-", "/"), "{", "}", duration);
                    //return string.Format("{2}\"messagecode\":\"2\",\"message\":\"success\",\"startdate\":\"{0}\",\"enddate\":\"{1}\",\"duration\":\"{4}\"{3}", calculatedDates[0].ToString("yyyy-MM-dd"), calculatedDates[1].ToString("yyyy-MM-dd"), "{", "}", duration);
                }
                else
                {
                    return "{\"messagecode\":\"0\",\"message\":\"error\"}";
                }
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }

        private static string CalculateDuration(DateTime startDate, DateTime endDate)
        {
            string duration = string.Empty;
            try
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startDate, endDate);
                int week = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                if (week > 0)
                {
                    duration = string.Format("{0} week(s)", week);

                }
                else
                {
                    duration = string.Format("{0} days(s)", noOfWorkingDays);
                }
            }
            catch
            {

            }
            return duration;
        }

        public static string GetDurationInWeeks(DateTime startDate, DateTime endDate)
        {
            try
            {
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startDate, endDate);
                int noOfWeeks = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                return string.Format(noOfWeeks.ToString());
            }
            catch
            {
                return "{\"messagecode\":\"0\",\"message\":\"error\"}";
            }
        }
    }
}