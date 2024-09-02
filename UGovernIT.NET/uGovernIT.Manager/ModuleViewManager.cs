using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IModuleViewManager : IManagerBase<UGITModule>
    {
        // List<ModuleUserType> GetUserModelTypeData();
        List<UGITModule> LoadAllModule();

        UGITModule LoadByName(string moduleName);
        UGITModule LoadByName(string moduleName, bool useCache);
        string GetModuleTableName(string moduleName);
        long GetModuleLastSequence(long id);
        string GetModuleByTableName(string tableName);
    }

    public class ModuleViewManager : ManagerBase<UGITModule>, IModuleViewManager
    {
        public ModuleViewManager(ApplicationContext context) : base(context)
        {
            store = new ModuleStore(this.dbContext);
        }

        public UGITModule GetByName(string moduleName)
        {
            return (store as ModuleStore).LoadByName(moduleName);
        }

        public UGITModule GetByID(long ID)
        {
            return (store as ModuleStore).LoadByID(ID);
        }

        public long GetModuleLastSequence(long id)
        {
            return (store as ModuleStore).GetModuleLastSequence(id);
        }

        //public List<ModuleUserType> GetUserModelTypeData()
        //{
        //    ModuleUserTypeStore moduleTypeUserData = new ModuleUserTypeStore(this.dbContext);
        //    return moduleTypeUserData.GetConfigModuleUserTypeData();
        //}

        public override UGITModule LoadByID(long ID)
        {
            return (store as ModuleStore).LoadByID(ID);
        }

        public List<UGITModule> LoadAllModule()
        {
            return this.Load().ToList();
        }

        public DataTable LoadAllModules()
        {
            List<UGITModule> modulelist = this.Load();
            return UGITUtility.ToDataTable<UGITModule>(modulelist);
        }

        public UGITModule LoadByName(string moduleName)
        {
            return (store as ModuleStore).LoadByName(moduleName);
        }
        public UGITModule LoadByName(string moduleName, bool useCache)
        {
            return (store as ModuleStore).LoadByName(moduleName, useCache);
        }

        public DataTable LoadModuleListByName(string moduleName, string listName)
        {
            DataTable aResultDt = null;
            ModuleStore module = store as ModuleStore;

            // UGITModule ugitModule = module.LoadFirst(string.Format(" where ModuleName = '{0}'", moduleName)); // ModuleDAL.getModuleConfigData(moduleName);
            UGITModule ugitModule = module.LoadByName(moduleName);

            if (ugitModule != null || listName != null)
            {
                if (listName == DatabaseObjects.Tables.ProjectLifeCycles)
                {
                    aResultDt = UGITUtility.ToDataTable<LifeCycle>(ugitModule.List_LifeCycles);
                }
                if (listName == DatabaseObjects.Tables.FormLayout)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleFormLayout>(ugitModule.List_FormLayout);
                }
                if (listName == DatabaseObjects.Tables.RequestRoleWriteAccess)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleRoleWriteAccess>(ugitModule.List_RoleWriteAccess);
                }
                if (listName == DatabaseObjects.Tables.ModuleFormTab)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleFormTab>(ugitModule.List_FormTab);
                }
                if (listName == DatabaseObjects.Tables.ModuleDefaultValues)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleDefaultValue>(ugitModule.List_DefaultValues);
                }
                if (listName == DatabaseObjects.Tables.TaskEmails)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleTaskEmail>(ugitModule.List_TaskEmail);
                }
                if (listName == DatabaseObjects.Tables.RequestType)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleRequestType>(ugitModule.List_RequestTypes);
                }
                if (listName == DatabaseObjects.Tables.ModuleColumns)
                {
                    aResultDt = UGITUtility.ToDataTable<Utility.ModuleColumn>(ugitModule.List_ModuleColumns);
                }
                if (listName == DatabaseObjects.Tables.ModuleUserTypes)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleUserType>(ugitModule.List_ModuleUserTypes);
                }
                if (listName == DatabaseObjects.Tables.RequestPriority)
                {
                    aResultDt = UGITUtility.ToDataTable<ModulePrioirty>(ugitModule.List_Priorities);
                }
                if (listName == DatabaseObjects.Tables.TicketImpact)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleImpact>(ugitModule.List_Impacts);
                }
                if (listName == DatabaseObjects.Tables.TicketSeverity)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleSeverity>(ugitModule.List_Severities);
                }
                if (listName == DatabaseObjects.Tables.RequestTypeByLocation)
                {
                    aResultDt = UGITUtility.ToDataTable<ModuleRequestTypeLocation>(ugitModule.List_RequestTypeByLocation);
                }
            }
            return aResultDt;
        }

        /// <summary>
        /// Returns Table Name in Database related to the Module
        /// </summary>        
        public string GetModuleTableName(string moduleName)
        {
            return (store as ModuleStore).GetModuleTableName(moduleName);
        }

        /// <summary>
        /// Returns ID in Database related to the Module
        /// </summary>   
        public long GetModuleIdByName(string moduleName)
        {
            return (store as ModuleStore).GetModuleIdByName(moduleName);
        }

        /// <summary>
        /// Method to return Module Name, if Table Name is workflow.
        /// </summary>
        public string GetModuleByTableName(string tableName)
        {
            return (store as ModuleStore).GetModuleByTableName(tableName);
        }

        public void UpdateCache(string moduleName)
        {
            (store as ModuleStore).UpdateCache(moduleName);
        }

        public void RefreshCache()
        {
            List<UGITModule> lstModules = this.Load().Where(x => x.EnableModule == true && x.EnableCache == true).ToList();
            if (lstModules == null || lstModules.Count == 0)
                return;

            //ThreadStart threadStart = delegate () {
                foreach (UGITModule ugModule in lstModules)
                {
                    (store as ModuleStore).LoadByName(ugModule.ModuleName, false);
                }
            //};
            //Thread _threadStart = new Thread(threadStart);
            //_threadStart.Start();
        }
    }
}
