using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class UserDetailsPanel : System.Web.UI.UserControl
    {
        private ApplicationContext _applicationContext = null;
        UserProfileManager ObjUserProfileManager = null;
        List<Utility.Company> companies = new List<Utility.Company>();
        CompanyManager objCompanyManager;
        DepartmentManager deptManager;
        GlobalRoleManager roleManager = null;
        FieldConfiguration field = null;
        FieldConfigurationManager fmanger = null;

        public Unit Width { get; set; }
        public Unit Height { get; set; }

        public string UserId { get; set; }

        public UserProfile UserData
        {
            get 
            {
                UserProfile userInfo = ObjUserProfileManager.LoadById(!string.IsNullOrWhiteSpace(this.UserId) ? this.UserId : _applicationContext.CurrentUser.Id);
                return userInfo;
            }
        }

        public string Division
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(UserData?.Department))
                {
                    List<Department> allDepartments = deptManager.GetDepartmentData();
                    Utility.Department selectedDepartment = allDepartments?.FirstOrDefault(x => x.ID == long.Parse(UserData.Department) && x.Deleted != true);
                    if (selectedDepartment != null)
                    {
                        CompanyDivision selectedDivision = new CompanyDivision();
                        companies = objCompanyManager.LoadAllHierarchy();
                        Company cmp = companies?.FirstOrDefault(x => x.CompanyDivisions != null && x.CompanyDivisions.Exists(y => y.ID.ToString() == selectedDepartment.DivisionLookup.ID));
                        if (cmp != null)
                        {
                            selectedDivision = cmp.CompanyDivisions.FirstOrDefault(x => x.ID.ToString() == selectedDepartment.DivisionLookup.ID);
                        }
                        return selectedDivision.Title;
                    }
                }
                return string.Empty;
            }
        }

        public string Roles
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(UserData?.GlobalRoleId))
                {
                    GlobalRole role = roleManager.LoadById(UserData.GlobalRoleId);
                    if (role != null)
                    {
                        return role.Name;
                    }
                }

                return string.Empty;
            }
        }

        public string Skills 
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(UserData?.Skills))
                {
                    field = fmanger.GetFieldByFieldName(DatabaseObjects.Columns.UserSkillLookup);
                    if (field != null && field.Datatype == "Lookup")
                    {
                        string value = fmanger.GetFieldConfigurationData(field, string.Join("; ", UserData.Skills));
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            return value;
                        }
                    }
                }
                return string.Empty;
            }
        }
        public UserDetailsPanel()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ObjUserProfileManager = new UserProfileManager(_applicationContext);
            deptManager = new DepartmentManager(_applicationContext);
            objCompanyManager = new CompanyManager(_applicationContext);
            roleManager = new GlobalRoleManager(_applicationContext);
            fmanger = new FieldConfigurationManager(_applicationContext);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                this.UserId = _applicationContext.CurrentUser.Id;
            }
        }
    }
}