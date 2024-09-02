using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ServiceTaskFlowChartHelper
    {
        public ApplicationContext applicationContext = null;
        public ServiceTaskFlowChartHelper(ApplicationContext context)
        {
            applicationContext = context;
        }

        private UGITTaskManager _uGITTaskManager = null;
        private ModuleViewManager _moduleViewManager = null;


        protected UGITTaskManager uGITTaskManager
        {
            get
            {
                if (_uGITTaskManager == null)
                {
                    _uGITTaskManager = new UGITTaskManager(applicationContext);
                }
                return _uGITTaskManager;
                
            }
        }

        protected ModuleViewManager moduleViewManager
        {
            get
            {
                if (_moduleViewManager ==  null)
                {
                    _moduleViewManager = new ModuleViewManager(applicationContext);
                }
                return _moduleViewManager;
                
            }
        }


        public DataTable GetServiceTicketTask(string ModuleName,string TicketId)
        {
            DataTable moduleTaskList = null;
            string PredIds = string.Empty;
            if (!string.IsNullOrEmpty(ModuleName))
            {
                
                moduleTaskList = UGITUtility.ToDataTable<UGITTask>(uGITTaskManager.LoadByProjectID(ModuleName, TicketId));
            }

            for (int rowIndex = 0; rowIndex < moduleTaskList.Rows.Count; rowIndex++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors])))
                {
                    PredIds = Convert.ToString(moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors]);
                    moduleTaskList.Rows[rowIndex][DatabaseObjects.Columns.Predecessors] = UGITUtility.GetPredecessors(moduleTaskList, PredIds);
                }
            }

            

            return moduleTaskList;
        }


        public string getTicketFullUrl(string relatedTicketId,string relatedTitle)
        {
            string onclickUrl =  string.Format("{0}, {1},{2}, {3}, {4},{5})", Ticket.GenerateTicketURL(applicationContext, relatedTicketId), string.Empty, (relatedTicketId + " : " + relatedTitle), "90", "90", "0");
            return onclickUrl;
        }
        
        

    }
 }

