using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/widget")]
    public class WidgetsController : ApiController
    {

        private ApplicationContext _applicationContext  { get; set; }
        private AgentsManager _agentsManager { get; set; }
        private WidgetsHelper _widgetsHelper { get; set; }
        private WidgetRunResponse _widgetRunResponse { get; set; }

        public WidgetsController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _agentsManager = new AgentsManager(_applicationContext);
            _widgetsHelper = new WidgetsHelper();
            _widgetRunResponse = new WidgetRunResponse();
        }

        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}


        [Route("GetWidgetsResponse")]
        [HttpGet]
        public async Task<IHttpActionResult> GetWidgetsResponse(string widgetId)
        {
            await Task.FromResult(0);
            try
            {
                if (string.IsNullOrEmpty(widgetId))
                    return Ok(HttpStatusCode.BadRequest);

                long.TryParse(widgetId, out long lwidgetId);
                _widgetRunResponse = _widgetsHelper.getWidgetResponse(lwidgetId);
                string jsonWidgetRunResponse = JsonConvert.SerializeObject(_widgetRunResponse);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonWidgetRunResponse, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWidgetsResponse: " + ex);
                return InternalServerError();
            }

        }
    }
}