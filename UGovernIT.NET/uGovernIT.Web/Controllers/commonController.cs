using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/common")]
    public class commonController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [HttpGet]
        [Route("GetRoleDetails")]
        public async Task<IHttpActionResult> GetRoleDetails(string deptid = null, string roleid = null)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@TenantID", context.TenantID);
                arrParams.Add("@deptid", deptid);
                arrParams.Add("@roleid", roleid);
                DataSet dtResultBillings = uGITDAL.ExecuteDataSet_WithParameters("usp_GetRole", arrParams);
                if (dtResultBillings != null && dtResultBillings.Tables.Count > 0)
                {
                    //strTables = JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);
                    //return Ok(JsonConvert.SerializeObject(dtResultBillings, Newtonsoft.Json.Formatting.Indented));

                    return Ok(dtResultBillings);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRoleDetails: " + ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("GetJobTitle")]
        public async Task<IHttpActionResult> GetJobTitle(string deptid = null, string roleid = null)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("@TenantID", context.TenantID);
                arrParams.Add("@deptid", deptid);
                arrParams.Add("@roleid", roleid);
                DataSet dtResultBillings = uGITDAL.ExecuteDataSet_WithParameters("USP_GetJobTitle", arrParams);
                if (dtResultBillings != null && dtResultBillings.Tables.Count > 0)
                {
                    //strTables = JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);
                    //return Ok(JsonConvert.SerializeObject(dtResultBillings, Newtonsoft.Json.Formatting.Indented));

                    return Ok(dtResultBillings);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetJobTitle: " + ex);
                return InternalServerError();
            }
        }
    }
}