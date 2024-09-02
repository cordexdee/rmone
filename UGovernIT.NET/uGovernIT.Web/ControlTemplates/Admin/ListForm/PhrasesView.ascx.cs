using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class PhrasesView :UserControl
    {
        private PhraseManager _phraseManager = null;
        private ApplicationContext _context = null;
        private RequestTypeManager _requestTypeManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private ServicesManager _ServicesManager = null;

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

        protected PhraseManager PhraseManager
        {
            get
            {
                if (_phraseManager == null)
                {
                    _phraseManager = new PhraseManager(ApplicationContext);
                }
                return _phraseManager;
            }
        }

        protected RequestTypeManager RequestTypeManager
        {
            get
            {
                if (_requestTypeManager == null)
                {
                    _requestTypeManager = new RequestTypeManager(ApplicationContext);
                }
                return _requestTypeManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
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


        protected ServicesManager ServicesManager
        {
            get
            {
                if (_ServicesManager == null)
                {
                    _ServicesManager = new ServicesManager(ApplicationContext);
                }
                return _ServicesManager;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            //addNewItem = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=acrtypenew");
            //aAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','ACR Types - New Itemss','500','150',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));
            //aAddItem_Top.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','ACR Types - New Item','500','150',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath)));



            string sAddNewPhrase = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=phrasesaddnew");
            aPhrase.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','Add Phrases - New Items','600','490',0,'{1}','true')", sAddNewPhrase, Server.UrlEncode(Request.Url.AbsolutePath)));
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
            List<Phrases> listOfPhrases = PhraseManager.Load();
            ModuleRequestType moduleRequestType = null;
            UGITModule uGITModule = null;
            Services services = null;

            listOfPhrases.ForEach(
                    x =>
                    {
                        long Id = x.RequestType ?? 0;
                        if (Id != 0)
                        {
                            moduleRequestType = RequestTypeManager.LoadByID(Id);
                            if (moduleRequestType != null)
                                x.RequestTypeName = moduleRequestType.RequestType;
                            else
                                x.RequestTypeName = string.Empty;
                        }
                        else
                            x.RequestTypeName = string.Empty;

                        Id = UGITUtility.StringToLong(x.TicketType);
                        if (Id != 0)
                        {
                            uGITModule = ModuleViewManager.LoadByID(Id);
                            if (uGITModule != null)
                                x.TicketType = uGITModule.ModuleName;
                            else
                                x.TicketType = string.Empty;
                        }
                        else
                            x.TicketType = string.Empty;

                        Id = x.Services ?? 0;
                        if (Id != 0)
                        {
                            services = ServicesManager.LoadByID(Id);
                            if (services != null)
                                x.ServiceName = services.Title;
                            else
                                x.ServiceName = string.Empty;
                        }
                        else
                            x.ServiceName = string.Empty;
                    });

            if (listOfPhrases != null)
            {
                gridPhrase.DataSource = listOfPhrases;

            }
            else
                gridPhrase.DataSource = null;

            gridPhrase.DataBind();
        }

        protected void gridPhrase_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "Edit" || e.DataColumn.FieldName == "Phrase")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string sPhrase = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Phrase));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=phraseedit&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:UgitOpenPopupDialog('{0}','','Phrase - {1}','600','580',0,'{2}','true')", editItem, sPhrase, Server.UrlEncode(Request.Url.AbsolutePath));
                HtmlAnchor aHtml = (HtmlAnchor)gridPhrase.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "Phrase")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }

            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AgentType)
            {
                string dataKeyValue = Convert.ToString(e.KeyValue);
                var sPhrase = Convert.ToInt32(e.GetValue(DatabaseObjects.Columns.AgentType));
                var sAgentType = Enum.GetName(typeof(Enums.Agent), sPhrase);
                e.Cell.Text = sAgentType;
            }            
        }
    }
}