using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.ModuleStages)]
   [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, AsReferenceDefault =true)]
    public class LifeCycleStage:DBBaseEntity
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int StageStep { get; set; }
       
        public string Action { get; set; }
        public string ActionUser { get; set; }

        public string NavigationUrl { get; set; }
        public string NavigationType { get; set; }

        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string DataEditors { get; set; } = string.Empty;
        public string UserPrompt { get; set; }
        public string UserWorkflowStatus { get; set; }
        [NotMapped]
       [JsonProperty(ReferenceLoopHandling =ReferenceLoopHandling.Ignore, ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
       
        public LifeCycleStage ApprovedStage { get; set; }
        [NotMapped]
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public LifeCycleStage RejectStage { get; set; }
        [NotMapped]
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore,ItemReferenceLoopHandling =ReferenceLoopHandling.Ignore)]
   
        public LifeCycleStage ReturnStage { get; set; }

        public string SkipOnCondition { get; set; }
        public string StageApproveButtonName { get; set; }
        public string StageRejectedButtonName { get; set; }
        public string StageReturnButtonName { get; set; }

        public bool StageAllApprovalsRequired { get; set; }
        public string ShortStageTitle { get; set; }
        public string ApproveActionDescription { get; set; }
        public string ApproveButtonTooltip { get; set; }
        public string RejectActionDescription { get; set; }
        public string ReturnActionDescription { get; set; }
        public string RejectButtonTooltip { get; set; }
        public string ReturnButtonTooltip { get; set; }
        public string StageTitle { get; set; }
        [NotMapped]
        public string Title { get; set; }
        public int StageApprovedStatus { get; set; }
        public int StageReturnStatus { get; set; }
        public int StageRejectedStatus { get; set; }
        [NotMapped]
        public int? StageTypeLookup { get; set; }
        
        public double StageWeight { get; set; }
        //private bool IsDeleted { get; set; }
        public bool ShowBaselineButtons { get; set; }
        private LookupValue _lifeCycle;
        public string ModuleNameLookup { get; set; }
        public bool EnableCustomReturn { get; set; }
        [NotMapped]
        public int SelectedTab { get; set; }
        public string StageTypeChoice { get; set; }

        public string CustomProperties { get; set; }
        [NotMapped]
        public bool? Prop_QuickClose { get; set; }
        [NotMapped]
        public bool? Prop_SelfAssign { get; set; }
        [NotMapped]
        public bool? Prop_DoNotWaitForActionUser { get; set; }
        public bool? DisableAutoApprove { get; set; }
        [NotMapped]
        public bool? Prop_AutoApprove
        {
            get;set;            
        }
        [NotMapped]
        public bool? Prop_BaseLine { get; set; }
        [NotMapped]
        public string Prop_CustomIconApprove { get; set; }
        [NotMapped]
        public string Prop_CustomIconReject { get; set; }
        [NotMapped]
        public string Prop_CustomIconReturn { get; set; }
        [NotMapped]
        public bool? Prop_NewNotification { get; set; }
        [NotMapped]
        public bool? Prop_UpdateNotification { get; set; }
        [NotMapped]
        public bool? Prop_OnResolveNotification { get; set; }
        [NotMapped]
        public bool? Prop_ResolvedNotification { get; set; }
        [NotMapped]
        public bool? Prop_CheckAssigneeToAllTask { get; set; }
        
        /// <summary>
        /// Used in NPR only
        /// </summary>
        [NotMapped]
        public bool? Prop_ReadyToImport { get; set; }
        [NotMapped]
        public bool? Prop_PMOReview { get; set; }
        [NotMapped]
        public bool? Prop_ITGReview { get; set; }
        [NotMapped]
        public bool? Prop_ITSCReview { get; set; }

        //new property..
        public string ApproveIcon { get; set; }
        public string ReturnIcon { get; set; }
        public string RejectIcon { get; set; }
        [NotMapped]
        public string ApproveActionTooltip { get; set; }
        [NotMapped]
        public string RejectActionTooltip { get; set; }
        [NotMapped]
        public string ReturnActionToolip { get; set; }

        public int StageCapacityNormal { get; set; }
        public int StageCapacityMax { get; set; }
        public int SelectedTabNumber { get; set; }
        public bool AllowReassignFromList { get; set; }
        [NotMapped]
        public int LifeCycleID { get; set; }
        [NotMapped]
        public LookupValue LifeCycle
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

        public long? LifeCycleName { get; set; }
        [NotMapped]
        public bool Prop_AllowEmailApproval { get; set; }
        public LifeCycleStage()
        {
            StageStep = 0;
        }
        public bool AutoApproveOnStageTasks { get; set; }
        public object Clone()
        {
            LifeCycleStage objClone = new LifeCycleStage();
            objClone.ID = this.ID;
            objClone.Name = this.Name;
            objClone.StageStep = this.StageStep;
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
            objClone.StageTypeChoice = this.StageTypeChoice;
            objClone.LifeCycleID = this.LifeCycleID;
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
            objClone.Prop_AllowEmailApproval = this.Prop_AllowEmailApproval;
            //objClone.Prop_EnableAPEmailButton = this.Prop_EnableAPEmailButton;
            objClone.ApproveIcon = this.ApproveIcon;
            objClone.ReturnIcon = this.ReturnIcon;
            objClone.RejectIcon = this.RejectIcon;
            objClone.StageWeight = this.StageWeight;
            objClone.StageCapacityNormal = this.StageCapacityNormal;
            objClone.StageCapacityMax = this.StageCapacityMax;
            objClone.StageTitle = this.StageTitle;
            objClone.AllowReassignFromList = this.AllowReassignFromList;
            objClone.AutoApproveOnStageTasks = this.AutoApproveOnStageTasks;

            return objClone;
        }
    }
}
