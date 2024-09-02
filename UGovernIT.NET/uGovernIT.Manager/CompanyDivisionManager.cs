using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CompanyDivisionManager:ManagerBase<CompanyDivision>, ICompanyDivisionManager
    {
        DepartmentManager _deptManager;
        ConfigurationVariableManager _configManager;
        public string DivisionLabel;
        public CompanyDivisionManager(ApplicationContext context):base(context)
        {
            _deptManager = new DepartmentManager(this.dbContext);
            store = new CompanyDivisionStore(this.dbContext);
            _configManager = new ConfigurationVariableManager(this.dbContext);
            DivisionLabel = GetDivisionLabel();
        }
        public List<CompanyDivision> GetCompanyDivisionData()
        {
            List<CompanyDivision> companyDivisions = store.Load();

            //List<Department> deptments = deptManager.GetDepartmentData();
            //foreach (CompanyDivision cd in companyDivisions)
            //{
            //    Department dept = deptments.FirstOrDefault(x => x.DivisionIdLookup == cd.ID);
            //    if (dept != null) { cd.Departments.Add(dept); }
            //}
            return companyDivisions;
        }
        public List<CompanyDivision> GetCompanyDivisionDataWithAllDepartments()
        {
            List<CompanyDivision> companyDivisions = store.Load();
            List<Department> deptments = _deptManager.GetDepartmentData();
            foreach (CompanyDivision cd in companyDivisions)
            {
                if (cd.Departments == null) 
                    cd.Departments = new List<Department>();
                cd.Departments.AddRange(deptments.Where(x => x.DivisionIdLookup == cd.ID && !x.Deleted).ToList());
            }
            return companyDivisions;
        }

        public string GetDivisionLabel()
        {
            string DivisionLabel = "Division";
            string divisionLabelValue = _configManager.GetValue(ConfigConstants.DivisionLabel);
            if (!string.IsNullOrEmpty(divisionLabelValue))
                return divisionLabelValue;

            return DivisionLabel;
        }
    }
    public interface ICompanyDivisionManager : IManagerBase<CompanyDivision>
    {
        List<CompanyDivision> GetCompanyDivisionData();
    }

}
