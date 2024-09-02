using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/kanbanapi")]
    public class ProjectKanBanController : ApiController
    {
        private ApplicationContext _context = null;

        protected ApplicationContext context
        {
            get
            {
                if (_context == null)
                {
                    _context = System.Web.HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        [Route("GetTask")]
        public IHttpActionResult GetTask(string TicketID)
        {
            try
            {
               // await Task.FromResult(0);
                if (string.IsNullOrEmpty(TicketID))
                    return BadRequest();

                    ProjectTaskKanabanHelper projectTaskKanabanHelper = new ProjectTaskKanabanHelper(context);
                    List<KanaBanTaskNode> kanaBanTaskNode = new List<KanaBanTaskNode>();
                    kanaBanTaskNode = projectTaskKanabanHelper.getAllTaskOfTicket(TicketID);

                    if(kanaBanTaskNode != null)
                    {
                    var resultList = kanaBanTaskNode.OrderBy(x => x.ItemOrder);
                    string usersJson = JsonConvert.SerializeObject(resultList);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(usersJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                   }

                return Ok();

            }
            catch (Exception ex)
            {
                //Util.Log.ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetTask: " + ex);
                return null;
            }
        }

        [Route("GetCategory")]
        public  IHttpActionResult GetCategory(string TicketID,string CategoryID)
        {
            try
            {
                if (string.IsNullOrEmpty(TicketID) || string.IsNullOrEmpty(CategoryID))
                    return BadRequest();

                ProjectTaskKanabanHelper projectTaskKanabanHelper = new ProjectTaskKanabanHelper(context);
                List<KanaBanTaskNode> kanaBanTaskNode = new List<KanaBanTaskNode>();
                kanaBanTaskNode = projectTaskKanabanHelper.getAllTaskOfTicket(TicketID);

                if (kanaBanTaskNode != null)
                {
                    var resultList = kanaBanTaskNode.Where(x=>x.ID == Convert.ToInt64(CategoryID));
                    string usersJson = JsonConvert.SerializeObject(resultList);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(usersJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {

                //Util.Log.ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetCategory: " + ex);
                return null;
            }
        }


        [Route("UpdateCategory")]
        public IHttpActionResult UpdateCategory(string TaskID, string Category)
        {
            try
            {
                // await Task.FromResult(0);

                List<KanBanCategory> lstKanBanCategory = new List<KanBanCategory>();
                KanBanCategory kanBanCategory1 = new KanBanCategory();
                KanBanCategory kanBanCategory2 = new KanBanCategory();
                KanBanCategory kanBanCategory3 = new KanBanCategory();

                kanBanCategory1.ID = 1;
                kanBanCategory2.ID = 2;
                kanBanCategory3.ID = 3;

                kanBanCategory1.Name = "Not Started";
                kanBanCategory2.Name = "Waiting";
                kanBanCategory3.Name = "In Progress";

                lstKanBanCategory.Add(kanBanCategory1);
                lstKanBanCategory.Add(kanBanCategory2);
                lstKanBanCategory.Add(kanBanCategory3);


                // string usersJson = JsonConvert.SerializeObject(lstKanBanCategory);
                var response = this.Request.CreateResponse(HttpStatusCode.OK, lstKanBanCategory);
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateCategory: " + ex);
                //Util.Log.ULog.WriteException(ex);
                return null;
            }
        }

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


    }
}