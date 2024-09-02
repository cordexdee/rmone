using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ImportCheckListTemplate : UserControl
    {
        public string PublicTicketID { get; set; }
        public string ModuleName { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListTemplatesManager checkListTemplatesManager = null;
        CheckListsManager checkListsManager = null;
        CheckListRoleTemplatesManager checkListRoleTemplatesManager = null;
        CheckListRolesManager checkListRolesManager = null;
        CheckListTaskTemplatesManager checkListTaskTemplatesManager = null;
        CheckListTasksManager checkListTasksManager = null;
        CheckListTaskStatusManager checkListTaskStatusManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            checkListTemplatesManager = new CheckListTemplatesManager(context);
            checkListsManager = new CheckListsManager(context);
            checkListRoleTemplatesManager = new CheckListRoleTemplatesManager(context);
            checkListRolesManager = new CheckListRolesManager(context);
            checkListTaskTemplatesManager = new CheckListTaskTemplatesManager(context);
            checkListTasksManager = new CheckListTasksManager(context);
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);

            if (!IsPostBack)
                BindCheckListTemplate();
        }

        protected void btnImportCheckListTemplate_Click(object sender, EventArgs e)
        {
            if (ddlCheckListTemplate.Items.Count <= 0)
                return;

            //add checklisttemplate item into checklist.
            CheckLists splstCheckListItem = new CheckLists();
            splstCheckListItem.Title = ddlCheckListTemplate.SelectedItem.Text;
            splstCheckListItem.CheckListTemplateLookup = Convert.ToInt64(ddlCheckListTemplate.SelectedValue);
            splstCheckListItem.Module = ModuleName;
            splstCheckListItem.TicketId = PublicTicketID;
            checkListsManager.Insert(splstCheckListItem);


            //add roles for the particular checklisttemplate.
            List<CheckListRoleTemplates> splstcolRoleTemplate = checkListRoleTemplatesManager.Load(x => x.CheckListTemplateLookup == Convert.ToInt64(ddlCheckListTemplate.SelectedValue));

            if (splstcolRoleTemplate != null && splstcolRoleTemplate.Count > 0)
            {
                foreach (CheckListRoleTemplates item in splstcolRoleTemplate)
                {
                    CheckListRoles splstCheckListRoleItem = new CheckListRoles();
                    splstCheckListRoleItem.Title = item.Title;
                    splstCheckListRoleItem.Module = item.Module;
                    splstCheckListRoleItem.TicketId = PublicTicketID;
                    splstCheckListRoleItem.EmailAddress = item.EmailAddress;
                    splstCheckListRoleItem.CheckListLookup = splstCheckListItem.ID;
                    splstCheckListRoleItem.Type = item.Type;
                    checkListRolesManager.Insert(splstCheckListRoleItem);
                }
            }
            else
            {
                CheckListRoles splstCheckListRoleItem = new CheckListRoles();
                splstCheckListRoleItem.Title = "";
                splstCheckListRoleItem.Module = ModuleName;
                splstCheckListRoleItem.TicketId = PublicTicketID;
                splstCheckListRoleItem.EmailAddress = "";
                splstCheckListRoleItem.CheckListLookup = splstCheckListItem.ID;
                checkListRolesManager.Insert(splstCheckListRoleItem);
            }


            //add tasks for the particular checklisttemplate.
            List<CheckListTaskTemplates> splstcolTaskTemplate = checkListTaskTemplatesManager.Load(x => x.CheckListTemplateLookup == Convert.ToInt64(ddlCheckListTemplate.SelectedValue));

            if (splstcolTaskTemplate != null && splstcolTaskTemplate.Count > 0)
            {
                foreach (CheckListTaskTemplates item in splstcolTaskTemplate)
                {
                    CheckListTasks splstCheckListTaskItem = new CheckListTasks();
                    splstCheckListTaskItem.Title = item.Title;
                    splstCheckListTaskItem.Module = item.Module;
                    splstCheckListTaskItem.TicketId = PublicTicketID;
                    splstCheckListTaskItem.CheckListLookup = splstCheckListItem.ID;
                    checkListTasksManager.Insert(splstCheckListTaskItem);


                    //create checklist status entry..
                    List<CheckListRoles> dtCheckListRole = checkListRolesManager.Load(x => x.CheckListLookup == splstCheckListItem.ID);

                    if (dtCheckListRole != null && dtCheckListRole.Count > 0)
                    {
                        foreach (CheckListRoles checkListRoleRowItem in dtCheckListRole)
                        {
                            CheckListTaskStatus checkListTaskStatusItem = new CheckListTaskStatus();
                            checkListTaskStatusItem.TicketId = PublicTicketID;
                            checkListTaskStatusItem.UGITCheckListTaskStatus = "NC";
                            checkListTaskStatusItem.CheckListRoleLookup = checkListRoleRowItem.ID;
                            checkListTaskStatusItem.CheckListTaskLookup = splstCheckListTaskItem.ID;
                            checkListTaskStatusItem.Module = splstCheckListTaskItem.Module;
                            checkListTaskStatusItem.CheckListLookup = splstCheckListItem.ID;

                            checkListTaskStatusManager.Insert(checkListTaskStatusItem);
                        }
                    }
                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        void BindCheckListTemplate()
        {
            var checkListTemplates = checkListTemplatesManager.Load(x => x.Module == ModuleName).Select(x => new { x.ID, x.Title }).ToList();

            if (checkListTemplates != null && checkListTemplates.Count > 0)
            {
                ddlCheckListTemplate.Items.Clear();
                ddlCheckListTemplate.DataValueField = DatabaseObjects.Columns.ID;
                ddlCheckListTemplate.DataTextField = DatabaseObjects.Columns.Title;
                ddlCheckListTemplate.DataSource = checkListTemplates;
                ddlCheckListTemplate.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}