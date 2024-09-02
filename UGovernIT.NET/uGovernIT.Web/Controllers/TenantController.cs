using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using uGovernIT.DefaultConfig;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    public class TenantController : Controller
    {
        private ApplicationContext _applicationContext;
        private UserProfileManager _userProfileManager;

        public TenantController()
        {
            _applicationContext = ApplicationContext.Create();
            ViewBag.AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public ActionResult Index()
        {
            _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            //if (!userManager.IsUGITSuperAdmin(user))
            //{
            //    Logout();
            //    //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
            //    //HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //    //return  Redirect("~/Account/Login.aspx");
            //}

            //This code is for get user by tenant name; 
            //UserProfileManager profileManager = HttpContext.GetOwinContext().Get<UserProfileManager>();
            //var model = profileManager.GetUsersProfileByTenant("fef45b2a-4ffd-4bb7-9320-4ab087f241c2");
            //ViewBag.TenantsUserList = model;

            ViewBag.TenantList = GetTenantList();
            return View();
        }

        public ActionResult CreateTenant(Tenant model)
        {
            var result = new UpdateResult();
            var defaultConfigManager = new DefaultConfigManager();

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TenantList = GetTenantList();
                    return View();
                }

                var accountInfo = defaultConfigManager.GetAdminAccountInfo(model.AccountID, model.Email);

                result = defaultConfigManager.CreateTenantInfo(model, "all", accountInfo);

                if (result.success)
                {
                    ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(System.Web.HttpContext.Current.GetManagerContext());
                    string forwardMailAddress = ObjConfigVariable.GetValue(ConfigConstants.ForwardMailAddress);

                    if (!string.IsNullOrEmpty(forwardMailAddress))
                    {
                        var lstforwardMailAddress = forwardMailAddress.Split(',');
                        foreach (string strMail in lstforwardMailAddress)
                        {
                            var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(strMail, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                        }
                    }
                    else
                    {
                        var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(accountInfo.Email, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                    }
                            
                }
                               
                //Util.Cache.CacheHelper<object>.Clear();   //Added to show updated Tenant Data in Grid
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                ULog.WriteException($"An Exception Occurred in CreateTenant: " + ex);
            }
            return Json(result);
        }

        private List<Tenant> GetTenantList()
        {
            try
            {
                var tenantManager = new TenantManager(_applicationContext);
                var tenants = tenantManager.GetTenantList();
                var defaultTenant = ConfigHelper.DefaultTenant.ToLower();

                var sortList = tenants.Where(x => x.AccountID.Equals(defaultTenant, StringComparison.CurrentCultureIgnoreCase)).ToList();
                sortList.AddRange(tenants.Where(x => x.AccountID.ToLower() != defaultTenant));

                return sortList;
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTenantList: " + ex);
                return null;
            }
        }

        [ValidateInput(false)]
        public ActionResult TenantGridViewPartial()
        {
            var model = GetTenantList();
            return PartialView("_TenantGridViewPartial", model);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult TenantGridViewPartialUpdate(Tenant item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tenantManager = new TenantManager(_applicationContext);
                    var tenantInfo = tenantManager.GetTenant(item.AccountID);

                    if (tenantInfo != null)
                    {
                        item.TenantID = tenantInfo.TenantID;
                        item.Country = tenantInfo.Country;
                        item.DBName = tenantInfo.DBName;
                        item.DBServer = tenantInfo.DBServer;
                        item.ModifiedByUser = tenantInfo.ModifiedByUser;
                        item.CreatedByUser = tenantInfo.CreatedByUser;
                        item.SelfRegisteredTenant = tenantInfo.SelfRegisteredTenant;
                        item.Name = tenantInfo.Name;
                        item.Title = tenantInfo.Title;
                        item.Contact = tenantInfo.Contact;
                        item.Subscription = item.Subscription;
                        item.Modified = DateTime.Now;
                        item.TenantUrl = string.IsNullOrEmpty(item.TenantUrl) ? string.Empty : item.TenantUrl;
                        item.Email = item.Email;
                        
                    }
                    var result = tenantManager.UpdateItemCommon(item);


                    List<Tenant> tenants = (List<Tenant>)Util.Cache.CacheHelper<object>.Get(string.Format("Available_Tenants"));
                    tenants.Remove(tenants.FirstOrDefault(x => x.TenantID == item.TenantID));
                    tenants.Add(item);
                    CacheHelper<object>.AddOrUpdate("Available_Tenants", tenants);

                    //Util.Cache.CacheHelper<object>.Clear();   //Added to show updated Tenant Data in Grid                    
                }
                catch (Exception e)
                {
                    ULog.WriteException($"An Exception Occurred in GetExternalLogin: " + e);
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = GetTenantList();

            return PartialView("_TenantGridViewPartial", model);
        }
    }
}