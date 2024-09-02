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
using System.Web;
using System.Linq;
using System.Collections.Generic;
namespace uGovernIT.Web
{
    public partial class EmailNotificationView : UserControl
    {
        string addNetItem = string.Empty;
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        TaskEmailViewManager taskEmailViewManager = new TaskEmailViewManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            addNetItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=emailnotification");
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Email Notification - New Item','620','700',0,'{1}','true')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Email Notification - New Item','620','700',0,'{1}','true')", addNetItem, Server.UrlEncode(Request.Url.AbsolutePath)));

            dxShowDeleted.Checked = UGITUtility.StringToBoolean(ConfigManager.GetValueAsBool(ConfigConstants.SendEmail));

            BindModuleName();
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
                    dxddlModule.SelectedIndex = 0;
                }
            }

            BindGridView(Convert.ToString(dxddlModule.Value));
            base.OnLoad(e);
        }
        private void BindModuleName()
        {

            dxddlModule.Items.Clear();
            List<ModuleTaskEmail> taskEmailViewList = taskEmailViewManager.Load();
            List<UGITModule> ModuleList = ModuleManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();
            if (taskEmailViewList != null && ModuleList.Count > 0)
            {
                foreach (UGITModule moduleRow in ModuleList)
                {
                    var dr = taskEmailViewList.Where(x => x.ModuleNameLookup == moduleRow.ModuleName).ToList();
                    if (dr.Count() > 0)
                    {
                        dxddlModule.Items.Add(new ListEditItem { Text = moduleRow.Title, Value = moduleRow.ModuleName });
                    }
                }
            }
            dxddlModule.DataBind();
        }

        private void BindGridView(string moduleid)
        {
            //Fetch all roles of selected module
            // SPList spListUserEmail = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleUserTypes);
            //ModuleViewManager moduleViewManager = new ModuleViewManager();
            ModuleUserTypeManager moduleUserTypeManager = new ModuleUserTypeManager(HttpContext.Current.GetManagerContext());
            List<ModuleUserType> dtUserEmail = moduleUserTypeManager.Load(x => x.ModuleNameLookup.Equals(dxddlModule.Value));
            //DataTable dtUserEmail = spListUserEmail.Items.GetDataTable();
            if (dtUserEmail != null && dtUserEmail.Count() > 0)
            {
                ModuleUserType row = new ModuleUserType();
                row.UserTypes = "Escalation Manager";
                row.ColumnName = "EscalationManager";
                row.ModuleNameLookup = Convert.ToString(dxddlModule.Value);
                dtUserEmail.Add(row);
                row.UserTypes = "Backup Escalation Manager";
                row.ColumnName = DatabaseObjects.Columns.RequestTypeBackupEscalationManager;
                row.ModuleNameLookup = Convert.ToString(dxddlModule.Value);
                dtUserEmail.Add(row);
            }
            List<ModuleTaskEmail> taskEmail = taskEmailViewManager.Load(x => x.Deleted == false).Where(x => x.ModuleNameLookup.Equals(moduleid, StringComparison.CurrentCultureIgnoreCase)).ToList();//(x => x.ModuleNameLookup.Equals(moduleid, StringComparison.CurrentCultureIgnoreCase)).Where(x => x.Deleted == false);
            if (taskEmail != null && taskEmail.Count > 0)
            {
                LifeCycleManager lifecyclehelper = new Manager.LifeCycleManager(HttpContext.Current.GetManagerContext());
                PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(HttpContext.Current.GetManagerContext());
                List<LifeCycle> lifeCycleList = lifecyclehelper.LoadLifeCycleByModule(moduleid);
                List<ModulePrioirty> modulePrioirties = prioirtyViewManager.Load();
                ModulePrioirty modulePrioirty = null;
                //var newlst = from a in taskEmail
                //             join b in lst on a.StageStep equals Convert.ToInt32(b.ID) into v
                //             from d in v.DefaultIfEmpty()
                var query = from a in taskEmail
                            join b in lifeCycleList[0].Stages on a.StageStep equals Convert.ToInt32(b.ID)
                            into c
                            from d in c.DefaultIfEmpty()
                            from b in lifeCycleList[0].Stages.Where(x => d == null ? false : x.ID == d.ID).DefaultIfEmpty()
                            select new ModuleTaskEmail
                            {
                                ID = a.ID,
                                CustomProperties = a.CustomProperties,
                                EmailBody = a.EmailBody,
                                EmailIDCC = a.EmailIDCC,
                                EmailTitle = a.EmailTitle,
                                EmailUserTypes = a.EmailUserTypes,
                                HideFooter = a.HideFooter,
                                ModuleNameLookup = a.ModuleNameLookup,
                                StageStep = a.StageStep,
                                SendEvenIfStageSkipped = a.SendEvenIfStageSkipped,
                                Status = a.Status,
                                Title = a.Title,
                                Deleted = a.Deleted,
                                Attachments = a.Attachments,
                                TicketPriorityLookup = a.TicketPriorityLookup,
                                NotifyInPlainText = a.NotifyInPlainText,
                                EmailEventType = a.EmailEventType,
                                Stage = b == null ? string.Empty : Convert.ToString(b.StageStep) + "-" + b.StageTitle,
                                TenantID = a.TenantID
                            };
                              
                taskEmail = query.ToList();
                //SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, moduleid);
                //query.ViewFields = "<FieldRef Name='ID' />" +
                //                   "<FieldRef Name='TicketStatus' />" +
                //                   "<FieldRef Name='EmailTitle' />" +
                //                   "<FieldRef Name='ModuleStepLookup' />" +
                //                   "<FieldRef Name='EmailUserTypes' />";
                //query.ViewFieldsOnly = true;
                //SPListItemCollection itemCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.TaskEmails, query);

                taskEmail.ForEach(
                    x => {
                        if (x.TicketPriorityLookup != null)
                        {
                            modulePrioirty = modulePrioirties.Where(y => y.ID == x.TicketPriorityLookup).FirstOrDefault();
                            if (modulePrioirty != null)
                                x.Priority = modulePrioirty.Title;
                        }

                        if (!string.IsNullOrEmpty(x.EmailUserTypes))
                            x.EmailUserTypes = x.EmailUserTypes.Replace(Constants.Separator, Constants.UserInfoSeparator);
                    });

                dxgrid.DataSource = taskEmail;
                dxgrid.DataBind();
            }
        }
        protected void dxddlmodule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.Value);
            string url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=emailnotification&pageTitle=Email Notifications&isdlg=1&isudlg=1&module=" + moduleName);
            Response.Redirect(url);
        }
        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            ConfigurationVariable variable = ConfigManager.LoadVaribale(ConfigConstants.SendEmail);
            if (variable != null)
            {
                variable.KeyValue = dxShowDeleted.Checked.ToString();
                ConfigManager.Update(variable);
            }
            else
            {
                ConfigurationVariable variableNew = new ConfigurationVariable
                {
                    KeyName = ConfigConstants.SendEmail,
                    KeyValue = dxShowDeleted.Checked.ToString(),
                    Type = string.Empty,
                    CategoryName = "E-mail",
                    Description = "Enable/disable e-mail notifications",
                    Title = "SendEmail",

                };

                ConfigManager.Insert(variableNew);
            }
            //variable.Save();
            //uGITCache.RefreshList(DatabaseObjects.Tables.ConfigurationVariable);

        }
        protected void dxgrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit" || e.DataColumn.FieldName == DatabaseObjects.Columns.TicketStatus)
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string ticketStatus = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketStatus));
                string editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=emailnotification&ID=" + dataKeyValue + " ");
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','Email Notification - " + ticketStatus + "','620','700',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)dxgrid.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketStatus)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            string moduleName = Convert.ToString(dxddlModule.Value);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(moduleName, false);
            if (module != null)
            {
                Util.Cache.CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, context.TenantID, module);
            }
        }
    }
}
