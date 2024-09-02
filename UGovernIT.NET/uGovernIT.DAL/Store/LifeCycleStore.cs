using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class LifeCycleStore : StoreBase<LifeCycle> , ILifeCycleStore
    {
        public LifeCycleStore(CustomDbContext context) : base(context)
        {

        }
        public List<LifeCycle> LoadByModule(string moduleName)
        {
            StoreBase<LifeCycleStage> store = new StoreBase<LifeCycleStage>(this.context);
            List<LifeCycleStage> objLifeCycleStages = new List<LifeCycleStage>();
            objLifeCycleStages = store.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));   //lifeCycleStages.GetData().ToList();
            StoreBase<LifeCycle> modulelifecyclestore = new DAL.StoreBase<LifeCycle>(this.context);
            List<LifeCycle> modulelifecylelist = new List<LifeCycle>();
            if(!moduleName.Equals("PMM", StringComparison.CurrentCultureIgnoreCase))
                modulelifecylelist.Add(new LifeCycle() { Name = null, Description = "Default", ID = 0, ModuleNameLookup = moduleName });
            modulelifecylelist.AddRange(modulelifecyclestore.Load(x => x.ModuleNameLookup == moduleName).ToList());
            int i = 1;

            foreach (LifeCycle item in modulelifecylelist)
            {
                if (objLifeCycleStages == null)
                    continue;
                if (item.ID == 0)
                    item.Stages = LoadStage(objLifeCycleStages.Where(x => !x.LifeCycleName.HasValue).OrderBy(x => x.StageStep).ToList());
                else
                    item.Stages = LoadStage(objLifeCycleStages.Where(x => x.LifeCycleName.HasValue && x.LifeCycleName.Value == item.ID).OrderBy(x => x.StageStep).ToList());

                //item.ItemOrder = i;
                i++;
            }
            return modulelifecylelist;
        }
        private List<LifeCycleStage> LoadStage(List<LifeCycleStage> items)
        {
            foreach (LifeCycleStage item in items)
            {
                item.ApprovedStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageApprovedStatus) == x.StageStep);
                item.ReturnStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageReturnStatus) == x.StageStep);
                item.RejectStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageRejectedStatus) == x.StageStep);
                LoadStageItem(item);
            }
            return items;
        }     

        private LifeCycleStage LoadStageItem(LifeCycleStage item)
        {
            Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(item.CustomProperties, Constants.Separator);

            if (customProperties.ContainsKey(CustomProperties.BaseLine))
                item.Prop_BaseLine = UGITUtility.StringToBoolean(customProperties[CustomProperties.BaseLine]);

            if (customProperties.ContainsKey(CustomProperties.QuickClose))
                item.Prop_QuickClose = UGITUtility.StringToBoolean(customProperties[CustomProperties.QuickClose]);

            if (customProperties.ContainsKey(CustomProperties.SelfAssign))
                item.Prop_SelfAssign = UGITUtility.StringToBoolean(customProperties[CustomProperties.SelfAssign]);

            if (customProperties.ContainsKey(CustomProperties.DoNotWaitForActionUser))
                item.Prop_DoNotWaitForActionUser = UGITUtility.StringToBoolean(customProperties[CustomProperties.DoNotWaitForActionUser]);

            if (customProperties.ContainsKey(CustomProperties.QuickClose))
                item.Prop_QuickClose = UGITUtility.StringToBoolean(customProperties[CustomProperties.QuickClose]);

            if (customProperties.ContainsKey(CustomProperties.AutoApprove))
                item.Prop_AutoApprove = UGITUtility.StringToBoolean(customProperties[CustomProperties.AutoApprove]);

            if (customProperties.ContainsKey(CustomProperties.UpdateNotification))
                item.Prop_UpdateNotification = UGITUtility.StringToBoolean(customProperties[CustomProperties.UpdateNotification]);

            if (customProperties.ContainsKey(CustomProperties.OnResolveNotification))
                item.Prop_OnResolveNotification = UGITUtility.StringToBoolean(customProperties[CustomProperties.OnResolveNotification]);

            if (customProperties.ContainsKey(CustomProperties.ResolvedNotification))
                item.Prop_ResolvedNotification = UGITUtility.StringToBoolean(customProperties[CustomProperties.ResolvedNotification]);

            if (customProperties.ContainsKey(CustomProperties.NewNotification))
                item.Prop_NewNotification = UGITUtility.StringToBoolean(customProperties[CustomProperties.NewNotification]);

            if (customProperties.ContainsKey(CustomProperties.CustomIconApprove))
                item.Prop_CustomIconApprove = customProperties[CustomProperties.CustomIconApprove];

            if (customProperties.ContainsKey(CustomProperties.CustomIconReject))
                item.Prop_CustomIconReject = customProperties[CustomProperties.CustomIconReject];

            if (customProperties.ContainsKey(CustomProperties.CustomIconReturn))
                item.Prop_CustomIconReturn = customProperties[CustomProperties.CustomIconReturn];

            if (customProperties.ContainsKey(CustomProperties.CheckAssigneeToAllTask))
                item.Prop_CheckAssigneeToAllTask = UGITUtility.StringToBoolean(customProperties[CustomProperties.CheckAssigneeToAllTask]);

            if (customProperties.ContainsKey(CustomProperties.ReadyToImport))
                item.Prop_ReadyToImport = UGITUtility.StringToBoolean(customProperties[CustomProperties.ReadyToImport]);

            if (customProperties.ContainsKey(CustomProperties.ITSCReview))
                item.Prop_ITSCReview = UGITUtility.StringToBoolean(customProperties[CustomProperties.ITSCReview]);

            if (customProperties.ContainsKey(CustomProperties.ITGReview))
                item.Prop_ITGReview = UGITUtility.StringToBoolean(customProperties[CustomProperties.ITGReview]);

            if (customProperties.ContainsKey(CustomProperties.PMOReview))
                item.Prop_PMOReview = UGITUtility.StringToBoolean(customProperties[CustomProperties.PMOReview]);

            if (customProperties.ContainsKey(CustomProperties.AllowEmailApproval))
                item.Prop_AllowEmailApproval = UGITUtility.StringToBoolean(customProperties[CustomProperties.AllowEmailApproval]);

            return item;
        }

        public List<LifeCycle> LoadLifeCyleForPMM()
        {
            StoreBase<LifeCycleStage> store = new StoreBase<LifeCycleStage>(this.context);
            List<LifeCycleStage> objLifeCycleStages = new List<LifeCycleStage>();
            objLifeCycleStages = store.Load(string.Format("Where ModuleNameLookup='{0}'", ModuleNames.PMM));   //lifeCycleStages.GetData().ToList();
            StoreBase<LifeCycle> modulelifecyclestore = new DAL.StoreBase<LifeCycle>(this.context);
            List<LifeCycle> modulelifecylelist = new List<LifeCycle>();
            modulelifecylelist.AddRange(modulelifecyclestore.Load().Where(x => x.ModuleNameLookup == ModuleNames.PMM).ToList());
            int i = 1;

            foreach (LifeCycle item in modulelifecylelist)
            {
                if (item.ID == 0)
                    item.Stages = LoadStage(objLifeCycleStages.Where(x => !x.LifeCycleName.HasValue).OrderBy(x => x.StageStep).ToList());
                else
                    item.Stages = LoadStage(objLifeCycleStages.Where(x => x.LifeCycleName.HasValue && x.LifeCycleName.Value == item.ID).OrderBy(x => x.StageStep).ToList());

                //item.ItemOrder = i;
                i++;
            }
            return modulelifecylelist;
        }
    }

    public interface ILifeCycleStore : IStore<LifeCycle>
    {
        List<LifeCycle> LoadByModule(string moduleName);
    }
}
