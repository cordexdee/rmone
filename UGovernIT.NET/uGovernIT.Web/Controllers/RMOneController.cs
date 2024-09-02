using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Models;
using DevExpress.UnitConversion;
using AutoMapper;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Manager.RMM.ViewModel;
using DevExpress.ExpressApp;
using Antlr.Runtime.Misc;
using DevExpress.Xpo.DB;
using uGovernIT.Manager.Core;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/RMOne")]
    public class RMOneController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager ModuleManager;
        private ModuleViewManager _moduleViewManager = null;
        private TenantManager _tenantManager;
        private UserProfileManager _userProfileManager;
        private TicketManager _ticketManager;
        private ProjectEstimatedAllocationManager projectEstimatedAllocationMGR;
        private GlobalRoleManager _globalRoleManager;
        public List<GlobalRole> GlobalRoles { get; set; }
        protected string sourceURL;
        private DataRow moduleRow;


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
        public RMOneController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ModuleManager = new ModuleViewManager(_applicationContext);
            _tenantManager = new TenantManager(_applicationContext);
            _userProfileManager = new UserProfileManager(_applicationContext);
            projectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(_applicationContext);
            _ticketManager = new TicketManager(_applicationContext);
            _globalRoleManager = new GlobalRoleManager(_applicationContext);

        }
        [Route("GetResourceAllocationTrendChart")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetResourceAllocationTrendChart([FromUri] ExecutivePageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                DateTime StartDate = DateTime.Now.AddMonths(-3);
                DateTime EndDate = DateTime.Now.AddMonths(3);

                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    StartDate = Convert.ToDateTime(request.StartDate);
                    EndDate = Convert.ToDateTime(request.EndDate);
                }
                else
                {
                    switch (request.Filter)
                    {
                        case "HalfYearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-3);
                                EndDate = DateTime.Now.AddMonths(3);
                            }
                            break;
                        case "Yearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-6);
                                EndDate = DateTime.Now.AddMonths(6);
                            }
                            break;
                        case "TwoYear":
                            {
                                StartDate = DateTime.Now.AddMonths(-12);
                                EndDate = DateTime.Now.AddMonths(12);
                            }
                            break;
                        default:
                            break;
                    }
                }
                arrParams.Add("TenantID", _applicationContext.TenantID);
                arrParams.Add("Startdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams.Add("Enddate", EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams.Add("Division", UGITUtility.StringToLong(request.Division));
                arrParams.Add("Role", string.Empty);
                DataTable dtWorkforceAllocTrend = GetTableDataManager.GetData("WorkforceAllocationTrend", arrParams);
                if (dtWorkforceAllocTrend != null && dtWorkforceAllocTrend.Rows.Count > 0)
                {
                    IEnumerable<DataRow> sortedRows = dtWorkforceAllocTrend.AsEnumerable().OrderBy(x => x.Field<DateTime>("week"));
                    DataTable dtsortedtable = sortedRows.CopyToDataTable();
                    return Ok(dtsortedtable);
                }
                
            }
            catch (Exception e)
            {
                ULog.WriteException($"An Exception Occurred in GetResourceAllocationTrendChart: " + e);
            }
            return Ok("Data Not Found:GetResourceAllocationTrendChart");
        }
        [Route("GetKeyIndicators")]
        public async Task<IHttpActionResult> GetKeyIndicators([FromUri] ExecutivePageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                DateTime StartDate = DateTime.Now.AddMonths(-3);
                DateTime EndDate = DateTime.Now.AddMonths(3);
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    StartDate = Convert.ToDateTime(request.StartDate);
                    EndDate = Convert.ToDateTime(request.EndDate);
                }
                else
                {
                    switch (request.Filter)
                    {
                        case "HalfYearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-3);
                                EndDate = DateTime.Now.AddMonths(3);
                            }
                            break;
                        case "Yearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-6);
                                EndDate = DateTime.Now.AddMonths(6);
                            }
                            break;
                        case "TwoYear":
                            {
                                StartDate = DateTime.Now.AddMonths(-12);
                                EndDate = DateTime.Now.AddMonths(12);
                            }
                            break;
                        default:
                            break;
                    }
                }

                arrParams.Add("TenantID", _applicationContext.TenantID);
                arrParams.Add("Fromdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams.Add("Todate", EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams.Add("Division", int.Parse(request.Division));
                DataTable data = GetTableDataManager.GetData("BilledAndAcquisionHours", arrParams);

                Dictionary<string, object> arrParams1 = new Dictionary<string, object>();
                arrParams1.Add("TenantID", _applicationContext.TenantID);
                arrParams1.Add("Startdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams1.Add("Enddate", EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DataTable data1 = GetTableDataManager.GetData("OpportunityAndProjectCount", arrParams1);

                Dictionary<string, object> arrParams2 = new Dictionary<string, object>();
                arrParams2.Add("TenantID", _applicationContext.TenantID);
                arrParams2.Add("Fromdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                arrParams2.Add("Todate", EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DataTable data2 = GetTableDataManager.GetData("ProjectWonRation", arrParams2);

                KpiIndicators obj = new KpiIndicators()
                {
                    AcqHours = data.Rows[0]["AcquisionHours"].ToString(),
                    AcqRatio = data.Rows[0]["AcquisionRatio"].ToString(),
                    BilledHours = data.Rows[0]["BilledHours"].ToString(),
                    OpmLastMonth = data1.Rows[0]["LastMonthOpportunityCount"].ToString(),
                    OpmAverage = data1.Rows[0]["AverageMonthlyOpportunityCount"].ToString(),
                    CPRLastMonth = data1.Rows[0]["LastMonthProjectCount"].ToString(),
                    CPRAverage = data1.Rows[0]["AverageMonthlyProjectCount"].ToString(),
                    opmlost = "143",
                    cprwon = data2.Rows[0]["ProjectWonRation"].ToString(),
                    LastDay = "55%",
                    NextDay = "62%"
                };

                return Ok(obj);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetKeyIndicators: " + ex);
            }
            return Ok("An Exception Occurred in GetKeyIndicators");
        }

        [Route("GetResourceBillingData")]
        public async Task<IHttpActionResult> GetResourceBillingData([FromUri] ExecutivePageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                DataTable data = new DataTable();
                data.Columns.Add("Project Size", typeof(string));
                data.Columns.Add("Resource Hours", typeof(string));
                data.Columns.Add("# Projects", typeof(int));

                DataRow dr = null;
                dr = data.NewRow();
                dr[0] = "XL";
                dr[1] = "28.8K";
                dr[2] = 12;
                data.Rows.Add(dr);

                dr = data.NewRow();
                dr[0] = "M";
                dr[1] = "28.8K";
                dr[2] = 30;
                data.Rows.Add(dr);
                return Ok(data);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResourceBillingData: " + ex);
            }
            return Ok("An Exception Occurred in GetResourceBillingData");


        }

        [Route("GetStaffingChartData")]
        public async Task<IHttpActionResult> GetStaffingChartData([FromUri] ExecutivePageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                DateTime StartDate = DateTime.Now.AddMonths(-3);
                DateTime EndDate = DateTime.Now.AddMonths(3);
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    StartDate = Convert.ToDateTime(request.StartDate);
                    EndDate = Convert.ToDateTime(request.EndDate);
                }
                else
                {
                    switch (request.Filter)
                    {
                        case "HalfYearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-3);
                                EndDate = DateTime.Now.AddMonths(3);
                            }
                            break;
                        case "Yearly":
                            {
                                StartDate = DateTime.Now.AddMonths(-6);
                                EndDate = DateTime.Now.AddMonths(6);
                            }
                            break;
                        case "TwoYear":
                            {
                                StartDate = DateTime.Now.AddMonths(-12);
                                EndDate = DateTime.Now.AddMonths(12);
                            }
                            break;
                        default:
                            break;
                    }
                }
                values.Add("@Startdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                values.Add("@Enddate", EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                values.Add("@division", UGITUtility.StringToLong(request.Division));
                DataTable dt = GetTableDataManager.GetData("StaffingChartData", values);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStaffingChartData: " + ex);
            }
            return Ok("An Exception Occurred in GetStaffingChartData");
        }

        [Route("GetProjectChartData")]
        public async Task<IHttpActionResult> GetProjectChartData([FromUri] ProjectGanttPageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@division", UGITUtility.StringToLong(request.Division));
                values.Add("@studio", UGITUtility.ObjectToString(request.Studio));
                values.Add("@sector", UGITUtility.ObjectToString(request.Sector));
                values.Add("@closed", true);
                if (!string.IsNullOrEmpty(request.StartDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                if (!string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                DataTable dt = GetTableDataManager.GetData("ProjectChart", values);
                List<ProjectGanttBarResponse> barResponse = new List<ProjectGanttBarResponse>();
                ProjectGanttBarResponse response = new ProjectGanttBarResponse();
                response.OPM = response.CPR = response.TP = null;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString() == "Opportunities")
                        response.OPM = UGITUtility.StringToInt(dr[1]);
                    else if (dr[0].ToString() == "Construction")
                        response.CPR = UGITUtility.StringToInt(dr[1]);
                    else if (dr[0].ToString() == "Tracked Projects")
                        response.TP = UGITUtility.StringToInt(dr[1]);
                }
                barResponse.Add(response);
                return Ok(barResponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectChartData: " + ex);
            }
            return Ok("An Exception Occurred in GetProjectChartData");

        }

        [Route("GetUtilizationForecastData")]
        public async Task<IHttpActionResult> GetUtilizationForecastData([FromUri] string Year, int Division)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@Year", Year);
                values.Add("@division", Division);
                DataTable dt = GetTableDataManager.GetData("UtilizationForecastData", values);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUtilizationForecastData: " + ex);
            }
            return Ok("An Exception Occurred in GetUtilizationForecastData");
        }

        [Route("GetNCOHours")]
        public async Task<IHttpActionResult> GetNCOHours([FromUri] ExecutivePageRequest request)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                DateTime StartDate = DateTime.Now.AddMonths(-3);
                DateTime EndDate = DateTime.Now.AddMonths(3);
                switch (request.Period)
                {
                    case "HalfYearly":
                        {
                            StartDate = DateTime.Now.AddMonths(-3);
                            EndDate = DateTime.Now.AddMonths(3);
                        }
                        break;
                    case "Yearly":
                        {
                            StartDate = DateTime.Now.AddMonths(-6);
                            EndDate = DateTime.Now.AddMonths(6);
                        }
                        break;
                    case "TwoYear":
                        {
                            StartDate = DateTime.Now.AddMonths(-12);
                            EndDate = DateTime.Now.AddMonths(12);
                        }
                        break;
                    default:
                        break;
                }
                values.Add("@TenantID", _applicationContext.TenantID);
                values.Add("@Filter", request.Filter);
                values.Add("@Startdate", StartDate);
                values.Add("@Enddate", EndDate);
                DataTable dt = GetTableDataManager.GetData("NCOHours", values);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetNCOHours: " + ex);
            }
            return Ok("An Exception Occurred in GetNCOHours");

        }
        [Route("GetNextWorkingDateAndTime")]
        public async Task<IHttpActionResult> GetNextWorkingDateAndTime([FromUri] string dateString)
        {
            await Task.FromResult(0);
            try
            {
                string retValue = uHelper.GetNextWorkingDateAndTime(_applicationContext, UGITUtility.StringToDateTime(dateString));
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetNextWorkingDateAndTime: " + ex);
            }
            return Ok("An Exception Occurred in GetNextWorkingDateAndTime");
        }
        [Route("UpdateRecord")]
        public async Task<IHttpActionResult> UpdateRecord([FromBody] UpdateTicketViewModel model)
        {
            await Task.FromResult(0);
            string moduleName;
            try
            {
                moduleName = model.RecordId.Substring(0, 3);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateRecord: " + ex);
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Missing RecordId" } });
            }
            if (string.IsNullOrEmpty(moduleName))
            {
                return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module should not be empty or null" } });
            }

            try
            {

                UGITModule module = ModuleViewManager.LoadByName(moduleName);
                if (module == null)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Module does not exist" } });
                var ticket = Ticket.GetCurrentTicket(_applicationContext, moduleName, model.RecordId);
                if (ticket == null)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = new List<string>() { "Record not found with this recordID" } });
                List<TicketColumnError> errors = new List<TicketColumnError>();
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                List<TicketColumnValue> formValues = ParseJson(model.Fields, UserManager, errors);
                //Based on config variable allow partial update 
                List<string> lstOfError = new List<string>();


                //Check pass fields exist or not
                List<TicketColumnError> invalidCols = new List<TicketColumnError>();
                formValues.ForEach(x =>
                {
                    if (!UGITUtility.IfColumnExists(ticket, x.InternalFieldName) && !x.InternalFieldName.EqualsIgnoreCase("ExternalTeam"))
                        invalidCols.Add(TicketColumnError.AddError(x.InternalFieldName, "", "Invalid field name"));
                });


                //Get actual mpped id for Lookup,User
                List<TicketColumnError> columnErrors = new List<TicketColumnError>();
                formValues = MappedValues(formValues, module, columnErrors, model.Fields, _applicationContext);

                var user = HttpContext.Current.CurrentUser();
                var TicketRequest = new Ticket(_applicationContext, moduleName, user);
                var IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);

                TicketColumnValue externalTeam = formValues.Where(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam")).FirstOrDefault();
                formValues.RemoveAll(x => x.InternalFieldName.EqualsIgnoreCase("ExternalTeam"));

                errors.Clear();
                TicketRequest.Validate(formValues, ticket, errors, /*ignoreMandatory*/ true, IsAdmin, 1);
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
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = lstOfError });
                }

                errors = columnErrors;
                errors.AddRange(invalidCols);
                TicketRequest.SetItemValues(ticket, formValues, IsAdmin, /*updateChangesInHistory*/ true, _applicationContext.CurrentUser.Id, false, true);
                
                if (model.UpdateAllocations)
                {
                    projectEstimatedAllocationMGR.UpdatedAllocationDates(model.RecordId, ticket, model.UpdatePastAllocations);
                }
                
                TicketRequest.CommitChanges(ticket);

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = errors.Select(s => s.InternalFieldName + ": " + s.Message).ToList() });

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateRecord: " + ex);
                return InternalServerError();
            }
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
                    else messages.Add(TicketColumnError.AddError("TenantID", "", " not valid"));
                }
                foreach (var field in formFields)
                {
                    if (tenantId != null && field.FieldName == "TenantID")
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
            try
            {
                foreach (TicketColumnValue columnVal in columnValues)
                {
                    if (columnVal.InternalFieldName.EqualsIgnoreCase("ExternalTeam") || columnVal.InternalFieldName.EqualsIgnoreCase("Data"))
                        continue;

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
            DataTable dt = null;
            string moduleTable = string.Empty;
            try
            {
                if (module != null)
                    moduleTable = module.ModuleTable;
                TicketColumnError ticketColumnError = new TicketColumnError();
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
                        columnErrors.Add(ticketColumnError);
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
                ULog.WriteException($"An Exception Occurred in CurrentWorkItems: " + ex);
            }
        }

        private void AddUpdateExternalTeam(string TicketId, TicketColumnValue externalTeam, bool updateTeam = false)
        {
            RelatedCompanyManager relatedCompanyManager = new RelatedCompanyManager(_applicationContext);
            try
            {
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
                ULog.WriteException($"An Exception Occurred in AddUpdateExternalTeam: " + ex);
            }
        }

        [Route("GetProjectDates")]
        public async Task<IHttpActionResult> GetProjectDates([FromUri] string TicketId)
        {
            await Task.FromResult(0);
            try
            {
                if (!string.IsNullOrEmpty(TicketId))
                {
                    if (UGITUtility.IsValidTicketID(TicketId))
                    {
                        string moduleName = uHelper.getModuleNameByTicketId(TicketId);
                        TicketManager objTicketManager = new TicketManager(_applicationContext);
                        DataRow ticketRow = objTicketManager.GetByTicketIdFromCache(moduleName, TicketId);
                        if (ticketRow != null)
                        {
                            ProjectDatesResponse objprojectDatesResponse = new ProjectDatesResponse();
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.PreconStartDate, ticketRow.Table))
                                objprojectDatesResponse.PreconStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconStartDate]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.PreconEndDate, ticketRow.Table))
                                objprojectDatesResponse.PreconEnd = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconEndDate]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.EstimatedConstructionStart, ticketRow.Table))
                                objprojectDatesResponse.ConstStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.EstimatedConstructionEnd, ticketRow.Table))
                                objprojectDatesResponse.ConstEnd = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.CloseoutStartDate, ticketRow.Table))
                                objprojectDatesResponse.CloseoutStart = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.CloseoutDate, ticketRow.Table))
                                objprojectDatesResponse.Closeout = (UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]) == DateTime.MinValue
                                            && UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]) != DateTime.MinValue)
                                            ? UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]).AddWorkingDays(uHelper.getCloseoutperiod(_applicationContext))
                                            : UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]);
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHold, ticketRow.Table))
                                objprojectDatesResponse.OnHold = UGITUtility.StringToBoolean(ticketRow[DatabaseObjects.Columns.TicketOnHold]);

                            if (objprojectDatesResponse.PreconStart != DateTime.MinValue && objprojectDatesResponse.PreconEnd != DateTime.MinValue)
                            {
                                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), objprojectDatesResponse.PreconStart, objprojectDatesResponse.PreconEnd);
                                objprojectDatesResponse.PreconDuration = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                            }

                            if (objprojectDatesResponse.ConstStart != DateTime.MinValue && objprojectDatesResponse.ConstEnd != DateTime.MinValue)
                            {
                                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), objprojectDatesResponse.ConstStart, objprojectDatesResponse.ConstEnd);
                                objprojectDatesResponse.ConstDuration = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                            }

                            if (objprojectDatesResponse.CloseoutStart != DateTime.MinValue && objprojectDatesResponse.Closeout != DateTime.MinValue)
                            {
                                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), objprojectDatesResponse.CloseoutStart, objprojectDatesResponse.Closeout);
                                objprojectDatesResponse.CloseOutDuration = uHelper.GetWeeksFromDays(HttpContext.Current.GetManagerContext(), noOfWorkingDays);
                            }
                            objprojectDatesResponse.HasAnyPastAllocation = uHelper.HasAnyPastAllocation(TicketId);

                            return Ok(objprojectDatesResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectDates: " + ex);
            }
            return Ok("An Exception Occurred in GetProjectDates");
        }

        [Route("GetModuleConstraintsList")]
        public async Task<IHttpActionResult> GetModuleConstraintsList([FromUri] string projectID)
        {
            await Task.FromResult(0);
            try
            {
                if (!string.IsNullOrEmpty(projectID))
                {
                    if (UGITUtility.IsValidTicketID(projectID))
                    {
                        List<UserProfile> lstUserProfiles = _applicationContext.UserManager.GetUsersProfile();

                        string moduleName = uHelper.getModuleNameByTicketId(projectID);
                        UserProfile uProfile = null;
                        Role uRoles = null;
                        ModuleStageConstraintsManager objModuleStageConstraintManager = new ModuleStageConstraintsManager(_applicationContext);
                        //[Added for Comment]
                        ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
                        UserProfileManager UserManager = new UserProfileManager(_applicationContext);
                        List<HistoryEntry> historyList = new List<HistoryEntry>();
                        //[END]
                        List<ModuleStageConstraints> lstModuleStageConstraints = objModuleStageConstraintManager.Load(x => x.TicketId == projectID && !x.Deleted);
                        foreach (ModuleStageConstraints moduleConstraint in lstModuleStageConstraints)
                        {
                            List<string> lstUserIds = UGITUtility.ConvertStringToList(moduleConstraint.AssignedTo, Constants.Separator6);
                            List<UserProfile> lstAssignedToUsers = lstUserProfiles.Where(x => lstUserIds.Contains(x.Id)).ToList();
                            if (lstAssignedToUsers.Count > 0)
                            {
                                if (moduleConstraint.TaskDueDate == null || moduleConstraint.TaskDueDate == DateTime.MinValue)
                                {
                                    moduleConstraint.DueDaysLeft = null;
                                    moduleConstraint.TaskDueDate = null;
                                }
                                else
                                    moduleConstraint.DueDaysLeft = (moduleConstraint.TaskDueDate.Value - DateTime.Today.Date).TotalDays;
                            }
                            string name = string.Empty;
                            for (int i = 0; i < lstUserIds.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(lstUserIds[i])))
                                {
                                    if (i == 0)
                                    {
                                        uProfile = lstUserProfiles.Where(x => x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                        if (uProfile != null)
                                        {
                                            name = uProfile.Name;
                                        }
                                        else
                                        {
                                            uRoles = _applicationContext.UserManager.GetUserGroups().Where(x => x.TenantID.Equals(_applicationContext.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                            if (uRoles != null)
                                            {
                                                name = uRoles.Name;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        uProfile = lstUserProfiles.Where(x => x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                        if (uProfile != null)
                                        {
                                            name += ", " + uProfile.Name;
                                        }
                                        else
                                        {
                                            uRoles = _applicationContext.UserManager.GetUserGroups().Where(x => x.TenantID.Equals(_applicationContext.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                            if (uRoles != null)
                                            {
                                                name += ", " + uRoles.Title;
                                            }
                                        }

                                    }
                                }
                            }
                            //moduleConstraint.AssignedToName = UGITUtility.ConvertListToString(lstAssignedToUsers.Select(x => x.Name).ToList(), Constants.Separator6);
                            moduleConstraint.AssignedToName = name;
                            moduleConstraint.ProfilePics = UGITUtility.ConvertListToString(lstAssignedToUsers.Select(x => x.Picture).ToList(), Constants.Separator6);
                            //}
                        }
                        //[Added for Comment]
                        DataRow ticketCollection = Ticket.GetCurrentTicket(_applicationContext, moduleName, projectID);
                        bool oldestFirst = !configurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                        historyList = uHelper.GetHistory(ticketCollection, DatabaseObjects.Columns.TicketComment, oldestFirst);
                        int indexVal = 0;
                        if (historyList != null)
                        {
                            foreach (HistoryEntry historyEntry in historyList)
                            {
                                var userProfile = lstUserProfiles.Where(x => x.Id == historyEntry.createdBy).FirstOrDefault();// UserManager.GetUserInfoByIdOrName(historyEntry.createdBy);
                                if (userProfile != null)
                                {
                                    if (!string.IsNullOrEmpty(userProfile.Name))
                                        historyEntry.createdBy = userProfile.Name;

                                    if (!string.IsNullOrEmpty(userProfile.Picture) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(userProfile.Picture)))
                                        historyEntry.Picture = userProfile.Picture;
                                    else
                                        historyEntry.Picture = "/Content/Images/userNew.png";
                                }
                                else
                                {
                                    historyEntry.Picture = "/Content/Images/userNew.png";
                                }
                                historyEntry.Index = indexVal;
                                indexVal++;
                            }
                        }
                        //if ((lstModuleStageConstraints != null && lstModuleStageConstraints.Count > 0) || (historyList != null && historyList.Count > 0))
                        //{
                        return Ok(new { lstModuleStageConstraints = lstModuleStageConstraints, ListComments = historyList });
                        //}
                        //[END]
                    }
                }
                return Ok("An Exception Occurred in GetModuleConstraintsList");
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error is in GetModuleConstraintsList api");
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHardSoftModuleFlag")]
        public async Task<IHttpActionResult> GetHardSoftModuleFlag([FromUri] string moduleName)
        {
            await Task.FromResult(0);
            try
            {
                ModuleViewManager moduleViewMGR = new ModuleViewManager(_applicationContext);
                UGITModule moduleObj = moduleViewMGR.LoadByName(moduleName);
                if (moduleObj == null) { return Ok(false); }
                else { return Ok(moduleObj.IsAllocationTypeHard_Soft); }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetHardSoftModuleFlag: " + ex);
            }
            return Ok("An Exception Occurred in GetHardSoftModuleFlag");

        }

        [HttpGet]
        [Route("GetUserName")]
        public async Task<IHttpActionResult> GetUserName([FromUri] string userId)
        {
            await Task.FromResult(0);
            try
            {
                string userName = string.Empty;
                List<UserProfile> lstUserProfiles = _applicationContext.UserManager.GetUsersProfile();
                if (lstUserProfiles != null && lstUserProfiles.Count > 0)
                {
                    userName = lstUserProfiles.Where(o => o.Id == userId).FirstOrDefault()?.Name ?? string.Empty;
                }

                return Ok(userName);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserName: " + ex);
                return InternalServerError();
            }
        }

       

        
        

        public string GetImagePathFromBlob(DataRow currentRow)
        {
            string iconUrl = "";
            if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.IconBlob))
            {
                object imageBytes = UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.IconBlob);
                if (imageBytes != null && imageBytes != DBNull.Value)
                {
                    iconUrl = "data:image/png;base64," + Convert.ToBase64String((byte[])imageBytes);
                }
            }
            return iconUrl;
        }
         
        public string GetLeadSourceName(string ticketId)
        {
            if (!string.IsNullOrWhiteSpace(ticketId))
            {
                UGITModule conModule = ModuleViewManager.LoadByName(ModuleNames.CON);
                DataRow contactData = _ticketManager.GetByTicketID(conModule, ticketId);
                return contactData.Field<string>(DatabaseObjects.Columns.Title);
            }
            return string.Empty;
        }
        
        [HttpGet]
        [Route("GetEstimatorProjectDetails")]
        public async Task<IHttpActionResult> GetEstimatorProjectDetails()
        {
            await Task.FromResult(0);
            try
            {                
                TicketManager ticketManagerObj = new TicketManager(_applicationContext);
                ModuleStageConstraintsManager objModuleStageConstraintManager = new ModuleStageConstraintsManager(_applicationContext);
                FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_applicationContext);
                ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(_applicationContext);

                List<RResourceAllocation> lstAllOverLappingAlloc = new List<RResourceAllocation>();
                lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(DateTime.Now, DateTime.Now.AddDays(84));
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_applicationContext, DateTime.Now, DateTime.Now.AddDays(84));
                List<string> moduleNames = new List<string> { "CPR", "OPM", "CNS" };
                Dictionary<string, string> mdict = new Dictionary<string, string>();
                foreach (var item in moduleNames)
                {
                    List<RResourceAllocation> lstUserAllocation = lstAllOverLappingAlloc.Where(x => x.Resource == _applicationContext.CurrentUser.Id
                    && x.ProjectEstimatedAllocationId != null && x.TicketID.StartsWith(item)).OrderBy(x => x.AllocationStartDate).ToList();
                    if (lstUserAllocation != null && lstUserAllocation.Count > 0)
                    {
                        List<DateTime> lstOfStartDate = null;
                        List<DateTime> lstOfEndDate = null;
                        double? totalAllocOverlapDays = 0;
                        double? hardAllocOverlapDays = 0;

                        foreach (RResourceAllocation ralloc in lstUserAllocation)
                        {
                            lstOfStartDate = new List<DateTime>() { DateTime.Now, ralloc.AllocationStartDate.Value };
                            lstOfEndDate = new List<DateTime>() { DateTime.Now.AddDays(84), ralloc.AllocationEndDate.Value };
                            DateTime maxStartDate = lstOfStartDate.Max();
                            DateTime minEndDate = lstOfEndDate.Min();
                            double workingDays = uHelper.GetTotalWorkingDaysBetween(_applicationContext, maxStartDate, minEndDate);
                            double? pctAlloc = 0;
                            if (ralloc.PctAllocation.HasValue)
                                pctAlloc = ralloc.PctAllocation / 100;
                            totalAllocOverlapDays += workingDays * pctAlloc;
                            if (!ralloc.SoftAllocation)
                                hardAllocOverlapDays += workingDays * pctAlloc;
                        }
                        mdict.Add(item, Math.Round(((hardAllocOverlapDays / searchPeriodDays) * 100).Value, 1).ToString());
                    }
                }
                
                List<ProjectDataModel> projectDataModels = new List<ProjectDataModel>();
                DataTable userTicketData = _ticketManager.GetProjectDetailsByCurrentUser();
                foreach (DataRow dr in userTicketData.Rows)
                {
                    int totalAllocation = 0;
                    int filledAllocation = 0;                    
                    List<ModuleStageConstraints> lstModuleStageConstraints = objModuleStageConstraintManager.Load(x => x.TicketId == dr.Field<string>(DatabaseObjects.Columns.TicketId) && !x.Deleted);
                    
                    SetModuleStageConstraint(lstModuleStageConstraints);

                    if (dr["ResourceAllocationCount"] != DBNull.Value)
                    {
                        string[] rvalues = UGITUtility.SplitString(dr["ResourceAllocationCount"], Constants.Separator);
                        totalAllocation = int.Parse(rvalues[0]);
                        int unAllocatedResource = int.Parse(rvalues[1]);
                        filledAllocation = totalAllocation - unAllocatedResource;
                    }
                    projectDataModels.Add(new ProjectDataModel()
                    {
                        TicketId = dr.Field<string>(DatabaseObjects.Columns.TicketId),
                        Type = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.RelationshipType) && dr[DatabaseObjects.Columns.RelationshipType] != DBNull.Value 
                            ? dr.Field<string>(DatabaseObjects.Columns.RelationshipType) : "",
                        ProjectName = dr.Field<string>(DatabaseObjects.Columns.Title),
                        ClientName = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.ClientName) && dr[DatabaseObjects.Columns.ClientName] != DBNull.Value
                            ? dr.Field<string>(DatabaseObjects.Columns.ClientName) : "",
                        RequestTypeTitle = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.RequestTypeTitle) && dr[DatabaseObjects.Columns.RequestTypeTitle] != DBNull.Value
                            ? dr.Field<string>(DatabaseObjects.Columns.RequestTypeTitle) : "",
                        RequestType = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.RequestType) && dr[DatabaseObjects.Columns.RequestType] != DBNull.Value
                            ? dr.Field<string>(DatabaseObjects.Columns.RequestType) : "",
                        DueDate = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.BidDueDate) && dr[DatabaseObjects.Columns.BidDueDate] != DBNull.Value 
                            ? dr.Field<DateTime>(DatabaseObjects.Columns.BidDueDate) : DateTime.MinValue,
                        //Priority = dr.Field<Int64>(DatabaseObjects.Columns.TicketPriority).ToString(),
                        ChanceOfSuccess = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.ChanceOfSuccess) && dr[DatabaseObjects.Columns.ChanceOfSuccess] != DBNull.Value
                        ? dr.Field<string>(DatabaseObjects.Columns.ChanceOfSuccess) : "",
                        ProjectShortName = dr.Field<string>(DatabaseObjects.Columns.ShortName),
                        ERPJobId = dr.Field<string>(DatabaseObjects.Columns.ERPJobID),
                        TotalAllocations = totalAllocation.ToString(),
                        Partners = dr.Field<string>("ExternalTeam"),
                        PartnersType = dr.Field<string>("ExternalTeamType"),
                        ModuleName = dr.Field<string>("ModuleName"),
                        FilledAllocations = filledAllocation.ToString(),
                        StartDate = dr[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.PreconStartDate) : DateTime.MinValue,
                        EndDate = dr[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionEnd) : DateTime.MinValue,
                        Volume = dr[DatabaseObjects.Columns.ApproxContractValue] != DBNull.Value ? dr.Field<double>(DatabaseObjects.Columns.ApproxContractValue).ToString("N0") : "0",
                        moduleLink = GenerateModuleWiseLink(UGITUtility.ObjectToString(uHelper.getModuleNameByTicketId(dr.Field<string>(DatabaseObjects.Columns.TicketId))), dr.Field<string>(DatabaseObjects.Columns.TicketId), dr.Field<string>(DatabaseObjects.Columns.Title), "95", "95"),
                        AllocationLink = GenerateAllocationLink(UGITUtility.ObjectToString(uHelper.getModuleNameByTicketId(dr.Field<string>(DatabaseObjects.Columns.TicketId))), dr.Field<string>(DatabaseObjects.Columns.TicketId), dr.Field<string>(DatabaseObjects.Columns.Title), dr.Field<string>("ResourceAllocationCount")),
                        TotalTasks = lstModuleStageConstraints?.Count.ToString() ?? "",
                        CompletedTasks = lstModuleStageConstraints?.Where(x => !string.IsNullOrWhiteSpace(x.TaskStatus) && x.TaskStatus.Equals("Completed"))?.ToList().Count.ToString() ?? "",
                        UserTasks = lstModuleStageConstraints
                    });
                }

                return Ok(new { ProjectDataModels = projectDataModels, UserUtilization = mdict });
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error is in GetEstimatorProjectDetails api");
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLeadEstimatorWithProjectDetails")]
        public async Task<IHttpActionResult> GetLeadEstimatorWithProjectDetails()
        {
            await Task.FromResult(0);
            try
            {
                TicketManager ticketManagerObj = new TicketManager(_applicationContext);
                ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(_applicationContext);
                GlobalRoleManager roleManager = new GlobalRoleManager(_applicationContext);
                ModuleStageConstraintsManager objModuleStageConstraintManager = new ModuleStageConstraintsManager(_applicationContext);
                DataTable dtOpportunityTickets = ticketManagerObj.GetAllOpenTicketsBasedOnModuleName(ModuleNames.OPM);
                List<ProjectDataModel> projectDatas = new List<ProjectDataModel>();
                List<UserProfile> lstUserProfiles = _applicationContext.UserManager.GetUsersProfile();
                List<RResourceAllocation> lstAllOverLappingAlloc = new List<RResourceAllocation>();
                lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(DateTime.Now, DateTime.Now.AddDays(84));
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_applicationContext, DateTime.Now, DateTime.Now.AddDays(84));
                DataTable groupedData = dtOpportunityTickets.DefaultView.ToTable(true, DatabaseObjects.Columns.LeadEstimatorUser);
                List<Estimatordetails> lstEstimatordetails = new List<Estimatordetails>();
                GlobalRoles = roleManager.Load(x => x.TenantID == _applicationContext.TenantID);
                foreach (DataRow gd in groupedData.Rows)
                {
                    int totalAllocation = 0;
                    int filledAllocation = 0;
                    double pctAllocation = 0;
                    UserProfile user = lstUserProfiles.Where(x => x.Name == gd.Field<string>(DatabaseObjects.Columns.LeadEstimatorUser)).FirstOrDefault();
                    Estimatordetails estimatordetails = new Estimatordetails();
                    estimatordetails.items = new List<ProjectDataModel>();
                    estimatordetails.ResourceId = user?.Id;
                    estimatordetails.UserName = user?.Name;
                    estimatordetails.UserImageURL = user?.Picture;
                    if (user != null)
                    {
                        GlobalRole typeGroup = GlobalRoles.FirstOrDefault(x => x.Id == user.GlobalRoleId);
                        estimatordetails.Role = typeGroup != null ? typeGroup.Name : "";
                        List<RResourceAllocation> lstUserAllocation = lstAllOverLappingAlloc.Where(x => x.Resource == user.Id && x.ProjectEstimatedAllocationId != null).OrderBy(x => x.AllocationStartDate).ToList();
                        if (lstUserAllocation != null && lstUserAllocation.Count > 0)
                        {
                            List<DateTime> lstOfStartDate = null;
                            List<DateTime> lstOfEndDate = null;
                            double? totalAllocOverlapDays = 0;
                            double? hardAllocOverlapDays = 0;

                            foreach (RResourceAllocation ralloc in lstUserAllocation)
                            {
                                lstOfStartDate = new List<DateTime>() { DateTime.Now, ralloc.AllocationStartDate.Value };
                                lstOfEndDate = new List<DateTime>() { DateTime.Now.AddDays(84), ralloc.AllocationEndDate.Value };
                                DateTime maxStartDate = lstOfStartDate.Max();
                                DateTime minEndDate = lstOfEndDate.Min();
                                double workingDays = uHelper.GetTotalWorkingDaysBetween(_applicationContext, maxStartDate, minEndDate);
                                double? pctAlloc = 0;
                                if (ralloc.PctAllocation.HasValue)
                                    pctAlloc = ralloc.PctAllocation / 100;
                                totalAllocOverlapDays += workingDays * pctAlloc;
                                if (!ralloc.SoftAllocation)
                                    hardAllocOverlapDays += workingDays * pctAlloc;
                            }

                            pctAllocation = Math.Round(((hardAllocOverlapDays / searchPeriodDays) * 100).Value, 1);
                        }
                    }
                    estimatordetails.PctAllocations = pctAllocation.ToString();
                    DataTable groupedTickets = dtOpportunityTickets.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.LeadEstimatorUser) == gd.Field<string>(DatabaseObjects.Columns.LeadEstimatorUser)).CopyToDataTable();
                    foreach (DataRow dr in groupedTickets.Rows)
                    {
                        if (dr["ResourceAllocationCount"] != DBNull.Value)
                        {
                            string[] rvalues = UGITUtility.SplitString(dr["ResourceAllocationCount"], Constants.Separator);
                            totalAllocation = int.Parse(rvalues[0]);
                            int unAllocatedResource = int.Parse(rvalues[1]);
                            filledAllocation = totalAllocation - unAllocatedResource;
                        }
                        string ticketID = dr.Field<string>(DatabaseObjects.Columns.TicketId);
                        List<ModuleStageConstraints> lstModuleStageConstraints = objModuleStageConstraintManager.Load(x => x.TicketId == ticketID && !x.Deleted);

                        estimatordetails.items.Add(new ProjectDataModel()
                        {
                            TicketId = dr.Field<string>(DatabaseObjects.Columns.TicketId),
                            CompanyName = UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.CRMCompanyTitle) && dr[DatabaseObjects.Columns.CRMCompanyTitle] != DBNull.Value
                                ? dr.Field<string>(DatabaseObjects.Columns.CRMCompanyTitle) : "",
                            ProjectName = dr.Field<string>(DatabaseObjects.Columns.Title),
                            Priority = dr.Field<string>(DatabaseObjects.Columns.TicketPriority),
                            Studio = dr.Field<string>(DatabaseObjects.Columns.StudioLookup),
                            Division = dr.Field<string>(DatabaseObjects.Columns.DivisionLookup),
                            Type = dr.Field<string>(DatabaseObjects.Columns.RequestTypeLookup),
                            Volume = dr[DatabaseObjects.Columns.ApproxContractValue] != DBNull.Value ? dr.Field<double>(DatabaseObjects.Columns.ApproxContractValue).ToString("N0") : "0",
                            StartDate = dr[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionStart) : DateTime.MinValue,
                            EndDate = dr[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionEnd) : DateTime.MinValue,
                            DueDate = dr[DatabaseObjects.Columns.BidDueDate] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.BidDueDate) : DateTime.MinValue,
                            PreConStartDate= dr[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.PreconStartDate) : DateTime.MinValue,
                            PreConEndDate= dr[DatabaseObjects.Columns.PreconEndDate] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.PreconEndDate) : DateTime.MinValue,
                            TotalAllocations = totalAllocation.ToString(),
                            FilledAllocations = filledAllocation.ToString(),
                            TotalTasks = lstModuleStageConstraints?.Count.ToString() ?? "",
                            CompletedTasks = lstModuleStageConstraints?.Where(x => !string.IsNullOrWhiteSpace(x.TaskStatus) && x.TaskStatus.Equals("Completed"))?.ToList().Count.ToString() ?? "",
                            LeadUsers = GetLeadUsers(dr),
                            LeadUserImageUrl = lstUserProfiles?.Where(x => x.Id == dr.Field<string>(DatabaseObjects.Columns.ProjectLeadUser + "$Id"))?.FirstOrDefault()?.Picture ?? string.Empty,
                            LeadUserName = lstUserProfiles?.Where(x => x.Id == dr.Field<string>(DatabaseObjects.Columns.ProjectLeadUser + "$Id"))?.FirstOrDefault()?.Name ?? string.Empty,
                            LeadUserId = lstUserProfiles?.Where(x => x.Id == dr.Field<string>(DatabaseObjects.Columns.ProjectLeadUser + "$Id"))?.FirstOrDefault()?.Id ?? string.Empty,
                            LeadRole = getGlobalRole(lstUserProfiles?.Where(x => x.Id == dr.Field<string>(DatabaseObjects.Columns.ProjectLeadUser + "$Id"))?.FirstOrDefault()?.GlobalRoleId ?? string.Empty),
                            LeadProjectUrl = getProjectUrl(dr.Field<string>(DatabaseObjects.Columns.TicketId), dr.Field<string>(DatabaseObjects.Columns.Title))
                        });
                    }
                    lstEstimatordetails.Add(estimatordetails);
                }
                return Ok(new { estimatorDetails = lstEstimatordetails, lstUserProfiles = lstUserProfiles.Where(x => x.Enabled).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToList() });
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error is in GetLeadEstimatorWithProjectDetails api");
                return Ok(ex.Message);
            }
        }

        private List<UpdateLeadUsersResponse> GetLeadUsers(DataRow projectRow)
        {
            var leadUsers = new List<UpdateLeadUsersResponse>();

            string userID = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.ProjectLeadUser]);
            leadUsers.Add(new UpdateLeadUsersResponse()
            {
                Field = DatabaseObjects.Columns.ProjectLeadUser,
                Name = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.ProjectLeadUser]),
                UserId = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.ProjectLeadUser + "$Id"])
            });

            userID = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadEstimatorUser]);
            leadUsers.Add(new UpdateLeadUsersResponse()
            {
                Field = DatabaseObjects.Columns.LeadEstimatorUser,
                Name = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadEstimatorUser]),
                UserId = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadEstimatorUser + "$Id"])
            });

            userID = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadSuperintendentUser]);
            leadUsers.Add(new UpdateLeadUsersResponse()
            {
                Field = DatabaseObjects.Columns.LeadSuperintendentUser,
                Name = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadSuperintendentUser]),
                UserId = UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.LeadSuperintendentUser + "$Id"])
            });
            return leadUsers;
        }

        public string getProjectUrl(string ticketId, string title)
        {
            string url = string.Empty;
            string viewUrl = string.Empty;
            string ModuleName= uHelper.getModuleNameByTicketId(ticketId);
            UGITModule module = ModuleViewManager.LoadByName(ModuleName, true);
            DataTable moduledt = UGITUtility.ObjectToData(module);
            if (moduledt.Rows.Count > 0)
            {
                moduleRow = moduledt.Rows[0];
            }
            if (moduleRow != null)
            {
                viewUrl = string.Empty;
                if (moduleRow[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                    viewUrl = UGITUtility.GetAbsoluteURL(moduleRow[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                if (ticketId != string.Empty)
                {
                    title = string.Format("{0}: {1}", ticketId, title);
                }
            }
            title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
            title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening
            
            if (!string.IsNullOrEmpty(viewUrl))
            {
                url = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceURL, 95, 95);
            }
            return url;
        }
        private string getGlobalRole(string roleId)
        {
            string leadRoleName = string.Empty;
            if (!string.IsNullOrEmpty(roleId))
            {
                
                GlobalRole typeGroup = GlobalRoles.FirstOrDefault(x => x.Id == roleId);
                leadRoleName = typeGroup != null ? typeGroup.Name : "";
            }
            return leadRoleName;
        }
        [HttpGet]
        [Authorize]
        [Route("GetCalendarEventsByUserID")]
        public async Task<IHttpActionResult> GetCalendarEventsByUserID()
        {
            await Task.FromResult(0);
            try
            {
                var loggedinUser = _userProfileManager.GetUserProfile(_applicationContext.CurrentUser.Id);
                if (loggedinUser == null)
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username or password" }, User = null });

                if (loggedinUser != null && loggedinUser.Enabled == false)
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Inactive User" }, User = null });

                var user = new { loggedinUser.Id, loggedinUser.TenantID, loggedinUser.UserName, loggedinUser.Name, loggedinUser.Email, loggedinUser.Picture };
                return Ok(new UserResponse { Status = true, ErrorMessages = null, User = user });
            }
            catch (Exception ex)
            {
                return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetLoggedInUserProfile")]
        public async Task<IHttpActionResult> GetLoggedInUserProfile()
        {
            await Task.FromResult(0);
            try
            {
                var loggedinUser = _userProfileManager.GetUserProfile(_applicationContext.CurrentUser.Id);
                if (loggedinUser == null)
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username or password" }, User = null });

                if (loggedinUser != null && loggedinUser.Enabled == false)
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Inactive User" }, User = null });

                var user = new { loggedinUser.Id, loggedinUser.TenantID, loggedinUser.UserName, loggedinUser.Name, loggedinUser.Email, loggedinUser.Picture };
                return Ok(new UserResponse { Status = true, ErrorMessages = null, User = user });
            }
            catch (Exception ex)
            {
                return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [Route("GetModuleComments")]
        public async Task<IHttpActionResult> GetModuleComments([FromUri] string projectID)
        {
            await Task.FromResult(0);
            try
            {
                if (!string.IsNullOrEmpty(projectID))
                {
                    if (UGITUtility.IsValidTicketID(projectID))
                    {
                        List<UserProfile> lstUserProfiles = _applicationContext.UserManager.GetUsersProfile();

                        string moduleName = uHelper.getModuleNameByTicketId(projectID);
                        //[Added for Comment]
                        ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
                        UserProfileManager UserManager = new UserProfileManager(_applicationContext);
                        List<HistoryEntry> historyList = new List<HistoryEntry>();
                        //[END]
                        
                        //[Added for Comment]
                        DataRow ticketCollection = Ticket.GetCurrentTicket(_applicationContext, moduleName, projectID);
                        bool oldestFirst = !configurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                        historyList = uHelper.GetHistory(ticketCollection, DatabaseObjects.Columns.TicketComment, oldestFirst);
                        int indexVal = 0;
                        if (historyList != null)
                        {
                            bool showPrivateComment = false;
                            UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                            // checking comment if the current user is in ticket owner, in PRP Group, TicketORP,TicketPRP
                            var user = _applicationContext.CurrentUser;
                            var IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);
                            string PRPGroup = Convert.ToString(ticketCollection[DatabaseObjects.Columns.PRPGroup]);
                            if (IsAdmin || userProfileManager.CheckUserIsInGroup(PRPGroup, _applicationContext.CurrentUser) ||
                                 userProfileManager.IsUserPresentInField(_applicationContext.CurrentUser, ticketCollection, DatabaseObjects.Columns.TicketOwner) ||
                                 userProfileManager.IsUserPresentInField(_applicationContext.CurrentUser, ticketCollection, DatabaseObjects.Columns.TicketORP) ||
                                 userProfileManager.IsUserPresentInField(_applicationContext.CurrentUser, ticketCollection, DatabaseObjects.Columns.TicketPRP))
                            {
                                showPrivateComment = true;
                            }

                            foreach (HistoryEntry historyEntry in historyList)
                            {
                                if (historyEntry.IsPrivate && showPrivateComment)
                                {
                                    historyEntry.PrivateCommentImage += "<img src='/Content/images/lock16X16.png' style='border: none; vertical-align:bottom; padding-right: 5px; width:18px;' title='Private' alt=''>";
                                }
                                else
                                {
                                    historyEntry.PrivateCommentImage = "";
                                }
                                var userProfile = lstUserProfiles.Where(x => x.Id == historyEntry.createdBy).FirstOrDefault();// UserManager.GetUserInfoByIdOrName(historyEntry.createdBy);
                                if (userProfile != null)
                                {
                                    if (!string.IsNullOrEmpty(userProfile.Name))
                                        historyEntry.createdBy = userProfile.Name;

                                    if (!string.IsNullOrEmpty(userProfile.Picture) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(userProfile.Picture)))
                                        historyEntry.Picture = userProfile.Picture;
                                    else
                                        historyEntry.Picture = "/Content/Images/userNew.png";
                                }
                                else
                                {
                                    historyEntry.Picture = "/Content/Images/userNew.png";
                                }
                                historyEntry.Index = indexVal;
                                indexVal++;
                            }
                        }
                        return Ok(new { ListComments = historyList });
                        //[END]
                    }
                }
                return Ok("An Exception Occurred in GetModuleConstraintsList");
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error is in GetModuleConstraintsList api");
                return Ok(ex.Message);
            }
        }


        [HttpPost]
        [Route("AddCommentDetails")]
        public async Task<IHttpActionResult> AddCommentDetails(string Comment, string TicketID)
        {
            await Task.FromResult(0);
            try
            {
                Comment = HttpContext.Current.Server.UrlDecode(Comment);
                ModuleViewManager moduleViewManager = new ModuleViewManager(_applicationContext);
                UserProfile user = new UserProfile();
                user = _applicationContext.CurrentUser;
                string ModuleName = uHelper.getModuleNameByTicketId(TicketID);
                Ticket ticketRequest = new Ticket(_applicationContext, ModuleName);
                //string moduletable = moduleViewManager.GetModuleTableName(ModuleName);
                TicketManager ticketManager = new TicketManager(_applicationContext);
                DataRow saveTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, TicketID);
                saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, Comment.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, false);
                ticketRequest.CommitChanges(saveTicket);
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in AddCommentDetails: " + ex);
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("UpdateCommentDetails")]
        public async Task<IHttpActionResult> UpdateCommentDetails(string Comment, int Index, string TicketID)
        {
            await Task.FromResult(0);
            try
            {
                Comment = HttpContext.Current.Server.UrlDecode(Comment);
                ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
                ModuleViewManager moduleViewManager = new ModuleViewManager(_applicationContext);
                TicketManager ticketManager = new TicketManager(_applicationContext);
                List<HistoryEntry> datalist = new List<HistoryEntry>();
                UserProfile user = new UserProfile();
                user = _applicationContext.CurrentUser;
                string ModuleName = uHelper.getModuleNameByTicketId(TicketID);
                DataRow ticketCollection = Ticket.GetCurrentTicket(_applicationContext, ModuleName, TicketID);
                bool oldestFirst = !configurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                datalist = uHelper.GetHistory(ticketCollection, DatabaseObjects.Columns.TicketComment, oldestFirst);
                HistoryEntry entry = datalist[Index];
                if (entry != null && entry.entry.ToLower() != Comment.ToLower())
                {
                    string oldEntry = entry.entry;
                    entry.entry = Comment;
                    Ticket ticketRequest = new Ticket(_applicationContext, ModuleName);
                    //string moduletable = moduleViewManager.GetModuleTableName(ModuleName);
                    DataRow saveTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, TicketID);
                    saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentsbyDataList(datalist);
                    ticketRequest.CommitChanges(saveTicket);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateCommentDetails: " + ex);
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteComments")]
        public async Task<IHttpActionResult> DeleteComments(Comments comments)
        {
            await Task.FromResult(0);
            try
            {
                string TicketID = comments.lstComments[0].TicketID;
                ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
                ModuleViewManager moduleViewManager = new ModuleViewManager(_applicationContext);
                List<HistoryEntry> datalist = new List<HistoryEntry>();
                string ModuleName = uHelper.getModuleNameByTicketId(TicketID);
                DataRow ticketCollection = Ticket.GetCurrentTicket(_applicationContext, ModuleName, TicketID);
                bool oldestFirst = !configurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                datalist = uHelper.GetHistory(ticketCollection, DatabaseObjects.Columns.TicketComment, oldestFirst);

                int count = 0;
                for (int i = 0; i < comments.lstComments.Count; i++)
                {
                    datalist.RemoveAll(element => element.created == Convert.ToString(comments.lstComments[i].Created) && element.entry == Convert.ToString(comments.lstComments[i].Entry));
                    count++;
                }

                List<HistoryEntry> tempdatalist = new List<HistoryEntry>();
                tempdatalist = datalist.OrderBy(tt => Convert.ToDateTime(tt.created)).ToList();
                
                Ticket ticketRequest = new Ticket(_applicationContext, ModuleName);
                DataRow saveTicket = Ticket.GetCurrentTicket(_applicationContext, ModuleName, TicketID);
                saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentsbyDataList(tempdatalist);
                ticketRequest.CommitChanges(saveTicket);

                string retValue = count + " comments are deleted.";
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetNextWorkingDateAndTime: " + ex);
            }
            return Ok("An Exception Occurred in GetNextWorkingDateAndTime");
        }


        [HttpPost]
        [Route("GetUsersCommonProjects")]
        public async Task<IHttpActionResult> GetUsersCommonProjects(string userIds, string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                TicketManager ticketManager = new TicketManager(_applicationContext);
                List<string> users = userIds.Split(',').Distinct().ToList();
                List<string> commonProjectIds = uHelper.GetUsersCommonProjects(users);
                List<object> projectData = new List<object>();

                DataRow currentProject = ticketManager.GetByTicketIdFromCache(ticketId.Substring(0, 3), ticketId);

                List<ProjectTag> sProjectTags = currentProject != null && currentProject[DatabaseObjects.Columns.TagMultiLookup] != DBNull.Value
                    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProjectTag>>(currentProject.Field<string>(DatabaseObjects.Columns.TagMultiLookup)) : null;

                int sExpTagCount = sProjectTags?.Count() ?? 0;
                foreach (string projectId in commonProjectIds)
                {
                    DataRow dr = ticketManager.GetByTicketIdFromCache(projectId.Substring(0, 3), projectId);
                    int dExpTagMatchCount = 0;
                    List<ProjectTag> dProjectTags = dr[DatabaseObjects.Columns.TagMultiLookup] != DBNull.Value 
                        ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProjectTag>>(dr.Field<string>(DatabaseObjects.Columns.TagMultiLookup)) : null;
                    if (sExpTagCount > 0 && dProjectTags?.Count() > 0)
                    {
                        dExpTagMatchCount = sProjectTags.Intersect(dProjectTags).Count();
                    }

                    projectData.Add(new
                    {
                        Title = dr.Field<string>(DatabaseObjects.Columns.Title),
                        ERPJobID = dr.Field<string>(DatabaseObjects.Columns.ERPJobID),
                        ModuleName = dr.Field<string>(DatabaseObjects.Columns.TicketId).Substring(0,3),
                        TicketId = dr.Field<string>(DatabaseObjects.Columns.TicketId),
                        ERPJobIDNC = dr.Field<string>(DatabaseObjects.Columns.ERPJobIDNC),
                        Status = dr.Field<string>(DatabaseObjects.Columns.Status),
                        Client = dr.Field<string>(DatabaseObjects.Columns.CRMCompanyTitle),
                        MatchedTag = dExpTagMatchCount.ToString(),
                        TotalTag = sExpTagCount.ToString(),
                        StartDate = dr[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionStart) : DateTime.MinValue,
                        CloseDate = dr[DatabaseObjects.Columns.CloseDate] != DBNull.Value ? dr.Field<DateTime>(DatabaseObjects.Columns.CloseDate) : DateTime.MinValue,
                    });
                }
                return Ok(projectData);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUsersCommonProjects: " + ex);
                return Ok(ex.Message);
            }
        }
        public string GenerateModuleWiseLink(string ModuleName, string TicketId, string Title, string width, string height)
        {
            DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(ModuleName, true));
            if (moduledt.Rows.Count > 0)
            {
                DataRow moduleDetail = moduledt.Rows[0];
                //string moduleTableName = moduleDetail[DatabaseObjects.Columns.ModuleTicketTable].ToString();
                string viewUrl = string.Empty;
                if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                    viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                Title = TicketId + ": " + Title;
                return string.Format("event.stopPropagation(); openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}'); return false;", viewUrl
                    , string.Format("TicketId={0}", TicketId), Title, sourceURL, width, height);
            }
            return string.Empty;
        }

        public string GenerateAllocationLink(string ModuleName, string TicketId, string Title,string ResourceAllocationCount)
        {
            Title = TicketId + ": " + Title;
            string path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + TicketId + "&module=" + ModuleName;
            string func = string.Format("event.stopPropagation(); javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, string.Format("moduleName={0}&ConfirmBeforeClose=true", ModuleName), Title, sourceURL);
            string NewOPMWizardPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=newopmwizard");
            //editCell.Attributes.Add("onClick", func);
            if (ResourceAllocationCount != null)
            {
                string[] values = UGITUtility.SplitString(ResourceAllocationCount, Constants.Separator);
                int totalAllocation = int.Parse(values[0]);
                if (totalAllocation == 0 && !uHelper.HideAllocationTemplate(_applicationContext) && (ModuleName == "CPR" || ModuleName == "OPM" || ModuleName == "CNS"))
                {
                    string popupTitle = "Add Resource Allocations to " + (ModuleName == "CPR" ? "Project" : ModuleName == "OPM" ? "Opportunity" : "Service");
                    path = NewOPMWizardPageUrl + "&ticketId=" + TicketId + "&module=" + ModuleName + "&selectionmode=NewAllocatonsFromProjects&title=" + Title;
                    func = string.Format("event.stopPropagation(); javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, "", popupTitle, sourceURL);
                    //editCell.Attributes.Add("onClick", func);
                }

                return func;
            }
            return string.Empty;
        }

        public void SetModuleStageConstraint(List<ModuleStageConstraints> lstModuleStageConstraints)
        {
            List<UserProfile> lstUserProfiles = _applicationContext.UserManager.GetUsersProfile();
            UserProfile uProfile = null;
            Role uRoles = null;
            foreach (ModuleStageConstraints moduleConstraint in lstModuleStageConstraints)
            {
                List<string> lstUserIds = UGITUtility.ConvertStringToList(moduleConstraint.AssignedTo, Constants.Separator6);
                List<UserProfile> lstAssignedToUsers = lstUserProfiles.Where(x => lstUserIds.Contains(x.Id)).ToList();
                if (lstAssignedToUsers.Count > 0)
                {
                    if (moduleConstraint.TaskDueDate == null || moduleConstraint.TaskDueDate == DateTime.MinValue)
                    {
                        moduleConstraint.DueDaysLeft = null;
                        moduleConstraint.TaskDueDate = null;
                    }
                    else
                        moduleConstraint.DueDaysLeft = (moduleConstraint.TaskDueDate.Value - DateTime.Today.Date).TotalDays;
                }
                string name = string.Empty;
                for (int i = 0; i < lstUserIds.Count; i++)
                {
                    if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(lstUserIds[i])))
                    {
                        if (i == 0)
                        {
                            uProfile = lstUserProfiles.Where(x => x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                            if (uProfile != null)
                            {
                                name = uProfile.Name;
                            }
                            else
                            {
                                uRoles = _applicationContext.UserManager.GetUserGroups().Where(x => x.TenantID.Equals(_applicationContext.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                if (uRoles != null)
                                {
                                    name = uRoles.Name;
                                }
                            }

                        }
                        else
                        {
                            uProfile = lstUserProfiles.Where(x => x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                            if (uProfile != null)
                            {
                                name += ", " + uProfile.Name;
                            }
                            else
                            {
                                uRoles = _applicationContext.UserManager.GetUserGroups().Where(x => x.TenantID.Equals(_applicationContext.TenantID, StringComparison.InvariantCultureIgnoreCase) && x.Id == UGITUtility.ObjectToString(lstUserIds[i])).FirstOrDefault();
                                if (uRoles != null)
                                {
                                    name += ", " + uRoles.Title;
                                }
                            }

                        }
                    }
                }
                //moduleConstraint.AssignedToName = UGITUtility.ConvertListToString(lstAssignedToUsers.Select(x => x.Name).ToList(), Constants.Separator6);
                moduleConstraint.AssignedToName = name;
                moduleConstraint.ProfilePics = UGITUtility.ConvertListToString(lstAssignedToUsers.Select(x => x.Picture).ToList(), Constants.Separator6);
                //}
            }
        }

    }

    public class ProjectDatesResponse
    {
        public DateTime PreconStart { get; set; }
        public DateTime PreconEnd { get; set; }
        public DateTime ConstStart { get; set; }
        public DateTime ConstEnd { get; set; }
        public DateTime CloseoutStart { get; set; }
        public DateTime Closeout { get; set; }
        public int PreconDuration { get; set; }
        public int ConstDuration { get; set; }
        public int CloseOutDuration { get; set; }
        public bool OnHold { get; set; }
        public bool HasAnyPastAllocation { get; set; }
    }

    public class ExecutivePageRequest
    {
        public string Division { get; set; }
        public string Role { get; set; }
        public string Filter { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Period { get; set; }
    }

    public class KpiIndicators
    {
        public string AcqRatio { get; set; }
        public string BilledHours { get; set; }
        public string AcqHours { get; set; }
        public string OpmLastMonth { get; set; }
        public string OpmAverage { get; set; }
        public string CPRLastMonth { get; set; }
        public string CPRAverage { get; set; }
        public string opmlost { get; set; }
        public string cprwon { get; set; }
        public string LastDay { get; set; }
        public string NextDay { get; set; }
    }

    public class ProjectGanttPage
    {
        public string Status { get; set; }
        public string No_of_projects { get; set; }
    }
    public class ProjectGanttBarResponse
    {
        public string category = "Projects";
        public int? OPM { get; set; }
        public int? TP { get; set; }
        public int? CPR { get; set; }
    }

    public class ProjectGanttPageRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Division { get; set; }
        public int Studio { get; set; }
        public string Sector { get; set; }
    }
}
