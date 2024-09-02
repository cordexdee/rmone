using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AdministerCore : UserControl
    {
        ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        string defaultUser = ConfigurationManager.AppSettings["DefaultUser"];
        public string DisplayName = string.Empty;
        public string DisabledAdminModules = string.Empty;
        List<string> disableControl = new List<string>();
        UserProfileManager profileManager = null;
        public ApplicationContext appContext
        {
            get
            {
                return HttpContext.Current.GetManagerContext();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            profileManager = new UserProfileManager(appContext);
            DisabledAdminModules = ObjConfigVariable.GetValue(ConfigConstants.DisableAdminModules).Replace("\r", "").Replace("\n", "").Trim();
            DisplayName = ObjConfigVariable.GetValue("DisplayName");
            var data = DisabledAdminModules.Split(',');
            disableControl = data.ToList();
        }
        private bool sendEmail(string To, string userName, string companyName, string userEmail)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = appContext.CurrentUser.UserName;
            }
            if (string.IsNullOrEmpty(userEmail))
            {
                userEmail = appContext.CurrentUser.Email;
            }
            if (string.IsNullOrEmpty(companyName))
            {
                companyName = appContext.TenantAccountId;
            }
            //if (string.IsNullOrEmpty(To))
            //{
            //    To = profileManager.GetUserByBothUserNameandDisplayName(defaultUser).Email;
            //}
            MailMessenger mailMessage = new MailMessenger(appContext);
            var defaultTenantContext = ApplicationContext.Create();
            UserRoleManager uRole = new UserRoleManager(defaultTenantContext);
            var Role = uRole.GetRoleByName("UGITSuperAdmin");
            if (Role != null)
            {
                var users = defaultTenantContext.UserManager.GetUsersByGroupID(Role.Id);
                To = string.Join(";", users.Select(x => x.Email).ToList());
                string mailBody = $"{userName} of {companyName} {userEmail} needs assistance to enable admin features";
                var result = mailMessage.SendMail(To, "Need assistance to enable admin features", string.Empty, mailBody, true);
                if (!string.IsNullOrEmpty(result) && !result.Contains("FAILED"))
                    return true;
                else
                    return false;
            }
            return false;

        }
        //protected void template_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    TemplatesAdmin templatesAdmin = (TemplatesAdmin)Page.LoadControl("~/ControlTemplates/Admin/TemplatesAdmin.ascx");

        //    admincorePanel.Controls.Add(templatesAdmin);

        //}

        //protected void systemResource_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    SystemResource systemResource = (SystemResource)Page.LoadControl("~/ControlTemplates/Admin/SystemResource.ascx");

        //    admincorePanel.Controls.Add(systemResource);
        //}

        //protected void knowledge_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    Knowledge knowledge = (Knowledge)Page.LoadControl("~/ControlTemplates/Admin/Knowledge.ascx");

        //    admincorePanel.Controls.Add(knowledge);
        //}

        //protected void workflow_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    WorkFlowAdmin work = (WorkFlowAdmin)Page.LoadControl("~/ControlTemplates/Admin/WorkFlowAdmin.ascx");
        //    admincorePanel.Controls.Add(work);
        //}

        //protected void coreSetUp_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    InitialSetUp initialSetUp = (InitialSetUp)Page.LoadControl("~/ControlTemplates/Admin/InitialSetUp.ascx");
        //    admincorePanel.Controls.Add(initialSetUp);
        //}

        //protected void manageInterface_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    ManageInterfaceAdmin manageInterfaceAdmin = (ManageInterfaceAdmin)Page.LoadControl("~/ControlTemplates/Admin/ManageInterfaceAdmin.ascx");
        //    admincorePanel.Controls.Add(manageInterfaceAdmin);
        //}

        //protected void resourceConfig_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    ConfigureUserAdmin configureUserAdmin = (ConfigureUserAdmin)Page.LoadControl("~/ControlTemplates/Admin/ConfigureUserAdmin.ascx");
        //    admincorePanel.Controls.Add(configureUserAdmin);
        //}

        protected void btnBackButton_Click(object sender, EventArgs e)
        {
            divContainer.Visible = true;
            btnBackButton.Visible = false;
            backAdminBtn.Visible = false;
        }

        //protected void governance_Click(object sender, ImageClickEventArgs e)
        //{
        //    btnBackButton.Visible = true;
        //    divContainer.Visible = false;
        //    Governance governance = (Governance)Page.LoadControl("~/ControlTemplates/Admin/Governance.ascx");
        //    admincorePanel.Controls.Add(governance);

        //}

        protected void acoreSetUp_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("acoreSetUp"))
            {
                DataTable tenantData = GetTableDataManager.GetTenantDataUsingQueries($"select {DatabaseObjects.Columns.Subscription} from {DatabaseObjects.Tables.Tenant} where {DatabaseObjects.Columns.AccountId} = '{appContext.TenantAccountId}'");
                if (tenantData != null && tenantData.Rows.Count > 0)
                {
                    int subscription = UGITUtility.StringToInt(tenantData.Rows[0][0]);
                    if (subscription == 1 || subscription == 2)
                    {
                        btnBackButton.Visible = true;
                        backAdminBtn.Visible = true;

                        divContainer.Visible = false;
                        InitialSetUp initialSetUp = (InitialSetUp)Page.LoadControl("~/ControlTemplates/Admin/InitialSetUp.ascx");
                        admincorePanel.Controls.Add(initialSetUp);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('SERVICE PRIME SET-UP');", true);
                    }
                    else
                    {
                        Response.Redirect("~/SitePages/NewLoginWizard", true);
                    }
                }                
            }
            else
            {
                //send mail here 
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }

        protected void aworkflow_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("aworkflow"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                WorkFlowAdmin work = (WorkFlowAdmin)Page.LoadControl("~/ControlTemplates/Admin/WorkFlowAdmin.ascx");
                admincorePanel.Controls.Add(work);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('WorkFlows');", true);
            }
            else
            {
                //send mail here 
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }


        }

        protected void aKnowledge_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("aKnowledge"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                Knowledge knowledge = (Knowledge)Page.LoadControl("~/ControlTemplates/Admin/Knowledge.ascx");
                //SettingsMenuBar settingsMenuBar = (SettingsMenuBar)Page.LoadControl("~/ControlTemplates/Shared/SettingsMenuBar.ascx");
                //settingsMenuBar.Page.Title = "knowledgeTitle";



                //Label c = (Label)settingsMenuBar.FindControl("pageTitle");
                //c.Text = "knowledgeTitle";
                // settingsMenuBar.Page.Controls.

                //SettingsMenuBar settingsMenuBar1 = Page.FindControl("SettingsMenuBar.ascx");
                //settingsMenuBar1.FindControl("pageTitle");
                admincorePanel.Controls.Add(knowledge);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('Knowledge');", true);
            }
            else
            {
                //send mail here 
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }



        }

        protected void asystemResource_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("asystemResource"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                SystemResource systemResource = (SystemResource)Page.LoadControl("~/ControlTemplates/Admin/SystemResource.ascx");

                admincorePanel.Controls.Add(systemResource);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('System');", true);
            }
            else
            {
                //send mail here 
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }

        }

        protected void amanageInterface_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("amanageInterface"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                ManageInterfaceAdmin manageInterfaceAdmin = (ManageInterfaceAdmin)Page.LoadControl("~/ControlTemplates/Admin/ManageInterfaceAdmin.ascx");
                admincorePanel.Controls.Add(manageInterfaceAdmin);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('Interfaces');", true);
            }
            else
            {
                //send mail here 
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }


        }

        protected void aresourceConfig_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("aresourceConfig"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                ConfigureUserAdmin configureUserAdmin = (ConfigureUserAdmin)Page.LoadControl("~/ControlTemplates/Admin/ConfigureUserAdmin.ascx");
                admincorePanel.Controls.Add(configureUserAdmin);
            }
            else
            {
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }

        }

        protected void agovernance_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("agovernance"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                Governance governance = (Governance)Page.LoadControl("~/ControlTemplates/Admin/Governance.ascx");
                admincorePanel.Controls.Add(governance);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('Governance');", true);
            }
            else
            {
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }


        }

        protected void atemplate_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("atemplate"))
            {
                btnBackButton.Visible = true;
                backAdminBtn.Visible = true;
                divContainer.Visible = false;
                TemplatesAdmin templatesAdmin = (TemplatesAdmin)Page.LoadControl("~/ControlTemplates/Admin/TemplatesAdmin.ascx");
                admincorePanel.Controls.Add(templatesAdmin);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageTitle", "javascript:setPageTitleHeader('Templates');", true);
            }
            else
            {
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }

        protected void a2_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("a2"))
            {
            }
            else
            {
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }

        protected void serviceCatelogAndAgent_ServerClick(object sender, EventArgs e)
        {
            if (!disableControl.Contains("serviceCatelogAndAgent"))
            {
            }
            else
            {
                sendEmail(string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }
    }

}