using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class NewProjectTask : System.Web.UI.UserControl
    {
        public int Ids { get; set; }
        public string TicketID { get; set; }
        public int BaseLineID { get; set; }
        public string ModuleName { get; set; }
        public string ProjectStartDate = DateTime.Now.ToString();
        public string FrameId;
        public bool showBaseline;
        public DateTime minDate = DateTime.MinValue;
        protected string calendarURL = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx");
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UGITTaskManager objUGITTaskManager;
        public bool bActionUser;
        public bool IsCompactView { get; set; }
        protected int tasKIndex = -1;
        protected string TaskDetailViewURL = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/delegatecontrol.aspx?control=resourceusage");
        public string reportUrl = string.Empty;
        public string userID = string.Empty;
        public string ResourceSelectFilter = string.Empty;
        public bool actualHoursByUser;
        public bool isMac = false;
        public string isPlatform = "";
        public string BrowserType = "";
        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ajaxhelper.aspx");
        public string TrackProjectStageUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=trackprojectstagehistory");
        Ticket ticketReq;
        protected long stageStep;
        protected override void OnInit(EventArgs e)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            TicketHoursManager ticketHourManager = new TicketHoursManager(context);
            actualHoursByUser = ticketHourManager.IsActualHoursByUserEnable(context, ModuleName);
            
            // Fetching task index from cache to set focus of the grid on this task
            if (!IsPostBack)
            {
                object cacheVal = Context.Cache.Get(string.Format("TaskInfo-{0}", context.CurrentUser.Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("TaskInfo-{0}", context.CurrentUser.Id));

                    Dictionary<string, string> cacheParams = UGITUtility.GetCustomProperties(Convert.ToString(cacheVal), "&");
                    if (cacheParams != null && cacheParams.Count > 0 && cacheParams.ContainsKey("taskindex"))
                    {
                        tasKIndex = UGITUtility.StringToInt(cacheParams["taskindex"]);
                    }
                }
            }

            if (Request["showBaseline"]!=null)
                showBaseline = UGITUtility.StringToBoolean(Request["showBaseline"]);

            reportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            userID = context.CurrentUser.Id;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            ResourceSelectFilter = configurationVariableManager.GetValue(ConfigConstants.ResourceSelectFilter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            objUGITTaskManager = new UGITTaskManager(context);
            UGITTask uGITTask = new UGITTask();
            uGITTask.TicketId = TicketID;



            DataRow ticket = Ticket.GetCurrentTicket(context, ModuleName, TicketID);
            if (ticket != null)
            {
                ProjectStartDate = Convert.ToString(ticket[DatabaseObjects.Columns.ActualStartDate]);
                ticketReq = new Ticket(context, uHelper.getModuleIdByTicketID(context, TicketID));
                LifeCycleStage lifeCycleStage= ticketReq.GetTicketCurrentStage(ticket);
                if (lifeCycleStage != null)
                    stageStep = lifeCycleStage.StageStep;
                Ids = UGITUtility.StringToInt(ticket[DatabaseObjects.Columns.ID]);
            }
            if (context.UserManager.IsActionUser(ticket, context.CurrentUser) || context.UserManager.IsAdmin(context.CurrentUser) || context.UserManager.IsTicketAdmin(context.CurrentUser))
            {
                bActionUser = true;
            }
            
        }
    }
}