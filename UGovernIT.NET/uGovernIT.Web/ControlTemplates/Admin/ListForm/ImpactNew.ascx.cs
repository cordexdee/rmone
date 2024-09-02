
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class ImpactNew : UserControl
    {
        public ViewMode Mode { private get; set; }
        private string ListName = string.Empty;
        private string ColumnName = string.Empty;
        ImpactManager impactMGR;
        SeverityManager severityMGR;
        PrioirtyViewManager priorityMGR;
        ModuleViewManager ObjModuleViewManager = null;
        //DataTable splTickets;
        List<ModuleImpact> moduleImpactList;
        List<ModuleSeverity> moduleSeverityList;
        protected override void OnInit(EventArgs e)
        {
            ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            switch (Mode)
            {
                case ViewMode.Impact:
                    ListName = DatabaseObjects.Tables.TicketImpact;
                    ColumnName = DatabaseObjects.Columns.Impact;
                    impactMGR = new ImpactManager(HttpContext.Current.GetManagerContext());
                    // splTickets = UGITUtility.ToDataTable<ModuleImpact>(impactMGR.Load());
                    moduleImpactList = impactMGR.Load();
                    break;
                case ViewMode.Severity:
                    ListName = DatabaseObjects.Tables.TicketSeverity;
                    ColumnName = DatabaseObjects.Columns.Severity;
                    severityMGR = new SeverityManager(HttpContext.Current.GetManagerContext());
                    // splTickets = UGITUtility.ToDataTable<ModuleSeverity>(severityMGR.Load());
                    moduleSeverityList = severityMGR.Load();
                    break;
                case ViewMode.Priority:
                    ListName = DatabaseObjects.Tables.TicketPriority;
                    ColumnName = DatabaseObjects.Columns.Priority;
                    priorityMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
                    break;
                default:
                    break;
            }

            //TicketImpact = SPListHelper.GetSPList(ListName).AddItem();
            //BindModule(); 
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["moduleName"] != null)
                {
                    string moduleName = Request["moduleName"].ToString();
                    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }
                if (Session["ModuleName"] != null)
                {
                    string moduleName = Convert.ToString(Session["ModuleName"]);
                    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string moduleNameLookup = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
            UGITModule uGITModule = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues()));
            switch (Mode)
            {
                case ViewMode.Impact:
                    moduleImpactList = uGITModule.List_Impacts.Where(x=> x.ModuleNameLookup== moduleNameLookup && x.Impact == txtImpact.Text.Trim()).ToList();
                    if (moduleImpactList!=null && moduleImpactList.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Impact For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    //moduleImpactList = uGITModule.List_Impacts.Where(x=> x.ModuleNameLookup==moduleNameLookup && x.ItemOrder== Convert.ToInt32(txtItemOrder.Text.Trim())).ToList();
                    //if (moduleImpactList != null && moduleImpactList.Count > 0)
                    //{
                    //    lblError.Text = "Cannot Insert Duplicate Item Order For Module " + moduleNameLookup;
                    //    lblError.Visible = true;
                    //    return;
                    //}
                    ModuleImpact objImpact = new ModuleImpact();
                    objImpact.ID = 0;
                    objImpact.ModuleNameLookup = moduleNameLookup;
                    objImpact.Title = txtImpact.Text.Trim();
                    objImpact.Impact = txtImpact.Text.Trim();
                    objImpact.ItemOrder = Convert.ToInt32( txtItemOrder.Text.Trim());
                    objImpact.Deleted = chkDeleted.Checked;
                    impactMGR.Insert(objImpact);
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Impact: {objImpact.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    break;
                case ViewMode.Severity:
                    moduleSeverityList = uGITModule.List_Severities.Where(x=> x.ModuleNameLookup== moduleNameLookup && x.Severity== txtImpact.Text.Trim()).ToList();
                    if (moduleSeverityList!=null && moduleSeverityList.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Severity For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    moduleSeverityList = uGITModule.List_Severities.Where(x => x.ModuleNameLookup == moduleNameLookup && x.ItemOrder == Convert.ToInt32(txtItemOrder.Text.Trim())).ToList();
                    if (moduleSeverityList != null && moduleSeverityList.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Item order For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    ModuleSeverity objSeverity = new ModuleSeverity();
                    objSeverity.ID = 0;
                    objSeverity.ModuleNameLookup = moduleNameLookup;
                    objSeverity.Title = txtImpact.Text.Trim();
                    objSeverity.Severity = txtImpact.Text.Trim();
                    objSeverity.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
                    objSeverity.Deleted = chkDeleted.Checked;
                    severityMGR.Insert(objSeverity);
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added Severity: {objSeverity.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    break;
                default:
                    break;
            }
            Session["ModuleName"]= ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
