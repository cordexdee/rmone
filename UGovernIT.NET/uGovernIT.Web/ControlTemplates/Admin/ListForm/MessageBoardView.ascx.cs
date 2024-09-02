
using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class MessageBoardView : UserControl
    {
        //private DataTable _SPList;
        //private DataTable _DataTable;
        private string addNewItem = string.Empty;
        private string overallStatus = Constants.MessageTypeValues.Ok;
        List<MessageBoard> MessageBoardList;
        List<MessageBoard> MessageBoardDataList;

        #region constant
        private const string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
        private const string absoluteUrlView = "/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&showExpired={2}&type={3}";
        private const string formTitle = "Message Board";
        private const string viewParam = "messageboard";
        private const string newParam = "messageboardnew";
        private const string editParam = "messageboardedit";
        ConfigurationVariableManager objConfigurationVariableManager;
        MessageBoardManager objMessageBoardManager;
      
        #endregion

        protected override void OnInit(EventArgs e)
        {
            objMessageBoardManager = new MessageBoardManager(HttpContext.Current.GetManagerContext());
            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, "0"));
            aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','415',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','415',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            // _SPList = objMessageBoardManager.GetDataTable();
            MessageBoardList = objMessageBoardManager.Load();
            BindMessageType();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            if (!IsPostBack)
            {
                if (Request["showExpired"] != null && Convert.ToString(Request["showExpired"]).Trim() != string.Empty)
                {
                    chkShowExpired.Checked = Convert.ToString(Request["showExpired"]) == "0" ? false : true;
                }

                if (Request["type"] != null && Convert.ToString(Request["type"]).Trim() != string.Empty)
                {
                    ddlMessageType.Text = Convert.ToString(Request["type"]);
                }

                chkHideIfEmpty.Checked = objConfigurationVariableManager.GetValueAsBool(Constants.MessageBoardHideIfEmpty);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindGrid(ddlMessageType.Text);
            base.OnPreRender(e);
        }

        protected void ddlMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string showExpired = chkShowExpired.Checked ? "1" : "0";
            string type = ddlMessageType.Text;
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showExpired, type));
            Response.Redirect(url);
        }

        private void BindMessageType()
        {
            ddlMessageType.Items.Add(new ListItem("All"));
            ddlMessageType.Items.Add(new ListItem(Constants.MessageTypeValues.Ok));
            ddlMessageType.Items.Add(new ListItem(Constants.MessageTypeValues.Information));
            ddlMessageType.Items.Add(new ListItem(Constants.MessageTypeValues.Warning));
            ddlMessageType.Items.Add(new ListItem(Constants.MessageTypeValues.Critical));
        }
            
        void BindGrid(string type)
        {
            MessageBoardDataList = (List<MessageBoard>)CacheHelper<object>.Get($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID);
            if (MessageBoardDataList == null)
            {
                MessageBoardDataList = MessageBoardList.Where(x => x.TicketId == null || x.TicketId == "").ToList();
                var modifiedBy = string.Join(",", MessageBoardDataList.Select(x => x.ModifiedBy).Distinct().ToList());
                UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                List<UserProfile> userInfoList = UserManager.GetUserInfosById(modifiedBy);
                foreach (var dataList in MessageBoardDataList)
                {
                    var userName = userInfoList.Where(x => x.Id == dataList.ModifiedBy).Select(x => x.Name).FirstOrDefault();
                    dataList.ModifiedBy = userName;
                }
                CacheHelper<object>.AddOrUpdate($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID, MessageBoardDataList);
            }
            if (MessageBoardDataList.Count <= 0)
                MessageBoardDataList.Clear();

            if (MessageBoardDataList.Count > 0)
            {
                //int expireColumnIdx = _DataTable.Columns.IndexOf(DatabaseObjects.Columns.Expires);
                //int titleColumnIdx = _DataTable.Columns.IndexOf(DatabaseObjects.Columns.Title);
                
                //Set overall status                
                MessageBoard overallStatusRow = MessageBoardDataList.Where(x => x.Title.Equals(Constants.OverallStatusKey, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (overallStatusRow != null)
                {
                    overallStatus = Convert.ToString(overallStatusRow.MessageType);
                    rbMessageTypeList.SelectedIndex = rbMessageTypeList.Items.IndexOf(rbMessageTypeList.Items.FindByValue(overallStatus));
                }
                                
                MessageBoardDataList = MessageBoardDataList.Where(x => !x.Title.Equals(Constants.OverallStatusKey, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (!chkShowExpired.Checked)
                {
                    if (type == "All")
                    {
                        //dataRows = dataRows.Where(x => x.ItemArray[expireColumnIdx] == DBNull.Value || x.Field<DateTime>(DatabaseObjects.Columns.Expires).Date >= DateTime.Now.Date).ToArray();
                        MessageBoardDataList = MessageBoardDataList.Where(x => x.Expires >= DateTime.Now.Date || x.Expires == null).ToList();
                    }
                    else
                    {
                        //dataRows = dataRows.Where(x => (x.ItemArray[expireColumnIdx] == DBNull.Value || x.Field<DateTime>(DatabaseObjects.Columns.Expires).Date >= DateTime.Now.Date) && x.Field<string>(DatabaseObjects.Columns.MessageType).ToLower() == type.ToLower()).ToArray();
                        MessageBoardDataList = MessageBoardDataList.Where(x => (x.Expires >= DateTime.Now.Date || x.Expires == null) && x.MessageType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }
                }
                else
                {
                    if (type != "All")
                    {
                        //dataRows = dataRows.Where(x => x.Field<string>(DatabaseObjects.Columns.MessageType).ToLower() == type.ToLower()).ToArray();
                        MessageBoardDataList = MessageBoardDataList.Where(x => x.MessageType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }

                }
            }
            if (MessageBoardDataList != null && MessageBoardDataList.Count > 0)
            {
                _gridView.DataSource = MessageBoardDataList;
                _gridView.DataBind();
            }
        }

        protected void chkShowExpired_CheckedChanged(object sender, EventArgs e)
        {
            //string showExpired = chkShowExpired.Checked ? "1" : "0";
            //string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showExpired, ddlMessageType.Text));
            //Response.Redirect(url);
            RefreshPage();
        }

        protected void rbMessageTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBoard objMessageBoard = objMessageBoardManager.Load(x => x.Title == Constants.OverallStatusKey).FirstOrDefault();
            if (objMessageBoard == null)
            {
                objMessageBoard = new MessageBoard();
                objMessageBoard.Title = Constants.OverallStatusKey;
            }
            objMessageBoard.MessageType= rbMessageTypeList.SelectedValue;
            objMessageBoardManager.Update(objMessageBoard);
            RefreshPage();
        }

        protected void RefreshPage()
        {
            string showExpired = chkShowExpired.Checked ? "1" : "0";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, viewParam, formTitle, showExpired, ddlMessageType.Text));
            Response.Redirect(url);
        }

        protected void chkHideIfEmpty_CheckedChanged(object sender, EventArgs e)
        {
            objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            ConfigurationVariable configVariable = objConfigurationVariableManager.LoadVaribale(Constants.MessageBoardHideIfEmpty);
            configVariable.KeyValue = chkHideIfEmpty.Checked.ToString();
            objConfigurationVariableManager.Update(configVariable);
        }
                
        protected void _gridView_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var viewRow = _gridView.GetRow(e.VisibleIndex);
                if (viewRow == null) return;
                bool isExpire = false;

                
                //if (((MessageBoard)viewRow).Expires != DBNull.Value) //Need to check, with previous code.
                //{
                    DateTime expiryDate = Convert.ToDateTime(((MessageBoard)viewRow).Expires);
                    if (expiryDate != DateTime.MinValue && expiryDate.Date < DateTime.Now.Date)
                    {
                        isExpire = true;
                    }
                //}

                if (isExpire)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#A53421;");
                    e.Row.Style.Add(HtmlTextWriterStyle.Color, "#fff");
                }
                HtmlImage imgMessageType = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "ImgMessageType") as HtmlImage;
                string value = Convert.ToString(((MessageBoard)viewRow).MessageType);

                switch (value)
                {
                    case Constants.MessageTypeValues.Ok:
                        imgMessageType.Src = "/Content/Images/message_good.png";
                        break;
                    case Constants.MessageTypeValues.Information:
                        imgMessageType.Src = "/Content/Images/message_Information.png";
                        break;
                    case Constants.MessageTypeValues.Warning:
                        imgMessageType.Src = "/Content/Images/message_warning.png";
                        break;
                    case Constants.MessageTypeValues.Critical:
                        imgMessageType.Src = "/Content/Images/message_critical.png";
                        break;
                    default:
                        break;
                }

                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, ((MessageBoard)viewRow).ID));
                HtmlAnchor anchorEdit = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "aEdit") as HtmlAnchor;
                HtmlAnchor aBody = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "aBody") as HtmlAnchor;
                HiddenField hiddenTitle = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "hiddenBody") as HiddenField;
                HtmlAnchor lnkShowdetail = _gridView.FindRowCellTemplateControl(e.VisibleIndex, null, "aKeyname") as HtmlAnchor;
                string Body = UGITUtility.StripHTML(hiddenTitle.Value);
                string Title = "Edit Item";
                anchorEdit.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','415',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
                aBody.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','415',0,'{2}','true')", editItem, Title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
                aBody.InnerText = Body;
                if (isExpire)
                    aBody.Style.Add("color", "#FFF");
            }
        }

    }
}
