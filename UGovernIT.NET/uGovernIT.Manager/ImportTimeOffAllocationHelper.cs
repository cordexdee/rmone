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
using System.Threading;
using System.Web;
using uGovernIT.Helpers;

namespace uGovernIT.Manager
{
    public class ImportTimeOffAllocationHelper
    {
        public static void ImportTimeOffAllocations(ApplicationContext context, string listName, string filePath, bool deleteExistingRecords, ref string errorMessage, ref string summary)
        {
            ResourceWorkItemsManager ObjWorkItemsManager = new ResourceWorkItemsManager(context);
            ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(context);
            ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(context);
            RequestTypeManager requestTypeManager = new RequestTypeManager(context);
            ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(context);

            UserProfileManager profileManager = new UserProfileManager(context);

            DateTime dt;
            UserProfile user = null;
            RResourceAllocation rAllocation = null;
            ModuleRequestType moduleRequestType = null;
            //ResourceTimeSheetSignOff resourceTimeSheetSignOff = null;

            StringBuilder sbMissingItems = new StringBuilder();
            StringBuilder sbInvalidDateItems = new StringBuilder();
            //StringBuilder sbAllocationChanges = new StringBuilder();

            string PTORequestType = configVariableManager.GetValue(ConfigConstants.PTORequestType);
            string RequestCategory = string.Empty, RequestType = string.Empty;
            string[] arrPTORequestType = PTORequestType.Split(new string[] { ">" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrPTORequestType.Length > 0)
            {
                RequestCategory = arrPTORequestType[0].Trim();
                RequestType = arrPTORequestType[1].Trim();
            }

            string missingUsers = string.Empty;
            string missingRoles = string.Empty;

            int totalRecords = 0, recordsImported = 0, missingUsersCount = 0;
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

                //List<UserProfile> userProfiles = profileManager.GetFilteredUsers("", null, null, null); //profileManager.GetUsersProfile();
                List<UserProfile> userProfiles = profileManager.GetUsersProfile().Where(x => x.Enabled == true).ToList();

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
                List<TimeOffAllocation> Allocations = new List<TimeOffAllocation>();
                RowCollection rows = worksheet.Rows;
                for (int i = dataRange.TopRowIndex + 1; i <= dataRange.BottomRowIndex; i++)
                {
                    Row rowData = rows[i];

                    facItem = objFieldAliasCollection.Where(x => x.InternalName.EqualsIgnoreCase("TicketID")).FirstOrDefault();
                    string TicketID = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "AssignedToUser").FirstOrDefault();
                    string AssignedToUser = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "EmployeeId").FirstOrDefault();
                    string EmployeeId = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "SignOffStatus").FirstOrDefault();
                    string SignOffStatus = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

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
                    if (UGITUtility.StringToDateTime(AllocationEndDt) < UGITUtility.StringToDateTime(AllocationStartDt))
                    {
                        sbInvalidDateItems.AppendLine(UGITUtility.ObjectToString("RowId: "+ i +" > "+ UGITUtility.StringToDateTime(AllocationStartDt).ToString("MMM dd, yyyy") +" - "+ UGITUtility.StringToDateTime(AllocationEndDt).ToString("MMM dd, yyyy")));
                        continue;
                    }

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "Hours").FirstOrDefault();
                    string Hours = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    facItem = objFieldAliasCollection.Where(x => x.InternalName == "ManagerUser").FirstOrDefault();
                    string ManagerIDUser = uHelper.GetValueByColumn(rowData, facItem, lstColumn);

