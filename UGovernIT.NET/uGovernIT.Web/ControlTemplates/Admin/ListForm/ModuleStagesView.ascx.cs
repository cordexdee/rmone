
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL;
using uGovernIT.Manager.Core;
using System.Threading;
using uGovernIT.Web.Helpers;
using DevExpress.XtraSpreadsheet.API.Native.Implementation;

namespace uGovernIT.Web
{
    public partial class ModuleStagesView : UserControl
    {
        string moduleName = string.Empty;
        string selectedModule = string.Empty;
        string lifeCycleName = string.Empty;

        private string StageFormTitle = "Module Stage";
        private string absoluteEditCycleStageUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulestageaddedit");
        private ApplicationContext _context = null;


        public bool fromAdmin { get; set; }

        
        protected bool lifeCycleInUse;

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context= HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        DataTable moduleList = null;          
        List<LifeCycle> lifeCycles = new List<LifeCycle>();
        LifeCycle selLifeCycle;
       
       // ApplicationContext context = HttpContext.Current.GetManagerContext();

        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        LifeCycleStageManager objLifeCycleStageManager;

        protected override void OnInit(EventArgs e)
        {
            objLifeCycleStageManager = new LifeCycleStageManager(ApplicationContext);

            if (Request["Module"] != null)
            {
                selectedModule = Request["Module"].Trim();
            }
            if(Request["LifeCycle"] != null)
            {
                lifeCycleName = Request["LifeCycle"].Trim();
            }

            DataTable dtModule = ObjModuleViewManager.LoadAllModules();
            if (dtModule!=null && dtModule.Rows.Count > 0)
            {
                //DataTable dtModule = spModuleList.Items.GetDataTable();
                dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                dtModule = dtModule.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, 
                                                                             DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableWorkflow });
                DataRow[] moduleRows = dtModule.Select(string.Format("{0}='True' AND {1}='True'", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableWorkflow));

                moduleList = (moduleRows != null && moduleRows.Length > 0 ? moduleRows.CopyToDataTable() : null);

                cmbModuleName.DataSource = moduleList;
                cmbModuleName.TextField = DatabaseObjects.Columns.ModuleName;
                cmbModuleName.ValueField= DatabaseObjects.Columns.ModuleName;
                cmbModuleName.DataBind();

                // setting default module.
                if (selectedModule == string.Empty)
                    selectedModule = (moduleList != null && moduleList.Rows.Count > 0) ? Convert.ToString(moduleList.Rows[0]["ModuleName"]):string.Empty;

                LifeCycleManager lifecyclehelper = new Manager.LifeCycleManager(ApplicationContext);
                List<LifeCycle> lifeCycleList = lifecyclehelper.LoadLifeCycleByModule(selectedModule);
                if(lifeCycles != null)
                {
                    foreach (LifeCycle item in lifeCycleList)
                    {
                        moduleLifeCycles.Items.Add(item.Name, item.Name);
                        lifeCycleName = item.Name;
                    }
                }
            }

            LifeCycleManager lcHelper = new LifeCycleManager(ApplicationContext);
            lifeCycles = lcHelper.LoadLifeCycleByModule(selectedModule);
            if (string.IsNullOrEmpty(lifeCycleName))
            {
                selLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();
                moduleLifeCycles.SelectedIndex = 0;
               // selLifeCycle =lifeCycles.FirstOrDefault();

            }
            else
            {
                selLifeCycle = lifeCycles.Where(m => m.Name == lifeCycleName).FirstOrDefault();
                moduleLifeCycles.SelectedIndex = moduleLifeCycles.Items.IndexOf(moduleLifeCycles.Items.FindByValue(lifeCycleName));
            }

            lbMsg.Text = string.Empty;
            object abc = Request.Form["__CALLBACKPARAM"];

