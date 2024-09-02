using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT;
using System.Data;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System.Web;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Web
{
    public partial class CustomTaskCalendarControl : UserControl
    {
        public string ModuleName { get; set; }
        public string ProjectPublicId { get; set; }
        public int UserId { get; set; }
        UGITTaskManager TaskManager;
        object lastInsertedTaskId;
        UserProfile CurrentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            CurrentUser = HttpContext.Current.CurrentUser();
            calendarTaskView.Storage.Appointments.AutoRetrieveId = true;
            if (!Page.IsPostBack) 
            { 
                DateTime minStartDate = new DateTime();
                if (string.IsNullOrEmpty(Request["minStartDate"]) ||
                !DateTime.TryParse(Convert.ToString(Request["minStartDate"]), out minStartDate))
                    minStartDate = DateTime.Now; 
                if (minStartDate == DateTime.MinValue)
                    minStartDate = DateTime.Now;
                calendarTaskView.Start = UGITUtility.FirstDayOfMonth(minStartDate);
            }
            //string urlParam = "Sched=True&control=taskedit&moduleName=" + ModuleName + "&ticketId=" + ProjectPublicId;
            //string editTaskUrl = UGITUtility.ToAbsoluteUrl("/Layouts/uGovernIT/DelegateControl.aspx?" + urlParam);
            //calendarTaskView.OptionsForms.AppointmentFormTemplateUrl = editTaskUrl;
        }

        //private void BuildCustomTaskCalendarView()
        //{
        //    string pageType = string.Empty;
        //    SPWeb spWeb = SPContext.Current.Web;
        //    SPCalendarItemCollection calendarItems = new SPCalendarItemCollection();
        //    string viewType = ddlViewType.SelectedValue;
        //    DataTable taskrows = null;
        //    if (ModuleName == "ALL")
        //    {
        //        taskrows = TaskCache.GetAllTasks(); // get all task..
        //    }
        //    else if (ModuleName != null && ModuleName != "" && ModuleName != "ALL" && ProjectPublicId != null && ProjectPublicId != "")
        //    {
        //        DataTable tempdata = TaskCache.GetAllTasksByProjectID(ModuleName, ProjectPublicId);

        //        if (tempdata.Rows.Count > 0)
        //        {
        //            var rowcount = tempdata.AsEnumerable()
        //                        .Where(i => i.Field<Int32>("UGITChildCount") == 0)
        //                        .Count();
        //            if (rowcount > 0)
        //            {
        //                taskrows = tempdata.AsEnumerable()
        //                            .Where(i => i.Field<Int32>("UGITChildCount") == 0)
        //                            .CopyToDataTable();
        //            }
        //        }
        //    }
        //    else if (ModuleName != null && ModuleName != "" && ModuleName != "ALL")
        //    {
        //        taskrows = TaskCache.GetAllTasks(ModuleName); // get all task of module
        //    }
        //    else
        //    {
        //        taskrows = TaskCache.GetAllTasksByUser(UserId); // get all task for user.
        //        pageType = "user";
        //    }

        //    List<string> tooltipJson = new List<string>();
        //    if (taskrows != null)
        //    {
        //        foreach (DataRow row in taskrows.Rows)
        //        {
        //            SPCalendarItem item = new SPCalendarItem();
        //            item.ItemID = string.Empty;
        //            item.StartDate = Convert.ToDateTime(row[DatabaseObjects.Columns.StartDate]);
        //            if (row[DatabaseObjects.Columns.DueDate].ToString() != "")
        //            {
        //                item.EndDate = Convert.ToDateTime(row[DatabaseObjects.Columns.DueDate]);

        //            }
        //            else
        //            {
        //                item.EndDate = Convert.ToDateTime(row[DatabaseObjects.Columns.StartDate]);
        //            }
        //            item.hasEndDate = false;
        //            string title = uHelper.ReplaceInvalidCharsInURL(Convert.ToString(row[DatabaseObjects.Columns.Title]).Trim());
        //            string ticketTitle_100chars = uHelper.TruncateWithEllipsis(title, 100, string.Empty);
        //            string lnk = string.Empty;

        //            if (SPContext.Current.Web.ServerRelativeUrl == "/")
        //            {
        //                if (pageType == "user")
        //                {
        //                    lnk = string.Format("{0}?projectid={1}&taskID={3}&moduleName={2}&Control=edittask&taskType=task&viewtype=1", "/_layouts/15/ugovernit/DelegateControl.aspx", row[DatabaseObjects.Columns.ProjectID], row[DatabaseObjects.Columns.ModuleName], row[DatabaseObjects.Columns.Id]);
        //                }
        //                else
        //                {
        //                    lnk = string.Format("{0}?projectid={1}&taskID={3}&moduleName={2}&Control=edittask", "/_layouts/15/ugovernit/DelegateControl.aspx", row[DatabaseObjects.Columns.ProjectID], row[DatabaseObjects.Columns.ModuleName], row[DatabaseObjects.Columns.Id]);
        //                }
        //            }
        //            else
        //            {
        //                if (pageType == "user")
        //                {
        //                    lnk = string.Format("{0}?projectid={1}&taskID={3}&moduleName={2}&Control=edittask&taskType=task&viewtype=1", SPContext.Current.Web.ServerRelativeUrl + "/_layouts/15/ugovernit/DelegateControl.aspx", row[DatabaseObjects.Columns.ProjectID], row[DatabaseObjects.Columns.ModuleName], row[DatabaseObjects.Columns.Id]);
        //                }
        //                else
        //                {
        //                    lnk = string.Format("{0}?projectid={1}&taskID={3}&moduleName={2}&Control=edittask", SPContext.Current.Web.ServerRelativeUrl + "/_layouts/15/ugovernit/DelegateControl.aspx", row[DatabaseObjects.Columns.ProjectID], row[DatabaseObjects.Columns.ModuleName], row[DatabaseObjects.Columns.Id]);
        //                }
        //            }

        //            if (viewType == "0" || viewType == "1")
        //                title = uHelper.TruncateWithEllipsis(title, 20);

        //            StringBuilder tooltip = new StringBuilder();
        //            tooltip.AppendFormat("<b>{0}</b>", uHelper.ReplaceInvalidCharsInURL(Convert.ToString(row[DatabaseObjects.Columns.Title])));


        //            tooltip.AppendFormat("<br/>");
        //            tooltip.AppendFormat("<b>Status:</b> {0}", Convert.ToString(row[DatabaseObjects.Columns.Status]));
        //            tooltip.AppendFormat("<br/>");
        //            if (row[DatabaseObjects.Columns.DueDate].ToString() != "")
        //            {
        //                tooltip.AppendFormat("<b>Due Date:</b> {0}", Convert.ToDateTime(row[DatabaseObjects.Columns.DueDate]).ToString("MMM-dd-yyyy"));
        //                tooltip.AppendFormat("<br/>");
        //            }
        //            tooltip.AppendFormat("<b>{0}:</b> {1}", Convert.ToString(row[DatabaseObjects.Columns.ProjectID]), Convert.ToString(row["projectTitle"]));
        //            tooltipJson.Add(string.Format("{3}\"module\":\"{0}\",\"taskid\":\"{1}\",\"tasktitle\":\"{5}\",\"tooltip\":\"{2}\"{4}", row[DatabaseObjects.Columns.ModuleName], row[DatabaseObjects.Columns.Id], tooltip.ToString(), '{', '}', uHelper.ReplaceInvalidCharsInURL(Convert.ToString(row[DatabaseObjects.Columns.Title]))));
        //            // item.Title = Convert.ToString(row[DatabaseObjects.Columns.ProjectID]) + "\n" + "(" + title + ")" + str;
        //            item.Title = title;
        //            item.DisplayFormUrl = string.Format("{0}", lnk);
        //            item.IsAllDayEvent = true;
        //            item.IsRecurrence = false;
        //            item.CalendarType = Convert.ToInt32(SPCalendarType.Gregorian);
        //            if (item.EndDate < DateTime.Now && Convert.ToString(row[DatabaseObjects.Columns.Status]) != "Completed")
        //            {
        //                item.BackgroundColorClassName = "backgroundColorRed";
        //            }
        //            else
        //            {
        //                item.BackgroundColorClassName = "backgroundNormalTask";
        //            }
        //            item.Description = Convert.ToString(row["Title"]);
        //            calendarItems.Add(item);
        //        }
        //    }

        //    ASPxHFCalendar.Set("tooltipdata", string.Format("[{0}]", string.Join(",", tooltipJson.ToArray())));
        //    calendarTaskView.EnableViewState = true;
        //    // Set Calendar Type 
        //    calendarTaskView.ViewType = GetCalendarType(viewType);
        //    calendarTaskView.EnableV4Rendering = false;
        //    // Binds a datasource to the SPCalendarView 
        //    calendarTaskView.DataSource = calendarItems;
        //    calendarTaskView.DataBind();
        //}

        protected static string GetCalendarType(string type)
        {
            if (type == null)
                type = string.Empty;
            switch (type.ToLower())
            {
                case "1":
                    return "week";
                case "2":
                    return "day";
                case "3":
                    return "timeline";
                default:
                    return "month";
            }
        }

        #region #setappointment
        CustomTaskDataSource objectInstance;
        // Obtain the ID of the last inserted appointment from the object data source and assign it to the appointment in the ASPxScheduler storage.
        protected void calendarTaskView_AppointmentRowInserted(object sender, DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertedEventArgs e)
        {
            e.KeyFieldValue = this.objectInstance.ObtainLastInsertedId();
        }
        protected void taskDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {
            this.objectInstance = new CustomTaskDataSource(GetCustomEvents());
            e.ObjectInstance = this.objectInstance;
        }

        CustomTaskList GetCustomEvents()
        {
            CustomTaskList events = Session["CustomTaskListData"] as CustomTaskList;
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            if (events == null)
            {
                events = new CustomTaskList();
                List<string> tooltipJson = new List<string>();
                List<UGITTask> tasks = new List<UGITTask>();
                if(ModuleName == "MyTask")
                {
                    string[] mytaskmodules = { "NPR", "SVCConfig", "Template" };
                    tasks = TaskManager.GetOpenedTasksByUserList(CurrentUser.Id, true, mytaskmodules);
                }
                else
                {
                    tasks = TaskManager.LoadByProjectID(ModuleName, ProjectPublicId);
                }
                foreach(UGITTask task in tasks)
                {
                    events.Add(task);
                }
                Session["CustomTaskListData"] = events;
            }
            return events;
        }
        #endregion #setappointment
        // The following code is unnecessary if the ASPxScheduler.Storage.Appointments.AutoRetrieveId option is TRUE.
        protected void calendarTaskView_AppointmentsInserted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            SetAppointmentId(sender, e);
        }

        protected void taskDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.lastInsertedTaskId = e.ReturnValue;
        }
        void SetAppointmentId(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage storage = (DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage)sender;
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Objects[0];
            storage.SetAppointmentId(apt, this.lastInsertedTaskId);
        }

        protected void calendarTaskView_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
        {
            ASPxSchedulerPopupMenu menu = e.Menu;
            if (e.Menu.MenuId == SchedulerMenuItemId.AppointmentMenu)
            {
                e.Menu.Visible = false;
                DevExpress.Web.MenuItemCollection menuItems = menu.Items;
                foreach (DevExpress.Web.MenuItem m in menuItems)
                {
                    if (m.Name == "OpenAppointment")
                        m.Visible = false;
                    else if (m.Name == "EditSeries")
                        m.Visible = false;
                    else if (m.Name == "RestoreOccurrence")
                        m.Visible = false;
                    else if (m.Name == "StatusSubMenu")
                        m.Visible = false;
                    else if (m.Name == "LabelSubMenu")
                        m.Visible = false;
                    else if (m.Name == "DeleteAppointment")
                        m.Visible = false;
                }
            }
                
            if (e.Menu.MenuId == SchedulerMenuItemId.DefaultMenu)
            {
                DevExpress.Web.MenuItemCollection menuItems = menu.Items;
                foreach (DevExpress.Web.MenuItem m in menuItems)
                {
                    if (m.GroupName == "TSL")
                        m.Visible = false;
                    if (m.Name == "NewAppointment") { 
                        m.Visible = true;
                        m.Text = "New Task";
                    }
                    else if (m.Name == "NewAllDayEvent")
                        m.Visible = false;
                    else if (m.Name == "NewRecurringAppointment")
                        m.Visible = false;
                    else if (m.Name == "NewRecurringEvent")
                        m.Visible = false;
                    else if (m.Name == "GotoToday")
                        m.Visible = false;
                    else if (m.Name == "GotoDate")
                        m.Visible = false;
                    else if (m.Name == "SwitchViewMenu")
                        m.Visible = false;
                    else if (m.Name == "GotoThisDay")
                        m.Visible = false;
                }
            }
        }

        protected void taskDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

        }
    }
}
