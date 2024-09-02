using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class CRMProjectAllocationViewNew : System.Web.UI.UserControl
    {
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        protected DateTime StartDate { get; set; }
        protected DateTime EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public bool ScheduleDateOverLap { get; set; }
        public string ConstructionStartDateString { get; set; }
        public string ConstructionEndDateString { get; set; }
        public string PreConStartDateString { get; set; }
        public string PreConEndDateString { get; set; }
        public string CloseOutStartDateString { get; set; }
        public string CloseOutEndDateString { get; set; }
        public string PreConStartDate { get; set; }
        public string PreConEndDate { get; set; }

        public DateTime PreConStartDateType { get; set; }
        public DateTime PreConEndDateType { get; set; }

        public string Customer { get; set; }
        public string Sector { get; set; }

        public string HidePrecon { get; set; }
        public string HideCloseout { get; set; }
        public string HideConst { get; set; }

        public string EnableMultiAllocation { get; set; }
        public string EnableAllocation { get; set; }
        public string SetAlignment { get; set; }

        public bool EnableGanttProfilePic { get; set; }
        public bool ShowTotalAllocationsInSearch { get; set; }
        public bool EnforcePhaseConstraints { get; set; }
        public bool PhaseSummaryView { get; set; }
        public bool EnableLockUnlockAllocation { get; set; }
        public string CompanyLookup = string.Empty;

        public string ProjectComplexity = string.Empty;

        public string RequestType = string.Empty;
        public string TagMultiLookup = string.Empty;
        public bool IsAllocationTypeHard_Soft { get; set; }
        public bool HideAllocationTemplate { get; set; }
        public bool isTicketClosed { get; set; }

        DataRow spListItem;
        private string formTitle = "Project Allocation";
        public string isclosebtnrequired { get; set; }
        //private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketId={2}";
        protected string importAllocationTemplateUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importallocationtemplate&ProjectID={0}");
        protected string buildProfileUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=buildprofile&ProjectID={0}");
        protected string compareProfileUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=compareuserprofile");
        public string NewProjectSummaryPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=projectsummary");
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&WorkItem={2}&SelectedUsersList={3}&Type={4}&IsRedirectFromTeamAgent=true";
        public string allocationGanttUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=customprojectteamgantt");
        protected string experienceTagProjectsURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=cutomfilterTicket");
        public string ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/module");
        public string rmoneControllerUrl = UGITUtility.GetAbsoluteURL("/api/RMOne/");
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        public string TicketUrl { get; set; }
        public string MultiAddUrl { get; set; }
        private ApplicationContext _context = null;
        LifeCycleStageManager objLifeCycleStageManager = null;
        RequestTypeManager requestTypeManager = null;
        public ConfigurationVariableManager configManager = null;
        protected int closeoutperiod = 0;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }
        public bool HasAccessToAddTags
        {
            get
            {
                return uHelper.HasAccessToAddExpTags(ApplicationContext);
            }
        }
        public bool AllowGroupFilterOnExpTags
        {
            get
            {
                return uHelper.IsExperienceTagAllowGroupFilter(ApplicationContext);
            }
        }

        public string CMICNumber { get; set; }
        protected override void OnInit(EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            requestTypeManager = new RequestTypeManager(ApplicationContext);
            configManager = new ConfigurationVariableManager(ApplicationContext);
            int ticketStageStep = 1;
            int awardStageStep = 1000; // Changed to 1000 as mostly ticketStageStep may not go beyond this No., & a module with 'awardstage' will change this step No. in below code.
            string title = string.Format("{0} - New Item", formTitle);
            string module = string.Empty;
            DataRow spCompanyItem = null;
            closeoutperiod = uHelper.getCloseoutperiod(HttpContext.Current.GetManagerContext());
            EnableGanttProfilePic = configManager.GetValueAsBool(ConfigConstants.EnableGanttProfilePic);
            ShowTotalAllocationsInSearch = configManager.GetValueAsBool(ConfigConstants.ShowTotalAllocationsInSearch);
            EnforcePhaseConstraints = configManager.GetValueAsBool(ConfigConstants.EnforcePhaseConstraints);
            PhaseSummaryView = configManager.GetValueAsBool(ConfigConstants.PhaseSummaryView);
            EnableLockUnlockAllocation = configManager.GetValueAsBool(ConfigConstants.EnableLockUnlockAllocation);
            HideAllocationTemplate = uHelper.HideAllocationTemplate(HttpContext.Current.GetManagerContext());

            ModuleViewManager moduleViewManager = new ModuleViewManager(ApplicationContext);
            UGITModule uGITModule = moduleViewManager.LoadByName(uHelper.getModuleNameByTicketId(TicketID));
            IsAllocationTypeHard_Soft = uGITModule.IsAllocationTypeHard_Soft;

            if (!string.IsNullOrEmpty(TicketID))
            {
                isclosebtnrequired = UGITUtility.ObjectToString(Request["isdlg"]);
                objLifeCycleStageManager = new LifeCycleStageManager(ApplicationContext);
                spListItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(TicketID), TicketID);
                var stage = objLifeCycleStageManager.Load(x => x.ModuleNameLookup == ModuleName && x.CustomProperties.Contains("awardstage"));
                if (stage != null && stage.Count > 0)
                    awardStageStep = stage.FirstOrDefault().StageStep;

                if (spListItem != null)
                {
                    if (ModuleName == "CPR" && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.ERPJobID) && spListItem[DatabaseObjects.Columns.ERPJobID] != null)
                    {
                        CMICNumber = !string.IsNullOrWhiteSpace(Convert.ToString(spListItem[DatabaseObjects.Columns.ERPJobID]))
                            ? Convert.ToString(spListItem[DatabaseObjects.Columns.ERPJobID])
                            : TicketID;
                    }
                    else if (ModuleName == "OPM" && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.ERPJobIDNC) && spListItem[DatabaseObjects.Columns.ERPJobIDNC] != null)
                    {
                        CMICNumber = !string.IsNullOrWhiteSpace(Convert.ToString(spListItem[DatabaseObjects.Columns.ERPJobIDNC])) 
                            ? Convert.ToString(spListItem[DatabaseObjects.Columns.ERPJobIDNC])
                            : TicketID;
                    }

                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.Closed))
                    {
                        isTicketClosed = UGITUtility.StringToBoolean(spListItem[DatabaseObjects.Columns.Closed]);
                    }

                    if (uGITModule?.StaticModulePagePath != null)
                    {
                        string text = string.Format("{0}: {1}", CMICNumber, UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.Title]));
                        TicketUrl = string.Format("openTicketDialog('{0}','TicketId={1}','{2}','{4}','{5}', 0, '{3}')", uGITModule.StaticModulePagePath, TicketID, text, "", 95, 95);
                    }
                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CRMProjectID) && spListItem[DatabaseObjects.Columns.CRMProjectID] != null)
                        title = string.Format("{0} {1}", Convert.ToString(spListItem[DatabaseObjects.Columns.CRMProjectID]), Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                    else
                        title = string.Format("{0}", Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));

                    ticketStageStep = UGITUtility.StringToInt(spListItem[DatabaseObjects.Columns.StageStep]);

                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.BCCISector))
                        Sector = UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.BCCISector]);

                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CRMProjectComplexity))
                        ProjectComplexity = UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.CRMProjectComplexity]);

                    if (!string.IsNullOrEmpty(ProjectComplexity))
                    {
                        ProjectComplexity = $"{ProjectComplexity}+";
                    }

                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        long Id = 0;
                        UGITUtility.IsNumber(UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.TicketRequestTypeLookup]), out Id);
                        ModuleRequestType reqType = requestTypeManager.LoadByID(Id);
                        if (reqType != null)
                        {
                            RequestType = reqType.RequestType;
                        }
                    }

                    if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CRMCompanyLookup))
                    {
                        CompanyLookup = UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.CRMCompanyLookup]);

                        if (!string.IsNullOrEmpty(CompanyLookup))
                        {
                            spCompanyItem = Ticket.GetCurrentTicket(ApplicationContext, ModuleNames.COM, CompanyLookup);
                            Customer = UGITUtility.ObjectToString(spCompanyItem[DatabaseObjects.Columns.Title]);
                        }
                    }
                    if (ModuleName == "CPR" && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.TagMultiLookup))
                    {
                        TagMultiLookup = UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.TagMultiLookup]);
                    }
                }

                MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "multiallocationjs", "Add Multiple Allocations", UGITUtility.ObjectToString(TicketID), "", ModuleName));
            }



            importAllocationTemplateUrl = string.Format(importAllocationTemplateUrl, TicketID);
            buildProfileUrl = string.Format(buildProfileUrl, TicketID);

            ctrSaveAllocationAsTemplate.ModuleName = ModuleName;
            ctrSaveAllocationAsTemplate.ProjectID = TicketID;
            ctrSaveAllocationAsTemplate.PopupID = pcSaveAsTemplate.ClientID;

            //CPR New Date logic
            if (ModuleName == "CPR" || ModuleName == "CNS" || ModuleName == "OPM")
            {
                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart))))
                    {
                        StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        ConstructionStartDateString = StartDate.ToString("MMM d, yyyy");
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))))
                    {
                        EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        ConstructionEndDateString = EndDate.ToString("MMM d, yyyy");
                    }
                    if (string.IsNullOrEmpty(ConstructionStartDateString) && string.IsNullOrEmpty(ConstructionEndDateString))
                        HideConst = "Hidden";
                }
                else
                    HideConst = "Hidden";

                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.PreconStartDate) && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.PreconEndDate))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.PreconStartDate))))
                    {
                        PreConStartDateType = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconStartDate]);
                        PreConStartDateString = PreConStartDateType.ToString("MMM d, yyyy");
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.PreconEndDate))))
                    {
                        PreConEndDateType = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconEndDate]);
                        PreConEndDateString = PreConEndDateType.ToString("MMM d, yyyy");
                    }
                    if (string.IsNullOrEmpty(PreConStartDateString) && string.IsNullOrEmpty(PreConEndDateString))
                        HidePrecon = "Hidden";
                }
                else
                    HidePrecon = "Hidden";

                if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue && PreConEndDateType != DateTime.MinValue && PreConStartDateType != DateTime.MinValue)
                {
                    if (StartDate <= PreConEndDateType && EndDate >= PreConStartDateType)
                    {
                        ScheduleDateOverLap = true;
                    }
                }
                
                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CloseoutStartDate))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.CloseoutStartDate))))
                    {
                        CloseOutStartDateString = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.CloseoutStartDate]).ToString("MMM d, yyyy");
                        CloseOutEndDateString = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.CloseoutStartDate]).AddWorkingDays(uHelper.getCloseoutperiod(_context)).ToString("MMM d, yyyy");
                    }
                }

                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CloseoutDate))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.CloseoutDate))))
                        CloseOutEndDateString = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.CloseoutDate]).ToString("MMM d, yyyy");
                }

                if (string.IsNullOrEmpty(CloseOutStartDateString) && string.IsNullOrEmpty(CloseOutEndDateString))
                    HideCloseout = "Hidden";
                EnableAllocation = "Hidden";
            }
            else
            {
                HideConst = "Hidden";
                HidePrecon = "Hidden";
                HideCloseout = "Hidden";
                EnableMultiAllocation = "Hidden";
                SetAlignment = "style=float:right";

                /*
             CPR Resource Allocation Date Defaults: (as per mail Dt. 01 Apr 2021)
                -  If Prior to “Awarded” Stage, use Precon Start and End dates, unless blank, then use Construction Start and Construction End
                - If Awarded or beyond, use Construction Start and End dates
             */

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.TicketTargetStartDate))
                {
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.TicketTargetStartDate]);
                }

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart))
                {
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                }

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.PreconStartDate) && (ticketStageStep < awardStageStep))
                {
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconStartDate]);
                }

                if (StartDate == DateTime.MinValue && UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.ContractStartDate))
                {
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.ContractStartDate]);
                }

                if (StartDate == DateTime.MinValue && UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.TicketCreationDate))
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.TicketCreationDate]);

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.PreconEndDate) && (ticketStageStep < awardStageStep))
                {
                    EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconEndDate]);
                }

                if (EndDate == DateTime.MinValue && UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.ContractExpirationDate))
                {
                    EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.ContractExpirationDate]);
                }

                if (EndDate == DateTime.MinValue && UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.TicketDesiredCompletionDate))
                    EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.TicketDesiredCompletionDate]);


                //if (EndDate < StartDate)
                //    EndDate = StartDate;
                //if (EndDate == DateTime.MinValue)
                //    EndDate = StartDate;
            }




        }
    }
}