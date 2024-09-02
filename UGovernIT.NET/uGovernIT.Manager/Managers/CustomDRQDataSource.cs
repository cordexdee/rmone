using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Managers
{
    [Serializable]
    public class CustomDRQEvent
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualCompletionDate { get; set; }
        public string TicketId { get; set; }
        public string Status { get; set; }
        public string TenantID { get; set; }
        public string ModuleStepLookup { get; set; }
        public int StageStep { get; set; }
    }

    [Serializable]
    public class CustomDRQList : BindingList<CustomDRQEvent>
    {
        public void AddRange(CustomDRQList events)
        {
            foreach (CustomDRQEvent customEvent in events)
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

    public class CustomDRQDataSource
    {
        CustomDRQList events;
        public CustomDRQDataSource(CustomDRQList events)
        {
            if (events == null)
                DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events");
            this.events = events;
        }
        public CustomDRQDataSource()
        : this(new CustomDRQList())
        {
        }
        public CustomDRQList Events { get { return events; } set { events = value; } }
        public int Count { get { return Events.Count; } }


        #region ObjectDataSource methods
        public object InsertMethodHandler(CustomDRQEvent customEvent)
        {
            Object id = customEvent.GetHashCode();
            customEvent.ID = Convert.ToInt32(id);
            Events.Add(customEvent);
            return id;
        }
        public void DeleteMethodHandler(CustomDRQEvent customEvent)
        {
            int eventIndex = Events.GetEventIndex(customEvent.ID);
            if (eventIndex >= 0)
                Events.RemoveAt(eventIndex);
        }
        public void UpdateMethodHandler(CustomDRQEvent customEvent)
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
            CustomDRQList result = new CustomDRQList();
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

        public static List<CustomDRQEvent> LoadDrqProjects(string moduleName, ApplicationContext context)
        {
            List<CustomDRQEvent> drqList = new List<CustomDRQEvent>();
            TicketManager tManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            DataTable dtAllTickets = tManager.GetAllTickets(moduleManager.GetByName(moduleName));
            drqList = UGITUtility.ConvertCustomDataTable<CustomDRQEvent>(dtAllTickets);
            return drqList;
        }
    }
}
