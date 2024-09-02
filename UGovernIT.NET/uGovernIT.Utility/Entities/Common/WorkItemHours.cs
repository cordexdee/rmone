using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class WorkItemHours
    {
        public int WorkItemID;
        public double Mon;
        public double Tue;
        public double Wed;
        public double Thu;
        public double Fri;
        public double Sat;
        public double Sun;
        public string WorkItemType;
        public string WorkItem;
        public string SubWorkItem;
        public string SubSubWorkItem;

        public double GetHoursOfDay(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return this.Mon;
                case DayOfWeek.Tuesday:
                    return this.Tue;
                case DayOfWeek.Wednesday:
                    return this.Wed;
                case DayOfWeek.Thursday:
                    return this.Thu;
                case DayOfWeek.Friday:
                    return this.Fri;
                case DayOfWeek.Saturday:
                    return this.Sat;
                case DayOfWeek.Sunday:
                    return this.Sun;
                default:
                    return 0;
            }
        }

        public void SetHoursOfDay(DayOfWeek day, double hours)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    this.Mon = hours;
                    break;
                case DayOfWeek.Tuesday:
                    this.Tue = hours;
                    break;
                case DayOfWeek.Wednesday:
                    this.Wed = hours;
                    break;
                case DayOfWeek.Thursday:
                    this.Thu = hours;
                    break;
                case DayOfWeek.Friday:
                    this.Fri = hours;
                    break;
                case DayOfWeek.Saturday:
                    this.Sat = hours;
                    break;
                case DayOfWeek.Sunday:
                    this.Sun = hours;
                    break;
                default:
                    break;
            }
        }
    }

}
