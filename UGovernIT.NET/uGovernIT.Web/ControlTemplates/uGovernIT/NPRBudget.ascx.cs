using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Web;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class NPRBudget : UserControl
    {
        protected int currentYear = DateTime.Today.Year;
        public int nprId { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        private bool isBindBudgetDone;
        public bool ReadOnly;
        public string FrameId;
        public string NPRResourceAddEditUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernit/delegatecontrol.aspx?control=NPRResourceAddEdit");
        public string NPRBudgetAddEditUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernit/delegatecontrol.aspx?control=NPRBudgetAddEdit");
        UserProfile User;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(HttpContext.Current.GetManagerContext());
        ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(HttpContext.Current.GetManagerContext());
        protected override void OnPreRender(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            if (!isBindBudgetDone)
            {
                if (!ReadOnly)
                {
                    BindNPRBudgetList();
                    //CloseInsertPanel();
                }
                else
                {
                    BindReadOnlyNPRBudgetList();
                    BindReadOnlyNPRResourceList();
                }
            }

            if (ReadOnly)
            {
                //Get total budget plan budget amount
                if (readOnlyNPRBugetList.DataSource != null)
                {
                    double bugdetTotal = 0;
                    DataTable budgetTable = (DataTable)readOnlyNPRBugetList.DataSource;
                    foreach (DataRow dr in budgetTable.Rows)
                    {
                        bugdetTotal = bugdetTotal + Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                    }
                    Literal lblTotalBudget = (Literal)readOnlyNPRBugetList.FindControl("lblTotalBudget");
                    if (lblTotalBudget != null)
                    {
                        lblTotalBudget.Text = string.Format("{0:C}", bugdetTotal);
                    }
                }
            }
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check the project stage and overwrite the readonly variable here else leave it.
            // SPListItem nprTicket = Ticket.getCurrentTicket("NPR", nprId.ToString());
            //DataRow nprTicket = Ticket.getCurrentTicket("NPR", nprId.ToString());
            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR", TicketID);
            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager objmgr = new ModuleViewManager(context);
            UserProfileManager UserManager = new UserProfileManager(context);
            if (nprTicket == null)
            {
                nprTicket = ticketManager.GetByID(objmgr.LoadByName("NPR"), nprId);
                TicketID = Convert.ToString(nprTicket["TicketId"]);
            }
            if (!(UserManager.IsActionUser(nprTicket, User) || UserManager.IsDataEditor(nprTicket, User)) || uHelper.IsProjectApproved(context, nprTicket))
                ReadOnly = true;
            if (ReadOnly)
            {
                viewMode.Visible = true;
                editMode.Visible = false;
            }
            else
            {
                viewMode.Visible = false;
                editMode.Visible = true;
            }

            HttpCookie budgetYear = Request.Cookies.Get("budgetyear");
            if (budgetYear != null && budgetYear.Value.Trim() != string.Empty)
            {
                if (!int.TryParse(budgetYear.Value.Trim(), out currentYear) || currentYear == 0)
                    currentYear = DateTime.Today.Year;
            }

            if (!IsPostBack)
                BindNPRResourceList();
        }

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
        }

        protected void FillDropDownLevel1(object sender, EventArgs e)
        {

            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                DataTable resultedTable = budgetCategoryViewManager.LoadCategories();
                List<ListItem> items = budgetCategoryViewManager.LoadCategoriesDropDownItems(resultedTable);
                foreach (ListItem item in items)
                {
                    level1.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Fill level1 on the basis of level1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BindNPRBudgetList()
        {
            DataTable budgets = new DataTable();
            budgets = GetNPRBudgetList();

            aspxNPRBudgetgrid.DataSource = budgets;
            aspxNPRBudgetgrid.DataBind();
            BindProjectPlan();
            isBindBudgetDone = true;
        }

        private void BindReadOnlyNPRBudgetList()
        {
            DataTable budgets = new DataTable();
            budgets = GetNPRBudgetList();

            if (budgets.Rows.Count <= 0 || budgets == null)
                fldSetReadOnlyBudgetItems.Visible = false;

            readOnlyNPRBugetList.DataSource = budgets;
            readOnlyNPRBugetList.DataBind();
            if (readOnlyNPRBugetList.DataSource != null)
            {
                double bugdetTotal = 0;
                DataTable budgetTable = (DataTable)readOnlyNPRBugetList.DataSource;
                foreach (DataRow dr in budgetTable.Rows)
                {
                    bugdetTotal = bugdetTotal + Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                }
                Literal lblTotalBudget = (Literal)readOnlyNPRBugetList.FindControl("lblTotalBudget");
                if (lblTotalBudget != null)
                {
                    lblTotalBudget.Text = string.Format("{0:C}", bugdetTotal);
                }
            }
            BindReadOnlyProjectPlan();

            isBindBudgetDone = true;

        }

        private DataTable GetNPRBudgetList()
        {
            return moduleBudgetManager.LoadBudgetByTicketID(TicketID);
        }
        private void BindReadOnlyNPRResourceList()
        {
            DataTable dt = new DataTable();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            dt = GetTableDataManager.GetData(DatabaseObjects.Tables.NPRResources, values);
            string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, TicketID);
            DataRow[] myModuleUserStatisticsRows = dt.Select(selectQuery);
            if (myModuleUserStatisticsRows != null && myModuleUserStatisticsRows.Length > 0)
            {
                aspxReadOnlyNPRResource.DataSource = myModuleUserStatisticsRows.CopyToDataTable();
                aspxReadOnlyNPRResource.DataBind();
            }
        }

        #region Time Sheet

        private void BindProjectPlan()
        {
            projectPlanSheet.DataSource = GetProjectPlans(currentYear);
            projectPlanSheet.DataBind();
        }

        private void BindReadOnlyProjectPlan()
        {
            readOnlyPlanSheet.DataSource = GetProjectPlans(currentYear);
            readOnlyPlanSheet.DataBind();
        }

        private DataTable GetProjectPlans(int currentYearVal)
        {
            string rQueryWithCategoryFilter = string.Empty;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            DateTime startDate1 = new DateTime(currentYearVal, 1, 1);
            DateTime endDate1 = new DateTime(currentYearVal, 12, 31);
            DataTable projectPlan = uHelper.GetMonthlyBudget(context, startDate1, endDate1, 0, BudgetType.NPR, nprId, uGovernIT.Utility.ColumnStyle.MonthNumber);
            projectPlan.Rows[0].Delete();
            projectPlan.Rows[0][DatabaseObjects.Columns.Title] = "Budget";
            projectPlan.Rows[0]["Category"] = "0";

            if (configurationVariableManager.GetValue(Constants.StaffingBudget) == "true")
            {
                for (int ctype = 0; ctype < 3; ctype++)
                {
                    string category = string.Empty;
                    DataRow row = projectPlan.NewRow();
                    row["Category"] = ctype;

                    switch (ctype)
                    {
                        case 1:
                            row[DatabaseObjects.Columns.Title] = category = "On-Site Consultants";
                            break;
                        case 2:
                            row[DatabaseObjects.Columns.Title] = category = "Off-Site Consultants";
                            break;
                        default:
                            row[DatabaseObjects.Columns.Title] = category = "Staff";

                            break;
                    }

                    // Get count and put in month table// 
                     
                    rQueryWithCategoryFilter = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.TicketId, TicketID, DatabaseObjects.Columns.BudgetType, category);
                    DataTable monthlyBudget = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {rQueryWithCategoryFilter}");
                    float totalCount = 0;
                    //float workingdays = 0;
                    foreach (DataRow monthBudget in monthlyBudget.Rows)
                    {
                        DateTime startDate = (DateTime)monthBudget[DatabaseObjects.Columns.AllocationStartDate];
                        if (startDate.Year == currentYearVal)
                        {
                            float myCount = float.Parse(monthBudget[DatabaseObjects.Columns.BudgetAmount].ToString());
                            row["Month" + startDate.Month] = Math.Round(myCount, 2);
                            totalCount += myCount;
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        float existingCount = 0;
                        if (row["Month" + i] != null)
                        {
                            float.TryParse(row["Month" + i].ToString(), out existingCount);
                        }
                        if (existingCount <= 0)
                        {
                            row["Month" + i] = "0";
                        }
                    }

                    if (totalCount > 0)
                    {
                        row["Total"] = String.Format("{0} WMs", Math.Round(totalCount, 2));
                    }
                    else
                    {
                        row["Total"] = totalCount;
                    }
                    projectPlan.Rows.Add(row);
                }
            }

            return projectPlan;
        }

        #endregion


        #region NPRResource
        private void BindNPRResourceList()
        {
            DataTable dtNprResource = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (dtNprResource != null && dtNprResource.Rows.Count > 0)
            {
                DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID));
                if (nprResourcesColl != null && nprResourcesColl.Length > 0)
                {
                    dtNprResource = nprResourcesColl.CopyToDataTable();
                }
                aspxNPRResourceList.DataSource = dtNprResource;
                aspxNPRResourceList.DataBind();
            }
        }
        protected void aspxNPRResourceList_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = aspxNPRResourceList.GetDataRow(e.VisibleIndex);
            if (currentRow == null) return;

            string func = string.Empty;
            string resourceID = string.Empty;

            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            {
                resourceID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            }
            if (!ReadOnly)
            {
                aspxNPRResourceList.Columns[8].Visible = true;
                func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(NPRResourceAddEditUrl + "&ID=" + resourceID + "&NPRTicketID=" + TicketID), "", "Edit Resource ", "800px", "50");
                HtmlImage imgEdit = (HtmlImage)aspxNPRResourceList.FindRowCellTemplateControl(e.VisibleIndex, null, "imgEdit");
                imgEdit.Attributes.Add("onclick", func);

            }
        }

        public static void UpdateNPRMonthlyDistributionResource(DataRow resourceItem, DataRow nprResourceItem)
        {
            string nprTicketId = string.Empty; // here we need to use Ticketid instead of module table ID becasue we are ticketID in monthlybudget table.
            string type = string.Empty;
            string trnsType = string.Empty;
            if (resourceItem != null)
            {
                DataRow listItem = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), "NPR", Convert.ToString(resourceItem[DatabaseObjects.Columns.TicketId]));
                nprTicketId = Convert.ToString(listItem[DatabaseObjects.Columns.ID]);
                type = Convert.ToString(resourceItem[DatabaseObjects.Columns.BudgetType]);
            }
            else if (nprResourceItem != null)
            {
                nprTicketId = Convert.ToString(nprResourceItem[DatabaseObjects.Columns.TicketId]);
                type = Convert.ToString(nprResourceItem[DatabaseObjects.Columns.BudgetType]);
            }
            RemoveNPRMonthlyDistributionResource(nprTicketId, type, HttpContext.Current.GetManagerContext());
            AddNPRMonthlyDistributionResource(nprTicketId, type, HttpContext.Current.GetManagerContext());
        }

        private static void RemoveNPRMonthlyDistributionResource(string nprTicketId, string type, ApplicationContext context)
        {
            DataTable nprResourceList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); 
            DataTable nprMonthlyBudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}= '{ModuleNames.NPR}'");
            if (!string.IsNullOrEmpty(nprTicketId))
            {
                DataRow[] nprBudgetCollection = nprMonthlyBudgetList.Select(string.Format("{0}={1} And {2}={3}", DatabaseObjects.Columns.TicketId, nprTicketId, DatabaseObjects.Columns.BudgetType, type));
                if (nprBudgetCollection != null)
                {
                    for (int i = nprBudgetCollection.Length - 1; i >= 0; i--)
                    {
                        nprBudgetCollection[i].Delete();
                    }
                }
            }
        }

        private static void AddNPRMonthlyDistributionResource(string nprTicketId, string type, ApplicationContext context)
        {
            DataTable nprResourceList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'"); 
            DataTable nprMonthlyBudgetList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudget, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}= '{ModuleNames.NPR}'");
            ModuleMonthlyBudgetManager objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);
            if (!string.IsNullOrEmpty(nprTicketId))
            {
                DataRow[] nprResourceItemCollection = nprResourceList.Select(string.Format("{0}={1} And {2}={3}", DatabaseObjects.Columns.TicketId, nprTicketId, DatabaseObjects.Columns.BudgetType, type));
                if (nprResourceItemCollection.Length > 0)
                {
                    foreach (DataRow ritem in nprResourceItemCollection)
                    {
                        DataRow[] nprBudgetCollection = nprMonthlyBudgetList.Select(string.Format("{0}={1} And {2}={3}", DatabaseObjects.Columns.TicketId, nprTicketId, DatabaseObjects.Columns.BudgetType, type));
                        DataTable budgetTable = null;
                        if (nprBudgetCollection.Length > 0)
                        {
                            budgetTable = nprBudgetCollection.CopyToDataTable();
                        }

                        DateTime startDate = (DateTime)ritem[DatabaseObjects.Columns.AllocationStartDate];
                        DateTime endDate = (DateTime)ritem[DatabaseObjects.Columns.AllocationEndDate];
                        double totalFTEs = Convert.ToDouble(ritem[DatabaseObjects.Columns.TicketNoOfFTEs]);

                        // Distribute the amount within specified dates and get the result in month and amount format.
                        Dictionary<DateTime, double> distributions = uHelper.MonthlyDistributeFTEs(HttpContext.Current.GetManagerContext(), startDate, endDate, totalFTEs);
                        ModuleMonthlyBudget item = new ModuleMonthlyBudget();
                        // Add new Distribution
                        foreach (DateTime key in distributions.Keys)
                        {
                            double val = distributions[key];
                            double oldValue = 0;
                            DataRow oldItem = null;
                            if (budgetTable != null && budgetTable.Rows.Count > 0)
                                oldItem = budgetTable.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Date == key.Date);

                            if (oldItem != null)
                            {
                                DataRow oldBudgetItem = nprMonthlyBudgetList.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, (int)oldItem[DatabaseObjects.Columns.ID]))[0];
                                item = objModuleMonthlyBudgetManager.LoadByID(Convert.ToInt32(oldBudgetItem[DatabaseObjects.Columns.ID]));
                                if (item != null)
                                {
                                    double.TryParse(Convert.ToString(oldBudgetItem[DatabaseObjects.Columns.BudgetAmount]), out oldValue);
                                    item.BudgetAmount = oldValue + val;
                                    item.ModuleName = ModuleNames.NPR;
                                    item.TicketId = nprTicketId;
                                    objModuleMonthlyBudgetManager.InsertORUpdateData(item);
                                }
                            }
                            else
                            {
                                item.AllocationStartDate = new DateTime(key.Year, key.Month, 1);
                                item.BudgetAmount = val;
                                item.TicketId = nprTicketId;
                                item.BudgetType = type;
                                item.ModuleName = ModuleNames.NPR;
                                objModuleMonthlyBudgetManager.InsertORUpdateData(item);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        protected void aspxNPRResourceList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            BindProjectPlan();
        }

        protected void aspxNPRResourceList_DataBinding(object sender, EventArgs e)
        {
            aspxNPRResourceList.ForceDataRowType(typeof(DataRow));
        }
        protected void aspxNPRResourceList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.RequestedResources)
            {
                if (e.GetValue(DatabaseObjects.Columns.RequestedResources) == null) return;

                e.Cell.Text = UGITUtility.ObjectToString(e.GetValue(DatabaseObjects.Columns.RequestedResources));
                //string.Join(", ", uHelper.GetMultiLookupValue(e.GetValue(DatabaseObjects.Columns.RequestedResources).ToString()));
            }
        }

        protected void aspxNPRBudgetgrid_DataBinding(object sender, EventArgs e)
        {

        }

        protected void aspxNPRBudgetgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string func = string.Empty;
            string resourceID = string.Empty;

            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = aspxNPRBudgetgrid.GetDataRow(e.VisibleIndex);
            if (currentRow == null) return;

            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
                resourceID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            
            func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(NPRBudgetAddEditUrl + "&ItemID=" + resourceID + "&NPRID=" + nprId), "", "Edit Budget ", "600px", "50");
            HtmlImage imgEdit = (HtmlImage)aspxNPRBudgetgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgEdit");
            imgEdit.Attributes.Add("onclick", func);
        }

        protected void imgDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton deletebutton = (ImageButton)sender;
            int Id = Convert.ToInt32(deletebutton.CommandArgument);
            ModuleBudget objModuleBudget = new ModuleBudget();
            objModuleBudget.ID = Id;
            bool Deleted = moduleBudgetManager.Delete(objModuleBudget);
            if (Deleted)
            {
                budgetMessage.Text = "Item Deleted Successfully.";
                budgetMessage.ForeColor = System.Drawing.Color.Blue;
            }
        }

        protected void imgDelete_Click1(object sender, ImageClickEventArgs e)
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(context);

            ImageButton deletebutton = (ImageButton)sender;
            int resourceId = Convert.ToInt32(deletebutton.CommandArgument);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            DataTable dtNprResource = GetTableDataManager.GetData(DatabaseObjects.Tables.NPRResources, values);
            if (dtNprResource != null && dtNprResource.Rows.Count > 0)
            {
                DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, resourceId));
                if (nprResourcesColl != null)
                {
                    dtNprResource = nprResourcesColl.CopyToDataTable();
                }
            }
            DataRow drnprresourceItem = dtNprResource.Rows[0];
            DataRow nprTicket = Ticket.GetCurrentTicket(context, ModuleNames.NPR, nprId.ToString());
            List<UGITTask> ptasks = objUGITTaskManager.LoadByProjectID(ModuleNames.NPR, Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));

            // Only delete if task count is zero
            if (ptasks != null && ptasks.Count == 0)
            {
                UGITTask tsk = new UGITTask();
                tsk.ID = Convert.ToInt16(dtNprResource.Rows[0][DatabaseObjects.Columns.Id]);
                tsk.Title = Convert.ToString(dtNprResource.Rows[0][DatabaseObjects.Columns.UserSkillLookup]);
                tsk.StartDate = Convert.ToDateTime(dtNprResource.Rows[0][DatabaseObjects.Columns.AllocationStartDate]);
                tsk.DueDate = Convert.ToDateTime(dtNprResource.Rows[0][DatabaseObjects.Columns.AllocationEndDate]);
                tsk.EstimatedHours = Convert.ToInt16(dtNprResource.Rows[0][DatabaseObjects.Columns.EstimatedHours]);
                tsk.PercentComplete = Convert.ToDouble(dtNprResource.Rows[0][DatabaseObjects.Columns.TicketNoOfFTEs]);

                List<string> existingUsers = new List<string>();
                if (tsk.AssignedTo != null)
                    existingUsers = tsk.AssignedTo.Split(new Char[] { ',','#' }).ToList();
                uGovernIT.Helpers.RMMSummaryHelper.DeleteAllocationsByTask(context, ptasks, existingUsers, ModuleNames.NPR, Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));
            }
            dtNprResource.Rows[0].Delete();
            UpdateProjectStartEndDate();
            UpdateNPRMonthlyDistributionResource(drnprresourceItem, null);
            BindNPRResourceList();
        }

        public void UpdateProjectStartEndDate()
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(context);
            ModuleViewManager objmodule = new ModuleViewManager(context);
            TicketManager objTicketManager = new TicketManager(context);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR", Convert.ToString(nprId));
            List<UGITTask> nprTasks = objUGITTaskManager.LoadByProjectID("NPR", Convert.ToString(UGITUtility.GetSPItemValue(nprTicket, DatabaseObjects.Columns.TicketId)));
            if (nprTasks.Count == 0)
            {
                DataTable dtNprResource = GetTableDataManager.GetData(DatabaseObjects.Tables.NPRResources, values);
                if (dtNprResource != null && dtNprResource.Rows.Count > 0)
                {
                    DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}={1}", DatabaseObjects.Columns.TicketId, nprId));
                    if (nprResourcesColl != null)
                    {
                        dtNprResource = nprResourcesColl.CopyToDataTable();
                        DateTime minDate = dtNprResource.AsEnumerable().Min(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate));
                        DateTime maxDate = dtNprResource.AsEnumerable().Max(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate));
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = minDate;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = maxDate;
                        int result = objTicketManager.Save(objmodule.LoadByName(ModuleNames.NPR), nprTicket);
                        if (result <= 0)
                            ULog.WriteException("NPR Budget Fail: actualstartdate and completetiondate not saved!");
                    }
                    else
                    {
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = null;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = null;
                        int result = objTicketManager.Save(objmodule.LoadByName(ModuleNames.NPR), nprTicket);
                        if (result <= 0)
                            ULog.WriteException("NPR Budget Fail: actualstartdate and completetiondate not saved!");
                    }
                }
                
            }
        }
    }
}
