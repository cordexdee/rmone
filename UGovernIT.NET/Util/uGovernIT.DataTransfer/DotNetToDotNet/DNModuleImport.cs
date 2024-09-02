using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.DataTransfer.DotNetToDotNet
{
    public class DNModuleImport : ModuleEntity
    {
        bool importWithUpdate;
        bool deleteBeforeImport;
        bool importData;
        DNImportContext context;
        List<string> columnsToSkipConversion = new List<string> { DatabaseObjects.Columns.CRMCompanyLookup, DatabaseObjects.Columns.ContactLookup };
        public DNModuleImport(DNImportContext context, string moduleName) : base(context, moduleName)
        {
            this.context = context;
            importWithUpdate = false;
            if (JsonConfig.Config.Global.modules != null)
            {
                foreach (var m in JsonConfig.Config.Global.modules)
                {
                    if (Convert.ToString(m.name).ToLower() == moduleName.ToLower())
                    {
                        if (m.importwithupdate)
                            importWithUpdate = true;

                        deleteBeforeImport = false;
                        if (m.deletebeforeimport)
                            deleteBeforeImport = true;

                        importData = false;
                        if (m.importdata)
                            importData = true;
                        break;
                    }
                }
            }
        }

        public override void UpdateModuleColumns()
        {
            base.UpdateModuleColumns();
            bool import = context.IsImportEnable("ModuleColumns", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating columns {moduleName}");

            {
                ModuleColumnManager mgr = new ModuleColumnManager(context.AppContext);
                List<ModuleColumn> dbData = mgr.Load(x => x.CategoryName == moduleName);
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleColumn>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }


                int targetNewItemCount = 0;

                string fieldName = string.Empty;
                ModuleColumn targetItem = null;

                ModuleColumnManager sourceMgr = new ModuleColumnManager(context.SourceAppContext);
                List<ModuleColumn> sourceDbData = sourceMgr.Load(x => x.CategoryName == moduleName);

                foreach (ModuleColumn item in sourceDbData)
                {
                    fieldName = item.FieldName;
                    targetItem = dbData.FirstOrDefault(x => x.FieldName == fieldName);

                    if (targetItem == null)
                    {
                        targetItem = new ModuleColumn();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} columns added");
            }
        }

        public override void UpdateRequestTypes()
        {
            base.UpdateRequestTypes();
            bool import = context.IsImportEnable("RequestTypes", moduleName);

            if (import)
                ULog.WriteLog($"updating request types {moduleName}");
            else
                ULog.WriteLog($"Load request types mapping");

            if (import && deleteBeforeImport)
            {
                try
                {
                    RequestTypeByLocationManager mgr = new RequestTypeByLocationManager(context.AppContext);
                    List<ModuleRequestTypeLocation> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                    mgr.Delete(dbData);

                    RequestTypeManager mgr1 = new RequestTypeManager(context.AppContext);
                    List<ModuleRequestType> dbData1 = mgr1.Load(x => x.ModuleNameLookup == moduleName);
                    mgr1.Delete(dbData1);
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            {
                string listName = DatabaseObjects.Tables.RequestType;
                MappedItemList maplist = context.GetMappedList(listName);
                RequestTypeManager mgr = new RequestTypeManager(context.AppContext);
                List<ModuleRequestType> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                int targetNewItemCount = 0;

                string category = string.Empty, subCategory = string.Empty, requestType = string.Empty;
                ModuleRequestType targetItem = null;

                MappedItemList userList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);

                RequestTypeManager sourceMgr = new RequestTypeManager(context.SourceAppContext);
                List<ModuleRequestType> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                string sourceItemID = string.Empty;
                foreach (ModuleRequestType item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    category = item.Category;
                    subCategory = item.SubCategory;
                    requestType = item.RequestType;

                    targetItem = dbData.FirstOrDefault(x => x.Category == category && x.SubCategory == subCategory && x.RequestType == requestType);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ModuleRequestType();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = 0;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;


                            targetItem.Owner = context.GetTargetUserValue(targetItem.Owner);
                            targetItem.EscalationManager = context.GetTargetUserValue(targetItem.EscalationManager);
                            targetItem.BackupEscalationManager = context.GetTargetUserValue(targetItem.BackupEscalationManager);
                            targetItem.PRPGroup = context.GetTargetUserValue(targetItem.PRPGroup);
                            targetItem.ORP = context.GetTargetUserValue(targetItem.ORP);

                            targetItem.BudgetIdLookup = context.GetTargetLookupValueLong(Convert.ToString(targetItem.BudgetIdLookup), "BudgetIdLookup", DatabaseObjects.Tables.RequestType);
                            targetItem.FunctionalAreaLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.FunctionalAreaLookup), "FunctionalAreaLookup", DatabaseObjects.Tables.RequestType);
                            targetItem.APPTitleLookup = (int?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.APPTitleLookup), "APPTitleLookup", DatabaseObjects.Tables.RequestType);
                            targetItem.ApplicationModulesLookup = context.GetTargetLookupValue(Convert.ToString(targetItem.ApplicationModulesLookup), "ApplicationModulesLookup", DatabaseObjects.Tables.RequestType);
                            targetItem.TaskTemplateLookup = (int?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.TaskTemplateLookup), "TaskTemplateLookup", DatabaseObjects.Tables.RequestType);
                        }

                        //not found
                        //targetItem.PRP = Convert.ToString(item[SPDatabaseObjects.Columns.PRP]);
                        //targetItem.MatchAllKeywords = Convert.ToString(item[SPDatabaseObjects.Columns.MatchAllKeywords]);
                        //targetItem.breakfx = Convert.ToString(item[SPDatabaseObjects.Columns.BreakFix]);

                        if (targetItem.ID > 0)
                        {
                            mgr.Update(targetItem);
                        }
                        else
                        {
                            mgr.Insert(targetItem);
                        }
                    }

                    if (targetItem != null)
                        maplist.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} request type added");


                RequestTypeByLocationManager _mgr = new RequestTypeByLocationManager(context.AppContext);
                List<ModuleRequestTypeLocation> _dbData = _mgr.Load(x => x.ModuleNameLookup == moduleName);

                listName = DatabaseObjects.Tables.RequestTypeByLocation;
                targetNewItemCount = 0;

                ModuleRequestTypeLocation _targetItem = null;
                MappedItemList locationMappedList = context.GetMappedList(DatabaseObjects.Tables.Location);
                 


                RequestTypeByLocationManager _sourceMgr = new RequestTypeByLocationManager(context.SourceAppContext);
                List<ModuleRequestTypeLocation> _sourceDbData = _sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (ModuleRequestTypeLocation item in _sourceDbData)
                {
                    long targetRequestTypeID = UGITUtility.StringToLong(maplist.GetTargetID(item.RequestTypeLookup.ToString()));
                    if (targetRequestTypeID <= 0)
                        continue;

                    long targetLocationID = 0;
                    if (locationMappedList != null)
                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(item.LocationLookup.ToString()));

                    if (targetLocationID <= 0)
                        continue;


                    _targetItem = _dbData.FirstOrDefault(x => x.RequestTypeLookup == targetRequestTypeID && x.LocationLookup == targetLocationID);
                    if (import)
                    {
                        if (_targetItem == null)
                        {
                            _targetItem = new ModuleRequestTypeLocation();
                            _dbData.Add(_targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = _targetItem.ID;
                            item.TenantID = _targetItem.TenantID;
                            item.ModifiedBy = _targetItem.ModifiedBy;
                            item.CreatedBy = _targetItem.CreatedBy;
                            item.RequestTypeLookup = targetRequestTypeID;
                            item.LocationLookup = targetLocationID;

                            _targetItem = item;
                            _targetItem.Owner = context.GetTargetUserValue(targetItem.Owner);
                            _targetItem.EscalationManager = context.GetTargetUserValue(targetItem.EscalationManager);
                            _targetItem.BackupEscalationManager = context.GetTargetUserValue(targetItem.BackupEscalationManager);
                            _targetItem.PRPGroup = context.GetTargetUserValue(targetItem.PRPGroup);
                            _targetItem.ORP = context.GetTargetUserValue(targetItem.ORP);

                        }

                        //not found
                        //targetItem.PRP = Convert.ToString(item[SPDatabaseObjects.Columns.PRP]);
                        if (_targetItem.ID > 0)
                            _mgr.Update(_targetItem);
                        else
                            _mgr.Insert(_targetItem);
                    }
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} request types by location added");
            }
        }

        public override void UpdateWorkflow()
        {
            base.UpdateWorkflow();

            bool import = context.IsImportEnable("Workflow", moduleName);

            if (import)
                ULog.WriteLog($"Updating user types, workflow, defaultvalues {moduleName}");
            else
                ULog.WriteLog($"Load stage mapping");


            if (import)
            {
                ULog.WriteLog($"Updating user types ({moduleName})");
                ModuleUserTypeManager mgr = new ModuleUserTypeManager(context.AppContext);
                List<ModuleUserType> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                int targetNewItemCount = 0;

                string fieldName = string.Empty;
                ModuleUserType targetItem = null;
                MappedItemList userList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);

                ModuleUserTypeManager sourceMgr = new ModuleUserTypeManager(context.SourceAppContext);
                List<ModuleUserType> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleUserType item in sourceDbData)
                {
                    fieldName = item.FieldName;
                    targetItem = dbData.FirstOrDefault(x => x.ColumnName == fieldName);
                    if (targetItem == null)
                    {
                        targetItem = new ModuleUserType();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {

                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;


                        targetItem.Groups = context.GetTargetUserValue(targetItem.Groups);
                        targetItem.DefaultUser = context.GetTargetUserValue(targetItem.DefaultUser);


                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);
                }


                ULog.WriteLog($"{targetNewItemCount} user types added");
            }



            ULog.WriteLog($"Updating workflow");
            {
                LifeCycleStageManager mgr = new LifeCycleStageManager(context.AppContext);
                List<LifeCycleStage> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<LifeCycleStage>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                string listName = DatabaseObjects.Tables.ModuleStages;
                int targetNewItemCount = 0;

                int stageStep = 0;
                LifeCycleStage targetItem = null;
                MappedItemList userList = context.GetMappedList(DatabaseObjects.Tables.AspNetUsers);
                MappedItemList targetStagetMappedList = context.GetMappedList(listName);

                LifeCycleStageManager sourceMgr = new LifeCycleStageManager(context.SourceAppContext);
                List<LifeCycleStage> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                string sourceItemID = string.Empty;
                foreach (LifeCycleStage item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    stageStep = item.StageStep;
                    targetItem = dbData.FirstOrDefault(x => x.StageStep == stageStep);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new LifeCycleStage();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }
                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;
                        }

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);

                    }
                    if (targetItem != null)
                    {
                        MappedItem mitem = new MappedItem(sourceItemID, targetItem.ID.ToString());
                        targetStagetMappedList.Add(mitem);
                    }
                }

                //Update approved, reject, return
                ULog.WriteLog($"{targetNewItemCount} stages added");
            }

            if (import)
            {
                ULog.WriteLog($"Updating module defaults");

                ModuleDefaultValueManager mgr = new ModuleDefaultValueManager(context.AppContext);
                List<ModuleDefaultValue> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);




                int targetNewItemCount = 0;

                string keyName = string.Empty, moduleStageLookup = string.Empty;
                ModuleDefaultValue targetItem = null;


                ModuleDefaultValueManager sourceMgr = new ModuleDefaultValueManager(context.SourceAppContext);
                List<ModuleDefaultValue> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (ModuleDefaultValue item in sourceDbData)
                {
                    keyName = item.KeyName;
                    moduleStageLookup = context.GetTargetLookupValue(item.ModuleStepLookup, "ModuleStepLookup", DatabaseObjects.Tables.ModuleDefaultValues);

                    targetItem = dbData.FirstOrDefault(x => x.KeyName == keyName && x.ModuleStepLookup == moduleStageLookup);
                    if (!string.IsNullOrWhiteSpace(moduleStageLookup))
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ModuleDefaultValue();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = item.Title;
                            targetItem.ModuleNameLookup = moduleName;
                            targetItem.ModuleStepLookup = moduleStageLookup;
                            targetItem.CustomProperties = item.CustomProperties;
                            targetItem.KeyName = keyName;
                            targetItem.KeyValue = item.KeyValue;
                        }

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }


                }

                ULog.WriteLog($"{targetNewItemCount} module default values added");

            }
        }
        public override void UpdateProjectLifecycles()
        {
            base.UpdateProjectLifecycles();

            bool import = context.IsImportEnable("ProjectLifecycles");

            if (import)
                ULog.WriteLog($"Updating ProjectLifecycles");
            else
                ULog.WriteLog($"Load ProjectLifecycles Mapping");

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating ProjectLifecycles");

            LifeCycle lifeCycle = null;
            LifeCycleManager lcMgr = new LifeCycleManager(context.AppContext);
            List<LifeCycle> dbLcData = lcMgr.Load();
            if (import && deleteBeforeImport)
            {
                try
                {
                    lcMgr.Delete(dbLcData);
                    dbLcData = new List<LifeCycle>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ProjectLifeCycles);

            LifeCycleManager sourceLcMgr = new LifeCycleManager(context.SourceAppContext);
            List<LifeCycle> sourceDbLcData = sourceLcMgr.Load(x => x.ModuleNameLookup == moduleName);
            LifeCycle targetItem = null;
            string sourceItemID = string.Empty;
            foreach (LifeCycle item in sourceDbLcData)
            {
                sourceItemID = item.ID.ToString();
                targetItem = dbLcData.FirstOrDefault(x => x.Name == item.Name && x.ModuleNameLookup == item.ModuleNameLookup);
                if (import)
                {
                    if (targetItem == null)
                    {
                        targetItem = new LifeCycle();
                        dbLcData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            lcMgr.Update(targetItem);
                        else
                            lcMgr.Insert(targetItem);
                    }
                }
                if (targetItem != null)
                    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
            }

            ULog.WriteLog($"{targetNewItemCount} ProjectLifecycles added");


            targetNewItemCount = 0;
            ULog.WriteLog($"Updating ProjectLifecycles Stages");

            LifeCycleStageManager lcsMgr = new LifeCycleStageManager(context.AppContext);
            List<LifeCycleStage> dbLcsData = lcsMgr.Load(x => !dbLcData.Any(y => x.LifeCycleName == y.ID) && x.ModuleNameLookup == moduleName);
            if (import && deleteBeforeImport)
            {
                try
                {
                    lcsMgr.Delete(dbLcsData);
                    dbLcsData = new List<LifeCycleStage>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            else
            {
                dbLcsData.ForEach(x => {
                    lifeCycle = lcMgr.LoadByID(x.LifeCycleName ?? 0);
                    if (lifeCycle != null)
                        x.Title = lifeCycle.Name;
                });
            }

            LifeCycleStageManager sourceLcsMgr = new LifeCycleStageManager(context.SourceAppContext);
            List<LifeCycleStage> sourceDbLcsData = sourceLcsMgr.Load(x => !sourceDbLcData.Any(y => x.LifeCycleName == y.ID) && x.ModuleNameLookup == moduleName);
            LifeCycleStage targetItemLcs = null;


            sourceDbLcsData.ForEach(x => {
                lifeCycle = sourceLcMgr.LoadByID(x.LifeCycleName ?? 0);
                if (lifeCycle != null)
                    x.Title = lifeCycle.Name;
            });

            long LifeCycleName;

            foreach (LifeCycleStage item in sourceDbLcsData)
            {
                targetItemLcs = dbLcsData.FirstOrDefault(x => x.StageTitle == item.StageTitle && x.ModuleNameLookup == item.ModuleNameLookup && x.Title == item.Title);
                if (import)
                {
                    if (targetItemLcs == null)
                    {
                        targetItemLcs = new LifeCycleStage();
                        dbLcsData.Add(targetItemLcs);
                        targetNewItemCount++;
                    }

                    if (targetItemLcs.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItemLcs.ID;
                        item.TenantID = targetItemLcs.TenantID;
                        item.ModifiedBy = targetItemLcs.ModifiedBy;
                        item.CreatedBy = targetItemLcs.CreatedBy;
                        targetItemLcs = item;

                        //targetItemLcs.LifeCycleName = context.GetTargetValue(targetItemLcs.LifeCycleName, DatabaseObjects.Columns.LifeCycleName, DatabaseObjects.Tables.ProjectLifeCycles, "Lookup");
                        LifeCycleName = Convert.ToInt64(context.GetTargetValue(targetItemLcs.LifeCycleName, DatabaseObjects.Columns.LifeCycleName, DatabaseObjects.Tables.ProjectLifeCycles, "Lookup"));
                        if (LifeCycleName != 0)
                            targetItemLcs.LifeCycleName = LifeCycleName;
                        else
                            targetItemLcs.LifeCycleName = null;

                        if (targetItemLcs.ID > 0)
                            lcsMgr.Update(targetItemLcs);
                        else
                            lcsMgr.Insert(targetItemLcs);
                    }
                }
            }

            ULog.WriteLog($"{targetNewItemCount} ProjectLifecycles Stages added");
        }
        public override void UpdateFormLayoutAndAccess()
        {
            base.UpdateFormLayoutAndAccess();
            bool import = context.IsImportEnable("FormLayoutAndAccess", moduleName);
            if (!import)
                return;


            ULog.WriteLog($"Updating formtabs ({moduleName})");
            {
                ModuleFormTabManager mgr = new ModuleFormTabManager(context.AppContext);
                List<ModuleFormTab> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleFormTab>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                string tabName = string.Empty;
                ModuleFormTab targetItem = null;


                ModuleFormTabManager sourceMgr = new ModuleFormTabManager(context.SourceAppContext);
                List<ModuleFormTab> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleFormTab item in sourceDbData)
                {
                    tabName = item.TabName;
                    targetItem = dbData.FirstOrDefault(x => x.TabName == tabName && x.TabId == item.TabId);
                    if (targetItem == null)
                    {
                        targetItem = new ModuleFormTab();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;
                        targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);
                        targetItem.AuthorizedToEdit = context.GetTargetUserValue(targetItem.AuthorizedToEdit);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} formtabs added");
            }

            ULog.WriteLog($"Updating Formlayout");
            {

                ModuleFormLayoutManager mgr = new ModuleFormLayoutManager(context.AppContext);
                List<ModuleFormLayout> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);


                int targetNewItemCount = 0;

                int tabID = 0;
                string fieldName = string.Empty;
                ModuleFormLayout targetItem = null;


                ModuleFormLayoutManager sourceMgr = new ModuleFormLayoutManager(context.SourceAppContext);
                List<ModuleFormLayout> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleFormLayout item in sourceDbData)
                {
                    tabID = item.TabId;
                    fieldName = item.FieldName;

                    targetItem = dbData.FirstOrDefault(x => x.TabId == tabID && x.FieldName == fieldName && x.FieldDisplayName == item.FieldDisplayName);
                    if (targetItem == null)
                    {
                        targetItem = new ModuleFormLayout();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }
                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;
                    }
                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }


                ULog.WriteLog($"{targetNewItemCount} Formlayout added");
            }

            ULog.WriteLog($"Updating rolewriteaccess");
            {

                RequestRoleWriteAccessManager mgr = new RequestRoleWriteAccessManager(context.AppContext);
                List<ModuleRoleWriteAccess> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);



                int targetNewItemCount = 0;

                int stageStep = 0;
                string fieldName = string.Empty;
                ModuleRoleWriteAccess targetItem = null;


                RequestRoleWriteAccessManager sourceMgr = new RequestRoleWriteAccessManager(context.SourceAppContext);
                List<ModuleRoleWriteAccess> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleRoleWriteAccess item in sourceDbData)
                {
                    stageStep = item.StageStep;
                    fieldName = item.FieldName;
                    targetItem = dbData.FirstOrDefault(x => x.StageStep == stageStep && x.FieldName == fieldName);
                    if (targetItem == null)
                    {
                        targetItem = new ModuleRoleWriteAccess();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);
                    }

                }

                ULog.WriteLog($"{targetNewItemCount} Formlayout added");
            }
        }

        public override void UpdatePriorityMap()
        {
            base.UpdatePriorityMap();
            bool import = context.IsImportEnable("PriorityMap", moduleName);

            if (import)
                ULog.WriteLog($"Updating Impact ({moduleName})");
            else
                ULog.WriteLog($"Load impact mapping");

            string listName = DatabaseObjects.Tables.TicketImpact;

            {
                ImpactManager mgr = new ImpactManager(context.AppContext);
                List<ModuleImpact> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList = context.GetMappedList(listName);


                int targetNewItemCount = 0;

                string impact = string.Empty;
                ModuleImpact targetItem = null;


                ImpactManager sourceMgr = new ImpactManager(context.SourceAppContext);
                List<ModuleImpact> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                string sourceItemID = string.Empty;
                foreach (ModuleImpact item in sourceDbData)
                {
                    impact = item.Impact;
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.Impact == impact);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ModuleImpact();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                    }

                    if (targetItem != null)
                        mappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} impact added");

            }


            if (import)
                ULog.WriteLog($"Updating severity");
            else
                ULog.WriteLog($"Load severity mapping");

            listName = DatabaseObjects.Tables.TicketSeverity;
            {
                SeverityManager mgr = new SeverityManager(context.AppContext);
                List<ModuleSeverity> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList = context.GetMappedList(listName);

                int targetNewItemCount = 0;

                string severity = string.Empty;
                ModuleSeverity targetItem = null;


                SeverityManager sourceMgr = new SeverityManager(context.SourceAppContext);
                List<ModuleSeverity> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                string sourceItemID = string.Empty;
                foreach (ModuleSeverity item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    severity = item.Severity;
                    targetItem = dbData.FirstOrDefault(x => x.Severity == severity);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ModuleSeverity();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }

                    if (targetItem != null)
                        mappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }


                if (import)
                    ULog.WriteLog($"{targetNewItemCount} severity added");

            }


            if (import)
                ULog.WriteLog($"Updating priority");
            else
                ULog.WriteLog($"Load priority mapping");

            listName = DatabaseObjects.Tables.TicketPriority;
            {
                PrioirtyViewManager mgr = new PrioirtyViewManager(context.AppContext);
                List<ModulePrioirty> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList = context.GetMappedList(listName);


                int targetNewItemCount = 0;

                string uPriority = string.Empty;
                ModulePrioirty targetItem = null;


                PrioirtyViewManager sourceMgr = new PrioirtyViewManager(context.SourceAppContext);
                List<ModulePrioirty> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                string sourceItemID = string.Empty;
                foreach (ModulePrioirty item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    uPriority = item.uPriority;
                    targetItem = dbData.FirstOrDefault(x => x.uPriority == uPriority);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new ModulePrioirty();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }
                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                    }

                    if (targetItem != null)
                        mappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }


                ULog.WriteLog($"{targetNewItemCount} priority added");

            }

            if (import)
            {
                ULog.WriteLog($"Updating priority, severity, impact mapping");
                listName = DatabaseObjects.Tables.RequestPriority;
                {
                    RequestPriorityManager mgr = new RequestPriorityManager(context.AppContext);
                    List<ModulePriorityMap> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);



                    int targetNewItemCount = 0;

                    long priorityLookup = 0, severityLookup = 0, impactLookup = 0;
                    ModulePriorityMap targetItem = null;


                    RequestPriorityManager sourceMgr = new RequestPriorityManager(context.SourceAppContext);
                    List<ModulePriorityMap> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                    foreach (ModulePriorityMap item in sourceDbData)
                    {
                        priorityLookup = context.GetTargetLookupValueLong(item.PriorityLookup.ToString(), "PriorityLookup", null);
                        severityLookup = context.GetTargetLookupValueLong(item.SeverityLookup.ToString(), "SeverityLookup", null);
                        impactLookup = context.GetTargetLookupValueLong(item.ImpactLookup.ToString(), "ImpactLookup", null);
                        targetItem = dbData.FirstOrDefault(x => x.PriorityLookup == priorityLookup && x.SeverityLookup == severityLookup && x.ImpactLookup == impactLookup);
                        if (targetItem == null)
                        {
                            targetItem = new ModulePriorityMap();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.ModuleNameLookup = moduleName;
                            targetItem.ImpactLookup = impactLookup;
                            targetItem.SeverityLookup = severityLookup;
                            targetItem.PriorityLookup = priorityLookup;

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }


                    }


                    ULog.WriteLog($"{targetNewItemCount} priority added");
                }
            }
        }

        public override void UpdateTicketData()
        {
            base.UpdateTicketData();
            if (!importData)
                return;

            ULog.WriteLog($"Updating ticket data");
            try
            {
                {
                    TicketManager ticketMgr = new TicketManager(context.AppContext);
                    ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                    UGITModule module = moduleMgr.LoadByName(moduleName);
                    DataTable ticketSchema = ticketMgr.GetCachedModuleTableSchema(module);


                    TicketManager sourceTicketMgr = new TicketManager(context.SourceAppContext);
                    ModuleViewManager sourceModuleMgr = new ModuleViewManager(context.SourceAppContext);
                    UGITModule sourceModule = sourceModuleMgr.LoadByName(moduleName);
                    if (string.IsNullOrWhiteSpace(sourceModule.ModuleTable))
                    {
                        ULog.WriteException($"Module {moduleName} table is not specified.");
                        return;
                    }
                    DataTable sourceTicketdata = sourceTicketMgr.GetAllTickets(sourceModule);
                    if (sourceTicketdata == null)
                    {
                        ULog.WriteException($"Module {moduleName} is not found");
                        return;
                    }


                    int targetNewItemCount = 0;

                    string ticketID = string.Empty;
                    DataRow targetItem = null;
                    string targetItemID = string.Empty;
                    MappedItemList targetMappedList = context.GetMappedList(moduleName);
                    string sourceItemID = string.Empty;
                    try
                    {
                        object targetVal = string.Empty;
                        foreach (DataRow item in sourceTicketdata.Rows)
                        {
                            sourceItemID = Convert.ToString(item[DatabaseObjects.Columns.ID]);

                            ticketID = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                            targetItem = ticketMgr.GetByTicketID(module, ticketID);
                            if (targetItem == null)
                            {
                                targetItem = ticketSchema.NewRow();
                                ticketSchema.Rows.Add(targetItem);
                            }



                            if (string.IsNullOrWhiteSpace(Convert.ToString(targetItem[DatabaseObjects.Columns.TicketId])) || importWithUpdate)
                            {
                                foreach (DataColumn column in sourceTicketdata.Columns)
                                {
                                    if (column.ColumnName != DatabaseObjects.Columns.ID && targetItem.Table.Columns.Contains(column.ColumnName))
                                    {
                                        if (columnsToSkipConversion.Contains(column.ColumnName))
                                        {
                                            targetItem[column.ColumnName] = item[column.ColumnName];
                                            continue;
                                        }

                                        targetVal = context.GetTargetValue(item[column.ColumnName], column.ColumnName, module.ModuleTable, null);

                                        if (targetVal == null)
                                            targetItem[column.ColumnName] = DBNull.Value;
                                        else
                                            targetItem[column.ColumnName] = targetVal;
                                    }
                                }

                                targetItem[DatabaseObjects.Columns.History] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.History]));
                                targetItem[DatabaseObjects.Columns.Comment] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.Comment]));

                                if (UGITUtility.IfColumnExists(targetItem, DatabaseObjects.Columns.ResolutionComments))
                                    targetItem[DatabaseObjects.Columns.ResolutionComments] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.ResolutionComments]));

                                targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context.AppContext, Convert.ToString(targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), targetItem);
                                targetItem[DatabaseObjects.Columns.CreatedByUser] = Guid.Empty.ToString();
                                targetItem[DatabaseObjects.Columns.ModifiedByUser] = Guid.Empty.ToString();
                                targetItem[DatabaseObjects.Columns.Created] = DateTime.UtcNow;
                                targetItem[DatabaseObjects.Columns.Modified] = DateTime.UtcNow;
                                targetItem[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                                targetItem[DatabaseObjects.Columns.Attachments] = context.GetTargetValue(item[DatabaseObjects.Columns.Attachments], DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment");
                                int rowEffected = ticketMgr.Save(module, targetItem, out targetItemID);

                                if (rowEffected > 0)
                                    targetNewItemCount++;
                                else
                                    ULog.WriteException($"Item: {ticketID} is not updated for table: {module.ModuleTable}");
                            }

                            if (targetItem != null)
                                //targetMappedList.Add(new MappedItem(sourceItemID, Convert.ToString(targetItem[DatabaseObjects.Columns.ID])));
                                targetMappedList.Add(new MappedItem(sourceItemID, targetItemID));
                        }
                    }
                    catch (Exception ex1)
                    {
                        ULog.WriteException(ex1);
                    }

                    ULog.WriteLog($"{targetNewItemCount} item updated/added");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        /// <summary>
        /// Method to Convert UserId in History, Comments, Resoultion Comments, to New Tenants Userid
        /// </summary>
        private string ConvertColumnData(string Value)
        {
            if (string.IsNullOrEmpty(Value))
                return null;

            string[] arrValues = Value.Split(new string[] { Constants.SeparatorForVersions }, StringSplitOptions.RemoveEmptyEntries);
            string UserID = string.Empty;
            string row = Value;
            Guid guidValue = Guid.Empty;
            foreach (var value in arrValues)
            {
                string[] data = value.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (Guid.TryParse(data[0], out guidValue))
                {
                    UserID = context.GetTargetUserValue(data[0]);
                    row = row.Replace(data[0], UserID);
                }
            }
            Value = row;

            return Value;
        }

        public override void UpdateTicketArchiveData()
        {
            base.UpdateTicketData();
            if (!importData)
                return;

            ULog.WriteLog($"Updating {moduleName}_Archive data");
            try
            {
                {
                    TicketManager ticketMgr = new TicketManager(context.AppContext);
                    ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                    UGITModule module = moduleMgr.LoadByName(moduleName);
                    //DataTable ticketSchema = ticketMgr.GetTableSchema(module);
                    DataTable ticketSchema = GetTableDataManager.GetTableStructure($"{module.ModuleTable}_Archive");
                    DataTable targetTicketdata = GetTableDataManager.GetTableStructure($"{module.ModuleTable}_Archive");

                    TicketManager sourceTicketMgr = new TicketManager(context.SourceAppContext);
                    ModuleViewManager sourceModuleMgr = new ModuleViewManager(context.SourceAppContext);
                    UGITModule sourceModule = sourceModuleMgr.LoadByName(moduleName);
                    if (string.IsNullOrWhiteSpace(sourceModule.ModuleTable))
                    {
                        ULog.WriteException($"Module {moduleName} table is not specified.");
                        return;
                    }
                    //DataTable sourceTicketdata = sourceTicketMgr.GetAllTickets(sourceModule);
                    DataTable sourceTicketdata = GetTableDataManager.GetTableData($"{module.ModuleTable}_Archive", $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'");

                    if (sourceTicketdata == null && sourceTicketdata.Rows.Count <= 0)
                    {
                        ULog.WriteException($"Module {moduleName} is not found");
                        return;
                    }


                    int targetNewItemCount = 0;

                    string ticketID = string.Empty;
                    DataRow targetItem = null;

                    //string sourceItemID = string.Empty;
                    try
                    {
                        object targetVal = string.Empty;
                        foreach (DataRow item in sourceTicketdata.Rows)
                        {
                            ticketID = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                            targetItem = item; //ticketMgr.GetByTicketID(module, ticketID);
                            if (targetItem == null)
                            {
                                targetItem = ticketSchema.NewRow();
                                ticketSchema.Rows.Add(targetItem);
                            }



                            if (string.IsNullOrWhiteSpace(Convert.ToString(targetItem[DatabaseObjects.Columns.TicketId])) || importWithUpdate)
                            {
                                foreach (DataColumn column in sourceTicketdata.Columns)
                                {
                                    if (column.ColumnName != DatabaseObjects.Columns.ID && targetItem.Table.Columns.Contains(column.ColumnName))
                                    {
                                        targetVal = context.GetTargetValue(item[column.ColumnName], column.ColumnName, module.ModuleTable, null);

                                        if (targetVal == null)
                                            targetItem[column.ColumnName] = DBNull.Value;
                                        else
                                            targetItem[column.ColumnName] = targetVal;
                                    }
                                }

                                targetItem[DatabaseObjects.Columns.History] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.History]));
                                targetItem[DatabaseObjects.Columns.Comment] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.Comment]));

                                if (UGITUtility.IfColumnExists(targetItem, DatabaseObjects.Columns.ResolutionComments))
                                    targetItem[DatabaseObjects.Columns.ResolutionComments] = ConvertColumnData(Convert.ToString(targetItem[DatabaseObjects.Columns.ResolutionComments]));

                                targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context.AppContext, Convert.ToString(targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), targetItem);
                                targetItem[DatabaseObjects.Columns.CreatedByUser] = Guid.Empty.ToString();
                                targetItem[DatabaseObjects.Columns.ModifiedByUser] = Guid.Empty.ToString();
                                targetItem[DatabaseObjects.Columns.Created] = DateTime.UtcNow;
                                targetItem[DatabaseObjects.Columns.Modified] = DateTime.UtcNow;
                                targetItem[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                                targetItem[DatabaseObjects.Columns.Attachments] = context.GetTargetValue(item[DatabaseObjects.Columns.Attachments], DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment");

                                targetTicketdata.Rows.Add(targetItem.ItemArray);
                                targetNewItemCount++;                                
                            }                         
                        }

                        GetTableDataManager.bulkupload(targetTicketdata, $"{module.ModuleTable}_Archive");
                    }
                    catch (Exception ex1)
                    {
                        ULog.WriteException(ex1);
                    }

                    ULog.WriteLog($"{targetNewItemCount} item updated/added in {module.ModuleTable}_Archive table");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public override void UpdateModuleEmails()
        {
            base.UpdateModuleEmails();

            bool import = context.IsImportEnable("ModuleEmails", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating module emails ({moduleName})");

            {
                TaskEmailViewManager mgr = new TaskEmailViewManager(context.AppContext);
                List<ModuleTaskEmail> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleTaskEmail>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                MappedItemList moduleStageMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleStages);
                string title = string.Empty;
                ModuleTaskEmail targetItem = null;


                TaskEmailViewManager sourceMgr = new TaskEmailViewManager(context.SourceAppContext);
                List<ModuleTaskEmail> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleTaskEmail item in sourceDbData)
                {
                    title = item.Title;
                    targetItem = dbData.FirstOrDefault(x => x.Title == title);

                    if (targetItem == null)
                    {
                        targetItem = new ModuleTaskEmail();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.TenantID = targetItem.TenantID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        targetItem = item;

                        targetItem.TicketPriorityLookup = context.GetTargetLookupValueOptionLong(targetItem.TicketPriorityLookup.GetValueOrDefault(0).ToString(), "TicketPriorityLookup", null);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} module emails added");
            }
        }

        public override void UpdateTicketSLAs()
        {
            base.UpdateTicketSLAs();

            bool import = context.IsImportEnable("TicketSLAs", moduleName);

            if (!import)
                return;

            ULog.WriteLog($"updating Ticket SLAs ({moduleName})");


            if (import && deleteBeforeImport)
            {
                try
                {
                    SlaRulesManager mgr = new SlaRulesManager(context.AppContext);
                    List<ModuleSLARule> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                    ModuleEscalationRuleManager mgr1 = new ModuleEscalationRuleManager(context.AppContext);
                    List<ModuleEscalationRule> dbData1 = mgr1.Load().Where(x => dbData.Exists(y => y.ID == x.SLARuleIdLookup)).ToList();
                    mgr1.Delete(dbData1);

                    mgr.Delete(dbData);
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }

            string listName = DatabaseObjects.Tables.SLARule;
            MappedItemList targetMappedList = new MappedItemList(listName);
            {

                int targetNewItemCount = 0;
                {
                    LifeCycleStageManager lsMgr = new LifeCycleStageManager(context.AppContext);
                    List<LifeCycleStage> lifeCycleStages = lsMgr.Load(x => x.ModuleNameLookup == moduleName);
                    LifeCycleStage selectedStage = null;

                    SlaRulesManager mgr = new SlaRulesManager(context.AppContext);
                    List<ModuleSLARule> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                    string title = string.Empty;
                    ModuleSLARule targetItem = null;


                    SlaRulesManager sourceMgr = new SlaRulesManager(context.SourceAppContext);
                    List<ModuleSLARule> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                    string sourceItemID = string.Empty;
                    foreach (ModuleSLARule item in sourceDbData)
                    {
                        sourceItemID = item.ID.ToString();
                        title = item.Title;

                        targetItem = dbData.FirstOrDefault(x => x.Title == title);
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new ModuleSLARule();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                item.ID = targetItem.ID;
                                item.TenantID = targetItem.TenantID;
                                item.ModifiedBy = targetItem.ModifiedBy;
                                item.CreatedBy = targetItem.CreatedBy;
                                targetItem = item;



                                targetItem.StageTitleLookup = context.GetTargetLookupValueLong(targetItem.StageTitleLookup.ToString(), "StageTitleLookup", null);
                                targetItem.EndStageTitleLookup = context.GetTargetLookupValueLong(targetItem.StageTitleLookup.ToString(), "EndStageTitleLookup", null);
                                if (targetItem.StageTitleLookup > 0)
                                {
                                    selectedStage = lifeCycleStages.FirstOrDefault(x => x.ID == targetItem.StageTitleLookup);
                                    if (selectedStage != null)
                                    {
                                        targetItem.StartStageStep = selectedStage.StageStep;
                                    }
                                }

                                if (targetItem.EndStageTitleLookup > 0)
                                {
                                    selectedStage = lifeCycleStages.FirstOrDefault(x => x.ID == targetItem.EndStageTitleLookup);
                                    if (selectedStage != null)
                                    {
                                        targetItem.EndStageStep = selectedStage.StageStep;
                                    }
                                }
                            }

                            if (targetItem.ID > 0)
                            {
                                mgr.Update(targetItem);
                            }
                            else
                            {
                                mgr.Insert(targetItem);
                            }
                        }

                        if (targetItem != null)
                            targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                    }


                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} SLA Rules added");
                }

                List<MappedItem> allRules = targetMappedList.GetAll();
                targetNewItemCount = 0;
                foreach (MappedItem slaRuleMap in allRules)
                {
                    ModuleEscalationRuleManager mgr = new ModuleEscalationRuleManager(context.AppContext);
                    List<ModuleEscalationRule> dbData = mgr.Load(x => x.SLARuleIdLookup == UGITUtility.StringToLong(slaRuleMap.TargetID));

                    listName = DatabaseObjects.Tables.EscalationRule;
                    ModuleEscalationRule targetItem = null;

                    ModuleEscalationRuleManager sourceMgr = new ModuleEscalationRuleManager(context.SourceAppContext);
                    List<ModuleEscalationRule> sourceDbData = sourceMgr.Load(x => x.SLARuleIdLookup == UGITUtility.StringToLong(slaRuleMap.TargetID));

                    foreach (ModuleEscalationRule item in sourceDbData)
                    {

                        targetItem = dbData.FirstOrDefault(x => x.Title == item.Title);
                        if (import)
                        {
                            if (targetItem == null)
                            {
                                targetItem = new ModuleEscalationRule();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                item.ID = targetItem.ID;
                                item.TenantID = targetItem.TenantID;
                                item.ModifiedBy = targetItem.ModifiedBy;
                                item.CreatedBy = targetItem.CreatedBy;
                                item.SLARuleIdLookup = UGITUtility.StringToLong(slaRuleMap.TargetID);

                                targetItem = item;
                            }

                            //not found
                            //targetItem.PRP = Convert.ToString(item[SPDatabaseObjects.Columns.PRP]);
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }


                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} escalation rules added");
            }
        }

        public override void UpdateTicketDataHistory()
        {
            base.UpdateTicketDataHistory();

            if (!importData)
                return;

            ULog.WriteLog($"Updating Module data workflow history ({moduleName})");
            {
                ModuleWorkflowHistoryManager mgr = new ModuleWorkflowHistoryManager(context.AppContext);
                List<ModuleWorkflowHistory> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleWorkflowHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                //string listName = DatabaseObjects.Tables.ModuleWorkflowHistory;
                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                ModuleWorkflowHistory targetItem = null;


                ModuleWorkflowHistoryManager sourceMgr = new ModuleWorkflowHistoryManager(context.SourceAppContext);
                List<ModuleWorkflowHistory> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleWorkflowHistory item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID);

                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleWorkflowHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = 0;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;
                        targetItem = item;
                        targetItem.StageClosedBy = context.GetTargetUserValue(targetItem.StageClosedBy);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} Module data workflow history added");
            }


            ULog.WriteLog($"Updating Module SLA history");
            {
                WorkflowSLASummaryManager mgr = new WorkflowSLASummaryManager(context.AppContext);
                List<WorkflowSLASummary> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WorkflowSLASummary>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                string listName = DatabaseObjects.Tables.TicketWorkflowSLASummary;
                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                WorkflowSLASummary targetItem = null;


                WorkflowSLASummaryManager sourceMgr = new WorkflowSLASummaryManager(context.SourceAppContext);
                List<WorkflowSLASummary> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (WorkflowSLASummary item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new WorkflowSLASummary();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = 0;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.ServiceLookup = context.GetTargetLookupValueOptionLong(targetItem.ServiceLookup, "ServiceLookup", listName);
                        targetItem.RuleNameLookup = context.GetTargetLookupValueOptionLong(targetItem.ServiceLookup, "RuleNameLookup", listName);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} Module SLA history added");
            }

            
            ULog.WriteLog($"Updating Module Emails");
            {
                EmailsManager mgr = new EmailsManager(context.AppContext);
                List<Email> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<Email>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                Email targetItem = null;


                EmailsManager sourceMgr = new EmailsManager(context.SourceAppContext);
                List<Email> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (Email item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new Email();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = 0;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;                       
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} Module Email added");
            }

            ULog.WriteLog($"Updating ModuleUserStatistics");
            {
                ModuleUserStatisticsManager mgr = new ModuleUserStatisticsManager(context.AppContext);
                List<ModuleUserStatistic> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleUserStatistic>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                ModuleUserStatistic targetItem = null;


                ModuleUserStatisticsManager sourceMgr = new ModuleUserStatisticsManager(context.SourceAppContext);
                List<ModuleUserStatistic> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleUserStatistic item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleUserStatistic();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = 0;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.UserName = context.GetTargetUserValue(targetItem.UserName);                        
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ModuleUserStatistics added");
            }

            ULog.WriteLog($"Updating DashboardSummary");
            {
                DashboardSummaryManager mgr = new DashboardSummaryManager(context.AppContext);
                List<DashboardSummary> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<DashboardSummary>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                DashboardSummary targetItem = null;


                DashboardSummaryManager sourceMgr = new DashboardSummaryManager(context.SourceAppContext);
                List<DashboardSummary> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (DashboardSummary item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new DashboardSummary();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = 0;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.Initiator = context.GetTargetUserValue(targetItem.Initiator);
                        targetItem.Owner = context.GetTargetUserValue(targetItem.Owner);
                        targetItem.Requestor = context.GetTargetUserValue(targetItem.Requestor);
                        targetItem.StageActionUsers = context.GetTargetUserValue(targetItem.StageActionUsers);
                        targetItem.PRP = context.GetTargetUserValue(targetItem.PRP);
                        targetItem.ORP = context.GetTargetUserValue(targetItem.ORP);
                        targetItem.PRPGroup = context.GetTargetUserValue(targetItem.PRPGroup);
                        targetItem.ModuleStepLookup = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleStepLookup), "ModuleStepLookup", DatabaseObjects.Tables.ModuleStages));
                        targetItem.FunctionalAreaLookup = (long?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.FunctionalAreaLookup), "FunctionalAreaLookup", DatabaseObjects.Tables.FunctionalAreas);
                        targetItem.LocationLookup = (long?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.LocationLookup), "LocationLookup", DatabaseObjects.Tables.Location);
                        targetItem.PriorityLookup = (long?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PriorityLookup), "PriorityLookup", DatabaseObjects.Tables.TicketPriority);
                        targetItem.RequestTypeLookup = (long?)context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.RequestTypeLookup), "RequestTypeLookup", DatabaseObjects.Tables.RequestType);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} DashboardSummary added");
            }
        }

        public override void StageExitCriteria()
        {
            base.StageExitCriteria();

            bool import = context.IsImportEnable("StageExitCriteria", moduleName);
            if (!import)
                return;

            if (!importData)
                return;

            ULog.WriteLog($"Updating StageExitCriteria");
            {
                ModuleStageConstraintsManager mgr = new ModuleStageConstraintsManager(context.AppContext);
                List<ModuleStageConstraints> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleStageConstraints>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;

                string ticketID = string.Empty;
                ModuleStageConstraints targetItem = null;


                ModuleStageConstraintsManager sourceMgr = new ModuleStageConstraintsManager(context.SourceAppContext);
                List<ModuleStageConstraints> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);

                foreach (ModuleStageConstraints item in sourceDbData)
                {
                    ticketID = item.TicketId;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == ticketID && x.Title == item.Title && x.ModuleStep == item.ModuleStep);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleStageConstraints();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.AssignedTo = context.GetTargetUserValue(targetItem.AssignedTo);
                        targetItem.CompletedBy = context.GetTargetUserValue(targetItem.CompletedBy);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} StageExitCriteria  ({moduleName}) added");
            }
        }

        public override void UpdateHelpCards()
        {
            base.UpdateHelpCards();

            bool import = context.IsImportEnable("HelpCards", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating Help Cards");

            HelpCardContentManager helpCardContentManager = new HelpCardContentManager(context.AppContext);
            HelpCardManager helpCardManager = new HelpCardManager(context.AppContext);

            //string ticketId = string.Empty;
            List<HelpCardContent> dbHelpCardContents = helpCardContentManager.Load();
            List<HelpCard> dbHelpCards = helpCardManager.Load();

            if (import && deleteBeforeImport)
            {
                try
                {
                    helpCardManager.Delete(dbHelpCards);
                    dbHelpCards = new List<HelpCard>();

                    helpCardContentManager.Delete(dbHelpCardContents);
                    dbHelpCardContents = new List<HelpCardContent>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting Help Cards");
                    ULog.WriteException(ex);
                }
            }

            int targetNewItemCount = 0;

            string fieldName = string.Empty;
            HelpCardContent targetItemContent = null;
            HelpCard targetItemCard = null;
            HelpCard itemCard = null;

            string HelpCardCreatedBy = string.Empty;

            HelpCardContentManager sourceMgr = new HelpCardContentManager(context.SourceAppContext);
            List<HelpCardContent> sourceDbData = sourceMgr.Load();

            HelpCardManager sourceMgrCard = new HelpCardManager(context.SourceAppContext);
            List<HelpCard> sourceDbDataCard = sourceMgrCard.Load();

            foreach (HelpCardContent item in sourceDbData)
            {
                itemCard = sourceDbDataCard.FirstOrDefault(x => x.HelpCardContentID == item.ID);
                HelpCardCreatedBy = item.CreatedBy;
                //Ticket obj = new Ticket(context.AppContext, moduleName);
                //ticketId = obj.GetNewTicketId();

                fieldName = item.Title;
                targetItemContent = dbHelpCardContents.FirstOrDefault(x => x.Title == fieldName);
                targetItemCard = dbHelpCards.FirstOrDefault(x => x.Title == fieldName);

                if (targetItemContent == null)
                {
                    targetItemContent = new HelpCardContent();
                    dbHelpCardContents.Add(targetItemContent);

                    targetItemCard = new HelpCard();
                    dbHelpCards.Add(targetItemCard);

                    targetNewItemCount++;
                }

                if (targetItemContent.ID == 0 || importWithUpdate)
                {
                    item.ID = targetItemContent.ID;
                    //item.TicketId = ticketId;
                    item.TenantID = context.AppContext.TenantID;
                    item.ModifiedBy = targetItemContent.ModifiedBy;
                    item.CreatedBy = targetItemContent.CreatedBy;
                    targetItemContent = item;

                    itemCard.ID = targetItemCard.ID;
                    //itemCard.TicketId = ticketId;
                    itemCard.TenantID = context.AppContext.TenantID;
                    item.ModifiedBy = targetItemContent.ModifiedBy;
                    itemCard.CreatedBy = targetItemContent.CreatedBy;
                    targetItemCard = itemCard;

                    if (!string.IsNullOrWhiteSpace(targetItemCard.AuthorizedToView))
                        targetItemCard.AuthorizedToView = context.GetTargetUserValue(targetItemCard.AuthorizedToView);
                }

                if (targetItemContent.ID > 0)
                {
                    helpCardContentManager.Update(targetItemContent);
                    targetItemCard.HelpCardContentID = targetItemContent.ID;
                    helpCardManager.Update(targetItemCard);                   
                }
                else
                {
                    helpCardContentManager.Insert(targetItemContent);
                    targetItemCard.HelpCardContentID = targetItemContent.ID;
                    helpCardManager.Insert(targetItemCard);
                }

                HelpCardCreatedBy = context.GetTargetUserValue(HelpCardCreatedBy);
                
                // Using ADO.NET methods, as CreatedBy is set to Empty GUID,  where as User's new Id is required.
                GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.HelpCardContent} set {DatabaseObjects.Columns.CreatedByUser} = '{HelpCardCreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItemContent.ID}");
                GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.HelpCard} set {DatabaseObjects.Columns.CreatedByUser} = '{HelpCardCreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItemCard.ID}");
            }
            
            ULog.WriteLog($"{targetNewItemCount} columns added");
                        
        }

        public override void UpdateWikis()
        {
            base.UpdateWikis();

            bool import = context.IsImportEnable("WikiArticles", moduleName);
            if (!import)
                return;

            WikiContents();
            WikiArticles();
            WikiDiscussion();
            WikiLinks();
            WikiReviews();
        }

        private void WikiContents()
        {
            ULog.WriteLog($"Updating WikiContents");
            {
                string CreatedBy = string.Empty;
                WikiContentsManager mgr = new WikiContentsManager(context.AppContext);
                List<WikiContents> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WikiContents>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                WikiContents targetItem = null;

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.WikiContents);
                WikiContentsManager sourceMgr = new WikiContentsManager(context.SourceAppContext);
                List<WikiContents> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                foreach (WikiContents item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    CreatedBy = item.CreatedBy;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new WikiContents();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));

                    CreatedBy = context.GetTargetUserValue(CreatedBy);
                    if (!string.IsNullOrEmpty(CreatedBy))
                        GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.WikiContents} set {DatabaseObjects.Columns.CreatedByUser} = '{CreatedBy}', {DatabaseObjects.Columns.ModifiedByUser} = '{CreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItem.ID}");
                }

                ULog.WriteLog($"{targetNewItemCount} WikiContents added");
            }
        }

        private void WikiArticles()
        {
            ULog.WriteLog($"Updating WikiArticles");
            {
                string CreatedBy = string.Empty;
                WikiArticlesManager mgr = new WikiArticlesManager(context.AppContext);
                List<WikiArticles> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WikiArticles>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                WikiArticles targetItem = null;

                WikiArticlesManager sourceMgr = new WikiArticlesManager(context.SourceAppContext);
                List<WikiArticles> sourceDbData = sourceMgr.Load();
                foreach (WikiArticles item in sourceDbData)
                {
                    CreatedBy = item.CreatedBy;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new WikiArticles();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.WikiContentID = context.GetTargetLookupValueLong(item.WikiContentID, DatabaseObjects.Columns.WikiContentID, DatabaseObjects.Tables.WikiContents);
                        targetItem.RequestTypeLookup = context.GetTargetLookupValueLong(item.RequestTypeLookup, DatabaseObjects.Columns.TicketRequestTypeLookup, DatabaseObjects.Tables.RequestType);

                        if (!string.IsNullOrWhiteSpace(targetItem.AuthorizedToView))
                            targetItem.AuthorizedToView = context.GetTargetUserValue(targetItem.AuthorizedToView);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    CreatedBy = context.GetTargetUserValue(CreatedBy);
                    if (!string.IsNullOrEmpty(CreatedBy))
                        GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.WikiArticles} set {DatabaseObjects.Columns.CreatedByUser} = '{CreatedBy}', {DatabaseObjects.Columns.ModifiedByUser} = '{CreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItem.ID}");
                }

                ULog.WriteLog($"{targetNewItemCount} WikiArticles added");
            }
        }

        private void WikiDiscussion()
        {
            ULog.WriteLog($"Updating WikiDiscussion");
            {
                string CreatedBy = string.Empty;
                WikiDiscussionManager mgr = new WikiDiscussionManager(context.AppContext);
                List<WikiDiscussion> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WikiDiscussion>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                WikiDiscussion targetItem = null;

                WikiDiscussionManager sourceMgr = new WikiDiscussionManager(context.SourceAppContext);
                List<WikiDiscussion> sourceDbData = sourceMgr.Load();
                foreach (WikiDiscussion item in sourceDbData)
                {
                    CreatedBy = item.CreatedBy;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Comment == item.Comment);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new WikiDiscussion();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;                        
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    CreatedBy = context.GetTargetUserValue(CreatedBy);
                    if (!string.IsNullOrEmpty(CreatedBy))
                        GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.WikiDiscussion} set {DatabaseObjects.Columns.CreatedByUser} = '{CreatedBy}', {DatabaseObjects.Columns.ModifiedByUser} = '{CreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItem.ID}");
                }

                ULog.WriteLog($"{targetNewItemCount} WikiDiscussion added");
            }
        }

        private void WikiLinks()
        {
            ULog.WriteLog($"Updating WikiLinks");
            {
                string CreatedBy = string.Empty;
                WikiLinksManager mgr = new WikiLinksManager(context.AppContext);
                List<WikiLinks> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WikiLinks>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                WikiLinks targetItem = null;

                WikiLinksManager sourceMgr = new WikiLinksManager(context.SourceAppContext);
                List<WikiLinks> sourceDbData = sourceMgr.Load();
                foreach (WikiLinks item in sourceDbData)
                {
                    CreatedBy = item.CreatedBy;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new WikiLinks();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    CreatedBy = context.GetTargetUserValue(CreatedBy);
                    if (!string.IsNullOrEmpty(CreatedBy))
                        GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.WikiLinks} set {DatabaseObjects.Columns.CreatedByUser} = '{CreatedBy}', {DatabaseObjects.Columns.ModifiedByUser} = '{CreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItem.ID}");
                }

                ULog.WriteLog($"{targetNewItemCount} WikiLinks added");
            }
        }

        private void WikiReviews()
        {
            ULog.WriteLog($"Updating WikiReviews");
            {
                string CreatedBy = string.Empty;
                WikiReviewsManager mgr = new WikiReviewsManager(context.AppContext);
                List<WikiReviews> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<WikiReviews>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                WikiReviews targetItem = null;

                WikiReviewsManager sourceMgr = new WikiReviewsManager(context.SourceAppContext);
                List<WikiReviews> sourceDbData = sourceMgr.Load();
                foreach (WikiReviews item in sourceDbData)
                {
                    CreatedBy = item.CreatedBy;
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.ReviewType == item.ReviewType && x.ReviewStatus == item.ReviewStatus && x.Rating == item.Rating && x.Score == item.Score);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new WikiReviews();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    CreatedBy = context.GetTargetUserValue(CreatedBy);
                    if (!string.IsNullOrEmpty(CreatedBy))
                        GetTableDataManager.ExecuteQuery($"update {DatabaseObjects.Tables.WikiReviews} set {DatabaseObjects.Columns.CreatedByUser} = '{CreatedBy}', {DatabaseObjects.Columns.ModifiedByUser} = '{CreatedBy}' where {DatabaseObjects.Columns.ID}  = {targetItem.ID}");
                }

                ULog.WriteLog($"{targetNewItemCount} WikiReviews added");
            }
        }

        public override void UpdateTicketTasks()
        {
            base.UpdateTicketTasks();

            bool import = context.IsImportEnable("TicketTasks", moduleName);
            if (!import)
                return;

            if (!importData)
                return;

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating {moduleName} Tasks");
            {
                UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
                List<UGITTask> dbData = mgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);
                if (import && deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<UGITTask>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                
                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleTasks);

                UGITTaskManager sourceMgr = new UGITTaskManager(context.SourceAppContext);
                List<UGITTask> sourceDbData = sourceMgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);

                string sourceItemID = string.Empty;
                UGITTask targetItem = null;
                foreach (UGITTask item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.TicketId == item.TicketId);
                    if (import)
                    {
                        if (targetItem == null)
                        {
                            targetItem = new UGITTask();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            item.ID = targetItem.ID;
                            item.TenantID = targetItem.TenantID;
                            item.ModifiedBy = targetItem.ModifiedBy;
                            item.CreatedBy = targetItem.CreatedBy;
                            targetItem = item;

                            targetItem.RequestTypeCategory = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.RequestTypeCategory), "RequestTypeCategory", DatabaseObjects.Tables.RequestType));

                            if (!string.IsNullOrWhiteSpace(targetItem.Approver))
                                targetItem.Approver = context.GetTargetUserValue(targetItem.Approver);

                            if (!string.IsNullOrWhiteSpace(targetItem.AssignedTo))
                                targetItem.AssignedTo = context.GetTargetUserValue(targetItem.AssignedTo);

                            if (!string.IsNullOrWhiteSpace(targetItem.AssignToPct))
                            {
                                string[] assigments = targetItem.AssignToPct.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                                string UserID = string.Empty;
                                string allocation = targetItem.AssignToPct;
                                foreach (var value in assigments)
                                {
                                    string[] data = value.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                                    UserID = context.GetTargetUserValue(data[0]);
                                    allocation = allocation.Replace(data[0], UserID);
                                }
                                targetItem.AssignToPct = allocation;
                            }

                            if (!string.IsNullOrWhiteSpace(targetItem.CompletedBy))
                                targetItem.CompletedBy = context.GetTargetUserValue(targetItem.CompletedBy);

                            if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                                targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    }
                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }
                ULog.WriteLog($"{targetNewItemCount} {moduleName} Tasks");
            }

            UpdateTicketTaskPredecessors();
        }

        private void UpdateTicketTaskPredecessors()
        {
            UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> dbData = mgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);

            dbData.ForEach(x => {
                if (!string.IsNullOrEmpty(x.Predecessors))
                    x.Predecessors = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(x.Predecessors), DatabaseObjects.Columns.Predecessors, DatabaseObjects.Tables.ModuleTasks));
            });

            mgr.UpdateItems(dbData);
        }

        public override void UpdateRelatedTickets()
        {
            base.UpdateRelatedTickets();

            bool import = context.IsImportEnable("RelatedTickets", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating Related Tickets ({moduleName})");
            {
                TicketRelationManager mgr = new TicketRelationManager(context.AppContext);
                List<TicketRelation> dbData = mgr.Load(x => x.ParentModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<TicketRelation>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                TicketRelation targetItem = null;
                
                TicketRelationManager sourceMgr = new TicketRelationManager(context.SourceAppContext);
                List<TicketRelation> sourceDbData = sourceMgr.Load(x => x.ParentModuleName == moduleName);

                foreach (TicketRelation item in sourceDbData)
                {

                    targetItem = dbData.FirstOrDefault(x => x.ParentModuleName == item.ParentModuleName && x.ParentTicketID == item.ParentTicketID && x.ChildModuleName == item.ChildModuleName && x.ChildTicketID == item.ChildTicketID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new TicketRelation();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} Related Tickets added");
            }
        }

        public override void ProjectMonitorState()
        {
            base.ProjectMonitorState();

            bool import = context.IsImportEnable("ProjectMonitorState", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ProjectMonitorState ({moduleName})");
            {
                ProjectMonitorStateManager mgr = new ProjectMonitorStateManager(context.AppContext);
                List<ProjectMonitorState> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ProjectMonitorState>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ProjectMonitorState targetItem = null;

                ProjectMonitorStateManager sourceMgr = new ProjectMonitorStateManager(context.SourceAppContext);
                List<ProjectMonitorState> sourceDbData = sourceMgr.Load();

                foreach (ProjectMonitorState item in sourceDbData)
                {

                    targetItem = dbData.FirstOrDefault(x => x.ModuleNameLookup == item.ModuleNameLookup && x.TicketId == item.TicketId && x.Title == item.Title);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ProjectMonitorState();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.ModuleMonitorNameLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleMonitorNameLookup), DatabaseObjects.Columns.ModuleMonitorNameLookup, DatabaseObjects.Tables.ModuleMonitors) ?? 0;
                        targetItem.ModuleMonitorOptionIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleMonitorOptionIdLookup), DatabaseObjects.Columns.ModuleMonitorOptionIdLookup, DatabaseObjects.Tables.ModuleMonitorOptions) ?? 0;
                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ProjectMonitorState added");
            }
        }

        public override void ProjectMonitorStateHistory()
        {
            base.ProjectMonitorStateHistory();

            bool import = context.IsImportEnable("ProjectMonitorStateHistory", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ProjectMonitorStateHistory ({moduleName})");
            {
                ProjectMonitorStateHistoryManager mgr = new ProjectMonitorStateHistoryManager(context.AppContext);
                List<ProjectMonitorStateHistory> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ProjectMonitorStateHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ProjectMonitorStateHistory targetItem = null;

                ProjectMonitorStateHistoryManager sourceMgr = new ProjectMonitorStateHistoryManager(context.SourceAppContext);
                List<ProjectMonitorStateHistory> sourceDbData = sourceMgr.Load();

                foreach (ProjectMonitorStateHistory item in sourceDbData)
                {

                    targetItem = dbData.FirstOrDefault(x => x.ModuleName == item.ModuleName && x.TicketId == item.TicketId && x.Title == item.Title);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ProjectMonitorStateHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.ModuleMonitorNameLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleMonitorNameLookup), DatabaseObjects.Columns.ModuleMonitorNameLookup, DatabaseObjects.Tables.ModuleMonitors) ?? 0;
                        targetItem.ModuleMonitorOptionIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ModuleMonitorOptionIdLookup), DatabaseObjects.Columns.ModuleMonitorOptionIdLookup, DatabaseObjects.Tables.ModuleMonitorOptions) ?? 0;
                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ProjectMonitorStateHistory added");
            }
        }

        public override void Sprint()
        {
            base.Sprint();

            bool import = context.IsImportEnable("Sprint", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating Sprint ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                SprintManager mgr = new SprintManager(context.AppContext);
                List<Sprint> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<Sprint>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    DataTable PMMProjectsDb = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'", "ID,Title", null);
                    dbData.ForEach(x => {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                    });
                }

                int targetNewItemCount = 0;
                Sprint targetItem = null;

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.Sprint);
                SprintManager sourceMgr = new SprintManager(context.SourceAppContext);
                List<Sprint> sourceDbData = sourceMgr.Load();

                DataTable PMMProjects = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'", "ID,Title", null);
                sourceDbData.ForEach(x => {
                    x.PMMTitle = Convert.ToString(PMMProjects.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                });

                string sourceItemID = string.Empty;
                foreach (Sprint item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.PMMTitle == item.PMMTitle);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new Sprint();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);


                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} Sprint added");
            }
        }

        public override void ProjectReleases()
        {
            base.ProjectReleases();

            bool import = context.IsImportEnable("ProjectReleases", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ProjectReleases ({moduleName})");
            {
                PMMReleaseManager mgr = new PMMReleaseManager(context.AppContext);
                List<ProjectReleases> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ProjectReleases>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ProjectReleases targetItem = null;

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ProjectReleases);
                PMMReleaseManager sourceMgr = new PMMReleaseManager(context.SourceAppContext);
                List<ProjectReleases> sourceDbData = sourceMgr.Load();
                string sourceItemID = string.Empty;
                foreach (ProjectReleases item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.ReleaseID == item.ReleaseID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ProjectReleases();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);


                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ProjectReleases added");
            }

        }

        public override void SprintTasks()
        {
            base.SprintTasks();

            bool import = context.IsImportEnable("SprintTasks", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating SprintTasks ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);

                SprintTaskManager mgr = new SprintTaskManager(context.AppContext);
                List<SprintTasks> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<SprintTasks>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    SprintManager sprintMgr = new SprintManager(context.AppContext);
                    List<Sprint> dbSprintData = sprintMgr.Load();

                    DataTable PMMProjectsDb = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'", "ID,Title", null);
                    dbData.ForEach(x => {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                        x.SprintTitle = dbSprintData.FirstOrDefault(y => y.ID == x.SprintLookup).Title;
                    });
                }

                int targetNewItemCount = 0;
                SprintTasks targetItem = null;

                //MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.SprintTasks);
                SprintTaskManager sourceMgr = new SprintTaskManager(context.SourceAppContext);
                List<SprintTasks> sourceDbData = sourceMgr.Load();
                //string sourceItemID = string.Empty;

                SprintManager sprintSourceMgr = new SprintManager(context.SourceAppContext);
                List<Sprint> dbSprintSourceData = sprintSourceMgr.Load();
                DataTable PMMProjects = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'", "ID,Title", null);
                sourceDbData.ForEach(x => {
                    x.PMMTitle = Convert.ToString(PMMProjects.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                    x.SprintTitle = dbSprintSourceData.FirstOrDefault(y => y.ID == x.SprintLookup).Title;
                });

                foreach (SprintTasks item in sourceDbData)
                {
                    //sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.PMMTitle == item.PMMTitle && x.SprintTitle == item.SprintTitle);  

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new SprintTasks();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;
                        targetItem.SprintLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.SprintLookup), DatabaseObjects.Columns.SprintLookup, DatabaseObjects.Tables.Sprint) ?? 0;
                        targetItem.ReleaseLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.ReleaseLookup), DatabaseObjects.Columns.ReleaseLookup, DatabaseObjects.Tables.ProjectReleases) ?? 0;

                        if (!string.IsNullOrWhiteSpace(targetItem.AssignedTo))
                            targetItem.AssignedTo = context.GetTargetUserValue(targetItem.AssignedTo);

                        if (!string.IsNullOrWhiteSpace(targetItem.Attachments))
                            targetItem.Attachments = Convert.ToString(context.GetTargetValue(targetItem.Attachments, DatabaseObjects.Columns.Attachments, DatabaseObjects.Tables.Documents, "Attachment"));

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);


                    //if (targetItem != null)
                    //    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} SprintTasks added");
            }
        }

        public override void PMMEvents()
        {
            base.PMMEvents();

            bool import = context.IsImportEnable("PMMEvents", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating PMMEvents ({moduleName})");
            {
                PMMEventManager mgr = new PMMEventManager(context.AppContext);
                List<PMMEvents> dbData = mgr.Load(x => x.PMMIdLookup.StartsWith(moduleName));
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<PMMEvents>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                PMMEvents targetItem = null;

                PMMEventManager sourceMgr = new PMMEventManager(context.SourceAppContext);
                List<PMMEvents> sourceDbData = sourceMgr.Load(x => x.PMMIdLookup.StartsWith(moduleName));
                foreach (PMMEvents item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.PMMIdLookup == item.PMMIdLookup && x.StartDate == item.StartDate && x.EndDate == item.EndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new PMMEvents();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} PMMEvents added");
            }
        }

        public override void NPRResources()
        {
            base.NPRResources();

            bool import = context.IsImportEnable("NPRResources", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating NPRResources");
            {
                NPRResourcesManager mgr = new NPRResourcesManager(context.AppContext);
                List<NPRResource> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<NPRResource>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                NPRResource targetItem = null;

                NPRResourcesManager sourceMgr = new NPRResourcesManager(context.SourceAppContext);
                List<NPRResource> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (NPRResource item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.RoleNameChoice == item.RoleNameChoice && x.BudgetTypeChoice == item.BudgetTypeChoice && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new NPRResource();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.UserSkillLookup = context.GetTargetLookupValueLong(item.UserSkillLookup, DatabaseObjects.Columns.UserSkillLookup, DatabaseObjects.Tables.UserSkills);
                        targetItem._ResourceType = Convert.ToString(targetItem.UserSkillLookup);
                        targetItem.Title = Convert.ToString(targetItem.UserSkillLookup);

                        if (!string.IsNullOrWhiteSpace(targetItem.RequestedResourcesUser))
                            targetItem.RequestedResourcesUser = context.GetTargetUserValue(targetItem.RequestedResourcesUser);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} NPRResources added");
            }
        }

        public override void TicketCountTrends()
        {
            base.TicketCountTrends();

            bool import = context.IsImportEnable("TicketCountTrends", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating TicketCountTrends ({moduleName})");
            {
                TicketCountTrendsManager mgr = new TicketCountTrendsManager(context.AppContext);
                List<TicketCountTrends> dbData = mgr.Load(x => x.ModuleName == moduleName);

                try
                {
                    mgr.Delete(dbData);
                    dbData = new List<TicketCountTrends>();
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
                
                int targetNewItemCount = 0;

                TicketCountTrendsManager sourceMgr = new TicketCountTrendsManager(context.SourceAppContext);
                List<TicketCountTrends> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);

                sourceDbData.ForEach(x => {
                    x.TenantID = context.AppContext.TenantID;
                    x.CreatedBy = string.Empty;
                    x.ModifiedBy = string.Empty;
                });

                mgr.InsertItems(sourceDbData);
                targetNewItemCount = sourceDbData.Count;
                ULog.WriteLog($"{targetNewItemCount} TicketCountTrends added");
            }
        }

        public override void ModuleBudget()
        {
            base.ModuleBudget();

            bool import = context.IsImportEnable("ModuleBudget", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleBudget ({moduleName})");
            {
                ModuleBudgetManager mgr = new ModuleBudgetManager(context.AppContext);
                List<ModuleBudget> dbData = mgr.Load(x => x.ModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleBudget>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ModuleBudget targetItem = null;

                MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.ModuleBudget);

                ModuleBudgetManager sourceMgr = new ModuleBudgetManager(context.SourceAppContext);
                List<ModuleBudget> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);
                string sourceItemID = string.Empty;
                foreach (ModuleBudget item in sourceDbData)
                {
                    sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title && x.BudgetItem == item.BudgetItem && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleBudget();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.BudgetCategoryLookup = context.GetTargetLookupValueLong(item.BudgetCategoryLookup, DatabaseObjects.Columns.BudgetCategoryLookup, DatabaseObjects.Tables.BudgetCategories);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    if (targetItem != null)
                        targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ModuleBudget added");
            }
        }

        public override void ModuleBudgetActuals()
        {
            base.ModuleBudgetActuals();

            bool import = context.IsImportEnable("ModuleBudgetActuals", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleBudgetActuals ({moduleName})");
            {
                BudgetActualsManager mgr = new BudgetActualsManager(context.AppContext);
                List<BudgetActual> dbData = mgr.Load(x => x.ModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<BudgetActual>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                BudgetActual targetItem = null;

                //MappedItemList targetMappedList = context.GetMappedList(DatabaseObjects.Tables.BudgetActual);

                BudgetActualsManager sourceMgr = new BudgetActualsManager(context.SourceAppContext);
                List<BudgetActual> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);
                //string sourceItemID = string.Empty;
                foreach (BudgetActual item in sourceDbData)
                {
                    //sourceItemID = item.ID.ToString();
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new BudgetActual();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.ModuleBudgetLookup = context.GetTargetLookupValueLong(item.ModuleBudgetLookup, DatabaseObjects.Columns.ModuleBudgetLookup, DatabaseObjects.Tables.ModuleBudget);
                        targetItem.VendorLookup = context.GetTargetLookupValueLong(item.VendorLookup, DatabaseObjects.Columns.VendorLookup, DatabaseObjects.Tables.AssetVendors);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                    //if (targetItem != null)
                    //    targetMappedList.Add(new MappedItem(sourceItemID, targetItem.ID.ToString()));
                }

                ULog.WriteLog($"{targetNewItemCount} ModuleBudgetActuals added");
            }
        }

        public override void ModuleBudgetActualsHistory()
        {
            base.ModuleBudgetActualsHistory();

            bool import = context.IsImportEnable("ModuleBudgetActualsHistory", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleBudgetActualsHistory ({moduleName})");
            {
                ModuleBudgetActualsHistoryManager mgr = new ModuleBudgetActualsHistoryManager(context.AppContext);
                List<ModuleBudgetsActualHistory> dbData = mgr.Load(x => x.ModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleBudgetsActualHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ModuleBudgetsActualHistory targetItem = null;

                ModuleBudgetActualsHistoryManager sourceMgr = new ModuleBudgetActualsHistoryManager(context.SourceAppContext);
                List<ModuleBudgetsActualHistory> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);
                foreach (ModuleBudgetsActualHistory item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.Title == item.Title && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleBudgetsActualHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.ModuleBudgetLookup = context.GetTargetLookupValueLong(item.ModuleBudgetLookup, DatabaseObjects.Columns.ModuleBudgetLookup, DatabaseObjects.Tables.ModuleBudget);
                        targetItem.VendorLookup = context.GetTargetLookupValueLong(item.VendorLookup, DatabaseObjects.Columns.VendorLookup, DatabaseObjects.Tables.AssetVendors);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ModuleBudgetActualsHistory added");
            }
        }

        public override void ModuleBudgetHistory()
        {
            base.ModuleBudgetHistory();

            bool import = context.IsImportEnable("ModuleBudgetHistory", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleBudgetHistory ({moduleName})");
            {
                ModuleBudgetHistoryManager mgr = new ModuleBudgetHistoryManager(context.AppContext);
                List<ModuleBudgetHistory> dbData = mgr.Load(x => x.TicketId.StartsWith(moduleName));
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleBudgetHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ModuleBudgetHistory targetItem = null;

                ModuleBudgetHistoryManager sourceMgr = new ModuleBudgetHistoryManager(context.SourceAppContext);
                List<ModuleBudgetHistory> sourceDbData = sourceMgr.Load(x => x.TicketId.StartsWith(moduleName));
                foreach (ModuleBudgetHistory item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.TicketId == item.TicketId && x.BudgetItem == item.BudgetItem && x.BaselineId == item.BaselineId && x.AllocationStartDate == item.AllocationStartDate && x.AllocationEndDate == item.AllocationEndDate && x.BaselineDate == item.BaselineDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleBudgetHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.BudgetLookup = context.GetTargetLookupValueLong(item.BudgetLookup, DatabaseObjects.Columns.BudgetCategoryLookup, DatabaseObjects.Tables.BudgetCategories);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ModuleBudgetHistory added");
            }
        }

        public override void ModuleMonthlyBudget()
        {
            base.ModuleMonthlyBudget();

            bool import = context.IsImportEnable("ModuleMonthlyBudget", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleMonthlyBudget ({moduleName})");
            {
                ModuleMonthlyBudgetManager mgr = new ModuleMonthlyBudgetManager(context.AppContext);
                List<ModuleMonthlyBudget> dbData = mgr.Load(x => x.ModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleMonthlyBudget>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ModuleMonthlyBudget targetItem = null;

                ModuleMonthlyBudgetManager sourceMgr = new ModuleMonthlyBudgetManager(context.SourceAppContext);
                List<ModuleMonthlyBudget> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);
                foreach (ModuleMonthlyBudget item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.ModuleName == item.ModuleName && x.TicketId == item.TicketId && x.AllocationStartDate == item.AllocationStartDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new ModuleMonthlyBudget();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.BudgetCategoryLookup = context.GetTargetLookupValueLong(item.BudgetCategoryLookup, DatabaseObjects.Columns.BudgetCategoryLookup, DatabaseObjects.Tables.BudgetCategories);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ModuleMonthlyBudget added");
            }
        }

        public override void ModuleMonthlyBudgetHistory()
        {
            base.ModuleMonthlyBudgetHistory();

            bool import = context.IsImportEnable("ModuleMonthlyBudgetHistory", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ModuleMonthlyBudgetHistory ({moduleName})");
            {
                ModuleMonthlyBudgetHistoryManager mgr = new ModuleMonthlyBudgetHistoryManager(context.AppContext);
                List<ModuleMonthlyBudgetHistory> dbData = mgr.Load(x => x.ModuleName == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ModuleMonthlyBudgetHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ModuleMonthlyBudgetHistory targetItem = null;

                ModuleMonthlyBudgetHistoryManager sourceMgr = new ModuleMonthlyBudgetHistoryManager(context.SourceAppContext);
                List<ModuleMonthlyBudgetHistory> sourceDbData = sourceMgr.Load(x => x.ModuleName == moduleName);
                foreach (ModuleMonthlyBudgetHistory item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.ModuleName == item.ModuleName && x.TicketId == item.TicketId && x.AllocationStartDate == item.AllocationStartDate && x.BaselineDate == item.BaselineDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new ModuleMonthlyBudgetHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                        targetItem.BudgetLookup = context.GetTargetLookupValueLong(item.BudgetLookup, DatabaseObjects.Columns.BudgetCategoryLookup, DatabaseObjects.Tables.BudgetCategories);

                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} ModuleMonthlyBudgetHistory added");
            }
        }

        public override void BaseLineDetails()
        {
            base.BaseLineDetails();

            bool import = context.IsImportEnable("BaseLineDetails", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating BaseLineDetails ({moduleName})");
            {
                BaseLineDetailsManager mgr = new BaseLineDetailsManager(context.AppContext);
                List<BaseLineDetails> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<BaseLineDetails>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                BaseLineDetails targetItem = null;

                BaseLineDetailsManager sourceMgr = new BaseLineDetailsManager(context.SourceAppContext);
                List<BaseLineDetails> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (BaseLineDetails item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.ModuleNameLookup == item.ModuleNameLookup && x.TicketID == item.TicketID && x.BaselineComment == item.BaselineComment && x.BaselineDate == item.BaselineDate && x.BaselineId == item.BaselineId);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new BaseLineDetails();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} BaseLineDetails added");
            }
        }

        public override void SchedulerActions()
        {
            base.SchedulerActions();

            bool import = context.IsImportEnable("SchedulerActions", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating SchedulerAction ({moduleName})");
            {
                ScheduleActionsManager mgr = new ScheduleActionsManager(context.AppContext);
                List<SchedulerAction> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<SchedulerAction>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                SchedulerAction targetItem = null;

                ScheduleActionsManager sourceMgr = new ScheduleActionsManager(context.SourceAppContext);
                List<SchedulerAction> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (SchedulerAction item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.ModuleNameLookup == item.ModuleNameLookup && x.TicketId == item.TicketId && x.ActionType == item.ActionType && x.StartTime == item.StartTime && x.MailSubject == item.MailSubject);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new SchedulerAction();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} SchedulerAction added");
            }
        }

        public override void SchedulerActionArchives()
        {
            base.SchedulerActionArchives();

            bool import = context.IsImportEnable("SchedulerActionArchives", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating SchedulerActionArchives ({moduleName})");
            {
                ScheduleActionsArchiveManager mgr = new ScheduleActionsArchiveManager(context.AppContext);
                List<SchedulerActionArchive> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<SchedulerActionArchive>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                SchedulerActionArchive targetItem = null;

                ScheduleActionsArchiveManager sourceMgr = new ScheduleActionsArchiveManager(context.SourceAppContext);
                List<SchedulerActionArchive> sourceDbData = sourceMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (SchedulerActionArchive item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.ModuleNameLookup == item.ModuleNameLookup && x.TicketId == item.TicketId && x.ActionType == item.ActionType && x.StartTime == item.StartTime && x.MailSubject == item.MailSubject);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new SchedulerActionArchive();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;
                        item.FileLocation = targetItem.FileLocation;
                        targetItem = item;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} SchedulerActionArchive added");
            }
        }

        public override void PMMComments()
        {
            base.PMMComments();

            bool import = context.IsImportEnable("PMMComments", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating PMMComments ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);

                PMMCommentManager mgr = new PMMCommentManager(context.AppContext);
                List<PMMComments> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<PMMComments>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    DataTable PMMProjectsDb = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'", "ID,Title", null);
                    if (PMMProjectsDb.Rows.Count > 0)
                    {
                        dbData.ForEach(x =>
                        {
                            x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                        });
                    }
                }

                int targetNewItemCount = 0;
                PMMComments targetItem = null;

                PMMCommentManager sourceMgr = new PMMCommentManager(context.SourceAppContext);
                List<PMMComments> sourceDbData = sourceMgr.Load();

                DataTable PMMProjects = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'", "ID,Title", null);
                sourceDbData.ForEach(x => {
                    x.PMMTitle = Convert.ToString(PMMProjects.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                });

                foreach (PMMComments item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.TicketId == item.TicketId && x.PMMTitle == item.PMMTitle && x.ProjectNoteType == item.ProjectNoteType && x.ProjectNote == item.ProjectNote && x.AccomplishmentDate == item.AccomplishmentDate && x.EndDate == item.EndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new PMMComments();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} PMMComments added");
            }
        }

        public override void PMMCommentsHistory()
        {
            base.PMMCommentsHistory();

            bool import = context.IsImportEnable("PMMCommentsHistory", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating PMMCommentsHistory ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);

                PMMCommentHistoryManager mgr = new PMMCommentHistoryManager(context.AppContext);
                List<PMMCommentHistory> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<PMMCommentHistory>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }
                else
                {
                    DataTable PMMProjectsDb = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'", "ID,Title", null);
                    dbData.ForEach(x => {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                    });
                }

                int targetNewItemCount = 0;
                PMMCommentHistory targetItem = null;

                PMMCommentHistoryManager sourceMgr = new PMMCommentHistoryManager(context.SourceAppContext);
                List<PMMCommentHistory> sourceDbData = sourceMgr.Load();

                DataTable PMMProjects = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'", "ID,Title", null);
                sourceDbData.ForEach(x => {
                    x.PMMTitle = Convert.ToString(PMMProjects.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                });

                foreach (PMMCommentHistory item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.TicketId == item.TicketId && x.PMMTitle == item.PMMTitle && x.ProjectNoteType == item.ProjectNoteType && x.ProjectNote == item.ProjectNote && x.AccomplishmentDate == item.AccomplishmentDate && x.BaselineDate == item.BaselineDate && x.EndDate == item.EndDate);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;


                    if (targetItem == null)
                    {
                        targetItem = new PMMCommentHistory();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        item.ID = targetItem.ID;
                        item.ModifiedBy = targetItem.ModifiedBy;
                        item.CreatedBy = targetItem.CreatedBy;
                        item.TenantID = context.AppContext.TenantID;

                        targetItem = item;

                        targetItem.PMMIdLookup = context.GetTargetLookupValueOptionLong(Convert.ToString(targetItem.PMMIdLookup), DatabaseObjects.Columns.TicketPMMIdLookup, moduleName) ?? 0;
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} PMMCommentsHistory added");
            }
        }

        public override void TicketEvents()
        {
            base.TicketEvents();
            bool import = context.IsImportEnable("TicketEvents", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating TicketEvents ({moduleName})");
            {
                TicketEventManager mgr = new TicketEventManager(context.AppContext);
                List<TicketEvents> dbData = mgr.Load(x => x.ModuleName == moduleName && x.TenantID == context.AppContext.TenantID);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<TicketEvents>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                TicketEvents targetItem = null;

                TicketEventManager sourceMgr = new TicketEventManager(context.SourceAppContext);
                List<TicketEvents> sourceDbData = sourceMgr.Load();

                DataTable TicketEvents = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketEvents, $"{DatabaseObjects.Columns.TenantID} = '{context.SourceAppContext.TenantID}'", "ID,Title", null);
                sourceDbData.ForEach(x => {
                    x.Title = Convert.ToString(TicketEvents.AsEnumerable().FirstOrDefault(y => y.Field<string>(DatabaseObjects.Columns.Title) == x.Title)[DatabaseObjects.Columns.Title]);
                });
                foreach (TicketEvents item in sourceDbData)
                {
                    targetItem = dbData.FirstOrDefault(x => x.Title == item.Title && x.TenantID== context.SourceAppContext.TenantID);

                    //only add entry if no record found otherwise continue
                    if (targetItem != null)
                        continue;

                    if (targetItem == null)
                    {
                        targetItem = new TicketEvents();
                        dbData.Add(targetItem);
                        targetNewItemCount++;
                    }

                    if (targetItem.ID == 0 || importWithUpdate)
                    {
                        targetItem.Ticketid = item.Ticketid;
                        targetItem.Status = item.Status;
                        targetItem.StageStep = item.StageStep;
                        targetItem.ModuleName = moduleName;
                        targetItem.TicketEventType = item.TicketEventType;
                        targetItem.EventTime = item.EventTime;
                        targetItem.TicketEventBy = Guid.Empty.ToString();
                        targetItem.CreatedByUser = Guid.Empty.ToString();
                        targetItem.Automatic = item.Automatic;
                        targetItem.Comment = item.Comment;
                        targetItem.AffectedUsers= item.AffectedUsers;
                        targetItem.Created = item.Created;
                        targetItem.PlannedEndDate = item.PlannedEndDate;
                        targetItem.EventReason = item.EventReason;
                        targetItem.Title = item.Title;
                        targetItem.TenantID = Convert.ToString(context.Tenant.TenantID);
                    }

                    if (targetItem.ID > 0)
                        mgr.Update(targetItem);
                    else
                        mgr.Insert(targetItem);

                }

                ULog.WriteLog($"{targetNewItemCount} TicketEvents added");
            }
        }
    }
}
