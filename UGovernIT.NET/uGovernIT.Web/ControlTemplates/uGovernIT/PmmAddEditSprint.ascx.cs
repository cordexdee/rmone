using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class PmmAddEditSprint : System.Web.UI.UserControl
    {
        public string ticketId { get; set; }
        public int SprintID { get; set; }
        public bool IsNew { get; set; }
        public DataTable dtSelectedTasks { get; set; }
        public bool ReadOnly;
        public long Id = 0;
        public string DocumentManagementUrl { get; set; }
        public string FolderName { get; set; }
        public string DocumentName { get; set; }
        public string Iframe { get; set; }
        public bool IsTabActive { get; set; }
        protected string projectID = string.Empty;
        protected string projectPublicID = string.Empty;
        public string ModuleName { get; set; }
        public new long ID = 0;
        public string SprintTitle { get; set; }
        protected int noOfWorkingDays { get; set; }


        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager configurationVariable = null;
        Ticket tkt = null;
        SprintManager sprintManager = null;
        SprintTaskManager sprintTaskManager = null;
        PMMReleaseManager releaseManager = null;
        UserProfileManager UserManager = null;
        TicketManager ticketManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            configurationVariable = new ConfigurationVariableManager(context);
            tkt = new Ticket(context, "PMM");
            sprintManager = new SprintManager(context);
            sprintTaskManager = new SprintTaskManager(context);
            releaseManager = new PMMReleaseManager(context);
            UserManager = HttpContext.Current.GetUserManager();
            ticketManager = new TicketManager(context);

            if (!IsPostBack)
            {

                ID = Convert.ToInt64(ticketManager.GetSingleValueByTicketID("PMM", DatabaseObjects.Columns.ID, ticketId));

                if (SprintID > 0)
                {
                    EditSprint(SprintID, "");
                    hdnSprintId.Value = SprintID.ToString();
                }
                else
                {
                    AddSprint();
                }
            }
        }


        public void AddSprint()
        {
            txtTitle.Text = string.Empty;
            //dtcEndDate.ClearSelection();
            //dtcStartDate.ClearSelection();
            dtcEndDate.Date = DateTime.MinValue;
            dtcStartDate.Date = DateTime.MinValue;
            //hdnSprintId.Value = string.Empty;
            //hdnSprintTitle.Value = string.Empty;
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
            if (item != null)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.SprintDuration])) && Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]) > 0)
                {
                    noOfWorkingDays = Convert.ToInt32(item[DatabaseObjects.Columns.SprintDuration]);
                }
                else
                {
                    string defaultSprintDuration = configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration);
                    if (!string.IsNullOrEmpty(defaultSprintDuration))
                    {
                        noOfWorkingDays = Convert.ToInt32(configurationVariable.GetValue(ConfigConstants.DefaultSprintDuration));
                    }
                    else
                    {
                        noOfWorkingDays = 10;
                    }
                }

                Sprint col1 = sprintManager.Load(x => x.PMMIdLookup == ID).FirstOrDefault();
                DateTime maxDate = sprintManager.Load(x => x.PMMIdLookup == ID).Max(x => x.EndDate) ?? DateTime.MinValue;

                if (col1 != null)
                {
                    DateTime maxEndDate = Convert.ToDateTime(maxDate);
                    string nextWorkingDate = uHelper.GetNextWorkingDateAndTime(context, maxEndDate);
                    if (!string.IsNullOrEmpty(nextWorkingDate))
                    {
                        string[] nextWorkingDaysDateTime = nextWorkingDate.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                        if (nextWorkingDaysDateTime != null && nextWorkingDaysDateTime.Length > 0)
                        {
                            dtcStartDate.Date = Convert.ToDateTime(nextWorkingDaysDateTime[0]);
                        }
                    }
                }
                else
                {
                    dtcStartDate.Date = DateTime.Now;
                }
                if (dtcStartDate.Date != null)
                {
                    DateTime[] calculatedDates = null;
                    calculatedDates = uHelper.GetEndDateByWorkingDays(context, dtcStartDate.Date, noOfWorkingDays);
                    if (calculatedDates != null && calculatedDates.Length > 0)
                    {
                        dtcEndDate.Date = calculatedDates[1];
                        int week = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                        if (week > 0)
                        {
                            lblSprintDuration.Text = string.Format("{0} week(s)", week);
                        }
                        else
                        {
                            lblSprintDuration.Text = string.Format("{0} day(s)", noOfWorkingDays);
                        }
                    }
                }
            }
           
        }

        private void EditSprint(int id, string sprintTitle)
        {
                if (id > 0)
            {
                Sprint spItem = sprintManager.Load(x => x.ID == UGITUtility.StringToLong(id)).FirstOrDefault();

                if (spItem != null)
                {
                    txtTitle.Text = Convert.ToString(spItem.Title);
                    dtcStartDate.Date = (Convert.ToDateTime(spItem.StartDate));
                    dtcEndDate.Date = (Convert.ToDateTime(spItem.EndDate));
                    DataRow item = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                    if (item != null)
                    {

                        noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, dtcStartDate.Date, dtcEndDate.Date);
                        int week = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                        if (week > 0)
                        {
                            lblSprintDuration.Text = string.Format("{0} week(s)", week);
                        }
                        else
                        {
                            lblSprintDuration.Text = string.Format("{0} day(s)", noOfWorkingDays);
                        }
                    }
                }
            }
           
        }

        protected void lnkSaveSprint_Click(object sender, EventArgs e)
        {

            if(ID==0)
                ID = Convert.ToInt64(ticketManager.GetSingleValueByTicketID("PMM", DatabaseObjects.Columns.ID, ticketId));

            List<Sprint> lstSprints = new List<Sprint>();
            bool isError = false;
            if (dtcStartDate.Date == null)
            {
                lbStartDate.Text = "Please enter start date";
                isError = true;
                lbStartDate.Style.Add("display", "");
            }
            else if (dtcEndDate.Date == null)
            {
                dtcDateError.Text = "Please enter end date";
                isError = true;
                dtcDateError.Style.Add("display", "");
            }
            else if (dtcStartDate.Date > dtcEndDate.Date)
            {
                isError = true;
                dtcDateError.Text = "End Date cannot be before Start Date";
                dtcDateError.Style.Add("display", "");
            }
            else if (string.IsNullOrEmpty(hdnSprintId.Value.Trim()))
            {
                lstSprints = sprintManager.Load(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.Sprint, $"{DatabaseObjects.Columns.TenantID}= '{context.TenantID}'");  //SPListHelper.GetSPList(DatabaseObjects.Lists.Sprint);

                Sprint coll = lstSprints.Where(x => x.PMMIdLookup == ID && x.Title.Equals(txtTitle.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (coll != null)
                {
                    lblTitleError.Text = "Sprint with same name already exists.";
                    lblTitleError.Style.Add("display", "");
                    isError = true;
                }
            }
            if (isError)
            {
                return;
            }

            Sprint item = null;
            if (string.IsNullOrEmpty(hdnSprintId.Value.Trim()) && lstSprints != null)
            {
                item = new Sprint();
                int collCount = lstSprints.Where(x => x.PMMIdLookup == ID).Count();
                if (collCount > 0)
                {
                    item.ItemOrder = collCount + 1;
                }
                else
                {
                    item.ItemOrder = 1;
                }

                DataRow projectItem = Ticket.GetCurrentTicket(context, "PMM", ticketId);
                if (projectItem != null)
                {
                    item.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]);
                }
            }
            else
            {
                item = sprintManager.LoadByID(Convert.ToInt64(hdnSprintId.Value.Trim()));
            }
            if (item != null)
            {
                item.Title = txtTitle.Text.Trim();
                item.StartDate = dtcStartDate.Date;
                item.EndDate = dtcEndDate.Date;
                if (item.RemainingHours == null)
                    item.RemainingHours = 0;
                if (item.TaskEstimatedHours == null)
                    item.TaskEstimatedHours = 0;
                if (item.PercentComplete == null)
                    item.PercentComplete = 0;

                if (item.ID > 0)
                    sprintManager.Update(item);
                else
                    sprintManager.Insert(item);

                txtTitle.Text = string.Empty;
                dtcStartDate.Date = DateTime.MinValue;
                dtcEndDate.Date = DateTime.MinValue;
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }

        }
    }
}