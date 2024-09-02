using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.Helpers
{
    public class SummaryHelper
    {
        private ApplicationContext _applicationContext { get; set; }
        public SummaryHelper()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            
        }
        public string GetDueDateSummaryText(DataRow currentRow)
        {
            DateTime projectEndDate = DateTime.MinValue;
            if (UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.DueDate) is DateTime)
                projectEndDate = (DateTime)UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.DueDate);
            if (currentRow != null)
            {
                return projectEndDate.ToString("MMM-d-yyyy");
            }
            else
            {
                return string.Empty;
            }

        }

        public string GetStartDateSummaryText(DataRow currentRow)
        {
            DateTime projectStartDate = DateTime.MinValue;
            if (UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.StartDate) is DateTime)
                projectStartDate = (DateTime)UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.StartDate);
            if (currentRow != null)
            {
                return projectStartDate.ToString("MMM-d-yyyy");
            }
            else
            {
                return string.Empty;
            }

        }

        public string GetStartDateSummaryText(DateTime date)
        {
            DateTime projectStartDate = DateTime.MinValue;
            if (date != null)
                projectStartDate = date;
            if (date != null)
            {
                return projectStartDate.ToString("MMM-d-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetPercentSummaryText(string value)
        {
            double pctComplete = UGITUtility.StringToDouble(value);           
            if (value != null)
            {
                if (pctComplete > 99.9 && pctComplete < 100)
                {
                    pctComplete = 99.9; // Don't show 100% unless all the way done!
                    return string.Format("{0} {1}% complete", "Project is ", pctComplete);
                }
                else
                {
                    pctComplete = Math.Round(pctComplete, 1, MidpointRounding.AwayFromZero); // Round to nearest 0.1                    
                    return string.Format("{0} {1}% complete", "Project is ", pctComplete);
                }
            }
            else
            {
                return string.Empty;
            }          

        }

        public string GetEstimatedHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);                
                return string.Format("{0}", hrs.ToString("#0"));
            }
            else
                return string.Empty;
        }

        public string GetActualHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);                
                return string.Format("{0}", hrs.ToString("#0"));
            }
            else
                return string.Empty;
        }

        public string GetEstimatedRemainingHoursSummaryText(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);
                return string.Format("{0}", hrs.ToString("#0"));
            }
            else
                return string.Empty;
        }

        public string GetDurationSummary(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                double hrs = UGITUtility.StringToDouble(value);
                return string.Format("ERH: {0}", hrs.ToString("#,0.#"));
            }
            else
                return string.Empty;
        }

        
    }
}