using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Xml;
using System.Text;
using DevExpress.Web;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ApplicationsRelatedAssetsEdit : System.Web.UI.UserControl
    {
        public long Id { get; set; }
        public int AssetId { get; set; }
        protected DataTable dtOpenTickets;
        ApplicationServer spitem;
        ApplicationContext spWeb;
        protected override void OnInit(EventArgs e)
        {
            spWeb = HttpContext.Current.GetManagerContext();
            ApplicationServersManager mgr = new ApplicationServersManager(spWeb);
            if (Id > 0)
                spitem = mgr.LoadByID(Id);
            else
                //spitem = SPListHelper.GetSPList(DatabaseObjects.Lists.ApplicationServers).AddItem();

                Fill();
            string url = UGITUtility.GetAbsoluteURL("layouts/ugovernit/DelegateControl.aspx?control=environmentnew&ID=0");
            imgEnviroment.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','Environment - New Item','600','250',0,'{1}','true')", url, Server.UrlEncode(Request.Url.AbsolutePath)));
            BindServerFunctions();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        void Fill()
        {
            FillEnvironment();
            FillApplications();
            //SPFieldLookupValue spLookupValue = new SPFieldLookupValue(Convert.ToString(spitem[DatabaseObjects.Columns.EnvironmentLookup]));
            //ddlEnvironment.SelectedValue = Convert.ToString(spLookupValue.LookupId);
        }

        protected void FillEnvironment()
        {
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Environment, $"{DatabaseObjects.Columns.TenantID} = '{spWeb.TenantID}'");
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (UGITUtility.StringToInt(dr[DatabaseObjects.Columns.Deleted]) == 0)
                        ddlEnvironment.Items.Add(new ListItem(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.Id])));
                }
            }
            ddlEnvironment.Items.Insert(0, new ListItem("(None)", "0"));
        }
        protected void FillApplications()
        {
            TicketManager ticketManager = new TicketManager(spWeb);
            Ticket moduleRequest = new Ticket(spWeb, "APP");
            ModuleViewManager ObjModuleViewManager = new ModuleViewManager(spWeb);
            List<ModuleColumn> columns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
            GridViewDataTextColumn colId;

            foreach (ModuleColumn moduleColumn in columns)
            {
                string fieldColumn = Convert.ToString(moduleColumn.FieldName);

                colId = new GridViewDataTextColumn();
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                if (fieldColumn == DatabaseObjects.Columns.AssetDescription)
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                else
                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.FieldName = fieldColumn;

                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                if (fieldColumn == DatabaseObjects.Columns.AssetTagNum)
                    colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;

                colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                colId.Name = fieldColumn;
                grid.Columns.Add(colId);
            }
            UGITModule module = ObjModuleViewManager.GetByName("APP");
            dtOpenTickets = ticketManager.GetOpenTickets(module);
            grid.DataBind();
        }

        protected void BindServerFunctions()
        {
            FieldConfigurationManager mgr = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            DataTable spLstApplicationServer = mgr.GetFieldDataByFieldName(DatabaseObjects.Columns.ServerFunctions, "CMDB", "", HttpContext.Current.GetManagerContext().TenantID);

            if (spLstApplicationServer != null)
            {
                glServerFunctions.DataSource = spLstApplicationServer;
                glServerFunctions.DataBind();
            }

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (dtOpenTickets != null && dtOpenTickets.Rows.Count > 0)
            {
                grid.DataSource = dtOpenTickets;
                if (grid.Columns[DatabaseObjects.Columns.Attachments] != null)
                {
                    grid.Columns[DatabaseObjects.Columns.Attachments].Caption = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
                    grid.Columns[DatabaseObjects.Columns.Attachments].Width = Unit.Pixel(50);
                }
            }
        }

        protected void grid_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.Attachments)
            {
                if (UGITUtility.StringToBoolean(e.Value))
                    e.DisplayText = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
                else
                    e.DisplayText = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                List<string> lstServerFunctions = new List<string>();
                List<string> selectedServerFunctions = glServerFunctions.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string sau in selectedServerFunctions)
                {
                    lstServerFunctions.Add(sau);
                }

                bool flag = true;
                DataTable spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationServers, $"{DatabaseObjects.Columns.TenantID} = '{spWeb.TenantID}' and AssetsTitleLookup={AssetId}");
                ApplicationServersManager mgr = new ApplicationServersManager(spWeb);
                foreach (DataRow item in spListItemColl.Rows)
                {
                    int environmentId = Convert.ToInt32(item[DatabaseObjects.Columns.EnvironmentLookup]);
                    int applicationId = Convert.ToInt32(item[DatabaseObjects.Columns.APPTitleLookup]);

                    if (Convert.ToString(environmentId) == ddlEnvironment.SelectedValue && applicationId == Convert.ToInt32(grid.GetSelectedFieldValues(DatabaseObjects.Columns.Id).Count > 0 ? grid.GetSelectedFieldValues(DatabaseObjects.Columns.Id)[0] : 0))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    ApplicationServer sp = new ApplicationServer();
                    string title = ddlEnvironment.SelectedItem.Text + (grid.GetSelectedFieldValues(DatabaseObjects.Columns.Title).Count > 0 ? " - " + grid.GetSelectedFieldValues(DatabaseObjects.Columns.Title)[0] : string.Empty);
                    sp.Title = title;
                    sp.EnvironmentLookup = Convert.ToInt64(ddlEnvironment.SelectedValue);
                    sp.AssetsTitleLookup = AssetId;
                    sp.APPTitleLookup = grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID).Count > 0 ? Convert.ToInt64(grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0]) : 0;
                    sp.ServerFunctionsChoice = string.Join(Constants.Separator, lstServerFunctions);
                    mgr.Insert(sp);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('This application is already linked to this asset.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an application.');", true);
            }

        }
    }
}