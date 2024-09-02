using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class SimilarProjectPreviewAllocations : System.Web.UI.UserControl
    {
        public string ActualProjectID { get; set; }
        public string ticketID { get; set; }
        public string ModuleName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public DateTime PreConStartDateType { get; set; }
        public DateTime PreConEndDateType { get; set; }
        public string PreConStartDate { get; set; }
        public string PreConEndDate { get; set; }
        public string Customer { get; set; }
        public string Sector { get; set; }

        public string HidePrecon { get; set; }
        public string HideConst { get; set; }
        public bool EnableGanttProfilePic { get; set; }

        public string CompanyLookup = string.Empty;

        public string ProjectComplexity = string.Empty;

        public string RequestType = string.Empty;

        DataRow spListItem;
        private string formTitle = "Project Allocation";

        //private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketId={2}";
        protected string importAllocationTemplateUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importallocationtemplate&ProjectID={0}");
        protected string buildProfileUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=buildprofile&ProjectID={0}");

        private ApplicationContext _context = null;
        LifeCycleStageManager objLifeCycleStageManager = null;
        RequestTypeManager requestTypeManager = null;
        public ConfigurationVariableManager configManager = null;
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
            EnableGanttProfilePic = configManager.GetValueAsBool(ConfigConstants.EnableGanttProfilePic);
            ModuleName = uHelper.getModuleNameByTicketId(ticketID);

            if (!string.IsNullOrEmpty(ticketID))
            {
                objLifeCycleStageManager = new LifeCycleStageManager(ApplicationContext);
                spListItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);
                var stage = objLifeCycleStageManager.Load(x => x.ModuleNameLookup == ModuleName && x.CustomProperties.Contains("awardstage"));
                if (stage != null && stage.Count > 0)
                    awardStageStep = stage.FirstOrDefault().StageStep;

                if (spListItem != null)
                {
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
                }
            }



            importAllocationTemplateUrl = string.Format(importAllocationTemplateUrl, ticketID);
            buildProfileUrl = string.Format(buildProfileUrl, ticketID);

            ctrSaveAllocationAsTemplate.ModuleName = ModuleName;
            ctrSaveAllocationAsTemplate.ProjectID = ticketID;
            ctrSaveAllocationAsTemplate.PopupID = pcSaveAsTemplate.ClientID;

            //CPR New Date logic
            if (ModuleName == "CPR" || ModuleName == "CNS" || ModuleName == "OPM")
            {
                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart))
                    StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                else
                    HideConst = "Hidden";
                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                else
                    HideConst = "Hidden";

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.PreconStartDate))
                    PreConStartDateType = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconStartDate]);
                else
                    HidePrecon = "Hidden";

                if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.PreconEndDate))
                    PreConEndDateType = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconEndDate]);
                else
                    HidePrecon = "Hidden";

            }
            else
            {
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


                if (EndDate < StartDate)
                    EndDate = StartDate;
                if (EndDate == DateTime.MinValue)
                    EndDate = StartDate;
            }


            StartDateString = StartDate.ToString("MMM d, yyyy");
            EndDateString = EndDate.ToString("MMM d, yyyy");

            PreConStartDate = PreConStartDateType.ToString("MMM d, yyyy");
            PreConEndDate = PreConEndDateType.ToString("MMM d, yyyy");


        }
    }
}