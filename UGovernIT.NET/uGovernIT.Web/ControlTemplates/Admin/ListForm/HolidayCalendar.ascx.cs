using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.XtraScheduler;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Data;
using System.Collections.Specialized;
using System.Collections;
using DevExpress.Web.ASPxScheduler;

namespace uGovernIT.Web
{
    public partial class HolidayCalendar : System.Web.UI.UserControl
    {
        
        object lastInsertedAppointmentId;

        ConfigurationVariableManager configHelper;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            configHelper = new ConfigurationVariableManager(context);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SchedulerCalendar.Storage.Appointments.AutoRetrieveId = true;
            string aWeekDays = configHelper.GetValue("WeekDays");
            string[] weekdayslist = aWeekDays.Split(',');
            if (weekdayslist.Count() > 0)
            {
                SchedulerCalendar.WorkDays.Clear();
                foreach (string day in weekdayslist)
                {
                    SchedulerCalendar.WorkDays.Add(ReturnWeekDayFromString(day));
                }
            }

            string startDay = configHelper.GetValue("StartDay");
            if (!string.IsNullOrEmpty(startDay))
                SchedulerCalendar.OptionsView.FirstDayOfWeek = ReturnFirstDayOfWeekFromString(Convert.ToString(startDay));

            string sStartTime = configHelper.GetValue("WorkdayStartTime");
            string sEndTime = configHelper.GetValue("WorkdayEndTime");
            if (!string.IsNullOrEmpty(sEndTime) && !string.IsNullOrEmpty(sStartTime))
            {
                DateTime endtime = Convert.ToDateTime(sEndTime);
                DateTime starttime = Convert.ToDateTime(sStartTime);
                if (endtime.TimeOfDay < SchedulerCalendar.DayView.WorkTime.Start)
                {
                    SchedulerCalendar.DayView.WorkTime.Start = new TimeSpan(starttime.Hour, starttime.Minute, starttime.Second);
                    SchedulerCalendar.WorkWeekView.WorkTime.Start = new TimeSpan(starttime.Hour, starttime.Minute, starttime.Second);

                    SchedulerCalendar.DayView.WorkTime.End = new TimeSpan(endtime.Hour, endtime.Minute, endtime.Second);
                    SchedulerCalendar.WorkWeekView.WorkTime.End = new TimeSpan(endtime.Hour, endtime.Minute, endtime.Second);
                }
                else
                {
                    SchedulerCalendar.DayView.WorkTime.End = new TimeSpan(endtime.Hour, endtime.Minute, endtime.Second);
                    SchedulerCalendar.WorkWeekView.WorkTime.End = new TimeSpan(endtime.Hour, endtime.Minute, endtime.Second);

                    SchedulerCalendar.DayView.WorkTime.Start = new TimeSpan(starttime.Hour, starttime.Minute, starttime.Second);
                    SchedulerCalendar.WorkWeekView.WorkTime.Start = new TimeSpan(starttime.Hour, starttime.Minute, starttime.Second);
                }
            }

            //if (!string.IsNullOrEmpty(sStartTime))
            //{
                
            //}

            if (!IsPostBack)
            {
                List<ConfigurationVariable> configVarList = configHelper.Load(x => x.CategoryName == "Calendar");
                StartTimeEdit.Text = configVarList.FirstOrDefault(x => x.KeyName == ConfigConstants.WorkdayStartTime) == null ? string.Empty : configVarList.Where(x => x.KeyName == "WorkdayStartTime").First().KeyValue;
                EndTimeEdit.Text = configVarList.FirstOrDefault(x => x.KeyName == ConfigConstants.WorkdayEndTime) == null ? string.Empty : configVarList.Where(x => x.KeyName == "WorkdayEndTime").First().KeyValue;
                string firstDayOfWeek = configVarList.FirstOrDefault(x => x.KeyName == "StartDay") == null ? string.Empty : configVarList.Where(x => x.KeyName == "StartDay").First().KeyValue;
                rblFirstDayOfWeek.SelectedIndex = rblFirstDayOfWeek.Items.IndexOf(rblFirstDayOfWeek.Items.FindByValue(firstDayOfWeek));
                string weekdays = configVarList.FirstOrDefault(x => x.KeyName == "WeekDays") == null ? string.Empty : configVarList.Where(x => x.KeyName == "WeekDays").First().KeyValue;
                string[] weekdayArray = weekdays.Split(',');
                foreach (string item in weekdayArray)
                {
                    ListEditItem currentItem = chkWeekDays.Items.FindByValue(item);
                    if (currentItem != null && !string.IsNullOrEmpty(Convert.ToString(currentItem.Value)))
                        currentItem.Selected = true;
                }

            }
        }

