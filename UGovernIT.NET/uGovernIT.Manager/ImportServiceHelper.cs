using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class ImportServiceHelper
    {


     
        ServiceExtension serviceError;
        ServicesManager objServicesManager;
        ServiceQuestionMappingManager objServiceQuestionMappingManager;
        ServiceSectionManager objServiceSectionManager;
        ServiceQuestionManager objServiceQuestionManager;
        List<Services> lstServices = new List<Services>();

        ApplicationContext _context = null;

        public ImportServiceHelper(ApplicationContext context)
        {
          
            serviceError = null;
            this._context = context;
        }


        public void UpdateServices()
        {
            ApplicationContext applicationContext = ApplicationContext.Create();
            TenantManager tenantManager = new TenantManager(applicationContext);
            List<Tenant> lstTenants = tenantManager.GetTenantList();

            foreach (Tenant tenant in lstTenants)
            {
                if (tenant.TenantID.ToString() != applicationContext.TenantID)
                {
                    InsertUpdatedServiceToTenant(tenant.TenantID.ToString());
                }
                else
                {
                }
            }
        }


        public void InsertUpdatedServiceToTenant(string tenantID)
        {
            ServiceUpdates_Master serviceUpdates_Master = new ServiceUpdates_Master();
            UpdateServicesMasterManager updateServicesMasterManager = new UpdateServicesMasterManager(_context);
            List<ServiceUpdates_Master> lstServiceUpdates_Masters = updateServicesMasterManager.Load(x => x.AvailableForUpdate == true).ToList();

            ServiceExtension serviceError;
            ServiceExtension service = new ServiceExtension();


            foreach (ServiceUpdates_Master serviceUpdates_Masters in lstServiceUpdates_Masters)
            {
                using (Stream stream = new MemoryStream())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(serviceUpdates_Masters.ServiceInfo);
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;
                    DataContractSerializer deserializer = new DataContractSerializer(typeof(ServiceExtension));
                    service = (ServiceExtension)deserializer.ReadObject(stream);
                    serviceError = new ServiceExtension();
                    service.IsActivated = true;
                    _context = ApplicationContext.CreateContext(tenantID);
                    objServicesManager = new ServicesManager(_context);
                    List<Services> existingService = objServicesManager.Load(x => x.Title.Equals(service.Title, StringComparison.InvariantCultureIgnoreCase));
                    if (existingService != null && existingService.Count > 0)
                    {
                        foreach (Services serv in existingService)
                        {
                            objServicesManager.Archive(serv.ID);
                        }
                        
                    }
                    ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(_context);
                    if (!string.IsNullOrEmpty(service.ServiceCategoryType))
                    {
                        List<ServiceCategory> allServiceCategories = objServiceCategoryManager.Load();
                        ServiceCategory category = allServiceCategories.FirstOrDefault(x => x.CategoryName == service.ServiceCategoryType);
                        if (category == null)
                        {
                            category = new ServiceCategory();
                            category.CategoryName = service.ServiceCategoryType;
                            category.Title = service.ServiceCategoryType;
                            objServiceCategoryManager.Insert(category);
                        }
                        
                        service.ServiceCategoryType = category.CategoryName;
                        service.CategoryId = category.ID;
                        SaveService(service, serviceError);
                    }
                }
            }
        }

        public void LoadService()
        {
            ServiceExtension services;
            ServiceExtension serviceError;
            
            ServiceExtension service = new ServiceExtension();
            //ImportServiceHelper importServiceHelper = new ImportServiceHelper();
            //string text = System.IO.File.ReadAllText(System.IO.Path.Combine(uHelper.GetUploadFolderPath(), "EMSdefaultonboarding.xml"));

            if (!Directory.Exists(Path.Combine(uHelper.GetDefaultServicesPath())))
                return;

            //Get default services for every tenant
            foreach (string Servicefile in Directory.EnumerateFiles(Path.Combine(uHelper.GetDefaultServicesPath()), "*.xml"))
            {
                string servicesContents = File.ReadAllText(Servicefile);

                using (Stream stream = new MemoryStream())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(servicesContents);
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;
                    DataContractSerializer deserializer = new DataContractSerializer(typeof(ServiceExtension));
                    service = (ServiceExtension)deserializer.ReadObject(stream);
                    //service.TenantID = _context.TenantID;

                    // Commented as services are not importing
                    //if (service.IncludeInDefaultData)
                    {
                        //Add Navigation link
                        if (!string.IsNullOrEmpty(service.ServiceCategoryType))
                        {
                            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(_context);
                            List<ServiceCategory> allServiceCategories = serviceCategoryManager.Load();
                            ServiceCategory category = allServiceCategories.FirstOrDefault(x => x.CategoryName == service.ServiceCategoryType);
                            if (category == null)
                            {
                                category = new ServiceCategory();
                                category.CategoryName = service.ServiceCategoryType;
                                category.Title = service.ServiceCategoryType;
                                serviceCategoryManager.Insert(category);
                            }
                            service.ServiceCategoryType = category.CategoryName;
                            service.CategoryId = category.ID;

                        }

                        if ("Employee on-boarding".EqualsIgnoreCase(service.Title))
                        {
                            WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(_context);

                            WikiArticles servicesWikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup}='svc'");

                            if (servicesWikiArticle != null && !string.IsNullOrEmpty(servicesWikiArticle.TicketId))
                            {
                                var NavigationUrl = $"/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={servicesWikiArticle.TicketId}";

                                service.NavigationUrl = NavigationUrl;
                            }
                        }

                        if ("Employee on-boarding".EqualsIgnoreCase(service.Title))
                        {
                            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(_context);
                            string where = $"Title = 'Employee Management'";
                            ServiceCategory serviceCategory = serviceCategoryManager.Load(where).ToList().FirstOrDefault();

                            if (serviceCategory != null)
                            {
                                service.CategoryId = serviceCategory.ID;

                            }
                        }

                        if ("Printer Issue".EqualsIgnoreCase(service.Title) || "Report a Problem".EqualsIgnoreCase(service.Title) || "Password Issue".EqualsIgnoreCase(service.Title))
                        {
                            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(_context);
                            string where = $"Title = 'I Have an Issue'";
                            ServiceCategory serviceCategory = serviceCategoryManager.Load(where).ToList().FirstOrDefault();

                            if (serviceCategory != null)
                            {
                                service.CategoryId = serviceCategory.ID;

                            }
                        }

                        if ("Need Laptop".EqualsIgnoreCase(service.Title))
                        {
                            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(_context);
                            string where = $"Title = 'Purchase'";
                            ServiceCategory serviceCategory = serviceCategoryManager.Load(where).ToList().FirstOrDefault();

                            if (serviceCategory != null)
                            {
                                service.CategoryId = serviceCategory.ID;

                            }
                        }
                        lstServices.Add(service);
                    }
                }


                if (lstServices != null && lstServices.Count > 0)
                {
                    foreach (ServiceExtension svc in lstServices)
                    {
                        services = svc;
                        //services.TenantID = _context.TenantID;
                        serviceError = new ServiceExtension();
                        services.IsActivated = true;
                        //importServiceHelper.SaveService(services, serviceError);
                        SaveService(services, serviceError);
                    }
                    lstServices.Clear();

                }
            }
        }
        
        public void SaveService(ServiceExtension serviceExt, ServiceExtension serviceError)
        {
            Services service = (Services)serviceExt;
            Services newService = service;
            newService.ID = 0;
            
            List<string> owners = new List<string>();
            if ((serviceError == null || serviceError.OwnerUser == null) && serviceExt.OwnerUser != null)
            {
                foreach (string userEntity in UGITUtility.ConvertStringToList(serviceExt.OwnerUser, Constants.Separator6))
                {
                    UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.UserName == userEntity);
                    if (user != null)
                    {
                        if (user.isRole)
                        {
                            UserProfile spgrp = user;
                            if (spgrp != null && spgrp.Name == user.Name)
                            {
                                owners.Add(spgrp.Id);
                            }
                        }
                        else
                        {
                            UserProfile spuser = user;// UserProfile.GetUserById(user.ID);
                            if (spuser != null)
                            {
                                owners.Add(spuser.Id);
                            }
                        }
                    }
                }
            }

            //SPFieldUserValueCollection authToView = new SPFieldUserValueCollection();
            List<string> authToView = new List<string>();
            if ((serviceError == null || serviceError.AuthorizedToView == null) && serviceExt.AuthorizedToView != null)
            {
                foreach (var userEntity in UGITUtility.ConvertStringToList(serviceExt.AuthorizedToView, Constants.Separator6))
                {
                    UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.UserName == userEntity);
                    if (user != null)
                    {
                        if (user.isRole)
                        {
                            UserProfile spgrp = user; //UserProfile.GetGroupByName(user.Name);
                            if (spgrp != null && spgrp.Name == user.Name)
                            {
                                authToView.Add(spgrp.Id);
                            }
                        }
                        else
                        {
                            UserProfile spusr = user; //UserProfile.GetUserById(user.ID);
                            if (spusr != null)
                            {
                                authToView.Add(spusr.Id);
                            }
                        }
                    }
                }
            }

            newService.AuthorizedToView = string.Join(Constants.Separator6, authToView);
            newService.OwnerUser = string.Join(Constants.Separator6, owners);
            ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(_context);
            if (!string.IsNullOrEmpty(serviceError.ServiceCategoryType))
            {
                List<ServiceCategory> allServiceCategories = objServiceCategoryManager.Load(); // ServiceCategory.LoadAllCategories();

                ServiceCategory category = allServiceCategories.FirstOrDefault(x => x.CategoryName == serviceError.ServiceCategoryType);
                if (category == null)
                {
                    category = new ServiceCategory();
                    category.CategoryName = serviceError.ServiceCategoryType;
                    category.Title = serviceError.ServiceCategoryType;
                    objServiceCategoryManager.Insert(category);
                }
                newService.ServiceCategoryType = category.CategoryName;
                newService.CategoryId = category.ID;
            }
            if (serviceError != null)
            {
                Dictionary<string, string> customProperties = newService.CustomProperties;
                if (customProperties == null)
                {
                    customProperties = new Dictionary<string, string>();
                }
                if (!customProperties.ContainsKey("serviceerrors"))
                {
                    customProperties.Add("serviceerrors", Convert.ToString(SerializeService(serviceError)).Replace("=", "~"));
                }
                else
                {
                    customProperties["serviceerrors"] = Convert.ToString(SerializeService(serviceError));
                }
                if (!customProperties.ContainsKey("serviceerrorshtml"))
                {
                    //customProperties.Add("serviceerrorshtml", HttpContext.Current.Server.HtmlEncode(divError.InnerHtml));
                }
                else
                {
                   // customProperties["serviceerrorshtml"] = HttpContext.Current.Server.HtmlEncode(divError.InnerHtml);
                }
                newService.CustomProperties = customProperties;
            }
            objServicesManager = new ServicesManager(_context);
            var map = new AutoMapper.MapperConfiguration(config => {
                config.CreateMap<ServiceExtension, Services>();
            }).CreateMapper();

            Services svc = map.Map<Services>(serviceExt);

            objServicesManager.Save(svc);
            SaveSection(svc);
            SaveQuestions(svc);
            SaveSectionQuestionOrder(svc);
            SaveTasks(svc, serviceExt);
            SaveSkipTasksAndSections(svc);
            SaveMapDeafultValues(svc);
        }

        private void SaveSection(Services newService)
        {
            objServiceSectionManager = new ServiceSectionManager(_context);
            if (newService.Sections != null && newService.Sections.Count > 0)
            {
                foreach (ServiceSection section in newService.Sections)
                {
                    long id = section.ID;
                    string sectionName = section.SectionName;
                    if (string.IsNullOrWhiteSpace(sectionName))
                        sectionName = section.Title;
                    section.SectionName = sectionName;
                    section.ServiceID = newService.ID;
                    section.ID = 0;
                    section.TenantID = _context.TenantID; // Assigning loggedin users TenantID;
                    objServiceSectionManager.Save(section);
                    // section.SetSectionId();
                    //section.Save();
                    //List<ServiceQuestion> svcQuestionList = newService.Questions.Where(c => c.ServiceSectionID == id && Convert.ToString(c.ServiceSectionName).Trim() == sectionName.Trim()).ToList();
                    List<ServiceQuestion> svcQuestionList = newService.Questions.Where(c => c.ServiceSectionID == id).ToList();
                    foreach (ServiceQuestion svcQuest in svcQuestionList)
                    {
                        svcQuest.ServiceSectionID = section.ID;
                        svcQuest.TenantID = _context.TenantID;
                    }
                    if (newService.SkipSectionCondition != null)
                    {
                        foreach (ServiceSectionCondition sectionCondition in newService.SkipSectionCondition)
                        {
                            List<long> sections = sectionCondition.SkipSectionsID;
                            if (sections != null && sections.Count > 0)
                            {
                                for (int i = 0; i < sections.Count; i++)
                                {
                                    if (sections[i] == id)
                                        sections[i] = section.ID;
                                }
                                sectionCondition.SkipSectionsID = sections;
                            }
                        }
                    }
                }
            }
        }

        private void SaveQuestions(Services newService)
        {
            objServiceQuestionManager = new ServiceQuestionManager(_context);
            if (newService.Questions != null && newService.Questions.Count > 0)
            {
                // SPList applicationList = SPListHelper.GetSPList(DatabaseObjects.Lists.Applications);
                //DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications); //applicationList.Items.GetDataTable();
                foreach (ServiceQuestion question in newService.Questions)
                {
                    int questionId = Convert.ToInt32(question.ID);
                    string questionTitle = question.QuestionTitle;
                    // question.SetQuestionId();
                    question.ID = 0;
                    question.ServiceID = newService.ID;

                    //Add Navigateurl at question level

                    //WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(_context);

                    //WikiArticles servicesWikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.ModuleName}='svc'");//ModuleName related to question nee to add in wikiarticle

                    //if (servicesWikiArticle != null && !string.IsNullOrEmpty(servicesWikiArticle.TicketId))
                    //{
                    //    var NavigationUrl = $"/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={servicesWikiArticle.TicketId}";

                    //    question.NavigationUrl = NavigationUrl;
                    //}

                    objServiceQuestionManager.Save(question);
                    foreach (ServiceQuestion ques in newService.Questions.Where(x => x.QuestionType.ToLower() == "applicationaccessrequest"))
                    {
                        if (ques.QuestionTypePropertiesDicObj != null && ques.QuestionTypePropertiesDicObj.Count > 0)
                        {
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            string changedKey = string.Empty;
                            KeyValuePair<string, string> existingUser = ques.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "existinguser");
                            if (existingUser.Key != null)
                            {
                                changedKey = "existinguser";

                            }
                            if (!string.IsNullOrWhiteSpace(changedKey))
                            {
                                ques.QuestionTypePropertiesDicObj[changedKey] = Convert.ToString(question.ID);
                            }
                        }
                    }

                    foreach (ServiceQuestionMapping quesMap in newService.QuestionsMapping)
                    {
                        quesMap.ID = 0;
                        //quesMap.SetDefaultId();
                        quesMap.ServiceID = newService.ID;
                        quesMap.TenantID = _context.TenantID;
                        if (quesMap.ServiceQuestionID > 0 && quesMap.ServiceQuestionID == questionId && !string.IsNullOrEmpty(quesMap.ServiceQuestionName) && quesMap.ServiceQuestionName.Trim() == Convert.ToString(questionTitle).Trim())
                        {
                            quesMap.ServiceQuestionID = Convert.ToInt32(question.ID);
                            quesMap.PickValueFrom = Convert.ToString(question.ID);
                        }
                    }
                    if (newService.SkipSectionCondition != null)
                    {
                        foreach (ServiceSectionCondition sectionCondition in newService.SkipSectionCondition)
                        {
                            List<long> questions = sectionCondition.SkipQuestionsID;
                            if (questions != null && questions.Count > 0)
                            {
                                for (int i = 0; i < questions.Count; i++)
                                {
                                    if (questions[i] == questionId)
                                        questions[i] = Convert.ToInt32(question.ID);
                                }
                                sectionCondition.SkipQuestionsID = questions;
                            }
                        }
                    }

                }
            }
        }

        private void SaveSectionQuestionOrder(Services newService)
        {
            if (newService.Sections != null && newService.Sections.Count > 0)
            {
                //  ServiceSection.SaveOrder(newService.Sections);
            }
            if (newService.Questions != null && newService.Questions.Count > 0)
            {
                //  ServiceQuestion.SaveOrder(newService.Questions);
            }

        }


        private void SaveTasks(Services newService, ServiceExtension serviceExt)
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(_context);
            Dictionary<long, long> tempdic = new Dictionary<long, long>();
            if (newService.Tasks != null && newService.Tasks.Count > 0)
            {
                UGITTask task = null;
                foreach (UGITTask task1 in newService.Tasks)
                {
                    task = task1;
                    long id = task.ID;
                    string title = task.Title;
                    task.TicketId = Convert.ToString(newService.ID);
                    task.TenantID = _context.TenantID;
                    task.ID = 0;
                    UGITTask errorTask = null;
                    if (serviceError != null && serviceError.Tasks != null && serviceError.Tasks.Count > 0)
                    {
                        errorTask = serviceError.Tasks.FirstOrDefault(c => c.ID == id);
                    }

                    if (errorTask != null && errorTask.AssignedTo != null)
                    {
                        task.AssignedTo = null;
                    }
                    else if (task.AssignedTo != null && task.AssignedTo != "")
                    {
                        //  SPFieldUserValueCollection userValueCollAssigned = new SPFieldUserValueCollection();
                        List<string> userValueCollAssigned = new List<string>();
                        foreach (var userEntity in UGITUtility.ConvertStringToList(task.AssignedTo, Constants.Separator6))
                        {
                            UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.Id == userEntity);
                            if (user == null)
                                continue;
                            if (user.isRole)
                            {
                                UserProfile spgroup = user; //UserProfile.GetGroupByName(user.Name);
                                if (spgroup != null && spgroup.Name == user.Name)
                                {
                                    //SPFieldUserValue userVal = new SPFieldUserValue(SPContext.Current.Web, spgroup.ID, spgroup.Name);
                                    if (spgroup != null)
                                        userValueCollAssigned.Add(spgroup.Id);
                                }
                            }
                            else
                            {
                                UserProfile spuser = user;
                                //SPUser spuser = UserProfile.GetUserById(userEntity.LookupId);
                                if (spuser != null && spuser.Name == user.Name)
                                {
                                    //SPFieldUserValue userVal = new SPFieldUserValue(SPContext.Current.Web, user.ID, user.Name);
                                    //if (userVal != null && userVal.User != null)
                                    //    userValueCollAssigned.Add(userVal);
                                    userValueCollAssigned.Add(spuser.Id);
                                }
                            }
                        }
                        task.AssignedTo = string.Join(Constants.Separator6, userValueCollAssigned);
                    }
                    if (errorTask != null && errorTask.ModuleNameLookup != null)
                    {
                        task.ModuleNameLookup = null;
                    }
                    else if (task.ModuleNameLookup != null)
                    {
                        LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == task.ModuleNameLookup && c.ListName.ToLower() == "config_modules") as LookupValueServiceExtension;
                        if (svcLookup != null)
                        {
                            task.ModuleNameLookup = Convert.ToString(svcLookup.ID);
                            //SPFieldLookupValue lookUpValue = new SPFieldLookupValue(svcLookup.ID, svcLookup.Value);
                            //if (lookUpValue != null)
                            //    task.ModuleName = lookUpValue;
                        }
                    }

                    if (errorTask != null && errorTask.RequestTypeCategory != null)
                    {
                        task.RequestTypeCategory = null;
                    }
                    else if (task.RequestTypeCategory != null && task.RequestTypeCategory != "")
                    {
                        LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == task.RequestTypeCategory && c.ListName.ToLower() == "requestcategory");
                        if (svcLookup != null)
                        {
                            task.RequestTypeCategory = svcLookup.ListName;
                            //SPFieldLookupValue lookUpValue = new SPFieldLookupValue(svcLookup.ID, svcLookup.Value);
                            //if (lookUpValue != null)
                            //    task.RequestCategory = lookUpValue;
                        }
                    }

                    if (task.ParentTaskID > 0)
                    {
                        long parentId = task.ParentTaskID;
                        task.ParentTaskID = tempdic[parentId];
                    }
                    string TicketId = Convert.ToString(newService.ID);
                    string moduleName = newService.ModuleNameLookup;
                    objUGITTaskManager.SaveTask(ref task, moduleName, TicketId);
                    // task.Save();
                    tempdic.Add(id, task.ID);

                    foreach (UGITTask svcTask in newService.Tasks)
                    {
                        if (svcTask.Predecessors != null)
                        {
                            foreach (string lookupValue in UGITUtility.ConvertStringToList(svcTask.Predecessors, Constants.Separator6))
                            {
                                LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == lookupValue && c.ListName.ToLower() == "serviceticketrelationships");
                                //if (svcLookup != null && (svcLookup.ID == id && svcLookup.Value == title))
                                //{
                                //    lookupValue = task.ID;
                                //}
                            }
                        }
                    }
                    if (newService.SkipTaskCondition != null)
                    {
                        foreach (ServiceTaskCondition taskCondition in newService.SkipTaskCondition)
                        {
                            List<long> tasks = taskCondition.SkipTasks;
                            if (tasks != null && tasks.Count > 0)
                            {
                                for (int i = 0; i < tasks.Count; i++)
                                {
                                    if (tasks[i] == id)
                                        tasks[i] = task.ID;
                                }
                                taskCondition.SkipTasks = tasks;
                            }
                        }
                    }
                    List<ServiceQuestionMapping> questionMapList = newService.QuestionsMapping.Where(x => x.ServiceTaskID == id).ToList();
                    if (questionMapList != null && questionMapList.Count > 0)
                    {
                        foreach (ServiceQuestionMapping questionMap in questionMapList)
                        {
                            questionMap.ServiceTaskID = task.ID;
                        }
                    }
                }
            }
        }

        public static string SerializeService(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static object DeSerializeService(string s, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(s);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }

        private void SaveSkipTasksAndSections(Services newService)
        {
            if ((newService.SkipTaskCondition != null && newService.SkipTaskCondition.Count > 0) || (newService.SkipSectionCondition != null && newService.SkipSectionCondition.Count > 0))
            {
                objServicesManager = new ServicesManager(_context);
                objServicesManager.Save(newService);
                //newService.Save();
            }
        }

        private void SaveMapDeafultValues(Services newService)
        {
            if (newService.QuestionsMapping != null && newService.QuestionsMapping.Count > 0)
            {
                foreach (ServiceQuestionMapping serviceQuesMap in newService.QuestionsMapping)
                {
                    objServiceQuestionMappingManager = new ServiceQuestionMappingManager(_context);
                    objServiceQuestionMappingManager.Save(serviceQuesMap);
                    // serviceQuesMap.Save();
                }
            }
        }
    }
}
