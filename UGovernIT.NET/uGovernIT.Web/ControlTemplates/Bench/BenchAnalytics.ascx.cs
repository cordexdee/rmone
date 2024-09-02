using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Charts.Native;
using DevExpress.XtraReports.Design;
using DevExpress.Web;

namespace uGovernIT.Web.ControlTemplates.Bench
{
    public partial class BenchAnalytics : System.Web.UI.UserControl
    {
        ConfigurationVariableManager _configVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        ApplicationContext _applicationContext = HttpContext.Current.GetManagerContext();
        public Unit Width { get; internal set; }
        public Unit Height { get; internal set; }
        public string DefaultDisplayMode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool generateColumns = false;
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
            string ControlName = uHelper.GetPostBackControlId(this.Page);

            if (ControlName == "userGroupGridLookup")
            {
                // UGITUtility.CreateCookie(Response, "filter", string.Format("group${0}~#type${1}", ddlUserGroup.SelectedValue, selectedCategory));
                hdnGenerateColumns.Value = "1";
                generateColumns = true;
            }
            if (userGroupGridLookup.DataSource == null)
            {
                LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value, hdnaspFunction.Value);
            }

            imgAdvanceMode.Src = UGITUtility.GetAbsoluteURL("/Content/Images/Newfilter.png");

            if (!IsPostBack)
            {
                loadFunctions();
                //LoadFunctionalRoles();
            }
        }
        private void LoadRolesOnDepartmentAndFunctional(string departmentValues = "", string functionValues = "")
        {
            List<GlobalRole> globalRoles = new List<GlobalRole>();
            globalRoles = uHelper.GetGlobalRoles(_applicationContext, false);

            if (globalRoles?.Count() > 0 && !string.IsNullOrWhiteSpace(functionValues))
            {
                FunctionRoleMappingManager _functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
                List<long> ids = UGITUtility.SplitString(functionValues, Constants.Separator)?.Select(long.Parse)?.ToList() ?? null;
                List<FunctionRoleMapping> lstFunctionRoleMapping = _functionRoleMappingManager.Load(x => ids.Contains(x.FunctionId));
                if (lstFunctionRoleMapping != null && lstFunctionRoleMapping.Count > 0)
                {
                    List<string> lstRoleids = lstFunctionRoleMapping.Select(x => x.RoleId).ToList();
                    globalRoles = globalRoles.Where(x => lstRoleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
                }
            }

            if (globalRoles?.Count() > 0)
            {
                JobTitleManager jobTitleManager = new JobTitleManager(_applicationContext);
                List<JobTitle> jobTitles = new List<JobTitle>();
                List<string> lstDepartments = UGITUtility.ConvertStringToList(departmentValues, Constants.Separator6);
                if (string.IsNullOrEmpty(departmentValues) || departmentValues == "undefined")
                    jobTitles = jobTitleManager.Load();
                else
                    jobTitles = jobTitleManager.Load(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.DepartmentId)));
                List<string> jobtitleids = jobTitles.Select(x => x.RoleId).ToList();
                globalRoles = globalRoles.Where(x => jobtitleids.Contains(x.Id))?.OrderBy(y => y.Name)?.ToList();
            }

            if (globalRoles != null)
            {
                userGroupGridLookup.KeyFieldName = "Id";
                userGroupGridLookup.DataSource = globalRoles;
                userGroupGridLookup.DataBind();
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

            functionGridLookup.KeyFieldName = DatabaseObjects.Columns.ID;
            functionGridLookup.DataSource = functionRoles;
            functionGridLookup.DataBind();
        }

        //private void LoadFunctionalRoles()
        //{
        //    FunctionRoleMappingManager _functionRoleMappingManager = new FunctionRoleMappingManager(HttpContext.Current.GetManagerContext());
        //    List<FunctionRoleMapping> lstFunctionRoleMapping = _functionRoleMappingManager.Load();
        //    if (lstFunctionRoleMapping != null && lstFunctionRoleMapping.Count > 0)
        //    {
        //        List<string> lstRoleids = lstFunctionRoleMapping.Select(x => x.RoleId).ToList();
        //        List<GlobalRole> globalRoles = new List<GlobalRole>();
        //        globalRoles = uHelper.GetGlobalRoles(HttpContext.Current.GetManagerContext(), false).Where(x => lstRoleids.Contains(x.Id)).OrderBy(y => y.Name).ToList();
        //        if (globalRoles != null)
        //        {
        //            cmbRole.DataSource = globalRoles;
        //            cmbRole.TextField = "Name";
        //            cmbRole.ValueField = "ID";
        //            cmbRole.DataBind();

        //        }
        //    }
        //}

        protected void ddlUserGroup_Init(object sender, EventArgs e)
        {
            string hdndept = UGITUtility.ObjectToString(Request[hdnaspDepartment.UniqueID]);
            if (string.IsNullOrEmpty(hdndept))
            {
                GlobalRoleManager globalRoleManager = new GlobalRoleManager(_applicationContext);
                List<GlobalRole> roles = uHelper.GetGlobalRoles(_applicationContext, false); //globalRoleManager.Load(x => !x.Deleted).OrderBy(x => x.Name).ToList();
                if (roles != null)
                {
                    ddlUserGroup.DataSource = roles.OrderBy(x => x.Name);
                    ddlUserGroup.DataTextField = "Name";
                    ddlUserGroup.DataValueField = "Id";
                    ddlUserGroup.DataBind();
                }

                ddlUserGroup.Items.Insert(0, new ListItem("All Roles", ""));
            }
            else
            {
                LoadRolesOnDepartmentAndFunctional(hdndept);
            }
        }
        //protected void ddlUserGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //uHelper.CreateCookie(Response, "filter", string.Format("group-{0}", ddlUserGroup.SelectedValue));
        //    string selectedValue = string.Join(",", userGroupGridLookup.GridView.GetSelectedFieldValues("Id"));
        //    //UGITUtility.CreateCookie(Response, "filter", string.Format("group-{0}#type-{1}", selectedValue, glType.Text));
        //    //hdnSelectedGroup.Value = selectedValue;

        //    //PrepareAllocationGrid();
        //}

        protected void cbpResourceAvailability_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string parameters = UGITUtility.ObjectToString(e.Parameter);
            string[] arrParams = UGITUtility.SplitString(parameters, Constants.CommentSeparator);
            if (parameters.Contains("LoadRoles"))
            {
                if (arrParams.Count() > 0)
                {
                    string[] values = UGITUtility.SplitString(arrParams[0], Constants.Separator2);
                    if (values.Count() >= 1)
                    {
                        LoadRolesOnDepartmentAndFunctional(values[1], hdnaspFunction.Value);
                        if (values[1].EqualsIgnoreCase("undefined"))
                            hdnaspDepartment.Value = "";
                        else
                            hdnaspDepartment.Value = values[1];
                    }
                }
            }
            else if (parameters.Contains("FunctionalRole"))
            {
                if (arrParams.Count() > 1 && !string.IsNullOrWhiteSpace(arrParams[1]))
                {
                    LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value, arrParams[1]);
                }
                else
                {
                    LoadRolesOnDepartmentAndFunctional(hdnaspDepartment.Value, "");
                }
            }
            ASPxCallbackPanel gv = (ASPxCallbackPanel)sender;
            gv.JSProperties["cpResourceAvailabilityCallback"] = hdnaspDepartment.Value;
        }
    }
}