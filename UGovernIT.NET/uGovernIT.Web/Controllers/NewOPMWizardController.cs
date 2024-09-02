using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.RMM;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using DevExpress.ExpressApp;
using DevExpress.XtraPrinting.Export.Pdf;
using uGovernIT.Web.Models;
using DevExpress.XtraPivotGrid;

namespace uGovernIT.Web.Controllers
{

    [RoutePrefix("api/OPMWizard")]
    public class NewOPMWizardController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager _ticketManager = null;
        private uGovernIT.Manager.StateManager _StateManager;
        private ResourceAllocationManager _resourceAllocationManager = null;
        private ProjectEstimatedAllocationManager _projectEstimatedAllocationManager = null;
        private GlobalRoleManager _roleManager;
        private UserProfileManager _userProfileManager;
        private ConfigurationVariableManager _configVariableManager;
        private UserProjectExperienceManager _userProjectExperienceManager;
        private List<ScoreColorRange> scoreColorRanges;
        public NewOPMWizardController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _moduleViewManager = new ModuleViewManager(_applicationContext);
            _ticketManager = new TicketManager(_applicationContext);
            _StateManager = new uGovernIT.Manager.StateManager(_applicationContext);
            _resourceAllocationManager = new ResourceAllocationManager(_applicationContext);
            _projectEstimatedAllocationManager = new ProjectEstimatedAllocationManager(_applicationContext);
            _roleManager = new GlobalRoleManager(_applicationContext);
            _userProfileManager = new UserProfileManager(_applicationContext);
            _configVariableManager = new ConfigurationVariableManager(_applicationContext);
            _userProjectExperienceManager = new UserProjectExperienceManager(_applicationContext);
        }

        [HttpPost]
        [Route("CreateOpportunity")]
        public async Task<IHttpActionResult> CreateOpportunity(CreateOpportunityRequest request)
        {
            await Task.FromResult(0);
            try
            {
                if (request.SourceModule == "Lead")
                {
                    string moduleName = ModuleNames.LEM;
                    UGITModule moduleObj = _moduleViewManager.GetByName(ModuleNames.OPM);
                    DataTable emptyTable = _ticketManager.GetDatabaseTableSchema(moduleObj.ModuleTable);
                    DataRow sourceTicketRow = Ticket.GetCurrentTicket(_applicationContext, moduleName, request.SourceTicketId);
                    if (sourceTicketRow != null)
                    {
                        DataRow row = emptyTable.NewRow();
                        //copy values from sourceTicket to newTicket
                        row[DatabaseObjects.Columns.ID] = DBNull.Value;
                        row[DatabaseObjects.Columns.Title] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Title]);
                        row[DatabaseObjects.Columns.Description] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Description]);
                        row[DatabaseObjects.Columns.TicketStageActionUsers] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.TicketStageActionUsers]);
                        row[DatabaseObjects.Columns.TicketStageActionUserTypes] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                        row[DatabaseObjects.Columns.TicketPctComplete] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.TicketPctComplete]);
                        row["CategoryLookup"] = UGITUtility.StringToLong(sourceTicketRow["CategoryLookup"]);
                        row[DatabaseObjects.Columns.Address] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Address]);
                        row[DatabaseObjects.Columns.StreetAddress1] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.StreetAddress1]);
                        row[DatabaseObjects.Columns.City] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.City]);
                        row[DatabaseObjects.Columns.ContactLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.ContactLookup]);
                        row[DatabaseObjects.Columns.DepartmentLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.DepartmentLookup]);
                        row[DatabaseObjects.Columns.DivisionLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.DivisionLookup]);
                        row[DatabaseObjects.Columns.OwnerUser] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.OwnerUser]);
                        row[DatabaseObjects.Columns.ModuleStepLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.ModuleStepLookup]);
                        row[DatabaseObjects.Columns.RequestTypeLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.RequestTypeLookup]);
                        row[DatabaseObjects.Columns.ApproxContractValue] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.ApproxContractValue]);
                        //row[DatabaseObjects.Columns.EstimatedConstructionStart] = UGITUtility.StringToDateTime(sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        //row[DatabaseObjects.Columns.EstimatedConstructionEnd] = UGITUtility.StringToDateTime(sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        //row[DatabaseObjects.Columns.DueDate]= UGITUtility.StringToDateTime(sourceTicketRow[DatabaseObjects.Columns.DueDate]);

                        if (sourceTicketRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                            row[DatabaseObjects.Columns.LEMIdLookup] = Convert.ToString(sourceTicketRow[DatabaseObjects.Columns.TicketId]);

                        if (sourceTicketRow.Table.Columns.Contains(DatabaseObjects.Columns.SuccessChance))
                            row[DatabaseObjects.Columns.SuccessChance] = Convert.ToString(sourceTicketRow[DatabaseObjects.Columns.SuccessChance]);

                        if (sourceTicketRow.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                            row[DatabaseObjects.Columns.BCCISector] = Convert.ToString(sourceTicketRow[DatabaseObjects.Columns.BCCISector]);

                        Ticket ticketRequest = new Ticket(_applicationContext, moduleObj.ModuleName);
                        ticketRequest.Create(row, _applicationContext.CurrentUser);
                        string error = ticketRequest.CommitChanges(row);
                        if (string.IsNullOrEmpty(error))
                        {
                            _projectEstimatedAllocationManager.ImportAllocation(_applicationContext, request.Allocations, request.ProjectStartDate, request.ProjectEndDate);
                            NewOpportunityResponse newOpportunityResponse = new NewOpportunityResponse();
                            newOpportunityResponse.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                            newOpportunityResponse.Title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                            return Ok(newOpportunityResponse);
                        }
                        else
                        {
                            return Ok(error);
                        }
                    }
                }
                else if (request.SourceModule == "OPM")
                {
                    DataRow sourceTicketRow = Ticket.GetCurrentTicket(_applicationContext, uHelper.getModuleNameByTicketId(request.SourceTicketId), request.SourceTicketId);
                    if (sourceTicketRow != null)
                    {
                        UGITModule moduleObj = null;
                        if (request.OpportunityTarget == "Service")
                            moduleObj = _moduleViewManager.GetByName(ModuleNames.CNS);
                        else
                            moduleObj = _moduleViewManager.GetByName(ModuleNames.CPR);

                        DataTable emptyTable = _ticketManager.GetDatabaseTableSchema(moduleObj.ModuleTable);
                        DataRow row = emptyTable.NewRow();
                        //copy values from sourceTicket to newTicket
                        row[DatabaseObjects.Columns.ID] = DBNull.Value;
                        row[DatabaseObjects.Columns.Title] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Title]);
                        row[DatabaseObjects.Columns.Description] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Description]);
                        row[DatabaseObjects.Columns.TicketStageActionUsers] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.TicketStageActionUsers]);
                        row[DatabaseObjects.Columns.TicketStageActionUserTypes] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                        row[DatabaseObjects.Columns.TicketPctComplete] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.TicketPctComplete]);
                        row["CategoryLookup"] = UGITUtility.StringToLong(sourceTicketRow["CategoryLookup"]);
                        row[DatabaseObjects.Columns.Address] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.Address]);
                        row[DatabaseObjects.Columns.StreetAddress1] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.StreetAddress1]);
                        row[DatabaseObjects.Columns.City] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.City]);
                        row[DatabaseObjects.Columns.ContactLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.ContactLookup]);
                        // = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.DepartmentLookup]);
                        row[DatabaseObjects.Columns.DivisionLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.DivisionLookup]);
                        //row[DatabaseObjects.Columns.OwnerUser] = UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.OwnerUser]);
                        //row[DatabaseObjects.Columns.ModuleStepLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.ModuleStepLookup]);
                        row[DatabaseObjects.Columns.RequestTypeLookup] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.RequestTypeLookup]);
                        row[DatabaseObjects.Columns.ApproxContractValue] = UGITUtility.StringToDouble(sourceTicketRow[DatabaseObjects.Columns.ApproxContractValue]);
                        row[DatabaseObjects.Columns.PreconStartDate] = sourceTicketRow[DatabaseObjects.Columns.PreconStartDate] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.PreconStartDate]);
                        row[DatabaseObjects.Columns.PreconEndDate] = sourceTicketRow[DatabaseObjects.Columns.PreconEndDate] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.PreconEndDate]);
                        row[DatabaseObjects.Columns.EstimatedConstructionStart] = sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionStart] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        row[DatabaseObjects.Columns.EstimatedConstructionEnd] = sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionEnd] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        row[DatabaseObjects.Columns.CloseoutDate] = sourceTicketRow[DatabaseObjects.Columns.CloseoutDate] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.CloseoutDate]);
                        row[DatabaseObjects.Columns.CloseoutStartDate] = sourceTicketRow[DatabaseObjects.Columns.CloseoutStartDate] == DBNull.Value ? (object)DBNull.Value : UGITUtility.ObjectToString(sourceTicketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                        row[DatabaseObjects.Columns.Closed] = false;
                        row[DatabaseObjects.Columns.CRMDuration] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.CRMDuration]);
                        row[DatabaseObjects.Columns.PreconDuration] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.PreconDuration]);
                        row[DatabaseObjects.Columns.EstimatedConstructionDuration] = UGITUtility.StringToLong(sourceTicketRow[DatabaseObjects.Columns.EstimatedConstructionDuration]);

                        if (sourceTicketRow.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                            row[DatabaseObjects.Columns.BCCISector] = Convert.ToString(sourceTicketRow[DatabaseObjects.Columns.BCCISector]);

                        Ticket ticketRequest = new Ticket(_applicationContext, moduleObj.ModuleName);
                        ticketRequest.Create(row, _applicationContext.CurrentUser);
                        string error = ticketRequest.CommitChanges(row);
                        if (string.IsNullOrEmpty(error))
                        {
                            if (request.Allocations != null && request.Allocations.Count > 0)
                            {
                                foreach (var alloc in request.Allocations)
                                {
                                    alloc.ProjectID = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                                }
                                request.SourceTicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                                request.SourceModule = moduleObj.ModuleName;
                                _projectEstimatedAllocationManager.ImportAllocation(_applicationContext, request.Allocations, request.ProjectStartDate, request.ProjectEndDate);
                            }
                            NewOpportunityResponse newOpportunityResponse = new NewOpportunityResponse();
                            newOpportunityResponse.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                            newOpportunityResponse.Title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                            return Ok(newOpportunityResponse);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateOpportunity: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetSimilarProjects")]
        public async Task<IHttpActionResult> GetSimilarProjects(string TicketId, bool useMinimumSimilarityScore = false)
        {
            await Task.FromResult(0);
            try
            {
                List<SimilarProjectResponse> lstResponse = new List<SimilarProjectResponse>();
                ProjectSimilarityConfigManager projectSimilarityConfigManager = new ProjectSimilarityConfigManager(_applicationContext);
                string moduleName = uHelper.getModuleNameByTicketId(TicketId);
                List<string> columnsRequired = new List<string>() { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ID, DatabaseObjects.Columns.Title,
                DatabaseObjects.Columns.RequestTypeLookup, DatabaseObjects.Columns.BCCISector, DatabaseObjects.Columns.ActualAcquisitionCost,
                DatabaseObjects.Columns.ApproxContractValue, DatabaseObjects.Columns.MarketSector, DatabaseObjects.Columns.PreconDuration, DatabaseObjects.Columns.Status,
                DatabaseObjects.Columns.EstimatedConstructionDuration, DatabaseObjects.Columns.CRMDuration, DatabaseObjects.Columns.PreconStartDate,
                DatabaseObjects.Columns.PreconEndDate, DatabaseObjects.Columns.EstimatedConstructionStart, DatabaseObjects.Columns.EstimatedConstructionEnd,
                DatabaseObjects.Columns.CloseoutDate, DatabaseObjects.Columns.CloseoutStartDate, DatabaseObjects.Columns.ERPJobIDNC, DatabaseObjects.Columns.TagMultiLookup};
                string strProjectComparisonMatrixColor = _configVariableManager.GetValue(ConfigConstants.ProjectComparisonMatrixColor);
                if (!string.IsNullOrWhiteSpace(strProjectComparisonMatrixColor))
                {
                    var items = strProjectComparisonMatrixColor.Split(';');
                    if (items.Length > 0)
                    {
                        scoreColorRanges = new List<ScoreColorRange>();
                        double startRange = 0;    
                        foreach (var item in items)
                        {
                            var values = item.Split('=');

                            scoreColorRanges.Add(new ScoreColorRange { HexColor = values[0].Trim(), MinRange = startRange, MaxRange = Convert.ToDouble(values[1].Trim()) });
                            startRange = Convert.ToDouble(values[1]) + 0.1;
                        }
                    }
                }
                DataRow currentRow = Ticket.GetCurrentTicket(_applicationContext, moduleName, TicketId);
                
                UGITModule ObjuGITModule = null;
                if (moduleName == ModuleNames.CPR || UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.OpportunityTargetChoice]) == "Service")
                    ObjuGITModule = _moduleViewManager.LoadByName(ModuleNames.CNS);
                else
                    ObjuGITModule = _moduleViewManager.LoadByName(ModuleNames.CPR);

                DataTable dataTable = null;
                if (ObjuGITModule.ModuleName == ModuleNames.OPM)
                    dataTable = _ticketManager.GetOpenTickets(ObjuGITModule, null, columnsRequired);
                else
                    dataTable = _ticketManager.GetAllTickets(ObjuGITModule, columnsRequired);

                DataTable dtAllTickets = dataTable.Select($"{DatabaseObjects.Columns.Status} NOT LIKE 'Cancelled'").CopyToDataTable();
                dtAllTickets.Merge(currentRow.Table);

                if (dtAllTickets != null && dtAllTickets.Rows.Count > 0)
                {
                    List<string> lstTicketIds = dtAllTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).ToList();

                    string defaultComparisonMetricType = _configVariableManager.GetValue(ConfigConstants.DefaultComparisonMetricType);
                    if (string.IsNullOrWhiteSpace(defaultComparisonMetricType))
                        defaultComparisonMetricType = "Similarity";

                    List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == ObjuGITModule.ModuleName && x.MetricType.ToLower() == defaultComparisonMetricType.ToLower() && x.Deleted == false).ToList();

                    DataTable prjSimilarityData = new DataTable();
                    string col = string.Empty;
                    if (!prjSimilarityData.Columns.Contains(DatabaseObjects.Columns.TicketId))
                    {
                        prjSimilarityData.Columns.Add(DatabaseObjects.Columns.TicketId);
                    }

                    foreach (DataRow item in dtAllTickets.Rows)
                    {
                        col = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        if (!prjSimilarityData.Columns.Contains(col))
                        {
                            prjSimilarityData.Columns.Add(col);
                        }
                    }

                    var projectComparisonScores = projectSimilarityConfigManager.GetComparisonScore(
                        new DataRow[] { currentRow },
                        dtAllTickets.Rows.OfType<DataRow>().ToArray(),
                        lstProjSimilarityConfig);

                    foreach (var score in projectComparisonScores)
                    {
                        List<object> data = new List<object>();
                        data.Add(score.PrimaryProjectTicketID);
                        foreach (var item in score.SecondaryProjects)
                        {
                            data.Add(item.TotalScore);
                        }
                        prjSimilarityData.Rows.Add(data.ToArray());
                    }

                    if (prjSimilarityData != null)
                    {
                        DataRow selectedRow = prjSimilarityData.Select($"{DatabaseObjects.Columns.TicketId} = '{TicketId}'")[0];
                        if (selectedRow != null)
                        {
                            string minimumSimilarityScore = _configVariableManager.GetValue(ConfigConstants.MinimumSimilarityScore);
                            if (!double.TryParse(minimumSimilarityScore, out double minimumSimilarityScoreValue))
                                minimumSimilarityScoreValue = 30;

                            foreach (DataColumn ticketCol in prjSimilarityData.Columns)
                            {
                                double score = UGITUtility.StringToDouble(selectedRow[ticketCol.ColumnName]);
                                if ((useMinimumSimilarityScore && score >= minimumSimilarityScoreValue) || !useMinimumSimilarityScore)
                                {
                                    DataRow[] similarTicketRowCollection = dtAllTickets.Select($"{DatabaseObjects.Columns.TicketId}='{ticketCol.ColumnName}'");
                                    if (similarTicketRowCollection != null && similarTicketRowCollection.Length > 0)
                                    {
                                        DataRow similarTicketRow = similarTicketRowCollection[0];
                                        SimilarProjectResponse response = new SimilarProjectResponse();
                                        response.TicketId = UGITUtility.ObjectToString(similarTicketRow[DatabaseObjects.Columns.TicketId]);
                                        response.Title = UGITUtility.ObjectToString(similarTicketRow[DatabaseObjects.Columns.Title]);
                                        response.Duration = UGITUtility.StringToInt(similarTicketRow[DatabaseObjects.Columns.CRMDuration]);
                                        response.ERPJobIDNC = UGITUtility.ObjectToString(similarTicketRow[DatabaseObjects.Columns.ERPJobIDNC]);

                                        List<ProjectEstimatedAllocation> lstprojectEstimateallocations = _projectEstimatedAllocationManager.Load(x => x.TicketId == response.TicketId);
                                        response.Resources = lstprojectEstimateallocations.Count();

                                        response.Score = score;

                                        var colorRange = scoreColorRanges?.Where(x => x.MinRange <= response.Score && x.MaxRange >= response.Score).FirstOrDefault();
                                        if (colorRange != null)
                                        {
                                            response.ColorCode = UGITUtility.ObjectToString(colorRange.HexColor);
                                        }
                                        if (!uHelper.IsProjectDatesOverLappingOrInValid(similarTicketRow) && response.Resources > 0)
                                            lstResponse.Add(response);
                                    }

                                }
                            }
                        }
                        return Ok(lstResponse.OrderByDescending(x => x.Score));
                    }
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSimilarProjects: " + ex);
            }
            return Ok();
        }

        [HttpPost]
        [Route("CreateQuickOpportunity")]
        public async Task<IHttpActionResult> CreateQuickOpportunity([FromBody] NewQuickOpportunityResponse newQuickOpportunityResponse)
        {
            await Task.FromResult(0);
            try
            {
                DataTable data = _ticketManager.GetDatabaseTableSchema(_moduleViewManager.GetByName(ModuleNames.OPM).ModuleTable);

                DataRow row = data.NewRow();
                row[DatabaseObjects.Columns.ProjectName] = UGITUtility.ObjectToString(newQuickOpportunityResponse.ProjectName);
                row[DatabaseObjects.Columns.Title] = UGITUtility.ObjectToString(newQuickOpportunityResponse.ProjectName);
                row[DatabaseObjects.Columns.ContactLookup] = UGITUtility.ObjectToString(newQuickOpportunityResponse.Client);
                row[DatabaseObjects.Columns.RequestTypeLookup] = UGITUtility.StringToLong(newQuickOpportunityResponse.RequestTypeLookup);
                row[DatabaseObjects.Columns.ERPJobID] = UGITUtility.ObjectToString(newQuickOpportunityResponse.CMIC);
                row[DatabaseObjects.Columns.ERPJobIDNC] = UGITUtility.ObjectToString(newQuickOpportunityResponse.NCO);
                row[DatabaseObjects.Columns.BidDueDate] = newQuickOpportunityResponse.DueDate == DateTime.MinValue ? (object)DBNull.Value : UGITUtility.ObjectToString(newQuickOpportunityResponse.DueDate);
                row[DatabaseObjects.Columns.EstimatedConstructionStart] = newQuickOpportunityResponse.ConstStartDate == DateTime.MinValue ? (object)DBNull.Value : UGITUtility.ObjectToString(newQuickOpportunityResponse.ConstStartDate);
                row[DatabaseObjects.Columns.OwnerContractTypeChoice] = UGITUtility.ObjectToString(newQuickOpportunityResponse.ContractType);
                row[DatabaseObjects.Columns.ApproxContractValue] = UGITUtility.StringToDouble(newQuickOpportunityResponse.ContractValue);
                row[DatabaseObjects.Columns.Address] = UGITUtility.ObjectToString(newQuickOpportunityResponse.Address);
                row[DatabaseObjects.Columns.City] = UGITUtility.ObjectToString(newQuickOpportunityResponse.City);
                row[DatabaseObjects.Columns.StateLookup] = UGITUtility.StringToLong(newQuickOpportunityResponse.State);
                row[DatabaseObjects.Columns.Zip] = UGITUtility.ObjectToString(newQuickOpportunityResponse.Zip);
                row[DatabaseObjects.Columns.UsableSqFt] = UGITUtility.StringToInt(newQuickOpportunityResponse.UsableSqFt);//need To discuss
                row[DatabaseObjects.Columns.RetailSqftNum] = UGITUtility.StringToInt(newQuickOpportunityResponse.RetailSqftNum);//need To discuss
                row[DatabaseObjects.Columns.CRMProjectComplexity] = UGITUtility.ObjectToString(newQuickOpportunityResponse.Complexity);
                row[DatabaseObjects.Columns.OpportunityTypeChoice] = UGITUtility.ObjectToString(newQuickOpportunityResponse.OPMType);//need To discuss
                row[DatabaseObjects.Columns.ChanceOfSuccess] = UGITUtility.ObjectToString(newQuickOpportunityResponse.WinLikeHood);//need To discuss
                row[DatabaseObjects.Columns.Description] = UGITUtility.ObjectToString(newQuickOpportunityResponse.Description);
                row[DatabaseObjects.Columns.Closed] = false;
                row[DatabaseObjects.Columns.OpportunityTargetChoice] = "Project";

                Ticket ticketRequest = new Ticket(_applicationContext, ModuleNames.OPM);
                ticketRequest.Create(row, _applicationContext.CurrentUser);
                string error = ticketRequest.CommitChanges(row);
                if (string.IsNullOrEmpty(error))
                {
                    NewOpportunityResponse newOpportunityResponse = new NewOpportunityResponse();
                    newOpportunityResponse.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                    newOpportunityResponse.Title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                    return Ok(newOpportunityResponse);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateQuickOpportunity: " + ex);
                return InternalServerError();
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetStateDetails")]
        public async Task<IHttpActionResult> GetStateDetails()
        {
            await Task.FromResult(0);
            try
            {
                var lstlifeCycles = _StateManager.Load();
                if (lstlifeCycles != null)
                {
                    string jsonLifeCycles = JsonConvert.SerializeObject(lstlifeCycles);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonLifeCycles, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStateDetails: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetRequestTypeList")]
        public async Task<IHttpActionResult> GetRequestTypeList()
        {
            await Task.FromResult(0);
            try
            {
                UGITModule module = _moduleViewManager.GetByName(ModuleNames.OPM);
                var RequestTypeList = module.List_RequestTypes.ToList();
                if (RequestTypeList != null)
                {
                    string jsonLifeCycles = JsonConvert.SerializeObject(RequestTypeList);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonLifeCycles, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRequestTypeList: " + ex);
                return InternalServerError();
            }
            return Ok();
        }


        [HttpGet]
        [Route("GetChoiceField")]
        public async Task<IHttpActionResult> GetChoiceField(string ChoiceField)
        {
            await Task.FromResult(0);
            try
            {
                FieldConfigurationManager configManger = new FieldConfigurationManager(_applicationContext);
                var ChanceOfSuccessChoice = configManger.GetFieldDataByFieldName(ChoiceField);
                if (ChanceOfSuccessChoice != null)
                {
                    string jsonLifeCycles = JsonConvert.SerializeObject(ChanceOfSuccessChoice);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonLifeCycles, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChoiceField: " + ex);
                return InternalServerError();
            }
            return Ok();
        }


        [HttpGet]
        [Route("GetClientContactDetails")]
        public async Task<IHttpActionResult> GetClientContactDetails()
        {
            await Task.FromResult(0);
            try
            {
                UGITModule comModule = _moduleViewManager.LoadByName(ModuleNames.CON);
                List<string> collist = new List<string>(new string[] { DatabaseObjects.Columns.ID, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title });
                var lstlifeCycles = _ticketManager.GetAllTickets(comModule, collist);

                if (lstlifeCycles != null)
                {
                    string jsonLifeCycles = JsonConvert.SerializeObject(lstlifeCycles);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonLifeCycles, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetClientContactDetails: " + ex);
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("OverrideCurrentAllocations")]
        public async Task<IHttpActionResult> OverrideCurrentAllocations(string SourceOPMId, String TargetOPMId)
        {
            await Task.FromResult(0);

            return Ok();
        }

        [HttpPost]
        [Route("UpdateLeadUserFields")]
        public async Task<IHttpActionResult> UpdateLeadUserFields(string TicketId, string ProjectLead, string LeadEstimator, string Superintendent)
        {
            await Task.FromResult(0);
            if (string.IsNullOrEmpty(TicketId))
                return Ok("Invalid TicketId");

            List<string> errorMessages = new List<string>();

            try
            {
                string moduleName = uHelper.getModuleNameByTicketId(TicketId);
                UGITModule uGITModule = _moduleViewManager.GetByName(moduleName);

                DataRow projectRow = Ticket.GetCurrentTicket(_applicationContext, moduleName, TicketId);

                List<ProjectEstimatedAllocation> existingAllocations = _projectEstimatedAllocationManager.Load(x => x.TicketId == TicketId);
                List<string> existingAllocatedUserIDs = existingAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                List<string> newUserIDs = new List<string>() { ProjectLead, LeadEstimator, Superintendent }.Where(x => !string.IsNullOrWhiteSpace(x) && x != "null").Distinct().ToList();


                string defaultAllocationPercentage = _configVariableManager.GetValue(ConfigConstants.DefaultAllocationPercentage);
                if (string.IsNullOrWhiteSpace(defaultAllocationPercentage))
                {
                    errorMessages.Add($"No Default Allocation Percentage value is set.");
                }
                else
                {
                    bool isValidDates = true;
                    DateTime AllocationStartDate = DateTime.Now;
                    DateTime AllocationEndDate = DateTime.Now;

                    if ((!string.IsNullOrEmpty(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.PreconStartDate])))
                            && (!string.IsNullOrEmpty(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.PreconEndDate]))))
                    {
                        AllocationStartDate = Convert.ToDateTime(projectRow[DatabaseObjects.Columns.PreconStartDate]);
                        AllocationEndDate = Convert.ToDateTime(projectRow[DatabaseObjects.Columns.PreconEndDate]);
                    }
                    else if ((!string.IsNullOrEmpty(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.EstimatedConstructionStart])))
                        && (!string.IsNullOrEmpty(UGITUtility.ObjectToString(projectRow[DatabaseObjects.Columns.EstimatedConstructionEnd]))))
                    {
                        AllocationStartDate = Convert.ToDateTime(projectRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        AllocationEndDate = Convert.ToDateTime(projectRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    }
                    else
                    {
                        errorMessages.Add($"No Precon OR Construction dates set in the Project.");
                        isValidDates = false;
                    }

                    if (isValidDates)
                    {
                        List<UserWithPercentage> allocationWithPercentageList = new List<UserWithPercentage>();
                        List<string> historyList = new List<string>();
                        foreach (string userID in newUserIDs)
                        {
                            var profile = _userProfileManager.Load(x => x.Id == userID).FirstOrDefault();
                            if (profile == null)
                            {
                                errorMessages.Add($"Resource: {profile.Name} - Not Found.");
                                continue;
                            }
                            if (existingAllocatedUserIDs.Contains(userID))
                            {
                                //errorMessages.Add($"Resource: <b>{profile.Name}</b> - Allocation already exists.");
                                continue;
                            }

                            GlobalRole globalRole = _roleManager.Get(x => x.Id.Equals(profile.GlobalRoleId, System.StringComparison.InvariantCultureIgnoreCase));
                            if (globalRole == null)
                            {
                                errorMessages.Add($"Resource: {profile.Name} - Global Role is not set.");
                                continue;
                            }

                            //if (profile != null && (AllocationStartDate < profile.UGITStartDate || AllocationEndDate > profile.UGITEndDate))
                            //{
                            //    errorMessages.Add(string.Format("Resource: {0} - Allocation date entered is either prior to start date or after the end date of the resource. <br/>Start Date: {1} End Date: {2}", profile.Name, profile.UGITStartDate.ToShortDateString(), profile.UGITEndDate.ToShortDateString()));
                            //    continue;
                            //}

                            ULog.WriteLog($"Resource: {userID} - Creating allocation.");


                            int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_applicationContext, AllocationStartDate, AllocationEndDate);
                            int noOfWeeks = uHelper.GetWeeksFromDays(_applicationContext, noOfWorkingDays);

                            ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                            crmAllocation.AllocationStartDate = AllocationStartDate;
                            crmAllocation.AllocationEndDate = AllocationEndDate;
                            crmAllocation.AssignedTo = userID;
                            crmAllocation.PctAllocation = Convert.ToDouble(defaultAllocationPercentage);//config variable
                            crmAllocation.Type = globalRole.Id;//global role
                            crmAllocation.Duration = noOfWeeks;
                            crmAllocation.Title = null;
                            crmAllocation.TicketId = TicketId;
                            crmAllocation.SoftAllocation = uGITModule.IsAllocationTypeHard_Soft;
                            crmAllocation.NonChargeable = false;
                            crmAllocation.IsLocked = false;
                            crmAllocation.ID = 0;

                            if (_projectEstimatedAllocationManager.Insert(crmAllocation) > 0)
                            {
                                allocationWithPercentageList.Add(
                                    new UserWithPercentage()
                                    {
                                        EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue,
                                        StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue,
                                        Percentage = crmAllocation.PctAllocation,
                                        UserId = crmAllocation.AssignedTo,
                                        RoleTitle = globalRole.Name,
                                        ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID),
                                        RoleId = crmAllocation.Type,
                                        SoftAllocation = crmAllocation.SoftAllocation,
                                        NonChargeable = crmAllocation.NonChargeable,
                                    });

                                ULog.WriteLog($"Resource: {userID} - Allocation created.");
                                historyList.Add($"Created new allocation for user: {profile.Name} - {globalRole.Name} {crmAllocation.PctAllocation}% {String.Format("{0:MM/dd/yyyy}", crmAllocation.AllocationStartDate)}-{String.Format("{0:MM/dd/yyyy}", crmAllocation.AllocationEndDate)}");
                            }
                        }
                        List<string> allUserIDs = allocationWithPercentageList.Select(x => x.UserId).Distinct().ToList();
                        if (allUserIDs.Count > 0)
                        {
                            List<ProjectTag> tags = _userProjectExperienceManager.GetProjectExperienceTags(TicketId, false);
                            List<string> tagIDs = tags?.Select(x => x.TagId).ToList() ?? null;
                            _userProjectExperienceManager.UpdateUserProjectTagExperience(tagIDs, TicketId);

                            ResourceAllocationManager.CPRResourceAllocation(_applicationContext, moduleName, TicketId, allocationWithPercentageList, existingAllocatedUserIDs.Union(allUserIDs).ToList());
                            ResourceAllocationManager.UpdateHistory(_applicationContext, historyList, TicketId, null, true);
                            historyList.ForEach(o =>
                            {
                                ULog.WriteLog("PT >> " + _applicationContext.CurrentUser.Name + o);
                            });
                        }
                    }
                }

                projectRow[DatabaseObjects.Columns.ProjectLeadUser] = ProjectLead;
                projectRow[DatabaseObjects.Columns.LeadEstimatorUser] = LeadEstimator;
                projectRow[DatabaseObjects.Columns.LeadSuperintendentUser] = Superintendent;
                Ticket ticketRequest = new Ticket(_applicationContext, moduleName);
                string error = ticketRequest.CommitChanges(projectRow);
                if (string.IsNullOrEmpty(error))
                {
                    string projectleadname = _applicationContext.UserManager.GetUserProfile(ProjectLead)?.Name;
                    string leadestimatorname = _applicationContext.UserManager.GetUserProfile(LeadEstimator)?.Name;
                    string leadsuperintendent = _applicationContext.UserManager.GetUserProfile(Superintendent)?.Name;

                    List<UpdateLeadUsersResponse> lstResponse = new List<UpdateLeadUsersResponse>
                    {
                        new UpdateLeadUsersResponse
                        {
                            Field = DatabaseObjects.Columns.ProjectLeadUser,
                            Name = projectleadname,
                            UserId = ProjectLead
                        },
                        new UpdateLeadUsersResponse
                        {
                            Field = DatabaseObjects.Columns.LeadEstimatorUser,
                            Name = leadestimatorname,
                            UserId = LeadEstimator
                        },
                            new UpdateLeadUsersResponse
                        {
                            Field = DatabaseObjects.Columns.LeadSuperintendentUser,
                            Name = leadsuperintendent,
                            UserId = Superintendent,
                        }
                    };

                    return Ok(new { IsSuccess = true, Data = lstResponse, ErrorMessages = errorMessages });
                }
                return Ok(new { IsSuccess = false, Data = new List<UpdateLeadUsersResponse>(), ErrorMessages = errorMessages });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateLeadUserFields: " + ex);
                //ULog.WriteException(ex);
                return Ok(new { IsSuccess = false, Data = new List<UpdateLeadUsersResponse>(), ErrorMessages = errorMessages });

            }
        }
    }
    public class NewOpportunityResponse
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
    }
    public class NewQuickOpportunityResponse
    {
        public string ProjectName { get; set; }
        public string Client { get; set; }
        public string RequestTypeLookup { get; set; }
        public string CMIC { get; set; }
        public string NCO { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ConstStartDate { get; set; }
        public string ContractType { get; set; }
        public string ContractValue { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string UsableSqFt { get; set; }
        public string RetailSqftNum { get; set; }
        public string Complexity { get; set; }
        public string OPMType { get; set; }
        public string WinLikeHood { get; set; }
        public string Description { get; set; }

    }

    public class CreateOpportunityRequest
    {
        public string SourceTicketId { get; set; }
        public string SourceModule { get; set; }
        public string AllocationType { get; set; }
        public string TemplateID { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string OpportunityTarget { get; set; }
        public List<AllocationTemplateModel> Allocations { get; set; }
    }
}
