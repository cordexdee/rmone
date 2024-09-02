using System;
using System.Web.UI;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility.Entities;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Web
{
    public partial class ResourceCostView : UserControl
    {
        public string TicketId { get; set; }
        public DataTable LabourChargesData { get; set; }
        public string ModuleName { get; set; }
        List<ResourceWorkItems> resourceWorkItemsList;
        List<long> ticketWorkItems;
        DataTable resourceCostData;
        public string resourceInfo = string.Empty;
        ApplicationContext Appcontect;
        ResourceWorkItemsManager ResourceWorkItemsMGR;
        ConfigurationVariableManager ConfigVariableMGR;
        protected override void OnInit(EventArgs e)
        {
            Appcontect = HttpContext.Current.GetManagerContext();
            ConfigVariableMGR = new ConfigurationVariableManager(Appcontect);
            ResourceWorkItemsMGR = new ResourceWorkItemsManager(Appcontect);
            resourceWorkItemsList = ResourceWorkItemsMGR.Load();

            ticketWorkItems = GetTicketWorkItems();
            labourChargesGrid.DataSource = GetResourceData();
            labourChargesGrid.DataBind();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Method to get Work Items from ResourceWorkItems list
        /// <summary>
        /// This method is used to get work items from ResourceWorkItems list and set workItem Id in a list.
        /// </summary>
        protected List<long> GetTicketWorkItems()
        {
            List<long> workItemsList = new List<long>();
            if (string.IsNullOrEmpty(TicketId) || string.IsNullOrEmpty(ModuleName))
                return workItemsList;

            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.WorkItemType, ModuleName));
            requiredQuery.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.WorkItem, TicketId));

            if (resourceWorkItemsList != null && resourceWorkItemsList.Count > 0)
            {
                //SPQuery sQuery = new SPQuery();
                //sQuery.ViewFields = string.Format("<FieldRef Name= '{0}' /><FieldRef Name= '{1}' />", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Resource);
                //sQuery.ViewFieldsOnly = true;
                //sQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
                //SPListItemCollection resultCollection = resourceWorkItemsList.GetItems(sQuery);

                //if (resultCollection != null && resultCollection.Count > 0)
                //{
                //    foreach (SPListItem item in resultCollection)
                //    {
                //        workItemsList.Add(uHelper.StringToInt(item[DatabaseObjects.Columns.Id]));
                //    }
                //}
                List<ResourceWorkItems> resultCollection = resourceWorkItemsList.Where(x => x.WorkItemType == ModuleName && x.WorkItem == TicketId).ToList();
                foreach(ResourceWorkItems item in resultCollection)
                {
                    workItemsList.Add(item.ID);
                }
            }
            
            return workItemsList;
        }
        #endregion Method to get Work Items from ResourceWorkItems list

        #region Method to get Labour Charges from ResourceTimeSheet List
        /// <summary>
        /// This method is used to get Labour Charges from ResourceTimeSheet List
        /// </summary>
        /// <returns>Datatable</returns>
        protected DataTable GetResourceData()
        {
            ResourceTimeSheetManager resourceTimesheetManager = new ResourceTimeSheetManager(Appcontect);
            resourceCostData = new DataTable();
            resourceCostData.Columns.Add(DatabaseObjects.Columns.ResourceId, typeof(string));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.ResourceName, typeof(string));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.HourlyRate, typeof(double));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.HoursTaken, typeof(double));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.LabourCharges, typeof(double));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.WorkDate, typeof(DateTime));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.StartDate, typeof(DateTime));
            resourceCostData.Columns.Add(DatabaseObjects.Columns.EndDate, typeof(DateTime));

            if (ticketWorkItems == null || ticketWorkItems.Count == 0)
                return resourceCostData;

            string requiredQuery = string.Empty;
            bool isExceedLimit = ticketWorkItems.Count > 200;

            Expression<Func<ResourceTimeSheet, bool>> exp = (t) => true;
            //if (!isExceedLimit)
            exp = exp.And(x => ticketWorkItems.Contains(x.ResourceWorkItemLookup));
            //    requiredQuery = string.Format("<In><FieldRef Name='{0}' LookupId='TRUE' /><Values>{1}</Values></In>", DatabaseObjects.Columns.ResourceWorkItemLookup, string.Join("", ticketWorkItems.Select(x => string.Format("<Value Type='Integer'>{0}</Value>", x))));

            List<ResourceTimeSheet> resultCollection = resourceTimesheetManager.Load(exp);
            if (resultCollection == null || resultCollection.Count == 0)
                return resourceCostData;

            foreach (ResourceTimeSheet item in resultCollection)
            {
                //Remove those records which aren't in ticketWorkItems list
                //if (isExceedLimit)
                //{
                //    SPFieldLookupValue workItemLookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ResourceWorkItemLookup]));
                //    if (workItemLookup != null)
                //    {
                //        bool isWorkItemExist = ticketWorkItems.Any(x => x.Equals(workItemLookup.LookupId));
                //        if (!isWorkItemExist)
                //            continue;
                //    }
                //    else
                //        continue;
                //}

                double hoursTaken = UGITUtility.StringToDouble(item.HoursTaken);

                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(item.Resource)))
                {
                    resourceCostData.Rows.Add(0, "Undefined", 0, hoursTaken, 0, UGITUtility.StringToDateTime(item.WorkDate));
                    continue;
                }

                int defaultHourlyRate = UGITUtility.StringToInt(ConfigVariableMGR.GetValue(ConfigConstants.ResourceHourlyRate)); // Default hourly rate
                string resourceLookup = UGITUtility.ObjectToString(item.Resource);
                if (!string.IsNullOrEmpty(resourceLookup))
                {
                    UserProfile user = Appcontect.UserManager.LoadById(resourceLookup);
                    if (user != null)
                    {
                        double totalCost = user.HourlyRate * hoursTaken;
                        resourceCostData.Rows.Add(resourceLookup, user.Name, user.HourlyRate, hoursTaken, totalCost, Convert.ToDateTime(item.WorkDate));
                    }
                    else
                    {
                        resourceCostData.Rows.Add(resourceLookup, resourceLookup, defaultHourlyRate, hoursTaken, defaultHourlyRate * hoursTaken, Convert.ToDateTime(item.WorkDate));
                    }
                }
                else
                {
                    resourceCostData.Rows.Add(0, Convert.ToString(item.Resource), defaultHourlyRate, hoursTaken, defaultHourlyRate * hoursTaken, Convert.ToDateTime(item.WorkDate));
                }
            }

            LabourChargesData = resourceCostData;

            // Apply groupby for mulitple records of same user for the same ticket
            if (resourceCostData != null && resourceCostData.Rows.Count > 0)
            {
                var groupedData = resourceCostData.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.ResourceName)).Select(y => new
                {
                    ResourceId = y.First().Field<string>(DatabaseObjects.Columns.ResourceId),
                    ResourceNameUser = y.Key,
                    HourlyRate = y.First().Field<double>(DatabaseObjects.Columns.HourlyRate),
                    StartDate = y.Min(s => s.Field<DateTime>(DatabaseObjects.Columns.WorkDate)).ToString("MMM-dd-yyyy"),
                    EndDate = y.Max(s => s.Field<DateTime>(DatabaseObjects.Columns.WorkDate)).ToString("MMM-dd-yyyy"),
                    HoursTaken = y.Sum(s => s.Field<double>(DatabaseObjects.Columns.HoursTaken)),
                    LabourCharges = y.Sum(s => s.Field<double>(DatabaseObjects.Columns.LabourCharges))
                });

                if (groupedData != null)
                    resourceCostData = UGITUtility.LINQResultToDataTable(groupedData.ToArray());
            }

            return resourceCostData;
        }
        #endregion Method to get Labour Charges from TicketHours List

        #region Timesheet Cost (Estimated Labor) grid Methods
        protected void labourChargesGrid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            if (e.VisibleIndex == grid.VisibleRowCount - 1)
            {
                e.Row.CssClass = "clsNthrowcolor";
            }
        }

        #region Method to create hyperlinks for Total Hours column
        /// <summary>
        /// This method is used to create hyperlinks for Total Hours column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void labourChargesGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.HoursTaken)
            {
                int index = e.VisibleIndex;
                HtmlAnchor aHoursTaken = (HtmlAnchor)labourChargesGrid.FindRowCellTemplateControl(index, e.DataColumn, "lnkHoursTaken");

                DataRow row = labourChargesGrid.GetDataRow(index);
                string resourceName = Convert.ToString(row[DatabaseObjects.Columns.ResourceName]);
                string resourceid = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ResourceId]);
                string resource = resourceid + Constants.Separator + resourceName;

                aHoursTaken.Attributes.Add("onclick", "OnTotalHoursClick(this, '" + resource + "' )");
                aHoursTaken.InnerText = e.CellValue.ToString();
            }
        }
        #endregion Method to create hyperlinks for Total Hours column

        #endregion Timesheet Cost (Estimated Labor) grid Methods

        #region Callback method of resourceHoursGridContainer
        protected void resourceHoursGridContainer_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                resourceInfo = e.Parameter;
                resourceHoursGrid.DataBind();
            }
        }
        #endregion Callback method of resourceHoursGridContainer

        #region DataBinding Method of resourceHoursGrid
        protected void resourceHoursGrid_DataBinding(object sender, EventArgs e)
        {
            resourceHoursGrid.DataSource = GetResourceHoursData();
        }
        #endregion DataBinding Method of resourceHoursGrid

        #region Method to get Resource Hours Data
        /// <summary>
        /// This Method is used to get Resource Hours Data on the basis of resource Id
        /// </summary>
        /// <returns>DataTable</returns>
        protected DataTable GetResourceHoursData()
        {
            DataTable dtHoursData = new DataTable();

            if (string.IsNullOrEmpty(resourceInfo) || LabourChargesData == null || LabourChargesData.Rows.Count == 0)
                return dtHoursData;

            string resourceId = resourceInfo.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
            var rows = LabourChargesData.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceId) == resourceId).Select(y => new
            {
                WorkDate = (y.Field<DateTime>(DatabaseObjects.Columns.WorkDate)).ToString("MMM-dd-yyyy"),
                HoursTaken = y.Field<double>(DatabaseObjects.Columns.HoursTaken)
            });

            if (rows != null)
            {
                dtHoursData = UGITUtility.LINQResultToDataTable(rows.ToArray());
            }

            return dtHoursData;
        }
        #endregion Method to get Resource Hours Data
    }
}
