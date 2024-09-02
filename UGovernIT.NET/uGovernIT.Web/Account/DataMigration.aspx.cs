using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.DataTransfer;
using uGovernIT.Manager;
using uGovernIT.Utility;
using Newtonsoft.Json.Linq;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.Account
{
    public partial class DataMigration : System.Web.UI.Page
    {
        private ApplicationContext _applicationContext;
        private TenantManager _tenantManager;
        List<string> lstConfiguration = new List<string> { "Location", "Departments", "FunctionalAreas", "Users","Roles", "Modules", "BudgetsCategory", "ConfigurationVariables", "DefaultEntries", "WikiCategories", "StageExitCriteriaTemplates", "DashboardAndQueries", "Attachments", "FactTables", "QuickTickets", "AssetVendors", "AssetModels", "MessageBoard", "UserSkills", "ServiceCatalogAndAgents",
                        "ACRTypes", "DRQRapidTypes", "DRQSystemAreas", "Environment", "SubLocation", "ProjectLifecycles", "ProjectInitiative", "ProjectClass", "ProjectStandards", "GlobalRoles", "LandingPages", "JobTitle", "EmployeeTypes", "uGovernITLogs", "GovernanceConfiguration", "ModuleMonitors", "ModuleMonitorOptions", "ProjectComplexity", "MailTokenColumnName", "GenericStatus", "LinkViews", "TenantScheduler", "SurveyFeedback", "Phrases", "Widgets", "DocumentManagement", "ChecklistTemplates", "LeadCriteria", "RankingCriteria","Studio","State", "ResourceWorkItems", "TicketHours", "ResourceTimeSheet", "ResourceAllocation", "ResourceAllocationMonthly", "ProjectEstimatedAllocation","ProjectPlannedAllocation", "ResourceUsageSummaryWeekWise", "ResourceUsageSummaryMonthWise" };

        List<string> lstModules = new List<string> { "TSR", "ACR", "RCA",  "DRQ", "INC", "BTS", "APP", "TSK", "SVC", "WIKI", "NPR", "PMM", "CMDB", "HLP", "CMT", "COM", "CON", "LEM", "OPM", "CPR", "CNS", "PRS","ITG" ,"RMM"};
        StringBuilder sb = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSourceTenants();
            }
            BindConfigCheckBoxes();
            BindModulesCheckBoxes();
        }

        private void BindSourceTenants()
        {
            _applicationContext = ApplicationContext.Create();
            _tenantManager = new TenantManager(_applicationContext);
            var tenants = _tenantManager.GetTenantList().OrderBy(x => x.AccountID).ToList();
            if (tenants != null)
            {
                ddlSourceTenants.DataValueField = "AccountID";
                ddlSourceTenants.DataTextField = "AccountID";
                ddlSourceTenants.DataSource = tenants;
                ddlSourceTenants.DataBind();
            }
        }

        private void BindConfigCheckBoxes()
        {
            sb.Clear();
            HtmlGenericControl configDiv = new HtmlGenericControl("div");
            configDiv.Attributes.Add("class", "form-group");

            sb.Append("<div class=\"row marginTop-15\">");
            sb.Append("<div class=\"col-md-12 col-sm-12 col-xs-12 chkBox\">");
            sb.Append("<input type = \"checkbox\" id=\"chkSelectAllConfig\" />");
            sb.Append("<label for=\"chkSelectAllConfig\">Select All</label>");
            sb.Append("</div>");
            sb.Append("</div>");
            for (int i = 0; i < lstConfiguration.Count; i += 3)
            {
                sb.Append("<div class=\"row marginTop-15\">");
                for (int j = 0; j < 3; j++)
                {
                    if ((i + j) < lstConfiguration.Count)
                    {
                        sb.Append("<div class=\"col-md-4 col-sm-6 col-xs-12 chkBox\">");
                        sb.Append("<input type = \"checkbox\" id=\"" + lstConfiguration[i + j] + "\" value = \"" + lstConfiguration[i + j] + "\" name = \"chkGrpConfig\" />");
                        sb.Append("<label for=\"" + lstConfiguration[i + j] + "\">" + lstConfiguration[i + j] + "</label>");
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");
            }

            configDiv.InnerHtml = Convert.ToString(sb);
            pnlConfig.Controls.Add(configDiv);
        }

        private void BindModulesCheckBoxes()
        {
            sb.Clear();
            HtmlGenericControl configDiv = new HtmlGenericControl("div");
            configDiv.Attributes.Add("class", "form-group");

            sb.Append("<div class=\"row marginTop-15\">");
            sb.Append("<div class=\"col-md-12 col-sm-12 col-xs-12 chkBox\">");
            sb.Append("<input type = \"checkbox\" id=\"chkSelectAllModules\" />");
            sb.Append("<label for=\"chkSelectAllModules\">Select All</label>");
            sb.Append("</div>");
            sb.Append("</div>");
            for (int i = 0; i < lstModules.Count; i += 3)
            {
                sb.Append("<div class=\"row marginTop-15\">");
                for (int j = 0; j < 3; j++)
                {
                    if ((i + j) < lstModules.Count)
                    {
                        sb.Append("<div class=\"col-md-4 col-sm-6 col-xs-12 chkBox\">");
                        sb.Append("<input type = \"checkbox\" id=\"" + lstModules[i + j] + "\"  value = \"" + lstModules[i + j] + "\"  name = \"chkGrpModules\" />");
                        sb.Append("<label for=\"" + lstModules[i + j] + "\">" + lstModules[i + j] + "</label>");
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");
            }

            configDiv.InnerHtml = Convert.ToString(sb);
            pnlModules.Controls.Add(configDiv);
        }

        protected void btnMigrateFromForm_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedConfig = Request.Form["chkGrpConfig"];
                string selectedModules = Request.Form["chkGrpModules"];

                if (string.IsNullOrEmpty(selectedConfig) && string.IsNullOrEmpty(selectedModules))
                    return;

                DMTenant sourceTenant = new DMTenant();
                sourceTenant.tenantid = ddlSourceTenants.SelectedValue;
                sourceTenant.tenantname = ddlSourceTenants.SelectedValue;
                sourceTenant.dbconnection = txtSourceDbConnString.Text;
                sourceTenant.commondbconnection = txtSourceTenantDbConnString.Text;

                DMTenant targetTenant = new DMTenant();
                targetTenant.tenantid = txtTargetTenant.Text;
                targetTenant.tenantname = txtTargetTenant.Text;
                targetTenant.dbconnection = txtTargetDbConnString.Text;
                targetTenant.commondbconnection = txtTargetTenantDbConnString.Text;
                targetTenant.username = $"Administrator_{txtUsername.Text}";
                targetTenant.password = txtPassword.Text;
                targetTenant.userEmail = txtUserEmail.Text;
                targetTenant.TenantEmail = txtUserEmail.Text;
                ImportManager manager = null;
                if (ddlMigrationOptions.SelectedValue == "std")
                {
                    DMTenant spSource = new DMTenant();
                    spSource.url = txtSPUrl.Text.Trim();
                    spSource.username = txtSPUserName.Text.Trim();
                    spSource.password = txtSPPassword.Text.Trim();
                    spSource.domain = txtSPDomain.Text.Trim();
                    manager = new ImportManager(selectedConfig, selectedModules, sourceTenant, targetTenant, spSource);
                }
                else
                    manager = new ImportManager(selectedConfig, selectedModules, sourceTenant, targetTenant);
                manager.Excute(ddlMigrationOptions.SelectedValue);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex, "Exception in Data Migration from Form", "Error");
            }
        }

        protected void btnMigrateFromFile_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            try
            {
                if (DMFileUpload.HasFile)
                {
                    path = "/Content/FileUploads/";
                    if (!Directory.Exists(Server.MapPath(path)))
                        Directory.CreateDirectory(Server.MapPath(Path.GetDirectoryName(path)));

                    path = $"{path}{Guid.NewGuid().ToString()}_{DMFileUpload.FileName}";
                    DMFileUpload.SaveAs(Server.MapPath(path));
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + path);
                }

                ImportManager manager = new ImportManager(path);
                manager.Excute(ddlMigrationOptionsFileUpload.SelectedValue);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex, "Exception in Data Migration from uploaded File", "Error");
            }
            finally
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
        }
    }
}