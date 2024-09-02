using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class PMMIssueManager : ManagerBase<PMMIssues>, IPMMIssueManager
    {
        public PMMIssueManager(ApplicationContext context) : base(context)
        {
            store = new PMMIssueStore(this.dbContext);
        }

        public DataTable GetAllPMMIssues()
        {
            DataTable dtPMMIssues = null;

            List<PMMIssues> lstPMMIssues = new List<PMMIssues>();
            lstPMMIssues = Load(x => (bool)!x.Deleted && x.PMMIdLookup > 0);

            dtPMMIssues = UGITUtility.ToDataTable<PMMIssues>(lstPMMIssues);
            return dtPMMIssues;
        }

        public DataTable GetOpenPMMIssues()
        {
            DataTable dtPMMIssues = null;

            List<PMMIssues> lstPMMIssues = new List<PMMIssues>();
            lstPMMIssues = Load(x => (bool)!x.Deleted && x.PMMIdLookup > 0 && x.Status != "Completed");

            dtPMMIssues = UGITUtility.ToDataTable<PMMIssues>(lstPMMIssues);
            return dtPMMIssues;
        }

        public DataTable GetCompletedPMMIssues()
        {
            DataTable dtPMMIssues = null;

            List<PMMIssues> lstPMMIssues = new List<PMMIssues>();
            lstPMMIssues = Load(x => (bool)!x.Deleted && x.PMMIdLookup > 0 && x.Status == "Completed");

            dtPMMIssues = UGITUtility.ToDataTable<PMMIssues>(lstPMMIssues);
            return dtPMMIssues;
        }
    }
    public interface IPMMIssueManager : IManagerBase<PMMIssues>
    {

    }
}
