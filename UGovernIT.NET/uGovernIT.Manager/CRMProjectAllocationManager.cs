using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Threading;
using uGovernIT.Manager.RMM;
using uGovernIT.Helpers;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Util.Cache;
using System.Linq.Expressions;
using uGovernIT.DAL;
using Microsoft.Owin;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;
using uGovernIT.Manager.Core;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.CodeParser;
using Bond;
using DevExpress.Data.Browsing;
using static uGovernIT.Utility.SolarwindAssetModel;
using DevExpress.Office.Utils;
using DevExpress.XtraReports.Wizards.Native;
using Newtonsoft.Json;
using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DevExpress.Utils.DirectXPaint;

namespace uGovernIT.Manager
{
    public class ProjectEstimatedAllocationManager : ManagerBase<ProjectEstimatedAllocation>, ICRMProjectAllocation
    {
        private static bool IsProcessActive;

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private UserProfileManager _userManager = null;
        private ModuleViewManager _moduleManager = null;
        private GlobalRoleManager _roleManager = null;
        private ResourceUsageSummaryWeekWiseManager _resourceUsageSummaryWeekWiseMGR = null;
        public ProjectEstimatedAllocationManager(ApplicationContext context) : base(context)
        {
            _context = context;
            store = new CRMProjectAllocationStore(this.dbContext);
            _configurationVariableManager = new ConfigurationVariableManager(context);
            _ticketManager = new TicketManager(_context);
            _userManager = new UserProfileManager(_context);
            _moduleManager = new ModuleViewManager(_context);
            _roleManager = new GlobalRoleManager(_context);
            _resourceUsageSummaryWeekWiseMGR = new ResourceUsageSummaryWeekWiseManager(_context);
        }

