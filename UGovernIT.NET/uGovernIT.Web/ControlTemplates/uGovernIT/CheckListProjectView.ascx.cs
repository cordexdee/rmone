using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CheckListProjectView : UserControl
    {
        public string PublicTicketID { get; set; }
        public string ModuleName { get; set; }
        //DataTable tempCheckListTable = null;

        public string absoluteUrlCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&IsTemplate=false&isdlg=1&isudlg=1";
        public string absoluteUrlCheckListRole = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&IsTemplate=false&isdlg=1&isudlg=1";
        public string absoluteUrlCheckListTask = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&IsTemplate=false&isdlg=1&isudlg=1";
        public string absoluteUrlImportCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&module={3}&isdlg=1&isudlg=1";
        public string absoluteUrlEmailNotification = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&ticketId={2}&module={3}&isdlg=1&isudlg=1";

        static string controlName = string.Empty;
        bool IsFirstSaveClick = false;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListTaskStatusManager checkListTaskStatusManager = null;

        protected override void OnInit(EventArgs e)
        {
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);

            absoluteUrlCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckList, "addchecklist", "Add CheckList", PublicTicketID));
            absoluteUrlCheckListTask = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckListTask, "addchecklisttask", "Add CheckList Task", PublicTicketID));
            absoluteUrlCheckListRole = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckListRole, "addchecklistrole", "Add CheckList Role", PublicTicketID));
            absoluteUrlImportCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImportCheckList, "importchecklisttemplate", "Import CheckList Template", PublicTicketID, ModuleName));
            absoluteUrlEmailNotification = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEmailNotification, "customemailnotification", "Email Notification", PublicTicketID, ModuleName));

            controlName = uHelper.GetPostBackControlId(this.Page);
            BindCheckList();
            //BindDDLProjectCheckList();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridCheckList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //DataTable dtPrioriyFilter = null;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int index = 0;
                foreach (DataControlFieldCell cell in e.Row.Cells)
                {
                    if (index == 0)
                    {
                        GridView grd = (GridView)sender;
                        Label lblCheckListName = (Label)grd.Parent.FindControl("lblCheckListName");
                        HiddenField hdnCheckListId = (HiddenField)grd.Parent.FindControl("hdnCheckListId");
                                                
                        string divHtml = String.Format("<a class='divcell_0' href='javascript:void();' onclick='OpenEditCheckListPopup({2})' title='{1}'>{0}</a>", UGITUtility.TruncateWithEllipsis(lblCheckListName.Text, 25), lblCheckListName.Text, hdnCheckListId.Value);
                        cell.Text = divHtml;
                        cell.CssClass = "cell_0_0";

                    }
                    else if (index == e.Row.Cells.Count - 1)
                    {
                        cell.CssClass = "addiconheader";
                        cell.Text = Context.Server.HtmlDecode(cell.Text);
                    }
                    else
                    {
                        cell.CssClass = "header";
                        cell.Text = Context.Server.HtmlDecode(cell.Text);
                    }
                    index++;
                }
            }

            else if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;
                GridView grd = (GridView)sender;
                AddControls(e.Row, row.DataView.Count, grd);
            }
        }

        private void AddControls(GridViewRow row, int rowCount, GridView grd)
        {
            try
            {
                int index = 0;
                foreach (DataControlFieldCell cell in row.Cells)
                {
                    if (index == 0)
                    {
                        cell.CssClass = "tdrowhead";
                        cell.Text = Context.Server.HtmlDecode(cell.Text);
                    }
                    if (index > 0 && row.RowIndex == rowCount - 1)
                        continue;
                    if (index > 0 && index < row.Cells.Count - 1)
                    {
                        CheckBox checkbox = new CheckBox();
                        checkbox.ID = "chk_" + row.RowIndex.ToString() + "_" + index.ToString();
                        checkbox.Attributes.Add("onclick", "onCheckboxClick(this)");

                        TextBox txtbox = new TextBox();
                        txtbox.ID = "txtbox_" + row.RowIndex.ToString() + "_" + index.ToString();
                        txtbox.Text = "n/a";
                        txtbox.Width = 32;
                        txtbox.Attributes.Add("onclick", "ontxtboxClick(this)");
                        txtbox.ReadOnly = true;

                        HiddenField hdntxtbox = new HiddenField();
                        hdntxtbox.ID = "hdntxtbox_" + row.RowIndex.ToString() + "_" + index.ToString();

                        string[] strCheckListTask = grd.HeaderRow.Cells[index].Text.Split(new string[] { "name=" }, StringSplitOptions.None);
                        string[] strCheckListTaskId = strCheckListTask[1].Split(new string[] { ">" }, StringSplitOptions.None);

                        string[] strCheckListRole = row.Cells[0].Text.Split(new string[] { "name=" }, StringSplitOptions.None);
                        string[] strCheckListRoleId = strCheckListRole[1].Split(new string[] { "/>" }, StringSplitOptions.None);
                        /*
                        SPQuery queryCheckListTaskStatus = new SPQuery();
                        queryCheckListTaskStatus.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><And><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq><Eq><FieldRef Name='{4}' LookupId='TRUE'/><Value Type='Lookup'>{5}</Value></Eq></And></And></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID, DatabaseObjects.Columns.CheckListRoleLookup, Convert.ToInt32(strCheckListRoleId[0]), DatabaseObjects.Columns.CheckListTaskLookup, Convert.ToInt32(strCheckListTaskId[0]));
                        queryCheckListTaskStatus.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListLookup, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListTaskStatus);
                        queryCheckListTaskStatus.ViewFieldsOnly = true;
                        */
                        string queryCheckListTaskStatus = $"{DatabaseObjects.Columns.TicketId} = '{PublicTicketID}' and {DatabaseObjects.Columns.CheckListRoleLookup} = {Convert.ToInt64(strCheckListRoleId[0])} and {DatabaseObjects.Columns.CheckListTaskLookup} = {Convert.ToInt64(strCheckListTaskId[0])} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                        DataTable spColCheckListStatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskStatus, queryCheckListTaskStatus);

                        HiddenField hdnField = new HiddenField();
                        hdnField.ID = "hdnField_" + row.RowIndex.ToString() + "_" + index.ToString();
                        hdnField.Value = Convert.ToString(spColCheckListStatus.Rows[0][DatabaseObjects.Columns.ID]);


                        if (controlName == "btnSave" && IsFirstSaveClick)
                        {

                            if (Convert.ToString(spColCheckListStatus.Rows[0][DatabaseObjects.Columns.UGITCheckListTaskStatus]) == "C")
                            {
                                checkbox.Checked = true;
                                txtbox.CssClass = "txtNormalBorder";
                                hdntxtbox.Value = "C";

                                txtbox.Style.Add("visibility", "hidden");
                                checkbox.Style.Add("visibility", "visible");
                            }
                            else if (Convert.ToString(spColCheckListStatus.Rows[0][DatabaseObjects.Columns.UGITCheckListTaskStatus]) == "NA")
                            {
                                txtbox.CssClass = "txtBoldBorder";
                                checkbox.Checked = false;
                                hdntxtbox.Value = "NA";

                                txtbox.Style.Add("visibility", "visible");
                                checkbox.Style.Add("visibility", "hidden");
                            }
                            else
                            {
                                checkbox.Checked = false;
                                txtbox.CssClass = "txtNormalBorder";
                                hdntxtbox.Value = "NC";

                                txtbox.Style.Add("visibility", "hidden");
                                checkbox.Style.Add("visibility", "visible");
                            }
                        }
                        else if (controlName != "btnSave")
                        {
                            if (Convert.ToString(spColCheckListStatus.Rows[0][DatabaseObjects.Columns.UGITCheckListTaskStatus]) == "C")
                            {
                                checkbox.Checked = true;
                                txtbox.CssClass = "txtNormalBorder";
                                hdntxtbox.Value = "C";

                                txtbox.Style.Add("visibility", "hidden");
                                checkbox.Style.Add("visibility", "visible");
                            }
                            else if (Convert.ToString(spColCheckListStatus.Rows[0][DatabaseObjects.Columns.UGITCheckListTaskStatus]) == "NA")
                            {
                                txtbox.CssClass = "txtBoldBorder";
                                checkbox.Checked = false;
                                hdntxtbox.Value = "NA";

                                txtbox.Style.Add("visibility", "visible");
                                checkbox.Style.Add("visibility", "hidden");
                            }
                            else
                            {
                                checkbox.Checked = false;
                                txtbox.CssClass = "txtNormalBorder";
                                hdntxtbox.Value = "NC";

                                txtbox.Style.Add("visibility", "hidden");
                                checkbox.Style.Add("visibility", "visible");
                            }
                        }

                        cell.Wrap = false;
                        cell.Controls.Add(checkbox);
                        cell.Controls.Add(txtbox);
                        cell.Controls.Add(hdnField);
                        cell.Controls.Add(hdntxtbox);
                        cell.CssClass = "tdrowdata";

                        //onmouseover='showeditCheckList(this)' onmouseout='hideeditCheckList(this)'
                        cell.Attributes.Add("onmouseover", "showNA(this)");
                        cell.Attributes.Add("onmouseout", "hideNA(this)");
                    }
                    index += 1;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void RptCheckList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnCheckListId = (HiddenField)e.Item.FindControl("hdnCheckListId");
                GridView grdListItem = (GridView)e.Item.FindControl("gridCheckList");

                /*
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID, DatabaseObjects.Columns.CheckListLookup, hdnCheckListId.Value);
                query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/><FieldRef Name='{5}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListLookup, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CheckListTaskStatus);
                query.ViewFieldsOnly = true;
                */
                string query = $"{DatabaseObjects.Columns.TicketId}='{PublicTicketID}' and {DatabaseObjects.Columns.CheckListLookup} = {hdnCheckListId.Value} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable spColCheckListStatus = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskStatus, query);
                
                DataTable tempCheckListTable = new DataTable();
                tempCheckListTable.Columns.Add(DatabaseObjects.Columns.ID, typeof(long));
                tempCheckListTable.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
                tempCheckListTable.Columns.Add(DatabaseObjects.Columns.UGITCheckListTaskStatus, typeof(string));
                tempCheckListTable.Columns.Add(DatabaseObjects.Columns.CheckListRoleLookup, typeof(string));
                tempCheckListTable.Columns.Add(DatabaseObjects.Columns.CheckListTaskLookup, typeof(string));
                tempCheckListTable.Columns.Add("CheckListRoleLookupId", typeof(long));
                tempCheckListTable.Columns.Add("CheckListTaskLookupId", typeof(long));

                if (spColCheckListStatus != null && spColCheckListStatus.Rows.Count > 0)
                {
                    foreach (DataRow item in spColCheckListStatus.Rows)
                    {
                        DataRow newtempSubConTaskRow = tempCheckListTable.NewRow();

                        newtempSubConTaskRow[DatabaseObjects.Columns.Id] = Convert.ToInt32(item[DatabaseObjects.Columns.Id]);
                        newtempSubConTaskRow[DatabaseObjects.Columns.TicketId] = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        newtempSubConTaskRow[DatabaseObjects.Columns.UGITCheckListTaskStatus] = Convert.ToString(item[DatabaseObjects.Columns.UGITCheckListTaskStatus]);

                        //SPFieldLookupValue CheckListRoleLookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.CheckListRoleLookup]));
                        newtempSubConTaskRow[DatabaseObjects.Columns.CheckListRoleLookup] = GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.CheckListRoles, DatabaseObjects.Columns.Title, Convert.ToString(item[DatabaseObjects.Columns.CheckListRoleLookup]), context.TenantID); //CheckListRoleLookup.LookupValue;
                        newtempSubConTaskRow["CheckListRoleLookupId"] = item[DatabaseObjects.Columns.CheckListRoleLookup];

                        //SPFieldLookupValue CheckListTaskLookup = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.CheckListTaskLookup]));
                        newtempSubConTaskRow[DatabaseObjects.Columns.CheckListTaskLookup] = GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.CheckListTasks, DatabaseObjects.Columns.Title, Convert.ToString(item[DatabaseObjects.Columns.CheckListTaskLookup]), context.TenantID); //CheckListTaskLookup.LookupValue;
                        newtempSubConTaskRow["CheckListTaskLookupId"] = item[DatabaseObjects.Columns.CheckListTaskLookup];

                        tempCheckListTable.Rows.Add(newtempSubConTaskRow);
                    }
                }


                DataTable dynamicSubConTask = new DataTable();
                dynamicSubConTask.Columns.Add(" ");

                if (tempCheckListTable.Rows.Count > 0)
                {
                    DataView viewCheckListRole = new DataView(tempCheckListTable);
                    viewCheckListRole.Sort = "CheckListRoleLookup ASC";
                    DataTable dtdistinctCheckListRole = viewCheckListRole.ToTable(true, DatabaseObjects.Columns.CheckListRoleLookup, "CheckListRoleLookupId");

                    DataView viewCheckListTask = new DataView(tempCheckListTable);
                    //  viewSubTask.Sort = "TaskLookup ASC";
                    DataTable dtdistinctCheckListTask = viewCheckListTask.ToTable(true, DatabaseObjects.Columns.CheckListTaskLookup, "CheckListTaskLookupId");


                    for (int s = 0; s < dtdistinctCheckListTask.Rows.Count; s++)
                    {
                        string str = string.Format("<a style='position:relative;' href='javascript:void();' onclick='OpenEditTaskPopup({2},{3})' title='{1}' name={3}>{0}</a>", UGITUtility.TruncateWithEllipsis(dtdistinctCheckListTask.Rows[s][0].ToString(), 34), dtdistinctCheckListTask.Rows[s][0].ToString(), hdnCheckListId.Value, dtdistinctCheckListTask.Rows[s][1].ToString());
                        dynamicSubConTask.Columns.Add(str);
                    }

                    for (int k = 0; k < dtdistinctCheckListRole.Rows.Count; k++)
                    {
                        dynamicSubConTask.Rows.Add();
                        
                        string Emailstr = string.Format("<img id='roleEmail_{0}' style='visibility:hidden;padding-left:2px;float:right;' src='/Content/images/uGovernIT/MailTo16X16.png' onclick='CheckListRoleEmail({0},{1})' name={1} />", hdnCheckListId.Value, dtdistinctCheckListRole.Rows[k][1].ToString());
                        string str = string.Format("<div onmouseover='showActionRole(this)' onmouseout='hideActionRole(this)'><a href='javascript:void();' style='padding-left:0px;font-size:10px;'  onclick='OpenEditRolePopup({3},{4})' title='{1}'>{0}</a>{2}</div>", UGITUtility.TruncateWithEllipsis(dtdistinctCheckListRole.Rows[k][0].ToString(), 20), dtdistinctCheckListRole.Rows[k][0].ToString(), Emailstr, hdnCheckListId.Value, dtdistinctCheckListRole.Rows[k][1].ToString());
                        
                        dynamicSubConTask.Rows[k][0] = str;
                    }
                }

                //add default column.
                string strNewAddTask = string.Format("<img id='newaddtask_{0}' title='Add Task' style='float:left;padding-left:4px;padding-top:2px;' src='/Content/Images/plus-blue.png' onclick='OpenAddTaskPopup({0})' />", hdnCheckListId.Value);
                dynamicSubConTask.Columns.Add(strNewAddTask);

                string strnewAddRole = string.Format("<img id='newaddRole_{0}' title='Add Role' style='float:left;padding-left:4px;padding-top:2px;' src='/Content/Images/plus-blue.png' onclick='OpenAddRolePopup({0})' />", hdnCheckListId.Value);
                DataRow dr = dynamicSubConTask.NewRow();
                dr[0] = strnewAddRole;
                dynamicSubConTask.Rows.Add(dr);

                grdListItem.DataSource = dynamicSubConTask;
                grdListItem.DataBind();

                if (dynamicSubConTask != null && dynamicSubConTask.Columns.Count > 1)
                    grdListItem.Width = dynamicSubConTask.Columns.Count * 100 - 30;
            }
        }

        private void BindCheckList()
        {
            string query = $"{DatabaseObjects.Columns.TicketId} = '{PublicTicketID}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
            DataTable dtCheckListTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckLists, query);
            if (dtCheckListTemplates != null && dtCheckListTemplates.Rows.Count > 0)
            {
                ddlProjectCheckList.DataSource = dtCheckListTemplates;
                ddlProjectCheckList.DataTextField = DatabaseObjects.Columns.Title;
                ddlProjectCheckList.DataValueField = DatabaseObjects.Columns.ID;
                ddlProjectCheckList.DataBind();
                lblInformationMessage.Visible = false;
                lnkbtnSubContractorProcore.Visible = true;
            }
            else
            {
                lnkbtnSubContractorProcore.Visible = false;
                lblInformationMessage.Text = "checklist required.";
                lblInformationMessage.Visible = true;
            }

            RptCheckList.DataSource = dtCheckListTemplates;
            RptCheckList.DataBind();
        }

        protected void lnkbtnSubContractorProcore_Click(object sender, EventArgs e)
        {
            //#region Sync with Procore

            ////Procore URl for token.
            //string ProcoreBaseUrl = ConfigurationVariable.GetValue(ConfigConstants.ProcoreBaseUrl);
            //if (!string.IsNullOrEmpty(ProcoreBaseUrl))
            //{
            // #region Procore Token
            //    string ProcoreAPIToken = string.Empty;
            //    object cacheVal = Context.Cache.Get("ProcoreToken");
            //    if (cacheVal != null)
            //    {
            //        ProcoreAPIToken = cacheVal.ToString();
            //    }
            //    else
            //    {
            //        //Login and Password  of Procore.
            //        string[] credentaildetails = null;
            //        string ProcoreCredentials = ConfigurationVariable.GetValue(ConfigConstants.ProcoreCredentials);
            //        if (!string.IsNullOrEmpty(ProcoreCredentials))
            //        {
            //            string decryptedCredential = uGovernITCrypto.Decrypt(ProcoreCredentials, Constants.UGITAPass);
            //            credentaildetails = decryptedCredential.Split(',');
            //        }

            //        try
            //        {
            //            var url = string.Format("{2}?login={0}&password={1}", credentaildetails[0].Trim(), credentaildetails[1].Trim(), ProcoreBaseUrl.Trim() + "token");

            //            var syncClient = new WebClient();
            //            var content = syncClient.DownloadString(url);
            //            DataContractJsonSerializer serializerToken = new DataContractJsonSerializer(typeof(LoginToken));
            //            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
            //            {
            //                var TokenData = (LoginToken)serializerToken.ReadObject(ms);
            //                ProcoreAPIToken = TokenData.token;
            //                Context.Cache.Add("ProcoreToken", TokenData.token, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.WriteException(ex, "ModuleWebPart- ProcoreAPIToken");
            //            lblInformationMessage.Text = "Unable to connect with Procore";
            //            lblInformationMessage.Visible = true;
            //            return;
            //        }

            //    }
            //    #endregion

            //    if (!string.IsNullOrEmpty(ProcoreAPIToken))
            //    {
            //        SPListItem currentProject = uHelper.GetTicket(PublicTicketID);
            //        if (!string.IsNullOrEmpty(Convert.ToString(currentProject[DatabaseObjects.Columns.ExternalProjectId])))
            //        {
            //            try
            //            {
            //                var apiUrl = string.Format("{2}commitments?token={0}&project_id={1}", ProcoreAPIToken.Trim(), Convert.ToString(currentProject[DatabaseObjects.Columns.ExternalProjectId]), ProcoreBaseUrl.Trim());
            //                // var apiUrl = string.Format("{2}projects/{1}/users?token={0}", ProcoreAPIToken.Trim(), "127848", ProcoreBaseUrl.Trim());

            //                var syncClient = new WebClient();
            //                var content = syncClient.DownloadString(apiUrl);
            //                List<ProjectCommitments> projectCommitmentsData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProjectCommitments>>(content);

            //                if (projectCommitmentsData != null && projectCommitmentsData.Count > 0)
            //                {
            //                    #region Delete SubConTask & SubContractor
            //                    // Delete all entry from SubCon form SubConTask.
            //                    SPQuery query = new SPQuery();
            //                    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID);
            //                    query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.SubContractorLookup, DatabaseObjects.Columns.TaskLookup, DatabaseObjects.Columns.TaskDone, DatabaseObjects.Columns.TicketId);
            //                    SPListItemCollection spColSubConTask = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CRMSubConTasks, query);

            //                    List<int> Ids = new List<int>();
            //                    foreach (SPListItem item in spColSubConTask)
            //                    {
            //                        Ids.Add(Convert.ToInt32(item[DatabaseObjects.Columns.Id]));
            //                    }

            //                    if (Ids.Count > 0)
            //                    {
            //                        RMMSummaryHelper.BatchDeleteListItems(Ids, DatabaseObjects.Lists.CRMSubConTasks, SPContext.Current.Web.Url);
            //                    }

            //                    // Delete all entry from SubCon form SubCon.
            //                    SPQuery subconquery = new SPQuery();
            //                    subconquery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID);
            //                    subconquery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId);
            //                    SPListItemCollection spColSubCon = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CRMSubContractors, subconquery);

            //                    List<int> SubConIds = new List<int>();
            //                    foreach (SPListItem item in spColSubCon)
            //                    {
            //                        SubConIds.Add(Convert.ToInt32(item[DatabaseObjects.Columns.Id]));
            //                    }

            //                    if (SubConIds.Count > 0)
            //                    {
            //                        RMMSummaryHelper.BatchDeleteListItems(SubConIds, DatabaseObjects.Lists.CRMSubContractors, SPContext.Current.Web.Url);
            //                    }
            //                    #endregion


            //                    foreach (ProjectCommitments subconItem in projectCommitmentsData)
            //                    {
            //                        //var commitmentVenderApiUrl = string.Format("{2}commitments/{3}?token={0}&project_id={1}", ProcoreAPIToken.Trim(), "127848", ProcoreBaseUrl.Trim(), subconItem.id);
            //                        var commitmentVenderApiUrl = string.Format("{2}commitments/{3}?token={0}&project_id={1}", ProcoreAPIToken.Trim(), Convert.ToString(currentProject[DatabaseObjects.Columns.ExternalProjectId]), ProcoreBaseUrl.Trim(), subconItem.id);

            //                        var syncSubcontractorClient = new WebClient();
            //                        var SubcontractorContent = syncSubcontractorClient.DownloadString(commitmentVenderApiUrl);
            //                        ShowCommitments projectShowCommitmentsData = Newtonsoft.Json.JsonConvert.DeserializeObject<ShowCommitments>(SubcontractorContent);

            //                        if (projectShowCommitmentsData != null)
            //                        {

            //                            if (projectShowCommitmentsData.vendor != null && !string.IsNullOrEmpty(projectShowCommitmentsData.vendor.company))
            //                            {

            //                                SPList lstSubContractor = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMSubContractors);
            //                                SPListItem subContractorItem = lstSubContractor.Items.Add();
            //                                subContractorItem[DatabaseObjects.Columns.TicketId] = PublicTicketID;
            //                                subContractorItem[DatabaseObjects.Columns.SubContractorName] = projectShowCommitmentsData.vendor.company;
            //                                subContractorItem[DatabaseObjects.Columns.ExternalSubcontractorId] = projectShowCommitmentsData.vendor.id;
            //                                subContractorItem[DatabaseObjects.Columns.EmailAddress] = projectShowCommitmentsData.vendor.email_address;
            //                                subContractorItem.Update();

            //                                //SPList lstSubConTask = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMSubConTaskTemplate);
            //                                SPQuery newquery = new SPQuery();
            //                                newquery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID);
            //                                DataTable dttask = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMSubConTaskTemplate, newquery);
            //                                if (dttask != null && dttask.Rows.Count > 0)
            //                                {
            //                                    SPList lstCRMSubConTasks = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMSubConTasks);
            //                                    foreach (DataRow taskrowitem in dttask.Rows)
            //                                    {
            //                                        SPListItem subConTasksItem = lstCRMSubConTasks.Items.Add();
            //                                        subConTasksItem[DatabaseObjects.Columns.TicketId] = PublicTicketID;
            //                                        subConTasksItem[DatabaseObjects.Columns.TaskDone] = "NC";
            //                                        subConTasksItem[DatabaseObjects.Columns.SubContractorLookup] = subContractorItem.ID;
            //                                        subConTasksItem[DatabaseObjects.Columns.TaskLookup] = taskrowitem[DatabaseObjects.Columns.Id];
            //                                        subConTasksItem.Update();
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //            catch (Exception ex)
            //            {
            //                Log.WriteException(ex, "CRMSubConTaskView - GetSubContractor");
            //                lblInformationMessage.Text = "Unable to connect with Procore";
            //                lblInformationMessage.Visible = true;
            //                return;
            //            }

            //        }
            //        //else
            //        //{
            //        //    lnkbtnSyncWithProcore.Visible = false;
            //        //    lblInformationMessage.Text = "Project not found on procore!!";
            //        //    lblInformationMessage.Visible = true;
            //        //}
            //    }
            //}
            //#endregion  

            SyncCheckListWithProcorePopup.ShowOnPageLoad = false;
        }

        private void BindDDLProjectCheckList()
        {
            /*
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID);
            query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.CheckListTemplateLookup);
            query.ViewFieldsOnly = true;
            */

            string query = $"{DatabaseObjects.Columns.TicketId} = '{PublicTicketID}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'";
            //DataTable dtCheckListTemplates = SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckLists, query);
            DataTable dtCheckListTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckLists, query);

            if (dtCheckListTemplates != null && dtCheckListTemplates.Rows.Count > 0)
            {
                ddlProjectCheckList.DataSource = dtCheckListTemplates;
                ddlProjectCheckList.DataTextField = DatabaseObjects.Columns.Title;
                ddlProjectCheckList.DataValueField = DatabaseObjects.Columns.ID;
                ddlProjectCheckList.DataBind();
                lblInformationMessage.Visible = false;
                lnkbtnSubContractorProcore.Visible = true;
            }
            else
            {
                lnkbtnSubContractorProcore.Visible = false;
                lblInformationMessage.Text = "checklist required.";
                lblInformationMessage.Visible = true;
            }
            //ddlProjectCheckList
        }

        protected void btntempsave_Click(object sender, EventArgs e)
        {
            CheckListTaskStatus checkListTaskStatus = new CheckListTaskStatus();
            foreach (RepeaterItem item in RptCheckList.Items)
            {
                GridView grdListItem = (GridView)item.FindControl("gridCheckList");

                //IsFirstSaveClick = true;
                try
                {
                    foreach (GridViewRow row in grdListItem.Rows)
                    {
                        if (row.RowIndex == grdListItem.Rows.Count - 1)
                            continue;

                        int index = 0;
                        foreach (DataControlFieldCell cell in row.Cells)
                        {
                            if (index > 0 && index < row.Cells.Count - 1)
                            {
                                CheckBox chkbox = row.Cells[index].FindControl("chk_" + row.RowIndex.ToString() + "_" + index.ToString()) as CheckBox;
                                TextBox txtbox = row.Cells[index].FindControl("txtbox_" + row.RowIndex.ToString() + "_" + index.ToString()) as TextBox;
                                HiddenField hdnField = row.Cells[index].FindControl("hdnField_" + row.RowIndex.ToString() + "_" + index.ToString()) as HiddenField;
                                HiddenField hdntxtbox = row.Cells[index].FindControl("hdntxtbox_" + row.RowIndex.ToString() + "_" + index.ToString()) as HiddenField;

                                checkListTaskStatus = checkListTaskStatusManager.LoadByID(Convert.ToInt64(hdnField.Value));
                                if (checkListTaskStatus != null)
                                {
                                    checkListTaskStatus.UGITCheckListTaskStatus = hdntxtbox.Value;
                                    checkListTaskStatusManager.Update(checkListTaskStatus);
                                }
                            }
                            index += 1;
                        }
                    }

                }
                catch (Exception ex)
                {
                    //lblMsg.ForeColor = System.Drawing.Color.Red;
                    //lblMsg.Text = ex.ToString();
                    ULog.WriteException(ex);
                }
            }

            BindCheckList();
        }

    }
}