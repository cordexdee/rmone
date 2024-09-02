using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Web.Models;

namespace uGovernIT.Web
{
    public partial class CreateProjectTags : System.Web.UI.UserControl
    {
        private ApplicationContext context;
        private ExperiencedTagManager experiencedTagManager;
        private ModuleViewManager moduleViewManager;
        private TicketManager ticketManager;
        private UserProjectExperienceManager userProjectExperienceManager;
        private StatisticsManager statisticsManager;

        private List<string> ticketIDs;
        private bool processSelectedRecords;
        private UGITModule selectedModule;

        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            experiencedTagManager = new ExperiencedTagManager(context);
            moduleViewManager = new ModuleViewManager(context);
            ticketManager = new TicketManager(context);
            userProjectExperienceManager = new UserProjectExperienceManager(context);
            statisticsManager = new StatisticsManager(context);

            processSelectedRecords = Request.QueryString["ProcessSelectedRecords"] == "True" ? true : false;
            if (processSelectedRecords)
            {
                ticketIDs = new List<string>();
                ticketIDs.AddRange(Request.QueryString["TicketIDs"]?.Split(','));
                string moduleName = uHelper.getModuleNameByTicketId(ticketIDs.First());
                selectedModule = moduleViewManager.GetByName(moduleName);

                divModuleSelector.Visible = false;
                divRequestTypeSelector.Visible = false; 
                lblMsg.Text = "Updating tags will update OR delete all existing and create new experience tags. Please select one of the options below.";
            }
            else
            {
                divModuleSelector.Visible = true;
                divRequestTypeSelector.Visible = true;

                lblMsg.Text = "Creating tags will update OR delete all existing and create new experience tags. Please select the module(s) and request type(s) below.";
            }
        }

        protected void btnNewTags_Click(object sender, EventArgs e)
        {
            StartProcessingTags(false);
        }

        protected void btnUpdateTags_Click(object sender, EventArgs e)
        {
            StartProcessingTags(true);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void StartProcessingTags(bool tagsToUpdate)
        {
            try
            {
                var tags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID);

                if (processSelectedRecords)
                {
                    bool isSucceed = ProcessProjects(tags, ticketIDs, selectedModule, tagsToUpdate);
                    if (isSucceed)
                    {
                        refreshData.Text = "All tags are processed successfully.";
                        uHelper.ClosePopUpAndEndResponse(Context, false);
                    } 
                }
                else
                {
                    List<UGITModule> selectedModules = new List<UGITModule>();
                    if (chkCPR.Checked)
                        selectedModules.Add(moduleViewManager.GetByName(ModuleNames.CPR));
                    if (chkCNS.Checked)
                        selectedModules.Add(moduleViewManager.GetByName(ModuleNames.CNS));
                    if (chkOPM.Checked)
                        selectedModules.Add(moduleViewManager.GetByName(ModuleNames.OPM));

                    if (selectedModules.Count == 0)
                    {
                        refreshData.Text = "Please select at least one module.";
                        return;
                    }
                    if (!(chkOpenRecords.Checked || chkClosedRecords.Checked))
                    {
                        refreshData.Text = "Please select at least one request type.";
                        return;
                    }

                    bool isSucceed = true;

                    foreach (var module in selectedModules)
                    {
                        DataTable projects = new DataTable();
                        
                        if (chkOpenRecords.Checked && chkClosedRecords.Checked)
                            projects= ticketManager.GetAllTickets(module);
                        if (chkOpenRecords.Checked && !chkClosedRecords.Checked)
                            projects = ticketManager.GetOpenTickets(module);
                        if (!chkOpenRecords.Checked && chkClosedRecords.Checked)
                            projects = ticketManager.GetClosedTickets(module);

                        var projectRows = projects.AsEnumerable().Select(r => r.Field<string>(DatabaseObjects.Columns.TicketId));
                        isSucceed = ProcessProjects(tags, projectRows.ToList(), module, tagsToUpdate);
                        if (isSucceed)
                        {
                            refreshData.Text = $"All {module.ShortName} tags are processed successfully.";
                        }
                        else
                        {
                            refreshData.Text = $"Failed to process {module.ShortName} tags.";
                            break;
                        }
                    }
                    if (isSucceed)
                        uHelper.ClosePopUpAndEndResponse(Context, false);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
                refreshData.Text = $"Failed to process tags.";
            }
        }

        private bool ProcessProjects(List<ExperiencedTag> tags, List<string> ticketIDs, UGITModule module, bool tagsToUpdate)
        {
            bool isSucceed = true;

            DataTable dataTable = ticketManager.GetByTicketIDs(module, ticketIDs);
            if (!(UGITUtility.IfColumnExists(DatabaseObjects.Columns.Title, dataTable) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.Description, dataTable)
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.Comment, dataTable)
                && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TagMultiLookup, dataTable)))
            {
                refreshData.Text = "Please check colums: Title, Description, Comment, TagMultiLookup. available in tables.";
                isSucceed = false;
                return isSucceed;
            }

            ULog.WriteLog($"Creating projects tags for {dataTable.Rows.Count} {ModuleNames.CPR} projects");

            foreach (DataRow projectRow in dataTable.Rows)
            {
                string ticketID = Convert.ToString(projectRow[DatabaseObjects.Columns.TicketId]);

                List<ProjectTag> commonTags = experiencedTagManager.GetMatchingExperienceTags(tags, projectRow, tagsToUpdate);

                if (commonTags != null)
                {
                    ULog.WriteLog($" -- Updating project - {ticketID} - with {commonTags.Count} Experience tags.");
                    projectRow[DatabaseObjects.Columns.TagMultiLookup] = commonTags.Count == 0 ? null : JsonConvert.SerializeObject(commonTags);
                    var isSucceeded = ticketManager.Save(module, projectRow) > 0;

                    if (isSucceeded)
                    {
                        List<string> tagLookup = commonTags.Select(x => x.TagId)?.ToList() ?? null;
                        userProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, ticketID);

                        ULog.WriteLog($" -- Completed project - {ticketID} - Updated with {commonTags.Count} Experience tags.");
                    }
                }
            }
            #region Process Statistics

            ThreadStart threadStartProcessStatistics = delegate ()
            {
                statisticsManager.ProcessStatistics(ticketIDs, module);
            };
            Thread threadProcessStatistics = new Thread(threadStartProcessStatistics);
            threadProcessStatistics.IsBackground = true;
            threadProcessStatistics.Start();

            #endregion
            return isSucceed;
        }
    }
}