using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Web.Models;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/cache")]
    public class CacheController : ApiController
    {

        [HttpGet]
        [Route("GetCacheDetail")]
        public async Task<IHttpActionResult> GetCacheDetail()
        {
            try
            {
                List<CacheStatisticTypeDetail> data = new List<CacheStatisticTypeDetail>();
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                data = RegisterCache.GetCacheDetail(context);
                await Task.FromResult(0);
                return Ok(data);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException($"An Exception Occurred in GetCacheDetail: " + ex);
                return InternalServerError();
            }
        }
    }
}
