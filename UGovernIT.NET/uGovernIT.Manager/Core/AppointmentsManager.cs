using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.DAL;
using uGovernIT.Utility;
using System.ComponentModel;
using System.Collections;
using DevExpress.Web;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{

    public class AppointmentsManager:ManagerBase<Utility.Appointment>, IAppointmentsManager
    {
        
        OccurrenceCalculator calc = null;
        List<DevExpress.XtraScheduler.Appointment> pattern;
        ASPxScheduler sched = new ASPxScheduler();
        public OccurrenceCalculator Calc { get { return calc; } }
        public List<DevExpress.XtraScheduler.Appointment> Pattern { get { return pattern; } }
        public ASPxScheduler Scheduler { get { return sched; } }
        public AppointmentsManager(ApplicationContext context):base(context)
        {
            store = new AppointmentsStore(this.dbContext);
            ASPxSchedulerStorage storage = sched.Storage;
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
                mappings.ResourceId = "ownerId";
                mappings.Status = "Status";
                mappings.Type = "EventType";
            }
            finally
            {
                storage.EndUpdate();
            }
            //AppointmentsManager manager = new AppointmentsManager();
            sched.AppointmentDataSource = store.Load();
            sched.DataBind();

            pattern = sched.Storage.Appointments.Items.Where(item => Convert.ToInt32(item.LabelKey) == 4).ToList();
            if (pattern != null && pattern.Count > 0 && pattern[0].RecurrenceInfo != null)
            {
                calc = OccurrenceCalculator.CreateInstance(pattern[0].RecurrenceInfo);
            }
          
        }
        public DataTable ReturnAppointments()
        {
            return (store as ManagerBase<Utility.Appointment>).GetDataTable();
        }
        //public static OccurrenceCalculator GetOccurrenceCalculatorObject()
        //{
        //    OccurrenceCalculator calc = null;
        //    ASPxScheduler sched = new ASPxScheduler();
        //    ASPxSchedulerStorage storage = sched.Storage;
        //    storage.Appointments.AutoRetrieveId = true;
        //    ASPxAppointmentMappingInfo mappings = storage.Appointments.Mappings;
        //    storage.BeginUpdate();
        //    try
        //    {
        //        mappings.AppointmentId = "Id";
        //        mappings.Start = "StartTime";
        //        mappings.End = "EndTime";
        //        mappings.Subject = "Subject";
        //        mappings.AllDay = "AllDay";
        //        mappings.Description = "Description";
        //        mappings.Label = "Label";
        //        mappings.Location = "Location";
        //        mappings.RecurrenceInfo = "RecurrenceInfo";
        //        mappings.ReminderInfo = "ReminderInfo";
        //        mappings.ResourceId = "ownerId";
        //        mappings.Status = "Status";
        //        mappings.Type = "EventType";
        //    }
        //    finally
        //    {
        //        storage.EndUpdate();
        //    }
        //    AppointmentsManager manager = new AppointmentsManager();
        //    sched.AppointmentDataSource = manager.GetAppointments();
        //    sched.DataBind();
        //    ///TimeIntervalCollection tic = sched.ActiveView.GetVisibleIntervals();
        //    List<DateTime> appointmentDates = new List<DateTime>();
        //    //TimeSpan totalHours = new TimeSpan();
        //    // Calculate occurrences for the first recurrenct series.
        //    List<DevExpress.XtraScheduler.Appointment> pattern = sched.Storage.Appointments.Items.ToList(); //.Where(item => item.Type == AppointmentType.Pattern).ToList();
        //    //if (pattern != null)
        //    //{
        //    //    calc = OccurrenceCalculator.CreateInstance(pattern[0].RecurrenceInfo);
        //    //    TimeInterval processedInterval = new TimeInterval(calc.CalcOccurrenceStartTime(0), DateTime.Today.AddDays(calc.CalcLastOccurrenceIndex(pattern[0])));
        //    //    AppointmentBaseCollection calculatedOccurrences = calc.CalcOccurrences(processedInterval, pattern[0]);
        //    //    var nextoccurence = calc.FindNextOccurrenceTimeAfter(processedInterval.End, pattern[1]);
        //    //}

        //    foreach (DevExpress.XtraScheduler.Appointment item in pattern)
        //    {
        //        if (item.Type == AppointmentType.Pattern)
        //        {
        //            calc = OccurrenceCalculator.CreateInstance(item.RecurrenceInfo);
        //            TimeInterval processedInterval = new TimeInterval(new DateTime(2017,4,1), new DateTime(2017,4,30));
        //            //AppointmentBaseCollection calculatedOccurrences = calc.CalcOccurrences(processedInterval, item);
        //            var nextoccurence = calc.FindNextOccurrenceTimeAfter(processedInterval.End, item);

        //            TimeSpan duration = item.Duration;
        //            DateTime start = calc.CalcOccurrenceStartTime(0);
        //            int length = item.RecurrenceInfo.OccurrenceCount;
        //            for (int i = 0; i < length; i++)
        //            {
        //                //appointmentDates.Add(start.AddDays(i).Date);
        //                //totalHours.Add(duration);
        //                sched.WorkDays.AddHoliday(start.AddDays(i).Date, item.Description);
        //            }
        //        }
        //        if(item.Type == AppointmentType.Normal)
        //        {
        //            //appointmentDates.Add(item.Start);
        //            sched.WorkDays.AddHoliday(item.Start.Date, item.Description);
        //        }
        //    }

        //    List<DateTime> uniqueDates = appointmentDates.Distinct().ToList();

        //    List<DateTime> workingDays = new List<DateTime>();
        //    for (int i = 1; i < 31; i++)
        //    {
        //        DateTime date = new DateTime(2017, 4, i);
        //        if (sched.WorkDays.IsWorkDay(date))
        //            workingDays.Add(date);
        //        //if(!uniqueDates.Exists(x=>x.Date == date) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
        //        //{

        //        //    workingDays.Add(date);
        //        //}
        //    }

        //    return calc;
        //}

        public class Range<T> where T : IComparable
        {
            readonly T min;
            readonly T max;

            public Range(T min, T max)
            {
                this.min = min;
                this.max = max;
            }

            public bool IsOverlapped(Range<T> other)
            {
                return Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;
            }

            public T Min { get { return min; } }
            public T Max { get { return max; } }
        }
    }
    public interface IAppointmentsManager : IManagerBase<Utility.Appointment>
    {
        DataTable ReturnAppointments();
    }
}
