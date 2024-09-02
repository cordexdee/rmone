using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Util.ImportExportMPP
{
    public enum ProjectFormatUnit
    {
        pjMinutes = 3,
        pjElapsedMinutes = 4,
        pjHours = 5,
        pjElapsedHours = 6,
        pjDays = 7,
        pjElapsedDays = 8,
        pjWeeks = 9,
        pjElapsedWeeks = 10,
        pjMonths = 11,
        pjElapsedMonths = 12,
        pjMinutesEstimated = 35,
        pjElapsedMinutesEstimated = 36,
        pjHoursEstimated = 37,
        pjElapsedHoursEstimated = 38,
        pjDaysEstimated = 39,
        pjElapsedDaysEstimated = 40,
        pjWeeksEstimated = 41,
        pjElapsedWeeksEstimated = 42,
        pjMonthsEstimated = 43,
        pjElapsedMonthsEstimated = 44,
        PjPercentage = 19

    }

    public enum ProjectTaskLinkType
    {
        pjFinishToFinish = 0,
        pjFinishToStart = 1,
        pjStartToFinish = 2,
        pjStartToStart = 3
    }

}
