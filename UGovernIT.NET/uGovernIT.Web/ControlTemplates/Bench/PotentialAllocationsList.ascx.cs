using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PotentialAllocationsList : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ResourceAllocationManager objResourceAllocationManager = null;
        ConfigurationVariableManager objConfigVariableManager = null;
        private UserProfileManager objUserProfileManager = null;
        DepartmentManager objDepartmentManager = null;
        DataTable dtResultset = null;
        public DateTime StartDate;
        public DateTime EndDate;
        public string UserId;
        public string RoleId;
        public string UserName;
        public string ImageURL;
        public string department;
        public string division;
        public string ERPJobIDName
        {
            get
            {
                return uHelper.GetERPJobIDName(context);
            }
        }
        public UserProfile userProfile;
        public string customResourceAllocationURL;

        public string RoleName
        {
            get {
                GlobalRoleManager roleManager = new GlobalRoleManager(context);
                return roleManager.LoadById(RoleId)?.Name ?? string.Empty;
            }
        }

        public bool ShowBenchProjectDepartment
        {
            get {
                return objConfigVariableManager.GetValueAsBool(ConfigConstants.ShowBenchProjectDepartment);
            }
        }
        protected override void OnInit(EventArgs e)
        {
            objUserProfileManager = new UserProfileManager(context);
            objDepartmentManager = new DepartmentManager(context);
            objConfigVariableManager = new ConfigurationVariableManager(context);
            if (Request["StartDate"] != null)
                DateTime.TryParse(Request["StartDate"], out StartDate);

            if (Request["EndDate"] != null)
                DateTime.TryParse(Request["EndDate"], out EndDate);
            UserId = UGITUtility.ObjectToString(Request["userId"]);
            if (!string.IsNullOrEmpty(UserId))
                userProfile = objUserProfileManager.GetUserById(UserId);
            if (userProfile != null) { 
                RoleId = userProfile.GlobalRoleId;
                UserName = userProfile.Name;
                ImageURL = !string.IsNullOrWhiteSpace(userProfile.Picture) ? userProfile.Picture : "/Content/Images/userNew.png";
                if (!string.IsNullOrWhiteSpace(userProfile.Department))
                {
                    Department departmentObj = objDepartmentManager.LoadByID(Convert.ToInt64(userProfile.Department));
                    CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
                    CompanyDivision companyDivision = companyDivisionManager.Get(y => y.ID.Equals(departmentObj.DivisionIdLookup) && y.Deleted == false);
                    department = departmentObj.Title;
                    division = companyDivision?.Title ?? string.Empty;
                }
            }
            //string pageParam = "CustomResourceAllocation";
            bool isUserEnabled = userProfile.Enabled;
            string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&showuserresume=false&selecteddepartment=-1&SelectedResource={0}", UserId));
            //customResourceAllocationURL = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&allocationType={4}&monthlyAllocationEdit=false&showtimeoff=true&IsRedirectFromUtilization=true&IncludeClosed={5}&managerFrom=BenchTab",
            //    pageParam, userProfile.Id, StartDate, EndDate, ResourceAllocationType.Estimated, false));
            customResourceAllocationURL = userLinkUrl;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            objResourceAllocationManager = new ResourceAllocationManager(context);
            //gridAllocation.DataBind();
        }


        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if(dtResultset == null)
            { 
                //dtResultset = objResourceAllocationManager.GetPotentialAllocationsList(context, new DateTime(2024, 2, 1), "20EB6220-399C-4A3E-B050-EA258AE49378", "a9a6495c-fe3d-4689-94c9-6b18442ac591");
            }
            //gridAllocation.DataSource = dtResultset;
        }
    }
}