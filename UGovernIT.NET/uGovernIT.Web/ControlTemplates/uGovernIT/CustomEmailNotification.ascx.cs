using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class CustomEmailNotification : UserControl
    {
        public long CheckListRoleId { get; set; }
        public long CheckListId { get; set; }
        public string ticketId { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        CheckListRolesManager checkListRolesManager = null;
        CheckListsManager checkListsManager = null;
        CheckListTaskStatusManager checkListTaskStatusManager = null;
        CheckListTasksManager checkListTasksManager = null;

        Hashtable htCheckListTasks = new Hashtable();

        protected override void OnInit(EventArgs e)
        {
            checkListRolesManager = new CheckListRolesManager(context);
            checkListsManager = new CheckListsManager(context);
            checkListTaskStatusManager = new CheckListTaskStatusManager(context);
            checkListTasksManager = new CheckListTasksManager(context);

            if (!IsPostBack)
            {
                CheckListRoles splstCheckListRoleItem = checkListRolesManager.LoadByID(CheckListRoleId); //SPListHelper.GetSPListItem(DatabaseObjects.Lists.CheckListRoles, Convert.ToInt32(CheckListRoleId));

                string strNames = string.Empty;

                if (!string.IsNullOrEmpty(Convert.ToString(splstCheckListRoleItem.EmailAddress)))
                {
                    if (Convert.ToString(splstCheckListRoleItem.Type) == "UserField")
                    {
                        /*
                        SPFieldUserValue userLookup = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(splstCheckListRoleItem.EmailAddress));

                        if (userLookup != null)
                        {
                            //UserProfile user = UserProfile.LoadById(userLookup.LookupId);
                            
                            if (user != null)
                            {
                                txtEmailTo.Text = user.Email;
                                strNames = user.Name;
                            }
                        }
                        */

                        var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
                        UserProfile user = manager.FindByEmail(Convert.ToString(splstCheckListRoleItem.EmailAddress));
                        strNames = user.Name;
                    }
                    else if (Convert.ToString(splstCheckListRoleItem.Type) == "Contact")
                    {
                        /*
                        SPListItem splstItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMContact, UGITUtility.StringToInt(splstCheckListRoleItem.EmailAddress));

                        txtEmailTo.Text = Convert.ToString(splstItem[DatabaseObjects.Columns.EmailAddress]);
                        strNames = Convert.ToString(splstItem[DatabaseObjects.Columns.Title]);
                        */

                        DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{DatabaseObjects.Columns.EmailAddress} = '{splstCheckListRoleItem.EmailAddress}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                        txtEmailTo.Text = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.EmailAddress]);
                        strNames = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.Title]);
                    }
                    else
                    {
                        txtEmailTo.Text = Convert.ToString(splstCheckListRoleItem.EmailAddress);
                    }
                }


                //SPListItem splstCheckListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CheckLists, Convert.ToInt32(CheckListId));
                CheckLists splstCheckListItem = checkListsManager.LoadByID(CheckListId);

                txtSubject.Text = string.Format("{0} due items notification", splstCheckListItem.Title);

                StringBuilder emailBody = new StringBuilder();
                emailBody.AppendFormat("Hi {0} <br><br><br>", strNames);
                emailBody.Append("The following items are due: <br><br>");

                /*
                SPQuery query = new SPQuery();
                query.Query = string.Format("<Where><And><Eq><FieldRef Name='{6}'/><Value Type='Text'>{7}</Value></Eq><And><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq><And><Eq><FieldRef Name='{2}' LookupId='TRUE'/><Value Type='Lookup'>{3}</Value></Eq><Eq><FieldRef Name='{4}' LookupId='TRUE'/><Value Type='Lookup'>{5}</Value></Eq></And></And></And></Where>", DatabaseObjects.Columns.TicketId, ticketId, DatabaseObjects.Columns.CheckListLookup, CheckListId, DatabaseObjects.Columns.CheckListRoleLookup, CheckListRoleId, DatabaseObjects.Columns.UGITCheckListTaskStatus, "NC");
                query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.CheckListLookup, DatabaseObjects.Columns.CheckListRoleLookup, DatabaseObjects.Columns.CheckListTaskLookup, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.UGITCheckListTaskStatus);
                query.ViewFieldsOnly = true;
                SPListItemCollection spColCheckListStatus = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CheckListTaskStatus, query);
                */

                List<CheckListTaskStatus> spColCheckListStatus = checkListTaskStatusManager.Load(x => x.TicketId == ticketId && x.CheckListLookup == CheckListId && x.CheckListRoleLookup == CheckListRoleId && x.UGITCheckListTaskStatus == "NC");
                List<CheckListTasks> lstCheckListTasks = checkListTasksManager.Load(x => x.CheckListLookup == CheckListId);

                lstCheckListTasks.ForEach(x => {
                    htCheckListTasks.Add(x.ID, x.Title);
                });

                foreach (CheckListTaskStatus item in spColCheckListStatus)
                {
                    //SPFieldLookupValue splookup = new SPFieldLookupValue(Convert.ToString(item.CheckListTaskLookup));
                    //if (splookup != null)
                    {
                        emailBody.Append(string.Format("{0} <br>", htCheckListTasks[item.CheckListTaskLookup]));
                    }
                }

                emailBody.Append(string.Format("<br><br>Thank you <br>{0} ", HttpContext.Current.CurrentUser().Name));
                htmlEditorTicketEmailBody.Html = emailBody.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmailTo.Text))
            {
                MailMessenger mail = new MailMessenger(context);                
                mail.SendMail(txtEmailTo.Text, txtSubject.Text, txtEmailCC.Text, htmlEditorTicketEmailBody.Html, true);
            }
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}