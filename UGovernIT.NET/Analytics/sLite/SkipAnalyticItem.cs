using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLite
{
    public class SkipAnalyticItem
    {
        public string Key { get; set; }
        public bool IsSkip { get; set; }


        public static List<SkipAnalyticItem> ProcessSkipLogic(Analytic analytic)
        {
            List<SkipAnalyticItem> items = new List<SkipAnalyticItem>();
            bool skipMetric = true;
            bool skipKpi = true;

            SkipAnalyticItem item = null;
            foreach (sLite.KPI kpi in analytic.KPIs)
            {
                skipKpi = true;
                foreach (sLite.Metric metric in kpi.Metrics)
                {
                    skipMetric = true;
                    foreach (sLite.Function func in metric.Functions)
                    {
                        item = new SkipAnalyticItem();
                        item.Key = func.ID.ToString();
                        item.IsSkip = true;
                        items.Add(item);
                        if (string.IsNullOrWhiteSpace(func.AskFrom) || func.AskFrom.Equals("AskFromUser"))
                        {
                            skipMetric = false;
                            item.IsSkip = false;
                        }
                    }

                    item = new SkipAnalyticItem();
                    item.Key = metric.ID.ToString();
                    item.IsSkip = true;
                    items.Add(item);
                    if (!skipMetric)
                    {
                        item.IsSkip = false;
                        skipKpi = false;
                    }

                }

                item = new SkipAnalyticItem();
                item.Key = kpi.ID.ToString();
                item.IsSkip = true;
                items.Add(item);
                if (!skipKpi)
                {
                    item.IsSkip = false;
                }
            }
            return items;
        }
    }
}
