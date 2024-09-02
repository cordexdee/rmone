using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CompanyManager : ManagerBase<Company>, ICompanyManager
    {
        DepartmentManager deptManager;
        public CompanyManager(ApplicationContext context):base(context)
        {
            deptManager = new DepartmentManager(this.dbContext);
            store = new CompanyStore(this.dbContext);
        }
        public List<Company> LoadAllHierarchy()
        {
            List<Company> companies = new List<Company>();
            companies = GetCompanyData();
            bool isDefaultCompany = true;
            if (companies.Count > 0)
            {
                isDefaultCompany = false;
            }
            //Load all divisions if any
            List<CompanyDivision> divisions = new List<CompanyDivision>();
            CompanyDivisionManager comdivManager = new CompanyDivisionManager(this.dbContext);
            divisions = comdivManager.Load(x => x.Deleted == false);
            //Load all departments if any
            //DepartmentHelper dHelper = new DepartmentHelper();
            //dHelper.IsDeleted = this.IsDeleted;
            List<Department> departments = new List<Department>();
            
            departments = deptManager.GetDepartmentData();

            //the no company exist then put all division and department in default company
            Company defaultCompany = new Company();
            if (isDefaultCompany)
            {
                defaultCompany.CompanyDivisions = divisions.OrderBy(x => x.Title).ToList();
                defaultCompany.Departments = departments.OrderBy(x => x.Title).ToList();
                companies.Add(defaultCompany);
            }
            else
            {
                foreach (Company cmp in companies)
                {
                    cmp.CompanyDivisions = divisions.Where(x => x.CompanyIdLookup == cmp.ID).OrderBy(x => x.Title).ToList();
                    cmp.Departments = departments.Where(x => x.CompanyLookup != null && x.CompanyIdLookup == cmp.ID).OrderBy(x => x.Title).ToList();
                }
            }

            foreach (CompanyDivision division in divisions)
            {
                division.Departments = departments.Where(x => x.DivisionLookup.ID == Convert.ToString(division.ID)).OrderBy(x => x.Title).ToList();
            }


            return companies;
        }
        public List<Company> GetCompanyData()
        {
            List<Company> companyList = store.Load();
           
            List<Department> deptments = deptManager.GetDepartmentData();
            foreach (Company cp in companyList)
            {
                Department dept = deptments.FirstOrDefault(x => x.CompanyIdLookup == cp.ID);
                if (dept != null) { cp.Departments.Add(dept); }

            }
            //List<CompanyDivision> companyDivs = GetCompanyDivisionData();
            //foreach (Company cp in companyList)
            //{
            //    CompanyDivision cpDivs = companyDivs.FirstOrDefault(x => x.CompanyIdLookup == cp.ID);
            //    if (cpDivs != null)
            //        cp.CompanyDivisions.Add(cpDivs);
            //}
            return companyList;
        }
    }
    public interface ICompanyManager : IManagerBase<Company>
    {
        List<Company> LoadAllHierarchy();
        List<Company> GetCompanyData();
      
    }
}
