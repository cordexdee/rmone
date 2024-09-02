using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using System.Threading;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace uGovernIT.Web
{
    public partial class SLARulesView : UserControl
    {
        //DataTable spSLARules;
        string addNewItem;
        private string TenantID = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager objConfigurationVariableHelper;
        SlaRulesManager slaRuleManager;
        PrioirtyViewManager prioirtyViewManager = null;
        protected override void OnPreRender(EventArgs e)
        {

            //show process button if refresh escalation is in progress state
            ProcessAllTicketEscalationHelper escHelper1 = new ProcessAllTicketEscalationHelper(context);
            if (escHelper1.ProcessState())
            {
                dxRefreshingEscalation.Visible = true;
                dxReGenerateEscalation.Visible = false;
            }
            else
            {
                dxRefreshingEscalation.Visible = false;
                dxReGenerateEscalation.Visible = true;
            }

            base.OnPreRender(e);
        }
        protected override void OnInit(EventArgs e)
        {
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            slaRuleManager = new SlaRulesManager(context);
            prioirtyViewManager = new PrioirtyViewManager(context);
            TenantID = Convert.ToString(Session["TenantID"]);
            addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=slarulenew");
            if (Request["module"] != null)
            {
                string module = Convert.ToString(Request["module"]);
                addNewItem = string.Format("{0}&Module={1}", addNewItem, module);
            }
          //  aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Rule - New Item','650','500',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
           // aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Rule - New Item','650','500',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            string aAddItem_TopURL = string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Rule - New Item','650','870',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath));
            string aAddItem_BottomURL = string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Rule - New Item','650','870',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath));
            aAddItem_Top.ClientSideEvents.Click = "function(){ " + aAddItem_TopURL + " }";
            aAddItem.ClientSideEvents.Click = "function(){ " + aAddItem_BottomURL + " }";
            BindModule();
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["module"] != null)
                {
                    string module = Convert.ToString(Request["module"]);
                    dxddlModule.Value = module;
                }
                else
                {
                    addNewItem = string.Format("{0}&Module={1}", addNewItem, dxddlModule.Value);
                    dxddlModule.SelectedIndex = 0;
                }


                if (Request["showdelete"] != null)
                {
                    string showdelete = Convert.ToString(Request["showdelete"]);
                    dxShowDeleted.Checked = showdelete == "0" ? false : true;
                }
            }
            BindGridView(Convert.ToString(dxddlModule.Value));

            base.OnLoad(e);
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.Value);
            BindGridView(moduleName);
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule&pageTitle=SLA Rule&isdlg=1&isudlg=1&module=" + moduleName + "&showdelete=" + showdelete);
            Response.Redirect(url);
        }
        void BindModule()
        {
            dxddlModule.Items.Clear();
            ModuleViewManager ObjModuleViewManager = new ModuleViewManager(context);
            List<UGITModule> _dataTable = null;
            List<ModuleSLARule> list = slaRuleManager.Load();
            if (list.Count() > 0)
            {
                List<string> moduleName = list.Select(y => y.ModuleNameLookup).Distinct().ToList();
                _dataTable = ObjModuleViewManager.Load(x => x.EnableModule).Where(y => moduleName.Contains(y.ModuleName)).OrderBy(x => x.ModuleName).ToList();
                if (_dataTable != null && _dataTable.Count() > 0)
                {
                    _dataTable.ForEach(moduleRow =>
                    {
                        dxddlModule.Items.Add(new ListEditItem { Text = Convert.ToString(moduleRow.Title), Value = moduleRow.ModuleName });
                    });
                }
            }
        }
        void BindGridView(string Module)
        {
            List<ModulePrioirty> modulePrioirties =  prioirtyViewManager.LoadByModule(Module);
            ModulePrioirty modulePriority = null;
            DataTable dt = slaRuleManager.GetDataTable($"{DatabaseObjects.Columns.ModuleNameLookup} = '{Module}'");
            
            if (!dxShowDeleted.Checked)
            {
                DataRow[] dr = dt.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, Module, DatabaseObjects.Columns.Deleted, "False"));
                dt = null;
                if (dr.Count() > 0)
                {
                    dt = dr.CopyToDataTable();
                }
               
            }
            //else
            //{
            //    DataRow[] dr = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, Module));
            //    if (dr.Count() > 0)
            //    {
            //        dt = dr.CopyToDataTable();
            //    }
            //}
            
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("StartStage"))
                    dt.Columns.Add("StartStage", typeof(string));
                if (!dt.Columns.Contains("EndStage"))
                    dt.Columns.Add("EndStage", typeof(string));
                if (!dt.Columns.Contains("SLA"))
                    dt.Columns.Add("SLA", typeof(string));
                if (!dt.Columns.Contains("Priority"))
                    dt.Columns.Add("Priority", typeof(string));

                int hoursInADay = uHelper.GetWorkingHoursInADay(context, true);

                foreach (DataRow dr in dt.Rows)
                {
                    string startStep = Convert.ToString(dr[DatabaseObjects.Columns.StartStageStep]);
                    string endStep = Convert.ToString(dr[DatabaseObjects.Columns.EndStageStep]);
                    string startstage = string.Empty;
                    string endstage = string.Empty;
                    LifeCycleManager objLifeCycleHelper = new LifeCycleManager(context);
                    if (!string.IsNullOrEmpty(Module))
                    {
                        LifeCycle obj = objLifeCycleHelper.LoadLifeCycleByModule(Module)[0];
                        if (obj != null)
                        {
                            if (obj.Stages.Where(x=>x.StageStep== Convert.ToInt32(startStep)).ToList().Count>0)
                                startstage = obj.Stages.FirstOrDefault(x => x.StageStep == Convert.ToInt32(startStep)).StageTitle;
                            if(obj.Stages.Where(x => x.StageStep == Convert.ToInt32(endStep)).ToList().Count > 0)
                               endstage = obj.Stages.FirstOrDefault(x => x.StageStep == Convert.ToInt32(endStep)).StageTitle;
                        }

                    }
                    if (!string.IsNullOrEmpty(startStep))
                        dr["StartStage"] = string.Format("{0} - {1}", dr[DatabaseObjects.Columns.StartStageStep], startstage);
                    else
                        dr["StartStage"] = startstage;//Convert.ToString(dr[DatabaseObjects.Columns.StageTitleLookup]);

                    if (!string.IsNullOrEmpty(startStep))
                        dr["EndStage"] = string.Format("{0} - {1}", dr[DatabaseObjects.Columns.EndStageStep], endstage);
                    else
                        dr["EndStage"] = endstage; //Convert.ToString(dr[DatabaseObjects.Columns.EndStageTitleLookup]);

                    double SLAHours = Convert.ToDouble(dr["SLAHours"]);
                    if (SLAHours % hoursInADay == 0)
                    {
                        dr["SLA"] = string.Format("{0:0.##} Days", SLAHours / hoursInADay);
                    }
                    else if (SLAHours % 1 == 0)
                    {
                        dr["SLA"] = string.Format("{0:0.##} Hrs", SLAHours);
                    }
                    else
                    {
                        dr["SLA"] = string.Format("{0:0.##} Mins", Convert.ToInt32(SLAHours * 60));
                    }

                    modulePriority = modulePrioirties.FirstOrDefault(x => x.ID == Convert.ToInt64(dr[DatabaseObjects.Columns.TicketPriorityLookup]));
                    if (modulePriority != null)
                        dr["Priority"] = modulePriority.Title;
                }

                dt.DefaultView.Sort = "SLACategoryChoice, StartStage, EndStage, Priority";
                dx_SPGrid.DataSource = dt.DefaultView;
                dx_SPGrid.DataBind();
                dx_SPGrid.ExpandAll();
            }
        }
        protected void dxReGenerateEscalation_Click(object sender, EventArgs e)
        {
            ProcessAllTicketEscalationHelper escHelper = new ProcessAllTicketEscalationHelper(context);
            if (!escHelper.ProcessState())
            {
                Thread escThread = new Thread(delegate ()
                {
                    escHelper.ReGenerateAllEscalations();
                });
                escThread.Start();

                dxRefreshingEscalation.Visible = true;
                dxReGenerateEscalation.Visible = false;
            }
            else
            {
                dxRefreshingEscalation.Visible = true;
                dxReGenerateEscalation.Visible = false;
            }
        }
        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.Value);
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule&pageTitle=SLA Rule&isdlg=1&isudlg=1&module=" + moduleName + "&showdelete=" + showdelete);
            Response.Redirect(url);

        }
        protected void dxddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.Value);
            BindGridView(moduleName);
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=slarule&pageTitle=SLA Rule&isdlg=1&isudlg=1&module=" + moduleName + "&showdelete=" + showdelete);
            Response.Redirect(url);
        }
        protected void dx_SPGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == "Title")
            {
                int Index = e.VisibleIndex;
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string Title = (string)e.GetValue(DatabaseObjects.Columns.Title);
                string edititem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=slaruleedit&ID=" + datakeyvalue + " ");
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','650','870',0,'{1}','true')", edititem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
                HtmlAnchor ahtml = (HtmlAnchor)dx_SPGrid.FindRowCellTemplateControl(Index, e.DataColumn, "editLink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    ahtml.InnerText = Convert.ToString(e.CellValue);
                }

            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(Convert.ToString(dxddlModule.Value), false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }
        }
    }
}
