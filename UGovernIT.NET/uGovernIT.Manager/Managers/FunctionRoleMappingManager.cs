using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager.Managers
{
    public class FunctionRoleMappingManager : ManagerBase<FunctionRoleMapping>, IFunctionRoleMappingManager
    {
        public FunctionRoleMappingManager(ApplicationContext context) : base(context)
        {
            store = new FunctionRoleMappingStore(this.dbContext);
        }

        public  List<FunctionRoleMapping> LoadFunctionRoleMapping()
        {
            try
            {
                FunctionRoleManager _functionRoleManager = new FunctionRoleManager(this.dbContext);
                List<FunctionRoleMapping> lstFunctionrolemapping = this.Load();
                if (lstFunctionrolemapping != null)
                {
                    foreach (FunctionRoleMapping item in lstFunctionrolemapping)
                    {
                        FunctionRole functionRoleObj = _functionRoleManager.LoadByID(item.FunctionId);
                        if (functionRoleObj != null)
                            item.FunctionName = functionRoleObj.Title;

                        GlobalRoleManager _roleManager = new GlobalRoleManager(this.dbContext);
                        GlobalRole roleObj = _roleManager.Load(x => x.Id == item.RoleId).FirstOrDefault();
                        if (roleObj != null)
                            item.RoleName = roleObj.Name;
                    }
                }

                return lstFunctionrolemapping;
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return null;
        }
        public List<FunctionRoleMapping> LoadFunctionRoleMappingById(long FunctionId)
        {
            try
            {
                FunctionRoleManager _functionRoleManager = new FunctionRoleManager(this.dbContext);
                List<FunctionRoleMapping> lstFunctionrolemapping = this.Load().Where(x=> x.FunctionId == FunctionId).ToList();
                if (lstFunctionrolemapping != null)
                {
                    foreach (FunctionRoleMapping item in lstFunctionrolemapping)
                    {
                        FunctionRole functionRoleObj = _functionRoleManager.LoadByID(item.FunctionId);
                        if (functionRoleObj != null)
                            item.FunctionName = functionRoleObj.Title;

                        GlobalRoleManager _roleManager = new GlobalRoleManager(this.dbContext);
                        GlobalRole roleObj = _roleManager.Load(x => x.Id == item.RoleId).FirstOrDefault();
                        if (roleObj != null)
                            item.RoleName = roleObj.Name;
                    }
                }

                return lstFunctionrolemapping;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return null;
        }
    }

    public interface IFunctionRoleMappingManager : IManagerBase<FunctionRoleMapping>
    {

    }
}
