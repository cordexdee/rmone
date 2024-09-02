using DevExpress.CodeParser;
using DevExpress.Internal.WinApi;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting.Control;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using ApplicationContext = uGovernIT.Manager.ApplicationContext;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class ProjectSummary : System.Web.UI.UserControl
    {
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        public string ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/RMOne/");
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        protected string Token { get; set; }
        public string TicketId { get; set; }
        public string PreconStartDate { get; set; }
        public string PreconEndDate { get; set; }
        public string ConstStartDate { get; set; }
        public string ConstEndDate { get; set; }
        public string CloseoutStartDate { get; set; }
        public string CloseoutEndDate { get; set; }
        public string Sector { get; set; }
        public string Address { get; set; }
        public string NetRentableSqFt { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string RetailSqftNum { get; set; }
        public string AcquisitionCost { get; set; }
        public string ProjectComplexityChoice { get; set; }
        public string ActualAcquisitionCost { get; set; }
        public string ForecastedProjectCost { get; set; }
        public string ActualProjectCost { get; set; }
        public string ApproxContractValue { get; set; }
        public string ERPJobID { get; set; }
        public string ERPJobIDNC { get; set; }
        public string ProjectType { get; set; }
        public string ProjectTitle { get; set; }
        public string CRMCompanyLookup { get; set; }
        public string ProjectDescription { get; set; }
        public string CurrentTicketStage { get; set; }
        public string ContractType { get; set; }
        public string SalesForceId { get; set; }

        public int Volatility { get; set; }

        public int Complexity { get; set; }

        public double ResouceHoursBilled { get; set; }
        public double ResourceHoursPrecon { get; set; }
        public double ResourceHoursBilledtoDate { get; set; }
        public double ResourceHoursActual { get; set; }
        public double ResourceHoursRemaining { get; set; }
        public double TotalResourceHours { get; set; }
        public double TotalResourceCost { get; set; }
        protected FieldConfigurationManager fieldConfigurationManager;
        protected int closeoutperiod = 0;
        public string TitleLink { get; set; }
        public string TicketIDLink { get; set; }
        public string IsParentModuleWebPart { get; set; }
        public string ProjectLeadUser { get; set; }
        public string LeadEstimatorUser { get; set; }
        public string ProjectLeadUserID { get; set; }
        public string LeadEstimatorUserID { get; set; }
        public string LeadSuperintendentUser { get;set; }
        public string LeadSuprintendentUserID { get;set; }
        public string TicketStageTitle { get; set; }
        public string OpportunityTypeChoice { get; set; }
        public string AwardedLossDate { get; set; }
        public bool HideAnalyticsValue { get; set; }
        protected string ModuleName
        {
            get
            {
                return uHelper.getModuleNameByTicketId(TicketId);
            }
        }
        protected DataRow CurrentTicket
        {
            get
            {
                Ticket ticket = new Ticket(AppContext, ModuleName);
                return Ticket.GetCurrentTicket(AppContext, ModuleName, TicketId);
            }
        }

        public Boolean IsModuleWebpartChild
        {
            get { return UGITUtility.StringToBoolean(Request["isModuleWebpartChild"]); }
        }

        public bool HasAnyPastAllocation
        {
            get {
                return uHelper.HasAnyPastAllocation(TicketId);
            }
        }

        public string ERPJobIDName
        {
            get
            {
                return uHelper.GetERPJobIDName(AppContext);
            }
        }

        public string ERPJobIDNCName
        {
            get
            {
                return uHelper.GetERPJobIDNCName(AppContext);
            }
        }
        public bool IsRequestFromSummary { get; set; }
        public List<ModuleFormLayout> Layouts = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(AppContext);
            TicketManager ticketManager = new TicketManager(AppContext);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(AppContext);
            HideAnalyticsValue = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.HideAnalyticsValue);

            if (this.CurrentTicket != null)
            {
                UGITModule moduleObj = moduleManager.LoadByName(ModuleName);
                closeoutperiod = uHelper.getCloseoutperiod(AppContext);
                
                Layouts = moduleObj.List_FormLayout.Where(x => x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();
                ModuleConstraintsListDx.TicketId = TicketId;

                //set labels from module form layouts
                divCRMCompanyLookup.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.CRMCompanyLookup) ?? "Company";
                divSector.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.BCCISector) ?? "Sector";
                divProjectType.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.RequestTypeLookup) ?? "Project Type";
                divERPJobID.InnerText = ERPJobIDName;
                divERPJobIDNC.InnerText = ERPJobIDNCName;
                divSalesForceId.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ExternalID) ?? "Salesforce Id";
                divProjectLead.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ProjectLeadUser) ?? "Project Lead";
                divLeadEstimator.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.LeadEstimatorUser) ?? "Lead Estimator";
                divLeadSuperintendent.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.LeadSuperintendentUser) ?? "Lead Superintendent";
                divAddress.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.Address) ?? "Address";
                divCity.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.City) ?? "City";
                divZip.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.Zip) ?? "Zip";
                divNetRentableSqFt.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.UsableSqFtNum) ?? "Net Rentable Sq Ft";
                divContractType.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ContactType) ?? "Contract Type";
                divRetailSqftNum.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.RetailSqftNum) ?? "Gross Sq FT";
                divApproxContractValue.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ApproxContractValue)?? "Contract Cost";
                //divResourceHoursBilled.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ResouceHoursBilled) ?? "Resource Hours Billed";
                divAwardedLossDate.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.AwardedLossDate) ?? "Award/Loss Date";
                divActualAcquisitionCost.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ActualAcquisitionCost) ?? "Actual Acquisition Cost";
                divForecastedProjectCost.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ForecastedProjectCost) ?? "Forecasted Project Cost";
                divActualProjectCost.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ActualProjectCost) ?? "Actual Project Cost";
                divResourceHoursPrecon.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ResourceHoursPrecon) ?? "Resource 'Hours' Precon";
                divResourceHoursBilledtoDate.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ResourceHoursBilledtoDate) ?? "Resource Hours Billed to Date";
                divResourceHoursActual.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ResourceHoursActual) ?? "Resource 'Hours' Actual";
                divResourceHoursRemaining.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.ResourceHoursRemaining) ?? "Resource 'Hours' Remaining";
                divTotalResourceHours.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.TotalResourceHours) ?? "Total Resource Hours";
                divTotalResourceCost.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.TotalResourceCost) ?? "Total Resource Cost";
                divComplexity.InnerText = GetFormFieldLabel(DatabaseObjects.Columns.CRMProjectComplexity) ?? "Complexity";
                //Added below 3 line to identify request is coming from Summary or AgentData
                ModuleConstraintsListDx.IsRequestFromSummary = IsRequestFromSummary;
                //ModuleConstraintsListDx.IsRequestFromSummaryOrTask = Session["IsRequestFromSummaryOrTask"] != null ? Convert.ToBoolean(Session["IsRequestFromSummaryOrTask"]) : false;
                //Session["IsRequestFromSummaryOrTask"] = null;
                //ENd
                AddProjectExperienceTags.TicketId = TicketId;
                AddProjectExperienceTags.RequestFrom = "summaryTab";
                fieldConfigurationManager = new FieldConfigurationManager(AppContext);
                this.PreconStartDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.PreconStartDate]), false);
                this.PreconEndDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.PreconEndDate]), false);                
                this.ConstStartDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]), false);                
                this.ConstEndDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]), false);                
                this.CloseoutStartDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.CloseoutStartDate]), false);
                //If CloseoutStartDate is blank, then no need to add days to CloseoutStartDate to create the CloseoutEndDate.
                if (string.IsNullOrEmpty(this.CloseoutStartDate))
                    this.CloseoutEndDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket["CloseOutDate"]), false);
                else
                    this.CloseoutEndDate = UGITUtility.StringToDateTime(Convert.ToString(CurrentTicket["CloseOutDate"])) != DateTime.MinValue
                        ? UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket["CloseOutDate"]), false)
                        : UGITUtility.GetDateStringInFormat(UGITUtility.StringToDateTime(CurrentTicket[DatabaseObjects.Columns.CloseoutStartDate]).AddWorkingDays(uHelper.getCloseoutperiod(AppContext)).ToString(), false);

                if (!string.IsNullOrEmpty(this.PreconStartDate))
                {
                    PreconStartDateCss.Attributes.Add("class", "rcorners2");
                }
                if (!string.IsNullOrEmpty(this.PreconEndDate))
                {
                    PreconEndDateCss.Attributes.Add("class", "rcorners2");
                }
                if (!string.IsNullOrEmpty(this.ConstStartDate))
                {
                    ConstStartDateCss.Attributes.Add("class", "rcorners2");
                }
                if (!string.IsNullOrEmpty(this.ConstEndDate))
                {
                    ConstEndDateCss.Attributes.Add("class", "rcorners2");
                }
                if (!string.IsNullOrEmpty(this.CloseoutStartDate))
                {
                    CloseoutStartDateCss.Attributes.Add("class", "rcorners2");
                }
                if (!string.IsNullOrEmpty(this.CloseoutEndDate))
                {
                    CloseoutEndDateCss.Attributes.Add("class", "rcorners2");
                }
                

                this.Sector = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.BCCISector]);
                this.Address = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.StreetAddress1]);
                this.NetRentableSqFt = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.UsableSqFtNum].ToString()) ? 
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.UsableSqFtNum]).ToString("N0") : "0";
                this.City = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.City]);
                this.ZipCode = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.Zip]);
                this.RetailSqftNum = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.RetailSqftNum].ToString()) ? 
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.RetailSqftNum]).ToString("N0") : "0";

                // Forecasted AcquisitionCost
                this.AcquisitionCost = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.AcquisitionCost].ToString()) ?
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.AcquisitionCost]).ToString("N0") : "0";
                this.ActualAcquisitionCost = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.ActualAcquisitionCost].ToString()) ?
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ActualAcquisitionCost]).ToString("N0") : "0";
                this.ForecastedProjectCost = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.ForecastedProjectCost].ToString()) ?
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ForecastedProjectCost]).ToString("N0") : "0";
                this.ActualProjectCost = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.ActualProjectCost].ToString()) ?
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ActualProjectCost]).ToString("N0") : "0";
                this.ProjectComplexityChoice = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.CRMProjectComplexity]);
                this.ApproxContractValue = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.ApproxContractValue].ToString()) ? 
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ApproxContractValue]).ToString("N0") : "0";
                this.ERPJobID = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.ERPJobID]);
                this.ERPJobIDNC = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.ERPJobIDNC]);
                this.ProjectType = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RequestTypeLookup, Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.RequestTypeLookup]));
                this.ProjectTitle = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.Title]);
                if(!string.IsNullOrEmpty(Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.CRMCompanyLookup])))
                    this.CRMCompanyLookup = ticketManager.GetByTicketIdFromCache(ModuleNames.COM, Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.CRMCompanyLookup]))[DatabaseObjects.Columns.Title].ToString(); //fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyLookup, Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.CRMCompanyLookup]));
                this.ProjectDescription = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.Description]);
                if (this.CurrentTicket[DatabaseObjects.Columns.OnHold].ToString() == "1")
                {
                    this.CurrentTicketStage = "OnHold";
                    this.TicketStageTitle = "OnHold";
                }
                else
                {
                    this.CurrentTicketStage = this.CheckTicketStage(ModuleName, HttpContext.Current.CurrentUser(), int.Parse(this.CurrentTicket["StageStep"].ToString()));
                    this.TicketStageTitle = this.GetTicketStageTitle(ModuleName, HttpContext.Current.CurrentUser(), int.Parse(this.CurrentTicket["StageStep"].ToString()));
                }
                this.ContractType = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.OwnerContractTypeChoice]);
                this.SalesForceId = Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.ExternalID]);

                if (this.CurrentTicket[DatabaseObjects.Columns.Complexity] == System.DBNull.Value)
                {
                    this.Complexity = 0;
                }
                else
                {
                    this.Complexity = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.Complexity]);
                }

                if (this.CurrentTicket[DatabaseObjects.Columns.ResouceHoursBilled] == System.DBNull.Value)
                {
                    this.ResouceHoursBilled = 0;
                }
                else
                {
                    this.ResouceHoursBilled = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ResouceHoursBilled]);
                }

                if (this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursPrecon] == System.DBNull.Value)
                {
                    this.ResourceHoursPrecon = 0;
                }
                else
                {
                    this.ResourceHoursPrecon = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursPrecon]);
                }


                if (this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursBilledtoDate] == System.DBNull.Value)
                {
                    this.ResourceHoursBilledtoDate = 0;
                }
                else
                {
                    this.ResourceHoursBilledtoDate = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursBilledtoDate]);
                }
                if (this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursActual] == System.DBNull.Value)
                {
                    this.ResourceHoursActual = 0;
                }
                else
                {
                    this.ResourceHoursActual = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursActual]);
                }
                if (this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursRemaining] == System.DBNull.Value)
                {
                    this.ResourceHoursRemaining = 0;
                }
                else
                {
                    this.ResourceHoursRemaining = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ResourceHoursRemaining]);
                }
                if (this.CurrentTicket[DatabaseObjects.Columns.TotalResourceHours] == System.DBNull.Value)
                {
                    this.TotalResourceHours = 0;
                }
                else
                {
                    this.TotalResourceHours = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.TotalResourceHours]);
                }
                if (this.CurrentTicket[DatabaseObjects.Columns.TotalResourceCost] == System.DBNull.Value)
                {
                    this.TotalResourceCost = 0;
                }
                else
                {
                    this.TotalResourceCost = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.TotalResourceCost]);
                }
                if (this.CurrentTicket[DatabaseObjects.Columns.Volatility] == System.DBNull.Value)
                {
                    this.Volatility = 0;
                }
                else
                {
                    this.Volatility = Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.Volatility]);
                }

                this.ProjectLeadUserID = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.ProjectLeadUser]);
                this.LeadEstimatorUserID = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.LeadEstimatorUser]);
                this.ProjectLeadUser = uHelper.GetUserNameBasedOnId(AppContext, UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.ProjectLeadUser]));
                this.LeadEstimatorUser = uHelper.GetUserNameBasedOnId(AppContext, UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.LeadEstimatorUser]));
                this.LeadSuprintendentUserID = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.LeadSuperintendentUser]);
                this.LeadSuperintendentUser = uHelper.GetUserNameBasedOnId(AppContext, UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.LeadSuperintendentUser]));
                this.OpportunityTypeChoice= Convert.ToString(this.CurrentTicket[DatabaseObjects.Columns.OpportunityTypeChoice]);
                this.AwardedLossDate = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.AwardedLossDate]), false);
                // Access the parent control

                if (IsModuleWebpartChild)
                {
                    TitleLink = ProjectTitle;
                    IsParentModuleWebPart = "1";
                }
                else
                {
                    TitleLink = $"<a href='{uHelper.GetHyperLinkControlForTicketID(moduleObj, TicketId, true, ProjectTitle).NavigateUrl}' style='color:black;font-weight:800px !important;'>{ProjectTitle}</a>";
                    IsParentModuleWebPart = "0";
                }
            }


        }

        public string CheckTicketStage(string currentModuleName, UserProfile user, int stagStep)
        {
            Ticket TicketRequest = new Ticket(AppContext, currentModuleName, user);
            LifeCycle lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault();
            List<LifeCycleStage> allStages = lifeCycle.Stages;
            int resolvedStageStep = 0;
            int closedStageStep = 0;
            LifeCycleStage resolvedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());
            LifeCycleStage closedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());
            if (resolvedStage != null)
                resolvedStageStep = resolvedStage.StageStep;
            if (closedStage != null)
                closedStageStep = closedStage.StageStep;

            if (stagStep < resolvedStageStep)
            {
                return "Precon";
            }
            else if (stagStep >= resolvedStageStep && stagStep < closedStageStep)
            {
                return "Const";
            }
            else
            {
                return "Closeout";
            }
        }

        public string GetTicketStageTitle(string currentModuleName, UserProfile user, int stagStep)
        {
            Ticket TicketRequest = new Ticket(AppContext, currentModuleName, user);
            LifeCycle lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault();
            List<LifeCycleStage> allStages = lifeCycle.Stages;
            return allStages.Where(x => x.StageStep == stagStep)?.FirstOrDefault()?.StageTitle ?? string.Empty;
        }

        public string GetFormFieldLabel(string fieldName)
        {
            string fieldLabel = null;
            if(Layouts != null)
            {
                ModuleFormLayout moduleFormLayoutObj = Layouts.FirstOrDefault(x=>x.FieldName.ToLower() == fieldName.ToLower());
                if (moduleFormLayoutObj != null)
                {
                    fieldLabel = moduleFormLayoutObj.FieldDisplayName;
                }
            }
            return fieldLabel;
        }
    }
}