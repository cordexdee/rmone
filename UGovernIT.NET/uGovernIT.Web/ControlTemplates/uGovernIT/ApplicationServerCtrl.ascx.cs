
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Core;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ApplicationServerCtrl : UserControl
    {
        public int ApplicationId { get; set; }
        List<ApplicationServer> spListItemColl = null;
        DataTable dt = new DataTable();
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&ItemID={1}&AppId={2}";
        private string addNewItem = string.Empty;
        public bool isUserExists { get; set; }
        public bool isDataBind;
        UserProfile User = HttpContext.Current.CurrentUser();
        ApplicationServersManager appServersManager = new ApplicationServersManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            DataRow spItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, ApplicationId)[0];
            isUserExists = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(), spItem, User);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            values.Add("@APPTitleLookup", ApplicationId);
            dt = GetTableDataManager.GetData(DatabaseObjects.Tables.ApplicationServers, values);
            grdAppServer.Columns[6].Visible = false;
            if (isUserExists)
                grdAppServer.Columns[6].Visible = true;


            BindGrid();

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, "appserveredit", "0", ApplicationId, "ADD"));
            string url = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2} - New Item','1000','500',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), "Application Infrastructure");
            aAddNew.ClientSideEvents.Click = "function(){ " + url + " }";
            aAddNew.Visible = isUserExists;
            base.OnInit(e);
        }

        private void BindGrid()
        {
            if (isDataBind)
                return;
            if (dt != null)
            {
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UGITModule module = moduleViewManager.LoadByName("CMDB");
                TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                DataTable dtAssets = ticketManager.GetAllTickets(module);
                if (!dt.Columns.Contains(DatabaseObjects.Columns.AssetDescription))
                    dt.Columns.Add(DatabaseObjects.Columns.AssetDescription);

                if (!dt.Columns.Contains(DatabaseObjects.Columns.AssetTagNum))
                    dt.Columns.Add(DatabaseObjects.Columns.AssetTagNum);

                if (!dt.Columns.Contains(DatabaseObjects.Columns.ServerFunctions))
                    dt.Columns.Add(DatabaseObjects.Columns.ServerFunctions);

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drAsset = dtAssets.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.AssetName, Convert.ToString(dr["AssetsTitleLookup"]))).FirstOrDefault();
                    if (drAsset != null)
                    {
                        dr[DatabaseObjects.Columns.AssetDescription] = drAsset[DatabaseObjects.Columns.AssetDescription];
                        dr[DatabaseObjects.Columns.AssetTagNum] = drAsset[DatabaseObjects.Columns.AssetTagNum];
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ServerFunctions])))
                    {
                        dr[DatabaseObjects.Columns.ServerFunctions] = Convert.ToString(dr[DatabaseObjects.Columns.ServerFunctions]).Replace(";#",", ");
                    }
                    //SPFieldMultiChoiceValue serverFunctions = new SPFieldMultiChoiceValue(Convert.ToString(dr[DatabaseObjects.Columns.ServerFunctions]));
                    //if(serverFunctions!=null)
                    //{
                    //    string strServerFunctions = string.Empty;
                    //    for (int i = 0; i < serverFunctions.Count; i++)
                    //        strServerFunctions += serverFunctions[i] + "; ";
                    //    if(!string.IsNullOrEmpty(strServerFunctions))
                    //    {
                    //        dr[DatabaseObjects.Columns.ServerFunctions] = strServerFunctions;
                    //    }
                    //}
                }
            }

            grdAppServer.DataSource = dt;
            grdAppServer.DataBind();
            isDataBind = true;
        }

        protected void grdAppServer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    foreach (DataControlFieldCell cell in e.Row.Cells)
            //    {
            //        // check all cells in one row
            //        foreach (Control control in cell.Controls)
            //        {
            //            // Must use LinkButton here instead of ImageButton
            //            // if you are having Links (not images) as the command button.
            //            ImageButton button = control as ImageButton;
            //            if (button != null && button.CommandName == "Delete")
            //                // Add delete confirmation
            //                button.OnClientClick = "if (!confirm('Are you sure you want to unlink this asset?')) return;";
            //        }
            //    }
            //}
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindGrid();
            base.OnPreRender(e);
        }

        protected void grdAppServer_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
           
        }


        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ASPxButton lnkDelete = sender as ASPxButton;
            int dataKey = Convert.ToInt32(lnkDelete.CommandArgument);
            ApplicationServer item = appServersManager.LoadByID(dataKey); // spListItemColl.GetItemById(Convert.ToInt32(e.Keys[0]));
            appServersManager.Delete(item);
            
            isDataBind = false;
            spListItemColl = appServersManager.Load(x => x.APPTitleLookup == ApplicationId).ToList();
            BindGrid();
        }

        protected void grdAppServer_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                string lsDataKeyValue = Convert.ToString(e.KeyValue);
                ASPxButton lnkDelete = grdAppServer.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkDelete") as ASPxButton;
                if (lnkDelete != null)
                {
                    lnkDelete.CommandArgument = lsDataKeyValue;
                    lnkDelete.Visible = isUserExists;
                }
            }
        }
    }
}