                    Allocations.Add(new TimeOffAllocation { AssignedToUser = AssignedToUser, TicketID = TicketID, AllocationStartDt = AllocationStartDt, AllocationEndDt = AllocationEndDt, PctAllocation = 100, SignOffStatus = SignOffStatus , ManagerIDUser = ManagerIDUser, Hours = Hours,EmployeeId= EmployeeId });
                }

                totalRecords = Allocations.Count;

                List<TimeOffAllocation> lstMissingUsers = new List<TimeOffAllocation>();
                
                moduleRequestType = requestTypeManager.Load(x => x.Category.EqualsIgnoreCase(RequestCategory) && x.RequestType.EqualsIgnoreCase(RequestType)).FirstOrDefault();
                if (moduleRequestType == null)
                {
                    summary = $"Missing Request Type";
                    return;
                }

                foreach (var item in Allocations)
                {
                    user = null;

                    if (!string.IsNullOrEmpty(item.EmployeeId))
                        user = userProfiles.FirstOrDefault(x => x.EmployeeId.EqualsIgnoreCase(item.EmployeeId));

                    if (user == null)
                        user = userProfiles.FirstOrDefault(x => x.Name.EqualsIgnoreCase(item.AssignedToUser) || x.UserName.EqualsIgnoreCase(item.AssignedToUser));

                    if (user == null)
                    {
                        lstMissingUsers.Add(item);
                        continue;
                    }

                    rAllocation = objAllocationManager.Load(x => x.AllocationStartDate == Convert.ToDateTime(item.AllocationStartDt) && x.AllocationEndDate == Convert.ToDateTime(item.AllocationEndDt) && x.Resource.EqualsIgnoreCase(user.Id)).FirstOrDefault();
                    if (rAllocation == null)
                        rAllocation = new RResourceAllocation();

                    rAllocation.ResourceWorkItems = ObjWorkItemsManager.LoadByWorkItem(user.Id, moduleRequestType.RequestCategory, moduleRequestType.Category, moduleRequestType.RequestType, "", Convert.ToString(item.AllocationStartDt), Convert.ToString(item.AllocationEndDt));
                    if (moduleRequestType != null)
                    {
                        if (rAllocation.ResourceWorkItems != null)
                        {
                            rAllocation.ResourceWorkItems.WorkItemType = moduleRequestType.RequestCategory;
                            rAllocation.ResourceWorkItems.WorkItem = moduleRequestType.Category;
                            rAllocation.ResourceWorkItems.SubWorkItem = moduleRequestType.RequestType;
                        }
                        else
                        {
                            ResourceWorkItems rWorkItem = new ResourceWorkItems();
                            rWorkItem.Resource = user.Id;
                            rWorkItem.WorkItemType = moduleRequestType.RequestCategory;
                            rWorkItem.WorkItem = moduleRequestType.Category;
                            rWorkItem.SubWorkItem = moduleRequestType.RequestType;
                            rWorkItem.StartDate = Convert.ToDateTime(item.AllocationStartDt);
                            rWorkItem.EndDate = Convert.ToDateTime(item.AllocationEndDt);
                            ObjWorkItemsManager.Save(rWorkItem);

                            rAllocation.ResourceWorkItems = rWorkItem;
                            rAllocation.ResourceWorkItems.WorkItemType = rWorkItem.WorkItemType;
                            rAllocation.ResourceWorkItems.WorkItem = rWorkItem.WorkItem;
                            rAllocation.ResourceWorkItems.SubWorkItem = rWorkItem.SubWorkItem;
                        }

                        rAllocation.RoleId = moduleRequestType.RequestType;
                    }
                    
                    //rAllocation.TicketID = item.TicketID;
                    rAllocation.TicketID = moduleRequestType.Category;

                    rAllocation.ResourceWorkItems.StartDate = Convert.ToDateTime(item.AllocationStartDt);
                    rAllocation.ResourceWorkItems.EndDate = Convert.ToDateTime(item.AllocationEndDt);
                    //ObjWorkItemsManager.Insert(rAllocation.ResourceWorkItems);
                    rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                    rAllocation.PctAllocation = item.PctAllocation;
                    rAllocation.PctEstimatedAllocation = item.PctAllocation;

                    rAllocation.AllocationStartDate = rAllocation.ResourceWorkItems.StartDate;
                    rAllocation.AllocationEndDate = rAllocation.ResourceWorkItems.EndDate;
                    rAllocation.Resource = user.Id;

                    string message = objAllocationManager.Save(rAllocation);

                    if (message == string.Empty && rAllocation.ResourceWorkItemLookup > 0)
                    {
                        long workItemID = rAllocation.ResourceWorkItemLookup;

                        //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                        ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(context, workItemID); };
                        Thread sThread = new Thread(threadStartMethod);
                        sThread.IsBackground = true;
                        sThread.Start();
                    }
                    recordsImported++;
                    //resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();
                    //resourceTimeSheetSignOff.Title = user.Name + Constants.Separator7 + Constants.SpaceSeparator + weekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + weekEndDate.ToString("yyyy-MM-dd");
                    //resourceTimeSheetSignOff.StartDate = weekStartDate;
                    //resourceTimeSheetSignOff.Resource = user.Id;
                    //resourceTimeSheetSignOff.EndDate = weekEndDate;
                    //resourceTimeSheetSignOff.History = user.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + item.SignOffStatus;
                    //resourceTimeSheetSignOff.SignOffStatus = Constants.PendingApproval;
                    //resourceTimeSheetSignOff.ModifiedBy = item.ManagerIDUser;
                    //resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);
                }
                
                if (lstMissingUsers?.Count > 0)
                {
                    missingUsers = string.Join(",\n", lstMissingUsers.Select(x => x.AssignedToUser).Except(userProfiles.Select(y => y.Name)).ToList());
                    missingUsersCount = lstMissingUsers.Select(x => x.AssignedToUser).Except(userProfiles.Select(y => y.Name)).Count();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ULog.WriteLog(errorMessage);
            }
            summary = $"<b>Total Records from Excel:</b> {totalRecords}\n<b>Records Processed:</b> {recordsImported}\n<b>Missing Users count: </b>{missingUsersCount}\n<b>Missing Users: </b>\n{missingUsers}" + 
                        $"\n<b>Invalid Date Items:</b>\n{Convert.ToString(sbInvalidDateItems)}\n";
        }         
    }

    public class TimeOffAllocation
    {
        public string TicketID { get; set; }
        public string AssignedToUser { get; set; }
        public string EmployeeId { get; set; }
        public string SignOffStatus { get; set; }
        public string AllocationStartDt { get; set; }
        public string AllocationEndDt { get; set; }
        public string Hours { get; set; }
        public string ManagerIDUser { get; set; }
        public int PctAllocation { get; set; }
    }
}
