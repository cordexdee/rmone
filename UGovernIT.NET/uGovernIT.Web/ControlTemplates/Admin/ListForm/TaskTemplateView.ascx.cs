using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using uGovernIT.Utility;
using System.IO;
using uGovernIT.Manager.PMM;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class TaskTemplateView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGriview();
        }
        private void BindGriview()
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UGITTaskManager tskManager = new UGITTaskManager(context);
            if (!string.IsNullOrEmpty(Request["ID"]))
            {
                List<UGITTask> taskTemplates = tskManager.LoadByTemplateID(UGITUtility.StringToLong(Request["ID"].ToString()));
                if (taskTemplates != null && taskTemplates.Count > 0)
                {
                    dxgridview.DataSource = taskTemplates.OrderBy(o => o.ItemOrder).ToList();
                    dxgridview.DataBind();
                }
            }
        }
    }
}