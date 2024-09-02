using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using uGovernIT.Utility;
using System.IO;
using uGovernIT.Manager.PMM;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/pmmapi")]
    public class PMMController : ApiController
    {
        // The structure that represents chunk details
        public class ChunkMetadata
        {
            public int Index { get; set; }
            public int TotalCount { get; set; }
            public int FileSize { get; set; }
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileGuid { get; set; }
        }

        /// <summary>
        /// This API is used to get PMM projects which have the title similar to the input title
        /// </summary>
        /// <param name="title">input title</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSuggestedPMM")]
        public async Task<IHttpActionResult> GetSuggestedPMM(string title)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                TicketManager tManager = new TicketManager(context);

                if (string.IsNullOrEmpty(title))
                    return Ok();

                // Get All PMM tickets and filter them on the basis of title
                var query = $"select ID, Title, TicketId from PMM where ({DatabaseObjects.Columns.Title} like '%{title}%')";
                DataTable dtAllTickets = tManager.GetTickets(query);

                if (dtAllTickets != null)
                {
                    // Pick a max. of 5 records and create response
                    if (dtAllTickets.Rows.Count > 0)
                        dtAllTickets = dtAllTickets.AsEnumerable().Take(5).CopyToDataTable();

                    string pmmTicketsJson = JsonConvert.SerializeObject(dtAllTickets);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(pmmTicketsJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSuggestedPMM: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to get PMM projects which are similar to the project imported through MPP file
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMPPSuggestedPMM")]
        public async Task<IHttpActionResult> GetMPPSuggestedPMM(string title, string path)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                TicketManager tManager = new TicketManager(context);
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                DataTable dtAllTickets = tManager.GetAllTickets(moduleManager.GetByName("PMM"));

                if (dtAllTickets != null)
                {
                    if (dtAllTickets.Rows.Count > 0)
                        dtAllTickets = dtAllTickets.AsEnumerable().Take(5).CopyToDataTable();

                    string pmmTicketsJson = JsonConvert.SerializeObject(dtAllTickets);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(pmmTicketsJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetMPPSuggestedPMM: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API mehtod is used to Get NPR tickets which can be used to create PMM projects
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNPR")]
        public async Task<IHttpActionResult> GetNPR([FromUri] NPRorTemplateRequest moduleRequest)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                TicketManager tManager = new TicketManager(context);
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                UGITModule mDetails = moduleManager.LoadByName("NPR");
                string ticketStage = string.Empty;

                if (mDetails.List_LifeCycles.Count > 1)
                {
                    ticketStage = mDetails.List_LifeCycles.FirstOrDefault(x => x.ID == 0).Stages.FirstOrDefault(x => x.Prop_ReadyToImport == true).Name;
                }
                else if (mDetails.List_LifeCycles.Count == 1)
                {
                    ticketStage = mDetails.List_LifeCycles[0].Stages.FirstOrDefault(x => x.Prop_ReadyToImport == true).Name;
                }

                // Get All NPR tickets which are eligible to be Imported into PMM
                var query = string.Format("select ID, Title, TicketId from NPR where {0}='{3}' and ({1} is null or {1}= 0) and ({2} is null or {2} = 0) and {4} = '{5}'",
                    DatabaseObjects.Columns.Status, DatabaseObjects.Columns.TicketPMMIdLookup, DatabaseObjects.Columns.TicketOnHold, ticketStage, DatabaseObjects.Columns.TenantID, context.TenantID); // $"select Title, TicketId from NPR where ({DatabaseObjects.Columns.Title} like '%{titleText}%')";
                DataTable dtAllTickets = tManager.GetTickets(query);

                if (dtAllTickets != null)
                {
                    DataTable result = dtAllTickets.Clone();
                    DataRow[] similarTitleItems = null;

                    if (dtAllTickets.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(moduleRequest.Title))
                        {
                            similarTitleItems = dtAllTickets.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Title).ToLower().Contains(moduleRequest.Title)).ToArray();
                        }

                        if (similarTitleItems != null && similarTitleItems.Length > 0)
                        {
                            result = similarTitleItems.CopyToDataTable();
                            DataRow[] nonSimilar = dtAllTickets.AsEnumerable().Where(x => !x.Field<string>(DatabaseObjects.Columns.Title).ToLower().Contains(moduleRequest.Title)).ToArray();

                            if (nonSimilar != null && nonSimilar.Length > 0)
                                result.Merge(nonSimilar.CopyToDataTable());
                        }
                        else
                        {
                            result = dtAllTickets;
                        }

                        if (moduleRequest.Paging > 0)
                            result = result.AsEnumerable().Skip(moduleRequest.Start).Take(moduleRequest.Paging).CopyToDataTable();
                    }

                    string nprTicketsJson = JsonConvert.SerializeObject(result);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(nprTicketsJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetNPR: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to get Task Templates to create PMM projects
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTemplates")]
        public async Task<IHttpActionResult> GetTemplates([FromUri] NPRorTemplateRequest moduleRequest)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                TaskTemplateManager tManager = new TaskTemplateManager(context);
                DataTable dtTaskTemplates = tManager.GetDataTable();

                if (dtTaskTemplates != null)
                {
                    DataView view = dtTaskTemplates.DefaultView;
                    dtTaskTemplates.DefaultView.Sort = "Title asc";
                    DataTable result = view.ToTable(true, new string[] { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ID });

                    // Apply skip and paging to fetch data for current page of grid
                    if (dtTaskTemplates.Rows.Count > 0 && moduleRequest.Paging > 0)
                        result = result.AsEnumerable().Skip(moduleRequest.Start).Take(moduleRequest.Paging).CopyToDataTable();

                    string taskTemplatesJson = JsonConvert.SerializeObject(result);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(taskTemplatesJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTemplates: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to create a PMM project
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateProject")]
        public async Task<IHttpActionResult> CreateProject(NewProjectRequest moduleRequest)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ProjectManager projectManager = new ProjectManager(context);
                string message = string.Empty;

                if (!ModelState.IsValid && ModelState["Title"] != null && ModelState["Title"].Errors.Count > 0)
                {
                    message = ModelState["Title"].Errors[0].ErrorMessage;
                    return BadRequest(message);
                }

                if (moduleRequest.Mode != PMMMode.Scratch && string.IsNullOrEmpty(moduleRequest.SelectedItem))
                {
                    message = "Selected item is not defined.";
                    ULog.WriteLog(message);
                    return BadRequest(message);
                }


                // Create PMM project
                TicketManager tManager = new TicketManager(context);

                message = projectManager.CreatePMM(moduleRequest, tManager);

                DocumentManagementController documentManagement = new DocumentManagementController();

                if (string.IsNullOrEmpty(message))
                {
                    message = "Error occurred.";
                    return BadRequest(message);
                }
                else if (message.Contains("PMM-"))        // Check if message contains PMM TicketID
                {
                    if (moduleRequest.IscreateDocumentPortal)
                    {
                        documentManagement.Index(Convert.ToString(moduleRequest.IsDefaultFolder), message);//Passing ticketID through message

                    }

                    return Ok(message);
                }
                else
                {
                    return BadRequest(message);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateProject: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to upload 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UploadManager uManager = new UploadManager(context);
            HttpRequest request = HttpContext.Current.Request;
            string savedFileName = string.Empty;
            try
            {
                if (request == null || request.Files.Count == 0)
                    return BadRequest("File not valid or uploaded");

                string chunkMetadata = UGITUtility.ObjectToString(request.Params["chunkMetadata"]);

                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    // Gets chunk details
                    ChunkMetadata metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);
                    string uploadedFilePath = uHelper.GetTempFolderPath();
                    var uploadFile = request.Files[0];
                    // Creates a directory for temporary files if it does not exist
                    if (!Directory.Exists(uploadedFilePath))
                        Directory.CreateDirectory(uploadedFilePath);

                    var tempmppFilePath = Path.Combine(uploadedFilePath, metaDataObject.FileGuid + ".mpp");

                    // Appends the chunk to the file
                    UGITUtility.AppendContentToFile(tempmppFilePath, uploadFile);

                    // Saves the file if all chunks are received
                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1))
                    {
                        savedFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + metaDataObject.FileName;
                        System.IO.File.Copy(tempmppFilePath, Path.Combine(uploadedFilePath, savedFileName));
                    }
                    return Ok(savedFileName);
                }

            }
            catch (Exception ex)
            {
                // ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in UploadFile: " + ex);
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// This API is used to Get Tasks for a Task Template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTemplateTasks")]
        public async Task<IHttpActionResult> GetTemplateTasks([FromUri] NPRorTemplateRequest request)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();

                UGITTaskManager tskManager = new UGITTaskManager(context);
                List<UGITTask> taskTemplates = tskManager.LoadByTemplateID(UGITUtility.StringToLong(request.Title));

                if (taskTemplates != null)
                {
                    if (taskTemplates.Count > 0 && request.Paging > 0)
                        taskTemplates = taskTemplates.OrderBy(x => x.Title).Skip(request.Start).Take(request.Paging).ToList();
                    taskTemplates = taskTemplates.OrderBy(x => x.Title).ToList();
                    string path = JsonConvert.SerializeObject(taskTemplates);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(path, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTemplateTasks: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to Get Root Tasks for a PMM project
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRootTasks")]
        public async Task<IHttpActionResult> GetRootTasks(string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();

                UGITTaskManager tskManager = new UGITTaskManager(context);
                string query = $"({DatabaseObjects.Columns.TicketId}= '{ticketId}' and {DatabaseObjects.Columns.ParentTaskID}= 0 and {DatabaseObjects.Columns.Deleted}= 0)";
                List<UGITTask> lstRootTasks = tskManager.Load(query);

                if (lstRootTasks != null)
                {
                    DataView view = UGITUtility.ToDataTable(lstRootTasks).DefaultView;
                    DataTable result = view.ToTable(true, new string[] { DatabaseObjects.Columns.ID, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.PercentComplete, DatabaseObjects.Columns.Status });

                    string path = JsonConvert.SerializeObject(result);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(path, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRootTasks: " + ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This API is used to Get Root Tasks for a PMM project
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTaskWorkflowData")]
        public async Task<IHttpActionResult> GetTaskWorkflowData(string ticketId, string moduleName)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();

                if (string.IsNullOrEmpty(ticketId) || string.IsNullOrEmpty(moduleName))
                    return BadRequest("Either TicketId or ModuleName is empty.");

                UGITTaskManager tskManager = new UGITTaskManager(context);

                // Get Root task which aren't deleted
                string query = $"({DatabaseObjects.Columns.TicketId}= '{ticketId}' and {DatabaseObjects.Columns.ModuleName}= '{moduleName}' and {DatabaseObjects.Columns.ParentTaskID}= 0 and {DatabaseObjects.Columns.Deleted}= 0)";
                List<UGITTask> lstRootTasks = tskManager.Load(query);

                if (lstRootTasks != null)
                {
                    lstRootTasks = lstRootTasks.OrderBy(x => x.ItemOrder).ToList();
                    DataTable workflowData = UGITUtility.ToDataTable(lstRootTasks);
                    workflowData.Columns.Add("from");
                    workflowData.Columns.Add("to");
                    int index = 0;

                    while (index < workflowData.Rows.Count - 1)
                    {
                        DataRow row = workflowData.Rows[index];
                        row["from"] = Convert.ToString(row[DatabaseObjects.Columns.ID]);
                        row["to"] = Convert.ToString(workflowData.Rows[index + 1][DatabaseObjects.Columns.ID]);
                        index++;
                    }

                    var nodes = (from ft in workflowData.AsEnumerable()
                                 select new
                                 {
                                     id = ft.Field<string>(DatabaseObjects.Columns.ID),
                                     text = UGITUtility.TruncateWithEllipsis(ft.Field<string>(DatabaseObjects.Columns.Title), 18),
                                     type = "roottask",
                                     title = ft.Field<string>(DatabaseObjects.Columns.Title),
                                     pctComplete = ft.Field<string>(DatabaseObjects.Columns.PercentComplete),
                                     status = ft.Field<string>(DatabaseObjects.Columns.Status),
                                     dueDate = ft.Field<DateTime>(DatabaseObjects.Columns.DueDate)
                                 }).ToArray();

                    index = 0;
                    var edges = (from edge in workflowData.AsEnumerable()
                                 where edge.Field<string>("from") != null && edge.Field<string>("from") != string.Empty && edge.Field<string>("to") != null && edge.Field<string>("to") != string.Empty
                                 select new
                                 {
                                     id = ++index,
                                     fromId = edge.Field<string>("from"),
                                     to = edge.Field<string>("to")
                                 }).ToArray();

                    var data = new
                    {
                        Nodes = nodes,
                        Links = edges
                    };

                    string path = JsonConvert.SerializeObject(data, Formatting.Indented);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(path, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTaskWorkflowData: " + ex);
            }
            return BadRequest("Error occurred.");

        }

        /// <summary>
        /// This API is used to update the title of a PMM project
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateProjectTitle")]
        public async Task<IHttpActionResult> UpdateProjectTitle(string title, string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                string error = string.Empty;

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(ticketId))
                {
                    error = "Either title or ticketId is empty.";
                    ULog.WriteLog(error);
                    return BadRequest(error);
                }

                string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                UGITModule module = moduleManager.GetByName(moduleName);
                TicketManager tManager = new TicketManager(context);
                DataRow currentTicket = tManager.GetByTicketID(module, ticketId);

                if (currentTicket == null)
                {
                    error = "Ticket is not found.";
                    ULog.WriteLog(error);
                    return BadRequest(error);
                }

                currentTicket[DatabaseObjects.Columns.Title] = title;
                Ticket objTicket = new Ticket(context, moduleName, context.CurrentUser);
                error = objTicket.CommitChanges(currentTicket);

                if (!string.IsNullOrEmpty(error))
                {
                    ULog.WriteLog(error);
                    return BadRequest(error);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateProjectTitle: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("UploadProjectFile")]
        public async Task<IHttpActionResult> UploadProjectFile()
        {
            await Task.FromResult(0);
            HttpRequest request = HttpContext.Current.Request;
            return Ok();
        }
    }
}
