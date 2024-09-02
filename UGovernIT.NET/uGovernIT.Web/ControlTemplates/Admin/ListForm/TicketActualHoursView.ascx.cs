using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Core;
using System.Threading;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Web
{
    public partial class TicketActualHoursView : UserControl
    {
        DataTable data;
        public string TicketID { get; set; }
        public string WorkItem { get; set; }
        public string ModuleName { get; set; }
        public long TaskID { get; set; }
        public int TicketStageStep { get; set; }
        public UGITTask pTask { get; set; }
        public UGITTask SVCTask { get; set; }
        public bool HideActions { get; set; }
        public EventHandler AfterSave;

        protected bool allowZeroHourResolution;
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            allowZeroHourResolution = ConfigManager.GetValueAsBool(ConfigConstants.AllowZeroResolutionHrs);
            if (aspxGrid != null && HideActions)
                aspxGrid.Columns["CommandColumn"].Visible = false;

            if (aspxGrid.JSProperties.ContainsKey("cpDisableAddButton"))
                aspxGrid.JSProperties["cpDisableAddButton"] = false;
            else
                aspxGrid.JSProperties.Add("cpDisableAddButton", false);


            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //set ticket id as work item if work item is not exist
            if (string.IsNullOrWhiteSpace(WorkItem))
                WorkItem = TicketID;
        }

        protected override void OnPreRender(EventArgs e)
        {
            aspxGrid.DataBind();
            base.OnPreRender(e);
        }

        protected void aspxGrid_DataBinding(object sender, EventArgs e)
        {
            if (data == null)
            {
                data = loadData();
            }
            aspxGrid.DataSource = data;
        }

        protected DataTable loadData()
        {
            //Expression<Func<ActualHour, bool>> exp = (t) => true;
            //exp = exp.And(x => x.ModuleNameLookup == ModuleName);
            //exp = exp.And(x => x.TicketID == TicketID);

            //if (TaskID > 0)
            //{
            //    exp = exp.And(x => x.TaskID == TaskID);
            //}

            //TicketHoursManager ticketHourManager = new TicketHoursManager(HttpContext.Current.GetManagerContext());
            //List<ActualHour> lstTicketHours = ticketHourManager.Load(exp);
            //if (lstTicketHours != null && lstTicketHours.Count > 0)
            //{
            //    //dt.DefaultView.Sort = string.Format("{0} ASC, {1} ASC", DatabaseObjects.Columns.WorkDate, DatabaseObjects.Columns.Resource);
            //    //dt = dt.DefaultView.ToTable();

            //}
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            values.Add("@ModuleName", ModuleName);
            values.Add("@TicketID", TicketID);
            if(!string.IsNullOrEmpty(TaskID.ToString()) && TaskID > 0)
                values.Add("@TaskID", TaskID);
            DataTable dt = GetTableDataManager.GetData(DatabaseObjects.Tables.TicketHours, values);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = string.Format("{0} ASC, {1} ASC", DatabaseObjects.Columns.WorkDate, DatabaseObjects.Columns.Resource);
                dt = dt.DefaultView.ToTable();
            }
            return dt;
        }

        protected void aspxGrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            List<Tuple<int, DateTime>> updatedItems = new List<Tuple<int, DateTime>>();
            TicketHoursManager tHoursHelper = new TicketHoursManager(HttpContext.Current.GetManagerContext());

            #region InsertNewValues
            if (e.InsertValues.Count > 0)
            {
                for (int i = 0; i < e.InsertValues.Count; i++)
                {
                    ActualHourHelper hourHelper = new ActualHourHelper();
                    var item = e.InsertValues[i];

                    if (item != null)
                    {
                        OrderedDictionary newValues = item.NewValues;
                        if (newValues != null)
                        {
                            hourHelper.TimeSpent = UGITUtility.StringToDouble(newValues[DatabaseObjects.Columns.HoursTaken]);
                            hourHelper.WorkDate = UGITUtility.StringToDateTime(newValues[DatabaseObjects.Columns.WorkDate]);
                            hourHelper.TicketId = TicketID;
                            hourHelper.WorkItem = WorkItem;
                            hourHelper.ResolutionDescription = Convert.ToString(newValues[DatabaseObjects.Columns.TicketComment]);
                            hourHelper.TicketStageStep = TicketStageStep;
                            hourHelper.TaskId = TaskID;
                            hourHelper.Title = Convert.ToString(newValues[DatabaseObjects.Columns.Title]);
                            hourHelper.ItemId =0;
                            tHoursHelper.CreateOrUpdateOrDeleteActualHours("Create", hourHelper, ModuleName, true);
                        }
                    }
                }
            }
            #endregion

            #region UpdateOldValue

            foreach (var args in e.UpdateValues)
            {
                OrderedDictionary newValues = args.NewValues;
                if (newValues != null)
                {
                    ActualHourHelper hourHelper = new ActualHourHelper();
                    hourHelper.TimeSpent = UGITUtility.StringToDouble(newValues[DatabaseObjects.Columns.HoursTaken]);
                    hourHelper.WorkDate = UGITUtility.StringToDateTime(newValues[DatabaseObjects.Columns.WorkDate]);
                    hourHelper.TicketId = TicketID;
                    hourHelper.WorkItem = WorkItem;
                    hourHelper.ResolutionDescription = Convert.ToString(newValues[DatabaseObjects.Columns.TicketComment]);
                    hourHelper.TicketStageStep = TicketStageStep;
                    hourHelper.TaskId = TaskID;
                    hourHelper.ItemId = UGITUtility.StringToInt(args.Keys[DatabaseObjects.Columns.ID]);
                    tHoursHelper.CreateOrUpdateOrDeleteActualHours("Update", hourHelper, ModuleName, true);
                }

            }
            #endregion

            #region DeleteValues
            if (e.DeleteValues.Count > 0)
            {
                foreach (var args in e.DeleteValues)
                {
                    int id = UGITUtility.StringToInt(args.Keys[DatabaseObjects.Columns.ID]);

                    if (id > 0)
                    {
                        ActualHourHelper hourHelper = new ActualHourHelper();
                        hourHelper.TicketId = TicketID;
                        hourHelper.WorkItem = WorkItem;
                        hourHelper.TaskId = TaskID;
                        hourHelper.ItemId = id;
                        tHoursHelper.CreateOrUpdateOrDeleteActualHours("Delete", hourHelper, ModuleName, true);
                    }
                }
            }
            #endregion

            data = null;
            aspxGrid.DataBind();
            e.Handled = true;
            aspxGrid.CancelEdit();

            //Update Ticket Actual hours field of task
            double totalHoursTaken = 0;
            if (data != null && data.Rows.Count > 0)
                totalHoursTaken = UGITUtility.StringToDouble(data.Compute("Sum(HoursTaken)", ""));

            if (pTask != null)
            {
                pTask.ActualHours = totalHoursTaken;
                pTask.EstimatedRemainingHours = pTask.EstimatedHours - pTask.ActualHours;
                if (pTask.EstimatedRemainingHours <= 0)
                    pTask.EstimatedRemainingHours = 0;
                UGITTask task = pTask;
                //UGITTaskHelper.SaveTask(SPContext.Current.Web, ref task, ModuleName);
                aspxGrid.JSProperties.Add("cptotalActualHours", totalHoursTaken);

            }
            else if (SVCTask != null)
            {
                SVCTask.ActualHours = totalHoursTaken;
                //SVCTask.Save();
                aspxGrid.JSProperties.Add("cptotalActualHours", totalHoursTaken);

            }


            if (AfterSave != null)
            {
                AfterSave(sender, e);
            }

        }

        protected void aspxGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (HideActions)
                e.Editor.ReadOnly = true;
            else
                e.Editor.ReadOnly = false;
        }

        protected void aspxGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues[DatabaseObjects.Columns.WorkDate] = DateTime.Now;
            e.NewValues[DatabaseObjects.Columns.Resource] = HttpContext.Current.CurrentUser().Name;
        }

        protected void aspxGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Keys.Count <= 0)
                return;

            int itemId = UGITUtility.StringToInt(e.Keys[DatabaseObjects.Columns.ID]);

            if (itemId > 0)
            {
                //SPListItem item = SPListHelper.GetSPListItem(DatabaseObjects.Lists.TicketHours, itemId, SPContext.Current.Web);
                //if (item != null)
                //    item.Delete();
            }

            data = loadData();
            aspxGrid.DataSource = data;
            aspxGrid.DataBind();
            e.Cancel = true;


            //Update Ticket Actual hours field of task
            double totalHoursTaken = 0;
            if (data != null && data.Rows.Count > 0)
                totalHoursTaken = UGITUtility.StringToDouble(data.Compute("Sum(HoursTaken)", ""));

            if (pTask != null)
            {
                pTask.ActualHours = totalHoursTaken;
                pTask.EstimatedRemainingHours = pTask.EstimatedHours - pTask.ActualHours;
                if (pTask.EstimatedRemainingHours <= 0)
                    pTask.EstimatedRemainingHours = 0;
                UGITTask task = pTask;
                //UGITTaskHelper.SaveTask(SPContext.Current.Web, ref task, ModuleName);
                aspxGrid.JSProperties.Add("cptotalActualHours", totalHoursTaken);

            }
            else if (SVCTask != null)
            {
                SVCTask.ActualHours = totalHoursTaken;
                //SVCTask.Save();
                aspxGrid.JSProperties.Add("cptotalActualHours", totalHoursTaken);

            }

            if (AfterSave != null)
            {
                AfterSave(sender, e);
            }
        }

        protected void aspxGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType == ColumnCommandButtonType.Delete && HideActions)
                e.Visible = false;
        }

        protected void aspxGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (UGITUtility.StringToDateTime(e.NewValues[DatabaseObjects.Columns.WorkDate]) == DateTime.MinValue)
            {
                e.Errors.Add(aspxGrid.Columns["WorkDate"], "Please enter date.");
                if (e.IsNewRow)
                {
                    if (aspxGrid.JSProperties.ContainsKey("cpDisableAddButton"))
                        aspxGrid.JSProperties["cpDisableAddButton"] = true;
                    else
                        aspxGrid.JSProperties.Add("cpDisableAddButton", true);
                }
            }
            else if (allowZeroHourResolution && e.NewValues[DatabaseObjects.Columns.HoursTaken] == null)
            {
                e.Errors.Add(aspxGrid.Columns[DatabaseObjects.Columns.HoursTaken], "Please enter 0 to 24");
                if (e.IsNewRow)
                {
                    if (aspxGrid.JSProperties.ContainsKey("cpDisableAddButton"))
                        aspxGrid.JSProperties["cpDisableAddButton"] = true;
                    else
                        aspxGrid.JSProperties.Add("cpDisableAddButton", true);
                }
            }
            else if (!allowZeroHourResolution && UGITUtility.StringToDouble(e.NewValues[DatabaseObjects.Columns.HoursTaken]) < 0.25)
            {
                e.Errors.Add(aspxGrid.Columns[DatabaseObjects.Columns.HoursTaken], "Please enter 0.25 to 24");
                if (e.IsNewRow)
                {
                    if (aspxGrid.JSProperties.ContainsKey("cpDisableAddButton"))
                        aspxGrid.JSProperties["cpDisableAddButton"] = true;
                    else
                        aspxGrid.JSProperties.Add("cpDisableAddButton", true);
                }
            }
        }
    }
}
