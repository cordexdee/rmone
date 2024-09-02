using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ProjectAllocationManager : ManagerBase<ProjectAllocation>, IProjectAllocationManager
    {
      
        DataTable allocationList;
        public ProjectAllocationManager(ApplicationContext context) : base(context)
        {
            allocationList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectAllocations, $"TenantID='{context.TenantID}'");
        }

        public DataRow[] GetItems(string ticketID, string TenantID,string userID = null)
        {
            string query;
            if (!string.IsNullOrEmpty(userID))
            {
                query = string.Format(" {0} = '{1}' and {2} = '{3}' ",
                           DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.Resource, userID);

            }
            else
            {
                query = string.Format("{0} = '{1}'",
                            DatabaseObjects.Columns.TicketId, ticketID);
            }

            DataRow[] getProjectAllocation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectAllocations,$"{DatabaseObjects.Columns.TenantID} = '{TenantID}'").Select(query);
            return getProjectAllocation;
        }


        public List<ProjectAllocation> GetItemList(string ticketID, string userID = null)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return this.Load(x => x.TicketID == ticketID).ToList();

            return this.Load(x => x.TicketID == ticketID && x.Resource == userID).ToList();
        }

        public ProjectAllocation Save(ProjectAllocation projectAllocation)
        {
            if (projectAllocation.ID > 0)
                this.Update(projectAllocation);
            else
                this.Insert(projectAllocation);
            return projectAllocation;
        }
    }

    public interface IProjectAllocationManager : IManagerBase<ProjectAllocation>
    {

    }


}
