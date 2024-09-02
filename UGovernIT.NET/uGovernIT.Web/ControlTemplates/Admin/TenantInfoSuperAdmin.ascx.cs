using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Helpers;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility.Entities.Common;
using Microsoft.AspNet.Identity.Owin;
using System.IdentityModel.Claims;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class TenantInfoSuperAdmin : UserControl
    {
        UserProfile currentUser;
        UserProfileManager userManager;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;
        private UGITTaskManager _taskManager = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TenantManager _tenantManager = null;
        private UserProfileManager _userProfileManager = null;
        private ApplicationContext _context = null;

        public string statusred = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=status"));

        ModuleStatisticRequest mRequest = new ModuleStatisticRequest();

        protected Ticket ticketRequest;

        //public TenantInfoSuperAdmin()
        //{
        //    _context = ApplicationContext.Create();

        //    _tenantManager = new TenantManager(_context);
        //}

        ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

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

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                }
                return _ticketManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _moduleViewManager;
            }
        }

        protected UGITTaskManager UGITTaskManager
        {
            get
            {
                if (_taskManager == null)
                {
                    _taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                }
                return _taskManager;
            }

        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                }
                return _fieldConfigurationManager;
            }
        }

        protected UserProfileManager userProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                }
                return _userProfileManager;
            }

        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                }
                return _configurationVariableManager;
            }
        }

        protected TenantManager TenantManager
        {
            get
            {
                if (_tenantManager == null)
                {
                    _tenantManager = new TenantManager(HttpContext.Current.GetManagerContext());
                }
                return _tenantManager;
            }
        }
        public TenantOnBoardingHelper tenantOnBoardingHelper = null;
        public TenantConstraints tenantConstraints = new TenantConstraints();
        public DataTable GetAllTenant;


        protected override void OnPreRender(EventArgs e)
        {      
            ViewState["DeleteTenant"] = Session["DeleteTenant"];
            ViewState["ResetPassword"] = Session["ResetPassword"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = HttpContext.Current.CurrentUser();
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var isSuperAdmin = userManager.IsUGITSuperAdmin(currentUser);
            if (isSuperAdmin)
            {
                BindTenantConstraints();
                BindTenantRegistration();

                if (!IsPostBack)
                {
                    ViewState["DeleteTenant"] = Server.UrlEncode(DateTime.Now.ToString());
                    Session["DeleteTenant"] = ViewState["DeleteTenant"];

                    ViewState["ResetPassword"] = Server.UrlEncode(DateTime.Now.ToString());
                    Session["ResetPassword"] = ViewState["ResetPassword"];
                }
            }
            else
            {
                Response.Redirect("~/SuperAdmin/SuperAdmin.aspx", false);
            }
        }

        protected void deleteTenant_Init(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            btn.ID = "deleteTenant_" + Convert.ToString(container.Grid.GetRowValues(container.VisibleIndex, "TenantID")).Replace("-", "");
        }


        protected void resetPassword_Init(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            btn.ID = "resetPassword_" + Convert.ToString(container.Grid.GetRowValues(container.VisibleIndex, "TenantID")).Replace("-", "");
        }
        #region TenantConstraints

        private void BindTenantConstraints()
        {
            var tenantsCount = GetTableDataManager.GetTenantDataUsingQueries($"select Count(*) from {DatabaseObjects.Tables.Tenant}");
            GetAllTenant = GetTableDataManager.GetTenantDataUsingQueries($"GetUsageStatistics");

            if (GetAllTenant != null)
            {
                ApplicationContext applicationContext = ApplicationContext.Create();
                tenantOnBoardingHelper = new TenantOnBoardingHelper(applicationContext);
                tenantConstraints = tenantOnBoardingHelper.getTenantConstraints();
                lblTenantCreatedCount.Text = string.Format("{0:00}", GetAllTenant.Rows.Count); ;

                lblTenantCreatedCount.Attributes.Add("class", "normal-constraints");
                //hdTenantCreadted.Attributes.Add("class", "normal-constraints");
                if (GetAllTenant.Rows.Count >= tenantConstraints.TenantCountHigh && GetAllTenant.Rows.Count < tenantConstraints.TenantCountCritical)
                {
                    lblTenantCreatedCount.Attributes.Add("class", "high-constraints");
                    //hdTenantCreadted.Attributes.Add("class", "high-constraints");

                }
                if (GetAllTenant.Rows.Count >= tenantConstraints.TenantCountCritical)
                {
                    lblTenantCreatedCount.Attributes.Add("class", "critical-constraints");
                    //hdTenantCreadted.Attributes.Add("class", "critical-constraints");
                }
            }            
        }

        #endregion

        private DataTable TenantRegistrationInfo()
        {
            var dt = new DataTable();
            dt.Columns.Add("Trials", typeof(string));
            dt.Columns.Add("CompanyName", typeof(string));
            dt.Columns.Add("AccountId", typeof(string));
            dt.Columns.Add("Date Requested", typeof(DateTime));
            dt.Columns.Add("Approval By", typeof(string));
            dt.Columns.Add("Approved Date", typeof(string));//At date of trail completion of user and be the actual customer of this product
            dt.Columns.Add("Current Step", typeof(string));
            dt.Columns.Add("TicketId", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("EnableOnboarding", typeof(Boolean));
            dt.Columns.Add("IsSelfRegistration", typeof(string));
            dt.Columns.Add("TenantID", typeof(string));
            dt.Columns.Add("Deleted", typeof(string));
            dt.Columns.Add("IsTenantExist", typeof(string));
            dt.Columns.Add("IsSubscription", typeof(Boolean));
            dt.Columns.Add("TicketCount", typeof(string));
            dt.Columns.Add("ServiceCount", typeof(string));
            dt.Columns.Add("Class_TicketCount", typeof(string));
            dt.Columns.Add("Class_ServiceCount", typeof(string));
            var applicationContext = ApplicationContext.Create();

            // TenantManager tenantManager = new TenantManager(applicationContext);

            List<String> SvcTicketIdCompanyName = new List<string>();

            var tenants = TenantManager.GetTenantList();
            //var selfRegtenantList = tenants.Where(x => x.SelfRegisteredTenant == true && !x.Deleted);
            var RegtenantList = tenants.Where(x => x.Deleted != true).ToList();

            //UGITModule svcModule = moduleViewManager.LoadByName("SVC");

            //var svcOpenTicket = TicketManager.GetOpenTickets(svcModule);

            DataTable NonSelfRegisteredSvcTicket = new DataTable();

            NonSelfRegisteredSvcTicket = SvcTicketRequest(tenants);

            XmlDocument doc = new XmlDocument();
            if (NonSelfRegisteredSvcTicket != null)
            {
                foreach (DataRow data in NonSelfRegisteredSvcTicket.Rows)
                {
                    DataRow dr = dt.NewRow();
                    try
                    {
                        string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(data, DatabaseObjects.Columns.UserQuestionSummary));

                        doc.LoadXml(questionInputs.Trim());

                        var inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                        var nameOfCustomer = inputObj.ServiceSections[0].Questions.Where(x => x.Token == "name").Select(x => x.Value).FirstOrDefault();

                        string companyname = inputObj.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;
                        var email = inputObj.ServiceSections[0].Questions.Where(x => x.Token == "email").Select(x => x.Value).FirstOrDefault();

                        var dateRequest = Convert.ToDateTime(data[DatabaseObjects.Columns.TicketCreationDate]);

                        var tenantTask = UGITTaskManager.LoadByProjectID("SVC", Convert.ToString(data[DatabaseObjects.Columns.TicketId]));

                        if (tenantTask != null && tenantTask.Count > 0)
                        {
                            var itemOrder = tenantTask.OrderByDescending(x => x.ID).Select(x => x.ItemOrder).FirstOrDefault();

                            foreach (var task in tenantTask)
                            {
                                if (task.ItemOrder == itemOrder && task.Status == "Completed")
                                {
                                    dr["Approved Date"] = task.CompletionDate.ToShortDateString();
                                }
                            }
                        }
                        if (Convert.ToBoolean(data["IsTenantExist"]))
                        {
                            Guid tenantGUID;
                            Guid.TryParse(data["TenantId"].ToString(), out tenantGUID);
                            var row = GetAllTenant.AsEnumerable().Where(x => x.Field<Guid>("TenantID") == tenantGUID).CopyToDataTable();
                            TenantStatistics tenantStatistics = new TenantStatistics();
                            tenantStatistics = tenantOnBoardingHelper.GetTicketOrServiceCountAndClass(row, tenantConstraints);
                            dr["TicketCount"] = string.Format("{0:00}", tenantStatistics.TicketCount);
                            dr["ServiceCount"] = string.Format("{0:00}", tenantStatistics.ServiceCount);
                            dr["Class_TicketCount"] = tenantStatistics.TicketClass;
                            dr["Class_ServiceCount"] = tenantStatistics.ServiceClass;

                        }

                        dr["Date Requested"] = dateRequest.ToShortDateString();
                        dr["Trials"] = nameOfCustomer;
                        dr["CompanyName"] = companyname;
                        dr["AccountId"] = companyname;
                        dr["Approval By"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.Approver, Convert.ToString(data[DatabaseObjects.Columns.Approver]));
                        //dr["Current Step"] = "~/Content/Images/NewAdmin/services.png";
                        dr["TicketId"] = data[DatabaseObjects.Columns.TicketId];
                        dr["Email"] = email;
                        dr["EnableOnboarding"] = true;
                        
                        dr["IsSelfRegistration"] = "e-Governance";
                        dr["TenantID"] = data[DatabaseObjects.Columns.TenantID];
                        dr["Deleted"] = data[DatabaseObjects.Columns.Deleted];
                        dr["IsSubscription"] = data["IsSubscription"];
                        dr["IsTenantExist"] = data["IsTenantExist"];

                        dt.Rows.Add(dr);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                }
            }

            foreach (Tenant tenant in RegtenantList)
            {
                DataRow dr = dt.NewRow();
                UserProfile user = HttpContext.Current.CurrentUser();
                //Guid tenantGUID;
                //Guid.TryParse(TenantID, out tenantGUID);
                var row = GetAllTenant.AsEnumerable().Where(x => x.Field<Guid>("TenantID") == tenant.TenantID).CopyToDataTable();
                TenantStatistics tenantStatistics = new TenantStatistics();
                tenantStatistics = tenantOnBoardingHelper.GetTicketOrServiceCountAndClass(row, tenantConstraints);
                dr["TicketCount"] = string.Format("{0:00}", tenantStatistics.TicketCount);
                dr["ServiceCount"] = string.Format("{0:00}", tenantStatistics.ServiceCount);
                dr["Class_TicketCount"] = tenantStatistics.TicketClass;
                dr["Class_ServiceCount"] = tenantStatistics.ServiceClass;
                dr["Date Requested"] = tenant.Created.ToShortDateString();
                dr["Trials"] = tenant.Name;
                dr["CompanyName"] = tenant.TenantName;
                dr["Approval By"] = user.Name;
                dr["Approved Date"] = tenant.Created.ToShortDateString();
                //dr["Current Step"] = "~/Content/Images/NewAdmin/services.png";
                dr["TicketId"] = "no-ticket-id";
                dr["Email"] = tenant.Email;
                dr["EnableOnboarding"] = false;
                dr["AccountId"] = tenant.AccountID;
                if (tenant.SelfRegisteredTenant.HasValue && tenant.SelfRegisteredTenant.Value == true)
                    dr["IsSelfRegistration"] = "Self Registered";
                else
                    dr["IsSelfRegistration"] = "Manual";

                dr["TenantID"] = tenant.TenantID;
                dr["Deleted"] = tenant.Deleted;

                if (tenant.Subscription == 0 || tenant.Subscription == null)
                {
                    dr["IsSubscription"] = false;
                }
                else
                {
                    dr["IsSubscription"] = true;
                }
                dr["IsTenantExist"] = true;
                dt.Rows.Add(dr);
            }

            //foreach (Tenant tenant in AlltenantList)
            //{
            //    DataRow dr = dt.NewRow();
            //    var result = svcOpenTicket.Select($"{DatabaseObjects.Columns.Title} like '%Tenant On-boarding for: {tenant.TenantName}%'").FirstOrDefault();
            //    if(result == null || tenant.SelfRegisteredTenant == true)
            //    {
            //        UserProfile user = HttpContext.Current.CurrentUser();
            //        dr["Date Requested"] = tenant.Created.ToShortDateString();
            //        dr["Trials"] = tenant.TenantName;
            //        dr["Approval By"] = user.Name;
            //        dr["Approved Date"] = tenant.Created.ToShortDateString();
            //        //dr["Current Step"] = "~/Content/Images/NewAdmin/services.png";
            //        dr["TicketId"] = "no-ticket-id";
            //        dr["Email"] = tenant.Email;
            //        dr["EnableOnboarding"] = false;
            //        dr["IsSelfRegistration"] = "Self Registered";
            //        dt.Rows.Add(dr);

            //    }
            //    else
            //    {
            //        string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(result, DatabaseObjects.Columns.UserQuestionSummary));

            //        doc.LoadXml(questionInputs.Trim());
            //        var inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
            //        var nameOfCustomer = inputObj.ServiceSections[0].Questions.Where(x => x.Token == "name").Select(x => x.Value).FirstOrDefault();
            //        var email = inputObj.ServiceSections[0].Questions.Where(x => x.Token == "email").Select(x => x.Value).FirstOrDefault();
            //        var dateRequest = Convert.ToDateTime(result[DatabaseObjects.Columns.TicketCreationDate]);

            //        var tenantTask = UGITTaskManager.LoadByProjectID("SVC", Convert.ToString(result[DatabaseObjects.Columns.TicketId]));


            //        if (tenantTask != null && tenantTask.Count > 0)
            //        {
            //            var itemOrder = tenantTask.OrderByDescending(x => x.ID).Select(x => x.ItemOrder).FirstOrDefault();
            //            // var itemOrder = from t in tenantTask orderby t.ID descending select t.ItemOrder;

            //            foreach (var task in tenantTask)
            //            {
            //                if (task.ItemOrder == itemOrder && task.Status == "Completed")
            //                {
            //                    dr["Approved Date"] = task.CompletionDate.ToShortDateString();

            //                }
            //            }
            //        }

            //        dr["Date Requested"] = dateRequest.ToShortDateString();
            //        dr["Trials"] = tenant.TenantName;
            //        dr["Approval By"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.Approver, Convert.ToString(result[DatabaseObjects.Columns.Approver]));
            //        //dr["Current Step"] = "~/Content/Images/NewAdmin/services.png";
            //        dr["TicketId"] = result[DatabaseObjects.Columns.TicketId];
            //        dr["Email"] = email;
            //        dr["EnableOnboarding"] = true;
            //        dr["IsSelfRegistration"] = "e-Governance";
            //        dt.Rows.Add(dr);

            //    }

            //}

            return dt;
        }

        private string GetRegistrationUrl(string TicketId, string EmailAddress)
        {
            string tid = QueryString.Encode($"{TicketId}&{EmailAddress}");
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/index?tid={tid}";
            return SiteUrl;
        }

        protected void onBoardingWorkFlow_Click(object sender, ImageClickEventArgs e)
        {
            string data = ((ImageButton)sender).CommandArgument;
            string[] arr = new string[2];
            if (!string.IsNullOrEmpty(data))
            {
                arr = data.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
            }

            string url = GetRegistrationUrl(arr[0], arr[1]);
            Response.Redirect(url, false);
            //Response.Redirect("/applicationregistrationrequest/Index");
        }

        protected void DeleteTenant_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["DeleteTenant"]) == Convert.ToString(ViewState["DeleteTenant"]))
                {
                    var result = new MessageResult();

                    var argument = ((ImageButton)sender).CommandArgument;

                    var tenantParams = argument.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);

                    if (Convert.ToString(tenantParams[2]) == "e-Governance")
                    {
                        var ticketId = tenantParams[1];
                        var eGovernanceTicket = SvcTicketRequest().AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == Convert.ToString(ticketId));

                        if (eGovernanceTicket != null)
                        {
                            var isTenantExist = tenantParams[3];
                            if (isTenantExist.ToLower() == "true")
                            {
                                result = DeleteTenantData(tenantParams);
                            }
                            else
                            {
                                result.Status = true;
                            }

                            if (result.Status)
                            {
                                //string query = $"update SVCRequests set Deleted = 1  where TicketId = '{eGovernanceTicket[DatabaseObjects.Columns.TicketId]}' and TenantID = '{eGovernanceTicket[DatabaseObjects.Columns.TenantID]}'";
                                //var success = GetTableDataManager.ExecuteNonQuery(query);

                                UGITModule svcModule = moduleViewManager.LoadByName("SVC");
                                Ticket baseTicket = new Ticket(ApplicationContext, svcModule.ModuleName);
                                DataRow dr = Ticket.GetCurrentTicket(ApplicationContext, svcModule.ModuleName, Convert.ToString(eGovernanceTicket[DatabaseObjects.Columns.TicketId])); //TicketManager.GetByTicketID(svcModule, Convert.ToString(eGovernanceTicket[DatabaseObjects.Columns.TicketId]));
                                dr[DatabaseObjects.Columns.Deleted] = true;
                                string success = baseTicket.CommitChanges(dr);
                                baseTicket.UpdateTicketCache(dr, svcModule.ModuleName);

                                if (string.IsNullOrEmpty(success))
                                {
                                    result.AddInfo("Client deleted successfully");
                                }
                                else
                                {
                                    result.AddError("Failed to update service request data");
                                }
                            }
                        }
                    }
                    else
                    {
                        result = DeleteTenantData(tenantParams);
                    }

                    CacheHelper<object>.Delete(string.Format("Available_Tenants"));

                    BindTenantConstraints();
                    BindTenantRegistration();

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "TenantDeletionMessage", "alert('" + result.GetMessage() + "')", true);

                    Session["DeleteTenant"] = Server.UrlEncode(DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex, MessageCategory.TenantDeletion);
            }
        }

        protected void reset_Password_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["ResetPassword"]) == Convert.ToString(ViewState["ResetPassword"]))
                {
                    var result = new MessageResult();
                    String userId = string.Empty;
                    String UserName = string.Empty;
                    String email = string.Empty;
                    var argument = ((ImageButton)sender).CommandArgument;

                    var tenantParams = argument.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                    UserProfileManager umanager = umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                    UserProfileManager userProfileManager = new UserProfileManager(ApplicationContext);
                    Tenant tenant = TenantManager.GetTenantById(tenantParams[0]);

                    string Query = $"Select * from AspNetUsers  where IsDefaultAdmin =  1 and enabled = 1 and TenantID = '{tenantParams[0]}'";
                    DataTable userData = GetTableDataManager.ExecuteQuery(Query);
                    if(userData != null && userData.Rows.Count > 0)
                    {
                        userId = userData.Rows[0]["Id"].ToString();
                        email = userData.Rows[0]["Email"].ToString();
                    }
                    else
                    {
                        UserName = "Administrator_" + tenant.AccountID;
                        UserProfile user = umanager.FindByName(UserName, tenantParams[0]);
                        email = user.Email;
                        userId = user.Id;
                    }
                    
                    Dictionary<string,string>  resultForPasswordReset  =  userProfileManager.resetPasswordForDefaultAdmin(userId, umanager);
                    if(resultForPasswordReset["isPasswortReset"] == "True")
                    {
                        result.AddInfo("Password successfully reset");
                        //Util.Cache.CacheHelper<object>.Clear();
                        BindTenantConstraints();
                        BindTenantRegistration();
                        EmailHelper emailHelper = new EmailHelper(ApplicationContext);

                        emailHelper.SendMailToUserAboutForgetPassword(tenantParams[0], tenant.AccountID, tenant.Name, UserName, resultForPasswordReset["NewPassword"], email);
                    }
                    else
                    {
                        result.AddInfo("Password reset failed");
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Reset Password", "alert('" + result.GetMessage() + "')", true);

                    Session["ResetPassword"] = Server.UrlEncode(DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex, MessageCategory.ResetPassword);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Reset Password", "alert('" + "Password reset failed" + "')", true);
            }
        }

        private DataTable SvcTicketRequest(List<Tenant> tenants = null)
        {
            if (tenants == null)
            {
                tenants = TenantManager.GetTenantList();
            }

            var activeTenants = tenants.Where(x => !x.Deleted).ToList();

            var uGoveranceTenants = activeTenants.Where(x => x.SelfRegisteredTenant == false || x.SelfRegisteredTenant == null).ToList();
            var SelfRegtenantList = tenants.Where(x => x.SelfRegisteredTenant == true && !x.Deleted);

            UGITModule svcModule = moduleViewManager.LoadByName("SVC");
            var svcOpenTicket = TicketManager.GetOpenTickets(svcModule);

            DataTable NonSelfRegisteredSvcTicket = new DataTable();

            if (svcOpenTicket != null && svcOpenTicket.Rows.Count > 0)
            {
                NonSelfRegisteredSvcTicket = svcOpenTicket.Clone();
                NonSelfRegisteredSvcTicket.Columns.Add("IsTenantExist", typeof(string));
                NonSelfRegisteredSvcTicket.Columns.Add("IsSubscription", typeof(string));

                var result = svcOpenTicket.Select($"{DatabaseObjects.Columns.Title} like '%Tenant On-boarding for:%'");

                foreach (var data in result)
                {
                    var matchSVC = SelfRegtenantList.Where(x => x.TenantName == data[DatabaseObjects.Columns.Title].ToString().Split(':').Last().Trim()).ToList();
                    bool IsTenantExist = false;

                    if (matchSVC == null || matchSVC.Count == 0)
                    {
                        var tenant = uGoveranceTenants.FirstOrDefault(x => x.TenantName == data[DatabaseObjects.Columns.Title].ToString().Split(':').Last().Trim());
                        if (tenant != null)
                        {
                            data["TenantID"] = tenant.TenantID;
                            IsTenantExist = true;
                        }

                        DataRow dr = NonSelfRegisteredSvcTicket.NewRow();
                        dr = data;
                        if (!dr.Table.Columns.Contains("IsTenantExist") && !dr.Table.Columns.Contains("IsSubscription"))
                        {
                            dr.Table.Columns.Add("IsTenantExist", typeof(string));
                            dr.Table.Columns.Add("IsSubscription", typeof(string));
                        }

                        if (IsTenantExist)
                        {
                            dr["IsTenantExist"] = true;
                            if (tenant.Subscription == 0 || tenant.Subscription == null)
                            {
                                dr["IsSubscription"] = false;
                            }
                            else
                            {
                                dr["IsSubscription"] = true;
                            }

                        }
                        else
                        {
                            dr["IsTenantExist"] = false;
                            dr["IsSubscription"] = false;

                        }
                        NonSelfRegisteredSvcTicket.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        //dont include svc ticket for self registered tenant 
                    }
                }
            }

            return NonSelfRegisteredSvcTicket;
        }

        private void BindTenantRegistration()
        {
            DataTable tenantinfo = TenantRegistrationInfo().AsEnumerable().Where(x => !UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.Deleted])).CopyToDataTable();

            tenantinfo.DefaultView.Sort = "Date Requested desc";
            tenantinfo = tenantinfo.DefaultView.ToTable();

            superAdminGrid.DataSource = tenantinfo;
            superAdminGrid.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            superAdminGrid.SettingsFilterControl.ViewMode = FilterControlViewMode.VisualAndText;
            superAdminGrid.SettingsFilterControl.AllowHierarchicalColumns = false;
            superAdminGrid.SettingsFilterControl.ShowAllDataSourceColumns = true;
            superAdminGrid.SettingsFilterControl.HierarchicalColumnPopupHeight = 200;

            superAdminGrid.DataBind();
        }

        private MessageResult DeleteTenantData(string[] paramIds)
        {
            var result = new MessageResult();
            var message = "";
            string accountId = string.Empty;
            try
            {
                string tenantId = paramIds[0];
                var tenant = TenantManager.GetTenantById(tenantId);

                string DefaultTenant = Convert.ToString(ConfigurationManager.AppSettings["DefaultTenant"]);
                if (tenant != null && tenant.AccountID.EqualsIgnoreCase(DefaultTenant))
                {
                    message = "Default Tenant cannot be deleted.";
                    result.AddError(message);
                    return result;
                }
                
                if (tenant != null)
                {
                    //Get Database Backup
                    string filePath = Convert.ToString(ConfigurationManager.AppSettings["DatabaseBackupPath"]);
                    accountId = tenant.AccountID;

                    if (string.IsNullOrEmpty(filePath) || !Directory.Exists(filePath))
                    {
                        message = "Database backup directory does not exists";
                        result.AddError(message);

                        Util.Log.ULog.WriteException(message, MessageCategory.TenantDeletion);
                        return result;
                    }

                    // create database backup before tenant deletion
                    var status = ConfigurationVariableManager.CreateDatabaseBackup(filePath, accountId);

                    if (status)
                    {
                        // delete tenant data
                        status = ConfigurationVariableManager.DeleteTenantData(tenantId);

                        if (status)
                        {
                            // delete tenant from master database
                            status = ConfigurationVariableManager.DeleteTenant(tenantId);

                            if (status)
                            {
                                message = $"{accountId} successfully deleted"; //"Client deleted successfully";
                                result.Status = true;
                                result.AddInfo(message);

                                Util.Log.ULog.WriteLog($"{message} for TenantId : {tenantId}, AccountId : {accountId}", MessageCategory.TenantDeletion);
                            }
                            else
                            {
                                message = "Failed to delete client";
                                result.AddError(message);

                                Util.Log.ULog.WriteException($"{message} for TenantId : {tenantId}, AccountId : {accountId}", MessageCategory.TenantDeletion);
                            }
                        }
                        else
                        {
                            message = "Failed to delete client data";
                            result.AddError(message);

                            Util.Log.ULog.WriteException($"{message} for TenantId : {tenantId}, AccountId : {accountId}", MessageCategory.TenantDeletion);
                        }                   
                    }
                    else
                    {
                        message = "Failed to process database backup";
                        result.AddError(message);

                        Util.Log.ULog.WriteException($"{message} for TenantId : {tenantId}, AccountId : {accountId}", MessageCategory.TenantDeletion);
                    }
                }
                else
                {
                    message = "Client does not exists";
                    result.AddError(message);

                    Util.Log.ULog.WriteException($"{message} for TenantId : {tenantId}, AccountId : {accountId}", MessageCategory.TenantDeletion);
                }
            }
            catch (Exception ex)
            {
                message = "Failed to delete client data";
                result.AddError(message);

                Util.Log.ULog.WriteException(ex, MessageCategory.TenantDeletion);
            }
            finally
            {
                Util.Log.ULog.WriteUGITLog(ApplicationContext.CurrentUser.Id, $"{message} for TenantId : {paramIds[0]}, AccountId : {accountId}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), ApplicationContext.TenantID);
            }

            return result;
        }

        protected void superAdminGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Actions")
            {
                int index = e.VisibleIndex;

                DataRow currentRow = superAdminGrid.GetDataRow(e.VisibleIndex);
                if (currentRow != null)
                {
                    string tenantId = currentRow["TenantId"].ToString();
                    string statusredlocal = statusred + "&tenantId=" + tenantId;
                    string script = string.Format("JavaScript:UgitOpenPopupDialog('{0}','','Client Profile','625px','588px','','')", statusredlocal);
                    HtmlAnchor aHtml = (HtmlAnchor)superAdminGrid.FindRowCellTemplateControl(index, e.DataColumn, "stat");
                    aHtml.Attributes.Add("href", script);
                }
            }
        }

        protected void loginToCompany_Click(object sender, EventArgs e)
        {
            var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            var argument = ((LinkButton)sender).CommandArgument;
            var tenantParams = argument.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);

            if( !string.IsNullOrEmpty(tenantParams[0]) && tenantParams[3].ToString() == "True")
            {
                string ParentUserId = HttpContext.Current.CurrentUser().Id;
                //ApplicationContext applicationContext = null;
                //applicationContext = ApplicationContext.CreateContext(tenantParams[0]);
                ApplicationContext applicationContext = ApplicationContext.CreateContext(tenantParams[0], "SuperAdmin");
                UserProfile Companyuser = new UserProfile();
                Companyuser = applicationContext.CurrentUser;

                if (Companyuser != null)
                {
                    //var identity = (System.Security.Claims.ClaimsIdentity)Context.User.Identity;
                    //Companyuser.ParentUserId = ParentUserId;
                    //claim  code end 
                    signinManager.SignInAsync(Companyuser, true, true);
                    Session["isFromSuprAdmin"] = ParentUserId;
                }
                Response.Redirect("~/Admin/Admin.aspx");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Reset Password", "alert('Tenant does not exist till now')", true);
            }
        }
    }
}