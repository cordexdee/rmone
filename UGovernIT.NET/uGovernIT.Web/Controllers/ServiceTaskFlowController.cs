using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/svctaskapi")]
    public class ServiceTaskFlowController : ApiController
    {

        private ApplicationContext _context = null;

        protected ApplicationContext context
        {
            get
            {
                if (_context == null)
                {
                    _context = System.Web.HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }



        [Route("GetServiceTaskFlowData")]
        public IHttpActionResult GetServiceTaskFlowData( string TicketId,string ModuleName)
        {
            try
            {
                List<Node> lstNodes = new List<Node>();
                List<Edges> lstEdges = new List<Edges>();
                List<Edges> lstTaskPredecessorEdges = new List<Edges>();
                if (string.IsNullOrEmpty(TicketId) || string.IsNullOrEmpty(ModuleName))
                 return BadRequest();

                DataTable TaskList = null; 
                ServiceTaskFlowChartHelper serviceTaskFlowChartHelper = new ServiceTaskFlowChartHelper(context);
                TaskList = serviceTaskFlowChartHelper.GetServiceTicketTask(ModuleName,TicketId);
                List<DataRow> lstTaskList = TaskList.AsEnumerable().ToList();
                int index = 0;

                
                List<TaskNode> masterListTask = new List<TaskNode>(); //it will store master details along with step
                List<TaskNode> stepListTask = new List<TaskNode>();
                List<TaskNode> tempListTask = new List<TaskNode>(); 
                foreach (var task in lstTaskList)
                {
                    TaskNode tk = new TaskNode();
                    tk.Id = task[DatabaseObjects.Columns.ID].ToString();
                    tk.Predecessor = task[DatabaseObjects.Columns.Predecessors].ToString();
                    tk.itemOrder = Convert.ToInt32(task[DatabaseObjects.Columns.ItemOrder]);
                    if (task[DatabaseObjects.Columns.Predecessors] == null || task[DatabaseObjects.Columns.Predecessors].ToString() == "")
                    {
                        tk.step = 1;
                       
                    }
                    else
                    {
                        tk.step = 2;
                        TaskNode tkForStepList = new TaskNode();
                        tkForStepList.Id = task[DatabaseObjects.Columns.ID].ToString();
                        tkForStepList.Predecessor = task[DatabaseObjects.Columns.Predecessors].ToString();
                        tkForStepList.itemOrder = Convert.ToInt32(task[DatabaseObjects.Columns.ItemOrder]);
                        tkForStepList.step = 2;
                        stepListTask.Add(tkForStepList);
                        
                    }
                    masterListTask.Add(tk);
                    
                }

                index = 2;
                int runtill = masterListTask.Count();
                bool notforthisstage = false; 

                for(int i=0;i< runtill; i++)
                {
                    foreach(TaskNode tk in stepListTask)
                    {
                        var pred = tk.Predecessor.Split(',');
                        notforthisstage = false;
                        foreach (string pd in pred)
                        {
                           TaskNode node = stepListTask.Where(x => x.itemOrder == Convert.ToInt32(pd) && x.step == index).SingleOrDefault();
                            if(node != null)
                            {
                                notforthisstage = true;
                                break;
                            }
                        }
                        if(notforthisstage)
                        {

                            TaskNode masterNode = masterListTask.Where(x => x.Id == tk.Id).SingleOrDefault();
                            TaskNode newTk = new TaskNode();
                            newTk.Id = tk.Id;
                            newTk.itemOrder = tk.itemOrder;
                            newTk.step = index + 1;
                            newTk.Predecessor = tk.Predecessor;

                            masterNode.step = index + 1;
                            tempListTask.Add(newTk);
                            notforthisstage = false;
                        }

                    }

                    stepListTask = new List<TaskNode>();
                    
                    stepListTask.AddRange(tempListTask);
                    tempListTask = new List<TaskNode>();
                    ++index;
                    i = 0;
                    runtill = stepListTask.Count();
                }

                double leftX = 0.0;
                double topY = 0.0;
                double childNodeTopY = 0.25;
                double childNodeLeftX = 0.25;
                int edgeIndex = 0;
                List<Node> diagramNodes = new List<Node>();
                var taskGroupByStage = masterListTask.GroupBy(x => x.step).ToList();
                foreach (var ContainerStage in taskGroupByStage)
                {
                    Node containerNode = new Node();
                    containerNode.id = ContainerStage.Key.ToString() + "C";
                    containerNode.text = "";
                    containerNode.type = "verticalContainer";
                    containerNode.width =  2.5;
                    containerNode.height = (ContainerStage.Count() * 0.75) + 0.75;
                    containerNode.leftX = leftX;
                    leftX  = leftX + 4;
                    containerNode.topY= topY ;

                    diagramNodes.Add(containerNode);

                    childNodeTopY = 0.5;
                    foreach (TaskNode taskNode in ContainerStage)
                    {
                        var task = lstTaskList.Where(x => x[DatabaseObjects.Columns.ID].ToString() == taskNode.Id).SingleOrDefault();


                        Node node = new Node();
                        node.id = taskNode.Id.ToString();
                        
                        node.taskType = task[DatabaseObjects.Columns.TaskBehaviour].ToString();
                        if(Constants.TaskType.Ticket.Equals(task[DatabaseObjects.Columns.TaskBehaviour].ToString()))
                        {
                            node.text = task[DatabaseObjects.Columns.RelatedTicketID].ToString() + '\n' + task[DatabaseObjects.Columns.Title].ToString();
                            node.onClickString = serviceTaskFlowChartHelper.getTicketFullUrl(task[DatabaseObjects.Columns.RelatedTicketID].ToString(), task[DatabaseObjects.Columns.Title].ToString());
                            node.onClickString = Ticket.GenerateTicketURL(_context, task[DatabaseObjects.Columns.RelatedTicketID].ToString());
                        }
                        else
                        {
                            node.text = task[DatabaseObjects.Columns.Title].ToString();
                        }
                        
                        node.status = task[DatabaseObjects.Columns.Status].ToString();
                        node.type = "rectangle";
                        node.width = 2;
                        node.height = 0.75;
                        node.topY = childNodeTopY ;
                        node.leftX = childNodeLeftX;
                        
                       // node.onClickString = 
                        childNodeTopY = childNodeTopY + 0.75;
                        node.ContainerId = ContainerStage.Key.ToString();
                        diagramNodes.Add(node);

                        //forming edges
                        
                        var predecessor = taskNode.Predecessor.Split(',');
                        Edges edges = new Edges();
                        foreach (var pd in predecessor)
                        {
                            if (!string.IsNullOrEmpty(pd))
                            {
                                edgeIndex = edgeIndex + 1;
                                if(masterListTask.Where(x => x.itemOrder == Convert.ToInt32(pd))!= null && masterListTask.Where(x => x.itemOrder == Convert.ToInt32(pd)).Count() > 0)
                                {
                                    TaskNode predNode = masterListTask.Where(x => x.itemOrder == Convert.ToInt32(pd)).FirstOrDefault();
                                    edges.id = edgeIndex.ToString();
                                    edges.from = taskNode.Id;
                                    edges.to = predNode.Id;
                                    edges.fromShapePoint = edgeIndex;
                                    lstEdges.Add(edges);
                                }
                               
                            }
                            
                        }


                    }

                    childNodeLeftX = childNodeLeftX + 4;
                }


            var data = new
            {
                Nodes = diagramNodes,
                Edges = lstEdges
            };

            var response = Request.CreateResponse(HttpStatusCode.OK,  data);
            return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                //Util.Log.ULog.WriteException(ex);
                Util.Log.ULog.WriteException($"An Exception Occurred in GetServiceTaskFlowData: " + ex);
                return null;
            }
        }

        
    }


    
}