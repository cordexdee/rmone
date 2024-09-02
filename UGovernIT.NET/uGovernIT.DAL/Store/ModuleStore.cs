using System;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IModuleStore : IStore<UGITModule>
    {
        List<UGITModule> LoadAllModule();
        UGITModule LoadByName(string moduleName);
        UGITModule LoadByName(string moduleName, bool useCache);
        string GetModuleTableName(string moduleName);
        long GetModuleLastSequence(long id);
        string GetModuleByTableName(string tableName);
    }

    public class ModuleStore : StoreBase<UGITModule>, IModuleStore
    {
        public ModuleStore(CustomDbContext context) : base(context)
        {

        }

        public override UGITModule LoadByID(long id)
        {
            var module = base.Get(x => x.ID == id);
            if(module != null)
            LoadDependentData(module);

            return module;
        } 

        public long GetModuleLastSequence(long id)
        {
            var module = base.Get(x => x.ID == id);

            return module.LastSequence;
        }

        public UGITModule LoadByName(string moduleName )
        {
           return LoadByName(moduleName,true);
        }

        public UGITModule LoadByName(string moduleName, bool usecache)
        {
            var moduleConfigData = new UGITModule();

            if (string.IsNullOrWhiteSpace(moduleName))
                return null;

            // use data from cache
            if (usecache == true)
            {
                 moduleConfigData = CacheHelper<UGITModule>.Get(moduleName, context.TenantID);

                //fetch module config data from database if object for particular module does not exist in cache
                if (moduleConfigData != null) return moduleConfigData;
            }

             moduleConfigData = base.Get(x => x.ModuleName == moduleName && x.TenantID == context.TenantID);

            if (moduleConfigData == null)
                return null;

            LoadDependentData(moduleConfigData);

            CacheHelper<UGITModule>.AddOrUpdate(moduleName, context.TenantID, moduleConfigData);

            return moduleConfigData;
        }

        private void LoadDependentData(UGITModule moduleConfigData)
        {
            var moduleName = moduleConfigData.ModuleName;

            // ModuleLifeCycleDAL lifecycle = new ModuleLifeCycleDAL();
            moduleConfigData.List_LifeCycles = new LifeCycleStore(this.context).LoadByModule(moduleName); // lifecycle.LoadByModule(moduleName);

            moduleConfigData.List_FormLayout = new ModuleFormLayoutStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_RoleWriteAccess = new ModuleRoleWriteAccessStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_DefaultValues = new ModuleDefaultValuesStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_FormTab = new ModuleFormTabStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_TaskEmail = new ModuleTaskEmailStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_RequestTypes = new ModuleRequestTypeStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_RequestTypeByLocation = new ModuleRequestTypeLocationStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_Priorities = new ModulePrioirtyStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_PriorityMaps = new ModulePriorityMapStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_Impacts = new ModuleImpactStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_Severities = new ModuleSeverityStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_ModuleColumns = new ModuleColumnsStore(this.context).LoadByModule(moduleName);

            moduleConfigData.List_ModuleUserTypes = new ModuleUserTypeStore(this.context).LoadByModule(moduleName);
        }

        public List<UGITModule> LoadAllModule()
        {
            return this.Load().ToList();
        }

        public static string GetCacheKey(string moduleName, string tenantId)
        {
            return $"{moduleName}_{tenantId}";
        }

        /// <summary>
        /// Returns Table Name in Database related to the Module
        /// </summary> 
        public string GetModuleTableName(string moduleName)
        {
            UGITModule module = base.Get(x => x.ModuleName.Equals(moduleName, System.StringComparison.InvariantCultureIgnoreCase));
            if (module != null)
                return module.ModuleTable;
            else
                return string.Empty;
        }

        //GetModuleIdByName
        /// <summary>
        /// Returns ID in Database related to the Module
        /// </summary> 
        public long GetModuleIdByName(string moduleName)
        {
            UGITModule module = base.Get(x => x.ModuleName.Equals(moduleName, System.StringComparison.InvariantCultureIgnoreCase));
            if (module != null)
                return module.ID;
            else
                return 0;
        }

        public void UpdateCache(string moduleName)
        {
            UGITModule module =  this.Get(x => x.ModuleName.ToLower() == moduleName.ToLower());
            if (module != null)
            {
                LoadDependentData(module);
                CacheHelper<UGITModule>.AddOrUpdate(moduleName, context.TenantID, module);
            }
        }

        /// <summary>
        /// Method to return Module Name, if Table Name is workflow.
        /// </summary>
        public string GetModuleByTableName(string tableName)
        {
            string cacheName = $"modules_{context.TenantID}";
            List<UGITModule> modules;
            UGITModule module;
            if (CacheHelper<object>.IsExists(cacheName, context.TenantID))
            {
                modules = CacheHelper<object>.Get(cacheName, context.TenantID) as List<UGITModule>;
            }
            else
            {
                modules = this.Load();
                CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, modules);
            }
            module = modules.FirstOrDefault(x => x.ModuleTable == tableName);
            return (module != null ?  module.ModuleName : string.Empty);
        }
    }
}
