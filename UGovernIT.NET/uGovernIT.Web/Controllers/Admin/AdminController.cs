using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
namespace uGovernIT.Web
{
    public class AdminController : Controller
    {
        private ApplicationContext _applicationContext = null;
        private UserProfile _user = null;
        private UserProfileManager _userProfileManager;
        private string _userId = string.Empty;
        
        public AdminController()
        {
            ViewBag.AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public ActionResult Index()
        {
            _applicationContext = System.Web.HttpContext.Current.GetManagerContext();
            _user = System.Web.HttpContext.Current.CurrentUser();
            _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            //_userId = _applicationContext.CurrentUser.Id;
            if (_userProfileManager.IsAdmin(_user)  || _userProfileManager.IsUGITSuperAdmin(_user))
            {
                return View();
            }
            else
                return Redirect("/Account/Login.aspx?ReturnUrl=admin/index");
        }

        public ActionResult RequestList()
        {
            return View();
        }

        public ActionResult Dashboards()
        {
            return View();
        }
        public ActionResult Workflows()
        {
            ModuleViewManager ModuleManager = new ModuleViewManager(System.Web.HttpContext.Current.GetManagerContext());
            Dictionary<string, Int64> moduleDictionary = new Dictionary<string, Int64>();
            List<UGITModule> moduleList = ModuleManager.Load().Where(x=>x.EnableModule).ToList();
            moduleList.ForEach(module => { moduleDictionary.Add(module.ModuleName, module.ID); });
            ViewBag.moduleDictionary = moduleDictionary;
            return View();
        }
        public ActionResult FlowControl()
        {
            return View();
        }
        public ActionResult Alert()
        {
            return View();
        }
        public ActionResult Reports()
        {
            ModuleViewManager ModuleManager = new ModuleViewManager(System.Web.HttpContext.Current.GetManagerContext());
            Dictionary<string, Int64> moduleDictionary = new Dictionary<string, Int64>();
            List<UGITModule> moduleList = ModuleManager.Load().Where(x => x.EnableModule).ToList();
            moduleList.ForEach(module => { moduleDictionary.Add(module.ModuleName, module.ID); });
            ViewBag.moduleDictionary = moduleDictionary;

            return View();
        }

    }
}
