using DevExpress.XtraScheduler.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class ViewSubContractor : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        public string PublicTicketID { get; set; }
        DataRow splstItem;

        protected int sunContratorCount = 0;

        private ApplicationContext _context = null;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            BindSetName(ModuleName, "SubContractor");
            if (Request["IsViewSubConFilter"] == "true")
            {
                ASPxGridViewProcoreSubContractor.Columns["UGITDescription"].Visible = false;
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["IsViewSubConFilter"] == "true")
            {
                ImgbtnSyncSubContractor.Visible = false;
            }
            else
            {

                var id = PublicTicketID;
                ////splstItem = uHelper.GetTicket(PublicTicketID);
                //// splstItem = UGITUtility.GetTicket(PublicTicketID);
                splstItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(PublicTicketID), PublicTicketID);

                if (string.IsNullOrEmpty(Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId])))
                {
                    //lnkbtnSyncSubContractor.Visible = false;
                    ImgbtnSyncSubContractor.Visible = false; 
                }
                else
                {

                    ImgbtnSyncSubContractor.Visible = false; /*added for now, need to undone once procore code will be done*/
                    ////SPQuery queryCheckSubContractor = new SPQuery();
                    string queryCheckSubContractor  = string.Format("{0} = '{1}' ", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));
                    //// queryCheckSubContractor.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));

                   //// SPListItemCollection splstColProcoreSubContractor = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.SubContractor, queryCheckSubContractor);
                    DataTable splstColProcoreSubContractor = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, $"{queryCheckSubContractor.ToString()} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);

                    if (splstColProcoreSubContractor != null)
                        sunContratorCount = splstColProcoreSubContractor.Rows.Count;
                }
               
            }
            BindProcoreSubContractor();
        }

        protected void ASPxGridViewProcoreSubContractor_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (Request["IsViewSubConFilter"] == "true")
            {
                if (e.RowType == DevExpress.Web.GridViewRowType.Data)
                {
                    DataRow currentRow = ASPxGridViewProcoreSubContractor.GetDataRow(e.VisibleIndex);

                    if (currentRow != null && e.Row.Cells.Count > 1)
                    {
                        string url = Ticket.GenerateTicketURL(ApplicationContext,Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]));
                        string func = string.Format("javascript:openTicketDialog('{0}','','{1}','90','90', 0, '{2}')", url, Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]), Server.UrlEncode(Request.Url.AbsolutePath));
                        e.Row.Attributes.Add("onclick", func);
                    }
                }
            }
        }



        private void BindProcoreSubContractor()
        {
            if (Request["IsViewSubConFilter"] == "true")
            {
               //// SPQuery query = new SPQuery();
                string query = string.Format("{0} = '{1}'", DatabaseObjects.Columns.CompanyName, Request["SubContractorName"].Replace('-', '&'));


                ////query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.CompanyName, Request["SubContractorName"].Replace('-', '&'));
                //// DataTable dtProcoreSubContractor = SPListHelper.GetDataTable(DatabaseObjects.Lists.SubContractor, query);
                DataTable dtProcoreSubContractor = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, $"{query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                DataTable dtnew = null;
                if (dtProcoreSubContractor != null && dtProcoreSubContractor.Rows.Count > 0)
                    dtnew = dtProcoreSubContractor.Clone();
                foreach (DataRow item in dtProcoreSubContractor.Rows)
                {
                    ////DataRow splstItem = uHelper.GetTicket(Convert.ToString(item[DatabaseObjects.Columns.TicketId]));

                    if (splstItem[DatabaseObjects.Columns.EstimatedConstructionStart] != null && splstItem[DatabaseObjects.Columns.EstimatedConstructionEnd] != null)
                    {
                        if ((Convert.ToDateTime(splstItem[DatabaseObjects.Columns.EstimatedConstructionStart]).Month <= Convert.ToDateTime(Request["startDate"]).Month && Convert.ToDateTime(splstItem[DatabaseObjects.Columns.EstimatedConstructionStart]).Year <= Convert.ToDateTime(Request["startDate"]).Year) && (Convert.ToDateTime(splstItem[DatabaseObjects.Columns.EstimatedConstructionEnd]).Month >= Convert.ToDateTime(Request["endDate"]).Month && Convert.ToDateTime(splstItem[DatabaseObjects.Columns.EstimatedConstructionEnd]).Year >= Convert.ToDateTime(Request["endDate"]).Year))
                        {
                            DataRow dr = dtnew.NewRow();
                            foreach (DataColumn dCol in dtnew.Columns)
                            {
                                dr[dCol.ToString()] = item[dCol.ToString()].ToString();
                            }

                            dtnew.Rows.Add(dr);
                        }
                    }
                }
                ASPxGridViewProcoreSubContractor.DataSource = dtnew;
                ASPxGridViewProcoreSubContractor.DataBind();
            }
            else
            {
                ////SPQuery query = new SPQuery();
                ////query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));
                ////DataTable dtProcoreSubContractor = SPListHelper.GetDataTable(DatabaseObjects.Tables.SubContractor, query);
                string query = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));
                DataTable dtProcoreSubContractor = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, $"{query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                ASPxGridViewProcoreSubContractor.DataSource = dtProcoreSubContractor;
                ASPxGridViewProcoreSubContractor.DataBind();
            }

        }


        //protected void lnkbtnSyncSubContractor_Click(object sender, EventArgs e)
        //{
        //    fieldSetPopupControl.ShowOnPageLoad = true;

        //}

        private void BindSetName(string selectedModule, string selectedUtility)
        {
            ddlSet.Items.Clear();
            ////SPQuery query = new SPQuery();
            ////query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.ModuleNameLookup, selectedModule, DatabaseObjects.Columns.ProcoreUtilityLookup, selectedUtility);
            ////query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.FieldSetName, DatabaseObjects.Columns.FieldSetType);
            ////DataTable dtMapping = SPListHelper.GetDataTable(DatabaseObjects.Tables.ProcoreMapping, query);
            ////string query = string.Format("{0} = '{1}' and {2} = '{3}'", DatabaseObjects.Columns.ModuleNameLookup, selectedModule, DatabaseObjects.Columns.ProcoreUtilityLookup, selectedUtility);


            ////DataTable dtMapping = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProcoreMapping, $"{query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);

            ////ddlSet.DataSource = dtMapping;
            ////ddlSet.DataTextField = DatabaseObjects.Columns.FieldSetName;
            ////ddlSet.DataValueField = DatabaseObjects.Columns.Id;
            ////ddlSet.DataBind();

        }

        protected void lnkbtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlSet.SelectedValue) || ddlSet.SelectedValue == "0")
            {
                lblMessage.Text = "Field set required.";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Black;
                fieldSetPopupControl.ShowOnPageLoad = false;
                return;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId])))
            {
                //Procore URl for token.

                ConfigurationVariableManager ConfigurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
                string ProcoreBaseUrl = ConfigurationVariableManager.GetValue(ConfigConstants.ProcoreBaseUrl);
                string ProcoreCompanyId = ConfigurationVariableManager.GetValue(ConfigConstants.ProcoreCompanyId);
                string ProcoreToken = ConfigurationVariableManager.GetValue(ConfigConstants.ProcoreToken);


               //// SPQuery query = new SPQuery();
                //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ProcoreMappingLookup, FieldSetId);

                string Query = string.Format("{0} = {1}", DatabaseObjects.Columns.ProcoreMappingLookup, ddlSet.SelectedValue);
                ////query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ProcoreMappingLookup, ddlSet.SelectedValue);
                ////query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/><FieldRef Name='{4}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.ProcoreColumnName, DatabaseObjects.Columns.InternalColumnName, DatabaseObjects.Columns.ProcoreMappingLookup, DatabaseObjects.Columns.PreSetValue);
                ////DataTable dtProcoreFieldsMapping = SPListHelper.GetDataTable(DatabaseObjects.Lists.ProcoreFieldsMapping, query);

                DataTable dtProcoreFieldsMapping = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProcoreFieldsMapping, $"{Query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);


                if (!string.IsNullOrEmpty(ProcoreBaseUrl) && !string.IsNullOrEmpty(ProcoreCompanyId))
                {
                    string ProcoreAPIToken = string.Empty;
                    if (!string.IsNullOrEmpty(ProcoreToken))
                    {
                        ProcoreAPIToken = ProcoreToken;
                        try
                        {
                            var urltokenInfo = string.Format("{0}oauth/token/info", ProcoreBaseUrl);

                            var syncTokenInfoClient = new WebClient();
                            syncTokenInfoClient.Headers.Add("Authorization", string.Format("bearer {0}", ProcoreAPIToken));
                            var contentTokenInfo = syncTokenInfoClient.DownloadString(urltokenInfo);
                            DataContractJsonSerializer serializerTokenInfo = new DataContractJsonSerializer(typeof(DevExpress.XtraScheduler.Native.TokenInfo));
                            using (var mstokeninfo = new MemoryStream(Encoding.Unicode.GetBytes(contentTokenInfo)))
                            {
                                var TokenInfoData = (DevExpress.XtraScheduler.Native.TokenInfo)serializerTokenInfo.ReadObject(mstokeninfo);

                                ////if (TokenInfoData.expires_in_seconds < 100)
                                ////{
                                ////    uHelper.ProcoreTokenRefresh(ProcoreBaseUrl);
                                ////    ProcoreAPIToken = ConfigurationVariable.GetValue(ConfigConstants.ProcoreToken);
                                ////}
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                            ////Log.WriteException(ex, "ProjectRoleView- ProcoreAPIToken");
                            //errorMsg.Text = "Unable to connect with Procore.";
                            //errorMsg.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        ////Log.WriteLog("Procore Token Required.", "ProjectRoleView- ProcoreAPIToken");
                        return;
                    }

                    #region Delete Old SubContractor
                    ////SPQuery queryCheckSubContractor = new SPQuery();
                    ////queryCheckSubContractor.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));
                    ////SPListItemCollection splstColProcoreSubContractor = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.SubContractor, queryCheckSubContractor);

                    string queryCheckSubContractor = string.Format("{0} = {1}", DatabaseObjects.Columns.ExternalProjectId, Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]));
                    DataTable splstColProcoreSubContractor = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, $"{Query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                    ////List<int> Ids = new List<int>();
                    List<long> Ids = new List<long>();
                    foreach (DataRow item in splstColProcoreSubContractor.Rows)
                    {
                        Ids.Add(Convert.ToInt32(item[DatabaseObjects.Columns.Id]));
                    }

                    if (Ids.Count > 0)
                    {
                        RMMSummaryHelper.BatchDeleteListItems(ApplicationContext, Ids, DatabaseObjects.Tables.SubContractor);
                    }
                    #endregion
                    //  if (ddlProcoreUtility.SelectedItem.Text == "Project Templates")
                    //  {
                    try
                    {


                        var apiUrl = string.Format("{2}vapid/commitments?access_token={0}&project_id={1}", ProcoreAPIToken.Trim(), Convert.ToString(splstItem[DatabaseObjects.Columns.ExternalProjectId]), ProcoreBaseUrl.Trim());

                        var newsyncClient = new WebClient();
                        var newcontent = newsyncClient.DownloadString(apiUrl);

                        List<ProjectCommitments> lstprojectcommitments = JsonConvert.DeserializeObject<List<ProjectCommitments>>(newcontent);
                        int counter = 0;
                        foreach (ProjectCommitments item in lstprojectcommitments)
                        {
                            DataRow itemProcoreSubContractor;
                            //Procorefields.Add(item.Key);
                            if (dtProcoreFieldsMapping != null && dtProcoreFieldsMapping.Rows.Count > 0)
                            {
                                ////SPList splstProcoreSubContrator = SPListHelper.GetSPList(DatabaseObjects.Lists.SubContractor);
                                //string query = "{1} = {1}";
                                DataTable splstProcoreSubContrator = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SubContractor, $"{Query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);
                                                                                                                                                                                                      //itemProcoreSubContractor = splstProcoreSubContrator.Items.Add();

                                SubContractor SubContractor = new SubContractor();
                                SubContractorManager SubContractorManager = new SubContractorManager(ApplicationContext);
                                itemProcoreSubContractor = splstProcoreSubContrator.NewRow();
                                foreach (DataRow rowItem in dtProcoreFieldsMapping.Rows)
                                {

                                    switch (Convert.ToString(rowItem[DatabaseObjects.Columns.ProcoreColumnName]))
                                    {
                                        case "title":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.title;
                                               //// SubContractor.[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.title;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;
                                        case "number":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.number;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;
                                        case "Id":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.id;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;
                                        case "description":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.description;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;
                                        case "created_at":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.created_at;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;
                                        case "status":
                                            if (string.IsNullOrEmpty(Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue])))
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = item.status;
                                            }
                                            else
                                            {
                                                itemProcoreSubContractor[Convert.ToString(rowItem[DatabaseObjects.Columns.InternalColumnName])] = Convert.ToString(rowItem[DatabaseObjects.Columns.PreSetValue]);
                                            }
                                            break;

                                        default:
                                            break;
                                    }

                                }

                                // itemProcoreLookupsFieldData[DatabaseObjects.Columns.ProcoreUtilityLookup] = ddlProcoreUtility.SelectedValue;

                                Vendor comtvendor = item.vendor;

                                if (!string.IsNullOrEmpty(Convert.ToString(comtvendor.id)))
                                {
                                    var apiComapnyVendorUrl = string.Format("{2}vapid/vendors/{3}?access_token={0}&company_id={1}", ProcoreAPIToken.Trim(), ProcoreCompanyId, ProcoreBaseUrl.Trim(), comtvendor.id);

                                    var newCompanyVendorClient = new WebClient();
                                    var newComapnyVendorcontent = newCompanyVendorClient.DownloadString(apiComapnyVendorUrl);

                                    CompanyVendor lstCompanyVendor = JsonConvert.DeserializeObject<CompanyVendor>(newComapnyVendorcontent);

                                    itemProcoreSubContractor[DatabaseObjects.Columns.CompanyName] = lstCompanyVendor.name;
                                }

                                itemProcoreSubContractor[DatabaseObjects.Columns.ExternalProjectId] = splstItem[DatabaseObjects.Columns.ExternalProjectId];
                                itemProcoreSubContractor[DatabaseObjects.Columns.TicketId] = PublicTicketID;
                               //// itemProcoreSubContractor.Update();
                                counter++;
                            }
                            //                    }

                            lblMessage.Text = string.Format("{0} Records Synced.", counter);
                            lblMessage.Visible = true;
                            lblMessage.ForeColor = System.Drawing.Color.Blue;

                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                        ////Log.WriteException(ex, "ViewSubContrator- Procore SubContractor(Commitments,Vendors)");
                        lblMessage.Text = "Unable to connect with Procore.";
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Black;
                    }
                }

            }
            ////fieldSetPopupControl.ShowOnPageLoad = false;
            ////BindProcoreSubContractor();
        }

        protected void lnkbtnCancel_Click(object sender, EventArgs e)
        {
            fieldSetPopupControl.ShowOnPageLoad = false;
        }

        protected void ImgbtnSyncSubContractor_Click(object sender, ImageClickEventArgs e)
        {
            fieldSetPopupControl.ShowOnPageLoad = true;
        }
    }
}