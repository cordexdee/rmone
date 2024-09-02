using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web;
using uGovernIT.Utility;
using System.Collections;
using DevExpress.XtraScheduler;
using DevExpress.Schedule;
using System.Web;
using Owin;


namespace uGovernIT.Manager
{
    public class CustomEventDataSource
    {

        CustomEventList events;
        AppointmentsManager objAppointmentsManager;

        public CustomEventDataSource()
        {

        }

        public CustomEventDataSource(ApplicationContext context, CustomEventList events)
        {           
            objAppointmentsManager = new AppointmentsManager(context);
            //if (events == null)
            //    DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events");
            //this.events = new CustomEventList();
            this.events = events;
        }
          
            public CustomEventList Events { get { return events; } set { events = value; } }
        public int Count { get { return Events.Count; } }

        #region ObjectDataSource methods
        public object InsertMethodHandler(Utility.Appointment customEvent)
        {
            //Object id = customEvent.GetHashCode();
            //customEvent.ID = id;
            objAppointmentsManager.Insert(customEvent);
            Events.Add(customEvent);
            return customEvent.ID;
        }
        public void DeleteMethodHandler(Utility.Appointment customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.ID);
            if (eventIndex >= 0)
            {
                Events.RemoveAt(eventIndex);
                objAppointmentsManager.Delete(customEvent);
            }
        }
        public void UpdateMethodHandler(Utility.Appointment customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.ID);
            if (eventIndex >= 0)
            {
                objAppointmentsManager.Update(customEvent);
                Events.RemoveAt(eventIndex);
                Events.Insert(eventIndex, customEvent);
            }
        }
        public IEnumerable SelectMethodHandler()
        {
            CustomEventList result = new CustomEventList();
            result.AddRange(Events);
            return result;
        }
        #endregion

        public object ObtainLastInsertedId()
        {
            if (Count < 1)
                return null;
            return Events[Count - 1].ID;
        }
    }
}
