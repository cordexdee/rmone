using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AddCheckList : UserControl
    {
        public string CheckListId { get; set; }
        DataRow splstCheckListItem;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListRolesManager checkListRolesManager = null;
        CheckListTaskStatusManager checkListTaskStatusManager = null;
        CheckListTasksManager checkListTasksManager = null;
        ModuleViewManager moduleViewManager = null;
        CheckListTemplatesManager checkListTemplatesManager = null;
        CheckListsManager checkListsManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            flpCheckListItemImport.Attributes["onchange"] = "UploadFile(this)";

            //peAssignedTo.UserTokenBoxAdd.GridViewStyles.FilterRow.CssClass = "userValueBox-HeaderFilterRow";
            //peAssignedTo.UserTokenBoxAdd.GridViewStyles.StatusBar.CssClass = "userValueBox-footerCloseBtn";
            //peAssignedTo.UserTokenBoxAdd.GridView.CssClass = "userValueBox-grid";
            checkListRolesManager = new CheckListRolesManager(context);
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);
            checkListTasksManager = new CheckListTasksManager(context);
            moduleViewManager = new ModuleViewManager(context);
            checkListTemplatesManager = new CheckListTemplatesManager(context);
            checkListsManager = new CheckListsManager(context);

            if (!IsPostBack)
                BindModule();

            if (Convert.ToInt64(CheckListId) > 0)
            {
                if (Request["IsTemplate"] == "true")
                {                 
                    trModule.Visible = true;
                    trUser.Visible = false;
                    trAutoLoad.Visible = true;
                    trCheckListItemImport.Visible = false;
                    splstCheckListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTemplates, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListId)}").Select()[0]; //SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckListTemplates, Convert.ToInt32(CheckListId));
                }
                else
                {                    
                    trModule.Visible = false;
                    trUser.Visible = true;
                    trAutoLoad.Visible = false;
                    trCheckListItemImport.Visible = true;
                    splstCheckListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckLists, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt64(CheckListId)}").Select()[0]; //SPListHelper.GetSPListItem(DatabaseObjects.Tables.CheckLists, Convert.ToInt32(CheckListId));
                }

                if (!IsPostBack)
                {
                    txtCheckListName.Text = Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.Title]);

                    string moduleLookup = Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.Module]);

                    if (moduleLookup != null)
                    {
                        ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(moduleLookup));
                    }

                    //if (Request["IsTemplate"] == "false" && uHelper.IfColumnExists(DatabaseObjects.Columns.AssignedTo, splstCheckListItem))
                    if (Request["IsTemplate"] == "false" && UGITUtility.IsSPItemExist(splstCheckListItem, DatabaseObjects.Columns.AssignedTo))                 
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.AssignedTo])))
                        {
                            string userLookups = Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.AssignedTo]);
                            if (userLookups != null)
                            {
                                //peAssignedTo.CommaSeparatedAccounts = UserProfile.CommaSeparatedAccountsFrom(userLookups, ",");
                                peAssignedTo.SetValues(userLookups);
                            }
                        }
                    }
                    else
                    {
                        if(UGITUtility.IsSPItemExist(splstCheckListItem, DatabaseObjects.Columns.AutoLoadChecklist))
                            chkAutoLoadCheckList.Checked = UGITUtility.StringToBoolean(Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.AutoLoadChecklist]));
                    }

                }

                lnkDeleteCheckList.Visible = true;
            }
            else
            {
                trCheckListItemImport.Visible = false;
                lnkDeleteCheckList.Visible = false;
                if (Request["IsTemplate"] == "true")
                {
                    trModule.Visible = true;
                    trUser.Visible = false;
                    trAutoLoad.Visible = true;
                    //Just need Table structure, with out any data
                    DataTable lstCheckListTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTemplates, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); //SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTemplates);
                    splstCheckListItem = lstCheckListTemplates.NewRow();
                }
                else
                {
                    //trrbFieldType.Visible = false;
                    trModule.Visible = false;
                    trUser.Visible = true;
                    trAutoLoad.Visible = false;
                    //Just need Table structure, with out any data
                    DataTable lstCheckListTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckLists, $"{DatabaseObjects.Columns.ID} < 0 and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); //SPListHelper.GetSPList(DatabaseObjects.Tables.CheckLists);
                    splstCheckListItem = lstCheckListTemplates.NewRow();
                }
            }

        }

        protected void btnSaveCheckList_Click(object sender, EventArgs e)
        {
            long Id = 0;

            splstCheckListItem[DatabaseObjects.Columns.Title] = txtCheckListName.Text;
            
            if (Request["IsTemplate"] == "false")
            {
                splstCheckListItem[DatabaseObjects.Columns.TicketId] = Convert.ToString(Request["ticketId"]);

                List<string> assignedUsers = new List<string>();
                /*
                SPFieldUserValueCollection userMultiLookup = new SPFieldUserValueCollection();
                if (peAssignedTo.Accounts.Count > 0)
                {
                    for (int i = 0; i < peAssignedTo.Accounts.Count; i++)
                    {
                        PickerEntity entity = (PickerEntity)peAssignedTo.Entities[i];
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

                //splstCheckListItem[DatabaseObjects.Columns.AssignedTo] = userMultiLookup;
                splstCheckListItem[DatabaseObjects.Columns.AssignedTo] = peAssignedTo.GetValues();
                splstCheckListItem[DatabaseObjects.Columns.Module] = uHelper.getModuleNameByTicketId(Request["ticketId"]); //Convert.ToString(Request["ticketId"]).Substring(0, (Convert.ToString(Request["ticketId"]).IndexOf('-'))); //uHelper.getModuleIdByTicketID(context, Request["ticketId"]);


                CheckLists checkLists = new CheckLists();
                long.TryParse(Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.ID]), out Id);
                checkLists.ID = Id;
                checkLists.Title = txtCheckListName.Text.Trim();
                checkLists.TicketId = Convert.ToString(Request["ticketId"]);
                checkLists.AssignedTo = peAssignedTo.GetValues();
                checkLists.Module = uHelper.getModuleNameByTicketId(Request["ticketId"]); //Convert.ToString(Request["ticketId"]).Substring(0, (Convert.ToString(Request["ticketId"]).IndexOf('-')));
                if (checkLists.ID <= 0)
                    checkListsManager.Insert(checkLists);
                else
                    checkListsManager.Update(checkLists);


                //Import CheckList Items..
                try
                {
                    if (flpCheckListItemImport.HasFile)
                    {
                        string FileName = Path.GetFileName(flpCheckListItemImport.PostedFile.FileName);
                        string Extension = Path.GetExtension(flpCheckListItemImport.PostedFile.FileName);
                        string FolderPath = flpCheckListItemImport.PostedFile.FileName;
                        if (Extension != ".xlsx")
                        {
                            lblMessage.Text = "only excel file required(.xlsx).";
                            lblMessage.Visible = true;
                            return;
                        }

                        string FilePath = string.Format(@"{0}\{1}", uHelper.GetTempFolderPath(), FileName);
                        flpCheckListItemImport.SaveAs(FilePath);
                       
                        ASPxSpreadsheet1.Document.LoadDocument(FilePath);

                        var worksheet = ASPxSpreadsheet1.Document.Worksheets.ActiveWorksheet;
                        Workbook wb = new Workbook();
                        wb.LoadDocument(FilePath);
                        worksheet.CopyFrom(wb.Worksheets[0]);

                        int colCount = worksheet.Columns.LastUsedIndex;
                        int rowCount = worksheet.Rows.LastUsedIndex;

                        RowCollection rows = worksheet.Rows;
                        Row row = rows[0];

                        //SPList lstCheckListRole = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListRoles);
                        for (int i = 1; i <= rowCount; i++)
                        {
                            /*
                                SPListItem splstCheckListRoleItem = lstCheckListRole.Items.Add();
                                splstCheckListRoleItem[DatabaseObjects.Columns.Title] = rows[i][0].DisplayText;
                                splstCheckListRoleItem[DatabaseObjects.Columns.EmailAddress] = rows[i][1].DisplayText;
                                splstCheckListRoleItem[DatabaseObjects.Columns.ModuleNameLookup] = splstCheckListItem[DatabaseObjects.Columns.ModuleNameLookup];
                                splstCheckListRoleItem[DatabaseObjects.Columns.TicketId] = splstCheckListItem[DatabaseObjects.Columns.TicketId];
                                splstCheckListRoleItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;
                                splstCheckListRoleItem.Update();
                            */

                            CheckListRoles splstCheckListRoleItem = new CheckListRoles();
                            splstCheckListRoleItem.Title = rows[i][0].DisplayText;
                            splstCheckListRoleItem.EmailAddress = rows[i][1].DisplayText;
                            splstCheckListRoleItem.Module = rows[i][2].DisplayText;
                            splstCheckListRoleItem.TicketId = Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.TicketId]);
                            splstCheckListRoleItem.CheckListLookup = Convert.ToInt64(CheckListId);
                            checkListRolesManager.Insert(splstCheckListRoleItem);

                            /*
                            SPQuery query = new SPQuery();
                            query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
                            DataTable dtCheckListTask = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTasks, query);
                            */

                            List<CheckListTasks> dtCheckListTask = checkListTasksManager.Load($"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId}");

                            if (dtCheckListTask != null && dtCheckListTask.Count() > 0)
                            {
                                //SPList lstCheckListTaskStatus = SPListHelper.GetSPList(DatabaseObjects.Tables.CheckListTaskStatus);
                                foreach (var checkListTaskRowItem in dtCheckListTask)
                                {
                                    /*
                                    SPListItem checkListTaskStatusItem = lstCheckListTaskStatus.Items.Add();
                                    checkListTaskStatusItem[DatabaseObjects.Columns.TicketId] = Request["ticketId"];
                                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskStatus] = "NC";

                                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListRoleLookup] = CheckListRoleLookup
                                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListTaskLookup] = checkListTaskRowItem[DatabaseObjects.Columns.Id];
                                    checkListTaskStatusItem[DatabaseObjects.Columns.ModuleNameLookup] = uHelper.getModuleIdByTicketID(context, Request["ticketId"]);
                                    checkListTaskStatusItem[DatabaseObjects.Columns.CheckListLookup] = CheckListId;

                                    checkListTaskStatusItem.Update();
                                    */

                                    CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                                    checkListTaskStatusItem.TicketId = Request["ticketId"];
                                    checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                                    checkListTaskStatusItem.CheckListRoleLookup = splstCheckListRoleItem.ID; //CheckListRoleLookup;
                                    checkListTaskStatusItem.CheckListTaskLookup = Convert.ToInt64(checkListTaskRowItem.ID);
                                    checkListTaskStatusItem.Module = uHelper.getModuleNameByTicketId(Request["ticketId"]); // Convert.ToString(Request["ticketId"]).Substring(0, (Convert.ToString(Request["ticketId"]).IndexOf('-')));
                                    checkListTaskStatusItem.CheckListLookup = Convert.ToInt64(CheckListId);
                                    checkListTaskStatusManager.Insert(checkListTaskStatusItem);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = string.Format("Unable to import-{0}", ex.Message);
                    lblMessage.Visible = true;
                }

            }
            else
            {
                //splstCheckListItem[DatabaseObjects.Columns.Module] = ddlModule.SelectedValue; //Convert.ToInt32(ddlModule.SelectedValue);
                //splstCheckListItem[DatabaseObjects.Columns.AutoLoadChecklist] = chkAutoLoadCheckList.Checked;

                CheckListTemplates checkListTemplates = new CheckListTemplates();                
                long.TryParse(Convert.ToString(splstCheckListItem[DatabaseObjects.Columns.ID]), out Id);
                checkListTemplates.ID = Id;
                checkListTemplates.Title = txtCheckListName.Text.Trim();
                checkListTemplates.Module = ddlModule.SelectedValue;
                checkListTemplates.AutoLoadChecklist = chkAutoLoadCheckList.Checked;

                if(checkListTemplates.ID <= 0)
                    checkListTemplatesManager.Insert(checkListTemplates);
                else
                    checkListTemplatesManager.Update(checkListTemplates);
            }

            //////splstCheckListItem.Update();

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void lnkDeleteCheckList_Click(object sender, EventArgs e)
        {
            if (Request["IsTemplate"] == "true")
            {
                DeleteCheckListTemplateTaskRoles();
                CheckListTemplates checkListTemplates = checkListTemplatesManager.LoadByID(Convert.ToInt64(splstCheckListItem[DatabaseObjects.Columns.ID]));
                checkListTemplatesManager.Delete(checkListTemplates);
            }
            else
            {
                DeleteCheckListTaskRoles();
                CheckLists checkLists = checkListsManager.LoadByID(Convert.ToInt64(splstCheckListItem[DatabaseObjects.Columns.ID]));
                checkListsManager.Delete(checkLists);
            }
            //splstCheckListItem.Delete();

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void DeleteCheckListTemplateTaskRoles()
        {
            //Delete Role related to checklist.
            /*
            SPQuery queryR = new SPQuery();
            queryR.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListTemplateLookup, CheckListId);
            queryR.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListTemplateLookup);
            queryR.ViewFieldsOnly = true;
            //query.ItemIdQuery = true;
            SPListItemCollection spColCheckListRole = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoleTemplates, queryR);
            */

            string queryR = $"{DatabaseObjects.Columns.CheckListTemplateLookup} = {CheckListId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
            DataTable spColCheckListRole = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoleTemplates, queryR);

            List<long> Ids = new List<long>();
            foreach (DataRow item in spColCheckListRole.Rows)
            {
                Ids.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
            }

            if (Ids.Count > 0)
            {
                //RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListRoleTemplates, HttpContext.Current.GetManagerContext().SiteUrl); // SPContext.Current.Web.Url); 
                RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListRoleTemplates); // SPContext.Current.Web.Url); 
            }


            //Delete Task related to checklist.
            /*
            SPQuery queryT = new SPQuery();
            queryT.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListTemplateLookup, CheckListId);
            queryT.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListTemplateLookup);
            queryT.ViewFieldsOnly = true;
            //query.ItemIdQuery = true;
            SPListItemCollection spColCheckListTask = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskTemplates, queryR);
            */

            string queryT = $"{DatabaseObjects.Columns.CheckListTemplateLookup} = {CheckListId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
            DataTable spColCheckListTask = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskTemplates, queryT);

            List<long> TaskIds = new List<long>();
            foreach (DataRow item in spColCheckListTask.Rows)
            {
                TaskIds.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
            }

            if (TaskIds.Count > 0)
            {
                //RMMSummaryHelper.BatchDeleteListItems(TaskIds, DatabaseObjects.Tables.CheckListTaskTemplates, SPContext.Current.Web.Url);
                RMMSummaryHelper.BatchDeleteListItems(context, TaskIds, DatabaseObjects.Tables.CheckListTaskTemplates); 
            }
        }

        private void DeleteCheckListTaskRoles()
        {
            //Delete status entry form CheckListStatus
            /*
            SPQuery queryTS = new SPQuery();
            queryTS.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
            queryTS.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListLookup);
            queryTS.ViewFieldsOnly = true;
            //query.ItemIdQuery = true;
            SPListItemCollection spColCheckListTaskStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskStatus, queryTS);
            */

            string queryTS = $"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
            DataTable spColCheckListTaskStatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskStatus, queryTS);

            List<long> TSIds = new List<long>();
            foreach (DataRow item in spColCheckListTaskStatus.Rows)
            {
                TSIds.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
            }

            if (TSIds.Count > 0)
            {
                //RMMSummaryHelper.BatchDeleteListItems(TSIds, DatabaseObjects.Tables.CheckListTaskStatus, SPContext.Current.Web.Url);
                RMMSummaryHelper.BatchDeleteListItems(context, TSIds, DatabaseObjects.Tables.CheckListTaskStatus); 
            }


            //Delete Role related to checklist.
            /*
            SPQuery queryR = new SPQuery();
            queryR.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
            queryR.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListLookup);
            queryR.ViewFieldsOnly = true;
            //query.ItemIdQuery = true;
            SPListItemCollection spColCheckListRole = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoles, queryR);
            */

            string queryR = $"{DatabaseObjects.Columns.CheckListLookup} = {CheckListId} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
            DataTable spColCheckListRole = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoles, queryR);

            List<long> Ids = new List<long>();
            foreach (DataRow item in spColCheckListRole.Rows)
            {
                Ids.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
            }

            if (Ids.Count > 0)
            {
                //RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Tables.CheckListRoles, SPContext.Current.Web.Url);
                RMMSummaryHelper.BatchDeleteListItems(context, Ids, DatabaseObjects.Tables.CheckListRoles);
            }


            //Delete Task related to checklist.
            /*
            SPQuery queryT = new SPQuery();
            queryT.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListLookup, CheckListId);
            queryT.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListLookup);
            queryT.ViewFieldsOnly = true;
            //query.ItemIdQuery = true;
            SPListItemCollection spColCheckListTask = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTasks, queryR);
            */
                        
            DataTable spColCheckListTask = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTasks, queryR);

            List<long> TaskIds = new List<long>();
            foreach (DataRow item in spColCheckListTask.Rows)
            {
                TaskIds.Add(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
            }

            if (TaskIds.Count > 0)
            {
                //RMMSummaryHelper.BatchDeleteListItems(TaskIds, DatabaseObjects.Tables.CheckListTasks, SPContext.Current.Web.Url);
                RMMSummaryHelper.BatchDeleteListItems(context, TaskIds, DatabaseObjects.Tables.CheckListTasks);
            }
        }

        void BindModule()
        {
            /*
            SPList spModuleList = SPListHelper.GetSPList(DatabaseObjects.Tables.Modules);
            ddlModule.Items.Clear();

            DataTable dtModule = spModuleList.Items.GetDataTable();
            dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
            dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule });
            DataRow[] moduleRows = dtModule.Select(string.Format("{0}='1'", DatabaseObjects.Columns.EnableModule));
            foreach (DataRow moduleRow in moduleRows)
            {
                string moduleId = Convert.ToString(moduleRow[DatabaseObjects.Columns.Id]);
                ddlModule.Items.Add(new ListItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.Title]), Value = moduleId });
            }
            */

            //List<UGITModule> lstModules = moduleViewManager.LoadAllModule();
            string queryR = $"{DatabaseObjects.Columns.EnableModule} = 1 and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
            DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, queryR, $"{DatabaseObjects.Columns.ID},{DatabaseObjects.Columns.Title},{DatabaseObjects.Columns.ModuleName}", null);
            if (dtModules != null && dtModules.Rows.Count > 0)
            {
                dtModules.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataSource = dtModules;
                ddlModule.DataBind();
            }
        }
    }
}