using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class MyOpenTickets : UserControl
    {
        protected string frameId = string.Empty;
        private List<string> authorizedModulesArray = new List<string>();
        UserProfile user;

        protected override void OnInit(EventArgs e)
        {
            var moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

            frameId = Request["frameObjId"];
            user = HttpContext.Current.CurrentUser();
            var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

           // List<UGITModule> drows = moduleViewManager.Load(x => x.EnableModule == true); // UGITUtility.GetModuleList(ModuleType.SMS);
            List<UGITModule> drows = moduleViewManager.Load(x => x.ModuleName == "LEM" || x.ModuleName == "OPM" || x.ModuleName == "CPR" || x.ModuleName == "CNS"); // UGITUtility.GetModuleList(ModuleType.SMS);
           // moduleTable = listModule.Where(x => x.ModuleName == "LEM" || x.ModuleName == "OPM" || x.ModuleName == "CPR" || x.ModuleName == "CNS").ToList();

            var moduleTable = new DataTable();
            if (drows != null && drows.Count > 0)
                moduleTable = UGITUtility.ToDataTable(drows);

            DataRow[] authorizedModules = new DataRow[0];

            var authorizedModuleRows = from row in moduleTable.AsEnumerable()
                                       where (uHelper.IsUserAuthorizedToViewModule(HttpContext.Current.GetManagerContext(), user, row))
                                       select row;

            if (authorizedModuleRows.Count() > 0)
                authorizedModules = authorizedModuleRows.CopyToDataTable().Select();

            if (authorizedModules.Length > 0)
            {
                authorizedModulesArray = authorizedModules.Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToList();
                //authorizedModulesIDArray = authorizedModules.Select(x => x.Field<string>(DatabaseObjects.Columns.ID)).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomFilteredTickets fTickets = (CustomFilteredTickets)Page.LoadControl("~/CONTROLTEMPLATES/shared/CustomFilteredTickets.ascx");
            fTickets.ModuleName = string.Empty;
            if (selectedMyTicketLink.Value != string.Empty)
            {
                fTickets.UserType = selectedMyTicketLink.Value.Trim();
            }
            else
            {
                fTickets.UserType = !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["UserType"])) ? Convert.ToString(Request.QueryString["UserType"]) : "my";
            }

            fTickets.MTicketStatus = TicketStatus.Open;
            if (Request["NoOfPreviewTickets"] != null && Request["NoOfPreviewTickets"] != string.Empty)
                fTickets.PageSize = UGITUtility.StringToInt(Request["NoOfPreviewTickets"]);
            if(fTickets.PageSize <= 0)
                fTickets.PageSize = 10;
            fTickets.IsPreview = true;
            fTickets.HideModuleDetail = true;
            fTickets.HideGlobalSearch = true;
            fTickets.HomeTabName = "mytickets";
            fTickets.MyHomeTab = Constants.MyHomeTicketTab;
            fTickets.IsHomePage = true;
            fTickets.CategoryName = "MyHomeTab";

            if (Convert.ToString(Request["source"]).Contains("DelegateControl.aspx"))
            {
                cMyTicketLinkspanel.Visible = false;
            }
                        
            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&IsPreview={4}&Status={5}&UserType={6}", frameId, "myopentickets", 10, Uri.EscapeDataString(Request.Url.AbsolutePath), "false", fTickets.MTicketStatus, fTickets.UserType));
            fTickets.MoreUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Item Details");
            
            //DataTable myOpenRequests = new DataTable();
            myTicketPreviewPanel.Controls.Add(fTickets);
            myTicketRepeater.Load += new EventHandler(MyTicketRepeater_Load);
            myTicketRepeater.ItemDataBound += new RepeaterItemEventHandler(MyTicketRepeater_ItemDataBound);
        }

        protected void MyTicketRepeater_Load(object sender, EventArgs e)
        {
            var tempResultedTable = new DataTable();
            tempResultedTable.Columns.Add("UserType", typeof(string));
            tempResultedTable.Columns.Add("Counter", typeof(int));

            var countTickets = new Dictionary<string, int>();
            var moduleStatistics = new ModuleStatistics(HttpContext.Current.GetManagerContext());
            countTickets = moduleStatistics.GetMyOpenTicketAsRoles(user, authorizedModulesArray);

            List<string> keys = countTickets.Keys.OrderBy(x => x).ToList();

            foreach (string key in keys)
            {
                DataRow newRow = tempResultedTable.NewRow();
                newRow["UserType"] = key;
                newRow["Counter"] = countTickets[key];
                tempResultedTable.Rows.Add(newRow);
            }

            myTicketRepeater.DataSource = tempResultedTable;
            myTicketRepeater.DataBind();

            // Total My Tickets
            //if (tempResultedTable.Rows.Count == 0)
            //{
            //    myrequestsLinkTab.Text = string.Format("{0} (0)", UpdateMyTickets);
            //}
            //else // rowcount > 1
            //{
            //    if (tempResultedTable.Rows.Count == 1)
            //        cMyTicketLinkspanel.Visible = false;


            //}
            // myclosedrequestsLinkTab.Text = string.Format("{0}", UpdateMyClosedTickets);
        }

        protected void MyTicketRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                RepeaterItem rItem = e.Item;
                DataRowView rowView = (DataRowView)rItem.DataItem;
                HtmlGenericControl linkDiv = (HtmlGenericControl)rItem.FindControl("myTicketsLinks");
                LinkButton link = (LinkButton)rItem.FindControl("myTicketLink");
                link.Text = string.Format("As&nbsp;{0}&nbsp;({1})</strong>", rowView["UserType"], rowView["Counter"]);

                if (rowView["UserType"].ToString() == selectedMyTicketLink.Value)
                    linkDiv.Attributes.Add("class", "ucontentdiv ugitsellinkbg clickedTab");

                link.OnClientClick = string.Format("SetSelectedMyTicketLink('{0}')", rowView["UserType"]);
            }
        }
    }
}