            if (Request.Form["__CALLBACKPARAM"] != null)
            {
                string[] val = Request.Form["__CALLBACKPARAM"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                //if (Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                //{
                if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DRAGROW"))
                {

                    int source = Convert.ToInt16(val[val.Length - 2]);
                    int target = Convert.ToInt16(val[val.Length - 1]);

                    LifeCycleStage lifeCycleStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == source);

                    foreach (LifeCycleStage lCycle in selLifeCycle.Stages)
                    {
                        if (target > source)
                        {
                            if (target >= lCycle.StageStep && lCycle.StageStep >= source)
                            {
                                if (source <= lCycle.StageStep)
                                {
                                    lCycle.StageStep--;
                                }
                            }
                        }
                        else
                        {
                            if (source >= lCycle.StageStep && lCycle.StageStep >= target)
                            {
                                if (source >= lCycle.StageStep)
                                {
                                    lCycle.StageStep++;
                                }
                            }
                        }
                    }

                    lifeCycleStage.StageStep = target;

                    selLifeCycle.Stages = selLifeCycle.Stages.OrderBy(x => x.StageStep).ToList();


                    //SPList spList = SPListHelper.GetSPList(DatabaseObjects.Tables.ModuleStages);
                    //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
                    //string updateMethodFormat = "<Method ID=\"{0}\">" +
                    // "<SetList>{1}</SetList>" +
                    // "<SetVar Name=\"Cmd\">Save</SetVar>" +
                    // "<SetVar Name=\"ID\">{2}</SetVar>" +
                    // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ModuleStep\">{3}</SetVar>" +
                    // "</Method>";
                    //StringBuilder query = new StringBuilder();
                    //foreach (LifeCycleStage lCycle in selLifeCycle.Stages)
                    //{
                    //    query.AppendFormat(updateMethodFormat, lCycle.ID, spList.ID, lCycle.ID, lCycle.StageStep);
                    //}

                    //string batch = string.Format(batchFormat, query.ToString());
                    //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);

                    #region Delete / Update From RequestRoleWriteAccess
                    DataTable dtRoleWriteAccess = ObjModuleViewManager.LoadModuleListByName(selectedModule, DatabaseObjects.Tables.RequestRoleWriteAccess); //SPListHelper.GetDataTable(DatabaseObjects.Tables.RequestRoleWriteAccess);

                    string expression = string.Empty;
                    DataRow[] roleWriteAccessCollection = dtRoleWriteAccess.Select(); 
                    if (target > source)
                        roleWriteAccessCollection = dtRoleWriteAccess.Select(string.Format("{2} >= {0} And {0} >= {1}", DatabaseObjects.Columns.ModuleStep, source, target));
                            //expression = "{2} >= {0} And {0} >= {1} And {3} = '{4}'";
                    else
                        roleWriteAccessCollection = dtRoleWriteAccess.Select(string.Format("{1} >= {0} And {0} >= {2}", DatabaseObjects.Columns.ModuleStep, source, target));
                    //expression = "{1} >= {0} And {0} >= {2} And {3} = '{4}'";

                    var itemsToUpdate = roleWriteAccessCollection;
                    //var itemsToUpdate = dtRoleWriteAccess.Select(string.Format(expression,
                    //                                             DatabaseObjects.Columns.ModuleStep, source, target,
                    //                                             DatabaseObjects.Columns.ModuleNameLookup, selectedModule));

                    if (itemsToUpdate.ToList().Count > 0)
                    {
                        foreach (DataRow dr in itemsToUpdate)
                        {
                            int moduleStep = Convert.ToInt32(dr[DatabaseObjects.Columns.ModuleStep]);
                            if (target > source)
                            {
                                if (moduleStep == source)
                                    dr[DatabaseObjects.Columns.ModuleStep] = target;
                                else
                                    dr[DatabaseObjects.Columns.ModuleStep] = moduleStep - 1;
                            }
                            else
                            {
                                if (moduleStep == source)
                                    dr[DatabaseObjects.Columns.ModuleStep] = target;
                                else
                                    dr[DatabaseObjects.Columns.ModuleStep] = moduleStep + 1;
                            }
                        }
                        BatchUpdateRequestRoleWriteAccess(DatabaseObjects.Tables.RequestRoleWriteAccess, itemsToUpdate);
                    }

                    #endregion
                    bindGrid();
                }
                else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("CONNECT"))
                {
                    int fromStage = Convert.ToInt16(val[val.Length - 1]);
                    int toStage = Convert.ToInt16(val[val.Length - 2]);
                    string type = Convert.ToString(val[val.Length - 3]);

                    LifeCycleStage lifeCycleStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == fromStage);
                    //DataRow spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.ModuleStages, lifeCycleStage.ID);
                    if (lifeCycleStage != null)
                    {
                        if (type == "approve")
                        {

                            lifeCycleStage.ApprovedStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == toStage);
                            lifeCycleStage.StageApprovedStatus = lifeCycleStage.ApprovedStage.StageStep;
                        }
                        else if (type == "return")
                        {
                            lifeCycleStage.ReturnStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == toStage);
                            lifeCycleStage.StageReturnStatus = lifeCycleStage.ReturnStage.StageStep;

                        }
                        else if (type == "reject")
                        {
                            lifeCycleStage.RejectStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == toStage);
                            lifeCycleStage.StageRejectedStatus = lifeCycleStage.RejectStage.StageStep;

                        }
                        // lcHelper.UpdateLifeCycleStage(lifeCycleStage); //spListItem.Update();
                        objLifeCycleStageManager.Update(lifeCycleStage);
                    }
                    bindGrid();
                }
                else if (Request.Form["__CALLBACKPARAM"].ToString().Contains("DELETE"))
                {
                    int stage = Convert.ToInt16(val[val.Length - 1]);
                    LifeCycleStage currLifeCycleStage = selLifeCycle.Stages.FirstOrDefault(x => x.StageStep == stage);
                    if (currLifeCycleStage != null)
                        // objLifeCycleStageManager.Delete(currLifeCycleStage);
                        //objLifeCycleStageManager.Delete(lifeCycleStage);
                        objLifeCycleStageManager.DeleteLifeCycleStage(selectedModule, currLifeCycleStage.ID);
                    LifeCycleManager lifeCycleHelper = new LifeCycleManager(ApplicationContext);
                    lifeCycles = lifeCycleHelper.LoadLifeCycleByModule(selectedModule);
                    selLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();

                    bindGrid();
                }
                //}
            }


            base.OnInit(e);
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            string lcValue = string.Empty;
            if (moduleLifeCycles.SelectedItem != null)
                lcValue = Convert.ToString(moduleLifeCycles.SelectedItem.Value);

            if (selectedModule == ModuleNames.PMM)
                divAddLifecycle.Visible = true;


            string jsFunc = string.Format("UgitOpenPopupDialog('{0}','stageid={3}&Module={4}&LifeCycle={5}','{2}','95','95',0,'{1}');", absoluteEditCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.StageFormTitle, 0, selectedModule, lcValue);
           // aAddItem.Attributes.Add("OnClick", jsFunc);
             addNewItem.Attributes.Add("onclick", string.Format("UgitOpenPopupDialog('{0}','stageid={3}&Module={4}&LifeCycle={5}','{2}','95','95',0,'{1}');", absoluteEditCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.StageFormTitle, 0, selectedModule, lcValue));
          //  aAddItem.ClientSideEvents.Click = "function(s, e) { " + jsFunc + " }";
            
	    btUpdateTickets.Visible = false;
            if (ApplicationContext.ConfigManager.GetValueAsBool(ConfigConstants.EnableUpdateTicketsOnWorkflowChange))
            {
                btUpdateTickets.Visible = true;
                SetSyncButtonStatus();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            bindGrid();
        }

        protected void _grid_DataBound(object sender, EventArgs e)
        {
            
        }
      
        private void bindGrid()
        {
            //System.Data.DataColumn newColumn = new System.Data.DataColumn(DatabaseObjects.Columns.StageApprovedStatus, typeof(System.String));
            //moduleList.Columns.Add(newColumn);
            //foreach (DataRow item in moduleList.Rows)
            //{
            //    FieldLookupValue lookupVals = new FieldLookupValue(Convert.ToInt16(item[DatabaseObjects.Columns.StageApprovedStatus]), DatabaseObjects.Columns.StageApprovedStatus, DatabaseObjects.Tables.ModuleStages);
            //    item[DatabaseObjects.Columns.DepartmentNameLookup] = lookupVals.Value;
            //}
            rModules.DataSource = moduleList;
            rModules.DataBind();

            BindGraphics();
        }

        private void BindGraphics()
        {
            if (selLifeCycle != null && selLifeCycle.Stages.Count > 0)
            {
                _grid.DataSource = selLifeCycle.Stages;
                _grid.DataBind();

                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/ControlTemplates/Shared/LifeCycleGUI.ascx");
                ctr2.fromAdmin = fromAdmin;
                ctr2.ModuleLifeCycle = selLifeCycle;
                ctr2.Attributes.Add("class", "amar12345");
                lcsgraphics.Controls.Add(ctr2);
            }
            else if (selLifeCycle != null && selLifeCycle.Stages.Count <= 0)
            {
                Label message = new Label();
                message.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                message.Text = "Please add stages in selected Lifecycle.";
                message.CssClass = "message-box";
                lcsgraphics.Controls.Add(message);
                _grid.DataSource = null;
                _grid.DataBind();
            }
            else if (selLifeCycle == null)
            {
                Label message = new Label();
                message.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                message.Text = "Lifecycle Not Found.";
                message.CssClass = "message-box";
                lcsgraphics.Controls.Add(message);
                _grid.DataSource = null;
                _grid.DataBind();
            }
        }

        protected void rModules_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btModuleStages = (LinkButton)e.Item.FindControl("btModuleStages");
                HiddenField hndModuleName = (HiddenField)e.Item.FindControl("hndModuleName");
                if (hndModuleName.Value.ToLower() == selectedModule.ToLower())
                {
                    btModuleStages.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#99FF99");
                    btModuleStages.Style.Add(HtmlTextWriterStyle.Color, "gray");
                }
            }
        }

        protected void btModuleStages_Click(object sender, EventArgs e)
        {
           
            LinkButton lblifeCycle = (LinkButton)sender;
            HiddenField hndModuleName = (HiddenField)lblifeCycle.Parent.FindControl("hndModuleName");
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            queryCollection.Set("Module", hndModuleName.Value);          
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        DataTable GetActionUser()
        {
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("ID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("NameRole");
            dtGroups.Columns.Add("Role");
            dtGroups.Columns.Add("Type");

            ModuleUserTypeManager userTypeManager = new ModuleUserTypeManager(ApplicationContext);
            EnumerableRowCollection<DataRow> rows = userTypeManager.LoadModuleUserTypeDt().AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == selectedModule);
            if (rows != null && rows.Count() > 0)
            {
                DataTable dtUserTypes = rows.CopyToDataTable();
                foreach (DataRow drUserTypes in dtUserTypes.Rows)
                {
                    DataRow dr = dtGroups.NewRow();
                    dr["ID"] = drUserTypes[DatabaseObjects.Columns.Id];
                    dr["Name"] = drUserTypes[DatabaseObjects.Columns.ColumnName];
                    dr["Role"] = drUserTypes[DatabaseObjects.Columns.UserTypes];
                    dr["NameRole"] = string.Format("{0} ({1})", drUserTypes[DatabaseObjects.Columns.UserTypes], drUserTypes[DatabaseObjects.Columns.ColumnName]);
                    dr["Type"] = "Role";
                    dtGroups.Rows.Add(dr);
                }
            }

            UserRoleManager roleManager = new UserRoleManager(ApplicationContext);
            List<Role> sRoles = roleManager.GetRoleList();
            //SPGroupCollection collGroups = SPContext.Current.Web.SiteGroups;
            foreach (Role oGroup in sRoles)
            {
                DataRow dr = dtGroups.NewRow();
                dr["ID"] = oGroup.Id;
                dr["Name"] = oGroup.Name;
                dr["Role"] = oGroup.Name;
                dr["NameRole"] = oGroup.Name;
                dr["Type"] = "Group";
                dtGroups.Rows.Add(dr);
            }

            return dtGroups;
        }

        private void BatchUpdateRequestRoleWriteAccess(string listName, DataRow[] drRow)
        {
            //SPList spList = SPListHelper.GetSPList(listName);
            //StringBuilder spUpdate = new StringBuilder();

            //spUpdate.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><ows:Batch OnError=\"Return\">");
            //string command = "<Method ID=\"{0}\"><SetList Scope=\"Request\">" + spList.ID + "</SetList>" +
            //                 "<SetVar Name=\"{1}ModuleStep\">{2}</SetVar>" + //update Field
            //                 "<SetVar Name=\"ID\">{0}</SetVar><SetVar Name=\"Cmd\">Save</SetVar></Method>";
            //foreach (DataRow dr in drRow)
            //{
            //    spUpdate.AppendFormat(command, dr[DatabaseObjects.Columns.Id], "urn:schemas-microsoft-com:office:office#", dr[DatabaseObjects.Columns.ModuleStep]);
            //}
            //spUpdate.Append("</ows:Batch>");

            //string batchReturn = SPContext.Current.Web.ProcessBatchData(spUpdate.ToString());
        }

        protected void _grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                LifeCycleStage stage = (LifeCycleStage)_grid.GetRow(e.VisibleIndex);
                HtmlAnchor anchorEdit = (HtmlAnchor)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["Edit"], "aEdit");
                HtmlAnchor lnkStage = (HtmlAnchor)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["Title"], "lnkStage");
                string stageid = string.Empty;
                if (stage != null)
                {
                    Label lblApprove = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["StageApprovedStatus"], "lblApprove");
                    Label lblReturn = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["StageReturnStatus"], "lblReturn");
                    Label lblReject = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["StageRejectedStatus"], "lblReject");
                    Label lblActionUser = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["ActionUser"], "lblActionUser");
                    Label lblDataEditor = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["DataEditors"], "lblDataEditor");
                    Label lblStageType = (Label)_grid.FindRowCellTemplateControl(e.VisibleIndex, _grid.DataColumns["StageTypeChoice"], "lblStageType");
                    DataTable dataTable = GetActionUser();
                    string[] users = null;
                    if (stage.ActionUser!=null && stage.ActionUser.IndexOf(Constants.Separator10) != -1)
                        stage.ActionUser = stage.ActionUser.Replace(Constants.Separator10, Constants.Separator);

                    users = UGITUtility.SplitString(stage.ActionUser, Constants.Separator);
                    List<string> actionUsers = new List<string>();
                    foreach (string auStage in users)
                    {
                        DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Name") == auStage.Trim());
                        string groupStr = auStage;
                        if (row == null)
                        {
                            DataRow groupRow = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == auStage.Trim());
                            if (groupRow != null)
                                groupStr = Convert.ToString(groupRow["Role"]);
                        }
                        if (row != null)
                        {
                            actionUsers.Add(Convert.ToString(row["Role"]));
                        }
                        else
                        {
                            actionUsers.Add(groupStr);
                        }
                    }

                    lblActionUser.Text = string.Join("; ", actionUsers.ToArray());

                    if (!string.IsNullOrEmpty(lblActionUser.Text))
                    {
                        lblActionUser.Text = lblActionUser.Text.Replace("#", " ");
                    }

                    #region data editors code
                    users = null;
                    if (stage.DataEditors!=null && stage.DataEditors.IndexOf(Constants.Separator10) != -1)
                        stage.DataEditors = stage.DataEditors.Replace(Constants.Separator10, Constants.Separator);

                    users = UGITUtility.SplitString(stage.DataEditors, Constants.Separator);
                    List<string> dataEditors = new List<string>();
                    foreach (string auStage in users)
                    {
                        DataRow row = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Role" && x.Field<string>("Name") == auStage.Trim());
                        string groupStr = auStage;
                        if (row == null)
                        {
                            DataRow groupRow = dataTable.AsEnumerable().FirstOrDefault(x => x.Field<string>("Type") == "Group" && x.Field<string>("Name") == auStage.Trim());
                            if (groupRow != null)
                                groupStr = Convert.ToString(groupRow["Role"]);
                        }
                        if (row != null)
                        {
                            dataEditors.Add(Convert.ToString(row["Role"]));
                        }
                        else
                        {
                            dataEditors.Add(groupStr);
                        }
                    }

                    lblDataEditor.Text = string.Join("; ", dataEditors.ToArray());

                    if (!string.IsNullOrEmpty(lblDataEditor.Text))
                    {
                        lblDataEditor.Text = lblDataEditor.Text.Replace("#", " ");
                    }

                    #endregion

                    lblApprove.Text = stage.ApprovedStage != null ? Convert.ToString(stage.ApprovedStage.StageStep) : string.Empty;
                    lblReject.Text = stage.RejectStage != null ? Convert.ToString(stage.RejectStage.StageStep) : string.Empty;
                    lblReturn.Text = stage.ReturnStage != null ? Convert.ToString(stage.ReturnStage.StageStep) : string.Empty;
                    if (stage.StageTypeChoice == null || stage.StageTypeChoice=="0" || stage.StageTypeChoice == string.Empty)
                    {
                        lblStageType.Text = "None";
                    }
                    else
                    {
                        lblStageType.Text = stage.StageTypeChoice;
                    }
                    stageid = Convert.ToString(stage.ID);

                    string jsFunc = string.Format("javascript:UgitOpenPopupDialog('{0}','stageid={3}&Module={4}&LifeCycle={5}','{2}','95','95',0,'{1}')", absoluteEditCycleStageUrl, Uri.EscapeUriString(Request.Url.AbsolutePath), this.StageFormTitle, stageid, selectedModule, Convert.ToString(moduleLifeCycles.SelectedItem.Value));
                    anchorEdit.Title = "Edit Module Stage";
                    anchorEdit.Attributes.Add("href", jsFunc);
                    lnkStage.Attributes.Add("href", jsFunc);
                }
            }
        }

        protected void moduleLifeCycles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LifeCycleManager lcHelper = new LifeCycleManager(ApplicationContext);
            lifeCycles = lcHelper.LoadLifeCycleByModule(selectedModule);
            if (moduleLifeCycles.SelectedItem != null)
            {
                string selectedvalue = Convert.ToString(moduleLifeCycles.SelectedItem.Value);
                selLifeCycle = lifeCycles.Where(m => m.Name == (selectedvalue == string.Empty ? null : selectedvalue)).FirstOrDefault();
                lifeCycleName = selectedvalue;
                string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                queryCollection.Set("LifeCycle", lifeCycleName);
                listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
                Context.Response.Redirect(listUrl);
            }
            else
                selLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();
        }

        protected void panelLifeCycle_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            LifeCycle newLifeCycle = new LifeCycle();
            newLifeCycle.Name = txtName.Text;
            newLifeCycle.Description = txtName.Text;
            newLifeCycle.ModuleNameLookup = selectedModule;
            //popupLifeCycle.ShowOnPageLoad = false;
            LifeCycleManager lcHelper = new LifeCycleManager(ApplicationContext);
            // lcHelper.AddNewLifeCycle(newLifeCycle);
            lcHelper.Insert(newLifeCycle);
        }

        protected void _grid_PageIndexChanged(object sender, EventArgs e)
        {
            bindGrid();
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            ModuleViewManager moduleMgr = new ModuleViewManager(ApplicationContext);
            moduleMgr.UpdateCache(selectedModule);
        }
        protected void btUpdateTickets_Click(object sender, EventArgs e)
        {
            if (!SyncTicketWithWorkflowJob.ProcessState())
            {
                ApplicationContext applicationContext = HttpContext.Current.GetManagerContext(); 
                Thread syncThread = new Thread(delegate ()
                {
                    SyncTicketWithWorkflowJob syncTicketWithWorkflowJob = new SyncTicketWithWorkflowJob();
                    syncTicketWithWorkflowJob.Execute(applicationContext, selectedModule);
                });

                syncThread.IsBackground = true;
                syncThread.Start();
            }
            SetSyncButtonStatus();
        }

        private void SetSyncButtonStatus()
        {
            if (SyncTicketWithWorkflowJob.ProcessState())
            {
                // Update already running
                btUpdateTickets.Enabled = false;
                btUpdateTickets.Text = string.Format("Update Tickets Running ({0}%)", SyncTicketWithWorkflowJob.GetProgressPercentage());
            }
            else
            {
                // Update not currently running
                btUpdateTickets.Enabled = true;
                btUpdateTickets.Text = "Update Tickets";
                btUpdateTickets.ToolTip = "Update StageStep in existing tickets based on ModuleStepLookup";
            }

        }
    }
}
