using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class AddCheckListRole : UserControl
    {
        public string CheckListId { get; set; }
        public string CheckListRoleId { get; set; }

        DataRow splstCheckListRoleItem;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListsManager checkListsManager = null;
        CheckListTasksManager checkListTasksManager = null;
        CheckListRolesManager checkListRolesManager = null;
        CheckListTaskStatusManager checkListTaskStatusManager = null;
        CheckListTaskTemplatesManager checkListTaskTemplatesManager = null;
        CheckListRoleTemplatesManager checkListRoleTemplatesManager = null;
        TicketManager ticketManager = null;
        ModuleViewManager moduleViewManager = null;
        UserProfileManager userProfileMgr = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            checkListsManager = new CheckListsManager(context);
            checkListTasksManager = new CheckListTasksManager(context);
            checkListRolesManager = new CheckListRolesManager(context);
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);
            checkListTaskTemplatesManager = new CheckListTaskTemplatesManager(context);
            checkListRoleTemplatesManager = new CheckListRoleTemplatesManager(context);
            ticketManager = new TicketManager(context);
            moduleViewManager = new ModuleViewManager(context);
            userProfileMgr = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (!IsPostBack)
                FillContact(0, -1, string.Empty);

            

            //peUserTo.UserTokenBoxAdd.GridViewProperties.Settings.VerticalScrollableHeight = 100;
            //peUserTo.UserTokenBoxAdd.GridViewStyles.FilterRow.CssClass = "userValueBox-filterRow";
            //peUserTo.UserTokenBoxAdd.GridViewStyles.StatusBar.CssClass = "userValueBox-footerCloseBtn";
            //peUserTo.UserTokenBoxAdd.GridView.CssClass = "userValueBox-grid";
            if (Convert.ToInt64(CheckListRoleId) > 0)
            {
                if (Request["IsTemplate"] == "true")
                {
                    //splstCheckListRoleItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListRoleTemplates, Convert.ToInt64(CheckListRoleId));
                    splstCheckListRoleItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoleTemplates, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListRoleId)}").Select()[0];
                    rbtnSubContractor.Visible = false;
                }
                else
                {
                    //splstCheckListRoleItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListRoles, Convert.ToInt64(CheckListRoleId));
                    splstCheckListRoleItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoles, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListRoleId)}").Select()[0];
                    rbtnSubContractor.Visible = true;
                }

                if (!IsPostBack)
                {
                    txtRoleName.Text = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Title]);

                    if (Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Type]) == "UserField")
                    {
                        rbtnUserField.Checked = true;

                    }
                    else if (Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Type]) == "Contact")
                    {
                        rbtnContact.Checked = true;
                    }
                    else
                    {
                        rbtnTextField.Checked = true;
                    }
                                                          
                    if (rbtnUserField.Checked)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress])))
                        {
                            //SPFieldUserValueCollection userLookups = new SPFieldUserValueCollection();
                            //userLookups.Add(new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress])));
                            //if (userLookups != null)
                            //{
                            //    peUserTo.CommaSeparatedAccounts = UserProfile.CommaSeparatedAccountsFrom(userLookups, ",");
                            //}
                            var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();                            
                            UserProfile user = manager.FindByEmail(Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]));
                            if (user != null && !string.IsNullOrEmpty(user.Id))
                            {
                                peUserTo.SetValues(user.Id);
                            }                            
                        }
                    }
                    else if (rbtnContact.Checked)
                    {
                        FillContact(0, Convert.ToInt64(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]), string.Empty);
                        cmbContact.Value = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]);
                    }
                    else
                    {
                        txtEmailAddress.Text = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]);
                    }
                }



                if (rbtnSubContractor.Checked)
                {
                    trUserField.Visible = false;
                    trEmail.Visible = false;
                    trContact.Visible = false;
                    trRole.Visible = false;
                }
                else if (rbtnUserField.Checked)
                {
                    trUserField.Visible = true;
                    trEmail.Visible = false;
                    trContact.Visible = false;
                    trRole.Visible = true;
                }
                else if (rbtnContact.Checked)
                {
                    trUserField.Visible = false;
                    trEmail.Visible = false;
                    trContact.Visible = true;
                    trRole.Visible = true;
                }
                else
                {
                    trUserField.Visible = false;
                    trEmail.Visible = true;
                    trContact.Visible = false;
                    trRole.Visible = true;
                }
                lnkDeleteCheckListRole.Visible = true;
            }
            else
            {
                lnkDeleteCheckListRole.Visible = false;
                if (Request["IsTemplate"] == "true")
                {
                    //SPList lstCheckListTaskTemplates = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListRoleTemplates);
                    //splstCheckListRoleItem = lstCheckListTaskTemplates.Items.Add();

                    //Just need Table structure, with out any data
                    DataTable lstCheckListTaskTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoleTemplates, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    splstCheckListRoleItem = lstCheckListTaskTemplates.NewRow();

                    rbtnSubContractor.Visible = false;
                }
                else
                {
                    //SPList lstCheckListTaskTemplates = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListRoles);
                    //splstCheckListRoleItem = lstCheckListTaskTemplates.Items.Add();

                    //Just need Table structure, with out any data
                    DataTable lstCheckListTaskTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoles, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); //SPListHelper.GetSPList(DatabaseObjects.Tables.CheckLists);
                    splstCheckListRoleItem = lstCheckListTaskTemplates.NewRow();

                    rbtnSubContractor.Visible = true;
                }

                if (rbtnSubContractor.Checked)
                {
                    trUserField.Visible = false;
                    trEmail.Visible = false;
                    trContact.Visible = false;
                    trRole.Visible = false;
                }
                else if (rbtnUserField.Checked)
                {
                    trUserField.Visible = true;
                    trEmail.Visible = false;
                    trContact.Visible = false;
                    trRole.Visible = true;
                }
                else if (rbtnContact.Checked)
                {
                    trUserField.Visible = false;
                    trEmail.Visible = false;
                    trContact.Visible = true;
                    trRole.Visible = true;
                }
                else
                {
                    trUserField.Visible = false;
                    trEmail.Visible = true;
                    trContact.Visible = false;
                    trRole.Visible = true;
                }

                if (!IsPostBack)
                    rbtnTextField.Checked = true;

            }

        }

        protected void lnkDeleteCheckListRole_Click(object sender, EventArgs e)
        {
            //splstCheckListRoleItem.Delete();
            if (Request["IsTemplate"] == "true")
            {
                CheckListRoleTemplates checkListRoleTemplates = checkListRoleTemplatesManager.LoadByID(Convert.ToInt64(splstCheckListRoleItem[DatabaseObjects.Columns.ID]));
                checkListRoleTemplatesManager.Delete(checkListRoleTemplates);
            }
            else if (Request["IsTemplate"] == "false")
            {
                // Delete all entry from CheckListStatus for that particular Role.
                /*
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE' /><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId, DatabaseObjects.Columns.CheckListRoleLookup, CheckListRoleId);
                query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.UGITCheckListTaskStatus, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListLookup);
                query.ViewFieldsOnly = true;
                SPListItemCollection spColCheckListStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskStatus, query);
                */

                string query = $"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.CheckListRoleLookup} = {CheckListRoleId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable spColCheckListStatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskStatus, query);

                List<long> Ids = new List<long>();
                foreach (DataRow item in spColCheckListStatus.Rows)
                {
                    Ids.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                }

                if (Ids.Count > 0)
                {
                    //RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                    RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListTaskStatus);
                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnSaveCheckListRole_Click(object sender, EventArgs e)
        {

            if (rbtnSubContractor.Checked)
            {
                /*
                SPQuery querySubContractor = new SPQuery();
                querySubContractor.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, Request["ticketId"]);
                DataTable dtProjectSubContractor = SPListHelper.GetDataTable(DatabaseObjects.Tables.SubContractor, querySubContractor);
                */

                string querySubContractor = $"{DatabaseObjects.Columns.TicketId} = '{Convert.ToString(Request["ticketId"])}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable dtProjectSubContractor = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, querySubContractor);

                if (dtProjectSubContractor != null && dtProjectSubContractor.Rows.Count > 0)
                {
                    //SPListItem spCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckLists, Convert.ToInt64(CheckListId));
                    CheckLists spCheckListItem = checkListsManager.LoadByID(Convert.ToInt64(CheckListId));

                    //must have atleast one task in checklist before create role.
                    /*
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                    DataTable dtCheckListTask = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTasks, query);
                    */
                    
                    int checkListTaskCount = checkListTasksManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId}").Count();

                    /*
                    if (dtCheckListTask == null || dtCheckListTask.Rows.Count < 1)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "You must have atleast one task.";
                        return;
                    }
                    */

                    if (checkListTaskCount < 1)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "You must have atleast one task.";
                        return;
                    }


                    //delete old row..
                    /*
                    SPQuery queryoldRole = new SPQuery();

                    queryoldRole.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketId, Request["ticketId"], DatabaseObjects.Columns.CheckListLookup, CheckListId);

                    SPListItemCollection splstColOldRole = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoles, queryoldRole);
                    */

                    List<CheckListRoles> spColCheckListRoles = checkListRolesManager.Load($"{DatabaseObjects.Columns.TicketId} = '{Convert.ToString(Request["ticketId"])}' and {DatabaseObjects.Columns.CheckListLookup} = {CheckListId}");

                    //if (splstColOldRole != null && splstColOldRole.Count > 0)
                    if (spColCheckListRoles != null && spColCheckListRoles.Count > 0)
                    {
                        #region delete role old entry
                        // Delete all entry from CheckListRole.
                        /*
                        SPQuery queryRole = new SPQuery();
                        queryRole.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketId, Request["ticketId"], DatabaseObjects.Columns.CheckListLookup, CheckListId);
                        queryRole.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
                        queryRole.ViewFieldsOnly = true;
                        //SPListItemCollection spColCheckListRoles = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoles, queryRole);
                        */

                        List<long> RoleIds = new List<long>();
                        foreach (var item in spColCheckListRoles)
                        {
                            RoleIds.Add(item.ID);
                        }

                        if (RoleIds.Count > 0)
                        {
                            //RMMSummaryHelper.BatchDeleteListItems(RoleIds, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                            RMMSummaryHelper.BatchDeleteListItems(context, RoleIds, DatabaseObjects.Tables.CheckListTaskStatus);
                        }
                        #endregion

                        #region delete checklist status
                        // Delete all entry from CheckListStatus 
                        /*
                        SPQuery querystatus = new SPQuery();
                        querystatus.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId, DatabaseObjects.Columns.TicketId, Request["ticketId"]);
                        querystatus.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.UGITCheckListTaskStatus, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListLookup);
                        querystatus.ViewFieldsOnly = true;
                        SPListItemCollection spColCheckListStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskStatus, querystatus);
                        */

                        List<CheckListTaskStatus> spColCheckListStatus = checkListTaskStatusManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.TicketId} = '{Request["ticketId"]}'");

                        List<long> Ids = new List<long>();
                        foreach (var item in spColCheckListStatus)
                        {
                            Ids.Add(item.ID);
                        }

                        if (Ids.Count > 0)
                        {
                            //RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                            RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListTaskStatus);
                        }
                        #endregion
                    }

                    foreach (DataRow rowitem in dtProjectSubContractor.Rows)
                    {
                        /*
                        SPList lstCheckListRoles = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListRoles);
                        SPListItem newsplstCheckListRoleItem = lstCheckListRoles.Items.Add();
                        newsplstCheckListRoleItem[DatabaseObjects.Columns.Module] = spCheckListItem != null ? spCheckListItem.Module : string.Empty; //spCheckListItem[DatabaseObjects.Columns.Module];
                        newsplstCheckListRoleItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;
                        
                        newsplstCheckListRoleItem[DatabaseObjects.Columns.Type] = "SubContractor";

                        string roletitle = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.Title])) && !string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName])))
                            roletitle = Convert.ToString(rowitem[DatabaseObjects.Columns.Title]) + "," + Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName]);
                        else
                            roletitle = Convert.ToString(rowitem[DatabaseObjects.Columns.Title]) + Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName]);

                        newsplstCheckListRoleItem[DatabaseObjects.Columns.Title] = roletitle;
                        newsplstCheckListRoleItem.Update();
                        */

                        CheckListRoles newsplstCheckListRoleItem = new CheckListRoles();
                        newsplstCheckListRoleItem.Module = spCheckListItem != null ? spCheckListItem.Module : string.Empty;
                        newsplstCheckListRoleItem.CheckListLookup = Convert.ToInt64(CheckListId);
                        newsplstCheckListRoleItem.Type = "SubContractor";

                        string roletitle = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.Title])) && !string.IsNullOrEmpty(Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName])))
                            roletitle = Convert.ToString(rowitem[DatabaseObjects.Columns.Title]) + "," + Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName]);
                        else
                            roletitle = Convert.ToString(rowitem[DatabaseObjects.Columns.Title]) + Convert.ToString(rowitem[DatabaseObjects.Columns.CompanyName]);

                        newsplstCheckListRoleItem.Title = roletitle;

                        if (newsplstCheckListRoleItem.ID <= 0)
                            checkListRolesManager.Insert(newsplstCheckListRoleItem);
                        else
                            checkListRolesManager.Update(newsplstCheckListRoleItem);

                        /*
                        SPQuery queryCheckListStatus = new SPQuery();
                        queryCheckListStatus.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                        DataTable newdtCheckListTask = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTasks, queryCheckListStatus);
                        if (newdtCheckListTask != null && newdtCheckListTask.Rows.Count > 0)
                        {
                            SPList lstCheckListTaskStatus = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskStatus);
                            foreach (DataRow checkListTaskRowItem in dtCheckListTask.Rows)
                            {
                                SPListItem checkListTaskStatusItem = lstCheckListTaskStatus.Items.Add();
                                checkListTaskStatusItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];
                                checkListTaskStatusItem[DatabaseObjects.Columns.UGITCheckListTaskStatus] = "NC";
                                checkListTaskStatusItem[DatabaseObjects.Columns.CheckListRoleLookup] = newsplstCheckListRoleItem.ID;
                                checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskLookup] = checkListTaskRowItem[DatabaseObjects.Columns.Id];
                                checkListTaskStatusItem[DatabaseObjects.Columns.Module] = uHelper.getModuleIdByTicketID(Request["ticketId"]);
                                checkListTaskStatusItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                                checkListTaskStatusItem.Update();
                            }
                        }
                        */

                        List<CheckListTasks> newdtCheckListTask = checkListTasksManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId}");

                        if(newdtCheckListTask != null && newdtCheckListTask.Count > 0)
                        {
                            foreach (CheckListTasks checkListTaskRowItem in newdtCheckListTask)
                            {
                                CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                                checkListTaskStatusItem.TicketId = Request["ticketId"];
                                checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                                checkListTaskStatusItem.CheckListRoleLookup = newsplstCheckListRoleItem.ID;
                                checkListTaskStatusItem.CheckListTaskLookup = checkListTaskRowItem.ID;
                                checkListTaskStatusItem.Module = uHelper.getModuleNameByTicketId(Request["ticketId"]);
                                checkListTaskStatusItem.CheckListLookup = Convert.ToInt64(CheckListId);

                                checkListTaskStatusManager.Insert(checkListTaskStatusItem);
                            }
                        }
                    }
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "no subcontractor found.";
                    return;
                }
            }
            else
            {

                if (Request["IsTemplate"] == "true")
                {
                    //SPListItem spCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTemplates, Convert.ToInt64(CheckListId));
                    DataRow spCheckListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTemplates, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListId)}").Select()[0];
                    splstCheckListRoleItem[DatabaseObjects.Columns.Module] = spCheckListItem[DatabaseObjects.Columns.Module];
                    splstCheckListRoleItem[DatabaseObjects.Columns.CheckListTemplateLookup] = CheckListId;


                    //must have atleast one task in checklist before create role.
                    /*
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListTemplateLookup, CheckListId);
                    DataTable dtCheckListTaskTemplate = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTaskTemplates, query);
                    */

                    int CheckListTaskTemplateCount = checkListTaskTemplatesManager.Load($"{DatabaseObjects.Columns.CheckListTemplateLookup} = {CheckListId}").Count();

                    //if (dtCheckListTaskTemplate == null || dtCheckListTaskTemplate.Rows.Count < 1)
                    if (CheckListTaskTemplateCount < 1)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "You must have atleast one task.";
                        return;
                    }

                }
                else
                {
                    //SPListItem spCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckLists, Convert.ToInt64(CheckListId));
                    CheckLists spCheckListItem = checkListsManager.LoadByID(Convert.ToInt64(CheckListId));

                    if(spCheckListItem != null)
                        splstCheckListRoleItem[DatabaseObjects.Columns.Module] = spCheckListItem.Module;

                    splstCheckListRoleItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];

                    splstCheckListRoleItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                    //must have atleast one task in checklist before create role.
                    /*
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                    DataTable dtCheckListTask = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTasks, query);
                    */

                    int CheckListTaskCount = checkListTasksManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId}").Count();
                    //if (dtCheckListTask == null || dtCheckListTask.Rows.Count < 1)
                    if(CheckListTaskCount < 1)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "You must have atleast one task.";
                        return;
                    }
                }


                //if (trUserField.Visible)
                if (rbtnUserField.Checked)
                {
                    /*                
                    List<string> assignedUsers = new List<string>();
                    SPFieldUserValueCollection userMultiLookup = new SPFieldUserValueCollection();
                    if (peUserTo.Accounts.Count > 1)
                        return;
                    if (peUserTo.Accounts.Count > 0)
                    {
                        for (int i = 0; i < peUserTo.Accounts.Count; i++)
                        {
                            PickerEntity entity = (PickerEntity)peUserTo.Entities[i];
                            if (entity != null && entity.Key != null)
                            {
                                SPUser user = UserProfile.GetUserByName(entity.Key, SPPrincipalType.User);
                                if (user != null)
                                {
                                    SPFieldUserValue userLookup = new SPFieldUserValue();
                                    userLookup.LookupId = user.ID;
                                    userMultiLookup.Add(userLookup);
                                }
                            }
                        }
                    }
                    */

                    splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress] = userProfileMgr.GetUserEmailId(peUserTo.GetValues());
                    splstCheckListRoleItem[DatabaseObjects.Columns.Type] = "UserField";
                }
                else if (rbtnContact.Checked)
                {
                    splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress] = cmbContact.Value;
                    splstCheckListRoleItem[DatabaseObjects.Columns.Type] = "Contact";
                }
                else
                {
                    splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress] = txtEmailAddress.Text;
                }

                splstCheckListRoleItem[DatabaseObjects.Columns.Title] = txtRoleName.Text;

                //splstCheckListRoleItem.Update();
                long Id = 0;
                long.TryParse(Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.ID]), out Id);
                if (Request["IsTemplate"] == "true")
                {
                    CheckListRoleTemplates checkListRoleTemplates = new CheckListRoleTemplates();

                    checkListRoleTemplates.ID = Id;
                    checkListRoleTemplates.Title = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Title]);
                    checkListRoleTemplates.TicketId = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.TicketId]);
                    checkListRoleTemplates.CheckListTemplateLookup = Convert.ToInt64(splstCheckListRoleItem[DatabaseObjects.Columns.CheckListTemplateLookup]);
                    checkListRoleTemplates.Module = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Module]);
                    checkListRoleTemplates.EmailAddress = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]);
                    checkListRoleTemplates.Type = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Type]);

                    if(Id <= 0)
                        checkListRoleTemplatesManager.Insert(checkListRoleTemplates);
                    else
                        checkListRoleTemplatesManager.Update(checkListRoleTemplates);

                    Id = checkListRoleTemplates.ID;
                }
                else
                {
                    CheckListRoles checkListRoles = new CheckListRoles();
                    checkListRoles.ID = Id;
                    checkListRoles.Title = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Title]);
                    checkListRoles.TicketId = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.TicketId]);
                    checkListRoles.EmailAddress = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress]); 
                    checkListRoles.Module = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Module]);
                    checkListRoles.CheckListLookup = Convert.ToInt64(splstCheckListRoleItem[DatabaseObjects.Columns.CheckListLookup]);
                    checkListRoles.Type = Convert.ToString(splstCheckListRoleItem[DatabaseObjects.Columns.Type]);

                    if (Id <= 0)
                        checkListRolesManager.Insert(checkListRoles);
                    else
                        checkListRolesManager.Update(checkListRoles);

                    Id = checkListRoles.ID;
                }


                if (Request["IsTemplate"] == "false" && Convert.ToInt64(CheckListRoleId) == 0)
                {
                    /*
                    SPQuery query = new SPQuery();
                    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                    DataTable dtCheckListTask = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTasks, query);
                    */

                    List<CheckListTasks> dtCheckListTask = checkListTasksManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId}");

                    if (dtCheckListTask != null && dtCheckListTask.Count > 0)
                    {
                        //SPList lstCheckListTaskStatus = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskStatus);
                        foreach (CheckListTasks checkListTaskRowItem in dtCheckListTask)
                        {
                            /*
                            SPListItem checkListTaskStatusItem = lstCheckListTaskStatus.Items.Add();
                            checkListTaskStatusItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];
                            checkListTaskStatusItem[DatabaseObjects.Columns.UGITCheckListTaskStatus] = "NC";

                            checkListTaskStatusItem[DatabaseObjects.Columns.CheckListRoleLookup] = splstCheckListRoleItem.ID;
                            checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskLookup] = checkListTaskRowItem[DatabaseObjects.Columns.Id];
                            checkListTaskStatusItem[DatabaseObjects.Columns.Module] = uHelper.getModuleIdByTicketID(Request["ticketId"]);
                            checkListTaskStatusItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                            checkListTaskStatusItem.Update();
                            */
                            CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                            checkListTaskStatusItem.TicketId = Request["ticketId"];
                            checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                            checkListTaskStatusItem.Module = uHelper.getModuleNameByTicketId(Request["ticketId"]);
                            checkListTaskStatusItem.CheckListLookup = Convert.ToInt64(CheckListId);
                            checkListTaskStatusItem.CheckListRoleLookup = Id;
                            checkListTaskStatusItem.CheckListTaskLookup = checkListTaskRowItem.ID;
                            checkListTaskStatusManager.Insert(checkListTaskStatusItem);
                        }
                    }


                    //delete default or blank row..
                    /*
                    SPQuery querydefaultRole = new SPQuery();

                    querydefaultRole.Query = string.Format("<Where><And><IsNull><FieldRef Name='{0}'/></IsNull><And><Eq><FieldRef Name='{1}'/><Value Type='Text'>{2}</Value></Eq><Eq><FieldRef Name='{3}' LookupId='TRUE'/><Value Type='Lookup'>{4}</Value></Eq></And></And></Where>", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketId, Request["ticketId"], DatabaseObjects.Columns.CheckListLookup, CheckListId);

                    //DataTable dtCheckListDefaultRole = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListRoles, query);
                    SPListItemCollection splstColRoledefault = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoles, querydefaultRole);
                    */
                    List<CheckListRoles> splstColRoledefault = checkListRolesManager.Load($"({DatabaseObjects.Columns.Title} is null or {DatabaseObjects.Columns.Title} = '' ) and {DatabaseObjects.Columns.TicketId} = '{Request["ticketId"]}' and {DatabaseObjects.Columns.CheckListLookup} = {CheckListId}");
                    
                    if (splstColRoledefault != null && splstColRoledefault.Count > 0)
                    {

                        // Delete all entry from CheckListStatus for that particular Role.
                        /*
                        SPQuery querystatus = new SPQuery();
                        querystatus.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE' /><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId, DatabaseObjects.Columns.CheckListRoleLookup, uHelper.StringToInt(Convert.ToString(splstColRoledefault[0][DatabaseObjects.Columns.Id])));
                        querystatus.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.UGITCheckListTaskStatus, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListLookup);
                        querystatus.ViewFieldsOnly = true;
                        SPListItemCollection spColCheckListStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskStatus, querystatus);
                        */

                        List<CheckListTaskStatus> spColCheckListStatus = checkListTaskStatusManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = '{CheckListId}' and {DatabaseObjects.Columns.CheckListRoleLookup} = {splstColRoledefault[0].ID}");

                        List<long> Ids = new List<long>();
                        foreach (CheckListTaskStatus item in spColCheckListStatus)
                        {
                            Ids.Add(item.ID);
                        }

                        if (Ids.Count > 0)
                        {
                            //RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                            RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListTaskStatus);
                        }

                        //splstColRoledefault[0].Delete();
                        checkListRolesManager.Delete(splstColRoledefault[0]);
                    }

                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        void FillContact(int startIndex, long value, string filter)
        {

            //DataTable dtContact = uGITCache.ModuleDataCache.GetOpenTickets(uHelper.getModuleIdByModuleName("CON"));
            UGITModule module = moduleViewManager.LoadByName("CON");
            DataTable dtContact = ticketManager.GetOpenTickets(module);
            if (dtContact != null)
            {
                DataRow selectRow = null;
                // due to crash put it here.
                selectRow = dtContact.AsEnumerable().FirstOrDefault(dr => dr.Field<long>(DatabaseObjects.Columns.ID) == value);


                DataRow[] drs = null;

                if (Request["ticketId"] != null)
                {
                    //SPListItem spCompanyItem = Ticket.getCurrentTicket(uHelper.getModuleNameByTicketId(Request["ticketId"]), Request["ticketId"]);
                    DataRow spCompanyItem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(Request["ticketId"]), Request["ticketId"]);
                    if (spCompanyItem != null && spCompanyItem[DatabaseObjects.Columns.CRMCompanyLookup] != null)
                    {
                        if (string.IsNullOrEmpty(filter))
                            drs = dtContact.AsEnumerable().Where(dr => !dr.IsNull(DatabaseObjects.Columns.CRMCompanyLookup) && dr.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup) == Convert.ToString(spCompanyItem[DatabaseObjects.Columns.CRMCompanyLookup])).ToArray().Cast<System.Data.DataRow>().Skip(startIndex).Take(20).ToArray(); //.Select(string.Format("{0} > {1}  Or {0} = {2}", DatabaseObjects.Columns.Id, startId,value))
                        else
                            drs = dtContact.AsEnumerable().Where(dr => !dr.IsNull(DatabaseObjects.Columns.Title) && dr.Field<string>(DatabaseObjects.Columns.Title).ToLower().Contains(filter.ToLower()) && !dr.IsNull(DatabaseObjects.Columns.CRMCompanyLookup) && dr.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup) == Convert.ToString(spCompanyItem[DatabaseObjects.Columns.CRMCompanyLookup])).ToArray();

                        // drs = dtContact.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CRMCompanyTitleLookup, Convert.ToString(spCompanyItem[DatabaseObjects.Columns.Title]))).ToArray();
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(filter))
                        drs = dtContact.Rows.Cast<System.Data.DataRow>().Skip(startIndex).Take(20).ToArray(); //.Select(string.Format("{0} > {1}  Or {0} = {2}", DatabaseObjects.Columns.Id, startId,value))
                    else
                        drs = dtContact.AsEnumerable().Where(dr => !dr.IsNull(DatabaseObjects.Columns.Title) && dr.Field<string>(DatabaseObjects.Columns.Title).ToLower().Contains(filter.ToLower())).ToArray();
                }

                if (drs != null && drs.Length > 0)
                    dtContact = drs.CopyToDataTable();
                else
                    dtContact = null;

                if (dtContact != null && selectRow != null)
                {
                    if (value > 0 && dtContact.AsEnumerable().Where(dr => dr.Field<long>(DatabaseObjects.Columns.ID) == value).ToArray().Length == 0)
                    {
                        dtContact.Rows.Add(selectRow.ItemArray);
                    }
                }
            }

            cmbContact.TextField = DatabaseObjects.Columns.Title;
            cmbContact.ValueField = DatabaseObjects.Columns.ID;
            cmbContact.DataSource = dtContact;
            cmbContact.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void cmbContact_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cmbContact_ItemsRequestedByFilterCondition(object source, DevExpress.Web.ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (IsPostBack)
            {
                FillContact(0, Convert.ToInt64(cmbContact.Value), e.Filter);
            }
        }

        protected void cmbContact_ItemRequestedByValue(object source, DevExpress.Web.ListEditItemRequestedByValueEventArgs e)
        {

        }

    }
}