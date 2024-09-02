using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Core;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ApplicationServerEdit : UserControl
    {
        public long id { get; set; }
        public long appid { get; set; }

        ApplicationServer spitem;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ApplicationServersManager appServerMGR = null;
        protected override void OnInit(EventArgs e)
        {
            appServerMGR = new ApplicationServersManager(context);

            if (id > 0)
            {
                spitem = appServerMGR.Load().FirstOrDefault(x => x.ID == id); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.ApplicationServers, id);
            }
            else
            {
                spitem = new ApplicationServer();
            }
            Fill();
            base.OnInit(e);
            string url = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=environmentnew&ID=0");
            string url1 = string.Format("window.parent.UgitOpenPopupDialog('{0}','','Environment - New Item','600','250',0,'{1}','true')", url, Server.UrlEncode(Request.Url.AbsolutePath));
            imgEnviroment.ClientSideEvents.Click = "function(){ " + url1 +" }";
            //imgEnviroment.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','Environment - New Item','600','250',0,'{1}','true')", url, Server.UrlEncode(Request.Url.AbsolutePath)));
            BindServerFunctions();
        }
        private void BindServerFunctions()
        {
            DataTable dt = new DataTable();

            //List<ApplicationServer> lstAppServers = appServerMGR.Load();
            //dt = UGITUtility.ToDataTable<ApplicationServer>(lstAppServers);

            string choiceFieldName = DatabaseObjects.Columns.ServerFunctions;
            if (!dt.Columns.Contains(choiceFieldName))
                dt.Columns.Add(choiceFieldName);
            //string[] colnames = new string[] { DatabaseObjects.Columns.ServerFunctions };
            //dt = dt.DefaultView.ToTable(false, colnames);
            //SPList spLstApplicationServer = SPListHelper.GetSPList(DatabaseObjects.Lists.ApplicationServers);
            if (dt != null && dt.Columns.Contains(choiceFieldName))
            {
                //SPFieldMultiChoice choiceField = spLstApplicationServer.Fields.GetFieldByInternalName(choiceFieldName) as SPFieldMultiChoice;
                //foreach (string choice in choiceField.Choices)
                //    dt.Rows.Add(choice);
                FieldConfigurationManager fieldMGR = new FieldConfigurationManager(context);
                FieldConfiguration field = fieldMGR.GetFieldByFieldName(DatabaseObjects.Columns.ServerFunctions);
                List<string> choices = UGITUtility.ConvertStringToList(field.Data, Constants.Separator);
                foreach (string choice in choices)
                {
                    dt.Rows.Add(choice);
                }
            }
            //DataRow[] cmdbData = dt.Select("ServerFunctions <> ''");
            glServerFunctions.DataSource = dt;
            glServerFunctions.DataBind();
          
        }
        void Fill()
        {
            FillEnvironment();
            FillAssets();
            //var spLookupValue = new SPFieldLookupValue(Convert.ToString(spitem[DatabaseObjects.Columns.EnvironmentLookup]));
            //ddlEnvironment.SelectedValue = Convert.ToString(spLookupValue.LookupId);
            //spLookupValue = new SPFieldLookupValue(Convert.ToString(spitem[DatabaseObjects.Columns.AssetsTitleLookup]));
        }

        protected void FillEnvironment()
        {
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Environment, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'"); // SPListHelper.GetDataTable(DatabaseObjects.Lists.Environment);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ddlEnvironment.Items.Add(new ListItem(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.Id])));
                }
            }
            ddlEnvironment.Items.Insert(0, new ListItem("(None)", "0"));
        }

        protected void FillAssets()
        {
            Ticket moduleRequest = new Ticket(context, "CMDB");

            List<ModuleColumn> columns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
            GridViewDataTextColumn colId;
            foreach (ModuleColumn moduleColumn in columns)
            {
                string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                if (fieldColumn != DatabaseObjects.Columns.AssetOwner && fieldColumn != DatabaseObjects.Columns.Attachments)
                {
                    colId = new GridViewDataTextColumn();
                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    if (fieldColumn == DatabaseObjects.Columns.AssetDescription)
                    {
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    }

                    colId.PropertiesTextEdit.EncodeHtml = false;

                    if (fieldColumn.EndsWith("Lookup") || fieldColumn.EndsWith("User"))
                        colId.FieldName = $"{fieldColumn}$";
                    else
                        colId.FieldName = fieldColumn;

                    colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                    colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    if (fieldColumn == DatabaseObjects.Columns.AssetTagNum)
                    {
                        colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                    }


                    colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                    colId.Name = fieldColumn;
                    grid.Columns.Add(colId);
                }
            }
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            
            UGITModule cmdb = moduleViewManager.LoadByName("CMDB");
            TicketManager ticketManager = new TicketManager(context); 
            grid.DataSource = ticketManager.GetAllTickets(cmdb);
            grid.DataBind();
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = GetData();
        }

        private void GetData()
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                List<string> lstServerFunctions = new List<string>();
                List<string> selectedServerFunctions = glServerFunctions.Text.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string sau in selectedServerFunctions)
                        lstServerFunctions.Add(sau);
               
                bool flag = true;

                List<ApplicationServer> listAppServers = appServerMGR.Load(x => x.APPTitleLookup == appid).ToList();
                foreach (ApplicationServer item in listAppServers)
                {
                    long eValue = item.EnvironmentLookup;
                    long aValue = item.AssetsTitleLookup;

                    if (Convert.ToString(eValue) == ddlEnvironment.SelectedValue && aValue == Convert.ToInt32(grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID).Count > 0 ? grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0] : 0))
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    spitem.Title = ddlEnvironment.SelectedItem.Text + (grid.GetSelectedFieldValues(DatabaseObjects.Columns.AssetName).Count > 0 ? " - " + grid.GetSelectedFieldValues(DatabaseObjects.Columns.AssetName)[0] : string.Empty);
                    spitem.EnvironmentLookup = Convert.ToInt64( ddlEnvironment.SelectedValue);
                    spitem.AssetsTitleLookup = Convert.ToInt64( grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID).Count > 0 ? grid.GetSelectedFieldValues(DatabaseObjects.Columns.ID)[0] : 0);
                    spitem.APPTitleLookup = appid;
                    spitem.ServerFunctionsChoice = string.Join(Constants.Separator, lstServerFunctions);
                    if (spitem.ID > 0)
                        appServerMGR.Update(spitem);
                    else
                        appServerMGR.Insert(spitem);

                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('This asset is already linked to this application');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an asset');", true);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
