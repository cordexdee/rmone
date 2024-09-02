using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ServiceQuestionMappingManager : ManagerBase<ServiceQuestionMapping>, IServiceQuestionMappingManager
    {
        ModuleViewManager moduleMgr;
        public ServiceQuestionMappingManager(ApplicationContext context) : base(context)
        {
            moduleMgr = new ModuleViewManager(context);

        }
        public List<ServiceQuestionMapping> GetByServiceID(long serviceID)
        {
            List<ServiceQuestionMapping> mappings = new List<ServiceQuestionMapping>();
            mappings = Load(x => x.ServiceID == serviceID);
            
            return mappings;
        }
        //Added by mudassir 7 feb 2020
        public  void DeleteTasks(string moduleName, List<ServiceQuestionMapping> tasks)
        {
            if (tasks.Count <= 0)
            {
                return;
            }
            
            foreach (ServiceQuestionMapping task in tasks)
            {
                
                bool result = Delete(task);
            }
        }
        //
        public DataTable GetTableByServiceID(long serviceID)
        {
            DataTable mappingTable = new DataTable();
            mappingTable.Columns.Add("ID", typeof(int));
            mappingTable.Columns.Add("ServiceID", typeof(int));
            mappingTable.Columns.Add("ServiceName");
            mappingTable.Columns.Add("ServiceQuestionID", typeof(int));
            mappingTable.Columns.Add("ServiceQuestionName");
            mappingTable.Columns.Add("ServiceTaskID", typeof(int));
            mappingTable.Columns.Add("ServiceTaskName");
            mappingTable.Columns.Add("ColumnName");
            mappingTable.Columns.Add("ColumnValue");
            mappingTable.Columns.Add("PickValueFrom");

            List<ServiceQuestionMapping> mappings = GetByServiceID(serviceID);
            foreach (ServiceQuestionMapping map in mappings)
            {
                DataRow row = mappingTable.NewRow();
                row["ID"] = map.ID;
                row["ServiceID"] = map.ServiceID;
                row["ServiceName"] = map.ServiceName;
                row["ServiceQuestionID"] = map.ServiceQuestionID;
                row["ServiceQuestionName"] = map.ServiceQuestionName;
                row["ServiceTaskID"] = map.ServiceTaskID;
                row["ServiceTaskName"] = map.Title;
                row["ColumnName"] = map.ColumnName;
                row["ColumnValue"] = map.ColumnValue;
                row["PickValueFrom"] = map.PickValueFrom;
                mappingTable.Rows.Add(row);
            }
            return mappingTable;
        }

        public List<ServiceQuestionMapping> GetByTaskID(long taskID)
        {
            List<ServiceQuestionMapping> mappings = new List<ServiceQuestionMapping>();

            //SPList defaultValueList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketDefaultValues);
            //SPQuery query = Queries.ByServiceTicketTitleLookup(taskID);
            //SPListItemCollection collection = defaultValueList.GetItems(query);
            //foreach (SPListItem item in collection)
            //{
            //    mappings.Add(LoadObj(item));
            //}
            return mappings;
        }

        public DataTable GetTableByTaskID(long taskID)
        {
            DataTable mappingTable = new DataTable();
            mappingTable.Columns.Add("ID", typeof(int));
            mappingTable.Columns.Add("ServiceID", typeof(int));
            mappingTable.Columns.Add("ServiceName");
            mappingTable.Columns.Add("ServiceQuestionID", typeof(int));
            mappingTable.Columns.Add("ServiceQuestionName");
            mappingTable.Columns.Add("ServiceTaskID", typeof(int));
            mappingTable.Columns.Add("ServiceTaskName");
            mappingTable.Columns.Add("ColumnName");
            mappingTable.Columns.Add("ColumnValue");
            mappingTable.Columns.Add("PickValueFrom");

            List<ServiceQuestionMapping> mappings = GetByTaskID(taskID);
            foreach (ServiceQuestionMapping map in mappings)
            {
                DataRow row = mappingTable.NewRow();
                row["ID"] = map.ID;
                row["ServiceID"] = map.ServiceID;
                row["ServiceName"] = map.ServiceName;
                row["ServiceQuestionID"] = map.ServiceQuestionID;
                row["ServiceQuestionName"] = map.ServiceQuestionName;
                row["ServiceTaskID"] = map.ServiceTaskID;
                row["ServiceTaskName"] = map.Title;
                row["ColumnName"] = map.ColumnName;
                row["ColumnValue"] = map.ColumnValue;
                row["PickValueFrom"] = map.PickValueFrom;
                mappingTable.Rows.Add(row);
            }
            return mappingTable;
        }

        public List<ServiceQuestionMapping> GetByQuestionID(long questionID)
        {
            List<ServiceQuestionMapping> mappings = new List<ServiceQuestionMapping>();

            //SPList defaultValueList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceTicketDefaultValues);
            //SPQuery query = Queries.ByServiceQuestionLookup(questionID);
            //SPListItemCollection collection = defaultValueList.GetItems(query);
            //foreach (SPListItem item in collection)
            //{
            //    mappings.Add(LoadObj(item));
            //}
            return mappings;
        }

        public DataTable GetTableByQuestionID(long questionID)
        {
            DataTable mappingTable = new DataTable();
            mappingTable.Columns.Add("ID", typeof(int));
            mappingTable.Columns.Add("ServiceID", typeof(int));
            mappingTable.Columns.Add("ServiceName");
            mappingTable.Columns.Add("ServiceQuestionID", typeof(int));
            mappingTable.Columns.Add("ServiceQuestionName");
            mappingTable.Columns.Add("ServiceTaskID", typeof(int));
            mappingTable.Columns.Add("ServiceTaskName");
            mappingTable.Columns.Add("ColumnName");
            mappingTable.Columns.Add("ColumnValue");
            mappingTable.Columns.Add("PickValueFrom");

            List<ServiceQuestionMapping> mappings = GetByQuestionID(questionID);
            foreach (ServiceQuestionMapping map in mappings)
            {
                DataRow row = mappingTable.NewRow();
                row["ID"] = map.ID;
                row["ServiceID"] = map.ServiceID;
                row["ServiceName"] = map.ServiceName;
                row["ServiceQuestionID"] = map.ServiceQuestionID;
                row["ServiceQuestionName"] = map.ServiceQuestionName;
                row["ServiceTaskID"] = map.ServiceTaskID;
                row["ServiceTaskName"] = map.Title;
                row["ColumnName"] = map.ColumnName;
                row["ColumnValue"] = map.ColumnValue;
                row["PickValueFrom"] = map.PickValueFrom;
                mappingTable.Rows.Add(row);
            }
            return mappingTable;
        }

        public ServiceQuestionMapping LoadObj(ServiceQuestionMapping item)
        {
            ServiceQuestionMapping questionMap = new ServiceQuestionMapping();

            //if (uHelper.IsSPItemExist(item, DatabaseObjects.Columns.ServiceTitleLookup))
            //{
            //    SPFieldLookupValue sectionLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ServiceTitleLookup)));
            //    if (sectionLookup.LookupId > 0)
            //    {
            //        questionMap.ServiceID = sectionLookup.LookupId;
            //        questionMap.ServiceName = sectionLookup.LookupValue;
            //    }
            //}
            //if (uHelper.IsSPItemExist(item, DatabaseObjects.Columns.ServiceTicketTitleLookup))
            //{
            //    SPFieldLookupValue sectionLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ServiceTicketTitleLookup)));
            //    if (sectionLookup.LookupId > 0)
            //    {
            //        questionMap.ServiceTaskID = sectionLookup.LookupId;
            //        questionMap.ServiceTaskName = sectionLookup.LookupValue;
            //    }
            //}
            //if (uHelper.IsSPItemExist(item, DatabaseObjects.Columns.ServiceQuestionTitleLookup))
            //{
            //    SPFieldLookupValue sectionLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ServiceQuestionTitleLookup)));
            //    if (sectionLookup.LookupId > 0)
            //    {
            //        questionMap.ServiceQuestionID = sectionLookup.LookupId;
            //        questionMap.ServiceQuestionName = sectionLookup.LookupValue;
            //    }
            //}
            //questionMap.ColumnName = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ColumnName));
            //questionMap.ColumnValue = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ColumnValue));
            //questionMap.PickValueFrom = Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.PickValueFrom));
            //questionMap.ID = item.ID;

            return questionMap;
        }

        public DataTable GetFieldsRequiredToCreateTickets(List<UGITTask> tickets)
        {
            DataTable requiredFields = new DataTable();
            requiredFields.Columns.Add("ID", typeof(int));
            requiredFields.Columns.Add("ModuleName");
            requiredFields.Columns.Add("ModuleID", typeof(int));
            requiredFields.Columns.Add("FieldName");
            requiredFields.Columns.Add("FieldDisplayName");
            requiredFields.Columns.Add("CustomProperties");
            requiredFields.Columns.Add("ServiceTicketTitle");
            requiredFields.Columns.Add("ServiceTicketID", typeof(int));
            requiredFields.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            requiredFields.Columns.Add(DatabaseObjects.Columns.FieldMandatory, typeof(bool));

            foreach (UGITTask ticket in tickets)
            {
                if (!string.IsNullOrEmpty(ticket.RelatedModule)) //For ticket fields
                {
                    UGITModule module = moduleMgr.LoadByName(ticket.RelatedModule);
                    DataRow[] fields = UGITUtility.ToDataTable<ModuleRoleWriteAccess>(module.List_RoleWriteAccess).Select(string.Format("({0}='0' OR {0}='1') AND ({1}='False' OR {1} is null)",
                                                                                                                                            DatabaseObjects.Columns.StageStep, DatabaseObjects.Columns.HideInServiceMapping));
                    DataRow[] layoutFields = UGITUtility.ToDataTable<ModuleFormLayout>(module.List_FormLayout).Select();

                    foreach (DataRow field in fields)
                    {
                        DataRow row = requiredFields.NewRow();
                        row["ID"] = row[DatabaseObjects.Columns.Id];
                        row["ModuleName"] = ticket.RelatedModule;
                        row["FieldName"] = field[DatabaseObjects.Columns.FieldName];
                        row[DatabaseObjects.Columns.FieldMandatory] = UGITUtility.StringToBoolean(field[DatabaseObjects.Columns.FieldMandatory]);
                        row["CustomProperties"] = field[DatabaseObjects.Columns.CustomProperties];
                        row["ServiceTicketTitle"] = ticket.Title;
                        row["ServiceTicketID"] = ticket.ID;

                        DataRow layoutField = layoutFields.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == Convert.ToString(field[DatabaseObjects.Columns.FieldName]));
                        if (layoutField != null)
                        {
                            int tabID = UGITUtility.StringToInt(layoutField[DatabaseObjects.Columns.TabId]);
                            int order = UGITUtility.StringToInt(layoutField[DatabaseObjects.Columns.FieldSequence]);
                            order = order == 0 ? order = 1000 : (tabID * 100 + order); // Sort by tab ID and then by sequence
                            row[DatabaseObjects.Columns.ItemOrder] = order;
                            row[DatabaseObjects.Columns.FieldDisplayName] = layoutField[DatabaseObjects.Columns.FieldDisplayName];
                        }
                        else
                        {
                            row[DatabaseObjects.Columns.ItemOrder] = 1000;
                            row[DatabaseObjects.Columns.FieldDisplayName] = field[DatabaseObjects.Columns.FieldName];
                        }

                        requiredFields.Rows.Add(row);
                    }
                }
                else //For Task fields
                {
                    DataRow row = requiredFields.NewRow();

                    if (ticket.SubTaskType == ServiceSubTaskType.AccountTask)
                    {
                        row["ID"] = 0;
                        row["ModuleName"] = "Task";
                        row["ModuleID"] = 0;
                        row["FieldName"] = DatabaseObjects.Columns.UGITNewUserName;
                        row[DatabaseObjects.Columns.FieldDisplayName] = "New User Name";
                        row["CustomProperties"] = string.Empty;
                        row["ServiceTicketTitle"] = ticket.Title;
                        row["ServiceTicketID"] = ticket.ID;
                        row[DatabaseObjects.Columns.ItemOrder] = 1;
                        requiredFields.Rows.Add(row);
                    }

                    row = requiredFields.NewRow();
                    row["ID"] = 0;
                    row["ModuleName"] = "Task";
                    row["ModuleID"] = 0;
                    row["FieldName"] = DatabaseObjects.Columns.Title;
                    row[DatabaseObjects.Columns.FieldDisplayName] = DatabaseObjects.Columns.Title;
                    row["CustomProperties"] = string.Empty;
                    row["ServiceTicketTitle"] = ticket.Title;
                    row["ServiceTicketID"] = ticket.ID;
                    row[DatabaseObjects.Columns.ItemOrder] = 1;
                    requiredFields.Rows.Add(row);

                    row = requiredFields.NewRow();
                    row["ID"] = 0;
                    row["ModuleName"] = "Task";
                    row["ModuleID"] = 0;
                    row["FieldName"] = DatabaseObjects.Columns.AssignedTo;
                    row[DatabaseObjects.Columns.FieldDisplayName] = "Assigned To";
                    row["CustomProperties"] = string.Empty;
                    row["ServiceTicketTitle"] = ticket.Title;
                    row["ServiceTicketID"] = ticket.ID;
                    row[DatabaseObjects.Columns.ItemOrder] = 2;
                    requiredFields.Rows.Add(row);

                    row = requiredFields.NewRow();
                    row["ID"] = 0;
                    row["ModuleName"] = "Task";
                    row["ModuleID"] = 0;
                    row["FieldName"] = DatabaseObjects.Columns.Description;
                    row[DatabaseObjects.Columns.FieldDisplayName] = DatabaseObjects.Columns.Description;
                    row["CustomProperties"] = string.Empty;
                    row["ServiceTicketTitle"] = ticket.Title;
                    row["ServiceTicketID"] = ticket.ID;
                    row[DatabaseObjects.Columns.ItemOrder] = 3;
                    requiredFields.Rows.Add(row);

                    row = requiredFields.NewRow();
                    row["ID"] = 0;
                    row["ModuleName"] = "Task";
                    row["ModuleID"] = 0;
                    row["FieldName"] = DatabaseObjects.Columns.DueDate;
                    row[DatabaseObjects.Columns.FieldDisplayName] = "Due Date";
                    row["CustomProperties"] = string.Empty;
                    row["ServiceTicketTitle"] = ticket.Title;
                    row["ServiceTicketID"] = ticket.ID;
                    row[DatabaseObjects.Columns.ItemOrder] = 4;
                    requiredFields.Rows.Add(row);

                    if (ticket.EnableApproval)
                    {
                        row = requiredFields.NewRow();
                        row["ID"] = 0;
                        row["ModuleName"] = "Task";
                        row["ModuleID"] = 0;
                        row["FieldName"] = DatabaseObjects.Columns.Approver;
                        row[DatabaseObjects.Columns.FieldDisplayName] = DatabaseObjects.Columns.Approver;
                        row["CustomProperties"] = string.Empty;
                        row["ServiceTicketTitle"] = ticket.Title;
                        row["ServiceTicketID"] = ticket.ID;
                        row[DatabaseObjects.Columns.ItemOrder] = 5;
                        requiredFields.Rows.Add(row);
                    }
                }
            }

            if (requiredFields.Rows.Count > 0)
            {
                DataView dataView = requiredFields.DefaultView;
                dataView.Sort = DatabaseObjects.Columns.FieldDisplayName + " ASC";
                requiredFields = dataView.ToTable();
            }

            requiredFields = requiredFields.DefaultView.ToTable(true);
            return requiredFields;
        }

        public DataTable GetFieldsRequiredToCreateAgents(UGITModule module)
        {
            DataTable requiredFields = new DataTable();
            requiredFields.Columns.Add("ID", typeof(int));
            requiredFields.Columns.Add("ModuleName");
            requiredFields.Columns.Add("ModuleID", typeof(int));
            requiredFields.Columns.Add("FieldName");
            requiredFields.Columns.Add("FieldDisplayName");
            requiredFields.Columns.Add("CustomProperties");
            requiredFields.Columns.Add("ServiceTicketID");
            requiredFields.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            DataRow[] fields = UGITUtility.ToDataTable<ModuleFormLayout>(module.List_FormLayout).Select(string.Format("{1} <> '0' And {0} = 'False'", DatabaseObjects.Columns.HideInTicketTemplate, DatabaseObjects.Columns.TabId));
            //DataRow[] fields = uGITCache.GetDataTable(DatabaseObjects.Lists.FormLayout, string.Format("{0}='{1}' And ({2} is null Or {2} <> '1')", DatabaseObjects.Columns.ModuleNameLookup, module.ModuleName, DatabaseObjects.Columns.HideInTicketTemplate));
            foreach (DataRow field in fields)
            {
                DataRow row = requiredFields.NewRow();
                row["ID"] = row[DatabaseObjects.Columns.Id];
                row["ModuleName"] = module.ModuleName;
                row["ModuleID"] = module.ModuleId;
                row["FieldName"] = field[DatabaseObjects.Columns.FieldName];
                row["FieldDisplayName"] = field[DatabaseObjects.Columns.FieldDisplayName];
                row["CustomProperties"] = field[DatabaseObjects.Columns.CustomProperties];
                row["ServiceTicketID"] = 0;
                int order = 0;
                if (!int.TryParse(Convert.ToString(field[DatabaseObjects.Columns.FieldSequence]), out order))
                    order = 1000;
                row[DatabaseObjects.Columns.ItemOrder] = order;
                requiredFields.Rows.Add(row);
            }

            requiredFields = requiredFields.Select(string.Format("{0} NOT IN ('#GroupEnd#','#GroupStart#','#Control#','#TableStart#','#TableEnd#','#Label#')", DatabaseObjects.Columns.FieldName)).CopyToDataTable();
            if (requiredFields.Rows.Count > 0)
            {
                DataView dataView = requiredFields.DefaultView;
                dataView.Sort = DatabaseObjects.Columns.FieldDisplayName + " ASC";
                requiredFields = dataView.ToTable();
            }
            requiredFields = requiredFields.DefaultView.ToTable(true, "ID", "ModuleName", "ModuleID", "FieldName", "FieldDisplayName", "CustomProperties", "ServiceTicketID");
            return requiredFields;
        }

        public void Save(ServiceQuestionMapping item)
        {
            if (item.ID <= 0)
            {
                long i = Insert(item);
            }
            else
            {
                bool result = Update(item);
            }
        }

        public void Save(List<ServiceQuestionMapping> questionFieldMappings)
        {

        }

        
        
        public void SetDefaultId()
        {
            //ID = 0;
        }
    }
    interface IServiceQuestionMappingManager : IManagerBase<ServiceQuestionMapping>
    {

    }
}
