using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    [Serializable]
    public class CustomEventList : BindingList<Appointment>
    {
        public void AddRange(CustomEventList events)
        {
            foreach (Appointment customEvent in events)
                this.Add(customEvent);
        }
        public int GetEventIndex(object eventId)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].ID == Convert.ToInt64(eventId))
                    return i;
            return -1;
        }
    }
}
