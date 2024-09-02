using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class PMMRisksEdit : UserControl
    {
        public string TicketID { get; set; }
        public int ItemID { get; set; }

        List<UGITTask> pmmRiskList;
        DataRow pmmItem;
        UGITTask item;
        UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
        StringBuilder linkFile = new StringBuilder();
        private DMSManagerService _dmsManagerService = null;

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(HttpContext.Current.GetManagerContext());
                }
                return _dmsManagerService;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            pmmRiskList = TaskManager.Load();   //SPListHelper.GetSPList(DatabaseObjects.Lists.PMMRisks);
            List<string> viewFields = new List<string>();
            viewFields.Add(DatabaseObjects.Columns.Id);
            viewFields.Add(DatabaseObjects.Columns.TicketId);
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule module = moduleViewManager.LoadByName("PMM");
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            pmmItem = ticketManager.GetByTicketID(module, TicketID);

            item = pmmRiskList.FirstOrDefault(x=>x.ID == ItemID); //SPListHelper.GetSPListItem(pmmRiskList, ItemID);


            FillData();


            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void FillData()
        {
            if (item == null)
                return;

            txtTitle.Text = Convert.ToString(item.Title);
            ddlImpact.SelectedIndex = ddlImpact.Items.IndexOf(ddlImpact.Items.FindByValue(Convert.ToString(item.IssueImpact)));  // = ddlImpact.Items.IndexOf(ddlImpact.Items.FindByValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.IssueImpact))));
            txtProbability.Text = Convert.ToString(item.RiskProbability);
            txtMitigationPlan.Text = Convert.ToString(item.MitigationPlan);
            txtContingencyPlan.Text = Convert.ToString(item.ContingencyPlan);


            lnkbtnArchive.Visible = true;
            LnkBtnUnarchive.Visible = false;
            LnkbtnDelete.Visible = false;
            if (item.Deleted)
            {
                LnkbtnDelete.Visible = true;
                LnkBtnUnarchive.Visible = true;
                lnkbtnArchive.Visible = false;
            }

            peAssignedTo.SetValues(item.AssignedTo);

            if (!string.IsNullOrEmpty(item.Attachments))
            {
                FileUploadControl.SetValue(item.Attachments);
            }

            //SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.AssignedTo)));
            //if (userValues != null && userValues.Count > 0)
            //{
            //    peAssignedTo.UpdateEntities(uHelper.getUsersListFromCollection(userValues));
            //    lbAssignedTo.Text = UserProfile.CommaSeparatedAccountsFrom(userValues, "; ");
            //}
        }        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (item == null)
                return;

            //SPFieldUserValueCollection userMultiLookup = uHelper.GetFieldUserValueCollection(peAssignedTo.ResolvedEntities);

            item.Title = txtTitle.Text.Trim();
            item.IssueImpact = ddlImpact.SelectedValue;
            item.RiskProbability = Convert.ToInt32(txtProbability.Text);
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

            //Bind files
            if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                item.Attachments = FileUploadControl.GetValue();

            TaskManager.Update(item);
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (item != null)
            {
                TaskManager.Delete(item); //  item.Delete();
            }
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void LnkBtnUnarchive_Click(object sender, EventArgs e)
        {
            if (item != null)
            {
                item.Deleted = false;
                TaskManager.Update(item);  // item.Update();
            }
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkbtnArchive_Click(object sender, EventArgs e)
        {
            if (item != null)
            {
                item.Deleted = true;
                item.Deleted = true;
                TaskManager.Update(item);
            }
            uHelper.ClosePopUpAndEndResponse(Context);
        }
    }
}
