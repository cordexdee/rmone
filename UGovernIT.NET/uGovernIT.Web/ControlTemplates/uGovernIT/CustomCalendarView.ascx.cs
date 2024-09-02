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
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class CustomCalendarView : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public string ticketURL = string.Empty;
        UGITModule module;
        ModuleViewManager moduleManager;
        object lastInsertedDRQId;
        string closedStageId;
        public Boolean OpenProjectOnly { get; set; }
        ConfigurationVariableManager configurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

        protected void Page_Load(object sender, EventArgs e)
        {
            OpenProjectOnly = false;
            moduleManager = new ModuleViewManager(context);
            ModuleName = ModuleNames.DRQ;
            module = moduleManager.LoadByName(ModuleName);
            ticketURL = UGITUtility.GetAbsoluteURL(module.StaticModulePagePath);
            calendarView.Storage.Appointments.AutoRetrieveId = true;
        }

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
        CustomDRQDataSource objectInstance;
        // Obtain the ID of the last inserted appointment from the object data source and assign it to the appointment in the ASPxScheduler storage.
        protected void calendarView_AppointmentRowInserted(object sender, DevExpress.Web.ASPxScheduler.ASPxSchedulerDataInsertedEventArgs e)
        {
            e.KeyFieldValue = this.objectInstance.ObtainLastInsertedId();
        }
        protected void drqDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {          
            this.objectInstance = new CustomDRQDataSource(GetCustomEvents());
            e.ObjectInstance = this.objectInstance;
        }

        CustomDRQList GetCustomEvents()
        {
            CustomDRQList events = Session["CustomDRQListData"] as CustomDRQList;
            if (events == null || events.Count == 0)
            {
                events = new CustomDRQList();
                List<CustomDRQEvent> drqEventList = CustomDRQDataSource.LoadDrqProjects(ModuleNames.DRQ, context);
                if (OpenProjectOnly)
                {
                    DataRow[] moduleStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{ModuleNames.DRQ}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                    DataRow[] moduleStageTypes = new DataRow[0];
                    DataTable moduleStageTypesTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.StageType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    if (moduleStageTypesTable != null)
                        moduleStageTypes = moduleStageTypesTable.Select();

                    // Get the "Closed" stage id for module
                    closedStageId = uHelper.GetModuleStageId(moduleStages, moduleStageTypes, StageType.Closed);
                    if (string.IsNullOrEmpty(closedStageId))
                        closedStageId = "0";
                    if (!string.IsNullOrEmpty(configurationVariableHelper.GetValue(ConfigConstants.DRQCalendarMinStage)))
                    {
                        int minStage = UGITUtility.StringToInt(configurationVariableHelper.GetValue(ConfigConstants.DRQCalendarMinStage));
                        drqEventList = drqEventList.FindAll(x=>x.ModuleStepLookup == closedStageId && x.StageStep == minStage);
                    }
                    else
                        drqEventList = drqEventList.FindAll(x => x.ModuleStepLookup == closedStageId);

                }
                else
                {
                    if (!string.IsNullOrEmpty(configurationVariableHelper.GetValue(ConfigConstants.DRQCalendarMinStage)))
                    {
                        int minStage = UGITUtility.StringToInt(configurationVariableHelper.GetValue(ConfigConstants.DRQCalendarMinStage));
                        drqEventList = drqEventList.FindAll(x => x.StageStep == minStage);
                    }
                }
                foreach (CustomDRQEvent drqEvent in drqEventList)
                {
                    events.Add(drqEvent);
                }
                Session["CustomDRQListData"] = events;
            }
            return events;
        }
        #endregion #setappointment
        // The following code is unnecessary if the ASPxScheduler.Storage.Appointments.AutoRetrieveId option is TRUE.
        protected void calendarView_AppointmentsInserted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            SetAppointmentId(sender, e);
        }

        protected void drqDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.lastInsertedDRQId = e.ReturnValue;
        }
        void SetAppointmentId(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage storage = (DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage)sender;
            DevExpress.XtraScheduler.Appointment apt = (DevExpress.XtraScheduler.Appointment)e.Objects[0];
            storage.SetAppointmentId(apt, this.lastInsertedDRQId);
        }

        protected void calendarView_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
        {
            ASPxSchedulerPopupMenu menu = e.Menu;
            if (e.Menu.MenuId == SchedulerMenuItemId.AppointmentMenu)
            {
                DevExpress.Web.MenuItemCollection menuItems = menu.Items;
                foreach (DevExpress.Web.MenuItem m in menuItems)
                {
                    if (m.Name == "OpenAppointment")
                        m.Visible = true;
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
                    if (m.Name == "NewAppointment")
                        m.Visible = false;
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

     

        protected void calendarView_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        {

            if (e.Appointment != null)
            {
                string labelTitle = string.Empty;
                labelTitle = string.Format("{0} ({1})", e.Appointment.Subject, e.Appointment.Id);
                e.Text = labelTitle;
            }
        }
    }
}