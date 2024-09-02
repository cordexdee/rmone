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
using uGovernIT.Utility;
using uGovernIT.Manager.JobSchedulers;
 
using uGovernIT.DxReport;
using uGovernIT.Util.Log;

namespace uGovernIT.Controllers
{
    [RoutePrefix("api/reportapi")]
    public class ReportController : ApiController
    {
        [HttpGet]
        [Route("GetReportPath")]
        public async Task<IHttpActionResult> GetReportPath(string scheduledActionID, string tenantId)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = ApplicationContext.CreateContext(tenantId);
                long itemId = UGITUtility.StringToLong(scheduledActionID);
                string reportPath = string.Empty;

                if (itemId > 0)
                {
                    ReportAgentJobHelper reportAgentHelper = new ReportAgentJobHelper(context);
                    reportPath = reportAgentHelper.GetReport(itemId);
                }

                //string reportJson = JsonConvert.SerializeObject(reportPath);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(reportPath, Encoding.UTF8, "text/plain");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetReportPath: " + ex);
                return InternalServerError();
            }
        }
    }
}