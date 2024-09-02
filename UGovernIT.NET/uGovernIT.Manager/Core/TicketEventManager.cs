using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
//Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
namespace uGovernIT.Manager
{
    public class TicketEventManager : ManagerBase<TicketEvents>, ITicketEventManager
    {
        TicketEventManager tem = null;
        ApplicationContext context = null;
        TicketManager ObjTicketManager = null;
        TicketRelationshipHelper RelationshipHelper = null;
        //TicketEvents TEM = new TicketEvents();

       
        string ticketID;
        string moduleName;
        public TicketEventManager(ApplicationContext spWeb, string moduleName, string ticketID) : base(spWeb)
        {
            context = spWeb;
            tem = new TicketEventManager(context);
            //ObjModuleViewManger = new ModuleViewManager(context);
            //ObjTicketManager = new TicketManager(context);
            //objTaskManager = new UGITTaskManager(context);
            //this.spWeb = spWeb;
            this.ticketID = ticketID;
            this.moduleName = moduleName;
            
        }
        public TicketEventManager(ApplicationContext context) : base(context)
        {
            store = new TicketEventsStore(this.dbContext);
        }
        //public TicketEvents GetTicketEvents(string ticketid,string modulename)
        public TicketEvents GetTicketEvents(string ticketId)
        {

            StoreBase<TicketEvents> moduleImpact = new StoreBase<TicketEvents>(this.dbContext);
            //TicketEvents moduleImpactList = moduleImpact.Load().Where(x => x.Ticketid == ticketid  && x.ModuleName==modulename).FirstOrDefault();
            TicketEvents moduleImpactList = moduleImpact.Load().Where(x => x.Ticketid == ticketId).FirstOrDefault();
            return moduleImpactList;
        }
        public void LogEvent(string ticketEventType, LifeCycleStage stage, bool bySystem = false, string comment = "", string eventReason = "", DateTime? plannedEndDate = null, string affectedUsers = "", UserProfile user = null, string subTaskTitle = "", string subTaskId = "")
        {

            string currentUserID =context.CurrentUser.Id;
            //int currentUserID = UGITUtility.StringToInt(context.CurrentUser.Id);
            Thread thread = new Thread(delegate ()
            {
                
                try
                {
                    LogEventImmediate(context, currentUserID, ticketEventType, stage, bySystem, comment, eventReason, plannedEndDate, affectedUsers, subTaskTitle, subTaskId);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "ERROR creating TicketEvents entry");
                }
               
            });

            thread.IsBackground = true;
            thread.Start();
        }

