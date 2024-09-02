using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.Core
{
    public class LifeCycleStage : ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string Action { get; set; }
        public List<string> ActionUser { get; set; }
        public string UserPrompt { get; set; }
        public string UserWorkflowStatus { get; set; }
        public LifeCycleStage ApprovedStage { get; set; }
        public LifeCycleStage RejectStage { get; set; }
        public LifeCycleStage ReturnStage { get; set; }

        public string SkipOnCondition { get; set; }
        public string StageApproveButtonName { get; set; }
        public string StageRejectedButtonName { get; set; }
        public string StageReturnButtonName { get; set; }

        public bool StageAllApprovalsRequired { get; set; }
        public string ShortStageTitle { get; set; }
        public string ApproveActionDescription { get; set; }
        public string RejectActionDescription { get; set; }
        public string ReturnActionDescription { get; set; }
        public string StageType { get; set; }
        private int _lifeCycleID;
        private string _lifeCycle;
        public bool EnableCustomReturn { get; set; }
        public int SelectedTab { get; set; }

        public string CustomProperties { get; set; }
        public bool? Prop_QuickClose { get; set; }
        public bool? Prop_SelfAssign { get; set; }
        public bool? Prop_DoNotWaitForActionUser { get; set; }
        public bool? Prop_AutoApprove { get; set; }
        public bool? Prop_BaseLine { get; set; }
        public string Prop_CustomIconApprove { get; set; }
        public string Prop_CustomIconReject { get; set; }
        public string Prop_CustomIconReturn { get; set; }
        public bool? Prop_NewNotification { get; set; }
        public bool? Prop_UpdateNotification { get; set; }
        public bool? Prop_OnResolveNotification { get; set; }
        public bool? Prop_ResolvedNotification { get; set; }
        public bool? Prop_CheckAssigneeToAllTask { get; set; }
        /// <summary>
        /// Used in NPR only
        /// </summary>
        public bool? Prop_ReadyToImport { get; set; }
        public bool? Prop_PMOReview { get; set; }
        public bool? Prop_ITGReview { get; set; }
        public bool? Prop_ITSCReview { get; set; }

        //new property..
        public string ApproveIcon { get; set; }
        public string ReturnIcon { get; set; }
        public string RejectIcon { get; set; }
        public string ApproveActionTooltip { get; set; }
        public string RejectActionTooltip { get; set; }
        public string ReturnActionToolip { get; set; }

        public int StageCapacityNormal { get; set; }
        public int StageCapacityMax { get; set; }

        public bool AllowReassignFromList { get; set; }

        public int LifeCycleID
        {
            get
            {
                return _lifeCycleID;
            }

            set
            {
                _lifeCycleID = value;
            }
        }
        public string LifeCycle
        {
            get
            {
                return _lifeCycle;
            }
            set
            {
                _lifeCycle = value;
            }
        }

        public LifeCycleStage()
        {
            Description = string.Empty;
            Name = string.Empty;
        }



        public object Clone()
        {
            LifeCycleStage objClone = new LifeCycleStage();
            objClone.ID = this.ID;
            objClone.Name = this.Name;
            objClone.Step = this.Step;
            objClone.Description = this.Description;
            objClone.Weight = this.Weight;
            objClone.Action = this.Action;
            objClone.ActionUser = this.ActionUser;
            objClone.UserPrompt = this.UserPrompt;
            objClone.UserWorkflowStatus = this.UserWorkflowStatus;
            objClone.ApprovedStage = this.ApprovedStage;
            objClone.RejectStage = this.RejectStage;
            objClone.ReturnStage = this.ReturnStage;
            objClone.SkipOnCondition = this.SkipOnCondition;
            objClone.StageApproveButtonName = this.StageApproveButtonName;
            objClone.StageRejectedButtonName = this.StageRejectedButtonName;
            objClone.StageReturnButtonName = this.StageReturnButtonName;
            objClone.StageAllApprovalsRequired = this.StageAllApprovalsRequired;
            objClone.ShortStageTitle = this.ShortStageTitle;
            objClone.ApproveActionDescription = this.ApproveActionDescription;
            objClone.RejectActionDescription = this.RejectActionDescription;
            objClone.ReturnActionDescription = this.ReturnActionDescription;
            objClone.StageType = this.StageType;
            objClone._lifeCycleID = this._lifeCycleID;
            objClone._lifeCycle = this._lifeCycle;
            objClone.CustomProperties = this.CustomProperties;
            objClone.Prop_QuickClose = this.Prop_QuickClose;
            objClone.Prop_SelfAssign = this.Prop_SelfAssign;
            objClone.Prop_DoNotWaitForActionUser = this.Prop_DoNotWaitForActionUser;
            objClone.Prop_AutoApprove = this.Prop_AutoApprove;
            objClone.Prop_BaseLine = this.Prop_BaseLine;
            objClone.Prop_CustomIconApprove = this.Prop_CustomIconApprove;
            objClone.Prop_CustomIconReject = this.Prop_CustomIconReject;
            objClone.Prop_CustomIconReturn = this.Prop_CustomIconReturn;
            objClone.Prop_NewNotification = this.Prop_NewNotification;
            objClone.Prop_UpdateNotification = this.Prop_UpdateNotification;
            objClone.Prop_OnResolveNotification = this.Prop_OnResolveNotification;
            objClone.Prop_ResolvedNotification = this.Prop_ResolvedNotification;
            objClone.Prop_CheckAssigneeToAllTask = this.Prop_CheckAssigneeToAllTask;
            objClone.Prop_ReadyToImport = this.Prop_ReadyToImport;
            objClone.Prop_ITGReview = this.Prop_ITGReview;
            objClone.Prop_ITSCReview = this.Prop_ITSCReview;
            objClone.Prop_PMOReview = this.Prop_PMOReview;

            //new lines..
            objClone.ApproveIcon = this.ApproveIcon;
            objClone.ReturnIcon = this.ReturnIcon;
            objClone.RejectIcon = this.RejectIcon;
            objClone.StageCapacityNormal = this.StageCapacityNormal;
            objClone.StageCapacityMax = this.StageCapacityMax;

            objClone.AllowReassignFromList = this.AllowReassignFromList;

            objClone.AutoApproveOnStageTasks = this.AutoApproveOnStageTasks;

            return objClone;

        }
    }
}