        CustomEventDataSource objectInstance;

        protected void SchedulerCalendar_AppointmentRowInserted(object sender, DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertedEventArgs e)
        {
            e.KeyFieldValue = this.objectInstance.ObtainLastInsertedId();
        }

        protected void appointmentDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {
            this.objectInstance = new CustomEventDataSource(context,GetCustomEvents());
            e.ObjectInstance = this.objectInstance;
        }
        CustomEventList GetCustomEvents()
        {
            CustomEventList events = Session["CustomEventListData"] as CustomEventList;
            if (events == null)
            {
                AppointmentsManager manager = new AppointmentsManager(context);
                events = new CustomEventList();
                foreach(Utility.Appointment a in manager.Load())
                {
                    events.Add(a);
                }
                Session["CustomEventListData"] = events;
            }
            return events;
        }

        protected void SchedulerCalendar_AppointmentsInserted(object sender, PersistentObjectsEventArgs e)
        {
            SetAppointmentId(sender, e);
        }
        void SetAppointmentId(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage storage = (DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage)sender;
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Objects[0];
            storage.SetAppointmentId(apt, this.lastInsertedAppointmentId);
        }

        protected void appointmentDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.lastInsertedAppointmentId = e.ReturnValue;
        }

        protected void SchedulerCalendar_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
        {
            ASPxSchedulerPopupMenu menu = e.Menu;
            if (e.Menu.MenuId == SchedulerMenuItemId.DefaultMenu)
            {
                DevExpress.Web.MenuItemCollection menuItems = menu.Items;
                foreach(DevExpress.Web.MenuItem m in menuItems)
                {
                    if (m.Name == "NewAppointment")
                        m.Text = "New Event";
                    else if (m.Name == "NewAllDayEvent")
                        m.Text = "New All Day Event";
                    else if (m.Name == "NewRecurringAppointment")
                        m.Text = "New Recurring";
                    else if (m.Name == "NewRecurringEvent")
                        m.Text = "New All Day Recurring";
                }
            }
        }

        public DevExpress.XtraScheduler.WeekDays ReturnWeekDayFromString(string day)
        {
            switch (day.ToLower())
            {
                case "monday":
                    return WeekDays.Monday;
                case "tuesday":
                    return WeekDays.Tuesday;
                case "wednesday":
                    return WeekDays.Wednesday;
                case "thursday":
                    return WeekDays.Thursday;
                case "friday":
                    return WeekDays.Friday;
                case "saturday":
                    return WeekDays.Saturday;
                case "sunday":
                    return WeekDays.Sunday;
            }

            return WeekDays.EveryDay;
        }

        public DevExpress.XtraScheduler.FirstDayOfWeek ReturnFirstDayOfWeekFromString(string day)
        {
            switch (day.ToLower())
            {
                case "monday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Monday;
                case "tuesday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Tuesday;
                case "wednesday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Wednesday;
                case "thursday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Thursday;
                case "friday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Friday;
                case "saturday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Saturday;
                case "sunday":
                    return DevExpress.XtraScheduler.FirstDayOfWeek.Sunday;
            }

            return DevExpress.XtraScheduler.FirstDayOfWeek.System;
        }

        //protected void ASPxButton1_Click(object sender, EventArgs e)
        //{
        //    ASPxScheduler sched = new ASPxScheduler();
        //    sched.AppointmentDataSource = ConfigurationVariableHelper.GetConfigurationVariableData().Where(x => x.CategoryName == "Calendar");
        //    sched.DataBind();
        //    // Calculate occurrences for the first recurrenct series.
        //    List<DevExpress.XtraScheduler.Appointment> pattern = sched.Storage.Appointments.Items.Where(item => item.Type == AppointmentType.Normal).ToList();
        //    if (pattern == null) return;
        //    OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(pattern[1].RecurrenceInfo);
        //    TimeInterval processedInterval = new TimeInterval(DateTime.Today, DateTime.Today.AddDays(7));
        //    AppointmentBaseCollection calculatedOccurrences = calc.CalcOccurrences(processedInterval, pattern[1]);
        //}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //GetTableDataManager.delete<int>(DatabaseObjects.Tables.ConfigurationVariable, DatabaseObjects.Columns.ID, Convert.ToString(dtConfigVariable.Rows[0][DatabaseObjects.Columns.ID]));
                //uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            List<ConfigurationVariable> configVariableList = new List<ConfigurationVariable>();
            
            if (!Page.IsValid)
                return;

            if (!string.IsNullOrEmpty(StartTimeEdit.Text))
            {
                ConfigurationVariable configVariableObj1 = new ConfigurationVariable();
                configVariableObj1.CategoryName = "Calendar";
                configVariableObj1.KeyName = "WorkdayStartTime";
                configVariableObj1.KeyValue = StartTimeEdit.Text;
                configVariableObj1.Title = "WorkdayStartTime";
                configVariableObj1.Description = "WorkdayStartTime";
                configVariableObj1.Type = "Date";
                configVariableList.Add(configVariableObj1);
            }

            if (!string.IsNullOrEmpty(EndTimeEdit.Text))
            {
                ConfigurationVariable configVariableObj2 = new ConfigurationVariable();
                configVariableObj2.CategoryName = "Calendar";
                configVariableObj2.KeyName = "WorkdayEndTime";
                configVariableObj2.KeyValue = EndTimeEdit.Text;
                configVariableObj2.Title = "WorkdayEndTime";
                configVariableObj2.Description = "WorkdayEndTime";
                configVariableObj2.Type = "Date";
                configVariableList.Add(configVariableObj2);
            }

            
            ConfigurationVariable configVariableObj3 = new ConfigurationVariable();
            configVariableObj3.CategoryName = "Calendar";
            configVariableObj3.KeyName = "WeekDays";
            string selectedWeeks = "";
            foreach (string s in chkWeekDays.SelectedValues)
            {
                selectedWeeks = selectedWeeks + ',' + s.ToString();
            }
            if (!string.IsNullOrEmpty(selectedWeeks))
                selectedWeeks = selectedWeeks.Remove(0, 1);
            configVariableObj3.KeyValue = selectedWeeks;
            configVariableObj3.Title = "WeekDays";
            configVariableObj3.Type = "Text";
            configVariableObj3.Description = "WeekDays";
            configVariableList.Add(configVariableObj3);

            ConfigurationVariable configVariableObj4 = new ConfigurationVariable();
            configVariableObj4.CategoryName = "Calendar";
            configVariableObj4.KeyName = "StartDay";
            configVariableObj4.KeyValue = Convert.ToString(rblFirstDayOfWeek.SelectedItem!=null? rblFirstDayOfWeek.SelectedItem.Value:string.Empty);
            configVariableObj4.Title = "StartDay";
            configVariableObj4.Description = "First Day Of Week";
            configVariableObj4.Type = "Text";
            configVariableList.Add(configVariableObj4);

            foreach (ConfigurationVariable configvar in configVariableList)
            {
                ConfigurationVariable var = configHelper.LoadVaribale(configvar.KeyName);
                if (var != null)
                    configvar.ID = var.ID;

                if (configvar.ID > 0)
                    configHelper.Update(configvar);
                else
                    configHelper.Insert(configvar);
            }
        }

        #region #beforeexecutecallbackcommand
        protected void ASPxSchedulerHolidayCalendar_BeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e)
        {
            if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
            {
                e.Command =
                    new HolidayCalendarEventSaveCallbackCommand((ASPxScheduler)sender);
            }
        }
        #endregion #beforeexecutecallbackcommand

        #region #appointmentformshowing
        protected void ASPxSchedulerHolidayCalendar_AppointmentFormShowing(object sender, DevExpress.Web.ASPxScheduler.AppointmentFormEventArgs e)
        {
            e.Container = new HolidayCalendarEventViewTemplateContainer((ASPxScheduler)sender);
            if (e.Appointment.AllDay)
            {
                e.Container.Caption = "All Day Event";
            }
            else
            {
                e.Container.Caption = "Event";
            }
            if (UGITUtility.StringToInt(e.Container.Appointment.Id) != 0)
            {
                e.Container.Caption += ": " + e.Container.Subject;
            }
        }
        #endregion #appointmentformshowing
    }
}