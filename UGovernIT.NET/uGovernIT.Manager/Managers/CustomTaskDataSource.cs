using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{

    [Serializable]
    public class CustomTaskList : BindingList<UGITTask>
    {
        public void AddRange(CustomTaskList events)
        {
            foreach (UGITTask customEvent in events)
                this.Add(customEvent);
        }
        public int GetEventIndex(object eventId)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].ID == Convert.ToInt32(eventId))
                    return i;
            return -1;
        }
    }

    public class CustomTaskDataSource
    {
        CustomTaskList events;
        public CustomTaskDataSource(CustomTaskList events)
        {
            if (events == null)
                DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events");
            this.events = events;
        }
        public CustomTaskDataSource()
        : this(new CustomTaskList()) {
        }
        public CustomTaskList Events { get { return events; } set { events = value; } }
        public int Count { get { return Events.Count; } }


        #region ObjectDataSource methods
        public object InsertMethodHandler(UGITTask customEvent)
        {
            Object id = customEvent.GetHashCode();
            customEvent.ID = Convert.ToInt32(id);
            Events.Add(customEvent);
            return id;
        }
        public void DeleteMethodHandler(UGITTask customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.ID);
            if (eventIndex >= 0)
                Events.RemoveAt(eventIndex);
        }
        public void UpdateMethodHandler(UGITTask customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.ID);
            if (eventIndex >= 0)
            {
                Events.RemoveAt(eventIndex);
                Events.Insert(eventIndex, customEvent);
            }
        }
        public IEnumerable SelectMethodHandler()
        {
            CustomTaskList result = new CustomTaskList();
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
