using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
using uGovernIT.Manager.RMM.ViewModel;

namespace uGovernIT.Manager
{
    public class ImportProjectAllocationHelper
    {
        public static void ImportProjectAllocations(ApplicationContext context, string listName, string filePath, bool deleteExistingRecords, ref string errorMessage, ref string summary)
        {
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            ResourceAllocationMonthlyManager resourceAllocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            ResourceUsageSummaryMonthWiseManager resourceUsageSummaryMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(context);
            ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);

            UserProfileManager profileManager = new UserProfileManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);

            DateTime dt;

            StringBuilder sbMissingItems = new StringBuilder();
            StringBuilder sbInvalidDateItems = new StringBuilder();
            StringBuilder sbAllocationChanges = new StringBuilder();

            string missingUsers = string.Empty;
            string missingRoles = string.Empty;

            int totalRecords = 0, recordsImported = 0;
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadDocument(filePath);
                Worksheet worksheet = workbook.Worksheets[0];
                CellRange dataRange = worksheet.GetDataRange();
                DataTable data = worksheet.CreateDataTable(dataRange, true);
                if (data == null)
                {
                    errorMessage = "No data found in file!";
                    ULog.WriteLog(errorMessage);
                    return;
                }

                List<string> lstColumn = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
                int lastIndex = workbook.Worksheets[0].Rows.LastUsedIndex;
                List<FieldAliasCollection> templstColumn = FieldAliasCollection.FillFieldAliasCollection();
                List<FieldAliasCollection> objFieldAliasCollection = new List<FieldAliasCollection>();
                
                List<UserProfile> userProfiles = profileManager.GetFilteredUsers("", null, null, null); //profileManager.GetUsersProfile();
                List<GlobalRole> roles = roleManager.Load();

                if (lstColumn == null || lstColumn.Count == 0 || dataRange.RowCount <= 1)
                {
                    errorMessage = "No data found in file!";
                    ULog.WriteLog(errorMessage);
                    return;
                }

                objFieldAliasCollection = templstColumn.Where(r => r.ListName == listName).ToList();

                if (objFieldAliasCollection == null || objFieldAliasCollection.Count == 0)
                {
                    errorMessage = string.Format("Import not configured for list {0}", listName);
                    ULog.WriteLog(errorMessage);
                    return;
                }

