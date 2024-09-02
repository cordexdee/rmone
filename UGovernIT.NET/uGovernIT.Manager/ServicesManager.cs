using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using System.Data;
using System.Xml;
using uGovernIT.Utility;
using System.Web;
using System.Web.UI.WebControls;
using uGovernIT.Utility.Entities;
using System.IO;
using System.Runtime.Serialization;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class ServicesManager : ManagerBase<Services>, IServicesManager
    {
        ApplicationContext _context;
        private Dictionary<int, int> importSrvQuestionList;
        ServiceSectionManager objServiceSectionManager;
        public ServicesManager(ApplicationContext context,string requestPage="") : base(context, requestPage)
        {
             if ( requestPage == "SelfRegistration")
            {
                _context = ApplicationContext.Create();
            }
            else
            _context = context;

            objServiceSectionManager = new ServiceSectionManager(_context);
        }

        /// <summary>
        /// Returns only service categories
        /// </summary>
        public List<ServiceCategory> ServiceCategories
        {
            get
            {
                ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(dbContext);
                List<ServiceCategory> categories = serviceCategoryManager.LoadAllCategories();
                if (categories != null && categories.Count > 0)
                {
                    categories = categories.Where(x => x.CategoryName != Constants.ModuleFeedback && x.CategoryName != Constants.ModuleAgent).OrderBy(x => x.ItemOrder).ToList();
                }
                return categories;
            }
        }

        /// <summary>
        /// Load Service by service id
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns></returns>
        
        public Services LoadByServiceID(long serviceId)
        {
            return LoadByServiceID(serviceId, true, true, true);
        }

        #region Method to Get Updated Service Title
        /// <summary>
        /// This method is used to provide suffix to the Service Title if the Service already exists
        /// </summary>
        /// <param name="oldServiceTitle"></param>
        /// <param name="dtServices"></param>
        /// <returns></returns>
        public static string GetUpdatedServiceTitle(string oldServiceTitle, List<Services> dtServices)
        {
            string newServiceTitle = string.Empty;

            if (string.IsNullOrEmpty(oldServiceTitle))
                return newServiceTitle;

            newServiceTitle = oldServiceTitle;

            if (dtServices == null || dtServices.Count == 0)
                return newServiceTitle;

            if (dtServices.Any(x => x.Title == newServiceTitle))
                newServiceTitle = oldServiceTitle + " - Copy";

            int counter = 1;
            while (dtServices.Any(x => x.Title == newServiceTitle))
            {
                newServiceTitle = oldServiceTitle + " - Copy" + counter;
                counter++;
            }

            return newServiceTitle;
        }
        #endregion Method to Get Updated Service Title
        #region Method to Get Updated Service Title overload
        /// <summary>
        /// This method is used to provide suffix to the Service Title if the Service already exists
        /// </summary>
        /// <param name="oldServiceTitle"></param>
        /// <param name="dtServices"></param>
        /// <returns></returns>
        public string GetUpdatedServiceTitle(string oldServiceTitle, DataTable dtServices)
        {
            string newServiceTitle = string.Empty;

            if (string.IsNullOrEmpty(oldServiceTitle))
                return newServiceTitle;

            newServiceTitle = oldServiceTitle;

            if (dtServices == null || dtServices.Rows.Count == 0)
                return newServiceTitle;

            if (dtServices.AsEnumerable().Any(x => x.Field<string>(DatabaseObjects.Columns.Title) == newServiceTitle))
                newServiceTitle = oldServiceTitle + " - Copy";

            int counter = 1;
            while (dtServices.AsEnumerable().Any(x => x.Field<string>(DatabaseObjects.Columns.Title) == newServiceTitle))
            {
                newServiceTitle = oldServiceTitle + " - Copy" + counter;
                counter++;
            }

            return newServiceTitle;
        }
        #endregion Method to Get Updated Service Title
        public Services LoadByServiceID(long serviceId, bool isLoadQuestions, bool isLoadSections, bool isLoadTasks)
        {
            Services service = null;
            if (serviceId != 0)
            {
                service = LoadByID(serviceId); // UGITUtility.GetSPListItem(DatabaseObjects.Tables.Services, serviceId, DatabaseObjects.ViewFields.Services);
                return LoadItem(service, isLoadQuestions, isLoadSections, isLoadTasks);
            }

            return null;
        }

        public Services LoadItem(Services item, bool isLoadQuestions, bool isLoadSections, bool isLoadTasks)
        {
            item.CustomProperties = UGITUtility.GetCustomProperties(Convert.ToString(item.CustomProperties), Constants.Separator);

            item.Tasks = new List<UGITTask>();
            item.Questions = new List<ServiceQuestion>();
            item.QuestionsMapping = new List<ServiceQuestionMapping>();
            item.Sections = new List<ServiceSection>();

            if (isLoadQuestions)
            {
                ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(dbContext);
                item.Questions = serviceQuestionManager.GetByServiceID(item.ID, false);
                if(item.Questions != null && item.Questions.Count > 0)
                {
                    foreach(ServiceQuestion q in item.Questions)
                    {
                        Dictionary<string, object> tempObj = null;
                        if (q.QuestionTypeProperties.StartsWith("<?xml") || q.QuestionTypeProperties.StartsWith("<ArrayOfDataItem xmlns:"))
                        {
                            if (!string.IsNullOrEmpty(q.QuestionTypeProperties))
                                tempObj = UGITUtility.DeserializeDicObject(q.QuestionTypeProperties);
                            if (tempObj != null)
                                q.QuestionTypePropertiesDicObj = tempObj.ToDictionary(k => k.Key, k => k.Value == null ? "" : Convert.ToString(k.Value));
                        }
                    }
                }
            }

            if (isLoadTasks)
            {
                ServiceTaskManager serviceTaskManager = new ServiceTaskManager(dbContext);
                item.Tasks = serviceTaskManager.LoadByServiceID(Convert.ToString(item.ID));
                ServiceQuestionMappingManager serviceQuestionMappingManager = new ServiceQuestionMappingManager(dbContext);
                item.QuestionsMapping = serviceQuestionMappingManager.GetByServiceID(item.ID);
            }

            if (isLoadSections)
            {
                item.Sections = objServiceSectionManager.GetByServiceID(item.ID);
            }

            string logic = Convert.ToString(item.ConditionalLogic);
            if (!string.IsNullOrEmpty(logic))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(logic);
                List<ServiceTaskCondition> conditions = new List<ServiceTaskCondition>();
                conditions = (List<ServiceTaskCondition>)uHelper.DeSerializeAnObject(doc, conditions);
                item.SkipTaskCondition = conditions;
            }

            string sectionLogic = Convert.ToString(item.SectionConditionalLogic);
            if (!string.IsNullOrEmpty(sectionLogic))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sectionLogic);
                List<ServiceSectionCondition> conditions = new List<ServiceSectionCondition>();
                conditions = (List<ServiceSectionCondition>)uHelper.DeSerializeAnObject(doc, conditions);
                item.SkipSectionCondition = conditions;
            }

            string mapVariables = Convert.ToString(item.QuestionMapVariables);
            item.QMapVariables = new List<QuestionMapVariable>();
            if (!string.IsNullOrEmpty(mapVariables))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(mapVariables);
                List<QuestionMapVariable> mapVars = new List<QuestionMapVariable>();
                mapVars = (List<QuestionMapVariable>)uHelper.DeSerializeAnObject(doc, mapVars);
                item.QMapVariables = mapVars;
            }

            int itemOrder = 0;
            int.TryParse(Convert.ToString(item.ItemOrder), out itemOrder);
            item.ItemOrder = itemOrder;
            
            //sr.LoadDefaultValue = UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.LoadDefaultValue]);

            if (item.AttachmentRequired != null)
            {
                item.AttachmentRequired = Convert.ToString(item.AttachmentRequired);
                if (string.IsNullOrEmpty(item.AttachmentRequired) || item.AttachmentRequired.ToLower() == "never" || item.AttachmentRequired.ToLower() == "optional")
                {
                    item.AttachmentRequired = "Optional";
                }
                else if (item.AttachmentRequired.ToLower() != "always" && item.AttachmentRequired.ToLower() != "disabled")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(item.AttachmentRequired);
                    ServiceSectionCondition condition = new ServiceSectionCondition();
                    condition = (ServiceSectionCondition)uHelper.DeSerializeAnObject(doc, condition);
                    item.AttachmentRequired = "Conditional";
                    item.AttachmentRequiredCondition = condition;
                }
                item.SLADisabled = UGITUtility.StringToBoolean(item.SLADisabled);
                item.ResolutionSLA = UGITUtility.StringToDouble(item.ResolutionSLA);
            }
            item.StartResolutionSLAFromAssigned = UGITUtility.StringToBoolean(item.StartResolutionSLAFromAssigned);

            if (!string.IsNullOrEmpty(item.SLAConfiguration))
            {
                try
                {
                    item.SLAConfig = new SLAConfiguration();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(item.SLAConfiguration);
                    SLAConfiguration slaConfiguration = new SLAConfiguration();
                    slaConfiguration = (SLAConfiguration)uHelper.DeSerializeAnObject(doc, slaConfiguration);
                    item.SLAConfig = slaConfiguration;
                }
                catch { }
            }

            return item;
        }

        public bool Save(Services objService)
        {
            #region sp to .net

            #endregion
            if (objService.CustomProperties != null && objService.CustomProperties.Count > 0)
            {
                StringBuilder param = new StringBuilder();
                foreach (string key in objService.CustomProperties.Keys)
                {
                    param.AppendFormat("{0}={1}{2}", key, objService.CustomProperties[key], Constants.Separator);
                }
            }

            if (objService.SkipTaskCondition != null && objService.SkipTaskCondition.Count > 0)
            {
                XmlDocument doc = uHelper.SerializeObject(objService.SkipTaskCondition);
                objService.ConditionalLogic = doc.OuterXml;
            }
            else
            {
                objService.ConditionalLogic = null;
            }

            if (objService.SkipSectionCondition != null && objService.SkipSectionCondition.Count > 0)
            {
                XmlDocument doc = uHelper.SerializeObject(objService.SkipSectionCondition);
                objService.SectionConditionalLogic = doc.OuterXml;
            }
            else
            {
                objService.SectionConditionalLogic = null;
            }

            if (objService.QMapVariables != null && objService.QMapVariables.Count > 0)
            {
                XmlDocument doc = uHelper.SerializeObject(objService.QMapVariables);
                objService.QuestionMapVariables = doc.OuterXml;
            }
            else
            {
                objService.QuestionMapVariables = null;
            }


            if (string.IsNullOrEmpty(objService.AttachmentRequired))
                objService.AttachmentRequired = "Optional";
            
            if (objService.AttachmentRequired.ToLower() == "conditional" && objService.AttachmentRequiredCondition != null)
            {
                XmlDocument doc = uHelper.SerializeObject(objService.AttachmentRequiredCondition);
                objService.AttachmentRequired = doc.OuterXml;
            }

            if (objService.SLAConfig != null)
            {
                XmlDocument slaDoc = uHelper.SerializeObject(objService.SLAConfig);
                objService.SLAConfiguration = slaDoc.OuterXml;
            }


            if (objService.ID <= 0)
            {
                long i = Insert(objService);
                if (i > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return Update(objService);
            }

        }

        //Added for converting from DataTable to List
        public List<Services> LoadAllServices(string serviceName)
        {
            List<Services> services = new List<Services>();
            services = Load();

            if (services != null && services.Count > 0)
            {

                if (serviceName.ToLower() == Constants.ModuleAgent.ToLower())
                    services = services.Where(x => x.ServiceType.Equals("ModuleAgent", StringComparison.InvariantCultureIgnoreCase)).ToList();
                //view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.ServiceType, "ModuleAgent");
                else if (serviceName.ToLower() == Constants.ModuleFeedback.ToLower())
                    services = services.Where(x => x.ServiceType.Equals("ModuleFeedback", StringComparison.InvariantCultureIgnoreCase)).ToList();
                //view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.ServiceType, "ModuleFeedback");
                else
                    services = services.Where(x => (!x.ServiceType.Equals("ModuleAgent", StringComparison.InvariantCultureIgnoreCase)) && (!x.ServiceType.Equals("ModuleFeedback", StringComparison.InvariantCultureIgnoreCase))).ToList();
                //view.RowFilter += string.Format(" AND {0} <> '{1}' AND {0} <> '{2}'", DatabaseObjects.Columns.ServiceType, "ModuleAgent", "ModuleFeedback");
            }

            return services;
        }

        public DataTable All(bool getModuleAgents = false)
        {
            DataTable table = UGITUtility.ToDataTable<Services>(Load()); // SPListHelper.GetDataTable(DatabaseObjects.Tables.Services);
            ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(dbContext);
            List<ServiceCategory> lstServiceCategory = serviceCategoryManager.LoadAllCategories();
            if (table != null && table.Rows.Count > 0)
            {
                if (!table.Columns.Contains(DatabaseObjects.Columns.CategoryItemOrder))
                    table.Columns.Add(DatabaseObjects.Columns.CategoryItemOrder);

                foreach (DataRow dr in table.Rows)
                {
                    ServiceCategory sCategory = lstServiceCategory.Where(m => m.CategoryName.Equals(Convert.ToString(dr[DatabaseObjects.Columns.ServiceCategoryNameLookup]), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (sCategory != null)
                        dr[DatabaseObjects.Columns.CategoryItemOrder] = sCategory.ItemOrder;
                }

                DataView view = table.DefaultView;
                if (getModuleAgents) // Get Module Agents only
                    view.RowFilter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ServiceType, Constants.ModuleAgent);
                else // Get services only (exclude agents and surveys)
                    view.RowFilter += string.Format("{0} <> '{1}' AND {0} <> '{2}'",
                                                    DatabaseObjects.Columns.ServiceType, Constants.ModuleAgent, Constants.ModuleFeedback);
                table = view.ToTable();
            }

            return table;
        }
        public List<Services> LoadAllServices()
        {
            List<Services> services = new List<Services>();
            services = Load();

            return services;
        }
        // changed by maynk singh
        public List<Services> LoadCurrentUserServices()
        {
            List<Services> services = new List<Services>();
            services = Load();
            ApplicationContext _Context = ApplicationContext.Create();
            if (services != null && services.Count > 0)
                services = services.Where(x => 
                x.ServiceType != Constants.ModuleFeedback && x.ServiceType != "ModuleAgent" || (!string.IsNullOrEmpty(x.AuthorizedToView) && x.AuthorizedToView.Contains(dbContext.CurrentUser.Id))).OrderBy(x=> x.Title).ToList();
           
            services?.ForEach(y => LoadItem(y, false, false, false));
            return services;
        }

        public List<Services> LoadAllSurveys()
        {
            List<Services> services = new List<Services>();
            services = Load().Where(x => x.ServiceType == Constants.ModuleFeedback.Replace(Constants.Separator2, string.Empty)).ToList();

            return services;
        }

        public List<Services> LoadAllAgents()
        {
            List<Services> services = new List<Services>();
            return services;
        }

        public Services LoadSurvey(string moduleName)
        {
            List<Services> allServices = LoadAllSurveys();
            if (allServices != null && allServices.Count > 0)
            {
                Services sr = null;
                if (string.IsNullOrEmpty(moduleName) || moduleName.ToLower() == "generic")
                    sr = allServices.FirstOrDefault(x => string.IsNullOrEmpty(x.ModuleNameLookup));
                else
                    sr = allServices.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.ModuleNameLookup) && x.ModuleNameLookup.ToLower() == moduleName.ToLower());

                //Load service object with questions
                if (sr != null)
                    sr = LoadByServiceID(sr.ID, true, true, false);

                return sr;
            }
            return null;
        }


        public Services LoadSurveybySurvey(int surveyId)
        {

            if (surveyId > 0)
            {
                Services sr = LoadByServiceID(surveyId, true, true, false);
                return sr;
            }

            return null;
        }

        public void Delete()
        {
            //ServiceQuestionMapping.Delete(this.QuestionsMapping);
            //ServiceTask.Delete(this.Tasks);
            //ServiceQuestion.Delete(this.Questions);
            //ServiceSection.Delete(this.Sections);
            //SPList servicesList = SPListHelper.GetSPList(DatabaseObjects.Tables.Services);
            //SPListItem serviceItem = SPListHelper.GetSPListItem(servicesList, ID);
            //serviceItem.Delete();
        }
        public void DeleteAll(long sID)
        {
            Services service = LoadByID(sID);
            service.Deleted = true;
            Update(service);
            /*Anurag- we are not deleting any from database so we have to use archive feature*/

            //SurveyFeedbackManager surveyFeedbackManager = new SurveyFeedbackManager(dbContext);
            //List<SurveyFeedback> objSurveyFeedback = surveyFeedbackManager.Load(x => x.ServiceLookUp == Convert.ToInt64(sID));
            //objSurveyFeedback.ForEach(y => { surveyFeedbackManager.Delete(y); });

            //ServiceQuestionMappingManager objServiceQuestionMappingManager = new ServiceQuestionMappingManager(_context);
            //List<ServiceQuestionMapping> ServiceQuestionMapping = objServiceQuestionMappingManager.Load(x => x.ServiceID == sID).ToList();
            //ServiceQuestionMapping.ForEach(y => { objServiceQuestionMappingManager.Delete(y); });

            //ServiceTaskManager objServiceTaskManager = new ServiceTaskManager(_context);
            //List<UGITTask> ServiceTask = objServiceTaskManager.Load(x => x.TicketId == Convert.ToString(sID)).ToList();
            //ServiceTask.ForEach(y=> { objServiceTaskManager.Delete(y);});

            // ServiceQuestionManager objServiceQuestionManager = new ServiceQuestionManager(_context);
            // List<ServiceQuestion> ServiceQuestion = objServiceQuestionManager.Load(x => x.ServiceID == sID).ToList();
            // ServiceQuestion.ForEach(y => objServiceQuestionManager.Delete(y));

            //ServiceSectionManager objServiceSectionManager = new ServiceSectionManager(_context);
            //List<ServiceSection> ServiceSection = objServiceSectionManager.Load(x => x.ServiceID == sID).ToList();
            //ServiceSection.ForEach(y=> objServiceSectionManager.Delete(y));
            //Delete(service);
        }
        public void Archive(long sID)
        {
            Services service = LoadByID(sID);
            service.Deleted = true;
            Update(service);
        }

        public void UnArchive(long sID)
        {
            Services service = LoadByID(sID);
            service.Deleted = false;
            Update(service);
            //SPList servicesList = SPListHelper.GetSPList(DatabaseObjects.Tables.Services);
            //SPListItem serviceItem = SPListHelper.GetSPListItem(servicesList, ID);
            //serviceItem[DatabaseObjects.Columns.IsDeleted] = 0;
            //serviceItem.Update();
        }

        #region Sub Tickets

        public void SaveCategoryOrder(Dictionary<int, int> categoryOrders)
        {
            //string cListID = Convert.ToString(uGITCache.GetListID(DatabaseObjects.Tables.ServiceCategories));
            //if (categoryOrders.Count <= 0)
            //{
            //    return;
            //}

            //SPList questionList = SPListHelper.GetSPList(DatabaseObjects.Tables.ServiceQuestions);

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemOrder\">{3}</SetVar>" +
            // "</Method>";

            //StringBuilder query = new StringBuilder();
            //foreach (int categoryID in categoryOrders.Keys)
            //{
            //    query.AppendFormat(updateMethodFormat, categoryID, cListID, categoryID, categoryOrders[categoryID]);
            //}
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }
        #endregion

        #region Method to Serialize a Service
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
        #endregion Method to Serialize a Service

        #region Method to DeSerialize a Service
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
        #endregion Method to DeSerialize a Service

        #region Method to Save Duplicate Service
        public void SaveDuplicateService(ServiceExtension serviceExt, ServiceExtension serviceError)
        {
            Services service = (Services)serviceExt;
            Services newService = service;
            newService.ID = 0;
            List<string> owners = new List<string>();
            
            if ((serviceError == null || serviceError.OwnerUser == null) && serviceExt.OwnerUser != null)
            {
                foreach (string userEntity in UGITUtility.ConvertStringToList(serviceExt.OwnerUser, Constants.Separator6))
                {
                    UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.Id == userEntity);
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
            var map = new AutoMapper.MapperConfiguration(config => {
                config.CreateMap<ServiceExtension, Services>();
            }).CreateMapper();

            Services svc = map.Map<Services>(serviceExt);

            Save(svc);
            SaveSection(svc);
            SaveQuestions(svc);
            SaveSectionQuestionOrder(svc);
            SaveTasks(svc);
            SaveSkipTasksAndSections(svc);
            SaveMapDeafultValues(svc);
        }
        #endregion Method to Save Duplicate Service

        public void SaveSection(Services newService)
        {
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

        public void SaveQuestions(Services newService)
        {
            ServiceQuestionManager objServiceQuestionManager = new ServiceQuestionManager(_context);
            //key would be imported question id and 
            // value would new question id against question
            Dictionary<int, int> questionImpMap = new Dictionary<int, int>();
            if (newService.Questions != null && newService.Questions.Count > 0)
            {
                DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");

                foreach (ServiceQuestion question in newService.Questions)
                {
                    int questionId = Convert.ToInt32(question.ID);
                    string questionTitle = question.QuestionTitle;
                    question.ID = 0;
                    question.ServiceID = newService.ID;
                    long quesSectionId = question.ServiceSectionID ?? 0;
                    if (quesSectionId > 0)
                    {
                        ServiceSection serviceSection =objServiceSectionManager.LoadByID(quesSectionId);
                        if (serviceSection == null)
                            question.ServiceSectionID = 0;
                    }
                    objServiceQuestionManager.Save(question);
                    if (!questionImpMap.ContainsKey(questionId))
                        questionImpMap.Add(questionId,UGITUtility.StringToInt(question.ID));
                }

                string[] questionRelatedKeys = new string[] {
                    "locationuserquestion",
                    "departmentuserquestion",
                    "dependentlocationquestion",
                    "companydivisions",
                    "MirrorAccessFrom",
                    "ExistingUser",
                    "NewUSer",
                    "userquestion",
                    "validateagainst"
                };

                bool saveChanges = false;
                foreach (ServiceQuestion question in newService.Questions)
                {
                    saveChanges = false;
                    if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                    {
                        List<KeyValuePair<string, string>> questionRelatedProps = question.QuestionTypePropertiesDicObj.Where(x => !string.IsNullOrWhiteSpace(x.Key) && questionRelatedKeys.Any(y => y.ToLower() == x.Key.ToLower())).ToList();
                        foreach (KeyValuePair<string, string> keyPair in questionRelatedProps)
                        {
                            if (string.IsNullOrWhiteSpace(keyPair.Value))
                                continue;


                            if (!newService.Questions.Any(x => x.TokenName.ToLower() == keyPair.Value.ToLower()))
                            {
                                int questOldID = UGITUtility.StringToInt(keyPair.Value);
                                if (questOldID <= 0)
                                    continue;

                                if (questionImpMap.ContainsKey(questOldID))
                                {
                                    saveChanges = true;
                                    question.QuestionTypePropertiesDicObj[keyPair.Key] = Convert.ToString(questionImpMap[questOldID]);
                                }
                            }
                            else
                            {
                                ServiceQuestion sQuest = newService.Questions.FirstOrDefault(x => x.TokenName.ToLower() == keyPair.Value.ToLower());
                                if (sQuest != null)
                                {
                                    saveChanges = true;
                                    question.QuestionTypePropertiesDicObj[keyPair.Key] = sQuest.ID.ToString();
                                }
                            }
                        }
                    }

                    if (saveChanges)
                        objServiceQuestionManager.Save(question);
                }

                foreach (ServiceQuestionMapping quesMap in newService.QuestionsMapping)
                {
                    quesMap.ID = 0;
                    quesMap.ServiceID = newService.ID;
                    int quesMapServiceId =UGITUtility.StringToInt(quesMap.ServiceQuestionID ?? 0);
                    if (quesMap.ServiceQuestionID > 0 && questionImpMap.ContainsKey(quesMapServiceId))
                    {
                        int questionID =UGITUtility.StringToInt(questionImpMap[quesMapServiceId]);
                        quesMap.ServiceQuestionID = questionID;
                        quesMap.PickValueFrom = Convert.ToString(questionID);
                    }
                }
                if (questionImpMap != null && questionImpMap.Count > 0)
                    importSrvQuestionList = questionImpMap;

                foreach (ServiceSectionCondition sectionCondition in newService.SkipSectionCondition)
                {
                    List<int> questions = new List<int>();
                    if(sectionCondition.SkipQuestionsID!=null && sectionCondition.SkipQuestionsID.Count>0)
                        questions=(sectionCondition.SkipQuestionsID).ConvertAll(i=>(int)i);

                    List<int> newQuestions = new List<int>();
                    if (questions != null && questions.Count > 0)
                    {
                        for (int i = 0; i < questions.Count; i++)
                        {
                            int oldQuestionID = UGITUtility.StringToInt(questions[i]);
                            if (questionImpMap.ContainsKey(oldQuestionID))
                                newQuestions.Add(questionImpMap[oldQuestionID]);
                        }

                        newQuestions = newQuestions ?? new List<int>();
                        sectionCondition.SkipQuestionsID = newQuestions.ConvertAll(i=>(long)i);
                    }
                }
            }
        }

        public void SaveSectionQuestionOrder(Services newService)
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

        public void SaveTasks(Services newService)
        {
            #region SP to .Net
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(_context);
            Dictionary<long, long> tempdic = new Dictionary<long, long>();

            if (newService.Tasks != null && newService.Tasks.Count > 0)
            {
                List<ServiceQuestion> updatedQuestions = new List<ServiceQuestion>();
                UGITTask task = null;
                foreach (UGITTask task1 in newService.Tasks)
                {
                    task = task1;
                    long id = task.ID;
                    string title = task.Title;
                    task.ID = 0;

                    if (task.ParentTask!=null && task.ParentTask.ID > 0)
                    {
                        long parentId = task.ParentTask.ID;
                        task.ParentTask.ID = tempdic[parentId];
                    }

                    int questionId = UGITUtility.StringToInt(task.QuestionID);
                    if (importSrvQuestionList != null && importSrvQuestionList.Count > 0 && importSrvQuestionList.ContainsKey(questionId))
                        task.QuestionID = Convert.ToString(importSrvQuestionList[questionId]);

                    string TicketId = Convert.ToString(newService.ID);
                    string moduleName = newService.ModuleNameLookup;
                    objUGITTaskManager.SaveTask(ref task, moduleName, TicketId);
                    tempdic.Add(id, task.ID);

                    foreach (UGITTask svcTask in newService.Tasks)
                    {
                        if (!string.IsNullOrWhiteSpace(svcTask.Predecessors))
                        {
                            List<string> lstOfPredecssor = UGITUtility.ConvertStringToList(svcTask.Predecessors, Constants.Separator6);
                            string updatePredecessor = svcTask.Predecessors;
                            foreach (string lookupValue in lstOfPredecssor)
                            {
                                if (UGITUtility.StringToLong(lookupValue) == id)
                                {
                                    updatePredecessor= updatePredecessor.Replace(lookupValue,UGITUtility.ObjectToString(task.ID));
                                }
                            }

                            svcTask.Predecessors = updatePredecessor;
                        }
                    }

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

                    List<ServiceQuestionMapping> questionMapList = newService.QuestionsMapping.Where(x => x.ServiceTaskID == id).ToList();
                    if (questionMapList != null && questionMapList.Count > 0)
                    {
                        foreach (ServiceQuestionMapping questionMap in questionMapList)
                        {
                            questionMap.ServiceTaskID = task.ID;
                        }
                    }

                    // Update TaskIDs in QuestionTypeProperties for Predecessors
                    foreach (ServiceQuestion ques in newService.Questions)
                    {
                        if (ques.QuestionTypePropertiesDicObj == null || ques.QuestionTypePropertiesDicObj.Count == 0 ||
                            !ques.QuestionTypePropertiesDicObj.Keys.Contains("predecessors") || !ques.QuestionTypePropertiesDicObj["predecessors"].Contains(id.ToString()))
                            continue;

                        ServiceQuestion question = ques;
                        bool isQuesExistInUpdateList = updatedQuestions.Any(x => x.ID == ques.ID);

                        if (updatedQuestions.Count > 0 && isQuesExistInUpdateList)
                            question = updatedQuestions.Where(x => x.ID == ques.ID).Select(y => y).FirstOrDefault();

                        string oldTaskIDs = question.QuestionTypePropertiesDicObj["predecessors"];

                        if (!oldTaskIDs.Contains(Constants.Separator5))
                        {
                            question.QuestionTypePropertiesDicObj["predecessors"] = Convert.ToString(task.ID);
                        }
                        else
                        {
                            string[] taskIDs = oldTaskIDs.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < taskIDs.Length; i++)
                            {
                                if (taskIDs[i] == id.ToString())
                                    taskIDs[i] = Convert.ToString(task.ID);
                            }

                            question.QuestionTypePropertiesDicObj["predecessors"] = string.Join(Constants.Separator5, taskIDs);
                        }

                        if (!isQuesExistInUpdateList)
                            updatedQuestions.Add(question);
                    }
                }
                ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(_context);
                foreach (ServiceQuestion question in updatedQuestions)
                {
                    serviceQuestionManager.Save(question);
                }
            }
            #endregion
        }

        public void SaveSkipTasksAndSections(Services newService)
        {
            if ((newService.SkipTaskCondition != null && newService.SkipTaskCondition.Count > 0) || (newService.SkipSectionCondition != null && newService.SkipSectionCondition.Count > 0))
            {
                Save(newService);
            }
        }

        public void SaveMapDeafultValues(Services newService)
        {
            if (newService.QuestionsMapping != null && newService.QuestionsMapping.Count > 0)
            {
                ServiceQuestionMappingManager objServiceQuestionMappingManager = null;
                foreach (ServiceQuestionMapping serviceQuesMap in newService.QuestionsMapping)
                {
                    if (serviceQuesMap.ServiceQuestionID == 0)
                        serviceQuesMap.ServiceQuestionID = null;
                    if (serviceQuesMap.ServiceTaskID == 0)
                        serviceQuesMap.ServiceTaskID = null;
                    objServiceQuestionMappingManager = new ServiceQuestionMappingManager(_context);
                    objServiceQuestionMappingManager.Save(serviceQuesMap);
                    // serviceQuesMap.Save();
                }
            }
        }

        public void loadDdlServices( ref DropDownList ddl)
        {
            //List<Services> services = ServicesManager.LoadAllServices().Where(x=>x.IsActivated =true).ToList();
            //DDLservices.Items.Clear();
            //if (services != null)
            //{
            //    DDLservices.DataSource = services;
            //    DDLservices.DataValueField = DatabaseObjects.Columns.ID;
            //    DDLservices.DataTextField = DatabaseObjects.Columns.Title;
            //}
            //DDLservices.DataBind();
            //DDLservices.Items.Insert(0, new ListItem("None", ""));

            List<Services> services = LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Deleted == false).OrderBy(x => x.Title).ToList();
            List<string> Categories = services.Select(x => x.ServiceCategoryType).Distinct().OrderBy(x => x).ToList();

            ddl.Items.Clear();
            if (services != null && services.Count > 0)
            {
                foreach (var item in Categories)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        ListItem category = new ListItem();

                        category.Text =  item ;
                        category.Value = "0";
                        category.Attributes.Add("Style", "color:#000;background:#f3f1f1;Font-size:14px; ");
                        category.Attributes.Add("Disabled", "true");
                        category.Attributes.Add("class", "category_row");
                        ddl.Items.Add(category);
                    }

                    foreach (var service in services.Where(x => x.ServiceCategoryType.EqualsIgnoreCase(item)))
                    {
                        ListItem srv = new ListItem();
                        srv.Text = "-" +  service.Title;
                        srv.Value = Convert.ToString(service.ID);
                        srv.Attributes["OptionGroup"] = item;

                        ddl.Items.Add(srv);
                    }
                }

            }
            ddl.Items.Insert(0, new ListItem("None", ""));
        }

        public void RefreshCache()
        {
            List<Services> lstServices = this.Load();
            if (lstServices == null || lstServices.Count == 0)
                return;

            foreach (Services service in lstServices)
            {
                LoadItem(service, true, true, true);
                CacheHelper<Services>.AddOrUpdate(service.Title, this.dbContext.TenantID, service);
            }
        }

        public SLAConfiguration GetSLAConfiguration(string slaConfigration)
        {
            SLAConfiguration slaconfig = new SLAConfiguration();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(slaConfigration);
                slaconfig = (SLAConfiguration)uHelper.DeSerializeAnObject(xmlDoc, slaconfig);

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "GetSLAConfiguration");
            }

            return slaconfig;
        }
        public Services LoadServiceByTitle(string title,bool excludeDeleted=false)
        {
            Services svr = null;
            List<Services> spItemColl = new List<Services>();
            spItemColl = Load(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase));
            if (excludeDeleted)
                spItemColl = Load(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase) && !x.Deleted);
            if (spItemColl != null && spItemColl.Count > 0)
            {
                svr = LoadItem(spItemColl[0], true, true, true);
            }

            return svr;
        }

    }
    interface IServicesManager : IManagerBase<Services>
    {

    }
}
