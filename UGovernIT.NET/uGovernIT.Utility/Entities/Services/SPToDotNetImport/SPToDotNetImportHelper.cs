using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using AutoMapper;
using System.Xml;
using Newtonsoft.Json;
namespace uGovernIT.Utility
{
    public class SPToDotNetImportHelper
    {
        ServiceExtension serviceExtension;
        SPServiceExtension sPServiceExtension;
        public Dictionary<long,string> ServiceCategories { get; set; }
        public string input { get; set; }
        public ServiceExtension ImportService(Stream stream)
        {
                try
                {
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(input);
                //var json = JsonConvert.SerializeObject(doc);
                //sPServiceExtension = new SPServiceExtension();
                //object obj1= JsonConvert.DeserializeObject(json);
                //sPServiceExtension = (SPServiceExtension)JsonConvert.DeserializeObject(json);
                DataContractSerializer deserializer = new DataContractSerializer(typeof(SPServiceExtension));
                sPServiceExtension = (SPServiceExtension)deserializer.ReadObject(stream);
                serviceExtension = ConvertSPExtToServiceExt(sPServiceExtension);
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex, "Error in Importing Service from sharepoint to dotnet.");
                }
            
            return serviceExtension;
        }
        public ServiceExtension ConvertSPExtToServiceExt(SPServiceExtension spServiceExtension)
        {
            ServiceExtension serviceExtension = new ServiceExtension();
            if (spServiceExtension != null)
            {
                spServiceExtension.Questions.ForEach(x => {
                    x.QuestionTypeProperties = x.QuestionTypeProperties.ReplaceInKeys("SPModule", "Module");
                });
                var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SPServiceExtension, ServiceExtension>()
                        .ForMember(dest=>dest.ServiceDescription,s=>s.MapFrom(src=>src.Description));
                        cfg.CreateMap<SPServiceQuestion, ServiceQuestion>()
                        .ForMember(dest=>dest.QuestionTypePropertiesDicObj,s=>s.MapFrom(src=>src.QuestionTypeProperties))
                        .ForMember(dest=>dest.FieldMandatory,s=>s.MapFrom(src=>src.Mandatory));
                        cfg.CreateMap<SPServiceSection, ServiceSection>()
                        .ForMember(dest=>dest.Title,s=>s.MapFrom(src=>src.SectionTitle))
                        .ForMember(dest=>dest.SectionName,s=>s.MapFrom(src=>src.SectionTitle));
                        cfg.CreateMap<SPVariableValue, VariableValue>();
                        cfg.CreateMap<SPQuestionMapVariable, QuestionMapVariable>();
                        cfg.CreateMap<SPServices, Services>()
                        .ForMember(dest=>dest.Deleted,s=>s.MapFrom(src=>src.IsDeleted));
                        cfg.CreateMap<SPServiceQuestionMapping, ServiceQuestionMapping>();
                        cfg.CreateMap<SPServiceSectionCondition, ServiceSectionCondition>();
                        cfg.CreateMap<SPServiceTaskCondition, ServiceTaskCondition>();
                        cfg.CreateMap<SPSLAConfiguration, SLAConfiguration>();
                        cfg.CreateMap<SPUserLookupValue, UserLookupValue>();
                        cfg.CreateMap<SPWhereExpression, WhereExpression>();
                        cfg.CreateMap<SPLookupValueServiceExtension, LookupValueServiceExtension>();
                        cfg.CreateMap<SPUserInfo, UserInfo>()
                        .ForMember(dest => dest.ID, s => s.MapFrom(src => src.ID));
                        cfg.CreateMap<ServiceTask, UGITTask>()
                        .ForMember(dest => dest.ParentTask, opt => opt.Ignore())
                        .ForMember(dest => dest.Module, opt => opt.Ignore())
                        .ForMember(dest => dest.Service, opt => opt.Ignore())
                        .ForMember(dest => dest.RequestCategory, opt => opt.Ignore())
                        .ForMember(dest => dest.RequestTypeCategory, s => s.MapFrom(src => Convert.ToString(src.RequestCategory.LookupId)))
                        .ForMember(dest => dest.SPPredecessor, s => s.MapFrom(src => src.Predecessors))
                        //.ForMember(dest => dest.ParentTaskID, s => s.MapFrom(src => src.SParentTask))
                        .ForMember(dest => dest.SPModule, s => s.MapFrom(src => src.Module))
                        .ForMember(dest => dest.Service, s => s.MapFrom(src => Convert.ToString(src.Service.LookupId)));


                    }).CreateMapper();

                serviceExtension = config.Map<ServiceExtension>(spServiceExtension);
                if (serviceExtension != null)
                {
                    string category = spServiceExtension.Category;
                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        var cate = ServiceCategories.FirstOrDefault(x =>!string.IsNullOrWhiteSpace(x.Value) && x.Value.Equals(category, StringComparison.InvariantCultureIgnoreCase));
                        if (cate.Key > 0)
                        {
                            serviceExtension.CategoryId = cate.Key;
                            serviceExtension.ServiceCategoryType = cate.Value;
                        }
                        if (category != Constants.ModuleFeedback && category != Constants.ModuleAgent)
                        {
                            serviceExtension.ServiceType = "Service";
                            if (serviceExtension.Tasks != null)
                                serviceExtension.Tasks.ForEach(x =>
                                {
                                    x.ModuleNameLookup = "SVCConfig";
                                });
                        }
                        else if (category == Constants.ModuleFeedback)
                            serviceExtension.ServiceType = Constants.ModuleFeedback;
                        else if (category == Constants.ModuleAgent)
                            serviceExtension.ServiceType = Constants.ModuleAgent;
                    }
                    LookupValueServiceExtension lookupValueServiceExtension = null;
                    