                FieldAliasCollection facItem;
                List<ProjectEstimatedAllocation> Allocations = new List<ProjectEstimatedAllocation>();
                RowCollection rows = worksheet.Rows;
                for (int i = dataRange.TopRowIndex + 1; i <= dataRange.BottomRowIndex; i++)
                {
                    Row rowData = rows[i];

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("TicketID")).FirstOrDefault();
                    string TicketID = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "AssignedToUser").FirstOrDefault();
                    string AssignedToUser = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ItemOrder").FirstOrDefault();
                    string ItemOrder = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "AllocationStartDate").FirstOrDefault();
                    string AllocationStartDt = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if(DateTime.TryParse(AllocationStartDt, out dt) == false)
                    {
                        sbInvalidDateItems.AppendLine(TicketID);
                        continue;
                    }

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "AllocationEndDate").FirstOrDefault();
                    string AllocationEndDt = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    if (DateTime.TryParse(AllocationEndDt, out dt) == false)
                    {
                        sbInvalidDateItems.AppendLine(TicketID);
                        continue;
                    }

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Type").FirstOrDefault();
                    string RoleName = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "PctAllocation").FirstOrDefault();
                    string PctAlloc = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    Allocations.Add(new ProjectEstimatedAllocation { ItemOrder = UGITUtility.StringToInt(ItemOrder), AssignedTo = AssignedToUser, TicketId = TicketID, Type = RoleName, AllocationStartDate = Convert.ToDateTime(AllocationStartDt), AllocationEndDate = Convert.ToDateTime(AllocationEndDt), PctAllocation = Convert.ToDouble(PctAlloc), Deleted = false });
                }

                totalRecords = Allocations.Count;

                missingRoles = string.Join(",\n", Allocations.Select(x => x.Type).Except(roles.Select(y => y.Name)).ToList());
                missingUsers = string.Join(",\n", Allocations.Select(x => x.AssignedTo).Except(userProfiles.Select(y => y.Name)).ToList());

                ILookup<string, ProjectEstimatedAllocation> allocationLookup = Allocations.ToLookup(x => x.TicketId);

                List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                List<AllocationListModel> allocationListModels = new List<AllocationListModel>();
                //List<ProjectAllocationModel> projectAllocationModels = new List<ProjectAllocationModel>();
                List<ProjectEstimatedAllocation> projectAllocations = null;
                List<ProjectEstimatedAllocation> projectAllocationsForDeletion = null;
                List<RResourceAllocation> resourceAllocations = null;
                List<ResourceAllocationMonthly> resourceAllocationMonthlies = null;
                List<ResourceUsageSummaryMonthWise> resourceUsageSummaryMonthWises = null;
                List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWises = null;

                UserProfile user = null;
                GlobalRole role = null;
                DataRow ticket = null;

                sbMissingItems.Clear();
                foreach (var ticketId in allocationLookup)
                {
                    ticket = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketId.Key), ticketId.Key);

                    if (ticket == null)
                    {
                        sbMissingItems.AppendLine(ticketId.Key);
                        continue;
                    }

                    //projectAllocationModels.Clear();
                    List<ProjectAllocationModel> projectAllocationModels = new List<ProjectAllocationModel>();
                    foreach (var item in allocationLookup[ticketId.Key].ToList())
                    {
                        ProjectAllocationModel pam = new ProjectAllocationModel();
                        user = userProfiles.FirstOrDefault(x => x.Name.EqualsIgnoreCase(item.AssignedTo));
                        if (user != null)
                        {
                            pam.AssignedTo = user.Id;
                        }
                        else
                        {
                            pam.AssignedTo = Guid.Empty.ToString();
                        }
                        pam.AssignedToName = item.AssignedTo;
                        pam.AllocationStartDate = item.AllocationStartDate ?? DateTime.MinValue;
                        pam.AllocationEndDate = item.AllocationEndDate ?? DateTime.MinValue;
                        pam.PctAllocation = (float)item.PctAllocation;  //Convert.ToSingle(item.PctAllocation);

                        role = roles.FirstOrDefault(x => x.Name.EqualsIgnoreCase(item.Type));
                        if (role != null)
                            pam.Type = role.Id;
                        pam.TypeName = item.Type;
                        pam.ProjectID = ticketId.Key;
                        projectAllocationModels.Add(pam);
                    }

                    if (deleteExistingRecords == false) // update existing Records
                    {
                        projectAllocations = CRMProjAllocManager.Load(x => x.TicketId == ticketId.Key && x.Deleted == false);
                        if (projectAllocations != null && projectAllocations.Count > 0)
                        {
                            int count = 0;
                            List<string> allocs = new List<string>();
                            foreach (var item in projectAllocations)
                            {
                                if (allocs.Contains($"{ticketId.Key};{item.AssignedTo};{item.Type}"))
                                    continue;
                                else
                                    allocs.Add($"{ticketId.Key};{item.AssignedTo};{item.Type}");

                                ProjectAllocationModel allocation = projectAllocationModels.FirstOrDefault(x => x.ProjectID == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type));
                                //ProjectAllocationModel allocation = projectAllocationModels.FirstOrDefault(x => x.ProjectID == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type) && x.AllocationStartDate.Date == item.AllocationStartDate.Value.Date && x.AllocationEndDate.Date == item.AllocationEndDate.Value.Date);
                                count = projectAllocations.Where(x => x.TicketId == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type)).Count();
                                
                                if (allocation != null && count == 1)
                                {
                                    projectAllocationModels.Find(x => x.ProjectID == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type)).ID = item.ID;
                                    //projectAllocationModels.Find(x => x.ProjectID == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type) && x.AllocationStartDate.Date == item.AllocationStartDate.Value.Date && x.AllocationEndDate.Date == item.AllocationEndDate.Value.Date).ID = item.ID;
                                }
                                else 
                                {
                                    ProjectAllocationModel pam = new ProjectAllocationModel();
                                    user = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(item.AssignedTo));
                                    if (user != null)
                                    {
                                        pam.AssignedToName = user.Name;
                                        pam.AssignedTo = item.AssignedTo;
                                    }
                                    else
                                    {
                                        pam.AssignedTo = Guid.Empty.ToString();
                                    }
                                    pam.AllocationStartDate = item.AllocationStartDate ?? DateTime.MinValue;
                                    pam.AllocationEndDate = item.AllocationEndDate ?? DateTime.MinValue;
                                    pam.PctAllocation = (float)item.PctAllocation;  //Convert.ToSingle(item.PctAllocation);

                                    role = roles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(item.Type));
                                    if (role != null)
                                        pam.TypeName = role.Name;
                                    pam.Type = item.Type;
                                    pam.ProjectID = ticketId.Key;
                                    pam.ID = item.ID;

                                    if (count > 1 && allocation != null)
                                    {
                                        projectAllocationsForDeletion = CRMProjAllocManager.Load(x => x.TicketId == ticketId.Key && x.AssignedTo.EqualsIgnoreCase(item.AssignedTo) && x.Type.EqualsIgnoreCase(item.Type) && x.Deleted == false);
                                        if (projectAllocationsForDeletion != null && projectAllocationsForDeletion.Count > 0)
                                        {
                                            sbAllocationChanges.AppendLine($"- {item.TicketId}  {pam.AssignedToName}  {pam.TypeName}:"); // {item.AllocationStartDate} to {item.AllocationEndDate}");
                                            projectAllocationsForDeletion.ForEach(
                                                    x => {
                                                        sbAllocationChanges.AppendLine($"\t\t\t\t{x.PctAllocation}%  {x.AllocationStartDate.Value.ToShortDateString()} to {x.AllocationEndDate.Value.ToShortDateString()}");
                                                     }
                                                );
                                            CRMProjAllocManager.Delete(projectAllocationsForDeletion);
                                        }

                                        resourceAllocations = allocationManager.Load(x => x.TicketID == ticketId.Key && x.Resource.EqualsIgnoreCase(item.AssignedTo) && x.RoleId.EqualsIgnoreCase(item.Type) &&  x.Deleted == false);
                                        if (resourceAllocations != null && resourceAllocations.Count > 0)
                                        {
                                            allocationManager.Delete(resourceAllocations);
                                        }

                                        resourceAllocationMonthlies = resourceAllocationMonthlyManager.Load(x => x.ResourceWorkItem == ticketId.Key && x.Resource.EqualsIgnoreCase(item.AssignedTo) && x.ResourceSubWorkItem.EqualsIgnoreCase(pam.TypeName) &&  x.Deleted == false);
                                        if (resourceAllocationMonthlies != null && resourceAllocationMonthlies.Count > 0)
                                            resourceAllocationMonthlyManager.Delete(resourceAllocationMonthlies);

                                        resourceUsageSummaryMonthWises = resourceUsageSummaryMonthWiseManager.Load(x => x.WorkItem == ticketId.Key && x.Resource.EqualsIgnoreCase(item.AssignedTo) && x.SubWorkItem.EqualsIgnoreCase(pam.TypeName) &&  x.Deleted == false);
                                        if (resourceUsageSummaryMonthWises != null && resourceUsageSummaryMonthWises.Count > 0)
                                            resourceUsageSummaryMonthWiseManager.Delete(resourceUsageSummaryMonthWises);

                                        resourceUsageSummaryWeekWises = resourceUsageSummaryWeekWiseManager.Load(x => x.WorkItem == ticketId.Key && x.Resource.EqualsIgnoreCase(item.AssignedTo) && x.SubWorkItem.EqualsIgnoreCase(pam.TypeName) && x.Deleted == false);
                                        if (resourceUsageSummaryWeekWises != null && resourceUsageSummaryWeekWises.Count > 0)
                                            resourceUsageSummaryWeekWiseManager.Delete(resourceUsageSummaryWeekWises);
                                    }

                                    if (allocation == null)
                                        projectAllocationModels.Add(pam);

                                    if (count > 1 && allocation != null)
                                        sbAllocationChanges.AppendLine($"+ {allocation.ProjectID}  {allocation.AssignedToName}  {allocation.TypeName}  {pam.PctAllocation}%  {allocation.AllocationStartDate.ToShortDateString()} to {allocation.AllocationEndDate.ToShortDateString()}");
                                }
                            }
                        }
                    }
                    else
                    {
                        projectAllocationsForDeletion = CRMProjAllocManager.Load(x => x.TicketId == ticketId.Key && x.Deleted == false);
                        if (projectAllocationsForDeletion != null && projectAllocationsForDeletion.Count > 0)
                            CRMProjAllocManager.Delete(projectAllocationsForDeletion);

                        resourceAllocations = allocationManager.Load(x => x.TicketID == ticketId.Key && x.Deleted == false);
                        if (resourceAllocations != null && resourceAllocations.Count > 0)
                            allocationManager.Delete(resourceAllocations);

                        resourceAllocationMonthlies = resourceAllocationMonthlyManager.Load(x => x.ResourceWorkItem == ticketId.Key && x.Deleted == false);
                        if (resourceAllocationMonthlies != null && resourceAllocationMonthlies.Count > 0)
                            resourceAllocationMonthlyManager.Delete(resourceAllocationMonthlies);

                        resourceUsageSummaryMonthWises = resourceUsageSummaryMonthWiseManager.Load(x => x.WorkItem == ticketId.Key && x.Deleted == false);
                        if (resourceUsageSummaryMonthWises != null && resourceUsageSummaryMonthWises.Count > 0)
                            resourceUsageSummaryMonthWiseManager.Delete(resourceUsageSummaryMonthWises);

                        resourceUsageSummaryWeekWises = resourceUsageSummaryWeekWiseManager.Load(x => x.WorkItem == ticketId.Key && x.Deleted == false);
                        if (resourceUsageSummaryWeekWises != null && resourceUsageSummaryWeekWises.Count > 0)
                            resourceUsageSummaryWeekWiseManager.Delete(resourceUsageSummaryWeekWises);
                    }

                    allocationListModels.Add(new AllocationListModel { ProjectID = ticketId.Key, Allocations = projectAllocationModels });
                }

                foreach (var item in allocationListModels)
                {
                    lstUserWithPercetage.Clear();
                    foreach (var allocation in item.Allocations)
                    {
                        ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                        crmAllocation.AllocationStartDate = allocation.AllocationStartDate;
                        crmAllocation.AllocationEndDate = allocation.AllocationEndDate;

                        int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                        int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                        crmAllocation.AssignedTo = allocation.AssignedTo;
                        crmAllocation.PctAllocation = allocation.PctAllocation;
                        crmAllocation.Type = allocation.Type;
                        crmAllocation.Duration = noOfWeeks;
                        crmAllocation.Title = allocation.Title;
                        crmAllocation.TicketId = item.ProjectID;
                        crmAllocation.ID = allocation.ID;
                        if (crmAllocation.ID > 0)
                        {
                            CRMProjAllocManager.Update(crmAllocation);
                            recordsImported++;
                        }
                        else
                        {
                            CRMProjAllocManager.Insert(crmAllocation);
                            recordsImported++;
                        }

                        lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue, StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue, Percentage = crmAllocation.PctAllocation, UserId = crmAllocation.AssignedTo, RoleTitle = allocation.TypeName, ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID), RoleId = crmAllocation.Type });
                    }
                    ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(item.ProjectID), item.ProjectID, lstUserWithPercetage, null);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ULog.WriteLog(errorMessage);
            }
            summary = $"<b>Total Records from Excel:</b> {totalRecords}\n<b>Records Updated:</b> {recordsImported}\n\n<b>Missing Roles:</b>\n{missingRoles}\n\n<b>Missing Items:</b>\n{Convert.ToString(sbMissingItems)}" + 
                        $"\n\n<b>Invalid Date Items:</b>\n{Convert.ToString(sbInvalidDateItems)}\n<b>Allocation changes:</b>\n{sbAllocationChanges.ToString()}";
        }         
    }
}
