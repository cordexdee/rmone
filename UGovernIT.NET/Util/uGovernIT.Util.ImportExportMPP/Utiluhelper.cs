using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.Util.ImportExportMPP
{

    public static class Utiluhelper
    {
        public static DateTime GetDateFromJavaUtil(java.util.Date strdate)
        {
            java.util.Date d = strdate;
            string javadate = new java.text.SimpleDateFormat("MM/dd/yyyy").format(d);

            //Added 27 jan 2020
            DateTime dtime = DateTime.ParseExact(javadate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            //DateTime dtime = DateTime.MinValue; commented 27 jan 2020
            //dtime = DateTime.Parse(javadate); commented 27 jan 2020
            //
            return dtime;
        }
        public static object GetProjectFormatUnit(string unit)
        {
            //string retUnit = string.Empty;
            object returnUnit;

            switch (unit)
            {
                case "d":
                    returnUnit = ProjectFormatUnit.pjDays;
                    //retUnit = "pjDays";
                    break;
                case "ed":
                    //retUnit = "pjElapsedDays";
                    returnUnit = ProjectFormatUnit.pjElapsedDays;
                    break;
                case "eh":
                    //retUnit = "pjElapsedHours";
                    returnUnit = ProjectFormatUnit.pjElapsedHours;
                    break;
                case "em":
                    //retUnit = "pjElapsedMinutes";
                    returnUnit = ProjectFormatUnit.pjElapsedMinutes;
                    break;
                case "emo":
                    //retUnit = "pjElapsedMonths";
                    returnUnit = ProjectFormatUnit.pjElapsedMonths;
                    break;
                //case "e%":
                //    retUnit = "pjDays";
                //    break;
                case "ew":
                    //retUnit = "pjElapsedWeeks";
                    returnUnit = ProjectFormatUnit.pjElapsedWeeks;
                    break;
                //case "ey":
                //    retUnit = "pjDays";
                //    break;
                case "h":
                    //retUnit = "pjHours";
                    returnUnit = ProjectFormatUnit.pjHours;
                    break;
                case "m":
                    //retUnit = "pjMinutes";
                    returnUnit = ProjectFormatUnit.pjMinutes;
                    break;
                case "mo":
                    //retUnit = "pjMonths";
                    returnUnit = ProjectFormatUnit.pjMonths;
                    break;
                case "%":
                    //retUnit = "PjPercentage";
                    returnUnit = ProjectFormatUnit.PjPercentage;
                    break;
                case "w":
                    //retUnit = "pjWeeks";
                    returnUnit = ProjectFormatUnit.pjWeeks;
                    break;
                //case "y":
                //    retUnit = "pjDays";
                //    break;
                default:
                    return returnUnit = null;
            }
            //return retUnit;
            return returnUnit;
        }
        public static java.util.Date GetJavaDateFromSystemDate(string systemdate)
        {
            DateTime dd = Convert.ToDateTime(systemdate);
            java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("MM-dd-yyyy");
            // java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("MM/dd/yyyy",java.text.DateFormatSymbols.getInstance(java.util.Locale.ENGLISH));
            sdf.setTimeZone(java.util.TimeZone.getDefault());
            string d = sdf.format(sdf.parse(dd.Date.ToString("MM-dd-yyyy")));
            java.util.Date sDate = sdf.parse(d);
            return sDate;
        }
        public static double DateDiff(System.DateTime startDate, System.DateTime endDate)
        {
            double diff = 0;
            System.TimeSpan TS = new System.TimeSpan(startDate.Ticks - endDate.Ticks);
            diff = Convert.ToDouble(TS.TotalDays);
            return diff;
        }
    }
}
