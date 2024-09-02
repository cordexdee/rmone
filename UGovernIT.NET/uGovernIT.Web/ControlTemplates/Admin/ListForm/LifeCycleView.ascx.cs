using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Web.UI.HtmlControls;
using uGovernIT.Core;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class LifeCycleView : UserControl
    {
        string moduleName = string.Empty;
        private string FormTitle = "Lifecycle";
        private string StageFormTitle = "Lifecycle Stage";

        private string absoluteNewCycleUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/uGovernITConfiguration.aspx?control=lifecyclenew");
        private string absoluteEditCycleUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/uGovernITConfiguration.aspx?control=lifecycleedit");
        private string absoluteNewCycleStageUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/uGovernITConfiguration.aspx?control=lifecyclestagenew");
        private string absoluteEditCycleStageUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/uGovernITConfiguration.aspx?control=lifecyclestageedit");
        List<LifeCycle> lifeCycles = new List<LifeCycle>();
        string selectedLifeCycle = string.Empty;
        LifeCycle selLifeCycle;
        protected bool lifeCycleInUse;
        bool showArchive;
        LifeCycleManager lcHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            btNewLifecycle.NavigateUrl = string.Format("javascript:UgitOpenPopupDialog('{0}','','New {2}','500px','400px',0,'{1}')", absoluteNewCycleUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.FormTitle);
          
            if (Request["lifecycle"] != null)
            {
                selectedLifeCycle = Request["lifecycle"].Trim();
            }

            if (Request["showarchive"] != null)
            {
                showArchive = UGITUtility.StringToBoolean(Request["showarchive"]);
                chkShowDelete.Checked = showArchive;
            }

            if (showArchive)
            {
                lifeCycles = lcHelper.LoadProjectLifeCycles();
            }
            else
            {
                lifeCycles = lcHelper.LoadProjectLifeCycles(false).OrderBy(x=>x.ItemOrder).ToList();
            }

            base.OnInit(e);
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (selectedLifeCycle == string.Empty && lifeCycles.Count > 0)
                selectedLifeCycle = lifeCycles.First(x => x.Name != null).Name;

            SetEditLifeCycle();
            selLifeCycle = lifeCycles.FirstOrDefault(x => x.Name == selectedLifeCycle);

            btnDelete.Visible = false;
            btnArchive.Visible = true;

            btnRemoveArchive.Visible = false;
            btEditLifeCycle.Visible = false;
            btNewStage.Visible = false;

            lbMsg.Text = string.Empty;

            if (selLifeCycle != null)
            {
                btEditLifeCycle.Visible = true;
                btNewStage.Visible = true;

                if (selLifeCycle.Deleted)
                {
                    btnRemoveArchive.Visible = true;
                    btnArchive.Visible = false;
                    btnDelete.Visible = true;

                    TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                    ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

                    UGITModule module = moduleManager.GetByName(ModuleNames.PMM);
                    DataTable pmmListCollection = ticketManager.GetAllTickets(module);
                    //SPQuery query = new SPQuery();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ProjectLifeCycleLookup, selLifeCycle.Name);
                    //SPListItemCollection pmmListCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMProjects, query);
                    if (pmmListCollection != null)
                    {
                        DataRow[] pmmlistwithlifecycle = pmmListCollection.Select(string.Format("{0} = {1}",DatabaseObjects.Columns.ProjectLifeCycleLookup, selLifeCycle.ID));
                        if (pmmlistwithlifecycle != null && pmmlistwithlifecycle.Count() > 0)
                        {
                            lifeCycleInUse = true;
                        }
                    }
                }
                else
                {
                    btnArchive.Visible = true;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            bindGrid();
        }
        protected void _grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            {
                LifeCycleStage stage = (LifeCycleStage)e.Row.DataItem;
                HtmlAnchor anchorEdit = (HtmlAnchor)e.Row.FindControl("aEdit");
                HtmlAnchor lnkStage = (HtmlAnchor)e.Row.FindControl("lnkStage");
           
                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','stageid={3}&lifecycle={4}','{2}','600px','400px',0,'{1}')", absoluteEditCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.StageFormTitle, stage.ID, selectedLifeCycle);
                anchorEdit.Title = "Edit Lifecycle Stage";
                anchorEdit.Attributes.Add("href", jsFunc);
                lnkStage.Attributes.Add("href", jsFunc);
            }
        }

        private void bindGrid()
        {
            rLifeCycle.DataSource = lifeCycles;
            rLifeCycle.DataBind();
           
            if (selLifeCycle != null && selLifeCycle.Stages != null && selLifeCycle.Stages.Count > 0)
            {
                _grid.DataSource = selLifeCycle.Stages;
                _grid.DataBind();

                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/CONTROLTEMPLATES/shared/LifeCycleGUI.ascx");
                ctr2.ModuleLifeCycle = selLifeCycle;
                lcsgraphics.Controls.Add(ctr2);
            }
            else if(selLifeCycle != null && selLifeCycle.Stages != null && selLifeCycle.Stages.Count <= 0) 
            {
                Label message = new Label();
                message.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                message.Text = "Please add stages in selected Lifecycle.";
                message.CssClass = "message-box";
                lcsgraphics.Controls.Add(message);
            }
            else if (selLifeCycle == null)
            {
                Label message = new Label();
                message.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                message.Text = "Lifecycle Not Found.";
                message.CssClass = "message-box";
                lcsgraphics.Controls.Add(message);
            }
        }

        protected void rLifeCycle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btLifeCycle = (LinkButton)e.Item.FindControl("btLifeCycle");
                HtmlTableRow rowLifeCycle = (HtmlTableRow)e.Item.FindControl("trLifeCycle");
                LifeCycle lc = (LifeCycle)e.Item.DataItem;
                if (lc.Deleted)
                {
                    btLifeCycle.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#A53421");
                    btLifeCycle.Style.Add(HtmlTextWriterStyle.Color, "white");
                }
                if (btLifeCycle.Text.ToLower() == selectedLifeCycle.ToLower())
                {
                    rowLifeCycle.Style.Clear();
                    rowLifeCycle.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#99FF99");
                    rowLifeCycle.Style.Add(HtmlTextWriterStyle.Color, "gray");
                }
              
                string jsFunc = string.Format("javascript:event.stopPropagation();UgitOpenPopupDialog('{0}','lifecycle={3}','Edit {2}','400px','300px',0,'{1}');", absoluteEditCycleUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.FormTitle, btLifeCycle.Text);
                btLifeCycle.Attributes.Add("href", "javascript:");
                btLifeCycle.Attributes.Add("onClick", jsFunc);
                
            }
        }

       

        private void SetEditLifeCycle()
        {
            if (!string.IsNullOrEmpty(selectedLifeCycle))
            {
                btEditLifeCycle.Visible = true;
                btEditLifeCycle.NavigateUrl = string.Format("javascript:UgitOpenPopupDialog('{0}','lifecycle={3}','Edit {2}','600px','400px',0,'{1}')", absoluteEditCycleUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.FormTitle, Uri.EscapeUriString(selectedLifeCycle));
                btNewStage.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','lifecycle={3}','{2}','600px','400px',0,'{1}')", absoluteNewCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), "New Lifecycle Stage", Uri.EscapeUriString(selectedLifeCycle)));
               // btNewStage.NavigateUrl = string.Format("javascript:UgitOpenPopupDialog('{0}','lifecycle={3}','{2}','400px','300px',0,'{1}')", absoluteNewCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), "New Lifecycle Stage", Uri.EscapeUriString(selectedLifeCycle));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LifeCycle selLifeCycle = lifeCycles.FirstOrDefault(x => x.Name.ToLower() == selectedLifeCycle.ToLower());

            if (selLifeCycle == null)
            {
                lbMsg.Text = "Lifecycle does not exist.";
                return;
            }

            LifeCycle lifeCycleItem = lcHelper.LoadByID(selLifeCycle.ID); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.ProjectLifeCycles, selLifeCycle.ID, SPContext.Current.Web);
            if (lifeCycleItem == null)
            {
                lbMsg.Text = "Not Stage found.";
                return;
            }

            if (lifeCycleInUse)
            {
                lbMsg.Text = "Lifecycle cannot be deleted because it is already being used in projects.";
            }
            else
            {
                List<LifeCycleStage> stageCollection = selLifeCycle.Stages;
                //SPQuery stageQuery = new SPQuery();
                //stageQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ProjectLifeCycleLookup, lifeCycleItem.ID);
                //SPListItemCollection stageCollection = SPListHelper.GetSPListItemCollection(uGITCache.GetListID(DatabaseObjects.Lists.ProjectLifeCycleStages), stageQuery);
                foreach(LifeCycleStage stage in stageCollection)
                {
                    LifeCycleStageManager stageManager = new LifeCycleStageManager(HttpContext.Current.GetManagerContext());
                    stageManager.Delete(stage);
                }
                Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted life cycle: {selLifeCycle.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                lcHelper.Delete(selLifeCycle);
                //lifeCycleItem.Delete();

                string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                queryCollection.Remove("lifecycle");
                listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
                Context.Response.Redirect(listUrl);
            }
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            LifeCycle selLifeCycle = lifeCycles.FirstOrDefault(x => x.Name == selectedLifeCycle);

            if (selLifeCycle == null)
            {
                lbMsg.Text = "Lifecycle does not exist.";
                return;
            }

            LifeCycle lifeCycleItem = lcHelper.LoadByID(selLifeCycle.ID); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.ProjectLifeCycles, selLifeCycle.ID, SPContext.Current.Web);
            if (lifeCycleItem == null)
            {
                lbMsg.Text = "Stage not found.";
                return;
            }

            lifeCycleItem.Deleted = true;
            lcHelper.Update(lifeCycleItem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Archive life cycle: {lifeCycleItem.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Remove("lifecycle");
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void btnRemoveArchive_Click(object sender, EventArgs e)
        {
            LifeCycle selLifeCycle = lifeCycles.FirstOrDefault(x => x.Name.ToLower() == selectedLifeCycle.ToLower());

            if (selLifeCycle == null)
            {
                lbMsg.Text = "Lifecycle does not exist.";
                return;
            }

            LifeCycle lifeCycleItem = lcHelper.LoadByID(selLifeCycle.ID);   // SPListHelper.GetSPListItem(DatabaseObjects.Lists.ProjectLifeCycles, selLifeCycle.ID, SPContext.Current.Web);
            if (lifeCycleItem == null)
            {
                lbMsg.Text = "Stage not found.";
                return;
            }

            lifeCycleItem.Deleted = false;
            lcHelper.Update(lifeCycleItem);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted life cycle: {lifeCycleItem.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Remove("lifecycle");
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void btLifeCycle_Click(object sender, EventArgs e)
        {
            Button lblifeCycle = (Button)sender;
            string lifecycle=Uri.UnescapeDataString(hdnSelectedLifeCycle.Value);
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("lifecycle", lifecycle);
            queryCollection.Set("showarchive", chkShowDelete.Checked.ToString());
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Response.Redirect(listUrl);
        }

        protected void chkShowDelete_CheckedChanged(object sender, EventArgs e)
        {
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("lifecycle", selectedLifeCycle);
            queryCollection.Set("showarchive", chkShowDelete.Checked.ToString());
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void _grid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if(e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                //LifeCycleStage stage = (LifeCycleStage)e.Row.DataItem;
                HtmlAnchor anchorEdit = (HtmlAnchor)e.Row.FindControlRecursive("aEdit");
                HtmlAnchor lnkStage = (HtmlAnchor)e.Row.FindControlRecursive("lnkStage");

                string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','stageid={3}&lifecycle={4}','{2}','600px','400px',0,'{1}')", absoluteEditCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.StageFormTitle, e.KeyValue, selectedLifeCycle);
                anchorEdit.Title = "Edit Lifecycle Stage";
                anchorEdit.Attributes.Add("href", jsFunc);
                lnkStage.Attributes.Add("href", jsFunc);

            }
        }
    }
}
