using Hangfire;
using System.Threading.Tasks;
namespace uGovernIT.Manager.JobSchedulers
{
    class EmailToTicketScheduler : IJobScheduler
    {
        public EmailToTicketScheduler()
        {

        }

        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            await Ticket.TicketFromMailAsync(context);
        }
    }
}
