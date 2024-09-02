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
    public partial class CheckListTemplatesView : UserControl
    {
        DataTable tempCheckListTemplateTable = null;

        public string absoluteUrlCheckList = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";
        public string absoluteUrlCheckListRole = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";
        public string absoluteUrlCheckListTask = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsTemplate=true&isdlg=1&isudlg=1";

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListTemplatesManager checkListTemplatesManager = null;

        protected override void OnInit(EventArgs e)
        {
            absoluteUrlCheckList = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckList, "addchecklist", "Add CheckList"));
            absoluteUrlCheckListTask = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckListTask, "addchecklisttask", "Add CheckList Task"));
            absoluteUrlCheckListRole = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlCheckListRole, "addchecklistrole", "Add CheckList Role"));

            checkListTemplatesManager = new CheckListTemplatesManager(context);

            BindCheckList();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridCheckList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                        string divHtml = String.Format("<a href='javascript:void();' class='divcell_0' onclick='OpenEditCheckListPopup({2})' title='{1}'>{0}</a>", UGITUtility.TruncateWithEllipsis(lblCheckListName.Text, 40), lblCheckListName.Text, hdnCheckListId.Value);
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
                try
                {
                    int index = 0;
                    foreach (DataControlFieldCell cell in e.Row.Cells)
                    {
                        if (index == 0)
                        {
                            cell.CssClass = "tdrowhead";
                            cell.Text = Context.Server.HtmlDecode(cell.Text);
                        }
                        index += 1;
                    }
                }
                catch (Exception)
                {
                }
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
                query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListTemplateLookup, Convert.ToInt32(hdnCheckListId.Value));
                query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListTemplateLookup);
                query.ViewFieldsOnly = true;
                SPListItemCollection spColCheckListRole = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListRoleTemplates, query);
                */
                string query = $"{DatabaseObjects.Columns.CheckListTemplateLookup} = {Convert.ToInt64(hdnCheckListId.Value)} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable spColCheckListRole = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListRoleTemplates, query);

                /*
                SPQuery query1 = new SPQuery();
                query1.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CheckListTemplateLookup, Convert.ToInt32(hdnCheckListId.Value));
                query1.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CheckListTemplateLookup);
                query1.ViewFieldsOnly = true;
                SPListItemCollection spColCheckListTask = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.CheckListTaskTemplates, query1);
                */

                string query1 = $"{DatabaseObjects.Columns.CheckListTemplateLookup} = {Convert.ToInt32(hdnCheckListId.Value)} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'";
                DataTable spColCheckListTask = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTaskTemplates, query1);

                tempCheckListTemplateTable = new DataTable();
                tempCheckListTemplateTable.Columns.Add(DatabaseObjects.Columns.ID, typeof(long));
                tempCheckListTemplateTable.Columns.Add(DatabaseObjects.Columns.SubContractorLookup, typeof(string));
                tempCheckListTemplateTable.Columns.Add(DatabaseObjects.Columns.TaskLookup, typeof(string));
                tempCheckListTemplateTable.Columns.Add("SubContractorLookupId", typeof(long));
                tempCheckListTemplateTable.Columns.Add("TaskLookupId", typeof(long));

                DataTable dynamicSubConTask = new DataTable();
                dynamicSubConTask.Columns.Add(" ");

                if (spColCheckListTask != null && spColCheckListTask.Rows.Count > 0)
                {
                    DataTable dtCheckListTask = spColCheckListTask; // spColCheckListTask.GetDataTable();

                    for (int s = 0; s < dtCheckListTask.Rows.Count; s++)
                    {
                        string str = string.Format("<a style='position:relative;' href='javascript:void();' onclick='OpenEditTaskPopup({2},{3})' title='{1}'>{0}</a>", UGITUtility.TruncateWithEllipsis(dtCheckListTask.Rows[s]["Title"].ToString(), 34), dtCheckListTask.Rows[s]["Title"].ToString(), hdnCheckListId.Value, dtCheckListTask.Rows[s][0].ToString());
                        dynamicSubConTask.Columns.Add(str);
                    }
                }

                if (spColCheckListRole != null && spColCheckListRole.Rows.Count > 0)
                {
                    DataTable dtCheckListRole = spColCheckListRole; //spColCheckListRole.GetDataTable();

                    for (int k = 0; k < dtCheckListRole.Rows.Count; k++)
                    {
                        dynamicSubConTask.Rows.Add();
                        string str = string.Format("<a style='padding-left:0px;font-size:10px;'  href='javascript:void();' onclick='OpenEditRolePopup({2},{3})'  title='{1}'>{0}</div>", UGITUtility.TruncateWithEllipsis(dtCheckListRole.Rows[k]["Title"].ToString(), 20), dtCheckListRole.Rows[k]["Title"].ToString(), hdnCheckListId.Value, dtCheckListRole.Rows[k][0].ToString());
                        dynamicSubConTask.Rows[k][0] = str;
                    }
                }

                //add default column.
                //string strNewAddTask = string.Format("<img id='newaddtask_{0}' title='Add Task' style='float:left;padding-left:4px;padding-top:2px;' src='/Content/images/uGovernIT/add_icon.png' onclick='OpenAddTaskPopup({0})' />", hdnCheckListId.Value);
                string strNewAddTask = string.Format("<img id='newaddtask_{0}' title='Add Task' style='float:left;padding-left:2px;padding-top:2px;' src='../../Content/Images/plus-blue-new.png' onclick='OpenAddTaskPopup({0})' />", hdnCheckListId.Value);
                dynamicSubConTask.Columns.Add(strNewAddTask);

                //string strnewAddRole = string.Format("<img id='newaddRole_{0}' title='Add Role' style='float:left;padding-left:4px;padding-top:2px;' src='/Content/images/uGovernIT/add_icon.png' onclick='OpenAddRolePopup({0})' />", hdnCheckListId.Value);
                string strnewAddRole = string.Format("<img id='newaddRole_{0}' title='Add Role' style='float:left;padding-left:2px;padding-top:2px;' src='../../Content/Images/plus-blue-new.png' onclick='OpenAddRolePopup({0})' />", hdnCheckListId.Value);
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
            //DataTable dtCheckListTemplates = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CheckListTemplates, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'"); //SPListHelper.GetDataTable(DatabaseObjects.Tables.CheckListTemplates);
            List<CheckListTemplates> dtCheckListTemplates = checkListTemplatesManager.Load();
            if (dtCheckListTemplates != null)
            {
                RptCheckList.DataSource = dtCheckListTemplates;
                RptCheckList.DataBind(); 
            }
        }
    }
}