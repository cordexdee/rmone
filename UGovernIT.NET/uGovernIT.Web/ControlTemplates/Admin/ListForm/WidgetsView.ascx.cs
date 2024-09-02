using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Web.ControlTemplates.Admin.ListForm
{
    public partial class WidgetsView : System.Web.UI.UserControl
    {
        private AgentsManager _agentsManager = null;
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

        protected AgentsManager agentsManager
        {
            get
            {
                if (_agentsManager == null)
                {
                    _agentsManager = new AgentsManager(ApplicationContext);
                }
                return _agentsManager;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            
            string widgetAddEdit = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=widgetaddedit&ID=0");
            aWidget.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Widget - New Items','600','700',0,'{1}','true')", widgetAddEdit, Server.UrlEncode(Request.Url.AbsolutePath)));
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            BindGridView();
            base.OnLoad(e);
        }
        private void BindGridView()
        {
            List<Agents> listOfAgents = agentsManager.Load();
            listOfAgents.ForEach(
                    x =>
                    {

                        //long Id = x.RequestType ?? 0;
                        //if (Id != 0)
                        //    x.RequestTypeName = RequestTypeManager.LoadByID(Id).RequestType;
                        //else
                        //    x.RequestTypeName = string.Empty;

                        //Id = UGITUtility.StringToLong(x.TicketType);
                        //if (Id != 0)
                        //    x.TicketType = ModuleViewManager.LoadByID(Id).ModuleName;
                        //else
                        //    x.TicketType = string.Empty;

                        //Id = x.Services ?? 0;
                        //if (Id != 0)
                        //    x.ServiceName = ServicesManager.LoadByID(Id).Title;
                        //else
                        //    x.ServiceName = string.Empty;
                    });

            if (listOfAgents != null)
            {
                gridWidgets.DataSource = listOfAgents;

            }
            else
                gridWidgets.DataSource = null;

            gridWidgets.DataBind();
        }

        protected void gridWidgets_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "Edit" || e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string sTitle = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=widgetaddedit&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','Widgets - {1}','600','700',0,'{2}','true')", editItem, sTitle, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)gridWidgets.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }

            }
           
        }
    }
}