using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class JobTitleManager : ManagerBase<JobTitle>, IJobTitleManager
    {
        public JobTitleManager(ApplicationContext context) : base(context)
        {
            store = new JobTitleStore(this.dbContext);
        }

        public List<JobTitle> loadJobTitles()
        {
            DepartmentManager departmentMGR = new DepartmentManager(dbContext);
            GlobalRoleManager roleMGR = new GlobalRoleManager(dbContext);
            List<JobTitle> lstjobTitles = new List<JobTitle>();
            lstjobTitles = Load();
            foreach(JobTitle jobtitle in lstjobTitles)
            {
                Department departmentObj = departmentMGR.LoadByID(UGITUtility.StringToLong(jobtitle.DepartmentId));
                if (departmentObj != null)
                {
                    jobtitle.DepartmentName = departmentObj.Title ==  null ? string.Empty : departmentObj.Title;
                    jobtitle.DepartmentDescription = departmentObj.DepartmentDescription;
                }
                GlobalRole roleObj = roleMGR.LoadById(jobtitle.RoleId);
                if (roleObj != null)
                    jobtitle.RoleName = roleObj.Name;
            }
            return lstjobTitles;
        }
    }
    public interface IJobTitleManager : IManagerBase<JobTitle>
    {

    }
}
