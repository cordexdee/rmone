using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace uGovernIT.Web
{
    public partial class uGovernITMessageBoardUserControl : UserControl
    {
        public string Width { get; set; }
        public string Height { get; set; }
        public bool DisplayOnDashboard { get; set; }
        public string Body { get; set; }
        public DateTime Expires { get; set; }
        public string BorderStyle { get; set; }
        public string overallmessagebody { get; set; }
        protected string editMessagePath = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/uGovernITConfiguration.aspx?control=messageboard&mode=config");
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configManager = null;
        MessageBoardManager objMessageBoardManager;

        protected void Page_Load(object sender, EventArgs e)
        {        
            configManager = new ConfigurationVariableManager(context);
            objMessageBoardManager = new MessageBoardManager(HttpContext.Current.GetManagerContext());

            // Allow edit access to super-admins and to members of MessageBoardAdmins group
            UserProfile currentUser =HttpContext.Current.CurrentUser();
            if (context.UserManager.IsUGITSuperAdmin(currentUser) || context.UserManager.CheckUserIsInGroup(configManager.GetValue(ConfigConstants.MessageBoardAdmins), currentUser))
                imgEditMessage.Visible = true;
            else
                imgEditMessage.Visible = false;

            DataTable tempResultedTable = new DataTable();
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.Title);
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.MessageType);
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.Body);
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.Expires);
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.NavigationUrl);
            tempResultedTable.Columns.Add(DatabaseObjects.Columns.TicketId);

            hndborderStyle.Value = BorderStyle;
            hndDisplayOnDashboard.Value = DisplayOnDashboard.ToString();

            List<MessageBoard> MessageBoardList = objMessageBoardManager.Load();

            DataTable messageList = UGITUtility.ToDataTable(MessageBoardList);

            string overallStatus = Constants.MessageTypeValues.Ok;
            DataRow[] _SPListItem = UGITUtility.ToDataTable(MessageBoardList).Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.Title, Constants.OverallStatusKey));
            if (_SPListItem != null && _SPListItem.Length > 0)
                overallStatus = Convert.ToString(UGITUtility.GetSPItemValue(_SPListItem[0], DatabaseObjects.Columns.MessageType));

            if (string.IsNullOrEmpty(overallStatus))
                overallStatus = Constants.MessageTypeValues.Ok;

            //string messageQuery  = string.Format("<Where><And><Neq><FieldRef Name='{4}' /><Value Type='Text'>{5}</Value></Neq><And><Or><Geq><FieldRef Name='{0}' /><Value Type='DateTime'>{1}</Value></Geq><IsNull><FieldRef Name='{0}' /></IsNull></Or><Or><Or><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='User'>{3}</Value></Eq><Membership Type='CurrentUserGroups'><FieldRef Name='{2}' /></Membership></Or><IsNull><FieldRef Name='{2}' /></IsNull></Or></And></And></Where>",
            //                                     DatabaseObjects.Columns.Expires, SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now), DatabaseObjects.Columns.AuthorizedToView, SPContext.Current.Web.CurrentUser.ID, DatabaseObjects.Columns.Title, Constants.OverallStatusKey);
           // string messageQuery = string.Format("{4}<>'{5}' and (({0}>={1} or {0}='') or ({2}='{3}' or {2}=''))", DatabaseObjects.Columns.Expires, DateTime.UtcNow, DatabaseObjects.Columns.AuthorizedToView, HttpContext.Current.CurrentUser().Id, DatabaseObjects.Columns.Title, Constants.OverallStatusKey);
            //Need to check datatime compare, Removing now datetime filter element from filter string
            string messageQuery = string.Format("{4}<>'{5}' and ({0}>='{1}' or {0} is null) and (({2}='{3}' or {2}=''))", DatabaseObjects.Columns.Expires, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), DatabaseObjects.Columns.AuthorizedToView, HttpContext.Current.CurrentUser().Id, DatabaseObjects.Columns.Title, Constants.OverallStatusKey);

            DataRow[] messageItems = messageList.Select(messageQuery);

            if (messageItems.Count() > 0)
                tempResultedTable = messageItems.CopyToDataTable();

            if (tempResultedTable.Rows.Count == 0)
            {
                if (configManager.GetValueAsBool(Constants.MessageBoardHideIfEmpty))
                {
                    dvMessageBoard.Visible = false;
                    dvMessageBoardHeader.Visible = false;
                    return;
                }
                else
                {
                    DataRow row = tempResultedTable.NewRow();
                    row[DatabaseObjects.Columns.MessageType] = overallStatus;
                    row[DatabaseObjects.Columns.Title] = "All systems operating normally";
                    row[DatabaseObjects.Columns.Body] = row[DatabaseObjects.Columns.Title];
                    tempResultedTable.Rows.Add(row);
                }
            }

            if (!tempResultedTable.Columns.Contains("SortingOrder"))
                tempResultedTable.Columns.Add("SortingOrder", typeof(int), "IIF(MessageType = 'Critical',1,IIF(MessageType = 'Warning',2,IIF(MessageType = 'Information',3,4)))"); //IIF(MessageType = 'Warning',2,IFF(MessageType = 'Information',3,4))

            tempResultedTable.DefaultView.Sort = "SortingOrder ASC";
            tempResultedTable = tempResultedTable.DefaultView.ToTable();
            MyMessageRepeater.DataSource = tempResultedTable;
            MyMessageRepeater.DataBind();
            ulMessageboard.Style.Add("width", "780px");
            imgMessage.Visible = true;

            if (DisplayOnDashboard)
            {
                dvMessageBoardHeader.Visible = false;
                overallStatus = Constants.MessageTypeValues.None;
            }

            if (this.Width != null)
                this.Width = this.Width.Replace("px", string.Empty);
            int controlWidth = UGITUtility.StringToInt(this.Width, 780);

            switch (overallStatus)
            {
                case Constants.MessageTypeValues.Ok:
                    imgMessage.Src = "/content/Images/overallstatus_good.png";
                    break;
                case Constants.MessageTypeValues.Information:
                    imgMessage.Src = "/content/Images/overallstatus_info.png";
                    break;
                case Constants.MessageTypeValues.Warning:
                    imgMessage.Src = "/content/Images/overallstatus_warning.png";
                    break;
                case Constants.MessageTypeValues.Critical:
                    imgMessage.Src = "/content/Images/overallstatus_critical.png";
                    break;
                case Constants.MessageTypeValues.None:
                    imgMessage.Visible = false;
                    if (!DisplayOnDashboard)
                        controlWidth += 74; // Increase width since we are not showing icon on left
                    break;
                default:
                    imgMessage.Src = "/content/Images/overallstatus_good.png";
                    break;
            }
            hdnimgFooter.Value = imgMessage.Src;
            string overallmessagebody = string.Empty;
            if (tempResultedTable != null && tempResultedTable.Rows.Count > 0)
            {
                hdnDispayFooterMsg.Value = Convert.ToString(tempResultedTable.Rows[0][DatabaseObjects.Columns.Body]).Replace("'", "\\'");
            }

            ulMessageboard.Style.Add("width", string.Format("{0}px", controlWidth));
            if (!string.IsNullOrEmpty(this.Height))
                ulMessageboard.Style.Add("height", this.Height);
            ulMessageboard.Style.Add("overflow-y", "auto");
        }

        protected void MyMessageRepeater_OnDataBinding(object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                HiddenField messageType = e.Item.FindControl("MessageType") as HiddenField;
                HiddenField navigationUrl = e.Item.FindControl("hdnNavigationUrl") as HiddenField;
                HiddenField ticketId = e.Item.FindControl("hdnTicketId") as HiddenField;
                HiddenField title = e.Item.FindControl("hdnTitle") as HiddenField;

                switch (messageType.Value)
                {
                    case Constants.MessageTypeValues.Ok:
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("class", "goodli");
                        break;
                    case Constants.MessageTypeValues.Information:
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("class", "informationli");
                        break;
                    case Constants.MessageTypeValues.Warning:
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("class", "warningli");
                        break;
                    case Constants.MessageTypeValues.Critical:
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("class", "criticalli");
                        break;
                    case Constants.MessageTypeValues.Reminder:
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("class", "personli");
                        string func = string.Format("javascript:openContactDialog('{0}','{1}','{2}')", navigationUrl.Value, ticketId.Value, title.Value);
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("onclick", func);
                        ((HtmlGenericControl)e.Item.FindControl("liMessage")).Attributes.Add("style", string.Format("cursor:pointer;text-decoration:underline;color:blue;"));
                        break;
                    default:
                        break;
                }
            }

            base.OnDataBinding(e);
        }
    }
}
