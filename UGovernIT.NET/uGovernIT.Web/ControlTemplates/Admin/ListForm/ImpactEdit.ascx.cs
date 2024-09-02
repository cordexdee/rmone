
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ImpactEdit : UserControl
    {
        public long TicketID { get; set; }
        public ViewMode Mode { private get; set; }
        private string ListName = string.Empty;
        private string ColumnName = string.Empty;
        private DataRow Ticket;

        ModuleImpact objImpact = new ModuleImpact();
        ModuleSeverity objSeverity = new ModuleSeverity();

        ImpactManager impactMGR;
        SeverityManager severityMGR;
        PrioirtyViewManager priorityMGR;
        ModuleViewManager ObjModuleViewManager = null;
        /// <summary>
        /// Initialize control Event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            switch (Mode)
            {
                case ViewMode.Impact:
                    ListName = DatabaseObjects.Tables.TicketImpact;
                    ColumnName = DatabaseObjects.Columns.Impact;
                    impactMGR = new ImpactManager(HttpContext.Current.GetManagerContext());
                    Ticket = UGITUtility.ObjectToData(impactMGR.LoadByID(TicketID)).Rows[0];
                    break;
                case ViewMode.Severity:
                    ListName = DatabaseObjects.Tables.TicketSeverity;
                    ColumnName = DatabaseObjects.Columns.Severity;
                    severityMGR = new SeverityManager(HttpContext.Current.GetManagerContext());
                    Ticket = UGITUtility.ObjectToData(severityMGR.LoadByID(TicketID)).Rows[0];
                    break;
                case ViewMode.Priority:
                    ListName = DatabaseObjects.Tables.TicketPriority;
                    ColumnName = DatabaseObjects.Columns.Priority;
                    priorityMGR = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
                    Ticket = UGITUtility.ObjectToData(priorityMGR.LoadByID(TicketID)).Rows[0];
                    break;
                default:
                    break;
            }
            //Ticket = SPListHelper.GetSPListItem(ListName, TicketID);
            //BindModule();
            //SPFieldLookupValue spFieldLookupModule = new SPFieldLookupValue(Convert.ToString(Ticket[DatabaseObjects.Columns.ModuleNameLookup]));
            string moduleName = Convert.ToString(Ticket[DatabaseObjects.Columns.ModuleNameLookup]);
            ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));
            txtImpact.Text = Convert.ToString(Ticket[DatabaseObjects.Columns.Title]);
            txtItemOrder.Text = Convert.ToString(Ticket[DatabaseObjects.Columns.ItemOrder]);
            chkDeleted.Checked = Convert.ToBoolean(Ticket[DatabaseObjects.Columns.Deleted]);
            
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string moduleNameLookup = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
            UGITModule uGITModule = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues()));
            switch (Mode)
            {
                case ViewMode.Impact:
                    List<ModuleImpact> moduleImpactList = impactMGR.Load();
                    // DataTable splTickets = UGITUtility.ToDataTable<ModuleImpact>(impactMGR.Load());
                    //DataRow[] lstImpact = splTickets.Select(string.Format("{0} = '{1}' And {2} = '{3}' And {4} <> {5}", DatabaseObjects.Columns.ModuleNameLookup, moduleNameLookup, DatabaseObjects.Columns.Impact, txtImpact.Text.Trim(), DatabaseObjects.Columns.ID, Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID])));
                    moduleImpactList = uGITModule.List_Impacts.Where(x=> x.ModuleNameLookup== moduleNameLookup && x.Impact== txtImpact.Text.Trim() && x.ID!= Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID])).ToList();
                    if (moduleImpactList.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Impact For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    moduleImpactList = uGITModule.List_Impacts.Where(x => x.ModuleNameLookup == moduleNameLookup && x.ItemOrder == Convert.ToInt32(txtItemOrder.Text.Trim()) && x.ID != Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID])).ToList();
                    if (moduleImpactList.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Item order For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }

                    objImpact.ID = Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID]);
                    //Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID]);
                    objImpact.ModuleNameLookup = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
                    objImpact.Title = txtImpact.Text.Trim();
                    objImpact.Impact = txtImpact.Text.Trim();
                    objImpact.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
                    objImpact.Deleted = chkDeleted.Checked;
                    impactMGR.Update(objImpact);
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Impact: {objImpact.Title}, Module: {objImpact.ModuleNameLookup}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    break;
                case ViewMode.Severity:
                    List<ModuleSeverity> ModuleSevrity = severityMGR.Load();
                    ModuleSevrity = uGITModule.List_Severities.Where(x=> x.ModuleNameLookup== moduleNameLookup && x.Severity== txtImpact.Text.Trim() && x.ID!= Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID])).ToList();
                    if (ModuleSevrity.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Severity For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    ModuleSevrity = uGITModule.List_Severities.Where(x => x.ModuleNameLookup == moduleNameLookup && x.ItemOrder == Convert.ToInt32(txtItemOrder.Text.Trim()) && x.ID != Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID])).ToList();
                    if (ModuleSevrity.Count > 0)
                    {
                        lblError.Text = "Cannot Insert Duplicate Item Order For Module " + moduleNameLookup;
                        lblError.Visible = true;
                        return;
                    }
                    objSeverity.ID = Convert.ToInt64(Ticket[DatabaseObjects.Columns.ID]);
                    objSeverity.ModuleNameLookup = ObjModuleViewManager.LoadByID(Convert.ToInt64(ddlModule.GetValues())).ModuleName;
                    objSeverity.Title = txtImpact.Text.Trim();
                    objSeverity.Severity = txtImpact.Text.Trim();
                    objSeverity.ItemOrder = Convert.ToInt32(txtItemOrder.Text.Trim());
                    objSeverity.Deleted = chkDeleted.Checked;
                    severityMGR.Update(objSeverity);
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Impact: {objSeverity.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    break;
                default:
                    break;
            }
            //DataRow dr = uGITCache.GetModuleDetails(ddlModule.SelectedValue);
            //Ticket[DatabaseObjects.Columns.ModuleNameLookup] = dr[DatabaseObjects.Columns.Id];
            //Ticket[DatabaseObjects.Columns.Title] = txtImpact.Text.Trim() ;
            //Ticket[ColumnName] = txtImpact.Text.Trim();
            //Ticket[DatabaseObjects.Columns.ItemOrder] = txtItemOrder.Text.Trim();
            //Ticket[DatabaseObjects.Columns.IsDeleted] = chkDeleted.Checked;

            //Ticket.Update();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
