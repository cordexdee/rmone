using Newtonsoft.Json;
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
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.RMM;

namespace uGovernIT.Web
{
    public partial class MultiAllocationsJS : System.Web.UI.UserControl
    {
        public string ticketID { get; set; }
        public string ModuleName { get; set; }
        public string UserID { get; set; }
        public string Type { get; set; }
        public string WorkItem { get; set; }
        protected DateTime StartDate { get; set; }
        protected DateTime EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
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
        public string SetAlignment { get; set; }

        public bool EnableGanttProfilePic { get; set; }

        public string CompanyLookup = string.Empty;

        public string ProjectComplexity = string.Empty;

        public string RequestType = string.Empty;
        public string RoleID { set; get; }

        protected string ajaxPageURL;

        DataRow spListItem;
        private string formTitle = "Project Allocation";
        public string isclosebtnrequired { get; set; }
        //private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketId={2}";
        protected string importAllocationTemplateUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importallocationtemplate&ProjectID={0}");
        protected string buildProfileUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=buildprofile&ProjectID={0}");
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&WorkItem={2}&SelectedUsersList={3}&Type={4}";
        public string MultiAddUrl { get; set; }
        private ApplicationContext _context = null;
        LifeCycleStageManager objLifeCycleStageManager = null;
        RequestTypeManager requestTypeManager = null;
        public ConfigurationVariableManager configManager = null;
        public string lstUserProfiles = null;
        public string TennantID { get; set; }
        public bool IsAllocationTypeHard_Soft { get; set; }
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
        private UserProfile _user;
        public string UserRole
        {
            get
            {
                if (_user != null)
                    return _user.GlobalRoleId;
                else
                    return ApplicationContext.CurrentUser?.Id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/rmmapi/");

            ModuleName = !string.IsNullOrEmpty(ModuleName) ? ModuleName : Type;
            TennantID = HttpContext.Current.GetManagerContext().TenantID;
            if (string.IsNullOrEmpty(UserID))
            {
                var lstUsers = HttpContext.Current.GetManagerContext().UserManager.GetEnabledUsers(); //.GetUsersProfile();
                hdnlstUserProfiles.Value = JsonConvert.SerializeObject(lstUsers);
            }
            else
            {
                List<DdlTypeList> lstType = new List<DdlTypeList>();
                DataTable resultedTable = AllocationTypeManager.LoadLevel1(HttpContext.Current.GetManagerContext());
                if (resultedTable != null)
                {
                    //ddlLevel1.Items.Add(new ListEditItem("--Select--", "--Select--"));
                    foreach (DataRow row in resultedTable.Rows)
                    {
                        DdlTypeList objType = new DdlTypeList();
                        if (row["LevelTitle"] != null && row["LevelTitle"].ToString() != string.Empty)
                        {
                            if (row["LevelName"].ToString() == "CPR" || row["LevelName"].ToString() == "CNS" || row["LevelName"].ToString() == "OPM")
                            {
                                objType.LevelName = row["LevelName"].ToString();
                                objType.LevelTitle = row["LevelTitle"].ToString();
                                lstType.Add(objType);
                            }
                        }
                    }
                }
                hdnLstType.Value = JsonConvert.SerializeObject(lstType);
                hdnlstUserProfiles.Value = "";
                _user = ApplicationContext.UserManager.GetUserById(UserID);
            }
            ticketID = !string.IsNullOrEmpty(ticketID) ? ticketID : WorkItem;


            requestTypeManager = new RequestTypeManager(ApplicationContext);
            configManager = new ConfigurationVariableManager(ApplicationContext);
            int ticketStageStep = 1;
            int awardStageStep = 1000; // Changed to 1000 as mostly ticketStageStep may not go beyond this No., & a module with 'awardstage' will change this step No. in below code.
            string title = string.Format("{0} - New Item", formTitle);
            string module = string.Empty;
            DataRow spCompanyItem = null;
            EnableGanttProfilePic = configManager.GetValueAsBool(ConfigConstants.EnableGanttProfilePic);


            if (!string.IsNullOrEmpty(ticketID))
            {
                isclosebtnrequired = UGITUtility.ObjectToString(Request["isdlg"]);
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

                MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "addmultiallocation", "Add Multiple Allocations", UGITUtility.ObjectToString(ticketID), "", ModuleName));
            }



            importAllocationTemplateUrl = string.Format(importAllocationTemplateUrl, ticketID);
            buildProfileUrl = string.Format(buildProfileUrl, ticketID);

            ModuleViewManager moduleViewManager = new ModuleViewManager(ApplicationContext);
            UGITModule uGITModule = moduleViewManager.LoadByName(ModuleName);
            if(uGITModule != null)
                IsAllocationTypeHard_Soft = uGITModule.IsAllocationTypeHard_Soft;
            else
                IsAllocationTypeHard_Soft = false;

            //CPR New Date logic
            if (ModuleName == "CPR" || ModuleName == "CNS" || ModuleName == "OPM")
            {
                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart))))
                    {
                        StartDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        hdnConstStartDate.Value = ConstructionStartDateString = StartDate.ToString("MMM d, yyyy");
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))))
                    {
                        EndDate = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        hdnConstEndDate.Value= ConstructionEndDateString = EndDate.ToString("MMM d, yyyy");
                        hdnCloseoutStartDate.Value = EndDate.AddDays(1).ToString("MMM d, yyyy");
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
                        hdnPreConStartDate.Value = PreConStartDateString = PreConStartDateType.ToString("MMM d, yyyy");
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.PreconEndDate))))
                    {
                        PreConEndDateType = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.PreconEndDate]);
                        hdnPreConEndDate.Value= PreConEndDateString = PreConEndDateType.ToString("MMM d, yyyy");
                    }
                    if (string.IsNullOrEmpty(PreConStartDateString) && string.IsNullOrEmpty(PreConEndDateString))
                        HidePrecon = "Hidden";
                }
                else
                    HidePrecon = "Hidden";

                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CloseoutDate))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.CloseoutDate))))
                        hdnCloseOutEndDate.Value = CloseOutEndDateString = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.CloseoutDate]).ToString("MMM d, yyyy");
                }
                if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.CloseoutDate))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(UGITUtility.GetSPItemValue(spListItem, DatabaseObjects.Columns.CloseoutStartDate))))
                        hdnCloseoutStartDate.Value = CloseOutStartDateString = UGITUtility.StringToDateTime(spListItem[DatabaseObjects.Columns.CloseoutStartDate]).ToString("MMM d, yyyy");
                }
                
                if (string.IsNullOrEmpty(CloseOutStartDateString) && string.IsNullOrEmpty(CloseOutEndDateString))
                    HideCloseout = "Hidden";

            }
            else
            {
                //HideConst = "Hidden";
                //HidePrecon = "Hidden";
                //HideCloseout = "Hidden";
                //EnableMultiAllocation = "Hidden";
                //SetAlignment = "style=float:right";

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
    public class DdlTypeList
    {
        public string LevelTitle { get; set; }
        public string LevelName { get; set; }
    }
}