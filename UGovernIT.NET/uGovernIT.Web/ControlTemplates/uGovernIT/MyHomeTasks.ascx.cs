using System;
using System.Data;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class MyHomeTasks : System.Web.UI.UserControl
    {
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
            _context = HttpContext.Current.GetManagerContext();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindAspxGridView();
        }

        private void BindAspxGridView()
        {
            grid.DataSource = getDataSource();
            grid.DataBind();
        }

        public DataTable getDataSource()
        {            
            var ownerColumn = DatabaseObjects.Columns.AssignedTo;
            var userid = _context.CurrentUser.Id;

            var uManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var role = uManager.GetUserRoles(userid).Select(x => x.Id).ToList();

            var sbQuery = new StringBuilder();

            sbQuery.Append($"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");

            sbQuery.Append($" AND ({ownerColumn}='{userid}'");
            foreach (var item in role)
            {
                sbQuery.Append($" OR {ownerColumn}='{item}'");
            }
            sbQuery.Append($")");

            return GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasks, sbQuery.ToString());
        }

    }
}