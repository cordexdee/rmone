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
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/RunInBackgroundServices")]
    public class RunInBackgroundServicesController : ApiController
    {

        private ApplicationContext _applicationContext = null;

        public RunInBackgroundServicesController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();

        }


        [HttpGet]
        [Route("GetStatus")]

        public async Task<IHttpActionResult> GetStatus()
        {
            await Task.FromResult(0);
            try
            {
                List<BackgroundServices> BackgroundServices = new List<BackgroundServices>();
                var value = DateTime.Now.Date;
                RunInBackgroundModuleExcelImport.runningImports.Where(x => x.EndDate == DateTime.Today.ToString()).Count();
                if (RunInBackgroundModuleExcelImport.runningImports.Where(x => x.Todays == DateTime.Today.Date).Count() > 0)
                {
                    foreach (var item in RunInBackgroundModuleExcelImport.runningImports)
                    {
                        var backgroundServices = new BackgroundServices();

                        if (item.moduleName == "PMM")
                            backgroundServices.Service = "Import PMM Projects ";
                        else
                            backgroundServices.Service = String.Format("Import {0} Tickets ", item.moduleName); ;

                        backgroundServices.Status = item.status;
                        backgroundServices.User = item.userName;
                        backgroundServices.StartDate = item.startDate;
                        backgroundServices.EndDate = item.EndDate;
                        BackgroundServices.Add(backgroundServices);
                    }
                }

                //phrases = _phraseManager.Load();
                if (BackgroundServices != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(BackgroundServices);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStatus: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetArchiveBackgroundProcessStatus")]

        public async Task<IHttpActionResult> GetArchiveBackgroundProcessStatus()
        {
            await Task.FromResult(0);
            try
            {
                BackgroundProcessStatusManager BackgroundProcessStatusManager = new BackgroundProcessStatusManager(_applicationContext);
                var BackgroundProcessStatusList = BackgroundProcessStatusManager.Load();
                // Up to 30 days (previous days from today) show history 
                var uptodate = DateTime.Now.AddDays(-30);

                if (BackgroundProcessStatusList.Count > 0)
                    BackgroundProcessStatusList = BackgroundProcessStatusList.Where(x => x.EndDate > DateTime.Now.AddDays(-30)).ToList();

                List<BackgroundServices> BackgroundServices = new List<BackgroundServices>();

                if (BackgroundProcessStatusList.Count > 0)
                {
                    foreach (var item in BackgroundProcessStatusList)
                    {
                        var backgroundServices = new BackgroundServices();
                        //backgroundServices.Service = String.Format("Import {0} Tickets ", item.ServiceName);
                        backgroundServices.Service = String.Format("{0} ", item.ServiceName);
                        backgroundServices.Status = item.Status;
                        backgroundServices.User = item.UserName;
                        backgroundServices.StartDate = item.StartDate.ToString();
                        backgroundServices.EndDate = item.EndDate.ToString();
                        BackgroundServices.Add(backgroundServices);
                    }
                }

                //phrases = _phraseManager.Load();
                if (BackgroundServices != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(BackgroundServices);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetArchiveBackgroundProcessStatus: " + ex);
                return InternalServerError();
            }

        }

        public class BackgroundServices
        {
            public string Status { get; set; }
            public string User { get; set; }
            public string Service { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        public class RunInBackgroundModuleExcelImport
        {
            public static List<RunInBackgroundImportStatus> runningImports = new List<RunInBackgroundImportStatus>();

            //public bool RunInBackgroundModuleExcelImport(string moduleName)
            //{
            //    if (string.IsNullOrEmpty(moduleName))
            //        return false;

            //    //return false;
            //    return (runningImports.Count > 0 && runningImports.Exists(x => !string.IsNullOrEmpty(x.moduleName) && x.moduleName == moduleName));
            //}


            //public int ModuleImportPercentageComplete(string moduleName)
            //{
            //    if (string.IsNullOrEmpty(moduleName))
            //        return -1;

            //    if (runningImports.Count > 0 && runningImports.Exists(x => !string.IsNullOrEmpty(x.moduleName) && x.moduleName == moduleName))
            //    {
            //        ImportStatus currentImportStatus = runningImports.FirstOrDefault(x => x.moduleName == moduleName);

            //        if (currentImportStatus.recordsProcessed > 0 && currentImportStatus.totalRecords > 0)
            //        {
            //            if (currentImportStatus.recordsProcessed == currentImportStatus.totalRecords)
            //                return 99;  // final processing happening, so don't show 100%
            //            else
            //                return currentImportStatus.recordsProcessed * 100 / currentImportStatus.totalRecords;
            //        }
            //        else
            //            return 0;
            //    }
            //    else
            //        return -1;

            //}
        }


        public class RunInBackgroundImportStatus
        {
            public string moduleName;
            public string userName;
            public string status;
            public string startDate;
            public string EndDate;
            public bool succeeded; // true = import succeeded, else failed
            public List<string> errorMessages = new List<string>();
            public DateTime Todays;
            public int numErrors;
            public int recordsProcessed;
            public int recordsSkipped;
            public int recordsAdded;
            public int recordsUpdated;
            public int totalRecords;

            public RunInBackgroundImportStatus(string moduleName,string userName,string status,string startDate,string EndDate,DateTime Today)
            {
                this.moduleName = moduleName; 
                this.userName = userName;
                this.status = status;
                this.startDate = startDate;
                this.EndDate = EndDate;
                this.Todays = Today;
            }
        }

    }

}

