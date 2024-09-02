using DevExpress.Web;
using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Text;

namespace uGovernIT.Web
{
    public partial class ProjectsTaskList : UserControl
    {     
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleManager = null;
        UGITTaskManager taskManager = null;
        UserProfileManager UserManager = null;
        FieldConfiguration field = null;
        FieldConfigurationManager fmanger = null;
        UserProfile user;
        UserProfile currentUser = null;
        public string TaskAlertUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=ManualEscalation");
        protected string ticketurl;
        List<string> assignees = null;
        List<string> myGroups = new List<string>();
        List<string> pressors = new List<string>();
        List<string> userGroups = new List<string>();
        Dictionary<string, string> dicAssignee = new Dictionary<string, string>();
        Dictionary<object, List<string>> dicAssignedTo = new Dictionary<object, List<string>>();
        DataRow[] ticketrow = null;
        DateTime DueDate;
        DateTime MinDate = new DateTime(1800,1,1);

        protected void Page_Init(object sender, EventArgs e)
        {
            moduleManager = new ModuleViewManager(context);
            taskManager = new UGITTaskManager(context);
            UserManager = HttpContext.Current.GetUserManager();
            currentUser = HttpContext.Current.CurrentUser();
            fmanger = new FieldConfigurationManager(context);
            assignees = new List<string>();


            if (!IsPostBack)
            {
                var criteria = new GroupOperator(GroupOperatorType.And, new CriteriaOperator[] {
                new BinaryOperator("Status", "Completed", BinaryOperatorType.NotEqual),
                new BinaryOperator("Status", "Closed", BinaryOperatorType.NotEqual),
                new BinaryOperator("Status", "Cancelled", BinaryOperatorType.NotEqual)
            });

                grid.FilterExpression = criteria.ToString();
            }

            DataRow moduleDetail = moduleManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, "SVC"))[0]; //uHelper.GetModuleDetails("SVC", context);
            if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
            {
                ticketurl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            DataTable dtSVCTaskData = new DataTable();
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.ParentTicketId, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.UGITTaskType, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.Predecessors, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.ChildTicketId, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.Status, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.AssignedTo, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.DueDate, typeof(string));
            dtSVCTaskData.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            dtSVCTaskData.Columns.Add("TicketTitle", typeof(string));

            DataTable svcticketList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SVCRequests, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            DataTable taskList = UGITUtility.ToDataTable<UGITTask>(taskManager.LoadByModule("SVC"));

