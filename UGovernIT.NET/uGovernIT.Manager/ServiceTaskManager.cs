using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ServiceTaskManager : ManagerBase<UGITTask>, IServiceTaskManager
    {
        public ServiceTaskManager(ApplicationContext context) : base(context)
        {

        }
        public List<UGITTask> LoadByServiceID(string serviceID)
        {
            List<UGITTask> ticketRelations = new List<UGITTask>();
            UGITTaskManager TaskManager = new UGITTaskManager(dbContext);
            ticketRelations = TaskManager.LoadByProjectID(serviceID);
            string modulename= "SVCConfig";
            if (ModuleNames.SVC == uHelper.getModuleNameByTicketId(serviceID))
            {
                modulename = ModuleNames.SVC;
            }
            //this code excludes any template type tasks if any
            if (ticketRelations != null && ticketRelations.Count > 0)
                ticketRelations = ticketRelations.Where(x => x.ModuleNameLookup == modulename).ToList();
            //SPList tickets = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships, spWeb);

            //if (tickets != null)
            //{
            //    try
            //    {
            //        SPQuery rQuery = new SPQuery();
            //        StringBuilder fields = new StringBuilder();
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Description);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Predecessors);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ModuleNameLookup);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketRequestType);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketRequestTypeCategory);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ServiceTitleLookup);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ItemOrder);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.StageWeight);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskEstimatedHours);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ParentTask);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.UGITLevel);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.EnableApproval);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Approver);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.QuestionID);
            //        fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.QuestionProperties);
            //        rQuery.ViewFields = fields.ToString();

            //        if (serviceID > 0)
            //        {
            //            rQuery = Queries.ByServiceId(serviceID);
            //            SPListItemCollection currentTicketDetails = tickets.GetItems(rQuery);
            //            if (currentTicketDetails.Count > 0)
            //            {
            //                foreach (SPListItem currentTicket in currentTicketDetails)
            //                {
            //                    ticketRelations.Add(ServiceTask.LoadTask(currentTicket));
            //                }
            //            }
            //        }
            //        ticketRelations = ticketRelations.OrderBy(x => x.ItemOrder).ToList();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.WriteException(ex);
            //    }
            //}

            return ticketRelations;
        }

        public static DataTable LoadTableByServiceID(int serviceID)
        {
            DataTable result = new DataTable("TicketRelations");
            //List<ServiceTask> tasks = LoadByServiceID(serviceID, spWeb);
            //result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.Description, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.Predecessors, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.PredecessorsID, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            //result.Columns.Add("ModuleNameID", typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.RequestCategory, typeof(string));
            //result.Columns.Add("RequestCategoryID", typeof(int));
            //result.Columns.Add("ServiceName", typeof(string));
            //result.Columns.Add("ServiceNameID", typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.AssignedTo, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.AssignedToID, typeof(string));
            //result.Columns.Add(DatabaseObjects.Columns.StageWeight, typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.TaskEstimatedHours, typeof(double));
            //result.Columns.Add(DatabaseObjects.Columns.ParentTask, typeof(int));
            //result.Columns.Add(DatabaseObjects.Columns.UGITLevel, typeof(int));

            //foreach (ServiceTask task in tasks)
            //{
            //    DataRow row = result.NewRow();
            //    row[DatabaseObjects.Columns.Id] = task.ID;
            //    row[DatabaseObjects.Columns.Title] = task.Title;
            //    row[DatabaseObjects.Columns.Description] = task.Description;

            //    if (task.Predecessors != null)
            //    {
            //        row[DatabaseObjects.Columns.Predecessors] = string.Join("; ", task.Predecessors.Select(x => x.LookupValue).ToArray());
            //        row[DatabaseObjects.Columns.PredecessorsID] = string.Join("; ", task.Predecessors.Select(x => x.LookupId.ToString()).ToArray());
            //    }

            //    if (task.Module != null && task.Module.LookupId > 0)
            //    {
            //        row[DatabaseObjects.Columns.ModuleName] = task.Module.LookupValue;
            //        row["ModuleNameID"] = task.Module.LookupId;
            //    }

            //    if (task.RequestCategory != null && task.RequestCategory.LookupId > 0)
            //    {
            //        row[DatabaseObjects.Columns.RequestCategory] = task.RequestCategory.LookupValue;
            //        row["RequestCategoryID"] = task.RequestCategory.LookupId;
            //    }

            //    if (task.Service != null && task.Service.LookupId > 0)
            //    {
            //        row["ServiceName"] = task.Service.LookupValue;
            //        row["ServiceNameID"] = task.Service.LookupId;
            //    }

            //    row[DatabaseObjects.Columns.AssignedTo] = string.Empty;
            //    row[DatabaseObjects.Columns.AssignedToID] = string.Empty;
            //    if (task.Predecessors != null && task.Predecessors.Count > 0)
            //    {
            //        row[DatabaseObjects.Columns.AssignedTo] = string.Join("; ", task.AssignedTo.Select(x => x.LookupValue).ToArray());
            //        row[DatabaseObjects.Columns.AssignedToID] = string.Join("; ", task.AssignedTo.Select(x => x.LookupId.ToString()).ToArray());
            //    }

            //    row[DatabaseObjects.Columns.StageWeight] = task.Weight;
            //    row[DatabaseObjects.Columns.TaskEstimatedHours] = task.EstimatedHours;

            //    row[DatabaseObjects.Columns.ParentTask] = task.ParentTask;
            //    row[DatabaseObjects.Columns.UGITLevel] = task.TaskLevel;

            //    row[DatabaseObjects.Columns.UGITSubTaskType] = task.UGITSubTaskType;
            //    result.Rows.Add(row);
            //}
            return result;
        }

        public static UGITTask LoadByID(int ID)
        {
            UGITTask task = new UGITTask();
            //SPList tickets = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships);
            //if (ID > 0)
            //{
            //    SPListItem item = SPListHelper.GetSPListItem(tickets, ID);
            //    task = ServiceTask.LoadTask(item);
            //}
            return task;
        }

        private static UGITTask LoadTask(DataRow rItem)
        {
            UGITTask task = new UGITTask();
            try
            {
                //task.ID = rItem.ID;
                //task.Title = Convert.ToString(rItem[DatabaseObjects.Columns.Title]);
                //task.Description = Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.Description));
                //task.Created = (DateTime)rItem[DatabaseObjects.Columns.Created];
                //task.Modified = (DateTime)rItem[DatabaseObjects.Columns.Modified];
                //task.ParentTask = uHelper.StringToInt(rItem[DatabaseObjects.Columns.ParentTask]);
                //task.TaskLevel = uHelper.StringToInt(rItem[DatabaseObjects.Columns.UGITLevel]);
                //task.UGITSubTaskType = Convert.ToString(rItem[DatabaseObjects.Columns.UGITSubTaskType]);
                //task.AutoCreateUser = Convert.ToBoolean(rItem[DatabaseObjects.Columns.AutoCreateUser]);
                //task.AutoFillRequestor = Convert.ToBoolean(rItem[DatabaseObjects.Columns.AutoFillRequestor]);
                //task.QuestionID = Convert.ToString(rItem[DatabaseObjects.Columns.QuestionID]);
                //task.QuestionProperties = Convert.ToString(rItem[DatabaseObjects.Columns.QuestionProperties]);

                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.Predecessors))
                //{
                //    task.Predecessors = (SPFieldLookupValueCollection)rItem[DatabaseObjects.Columns.Predecessors];
                //}

                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.TicketRequestTypeLookup))
                //{
                //    SPFieldLookupValue typeLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.TicketRequestTypeLookup)));
                //    if (typeLookup.LookupId > 0)
                //    {
                //        task.RequestCategory = typeLookup;
                //    }
                //}

                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.ModuleNameLookup))
                //{
                //    SPFieldLookupValue moduleLookup = new SPFieldLookupValue(Convert.ToString(rItem[DatabaseObjects.Columns.ModuleNameLookup]));
                //    if (moduleLookup.LookupId > 0)
                //    {
                //        task.Module = moduleLookup;
                //    }
                //}

                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.ServiceTitleLookup))
                //{
                //    SPFieldLookupValue serviceLookup = new SPFieldLookupValue(Convert.ToString(rItem[DatabaseObjects.Columns.ServiceTitleLookup]));
                //    if (serviceLookup.LookupId > 0)
                //    {
                //        task.Service = serviceLookup;
                //    }
                //}

                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.AssignedTo))
                //{
                //    SPFieldUserValueCollection multiLookups = new SPFieldUserValueCollection(rItem.Web, Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.AssignedTo)));
                //    if (multiLookups != null && multiLookups.Count > 0)
                //    {
                //        task.AssignedTo = multiLookups;
                //    }
                //}

                //int itemOrder = 0;
                //int.TryParse(Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.ItemOrder)), out itemOrder);
                //task.ItemOrder = itemOrder;

                //int weight = 0;
                //int.TryParse(Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.StageWeight)), out weight);
                //task.Weight = weight;

                //double estimatedHours = 0;
                //double.TryParse(Convert.ToString(uHelper.GetSPItemValue(rItem, DatabaseObjects.Columns.TaskEstimatedHours)), out estimatedHours);
                //task.EstimatedHours = estimatedHours;

                //task.ModifiedByUser = string.Empty;
                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.Editor))
                //{
                //    SPFieldUserValue lookup = new SPFieldUserValue(rItem.Web, Convert.ToString(rItem[DatabaseObjects.Columns.Editor]));
                //    if (lookup != null)
                //    {
                //        task.ModifiedByUser = lookup.LookupValue;
                //    }
                //}

                //task.Author = string.Empty;
                //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.Author))
                //{
                //    SPFieldUserValue lookup = new SPFieldUserValue(rItem.Web, Convert.ToString(rItem[DatabaseObjects.Columns.Author]));
                //    if (lookup != null)
                //    {
                //        task.Author = lookup.LookupValue;
                //    }
                //}

                //task.EnableApproval = Convert.ToBoolean(rItem[DatabaseObjects.Columns.EnableApproval]);
                //task.Approver = uHelper.GetUserCollectionFromValue(rItem.Web, Convert.ToString(rItem[DatabaseObjects.Columns.Approver]));
            }
            catch { }
            return task;
        }

        public bool Save()
        {
            //SPList ticketRelationships = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships);
            //SPListItem relationship = null;
            //if (ID > 0)
            //{
            //    relationship = SPListHelper.GetSPListItem(ticketRelationships, ID);
            //}
            //else
            //{
            //    relationship = ticketRelationships.Items.Add();
            //}
            //relationship[DatabaseObjects.Columns.QuestionID] = QuestionID;

            //relationship[DatabaseObjects.Columns.QuestionProperties] = QuestionProperties;

            //relationship[DatabaseObjects.Columns.UGITSubTaskType] = UGITSubTaskType;

            //relationship[DatabaseObjects.Columns.AutoCreateUser] = AutoCreateUser;

            //relationship[DatabaseObjects.Columns.AutoFillRequestor] = AutoFillRequestor;

            //relationship[DatabaseObjects.Columns.Title] = uHelper.StripHTML(Title);
            //relationship[DatabaseObjects.Columns.Predecessors] = null;
            //if (Predecessors != null && Predecessors.Count > 0)
            //{
            //    relationship[DatabaseObjects.Columns.Predecessors] = Predecessors;
            //}

            //relationship[DatabaseObjects.Columns.ModuleNameLookup] = null;
            //if (Module != null)
            //{
            //    relationship[DatabaseObjects.Columns.ModuleNameLookup] = Module;
            //}

            //relationship[DatabaseObjects.Columns.ServiceTitleLookup] = null;
            //if (Service != null)
            //{
            //    relationship[DatabaseObjects.Columns.ServiceTitleLookup] = Service;
            //}

            //relationship[DatabaseObjects.Columns.TicketRequestTypeLookup] = null;
            //if (RequestCategory != null)
            //{
            //    relationship[DatabaseObjects.Columns.TicketRequestTypeLookup] = RequestCategory;
            //}

            //relationship[DatabaseObjects.Columns.AssignedTo] = null;
            //if (AssignedTo != null && AssignedTo.Count > 0)
            //{
            //    relationship[DatabaseObjects.Columns.AssignedTo] = AssignedTo;
            //}

            //relationship[DatabaseObjects.Columns.Description] = this.Description;
            //relationship[DatabaseObjects.Columns.StageWeight] = this.Weight;
            //relationship[DatabaseObjects.Columns.StageWeight] = this.Weight;
            //relationship[DatabaseObjects.Columns.TaskEstimatedHours] = this.EstimatedHours;
            //relationship[DatabaseObjects.Columns.ParentTask] = this.ParentTask;
            //relationship[DatabaseObjects.Columns.UGITLevel] = this.TaskLevel;

            //relationship[DatabaseObjects.Columns.EnableApproval] = this.EnableApproval;
            //relationship[DatabaseObjects.Columns.Approver] = this.Approver;

            //relationship.UpdateOverwriteVersion();
            //this.ID = relationship.ID;
            //this.Created = (DateTime)relationship[DatabaseObjects.Columns.Created];
            //this.Modified = (DateTime)relationship[DatabaseObjects.Columns.Modified];

            return true;
        }

        public bool Delete()
        {
            //SPList ticketRelationships = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships);
            //SPListItem relationship = SPListHelper.GetSPListItem(ticketRelationships, ID);
            //relationship.Delete();
            return true;
        }

       
        public void SaveOrder(List<UGITTask> tasks)
        {
            //if (tasks.Count <= 0)
            //{
            //    return;
            //}

            //tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            //int ctr = 0;
            //foreach (ServiceTask sT in tasks)
            //{
            //    sT.ItemOrder = ++ctr;
            //}

            //SPList serviceTicketList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships);

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemOrder\">{3}</SetVar>" +
            // "</Method>";

            //StringBuilder query = new StringBuilder();
            //foreach (ServiceTask sTask in tasks)
            //{
            //    query.AppendFormat(updateMethodFormat, sTask.ID, serviceTicketList.ID, sTask.ID, sTask.ItemOrder);
            //}
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }

        public void SaveLevel(List<UGITTask> tasks)
        {
            //if (tasks.Count <= 0)
            //{
            //    return;
            //}

            //SPList serviceTicketList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketRelationships);

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#UGITLevel\">{3}</SetVar>" +
            // "</Method>";

            //StringBuilder query = new StringBuilder();
            //foreach (ServiceTask ctr in tasks)
            //{
            //    query.AppendFormat(updateMethodFormat, ctr.ID, serviceTicketList.ID, ctr.ID, ctr.TaskLevel);
            //}
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }

        public static void ReduceLevel(List<UGITTask> tasks, long tskTaskID)
        {
            tasks.Where(x => x.ParentTaskID == tskTaskID).ToList().ForEach(tsk =>
            {
                if (tsk.Level > 0)
                    tsk.Level--;
                else
                    tsk.Level = 0;
                List<UGITTask> coCs = tasks.Where(x => x.ParentTaskID == tsk.ID).ToList();
                if (coCs.Count > 0)
                {
                    coCs.ForEach(coC =>
                    {
                        ReduceLevel(tasks, coC.ID);
                    });
                }
            });
        }

        public static void IncreaseLevel(List<UGITTask> tasks, long tskTaskID)
        {
            tasks.Where(x => x.ParentTaskID == tskTaskID).ToList().ForEach(tsk =>
            {
                tsk.Level += 1;
                IncreaseLevel(tasks, tsk.ID);

            });
        }

        public void AutoCreateAccountTask(Services service, bool isAutoCreateAccount, string questionMappingID)
        {
            if (service == null)
                return;

            //ServiceTask task = service.Tasks.FirstOrDefault(x => x.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower());
            //if (task == null)
            //{
            //    task = new ServiceTask();
            //    task.Title = "Create AD Account";
            //    task.Description = "Auto Create AD Account";
            //    task.ItemOrder = service.Tasks.Count + 1;
            //    task.ParentTask = 0;
            //    task.Weight = 1;
            //    double estimatedHours = 0;
            //    task.EstimatedHours = estimatedHours;
            //    task.UGITSubTaskType = ServiceSubTaskType.AccountTask;
            //    task.AutoCreateUser = isAutoCreateAccount;
            //    task.RequestCategory = null;
            //    task.Service = new SPFieldLookupValue();
            //    task.Service.LookupId = service.ID;
            //    service.Tasks.Add(task);
            //    task.Save();
            //    if (string.IsNullOrEmpty(questionMappingID))
            //        return;

            //    List<string> questions = uHelper.ConvertStringToList(questionMappingID, Constants.Separator6);
            //    ServiceQuestionMapping questionMap = null;
            //    List<ServiceQuestion> questionsObj = service.Questions.Where(x => questions.Contains(x.ID.ToString())).ToList();

            //    if (questionsObj == null || questionsObj.Count <= 0)
            //        return;

            //    string userName = string.Join(" ", questionsObj.Select(x => string.Format("[${0}$]", x.TokenName)));

            //    questionMap = new ServiceQuestionMapping();
            //    questionMap.ServiceID = service.ID;
            //    questionMap.ServiceTaskID = task.ID;
            //    service.QuestionsMapping.Add(questionMap);
            //    questionMap.ColumnName = DatabaseObjects.Columns.Title;
            //    questionMap.ColumnValue = string.Format("{1} {0}", userName, task.Title);
            //    questionMap.PickValueFrom = string.Empty;
            //    questionMap.ServiceQuestionID = 0;
            //    questionMap.Save();

            //    questionMap = new ServiceQuestionMapping();
            //    questionMap.ServiceID = service.ID;
            //    questionMap.ServiceTaskID = task.ID;
            //    service.QuestionsMapping.Add(questionMap);
            //    questionMap.ColumnName = DatabaseObjects.Columns.UGITNewUserName;
            //    questionMap.ColumnValue = userName;
            //    questionMap.PickValueFrom = string.Empty;
            //    questionMap.ServiceQuestionID = 0;
            //    questionMap.Save();
            //}
            //else
            //{
            //    task.AutoCreateUser = isAutoCreateAccount;
            //    task.Save();
            //}

        }
    }
    interface IServiceTaskManager : IManagerBase<UGITTask>
    {

    }
}
