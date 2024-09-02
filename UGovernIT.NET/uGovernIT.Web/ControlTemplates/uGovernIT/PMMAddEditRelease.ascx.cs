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
    public partial class PMMAddEditRelease : System.Web.UI.UserControl
    {
        public string ticketId { get; set; }
        public int ReleaseID { get; set; }
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

        protected override void OnInit(EventArgs e)
        {
            configurationVariable = new ConfigurationVariableManager(context);
            tkt = new Ticket(context, "PMM");
            sprintManager = new SprintManager(context);
            sprintTaskManager = new SprintTaskManager(context);
            releaseManager = new PMMReleaseManager(context);
            UserManager = HttpContext.Current.GetUserManager();
            ticketManager = new TicketManager(context);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ReleaseID > 0)
                {
                    EditRelease(ReleaseID);
                    hdnReleaseId.Value = ReleaseID.ToString();
                }
                else
                {
                    AddRelease();
                }
            }
        }
        public void AddRelease()
        {
            txtRelease.Text = txtReleaseID.Text = string.Empty;
            dtcReleaseDate.Date = DateTime.MinValue;
            hdnReleaseId.Value = string.Empty;
        }
        private void EditRelease(int id)
        {
            if (id > 0)
            {
                DataRow spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases, $"{DatabaseObjects.Columns.ID} = {id} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select()[0];

                if (spItem != null)
                {
                    txtRelease.Text = Convert.ToString(spItem[DatabaseObjects.Columns.Title]);
                    dtcReleaseDate.Date = (Convert.ToDateTime(spItem[DatabaseObjects.Columns.ReleaseDate]));
                    txtReleaseID.Text = (Convert.ToString(spItem[DatabaseObjects.Columns.ReleaseID]));
                }
            }
        }

        protected void lnkSaveRelease_Click(object sender, EventArgs e)
        {
            
            List<ProjectReleases> lstReleases = new List<ProjectReleases>();
            bool isError = false;
            if (txtRelease.Text.Trim() == string.Empty)
            {
                lblRelease.Text = "Please enter release title.";
                isError = true;
                lblRelease.Style.Add("display", "");
            }

            if (dtcReleaseDate.Date == null)
            {
                lblReleaseDate.Text = "Please enter release date.";
                isError = true;
                lblReleaseDate.Style.Add("display", "");
            }
            if (txtReleaseID.Text.Trim() == string.Empty)
            {
                lblReleaseID.Text = "Please enter release id.";
                isError = true;
                lblReleaseID.Style.Add("display", "block");
            }
            else if (string.IsNullOrEmpty(hdnReleaseId.Value.Trim()))
            {
                lstReleases = releaseManager.Load();
                ProjectReleases coll = lstReleases.Where(x => x.PMMIdLookup == ID && x.ReleaseID.Equals(txtReleaseID.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (coll != null)
                {
                    lblReleaseID.Text = "Release with same Id already exists.";
                    lblReleaseID.Style.Add("display", "block");
                    isError = true;
                }
            }
            if (isError)
            {
                return;
            }

            ProjectReleases item = null;
            if (string.IsNullOrEmpty(hdnReleaseId.Value.Trim()) && lstReleases != null)
            {
                item = new ProjectReleases();
                int collCount = lstReleases.Where(x => x.PMMIdLookup == ID).Count();

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
                    item.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]); //new SPFieldLookupValue(projectItem.ID, ticketId);
                }
               
            }
            else
            {
                item = releaseManager.LoadByID(Convert.ToInt64(hdnReleaseId.Value.Trim())); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases, $"{DatabaseObjects.Columns.ID} = {Convert.ToInt32(hdnReleaseId.Value.Trim())} and {DatabaseObjects.Columns.TenantID}= '{context.TenantID}'").Select()[0];
            }
                if (item != null)
                {
                    item.Title = txtRelease.Text.Trim();
                    item.ReleaseID = txtReleaseID.Text.Trim();
                    item.ReleaseDate = dtcReleaseDate.Date;

                    if (item.ID > 0)
                        releaseManager.Update(item);
                    else
                        releaseManager.Insert(item);

                    txtRelease.Text = string.Empty;
                    txtReleaseID.Text = string.Empty;
                    dtcReleaseDate.Date = DateTime.MinValue;
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
        
    }
}