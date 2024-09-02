using DevExpress.Web;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Web
{
    public partial class TaskPredecessorsControl : UserControl
    {

        #region Local variables
        public string SelectedPredecessorsText
        {
            set
            {
               hdnPred.Value= value;
            }
            get { return hdnPred.Value; }
        }
        public List<string> SelectedPredecessorsId
        {
            get
            {
                List<string> selectedid = new List<string>();
                foreach (var item in hdnPredIds.Value.Split(';'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        selectedid.Add(item.Trim());
                    }
                }
                return selectedid;
            }
            set {
                hdnPredIds.Value = string.Join(";", value.ConvertAll<string>(delegate(string i) {return i; }).ToArray()) + ";";
            }
        }
        public string ProjectID { get; set; }
        public string ModuleName { get; set; }
        public int TaskID { get; set; }
        List<UGITTask> _HideTasksInPred = new List<UGITTask>();
        List<UGITTask> _HideModuleDepInPred = new List<UGITTask>();
        public PredecessorType PredecessorMode { get; set; }
        public List<UGITTask> ServiceTasks { get; set; }
        public List<UGITTask> moduleDepncies { get; set; }
        UGITTaskManager TaskManager;
        #endregion

        #region Common Code
        protected override void OnInit(EventArgs e)
        {
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            if (PredecessorMode == PredecessorType.Task)
                BindTask();
            else if (PredecessorMode == PredecessorType.ModuleDependency)
                BindModuleDependency();
            else if (PredecessorMode == PredecessorType.ServiceTask)
                BindServiceTask();

            base.OnInit(e);
        }

        public void SelectAllPredecessor(string Predecessors)
        {

            SelectedPredecessorsId = new List<string>(UGITUtility.SplitString(Predecessors, Constants.Separator6)); //Predecessors.Select(x => x.LookupId).ToList();
            string selectedpredText = string.Empty;
            foreach (var pred in SelectedPredecessorsId)
            {

                TreeViewNode tvnode = tvTasks.Nodes.FindByName(pred.ToString());
                if (tvnode != null)
                {
                    tvnode.Checked = true;

                    if (selectedpredText != string.Empty)
                        selectedpredText += "<br />";

                    //selectedpredText = selectedpredText + tvnode.Text + ";";
                    selectedpredText += tvnode.Text;
                }
            }
            SelectedPredecessorsText = selectedpredText;
        }
        #endregion

        #region Tasks list Predecessor
        private void BindTask()
        {
            
            var tasks = TaskManager.LoadByProjectID(ModuleName, ProjectID );
            tasks = UGITTaskManager.MapRelationalObjects(tasks);
            if (TaskID > 0)
            {
                var currentTask = tasks.FirstOrDefault(x => x.ID == TaskID);
                var parentTaskID = currentTask.ParentTaskID;


                if (currentTask == null)
                {
                    // For new task, just exclude all parents up the chain
                    currentTask = tasks.FirstOrDefault(x => x.ID == parentTaskID);
                    if (currentTask != null && currentTask.ParentTask != null)
                        UGITTaskManager.GetDependentTasks(currentTask.ParentTask, ref _HideTasksInPred, true);
                }
                else
                {
                    UGITTaskManager.GetDependentTasks(currentTask, ref _HideTasksInPred, false);
                }

                SelectedPredecessorsId.RemoveAll(x => _HideTasksInPred.Exists(y => SelectedPredecessorsId.Contains(Convert.ToString(y.ItemOrder))));
                //tasks = tasks.Where(x => x.ID != currentTask.ID).ToList();
                //SubTaskType condition added to make the condition similar to the ModuleController.cs > GetTaskData.
                var ptasks = tasks.FindAll(x => x.ParentTaskID == 0 && string.IsNullOrEmpty(x.SubTaskType));

                if (ptasks != null && ptasks.Count > 0)
                {
                    TreeViewNode node = new TreeViewNode { Text = "" };
                    AddNodesInTreeView(ptasks, ref node);
                    tvTasks.Nodes.AddRange(node.Nodes);
                }
                else
                {
                    lblmessage.Text = "No valid predecessor tasks";
                    tbPredecessors.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
            }
            else
            {
                var ptasks = tasks.FindAll(x => x.ParentTaskID == 0);

                TreeViewNode node = new TreeViewNode { Text = "" };
                AddNodesInTreeView(ptasks, ref node);
                tvTasks.Nodes.AddRange(node.Nodes);
            }


        }

        void ExpandCurrentTaskInTreeView(TreeViewNode cnode)
        {
            if (cnode.Parent.Text != string.Empty)
            {
                cnode.Expanded = true;
                ExpandCurrentTaskInTreeView(cnode.Parent);
            }
            else
            {
                cnode.Expanded = true;
                //tvTasks.ExpandToNode(cnode);
            }
        }

        void AddNodesInTreeView(List<UGITTask> tasks, ref TreeViewNode node)
        {
            foreach (var task in tasks)
            {
               
                TreeViewNode cnode = new TreeViewNode
                {
                    Text = task.ItemOrder + " - " + UGITUtility.TruncateWithEllipsis(task.Title,50),
                    Name = task.ItemOrder.ToString(),
                    ToolTip = task.Title
                };
                tvTasks.JSProperties.Add(string.Format("cp{0}", cnode.Name), string.Format("{0:MMM-dd-yyyy}", task.DueDate));

                if (SelectedPredecessorsId != null && SelectedPredecessorsId.Contains(Convert.ToString(task.ItemOrder)))
                {
                    cnode.Checked = true;
                }
                if (_HideTasksInPred.Exists(x => x.ItemOrder == task.ItemOrder))
                {
                    cnode.NodeStyle.CssClass = "leftpad100";
                    cnode.AllowCheck = false;
                }

                node.Nodes.Add(cnode);

                if (task.ItemOrder == TaskID)
                {
                    //cnode.AllowCheck = false;
                    ExpandCurrentTaskInTreeView(cnode);
                    cnode.NodeStyle.CssClass = "leftpad100 hightlightcurrentTask";
                }

                if (task.ChildCount > 0)
                {
                    cnode.TextStyle.Font.Bold = true;
                    AddNodesInTreeView(task.ChildTasks, ref cnode);
                }
            }
        }
        #endregion

        #region Module Dependency list Predecessor
        private void BindModuleDependency()
        {
            moduleDepncies = UGITTaskManager.MapRelationalObjects(moduleDepncies);
            if (moduleDepncies.Count > 0)
            {
                if (TaskID > 0)
                {
                    var currentTask = moduleDepncies.FirstOrDefault(x => x.ID == TaskID);
                    var parentTaskID = currentTask.ParentTaskID;


                    if (currentTask == null)
                    {
                        // For new task, just exclude all parents up the chain
                        currentTask = moduleDepncies.FirstOrDefault(x => x.ID == parentTaskID);
                        if (currentTask != null && currentTask.ParentTask != null)
                            UGITTaskManager.GetDependentTasks(currentTask.ParentTask, ref _HideModuleDepInPred, true);
                    }
                    else
                    {
                        UGITTaskManager.GetDependentTasks(currentTask, ref _HideModuleDepInPred, false);
                    }

                    SelectedPredecessorsId.RemoveAll(x => _HideModuleDepInPred.Exists(y => SelectedPredecessorsId.Contains(Convert.ToString(y.ID))));
                    //tasks = tasks.Where(x => x.ID != currentTask.ID).ToList();

                    var ptasks = moduleDepncies.FindAll(x => x.ParentTaskID == 0);

                    if (ptasks != null && ptasks.Count > 0)
                    {
                        TreeViewNode node = new TreeViewNode { Text = "" };
                        AddNodesInTreeView(ptasks, ref node);
                        tvTasks.Nodes.AddRange(node.Nodes);
                    }
                    else
                    {
                        lblmessage.Text = "No valid predecessor tasks";
                        tbPredecessors.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
                else
                {
                    var ptasks = moduleDepncies.FindAll(x => x.ParentTaskID == 0);

                    TreeViewNode node = new TreeViewNode { Text = "" };
                    AddNodesInTreeViewForModuleDependency(ptasks, ref node);
                    tvTasks.Nodes.AddRange(node.Nodes);
                }
            }
        }

        void AddNodesInTreeViewForModuleDependency(List<UGITTask> tasks, ref TreeViewNode node)
        {
            foreach (var task in tasks)
            {

                TreeViewNode cnode = new TreeViewNode
                {
                    Text = task.ItemOrder + " - " + task.Title,
                    Name = task.ItemOrder.ToString(),
                };

                if (SelectedPredecessorsId != null && SelectedPredecessorsId.Contains(Convert.ToString(task.ItemOrder)))
                {
                    cnode.Checked = true;
                }

                if (_HideModuleDepInPred.Exists(x => x.ID == task.ItemOrder))
                {
                    cnode.NodeStyle.CssClass = "leftpad100";
                    cnode.AllowCheck = false;
                }

                node.Nodes.Add(cnode);

                if (task.ItemOrder == TaskID)
                {
                    cnode.AllowCheck = false;
                    ExpandCurrentTaskInTreeView(cnode);
                    cnode.NodeStyle.CssClass = "leftpad100 hightlightcurrentTask";
                }

                if (task.ChildCount > 0)
                {
                    cnode.TextStyle.Font.Bold = true;
                    AddNodesInTreeView(task.ChildTasks, ref cnode);
                }
            }
        }
        #endregion 

        #region Service Task list Predecessor
        private void BindServiceTask()
        {
            TreeViewNode node = new TreeViewNode { Text = "" };
            AddNodesInTreeViewForServiceTask(ServiceTasks, ref node);
            tvTasks.Nodes.AddRange(node.Nodes);
        }

        void AddNodesInTreeViewForServiceTask(List<UGITTask> tasks, ref TreeViewNode node)
        {
            foreach (var task in tasks)
            {
                TreeViewNode cnode = new TreeViewNode
                {
                    Text = task.ItemOrder + " - " + task.Title,
                    Name = task.ItemOrder.ToString(),
                };

                if (SelectedPredecessorsId != null && SelectedPredecessorsId.Contains(Convert.ToString(task.ItemOrder)))
                {
                    cnode.Checked = true;
                }

                node.Nodes.Add(cnode);

                if (task.ItemOrder == TaskID)
                {
                    cnode.AllowCheck = false;
                    ExpandCurrentTaskInTreeView(cnode);
                    cnode.NodeStyle.CssClass = "leftpad100 hightlightcurrentTask";
                }
            }
        }
        #endregion 

    }

    public enum PredecessorType
    {
        Task,
        ModuleDependency,
        ServiceTask
    }
}
