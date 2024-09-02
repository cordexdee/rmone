using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Report.Intrastructure
{
    public class ReportScheduleFilterBase : UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        ScheduleActionsManager scheduleActionsManager;
        public long scheduleID { get; set; }
        public Dictionary<string, object> Filterproperties { get; set; }
        SchedulerAction spListitem;
        public ReportScheduleFilterBase()
        {
            scheduleActionsManager = new ScheduleActionsManager(applicationContext);
        }
        public void SaveFilters()
        {
            if (scheduleID > 0)
            {
                spListitem = scheduleActionsManager.LoadByID(scheduleID);
                spListitem.ActionTypeData = UGITUtility.SerializeDicObject(Filterproperties);
                scheduleActionsManager.Update(spListitem);
            }
        }


        public Dictionary<string, object> LoadFilter(long id)
        {
            if (id > 0)
            {  
                spListitem = scheduleActionsManager.LoadByID(id);
                if (spListitem.ActionTypeData != null)
                {
                    Filterproperties = UGITUtility.DeserializeDicObject(spListitem.ActionTypeData);
                }
            }
            return Filterproperties;
        }
    }
}