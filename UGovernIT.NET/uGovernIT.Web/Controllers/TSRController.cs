using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Web.Models;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/module")]
    public class TSRController : ApiController
    {

        private readonly string[] userFields = { "RequestorUser", "BusinessManagerUser", "StageActionUsersUser", "CreatedByUser", "ModifiedByUser", "InitiatorUser", "OwnerUser" };
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager _ticketManager = null;
        private ApplicationContext _applicationContext;
        private TenantManager _tenantManager;
        ConfigurationVariableManager configurationVariableManager = null;
        private bool apiAllowPartialUpdate;
        public TSRController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _tenantManager = new TenantManager(_applicationContext);
            configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
            apiAllowPartialUpdate = configurationVariableManager.GetValueAsBool(ConfigConstants.APIAllowPartialUpdate);

        }
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(_applicationContext);
                }
                return _moduleViewManager;
            }
        }
        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(_applicationContext);
                }
                return _ticketManager;
            }
        }



        [HttpPost]
        [Authorize]
        [Route("NewRecord")]
        public async Task<IHttpActionResult> NewRecord([FromBody] List<NewTicketFieldViewModel> formFields)
        {
            await Task.FromResult(0);
            ULog.WriteLog("API call NewRecord initiated, " + "JSON: " + JsonConvert.SerializeObject(formFields));
            var moduleName = formFields.Where(f => f.FieldName == "Module").Select(s => s.Value.ToString()).FirstOrDefault();
            if (string.IsNullOrEmpty(moduleName))
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Module should not be empty or null in >>" + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module should not be empty or null" } });
            }
            UGITModule module = ModuleViewManager.LoadByName(moduleName);
            if (module == null)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Module not valid >>" + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module not valid" } });
            }
            var newTicket = TicketManager.GetTableSchemaDetail(module.ModuleTable, string.Empty).NewRow();
            List<TicketColumnError> errors = new List<TicketColumnError>();
            var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            List<TicketColumnValue> formValues = ParseJson(formFields, UserManager, errors);

            //Based on config variable allow partial update 

            List<string> lstOfError = new List<string>();
            if (errors.Count > 0)
            {
                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList());
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Error Occured in ParseJson method called by " + _applicationContext.CurrentUser.Name + "Errors: " + string.Join(Constants.Separator1, errors.Select(s => s.InternalFieldName + ": " + s.Message)) + "JSON: " + JsonConvert.SerializeObject(formFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                if (!apiAllowPartialUpdate)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
            }

            //Check pass fields exist or not
            List<TicketColumnError> invalidCols = new List<TicketColumnError>();
            formValues.ForEach(x =>
            {
                if (!UGITUtility.IfColumnExists(newTicket, x.InternalFieldName) && x.InternalFieldName != "Module" && !x.InternalFieldName.EqualsIgnoreCase("ExternalTeam") && !x.InternalFieldName.EqualsIgnoreCase("Data"))
                    invalidCols.Add(TicketColumnError.AddError(x.InternalFieldName, "", "Invalid field name"));
            });

            if (invalidCols.Count > 0)
            {
                //ULog.WriteException("NewRecord: "+ _applicationContext.CurrentUser.Name + "Invalid Columns"+ string.Join(Constants.Separator1, invalidCols.Select(s => s.InternalFieldName + ": " + s.Message))+ "JSON: " + JsonConvert.SerializeObject(formFields));
                //ULog.WriteUGITLog(_applicationContext.CurrentUser.Name+ "NewRecord: Invalid Columns", string.Join(Constants.Separator1, invalidCols.Select(s => s.InternalFieldName + ": " + s.Message)) + "JSON: " + JsonConvert.SerializeObject(formFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                if (!apiAllowPartialUpdate)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = invalidCols.Select(s => s.InternalFieldName + ": " + s.Message).ToList() });
            }
            //Get actual mpped id for Lookup,User
            List<TicketColumnError> columnErrors = new List<TicketColumnError>();
            formValues = MappedValues(formValues, module, columnErrors, formFields, _applicationContext);
            if (columnErrors.Count > 0)
            {
                lstOfError.Clear();
                lstOfError.AddRange(columnErrors.Select(s => s.InternalFieldName + ": " + s.Message).ToList());
                //ULog.WriteUGITLog(_applicationContext.CurrentUser.Id ,"NewRecord: Invalid Columns >> " +string.Join(Constants.Separator1, lstOfError) + " JSON: " + JsonConvert.SerializeObject(formFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                if (!apiAllowPartialUpdate)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
            }
            var user = HttpContext.Current.CurrentUser();
            var TicketRequest = new Ticket(_applicationContext, module.ModuleName, user);
            var IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);

            TicketColumnValue externalTeam = formValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam")).FirstOrDefault();
            formValues.RemoveAll(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam"));

            errors.Clear();
            if (moduleName == ModuleNames.SVC)
                newTicket[DatabaseObjects.Columns.ID] = 0;

            TicketRequest.Validate(formValues, newTicket, errors, /*ignoreMandatory*/ false, IsAdmin, 1);

            if (errors.Count > 0)
            {
                columnErrors.ForEach(x =>
                {
                    if (errors.Any(y => y.InternalFieldName == x.InternalFieldName))
                    {
                        x.Type = ErrorType.Mandatory;
                    }
                });

                if (columnErrors.Count > 0)
                    columnErrors.AddRange(errors.Where(x => !columnErrors.Any(y => x.InternalFieldName == y.InternalFieldName)).ToList());

                lstOfError.AddRange(columnErrors.Select(s => s.InternalFieldName + ": " + s.Type + " - " + s.Message).ToList());
                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Type + " - " + s.Message).ToList());
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Error occurred while validating formValues and newTicket in >> " + _applicationContext.CurrentUser.Name + " >> " + string.Join(Constants.Separator1, lstOfError), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
            }

            errors = columnErrors;
            errors.AddRange(invalidCols);
            //if (errors.Count > 0)
            //    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Invalid Columns while creating ticket by " + _applicationContext.CurrentUser.Name +" >> "+ UGITUtility.ConvertListToString(errors.Select(x => x.InternalFieldName + ": " + x.Message).ToList(), Constants.Separator1) + " >> JSON: " + JsonConvert.SerializeObject(formFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);

            TicketRequest.SetItemValues(newTicket, formValues, IsAdmin, /*updateChangesInHistory*/ false, user.Id);
            newTicket = uHelper.FillShortNameIfExists(newTicket, _applicationContext);

            try
            {
                if (moduleName == ModuleNames.SVC)
                {
                    newTicket["TicketId"] = CreateServiceTicket(formValues, formFields);
                }
                else
                {
                    TicketRequest.Create(newTicket, user);
                    if (SetTicketStage(newTicket, formFields, module.ModuleName) == false)
                        TicketRequest.SetNextStage(newTicket);
                    string result = TicketRequest.CommitChanges(newTicket);
                    if (!string.IsNullOrEmpty(result))
                        ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Ticket creation failed by " + _applicationContext.CurrentUser.Name + " >> Ticket Details: " + UGITUtility.ObjectToString(newTicket[DatabaseObjects.Columns.TicketId]) + ": " + UGITUtility.ObjectToString(newTicket[DatabaseObjects.Columns.Title]), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    else
                    {
                        if (errors.Count > 0)
                            ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Ticket has been created by " + _applicationContext.CurrentUser.Name + " >> Ticket Details: " + UGITUtility.ObjectToString(newTicket[DatabaseObjects.Columns.TicketId]) + ": " + UGITUtility.ObjectToString(newTicket[DatabaseObjects.Columns.Title]) + " but Error occured while creating ticket by " + _applicationContext.CurrentUser.Name + " >> " + UGITUtility.ConvertListToString(errors.Select(x => x.InternalFieldName + ": " + x.Message).ToList(), Constants.Separator1) + " >> JSON: " + JsonConvert.SerializeObject(formFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    }

                    if (string.IsNullOrEmpty(result) && externalTeam != null)
                    {
                        ThreadStart threadStartMethod = delegate () { AddUpdateExternalTeam(Convert.ToString(newTicket["TicketId"]), externalTeam, false); };
                        Thread sThread = new Thread(threadStartMethod);
                        sThread.IsBackground = true;
                        sThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "NewRecord: Error occurred while creating ticket by >> " + "JSON: " + JsonConvert.SerializeObject(formFields) + "ExceptionMessage:" + ex.ToString() + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
            }
            ULog.WriteLog("API call NewRecord ended successfully !");
            TicketRequest.SendEmailToActionUsers(Convert.ToString(newTicket[DatabaseObjects.Columns.ModuleStepLookup]), newTicket, "1", performedAction: Convert.ToString(TicketActionType.Created));

            return Ok(new CommonTicketResponse { Status = true, ErrorMessages = errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList(), Data = newTicket["TicketId"].ToString() });
        }

        private string CreateServiceTicket(List<TicketColumnValue> formValues, List<NewTicketFieldViewModel> formFields)
        {
            ServicesManager serviceManager = new ServicesManager(_applicationContext);
            ServiceRequestBL requestBL = new ServiceRequestBL(_applicationContext);

            string ServiceTitle = formFields.Where(f => f.FieldName == "Title").Select(s => s.Value.ToString()).FirstOrDefault();

            var services = serviceManager.Load(x => x.Title.EqualsIgnoreCase(ServiceTitle)).FirstOrDefault();

            ServiceRequestDTO serviceRequestObj = new ServiceRequestDTO();
            serviceRequestObj.ServiceId = services.ID;

            TicketColumnValue questions = formValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("Data")).FirstOrDefault();

            List<QuestionsDTO> lstQuestions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsDTO>>(Convert.ToString(questions.Value));

            List<QuestionsDTO> replies = new List<QuestionsDTO>();
            QuestionsDTO question = null;
            foreach (var item in lstQuestions)
            {
                question = new QuestionsDTO();
                question.Token = item.Token;
                question.Value = item.Value;
                replies.Add(question);
            }
            serviceRequestObj.Questions = replies;
            var result = requestBL.CreateService(serviceRequestObj);

            if (result != null)
                return result.TicketId;
            else
                return string.Empty;
        }

        private bool SetTicketStage(DataRow newTicket, List<NewTicketFieldViewModel> formFields, string moduleName)
        {
            bool isTicketStageSet = false;
            try
            {
                var stage = formFields.FirstOrDefault(x => x.FieldName.EqualsIgnoreCase(DatabaseObjects.Columns.Status));
                var step = formFields.FirstOrDefault(x => x.FieldName.EqualsIgnoreCase(DatabaseObjects.Columns.StageStep));
                LifeCycleStageManager sourceMgr = new LifeCycleStageManager(_applicationContext);
                LifeCycleStage lifeCycleStage = null;

                if (stage != null)
                    lifeCycleStage = sourceMgr.Load(x => x.Name == UGITUtility.ObjectToString(stage.Value) && x.ModuleNameLookup == moduleName).FirstOrDefault();

                if (step != null)
                    lifeCycleStage = sourceMgr.Load(x => x.StageStep == UGITUtility.StringToInt(step.Value) && x.ModuleNameLookup == moduleName).FirstOrDefault();

                if (lifeCycleStage != null)
                {
                    newTicket[DatabaseObjects.Columns.TicketStatus] = lifeCycleStage.Name;
                    newTicket[DatabaseObjects.Columns.StageStep] = lifeCycleStage.StageStep;
                    newTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = lifeCycleStage.ActionUser;
                    newTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_applicationContext, Convert.ToString(newTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), newTicket);
                    newTicket[DatabaseObjects.Columns.DataEditors] = lifeCycleStage.DataEditors;
                    isTicketStageSet = true;
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SetTicketStage: " + ex);
            }
            return isTicketStageSet;
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateRecord")]
        public async Task<IHttpActionResult> UpdateRecord([FromBody] UpdateTicketViewModel model)
        {
            await Task.FromResult(0);
            string moduleName;
            ULog.WriteLog("API call UpdateRecord initiated, "+"JSON: " + JsonConvert.SerializeObject(model));
            List<TicketColumnError> errors = new List<TicketColumnError>();
            List<string> lstOfError = new List<string>();
            try
            {
                moduleName = model.RecordId.Substring(0, 3);
            }
            catch (Exception ex)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Missing RecordId >> " + "JSON: " + JsonConvert.SerializeObject(model) + "ExceptionMessage:" + ex.ToString() + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Missing RecordId" } });
            }
            if (string.IsNullOrEmpty(moduleName))
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Module should not be empty or null >> " + "JSON: " + JsonConvert.SerializeObject(model) + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module should not be empty or null" } });
            }

            UGITModule module = ModuleViewManager.LoadByName(moduleName);
            if (module == null)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Module should not be empty or null >> " + "JSON: " + JsonConvert.SerializeObject(model) + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module does not exist" } });
            }
            var ticket = Ticket.GetCurrentTicket(_applicationContext, moduleName, model.RecordId);
            if (ticket == null)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Record not found with this recordID >> " + "JSON: " + JsonConvert.SerializeObject(model) + " >> " + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Record not found with this recordID" } });
            }
            var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            List<TicketColumnValue> formValues = ParseJson(model.Fields, UserManager, errors);
            //Based on config variable allow partial update 
            if (errors.Count > 0)
            {
                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList());
                if (!apiAllowPartialUpdate)
                {
                    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: tried to parse data >> " + "JSON: " + JsonConvert.SerializeObject(model) + "ExceptionMessage:" + string.Join(",", errors) + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
                }
            }

            //Check pass fields exist or not
            formValues.ForEach(x =>
            {
                if (!UGITUtility.IfColumnExists(ticket, x.InternalFieldName) && !x.InternalFieldName.EqualsIgnoreCase("ExternalTeam"))
                    errors.Add(TicketColumnError.AddError(x.InternalFieldName, "", "Invalid field name"));
            });

            if (errors.Count > 0)
            {
                lstOfError.Clear();
                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList());
                if (!apiAllowPartialUpdate)
                {
                    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Error occured while updating ticket by >> " + UGITUtility.ConvertListToString(errors.Select(x => x.Message).ToList(), Constants.Separator1) + _applicationContext.CurrentUser.Name + " JSON: " + JsonConvert.SerializeObject(model), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList() });
                }
            }
            //Get actual mpped id for Lookup,User
            formValues = MappedValues(formValues, module, errors, model.Fields, _applicationContext);
            if (errors.Count > 0)
            {
                lstOfError.Clear();
                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList());
                if (!apiAllowPartialUpdate)
                {
                    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Error occured while updating ticket by >> " + UGITUtility.ConvertListToString(errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList(), Constants.Separator1) + _applicationContext.CurrentUser.Name + "JSON: " + JsonConvert.SerializeObject(model), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
                }
            }

            var user = HttpContext.Current.CurrentUser();
            var TicketRequest = new Ticket(_applicationContext, moduleName, user);
            var IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);

            TicketColumnValue externalTeam = formValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam")).FirstOrDefault();
            formValues.RemoveAll(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam"));

            errors.Clear();
            TicketRequest.Validate(formValues, ticket, errors, /*ignoreMandatory*/ false, IsAdmin, 1);
            if (errors.Count > 0)
            {
                errors.ForEach(x =>
                {
                    if (errors.Any(y => y.InternalFieldName == x.InternalFieldName))
                    {
                        x.Type = ErrorType.Mandatory;
                    }
                });

                if (errors.Count > 0)
                    errors.AddRange(errors.Where(x => !errors.Any(y => x.InternalFieldName == y.InternalFieldName)).ToList());

                lstOfError.AddRange(errors.Select(s => s.InternalFieldName + ": " + s.Type + " - " + s.Message).ToList());
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
            }

            if (lstOfError.Count > 0)
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Error occured while updating ticket by " + _applicationContext.CurrentUser.Name + " >>  ExceptionMessage: " + UGITUtility.ConvertListToString(lstOfError, Constants.Separator1) + " JSON: " + JsonConvert.SerializeObject(model), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);

            TicketRequest.SetItemValues(ticket, formValues, IsAdmin, /*updateChangesInHistory*/ true, user.Id, false, true);
            TicketRequest.CheckRequestType(ticket, false);
            SetTicketStage(ticket, model.Fields, module.ModuleName);
            try
            {
                TicketRequest.CommitChanges(ticket);
            }
            catch (Exception ex)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "UpdateRecord: Error occured  >> " + _applicationContext.CurrentUser.Name + "ExecptionMessage: " + ex.ToString() + " JSON: " + JsonConvert.SerializeObject(model), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
            }
            if (externalTeam != null)
            {
                ThreadStart threadStartMethod = delegate () { AddUpdateExternalTeam(model.RecordId, externalTeam, true); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
            ULog.WriteLog("API call UpdateRecord ended successfully !");
            return Ok(new CommonTicketResponse { Status = true, ErrorMessages = lstOfError });
        }

        public List<TicketColumnValue> ParseJson(List<NewTicketFieldViewModel> formFields, UserProfileManager userManager, List<TicketColumnError> messages)
        {
            List<TicketColumnValue> ctrValues = new List<TicketColumnValue>();
            object fieldValue = null; string tenantId = null;
            try
            {
                var tenantValue = formFields.Where(f => f.FieldName == "TenantID").Select(s => s.Value).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(tenantValue)) || _applicationContext != null)
                    tenantValue = _applicationContext.TenantAccountId;
                if (tenantValue != null)
                {
                    var tenant = _tenantManager.GetTenant(tenantValue.ToString());
                    if (tenant != null)
                    {
                        tenantId = tenant.TenantID.ToString();
                    }
                    else messages.Add(TicketColumnError.AddError("TenantID", "", " Invalid value"));
                }
                foreach (var field in formFields)
                {
                    if (tenantId != null && (userFields.Contains(field.FieldName)))
                    {
                        var user = userManager.FindByName(field.Value.ToString(), tenantId);
                        if (user != null)
                        {
                            fieldValue = user.Id;
                            field.IsExcluded = true;
                        }
                        else messages.Add(TicketColumnError.AddError(field.FieldName, "", " Invalid value"));
                    }
                    else if (tenantId != null && field.FieldName == "TenantID")
                    {
                        fieldValue = tenantId;
                    }
                    else
                    {
                        fieldValue = field.Value;
                    }
                    if (fieldValue != null)
                    {
                        TicketColumnValue columnV = ctrValues.FirstOrDefault(x => x.InternalFieldName == field.FieldName);
                        if (columnV == null)
                            columnV = new TicketColumnValue();

                        columnV.InternalFieldName = field.FieldName;
                        columnV.Value = fieldValue;
                        columnV.TabNumber = 0;
                        ctrValues.Add(columnV);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ParseJson: " + ex);
            }
            return ctrValues;
        }

        [HttpGet]
        [Authorize]
        [Route("Records/{moduleName}")]
        public async Task<IHttpActionResult> Records(string moduleName, [FromBody] List<string> viewFields)
        {
            //if (moduleName.EqualsIgnoreCase("_CON"))
            //  moduleName = moduleName.Replace("_", " ").Trim();
            await Task.FromResult(0);
            ULog.WriteLog("API call Records initiated, " + "JSON: " + JsonConvert.SerializeObject(viewFields));
            try
            {

                UGITModule module = ModuleViewManager.LoadByName(moduleName);
                if (module == null)
                {
                    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "Records: Module not valid >>" + _applicationContext.CurrentUser.Name, Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module not valid" } });
                }
                var tickets = TicketManager.GetAllTickets(module, viewFields);
                if (tickets == null)
                {
                    ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "Records: Data not found >>" + _applicationContext.CurrentUser.Name + " JSON: " + JsonConvert.SerializeObject(viewFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "Data not found" }, Data = null };
                }
                if (viewFields == null || viewFields.Count == 0)
                {
                    tickets = GetNotNullFilterTable(tickets);
                }
                ULog.WriteLog("API call Records ended successfully !");
                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = tickets });

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Records: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Record/{recordID}")]
        public async Task<IHttpActionResult> GetRecord(string recordID, [FromBody] List<string> viewFields)
        {
            await Task.FromResult(0);
            ULog.WriteLog("API call GetRecord initiated, " + "JSON: " + JsonConvert.SerializeObject(viewFields));
            string moduleName;
            try
            {
                moduleName = recordID.Substring(0, 3);
            }
            catch (Exception)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "GetRecord: Invalid ticket Id >>" + _applicationContext.CurrentUser.Name + " JSON: " + JsonConvert.SerializeObject(viewFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Invalid ticket Id" } });
            }
            UGITModule module = ModuleViewManager.LoadByName(moduleName);
            if (module == null)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "GetRecord: Module not valid >>" + _applicationContext.CurrentUser.Name + " JSON: " + JsonConvert.SerializeObject(viewFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module not valid" } });
            }
            var ticket = Ticket.GetCurrentTicket(_applicationContext, moduleName, recordID, viewFields);
            if (ticket == null)
            {
                ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, "GetRecord: Ticket not found >> " + _applicationContext.CurrentUser.Name + " >> JSON: " + JsonConvert.SerializeObject(viewFields), Convert.ToString(UGITLogSeverity.Error), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Ticket not found" } });
            }
            if (viewFields == null || viewFields.Count == 0)
            {
                var dt = ticket.Table;
                dt = GetNotNullFilterTable(dt);
                ticket = dt.Rows[0];
            }
            ULog.WriteLog("API call GetRecord ended successfully !");
            return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = SetUserAndLookupValues(ticket) });
        }

        [HttpGet]
        [Authorize]
        [Route("GetServices")]
        public async Task<IHttpActionResult> GetServices()
        {
            await Task.FromResult(0);
            try
            {
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                ServicesManager serviceManager = new ServicesManager(_applicationContext);
                var services = serviceManager.Load(x => x.Deleted == false).Select(x => new { Title = x.Title, Description = x.ServiceDescription, Category = x.ServiceCategoryType ?? string.Empty, AuthorizedToRun = UserManager.GetUserNamesById(x.AuthorizedToView), Active = x.IsActivated }).OrderBy(y => y.Title).ThenBy(z => z.Active).ToList();

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = services });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetServices: " + ex);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetServiceDetails")]
        public async Task<IHttpActionResult> GetServiceDetails(string title)
        {
            await Task.FromResult(0);
            if (string.IsNullOrEmpty(title))
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Invalid or Empty Service name" } });

            try
            {
                ServicesManager serviceManager = new ServicesManager(_applicationContext);
                ServiceQuestionManager srvQuestManager = new ServiceQuestionManager(_applicationContext);
                ServiceSectionManager objServiceSectionManager = new ServiceSectionManager(_applicationContext);
                var services = serviceManager.Load(x => x.Title.EqualsIgnoreCase(title)).FirstOrDefault();
                List<ServiceQuestion> serviceQuestions = srvQuestManager.Load(x => x.ServiceID == services.ID);
                List<ServiceSection> serviceSections = objServiceSectionManager.Load(x => x.ServiceID == services.ID);
                ServiceSection section = null;
                serviceQuestions.ForEach(
                    x =>
                    {
                        section = serviceSections.FirstOrDefault(y => y.ID == x.ServiceSectionID);
                        if (section != null)
                            x.ServiceSectionName = section.Title;
                    });
                var result = serviceQuestions.Select(x => new { Question = x.QuestionTitle, Token = x.TokenName, QuestionType = x.QuestionType, AvailableOptions = GetAvailableOptions(x.QuestionType, x.QuestionTypeProperties), Section = x.ServiceSectionName, Helptext = x.Helptext, Mandatory = x.FieldMandatory });

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = result });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetServiceDetails: " + ex);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        private string GetAvailableOptions(string questionType, string questionTypeProperties)
        {
            try
            {
                List<string> attributes = questionTypeProperties.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (questionType == "SingleChoice" || questionType == "MultiChoice")
                {
                    string options = attributes.Where(x => x.Contains("options=")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(options))
                    {
                        options = options.Replace("options=", "").Replace("~", ",");
                        return options;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (questionType == "Lookup")
                {
                    DataTable dt = new DataTable();

                    string options = attributes.Where(x => x.Contains("lookuplist=")).FirstOrDefault();
                    string module = attributes.Where(x => x.Contains("module=")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(options))
                    {
                        string lookupField = options.Replace("lookuplist=", "");

                        if (!string.IsNullOrEmpty(module))
                            module = module.Replace("module=", "");

                        FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_applicationContext);
                        FieldConfiguration config = fieldConfigurationManager.GetFieldByFieldName(lookupField);

                        if (config != null)
                        {
                            if (string.IsNullOrEmpty(module))
                                dt = GetTableDataManager.GetTableData(config.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{_applicationContext.TenantID}' and {DatabaseObjects.Columns.Deleted} = '0'", config.ParentFieldName, null);
                            else
                                dt = GetTableDataManager.GetTableData(config.ParentTableName, $"{DatabaseObjects.Columns.ModuleNameLookup} = '{module}' and {DatabaseObjects.Columns.TenantID} = '{_applicationContext.TenantID}' and {DatabaseObjects.Columns.Deleted} = '0'", config.ParentFieldName, null);

                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            return string.Join(",", dt.AsEnumerable().Select(r => r.Field<string>(config.ParentFieldName)).ToArray());
                        }

                        return "";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (questionType == "Checkbox")
                {
                    return "True,False";
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetAvailableOptions: " + ex);
            }
            return string.Empty;
        }

        private DataTable GetNotNullFilterTable(DataTable dt)
        {
            foreach (var column in dt.Columns.Cast<DataColumn>().ToArray())
            {
                if (dt.AsEnumerable().All(dr => dr.IsNull(column)))
                    dt.Columns.Remove(column);
            }
            return dt;
        }

        public DataTable SetUserAndLookupValues(DataRow data)
        {
            try
            {
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_applicationContext);
                DataTable dtNewTable = data.Table.Clone();
                foreach (DataColumn item in data.Table.Columns)
                {
                    if (userFields.Contains(item.ColumnName))
                    {
                        var username = UserManager.GetUserNamesById(data[item.ColumnName].ToString());
                        if (username != null)
                        {
                            data[item.ColumnName] = username;
                        }
                        else data[item.ColumnName] = string.Empty;
                    }
                    else if (item.ColumnName == "TenantID")
                    {
                        var Tenant = _tenantManager.GetTenantById(data[item.ColumnName].ToString());
                        if (Tenant != null)
                        {
                            data[item.ColumnName] = Tenant.AccountID;
                        }
                        else data[item.ColumnName] = string.Empty;
                    }
                    else if (item.ColumnName.EndsWith("Lookup"))
                    {
                        if (data[item.ColumnName].GetType().FullName != "System.String")
                            dtNewTable.Columns[item.ColumnName].DataType = typeof(System.String);
                    }
                }

                foreach (DataRow row in data.Table.Rows)
                    dtNewTable.ImportRow(row);

                foreach (DataColumn item in dtNewTable.Columns)
                {
                    if (item.ColumnName.EndsWith("Lookup"))
                    {
                        dtNewTable.Rows[0][item.ColumnName] = fieldConfigurationManager.GetFieldConfigurationData(item.ColumnName, Convert.ToString(data[item.ColumnName]));
                    }
                }

                //return data.Table;
                return dtNewTable;
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SetUserAndLookupValues: " + ex);
                return null;
            }
        }

        /// <summary>
        /// It is being used to mapp Lookup & User id corresponding passed value
        /// </summary>
        /// <param name="columnValues"></param>
        /// <param name="module"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<TicketColumnValue> MappedValues(List<TicketColumnValue> columnValues, UGITModule module, List<TicketColumnError> columnErrors, List<NewTicketFieldViewModel> formFields, ApplicationContext context)
        {
            ModuleFormLayoutManager formLayoutManager = new ModuleFormLayoutManager(_applicationContext);
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_applicationContext);
            List<TicketColumnValue> updateFormVals = new List<TicketColumnValue>();
            List<string> lstOfAllowedFields = new List<string>() { "Lookup", "Choice", "UserGroup", "User", "ModuleType", "MultiLookup" };
            TicketColumnValue colVal = null;
            ModuleFormLayout formLayout = null;
            bool divisionLookupAdded = false;
            try
            {
                foreach (TicketColumnValue columnVal in columnValues)
                {
                    if (columnVal.InternalFieldName.EqualsIgnoreCase("ExternalTeam") || columnVal.InternalFieldName.EqualsIgnoreCase("Data"))
                        continue;
                    if (columnVal.InternalFieldName == DatabaseObjects.Columns.DivisionLookup && divisionLookupAdded)
                        continue; // Do not add DivisionLookup if already added.

                    colVal = new TicketColumnValue();
                    colVal = columnVal;

                    formLayout = formLayoutManager.Load(x => x.FieldName.EqualsIgnoreCase(colVal.InternalFieldName) && x.ModuleNameLookup == module.ModuleName).FirstOrDefault();
                    if (formLayout != null)
                        colVal.DisplayName = formLayout.FieldDisplayName;
                    else
                        colVal.DisplayName = colVal.InternalFieldName;

                    updateFormVals.Add(colVal);
                    NewTicketFieldViewModel newTicketFieldViewModel = formFields.FirstOrDefault(x => x.FieldName.EqualsIgnoreCase(columnVal.InternalFieldName));
                    if (!lstOfAllowedFields.Any(x => columnVal.InternalFieldName.EndsWith(x)) || (newTicketFieldViewModel != null && newTicketFieldViewModel.IsExcluded))
                        continue;

                    FillColValues(columnVal, colVal, module, formFields, columnErrors, fieldConfigurationManager);
                    //Field specific code
                    if (colVal.InternalFieldName == DatabaseObjects.Columns.StudioLookup && !string.IsNullOrEmpty(UGITUtility.ObjectToString(colVal.Value)))
                    {
                        bool enableStudioDivisionHierarchy = configurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
                        if (enableStudioDivisionHierarchy)
                        {
                            StudioManager objStudioManager = new StudioManager(context);
                            long divisionID = objStudioManager.GetDivisionIdForStudio(UGITUtility.StringToLong(colVal.Value));
                            //Find if DivisionLookup is added in the array
                            TicketColumnValue divisionColumnValue = updateFormVals.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.DivisionLookup);

                            if (divisionColumnValue == null)
                            {
                                TicketColumnValue colValDivision = new TicketColumnValue();
                                //colValDivision = columnVal;
                                colValDivision.InternalFieldName = DatabaseObjects.Columns.DivisionLookup;
                                colValDivision.Value = divisionID;

                                formLayout = formLayoutManager.Load(x => x.FieldName.EqualsIgnoreCase(colValDivision.InternalFieldName) && x.ModuleNameLookup == module.ModuleName).FirstOrDefault();
                                if (formLayout != null)
                                    colValDivision.DisplayName = formLayout.FieldDisplayName;
                                else
                                    colValDivision.DisplayName = colValDivision.InternalFieldName;
                                updateFormVals.Add(colValDivision);
                                divisionLookupAdded = true;
                            }
                            else
                            {
                                divisionColumnValue.Value = divisionID;
                                divisionLookupAdded = true;
                            }
                        }
                    }
                }
                string fieldName = string.Empty;
                foreach (TicketColumnValue columnVal in columnValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam")))
                {
                    fieldName = columnVal.InternalFieldName;
                    List<ExternalTeamViewModel> externalTeam = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExternalTeamViewModel>>(Convert.ToString(columnVal.Value));
                    if (externalTeam != null && externalTeam.Count > 0)
                    {
                        foreach (var item in externalTeam)
                        {
                            colVal = new TicketColumnValue();
                            colVal = columnVal;
                            colVal.InternalFieldName = DatabaseObjects.Columns.CRMCompanyLookup;
                            colVal.Value = item.CRMCompanyLookup;
                            FillColValues(colVal, colVal, module, formFields, columnErrors, fieldConfigurationManager);
                            item.CRMCompanyLookup = Convert.ToString(colVal.Value);

                            colVal.InternalFieldName = DatabaseObjects.Columns.RelationshipTypeLookup;
                            colVal.Value = item.RelationshipTypeLookup;
                            FillColValues(colVal, colVal, module, formFields, columnErrors, fieldConfigurationManager);
                            item.RelationshipTypeLookup = Convert.ToString(colVal.Value);

                            colVal.InternalFieldName = DatabaseObjects.Columns.ContactLookup;
                            List<string> contacts = new List<string>();
                            foreach (var contact in item.ContactLookup)
                            {
                                colVal.Value = contact;
                                FillColValues(colVal, colVal, module, formFields, columnErrors, fieldConfigurationManager);
                                contacts.Add(Convert.ToString(colVal.Value));
                            }
                            item.ContactLookup.Clear();
                            item.ContactLookup.AddRange(contacts);
                        }
                    }
                    columnVal.Value = Newtonsoft.Json.JsonConvert.SerializeObject(externalTeam);
                    columnVal.InternalFieldName = fieldName;
                    updateFormVals.Add(columnVal);
                }

                var fields = columnValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("Data"));
                if (fields != null && fields.Count() > 0)
                {
                    ServicesManager serviceManager = new ServicesManager(_applicationContext);
                    ServiceQuestionManager srvQuestManager = new ServiceQuestionManager(_applicationContext);
                    string ServiceTitle = formFields.Where(f => f.FieldName == "Title").Select(s => s.Value.ToString()).FirstOrDefault();
                    var services = serviceManager.Load(x => x.Title.EqualsIgnoreCase(ServiceTitle)).FirstOrDefault();

                    List<ServiceQuestion> serviceQuestions = srvQuestManager.Load(x => x.ServiceID == services.ID);

                    foreach (TicketColumnValue columnVal in fields)
                    {
                        fieldName = columnVal.InternalFieldName;
                        List<QuestionsDTO> questions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsDTO>>(Convert.ToString(columnVal.Value));
                        ServiceQuestion question = null;
                        if (questions != null && questions.Count > 0)
                        {
                            foreach (var item in questions)
                            {
                                colVal = new TicketColumnValue();
                                colVal = columnVal;

                                question = serviceQuestions.FirstOrDefault(x => x.TokenName == item.Token);
                                if (question.QuestionType == "UserField")
                                {
                                    colVal.InternalFieldName = DatabaseObjects.Columns.Resource;
                                }
                                else if (question.QuestionType == "Lookup")
                                {
                                    string[] attr = question.QuestionTypeProperties.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                                    colVal.InternalFieldName = attr[0].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                }
                                else
                                {
                                    continue;
                                }


                                colVal.InternalFieldName = colVal.InternalFieldName;
                                colVal.Value = item.Value;
                                FillColValues(colVal, colVal, module, formFields, columnErrors, fieldConfigurationManager);
                                item.Value = Convert.ToString(colVal.Value);
                            }
                        }
                        columnVal.Value = Newtonsoft.Json.JsonConvert.SerializeObject(questions);
                        columnVal.InternalFieldName = fieldName;
                        updateFormVals.Add(columnVal);
                    }
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in MappedValues: " + ex);
            }
            return updateFormVals;
        }

        private void FillColValues(TicketColumnValue columnVal, TicketColumnValue colVal, UGITModule module, List<NewTicketFieldViewModel> formFields, List<TicketColumnError> columnErrors, FieldConfigurationManager fieldConfigurationManager)
        {
            if (string.IsNullOrEmpty(UGITUtility.ObjectToString(columnVal.Value)))
                return;
            DataTable dt = null;
            string moduleTable = string.Empty;
            if (module != null)
                moduleTable = module.ModuleTable;
            TicketColumnError ticketColumnError = new TicketColumnError();
            try
            {
                dt = fieldConfigurationManager.GetFieldDataByFieldName(columnVal.InternalFieldName, moduleTable, ticketColumnError);
                if (dt == null || dt.Rows.Count == 0)
                {
                    if (UGITUtility.IfColumnExists(columnVal.InternalFieldName, module.ModuleTable) && !string.IsNullOrEmpty(UGITUtility.ObjectToString(columnVal.Value)))
                    {
                        //continue;
                        return;
                    }
                    else
                    {
                        string errorMsg = "Invalid field name";
                        if (!columnErrors.Any(x => x.InternalFieldName == UGITUtility.ObjectToString(colVal.InternalFieldName) && x.Message.Trim() == errorMsg))
                            columnErrors.Add(TicketColumnError.AddError(colVal.InternalFieldName, string.Empty, errorMsg));
                        colVal.Value = string.Empty;
                        //continue;
                        return;
                    }
                }

                DataRow row = null;
                if (UGITUtility.IfColumnExists(dt.NewRow(), DatabaseObjects.Columns.TenantID))
                    row = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.Title) == UGITUtility.ObjectToString(columnVal.Value) && x.Field<string>(DatabaseObjects.Columns.TenantID).EqualsIgnoreCase(_applicationContext.TenantID));
                else
                    row = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.Title) == UGITUtility.ObjectToString(columnVal.Value));

                if (row == null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketId, dt))
                {
                    row = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == UGITUtility.ObjectToString(columnVal.Value));
                }

                if (row == null)
                {
                    string errorMsg = "Invalid value";
                    if (columnVal.InternalFieldName.EndsWith("Lookup"))
                        errorMsg = "Invalid lookup value";

                    if (!columnErrors.Any(x => x.InternalFieldName == UGITUtility.ObjectToString(colVal.InternalFieldName) && x.Message.Trim() == errorMsg))
                        columnErrors.Add(TicketColumnError.AddError(colVal.InternalFieldName, string.Empty, errorMsg));
                    colVal.Value = string.Empty;
                    //continue;
                    return;
                }

                if (UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.TicketId))
                    colVal.Value = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                else
                    colVal.Value = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ID]);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in FillColValues: " + ex);
            }
        }

        private void AddUpdateExternalTeam(string TicketId, TicketColumnValue externalTeam, bool updateTeam = false)
        {
            try
            {
                RelatedCompanyManager relatedCompanyManager = new RelatedCompanyManager(_applicationContext);

                if (updateTeam == true)
                {
                    var relatedCompanies = relatedCompanyManager.Load(x => x.TicketID == TicketId).ToList();
                    if (relatedCompanies != null && relatedCompanies.Count > 0)
                        relatedCompanyManager.Delete(relatedCompanies);
                }

                List<ExternalTeamViewModel> teams = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExternalTeamViewModel>>(Convert.ToString(externalTeam.Value));
                foreach (var team in teams)
                {
                    RelatedCompany relatedCompany = new RelatedCompany();
                    relatedCompany.TicketID = TicketId;
                    relatedCompany.CRMCompanyLookup = team.CRMCompanyLookup;
                    relatedCompany.RelationshipTypeLookup = Convert.ToInt64(!string.IsNullOrEmpty(team.RelationshipTypeLookup) ? team.RelationshipTypeLookup : null);
                    relatedCompany.ContactLookup = string.Join(Constants.Separator6, team.ContactLookup);
                    relatedCompany.ItemOrder = 1;
                    relatedCompanyManager.Insert(relatedCompany);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException("Error occurred in AddUpdateExternalTeam method " + ex.ToString());
            }

        }
    }
}
