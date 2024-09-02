using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class TaskUserControl : UserControl
    {
        public string landingPageUrl = string.Empty;
        private ApplicationContext _context = null;
        FieldConfigurationManager fmanger = null;
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

        private ModuleViewManager _moduleViewManager = null;
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        private ConfigurationVariableManager _configVariableMGR = null;
        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configVariableMGR == null)
                {
                    _configVariableMGR = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configVariableMGR;
            }
        }

        public string absoluteurl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx");
        public string ViewMode { get; set; }
        public string type { get; set; }
        //public string cardview { get; set; }
        //public string gantview { get; set; }
        //public string calendar { get; set; }
        protected string ConstraintTaskUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagetask");
        protected string ConstraintRuleUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagerule");
        public string sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
        protected override void OnInit(EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            fmanger = new FieldConfigurationManager(ApplicationContext);

            UserProfile user = HttpContext.Current.CurrentUser();
            if (user != null)
            {
                //var applicationContext = HttpContext.Current.GetManagerContext();
                if (ApplicationContext != null && !string.IsNullOrEmpty(user.UserRoleId))
                {
                    landingPageUrl = UGITUtility.GetAbsoluteURL(new LandingPagesManager(ApplicationContext).GetLandingPageById(user.UserRoleId));
                }
            }

            if (Request.QueryString["Viewmode"] != null)
            {
                ViewMode = Convert.ToString(Request.QueryString["Viewmode"]);
            }
            if (Request.QueryString["type"] != null)
            {
                type = Convert.ToString(Request.QueryString["type"]);
            }

            if (!IsPostBack)
            {
                //grid.Visible = false;
                //dvViewChangeCardPending.Visible = false;
                CardView.Visible = true;
                CardViewRecentTask.Visible = true;
                //taskScheduler.Visible = false;
                headCardView.Visible = true;
                headCardViewRecentTask.Visible = true;
                // homeCardView.Visible = true;

            }
            var HomecardView = Page.Master.Master.FindControlRecursive("UserDashboardCardView") as ASPxCardView;
            if (HomecardView != null)
                HomecardView.CssClass = "cardView-wrapper cardViewTable-wrap";
            BindAspxGridView();

            if (ViewMode == "gridview")
            {
                grid.Visible = true;
                tabs.Visible = true;
                CardView.Visible = false;
                CardViewRecentTask.Visible = false;
                //taskScheduler.Visible = false;
                headCardView.Visible = false;
                headCardViewRecentTask.Visible = false;
            }
            else if (ViewMode == "cardview")
            {
                grid.Visible = false;
                tabs.Visible = false;
                CardView.Visible = true;
                CardViewRecentTask.Visible = true;
                //taskScheduler.Visible = false;
                headCardView.Visible = true;
                headCardViewRecentTask.Visible = true;
            }
            else if (ViewMode == "calendar")
            {
                grid.Visible = false;
                tabs.Visible = false;
                CardView.Visible = false;
                CardViewRecentTask.Visible = false;
                //taskScheduler.Visible = true;
                headCardView.Visible = false;
                headCardViewRecentTask.Visible = false;
            }
        }

        private void BindAspxGridView()
        {
            grid.DataSource = getDataSource("PendingTask");
            grid.DataBind();

            //taskScheduler.DataSource = getDataSource(type);
            //taskScheduler.DataBind();
            //taskScheduler.AppointmentDataSource = getDataSource(type);
            //taskScheduler.DataBind();


            DataTable cardViewData = getDataSource("PendingTask");

            if (cardViewData != null && cardViewData.Rows.Count > 0)
            {
                cardViewData.DefaultView.Sort = "DueDate";
                cardViewData = cardViewData.DefaultView.ToTable();
                cardViewData.Columns.Add("AgeText", typeof(string));
                cardViewData.Columns.Add("Age", typeof(int));
                cardViewData.Columns.Add("Color", typeof(string));

                cardViewData = getFilteredData(cardViewData);

                DataView dv = cardViewData.DefaultView;
                dv.Sort = "Age desc";

                CardView.DataSource = dv.ToTable();
                CardView.DataBind();
            }
            else
            {
                //headCardView.Visible = false;
                //CardView.Visible = false;
            }


            cardViewData = getDataSource("CompletedTask");

            if (cardViewData != null && cardViewData.Rows.Count > 0)
            {
                cardViewData.Columns.Add("AgeText", typeof(string));
                cardViewData.Columns.Add("Age", typeof(int));
                cardViewData.Columns.Add("Color", typeof(string));

                cardViewData.DefaultView.Sort = "CompletionDate DESC";
                cardViewData = cardViewData.DefaultView.ToTable();
                cardViewData = getFilteredDataCom(cardViewData);

                CardViewRecentTask.DataSource = cardViewData;
                CardViewRecentTask.DataBind(); 
            }
            else
            {
                headCardViewRecentTask.Visible = false;
                CardViewRecentTask.Visible = false;
            }
        }

        public DataTable getDataSource(string type)
        {
            int tasksDisplayDuration = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue("CompletedTasksDisplayDuration"), 30);
            var ownerColumn = DatabaseObjects.Columns.AssignedTo;
            var Status = DatabaseObjects.Columns.Status;
            var TaskStatus = DatabaseObjects.Columns.TaskStatus;
            var ActivityStatus = DatabaseObjects.Columns.ActivityStatus;
            var userid = _context.CurrentUser.Id;
            var actualstatus = "Completed";
            var uManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var role = uManager.GetUserRoles(userid).Select(x => x.Id).ToList();

            var sbQuery = new StringBuilder();

            sbQuery.Append($"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' AND {DatabaseObjects.Columns.Deleted} <> 'True' ");

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
            CRMActivitiesManager CRMActivitiesManager = new CRMActivitiesManager(ApplicationContext);
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
                        item = GetTableDataManager.GetTableData(moduleTable, $"{DatabaseObjects.Columns.TicketId} = '{activity.ContactLookup}' and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'", "TicketId,FirstName,LastName", null);
                        if (item != null && item.Rows.Count > 0)
                            row["CardToolTip"] = $"{item.Rows[0]["TicketId"]}: {item.Rows[0]["FirstName"]} {item.Rows[0]["LastName"]}";
                        else
                            row["CardToolTip"] = row[DatabaseObjects.Columns.Title];
                    }
                }
                else
                {
                    try
                    {
                        string moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                        if(!string.IsNullOrEmpty(moduleName)){
                            moduleTable = modules.FirstOrDefault(x => x.ModuleName == moduleName).ModuleTable;
                            item = GetTableDataManager.GetTableData(moduleTable, $"{DatabaseObjects.Columns.TicketId} = '{Convert.ToString(row[DatabaseObjects.Columns.TicketId])}' and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'", "TicketId,Title", null);
                            if (item != null && item.Rows.Count > 0)
                                row["CardToolTip"] = $"{item.Rows[0]["TicketId"]}: {item.Rows[0]["Title"]}";
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

        public DataTable getFilteredData(DataTable cardViewData)
        {
            foreach (DataRow dr in cardViewData.Rows)
            {
                int Age = UGITUtility.GetDueInValue(Convert.ToDateTime(dr["DueDate"]));

                if (Age < 0)
                {
                    dr["AgeText"] = $" {Math.Abs(Age)} days late";
                    dr["Age"] = Age;

                    dr["color"] = "Low";
                }
                else if (Age > 0)
                {
                    if (Age == 1)
                    {
                        dr["AgeText"] = $"Due in {Math.Abs(Age)} day";
                    }
                    else
                    {
                        dr["AgeText"] = $"Due in {Math.Abs(Age)} days";
                    }
                    dr["Age"] = Age;
                    if (Age <= 3)
                    {
                        dr["color"] = "Middle";
                    }
                    else
                    {
                        dr["color"] = "High";
                    }
                }
                else if (Age == 0)
                {
                    dr["AgeText"] = $"Today";
                    dr["color"] = "Middle";
                }
            }

            return cardViewData;
        }

        public DataTable getFilteredDataCom(DataTable cardViewData)
        {
            foreach (DataRow dr in cardViewData.Rows)
            {
                if (dr["CompletionDate"] != DBNull.Value)
                {
                    int Age = UGITUtility.GetDueInValue(Convert.ToDateTime(dr["CompletionDate"]));
                    if (Convert.ToDateTime(dr["CompletionDate"]).Date == DateTime.Now.Date)
                    {
                        dr["AgeText"] = $" Today";
                    }
                    else
                    {
                        dr["AgeText"] = Convert.ToDateTime(dr["CompletionDate"]).ToString("MM/dd/yyyy");
                    }
                }
            }

            return cardViewData;
        }
        protected void CardView_HtmlCardPrepared(object sender, DevExpress.Web.ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            string color = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, "Color"));
            string ticketID = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string taskId = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ID));
            string ItemOrder = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ItemOrder));
            string TableName = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TableName));
            string ToolTip = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, "CardToolTip"));
            string contactId = "0";

            if (color == "Middle")
            {
                e.Card.BorderColor = Color.Orange;
                e.Card.CssClass = "PendingOrange col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            }

            else if (color == "Low")
            {
                e.Card.BorderColor = Color.DarkGray;
                e.Card.CssClass = "PendingGrey col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            }
            else if (color == "High")
            {
                e.Card.BorderColor = Color.Red;
                e.Card.CssClass = "PendingRed col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
            }
            e.Card.Style.Add("cursor", "pointer");
            if (!string.IsNullOrEmpty(ticketID))
            {

                string module = uHelper.getModuleNameByTicketId(ticketID);
                string url = "/Layouts/ugovernit/delegatecontrol.aspx";
                string title = string.Empty;
                // Added condition, as we are fetching Tasks from ModuleStageConstraints table (Stage Exist Criteria)
                if (TableName == DatabaseObjects.Tables.ModuleStageConstraints)
                {
                   
                    string param = $"module={module}&ticketId={ticketID}&conditionType=ModuleTaskCT&moduleStage={ Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.StageStep)) }&taskID={taskId}&type=ExistingConstraint&viewType=0";
                    title = ticketID + ": " + Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', '800px', '600px', 0, '{3}')", UGITUtility.GetAbsoluteURL(url) + "?control=modulestagetask", param, title, UGITUtility.GetAbsoluteURL(url)));
                }
                else if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    title = ticketID + ": " + Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit)));
                }
                else
                {
                    string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                    //title = $" Edit task for {ticketID}";
                    title = ticketID + ": " + Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                }
            }
            else
            {

                if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    string title = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit)));
                }
            }
            e.Card.ToolTip = ToolTip;
        }

        protected void CardViewRecentTask_HtmlCardPrepared(object sender, DevExpress.Web.ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            string color = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, "Color"));
            string ticketID = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string taskId = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ID));
            string ItemOrder = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.ItemOrder));
            string TableName = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TableName));
            string ToolTip = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, "CardToolTip"));
            string contactId = "0";
            if (!string.IsNullOrEmpty(ticketID))
            {
                e.Card.BorderColor = Color.SkyBlue;
                e.Card.CssClass = "CompletedBlue col-md-2 col-sm-3 col-xs-12 noPadding colFormd";

                string module = uHelper.getModuleNameByTicketId(ticketID);
                string url = "/Layouts/ugovernit/delegatecontrol.aspx";
                string title = string.Empty;

                // Added condition, as we are fetching Tasks from ModuleStageConstraints table (Stage Exist Criteria)
                if (TableName == DatabaseObjects.Tables.ModuleStageConstraints)
                {
                    //string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                    string param = $"module={module}&ticketId={ticketID}&conditionType=ModuleTaskCT&moduleStage={ Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.StageStep)) }&taskID={taskId}&type=ExistingConstraint&viewType=0";
                    title = ticketID + ": " + Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', '800px', '600px', 0, '{3}')", UGITUtility.GetAbsoluteURL(url) + "?control=modulestagetask", param, title, UGITUtility.GetAbsoluteURL(url)));
                }
                else if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    title = ticketID + ": " + Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit)));
                }
                else
                {
                    string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                    //title = $" Edit task for {ticketID}";
                    title = ticketID + ": " + Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                }
                e.Card.Style.Add("cursor", "pointer");
            }
            else
            {
                if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                {
                    e.Card.BorderColor = Color.SkyBlue;
                    e.Card.CssClass = "CompletedBlue col-md-2 col-sm-3 col-xs-12 noPadding colFormd";
                    string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                    string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                    string title = Convert.ToString(CardViewRecentTask.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
                    e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit)));
                    e.Card.Style.Add("cursor", "pointer");
                }
            }
            e.Card.ToolTip = ToolTip;
        }

        protected void CardView_CardLayoutCreated(object sender, DevExpress.Web.ASPxCardViewCardLayoutCreatedEventArgs e)
        {
        }

        protected void CardView_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            var data = e.LayoutData;
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = grid.GetDataRow(e.VisibleIndex);
                if (row != null)
                {
                    string ticketID = Convert.ToString(row[DatabaseObjects.Columns.TicketId]);
                    string taskId = Convert.ToString(row[DatabaseObjects.Columns.ID]);
                    string module = uHelper.getModuleNameByTicketId(ticketID);
                    string TableName = Convert.ToString(row[DatabaseObjects.Columns.TableName]);
                    //string ActivityId = Convert.ToString(row["ActivityId"]);
                    string contactId = "0";
                    string url = "/Layouts/ugovernit/delegatecontrol.aspx";

                    // Added condition, as we are fetching Tasks from ModuleStageConstraints table (Stage Exist Criteria)
                    if (TableName == DatabaseObjects.Tables.ModuleStageConstraints)
                    {
                        //string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                        string param = $"module={module}&ticketId={ticketID}&conditionType=ModuleTaskCT&moduleStage={ Convert.ToString(row[DatabaseObjects.Columns.StageStep]) }&taskID={taskId}&type=ExistingConstraint&viewType=0";
                        string title = $" Edit task for {ticketID}";
                        e.Row.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', '800px', '600px', 0, '{3}')", UGITUtility.GetAbsoluteURL(url) + "?control=modulestagetask", param, title, UGITUtility.GetAbsoluteURL(url)));
                    }
                    else if (TableName.EqualsIgnoreCase(DatabaseObjects.Tables.CRMActivities))
                    {
                        string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control=activitiesaddedit";
                        string param = string.Format("&ID={0}&contactID={1}&ticketID={2}", taskId, contactId, ticketID);
                        string title = $"Activities - Edit Item for {ticketID}";
                        e.Row.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(absoluteUrlEdit), param, title, UGITUtility.GetAbsoluteURL(absoluteUrlEdit)));
                    }
                    else
                    {
                        string param = string.Format("projectID={0}&ticketId={0}&taskID={1}&moduleName={2}&control=taskedit&taskType=task", ticketID, taskId, module);
                        string title = $" Edit task for {ticketID}";
                        e.Row.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 60, 80, 0, '{3}')", UGITUtility.GetAbsoluteURL(url), param, title, UGITUtility.GetAbsoluteURL(url)));
                    }
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            grid.Visible = true;
            tabs.Visible = true;
            CardView.Visible = false;
            CardViewRecentTask.Visible = false;
            //taskScheduler.Visible = false;
            headCardView.Visible = false;
            headCardViewRecentTask.Visible = false;
            dvViewChangeCardPending.Visible = true;
        }

        protected void viewChange_Click(object sender, ImageClickEventArgs e)
        {
            grid.Visible = true;
            tabs.Visible = true;
            CardView.Visible = false;
            CardViewRecentTask.Visible = false;
            //taskScheduler.Visible = false;
            headCardView.Visible = false;
            headCardViewRecentTask.Visible = false;
            dvViewChangeCardPending.Visible = true;
        }

        protected void ViewChangeCardPending_Click(object sender, ImageClickEventArgs e)
        {
            grid.Visible = false;
            tabs.Visible = false;
            CardView.Visible = true;
            CardViewRecentTask.Visible = true;
            //taskScheduler.Visible = false;
            viewChange.Visible = true;
            headCardView.Visible = true;
            headCardViewRecentTask.Visible = true;
            dvViewChangeCardPending.Visible = false;
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                if (!string.IsNullOrEmpty(value))
                {
                    e.Cell.Text = value;
                }
            }
            if(e.DataColumn.FieldName == DatabaseObjects.Columns.TicketId)
            {
                DataRow row = grid.GetDataRow(e.VisibleIndex);
                UGITModule module = ModuleViewManager.LoadByName(uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(row["Ticketid"])));
                if (module != null)
                {
                    string viewUrl = module.StaticModulePagePath;
                    string values = UGITUtility.ObjectToString(e.GetValue(e.DataColumn.FieldName));
                    if (row != null)
                    {
                        // e.Cell.Text = $"<span onclick=\"{string.Format("javascript:window.parent.UgitOpenPopupDialog(\'{0}\',\'{1}\',\'{2}\',\'{4}\',\'{5}\', 0, \'{3}\')", viewUrl, string.Format("TicketId={0}", row["Ticketid"]), row["CardToolTip"], sourceURL, 90, 90)};javascript:event.stopPropagation();\">{row["CardToolTip"]}</span>"; //UGITUtility.ObjectToString(row["CardToolTip"]);
                        e.Cell.Text = $"<span style=\'color:#1b3f91;cursor:pointer;\' onclick=\"{string.Format("javascript:window.parent.UgitOpenPopupDialog(\'{0}\',\'{1}\',\'{2}\',\'{4}\',\'{5}\', 0)", viewUrl, string.Format("TicketId={0}", row["Ticketid"]), row["CardToolTip"], sourceURL, 90, 90)};javascript:event.stopPropagation();\">{row["CardToolTip"]}</span>";
                    }
                }
            }
        }

        protected void tab_TabClick(object source, TabControlCancelEventArgs e)
        {
            if(e.Tab.Name == "PendingTask")
                grid.DataSource = getDataSource("PendingTask");
            else if (e.Tab.Name == "CompletedTask")
                grid.DataSource = getDataSource("CompletedTask");

            grid.DataBind();
        }

        //protected void CardView_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        //{
        //    grid.Visible = true;
        //    CardView.Visible = false;
        //    CardViewRecentTask.Visible = false;
        //    //taskScheduler.Visible = false;
        //    headCardView.Visible = false;
        //    headCardViewRecentTask.Visible = false;
        //    dvViewChangeCardPending.Visible = true;
        //}

        //protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    grid.Visible = false;
        //    CardView.Visible = true;
        //    CardViewRecentTask.Visible = true;
        //    //taskScheduler.Visible = false;
        //    viewChange.Visible = true;
        //    headCardViewRecentTask.Visible = true;
        //    dvViewChangeCardPending.Visible = false;

        //}
    }
}