using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ITSMCtrl : UserControl
    {
        ApplicationContext context;
        PrioirtyViewManager objPrioirtyViewManager;
        ImpactManager objImpactManager;
        SeverityManager objSeverityManager;
        TicketManager ticketManager = null;
        ModuleViewManager moduleViewManager;
        List<ModulePrioirty> lstModulePrioirty;
        List<ModuleImpact> lstModuleImpact;
        List<ModuleSeverity> lstModuleSeverity;
        RequestPriorityManager objRequestPriorityManager;
        ModuleImpact objModuleImpact;
        ModuleSeverity objModuleSeverity;
        List<ModulePriorityMap> obj;
        public string moduleName = "";
        DataTable legendTable = new DataTable();

        public Unit Height { get; set; }
        public Unit Width { get; set; }
        public string HeaderText { get; set; }
        public string BottleNeckUrlUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=workflowbottleneck");

        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objPrioirtyViewManager = new PrioirtyViewManager(context);
            objSeverityManager = new SeverityManager(context);
            moduleViewManager = new ModuleViewManager(context);
            ticketManager = new TicketManager(context);
            objRequestPriorityManager = new RequestPriorityManager(context);

            bindImpactGrid("TSR");
            bindSlaGrid("TSR");

        }

        protected void Impact_Callback(object sender, CallbackEventArgsBase e)
        {
            if (string.IsNullOrEmpty(e.Parameter))
                bindImpactGrid("TSR");
            else
                bindImpactGrid(Convert.ToString(e.Parameter));
        }

        protected void SlaPerformance_Callback(object sender, CallbackEventArgsBase e)
        {
            bindSlaGrid("TSR");
        }

        private void bindImpactGrid(string SelectedItem)
        {
            moduleName = SelectedItem;
            lstModulePrioirty = objPrioirtyViewManager.Load(x => x.ModuleNameLookup == moduleName && x.Deleted.Equals(false));
            DataTable Opentickets = new DataTable();
            DataTable mappingTable = new DataTable();
            objImpactManager = new ImpactManager(context);
            objSeverityManager = new SeverityManager(context);
            lstModuleImpact = objImpactManager.Load(s => s.ModuleNameLookup.Equals(SelectedItem) && s.Deleted.Equals(false), x => x.ItemOrder).ToList();
            if (lstModuleImpact != null && lstModuleImpact.Count > 0)
            {
                if (lstModuleImpact.Count > 0)
                {
                    Opentickets = ticketManager.GetOpenTickets(moduleViewManager.LoadByName(SelectedItem, true));
                    if (Opentickets == null && Opentickets.Rows.Count <= 0)
                    {
                        gridPriority.DataSource = null;
                        gridPriority.DataBind();
                        return;
                    }

                    lstModuleSeverity = objSeverityManager.Load(x => x.Deleted == false);
                    if (lstModuleSeverity != null && lstModuleSeverity.Count > 0)
                    {
                        lstModuleSeverity = lstModuleSeverity.Where(x => x.ModuleNameLookup.Equals(SelectedItem) && x.Deleted.Equals(false)).OrderBy(x => x.ItemOrder).ToList();
                        if (lstModuleSeverity.Count > 0)
                        {
                            mappingTable.Columns.Add(" ");
                            for (int s = 0; s < lstModuleSeverity.Count; s++)
                            {
                                mappingTable.Columns.Add(Convert.ToString(lstModuleSeverity[s].Title));
                            }

                            for (int k = 0; k < lstModuleImpact.Count; k++)
                            {
                                mappingTable.Rows.Add();
                                mappingTable.Rows[k][0] = Convert.ToString(lstModuleImpact[k].Impact);
                            }

                            for (int i = 0; i < mappingTable.Rows.Count; i++)
                            {
                                for (int j = 1; j < mappingTable.Columns.Count; j++)
                                {
                                    //mappingTable.Rows[i][j] = $"{lstModuleImpact[i].ID} - {lstModuleSeverity[j - 1].ID} ";
                                    mappingTable.Rows[i][j] = Opentickets.AsEnumerable().Where(x => x.Field<Int64?>(DatabaseObjects.Columns.TicketImpactLookup) == lstModuleImpact[i].ID && x.Field<Int64?>(DatabaseObjects.Columns.TicketSeverityLookup) == lstModuleSeverity[j - 1].ID).Count();
                                    
                                }
                            }

                            gridPriority.DataSource = mappingTable;
                            gridPriority.DataBind();
                        }
                    }
                }
            }
            else
            {
                gridPriority.DataSource = Opentickets;
                gridPriority.DataBind();
            }
        }

        protected void gridPriority_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int index = 0;
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    if (index == 0)
                    {
                        string divHtml = "<div class='d-flex justify-content-between px-2 higt'>" +
                                            "<span class='impact-headerLable align-self-end mb-1'>Impact</span>" +
                                            "<span class='severity-headerLable align-self-start mt-1'>Severity</span>" +
                                         "</div>";

                        cell.Text = divHtml;
                        cell.CssClass = "cell_0_0 diagonalLineTR";

                    }
                    else
                    {
                        cell.CssClass = "grid-header";
                    }
                    index++;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            {
                if (lstModulePrioirty.Count > 0)
                {
                    lstModulePrioirty = lstModulePrioirty.Where(x => x.ModuleNameLookup.Equals(moduleName)).ToList();
                    if (lstModulePrioirty.Count > 0)
                    {

                        SetControls(lstModulePrioirty, e.Row);
                    }
                }
            }
        }

        private void SetControls(List<ModulePrioirty> dt, GridViewRow row)
        {
            try
            {
                lstModulePrioirty = lstModulePrioirty.OrderBy(s => s.ItemOrder).ToList();
                int index = 0;
                foreach (DataControlFieldCell cell in row.Cells)
                {
                    if (index == 0)
                    {
                        cell.CssClass = "tdrowhead";
                    }
                    if (index > 0)
                    {

                        objModuleImpact = lstModuleImpact.FirstOrDefault(x => x.Impact.Equals(row.Cells[0].Text));
                        objModuleSeverity = lstModuleSeverity.FirstOrDefault(x => x.Severity.Equals(gridPriority.HeaderRow.Cells[index].Text));

                        obj = objRequestPriorityManager.Load(x => x.ModuleNameLookup.Equals(moduleName) && x.ImpactLookup.Equals(objModuleImpact.ID) && x.SeverityLookup.Equals(objModuleSeverity.ID)).ToList();
                        if (obj != null && obj.Count > 0)
                        {
                            ModulePriorityMap objectModulePriorityMap = obj[0];
                            ModulePrioirty priority = lstModulePrioirty.FirstOrDefault(x => x.ID == objectModulePriorityMap.PriorityLookup);

                            if (priority != null)
                            {
                                // dropDownList.SelectedValue = Convert.ToString(priority.ID);
                                cell.BackColor = ColorTranslator.FromHtml(priority.Color);
                                cell.Attributes["onclick"] = $"openDrillDownImpacts('{moduleName}','{objModuleImpact.ID}','{objModuleSeverity.ID}')";
                                cell.ToolTip = "Show List";
                            }
                            else
                            {
                                //dropDownList.SelectedValue = "0";
                            }
                        }

                        cell.Wrap = false;

                    }
                    index += 1;
                }
            }
            catch (Exception)
            {
            }
        }
        private void bindSlaGrid(string SelectedItem)
        {
            HeaderText = "Days";
            DataTable slaTable = uHelper.GetSLAAndTabularDashboardData(HttpContext.Current.GetManagerContext(), SelectedItem, null, "d", includeOpen: false);
            if (slaTable != null && slaTable.Rows.Count > 0)
            {
                DataRow[] dtRow = slaTable.AsEnumerable().GroupBy(r => r.Field<string>(DatabaseObjects.Columns.Title)).Select(g => g.FirstOrDefault()).ToArray();
                if (dtRow != null && dtRow.Length > 0)
                {
                    slaTable = dtRow.CopyToDataTable();
                    DataView view = slaTable.DefaultView;
                    view.Sort = string.Format("{0},{1} asc", DatabaseObjects.Columns.StartStageStep, DatabaseObjects.Columns.Title);
                    slaTable = view.ToTable();
                }
                rptSLAParent.DataSource = legendTable = slaTable;
                rptSLAParent.DataBind();
            }

            if (legendTable.Rows.Count <= 0)
                rptSLAParent.Visible = false;
        }

        protected void rptSLAParent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Visible = false; //ShowTotal;
                Label lbTotalTarget = e.Item.FindControl("lblSLATargetX2Total") as Label;
                Label lbTotalActual = e.Item.FindControl("lblSLAActualX2Total") as Label;
                if (legendTable != null && legendTable.Rows.Count > 0)
                {
                    lbTotalTarget.Text = Math.Round(Convert.ToDouble(legendTable.Compute("sum(SLATargetX2)", "")), 2).ToString();
                    lbTotalActual.Text = Math.Round(Convert.ToDouble(legendTable.Compute("sum(SLAActualX2)", "")), 2).ToString("n2");
                }
            }
        }
    }
}