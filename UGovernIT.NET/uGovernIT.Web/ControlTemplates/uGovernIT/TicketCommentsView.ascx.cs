using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using System.Web.UI;
using uGovernIT.Utility;
using System.Data;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.DAL;
using uGovernIT.Utility.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using uGovernIT.DAL.Store;

namespace uGovernIT.Web
{
    public partial class TicketCommentsView : UserControl
    {
        public string TicketID { get; set; }
        public string listName { get; set; }
        public string commentType { get; set; }
        public string currentModuleName { get; set; }
        private ConfigurationVariableManager _configurationVariableManager = null;
        List<HistoryEntry> datalist;
        DataRow[] ListItemCol;
        DataRow ListItem;
        string fieldName;
        bool isActionUser;
        bool isAdmin;
        UserProfile User;
        UserProfileManager UserManager;
        UGITModule Module = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager ObjModuleViewManager = null;
        TicketManager ObjTicketManager = null;
        TicketStore ticketStore;

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if(_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(context);
                }
                return _configurationVariableManager;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            //ObjModuleViewManager = new ModuleViewManager(context);
            //ObjTicketManager = new TicketManager(context);

            //UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            //User = Context.CurrentUser();
            //GetTicketComments();
            //aspxGrid.Styles.Header.CssClass = "homeGrid_headerColumn";
            //aspxGrid.SettingsCommandButton.ShowAdaptiveDetailButton.Styles.Style.CssClass = "homeGrid_openBTn";
            //aspxGrid.SettingsCommandButton.HideAdaptiveDetailButton.Styles.Style.CssClass = "homeGrid_closeBTn";
            //if(datalist!= null)
            //{
            //    aspxGrid.DataSource = GetHistoryData(datalist);
            //}
            //else
            //{
            //    DataRow ticketCollection = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(TicketID), TicketID);
            //    bool oldestFirst = !ConfigurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
            //    datalist = uHelper.GetHistory(ticketCollection, DatabaseObjects.Columns.TicketComment, oldestFirst);
            //    aspxGrid.DataSource = GetHistoryData(datalist);
            //}
            //aspxGrid.DataBind();
            //if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
            //{

            //    aspxGrid.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCells;
            //    aspxGrid.SettingsAdaptivity.AllowOnlyOneAdaptiveDetailExpanded = true;
            //}
            base.OnInit(e);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        DataTable GetHistoryData(List<HistoryEntry> history)
        {
            DataTable data = new DataTable();
            data.Columns.Add("IndexID", typeof(int));
            data.Columns.Add("Created");
            data.Columns.Add("createdByUser");
            data.Columns.Add("entry");
            for (int i = 0; i < history.Count; i++)
            {
                UserProfile user = UserManager.GetUserById(history[i].createdBy);
                if (user != null)
                {
                    data.Rows.Add(i, history[i].created, user.Name.ToString(), history[i].entry);
                }
                else
                {
                    data.Rows.Add(i, history[i].created, history[i].createdBy, history[i].entry);
                }
            }
            return data;
        }
        private void GetTicketComments()
        {            
            Module = ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(TicketID));
            //ListItemCol = ObjTicketManager.GetAllTickets(Module).Select(DatabaseObjects.Columns.TicketId+"='" + TicketID+"'");
            if (Module != null)
            {
                ListItemCol = ObjTicketManager.GetTicketTableBasedOnTicketId(Module.ModuleName, TicketID).Select();
            }            

