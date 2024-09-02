
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PMMRisksNew : UserControl
    {
        public string TicketID { get; set; }
        List<UGITTask> pmmRiskList;
        DataRow pmmItem;
        
        UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            pmmRiskList = TaskManager.Load();  // SPListHelper.GetSPList(DatabaseObjects.Lists.PMMRisks);
            List<string> viewFields = new List<string>();
            viewFields.Add(DatabaseObjects.Columns.Id);
            viewFields.Add(DatabaseObjects.Columns.TicketId);
            //Ticket ticket = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule module = moduleViewManager.LoadByName("PMM");
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            pmmItem = ticketManager.GetByTicketID(module,TicketID);  /// ticketManager.getCurrentTicket("PMM", TicketID, viewFields, true);

            //LoadRiskImpacts();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void LoadRiskImpacts()
        {
            //if (ddlImpact.Items.Count > 0)
            //    return;

            //SPFieldChoice choices = (SPFieldChoice)pmmRiskList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.IssueImpact);
            //foreach (string s in choices.Choices)
            //{
            //    ddlImpact.Items.Add(new ListItem(s, s));
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            //Creates uservaluecollection object from peoplepicker control
            string assignedUser = string.Empty;
            List<string> assignedUsers = new List<string>();
            //List<UserProfile> userMultiLookup = HttpContext.Current.GetUserManager().GetUserInfosById(peAssignedTo.GetValues());    // uHelper.GetFieldUserValueCollection(peAssignedTo.ResolvedEntities);

            UGITTask item = new UGITTask();
            item.Title = txtTitle.Text.Trim();
            item.ModuleNameLookup = ModuleNames.PMM;
            item.IssueImpact = ddlImpact.SelectedValue;
            item.RiskProbability = UGITUtility.StringToInt(txtProbability.Text);
            item.MitigationPlan = txtMitigationPlan.Text.Trim();
            item.ContingencyPlan = txtContingencyPlan.Text.Trim();
            item.Deleted = false;
            item.TicketId = Convert.ToString(pmmItem["TicketId"]);
            item.AssignedTo = peAssignedTo.GetValues();
            item.DueDate = new DateTime(1800, 1, 1);
            item.ProposedDate = new DateTime(1800, 1, 1);
            item.ResolutionDate = new DateTime(1800, 1, 1);
            item.StartDate = new DateTime(1800, 1, 1);
            item.SubTaskType = "Risk";
            if (!string.IsNullOrEmpty(PMMFileUploadControl.GetValue()))
                item.Attachments = PMMFileUploadControl.GetValue();
            TaskManager.Update(item);
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }
    }
}
