
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
namespace uGovernIT.Web
{
    public partial class PriorityMappingView : UserControl
    {
        string moduleName = string.Empty;
        //DataTable dtRequestPriority = null;
        //DataTable dtPriority = null;
        PrioirtyViewManager objPrioirtyViewManager;
        ImpactManager objImpactManager;
        SeverityManager objSeverityManager;
        ApplicationContext context;
        List<ModulePrioirty> lstModulePrioirty;
        List<ModuleImpact> lstModuleImpact;
        List<ModuleSeverity> lstModuleSeverity;
        RequestPriorityManager objRequestPriorityManager;
        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objPrioirtyViewManager = new PrioirtyViewManager(context);
            objRequestPriorityManager = new RequestPriorityManager(context);
            //lblMsg.Text = "";
            BindModuleName();
            if (Request["module"] != null)
            {
                moduleName = Request["module"].ToString();
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(moduleName));
            }
            else
            {
                moduleName = ddlModule.SelectedValue;
            }
            lstModulePrioirty = objPrioirtyViewManager.Load(x => x.ModuleNameLookup == moduleName && x.Deleted.Equals(false));

            bindGrid(moduleName);
            base.OnInit(e);
        }

        private void bindGrid(string SelectedItem)
        {
            DataTable mappingTable = new DataTable();
            objImpactManager = new ImpactManager(context);
            objSeverityManager = new SeverityManager(context);
            lstModuleImpact = objImpactManager.Load(s => s.ModuleNameLookup.Equals(SelectedItem) && s.Deleted.Equals(false), x => x.ItemOrder).ToList();
            if (lstModuleImpact != null && lstModuleImpact.Count > 0)
            {
                if (lstModuleImpact.Count > 0)
                {
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                 
                    lstModuleSeverity = objSeverityManager.Load();
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

                            gridPriority.DataSource = mappingTable;
                            gridPriority.DataBind();
                        }
                    }
                }
            }
            
            if (mappingTable == null || mappingTable.Rows.Count == 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "No Impact and/or Severity available for the selected module.";
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = ddlModule.SelectedValue;
            bindGrid(moduleName);
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=priortymapping&&Width=1024&Height=736.8&pageTitle=Priority%20Mapping&isdlg=1&isudlg=1&module=" + moduleName + "");
            Response.Redirect(url);
        }

        private void BindModuleName()
        {
            ddlModule.Items.Clear();
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> dtModule = moduleViewManager.Load(x => x.EnableModule && x.ModuleType == ModuleType.SMS);
            //DataRow[] moduleRows = dtModule.Select(string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.EnableModule, true,DatabaseObjects.Columns.ModuleType, ModuleType.SMS));
            foreach (UGITModule moduleRow in dtModule)
            {
                ddlModule.Items.Add(new ListItem { Text = moduleRow.Title, Value = moduleRow.ModuleName });
            }
            ddlModule.DataBind();
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
                        string divHtml = "<div class='divcell_0'>" +
                                            "<span style='margin: 3px; float: right'>Severity</span>" +
                                            "<br>" +
                                            "<span style='margin: 3px; float: left'>Impact</span>" +
                                         "</div>";

                        cell.Text = divHtml;
                        cell.CssClass = "cell_0_0 diagonalLineTR";

                    }
                    else
                    {
                        cell.CssClass = "header";
                    }
                    index++;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            {
                if (lstModulePrioirty.Count > 0)
                {
                    lstModulePrioirty = lstModulePrioirty.Where(x=> x.ModuleNameLookup.Equals(ddlModule.SelectedValue)).ToList();
                    if (lstModulePrioirty.Count > 0)
                    {
                      
                        AddControls(lstModulePrioirty, e.Row);
                    }
                }
            }
        }

        private void AddControls(List<ModulePrioirty> dt, GridViewRow row)
        {
            try
            {
                lstModulePrioirty = lstModulePrioirty.OrderBy(s=> s.ItemOrder).ToList();
                int index = 0;
                foreach (DataControlFieldCell cell in row.Cells)
                {
                    if (index == 0)
                    {
                        cell.CssClass = "tdrowhead";
                    }
                    if (index > 0)
                    {
                        DropDownList dropDownList = new DropDownList();
                        dropDownList.ID = "drp_" + row.RowIndex.ToString() + "_" + index.ToString();
                        dropDownList.Width = 140;
                        dropDownList.DataSource = dt;
                        dropDownList.DataValueField = DatabaseObjects.Columns.Id;
                        dropDownList.DataTextField = DatabaseObjects.Columns.UPriority;
                        dropDownList.DataBind();
                        dropDownList.Items.Insert(0, new ListItem("None", ""));
                        ModuleImpact objModuleImpact = lstModuleImpact.FirstOrDefault(x => x.Impact.Equals(row.Cells[0].Text));
                        ModuleSeverity objModuleSeverity = lstModuleSeverity.FirstOrDefault(x => x.Severity.Equals(gridPriority.HeaderRow.Cells[index].Text));
                       
                        List<ModulePriorityMap> obj = objRequestPriorityManager.Load(x=> x.ModuleNameLookup.Equals(ddlModule.SelectedValue) && x.ImpactLookup.Equals(objModuleImpact.ID) && x.SeverityLookup.Equals(objModuleSeverity.ID)).ToList();
                        if (obj != null &&  obj.Count > 0)
                        {
                            ModulePriorityMap objectModulePriorityMap = obj[0];
                            ModulePrioirty priority = lstModulePrioirty.FirstOrDefault(x => x.ID == objectModulePriorityMap.PriorityLookup);
                          
                            if (priority!=null)
                            {
                                dropDownList.SelectedValue = Convert.ToString(priority.ID);
                            }
                            else
                            {
                                dropDownList.SelectedValue = "0";
                            }
                        }
                        cell.Wrap = false;
                        cell.Controls.Add(dropDownList);
                    }
                    index += 1;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<ModulePriorityMap> spRequestPriorityList = objRequestPriorityManager.Load();
                string SelectedModel = ddlModule.SelectedValue;
                foreach (GridViewRow row in gridPriority.Rows)
                {
                    int index = 0;
                    string TicketImpacts = row.Cells[0].Text;
                    ModuleImpact obj = lstModuleImpact.FirstOrDefault(x => x.Impact.Equals(TicketImpacts));
                    foreach (DataControlFieldCell cell in row.Cells)
                    {
                        if (index > 0)
                        {
                            string TicketSeverity = gridPriority.HeaderRow.Cells[index].Text;
                            ModuleSeverity objmodulesevrity = lstModuleSeverity.FirstOrDefault(x=> x.Severity.Equals(TicketSeverity));
                            DropDownList dropDownList = row.Cells[index].FindControl("drp_" + row.RowIndex.ToString() + "_" + index.ToString()) as DropDownList;
                           
                            ModulePriorityMap objModulePriorityMap = spRequestPriorityList.FirstOrDefault(x=> x.ModuleNameLookup.Equals(SelectedModel) && x.SeverityLookup.Equals(objmodulesevrity.ID) && x.ImpactLookup.Equals(obj.ID));
                        
                            if (objModulePriorityMap != null)
                            {
                                ModulePriorityMap objRequestPriority = objModulePriorityMap;
                               
                                if (dropDownList==null|| string.IsNullOrWhiteSpace(dropDownList.SelectedValue))
                                {
                                    objRequestPriorityManager.Delete(objRequestPriority);
                                    
                                }
                                else
                                {
                                    objRequestPriority.PriorityLookup = Convert.ToInt32(dropDownList.SelectedValue);
                                    objRequestPriorityManager.Update(objRequestPriority);
                                   
                                }
                            }
                            else if (dropDownList != null && !string.IsNullOrWhiteSpace(dropDownList.SelectedValue))
                            {
                                ModulePriorityMap objRequestPriority = new ModulePriorityMap();
                                objRequestPriority.ModuleNameLookup = SelectedModel;
                                List<ModuleImpact> lst = objImpactManager.Load(x => x.ModuleNameLookup== SelectedModel && x.Impact == TicketImpacts).ToList();
                                if (lst != null && lst.Count > 0)
                                {
                                    objRequestPriority.ImpactLookup = lst[0].ID;
                                }
                                //to get severity ID.
                                List<ModuleSeverity> lstModuleSeverity = objSeverityManager.Load(y => y.ModuleNameLookup == SelectedModel && y.Severity == TicketSeverity);
                                if (lstModuleSeverity != null && lstModuleSeverity.Count > 0)
                                {
                                    objRequestPriority.SeverityLookup = lstModuleSeverity[0].ID;
                                   
                                }
                                objRequestPriority.PriorityLookup = Convert.ToInt32(dropDownList.SelectedValue);
                                objRequestPriorityManager.Insert(objRequestPriority);
                            }
                        }
                        index += 1;
                    }
                }
                lblMsg.Text = "Saved successfully.";

            }
            catch (Exception ex)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = ex.ToString();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(moduleName, false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }

            string cacheName = "Lookup_" + DatabaseObjects.Tables.RequestPriority + "_" + context.TenantID;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestPriority, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            Util.Cache.CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, dt);
        }
    }
}
