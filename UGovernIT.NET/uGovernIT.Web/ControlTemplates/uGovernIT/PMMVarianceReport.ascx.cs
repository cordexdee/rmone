using DevExpress.XtraScheduler.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PMMVarianceReport : UserControl
    {
        public int PMMID;
        public string FrameId { get; set; }
        private bool isBindVarianceDone;
        public bool IsReadOnly { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfile currentUser;
        Ticket ticket = null;
        string TicketId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = HttpContext.Current.CurrentUser();
            ticket = new Ticket(context, "");
        }
        protected override void OnPreRender(EventArgs e)
        {
            // Check whether current logged in user is authorise to edit item or not.
            DataRow pmmItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.ID} = {PMMID} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select()[0];
            TicketId = UGITUtility.ObjectToString(pmmItem[DatabaseObjects.Columns.TicketId]);
            if (!IsReadOnly)
            {
                if (!Ticket.IsActionUser(context, pmmItem, context.CurrentUser) && Ticket.IsDataEditor(pmmItem, context))
                    IsReadOnly = true;
            }

            if (IsReadOnly)
            {
                projectVarianceEditMode.Visible = false;
                projectVarianceReadMode.Visible = true;
                BindReadOnlyContVariances();
            }
            else
            {
                projectVarianceEditMode.Visible = true;
                projectVarianceReadMode.Visible = false;
                if (!isBindVarianceDone)
                    BindContVariances();
            }
            base.OnPreRender(e);
        }

        #region project cost variances
        private void BindContVariances()
        {
            lvCostVariances.DataSource = GetCostVariancetable();
            lvCostVariances.DataBind();
            isBindVarianceDone = true;
        }

        private void BindReadOnlyContVariances()
        {
            lvRoCostVariances.DataSource = GetCostVariancetable();
            lvRoCostVariances.DataBind();
        }

        private DataTable GetCostVariancetable()
        {
            DataRow pmmProject = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.ID} = {PMMID} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select()[0];
            DataTable vTable = new DataTable();
            vTable.Columns.Add(DatabaseObjects.Columns.Title);
            vTable.Columns.Add("Plan");
            vTable.Columns.Add("Replan");
            vTable.Columns.Add("Actual");
            vTable.Columns.Add("Variance1");
            vTable.Columns.Add("Variance2");
            vTable.Columns.Add("Note");
            try
            {
                double planAmount = 0;
                double replanAmount = 0;
                double actualAmount = 0;
                int planDays = 0;
                int replanDays = 0;
                int actualDays = 0;
                GetProjectCosts(UGITUtility.ObjectToString(pmmProject[DatabaseObjects.Columns.TicketId]) , ref planAmount, ref replanAmount, ref actualAmount);
                GetProjectSchedues(UGITUtility.ObjectToString(pmmProject[DatabaseObjects.Columns.TicketId]), ref planDays, ref replanDays, ref actualDays);
                double v1 = (actualAmount - planAmount);
                double v2 = (actualAmount - replanAmount);
                DataRow costRow = vTable.NewRow();
                costRow[DatabaseObjects.Columns.Title] = "Cost";
                costRow["Plan"] = "$" + planAmount;
                costRow["Replan"] = "$" + replanAmount;
                costRow["Actual"] = "$" + actualAmount;
                costRow["Variance1"] = "$" + (v1 > 0 ? v1.ToString() : "(" + v1.ToString() + ")");
                costRow["Variance2"] = "$" + (v2 > 0 ? v1.ToString() : "(" + v2.ToString() + ")");
                costRow["Note"] = pmmProject[DatabaseObjects.Columns.ProjectCostNote] != null ? pmmProject[DatabaseObjects.Columns.ProjectCostNote] : string.Empty;
                vTable.Rows.Add(costRow);

                DataRow scheduleRow = vTable.NewRow();
                scheduleRow[DatabaseObjects.Columns.Title] = "Schedule";
                scheduleRow["Plan"] = planDays + " Days";
                scheduleRow["Replan"] = replanDays + " Days";
                scheduleRow["Actual"] = actualDays + " Days";
                scheduleRow["Variance1"] = (actualDays - planDays) + " Days";
                scheduleRow["Variance2"] = (actualDays - replanDays) + " Days";
                scheduleRow["Note"] = pmmProject[DatabaseObjects.Columns.ProjectScheduleNote] != null ? pmmProject[DatabaseObjects.Columns.ProjectScheduleNote] : string.Empty;
                vTable.Rows.Add(scheduleRow);
            }
            catch (Exception ex){
                ULog.WriteException(ex);
            }
            return vTable;
        }
        protected void LvCostVariances_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            bool isUpdated = false;
            if (lvCostVariances.EditIndex > 0)
            {
                isUpdated = updateCostVariances(lvCostVariances.EditItem, lvCostVariances.EditIndex);
            }

            if (isUpdated || lvCostVariances.EditIndex <= 0)
            {
                lvCostVariances.EditIndex = e.NewEditIndex;
                BindContVariances();
            }
        }

        protected void LvCostVariances_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = lvCostVariances.Items[e.ItemIndex];
            if (updateCostVariances(item, e.ItemIndex))
            {
                lvCostVariances.EditIndex = -1;
                BindContVariances();
            }
        }

        private bool updateCostVariances(ListViewItem item, int editItemIndex)
        {
            bool isUpdated = false;
            TextBox txtNote = (TextBox)item.FindControl("txtNote");
            try
            {
                string key = lvCostVariances.DataKeys[editItemIndex].Value.ToString();
                //SPListItem pmmProject = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);
                DataRow pmmProject = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.ID} = {PMMID} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select()[0];

                costVarianceMessage.Text = "<b>Error updating note</b>";
                if (key.ToLower() == "cost")
                {
                    pmmProject[DatabaseObjects.Columns.ProjectCostNote] = txtNote.Text.Trim();
                    //pmmProject.UpdateOverwriteVersion();
                    ticket.CommitChanges(pmmProject);
                    costVarianceMessage.Text = "Updated Successfully";
                    isUpdated = true;
                }
                else if (key.ToLower() == "schedule")
                {
                    pmmProject[DatabaseObjects.Columns.ProjectScheduleNote] = txtNote.Text.Trim();
                    //pmmProject.UpdateOverwriteVersion();
                    ticket.CommitChanges(pmmProject);
                    costVarianceMessage.Text = "Updated Successfully";
                    isUpdated = true;
                }
            }
            catch { }

            return isUpdated;
        }

        protected void LvCostVariances_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvCostVariances.EditIndex = -1;
            BindContVariances();
        }


        private void GetProjectCosts(string TicketId, ref double planAmount, ref double replanAmount, ref double actualAmount)
        {
        
            DataTable planBudgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetHistory, $"{DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.BaselineId} = {1} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            if (planBudgets != null && planBudgets.Rows.Count > 0)
            {
                planAmount = planBudgets.AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount));
            }

            DataTable replanBudgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudget, $"{DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            if (replanBudgets != null && replanBudgets.Rows.Count > 0)
            {
                replanAmount = replanBudgets.AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount));
            }

            DataTable actualBudgets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetActuals, $"{DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

            if (actualBudgets != null && actualBudgets.Rows.Count > 0)
            {
                actualAmount = actualBudgets.AsEnumerable().Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount));
            }
        }

        private void GetProjectSchedues(string TicketId, ref int planDays, ref int replanDays, ref int actualDays)
        { 
            DataTable planTasks = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasksHistory, $"{DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.BaselineId} = {1} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataTable replanTasks = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasks, $"{DatabaseObjects.Columns.TicketId} = '{TicketId}' and {DatabaseObjects.Columns.ModuleNameLookup} = '{ModuleNames.PMM}' and {DatabaseObjects.Columns.UGITSubTaskType} not in ('Risk','Issue') and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (planTasks != null && planTasks.Rows.Count > 0)
            {
                DateTime startDate = (DateTime)planTasks.AsEnumerable().OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.StartDate)).ToArray()[0][DatabaseObjects.Columns.StartDate];
                DateTime endDate = (DateTime)planTasks.AsEnumerable().OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.DueDate)).ToArray()[0][DatabaseObjects.Columns.DueDate];
                if (endDate.Date.CompareTo(startDate.Date) > 0)
                {
                    TimeSpan tSpan = endDate.Subtract(startDate);
                    planDays = tSpan.Days;
                }
                else if (endDate.Date.CompareTo(startDate.Date) == 0)
                {
                    planDays = 1;
                }
            }

            if (replanTasks != null && replanTasks.Rows.Count > 0)
            {
                DateTime startDate = (DateTime)replanTasks.AsEnumerable().OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.StartDate)).ToArray()[0][DatabaseObjects.Columns.StartDate];
                DateTime endDate = (DateTime)replanTasks.AsEnumerable().OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.DueDate)).ToArray()[0][DatabaseObjects.Columns.DueDate];
                if (endDate.Date.CompareTo(startDate.Date) > 0)
                {
                    TimeSpan tSpan = endDate.Subtract(startDate);
                    replanDays = tSpan.Days;
                }
                else if (endDate.Date.CompareTo(startDate.Date) == 0)
                {
                    replanDays = 1;
                }
            }

            if (replanTasks != null && replanTasks.Rows.Count > 0)
            {
                DataRow[] Tasks1 = replanTasks.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Status).ToLower() == "completed").OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.StartDate)).ToArray();
                DataRow[] Tasks2 = replanTasks.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Status).ToLower() == "completed").OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.DueDate)).ToArray();
                if (Tasks1.Count() > 0)
                {
                    DateTime startDate = (DateTime)Tasks1[0][DatabaseObjects.Columns.StartDate];
                    DateTime endDate = (DateTime)Tasks2[0][DatabaseObjects.Columns.DueDate];
                    if (endDate.Date.CompareTo(startDate.Date) > 0)
                    {
                        TimeSpan tSpan = endDate.Subtract(startDate);
                        actualDays = tSpan.Days;
                    }
                    else if (endDate.Date.CompareTo(startDate.Date) == 0)
                    {
                        actualDays = 1;
                    }
                }
            }
        }

        #endregion
    }
}