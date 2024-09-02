using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class PhraseEdit : UserControl
    {
        public new long ID { get; set; }
        public long ModuleId = 0;

        private PhraseManager _phraseManager = null;
        private ModuleViewManager _ObjModuleManager = null;
        private ServicesManager _ServicesManager = null;
        private WikiArticlesManager _WikiArticlesManager = null;
        private HelpCardManager _helpCardManager = null;
        LookupValueBoxEdit dropDownBox;
        Phrases _phrases;

        protected PhraseManager PhraseManager
        {
            get
            {
                if (_phraseManager == null)
                {
                    _phraseManager = new PhraseManager(HttpContext.Current.GetManagerContext());
                }
                return _phraseManager;
            }
        }

        protected ModuleViewManager ObjModuleManager
        {
            get
            {
                if (_ObjModuleManager == null)
                {
                    _ObjModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _ObjModuleManager;
            }
        }

        
        protected ServicesManager ServicesManager
        {
            get
            {
                if (_ServicesManager == null)
                {
                    _ServicesManager = new ServicesManager(HttpContext.Current.GetManagerContext());
                }
                return _ServicesManager;
            }
        }
        protected WikiArticlesManager wikiArticlesManager
        {
            get
            {
                if (_WikiArticlesManager == null)
                {
                    _WikiArticlesManager = new WikiArticlesManager(HttpContext.Current.GetManagerContext());
                }
                return _WikiArticlesManager;
            }
        }
        
        protected HelpCardManager HelpCardManager
        {
            get
            {
                if(_helpCardManager == null)
                {
                    _helpCardManager = new HelpCardManager(HttpContext.Current.GetManagerContext());
                }
                return _helpCardManager;
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            _phrases = PhraseManager.LoadByID(ID);
            ModuleId = !string.IsNullOrEmpty(_phrases.TicketType) ? Convert.ToInt64(_phrases.TicketType) : 0;

            if (_phrases != null)
            {
                Fill();
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _phrases.Phrase = txtPhrase.Text;

            if (!string.IsNullOrEmpty(ddlPhrasesAgentType.SelectedValue))
                _phrases.AgentType = Convert.ToInt32(ddlPhrasesAgentType.SelectedValue);
            else
                _phrases.AgentType = 0;

            if (!string.IsNullOrEmpty(ddlPhraseTicketType.SelectedValue))
            {
                _phrases.TicketType = ddlPhraseTicketType.SelectedValue;

                if (!string.IsNullOrEmpty(dropDownBox.GetValues()))
                    _phrases.RequestType = UGITUtility.StringToLong(dropDownBox.GetValues());

                if (_phrases.AgentType == 3)
                {
                    _phrases.RequestType = null;
                    if (!string.IsNullOrEmpty(DDLservices.SelectedValue))
                        _phrases.Services = Convert.ToInt64(DDLservices.SelectedValue);
                    else
                        _phrases.Services = null;
                }
                else
                {
                    _phrases.Services = null;
                }

            }
            else
            {
                _phrases.TicketType = null;
                _phrases.RequestType = null;
                _phrases.Services = null;
            }
            // List<string> groupList = wikiLookUp.GetValuesAsList();
            // _phrases.WikiLookUp =  wikiLookUp.GetValues();
            _phrases.WikiLookUp = wikiTokenBox.Value.ToString();
            _phrases.HelpCardLookUp = helpCardTokenBox.Value.ToString();
            PhraseManager.Update(_phrases);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Updated Phrase: {_phrases.Phrase}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void Fill()
        {
            if (_phrases.AgentType == 3)
            {
                services.Style.Add("display", "block");
                DDLservices.Style.Add("display", "block");
                RequestType.Style.Add("display","none");
            }
            else
            {
                RequestType.Style.Add("display", "block");
                services.Style.Add("display", "none");
                DDLservices.Style.Add("display", "none");
            }
            LoadAgentTypePhrase();
            LoadPhraseTicketType();
           // LoadServices();
            ServicesManager.loadDdlServices(ref DDLservices);

            dropDownBox = new LookupValueBoxEdit();
            dropDownBox.ID = "ddRequestType";
            dropDownBox.isRequestType = true;
            dropDownBox.FieldName = DatabaseObjects.Columns.TicketRequestTypeLookup;
            dropDownBox.gridView.SettingsBehavior.AllowSelectByRowClick = true;
            dropDownBox.gridView.ClientSideEvents.SelectionChanged = "requestTypeSelectionChanged";
            dropDownBox.gridView.Width = Unit.Percentage(100);
            dropDownBox.gridView.EnableCallBacks = true;
            //dropDownBox.ModuleName = _phrases.TicketType;
            dropDownBox.CssClass = "rmmLookup-valueBoxEdit";
            //dropDownBox.dropBox.DropDownWindowWidth = 555;
            //dropDownBox.dropBox.Width = 555;

            if (!string.IsNullOrEmpty(_phrases.TicketType) || _phrases.TicketType != "0")
                dropDownBox.ModuleName = _phrases.TicketType;
            else
                dropDownBox.ModuleName = "0";
            divRequestType.Controls.Add(dropDownBox);

            txtPhrase.Text = _phrases.Phrase;
            ddlPhrasesAgentType.SelectedValue = Convert.ToString(_phrases.AgentType);
            dropDownBox.SetValues(Convert.ToString(_phrases.RequestType));
            DDLservices.SelectedValue = Convert.ToString(_phrases.Services);
            LoadWiki();
            LoadHelpCard();
            wikiTokenBox.Value = _phrases.WikiLookUp;
            helpCardTokenBox.Value = _phrases.HelpCardLookUp;                          

        }

        private void LoadAgentTypePhrase()
        {
            var values = Enum.GetValues(typeof(Enums.Agent));

            foreach (var item in values)
            {
                ddlPhrasesAgentType.Items.Add(new ListItem(Convert.ToString(item),((int)item).ToString()));
            }
            ddlPhrasesAgentType.Items.Insert(0, new ListItem("None", ""));
        }

        private void LoadPhraseTicketType()
        {
            ddlPhraseTicketType.Items.Clear();
            List<UGITModule> lstModule = ObjModuleManager.Load();

            // ModuleLabel.InnerText = "Module";
            if (lstModule != null && lstModule.Count > 0)
            {
                lstModule = lstModule.Where(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();

                ddlPhraseTicketType.DataSource = lstModule;
                ddlPhraseTicketType.DataValueField = DatabaseObjects.Columns.ID;
                ddlPhraseTicketType.DataTextField = DatabaseObjects.Columns.Title;

            }

            if (UGITUtility.StringToLong(_phrases.TicketType) != 0)
                ddlPhraseTicketType.SelectedValue = _phrases.TicketType;    
            
            ddlPhraseTicketType.DataBind();

            ddlPhraseTicketType.Items.Insert(0, new ListItem("None", ""));
        }

        public void LoadServices()
        {
            //List<Services> services = ServicesManager.LoadAllServices().Where(x=>x.IsActivated =true).ToList();
            //DDLservices.Items.Clear();
            //if (services != null)
            //{
            //    DDLservices.DataSource = services;
            //    DDLservices.DataValueField = DatabaseObjects.Columns.ID;
            //    DDLservices.DataTextField = DatabaseObjects.Columns.Title;
            //}
            //DDLservices.DataBind();
            //DDLservices.Items.Insert(0, new ListItem("None", ""));

            List<Services> services = ServicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Deleted == false).OrderBy(x => x.Title).ToList();
            List<string> Categories = services.Select(x => x.ServiceCategoryType).Distinct().OrderBy(x => x).ToList();

            DDLservices.Items.Clear();
            if (services != null && services.Count > 0)
            {
                foreach (var item in Categories)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        ListItem category = new ListItem();
                        category.Text = item;
                        category.Value = "0";
                        category.Attributes.Add("Style", "color:#848484");
                        category.Attributes.Add("Disabled", "true");
                        DDLservices.Items.Add(category);
                    }

                    foreach (var service in services.Where(x => x.ServiceCategoryType.EqualsIgnoreCase(item)))
                    {
                        ListItem srv = new ListItem();
                        srv.Text = service.Title;
                        srv.Value = Convert.ToString(service.ID);

                        DDLservices.Items.Add(srv);
                    }
                }

            }
            DDLservices.Items.Insert(0, new ListItem("None", ""));
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ID != 0)
            {
                Phrases phrase = PhraseManager.Load(x => x.PhraseId == ID).FirstOrDefault();
                if (phrase != null)
                {
                    Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Deleted Phrase: {phrase.Phrase}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                    PhraseManager.Delete(phrase);
                }              
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        public void LoadWiki()
        {
            List<WikiArticles> lstWikiArticles =  wikiArticlesManager.Load(x => x.Deleted != true);
            if(lstWikiArticles != null && lstWikiArticles.Count != 0)
            {
                var tokenData = lstWikiArticles.Select(x => new {
                    Title = $"{x.Title} ({x.TicketId}) ",
                    Value = x.TicketId

                }).ToList();
                wikiTokenBox.ValueField = "Value";
                wikiTokenBox.TextField = "Title";
                wikiTokenBox.DataSource = tokenData;
                wikiTokenBox.DataBind();
            }
            wikiTokenBox.ValueField = "Value";
            wikiTokenBox.TextField = "Title";
        }


        public void LoadHelpCard()
        {
            List<HelpCard> lstHelpCard = HelpCardManager.Load(x => x.Deleted != true);
            if (lstHelpCard != null && lstHelpCard.Count != 0)
            {
                var tokenData = lstHelpCard.Select(x => new {
                    Title = $"{x.Title} ({x.TicketId}) ",
                    Value = x.TicketId

                }).ToList();
                helpCardTokenBox.ValueField = "Value";
                helpCardTokenBox.TextField = "Title";
                helpCardTokenBox.DataSource = tokenData;
                helpCardTokenBox.DataBind();
            }
            helpCardTokenBox.ValueField = "Value";
            helpCardTokenBox.TextField = "Title";
        }
   
    }
}