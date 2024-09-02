using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using DevExpress.CodeParser;
using System.Xml;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Manager
{
    public class TicketRelationshipHelper
    {
        public XmlDocument ChildTicketIds { get; private set; }
        public string CurrentTicketId { get; set; }
        public XmlDocument ParentTicketIDs { get; private set; }
        TicketRelationManager tm = null;
        ApplicationContext context = null;
        ModuleViewManager ObjModuleViewManger = null;
        TicketManager ObjTicketManager = null;
        UGITTaskManager objTaskManager = null;
        ModuleColumnManager objmodulecolumnmanager;


        public TicketRelationshipHelper(ApplicationContext _context)
        {
            context = _context;
            tm = new TicketRelationManager(context);
            ObjModuleViewManger = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
            objTaskManager = new UGITTaskManager(context);
            objmodulecolumnmanager = new ModuleColumnManager(context);
        }
        public TicketRelationshipHelper(ApplicationContext _context, string currentTicketId, string childTicketId)
        {
            context = _context;
            tm = new TicketRelationManager(context);
            ObjModuleViewManger = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
            CurrentTicketId = currentTicketId;
            ChildTicketIds = new XmlDocument();
            System.Xml.XmlElement parents = ChildTicketIds.CreateElement("Children");
            parents.SetAttribute(DatabaseObjects.Columns.CurrentTicketId, currentTicketId);
            ChildTicketIds.AppendChild(parents);
            System.Xml.XmlElement element = ChildTicketIds.CreateElement("Child");
            element.SetAttribute(DatabaseObjects.Columns.ChildTicketId, childTicketId);
            parents.AppendChild(element);
            objmodulecolumnmanager = new ModuleColumnManager(context);
        }
        public TicketRelationshipHelper(ApplicationContext _context, string currentTicketId)
        {
            context = _context;
            tm = new TicketRelationManager(context);
            ObjModuleViewManger = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
            CurrentTicketId = currentTicketId;
            objmodulecolumnmanager = new ModuleColumnManager(context);
        }
        public DataTable GetTicketRelationshipDT()
        {
            //return tm.GetDataTable();
            return GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelation, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
        }
        public List<TicketRelation> GetTicketList()
        {
            List<TicketRelation> ticketRelationship = tm.Load();
            return ticketRelationship;
        }
        public List<TicketRelation> GetTicketChildList(string ticketID)
        {
            List<TicketRelation> tRelation = GetTicketList().Where(x => x.ParentTicketID == ticketID).ToList();
            return tRelation;
        }
        public List<TicketRelation> GetTicketParentList(string ticketID)
        {
            List<TicketRelation> tRelation = GetTicketList().Where(x => x.ChildTicketID == ticketID).ToList();
            return tRelation;
        }

        public List<TicketRelation> GetOpenChildTicket(string ticketID)
        {
            List<TicketRelation> tRelation = GetTicketList().Where(x => x.ParentTicketID == ticketID).ToList();
            List<TicketRelation> openTicket = new List<TicketRelation>();
            foreach (TicketRelation relatedTicket in tRelation)
            {
                DataRow dr = Ticket.GetCurrentTicket(context, uHelper.getModuleIdByTicketID(context, relatedTicket.ChildTicketID), relatedTicket.ChildTicketID);

                string moduleName = relatedTicket.ChildTicketID.Split('-')[0];
                Ticket objTicket = new Ticket(context, moduleName);
                if (objTicket.GetTicketCurrentStage(dr).StageTypeChoice != StageType.Closed.ToString())
                    openTicket.Add(relatedTicket);
            }
            return openTicket;
        }
        public DataTable GetTicketRelationshipByID(string ticketID)
        {
            List<TicketRelation> ticketRelationship = tm.Load(" where " + DatabaseObjects.Columns.ParentTicketId + "='" + ticketID + "' OR " + DatabaseObjects.Columns.ChildTicketId + "='" + ticketID + "'"); //uGITDAL.getTicketRelationship().ToList();
            DataTable dtTicketRelationship = UGITUtility.ToDataTable<TicketRelation>(ticketRelationship);
            return dtTicketRelationship;
        }
        public bool IsChildExist(string ticketId, DataTable relationshipTickets)
        {
            bool exist = false;
            if (relationshipTickets != null && relationshipTickets.Rows.Count > 0)
            {
                DataRow[] drTickets = relationshipTickets.Select(string.Format("{0} ='{1}' and ChildTicketId NOT like 'WIKI%'", DatabaseObjects.Columns.ParentTicketId, ticketId));
                if (drTickets != null && drTickets.Length > 0)
                {
                    exist = true;
                }
            }
            return exist;
        }
        public  bool IsRelationExist(string ticketId, DataTable relationshipTickets)
        {
            if (relationshipTickets != null && relationshipTickets.Rows.Count > 0)
            {
                DataRow[] drTickets = relationshipTickets.Select(string.Format("{0} ='{1}'", DatabaseObjects.Columns.ParentTicketId, ticketId));
                if (drTickets != null && drTickets.Length > 0)
                    return true;

                drTickets = relationshipTickets.Select(string.Format("{0} ='{1}'", DatabaseObjects.Columns.ChildTicketId, ticketId));
                if (drTickets != null && drTickets.Length > 0)
                    return true;
            }

            return false;
        }
        public TicketRelationshipHelper Load(string currentTicketId, int parentUpLevel, int childDownLevel, bool disableParentDetail, bool disableChildDetail)
        {
            TicketRelationshipHelper relationships = new TicketRelationshipHelper(context, currentTicketId);
            DataTable ticketRList = GetTicketRelationshipDT();
            DataRow[] rCollection = ticketRList.Select();
            if (rCollection != null && rCollection.Count() > 0)
            {
                DataTable rTable = rCollection.CopyToDataTable();
                DataRow[] dr = rTable.Select("ChildTicketId NOT like 'WIKI%'");
                rTable = null;
                if (dr != null && dr.Length > 0)
                {

                    rTable = dr.CopyToDataTable();
                    relationships = new TicketRelationshipHelper(context, currentTicketId);

                    relationships.ParentTicketIDs = GetPrarent(currentTicketId, rTable, parentUpLevel, disableParentDetail);
                    relationships.ChildTicketIds = GetChildren(currentTicketId, rTable, childDownLevel, disableChildDetail);
                }
            }
            return relationships;


        }

        public int DeleteRelation(string parentTicketId, string childTicketId)
        {
            int rowEffected = 0;
            if (childTicketId != null && childTicketId.Trim() != string.Empty && parentTicketId != null && parentTicketId.Trim() != string.Empty)
            {
                TicketRelation relation = tm.GetTicketRelationShip(childTicketId, parentTicketId);
                tm.Delete(relation);
                return 1;
            }
            return rowEffected;
        }
        public DataTable GetTicketDetail(string ticketId)
        {
            DataTable ticketDetail = null;
            if (ticketId != null)
            {
                string module = ticketId.Split(new char[] { '-' })[0];
                //DataTable moduleList =GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules,DatabaseObjects.Columns.ModuleName+"='"+module+"'");
                DataTable moduleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ModuleName}='{module}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                DataView view = new DataView(moduleList);
                moduleList = view.ToTable(false, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.ModuleTicketTable, DatabaseObjects.Columns.ModuleRelativePagePath, DatabaseObjects.Columns.StaticModulePagePath });

                //SPQuery mQuery = new SPQuery();
                //mQuery.Query = string.Format("<Where><Eq><FieldRef Name='{1}' /><Value Type='Text'>{0}</Value></Eq></Where>", module.Trim(), DatabaseObjects.Columns.ModuleName);
                //mQuery.ViewFields = string.Format("<FieldRef Name='{0}' /><FieldRef Name='{1}' /><FieldRef Name='{2}' />", DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.ModuleTicketTable, DatabaseObjects.Columns.ModuleRelativePagePath);
                //mQuery.ViewFieldsOnly = true;

                DataRow[] mCollection = moduleList.Select();
                if (mCollection != null && mCollection.Count() > 0)
                {
                    DataRow item = mCollection[0];

                    string moduleTicketTable = Convert.ToString(mCollection[0][DatabaseObjects.Columns.ModuleTicketTable]); //mCollection[0][moduleList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.ModuleTicketTable).ToString()].ToString();
                    DataTable moduleTicketList = null;
                    //moduleTicketList =GetTableDataManager.GetTableData(moduleTicketTable);
                    moduleTicketList = GetTableDataManager.GetTableData(moduleTicketTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.TicketId}='{ticketId}'");

                    //SPQuery tDetailQuery = new SPQuery();
                    //tDetailQuery.Query = string.Format("<Where><Eq><FieldRef Name='{1}' /><Value Type='Text'>{0}</Value></Eq></Where>", ticketId.Trim(), DatabaseObjects.Columns.TicketId);
                    //tDetailQuery.ViewFields = string.Format("<FieldRef Name='{0}' /><FieldRef Name='{1}' /><FieldRef Name='{2}' /><FieldRef Name='{3}' /><FieldRef Name='{4}' /><FieldRef Name='{5}' />",
                    //                                        DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.TicketPriorityLookup, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketCreationDate, DatabaseObjects.Columns.Title);
                    //tDetailQuery.ViewFieldsOnly = true;
                    string[] columnNames = moduleTicketList.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                    DataView dview = new DataView(moduleTicketList);
                    if (ticketId.Split('-')[0].ToUpper() == ModuleNames.CMDB)
                        moduleTicketList = dview.ToTable(false, new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.Title });
                    else if (ticketId.Split('-')[0].ToUpper() == ModuleNames.WIKI || ticketId.Split('-')[0].ToUpper() == ModuleNames.CMT || !dview.Table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                    {
                        moduleTicketList = dview.ToTable(false, new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketCreatedDate });
                    }
                    else
                        moduleTicketList = dview.ToTable(false, new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.TicketPriorityLookup, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketCreationDate, DatabaseObjects.Columns.Title });


                    DataRow[] tDetailCollection = moduleTicketList.Select();
                    string relativePath = Convert.ToString(mCollection[0][DatabaseObjects.Columns.StaticModulePagePath]);
                    string moduleName = Convert.ToString(mCollection[0][DatabaseObjects.Columns.ModuleName]);
                    if (tDetailCollection != null && tDetailCollection.Count() > 0)
                    {
                        ticketDetail = tDetailCollection.CopyToDataTable();
                        ticketDetail.Columns.Add(DatabaseObjects.Columns.StaticModulePagePath);
                        ticketDetail.Columns.Add(DatabaseObjects.Columns.ModuleName);

                        ticketDetail.Columns[DatabaseObjects.Columns.StaticModulePagePath].Expression = "'" + relativePath + "'";
                        ticketDetail.Columns[DatabaseObjects.Columns.ModuleName].Expression = "'" + moduleName + "'";
                    }
                }
            }
            return ticketDetail;
        }
        public int CreateRelation(ApplicationContext _context)
        {
            int rowEffected = 0;
            System.Xml.XmlNode node = ChildTicketIds.DocumentElement.ChildNodes[0];
            if (node != null && node.Attributes[DatabaseObjects.Columns.ChildTicketId] != null)
            {
                string childTicket = node.Attributes[DatabaseObjects.Columns.ChildTicketId].Value.Trim();
                string module = childTicket.Split(new char[] { '-' })[0];
                if (tm.Get(x => x.ChildTicketID == childTicket && x.ParentTicketID == this.CurrentTicketId) != null)
                    return 0;
                TicketRelation relation = new TicketRelation();
                relation.ParentTicketID = this.CurrentTicketId;
                relation.ChildTicketID = childTicket;
                relation.ChildModuleName = uHelper.getModuleNameByTicketId(childTicket);
                relation.ParentModuleName = uHelper.getModuleNameByTicketId(this.CurrentTicketId);
                tm.Insert(relation);
                if (relation.ID > 0)
                    rowEffected = 1;

                if (relation.ParentModuleName == ModuleNames.SVC)
                {
                    //update entry in moduletasks, so that entry will be available in services tickets
                    UGITTaskManager taskManager = new UGITTaskManager(_context);
                    DataRow splistitem = Ticket.GetCurrentTicket(_context, relation.ChildModuleName, childTicket);
                    int itemCount = taskManager.LoadByProjectID(ModuleNames.SVC, this.CurrentTicketId).Max(x => x.ItemOrder);

                    UGITTask moduleInstance = new UGITTask();
                    moduleInstance.ParentInstance = this.CurrentTicketId;
                    moduleInstance.ChildInstance = childTicket;
                    moduleInstance.Level = 0;
                    moduleInstance.ParentTaskID = 0;
                    moduleInstance.RelatedModule = uHelper.getModuleNameByTicketId(childTicket);
                    moduleInstance.RelatedTicketID = childTicket;
                    moduleInstance.DueDate = new DateTime(1800, 1, 1);
                    moduleInstance.StartDate = new DateTime(1800, 1, 1);
                    moduleInstance.TicketId = this.CurrentTicketId;
                    moduleInstance.ItemOrder = itemCount + 1;
                    moduleInstance.ModuleNameLookup = ModuleNames.SVC;
                    moduleInstance.Behaviour = "Ticket";
                    moduleInstance.Status = Convert.ToString(splistitem[DatabaseObjects.Columns.TicketStatus]);
                    moduleInstance.Title = Convert.ToString(splistitem[DatabaseObjects.Columns.Title]);

                    taskManager.Save(moduleInstance);
                }
            }
            return rowEffected;
        }
        public bool IsRelationExist(string parentTicketId, string childTicketId)
        {
            bool exist = false;
            DataTable ticketRList = GetTicketRelationshipDT();
            string Query;
            string moduleName = uHelper.getModuleNameByTicketId(parentTicketId);
            if (moduleName == "CMDB")
                Query = string.Format("({0}='{1}' and {2}='{3}') or({0}='{3}' and {2}='{1}')", DatabaseObjects.Columns.ChildTicketId, childTicketId, DatabaseObjects.Columns.ParentTicketId, parentTicketId);
            else
                Query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ChildTicketId, childTicketId, DatabaseObjects.Columns.ParentTicketId, parentTicketId);

            DataRow[] rCollection = ticketRList.Select(Query);
            if (rCollection != null && rCollection.Count() > 0)
                exist = true;
            return exist;
        }
        public bool IsPrarentExist(string ticketId)
        {
            bool exist = false;
            DataTable ticketRList = GetTicketRelationshipDT();
            string rQuery = string.Format("{0}={1}", DatabaseObjects.Columns.ChildTicketId, ticketId);
            DataRow[] rCollection = ticketRList.Select(rQuery);
            if (rCollection != null && rCollection.Count() > 0)
            {
                exist = true;
            }
            return exist;
        }
        //public static bool IsChildExist(string ticketId, DataTable relationshipTickets)
        //{
        //    bool exist = false;

        //    if (relationshipTickets != null && relationshipTickets.Rows.Count > 0)
        //    {
        //        DataRow[] drTickets = relationshipTickets.Select(string.Format("{0} ='{1}' and ChildTicketId NOT like 'WIK%'", DatabaseObjects.Columns.ParentTicketId, ticketId));
        //        if (drTickets != null && drTickets.Length > 0)
        //        {
        //            exist = true;
        //        }
        //    }
        //    return exist;
        //}

        public DataTable GetIndependentTickets(string moduleName, string ticketId)
        {

            DataTable resultedTable = new DataTable("Tickets");
            resultedTable.Columns.Add(DatabaseObjects.Columns.TicketId);
            resultedTable.Columns.Add(DatabaseObjects.Columns.Id);
            resultedTable.Columns.Add(DatabaseObjects.Columns.TicketDescription);
            resultedTable.Columns.Add(DatabaseObjects.Columns.Title);

            if (!string.IsNullOrEmpty(moduleName))
            {
                TicketRelationshipHelper ticketRelation = null;
                List<string> parents = new List<string>();
                if (ticketId != null && ticketId.Trim() != string.Empty)
                {
                    ticketRelation = Load(ticketId.Trim(), -1, 0, true, true);
                    if (ticketRelation.ParentTicketIDs != null)
                    {
                        XmlNodeList nodeList = ticketRelation.ParentTicketIDs.DocumentElement.SelectNodes("//Parent[@ParentTicketId]");
                        foreach (System.Xml.XmlNode node in nodeList)
                        {
                            parents.Add(node.Attributes["ParentTicketId"].Value);
                        }
                    }
                }

                DataTable dtmoduleTicketList = ObjTicketManager.GetAllTickets(ObjModuleViewManger.LoadByName(moduleName));
                DataTable dtTicketRList = GetTicketRelationshipDT();
                if (dtmoduleTicketList != null)
                {
                    if (dtTicketRList == null)
                    {
                        var resultedTickets1 = from m in dtmoduleTicketList.AsEnumerable()
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) != ticketId.Trim()
                                               && !(from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select new { m };

                        foreach (var row in resultedTickets1)
                        {
                            DataRow tRow = resultedTable.NewRow();
                            tRow[DatabaseObjects.Columns.Id] = row.m[DatabaseObjects.Columns.Id];
                            tRow[DatabaseObjects.Columns.TicketId] = row.m[DatabaseObjects.Columns.TicketId];
                            //tRow[DatabaseObjects.Columns.TicketDescription] = row.m[DatabaseObjects.Columns.TicketDescription];
                            tRow[DatabaseObjects.Columns.Title] = row.m[DatabaseObjects.Columns.Title];
                            resultedTable.Rows.Add(tRow);
                        }
                    }
                    else
                    {
                        var moduleTickets = from m in dtmoduleTicketList.AsEnumerable()
                                            select new { TicketId = m.Field<string>(DatabaseObjects.Columns.TicketId) };

                        List<string> results = new List<string>();
                        results = ParentTicketIDes(new List<string>() { ticketId }, dtTicketRList, results);

                        var relationalTickests = from p in dtTicketRList.AsEnumerable()
                                                 where p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(ticketId)
                                                 select new { TicketId = p.Field<string>(DatabaseObjects.Columns.ChildTicketId) };

                        var resultedTickets = moduleTickets.Except(relationalTickests);

                        var resultedTickets1 = from m in dtmoduleTicketList.AsEnumerable()
                                               join
                                               p in resultedTickets
                                               on m.Field<string>(DatabaseObjects.Columns.TicketId) equals p.TicketId
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) != ticketId.Trim()
                                               where !results.Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               && !(from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select new { m };

                        foreach (var row in resultedTickets1)
                        {
                            DataRow tRow = resultedTable.NewRow();
                            tRow[DatabaseObjects.Columns.Id] = row.m[DatabaseObjects.Columns.Id];
                            tRow[DatabaseObjects.Columns.TicketId] = row.m[DatabaseObjects.Columns.TicketId];
                            tRow[DatabaseObjects.Columns.Title] = row.m[DatabaseObjects.Columns.Title];
                            resultedTable.Rows.Add(tRow);
                        }
                    }
                }
            }

            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                DataView view = resultedTable.DefaultView;
                view.Sort = string.Format("{0} desc", DatabaseObjects.Columns.TicketId);
                resultedTable = view.ToTable();
            }

            return resultedTable;
        }

        public List<string> GetDependentTickets(string moduleName, string ticketId)
        {
            List<string> dependentTickets = new List<string>();

            if (!string.IsNullOrEmpty(moduleName))
            {
                DataTable dtmoduleTicketList = ObjTicketManager.GetAllTickets(ObjModuleViewManger.LoadByName(moduleName));

                TicketRelationshipHelper ticketRelation = null;
                List<string> parents = new List<string>();
                if (!string.IsNullOrEmpty(ticketId))
                {
                    ticketRelation = Load(ticketId.Trim(), -1, 0, true, true);
                    if (ticketRelation != null && ticketRelation.ParentTicketIDs != null)
                    {
                        XmlNodeList nodeList = ticketRelation.ParentTicketIDs.DocumentElement.SelectNodes("//Parent[@ParentTicketId]");
                        foreach (System.Xml.XmlNode node in nodeList)
                        {
                            parents.Add(node.Attributes["ParentTicketId"].Value);
                        }
                    }
                }

                DataTable dtTicketRList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelation,$"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                if (dtmoduleTicketList != null)
                {
                    if (dtTicketRList == null)
                    {
                        var resultedTickets1 = from m in dtmoduleTicketList.AsEnumerable()
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId.Trim()
                                               || (from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select new { m };

                        foreach (var row in resultedTickets1)
                        {
                            dependentTickets.Add(Convert.ToString(row.m[DatabaseObjects.Columns.TicketId]));
                        }
                    }
                    else
                    {
                        var moduleTickets = from m in dtmoduleTicketList.AsEnumerable()
                                            select new { TicketId = m.Field<string>(DatabaseObjects.Columns.TicketId) };

                        var relationalTickests = from p in dtTicketRList.AsEnumerable()
                                                 select new { TicketId = p.Field<string>(DatabaseObjects.Columns.ChildTicketId) };

                        var resultedTickets = moduleTickets.Intersect(relationalTickests);

                        var resultedTickets1 = from m in dtmoduleTicketList.AsEnumerable()
                                               join
                                               p in resultedTickets
                                               on m.Field<string>(DatabaseObjects.Columns.TicketId) equals p.TicketId
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId.Trim()
                                               || (from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select m;

                        foreach (var row in resultedTickets1)
                        {
                            dependentTickets.Add(Convert.ToString(row["TicketId"]));
                        }
                    }
                }
            }

            if (!dependentTickets.Contains(ticketId))
            {
                dependentTickets.Add(ticketId);
            }

            return dependentTickets;
        }

        public List<string> GetParentTickets(string moduleName, string ticketId)
        {
            List<string> parentTickets = new List<string>();
            List<string> lstParentTicketIDs = new List<string>();

            if (!string.IsNullOrEmpty(moduleName))
            {
                TicketRelationshipHelper ticketRelation = new TicketRelationshipHelper(context, ticketId.Trim());
                List<string> parents = new List<string>();
                if (!string.IsNullOrEmpty(ticketId))
                {
                    ticketRelation = Load(ticketId.Trim(), -1, 0, true, true);
                    if (ticketRelation != null && ticketRelation.ParentTicketIDs != null)
                    {
                        XmlNodeList nodeList = ticketRelation.ParentTicketIDs.DocumentElement.SelectNodes("//Parent[@ParentTicketId]");
                        foreach (System.Xml.XmlNode node in nodeList)
                        {
                            parents.Add(node.Attributes["ParentTicketId"].Value);
                        }
                    }
                }

                DataTable moduleTicketList_dt = ObjTicketManager.GetAllTickets(ObjModuleViewManger.LoadByName(moduleName));
                DataTable relationshipTicketList_dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelation, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                if (moduleTicketList_dt != null)
                {
                    if (relationshipTicketList_dt == null)
                    {
                        var resultedTickets1 = from m in moduleTicketList_dt.AsEnumerable()
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId.Trim()
                                               || (from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select new { m };

                        foreach (var row in resultedTickets1)
                        {
                            parentTickets.Add(Convert.ToString(row.m[DatabaseObjects.Columns.TicketId]));
                        }
                    }
                    else
                    {
                        var moduleTickets = from m in moduleTicketList_dt.AsEnumerable()
                                            select new { TicketId = m.Field<string>(DatabaseObjects.Columns.TicketId) };

                        var relationalTickests = from p in relationshipTicketList_dt.AsEnumerable()
                                                 where p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(ticketId)
                                                 select new { TicketId = p.Field<string>(DatabaseObjects.Columns.ChildTicketId) };

                        List<string> results = new List<string>();
                        results = ParentTicketIDes(new List<string>() { ticketId }, relationshipTicketList_dt, results);
                        //Added by mudassir 13 march 2020
                        if (results != null && results.Count > 0)
                        {
                            List<string> lstSibLing = relationshipTicketList_dt.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ParentTicketId) == results.FirstOrDefault()
                                                                                          && !string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.ChildTicketId))
                                                                                          && x.Field<string>(DatabaseObjects.Columns.ChildTicketId) != ticketId
                                                                                          && (!x.IsNull(DatabaseObjects.Columns.ParentModuleName) && x.Field<string>(DatabaseObjects.Columns.ParentModuleName) != "WIKI")).Select(x => x.Field<string>(DatabaseObjects.Columns.ChildTicketId)).ToList();
                            parentTickets.AddRange(lstSibLing);
                        }
                        //

                        var resultedTickets = moduleTickets.Intersect(relationalTickests);

                        var resultedTickets1 = from m in moduleTicketList_dt.AsEnumerable()
                                               join
                                               p in resultedTickets
                                               on m.Field<string>(DatabaseObjects.Columns.TicketId) equals p.TicketId
                                               where m.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId.Trim()
                                               where !results.Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               || (from o in parents select o).Contains(m.Field<string>(DatabaseObjects.Columns.TicketId))
                                               select new { m };
                        //Added by mudassir 13 march 2020
                        foreach (var row in resultedTickets)
                        {
                            parentTickets.Add(Convert.ToString(row.TicketId));

                        }

                        //foreach (var row in results)  commented 13 march 2020
                        //{
                        //    parentTickets.Add(row);
                        //} commented 13 march 2020
                        //



                    }
                }
            }

            if (!parentTickets.Contains(ticketId))
            {
                parentTickets.Add(ticketId);
            }
            //Added by mudassir 13 march 2020
            parentTickets = parentTickets.Distinct().ToList();
            //
            return parentTickets;
        }

        public List<string> ParentTicketIDes(List<string> ticketID, DataTable relationshipTicketList_dt, List<string> result)
        {

            var parentTicket = relationshipTicketList_dt
                                .AsEnumerable()
                                .Where(w => ticketID.Contains(w.Field<string>(DatabaseObjects.Columns.ChildTicketId)))
                                .Select(s => s.Field<string>(DatabaseObjects.Columns.ParentTicketId))
                                .ToList();

            foreach (var item in parentTicket)
                result.Add(item);

            if (parentTicket.Count > 0)
                ParentTicketIDes(parentTicket, relationshipTicketList_dt, result);

            return result;
        }

        private XmlDocument GetPrarent(string currentTicketId, DataTable rTable, int parentUpLevel, bool withDetail)
        {
            XmlDocument parentsDoc = null;
            if (parentUpLevel != 0)
            {
                parentsDoc = new XmlDocument();
                System.Xml.XmlElement Parents = parentsDoc.CreateElement("Parents");
                parentsDoc.AppendChild(Parents);
                Parents.SetAttribute("CurrentTicketId", currentTicketId);
                List<string> results = new List<string>();

                results = ParentTicketIDes(new List<string>() { currentTicketId }, rTable, results);
                foreach (string ticket in results)
                {
                    System.Xml.XmlElement parentXmlElement = GetParentXmlElement(parentsDoc, rTable, parentUpLevel, 0, ticket, withDetail);
                    if (parentXmlElement != null)
                    {
                        Parents.AppendChild(parentXmlElement);
                    }
                }
            }
            return parentsDoc;
        }

        private XmlDocument GetChildren(string currentTicketId, DataTable rTable, int childDownLevel, bool withDetail)
        {
            XmlDocument childsDoc = null;
            if (childDownLevel != 0)
            {
                childsDoc = new XmlDocument();
                System.Xml.XmlElement children = childsDoc.CreateElement("Children");
                childsDoc.AppendChild(children);
                children.SetAttribute("CurrentTicketId", currentTicketId);
                GetXmlElement(childsDoc, children, rTable, false, childDownLevel, 0, withDetail);
            }
            return childsDoc;
        }
        private void GetXmlElement(XmlDocument xmlDoc, System.Xml.XmlElement xmlEt, DataTable rTable, bool showParent, int requiredLevel, int currentLevel, bool withDetail)
        {
            string rootNodeName = "Children";
            string nodeName = "Child";
            string attibuteName = "ChildTicketId";
            string searchTicket = DatabaseObjects.Columns.ParentTicketId;
            string ticketColumn = DatabaseObjects.Columns.ChildTicketId;
            if (showParent)
            {
                rootNodeName = "Parents";
                nodeName = "Parent";
                attibuteName = "ParentTicketId";
                searchTicket = DatabaseObjects.Columns.ChildTicketId;
                ticketColumn = DatabaseObjects.Columns.ParentTicketId;
            }

            string ticketId = string.Empty;
            if (xmlEt.Name == rootNodeName)
            {
                ticketId = xmlEt.GetAttribute("CurrentTicketId");
            }
            else
            {
                ticketId = xmlEt.GetAttribute(attibuteName);
            }

            var rows = from u in rTable.AsEnumerable()
                       where u.Field<string>(searchTicket) == ticketId
                       select u;

            currentLevel = currentLevel + 1;
            if (rows != null && rows.Count() > 0)
            {
                DataRow[] rows1 = rows.ToArray();
                for (int i = 0; i < rows1.Length; i++)
                {
                    ticketId = rows1[i][ticketColumn].ToString();
                    System.Xml.XmlElement xmlChild = xmlDoc.CreateElement(nodeName);
                    xmlChild.SetAttribute(attibuteName, ticketId);
                    if (!withDetail)
                    {
                        xmlChild = GenerateTicketDetailXmlElement(xmlChild, ticketId);
                    }
                    xmlEt.AppendChild(xmlChild);
                    if (requiredLevel < 0 || currentLevel != requiredLevel)
                    {
                        GetXmlElement(xmlDoc, xmlChild, rTable, showParent, requiredLevel, currentLevel, withDetail);
                    }
                }
            }
        }

        private System.Xml.XmlElement GetParentXmlElement(XmlDocument xmlDoc, DataTable rTable, int requiredLevel, int currentLevel, string currentTicketId, bool withDetail)
        {
            System.Xml.XmlElement xmlEt = null; ;
            string nodeName = "Parent";
            string attibuteName = "ParentTicketId";
            string searchTicket = DatabaseObjects.Columns.ChildTicketId;
            string ticketColumn = DatabaseObjects.Columns.ParentTicketId;

            string ticketId = currentTicketId;

            var rows = from u in rTable.AsEnumerable()
                       where u.Field<string>(searchTicket) == ticketId
                       select u;
            currentLevel = currentLevel + 1;
            if (rows != null && rows.Count() > 0)
            {
                xmlEt = xmlDoc.CreateElement(nodeName);
                xmlEt.SetAttribute(attibuteName, rows.First()[ticketColumn].ToString());
                if (!withDetail)
                {
                    xmlEt = GenerateTicketDetailXmlElement(xmlEt, rows.First()[ticketColumn].ToString());
                }

                if (requiredLevel < 0 || currentLevel != requiredLevel)
                {
                    System.Xml.XmlElement xmlE = GetParentXmlElement(xmlDoc, rTable, requiredLevel, currentLevel, rows.First()[ticketColumn].ToString(), withDetail);
                    if (xmlE != null)
                    {
                        System.Xml.XmlElement element = null;
                        if (xmlE.LastChild == null)
                        {
                            element = xmlE;
                        }
                        else
                        {
                            element = (System.Xml.XmlElement)xmlE.SelectSingleNode("/" + nodeName);
                        }

                        element.AppendChild(xmlEt);
                        xmlEt = xmlE;
                    }
                }
            }
            return xmlEt;
        }

        public System.Xml.XmlElement GenerateTicketDetailXmlElement(System.Xml.XmlElement xmlEt, string ticketId)
        {
            DataTable table = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketId), ticketId).Table;
            if (xmlEt == null)
            {
                XmlDocument doc = new XmlDocument();
                xmlEt = doc.CreateElement("Ticket");
            }

            if (table != null)
            {
                if (table.Columns.Contains(DatabaseObjects.Columns.Title))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.Title, table.Rows[0][DatabaseObjects.Columns.Title].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.ModuleName, table.Rows[0][DatabaseObjects.Columns.ModuleName].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.ModuleName, table.Rows[0][DatabaseObjects.Columns.ModuleNameLookup].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.TicketPriorityLookup, table.Rows[0][DatabaseObjects.Columns.TicketPriorityLookup].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.ModuleStepLookup, table.Rows[0][DatabaseObjects.Columns.ModuleStepLookup].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.TicketCreationDate, table.Rows[0][DatabaseObjects.Columns.TicketCreationDate].ToString());
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleRelativePagePath))
                {
                    xmlEt.SetAttribute(DatabaseObjects.Columns.ModuleRelativePagePath, table.Rows[0][DatabaseObjects.Columns.ModuleRelativePagePath].ToString());
                }
            }
            return xmlEt;
        }

        //private static string LoadTicketDetail(string ticketId)
        //{
        //    StringBuilder details = new StringBuilder();
        //    details.Append("<strong>");
        //    details.Append(ticketId);
        //    details.Append(" </strong>");

        //    DataTable table = GetTicketDetail(ticketId);
        //    if (table.Columns.Contains(DatabaseObjects.Columns.Title))
        //    {
        //        details.Append(" (");
        //        string value = table.Rows[0][DatabaseObjects.Columns.Title].ToString();
        //        value = value.Length > 50 ? value.Remove(50) + "..." : value;
        //        details.Append(value.Trim());
        //        details.Append(") ");
        //    }

        //    if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
        //    {
        //        try
        //        {
        //            string value = table.Rows[0][DatabaseObjects.Columns.TicketCreationDate].ToString();
        //            DateTime creationDate = DateTime.Parse(value.Trim());
        //            details.Append("created <strong>");
        //            details.Append(creationDate.ToString("MMM-dd-yyyy"));
        //            details.Append(" </strong>");
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }

        //    if (table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
        //    {
        //        details.Append("has priority <strong>");
        //        string value = table.Rows[0][DatabaseObjects.Columns.TicketPriorityLookup].ToString();
        //        details.Append(value);
        //        details.Append(" </strong>");
        //    }

        //    if (table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
        //    {
        //        if (table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup).ToString() == null)
        //            details.Append("has ");
        //        else details.Append("and ");

        //        details.Append("status <Strong>");
        //        string value = table.Rows[0][DatabaseObjects.Columns.ModuleStepLookup].ToString();
        //        details.Append(value);
        //        details.Append(" </strong>");
        //    }

        //    //if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
        //    //{
        //    //    xmlEt.SetAttribute(DatabaseObjects.Columns.TicketCreationDate, table.Rows[0][DatabaseObjects.Columns.TicketCreationDate].ToString());
        //    //}

        //    //if (table.Columns.Contains(DatabaseObjects.Columns.ModuleRelativePagePath))
        //    //{
        //    //    xmlEt.SetAttribute(DatabaseObjects.Columns.ModuleRelativePagePath, table.Rows[0][DatabaseObjects.Columns.ModuleRelativePagePath].ToString());
        //    //}

        //    return details.ToString();
        //}
        public string CreateNavigateURL(string ticketId)
        {
            DataTable table = GetTicketDetail(ticketId);
            string navigationUrl = "javascript:";
            if (table != null && ticketId != null & ticketId.Trim() != string.Empty)
            {
                string url = string.Empty;
                if (table.Columns.Contains(DatabaseObjects.Columns.StaticModulePagePath))
                {
                    url = table.Rows[0][DatabaseObjects.Columns.StaticModulePagePath].ToString();
                }
                url = UGITUtility.GetAbsoluteURL(url);
                //navigationUrl = string.Format("javascript:(window.parent) ? window.parent.UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Ticket:{1}\",\"auto\",\"auto\") : UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Ticket: {1}\",\"auto\",\"auto\")", url, ticketId, table.Rows[0][DatabaseObjects.Columns.ModuleName].ToString());
                navigationUrl = string.Format("javascript:(window.parent) ? window.parent.UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Item:{1}\",\"auto\",\"auto\") : UgitOpenPopupDialog(\"{0}\",\"TicketId={1}\",\"{2} Item: {1}\",\"auto\",\"auto\")", url, ticketId, table.Rows[0][DatabaseObjects.Columns.ModuleName].ToString());
            }
            return System.Web.HttpUtility.HtmlEncode(navigationUrl);
        }
        public DataTable LoadTickets(string currentTicketID)
        {
            DataTable dtTickets = new DataTable();
            dtTickets.Columns.Add("ID", typeof(int));
            dtTickets.Columns.Add("ParentId", typeof(int));
            dtTickets.Columns.Add("TicketID", typeof(string));
            dtTickets.Columns.Add("NavigateURL", typeof(string));
            dtTickets.Columns.Add("TicketType", typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.TicketCreationDate, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup, typeof(string));
            dtTickets.Columns.Add(DatabaseObjects.Columns.ModuleStepLookup, typeof(string));
            DataTable relationshipTicketList_dt = GetTicketRelationshipDT();
            try
            {
                if (relationshipTicketList_dt != null && relationshipTicketList_dt.Rows.Count > 0)
                {
                    bool hasParentItem = false, hasChildItems = false, hasSiblingItems = false;

                    // Add group headers
                    int id = 0;
                    DataRow parentHeaderRow = dtTickets.Rows.Add(++id, 0, "Parent Item(s)", "javascript:");
                    DataRow siblingHeaderRow = dtTickets.Rows.Add(++id, 0, "Sibling Item(s)", "javascript:");
                    DataRow childHeaderRow = dtTickets.Rows.Add(++id, 0, "Child Item(s)", "javascript:");

                    // Group tickets by parent & child tickets
                    var tickets = from p in relationshipTicketList_dt.AsEnumerable()
                                  where ((!string.IsNullOrEmpty(Convert.ToString(p[DatabaseObjects.Columns.ParentTicketId])) && Convert.ToString(p[DatabaseObjects.Columns.ParentTicketId]) == currentTicketID) ||
                                         (!string.IsNullOrEmpty(Convert.ToString(p[DatabaseObjects.Columns.ChildTicketId])) && Convert.ToString(p[DatabaseObjects.Columns.ChildTicketId]) == currentTicketID)) &&
                                         (!p.IsNull(DatabaseObjects.Columns.ChildModuleName) && p.Field<string>(DatabaseObjects.Columns.ChildModuleName) != ModuleNames.WIKI)
                                         
                                  select new
                                  {
                                      TicketType = Convert.ToString(p[DatabaseObjects.Columns.ParentTicketId]) == currentTicketID ? "C" : "P",
                                      TicketID = Convert.ToString(p[DatabaseObjects.Columns.ParentTicketId]) == currentTicketID ?
                                                 Convert.ToString(p[DatabaseObjects.Columns.ChildTicketId]) : Convert.ToString(p[DatabaseObjects.Columns.ParentTicketId])
                                  };

                    // Get parent & child tickets
                    foreach (var t in tickets)
                    {
                        if (t.TicketType.Equals("P"))
                        {
                            dtTickets.Rows.Add(++id, 1, t.TicketID, CreateNavigateURL(t.TicketID), t.TicketType);
                            hasParentItem = true;
                        }
                        else if (t.TicketType.Equals("C"))
                        {
                            dtTickets.Rows.Add(++id, 3, t.TicketID, CreateNavigateURL(t.TicketID), t.TicketType);
                            hasChildItems = true;
                        }
                    }

                    // Get sibling tickets, excluding current ticket
                    if (dtTickets != null && dtTickets.Rows.Count > 0)
                    {
                        var groupByParentTicketId = dtTickets.AsEnumerable().Where(x => !x.IsNull("TicketType") && x.Field<string>("TicketType") == "P").GroupBy(x => x.Field<string>("TicketID"));
                        foreach (var parent in groupByParentTicketId)
                        {
                            DataRow[] siblings = relationshipTicketList_dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId]) == parent.Key &&
                                                                                                   !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId]) != currentTicketID &&
                                                                                                   !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ChildModuleName])) && Convert.ToString(x[DatabaseObjects.Columns.ChildModuleName]) != "WIKI").ToArray();
                            if (siblings == null || siblings.Length == 0)
                                continue;
                            foreach (DataRow dr in siblings)
                            {
                                string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.ChildTicketId]);
                                dtTickets.Rows.Add(++id, 2, ticketId, CreateNavigateURL(ticketId));
                                hasSiblingItems = true;
                            }
                        }
                    }

                    // Add additional details for tickets
                    foreach (DataRow dr in dtTickets.Rows)
                    {
                        DataTable table = GetTicketDetail(Convert.ToString(dr["TicketID"]));
                        if (table != null && table.Rows.Count > 0)
                        {
                            dr[DatabaseObjects.Columns.Title] = table.Rows[0][DatabaseObjects.Columns.Title];

                            if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                                dr[DatabaseObjects.Columns.TicketCreationDate] = table.Rows[0][DatabaseObjects.Columns.TicketCreationDate];
                            else if (table.Columns.Contains(DatabaseObjects.Columns.Created))
                                dr[DatabaseObjects.Columns.TicketCreationDate] = table.Rows[0][DatabaseObjects.Columns.Created];

                            if (table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                                dr[DatabaseObjects.Columns.TicketPriorityLookup] = table.Rows[0][DatabaseObjects.Columns.TicketPriorityLookup];

                            if (table.Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
                                dr[DatabaseObjects.Columns.ModuleStepLookup] = table.Rows[0][DatabaseObjects.Columns.ModuleStepLookup];
                        }
                    }

                    // Remove any unused sections
                    if (!hasParentItem)
                        dtTickets.Rows.Remove(parentHeaderRow);
                    if (!hasSiblingItems)
                        dtTickets.Rows.Remove(siblingHeaderRow);
                    if (!hasChildItems)
                        dtTickets.Rows.Remove(childHeaderRow);
                }
                return dtTickets;

            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteLog(string.Format("ERROR in Related Tickets LoadTickets(): {0}", ex.Message));
            }
            //if (relationshipTicketList_dt != null)
            //{

            //    var tickets = from p in relationshipTicketList_dt.AsEnumerable()
            //                  where ((!p.IsNull(DatabaseObjects.Columns.ParentTicketId) && p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID))
            //                     || (!p.IsNull(DatabaseObjects.Columns.ChildTicketId) && p.Field<string>(DatabaseObjects.Columns.ChildTicketId) == currentTicketID)) &&
            //                        (!p.IsNull(DatabaseObjects.Columns.ParentModuleName) && p.Field<string>(DatabaseObjects.Columns.ParentModuleName) != ModuleNames.WIKI)
            //                  select new
            //                  {
            //                      TicketType = p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID) ?
            //                                   "S" : "P",
            //                      TicketID = p.Field<string>(DatabaseObjects.Columns.ParentTicketId).Equals(currentTicketID) ?
            //                                 p.Field<string>(DatabaseObjects.Columns.ChildTicketId) :
            //                                 p.Field<string>(DatabaseObjects.Columns.ParentTicketId)
            //                  };

            //    int id = 3;

            //    foreach (var t in tickets.Distinct())
            //    {
            //        if (t.TicketType.Equals("P"))
            //        { dtTickets.Rows.Add(id++, 1, t.TicketID, CreateNavigateURL(t.TicketID), "Parent Item(s)"); }

            //        else if (t.TicketType.Equals("S"))
            //        { dtTickets.Rows.Add(id++, 2, t.TicketID, CreateNavigateURL(t.TicketID), "Sub Item(s)"); }

            //    }

            //    // Get sibling tickets, excluding current ticket
            //    if (dtTickets != null && dtTickets.Rows.Count > 0)
            //    {
            //        var groupByParentTicketId = dtTickets.AsEnumerable().Where(x => !x.IsNull("TicketType") && x.Field<string>("TicketType") == "Parent Item(s)").GroupBy(x => x.Field<string>("TicketID"));
            //        foreach (var parent in groupByParentTicketId)
            //        {
            //            DataRow[] siblings = relationshipTicketList_dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ParentTicketId]) == parent.Key &&
            //                                                                                   !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId])) && Convert.ToString(x[DatabaseObjects.Columns.ChildTicketId]) != currentTicketID &&
            //                                                                                   !string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.ParentModuleName])) && Convert.ToString(x[DatabaseObjects.Columns.ParentModuleName]) != ModuleNames.WIKI).ToArray();

            //            if (siblings == null || siblings.Length == 0)
            //                continue;
            //            foreach (DataRow dr in siblings)
            //            {
            //                string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.ChildTicketId]);
            //                dtTickets.Rows.Add(++id, 2, ticketId, CreateNavigateURL(ticketId), "Sibling Item(s)");
            //            }
            //        }
            //    }
            //    //
            //    foreach (DataRow dr in dtTickets.Rows)
            //    {
            //        DataTable dtable = GetTicketDetail(Convert.ToString(dr["TicketID"]));
            //        if (dtable == null || dtable.Rows.Count == 0)
            //            continue;
            //        DataRow[] rowColl= dtable.Select("TicketID='" + Convert.ToString(dr["TicketID"]) + "'");
            //        DataRow table = null;
            //        if (rowColl != null && rowColl.Length > 0)
            //            table = rowColl[0];

            //        if (table != null && (table.Table).Rows.Count > 0)
            //        {
            //            dr[DatabaseObjects.Columns.Title] = table[DatabaseObjects.Columns.Title];
            //            var moduleName = Convert.ToString(dr["TicketID"]).Split('-')[0];
            //            if (!(moduleName.ToUpper() == ModuleNames.CMDB))
            //            {
            //                if ((moduleName.ToUpper() == ModuleNames.WIKI || moduleName.ToUpper() == ModuleNames.CMT))
            //                {
            //                    dr[DatabaseObjects.Columns.TicketCreationDate] = table[DatabaseObjects.Columns.TicketCreatedDate];
            //                    if ((table.Table).Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
            //                        dr[DatabaseObjects.Columns.TicketPriorityLookup] = table[DatabaseObjects.Columns.TicketPriorityLookup];
            //                }
            //                else
            //                {
            //                    if ((table.Table).Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
            //                        dr[DatabaseObjects.Columns.TicketCreationDate] = table[DatabaseObjects.Columns.TicketCreationDate];
            //                    if ((table.Table).Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
            //                        dr[DatabaseObjects.Columns.TicketPriorityLookup] = table[DatabaseObjects.Columns.TicketPriorityLookup];
            //                    if ((table.Table).Columns.Contains(DatabaseObjects.Columns.ModuleStepLookup))
            //                        dr[DatabaseObjects.Columns.ModuleStepLookup] = table[DatabaseObjects.Columns.ModuleStepLookup];
            //                }
            //            }



            //        }
            //    }

            //    if (tickets.Count() == 0)
            //    {
            //        dtTickets.Clear();
            //    }
            //    else
            //    {

            //    }
            //}
            return dtTickets;
        }





        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="spweb"></param>
        ///// <param name="ticketID"></param>
        ///// <param name="type">1: Parent, 2: Children, else both parent & children</param>
        ///// <param name="resolution"></param>
        public void CloseTickets(string ticketID, int type, string resolution, UserProfile prp, string resolutionType)
        {
            DataTable relatedData = LoadTickets(ticketID);
            if (relatedData == null || relatedData.Rows.Count == 0)
                return;

            List<string> ticketClosed = new List<string>();
            DataRow[] childTickets = new DataRow[0];

            if (type == 1)
                childTickets = relatedData.Select(string.Format("ParentId=1"));
            else if (type == 2)
                childTickets = relatedData.Select(string.Format("ParentId=2"));
            else
                childTickets = relatedData.Select();

            List<Ticket> tickets = new List<Ticket>();
            DataRow item = null;

            foreach (DataRow row in childTickets)
            {
                string ctID = Convert.ToString(row["TicketID"]);
                string module = ctID.Split(new char[] { '-' })[0];
                Ticket tk = tickets.FirstOrDefault(x => x.Module != null && x.Module.ModuleName == module);

                if (tk == null)
                {
                    tk = new Ticket(context, module);

                    if (tk != null)
                        tickets.Add(tk);
                }

                if (tk == null)
                    continue;

                item = Ticket.GetCurrentTicket(context, module, ctID);

                if (item != null && !UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketClosed]) &&
                    !ticketClosed.Exists(x => x == Convert.ToString(item[DatabaseObjects.Columns.TicketId])))
                {
                    // Set actual hours and resolution type if null
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketPRP, item.Table) &&
                        item[DatabaseObjects.Columns.TicketPRP] == null)
                    {
                        item[DatabaseObjects.Columns.TicketPRP] = prp;
                    }

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketActualHours, item.Table) &&
                       string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.TicketActualHours])))
                    {
                        item[DatabaseObjects.Columns.TicketActualHours] = 0;
                    }

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketResolutionType, item.Table) &&
                       string.IsNullOrWhiteSpace(Convert.ToString(item[DatabaseObjects.Columns.TicketResolutionType])))
                    {
                        item[DatabaseObjects.Columns.TicketResolutionType] = resolutionType;
                    }

                    tk.CloseTicket(item, resolution);
                    tk.CommitChanges(item, null, forceUpdate: true);
                    ticketClosed.Add(Convert.ToString(item[DatabaseObjects.Columns.TicketId]));
                }
            }

            // Close child tasks for SVC
            string moduleName = uHelper.getModuleNameByTicketId(ticketID);

            if (moduleName == ModuleNames.SVC)
            {
                List<UGITTask> dependentTasks = objTaskManager.Load(x => x.TicketId == ticketID);
                if (dependentTasks != null && dependentTasks.Count > 0)
                {
                    foreach (UGITTask instDep in dependentTasks.Where(x => x.Behaviour == "Task"))
                    {
                        if (instDep.Status != Constants.Completed)
                        {
                            instDep.PercentComplete = 100;
                            instDep.Status = Utility.Constants.Cancelled;
                            objTaskManager.Save(instDep);
                        }
                    }
                    //TaskCache.ReloadProjectTasks(moduleName, ticketID);
                }
            }
        }

        public DataTable GetFilteredAssetDetail(string ticketId)
        {
            List<ModuleColumn> lstModuleColumns;
            lstModuleColumns = objmodulecolumnmanager.Load($"{DatabaseObjects.Columns.CategoryName}='" + Constants.MyAssets + "'").OrderBy(x => x.FieldSequence).ToList();
            List<string> columnNames = new List<string>();
            foreach (var col in lstModuleColumns)
            {
                columnNames.Add(col.FieldName);
            }
            if(!columnNames.Exists(e => e.Contains(DatabaseObjects.Columns.TicketId)))
            {
                columnNames.Add(DatabaseObjects.Columns.TicketId);
            }
            
            
            string[] colarr = columnNames.ToArray();

            DataTable moduleTicketList = null;
            if (ticketId != null)
            {
                string module = ticketId.Split(new char[] { '-' })[0];
                DataTable moduleList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ModuleName}='{module}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                DataView view = new DataView(moduleList);
                moduleList = view.ToTable(false, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.ModuleTicketTable, DatabaseObjects.Columns.ModuleRelativePagePath, DatabaseObjects.Columns.StaticModulePagePath });

                DataRow[] mCollection = moduleList.Select();
                if (mCollection != null && mCollection.Count() > 0)
                {
                    DataRow item = mCollection[0];
                    string moduleTicketTable = Convert.ToString(mCollection[0][DatabaseObjects.Columns.ModuleTicketTable]); //mCollection[0][moduleList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.ModuleTicketTable).ToString()].ToString();
                    moduleTicketList = GetTableDataManager.GetTableData(moduleTicketTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                    DataView dview = new DataView(moduleTicketList);
                    moduleTicketList = dview.ToTable(false, colarr);
                }
            }
            return moduleTicketList;
        }

        public DataTable GetRelatedTicketsDetails(int assetID, ApplicationContext context)
        {
            DataTable relatedTicketDetails = new DataTable();
            DataTable assetTicketList = GetTableDataManager.GetTableData("AssetIncidentRelations", $"TenantID='{context.TenantID}'");
            DataRow[] rCollection= assetTicketList.Select(string.Format("{0} LIKE '%{1}%'", DatabaseObjects.Columns.AssetTagNumLookup, assetID));
            if (rCollection != null && rCollection.Length > 0)
            {
                DataTable rTable = rCollection.CopyToDataTable();
                ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                TicketManager ticketManager = new TicketManager(context);
                List<string> lstModuleNames = new List<string>();
                foreach (DataRow item in rTable.Rows)
                {
                    string moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(item[DatabaseObjects.Columns.TicketId]));
                    UGITModule module = moduleViewManager.LoadByName(moduleName);
                    DataTable openTickets = ticketManager.GetAllTickets(module);
                    if (openTickets == null || openTickets.Rows.Count == 0)
                        continue;
                    DataRow[] dr = openTickets.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == Convert.ToString(item[DatabaseObjects.Columns.TicketId])).ToArray();
                    if (dr.Length > 0)
                    {
                        if (relatedTicketDetails.Rows.Count == 0)
                        {
                            relatedTicketDetails = openTickets.Clone();
                            relatedTicketDetails = dr.CopyToDataTable();
                        }
                        else
                            relatedTicketDetails.Merge(dr.CopyToDataTable());
                    }
                }
            }
            return relatedTicketDetails;
        }
    }
}
