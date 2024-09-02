using DevExpress.ExpressApp.Utils;
using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Util.Log;


namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/Mail")]
    public class MailController : ApiController
    {
        private ApplicationContext context = null;
        public MailController()
        {
            context = ApplicationContext.Create(); 
        }

        [HttpPost]
        [Route("SendMail")]

        public async Task<IHttpActionResult> SendMail([FromBody] WebMail obj)
        {
            await Task.FromResult(0);
            try
            {
                string htmlBody = @"<html>
                            <head></head>
                            <body>
                                <p> 
                                                RM1 Customer Details:
                                    <br /><br />
                                    <b>Customer Name:  </b>" + obj.Name + @"
                                    <br />
                                    <b>Customer Email:  </b>" + obj.BusinessEmail + @"
                                    <br />
                                    <b>Company Type:  </b>" + obj.Company_type + @"
                                    <br />
                                    <b>Employee Count:  </b>" + obj.EmployeeCount + @"
                                    <br />
                                    <b>Timecard:  </b>" + obj.Timecard + @"
                                    <br />
                                    <b>Use CRM?:  </b>" + obj.Cms_type + @"
                                    <br />
                                    <b>Crm Type:  </b>" + obj.Crm_type + @"
                                    <br />
                                </p>
                            </body>
                        </html>";

                var mail = new MailMessenger(context);
                string response = string.Empty;
                //var response = mail.SendMail("support@serviceprime.com", "RM1 Customer Details", "smurthy@ugovernit.com", htmlBody, true, new string[] { }, true, false);
                string TO = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RMOneTo"]);
                string CC = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RMOneCC"]);
                string Subject = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RMOneSubject"]);
                if (!string.IsNullOrEmpty(TO) && !string.IsNullOrEmpty(CC) && !string.IsNullOrEmpty(Subject))
                    response = mail.SendMail(TO, Subject, CC, htmlBody, true, new string[] { }, true, false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SendMail: " + ex);
                return InternalServerError();
            }
        }
    }

    public class WebMail
    {
        public string Company_type { get; set; }    
        public string EmployeeCount { get; set; }
        public string Timecard { get; set; }
        public string Cms_type { get; set; }
        public string Crm_type { get; set; }
        public string BusinessEmail { get; set; }
        public string Name { get; set; }

    }
}