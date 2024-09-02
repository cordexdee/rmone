using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using uGovernIT.Utility;
//using uGovernIT.Web.Models;



namespace uGovernIT.Manager
{
    public class AssetData
    {
        public AssetData()
        {

        }
        public string AssetName { get; set; }
        public string AssetModel { get; set; }
    }
    public class ServiceAgentsManager
    {
       
       
        private ApplicationContext _context;
        AssetData assetData = new AssetData();


        public ServiceAgentsManager(ApplicationContext context)
        {
            this._context = context;
        }

        public AssetData UserQuestionSummary(string ticketID)
        {


            var svcTicketData = GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.SVCRequests, DatabaseObjects.Columns.UserQuestionSummary, ticketID, _context.TenantID);
            try
            {
                XmlDocument doc = new XmlDocument();

                //string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(data, DatabaseObjects.Columns.UserQuestionSummary));

                doc.LoadXml(svcTicketData.ToString().Trim());
                var inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                foreach (var section in inputObj.ServiceSections)
                {
                    //Need to write for every asset
                            assetData.AssetName = section.Questions.Where(x => x.Token.ToLower() == "needlaptop").Select(x => x.Value).FirstOrDefault();
                            assetData.AssetModel = section.Questions.Where(x => x.Token.ToLower() == "laptopmodel").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(assetData.AssetName) && !string.IsNullOrEmpty(assetData.AssetModel))
                        break;

                }
                // var dateRequest = Convert.ToDateTime(data[DatabaseObjects.Columns.TicketCreationDate]);

            }
            catch (System.Exception)
            {

                throw;
            }
            return assetData;

        }

        //public void SetPRP(List<UGITTask> ticketRelatedTask)
        //{
        //    {
        //        var moduleTasksOrTicketData = ticketRelatedTask.Where(x => x.Title == serviceAgent.Workflow.Title).FirstOrDefault();

        //        if (moduleTasksOrTicketData != null)
        //        {

        //            moduleTypeTable = moduleTasksOrTicketData.Behaviour.ToLower() == "task" ? DatabaseObjects.Tables.ModuleTasks : "SVCRequests";

        //            title = moduleTasksOrTicketData.Title;

        //            columnName = moduleTypeTable == DatabaseObjects.Tables.ModuleTasks ? DatabaseObjects.Columns.AssignedTo : DatabaseObjects.Columns.PRP;

        //            //if (GetTableDataManager.IsLookaheadTicketExists(moduleTable, context.TenantID, serviceAgent.Workflow.Title, "",true))
        //            {

        //                if (moduleTasksOrTicketData.Behaviour.ToLower() == "task")
        //                {
        //                    groupOfOpenTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, context.TenantID, title, columnName, "Waiting");

        //                    groupOfClosedTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, context.TenantID, title, columnName, "Completed");
        //                }
        //                else
        //                {
        //                    groupOfOpenTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, context.TenantID, title, columnName, 0, true);
        //                    groupOfClosedTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, context.TenantID, title, columnName, 1, true);
        //                }
        //                columnName = moduleTasksOrTicketData.Behaviour.ToLower() == "task" ? DatabaseObjects.Columns.AssignedTo : DatabaseObjects.Columns.PRP;

        //                prpUser = AvailablePRPAndAssignTo.GetPRPOrAssignTo(groupOfOpenTicketPRP, groupOfClosedTicketPRP, columnName);

        //                var listOfTask = ticketRelatedTask.Where(x => x.ID == Convert.ToInt64(moduleTasksOrTicketData.ID)).ToList();

        //                AvailableAsset = GetTableDataManager.ISAsset(context.TenantID, "samsung", DatabaseObjects.Tables.AssetModels);//send Service question value.

        //                if (AvailableAsset)
        //                {
        //                    AssetModel availableAsset = AssetModelViewManager.Get($" {DatabaseObjects.Columns.ModelName}='samsung'");

        //                    if (availableAsset != null)
        //                    {
        //                        var assetTicket = AssetsManger.Get($"{DatabaseObjects.Columns.AssetModelLookup}={availableAsset.ID} and {DatabaseObjects.Columns.StageStep}=2");

        //                        if (assetTicket != null)
        //                        {
        //                            assetTicket.CurrentUser = "48b7d2da-1af0-40b7-aee7-031c0adae6da";//Need to assign current user which is created
        //                            var assignValue = AssetsManger.Update(assetTicket);

        //                            var ticketData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets, $"{DatabaseObjects.Columns.TicketId}='{assetTicket.TicketId}'");

        //                            TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, moduleTasksOrTicketData.RelatedTicketID, assetTicket.TicketId);
        //                            int rowEffected = tRelation.CreateRelation(context);
        //                            TicketRequest = new Ticket(context, "CMDB");

        //                            UserProfile userProfile = _userProfileManager.LoadById("48b7d2da-1af0-40b7-aee7-031c0adae6da");

        //                            string error = TicketRequest.Approve(userProfile, ticketData.Rows[0]);

        //                            if (string.IsNullOrEmpty(error))
        //                            {
        //                                error = TicketRequest.CommitChanges(ticketData.Rows[0]);
        //                            }

        //                            var childTicket = GetTableDataManager.GetTableData("TSR", $"{DatabaseObjects.Columns.TicketId}='{moduleTasksOrTicketData.RelatedTicketID}'").Select();

        //                            TicketRequest = new Ticket(context, "TSR");
        //                            TicketRequest.CloseTicket(childTicket[0], uHelper.GetCommentString(userProfile, $"test version", childTicket[0], DatabaseObjects.Columns.TicketResolutionComments, false));

        //                            error = TicketRequest.CommitChanges(childTicket[0]);
        //                            if (string.IsNullOrEmpty(error))
        //                            {
        //                                moduleTasksOrTicketData.Status = "Completed";
        //                                ticketClosed = true;
        //                            }
        //                            else
        //                            {
        //                                //Display error message
        //                            }

        //                            if (listOfTask != null)
        //                            {
        //                                foreach (UGITTask item in listOfTask)
        //                                {
        //                                    item.AssignedTo = prpUser;
        //                                    if (ticketClosed)
        //                                        item.Status = "Completed";
        //                                    item.Changes = true;
        //                                }
        //                            }
        //                            TaskManager.SaveTasks(ref listOfTask, "SVC", serviceAgent.Workflow.TicketId);

        //                        }

        //                        else
        //                        {
        //                            //Display message
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    //display msg 
        //                }


        //            }
        //        }
        //    }
        //}


    }
}
