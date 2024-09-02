using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class BenchReport : System.Web.UI.UserControl
    {
        public UserProfileManager _ProfileManager = HttpContext.Current.GetManagerContext().UserManager;
        ConfigurationVariableManager _configVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public string DefaultDisplayMode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            DefaultDisplayMode = _configVariableManager.GetValue(ConfigConstants.DefaultRMMDisplayMode);
            if (string.IsNullOrEmpty(DefaultDisplayMode))
                DefaultDisplayMode = "Weekly";

            if (DefaultDisplayMode == DisplayMode.Monthly.ToString())
            {
                StartDate = Convert.ToString(new DateTime(DateTime.Now.Year, 1, 1));
                EndDate = Convert.ToString(new DateTime(DateTime.Now.Year, 12, 31));
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                // First day of the month before the current month
                DateTime firstDayOfPreviousMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
                // Last day of the month after the current month
                DateTime lastDayOfNextMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(2).AddDays(-1);
                StartDate = firstDayOfPreviousMonth.ToShortDateString();
                EndDate = lastDayOfNextMonth.ToShortDateString();
            }

            if (!IsPostBack)
            {
                loadFunctions();
                LoadFunctionalRoles();
                LoadDdlResourceManager();
            }
        }

        private void loadFunctions()
        {
            FunctionRoleManager _functionRoleManager = new FunctionRoleManager(HttpContext.Current.GetManagerContext());
            List<FunctionRole> functionRoles = _functionRoleManager.Load();
            cmbFunctionRole.ValueField = DatabaseObjects.Columns.ID;
            cmbFunctionRole.TextField = DatabaseObjects.Columns.Title;
            cmbFunctionRole.DataSource = functionRoles;
            cmbFunctionRole.DataBind();
        }

        protected void LoadDdlResourceManager(string values = "", string selectedMgr = "")
        {

            List<UserProfile> lstUserProfile = new List<UserProfile>();
            List<string> lstDepartments = UGITUtility.ConvertStringToList(values, Constants.Separator6);
            if (values == "undefined" || values == "0" || string.IsNullOrEmpty(values))
                lstUserProfile = _ProfileManager.GetUsersProfile().Where(x => x.IsManager == true && x.Enabled == true).OrderBy(x => x.Name).ToList();
            else
                lstUserProfile = _ProfileManager.GetUsersProfile().Where(x => x.IsManager == true && x.Enabled == true && lstDepartments.Contains(x.Department)).OrderBy(x => x.Name).ToList();

            cmbResourceManager.Items.Clear();

            if (lstUserProfile != null)
            {
                foreach (UserProfile userProfileItem in lstUserProfile)
                {
                    cmbResourceManager.Items.Add(new ListEditItem(userProfileItem.Name, userProfileItem.Id.ToString()));
                }

                cmbResourceManager.Items.Insert(0, new ListEditItem("All Users", "0"));
            }
            else
            {
                cmbResourceManager.Items.Insert(0, new ListEditItem("All Users", "0"));
            }


            var formKeys = Request.Form?.AllKeys?.ToList();
            if (formKeys?.Any(x => x.EndsWith("ddlResourceManager")) == true)
            {
                selectedMgr = Request[formKeys.First(x => x.EndsWith("ddlResourceManager"))];
            }

            if (!string.IsNullOrEmpty(selectedMgr) && cmbResourceManager.Items.FindByValue(selectedMgr) != null)
            {
                cmbResourceManager.Items.FindByValue(selectedMgr).Selected = true;
            }

        }

        private void LoadFunctionalRoles()
        {
            FunctionRoleMappingManager _functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
            List<FunctionRoleMapping> lstFunctionRoleMapping = _functionRoleMappingManager.Load();
            if (lstFunctionRoleMapping != null && lstFunctionRoleMapping.Count > 0)
            {
                List<string> lstRoleids = lstFunctionRoleMapping.Select(x => x.RoleId).ToList();
                List<GlobalRole> globalRoles = new List<GlobalRole>();
                globalRoles = uHelper.GetGlobalRoles(HttpContext.Current.GetManagerContext(), false).Where(x => lstRoleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
                if (globalRoles != null)
                {
                    cmbRole.DataSource = globalRoles;
                    cmbRole.TextField = "Name";
                    cmbRole.ValueField = "ID";
                    cmbRole.DataBind();

                }
            }
        }
    }
}