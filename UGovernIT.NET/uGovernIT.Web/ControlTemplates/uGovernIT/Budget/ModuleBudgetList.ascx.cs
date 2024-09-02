using DevExpress.Spreadsheet;
using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Web
{
    public partial class ModuleBudgetList : UserControl
    {
        public string ModuleResourceAddEditUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernit/delegatecontrol.aspx?control=ModuleResourceAddEdit");
        public string BudgetAddEditUrl = "/Layouts/uGovernit/delegatecontrol.aspx?control=ModuleBudgetAddEdit";
        public string BudgetActualEditUrl = "/Layouts/uGovernit/delegatecontrol.aspx?control=BudgetActualAddEdit";
        public string CategoryName { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        public string FrameId;

        public bool ReadOnly;
        public static bool Checklabel;

        private int baselineId;

        private bool isBindBudgetDone;
        //private bool isBindBudgetActualDone;
        private bool isBaseline;

        private FieldConfigurationManager ObjFieldConfigManager = null;
        private ModuleMonthlyBudgetManager _moduleMonthlyBudgetManager = null;
        private ModuleMonthlyBudgetHistoryManager _moduleMonthlyBudgetHistoryManager = null;
        private string absoluteUrlImport = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&listName={1}";

        protected bool pmmBudgetNeedsApproval = false;
        protected bool pmmBudgetAllowEdit = false;
        private string pmmBudgetApprover = string.Empty;
        private bool isCurrentUserBudgetApprover = false;
        protected string importUrl;
        protected int currentYear = DateTime.Today.Year;
        
        UserProfile User;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        DataTable budgets = new DataTable();
        public DataTable LabourChargesData { get; set; }

        private class GlobalTicket
        {
            public static string ticketid;
        }

        private class GlobalModule
        {
            public static string modulename;
        }

        DataTable actuals = new DataTable();
        BudgetCategoryViewManager objbudgetCategoryViewManager;
        ModuleBudgetManager objModuleBudgetManager;
        NPRResourcesManager objNPRResourcesManager;
        BudgetActualsManager objBudgetActualsManager;
        UGITTaskManager objUGITTaskManager;
        ConfigurationVariableManager ObjConfigManager;

        protected ModuleMonthlyBudgetManager ModuleMonthlyBudgetManager
        {
            get
            {
                if (_moduleMonthlyBudgetManager == null)
                {
                    _moduleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);
                }
                return _moduleMonthlyBudgetManager;
            }
        }

        protected ModuleMonthlyBudgetHistoryManager ModuleMonthlyBudgetHistoryManager
        {
            get
            {
                if (_moduleMonthlyBudgetHistoryManager == null)
                {
                    _moduleMonthlyBudgetHistoryManager = new ModuleMonthlyBudgetHistoryManager(context);
                }
                return _moduleMonthlyBudgetHistoryManager;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            objUGITTaskManager = new UGITTaskManager(context);
           // objModuleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);
            objBudgetActualsManager = new BudgetActualsManager(context);
            objNPRResourcesManager = new NPRResourcesManager(context);
            objbudgetCategoryViewManager = new BudgetCategoryViewManager(context);
            objModuleBudgetManager = new ModuleBudgetManager(context);
            ObjConfigManager = new ConfigurationVariableManager(context);
            ObjFieldConfigManager = new FieldConfigurationManager(context);

            User = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (null != Request["showBaseline"] && null != Request["baselineNum"])
            {
                isBaseline = UGITUtility.StringToBoolean(Request["showBaseline"].Trim());

                baselineId = Convert.ToInt32(Request["baselineNum"]);
            }

            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", DatabaseObjects.Tables.ModuleMonthlyBudget));

            // Get config setting for PMM budget for approval/rejection and the mode so that user can edit the approved budget or not.
            pmmBudgetNeedsApproval = ObjConfigManager.GetValueAsBool(ConfigConstants.PMMBudgetNeedsApproval);
            pmmBudgetAllowEdit = ObjConfigManager.GetValueAsBool(ConfigConstants.PMMBudgetAllowEdit);
            pmmBudgetApprover = ObjConfigManager.GetValue(ConfigConstants.PMMBudgetApprover);

            // Check the current logon user is in Budgets Approver group.
            if (pmmBudgetNeedsApproval)
            {
                isCurrentUserBudgetApprover = UserManager.CheckUserIsInGroup(pmmBudgetApprover, context.CurrentUser);
                newBudget.Text = "Request Additional Budget";
            }
            else
                newBudget.Text = "Add Budget Item";

            if (!string.IsNullOrEmpty(ModuleName))
            {
                GlobalTicket.ticketid = TicketID;
                TicketID = TicketID.ToUpper();
                GlobalModule.modulename = ModuleName;
                ModuleName = ModuleName.ToUpper();
                if (ModuleName == ModuleNames.PMM)
                {
                    lblBudgetSummary.Text = "Budget Summary";
                    fldsetNPRResource.Visible = false;
                    aspxBudgetgrid.CategoryName = "PMMBudget";
                    aspxBudgetActualsgrid.CategoryName = "PMMActuals";
                    aspxBudgetgrid.LoadColumns();
                    aspxBudgetActualsgrid.LoadColumns();
                }
                else if (ModuleName == ModuleNames.NPR)
                {
                    lblBudgetSummary.Text = "Monthly Budget and Resources";
                    PMMBudgetActuals.Visible = false;
                    aspxBudgetgrid.CategoryName = "NPRBudget";
                    aspxModuleResourceList.CategoryName = "NPRResource";
                    aspxBudgetgrid.LoadColumns();
                    aspxModuleResourceList.LoadColumns();
                    BindNPRResourceList();
                }
                BindModuleBudgetData();
            }
            BindBudgetActual(baselineId, isBaseline);
            BindResourceCostViewControl();
        }

        public void SetLabelValue()
        {
            budgetMessage.Text = "Item Deleted Successfully.";
            budgetMessage.ForeColor = System.Drawing.Color.Blue;
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!int.TryParse(currentYearHidden.Value, out currentYear))
                currentYear = DateTime.Now.Year;

            User = HttpContext.Current.CurrentUser();  //HttpContext.Current.User.Identity.Name.ToString();
            if (!isBindBudgetDone)
            {
                if (!ReadOnly)
                {
                    BindModuleBudgetData();
                }
                else
                {
                    BindModuleBudgetData();
                    BindNPRResourceList();
                }
            }

            if (ReadOnly)
            {
                //Get total budget plan budget amount
                if (aspxBudgetgrid.DataSource != null)
                {
                    double bugdetTotal = 0;
                    DataTable budgetTable = (DataTable)aspxBudgetgrid.DataSource;
                    foreach (DataRow dr in budgetTable.Rows)
                    {
                        bugdetTotal = bugdetTotal + Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                    }
                    //Literal lblTotalBudget = (Literal)aspxNPRBudgetgrid.FindControl("lblTotalBudget");
                    //if (lblTotalBudget != null)
                    //{
                    //    lblTotalBudget.Text = string.Format("{0:C}", bugdetTotal);
                    //}
                }
            }
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(currentYearHidden.Value, out currentYear))
                currentYear = DateTime.Now.Year;

            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR", GlobalTicket.ticketid);
            if (!(UserManager.IsActionUser(nprTicket, User) || UserManager.IsDataEditor(nprTicket, User)) || uHelper.IsProjectApproved(context, nprTicket))
                ReadOnly = true;
            if (ReadOnly)
            {
                viewMode.Visible = true;
                editMode.Visible = true;
            }
            else
            {
                viewMode.Visible = false;
                editMode.Visible = true;
            }
            if (ModuleName == ModuleNames.NPR)
            {
                aspxBudgetgrid.SettingsContextMenu.Enabled = false;
                HttpCookie budgetYear = Request.Cookies.Get("budgetyear");
                if (budgetYear != null && budgetYear.Value.Trim() != string.Empty)
                {
                    if (!int.TryParse(budgetYear.Value.Trim(), out currentYear) || currentYear == 0)
                        currentYear = DateTime.Today.Year;
                }
            }

            if (!IsPostBack)
                BindNPRResourceList();
        }

        protected void FillDropDownLevel1(object sender, EventArgs e)
        {
            DropDownList level1 = (DropDownList)sender;
            if (level1.Items.Count <= 0)
            {
                DataTable resultedTable = objbudgetCategoryViewManager.LoadCategories();
                List<ListItem> items = objbudgetCategoryViewManager.LoadCategoriesDropDownItems(resultedTable);
                foreach (ListItem item in items)
                {
                    level1.Items.Add(item);
                }
            }
        }

        private void BindModuleBudgetData()
        {
            //add query string para in variables
            var moduleBudgetData = GetModuleBudgetData(isBaseline, baselineId);

            if (pmmBudgetNeedsApproval)
            {
                //Binding all module budget data in same grid with non approved in red colors, unlike sharepoint which uses diff grid for unapproved budget
                DataRow[] approvedBudgets = moduleBudgetData.Select(); 
                if (approvedBudgets.Length > 0)
                {
                    aspxBudgetgrid.DataSource = approvedBudgets.CopyToDataTable();
                    aspxBudgetgrid.DataBind();
                }
                else
                {
                    aspxBudgetgrid.DataSource = moduleBudgetData.Clone();
                    aspxBudgetgrid.DataBind();
                }
                
            }
            //Show Baseline for budget tab
            else if (Request["showBaseline"] != null && !pmmBudgetNeedsApproval)
            {

                aspxBudgetgrid.DataSource = moduleBudgetData;

                aspxBudgetgrid.DataBind();

                newBudget.Visible = false;

                // aspxBudgetgrid.Columns["func"].Visible = false;


            }

            else
            {
                // approvedHeader.Visible = false;
                // fSetPendingApprove.Visible = false;
                aspxBudgetgrid.DataSource = moduleBudgetData;
                aspxBudgetgrid.DataBind();
                
                DataRow project = Ticket.GetCurrentTicket(context, ModuleName, TicketID);
                if (project != null)
                {
                    project[DatabaseObjects.Columns.TicketTotalCost] = UGITUtility.StringToDouble(moduleBudgetData.Compute($"Sum({DatabaseObjects.Columns.BudgetAmount})", string.Empty));
                    Ticket ticketRequest = new Ticket(context, ModuleName);
                    ticketRequest.CommitChanges(project);
                }
            }

            isBindBudgetDone = true;
            BindProjectPlan();
        }

        private void BindBudgetActual(int baselineId = 0, bool isBaseline = false)
        {
            actuals = objModuleBudgetManager.GetPMMBudgetActualList(TicketID, baselineId, isBaseline);

            //isBindBudgetActualDone = true;

            aspxBudgetActualsgrid.DataSource = actuals;

            aspxBudgetActualsgrid.DataBind();

            if (isBaseline)
            {
                newActual.Visible = false;

                aspxBudgetActualsgrid.SettingsContextMenu.Enabled = false;

                //aspxBudgetActualsgrid.Columns["budgetActual"].Visible = false;


            }
        }

        private DataTable GetModuleBudgetData(bool isBaseline, int baselineId)
        {
            return objModuleBudgetManager.LoadBudgetByTicketID(TicketID, isBaseline, baselineId);
        }

        #region Time Sheet

        private void BindProjectPlan()
        {
            int.TryParse(currentYearHidden.Value, out currentYear);
            if (currentYear <= 0)
            {
                currentYear = DateTime.Now.Year;
            }
            ASPxPivotGrid1.DataSource = GetBudgetPlanPivotGridData();
            ASPxPivotGrid1.DataBind();

            lviewBudget.DataSource = GetBudgetPlan(isBaseline,baselineId);

            lviewBudget.DataBind();

            currentYearHidden.Value = currentYear.ToString();
        }
        #endregion

        #region NPRResource
        private void BindNPRResourceList()
        {
            DataTable dtNprResource = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            if (dtNprResource != null && dtNprResource.Rows.Count > 0)
            {

                DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, GlobalTicket.ticketid));
                if (nprResourcesColl != null && nprResourcesColl.Length > 0)
                {
                    dtNprResource = nprResourcesColl.CopyToDataTable();

                }
                else
                {
                    dtNprResource = new DataTable();
                }

            }
            aspxModuleResourceList.DataSource = dtNprResource;
            aspxModuleResourceList.DataBind();
            BindProjectPlan();
        }

        protected void aspxModuleResourceList_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = aspxModuleResourceList.GetDataRow(e.VisibleIndex);
            if (currentRow == null) return;

            string func = string.Empty;
            string resourceID = string.Empty;

            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            {
                resourceID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            }
            aspxModuleResourceList.Columns[8].Visible = true;
            string url = ModuleResourceAddEditUrl + "&ID=" + resourceID + "&TicketID=" + TicketID + "&ModuleName=" + ModuleName;
            func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", url, "", "Edit Resource ", "800px", "50");
            HtmlImage imgEdit = (HtmlImage)aspxModuleResourceList.FindRowCellTemplateControl(e.VisibleIndex, null, "imgEdit");
            imgEdit.Attributes.Add("onclick", func);
        }

        public void UpdateNPRMonthlyDistributionResource(DataRow resourceItem, DataRow nprResourceItem)
        {
            int nprID = 0;
            string type = string.Empty;
            string trnsType = string.Empty;
            if (resourceItem != null)
            {
                DataRow listItem = Ticket.GetCurrentTicket(context, "NPR", Convert.ToString(resourceItem[DatabaseObjects.Columns.TicketId]));
                nprID = Convert.ToInt32(listItem[DatabaseObjects.Columns.ID]);
                type = Convert.ToString(resourceItem[DatabaseObjects.Columns.BudgetTypeChoice]);
            }
            else if (nprResourceItem != null)
            {
                //SPFieldLookupValue nprTicketIdlookup = new SPFieldLookupValue(Convert.ToString(nprResourceItem[DatabaseObjects.Columns.TicketNPRIdLookup]));
                //nprID = nprTicketIdlookup.LookupId;
                type = Convert.ToString(nprResourceItem[DatabaseObjects.Columns.BudgetType]);
            }
            objNPRResourcesManager.RemoveNPRMonthlyDistributionResource(GlobalTicket.ticketid, type);
            objNPRResourcesManager.AddNPRMonthlyDistributionResource(GlobalTicket.ticketid, type, GlobalModule.modulename);
        }

        #endregion

        protected void aspxNPRResourceList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            BindProjectPlan();
        }

        protected void aspxModuleResourceList_DataBinding(object sender, EventArgs e)
        {
            aspxModuleResourceList.ForceDataRowType(typeof(DataRow));
        }

        protected void aspxModuleResourceList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.RequestedResources)
            {
                if (e.GetValue(DatabaseObjects.Columns.RequestedResources) == null) return;

                //e.Cell.Text = string.Join(", ", uHelper.GetMultiLookupValue(e.GetValue(DatabaseObjects.Columns.RequestedResources).ToString()));
            }
        }

        protected void aspxBudgetgrid_DataBinding(object sender, EventArgs e)
        {

        }

        protected void aspxBudgetgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = aspxBudgetgrid.GetDataRow(e.VisibleIndex);
            if (currentRow == null) return;

            string func = string.Empty;
            string resourceID = string.Empty;
            string categoryName = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            {
                resourceID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
                categoryName = Convert.ToString(currentRow[DatabaseObjects.Columns.BudgetCategory]).Trim();
            }
            func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(BudgetAddEditUrl + "&ID=" + resourceID + "&TicketID=" + TicketID + "&ModuleName=" + ModuleName + "&IsTabActive=true"+ "&folderName="), "", "Edit Budget -" + categoryName + "", "800px", "400px");
            HtmlImage imgEdit = (HtmlImage)aspxBudgetgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgEdit");
            imgEdit.Attributes.Add("onclick", func);

            HtmlImage imgApprove = (HtmlImage)aspxBudgetgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgApprove");
            HtmlImage imgReject = (HtmlImage)aspxBudgetgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgReject");
            ImageButton imgDelete = (ImageButton)aspxBudgetgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "BtDeleteBudget");
            string BudgetApprove = string.Format("ApproveBudget('{0}')", currentRow[DatabaseObjects.Columns.Id]);
            imgApprove.Attributes.Add("onclick", BudgetApprove);
            string BudgetReject = string.Format("RejectBudget('{0}')", currentRow[DatabaseObjects.Columns.Id]);
            imgReject.Attributes.Add("onclick", BudgetReject);

            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.BudgetStatus))
            {
                if (UGITUtility.ObjectToString(e.GetValue(DatabaseObjects.Columns.BudgetStatus)) != "0")
                {
                    imgApprove.Visible = false;
                    imgReject.Visible = false;
                }
                else
                {
                    // Make the approve && reject button vidible if the log on user is belong to BudgetApprover group.
                    if (isCurrentUserBudgetApprover)
                    {
                        imgApprove.Visible = true;
                        imgReject.Visible = true;
                    }
                    else
                    {
                        imgApprove.Visible = false;
                        imgReject.Visible = false;
                    }
                }
            }
            if (ModuleName == ModuleNames.NPR)
            {
                imgApprove.Visible = false;
                imgReject.Visible = false;
            }
            if (isBaseline)
            {
                imgApprove.Visible = false;
                imgReject.Visible = false;
                imgEdit.Visible = false;
                imgDelete.Visible = false;
            }
        }

        protected void imgDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton deletebutton = (ImageButton)sender;
            int Id = Convert.ToInt32(deletebutton.CommandArgument);
            if (Id == 0)
            {
                Id = Convert.ToInt32(HiddenFieldBudgetActual.Value);
            }
            DataTable dtActuals = objBudgetActualsManager.LoadModuleBudgetActuals();
            DataRow drActuals = dtActuals.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Id))[0];
            if (drActuals != null)
            {
                // Get Sub- category id from actual item.
                int subCategoryId = GetBudgetItemcategoryId(drActuals);
                BudgetActual objModuleBudgetActual = objBudgetActualsManager.LoadByID(Convert.ToInt32(drActuals[DatabaseObjects.Columns.ID]));
                bool Deleted = objBudgetActualsManager.Delete(objModuleBudgetActual);
                // Update the pmm monthly budget distribution list.
                objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketID, ModuleName);
                //Update monthly distribution after deleting budget item
                if (subCategoryId != -1)
                    objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(subCategoryId);
                if (Deleted)
                {
                    budgetMessage.Text = "Item Deleted Successfully.";
                    budgetMessage.ForeColor = System.Drawing.Color.Blue;
                }

            }
            // objModuleBudget.ID = Id;

            BindBudgetActual();
            BindModuleBudgetData();

        }

        private int GetBudgetItemcategoryId(DataRow budgetActual)
        {
            // Update Monthly distribution
            DataTable pmmBudgetList = objModuleBudgetManager.LoadModuleBudget();

            int subCategoryId = -1;
            DataRow[] drbudgetItemCollection = pmmBudgetList.Select($"{DatabaseObjects.Columns.ID}='{UGITUtility.StringToInt(budgetActual[DatabaseObjects.Columns.ModuleBudgetLookup])}' ");
            if (drbudgetItemCollection.Count() > 0)
            {
                subCategoryId = Convert.ToInt32((drbudgetItemCollection[0][DatabaseObjects.Columns.BudgetCategoryLookup]));
            }

            return subCategoryId;
        }

        protected void imgDelete_Click1(object sender, ImageClickEventArgs e)
        {
            ImageButton deletebutton = (ImageButton)sender;
            int resourceId = Convert.ToInt32(deletebutton.CommandArgument);
            if (resourceId == 0)
            {
                resourceId = Convert.ToInt32(HiddenFieldNprResource.Value);
            }
            DataTable dtNprResource = objNPRResourcesManager.LoadNprResources();     //GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources);
            DataRow drnprresourceItem = null;
            if (dtNprResource != null && dtNprResource.Rows.Count > 0)
            {
                DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, resourceId));
                if (nprResourcesColl != null && nprResourcesColl.Count() > 0)
                {
                    dtNprResource = nprResourcesColl.CopyToDataTable();
                    drnprresourceItem = dtNprResource.Rows[0];
                }
            }
            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR", TicketID);
            List<UGITTask> ptasks = objUGITTaskManager.LoadByProjectID("NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));

            // Only delete if task count is zero
            if (ptasks != null && ptasks.Count == 0)
            {
                UGITTask tsk = new UGITTask();
                tsk.ID = Convert.ToInt16(dtNprResource.Rows[0][DatabaseObjects.Columns.Id]);
                //tsk.Title = uHelper.GetLookupValue(Convert.ToString(nprResources[0][DatabaseObjects.Columns.UserSkillLookup]));
                tsk.StartDate = Convert.ToDateTime(dtNprResource.Rows[0][DatabaseObjects.Columns.AllocationStartDate]);
                tsk.DueDate = Convert.ToDateTime(dtNprResource.Rows[0][DatabaseObjects.Columns.AllocationEndDate]);
                tsk.EstimatedHours = Convert.ToInt16(dtNprResource.Rows[0][DatabaseObjects.Columns.EstimatedHours]);
                tsk.PercentComplete = Convert.ToDouble(dtNprResource.Rows[0][DatabaseObjects.Columns.TicketNoOfFTEs]);

                //List<int> existingUsers = new List<int>();
                //if (tsk.AssignedTo != null)
                //    existingUsers = tsk.AssignedTo.Select(x => x.LookupId).ToList();
                //RMMSummaryHelper.DeleteAllocationsByTask(SPContext.Current.Web, ptasks, existingUsers, "NPR", Convert.ToString(nprTicket[DatabaseObjects.Columns.TicketId]));
            }
            foreach (DataRow dr in dtNprResource.Rows)
            {
                GetTableDataManager.delete<int>(DatabaseObjects.Tables.NPRResources, DatabaseObjects.Columns.ID, Convert.ToString(dtNprResource.Rows[0][DatabaseObjects.Columns.ID]));
            }
            // nprResources[0].Delete();
            UpdateProjectStartEndDate();
            if(drnprresourceItem!=null)
                UpdateNPRMonthlyDistributionResource(drnprresourceItem, null);
            BindNPRResourceList();
        }

        public void UpdateProjectStartEndDate()
        {
            DataRow nprTicket = Ticket.GetCurrentTicket(context, "NPR", TicketID);
            List<UGITTask> nprTasks = objUGITTaskManager.LoadByProjectID("NPR", Convert.ToString(UGITUtility.GetSPItemValue(nprTicket, DatabaseObjects.Columns.TicketId)));
            if (nprTasks.Count == 0)
            {
                DataTable dtNprResource = objNPRResourcesManager.LoadNprResources();
                if (dtNprResource != null && dtNprResource.Rows.Count > 0)
                {
                    DataRow[] nprResourcesColl = dtNprResource.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID));
                    if (nprResourcesColl != null && nprResourcesColl.Count() > 0)
                    {
                        dtNprResource = nprResourcesColl.CopyToDataTable();
                        DateTime minDate = dtNprResource.AsEnumerable().Min(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate));
                        DateTime maxDate = dtNprResource.AsEnumerable().Max(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate));
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = minDate;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = maxDate;
                        DataTable dt = TicketDal.SaveTickettemp(nprTicket, "NPR", false);
                    }
                    else
                    {
                        nprTicket[DatabaseObjects.Columns.TicketActualStartDate] = DBNull.Value;
                        nprTicket[DatabaseObjects.Columns.TicketActualCompletionDate] = DBNull.Value;
                        DataTable dt = TicketDal.SaveTickettemp(nprTicket, "NPR", true);
                    }
                }
            }
        }

        protected void PreviousYearReadOnly_Click(object sender, EventArgs e)
        {
            try
            {
                int.Parse(currentYearHidden.Value);
            }
            catch
            {
                currentYearHidden.Value = DateTime.Now.Year.ToString();
            }
            currentYear = int.Parse(currentYearHidden.Value) - 1;
            currentYearHidden.Value = currentYear.ToString();
            currentYearHidden.Value = (currentYear).ToString();
            BindProjectPlan();
        }

        protected void NextYearReadOnly_Click(object sender, EventArgs e)
        {

            try
            {
                int.Parse(currentYearHidden.Value);
            }
            catch
            {
                currentYearHidden.Value = DateTime.Now.Year.ToString();
            }
            currentYear = int.Parse(currentYearHidden.Value) + 1;
            currentYearHidden.Value = (currentYear).ToString();
            currentYearHidden.Value = (currentYear).ToString();
            BindProjectPlan();
        }

        protected void aspxBudgetgrid_OnHtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.BudgetAmount)
            {
                if (e.GetValue(DatabaseObjects.Columns.BudgetAmount) == null) return;
                e.Cell.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.BudgetAmount)), "currencywithoutdecimal");

                //if (UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.BudgetAmount)) <= 0)
                //{
                //    e.Cell.Text = "<span style='color: Red;'>" + uHelper.FormatNumber(UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.UnapprovedAmount)), "currency") + " (UA)" + "</span>";
                //}
                if (UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.UnapprovedAmount)) > 0)
                {
                    e.Cell.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.BudgetAmount)), "currencywithoutdecimal") + "<span style='color: Red;'>" + " ( " + uHelper.FormatNumber(UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.UnapprovedAmount)), "currencywithoutdecimal") + " (UA) )" + "</span>";
                }
            }
        }

        protected void aspxBudgetActualsgrid_OnHtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.BudgetAmount)
            {
                if (e.GetValue(DatabaseObjects.Columns.BudgetAmount) == null) return;

                e.Cell.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.GetValue(DatabaseObjects.Columns.BudgetAmount)), "currencywithoutdecimal");
            }


        }

        protected void aspxBudgetActualsgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;
            DataRow currentRow = aspxBudgetActualsgrid.GetDataRow(e.VisibleIndex);
            if (currentRow == null) return;
            string func = string.Empty;
            string resourceID = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
            {
                resourceID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
            }
            func = string.Format("openDialog('{0}','{1}','{2}','{3}','{4}', 0)", UGITUtility.GetAbsoluteURL(BudgetActualEditUrl + "&ID=" + resourceID + "&TicketID=" + TicketID + "&ModuleName=" + ModuleName + "&IsTabActive=true" + "&folderName="), "", "Edit Actual ", "800px", "500px");
            HtmlImage imgEdit = (HtmlImage)aspxBudgetActualsgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgEdit");
            imgEdit.Attributes.Add("onclick", func);

            ImageButton lnkDelete = aspxBudgetActualsgrid.FindRowCellTemplateControl(e.VisibleIndex, null, "imgDelete") as ImageButton;

            if (isBaseline)
            {
                lnkDelete.Visible = false;
                imgEdit.Visible = false;
            }

        }

        protected void aspxBudgetgrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName == "BudgetAmount")
            {
                e.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.Value), "currencywithoutdecimal");
            }
        }

        protected void aspxBudgetActualsgrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName == "BudgetAmount")
            {
                e.Text = uHelper.FormatNumber(UGITUtility.StringToDouble(e.Value), "currencywithoutdecimal");
            }
        }

        protected void BtnRejectBudget_Click(object sender, EventArgs e)
        {
            try
            {
                int itemId = 0;
                
                if (itemId > 0)
                {
                    // Load the budget object.
                    ModuleBudget budgetItem = objModuleBudgetManager.LoadById(itemId, TicketID, ModuleName);

                    if (budgetItem != null)
                    {
                        // Delete the budget item from original list.
                        // budgetItem.Comment = txtBudgetComment.Text.Trim();
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Reject;
                        budgetMessage.Text = "Budget Item '" + budgetItem.BudgetItem + "' Rejected";
                        //  budgetItem.Save();
                        budgetMessage.ForeColor = System.Drawing.Color.Blue;

                        // Make an history entry
                        DataTable dtModuleBudget = objModuleBudgetManager.LoadModuleBudget();
                        DataRow drbudget = dtModuleBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID))[0];
                        string historyTxt = string.Format("Budget item {0} rejected", budgetItem.BudgetItem);
                        uHelper.CreateHistory(User, historyTxt, drbudget,context);
                        
                    }
                }
            }
            catch
            {
                budgetMessage.Text = "Error in Rejecting Budget";
                budgetMessage.ForeColor = System.Drawing.Color.Red;
            }
            BindBudgetActual();
            BindModuleBudgetData();
            
        }

        protected void BtDeleteBudget_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton deletebutton = (ImageButton)sender;
                int itemId = Convert.ToInt32(deletebutton.CommandArgument);
                if (itemId == 0)
                {
                    itemId = Convert.ToInt32(HiddenFieldDeleteBudget.Value);
                }

                DataTable dtModuleBudget = new DataTable();
                DataRow drBudget;
                dtModuleBudget = objModuleBudgetManager.LoadModuleBudget();
                if (itemId > 0)
                {
                    drBudget = dtModuleBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, itemId))[0];
                    
                    if (drBudget != null)
                    {
                        int subCategoryId = 0;
                        int.TryParse(Convert.ToString(drBudget[DatabaseObjects.Columns.BudgetCategoryLookup]), out subCategoryId);
                        string budgetSubCategory = Convert.ToString(drBudget[DatabaseObjects.Columns.BudgetCategoryLookup]);

                        DataTable dtactualList = objBudgetActualsManager.LoadModuleBudgetActuals();
                        DataRow[] dractuals = dtactualList.Select(string.Format("{0}='{1}' And {2}={3}", DatabaseObjects.Columns.TicketId, TicketID, DatabaseObjects.Columns.ModuleBudgetLookup, itemId));
                        
                        if (dractuals.Count() > 0)
                        {
                            foreach (DataRow itemactual in dractuals)
                            {
                                BudgetActual objModuleBudgetActual = objBudgetActualsManager.LoadByID(Convert.ToInt32(itemactual[DatabaseObjects.Columns.ID]));
                                objBudgetActualsManager.Delete(objModuleBudgetActual);
                            }
                            
                        }

                        // Update Project Monthly distribution of Actual in pmmMonthlybudget list.
                        objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketID, ModuleName);

                        // PMMBudget pmmBudget = PMMBudget.LoadById(itemId, TicketID);
                        ModuleBudget moduleBudget = objModuleBudgetManager.LoadById(itemId, TicketID, ModuleName);

                        //if (pmmBudget.BudgetStatus == (int)Enums.BudgetStatus.Approve)
                        {
                            // Update Project Monthly distribution of Budget in pmmMonthlybudget list.
                            // PMMBudget.UpdateProjectMonthlyDistributionBudget(pmmBudget, null);
                            objModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(moduleBudget, null);

                            if (drBudget != null)
                            {
                                ModuleBudget objModuleBudget = objModuleBudgetManager.LoadByID(Convert.ToInt32(drBudget[DatabaseObjects.Columns.ID]));
                                objModuleBudgetManager.Delete(objModuleBudget);
                            }

                            // delete the budget item from original list.
                            //   budgetItem.Delete();


                            // Make an history entry
                            //  pmmItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);
                            // string historyTxt = string.Format("Budget item {0} deleted", pmmBudget.BudgetItem);
                            //   uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyTxt, pmmItem);
                            DataRow drbudget = dtModuleBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID))[0];
                            string historyTxt = string.Format("Budget item {0} rejected", moduleBudget.BudgetItem);
                            // Update monthly distribution of Budget.
                            // PMMBudget.UpdateNonProjectMonthlyDistributionBudget(subCategoryId);
                            objModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(subCategoryId);

                            // Update monthly distribution of Actuals.
                            // PMMBudget.UpdateNonProjectMonthlyDistributionActual(subCategoryId);
                            objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(subCategoryId);

                            budgetMessage.Text = "Budget Item Deleted Successfully.";
                            budgetMessage.ForeColor = System.Drawing.Color.Blue;
                        }
                    }

                    //DDLBudgetItems_Load(ddlActualBudget, new EventArgs());
                }
            }
            catch
            {
                budgetMessage.Text = "Error in Deleting Budget";
                budgetMessage.ForeColor = System.Drawing.Color.Red;
            }
            BindBudgetActual();
            BindModuleBudgetData();

        }

        protected void cmntBudgetSave_Click(object sender, EventArgs e)
        {
            try
            {
                int itemId = 0;
                int.TryParse(approveValue.Value, out itemId);
                if (itemId > 0)
                {
                    ModuleBudget budgetItem = objModuleBudgetManager.LoadById(itemId, TicketID, ModuleName);
                    
                    ModuleBudget oldBudgetItem = ModuleBudgetManager.Clone(budgetItem);
                    if (budgetItem != null)
                    {
                        // Update status of budget item and save it.
                        budgetItem.BudgetAmount += budgetItem.UnapprovedAmount;
                        budgetItem.UnapprovedAmount = 0;
                        budgetItem.Comment = txtBudgetComment.Text.Trim();
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Approve;
                        objModuleBudgetManager.Save(budgetItem);

                        // Make an history entry

                        string historyTxt = string.Format("Budget item {0} approved", budgetItem.BudgetItem);
                        DataRow item = Ticket.GetCurrentTicket(context, ModuleName, TicketID);
                        uHelper.CreateHistory(context.CurrentUser, historyTxt, item, context);


                        // Update Monthly distribution of budget item in PMM Monthly budget list. Table:ModuleMonthlyBudget
                       // objModuleBudgetManager.UpdateProjectMonthlyDistributionBudget(oldBudgetItem, budgetItem);

                        objModuleBudgetManager.UpdateProjectMonthlyDistribution(context,  pmmBudgetNeedsApproval, budgetItem,TicketID, ModuleName);
                        // Update Monthly distribution of budget's actual in PMM Monthly budget list.
                        //objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketID, ModuleName);

                        // Update budget's subcategory monthly distribution.
                        //objModuleBudgetManager.UpdateNonProjectMonthlyDistributionBudget(budgetItem.budgetCategory.ID);

                        // Update budget's actuals monthly distribution by subcategory.
                        //objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(budgetItem.budgetCategory.ID);

                        budgetMessage.Text = "Budget Item '" + budgetItem.BudgetItem + "' Approved";
                        budgetMessage.ForeColor = System.Drawing.Color.Blue;
                        //uHelper.ClosePopUpAndEndResponse(Context, true);
                        comntbudget.ShowOnPageLoad = false;
                    }                 
                }
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
                comntbudget.ShowOnPageLoad = false;
                budgetMessage.Text = "Error in approving budget";
                budgetMessage.ForeColor = System.Drawing.Color.Red;
            }
            BindBudgetActual();
            BindModuleBudgetData();
        }

        protected void cmntBudgetReject_Click(object sender, EventArgs e)
        {
            try
            {
                // SPWeb thisWeb = SPContext.Current.Web;
                int itemId = 0;
                int.TryParse(rejectValue.Value, out itemId);

                if (itemId > 0)
                {
                    // Load the budget object.
                    ModuleBudget budgetItem = objModuleBudgetManager.LoadById(itemId, TicketID, ModuleName);

                    if (budgetItem != null)
                    {
                        budgetItem.UnapprovedAmount = 0;
                        // Delete the budget item from original list.
                        budgetItem.Comment = txtBudgetComment.Text.Trim();
                        budgetItem.BudgetStatus = (int)Enums.BudgetStatus.Reject;
                        budgetMessage.Text = "Budget Item '" + budgetItem.BudgetItem + "' Rejected";
                        objModuleBudgetManager.Save(budgetItem);
                        //budgetItem.Save();
                        budgetMessage.ForeColor = System.Drawing.Color.Blue;

                        // Make an history entry
                        // thisWeb.AllowUnsafeUpdates = true;
                        // pmmItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);
                        // DataRow pmmItem = ModuleBudgetManager.LoadModuleBudget(); 
                        DataTable dtModuleBudget = objModuleBudgetManager.LoadModuleBudget();
                        DataRow drbudget = dtModuleBudget.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketID))[0];
                        string historyTxt = string.Format("Budget item {0} rejected", budgetItem.BudgetItem);
                        uHelper.CreateHistory(User, historyTxt, drbudget,context);
                        uHelper.ClosePopUpAndEndResponse(Context, true);
                        // thisWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch
            {
                budgetMessage.Text = "Error in Rejecting Budget";
                budgetMessage.ForeColor = System.Drawing.Color.Red;
            }
            BindBudgetActual();
            BindModuleBudgetData();
        }

        private DataTable GetBudgetPlanPivotGridData()
        {
            DateTime startDate1 = new DateTime(currentYear, 1, 1);

            DateTime endDate1 = new DateTime(currentYear, 12, 31);

            DataTable dtModuleMonthlyBudget = ModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            DataRow[] drPmmMonthlyBudgetCollection = dtModuleMonthlyBudget.Select(string.Format("{0}>='{1}' And {0}<='{2}' And {3}='{4}'", DatabaseObjects.Columns.AllocationStartDate, startDate1.ToString("yyyy-MM-dd"), endDate1.ToString("yyyy-MM-dd"), DatabaseObjects.Columns.TicketId, TicketID));

            DataTable projectPlan = new DataTable();

            projectPlan.Columns.Add("Title", typeof(string));
            projectPlan.Columns.Add("AllocationStartDate", typeof(DateTime));
            projectPlan.Columns.Add("Total", typeof(double));

            double budgetSum = 0;
            double actualSum = 0;
            double varianceSum = 0;
            double staffSum = 0;
            double OnSiteConsultantSum = 0;
            double OffSiteConsultantSum = 0;

            foreach (DataRow item in drPmmMonthlyBudgetCollection)
            {
                DataRow budgetTotalRow = projectPlan.NewRow();
                budgetTotalRow["Title"] = "Budget";
                budgetTotalRow["AllocationStartDate"] = item[DatabaseObjects.Columns.AllocationStartDate];
                budgetTotalRow["Total"] = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                budgetSum = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                projectPlan.Rows.Add(budgetTotalRow);

                DataRow actualTotalRow = projectPlan.NewRow();
                actualTotalRow["Title"] = "Actual";
                actualTotalRow["AllocationStartDate"] = item[DatabaseObjects.Columns.AllocationStartDate];
                actualTotalRow["Total"] = Convert.ToDouble(item[DatabaseObjects.Columns.ActualCost]);
                actualSum = Convert.ToDouble(item[DatabaseObjects.Columns.ActualCost]);
                projectPlan.Rows.Add(actualTotalRow);


                DataRow varianceTotalRow = projectPlan.NewRow();
                varianceTotalRow["Title"] = "Variance";
                varianceTotalRow["AllocationStartDate"] = item[DatabaseObjects.Columns.AllocationStartDate];
                varianceSum = budgetSum - actualSum;
                varianceTotalRow["Total"] = varianceSum;
                projectPlan.Rows.Add(varianceTotalRow);

                DataRow staffTotalRow = projectPlan.NewRow();
                staffTotalRow["Title"] = "Staff";
                staffTotalRow["Total"] = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                staffSum = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                projectPlan.Rows.Add(staffTotalRow);

                DataRow OnSiteConsultantsTotalRow = projectPlan.NewRow();
                OnSiteConsultantsTotalRow["Title"] = "On-Site Consultants";
                OnSiteConsultantsTotalRow["Total"] = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                OnSiteConsultantSum = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                projectPlan.Rows.Add(OnSiteConsultantsTotalRow);

                DataRow OffSiteConsultantsTotalRow = projectPlan.NewRow();
                OffSiteConsultantsTotalRow["Title"] = "Off-Site Consultants";
                OffSiteConsultantsTotalRow["Total"] = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                OffSiteConsultantSum = Convert.ToDouble(item[DatabaseObjects.Columns.BudgetAmount]);
                projectPlan.Rows.Add(OffSiteConsultantsTotalRow);

            }

            return projectPlan;
        }

        protected void aspxBudgetgrid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            if (e.MenuType == GridViewContextMenuType.Rows && ((ASPxGridView)sender).VisibleRowCount > 0 && !isBaseline)
            {
                var itemApproved = e.CreateItem("Approve", "Approve");
                itemApproved.Image.Url = "/Content/Images/Approved16x16.png";
                var itemReject = e.CreateItem("Reject", "Reject");
                itemReject.Image.Url = "/Content/Images/Rejected16x16.png";
                var itemActual = e.CreateItem("Actual", "Actual");
                itemActual.Image.Url = "/Content/Images/invoice-icon.png";
                itemApproved.BeginGroup = true;
                itemReject.BeginGroup = true;
                itemActual.BeginGroup = true;
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), itemReject);
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), itemApproved);
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), itemActual);
            }
        }

        protected void aspxBudgetgrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < aspxBudgetgrid.VisibleRowCount; i++)
                list.Add((int)aspxBudgetgrid.GetRowValues(i, DatabaseObjects.Columns.BudgetStatus));
            e.Properties["cpBudgetStatus"] = list;

        }

        private DataTable GetBudgetPlan(bool isBaseline=false,int baselineId=0)
        {

            DateTime startDate1 = new DateTime(currentYear, 1, 1);

            DateTime endDate1 = new DateTime(currentYear, 12, 31);

            DataTable dtModuleMonthlyBudget = new DataTable();

            // New Code  **** 
            // SPQuery monthlyBusgetQuery = new SPQuery();


            //monthlyBusgetQuery.Query = string.Format("<Where><And><And><Geq><FieldRef Name='{0}' IncludeTimeValue='FALSE'></FieldRef><Value Type='DateTime'>{1}</Value></Geq><Leq><FieldRef Name='{0}' IncludeTimeValue='FALSE'></FieldRef><Value Type='DateTime'>{2}</Value></Leq></And><Eq><FieldRef LookupId='True' Name='{3}'></FieldRef><Value Type='Lookup'>{4}</Value></Eq></And></Where>",
            //                            DatabaseObjects.Columns.AllocationStartDate, startDate1.ToString("yyyy-MM-dd"), endDate1.ToString("yyyy-MM-dd"), DatabaseObjects.Columns.TicketPMMIdLookup, PMMID);
            //SPListItemCollection pmmMonthlyBudgetCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMMonthlyBudget, monthlyBusgetQuery);

            //DateTime startDate1 = new DateTime(currentYear, 1, 1);
            // DateTime endDate1 = new DateTime(currentYear, 12, 31);

            if (isBaseline)
            {
                dtModuleMonthlyBudget = ModuleMonthlyBudgetHistoryManager.GetDataTable($"{DatabaseObjects.Columns.BaselineId}={baselineId}");
            }

            else
            {
                dtModuleMonthlyBudget = ModuleMonthlyBudgetManager.LoadModuleMonthlyBudget();

            }

            DataRow[] drPmmMonthlyBudgetCollection = dtModuleMonthlyBudget.Select(string.Format("{0}>='{1}' And {0}<='{2}' And {3}='{4}'", DatabaseObjects.Columns.AllocationStartDate, startDate1.ToString("yyyy-MM-dd"), endDate1.ToString("yyyy-MM-dd"), DatabaseObjects.Columns.TicketId, TicketID));

            DataTable projectPlan = new DataTable();
            projectPlan.Columns.Add("EnableEdit", typeof(bool));
            projectPlan.Columns.Add("Title", typeof(string));
            projectPlan.Columns.Add("Total", typeof(string));
            projectPlan.Columns.Add("Category", typeof(string));

            // Add column for months as "Month1","Month2".
            for (int i = 1; i <= 12; i++)
            {
                projectPlan.Columns.Add("Month" + i.ToString(), typeof(string));
            }

            // Import the budget total monthly ditribution in dummyTable.
            DataRow budgetTotalRow = projectPlan.NewRow();
            budgetTotalRow["Category"] = "0";
            budgetTotalRow["Title"] = "Budget";
            budgetTotalRow["EnableEdit"] = false;
            budgetTotalRow["Total"] = String.Format("{0:C}", 0);

            DataRow actualTotalRow = projectPlan.NewRow();
            actualTotalRow["Category"] = "0";
            actualTotalRow["Title"] = "Actual";
            actualTotalRow["EnableEdit"] = false;
            actualTotalRow["Total"] = String.Format("{0:C}", 0);

            DataRow labourChargesRow = projectPlan.NewRow();
            labourChargesRow["Category"] = "0";
            labourChargesRow["Title"] = "Timesheet Cost";
            labourChargesRow["EnableEdit"] = false;
            labourChargesRow["Total"] = String.Format("{0:C}", 0);

            DataRow varianceTotalRow = projectPlan.NewRow();
            varianceTotalRow["Category"] = "0";
            varianceTotalRow["Title"] = "<b>Variance</b>";
            varianceTotalRow["EnableEdit"] = false;
            varianceTotalRow["Total"] = String.Format("{0:C}", 0);

            DataRow staffTotalRow = projectPlan.NewRow();
            staffTotalRow["Category"] = "0";
            staffTotalRow["Title"] = "Staff";
            staffTotalRow["EnableEdit"] = false;
            staffTotalRow["Total"] = String.Format("{0} WMs", 0);

            DataRow OnSiteConsultantsTotalRow = projectPlan.NewRow();
            OnSiteConsultantsTotalRow["Category"] = "0";
            OnSiteConsultantsTotalRow["Title"] = "On-Site Consultants";
            OnSiteConsultantsTotalRow["EnableEdit"] = false;
            OnSiteConsultantsTotalRow["Total"] = String.Format("{0} WMs", 0);

            DataRow OffSiteConsultantsTotalRow = projectPlan.NewRow();
            OffSiteConsultantsTotalRow["Category"] = "0";
            OffSiteConsultantsTotalRow["Title"] = "Off-Site Consultants";
            OffSiteConsultantsTotalRow["EnableEdit"] = false;
            OffSiteConsultantsTotalRow["Total"] = String.Format("{0} WMs", 0);

            // Make all the values set as "$0.00" by default.
            for (int i = 1; i <= 12; i++)
            {
                budgetTotalRow["Month" + i.ToString()] = String.Format("{0:C}", 0);
                actualTotalRow["Month" + i.ToString()] = String.Format("{0:C}", 0);
                labourChargesRow["Month" + i.ToString()] = String.Format("{0:C}", 0);
                varianceTotalRow["Month" + i.ToString()] = String.Format("{0:C}", 0);
                staffTotalRow["Month" + i.ToString()] = String.Format("{0}", 0);
                OnSiteConsultantsTotalRow["Month" + i.ToString()] = String.Format("{0}", 0);
                OffSiteConsultantsTotalRow["Month" + i.ToString()] = String.Format("{0}", 0);

            }

            // Get Labour Charges for selected year
            //DataTable currentYearLabourCharges = GetCurrentYearLabourCharges();

            //if (drPmmMonthlyBudgetCollection.Length > 0 || currentYearLabourCharges.Rows.Count > 0)
            if (drPmmMonthlyBudgetCollection.Length > 0)
            {
                bool isNewDataTable = true;
                DataTable pmmMonthlyTable = new DataTable();

                if (drPmmMonthlyBudgetCollection.Length > 0)
                {
                    pmmMonthlyTable = drPmmMonthlyBudgetCollection.CopyToDataTable();
                    isNewDataTable = false;
                }
                //else if (currentYearLabourCharges.Rows.Count > 0)
                //{
                //    pmmMonthlyTable = currentYearLabourCharges;
                //}

                // Adding a new column 'LabourCharges' to pmmMonthlyTable
                if (!isNewDataTable)
                {
                    //pmmMonthlyTable.Columns.Add(DatabaseObjects.Columns.LabourCharges);

                    // Adding labour charges data to pmmMonthlyTable
                    //if (currentYearLabourCharges.Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in currentYearLabourCharges.Rows)
                    //    {
                    //        DataRow newRow = pmmMonthlyTable.NewRow();
                    //        newRow[DatabaseObjects.Columns.AllocationStartDate] = Convert.ToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]);
                    //        newRow[DatabaseObjects.Columns.BudgetType] = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.BudgetType]);
                    //        newRow[DatabaseObjects.Columns.BudgetAmount] = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.BudgetAmount]);
                    //        newRow[DatabaseObjects.Columns.ActualCost] = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.ActualCost]);
                    //        newRow[DatabaseObjects.Columns.TicketPMMIdLookup] = Convert.ToString(row[DatabaseObjects.Columns.TicketPMMIdLookup]);
                    //        //newRow[DatabaseObjects.Columns.LabourCharges] = uHelper.StringToDouble(row[DatabaseObjects.Columns.LabourCharges]);
                    //        pmmMonthlyTable.Rows.Add(newRow);
                    //    }
                    //}
                }

                pmmMonthlyTable.DefaultView.Sort = DatabaseObjects.Columns.AllocationStartDate + " ASC";
                pmmMonthlyTable = pmmMonthlyTable.DefaultView.ToTable();

                DateTime tempDate = startDate1;
                double grandTotalBudget = 0;
                double grandTotalActual = 0;
                double grandTotalLabourCharges = 0;
                double grandTotalVariance = 0;
                double grandTotalStaff = 0;
                double grandTotalOnSiteConsultants = 0;
                double grandTotalOffSiteConsultants = 0;

                // loop through start to end date.
                while (tempDate <= endDate1)
                {
                    double budgetSum = 0;
                    double actualSum = 0;
                    double labourChargesSum = 0;
                    double varianceSum = 0;
                    double staffSum = 0;
                    double OnSiteConsultantSum = 0;
                    double OffSiteConsultantSum = 0;

                    // Get the current month rows.
                    var monthTotal =
                       (from summaryTable in pmmMonthlyTable.AsEnumerable()
                        where summaryTable.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).Month == tempDate.Month
                        select summaryTable);

                    // Add all budget and actuals if it is more then one(in case, but it should be only one for each month).
                    foreach (DataRow dr in monthTotal)
                    {
                        // Only sum entries with budget type = 0 (dollars)
                        if (Convert.ToString(dr[DatabaseObjects.Columns.BudgetType]) == "0")
                        {
                            if (!DBNull.Value.Equals(dr[DatabaseObjects.Columns.BudgetAmount]))
                                budgetSum += Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);

                            if (!DBNull.Value.Equals(dr[DatabaseObjects.Columns.ActualCost]))
                                actualSum += Convert.ToDouble(dr[DatabaseObjects.Columns.ActualCost]);

                            //if (!DBNull.Value.Equals(dr[DatabaseObjects.Columns.]))
                            //    labourChargesSum += Convert.ToDouble(dr[DatabaseObjects.Columns.LabourCharges]);
                        }
                        else if (Convert.ToString(dr[DatabaseObjects.Columns.BudgetType]) == "Staff")
                            staffSum += Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                        else if (Convert.ToString(dr[DatabaseObjects.Columns.BudgetType]) == "On-Site Consultants")
                            OnSiteConsultantSum += Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                        else if (Convert.ToString(dr[DatabaseObjects.Columns.BudgetType]) == "Off-Site Consultants")
                            OffSiteConsultantSum += Convert.ToDouble(dr[DatabaseObjects.Columns.BudgetAmount]);
                    }

                    varianceSum = budgetSum - actualSum - labourChargesSum;

                    budgetTotalRow["Month" + tempDate.Month.ToString()] = String.Format("{0:C}", budgetSum);
                    actualTotalRow["Month" + tempDate.Month.ToString()] = String.Format("{0:C}", actualSum);
                    labourChargesRow["Month" + tempDate.Month.ToString()] = String.Format("{0:C}", labourChargesSum);
                    staffTotalRow["Month" + tempDate.Month.ToString()] = String.Format("{0}", staffSum);
                    OnSiteConsultantsTotalRow["Month" + tempDate.Month.ToString()] = String.Format("{0}", OnSiteConsultantSum);
                    OffSiteConsultantsTotalRow["Month" + tempDate.Month.ToString()] = String.Format("{0}", OffSiteConsultantSum);

                    if (varianceSum < 0)
                        varianceTotalRow["Month" + tempDate.Month.ToString()] = "<span style='color: Red;'>" + String.Format("{0:C}", varianceSum) + "</span>";
                    else if (varianceSum > 0)
                        varianceTotalRow["Month" + tempDate.Month.ToString()] = "<span style='color: Green;'>" + String.Format("{0:C}", varianceSum) + "</span>";
                    else
                        varianceTotalRow["Month" + tempDate.Month.ToString()] = varianceSum;

                    grandTotalBudget += budgetSum;
                    grandTotalActual += actualSum;
                    grandTotalLabourCharges += labourChargesSum;
                    grandTotalVariance += varianceSum;
                    grandTotalStaff += staffSum;
                    grandTotalOnSiteConsultants += OnSiteConsultantSum;
                    grandTotalOffSiteConsultants += OffSiteConsultantSum;

                    tempDate = tempDate.AddMonths(1);
                }

                budgetTotalRow["Total"] = String.Format("{0:C}", grandTotalBudget);
                actualTotalRow["Total"] = String.Format("{0:C}", grandTotalActual);
                labourChargesRow["Total"] = String.Format("{0:C}", grandTotalLabourCharges);
                staffTotalRow["Total"] = String.Format("{0} WMs", Math.Round(grandTotalStaff, 2));
                OnSiteConsultantsTotalRow["Total"] = String.Format("{0} WMs", Math.Round(grandTotalOnSiteConsultants, 2));
                OffSiteConsultantsTotalRow["Total"] = String.Format("{0} WMs", Math.Round(grandTotalOffSiteConsultants, 2));


                if (grandTotalVariance < 0)
                    varianceTotalRow["Total"] = "<span style='color: Red;'>" + String.Format("{0:C}", grandTotalVariance) + "</span>";
                else if (grandTotalVariance > 0)
                    varianceTotalRow["Total"] = "<span style='color: Green;'>" + String.Format("{0:C}", grandTotalVariance) + "</span>";
                else
                    varianceTotalRow["Total"] = grandTotalVariance;
            }

            projectPlan.Rows.Add(budgetTotalRow);
            if (ModuleName == ModuleNames.PMM)
            {
                projectPlan.Rows.Add(actualTotalRow);
                projectPlan.Rows.Add(labourChargesRow);
                projectPlan.Rows.Add(varianceTotalRow);
            }
            if (ModuleName == ModuleNames.NPR)
            {
                projectPlan.Rows.Add(staffTotalRow);
                projectPlan.Rows.Add(OnSiteConsultantsTotalRow);
                projectPlan.Rows.Add(OffSiteConsultantsTotalRow);
            }
            return projectPlan;
        }

        protected void BindResourceCostViewControl()
        {
            ResourceCostView costView = (ResourceCostView)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Budget/ResourceCostView.ascx");
            costView.TicketId = TicketID;
            costView.ModuleName = ModuleName;
            pnlResourceCostView.Controls.Add(costView);
            LabourChargesData = costView.LabourChargesData;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if(ModuleName == ModuleNames.NPR)
                CategoryName= "NPRBudget";
            if (ModuleName == ModuleNames.PMM)
                CategoryName = "PMMBudget";
            List<ModuleColumn> moduleColumnsList = new List<ModuleColumn>();
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context);
            DataTable table = (DataTable)aspxBudgetgrid.DataSource;
            List<string> fields = new List<string>();
            List<string> dtSchema = null; 
            dtSchema = table.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            if (dtSchema != null && dtSchema.Count > 0)
                fields = dtSchema;
            moduleColumnsList = moduleColumnManager.Load($"{DatabaseObjects.Columns.CategoryName}='{CategoryName}'").OrderBy(x => x.FieldSequence).ToList();
            if (moduleColumnsList != null && moduleColumnsList.Count != 0)
            {
                List<string> selectedfields = new List<string>();
                foreach (ModuleColumn moduleColumn in moduleColumnsList)
                {
                    if (fields.Contains(moduleColumn.FieldName))
                    {
                        //fields.RemoveAll(x => x == moduleColumn.FieldName);
                        selectedfields.Add(moduleColumn.FieldName);
                    }
                }
                DataView dtview = new DataView(table);
                table = dtview.ToTable("ExportedData", false, selectedfields.ToArray());
            }
            var worksheet = SpreadSheetConfigVar.Document.Worksheets.ActiveWorksheet;
            worksheet.FreezeRows(0);
            worksheet.Import(table, true, 0, 0);
            MemoryStream st = new MemoryStream();
            SpreadSheetConfigVar.Document.SaveDocument(st, DocumentFormat.OpenXml);
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=BudgetData.xlsx");
            Response.BinaryWrite(st.ToArray());
            Response.End();
            
        }

        protected void aspxModuleResourceList_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName.Contains("Lookup"))
            {
                string lookupid = Convert.ToString(e.Value);
                string values = ObjFieldConfigManager.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
                if (!string.IsNullOrEmpty(values))
                {
                    e.DisplayText = values;
                }
            }
            if (e.Column.FieldName.EndsWith("User"))
            {
                string userIDs = Convert.ToString(e.Value);
                if (!string.IsNullOrEmpty(userIDs))
                {
                    if (userIDs != null)
                    {
                        string separator = Constants.Separator6;
                        if (userIDs.Contains(Constants.Separator))
                            separator = Constants.Separator;
                        List<string> userlist = UGITUtility.ConvertStringToList(userIDs, separator);

                        string commanames = UserManager.CommaSeparatedNamesFrom(userlist, Constants.Separator6);
                        e.DisplayText = !string.IsNullOrEmpty(commanames) ? commanames : string.Empty;
                    }
                }
            }
        }
    }
}