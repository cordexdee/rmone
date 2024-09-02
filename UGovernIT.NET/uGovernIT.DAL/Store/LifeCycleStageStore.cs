using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class LifeCycleStageStore : StoreBase<LifeCycleStage>, ILifeCycleStageStore
    {
        public LifeCycleStageStore(CustomDbContext context) : base(context)
        {

        }
        public List<LifeCycleStage> LoadStage(List<LifeCycleStage> items)
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
    }
    public interface ILifeCycleStageStore : IStore<LifeCycleStage>
    {
        List<LifeCycleStage> LoadStage(List<LifeCycleStage> items);
    }
}
