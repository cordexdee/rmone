using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using uGovernIT.Utility;
using System.Data;
using DevExpress.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager.Managers;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.Shared
{
    public partial class History : UserControl
    {
        public string TicketId;
        public string ListName;
        public bool ReadOnly;
        public string FrameId;        
        DataRow ListItemCol;

        protected void Page_Load(object sender, EventArgs e)
        {
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            List<UserProfile> lstUserProfiles = UserManager.GetUsersProfile();
            
            ListItemCol = ticketManager.GetByTicketID(moduleViewManager.LoadByName(ListName),TicketId);            
            
            if (ListItemCol != null)
            {
                DataRow ListItem = ListItemCol;
                List<HistoryEntry> historyList = uHelper.GetHistory(ListItem, DatabaseObjects.Columns.History);                
                historyList.ForEach(x => 
                {
                    var userProfile = lstUserProfiles.FirstOrDefault(z => z.Id == x.createdBy || z.Name==x.createdBy);
                    //var userProfile = UserManager.GetUserInfoByIdOrName(x.createdBy); 
                    if (userProfile != null)
                    {
                        if (!string.IsNullOrEmpty(userProfile.Name))
                            x.createdBy = userProfile.Name;
                         
                        if (!string.IsNullOrEmpty(userProfile.Picture) && System.IO.File.Exists(Server.MapPath(userProfile.Picture)))
                            x.Picture = userProfile.Picture;
                        else
                            x.Picture = "/Content/Images/userNew.png";
                    }
                    else
                    {
                        x.Picture = "/Content/Images/userNew.png";
                    }
                });

                historyTable.DataSource = historyList;
                historyTable.DataBind();

                //foreach (HistoryEntry historyEntry in historyList)
                //{
                //    if (!string.IsNullOrEmpty(historyEntry.entry))
                //    {
                //        addToTable("(" + historyEntry.created + ") " + historyEntry.entry, historyEntry.createdBy, "", 3);
                //    }
                //}
            }
            else
            {
                errorMessage.Visible = true;
                errorMessage.Text = "No History entries.";
            }
        }
        private void addToTable(string value, string labelName, string type, int width)
        {
            TableRow row = new TableRow();
            row.BorderStyle = BorderStyle.Solid;
            row.BorderWidth = 1;
            row.CssClass = "ms-selectednav whiteborder";
            row.BorderStyle = BorderStyle.Solid;
            TableCell newCell = new TableCell();
            TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCellType.BorderStyle = BorderStyle.None;
            newCellType.BorderWidth = 0;

            newCellType.Text = labelName;
            newCellType.Width = 200;
            newCellType.Height = 30;
            newCellType.CssClass = "tableCell labelCell ms-selectednav whiteborder";
            newCell.Text = value;
            newCell.Height = 30;
            if (type == string.Empty)
            {
                newCell.ForeColor = System.Drawing.Color.Black;
                newCell.ColumnSpan = (width * 2) - 1;
                newCell.CssClass = "tableCell ms-alternatingstrong";
            }

            row.Cells.Add(newCellType);
            row.Cells.Add(newCell);

            //historyTable.Rows.Add(row);
        }
    }
}