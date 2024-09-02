using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using uGovernIT.DAL;
using uGovernIT.DataTransfer.Infratructure;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;
using ClientOM = Microsoft.SharePoint.Client;

namespace uGovernIT.DataTransfer.SharePointToDotNet
{
    public class SPModuleImport : ModuleEntity
    {
        bool importWithUpdate;
        bool deleteBeforeImport;
        bool importData;
        Dictionary<string, string> moduleColumnMapped;

        private SPImportContext context;
        public SPModuleImport(SPImportContext context, string moduleName) : base(context, moduleName)
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
            if (moduleName != ModuleNames.RMM && moduleName != "HLP")
                moduleColumnMapped = LoadModuleColumnMapped();

        }

        public override void UpdateWorkflow()
        {
            base.UpdateWorkflow();

            bool import = context.IsImportEnable("Workflow", moduleName);

            if (import)
                ULog.WriteLog($"Updating user types, workflow, defaultvalues");
            else
                ULog.WriteLog($"Load stage mapping");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {

                if (import)
                {
                    ULog.WriteLog($"Updating user types");
                    ModuleUserTypeManager mgr = new ModuleUserTypeManager(context.AppContext);
                    List<ModuleUserType> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                    string listName = SPDatabaseObjects.Lists.ModuleUserTypes;
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;

                    string fieldName = string.Empty;
                    ModuleUserType targetItem = null;
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]);
                            targetItem = dbData.FirstOrDefault(x => x.ColumnName == fieldName);
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleUserType();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.ColumnName = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnName]);
                                if (moduleColumnMapped != null && moduleColumnMapped.ContainsKey(targetItem.ColumnName))
                                    targetItem.ColumnName = moduleColumnMapped[targetItem.ColumnName];
                                targetItem.UserTypes = Convert.ToString(item[SPDatabaseObjects.Columns.UserTypes]);
                                targetItem.ITOnly = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ITOnly]);
                                targetItem.ManagerOnly = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ManagerOnly]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.Groups] as ClientOM.FieldUserValue[];
                                if (userList != null && users != null)
                                    targetItem.Groups = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());

                                ClientOM.FieldUserValue user = item[SPDatabaseObjects.Columns.DefaultUser] as ClientOM.FieldUserValue;
                                if (user != null && userList != null)
                                    targetItem.DefaultUser = userList.GetTargetID(Convert.ToString(user.LookupId));

                                ClientOM.FieldUserValue Author = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Author != null && userList != null)
                                    targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(Author.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userList != null)
                                    targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} user types added");
                }


                // Below method is duplicate and same is calling name as projectlife cycle.
                //ULog.WriteLog($"Updating workflow");
                //{
                //    LifeCycleStageManager mgr = new LifeCycleStageManager(context.AppContext);
                //    List<LifeCycleStage> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                //    if (import && deleteBeforeImport)
                //    {
                //        try
                //        {
                //            mgr.Delete(dbData);
                //            dbData = new List<LifeCycleStage>();
                //        }
                //        catch (Exception ex)
                //        {
                //            ULog.WriteLog("Problem while deleting records");
                //            ULog.WriteException(ex);
                //        }
                //    }

                //    string listName = SPDatabaseObjects.Lists.ModuleStages;
                //    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                //    int targetNewItemCount = 0;

                //    int stageStep = 0;
                //    LifeCycleStage targetItem = null;
                //    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                //    MappedItemList targetStagetMappedList = context.GetMappedList(listName);
                //    MappedItemList tempStagetMappedList = new MappedItemList(listName);
                //    do
                //    {
                //        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, moduleName, position);
                //        position = collection.ListItemCollectionPosition;

                //        foreach (ClientOM.ListItem item in collection)
                //        {
                //            stageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]);
                //            targetItem = dbData.FirstOrDefault(x => x.StageStep == stageStep);
                //            if (import)
                //            {
                //                if (targetItem == null)
                //                {
                //                    targetItem = new LifeCycleStage();
                //                    dbData.Add(targetItem);
                //                    targetNewItemCount++;
                //                }
                //                if (targetItem.ID == 0 || importWithUpdate)
                //                {
                //                    try
                //                    {
                //                        targetItem.StageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.StageTitle]);
                //                        targetItem.Name = Convert.ToString(item[SPDatabaseObjects.Columns.StageTitle]);
                //                        targetItem.ModuleNameLookup = moduleName;
                //                        targetItem.StageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]);
                //                        targetItem.Action = Convert.ToString(item[SPDatabaseObjects.Columns.Action]);
                //                        targetItem.UserPrompt = Convert.ToString(item[SPDatabaseObjects.Columns.UserPrompt]);
                //                        targetItem.ShortStageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.ShortStageTitle]);
                //                        targetItem.SkipOnCondition = Convert.ToString(item[SPDatabaseObjects.Columns.SkipOnCondition]);
                //                        targetItem.ApproveActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveActionDescription]);
                //                        targetItem.RejectActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.RejectActionDescription]);
                //                        targetItem.ReturnActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnActionDescription]);
                //                        targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                //                        targetItem.EnableCustomReturn = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableCustomReturn]);
                //                        targetItem.ApproveIcon = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveIcon]);
                //                        targetItem.ReturnIcon = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnIcon]);
                //                        targetItem.RejectIcon = Convert.ToString(item[SPDatabaseObjects.Columns.RejectIcon]);
                //                        targetItem.SelectedTabNumber = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.SelectedTabNumber]);
                //                        targetItem.ApproveButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveButtonTooltip]);
                //                        targetItem.RejectButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.RejectButtonTooltip]);
                //                        targetItem.ReturnButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnButtonTooltip]);
                //                        targetItem.StageCapacityNormal = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StageCapacityNormal]);
                //                        targetItem.StageCapacityMax = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StageCapacityMax]);
                //                        targetItem.AllowReassignFromList = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowReassignFromList]);
                //                        targetItem.DisableAutoApprove = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisableAutoApprove]);
                //                        targetItem.AutoApproveOnStageTasks = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AutoApproveOnStageTasks]);
                //                        targetItem.StageWeight = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.StageWeight]);
                //                        targetItem.ShowBaselineButtons = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowBaselineButtons]);
                //                        targetItem.StageAllApprovalsRequired = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.StageAllApprovalsRequired]);
                //                        if (item[SPDatabaseObjects.Columns.StageTypeLookup] != null)
                //                            targetItem.StageTypeChoice = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageTypeLookup]).LookupValue);
                //                        targetItem.UserWorkflowStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UserWorkflowStatus]);
                //                        targetItem.StageReturnButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageReturnButtonName]);
                //                        targetItem.StageRejectedButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageRejectedButtonName]);
                //                        targetItem.StageApproveButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleStep]);
                //                        targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                //                        if (item[SPDatabaseObjects.Columns.StageReturnStatus] != null)
                //                            targetItem.StageReturnStatus = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageReturnStatus]).LookupId);
                //                        if (!string.IsNullOrWhiteSpace(targetItem.ActionUser) && moduleColumnMapped != null)
                //                            targetItem.ActionUser = context.CovertActionUserTypes(targetItem.ActionUser, moduleColumnMapped);
                //                        targetItem.DataEditors = Convert.ToString(item[SPDatabaseObjects.Columns.DataEditor]);
                //                        if (!string.IsNullOrWhiteSpace(targetItem.DataEditors) && moduleColumnMapped != null)
                //                            targetItem.DataEditors = context.CovertActionUserTypes(targetItem.DataEditors, moduleColumnMapped);
                //                    }
                //                    catch (Exception e)
                //                    {

                //                    }


                //                }

                //                if (targetItem.ID > 0)
                //                    mgr.Update(targetItem);
                //                else
                //                    mgr.Insert(targetItem);

                //            }
                //            if (targetItem != null)
                //            {
                //                MappedItem mitem = new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString());
                //                mitem.Source = item;
                //                mitem.Target = targetItem;
                //                targetStagetMappedList.Add(mitem);
                //                tempStagetMappedList.Add(mitem);
                //            }
                //        }
                //    } while (position != null);

                //    //Update approved, reject, return

                //    if (import)
                //    {
                //        foreach (var mItem in tempStagetMappedList.GetAll())
                //        {
                //            LifeCycleStage targetStage = mItem.Target as LifeCycleStage;
                //            ClientOM.ListItem sourceStage = mItem.Source as ClientOM.ListItem;

                //            if (sourceStage != null && targetStage != null)
                //            {
                //                ClientOM.FieldLookupValue sourceStagetLookup = sourceStage[SPDatabaseObjects.Columns.StageApprovedStatus] as ClientOM.FieldLookupValue;
                //                if (sourceStagetLookup != null)
                //                {
                //                    MappedItem lookupItem = targetStagetMappedList.Get(sourceStagetLookup.LookupId.ToString());
                //                    if (lookupItem != null)
                //                        if (lookupItem.Source != null)
                //                        {
                //                            targetStage.StageApprovedStatus = UGITUtility.StringToInt(((ClientOM.ListItem)lookupItem.Source)[SPDatabaseObjects.Columns.ModuleStep]);
                //                        }
                //                        else
                //                        {
                //                            targetStage.StageApprovedStatus = 0;
                //                        }
                //                }

                //                sourceStagetLookup = sourceStage[SPDatabaseObjects.Columns.StageReturnStatus] as ClientOM.FieldLookupValue;
                //                if (sourceStagetLookup != null)
                //                {
                //                    MappedItem lookupItem = targetStagetMappedList.Get(sourceStagetLookup.LookupId.ToString());
                //                    if (lookupItem != null)
                //                        if (lookupItem.Source != null)
                //                        {
                //                            targetStage.StageReturnStatus = UGITUtility.StringToInt(((ClientOM.ListItem)lookupItem.Source)[SPDatabaseObjects.Columns.ModuleStep]);
                //                        }
                //                        else
                //                        {
                //                            targetStage.StageReturnStatus = 0;
                //                        }
                //                }

                //                sourceStagetLookup = sourceStage[SPDatabaseObjects.Columns.StageRejectedStatus] as ClientOM.FieldLookupValue;
                //                if (sourceStagetLookup != null)
                //                {
                //                    MappedItem lookupItem = targetStagetMappedList.Get(sourceStagetLookup.LookupId.ToString());
                //                    if (lookupItem != null)
                //                        if (lookupItem.Source != null)
                //                        { targetStage.StageRejectedStatus = UGITUtility.StringToInt(((ClientOM.ListItem)lookupItem.Source)[SPDatabaseObjects.Columns.ModuleStep]); }
                //                        else
                //                        { targetStage.StageRejectedStatus = 0; }
                //                }

                //                mgr.Update(targetStage);
                //            }
                //        }

                //        ULog.WriteLog($"{targetNewItemCount} stages added");
                //    }
                //}

            }

            if (import)
            {
                ULog.WriteLog($"Updating module defaults");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    ModuleDefaultValueManager mgr = new ModuleDefaultValueManager(context.AppContext);
                    List<ModuleDefaultValue> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                    string listName = SPDatabaseObjects.Lists.ModuleDefaultValues;
                    MappedItemList mappedList = context.GetMappedList(listName);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;

                    string moduleStageLookup = string.Empty;
                    ModuleDefaultValue targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleDefaultValue();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                targetItem.KeyName = Convert.ToString(item[SPDatabaseObjects.Columns.KeyName]);
                                targetItem.KeyValue = Convert.ToString(item[SPDatabaseObjects.Columns.KeyValue]);
                                if (item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                {
                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                    targetItem.ModuleStepLookup = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId);
                                    SPTicketrefernces.Add("Module", moduleName);
                                    SPTicketrefernces.Add("Title", Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupValue));
                                    SPTicketrefernces.Add("KeyName", Convert.ToString(item[SPDatabaseObjects.Columns.KeyName]));
                                    GetTableDataManager.AddItem<int>("MigrationModuleDefaults", SPTicketrefernces);
                                }
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                            if (targetItem != null)
                                mappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} module default values added");
                }
            }
        }
        public override void UpdateProjectLifecycles()
        {
            base.UpdateProjectLifecycles();
            Hashtable htStagedtl = new Hashtable();
            bool import = context.IsImportEnable("ProjectLifecycles");

            if (import)
                ULog.WriteLog($"Updating ProjectLifecycles");
            else
                ULog.WriteLog($"Load ProjectLifecycles Mapping");

            int targetNewItemCount = 0;
            MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
            //ULog.WriteLog($"Updating ProjectLifecycles");

            //LifeCycle lifeCycle = null;
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.ProjectLifeCycles;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                LifeCycle targetItem = null;
                if (moduleName == "PMM")
                {
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbLcData.FirstOrDefault(x => x.Name == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem != null)
                                continue;

                            if (import)
                            {
                                if (targetItem == null)
                                {
                                    targetItem = new LifeCycle();
                                    dbLcData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                try
                                {
                                    if (targetItem.ID == 0 || importWithUpdate)
                                    {
                                        targetItem.Name = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                        targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                        targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                        targetItem.ModuleNameLookup = "PMM";
                                    }
                                    if (targetItem.ID > 0)
                                        lcMgr.Update(targetItem);
                                    else
                                        lcMgr.Insert(targetItem);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                }

                            }

                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} ProjectLifecycles added");
                }
            }

            targetNewItemCount = 0;
            ULog.WriteLog($"Updating ProjectLifecycles Stages");

            LifeCycleStageManager lcsMgr = new LifeCycleStageManager(context.AppContext);
            List<LifeCycleStage> dbLcsData = lcsMgr.Load(x => !dbLcData.Any(y => x.LifeCycleName == y.ID));
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
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = string.Empty;
                listName = SPDatabaseObjects.Lists.ModuleStages;
                if (moduleName == "PMM")
                    listName = SPDatabaseObjects.Lists.ProjectLifeCycleStages;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList lifecyclelist = context.GetMappedList(SPDatabaseObjects.Lists.ProjectLifeCycles);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                LifeCycleStage targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        try
                        {
                            if (moduleName != "PMM")
                                targetItem = dbLcsData.FirstOrDefault(x => x.Name == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue));
                            else
                            {
                                ClientOM.FieldLookupValue _ProjectLifeCycle = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                long id = UGITUtility.StringToLong(lifecyclelist.GetTargetID(Convert.ToString(_ProjectLifeCycle.LookupId)));
                                targetItem = dbLcsData.FirstOrDefault(x => x.Name == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == moduleName && x.StageWeight == Convert.ToInt32(item[SPDatabaseObjects.Columns.StageWeight]) && x.LifeCycleName == id);
                            }

                            if (import)
                            {
                                if (targetItem == null)
                                {
                                    targetItem = new LifeCycleStage();
                                    dbLcsData.Add(targetItem);
                                    targetNewItemCount++;
                                }

                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    try
                                    {
                                        if (moduleName == "PMM")
                                        {
                                            targetItem.StageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                            targetItem.Name = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                            targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                            targetItem.StageWeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageWeight]);
                                            targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]);
                                            //targetItem.StageApprovedStatus = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]) + 1;
                                            //targetItem.StageApproveButtonName = "Approved";
                                            //int i = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]) - 1;
                                            //if (i > 0)
                                            //{
                                            //    targetItem.StageReturnStatus = i;
                                            //    targetItem.StageReturnButtonName = "Return";
                                            //}
                                            ClientOM.FieldLookupValue lookupid = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                            targetItem.LifeCycleName = UGITUtility.StringToLong(lifecyclelist.GetTargetID(Convert.ToString(lookupid.LookupId)));
                                            targetItem.ModuleNameLookup = "PMM";
                                        }
                                        else
                                        {
                                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                            targetItem.StageWeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageWeight]);
                                            targetItem.StageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.StageTitle]);
                                            targetItem.Name = Convert.ToString(item[SPDatabaseObjects.Columns.StageTitle]);
                                            targetItem.ModuleNameLookup = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                            targetItem.StageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]);
                                            targetItem.Action = Convert.ToString(item[SPDatabaseObjects.Columns.Action]);
                                            targetItem.ActionUser = Convert.ToString(item[SPDatabaseObjects.Columns.ActionUser]);
                                            targetItem.DataEditors = Convert.ToString(item[SPDatabaseObjects.Columns.DataEditor]);
                                            targetItem.UserPrompt = Convert.ToString(item[SPDatabaseObjects.Columns.UserPrompt]);
                                            targetItem.ShortStageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.ShortStageTitle]);
                                            targetItem.SkipOnCondition = Convert.ToString(item[SPDatabaseObjects.Columns.SkipOnCondition]);
                                            targetItem.ApproveActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveActionDescription]);
                                            targetItem.RejectActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.RejectActionDescription]);
                                            targetItem.ReturnActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnActionDescription]);
                                            targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                            targetItem.EnableCustomReturn = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EnableCustomReturn]);
                                            targetItem.ApproveIcon = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveIcon]);
                                            targetItem.ReturnIcon = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnIcon]);
                                            targetItem.RejectIcon = Convert.ToString(item[SPDatabaseObjects.Columns.RejectIcon]);
                                            targetItem.SelectedTabNumber = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.SelectedTabNumber]);
                                            targetItem.ApproveButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveButtonTooltip]);
                                            targetItem.RejectButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.RejectButtonTooltip]);
                                            targetItem.ReturnButtonTooltip = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnButtonTooltip]);
                                            targetItem.StageCapacityNormal = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StageCapacityNormal]);
                                            targetItem.StageCapacityMax = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StageCapacityMax]);
                                            targetItem.AllowReassignFromList = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AllowReassignFromList]);
                                            targetItem.DisableAutoApprove = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisableAutoApprove]);
                                            targetItem.AutoApproveOnStageTasks = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.AutoApproveOnStageTasks]);
                                            targetItem.StageWeight = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.StageWeight]);
                                            targetItem.ShowBaselineButtons = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowBaselineButtons]);
                                            targetItem.StageAllApprovalsRequired = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.StageAllApprovalsRequired]);
                                            if (item[SPDatabaseObjects.Columns.StageTypeLookup] != null)
                                                targetItem.StageTypeChoice = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageTypeLookup]).LookupValue);
                                            targetItem.UserWorkflowStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UserWorkflowStatus]);
                                            targetItem.StageReturnButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageReturnButtonName]);
                                            targetItem.StageRejectedButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageRejectedButtonName]);
                                            targetItem.StageApproveButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageApproveButtonName]);
                                            if (item[SPDatabaseObjects.Columns.StageApprovedStatus] != null)
                                                targetItem.StageApprovedStatus = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageApprovedStatus]).LookupId);
                                            if (item[SPDatabaseObjects.Columns.StageRejectedStatus] != null)
                                                targetItem.StageRejectedStatus = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageRejectedStatus]).LookupId);
                                            targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);

                                            if (item[SPDatabaseObjects.Columns.StageReturnStatus] != null)
                                                targetItem.StageReturnStatus = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageReturnStatus]).LookupId);
                                            if (!string.IsNullOrWhiteSpace(targetItem.ActionUser) && moduleColumnMapped != null)
                                                targetItem.ActionUser = context.CovertActionUserTypes(targetItem.ActionUser, moduleColumnMapped);
                                            targetItem.DataEditors = Convert.ToString(item[SPDatabaseObjects.Columns.DataEditor]);
                                            if (!string.IsNullOrWhiteSpace(targetItem.DataEditors) && moduleColumnMapped != null)
                                                targetItem.DataEditors = context.CovertActionUserTypes(targetItem.DataEditors, moduleColumnMapped);
                                            //if (item[SPDatabaseObjects.Columns.StageApprovedStatus] != null)
                                            //    if (!htStagedtl.ContainsKey(Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageApprovedStatus]).LookupId)))
                                            //        htStagedtl.Add(Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageApprovedStatus]).LookupId), Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageApprovedStatus]).LookupValue));

                                            htStagedtl.Add(item["ID"], UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]));



                                        }
                                        ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                        if (users != null && userlist != null)
                                            targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                        ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                        if (Modifeduser != null && userlist != null)
                                            targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                        targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                        targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }

                                    //if (moduleName != "PMM")
                                    //{
                                    //    targetItem.ModuleNameLookup = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                    //    targetItem.ApproveActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ApproveActionDescription]);
                                    //    targetItem.StageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.StageTitle]);
                                    //    targetItem.Action = Convert.ToString(item[SPDatabaseObjects.Columns.Action]);
                                    //    targetItem.ActionUser = Convert.ToString(item[SPDatabaseObjects.Columns.ActionUser]);
                                    //    targetItem.SkipOnCondition = Convert.ToString(item[SPDatabaseObjects.Columns.SkipOnCondition]);
                                    //    targetItem.UserPrompt = Convert.ToString(item[SPDatabaseObjects.Columns.UserPrompt]);
                                    //    targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                    //    targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                    //    targetItem.UserWorkflowStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UserWorkflowStatus]);
                                    //    targetItem.StageApproveButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageApproveButtonName]);
                                    //    targetItem.StageRejectedButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageRejectedButtonName]);
                                    //    targetItem.StageReturnButtonName = Convert.ToString(item[SPDatabaseObjects.Columns.StageReturnButtonName]);
                                    //    if ((item[SPDatabaseObjects.Columns.StageReturnStatus] != null) && (moduleName == "NPR" || moduleName == "TSR" || moduleName == "TSK" || moduleName == "ACR" || moduleName == "SVC") || moduleName == "APP")
                                    //        targetItem.StageReturnStatus = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageReturnStatus]).LookupId);
                                    //    else
                                    //        targetItem.StageReturnStatus = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageReturnStatus]);
                                    //    targetItem.AutoApproveOnStageTasks = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoApproveOnStageTasks]);
                                    //    targetItem.DisableAutoApprove = Convert.ToBoolean(item[SPDatabaseObjects.Columns.DisableAutoApprove]);
                                    //    targetItem.ShortStageTitle = Convert.ToString(item[SPDatabaseObjects.Columns.ShortStageTitle]);
                                    //    targetItem.RejectActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.RejectActionDescription]);
                                    //    targetItem.ReturnActionDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ReturnActionDescription]);
                                    //    targetItem.StageApprovedStatus = item[SPDatabaseObjects.Columns.StageApprovedStatus] == null ? 0 : Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageApprovedStatus]).LookupId);
                                    //    targetItem.StageAllApprovalsRequired = Convert.ToBoolean(item[SPDatabaseObjects.Columns.StageAllApprovalsRequired]);
                                    //    targetItem.StageCapacityMax = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageCapacityMax]);
                                    //    targetItem.StageCapacityNormal = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageCapacityNormal]);
                                    //    if (item[SPDatabaseObjects.Columns.StageTypeLookup] != null)
                                    //        targetItem.StageTypeLookup = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageTypeLookup]).LookupId);
                                    //    targetItem.ShowBaselineButtons = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ShowBaselineButtons]);
                                    //    targetItem.SelectedTabNumber = Convert.ToInt32(item[SPDatabaseObjects.Columns.SelectedTabNumber]);
                                    //}
                                    //if (moduleName == "PMM")
                                    //{
                                    //    ClientOM.FieldLookupValue soruceReqeustTypeLookup = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                    //    if (soruceReqeustTypeLookup == null || soruceReqeustTypeLookup.LookupId == 0)
                                    //        continue;

                                    //    targetItem.LifeCycleName = UGITUtility.StringToLong(lifecyclelist.GetTargetID(soruceReqeustTypeLookup.LookupId.ToString()));
                                    //    targetItem.ModuleNameLookup = "PMM";
                                    //}
                                }
                                if (targetItem.ID > 0)
                                    lcsMgr.Update(targetItem);
                                else
                                    lcsMgr.Insert(targetItem);
                            }



                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    }
                } while (position != null);

                List<LifeCycleStage> _dbLcsData = lcsMgr.Load(x => x.ModuleNameLookup == moduleName);
                foreach (var _item in _dbLcsData)
                {
                    if (_item.StageApprovedStatus != 0 && htStagedtl.ContainsKey(_item.StageApprovedStatus))
                        _item.StageApprovedStatus = UGITUtility.StringToInt(htStagedtl[_item.StageApprovedStatus]); // _dbLcsData.Where(x => x.StageTitle == Convert.ToString(htStagedtl[_item.StageApprovedStatus])).FirstOrDefault().StageStep;
                    if (_item.StageReturnStatus != 0 && htStagedtl.ContainsKey(_item.StageReturnStatus))
                        _item.StageReturnStatus = UGITUtility.StringToInt(htStagedtl[_item.StageReturnStatus]);//_dbLcsData.Where(x => x.StageTitle == Convert.ToString(htStagedtl[_item.StageReturnStatus])).FirstOrDefault().StageStep;
                    if (_item.StageRejectedStatus != 0 && htStagedtl.ContainsKey(_item.StageRejectedStatus))
                        _item.StageRejectedStatus = UGITUtility.StringToInt(htStagedtl[_item.StageRejectedStatus]); //_dbLcsData.Where(x => x.StageTitle == Convert.ToString(htStagedtl[_item.StageRejectedStatus])).FirstOrDefault().StageStep;

                }
                lcsMgr.UpdateItems(_dbLcsData);
                if (import)
                    ULog.WriteLog($"{targetNewItemCount} ProjectLifecycles Stages added");

            }


        }
        public override void UpdateGenericStausMapping()
        {
            base.UpdateGenericStausMapping();
            bool import = context.IsImportEnable("GenericStatus");
            if (!import)
                return;

            ULog.WriteLog($"Updating GenericStaus Mapping");

            if (import && deleteBeforeImport)
            {
                try
                {
                    GetTableDataManager.ExecuteQuery($"Delete from {DatabaseObjects.Tables.TicketStatusMapping} where {DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'");
                }
                catch (Exception ex)
                {
                    ULog.WriteLog("Problem while deleting records");
                    ULog.WriteException(ex);
                }
            }
            int targetNewItemCount = 0;
            System.Data.DataTable dbData = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.TicketStatusMapping);
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GenericTicketStatus, $"{DatabaseObjects.Columns.TenantID} = '{context.AppContext.TenantID}'");

            MappedItemList genricstauslist = context.GetMappedList(DatabaseObjects.Tables.TicketStatusMapping);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                string listName = SPDatabaseObjects.Lists.TicketStatusMapping;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;
                    System.Data.DataRow dr = null;
                    foreach (ClientOM.ListItem item in collection)
                    {
                        if (dbData != null || dbData.Rows.Count == 0)
                        {
                            dr = dbData.NewRow();
                            dr[DatabaseObjects.Columns.Title] = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            ClientOM.FieldLookupValue _GenericStatusLookup = item[SPDatabaseObjects.Columns.GenericStatusLookup] as ClientOM.FieldLookupValue;
                            if (_GenericStatusLookup != null && !string.IsNullOrEmpty(_GenericStatusLookup.LookupValue))
                            {
                                DataRow[] genericTicketStatus = dt.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, Convert.ToString(_GenericStatusLookup.LookupValue)));
                                if (genericTicketStatus.Length > 0)
                                    dr[DatabaseObjects.Columns.GenericStatusLookup] = UGITUtility.StringToInt(genericTicketStatus[0][DatabaseObjects.Columns.ID]);
                            }


                            //dr[DatabaseObjects.Columns.GenericStatusLookup] = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.GenericStatusLookup);
                            dr[DatabaseObjects.Columns.StageTitleLookup] = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.StageTitleLookup);
                            dr[DatabaseObjects.Columns.ModuleNameLookup] = moduleName;
                            dr[DatabaseObjects.Columns.TenantID] = context.AppContext.TenantID;
                            dr[DatabaseObjects.Columns.Created] = DateTime.Now;
                            dr[DatabaseObjects.Columns.Modified] = DateTime.Now;
                            dr[DatabaseObjects.Columns.Deleted] = false;
                            dr[DatabaseObjects.Columns.CreatedByUser] = context.Tenant.CreatedByUser;
                            dr[DatabaseObjects.Columns.ModifiedByUser] = context.Tenant.ModifiedByUser;
                            dbData.Rows.Add(dr);
                        }
                    }
                    if (import)
                    {
                        GetTableDataManager.bulkupload(dbData, DatabaseObjects.Tables.TicketStatusMapping);
                        targetNewItemCount = dbData.Rows.Count;
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} GenericStatus Mapping added");
            }

        }
        public override void UpdateModuleColumns()
        {
            base.UpdateModuleColumns();
            bool import = context.IsImportEnable("ModuleColumns", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating modulecolumns- script will execute via sql");
            ULog.WriteLog($"Updating columns");
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                string listName = SPDatabaseObjects.Lists.ModuleColumns;
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string fieldName = string.Empty;
                ModuleColumn targetItem = null;
                string newfieldname = string.Empty;
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                        if (moduleColumnMapped != null && moduleColumnMapped.ContainsKey(fieldName))
                        {
                            fieldName = moduleColumnMapped[fieldName];
                            targetItem = dbData.FirstOrDefault(x => x.FieldName == fieldName);
                        }
                        else
                            targetItem = null;

                        if (targetItem == null)
                        {
                            targetItem = new ModuleColumn();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = moduleName + "-" + UGITUtility.SplitString(Convert.ToString(item[SPDatabaseObjects.Columns.Title]), "-")[1];
                            targetItem.CategoryName = moduleName;
                            targetItem.FieldSequence = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.FieldSequence]);
                            targetItem.FieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                            if (moduleColumnMapped != null && moduleColumnMapped.ContainsKey(targetItem.FieldName))
                                targetItem.FieldName = moduleColumnMapped[targetItem.FieldName];

                            targetItem.FieldDisplayName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldDisplayName]);
                            targetItem.SortOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.SortOrder]);
                            targetItem.IsDisplay = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDisplay]);
                            targetItem.ShowInMobile = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowInMobile]);
                            targetItem.DisplayForClosed = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisplayForClosed]);
                            targetItem.IsUseInWildCard = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsUseInWildCard]);
                            targetItem.DisplayForReport = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.DisplayForReport]);
                            targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                            targetItem.ColumnType = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnType]);
                            targetItem.IsAscending = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsAscending]);
                            targetItem.TextAlignment = Convert.ToString(item[SPDatabaseObjects.Columns.TextAlignment]);
                            targetItem.TruncateTextTo = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TruncateTextTo]);
                            targetItem.SelectedTabs = Convert.ToString(item[SPDatabaseObjects.Columns.SelectedTabs]);
                            //targetItem.ShowInCardView = Convert.ToBoolean(item[DatabaseObjects.Columns.ShowInCardView]);
                        }

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);

                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} columns added");
            }
        }

        public override void UpdateRequestTypes()
        {
            if (moduleName == "WIK")
                moduleName = ModuleNames.WIKI;
            base.UpdateRequestTypes();
            bool import = context.IsImportEnable("RequestTypes", moduleName);

            if (import)
                ULog.WriteLog($"updating request types");
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

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {

                string listName = SPDatabaseObjects.Lists.RequestType;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                //MappedItemList targetStagetMappedList = new MappedItemList(listName);
                MappedItemList targetStagetMappedList = context.GetMappedList(listName);
                {
                    RequestTypeManager mgr = new RequestTypeManager(context.AppContext);
                    List<ModuleRequestType> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);


                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;

                    string category = string.Empty, subCategory = string.Empty, requestType = string.Empty;
                    ModuleRequestType targetItem = null;

                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    do
                    {
                        if (moduleName == "WIKI")
                            moduleName = "WIK";
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            category = Convert.ToString(item[SPDatabaseObjects.Columns.Category]);
                            subCategory = Convert.ToString(item[SPDatabaseObjects.Columns.RequestTypeSubCategory]);
                            requestType = Convert.ToString(item[SPDatabaseObjects.Columns.TicketRequestType]);
                            targetItem = null;
                            //targetItem = dbData.FirstOrDefault(x => x.Category == category && x.SubCategory == subCategory && x.RequestType == requestType);
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    if (moduleName == "WIK")
                                        moduleName = "WIKI";
                                    targetItem.ModuleNameLookup = moduleName;
                                    targetItem.Category = Convert.ToString(item[SPDatabaseObjects.Columns.Category]);
                                    targetItem.SubCategory = Convert.ToString(item[SPDatabaseObjects.Columns.RequestTypeSubCategory]);
                                    targetItem.RequestType = Convert.ToString(item[SPDatabaseObjects.Columns.TicketRequestType]);
                                    targetItem.RequestCategory = Convert.ToString(item[SPDatabaseObjects.Columns.RequestCategory]);
                                    targetItem.WorkflowType = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowType]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.RequestTypeDescription]);
                                    targetItem.KeyWords = Convert.ToString(item[SPDatabaseObjects.Columns.KeyWords]);
                                    targetItem.EstimatedHours = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.UGITEstimatedHours]);
                                    targetItem.ServiceWizardOnly = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ServiceWizardOnly]);
                                    targetItem.OutOfOffice = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.OutOfOffice]);
                                    targetItem.AssignmentSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.AssignmentSLA]);
                                    targetItem.ResolutionSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ResolutionSLA]);
                                    targetItem.CloseSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.CloseSLA]);
                                    targetItem.RequestorContactSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.RequestorContactSLA]);
                                    targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                    targetItem.ResolutionTypes = Convert.ToString(item[SPDatabaseObjects.Columns.ResolutionTypes]);
                                    targetItem.SortToBottom = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.SortToBottom]);
                                    targetItem.IssueTypeOptions = Convert.ToString(item[SPDatabaseObjects.Columns.IssueTypeOptions]);
                                    targetItem.StagingId = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StagingId]);
                                    targetItem.AutoAssignPRP = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EmailToTicketSender]);
                                    targetItem.SLADisabled = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.SLADisabled]);
                                    targetItem.Use24x7Calendar = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.Use24x7Calendar]);
                                    targetItem.Owner = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeOwner);
                                    targetItem.EscalationManager = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeEscalationManager);
                                    targetItem.BackupEscalationManager = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeBackupEscalationManager);
                                    targetItem.PRPGroup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.PRPGroup);
                                    targetItem.ORP = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketORP);

                                    ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.RequestTypeOwner] as ClientOM.FieldUserValue[];
                                    if (_users != null && _users.Length > 0 && userList != null)
                                        targetItem.Owner = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                    targetItem.AppReferenceInfo = Convert.ToString(item[SPDatabaseObjects.Columns.AppReferenceInfo]);

                                    long id = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.BudgetIdLookup);
                                    if (id > 0)
                                        targetItem.BudgetIdLookup = id;
                                    targetItem.FunctionalAreaLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.FunctionalAreaLookup);


                                    targetItem.APPTitleLookup = null;
                                    string appTitleLookupValue = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.APPTitleLookup);
                                    if (!string.IsNullOrWhiteSpace(appTitleLookupValue))
                                        targetItem.APPTitleLookup = UGITUtility.StringToInt(appTitleLookupValue);

                                    targetItem.ApplicationModulesLookup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.ApplicationModulesLookup);

                                    targetItem.TaskTemplateLookup = null;
                                    string taskTemplateLookupValue = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.APPTitleLookup);
                                    if (!string.IsNullOrWhiteSpace(taskTemplateLookupValue))
                                        targetItem.TaskTemplateLookup = UGITUtility.StringToInt(taskTemplateLookupValue);

                                    targetItem.MatchAllKeywords = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.MatchAllKeywords]);
                                    targetItem.BreakFix = Convert.ToString(item[SPDatabaseObjects.Columns.BreakFix]);
                                }
                                if (targetItem.ID > 0)
                                {
                                    mgr.Update(targetItem);
                                }
                                else
                                {
                                    mgr.Insert(targetItem);
                                }
                                if (targetItem != null)
                                    targetStagetMappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                            }


                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} request type added");
                }


                {
                    RequestTypeByLocationManager mgr = new RequestTypeByLocationManager(context.AppContext);
                    List<ModuleRequestTypeLocation> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                    listName = SPDatabaseObjects.Lists.RequestTypeByLocation;
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;

                    ModuleRequestTypeLocation targetItem = null;
                    MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    do
                    {
                        if (moduleName == "WIKI")
                            moduleName = "WIK";
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, listName, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {


                            targetItem = null;
                            //targetItem = dbData.FirstOrDefault(x => x.RequestTypeLookup == targetRequestTypeID && x.LocationLookup == targetLocationID);
                            if (import)
                            {
                                if (targetItem == null)
                                {
                                    targetItem = new ModuleRequestTypeLocation();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }

                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    ClientOM.FieldLookupValue soruceReqeustTypeLookup = item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] as ClientOM.FieldLookupValue;
                                    long targetRequestTypeID = UGITUtility.StringToLong(targetStagetMappedList.GetTargetID(soruceReqeustTypeLookup.LookupId.ToString()));
                                    if (targetRequestTypeID > 0)
                                        targetItem.RequestTypeLookup = targetRequestTypeID;

                                    ClientOM.FieldLookupValue sourceLocationLookup = item[SPDatabaseObjects.Columns.LocationLookup] as ClientOM.FieldLookupValue;

                                    long targetLocationID = 0;
                                    if (locationMappedList != null)
                                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(sourceLocationLookup.LookupId.ToString()));

                                    if (targetLocationID > 0)
                                        targetItem.LocationLookup = targetLocationID;

                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ModuleNameLookup = moduleName;
                                    targetItem.AssignmentSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.AssignmentSLA]);
                                    targetItem.ResolutionSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ResolutionSLA]);
                                    targetItem.CloseSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.CloseSLA]);
                                    targetItem.RequestorContactSLA = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.RequestorContactSLA]);
                                    targetItem.Owner = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeOwner);
                                    targetItem.EscalationManager = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeEscalationManager);
                                    targetItem.BackupEscalationManager = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.RequestTypeBackupEscalationManager);
                                    targetItem.PRPGroup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.PRPGroup);
                                    targetItem.ORP = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketORP);
                                }

                                //not found
                                //targetItem.PRP = Convert.ToString(item[SPDatabaseObjects.Columns.PRP]);
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} request types by location added");
                }
            }
        }

        public override void UpdateFormLayoutAndAccess()
        {
            base.UpdateFormLayoutAndAccess();
            bool import = context.IsImportEnable("FormLayoutAndAccess", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating formtabs");
            string listName = SPDatabaseObjects.Lists.ModuleFormTab;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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


                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string tabName = string.Empty;
                ModuleFormTab targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;
                    try
                    {
                        foreach (ClientOM.ListItem item in collection)
                        {
                            //tabName = Convert.ToString(item[SPDatabaseObjects.Columns.TabName]);
                            //targetItem = dbData.FirstOrDefault(x => x.TabName == tabName);
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleFormTab();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.Title])))
                                    targetItem.Title = moduleName + "-" + UGITUtility.SplitString(Convert.ToString(item[SPDatabaseObjects.Columns.Title]), "-")[1];
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.TabSequence = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TabSequence]);
                                targetItem.TabId = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TabId]);
                                targetItem.TabName = Convert.ToString(item[SPDatabaseObjects.Columns.TabName]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                targetItem.ShowInMobile = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowInMobile]);
                                targetItem.AuthorizedToView = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.AuthorizedToView);
                                targetItem.AuthorizedToEdit = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.AuthorizedToEdit);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} formtabs added");

            }
            //ULog.WriteLog($"Updating Formlayout- script will execute via sql ");
            ULog.WriteLog($"Updating Formlayout");
            listName = SPDatabaseObjects.Lists.FormLayout;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {

                ModuleFormLayoutManager mgr = new ModuleFormLayoutManager(context.AppContext);
                List<ModuleFormLayout> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                int tabID = 0;
                string fieldName = string.Empty;
                ModuleFormLayout targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        tabID = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TabId]);
                        fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                        targetItem = null;
                        //targetItem = dbData.FirstOrDefault(x => x.TabId == tabID && x.FieldName == fieldName && x.FieldDisplayName == Convert.ToString(item[SPDatabaseObjects.Columns.FieldDisplayName]));
                        if (targetItem == null)
                        {
                            targetItem = new ModuleFormLayout();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }
                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.ModuleNameLookup = moduleName;
                            targetItem.TabId = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TabId]);
                            targetItem.FieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                            if (moduleColumnMapped != null && moduleColumnMapped.ContainsKey(targetItem.FieldName))
                                targetItem.FieldName = moduleColumnMapped[targetItem.FieldName];
                            targetItem.FieldSequence = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.FieldSequence]);
                            targetItem.FieldDisplayWidth = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.FieldDisplayWidth]);
                            targetItem.FieldDisplayName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldDisplayName]);
                            targetItem.ShowInMobile = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowInMobile]);
                            targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                            //targetItem.FieldType = Convert.ToString(item["UGITFieldType"]);
                            targetItem.SkipOnCondition = Convert.ToString(item[SPDatabaseObjects.Columns.SkipOnCondition]);
                            targetItem.HideInTemplate = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.HideInTicketTemplate]);
                            targetItem.TrimContentAfter = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TrimContentAfter]);
                            targetItem.TargetURL = Convert.ToString(item[SPDatabaseObjects.Columns.TargetURL]);
                            targetItem.Tooltip = Convert.ToString(item[SPDatabaseObjects.Columns.ToolTip]);
                            targetItem.TargetType = Convert.ToString(item[SPDatabaseObjects.Columns.TargetType]);
                            targetItem.ColumnType = Convert.ToString(item[SPDatabaseObjects.Columns.ColumnType]);
                        }
                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);

                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} Formlayout added");
            }

            ULog.WriteLog($"Updating rolewriteaccess - script will execute via sql");
            listName = SPDatabaseObjects.Lists.RequestRoleWriteAccess;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                RequestRoleWriteAccessManager mgr = new RequestRoleWriteAccessManager(context.AppContext);
                List<ModuleRoleWriteAccess> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                int stageStep = 0;
                string fieldName = string.Empty;
                ModuleRoleWriteAccess targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        stageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]);
                        fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                        targetItem = dbData.FirstOrDefault(x => x.StageStep == stageStep && x.FieldName == fieldName);
                        if (targetItem == null)
                        {
                            targetItem = new ModuleRoleWriteAccess();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = moduleName + "-" + UGITUtility.SplitString(Convert.ToString(item[SPDatabaseObjects.Columns.Title]), "-")[1];
                            targetItem.ModuleNameLookup = moduleName;
                            targetItem.FieldName = Convert.ToString(item[SPDatabaseObjects.Columns.FieldName]);
                            if (moduleColumnMapped != null && moduleColumnMapped.ContainsKey(targetItem.FieldName))
                                targetItem.FieldName = moduleColumnMapped[targetItem.FieldName];
                            targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                            targetItem.ActionUser = Convert.ToString(item[SPDatabaseObjects.Columns.ActionUser]);
                            targetItem.StageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ModuleStep]);
                            targetItem.FieldMandatory = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.FieldMandatory]);
                            targetItem.ShowEditButton = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowEditButton]);
                            targetItem.ShowWithCheckbox = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.ShowWithCheckBox]);
                            targetItem.HideInServiceMapping = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.HideInServiceMapping]);

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }

                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} rolewriteaccess added");
            }
        }

        public override void UpdatePriorityMap()
        {
            base.UpdatePriorityMap();
            bool import = context.IsImportEnable("PriorityMap", moduleName);

            if (import)
                ULog.WriteLog($"Updating Impact");
            else
                ULog.WriteLog($"Load impact mapping");

            string listName = SPDatabaseObjects.Lists.TicketImpact;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                ImpactManager mgr = new ImpactManager(context.AppContext);
                List<ModuleImpact> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList = context.GetMappedList(listName);

                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string impact = string.Empty;
                ModuleImpact targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        //impact = Convert.ToString(item[SPDatabaseObjects.Columns.Impact]);
                        //targetItem = dbData.FirstOrDefault(x => x.Impact == impact);
                        targetItem = null;
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);

                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Impact = Convert.ToString(item[SPDatabaseObjects.Columns.Impact]);
                                targetItem.ItemOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }

                        }

                        if (targetItem != null)
                            mappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} impact added");

            }


            if (import)
                ULog.WriteLog($"Updating severity");
            else
                ULog.WriteLog($"Load severity mapping");

            listName = SPDatabaseObjects.Lists.TicketSeverity;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                SeverityManager mgr = new SeverityManager(context.AppContext);
                List<ModuleSeverity> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList1 = context.GetMappedList(listName);

                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string severity = string.Empty;
                ModuleSeverity targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        severity = Convert.ToString(item[SPDatabaseObjects.Columns.Severity]);
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
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Severity = Convert.ToString(item[SPDatabaseObjects.Columns.Severity]);
                                targetItem.ItemOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }

                        if (targetItem != null)
                            mappedList1.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                    }
                } while (position != null);

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} severity added");

            }


            if (import)
                ULog.WriteLog($"Updating priority");
            else
                ULog.WriteLog($"Load priority mapping");

            listName = SPDatabaseObjects.Lists.TicketPriority;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                PrioirtyViewManager mgr = new PrioirtyViewManager(context.AppContext);
                List<ModulePrioirty> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                MappedItemList mappedList2 = context.GetMappedList(listName);

                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;

                string uPriority = string.Empty;
                ModulePrioirty targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        uPriority = Convert.ToString(item[SPDatabaseObjects.Columns.UPriority]);
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
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.uPriority = uPriority;
                                targetItem.ItemOrder = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Deleted = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                targetItem.NotifyInPlainText = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.NotifyInPlainText]);
                                targetItem.IsVIP = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsVIP]);
                                targetItem.EmailIDTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDTo]);
                                targetItem.Color = Convert.ToString(item[SPDatabaseObjects.Columns.Color]);

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                            if (targetItem != null)
                                mappedList2.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }


                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} priority added");

            }

            if (import)
            {
                ULog.WriteLog($"Updating priority, severity, impact mapping");
                listName = SPDatabaseObjects.Lists.RequestPriority;
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    RequestPriorityManager mgr = new RequestPriorityManager(context.AppContext);
                    List<ModulePriorityMap> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);


                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0;

                    long priorityLookup = 0, severityLookup = 0, impactLookup = 0;
                    ModulePriorityMap targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            priorityLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup);
                            severityLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.TicketSeverityLookup);
                            impactLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.TicketImpactLookup);
                            //targetItem = dbData.FirstOrDefault(x => x.PriorityLookup == priorityLookup && x.SeverityLookup == severityLookup && x.ImpactLookup == impactLookup);
                            targetItem = null;
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
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} priority added");
                }
            }
        }

        public override void UpdateTicketData()
        {
            base.UpdateTicketData();
            if (moduleName == ModuleNames.ITG || moduleName == ModuleNames.RMM || moduleName == ModuleNames.WIK || moduleName == ModuleNames.WIKI)
                importData = false;
            if (!importData)
                return;


            ULog.WriteLog($"Updating ticket data " + moduleName);
            try
            {
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    TicketManager ticketMgr = new TicketManager(context.AppContext);
                    ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                    UGITModule module = moduleMgr.LoadByName(moduleName, false);
                    DataTable ticketSchema = ticketMgr.GetDatabaseTableSchema(module.ModuleTable);
                    DocumentManager mgr = new DocumentManager(context.AppContext);
                    List<Document> docData = mgr.Load();
                    ClientOM.List moduleList = spContext.Web.Lists.GetByTitle(SPDatabaseObjects.Lists.Modules);
                    ClientOM.ListItemCollection moduleCollection = ContextHelper.GetItemCollectionList(spContext, moduleList, string.Format("<Eq><FieldRef Name='ModuleName'/><Value Type='Text'>{0}</Value></Eq>", moduleName), string.Format("<FieldRef Name='{0}'/>", SPDatabaseObjects.Columns.ModuleTicketTable), null);
                    if (moduleCollection.Count == 0)
                    {
                        ULog.WriteException($"Module {moduleName} is not found in sharepoint");
                        return;
                    }
                    string spModuleTable = Convert.ToString(moduleCollection[0][SPDatabaseObjects.Columns.ModuleTicketTable]);
                    if (string.IsNullOrWhiteSpace(spModuleTable))
                    {
                        ULog.WriteException($"moduletable is found for module {moduleName} in sharepoint");
                        return;
                    }

                    ClientOM.List list = spContext.Web.Lists.GetByTitle(spModuleTable);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0; byte[] imageArray = null; int _targetNewItemCount = 0;
                    MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
                    MappedItemList priorityMappedList = context.GetMappedList(SPDatabaseObjects.Lists.TicketPriority);
                    MappedItemList RequestTypeMappedList = context.GetMappedList(SPDatabaseObjects.Lists.RequestType);
                    MappedItemList AssetMappedList = context.GetMappedList(SPDatabaseObjects.Lists.AssetVendors);
                    MappedItemList ImpactMappedList = context.GetMappedList(SPDatabaseObjects.Lists.TicketImpact);
                    MappedItemList FunctionAreaMappedList = context.GetMappedList(SPDatabaseObjects.Lists.FunctionalAreas);
                    MappedItemList ModuleStagesMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList DepartmentList = context.GetMappedList(SPDatabaseObjects.Lists.Department);
                    MappedItemList TicketSeverityList = context.GetMappedList(SPDatabaseObjects.Lists.TicketSeverity);
                    MappedItemList targetStagetMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                    MappedItemList targetProjectInitiativeList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectInitiative);
                    MappedItemList targetProjectclassList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectClass);
                    MappedItemList targetProjectlifecycleList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectLifeCycles);
                    MappedItemList companylist = context.GetMappedList(SPDatabaseObjects.Lists.Company);
                    MappedItemList servicelist = context.GetMappedList(SPDatabaseObjects.Lists.Services);
                    MappedItemList divsionlist = context.GetMappedList(SPDatabaseObjects.Lists.CompanyDivisions);
                    MappedItemList acrlist = context.GetMappedList(SPDatabaseObjects.Lists.ACRTypes);
                    MappedItemList assetmodelList = context.GetMappedList(SPDatabaseObjects.Lists.AssetModels);
                    MappedItemList assetvendorList = context.GetMappedList(SPDatabaseObjects.Lists.AssetVendors);
                    MappedItemList drqlist = context.GetMappedList(SPDatabaseObjects.Lists.DRQRapidTypes);
                    MappedItemList drqSystemArealist = context.GetMappedList(SPDatabaseObjects.Lists.DRQSystemAreas);
                    MappedItemList appmappedList = context.GetMappedList(SPDatabaseObjects.Lists.Applications);



                    do
                    {
                        try
                        {
                            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                            position = collection.ListItemCollectionPosition;

                            string targetVal = string.Empty;
                            foreach (ClientOM.ListItem item in collection)
                            {
                                DataRow targetItem = null;
                                string ticketID = string.Empty; long targetLocationID = 0; long targetPriorityID = 0; long targetRequestTypeID = 0;
                                long targetAssetId = 0; long targetImpactId = 0; long FunctonalAreaId = 0; long ModuleStepId = 0; long DepartmentId = 0;
                                string Requesttype = string.Empty; long TicketSeverityId = 0; string stageids = string.Empty; long projectClass = 0;
                                long projectinitative = 0; long acrtypeid = 0;
                                //long serviceid = 0; 
                                long targetlookupID = 0;
                                //ModulePrioirty currentpriority = null; 
                                long vid = 0;
                                Document doctargetItem = null;
                                ticketID = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem = ticketMgr.GetByTicketID(module, ticketID);

                                if (moduleName == ModuleNames.TSR)
                                {
                                    if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (AssetMappedList != null && item[SPDatabaseObjects.Columns.AssetLookup] != null)
                                    {
                                        if (item[SPDatabaseObjects.Columns.AssetLookup].ToString() != "Microsoft.SharePoint.Client.FieldLookupValue[]")
                                            targetAssetId = UGITUtility.StringToLong(AssetMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.AssetLookup]).LookupId.ToString()));
                                    }

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetLocationID > 0)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            if (targetAssetId > 0)
                                                targetItem[DatabaseObjects.Columns.AssetLookup] = targetAssetId;
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? true : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.ElevatedPriority] = item[SPDatabaseObjects.Columns.ElevatedPriority] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ElevatedPriority];
                                            targetItem[DatabaseObjects.Columns.TicketGLCode] = item[SPDatabaseObjects.Columns.TicketGLCode];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            targetItem[DatabaseObjects.Columns.TicketInitiatorResolved] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketInitiatorResolved]);
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];

                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);


                                            targetItem[DatabaseObjects.Columns.TicketResolutionType] = item[SPDatabaseObjects.Columns.TicketResolutionType];
                                            targetItem[DatabaseObjects.Columns.RequestSource] = item[SPDatabaseObjects.Columns.TicketRequestSource];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue _prpgrpusers = item[SPDatabaseObjects.Columns.PRPGroup] as ClientOM.FieldUserValue;
                                            if (_prpgrpusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PRPGroup] = userList.GetTargetID(Convert.ToString(_prpgrpusers.LookupId));

                                            ClientOM.FieldUserValue _infrausers = item[SPDatabaseObjects.Columns.TicketInfrastructureManager] as ClientOM.FieldUserValue;
                                            if (_infrausers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInfrastructureManager] = userList.GetTargetID(Convert.ToString(_infrausers.LookupId));

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                            ClientOM.FieldUserValue[] securityapprover = item[SPDatabaseObjects.Columns.TicketSecurityManager] as ClientOM.FieldUserValue[];
                                            if (securityapprover != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketSecurityManager] = userList.GetTargetIDs(securityapprover.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                //List<string> steps = new List<string>();
                                                //List<string> stagesIds = new List<string>();
                                                //steps = stageids.Split(',').ToList();
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;// targetStagetMappedList.GetTargetIDs(steps);
                                                                                                                  //foreach (string id in steps)
                                                                                                                  //{
                                                                                                                  //    stagesIds.Add(targetStagetMappedList.GetTargetIDs(steps));
                                                                                                                  //}
                                                                                                                  //targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = ).ToList());
                                            }
                                            //Ticket ticketRequest = new Ticket(context.AppContext, moduleName);
                                            ////LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(targetItem);
                                            //LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(targetItem);
                                            //List<string> stagesIds = new List<string>();
                                            //foreach (LifeCycleStage iterativeStage in lifeCycle.Stages)
                                            //{
                                            //    if (!string.IsNullOrEmpty(iterativeStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(context.AppContext, iterativeStage.SkipOnCondition, targetItem))
                                            //    {
                                            //        if (!string.IsNullOrEmpty(Convert.ToString(iterativeStage.StageStep)))
                                            //        {
                                            //            stagesIds.Add(Convert.ToString(iterativeStage.StageStep));
                                            //        }
                                            //    }
                                            //}

                                            //if (targetItem.Table.Columns.Contains(DatabaseObjects.Columns.WorkflowSkipStages))
                                            //{
                                            //    targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = string.Join(",", stagesIds.ToArray());

                                            //}

                                            // Missing code added for TSR

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];

                                            ClientOM.FieldUserValue busers = item[SPDatabaseObjects.Columns.TicketBusinessManager2] as ClientOM.FieldUserValue;
                                            if (busers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.BusinessManager2User] = userList.GetTargetID(Convert.ToString(busers.LookupId));

                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _DepartmentManagerUser = item[SPDatabaseObjects.Columns.TicketDepartmentManager] as ClientOM.FieldUserValue;
                                            if (_DepartmentManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DepartmentManagerUser] = userList.GetTargetID(Convert.ToString(_DepartmentManagerUser.LookupId));

                                            ClientOM.FieldUserValue _DivisionManagerUSer = item[SPDatabaseObjects.Columns.TicketDivisionManager] as ClientOM.FieldUserValue;
                                            if (_DivisionManagerUSer != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DivisionManagerUser] = userList.GetTargetID(Convert.ToString(_DivisionManagerUSer.LookupId));

                                            targetItem[DatabaseObjects.Columns.PONumber] = item[SPDatabaseObjects.Columns.PONumber];
                                            targetItem[DatabaseObjects.Columns.ProjectCode] = item[SPDatabaseObjects.Columns.ProjectCode];
                                            targetItem[DatabaseObjects.Columns.PackingListNumber] = item[SPDatabaseObjects.Columns.PackingListNumber];
                                            targetItem[DatabaseObjects.Columns.AssetCondition] = item[SPDatabaseObjects.Columns.AssetCondition];

                                            targetItem[DatabaseObjects.Columns.PackingListNumber] = item[SPDatabaseObjects.Columns.PackingListNumber];

                                            ClientOM.FieldLookupValue _VendorLookup = item[SPDatabaseObjects.Columns.VendorLookup] as ClientOM.FieldLookupValue;
                                            if (_VendorLookup != null && assetvendorList != null)
                                                targetItem[DatabaseObjects.Columns.VendorLookup] = assetvendorList.GetTargetID(Convert.ToString(_VendorLookup.LookupId));

                                            targetItem[DatabaseObjects.Columns.PODate] = item[SPDatabaseObjects.Columns.PODate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.PODate];
                                            targetItem[DatabaseObjects.Columns.ActualCost] = Convert.ToDouble(item[SPDatabaseObjects.Columns.ActualCost]);
                                            targetItem[DatabaseObjects.Columns.ChargeBackAmount] = Convert.ToDouble(item[SPDatabaseObjects.Columns.ChargeBackAmount]);
                                            targetItem[DatabaseObjects.Columns.ReceivedOn] = item[SPDatabaseObjects.Columns.ReceivedOn] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ReceivedOn];
                                            targetItem[DatabaseObjects.Columns.WarrantyExpirationDate] = item[SPDatabaseObjects.Columns.WarrantyExpirationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.WarrantyExpirationDate];
                                            targetItem[DatabaseObjects.Columns.QuoteAmount] = Convert.ToInt64(item[SPDatabaseObjects.Columns.QuoteAmount]);
                                            targetItem[DatabaseObjects.Columns.FCRCategorization] = item[SPDatabaseObjects.Columns.FCRCategorization];
                                            targetItem[DatabaseObjects.Columns.AfterHours] = item[SPDatabaseObjects.Columns.AfterHours] == null ? 0 : item[SPDatabaseObjects.Columns.AfterHours];
                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            targetItem[DatabaseObjects.Columns.ReviewRequired] = item[SPDatabaseObjects.Columns.ReviewRequired] == null ? true : item[SPDatabaseObjects.Columns.ReviewRequired];
                                            ClientOM.FieldLookupValue _SubLocationLookup = item[SPDatabaseObjects.Columns.SubLocationLookup] as ClientOM.FieldLookupValue;
                                            if (_SubLocationLookup != null && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.SubLocationLookup] = locationMappedList.GetTargetID(Convert.ToString(_SubLocationLookup.LookupId));

                                            targetItem[DatabaseObjects.Columns.Quantity] = item[SPDatabaseObjects.Columns.Quantity] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.Quantity];
                                            targetItem[DatabaseObjects.Columns.Quantity2] = item[SPDatabaseObjects.Columns.Quantity2] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.Quantity2];

                                            targetItem[DatabaseObjects.Columns.BulkRequestCount] = item[SPDatabaseObjects.Columns.BulkRequestCount] == null ? 0 : item[SPDatabaseObjects.Columns.BulkRequestCount];
                                            targetItem[DatabaseObjects.Columns.BreakFix] = item[SPDatabaseObjects.Columns.BreakFix];
                                            targetItem[DatabaseObjects.Columns.RCADisabled] = item[SPDatabaseObjects.Columns.RCADisabled] == null ? true : item[SPDatabaseObjects.Columns.RCADisabled];
                                            targetItem[DatabaseObjects.Columns.RCARequested] = item[SPDatabaseObjects.Columns.RCARequested] == null ? true : item[SPDatabaseObjects.Columns.RCARequested];

                                            ClientOM.FieldUserValue _ApproverUser = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue;
                                            if (_ApproverUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ApproverUser] = userList.GetTargetID(Convert.ToString(_ApproverUser.LookupId));

                                            ClientOM.FieldUserValue _Approver2User = item[SPDatabaseObjects.Columns.Approver2] as ClientOM.FieldUserValue;
                                            if (_Approver2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.Approver2User] = userList.GetTargetID(Convert.ToString(_Approver2User.LookupId));

                                            ticketSchema.Rows.Add(targetItem);

                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                else if (moduleName == ModuleNames.PMM)
                                {

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));
                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));
                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (targetProjectInitiativeList != null && item[SPDatabaseObjects.Columns.ProjectInitiativeLookup] != null)
                                        projectinitative = UGITUtility.StringToLong(targetProjectInitiativeList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectInitiativeLookup]).LookupId.ToString()));

                                    if (targetProjectclassList != null && item[SPDatabaseObjects.Columns.ProjectClassLookup] != null)
                                        projectClass = UGITUtility.StringToLong(targetProjectclassList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectClassLookup]).LookupId.ToString()));
                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            ClientOM.FieldLookupValue[] multilocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                            if (multilocation != null && multilocation.Length > 0 && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multilocation.Select(x => x.LookupId.ToString()).ToList());
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                            if (projectinitative > 0)
                                                targetItem[DatabaseObjects.Columns.ProjectInitiativeLookup] = projectinitative;
                                            if (projectClass > 0)
                                                targetItem[DatabaseObjects.Columns.ProjectClassLookup] = projectClass;

                                            ClientOM.FieldLookupValue TicketAPPId = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                            if (TicketAPPId != null)
                                            {
                                                if (TicketAPPId.LookupId > 0 && TicketAPPId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.APPTitleLookup] = TicketAPPId.LookupId;
                                                    SPTicketrefernces.Add("SPAPPId", Convert.ToString(TicketAPPId.LookupId));
                                                    SPTicketrefernces.Add("SPAPPTicketId", UGITUtility.ObjectToString(TicketAPPId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }

                                            ClientOM.FieldLookupValue TicketNPRId = item[SPDatabaseObjects.Columns.TicketNPRIdLookup] as ClientOM.FieldLookupValue;
                                            if (TicketNPRId != null)
                                            {
                                                if (TicketNPRId.LookupId > 0 && TicketNPRId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.TicketNPRIdLookup] = TicketNPRId.LookupId;
                                                    SPTicketrefernces.Add("SPNPRId", Convert.ToString(TicketNPRId.LookupId));
                                                    SPTicketrefernces.Add("SPNPRTicketId", UGITUtility.ObjectToString(TicketNPRId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }

                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedRemainingHours] = item[SPDatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : item[SPDatabaseObjects.Columns.EstimatedRemainingHours];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.EstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.EstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.UGITDaysToComplete] = item[SPDatabaseObjects.Columns.UGITDaysToComplete] == null ? 0 : item[SPDatabaseObjects.Columns.UGITDaysToComplete];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.TicketProjectAssumptions] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketProjectAssumptions]);
                                            targetItem[DatabaseObjects.Columns.TicketProjectBenefits] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketProjectBenefits]);
                                            targetItem[DatabaseObjects.Columns.ProjectRiskNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRiskNotes]);
                                            targetItem[DatabaseObjects.Columns.ActualHour] = Convert.ToInt32(item[SPDatabaseObjects.Columns.ActualHour]);
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFEAmount] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEAmount]);
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFE] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFE]);
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFEType] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEType]);
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScore] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketArchitectureScore]);
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketArchitectureScoreNotes]);
                                            targetItem[DatabaseObjects.Columns.AutoAdjustAllocations] = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoAdjustAllocations]);
                                            targetItem[DatabaseObjects.Columns.ProjectSummaryNote] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectSummaryNote]);
                                            targetItem[DatabaseObjects.Columns.ProblemBeingSolved] = Convert.ToString(item[SPDatabaseObjects.Columns.ProblemBeingSolved]);
                                            targetItem[DatabaseObjects.Columns.ScrumLifeCycle] = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ScrumLifeCycle]);
                                            targetItem[DatabaseObjects.Columns.NextMilestone] = Convert.ToString(item[SPDatabaseObjects.Columns.NextMilestone]);
                                            targetItem[DatabaseObjects.Columns.TicketProjectScope] = item[SPDatabaseObjects.Columns.TicketProjectScope];
                                            targetItem[DatabaseObjects.Columns.TicketClassificationType] = item[SPDatabaseObjects.Columns.TicketClassificationType];
                                            targetItem[DatabaseObjects.Columns.TicketClassification] = item[SPDatabaseObjects.Columns.TicketClassification];
                                            targetItem[DatabaseObjects.Columns.TicketClassificationImpact] = item[SPDatabaseObjects.Columns.TicketClassificationImpact];
                                            targetItem[DatabaseObjects.Columns.ProjectCost] = item[SPDatabaseObjects.Columns.ProjectCost];
                                            targetItem[DatabaseObjects.Columns.TicketTotalCost] = item[SPDatabaseObjects.Columns.TicketTotalCost];
                                            targetItem[DatabaseObjects.Columns.TicketTotalCostsNotes] = item[SPDatabaseObjects.Columns.TicketTotalCostsNotes];
                                            targetItem[DatabaseObjects.Columns.TicketRiskScore] = item[SPDatabaseObjects.Columns.TicketRiskScore] == null ? 0 : item[SPDatabaseObjects.Columns.TicketRiskScore];
                                            targetItem[DatabaseObjects.Columns.TicketProjectScore] = item[SPDatabaseObjects.Columns.TicketProjectScore] == null ? 0 : item[SPDatabaseObjects.Columns.TicketProjectScore];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.Duration] = item[SPDatabaseObjects.Columns.TicketDuration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDuration];
                                            targetItem[DatabaseObjects.Columns.ModuleName] = item[SPDatabaseObjects.Columns.ModuleName] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ModuleName];
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && users_.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                            ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                            if (multlocation != null && multlocation.Length > 0 && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldLookupValue[] _TicketBeneficiaries = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                            if (_TicketBeneficiaries != null && _TicketBeneficiaries.Length > 0 && DepartmentList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(_TicketBeneficiaries.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldLookupValue projectlifecycles = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                            if (projectlifecycles != null && targetProjectlifecycleList != null)
                                                targetItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = targetProjectlifecycleList.GetTargetID(Convert.ToString(projectlifecycles.LookupId));

                                            targetItem[DatabaseObjects.Columns.ProjectRank] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank]);
                                            targetItem[DatabaseObjects.Columns.ProjectRank2] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank2]);
                                            targetItem[DatabaseObjects.Columns.ProjectRank3] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank3]);

                                            ClientOM.FieldUserValue[] StakeHoldersUser = item[SPDatabaseObjects.Columns.TicketStakeHolders] as ClientOM.FieldUserValue[];
                                            if (StakeHoldersUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.StakeHoldersUser] = userList.GetTargetIDs(StakeHoldersUser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] TicketProjectManagerUser = item[SPDatabaseObjects.Columns.TicketProjectManager] as ClientOM.FieldUserValue[];
                                            if (TicketProjectManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketProjectManager] = userList.GetTargetIDs(TicketProjectManagerUser.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.SponsorsUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketSponsors);
                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));

                                            ClientOM.FieldUserValue ITmanager = item[SPDatabaseObjects.Columns.TicketITManager] as ClientOM.FieldUserValue;
                                            if (ITmanager != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ITManagerUser] = userList.GetTargetID(Convert.ToString(ITmanager.LookupId));


                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }


                                            // Missing code added for PMM

                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _ResolvedByUser = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (_ResolvedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ResolvedByUser] = userList.GetTargetID(Convert.ToString(_ResolvedByUser.LookupId));
                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            targetItem[DatabaseObjects.Columns.ClassificationSizeChoice] = item[SPDatabaseObjects.Columns.TicketClassificationSize];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.AssignedAnalyst] = item[SPDatabaseObjects.Columns.AssignedAnalyst];
                                            targetItem[DatabaseObjects.Columns.ProjectConstraints] = item[SPDatabaseObjects.Columns.ProjectConstraints];
                                            targetItem[DatabaseObjects.Columns.ProjectState] = item[SPDatabaseObjects.Columns.ProjectState];
                                            targetItem[DatabaseObjects.Columns.AdoptionRiskChoice] = item[SPDatabaseObjects.Columns.AdoptionRisk];
                                            targetItem[DatabaseObjects.Columns.UGITDaysToComplete] = item[SPDatabaseObjects.Columns.UGITDaysToComplete] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITDaysToComplete];
                                            ClientOM.FieldUserValue[] SponsorsUser = item[SPDatabaseObjects.Columns.TicketSponsors] as ClientOM.FieldUserValue[];
                                            if (SponsorsUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.SponsorsUser] = userList.GetTargetIDs(SponsorsUser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldLookupValue _ServiceTitleLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                                            if (_ServiceTitleLookup != null && _ServiceTitleLookup.LookupId > 0)
                                                targetItem[DatabaseObjects.Columns.ServiceLookup] = _ServiceTitleLookup.LookupId;

                                            targetItem[DatabaseObjects.Columns.ProjectComplexityChoice] = item[SPDatabaseObjects.Columns.ProjectComplexity];
                                            targetItem[DatabaseObjects.Columns.OrganizationalImpactChoice] = item[SPDatabaseObjects.Columns.OrganizationalImpact];
                                            targetItem[DatabaseObjects.Columns.TechnologyUsabilityChoice] = item[SPDatabaseObjects.Columns.TicketTechnologyUsability];
                                            targetItem[DatabaseObjects.Columns.TechnologyReliabilityChoice] = item[SPDatabaseObjects.Columns.TicketTechnologyReliability];
                                            targetItem[DatabaseObjects.Columns.TechnologySecurityChoice] = item[SPDatabaseObjects.Columns.TicketTechnologySecurity];
                                            targetItem[DatabaseObjects.Columns.TechnologyImpactChoice] = item[SPDatabaseObjects.Columns.TechnologyImpact];
                                            targetItem[DatabaseObjects.Columns.InternalCapabilityChoice] = item[SPDatabaseObjects.Columns.InternalCapability];
                                            targetItem[DatabaseObjects.Columns.VendorSupport] = item[SPDatabaseObjects.Columns.VendorSupport];
                                            targetItem[DatabaseObjects.Columns.ImpactRevenueIncreaseChoice] = item[SPDatabaseObjects.Columns.ImpactRevenueIncrease];
                                            targetItem[DatabaseObjects.Columns.ImpactBusinessGrowthChoice] = item[SPDatabaseObjects.Columns.ImpactBusinessGrowth];
                                            targetItem[DatabaseObjects.Columns.ImpactReducesRiskChoice] = item[SPDatabaseObjects.Columns.ImpactReducesRisk];
                                            targetItem[DatabaseObjects.Columns.ImpactIncreasesProductivityChoice] = item[SPDatabaseObjects.Columns.ImpactIncreasesProductivity];
                                            targetItem[DatabaseObjects.Columns.ImpactReducesExpensesChoice] = item[SPDatabaseObjects.Columns.ImpactReducesExpenses];
                                            targetItem[DatabaseObjects.Columns.ImpactDecisionMakingChoice] = item[SPDatabaseObjects.Columns.ImpactDecisionMaking];
                                            targetItem[DatabaseObjects.Columns.PctROI] = item[SPDatabaseObjects.Columns.PctROI] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.PctROI];
                                            targetItem[DatabaseObjects.Columns.StrategicInitiativeChoice] = item[SPDatabaseObjects.Columns.TicketStrategicInitiative];
                                            targetItem[DatabaseObjects.Columns.PaybackCostSavings] = item[SPDatabaseObjects.Columns.PaybackCostSavings];
                                            targetItem[DatabaseObjects.Columns.ContributionToStrategy] = item[SPDatabaseObjects.Columns.ContributionToStrategy];
                                            targetItem[DatabaseObjects.Columns.CustomerBenefitChoice] = item[SPDatabaseObjects.Columns.CustomerBenefit];
                                            targetItem[DatabaseObjects.Columns.RegulatoryChoice] = item[SPDatabaseObjects.Columns.Regulatory];
                                            targetItem[DatabaseObjects.Columns.ITLifecycleRefreshChoice] = item[SPDatabaseObjects.Columns.ITLifecycleRefresh];
                                            targetItem[DatabaseObjects.Columns.CustomerProgram] = item[SPDatabaseObjects.Columns.CustomerProgram];
                                            targetItem[DatabaseObjects.Columns.ProductCode] = item[SPDatabaseObjects.Columns.ProductCode];
                                            targetItem[DatabaseObjects.Columns.ProductName] = item[SPDatabaseObjects.Columns.ProductName];
                                            targetItem[DatabaseObjects.Columns.ContractStatusChoice] = item[SPDatabaseObjects.Columns.ContractStatus];
                                            targetItem[DatabaseObjects.Columns.ClientLookup] = item[SPDatabaseObjects.Columns.ClientLookup] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ClientLookup];
                                            targetItem[DatabaseObjects.Columns.ProjectContractDecision] = item[SPDatabaseObjects.Columns.ProjectContractDecision];
                                            targetItem[DatabaseObjects.Columns.ProjectDataStatusChoice] = item[SPDatabaseObjects.Columns.ProjectDataStatus];
                                            targetItem[DatabaseObjects.Columns.ProjectDataDecision] = item[SPDatabaseObjects.Columns.ProjectDataDecision];
                                            targetItem[DatabaseObjects.Columns.ProjectApplicationStatusChoice] = item[SPDatabaseObjects.Columns.ProjectApplicationStatus];
                                            targetItem[DatabaseObjects.Columns.ProjectApplicationDecision] = item[SPDatabaseObjects.Columns.ProjectApplicationStatus];

                                            ClientOM.FieldUserValue[] _OwnerUser2User = item[SPDatabaseObjects.Columns.TicketOwner2] as ClientOM.FieldUserValue[];
                                            if (_OwnerUser2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.OwnerUser2User] = userList.GetTargetIDs(_OwnerUser2User.Select(x => x.LookupId.ToString()).ToList());


                                            ClientOM.FieldUserValue busers = item[SPDatabaseObjects.Columns.TicketBusinessManager2] as ClientOM.FieldUserValue;
                                            if (busers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.BusinessManager2User] = userList.GetTargetID(Convert.ToString(busers.LookupId));

                                            ClientOM.FieldUserValue _ITManager2User = item[SPDatabaseObjects.Columns.TicketITManager2] as ClientOM.FieldUserValue;
                                            if (_ITManager2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ITManager2User] = userList.GetTargetID(Convert.ToString(_ITManager2User.LookupId));

                                            targetItem[DatabaseObjects.Columns.CostSavings] = Convert.ToDouble(item[SPDatabaseObjects.Columns.CostSavings]);
                                            targetItem[DatabaseObjects.Columns.HighLevelRequirements] = Convert.ToString(item[SPDatabaseObjects.Columns.HighLevelRequirements]);
                                            targetItem[DatabaseObjects.Columns.SpecificInclusions] = Convert.ToString(item[SPDatabaseObjects.Columns.SpecificInclusions]);
                                            targetItem[DatabaseObjects.Columns.SpecificExclusions] = Convert.ToString(item[SPDatabaseObjects.Columns.SpecificExclusions]);
                                            targetItem[DatabaseObjects.Columns.ProjectDeliverables] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectDeliverables]);


                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }

                                }
                                else if (moduleName == ModuleNames.NPR)
                                {
                                    projectinitative = 0;
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();

                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;
                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFEAmount] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEAmount]);
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFE] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFE]);
                                            targetItem[DatabaseObjects.Columns.TicketApprovedRFEType] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEType]);
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                            targetItem[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes]);
                                            targetItem[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketTotalConsultantHeadcount]);
                                            targetItem[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketTotalStaffHeadcountNotes]);
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.ProjectRiskNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRiskNotes]);
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];

                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && users_.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];

                                            ClientOM.FieldUserValue[] usersPRP = item[SPDatabaseObjects.Columns.TicketProjectManager] as ClientOM.FieldUserValue[];
                                            if (usersPRP != null && usersPRP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketProjectManager] = userList.GetTargetIDs(usersPRP.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue[] businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue[];
                                            if (businessmgr != null && businessmgr.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetIDs(businessmgr.Select(x => x.LookupId.ToString()).ToList());
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            ClientOM.FieldLookupValue TicketAPPId = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                            if (TicketAPPId != null)
                                            {
                                                if (TicketAPPId.LookupId > 0 && TicketAPPId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.APPTitleLookup] = TicketAPPId.LookupId;
                                                    SPTicketrefernces.Add("SPAPPId", Convert.ToString(TicketAPPId.LookupId));
                                                    SPTicketrefernces.Add("SPAPPTicketId", UGITUtility.ObjectToString(TicketAPPId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScore] = item[SPDatabaseObjects.Columns.TicketArchitectureScore] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketArchitectureScore];
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = item[SPDatabaseObjects.Columns.TicketArchitectureScoreNotes];
                                            targetItem[DatabaseObjects.Columns.AssignedAnalyst] = item[SPDatabaseObjects.Columns.AssignedAnalyst];
                                            targetItem[DatabaseObjects.Columns.AutoAdjustAllocations] = item[SPDatabaseObjects.Columns.AutoAdjustAllocations] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AutoAdjustAllocations];
                                            ClientOM.FieldLookupValue[] beneflist = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                            if (beneflist != null && beneflist.Length > 0 && DepartmentList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(beneflist.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldLookupValue[] division = item[SPDatabaseObjects.Columns.DivisionMultiLookup] as ClientOM.FieldLookupValue[];
                                            if (division != null && division.Length > 0 && divsionlist != null)
                                                targetItem[DatabaseObjects.Columns.DivisionMultiLookup] = divsionlist.GetTargetIDs(division.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.DocumentLibraryName] = item[SPDatabaseObjects.Columns.DocumentLibraryName];
                                            targetItem[DatabaseObjects.Columns.Duration] = item[SPDatabaseObjects.Columns.TicketDuration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDuration];
                                            ClientOM.FieldLookupValue[] com = item[SPDatabaseObjects.Columns.CompanyMultiLookup] as ClientOM.FieldLookupValue[];
                                            if (com != null && com.Length > 0 && companylist != null)
                                                targetItem[DatabaseObjects.Columns.CompanyMultiLookup] = companylist.GetTargetIDs(com.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.IsITGApprovalRequired] = item[SPDatabaseObjects.Columns.IsITGApprovalRequired] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsITGApprovalRequired];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsPrivate];
                                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (userList != null && users != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(users.LookupId));
                                            ClientOM.FieldUserValue _iusers = item[SPDatabaseObjects.Columns.TicketITManager] as ClientOM.FieldUserValue;
                                            if (userList != null && _iusers != null)
                                                targetItem[DatabaseObjects.Columns.TicketITManager] = userList.GetTargetID(Convert.ToString(_iusers.LookupId));
                                            ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                            if (multlocation != null && multlocation.Length > 0 && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());

                                            if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            targetItem[DatabaseObjects.Columns.TicketNoOfFTEs] = item[SPDatabaseObjects.Columns.TicketNoOfFTEs] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketNoOfFTEs];
                                            targetItem[DatabaseObjects.Columns.TicketNoOfFTEsNotes] = item[SPDatabaseObjects.Columns.TicketNoOfFTEsNotes];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            ClientOM.FieldLookupValue TicketPMMId = item[SPDatabaseObjects.Columns.TicketPMMIdLookup] as ClientOM.FieldLookupValue;
                                            if (TicketPMMId != null)
                                            {
                                                if (TicketPMMId.LookupId > 0 && TicketPMMId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.TicketPMMIdLookup] = TicketPMMId.LookupId;
                                                    SPTicketrefernces.Add("SPPMMId", Convert.ToString(TicketPMMId.LookupId));
                                                    SPTicketrefernces.Add("SPPMMTicketId", UGITUtility.ObjectToString(TicketPMMId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }


                                            //if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                            //{
                                            //    DataRow dr = null; string Id;
                                            //    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                            //    dr = ticketMgr.GetByTicketID(module, Id);
                                            //    if (dr != null)
                                            //    {
                                            //        targetItem[DatabaseObjects.Columns.TicketPMMIdLookup] = dr["ID"];
                                            //    }
                                            //}

                                            targetItem[DatabaseObjects.Columns.ProblemBeingSolved] = item[SPDatabaseObjects.Columns.ProblemBeingSolved] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProblemBeingSolved];
                                            targetItem[DatabaseObjects.Columns.TicketProjectAssumptions] = item[SPDatabaseObjects.Columns.TicketProjectAssumptions] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketProjectAssumptions];
                                            targetItem[DatabaseObjects.Columns.ProjectCost] = item[SPDatabaseObjects.Columns.ProjectCost] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectCost];
                                            ClientOM.FieldUserValue pusers = item[SPDatabaseObjects.Columns.TicketProjectDirector] as ClientOM.FieldUserValue;
                                            if (userList != null && pusers != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.TicketProjectDirector] = userList.GetTargetID(Convert.ToString(pusers.LookupId));
                                            }
                                            if (targetProjectInitiativeList != null && item[SPDatabaseObjects.Columns.ProjectInitiativeLookup] != null)
                                            {
                                                projectinitative = 0;
                                                projectinitative = UGITUtility.StringToLong(targetProjectInitiativeList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectInitiativeLookup]).LookupId.ToString()));
                                                if (projectinitative > 0)
                                                    targetItem[SPDatabaseObjects.Columns.ProjectInitiativeLookup] = projectinitative;
                                            }
                                            targetItem[DatabaseObjects.Columns.ProjectRank] = item[SPDatabaseObjects.Columns.ProjectRank];
                                            targetItem[DatabaseObjects.Columns.ProjectRank2] = item[SPDatabaseObjects.Columns.ProjectRank2];
                                            targetItem[DatabaseObjects.Columns.ProjectRank3] = item[SPDatabaseObjects.Columns.ProjectRank3];
                                            targetItem[DatabaseObjects.Columns.ProjectRiskNotes] = item[SPDatabaseObjects.Columns.ProjectRiskNotes];
                                            targetItem[DatabaseObjects.Columns.TicketProjectScope] = item[SPDatabaseObjects.Columns.TicketProjectScope];
                                            targetItem[DatabaseObjects.Columns.TicketProjectScoreNotes] = item[SPDatabaseObjects.Columns.TicketProjectScoreNotes];
                                            targetItem[DatabaseObjects.Columns.TicketRiskScore] = item[SPDatabaseObjects.Columns.TicketRiskScore] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketRiskScore];
                                            targetItem[DatabaseObjects.Columns.TicketRiskScoreNotes] = item[SPDatabaseObjects.Columns.TicketRiskScoreNotes];
                                            targetItem[DatabaseObjects.Columns.ROI] = item[SPDatabaseObjects.Columns.TicketROI] == null ? 0 : item[SPDatabaseObjects.Columns.TicketROI];
                                            if (item[SPDatabaseObjects.Columns.TicketTargetStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketTargetStartDate]);
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.TicketTotalOffSiteConsultantHeadcount] = item[SPDatabaseObjects.Columns.TicketTotalOffSiteConsultantHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTotalOffSiteConsultantHeadcount];
                                            targetItem[DatabaseObjects.Columns.TicketClassificationType] = item[SPDatabaseObjects.Columns.TicketClassificationType];
                                            targetItem[DatabaseObjects.Columns.AdoptionRiskChoice] = item[SPDatabaseObjects.Columns.AdoptionRisk] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AdoptionRisk];
                                            targetItem[DatabaseObjects.Columns.AnalyticsArchitecture] = item[SPDatabaseObjects.Columns.AnalyticsArchitecture] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AnalyticsArchitecture] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AnalyticsArchitecture];
                                            targetItem[DatabaseObjects.Columns.AnalyticsCost] = item[SPDatabaseObjects.Columns.AnalyticsCost] == null ? 0 : item[SPDatabaseObjects.Columns.AnalyticsCost];
                                            targetItem[DatabaseObjects.Columns.AnalyticsRisk] = item[SPDatabaseObjects.Columns.AnalyticsRisk] == null ? 0 : item[SPDatabaseObjects.Columns.AnalyticsRisk];
                                            targetItem[DatabaseObjects.Columns.AnalyticsROI] = item[SPDatabaseObjects.Columns.AnalyticsROI] == null ? 0 : item[SPDatabaseObjects.Columns.AnalyticsROI];
                                            targetItem[DatabaseObjects.Columns.AnalyticsSchedule] = item[SPDatabaseObjects.Columns.AnalyticsSchedule] == null ? 0 : item[SPDatabaseObjects.Columns.AnalyticsSchedule];
                                            targetItem[DatabaseObjects.Columns.BreakEvenIn] = item[SPDatabaseObjects.Columns.BreakEvenIn] == null ? 0 : item[SPDatabaseObjects.Columns.BreakEvenIn];
                                            targetItem[DatabaseObjects.Columns.CannotStartBefore] = item[SPDatabaseObjects.Columns.CannotStartBefore] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CannotStartBefore];
                                            targetItem[DatabaseObjects.Columns.CannotStartBeforeNotes] = item[SPDatabaseObjects.Columns.CannotStartBeforeNotes];
                                            targetItem[DatabaseObjects.Columns.CapitalExpenditure] = item[SPDatabaseObjects.Columns.TicketCapitalExpenditure] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCapitalExpenditure];
                                            targetItem[DatabaseObjects.Columns.CapitalExpenditureNotes] = item[SPDatabaseObjects.Columns.CapitalExpenditureNotes] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CapitalExpenditureNotes];
                                            targetItem[DatabaseObjects.Columns.CapitalExpense] = item[SPDatabaseObjects.Columns.CapitalExpense] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CapitalExpense];
                                            targetItem[DatabaseObjects.Columns.TicketClassification] = item[SPDatabaseObjects.Columns.TicketClassification];
                                            targetItem[DatabaseObjects.Columns.TicketClassificationImpact] = item[SPDatabaseObjects.Columns.TicketClassificationImpact];
                                            targetItem[DatabaseObjects.Columns.ClassificationNotes] = item[SPDatabaseObjects.Columns.TicketClassificationNotes];
                                            targetItem[DatabaseObjects.Columns.ClassificationScopeChoice] = item[SPDatabaseObjects.Columns.TicketClassificationScope];
                                            targetItem[DatabaseObjects.Columns.ClassificationSizeChoice] = item[SPDatabaseObjects.Columns.TicketClassificationSize];
                                            targetItem[DatabaseObjects.Columns.TicketClassificationType] = item[SPDatabaseObjects.Columns.TicketClassificationType];
                                            targetItem[DatabaseObjects.Columns.ProjectComplexity] = item[SPDatabaseObjects.Columns.ProjectComplexity];
                                            targetItem[DatabaseObjects.Columns.ComplexityNotes] = item[SPDatabaseObjects.Columns.ComplexityNotes];
                                            targetItem[DatabaseObjects.Columns.ConstraintNotes] = item[SPDatabaseObjects.Columns.ConstraintNotes];
                                            targetItem[DatabaseObjects.Columns.DesiredCompletionDateNotes] = item[SPDatabaseObjects.Columns.DesiredCompletionDateNotes];
                                            targetItem[DatabaseObjects.Columns.EliminatesHeadcount] = item[SPDatabaseObjects.Columns.EliminatesHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EliminatesHeadcount];
                                            targetItem[DatabaseObjects.Columns.ImpactBusinessGrowthChoice] = item[SPDatabaseObjects.Columns.ImpactBusinessGrowth];
                                            targetItem[DatabaseObjects.Columns.ImpactDecisionMakingChoice] = item[SPDatabaseObjects.Columns.ImpactDecisionMaking];
                                            targetItem[DatabaseObjects.Columns.ImpactIncreasesProductivityChoice] = item[SPDatabaseObjects.Columns.ImpactIncreasesProductivity];
                                            targetItem[DatabaseObjects.Columns.ImpactReducesExpensesChoice] = item[SPDatabaseObjects.Columns.ImpactReducesExpenses];
                                            targetItem[DatabaseObjects.Columns.ImpactReducesRiskChoice] = item[SPDatabaseObjects.Columns.ImpactReducesRisk];
                                            targetItem[DatabaseObjects.Columns.ImpactRevenueIncreaseChoice] = item[SPDatabaseObjects.Columns.ImpactRevenueIncrease];
                                            targetItem[DatabaseObjects.Columns.ImprovesOperationalEfficiency] = item[SPDatabaseObjects.Columns.ImprovesOperationalEfficiency];
                                            targetItem[DatabaseObjects.Columns.ImprovesRevenues] = item[SPDatabaseObjects.Columns.ImprovesRevenues];
                                            targetItem[DatabaseObjects.Columns.InternalCapabilityChoice] = item[SPDatabaseObjects.Columns.InternalCapability];
                                            targetItem[DatabaseObjects.Columns.IsProjectBudgeted] = item[SPDatabaseObjects.Columns.IsProjectBudgeted] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsProjectBudgeted];
                                            targetItem[DatabaseObjects.Columns.IsSteeringApprovalRequired] = item[SPDatabaseObjects.Columns.IsSteeringApprovalRequired] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsSteeringApprovalRequired];
                                            targetItem[DatabaseObjects.Columns.ITGReviewApproval] = item[SPDatabaseObjects.Columns.ITGReviewApproval];
                                            targetItem[DatabaseObjects.Columns.ITSteeringCommitteeApproval] = item[SPDatabaseObjects.Columns.ITSteeringCommitteeApproval] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ITSteeringCommitteeApproval];
                                            targetItem[DatabaseObjects.Columns.ManagerApprovalNeeded] = item[SPDatabaseObjects.Columns.ManagerApprovalNeeded] == null ? false : item[SPDatabaseObjects.Columns.ManagerApprovalNeeded];
                                            targetItem[DatabaseObjects.Columns.OrganizationalImpactChoice] = item[SPDatabaseObjects.Columns.OrganizationalImpact];
                                            targetItem[DatabaseObjects.Columns.OtherDescribe] = item[SPDatabaseObjects.Columns.TicketOtherDescribe];
                                            targetItem[DatabaseObjects.Columns.MetricsNotes] = item[SPDatabaseObjects.Columns.MetricsNotes];
                                            targetItem[DatabaseObjects.Columns.NoAlternative] = item[SPDatabaseObjects.Columns.NoAlternative];
                                            targetItem[DatabaseObjects.Columns.NoAlternativeOtherDescribe] = item[SPDatabaseObjects.Columns.NoAlternativeOtherDescribe];
                                            targetItem[DatabaseObjects.Columns.NoOfConsultants] = item[SPDatabaseObjects.Columns.NoOfConsultants] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NoOfConsultants];
                                            targetItem[DatabaseObjects.Columns.NoOfConsultantsNotes] = item[SPDatabaseObjects.Columns.NoOfConsultantsNotes];
                                            targetItem[DatabaseObjects.Columns.NoOfReports] = item[SPDatabaseObjects.Columns.NoOfReports] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NoOfReports];
                                            targetItem[DatabaseObjects.Columns.NoOfReportsNotes] = item[SPDatabaseObjects.Columns.NoOfReportsNotes];
                                            targetItem[DatabaseObjects.Columns.NoOfScreens] = item[SPDatabaseObjects.Columns.NoOfScreens] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NoOfScreens];
                                            targetItem[DatabaseObjects.Columns.NoOfScreensNotes] = item[SPDatabaseObjects.Columns.NoOfScreensNotes];
                                            targetItem[DatabaseObjects.Columns.ProjectAssumptionsDescription] = item[SPDatabaseObjects.Columns.ProjectAssumptionsDescription] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectAssumptionsDescription];
                                            targetItem[DatabaseObjects.Columns.ProjectBenefits] = item[SPDatabaseObjects.Columns.ProjectBenefits] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectBenefits];
                                            targetItem[DatabaseObjects.Columns.ProjectBenefitsDescription] = item[SPDatabaseObjects.Columns.ProjectBenefitsDescription];
                                            targetItem[DatabaseObjects.Columns.ProjectComplexityChoice] = item[SPDatabaseObjects.Columns.ProjectComplexityChoice];
                                            targetItem[DatabaseObjects.Columns.ProjectEstDurationMaxDays] = item[SPDatabaseObjects.Columns.ProjectEstDurationMaxDays] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectEstDurationMaxDays];
                                            targetItem[DatabaseObjects.Columns.ProjectEstDurationMinDays] = item[SPDatabaseObjects.Columns.ProjectEstDurationMinDays] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectEstDurationMinDays];
                                            targetItem[DatabaseObjects.Columns.ProjectEstSizeMaxHrs] = item[SPDatabaseObjects.Columns.ProjectEstSizeMaxHrs] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectEstSizeMaxHrs];
                                            targetItem[DatabaseObjects.Columns.ProjectEstSizeMinHrs] = item[SPDatabaseObjects.Columns.ProjectEstSizeMinHrs] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectEstSizeMinHrs];
                                            targetItem[DatabaseObjects.Columns.ProjectScopeDescription] = item[SPDatabaseObjects.Columns.ProjectScopeDescription];
                                            targetItem[DatabaseObjects.Columns.RapidRequest] = item[SPDatabaseObjects.Columns.RapidRequest];
                                            targetItem[DatabaseObjects.Columns.ReducesCost] = item[SPDatabaseObjects.Columns.ReducesCost];
                                            targetItem[DatabaseObjects.Columns.RegulatoryCompliance] = item[SPDatabaseObjects.Columns.RegulatoryCompliance];
                                            targetItem[DatabaseObjects.Columns.ScheduleComplexity] = item[SPDatabaseObjects.Columns.ScheduleComplexity];

                                            ClientOM.FieldUserValue[] SponsorsUser = item[SPDatabaseObjects.Columns.TicketSponsors] as ClientOM.FieldUserValue[];
                                            if (SponsorsUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.SponsorsUser] = userList.GetTargetIDs(SponsorsUser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] StakeHoldersUser = item[SPDatabaseObjects.Columns.TicketStakeHolders] as ClientOM.FieldUserValue[];
                                            if (StakeHoldersUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.StakeHoldersUser] = userList.GetTargetIDs(StakeHoldersUser.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.StrategicInitiative] = item[SPDatabaseObjects.Columns.StrategicInitiative];
                                            targetItem[DatabaseObjects.Columns.Technology] = item[SPDatabaseObjects.Columns.Technology];
                                            targetItem[DatabaseObjects.Columns.TechnologyAvailability] = item[SPDatabaseObjects.Columns.TechnologyAvailability];
                                            targetItem[DatabaseObjects.Columns.TechnologyImpact] = item[SPDatabaseObjects.Columns.TechnologyImpact];
                                            targetItem[DatabaseObjects.Columns.TechnologyIntegration] = item[SPDatabaseObjects.Columns.TechnologyIntegration];
                                            targetItem[DatabaseObjects.Columns.TechnologyNotes1] = item[SPDatabaseObjects.Columns.TechnologyNotes1];
                                            targetItem[DatabaseObjects.Columns.TechnologyReliabilityChoice] = item[SPDatabaseObjects.Columns.TechnologyReliability];
                                            targetItem[DatabaseObjects.Columns.TechnologyRisk] = item[SPDatabaseObjects.Columns.TechnologyRisk];
                                            targetItem[DatabaseObjects.Columns.TechnologySecurityChoice] = item[SPDatabaseObjects.Columns.TechnologySecurity];
                                            targetItem[DatabaseObjects.Columns.TechnologyUsabilityChoice] = item[SPDatabaseObjects.Columns.TechnologyUsability];
                                            targetItem[DatabaseObjects.Columns.TotalOffSiteConsultantHeadcountNotes] = item[SPDatabaseObjects.Columns.TotalOffSiteConsultantHeadcountNotes];
                                            targetItem[DatabaseObjects.Columns.TotalOnSiteConsultantHeadcount] = item[SPDatabaseObjects.Columns.TotalOnSiteConsultantHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TotalOnSiteConsultantHeadcount];
                                            targetItem[DatabaseObjects.Columns.TotalOnSiteConsultantHeadcountNotes] = item[SPDatabaseObjects.Columns.TotalOnSiteConsultantHeadcountNotes];
                                            targetItem[DatabaseObjects.Columns.TotalStaffHeadcount] = item[SPDatabaseObjects.Columns.TotalStaffHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TotalStaffHeadcount];
                                            targetItem[DatabaseObjects.Columns.TotalStaffHeadcountNotes] = item[SPDatabaseObjects.Columns.TotalStaffHeadcountNotes];
                                            targetItem[DatabaseObjects.Columns.VendorSupportChoice] = item[SPDatabaseObjects.Columns.VendorSupportChoice];
                                            targetItem[DatabaseObjects.Columns.SharedServices] = item[SPDatabaseObjects.Columns.SharedServices];
                                            targetItem[DatabaseObjects.Columns.ResolvedByUser] = item[SPDatabaseObjects.Columns.ResolvedByUser];
                                            targetItem[DatabaseObjects.Columns.ProjectConstraints] = item[SPDatabaseObjects.Columns.ProjectConstraints];
                                            targetItem[DatabaseObjects.Columns.DataEditor] = item[SPDatabaseObjects.Columns.DataEditor];
                                            if (item[SPDatabaseObjects.Columns.ResolutionDate] != null)
                                                targetItem[DatabaseObjects.Columns.ResolutionDate] = item[SPDatabaseObjects.Columns.ResolutionDate];
                                            if (targetProjectclassList != null && item[SPDatabaseObjects.Columns.ProjectClassLookup] != null)
                                                projectClass = UGITUtility.StringToLong(targetProjectclassList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectClassLookup]).LookupId.ToString()));
                                            if (projectClass > 0)
                                                targetItem[SPDatabaseObjects.Columns.ProjectClassLookup] = projectClass;

                                            ClientOM.FieldUserValue[] PRPgrp = item[SPDatabaseObjects.Columns.PRPGroup] as ClientOM.FieldUserValue[];
                                            if (PRPgrp != null && PRPgrp.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.PRPGroup] = userList.GetTargetIDs(PRPgrp.Select(x => x.LookupId.ToString()).ToList());


                                            targetItem[DatabaseObjects.Columns.PaybackCostSavingsChoice] = item[SPDatabaseObjects.Columns.PaybackCostSavings];
                                            targetItem[DatabaseObjects.Columns.CustomerBenefitChoice] = item[SPDatabaseObjects.Columns.CustomerBenefit];

                                            targetItem[DatabaseObjects.Columns.ITLifecycleRefreshChoice] = item[SPDatabaseObjects.Columns.ITLifecycleRefresh];
                                            targetItem[DatabaseObjects.Columns.CostOfLabor] = item[SPDatabaseObjects.Columns.CostOfLabor] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CostOfLabor];

                                            ClientOM.FieldUserValue closedusers = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (closedusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketClosedBy] = userList.GetTargetID(Convert.ToString(closedusers.LookupId));

                                            // Missing code added for NPR

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.CustomerProgram] = item[SPDatabaseObjects.Columns.CustomerProgram];
                                            targetItem[DatabaseObjects.Columns.ProjectContractDecision] = item[SPDatabaseObjects.Columns.ProjectContractDecision];
                                            targetItem[DatabaseObjects.Columns.ProjectDataDecision] = item[SPDatabaseObjects.Columns.ProjectDataDecision];
                                            targetItem[DatabaseObjects.Columns.ProjectApplicationStatusChoice] = item[SPDatabaseObjects.Columns.ProjectApplicationStatus];
                                            targetItem[DatabaseObjects.Columns.ProjectApplicationDecision] = item[SPDatabaseObjects.Columns.ProjectApplicationStatus];

                                            ClientOM.FieldUserValue[] _OwnerUser2User = item[SPDatabaseObjects.Columns.TicketOwner2] as ClientOM.FieldUserValue[];
                                            if (_OwnerUser2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.OwnerUser2User] = userList.GetTargetIDs(_OwnerUser2User.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue busers = item[SPDatabaseObjects.Columns.TicketBusinessManager2] as ClientOM.FieldUserValue;
                                            if (busers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.BusinessManager2User] = userList.GetTargetID(Convert.ToString(busers.LookupId));


                                            ClientOM.FieldUserValue _ITManager2User = item[SPDatabaseObjects.Columns.TicketITManager2] as ClientOM.FieldUserValue;
                                            if (_ITManager2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ITManager2User] = userList.GetTargetID(Convert.ToString(_ITManager2User.LookupId));
                                            targetItem[DatabaseObjects.Columns.CostSavings] = Convert.ToDouble(item[SPDatabaseObjects.Columns.CostSavings]);
                                            targetItem[DatabaseObjects.Columns.HighLevelRequirements] = Convert.ToString(item[SPDatabaseObjects.Columns.HighLevelRequirements]);
                                            targetItem[DatabaseObjects.Columns.SpecificInclusions] = Convert.ToString(item[SPDatabaseObjects.Columns.SpecificInclusions]);
                                            targetItem[DatabaseObjects.Columns.SpecificExclusions] = Convert.ToString(item[SPDatabaseObjects.Columns.SpecificExclusions]);



                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.TSK)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;
                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.ActualHour] = item[SPDatabaseObjects.Columns.ActualHour] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ActualHour];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScore] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketArchitectureScore]);
                                            targetItem[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = item[SPDatabaseObjects.Columns.TicketArchitectureScoreNotes];
                                            targetItem[DatabaseObjects.Columns.AutoAdjustAllocations] = item[DatabaseObjects.Columns.AutoAdjustAllocations];
                                            ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                            if (multlocation != null && multlocation.Length > 0 && DepartmentList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem["BenefitsExperienced"] = item["BenefitsExperienced"];
                                            targetItem["BreakevenMonth"] = item["BreakevenMonth"];
                                            ClientOM.FieldLookupValue[] com = item[SPDatabaseObjects.Columns.CompanyMultiLookup] as ClientOM.FieldLookupValue[];
                                            if (com != null && com.Length > 0 && companylist != null)
                                                targetItem[DatabaseObjects.Columns.CompanyMultiLookup] = companylist.GetTargetIDs(com.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.TicketConstraintNotes] = item[SPDatabaseObjects.Columns.TicketConstraintNotes];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[DatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.UGITDaysToComplete] = item[SPDatabaseObjects.Columns.UGITDaysToComplete] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITDaysToComplete];
                                            if (item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate]);
                                            ClientOM.FieldLookupValue[] division = item[SPDatabaseObjects.Columns.DivisionMultiLookup] as ClientOM.FieldLookupValue[];
                                            if (division != null && division.Length > 0 && divsionlist != null)
                                                targetItem[DatabaseObjects.Columns.DivisionMultiLookup] = divsionlist.GetTargetIDs(division.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem[DatabaseObjects.Columns.DocumentLibraryName] = item[SPDatabaseObjects.Columns.DocumentLibraryName];
                                            targetItem[DatabaseObjects.Columns.Duration] = item[SPDatabaseObjects.Columns.TicketDuration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDuration];
                                            targetItem["EliminatesStaffHeadCount"] = item["EliminatesStaffHeadCount"] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDuration];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.EstimatedHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EstimatedHours];
                                            targetItem[DatabaseObjects.Columns.EstimatedRemainingHours] = item[SPDatabaseObjects.Columns.EstimatedRemainingHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EstimatedRemainingHours];
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            targetItem["ImpactAsImprovesRevenues"] = item["ImpactAsImprovesRevenues"];
                                            targetItem["ImpactAsOperationalEfficiency"] = item["ImpactAsOperationalEfficiency"];
                                            targetItem["ImpactAsSaveMoney"] = item["ImpactAsSaveMoney"];
                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;

                                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (userList != null && users != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(users.LookupId));
                                            }
                                            ClientOM.FieldUserValue _iusers = item[SPDatabaseObjects.Columns.TicketITManager] as ClientOM.FieldUserValue;
                                            if (userList != null && _iusers != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.TicketITManager] = userList.GetTargetID(Convert.ToString(_iusers.LookupId));
                                            }
                                            targetItem["LessonsLearned"] = item["LessonsLearned"];
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.NextActivity] = item[SPDatabaseObjects.Columns.NextActivity];
                                            targetItem[DatabaseObjects.Columns.NextMilestone] = item[SPDatabaseObjects.Columns.NextMilestone];
                                            targetItem[DatabaseObjects.Columns.TicketNoOfConsultants] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketNoOfConsultants]);
                                            targetItem[DatabaseObjects.Columns.TicketNoOfConsultantsNotes] = item[SPDatabaseObjects.Columns.TicketNoOfConsultantsNotes];
                                            targetItem[DatabaseObjects.Columns.TicketNoOfFTEs] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketNoOfFTEs]);
                                            targetItem[DatabaseObjects.Columns.TicketNoOfFTEsNotes] = item[SPDatabaseObjects.Columns.TicketNoOfFTEsNotes];
                                            targetItem[DatabaseObjects.Columns.TicketNPRIdLookup] = item[SPDatabaseObjects.Columns.TicketNPRIdLookup] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketNPRIdLookup];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (targetProjectclassList != null && item[SPDatabaseObjects.Columns.ProjectClassLookup] != null)
                                                projectClass = UGITUtility.StringToLong(targetProjectclassList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectClassLookup]).LookupId.ToString()));
                                            if (projectClass > 0)
                                                targetItem[SPDatabaseObjects.Columns.ProjectClassLookup] = projectClass;
                                            targetItem[DatabaseObjects.Columns.ProjectCost] = item[SPDatabaseObjects.Columns.ProjectCost] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectCost];
                                            targetItem[DatabaseObjects.Columns.ProjectCostNote] = item[DatabaseObjects.Columns.ProjectCostNote];

                                            if (targetProjectInitiativeList != null && item[SPDatabaseObjects.Columns.ProjectInitiativeLookup] != null)
                                                projectinitative = UGITUtility.StringToLong(targetProjectInitiativeList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectInitiativeLookup]).LookupId.ToString()));
                                            if (projectinitative > 0)
                                                targetItem[SPDatabaseObjects.Columns.ProjectInitiativeLookup] = projectinitative;
                                            ClientOM.FieldUserValue[] _musers = item[SPDatabaseObjects.Columns.TicketProjectManager] as ClientOM.FieldUserValue[];
                                            if (_musers != null && _musers.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketProjectManager] = userList.GetTargetIDs(_musers.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.ProjectPhasePctComplete] = item[DatabaseObjects.Columns.ProjectPhasePctComplete] == null ? 0 : item[SPDatabaseObjects.Columns.ProjectPhasePctComplete];
                                            targetItem[DatabaseObjects.Columns.ProjectScheduleNote] = item[DatabaseObjects.Columns.ProjectScheduleNote];
                                            targetItem[DatabaseObjects.Columns.TicketProjectScore] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketProjectScore]);
                                            targetItem[DatabaseObjects.Columns.TicketProjectScoreNotes] = item[SPDatabaseObjects.Columns.TicketProjectScoreNotes];
                                            targetItem[DatabaseObjects.Columns.ProjectSummaryNote] = item[DatabaseObjects.Columns.ProjectSummaryNote];
                                            ClientOM.FieldUserValue rusers = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue;
                                            if (userList != null && rusers != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetID(Convert.ToString(rusers.LookupId));
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = item[SPDatabaseObjects.Columns.TicketRequestTypeCategory];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory];
                                            targetItem[DatabaseObjects.Columns.TicketRiskScore] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketRiskScore]);
                                            targetItem[DatabaseObjects.Columns.TicketRiskScoreNotes] = item[SPDatabaseObjects.Columns.TicketRiskScoreNotes];
                                            targetItem["ROI"] = item["ROI"];

                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;
                                            targetItem["ShowProjectStatus"] = item["ShowProjectStatus"] == null ? DBNull.Value : item["ShowProjectStatus"];
                                            targetItem[DatabaseObjects.Columns.TicketSponsors] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketSponsors);
                                            ClientOM.FieldUserValue[] StakeHoldersUser = item[SPDatabaseObjects.Columns.TicketStakeHolders] as ClientOM.FieldUserValue[];
                                            if (StakeHoldersUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.StakeHoldersUser] = userList.GetTargetIDs(StakeHoldersUser.Select(x => x.LookupId.ToString()).ToList());
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketTargetCompletionDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketTargetStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketTargetStartDate]);
                                            targetItem[DatabaseObjects.Columns.TicketTotalConsultantHeadcount] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketTotalConsultantHeadcount]);
                                            targetItem[DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes] = item[SPDatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes];
                                            targetItem[DatabaseObjects.Columns.TicketTotalCost] = item[SPDatabaseObjects.Columns.TicketTotalCost] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTotalCost];
                                            targetItem[DatabaseObjects.Columns.TicketTotalCostsNotes] = item[SPDatabaseObjects.Columns.TicketTotalCostsNotes];
                                            targetItem[DatabaseObjects.Columns.TicketTotalStaffHeadcount] = item[SPDatabaseObjects.Columns.TicketTotalStaffHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTotalStaffHeadcount];
                                            targetItem[DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes] = item[SPDatabaseObjects.Columns.TicketTotalStaffHeadcountNotes];
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem["NextMilestoneDate"] = item["NextMilestoneDate"] == null ? DBNull.Value : item["NextMilestoneDate"];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            // Missing code added bt TSK

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _ResolvedByUser = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (_ResolvedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ResolvedByUser] = userList.GetTargetID(Convert.ToString(_ResolvedByUser.LookupId));

                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));


                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.ACR)
                                {
                                    if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));

                                    if (acrlist != null && item["ACRTypeTitleLookup"] != null)
                                        acrtypeid = UGITUtility.StringToLong(acrlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item["ACRTypeTitleLookup"]).LookupId.ToString()));

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetLocationID > 0)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;

                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;

                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;

                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;

                                            ClientOM.FieldLookupValue com = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                                            if (com != null && companylist != null)
                                            {
                                                long id = Convert.ToInt64(companylist.GetTargetID(Convert.ToString(com.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = id;
                                            }

                                            string appTitleLookupValue = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.APPTitleLookup);
                                            if (!string.IsNullOrWhiteSpace(appTitleLookupValue))
                                            {
                                                if (UGITUtility.StringToInt(appTitleLookupValue) > 0)
                                                    targetItem[DatabaseObjects.Columns.APPTitleLookup] = UGITUtility.StringToInt(appTitleLookupValue);
                                            }

                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.IsPerformanceTestingDone] = item[SPDatabaseObjects.Columns.IsPerformanceTestingDone] == null ? false : item[SPDatabaseObjects.Columns.IsPerformanceTestingDone];
                                            targetItem[DatabaseObjects.Columns.TicketBAAnalysisHours] = item[SPDatabaseObjects.Columns.TicketBAAnalysisHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketBAAnalysisHours];
                                            targetItem["BATestingHours"] = item[SPDatabaseObjects.Columns.TicketBATestingHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketBATestingHours];
                                            targetItem["BATotalHours"] = item[SPDatabaseObjects.Columns.TicketBATotalHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketBATotalHours];
                                            targetItem[DatabaseObjects.Columns.DeskLocation] = item[SPDatabaseObjects.Columns.DeskLocation] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.DeskLocation];
                                            targetItem["DeveloperCodingHours"] = item[SPDatabaseObjects.Columns.TicketDeveloperCodingHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketDeveloperCodingHours];
                                            targetItem["DeveloperSupportHours"] = item[SPDatabaseObjects.Columns.TicketDeveloperSupportHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketDeveloperSupportHours];
                                            targetItem[DatabaseObjects.Columns.TicketDeveloperTotalHours] = item[SPDatabaseObjects.Columns.TicketDeveloperTotalHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketDeveloperTotalHours];
                                            targetItem[DatabaseObjects.Columns.IssueTypeChoice] = item[SPDatabaseObjects.Columns.UGITIssueType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITIssueType];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? false : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.ElevatedPriority] = item[SPDatabaseObjects.Columns.ElevatedPriority] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ElevatedPriority];
                                            ClientOM.FieldLookupValue division = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;
                                            if (division != null && divsionlist != null)
                                            {
                                                long id = Convert.ToInt64(divsionlist.GetTargetID(Convert.ToString(division.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DivisionLookup] = id;
                                            }
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];

                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];

                                            if (acrtypeid > 0)
                                                targetItem["ACRTypeTitleLookup"] = acrtypeid;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            ClientOM.FieldLookupValue multlocation = item[SPDatabaseObjects.Columns.LocationLookup] as ClientOM.FieldLookupValue;
                                            if (multlocation != null && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = locationMappedList.GetTargetID(Convert.ToString(multlocation.LookupId));
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                //List<string> steps = new List<string>();
                                                //List<string> stagesIds = new List<string>();
                                                //steps = stageids.Split(',').ToList();
                                                // targetStagetMappedList.GetTargetIDs(steps);
                                                //foreach (string id in steps)
                                                //{
                                                //    stagesIds.Add(targetStagetMappedList.GetTargetIDs(steps));
                                                //}
                                                //targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = ).ToList());
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            // Missing code added for ACR

                                            ClientOM.FieldUserValue busers = item[SPDatabaseObjects.Columns.TicketBusinessManager2] as ClientOM.FieldUserValue;
                                            if (busers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.BusinessManager2User] = userList.GetTargetID(Convert.ToString(busers.LookupId));


                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _DepartmentManagerUser = item[SPDatabaseObjects.Columns.TicketDepartmentManager] as ClientOM.FieldUserValue;
                                            if (_DepartmentManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DepartmentManagerUser] = userList.GetTargetID(Convert.ToString(_DepartmentManagerUser.LookupId));

                                            ClientOM.FieldUserValue _DivisionManagerUser = item[SPDatabaseObjects.Columns.TicketDivisionManager] as ClientOM.FieldUserValue;
                                            if (_DivisionManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DivisionManagerUser] = userList.GetTargetID(Convert.ToString(_DivisionManagerUser.LookupId));

                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            ClientOM.FieldLookupValue _SubLocationLookup = item[SPDatabaseObjects.Columns.SubLocationLookup] as ClientOM.FieldLookupValue;
                                            if (_SubLocationLookup != null && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.SubLocationLookup] = locationMappedList.GetTargetID(Convert.ToString(_SubLocationLookup.LookupId));

                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.OrganizationalImpactChoice] = item[SPDatabaseObjects.Columns.OrganizationalImpact];
                                            targetItem[DatabaseObjects.Columns.ReleaseID] = Convert.ToString(item[SPDatabaseObjects.Columns.ReleaseID]);
                                            targetItem[DatabaseObjects.Columns.RiskLevelChoice] = item[SPDatabaseObjects.Columns.RiskLevel];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);

                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];

                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);



                                            ClientOM.FieldUserValue _ApproverUser = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue;
                                            if (_ApproverUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ApproverUser] = userList.GetTargetID(Convert.ToString(_ApproverUser.LookupId));

                                            ClientOM.FieldUserValue _Approver2User = item[SPDatabaseObjects.Columns.Approver2] as ClientOM.FieldUserValue;
                                            if (_Approver2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.Approver2User] = userList.GetTargetID(Convert.ToString(_Approver2User.LookupId));

                                            ticketSchema.Rows.Add(targetItem);

                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                else if (moduleName == ModuleNames.SVC)
                                {

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;

                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];

                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];

                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = item[SPDatabaseObjects.Columns.TicketTargetStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? true : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.EnableTaskReminder] = item[SPDatabaseObjects.Columns.EnableTaskReminder] == null ? false : item[SPDatabaseObjects.Columns.EnableTaskReminder];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];

                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            targetItem[DatabaseObjects.Columns.IsAllTaskComplete] = item[SPDatabaseObjects.Columns.IsAllTaskComplete] == null ? false : item[SPDatabaseObjects.Columns.IsAllTaskComplete];
                                            targetItem["ShowProjectStatus"] = item["ShowProjectStatus"] == null ? DBNull.Value : item["ShowProjectStatus"];
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] _ApproverUser = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue[];
                                            if (_ApproverUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ApproverUser] = userList.GetTargetIDs(_ApproverUser.Select(x => x.LookupId.ToString()).ToList());
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            // Missing code added for SVC

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];

                                            targetItem[DatabaseObjects.Columns.OwnerApprovalRequired] = item[SPDatabaseObjects.Columns.OwnerApprovalRequired] == null ? true : item[SPDatabaseObjects.Columns.OwnerApprovalRequired];
                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));


                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));
                                            targetItem[DatabaseObjects.Columns.BulkRequestCount] = item[SPDatabaseObjects.Columns.BulkRequestCount] == null ? 0 : item[SPDatabaseObjects.Columns.BulkRequestCount];

                                            ClientOM.FieldUserValue _Approver2User = item[SPDatabaseObjects.Columns.Approver2] as ClientOM.FieldUserValue;
                                            if (_Approver2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.Approver2User] = userList.GetTargetID(Convert.ToString(_Approver2User.LookupId));
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];


                                            ClientOM.FieldLookupValue multlocation = item[SPDatabaseObjects.Columns.LocationLookup] as ClientOM.FieldLookupValue;
                                            if (multlocation != null && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = locationMappedList.GetTargetID(Convert.ToString(multlocation.LookupId));


                                            ClientOM.FieldLookupValue com = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                                            if (com != null && companylist != null)
                                            {
                                                long id = Convert.ToInt64(companylist.GetTargetID(Convert.ToString(com.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = id;
                                            }
                                            ClientOM.FieldLookupValue division = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;
                                            if (division != null && divsionlist != null)
                                            {
                                                long id = Convert.ToInt64(divsionlist.GetTargetID(Convert.ToString(division.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DivisionLookup] = id;
                                            }

                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                else if (moduleName == ModuleNames.APP)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        //Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;

                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? true : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.CategoryNameChoice] = item[SPDatabaseObjects.Columns.CategoryName] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CategoryName];
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];


                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;

                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] _adminusers = item[SPDatabaseObjects.Columns.AccessAdmin] as ClientOM.FieldUserValue[];
                                            if (_adminusers != null && _adminusers.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.AccessAdmin] = userList.GetTargetIDs(_adminusers.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem[DatabaseObjects.Columns.AccessManageLevel] = item[SPDatabaseObjects.Columns.AccessManageLevel] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AccessManageLevel];
                                            targetItem["AllocatedSeats"] = item["AllocatedSeats"] == null ? DBNull.Value : item["AllocatedSeats"];
                                            targetItem[DatabaseObjects.Columns.ApprovalType] = item[SPDatabaseObjects.Columns.ApprovalType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ApprovalType];
                                            targetItem["BuildNumber"] = item["BuildNumber"] == null ? DBNull.Value : item["BuildNumber"];
                                            targetItem[DatabaseObjects.Columns.NumLicensesTotal] = item[SPDatabaseObjects.Columns.NumLicensesTotal] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NumLicensesTotal];
                                            targetItem[DatabaseObjects.Columns.NumUsers] = item[SPDatabaseObjects.Columns.NumUsers] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NumUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));

                                            ClientOM.FieldUserValue[] suppoteduser = item[SPDatabaseObjects.Columns.SupportedBy] as ClientOM.FieldUserValue[];
                                            if (suppoteduser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.SupportedBy] = userList.GetTargetIDs(suppoteduser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            ClientOM.FieldUserValue[] _ApproverUser = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue[];
                                            if (_ApproverUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ApproverUser] = userList.GetTargetIDs(_ApproverUser.Select(x => x.LookupId.ToString()).ToList());

                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }


                                            // Missing code added for APP
                                            targetItem[DatabaseObjects.Columns.TicketRiskScore] = item[DatabaseObjects.Columns.TicketRiskScore] == null ? 0 : item[DatabaseObjects.Columns.TicketRiskScore];
                                            targetItem[DatabaseObjects.Columns.FrequencyOfUpgradesChoice] = item[SPDatabaseObjects.Columns.FrequencyOfUpgrades] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.FrequencyOfUpgrades];
                                            targetItem[DatabaseObjects.Columns.FrequencyOfUpgradesNotes] = item[SPDatabaseObjects.Columns.FrequencyOfUpgradesNotes] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.FrequencyOfUpgradesNotes];
                                            targetItem[DatabaseObjects.Columns.FrequencyOfTypicalUse] = item[SPDatabaseObjects.Columns.FrequencyOfTypicalUse] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.FrequencyOfTypicalUse];
                                            targetItem[DatabaseObjects.Columns.HostingTypeChoice] = item[SPDatabaseObjects.Columns.HostingType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.FrequencyOfTypicalUse];
                                            targetItem[DatabaseObjects.Columns.NextPlannedMajorUpgrade] = item[SPDatabaseObjects.Columns.NextPlannedMajorUpgrade] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextPlannedMajorUpgrade];
                                            targetItem[DatabaseObjects.Columns.NextUpgradeDate] = item[SPDatabaseObjects.Columns.NextUpgradeDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextUpgradeDate];
                                            targetItem[DatabaseObjects.Columns.NextStandardReviewDate] = item[SPDatabaseObjects.Columns.NextStandardReviewDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextStandardReviewDate];
                                            targetItem[DatabaseObjects.Columns.SoftwareKey] = item[SPDatabaseObjects.Columns.SoftwareKey] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SoftwareKey];
                                            targetItem[DatabaseObjects.Columns.SoftwareMinorVersion] = item[SPDatabaseObjects.Columns.SoftwareMinorVersion] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SoftwareMinorVersion];
                                            targetItem[DatabaseObjects.Columns.SoftwarePatchRevision] = item[SPDatabaseObjects.Columns.SoftwarePatchRevision] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SoftwarePatchRevision];
                                            targetItem[DatabaseObjects.Columns.SoftwareVersion] = item[SPDatabaseObjects.Columns.SoftwareVersion] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SoftwareVersion];
                                            targetItem[DatabaseObjects.Columns.IssueTypeOptions] = item[SPDatabaseObjects.Columns.IssueTypeOptions] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IssueTypeOptions];
                                            targetItem[DatabaseObjects.Columns.Numberofseats] = item[SPDatabaseObjects.Columns.NumberOfSeats] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NumberOfSeats];
                                            targetItem[DatabaseObjects.Columns.VersionLatestRelease] = item[SPDatabaseObjects.Columns.VersionLatestRelease] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.VersionLatestRelease];
                                            targetItem[DatabaseObjects.Columns.LicenseBasisChoice] = item[SPDatabaseObjects.Columns.LicenseBasis] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.LicenseBasis];
                                            targetItem[DatabaseObjects.Columns.SubCategory] = item[SPDatabaseObjects.Columns.SubCategory] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SubCategory];
                                            targetItem[DatabaseObjects.Columns.AppLifeCycleChoice] = item[SPDatabaseObjects.Columns.AppLifeCycle] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AppLifeCycle];
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = item[SPDatabaseObjects.Columns.TicketRequestTypeCategory];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory];
                                            targetItem[DatabaseObjects.Columns.SecurityDescription] = item[SPDatabaseObjects.Columns.SecurityDescription];
                                            targetItem[DatabaseObjects.Columns.AuthenticationChoice] = item[SPDatabaseObjects.Columns.Authentication];
                                            targetItem[DatabaseObjects.Columns.NumUsers2] = item[SPDatabaseObjects.Columns.NumUsers2];
                                            ClientOM.FieldUserValue busers = item[SPDatabaseObjects.Columns.TicketBusinessManager2] as ClientOM.FieldUserValue;
                                            if (busers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.BusinessManager2User] = userList.GetTargetID(Convert.ToString(busers.LookupId));

                                            targetItem[DatabaseObjects.Columns.SupportedBrowsersChoice] = item[SPDatabaseObjects.Columns.SupportedBrowsers];



                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.CMDB)
                                {
                                    if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (assetmodelList != null && item[SPDatabaseObjects.Columns.AssetModelLookup] != null)
                                        targetlookupID = UGITUtility.StringToLong(assetmodelList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.AssetModelLookup]).LookupId.ToString()));

                                    if (assetvendorList != null && item[SPDatabaseObjects.Columns.VendorLookup] != null)
                                        vid = UGITUtility.StringToLong(assetvendorList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.VendorLookup]).LookupId.ToString()));

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetLocationID > 0)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;
                                            if (vid > 0)
                                                targetItem[DatabaseObjects.Columns.VendorLookup] = vid;
                                            targetItem[DatabaseObjects.Columns.AssetDescription] = item[SPDatabaseObjects.Columns.AssetDescription];
                                            targetItem[DatabaseObjects.Columns.AssetName] = item[SPDatabaseObjects.Columns.AssetName];
                                            targetItem[DatabaseObjects.Columns.AssetTagNum] = item[SPDatabaseObjects.Columns.AssetTagNum];
                                            if (targetlookupID > 0)
                                                targetItem[DatabaseObjects.Columns.AssetModelLookup] = targetlookupID;
                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;

                                            targetItem[DatabaseObjects.Columns.AssetDispositionChoice] = item[SPDatabaseObjects.Columns.AssetDisposition] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AssetDisposition];
                                            targetItem[DatabaseObjects.Columns.AcquisitionDate] = item[SPDatabaseObjects.Columns.AcquisitionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.AcquisitionDate];
                                            targetItem[DatabaseObjects.Columns.HostName] = item[SPDatabaseObjects.Columns.HostName];
                                            targetItem["ActualReplacementDate"] = item["ActualReplacementDate"] == null ? DBNull.Value : item["ActualReplacementDate"];
                                            targetItem["CPU"] = item["CPU"];
                                            targetItem[DatabaseObjects.Columns.UGITCost] = item[SPDatabaseObjects.Columns.UGITCost] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITCost];
                                            targetItem["DataRetention"] = item["DataRetention"] == null ? DBNull.Value : item["DataRetention"];
                                            targetItem["ImageInstallDate"] = item["ImageInstallDate"] == null ? DBNull.Value : item["ImageInstallDate"];
                                            ClientOM.FieldUserValue insuser = item["InstalledBy"] as ClientOM.FieldUserValue;
                                            if (insuser != null && userList != null)
                                                targetItem["InstalledByUser"] = userList.GetTargetID(Convert.ToString(insuser.LookupId));
                                            targetItem[DatabaseObjects.Columns.IPAddress] = item[SPDatabaseObjects.Columns.IPAddress];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem["InstalledDate"] = item["InstalledDate"] == null ? DBNull.Value : item["InstalledDate"];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem["LicenseKey"] = item["LicenseKey"] == null ? DBNull.Value : item["LicenseKey"];
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.Manufacturer] = item[SPDatabaseObjects.Columns.Manufacturer];
                                            targetItem[DatabaseObjects.Columns.OS] = item[SPDatabaseObjects.Columns.OS] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OS];
                                            targetItem[DatabaseObjects.Columns.NICAddress] = item[SPDatabaseObjects.Columns.NICAddress];
                                            targetItem["PreAcquired"] = item["PreAcquired"] == null ? false : item["PreAcquired"];
                                            targetItem[DatabaseObjects.Columns.SerialAssetDetail] = item[SPDatabaseObjects.Columns.SerialAssetDetail];
                                            targetItem[DatabaseObjects.Columns.SerialNum1] = item[SPDatabaseObjects.Columns.SerialNum1];
                                            targetItem[DatabaseObjects.Columns.SerialNum2] = item[SPDatabaseObjects.Columns.SerialNum2];
                                            targetItem[DatabaseObjects.Columns.SerialNum3] = item[SPDatabaseObjects.Columns.SerialNum3];
                                            targetItem["SerialNum1Description"] = item["SerialNum1Description"] == null ? DBNull.Value : item["SerialNum1Description"];
                                            targetItem["SerialNum2Description"] = item["SerialNum2Description"] == null ? DBNull.Value : item["SerialNum2Description"];
                                            targetItem["SerialNum3Description"] = item["SerialNum3Description"] == null ? DBNull.Value : item["SerialNum3Description"];
                                            targetItem["StatusChangeDate"] = item["StatusChangeDate"] == null ? DBNull.Value : item["StatusChangeDate"];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue _users = item[SPDatabaseObjects.Columns.AssetOwner] as ClientOM.FieldUserValue;
                                            if (_users != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetID(Convert.ToString(_users.LookupId));

                                            ClientOM.FieldUserValue preuser = item["PreviousUser"] as ClientOM.FieldUserValue;
                                            if (preuser != null && userList != null)
                                                targetItem["PreviousUser"] = userList.GetTargetID(Convert.ToString(preuser.LookupId));

                                            ClientOM.FieldUserValue preuser1 = item["PreviousOwner1"] as ClientOM.FieldUserValue;
                                            if (preuser1 != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PreviousOwner1] = userList.GetTargetID(Convert.ToString(preuser1.LookupId));
                                            ClientOM.FieldUserValue preuser2 = item["PreviousOwner2"] as ClientOM.FieldUserValue;
                                            if (preuser2 != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PreviousOwner2] = userList.GetTargetID(Convert.ToString(preuser2.LookupId));
                                            ClientOM.FieldUserValue preuser3 = item["PreviousOwner3"] as ClientOM.FieldUserValue;
                                            if (preuser3 != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PreviousOwner3] = userList.GetTargetID(Convert.ToString(preuser3.LookupId));

                                            targetItem["ReplacementOrderedDate"] = item["ReplacementOrderedDate"] == null ? DBNull.Value : item["ReplacementOrderedDate"];
                                            targetItem["ReplacementDeliveryDate"] = item["ReplacementDeliveryDate"] == null ? DBNull.Value : item["ReplacementDeliveryDate"];
                                            //targetItem[DatabaseObjects.Columns.ReplacementType] = item[DatabaseObjects.Columns.ReplacementType];
                                            targetItem["RetiredDate"] = item["RetiredDate"] == null ? DBNull.Value : item["RetiredDate"];
                                            targetItem["SaleDate"] = item["SaleDate"] == null ? DBNull.Value : item["SaleDate"];
                                            //targetItem["ScheduleStatusChoice"] = item["ScheduleStatusChoice"]== null ?DBNull.Value: item["ScheduleStatusChoice"];

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem["RAM"] = item["RAM"] == null ? DBNull.Value : item["RAM"];
                                            targetItem[DatabaseObjects.Columns.LocalAdminUser] = 0;
                                            // Missing code added for CMDB
                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            ClientOM.FieldLookupValue _SubLocationLookup = item[SPDatabaseObjects.Columns.SubLocationLookup] as ClientOM.FieldLookupValue;
                                            if (_SubLocationLookup != null && locationMappedList != null && _SubLocationLookup.LookupId > 0)
                                                targetItem[DatabaseObjects.Columns.SubLocationLookup] = locationMappedList.GetTargetID(Convert.ToString(_SubLocationLookup.LookupId));

                                            targetItem[DatabaseObjects.Columns.ProductionCritical] = item[SPDatabaseObjects.Columns.ProductionCritical] == null ? true : item[SPDatabaseObjects.Columns.ProductionCritical];
                                            targetItem[DatabaseObjects.Columns.Firmware] = item[SPDatabaseObjects.Columns.Firmware];
                                            targetItem[DatabaseObjects.Columns.VersionNumber] = item[SPDatabaseObjects.Columns.VersionNumber];
                                            targetItem[DatabaseObjects.Columns.Unmanaged] = item[SPDatabaseObjects.Columns.Unmanaged] == null ? true : item[SPDatabaseObjects.Columns.Unmanaged];
                                            targetItem[DatabaseObjects.Columns.SSLCertName] = item[SPDatabaseObjects.Columns.SSLCertName];
                                            targetItem[DatabaseObjects.Columns.SSLCertExpiration] = item[SPDatabaseObjects.Columns.SSLCertExpiration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.SSLCertExpiration];
                                            targetItem[DatabaseObjects.Columns.ProductReleaseDate] = item[SPDatabaseObjects.Columns.ProductReleaseDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProductReleaseDate];
                                            targetItem[DatabaseObjects.Columns.EndOfSaleDate] = item[SPDatabaseObjects.Columns.EndOfSaleDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfSaleDate];
                                            targetItem[DatabaseObjects.Columns.EndOfSoftwareMaintenanceDate] = item[SPDatabaseObjects.Columns.EndOfSoftwareMaintenanceDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfSoftwareMaintenanceDate];
                                            targetItem[DatabaseObjects.Columns.EndOfSecurityUpdatesDate] = item[SPDatabaseObjects.Columns.EndOfSecurityUpdatesDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfSecurityUpdatesDate];
                                            targetItem[DatabaseObjects.Columns.EndOfSupportDate] = item[SPDatabaseObjects.Columns.EndOfSupportDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfSupportDate];
                                            targetItem[DatabaseObjects.Columns.EndOfExtendedSupportDate] = item[SPDatabaseObjects.Columns.EndOfExtendedSupportDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfExtendedSupportDate];
                                            targetItem[DatabaseObjects.Columns.EndOfLifeDate] = item[SPDatabaseObjects.Columns.EndOfLifeDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EndOfLifeDate];
                                            targetItem[DatabaseObjects.Columns.StandardRefreshPeriod] = item[SPDatabaseObjects.Columns.StandardRefreshPeriod];

                                            ClientOM.FieldLookupValue _ContractLookup = item[SPDatabaseObjects.Columns.ContractLookup] as ClientOM.FieldLookupValue;
                                            if (_ContractLookup != null && _ContractLookup.LookupId > 0)
                                                targetItem[DatabaseObjects.Columns.ContractLookup] = Convert.ToString(_ContractLookup.LookupValue);

                                            targetItem[DatabaseObjects.Columns.StandardChoice] = item[SPDatabaseObjects.Columns.Standard];
                                            targetItem[DatabaseObjects.Columns.StandardReviewDate] = item[SPDatabaseObjects.Columns.StandardReviewDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.StandardReviewDate];
                                            targetItem[DatabaseObjects.Columns.NextStandardReviewDate] = item[SPDatabaseObjects.Columns.NextStandardReviewDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextStandardReviewDate];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldStartDate] = item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHoldStartDate];
                                            targetItem[DatabaseObjects.Columns.TotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];

                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason];
                                            targetItem[DatabaseObjects.Columns.BackedUpComponentsChoice] = item[SPDatabaseObjects.Columns.BackedUpComponents];
                                            targetItem[DatabaseObjects.Columns.OrderNum] = item[SPDatabaseObjects.Columns.OrderNum];
                                            targetItem[DatabaseObjects.Columns.NonStandardConfiguration] = item[SPDatabaseObjects.Columns.NonStandardConfiguration] == null ? true : item[SPDatabaseObjects.Columns.NonStandardConfiguration];
                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                else if (moduleName == ModuleNames.DRQ)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        //Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (AssetMappedList != null && item[SPDatabaseObjects.Columns.AssetLookup] != null)
                                    {
                                        if (item[SPDatabaseObjects.Columns.AssetLookup].ToString() != "Microsoft.SharePoint.Client.FieldLookupValue[]")
                                            targetAssetId = UGITUtility.StringToLong(AssetMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.AssetLookup]).LookupId.ToString()));
                                    }
                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();

                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? true : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketToBeSentByDate] = item[SPDatabaseObjects.Columns.TicketToBeSentByDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketToBeSentByDate];
                                            targetItem[DatabaseObjects.Columns.ScheduledEndDateTime] = item[SPDatabaseObjects.Columns.ScheduledEndDateTime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ScheduledEndDateTime];
                                            targetItem[DatabaseObjects.Columns.ScheduledStartDateTime] = item[SPDatabaseObjects.Columns.ScheduledStartDateTime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ScheduledStartDateTime];
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            targetItem["DRBRImpactChoice"] = item["TicketDRBRImpact"] == null ? DBNull.Value : item["TicketDRBRImpact"];
                                            targetItem["DRQChangeTypeChoice"] = item["DRQChangeType"] == null ? DBNull.Value : item["DRQChangeType"];
                                            targetItem["DRReplicationChangeChoice"] = item["DRReplicationChange"] == null ? DBNull.Value : item["DRReplicationChange"];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            if (targetAssetId > 0)
                                                targetItem[DatabaseObjects.Columns.AssetLookup] = targetAssetId;
                                            targetItem[DatabaseObjects.Columns.Duration] = item[SPDatabaseObjects.Columns.TicketDuration] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDuration];
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = item[SPDatabaseObjects.Columns.TicketTargetStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetStartDate];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;
                                            ClientOM.FieldLookupValue TicketAPPId = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                            if (TicketAPPId != null)
                                            {
                                                if (TicketAPPId.LookupId > 0 && TicketAPPId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.APPTitleLookup] = TicketAPPId.LookupId;
                                                    SPTicketrefernces.Add("SPAPPId", Convert.ToString(TicketAPPId.LookupId));
                                                    SPTicketrefernces.Add("SPAPPTicketId", UGITUtility.ObjectToString(TicketAPPId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }
                                            ClientOM.FieldLookupValue com = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                                            if (com != null && companylist != null)
                                            {
                                                long id = Convert.ToInt64(companylist.GetTargetID(Convert.ToString(com.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = id;
                                            }
                                            ClientOM.FieldLookupValue division = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;
                                            if (division != null && divsionlist != null)
                                            {
                                                long id = Convert.ToInt64(divsionlist.GetTargetID(Convert.ToString(division.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DivisionLookup] = id;
                                            }
                                            ClientOM.FieldUserValue[] rusers_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (rusers_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(rusers_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldLookupValue drqtype = item[SPDatabaseObjects.Columns.DRQRapidTypeLookup] as ClientOM.FieldLookupValue;
                                            if (drqtype != null && drqlist != null)
                                            {
                                                long id = Convert.ToInt64(drqlist.GetTargetID(Convert.ToString(drqtype.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DRQRapidTypeLookup] = id;
                                            }
                                            ClientOM.FieldLookupValue[] drqlisttype = item[SPDatabaseObjects.Columns.DRQSystemsLookup] as ClientOM.FieldLookupValue[];
                                            if (drqlisttype != null && drqSystemArealist != null)
                                            {
                                                targetItem["DRQSystemsLookup"] = drqSystemArealist.GetTargetIDs(drqlisttype.Select(x => x.LookupId.ToString()).ToList());
                                            }
                                            ClientOM.FieldUserValue _infrausers = item[SPDatabaseObjects.Columns.TicketInfrastructureManager] as ClientOM.FieldUserValue;
                                            if (_infrausers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInfrastructureManager] = userList.GetTargetID(Convert.ToString(_infrausers.LookupId));
                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());
                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;

                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                            if (multlocation != null && multlocation.Length > 0 && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());


                                            targetItem["RiskChoice"] = item["TicketRisk"] == null ? DBNull.Value : item["TicketRisk"];
                                            targetItem["Outage"] = item["TicketOutage"] == null ? DBNull.Value : item["TicketOutage"];
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem["ProductionVerificationPlan"] = item["TicketProductionVerificationPlan"] == null ? DBNull.Value : item["TicketProductionVerificationPlan"];
                                            targetItem["RapidRequest"] = item["TicketRapidRequest"] == null ? DBNull.Value : item["TicketRapidRequest"];
                                            targetItem["RecoveryPlan"] = item["TicketRecoveryPlan"] == null ? DBNull.Value : item["TicketRecoveryPlan"];
                                            targetItem[DatabaseObjects.Columns.RelatedRequestID] = item[DatabaseObjects.Columns.RelatedRequestID] == null ? DBNull.Value : item[DatabaseObjects.Columns.RelatedRequestID];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            targetItem[DatabaseObjects.Columns.AutoSend] = item[SPDatabaseObjects.Columns.AutoSend] == null ? false : item[SPDatabaseObjects.Columns.AutoSend];
                                            targetItem["ChangePurpose"] = item["ChangePurpose"] == null ? DBNull.Value : item["ChangePurpose"];
                                            targetItem["DeploymentPlan"] = item["DeploymentPlan"] == null ? DBNull.Value : item["DeploymentPlan"];
                                            targetItem[DatabaseObjects.Columns.DeskLocation] = item[SPDatabaseObjects.Columns.DeskLocation] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.DeskLocation];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.OutageHours] = item[SPDatabaseObjects.Columns.OutageHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OutageHours];
                                            targetItem["UserImpactDetails"] = item["TicketUserImpactDetails"] == null ? DBNull.Value : item["TicketUserImpactDetails"];
                                            targetItem["TestingDoneChoice"] = item["TicketTestingDone"] == null ? DBNull.Value : item["TicketTestingDone"];
                                            targetItem["ImpactsOrganization"] = item["ImpactsOrganization"] == null ? false : item["ImpactsOrganization"];
                                            targetItem["DRBRDescription"] = item["TicketDRBRDescription"] == null ? false : item["TicketDRBRDescription"];
                                            targetItem["NotificationText"] = item["NotificationText"] == null ? false : item["NotificationText"];

                                            ClientOM.FieldUserValue useraffected = item["TicketUsersAffected"] as ClientOM.FieldUserValue;
                                            if (useraffected != null && userList != null)
                                                targetItem["UsersAffectedUser"] = userList.GetTargetID(Convert.ToString(useraffected.LookupId));

                                            ClientOM.FieldUserValue drbrmgr = item["TicketDRBRManager"] as ClientOM.FieldUserValue;
                                            if (drbrmgr != null && userList != null)
                                                targetItem["DRBRManagerUser"] = userList.GetTargetID(Convert.ToString(drbrmgr.LookupId));

                                            ClientOM.FieldUserValue[] testeruser = item[SPDatabaseObjects.Columns.TicketTester] as ClientOM.FieldUserValue[];
                                            if (testeruser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketTester] = userList.GetTargetIDs(testeruser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] proderuser = item["ProductionVerifyResponsible"] as ClientOM.FieldUserValue[];
                                            if (proderuser != null && userList != null)
                                                targetItem["ProductionVerifyResponsibleUser"] = userList.GetTargetIDs(proderuser.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));

                                            ClientOM.FieldUserValue[] deploymgr = item["DeploymentResponsible"] as ClientOM.FieldUserValue[];
                                            if (deploymgr != null && userList != null)
                                                targetItem["DeploymentResponsibleUser"] = userList.GetTargetIDs(deploymgr.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem[DatabaseObjects.Columns.IsUserNotificationRequired] = item[SPDatabaseObjects.Columns.IsUserNotificationRequired] == null ? false : item[SPDatabaseObjects.Columns.IsUserNotificationRequired];

                                            ClientOM.FieldUserValue appusers = item[SPDatabaseObjects.Columns.TicketApplicationManager] as ClientOM.FieldUserValue;
                                            if (appusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketApplicationManager] = userList.GetTargetID(Convert.ToString(appusers.LookupId));

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());


                                            ClientOM.FieldUserValue[] _rollbackusers = item["RollbackResponsible"] as ClientOM.FieldUserValue[];
                                            if (_rollbackusers != null && _rollbackusers.Length > 0 && userList != null)
                                                targetItem["RollbackResponsibleUser"] = userList.GetTargetIDs(_rollbackusers.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] _adminusers = item[SPDatabaseObjects.Columns.TicketSecurityManager] as ClientOM.FieldUserValue[];
                                            if (_adminusers != null && _adminusers.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketSecurityManager] = userList.GetTargetIDs(_adminusers.Select(x => x.LookupId.ToString()).ToList());

                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }
                                            ClientOM.FieldLookupValue[] _TicketBeneficiaries = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                            if (_TicketBeneficiaries != null && _TicketBeneficiaries.Length > 0 && DepartmentList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(_TicketBeneficiaries.Select(x => x.LookupId.ToString()).ToList());
                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.RCA)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;

                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketAnalysisDetails] = item[SPDatabaseObjects.Columns.TicketAnalysisDetails] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketAnalysisDetails];
                                            ClientOM.FieldLookupValue com = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                                            if (com != null && companylist != null)
                                            {
                                                long id = Convert.ToInt64(companylist.GetTargetID(Convert.ToString(com.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = id;
                                            }
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.DeskLocation] = item[SPDatabaseObjects.Columns.DeskLocation] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.DeskLocation];
                                            ClientOM.FieldLookupValue division = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;
                                            if (division != null && divsionlist != null)
                                            {
                                                long id = Convert.ToInt64(divsionlist.GetTargetID(Convert.ToString(division.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DivisionLookup] = id;
                                            }
                                            targetItem[DatabaseObjects.Columns.DocumentLibraryName] = item[SPDatabaseObjects.Columns.DocumentLibraryName];
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            targetItem["ManagerApprovalNeeded"] = item["ManagerApprovalNeeded"] == null ? false : item["ManagerApprovalNeeded"];
                                            if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                                targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));
                                            if (targetLocationID > 0)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;

                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];

                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.ReopenCount] = item[SPDatabaseObjects.Columns.ReopenCount] == null ? 0 : item[SPDatabaseObjects.Columns.ReopenCount];
                                            targetItem[DatabaseObjects.Columns.RCATypeChoice] = item[SPDatabaseObjects.Columns.RCAType];
                                            if (item[SPDatabaseObjects.Columns.TicketReSubmissionDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketReSubmissionDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketReSubmissionDate]);
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            targetItem[DatabaseObjects.Columns.TicketResolutionType] = item[SPDatabaseObjects.Columns.TicketResolutionType];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            if (item[SPDatabaseObjects.Columns.ResolutionDate] != null)
                                                targetItem[DatabaseObjects.Columns.ResolutionDate] = item[SPDatabaseObjects.Columns.ResolutionDate];
                                            targetItem[SPDatabaseObjects.Columns.UGITIssueType] = item[SPDatabaseObjects.Columns.UGITIssueType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITIssueType];
                                            targetItem["Justification"] = item["Justification"] == null ? DBNull.Value : item["Justification"];
                                            targetItem["CorrectiveActions"] = item["CorrectiveActions"] == null ? DBNull.Value : item["CorrectiveActions"];
                                            targetItem["ContributingCauses"] = item["ContributingCauses"] == null ? DBNull.Value : item["ContributingCauses"];
                                            targetItem["MonitoringToolNotifiable"] = item["MonitoringToolNotifiable"] == null ? false : item["MonitoringToolNotifiable"];
                                            targetItem["RootCause"] = item["RootCause"] == null ? DBNull.Value : item["RootCause"];
                                            targetItem["WhyFinalVerify"] = item["WhyFinalVerify"] == null ? false : item["WhyFinalVerify"];
                                            targetItem["WhyFinal"] = item["WhyFinal"] == null ? DBNull.Value : item["WhyFinal"];
                                            targetItem["TemporaryCountermeasure"] = item["TemporaryCountermeasure"] == null ? DBNull.Value : item["TemporaryCountermeasure"];
                                            targetItem["Why1"] = item["Why1"] == null ? DBNull.Value : item["Why1"];
                                            targetItem["Why2"] = item["Why2"] == null ? DBNull.Value : item["Why2"];
                                            targetItem["Why3"] = item["Why3"] == null ? DBNull.Value : item["Why3"];
                                            targetItem["Why4"] = item["Why4"] == null ? DBNull.Value : item["Why4"];
                                            targetItem["Why5"] = item["Why5"] == null ? DBNull.Value : item["Why5"];
                                            targetItem["OccurrenceDetails"] = item["OccurrenceDetails"] == null ? DBNull.Value : item["OccurrenceDetails"];
                                            //targetItem["Rejected"] = item["Rejected"] == null ? false : item["Rejected"];

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = item[SPDatabaseObjects.Columns.TicketTargetStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetStartDate];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] PRPgrp = item[SPDatabaseObjects.Columns.PRPGroup] as ClientOM.FieldUserValue[];
                                            if (PRPgrp != null && PRPgrp.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.PRPGroup] = userList.GetTargetIDs(PRPgrp.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketAge];

                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];

                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));


                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.FinalCountermeasure] = Convert.ToString(item[SPDatabaseObjects.Columns.FinalCountermeasure]);
                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.BTS)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();

                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;

                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            ClientOM.FieldLookupValue com = item[SPDatabaseObjects.Columns.CompanyTitleLookup] as ClientOM.FieldLookupValue;
                                            if (com != null && companylist != null)
                                            {
                                                long id = Convert.ToInt64(companylist.GetTargetID(Convert.ToString(com.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = id;
                                            }
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            if (DepartmentId > 0)
                                                targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;

                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.DeskLocation] = item[SPDatabaseObjects.Columns.DeskLocation] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.DeskLocation];
                                            ClientOM.FieldLookupValue division = item[SPDatabaseObjects.Columns.DivisionLookup] as ClientOM.FieldLookupValue;
                                            if (division != null && divsionlist != null)
                                            {
                                                long id = Convert.ToInt64(divsionlist.GetTargetID(Convert.ToString(division.LookupId)));
                                                if (id > 0)
                                                    targetItem[DatabaseObjects.Columns.DivisionLookup] = id;
                                            }
                                            targetItem[DatabaseObjects.Columns.DocumentLibraryName] = item[SPDatabaseObjects.Columns.DocumentLibraryName];
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;

                                            if (targetImpactId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;
                                            if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                                targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));
                                            if (targetLocationID > 0)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;

                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.CategoryChoice] = item[SPDatabaseObjects.Columns.TicketCategory];
                                            targetItem[DatabaseObjects.Columns.ElevatedPriority] = item[SPDatabaseObjects.Columns.ElevatedPriority] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ElevatedPriority];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[SPDatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[SPDatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            targetItem[DatabaseObjects.Columns.TicketResolutionType] = item[SPDatabaseObjects.Columns.TicketResolutionType];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            if (item[SPDatabaseObjects.Columns.ResolutionDate] != null)
                                                targetItem[DatabaseObjects.Columns.ResolutionDate] = item[SPDatabaseObjects.Columns.ResolutionDate];

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            if (ModuleStepId > 0)
                                                targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTargetStartDate] = item[SPDatabaseObjects.Columns.TicketTargetStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetStartDate];
                                            if (targetRequestTypeID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);



                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue PRPgrp = item[SPDatabaseObjects.Columns.PRPGroup] as ClientOM.FieldUserValue;
                                            if (PRPgrp != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PRPGroup] = userList.GetTargetID(Convert.ToString(PRPgrp.LookupId));

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] testeruser = item[SPDatabaseObjects.Columns.TicketTester] as ClientOM.FieldUserValue[];
                                            if (testeruser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketTester] = userList.GetTargetIDs(testeruser.Select(x => x.LookupId.ToString()).ToList());


                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            ClientOM.FieldUserValue closedusers = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (closedusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketClosedBy] = userList.GetTargetID(Convert.ToString(closedusers.LookupId));


                                            ClientOM.FieldUserValue resolvedusers = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (resolvedusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketResolvedBy] = userList.GetTargetID(Convert.ToString(resolvedusers.LookupId));

                                            ClientOM.FieldUserValue assingedusers = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (assingedusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketAssignedByUser] = userList.GetTargetID(Convert.ToString(assingedusers.LookupId));

                                            ClientOM.FieldUserValue tester2users = item["TicketTester2"] as ClientOM.FieldUserValue;
                                            if (tester2users != null && userList != null)
                                                targetItem["Tester2User"] = userList.GetTargetID(Convert.ToString(tester2users.LookupId));

                                            targetItem[DatabaseObjects.Columns.ReopenCount] = item[SPDatabaseObjects.Columns.ReopenCount] == null ? 0 : item[SPDatabaseObjects.Columns.ReopenCount];


                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketAge];

                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }
                                            // Missing code added for BTS
                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.ClientLookup] = item[SPDatabaseObjects.Columns.ClientLookup] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ClientLookup];
                                            targetItem[DatabaseObjects.Columns.ReleaseID] = Convert.ToString(item[SPDatabaseObjects.Columns.ReleaseID]);
                                            targetItem[DatabaseObjects.Columns.ReleaseDate] = item[SPDatabaseObjects.Columns.ReleaseDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ReleaseDate];

                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }
                                    }
                                }
                                else if (moduleName == ModuleNames.INC)
                                {
                                    if (targetItem == null)
                                    {
                                        targetItem = ticketSchema.NewRow();
                                        try
                                        {
                                            targetItem[DatabaseObjects.Columns.TicketActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            //targetItem["AffectedUsersUser"] = item["AffectedUsersUser"];
                                            if (AssetMappedList != null && item[SPDatabaseObjects.Columns.AssetLookup] != null)
                                            {
                                                ClientOM.FieldLookupValue[] AssetLookups = item[SPDatabaseObjects.Columns.AssetLookup] as ClientOM.FieldLookupValue[];
                                                if (AssetLookups != null && AssetLookups.Length > 0 && AssetMappedList != null)
                                                    targetItem[DatabaseObjects.Columns.AssetLookup] = AssetMappedList.GetTargetIDs(AssetLookups.Select(x => x.LookupId.ToString()).ToList());
                                            }
                                            //targetItem[DatabaseObjects.Columns.AssetMultiLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.C);
                                            ClientOM.FieldLookupValue[] _TicketBeneficiaries = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                            if (_TicketBeneficiaries != null && _TicketBeneficiaries.Length > 0 && DepartmentList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(_TicketBeneficiaries.Select(x => x.LookupId.ToString()).ToList());

                                            targetItem[DatabaseObjects.Columns.TicketBusinessManager] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketBusinessManager);
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.CompanyTitleLookup) != null)
                                                targetItem[DatabaseObjects.Columns.CompanyTitleLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.CompanyTitleLookup);
                                            if (item[SPDatabaseObjects.Columns.TicketCreationDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            if (item[DatabaseObjects.Columns.CurrentStageStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DepartmentLookup) != null)
                                                if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DepartmentLookup) != null)
                                                    targetItem[DatabaseObjects.Columns.DepartmentLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DepartmentLookup);
                                            targetItem[DatabaseObjects.Columns.Description] = item[SPDatabaseObjects.Columns.TicketDescription];
                                            targetItem[DatabaseObjects.Columns.DeskLocation] = item[SPDatabaseObjects.Columns.DeskLocation];
                                            if (item[SPDatabaseObjects.Columns.DetectionDate] != null)
                                                targetItem[DatabaseObjects.Columns.DetectionDate] = item[SPDatabaseObjects.Columns.DetectionDate];
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DivisionLookup) != null)
                                                targetItem[DatabaseObjects.Columns.DivisionLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DivisionLookup);
                                            targetItem[DatabaseObjects.Columns.DocumentLibraryName] = item[DatabaseObjects.Columns.DocumentLibraryName];
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.FunctionalAreaLookup) != null)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.FunctionalAreaLookup);
                                            targetItem[DatabaseObjects.Columns.ImpactsOrganization] = item[DatabaseObjects.Columns.ImpactsOrganization];
                                            targetItem[DatabaseObjects.Columns.TicketInitiator] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketInitiator);
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.LocationLookup) != null)
                                                targetItem[DatabaseObjects.Columns.LocationLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.LocationLookup);
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.LocationMultLookup) != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.LocationMultLookup);
                                            targetItem[DatabaseObjects.Columns.ModuleStepLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.ModuleStepLookup);
                                            targetItem[DatabaseObjects.Columns.NextSLATime] = item[DatabaseObjects.Columns.NextSLATime] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLATime];
                                            targetItem[DatabaseObjects.Columns.NextSLAType] = item[DatabaseObjects.Columns.NextSLAType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NextSLAType];
                                            if (item[DatabaseObjects.Columns.OccurrenceDate] != null)
                                                targetItem[DatabaseObjects.Columns.OccurrenceDate] = item[SPDatabaseObjects.Columns.OccurrenceDate];

                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];

                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = item[SPDatabaseObjects.Columns.TicketOnHoldTillDate];

                                            targetItem[DatabaseObjects.Columns.TicketORP] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketORP);
                                            targetItem[DatabaseObjects.Columns.Owner] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketOwner);
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup) != null)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup);
                                            targetItem[DatabaseObjects.Columns.TicketPRP] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketPRP);
                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.PRSLookup) != null)
                                                targetItem[DatabaseObjects.Columns.PRSLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.PRSLookup);
                                            targetItem[DatabaseObjects.Columns.ReopenCount] = item[SPDatabaseObjects.Columns.ReopenCount] == null ? 0 : Convert.ToInt32(item[SPDatabaseObjects.Columns.ReopenCount]);
                                            targetItem[DatabaseObjects.Columns.TicketRequestor] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketRequestor);
                                            targetItem[DatabaseObjects.Columns.RequestorContacted] = item[SPDatabaseObjects.Columns.RequestorContacted] == null ? 0 : Convert.ToInt32(item[SPDatabaseObjects.Columns.RequestorContacted]);
                                            targetItem[DatabaseObjects.Columns.RequestSource] = item[SPDatabaseObjects.Columns.TicketRequestSource];
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketRequestTypeLookup);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.TicketAge] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketAge]);
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];

                                            if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketSeverityLookup) != null)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketSeverityLookup);
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }
                                            targetItem[DatabaseObjects.Columns.DataEditors] = Convert.ToString(item[SPDatabaseObjects.Columns.DataEditor]);
                                            if (item[SPDatabaseObjects.Columns.ResolutionDate] != null)
                                                targetItem[DatabaseObjects.Columns.ResolutionDate] = item[SPDatabaseObjects.Columns.ResolutionDate];
                                            targetItem[DatabaseObjects.Columns.OutageHours] = item[SPDatabaseObjects.Columns.OutageHours] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OutageHours];



                                            // Missing code added for INC

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.IssueTypeChoice] = item[SPDatabaseObjects.Columns.UGITIssueType] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.UGITIssueType];

                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));


                                            ClientOM.FieldUserValue _ResolvedByUser = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (_ResolvedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ResolvedByUser] = userList.GetTargetID(Convert.ToString(_ResolvedByUser.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];


                                            ticketSchema.Rows.Add(targetItem);
                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }


                                    }

                                }
                                else if (moduleName == ModuleNames.PRS)
                                {
                                    if (locationMappedList != null && item[SPDatabaseObjects.Columns.LocationLookup] != null)
                                        targetLocationID = UGITUtility.StringToLong(locationMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.LocationLookup]).LookupId.ToString()));

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));

                                    if (AssetMappedList != null && item[SPDatabaseObjects.Columns.AssetLookup] != null)
                                    {
                                        if (item[SPDatabaseObjects.Columns.AssetLookup].ToString() != "Microsoft.SharePoint.Client.FieldLookupValue[]")
                                            targetAssetId = UGITUtility.StringToLong(AssetMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.AssetLookup]).LookupId.ToString()));
                                    }

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (DepartmentList != null && item[SPDatabaseObjects.Columns.DepartmentLookup] != null)
                                        DepartmentId = UGITUtility.StringToLong(DepartmentList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.DepartmentLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (TicketSeverityList != null && item[SPDatabaseObjects.Columns.TicketSeverityLookup] != null)
                                    {
                                        TicketSeverityId = UGITUtility.StringToLong(TicketSeverityList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketSeverityLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            targetItem[DatabaseObjects.Columns.LocationLookup] = targetLocationID;
                                            targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                            targetItem[DatabaseObjects.Columns.AssetLookup] = targetAssetId;
                                            targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                            targetItem[DatabaseObjects.Columns.DepartmentLookup] = DepartmentId;
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.SLADisabled] = item[SPDatabaseObjects.Columns.SLADisabled] == null ? true : item[SPDatabaseObjects.Columns.SLADisabled];
                                            targetItem[DatabaseObjects.Columns.TicketAge] = item[SPDatabaseObjects.Columns.TicketAge];
                                            targetItem[DatabaseObjects.Columns.ProjectName] = item[SPDatabaseObjects.Columns.ProjectName];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            targetItem[DatabaseObjects.Columns.TicketInitiatorResolved] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketInitiatorResolved]);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            targetItem[DatabaseObjects.Columns.TicketResolutionType] = item[SPDatabaseObjects.Columns.TicketResolutionType];
                                            targetItem[DatabaseObjects.Columns.RequestSource] = item[SPDatabaseObjects.Columns.TicketRequestSource];
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue _prpgrpusers = item[SPDatabaseObjects.Columns.PRPGroup] as ClientOM.FieldUserValue;
                                            if (_prpgrpusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.PRPGroup] = userList.GetTargetID(Convert.ToString(_prpgrpusers.LookupId));

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                            ClientOM.FieldUserValue usersPRP = item[SPDatabaseObjects.Columns.TicketPRP] as ClientOM.FieldUserValue;
                                            if (usersPRP != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketPRP] = userList.GetTargetID(Convert.ToString(usersPRP.LookupId));

                                            ClientOM.FieldUserValue[] usersORP = item[SPDatabaseObjects.Columns.TicketORP] as ClientOM.FieldUserValue[];
                                            if (usersORP != null && usersORP.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketORP] = userList.GetTargetIDs(usersORP.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }
                                            // Missing code added for PRS

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.TaskActualHours] = item[SPDatabaseObjects.Columns.TicketActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketActualHours];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];
                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _ResolvedByUser = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (_ResolvedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ResolvedByUser] = userList.GetTargetID(Convert.ToString(_ResolvedByUser.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];

                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            ClientOM.FieldUserValue _DepartmentManagerUser = item[SPDatabaseObjects.Columns.TicketDepartmentManager] as ClientOM.FieldUserValue;
                                            if (_DepartmentManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DepartmentManagerUser] = userList.GetTargetID(Convert.ToString(_DepartmentManagerUser.LookupId));

                                            ClientOM.FieldUserValue _DivisionManagerUser = item[SPDatabaseObjects.Columns.TicketDivisionManager] as ClientOM.FieldUserValue;
                                            if (_DivisionManagerUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.DivisionManagerUser] = userList.GetTargetID(Convert.ToString(_DivisionManagerUser.LookupId));

                                            ClientOM.FieldUserValue _ApproverUser = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue;
                                            if (_ApproverUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ApproverUser] = userList.GetTargetID(Convert.ToString(_ApproverUser.LookupId));

                                            ClientOM.FieldUserValue _Approver2User = item[SPDatabaseObjects.Columns.Approver2] as ClientOM.FieldUserValue;
                                            if (_Approver2User != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.Approver2User] = userList.GetTargetID(Convert.ToString(_Approver2User.LookupId));

                                            targetItem[DatabaseObjects.Columns.UserQuestionSummary] = item[SPDatabaseObjects.Columns.UserQuestionSummary];
                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];

                                            ticketSchema.Rows.Add(targetItem);

                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                if (moduleName == ModuleNames.CMT)
                                {
                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));

                                    if (AssetMappedList != null && item[SPDatabaseObjects.Columns.AssetLookup] != null)
                                    {
                                        ClientOM.FieldLookupValue[] AssetLookups = item[SPDatabaseObjects.Columns.AssetLookup] as ClientOM.FieldLookupValue[];
                                        if (AssetLookups != null && AssetLookups.Length > 0 && AssetMappedList != null)
                                            targetItem[DatabaseObjects.Columns.AssetLookup] = AssetMappedList.GetTargetIDs(AssetLookups.Select(x => x.LookupId.ToString()).ToList());
                                    }

                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }

                                    if (targetItem == null)
                                    {
                                        try
                                        {
                                            targetItem = ticketSchema.NewRow();
                                            if (targetPriorityID > 0)
                                                targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                            if (FunctonalAreaId > 0)
                                                targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                            targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                            targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                            targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.TicketEstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.TicketEstimatedHours];
                                            targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                            targetItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = item[SPDatabaseObjects.Columns.TicketTotalHoldDuration] == null ? 0 : item[SPDatabaseObjects.Columns.TicketTotalHoldDuration];
                                            targetItem[DatabaseObjects.Columns.TicketOnHold] = item[SPDatabaseObjects.Columns.TicketOnHold] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketOnHold];
                                            targetItem[DatabaseObjects.Columns.OnHoldReasonChoice] = item[SPDatabaseObjects.Columns.OnHoldReason] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.OnHoldReason];
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                            if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                                targetItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                            targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                            targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                            targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                            targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                            targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                            targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeWorkflow]).LookupValue);
                                            targetItem[DatabaseObjects.Columns.ResolutionComments] = item[SPDatabaseObjects.Columns.TicketResolutionComments];
                                            targetItem[DatabaseObjects.Columns.RequestSource] = item[SPDatabaseObjects.Columns.TicketRequestSource];
                                            targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;

                                            ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                            if (multlocation != null && multlocation.Length > 0 && locationMappedList != null)
                                                targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());

                                            if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);

                                            ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                            if (_users != null && _users.Length > 0 && userList != null)
                                                targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                            if (users_ != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());

                                            ClientOM.FieldUserValue iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue;
                                            if (iusers != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetID(Convert.ToString(iusers.LookupId));

                                            targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                            targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                            targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                            if (TicketSeverityId > 0)
                                                targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                            ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                            if (businessmgr != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));
                                            stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                            if (stageids != null && targetStagetMappedList != null)
                                            {
                                                targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                            }

                                            // Missing code added for CMT

                                            targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                            targetItem[DatabaseObjects.Columns.TotalActualHours] = item[SPDatabaseObjects.Columns.TotalActualHours] == null ? 0 : item[SPDatabaseObjects.Columns.TotalActualHours];

                                            ClientOM.FieldUserValue _ClosedByUser = item[SPDatabaseObjects.Columns.TicketClosedBy] as ClientOM.FieldUserValue;
                                            if (_ClosedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ClosedByUser] = userList.GetTargetID(Convert.ToString(_ClosedByUser.LookupId));

                                            ClientOM.FieldUserValue _ResolvedByUser = item[SPDatabaseObjects.Columns.TicketResolvedBy] as ClientOM.FieldUserValue;
                                            if (_ResolvedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.ResolvedByUser] = userList.GetTargetID(Convert.ToString(_ResolvedByUser.LookupId));

                                            ClientOM.FieldLookupValue _VendorLookup = item[SPDatabaseObjects.Columns.VendorLookup] as ClientOM.FieldLookupValue;
                                            if (_VendorLookup != null && assetvendorList != null)
                                                targetItem[DatabaseObjects.Columns.VendorLookup] = assetvendorList.GetTargetID(Convert.ToString(_VendorLookup.LookupId));

                                            ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                            if (_AssignedByUser != null && userList != null)
                                                targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                            targetItem[DatabaseObjects.Columns.Archived] = item[SPDatabaseObjects.Columns.Archived] == null ? true : item[SPDatabaseObjects.Columns.Archived];
                                            targetItem[DatabaseObjects.Columns.FriendlyName] = item[SPDatabaseObjects.Columns.FriendlyName];
                                            targetItem[DatabaseObjects.Columns.Handle] = item[SPDatabaseObjects.Columns.Handle];
                                            targetItem[DatabaseObjects.Columns.HasPrivateKey] = item[SPDatabaseObjects.Columns.HasPrivateKey] == null ? true : item[SPDatabaseObjects.Columns.HasPrivateKey];
                                            targetItem[DatabaseObjects.Columns.IssuerName] = item[SPDatabaseObjects.Columns.IssuerName];
                                            targetItem[DatabaseObjects.Columns.NotAfter] = item[SPDatabaseObjects.Columns.NotAfter] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NotAfter];
                                            targetItem[DatabaseObjects.Columns.NotBefore] = item[SPDatabaseObjects.Columns.NotBefore] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.NotBefore];
                                            targetItem[DatabaseObjects.Columns.PublicKey] = item[SPDatabaseObjects.Columns.PublicKey];
                                            targetItem[DatabaseObjects.Columns.RawData] = item[SPDatabaseObjects.Columns.RawData];
                                            targetItem[DatabaseObjects.Columns.SignatureAlgorithm] = item[SPDatabaseObjects.Columns.SignatureAlgorithm];
                                            targetItem[DatabaseObjects.Columns.Subject] = item[SPDatabaseObjects.Columns.Subject];
                                            targetItem[DatabaseObjects.Columns.SubjectName] = item[SPDatabaseObjects.Columns.SubjectName];
                                            targetItem[DatabaseObjects.Columns.Thumbprint] = item[SPDatabaseObjects.Columns.Thumbprint];
                                            targetItem[DatabaseObjects.Columns.DnsNameList] = item[SPDatabaseObjects.Columns.DnsNameList];
                                            targetItem[DatabaseObjects.Columns.EnhancedKeyUsageList] = item[SPDatabaseObjects.Columns.EnhancedKeyUsageList];
                                            targetItem[DatabaseObjects.Columns.SendAsTrustedIssuer] = item[SPDatabaseObjects.Columns.SendAsTrustedIssuer] == null ? true : item[SPDatabaseObjects.Columns.SendAsTrustedIssuer];
                                            targetItem[DatabaseObjects.Columns.PaymentTerms] = item[SPDatabaseObjects.Columns.PaymentTerms];
                                            targetItem[DatabaseObjects.Columns.ManufacturingContact] = item[SPDatabaseObjects.Columns.ManufacturingContact];
                                            targetItem[DatabaseObjects.Columns.VendorContact] = item[SPDatabaseObjects.Columns.VendorContact];
                                            targetItem[DatabaseObjects.Columns.SalesRepName] = item[SPDatabaseObjects.Columns.SalesRepName];
                                            targetItem[DatabaseObjects.Columns.CurrentFundingProject] = item[SPDatabaseObjects.Columns.CurrentFundingProject];
                                            targetItem[DatabaseObjects.Columns.CurrentGWOFields] = item[SPDatabaseObjects.Columns.CurrentGWOFields];
                                            ClientOM.FieldLookupValue TicketAPPId = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                            if (TicketAPPId != null)
                                            {
                                                if (TicketAPPId.LookupId > 0 && TicketAPPId.LookupValue != null)
                                                {
                                                    Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                    targetItem[DatabaseObjects.Columns.APPTitleLookup] = TicketAPPId.LookupId;
                                                    SPTicketrefernces.Add("SPAPPId", Convert.ToString(TicketAPPId.LookupId));
                                                    SPTicketrefernces.Add("SPAPPTicketId", UGITUtility.ObjectToString(TicketAPPId.LookupValue));
                                                    GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                                }
                                            }
                                            ticketSchema.Rows.Add(targetItem);

                                        }
                                        catch (Exception ex)
                                        {
                                            ULog.WriteException(ex);
                                        }

                                    }
                                }
                                if (moduleName != ModuleNames.CMDB)
                                {
                                    ClientOM.FieldLookupValue ServiceTitleLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                                    if (ServiceTitleLookup != null && ServiceTitleLookup.LookupId > 0)
                                        targetItem[DatabaseObjects.Columns.ServiceTitleLookup] = ServiceTitleLookup.LookupId;
                                }
                                if (string.IsNullOrWhiteSpace(Convert.ToString(targetItem[DatabaseObjects.Columns.TicketId])) || importWithUpdate)
                                {
                                    //foreach (string spFieldName in moduleColumnMapped.Keys)
                                    //{
                                    //    targetVal = string.Empty;
                                    //    string sourceFieldName = moduleColumnMapped[spFieldName];
                                    //    if (!string.IsNullOrWhiteSpace(sourceFieldName))
                                    //    {
                                    //        targetVal = context.GetTargetValue(spContext, list, item, spFieldName, sourceRow: targetItem, sourceFieldName: sourceFieldName);

                                    //        //if (targetVal == null)
                                    //        //    targetItem[sourceFieldName] = DBNull.Value;
                                    //        //else
                                    //        if (targetItem != null)
                                    //            targetItem[sourceFieldName] = targetVal;
                                    //    }
                                    //}
                                }
                                targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = context.CovertActionUserTypes(Convert.ToString(targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), moduleColumnMapped);
                                targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context.AppContext, Convert.ToString(targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), targetItem);
                                if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Author) != null)
                                    targetItem[DatabaseObjects.Columns.CreatedByUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Author);
                                else
                                    targetItem[DatabaseObjects.Columns.CreatedByUser] = Convert.ToString(Guid.Empty);

                                if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Editor) != null)
                                    targetItem[DatabaseObjects.Columns.ModifiedByUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Editor);
                                else
                                    targetItem[DatabaseObjects.Columns.ModifiedByUser] = Convert.ToString(Guid.Empty);

                                targetItem[DatabaseObjects.Columns.Created] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem[DatabaseObjects.Columns.Modified] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                targetItem[DatabaseObjects.Columns.TicketId] = item[SPDatabaseObjects.Columns.TicketId];
                                targetItem[DatabaseObjects.Columns.Title] = item[SPDatabaseObjects.Columns.Title];
                                targetItem[DatabaseObjects.Columns.Status] = item[SPDatabaseObjects.Columns.TicketStatus];
                                targetItem[DatabaseObjects.Columns.History] = item[SPDatabaseObjects.Columns.History];
                                targetItem[DatabaseObjects.Columns.Closed] = item[SPDatabaseObjects.Columns.TicketClosed];
                                if (item[SPDatabaseObjects.Columns.TicketCloseDate] != null)
                                    targetItem[DatabaseObjects.Columns.CloseDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketCloseDate]);
                                if (moduleName != ModuleNames.CMDB)
                                    targetItem[DatabaseObjects.Columns.Description] = item[SPDatabaseObjects.Columns.TicketDescription];
                                targetItem[DatabaseObjects.Columns.Comment] = item[SPDatabaseObjects.Columns.TicketComment];
                                if (Convert.ToBoolean(item[SPDatabaseObjects.Columns.Attachments]))
                                {
                                    List<string> lstOfAttachment = new List<string>();
                                    try
                                    {
                                        ClientOM.AttachmentCollection oAttachments = item.AttachmentFiles;
                                        spContext.Load(oAttachments);
                                        spContext.ExecuteQuery();
                                        foreach (ClientOM.Attachment attachment in oAttachments)
                                        {
                                            ClientOM.File file = item.ParentList.ParentWeb.GetFileByServerRelativeUrl(attachment.ServerRelativeUrl);
                                            spContext.Load(file);
                                            spContext.ExecuteQuery();
                                            ClientOM.ClientResult<System.IO.Stream> data = file.OpenBinaryStream();
                                            spContext.Load(file);
                                            spContext.ExecuteQuery();
                                            using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
                                            {
                                                if (data != null)
                                                {
                                                    data.Value.CopyTo(mStream);
                                                    imageArray = mStream.ToArray();
                                                }
                                            }
                                            doctargetItem = null;
                                            if (doctargetItem == null)
                                            {
                                                doctargetItem = new Document();
                                                docData.Add(doctargetItem);
                                                //_targetNewItemCount++;
                                            }
                                            if (doctargetItem.Id == 0)
                                            {
                                                doctargetItem.Name = file.Name;
                                                doctargetItem.FileID = Guid.NewGuid().ToString();
                                                doctargetItem.Blob = imageArray;
                                                doctargetItem.Extension = file.Name.Split('.').Last();
                                                mgr.Insert(doctargetItem);
                                                _targetNewItemCount++;
                                            }

                                            lstOfAttachment.Add(doctargetItem.FileID);
                                        }
                                        targetItem[DatabaseObjects.Columns.Attachments] = string.Join(Constants.Separator6, lstOfAttachment);
                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteLog($"Issue File Attachments conversion");
                                        ULog.WriteException(ex);
                                    }
                                }

                                if (moduleName == ModuleNames.TSR || moduleName == ModuleNames.NPR || moduleName == ModuleNames.ACR || moduleName == ModuleNames.DRQ || moduleName == ModuleNames.PMM || moduleName == ModuleNames.RCA || moduleName == ModuleNames.BTS || moduleName == ModuleNames.INC || moduleName == ModuleNames.PRS || moduleName == ModuleNames.SVC)
                                {
                                    targetItem["CustomUGChoice01"] = item["CustomUGChoice01"] == null ? DBNull.Value : item["CustomUGChoice01"];
                                    targetItem["CustomUGChoice02"] = item["CustomUGChoice02"] == null ? DBNull.Value : item["CustomUGChoice02"];
                                    targetItem["CustomUGChoice03"] = item["CustomUGChoice03"] == null ? DBNull.Value : item["CustomUGChoice03"];
                                    targetItem["CustomUGChoice04"] = item["CustomUGChoice04"] == null ? DBNull.Value : item["CustomUGChoice04"];

                                    targetItem["CustomUGDate01"] = item["CustomUGDate01"] == null ? DBNull.Value : item["CustomUGDate01"];
                                    targetItem["CustomUGDate02"] = item["CustomUGDate02"] == null ? DBNull.Value : item["CustomUGDate02"];
                                    targetItem["CustomUGDate03"] = item["CustomUGDate03"] == null ? DBNull.Value : item["CustomUGDate03"];
                                    targetItem["CustomUGDate04"] = item["CustomUGDate04"] == null ? DBNull.Value : item["CustomUGDate04"];

                                    targetItem["CustomUGText01"] = item["CustomUGText01"] == null ? DBNull.Value : item["CustomUGText01"];
                                    targetItem["CustomUGText02"] = item["CustomUGText02"] == null ? DBNull.Value : item["CustomUGText02"];
                                    targetItem["CustomUGText03"] = item["CustomUGText03"] == null ? DBNull.Value : item["CustomUGText03"];
                                    targetItem["CustomUGText04"] = item["CustomUGText04"] == null ? DBNull.Value : item["CustomUGText04"];
                                    targetItem["CustomUGText05"] = item["CustomUGText05"] == null ? DBNull.Value : item["CustomUGText05"];
                                    targetItem["CustomUGText06"] = item["CustomUGText06"] == null ? DBNull.Value : item["CustomUGText06"];
                                    targetItem["CustomUGText07"] = item["CustomUGText07"] == null ? DBNull.Value : item["CustomUGText07"];
                                    targetItem["CustomUGText08"] = item["CustomUGText08"] == null ? DBNull.Value : item["CustomUGText08"];

                                    targetItem["CustomUGUser01"] = item["CustomUGUser01"] == null ? DBNull.Value : item["CustomUGUser01"];
                                    targetItem["CustomUGUser02"] = item["CustomUGUser02"] == null ? DBNull.Value : item["CustomUGUser02"];
                                    targetItem["CustomUGUser03"] = item["CustomUGUser03"] == null ? DBNull.Value : item["CustomUGUser03"];
                                    targetItem["CustomUGUser04"] = item["CustomUGUser04"] == null ? DBNull.Value : item["CustomUGUser04"];

                                    targetItem["CustomUGUserMulti01"] = item["CustomUGUserMulti01"] == null ? DBNull.Value : item["CustomUGUserMulti01"];
                                    targetItem["CustomUGUserMulti02"] = item["CustomUGUserMulti02"] == null ? DBNull.Value : item["CustomUGUserMulti02"];
                                    targetItem["CustomUGUserMulti03"] = item["CustomUGUserMulti03"] == null ? DBNull.Value : item["CustomUGUserMulti03"];
                                    targetItem["CustomUGUserMulti04"] = item["CustomUGUserMulti04"] == null ? DBNull.Value : item["CustomUGUserMulti04"];
                                }
                                int rowEffected = ticketMgr.Save(module, targetItem);

                                if (rowEffected > 0)
                                    targetNewItemCount++;
                                else
                                    ULog.WriteException($"Item: {ticketID} is not updated for table: {module.ModuleTable}");

                                if (targetItem != null && rowEffected > 0 && moduleName == ModuleNames.APP)
                                    appmappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), Convert.ToString(targetItem["ID"])));
                            }
                        }
                        catch (Exception ex1)
                        {
                            ULog.WriteException(ex1);
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} item updated/added {moduleName}");
                    ULog.WriteLog($"{_targetNewItemCount} attachment added for {moduleName}");
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

            ULog.WriteLog($"Updating module emails");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                ClientOM.List list = spContext.Web.Lists.GetByTitle(SPDatabaseObjects.Lists.TaskEmails);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                MappedItemList moduleStageMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                string title = string.Empty;
                ModuleTaskEmail targetItem = null;

                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        //title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        //targetItem = dbData.FirstOrDefault(x => x.Title == title);
                        targetItem = null;
                        if (targetItem == null)
                        {
                            targetItem = new ModuleTaskEmail();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.ModuleNameLookup = moduleName;

                            targetItem.Status = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.TicketStatus]);
                            targetItem.EmailBody = Convert.ToString(item[SPDatabaseObjects.Columns.EmailBody]);
                            targetItem.EmailTitle = Convert.ToString(item[SPDatabaseObjects.Columns.EmailTitle]);

                            targetItem.EmailUserTypes = Convert.ToString(item[SPDatabaseObjects.Columns.EmailUserTypes]);
                            targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                            targetItem.EmailIDCC = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDCC]);
                            targetItem.SendEvenIfStageSkipped = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.SendEvenIfStageSkipped]);
                            targetItem.HideFooter = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.HideTicketFooter]);
                            targetItem.EmailEventType = Convert.ToString(item[SPDatabaseObjects.Columns.EmailEventType]);
                            targetItem.NotifyInPlainText = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.NotifyInPlainText]);
                            targetItem.TicketPriorityLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup);
                            targetItem.EmailUserTypes = context.CovertActionUserTypes(Convert.ToString(targetItem.EmailUserTypes), moduleColumnMapped);
                            ClientOM.FieldLookupValue sourceStagetLookup = item[SPDatabaseObjects.Columns.ModuleStepLookup] as ClientOM.FieldLookupValue;
                            if (sourceStagetLookup != null && sourceStagetLookup.LookupId > 0)
                            {
                                targetItem.StageStep = Convert.ToInt32(moduleStageMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));
                            }
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userList != null)
                                targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userList != null)
                                targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                        }

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);

                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} module emails added");
            }
        }

        public override void UpdateTicketEmails()
        {
            base.UpdateTicketEmails();

            bool import = context.IsImportEnable("TicketEmails", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"{moduleName} Updating TicketEmails");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                EmailsManager mgr = new EmailsManager(context.AppContext);
                List<Email> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                if (import && deleteBeforeImport)
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

                ClientOM.List list = spContext.Web.Lists.GetByTitle(SPDatabaseObjects.Lists.TicketEmails);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                MappedItemList moduleStageMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                string title = string.Empty;


                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        Email targetItem = null;
                        title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        //targetItem = dbData.FirstOrDefault(x => x.Title == title);
                        if (targetItem == null)
                        {
                            targetItem = new Email();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }

                        if (targetItem.ID == 0 || importWithUpdate)
                        {
                            targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem.ModuleNameLookup = moduleName;
                            targetItem.TicketId = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.TicketId]);
                            targetItem.EmailIDTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDTo]);
                            targetItem.EmailStatus = Convert.ToString(item[SPDatabaseObjects.Columns.EmailStatus]);
                            targetItem.EmailIDFrom = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDFrom]);
                            targetItem.EmailReplyTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailReplyTo]);
                            targetItem.EmailIDCC = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDCC]);
                            targetItem.EscalationEmailBody = Convert.ToString(item[SPDatabaseObjects.Columns.EscalationEmailBody]);
                            targetItem.IsIncomingMail = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.IsIncomingMail]);
                            targetItem.MailSubject = Convert.ToString(item[SPDatabaseObjects.Columns.MailSubject]);
                            targetItem.EmailError = Convert.ToString(item[SPDatabaseObjects.Columns.EmailError]);
                            ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                            if (users != null && userList != null)
                                targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(users.LookupId));
                            ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                            if (Modifeduser != null && userList != null)
                                targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                            targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                            targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                        }

                        if (targetItem.ID > 0)
                            mgr.Update(targetItem);
                        else
                            mgr.Insert(targetItem);

                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} TicketEmails added");
            }
        }
        public override void UpdateTicketSLAs()
        {
            base.UpdateTicketSLAs();

            bool import = context.IsImportEnable("TicketSLAs", moduleName);

            if (!import)
                return;

            ULog.WriteLog($"updating Ticket SLAs");


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

            string listName = SPDatabaseObjects.Lists.SLARule;
            //MappedItemList targetMappedList = new MappedItemList(listName);
            MappedItemList targetMappedList = context.GetMappedList(listName);
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {

                int targetNewItemCount = 0;
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                {
                    LifeCycleStageManager lsMgr = new LifeCycleStageManager(context.AppContext);
                    List<LifeCycleStage> lifeCycleStages = lsMgr.Load(x => x.ModuleNameLookup == moduleName);
                    LifeCycleStage selectedStage = null;

                    SlaRulesManager mgr = new SlaRulesManager(context.AppContext);
                    List<ModuleSLARule> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);

                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();

                    string title = string.Empty;
                    ModuleSLARule targetItem = null;

                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);

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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ModuleNameLookup = moduleName;

                                    targetItem.SLACategoryChoice = Convert.ToString(item[SPDatabaseObjects.Columns.SLACategory]);
                                    targetItem.PriorityLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup);
                                    targetItem.SLAHours = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.SLAHours]);
                                    targetItem.SLADaysRoundUpDownChoice = Convert.ToString(item[SPDatabaseObjects.Columns.SLADaysRoundUpDown]);
                                    targetItem.SLATarget = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.SLATarget]);
                                    targetItem.ModuleDescription = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleDescription]);

                                    targetItem.StageTitleLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.StageTitleLookup);
                                    targetItem.EndStageTitleLookup = context.GetTargetValueAsLong(spContext, list, item, SPDatabaseObjects.Columns.EndStageTitleLookup);
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
                                targetMappedList.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    if (import)
                        ULog.WriteLog($"{targetNewItemCount} Ticket SLA added");
                }

                List<MappedItem> allRules = targetMappedList.GetAll();
                targetNewItemCount = 0;
                foreach (MappedItem slaRuleMap in allRules)
                {
                    ModuleEscalationRuleManager mgr = new ModuleEscalationRuleManager(context.AppContext);
                    List<ModuleEscalationRule> dbData = mgr.Load(x => x.SLARuleIdLookup == UGITUtility.StringToLong(slaRuleMap.TargetID));

                    listName = SPDatabaseObjects.Lists.EscalationRule;
                    list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();


                    ModuleEscalationRule targetItem = null;
                    MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    do
                    {
                        ClientOM.CamlQuery spQuery = new ClientOM.CamlQuery();
                        ClientOM.ListItemCollection collection = ContextHelper.GetItemCollectionList(spContext, list, string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq>", SPDatabaseObjects.Columns.SlaRuleIdLookup, slaRuleMap.SourceID), null, position);

                        position = collection.ListItemCollectionPosition;
                        foreach (ClientOM.ListItem item in collection)
                        {

                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
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
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.SLARuleIdLookup = UGITUtility.StringToLong(slaRuleMap.TargetID);


                                    targetItem.EscalationMinutes = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.EscalationMinutes]);
                                    targetItem.EscalationToRoles = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.EscalationToRoles]);
                                    targetItem.EscalationFrequency = UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.EscalationFrequency]);
                                    targetItem.EscalationToEmails = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.EscalationToEmails]);
                                    targetItem.EscalationMailSubject = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.EscalationMailSubject]);
                                    targetItem.EscalationEmailBody = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.EscalationEmailBody]);
                                    targetItem.EscalationDescription = UGITUtility.ObjectToString(item[SPDatabaseObjects.Columns.EscalationDescription]);
                                    targetItem.IncludeActionUsers = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.EscalationDescription]);
                                    targetItem.UseDesiredCompletionDate = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.UseDesiredCompletionDate]);
                                    targetItem.NotifyInPlainText = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.NotifyInPlainText]);
                                }

                                //not found
                                //targetItem.PRP = Convert.ToString(item[SPDatabaseObjects.Columns.PRP]);
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                    } while (position != null);

                }

                if (import)
                    ULog.WriteLog($"{targetNewItemCount} escalation rules added");
            }


        }

        private Dictionary<string, string> LoadModuleColumnMapped()
        {
            ULog.WriteLog("Load module data column mapping");
            Dictionary<string, string> mappedFields = new Dictionary<string, string>();
            List<string> missingColumns = new List<string>();
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                DataTable ticketSchema = null;
                try
                {
                    if (module == null)
                        ULog.WriteException($"Module table not found with name {moduleName}");

                    ticketSchema = ticketMgr.GetDatabaseTableSchema(module.ModuleTable);
                }
                catch
                {
                    ULog.WriteException($"Module table not found with name {module.ModuleTable}");
                    return mappedFields;
                }

                ClientOM.List moduleList = spContext.Web.Lists.GetByTitle(SPDatabaseObjects.Lists.Modules);
                if (moduleName == "WIKI")
                    moduleName = "WIK";

                ClientOM.ListItemCollection moduleCollection = ContextHelper.GetItemCollectionList(spContext, moduleList, string.Format("<Eq><FieldRef Name='ModuleName'/><Value Type='Text'>{0}</Value></Eq>", moduleName), string.Format("<FieldRef Name='{0}'/>", SPDatabaseObjects.Columns.ModuleTicketTable), null);
                if (moduleCollection.Count == 0)
                {
                    ULog.WriteException($"Module {moduleName} is not found in sharepoint");
                    return mappedFields;
                }
                string spModuleTable = Convert.ToString(moduleCollection[0][SPDatabaseObjects.Columns.ModuleTicketTable]);
                if (string.IsNullOrWhiteSpace(spModuleTable))
                {
                    ULog.WriteException($"moduletable is found for module {moduleName} in sharepoint");
                    return mappedFields;
                }

                ClientOM.List list = spContext.Web.Lists.GetByTitle(spModuleTable);
                spContext.Load(list);
                spContext.Load(list.Fields);
                spContext.ExecuteQuery();

                DataRow targetItem = ticketSchema.NewRow();

                foreach (ClientOM.Field spField in list.Fields)
                {
                    if (spField.Sealed || spField.Hidden || spField.InternalName == DatabaseObjects.Columns.Id)
                        continue;


                    string sourceFieldName = context.GetTargetFieldName(list, spField, targetItem);
                    if (string.IsNullOrWhiteSpace(sourceFieldName))
                        missingColumns.Add(spField.InternalName);
                    else
                        mappedFields.Add(spField.InternalName, sourceFieldName);
                }
            }

            if (missingColumns.Count > 0)
            {
                ULog.WriteLog("Missing columns");
                ULog.WriteLog(string.Join(", ", missingColumns));
            }
            return mappedFields;
        }

        public override void UpdateTicketDataHistory()
        {
            base.UpdateTicketDataHistory();

            if (!importData)
                return;
            try
            {
                ULog.WriteLog($"Updating Module data workflow history");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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

                    string listName = SPDatabaseObjects.Lists.ModuleWorkflowHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    int targetNewItemCount = 0;
                    string userid = string.Empty;
                    string fieldName = string.Empty;
                    ModuleWorkflowHistory targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            //fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            //targetItem = dbData.FirstOrDefault(x => x.TicketId == fieldName && x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (userList != null && item[SPDatabaseObjects.Columns.StageClosedBy] != null)
                            {
                                userid = userList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.StageClosedBy]).LookupId.ToString());
                            }
                            if (targetItem != null)
                                continue;

                            if (targetItem == null)
                            {
                                targetItem = new ModuleWorkflowHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.SLAMet = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.SLAMet]);
                                targetItem.Duration = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.Duration]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);

                                targetItem.ActionUserType = Convert.ToString(item[SPDatabaseObjects.Columns.ActionUserType]);
                                targetItem.StageClosedByName = Convert.ToString(item[SPDatabaseObjects.Columns.StageClosedByName]);
                                targetItem.StageStep = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.StageStep]);
                                targetItem.StageStartDate = UGITUtility.GetObjetToDateTime(UGITUtility.StringToDateTime(item[SPDatabaseObjects.Columns.StageStartDate]));
                                targetItem.StageEndDate = null;
                                if (item[SPDatabaseObjects.Columns.StageEndDate] != null)
                                    targetItem.StageEndDate = UGITUtility.GetObjetToDateTime(UGITUtility.StringToDateTime(item[SPDatabaseObjects.Columns.StageEndDate]));
                                targetItem.OnHoldDuration = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.OnHoldDuration]);
                                targetItem.StageClosedBy = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.StageClosedBy);
                                targetItem.StageClosedBy = userid;
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userList != null)
                                    targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userList != null)
                                    targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} Module data workflow history added");
                }

                ULog.WriteLog($"Updating Module SLA history");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    WorkflowSLASummaryManager mgr = new WorkflowSLASummaryManager(context.AppContext);
                    List<WorkflowSLASummary> dbData = mgr.Load(x => x.TenantID == context.Tenant.TenantID.ToString());
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

                    string listName = SPDatabaseObjects.Lists.TicketWorkflowSLASummary;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    int targetNewItemCount = 0; long ruleid = 0;
                    MappedItemList servicelist = context.GetMappedList(SPDatabaseObjects.Lists.Services);
                    MappedItemList rulelist = context.GetMappedList(SPDatabaseObjects.Lists.SLARule);
                    string fieldName = string.Empty;
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    WorkflowSLASummary targetItem = null;

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            string module = uHelper.getModuleNameByTicketId(fieldName);
                            if (moduleName == module)
                                targetItem = dbData.FirstOrDefault(x => x.TicketId == fieldName && x.ModuleNameLookup == module && x.SLACategoryChoice == Convert.ToString(item[SPDatabaseObjects.Columns.SLACategory]) && x.SLARuleName == Convert.ToString(item[SPDatabaseObjects.Columns.SLARuleName]));



                            if (rulelist != null && item[SPDatabaseObjects.Columns.RuleNameLookup] != null)
                                ruleid = UGITUtility.StringToLong(rulelist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.RuleNameLookup]).LookupId.ToString()));
                            else
                                ruleid = 0;
                            //only add entry if no record found otherwise continue
                            if (targetItem != null)
                                continue;

                            if (targetItem == null)
                            {
                                targetItem = new WorkflowSLASummary();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                ClientOM.FieldLookupValue ServiceTitleLookup = item[SPDatabaseObjects.Columns.ServiceTitleLookup] as ClientOM.FieldLookupValue;
                                if (ServiceTitleLookup != null && ServiceTitleLookup.LookupId > 0)
                                    targetItem.ServiceLookup = ServiceTitleLookup.LookupId;
                                targetItem.TargetTime = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.TargetTime]);
                                targetItem.ActualTime = UGITUtility.StringToInt(item[SPDatabaseObjects.Columns.ActualTime]);
                                if (ruleid > 0)
                                    targetItem.RuleNameLookup = ruleid;//context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.RuleNameLookup);
                                targetItem.SLARuleName = Convert.ToString(item[SPDatabaseObjects.Columns.SLARuleName]);
                                targetItem.SLACategoryChoice = Convert.ToString(item[SPDatabaseObjects.Columns.SLACategory]);

                                targetItem.StartStageStep = (int?)context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.StartStageStep);
                                targetItem.EndStageStep = (int?)context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.EndStageStep);

                                targetItem.StartStageName = Convert.ToString(item[SPDatabaseObjects.Columns.StartStageName]);
                                targetItem.EndStageName = Convert.ToString(item[SPDatabaseObjects.Columns.EndStageName]);

                                DateTime date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.StageStartDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.StageStartDate = date;

                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.StageEndDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.StageEndDate = date;


                                targetItem.Closed = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.TargetTime]);
                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.UGITDueDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.DueDate = date;

                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.TicketCreationDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.Created = date;

                                targetItem.TotalHoldDuration = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.TicketTotalHoldDuration);
                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.TicketCloseDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.CloseDate = date;

                       
                                targetItem.Status = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketStatus);
                                targetItem.OnHold = (int?)context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.TicketOnHold);
                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.OnHoldStartDate = date;
                                date = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);
                                if (date != DateTime.MinValue)
                                    targetItem.OnHoldTillDate = date;

                                targetItem.Use24x7Calendar = UGITUtility.StringToBoolean(item[SPDatabaseObjects.Columns.StageEndDate]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} Module SLA history added");
                }

                ULog.WriteLog($"Updating ModuleUserStatistics");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
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
                    string listName = SPDatabaseObjects.Lists.ModuleUserStatistics;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    string fieldName = string.Empty;
                    UserProfileManager umgr = new UserProfileManager(context.AppContext);
                    var LstUserProfile = umgr.GetUsersProfileWithGroup().Where(x => x.TenantID.Equals(context.AppContext.TenantID, StringComparison.InvariantCultureIgnoreCase)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                    UserProfile user = new UserProfile();


                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            string module = uHelper.getModuleNameByTicketId(fieldName);
                            if (module != moduleName)
                                continue;
                            else
                                targetItem = null;

                            if (targetItem == null)
                            {
                                targetItem = new ModuleUserStatistic();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = module;
                                targetItem.UserRole = Convert.ToString(item[SPDatabaseObjects.Columns.UserRole]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.IsActionUser = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsActionUser]);
                                var uid = LstUserProfile.FirstOrDefault(x => x.Name.Equals(Convert.ToString(item[SPDatabaseObjects.Columns.TicketUser]), StringComparison.InvariantCultureIgnoreCase));
                                if (uid != null)
                                    targetItem.UserName = uid.Id;

                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ModuleUserStatistics added");
                }

                ULog.WriteLog($"Updating DashboardSummary");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    DashboardSummaryManager mgr = new DashboardSummaryManager(context.AppContext);
                    DataTable dtgenericstatus = new DataTable();
                    dtgenericstatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GenericTicketStatus, string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, context.AppContext.TenantID));
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
                    string listName = SPDatabaseObjects.Lists.DashboardSummary;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    string fieldName = string.Empty;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(DatabaseObjects.Columns.TicketStageActionUsers);

                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            fieldName = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            string module = uHelper.getModuleNameByTicketId(fieldName);
                            if (module != moduleName)
                                continue;
                            else
                                targetItem = null;

                            if (targetItem == null)
                            {
                                targetItem = new DashboardSummary();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.ActualHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TicketActualHours]);
                                targetItem.Age = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketAge]);
                                targetItem.ALLSLAsMet = Convert.ToString(item[SPDatabaseObjects.Columns.ALLSLAsMet]);
                                targetItem.AssignmentSLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.AssignmentSLAMet]);
                                targetItem.Category = Convert.ToString(item[SPDatabaseObjects.Columns.Category]);
                                targetItem.Closed = Convert.ToBoolean(item[SPDatabaseObjects.Columns.TicketClosed]);
                                targetItem.CloseSLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.CloseSLAMet]);
                                targetItem.Country = Convert.ToString(item[SPDatabaseObjects.Columns.UGITCountry]);
                                if (item[SPDatabaseObjects.Columns.TicketCreationDate] != null)
                                    targetItem.CreationDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketCreationDate]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.TicketDescription]);
                                targetItem.FunctionalAreaLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.FunctionalAreaLookup);

                                ClientOM.FieldLookupValue _GenericStatusLookup = item[SPDatabaseObjects.Columns.GenericStatusLookup] as ClientOM.FieldLookupValue;
                                if (_GenericStatusLookup != null && _GenericStatusLookup.LookupId > 0)
                                {
                                    DataRow _dr = dtgenericstatus.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, _GenericStatusLookup.LookupValue))[0];
                                    //targetItem.GenericStatusLookup = Convert.ToInt32(context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.GenericStatusLookup));
                                    targetItem.GenericStatusLookup = Convert.ToInt32(_dr[DatabaseObjects.Columns.ID]);
                                }

                                targetItem.FCRCategorization = Convert.ToString(item[SPDatabaseObjects.Columns.FCRCategorization]);
                                targetItem.BreakFix = Convert.ToString(item[SPDatabaseObjects.Columns.BreakFix]);
                                targetItem.BulkRequestCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BulkRequestCount]);
                                targetItem.Initiator = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketInitiator);
                                targetItem.InitiatorResolved = Convert.ToString(item[SPDatabaseObjects.Columns.TicketInitiatorResolved]);
                                targetItem.LocationLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.LocationLookup);
                                targetItem.ModuleStepLookup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.ModuleStepLookup);
                                targetItem.OnHold = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketOnHold]);
                                targetItem.ORP = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketORP);
                                targetItem.OtherSLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.OtherSLAMet]);
                                targetItem.Owner = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketOwner);
                                targetItem.PriorityLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.TicketPriorityLookup);
                                targetItem.PRP = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketPRP);
                                targetItem.PRPGroup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.PRPGroup);
                                targetItem.Region = Convert.ToString(item[SPDatabaseObjects.Columns.UGITRegion]);
                                targetItem.ReopenCount = item[SPDatabaseObjects.Columns.ReopenCount] == null ? 0 : Convert.ToInt32(item[SPDatabaseObjects.Columns.ReopenCount]);
                                targetItem.Requestor = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketRequestor);
                                targetItem.RequestorCompany = Convert.ToString(item[SPDatabaseObjects.Columns.RequestorCompany]);
                                targetItem.RequestorContactSLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.RequestorContactSLAMet]);
                                targetItem.RequestorDepartment = Convert.ToString(item[SPDatabaseObjects.Columns.RequestorDepartment]);
                                targetItem.RequestorDivision = Convert.ToString(item[SPDatabaseObjects.Columns.RequestorDivision]);
                                targetItem.RequestSourceChoice = Convert.ToString(item[SPDatabaseObjects.Columns.TicketRequestSource]);
                                targetItem.RequestTypeLookup = context.GetTargetValueAsOptionalLong(spContext, list, item, SPDatabaseObjects.Columns.TicketRequestTypeLookup);
                                targetItem.ResolutionSLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.ResolutionSLAMet]);
                                targetItem.ResolutionTypeChoice = Convert.ToString(item[SPDatabaseObjects.Columns.TicketResolutionType]);
                                targetItem.ServiceCategoryName = Convert.ToString(item[SPDatabaseObjects.Columns.ServiceCategoryName]);
                                targetItem.ServiceName = Convert.ToString(item[SPDatabaseObjects.Columns.ServiceName]);
                                targetItem.SLAMet = Convert.ToString(item[SPDatabaseObjects.Columns.SLAMet]);
                                DataRow dr = dt.NewRow();
                                dr[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                dt.Rows.Add(dr);
                                targetItem.StageActionUsers = uHelper.GetUsersAsString(context.AppContext, Convert.ToString(DatabaseObjects.Columns.TicketStageActionUsers), dr);
                                targetItem.State = Convert.ToString(item[SPDatabaseObjects.Columns.UGITState]);
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.TicketStatus]);
                                targetItem.SubCategory = Convert.ToString(item[SPDatabaseObjects.Columns.SubCategory]);
                                targetItem.WorkflowType = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowType]);
                                if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                    targetItem.OnHoldTillDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                targetItem.TotalHoldDuration = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketTotalHoldDuration]);
                                if (item[SPDatabaseObjects.Columns.InitiatedDate] != null)
                                    targetItem.InitiatedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.InitiatedDate]);

                                if (item[SPDatabaseObjects.Columns.AssignedDate] != null)
                                    targetItem.AssignedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AssignedDate]);

                                if (item[SPDatabaseObjects.Columns.ResolvedDate] != null)
                                    targetItem.ResolvedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.ResolvedDate]);

                                if (item["TestedDate"] != null)
                                    targetItem.TestedDate = Convert.ToDateTime(item["TestedDate"]);

                                if (item[SPDatabaseObjects.Columns.ClosedDate] != null)
                                    targetItem.ClosedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.ClosedDate]);

                                targetItem.Rejected = Convert.ToBoolean(item[SPDatabaseObjects.Columns.TicketRejected]);
                                targetItem.ClosedBy = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketClosedBy);
                                targetItem.ResolvedBy = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketResolvedBy);
                                targetItem.AssignedBy = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketAssignedBy);
                                targetItem.SLADisabled = Convert.ToBoolean(item[SPDatabaseObjects.Columns.SLADisabled]);
                                targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);

                                if (item[SPDatabaseObjects.Columns.TicketOnHoldStartDate] != null)
                                    targetItem.OnHoldStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldStartDate]);
                                targetItem.OnHold = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketOnHold]);
                                if (item[SPDatabaseObjects.Columns.TicketOnHoldTillDate] != null)
                                    targetItem.OnHoldTillDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketOnHoldTillDate]);

                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} DashboardSummary added");
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);

            }

        }
        //Anurag started below 08/11/21
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
        public override void StageExitCriteria()
        {
            base.StageExitCriteria();

            bool import = context.IsImportEnable("StageExitCriteria", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating StageExitCriteria");

            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                ModuleStageConstraintsManager mgr = new ModuleStageConstraintsManager(context.AppContext);
                List<ModuleStageConstraints> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName);
                PrioirtyViewManager _mgr = new PrioirtyViewManager(context.AppContext);
                List<ModulePrioirty> _dbData = _mgr.Load();
                if (import && deleteBeforeImport)
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
                ClientOM.List list = spContext.Web.Lists.GetByTitle(SPDatabaseObjects.Lists.ModuleStageConstraints);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                int targetNewItemCount = 0;
                MappedItemList moduleStageMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStageConstraints);
                string title = string.Empty;
                ModuleStageConstraints targetItem = null;
                MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                do
                {
                    ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        //title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                        //targetItem = dbData.FirstOrDefault(x => x.Title == title);
                        targetItem = null;
                        if (targetItem == null)
                        {
                            targetItem = new ModuleStageConstraints();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }
                        try
                        {
                            if (targetItem.ID == 0 || importWithUpdate)
                            {

                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                if (item[SPDatabaseObjects.Columns.UGITProposedDate] != null)
                                    targetItem.ProposedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITProposedDate]);
                                targetItem.TaskEstimatedHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.TaskStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UGITTaskStatus]);
                                targetItem.ModuleAutoApprove = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ModuleAutoApprove]);
                                targetItem.ModuleStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]);
                                targetItem.RelatedItems = Convert.ToString(item[SPDatabaseObjects.Columns.RelatedItems]);
                                string PriorityLookup = Convert.ToString(item["Priority"]) == null ? string.Empty : Convert.ToString(item["Priority"]);
                                if (!string.IsNullOrEmpty(PriorityLookup))
                                {
                                    targetItem.Priority = PriorityLookup;
                                }

                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.TicketComment]);
                                if (item[SPDatabaseObjects.Columns.CompletionDate] != null)
                                    targetItem.CompletionDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.CompletionDate]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                targetItem.ProposedStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UGITProposedStatus]);
                                targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                targetItem.TaskActualHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                targetItem.TaskDueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TaskDueDate]);
                                //ClientOM.FieldUserValue AssignedTo = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue;
                                //if (AssignedTo != null && userList != null)
                                //    targetItem.AssignedTo = userList.GetTargetID(Convert.ToString(AssignedTo.LookupId));
                                ClientOM.FieldUserValue[] AssignedTo = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                if (userList != null && AssignedTo != null)
                                    targetItem.AssignedTo = userList.GetTargetIDs(AssignedTo.Select(x => x.LookupId.ToString()).ToList());

                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (users != null && userList != null)
                                    targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(users.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userList != null)
                                    targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    }
                } while (position != null);

                ULog.WriteLog($"{targetNewItemCount} StageExitCriteria  ({moduleName}) added");
            }
        }

        public override void UpdateWikis()
        {
            base.UpdateWikis();
            if (moduleName == "WIK")
                moduleName = "WIKI";
            bool import = context.IsImportEnable("WikiArticles", moduleName);
            if (!import)
                return;


            WikiArticles();
            WikiDiscussion();
            WikiLinks();
            WikiReviews();

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

                //WikiArticlesManager sourceMgr = new WikiArticlesManager(context.SourceAppContext);
                //List<WikiArticles> sourceDbData = sourceMgr.Load();
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.WikiArticles;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList RequestTypeMappedList = context.GetMappedList(SPDatabaseObjects.Lists.RequestType);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            targetItem = dbData.FirstOrDefault(x => x.Title == title);
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new WikiArticles();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleNameLookup = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]) == "WIK" ? "WIKI" : Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                // Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AuthorizedToView] as ClientOM.FieldUserValue[];

                                if (users != null && users.Length > 0 && userList != null)
                                    targetItem.AuthorizedToView = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());

                                //if (item[SPDatabaseObjects.Columns.ResolutionDate] != null)
                                //    targetItem.ResolutionDate = item[SPDatabaseObjects.Columns.ResolutionDate] == null ? Convert.ToDateTime("1753-01-01 00:00:00") : Convert.ToDateTime(item[SPDatabaseObjects.Columns.ResolutionDate]);
                                long targetRequestTypeID = 0;
                                if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                {
                                    targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                }
                                if (targetRequestTypeID > 0)
                                    targetItem.RequestTypeLookup = targetRequestTypeID;

                                targetItem.WikiHistory = Convert.ToString(item[SPDatabaseObjects.Columns.WikiHistory]);
                                targetItem.WikiAverageScore = item[DatabaseObjects.Columns.WikiAverageScore] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiAverageScore]);
                                targetItem.WikiDescription = item[DatabaseObjects.Columns.WikiDescription] == null ? string.Empty : Convert.ToString(item[DatabaseObjects.Columns.WikiDescription]);
                                targetItem.WikiDislikesCount = item[DatabaseObjects.Columns.WikiDislikesCount] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiDislikesCount]);
                                targetItem.WikiLinksCount = item[DatabaseObjects.Columns.WikiLinksCount] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiLinksCount]);
                                targetItem.WikiServiceRequestCount = item[DatabaseObjects.Columns.WikiServiceRequestCount] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiServiceRequestCount]);
                                targetItem.WikiViews = item[SPDatabaseObjects.Columns.WikiViewsCount] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiViewsCount]);

                                targetItem.WikiLikesCount = item[DatabaseObjects.Columns.WikiLikesCount] == null ? 0 : Convert.ToInt32(item[DatabaseObjects.Columns.WikiLikesCount]);
                                targetItem.WikiFavorites = item[DatabaseObjects.Columns.WikiFavorites] == null ? false : Convert.ToBoolean(item[DatabaseObjects.Columns.WikiFavorites]);

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                            {
                                mgr.Insert(targetItem);
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userList != null)
                                {
                                    targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(Createduser.LookupId));
                                }
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userList != null)
                                {
                                    targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                }
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                mgr.Update(targetItem);
                            }
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} WikiArticles added");
                }

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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    string listName = SPDatabaseObjects.Lists.WikiDiscussion;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        if (moduleName == "WIKI")
                            moduleName = "WIK";

                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.TicketId == Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new WikiDiscussion();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }

                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);
                                ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Createduser != null && userList != null)
                                {
                                    targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(Createduser.LookupId));
                                }
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userList != null)
                                {
                                    targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));

                                }
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} WikiDiscussion added");
                }

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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.WikiLinks;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        if (moduleName == "WIKI")
                            moduleName = "WIK";
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                                //need to add flat data
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new WikiLinks();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }

                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    if (item[SPDatabaseObjects.Columns.URL] != null)
                                        targetItem.URL = Convert.ToString(((Microsoft.SharePoint.Client.FieldUrlValue)item[SPDatabaseObjects.Columns.URL]).Url);
                                    targetItem.Comments = Convert.ToString(item[SPDatabaseObjects.Columns.Comments]) == null ? "NO Item" : Convert.ToString(item[SPDatabaseObjects.Columns.Comments]);
                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception e)
                        {
                            ULog.WriteException(e.ToString());
                        }

                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} WikiLinks added");
                }
            }
        }

        private void WikiReviews()
        {
            try
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
                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        string listName = SPDatabaseObjects.Lists.WikiReview;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        string title = string.Empty;
                        do
                        {
                            if (moduleName == "WIKI")
                                moduleName = "WIK";
                            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                            position = collection.ListItemCollectionPosition;

                            foreach (ClientOM.ListItem item in collection)
                            {
                                //targetItem = dbData.FirstOrDefault(x => x.TicketId == Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]));
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new WikiReviews();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                    targetItem.Score = Convert.ToInt32(item["WikiScore"]);
                                    targetItem.Rating = Convert.ToInt32(item["WikiRatingDetails"]);
                                    string val = Convert.ToString(item[SPDatabaseObjects.Columns.WikiUserType]) == "liked" ? "Like" : Convert.ToString(item[SPDatabaseObjects.Columns.WikiUserType]);

                                    targetItem.ReviewType = (ReviewType)Enum.Parse(typeof(ReviewType), val, true);
                                    ClientOM.FieldUserValue Createduser = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (Createduser != null && userList != null)
                                        targetItem.CreatedBy = userList.GetTargetID(Convert.ToString(Createduser.LookupId));
                                    ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (Modifeduser != null && userList != null)
                                        targetItem.ModifiedBy = userList.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                    targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        } while (position != null);

                        ULog.WriteLog($"{targetNewItemCount} WikiReviews added");
                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException(e);

            }

        }

        public override void UpdateTicketTasks()
        {
            DataTable dtRel = new DataTable();
            dtRel.Columns.Add("SPId");
            dtRel.Columns.Add("DId");
            dtRel.Columns.Add("TenantId");
            dtRel.Columns.Add("TicketId");
            dtRel.Columns.Add("ModuleId");
            base.UpdateTicketTasks();

            bool import = context.IsImportEnable("TicketTasks", moduleName);
            if (!import)
                return;

            int targetNewItemCount = 0;

            ULog.WriteLog($"Updating {moduleName} Tasks");
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
            UGITTask targetItem = null;
            string listName = string.Empty;
            using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            {
                if (moduleName == ModuleNames.PMM)
                {
                    listName = SPDatabaseObjects.Lists.PMMTasks;
                }
                else if (moduleName == ModuleNames.NPR)
                {
                    listName = SPDatabaseObjects.Lists.NPRTasks;
                }
                else if (moduleName == ModuleNames.TSK)
                {
                    listName = SPDatabaseObjects.Lists.TSKTasks;
                }
                else if (moduleName == ModuleNames.SVC)
                {
                    listName = SPDatabaseObjects.Lists.TicketRelationship;
                }
                else
                {
                    listName = SPDatabaseObjects.Lists.ModuleTasks;
                }
                ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                MappedItemList locaionlist = context.GetMappedList(listName);
                MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                string title = string.Empty; string ticketid = string.Empty;
                do
                {
                    ClientOM.ListItemCollection collection = null;
                    if (moduleName != ModuleNames.SVC)
                    {
                        collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                    }
                    else
                    {
                        collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                    }

                    position = collection.ListItemCollectionPosition;

                    foreach (ClientOM.ListItem item in collection)
                    {
                        if (moduleName == ModuleNames.NPR)
                            ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketNPRIdLookup]).LookupValue;
                        if (moduleName == ModuleNames.PMM)
                            ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                        if (moduleName == ModuleNames.TSK)
                            ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TSKIDLookup]).LookupValue;
                        if (listName == DatabaseObjects.Tables.ModuleTasks)
                        {
                            if (Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue) != "SVCConfig" && Convert.ToString(item[SPDatabaseObjects.Columns.ModuleNameLookup]) != null)
                            {
                                ticketid = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                            }
                            else
                            {
                                ticketid = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]), "ServiceID", DatabaseObjects.Tables.Services));
                            }

                        }
                        if (moduleName == ModuleNames.SVC)
                            ticketid = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
                        if (moduleName != ModuleNames.SVC)
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == moduleName && x.TicketId == ticketid && x.Status == Convert.ToString(item[SPDatabaseObjects.Columns.Status]) && x.ItemOrder == Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]));
                        else
                            targetItem = null;
                        //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.TicketId == ticketid && x.RelatedTicketID == UGITUtility.ObjectToString((item[SPDatabaseObjects.Columns.ChildTicketId]) == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ChildTicketId]));
                        if (targetItem == null)
                        {
                            targetItem = new UGITTask();
                            dbData.Add(targetItem);
                            targetNewItemCount++;
                        }
                        try
                        {
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Priority = item["Priority"] == null ? string.Empty : Convert.ToString(item["Priority"]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                targetItem.TicketId = ticketid;
                                if (listName == DatabaseObjects.Tables.ModuleTasks && moduleName != ModuleNames.ACR)
                                    targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.ModuleStep]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                targetItem.ParentTaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                ClientOM.FieldLookupValue[] _predcessors = item[SPDatabaseObjects.Columns.Predecessors] as ClientOM.FieldLookupValue[];
                                List<string> pressors = new List<string>();
                                if (_predcessors != null && _predcessors.Count() > 0)
                                {
                                    if (_predcessors != null && _predcessors.Count() > 0)
                                    {
                                        pressors.Clear();
                                        foreach (var pred in _predcessors)
                                        {
                                            //var val = dbData.FirstOrDefault(x => x.Title == pred.LookupValue);
                                            pressors.Add(Convert.ToString(pred.LookupValue));
                                        }
                                        targetItem.Predecessors = string.Join(";# ", pressors.ToArray());
                                    }
                                }

                                if (moduleName == ModuleNames.TSK)
                                {
                                    targetItem.EstimatedRemainingHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.EstimatedRemainingHours]);
                                    targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                    targetItem.ActualHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                    if (item[SPDatabaseObjects.Columns.UGITProposedDate] != null)
                                        targetItem.ProposedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITProposedDate]);
                                    targetItem.ProposedStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), item[SPDatabaseObjects.Columns.UGITProposedStatus].ToString());

                                }
                                ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                if (userList != null && users != null)
                                {
                                    targetItem.AssignedTo = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                }

                                if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                if (item[SPDatabaseObjects.Columns.DueDate] != null)
                                    targetItem.DueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DueDate]);
                                if (item[SPDatabaseObjects.Columns.CompletionDate] != null)
                                    targetItem.CompletionDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.CompletionDate]);
                                if (moduleName != ModuleNames.SVC)
                                {
                                    targetItem.IsCritical = item[SPDatabaseObjects.Columns.IsCritical] == null ? false : Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsCritical]);
                                    targetItem.Duration = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITDuration]);
                                    targetItem.ChildCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITChildCount]);
                                    targetItem.AssignToPct = Convert.ToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]);
                                    //List<string> lstUsernames = UGITUtility.ConvertStringToList(targetItem.AssignedTo, Constants.Separator);
                                    //List<string> lstAssigntopct = new List<string>();
                                    //List<string> lstsppct = UGITUtility.ConvertStringToList(Convert.ToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]), Constants.Separator);
                                    //for (int i = 0; i < lstUsernames.Count; i++)
                                    //{
                                    //    List<string> namesandpct = UGITUtility.ConvertStringToList(lstsppct[i], Constants.Separator1);
                                    //    string username = UGITUtility.ConvertStringToList(Convert.ToString(namesandpct[0]), "\\").LastOrDefault();
                                    //    string pct = Convert.ToString(namesandpct[1]);
                                    //    lstAssigntopct.Add(lstUsernames[i] + ";~" + pct + ";~" + username);
                                    //}
                                    //targetItem.AssignToPct = UGITUtility.ConvertListToString(lstAssigntopct, Constants.Separator);  
                                    // Convert.ToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]);
                                }
                                if (moduleName == ModuleNames.SVC)
                                {
                                    targetItem.SubTaskType = Convert.ToString(item[SPDatabaseObjects.Columns.UGITTaskType]);
                                    targetItem.RelatedTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]);
                                    targetItem.Behaviour = item[SPDatabaseObjects.Columns.ChildTicketId] == null ? "Task" : "Ticket";
                                    targetItem.RelatedModule = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]));
                                    targetItem.ActualHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                }
                                targetItem.EstimatedHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.Level = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITLevel]);
                                if (moduleName == ModuleNames.PMM)
                                {
                                    targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                    targetItem.Behaviour = Convert.ToString(item[SPDatabaseObjects.Columns.TaskBehaviour]);
                                    targetItem.ActualHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                    targetItem.EstimatedRemainingHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.EstimatedRemainingHours]);
                                }

                                if (listName == DatabaseObjects.Tables.ModuleTasks)
                                {
                                    targetItem.ActualHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                    targetItem.EstimatedRemainingHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.EstimatedRemainingHours]);
                                    if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                        targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                    if (item[SPDatabaseObjects.Columns.DueDate] != null)
                                        targetItem.DueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DueDate]);
                                    if (item[SPDatabaseObjects.Columns.CompletionDate] != null)
                                        targetItem.CompletionDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.CompletionDate]);

                                    targetItem.IsCritical = item[SPDatabaseObjects.Columns.IsCritical] == null ? false : Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsCritical]);
                                    targetItem.Duration = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITDuration]);
                                    targetItem.ChildCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITChildCount]);
                                    targetItem.AssignToPct = Convert.ToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]);
                                }

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                            // adding below process to make parent child relationship without predecessors
                            DataRow dr = dtRel.NewRow();
                            dr["SPId"] = item[SPDatabaseObjects.Columns.Id];
                            dr["DId"] = targetItem.ID;
                            dr["TenantId"] = context.AppContext.TenantID;
                            dr["TicketId"] = ticketid;
                            dr["ModuleId"] = moduleName;
                            dtRel.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    }
                } while (position != null);
                GetTableDataManager.bulkupload(dtRel, GetTableDataManager.CreateTempTable());
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.AppContext.TenantID);
                values.Add("@ModuleId", moduleName);
                DAL.uGITDAL.ExecuteDataSetWithParameters("usp_updateparenttask", values);
                ULog.WriteLog($"{targetNewItemCount} {moduleName} Tasks");
            }

            if (moduleName == ModuleNames.PMM)
            {
                ULog.WriteLog($"Updating {moduleName} Issues");
                targetItem = null;
                dtRel.Clear();
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    listName = SPDatabaseObjects.Lists.PMMIssues;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty; string ticketid = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = null;
                        collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == moduleName && x.TicketId == ticketid && x.Status == Convert.ToString(item[SPDatabaseObjects.Columns.Status]) && x.ItemOrder == Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new UGITTask();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Priority = item["Priority"] == null ? string.Empty : Convert.ToString(item["Priority"]);
                                    targetItem.ModuleNameLookup = moduleName;
                                    targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                    targetItem.TicketId = ticketid;

                                    targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                    targetItem.ParentTaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                    string[] arrPredecessors = Convert.ToString(item[DatabaseObjects.Columns.Predecessors]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    List<string> pressors = new List<string>();
                                    if (arrPredecessors != null && arrPredecessors.Count() > 0)
                                    {
                                        pressors.Clear();
                                        foreach (var pred in arrPredecessors)
                                        {
                                            if (pred == "Microsoft.SharePoint.Client.FieldLookupValue[]")
                                                continue;
                                            pressors.Add(Convert.ToString(pred));
                                        }
                                        targetItem.Predecessors = string.Join(", ", pressors.ToArray());
                                    }

                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                    if (userList != null && users != null)
                                    {
                                        targetItem.AssignedTo = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                    }

                                    if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                        targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                    if (item[SPDatabaseObjects.Columns.DueDate] != null)
                                        targetItem.DueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DueDate]);

                                    targetItem.Duration = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITDuration]);
                                    targetItem.ChildCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITChildCount]);
                                    targetItem.IssueImpact = Convert.ToString(item[SPDatabaseObjects.Columns.IssueImpact]);
                                    targetItem.SubTaskType = "Issue";
                                    targetItem.EstimatedHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                    targetItem.Level = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITLevel]);

                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                                // adding below process to make parent child relationship without predecessors
                                //DataRow dr = dtRel.NewRow();
                                //dr["SPId"] = item[SPDatabaseObjects.Columns.Id];
                                //dr["DId"] = targetItem.ID;
                                //dr["TenantId"] = context.AppContext.TenantID;
                                //dr["TicketId"] = ticketid;
                                //dr["ModuleId"] = moduleName;
                                //dtRel.Rows.Add(dr);
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                        }
                    } while (position != null);
                    //GetTableDataManager.bulkupload(dtRel, GetTableDataManager.CreateTempTable());
                    //Dictionary<string, object> values = new Dictionary<string, object>();
                    //values.Add("@TenantID", context.AppContext.TenantID);
                    //values.Add("@ModuleId", moduleName);
                    //DAL.uGITDAL.ExecuteDataSetWithParameters("usp_updateparenttask", values);
                    ULog.WriteLog($"{targetNewItemCount} {moduleName} Issues");
                }

                ULog.WriteLog($"Updating {moduleName} Risks");
                targetItem = null;
                dtRel.Clear();
                try
                {
                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        listName = SPDatabaseObjects.Lists.PMMRisks;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        string title = string.Empty; string ticketid = string.Empty;
                        do
                        {
                            ClientOM.ListItemCollection collection = null;
                            collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                            position = collection.ListItemCollectionPosition;

                            foreach (ClientOM.ListItem item in collection)
                            {
                                ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == moduleName && x.TicketId == ticketid && x.Status == Convert.ToString(item[SPDatabaseObjects.Columns.Status]) && x.ItemOrder == Convert.ToInt32(item[SPDatabaseObjects.Columns.Title]));
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new UGITTask();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                try
                                {
                                    if (targetItem.ID == 0 || importWithUpdate)
                                    {
                                        targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                        //targetItem.Priority = item["Priority"] == null ? string.Empty : Convert.ToString(item["Priority"]);
                                        targetItem.ModuleNameLookup = moduleName;
                                        targetItem.MitigationPlan = Convert.ToString(item[SPDatabaseObjects.Columns.MitigationPlan]);
                                        targetItem.RiskProbability = Convert.ToInt32(item[SPDatabaseObjects.Columns.RiskProbability]);
                                        targetItem.ContingencyPlan = Convert.ToString(item[SPDatabaseObjects.Columns.ContingencyPlan]);
                                        targetItem.IssueImpact = Convert.ToString(item[SPDatabaseObjects.Columns.IssueImpact]);
                                        targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                        targetItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                        targetItem.TicketId = ticketid;

                                        ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                        if (userList != null && users != null)
                                        {
                                            targetItem.AssignedTo = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                        }

                                        targetItem.SubTaskType = "Risk";


                                    }

                                    if (targetItem.ID > 0)
                                        mgr.Update(targetItem);
                                    else
                                        mgr.Insert(targetItem);

                                    // adding below process to make parent child relationship without predecessors
                                    //DataRow dr = dtRel.NewRow();
                                    //dr["SPId"] = item[SPDatabaseObjects.Columns.Id];
                                    //dr["DId"] = targetItem.ID;
                                    //dr["TenantId"] = context.AppContext.TenantID;
                                    //dr["TicketId"] = ticketid;
                                    //dr["ModuleId"] = moduleName;
                                    //dtRel.Rows.Add(dr);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex);
                                }
                            }
                        } while (position != null);
                        //GetTableDataManager.bulkupload(dtRel, GetTableDataManager.CreateTempTable());
                        //Dictionary<string, object> values = new Dictionary<string, object>();
                        //values.Add("@TenantID", context.AppContext.TenantID);
                        //values.Add("@ModuleId", moduleName);
                        //DAL.uGITDAL.ExecuteDataSetWithParameters("usp_updateparenttask", values);
                        ULog.WriteLog($"{targetNewItemCount} {moduleName} Tasks");
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
                //Updating ModuleTaskHistory
                ULog.WriteLog($"Updating {moduleName} Task History");
                ModuleTasksHistory moduleTaskHistory = new ModuleTasksHistory();
                moduleTaskHistory = null;
                dtRel.Clear();
                ModuleTaskHistoryManager _mgr = new ModuleTaskHistoryManager(context.AppContext);
                List<ModuleTasksHistory> _dbData = _mgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    if (moduleName == ModuleNames.PMM)
                    {
                        listName = SPDatabaseObjects.Lists.PMMTasksHistory;
                    }
                    targetNewItemCount = 0;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty; string ticketid = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = null;
                        collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            if (moduleName == ModuleNames.PMM)
                                ticketid = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;

                            moduleTaskHistory = _dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.ModuleNameLookup == moduleName && x.TicketId == ticketid && x.Status == Convert.ToString(item[SPDatabaseObjects.Columns.Status]) && x.ItemOrder == Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]));
                            moduleTaskHistory = null;
                            if (moduleTaskHistory == null)
                            {
                                moduleTaskHistory = new ModuleTasksHistory();
                                _dbData.Add(moduleTaskHistory);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (moduleTaskHistory.ID == 0 || importWithUpdate)
                                {
                                    moduleTaskHistory.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    moduleTaskHistory.Priority = item["Priority"] == null ? string.Empty : Convert.ToString(item["Priority"]);
                                    moduleTaskHistory.ModuleNameLookup = moduleName;
                                    moduleTaskHistory.Status = Convert.ToString(item[SPDatabaseObjects.Columns.Status]);
                                    moduleTaskHistory.TicketId = ticketid;
                                    moduleTaskHistory.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                    moduleTaskHistory.ParentTaskID = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                    ClientOM.FieldLookupValue[] _predcessors = item[SPDatabaseObjects.Columns.Predecessors] as ClientOM.FieldLookupValue[];
                                    List<string> pressors = new List<string>();
                                    if (_predcessors != null && _predcessors.Count() > 0)
                                    {
                                        if (_predcessors != null && _predcessors.Count() > 0)
                                        {
                                            pressors.Clear();
                                            foreach (var pred in _predcessors)
                                            {
                                                pressors.Add(Convert.ToString(pred.LookupValue));
                                            }
                                            moduleTaskHistory.Predecessors = string.Join(";# ", pressors.ToArray());
                                        }
                                    }
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue[];
                                    if (userList != null && users != null)
                                    {
                                        moduleTaskHistory.AssignedTo = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                    }
                                    if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                        moduleTaskHistory.StartDate = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                    else
                                        moduleTaskHistory.StartDate = UGITUtility.GetObjetToDateTime("1753-01-01 00:00:00");
                                    if (item[SPDatabaseObjects.Columns.DueDate] != null)
                                        moduleTaskHistory.DueDate = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.DueDate]);
                                    else
                                        moduleTaskHistory.DueDate = UGITUtility.GetObjetToDateTime("1753-01-01 00:00:00");
                                    if (item[SPDatabaseObjects.Columns.CompletionDate] != null)
                                        moduleTaskHistory.CompletionDate = UGITUtility.GetObjetToDateTime(item[SPDatabaseObjects.Columns.CompletionDate]);

                                    moduleTaskHistory.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                    moduleTaskHistory.Behaviour = Convert.ToString(item[SPDatabaseObjects.Columns.TaskBehaviour]);
                                    moduleTaskHistory.EstimatedHours = Convert.ToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                    moduleTaskHistory.IsMileStone = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsMilestone]);
                                    if (item[SPDatabaseObjects.Columns.IsCritical] != null)
                                        moduleTaskHistory.IsCritical = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsCritical]);
                                    moduleTaskHistory.CompletedBy = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.CompletedBy);
                                    moduleTaskHistory.ActualHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.ActualHour]);
                                    moduleTaskHistory.AssignToPct = Convert.ToString(item[SPDatabaseObjects.Columns.UGITAssignToPct]);
                                    moduleTaskHistory.Body = Convert.ToString(item[SPDatabaseObjects.Columns.Body]);

                                    if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    {
                                        moduleTaskHistory.BaselineDate = UGITUtility.GetObjetToDateTime(item[DatabaseObjects.Columns.BaselineDate]);
                                    }
                                    moduleTaskHistory.BaselineId = item[SPDatabaseObjects.Columns.BaselineNum] == null ? 0 : Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);
                                    moduleTaskHistory.ProposedStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), item[SPDatabaseObjects.Columns.UGITProposedStatus].ToString());
                                    moduleTaskHistory.SprintLookup = Convert.ToInt64(context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.SprintLookup));
                                    moduleTaskHistory.UserSkillMultiLookup = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.UserSkillMultiLookup);
                                    moduleTaskHistory.EstimatedHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                    moduleTaskHistory.EstimatedRemainingHours = item[SPDatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : Convert.ToDouble(item[SPDatabaseObjects.Columns.EstimatedRemainingHours]);
                                    moduleTaskHistory.ReminderEnabled = Convert.ToBoolean(item[SPDatabaseObjects.Columns.TaskReminderEnabled]);
                                    moduleTaskHistory.ReminderDays = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskReminderDays]);
                                    moduleTaskHistory.Created = UGITUtility.GetObjetToDateTime(item[DatabaseObjects.Columns.Created]);
                                    moduleTaskHistory.Modified = UGITUtility.GetObjetToDateTime(item[DatabaseObjects.Columns.Modified]);
                                }

                                if (moduleTaskHistory.ID > 0)
                                    _mgr.Update(moduleTaskHistory);
                                else
                                    _mgr.Insert(moduleTaskHistory);

                                // adding below process to make parent child relationship without predecessors
                                DataRow dr = dtRel.NewRow();
                                dr["SPId"] = item[SPDatabaseObjects.Columns.Id];
                                dr["DId"] = targetItem.ID;
                                dr["TenantId"] = context.AppContext.TenantID;
                                dr["TicketId"] = ticketid;
                                dr["ModuleId"] = moduleName;
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                        }
                    } while (position != null);
                    GetTableDataManager.bulkupload(dtRel, GetTableDataManager.CreateTempTable());
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("@TenantID", context.AppContext.TenantID);
                    values.Add("@ModuleId", moduleName);
                    DAL.uGITDAL.ExecuteDataSetWithParameters("usp_updateparenttaskHistory", values);
                    ULog.WriteLog($"{targetNewItemCount} {moduleName} Task History");
                }

                //Updating PMMHistory
                ULog.WriteLog($"Updating {moduleName} PMMHistory");
                TicketDal ticketDal = new TicketDal(context.AppContext);
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                DataTable ticketSchema = ticketMgr.GetDatabaseTableSchema(DatabaseObjects.Tables.PMMHistory);

                try
                {
                    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                    {
                        string ticketID = string.Empty; long targetLocationID = 0; long targetPriorityID = 0; long targetRequestTypeID = 0;
                        long targetImpactId = 0; long FunctonalAreaId = 0; long ModuleStepId = 0;
                        string Requesttype = string.Empty; long TicketSeverityId = 0; string stageids = string.Empty; long projectClass = 0;
                        long projectinitative = 0;
                        listName = SPDatabaseObjects.Lists.PMMProjectsHistory;
                        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                        MappedItemList locaionlist = context.GetMappedList(listName);
                        MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                        string title = string.Empty; string ticketid = string.Empty;
                        MappedItemList locationMappedList = context.GetMappedList(SPDatabaseObjects.Lists.Location);
                        MappedItemList priorityMappedList = context.GetMappedList(SPDatabaseObjects.Lists.TicketPriority);
                        MappedItemList RequestTypeMappedList = context.GetMappedList(SPDatabaseObjects.Lists.RequestType);
                        MappedItemList AssetMappedList = context.GetMappedList(SPDatabaseObjects.Lists.AssetVendors);
                        MappedItemList ImpactMappedList = context.GetMappedList(SPDatabaseObjects.Lists.TicketImpact);
                        MappedItemList FunctionAreaMappedList = context.GetMappedList(SPDatabaseObjects.Lists.FunctionalAreas);
                        MappedItemList ModuleStagesMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                        MappedItemList DepartmentList = context.GetMappedList(SPDatabaseObjects.Lists.Department);
                        MappedItemList TicketSeverityList = context.GetMappedList(SPDatabaseObjects.Lists.TicketSeverity);
                        MappedItemList targetStagetMappedList = context.GetMappedList(SPDatabaseObjects.Lists.ModuleStages);
                        MappedItemList targetProjectInitiativeList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectInitiative);
                        MappedItemList targetProjectclassList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectClass);
                        MappedItemList targetProjectlifecycleList = context.GetMappedList(SPDatabaseObjects.Lists.ProjectLifeCycles);
                        MappedItemList companylist = context.GetMappedList(SPDatabaseObjects.Lists.Company);
                        MappedItemList servicelist = context.GetMappedList(SPDatabaseObjects.Lists.Services);

                        do
                        {
                            ClientOM.ListItemCollection collection = null;
                            collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                            position = collection.ListItemCollectionPosition;
                            foreach (ClientOM.ListItem item in collection)
                            {
                                DataRow _targetItem = null;
                                if (_targetItem == null)
                                {

                                    _targetItem = ticketSchema.NewRow();

                                    if (priorityMappedList != null && item[SPDatabaseObjects.Columns.TicketPriorityLookup] != null)
                                        targetPriorityID = UGITUtility.StringToLong(priorityMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPriorityLookup]).LookupId.ToString()));
                                    if (ModuleStagesMappedList != null && item[SPDatabaseObjects.Columns.ModuleStepLookup] != null)
                                        ModuleStepId = UGITUtility.StringToLong(ModuleStagesMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleStepLookup]).LookupId.ToString()));

                                    if (ImpactMappedList != null && item[SPDatabaseObjects.Columns.TicketImpactLookup] != null)
                                        targetImpactId = UGITUtility.StringToLong(ImpactMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketImpactLookup]).LookupId.ToString()));
                                    if (RequestTypeMappedList != null && item[SPDatabaseObjects.Columns.TicketRequestTypeLookup] != null)
                                    {
                                        Requesttype = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupValue);
                                        targetRequestTypeID = UGITUtility.StringToLong(RequestTypeMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeLookup]).LookupId.ToString()));
                                    }
                                    if (FunctionAreaMappedList != null && item[SPDatabaseObjects.Columns.FunctionalAreaLookup] != null)
                                        FunctonalAreaId = UGITUtility.StringToLong(FunctionAreaMappedList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.FunctionalAreaLookup]).LookupId.ToString()));

                                    if (targetProjectInitiativeList != null && item[SPDatabaseObjects.Columns.ProjectInitiativeLookup] != null)
                                        projectinitative = UGITUtility.StringToLong(targetProjectInitiativeList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectInitiativeLookup]).LookupId.ToString()));

                                    if (targetProjectclassList != null && item[SPDatabaseObjects.Columns.ProjectClassLookup] != null)
                                        projectClass = UGITUtility.StringToLong(targetProjectclassList.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ProjectClassLookup]).LookupId.ToString()));

                                    try
                                    {
                                        _targetItem = ticketSchema.NewRow();
                                        _targetItem[DatabaseObjects.Columns.LocationMultLookup] = targetLocationID;
                                        _targetItem[DatabaseObjects.Columns.TicketPriorityLookup] = targetPriorityID;
                                        if (targetImpactId > 0)
                                            _targetItem[DatabaseObjects.Columns.TicketImpactLookup] = targetImpactId;
                                        _targetItem[DatabaseObjects.Columns.FunctionalAreaLookup] = FunctonalAreaId;
                                        if (projectinitative > 0)
                                            _targetItem[DatabaseObjects.Columns.ProjectInitiativeLookup] = projectinitative;
                                        if (projectClass > 0)
                                            _targetItem[DatabaseObjects.Columns.ProjectClassLookup] = projectClass;

                                        ClientOM.FieldLookupValue TicketAPPId = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                        if (TicketAPPId != null)
                                        {
                                            if (TicketAPPId.LookupId > 0 && TicketAPPId.LookupValue != null)
                                            {
                                                Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                _targetItem[DatabaseObjects.Columns.APPTitleLookup] = TicketAPPId.LookupId;
                                                SPTicketrefernces.Add("SPAPPId", Convert.ToString(TicketAPPId.LookupId));
                                                SPTicketrefernces.Add("SPAPPTicketId", UGITUtility.ObjectToString(TicketAPPId.LookupValue));
                                                GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                            }
                                        }

                                        ClientOM.FieldLookupValue TicketNPRId = item[SPDatabaseObjects.Columns.TicketNPRIdLookup] as ClientOM.FieldLookupValue;
                                        if (TicketNPRId != null)
                                        {
                                            if (TicketNPRId.LookupId > 0 && TicketNPRId.LookupValue != null)
                                            {
                                                Dictionary<string, object> SPTicketrefernces = new Dictionary<string, object>();
                                                _targetItem[DatabaseObjects.Columns.TicketNPRIdLookup] = TicketNPRId.LookupId;
                                                SPTicketrefernces.Add("SPNPRId", Convert.ToString(TicketNPRId.LookupId));
                                                SPTicketrefernces.Add("SPNPRTicketId", UGITUtility.ObjectToString(TicketNPRId.LookupValue));
                                                GetTableDataManager.AddItem<int>("MigrationRecordUpdate", SPTicketrefernces);
                                            }
                                        }
                                        _targetItem[DatabaseObjects.Columns.TicketActualCompletionDate] = item[SPDatabaseObjects.Columns.TicketActualCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualCompletionDate];
                                        _targetItem[DatabaseObjects.Columns.TicketActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                        _targetItem[DatabaseObjects.Columns.EstimatedRemainingHours] = item[SPDatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : item[SPDatabaseObjects.Columns.EstimatedRemainingHours];
                                        _targetItem[DatabaseObjects.Columns.ActualStartDate] = item[SPDatabaseObjects.Columns.TicketActualStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketActualStartDate];
                                        _targetItem[DatabaseObjects.Columns.TicketDesiredCompletionDate] = item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketDesiredCompletionDate];
                                        _targetItem[DatabaseObjects.Columns.EstimatedHours] = item[SPDatabaseObjects.Columns.EstimatedHours] == null ? 0 : item[SPDatabaseObjects.Columns.EstimatedHours];
                                        _targetItem[DatabaseObjects.Columns.TicketTargetCompletionDate] = item[SPDatabaseObjects.Columns.TicketTargetCompletionDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketTargetCompletionDate];
                                        _targetItem[DatabaseObjects.Columns.UGITDaysToComplete] = item[SPDatabaseObjects.Columns.UGITDaysToComplete] == null ? 0 : item[SPDatabaseObjects.Columns.UGITDaysToComplete];
                                        _targetItem[DatabaseObjects.Columns.IsPrivate] = item[SPDatabaseObjects.Columns.IsPrivate] == null ? false : item[SPDatabaseObjects.Columns.IsPrivate];
                                        _targetItem[DatabaseObjects.Columns.TicketProjectAssumptions] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketProjectAssumptions]);
                                        _targetItem[DatabaseObjects.Columns.TicketProjectBenefits] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketProjectBenefits]);
                                        _targetItem[DatabaseObjects.Columns.ProjectRiskNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRiskNotes]);
                                        _targetItem[DatabaseObjects.Columns.ActualHour] = Convert.ToInt32(item[SPDatabaseObjects.Columns.ActualHour]);
                                        _targetItem[DatabaseObjects.Columns.TicketApprovedRFEAmount] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEAmount]);
                                        _targetItem[DatabaseObjects.Columns.TicketApprovedRFE] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFE]);
                                        _targetItem[DatabaseObjects.Columns.TicketApprovedRFEType] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketApprovedRFEType]);
                                        _targetItem[DatabaseObjects.Columns.TicketArchitectureScore] = Convert.ToInt32(item[SPDatabaseObjects.Columns.TicketArchitectureScore]);
                                        _targetItem[DatabaseObjects.Columns.TicketArchitectureScoreNotes] = Convert.ToString(item[SPDatabaseObjects.Columns.TicketArchitectureScoreNotes]);
                                        _targetItem[DatabaseObjects.Columns.AutoAdjustAllocations] = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoAdjustAllocations]);
                                        _targetItem[DatabaseObjects.Columns.ProjectSummaryNote] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectSummaryNote]);
                                        _targetItem[DatabaseObjects.Columns.ProblemBeingSolved] = Convert.ToString(item[SPDatabaseObjects.Columns.ProblemBeingSolved]);
                                        _targetItem[DatabaseObjects.Columns.ScrumLifeCycle] = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ScrumLifeCycle]);
                                        _targetItem[DatabaseObjects.Columns.NextMilestone] = Convert.ToString(item[SPDatabaseObjects.Columns.NextMilestone]);
                                        _targetItem[DatabaseObjects.Columns.TicketProjectScope] = item[SPDatabaseObjects.Columns.TicketProjectScope];
                                        _targetItem[DatabaseObjects.Columns.TicketClassificationType] = item[SPDatabaseObjects.Columns.TicketClassificationType];
                                        _targetItem[DatabaseObjects.Columns.TicketClassification] = item[SPDatabaseObjects.Columns.TicketClassification];
                                        _targetItem[DatabaseObjects.Columns.TicketClassificationImpact] = item[SPDatabaseObjects.Columns.TicketClassificationImpact];
                                        _targetItem[DatabaseObjects.Columns.ProjectCost] = item[SPDatabaseObjects.Columns.ProjectCost];
                                        _targetItem[DatabaseObjects.Columns.TicketTotalCost] = item[SPDatabaseObjects.Columns.TicketTotalCost];
                                        _targetItem[DatabaseObjects.Columns.TicketTotalCostsNotes] = item[SPDatabaseObjects.Columns.TicketTotalCostsNotes];
                                        _targetItem[DatabaseObjects.Columns.TicketRiskScore] = item[SPDatabaseObjects.Columns.TicketRiskScore] == null ? 0 : item[SPDatabaseObjects.Columns.TicketRiskScore];
                                        _targetItem[DatabaseObjects.Columns.DataEditors] = item[SPDatabaseObjects.Columns.DataEditor];
                                        _targetItem[DatabaseObjects.Columns.BaselineId] = item[SPDatabaseObjects.Columns.BaselineNum] == null ? 0 : item[SPDatabaseObjects.Columns.BaselineNum];
                                        _targetItem[DatabaseObjects.Columns.TicketCreationDate] = item[SPDatabaseObjects.Columns.TicketCreationDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketCreationDate];
                                        _targetItem[DatabaseObjects.Columns.CurrentStageStartDate] = item[SPDatabaseObjects.Columns.CurrentStageStartDate] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.CurrentStageStartDate];
                                        _targetItem[DatabaseObjects.Columns.TicketPctComplete] = item[SPDatabaseObjects.Columns.TicketPctComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TicketPctComplete]) * 100;
                                        if (ModuleStepId > 0)
                                            _targetItem[DatabaseObjects.Columns.ModuleStepLookup] = ModuleStepId;
                                        _targetItem[DatabaseObjects.Columns.StageStep] = item[SPDatabaseObjects.Columns.StageStep] == null ? 0 : item[SPDatabaseObjects.Columns.StageStep];
                                        if (targetRequestTypeID > 0)
                                            _targetItem[DatabaseObjects.Columns.TicketRequestTypeLookup] = targetRequestTypeID;
                                        if (item[SPDatabaseObjects.Columns.TicketRequestTypeCategory] != null)
                                            _targetItem[DatabaseObjects.Columns.TicketRequestTypeCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeCategory]).LookupValue);
                                        if (item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory] != null)
                                            _targetItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory] = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketRequestTypeSubCategory]).LookupValue); ;
                                        ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                        if (_users != null && _users.Length > 0 && userList != null)
                                            _targetItem[DatabaseObjects.Columns.Owner] = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                        ClientOM.FieldUserValue[] users_ = item[SPDatabaseObjects.Columns.TicketRequestor] as ClientOM.FieldUserValue[];
                                        if (users_ != null && users_.Length > 0 && userList != null)
                                            _targetItem[DatabaseObjects.Columns.TicketRequestor] = userList.GetTargetIDs(users_.Select(x => x.LookupId.ToString()).ToList());
                                        ClientOM.FieldUserValue[] iusers = item[SPDatabaseObjects.Columns.TicketInitiator] as ClientOM.FieldUserValue[];
                                        if (iusers != null && iusers.Length > 0 && userList != null)
                                            _targetItem[DatabaseObjects.Columns.TicketInitiator] = userList.GetTargetIDs(iusers.Select(x => x.LookupId.ToString()).ToList());
                                        _targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = item[SPDatabaseObjects.Columns.TicketStageActionUsers];
                                        _targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = item[SPDatabaseObjects.Columns.TicketStageActionUserTypes];
                                        _targetItem[DatabaseObjects.Columns.StatusChanged] = item[SPDatabaseObjects.Columns.TicketStatusChanged] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.TicketStatusChanged];
                                        if (TicketSeverityId > 0)
                                            _targetItem[DatabaseObjects.Columns.TicketSeverityLookup] = TicketSeverityId;

                                        ClientOM.FieldLookupValue[] multlocation = item[SPDatabaseObjects.Columns.LocationMultLookup] as ClientOM.FieldLookupValue[];
                                        if (multlocation != null && multlocation.Length > 0 && locationMappedList != null)
                                            _targetItem[DatabaseObjects.Columns.LocationMultLookup] = locationMappedList.GetTargetIDs(multlocation.Select(x => x.LookupId.ToString()).ToList());
                                        ClientOM.FieldLookupValue[] _TicketBeneficiaries = item[SPDatabaseObjects.Columns.TicketBeneficiaries] as ClientOM.FieldLookupValue[];
                                        if (_TicketBeneficiaries != null && _TicketBeneficiaries.Length > 0 && DepartmentList != null)
                                            _targetItem[DatabaseObjects.Columns.TicketBeneficiaries] = DepartmentList.GetTargetIDs(_TicketBeneficiaries.Select(x => x.LookupId.ToString()).ToList());
                                        ClientOM.FieldLookupValue projectlifecycles = item[SPDatabaseObjects.Columns.ProjectLifeCycleLookup] as ClientOM.FieldLookupValue;
                                        if (projectlifecycles != null && targetProjectlifecycleList != null)
                                            _targetItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = targetProjectlifecycleList.GetTargetID(Convert.ToString(projectlifecycles.LookupId));

                                        _targetItem[DatabaseObjects.Columns.ProjectRank] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank]);
                                        _targetItem[DatabaseObjects.Columns.ProjectRank2] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank2]);
                                        _targetItem[DatabaseObjects.Columns.ProjectRank3] = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectRank3]);

                                        ClientOM.FieldUserValue[] StakeHoldersUser = item[SPDatabaseObjects.Columns.TicketStakeHolders] as ClientOM.FieldUserValue[];
                                        if (StakeHoldersUser != null && userList != null)
                                            _targetItem[DatabaseObjects.Columns.StakeHoldersUser] = userList.GetTargetIDs(StakeHoldersUser.Select(x => x.LookupId.ToString()).ToList());

                                        ClientOM.FieldUserValue[] TicketProjectManagerUser = item[SPDatabaseObjects.Columns.TicketProjectManager] as ClientOM.FieldUserValue[];
                                        if (TicketProjectManagerUser != null && userList != null)
                                            _targetItem[DatabaseObjects.Columns.TicketProjectManager] = userList.GetTargetIDs(TicketProjectManagerUser.Select(x => x.LookupId.ToString()).ToList());
                                        _targetItem[DatabaseObjects.Columns.SponsorsUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.TicketSponsors);
                                        ClientOM.FieldUserValue businessmgr = item[SPDatabaseObjects.Columns.TicketBusinessManager] as ClientOM.FieldUserValue;
                                        if (businessmgr != null && userList != null)
                                            _targetItem[DatabaseObjects.Columns.TicketBusinessManager] = userList.GetTargetID(Convert.ToString(businessmgr.LookupId));
                                        stageids = Convert.ToString(item[SPDatabaseObjects.Columns.WorkflowSkipStages]);
                                        if (stageids != null && targetStagetMappedList != null)
                                        {
                                            _targetItem[DatabaseObjects.Columns.WorkflowSkipStages] = stageids;
                                        }
                                        _targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = context.CovertActionUserTypes(Convert.ToString(_targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), moduleColumnMapped);
                                        _targetItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context.AppContext, Convert.ToString(_targetItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), _targetItem);
                                        if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Author) != null)
                                            _targetItem[DatabaseObjects.Columns.CreatedByUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Author);
                                        else
                                            _targetItem[DatabaseObjects.Columns.CreatedByUser] = Convert.ToString(Guid.Empty);

                                        if (context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Editor) != null)
                                            _targetItem[DatabaseObjects.Columns.ModifiedByUser] = context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.Editor);
                                        else
                                            _targetItem[DatabaseObjects.Columns.ModifiedByUser] = Convert.ToString(Guid.Empty);

                                        _targetItem[DatabaseObjects.Columns.Created] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                        _targetItem[DatabaseObjects.Columns.Modified] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                        _targetItem[DatabaseObjects.Columns.TicketId] = item[SPDatabaseObjects.Columns.TicketId];
                                        _targetItem[DatabaseObjects.Columns.Title] = item[SPDatabaseObjects.Columns.Title];
                                        _targetItem[DatabaseObjects.Columns.Status] = item[SPDatabaseObjects.Columns.TicketStatus];
                                        _targetItem[DatabaseObjects.Columns.History] = item[SPDatabaseObjects.Columns.History];
                                        _targetItem[DatabaseObjects.Columns.Closed] = item[SPDatabaseObjects.Columns.TicketClosed];
                                        if (item[SPDatabaseObjects.Columns.TicketCloseDate] != null)
                                            _targetItem[DatabaseObjects.Columns.CloseDate] = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketCloseDate]);
                                        if (moduleName != ModuleNames.CMDB)
                                            _targetItem[DatabaseObjects.Columns.Description] = item[SPDatabaseObjects.Columns.TicketDescription];
                                        _targetItem[DatabaseObjects.Columns.Comment] = item[SPDatabaseObjects.Columns.TicketComment];
                                        _targetItem["CustomUGChoice01"] = item["CustomUGChoice01"] == null ? DBNull.Value : item["CustomUGChoice01"];
                                        _targetItem["CustomUGChoice02"] = item["CustomUGChoice02"] == null ? DBNull.Value : item["CustomUGChoice02"];
                                        _targetItem["CustomUGChoice03"] = item["CustomUGChoice03"] == null ? DBNull.Value : item["CustomUGChoice03"];
                                        _targetItem["CustomUGChoice04"] = item["CustomUGChoice04"] == null ? DBNull.Value : item["CustomUGChoice04"];

                                        _targetItem["CustomUGDate01"] = item["CustomUGDate01"] == null ? DBNull.Value : item["CustomUGDate01"];
                                        _targetItem["CustomUGDate02"] = item["CustomUGDate02"] == null ? DBNull.Value : item["CustomUGDate02"];
                                        _targetItem["CustomUGDate03"] = item["CustomUGDate03"] == null ? DBNull.Value : item["CustomUGDate03"];
                                        _targetItem["CustomUGDate04"] = item["CustomUGDate04"] == null ? DBNull.Value : item["CustomUGDate04"];

                                        _targetItem["CustomUGText01"] = item["CustomUGText01"] == null ? DBNull.Value : item["CustomUGText01"];
                                        _targetItem["CustomUGText02"] = item["CustomUGText02"] == null ? DBNull.Value : item["CustomUGText02"];
                                        _targetItem["CustomUGText03"] = item["CustomUGText03"] == null ? DBNull.Value : item["CustomUGText03"];
                                        _targetItem["CustomUGText04"] = item["CustomUGText04"] == null ? DBNull.Value : item["CustomUGText04"];
                                        _targetItem["CustomUGText05"] = item["CustomUGText05"] == null ? DBNull.Value : item["CustomUGText05"];
                                        _targetItem["CustomUGText06"] = item["CustomUGText06"] == null ? DBNull.Value : item["CustomUGText06"];
                                        _targetItem["CustomUGText07"] = item["CustomUGText07"] == null ? DBNull.Value : item["CustomUGText07"];
                                        _targetItem["CustomUGText08"] = item["CustomUGText08"] == null ? DBNull.Value : item["CustomUGText08"];

                                        _targetItem["CustomUGUser01"] = item["CustomUGUser01"] == null ? DBNull.Value : item["CustomUGUser01"];
                                        _targetItem["CustomUGUser02"] = item["CustomUGUser02"] == null ? DBNull.Value : item["CustomUGUser02"];
                                        _targetItem["CustomUGUser03"] = item["CustomUGUser03"] == null ? DBNull.Value : item["CustomUGUser03"];
                                        _targetItem["CustomUGUser04"] = item["CustomUGUser04"] == null ? DBNull.Value : item["CustomUGUser04"];

                                        _targetItem["CustomUGUserMulti01"] = item["CustomUGUserMulti01"] == null ? DBNull.Value : item["CustomUGUserMulti01"];
                                        _targetItem["CustomUGUserMulti02"] = item["CustomUGUserMulti02"] == null ? DBNull.Value : item["CustomUGUserMulti02"];
                                        _targetItem["CustomUGUserMulti03"] = item["CustomUGUserMulti03"] == null ? DBNull.Value : item["CustomUGUserMulti03"];
                                        _targetItem["CustomUGUserMulti04"] = item["CustomUGUserMulti04"] == null ? DBNull.Value : item["CustomUGUserMulti04"];

                                        _targetItem[DatabaseObjects.Columns.Rejected] = item[SPDatabaseObjects.Columns.TicketRejected] == null ? false : item[SPDatabaseObjects.Columns.TicketRejected];
                                        _targetItem[DatabaseObjects.Columns.ClassificationSizeChoice] = item[SPDatabaseObjects.Columns.TicketClassificationSize];
                                        _targetItem[DatabaseObjects.Columns.ProjectConstraints] = item[SPDatabaseObjects.Columns.ProjectConstraints];
                                        _targetItem[DatabaseObjects.Columns.AssignedAnalyst] = item[SPDatabaseObjects.Columns.AssignedAnalyst];
                                        _targetItem[DatabaseObjects.Columns.ProjectState] = item[SPDatabaseObjects.Columns.ProjectState];
                                        ClientOM.FieldUserValue _AssignedByUser = item[SPDatabaseObjects.Columns.TicketAssignedBy] as ClientOM.FieldUserValue;
                                        if (_AssignedByUser != null && userList != null)
                                            _targetItem[DatabaseObjects.Columns.AssignedByUser] = userList.GetTargetID(Convert.ToString(_AssignedByUser.LookupId));

                                        _targetItem[DatabaseObjects.Columns.IsITGApprovalRequired] = item[SPDatabaseObjects.Columns.IsITGApprovalRequired] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsITGApprovalRequired];
                                        _targetItem[DatabaseObjects.Columns.IsSteeringApprovalRequired] = item[SPDatabaseObjects.Columns.IsSteeringApprovalRequired] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.IsSteeringApprovalRequired];
                                        _targetItem[DatabaseObjects.Columns.ClassificationNotes] = item[SPDatabaseObjects.Columns.TicketClassificationNotes];
                                        _targetItem[DatabaseObjects.Columns.TicketClassificationScope] = item[SPDatabaseObjects.Columns.TicketClassificationScope];
                                        _targetItem[DatabaseObjects.Columns.ProjectComplexity] = item[SPDatabaseObjects.Columns.ProjectComplexity] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ProjectComplexity];
                                        _targetItem[DatabaseObjects.Columns.TechnologyReliabilityChoice] = item[SPDatabaseObjects.Columns.TechnologyReliability];
                                        _targetItem[DatabaseObjects.Columns.TechnologySecurityChoice] = item[SPDatabaseObjects.Columns.TicketTechnologySecurity];
                                        _targetItem[DatabaseObjects.Columns.TechnologyImpactChoice] = item[SPDatabaseObjects.Columns.TechnologyImpact];
                                        _targetItem[DatabaseObjects.Columns.InternalCapabilityChoice] = item[SPDatabaseObjects.Columns.InternalCapability];
                                        _targetItem[DatabaseObjects.Columns.InternalCapabilityChoice] = item[SPDatabaseObjects.Columns.InternalCapability];
                                        _targetItem[DatabaseObjects.Columns.VendorSupport] = item[SPDatabaseObjects.Columns.VendorSupport];
                                        _targetItem[DatabaseObjects.Columns.ImpactRevenueIncreaseChoice] = item[SPDatabaseObjects.Columns.ImpactRevenueIncrease];
                                        _targetItem[DatabaseObjects.Columns.ImpactBusinessGrowthChoice] = item[SPDatabaseObjects.Columns.ImpactBusinessGrowth];
                                        _targetItem[DatabaseObjects.Columns.ImpactReducesRiskChoice] = item[SPDatabaseObjects.Columns.ImpactReducesRisk];
                                        _targetItem[DatabaseObjects.Columns.ImpactIncreasesProductivityChoice] = item[SPDatabaseObjects.Columns.ImpactIncreasesProductivity];
                                        _targetItem[DatabaseObjects.Columns.ImpactReducesExpensesChoice] = item[SPDatabaseObjects.Columns.ImpactReducesExpenses];
                                        _targetItem[DatabaseObjects.Columns.ImpactDecisionMakingChoice] = item[SPDatabaseObjects.Columns.ImpactDecisionMaking];
                                        _targetItem[DatabaseObjects.Columns.PctROI] = item[SPDatabaseObjects.Columns.PctROI] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.PctROI];
                                        _targetItem[DatabaseObjects.Columns.StrategicInitiativeChoice] = item[SPDatabaseObjects.Columns.TicketStrategicInitiative];
                                        _targetItem[DatabaseObjects.Columns.PaybackCostSavings] = item[SPDatabaseObjects.Columns.PaybackCostSavings];
                                        _targetItem[DatabaseObjects.Columns.ContributionToStrategy] = item[SPDatabaseObjects.Columns.ContributionToStrategy];
                                        _targetItem[DatabaseObjects.Columns.CustomerBenefitChoice] = item[SPDatabaseObjects.Columns.CustomerBenefit];
                                        _targetItem[DatabaseObjects.Columns.RegulatoryChoice] = item[SPDatabaseObjects.Columns.Regulatory];
                                        _targetItem[DatabaseObjects.Columns.ITLifecycleRefreshChoice] = item[SPDatabaseObjects.Columns.ITLifecycleRefresh];
                                        _targetItem[DatabaseObjects.Columns.BreakEvenIn] = item[SPDatabaseObjects.Columns.BreakEvenIn] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.BreakEvenIn];
                                        _targetItem[DatabaseObjects.Columns.EliminatesHeadcount] = item[SPDatabaseObjects.Columns.EliminatesHeadcount] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.EliminatesHeadcount];
                                        _targetItem[DatabaseObjects.Columns.OtherDescribe] = item[SPDatabaseObjects.Columns.TicketOtherDescribe];
                                        _targetItem[DatabaseObjects.Columns.CustomerProgram] = item[SPDatabaseObjects.Columns.CustomerProgram];
                                        _targetItem[DatabaseObjects.Columns.ProductCode] = item[SPDatabaseObjects.Columns.ProductCode];
                                        _targetItem[DatabaseObjects.Columns.ProductName] = item[SPDatabaseObjects.Columns.ProductName];
                                        ClientOM.FieldUserValue pusers = item[SPDatabaseObjects.Columns.TicketProjectDirector] as ClientOM.FieldUserValue;
                                        if (userList != null && pusers != null)
                                        {
                                            _targetItem[DatabaseObjects.Columns.TicketProjectDirector] = userList.GetTargetID(Convert.ToString(pusers.LookupId));
                                        }
                                        _targetItem[DatabaseObjects.Columns.CostSavings] = Convert.ToDouble(item[SPDatabaseObjects.Columns.CostSavings]);
                                        _targetItem[DatabaseObjects.Columns.ClientLookup] = item[SPDatabaseObjects.Columns.ClientLookup] == null ? DBNull.Value : item[SPDatabaseObjects.Columns.ClientLookup];
                                        ticketSchema.Rows.Add(_targetItem);

                                    }
                                    catch (Exception ex)
                                    {
                                        ULog.WriteException(ex);
                                    }
                                    var pmmHistoryStatus = ticketDal.SaveHistory(DatabaseObjects.Tables.PMMHistory, _targetItem);

                                }

                            }

                        } while (position != null);
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }

                ULog.WriteLog($"Updating {moduleName} DecisionLog");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    DecisionLog dlogItem = null;
                    targetNewItemCount = 0;
                    listName = SPDatabaseObjects.Lists.DecisionLog;
                    DecisionLogManager decisionLogManager = new DecisionLogManager(context.AppContext);
                    List<DecisionLog> Data = decisionLogManager.Load();
                    if (import && deleteBeforeImport)
                    {
                        try
                        {
                            decisionLogManager.Delete(Data);
                            Data = new List<DecisionLog>();
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteLog("Problem while deleting DecisionLog records");
                            ULog.WriteException(ex);
                        }
                    }
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList _userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty; string ticketid = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = null;
                        collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            dlogItem = null;
                            if (dlogItem == null)
                            {
                                dlogItem = new DecisionLog();
                                Data.Add(dlogItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (dlogItem.ID == 0 || importWithUpdate)
                                {
                                    dlogItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                    dlogItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    dlogItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                    dlogItem.ReleaseID = Convert.ToString(item[SPDatabaseObjects.Columns.ReleaseID]);
                                    dlogItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.UGITAssignedTo] as ClientOM.FieldUserValue[];
                                    if (_userList != null && users != null)
                                        dlogItem.AssignedTo = _userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                    dlogItem.AdditionalComments = Convert.ToString(item[SPDatabaseObjects.Columns.AdditionalComments]);
                                    dlogItem.ModuleName = moduleName;
                                    dlogItem.DecisionStatus = Convert.ToString(item[SPDatabaseObjects.Columns.DecisionStatus]);
                                    ClientOM.FieldUserValue[] _DecisionMaker = item[SPDatabaseObjects.Columns.DecisionMaker] as ClientOM.FieldUserValue[];
                                    if (_userList != null && _DecisionMaker != null)
                                        dlogItem.DecisionMaker = _userList.GetTargetIDs(_DecisionMaker.Select(x => x.LookupId.ToString()).ToList());
                                    if (item[SPDatabaseObjects.Columns.DateIdentified] != null)
                                        dlogItem.DateAssigned = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DateIdentified]);
                                    dlogItem.Decision = Convert.ToString(item[SPDatabaseObjects.Columns.Decision]);
                                    dlogItem.DecisionSource = Convert.ToString(item[SPDatabaseObjects.Columns.DecisionSource]);
                                    if (item[SPDatabaseObjects.Columns.DecisionDate] != null)
                                        dlogItem.DecisionDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.DecisionDate]);
                                    dlogItem.Deleted = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsDeleted]);
                                }
                                if (dlogItem.ID > 0)
                                    decisionLogManager.Update(dlogItem);
                                else
                                    decisionLogManager.Insert(dlogItem);

                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} {moduleName} DecisionLog");
                }


                ULog.WriteLog($"Updating {moduleName} ProjectStageHistory");
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    ProjectStageHistory plogItem = null;
                    targetNewItemCount = 0;
                    listName = SPDatabaseObjects.Lists.ProjectStageHistory;
                    ProjectStageHistoryManager projectStageHistory = new ProjectStageHistoryManager(context.AppContext);
                    List<ProjectStageHistory> Data = projectStageHistory.Load();
                    if (import && deleteBeforeImport)
                    {
                        try
                        {
                            projectStageHistory.Delete(Data);
                            Data = new List<ProjectStageHistory>();
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteLog("Problem while deleting DecisionLog records");
                            ULog.WriteException(ex);
                        }
                    }
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList _userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty; string ticketid = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = null;
                        collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            plogItem = null;
                            if (plogItem == null)
                            {
                                plogItem = new ProjectStageHistory();
                                Data.Add(plogItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (plogItem.ID == 0 || importWithUpdate)
                                {
                                    plogItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                    plogItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    plogItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                    plogItem.TaskID = Convert.ToString(item[SPDatabaseObjects.Columns.TaskID]);
                                    plogItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITStartDate]);
                                    plogItem.EndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);
                                    plogItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                    plogItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                                    ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                    if (_userList != null && users != null)
                                        plogItem.CreatedBy = _userList.GetTargetID(Convert.ToString(users.LookupId));

                                    ClientOM.FieldUserValue Editor = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                    if (_userList != null && Editor != null)
                                        plogItem.ModifiedBy = _userList.GetTargetID(Convert.ToString(Editor.LookupId));

                                }
                                if (targetItem.ID > 0)
                                {
                                    projectStageHistory.Update(plogItem);
                                }
                                else
                                    projectStageHistory.Insert(plogItem);

                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} {moduleName} ProjectStage History");
                }
            }
            UpdateTicketTaskPredecessors();
        }

        private void UpdateTicketTaskPredecessors()
        {
            UGITTaskManager mgr = new UGITTaskManager(context.AppContext);
            List<UGITTask> dbData = mgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);

            dbData.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Predecessors) && x.Predecessors.Contains(";#"))
                {
                    List<string> collection = new List<string>();
                    List<string> pressors = new List<string>();
                    collection = UGITUtility.SplitString(x.Predecessors, Constants.Separator).ToList();
                    foreach (var item in collection)
                    {
                        pressors.Add(UGITUtility.ObjectToString(dbData.Where(y => y.Title == item.Trim() && y.TicketId == x.TicketId).FirstOrDefault().ID));
                    }
                    x.Predecessors = string.Join(",", pressors.ToArray());
                }
                else if (!string.IsNullOrEmpty(x.Predecessors))
                    x.Predecessors = UGITUtility.ObjectToString(dbData.Where(y => y.Title == x.Predecessors && y.TicketId == x.TicketId).FirstOrDefault().ID);
            });

            //dbData.ForEach(x =>
            //{
            //    if (!string.IsNullOrEmpty(x.Predecessors))
            //        x.Predecessors = Convert.ToString(context.GetTargetLookupValueOptionLong(Convert.ToString(x.Predecessors), DatabaseObjects.Columns.Predecessors, DatabaseObjects.Tables.ModuleTasks));
            //});

            mgr.UpdateItems(dbData);


            // updating Predecessors for ModuleTask History

            ModuleTaskHistoryManager _mgr = new ModuleTaskHistoryManager(context.AppContext);
            List<ModuleTasksHistory> _dbData = _mgr.Load(x => x.TicketId.StartsWith($"{moduleName}-") && x.ModuleNameLookup == moduleName);

            _dbData.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Predecessors) && x.Predecessors.Contains(";#"))
                {
                    List<string> collection = new List<string>();
                    List<string> pressors = new List<string>();
                    collection = UGITUtility.SplitString(x.Predecessors, Constants.Separator).ToList();
                    foreach (var item in collection)
                    {
                        pressors.Add(UGITUtility.ObjectToString(_dbData.Where(y => y.Title == item.Trim() && y.TicketId == x.TicketId).FirstOrDefault().ID));
                    }
                    x.Predecessors = string.Join(",", pressors.ToArray());
                }
                else if (!string.IsNullOrEmpty(x.Predecessors))
                    x.Predecessors = UGITUtility.ObjectToString(_dbData.Where(y => y.Title == x.Predecessors && y.TicketId == x.TicketId).FirstOrDefault().ID);
            });
            _mgr.UpdateItems(_dbData);

        }
        public override void UpdateRelatedTickets()
        {
            //base.UpdateRelatedTickets();

            //bool import = context.IsImportEnable("RelatedTickets", moduleName);
            //if (!import)
            //    return;

            //ULog.WriteLog($"Updating Related Tickets ({moduleName})");
            //{
            //    TicketRelationManager mgr = new TicketRelationManager(context.AppContext);
            //    List<TicketRelation> dbData = mgr.Load(x => x.ParentModuleName == moduleName);
            //    if (deleteBeforeImport)
            //    {
            //        try
            //        {
            //            mgr.Delete(dbData);
            //            dbData = new List<TicketRelation>();
            //        }
            //        catch (Exception ex)
            //        {
            //            ULog.WriteLog("Problem while deleting records");
            //            ULog.WriteException(ex);
            //        }
            //    }

            //    int targetNewItemCount = 0;
            //    TicketRelation targetItem = null;

            //    using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
            //    {
            //        string listName = SPDatabaseObjects.Lists.TicketRelationship;
            //        if (moduleName == ModuleNames.CMDB)
            //            listName = SPDatabaseObjects.Lists.AssetRelations;
            //        ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
            //        MappedItemList locaionlist = context.GetMappedList(listName);
            //        ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
            //        string title = string.Empty;
            //        do
            //        {
            //            ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
            //            position = collection.ListItemCollectionPosition;
            //            try
            //            {
            //                foreach (ClientOM.ListItem item in collection)
            //                {
            //                    targetItem = null;
            //                    //targetItem = dbData.FirstOrDefault(x => x.ParentTicketID == Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]) && x.ParentModuleName == moduleName);
            //                    //if (moduleName == ModuleNames.CMDB)
            //                    //    targetItem = dbData.FirstOrDefault(x => x.ParentTicketID == Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]) && x.ChildTicketID == Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]));
            //                    if (targetItem == null)
            //                    {
            //                        targetItem = new TicketRelation();
            //                        dbData.Add(targetItem);
            //                        targetNewItemCount++;
            //                    }

            //                    if (targetItem.ID == 0 || importWithUpdate)
            //                    {
            //                        //string ModuleNameLookup = item[SPDatabaseObjects.Columns.ModuleNameLookup] == null ? string.Empty : Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
            //                        targetItem.ParentModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]));
            //                        targetItem.ParentTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
            //                        targetItem.ChildTicketID = Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]);
            //                        targetItem.ChildModuleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[SPDatabaseObjects.Columns.ChildTicketId]));
            //                    }

            //                    if (targetItem.ID > 0)
            //                        mgr.Update(targetItem);
            //                    else
            //                        mgr.Insert(targetItem);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                ULog.WriteException(ex);

            //            }

            //        } while (position != null);

            //        ULog.WriteLog($"{targetNewItemCount} Related Tickets added");
            //    }
            //}
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
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName, true);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ProjectMonitorState;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList MappedMonitorlist = context.GetMappedList(SPDatabaseObjects.Lists.ModuleMonitors);
                    MappedItemList MappedMonitorOptionslist = context.GetMappedList(SPDatabaseObjects.Lists.ModuleMonitorOptions);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                //targetItem = dbData.FirstOrDefault(x => x.TicketId == Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue));
                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                }

                                if (targetItem == null)
                                {
                                    targetItem = new ProjectMonitorState();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }

                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                        targetItem.TicketId = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    targetItem.ProjectMonitorWeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.ProjectMonitorWeight]);
                                    targetItem.ProjectMonitorNotes = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectMonitorNotes]);
                                    targetItem.AutoCalculate = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoCalculate]);
                                    if (item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup] != null)
                                    {
                                        targetItem.ModuleMonitorNameLookup = UGITUtility.StringToLong(MappedMonitorlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup]).LookupId.ToString()));
                                    }
                                    if (item[SPDatabaseObjects.Columns.ModuleMonitorOptionIdLookup] != null)
                                    {
                                        targetItem.ModuleMonitorOptionIdLookup = UGITUtility.StringToLong(MappedMonitorOptionslist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleMonitorOptionIdLookup]).LookupId.ToString()));
                                    }
                                    if (moduleName == "PMM")
                                    {
                                        targetItem.ModuleNameLookup = "PMM";
                                    }
                                    if (dr != null)
                                        targetItem.PMMIdLookup = Convert.ToInt64(dr["Id"]);

                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ProjectMonitorState added");
                }
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
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ProjectMonitorStateHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList MappedMonitorlist = context.GetMappedList(SPDatabaseObjects.Lists.ModuleMonitors);
                    MappedItemList MappedMonitorOptionslist = context.GetMappedList(SPDatabaseObjects.Lists.ModuleMonitorOptions);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            //targetItem = dbData.FirstOrDefault(x => x.TicketId == Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue));
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));

                            if (targetItem == null)
                            {
                                targetItem = new ProjectMonitorStateHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.TicketId = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue);
                                    if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                        targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);
                                    if (item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup] != null)
                                    {
                                        targetItem.ModuleMonitorNameLookup = UGITUtility.StringToLong(MappedMonitorlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleMonitorNameLookup]).LookupId.ToString()));
                                    }
                                    if (item[SPDatabaseObjects.Columns.ModuleMonitorOptionIdLookup] != null)
                                    {
                                        targetItem.ModuleMonitorOptionIdLookup = UGITUtility.StringToLong(MappedMonitorOptionslist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleMonitorOptionIdLookup]).LookupId.ToString()));
                                    }
                                    targetItem.AutoCalculate = Convert.ToBoolean(item[SPDatabaseObjects.Columns.AutoCalculate]);
                                    targetItem.ProjectMonitorWeight = Convert.ToInt32(item[SPDatabaseObjects.Columns.ProjectMonitorWeight]);
                                    targetItem.ProjectMonitorNotes = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectMonitorNotes]);
                                    if (moduleName == "PMM")
                                    {
                                        targetItem.ModuleName = "PMM";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }


                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ProjectMonitorStateHistory added");
                }
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
                    dbData.ForEach(x =>
                    {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                    });
                }

                int targetNewItemCount = 0;
                Sprint targetItem = null;
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.Sprint;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new Sprint();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITStartDate]);
                                targetItem.EndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);
                                targetItem.TaskEstimatedHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.RemainingHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.RemainingHours]);
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                }
                                if (dr != null)
                                    targetItem.PMMIdLookup = Convert.ToInt64(dr["Id"]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} Sprint added");
                }

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
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ProjectReleases;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ProjectReleases();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.ReleaseID = Convert.ToString(item[SPDatabaseObjects.Columns.ReleaseID]);
                                targetItem.ReleaseDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.ReleaseDate]);
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                }
                                if (dr != null)
                                    targetItem.PMMIdLookup = Convert.ToInt64(dr["Id"]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);
                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));
                        }
                    } while (position != null);

                    ULog.WriteLog($"{targetNewItemCount} ProjectReleases added");
                }
            }

        }
        public override void SprintTasks()
        {
            base.SprintTasks();

            bool import = context.IsImportEnable("SprintTasks", moduleName);
            if (!import)
                return;
            int targetNewItemCount = 0;
            TicketManager ticketMgr = new TicketManager(context.AppContext);
            ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
            UGITModule module = moduleMgr.LoadByName(moduleName);
            ULog.WriteLog($"Updating SprintTasks ({moduleName})");
            {
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
                    dbData.ForEach(x =>
                    {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                        if (x.SprintLookup != null)
                            x.SprintTitle = dbSprintData.FirstOrDefault(y => y.ID == x.SprintLookup).Title;
                    });
                }


                SprintTasks targetItem = null;
                
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.SprintTasks;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList ProjectReleaseslist = context.GetMappedList(SPDatabaseObjects.Lists.ProjectReleases);
                    MappedItemList sprintlist = context.GetMappedList(SPDatabaseObjects.Lists.Sprint);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new SprintTasks();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.TaskStatus = Convert.ToString(item[SPDatabaseObjects.Columns.TaskStatus]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Priority = Convert.ToString(item[DatabaseObjects.Columns.TaskPriority]);
                                targetItem.Body = Convert.ToString(item[DatabaseObjects.Columns.Body]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                targetItem.TaskGroup = Convert.ToString(item[SPDatabaseObjects.Columns.TaskGroup]);
                                if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                if (item[SPDatabaseObjects.Columns.TaskDueDate] != null)
                                    targetItem.TaskDueDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TaskDueDate]);
                                targetItem.TaskEstimatedHours = item[SPDatabaseObjects.Columns.TaskEstimatedHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.TaskActualHours = item[SPDatabaseObjects.Columns.TaskActualHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TaskActualHours]);
                                targetItem.ParentTask = Convert.ToInt32(item[SPDatabaseObjects.Columns.ParentTask]);
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                targetItem.ChildCount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITChildCount]);
                                targetItem.Level = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITLevel]);
                                targetItem.Contribution = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITContribution]);
                                targetItem.Duration = Convert.ToInt32(item[SPDatabaseObjects.Columns.UGITDuration]);
                                if (item[SPDatabaseObjects.Columns.UGITProposedDate] != null)
                                    targetItem.ProposedDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITProposedDate]);
                                targetItem.ProposedStatus = Convert.ToString(item[SPDatabaseObjects.Columns.UGITProposedStatus]);
                                targetItem.IsMilestone = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsMilestone]);
                                targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                targetItem.TaskBehaviour = Convert.ToString(item[SPDatabaseObjects.Columns.TaskBehaviour]);
                                targetItem.ShowOnProjectCalendar = Convert.ToBoolean(item[SPDatabaseObjects.Columns.ShowOnProjectCalendar]);
                                targetItem.TaskBehaviour = Convert.ToString(item[SPDatabaseObjects.Columns.TaskBehaviour]);
                                targetItem.SprintOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.SprintOrder]);
                                if (item[SPDatabaseObjects.Columns.SprintLookup] != null)
                                {
                                    targetItem.SprintLookup = UGITUtility.StringToLong(sprintlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.SprintLookup]).LookupId.ToString()));
                                }
                                if (item[SPDatabaseObjects.Columns.ReleaseLookup] != null)
                                {
                                    targetItem.ReleaseLookup = UGITUtility.StringToLong(sprintlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ReleaseLookup]).LookupId.ToString()));
                                }
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                }
                                if (dr != null)
                                    targetItem.PMMIdLookup = Convert.ToInt64(dr["Id"]);
                                ClientOM.FieldUserValue users = item[SPDatabaseObjects.Columns.AssignedTo] as ClientOM.FieldUserValue;
                                if (userlist != null && users != null)
                                {
                                    targetItem.AssignedTo = userlist.GetTargetID(Convert.ToString(users.LookupId));
                                }
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} SprintTasks added");

            }

            ULog.WriteLog($"Updating SprintSummary ({moduleName})");
            {
                SprintSummaryManager mgr = new SprintSummaryManager(context.AppContext);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    SprintSummary targetItem = null;
                    string listName = SPDatabaseObjects.Lists.SprintSummary;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList sprintlist = context.GetMappedList(SPDatabaseObjects.Lists.Sprint);
                    MappedItemList userlist = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new SprintSummary();
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ItemOrder = Convert.ToInt32(item[SPDatabaseObjects.Columns.ItemOrder]);
                                targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                targetItem.PercentComplete = item[SPDatabaseObjects.Columns.PercentComplete] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.PercentComplete]) * 100;
                                if (item[SPDatabaseObjects.Columns.UGITStartDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITStartDate]);

                                if (item[SPDatabaseObjects.Columns.UGITEndDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);

                                targetItem.TaskEstimatedHours = item[SPDatabaseObjects.Columns.TaskEstimatedHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.TaskEstimatedHours]);
                                targetItem.RemainingHours = item[SPDatabaseObjects.Columns.RemainingHours] == null ? 0 : UGITUtility.StringToDouble(item[SPDatabaseObjects.Columns.RemainingHours]);
                                if (item[SPDatabaseObjects.Columns.SprintLookup] != null)
                                {
                                    targetItem.SprintLookup = UGITUtility.StringToLong(sprintlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.SprintLookup]).LookupId.ToString()));
                                }
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                    if (dr != null)
                                        targetItem.PMMIdLookup = Convert.ToInt64(dr["Id"]);
                                }
                                ClientOM.FieldUserValue Author = item[SPDatabaseObjects.Columns.Author] as ClientOM.FieldUserValue;
                                if (Author != null && userlist != null)
                                    targetItem.CreatedBy = userlist.GetTargetID(Convert.ToString(Author.LookupId));
                                ClientOM.FieldUserValue Modifeduser = item[SPDatabaseObjects.Columns.Editor] as ClientOM.FieldUserValue;
                                if (Modifeduser != null && userlist != null)
                                    targetItem.ModifiedBy = userlist.GetTargetID(Convert.ToString(Modifeduser.LookupId));
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);
                                targetItem.Modified = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Modified]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} SprintSummary added");
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
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.PMMEvents;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new PMMEvents();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                DataRow dr = null; string Id;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                }
                                if (dr != null)
                                    targetItem.PMMIdLookup = Convert.ToString(dr["Id"]);
                                targetItem.RecurrenceData = Convert.ToString(item[SPDatabaseObjects.Columns.RecurrenceInfo]);
                                targetItem.EventType = Convert.ToString(item[SPDatabaseObjects.Columns.UGITEventType]);
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.UGITStatus]);
                                if (item[SPDatabaseObjects.Columns.StartDate] != null)
                                    targetItem.StartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartDate]);
                                if (item[SPDatabaseObjects.Columns.EndDate] != null)
                                    targetItem.EndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.EndDate]);
                                targetItem.Comments = Convert.ToString(item[SPDatabaseObjects.Columns.Comments]);
                                targetItem.Category = Convert.ToString(item[SPDatabaseObjects.Columns.Category]);
                                targetItem.fAllDayEvent = Convert.ToString(item[SPDatabaseObjects.Columns.fAllDayEvent]);
                                targetItem.fRecurrence = Convert.ToString(item["fRecurrence"]);
                                targetItem.Location = Convert.ToString(item["Location"]);
                                targetItem.Duration = Convert.ToString(item[SPDatabaseObjects.Columns.Duration]);
                                targetItem.EventCanceled = Convert.ToBoolean(item["EventCanceled"]);
                                targetItem.Workspace = Convert.ToString(item["Workspace"]);
                                targetItem.WorkspaceLink = Convert.ToString(item["WorkspaceLink"]);
                                targetItem.TimeZone = Convert.ToString(item["TimeZone"]);
                                targetItem.XMLTZone = Convert.ToString(item["XMLTZone"]);
                                targetItem.MasterSeriesItemID = Convert.ToString(item["MasterSeriesItemID"]);

                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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
                List<NPRResource> dbData = mgr.Load(x => x.ModuleNameLookup == moduleName && x.TenantID == context.AppContext.TenantID);
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.NPRResources;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    MappedItemList skilllist = context.GetMappedList(SPDatabaseObjects.Lists.UserSkills);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new NPRResource();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                    targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                    targetItem.TicketId = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketNPRIdLookup]).LookupValue);
                                    targetItem.BudgetDescription = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetDescription]);
                                    targetItem.BudgetTypeChoice = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetType]);
                                    targetItem._ResourceType = Convert.ToString(item[SPDatabaseObjects.Columns._ResourceType]);
                                    targetItem.NoOfFTEs = Convert.ToDouble(item[SPDatabaseObjects.Columns.TicketNoOfFTEs]);
                                    targetItem.EstimatedHours = Convert.ToInt32(item[SPDatabaseObjects.Columns.EstimatedHours]);
                                    targetItem.HourlyRate = Convert.ToDecimal(Convert.IsDBNull(item[DatabaseObjects.Columns.HourlyRate]));
                                    targetItem.RoleNameChoice = Convert.ToString(item[SPDatabaseObjects.Columns.RoleName]);
                                    targetItem.ModuleNameLookup = moduleName;
                                    ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.RequestedResources] as ClientOM.FieldUserValue[];
                                    if (userList != null && users != null)
                                    {
                                        targetItem.RequestedResourcesUser = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                    }
                                    ClientOM.FieldLookupValue skill = item[SPDatabaseObjects.Columns.UserSkillLookup] as ClientOM.FieldLookupValue;
                                    if (skilllist != null && skill != null)
                                    {
                                        long id = UGITUtility.StringToLong(skilllist.GetTargetID(Convert.ToString(skill.LookupId)));
                                        if (id > 0)
                                            targetItem.UserSkillLookup = id;
                                    }

                                }

                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }


                        }
                    } while (position != null);

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
                TicketCountTrends targetItem = null;
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.TicketCountTrends;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.ModuleName == Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new TicketCountTrends();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.ModuleName = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                if (item[SPDatabaseObjects.Columns.EndOfDay] != null)
                                    targetItem.EndOfDay = Convert.ToDateTime(item[SPDatabaseObjects.Columns.EndOfDay]);
                                targetItem.NumClosed = Convert.ToInt32(item[SPDatabaseObjects.Columns.NumClosed]);
                                targetItem.NumResolved = Convert.ToInt32(item[SPDatabaseObjects.Columns.NumResolved]);
                                targetItem.NumCreated = Convert.ToInt32(item[SPDatabaseObjects.Columns.NumCreated]);
                                targetItem.TotalActive = Convert.ToInt32(item[SPDatabaseObjects.Columns.TotalActive]);
                                targetItem.TotalClosed = Convert.ToInt32(item[SPDatabaseObjects.Columns.TotalClosed]);
                                targetItem.TotalOnHold = Convert.ToInt32(item[SPDatabaseObjects.Columns.TotalOnHold]);
                                targetItem.TotalResolved = Convert.ToInt32(item[SPDatabaseObjects.Columns.TotalResolved]);
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
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
                string listName = string.Empty;
                BudgetCategoryViewManager mgr_ = new BudgetCategoryViewManager(context.AppContext);
                List<BudgetCategory> dbData_ = mgr_.Load();
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    if (moduleName == "PMM")
                        listName = SPDatabaseObjects.Lists.PMMBudget;
                    else if (moduleName == "NPR")
                        listName = SPDatabaseObjects.Lists.NPRBudget;
                    else
                        listName = SPDatabaseObjects.Lists.ITGBudget;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList bcatlist = context.GetMappedList(SPDatabaseObjects.Lists.BudgetCategories);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty, val = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleBudget();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            try
                            {
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.ModuleName = moduleName;
                                    if (moduleName != "ITG")
                                    {
                                        targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                        targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                        targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                        targetItem.BudgetItem = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetItem]);
                                        targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetItem]);
                                        targetItem.IsAutoCalculated = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsAutoCalculated]);
                                    }
                                    if (moduleName == "PMM")
                                    {
                                        targetItem.BudgetStatus = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetStatus]);
                                        targetItem.UnapprovedAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UnapprovedAmount]);
                                        targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);

                                    }

                                    ClientOM.FieldLookupValue BudgetCategoryLookup = item["BudgetLookup"] as ClientOM.FieldLookupValue;

                                    if (bcatlist != null && BudgetCategoryLookup != null)
                                    {
                                        targetItem.BudgetCategoryLookup = Convert.ToInt64(bcatlist.GetTargetID(Convert.ToString(BudgetCategoryLookup.LookupId)));
                                    }

                                    string Id = string.Empty;
                                    if (moduleName == "PMM")
                                    {
                                        if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                        {
                                            Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                            //dr = ticketMgr.GetByTicketID(module, Id);
                                            targetItem.TicketId = Convert.ToString(Id);
                                        }
                                    }
                                    if (moduleName == "NPR")
                                    {
                                        if (item[SPDatabaseObjects.Columns.TicketNPRIdLookup] != null)
                                        {
                                            Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketNPRIdLookup]).LookupValue;
                                            //dr = ticketMgr.GetByTicketID(module, Id);
                                            targetItem.TicketId = Convert.ToString(Id);
                                        }
                                        NPRResourcesManager objNPRResourcesManager = new NPRResourcesManager(context.AppContext);
                                        List<NPRResource> lstrespurces = new List<NPRResource>();
                                        lstrespurces = objNPRResourcesManager.Load();
                                        if ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.NPRResourceLookup] != null)
                                        {
                                            val = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.NPRResourceLookup]).LookupValue;
                                            if (val != null)
                                                targetItem.ResourceLookup = Convert.ToInt32(lstrespurces.FirstOrDefault(x => x.Title == val).ID);
                                        }
                                    }
                                    if (moduleName == "ITG")
                                    {
                                        targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                        targetItem.GLCode = Convert.ToString(item[SPDatabaseObjects.Columns.GLCode]);
                                        targetItem.UnapprovedAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UnapprovedAmount]);
                                        targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BudgetStartDate]);
                                        targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BudgetEndDate]);
                                        long id = UGITUtility.StringToLong(context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.DepartmentLookup));
                                        if (id > 0)
                                            targetItem.DepartmentLookup = id;
                                    }
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                            }
                            if (targetItem != null)
                                locaionlist.Add(new MappedItem(Convert.ToString(item[SPDatabaseObjects.Columns.Id]), targetItem.ID.ToString()));

                        }
                    } while (position != null);

                }


                ULog.WriteLog($"{targetNewItemCount} ModuleBudget added");
            }
        }

        public override void ModuleBudgetActuals()
        {
            base.ModuleBudgetActuals();

            bool import = context.IsImportEnable("ModuleBudgetActuals", moduleName);
            if (moduleName == ModuleNames.NPR)
                import = false;
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
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string _listname = string.Empty;
                    string _list = string.Empty;
                    if (moduleName == "PMM")
                    {
                        _listname = SPDatabaseObjects.Lists.PMMBudgetActuals;
                        _list = DatabaseObjects.Tables.PMMBudget;
                    }
                    else if (moduleName == "ITG")
                    {
                        _list = DatabaseObjects.Tables.ITGBudget;
                        _listname = SPDatabaseObjects.Lists.ITGActual;
                    }
                    string Id = string.Empty; //DataRow dr = null;
                    MappedItemList modulebudgetlist = context.GetMappedList(_list);
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(_listname);
                    MappedItemList locaionlist = context.GetMappedList(_listname);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new BudgetActual();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    if (moduleName == "PMM" || moduleName == "ITG")
                                        targetItem.ModuleName = moduleName;
                                    else
                                        targetItem.ModuleName = Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                    if (moduleName == "PMM")
                                    {
                                        targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                        targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                    }
                                    if (moduleName == "ITG")
                                    {
                                        targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BudgetStartDate]);
                                        targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BudgetEndDate]);
                                        targetItem.ActualCost = Convert.ToDouble(item[SPDatabaseObjects.Columns.ActualCost]);
                                        targetItem.BudgetDescription = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                        if (modulebudgetlist != null && item[SPDatabaseObjects.Columns.ITGBudgetLookup] != null)
                                        {
                                            long id = UGITUtility.StringToLong(modulebudgetlist.GetTargetID(Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ITGBudgetLookup]).LookupId)));
                                            if (id > 0)
                                                targetItem.ModuleBudgetLookup = id;
                                        }

                                    }
                                    if (moduleName != "ITG")
                                    {
                                        targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                        if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                        {
                                            Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                            //dr = ticketMgr.GetByTicketID(module, Id);
                                            targetItem.TicketId = Id; //Convert.ToString(dr["Id"]);
                                        }
                                        if (modulebudgetlist != null && item[SPDatabaseObjects.Columns.PMMBudgetLookup] != null)
                                        {
                                            long id = UGITUtility.StringToLong(modulebudgetlist.GetTargetID(Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.PMMBudgetLookup]).LookupId)));
                                            if (id > 0)
                                                targetItem.ModuleBudgetLookup = id;
                                        }
                                    }
                                    if (moduleName == "ITG" || moduleName == "PMM")
                                    {
                                        targetItem.InvoiceNumber = Convert.ToString(item[SPDatabaseObjects.Columns.InvoiceNumber]);
                                        long id = UGITUtility.StringToLong(context.GetTargetValue(spContext, list, item, SPDatabaseObjects.Columns.VendorLookup));
                                        if (id > 0)
                                            targetItem.VendorLookup = id;
                                    }

                                    if (moduleName != "PMM" && moduleName != "ITG")
                                        targetItem.BudgetItem = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetItem]);
                                    else
                                        targetItem.BudgetItem = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);

                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                        catch (Exception e)
                        {
                            ULog.WriteException(e);

                        }

                    } while (position != null);
                }
                ULog.WriteLog($"{targetNewItemCount} ModuleBudgetActuals added");
            }
        }

        public override void ModuleBudgetActualsHistory()
        {
            base.ModuleBudgetActualsHistory();

            bool import = false;// context.IsImportEnable("ModuleBudgetActualsHistory", moduleName);
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string _listname = string.Empty;
                    if (moduleName == "PMM")
                        _listname = SPDatabaseObjects.Lists.PMMBudgetHistory;
                    else
                        _listname = SPDatabaseObjects.Lists.NPRBudget;
                    MappedItemList modulebudgetlist = context.GetMappedList(_listname);
                    string listName = SPDatabaseObjects.Lists.PMMBudgetHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleBudgetsActualHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleName = moduleName; //Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    targetItem.TicketId = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue);
                                }
                                if (modulebudgetlist != null && item[SPDatabaseObjects.Columns.BudgetLookup] != null)
                                {
                                    long id = UGITUtility.StringToLong(modulebudgetlist.GetTargetID(Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.BudgetLookup]).LookupId)));
                                    if (id > 0)
                                        targetItem.ModuleBudgetLookup = id;

                                }
                                if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);
                                targetItem.BaselineId = Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} ModuleBudgetActualsHistory added");
            }
        }

        public override void ModuleBudgetHistory()
        {
            base.ModuleBudgetHistory();

            bool import = context.IsImportEnable("ModuleBudgetHistory", moduleName);
            if (moduleName == ModuleNames.NPR)
                import = false;
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {

                    string listName = SPDatabaseObjects.Lists.PMMBudgetHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    string _listname = string.Empty;
                    if (moduleName == "PMM")
                        _listname = SPDatabaseObjects.Lists.PMMBudgetHistory;

                    MappedItemList modulebudgetlist = context.GetMappedList(SPDatabaseObjects.Lists.PMMBudget);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleBudgetHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleName = moduleName; //Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]);
                                targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                targetItem.AllocationEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationEndDate]);
                                targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);

                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    targetItem.TicketId = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue);
                                }
                                if (modulebudgetlist != null && item[SPDatabaseObjects.Columns.BudgetLookup] != null)
                                {
                                    targetItem.BudgetLookup = UGITUtility.StringToLong(modulebudgetlist.GetTargetID(Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.BudgetLookup]).LookupId)));
                                }
                                if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);
                                targetItem.BaselineId = Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);
                                if (moduleName == "PMM")
                                {
                                    targetItem.BudgetStatus = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetStatus]);
                                    targetItem.UnapprovedAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.UnapprovedAmount]);
                                    targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                    targetItem.IsAutoCalculated = Convert.ToBoolean(item[SPDatabaseObjects.Columns.IsAutoCalculated]);
                                }
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {

                    string listName = SPDatabaseObjects.Lists.MonthlyBudget;
                    if (moduleName == "PMM")
                        listName = SPDatabaseObjects.Lists.PMMMonthlyBudget;
                    if (moduleName == "NPR")
                        listName = SPDatabaseObjects.Lists.NPRMonthlyBudget;
                    if (moduleName == "ITG")
                        listName = SPDatabaseObjects.Lists.ITGMonthlyBudget;

                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    MappedItemList bcatlist = context.GetMappedList(SPDatabaseObjects.Lists.BudgetCategories);
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleMonthlyBudget();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ModuleName = moduleName;
                                targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);
                                targetItem.BudgetType = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetType]);
                                targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                if (moduleName == "NPR")
                                {
                                    if (item[SPDatabaseObjects.Columns.TicketNPRIdLookup] != null)
                                    {
                                        string Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketNPRIdLookup]).LookupValue;
                                        //dr = ticketMgr.GetByTicketID(module, Id);
                                        targetItem.TicketId = Convert.ToString(Id);
                                    }
                                }
                                if (moduleName == "PMM")
                                {
                                    if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                    {
                                        string Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                        targetItem.TicketId = Convert.ToString(Id);
                                    }
                                    targetItem.ActualCost = Convert.ToDouble(item[SPDatabaseObjects.Columns.ActualCost]);

                                }
                                if (moduleName == "ITG")
                                {
                                    targetItem.ProjectPlanedTotal = Convert.ToDouble(item["ProjectPlanedTotal"]);
                                    targetItem.NonProjectPlanedTotal = Convert.ToDouble(item["NonProjectPlanedTotal"]);
                                    targetItem.NonProjectActualTotal = Convert.ToDouble(item[SPDatabaseObjects.Columns.NonProjectActualTotal]);
                                    targetItem.ProjectActualTotal = Convert.ToDouble(item[SPDatabaseObjects.Columns.ProjectActualTotal]);
                                    targetItem.ActualCost = Convert.ToDouble(item[SPDatabaseObjects.Columns.ActualCost]);
                                    targetItem.EstimatedCost = Convert.ToDouble(item[SPDatabaseObjects.Columns.EstimatedCost]);
                                    targetItem.ResourceCost = Convert.ToDouble(item[SPDatabaseObjects.Columns.ResourceCost]);

                                    ClientOM.FieldLookupValue BudgetCategoryLookup = item["BudgetLookup"] as ClientOM.FieldLookupValue;

                                    if (bcatlist != null && BudgetCategoryLookup != null)
                                    {
                                        targetItem.BudgetCategoryLookup = Convert.ToInt64(bcatlist.GetTargetID(Convert.ToString(BudgetCategoryLookup.LookupId)));
                                    }
                                }

                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }
                ULog.WriteLog($"{targetNewItemCount} ModuleMonthlyBudget added");
            }
        }
        public override void ModuleMonthlyBudgetHistory()
        {
            base.ModuleMonthlyBudgetHistory();

            bool import = context.IsImportEnable("ModuleMonthlyBudgetHistory", moduleName);
            if (moduleName == ModuleNames.NPR)
                import = false;
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.PMMMonthlyBudgetHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    string _listname = string.Empty;
                    if (moduleName == "PMM")
                        _listname = SPDatabaseObjects.Lists.PMMMonthlyBudgetHistory;

                    MappedItemList modulebudgetlist = context.GetMappedList(SPDatabaseObjects.Lists.PMMBudget);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new ModuleMonthlyBudgetHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.BudgetType = Convert.ToString(item[SPDatabaseObjects.Columns.BudgetType]);
                                targetItem.ModuleName = moduleName;
                                targetItem.AllocationStartDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AllocationStartDate]);

                                targetItem.BudgetAmount = Convert.ToInt32(item[SPDatabaseObjects.Columns.BudgetAmount]);
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    targetItem.TicketId = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue);
                                }

                                if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);
                                targetItem.BaselineId = Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.PMMBaselineDetail;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new BaseLineDetails();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);

                                targetItem.BaselineId = Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);
                                targetItem.ModuleNameLookup = moduleName;
                                targetItem.BaselineComment = Convert.ToString(item[SPDatabaseObjects.Columns.BaselineComment]);
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                    targetItem.TicketID = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue);


                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ScheduleActions;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new SchedulerAction();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.StartTime = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartTime]);
                                targetItem.ActionType = Convert.ToString(item[SPDatabaseObjects.Columns.ActionType]);
                                targetItem.ActionTypeData = Convert.ToString(item[SPDatabaseObjects.Columns.ActionTypeData]);
                                targetItem.AlertCondition = Convert.ToString(item[SPDatabaseObjects.Columns.AlertCondition]);
                                targetItem.AttachmentFormat = Convert.ToString(item[SPDatabaseObjects.Columns.AttachmentFormat]);
                                targetItem.Attachments = Convert.ToString(item[SPDatabaseObjects.Columns.Attachments]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                targetItem.EmailBody = Convert.ToString(item[SPDatabaseObjects.Columns.EmailBody]);
                                targetItem.EmailIDTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDTo]);
                                targetItem.EmailIDCC = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDCC]);
                                targetItem.EmailIDTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDTo]);
                                targetItem.ListName = Convert.ToString(item[SPDatabaseObjects.Columns.ListName]);
                                targetItem.MailSubject = Convert.ToString(item[SPDatabaseObjects.Columns.MailSubject]);
                                targetItem.Recurring = Convert.ToBoolean(item[SPDatabaseObjects.Columns.Recurring]);
                                //targetItem.RecurringEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.RecurringEndDate]);
                                targetItem.RecurringInterval = Convert.ToInt32(item[SPDatabaseObjects.Columns.RecurringInterval]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);



                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ScheduleActionsArchive;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new SchedulerActionArchive();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.StartTime = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartTime]);
                                targetItem.ActionType = Convert.ToString(item[SPDatabaseObjects.Columns.ActionType]);
                                targetItem.EmailBody = Convert.ToString(item[SPDatabaseObjects.Columns.EmailBody]);
                                targetItem.EmailIDTo = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDTo]);
                                targetItem.EmailIDCC = Convert.ToString(item[SPDatabaseObjects.Columns.EmailIDCC]);
                                targetItem.MailSubject = Convert.ToString(item[SPDatabaseObjects.Columns.MailSubject]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.Recurring = Convert.ToBoolean(item[SPDatabaseObjects.Columns.Recurring]);
                                targetItem.RecurringInterval = Convert.ToInt32(item[SPDatabaseObjects.Columns.RecurringInterval]);
                                targetItem.RecurringEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.StartTime]);
                                targetItem.CustomProperties = Convert.ToString(item[SPDatabaseObjects.Columns.CustomProperties]);
                                targetItem.ActionTypeData = Convert.ToString(item[SPDatabaseObjects.Columns.ActionTypeData]);
                                targetItem.AlertCondition = Convert.ToString(item[SPDatabaseObjects.Columns.AlertCondition]);
                                targetItem.AgentJobStatus = Convert.ToString(item[SPDatabaseObjects.Columns.AgentJobStatus]);
                                targetItem.FileLocation = Convert.ToString(item[SPDatabaseObjects.Columns.FileLocation]);
                                targetItem.AgentJobStatus = Convert.ToString(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.ModuleNameLookup]).LookupValue);
                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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
                TicketManager ticketMgr = new TicketManager(context.AppContext);
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

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.PMMComments;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            if (targetItem == null)
                            {
                                targetItem = new PMMComments();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                string Id; DataRow dr = null;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                    targetItem.TicketId = Id; //Convert.ToString(dr["Id"]);
                                    if (dr != null)
                                        targetItem.PMMIdLookup = Convert.ToInt32(dr["Id"]);
                                }
                                targetItem.ProjectNoteType = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNoteType]);
                                targetItem.ProjectNote = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNote]);
                                if (item[SPDatabaseObjects.Columns.AccomplishmentDate] != null)
                                    targetItem.AccomplishmentDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.AccomplishmentDate]);
                                if (item[SPDatabaseObjects.Columns.UGITEndDate] != null)
                                    targetItem.EndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);

                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

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
                TicketManager ticketMgr = new TicketManager(context.AppContext);

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
                    dbData.ForEach(x =>
                    {
                        x.PMMTitle = Convert.ToString(PMMProjectsDb.AsEnumerable().FirstOrDefault(y => y.Field<long>(DatabaseObjects.Columns.ID) == x.PMMIdLookup)[DatabaseObjects.Columns.Title]);
                    });
                }

                int targetNewItemCount = 0;
                PMMCommentHistory targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.PMMCommentsHistory;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            //targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                            targetItem = null;
                            if (targetItem == null)
                            {
                                targetItem = new PMMCommentHistory();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                string Id; DataRow dr = null;
                                if (item[SPDatabaseObjects.Columns.TicketPMMIdLookup] != null)
                                {
                                    Id = ((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.TicketPMMIdLookup]).LookupValue;
                                    dr = ticketMgr.GetByTicketID(module, Id);
                                    targetItem.TicketId = Id; //Convert.ToString(dr["Id"]);
                                    if (dr != null)
                                        targetItem.PMMIdLookup = Convert.ToInt32(dr["Id"]);

                                }
                                targetItem.ProjectNoteType = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNoteType]);
                                targetItem.ProjectNote = Convert.ToString(item[SPDatabaseObjects.Columns.ProjectNote]);
                                if (item[SPDatabaseObjects.Columns.BaselineDate] != null)
                                    targetItem.BaselineDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.BaselineDate]);

                                targetItem.BaselineId = Convert.ToInt32(item[SPDatabaseObjects.Columns.BaselineNum]);

                                if (item[SPDatabaseObjects.Columns.UGITEndDate] != null)
                                    targetItem.EndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.UGITEndDate]);

                            }
                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} PMMCommentsHistory added");
            }
        }
        public override void AssetIncidentRelation()
        {
            base.AssetIncidentRelation();

            bool import = context.IsImportEnable("AssetIncidentRelation", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating AssetIncidentRelation ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                AssetIncidentRelationsManager mgr = new AssetIncidentRelationsManager(context.AppContext);
                List<AssetIncidentRelations> dbData = mgr.Load();
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<AssetIncidentRelations>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                AssetIncidentRelations targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.AssetIncidentRelations;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            DataRow _dr = null;
                            targetItem = null;
                            //targetItem = dbData.FirstOrDefault(x => x.AssetTagNumLookup == Convert.ToString(item[SPDatabaseObjects.Columns.AssetTagNumLookup]) && x.TicketId == Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]));
                            if (targetItem == null)
                            {
                                targetItem = new AssetIncidentRelations();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                ClientOM.FieldLookupValue assetLookup = item[SPDatabaseObjects.Columns.AssetTagNumLookup] as ClientOM.FieldLookupValue;
                                string id = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
                                _dr = ticketMgr.GetByTicketID(module, id);

                                targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                targetItem.ParentTicketId = Convert.ToString(item[SPDatabaseObjects.Columns.ParentTicketId]);
                                targetItem.TicketId = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                if (_dr != null)
                                    targetItem.AssetTagNumLookup = Convert.ToString(_dr[DatabaseObjects.Columns.ID]);
                            }
                            if (_dr != null)
                            {
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);
                            }
                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} AssetIncidentRelations added");
            }
        }
        public override void ApplicationServer()
        {
            base.ApplicationServer();

            bool import = context.IsImportEnable("ApplicationServer", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ApplicationServer ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                ApplicationServersManager mgr = new ApplicationServersManager(context.AppContext);
                List<ApplicationServer> dbData = mgr.Load(x => x.TenantID == context.AppContext.TenantID);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ApplicationServer>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ApplicationServer targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ApplicationServers;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList Environmentlist = context.GetMappedList(SPDatabaseObjects.Lists.Environment);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                long id = 0;
                                ClientOM.FieldLookupValue appLookup = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                if (!string.IsNullOrEmpty(appLookup.LookupValue))
                                {
                                    id = Convert.ToInt64(ticketMgr.GetIDByTitle(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, appLookup.LookupValue));
                                }
                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.APPTitleLookup == id);
                                if (targetItem == null)
                                {
                                    targetItem = new ApplicationServer();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    ClientOM.FieldLookupValue assetLookup = item[SPDatabaseObjects.Columns.AssetsTitleLookup] as ClientOM.FieldLookupValue;
                                    if (!string.IsNullOrEmpty(assetLookup.LookupValue))
                                    {
                                        long id_ = Convert.ToInt64(ticketMgr.GetIDByTitle(DatabaseObjects.Tables.Assets, DatabaseObjects.Columns.ID, assetLookup.LookupValue));
                                        if (id_ > 0)
                                            targetItem.AssetsTitleLookup = id_;
                                    }
                                    if (id > 0)
                                        targetItem.APPTitleLookup = id;
                                    long envid = 0;
                                    if (Environmentlist != null && item[SPDatabaseObjects.Columns.EnvironmentLookup] != null)
                                        envid = UGITUtility.StringToLong(Environmentlist.GetTargetID(((Microsoft.SharePoint.Client.FieldLookupValue)item[SPDatabaseObjects.Columns.EnvironmentLookup]).LookupId.ToString()));

                                    if (envid > 0)
                                        targetItem.EnvironmentLookup = envid;

                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    var _selectedServerFunctions = item[SPDatabaseObjects.Columns.ServerFunctions];
                                    if (_selectedServerFunctions != null)
                                        targetItem.ServerFunctionsChoice = string.Join(";#", (string[])_selectedServerFunctions);
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} ApplicationServer added");
            }
        }
        public override void ApplicationModules()
        {
            base.ApplicationModules();

            bool import = context.IsImportEnable("ApplicationModules", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ApplicationModules ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                ApplicationModuleManager mgr = new ApplicationModuleManager(context.AppContext);
                List<ApplicationModule> dbData = mgr.Load(x => x.TenantID == context.AppContext.TenantID);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ApplicationModule>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ApplicationModule targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ApplicationModules;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList Environmentlist = context.GetMappedList(SPDatabaseObjects.Lists.Environment);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                long id = 0;
                                ClientOM.FieldLookupValue appLookup = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                if (!string.IsNullOrEmpty(appLookup.LookupValue))
                                {
                                    id = Convert.ToInt64(ticketMgr.GetIDByTitle(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, appLookup.LookupValue));
                                }

                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));
                                if (targetItem == null)
                                {
                                    targetItem = new ApplicationModule();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {

                                    if (id > 0)
                                        targetItem.APPTitleLookup = id;
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);
                                    targetItem.ApprovalTypeChoice = Convert.ToString(item[SPDatabaseObjects.Columns.ApprovalType]);
                                    ClientOM.FieldUserValue[] _users = item[SPDatabaseObjects.Columns.TicketOwner] as ClientOM.FieldUserValue[];
                                    if (_users != null && _users.Length > 0 && userList != null)
                                        targetItem.Owner = userList.GetTargetIDs(_users.Select(x => x.LookupId.ToString()).ToList());
                                    ClientOM.FieldUserValue[] _SupportedBy = item[SPDatabaseObjects.Columns.SupportedBy] as ClientOM.FieldUserValue[];
                                    if (_SupportedBy != null && _SupportedBy.Length > 0 && userList != null)
                                        targetItem.SupportedBy = userList.GetTargetIDs(_SupportedBy.Select(x => x.LookupId.ToString()).ToList());
                                    ClientOM.FieldUserValue[] _AccessAdmin = item[SPDatabaseObjects.Columns.AccessAdmin] as ClientOM.FieldUserValue[];
                                    if (_AccessAdmin != null && _AccessAdmin.Length > 0 && userList != null)
                                        targetItem.AccessAdmin = userList.GetTargetIDs(_AccessAdmin.Select(x => x.LookupId.ToString()).ToList());
                                    ClientOM.FieldUserValue[] _Approver = item[SPDatabaseObjects.Columns.Approver] as ClientOM.FieldUserValue[];
                                    if (_Approver != null && _Approver.Length > 0 && userList != null)
                                        targetItem.Approver = userList.GetTargetIDs(_Approver.Select(x => x.LookupId.ToString()).ToList());
                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} ApplicationModules added");
            }
        }
        public override void ApplicationRole()
        {
            base.ApplicationRole();

            bool import = context.IsImportEnable("ApplicationRole", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ApplicationRole ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                ApplicationRoleManager mgr = new ApplicationRoleManager(context.AppContext);
                List<ApplicationRole> dbData = mgr.Load(x => x.TenantID == context.AppContext.TenantID);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ApplicationRole>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ApplicationRole targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ApplicationRole;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    MappedItemList Environmentlist = context.GetMappedList(SPDatabaseObjects.Lists.Environment);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, string.Empty, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                long id = 0;
                                ClientOM.FieldLookupValue appLookup = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                if (!string.IsNullOrEmpty(appLookup.LookupValue))
                                {
                                    id = Convert.ToInt64(ticketMgr.GetIDByTitle(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, appLookup.LookupValue));
                                }
                                targetItem = dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]) && x.APPTitleLookup == id);
                                if (targetItem == null)
                                {
                                    targetItem = new ApplicationRole();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    if (id > 0)
                                        targetItem.APPTitleLookup = id;
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);

                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} ApplicationRole added");
            }
        }

        public override void ApplicationAccess()
        {
            base.ApplicationAccess();

            bool import = context.IsImportEnable("ApplicationAccess", moduleName);
            if (!import)
                return;

            ULog.WriteLog($"Updating ApplicationAccess ({moduleName})");
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context.AppContext);
                UGITModule module = moduleMgr.LoadByName(moduleName);
                TicketManager ticketMgr = new TicketManager(context.AppContext);

                ApplicationRoleManager rolemgr = new ApplicationRoleManager(context.AppContext);
                List<ApplicationRole> dbroleData = rolemgr.Load(x => x.TenantID == context.AppContext.TenantID);
                ApplicationRole rolelist = null;
                ApplicationModuleManager appmodulemgr = new ApplicationModuleManager(context.AppContext);
                List<ApplicationModule> appmodulData = appmodulemgr.Load(x => x.TenantID == context.AppContext.TenantID);
                ApplicationModule appmodullist = null;
                ApplicationAccessManager mgr = new ApplicationAccessManager(context.AppContext);
                List<ApplicationAccess> dbData = mgr.Load(x => x.TenantID == context.AppContext.TenantID);
                if (deleteBeforeImport)
                {
                    try
                    {
                        mgr.Delete(dbData);
                        dbData = new List<ApplicationAccess>();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog("Problem while deleting records");
                        ULog.WriteException(ex);
                    }
                }

                int targetNewItemCount = 0;
                ApplicationAccess targetItem = null;

                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.ApplModuleRoleRelationship;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);

                    MappedItemList Environmentlist = context.GetMappedList(SPDatabaseObjects.Lists.Environment);
                    MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;
                        try
                        {
                            foreach (ClientOM.ListItem item in collection)
                            {
                                long id = 0;
                                ClientOM.FieldLookupValue appLookup = item[SPDatabaseObjects.Columns.APPTitleLookup] as ClientOM.FieldLookupValue;
                                if (!string.IsNullOrEmpty(appLookup.LookupValue))
                                {
                                    id = Convert.ToInt64(ticketMgr.GetIDByTitle(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, appLookup.LookupValue));
                                }
                                targetItem = null;
                                if (targetItem == null)
                                {
                                    targetItem = new ApplicationAccess();
                                    dbData.Add(targetItem);
                                    targetNewItemCount++;
                                }
                                if (targetItem.ID == 0 || importWithUpdate)
                                {
                                    ClientOM.FieldLookupValue appModuleLookup = item[SPDatabaseObjects.Columns.ApplicationModulesLookup] as ClientOM.FieldLookupValue;
                                    if (appModuleLookup != null)
                                    {
                                        appmodullist = appmodulData.FirstOrDefault(x => x.Title == UGITUtility.ObjectToString(appModuleLookup.LookupValue));
                                        targetItem.ApplicationModulesLookup = appmodullist.ID;
                                    }
                                    else
                                        targetItem.ApplicationModulesLookup = 0;

                                    ClientOM.FieldLookupValue appRoleLookup = item[SPDatabaseObjects.Columns.ApplicationRoleLookup] as ClientOM.FieldLookupValue;
                                    if (appRoleLookup != null)
                                    {
                                        rolelist = dbroleData.FirstOrDefault(x => x.Title == UGITUtility.ObjectToString(appRoleLookup.LookupValue));
                                        targetItem.ApplicationRoleLookup = rolelist.ID;
                                    }
                                    else
                                        targetItem.ApplicationRoleLookup = 0;
                                    if (id > 0)
                                        targetItem.APPTitleLookup = id;
                                    ClientOM.FieldUserValue assignuser = item[SPDatabaseObjects.Columns.ApplicationRoleAssign] as ClientOM.FieldUserValue;
                                    if (assignuser != null && userList != null)
                                    {
                                        targetItem.ApplicationRoleAssign = userList.GetTargetID(Convert.ToString(assignuser.LookupId));
                                        //if (string.IsNullOrEmpty(UGITUtility.ObjectToString(userList.GetTargetID(Convert.ToString(assignuser.LookupId)))))
                                        //{
                                        //    UserProfileManager obj = new UserProfileManager(context.AppContext);
                                        //    UserProfile objuser = obj.GetUserByBothUserNameandDisplayName(Convert.ToString(assignuser.LookupValue));
                                        //    targetItem.ApplicationRoleAssign = objuser.Id;
                                        //}
                                    }
                                    targetItem.Title = Convert.ToString(item[SPDatabaseObjects.Columns.Title]);
                                    targetItem.Description = Convert.ToString(item[SPDatabaseObjects.Columns.UGITDescription]);


                                }
                                if (targetItem.ID > 0)
                                    mgr.Update(targetItem);
                                else
                                    mgr.Insert(targetItem);

                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    } while (position != null);
                }
                ULog.WriteLog($"{targetNewItemCount} ApplicationAccess added");
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
                MappedItemList userList = context.GetMappedList(SPDatabaseObjects.Lists.UserInformationList);
                using (ClientOM.ClientContext spContext = ContextHelper.CreateContext(context.SPSite))
                {
                    string listName = SPDatabaseObjects.Lists.TicketEvents;
                    ClientOM.List list = spContext.Web.Lists.GetByTitle(listName);
                    MappedItemList locaionlist = context.GetMappedList(listName);
                    ClientOM.ListItemCollectionPosition position = new ClientOM.ListItemCollectionPosition();
                    string title = string.Empty;
                    do
                    {
                        ClientOM.ListItemCollection collection = ContextHelper.GetDataFromList(spContext, list, moduleName, position);
                        position = collection.ListItemCollectionPosition;

                        foreach (ClientOM.ListItem item in collection)
                        {
                            targetItem = null;// dbData.FirstOrDefault(x => x.Title == Convert.ToString(item[SPDatabaseObjects.Columns.Title]));

                            if (Convert.ToString(item[SPDatabaseObjects.Columns.ModuleName]) != moduleName)
                                continue;


                            if (targetItem == null)
                            {
                                targetItem = new TicketEvents();
                                dbData.Add(targetItem);
                                targetNewItemCount++;
                            }
                            if (targetItem.ID == 0 || importWithUpdate)
                            {
                                targetItem.Ticketid = Convert.ToString(item[SPDatabaseObjects.Columns.TicketId]);
                                targetItem.Status = Convert.ToString(item[SPDatabaseObjects.Columns.TicketStatus]);
                                targetItem.StageStep = Convert.ToInt32(item[SPDatabaseObjects.Columns.StageStep]);
                                targetItem.ModuleName = moduleName;
                                targetItem.TicketEventType = Convert.ToString(item[SPDatabaseObjects.Columns.TicketEventType]);
                                targetItem.EventTime = Convert.ToDateTime(item[SPDatabaseObjects.Columns.TicketEventTime]);
                                ClientOM.FieldUserValue TicketEventBy = item[SPDatabaseObjects.Columns.TicketEventBy] as ClientOM.FieldUserValue;
                                if (TicketEventBy != null && userList != null)
                                {
                                    targetItem.TicketEventBy = Convert.ToString(TicketEventBy.LookupValue);
                                    targetItem.CreatedByUser = userList.GetTargetID(Convert.ToString(TicketEventBy.LookupId));
                                }
                                targetItem.Automatic = Convert.ToBoolean(item[SPDatabaseObjects.Columns.Automatic]);
                                targetItem.Comment = Convert.ToString(item[SPDatabaseObjects.Columns.UGITComment]);
                                //targetItem.AffectedUsers = Convert.ToString(item[SPDatabaseObjects.Columns.AffectedUsers]);
                                targetItem.Created = Convert.ToDateTime(item[SPDatabaseObjects.Columns.Created]);

                                ClientOM.FieldUserValue[] users = item[SPDatabaseObjects.Columns.AffectedUsers] as ClientOM.FieldUserValue[];
                                if (userList != null && users != null)
                                {
                                    targetItem.AffectedUsers = userList.GetTargetIDs(users.Select(x => x.LookupId.ToString()).ToList());
                                }

                                targetItem.PlannedEndDate = Convert.ToDateTime(item[SPDatabaseObjects.Columns.PlannedEndDate]);
                                targetItem.EventReason = Convert.ToString(item[SPDatabaseObjects.Columns.EventReason]);
                                targetItem.Title = string.Format("{0} {1}", targetItem.Ticketid, targetItem.TicketEventType);
                                if (!string.IsNullOrEmpty(Convert.ToString(item[SPDatabaseObjects.Columns.SubTaskTitle])))
                                    targetItem.SubTaskTitle = Convert.ToString(item[SPDatabaseObjects.Columns.SubTaskTitle]);

                                if (!string.IsNullOrEmpty(Convert.ToString(item[SPDatabaseObjects.Columns.SubTaskId])))
                                    targetItem.SubTaskId = Convert.ToString(item[SPDatabaseObjects.Columns.SubTaskId]);
                                targetItem.TenantID = Convert.ToString(context.Tenant.TenantID);


                            }

                            if (targetItem.ID > 0)
                                mgr.Update(targetItem);
                            else
                                mgr.Insert(targetItem);

                        }
                    } while (position != null);

                }

                ULog.WriteLog($"{targetNewItemCount} TicketEvents added");
            }
        }
    }

}
