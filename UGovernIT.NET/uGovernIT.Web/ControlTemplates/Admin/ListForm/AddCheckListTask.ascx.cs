using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AddCheckListTask : UserControl
    {
        public string CheckListId { get; set; }
        public string CheckListTaskId { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListTaskStatusManager checkListTaskStatusManager = null;
        CheckListsManager checkListsManager = null;
        CheckListTemplatesManager checkListTemplatesManager = null;
        CheckListRolesManager checkListRolesManager = null;
        CheckListTasksManager checkListTasksManager = null;
        CheckListTaskTemplatesManager checkListTaskTemplatesManager = null;
        DataRow splstCheckListTaskItem;

        protected void Page_Load(object sender, EventArgs e)
        {
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);
            checkListsManager = new CheckListsManager(context);
            checkListTemplatesManager = new CheckListTemplatesManager(context);
            checkListRolesManager = new CheckListRolesManager(context);
            checkListTasksManager = new CheckListTasksManager(context);
            checkListTaskTemplatesManager = new CheckListTaskTemplatesManager(context);

            if (Convert.ToInt64(CheckListTaskId) > 0)
            {
                if (Request["IsTemplate"] == "true")
                    //splstCheckListTaskItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTaskTemplates, Convert.ToInt32(CheckListTaskId));
                    splstCheckListTaskItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskTemplates, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListTaskId)}").Select()[0];
                else
                    //splstCheckListTaskItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTasks, Convert.ToInt32(CheckListTaskId));
                    splstCheckListTaskItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTasks, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListTaskId)}").Select()[0];

                if (!IsPostBack)
                {
                    txtTaskName.Text = Convert.ToString(splstCheckListTaskItem[DatabaseObjects.Columns.Title]);
                }
                lnkDeleteCheckListTask.Visible = true;
            }
            else
            {
                lnkDeleteCheckListTask.Visible = false;
                if (Request["IsTemplate"] == "true")
                {
                    //SPList lstCheckListTaskTemplates = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskTemplates);
                    //splstCheckListTaskItem = lstCheckListTaskTemplates.Items.Add();

                    //Just need Table structure, with out any data
                    DataTable lstCheckListTaskTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskTemplates, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); 
                    splstCheckListTaskItem = lstCheckListTaskTemplates.NewRow();
                }
                else
                {
                    //SPList lstCheckListTaskTemplates = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTasks);
                    //splstCheckListTaskItem = lstCheckListTaskTemplates.Items.Add();

                    //Just need Table structure, with out any data
                    DataTable lstCheckListTasks = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTasks, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    splstCheckListTaskItem = lstCheckListTasks.NewRow();
                }
            }
        }

        protected void lnkDeleteCheckListTask_Click(object sender, EventArgs e)
        {
            //SPListItem splstSubConTask = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTemplates, Convert.ToInt32(CheckListId));
            //splstCheckListTaskItem.Delete();
            if (Request["IsTemplate"] == "true")
            {
                CheckListTaskTemplates checkListTaskTemplates = checkListTaskTemplatesManager.LoadByID(Convert.ToInt64(splstCheckListTaskItem[DatabaseObjects.Columns.ID]));
                checkListTaskTemplatesManager.Delete(checkListTaskTemplates);
            }
            else if (Request["IsTemplate"] == "false")
            {
                // Delete all entry from CheckListStatus for that particular Role.
                /*
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE' /><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId, DatabaseObjects.Columns.CheckListTaskLookup, CheckListTaskId);
                query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.UGITCheckListTaskStatus, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListLookup);
                query.ViewFieldsOnly = true;
                SPListItemCollection spColCheckListStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskStatus, query);
                */

                string query = $"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.CheckListTaskLookup} = {CheckListTaskId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable spColCheckListStatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskStatus, query);

                List<long> Ids = new List<long>();
                foreach (DataRow item in spColCheckListStatus.Rows)
                {
                    Ids.Add(Convert.ToInt64(item[DatabaseObjects.Columns.Id]));
                }

                if (Ids.Count > 0)
                {
                    //RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                    RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListTaskStatus);
                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnSaveCheckListTask_Click(object sender, EventArgs e)
        {
            long Id = 0;
            long CheckListTaskTemplatesID = 0;
            long CheckListTaskID = 0;
            string TicketId = string.Empty, Module = string.Empty;

            long.TryParse(Convert.ToString(splstCheckListTaskItem[DatabaseObjects.Columns.ID]), out Id);
            if (Request["IsTemplate"] == "true")
            {
                /*
                SPListItem spCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTemplates, Convert.ToInt64(CheckListId));
                splstCheckListTaskItem[DatabaseObjects.Columns.Module] = spCheckListItem[DatabaseObjects.Columns.Module];
                
                splstCheckListTaskItem[DatabaseObjects.Columns.CheckListTemplateLookup] = CheckListId;
                */

                CheckListTemplates spCheckListItem = checkListTemplatesManager.LoadByID(Convert.ToInt64(CheckListId));

                CheckListTaskTemplates splstCheckListTaskItem = new CheckListTaskTemplates();

                splstCheckListTaskItem.ID = Id;
                splstCheckListTaskItem.Module = spCheckListItem.Module;
                splstCheckListTaskItem.CheckListTemplateLookup = Convert.ToInt64(CheckListId);

                splstCheckListTaskItem.Title = txtTaskName.Text;

                if (splstCheckListTaskItem.ID <= 0)
                    checkListTaskTemplatesManager.Insert(splstCheckListTaskItem);
                else
                    checkListTaskTemplatesManager.Update(splstCheckListTaskItem);

                CheckListTaskTemplatesID = splstCheckListTaskItem.ID;
                Module = splstCheckListTaskItem.Module;
            }
            else
            {
                /*
                SPListItem spCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckLists, Convert.ToInt64(CheckListId));
                splstCheckListTaskItem[DatabaseObjects.Columns.Module] = spCheckListItem[DatabaseObjects.Columns.Module];
                splstCheckListTaskItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];

                splstCheckListTaskItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;
                */
                
                CheckLists spCheckListItem = checkListsManager.LoadByID(Convert.ToInt64(CheckListId));

                CheckListTasks splstCheckListTaskItem = new CheckListTasks();
                splstCheckListTaskItem.Module = spCheckListItem.Module;
                splstCheckListTaskItem.TicketId = Request["ticketId"];
                splstCheckListTaskItem.CheckListLookup  = Convert.ToInt64(CheckListId);

                splstCheckListTaskItem.Title = txtTaskName.Text;

                if (splstCheckListTaskItem.ID <= 0)
                    checkListTasksManager.Insert(splstCheckListTaskItem);
                else
                    checkListTasksManager.Update(splstCheckListTaskItem);

                CheckListTaskID = splstCheckListTaskItem.ID;
                Module = splstCheckListTaskItem.Module;
                TicketId = splstCheckListTaskItem.TicketId;
            }

            //splstCheckListTaskItem[DatabaseObjects.Columns.Title] = txtTaskName.Text;
            //splstCheckListTaskItem.Update();




            if (Request["IsTemplate"] == "false" && Convert.ToInt64(CheckListTaskId) == 0)
            {
                /*
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                DataTable dtCheckListRole = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListRoles, query);
                */

                string query = $"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable dtCheckListRole = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoles, query);

                if (dtCheckListRole != null && dtCheckListRole.Rows.Count > 0)
                {
                    //SPList lstCheckListTaskStatus = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskStatus);
                    foreach (DataRow checkListRoleRowItem in dtCheckListRole.Rows)
                    {
                        /*
                        SPListItem checkListTaskStatusItem = lstCheckListTaskStatus.Items.Add();
                        checkListTaskStatusItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];
                        checkListTaskStatusItem[DatabaseObjects.Columns.UGITCheckListTaskStatus] = "NC";

                        checkListTaskStatusItem[DatabaseObjects.Columns.CheckListRoleLookup] = checkListRoleRowItem[DatabaseObjects.Columns.Id];
                        checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskLookup] = splstCheckListTaskItem.ID;
                        checkListTaskStatusItem[DatabaseObjects.Columns.Module] = Convert.ToString(Request["ticketId"]).Substring(0, (Convert.ToString(Request["ticketId"]).IndexOf('-'))); //uHelper.getModuleIdByTicketID(Request["ticketId"]);
                        checkListTaskStatusItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                        checkListTaskStatusItem.Update();
                        */
                        CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                        checkListTaskStatusItem.TicketId = Request["ticketId"];
                        checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                        checkListTaskStatusItem.Module = Convert.ToString(Request["ticketId"]).Substring(0, (Convert.ToString(Request["ticketId"]).IndexOf('-')));
                        checkListTaskStatusItem.CheckListRoleLookup = Convert.ToInt64(checkListRoleRowItem[DatabaseObjects.Columns.ID]);
                        checkListTaskStatusItem.CheckListTaskLookup = CheckListTaskID; //Convert.ToInt64(splstCheckListTaskItem[DatabaseObjects.Columns.ID]);
                        checkListTaskStatusItem.CheckListLookup = Convert.ToInt64(CheckListId);
                        checkListTaskStatusManager.Insert(checkListTaskStatusItem);
                    }
                }
                else
                {
                    //create default role and default taskstatus entry
                    /*
                    SPList lstCheckListRole = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListRoles);
                    SPListItem splstCheckListRoleItem = lstCheckListRole.Items.Add();
                    splstCheckListRoleItem[DatabaseObjects.Columns.Title] = "";
                    splstCheckListRoleItem[DatabaseObjects.Columns.Module] = splstCheckListTaskItem[DatabaseObjects.Columns.Module];
                    splstCheckListRoleItem[DatabaseObjects.Columns.TicketId] = splstCheckListTaskItem[DatabaseObjects.Columns.TicketId];
                    splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress] = "";
                    splstCheckListRoleItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;
                    splstCheckListRoleItem.Update();

                    SPList lstCheckListTaskStatus = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskStatus);

                    SPListItem checkListTaskStatusItem = lstCheckListTaskStatus.Items.Add();
                    checkListTaskStatusItem[DatabaseObjects.Columns.TicketId] = splstCheckListTaskItem[DatabaseObjects.Columns.TicketId];
                    checkListTaskStatusItem[DatabaseObjects.Columns.UGITCheckListTaskStatus] = "NC";


                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListRoleLookup] = splstCheckListRoleItem[DatabaseObjects.Columns.Id];
                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskLookup] = splstCheckListTaskItem.ID;
                    checkListTaskStatusItem[DatabaseObjects.Columns.Module] = splstCheckListTaskItem[DatabaseObjects.Columns.Module];
                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                    checkListTaskStatusItem.Update();
                    */
                                     
                    CheckListRoles splstCheckListRoleItem = new CheckListRoles();
                    splstCheckListRoleItem.Title = string.Empty;
                    splstCheckListRoleItem.Module = Module; //Convert.ToString(splstCheckListTaskItem[DatabaseObjects.Columns.Module]);
                    splstCheckListRoleItem.TicketId = TicketId; //Convert.ToString(splstCheckListTaskItem[DatabaseObjects.Columns.TicketId]);
                    splstCheckListRoleItem.EmailAddress = string.Empty;
                    splstCheckListRoleItem.CheckListLookup = Convert.ToInt64(CheckListId);
                    checkListRolesManager.Insert(splstCheckListRoleItem);


                    CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                    checkListTaskStatusItem.TicketId = TicketId;
                    checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                    checkListTaskStatusItem.CheckListRoleLookup = splstCheckListRoleItem.ID;
                    checkListTaskStatusItem.CheckListTaskLookup = CheckListTaskID; //Convert.ToInt64(splstCheckListTaskItem[DatabaseObjects.Columns.ID]);
                    checkListTaskStatusItem.Module = Module;
                    checkListTaskStatusItem.CheckListLookup = Convert.ToInt64(CheckListId);
                    checkListTaskStatusManager.Insert(checkListTaskStatusItem);

                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}