            if (taskList != null && taskList.Rows.Count > 0)
            {
                userGroups.Clear();
                myGroups = UserGroups(currentUser.Id);
                DataRow taskrow = null;

                foreach (DataRow item in taskList.Rows)
                {
                    // First see if we are showing this task
                    string assignedTo = string.Empty;
                    bool includeTask = rbtnAllTasks.Checked; // If "All Tasks" chosen, always show task even if unassigned
                    if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.AssignedTo))
                    {
                        if (dicAssignedTo.ContainsKey(item[DatabaseObjects.Columns.AssignedTo]))
                        {
                            assignees = dicAssignedTo[item[DatabaseObjects.Columns.AssignedTo]];
                        }
                        else
                        {
                            assignees = UGITUtility.ConvertStringToList(Convert.ToString(item[DatabaseObjects.Columns.AssignedTo]), ",");
                            dicAssignedTo[item[DatabaseObjects.Columns.AssignedTo]] = assignees;
                        }

                        if (dicAssignee != null && assignees.Count > 0)
                        {
                            if (dicAssignee.ContainsKey(assignedTo))
                                assignedTo = dicAssignee[assignedTo];
                            else
                            {
                                assignedTo = UserManager.GetUserOrGroupName(assignees);
                                dicAssignee[assignedTo] = assignedTo;
                            }
                        }

                        if (rbtnMyTasks.Checked)
                        {
                            if (assignees.Any(x => x == currentUser.Id))
                                includeTask = true;
                        }
                        else if (rbtnMyGroupTasks.Checked)
                        {
                            var AssignedToGroups = GetAssigneeGroups(assignees);

                            if (AssignedToGroups != null && AssignedToGroups.Count > 0 && AssignedToGroups.Any(x => myGroups.Contains(x)))
                                includeTask = true;
                        }
                    }
                    DueDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.DueDate]);
                    if (!includeTask)
                        continue;

                    // Showing task, now add row and set the columns
                    DataRow row = dtSVCTaskData.NewRow();
                    row[DatabaseObjects.Columns.Id] = item[DatabaseObjects.Columns.Id];
                    row[DatabaseObjects.Columns.AssignedTo] = assignedTo;
                    row[DatabaseObjects.Columns.ParentTicketId] = item[DatabaseObjects.Columns.TicketId];
                    row[DatabaseObjects.Columns.UGITTaskType] = item[DatabaseObjects.Columns.TaskBehaviour];
                    row[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                    row[DatabaseObjects.Columns.ChildTicketId] = item[DatabaseObjects.Columns.RelatedTicketID];
                    row[DatabaseObjects.Columns.Status] = item[DatabaseObjects.Columns.Status];
                    row[DatabaseObjects.Columns.ItemOrder] = item[DatabaseObjects.Columns.ItemOrder];
                    row[DatabaseObjects.Columns.DueDate] =  DueDate != DateTime.MinValue ? string.Format("{0:MMM-dd-yyyy}", item[DatabaseObjects.Columns.DueDate]) : string.Empty ; //item[DatabaseObjects.Columns.DueDate];

                    if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.Title])))
                    {
                        ticketrow = svcticketList.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, Convert.ToString(item[DatabaseObjects.Columns.TicketId])));
                        if (ticketrow != null && ticketrow.Length > 0)
                            row["TicketTitle"] = string.Format("<a onClick=\"return openTicketDialog('{0}','{1}');\" href='#'>{0}: {1}</a>", Convert.ToString(ticketrow[0][DatabaseObjects.Columns.TicketId]), uHelper.ReplaceInvalidCharsInURL(Convert.ToString(ticketrow[0][DatabaseObjects.Columns.Title])));
                        else
                            continue;
                    }



                    if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.Predecessors))
                    {
                        string[] arrPredecessors = Convert.ToString(item[DatabaseObjects.Columns.Predecessors]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        if (arrPredecessors != null && arrPredecessors.Count() > 0)
                        {
                            pressors.Clear();

                            foreach (var pred in arrPredecessors)
                            {
                                taskrow = taskList.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, pred)).FirstOrDefault();
                                if (taskrow != null)
                                    pressors.Add(Convert.ToString(taskrow[DatabaseObjects.Columns.ItemOrder]));
                            }
                            row[DatabaseObjects.Columns.Predecessors] = string.Join("; ", pressors.ToArray());
                        }
                    }

                    dtSVCTaskData.Rows.Add(row);
                }
            }

            if (dtSVCTaskData != null && dtSVCTaskData.Rows.Count > 0)
                dtSVCTaskData.DefaultView.Sort = string.Format("{0}, {1} ASC", DatabaseObjects.Columns.ParentTicketId, DatabaseObjects.Columns.ItemOrder);


            grid.DataSource = dtSVCTaskData;
            grid.DataBind();

        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void rbtnAllTasks_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void rbtnMyTasks_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void rbtnMyGroupTasks_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private List<string> UserGroups(string userId)
        {
            List<string> UsrGrp = new List<string>();

            field = fmanger.GetFieldByFieldName(DatabaseObjects.Columns.UserGroup);
            if (field != null && field.Datatype == "GroupField")
            {
                UsrGrp = UserManager.GetUserGroups(userId);
                return UsrGrp;
            }
            return UsrGrp;
        }

        private List<string> GetAssigneeGroups(List<string> userId)
        {
            List<string> assigneeGroups = new List<string>();
            StringBuilder userName = new StringBuilder();

            foreach (string entity in userId)
            {
                user = UserManager.GetUserById(entity);

                if (user == null) //for groups
                {
                    assigneeGroups.Add(entity); // do nothing as this is group
                }
                else
                {
                    assigneeGroups.AddRange(UserGroups(entity));
                }
            }

            return assigneeGroups;
        }
        
        /*
        private string GetUserOrGroupName(List<string> userId)
        {
            StringBuilder userName = new StringBuilder();
            foreach (string entity in userId)
            {
                user = UserManager.GetUserById(entity);

                if (user == null) // for User Groups
                {
                    List<UserProfile> lstGroupUsers = UserManager.GetUsersByGroupID(entity);

                    foreach (UserProfile userProfileItem in lstGroupUsers)
                    {
                        user = UserManager.GetUserById(userProfileItem.Id);

                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            if (userName.Length != 0)
                                userName.Append(";");
                            userName.Append(user.Name);
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Name))
                    {
                        if (userName.Length != 0)
                            userName.Append(";");
                        userName.Append(user.Name);
                    }
                }
            }
            return Convert.ToString(userName);
        }
        */
    }
}