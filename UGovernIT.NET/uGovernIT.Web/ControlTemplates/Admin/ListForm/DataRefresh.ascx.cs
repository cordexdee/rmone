using FuzzySharp.Extractor;
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.SimilarityRatio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DxReport;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using FuzzySharp;
using DevExpress.ExpressApp;
using Newtonsoft.Json;
using uGovernIT.Manager.RMM.ViewModel;
using DevExpress.Office.Utils;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Models;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class DataRefresh : System.Web.UI.UserControl
    {
        public int ShortNameLength;
        private ApplicationContext context;
        private ConfigurationVariableManager configVariableManager = null;
        private ResourceAllocationManager allocationmanager = null;
        private ExperiencedTagManager experiencedTagManager;
        private TicketManager ticketManager;
        private ModuleViewManager moduleViewManager;
        private Ticket cprTicketRequest;
        private UserProjectExperienceManager userProjectExperienceManager;
        private UserProfileManager userProfileManager;
       
        ProjectEstimatedAllocationManager estimatedAllocationManager = null;
        public DataRefresh()
        {
            context = HttpContext.Current.GetManagerContext();
            configVariableManager = new ConfigurationVariableManager(context);
            allocationmanager = new ResourceAllocationManager(context);
            experiencedTagManager = new ExperiencedTagManager(context);
            ticketManager = new TicketManager(context);
            moduleViewManager = new ModuleViewManager(context);
            cprTicketRequest = new Ticket(context, ModuleNames.CPR);
            userProjectExperienceManager = new UserProjectExperienceManager(context);
            userProfileManager = new UserProfileManager(context);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            estimatedAllocationManager = new ProjectEstimatedAllocationManager(context);
            bool UpdateResourceAllocation = configVariableManager.GetValueAsBool(ConfigConstants.UpdateResourceAllocation);
            btnUpdateResourceAllocation.Visible = UpdateResourceAllocation;
            bool del = Delay(1000);
            ShortNameLength = UGITUtility.StringToInt(configVariableManager.GetValue(ConfigConstants.ShortNameCharacters));
            btnImportProjAllocations.Visible= configVariableManager.GetValueAsBool(ConfigConstants.AllocationImportEnabled);
            if (RMMSummaryHelper.ProcessState())
            {
                if (BtnUpdateSummaryDataInProcess.Visible)
                    BtnUpdateSummaryDataInProcess.ClientEnabled = false;
                if (btnUpdateResourceAllocation.Visible)
                    btnUpdateResourceAllocation.ClientEnabled = false;
            }
            else
            {
                    BtnUpdateSummaryDataInProcess.ClientEnabled = true;
                    btnUpdateResourceAllocation.ClientEnabled = true;
            }

            //if (allocationmanager.ProcessState())
            //{
            //    btnUpdateResourceAllocation.Text = "Update Allocation In Process";
            //    btnUpdateResourceAllocation.ClientEnabled = false;
            //}
            bool canRecreateExperienceTags = configVariableManager.GetValueAsBool(ConfigConstants.AllowRecreatingExperienceTags);
            if (!canRecreateExperienceTags)
            {
                btn_createProjectTags.Visible = false;
                btn_createResourceTags.Visible = false; 
            }
        }

        private bool Delay(int millisecond)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            bool flag = false;
            while (!flag)
            {
                if (sw.ElapsedMilliseconds > millisecond)
                {
                    flag = true;
                }
            }
            sw.Stop();
            return true;
        }

        protected void btnUpdateOPM_ERPJOBID_Click(object sender, EventArgs e)
        {
            try
            {
                ticketManager.UpdateTicketCache(moduleViewManager.LoadByName(ModuleNames.OPM));
                refreshData.Text = "OPM Data has been refreshed";
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }
            
        }

        protected void btnfill_ResourceUtil_SummaryData_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = RMMSummaryHelper.FillResourceUtilization(context);
                if (status)
                {
                    refreshData.Text = "Data has been filled in resource utilization summary table";
                }
                else
                {
                    refreshData.Text = "System is unable to fill data in resource utilization summary table";
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }
        }

        protected void btn_updateshortname_Click(object sender, EventArgs e)
        {
            try
            {
                string[] moduleNames = { "CNS", "CPR", "OPM"};
                bool status = uHelper.Updateshortname(context);
                if (status)
                {
                    foreach (var item in moduleNames)
                    {
                        UGITModule module = moduleViewManager.LoadByName(item);
                        ticketManager.RefreshCacheModuleWise(module);
                    }
                    refreshData.Text = "Short Name has been updated based on configuration!!";
                }
                else
                {
                    refreshData.Text = "System is unable to update Short Name !!";
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }
        }

        protected void btn_createNewUserExpTags_Click(object sender, EventArgs e)
        {
            StartProcessingUserResourceTags(false);
        }

        protected void btn_updateUserExpTags_Click(object sender, EventArgs e)
        {
            StartProcessingUserResourceTags(true);
        }

        private void StartProcessingUserResourceTags(bool toUpdateTags)
        {
            try
            {
                var userProfiles = userProfileManager.GetUsersProfile();

                ULog.WriteLog($"Creating resource tags for {userProfiles.Count} users.");

                foreach (UserProfile profile in userProfiles)
                {
                    ULog.WriteLog($" -- Processing resource tags for user - {profile.Id}");
                    List<string> commonTags = userProjectExperienceManager.GetProjectTagsToUpdateForUserTags(toUpdateTags, profile);
                    userProjectExperienceManager.UpdateUserProjectTagExperience(commonTags, "", profile.Id);

                    ULog.WriteLog($" -- Completed resource tags for user - {profile.Id}");
                }
                refreshData.Text = "All tags are processed successfully.";
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
                refreshData.Text = $"Failed to process tags.";
            }
        }

        protected void btn_updatedisabledusersallocation_Click(object sender, EventArgs e) {
            estimatedAllocationManager.UpdateAllocationForDisabledUsers();
            refreshData.Text = "Allocations have been updated for disabled users.";
        }
    }
}