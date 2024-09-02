using DevExpress.Web;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ImportAllocationTemplate : System.Web.UI.UserControl
    {
        public string ProjectID, searchData = string.Empty; //Sector = string.Empty, ProjectComplexity = "0",
        public string searchColumns = string.Empty;
        //List<ProjectAllocationTemplate> ProjectAllocationTemplates;
        ProjectSimilarityMetricsManager projectSimilarityMetricsManager = new ProjectSimilarityMetricsManager(HttpContext.Current.GetManagerContext());
        ProjectEstimatedAllocationManager CRMProjectAllocMGR = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
        ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        List<ProjectEstimatedAllocation> dbCRMAllocations;
        Dictionary<string, string> dicSearchCols = null;
        protected bool allocationExist;
        public bool IsGroupAdmin { get; set; }
        public bool IsResourceAdmin
        {
            get
            {
                return HttpContext.Current.GetManagerContext().UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dicSearchCols = new Dictionary<string, string>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();


            uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(HttpContext.Current.GetManagerContext());
            string moduleName = uHelper.getModuleNameByTicketId(ProjectID);
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule module = moduleViewManager.LoadByName(moduleName);
            DataRow newTicketObj = null; ; 
            if (!string.IsNullOrEmpty(ProjectID))
            {
                newTicketObj = ticketManager.GetByTicketID(module, ProjectID);
                dbCRMAllocations = CRMProjectAllocMGR.Load(x => x.TicketId == ProjectID);
                if (dbCRMAllocations != null && dbCRMAllocations.Count > 0)
                    allocationExist = true;
            }
            DateTime projectEndDate = DateTime.MinValue;
            DateTime projectStarDate = DateTime.MinValue;
            bool enableSimiarityFunction = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableSimilarityFunction);
            if (!enableSimiarityFunction)
                btnFindSimilarProjects.Visible = false;
            var metrics = projectSimilarityMetricsManager.Load(x => x.ModuleNameLookup == moduleName).FirstOrDefault();
            if (metrics != null)
                searchColumns = metrics.SearchColumns;

            if (newTicketObj != null)
            {
                //if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.BCCISector))
                //    Sector = Convert.ToString(newTicketObj[DatabaseObjects.Columns.BCCISector]);
                //if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.CRMProjectComplexity))
                //    ProjectComplexity = Convert.ToString(newTicketObj[DatabaseObjects.Columns.CRMProjectComplexity]);

                if (!string.IsNullOrEmpty(searchColumns))
                {
                    string value = string.Empty;
                    string[] arrSrch = searchColumns.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in arrSrch)
                    {
                        value = string.Empty;
                        if (UGITUtility.IsSPItemExist(newTicketObj, item))
                            value = Convert.ToString(newTicketObj[item]);

                        dicSearchCols.Add(item, value);
                    }

                    searchData = serializer.Serialize(dicSearchCols);
                }

                if (UGITUtility.IfColumnExists(newTicketObj, DatabaseObjects.Columns.CloseoutDate) && UGITUtility.IfColumnExists(newTicketObj, DatabaseObjects.Columns.CloseoutStartDate))
                {
                    DateTime closeOutEndDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.CloseoutDate]);
                    DateTime closeOutStartDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (closeOutEndDate != DateTime.MinValue)
                    {
                        projectEndDate = closeOutEndDate;
                    }
                    else if (closeOutStartDate != DateTime.MinValue)
                    {
                        projectEndDate = closeOutStartDate.AddWorkingDays(uHelper.getCloseoutperiod(HttpContext.Current.GetManagerContext()));
                    }
                }

                if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.EstimatedConstructionEnd) && projectEndDate == DateTime.MinValue)
                    projectEndDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                if(UGITUtility.IsPropertyExists(newTicketObj, DatabaseObjects.Columns.TicketTargetCompletionDate) && projectEndDate == DateTime.MinValue)
                    projectEndDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.TicketTargetCompletionDate]);
                if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.PreconStartDate))
                    projectStarDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.PreconStartDate]);
                if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.TicketTargetStartDate) && projectStarDate == DateTime.MinValue)
                    projectStarDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.TicketTargetStartDate]);
                if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.EstimatedConstructionStart) && projectStarDate == DateTime.MinValue)
                    projectStarDate = UGITUtility.GetObjetToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);

                if (projectStarDate == DateTime.MinValue)
                {
                    projectStarDate = UGITUtility.GetObjetToDateTime(newTicketObj["CreationDate"]);
                }
                if (projectStarDate == DateTime.MinValue) {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alertMessage();", true);
                    return;
                }
                //int duration = UGITUtility.StringToInt(newTicketObj["ProjectDuration"]);
                int duration = 0;
                if (UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.ProjectDuration))
                    duration = UGITUtility.StringToInt(newTicketObj[DatabaseObjects.Columns.ProjectDuration]);
                else if(UGITUtility.IsSPItemExist(newTicketObj, DatabaseObjects.Columns.CRMDuration))
                    duration = UGITUtility.StringToInt(newTicketObj[DatabaseObjects.Columns.CRMDuration]);

                txtDuration.Text = duration.ToString();
                if (projectEndDate == DateTime.MinValue)
                {
                    if (duration <= 0)
                        projectEndDate = projectStarDate;
                    else
                        projectEndDate = projectStarDate.AddDays(duration * 7);
                }
            }
           

            if (projectStarDate > projectEndDate)
                projectStarDate = projectEndDate;

            dteStartDate.Date = projectStarDate;
            dteEndDate.Date = projectEndDate;

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            if (context.UserManager.IsAdmin(context.CurrentUser) || context.UserManager.IsUGITSuperAdmin(context.CurrentUser) || context.UserManager.IsTicketAdmin(context.CurrentUser))
            {
                IsGroupAdmin = true;
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            
            uHelper.ClosePopUpAndEndResponse(HttpContext.Current, true);
        }

    }
}