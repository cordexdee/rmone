using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ModuleStageTaskTemplateList : UserControl
    {
        protected string delegateUrl, addNetItem;
        string moduleName = string.Empty;

        //DataTable userRoles = null;
        List<ModuleUserType> userRoles = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showdelete={2}&module={3}";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            addNetItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetasktemplate");
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["showdelete"] != null)
                {
                    dxShowDeleted.Checked = Convert.ToString(Request["showdelete"]) == "0" ? false : true;
                }
                BindModuleName();
                if (Request["module"] != null)
                {
                    moduleName = Request["module"].ToString();
                    ddlModule.SelectedValue = moduleName.ToUpper();
                    //BindGridView(moduleName.ToUpper());
                }
            }
            if (ddlModule.SelectedValue != "")
            {
                BindGridView(ddlModule.SelectedValue);
            }

            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','moduleName={2}','Exit Criteria - New Item','85','90',0,'{1}')", addNetItem, Server.UrlEncode(Request.Url.AbsoluteUri), ddlModule.SelectedValue));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','moduleName={2}','Exit Criteria - New Item','85','90',0,'{1}')", addNetItem, Server.UrlEncode(Request.Url.AbsoluteUri), ddlModule.SelectedValue));

            base.OnLoad(e);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(ddlModule.SelectedValue);
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("module", ddlModule.SelectedValue);
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        protected void spGrid_Templates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            //{
            //    DataRowView rowView = (DataRowView)e.Row.DataItem;
            //    DataRow row = rowView.Row;
            //    string lsDataKeyValue = spGrid_Templates.DataKeys[e.Row.RowIndex].Value.ToString();
            //    string editItem;
            //    editItem = UGITUtility.GetAbsoluteURL("_layouts/15/ugovernit/uGovernITConfiguration.aspx?control=modulestagetasktemplate&ItemID=" + lsDataKeyValue + " ");
            //    HtmlAnchor anchorEdit = (HtmlAnchor)e.Row.FindControl("aEdit");
            //    HtmlAnchor lnkShowdetail = (HtmlAnchor)e.Row.FindControl("aKeyname");

            //    string title = Convert.ToString(row[DatabaseObjects.Columns.Title]);

            //    if(anchorEdit!=null)
            //    anchorEdit.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','85','85',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath) ,title));
            //    if (lnkShowdetail != null)
            //        lnkShowdetail.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','85','85',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), title));

            //    // Convert role field names into the user-friendly names
            //    if (userRoles != null)
            //    {
            //        Label lblUserRoleType = (Label)e.Row.FindControl("lblUserRoleType");
            //        string userRoleType = Convert.ToString(row[DatabaseObjects.Columns.UserRoleType]);
            //        DataRow userRole = userRoles.AsEnumerable().FirstOrDefault(x => x.Field<string>("Name") == userRoleType);
            //        if (userRole != null)
            //            userRoleType = Convert.ToString(userRole["Role"]);
            //        lblUserRoleType.Text = userRoleType;
            //    }
            //}
        }

        private void BindModuleName()
        {
            ddlModule.Items.Clear();
            ModuleViewManager objModuleViewManager = new ModuleViewManager(context);
            List<UGITModule> lstModule = objModuleViewManager.Load(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();

            if (lstModule != null && lstModule.Count > 0)
            {

                ddlModule.DataSource = lstModule;
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataBind();
            }
        }

        private void BindGridView(string moduleid)
        {
            ModuleStageConstraintTemplatesManager objMSC = new ModuleStageConstraintTemplatesManager(context);
            LifeCycleStageManager lifeCycleStageManager = new LifeCycleStageManager(context);
            userRoles = GetUserRoles();
            bool val = false;
            var itemCollection = objMSC.Load().Where(x => x.ModuleNameLookup == moduleid && x.Deleted == val).ToList();
            if (dxShowDeleted.Checked)
            {
                val = true;
                itemCollection = objMSC.Load().Where(x => x.ModuleNameLookup == moduleid).ToList();
            }

            if (itemCollection != null)
            {
                var listLifeCycle = lifeCycleStageManager.Load();
                LifeCycleStage[] moduleStagesRow = null;
                if (listLifeCycle != null && listLifeCycle.Count > 0)
                {
                    moduleStagesRow = listLifeCycle.Where(x => x.ModuleNameLookup == moduleid).OrderBy(x => x.StageStep).ToArray();
                }
                LifeCycleStage currentStageRow = null;
                int rowCounter = 0;
                if (moduleStagesRow.Count() > 0)
                {
                    foreach (var row in itemCollection)
                    {
                        currentStageRow = moduleStagesRow.FirstOrDefault(x => x.StageStep == row.ModuleStep);
                        if (currentStageRow != null && itemCollection.Count > rowCounter)
                        {
                            itemCollection[rowCounter].ModuleStageName = string.Format("Stage {0}: {1}", row.ModuleStep, currentStageRow.StageTitle);
                            List<string> lstMultiuserValues = new List<string>();


                            string[] multiLookupValue = row.AssignedTo.ToString().Split(',');
                            for (int i = 0; i < multiLookupValue.Length; i++)
                            {
                                string name = uHelper.GetUserNameBasedOnId(multiLookupValue[i]);
                                if (string.IsNullOrEmpty(name))
                                {
                                    name = uHelper.GetRoleNameBasedOnId(multiLookupValue[i]);
                                }
                                lstMultiuserValues.Add(name);
                            }
                            lstMultiuserValues.ToArray();

                            itemCollection[rowCounter].AssignedTo = String.Join(",", lstMultiuserValues);
                        }
                        rowCounter++;
                    }
                }
                // Remove Ids from AssignTo 
                if (itemCollection != null && itemCollection.Count > 0)
                    itemCollection.AsEnumerable().ToList().ForEach(x => x.AssignedTo = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(x.AssignedTo)));
            }

            if (itemCollection != null && itemCollection.Count > 0)
            {
                // spGrid_Templates.AllowGrouping = true;               
                itemCollection = itemCollection.OrderBy(x => x.ModuleStageName).ToList();
                spGrid_Templates.DataSource = itemCollection;
                spGrid_Templates.DataBind();
            }
            else
            {
                spGrid_Templates.DataSource = null;
                spGrid_Templates.DataBind();
            }
        }

        protected void spGrid_Templates_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            ASPxButton aEditButton = spGrid_Templates.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as ASPxButton;
            aEditButton.AutoPostBack = false;
            string aTitle = e.GetValue("Title") as string;
            ASPxButton aEditLinkButton = spGrid_Templates.FindRowCellTemplateControl(e.VisibleIndex, null, "lblTitle") as ASPxButton;
            aEditLinkButton.Text = aTitle;
            aEditLinkButton.AutoPostBack = false;
            string editItem;
            editItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestagetasktemplate&ItemID=" + e.KeyValue + " ");
            string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2}','85','90',0,'{1}')", editItem, Server.UrlEncode(Request.Url.AbsoluteUri), aTitle);
            aEditLinkButton.ClientSideEvents.Click = "function(){ " + jsFunc + " }";
            aEditButton.ClientSideEvents.Click = "function(){ " + jsFunc + " }";
            // Convert role field names into the user-friendly names
            if (userRoles != null)
            {
                Label lblUserRoleType = spGrid_Templates.FindRowCellTemplateControl(e.VisibleIndex, null, "lblUserRoleType") as Label; //(Label)e.Row.FindControl("lblUserRoleType");
                string userRoleType = e.GetValue("UserRoleType") as string; //Convert.ToString(row[DatabaseObjects.Columns.UserRoleType]);
                var userRole = userRoles.FirstOrDefault(x => x.ColumnName == userRoleType);
                if (userRole != null)
                    userRoleType = userRole.UserTypes;
                lblUserRoleType.Text = userRoleType;
            }

        }
        protected void dxShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            string showdelete = dxShowDeleted.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "modulestagetemplates", "Stage Exit Criteria", showdelete, ddlModule.SelectedValue));
            Response.Redirect(url);
        }
        List<ModuleUserType> GetUserRoles()
        {
            ModuleUserTypeManager objModuleUserTypeManager = new ModuleUserTypeManager(context);
            List<ModuleUserType> listUserRoles = new List<ModuleUserType>();

            var list = objModuleUserTypeManager.Load().Where(x => x.ModuleNameLookup == ddlModule.SelectedValue).ToList();
            if (list != null && list.Count() > 0)
            {
                foreach (var userType in list)
                {
                    ModuleUserType moduleUserType = new ModuleUserType();
                    moduleUserType.ID = userType.ID;
                    moduleUserType.ColumnName = userType.ColumnName;
                    moduleUserType.UserTypes = userType.UserTypes;
                    listUserRoles.Add(moduleUserType);
                }
            }
            return listUserRoles;
        }
    }
}