        public bool ProcessState()
        {
            return IsProcessActive;
        }
        public static ProjectEstimatedAllocation DeepCopy(ProjectEstimatedAllocation projectEstimatedAllocation)
        {
            return new ProjectEstimatedAllocation()
            {
                ID = projectEstimatedAllocation.ID,
                AllocationEndDate = projectEstimatedAllocation.AllocationEndDate,
                AllocationStartDate = projectEstimatedAllocation.AllocationStartDate,
                AssignedTo = projectEstimatedAllocation.AssignedTo,
                Duration = projectEstimatedAllocation.Duration,
                ItemOrder = projectEstimatedAllocation.ItemOrder,
                PctAllocation = projectEstimatedAllocation.PctAllocation,
                Title = projectEstimatedAllocation.Title,
                Type = projectEstimatedAllocation.Type,
                TicketId = projectEstimatedAllocation.TicketId,
                SoftAllocation = projectEstimatedAllocation.SoftAllocation,
                NonChargeable = projectEstimatedAllocation.NonChargeable,
                IsLocked = projectEstimatedAllocation.IsLocked
            }; 
        }
        public void RefreshProjectComplexity()
        {
            if (IsProcessActive)
                return;

            try
            {
                IsProcessActive = true;

                List<string> lstmodules = new List<string> { "CPR", "OPM", "CNS", "PMM", "NPR" };
                foreach (string module in lstmodules)
                {
                    RefreshProjectComplexity(module);
                }
                IsProcessActive = false;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            finally
            {
                IsProcessActive = false;
            }
        }


        public void RefreshProjectComplexity(string module, List<string> userLists = null)
        {
            try
            {
                if (userLists != null)
                    userLists = userLists.Select(x => x.ToLower()).ToList();

                TicketManager ticketManager = new TicketManager(_context);
                UserProfileManager userProfileManager = new UserProfileManager(_context);
                ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(_context);
                ModuleViewManager moduleManager = new ModuleViewManager(_context);

                List<SummaryResourceProjectComplexity> allComplexities = complexityManager.Load(x => x.ModuleNameLookup == module && (userLists == null || userLists.Contains(x.UserId.ToLower())));
                List<string> complexityUList = allComplexities.Select(x => x.UserId.ToLower()).Distinct().ToList();
                List<ProjectEstimatedAllocation> lstCrmAllocations = Load(x => x.TicketId.ToLower().StartsWith(module.ToLower()) && x.AssignedTo != null && (userLists == null || userLists.Contains(x.AssignedTo.ToLower())));

                UGITModule moduleObj = moduleManager.LoadByName(module);
                DataTable dtAllTickets = null;
                if (moduleObj != null)
                    dtAllTickets = ticketManager.GetAllTickets(moduleObj);
                //DataTable dtOpenTickets = dtAllTickets.Select($"{DatabaseObjects.Columns.Closed} <> 1 ").CopyToDataTable();  //ticketManager.GetOpenTickets(moduleObj);

                DataTable dtOpenTickets = new DataTable();
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Closed, dtAllTickets) && dtAllTickets.Rows.Count > 0)
                    dtOpenTickets = dtAllTickets.Select($"{DatabaseObjects.Columns.Closed} <> 1 ").CopyToDataTable();

                string complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectComplexity, dtAllTickets))
                    complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CRMProjectComplexity, dtAllTickets))
                {
                    complexityColumnName = DatabaseObjects.Columns.CRMProjectComplexity;
                }
                else
                    return;
                if (dtOpenTickets == null || dtOpenTickets.Rows.Count == 0)
                {
                    if (allComplexities != null && allComplexities.Count > 0)
                        complexityManager.Delete(allComplexities);
                    return;
                }

                List<string> usersWithAllocation = lstCrmAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                ILookup<string, ProjectEstimatedAllocation> lookupallocation = lstCrmAllocations.ToLookup(x => x.AssignedTo);

                foreach (var itemuserid in lookupallocation)
                {
                    //removed processed user from exiting summary list
                    complexityUList.Remove(itemuserid.Key.ToLower());

                    UserProfile user = null;
                    if (itemuserid.Key == Guid.Empty.ToString())
                    {
                        user = new UserProfile() { Id = Guid.Empty.ToString(), Name = "Unfilled Roles", UserName = "UnfilledRoles", isRole = false, Enabled = true, TenantID = _context.TenantID };
                    }
                    else
                        user = userProfileManager.LoadById(itemuserid.Key);

                    if (user == null)
                        continue;

                    List<string> userCRMAllocations = itemuserid.ToArray().Select(x => $"'{x.TicketId}'").Distinct().ToList();
                    List<SummaryResourceProjectComplexity> userComplexities = allComplexities.Where(x => x.UserId.ToLower() == user.Id.ToLower()).ToList();

                    DataRow[] ticketRows = dtOpenTickets.Select("TicketId IN (" + string.Join(Constants.Separator6, userCRMAllocations) + ")");
                    if (ticketRows.Length == 0)
                    {
                        if (userComplexities != null && userComplexities.Count > 0)
                            complexityManager.Delete(userComplexities);
                        continue;
                    }

                    var complexityGroups = ticketRows.GroupBy(x => x.Field<string>(complexityColumnName));

                    //var firstComplexity = complexityGroups.Where(x => !x.Key.HasValue || x.Key.Value == 0 || x.Key.Value == 1);
                    //complexityGroups = complexityGroups.Where(x => x.Key.HasValue && x.Key.Value != 0 && x.Key.Value != 1);

                    var firstComplexity = complexityGroups.Where(x => x.Key == "0" || x.Key == "1");
                    complexityGroups = complexityGroups.Where(x => x.Key != "0" && x.Key != "1");

                    //var firstComplexity = complexityGroups.Where(x => !string.IsNullOrEmpty(x.Key));
                    //complexityGroups = complexityGroups.Where(x => !string.IsNullOrEmpty(x.Key));
                    List<string> processedCpx = new List<string>();


                    if (firstComplexity.Count() > 0)
                    {
                        processedCpx.Add("1");
                        var firstComplexityCost = firstComplexity.Select(x => x.Where(y => !y.IsNull("ApproxContractValue")).Sum(y => y.Field<double>("ApproxContractValue"))).Sum();
                        var firstComplexityCount = firstComplexity.Select(x => x.Count()).Sum();
                        var firstComplexityRq = string.Join(Constants.Separator6, firstComplexity.Select(x => string.Join(Constants.Separator6, x.Where(y => !y.IsNull("RequestTypeLookup")).Select(y => y.Field<long>("RequestTypeLookup")))));
                        string[] reqTypes = firstComplexityRq.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        firstComplexityRq = string.Join(Constants.Separator6, reqTypes.Distinct());

                        saveComplexity(user, "1", firstComplexityCount, firstComplexityCost, firstComplexityRq);
                    }


                    foreach (var cg in complexityGroups)
                    {

                        processedCpx.Add(cg.Key);

                        DataRow[] cRows = cg.ToArray();
                        double totalProjectCost = cRows.Where(x => !x.IsNull("ApproxContractValue")).Sum(x => x.Field<double>("ApproxContractValue"));
                        var requestypesvalues = string.Join(Constants.Separator6, cRows.Where(x => !x.IsNull("RequestTypeLookup")).Select(x => x.Field<long>("RequestTypeLookup")));
                        string[] reqTypes = requestypesvalues.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        requestypesvalues = string.Join(Constants.Separator6, reqTypes.Distinct());

                        int count = cRows.Count();
                        saveComplexity(user, cg.Key, count, totalProjectCost, requestypesvalues);
                    }

                    //remove summary data which is not required any more
                    //if (userComplexities != null && userComplexities.Count > 0)
                    //{
                    //    if (userComplexities.Where(x => !processedCpx.Contains(UGITUtility.ObjectToString(x.Complexity))).Count() > 0)
                    //        complexityManager.Delete(userComplexities.Where(x => !processedCpx.Contains(UGITUtility.ObjectToString(x.Complexity))).ToList());
                    //}
                    
                    ///////////////////////////////////////////////////////////
                    // Code to store All Records (Open & Closed) Complexity
                    DataRow[] AllTicketRows = dtAllTickets.Select("TicketId IN (" + string.Join(Constants.Separator6, userCRMAllocations) + ")");

                    complexityGroups = AllTicketRows.GroupBy(x => x.Field<string>(complexityColumnName));

                    ////firstComplexity = complexityGroups.Where(x => !string.IsNullOrWhiteSpace(x.Key));
                    ////complexityGroups = complexityGroups.Where(x => !string.IsNullOrWhiteSpace(x.Key));

                    //firstComplexity = complexityGroups.Where(x => !x.Key.HasValue || x.Key.Value == 0 || x.Key.Value == 1);
                    //complexityGroups = complexityGroups.Where(x => x.Key.HasValue && x.Key.Value != 0 && x.Key.Value != 1);

                    firstComplexity = complexityGroups.Where(x => x.Key == "0" || x.Key == "1");
                    complexityGroups = complexityGroups.Where(x => x.Key != "0" && x.Key != "1");

                    //List<int> processedCpx = new List<int>();
                    processedCpx.Clear();
                    if (firstComplexity != null)
                    {

                        if (firstComplexity.Count() > 0)
                        {
                            processedCpx.Add("1");
                            var firstComplexityCost = firstComplexity.Select(x => x.Where(y => !y.IsNull("ApproxContractValue")).Sum(y => y.Field<double>("ApproxContractValue"))).Sum();
                            var firstComplexityCount = firstComplexity.Select(x => x.Count()).Sum();
                            var firstComplexityRq = string.Join(Constants.Separator6, firstComplexity.Select(x => string.Join(Constants.Separator6, x.Where(y => !y.IsNull("RequestTypeLookup")).Select(y => y.Field<long>("RequestTypeLookup")))));
                            string[] reqTypes = firstComplexityRq.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                            firstComplexityRq = string.Join(Constants.Separator6, reqTypes.Distinct());
                            saveComplexity(user, "1", firstComplexityCount, firstComplexityCost, firstComplexityRq, true);
                        }

                    }
                    foreach (var cg in complexityGroups)
                    {

                        processedCpx.Add(cg.Key);

                        DataRow[] cRows = cg.ToArray();
                        double totalProjectCost = cRows.Where(x => !x.IsNull("ApproxContractValue")).Sum(x => x.Field<double>("ApproxContractValue"));
                        var requestypesvalues = string.Join(Constants.Separator6, cRows.Where(x => !x.IsNull("RequestTypeLookup")).Select(x => x.Field<long>("RequestTypeLookup")));
                        string[] reqTypes = requestypesvalues.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        requestypesvalues = string.Join(Constants.Separator6, reqTypes.Distinct());

                        int count = cRows.Count();
                        saveComplexity(user, cg.Key, count, totalProjectCost, requestypesvalues, true);
                    }

                    //remove summary data which is not required any more
                    if (userComplexities != null && userComplexities.Count > 0)
                    {
                        if (userComplexities.Where(x => !processedCpx.Contains(UGITUtility.ObjectToString(x.Complexity))).Count() > 0)
                            complexityManager.Delete(userComplexities.Where(x => !processedCpx.Contains(UGITUtility.ObjectToString(x.Complexity))).ToList());
                    }
                }

                //remove summary data which is not required any more
                allComplexities = allComplexities.Where(x => complexityUList.Contains(x.UserId.ToLower())).ToList();
                if (allComplexities != null && allComplexities.Count > 0)
                {
                    ULog.AuditTrail(string.Format("ProjectEstimatedAllocationManager >> RefreshProjectComplexity method called"));
                    complexityManager.Delete(allComplexities);
                }
                void saveComplexity(UserProfile user, string complexityKey, int count, double volume, string requesttypes, bool allItems = false)
                {
                    SummaryResourceProjectComplexity projectComplexity = complexityManager.Get(x => x.UserId == user.Id && UGITUtility.ObjectToString(x.Complexity) == complexityKey && x.ModuleNameLookup == module);
                    //List<Role> userRoles = userProfileManager.GetUserRoles(user.Id);
                    if (projectComplexity != null && projectComplexity.ID > 0)
                    {
                        //projectComplexity.Count = count;
                        projectComplexity.UserName = user.Name;
                        projectComplexity.Manager = user.ManagerID;
                        projectComplexity.FunctionalAreaID = user.FunctionalArea;

                        // allItems - denotes both OPen & Closed Projects
                        if (allItems == false)
                        {
                            projectComplexity.Count = count;
                            projectComplexity.HighProjectCapacity = volume;
                        }
                        else
                        {
                            projectComplexity.AllCount = count;
                            projectComplexity.AllHighProjectCapacity = volume;
                        }

                        projectComplexity.GroupID = user.GlobalRoleId;
                        projectComplexity.RequestTypes = requesttypes;
                        projectComplexity.ModuleNameLookup = module;
                        if (!string.IsNullOrEmpty(user.Department))
                            projectComplexity.DepartmentID = Convert.ToInt64(user.Department);
                        //if (userRoles != null && userRoles.Count > 0)
                        //    projectComplexity.GroupID = string.Join(Constants.Separator6, userRoles.Select(x => x.Id));
                        complexityManager.Update(projectComplexity);
                    }
                    else
                    {
                        projectComplexity = new SummaryResourceProjectComplexity();
                        projectComplexity.UserId = user.Id;
                        projectComplexity.UserName = user.Name;
                        projectComplexity.Manager = user.ManagerID;
                        projectComplexity.GroupID = user.GlobalRoleId;
                        projectComplexity.FunctionalAreaID = user.FunctionalArea;

                        if (allItems == false)
                        {
                            projectComplexity.Count = count;
                            projectComplexity.HighProjectCapacity = volume;
                        }
                        else
                        {
                            projectComplexity.AllCount = count;
                            projectComplexity.AllHighProjectCapacity = volume;
                        }

                        if (!string.IsNullOrEmpty(user.Department))
                            projectComplexity.DepartmentID = Convert.ToInt64(user.Department);
                        projectComplexity.Complexity = UGITUtility.StringToInt(complexityKey);
                        //projectComplexity.Count = count;
                        projectComplexity.RequestTypes = requesttypes;
                        projectComplexity.ModuleNameLookup = module;
                        projectComplexity.GroupID = user.GlobalRoleId;
                        //if (userRoles != null && userRoles.Count > 0)
                        //    projectComplexity.GroupID = string.Join(Constants.Separator6, userRoles.Select(x => x.Id));
                        complexityManager.Insert(projectComplexity);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public void RefreshProjectComplexityForProject(string moduleName, string ticketID)
        {
            try
            {
                List<ProjectEstimatedAllocation> projectAllocations = this.Load(x => x.TicketId == ticketID).ToList();
                if (projectAllocations != null && projectAllocations.Count > 0)
                {
                    List<string> allocatedUsers = projectAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                    if (allocatedUsers.Count > 0)
                    {
                        this.RefreshProjectComplexity(moduleName, allocatedUsers);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

        }

        public static ProjectAllocationTemplate GetTemplateAllocations(ApplicationContext context, long template)
        {
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);

            if (template > 0)
            {
                ProjectAllocationTemplate projAllocTemplate = projectAllocationTemplateMGR.LoadByID(template);
                if (projAllocTemplate != null)
                {
                    return projAllocTemplate;
                }
            }
            return null;

        }

        public void UpdateProjectStartAndEndDate(ApplicationContext context, DateTime pStartDate, DateTime pEndDate, string pProjectID)
        {
            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = moduleManager.LoadByName(uHelper.getModuleNameByTicketId(pProjectID));
            DataRow projectTicket = ticketManager.GetByTicketID(module, pProjectID);
            DateTime oldStartDate = new DateTime();
            DateTime oldEndDate = new DateTime();
            DateTime oldEstConstStartDate = new DateTime();

            if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.PreconStartDate))
                oldStartDate = UGITUtility.StringToDateTime(projectTicket[DatabaseObjects.Columns.PreconStartDate]);
            if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                oldEndDate = UGITUtility.StringToDateTime(projectTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.EstimatedConstructionStart))
                oldEstConstStartDate = UGITUtility.StringToDateTime(projectTicket[DatabaseObjects.Columns.EstimatedConstructionStart]);


            if (oldStartDate != pStartDate && pStartDate != DateTime.MinValue)
            {
                if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.PreconStartDate))
                    projectTicket[DatabaseObjects.Columns.PreconStartDate] = pStartDate;
                if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.TicketTargetStartDate))
                    projectTicket[DatabaseObjects.Columns.TicketTargetStartDate] = pStartDate;
            }

            if (oldEndDate != pEndDate && pEndDate != DateTime.MinValue)
            {
                if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    projectTicket[DatabaseObjects.Columns.EstimatedConstructionEnd] = pEndDate;
                if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.TicketTargetCompletionDate))
                    projectTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] = pEndDate;
            }

            if (oldEstConstStartDate != null && oldEstConstStartDate.Date > DateTime.MinValue)
            {
                int days = uHelper.GetTotalWorkingDaysBetween(context, oldEstConstStartDate, pEndDate);
                int duration = uHelper.GetWeeksFromDays(context, days);

                if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.CRMDuration))
                    projectTicket[DatabaseObjects.Columns.CRMDuration] = duration;
                else if (UGITUtility.IsSPItemExist(projectTicket, DatabaseObjects.Columns.Duration))
                    projectTicket[DatabaseObjects.Columns.Duration] = duration;
                //projectTicket[DatabaseObjects.Columns.Duration] = duration;
            }

            int result = ticketManager.Save(module, projectTicket);
        }

        public void ImportAllocation(ApplicationContext context, List<AllocationTemplateModel> model, DateTime pStartDate, DateTime pEndDate, bool deleteExistingAllocations = true)
        {
            ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            UserProjectExperienceManager ObjUserProjectExperienceManager = new UserProjectExperienceManager(context);
            if (model != null && model.Count > 0)
            {
                List<string> historyDesc = new List<string>();
                string projectID = model.FirstOrDefault().ProjectID;

                //update ticket enddates and startdates based on new entered values 
                UpdateProjectStartAndEndDate(context, pStartDate, pEndDate, projectID);

                //model = model.FindAll(x => x.AssignedTo != null && x.AssignedTo != string.Empty);

                model.ForEach(
                   x =>
                   {
                       if (string.IsNullOrEmpty(x.AssignedTo))
                       {
                           x.AssignedTo = Guid.Empty.ToString();
                           x.AssignedToName = "Unassigned";
                       }
                   });

                // Current Ticket Start Date, End Date
                //Delete existing allocations
                List<string> oldAllocatedUsers = new List<string>();
                if (deleteExistingAllocations)
                {
                    List<ProjectEstimatedAllocation> dbCRMAllocations = Load(x => x.TicketId == projectID);
                    if (dbCRMAllocations != null && dbCRMAllocations.Count > 0)
                    {
                        RMMSummaryHelper.DeleteAllCRMAllocations(context, projectID);
                        oldAllocatedUsers = dbCRMAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        GlobalRole uRole = null;
                        foreach (var item in dbCRMAllocations)
                        {
                            string userName = context.UserManager.GetUserNameById(item.AssignedTo);
                            uRole = roleManager.Get(x => x.Id == item.Type);
                            string userRole = string.Empty;
                            if (uRole != null)
                                userRole = uRole.Name;

                            historyDesc.Add(string.Format("Allocation removed for user: {0} - {1} {2}% {3}-{4}", string.IsNullOrEmpty(userName) ? "Unassigned" : userName, userRole, item.PctAllocation, String.Format("{0:MM/dd/yyyy}", item.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", item.AllocationEndDate)));
                        }
                    }
                }

                //Add new allocation based on template
                List<ProjectEstimatedAllocation> crmAllocations = new List<ProjectEstimatedAllocation>();
                //List<CRMProjectAllocation> templateAllocs = (List<CRMProjectAllocation>)Newtonsoft.Json.JsonConvert.DeserializeObject(projAllocTemplate.Template, typeof(List<CRMProjectAllocation>));
                foreach (AllocationTemplateModel tempAlloc in model)
                {
                    ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                    crmAllocation.AllocationStartDate = tempAlloc.AllocationStartDate;
                    crmAllocation.AllocationEndDate = tempAlloc.AllocationEndDate;

                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                    int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);

                    crmAllocation.AssignedTo = tempAlloc.AssignedTo;
                    crmAllocation.PctAllocation = tempAlloc.PctAllocation;
                    crmAllocation.Type = tempAlloc.Type;
                    crmAllocation.Duration = noOfWeeks;
                    crmAllocation.Title = tempAlloc.Title;
                    crmAllocation.TicketId = projectID;
                    crmAllocation.SoftAllocation = tempAlloc.SoftAllocation;
                    crmAllocation.NonChargeable = tempAlloc.NonChargeable;
                    Insert(crmAllocation);
                    crmAllocations.Add(crmAllocation);
                    historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", string.IsNullOrEmpty(tempAlloc.AssignedToName) ? "Unassigned" : tempAlloc.AssignedToName, tempAlloc.TypeName, tempAlloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationEndDate)));

                    lstUserWithPercetage.Add(new UserWithPercentage() { 
                        EndDate = tempAlloc.AllocationEndDate ?? DateTime.MinValue,
                        StartDate = tempAlloc.AllocationStartDate ?? DateTime.MinValue,
                        Percentage = tempAlloc.PctAllocation, 
                        RoleTitle = tempAlloc.TypeName,
                        UserId = tempAlloc.AssignedTo,
                        ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID)
                    });
                }

                if (!string.IsNullOrWhiteSpace(projectID) && UGITUtility.IsValidTicketID(projectID))
                {
                    List<string> tagLookup = ObjUserProjectExperienceManager.GetProjectExperienceTags(projectID, false)?.Select(x => x.TagId)?.ToList() ?? null;
                    ObjUserProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, projectID);
                }

                //to-do
                var taskManager = new UGITTaskManager(context);
                List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(projectID), projectID);
                List<string> lstUsers = crmAllocations.Select(a => a.AssignedTo).ToList();
                var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                // Only create allocation enties if user is not in schedule
                if (res == null || res.Count == 0)
                {
                    ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(projectID), projectID, lstUserWithPercetage, oldAllocatedUsers); ResourceAllocationManager.UpdateHistory(context, historyDesc, projectID); historyDesc.ForEach(o => { ULog.WriteLog(o); }); };
                    Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                    sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                    sThreadStartMethodUpdateCPRProjectAllocation.Start();
                }
            }
        }

        public List<FindResourceResponse> FindResourceBasedOnGroup(ApplicationContext context, FindResourceRequest request)
        {
            //all required managers initialization
            ResourceUsageSummaryWeekWiseManager weekwiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            UserProfileManager userManager = new UserProfileManager(context);
            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(context);
            ResourceAllocationMonthlyManager monthlyManager = new ResourceAllocationMonthlyManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            TicketManager ticketManagerr = new TicketManager(context);
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
            List<FindResourceResponse> lstResponse = new List<FindResourceResponse>();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<GlobalRole> globalRoles = roleManager.Load(x => x.TenantID == context.TenantID);
            DepartmentManager departmentManager = new DepartmentManager(context);
            try
            {
                //load overlap resource allocation between requested date range
                //feteching selected row group role obj and then selecting users based on role name 
                List<UserProfile> lstUProfile = new List<UserProfile>();
                //select only one usser if selecteduserid have value mean user forces to select any user.
                if (!string.IsNullOrEmpty(request.SelectedUserID))
                    lstUProfile = userManager.Load(x => x.Id == request.SelectedUserID && x.Enabled);
                else if (!string.IsNullOrEmpty(request.GroupID))
                    lstUProfile = userManager.Load(x => x.Enabled && x.GlobalRoleId.EqualsIgnoreCase(request.GroupID));
                else
                    lstUProfile = userManager.Load(x => x.Enabled);

                #region load allocation from resource allocation
                List<RResourceAllocation> lstAllOverLappingAlloc = new List<RResourceAllocation>();
                if (!string.IsNullOrEmpty(request.SelectedUserID) && lstUProfile != null && lstUProfile.Count == 1)
                    lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(request.AllocationStartDate.Date, request.AllocationEndDate.Date, lstUProfile.FirstOrDefault().Id);
                else
                    lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                #endregion

                List<string> selectedUsers = new List<string>();
                int projectComplexity = 0;
                //loading current cpr ticket
                string moduleName = uHelper.getModuleNameByTicketId(request.ProjectID);
                UGITModule module = moduleManager.LoadByName(moduleName);

                string complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectComplexity, module.ModuleTable))
                    complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CRMProjectComplexity, module.ModuleTable))
                    complexityColumnName = DatabaseObjects.Columns.CRMProjectComplexity;

                DataRow projectRow = ticketManagerr.GetByTicketID(module, request.ProjectID, new List<string>() { complexityColumnName, DatabaseObjects.Columns.TicketRequestTypeLookup });

                if ((request.Complexity || request.ProjectVolume || request.ProjectCount) && UGITUtility.IfColumnExists(projectRow, complexityColumnName))
                {

                    projectComplexity = UGITUtility.StringToInt(Convert.ToString(projectRow[complexityColumnName]));
                    List<UserProfile> lstComplexityUsers = new List<UserProfile>();
                    List<SummaryResourceProjectComplexity> complexityAboveCurrentCRP = complexityManager.Load(x => x.Complexity >= projectComplexity);
                    if (complexityAboveCurrentCRP != null && complexityAboveCurrentCRP.Count > 0)
                        lstComplexityUsers = lstUProfile.FindAll(x => complexityAboveCurrentCRP.Any(y => y.UserId == x.Id)).ToList();

                    //filtering only those users which have complexity saved in summary_resourceprojectcoomplexity table bcz project count and revenue count depends on summary table only
                    if (request.Complexity)
                    {
                        selectedUsers = lstComplexityUsers.Select(x => x.Id).Distinct().ToList();
                    }
                }

                if (request.RequestTypes)
                {
                    List<UserProfile> lstRequestTypeUsers = new List<UserProfile>();
                    string requestType = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                    List<SummaryResourceProjectComplexity> complexityWithRequestTypes = complexityManager.Load(x => x.RequestTypes.Contains(requestType));
                    List<string> userProfilesWithRequestTypes = complexityWithRequestTypes.Select(x => x.UserId).Distinct().ToList();
                    selectedUsers = selectedUsers.Intersect(userProfilesWithRequestTypes).ToList();
                }

                if (!request.Complexity)
                {
                    selectedUsers = lstUProfile.Select(x => x.Id).Distinct().ToList();
                }

                List<JobTitle> jobTitles = new List<JobTitle>();
                if (!string.IsNullOrEmpty(request.JobTitles))
                {
                    List<string> lstjobtitles = UGITUtility.ConvertStringToList(request.JobTitles, Constants.Separator);
                    jobTitles = jobTitleManager.Load(x => lstjobtitles.Contains(x.Title));
                }

                List<UserProfile> selectedUserProfiles = userManager.GetUserInfosById(UGITUtility.ConvertListToString(selectedUsers, Constants.Separator6));
                if (jobTitles.Count > 0)
                {
                    // filter based on job title.
                    List<string> lstjobtitles = jobTitles.Select(x => Convert.ToString(x.ID)).ToList();
                    selectedUserProfiles = selectedUserProfiles.FindAll(x => lstjobtitles.Contains(Convert.ToString(x.JobTitleLookup)));
                }
                if (request.departments > 0)
                {
                    // filter based on department selected.
                    selectedUserProfiles = selectedUserProfiles.Where(o => o.Department == request.departments.ToString()).ToList();
                }
                if(!string.IsNullOrEmpty(request.DivisionId) && UGITUtility.StringToInt(request.DivisionId) !=0) 
                {
                    List<string> lstDepartments = departmentManager.Load(x => request.DivisionId.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Contains(UGITUtility.ObjectToString(x.DivisionIdLookup))
                    && !x.Deleted).Select(x => UGITUtility.ObjectToString(x.ID)).ToList();
                    selectedUserProfiles = selectedUserProfiles.FindAll(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.Department)));
                }
                selectedUsers =  selectedUserProfiles.Select(x => x.Id).ToList() ?? null;

                DataTable dtTicketIds = new DataTable();
                if (request.Customer == true && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == true && request.Sector == false)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == false && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }

                if (dtTicketIds != null && dtTicketIds.Rows.Count > 0)
                {
                    List<string> LstOpenTicketIds = new List<string>();
                    LstOpenTicketIds.AddRange(dtTicketIds.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));

                    if (selectedUsers != null && selectedUsers.Count > 0)
                    {
                        List<string> users = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.TicketID.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                        selectedUsers = selectedUsers.Intersect(users).ToList();
                    }
                    else
                    {
                        selectedUsers = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.TicketID.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                    }
                }


                UserProfile profile = null;
                JobTitle jobTitle = null;
                List<RResourceAllocation> lstUserAllocation = null;
                List<SummaryResourceProjectComplexity> userComplexities = new List<SummaryResourceProjectComplexity>();

                int noOfWorkingHoursPerDay = uHelper.GetWorkingHoursInADay(_context);
                int noOfWorkingDaysPerWeek = uHelper.GetWorkingDaysInWeeks(_context, 1);
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_context, request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                //int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);

                //selectedUsers = selectedUsers.Where(x => x == "f4d20c1f-9915-41c4-b344-8991e24801dc").ToList();
                //code to create response object only for those users who have complexity saved
                //calculat allocation based on following logic
                // if we have 3 weeks from start 4th to 15th then
                // (w1*4 + w2*7 + w3*4) / (4+7+4)
                //selectedUsers =new List<string>() { selectedUsers.FirstOrDefault(x => x.EqualsIgnoreCase("44380d17-c887-488c-856b-31753e4197b7")) };
                UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);

                List<ProjectTag> projectTags = null;
                List<ProjectTag> certificates = null;
                if (uHelper.IsExperienceTagAllowGroupFilter(_context) && request.SelectedTags != null && request.SelectedTags.Count() > 0)
                {
                    projectTags = request.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Experience).ToList();
                    certificates = request.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Certificate).ToList();
                }

                foreach (string uProfile in selectedUsers)
                {
                    profile = lstUProfile.FirstOrDefault(x => x.Id == uProfile);

                    if (profile == null)
                        continue;
                    
                    FindResourceResponse response = new FindResourceResponse();
                    response.AssignedTo = profile.Id;
                    response.AssignedToName = profile.Name;
                    response.GroupID = profile.GlobalRoleId;   // request.GroupID;
                    response.JobTitle = profile.JobProfile;
                    response.UserImagePath = profile.Picture;
                    
                    GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == profile.GlobalRoleId);
                    if (typeGroup != null)
                    {
                        response.RoleName = typeGroup.Name;
                    }

                    if (certificates != null && certificates.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(profile.UserCertificateLookup))
                        {
                            continue;
                        }
                        else 
                        {
                            bool resourceSelected = true;
                            List<string> userCertificates = profile.UserCertificateLookup.Split(',').ToList();
                            certificates.ForEach(x => {
                                if (userCertificates.Contains(x.TagId))
                                {
                                    if (response.ResourceTags == null)
                                    {
                                        response.ResourceTags = new List<ResourceTag>();
                                    }
                                    response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = "Y", Type = TagType.Certificate });
                                }
                                else {
                                    resourceSelected = false;
                                }
                            });
                            if (!resourceSelected)
                            {
                                continue;
                            }
                        }
                    }

                    if (projectTags != null && projectTags.Count() > 0)
                    {
                        List<UserProjectExperience> userProjectExperiences = userProjectExperienceMGR.Load(x => !string.IsNullOrWhiteSpace(x.ProjectID) && x.UserId == profile.Id);
                        bool resourceSelected = true;
                        projectTags.ForEach(x =>
                        {
                            int userTagCount = userProjectExperiences.Where(y => y.TagLookup.ToString() == x.TagId).Count();
                            if (userTagCount >= x.MinValue)
                            {
                                if (response.ResourceTags == null)
                                {
                                    response.ResourceTags = new List<ResourceTag>();
                                }
                                response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = userTagCount.ToString(), Type = TagType.Experience });
                            }
                            else
                            {
                                resourceSelected = false;
                            }

                        });
                        if (!resourceSelected)
                        {
                            continue;
                        }
                    }
                    //selecting monthlly allocation of given user only
                    //assigning average pct allocation on response object
                    double? totalPercentAllocatedInRange = 0;
                    double? hardPercentAllocatedInRange = 0;
                    double? softPercentAllocatedInRange = 0;
                    //User related allocation
                    lstUserAllocation = lstAllOverLappingAlloc.Where(x => x.Resource == profile.Id && x.ProjectEstimatedAllocationId != null).OrderBy(x => x.AllocationStartDate).ToList();
                    if (lstUserAllocation != null && lstUserAllocation.Count > 0)
                    {
                        List<DateTime> lstOfStartDate = null;
                        List<DateTime> lstOfEndDate = null;
                        double? totalAllocOverlapDays = 0;
                        double? hardAllocOverlapDays = 0;
                        double? softAllocOverlapDays = 0;

                        foreach (RResourceAllocation ralloc in lstUserAllocation)
                        {
                            //To get overlapping days (Max start date and Min end date)
                            lstOfStartDate = new List<DateTime>() { request.AllocationStartDate, ralloc.AllocationStartDate.Value };
                            lstOfEndDate = new List<DateTime>() { request.AllocationEndDate, ralloc.AllocationEndDate.Value };
                            DateTime maxStartDate = lstOfStartDate.Max();
                            DateTime minEndDate = lstOfEndDate.Min();
                            //Overlap days based on max start date and min end date
                            double workingDays = uHelper.GetTotalWorkingDaysBetween(_context, maxStartDate, minEndDate);
                            double? pctAlloc = 0;
                            if (ralloc.PctAllocation.HasValue)
                                pctAlloc = ralloc.PctAllocation / 100;
                            //Get allocation overlap percentage
                            //double? allocOverlapPct = (workingDays / searchPeriodDays) * pctAlloc;
                            // If more than one allocation user has 
                            totalAllocOverlapDays += workingDays * pctAlloc;
                            if (ralloc.SoftAllocation)
                                softAllocOverlapDays += workingDays * pctAlloc;
                            else
                                hardAllocOverlapDays += workingDays * pctAlloc;
                        }

                        //allocation in given date range 
                        totalPercentAllocatedInRange = (totalAllocOverlapDays / searchPeriodDays) * 100;
                        softPercentAllocatedInRange = (softAllocOverlapDays / searchPeriodDays) * 100;
                        hardPercentAllocatedInRange = (hardAllocOverlapDays / searchPeriodDays) * 100;
                    }


                    response.TotalPctAllocation = 0;
                    response.PctAllocation = 0;
                    response.SoftPctAllocation = 0;
                    if (totalPercentAllocatedInRange > 0)
                        response.TotalPctAllocation = Math.Round(totalPercentAllocatedInRange.Value, 1);
                    if (softPercentAllocatedInRange > 0)
                        response.SoftPctAllocation = Math.Round(softPercentAllocatedInRange.Value, 1);
                    if (totalPercentAllocatedInRange > 0)
                        response.PctAllocation = Math.Round(hardPercentAllocatedInRange.Value, 1);

                    //BTS-22-000946: Changed the tile color as per the BTS
                    if (response.TotalPctAllocation < 80)
                        response.AllocationRange = 0;
                    else if (response.TotalPctAllocation >= 80 && response.TotalPctAllocation <= 99)
                        response.AllocationRange = 1;
                    else if (response.TotalPctAllocation >= 100 && response.TotalPctAllocation <= 110)
                        response.AllocationRange = 2;
                    else if (response.TotalPctAllocation > 110)
                        response.AllocationRange = 3;

                    //list of complexity to count highest complexity, totalreveneu and project count for each user
                    //List<SummaryResourceProjectComplexity> userComplexities = new List<SummaryResourceProjectComplexity>();
                    userComplexities = complexityManager.Load(x => x.UserId == profile.Id);

                    //capacity code only needed if complexity exits for that user
                    if (userComplexities != null && userComplexities.Count > 0)
                    {
                        response.HighestComplexity = userComplexities.Max(x => x.Complexity);
                        //find project count
                        //JobTitle jobTitle = null;
                        if (request.ProjectCount)
                        {
                            response.ProjectCount = userComplexities.Sum(x => x.Count);
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            response.TotalVolumeRange = 0;
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowProjectCapacity <= 0 && jobTitle.HighProjectCapacity <= 0) || response.ProjectCount <= jobTitle.LowProjectCapacity)
                                    response.projectCountRange = 0;
                                else if (response.ProjectCount > jobTitle.HighProjectCapacity)
                                    response.projectCountRange = 2;
                                else
                                    response.projectCountRange = 1;
                            }
                        }

                        if (request.ProjectVolume)
                        {
                            double cost = 0;
                            cost = userComplexities.Sum(x => x.HighProjectCapacity);

                            response.TotalVolume = UGITUtility.FormatNumber(cost, "currency");
                            response.TotalVolumeRange = 0;
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowRevenueCapacity <= 0 && jobTitle.HighRevenueCapacity <= 0) || cost <= jobTitle.LowRevenueCapacity)
                                    response.TotalVolumeRange = 0;
                                else if (cost > jobTitle.HighRevenueCapacity)
                                    response.TotalVolumeRange = 2;
                                else
                                    response.TotalVolumeRange = 1;
                            }
                        }
                    }
                    lstResponse.Add(response);
                }

                List<FindResourceResponse> lstResponseAvailability = new List<FindResourceResponse>();
                if (request.isAllocationView == false && request.ResourceAvailability != ResourceAvailabilityType.AllResource)
                {

                    foreach (FindResourceResponse responseAvailability in lstResponse)
                    {
                        if (responseAvailability.PctAllocation <= 100)
                        {
                            lstResponseAvailability.Add(responseAvailability);
                        }
                    }
                    lstResponse = lstResponseAvailability;
                }


                //set ordering in last in descending order and asc for pct allocation
                if (request.ResourceAvailability == ResourceAvailabilityType.FullyAvailable)
                    lstResponse = lstResponse.Where(x => x.PctAllocation == 0).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                else if (request.ResourceAvailability == ResourceAvailabilityType.PartiallyAvailable)
                    lstResponse = lstResponse.Where(x => x.PctAllocation >= 0).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                else
                    lstResponse = lstResponse.OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();

                //if (!string.IsNullOrEmpty(request.SelectedTags.Select(x => x.TagId).Aggregate((x,y) => x + "," + y)))
                //{
                //    var userIds = string.Join(",", lstResponse.Select(x => x.AssignedTo));
                //    Dictionary<string, object> valuesUPE = new Dictionary<string, object>();
                //    valuesUPE.Add("@TenantID", profile.TenantID);
                //    valuesUPE.Add("@UserId", userIds);
                //    valuesUPE.Add("@ProjectTag", request.SelectedTags.Select(x => x.TagId).Aggregate((x, y) => x + "," + y));

                //    DataTable dt = uGITDAL.ExecuteDataSetWithParameters("usp_getUserProjectExperienceTag", valuesUPE);

                //    if (dt.Rows.Count > 0)
                //    {
                //        string userSel = Convert.ToString(dt.Rows[0]["UserId"]);
                //        List<FindResourceResponse> unique = new List<FindResourceResponse>();
                //        if (!string.IsNullOrEmpty(userSel))
                //        {
                //            if (userSel.Contains(','))
                //            {
                //                string[] sel = userSel.Split(',');
                //                foreach (var item in sel)
                //                {
                //                    unique.AddRange(lstResponse.Where(x => x.AssignedTo.Contains(item)).ToList());
                //                }
                //                lstResponse = new List<FindResourceResponse>();
                //                lstResponse = unique;

                //            }
                //            else
                //            {
                //                lstResponse = lstResponse.Where(x => x.AssignedTo.Contains(userSel)).ToList();
                //            }
                //        }
                //        else
                //        {
                //            lstResponse = new List<FindResourceResponse>();
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return lstResponse;
        }

        public List<FindTeamsResponse> FindTeams(ApplicationContext context, FindTeamsRequest request)
        {
            List<FindTeamsResponse> lstResponse = new List<FindTeamsResponse>();

            UserProjectExperienceManager _userProjectExperienceManager = new UserProjectExperienceManager(_context);
            UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);
            List<string> selectedUsers = new List<string>();
            List<ResourceUsageSummaryWeekWise> lstAllOverLappingAlloc = new List<ResourceUsageSummaryWeekWise>();
            List<UserProjectExperience> userProjectExperiences = null;
            List<ProjectTag> projectTags = null;
            List<ProjectTag> certificates = null;

            UGITModule module = _moduleManager.GetByName(uHelper.getModuleNameByTicketId(request.TicketID));
            var projectRow = _ticketManager.GetByTicketID(module, request.TicketID);
            if (projectRow == null)
                return lstResponse;

            List<string> selectedRoles = request.SelectedResources.Select(x => x.RoleID).ToList();
            List<string> selectedResourceUserIDs = request.SelectedResources.Select(x => x.UserID).ToList();
            List<UserProfile> originalProfiles = _userManager.GetUsersProfile().Where(x => x.Enabled == true).ToList();
            string projectSector = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.BCCISector]);
            if (string.IsNullOrWhiteSpace(projectSector) && request.SearchTeamCriteria.CommonSector)
                return lstResponse;

            string clientCompany = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.CRMCompanyLookup]);
            if (string.IsNullOrWhiteSpace(clientCompany) && request.SearchTeamCriteria.CommonClient)
                return lstResponse;


            if (uHelper.IsExperienceTagAllowGroupFilter(_context) && request.SearchTeamCriteria.SelectedTags != null && request.SearchTeamCriteria.SelectedTags.Count() > 0)
            {
                projectTags = request.SearchTeamCriteria.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Experience).ToList();
                
            }

            //if (request.SearchTeamCriteria.CommonExperiences)
            //{
            //    try
            //    {
            //        string projectTagsStr = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.TagMultiLookup]);
            //        if (!string.IsNullOrEmpty(projectTagsStr))
            //            projectTags = JsonConvert.DeserializeObject<List<ProjectTag>>(projectTagsStr);

            //        if (projectTags == null ||projectTags?.Count == 0)
            //            return lstResponse;

            //        projectTags = projectTags.Where(p => p.IsMandatory == true && p.Type == TagType.Experience).ToList();

            //    }
            //    catch (Exception ex)
            //    {
            //        ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.TagMultiLookup])}.");
            //        return lstResponse;
            //    }
            //}

            List<GlobalRole> globalRoles = _roleManager.Load(x => x.TenantID == context.TenantID);

            List<string> commonSectorClientTicketIds = new List<string>();
            List<string> commonExperienceTagsTicketIds = new List<string>();
            List<string> commonTicketIDs = new List<string>();

            DataTable dtCommonSectorClientTickets = new DataTable();

            if (request.SearchTeamCriteria.CommonClient && request.SearchTeamCriteria.CommonSector)
            {
                dtCommonSectorClientTickets = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{clientCompany}' and {DatabaseObjects.Columns.BCCISector}='{projectSector}'", DatabaseObjects.Columns.TicketId, null);
            }
            else if (request.SearchTeamCriteria.CommonClient && !request.SearchTeamCriteria.CommonSector)
            {
                dtCommonSectorClientTickets = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{clientCompany}'", DatabaseObjects.Columns.TicketId, null);
            }
            else if (!request.SearchTeamCriteria.CommonClient && request.SearchTeamCriteria.CommonSector)
            {
                dtCommonSectorClientTickets = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.BCCISector}='{projectSector}'", DatabaseObjects.Columns.TicketId, null);
            }

            commonSectorClientTicketIds.AddRange(dtCommonSectorClientTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
            commonTicketIDs = commonSectorClientTicketIds;

            //if (projectTags?.Count() > 0)
            //{
            //    DataTable dtCommonExperienceTagsTickets = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'",
            //       string.Join(",", new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.TagMultiLookup }), null);
            //    commonExperienceTagsTicketIds.AddRange(dtCommonExperienceTagsTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                
            //    List<string> commonIDs = new List<string>();
            //    if (request.SearchTeamCriteria.CommonClient || request.SearchTeamCriteria.CommonSector)
            //        commonIDs = commonSectorClientTicketIds.Intersect(commonExperienceTagsTicketIds).ToList();
            //    else
            //        commonIDs = commonExperienceTagsTicketIds;

            //    List<string> projectIDs = new List<string>();
            //    List<ProjectTag> tags = null;
            //    foreach (string ticketID in commonIDs)
            //    {
                   
            //        DataRow dr = dtCommonExperienceTagsTickets.Select($"{DatabaseObjects.Columns.TicketId}='{ticketID}'").FirstOrDefault();
            //        if (dr != null)
            //        {
            //            try
            //            {
            //                tags = null;
            //                string projectTagsStr = UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.TagMultiLookup]);
            //                if (!string.IsNullOrEmpty(projectTagsStr))
            //                    tags = JsonConvert.DeserializeObject<List<ProjectTag>>(projectTagsStr);
            //            }
            //            catch (Exception ex)
            //            {
            //                ULog.WriteException(ex, $" -- Failed to DeserializeObject TagString - {UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.TagMultiLookup])}.");
            //            }

            //            if (tags != null && tags.Count > 0)
            //            {
            //                tags = tags.Where(p => p.IsMandatory == true && p.Type == TagType.Experience).ToList();

            //                var commonTags = projectTags.Intersect(tags);
            //                if (commonTags.Count() >= projectTags.Count())
            //                {
            //                    projectIDs.Add(ticketID);
            //                }
            //            }
            //        }
                   
            //    }
            //    if (request.SearchTeamCriteria.CommonClient || request.SearchTeamCriteria.CommonSector)
            //        commonTicketIDs = commonSectorClientTicketIds.Intersect(projectIDs).ToList();
            //    else
            //        commonTicketIDs = projectIDs;
            //}
            //else
            //{
            //    commonTicketIDs = commonSectorClientTicketIds;
            //}

            foreach (AllocationResource allocationResource in request.SelectedResources)
            {
                FindTeamsResponse findTeamsResponse = new FindTeamsResponse()
                {
                    RoleID = allocationResource.RoleID,
                    RoleName = globalRoles.FirstOrDefault(x => x.Id == allocationResource.RoleID)?.Name,
                    Resources = new List<FindResourceResponse>()
                };

                List<UserProfile> userProfiles = originalProfiles.Where(x => x.Enabled && x.GlobalRoleId.EqualsIgnoreCase(allocationResource.RoleID)).ToList();
                selectedUsers = userProfiles.Select(x => x.Id).Distinct().ToList();
                if (userProfiles != null && userProfiles.Count == 1)
                    lstAllOverLappingAlloc = _resourceUsageSummaryWeekWiseMGR.LoadOpenItems(context, allocationResource.AllocationStartDate.Date, allocationResource.AllocationEndDate.Date, userProfiles.First().Id);
                else
                    lstAllOverLappingAlloc = _resourceUsageSummaryWeekWiseMGR.LoadOpenItems(context, allocationResource.AllocationStartDate.Date, allocationResource.AllocationEndDate.Date, UGITUtility.ConvertListToString(selectedUsers, Constants.Separator6));
                

                if (commonTicketIDs.Count > 0)
                {
                    if (selectedUsers != null && selectedUsers.Count > 0)
                    {
                        List<string> users = lstAllOverLappingAlloc.Where(x => commonTicketIDs.Any(y => x.WorkItem.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                        selectedUsers = selectedUsers.Intersect(users).ToList();
                    }
                    else
                    {
                        selectedUsers = lstAllOverLappingAlloc.Where(x => commonTicketIDs.Any(y => x.WorkItem.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                    }
                }


                int noOfWorkingHoursPerDay = uHelper.GetWorkingHoursInADay(_context);
                int noOfWorkingDaysPerWeek = uHelper.GetWorkingDaysInWeeks(_context, 1);
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_context, allocationResource.AllocationStartDate.Date, allocationResource.AllocationEndDate.Date);

                List<ResourceWeekWiseAvailabilityResponse> lstResourceWeekWiseAvailabilityResponce = null;
                List<Tuple<DateTime, DateTime>> tupleAllocationsDate = this.SplitAllocationDateAcrossPhases(request.TicketID, allocationResource.AllocationStartDate.Date, allocationResource.AllocationEndDate.Date);
                List<AllocationData> allocations = new List<AllocationData>();
                tupleAllocationsDate.ForEach(x =>
                {
                    allocations.Add(new AllocationData() { StartDate = x.Item1, EndDate = x.Item2, RequiredPctAllocation = allocationResource.RequiredPctAllocation });
                });

                if (allocations?.Count > 0 && selectedUsers?.Count > 0)
                {
                    List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeeks = lstAllOverLappingAlloc.Where(x => !x.SoftAllocation && !(x.WorkItem == request.TicketID
                        && allocations.Select(y => y.StartDate).ToList().Contains(x.ActualStartDate.Value)
                        && allocations.Select(y => y.EndDate).ToList().Contains(x.ActualEndDate.Value)))?.ToList() ?? null;
                    lstResourceWeekWiseAvailabilityResponce = uHelper.GetWeekWiseAveragePctAllocation(resourceUsageSummaryWeeks, allocations, selectedUsers);
                }

                if (projectTags?.Count > 0)
                    userProjectExperiences = userProjectExperienceMGR.Load(x => selectedUsers.Contains(x.UserId));

                foreach (string uProfile in selectedUsers)
                {
                    UserProfile profile = userProfiles.FirstOrDefault(x => x.Id == uProfile);

                    if (profile == null)
                        continue;
                    FindResourceResponse response = new FindResourceResponse();
                    response.AssignedTo = profile.Id;
                    response.AssignedToName = profile.Name;
                    response.GroupID = profile.GlobalRoleId;   // request.GroupID;
                    response.JobTitle = profile.JobProfile;
                    response.UserImagePath = profile.Picture;

                    GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == profile.GlobalRoleId);
                    if (typeGroup != null)
                    {
                        response.RoleName = typeGroup.Name;
                    }

                    if (!string.IsNullOrWhiteSpace(request.SearchTeamCriteria.SelectedCertifications))
                    {
                        if (string.IsNullOrWhiteSpace(profile.UserCertificateLookup))
                            continue;
                        else
                        {
                            List<string> certId = request.SearchTeamCriteria.SelectedCertifications.Split(',').ToList();
                            List<string> userCertificates = profile.UserCertificateLookup.Split(',').ToList();
                            if (!certId.All(x => userCertificates.Contains(x)))
                            {
                                continue;
                            }
                        }
                    }
                    if (projectTags?.Count() > 0)
                    {
                        List<UserProjectExperience> userProjectExplist = userProjectExperiences.Where(x => x.UserId == profile.Id).ToList();
                        bool resourceSelected = true;
                        projectTags.ForEach(x =>
                        {
                            int userTagCount = userProjectExplist.Where(y => y.TagLookup.ToString() == x.TagId).Count();
                            if (userTagCount >= x.MinValue)
                            {
                                if (response.ResourceTags == null)
                                    response.ResourceTags = new List<ResourceTag>();
                                response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = userTagCount.ToString(), Type = TagType.Experience });
                            }
                            else
                                resourceSelected = false;
                        });
                        if (!resourceSelected)
                            continue;
                    }
                    ResourceWeekWiseAvailabilityResponse resourceWeekWiseAvailabilityResponse = lstResourceWeekWiseAvailabilityResponce?.Where(x => x.UserId == profile.Id)?.FirstOrDefault();
                    double postPctAllocation = 0;
                    Availability availability = Availability.FullyAvailable;
                    if (resourceWeekWiseAvailabilityResponse != null)
                    {
                        //response.AllocationRange = (int)resourceWeekWiseAvailabilityResponse.AvailabilityType;
                        response.PctAllocation = resourceWeekWiseAvailabilityResponse.AverageUtilization;
                        response.WeekWiseAllocations = resourceWeekWiseAvailabilityResponse.WeekWiseAllocations;
                        postPctAllocation = resourceWeekWiseAvailabilityResponse.PostAverageUtilization;
                        availability = resourceWeekWiseAvailabilityResponse.AvailabilityType;
                    }
                    //if (response.PctAllocation > 100)
                    //    response.AllocationRange = 2;
                    if (postPctAllocation < 80)
                        response.AllocationRange = 0;
                    else if (postPctAllocation >= 80 && postPctAllocation <= 100)
                        response.AllocationRange = 1;
                    else if (postPctAllocation > 100)
                        response.AllocationRange = 2;
                    //else if (response.PctAllocation > 110)
                    //    response.AllocationRange = 3;

                    response.TotalPctAllocation = 0;
                    //response.PctAllocation = 0;
                    response.SoftPctAllocation = 0;
                    
                    findTeamsResponse.Resources.Add(response);
                }
                findTeamsResponse.Resources = findTeamsResponse.Resources.OrderBy(y => y.PctAllocation).ThenBy(y => y.AssignedToName).ToList();
                findTeamsResponse.ID = allocationResource.ID;
                findTeamsResponse.Index = allocationResource.Index;
                findTeamsResponse.TypeName = allocationResource.TypeName;
                lstResponse.Add(findTeamsResponse);
            }

            //DataTable projects = _ticketManager.GetAllTickets(module);
            //DataRow[] projectRows = projects.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.BCCISector.ToLower(), projectSector.ToLower()));

            //foreach (AllocationResource resource in request.SelectedResources)
            //{
            //    foreach (DataRow dr in projectRows)
            //    {
            //        List<ProjectEstimatedAllocation> oldAllocations = this.Load(x => x.TicketId != request.TicketID && x.TicketId == UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.TicketId]));
            //        List<string> oldAllocatedUsers = oldAllocations.Select(x => x.AssignedTo).Distinct().ToList();
            //        List<string> usersWithSelectedRole = userProfiles.Where(x => x.GlobalRoleId.EqualsIgnoreCase(resource.RoleID)).Select(x=>x.Id).ToList();
            //        List<string> common = usersWithSelectedRole.Intersect(oldAllocatedUsers).ToList();
            //        foreach (var item in common)
            //        {
            //            UserProfile user = userProfiles.Find((x) => x.Id == item);
            //            if (user != null)
            //            {
            //                lstResponse.Add(new FindTeamsResponse
            //                {
            //                    UserID = user.Id,
            //                    Name = user.Name,
            //                    RoleID = resource.RoleID,
            //                    RoleName = globalRoles.Find(x=>x.Id == user.GlobalRoleId)?.Name
            //                });
            //            }

            //        }

            //    }

            //}


            //return new List<FindTeamsResponse>()
            //{
            //    new FindTeamsResponse{ Name = "Test1", UserID = Guid.NewGuid().ToString(), RoleID = "1000", RoleName = "Role1" }
            //};

            return lstResponse;
        }

        public List<FindResourceResponse> FindResourceBasedOnGroupNew(ApplicationContext context, FindResourceRequest request)
        {
            //all required managers initialization
            UserProfileManager userManager = new UserProfileManager(context);
            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            TicketManager ticketManagerr = new TicketManager(context);
            ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseMGR = new ResourceUsageSummaryWeekWiseManager(context);
            List<FindResourceResponse> lstResponse = new List<FindResourceResponse>();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<GlobalRole> globalRoles = roleManager.Load(x => x.TenantID == context.TenantID);
            DepartmentManager departmentManager = new DepartmentManager(context);
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);
            UserProfile profile = null;
            JobTitle jobTitle = null;
            List<ProjectTag> projectTags = null;
            List<SummaryResourceProjectComplexity> userComplexities = null;
            List<SummaryResourceProjectComplexity> Complexities = new List<SummaryResourceProjectComplexity>();
            List<UserProjectExperience> userProjectExperiences=null;

            try
            {
                //load overlap resource allocation between requested date range
                //feteching selected row group role obj and then selecting users based on role name 
                List<UserProfile> lstUProfile = new List<UserProfile>();
                lstUProfile = userManager.GetUsersProfile().Where(x => x.Enabled == true).ToList();
                //select only one usser if selecteduserid have value mean user forces to select any user.
                if (!string.IsNullOrEmpty(request.SelectedUserID))
                    lstUProfile = lstUProfile.Where(x => x.Id == request.SelectedUserID).ToList();
                else if (!string.IsNullOrEmpty(request.GroupID))
                    lstUProfile = lstUProfile.Where(x => x.Enabled && x.GlobalRoleId.EqualsIgnoreCase(request.GroupID)).ToList();

                List<string> selectedUsers = new List<string>();
                int projectComplexity = 0;
                //loading current cpr ticket
                string moduleName = uHelper.getModuleNameByTicketId(request.ProjectID);
                UGITModule module = moduleManager.LoadByName(moduleName);

                string complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectComplexity, module.ModuleTable))
                    complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
                else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CRMProjectComplexity, module.ModuleTable))
                    complexityColumnName = DatabaseObjects.Columns.CRMProjectComplexity;

                DataRow projectRow = ticketManagerr.GetByTicketID(module, request.ProjectID, new List<string>() { complexityColumnName, DatabaseObjects.Columns.TicketRequestTypeLookup });

                if ((request.Complexity || request.ProjectVolume || request.ProjectCount) && UGITUtility.IfColumnExists(projectRow, complexityColumnName))
                {

                    projectComplexity = UGITUtility.StringToInt(Convert.ToString(projectRow[complexityColumnName]));
                    List<UserProfile> lstComplexityUsers = new List<UserProfile>();
                    List<SummaryResourceProjectComplexity> complexityAboveCurrentCRP = complexityManager.Load(x => x.Complexity >= projectComplexity);
                    if (complexityAboveCurrentCRP != null && complexityAboveCurrentCRP.Count > 0)
                        lstComplexityUsers = lstUProfile.FindAll(x => complexityAboveCurrentCRP.Any(y => y.UserId == x.Id)).ToList();

                    //filtering only those users which have complexity saved in summary_resourceprojectcoomplexity table bcz project count and revenue count depends on summary table only
                    if (request.Complexity)
                    {
                        
                        selectedUsers = lstComplexityUsers.Select(x => x.Id).Distinct().ToList();
                    }
                }

                if (request.RequestTypes)
                {
                    List<UserProfile> lstRequestTypeUsers = new List<UserProfile>();
                    string requestType = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                    List<SummaryResourceProjectComplexity> complexityWithRequestTypes = complexityManager.Load(x => x.RequestTypes.Contains(requestType));
                    List<string> userProfilesWithRequestTypes = complexityWithRequestTypes.Select(x => x.UserId).Distinct().ToList();
                    selectedUsers = selectedUsers.Intersect(userProfilesWithRequestTypes).ToList();
                }

                if (!request.Complexity)
                {
                    selectedUsers = lstUProfile.Select(x => x.Id).Distinct().ToList();
                }
                #region load allocation from resource allocation
                List<ResourceUsageSummaryWeekWise> lstAllOverLappingAlloc = new List<ResourceUsageSummaryWeekWise>();
                if (!string.IsNullOrEmpty(request.SelectedUserID) && lstUProfile != null && lstUProfile.Count == 1)
                    lstAllOverLappingAlloc = resourceUsageSummaryWeekWiseMGR.LoadOpenItems(context, request.AllocationStartDate.Date, request.AllocationEndDate.Date, lstUProfile.FirstOrDefault().Id);
                else
                    lstAllOverLappingAlloc = resourceUsageSummaryWeekWiseMGR.LoadOpenItems(context, request.AllocationStartDate.Date, request.AllocationEndDate.Date, UGITUtility.ConvertListToString(selectedUsers, Constants.Separator6));
                #endregion
                List<JobTitle> jobTitles = new List<JobTitle>();
                if (!string.IsNullOrEmpty(request.JobTitles))
                {
                    List<string> lstjobtitles = UGITUtility.ConvertStringToList(request.JobTitles, Constants.Separator);
                    jobTitles = jobTitleManager.Load(x => lstjobtitles.Contains(x.Title));
                }
                 
                if (jobTitles.Count > 0)
                {
                    // filter based on job title.
                    List<string> lstjobtitles = jobTitles.Select(x => Convert.ToString(x.ID)).ToList();
                    lstUProfile = lstUProfile?.FindAll(x => lstjobtitles.Contains(Convert.ToString(x.JobTitleLookup)));
                }
                if (request.departments > 0)
                {
                    // filter based on department selected.
                    lstUProfile = lstUProfile?.Where(o => o.Department == request.departments.ToString()).ToList();
                }
                if (!string.IsNullOrEmpty(request.DivisionId) && UGITUtility.StringToInt(request.DivisionId) != 0)
                {
                    List<string> lstDepartments = departmentManager.Load(x => request.DivisionId.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Contains(UGITUtility.ObjectToString(x.DivisionIdLookup))
                    && !x.Deleted).Select(x => UGITUtility.ObjectToString(x.ID)).ToList();
                    lstUProfile = lstUProfile?.FindAll(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.Department)));
                }

                if (!string.IsNullOrEmpty(request.DepartmentId) && request.DepartmentId != "0") {
                    List<string> deptIds = request.DepartmentId.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    lstUProfile = lstUProfile?.FindAll(x => deptIds.Contains(UGITUtility.ObjectToString(x.Department)));
                }
                //selectedUsers = lstUProfile.Select(x => x.Id).ToList() ?? null;

                DataTable dtTicketIds = new DataTable();
                if (request.Customer == true && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == true && request.Sector == false)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == false && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }

                if (dtTicketIds != null && dtTicketIds.Rows.Count > 0)
                {
                    List<string> LstOpenTicketIds = new List<string>();
                    LstOpenTicketIds.AddRange(dtTicketIds.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));

                    if (selectedUsers != null && selectedUsers.Count > 0)
                    {
                        List<string> users = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.WorkItem.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                        selectedUsers = selectedUsers.Intersect(users).ToList();
                    }
                    else
                    {
                        selectedUsers = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.WorkItem.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                    }
                }
                
                Complexities = complexityManager.Load(x => selectedUsers.Contains(x.UserId));
                int noOfWorkingHoursPerDay = uHelper.GetWorkingHoursInADay(_context);
                int noOfWorkingDaysPerWeek = uHelper.GetWorkingDaysInWeeks(_context, 1);
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_context, request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                //int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);

                //selectedUsers = selectedUsers.Where(x => x == "f4d20c1f-9915-41c4-b344-8991e24801dc").ToList();
                //code to create response object only for those users who have complexity saved
                //calculat allocation based on following logic
                // if we have 3 weeks from start 4th to 15th then
                // (w1*4 + w2*7 + w3*4) / (4+7+4)
                //selectedUsers =new List<string>() { selectedUsers.FirstOrDefault(x => x.EqualsIgnoreCase("44380d17-c887-488c-856b-31753e4197b7")) };
                
                if (uHelper.IsExperienceTagAllowGroupFilter(_context) && request.SelectedTags != null && request.SelectedTags.Count() > 0)
                {
                    projectTags = request.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Experience).ToList();
                    if(projectTags.Count>0)
                        userProjectExperiences = userProjectExperienceMGR.Load(x => selectedUsers.Contains(x.UserId));
                }
                List<ResourceWeekWiseAvailabilityResponse> lstResourceWeekWiseAvailabilityResponce = null;
                if (!request.IsRequestFromSummaryView)
                {
                    List<Tuple<DateTime, DateTime>>  tupleAllocationsDate = CRMProjAllocManager.SplitAllocationDateAcrossPhases(request.ProjectID, request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                    request.Allocations = new List<AllocationData>();
                    tupleAllocationsDate.ForEach(x =>
                    {
                        request.Allocations.Add(new AllocationData() { StartDate = x.Item1, EndDate = x.Item2, RequiredPctAllocation = request.PctAllocation });
                    });
                }

                if (request.Allocations?.Count > 0 && selectedUsers?.Count > 0) {
                    //List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeeks = lstAllOverLappingAlloc.Where(x => !x.SoftAllocation && !(x.WorkItem == request.ProjectID
                    //    && request.Allocations.Select(y => y.StartDate).ToList().Contains(x.ActualStartDate.Value)
                    //    && request.Allocations.Select(y => y.EndDate).ToList().Contains(x.ActualEndDate.Value)))?.ToList() ?? null;
                    List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeeks = lstAllOverLappingAlloc.Where(x => !x.SoftAllocation)?.ToList() ?? null;
                    lstResourceWeekWiseAvailabilityResponce = uHelper.GetWeekWiseAveragePctAllocation(resourceUsageSummaryWeeks, request.Allocations, selectedUsers);
                }

                foreach (string uProfile in selectedUsers)
                {
                    profile = lstUProfile.FirstOrDefault(x => x.Id == uProfile);

                    if (profile == null)
                        continue;
                    FindResourceResponse response = new FindResourceResponse();
                    response.AssignedTo = profile.Id;
                    response.AssignedToName = profile.Name;
                    response.GroupID = profile.GlobalRoleId;   // request.GroupID;
                    response.JobTitle = profile.JobProfile;
                    response.UserImagePath = profile.Picture;

                    GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == profile.GlobalRoleId);
                    if (typeGroup != null)
                    {
                        response.RoleName = typeGroup.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(request.SelectedCertifications))
                    {
                        if (string.IsNullOrWhiteSpace(profile.UserCertificateLookup))
                            continue;
                        else
                        {
                            List<string> certId = request.SelectedCertifications.Split(',').ToList();
                            List<string> userCertificates = profile.UserCertificateLookup.Split(',').ToList();
                            if (!certId.All(x => userCertificates.Contains(x)))
                            {
                                continue;
                            }
                        }
                    }

                    if (projectTags != null && projectTags.Count() > 0)
                    {
                        List<UserProjectExperience> userProjectExplist = userProjectExperiences.Where(x => x.UserId == profile.Id).ToList();
                        bool resourceSelected = true;
                        projectTags.ForEach(x =>
                        {
                            int userTagCount = userProjectExplist.Where(y => y.TagLookup.ToString() == x.TagId).Count();
                            if (userTagCount >= x.MinValue)
                            {
                                if (response.ResourceTags == null)
                                    response.ResourceTags = new List<ResourceTag>();
                                response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = userTagCount.ToString(), Type = TagType.Experience });
                            }
                            else
                                resourceSelected = false;
                        });
                        if (!resourceSelected)
                            continue;
                    }
                    ResourceWeekWiseAvailabilityResponse resourceWeekWiseAvailabilityResponse = lstResourceWeekWiseAvailabilityResponce?.Where(x => x.UserId == profile.Id)?.FirstOrDefault();
                    double postPctAllocation = 0;
                    Availability availability = Availability.FullyAvailable;
                    if (resourceWeekWiseAvailabilityResponse != null)
                    {
                        //response.AllocationRange = (int)resourceWeekWiseAvailabilityResponse.AvailabilityType;
                        response.PctAllocation = resourceWeekWiseAvailabilityResponse.AverageUtilization;
                        response.WeekWiseAllocations = resourceWeekWiseAvailabilityResponse.WeekWiseAllocations;
                        postPctAllocation = resourceWeekWiseAvailabilityResponse.PostAverageUtilization;
                        availability = resourceWeekWiseAvailabilityResponse.AvailabilityType;
                    }
                    //if (response.PctAllocation > 100)
                    //    response.AllocationRange = 2;
                    if (postPctAllocation < 80)
                        response.AllocationRange = 0;
                    else if (postPctAllocation >= 80 && postPctAllocation <= 100)
                        response.AllocationRange = 1;
                    else if (postPctAllocation > 100)
                        response.AllocationRange = 2;
                    //else if (response.PctAllocation > 110)
                    //    response.AllocationRange = 3;


                    if (request.ResourceAvailability == ResourceAvailabilityType.FullyAvailable && availability != Availability.FullyAvailable)
                        continue;
                    else if (request.ResourceAvailability == ResourceAvailabilityType.PartiallyAvailable && (availability != Availability.PartiallyAvailable && availability != Availability.NearToFullyAvailable))
                        continue;

                    response.TotalPctAllocation = 0;
                    //response.PctAllocation = 0;
                    response.SoftPctAllocation = 0;
                    
                    userComplexities = Complexities.Where(x => x.UserId == profile.Id).ToList();
                    //capacity code only needed if complexity exits for that user
                    if (userComplexities != null && userComplexities.Count > 0)
                    {
                        response.HighestComplexity = userComplexities.Max(x => x.Complexity);
                        //find project count
                        //JobTitle jobTitle = null;
                        if (request.ProjectCount)
                        {
                            response.ProjectCount = userComplexities.Sum(x => x.Count);
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            response.TotalVolumeRange = 0;
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowProjectCapacity <= 0 && jobTitle.HighProjectCapacity <= 0) || response.ProjectCount <= jobTitle.LowProjectCapacity)
                                    response.projectCountRange = 0;
                                else if (response.ProjectCount > jobTitle.HighProjectCapacity)
                                    response.projectCountRange = 2;
                                else
                                    response.projectCountRange = 1;
                            }
                        }

                        if (request.ProjectVolume)
                        {
                            double cost = 0;
                            cost = userComplexities.Sum(x => x.HighProjectCapacity);

                            response.TotalVolume = UGITUtility.FormatNumber(cost, "currency");
                            response.TotalVolumeRange = 0;
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowRevenueCapacity <= 0 && jobTitle.HighRevenueCapacity <= 0) || cost <= jobTitle.LowRevenueCapacity)
                                    response.TotalVolumeRange = 0;
                                else if (cost > jobTitle.HighRevenueCapacity)
                                    response.TotalVolumeRange = 2;
                                else
                                    response.TotalVolumeRange = 1;
                            }
                        }
                    }
                    lstResponse.Add(response);
                }

                List<FindResourceResponse> lstResponseAvailability = new List<FindResourceResponse>();
                if (request.isAllocationView == false && request.ResourceAvailability != ResourceAvailabilityType.AllResource)
                {

                    foreach (FindResourceResponse responseAvailability in lstResponse)
                    {
                        if (responseAvailability.PctAllocation <= 100)
                        {
                            lstResponseAvailability.Add(responseAvailability);
                        }
                    }
                    lstResponse = lstResponseAvailability;
                }


                //set ordering in last in descending order and asc for pct allocation
                //if (request.ResourceAvailability == ResourceAvailabilityType.FullyAvailable)
                //    lstResponse = lstResponse.Where(x => x.AllocationRange == 0).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                //else if (request.ResourceAvailability == ResourceAvailabilityType.PartiallyAvailable)
                //    lstResponse = lstResponse.Where(x => x.AllocationRange == 1).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                //else
                    lstResponse = lstResponse.OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return lstResponse;
        }
        public ProjectAllocationTemplate NormaliseTemplate(ProjectAllocationTemplate templateAllocation, string newProjectID, DateTime pStartDate, DateTime pEndDate)
        {
            try
            {
                List<AllocationTemplateModel> lstTemplates = Newtonsoft.Json.JsonConvert.DeserializeObject(templateAllocation.Template, typeof(List<AllocationTemplateModel>)) as List<AllocationTemplateModel>;
                GlobalRoleManager roleManager = new GlobalRoleManager(_context);
                uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(_context);
                string moduleName = uHelper.getModuleNameByTicketId(newProjectID);
                ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
                UGITModule module = moduleViewManager.LoadByName(moduleName);
                DataRow newTicketObj = ticketManager.GetByTicketID(module, newProjectID);

                var preconStartDate = templateAllocation.PreconStartDate;
                var preconEndDate = templateAllocation.PreconEndDate;
                var constStartDate = templateAllocation.ConstStartDate;
                var constEndDate = templateAllocation.ConstEndDate;
                var closeoutStartDate = templateAllocation.CloseOutStartDate;
                var closeoutEndDate = templateAllocation.CloseOutEndDate;
                if (closeoutStartDate == DateTime.MinValue && constEndDate != DateTime.MinValue)
                {
                    closeoutStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDate.Value));
                }
                if (closeoutEndDate == DateTime.MinValue && closeoutStartDate != DateTime.MinValue)
                {
                    closeoutEndDate = closeoutStartDate.Value.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                }
                var preconStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.PreconStartDate]);
                var preconEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.PreconEndDate]);
                var constStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
                var constEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                var closeoutStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.CloseoutStartDate]);
                var closeoutEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.CloseOutDate]);
                if (closeoutStartDateProject == DateTime.MinValue && constEndDateProject != DateTime.MinValue)
                {
                    closeoutStartDateProject = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDateProject));
                }
                if (closeoutEndDateProject == DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue)
                {
                    closeoutEndDateProject = closeoutStartDateProject.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                }
                // Template Start Date, End Date
                var templateStartDate = UGITUtility.GetObjetToDateTime(templateAllocation.TicketStartDate);
                var templateEndDate = UGITUtility.GetObjetToDateTime(templateAllocation.TicketEndDate);

                DateTime tempStartDate = DateTime.MinValue;
                DateTime tempEndDate = DateTime.MinValue;
                if (lstTemplates.Count > 0)
                {
                    tempStartDate = lstTemplates.Where(x => x.AllocationStartDate.HasValue).Min(x => x.AllocationStartDate.Value);
                    tempEndDate = lstTemplates.Where(x => x.AllocationEndDate.HasValue).Max(x => x.AllocationEndDate.Value);
                }
                int addDaysToPrecon = 0, addDaysToCloseout = 0;
                double preconRatio = 1, clouseOutRatio = 1;
                if (tempStartDate < templateStartDate)
                {
                    addDaysToPrecon = (tempStartDate - templateStartDate).Days;
                    if (preconEndDateProject != DateTime.MinValue && preconStartDateProject != DateTime.MinValue
                        && preconEndDate != DateTime.MinValue && preconStartDate != DateTime.MinValue && preconEndDate != null && preconStartDate != null)
                    {
                        preconRatio = UGITUtility.StringToDouble((preconEndDateProject - preconStartDateProject).Days) / Convert.ToDouble((preconEndDate.Value - preconStartDate.Value).Days);
                    }
                    templateStartDate = tempStartDate;
                }

                if (tempEndDate > templateEndDate)
                {
                    addDaysToCloseout = (tempEndDate - templateEndDate).Days;
                    if (closeoutEndDateProject != DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue
                        && closeoutEndDate != DateTime.MinValue && closeoutStartDate != DateTime.MinValue && closeoutEndDate != null && closeoutStartDate != null)
                    {
                        clouseOutRatio = UGITUtility.StringToDouble((closeoutEndDateProject - closeoutStartDateProject).Days) / Convert.ToDouble((closeoutEndDate.Value - closeoutStartDate.Value).Days);
                    }
                    templateEndDate = tempEndDate;
                }

                if (templateEndDate == DateTime.MinValue)
                {
                    templateEndDate = templateStartDate.AddDays(templateAllocation.Duration);
                }

                // Current Project Start and End Date.
                var projectEndDate = pEndDate;
                var projectStartDate = pStartDate;

                foreach (AllocationTemplateModel tempAlloc in lstTemplates)
                {

                    var startDate = tempAlloc.AllocationStartDate;
                    var endDate = tempAlloc.AllocationEndDate;
                    bool pOverlaps = false, cOverlaps = false, coOverlaps = false;
                    if (startDate <= preconEndDate && endDate >= preconStartDate)
                    {
                        pOverlaps = true;
                    }
                    if (startDate <= constEndDate && endDate >= constStartDate)
                    {
                        cOverlaps = true;
                    }
                    if (startDate <= closeoutEndDate && endDate >= closeoutStartDate)
                    {
                        coOverlaps = true;
                    }
                    if (pOverlaps)
                    {
                        templateStartDate = preconStartDate.Value.AddDays(addDaysToPrecon);
                        templateEndDate = preconEndDate.Value;
                        projectStartDate = preconStartDateProject.AddDays(preconRatio * addDaysToPrecon);
                        projectEndDate = preconEndDateProject;
                    }
                    if (cOverlaps)
                    {
                        templateStartDate = constStartDate.Value;
                        templateEndDate = constEndDate.Value;
                        projectStartDate = constStartDateProject;
                        projectEndDate = constEndDateProject;
                    }
                    if (coOverlaps)
                    {
                        templateStartDate = closeoutStartDate.Value;
                        templateEndDate = closeoutEndDate.Value.AddDays(addDaysToCloseout);
                        projectStartDate = closeoutStartDateProject;
                        projectEndDate = closeoutEndDateProject.AddDays(clouseOutRatio * addDaysToCloseout);
                    }

                    int templateTticketDateDiff = templateEndDate != DateTime.MinValue && templateStartDate != DateTime.MinValue
                    ? (templateEndDate - templateStartDate).Days : 0;
                    int newTicketDiffDays = projectEndDate != DateTime.MinValue && projectStartDate != DateTime.MinValue
                        ? (projectEndDate - projectStartDate).Days : 0;
                    double daysRatio = newTicketDiffDays > 0 && templateTticketDateDiff > 0
                        ? Convert.ToDouble(newTicketDiffDays) / Convert.ToDouble(templateTticketDateDiff) : 0.0;

                    // New logic to calculate start date and end date from template allocation.
                    int diffStartDateTemplate = Convert.ToInt16((tempAlloc.AllocationStartDate.Value.Date - templateStartDate.Date).Days * daysRatio);
                    int diffEndDateTemplate = Convert.ToInt16((tempAlloc.AllocationEndDate.Value.Date - tempAlloc.AllocationStartDate.Value.Date).Days * daysRatio);
                    tempAlloc.AllocationStartDate = projectStartDate.AddDays(diffStartDateTemplate);
                    tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate.Value.Date.AddDays(diffEndDateTemplate);

                    if (projectStartDate != DateTime.MinValue && tempAlloc.AllocationStartDate < projectStartDate)
                        tempAlloc.AllocationStartDate = projectStartDate;

                    if (projectEndDate != DateTime.MinValue && tempAlloc.AllocationEndDate > projectEndDate)
                        tempAlloc.AllocationEndDate = projectEndDate;
                    if (tempAlloc.AllocationEndDate < tempAlloc.AllocationStartDate)
                    {
                        tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate;
                    }

                    tempAlloc.ProjectID = newProjectID;
                    UserProfile assignedto = _context.UserManager.GetUserById(tempAlloc.AssignedTo);

                    GlobalRole typeGroup = roleManager.Get(x => x.Id == tempAlloc.Type);      //.GetUserById(tempAlloc.Type);

                    if (assignedto != null)
                        tempAlloc.AssignedToName = assignedto.Name;
                    if (typeGroup != null)
                        tempAlloc.TypeName = typeGroup.Name;
                }

                lstTemplates = lstTemplates.OrderBy(x => x.TypeName).ThenBy(s => s.AllocationStartDate).ToList();
                templateAllocation.Template = Newtonsoft.Json.JsonConvert.SerializeObject(lstTemplates);
            }catch(Exception ex)
            {
                ULog.WriteException("Method NormaliseTemplate: " + ex.Message);
            }
            return templateAllocation;
        }

        /// <summary>
        /// New allocations date will be generated based on baseProjectId.
        /// </summary>
        /// <param name="baseProjectId"></param>
        /// <param name="newProjectID"></param>
        /// <returns></returns>
        public List<ProjectEstimatedAllocation> NormaliseProjectAllocation(string baseProjectId, string newProjectID)
        {
            uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(_context);

            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            UGITModule baseModule = moduleViewManager.LoadByName(uHelper.getModuleNameByTicketId(baseProjectId));
            DataRow baseTicketObj = ticketManager.GetByTicketID(baseModule, baseProjectId);
            List<ProjectEstimatedAllocation> baseProjectAllocations = this.Load(x => x.TicketId == baseProjectId && x.Deleted != true);
            List<ProjectEstimatedAllocation> splitedAllocations = this.SplitProjectAllocation(baseProjectId);

            string moduleName = uHelper.getModuleNameByTicketId(newProjectID);
            UGITModule module = moduleViewManager.LoadByName(moduleName);
            DataRow newTicketObj = ticketManager.GetByTicketID(module, newProjectID);

            // base project Start Date, End Date
            var templateStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconStartDate]);
            var templateEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseOutDate]);

            var preconStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconStartDate]);
            var preconEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconEndDate]);
            var constStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
            var constEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            var closeoutStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseoutStartDate]);
            var closeoutEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseOutDate]);
            if (closeoutStartDate == DateTime.MinValue && constEndDate != DateTime.MinValue)
            {
                closeoutStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDate));
            }
            if (closeoutEndDate == DateTime.MinValue && closeoutStartDate != DateTime.MinValue)
            {
                closeoutEndDate = closeoutStartDate.AddWorkingDays(uHelper.getCloseoutperiod(_context));
            }

            var preconStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.PreconStartDate]);
            var preconEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.PreconEndDate]);
            var constStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
            var constEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            var closeoutStartDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.CloseoutStartDate]);
            var closeoutEndDateProject = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.CloseOutDate]);
            if (closeoutStartDateProject == DateTime.MinValue && constEndDateProject != DateTime.MinValue)
            {
                closeoutStartDateProject = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDateProject));
            }
            if (closeoutEndDateProject == DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue)
            {
                closeoutEndDateProject = closeoutStartDateProject.AddWorkingDays(uHelper.getCloseoutperiod(_context));
            }

            DateTime tempStartDate = DateTime.MinValue;
            DateTime tempEndDate = DateTime.MinValue;
            int addDaysToPrecon = 0, addDaysToCloseout = 0;
            double preconRatio = 1, clouseOutRatio = 1;
            if (baseProjectAllocations.Count > 0)
            {
                tempStartDate = baseProjectAllocations.Where(x => x.AllocationStartDate.HasValue).Min(x => x.AllocationStartDate.Value);
                tempEndDate = baseProjectAllocations.Where(x => x.AllocationEndDate.HasValue).Max(x => x.AllocationEndDate.Value);
            }

            if (tempStartDate < templateStartDate)
            {
                addDaysToPrecon = (tempStartDate - templateStartDate).Days;
                if (preconEndDateProject != DateTime.MinValue && preconStartDateProject != DateTime.MinValue
                    && preconEndDate != DateTime.MinValue && preconStartDate != DateTime.MinValue)
                {
                    preconRatio = Convert.ToDouble((preconEndDateProject - preconStartDateProject).Days) / Convert.ToDouble((preconEndDate - preconStartDate).Days);
                }
                templateStartDate = tempStartDate;
            }

            if (tempEndDate > templateEndDate)
            {
                addDaysToCloseout = (tempEndDate - templateEndDate).Days;
                if (closeoutEndDateProject != DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue
                    && closeoutEndDate != DateTime.MinValue && closeoutStartDate != DateTime.MinValue)
                {
                    clouseOutRatio = Convert.ToDouble((closeoutEndDateProject - closeoutStartDateProject).Days) / Convert.ToDouble((closeoutEndDate - closeoutStartDate).Days);
                }
                templateEndDate = tempEndDate;
            }

            // Current Project Start and End Date.
            var projectEndDate = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.CloseOutDate]); ;
            var projectStartDate = UGITUtility.StringToDateTime(newTicketObj[DatabaseObjects.Columns.PreconStartDate]);

            foreach (ProjectEstimatedAllocation tempAlloc in splitedAllocations)
            {
                var startDate = tempAlloc.AllocationStartDate;
                var endDate = tempAlloc.AllocationEndDate;
                bool pOverlaps = false, cOverlaps = false, coOverlaps = false;
                if (startDate <= preconEndDate && endDate >= preconStartDate)
                {
                    pOverlaps = true;
                }
                if (startDate <= constEndDate && endDate >= constStartDate)
                {
                    cOverlaps = true;
                }
                if (startDate <= closeoutEndDate && endDate >= closeoutStartDate)
                {
                    coOverlaps = true;
                }
                if (pOverlaps)
                {
                    templateStartDate = preconStartDate.AddDays(addDaysToPrecon);
                    templateEndDate = preconEndDate;
                    projectStartDate = preconStartDateProject.AddDays(preconRatio * addDaysToPrecon);
                    projectEndDate = preconEndDateProject;
                }
                if (cOverlaps)
                {
                    templateStartDate = constStartDate;
                    templateEndDate = constEndDate;
                    projectStartDate = constStartDateProject;
                    projectEndDate = constEndDateProject;
                }
                if (coOverlaps)
                {
                    templateStartDate = closeoutStartDate;
                    templateEndDate = closeoutEndDate.AddDays(addDaysToCloseout);
                    projectStartDate = closeoutStartDateProject;
                    projectEndDate = closeoutEndDateProject.AddDays(clouseOutRatio * addDaysToCloseout);
                }

                int templateTticketDateDiff = templateEndDate != DateTime.MinValue && templateStartDate != DateTime.MinValue
                ? (templateEndDate - templateStartDate).Days : 0;
                int newTicketDiffDays = projectEndDate != DateTime.MinValue && projectStartDate != DateTime.MinValue
                    ? (projectEndDate - projectStartDate).Days : 0;
                double daysRatio = newTicketDiffDays > 0 && templateTticketDateDiff > 0
                    ? Convert.ToDouble(newTicketDiffDays) / Convert.ToDouble(templateTticketDateDiff) : 0.0;

                // New logic to calculate start date and end date from template allocation.
                int diffStartDateTemplate = Convert.ToInt16((tempAlloc.AllocationStartDate.Value.Date - templateStartDate.Date).Days * daysRatio);
                int diffEndDateTemplate = Convert.ToInt16((tempAlloc.AllocationEndDate.Value.Date - tempAlloc.AllocationStartDate.Value.Date).Days * daysRatio);
                tempAlloc.AllocationStartDate = projectStartDate.AddDays(diffStartDateTemplate);
                tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate.Value.Date.AddDays(diffEndDateTemplate);

                if (projectStartDate != DateTime.MinValue && tempAlloc.AllocationStartDate < projectStartDate)
                    tempAlloc.AllocationStartDate = projectStartDate;

                if (projectEndDate != DateTime.MinValue && tempAlloc.AllocationEndDate > projectEndDate)
                    tempAlloc.AllocationEndDate = projectEndDate;
                if (tempAlloc.AllocationEndDate < tempAlloc.AllocationStartDate)
                {
                    tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate;
                }
            }

            return splitedAllocations;
        }

        /// <summary>
        /// Split allocation across phases.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ProjectEstimatedAllocation> SplitProjectAllocation(string projectId)
        {
            uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(_context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            string moduleName = uHelper.getModuleNameByTicketId(projectId);
            UGITModule module = moduleViewManager.LoadByName(moduleName);
            DataRow ticketObj = ticketManager.GetByTicketID(module, projectId);
            List<ProjectEstimatedAllocation> projectAllocations = this.Load(x => x.TicketId == projectId && x.Deleted != true);
            List<ProjectEstimatedAllocation> splitedAllocations = new List<ProjectEstimatedAllocation>();
            int index = -1;
            projectAllocations.ForEach(e =>
            {
                var preconStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.PreconStartDate]);
                var preconEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.PreconEndDate]);
                var constStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
                var constEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                var closeoutStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.CloseoutStartDate]);
                var closeoutEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.CloseOutDate]);
                var startDate = e.AllocationStartDate;
                var endDate = e.AllocationEndDate;

                if (startDate != DateTime.MinValue || endDate != DateTime.MinValue)
                {
                    bool pOverlaps = false, cOverlaps = false, coOverlaps = false;
                    if (preconStartDate == DateTime.MinValue && startDate < constStartDate)
                    {
                        preconStartDate = startDate.Value;
                    }
                    if (preconEndDate == DateTime.MinValue && constStartDate != DateTime.MinValue)
                    {
                        preconEndDate = constStartDate.AddDays(-1);
                    }

                    if (closeoutEndDate == DateTime.MinValue && endDate > constEndDate)
                    {
                        closeoutEndDate = endDate.Value;
                    }
                    if (closeoutStartDate == DateTime.MinValue && constEndDate != DateTime.MinValue)
                    {
                        closeoutStartDate = constEndDate.AddDays(1);
                    }

                    if (startDate <= preconEndDate && endDate >= preconStartDate)
                    {
                        pOverlaps = true;
                    }
                    if (startDate <= constEndDate && endDate >= constStartDate)
                    {
                        cOverlaps = true;
                    }
                    if (startDate <= closeoutEndDate && endDate >= closeoutStartDate)
                    {
                        coOverlaps = true;
                    }

                    if (pOverlaps && cOverlaps && coOverlaps)
                    {
                        var alloc1 = DeepCopy(e);
                        var alloc2 = DeepCopy(e);
                        var alloc3 = DeepCopy(e);
                        alloc1.AllocationEndDate = preconEndDate;
                        alloc2.AllocationStartDate = constStartDate;
                        alloc2.AllocationEndDate = constEndDate;
                        alloc3.AllocationStartDate = closeoutStartDate;
                        splitedAllocations.Add(alloc1);
                        index -= 1;
                        alloc2.ID = index;
                        splitedAllocations.Add(alloc2);
                        index -= 1;
                        alloc3.ID = index;
                        splitedAllocations.Add(alloc3);
                    }
                    else
                    {
                        if (pOverlaps && cOverlaps)
                        {
                            var alloc1 = DeepCopy(e);
                            var alloc2 = DeepCopy(e);

                            alloc1.AllocationEndDate = preconEndDate;
                            alloc2.AllocationStartDate = constStartDate;
                            splitedAllocations.Add(alloc1);
                            index -= 1;
                            alloc2.ID = index;
                            splitedAllocations.Add(alloc2);
                        }
                        if (cOverlaps && coOverlaps)
                        {
                            var alloc1 = DeepCopy(e);
                            var alloc2 = DeepCopy(e);

                            alloc1.AllocationStartDate = startDate < constStartDate
                                ? constStartDate : alloc1.AllocationStartDate;

                            alloc1.AllocationEndDate = constEndDate;
                            alloc2.AllocationStartDate = closeoutStartDate;
                            splitedAllocations.Add(alloc1);
                            index -= 1;
                            alloc2.ID = index;
                            splitedAllocations.Add(alloc2);
                        }
                        if (pOverlaps && !cOverlaps && !coOverlaps)
                        {
                            var alloc1 = DeepCopy(e);
                            if (endDate > preconEndDate)
                            {
                                alloc1.AllocationEndDate = preconEndDate;
                            }
                            splitedAllocations.Add(alloc1);
                        }
                        if (!pOverlaps && cOverlaps && !coOverlaps)
                        {
                            var alloc1 = DeepCopy(e);
                            if (startDate < constStartDate || endDate > constEndDate)
                            {
                                alloc1.AllocationStartDate = startDate < constStartDate
                                    ? constStartDate : alloc1.AllocationStartDate;
                                alloc1.AllocationEndDate = endDate > constEndDate
                                    ? constEndDate : alloc1.AllocationEndDate;
                            }
                            splitedAllocations.Add(alloc1);
                        }
                        if (!pOverlaps && !cOverlaps && coOverlaps)
                        {
                            var alloc1 = DeepCopy(e);
                            if (startDate < closeoutStartDate)
                            {
                                alloc1.AllocationStartDate = closeoutStartDate;
                            }
                            splitedAllocations.Add(alloc1);
                        }
                    }
                }
            });

            return splitedAllocations;
        }

        public List<Tuple<DateTime, DateTime>> SplitAllocationDateAcrossPhases(string projectId, DateTime sDate, DateTime eDate)
        {
            uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(_context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            string moduleName = uHelper.getModuleNameByTicketId(projectId);
            UGITModule module = moduleViewManager.LoadByName(moduleName);
            DataRow ticketObj = ticketManager.GetByTicketID(module, projectId);
            List<Tuple<DateTime, DateTime>> splitedDates = new List<Tuple<DateTime, DateTime>>();

            var preconStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.PreconStartDate]);
            var preconEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.PreconEndDate]);
            var constStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
            var constEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            var closeoutStartDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.CloseoutStartDate]);
            var closeoutEndDate = UGITUtility.StringToDateTime(ticketObj[DatabaseObjects.Columns.CloseOutDate]);
            var startDate = sDate;
            var endDate = eDate;

            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue)
            {
                bool pOverlaps = false, cOverlaps = false, coOverlaps = false;
                if (preconStartDate == DateTime.MinValue && startDate < constStartDate)
                {
                    preconStartDate = startDate;
                }
                if (preconEndDate == DateTime.MinValue && constStartDate != DateTime.MinValue)
                {
                    preconEndDate = constStartDate.AddDays(-1);
                }

                if (closeoutEndDate == DateTime.MinValue && endDate > constEndDate)
                {
                    closeoutEndDate = endDate;
                }
                if (closeoutStartDate == DateTime.MinValue && constEndDate != DateTime.MinValue)
                {
                    closeoutStartDate = constEndDate.AddDays(1);
                }

                if (startDate <= preconEndDate && endDate >= preconStartDate)
                {
                    pOverlaps = true;
                }
                if (startDate <= constEndDate && endDate >= constStartDate)
                {
                    cOverlaps = true;
                }
                if (startDate <= closeoutEndDate && endDate >= closeoutStartDate)
                {
                    coOverlaps = true;
                }

                if (pOverlaps && cOverlaps && coOverlaps)
                {
                    splitedDates.Add(new Tuple<DateTime, DateTime>(sDate, preconEndDate));
                    splitedDates.Add(new Tuple<DateTime, DateTime>(constStartDate, constEndDate));
                    splitedDates.Add(new Tuple<DateTime, DateTime>(closeoutStartDate, eDate));
                }
                else
                {
                    if (pOverlaps && cOverlaps)
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(sDate, preconEndDate));
                        splitedDates.Add(new Tuple<DateTime, DateTime>(constStartDate, eDate));
                    }
                    else if (cOverlaps && coOverlaps)
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(startDate < constStartDate ? constStartDate : sDate, constEndDate));
                        splitedDates.Add(new Tuple<DateTime, DateTime>(closeoutStartDate, eDate));
                    }
                    else if (pOverlaps && !cOverlaps && !coOverlaps)
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(sDate, endDate > preconEndDate ? preconEndDate : eDate));
                    }
                    else if (!pOverlaps && cOverlaps && !coOverlaps)
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(startDate < constStartDate ? constStartDate : sDate, endDate > constEndDate ? constEndDate : eDate));
                    }
                    else if (!pOverlaps && !cOverlaps && coOverlaps)
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(startDate < closeoutStartDate ? closeoutStartDate : sDate, eDate));
                    }
                    else
                    {
                        splitedDates.Add(new Tuple<DateTime, DateTime>(sDate, eDate));
                    }
                }
            }

            return splitedDates;
        }
        public List<MostAvailableResourceResponse> SelectMostAvailableResource(ApplicationContext context, List<FindResourceRequest> request)
        {
            //all required managers initialization
            UserProfileManager userManager = new UserProfileManager(context);
            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(context);
            ResourceAllocationMonthlyManager monthlyManager = new ResourceAllocationMonthlyManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            TicketManager ticketManagerr = new TicketManager(context);
            GlobalRoleManager roleMgr = new GlobalRoleManager(context);

            int allocationMin = 30;
            int allocationMax = 100;
            string moduleName = uHelper.getModuleNameByTicketId(request.FirstOrDefault().ProjectID);
            List<MostAvailableResourceResponse> lstFinalResponse = new List<MostAvailableResourceResponse>();
            //loading current cpr ticket
            UGITModule module = moduleManager.LoadByName(moduleName);
            string complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ProjectComplexity, module.ModuleTable))
                complexityColumnName = DatabaseObjects.Columns.ProjectComplexity;
            else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CRMProjectComplexity, module.ModuleTable))
                complexityColumnName = DatabaseObjects.Columns.CRMProjectComplexity;

            DataRow projectRow = ticketManagerr.GetByTicketID(module, request.FirstOrDefault().ProjectID, new List<string>() { complexityColumnName });
            int projectComplexity = UGITUtility.StringToInt(Convert.ToString(projectRow[complexityColumnName]));
            List<UserProfile> lstComplexityUsers = new List<UserProfile>();
            List<SummaryResourceProjectComplexity> complexityAboveCurrentCRP = complexityManager.Load(x => Convert.ToInt32(x.Complexity) >= projectComplexity);
            List<string> assignedUsers = new List<string>();
           
            foreach (FindResourceRequest item in request)
            {
                try
                {
                    
                    List<MostAvailableResourceResponse> lstResponse = new List<MostAvailableResourceResponse>();
                    //load all monthly allocation between requested date range
                    List<ResourceAllocationMonthly> lstCrmMonthlyAllocations = monthlyManager.Load(x => x.MonthStartDate >= item.AllocationStartDate || x.MonthStartDate <= item.AllocationEndDate);

                    //feteching selected row group role obj and then selecting users based on role name 
                    GlobalRole role = roleMgr.Get(x => x.Id == item.Type);
                    List<UserProfile> lstUProfile = userManager.Load(x => x.GlobalRoleId == role.Id && x.Enabled);  // userManager.GetUserProfilesByGroupName(role.Name);
                    List<string> selectedUsers = new List<string>();
                    selectedUsers = lstUProfile.Select(x => x.Id).Except(assignedUsers.Select(y => y)).ToList();
                    //code to create response object only for those users who have complexity saved

                    if (selectedUsers.Count == 0)
                    {
                        MostAvailableResourceResponse response = new MostAvailableResourceResponse();
                        response.ID = item.ID;
                        response.AssignedTo = Guid.Empty.ToString();
                        response.AssignedToName = string.Empty;

                        response.Type = item.Type;
                        response.TypeName = role.Name;
                        response.AllocationEndDate = item.AllocationEndDate;
                        response.AllocationStartDate = item.AllocationStartDate;
                        response.PctAllocation = item.PctAllocation;
                        response.PctAllocationConst = item.PctAllocationConst;
                        response.PctAllocationCloseOut = item.PctAllocationCloseOut;

                        lstResponse.Add(response);
                    }

                    foreach (string uProfile in selectedUsers)
                    {
                        UserProfile profile = lstUProfile.FirstOrDefault(x => x.Id == uProfile);
                        MostAvailableResourceResponse response = new MostAvailableResourceResponse();
                        response.ID = item.ID;
                        response.AssignedTo = profile.Id;
                        response.AssignedToName = profile.Name;

                        response.Type = item.Type;
                        response.TypeName = role.Name;
                        response.AllocationEndDate = item.AllocationEndDate;
                        response.AllocationStartDate = item.AllocationStartDate;
                        response.PctAllocation = item.PctAllocation;
                        response.PctAllocationConst = item.PctAllocationConst;
                        response.PctAllocationCloseOut = item.PctAllocationCloseOut;
                        ////selecting monthlly allocation of given user only
                        ////assigning average pct allocation on response object
                        //List<ResourceAllocationMonthly> lstUserAllocation = lstCrmMonthlyAllocations.Where(x => x.Resource == profile.Id).ToList();
                        //response.PctAllocation = 0;
                        //if (lstUserAllocation.Count > 0)
                        //    response.PctAllocation = Math.Round(lstUserAllocation.Sum(x => x.PctAllocation.Value) / lstUserAllocation.Count, 0);

                        if (response.PctAllocation <= 0)
                            response.AllocationRange = 0;
                        else if (response.PctAllocation < allocationMin)
                            response.AllocationRange = 1;
                        else if (response.PctAllocation >= allocationMax)
                            response.AllocationRange = 3;
                        else
                            response.AllocationRange = 2;

                        List<SummaryResourceProjectComplexity> usrComplexities = complexityAboveCurrentCRP.Where(x => x.UserId == uProfile).ToList();
                        if (usrComplexities != null && usrComplexities.Count > 0)
                        {
                            int higestComplxity = usrComplexities.Max(x => x.Complexity);
                            response.Complexity = higestComplxity;
                        }
                        lstResponse.Add(response);
                    }

                    lstResponse = lstResponse.OrderBy(x => x.PctAllocation).ThenByDescending(x => x.Complexity).ToList();

                    MostAvailableResourceResponse bestResource = lstResponse.FirstOrDefault();
                    if (bestResource == null)
                        continue;

                    lstFinalResponse.Add(bestResource);

                    if (!assignedUsers.Contains(bestResource.AssignedTo))
                        assignedUsers.Add(bestResource.AssignedTo);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return lstFinalResponse;

        }

        public void DeleteAllocationTemplate(ApplicationContext context, DeleteTemplateAllocationRequestModel model)
        {
            ProjectAllocationTemplateManager manager = new ProjectAllocationTemplateManager(context);
            if (model.DeleteTemplate)
            {
                ProjectAllocationTemplate projectAllocationTemplate = manager.LoadByID(model.ID);
                if (projectAllocationTemplate != null)
                    manager.Delete(projectAllocationTemplate);
            }
        }

        public void CopyDependentDataFromOPM(string parentTicketID, int opmID)
        {
            string module = uHelper.getModuleNameByTicketId(parentTicketID);
            if (string.IsNullOrEmpty(module))
                return;

            DataRow opmItem = Ticket.GetCurrentTicket(_context, "OPM", opmID.ToString());
            DataRow cprItem = Ticket.GetCurrentTicket(_context, "CPR", parentTicketID);
            List<ProjectEstimatedAllocation> crpAllocations = this.Load(x => x.TicketId == Convert.ToString(opmItem[DatabaseObjects.Columns.TicketId]));
            if (crpAllocations.Count > 0)
            {
                List<AllocationTemplateModel> templateAllocs = new List<AllocationTemplateModel>();
                foreach (ProjectEstimatedAllocation alloc in crpAllocations)
                {
                    templateAllocs.Add(new AllocationTemplateModel()
                    {
                        ProjectID = parentTicketID,
                        AllocationStartDate = alloc.AllocationStartDate,
                        AllocationEndDate = alloc.AllocationEndDate,
                        AssignedTo = alloc.AssignedTo,
                        PctAllocation = alloc.PctAllocation,
                        Type = alloc.Type,
                        Title = alloc.Title,
                    });
                }

                DateTime startDate = UGITUtility.StringToDateTime(cprItem[DatabaseObjects.Columns.PreconStartDate]);
                if (startDate == DateTime.MinValue)
                    startDate = UGITUtility.StringToDateTime(cprItem[DatabaseObjects.Columns.TicketCreationDate]);
                DateTime endDate = UGITUtility.StringToDateTime(cprItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                this.ImportAllocation(_context, templateAllocs, startDate, endDate);
            }
        }

        public void UpdateProjectGroups(string moduleName, string ticketID, List<ProjectEstimatedAllocation> projectEstimatedAllocations = null)
        {
            try
            {
                if (!UGITUtility.IsValidTicketID(ticketID))
                    return;
                List<ProjectEstimatedAllocation> allocations = projectEstimatedAllocations;
                if (allocations == null)
                    allocations = this.Load(x => x.TicketId == ticketID);

                var userByGlobalRoles = allocations.ToLookup(x => x.Type);
                DataRow row = _ticketManager.GetDataRowByTicketId(ticketID);
                //if (tickettable == null || tickettable.Rows.Count == 0)
                //    return;

                //DataRow row = tickettable.Rows[0];
                if (row == null)
                    return;

                Ticket ticket = new Ticket(_context, moduleName);
                GlobalRoleManager roleManager = new GlobalRoleManager(_context);
                GlobalRole uRole;
                var rolesNames = roleManager.Load(x => x.FieldName != null).Select(x => x.FieldName).ToList();
                foreach (var item in rolesNames)
                {
                    if (row.Table.Columns.Contains(item))
                    {
                        row[item] = DBNull.Value;
                    }
                }

                foreach (var g in userByGlobalRoles)
                {
                    string roleFieldName = string.Empty;
                    uRole = roleManager.LoadById(g.Key);
                    if (uRole != null)
                        roleFieldName = uRole.FieldName;

                    if (!string.IsNullOrWhiteSpace(roleFieldName) && row.Table.Columns.Contains(roleFieldName))
                    {
                        row[roleFieldName] = string.Join(",", g.Select(x => x.AssignedTo).Distinct());
                    }
                }

                row[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(row[DatabaseObjects.Columns.TicketStageActionUserTypes]), row);
                if (UGITUtility.IfColumnExists(row, "ResourceAllocationCount"))
                {
                    row["ResourceAllocationCount"] = $"{allocations.Where(x => x.TicketId == ticketID && x.Deleted == false).Count()};#{allocations.Where(x => x.TicketId == ticketID && x.AssignedTo == Guid.Empty.ToString() && x.Deleted == false).Count()}";
                }
                if (UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.AcquisitionCost))
                {
                    //row[DatabaseObjects.Columns.AcquisitionCost] = GetAcquisitionCost(_context, ticketID, moduleName);
                    DataTable dt = GetForecastAndAcquisitionCosts(ticketID);
                    if (dt.Rows.Count > 0)
                    {
                        row[DatabaseObjects.Columns.AcquisitionCost] = dt.Rows[0]["ForecastedAcquisitionCost"];
                        row[DatabaseObjects.Columns.ActualAcquisitionCost] = dt.Rows[0][DatabaseObjects.Columns.ActualAcquisitionCost];
                        row[DatabaseObjects.Columns.ForecastedProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ForecastedProjectCost];
                        row[DatabaseObjects.Columns.ActualProjectCost] = dt.Rows[0][DatabaseObjects.Columns.ActualProjectCost];
                    }
                }
                ticket.CommitChanges(row);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public ProjectEstimatedAllocation MapIntoEstAllocationFromResourceObject(RResourceAllocation allocation)
        {
            ProjectEstimatedAllocation estimatedAllocation = new ProjectEstimatedAllocation();
            estimatedAllocation.AllocationStartDate = allocation.AllocationStartDate;
            estimatedAllocation.AllocationEndDate = allocation.AllocationEndDate;
            estimatedAllocation.AssignedTo = allocation.Resource;
            estimatedAllocation.PctAllocation = UGITUtility.StringToDouble(allocation.PctAllocation);
            estimatedAllocation.TicketId = allocation.ResourceWorkItems.WorkItem;
            estimatedAllocation.Title = allocation.ResourceWorkItems.SubWorkItem;
            estimatedAllocation.Type = allocation.RoleId;
            estimatedAllocation.SoftAllocation = allocation.SoftAllocation;
            estimatedAllocation.NonChargeable = allocation.NonChargeable;
            return estimatedAllocation;
        }

        public object GetAcquisitionCost(ApplicationContext _context, string TicketID, string moduleName)
        {
            object AcquisitionCost = null;
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("TenantID", _context.TenantID);
            arrParams.Add("TicketID", TicketID);
            arrParams.Add("Module", moduleName);
            DataTable dt = uGovernIT.DAL.uGITDAL.ExecuteDataSetWithParameters("GetAcquisitionCost", arrParams);
            if (dt != null && dt.Rows.Count > 0)
            {
                AcquisitionCost = dt.Rows[0]["AcquisitionCost"];
            }

            return AcquisitionCost;
        }

        public DataTable GetForecastAndAcquisitionCosts(string TicketID)
        {
            DataTable data = new DataTable();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("TenantID", _context.TenantID);
            arrParams.Add("TicketID", TicketID);
            DataTable dt = uGITDAL.ExecuteDataSetWithParameters("GetForecastAndAcquisitionCosts", arrParams);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt;
            }

            return data;
        }

        public void UpdateAllocationForDisabledUsers(string userId = "")
        {
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(_context);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(_context);
            UserProfileManager ObjUserProfileManager = new UserProfileManager(_context);
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);

            List<UserProfile> userProfiles = ObjUserProfileManager.GetUsersProfile();
            List<GlobalRole> roles = roleManager.Load();

            List<UserProfile> disabledUsers = null;
            if (string.IsNullOrWhiteSpace(userId))
            {
                disabledUsers = userProfiles.Where(x => !x.Enabled).ToList();
            }
            else 
            {
                disabledUsers = userProfiles.Where(x => x.Id == userId).ToList();
            }

            List<ProjectEstimatedAllocation> projectAllocations = Load(x => disabledUsers.Select(y => y.Id).Contains(x.AssignedTo) && x.Deleted != true);
            List<RResourceAllocation> rAllocations = allocationManager.Load(x => disabledUsers.Select(y => y.Id).Contains(x.Resource) && x.Deleted != true);
            
            List<long> workitemIds = new List<long>();
            List<Tuple<string, string>> historyDescLst = new List<Tuple<string, string>>();
            List<ProjectEstimatedAllocation> allocationToAdd = new List<ProjectEstimatedAllocation>();
            
            foreach (UserProfile profile in disabledUsers)
            {
                DateTime endDate = profile.UGITEndDate;
                List<ProjectEstimatedAllocation> userProjectAllocations = projectAllocations.Where(x => x.AssignedTo == profile.Id).ToList();
                ULog.WriteLog("Disabled User >> " + profile.Name);
                foreach (ProjectEstimatedAllocation item in userProjectAllocations)
                {
                    RResourceAllocation rItem = rAllocations.FirstOrDefault(x => x.ProjectEstimatedAllocationId == item.ID.ToString());
                    if (rItem == null)
                        continue;
                    if (item.AllocationStartDate > endDate && item.AllocationEndDate > endDate)
                    {
                        item.AssignedTo = Guid.Empty.ToString();
                        CRMProjAllocManager.Update(item);

                        string userRole = "";
                        var role = roles.FirstOrDefault(x => x.Id == item.Type);
                        if (role != null)
                            userRole = role.Name;
                        
                        rItem.Resource = Guid.Empty.ToString();
                        rItem.ResourceWorkItems = new ResourceWorkItems()
                        {
                            ID = rItem.ResourceWorkItemLookup,
                            WorkItem = item.TicketId,
                            WorkItemType = uHelper.getModuleNameByTicketId(item.TicketId),
                            SubWorkItem = userRole,
                        };
                        allocationManager.Save(rItem);
                        string historyDesc = string.Format("Disabled user >> Updated allocation from user: {0} - {1} {2}% {3}-{4}  to  {5} - {6} {7}% {8}-{9}", profile.Name, userRole, item.PctAllocation, String.Format("{0:MM/dd/yyyy}", item.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", item.AllocationEndDate),
                                                                                                                                    "undefined", userRole, item.PctAllocation, String.Format("{0:MM/dd/yyyy}", item.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", item.AllocationEndDate));
                        historyDescLst.Add(new Tuple<string, string>(item.TicketId, historyDesc));
                        workitemIds.Add(rItem.ResourceWorkItemLookup);
                    }
                    else if (item.AllocationEndDate > endDate)
                    {
                        var allocation1 = DeepCopy(item);
                        var allocation2 = DeepCopy(item);

                        allocation1.AllocationEndDate = endDate;
                        CRMProjAllocManager.Update(allocation1);
                        rItem.AllocationEndDate = endDate;
                        string userRole = "";
                        var role = roles.FirstOrDefault(x => x.Id == item.Type);
                        if (role != null)
                            userRole = role.Name;
                        
                        rItem.ResourceWorkItems = new ResourceWorkItems()
                        {
                            ID = rItem.ResourceWorkItemLookup,
                            WorkItem = item.TicketId,
                            WorkItemType = uHelper.getModuleNameByTicketId(item.TicketId),
                            SubWorkItem = userRole,
                        };
                        allocationManager.Save(rItem);
                        string historyDesc = string.Format("Disabled user >> Updated allocation from user: {0} - {1} {2}% {3}-{4}  to  {5} - {6} {7}% {8}-{9}", profile.Name, userRole, item.PctAllocation, String.Format("{0:MM/dd/yyyy}", item.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", item.AllocationEndDate),
                                                                                                                                    profile.Name, userRole, item.PctAllocation, String.Format("{0:MM/dd/yyyy}", item.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", endDate));
                        historyDescLst.Add(new Tuple<string, string>(item.TicketId, historyDesc));
                        workitemIds.Add(rItem.ResourceWorkItemLookup);
                        allocation2.AllocationStartDate = endDate.AddWorkingDays(1);
                        allocation2.ID = 0;
                        allocation2.AssignedTo = Guid.Empty.ToString();
                        allocationToAdd.Add(allocation2);
                    }
                }
            }
            if (allocationToAdd?.Count > 0)
            {
                foreach (var allocation in allocationToAdd)
                {
                    RResourceAllocation rAllocation = new RResourceAllocation();
                    rAllocation.TicketID = allocation.TicketId;
                    rAllocation.RoleId = allocation.Type;
                    rAllocation.PctAllocation = allocation.PctAllocation;
                    rAllocation.PctEstimatedAllocation = allocation.PctAllocation;
                    rAllocation.SoftAllocation = allocation.SoftAllocation;
                    rAllocation.NonChargeable = allocation.NonChargeable;

                    rAllocation.AllocationStartDate = allocation.AllocationStartDate;
                    rAllocation.AllocationEndDate = allocation.AllocationEndDate;
                    rAllocation.Resource = allocation.AssignedTo;
                    CRMProjAllocManager.Insert(allocation);
                    if (allocation.ID > 0)
                    {
                        rAllocation.ProjectEstimatedAllocationId = UGITUtility.ObjectToString(allocation.ID);
                    }

                    string userRole = "";
                    var role = roles.FirstOrDefault(x => x.Id == allocation.Type);
                    if (role != null)
                        userRole = role.Name;

                    rAllocation.ResourceWorkItems = new ResourceWorkItems()
                    {
                        WorkItem = allocation.TicketId,
                        WorkItemType = uHelper.getModuleNameByTicketId(allocation.TicketId),
                        SubWorkItem = userRole,
                    };
                    allocationManager.Save(rAllocation);
                    if (rAllocation.ResourceWorkItemLookup > 0)
                    {
                        workitemIds.Add(rAllocation.ResourceWorkItemLookup);
                    }
                    
                    string historyDesc = string.Format("Disabled user >> Created new allocation for user: {0} - {1} {2}% {3}-{4}", userProfiles.FirstOrDefault(x => x.Id == allocation.AssignedTo)?.Name, userRole, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate));
                    historyDescLst.Add(new Tuple<string, string>(allocation.TicketId, historyDesc));
                }
            }

            if (workitemIds?.Count > 0)
            {
                ThreadStart threadStartMethod = delegate ()
                {
                    workitemIds.ForEach(wItem =>
                    {
                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, wItem);
                    });
                    historyDescLst.ForEach(hDesc =>
                    {
                        ResourceAllocationManager.UpdateHistory(_context, hDesc.Item2, hDesc.Item1);
                        ULog.WriteLog(_context.CurrentUser.Name + " >> " + hDesc.Item1 + hDesc.Item2);
                    });
                };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
        }
        public void UpdatedAllocationDates(string projectID, DataRow newData, bool updatePastDates = true)
        {
            //bool ChangeAllocationDatesAutomatically = _configurationVariableManager.GetValueAsBool(ConfigConstants.ChangeAllocationDatesAutomatically);
            //if (ChangeAllocationDatesAutomatically)
            //{
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(_context);
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            string moduleName = uHelper.getModuleNameByTicketId(projectID);
            DataRow currentTicket = Ticket.GetCurrentTicket(_context, moduleName, projectID);
            List<string> historyDesc = new List<string>();

            List<ProjectEstimatedAllocation> projectAllocations = Load(x => x.TicketId == projectID && x.Deleted != true).OrderBy(o => o.AssignedTo).ThenBy(t => t.Type).ThenBy(s => s.AllocationStartDate).ToList();

            DateTime preconStartTemp = DateTime.MinValue, preconEnd = DateTime.MinValue, constStart = DateTime.MinValue, constEnd = DateTime.MinValue,
                closeoutStart = DateTime.MinValue, closeoutEndTemp = DateTime.MinValue;
            if (currentTicket != null)
            {
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconEndDate))
                {
                    preconStartTemp = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconStartDate]);
                    preconEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconEndDate]);
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    constStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    constEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    if ((preconEnd == DateTime.MinValue || preconStartTemp == DateTime.MinValue) && constStart != DateTime.MinValue)
                    {
                        preconEnd = constStart.AddDays(-1);
                        preconStartTemp = projectAllocations != null && projectAllocations.Any(o => o.AllocationStartDate.Value < preconEnd)
                            ? projectAllocations.Where(o => o.AllocationStartDate < preconEnd).Min(o => o.AllocationStartDate.Value) : preconEnd.AddDays(-1);
                    }
                    
                    if ((constStart == DateTime.MinValue || constEnd == DateTime.MinValue) && preconEnd != DateTime.MinValue)
                    {
                        constStart = preconEnd.AddDays(1);
                        constEnd = projectAllocations != null && projectAllocations.Any(o => o.AllocationEndDate.Value > constStart)
                            ? projectAllocations.Where(o => o.AllocationEndDate.Value > constStart).Max(o => o.AllocationEndDate.Value) : constStart.AddDays(1);
                    }
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutDate))
                {
                    closeoutStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                    closeoutEndTemp = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutDate]);

                    closeoutStart = closeoutStart == DateTime.MinValue && constEnd != DateTime.MinValue
                        ? UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEnd))
                        : closeoutStart;
                    if (closeoutEndTemp == DateTime.MinValue && closeoutStart != DateTime.MinValue)
                    {
                        closeoutEndTemp = closeoutStart.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                    }
                }
            }

            DateTime preconStartNewTemp = DateTime.MinValue, preconEndNew = DateTime.MinValue, constStartNew = DateTime.MinValue, constEndNew = DateTime.MinValue,
                closeoutStartNew = DateTime.MinValue, closeoutEndNewTemp = DateTime.MinValue;
            if (newData != null)
            {
                if (UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.PreconStartDate) &&
                    UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.PreconEndDate))
                {
                    preconStartNewTemp = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.PreconStartDate]);
                    preconEndNew = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.PreconEndDate]);
                }
                if (UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                    UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    constStartNew = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    constEndNew = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    if (preconEndNew == DateTime.MinValue && constStartNew != DateTime.MinValue)
                    {
                        preconEndNew = constStartNew.AddDays(-1);
                    }
                    if (preconStartNewTemp == DateTime.MinValue && preconStartTemp != DateTime.MinValue && preconEnd != DateTime.MinValue)
                    {
                        preconStartNewTemp = preconEndNew.AddDays(-(preconEnd - preconStartTemp).Days);
                    }

                    if (constStartNew == DateTime.MinValue && preconEndNew != DateTime.MinValue)
                    {
                        constStartNew = preconEndNew.AddDays(1);
                    }
                    if (constEndNew == DateTime.MinValue && constStartNew != DateTime.MinValue && constEnd != DateTime.MinValue && constStart != DateTime.MinValue)
                    {
                        constEndNew = constStartNew.AddDays((constEnd - constStart).Days + 1);
                    }
                }
                if (UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.CloseoutStartDate) &&
                    UGITUtility.IfColumnExists(newData, DatabaseObjects.Columns.CloseoutDate))
                {
                    closeoutStartNew = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.CloseoutStartDate]);
                    closeoutEndNewTemp = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.CloseoutDate]);

                    closeoutStartNew = closeoutStartNew == DateTime.MinValue && constEndNew != DateTime.MinValue
                        ? UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndNew))
                        : closeoutStartNew;
                    if (closeoutEndNewTemp == DateTime.MinValue && closeoutStartNew != DateTime.MinValue)
                    {
                        closeoutEndNewTemp = closeoutStart.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                    }
                }
            }

            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            string prevUsername = "", prevUserRole = "";
            DateTime prevAllocEndDate = DateTime.MinValue;
            if (preconStartTemp != DateTime.MinValue && preconEnd != DateTime.MinValue && constStart != DateTime.MinValue && constEnd != DateTime.MinValue
            && preconStartNewTemp != DateTime.MinValue && preconEndNew != DateTime.MinValue && constStartNew != DateTime.MinValue && constEndNew != DateTime.MinValue)
            {
                foreach (ProjectEstimatedAllocation alloc in projectAllocations)
                {
                    DateTime preconStart = preconStartTemp, preconStartNew = preconStartNewTemp
                        , closeoutEnd = closeoutEndTemp, closeoutEndNew = closeoutEndNewTemp;
                    DateTime allocStartDate = alloc.AllocationStartDate.Value, allocEndDate = alloc.AllocationEndDate.Value;

                    if (alloc.IsLocked)
                        continue;
                    //if (alloc.AllocationStartDate < DateTime.Now && alloc.AllocationEndDate < DateTime.Now)
                    //    continue;
                    //if (alloc.AllocationStartDate < DateTime.Now)
                    //    allocStartDate = DateTime.Now;

                    if (closeoutEnd != DateTime.MinValue && alloc.AllocationEndDate > closeoutEnd)
                    {
                        int noOfExtraDaysInCloseOut = (alloc.AllocationEndDate.Value - closeoutEnd).Days;
                        closeoutEnd = alloc.AllocationEndDate.Value;
                        closeoutEndNew = closeoutEndNew.AddDays(noOfExtraDaysInCloseOut);
                    }

                    if (preconStart != DateTime.MinValue && alloc.AllocationStartDate < preconStart)
                    {
                        int noOfExtraDaysInPrecon = (preconStart - alloc.AllocationStartDate.Value).Days;
                        preconStart = alloc.AllocationStartDate.Value;
                        preconStartNew = preconStartNew.AddDays(-noOfExtraDaysInPrecon);
                    }
                    
                    string userName = _context.UserManager.GetUserNameById(alloc.AssignedTo);
                    string userRole = string.Empty;
                    var role = roleManager.Get(x => x.Id == alloc.Type);
                    if (role != null)
                        userRole = role.Name;

                    DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue, startDateNew = DateTime.MinValue, endDateNew = DateTime.MinValue;
                    bool isInPreconStage = false, isInConstStage = false, isInCloseoutStage = false;
                    if (currentTicket != null)
                    {
                        if (preconEnd != DateTime.MinValue && preconStart != DateTime.MinValue)
                        {
                            if (allocStartDate <= preconEnd && alloc.AllocationEndDate >= preconStart)
                            {
                                isInPreconStage = true;
                            }
                        }

                        if (constStart != DateTime.MinValue && constEnd != DateTime.MinValue)
                        {
                            if (allocStartDate <= constEnd && alloc.AllocationEndDate >= constStart)
                            {
                                isInConstStage = true;
                            }
                        }

                        if (closeoutEnd != DateTime.MinValue && closeoutStart != DateTime.MinValue)
                        {
                            if (allocStartDate <= closeoutEnd && alloc.AllocationEndDate >= closeoutStart)
                            {
                                isInCloseoutStage = true;
                            }
                        }
                        if (isInPreconStage && !isInConstStage && !isInCloseoutStage)
                        {
                            startDate = preconStart;
                            endDate = preconEnd;
                            startDateNew = preconStartNew;
                            endDateNew = preconEndNew;
                        }
                        else if (!isInPreconStage && isInConstStage && !isInCloseoutStage)
                        {
                            startDate = constStart;
                            endDate = constEnd;
                            startDateNew = constStartNew;
                            endDateNew = constEndNew;
                        }
                        else if (!isInPreconStage && !isInConstStage && isInCloseoutStage)
                        {
                            startDate = closeoutStart;
                            endDate = closeoutEnd;
                            startDateNew = closeoutStartNew;
                            endDateNew = closeoutEndNew;
                        }
                        else if (isInPreconStage && isInConstStage && !isInCloseoutStage)
                        {
                            startDate = preconStart;
                            endDate = constEnd;
                            startDateNew = preconStartNew;
                            endDateNew = constEndNew;
                        }
                        else if (!isInPreconStage && isInConstStage && isInCloseoutStage)
                        {
                            startDate = constStart;
                            endDate = closeoutEnd;
                            startDateNew = constStartNew;
                            endDateNew = closeoutEndNew;
                        }
                        else if (isInPreconStage && isInConstStage && isInCloseoutStage)
                        {
                            startDate = preconStart;
                            endDate = closeoutEnd;
                            startDateNew = preconStartNew;
                            endDateNew = closeoutEndNew;
                        }

                        double daysDiff = startDate != DateTime.MinValue && endDate != DateTime.MinValue
                                ? (endDate - startDate).TotalDays : 0;
                        double daysDiffNew = endDateNew != DateTime.MinValue && startDateNew != DateTime.MinValue
                            ? (endDateNew - startDateNew).TotalDays : 0;
                        double daysRatio = daysDiff > 0 && daysDiffNew > 0
                            ? daysDiffNew / daysDiff : 0.0;
                        int diffStartDate = Convert.ToInt16(Math.Round((allocStartDate.Date - startDate.Date).Days * daysRatio));
                        int diffEndDate = Convert.ToInt16(Math.Round((allocEndDate.Date - allocStartDate.Date).Days * daysRatio));

                        //if (alloc.AllocationStartDate < DateTime.Now)
                        //{
                        //    DateTime tempStartDate = startDateNew.AddDays(diffStartDate);
                        //    alloc.AllocationEndDate = tempStartDate.AddDays(diffEndDate);
                        //    alloc.AllocationEndDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, alloc.AllocationEndDate.Value.AddDays(-1)));
                        //}
                        //else
                        //{
                        alloc.AllocationStartDate = startDateNew.AddDays(diffStartDate);
                        alloc.AllocationEndDate = alloc.AllocationStartDate.Value.AddDays(diffEndDate);
                        alloc.AllocationStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, alloc.AllocationStartDate.Value.AddDays(-1)));
                        alloc.AllocationEndDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, alloc.AllocationEndDate.Value.AddDays(-1)));
                        //}

                        if (prevAllocEndDate != DateTime.MinValue && userName == prevUsername
                            && userRole == prevUserRole && prevAllocEndDate == alloc.AllocationStartDate)
                        {
                            if (alloc.AllocationStartDate == alloc.AllocationEndDate)
                            {
                                alloc.AllocationEndDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, alloc.AllocationEndDate.Value));
                            }
                            alloc.AllocationStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, alloc.AllocationStartDate.Value));
                        }

                        if (!updatePastDates)
                        {
                            if (allocStartDate < DateTime.Now)
                            {
                                alloc.AllocationStartDate = allocStartDate;
                            }

                            if (allocEndDate < DateTime.Now)
                            {
                                alloc.AllocationEndDate = allocEndDate;
                            }

                            if (alloc.AllocationEndDate < alloc.AllocationStartDate)
                            {
                                alloc.AllocationStartDate = allocStartDate;
                                alloc.AllocationEndDate = allocEndDate;
                            }
                        }

                        prevAllocEndDate = alloc.AllocationEndDate.Value;
                        prevUsername = userName;
                        prevUserRole = userRole;

                        if (allocStartDate != alloc.AllocationStartDate || allocEndDate != alloc.AllocationEndDate)
                        {
                            historyDesc.Add(string.Format("Updated allocation date for user: {0} - {1} from {2}-{3}  to  {4}-{5}", userName, userRole, String.Format("{0:MM/dd/yyyy}", alloc.AllocationStartDate < DateTime.Now ? alloc.AllocationStartDate : allocStartDate)
                                , String.Format("{0:MM/dd/yyyy}", allocEndDate), String.Format("{0:MM/dd/yyyy}", alloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", alloc.AllocationEndDate)));
                            CRMProjAllocManager.Update(alloc);

                            lstUserWithPercetage.Add(
                            new UserWithPercentage()
                            {
                                EndDate = alloc.AllocationEndDate ?? DateTime.MinValue,
                                StartDate = alloc.AllocationStartDate ?? DateTime.MinValue,
                                Percentage = alloc.PctAllocation,
                                UserId = alloc.AssignedTo,
                                RoleTitle = userRole,
                                ProjectEstiAllocId = UGITUtility.ObjectToString(alloc.ID),
                                RoleId = alloc.Type,
                                SoftAllocation = alloc.SoftAllocation,
                            });
                        }
                    }
                }

                var taskManager = new UGITTaskManager(_context);
                List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(projectID), projectID);
                List<string> lstUsers = projectAllocations.Select(a => a.AssignedTo).ToList();
                var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                // Only create allocation enties if user is not in schedule
                //newAllocatedUsers = newAllocatedUsers.Union(oldAllocatedUsers).ToList();
                try
                {
                    if (res == null || res.Count == 0)
                    {
                        ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(projectID), projectID, lstUserWithPercetage, lstUsers); ResourceAllocationManager.UpdateHistory(_context, historyDesc, projectID); };
                        Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                        sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                        sThreadStartMethodUpdateCPRProjectAllocation.Start();
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            //}
        }

        public void UpdatedAllocationDatesNew(string baseProjectId, DataRow newData)
        {
            uGovernIT.Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(_context);
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(_context);
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            UGITModule baseModule = moduleViewManager.LoadByName(uHelper.getModuleNameByTicketId(baseProjectId));
            DataRow baseTicketObj = ticketManager.GetByTicketID(baseModule, baseProjectId);
            List<ProjectEstimatedAllocation> baseProjectAllocations = this.Load(x => x.TicketId == baseProjectId && x.Deleted != true);
            List<ProjectEstimatedAllocation> splitedAllocations = this.SplitProjectAllocation(baseProjectId);
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            UserProfileManager userManager = new UserProfileManager(_context);
            List<UserProfile> userProfiles = userManager.GetUsersProfile();
            List<string> historyDesc = new List<string>();

            // base project Start Date, End Date
            var templateStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconStartDate]);
            var templateEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseOutDate]);

            var preconStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconStartDate]);
            var preconEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.PreconEndDate]);
            var constStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.EstimatedConstructionStart]);
            var constEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            var closeoutStartDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseoutStartDate]);
            var closeoutEndDate = UGITUtility.StringToDateTime(baseTicketObj[DatabaseObjects.Columns.CloseOutDate]);
            if (closeoutStartDate == DateTime.MinValue && constEndDate != DateTime.MinValue)
            {
                closeoutStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDate));
            }
            if (closeoutEndDate == DateTime.MinValue && closeoutStartDate != DateTime.MinValue)
            {
                closeoutEndDate = closeoutStartDate.AddWorkingDays(uHelper.getCloseoutperiod(_context));
            }

            var preconStartDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.PreconStartDate]);
            var preconEndDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.PreconEndDate]);
            var constStartDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.EstimatedConstructionStart]);
            var constEndDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            var closeoutStartDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.CloseoutStartDate]);
            var closeoutEndDateProject = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.CloseOutDate]);
            if (closeoutStartDateProject == DateTime.MinValue && constEndDateProject != DateTime.MinValue)
            {
                closeoutStartDateProject = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, constEndDateProject));
            }
            if (closeoutEndDateProject == DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue)
            {
                closeoutEndDateProject = closeoutStartDateProject.AddWorkingDays(uHelper.getCloseoutperiod(_context));
            }

            if (preconStartDate != DateTime.MinValue && preconEndDate != DateTime.MinValue && constStartDate != DateTime.MinValue && constEndDate != DateTime.MinValue
                && preconStartDateProject != DateTime.MinValue && preconEndDateProject != DateTime.MinValue && constStartDateProject != DateTime.MinValue && constEndDateProject != DateTime.MinValue)
            {
                DateTime tempStartDate = DateTime.MinValue;
                DateTime tempEndDate = DateTime.MinValue;
                int addDaysToPrecon = 0, addDaysToCloseout = 0;
                double preconRatio = 1, clouseOutRatio = 1;
                if (baseProjectAllocations.Count > 0)
                {
                    tempStartDate = baseProjectAllocations.Where(x => x.AllocationStartDate.HasValue).Min(x => x.AllocationStartDate.Value);
                    tempEndDate = baseProjectAllocations.Where(x => x.AllocationEndDate.HasValue).Max(x => x.AllocationEndDate.Value);
                }

                if (tempStartDate < templateStartDate)
                {
                    addDaysToPrecon = (tempStartDate - templateStartDate).Days;
                    if (preconEndDateProject != DateTime.MinValue && preconStartDateProject != DateTime.MinValue
                        && preconEndDate != DateTime.MinValue && preconStartDate != DateTime.MinValue)
                    {
                        preconRatio = Convert.ToDouble((preconEndDateProject - preconStartDateProject).Days) / Convert.ToDouble((preconEndDate - preconStartDate).Days);
                    }
                    templateStartDate = tempStartDate;
                }

                if (tempEndDate > templateEndDate)
                {
                    addDaysToCloseout = (tempEndDate - templateEndDate).Days;
                    if (closeoutEndDateProject != DateTime.MinValue && closeoutStartDateProject != DateTime.MinValue
                        && closeoutEndDate != DateTime.MinValue && closeoutStartDate != DateTime.MinValue)
                    {
                        clouseOutRatio = Convert.ToDouble((closeoutEndDateProject - closeoutStartDateProject).Days) / Convert.ToDouble((closeoutEndDate - closeoutStartDate).Days);
                    }
                    templateEndDate = tempEndDate;
                }

                // Current Project Start and End Date.
                var projectEndDate = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.CloseOutDate]); ;
                var projectStartDate = UGITUtility.StringToDateTime(newData[DatabaseObjects.Columns.PreconStartDate]);

                foreach (ProjectEstimatedAllocation tempAlloc in splitedAllocations)
                {
                    if (tempAlloc.IsLocked)
                        continue;
                    var startDate = tempAlloc.AllocationStartDate;
                    var endDate = tempAlloc.AllocationEndDate;
                    bool pOverlaps = false, cOverlaps = false, coOverlaps = false;
                    if (startDate <= preconEndDate && endDate >= preconStartDate)
                    {
                        pOverlaps = true;
                    }
                    if (startDate <= constEndDate && endDate >= constStartDate)
                    {
                        cOverlaps = true;
                    }
                    if (startDate <= closeoutEndDate && endDate >= closeoutStartDate)
                    {
                        coOverlaps = true;
                    }
                    if (pOverlaps)
                    {
                        templateStartDate = preconStartDate.AddDays(addDaysToPrecon);
                        templateEndDate = preconEndDate;
                        projectStartDate = preconStartDateProject.AddDays(preconRatio * addDaysToPrecon);
                        projectEndDate = preconEndDateProject;
                    }
                    if (cOverlaps)
                    {
                        templateStartDate = constStartDate;
                        templateEndDate = constEndDate;
                        projectStartDate = constStartDateProject;
                        projectEndDate = constEndDateProject;
                    }
                    if (coOverlaps)
                    {
                        templateStartDate = closeoutStartDate;
                        templateEndDate = closeoutEndDate.AddDays(addDaysToCloseout);
                        projectStartDate = closeoutStartDateProject;
                        projectEndDate = closeoutEndDateProject.AddDays(clouseOutRatio * addDaysToCloseout);
                    }

                    int templateTticketDateDiff = templateEndDate != DateTime.MinValue && templateStartDate != DateTime.MinValue
                    ? (templateEndDate - templateStartDate).Days : 0;
                    int newTicketDiffDays = projectEndDate != DateTime.MinValue && projectStartDate != DateTime.MinValue
                        ? (projectEndDate - projectStartDate).Days : 0;
                    double daysRatio = newTicketDiffDays > 0 && templateTticketDateDiff > 0
                        ? Convert.ToDouble(newTicketDiffDays) / Convert.ToDouble(templateTticketDateDiff) : 0.0;

                    int diffStartDateTemplate = Convert.ToInt16((tempAlloc.AllocationStartDate.Value.Date - templateStartDate.Date).Days * daysRatio);
                    int diffEndDateTemplate = Convert.ToInt16((tempAlloc.AllocationEndDate.Value.Date - tempAlloc.AllocationStartDate.Value.Date).Days * daysRatio);
                    tempAlloc.AllocationStartDate = projectStartDate.AddDays(diffStartDateTemplate);
                    tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate.Value.Date.AddDays(diffEndDateTemplate);

                    if (projectStartDate != DateTime.MinValue && tempAlloc.AllocationStartDate < projectStartDate)
                        tempAlloc.AllocationStartDate = projectStartDate;

                    if (projectEndDate != DateTime.MinValue && tempAlloc.AllocationEndDate > projectEndDate)
                        tempAlloc.AllocationEndDate = projectEndDate;
                    if (tempAlloc.AllocationEndDate < tempAlloc.AllocationStartDate)
                    {
                        tempAlloc.AllocationEndDate = tempAlloc.AllocationStartDate;
                    }

                    string userName = userProfiles.Find(x => x.Id == tempAlloc.AssignedTo)?.Name ?? string.Empty;
                    string roleName = string.Empty;
                    GlobalRole uRole = roleManager.Get(x => x.Id == tempAlloc.Type);
                    if (uRole != null)
                        roleName = uRole.Name;

                    if (startDate != tempAlloc.AllocationStartDate || endDate != tempAlloc.AllocationEndDate || tempAlloc.ID < 0)
                    {
                        if (tempAlloc.ID > 0)
                        {
                            historyDesc.Add(string.Format("Updated allocation date for user: {0} - {1} from {2}-{3}  to  {4}-{5}", userName, roleName, String.Format("{0:MM/dd/yyyy}", startDate)
                                , String.Format("{0:MM/dd/yyyy}", endDate), String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationEndDate)));
                            CRMProjAllocManager.Update(tempAlloc);
                        }
                        else
                        {
                            historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", userName, roleName, tempAlloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", tempAlloc.AllocationEndDate)));
                            tempAlloc.ID = 0;
                            CRMProjAllocManager.Insert(tempAlloc);
                        }
                        lstUserWithPercetage.Add(
                                new UserWithPercentage()
                                {
                                    EndDate = tempAlloc.AllocationEndDate ?? DateTime.MinValue,
                                    StartDate = tempAlloc.AllocationStartDate ?? DateTime.MinValue,
                                    Percentage = tempAlloc.PctAllocation,
                                    UserId = tempAlloc.AssignedTo,
                                    RoleTitle = roleName,
                                    ProjectEstiAllocId = UGITUtility.ObjectToString(tempAlloc.ID),
                                    RoleId = tempAlloc.Type,
                                    SoftAllocation = tempAlloc.SoftAllocation,
                                });
                    }
                }
                var taskManager = new UGITTaskManager(_context);
                List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(baseProjectId), baseProjectId);
                List<string> lstUsers = splitedAllocations.Select(a => a.AssignedTo).ToList();
                var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                // Only create allocation enties if user is not in schedule
                //newAllocatedUsers = newAllocatedUsers.Union(oldAllocatedUsers).ToList();
                try
                {
                    if (res == null || res.Count == 0)
                    {
                        ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(baseProjectId), baseProjectId, lstUserWithPercetage, lstUsers); ResourceAllocationManager.UpdateHistory(_context, historyDesc, baseProjectId); };
                        Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                        sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                        sThreadStartMethodUpdateCPRProjectAllocation.Start();
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }

        /// <summary>
        /// To change the allocation type during hold/unhold.
        /// hold => Soft, UnHold => Hard
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="isSoftAllocation"></param>
        public void UpdatedAllocationType(string projectID, bool isSoftAllocation)
        {
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            string moduleName = uHelper.getModuleNameByTicketId(projectID);
            DataRow currentTicket = Ticket.GetCurrentTicket(_context, moduleName, projectID);
            List<ProjectEstimatedAllocation> projectAllocations = this.Load(x => x.TicketId == projectID && x.Deleted != true).OrderBy(o => o.AssignedTo).ThenBy(t => t.Type).ThenBy(s => s.AllocationStartDate).ToList();
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            foreach (ProjectEstimatedAllocation alloc in projectAllocations)
            {
                alloc.SoftAllocation = isSoftAllocation;
                this.Update(alloc);
                string roleName = string.Empty;
                GlobalRole uRole = roleManager.Get(x => x.Id == alloc.Type);
                if (uRole != null)
                    roleName = uRole.Name;

                lstUserWithPercetage.Add(
                        new UserWithPercentage()
                        {
                            EndDate = alloc.AllocationEndDate ?? DateTime.MinValue,
                            StartDate = alloc.AllocationStartDate ?? DateTime.MinValue,
                            Percentage = alloc.PctAllocation,
                            UserId = alloc.AssignedTo,
                            RoleTitle = roleName,
                            ProjectEstiAllocId = UGITUtility.ObjectToString(alloc.ID),
                            RoleId = alloc.Type,
                            SoftAllocation = alloc.SoftAllocation,
                        });
            }

            var taskManager = new UGITTaskManager(_context);
            List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(projectID), projectID);
            List<string> lstUsers = projectAllocations.Select(a => a.AssignedTo).ToList();
            var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
            try
            {
                if (res == null || res.Count == 0)
                {
                    ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(projectID), projectID, lstUserWithPercetage, lstUsers); };
                    Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                    sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                    sThreadStartMethodUpdateCPRProjectAllocation.Start();
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public void UpdateProjectAllocationsAfterOnHold(string projectID)
        {
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            string moduleName = uHelper.getModuleNameByTicketId(projectID);
            DataRow currentTicket = Ticket.GetCurrentTicket(_context, moduleName, projectID);
            List<ProjectEstimatedAllocation> oldprojectAllocations = this.Load(x => x.TicketId == projectID && x.Deleted != true).OrderBy(o => o.AssignedTo).ThenBy(t => t.Type).ThenBy(s => s.AllocationStartDate).ToList();
            List<ProjectEstimatedAllocation> allocationsNeedToSplit = new List<ProjectEstimatedAllocation>();
            List<ProjectEstimatedAllocation> allocationsNeedToUpdate = new List<ProjectEstimatedAllocation>();
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();

            List<string> historyDesc = new List<string>();

            foreach (ProjectEstimatedAllocation allocation in oldprojectAllocations)
            {
                if (!allocation.SoftAllocation)
                {
                    if (allocation.AllocationStartDate <= DateTime.Now && allocation.AllocationEndDate >= DateTime.Now)
                    {
                        allocationsNeedToSplit.Add(allocation);
                    }
                    if(allocation.AllocationStartDate > DateTime.Now && allocation.AllocationEndDate > DateTime.Now)
                    {
                        allocationsNeedToUpdate.Add(allocation);
                    }
                }
            }
            
            foreach(ProjectEstimatedAllocation alloc in allocationsNeedToSplit)
            {
                string roleName = GetRoleNameBasedOnId(roleManager, alloc);

                ProjectEstimatedAllocation preHoldAlloc = new ProjectEstimatedAllocation()
                {
                    ID = 0,
                    AllocationStartDate = alloc.AllocationStartDate,
                    AllocationEndDate = DateTime.Now,
                    AssignedTo = alloc.AssignedTo,
                    PctAllocation = alloc.PctAllocation,
                    SoftAllocation = false,
                    IsLocked = alloc.IsLocked,
                    Type = alloc.Type,
                    NonChargeable = alloc.NonChargeable,
                    TicketId = projectID
                };
                Insert(preHoldAlloc);
                historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", preHoldAlloc.AssignedTo, preHoldAlloc.Type, preHoldAlloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", preHoldAlloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", preHoldAlloc.AllocationEndDate)));

                lstUserWithPercetage.Add(
                        new UserWithPercentage()
                        {
                            EndDate = preHoldAlloc.AllocationEndDate ?? DateTime.MinValue,
                            StartDate = preHoldAlloc.AllocationStartDate ?? DateTime.MinValue,
                            Percentage = preHoldAlloc.PctAllocation,
                            UserId = preHoldAlloc.AssignedTo,
                            RoleTitle = roleName,
                            ProjectEstiAllocId = UGITUtility.ObjectToString(preHoldAlloc.ID),
                            RoleId = preHoldAlloc.Type,
                            SoftAllocation = preHoldAlloc.SoftAllocation,
                        });

                ProjectEstimatedAllocation postHoldAlloc = new ProjectEstimatedAllocation()
                {
                    ID = 0,
                    AllocationStartDate = DateTime.Now,
                    AllocationEndDate = alloc.AllocationEndDate,
                    AssignedTo = alloc.AssignedTo,
                    PctAllocation = alloc.PctAllocation,
                    SoftAllocation = true,
                    IsLocked = alloc.IsLocked,
                    Type = alloc.Type,
                    NonChargeable = alloc.NonChargeable,
                    TicketId = projectID
                };
                Insert(postHoldAlloc);
                historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", postHoldAlloc.AssignedTo, postHoldAlloc.Type, postHoldAlloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", postHoldAlloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", postHoldAlloc.AllocationEndDate)));
                lstUserWithPercetage.Add(
                        new UserWithPercentage()
                        {
                            EndDate = postHoldAlloc.AllocationEndDate ?? DateTime.MinValue,
                            StartDate = postHoldAlloc.AllocationStartDate ?? DateTime.MinValue,
                            Percentage = postHoldAlloc.PctAllocation,
                            UserId = postHoldAlloc.AssignedTo,
                            RoleTitle = roleName,
                            ProjectEstiAllocId = UGITUtility.ObjectToString(postHoldAlloc.ID),
                            RoleId = postHoldAlloc.Type,
                            SoftAllocation = postHoldAlloc.SoftAllocation,
                        });

                List<AllocationDeleteModel> allocationDeleteModel = new List<AllocationDeleteModel>();
                allocationDeleteModel.Add(new AllocationDeleteModel { ID=alloc.ID, TicketID = alloc.TicketId, UserID = alloc.AssignedTo });
                Task.Run(() => DeleteEstimatedAllocationUsingSP(allocationDeleteModel));
            }

            foreach(ProjectEstimatedAllocation alloc in allocationsNeedToUpdate)
            {
                string roleName = GetRoleNameBasedOnId(roleManager, alloc);
                alloc.SoftAllocation = true;
                Update(alloc);

                lstUserWithPercetage.Add(
                        new UserWithPercentage()
                        {
                            EndDate = alloc.AllocationEndDate ?? DateTime.MinValue,
                            StartDate = alloc.AllocationStartDate ?? DateTime.MinValue,
                            Percentage = alloc.PctAllocation,
                            UserId = alloc.AssignedTo,
                            RoleTitle = roleName,
                            ProjectEstiAllocId = UGITUtility.ObjectToString(alloc.ID),
                            RoleId = alloc.Type,
                            SoftAllocation = true,
                        });
            }

            if (lstUserWithPercetage.Count > 0)
            {
                ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                {
                    ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(projectID), projectID, lstUserWithPercetage);
                    ResourceAllocationManager.UpdateHistory(_context, historyDesc, projectID);
                    historyDesc.ForEach(o =>
                    {
                        ULog.WriteLog("OnHold Ticket Allocation Update >> " + _context.CurrentUser.Name + o);
                    });

                };
                Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                sThreadStartMethodUpdateCPRProjectAllocation.Start();
            }
        }

        private static string GetRoleNameBasedOnId(GlobalRoleManager roleManager, ProjectEstimatedAllocation alloc)
        {
            string roleName = string.Empty;
            GlobalRole uRole = roleManager.Get(x => x.Id == alloc.Type);
            if (uRole != null)
                roleName = uRole.Name;
            return roleName;
        }

        public int GetAllocatedResourceCountOnProject(string TicketId)
        {
            int resourceCount = 0;
            List<ProjectEstimatedAllocation> projectEstimatedAllocationLst = Load(x => x.TicketId == TicketId);
            if (projectEstimatedAllocationLst != null && projectEstimatedAllocationLst.Count > 0)
                resourceCount = projectEstimatedAllocationLst.Count;

            return resourceCount;
        }

        public async Task DeleteEstimatedAllocationUsingSP(List<AllocationDeleteModel> lstModel)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            List<string> historyDesc = new List<string>();
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> allocations = CRMProjAllocManager.Load(x => x.TicketId == lstModel[0].TicketID);
            ProjectEstimatedAllocation spListItem = allocations.FirstOrDefault(x => x.ID == lstModel[0].ID);
            ResourceAllocationManager resourceAllocManager = new ResourceAllocationManager(context);
            Dictionary<string, object> values = new Dictionary<string, object>();
            List<string> lstIds = new List<string>();
            List<long> lstAllocationIds = new List<long>();
            foreach (var item in lstModel)
            {
                lstIds.Add(item.ID.ToString());
            }
            string roleName = lstModel[0].RoleName;
            string userName = lstModel[0].UserName;
            values.Add("@TenantID", context.TenantID);
            values.Add("@ProjectEstId", string.Join(",", lstIds.ToArray()));
            values.Add("@TicketID", lstModel[0].TicketID);
            try
            {
                List<RResourceAllocation> lstProjectEstimatedAllocations = resourceAllocManager.Load(x => lstIds.Contains(UGITUtility.ObjectToString(x.ProjectEstimatedAllocationId)));
                if(lstProjectEstimatedAllocations != null && lstProjectEstimatedAllocations.Count > 0)
                {
                    lstAllocationIds = lstProjectEstimatedAllocations.Select(x=>x.ID).ToList();
                }
                await Task.Run( () => GetTableDataManager.DeleteData("usp_DeleteAllocations", values, context));
            }
            catch (Exception ex)
            {
                ULog.WriteException("Exception Occurred DeleteEstimatedAllocationUsingSP: "+ ex.ToString());
            }
            historyDesc.Add(string.Format("Allocation removed for user: {0} - {1} {2}% {3}-{4}", string.IsNullOrEmpty(userName) ? "Unassigned" : userName, roleName, spListItem.PctAllocation, String.Format("{0:MM/dd/yyyy}", spListItem.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", spListItem.AllocationEndDate)));
            ResourceAllocationManager.UpdateHistory(_context, historyDesc, lstModel[0].TicketID);
            // Execute both methods concurrently without waiting for each other
            Task updateDeletedAllocationCacheTask = Task.Run(() => resourceAllocManager.UpdateDeletedAllocationCache(lstAllocationIds));
            Task updateProjectGroupsTask = Task.Run(() => UpdateProjectGroups(uHelper.getModuleNameByTicketId(lstModel[0].TicketID), lstModel[0].TicketID));

            await Task.WhenAll(updateDeletedAllocationCacheTask, updateProjectGroupsTask);
        }
    }


    public interface ICRMProjectAllocation : IManagerBase<ProjectEstimatedAllocation>
    {

    }


}
