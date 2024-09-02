using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Collections;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using System.Configuration;
using DevExpress.XtraPrinting;

namespace uGovernIT.Web
{
    public partial class ManualEscalation : UserControl
    {
        public string TicketId { get; set; }
        public string ModuleName { get; set; }
        string ticketToken = "[$TicketId$]";
        string titleToken = "[$Title$]";

        long ticketEmailID = 0;

        private string newAgentURL = UGITUtility.GetAbsoluteURL("/SitePages/ServiceWizard.aspx?isdlg=1&serviceID=");

        UserProfile user = null;
        UserProfileManager UserManager;

        DataRow spLItem;
        DataRow ticket;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager cvHelper = null;
        EscalationProcess objEscalationProcess = null;
        ModuleViewManager ObjModuleViewManager = null;
        TicketManager ObjTicketManager = null;
        EmailsManager ObjEmailsManager = null;
        StateManager stateManager = null;
        RelatedCompanyManager relatedCompanyManager = null;
        CRMRelationshipTypeManager crmRelationshipTypeManager = null;
        ProjectEstimatedAllocationManager crmProjectAllocationManager = null;
        GlobalRoleManager roleManager = null;

        protected override void OnInit(EventArgs e)
        {
            cvHelper = new ConfigurationVariableManager(context);
            objEscalationProcess = new EscalationProcess(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
            ObjEmailsManager = new EmailsManager(context);
            stateManager = new StateManager(context);
            relatedCompanyManager = new RelatedCompanyManager(context);
            crmRelationshipTypeManager = new CRMRelationshipTypeManager(context);
            crmProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            roleManager = new GlobalRoleManager(context);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //cvTO.ControlToValidate = pEditorTo.UserTokenBoxAdd.ID;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            user = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            if (!IsPostBack)
            {
                // Check if storing a copy of outgoing emails is enabled
                UGITModule moduleitem = ObjModuleViewManager.LoadByName(ModuleName);
                if (moduleitem != null && UGITUtility.StringToBoolean(moduleitem.StoreEmails))
                {
                    trTicketCopy.Visible = true;
                    chbTicketCopy.Checked = true;
                }
                else
                {
                    trTicketCopy.Visible = false;
                    chbTicketCopy.Checked = false;
                }

                TicketValidationCheck();
                txtEscalationEmail.ReadOnly = false;
                txtEscalationEmail.Enabled = true;
                revEmailTo.Enabled = true;
                tr9.Visible = true;

                if (Request["AlertType"] == "SVCTask")
                {
                    List<string> tickettaskIds = Request["tickettaskIds"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    lblEmailToActionUser.InnerText = "Mail to Action User";
                    cbIncludeActionUser.Visible = true;
                    lblActionUser.Visible = true;
                    lblActionUser.Text = "Task/Ticket Assignee or Approver";
                    string strbody = string.Empty;
                    tr5.Visible = false;
                    txtMailTOCC.Text = user.Email;

                    ((HtmlEditorControl)htmlEditor).Html = string.Format("<br/>[Task/Ticket links will be inserted here]");

                    txtMailSubject.Text = string.Format("[Task/Ticket] Needs Action");
                    trattachment.Visible = false;
                }
                else if (Request["sendresume"] != null)
                {
                    tr5.Visible = false;
                    htmlEditor.Visible = false;
                    EmailHtmlBody.Visible = true;
                    EmailHtmlBody.Html = Request["userbody"].ToString();
                    txtMailSubject.Text = Request["usersubject"].ToString();
                    
                }
                else if (!string.IsNullOrEmpty(TicketId))
                {
                    List<string> ticketIds = TicketId.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    txtMailTOCC.Text = user.Email;

                    if (Request["EmailAlert"] == "True")
                    {
                        lblEmailToActionUser.InnerText = "Mail to Action User";
                        cbIncludeActionUser.Visible = false;
                        lblActionUser.Visible = true;
                        string strbody = string.Empty;
                        tr5.Visible = false;

                        StringBuilder emailBody = new StringBuilder();
                        ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                        mRequest.UserID = user.Id;
                        Enum myColors = CustomFilterTab.AllTickets;
                        //mRequest.CurrentTab = CustomFilterTab.AllTickets;
                        mRequest.ModuleName = ModuleName;
                        ModuleStatistics moduleStats = new ModuleStatistics(context);
                        ModuleStatisticResponse stat = moduleStats.Load(mRequest);

                        htmlEditor.Visible = false;
                        EmailHtmlBody.Visible = true;
                        emailBody.AppendFormat("Hello,");

                        if (ModuleName == "CPR" || ModuleName == "CNS")
                        {
                            foreach (var ticketid in ticketIds)
                            {
                                if (stat.ResultedData != null)
                                {
                                    DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketid));
                                    if (drs != null && drs.Length > 0)
                                    {
                                        emailBody.AppendFormat("<br><br>");
                                        lblEmailToActionUser.InnerText = "Mail to Action User";

                                        if (!string.IsNullOrEmpty(Convert.ToString(drs[0][DatabaseObjects.Columns.CRMProjectID])))
                                            emailBody.AppendFormat("<b>Project No: {0} </b><br>", drs[0][DatabaseObjects.Columns.CRMProjectID]);
                                        else
                                            emailBody.AppendFormat("<b>Estimate No: {0} </b><br>", drs[0][DatabaseObjects.Columns.EstimateNo]);

                                        emailBody.AppendFormat("Project Name: {0} <br>", drs[0][DatabaseObjects.Columns.Title]);
                                        emailBody.AppendFormat("Address: {0} <br>", drs[0][DatabaseObjects.Columns.Address]);
                                        emailBody.AppendFormat("Contract Amount: {0} <br><br>", drs[0][DatabaseObjects.Columns.ApproxContractValue]);
                                    }
                                }
                            }

                            emailBody.AppendFormat("[Your content goes here]");
                            emailBody.AppendFormat("<br><br>Thanks,<br>Precon Team<br>");

                            EmailHtmlBody.Html = emailBody.ToString();
                            emailBody.Clear();
                            txtMailSubject.Text = string.Format("Project Email");
                        }
                        else if (ModuleName == "OPM")
                        {
                            foreach (var ticketid in ticketIds)
                            {
                                if (stat.ResultedData != null)
                                {
                                    DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketid));
                                    if (drs != null && drs.Length > 0)
                                    {
                                        emailBody.AppendFormat("<br><br>");
                                        emailBody.AppendFormat("<b>Opportunity No: {0} </b><br>", drs[0][DatabaseObjects.Columns.TicketId]);
                                        emailBody.AppendFormat("Opportunity Name: {0} <br>", drs[0][DatabaseObjects.Columns.Title]);

                                        List<string> lstAddress = new List<string>();
                                        string address = string.Empty;

                                        if (!string.IsNullOrEmpty(Convert.ToString(drs[0][DatabaseObjects.Columns.StreetAddress1])))
                                            lstAddress.Add(Convert.ToString(drs[0][DatabaseObjects.Columns.StreetAddress1]));

                                        if (!string.IsNullOrEmpty(Convert.ToString(drs[0][DatabaseObjects.Columns.City])))
                                            lstAddress.Add(Convert.ToString(drs[0][DatabaseObjects.Columns.City]));

                                        if (UGITUtility.IsSPItemExist(drs[0], DatabaseObjects.Columns.StateLookup) && !string.IsNullOrEmpty(Convert.ToString(drs[0][DatabaseObjects.Columns.StateLookup])))
                                        {
                                            var State = stateManager.LoadByID(Convert.ToInt64(drs[0][DatabaseObjects.Columns.StateLookup]));
                                            if (State != null)
                                            {
                                                lstAddress.Add(State.Title);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(Convert.ToString(drs[0][DatabaseObjects.Columns.Zip])))
                                            lstAddress.Add(Convert.ToString(drs[0][DatabaseObjects.Columns.Zip]));

                                        address = string.Join(", ", lstAddress);

                                        emailBody.AppendFormat("Address: {0} <br>", address);
                                        emailBody.AppendFormat("Contract Amount: {0:C0} <br><br>", drs[0][DatabaseObjects.Columns.ApproxContractValue]);
                                    }
                                }
                            }

                            emailBody.AppendFormat("[Your content goes here]");
                            emailBody.AppendFormat("<br><br>Thanks,<br>Opportunity Team<br>");

                            EmailHtmlBody.Html = emailBody.ToString();
                            emailBody.Clear();
                            txtMailSubject.Text = string.Format("Opportunity Email");
                        }
                        else if (ModuleName == "LEM")
                        {
                            foreach (var ticketid in ticketIds)
                            {
                                if (stat.ResultedData != null)
                                {
                                    DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketid));
                                    if (drs != null && drs.Length > 0)
                                    {
                                        emailBody.AppendFormat("<br><br>");
                                        emailBody.AppendFormat("<b>Lead No: {0} </b><br>", drs[0][DatabaseObjects.Columns.TicketId]);
                                        emailBody.AppendFormat("Lead Name: {0} <br>", drs[0][DatabaseObjects.Columns.Title]);
                                        emailBody.AppendFormat("Address: {0} <br>", drs[0][DatabaseObjects.Columns.Address]);
                                        emailBody.AppendFormat("Contract Amount: {0:C0} <br><br>", drs[0][DatabaseObjects.Columns.ContractValue]);
                                    }
                                }
                            }

                            emailBody.AppendFormat("[Your content goes here]");
                            emailBody.AppendFormat("<br><br>Thanks,<br>Lead Team<br>");

                            EmailHtmlBody.Html = emailBody.ToString();
                            emailBody.Clear();
                            txtMailSubject.Text = string.Format("Lead Email");
                        }
                        else if (ModuleName == "CPP")
                        {
                        }
                        else if (ModuleName == "CCM")
                        {
                        }

                        if (ticketIds.Count == 1)
                            txtMailSubject.Text = string.Format("{0} Needs Action: {1}", ticketIds[0], Request["StageTitle"]);
                        else
                            txtMailSubject.Text = string.Format("[Ticket ID] Needs Action: {0}", Request["StageTitle"]);

                        /*
                        foreach (var ticketid in ticketIds)
                        {
                            string title = string.Empty;
                            if (stat.ResultedData != null)
                            {
                                DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketid));
                                if (drs != null && drs.Length > 0)
                                {
                                    title = Convert.ToString(drs[0][DatabaseObjects.Columns.Title]);
                                }
                            }

                            string strinURL = string.Format("{0}?TicketId={1}&ModuleName={2}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), ticketid, ModuleName);
                            strbody += string.Format("<a href={1}>{0}: {2}</a><br/>", ticketid, strinURL, title);
                        }

                        ((HtmlEditorControl)htmlEditor).Html = string.Format(@"Hello, <br/><br/> The following {1} needs action:<br/> {0}<br/><br/>Thanks,<br/>Administrator",
                                                                             strbody, UGITUtility.moduleTypeName(ModuleName).ToLower());

                        DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                        Ticket tkt = new Ticket(context, ModuleName);
                        hdnActionUser.Value = tkt.GetStageActionUsers(Request["StageTitle"], ModuleName);
                        string[] delim = { Constants.Separator };
                        string[] arrActionUser = hdnActionUser.Value.Split(delim, StringSplitOptions.None);

                        foreach (var item in arrActionUser)
                        {
                            DataRow[] dataRows = dt.Select(string.Format("{0}='{1}' AND {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleName, DatabaseObjects.Columns.ColumnName, item));

                            if (!string.IsNullOrEmpty(lblActionUser.Text))
                            {
                                lblActionUser.Text += Constants.UserInfoSeparator;
                            }

                            if (dataRows != null && dataRows.Length > 0)
                            {
                                lblActionUser.Text += Convert.ToString(dataRows[0][DatabaseObjects.Columns.UserTypes]);
                            }
                            else
                                lblActionUser.Text += item;
                        }

                        UGITModule moduleRow = ObjModuleViewManager.LoadByName(ModuleName);
                        if (moduleRow != null)
                            txtMailSubject.Text = string.Format("{0} Needs Action: {1}", moduleRow.Title, Request["StageTitle"]);

                        trattachment.Visible = false;

                        */
                    }
                    else if (Request["Notification"] == "initialemail")
                    {
                        tr5.Visible = false;
                        trUserTo.Visible = true;
                        trMailToCC.Visible = true;
                        StringBuilder emailBody = new StringBuilder();
                        htmlEditor.Visible = false;
                        EmailHtmlBody.Visible = true;
                        emailBody.AppendFormat("Hello,");
                        if (ModuleName == "OPM")
                        {
                            foreach (var ticketid in ticketIds)
                            {
                                DataTable dtresult = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $"{DatabaseObjects.Columns.TicketId} = '{ticketid}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                                if (dtresult != null && dtresult.Rows.Count > 0)
                                {
                                    emailBody.AppendFormat("<br><br>");

                                    string strProposalRecipient = string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalRecipient])) ? string.Empty : string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalRecipient])));
                                    emailBody.AppendFormat(string.Format("Please be advised that the following e-proposal is due {0} (to {1}) and hard copies are due {2}.", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).ToString("MM/dd/yyyy"), strProposalRecipient, Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).ToString("MM/dd/yyyy")));

                                    emailBody.AppendFormat("<br><br>");

                                    emailBody.AppendFormat("<b>Opportunity No: {0} </b><br>", dtresult.Rows[0][DatabaseObjects.Columns.TicketId]);
                                    emailBody.AppendFormat("Opportunity Name: {0} <br>", dtresult.Rows[0][DatabaseObjects.Columns.Title]);


                                    List<string> lstAddress = new List<string>();
                                    string address = string.Empty;

                                    if (!string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.StreetAddress1])))
                                        lstAddress.Add(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.StreetAddress1]));

                                    if (!string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.City])))
                                        lstAddress.Add(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.City]));

                                    if (UGITUtility.IfColumnExists(dtresult.Rows[0], DatabaseObjects.Columns.StateLookup) && !string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.StateLookup])))
                                    {
                                        var State = stateManager.LoadByID(Convert.ToInt64(dtresult.Rows[0][DatabaseObjects.Columns.StateLookup]));
                                        if (State != null)
                                        {
                                            lstAddress.Add(State.Title);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.Zip])))
                                        lstAddress.Add(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.Zip]));

                                    address = string.Join(", ", lstAddress);

                                    emailBody.AppendFormat("Address: {0} <br>", address);

                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("Square Feet: {0:N0} <br>", dtresult.Rows[0][DatabaseObjects.Columns.UsableSqFt]);
                                    emailBody.AppendFormat("Contract Value: {0:C0} <br>", dtresult.Rows[0][DatabaseObjects.Columns.ApproxContractValue]);

                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("<b>Schedule </b><br>");
                                    emailBody.AppendFormat("• {0} Job Walk <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.JobWalkthroughDate]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.JobWalkthroughDate]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Questions Due <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.QuestionDueDate]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.QuestionDueDate]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Proposal E-Copy Due <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Proposal Hard Copy Due <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.HardCopyDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.HardCopyDeadline]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Interviews <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.InterviewDate]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.InterviewDate]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Project Award <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.DecisionDate]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.DecisionDate]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Construction Start <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.EstimatedConstructionStart]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.EstimatedConstructionStart]).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("• {0} Projected Completion <br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.EstimatedConstructionEnd]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.EstimatedConstructionEnd]).ToString("MM/dd/yyyy"));

                                    //string strCRMCompanyTitleLookup = string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.CRMCompanyTitleLookup])) ? string.Empty : string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.CRMCompanyTitleLookup])));
                                    string strCRMCompanyLookup = string.IsNullOrEmpty(Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.CRMCompanyLookup])) ? string.Empty : Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.CRMCompanyLookup]), context.TenantID));

                                    string strConstructionManagementCompany = string.Empty;
                                    string strCompanyArchitect = string.Empty;
                                    string strCompanyEngineer = string.Empty;
                                    string strBroker = string.Empty;

                                    string strProjectTeamLookup = string.Empty;
                                    string strConstructionManagerContact = string.Empty;
                                    string strArchitect = string.Empty;
                                    string strEngineerContact = string.Empty;
                                    string strBrokerContact = string.Empty;

                                    List<RelatedCompany> collection = relatedCompanyManager.Load(x => x.TicketID == TicketId);
                                    //DataTable dtRelatedCompanies = null;

                                    if (collection != null && collection.Count > 0)
                                    {
                                        //dtRelatedCompanies = collection.GetDataTable();
                                        var relationshipType = crmRelationshipTypeManager.Load();
                                        if (relationshipType != null && relationshipType.Count() > 0)
                                        {
                                            //var result = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.RelationshipTypeLookup).Contains("Engineer"));
                                            var result = (from c in collection
                                                          join r in relationshipType on c.RelationshipTypeLookup equals r.ID
                                                          where r.Deleted != true && r.Title.Contains("Engineer")
                                                          select c
                                                          ).FirstOrDefault();

                                            if (result != null)
                                            {
                                                //strCompanyEngineer = Convert.ToString(result[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                                //strEngineerContact = Convert.ToString(result[DatabaseObjects.Columns.ContactLookup]);

                                                strCompanyEngineer = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(result.CRMCompanyLookup), context.TenantID));
                                                strEngineerContact = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMContact, DatabaseObjects.Columns.Title, Convert.ToString(result.ContactLookup), context.TenantID));
                                            }

                                            //result = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.RelationshipTypeLookup).Contains(DatabaseObjects.Columns.Architect));
                                            result = (from c in collection
                                                      join r in relationshipType on c.RelationshipTypeLookup equals r.ID
                                                      where r.Deleted != true && r.Title.Contains(DatabaseObjects.Columns.Architect)
                                                      select c
                                                    ).FirstOrDefault();

                                            if (result != null)
                                            {
                                                //strCompanyArchitect = Convert.ToString(result[DatabaseObjects.Columns.CRMCompanyLookup]);
                                                //strArchitect = Convert.ToString(result[DatabaseObjects.Columns.ContactLookup]);

                                                strCompanyArchitect = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(result.CRMCompanyLookup), context.TenantID));
                                                strArchitect = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMContact, DatabaseObjects.Columns.Title, Convert.ToString(result.ContactLookup), context.TenantID));
                                            }

                                            //result = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.RelationshipTypeLookup).Contains("Construction"));
                                            result = (from c in collection
                                                      join r in relationshipType on c.RelationshipTypeLookup equals r.ID
                                                      where  r.Deleted != true && r.Title.Contains("Construction")
                                                      select c
                                                    ).FirstOrDefault();

                                            if (result != null)
                                            {
                                                //strConstructionManagementCompany = Convert.ToString(result[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                                //strConstructionManagerContact = Convert.ToString(result[DatabaseObjects.Columns.ContactLookup]);

                                                strConstructionManagementCompany = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(result.CRMCompanyLookup), context.TenantID));
                                                strConstructionManagerContact = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMContact, DatabaseObjects.Columns.Title, Convert.ToString(result.ContactLookup), context.TenantID));
                                            }

                                            //result = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.RelationshipTypeLookup).Contains("Broker"));
                                            result = (from c in collection
                                                      join r in relationshipType on c.RelationshipTypeLookup equals r.ID
                                                      where r.Deleted != true && r.Title.Contains("Broker")
                                                      select c
                                                    ).FirstOrDefault();

                                            if (result != null)
                                            {
                                                //strBroker = Convert.ToString(result[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                                //strBrokerContact = Convert.ToString(result[DatabaseObjects.Columns.ContactLookup]);

                                                strBroker = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(result.CRMCompanyLookup), context.TenantID));
                                                strBrokerContact = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMContact, DatabaseObjects.Columns.Title, Convert.ToString(result.ContactLookup), context.TenantID));
                                            }

                                            //result = dtRelatedCompanies.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.RelationshipTypeLookup) == string.Empty);
                                            result = (from c in collection
                                                      join r in relationshipType on c.RelationshipTypeLookup equals r.ID
                                                      where  r.Deleted != true && c.RelationshipTypeLookup == null
                                                      select c
                                                    ).FirstOrDefault();

                                            if (result != null)
                                            {
                                                //strProjectTeamLookup = Convert.ToString(result[DatabaseObjects.Columns.ContactLookup]);
                                                strProjectTeamLookup = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMContact, DatabaseObjects.Columns.Title, Convert.ToString(result.ContactLookup), context.TenantID));
                                            }
                                        }
                                    }

                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("<b>External Team </b><br>");
                                    emailBody.AppendFormat("• Client: {0} - {1} <br>", strCRMCompanyLookup, strProjectTeamLookup);
                                    emailBody.AppendFormat("• Construction Manager: {0} - {1}<br>", strConstructionManagementCompany, strConstructionManagerContact);
                                    emailBody.AppendFormat("• Architect: {0} - {1}<br>", strCompanyArchitect, strArchitect);
                                    emailBody.AppendFormat("• Engineer: {0} - {1}<br>", strCompanyEngineer, strEngineerContact);
                                    emailBody.AppendFormat("• Broker: {0} - {1}<br>", strBroker, strBrokerContact);


                                    //query for get the project internal team.
                                    //queryInternalTeam.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, ticketid);
                                    //DataTable dtProjectInternalTeam = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMProjectAllocation, queryInternalTeam);

                                    List<ProjectEstimatedAllocation> dtProjectInternalTeam = crmProjectAllocationManager.Load(x => x.TicketId == TicketId);
                                    List<GlobalRole> roles = roleManager.Load();
                                    string strBCCIPD = string.Empty;
                                    string strBCCIPM = string.Empty;
                                    string strBCCIEst = string.Empty;
                                    string strBCCISupt = string.Empty;

                                    if (dtProjectInternalTeam != null && dtProjectInternalTeam.Count() > 0 && roles != null && roles.Count > 0)
                                    {

                                        var drPD = (from it in dtProjectInternalTeam
                                                    join r in roles on it.Type equals r.Id
                                                    where it.Deleted != true  && r.Deleted != true && r.Name.Equals("Project Executive", StringComparison.InvariantCultureIgnoreCase)
                                                    select it.AssignedTo).ToList();

                                        var drPM = (from it in dtProjectInternalTeam
                                                    join r in roles on it.Type equals r.Id
                                                    where it.Deleted != true  && r.Deleted != true && r.Name.Equals("Project Manager", StringComparison.InvariantCultureIgnoreCase)
                                                    select it.AssignedTo).ToList();


                                        var drEst = (from it in dtProjectInternalTeam
                                                     join r in roles on it.Type equals r.Id
                                                     where it.Deleted != true  && r.Deleted != true && r.Name.Equals("Estimator", StringComparison.InvariantCultureIgnoreCase)
                                                     select it.AssignedTo).ToList();


                                        var drSupt = (from it in dtProjectInternalTeam
                                                      join r in roles on it.Type equals r.Id
                                                      where it.Deleted != true  && r.Deleted != true && r.Name.Equals("Superintendent", StringComparison.InvariantCultureIgnoreCase)
                                                      select it.AssignedTo).ToList();

                                        if (drPD != null && drPD.Count() > 0)
                                        {
                                            //foreach (DataRow ritem in drPD)
                                            //{
                                            //    if (!string.IsNullOrEmpty(strBCCIPD))
                                            //        strBCCIPD += ",";

                                            //    if (Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo]) != null)
                                            //        strBCCIPD += string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo])));
                                            //}

                                            strBCCIPD = UserManager.GetUserOrGroupName(drPD);
                                            if (!string.IsNullOrEmpty(strBCCIPD))
                                                strBCCIPD = strBCCIPD.Replace(Constants.Separator5, ", ");
                                        }


                                        if (drPM != null && drPM.Count() > 0)
                                        {
                                            //foreach (DataRow ritem in drPM)
                                            //{
                                            //    if (!string.IsNullOrEmpty(strBCCIPM))
                                            //        strBCCIPM += ",";

                                            //    if (Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo]) != null)
                                            //        strBCCIPM += string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo])));
                                            //}

                                            strBCCIPM = UserManager.GetUserOrGroupName(drPM);
                                            if (!string.IsNullOrEmpty(strBCCIPM))
                                                strBCCIPM = strBCCIPM.Replace(Constants.Separator5, ", ");
                                        }


                                        if (drEst != null && drEst.Count() > 0)
                                        {
                                            //foreach (DataRow ritem in drEst)
                                            //{
                                            //    if (!string.IsNullOrEmpty(strBCCIEst))
                                            //        strBCCIEst += ",";

                                            //    if (Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo]) != null)
                                            //        strBCCIEst += string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo])));
                                            //}
                                            strBCCIEst = UserManager.GetUserOrGroupName(drEst);
                                            if (!string.IsNullOrEmpty(strBCCIEst))
                                                strBCCIEst = strBCCIEst.Replace(Constants.Separator5, ", ");
                                        }

                                        if (drSupt != null && drSupt.Count() > 0)
                                        {
                                            //foreach (DataRow ritem in drSupt)
                                            //{
                                            //    if (!string.IsNullOrEmpty(strBCCISupt))
                                            //        strBCCISupt += ",";

                                            //    if (Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo]) != null)
                                            //        strBCCISupt += string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(ritem[DatabaseObjects.Columns.UGITAssignedTo])));
                                            //}

                                            strBCCISupt = UserManager.GetUserOrGroupName(drSupt);
                                            if (!string.IsNullOrEmpty(strBCCISupt))
                                                strBCCISupt = strBCCISupt.Replace(Constants.Separator5, ", ");
                                        }
                                    }


                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("<b>Internal Team </b><br>");

                                    string CompanyEmailKey = cvHelper.GetValue("CompanyEmailKey");
                                    emailBody.AppendFormat("<b>• {1} PD:</b> {0} <br>", strBCCIPD, CompanyEmailKey);
                                    emailBody.AppendFormat("<b>• {1} PM:</b> {0}<br>", strBCCIPM, CompanyEmailKey);
                                    emailBody.AppendFormat("<b>• {1} Est:</b> {0}<br>", strBCCIEst, CompanyEmailKey);
                                    emailBody.AppendFormat("<b>• {1} Supt:</b> {0}<br>", strBCCISupt, CompanyEmailKey);

                                    emailBody.AppendFormat("<b>*Please confirm team members by {0} at 12pm.</b><br>", DateTime.Now.AddDays(1).ToString("MM/dd/yyyy"));

                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("<b>Project Description</b><br>");
                                    emailBody.AppendFormat("{0}", dtresult.Rows[0][DatabaseObjects.Columns.TicketDescription]);

                                    emailBody.AppendFormat("<br><br>");
                                    emailBody.AppendFormat("<b>Deadlines </b><br>");

                                    emailBody.AppendFormat("<b>• Schedule (reviewed and formatted)</b> is due to Marketing by {0} at 3pm.<br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).AddDays(-2).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("<b>• Logistics Plan</b> is due to Marketing by {0} at 3pm.<br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).AddDays(-2).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("<b>• GC and Fees</b>  are due to Marketing by {0} at 10am.<br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).AddDays(-1).ToString("MM/dd/yyyy"));
                                    emailBody.AppendFormat("<b>• Budget</b> is due to Marketing by {0} at 10am.<br>", Convert.ToString(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]) == string.Empty ? string.Empty : Convert.ToDateTime(dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline]).ToString("MM/dd/yyyy"));

                                }
                                emailBody.AppendFormat("<br><br>Thanks,<br>Opportunity Team<br>");

                                EmailHtmlBody.Html = emailBody.ToString();
                                emailBody.Clear();
                                txtMailSubject.Text = string.Format("RFP due {0:MM/dd/yyyy}: {1}", dtresult.Rows[0][DatabaseObjects.Columns.ProposalDeadline], dtresult.Rows[0][DatabaseObjects.Columns.Title]);
                            }
                        }
                    }
                    else
                    {
                        if (Request["EmailAlert"] == "True")
                        {
                            lblEmailToActionUser.InnerText = "Mail to Action User";
                            cbIncludeActionUser.Visible = false;
                            lblActionUser.Visible = true;
                            string strbody = string.Empty;
                            tr5.Visible = false;

                            //StringBuilder emailBody = new StringBuilder();
                            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                            mRequest.UserID = UserManager.GetUserByUserName(user.Name).Id;
                            //Enum myColors = CustomFilterTab.AllTickets;                            
                            mRequest.ModuleName = ModuleName;
                            ModuleStatistics moduleStats = new ModuleStatistics(context);
                            ModuleStatisticResponse stat = moduleStats.Load(mRequest);

                            foreach (var ticketid in ticketIds)
                            {
                                string title = string.Empty;
                                if (stat.ResultedData != null)
                                {
                                    DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketid));
                                    if (drs != null && drs.Length > 0)
                                    {
                                        title = Convert.ToString(drs[0][DatabaseObjects.Columns.Title]);
                                    }
                                }
                                string strinURL = string.Format("{3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath),
                                                                    ticketid, ModuleName, ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), context.TenantID);
                                strbody += string.Format("<a href={1}>{0}: {2}</a><br/>", ticketid, strinURL, title);
                            }

                            ((HtmlEditorControl)htmlEditor).Html = string.Format(@"Hello, <br/><br/> The following {1} needs action:<br/> {0}<br/><br/>Thanks,<br/>Administrator",
                                                                             strbody, UGITUtility.moduleTypeName(ModuleName).ToLower());

                            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                            Ticket tkt = new Ticket(context, ModuleName);
                            hdnActionUser.Value = tkt.GetStageActionUsers(Request["StageTitle"], ModuleName);
                            string[] delim = { Constants.Separator };
                            string[] arrActionUser = hdnActionUser.Value.Split(delim, StringSplitOptions.None);

                            foreach (var item in arrActionUser)
                            {
                                DataRow[] dataRows = dt.Select(string.Format("{0}='{1}' AND {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleName, DatabaseObjects.Columns.ColumnName, item));

                                if (!string.IsNullOrEmpty(lblActionUser.Text))
                                {
                                    lblActionUser.Text += Constants.UserInfoSeparator;
                                }

                                if (dataRows != null && dataRows.Length > 0)
                                {
                                    lblActionUser.Text += Convert.ToString(dataRows[0][DatabaseObjects.Columns.UserTypes]);
                                }
                                else
                                    lblActionUser.Text += item;
                            }

                            UGITModule moduleRow = ObjModuleViewManager.LoadByName(ModuleName);
                            if (moduleRow != null)
                                txtMailSubject.Text = string.Format("{0} Needs Action: {1}", moduleRow.Title, Request["StageTitle"]);

                            trattachment.Visible = false;
                        }
                        #region sendagentlink
                        else if (Request["sendagentlink"] != null)
                        {
                            string agentID = Convert.ToString(Request["agentID"]);
                            string moduleName = Convert.ToString(Request["ModuleName"]);
                            StringBuilder emailBody = new StringBuilder();
                            htmlEditor.Visible = false;
                            EmailHtmlBody.Visible = true;
                            tr9.Visible = false;
                            trUserTo.Visible = false;
                            trMailToCC.Visible = false;
                            tr5.Visible = true;

                            foreach (string ticketId in ticketIds)
                            {
                                DataRow currentTicketItem = ObjTicketManager.GetTicketTableBasedOnTicketId(ModuleName, ticketId).Rows[0];
                                if (currentTicketItem == null)
                                    continue;
                                if (txtEscalationEmail.Text.IndexOf(Convert.ToString(currentTicketItem[DatabaseObjects.Columns.TicketStageActionUsers])) == -1)
                                    txtEscalationEmail.Text += Convert.ToString(currentTicketItem[DatabaseObjects.Columns.TicketStageActionUsers]) + "; ";
                            }

                            txtEscalationEmail.ReadOnly = true;
                            revEmailTo.Enabled = false;
                            txtEscalationEmail.Enabled = false;
                            //Services agentService = Services.LoadAllAgents().FirstOrDefault(x => x.ID == Convert.ToInt32(agentID) && x.Activated == true);
                            //if (agentService != null)
                            //{
                            //    string param = string.Format("&ModuleName={0}&TicketId={1}", moduleName, String.Join(";", ticketIds.ToArray()));
                            //    string ticketType = uHelper.moduleTypeName(ModuleName);
                            //    newAgentURL = string.Format("{0}{1}{2}", newAgentURL, Convert.ToString(agentService.ID), param);
                            //    hdnAgentUrl.Value = newAgentURL;
                            //    emailBody.AppendFormat("Hi, <br/><br/> You are being contacted to collect data and/or approval for {0} <strong>{1}</strong>.<br/><br/>", ticketType.ToLower(), ticketToken);
                            //    emailBody.AppendFormat("<a href='{1}'>Please click here to enter the required data</a><br><br>Thanks,<br>{0}<br>", SPContext.Current.Web.CurrentUser.Name, newAgentURL);
                            //    EmailHtmlBody.Html = emailBody.ToString();
                            //    emailBody.Clear();
                            //}

                            txtMailSubject.Text = string.Format("Agent for {0}", TicketId);
                        }
                        #endregion
                        else
                        {
                            tr5.Visible = false;

                            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                            // mRequest.SPWebObj = SPContext.Current.Web;
                            mRequest.UserID = user.Id;
                            mRequest.CurrentTab = CustomFilterTab.AllTickets.ToString();
                            mRequest.ModuleName = ModuleName;
                            ModuleStatistics moduleStats = new ModuleStatistics(context);
                            ModuleStatisticResponse stat = moduleStats.Load(mRequest);


                            StringBuilder emailBody = new StringBuilder();
                            string ticketType = UGITUtility.moduleTypeName(ModuleName);
                            htmlEditor.Visible = false;
                            EmailHtmlBody.Visible = true;
                            if (ticketIds.Count == 1)
                            {
                                ticketToken = ticketIds[0];

                                if (!string.IsNullOrEmpty(ticketToken))
                                {
                                    DataRow[] drs = stat.ResultedData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketToken));
                                    if (drs != null && drs.Length > 0)
                                    {
                                        titleToken = Convert.ToString(drs[0][DatabaseObjects.Columns.Title]);
                                    }
                                }
                            }

                            emailBody.AppendFormat("Hi, <br/><br/> You are being contacted in regards to {0} <strong>{1}</strong>.<br/><br/>", ticketType.ToLower(), ticketToken);
                            emailBody.AppendFormat("[Your content goes here]<br><br>Thanks,<br>{0}<br>", user.Name);
                            EmailHtmlBody.Html = emailBody.ToString();

                            //txtMailSubject.Text = string.Format("{0} escalation", ticketToken);
                            txtMailSubject.Text = string.Format("{0}: {1}", TicketId, titleToken);
                        }
                    }
                }
            }
        }

        protected void chklstEscalationRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hdnEscalationRules.Value = string.Empty;
            //if (chklstEscalationRules.SelectedIndex > -1)
            //{
            //    foreach (ListItem item in chklstEscalationRules.Items)
            //    {
            //        if (item.Selected)
            //        {
            //            hdnEscalationRules.Value = hdnEscalationRules.Value.Trim() + (String.IsNullOrEmpty(hdnEscalationRules.Value.Trim()) ? "" : Constants.Separator) + item.Value;
            //        }
            //    }
            //}
        }

        private void BindEscalationRole()
        {
            //chklstEscalationRules.Items.Clear();

            ///in Escalation modules are not specified
            ///to get module need to load SLA Rules.
            DataTable spSLARules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            //SPListItem spSLARulesItem = SPListHelper.GetSPListItem(spSLARules, Convert.ToInt32(ddlSLARule.SelectedValue));
            //SPFieldLookupValue spvalue = new SPFieldLookupValue(Convert.ToString(spSLARulesItem[DatabaseObjects.Columns.ModuleNameLookup]));
            string moduleName = "TSR";

            DataTable spListUserEmail = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataTable dtUserEmail = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataRow[] rows = dtUserEmail.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            if (rows.Length > 0)
            {
                foreach (DataRow dr in rows)
                {
                    // chklstEscalationRules.Items.Add(new ListItem(Convert.ToString(dr[DatabaseObjects.Columns.UserTypes]), Convert.ToString(dr[DatabaseObjects.Columns.ColumnName])));
                }
            }
            //chklstEscalationRules.Items.Add(new ListItem("Escalation Manager", "RequestTypeEscalationManager"));
            //chklstEscalationRules.Items.Add(new ListItem("Backup Escalation Manager", "RequestTypeBackupEscalationManager"));
            //chklstEscalationRules.Items.Add(new ListItem("PRP Manager", "PRPManager"));
            //chklstEscalationRules.Items.Add(new ListItem("ORP Manager", "ORPManager"));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            bool isHtmlBody = true;
            bool disableTicketLink = false;

            if (chkDisableTicketLink.Checked)
                disableTicketLink = true;

            if (chkPlainText.Checked)
                isHtmlBody = false;

            if (Request["AlertType"] == "SVCTask")
            {
                List<string> tickettaskIds = Request["tickettaskIds"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                UGITTask moduleInstDepenedency = null;
                UGITTaskManager ModuleTaskManager = new UGITTaskManager(context);

                foreach (string taskid in tickettaskIds)
                {
                    //ModuleInstanceDependency moduleInstDepenedency = ModuleInstanceDependency.LoadByID(context, UGITUtility.StringToInt(taskid));
                    moduleInstDepenedency = ModuleTaskManager.LoadByID(UGITUtility.StringToInt(taskid));

                    if (Request["EmailAlert"] == "True")
                    {
                        List<string> lstUserNameWithEmail = GetUserNameWithEmailId();

                        StringBuilder userNameTo = new StringBuilder(lstUserNameWithEmail[0]);
                        StringBuilder userEmailTo = new StringBuilder(lstUserNameWithEmail[1]); //GetUserEmailId();

                        //SPListItem project = Ticket.getCurrentTicket("SVC", moduleInstDepenedency.ParentInstance);
                        DataRow project = Ticket.GetCurrentTicket(context, "SVC", moduleInstDepenedency.TicketId);

                        StringBuilder mailCCList = new StringBuilder();
                        string[] separator = { Constants.UserInfoSeparator };

                        //UserProfile.UsersInfo emailUsers = new UserProfile.UsersInfo();
                        List<UserProfile> emailUsers = new List<UserProfile>();

                        //SPWeb _spWeb = null;
                        if (project == null)
                            return;
                        //_spWeb = project.Web;

                        string moduleName = moduleInstDepenedency.ModuleNameLookup; //string.Empty;
                        bool IsApprovalRequired = false;
                        DataRow childinstance = null;
                        if (cbIncludeActionUser.Checked)
                        {
                            if (moduleInstDepenedency == null)
                                continue;

                            //if (moduleInstDepenedency.TaskType != "Ticket")
                            if (moduleInstDepenedency.Behaviour != "Ticket")
                            {
                                if (moduleInstDepenedency.Status == "pending" && !String.IsNullOrEmpty(moduleInstDepenedency.Approver))
                                {
                                    IsApprovalRequired = true;
                                    emailUsers = UserManager.GetUserInfosById(moduleInstDepenedency.Approver); //UserProfile.GetUserInfo(moduleInstDepenedency.Approver, _spWeb);
                                }
                                //else if (!IsApprovalRequired && moduleInstDepenedency.AssignedTo != null && moduleInstDepenedency.AssignedTo.Count > 0)
                                else if (!IsApprovalRequired && !String.IsNullOrEmpty(moduleInstDepenedency.AssignedTo))
                                {
                                    emailUsers = UserManager.GetUserInfosById(moduleInstDepenedency.AssignedTo); //UserProfile.GetUserInfo(moduleInstDepenedency.AssignedTo, _spWeb);
                                }
                            }
                            else
                            {
                                //moduleName = uHelper.getModuleNameByTicketId(moduleInstDepenedency.ChildInstance);
                                childinstance = Ticket.GetCurrentTicket(context, moduleInstDepenedency.ModuleNameLookup, moduleInstDepenedency.ChildInstance);
                                if (childinstance == null)
                                    continue;

                                //if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketStageActionUsers, childinstance.ParentList) && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketStageActionUserTypes, childinstance.ParentList))
                                //IsSPItemExist
                                if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketStageActionUsers) && UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketStageActionUserTypes))
                                {
                                    //string[] actionUserTypes = UGITUtility.SplitString(Convert.ToString(childinstance[DatabaseObjects.Columns.TicketStageActionUserTypes]), Constants.Separator);
                                    //emailUsers = UserProfile.GetUserInfo(childinstance, actionUserTypes, _spWeb);
                                    emailUsers = UserManager.GetUserInfosById(Convert.ToString(project[DatabaseObjects.Columns.TicketStageActionUsers]));
                                }
                            }
                        }

                        //Common Code for all
                        //if (emailUsers != null && !string.IsNullOrEmpty(emailUsers.userEmails))
                        if (emailUsers != null && emailUsers.Count > 0)
                        {
                            //string[] actionUserEmails = emailUsers.userEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            //for (int i = 0; i < actionUserEmails.Length; i++)
                            for (int i = 0; i < emailUsers.Count; i++)
                            {
                                //string actionUser = Convert.ToString(actionUserEmails[i]);
                                string actionUser = Convert.ToString(emailUsers[i].Email);
                                if (actionUser != string.Empty)
                                {
                                    if (mailCCList.Length != 0)
                                        mailCCList.Append(";");
                                    mailCCList.Append(actionUser);
                                }

                                string actionUsername = Convert.ToString(emailUsers[i].UserName);
                                if (actionUsername != string.Empty)
                                {
                                    if (userNameTo.Length != 0)
                                        userNameTo.Append(";");
                                    userNameTo.Append(actionUsername);
                                }
                            }

                            //string[] actionUserNames = emailUsers.userNames.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            //for (int i = 0; i < actionUserNames.Length; i++)
                            //{
                            //string actionUsername = Convert.ToString(actionUserNames[i]);
                            //if (actionUsername != string.Empty)
                            //{
                            //    if (userNameTo.Length != 0)
                            //        userNameTo.Append(";");
                            //    userNameTo.Append(actionUsername);
                            //}
                            //}
                        }

                        string mailCC = mailCCList.ToString();
                        string mailTo = string.Empty;

                        if (userEmailTo.Length > 0)
                            mailTo += string.Format("{0}", userEmailTo);

                        if (!string.IsNullOrEmpty(mailTo))
                            mailTo += ";";

                        mailTo += mailCCList.ToString();
                        mailCC = string.Empty;

                        if (!string.IsNullOrEmpty(txtMailTOCC.Text))
                        {
                            if (string.IsNullOrEmpty(mailCC))
                                mailCC += string.Format("{0}", txtMailTOCC.Text);
                            else
                                mailCC += string.Format("; {0}", txtMailTOCC.Text);
                        }

                        if (!string.IsNullOrEmpty(mailTo))
                            mailTo = UGITUtility.RemoveDuplicateEmails(mailTo);


                        string strSVCEmailBody = ((HtmlEditorControl)htmlEditor).Html;
                        string subject = txtMailSubject.Text.Trim();
                        if (moduleInstDepenedency.Behaviour == "Ticket")
                        {
                            string title = string.Format("{0}: {1}", moduleInstDepenedency.RelatedTicketID, moduleInstDepenedency.Title);

                            string strinURL = string.Format("{3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath),
                                moduleInstDepenedency.RelatedTicketID, moduleInstDepenedency.RelatedModule,
                                ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), context.TenantID);

                            string lnkTicket = string.Format("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><a href='{1}'>{0}: {2}</a>", moduleInstDepenedency.ChildInstance, strinURL, moduleInstDepenedency.Title);
                            if (disableTicketLink)
                                lnkTicket = string.Format("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>{0}: {1}", moduleInstDepenedency.ChildInstance, moduleInstDepenedency.Title);

                            subject = string.Format("Ticket {0}: {1}", subject.Replace("[Task/Ticket]", moduleInstDepenedency.ChildInstance), moduleInstDepenedency.Status);
                            strSVCEmailBody = strSVCEmailBody.Replace("[Task/Ticket links will be inserted here]", string.Format("The following ticket needs action:<br/> {0}<br/><br/>{1}", lnkTicket, "If you have any questions or concerns about the action needed, please contact the Service Desk."));
                        }
                        else
                        {
                            string url = string.Format("{6}{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), "task", "1", moduleInstDepenedency.ParentInstance, moduleInstDepenedency.ID, "SVC", ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'));
                            string taskLink = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + "<a href='" + url + "'>" + moduleInstDepenedency.Title + "</a>";
                            if (disableTicketLink)
                                taskLink = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + moduleInstDepenedency.Title;
                            subject = string.Format("{0}: {1}", subject.Replace("[Task/Ticket]", "Service Task"), moduleInstDepenedency.Status == "pending" ? "Approval Needed" : "Assigned for Completion");
                            strSVCEmailBody = strSVCEmailBody.Replace("[Task/Ticket links will be inserted here]", string.Format("The following task needs action:<br/> {0}<br/><br/>{1}", taskLink, "If you have any questions or concerns about the action needed, please contact the Service Desk."));
                        }

                        SendEmail(project, subject, strSVCEmailBody, mailTo, mailCC, userNameTo.ToString(), isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(TicketId))
            {
                List<string> ticketIds = TicketId.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                DataRow ticket = null;

                foreach (string ticketId in ticketIds)
                {
                    string mName = uHelper.getModuleNameByTicketId(ticketId);
                    UGITModule spLItem = ObjModuleViewManager.LoadByName(mName);
                    if (spLItem != null)
                    {
                        ticket = ObjTicketManager.GetByTicketID(spLItem, ticketId);
                    }

                    if (ticket != null)
                    {
                        if (Request["Notification"] == "award")
                        {
                            List<string> lstUserNameWithEmail = GetUserNameWithEmailId();

                            StringBuilder userNameTo = new StringBuilder(lstUserNameWithEmail[0]);
                            StringBuilder userEmailTo = new StringBuilder(lstUserNameWithEmail[1]);

                            StringBuilder mailCCList = new StringBuilder();
                            if (cbIncludeActionUser.Checked)
                            {
                                string stageActionUser = objEscalationProcess.getStageActionUsers(ticket);
                                //UserProfile.UsersInfo actionUseruserInfo = UserProfile.GetUserInfo(ticket, stageActionUser);
                                string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));

                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserEmails))
                                {
                                    string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty)
                                        {
                                            if (mailCCList.Length != 0)
                                                mailCCList.Append(";");
                                            mailCCList.Append(actionUser);
                                        }
                                    }
                                }
                            }
                            string mailCC = mailCCList.ToString();
                            string mailTo = string.Empty;

                            if (userEmailTo.Length > 0)
                                mailTo +=  userEmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                                mailTo += ";";

                            mailTo += mailCCList.ToString();
                            mailCC = string.Empty;

                            if (!string.IsNullOrEmpty(txtMailTOCC.Text))
                            {
                                if (string.IsNullOrEmpty(mailCC))
                                    mailCC += string.Format("{0}", txtMailTOCC.Text);
                                else
                                    mailCC += string.Format("; {0}", txtMailTOCC.Text);

                                mailCC = UGITUtility.RemoveDuplicateEmails(mailCC, Constants.Separator5);
                            }

                            if (userEmailTo.Length > 0)
                                mailTo += string.Format(";{0}", UGITUtility.RemoveDuplicateEmails(Convert.ToString(userEmailTo), Constants.Separator5));

                            mailTo = UGITUtility.RemoveDuplicateEmails(Convert.ToString(mailTo), Constants.Separator5);

                            mailCC = UGITUtility.RemoveDuplicateCcFromToEmails(mailTo, mailCC, Constants.Separator5);

                            string strinURL = string.Format("{3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", UGITUtility.GetAbsoluteURL(Constants.HomePage), ticketId,
                                uHelper.getModuleNameByTicketId(ticketId), ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), context.TenantID);
                            string lnkTicket = string.Format("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><a href='{1}'>{0}: {2}</a>", ticketId, strinURL, ticket[DatabaseObjects.Columns.Title]);
                            if (disableTicketLink)
                                lnkTicket = string.Format("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>{0}: {1}", ticketId, ticket[DatabaseObjects.Columns.Title]);

                            string subject = txtMailSubject.Text.Replace("[Ticket ID]", ticketId);
                            
                            //string strEmailBody = ((HtmlEditorControl)htmlEditor).Html.Replace("[ticket links will be inserted here]", lnkTicket);
                            string strEmailBody = EmailHtmlBody.Html.Replace("[ticket links will be inserted here]", lnkTicket);
                            
                            //SendEmail(ticket, subject, strEmailBody, mailTo, mailCC, userNameTo.ToString());
                            SendEmail(ticket, subject, strEmailBody, mailTo, mailCC, string.Empty, isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);
                        }
                        else
                        {
                            if (Request["EmailAlert"] == "True")
                            {
                                string Email = string.Empty;
                                StringBuilder userEmailTo = GetUserEmailId();

                                StringBuilder mailCCList = new StringBuilder();
                                string[] actionUsers = UGITUtility.SplitString(hdnActionUser.Value.ToString(), Constants.Separator);
                                UserProfile actionUseruserInfo = UserManager.GetUserInfo(ticket, actionUsers, true);
                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserInfo.Email))
                                {
                                    string[] actionUserEmails = actionUseruserInfo.Email.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty)
                                        {
                                            if (mailCCList.Length != 0)
                                                mailCCList.Append(";");
                                            mailCCList.Append(actionUser);
                                        }
                                    }
                                }

                                string mailCC = mailCCList.ToString();
                                string mailTo = string.Empty;

                                if (!string.IsNullOrEmpty(txtMailTOCC.Text))
                                {
                                    if (string.IsNullOrEmpty(mailCC))
                                        mailCC += string.Format("{0}", txtMailTOCC.Text);
                                    else
                                        mailCC += string.Format("; {0}", txtMailTOCC.Text);
                                }

                                if (userEmailTo.Length > 0)
                                    mailTo += userEmailTo;

                                SendEmail(ticket, txtMailSubject.Text, ((HtmlEditorControl)htmlEditor).Html, mailTo, mailCC, string.Empty, isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);
                            }
                            #region sendagentlink
                            else if (Request["sendagentlink"] != null)
                            {
                                string agentID = Convert.ToString(Request["agentID"]);
                                string moduleName = Convert.ToString(Request["ModuleName"]);
                                string mailTo = string.Empty;
                                StringBuilder mailToList = new StringBuilder();
                                string[] field = UGITUtility.SplitString(Convert.ToString(ticket[DatabaseObjects.Columns.TicketStageActionUserTypes]), Constants.Separator);
                                UserProfile actionUseruserInfo = UserManager.GetUserInfo(ticket, field, true);
                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserInfo.Email))
                                {
                                    string[] actionUserEmails = actionUseruserInfo.Email.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty && !mailTo.Contains(actionUser))
                                        {
                                            if (mailToList.Length != 0)
                                                mailToList.Append(";");
                                            mailToList.Append(actionUser);
                                        }
                                    }
                                    mailTo = mailToList.ToString();
                                }
                                txtMailSubject.Text = string.Format("Agent for {0}", ticketId);
                                string agentURL = string.Format("{0}&TicketId={1}", hdnAgentUrl.Value, ticketId);
                                string emailBody = (EmailHtmlBody.Html).Replace("&amp;", "&");

                                if (!string.IsNullOrEmpty(hdnAgentUrl.Value))
                                    emailBody = emailBody.Replace(hdnAgentUrl.Value, agentURL);

                                SendEmail(ticket, txtMailSubject.Text, emailBody, mailTo, "", string.Empty, isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);
                            }
                            #endregion
                            else
                            {
                                StringBuilder userEmailTo = GetUserEmailId();

                                string mailTo = string.Empty;
                                StringBuilder mailCCList = new StringBuilder();
                                if (cbIncludeActionUser.Checked)
                                {
                                    string stageActionUser = objEscalationProcess.getStageActionUsers(ticket);
                                    //UserProfile actionUseruserInfo = UserManager.GetUserInfo(ticket, UGITUtility.SplitString(stageActionUser, Constants.Separator), true);
                                    string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));

                                    string[] separator = { Constants.UserInfoSeparator };
                                    if (!string.IsNullOrEmpty(actionUseruserEmails))
                                    {
                                        string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                        for (int i = 0; i < actionUserEmails.Length; i++)
                                        {
                                            string actionUser = Convert.ToString(actionUserEmails[i]);
                                            if (actionUser != string.Empty)
                                            {
                                                if (mailCCList.Length != 0)
                                                    mailCCList.Append(";");
                                                mailCCList.Append(actionUser);
                                            }
                                        }

                                    }
                                }

                                string mailCC = UGITUtility.RemoveDuplicateCcFromToEmails(Convert.ToString(userEmailTo), mailCCList.ToString(), Constants.Separator5);

                                if (!string.IsNullOrEmpty(txtMailTOCC.Text))
                                {
                                    if (string.IsNullOrEmpty(mailCC))
                                        mailCC += string.Format("{0}", txtMailTOCC.Text);
                                    else
                                        mailCC += string.Format("; {0}", txtMailTOCC.Text);

                                    mailCC = UGITUtility.RemoveDuplicateEmails(mailCC, Constants.Separator5);
                                }

                                if (userEmailTo.Length > 0)
                                    mailTo += string.Format("{0}", UGITUtility.RemoveDuplicateEmails(Convert.ToString(userEmailTo), Constants.Separator5));

                                mailCC = UGITUtility.RemoveDuplicateCcFromToEmails(mailTo, mailCC, Constants.Separator5);

                                if (EmailHtmlBody.Visible)
                                    SendEmail(ticket, txtMailSubject.Text, EmailHtmlBody.Html, mailTo, mailCC, string.Empty, isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);

                                else
                                    SendEmail(ticket, txtMailSubject.Text, ((HtmlEditorControl)htmlEditor).Html, mailTo, mailCC, string.Empty, isHtmlBody: isHtmlBody, disableTicketLink: disableTicketLink);

                            }
                        }
                    }
                }
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private StringBuilder GetUserEmailId()
        {
            StringBuilder userEmailTo = new StringBuilder();
            List<string> userId = UGITUtility.ConvertStringToList(pEditorTo.GetValues(), ",");
            foreach (string entity in userId)
            {
                user = UserManager.GetUserById(entity);
                if (user == null)
                    continue;

                if (user != null && user.isRole == true) // for User Groups
                {
                    if (UserManager.CheckUserIsGroup(entity))
                    {
                        List<UserProfile> lstGroupUsers = UserManager.GetUsersByGroupID(entity);

                        foreach (UserProfile userProfileItem in lstGroupUsers)
                        {
                            user = UserManager.GetUserById(userProfileItem.Id);

                            if (user != null && (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail)))
                            {
                                if (userEmailTo.Length != 0)
                                    userEmailTo.Append(";");
                                userEmailTo.Append(!string.IsNullOrEmpty(user.NotificationEmail) ? user.NotificationEmail : user.Email);
                            }
                        }
                    }
                    else
                    {
                        TicketManager ticketManager = new TicketManager(context);
                        ModuleViewManager moduleManager = new ModuleViewManager(context);
                        UGITModule module = moduleManager.GetByName(ModuleNames.CON);
                        bool isLong = long.TryParse(entity, out long contactid);
                        if (isLong)
                        {
                            DataRow[] contacts = ticketManager.GetAllTickets(module).Select($"{DatabaseObjects.Columns.ID} = {contactid} ");
                            DataRow contactUser = null;
                            if (contacts != null && contacts.Count() > 0)
                            {
                                contactUser = contacts[0];
                            }
                            if (contactUser != null && (!string.IsNullOrEmpty(UGITUtility.ObjectToString(contactUser[DatabaseObjects.Columns.EmailAddress]))))
                            {
                                if (userEmailTo.Length != 0)
                                    userEmailTo.Append(";");
                                userEmailTo.Append(UGITUtility.ObjectToString(contactUser[DatabaseObjects.Columns.EmailAddress]));
                            }
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail))
                    {
                        if (userEmailTo.Length != 0)
                            userEmailTo.Append(";");
                        userEmailTo.Append(!string.IsNullOrEmpty(user.NotificationEmail) ? user.NotificationEmail : user.Email);
                    }
                }
            }
            return userEmailTo;
        }

        private List<string> GetUserNameWithEmailId()
        {
            List<string> lstUserNameWithEmail = new List<string>();
            StringBuilder userEmailTo = new StringBuilder();
            StringBuilder userNameTo = new StringBuilder();
            List<string> userId = UGITUtility.ConvertStringToList(pEditorTo.GetValues(), ",");
            foreach (string entity in userId)
            {
                user = UserManager.GetUserById(entity);
                if (user == null)
                    continue;

                if (user != null && user.isRole == true) // for User Groups
                {
                    List<UserProfile> lstGroupUsers = UserManager.GetUsersByGroupID(entity);

                    foreach (UserProfile userProfileItem in lstGroupUsers)
                    {
                        user = UserManager.GetUserById(userProfileItem.Id);

                        //if (user != null && !string.IsNullOrEmpty(user.Email))
                        if (user != null)
                        {
                            if (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail))
                            {
                                if (userEmailTo.Length != 0)
                                    userEmailTo.Append(";");
                                userEmailTo.Append(!string.IsNullOrEmpty(user.NotificationEmail) ? user.NotificationEmail : user.Email); 
                            }

                            if (!string.IsNullOrEmpty(user.Name))
                            {
                                if (userNameTo.Length != 0)
                                    userNameTo.Append(";");
                                userNameTo.Append(user.Name); 
                            }
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail))
                    {
                        if (userEmailTo.Length != 0)
                            userEmailTo.Append(";");
                        userEmailTo.Append(!string.IsNullOrEmpty(user.NotificationEmail) ? user.NotificationEmail : user.Email);
                    }

                    if (!string.IsNullOrEmpty(user.Name))
                    { 
                        if (userNameTo.Length != 0)
                            userNameTo.Append(";");
                        userNameTo.Append(user.Name);
                    }
                }
            }
            lstUserNameWithEmail.Add(userNameTo.ToString());
            lstUserNameWithEmail.Add(userEmailTo.ToString());

            return lstUserNameWithEmail;
        }

        public void SendEmail(DataRow ticketItem, string subject, string body, string mailTo, string ccTo, string nameMailTo = "", bool isHtmlBody = true, bool disableTicketLink = false)
        {
            //ConfigurationVariableManager cvHelper = new ConfigurationVariableManager(context);
            //LifeCycleStage currentStage = GetTicketCurrentStage(ticketItem);
            string greeting = cvHelper.GetValue("Greeting");
            string signature = cvHelper.GetValue("Signature");

            string ticketId = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]);
            string emailBodyTemp = string.Empty;
            if (Request["EmailAlert"] == "True")
            {
                if (!string.IsNullOrEmpty(mailTo))
                {
                    emailBodyTemp = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}<br/><br/>
                                                {3}<br/>", greeting, nameMailTo != string.Empty ? nameMailTo.Replace(";", ", ") : mailTo.Replace(";", ", "), body.Replace("[$TicketId$]", ticketId), signature);

                    emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(context, ticketItem, ModuleName, true, disableTicketLink));

                    //DataRow emailTicketItem = null;
                    Email emailTicketItem = null;
                    if (chbTicketCopy.Checked)
                    {
                        emailTicketItem = ObjEmailsManager.Load().FirstOrDefault();
                        emailTicketItem.Title = subject.Replace("[$TicketId$]", ticketId);
                        emailTicketItem.EmailIDTo = mailTo;
                        emailTicketItem.EmailIDCC = ccTo;
                        emailTicketItem.MailSubject = subject.Replace("[$TicketId$]", ticketId);
                        emailTicketItem.EscalationEmailBody = emailBodyTemp;
                        emailTicketItem.TicketId = ticketId;
                        emailTicketItem.ModuleNameLookup = ModuleName;
                        emailTicketItem.EmailIDFrom = MailHelper.GetFromEmailId(chkSendMailFromLoggedInUser.Checked);
                        emailTicketItem.IsIncomingMail = false;
                        emailTicketItem.EmailStatus = Constants.EmailStatus.InProgress;
                    }

                    string[] attachments;
                    HttpFileCollection file = Request.Files;
                    attachments = new string[file.Count];
                    for (int i = 0; i < file.Count; i++)
                    {
                        if (file[i] != null && file[i].ContentLength > 0 && !string.IsNullOrEmpty(file[i].FileName))
                        {
                            HttpPostedFile filetype = Request.Files[i];

                            string outputFilePath = string.Empty;
                            string outputPath = uHelper.GetTempFolderPath();
                            outputFilePath = System.IO.Path.Combine(outputPath, file[i].FileName);
                            if (File.Exists(outputFilePath))
                            {
                                File.Delete(outputFilePath);
                            }
                            filetype.SaveAs(outputFilePath);
                            attachments[i] = outputFilePath;

                            if (filetype.ContentType.Contains("application/x-msdownload"))
                            {
                                lblerror.Visible = true;
                                lblerror.Text = file[i].FileName + " has invalid file.";
                                return;
                            }

                            byte[] fileData = null;
                            using (FileStream fsSource = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read))
                            {
                                // Read the source file into a byte array.
                                fileData = new byte[fsSource.Length];
                                // Read may return anything from 0 to numBytesToRead.
                                int n = fsSource.Read(fileData, 0, (int)fsSource.Length);
                            }

                            if (chbTicketCopy.Checked)
                            {
                                //emailTicketItem.Attachments.Add(file[i].FileName, fileData);
                            }

                        }
                    }

                    if (chbTicketCopy.Checked)
                    {
                        ////emailTicketItem[DatabaseObjects.Columns.Title] = subject.Replace("[$TicketId$]", ticketId);
                        ////emailTicketItem[DatabaseObjects.Columns.EmailIDTo] = mailTo;
                        ////emailTicketItem[DatabaseObjects.Columns.EmailIDCC] = ccTo;
                        ////emailTicketItem[DatabaseObjects.Columns.MailSubject] = subject.Replace("[$TicketId$]", ticketId);
                        ////emailTicketItem[DatabaseObjects.Columns.EscalationEmailBody] = emailBodyTemp;
                        ////emailTicketItem[DatabaseObjects.Columns.TicketId] = ticketId;
                        ////emailTicketItem[DatabaseObjects.Columns.ModuleNameLookup] = uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), ModuleName);
                        ////emailTicketItem[DatabaseObjects.Columns.EmailIDFrom] = "";//SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                        ////emailTicketItem[DatabaseObjects.Columns.IsIncomingMail] = false;
                        //////emailTicketItem.UpdateOverwriteVersion();

                        if (emailTicketItem != null)
                        {
                            emailTicketItem.Title = subject.Replace("[$TicketId$]", ticketId);
                            emailTicketItem.EmailIDTo = mailTo;
                            emailTicketItem.EmailIDCC = ccTo;
                            emailTicketItem.MailSubject = subject.Replace("[$TicketId$]", ticketId);
                            emailTicketItem.EscalationEmailBody = emailBodyTemp;
                            emailTicketItem.TicketId = ticketId;
                            emailTicketItem.ModuleNameLookup = ModuleName;
                            emailTicketItem.EmailIDFrom = MailHelper.GetFromEmailId(chkSendMailFromLoggedInUser.Checked);
                            emailTicketItem.IsIncomingMail = false;
                            emailTicketItem.EmailStatus = Constants.EmailStatus.InProgress;
                        }

                    }

                    MailMessenger mail = new MailMessenger(context);
                    mail.SendMail(mailTo, subject.Replace("[$TicketId$]", ticketId), ccTo, emailBodyTemp, isHtmlBody, new string[] { }, true, chkSendMailFromLoggedInUser.Checked, chbTicketCopy.Checked ? ticketId : null);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(mailTo))
                {
                    emailBodyTemp = string.Format(@"{0}<br/><br/>", body.Replace("[$TicketId$]", ticketId));

                    emailBodyTemp += HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(context, ticketItem, ModuleName, true, disableTicketLink));

                    //DataRow emailTicketItem = null;
                    Email emailTicketItem = new Email();
                    ////if (chbTicketCopy.Checked)
                    ////{
                    ////    //DataTable lcList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Emails, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    ////    //var lcList = ObjEmailsManager.Load($"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                    ////    //if (lcList.Count > 0)
                    ////    //    foreach (var item in lcList)
                    ////    //    {
                    ////    //        emailTicketItem = item;
                    ////    //    }
                    ////    //else
                    ////    //    emailTicketItem = new Email();
                    ////    // emailTicketItem = lcList.Rows[0];
                    ////}

                    string[] attachments;
                    HttpFileCollection file = Request.Files;
                    attachments = new string[file.Count];
                    for (int i = 0; i < file.Count; i++)
                    {
                        if (file[i] != null && file[i].ContentLength > 0 && !string.IsNullOrEmpty(file[i].FileName))
                        {
                            HttpPostedFile filetype = Request.Files[i];

                            string outputFilePath = string.Empty;
                            string outputPath = uHelper.GetTempFolderPath();
                            outputFilePath = System.IO.Path.Combine(outputPath, file[i].FileName);
                            if (!Directory.Exists(outputPath))
                            {
                                Directory.CreateDirectory(outputPath);
                            }
                            if (File.Exists(outputFilePath))
                            {
                                File.Delete(outputFilePath);
                            }
                            filetype.SaveAs(outputFilePath);
                            attachments[i] = outputFilePath;

                            if (filetype.ContentType.Contains("application/x-msdownload"))
                            {
                                lblerror.Visible = true;
                                lblerror.Text = file[i].FileName + " has invalid file.";
                                return;
                            }

                            byte[] fileData = null;
                            using (FileStream fsSource = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read))
                            {
                                fileData = new byte[fsSource.Length];
                                int n = fsSource.Read(fileData, 0, (int)fsSource.Length);
                            }

                            if (chbTicketCopy.Checked)
                            {
                                //emailTicketItem.Attachments.Add(file[i].FileName, fileData);
                            }
                        }
                    }

                    if (Request["sendresume"] != null)
                    {
                        Array.Resize(ref attachments, file.Count + 1);
                        attachments[file.Count] = System.IO.Path.Combine(uHelper.GetTempFolderPath(), "UserResume.pdf");
                    }
                    if (chbTicketCopy.Checked)
                    {
                        emailTicketItem.Title = subject.Replace("[$TicketId$]", ticketId);
                        emailTicketItem.EmailIDTo = mailTo;
                        emailTicketItem.EmailIDCC = ccTo;
                        emailTicketItem.MailSubject = subject.Replace("[$TicketId$]", ticketId);
                        emailTicketItem.EscalationEmailBody = emailBodyTemp;
                        emailTicketItem.TicketId = ticketId;
                        emailTicketItem.ModuleNameLookup = ModuleName; //Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), ModuleName));
                        emailTicketItem.EmailIDFrom = MailHelper.GetFromEmailId(chkSendMailFromLoggedInUser.Checked);//SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                        emailTicketItem.IsIncomingMail = false;
                        emailTicketItem.EmailStatus = Constants.EmailStatus.InProgress;

                        ObjEmailsManager.Insert(emailTicketItem);
                        ticketEmailID = emailTicketItem.ID; 
                        //emailTicketItem.UpdateOverwriteVersion();
                    }

                    MailMessenger mail = new MailMessenger(context);
                    mail.ticketEmailID = ticketEmailID;
                    mail.SendMail(mailTo, subject.Replace("[$TicketId$]", ticketId), ccTo, emailBodyTemp, isHtmlBody, attachments, true, chkSendMailFromLoggedInUser.Checked, saveToTicketId: chbTicketCopy.Checked ? ticketId : null);
                }
            }
        }

        private void TicketValidationCheck()
        {
            if (String.IsNullOrEmpty(TicketId))
                return;

            bool IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user) || UserManager.IsAdmin(user);

            List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (ticketIds.Count > 0)
            {
                string moduleName;
                //DataRow[] moduleStagesRow;
                int moduleId = uHelper.getModuleIdByTicketID(HttpContext.Current.GetManagerContext(), ticketIds[0]);

                UGITModule moduleDetail = ObjModuleViewManager.LoadByID(moduleId);
                moduleName = moduleDetail.ModuleName;

                foreach (string ticketId in ticketIds)
                {
                    spLItem = uHelper.getModuleItemByTicketID(ticketId);
                    if (spLItem != null)
                    {
                        // ticket = SPListHelper.GetListItem(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId, ticketIds[0], "Text", SPContext.Current.Web);
                        //ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId+"='"+ ticketIds[0]+"'").Rows[0];
                        ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TicketId} = '{ticketIds[0]}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Rows[0];
                    }

                    if (ticket != null)
                    {
                        //if (!UserManager.IsActionUser(ticket, moduleName) && !IsAdmin)
                        if (!UserManager.IsActionUser(ticket, user) && !IsAdmin)
                        {
                            tblMain.Visible = false;
                            tblErrorMessage.Visible = true;
                            lblErrorMessage.Text = "You don't have permission to escalate the selected tickets.";
                            if (Request["EmailAlert"] == "True")
                                lblErrorMessage.Text = "Selected Tickets can not send alert because you are not an action user.";
                            else
                                lblErrorMessage.Text = "Selected Tickets can not escalation because you are not an action user.";
                            break;
                        }
                    }
                }
            }

        }
        
        protected void cvTO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //List<string> userList = pEditorTo.GetValuesAsList();

            //if (userList.Count == 0)
            //{
            //    args.IsValid = false;
            //}
        }

        /*
        private string GetFromEmailId(bool SendMailFromLoggedInUser = false)
        {
            if (SendMailFromLoggedInUser == true)
            {
                return context.CurrentUser.Email;
            }
            else
            {
                SmtpConfiguration smtpSettings = context.ConfigManager.GetValueAsClassObj(ConfigConstants.SmtpCredentials, typeof(SmtpConfiguration)) as SmtpConfiguration;
                if (smtpSettings != null && !string.IsNullOrEmpty(smtpSettings.SmtpFrom))
                {
                    return smtpSettings.SmtpFrom;
                }
            }

            return string.Empty;
        }
        */
    }
}
