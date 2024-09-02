using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ContactManager : ManagerBase<Contact>, IContactManager
    {
     
        public ContactManager(ApplicationContext context):base(context)        {
         
            store = new ContactStore(this.dbContext);
        }
        
        public string GetNameByTicketId(string id)
        {
            TicketManager ticketMGR = new TicketManager(this.dbContext);
            ModuleViewManager moduleMGR = new ModuleViewManager(this.dbContext);
            UGITModule contactModule = moduleMGR.LoadByName(ModuleNames.CON);
            DataTable dtAllTickets = ticketMGR.GetAllTickets(contactModule);
            DataRow[] _contact = dtAllTickets.Select($"TicketId = '{id}'");
            if(_contact != null && _contact.Count() > 0)
            {
                return UGITUtility.ObjectToString(_contact[0][DatabaseObjects.Columns.Title]);
            }
            else
                return string.Empty;
        }

        public string GetCompanyNameByTicketId(string id)
        {
            TicketManager ticketMGR = new TicketManager(this.dbContext);
            ModuleViewManager moduleMGR = new ModuleViewManager(this.dbContext);
            UGITModule companyModule = moduleMGR.LoadByName(ModuleNames.COM);
            DataTable dtAllTickets = ticketMGR.GetAllTickets(companyModule);
            DataRow[] _company = dtAllTickets.Select($"TicketId = '{id}'");
            if (_company != null && _company.Count() > 0)
            {
                return UGITUtility.ObjectToString(_company[0][DatabaseObjects.Columns.Title]);
            }
            else
                return string.Empty;
        }
    }
    public interface IContactManager : IManagerBase<Contact>
    {      
    }
}
