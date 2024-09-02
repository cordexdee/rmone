using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities.Common;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class ApplicationPassword : UserControl
    {
        public int ApplicationId { get; set; }
        public string TicketID { get; set; }
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&AppId={2}&Mode={3}&ticketID={4}";
        public string addNewItem { get; set; }
        public bool isUserExists { get; set; }
        ApplicationContext context;
        ApplicationPasswordManager appPwdManager;
        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            appPwdManager = new ApplicationPasswordManager(context);
            isUserExists = CheckPermission();
            BindGrid();

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "creatapplicationpassword", "0", ApplicationId, "ADD", TicketID));
            string url = string.Format("window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','600','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Application Password");
            aAddNew.ClientSideEvents.Click = "function(){ " + url + " }";
            //aAddNew.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','600','300',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Application Password"));
            aAddNew.Visible = isUserExists;
            base.OnInit(e);
        }
        protected void Page_Load(EventArgs e)
        {
        }

        protected void grdApplPassword_RowDataBound(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                HtmlGenericControl divActions = grdApplPassword.FindRowCellTemplateControl(e.VisibleIndex, null, "divActions") as HtmlGenericControl;

                if (isUserExists)
                {
                    divActions.Style.Add("display", "block");
                    //e.Row.Attributes.Add("onmouseover", "ShowActionImages(this)");
                    //e.Row.Attributes.Add("onmouseout", "HideActionImages(this)");
                }
                else
                {
                    divActions.Style.Add("display", "none");
                }
            }
        }

        protected void imgDeletePassword_Click(object sender, EventArgs e)
        {
            ImageButton btnDelete = (ImageButton)sender;
            int id = UGITUtility.StringToInt(btnDelete.Attributes["PasswordId"]);
            string userName = Convert.ToString(btnDelete.Attributes["UserName"]);
           string spQuery= string.Format("{0}={1}", DatabaseObjects.Columns.Id, id);
            ApplicationPasswordEntity spListItemColl = appPwdManager.LoadByID(id);
            //DataRow[] spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.ApplicationPassword, spQuery);
            DataRow spItemApplication = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, ApplicationId))[0];
            if (spListItemColl != null)
            {               
                appPwdManager.Delete(spListItemColl);
                string historyDescription = @" Password delted for user " + userName + " application id " + TicketID;
                uHelper.CreateHistory(context.CurrentUser, historyDescription, spItemApplication,context);
                BindGrid();
            }

        }

        private void BindGrid()
        {
            //SPQuery spQuery = new SPQuery();
            string spQuery = string.Format("'{0}'='{1}'", DatabaseObjects.Columns.APPTitleLookup, Convert.ToInt64(ApplicationId));
            DataRow[] spListItemColl = appPwdManager.GetDataTable().Select(spQuery);
            //List<ApplicationPasswordEntity> spListItemColl = appPwdManager.Load().Where(x=>x.APPTitleLookup==ApplicationId).ToList() ;
            if (spListItemColl != null && spListItemColl.Count() > 0)
            {
                DataTable dt = spListItemColl.CopyToDataTable();
                //DataTable dt = new DataTable();
                //DataColumn dcAuthorId = new DataColumn("AuthorId");
                //DataColumn dcUGITDescription = new DataColumn("UGITDescription");
                //DataColumn dcID = new DataColumn("ID");
                //DataColumn dcAPPUserName = new DataColumn("APPUserName");
                //DataColumn dcAPPPasswordTitle = new DataColumn("APPPasswordTitle");
                //dt.Columns.Add(dcAuthorId);
                //dt.Columns.Add(dcUGITDescription);
                //dt.Columns.Add(dcID);
                //dt.Columns.Add(dcAPPUserName);
                //foreach (SPListItem spitem in spListItemColl)
                //{
                //    DataRow dr = dt.NewRow();
                //    SPFieldUserValueCollection spFieldAuthor = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(spitem[DatabaseObjects.Columns.Author]));
                //    if (spFieldAuthor != null && spFieldAuthor.Count > 0)
                //    {
                //        dr["AuthorId"] = spFieldAuthor[0].User.ID;
                //    }
                //    SPFieldUserValueCollection spFieldAPPUserName = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(spitem[DatabaseObjects.Columns.APPUserName]));
                //    if (spFieldAPPUserName != null && spFieldAPPUserName.Count > 0)
                //    {
                //        dr["APPUserName"] = spFieldAPPUserName[0].User.Name;
                //    }
                //    dr["UGITDescription"] = spitem[DatabaseObjects.Columns.UGITDescription];
                //    dr["ID"] = spitem[DatabaseObjects.Columns.Id];
                //    dt.Rows.Add(dr);
                //}
                dt.DefaultView.Sort = DatabaseObjects.Columns.APPUserName + " ASC";
                grdApplPassword.DataSource = dt.DefaultView;
                grdApplPassword.DataBind();
            }
            else
            {
                grdApplPassword.DataSource = null;
                grdApplPassword.DataBind();
            }
        }

        private bool CheckPermission()
        {
            string currentUserID = HttpContext.Current.CurrentUser().Id;
            bool isExists = false;
            if (ApplicationId > 0)
            {
                // SPListItem spItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Applications, ApplicationId);
                DataRow spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, ApplicationId))[0];
                isExists = Ticket.IsActionUser(context, spItem, HttpContext.Current.CurrentUser());

                //if (spItem != null)
                //{
                //    SPFieldUserValueCollection Owners = (SPFieldUserValueCollection)spItem[DatabaseObjects.Columns.TicketOwner];
                //    SPFieldUserValueCollection Admins = (SPFieldUserValueCollection)spItem[DatabaseObjects.Columns.AccessAdmin];
                //    if (Owners != null && Owners.Count>0)
                //    {
                //        foreach (SPFieldUserValue spFieldUserValue in Owners)
                //        {
                //           // SPFieldUserValue spFieldUserValue = Owners[0];
                //            SPFieldUserValue userLookup = new SPFieldUserValue();

                //            if (currentUserID == spFieldUserValue.User.ID)
                //            {
                //                isExists = true;
                //            }
                //        }
                //    }
                //    if (Admins != null && Admins.Count>0)
                //    {
                //        foreach (SPFieldUserValue spFieldUserValue in Admins)
                //        {
                //            // SPFieldUserValue spFieldUserValue = Owners[0];
                //            SPFieldUserValue userLookup = new SPFieldUserValue();

                //            if (currentUserID == spFieldUserValue.User.ID)
                //            {
                //                isExists = true;
                //            }
                //        }
                //    }
                //}
            }
            return isExists;
        }
     
    }

}
