using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ProjectTemplateEdit : UserControl
    {
        public int ItemID { get; set; }
        TaskTemplate item;
        private LifeCycleManager _lifeCycleManager = null;

        protected LifeCycleManager LifeCycleManager
        {
            get
            {
                if (_lifeCycleManager == null)
                {
                    _lifeCycleManager = new LifeCycleManager(HttpContext.Current.GetManagerContext());
                }
                return _lifeCycleManager;
            }
        }

        TaskTemplateManager TaskTemplateMGR = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            item = TaskTemplateMGR.LoadByID(ItemID);   // SPListHelper.GetSPListItem(DatabaseObjects.Lists.UGITTaskTemplates, ItemID);

            if (item != null)
            {
                txtTitle.Text = item.Title;
                txtDescription.Text = Convert.ToString(item.Description);
                if (item.ProjectLifeCycleLookup != null)
                {
                    var lookup = TaskTemplateMGR.Get(item.ProjectLifeCycleLookup);
                    //SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(item.ProjectLifeCycleLookup));
                    if (lookup != null)
                    {
                        var lifeCycleName = LifeCycleManager.Get($"Where ID={lookup.ID}");
                        lbLifeCycle.Text = Convert.ToString(lifeCycleName.Name);
                    }
                }
            }

            ProjectTasks taskCtr = (ProjectTasks)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/ProjectTasks.ascx");
            taskCtr.PMMID = ItemID.ToString();
            taskCtr.ShowTaskTemplate = true;
            taskCtr.ShowTitleOnly = true;
            pTaskTreeView.Controls.Add(taskCtr);

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (item != null && Page.IsValid)
            {
                item.Title = txtTitle.Text.Trim();
                item.Description = txtDescription.Text.Trim();
                TaskTemplateMGR.Update(item);
                uHelper.ClosePopUpAndEndResponse(Context,true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (item != null)
                TaskTemplateMGR.Delete(item);

            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void cvtxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.Title, txtTitle.Text.Trim());
            //DataTable collection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.UGITTaskTemplates, query).GetDataTable();
            DataTable collection = TaskTemplateMGR.GetDataTable();
            DataView dv = new DataView(collection);
            dv.RowFilter = "Title =" + "'"+txtTitle.Text.Trim()+"'";
          
            collection = dv.ToTable();
            if (collection != null && collection.Rows.Count > 0)
            {
                DataRow row = collection.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ID) != Convert.ToString(ItemID));
                if (row != null) 
                {
                    args.IsValid = false;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
    
}
