
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ServiceRequestBL
    {
        Services service;
        ApplicationContext _context = null;
        ModuleViewManager ModuleManager = null;
        TicketManager ObjTicketManager = null;
        public ServiceRequestBL(ApplicationContext context)
        {
            _context = context;
            ModuleManager = new ModuleViewManager(_context);
            ObjTicketManager = new TicketManager(_context);
        }

        // changes made for New UI Admin Userinfo login and registartion
        public ServiceRequestBL(ApplicationContext context, string requestPage = "")
        {
            if (requestPage == "SelfRegistration")
            {
                _context = ApplicationContext.Create();
            }
            else
                _context = context;

            ModuleManager = new ModuleViewManager(_context);
            ObjTicketManager = new TicketManager(_context);

        }

        public ServiceInput ServiceInput { get; set; }

        /// <summary>
        /// This method calls other functions to create service request, tasks, tickets, skip logic.
        /// </summary>
        /// <param name="ServiceRequestDTO"></param>
        /// <returns></returns>
        public ServiceResponseTreeNodeParent CreateService(ServiceRequestDTO ServiceRequestDTO, string requestPage = "")
        {

            ServicesManager objServicesManager = new ServicesManager(_context, requestPage); //changed For New Ui admin Userinfo login & submit 
            ServiceResponseTreeNodeParent serviceResponseTreeNodeParent = new ServiceResponseTreeNodeParent();
            if (ServiceRequestDTO != null && ServiceRequestDTO.ServiceId > 0)
            {
                service = objServicesManager.LoadByServiceID(ServiceRequestDTO.ServiceId);
            }

            //If service null then returns right away
            if (service == null)
            {
                return serviceResponseTreeNodeParent;
            }

            List<ServiceQuestionMapping> qMappingList = service.QuestionsMapping;
            List<ServiceQuestion> questionList = service.Questions;
            Dictionary<string, object> sTicketData = new Dictionary<string, object>();
            List<UGITTask> serviceTaskList = service.Tasks;

            //Append variable value with service request values
            if (service.QMapVariables != null && service.QMapVariables.Count > 0)
            {
                AppendVariableData(service, ref ServiceRequestDTO);
            }

            string webURL = string.Empty;
            serviceTaskList = GetTasksNeedToCreate(serviceTaskList, service, ServiceRequestDTO);
            Dictionary<long, TaskTempInfo> taskTickets = new Dictionary<long, TaskTempInfo>();
            List<UGITTask> onlyTasks = serviceTaskList.Where(x => string.Equals(x.ModuleNameLookup, "SVCConfig", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(x.RelatedModule)).ToList();

            //changed For New Ui admin Userinfo login & submit 
            UGITTaskManager taskManager = new UGITTaskManager(_context, requestPage);
            DataTable serviceTicketRelationshipsList = taskManager.GetDataTable();
            foreach (UGITTask onlyTask in onlyTasks)
            {
                List<ServiceQuestionMapping> questMappingList = qMappingList.Where(x => x.ServiceTaskID == onlyTask.ID).ToList();
                TaskTempInfo taskTempInfo = new TaskTempInfo();
                foreach (ServiceQuestionMapping questMap in questMappingList)
                {
                    taskTempInfo.TaskID = onlyTask.ID;
                    if (questMap.ColumnName == DatabaseObjects.Columns.Title)
                    {
                        taskTempInfo.Title = Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO));
                    }
                    else if (questMap.ColumnName == DatabaseObjects.Columns.AssignedTo)
                    {
                        taskTempInfo.AssignTo = Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO));
                    }
                    else if (questMap.ColumnName == DatabaseObjects.Columns.Description)
                    {
                        taskTempInfo.Desc = Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO));
                    }
                    else if (questMap.ColumnName == DatabaseObjects.Columns.DueDate)
                    {
                        DateTime datetime = DateTime.MinValue;
                        DateTime.TryParse(Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO)), out datetime);
                        if (datetime != DateTime.MinValue)
                        {
                            taskTempInfo.DueDate = datetime;
                        }
                    }
                    else if (questMap.ColumnName == DatabaseObjects.Columns.UGITNewUserName)
                    {
                        taskTempInfo.UGITNewUserName = Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO));
                    }
                    else if (questMap.ColumnName == DatabaseObjects.Columns.Approver)
                    {
                        string approvers = Convert.ToString(generateColumnValue(0, "Task", questMap, serviceTicketRelationshipsList.Columns[questMap.ColumnName], service, ServiceRequestDTO));
                        taskTempInfo.Approver = approvers;
                    }

                }
                taskTempInfo.EstimatedHours = onlyTask.EstimatedHours;
                taskTempInfo.StageStep = onlyTask.StageStep;
                //if (onlyTask.Behaviour == "Task")  commented by mudassir 2 march 2020
                taskTickets.Add(onlyTask.ID, taskTempInfo);
            }

            if (service.Questions != null && service.Questions.Count() > 0)
            {
                int id = -1;
                List<ServiceQuestion> serviceQuestionsList = service.Questions.Where(c => c.QuestionType.ToLower() == "applicationaccessrequest").ToList();

                foreach (ServiceQuestion serviceQues in serviceQuestionsList)
                {

                    QuestionsDTO questionDTO = ServiceRequestDTO.Questions.Where(c => c.Token == serviceQues.TokenName).FirstOrDefault();
                    KeyValuePair<string, string> newuser = serviceQues.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.NewUSer);
                    KeyValuePair<string, string> applicationApprover = serviceQues.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == Constants.ApplicationApprover.ToLower());
                    KeyValuePair<string, string> dueDateFrom = serviceQues.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "duedatefrom");
                    KeyValuePair<string, string> predecessors = serviceQues.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "predecessors");
                    UGITTask createAccountTask = serviceTaskList.FirstOrDefault(x => x.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask);
                    //continue from loop if no account task exist and new user is selected in application question
                    if (createAccountTask == null && newuser.Key != null && newuser.Key != string.Empty)
                        continue;
                    if (questionDTO != null && !string.IsNullOrEmpty(questionDTO.Value))
                    {

                        DataTable applicationList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                        List<ServiceMatrixData> lstServiceMatrixData = new List<ServiceMatrixData>();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(questionDTO.Value.Trim());
                        lstServiceMatrixData = (List<ServiceMatrixData>)uHelper.DeSerializeAnObject(doc, lstServiceMatrixData);
                        foreach (ServiceMatrixData serviceMatrixData in lstServiceMatrixData)
                        {
                            if (CheckIsRelationChanged(serviceMatrixData))
                            {
                                DataRow[] splistCollection = applicationList.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, serviceMatrixData.ID));
                                TaskTempInfo task = new TaskTempInfo();
                                UGITTask serviceTask = new UGITTask();

                                serviceTask.EstimatedHours = task.EstimatedHours = 0;
                                List<UserProfile> userValue = _context.UserManager.GetUserInfosById(Convert.ToString(splistCollection[0][DatabaseObjects.Columns.AccessAdmin])); // new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(splistCollection[0][DatabaseObjects.Columns.AccessAdmin]));
                                List<string> userValueX = new List<string>();
                                foreach (UserProfile usr in userValue)
                                {
                                    userValueX.Add(usr.Id);
                                }
                                serviceTask.AssignedTo = string.Join(Constants.Separator6, userValueX);
                                task.AssignTo = string.Join(Constants.Separator6, userValueX); ;
                                serviceTask.ItemOrder = serviceTaskList.Count + 1;
                                serviceTask.Description = task.Desc = serviceMatrixData.Note;
                                string RoleAssignee = string.Empty;
                                UserProfile arrRoleAssignee = _context.UserManager.GetUserInfoById(Convert.ToString(lstServiceMatrixData[0].RoleAssignee));  // new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(lstServiceMatrixData[0].RoleAssignee));
                                if (arrRoleAssignee != null && string.IsNullOrEmpty(arrRoleAssignee.Id))
                                {
                                    RoleAssignee = arrRoleAssignee.UserName;
                                    serviceMatrixData.RoleAssignee = Convert.ToString(arrRoleAssignee.Id);
                                }
                                if (!string.IsNullOrEmpty(lstServiceMatrixData[0].AccessRequestMode) && lstServiceMatrixData[0].AccessRequestMode.ToLower() != "add")
                                    serviceTask.Title = task.Title = string.Format("Remove {0} access {1}",
                                                                                    Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Title]),
                                                                                    !string.IsNullOrWhiteSpace(RoleAssignee) ? "for " + RoleAssignee : string.Empty);
                                else
                                    serviceTask.Title = task.Title = string.Format("Update {0} access {1}",
                                                                                    Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Title]),
                                                                                    !string.IsNullOrWhiteSpace(RoleAssignee) ? "for " + RoleAssignee : string.Empty);
                                XmlDocument serviceMatrixDoc = uHelper.SerializeObject(serviceMatrixData);
                                task.ServiceApplicationAccessXml = HttpContext.Current.Server.HtmlEncode(serviceMatrixDoc.OuterXml);
                                serviceTask.ID = task.TaskID = id;
                                serviceTask.SubTaskType = "applicationaccessrequest";

                                if (newuser.Key != null)
                                {
                                    //serviceTask.PickUserFrom = newuser.Key;
                                }

                                serviceTask.ApprovalType = Convert.ToString(splistCollection[0][DatabaseObjects.Columns.ApprovalType]);
                                if (serviceTask.ApprovalType.ToLower() != string.Empty && serviceTask.ApprovalType.ToLower() != "none")
                                {
                                    serviceTask.EnableApproval = true;
                                    List<UserProfile> users = _context.UserManager.GetUserInfosById(Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Approver])); // new List<UserProfile>(SPContext.Current.Web, Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Approver]));
                                    if (users != null && users.Count > 0)
                                    {
                                        serviceTask.ApprovalStatus = Constants.Pending;
                                        //in case of any one, pick first user from approval
                                        if (serviceTask.ApprovalType.ToLower() == ApprovalType.AnyOne)
                                        {
                                            serviceTask.Approver = users[0].Id; // new SPFieldUserValueCollection() { users[0] };
                                        }
                                        else
                                        {
                                            serviceTask.Approver = string.Join<UserProfile>(Constants.Separator6, users);
                                        }
                                        //serviceTask.TaskActionUser = String.Join(Constants.Separator, serviceTask.Approver.Select(x => x.LookupId));
                                    }
                                    else
                                    {
                                        serviceTask.ApprovalType = "none";
                                    }
                                }
                                //serviceTask.UseADAuthentication = UGITUtility.StringToBoolean(splistCollection[0][DatabaseObjects.Columns.UsesADAuthentication]);
                                if (dueDateFrom.Key != null)
                                {
                                    DateTime dueDate = DateTime.Now;
                                    int noOfDays = UGITUtility.StringToInt(dueDateFrom.Value.Split(':')[1]);
                                    string dueDateFm = dueDateFrom.Value.Split(':')[0];
                                    if (dueDateFm.ToLower() != "<today>")
                                    {
                                        ServiceQuestion dueDateQues = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(dueDateFm));
                                        if (dueDateQues != null)
                                        {
                                            QuestionsDTO dueDateDTO = ServiceRequestDTO.Questions.FirstOrDefault(c => c.Token == dueDateQues.TokenName);
                                            if (!string.IsNullOrEmpty(dueDateDTO.Value))
                                            {
                                                DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context, Convert.ToDateTime(dueDateDTO.Value), noOfDays + 1);
                                                if (dates.Length > 1)
                                                    dueDate = dates[1];
                                            }

                                        }
                                        else
                                            ULog.WriteLog("ERROR: Due date note found - " + dueDateFm);
                                    }
                                    else
                                    {
                                        DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context, dueDate, noOfDays + 1);
                                        if (dates.Length > 1)
                                            dueDate = dates[1];
                                    }


                                    task.DueDate = dueDate;
                                }
                                else
                                    task.DueDate = DateTime.MinValue;

                                if (predecessors.Key != null)
                                {
                                    serviceTask.Predecessors = predecessors.Value;
                                }

                                serviceTask.SLADisabled = false;
                                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, splistCollection.CopyToDataTable()))
                                    serviceTask.SLADisabled = UGITUtility.StringToBoolean(splistCollection[0][DatabaseObjects.Columns.SLADisabled]);
                                serviceTask.Weight = 1;
                                taskTickets.Add(id, task);
                                serviceTask.ItemOrder = serviceTaskList.Count;
                                serviceTaskList.Add(serviceTask);
                                id--;
                            }
                        }
                    }
                }

                List<ServiceQuestion> serviceQuestionsRemoveAccessList = service.Questions.Where(c => c.QuestionType.ToLower() == "removeuseraccess").ToList();
                foreach (ServiceQuestion serviceQuesRemoveAccess in serviceQuestionsRemoveAccessList)
                {
                    QuestionsDTO removeAccessQuestionDTO = ServiceRequestDTO.Questions.Where(c => c.Token == serviceQuesRemoveAccess.TokenName).FirstOrDefault();
                    if (removeAccessQuestionDTO != null && !string.IsNullOrEmpty(removeAccessQuestionDTO.Value))
                    {
                        //DataTable applicationList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications);  // SPListHelper.GetSPList(DatabaseObjects.Lists.Applications);
                        //RemoveAccessList removeAccessList = new RemoveAccessList();
                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(removeAccessQuestionDTO.Value.Trim());
                        //removeAccessList = (RemoveAccessList)uHelper.DeSerializeAnObject(doc, removeAccessList);

                        // List<ModuleRoleRelation> lstModuleRoleRelation = removeAccessList.ModuleRoleRelationList.GroupBy(c => c.ApplicationId).Select(group => group.First()).ToList();
                        // foreach (ModuleRoleRelation moduleRoleRelation in lstModuleRoleRelation)
                        // {
                        // SPQuery query = new SPQuery();
                        // query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, moduleRoleRelation.ApplicationId);
                        // SPListItemCollection splistCollection = applicationList.GetItems(query);
                        // TaskTempInfo task = new TaskTempInfo();
                        // ServiceTask serviceTask = new ServiceTask();
                        // serviceTask.EstimatedHours = task.EstimatedHours = 0;
                        // SPFieldUserValueCollection userValue = (SPFieldUserValueCollection)splistCollection[0][DatabaseObjects.Columns.AccessAdmin];
                        // serviceTask.AssignedTo = userValue;
                        // task.AssignTo = userValue.ToString();
                        // serviceTask.Description = task.Desc = string.Format("Remove all access for {0}", Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Title]));
                        // string RoleAssignee = string.Empty;
                        // SPFieldUserValue arrRoleAssignee = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(removeAccessList.UserId));
                        // if (arrRoleAssignee != null && arrRoleAssignee.User != null)
                        // RoleAssignee = arrRoleAssignee.User.Name;
                        //serviceTask.Title = task.Title = string.Format("Remove {0} access for {1}", Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Title]), RoleAssignee);
                        //RemoveAccessList lstRemoveAccess = new RemoveAccessList();
                        //List<ModuleRoleRelation> ModuleRoleRelationList = removeAccessList.ModuleRoleRelationList.Where(c => c.ApplicationId == moduleRoleRelation.ApplicationId).Select(c => c).ToList();
                        // lstRemoveAccess.ModuleRoleRelationList = ModuleRoleRelationList;
                        //lstRemoveAccess.UserId = removeAccessList.UserId;
                        //lstRemoveAccess.SelectionType = removeAccessList.SelectionType;

                        //if (removeAccessList.ServiceMatrixDataList != null && removeAccessList.ServiceMatrixDataList.Count > 0)
                        //{
                        //    ServiceMatrixData serviceMatrixData = removeAccessList.ServiceMatrixDataList.Where(c => c.ID == moduleRoleRelation.ApplicationId).Select(c => c).FirstOrDefault();
                        //    // serviceTask.Description = task.Desc =serviceMatrixData.Note;
                        //    List<ServiceMatrixData> lstServiceMatrix = new List<ServiceMatrixData>();
                        //    lstServiceMatrix.Add(serviceMatrixData);
                        //    lstRemoveAccess.ServiceMatrixDataList = lstServiceMatrix;
                        //}

                        //XmlDocument moduleRoleRelationDoc = uHelper.SerializeObject(lstRemoveAccess);
                        //task.ServiceApplicationAccessXml = HttpContext.Current.Server.HtmlEncode(moduleRoleRelationDoc.OuterXml);
                        //serviceTask.ID = task.TaskID = id;
                        //task.DueDate = DateTime.MinValue;
                        //serviceTask.Weight = 1;
                        //taskTickets.Add(id, task);
                        //serviceTask.ItemOrder = serviceTaskList.Count;
                        //serviceTaskList.Add(serviceTask);
                        //id--;
                        // }
                    }
                }
            }

            if (service.CreateParentServiceRequest)
            {
                DataRow sModuleItem = CreateServiceRequest(qMappingList, service, ServiceRequestDTO);
                if (service.AllowServiceTasksInBackground)
                {
                    if (requestPage == "SelfRegistration")
                    {
                        serviceResponseTreeNodeParent = CreateTasksInBackground(ServiceRequestDTO, taskTickets, qMappingList, serviceTaskList, sModuleItem, requestPage);
                    }
                    else
                    {
                        serviceResponseTreeNodeParent = CreateTasksInBackground(ServiceRequestDTO, taskTickets, qMappingList, serviceTaskList, sModuleItem);

                    }
                }

                else
                {
                    //changed For New Ui admin Userinfo login & submit 

                    if (requestPage == "SelfRegistration")
                    {

                        serviceResponseTreeNodeParent = CreateTasks(ServiceRequestDTO, taskTickets, qMappingList, serviceTaskList, sModuleItem, _context, requestPage);
                    }
                    else
                    {
                        serviceResponseTreeNodeParent = CreateTasks(ServiceRequestDTO, taskTickets, qMappingList, serviceTaskList, sModuleItem, _context);
                    }
                }
            }
            else
            {
                taskTickets = CreateRelatedTickets(serviceTaskList, qMappingList, service, ServiceRequestDTO, _context);
                if (taskTickets.Count == 0)//if no svc and no tickets to be created return null
                    return null;
                serviceResponseTreeNodeParent = GenerateTaskSummary(taskTickets);
            }

            return serviceResponseTreeNodeParent;
        }

        private ServiceResponseTreeNodeParent CreateTasksInBackground(ServiceRequestDTO ServiceRequestDTO, Dictionary<long, TaskTempInfo> taskTickets, List<ServiceQuestionMapping> qMappingList, List<UGITTask> serviceTaskList, DataRow sModuleItem, string requestPage = null)
        {
            ServiceResponseTreeNodeParent serviceResponseTreeNodeParent = new ServiceResponseTreeNodeParent();
            serviceResponseTreeNodeParent.TicketId = Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]);
            serviceResponseTreeNodeParent.ID = Convert.ToInt32(sModuleItem[DatabaseObjects.Columns.Id]);
            serviceResponseTreeNodeParent.MName = "SVC";
            serviceResponseTreeNodeParent.Text = Convert.ToString(sModuleItem[DatabaseObjects.Columns.Title]);
            ThreadStart starter = delegate { CreateTasks(ServiceRequestDTO, taskTickets, qMappingList, serviceTaskList, sModuleItem, _context, requestPage); };
            Thread thread = new Thread(starter);
            thread.IsBackground = true;
            thread.Start();

            return serviceResponseTreeNodeParent;
        }

        private ServiceResponseTreeNodeParent CreateTasks(ServiceRequestDTO ServiceRequestDTO, Dictionary<long, TaskTempInfo> taskTickets, List<ServiceQuestionMapping> qMappingList, List<UGITTask> serviceTaskList, DataRow sModuleItem, ApplicationContext context, string requestPage = "")
        {
            ServiceResponseTreeNodeParent serviceResponseTreeNodeParent = new ServiceResponseTreeNodeParent();

            Dictionary<long, TaskTempInfo> NewtaskTickets = CreateRelatedTickets(serviceTaskList, qMappingList, service, ServiceRequestDTO, context);

            List<string> lstTickets = new List<string>();

            foreach (KeyValuePair<long, TaskTempInfo> entry in NewtaskTickets)
            {
                taskTickets.Add(entry.Key, entry.Value);
                lstTickets.Add(entry.Value.TicketID);
            }

            //Having Mapped data for all services and task
            serviceResponseTreeNodeParent = MoveTicketToServiceRelationship(serviceTaskList, sModuleItem, taskTickets, service);

            //changed For New Ui admin Userinfo login & submit 
            if (requestPage == "SelfRegistration")
            {
                _context = ApplicationContext.Create();
            }

            DataRow SVCParentItem = Ticket.GetCurrentTicket(_context, "SVC", Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]));

            UGITTaskManager taskManager = new UGITTaskManager(_context);
            if (SVCParentItem != null)
                taskManager.UpdateSVCPRPORP(SVCParentItem, requestPage);

            lstTickets.Add(Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]));
            if (service.AllowServiceTasksInBackground)
            {
                // SPListHelper.ReloadTicketsInCache(lstTickets, web);
                ULog.WriteLog("Done creating tickets and tasks for ticket " + Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]));
            }

            List<UGITTask> taskList = taskManager.LoadByProjectID(Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId])); //.LoadByProjectID.LoadByPublicID(web, Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]), includeOpenTicketsOnly: false);

            //Create Reminders for SVC tasks in ScheduleActions List

            bool needTaskReminder = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValueAsString(sModuleItem, DatabaseObjects.Columns.EnableTaskReminder));
            if (taskList != null && taskList.Count > 0 && needTaskReminder)
                UGITTaskManager.UpdateScheduleActions(_context, taskList, needTaskReminder, service);
            return serviceResponseTreeNodeParent;
        }

        /// <summary>
        /// This method creates service request
        /// </summary>
        /// <param name="qMappingList"></param>
        /// <param name="service"></param>
        /// <param name="serviceRequestDTO"></param>
        /// <returns></returns>
        private DataRow CreateServiceRequest(List<ServiceQuestionMapping> qMappingList, Services service, ServiceRequestDTO serviceRequestDTO)
        {
            //Creates new service request and get splistitem
            DataRow sModuleItem = null;
            Dictionary<string, object> sTicketData = new Dictionary<string, object>();
            List<ServiceQuestionMapping> serviceInstMappingList = qMappingList.Where(x => x.ServiceTaskID == 0 || x.ServiceTaskID == null).ToList();

            UGITModule sModuleDetail = ModuleManager.LoadByName("SVC");//   uGITCache.GetModuleDetails("SVC");
            string sModuleListName = sModuleDetail.ModuleTable;
            long sModuleId = sModuleDetail.ID;
            string sModuleName = sModuleDetail.ModuleName;
            ///DataRow[] sModuleStagesRow = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, sModuleName).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();
            DataTable sModuleList = ObjTicketManager.GetAllTickets(sModuleDetail);// GetTableDataManager.GetTableData(sModuleListName); //SPListHelper.GetSPList(sModuleListName);
            //Picks mapping list and iterate one by one and find mapping value
            foreach (ServiceQuestionMapping questMap in serviceInstMappingList)
            {
                if (!sModuleList.Columns.Contains(questMap.ColumnName))
                    continue;

                if (sTicketData.ContainsKey(questMap.ColumnName))
                {
                    sTicketData[questMap.ColumnName] = generateColumnValue(sModuleId, sModuleName, questMap, sModuleList.Columns[questMap.ColumnName], service, serviceRequestDTO);
                }
                else
                {
                    sTicketData.Add(questMap.ColumnName, generateColumnValue(sModuleId, sModuleName, questMap, sModuleList.Columns[questMap.ColumnName], service, serviceRequestDTO));
                }
            }

            if (sTicketData.ContainsKey(DatabaseObjects.Columns.Title))
            {
                if (string.IsNullOrEmpty(Convert.ToString(sTicketData[DatabaseObjects.Columns.Title])))
                    sTicketData[DatabaseObjects.Columns.Title] = service.Title;
            }
            else
            {
                sTicketData.Add(DatabaseObjects.Columns.Title, service.Title);
            }

            if (sTicketData.ContainsKey(DatabaseObjects.Columns.Title))
            {
                sTicketData[DatabaseObjects.Columns.TicketOwner] = service.OwnerUser;
            }
            else
            {
                sTicketData.Add(DatabaseObjects.Columns.TicketOwner, service.OwnerUser);
            }

            //If requestor field is not present in dictionary then create field and assign current user into it
            //If requestor field in empty or null then assign current user into it
            if (!sTicketData.ContainsKey(DatabaseObjects.Columns.TicketRequestor))
            {
                sTicketData.Add(DatabaseObjects.Columns.TicketRequestor, _context.CurrentUser.Id/*string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName)*/);
            }
            else if (sTicketData[DatabaseObjects.Columns.TicketRequestor] == null || Convert.ToString(sTicketData[DatabaseObjects.Columns.TicketRequestor]).Trim() == string.Empty)
            {
                if (_context.CurrentUser == null && HttpContext.Current.Request["requestPage"].Trim().ToLower() == Convert.ToString(ConfigurationManager.AppSettings["OnBoardingLoggingURL"]).Trim().ToLower())
                {
                    _context = ApplicationContext.Create();
                }
                sTicketData[DatabaseObjects.Columns.TicketRequestor] = _context.CurrentUser.Id; // string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName);
            }

            //Forcely set ticketinitiator field value to current loggedin user
            if (!sTicketData.ContainsKey(DatabaseObjects.Columns.TicketInitiator))
            {
                sTicketData.Add(DatabaseObjects.Columns.TicketInitiator, _context.CurrentUser.Id /*string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName)*/);
            }
            else
            {
                sTicketData[DatabaseObjects.Columns.TicketInitiator] = _context.CurrentUser.Id;  // string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName);
            }

            if (service.Questions != null && service.Questions.Count() > 0)
            {
                var removeUserAccessQuestion = service.Questions.Where(c => c.QuestionType.ToLower() == "removeuseraccess").FirstOrDefault();
                if (removeUserAccessQuestion != null)
                {
                    var removeAccessValue = serviceRequestDTO.Questions.Where(c => c.Token == removeUserAccessQuestion.TokenName).Select(c => c).FirstOrDefault();
                    // SPList applicationList = SPListHelper.GetSPList(DatabaseObjects.Lists.Applications);
                    DataTable applicationList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                    // RemoveAccessList removeAccessList = new RemoveAccessList();
                    //XmlDocument doc = new XmlDocument();
                    // doc.LoadXml(removeAccessValue.Value.Trim());
                    // removeAccessList = (RemoveAccessList)uHelper.DeSerializeAnObject(doc, removeAccessList);
                    // if (removeAccessList != null && removeAccessList.SelectionType.ToLower() == "removeallaccess" && removeAccessList.ModuleRoleRelationList != null && removeAccessList.ModuleRoleRelationList.Count() > 0)
                    {
                        //List<ModuleRoleRelation> lstModuleRoleRelation = removeAccessList.ModuleRoleRelationList.GroupBy(c => c.ApplicationId).Select(group => group.First()).ToList();
                        string applications = string.Empty;
                        //string RoleAssignee = string.Empty;
                        //SPFieldUserValue arrRoleAssignee = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(removeAccessList.UserId));
                        //if (arrRoleAssignee != null && arrRoleAssignee.User != null)
                        //    RoleAssignee = arrRoleAssignee.User.Name;
                        List<string> lstApplications = new List<string>();
                        //foreach (ModuleRoleRelation moduleRoleRelation in lstModuleRoleRelation)
                        //{
                        //    SPQuery query = new SPQuery();
                        //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Id, moduleRoleRelation.ApplicationId);
                        //    SPListItemCollection splistCollection = applicationList.GetItems(query);
                        //    lstApplications.Add(Convert.ToString(splistCollection[0][DatabaseObjects.Columns.Title]));
                        //}
                        if (lstApplications != null && lstApplications.Count > 0)
                        {
                            lstApplications = lstApplications.OrderBy(x => x.ToString()).ToList();
                            applications = String.Join(", ", lstApplications.Select(x => x.ToString()).ToArray());
                        }
                        if (sTicketData.ContainsKey(DatabaseObjects.Columns.TicketDescription))
                        {
                            sTicketData[DatabaseObjects.Columns.TicketDescription] = string.Format("Remove all access for {0} on applications: {1}", string.Empty, applications);
                        }
                        else
                        {
                            sTicketData.Add(DatabaseObjects.Columns.TicketDescription, string.Format("Remove all access for {0} on applications: {1}", string.Empty, applications));
                        }
                    }
                }

            }
            Ticket sModuleTicket = new Ticket(_context, sModuleId);
            //Note: Forward required manager approval flag to service ticket if OwnerApprovalRequired is true
            sTicketData.Add(DatabaseObjects.Columns.OwnerApprovalRequired, service.OwnerApprovalRequired ? 1 : 0);
            sTicketData.Add(DatabaseObjects.Columns.ServiceTitleLookup, service.ID.ToString());

            sModuleItem = sModuleTicket.CreateModuleInstance(sTicketData);
            if (ServiceInput != null)
            {
                XmlDocument dc = uHelper.SerializeObject(ServiceInput);
                sModuleItem[DatabaseObjects.Columns.UserQuestionSummary] = dc.InnerXml;
            }
            sModuleItem[DatabaseObjects.Columns.SLADisabled] = service.SLADisabled;
            sModuleItem[DatabaseObjects.Columns.EnableTaskReminder] = service.EnableTaskReminder;
            SaveAttachments(sModuleItem, serviceRequestDTO);
            sModuleTicket.CommitChanges(sModuleItem, null);
            return sModuleItem;
        }

        /// <summary>
        /// Gets all the tasks, needs to be created leaving the task need to be skipped.
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="service"></param>
        /// <param name="ServiceRequestDTO"></param>
        /// <returns></returns>
        private List<UGITTask> GetTasksNeedToCreate(List<UGITTask> tasks, Services service, ServiceRequestDTO ServiceRequestDTO)
        {
            if (service.SkipTaskCondition != null && service.SkipTaskCondition.Count > 0)
            {
                DataTable skipTaskConditions = GetSkipTaskConditions(service.SkipTaskCondition, service, ServiceRequestDTO);
                bool isTrue = false;
                ServiceTaskCondition taskCondition = null;
                foreach (DataColumn coln in skipTaskConditions.Columns)
                {
                    isTrue = UGITUtility.StringToBoolean(skipTaskConditions.Rows[0][coln.ColumnName]);
                    if (isTrue)
                    {
                        taskCondition = service.SkipTaskCondition.FirstOrDefault(x => x.ID.ToString() == coln.ColumnName);
                        foreach (int taskID in taskCondition.SkipTasks)
                        {
                            UGITTask sTask = tasks.FirstOrDefault(x => x.ID == taskID);
                            if (sTask != null)
                            {
                                tasks.Remove(sTask);
                            }
                        }
                    }
                    isTrue = false;
                }
            }
            return tasks;
        }

        /// <summary>
        /// gets the skip task condition
        /// </summary>
        /// <param name="Conditions"></param>
        /// <param name="service"></param>
        /// <param name="ServiceRequestDTO"></param>
        /// <returns></returns>
        private DataTable GetSkipTaskConditions(List<ServiceTaskCondition> Conditions, Services service, ServiceRequestDTO ServiceRequestDTO)
        {
            //Creates table which contain all condition as boolean column
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);
            DataTable conditionTable = new DataTable();
            foreach (ServiceTaskCondition condition in Conditions)
            {
                conditionTable.Columns.Add(condition.ID.ToString(), typeof(bool));
            }

            string conditionToken = string.Empty;

            //Iterate each condition to check its validity
            foreach (ServiceTaskCondition condition in Conditions)
            {
                //If there is not condition defined then just discard that condition
                if (condition.Conditions == null || condition.Conditions.Count <= 0) continue;


                //Only picks first condition because we are supporting and or logical operator right now.
                WhereExpression whereClause = condition.Conditions[0];

                //Gets question token and if it is not come with [$$] format then remove it into token
                conditionToken = whereClause.Variable;
                if (conditionToken.IndexOf("[$") > 0)
                {
                    conditionToken = conditionToken.Replace("[$", string.Empty).Replace("$]", string.Empty);
                }

                //Discard condition if condition's question is not found against token
                ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == conditionToken.ToLower());
                if (tokenQuestion == null) continue;

                //Discard condition if input is not ask yet
                string lsValue = string.Empty;

                var tokenQuestionInput = ServiceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == conditionToken.ToLower());
                if (tokenQuestionInput != null)
                    lsValue = tokenQuestionInput.Value;
                bool result = serviceQuestionManager.TestCondition(lsValue, whereClause.Operator, whereClause.Value, tokenQuestion.QuestionType);
                conditionTable.Columns[condition.ID.ToString()].Expression = string.Format("{0}", result);
            }

            DataRow row = conditionTable.NewRow();
            conditionTable.Rows.Add(row);

            return conditionTable;
        }

        /// <summary>
        /// This method saves each task and ticket in the ticketrelationship list
        /// and moves service tasks if owner approval is NOT Required && all tasks are assigned else not.
        /// </summary>
        /// <param name="serviceTaskList"></param>
        /// <param name="mapTable"></param>
        /// <param name="sModuleItem"></param>
        /// <param name="taskTickets"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        private ServiceResponseTreeNodeParent MoveTicketToServiceRelationship(List<UGITTask> serviceTaskList, DataRow sModuleItem, Dictionary<long, TaskTempInfo> taskTickets, Services service)
        {
            DataRow mapRow = null;
            DataTable mapTable = new DataTable();
            mapTable.Columns.Add("configID", typeof(int));
            mapTable.Columns.Add("reqID", typeof(int));

            //Moves TicketRelationship define in service to service request relationship
            List<UGITTask> mInstDependencies = new List<UGITTask>();
            UGITModule sModuleDetail = ModuleManager.LoadByName("SVC");//uGITCache.GetModuleDetails("SVC");
            string sModuleListName = Convert.ToString(sModuleDetail.ModuleTable);

            serviceTaskList = serviceTaskList.OrderBy(x => x.ItemOrder).ToList();
            for (int i = 1; i <= serviceTaskList.Count; i++)
            {
                serviceTaskList[i - 1].ItemOrder = i;
            }

            foreach (UGITTask task in serviceTaskList)
            {
                mapRow = mapTable.NewRow();
                mapRow["configID"] = task.ID;

                UGITTask dependency = new UGITTask();
                dependency.ParentInstance = Convert.ToString(UGITUtility.GetSPItemValue(sModuleItem, DatabaseObjects.Columns.TicketId));
                dependency.TicketId = dependency.ParentInstance;
                TaskTempInfo taskTempInfo = null;
                taskTickets.TryGetValue(task.ID, out taskTempInfo);
                if (taskTempInfo == null)
                    continue;

                dependency.StageStep = taskTempInfo.StageStep;
                dependency.EstimatedHours = taskTempInfo.EstimatedHours;
                if (task.RelatedModule != null)
                {
                    dependency.ChildInstance = taskTempInfo.TicketID;
                    dependency.ModuleNameLookup = task.RelatedModule;   // uGITCache.ModuleConfigCache.getModuleName(taskTempInfo.ModuleID);
                    dependency.Title = taskTempInfo.Title;
                    dependency.Description = task.Description;
                    dependency.Status = "Waiting";
                    dependency.SLADisabled = taskTempInfo.SLADisabled;
                }
                else
                {
                    if (taskTempInfo.UGITNewUserName != string.Empty)
                        dependency.NewUserName = taskTempInfo.UGITNewUserName;

                    dependency.ChildInstance = string.Empty;
                    dependency.Status = "Waiting";
                    dependency.AssignedTo = taskTempInfo.AssignTo;
                    if (dependency.AssignedTo == null /*|| dependency.AssignedTo.Count <= 0*/)
                    {
                        dependency.AssignedTo = task.AssignedTo;
                    }
                    dependency.Title = taskTempInfo.Title;
                    if (string.IsNullOrEmpty(dependency.Title))
                    {
                        dependency.Title = task.Title;
                    }
                    if (!string.IsNullOrEmpty(task.SubTaskType))
                    {
                        dependency.SubTaskType = task.SubTaskType;
                    }
                    dependency.Description = taskTempInfo.Desc;
                    if (string.IsNullOrEmpty(dependency.Description))
                    {
                        dependency.Description = task.Description;
                    }

                    dependency.EndDate = taskTempInfo.DueDate;
                    dependency.IsEndDateChange = true;

                    if (task.EnableApproval)
                    {
                        dependency.Approver = taskTempInfo.Approver;
                        if (dependency.Approver == null /*|| dependency.Approver.Count == 0*/)
                            dependency.Approver = task.Approver;
                        if (dependency.Approver != null)
                        {
                            dependency.ApprovalStatus = TaskApprovalStatus.NotStarted;
                            dependency.ApprovalType = ApprovalType.All;
                            //dependency.TaskActionUser = String.Join(Constants.Separator, dependency.Approver.Select(x => x.LookupId));
                        }
                        else
                            task.EnableApproval = false;
                    }
                    dependency.SLADisabled = task.SLADisabled;
                }
                //Assignee Notification
                dependency.NotificationDisabled = task.NotificationDisabled;
                // Update parent task with the help of  ItemOrder & TaskLevel. Can't use parent id directly because id's are differnt from source table to destination table 
                dependency.ParentInstance = "0";
                UGITTask serviceTask = serviceTaskList.FirstOrDefault(m => m.ID == task.ParentTaskID);
                if (serviceTask != null)
                {
                    UGITTask mID = mInstDependencies.FirstOrDefault(x => x.ItemOrder == serviceTask.ItemOrder && x.Level == serviceTask.Level);
                    if (mID != null)
                    {
                        dependency.ParentTaskID = mID.ID;
                    }
                }

                dependency.Level = task.Level;
                dependency.ItemOrder = task.ItemOrder;
                dependency.Weight = task.Weight;
                dependency.ServiceApplicationAccessXml = taskTempInfo.ServiceApplicationAccessXml;
                dependency.SubTaskType = task.SubTaskType;
                dependency.AutoCreateUser = task.AutoCreateUser;
                dependency.AutoFillRequestor = task.AutoFillRequestor;
                dependency.ModuleNameLookup = "SVC";
                dependency.DueDate = taskTempInfo.DueDate;
                dependency.Behaviour = task.Behaviour;
                dependency.RelatedModule = task.RelatedModule;
                dependency.RelatedTicketID = taskTempInfo.TicketID;
                //dependency.ID = task.ID; commented by mudassir 2 march 2020
                UGITTaskManager taskManager = new UGITTaskManager(_context);
                taskManager.Save(dependency, _context);
                //taskManager.SaveTask(ref dependency, dependency.ModuleNameLookup, dependency.TicketId);
                mapRow["reqID"] = dependency.ID;
                mInstDependencies.Add(dependency);

                mapTable.Rows.Add(mapRow);
                mapRow = null;
            }

            //  List<ServiceQuestion> serviceQuestionsList = service.Questions.Where(c => c.QuestionType.ToLower() == "applicationaccessrequest").ToList();
            List<UGITTask> taskList = serviceTaskList.Where(z => z.SubTaskType.ToLower() == ServiceSubTaskType.ApplicationAccessRequest).ToList();
            foreach (UGITTask inst in taskList)
            {
                //if (inst.PickUserFrom == ConfigConstants.NewUSer && inst.UseADAuthentication)
                //{
                //    UGITTask createAccountTask = serviceTaskList.FirstOrDefault(x => x.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccountTask);
                //    if (createAccountTask != null)
                //    {
                //        SPFieldLookupValueCollection taskPredecessor = new SPFieldLookupValueCollection();
                //        SPFieldLookupValue preLookup = new SPFieldLookupValue();
                //        preLookup.LookupId = createAccountTask.ID;
                //        taskPredecessor.Add(preLookup);
                //        inst.Predecessors = taskPredecessor;
                //    }
                //}
            }

            //Save predecessors of the each task and ticket
            SavePredecessors(serviceTaskList, mapTable, mInstDependencies);

            // Add Related Tickets to Services in TicketRelation table
            RelateTickets(mInstDependencies);

            List<UGITTask> midepny = mInstDependencies.Where(x => x.SubTaskType == "applicationaccessrequest").ToList();
            UGITTask depndntTask = new UGITTask();
            foreach (UGITTask depndntTask1 in midepny)
            {
                depndntTask = depndntTask1;
                UGITTask createAccountTask = mInstDependencies.FirstOrDefault(x => x.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask /*&& depndntTask.Predecessors != null && depndntTask.Predecessors.con(y => y.LookupId == x.ID)*/);

                if (createAccountTask != null)
                {
                    depndntTask.Predecessors = Convert.ToString(createAccountTask.ID);
                    //depndntTask.NewUserName = createAccountTask.NewUserName;
                    //depndntTask.Title += " for " + createAccountTask.NewUserName;
                    UGITTaskManager taskManager = new UGITTaskManager(_context);
                    taskManager.SaveTask(ref depndntTask);
                }
            }

            ServiceResponseTreeNodeParent serviceResponseTreeNodeParent = GenerateTaskSummary(mInstDependencies, sModuleItem);

            //Reload service request ticket again
            if (taskTickets.Count > 0)
            {
                // sModuleItem = SPListHelper.GetSPListItem(sModuleItem.Table.Title, sModuleItem.ID);
                sModuleItem[DatabaseObjects.Columns.TicketEstimatedHours] = taskTickets.Sum(x => x.Value.EstimatedHours);
                // sModuleItem.UpdateOverwriteVersion();
            }

            Ticket sTicket = new Ticket(_context, "SVC");

            bool startTask = sTicket.IsNextStageSkip(sModuleItem);
            sTicket.QuickClose(sTicket.Module.ID, sModuleItem, string.Empty);
            sTicket.CommitChanges(sModuleItem, null, donotUpdateEscalations: true);

            //Move service tasks if owner approval is NOT Required && all tasks are assigned
            LifeCycleStage svcCurrentStage = sTicket.GetTicketCurrentStage(sModuleItem);
            if (svcCurrentStage != null && svcCurrentStage.StageTypeChoice == StageType.Assigned.ToString())
            {
                UGITTaskManager taskManager = new UGITTaskManager(_context);
                taskManager.StartTasks(Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]));
                taskManager.MoveSVCTicket(Convert.ToString(sModuleItem[DatabaseObjects.Columns.TicketId]));
            }

            // Must do this last!
            if (mInstDependencies.Exists(x => x.ModuleNameLookup == null))
            {
                //TaskCache.ReloadProjectTasks("SVC", mInstDependencies[0].ParentInstance);
            }

            return serviceResponseTreeNodeParent;
        }

        private void RelateTickets(List<UGITTask> serviceTaskList)
        {
            TicketRelationManager ticketRelationManager = new TicketRelationManager(_context);
            TicketRelation relation = null;
            foreach (var item in serviceTaskList.Where(x => x.Behaviour.EqualsIgnoreCase("Ticket")))
            {
                relation = new TicketRelation();
                relation.ParentTicketID = item.TicketId;
                relation.ChildTicketID = item.RelatedTicketID;
                relation.ChildModuleName = item.RelatedModule;
                relation.ParentModuleName = uHelper.getModuleNameByTicketId(item.TicketId);
                ticketRelationManager.Insert(relation);
            }
        }


        /// <summary>
        /// This method creates task and tickets based on condition
        /// </summary>
        /// <param name="serviceTaskList"></param>
        /// <param name="qMappingList"></param>
        /// <param name="service"></param>
        /// <param name="serviceRequestDTO"></param>
        /// <returns></returns>
        private Dictionary<long, TaskTempInfo> CreateRelatedTickets(List<UGITTask> serviceTaskList, List<ServiceQuestionMapping> qMappingList, Services service, ServiceRequestDTO serviceRequestDTO, ApplicationContext context)
        {
            if (context != null)
                _context = context;
            Dictionary<long, TaskTempInfo> taskTickets = new Dictionary<long, TaskTempInfo>();

            List<UGITTask> tickets = serviceTaskList.Where(x => string.Equals(x.ModuleNameLookup, "SVCConfig", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(x.RelatedModule)).ToList();
            foreach (UGITTask ticket in tickets)
            {
                Dictionary<string, object> ticketData = new Dictionary<string, object>();
                List<ServiceQuestionMapping> questMappingList = qMappingList.Where(x => x.ServiceTaskID == ticket.ID).ToList();
                UGITModule moduleDetail = ModuleManager.LoadByName(ticket.RelatedModule);
                string moduleListName = moduleDetail.ModuleTable;
                long moduleId = moduleDetail.ID;
                string moduleName = moduleDetail.ModuleName;
                // DataRow[] moduleStagesRow = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, ticket.RelatedModule).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                DataTable moduleList = ObjTicketManager.GetAllTickets(moduleDetail);// GetTableDataManager.GetTableData(moduleListName); //SPListHelper.GetSPList(moduleListName);
                foreach (ServiceQuestionMapping questMap in questMappingList)
                {
                    if (!moduleList.Columns.Contains(questMap.ColumnName))
                        continue;

                    if (ticketData.ContainsKey(questMap.ColumnName))
                    {
                        continue;
                        //ticketData[questMap.ColumnName] = generateColumnValue(moduleId, moduleName, questMap, moduleList.Columns[questMap.ColumnName], service, serviceRequestDTO);
                    }
                    else
                    {
                        ticketData.Add(questMap.ColumnName, generateColumnValue(moduleId, moduleName, questMap, moduleList.Columns[questMap.ColumnName], service, serviceRequestDTO));
                    }
                }

                //If request type setting in null is config ticket then don't need to sent requesttype 
                //Because it should be mapped to question.
                if (!string.IsNullOrEmpty(ticket.RequestTypeCategory))
                {
                    if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        ticketData[DatabaseObjects.Columns.TicketRequestTypeLookup] = ticket.RequestTypeCategory;
                    }
                    else
                    {
                        ticketData.Add(DatabaseObjects.Columns.TicketRequestTypeLookup, ticket.RequestTypeCategory);
                    }
                }

                //Sets ticketstatus to waiting if ParentServiceRequest Need to create
                if (service.CreateParentServiceRequest)
                {
                    if (ticketData.ContainsKey(DatabaseObjects.Columns.TicketStatus))
                    {
                        ticketData[DatabaseObjects.Columns.TicketStatus] = Constants.Waiting;
                    }
                    else
                    {
                        ticketData.Add(DatabaseObjects.Columns.TicketStatus, Constants.Waiting);
                    }

                }

                //If requestor field is not present in dictionary then create field and assign current user into it
                //If requestor field in empty or null then assign current user into it
                if (!ticketData.ContainsKey(DatabaseObjects.Columns.TicketRequestor))
                {
                    ticketData.Add(DatabaseObjects.Columns.TicketRequestor, string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName));
                }
                else if (ticketData[DatabaseObjects.Columns.TicketRequestor] == null || Convert.ToString(ticketData[DatabaseObjects.Columns.TicketRequestor]).Trim() == string.Empty)
                {
                    ticketData[DatabaseObjects.Columns.TicketRequestor] = string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName);
                }

                //Forcely set ticketinitiator field value to current loggedin user
                if (!ticketData.ContainsKey(DatabaseObjects.Columns.TicketInitiator))
                {
                    ticketData.Add(DatabaseObjects.Columns.TicketInitiator, string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName));
                }
                else
                {
                    ticketData[DatabaseObjects.Columns.TicketInitiator] = string.Format("{0}{1}{2}", _context.CurrentUser.Id, Constants.Separator, _context.CurrentUser.UserName);
                }

                Ticket moduleTicket = new Ticket(_context, moduleId);
                if (!ticketData.ContainsKey(DatabaseObjects.Columns.Title))
                    ticketData.Add(DatabaseObjects.Columns.Title, ticket.Title);
                DataRow moduleItem = moduleTicket.CreateModuleInstance(ticketData);

                //Execute Ticket Stage Process is CreateParentServiceRequest is false.
                //Because During ParentServiceRequest, We need to start only non related ticket and task first.

                if (!service.CreateParentServiceRequest)
                {
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.UserQuestionSummary, moduleItem.Table) && ServiceInput != null)
                    {
                        XmlDocument dc = uHelper.SerializeObject(ServiceInput);
                        moduleItem[DatabaseObjects.Columns.UserQuestionSummary] = dc.OuterXml;

                    }
                    moduleTicket.CommitChanges(moduleItem, null, donotUpdateEscalations: true);
                    moduleTicket.QuickClose(moduleId, moduleItem, "0", Convert.ToString(TicketActionType.Created));
                }

                // Tie ticket back to the type of service that created it
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ServiceTitleLookup, moduleItem.Table))
                    moduleItem[DatabaseObjects.Columns.ServiceTitleLookup] = service.ID;
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.UserQuestionSummary, moduleItem.Table) && ServiceInput != null)
                {
                    XmlDocument dc = uHelper.SerializeObject(ServiceInput);
                    moduleItem[DatabaseObjects.Columns.UserQuestionSummary] = dc.OuterXml;

                }
                if (!service.CreateParentServiceRequest || service.AttachmentsInChildTickets)
                    SaveAttachments(moduleItem, serviceRequestDTO);
                if (service.CreateParentServiceRequest && UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, moduleItem.Table))
                {
                    moduleItem[DatabaseObjects.Columns.SLADisabled] = ticket.SLADisabled;
                }
                //Update escalation if service is not being created in background
                moduleTicket.CommitChanges(moduleItem, null, donotUpdateEscalations: service.CreateParentServiceRequest);
                if (moduleName == "NPR")
                {
                    var resourceQuestion = service.Questions.Where(c => c.QuestionType.ToLower() == "resources").FirstOrDefault();
                    if (resourceQuestion != null)
                    {
                        var reqourceQuestionValue = serviceRequestDTO.Questions.Where(c => c.Token == resourceQuestion.TokenName).Select(c => c).FirstOrDefault();
                        XmlDocument doc = new XmlDocument();
                        List<NPRResource> lstResources = new List<NPRResource>();
                        doc.LoadXml(reqourceQuestionValue.Value.Trim());
                        lstResources = (List<NPRResource>)uHelper.DeSerializeAnObject(doc, lstResources);
                        //SPList nprResourcesList = SPListHelper.GetSPList(DatabaseObjects.Lists.NPRResources);
                        NPRResourcesManager objNPRResourcesManager = new NPRResourcesManager(_context);
                        List<NPRResource> listNPRResources = objNPRResourcesManager.Load();
                        foreach (NPRResource resource in lstResources)
                        {
                            // SPListItem item = nprResourcesList.AddItem();
                            // item[DatabaseObjects.Columns.UserSkillLookup] = resource.UserSkillLookup.SkillID;
                            // item[DatabaseObjects.Columns.BudgetType] = resource.BudgetType;
                            // item[DatabaseObjects.Columns._ResourceType] = resource._ResourceType;
                            // item[DatabaseObjects.Columns.TicketNoOfFTEs] = resource.TicketNoOfFTEs;
                            //  item[DatabaseObjects.Columns.AllocationStartDate] = resource.AllocationStartDate;
                            //  item[DatabaseObjects.Columns.AllocationEndDate] = resource.AllocationEndDate;
                            //  item[DatabaseObjects.Columns.BudgetDescription] = resource.BudgetDescription;
                            //  item[DatabaseObjects.Columns.TicketNPRIdLookup] = moduleItem.ID;
                            //  item[DatabaseObjects.Columns.EstimatedHours] = resource.EstimatedHours;
                            //  item[DatabaseObjects.Columns.RoleName] = resource.RoleName;
                            //  item[DatabaseObjects.Columns.HourlyRate] = resource.HourlyRate;
                            //  SPFieldUserValueCollection users = new SPFieldUserValueCollection();
                            //string[] usersArr = resource.RequestedResources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            //foreach (var user in usersArr)
                            //{
                            //    UserProfile userProfile = UserProfile.LoadUserProfileByName(user, web);
                            //    if (userProfile != null)
                            //        users.Add(new SPFieldUserValue(web, userProfile.ID, userProfile.LoginName));
                            //}

                            //item[DatabaseObjects.Columns.RequestedResources] = users;
                            // item.Update();
                            //if (resource.CreateBudget)
                            //{
                            //    Budget nprBudget = GetNPRBudget(resource);
                            //    nprBudget.NPRId = moduleItem.ID;
                            //    nprBudget.NPRResourceId = item.ID;
                            //    if (nprBudget.Id > 0)
                            //        Budget.Update(nprBudget.Id, nprBudget);
                            //    else
                            //        nprBudget.Insert(web);
                            //}
                        }


                    }
                }

                //ticket.ChildInstance = Convert.ToString(moduleItem[DatabaseObjects.Columns.TicketId]);
                TaskTempInfo taskTempInfo = new TaskTempInfo();
                taskTempInfo.TaskID = ticket.ID;
                taskTempInfo.ModuleID = Convert.ToInt32(moduleId);
                taskTempInfo.TicketID = Convert.ToString(moduleItem[DatabaseObjects.Columns.TicketId]);
                taskTempInfo.Title = Convert.ToString(moduleItem[DatabaseObjects.Columns.Title]);
                taskTempInfo.StageStep = ticket.StageStep;
                if (moduleItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketEstimatedHours) && moduleItem[DatabaseObjects.Columns.TicketEstimatedHours] != DBNull.Value)
                {
                    taskTempInfo.EstimatedHours = Convert.ToDouble(moduleItem[DatabaseObjects.Columns.TicketEstimatedHours]);
                }
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, moduleItem.Table))
                {
                    taskTempInfo.SLADisabled = UGITUtility.StringToBoolean(moduleItem[DatabaseObjects.Columns.SLADisabled]);
                }
                if (moduleItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && moduleItem[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                {
                    taskTempInfo.DueDate = Convert.ToDateTime(moduleItem[DatabaseObjects.Columns.TicketTargetCompletionDate]);
                }
                taskTickets.Add(ticket.ID, taskTempInfo);
            }
            return taskTickets;
        }

        private ModuleBudget GetNPRBudget(NPRResource resource)
        {
            ModuleBudget _nprBudget = new ModuleBudget();
            _nprBudget.budgetCategory = new BudgetCategory();
            // _nprBudget.budgetCategory.ID = resource.BudgetCategoryId;
            // _nprBudget.BudgetAmount = resource.;
            _nprBudget.BudgetDescription = resource.BudgetDescription;
            _nprBudget.BudgetItem = resource._ResourceType;
            _nprBudget.AllocationStartDate = resource.AllocationStartDate;
            _nprBudget.AllocationEndDate = resource.AllocationEndDate;
            return _nprBudget;
        }

        private void SaveAttachments(DataRow item, ServiceRequestDTO serviceRequestDTO)
        {
            string tempPath = uHelper.GetTempFolderPath();
            if (item != null)
            {
                QuestionsDTO questionAttachemsts = serviceRequestDTO.Questions.Where(c => c.Token == "attachments").FirstOrDefault();
                if (questionAttachemsts != null && !string.IsNullOrWhiteSpace(questionAttachemsts.Value))
                {
                    item[DatabaseObjects.Columns.Attachments] = questionAttachemsts.Value;
                    List<string> attachedFiles = new List<string>();
                    attachedFiles = questionAttachemsts.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string fileName in attachedFiles)
                    {
                        if (File.Exists(string.Format("{0}\\{1}", tempPath, fileName)))
                        {
                            byte[] byteArray = File.ReadAllBytes(string.Format("{0}\\{1}", tempPath, fileName));
                            //SPAttachmentCollection attachments = item.Attachments;
                            string extension = Path.GetExtension(fileName);
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                            string Name = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.LastIndexOf('_')) + extension;
                            //attachments.Add(Name, byteArray);
                        }
                        else
                            ULog.WriteLog(string.Format("{0} file not found.", fileName));
                    }
                }
            }
        }

        /// <summary>
        /// Saves the predeccessors
        /// </summary>
        /// <param name="serviceTaskList"></param>
        /// <param name="mapTable"></param>
        /// <param name="mInstDependencies"></param>
        private void SavePredecessors(List<UGITTask> serviceTaskList, DataTable mapTable, List<UGITTask> mInstDependencies)
        {
            DataRow mappedRow = null;
            if (mapTable == null || mapTable.Rows.Count < 1)
                return;
            foreach (UGITTask task in serviceTaskList)
            {
                mappedRow = mapTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("configID") == task.ID);

                int depID = 0;
                int.TryParse(UGITUtility.ObjectToString(mappedRow["reqID"]), out depID);
                UGITTask midepny = mInstDependencies.FirstOrDefault(x => x.ID == depID);
                if (midepny != null)
                {
                    List<string> predecessors = UGITUtility.ConvertStringToList(task.Predecessors, Constants.Separator6);
                    List<string> newPredecessors = new List<string>();
                    if (predecessors != null && predecessors.Count > 0)
                    {
                        foreach (string lookVal in predecessors)
                        {
                            mappedRow = mapTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("configID") == Convert.ToInt32(lookVal));
                            if (mappedRow != null)
                            {
                                int.TryParse(Convert.ToString(mappedRow["reqID"]), out depID);

                                UGITTask mdepny = mInstDependencies.FirstOrDefault(x => x.ID == depID);
                                if (mdepny != null)
                                {
                                    string fLookup = Convert.ToString(mdepny.ID);
                                    newPredecessors.Add(fLookup);
                                }
                            }
                        }
                    }

                    if (newPredecessors != null && newPredecessors.Count > 0)
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(_context);
                        midepny.Predecessors = string.Join(",", newPredecessors);
                        taskManager.SaveTask(ref midepny);
                    }
                }
            }
        }

        /// <summary>
        /// This method gets value based on token
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="moduleName"></param>
        /// <param name="questMap"></param>
        /// <param name="field"></param>
        /// <param name="service"></param>
        /// <param name="serviceRequestDTO"></param>
        /// <returns></returns>
        public object generateColumnValue(long moduleID, string moduleName, ServiceQuestionMapping questMap, DataColumn field, Services service, ServiceRequestDTO serviceRequestDTO)
        {
            object value = null;

            string resultedValue = string.Empty;
            string defaultValue = questMap.ColumnValue;
            string mappedInputValue = string.Empty;
            string mappedVInputValue = string.Empty;
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);
            //Picks default question if any is mapped to field
            ServiceQuestion mappedQuestion = service.Questions.FirstOrDefault(x => x.ID == questMap.ServiceQuestionID);
            QuestionMapVariable mappedVariable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == questMap.PickValueFrom.ToLower());

            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(_context);
            FieldConfiguration configField = configFieldManager.GetFieldByFieldName(field.ColumnName);

            string fieldColumnType = string.Empty;
            if (configField != null)
            {
                fieldColumnType = Convert.ToString(configField.Datatype);
            }
            else
                fieldColumnType = Convert.ToString(field.DataType);
            //If question is mapped then pick question input value from input object.
            //ServiceQuestionInput mappedQInputObj = null;
            QuestionsDTO mappedQInputObj = null;
            QuestionsDTO mappedVInputObj = null;

            //Fetch mapped question value
            if (mappedQuestion != null && serviceRequestDTO.Questions != null && serviceRequestDTO.Questions.Count() > 0)
            {
                mappedQInputObj = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == mappedQuestion.TokenName);
                if (mappedQInputObj != null && (mappedQInputObj.Value != null && mappedQInputObj.Value.Trim() != string.Empty))
                {

                    mappedInputValue = mappedQInputObj.Value;

                    //for issue type, pick alue from request type sub token
                    if (questMap.ColumnName == DatabaseObjects.Columns.UGITIssueType && mappedQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                    {
                        string issueTypeToken = string.Format("{0}|issuetype", mappedQuestion.TokenName);
                        string issueTypes = GetSubTokenValue(issueTypeToken, mappedQuestion, mappedQInputObj);
                        mappedInputValue = string.Empty;
                        if (!string.IsNullOrWhiteSpace(issueTypes))
                            mappedInputValue = issueTypes.Replace("; ", Constants.Separator);
                    }
                }
            }

            //Fetch mappedvariable value
            if (mappedVariable != null && serviceRequestDTO.Questions != null && serviceRequestDTO.Questions.Count() > 0)
            {
                mappedVInputObj = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == mappedVariable.ShortName);
                if (mappedVInputObj != null && (mappedVInputObj.Value != null && mappedVInputObj.Value.Trim() != string.Empty))
                {
                    mappedVInputValue = mappedVInputObj.Value;
                }
            }

            //if (string.IsNullOrEmpty(mappedVInputValue) && serviceQuestionManager.IsDefaultTokenPicked(questMap.PickValueFrom))
            //{
            //    mappedVInputValue = uHelper.GetDefaultTokenValue(_context, questMap.PickValueFrom, false, service);
            //}

            // specail case for userfield variable
            List<ServiceQuestion> questions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
            string strvalue = questMap.PickValueFrom.ToLower().Replace("[$", string.Empty).Replace("$]", string.Empty);
            bool IsDefaultTokenPickedUserField = false;
            foreach (ServiceQuestion item in questions)
            {
                if ((item.TokenName + "~manager").ToLower() == strvalue.ToLower())
                {
                    IsDefaultTokenPickedUserField = true;
                    break;
                }

            }

            if (string.IsNullOrEmpty(mappedVInputValue))
            {

                if (serviceQuestionManager.IsDefaultTokenPicked(questMap.PickValueFrom))
                    mappedVInputValue = uHelper.GetDefaultTokenValue(_context, questMap.PickValueFrom, false);
                else if (IsDefaultTokenPickedUserField)
                {

                    string valToken = questMap.PickValueFrom;
                    if (questMap.PickValueFrom.StartsWith("[$"))
                    {
                        valToken = questMap.PickValueFrom.Replace("[$", string.Empty).Replace("$]", string.Empty);
                    }

                    //if (currentUser == null)
                    //    currentUser = web.CurrentUser;

                    ServiceQuestion sQuestion = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD && x.TokenName.ToLower() == valToken.Replace("~Manager", "").ToLower()).FirstOrDefault();
                    if (sQuestion != null)
                    {
                        QuestionsDTO qDTO = serviceRequestDTO.Questions.Where(x => x.Token == sQuestion.TokenName).FirstOrDefault();

                        if (qDTO != null)
                        {
                            UserProfileManager profileManager = new UserProfileManager(_context);
                            UserProfile user = profileManager.GetUserById(qDTO.Value);
                            if (user != null)
                            {
                                UserProfile userProfile = profileManager.LoadById(user.Id);
                                if (userProfile != null)
                                {
                                    mappedInputValue = string.Format("{0}", userProfile.ManagerID);
                                }
                            }
                        }
                    }

                }
            }

            //Returns null if no value found
            if (defaultValue == string.Empty && mappedInputValue == string.Empty && mappedVInputValue == string.Empty)
            {
                return null;
            }

            //Pick value and put it in resultedValue
            resultedValue = defaultValue;
            if (mappedInputValue != null && mappedInputValue != string.Empty)
            {
                resultedValue = mappedInputValue;
            }
            else if (mappedVInputValue != null && mappedVInputValue != string.Empty)
            {
                resultedValue = mappedVInputValue;
            }

            if (fieldColumnType == "System.String" || fieldColumnType == "System.DateTime" || fieldColumnType == "Date" || fieldColumnType == "NoteField")
            {
                #region string/Datetime mapping
                string txtString = defaultValue;

                MatchCollection matchedTokens = Regex.Matches(txtString, "\\[\\$(.+?)\\$\\]", RegexOptions.IgnoreCase);
                foreach (Match token in matchedTokens)
                {
                    // This should not happen unless there is a mapping error, but check to prevent crashes
                    if (string.IsNullOrEmpty(txtString))
                        break; // Nothing more to parse

                    if (serviceQuestionManager.IsDefaultTokenPicked(token.ToString()))
                    {
                        //if default token exist then fetch default value
                        txtString = txtString.Replace(token.ToString(), uHelper.GetDefaultTokenValue(_context, token.ToString(), true, service));
                    }
                    else
                    {
                        string tokenVal = token.ToString().Replace("[$", string.Empty).Replace("$]", string.Empty);
                        string subToken = string.Empty;
                        string[] tokenSplit = tokenVal.Split(new char[] { '|' });
                        if (tokenSplit.Length > 1)
                        {
                            tokenVal = tokenSplit[0];
                            subToken = tokenSplit[1];
                        }

                        ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(y => y.TokenName.ToLower() == tokenVal.ToLower());
                        QuestionMapVariable tokenVariable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == tokenVal.ToLower());

                        QuestionsDTO tokenQuestInput = null;
                        if (serviceRequestDTO.Questions != null && serviceRequestDTO.Questions.Count() > 0)
                        {
                            tokenQuestInput = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == tokenVal.ToLower());
                        }

                        if (tokenQuestInput != null)
                        {
                            if (tokenQuestion != null)
                            {
                                if (!string.IsNullOrWhiteSpace(subToken))
                                {
                                    txtString = txtString.Replace(token.ToString(), GetSubTokenValue(token.ToString(), tokenQuestion, tokenQuestInput));
                                }
                                else
                                    txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(tokenQuestion.QuestionType, tokenQuestInput.Value, tokenQuestion));
                            }
                            else if (tokenVariable != null)
                            {
                                txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(tokenVariable.Type, tokenQuestInput.Value));
                            }
                        }
                        else if (tokenVal.ToLower() == "current")
                        {
                            if (serviceQuestionManager.IsDefaultTokenPicked(questMap.PickValueFrom))
                            {
                                txtString = txtString.Replace(token.ToString(), uHelper.GetDefaultTokenValue(_context, questMap.PickValueFrom, true, service));
                            }
                            else if (mappedQuestion != null && mappedQInputObj != null)
                            {
                                txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(mappedQuestion, mappedQInputObj.Value));
                            }
                            else if (tokenQuestion == null && mappedQInputObj == null)
                            {
                                QuestionMapVariable qVariable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == questMap.PickValueFrom.ToLower());
                                if (qVariable != null)
                                {
                                    tokenQuestInput = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == qVariable.ShortName.ToLower());
                                    if (tokenQuestInput != null)
                                    {
                                        txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(qVariable.Type, tokenQuestInput.Value));
                                    }
                                    else
                                    {
                                        txtString = txtString.Replace(token.ToString(), string.Empty);
                                    }
                                }
                                else
                                {
                                    // If we have [$Current$] token but nothing to map it to, then make value empty!
                                    txtString = null;
                                }
                            }
                        }
                        else
                        {
                            txtString = txtString.Replace(token.ToString(), string.Empty);
                        }
                    }
                }

                resultedValue = txtString;

                //Evaluate functions
                resultedValue = ExpressionCalc.ExecuteFunctions(_context, resultedValue);

                if (fieldColumnType == "System.DateTime" || fieldColumnType == "Date")
                {
                    DateTime date = DateTime.MinValue;
                    DateTime.TryParse(Convert.ToString(resultedValue), out date);
                    if (date == DateTime.MinValue)
                    {
                        resultedValue = string.Empty;
                    }
                    else
                    {
                        date = DateTime.Now;
                    }
                }

                value = resultedValue;
                #endregion
            }
            else if (fieldColumnType == "Lookup")
            {
                #region lookup mapping
                string lookupModuleName = string.Empty;

                if (mappedQuestion != null)
                {
                    mappedQuestion.QuestionTypePropertiesDicObj.TryGetValue("module", out lookupModuleName);
                    //If lookup question is not mapped with right module ticket then dont save in module ticket
                    if (!string.IsNullOrEmpty(lookupModuleName) && lookupModuleName != moduleName)
                    {
                        resultedValue = defaultValue;
                    }
                }



                value = resultedValue;
                //Skip block if field name is ticketrequesttypelookup because requesttype is a speration type of question in service.
                if (field.ColumnName != DatabaseObjects.Columns.TicketRequestTypeLookup)
                {
                    //SPFieldLookup lookupF = (SPFieldLookup)field;
                    //SPList lookupList = SPListHelper.GetSPList(new Guid(lookupF.LookupList));
                    //SPField lookupPField = lookupList.Fields.GetField(lookupF.LookupField);
                    //SPQuery query = new SPQuery();
                    //List<string> queryExpList = new List<string>();


                    //if (lookupList.Fields.ContainsField(DatabaseObjects.Columns.ModuleNameLookup))
                    //{
                    //    queryExpList.Add(string.Format("<Eq><FieldRef Name='{0}' Lookup='TRUE'/><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
                    //}
                    //if (resultedValue.Contains(Constants.Separator))
                    //{
                    //    SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection(resultedValue);
                    //    if (lookups != null && lookups.Count > 0)
                    //    {
                    //        List<string> newQueryList = new List<string>();
                    //        if (field.ColumnName == DatabaseObjects.Columns.AssetLookup)
                    //        {
                    //            foreach (SPFieldLookupValue lookup in lookups)
                    //            {
                    //                newQueryList.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='{2}'>{1}</Value></Eq>", DatabaseObjects.Columns.Id, lookup.LookupId, lookupPField.Type));
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (SPFieldLookupValue lookup in lookups)
                    //            {
                    //                newQueryList.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='{2}'>{1}</Value></Eq>", lookupPField.InternalName, lookup.LookupValue, lookupPField.Type));
                    //            }
                    //        }

                    //        string newquery = uHelper.GenerateWhereQueryWithAndOr(newQueryList, false);

                    //        queryExpList.Add(newquery);
                    //    }

                    //}
                    //else
                    //{
                    //    queryExpList.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='{2}'>{1}</Value></Eq>", lookupPField.InternalName, resultedValue, lookupPField.Type));
                    //}

                    //query.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryExpList, queryExpList.Count - 1, true));
                    //SPListItemCollection collection = lookupList.GetItems(query);
                    //if (collection.Count > 0)
                    //{
                    //    List<string> vals = new List<string>();
                    //    foreach (SPListItem item in collection)
                    //    {
                    //        vals.Add(string.Format("{0}{1}{2}", UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.Id), Constants.Separator, uHelper.GetSPItemValue(item, lookupPField.StaticName)));
                    //    }
                    //    value = string.Join(Constants.Separator, vals.ToArray());
                    //}
                    //else
                    //{
                    //    value = null;
                    //}
                }
                #endregion
            }
            else if (fieldColumnType == "Choices")
            {
                #region choices
                string lVal = string.Empty;
                if (fieldColumnType == "Choices")//field.Type == SPFieldType.MultiChoice)
                {
                    //SPFieldMultiChoice choiceField = (SPFieldMultiChoice)field;
                    lVal = resultedValue;

                    //Exclude issue type from checking valid choice
                    //for issue type, saving other values based on request type.
                    if (field.ColumnName != DatabaseObjects.Columns.UGITIssueType)
                    {
                        string[] rValues = UGITUtility.SplitString(resultedValue, Constants.Separator5, StringSplitOptions.RemoveEmptyEntries);
                        if (rValues != null)
                        {
                            List<string> validValues = new List<string>();
                            foreach (string val in rValues)
                            {
                                if (configField.Data.Contains(val))
                                {
                                    validValues.Add(val);
                                }
                            }
                            lVal = string.Join(Constants.Separator, validValues);
                        }
                    }
                }

                value = lVal;
                #endregion
            }
            else if (fieldColumnType == "System.Int32" || fieldColumnType == "System.Int64" || fieldColumnType == "Currency")
            {
                double num = 0;
                double.TryParse(resultedValue, out num);
                value = num.ToString();
            }
            else if (fieldColumnType == "System.Boolean")
            {
                value = "0";
                if (UGITUtility.StringToBoolean(resultedValue))
                {
                    value = "1";
                }
            }
            else if (fieldColumnType == "UserType" || fieldColumnType == "UserField")
            {
                #region UserType
                // SPFieldUserValueCollection userFromTokens = new SPFieldUserValueCollection();
                List<UserProfile> userFromTokens = _context.UserManager.GetUserInfosById(resultedValue);
                string txtString = defaultValue;
                MatchCollection matchedTokens = Regex.Matches(txtString, "\\[\\$(.+?)\\$\\]", RegexOptions.IgnoreCase);
                if (matchedTokens.Count > 0)
                {
                    foreach (Match token in matchedTokens)
                    {
                        string tokenVal = token.ToString().Replace("[$", string.Empty).Replace("$]", string.Empty);
                        if (tokenVal.ToLower() == "current")
                        {
                            if (mappedQuestion != null)
                            {
                                tokenVal = mappedQuestion.TokenName;
                            }
                            else
                            {
                                tokenVal = questMap.PickValueFrom;
                            }
                        }
                        tokenVal = GetValueFromToken(tokenVal, serviceRequestDTO);
                        if (!string.IsNullOrWhiteSpace(tokenVal))
                        {
                            //SPFieldUserValueCollection userLookup = new SPFieldUserValueCollection(web, tokenVal);
                            //if (userLookup.Count != 0)
                            //{
                            //    SPFieldUser user = (SPFieldUser)field;
                            //    if (user.AllowMultipleValues)
                            //        userFromTokens.AddRange(userLookup);
                            //    else
                            //        userFromTokens.Add(userLookup[0]);
                            //}
                        }
                    }
                    value = string.Join<string>(",", userFromTokens.Select(x => x.Id));
                }
                else
                {
                    //SPFieldUserValueCollection userValCollection = new SPFieldUserValueCollection(web, resultedValue);
                    List<UserProfile> lstUserProfile = _context.UserManager.GetUserInfosById(resultedValue);
                    if (lstUserProfile.Count > 0)
                    {
                        //SPFieldUser user = (SPFieldUser)field;
                        //if (user.AllowMultipleValues)
                        //    value = userValCollection;
                        //else
                        value = lstUserProfile[0].Id;
                    }
                    else
                    {
                        string userid = resultedValue;
                        if (userid != null)
                        {
                            UserProfile user = _context.UserManager.GetUserById(userid);  //UserProfile.LoadById(userid, web);
                            if (user != null)
                                value = string.Format("{0}{1}{2}", user.Id, Constants.Separator, user.Name);
                            else
                                value = string.Format("{0}{1}", userid, Constants.Separator);
                        }
                    }
                }
                #endregion
            }
            return value;
        }

        private string GetValueFromToken(string token, ServiceRequestDTO serviceRequestDTO)
        {
            string tokenVal = token;
            ServiceQuestion tokenQuestion = null;
            QuestionMapVariable tokenVariable = null;
            QuestionsDTO tokenQuestInput = null;
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);
            if (serviceQuestionManager.IsDefaultTokenPicked(tokenVal.ToString()))
            {
                //if default token exist then fetch default value
                tokenQuestion = serviceQuestionManager.GetBuiltInQuestions().FirstOrDefault(x => x.TokenName == tokenVal.ToString());
                if (tokenQuestion != null && tokenQuestion.QuestionType == Constants.ServiceQuestionType.USERFIELD)
                {
                    tokenVal = uHelper.GetDefaultTokenValue(_context, tokenVal.ToString(), false, service);
                }
            }
            else
            {
                tokenQuestion = service.Questions.FirstOrDefault(y => y.TokenName.ToLower() == tokenVal.ToLower());
                tokenVariable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == tokenVal.ToLower());


                if (serviceRequestDTO.Questions != null && serviceRequestDTO.Questions.Count() > 0)
                {
                    tokenQuestInput = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == tokenVal.ToLower());
                }

                if (tokenQuestInput != null)
                {
                    if ((tokenQuestion != null && tokenQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD) ||
                        (tokenVariable != null && tokenVariable.Type == Constants.ServiceQuestionType.USERFIELD))
                    {
                        tokenVal = tokenQuestInput.Value;
                    }
                }
            }

            if (tokenVal == token)
                tokenVal = string.Empty;

            return tokenVal;
        }

        // Used to support sub-token values. Examples:
        //  [$RequestType|Category$],[$RequestType|SubCategory$]
        //  [$Requestor|Manager$],[$Requestor|Email$],[$Requestor|MobilePhone$],[$Requestor|Skills$],[$Requestor|FunctionalArea$],[$Requestor|JobProfile$]
        private string GetSubTokenValue(string token, ServiceQuestion serviceQuestion, QuestionsDTO questionVal)
        {
            string value = string.Empty;
            token = token.Replace("[$", string.Empty).Replace("$]", string.Empty);
            string[] tokenSplit = token.Split(new char[] { '|' });
            string mainToken = tokenSplit.First();
            string subToken = string.Empty;
            if (tokenSplit.Length > 1)
                subToken = tokenSplit[1];

            if (serviceQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
            {
                // SPFieldLookupValue lookup = new SPFieldLookupValue(questionVal.Value);
                if (!string.IsNullOrWhiteSpace(questionVal.Value))
                {
                    string moduleName = string.Empty;
                    serviceQuestion.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);
                    UGITModule module = ModuleManager.LoadByName(moduleName);
                    ModuleRequestType requestType = module.List_RequestTypes.FirstOrDefault(x => x.ID == Convert.ToInt32(questionVal.Value));
                    if (requestType != null)
                    {
                        switch (subToken.ToLower())
                        {
                            case "category":
                                value = requestType.Category;
                                break;
                            case "subcategory":
                                value = requestType.SubCategory;
                                break;
                            case "issuetype":
                                {
                                    if (questionVal.SubTokensValue != null && questionVal.SubTokensValue.Count > 0)
                                    {
                                        QuestionsDTO issueTypeVal = questionVal.SubTokensValue.FirstOrDefault(x => x.Token.ToLower() == "issuetype");
                                        if (issueTypeVal != null)
                                            value = string.Join("; ", UGITUtility.SplitString(issueTypeVal.Value, new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries));
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else if (serviceQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
            {
                // SPFieldUserValue lookup = new SPFieldUserValue(spWeb, questionVal.Value);
                UserProfile uProfile = _context.UserManager.GetUserById(questionVal.Value); ; //UserProfile.LoadById(lookup.LookupId, spWeb);
                if (uProfile != null)
                {
                    System.Reflection.PropertyInfo info = uProfile.GetType().GetProperty(subToken);
                    if (info != null)
                    {
                        object propertyValue = info.GetValue(uProfile);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(propertyValue)))
                        {
                            if (propertyValue is UserLookupValue)
                                value = (propertyValue as UserLookupValue).Name;
                            else if (propertyValue is LookupValue)
                                value = (propertyValue as LookupValue).Value;
                            else if (propertyValue is List<LookupValue>)
                                value = string.Join(", ", (propertyValue as List<LookupValue>).Select(x => x.Value));
                            else if (propertyValue is DateTime)
                                value = UGITUtility.GetDateStringInFormat((DateTime)propertyValue, false);
                            else
                                value = Convert.ToString(info.GetValue(uProfile));
                        }
                    }
                }
            }
            else if (serviceQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
            {
                ModuleViewManager moduleManager = new ModuleViewManager(_context);
                //SPFieldLookupValueCollection lookup = new SPFieldLookupValueCollection(questionVal.Value);
                if (!string.IsNullOrEmpty(questionVal.Value) && subToken.ToLower() == "detail")
                {
                    List<int> lstAssetsIds = questionVal.Value.Split(',').Select(int.Parse).ToList(); // lookup.AsEnumerable().Select(x => x.LookupId).ToList();
                    UGITModule cmdbModuleConfig = ModuleManager.GetByName("CMDB");  // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, "CMDB");
                    if (cmdbModuleConfig == null)
                        return value;

                    TicketManager ticketManager = new TicketManager(_context);
                    DataTable assetsTables = ticketManager.GetOpenTickets(cmdbModuleConfig);   // uGITCache.ModuleDataCache.GetOpenTickets(cmdbModuleConfig.ID, spWeb);
                    if (assetsTables == null || assetsTables.Rows.Count == 0)
                        return value;

                    List<string> lstSelectedTags = new List<string>();
                    DataRow[] drColl = assetsTables.Select(string.Format("{0} IN ({1})", DatabaseObjects.Columns.ID, string.Join(",", lstAssetsIds)));
                    if (drColl == null || drColl.Length == 0)
                        return value;


                    List<ModuleColumn> columns = cmdbModuleConfig.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
                    List<Tuple<string, string>> lstSchemaColumns = new List<Tuple<string, string>>();
                    foreach (ModuleColumn moduleColumn in columns)
                    {
                        string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                        string fieldDisplayName = Convert.ToString(moduleColumn.FieldDisplayName);
                        if (fieldColumn != DatabaseObjects.Columns.Attachments)
                        {
                            lstSchemaColumns.Add(new Tuple<string, string>(moduleColumn.FieldDisplayName, moduleColumn.FieldName));
                        }
                    }

                    //get asset detail
                    StringBuilder builder = new StringBuilder();
                    foreach (DataRow dr in drColl)
                    {
                        List<string> assetsDetail = new List<string>();
                        foreach (Tuple<string, string> tup in lstSchemaColumns)
                        {
                            if (UGITUtility.IfColumnExists(tup.Item2, assetsTables) && Convert.ToString(dr[tup.Item2]) != string.Empty)
                                assetsDetail.Add(string.Format("{0}: {1}", tup.Item1, dr[tup.Item2]));
                        }
                        if (assetsDetail.Count > 0)
                            lstSelectedTags.Add(string.Format("[{0}]", string.Join(", ", assetsDetail)));
                    }

                    value = string.Join("&#13;&#10;", lstSelectedTags);
                }
            }

            return value;
        }

        /// <summary>
        /// This method creates service response based on created services,task and tickets if parent service exists.
        /// </summary>
        /// <param name="serviceTasks"></param>
        /// <param name="serviceTicket"></param>
        /// <returns></returns>
        private ServiceResponseTreeNodeParent GenerateTaskSummary(List<UGITTask> serviceTasks, DataRow serviceTicket)
        {
            ServiceResponseTreeNodeParent serviceTicketNode = new ServiceResponseTreeNodeParent();
            serviceTicketNode.TicketId = Convert.ToString(serviceTicket[DatabaseObjects.Columns.TicketId]);
            serviceTicketNode.ID = Convert.ToInt32(serviceTicket[DatabaseObjects.Columns.Id]);
            serviceTicketNode.MName = "SVC";
            serviceTicketNode.Text = Convert.ToString(serviceTicket[DatabaseObjects.Columns.Title]);

            List<ServiceResponseTreeNode> lstServiceResponse = new List<ServiceResponseTreeNode>();
            foreach (UGITTask task in serviceTasks)
            {
                ServiceResponseTreeNode taskNode = new ServiceResponseTreeNode();
                if (string.IsNullOrEmpty(task.ChildInstance))
                {
                    // sub-task
                    taskNode.Text = task.Title;
                    taskNode.Type = 0;
                    taskNode.MName = string.Empty;
                    taskNode.ID = task.ID;
                }
                else
                {
                    // sub-ticket
                    taskNode.Text = task.Title;
                    taskNode.Type = 1;
                    taskNode.MName = uHelper.getModuleNameByTicketId(task.ChildInstance);  //Convert.ToString(uGITCache.ModuleConfigCache.getModuleName(task.ModulenName, serviceTicket)[DatabaseObjects.Columns.ModuleName]);
                    taskNode.ID = task.ID;
                    taskNode.TicketID = task.ChildInstance;
                }

                lstServiceResponse.Add(taskNode);
            }

            serviceTicketNode.ServiceResponseTreeNode = lstServiceResponse;
            return serviceTicketNode;
        }

        /// <summary>
        /// This method creates service response based on created ticket if parent service does not exists.
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private ServiceResponseTreeNodeParent GenerateTaskSummary(Dictionary<long, TaskTempInfo> tickets)
        {
            ServiceResponseTreeNodeParent serviceTicketNode = new ServiceResponseTreeNodeParent();
            List<ServiceResponseTreeNode> lstServiceResponse = new List<ServiceResponseTreeNode>();

            foreach (int index in tickets.Keys)
            {
                ServiceResponseTreeNode taskNode = new ServiceResponseTreeNode();
                taskNode.Text = tickets[index].Title;
                taskNode.Type = 1;
                taskNode.TicketID = tickets[index].TicketID;

                if (tickets[index].ModuleID > 0)
                    taskNode.MName = ModuleManager.LoadByID(Convert.ToInt64(tickets[index].ModuleID)).ModuleName;//  Convert.ToString(uGITCache.GetModuleDetails(tickets[index].ModuleID)[DatabaseObjects.Columns.ModuleName]);

                taskNode.ID = tickets[index].TaskID;
                lstServiceResponse.Add(taskNode);
            }

            serviceTicketNode.ServiceResponseTreeNode = lstServiceResponse;
            return serviceTicketNode;
        }

        private void AppendVariableData(Services service, ref ServiceRequestDTO serviceRequestDTO)
        {
            if (service.QMapVariables == null || service.QMapVariables.Count <= 0)
            {
                return;
            }

            foreach (QuestionMapVariable variable in service.QMapVariables)
            {
                QuestionsDTO question = new QuestionsDTO();
                question.Token = variable.ShortName;
                question.Value = GetVariableData(variable, serviceRequestDTO);
                serviceRequestDTO.Questions.Add(question);
            }
        }

        private string GetVariableData(QuestionMapVariable variable, ServiceRequestDTO serviceRequestDTO)
        {
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);
            //returns if no condition found in variable
            if ((variable.VariableValues == null || variable.VariableValues.Count <= 0) && variable.DefaultValue == null)
            {
                return string.Empty;
            }

            string value = string.Empty;
            bool conditionMet = false;
            if (variable.VariableValues != null)
            {
                string conditionToken = string.Empty;
                foreach (VariableValue variableVal in variable.VariableValues)
                {
                    if (variable.VariableValues[0].Conditions == null || variable.VariableValues[0].Conditions.Count <= 0)
                        continue;

                    #region MyRegion
                    bool isQuestionNotFound = false;
                    List<WhereExpression> whrExpressions = variableVal.Conditions;

                    // Initialize itemId for each where expression if any Id is set to zero in any whereExpression
                    if (whrExpressions.Any(x => x.Id == 0))
                    {
                        int itemIndex = 0;
                        foreach (WhereExpression wExpression in whrExpressions)
                        {
                            itemIndex++;
                            wExpression.Id = itemIndex;
                        }
                    }

                    StringBuilder expression = new StringBuilder();
                    List<WhereExpression> rootWhere = whrExpressions.Where(x => x.ParentId == 0).ToList();

                    foreach (WhereExpression rWhere in rootWhere)
                    {
                        if (!string.IsNullOrEmpty(rWhere.LogicalRelOperator) && rWhere.LogicalRelOperator.ToLower() != "none")
                            expression.Append(rWhere.LogicalRelOperator + " ");

                        List<WhereExpression> subWhere = new List<WhereExpression>();
                        WhereExpression rWhereCopy = rWhere;
                        subWhere.Add(rWhereCopy);
                        subWhere.AddRange(whrExpressions.Where(x => x.ParentId == rWhere.Id));

                        List<string> expList = new List<string>();
                        for (int i = 0; i < subWhere.Count; i++)
                        {
                            StringBuilder subQuery = new StringBuilder();
                            WhereExpression where = subWhere[i];

                            if (expList.Count > 0 && !string.IsNullOrEmpty(where.LogicalRelOperator) && where.LogicalRelOperator.ToLower() != "none")
                                subQuery.AppendFormat(where.LogicalRelOperator + " ");

                            #region calculation to find whether the input value meets the skip logic for current question only
                            //Gets question token and if it is not come with [$$] format then remove it into token
                            conditionToken = where.Variable;
                            if (conditionToken.IndexOf("[$") > 0)
                            {
                                conditionToken = conditionToken.Replace("[$", string.Empty).Replace("$]", string.Empty);
                            }

                            //Discard condition if condition's question is not found against token
                            ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == conditionToken.ToLower());
                            if (tokenQuestion == null)
                            {
                                isQuestionNotFound = true;
                                break;
                            }

                            //Discard condition if input is not ask yet
                            string lsValue = string.Empty;
                            var tokenQuestionInput = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == conditionToken.ToLower());
                            if (tokenQuestionInput != null)
                                lsValue = tokenQuestionInput.Value;

                            bool result = serviceQuestionManager.TestCondition(lsValue, where.Operator, where.Value, tokenQuestion.QuestionType);

                            #endregion calculation to find whether the input value meets the skip logic for current question only

                            subQuery.Append(result);
                            expList.Add(subQuery.ToString());
                        }

                        if (expList.Count == 1)
                            expression.AppendFormat("{0} ", string.Join(" ", expList));
                        else if (expList.Count > 1)
                            expression.AppendFormat("({0}) ", string.Join(" ", expList));

                        if (isQuestionNotFound)
                            break;
                    }

                    string finalExpression = (expression.ToString()).Trim().ToLower();

                    bool finalResult = EvaluateStringToBoolean(finalExpression);

                    if (isQuestionNotFound)
                        continue;

                    if (finalResult)
                    {
                        conditionMet = true;
                        if (variableVal.IsPickFromConstant && variable.Type.ToLower() == Constants.ServiceQuestionType.TEXTBOX)
                        {
                            value = ReplaceTokens(variableVal.PickFrom, serviceRequestDTO);
                        }
                        else
                        {
                            string token = variableVal.PickFrom;
                            if (token.IndexOf(Constants.TokenStart) == -1)
                            {
                                token = string.Format("{1}{0}{2}", token, Constants.TokenStart, Constants.TokenEnd);
                            }

                            QuestionsDTO questionVal1 = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == variableVal.PickFrom);
                            value = string.Empty;
                            if (questionVal1 != null)
                            {
                                value = questionVal1.Value;
                                ServiceQuestion question = service.Questions.FirstOrDefault(z => z.TokenName == variableVal.PickFrom);

                                if (variable.Type == Constants.ServiceQuestionType.TEXTBOX && question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                                {
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        UserProfile user = _context.UserManager.GetUserById(value);
                                        if (user != null)
                                        {
                                            value = user.Name;
                                        }
                                    }
                                }

                            }
                            else if (serviceQuestionManager.IsDefaultTokenPicked(token))
                            {
                                value = uHelper.GetDefaultTokenValue(_context, token, false, service);
                            }
                            else
                            {
                                value = variableVal.PickFrom;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region  old code
                    //WhereExpression whereExp = variableVal.Conditions[0];
                    //ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == whereExp.Variable);
                    //if (sQuestion == null)
                    //{
                    //    return string.Empty;
                    //}

                    //QuestionsDTO questionVal = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == whereExp.Variable);
                    //if (questionVal != null)
                    //{
                    //    value = questionVal.Value;
                    //}

                    //if (serviceQuestionManager.TestCondition(value, whereExp.Operator, whereExp.Value, sQuestion.QuestionType))
                    //{
                    //    conditionMet = true;
                    //    if (variableVal.IsPickFromConstant && variable.Type.ToLower() == Constants.ServiceQuestionType.TEXTBOX)
                    //    {
                    //        value = ReplaceTokens(variableVal.PickFrom, serviceRequestDTO);
                    //    }
                    //    else
                    //    {
                    //        string token = variableVal.PickFrom;
                    //        if (token.IndexOf(Constants.TokenStart) == -1)
                    //        {
                    //            token = string.Format("{1}{0}{2}", token, Constants.TokenStart, Constants.TokenEnd);
                    //        }

                    //        QuestionsDTO questionVal1 = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == variableVal.PickFrom);
                    //        value = string.Empty;
                    //        if (questionVal1 != null)
                    //        {
                    //            value = questionVal1.Value;
                    //            ServiceQuestion question = service.Questions.FirstOrDefault(z => z.TokenName == variableVal.PickFrom);

                    //            if (variable.Type == Constants.ServiceQuestionType.TEXTBOX && question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                    //            {
                    //                //SPFieldUserValue userLookup = new SPFieldUserValue(SPContext.Current.Web, value);
                    //                //if (userLookup != null && userLookup.LookupId > 0)
                    //                //{
                    //                //    UserProfile user = UserProfile.LoadById(userLookup.LookupId);
                    //                //    if (user != null)
                    //                //    {
                    //                //        value = user.Name;
                    //                //    }
                    //                //}
                    //            }

                    //        }
                    //        else if (serviceQuestionManager.IsDefaultTokenPicked(token))
                    //        {
                    //            value = uHelper.GetDefaultTokenValue(_context, token, false, service);
                    //        }
                    //        else
                    //        {
                    //            value = variableVal.PickFrom;
                    //        }
                    //    }
                    //    break;
                    //}
                    #endregion

                }
            }

            //test condition then return value based on match condition otherwise return default 
            if (!conditionMet)
            {
                if (variable.DefaultValue.IsPickFromConstant && variable.Type.ToLower() == Constants.ServiceQuestionType.TEXTBOX)
                {
                    value = ReplaceTokens(variable.DefaultValue.PickFrom, serviceRequestDTO);
                }
                else
                {
                    string token = variable.DefaultValue.PickFrom;
                    if (token.IndexOf(Constants.TokenStart) == -1)
                    {
                        token = string.Format("{1}{0}{2}", token, Constants.TokenStart, Constants.TokenEnd);
                    }
                    QuestionsDTO questionVal1 = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token == variable.DefaultValue.PickFrom);
                    value = string.Empty;
                    if (questionVal1 != null)
                    {
                        value = questionVal1.Value;
                    }
                    else if (serviceQuestionManager.IsDefaultTokenPicked(token))
                    {
                        value = uHelper.GetDefaultTokenValue(_context, token, false, service);
                    }
                    else
                    {
                        value = variable.DefaultValue.PickFrom;
                    }
                }
            }

            //Checks valid value for question types
            if (variable.Type.ToLower() == Constants.ServiceQuestionType.DATETIME)
            {
                DateTime time = DateTime.MinValue;
                DateTime.TryParse(value, out time);
                if (time == DateTime.MinValue)
                    value = null;

            }
            else if (variable.Type.ToLower() == Constants.ServiceQuestionType.USERFIELD)
            {
                try
                {
                    // SPFieldUserValue users = new SPFieldUserValue(SPContext.Current.Web, value);
                }
                catch
                {
                    value = null;
                }
            }
            else if (variable.Type.ToLower() == Constants.ServiceQuestionType.Number)
            {
                double number = 0;
                if (!double.TryParse(value, out number))
                {
                    value = "0";
                }
            }

            return value;
        }

        private string ReplaceTokens(string txtString, ServiceRequestDTO serviceRequestDTO)
        {
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);

            MatchCollection matchedTokens = Regex.Matches(txtString, "\\[\\$(.+?)\\$\\]", RegexOptions.IgnoreCase);
            foreach (Match token in matchedTokens)
            {
                // This should not happen unless there is a mapping error, but check to prevent crashes
                if (string.IsNullOrEmpty(txtString))
                    break; // Nothing more to parse

                if (serviceQuestionManager.IsDefaultTokenPicked(token.ToString()))
                {
                    //if default token exist then fetch default value
                    txtString = txtString.Replace(token.ToString(), uHelper.GetDefaultTokenValue(_context, token.ToString(), true, service));
                }
                else
                {
                    string tokenVal = token.ToString().Replace("[$", string.Empty).Replace("$]", string.Empty);
                    ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(y => y.TokenName.ToLower() == tokenVal.ToLower());
                    QuestionMapVariable tokenVariable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == tokenVal.ToLower());

                    QuestionsDTO tokenQuestInput = null;
                    if (serviceRequestDTO.Questions != null && serviceRequestDTO.Questions.Count() > 0)
                    {
                        tokenQuestInput = serviceRequestDTO.Questions.FirstOrDefault(x => x.Token.ToLower() == tokenVal.ToLower());
                    }

                    if (tokenQuestInput != null)
                    {
                        if (tokenQuestion != null)
                        {
                            txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(tokenQuestion, tokenQuestInput.Value));
                        }
                        else if (tokenVariable != null)
                        {
                            txtString = txtString.Replace(token.ToString(), serviceQuestionManager.ParseQuestionVal(tokenVariable.Type, tokenQuestInput.Value));
                        }
                    }
                    else
                    {
                        txtString = txtString.Replace(token.ToString(), string.Empty);
                    }
                }
            }

            return txtString;
        }

        public bool CheckIsRelationChanged(ServiceMatrixData serviceMatrixData)
        {
            bool isNew = false;
            string appID = serviceMatrixData.ID;

            string query = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.APPTitleLookup, serviceMatrixData.ID, DatabaseObjects.Columns.ApplicationRoleAssign, serviceMatrixData.RoleAssignee);
            //  query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ApplicationRoleAssign, serviceMatrixData.RoleAssignee);

            DataRow[] spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(query);
            if ((spListItemColl == null || spListItemColl.Count() == 0) && (serviceMatrixData.lstGridData.Where(x => x.ID == appID) != null && serviceMatrixData.lstGridData.Where(x => x.ID == appID).Count() > 0))
            {
                isNew = true;
            }
            else if ((spListItemColl != null && spListItemColl.Count() > 0) && (serviceMatrixData.lstGridData.Where(x => x.ID == appID) == null || serviceMatrixData.lstGridData.Where(x => x.ID == appID).Count() == 0))
            {
                isNew = true;
            }
            else if (spListItemColl != null && spListItemColl.Count() > 0 && serviceMatrixData.lstGridData.Where(x => x.ID == appID) != null && serviceMatrixData.lstGridData.Where(x => x.ID == appID).Count() > 0)
            {
                foreach (DataRow item in spListItemColl)
                {
                    string spFieldLookupValueAppTitle = (Convert.ToString(item[DatabaseObjects.Columns.APPTitleLookup]));
                    string spFieldLookupValueModules = (Convert.ToString(item[DatabaseObjects.Columns.ApplicationModulesLookup]));
                    string spFieldLookupValueRoles = (Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleLookup]));
                    ServiceData serviceData = serviceMatrixData.lstGridData.Where(c => c.RowName == Convert.ToString(spFieldLookupValueRoles) && c.ColumnName == Convert.ToString(spFieldLookupValueModules != null ? spFieldLookupValueModules : spFieldLookupValueAppTitle)).FirstOrDefault();
                    if (serviceData == null)
                    {
                        isNew = true;
                        break;
                    }
                }

                if (!isNew)
                {
                    foreach (ServiceData serviceData in serviceMatrixData.lstGridData.Where(x => x.ID == appID))
                    {
                        int count = serviceMatrixData.lstGridData.Where(x => x.ID == appID).Count();
                        DataRow[] dr = spListItemColl.CopyToDataTable().Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.ApplicationRoleLookup, serviceData.RowName.Replace("'", "''"), DatabaseObjects.Columns.ApplicationModulesLookup, serviceData.ColumnName.Replace("'", "''")));

                        if (dr.Length == 0)
                        {
                            dr = spListItemColl.CopyToDataTable().Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.ApplicationRoleLookup, serviceData.RowName.Replace("'", "''"), DatabaseObjects.Columns.APPTitleLookup, serviceData.ColumnName.Replace("'", "''")));
                            if (dr.Length == 0)
                            {
                                isNew = true;
                                break;
                            }
                        }
                    }
                }
            }
            return isNew;
        }

        private bool EvaluateStringToBoolean(string inputExpression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("", typeof(Boolean));
            table.Columns[0].Expression = inputExpression;

            DataRow row = table.NewRow();
            table.Rows.Add(row);
            bool boolResult = UGITUtility.StringToBoolean(row[0]);

            return boolResult;
        }
    }
}
