using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    class TextColorItemTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            TreeViewNodeTemplateContainer nodeContainer = container as TreeViewNodeTemplateContainer;
            LiteralControl lControl = new LiteralControl(nodeContainer.Node.Text);
            lControl.ID = lControl.ClientID;
            container.Controls.Add(lControl);
        }
    }

    public partial class ProjectTasks : UserControl
    {
        public string PMMID;
        public string FrameId { get; set; }

        protected string projectTask = string.Empty;
        protected string ajaxHelper;

        public bool IsReadOnly { get; set; }
        public bool ShowBaseline { get; set; }
        public bool ShowTaskTemplate { get; set; }
        public bool ShowTitleOnly { get; set; }

        public double BaselineNum { get; set; }

        private ApplicationContext _context = null;

        protected ApplicationContext ApplicationContext
        {

            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ajaxHelper = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");//string.Empty;

            DataTable data = null;
            if (ShowTaskTemplate)
            {
                pTreeView.GroupingText = "Template Tasks";
                //Get task template and create tree view base on it.
                UGITTaskManager taskHelper = new UGITTaskManager(ApplicationContext, DatabaseObjects.Tables.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
                //UGITTaskHelper helper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Lists.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
                List<UGITTask> tasks = taskHelper.LoadByProjectID(Convert.ToString(PMMID));
                data = UGITUtility.ToDataTable<UGITTask>(tasks);//UGITTaskHelper.FillTaskTable(tasks);

                //List<UGITTask> tasks = LoadByProjectID(moduleName, projectPublicID);
                //taskTable = UGITUtility.ToDataTable<UGITTask>(tasks);
                LoadTree(data, ShowTitleOnly);

            }

            //else if (BaselineNum > 0)
            //{
            //    data = UGITTaskHelper.LoadBaselineTableByProjectId(SPContext.Current.Web, "PMM", PMMID, BaselineNum);
            //    LoadTree(data, ShowTitleOnly);
            //    treeView.DataSource = data;
            //}

            //else
            //{
            //    data = TaskCache.GetAllTasksByProjectID("PMM", PMMID);
            //    LoadTree(data, ShowTitleOnly);
            //}

            if (treeView.Nodes.Count == 0)
            {
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Visible = false;
            }
        }

        public void LoadTree(DataTable pmmTasks, bool showOnlyTitle)
        {
            if (pmmTasks == null || pmmTasks.Rows.Count <= 0)
                return;

            DataRow[] rows = pmmTasks.AsEnumerable().Where(p => p.Field<string>(DatabaseObjects.Columns.ParentTaskID) == "0").ToArray();
            treeView.Nodes.Add("Tasks");
            treeView.Nodes[0].Expanded = true;
            treeView.Nodes[0].Image.Url = "~/Content/Images/TreeView/FOLDER.GIF";//check the location
            
           // treeView.Nodes[0].Image.Url = "/layouts/images/ugovernIT/DocumentLibraryManagement/FOLDER.GIF";//Add image 
            if (rows.Count() > 0)
            {
                for (int i = 0; i < rows.Count(); i++)
                {
                    CreateNode(rows[i], pmmTasks, i, showOnlyTitle, treeView.Nodes[0]);
                }
            }
        }

        private string CreateNode(DataRow row, DataTable pmmTasks, int count, bool showOnlyTitle, TreeViewNode parentNode)
        {

            string title = row[DatabaseObjects.Columns.Title] != null ? Convert.ToString(row[DatabaseObjects.Columns.Title]) : string.Empty;
            title = uHelper.ReplaceInvalidCharsInURL(title);
            DateTime dueDate = DateTime.MaxValue;
            if (!(row[DatabaseObjects.Columns.DueDate] is DBNull))
                Convert.ToDateTime(row[DatabaseObjects.Columns.DueDate]);
            string background = string.Empty;
            if (dueDate.Date < DateTime.Now.Date && Convert.ToString(row[DatabaseObjects.Columns.Status]) != Constants.Completed)
            {
                background = "style=background:#FFAAAA;";
            }
            string type = "Task";
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour) && Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]) != string.Empty)
            {
                type = Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]);
            }

            string treeTitle = string.Empty;
            string fontColor = type == "Milestone" ? "color: blue;" : string.Empty;
            if (showOnlyTitle)
                treeTitle = string.Format("<span style='margin-left:3px;font-size: 8pt;{1}'><b>{0}</b></span>", title, fontColor);
            else
                treeTitle = string.Format("<span style='margin-left:3px;font-size:8pt;{4}'><b>{0}</b> - <b>{1:MMM-d-yyyy}</b> to <b {3}>{2:MMM-d-yyyy}</b></span>", title, row[DatabaseObjects.Columns.StartDate], row[DatabaseObjects.Columns.DueDate], background, fontColor);


            double id = -1;
            double.TryParse(row[DatabaseObjects.Columns.Id].ToString(), out id);

            TreeViewNode tvNode = new TreeViewNode(treeTitle, id.ToString());
            string imgUrl = "/layouts/images/ittask.png";//Add image
            switch (type)
            {
                case "Milestone":
                   // imgUrl = "/layouts/15/images/ugovernit/milestone_icon.png";
                    imgUrl = "~/Content/Images/milestone_icon.png";
                    tvNode.TextStyle.ForeColor = System.Drawing.Color.Blue;
                    break;
                case "Deliverable":
                    imgUrl = "~/Content/Images/document_down.png";
                    break;
                case "Receivable":
                    imgUrl = "~/Content/Images/document_up.png";

                    break;
                case "Ticket":
                    imgUrl = "/_layouts/15/images/ittask.png";//Add image
                    break;
            }

            tvNode.TextStyle.Font.Bold = true;
            tvNode.Image.Url = imgUrl;
            tvNode.TextTemplate = new TextColorItemTemplate();
            DataRow[] rows = pmmTasks.AsEnumerable().Where(p => p.Field<string>(DatabaseObjects.Columns.ParentTaskID) == id.ToString()).ToArray();
            if (rows.Count() > 0)
            {
                parentNode.Nodes.Add(tvNode);
                for (int i = 0; i < rows.Count(); i++)
                {
                    CreateNode(rows[i], pmmTasks, i, showOnlyTitle, parentNode.Nodes.FirstOrDefault(x => x.Name == id.ToString()));
                }
            }
            else
            {
                if (parentNode != null)
                {
                    if (parentNode.Nodes.Where(x => x.Name == id.ToString()).ToList().Count == 0)
                    {
                        parentNode.Nodes.Add(tvNode);
                    }
                }
            }

            return treeTitle;
        }

    }
}