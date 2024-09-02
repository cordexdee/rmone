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
    public partial class PhrasesAddNew : UserControl
    {
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();

        private PhraseManager _phraseManager = null;
        private ModuleViewManager _ObjModuleManager = null;
        ServicesManager _ServicesManager = null;
        private WikiArticlesManager _WikiArticlesManager = null;
        private HelpCardManager _helpCardManager = null;

        LookupValueBoxEdit dropDownBox;
        //Phrases _phrases;

        protected PhraseManager PhraseManager
        {
            get
            {
                if (_phraseManager == null)
                {
                    _phraseManager = new PhraseManager(applicationContext);
                }
                return _phraseManager;
            }
        }

        protected ModuleViewManager ModuleManager
        {
            get
            {
                if (_ObjModuleManager == null)
                {
                    _ObjModuleManager = new ModuleViewManager(applicationContext);
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
                    _ServicesManager = new ServicesManager(applicationContext);
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
                if (_helpCardManager == null)
                {
                    _helpCardManager = new HelpCardManager(HttpContext.Current.GetManagerContext());
                }
                return _helpCardManager;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            LoadPhrase();
            BindModule();
            // BindSevices();
            LoadWiki();
            LoadHelpCard();
            ServicesManager.loadDdlServices(ref DDLservices);
            dropDownBox = new LookupValueBoxEdit();
            dropDownBox.ID = "ddRequestType";
            dropDownBox.isRequestType = true;
            dropDownBox.FieldName = DatabaseObjects.Columns.TicketRequestTypeLookup;
            dropDownBox.gridView.SettingsBehavior.AllowSelectByRowClick = true;
            dropDownBox.gridView.ClientSideEvents.SelectionChanged = "requestTypeSelectionChanged";
            dropDownBox.gridView.Width = Unit.Percentage(100);
            dropDownBox.gridView.EnableCallBacks = true;
            dropDownBox.ModuleName = "TSR";
            dropDownBox.CssClass = "rmmLookup-valueBoxEdit";
            //dropDownBox.dropBox.DropDownWindowWidth = 555;

            divRequestType.Controls.Add(dropDownBox);

        }


        protected void Page_Load(object sender, EventArgs e)
        {           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Phrases phraseItem = new Phrases();
            phraseItem.Phrase = txtPhrase.Text;
            phraseItem.AgentType = Convert.ToInt32(ddlPhrasesAgentType.SelectedValue);
            if (phraseItem.AgentType == 3)
            {
                var item = ModuleManager.LoadByName("SVC");
                phraseItem.TicketType = Convert.ToString(item.ModuleId);
                divRequestType.Visible = false;
            }
            else
            {
                phraseItem.TicketType = ddlPhraseTicketType.SelectedValue;
            }
            if (!string.IsNullOrEmpty(phraseItem.TicketType))
                phraseItem.RequestType = UGITUtility.StringToLong(dropDownBox.GetValues());
            if (!string.IsNullOrEmpty(DDLservices.SelectedValue))
                phraseItem.Services = Convert.ToInt64(DDLservices.SelectedValue);
            else
                phraseItem.Services = null;

            phraseItem.WikiLookUp = wikiTokenBox.Value.ToString();
            phraseItem.HelpCardLookUp = helpCardTokenBox.Value.ToString();
            PhraseManager.Insert(phraseItem);
            Util.Log.ULog.WriteUGITLog(applicationContext.CurrentUser.Id, $"Added Phrase: {phraseItem.Phrase}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), applicationContext.TenantID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void LoadPhrase()
        {

            var values = Enum.GetValues(typeof(Enums.Agent));
            foreach (var item in values)
            {
                ddlPhrasesAgentType.Items.Add(new ListItem(Convert.ToString(item), ((int)item).ToString()));
            }
            ddlPhrasesAgentType.Items.Insert(0, new ListItem("None", "0"));
        }

        private void BindModule()
        {
            ddlPhraseTicketType.Items.Clear();

            List<UGITModule> lstModule = ModuleManager.Load();
            var Selecteditem = new UGITModule();
            int SelecteditemID=0;
            // ModuleLabel.InnerText = "Module";
            if (lstModule != null && lstModule.Count > 0)
            {
                lstModule = lstModule.Where(x => x.EnableModule).OrderBy(x => x.ModuleName).ToList();

                ddlPhraseTicketType.DataSource = lstModule;
                // ddlPhraseTicketType.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlPhraseTicketType.DataValueField = DatabaseObjects.Columns.ModuleId;
                ddlPhraseTicketType.DataTextField = DatabaseObjects.Columns.Title;
                SelecteditemID = lstModule.Where(x => x.ModuleName == "SVC").FirstOrDefault().ModuleId;
               
            }
           // ddlPhraseTicketType.SelectedValue = lstModule.Where(x => x.ModuleName == "SVC").FirstOrDefault().Title;
            ddlPhraseTicketType.DataBind();
            ddlPhraseTicketType.Items.Insert(0, new ListItem("None", "0"));
            ddlPhraseTicketType.SelectedIndex = ddlPhraseTicketType.Items.IndexOf(ddlPhraseTicketType.Items.FindByValue(Convert.ToString(SelecteditemID)));


        }

        private void BindSevices()
        {
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

        public void LoadWiki()
        {
            List<WikiArticles> lstWikiArticles = wikiArticlesManager.Load(x => x.Deleted != true);
            if (lstWikiArticles != null && lstWikiArticles.Count != 0)
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