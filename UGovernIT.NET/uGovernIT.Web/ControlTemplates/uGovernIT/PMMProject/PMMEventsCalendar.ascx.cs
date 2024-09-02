using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Drawing;
//using DevExpress.XtraScheduler;
//using DevExpress.XtraScheduler.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class PMMEventsCalendar : UserControl
    {
        public string ProjectPublicID { get; set; }
        //string moduleName = "PMM";
        protected string ProjectId = string.Empty;
        ASPxSchedulerStorage Storage { get { return this.ASPxScheduler1.Storage; } }

        protected string ProjectTaskURL = UGITUtility.GetAbsoluteURL("/ControlTemplates/ugovernit/Task/edittask.aspx");
        public bool ReadOnly;
        PMMEventManager PMMEventManagerObj = new PMMEventManager(HttpContext.Current.GetManagerContext());
        EventCategoryViewManager EventCategoryManagerObj = new EventCategoryViewManager(HttpContext.Current.GetManagerContext()); 
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        TicketManager TicketManagerObj = new TicketManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            ASPxScheduler1.Storage.Appointments.Labels.Clear();
            AppointmentLabel lblDefault = ASPxScheduler1.Storage.Appointments.Labels.CreateNewLabel(0, string.Format("<div style='float:left;'> <span> None </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"),
                string.Format("<div style='float:left;'> <span> None </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"));
            ASPxScheduler1.Storage.Appointments.Labels.Add(lblDefault);
            
            DataTable dtlabel = EventCategoryManagerObj.GetDataTable();   // SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategoryQuery);
            if (dtlabel != null)
            {
                foreach (DataRow labelrow in dtlabel.Rows)
                {
                    System.Drawing.Color.FromName(Convert.ToString(labelrow[DatabaseObjects.Columns.UGITItemColor]));
                    AppointmentLabel lbl = ASPxScheduler1.Storage.Appointments.Labels.CreateNewLabel(labelrow[DatabaseObjects.Columns.ID], string.Format("<div style='float:left;'> <span> {0} </span> <span style='background-color:{1};border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>", 
                        Convert.ToString(labelrow[DatabaseObjects.Columns.Title]), 
                        Convert.ToString(labelrow[DatabaseObjects.Columns.UGITItemColor])), 
                        string.Format("<div style='float:left;'> <span> {0} </span> <span style='background-color:{1};border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>", 
                        Convert.ToString(labelrow[DatabaseObjects.Columns.Title]), 
                        Convert.ToString(labelrow[DatabaseObjects.Columns.UGITItemColor])));
                    ASPxScheduler1.Storage.Appointments.Labels.Add(lbl);
                }
            }
            appointmentDataSource.SelectMethod = "GetEventData";
            appointmentDataSource.UpdateMethod = "UpdateEvents";
            appointmentDataSource.InsertMethod = "InsertEvents";
            appointmentDataSource.DeleteMethod = "DeleteEvents";
            appointmentDataSource.TypeName = this.GetType().AssemblyQualifiedName;
            appointmentDataSource.ObjectCreating += appointmentDataSource_ObjectCreating;
            mapping(ASPxScheduler1);
            ASPxScheduler1.AppointmentDataSourceID = appointmentDataSource.ID;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxScheduler1.DataBind();
        }

        private void mapping(ASPxScheduler control)
        {
            ASPxSchedulerStorage storage = control.Storage;
            storage.Appointments.AutoRetrieveId = true;
            ASPxAppointmentMappingInfo mappings = storage.Appointments.Mappings;
            storage.BeginUpdate();
            try
            {
                mappings.AppointmentId = "ID";
                mappings.Start = "StartTime";
                mappings.End = "EndTime";
                mappings.Subject = "Subject";
                mappings.AllDay = "AllDay";
                mappings.Description = "Description";
                mappings.Label = "Label";
                mappings.Location = "Location";
                mappings.RecurrenceInfo = "RecurrenceInfo";
                mappings.ReminderInfo = "ReminderInfo";
                mappings.ResourceId = "OwnerId";
                mappings.Status = "Status";
                mappings.Type = "EventType";
            }
            finally
            {
                storage.EndUpdate();
            }
            ASPxAppointmentCustomFieldMappingCollection customMappings = storage.Appointments.CustomFieldMappings;
            customMappings.BeginUpdate();
            try
            {
                customMappings.Add(new DevExpress.XtraScheduler.AppointmentCustomFieldMapping("MeetingType", "MeetingType"));
            }
            finally
            {
                customMappings.EndUpdate();
            }
        }

        #region #appointmentformshowing
        protected void ASPxScheduler1_AppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {
            string id = Convert.ToString(e.Appointment.Id);

            if (id.Contains("ProjectTask"))
            {
                e.Cancel = true;
            }
            else
            {
                e.Container = new PMMEventViewTemplateContainer((ASPxScheduler)sender);
                e.Container.Caption = e.Container.Subject + " Event";
            }
        }
        #endregion #appointmentformshowing

        protected void appointmentDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }
        #region #beforeexecutecallbackcommand
        protected void ASPxScheduler1_BeforeExecuteCallbackCommand
                (object sender, SchedulerCallbackCommandEventArgs e)
        {
            if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
            {
                e.Command =
                    new PMMEventSaveCallbackCommand((ASPxScheduler)sender);
            }
        }
        #endregion #beforeexecutecallbackcommand

        public DataTable GetEventData()
        {
            return GetListData();
        }

        private DataTable GetListData()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                            new DataColumn("ID", typeof(object)),
                            new DataColumn("StartTime", typeof(DateTime)),
                            new DataColumn("EndTime", typeof(DateTime)),
                            new DataColumn("Subject",typeof(string)),
                            new DataColumn("AllDay", typeof(object)),
                            new DataColumn("Description",typeof(object)),
                            new DataColumn("Label", typeof(object)),
                            new DataColumn("Location",typeof(string)),
                            new DataColumn("RecurrenceInfo",typeof(object)),
                            new DataColumn("ReminderInfo",typeof(object)),
                            new DataColumn("OwnerId", typeof(object)),
                            new DataColumn("Status", typeof(object)),
                            new DataColumn("EventType",typeof(object)),
                            new DataColumn("MeetingType",typeof(string))
                          });

            // Get project internal ID
            List<string> viewFields = new List<string>();
            viewFields.Add(DatabaseObjects.Columns.Id);
            viewFields.Add(DatabaseObjects.Columns.TicketId);
            
            UGITModule module = ModuleManager.LoadByName(ModuleNames.PMM);
            DataRow projectItem = TicketManagerObj.GetByTicketID(module, ProjectPublicID); //Ticket.getCurrentTicket(moduleName, ProjectPublicID, viewFields, true);
            ProjectId = Convert.ToString(projectItem[DatabaseObjects.Columns.TicketId]);

            //SPQuery actualDQuery = new SPQuery();
            //actualDQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True' /><Value Type='Lookup'>{1}</Value></Eq></Where>",
            //                                    DatabaseObjects.Columns.TicketPMMIdLookup, ProjectId);
            
            List<PMMEvents> listPmmEvents = PMMEventManagerObj.Load(x=>x.PMMIdLookup == ProjectId);
            //List<PMMEvents> listPmmEvents = PMMEventManagerObj.Load(x => x.PMMIdLookup == Convert.ToString(projectItem[DatabaseObjects.Columns.ID]));
            DataTable dtPMMEvents = UGITUtility.ToDataTable<PMMEvents>(listPmmEvents);   // SPListHelper.GetDataTable(DatabaseObjects.Lists.PMMEvents, actualDQuery);
            if (dtPMMEvents != null && dtPMMEvents.Rows.Count > 0)
            {
                foreach (DataRow row in dtPMMEvents.Rows)
                {
                    int lableId = 0;
                    DataTable dtlabel = EventCategoryManagerObj.GetDataTable();  // SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategoryQuery);
                    if (dtlabel != null)
                    {
                        foreach (DataRow labelrow in dtlabel.Rows)
                        {
                            if (Convert.ToString(labelrow[DatabaseObjects.Columns.Title]) == Convert.ToString(row[DatabaseObjects.Columns.Category]))
                            {
                                lableId = dtlabel.Rows.IndexOf(labelrow) + 1;
                                break;
                            }
                        }
                    }

                    DataRow newrow = dt.NewRow();
                    newrow["ID"] = row[DatabaseObjects.Columns.Id];
                    newrow["StartTime"] = row[DatabaseObjects.Columns.StartDate];
                    newrow["EndTime"] = row[DatabaseObjects.Columns.EndDate];
                    newrow["Subject"] = row[DatabaseObjects.Columns.Title];
                    newrow["AllDay"] = Convert.ToString(row[DatabaseObjects.Columns.fAllDayEvent]) == "0" ? false : true;
                    newrow["Description"] = Server.HtmlDecode(Convert.ToString(row[DatabaseObjects.Columns.Comments]));
                    newrow["Label"] = lableId;
                    newrow["Location"] = row[DatabaseObjects.Columns.EventLocation];
                    newrow["RecurrenceInfo"] = row[DatabaseObjects.Columns.RecurrenceInfo];
                    newrow["ReminderInfo"] = "";
                    newrow["OwnerId"] = null;
                    newrow["Status"] =  row[DatabaseObjects.Columns.Status];
                    newrow["EventType"] = row[DatabaseObjects.Columns.EventType];
                    newrow["MeetingType"] = "";
                    dt.Rows.Add(newrow);
                }
            }

            DataTable dtProjectTaskData = GetProjectTaskdata();
            if (dtProjectTaskData != null && dtProjectTaskData.Rows.Count > 0)
            {
                foreach (DataRow row in dtProjectTaskData.Rows)
                {
                    DataRow newrow = dt.NewRow();
                    newrow["ID"] = row[DatabaseObjects.Columns.ID];
                    newrow["ID"] = row[DatabaseObjects.Columns.ID] + ":ProjectTask";
                    newrow["StartTime"] = row[DatabaseObjects.Columns.StartDate];
                    newrow["EndTime"] = row[DatabaseObjects.Columns.DueDate];
                    newrow["Subject"] = row[DatabaseObjects.Columns.Title];
                    newrow["AllDay"] = true;
                    newrow["Description"] = row[DatabaseObjects.Columns.Description];
                    newrow["Label"] = 0;
                    newrow["Location"] = "";
                    newrow["RecurrenceInfo"] = "";
                    newrow["ReminderInfo"] = "";
                    newrow["OwnerId"] = null;
                    newrow["Status"] = 0;
                    newrow["EventType"] = 0;
                    newrow["MeetingType"] = "";
                    dt.Rows.Add(newrow);
                }
            }
            return dt;
        }

        public void UpdateEvents(object EventType, DateTime StartTime, DateTime EndTime, object Id, object AllDay, object Status, object Label, object Description, object Location, object RecurrenceInfo, object ReminderInfo, object OwnerId, object Subject, object meetingType)
        {
            string lableText = string.Empty;
            lableText = "None";

            DataTable newdtlabel = EventCategoryManagerObj.GetDataTable();     // SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategoryQuery);
            if (newdtlabel != null)
            {
                foreach (DataRow labelrow in newdtlabel.Rows)
                {
                    if (newdtlabel.Rows.IndexOf(labelrow) == Convert.ToInt32(Label) - 1)
                    {
                        lableText = Convert.ToString(labelrow[DatabaseObjects.Columns.Title]);
                        break;
                    }
                }
            }

            PMMEvents item = PMMEventManagerObj.LoadByID(Convert.ToInt64(Id));   // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMEvents, Convert.ToInt32(Id));
            item.StartDate = StartTime;
            bool aAllDay = Convert.ToBoolean(AllDay);
            if (aAllDay)
                item.EndDate = EndTime.AddDays(-1);
            else
                item.EndDate = EndTime;

            item.Title = Convert.ToString(Subject);
            item.fAllDayEvent = Convert.ToString(aAllDay);
            item.Comments = Convert.ToString(Description);
            item.Location = Convert.ToString(Location);
            item.RecurrenceInfo = Convert.ToString(RecurrenceInfo);
            item.Status = Convert.ToString(Status);
            item.EventType = Convert.ToString(EventType);

            if (UGITUtility.ObjectToString(meetingType) != "")
            {
               
                DataTable dtlabel = EventCategoryManagerObj.GetDataTable();   //SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategorycheckQuery);
                string expression = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, meetingType);
                DataRow[] foundRows = dtlabel.Select(expression);

                if (foundRows == null || foundRows.Length == 0)
                {
                    
                    EventCategory categoryitem = new EventCategory();
                    categoryitem.ItemColor = "#F5F5F5";
                    categoryitem.Title = Convert.ToString(meetingType);
                    categoryitem.ItemOrder = dtlabel.Rows.Count + 1;
                    EventCategoryManagerObj.Insert(categoryitem);    // categoryitem.Update();
                    lableText = Convert.ToString(meetingType);
                }
                else
                {
                    lableText = Convert.ToString(meetingType);
                }
            }

            item.Category = lableText;
            PMMEventManagerObj.Update(item); //item.Update();
        }

        public void DeleteEvents(object Id)
        {
            PMMEvents item = PMMEventManagerObj.LoadByID(Convert.ToInt64(Id));
            PMMEventManagerObj.Delete(item); 
        }


        public void InsertEvents(object EventType, object AllDay, DateTime StartTime, DateTime EndTime, object Status, object Label, object Description, object Location, object RecurrenceInfo, object ReminderInfo, object OwnerId, object Subject, object meetingType)
        {

            PMMEvents item;
            string lableText = string.Empty;
            lableText = "None";
            
            DataTable newdtlabel = EventCategoryManagerObj.GetDataTable(); // SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategoryQuery);
            if (newdtlabel != null)
            {
                foreach (DataRow labelrow in newdtlabel.Rows)
                {
                    if (newdtlabel.Rows.IndexOf(labelrow) == Convert.ToInt32(Label) - 1)
                    {
                        lableText = Convert.ToString(labelrow[DatabaseObjects.Columns.Title]);
                        break;
                    }
                }
            }
            
            UGITModule module = ModuleManager.LoadByName(ModuleNames.PMM);
            DataRow projectItem = TicketManagerObj.GetByTicketID(module, ProjectPublicID);
            //SPListItem projectItem = Ticket.getCurrentTicket(moduleName, ProjectPublicID, viewFields, true);
            //SPList itemList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMEvents);
            item = new PMMEvents(); // itemList.AddItem();
            item.Title = Convert.ToString(Subject);
            item.Comments = Convert.ToString(Description);
            item.EndDate = EndTime;
            //bool aAllDay = Convert.ToBoolean(AllDay);
            string aAllDay = Convert.ToString(Convert.ToBoolean(AllDay));
            if (aAllDay == "True")
                item.EndDate = EndTime.AddDays(-1);
            else
                item.EndDate = EndTime;
            item.StartDate = StartTime;
            item.fAllDayEvent = aAllDay;
            item.Status = Convert.ToString(Status);
            item.RecurrenceInfo = Convert.ToString(RecurrenceInfo);
            item.Location = Convert.ToString(Location);
            item.EventType = Convert.ToString(EventType);
            item.PMMIdLookup = Convert.ToString( projectItem[DatabaseObjects.Columns.TicketId]);

            if (UGITUtility.ObjectToString(meetingType) != "")
            {
                DataTable dtlabel = EventCategoryManagerObj.GetDataTable(); // SPListHelper.GetDataTable(DatabaseObjects.Lists.EventCategories, eventCategorycheckQuery);
                
                EventCategory categoryitem = new EventCategory(); // CategoryitemList.AddItem();
                if (dtlabel != null)
                {
                    string expression = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, meetingType);
                    DataRow[] foundRows = dtlabel.Select(expression);
                    if (foundRows == null || foundRows.Length == 0)
                    {
                        
                        categoryitem.ItemColor = "#F5F5F5";
                        categoryitem.Title = Convert.ToString( meetingType);
                        categoryitem.ItemOrder = dtlabel.Rows.Count + 1; // CategoryitemList.ItemCount + 1;

                        EventCategoryManagerObj.Insert(categoryitem);  // categoryitem.Update();
                        lableText = Convert.ToString( meetingType);
                    }
                    else
                    {
                        lableText = Convert.ToString( meetingType);
                    }
                }
                else
                {
                    categoryitem.ItemColor = "#F5F5F5";
                    categoryitem.Title = Convert.ToString(meetingType);
                    categoryitem.ItemOrder = dtlabel.Rows.Count + 1;   // CategoryitemList.ItemCount + 1;
                    EventCategoryManagerObj.Insert(categoryitem); //categoryitem.Update();
                    lableText = Convert.ToString(meetingType);
                }
            }

            item.Category = lableText;
            PMMEventManagerObj.Insert(item);           
        }

        protected override void OnPreRender(EventArgs e)
        {
            ASPxScheduler1.DataBind();
            base.OnPreRender(e);
        }

        protected void ASPxScheduler1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu)
            {
                e.Menu.Items.Remove(e.Menu.Items.FindByName("NewRecurringAppointment"));
                DevExpress.Web.MenuItem item1 = e.Menu.Items.FindByName("NewAppointment");
                if (item1 != null) item1.Text = "New Event";
            }
            if (e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu)
            {
                foreach(DevExpress.Web.MenuItem item in e.Menu.Items)
                {
                    if (item.Name == "RestoreOccurrence" || item.Name == "StatusSubMenu" || item.Name == "LabelSubMenu")
                        item.Visible = false;
                }
                e.Menu.ClientSideEvents.PopUp = "OnClientPopupMenuShowing";
                e.Menu.ClientSideEvents.ItemClick = String.Format("function(s, e) {{ OnAppointmentMenuItemClick({0}, s, e); }}", ASPxScheduler1.ClientInstanceName);
            }
        }

        private DataTable GetProjectTaskdata()
        {
            UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            List<UGITTask> listTask = taskManager.LoadByProjectID(ProjectPublicID);
            listTask = listTask.Where(x => x.ShowOnProjectCalendar == true).ToList();
            DataTable dtProjectEvent = null;
            if (listTask != null && listTask.Count > 0)
            {
                dtProjectEvent = UGITUtility.ToDataTable<UGITTask>(listTask);
               
            }
            return dtProjectEvent;
        }

        protected void ASPxScheduler1_InitClientAppointment(object sender, InitClientAppointmentEventArgs args)
        {

        }
    }
}
