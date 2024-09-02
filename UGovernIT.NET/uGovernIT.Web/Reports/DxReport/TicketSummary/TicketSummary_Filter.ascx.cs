using System;
using System.Data;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
namespace uGovernIT.DxReport
{
    public partial class TicketSummary_Filter : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        public string delegateControl;
        public string reportUrl = string.Empty;
        public string LegendTitle = string.Empty;
        public long Id { get; set; }

        public ApplicationContext _context = null;
        private ModuleViewManager _moduleViewManager = null;

        public ApplicationContext ApplicationContext
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

        public ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        public TicketSummary_Filter()
        {
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            if (ModuleName == "CPR")
                ModuleName = "CPR";
            else
                ModuleName = "TSR";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _context = Session["context"] as ApplicationContext;

            if (Request.IsAuthenticated)
            { }
            if (!IsPostBack)
            {
                ModuleName = Request["Module"];
                FillModules();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(ModuleName == "CPR")
                LegendTitle = "Project";
            else
                LegendTitle = "Ticket";
        }

        private void FillModules()
        {   
            //   Util.Log.ULog.WriteLog("(Filter) TenantID : " + ApplicationContext.TenantID);

            cblModules.Items.Clear();

            var modules = ModuleViewManager.Load(x => x.EnableModule == true && x.ShowSummary == true).Select(x => new { x.Title, x.ModuleName }).ToList();
               
            if (ModuleName == "CPR")
                modules = modules.Where(x => x.ModuleName == "CPR").ToList();

            if (modules != null && modules.Count > 0)
            {                
                cblModules.DataTextField = DatabaseObjects.Columns.Title;
                cblModules.DataValueField = DatabaseObjects.Columns.ModuleName;
                cblModules.DataSource = modules;
                cblModules.DataBind();
            }

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}