        // Log event immediately - use this when timing is critical, i.e. event needs to be written before next part execution can happen
        public void LogEventImmediate(ApplicationContext web, string currentUserID, string ticketEventType, LifeCycleStage stage, bool bySystem = false, string comment = "", string eventReason = "", DateTime? plannedEndDate = null, string affectedUsers = null, string subTaskTitle = "", string subTaskId = "")
        {
            TicketEvents TEM = new TicketEvents();//tem.GetTicketEvents(ticketID,moduleName);
            TEM.Ticketid = ticketID;
            if (stage != null)
            {
                TEM.Status = stage.Name;
                TEM.StageStep = stage.StageStep;
            }
            TEM.ModuleName = moduleName;
            TEM.TicketEventType = ticketEventType;
            TEM.EventTime = DateTime.Now;
            TEM.TicketEventBy = currentUserID;
            TEM.Automatic = bySystem;
            TEM.Comment = comment;
            TEM.AffectedUsers = affectedUsers;
            TEM.Created = DateTime.Now;
            if (plannedEndDate != null && plannedEndDate.HasValue && plannedEndDate.Value != DateTime.MaxValue && plannedEndDate.Value != DateTime.MinValue)
                TEM.PlannedEndDate = plannedEndDate.Value;

            TEM.EventReason = eventReason;
            TEM.Title = string.Format("{0} {1}", ticketID, TEM.TicketEventType);

            // assign subtasktitle & subtaskid
            if (!string.IsNullOrEmpty(subTaskTitle))
                TEM.SubTaskTitle = subTaskTitle;

            if (!string.IsNullOrEmpty(subTaskId))
                TEM.SubTaskId = subTaskId;
            TEM.TicketEventBy = context.CurrentUser.UserName.ToString();
            TEM.TenantID = context.TenantID;
            tem.Insert(TEM);

            //return in case of SVC ticket, only need to check SVC child tickets
            if (moduleName.Equals(ModuleNames.SVC))
                return;

            //Get current ticket parent ticket if it is SVC
            string svcTicketID = GetParentSVCTicket(ticketID);
            if (string.IsNullOrWhiteSpace(svcTicketID))
                return; // Only need to do below for SVC child tickets
            TicketEvents relationship = new TicketEvents();

            //SPListItem relationship = ticketEventList.AddItem();
            //SPListItem parentTicket = uHelper.GetTicket(svcTicketID, web);
            DataRow parentTicket = ObjTicketManager.GetTicketTableBasedOnTicketId(moduleName, svcTicketID).Rows[0];
            if (parentTicket != null)
            {
                relationship.Ticketid = svcTicketID;
                relationship.ModuleName = ModuleNames.SVC;
                relationship.Title = string.Format("{0} {1}", svcTicketID, ticketEventType);
                relationship.Status = parentTicket.Field<String>("Status");
                relationship.StageStep = parentTicket.Field<Int32>("StageStep");
            }

            //assign subtasktitle & subtaskid
            //SPListItem subTicketItem = uHelper.GetTicket(ticketID, web);
            DataRow subTicketItem = ObjTicketManager.GetTicketTableBasedOnTicketId(moduleName, ticketID).Rows[0];
            //don't create entry for waiting status, we are already doing it while creating  service task
            if (subTicketItem != null && Convert.ToString(subTicketItem.Field<String>("Status")) == "Waiting")
                return;

            if (subTicketItem != null)
                relationship.SubTaskTitle = subTicketItem.Field<String>("Title");
            relationship.SubTaskId = ticketID;

            relationship.TicketEventType = ticketEventType;
            relationship.EventTime = DateTime.Now;
            relationship.TicketEventBy = currentUserID.ToString();
            relationship.Automatic = bySystem;
            relationship.Comment = comment;
            relationship.AffectedUsers = affectedUsers;

            if (plannedEndDate != null && plannedEndDate.HasValue && plannedEndDate.Value != DateTime.MaxValue && plannedEndDate.Value != DateTime.MinValue)
                relationship.PlannedEndDate = plannedEndDate.Value;
            relationship.EventReason = eventReason;

            //bool allowUnsafeUpdate = web.AllowUnsafeUpdates;
            //web.AllowUnsafeUpdates = true;
            //relationship.Update();
            tem.Insert(relationship);
            //web.AllowUnsafeUpdates = allowUnsafeUpdate;

        }
       
        private string GetParentSVCTicket(string ticketID)
        {
            RelationshipHelper = new TicketRelationshipHelper(context, ticketID);
            List<TicketRelation> ParentList = RelationshipHelper.GetTicketParentList(ticketID);
            DataTable data = ConvertListToDataTable(ParentList);


            string svcTicketID = string.Empty;
            if (data != null)
            {
                DataRow row = data.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ParentTicketId).StartsWith("SVC"));
                if (row != null)
                    svcTicketID = Convert.ToString(row[DatabaseObjects.Columns.ParentTicketId]);
            }

            return svcTicketID;
        }
        public DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public TicketEvents GetTicketEvents()
        {
            throw new NotImplementedException();
        }
    }
    public interface ITicketEventManager : IManagerBase<TicketEvents>
    {
        //TicketEvents GetTicketEvents(string ticketid, string modulename);
        TicketEvents GetTicketEvents();
        //void LogEvent(string ticketEventType, LifeCycleStage stage, bool bySystem = false, string comment = "", string eventReason = "", DateTime? plannedEndDate = null, string affectedUsers = "", UserProfile user = null, string subTaskTitle = "", string subTaskId = "");
        //void LogEventImmediate(ApplicationContext web, int currentUserID, string ticketEventType, LifeCycleStage stage, bool bySystem = false, string comment = "", string eventReason = "", DateTime? plannedEndDate = null, string affectedUsers = null, string subTaskTitle = "", string subTaskId = "");
        //string GetParentSVCTicket(string ticketID);
        //DataTable ConvertListToDataTable<T>(List<T> items);
    }
}
