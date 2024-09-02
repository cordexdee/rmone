using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uGovernIT.Web.Controllers
{
    public class fetchKanbanViewController : Controller
    {
        // GET: fetchKanbanView
        public ActionResult GetKanBanViewHtmlData(string TicketId)
        {
            ViewBag.TicketId = TicketId;
            return PartialView("GetKanBanViewHtmlData", TicketId);
        }
    }
}