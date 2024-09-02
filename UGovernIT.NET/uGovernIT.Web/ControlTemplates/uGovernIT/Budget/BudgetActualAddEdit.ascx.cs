using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class BudgetActualAddEdit : System.Web.UI.UserControl
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string TicketId { get; set; }
        public string ModuleName { get; set; }

        private BudgetActual actualBudget { get; set; }
        private ApplicationContext _context = null;
        private DMSManagerService _dmsManagerService = null;
        private BudgetActualsManager _budgetActualsManager = null;
        private ModuleBudgetManager _moduleBudgetManager = null;
        private AssetVendorViewManager _assetVendorViewManager = null;

        StringBuilder linkFile = new StringBuilder();

        DataTable dtActuals;
        DataRow drActuals;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(ApplicationContext);
                }
                return _dmsManagerService;
            }
        }

        protected BudgetActualsManager BudgetActualsManager
        {
            get
            {
                if (_budgetActualsManager == null)
                {
                    _budgetActualsManager = new BudgetActualsManager(ApplicationContext);
                }
                return _budgetActualsManager;
            }
        }

        protected ModuleBudgetManager ModuleBudgetManager
        {
            get
            {
                if (_moduleBudgetManager == null)
                {
                    _moduleBudgetManager = new ModuleBudgetManager(ApplicationContext);
                }
                return _moduleBudgetManager;
            }
        }

        protected AssetVendorViewManager AssetVendorViewManager
        {
            get
            {
                if (_assetVendorViewManager == null)
                {
                    _assetVendorViewManager = new AssetVendorViewManager(ApplicationContext);
                }
                return _assetVendorViewManager;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            //BudgetActualsManager = new BudgetActualsManager(context);
            //objModuleBudgetManager = new ModuleBudgetManager(context);
            //objAssetVendorViewManager = new AssetVendorViewManager(context);

            dtActuals = BudgetActualsManager.LoadModuleBudgetActuals();
            if (Id > 0)
            {
                drActuals = dtActuals.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Id))[0];
                // Save();
            }

            glActualBudget1.DataBind();
            FillEditForm();

        }

        public void FillEditForm()
        {
            if (Id > 0)
            {
                txtActualTitle.Text = Convert.ToString(drActuals[DatabaseObjects.Columns.Title]);
                glActualBudget1.GridView.Selection.SetSelectionByKey(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(drActuals[DatabaseObjects.Columns.ModuleBudgetLookup]))), true);
                txtActualAmount.Text = Convert.ToString(drActuals[DatabaseObjects.Columns.BudgetAmount]);
                dtcActualStartDate.Date = Convert.ToDateTime(drActuals[DatabaseObjects.Columns.AllocationStartDate]);
                dtcActualEndDate.Date = Convert.ToDateTime(drActuals[DatabaseObjects.Columns.AllocationEndDate]);
                txtActualNotes.Text = Convert.ToString(drActuals[DatabaseObjects.Columns.BudgetDescription]);
                txtInvoiceNumber.Text = Convert.ToString(drActuals[DatabaseObjects.Columns.InvoiceNumber]);
                cbVendorList.Value = Convert.ToString(drActuals[DatabaseObjects.Columns.VendorLookup]);
                var Attachments = Convert.ToString(drActuals[DatabaseObjects.Columns.Attachments]);

                if (!string.IsNullOrEmpty(Attachments))
                {
                    FileUploadControl.SetValue(Attachments);
                }

            }
            if (BudgetId > 0)
            {
                glActualBudget1.GridView.Selection.SetSelectionByKey(BudgetId, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //rvactualstartdate.ControlToValidate = dtcActualStartDate.ID;
                //cvactualstartdate.ControlToValidate = dtcActualStartDate.ID;
                //rvactualenddate.ControlToValidate = dtcActualEndDate.ID;
                //cvactualenddate.ControlToValidate = dtcActualEndDate.ID;
                //cpactualenddate.ControlToValidate = dtcActualEndDate.ID;
            }

        }

        protected void btBudgetActualSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void glActualBudget1_DataBinding(object sender, EventArgs e)
        {
            DataTable budgets = null;
            budgets = ModuleBudgetManager.LoadBudgetByTicketID(TicketId);
            glActualBudget1.GridView.Width = 400;
            glActualBudget1.DataSource = budgets;
        }

        protected void cbVendorList_Load(object sender, EventArgs e)
        {
            if (cbVendorList.Items.Count > 0)
                return;
            List<AssetVendor> vendors = AssetVendorViewManager.Load();
            if (vendors != null && vendors.Count > 0)
            {
                cbVendorList.ValueField = DatabaseObjects.Columns.Id;
                cbVendorList.TextField = DatabaseObjects.Columns.Title;
                cbVendorList.DataSource = vendors;
                cbVendorList.DataBind();
            }
            cbVendorList.Items.Insert(0, new ListEditItem("--None--", ""));
        }

        protected void Save()
        {
            BudgetActual actualBudget = new BudgetActual();

            double amount;
            if (txtActualAmount != null && double.TryParse(txtActualAmount.Text, out amount))
            {
                actualBudget.BudgetAmount = amount;
            }

            if (txtActualNotes != null)
            {
                actualBudget.BudgetDescription = txtActualNotes.Text;
            }

            if (dtcActualStartDate.Date != null)
            {
                actualBudget.AllocationStartDate = dtcActualStartDate.Date;
            }

            if (dtcActualEndDate.Date != null)
            {
                actualBudget.AllocationEndDate = dtcActualEndDate.Date;
            }

            if (txtActualTitle != null)
            {
                actualBudget.Title = txtActualTitle.Text;

            }

            if (txtInvoiceNumber != null)
            {
                actualBudget.InvoiceNumber = txtInvoiceNumber.Text;
            }

            actualBudget.TicketId = TicketId;

            actualBudget.ModuleName = ModuleName;

            if (cbVendorList.SelectedIndex > 0)
            {
                actualBudget.VendorLookup = UGITUtility.StringToLong(cbVendorList.SelectedItem.Value); //Convert.ToString(cbVendorList.SelectedItem.Value);
                actualBudget.VendorName = cbVendorList.SelectedItem.Text;
            }

            else
            {
                actualBudget.VendorLookup = null;

            }
            actualBudget.ModuleBudgetLookup = Convert.ToInt32(glActualBudget1.Value);
            actualBudget.BudgetItem = glActualBudget1.Text;

            actualBudget.ID = Id;

            //Bind attached files
            if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                actualBudget.Attachments = FileUploadControl.GetValue();

            UpdateCommanFunction(actualBudget);
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=modulebudget");
        }

        protected BudgetActual UpdateCommanFunction(BudgetActual ObjActuals)
        {
            // BudgetActuals objbudgetActuals = new BudgetActuals();
            BudgetActualsManager.InsertORUpdateData(ObjActuals);
            BudgetActualsManager.UpdateProjectMonthlyDistributionActual(TicketId, ModuleName);
            //Update TicketTotalCost in PMM list.
            DataTable budgets = ModuleBudgetManager.GetPMMBudgetActualList(TicketId);
            if (budgets != null && budgets.Rows.Count > 0)
            {
                DataTable dtPMM = GetTableDataManager.GetTableData("PMM", $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                DataRow[] pmmItem = dtPMM.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, TicketId));
                if (pmmItem.Count() > 0)
                {
                    foreach (DataRow item in pmmItem)
                    {
                        item[DatabaseObjects.Columns.ProjectCost] = budgets.AsEnumerable().Sum(x => Convert.ToDouble(x.Field<string>(DatabaseObjects.Columns.BudgetAmount)));
                        Ticket objticket = new Ticket(ApplicationContext, ModuleName);
                        objticket.CommitChanges(item);
                    }
                }

            }
            ModuleBudget budget = ModuleBudgetManager.LoadById(UGITUtility.StringToLong(glActualBudget1.Value), TicketId, ModuleName);
            BudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(budget.budgetCategory.ID);
            
            return ObjActuals;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);

        }
    }
}