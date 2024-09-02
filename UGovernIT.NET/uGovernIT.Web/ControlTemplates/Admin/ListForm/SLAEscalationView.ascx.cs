using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;

namespace uGovernIT.Web
{
    public partial class SLAEscalationView : UserControl
    {
        string addNewItem;
        DataTable _SPListEscalations;
        DataTable _dtEscalations;
        DataRow[] moduleRoles = new DataRow[0];

        public string module { get; set; }
        public string moveToProductionUrl = "";
    //    private string TenantID = string.Empty;
        ApplicationContext context = null;
        ConfigurationVariableManager objConfigurationVariableHelper;
        SlaRulesManager objSlaRulesManager;
        ModuleViewManager moduleViewManager;
        ModuleUserTypeManager moduleUserTypeManager;
        ModuleEscalationRuleManager objModuleEscalationRuleManager;
        PrioirtyViewManager prioirtyViewManager = null;

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            moduleUserTypeManager = new ModuleUserTypeManager(this.context);
            objModuleEscalationRuleManager = new ModuleEscalationRuleManager(this.context);
            moduleViewManager = new ModuleViewManager(this.context);
            objSlaRulesManager = new SlaRulesManager(this.context);
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            prioirtyViewManager = new PrioirtyViewManager(context);
            //TenantID = Convert.ToString(Session["TenantID"]);
            addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=escalationrulenew");
            if (Request["module"] != null)
            {
                string module = Convert.ToString(Request["module"]);
                addNewItem = string.Format("{0}&Module={1}", addNewItem, module);
            }
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Escalations - New Item','700','880',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','SLA Escalations - New Item','700','880',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));

            cbEnableEscalation.Checked = UGITUtility.StringToBoolean(objConfigurationVariableHelper.GetValue(ConfigConstants.EnableEscalations));

            _SPListEscalations = objModuleEscalationRuleManager.GetDataTable(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.EscalationRule);
            BindModule();
            EnableMigrate();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            //Migrate Url
            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=movestagetoproduction&list={0}&module={1}", DatabaseObjects.Tables.EscalationRule, Convert.ToString(ddlModule.Value)));
            if (!IsPostBack)
            {
                if (Request["module"] != null)
                {
                    string module = Convert.ToString(Request["module"]);
                    ddlModule.Value = module;
                }
                else 
                {
                    addNewItem = string.Format("{0}&Module={1}", addNewItem, Convert.ToString(ddlModule.Value));
                    ddlModule.SelectedIndex = 0;
                }
            }

            BindGridView();
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //show process button if refresh escalation is in progress state
            ProcessAllTicketEscalationHelper escHelper = new ProcessAllTicketEscalationHelper(context);
            if (escHelper.ProcessState())
            {
                btRefreshingEscalation.Visible = true;
                btReGenerateEscalation.Visible = false;
            }
            else
            {
                btRefreshingEscalation.Visible = false;
                btReGenerateEscalation.Visible = true;
            }

            base.OnPreRender(e);
        }

        private void EnableMigrate()
        {
            if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                btnMigrateSLAEscalation.Visible = true;
            }
        }

        private void BindGridView()
        {
            //Fetch all roles of selected module
            DataTable spListUserEmail = moduleUserTypeManager.GetDataTable(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"TenantID='{TenantID}'");
            DataTable dtUserEmail = spListUserEmail;
            if (dtUserEmail != null)
            {
                DataRow row = dtUserEmail.NewRow();
                row[DatabaseObjects.Columns.UserTypes] = "Escalation Manager";
                row[DatabaseObjects.Columns.ColumnName] = "RequestTypeEscalationManager";
                row[DatabaseObjects.Columns.ModuleNameLookup] = Convert.ToString(ddlModule.Value);
                dtUserEmail.Rows.Add(row);

                row = dtUserEmail.NewRow();
                row[DatabaseObjects.Columns.UserTypes] = "Backup Escalation Manager";
                row[DatabaseObjects.Columns.ColumnName] = "RequestTypeBackupEscalationManager";
                row[DatabaseObjects.Columns.ModuleNameLookup] = Convert.ToString(ddlModule.Value);
                dtUserEmail.Rows.Add(row);

                row = dtUserEmail.NewRow();
                row[DatabaseObjects.Columns.UserTypes] = "PRP Manager";
                row[DatabaseObjects.Columns.ColumnName] = "PRPManager";
                row[DatabaseObjects.Columns.ModuleNameLookup] = Convert.ToString(ddlModule.Value);
                dtUserEmail.Rows.Add(row);

                row = dtUserEmail.NewRow();
                row[DatabaseObjects.Columns.UserTypes] = "ORP Manager";
                row[DatabaseObjects.Columns.ColumnName] = "ORPManager";
                row[DatabaseObjects.Columns.ModuleNameLookup] = Convert.ToString(ddlModule.Value);
                dtUserEmail.Rows.Add(row);
            }
            moduleRoles = dtUserEmail.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, Convert.ToString(ddlModule.Value)));
            if (moduleRoles == null)
                moduleRoles = new DataRow[0];

            DataTable modulePrioirties = prioirtyViewManager.GetDataTable($"{DatabaseObjects.Columns.ModuleNameLookup} = '{ddlModule.Value}'");

            DataTable spSLARules = objSlaRulesManager.GetDataTable($"{DatabaseObjects.Columns.ModuleNameLookup} = '{ddlModule.Value}'");  //GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, $"TenantID='{TenantID}'");
            DataTable dt = spSLARules;
            _dtEscalations = _SPListEscalations;
            if (dt != null && _dtEscalations != null)
            {
                DataRow[] dtMatchRows = (from escRow in _dtEscalations.AsEnumerable()
                                         join slaRow in dt.AsEnumerable() on
                                         (string)escRow[DatabaseObjects.Columns.SlaRuleIdLookup] equals (string)slaRow[DatabaseObjects.Columns.ID]
                                         join priority in modulePrioirties.AsEnumerable() on
                                         (string)slaRow[DatabaseObjects.Columns.TicketPriorityLookup] equals (string)priority[DatabaseObjects.Columns.ID]
                                         where (string)slaRow[DatabaseObjects.Columns.ModuleNameLookup] == Convert.ToString(ddlModule.Value)
                                         orderby slaRow[DatabaseObjects.Columns.SLACategoryChoice], priority[DatabaseObjects.Columns.Title], escRow[DatabaseObjects.Columns.EscalationMinutes]
                                         select escRow).ToArray();

                if (dtMatchRows.Length == 0)
                {
                    _SPGrid.DataSource = null;
                    _SPGrid.DataBind();
                    return;
                }

                DataTable dtMatch = dtMatchRows.CopyToDataTable();
                if (!dtMatch.Columns.Contains("SLA"))
                    dtMatch.Columns.Add("SLA", typeof(string));

                if (!dtMatch.Columns.Contains("SLAFrequency"))
                    dtMatch.Columns.Add("SLAFrequency", typeof(string));
                int hoursInADay = uHelper.GetWorkingHoursInADay(context, true);
                foreach (DataRow rw in dtMatch.Rows)
                {
                    double escalationMinutes = Convert.ToDouble(rw["EscalationMinutes"]);
                    if (escalationMinutes % (hoursInADay * 60) == 0)
                    {
                        rw["SLA"] = string.Format("{0:0.##} Days", escalationMinutes / (hoursInADay * 60));
                    }
                    else if (escalationMinutes % 60 == 0)
                    {
                        rw["SLA"] = string.Format("{0:0.##} Hrs", escalationMinutes / 60);
                    }
                    else
                    {
                        rw["SLA"] = string.Format("{0:0.##} Mins", escalationMinutes);
                    }

                    double escalationFrequency = Convert.ToDouble(rw["EscalationFrequency"]);
                    if (escalationFrequency % (hoursInADay * 60) == 0)
                    {
                        rw["SLAFrequency"] = string.Format("{0:0.##} Days", escalationFrequency / (hoursInADay * 60));
                    }
                    else if (escalationFrequency % 60 == 0)
                    {
                        rw["SLAFrequency"] = string.Format("{0:0.##} Hrs", escalationFrequency / 60);
                    }
                    else
                    {
                        rw["SLAFrequency"] = string.Format("{0:0.##} Mins", escalationFrequency);
                    }
                }

                //dtMatch.DefaultView.Sort = DatabaseObjects.Columns.SlaRuleIdLookup + " ASC, " + DatabaseObjects.Columns.EscalationMinutes + " ASC";
                //_SPGrid.DataSource = dtMatch.DefaultView;
                _SPGrid.DataSource = dtMatch;
                _SPGrid.DataBind();
            }
        }

        private void BindModule()
        {
            List<UGITModule> ugitModule = new List<UGITModule>();
            ugitModule = moduleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            ddlModule.Items.Clear();
            if (ugitModule != null && ugitModule.Count > 0)
            {
                foreach (UGITModule moduleRow in ugitModule)
                {
                    string moduleName = Convert.ToString(moduleRow.ModuleName);
                    if (moduleName != string.Empty)
                        ddlModule.Items.Add(new ListEditItem { Text = Convert.ToString(moduleRow.Title), Value = moduleName });
                }
                ddlModule.DataBind();
            }
        }

        protected void _SPGrid_RowDataBound(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            //DataRowView rowView = (DataRowView)e.VisibleIndex;
            string lsDataKeyValue = Convert.ToString(e.KeyValue);
            string editItem;
            editItem = string.Format("{0}&Module={1}", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=escalationruledit&ID=" + lsDataKeyValue + " "), Convert.ToString(ddlModule.Value));
            HtmlAnchor anchorEdit = (HtmlAnchor)e.Row.FindControlRecursive("editLink");
            HtmlAnchor aSLARule = (HtmlAnchor)e.Row.FindControlRecursive("aSLARule");
            HiddenField hiddenSLARule = (HiddenField)e.Row.FindControlRecursive("hiddenSLARule");


            HtmlAnchor lnkShowdetail = (HtmlAnchor)e.Row.FindControlRecursive("aKeyname");
            string Title = hiddenSLARule.Value;
            string Url = string.Format("javascript:window.UgitOpenPopupDialog('{0}','','SLA Escalations - {2}','700','880',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), Title);
            anchorEdit.Attributes.Add("href", Url);
            aSLARule.Attributes.Add("href", Url);
            aSLARule.InnerText = objSlaRulesManager.LoadByID(Convert.ToInt32(hiddenSLARule.Value)).Title;

            //Get EscalationToRoles cell then change formating.
            TableCell escalationToRoleCell = e.Row.Cells[3];
            string roles = Convert.ToString(e.GetValue(DatabaseObjects.Columns.EscalationToRoles));
            if (!string.IsNullOrEmpty(roles) && moduleRoles.Length > 0)
            {
                List<string> rolesTextArry = new List<string>();
                string[] rolesArray = roles.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                DataRow mRole = null;
                foreach (string r in rolesArray)
                {
                    mRole = moduleRoles.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ColumnName) == r);
                    if (mRole != null)
                    {
                        rolesTextArry.Add(Convert.ToString(mRole[DatabaseObjects.Columns.UserTypes]));
                    }
                    else
                    {
                        rolesTextArry.Add(r);
                    }
                }
                escalationToRoleCell.Text = string.Join("; ", rolesTextArry.ToArray());
            }

            //lnkShowdetail.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Configuration Variable - Show Item Detail','55','50',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath)));

        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindGridView();


            string moduleName = Convert.ToString(ddlModule.Value);
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=escalationrule&pageTitle=SLA Escalations&isdlg=1&isudlg=1&module=" + moduleName);
            Response.Redirect(url);
        }

        protected void cbEnableEscalation_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationVariableManager variable = new ConfigurationVariableManager(context);
            variable.Save(ConfigConstants.EnableEscalations, cbEnableEscalation.Checked.ToString());
            variable.RefreshCache();
        }

        protected void btReGenerateEscalation_Click(object sender, EventArgs e)
        {
            ProcessAllTicketEscalationHelper escHelper = new ProcessAllTicketEscalationHelper(context);
            if (!escHelper.ProcessState())
            {
                Thread escThread = new Thread(delegate ()
                {
                    escHelper.ReGenerateAllEscalations();
                });
                escThread.Start();

                btRefreshingEscalation.Visible = true;
                btReGenerateEscalation.Visible = false;
            }
            else
            {
                btRefreshingEscalation.Visible = true;
                btReGenerateEscalation.Visible = false;
            }
        }

        //To refresh email escalation,sla rule,module cache
        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(ddlModule.Value);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(Convert.ToString(ddlModule.Value), false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }
        }
    }
}
