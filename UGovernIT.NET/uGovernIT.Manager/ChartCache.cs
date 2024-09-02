using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using uGovernIT.Manager;

namespace uGovernIT
{
    public class ChartCacheData
    {
        public ChartCacheData(Guid guid)
        {
            //listTaskCache = new List<TaskData>();
            this.collectionId = guid;
        }
        public Guid collectionId { get; set; }
        public List<ChartCachedDataPoints> charts;
    }
    public class ChartCache
    {
         private ChartCache(Guid guid)
        {
            collectionId = guid;
        }

        public static Guid collectionId { get; set; }

        private static List<ChartCacheData> listChartCache { get; set; }

        public static ChartCacheData GetChartCacheInstance()
        {
            //if (SPContext.Current != null)
            //{
            //    return GetChartCacheInstance(SPContext.Current.Site.ID);
            //}
            //else
            //{
                return GetChartCacheInstance(collectionId);
           // }
        }

        public static ChartCacheData GetChartCacheInstance(Guid id)
        {
            if (listChartCache == null)
                listChartCache = new List<ChartCacheData>();

            collectionId = id;
            if (listChartCache != null && listChartCache.Exists(x => x.collectionId == id))
            {
                return listChartCache.Find(x => x.collectionId == id);
            }
            else
            {
                ChartCacheData chartCache = new ChartCacheData(id);
                listChartCache.Add(chartCache);
                return chartCache;
            }
        }

        private static object lockObject = new object();

        public static void ClearCache()
        {
            lock (lockObject)
            {
                GetChartCacheInstance().charts = null;
            }
        }

        public static void AddChartCache(ChartCachedDataPoints chartPoints)
        {
            if (GetChartCacheInstance().charts == null)
            {
                lock (lockObject)
                {
                    if (GetChartCacheInstance().charts == null)
                    {
                        GetChartCacheInstance().charts = new List<ChartCachedDataPoints>();

                    }
                }
            }

            lock (lockObject)
            {
                ChartCachedDataPoints points = GetChartCacheInstance().charts.FirstOrDefault(x => x.ChartID == chartPoints.ChartID);
                if (points != null)
                {
                    GetChartCacheInstance().charts.Remove(points);
                    GetChartCacheInstance().charts.Add(chartPoints);
                }
                else
                {
                    GetChartCacheInstance().charts.Add(chartPoints);
                }
            }
        }

        public static void RemoveChartFromCache(string dashboardID)
        {
            if (GetChartCacheInstance().charts == null || GetChartCacheInstance().charts.Count <= 0)
            {
                return;
            }

            lock (lockObject)
            {
                ChartCachedDataPoints chartPoints = GetChartCacheInstance().charts.FirstOrDefault(x => x.ChartID == dashboardID);
                if (chartPoints != null)
                {
                    GetChartCacheInstance().charts.Remove(chartPoints);
                }
            }
        }

        public static void RefreshChartCache(DevxChartHelper chartHelper, DataTable dataPoints = null)
        {
            ChartCachedDataPoints newChartDatapoints = new ChartCachedDataPoints();
            newChartDatapoints.ChartID = chartHelper.ChartSetting.ChartId.ToString();
            string imageMapId = Guid.NewGuid().ToString();
            newChartDatapoints.LastUpdated = DateTime.Now;
            newChartDatapoints.Title = chartHelper.ChartTitle;
            newChartDatapoints.IsUpdating = false;
            newChartDatapoints.DataPoints = dataPoints;
            ChartCache.AddChartCache(newChartDatapoints);
        }

        public static void RefreshChartCache(Chart cPChart, ChartHelper chartHelper, DataTable dataPoints = null)
        {
            ChartCachedDataPoints newChartDatapoints = new ChartCachedDataPoints();
            newChartDatapoints.ChartID = chartHelper.ChartSetting.ChartId.ToString();
            newChartDatapoints.ChartSeries = cPChart.Series;
            string imageMapId = Guid.NewGuid().ToString();
            newChartDatapoints.LastUpdated = DateTime.Now;
            newChartDatapoints.Title = chartHelper.ChartTitle;
            newChartDatapoints.IsUpdating = false;
            newChartDatapoints.DataPoints = dataPoints;
            ChartCache.AddChartCache(newChartDatapoints);
        }

        public static ChartCachedDataPoints GetChartCache(string dashboardID)
        {
            ChartCachedDataPoints chartPoints = null;
            lock (lockObject)
            {
                if (GetChartCacheInstance().charts == null || GetChartCacheInstance().charts.Count <= 0)
                {
                    return chartPoints;
                }

                chartPoints = GetChartCacheInstance().charts.FirstOrDefault(x => x.ChartID == dashboardID);
            }
            return chartPoints;
        }

        public static void SetChartToUpdate(string dashboardID)
        {
            ChartCachedDataPoints chartPoints = null;
            lock (lockObject)
            {
                if (GetChartCacheInstance().charts == null || GetChartCacheInstance().charts.Count <= 0)
                {
                    return;
                }

                chartPoints = GetChartCacheInstance().charts.FirstOrDefault(x => x.ChartID == dashboardID);
                if (chartPoints == null)
                {
                    return;
                }

                chartPoints.IsUpdating = true;

            }
        }
    }
}