                    serviceExtension.Tasks.ForEach(x =>
                    {
                        if (x.SPModule!=null && serviceExtension.LookupValues != null)
                        {
                            lookupValueServiceExtension= serviceExtension.LookupValues.FirstOrDefault(y => y.ListName == "Modules" && y.ID ==Convert.ToString(x.SPModule.LookupId));
                            if (lookupValueServiceExtension != null)
                                x.RelatedModule = lookupValueServiceExtension.Value;

                            lookupValueServiceExtension = null;
                        }

                        x.Predecessors = string.Empty;
                        if (x.SPPredecessor != null && x.SPPredecessor.Count > 0)
                        {
                            List<string> lstOfPredecessor = x.SPPredecessor.Select(y =>UGITUtility.ObjectToString(y.LookupId)).ToList();
                            x.Predecessors = UGITUtility.ConvertListToString(lstOfPredecessor, Constants.Separator6);
                        }
                    });

                    if (serviceExtension.Questions != null)
                    {
                        serviceExtension.Questions.ForEach(x =>
                        {
                            if (x.QuestionType.Equals("lookup", StringComparison.InvariantCultureIgnoreCase))
                            {
                                string keyVal = x.QuestionTypePropertiesDicObj["lookuplist"];
                                if (!string.IsNullOrWhiteSpace(keyVal))
                                {
                                    keyVal = keyVal.StartsWith("ticket", StringComparison.InvariantCultureIgnoreCase) ? string.Format("{0}Lookup", keyVal.Replace("Ticket", "").Trim()) : string.Format("{0}Lookup", keyVal);
                                    x.QuestionTypePropertiesDicObj["lookuplist"] = keyVal;
                                }
                            }
                        });
                    }

                    if (serviceExtension.QuestionsMapping != null)
                    {
                        serviceExtension.QuestionsMapping.ForEach(x =>
                        {
                            x.ColumnName = x.ColumnName.StartsWith("Ticket", StringComparison.InvariantCultureIgnoreCase) ? x.ColumnName.Replace("Ticket", "").Trim() : x.ColumnName;
                        });
                    }
                }
            }
            return serviceExtension;
        }

        public List<UGITTask> LoadTasks(List<ServiceTask> serviceTasks)
        {
            List<UGITTask> uGITTasks = new List<UGITTask>();
            UGITTask uGITTask = null;
            foreach (ServiceTask serviceTask in serviceTasks)
            {
                uGITTask = new UGITTask();
                uGITTask.ID = serviceTask.ID;
                uGITTask.Title = serviceTask.Title;
                //uGITTask.Description = serviceTask.Description;
                //uGITTask.Created = serviceTask.Created;
                //uGITTask.Modified = serviceTask.Modified;
                //uGITTask.ParentTaskID = serviceTask.SParentTask;
                //uGITTask.TaskLevel = serviceTask.TaskLevel;
                //uGITTask.UGITSubTaskType =uGITTask.SubTaskType= serviceTask.UGITSubTaskType;
                //uGITTask.AutoCreateUser = serviceTask.AutoCreateUser;
                //uGITTask.AutoFillRequestor = serviceTask.AutoFillRequestor;
                //uGITTask.QuestionID = serviceTask.QuestionID;
                //uGITTask.QuestionProperties = serviceTask.QuestionProperties;
                //if (serviceTask.Predecessors != null)
                //    uGITTask.Predecessors =string.Join(Constants.Separator6,serviceTask.Predecessors.Select(x => x.LookupId));

                //if (serviceTask.RCategory != null)
                //    uGITTask.RequestTypeCategory =UGITUtility.ObjectToString(serviceTask.RCategory.LookupId);

                //if (serviceTask.SPModule != null && sPServiceExtension.LookupValues.Any(x => x.ID == UGITUtility.ObjectToString(serviceTask.SPModule.LookupId) && x.ListName == "Modules"))
                //    uGITTask.RelatedModule = sPServiceExtension.LookupValues.FirstOrDefault(x => x.ID == UGITUtility.ObjectToString(serviceTask.SPModule.LookupId) && x.ListName == "Modules").Value;

                //if (serviceTask.Service != null)
                //    uGITTask.Service = UGITUtility.ObjectToString(serviceTask.Service.LookupId);

                //if (serviceTask.AssignedTo != null)
                //    uGITTask.AssignedTo = string.Join(Constants.Separator6, serviceTask.AssignedTo.Select(x => x.LookupId));

                //uGITTask.ItemOrder= serviceTask.ItemOrder;
                //uGITTask.Weight = serviceTask.Weight;
                //uGITTask.EstimatedHours = serviceTask.EstimatedHours;
                //uGITTask.ModifiedBy= serviceTask.ModifiedBy;
                //uGITTask.Author = serviceTask.Author;
                //uGITTask.EnableApproval = serviceTask.EnableApproval;
                //if(serviceTask.Approver!=null)
                //uGITTask.Approver = string.Join(Constants.Separator6, serviceTask.Approver.Select(x => x.LookupId));
                //uGITTask.SLADisabled = serviceTask.SLADisabled;
                //uGITTask.NotificationDisabled = serviceTask.NotificationDisabled;
                uGITTasks.Add(uGITTask);
            }

            return uGITTasks;
        }
    }
}