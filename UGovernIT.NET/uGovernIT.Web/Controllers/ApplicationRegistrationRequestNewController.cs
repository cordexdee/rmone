using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Manager;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using uGovernIT.Web.Helpers;
using uGovernIT.Utility.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    public class ApplicationRegistrationRequestNewController : Controller
    {
        private ApplicationContext _applicationContext;
        private TenantManager _tenantManager;
        private UserProfileManager _userProfileManager;
        // GET: ApplicationRegistrationRequestNew

        
        public ApplicationRegistrationRequestNewController()
        {
            _applicationContext = ApplicationContext.Create();
            _tenantManager = new TenantManager(_applicationContext);

            var defaultTenant = _tenantManager.GetTenantList().Where(y => y.AccountID.Equals(ConfigHelper.DefaultTenant, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            _applicationContext.TenantID = Convert.ToString(defaultTenant.TenantID);
            _applicationContext.TenantAccountId = Convert.ToString(defaultTenant.AccountID);
        }


        public ActionResult Index()
        {
            string ID = Convert.ToString(Request.QueryString["tid"]);
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    string data = QueryString.Decode(ID);
                    string[] arr = data.Split('&');
                    ViewBag.TicketID = Convert.ToString(arr[0]);
                    ViewBag.Email = Convert.ToString(arr[1]);
                }
                catch (Exception)
                {
                }
            }
            var serviceManager = new ServicesManager(_applicationContext);

            var services = serviceManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
            ViewBag.ServiceID = serviceManager.LoadAllServices("services").Where(x => x.Title.Equals(ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].ToString())).Select(x => x.ID).FirstOrDefault();

            return View();
        }

        public ActionResult RegistrationStatus(ApplicationRegistrationRequest requestTicketStatus)
        {
            var updateResult = new UpdateResult();
            var serviceInput = new ServiceInput();
            var moduleViewManager = new ModuleViewManager(_applicationContext);
            //var serviceManager = new ServicesManager(_applicationContext);

            if (string.IsNullOrEmpty(requestTicketStatus.EmailID) || string.IsNullOrEmpty(requestTicketStatus.TicketID))
            {
                updateResult.message = "Please enter valid Email Id and Ticket Id";
                return Json(updateResult);
            }

            try
            {
                //List<Services> services = serviceManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
                var moduleName = "SVC";
                var module = moduleViewManager.LoadByName(moduleName);

                //var tableData = GetTableDataManager.GetTableData(module.ModuleTable, DatabaseObjects.Columns.TicketId + " = '" + requestTicketStatus.TicketID.Trim() + "' and " + DatabaseObjects.Columns.Title + " = '" + ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].ToString() + "'");
                var tableData = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TicketId}='{requestTicketStatus.TicketID.Trim()}'");  //Above line commented as it is creating problem when 'Registration of new tenant' sevice has custom mapping in Service & Catalog Agents.

                if (tableData.Rows.Count > 0)
                {
                    var item = tableData.Rows[0];
                    string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.UserQuestionSummary));

                    if (!string.IsNullOrEmpty(questionInputs))
                    {
                        var doc = new XmlDocument();
                        Dictionary<string, string> queAns = new Dictionary<string, string>();

                        doc.LoadXml(questionInputs.Trim());

                        serviceInput = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                        serviceInput.ServiceSections.ForEach(x => x.Questions.ForEach(y => { queAns.Add(y.Token, y.Value); }));

                        if (requestTicketStatus.EmailID.Trim().Equals(queAns["email"], StringComparison.CurrentCultureIgnoreCase))// && requestTicketStatus.TicketID.Equals(queAns["companyname"],StringComparison.CurrentCultureIgnoreCase))
                        {
                            string ticketId = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketId));
                            var taskManager = new UGITTaskManager(_applicationContext);
                            var subtask = taskManager.LoadByProjectID(ticketId);
                            var status = new Dictionary<string, string>();

                            subtask.ForEach(x => { status.Add(Regex.Replace(x.Title, @"\s+", ""), Regex.Replace(x.Status.ToLower(), @"\s+", "_")); });

                            var jsonStatus = Newtonsoft.Json.JsonConvert.SerializeObject(status);

                            updateResult.data = jsonStatus;
                            updateResult.success = true;
                        }
                        else
                        {
                            updateResult.success = false;
                        }
                    }
                }
                else
                {
                    updateResult.success = false;
                }
                updateResult.message = updateResult.success == false ? updateResult.message = "Ticket ID or Email ID is invalid" : "";
            }
            catch (Exception ex)
            {
                updateResult.success = false;
                updateResult.message = "Error to fetch status";
                ULog.WriteException(ex);
            }
            return Json(updateResult);
        }

        //[Route("ApplicationRegistrationRequestNewController/UserVerfication/{str}")]

        public ActionResult UserVerfication(string id, string acc)
        {
            try
            {
                string identify = string.Empty;
                String Enabled = "";
                acc = acc.Replace(" ", "+");
                id = id.Replace(" ", "+");
                string userId = uGovernITCrypto.Decrypt(id, "ugitpass");
                string accountId = uGovernITCrypto.Decrypt(acc, "ugitpass");
                if (!String.IsNullOrEmpty(userId))
                {
                    _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                    UserProfile user = _userProfileManager.LoadById(userId);

                    string Query = $"Select * from AspNetUsers  where Id = '{userId}' and TenantID = '{accountId}';";
                    DataTable tableUser = GetTableDataManager.ExecuteQuery(Query);
                    if (tableUser != null)
                    {
                        Enabled = tableUser.Rows[0]["Enabled"].ToString();
                    }
                    if (Enabled == "False")
                    {
                        Query = $"Update AspNetUsers Set Enabled = 1 where Id = '{userId}' and TenantID = '{accountId}';";
                        DataTable dtlabel = GetTableDataManager.ExecuteQuery(Query);
                        identify = "true";
                        ViewBag.identifyact = "activated";
                        ViewBag.identify = "";
                    }
                    else
                    {
                        ViewBag.identify = "activated";
                        ViewBag.identifyact = "";
                        identify = "false";
                    }
                    //if (user.Enabled == false)
                    //{
                    //    user.Enabled = true;
                    //    var result = _userProfileManager.Update(user);
                    //    if (result.Succeeded == true)
                    //    {
                    //        identify = "true";
                    //        ViewBag.identifyact = "activated";
                    //        ViewBag.identify = "";
                    //    }
                    //}
                    //else
                    //{
                    //    ViewBag.identify = "activated";
                    //    ViewBag.identifyact = "";
                    //    identify = "false";
                    //}
                    //identify = "true";
                    //ViewBag.identifyact = "activated";
                    //ViewBag.identify = "";
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UserVerfication: " + ex);
            }
            return View(ViewBag);
        }

    }
}