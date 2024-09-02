using DevExpress.Web.ASPxScheduler;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class CalendarView : UserControl
    {
        public string UserID { get; set; }
        ASPxSchedulerStorage Storage { get { return this.ASPxSchedulerCalendar.Storage; } }
        HolidaysAndWorkDaysCalendarManager objHoliDaysManager = new HolidaysAndWorkDaysCalendarManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            ASPxSchedulerCalendar.Storage.Appointments.Labels.Clear();
            
            
            AppointmentLabel  lblDefault = ASPxSchedulerCalendar.Storage.Appointments.Labels.CreateNewLabel(System.Drawing.ColorTranslator.FromHtml("#F5F5F5"), string.Format("<div style='float:left;'> <span> None </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"), string.Format("<div style='float:left;'> <span> None </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"));
            AppointmentLabel lblDefault1 = ASPxSchedulerCalendar.Storage.Appointments.Labels.CreateNewLabel(System.Drawing.ColorTranslator.FromHtml("#F2D9DB"), string.Format("<div style='float:left;'> <span> Holiday </span> <span style='background-color:#F2D9DB;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"), string.Format("<div style='float:left;'> <span> Holiday </span> <span style='background-color:#F2D9DB;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"));
            AppointmentLabel lblDefault2 = ASPxSchedulerCalendar.Storage.Appointments.Labels.CreateNewLabel(System.Drawing.ColorTranslator.FromHtml("#F5F5F5"), string.Format("<div style='float:left;'> <span> Work Hours </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"), string.Format("<div style='float:left;'> <span> Work Hours </span> <span style='background-color:#F5F5F5;border-width: 1px;border-color: black;border-style: solid;'>&nbsp;&nbsp;&nbsp; </span></div>"));
            ASPxSchedulerCalendar.Storage.Appointments.Labels.Add(lblDefault);
            ASPxSchedulerCalendar.Storage.Appointments.Labels.Add(lblDefault1);
            ASPxSchedulerCalendar.Storage.Appointments.Labels.Add(lblDefault2);

            CalendarDataSource.SelectMethod = "GetHolidayEventData";
            CalendarDataSource.TypeName = this.GetType().AssemblyQualifiedName;
            CalendarDataSource.ObjectCreating += CalendarDataSource_ObjectCreating;
            mapping(ASPxSchedulerCalendar);
            ASPxSchedulerCalendar.AppointmentDataSourceID = CalendarDataSource.ID;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxSchedulerCalendar.DataBind();
        }

        private void mapping(ASPxScheduler control)
        {
            ASPxSchedulerStorage storage = control.Storage;
            storage.Appointments.AutoRetrieveId = true;
            ASPxAppointmentMappingInfo mappings = storage.Appointments.Mappings;
            storage.BeginUpdate();
            try
            {
                mappings.AppointmentId = "Id";
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
        }

        protected void CalendarDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        public DataTable GetHolidayEventData()
        {
            return GetListData();
        }

        private DataTable GetListData()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { 
                            new DataColumn("Id", typeof(object)),
                            new DataColumn("StartTime", typeof(DateTime)),
                            new DataColumn("EndTime", typeof(DateTime)),
                            new DataColumn("Subject",typeof(string)),
                            new DataColumn("AllDay", typeof(Boolean)),
                            new DataColumn("Description",typeof(string)),
                            new DataColumn("Label", typeof(int)),
                            new DataColumn("Location",typeof(string)),
                            new DataColumn("RecurrenceInfo",typeof(string)),
                            new DataColumn("ReminderInfo",typeof(string)),
                            new DataColumn("OwnerId", typeof(object)),
                            new DataColumn("Status", typeof(long)),
                            new DataColumn("EventType",typeof(int)),
                          });

            string actualDQuery = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.Deleted, false, DatabaseObjects.Columns.CategoryChoice, "Holiday");
            DataRow[] HolidaysRows = objHoliDaysManager.GetDataTable().Select(actualDQuery);
            DataTable dtholidayCalendarEvents = null;
            if (HolidaysRows != null && HolidaysRows.Count() > 0)
            {
                dtholidayCalendarEvents = HolidaysRows.CopyToDataTable();// objHoliDaysManager.GetDataTable();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.HolidaysAndWorkDaysCalendar).Select(actualDQuery).CopyToDataTable();
            }
            else
            {
                dtholidayCalendarEvents= objHoliDaysManager.GetDataTable().Clone();
            }
            if (dtholidayCalendarEvents != null && dtholidayCalendarEvents.Rows.Count > 0)
            {
                foreach (DataRow row in dtholidayCalendarEvents.Rows)
                {
                    int lableId = 0;

                    if (Convert.ToString(row[DatabaseObjects.Columns.Category]) == "Holiday")
                    {
                        lableId = 1;
                    }
                    else if (Convert.ToString(row[DatabaseObjects.Columns.Category]) == "Work Hours")
                    {
                        lableId = 2;
                    }

                    DataRow newrow = dt.NewRow();
                    newrow["Id"] = row[DatabaseObjects.Columns.Id];
                    newrow["AllDay"] = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.fAllDayEvent]);
                    newrow["StartTime"] = row[DatabaseObjects.Columns.StartDate];
                    newrow["EndTime"] = row[DatabaseObjects.Columns.EndDate];
                    newrow["Subject"] = row[DatabaseObjects.Columns.Title];
                    newrow["Description"] = row[DatabaseObjects.Columns.Comment];
                    newrow["Label"] = lableId;
                    newrow["Location"] = row[DatabaseObjects.Columns.EventLocation];
                    newrow["RecurrenceInfo"] = Convert.ToString(row[DatabaseObjects.Columns.RecurrenceInfo]);
                    newrow["ReminderInfo"] = "";
                    newrow["OwnerId"] = null;
                    newrow["Status"] = Convert.ToString(row[DatabaseObjects.Columns.UGITStatus]) == "" ? 0 : Convert.ToInt32(row[DatabaseObjects.Columns.UGITStatus]);
                    newrow["EventType"] = Convert.ToString(row[DatabaseObjects.Columns.UGITEventType]) == "" ? 0 : Convert.ToInt32(row[DatabaseObjects.Columns.UGITEventType]);
                    dt.Rows.Add(newrow);
                }
            }

            //out of office users
            UserProfileManager objUserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            List<UserProfile> users = new List<UserProfile>();
            UserProfile user = objUserManager.GetUserById(UserID);
            if (user == null)
                return dt;

            bool isResourceAdmin = objUserManager.IsUGITSuperAdmin(user) || objUserManager.IsResourceAdmin(user);
            if (isResourceAdmin)
                users = objUserManager.GetUsersProfile();     // uGITCache.UserProfileCache.GetAllUsers(SPContext.Current.Web);
            else
                objUserManager.LoadUserWorkingUnder(UserID, ref users);     // UserProfile.LoadUserWorkingUnder(uHelper.StringToInt(UserID), ref users);

            if (users == null || users.Count == 0)
                return dt;

            users = users.Where(x => x.EnableOutofOffice).ToList();

            foreach (UserProfile u in users)
            {
                DataRow newrow = dt.NewRow();
                newrow["Id"] = u.Id.ToString() + "OutOfOffice";
                newrow["StartTime"] = u.LeaveFromDate;
                newrow["EndTime"] = u.LeaveToDate.Date.AddMinutes(1439);
                newrow["Subject"] = string.Format("Out Of Office: {0}", u.Name);
                newrow["AllDay"] = false;
                newrow["Description"] = "";
                newrow["Label"] = 0;
                newrow["Location"] = "";
                newrow["RecurrenceInfo"] = "";
                newrow["ReminderInfo"] = "";
                newrow["OwnerId"] = null;
                newrow["Status"] = 2;
                newrow["EventType"] = 0;
                dt.Rows.Add(newrow);
            }

            return dt;
        }

        protected override void OnPreRender(EventArgs e)
        {
            ASPxSchedulerCalendar.DataBind();
            base.OnPreRender(e);
        }

        protected void ASPxSchedulerCalendar_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
        {
            if (e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu)
            {
                e.Menu.Items.Remove(e.Menu.Items.FindByName("NewRecurringEvent"));
                e.Menu.Items.Remove(e.Menu.Items.FindByName("NewRecurringAppointment"));
                e.Menu.Items.Remove(e.Menu.Items.FindByName("NewAppointment"));
                e.Menu.Items.Remove(e.Menu.Items.FindByName("NewAllDayEvent"));
            }
            if (e.Menu.MenuId == DevExpress.XtraScheduler.SchedulerMenuItemId.AppointmentMenu)
            {                
                e.Menu.Items.Clear();
            }
        }


        private DataTable GetRequestTypedata()
        {
            DataTable result = null;
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string actualDQuery = string.Format("{0}={1}", DatabaseObjects.Columns.OutOfOffice, true);
            RequestTypeManager ObjrequestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
            DataRow[] requestRows = ObjrequestTypeManager.GetDataTable().Select(actualDQuery);
            DataTable dtRequestType = null;
            if (requestRows != null && requestRows.Count() > 0)
            {
                dtRequestType = requestRows.CopyToDataTable();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType).Select(actualDQuery).CopyToDataTable();
            }
            else
            {
                dtRequestType = ObjrequestTypeManager.GetDataTable().Clone();
            }
            // modification lines of code's
            if (dtRequestType != null)
            {
                dtRequestType.Columns.Add("TempRequestCol", typeof(string), string.Format("{0} + '-' + {1} +'-' + {2}", Convert.ToString(DatabaseObjects.Columns.RequestCategory), Convert.ToString(DatabaseObjects.Columns.Category), Convert.ToString(DatabaseObjects.Columns.TicketRequestType)));
            }

            DataTable dttemp = new DataTable();
            dttemp.Columns.AddRange(new DataColumn[] { 
                            new DataColumn("Id", typeof(string)),
                            new DataColumn(DatabaseObjects.Columns.WorkItemType, typeof(string)),
                            new DataColumn(DatabaseObjects.Columns.WorkItem, typeof(string)),
                            new DataColumn(DatabaseObjects.Columns.SubWorkItem,typeof(string)),
            });
            UserProfileManager objUserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            List<UserProfile> userCollection = new List<UserProfile>();
            userCollection = objUserManager.GetUserByManager(UserID);
           // UserProfile.LoadUserWorkingUnder(Convert.ToInt32(UserID), ref userCollection);
            userCollection = userCollection.OrderBy(x => x.Name).ToList();
            string[] useridList = userCollection.Select(x => x.Id).ToArray();


            List<string> workitemqueryExpressions = new List<string>();
            string qexpression = string.Empty;
            string _query = "";
            if (useridList != null && useridList.Length > 0)
            {
                foreach (string userId in useridList)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        workitemqueryExpressions.Add(string.Format("{0}='{1}'",
                                                            DatabaseObjects.Columns.Resource, userId));
                    }
                }
                if (workitemqueryExpressions.Count > 0)
                {
                    qexpression = string.Join(" or ", workitemqueryExpressions);
                        //uHelper.GenerateWhereQueryWithAndOr(workitemqueryExpressions, workitemqueryExpressions.Count - 1, false);
                }
            }

            _query= qexpression;
            
            DataRow[] dtUserWorkItemRows = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(_query);
            if (dtUserWorkItemRows != null && dtUserWorkItemRows.Count() > 0)
            {
                DataTable dtUserWorkItem = dtUserWorkItemRows.CopyToDataTable();
                if (dtUserWorkItem != null && dtRequestType != null)
                {
                    // modification lines of code's
                    dtUserWorkItem.Columns.Add("TempCol", typeof(string), string.Format("{0} + '-' + {1} +'-' + {2}", Convert.ToString(DatabaseObjects.Columns.WorkItemType), Convert.ToString(DatabaseObjects.Columns.WorkItem), Convert.ToString(DatabaseObjects.Columns.SubWorkItem)));
                    string selectQuery = string.Format("{0} in ({1})", "TempCol", "'" + string.Join("','", dtRequestType.AsEnumerable().Select(s => s.Field<string>("TempRequestCol")).ToArray<string>()) + "'");
                    DataRow[] userworkitemRows = dtUserWorkItem.Select(selectQuery);

                    if (userworkitemRows != null)
                    {
                        foreach (DataRow workitemrow in userworkitemRows)
                        {
                            DataRow newrow = dttemp.NewRow();
                            newrow[DatabaseObjects.Columns.WorkItemType] = workitemrow[DatabaseObjects.Columns.WorkItemType];
                            newrow[DatabaseObjects.Columns.WorkItem] = workitemrow[DatabaseObjects.Columns.WorkItem];
                            newrow[DatabaseObjects.Columns.SubWorkItem] = workitemrow[DatabaseObjects.Columns.SubWorkItem];
                            newrow["Id"] = workitemrow[DatabaseObjects.Columns.Id];
                            dttemp.Rows.Add(newrow);
                        }
                    }
                }
            }
            DataTable dtResourseAllocation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (dttemp.Rows.Count > 0 && dtResourseAllocation != null)
            {
                var rowsData = (from t1 in dttemp.AsEnumerable()
                                join t2 in dtResourseAllocation.AsEnumerable()
                                on Convert.ToString(t1.Field<object>(DatabaseObjects.Columns.Id)) equals Convert.ToString(t2.Field<object>(DatabaseObjects.Columns.ResourceWorkItemLookup))
                                select t2.ItemArray.Concat(t1.ItemArray).ToArray());
                result = dtResourseAllocation.Clone();
                foreach (DataColumn item in dttemp.Columns)
                {
                    result.Columns.Add(item.ColumnName, item.DataType);
                }
                if (rowsData != null && rowsData.Count() > 0)
                {
                    foreach (object[] values in rowsData)
                        result.Rows.Add(values);
                }
            }
            return result;
        }

    }
}
