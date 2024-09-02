using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using System.Data;
using uGovernIT.DAL.Store;
using System.Web;

namespace uGovernIT.Manager
{
    public class DepartmentManager:ManagerBase<Department>, IDepartmentManager
    {
        private FieldConfigurationManager _configurationManager;
        
        public DepartmentManager(ApplicationContext context):base(context)
        {
            store = new DepartmentStore(this.dbContext);
            _configurationManager = new FieldConfigurationManager(this.dbContext);
        }
        public List<Department> GetDepartmentData()
        {
            List<Department> departmentList = store.Load();
            foreach (Department dp in departmentList)
            {
                if(dp.CompanyIdLookup.HasValue)
                dp.CompanyLookup = new LookupValue(dp.CompanyIdLookup.Value, Convert.ToString(dp.CompanyIdLookup.Value));

                if(dp.DivisionIdLookup.HasValue)
                dp.DivisionLookup = new LookupValue(dp.DivisionIdLookup.Value, Convert.ToString(dp.DivisionIdLookup.Value));
            }
            return departmentList;
        }

        public string GetDivisionName(long deptID)
        {
            List<Department> departmentList = store.Load(x => x.ID == deptID);
            if (departmentList != null && departmentList.Count == 1)
            {
                if (departmentList[0].DivisionIdLookup.HasValue)
                {
                    return _configurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.DivisionLookup, departmentList[0].DivisionIdLookup.Value.ToString());
                }
            }
            return null;
        }

        public DataTable GetDepartmentInfo(bool enableDivision, bool showDeleted)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@EnableDivision", enableDivision);
            arrParams.Add("@showDeleted", showDeleted);

            DataTable dtDepartments = uGITDAL.ExecuteDataSetWithParameters("usp_GetDepartmentsData", arrParams);
            return dtDepartments;
        }
    }
    public interface IDepartmentManager : IManagerBase<Department>
    {

    }
}
