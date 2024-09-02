using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using uGovernIT.Manager.Managers;
using System.Runtime.Remoting.Contexts;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class GlobalRoleManager : ManagerBase<GlobalRole> , IRolesManager
    {
        public GlobalRoleManager(ApplicationContext context) : base(context)
        {
            store = new GlobalRoleStore(this.dbContext);
        }

        public GlobalRole LoadById(string globalRoleId)
        {
            GlobalRole role = null;
            List<GlobalRole> roles = Load(x => x.Id == globalRoleId);
            if (roles != null && roles.Count > 0)
                return roles.FirstOrDefault();
            else
                return role;

        }

        public void addNewUserField(string pFieldName)
        {
            TicketManager ObjTicketManager = new TicketManager(this.dbContext);
            ModuleViewManager moduleManager = new ModuleViewManager(this.dbContext);
            UGITModule moduleCPR = moduleManager.GetByName(ModuleNames.CPR);
            UGITModule moduleOPM = moduleManager.GetByName(ModuleNames.OPM);
            UGITModule moduleCNS = moduleManager.GetByName(ModuleNames.CNS);
            UGITModule modulePMM = moduleManager.GetByName(ModuleNames.PMM);
            UGITModule moduleNPR = moduleManager.GetByName(ModuleNames.NPR);
            string cprtablename = string.Empty; 
            string opmtablename = string.Empty;
            string cnstablename = string.Empty;
            string pmmtablename = string.Empty;
            string nprtablename = string.Empty;
            DataTable dtCNS = null; DataTable dtOPM = null; DataTable dtCPR = null;
            if (moduleCPR!=null)
             cprtablename = moduleCPR.ModuleTable;
            if (moduleOPM != null)
                opmtablename = moduleOPM.ModuleTable;
            if (moduleCNS != null)
                cnstablename = moduleCNS.ModuleTable;
             pmmtablename = modulePMM.ModuleTable;
             nprtablename = moduleNPR.ModuleTable;
            if (moduleCPR != null)
                 dtCPR = ObjTicketManager.GetTableSchemaDetail(cprtablename, string.Empty);
            if (moduleOPM != null)
                 dtOPM = ObjTicketManager.GetTableSchemaDetail(opmtablename, string.Empty);
            if (moduleCNS != null)
                 dtCNS = ObjTicketManager.GetTableSchemaDetail(cnstablename, string.Empty);
            DataTable dtPMM = ObjTicketManager.GetTableSchemaDetail(pmmtablename, string.Empty);
            DataTable dtNPR = ObjTicketManager.GetTableSchemaDetail(nprtablename, string.Empty);
            using (DbConnection connection = new SqlConnection(UGITUtility.ObjectToString(ConfigurationManager.ConnectionStrings["cnn"])))
            {
                connection.Open();
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(this.dbContext);
                if (!uHelper.IfColumnExists(pFieldName, dtCPR) && dtCPR!=null)
                {
                    using (DbCommand command1 = new SqlCommand($"alter table {cprtablename} add {pFieldName} nvarchar(max)"))
                    {
                        command1.Connection = connection;
                        int i = command1.ExecuteNonQuery();
                    }
                    FieldConfiguration field = new FieldConfiguration() { FieldName = pFieldName, Datatype = "UserField", ParentTableName = cprtablename };
                    fieldManager.Insert(field);
                }
                if (!uHelper.IfColumnExists(pFieldName, dtOPM) && dtOPM != null)
                {
                    using (DbCommand command2 = new SqlCommand($"alter table {opmtablename} add {pFieldName} nvarchar(max)"))
                    {
                        command2.Connection = connection;
                        int i = command2.ExecuteNonQuery();
                    }
                    FieldConfiguration field = new FieldConfiguration() { FieldName = pFieldName, Datatype = "UserField", ParentTableName = opmtablename };
                    fieldManager.Insert(field);
                }
                if (!uHelper.IfColumnExists(pFieldName, dtCNS) && dtCNS != null)
                {
                    using (DbCommand command3 = new SqlCommand($"alter table {cnstablename} add {pFieldName} nvarchar(max)"))
                    {
                        command3.Connection = connection;
                        int i = command3.ExecuteNonQuery();
                    }
                    FieldConfiguration field = new FieldConfiguration() { FieldName = pFieldName, Datatype = "UserField", ParentTableName = cnstablename };
                    fieldManager.Insert(field);
                }
                if (!uHelper.IfColumnExists(pFieldName, dtPMM))
                {
                    using (DbCommand command4 = new SqlCommand($"alter table {pmmtablename} add {pFieldName} nvarchar(max)"))
                    {
                        command4.Connection = connection;
                        int i = command4.ExecuteNonQuery();
                    }
                    FieldConfiguration field = new FieldConfiguration() { FieldName = pFieldName, Datatype = "UserField", ParentTableName = pmmtablename };
                    fieldManager.Insert(field);
                }
                if (!uHelper.IfColumnExists(pFieldName, dtNPR))
                {
                    using (DbCommand command5 = new SqlCommand($"alter table {nprtablename} add {pFieldName} nvarchar(max)"))
                    {
                        command5.Connection = connection;
                        int i = command5.ExecuteNonQuery();
                    }
                    FieldConfiguration field = new FieldConfiguration() { FieldName = pFieldName, Datatype = "UserField", ParentTableName = nprtablename };
                    fieldManager.Insert(field);
                }
            }
            
        }

        public void MapUserRoles(string roleID, bool toDelete = false)
        {
            try
            {
                GlobalRole role = this.LoadById(roleID);
                string userRole = "";
                string fieldName = "";

                if (role != null)
                {
                    userRole = role.Name;
                    fieldName = role.FieldName;
                }
                else
                    return;

                ModuleViewManager ModuleManagerObj = new ModuleViewManager(this.dbContext);
                ModuleUserTypeManager objModuleUserTypeManager = new ModuleUserTypeManager(this.dbContext);

                List<string> moduleNames = new List<string>() { ModuleNames.CPR, ModuleNames.OPM, ModuleNames.CNS, ModuleNames.PMM, ModuleNames.NPR };
                List<UGITModule> lstModules = ModuleManagerObj.Load(x => x.EnableModule && moduleNames.Contains(x.ModuleName)).OrderBy(x => x.ModuleName).ToList();
                if (lstModules != null && lstModules.Count > 0)
                {
                    foreach (UGITModule module in lstModules)
                    {
                        ModuleUserType objModuleUserType;
                        ModuleUserType moduleUserType = objModuleUserTypeManager
                            .Load(x =>
                                x.ModuleNameLookup == module.ModuleName
                                && x.ColumnName.ToLower() == fieldName.ToLower()
                                && x.UserTypes.ToLower() == userRole.ToLower())
                            .FirstOrDefault();

                        if (moduleUserType != null)
                        {
                            objModuleUserType = moduleUserType;
                        }
                        else
                        {
                            if (toDelete)
                                continue;
                            objModuleUserType = new ModuleUserType();
                            objModuleUserType.ITOnly = false;
                            objModuleUserType.ManagerOnly = false;
                            objModuleUserType.CustomProperties = null;
                            objModuleUserType.DefaultUser = null;
                            objModuleUserType.Groups = null;
                        }

                        objModuleUserType.Title = module.ModuleName + " - " + userRole;
                        objModuleUserType.ModuleNameLookup = module.ModuleName;
                        objModuleUserType.ColumnName = fieldName;
                        objModuleUserType.UserTypes = userRole;

                        if (objModuleUserType.ID == 0)
                            objModuleUserTypeManager.Insert(objModuleUserType);
                        else if (objModuleUserType.ID > 0 && !toDelete)
                            objModuleUserTypeManager.Update(objModuleUserType);
                        else if (objModuleUserType.ID > 0 && toDelete)
                            objModuleUserTypeManager.Delete(objModuleUserType);

                        if (module != null)
                        {
                            module.List_ModuleUserTypes = objModuleUserTypeManager.Load(x => x.ModuleNameLookup == module.ModuleName);
                            CacheHelper<UGITModule>.AddOrUpdate(module.ModuleName, this.dbContext.TenantID, module);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, $"MapUserRoles - RoleID: {roleID} - Error: {ex.Message}");
            }
        }
    }
    public interface IRolesManager : IManagerBase<GlobalRole>
    {

    }
}
