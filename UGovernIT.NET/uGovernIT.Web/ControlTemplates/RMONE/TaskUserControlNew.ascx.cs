using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.XtraCharts.Native;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class TaskUserControlNew : System.Web.UI.UserControl
    {
        private ApplicationContext _applicationContext;
        public int UserOpportunitiesCount { get; set; }
        public int UserTrackedWorkCount { get; set; }
        public int UserOnGoingWorkCount { get; set; }

        public List<TaskData> OnGoingTask
        {
            get; set;
        }

        public List<TaskData> NotStartedTask { get; set; }
        public List<TaskData> CompletedTask { get; set; }
        public List<TaskData> AllTasks { get; set; }
        public TaskUserControlNew()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
        }
        private ConfigurationVariableManager _configVariableMGR = null;
        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configVariableMGR == null)
                {
                    _configVariableMGR = new ConfigurationVariableManager(_applicationContext);
                }
                return _configVariableMGR;
            }
        }

        private ModuleViewManager _moduleViewManager = null;
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(_applicationContext);
                }
                return _moduleViewManager;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, object> arrParams1 = new Dictionary<string, object>();
            arrParams1.Add("TenantId", _applicationContext.TenantID);
            arrParams1.Add("UserId", _applicationContext.CurrentUser.Id);
            DataTable data = GetTableDataManager.GetData("MyProjectCount", arrParams1);
            this.UserOpportunitiesCount = int.Parse(data.Rows[0]["Opportunities"].ToString());
            this.UserTrackedWorkCount = int.Parse(data.Rows[0]["TrackedWork"].ToString());
            this.UserOnGoingWorkCount = int.Parse(data.Rows[0]["OngoingWork"].ToString());
            GenerateTaskData();
        }

        public DataTable getDataSource(string type)
        {
            int tasksDisplayDuration = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue("CompletedTasksDisplayDuration"), 30);
            var ownerColumn = DatabaseObjects.Columns.AssignedTo;
            var Status = DatabaseObjects.Columns.Status;
            var TaskStatus = DatabaseObjects.Columns.TaskStatus;
            var ActivityStatus = DatabaseObjects.Columns.ActivityStatus;
            var userid = _applicationContext.CurrentUser.Id;
            var actualstatus = "Completed";
            var uManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var role = uManager.GetUserRoles(userid).Select(x => x.Id).ToList();

            var sbQuery = new StringBuilder();

            sbQuery.Append($"{DatabaseObjects.Columns.TenantID}='{_applicationContext.TenantID}' AND {DatabaseObjects.Columns.Deleted} <> 'True' ");

            sbQuery.Append($" AND ({ownerColumn} like '%{userid}%'");
            foreach (var item in role)
            {
                sbQuery.Append($" OR {ownerColumn} like '%{item}%'");
            }
            sbQuery.Append($")");
            if (type == "PendingTask")
            {
                sbQuery.Append($" AND ({Status} !='{actualstatus}')");
            }
            else if (type == "CompletedTask")
            {
                sbQuery.Append($" AND ({Status} = '{actualstatus}')");
                sbQuery.Append($" AND ({DatabaseObjects.Columns.CompletionDate} >= (GETDATE()-{tasksDisplayDuration}))");
            }

            DataTable dtModuleTasks = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasks, sbQuery.ToString());
            DataColumn newCol1 = new System.Data.DataColumn("TableName", typeof(System.String));
            newCol1.DefaultValue = DatabaseObjects.Tables.ModuleTasks;
            dtModuleTasks.Columns.Add(newCol1);
            dtModuleTasks.Columns.Add(new DataColumn("CardToolTip", typeof(System.String)));

            sbQuery.Replace(Status, TaskStatus);
            sbQuery.Replace(DatabaseObjects.Columns.DueDate, DatabaseObjects.Columns.TaskDueDate);
            DataTable dtStageConstraints = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStageConstraints, sbQuery.ToString());

            dtStageConstraints.Columns["ModuleNameLookup"].ColumnName = "ModuleName";
            dtStageConstraints.Columns["TaskDueDate"].ColumnName = "DueDate";
            dtStageConstraints.Columns["TaskStatus"].ColumnName = "Status";
            dtStageConstraints.Columns["TaskEstimatedHours"].ColumnName = "EstimatedHours";
            dtStageConstraints.Columns["ModuleStep"].ColumnName = "StageStep";

            DataColumn newCol2 = new System.Data.DataColumn("TableName", typeof(System.String));
            newCol2.DefaultValue = DatabaseObjects.Tables.ModuleStageConstraints;
            dtStageConstraints.Columns.Add(newCol2);
            dtStageConstraints.Columns.Add(new DataColumn("CardToolTip", typeof(System.String)));

            dtModuleTasks.Merge(dtStageConstraints, false, MissingSchemaAction.Ignore);

            #region Activity            
            sbQuery.Replace(TaskStatus, ActivityStatus);
            sbQuery.Replace("CompletionDate", "Modified");
            sbQuery.Replace(DatabaseObjects.Columns.TaskDueDate, DatabaseObjects.Columns.DueDate);
            DataTable dtActivities = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMActivities, $"{sbQuery.ToString()} and {DatabaseObjects.Columns.Deleted} = 0");

            //dtStageConstraints.Columns["ModuleNameLookup"].ColumnName = "ModuleName";
            //dtStageConstraints.Columns["TaskDueDate"].ColumnName = "DueDate";
            dtActivities.Columns[ActivityStatus].ColumnName = "Status";
            dtActivities.Columns[DatabaseObjects.Columns.EndDate].ColumnName = DatabaseObjects.Columns.CompletionDate;

            //dtActivities.Columns[DatabaseObjects.Columns.ID].ColumnName = "ActivityId";
            //dtActivities.Columns["ModuleStep"].ColumnName = "StageStep";

            DataColumn newCol3 = new System.Data.DataColumn("TableName", typeof(System.String));
            newCol3.DefaultValue = DatabaseObjects.Tables.CRMActivities;
            dtActivities.Columns.Add(newCol3);
            dtActivities.Columns.Add(new DataColumn("CardToolTip", typeof(System.String)));
            dtModuleTasks.Merge(dtActivities, false, MissingSchemaAction.Ignore);
            #endregion

            dtModuleTasks = FillToolTip(dtModuleTasks);

            return dtModuleTasks;
        }
        private DataTable FillToolTip(DataTable dtModuleTasks)
        {
            //ModuleViewManager ModuleManager = new ModuleViewManager(ApplicationContext);
            CRMActivitiesManager CRMActivitiesManager = new CRMActivitiesManager(_applicationContext);
            var modules = ModuleViewManager.Load().Select(x => new { x.ID, x.ModuleName, x.ModuleTable }).ToList();
            string moduleTable = string.Empty;
            DataTable item = new DataTable();
            foreach (DataRow row in dtModuleTasks.Rows)
            {
                if (Convert.ToString(row["TableName"]) == DatabaseObjects.Tables.CRMActivities)
                {
                    var activity = CRMActivitiesManager.LoadByID(Convert.ToInt64(row[DatabaseObjects.Columns.ID]));
                    if (activity != null)
                    {
                        moduleTable = modules.FirstOrDefault(x => x.ModuleName == ModuleNames.CON).ModuleTable;
                        item = GetTableDataManager.GetTableData(moduleTable, $"{DatabaseObjects.Columns.TicketId} = '{activity.ContactLookup}' and {DatabaseObjects.Columns.TenantID} = '{_applicationContext.TenantID}'", "TicketId,FirstName,LastName", null);
                        if (item != null && item.Rows.Count > 0)
                        {
                            row["CardToolTip"] = $"{item.Rows[0]["TicketId"]}: {item.Rows[0]["FirstName"]} {item.Rows[0]["LastName"]}";
                            row["TicketId"] = item.Rows[0]["TicketId"];
                        }
                        else
                            row["CardToolTip"] = row[DatabaseObjects.Columns.Title];
                    }
                }
                else
                {
                    try
                    {
                        string moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                        if (!string.IsNullOrEmpty(moduleName))
                        {
                            moduleTable = modules.FirstOrDefault(x => x.ModuleName == moduleName).ModuleTable;
                            item = GetTableDataManager.GetTableData(moduleTable, $"{DatabaseObjects.Columns.TicketId} = '{Convert.ToString(row[DatabaseObjects.Columns.TicketId])}' and {DatabaseObjects.Columns.TenantID} = '{_applicationContext.TenantID}'", "TicketId,Title", null);
                            if (item != null && item.Rows.Count > 0)
                            {
                                row["CardToolTip"] = $"{item.Rows[0]["TicketId"]}: {item.Rows[0]["Title"]}";
                                row["TicketId"] = item.Rows[0]["TicketId"];
                            }
                            else
                                row["CardToolTip"] = row[DatabaseObjects.Columns.TicketId];
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                }
            }

            return dtModuleTasks;
        }

        private void GenerateTaskData()
        {
            DataTable pendingTaskData = getDataSource("PendingTask");
            if (pendingTaskData != null && pendingTaskData.Rows.Count > 0)
            {
                pendingTaskData.DefaultView.Sort = "DueDate";
                pendingTaskData = pendingTaskData.DefaultView.ToTable();
                pendingTaskData.Columns.Add("AgeText", typeof(string));
                pendingTaskData.Columns.Add("Age", typeof(int));
                pendingTaskData.Columns.Add("Color", typeof(string));

                //completedTaskData = getFilteredData(completedTaskData);

                DataView dv = pendingTaskData.DefaultView;
                dv.Sort = "Age desc";

                //CardView.DataSource = dv.ToTable();
                //CardView.DataBind();
                AllTasks = (from DataRow dr in dv.ToTable().Rows
                            select new TaskData()
                            {
                                DueDate = dr["DueDate"] != null && Convert.ToDateTime(dr["DueDate"]) != DateTime.MinValue ? Convert.ToDateTime(dr["DueDate"]).ToString("MM/dd/yyyy") : String.Empty,
                                Duration = dr["Duration"] != null ? dr["Duration"].ToString() : string.Empty,
                                TicketId = dr["TicketId"] != null ? dr["TicketId"].ToString() : string.Empty,
                                EditLink = this.GenerateEditLink(dr),
                                DeleteLink = this.GenerateDeleteLink(dr),
                                MarkAsCompleteLink = this.GenerateMarkAsCompleteLink(dr),
                                TitleText = this.GetTitleText(dr),
                                AgeText = Convert.ToDateTime(dr["DueDate"]) != DateTime.MinValue ? dr["AgeText"].ToString() : string.Empty,
                                Color = dr["Color"].ToString(),
                                ToolTip = dr["CardToolTip"].ToString(),
                                PercentComplete = !string.IsNullOrWhiteSpace(dr["PercentComplete"]?.ToString()) ? Convert.ToDouble(dr["PercentComplete"].ToString()) : 0,
                            }).ToList();
                NotStartedTask = AllTasks.Where(o => o.PercentComplete == 0.0).ToList();
                OnGoingTask = AllTasks.Where(o => o.PercentComplete > 0 && o.PercentComplete < 100).ToList();
            }

            DataTable completedTaskData = getDataSource("CompletedTask");
            if (completedTaskData != null && completedTaskData.Rows.Count > 0)
            {
                completedTaskData.DefaultView.Sort = "DueDate";
                completedTaskData = completedTaskData.DefaultView.ToTable();
                completedTaskData.Columns.Add("AgeText", typeof(string));
                completedTaskData.Columns.Add("Age", typeof(int));
                completedTaskData.Columns.Add("Color", typeof(string));

                //completedTaskData = getFilteredData(completedTaskData);

                DataView dv = completedTaskData.DefaultView;
                dv.Sort = "Age desc";

                //CardView.DataSource = dv.ToTable();
                //CardView.DataBind();
                AllTasks = (from DataRow dr in dv.ToTable().Rows
                            select new TaskData()
                            {
                                DueDate = dr["CompletionDate"] != null && Convert.ToDateTime(dr["CompletionDate"]) != DateTime.MinValue ? Convert.ToDateTime(dr["CompletionDate"]).ToString("MM/dd/yyyy") : String.Empty,
                                Duration = dr["Duration"] != null ? dr["Duration"].ToString() : string.Empty,
                                TicketId = dr["TicketId"] != null ? dr["TicketId"].ToString() : string.Empty,
                                EditLink = this.GenerateEditLink(dr),
                                DeleteLink = this.GenerateDeleteLink(dr),
                                MarkAsCompleteLink = this.GenerateMarkAsCompleteLink(dr),
                                TitleText = this.GetTitleText(dr),
                                AgeText = dr["AgeText"].ToString(),
                                Color = dr["Color"].ToString(),
                                ToolTip = dr["CardToolTip"].ToString(),
                                PercentComplete = !string.IsNullOrWhiteSpace(dr["PercentComplete"]?.ToString()) ? Convert.ToDouble(dr["PercentComplete"].ToString()) : 0,
                            }).ToList();
                CompletedTask = AllTasks.ToList();
            }
        }
        public string GenerateEditLink(DataRow dr)
        {
            string color = dr["Color"].ToString();
            string ticketID = dr[DatabaseObjects.Columns.TicketId].ToString();
            string taskId = Convert.ToString(dr[DatabaseObjects.Columns.ID]);
            string ItemOrder = Convert.ToString(dr[DatabaseObjects.Columns.ItemOrder]);
            string TableName = Convert.ToString(dr[DatabaseObjects.Columns.TableName]);
            //string ToolTip = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, "CardToolTip"));
            string contactId = "0";

            //if (color == "Middle")
            //{
            //    e.Card.BorderColor = Color.Orange;
            //    e.Card.CssClass = "PendingOrange col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            //}

            //else if (color == "Low")
            //{
            //    e.Card.BorderColor = Color.DarkGray;
            //    e.Card.CssClass = "PendingGrey col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            //}
            //else if (color == "High")
            //{
            //    e.Card.BorderColor = Color.Red;
            //    e.Card.CssClass = "PendingRed col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            //}
            //e.Card.Style.Add("cursor", "pointer");
            if (!string.IsNullOrEmpty(ticketID))
            {

                string module = uHelper.getModuleNameByTicketId(ticketID);
                string url = "/Layouts/ugovernit/delegatecontrol.aspx";
                string title = string.Empty;
                // Added condition, as we are fetching Tasks from ModuleStageConstraints table (Stage Exist Criteria)
                if (TableName == DatabaseObjects.Tables.ModuleStageConstraints)
                {

                    string param = $"module={module}&ticketId={ticketID}&conditionType=ModuleTaskCT&moduleStage={Convert.ToString(dr[DatabaseObjects.Columns.StageStep])}&taskID={taskId}&type=ExistingConstraint&viewType=0";
                    title = ticketID + ": " + Convert.ToString(dr[DatabaseObjects.Columns.Title]);
                    return string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', '800px', '600px', 0, '{3}')", UGITUtility.GetAbsoluteURL(url) + "?control=modulestagetask", param, title, UGITUtility.GetAbsoluteURL(url));
                }
                else if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    title = ticketID + ": " + Convert.ToString(dr[DatabaseObjects.Columns.Title]);
                    return string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit));
                }
                else
                {
                    string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                    //title = $" Edit task for {ticketID}";
                    title = ticketID + ": " + Convert.ToString(dr[DatabaseObjects.Columns.Title]);
                    return string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url));
                }
            }
            else
            {

                if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    string title = Convert.ToString(dr[DatabaseObjects.Columns.Title]);
                    return string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit));
                }
                return string.Empty;
            }
        }

        public string GenerateDeleteLink(DataRow dr)
        {
            return string.Format("deleteTask('{0}','{1}','{2}')", dr[DatabaseObjects.Columns.TicketId].ToString(),
                Convert.ToString(dr[DatabaseObjects.Columns.ID]), Convert.ToString(dr[DatabaseObjects.Columns.TableName]));
        }
        public string GenerateMarkAsCompleteLink(DataRow dr) 
        {
            return string.Format("doTaskComplete('{0}','{1}','{2}')", dr[DatabaseObjects.Columns.TicketId].ToString(),
                Convert.ToString(dr[DatabaseObjects.Columns.ID]), Convert.ToString(dr[DatabaseObjects.Columns.TableName]));
        }
        public string GetTitleText(DataRow dr)
        {
            int Age = 0;
            if (dr["DueDate"] != null)
            {
                Age = ((Convert.ToDateTime(dr["DueDate"]) - DateTime.Now).Days);
            }
            dr["AgeText"] = $"{Age} days";
            if (Age < 0)
            {
                dr["Color"] = "red";
                //return "Stuff | Should Done Earlier";
            }
            else if (Age >= 0)
            {
                if (Age < 3)
                {
                    dr["Color"] = "green";
                    //return "Still Have Time";
                }
                else
                {
                    dr["Color"] = "orange";
                    //return "Plenty of Time";
                }
            }
            return dr[DatabaseObjects.Columns.Title] != null ? dr[DatabaseObjects.Columns.Title].ToString() : string.Empty;
        }


        public class TaskData
        {
            public string DueDate { get; set; }
            public string Duration { get; set; }
            public string TicketId { get; set; }
            public string AgeText { get; set; }
            public string Color { get; set; }
            public double PercentComplete { get; set; }
            public string EditLink { get; set; }
            public string DeleteLink { get; set; }
            public string TitleText { get; set; }
            public string ToolTip { get; set; }
            public string MarkAsCompleteLink { get; set; }

        }
    }
}