            if (ListItemCol != null)
            {
                 ListItem = ListItemCol[0];
                isActionUser = Ticket.IsActionUser(context,ListItem, User);
               isAdmin = UserManager.IsUGITSuperAdmin(User) || UserManager.IsTicketAdmin(User);

                if (commentType == "Comment")
                {
                    bool oldestFirst = true;// !ConfigurationVariableHelper.GetConfigVariableValueAsBool(ConfigConstants.CommentsNewestFirst);
                    datalist = uHelper.GetHistory(ListItem, DatabaseObjects.Columns.TicketComment, oldestFirst);
                    fieldName = DatabaseObjects.Columns.TicketComment;
                }
                else
                {
                    datalist = uHelper.GetHistory(ListItem, DatabaseObjects.Columns.TicketResolutionComments, false);
                    fieldName = DatabaseObjects.Columns.TicketResolutionComments;
                }

            }


        }

        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
          //  ticketStore = new TicketStore(context);
          //  List<object> selectedData = aspxGrid.GetSelectedFieldValues("IndexID", "entry", "Created");
          //  if (selectedData.Count == 0)
          //      return;

          //  int count = 0;
          //  for (int i = 0; i < selectedData.Count; i++)
          //  {
          //      object[] vals = (object[])selectedData[i];
          //      datalist.RemoveAll(element => element.created == Convert.ToString(vals[2]) && element.entry == Convert.ToString(vals[1]));
          //      count++;
          //  }

          //  string historyMsg = string.Empty;

          //  List<HistoryEntry> tempdatalist = new List<HistoryEntry>();
          //  tempdatalist = datalist.OrderBy(tt => Convert.ToDateTime(tt.created)).ToList();

          //  if (commentType == "Comment")
          //  {
          //      ListItem[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentsbyDataList(tempdatalist);
          //      historyMsg = count + " comments are deleted.";
          //  }
          //  else
          //  {
          //      ListItem[DatabaseObjects.Columns.TicketResolutionComments] = uHelper.GetCommentsbyDataList(tempdatalist);
          //      historyMsg = count + " resolution comments are deleted.";
          //  }

          //  TicketDal.SaveTicket(ListItem, Module.ModuleTable, false);
          ////  ticketStore.Save(Module, ListItem);

          //  if (count > 0)
          //  {
          //      uHelper.CreateHistory(User, historyMsg, ListItem, false,context);
          //  }

          //  GetTicketComments();
          //  aspxGrid.DataSource = GetHistoryData(datalist);
          //  aspxGrid.DataBind();

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void aspxGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            ticketStore = new TicketStore(context);
            int index = Convert.ToInt32(e.Keys[0]);
            if (datalist.Count > index)
            {
                string newEntry = Convert.ToString(e.NewValues["entry"]);
                HistoryEntry entry = datalist[index];

                if (entry != null && entry.entry.ToLower() != newEntry.ToLower())
                {
                    string oldEntry = entry.entry;

                    entry.entry = newEntry;
                    ListItem[fieldName] = uHelper.GetCommentsbyDataList(datalist);
                    string historyMsg = string.Empty;
                    if (fieldName == DatabaseObjects.Columns.TicketComment)
                        historyMsg = string.Format("Comment ({0} => {1})", oldEntry, newEntry);
                    else
                        historyMsg = string.Format("Resolution Description ({0} => {1})", oldEntry, newEntry);
                    uHelper.CreateHistory(User, historyMsg, ListItem, false,context);
                  
                  // ticketStore.Save(Module, ListItem);
                    TicketDal.SaveTicket(ListItem,Module.ModuleTable, false);
                    ticketStore.UpdateTicketCache(ListItem, Module, false);

                }
            }
            e.Cancel = true;
            //aspxGrid.CancelEdit();


            GetTicketComments();
            //aspxGrid.DataSource = GetHistoryData(datalist);
            //aspxGrid.DataBind();
        }


        protected void aspxGrid_CommandButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex >= 0)
                return;

            if (e.Column.Name == "editAction")
            {
                //DataRow row = aspxGrid.GetDataRow(e.VisibleIndex);
                //string user = Convert.ToString(row["createdByUser"]);
                e.Visible = true;
                //if (isActionUser && user.ToLower() == SPContext.Current.Web.CurrentUser.Name.ToLower())
                //{
                //    e.Visible = true;
                //}

                if (isAdmin)
                    e.Visible = true;
            }
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}