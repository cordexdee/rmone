using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/phraseSearchlobalTicketCreate")]
    public class PhraseSearchGlobalTicketCreateController : ApiController
    {
        public string newModuleTicketLink = string.Empty;

        public AgentSummary agentSummary;
        public UserProfile user;
        public UserProfileManager UserManager;
        public TicketManager ticketManager = null;

        string requestTypeLookUp = string.Empty;

        private ApplicationContext _applicationContext = null;
        private PhraseManager _phraseManager = null;
        private AvailablePRPAndAssignTo _availablePRPAndAssignTo = null;
        private ModuleViewManager _moduleViewManager = null;
        private CreateTicketManager _createTicketManager;
        private ServicesManager _ServicesManager = null;

        protected Ticket TicketRequest;
        protected DataRow saveTicket;
        protected string PRP = string.Empty;

        List<TicketColumnError> errors = new List<TicketColumnError>();
        List<Phrases> phrases = new List<Phrases>();
        WikiArticlesManager wikiArticlesManager = null;
        HelpCardManager helpCardManager = null;

        public PhraseSearchGlobalTicketCreateController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _phraseManager = new PhraseManager(_applicationContext);
            phrases = _phraseManager.Load();
            _moduleViewManager = new ModuleViewManager(_applicationContext);
            ticketManager = new TicketManager(_applicationContext);
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            _availablePRPAndAssignTo = new AvailablePRPAndAssignTo(_applicationContext);
            _createTicketManager = new CreateTicketManager(_applicationContext);
            _ServicesManager = new ServicesManager(_applicationContext);
            wikiArticlesManager = new WikiArticlesManager(_applicationContext);
            helpCardManager = new HelpCardManager(_applicationContext);
        }

        [HttpGet]
        [Route("GetPhrases")]
        public async Task<IHttpActionResult> GetPhrases()
        {
            await Task.FromResult(0);
            //phrases = _phraseManager.Load();
            try
            {
                if (phrases != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(phrases);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPhrases: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetPhraseWikiAndHelpForAgentBar")]
        public async Task<IHttpActionResult> GetPhraseWikiAndHelpForAgentBar(string WikiLookUp, string HelpCardLookUp)
        {
            await Task.FromResult(0);
            try
            {
                if (string.IsNullOrEmpty(WikiLookUp) && string.IsNullOrEmpty(HelpCardLookUp))
                {
                    return Ok();
                }
                string detailsURL = "/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId=";
                detailsURL = UGITUtility.GetAbsoluteURL(detailsURL);
                List<WikiArticles> wikiArticles = null;
                List<HelpCard> helpCards = null;
                string jsonHelpCard = string.Empty;
                string jsonWikiArticle = string.Empty;
                dynamic dynhelpCard = string.Empty;
                dynamic dynwikiArticle = string.Empty;

                if (!string.IsNullOrEmpty(WikiLookUp))
                {
                    wikiArticles = wikiArticlesManager.Load(x => WikiLookUp.Split(',').Contains(x.TicketId)).ToList();
                }
                if (!string.IsNullOrEmpty(HelpCardLookUp))
                {
                    helpCards = helpCardManager.Load(x => HelpCardLookUp.Split(',').Contains(x.TicketId)).ToList();
                }

                if (helpCards != null && helpCards.Count != 0)
                {
                    dynhelpCard = helpCards.Select(x => new
                    {
                        Type = "wiki",
                        Title = $"{x.Title} ({x.TicketId})",
                        Link = string.Format(@"javascript:openHelpCard('{0}','{1}')", x.TicketId, "")
                    }).ToList();
                }

                if (wikiArticles != null && wikiArticles.Count != 0)
                {
                    dynwikiArticle = wikiArticles.Select(x => new
                    {
                        Type = "wiki",
                        Title = $"{x.Title} ({x.TicketId})",
                        Link = string.Format(@"javascript:window.parent.UgitOpenPopupDialog('{0}','','Help','{2}','{3}',0,'{1}')", $"{ detailsURL}{x.TicketId}", "", 85, 85)
                    }).ToList();

                }
                var data = new
                {
                    dynhelpCard,
                    dynwikiArticle
                };
                string jsonmodules = JsonConvert.SerializeObject(data);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPhraseWikiAndHelpForAgentBar: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("CreateTicket")]
        public async Task<IHttpActionResult> CreateTicket([FromBody] TicketTypeInput input)
        {
            await Task.FromResult(0);
            Phrases phrase = new Phrases();
            UGITModule Module = null;
            string senderID = string.Empty;
            TicketTypeDetail TicketTypeDetail = new TicketTypeDetail();
            try
            {
                if (input != null)
                {
                    if (phrases != null)
                    {
                        phrase = phrases.Where(x => x.Phrase == input.title).FirstOrDefault();
                        if (phrase != null && !string.IsNullOrEmpty(phrase.TicketType) && phrase.TicketType != "0")
                        {
                            Module = _moduleViewManager.LoadByID(UGITUtility.StringToLong(phrase.TicketType));

                            #region new create Ticket
                            //Agent=Auto assign
                            if (Module != null && phrase.AgentType == 2)
                            {
                                TicketTypeDetail = AutoAssignAgent(Module, phrase);
                            }
                            #endregion

                            // if module is assinged to phrase  and if no any agent is assigned  ..then open respective assigned module popup
                            else if (Module != null && phrase.AgentType == 0)
                            {
                                TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString(Module.StaticModulePagePath));
                                TicketTypeDetail.ModuleName = string.Format("New {0} Ticket", Convert.ToString(UGITUtility.moduleTypeName(Module.ModuleName)));
                                TicketTypeDetail.Title = phrase.Phrase;
                                TicketTypeDetail.IsTicketCreated = false;
                                TicketTypeDetail.Description = phrase.Phrase;
                            }

                            //If Module assign to reset Password then @Chetan ask to VP***
                            else if (Module != null && phrase.AgentType == 1)
                            {
                                TicketTypeDetail = TicketTypeDetails(phrase, Module, input);
                            }
                            //agent is services
                            else if (Module != null && (phrase != null && phrase.AgentType == 3))
                            {
                                var id = phrase.Services ?? 0;
                                var service = _ServicesManager.LoadByID(id);
                                //TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString("/Pages/tsr"));
                                TicketTypeDetail.ModuleName = "SVC";
                                TicketTypeDetail.IsTicketCreated = false;
                                TicketTypeDetail.Title = service.Title;
                                TicketTypeDetail.Description = input.title;
                                TicketTypeDetail.ServiceId = service.ID;
                            }
                        }
                        // if module is not assinged to phrase  and if agent Type is resetpassword coz requesteor is complusary ..then by default open tsr
                        //Agent.ResetPassword=1
                        else if (Module == null && (phrase != null && phrase.AgentType == 1))
                        {
                            if (phrase.TicketType == null || phrase.TicketType == "0")
                            {
                                phrase.TicketType = "Service Prime Ticketing System";
                            }

                            TicketTypeDetail = TicketTypeDetails(phrase, Module, input);
                        }

                        // if Ticket type is not assigned
                        else if (Module == null)
                        {
                            TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString("/Pages/tsr"));
                            TicketTypeDetail.ModuleName = string.Format("New {0} Ticket", Convert.ToString("Service Prime Ticketing System"));
                            TicketTypeDetail.IsTicketCreated = false;
                            TicketTypeDetail.Title = input.title;
                            TicketTypeDetail.Description = input.title;
                        }

                        //If Phrase not match then open TSR page
                        else
                        {
                            TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString("/Pages/tsr"));
                            TicketTypeDetail.ModuleName = string.Format("New {0} Ticket", Convert.ToString("Service Prime Ticketing System"));
                            TicketTypeDetail.IsTicketCreated = false;
                            TicketTypeDetail.Title = input.title;
                            TicketTypeDetail.Description = input.title;
                        }
                    }
                }
                else
                {
                    TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString("/Pages/tsr"));
                    TicketTypeDetail.ModuleName = string.Format("New {0} Ticket", Convert.ToString("Service Prime Ticketing System"));
                    TicketTypeDetail.IsTicketCreated = false;
                    TicketTypeDetail.Title = phrase.Phrase;
                    TicketTypeDetail.Description = input.title;
                }

                if (TicketTypeDetail != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(TicketTypeDetail);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateTicket: " + ex);
                return InternalServerError();
            }
        }

        public TicketTypeDetail TicketTypeDetails(Phrases phrase, UGITModule module, [FromBody] TicketTypeInput input)
        {
            TicketTypeDetail ticketTypeDetail = new TicketTypeDetail();
            try
            {
                ticketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Convert.ToString("/Pages/tsr"));
                //ticketTypeDetail.ModuleName = string.Format("New {0} Ticket", phrase.TicketType);
                ticketTypeDetail.ModuleName = string.Format("New {0} Ticket", module.ModuleName);
                ticketTypeDetail.IsTicketCreated = false;
                ticketTypeDetail.Title = phrase.Phrase;
                ticketTypeDetail.Description = input.title;
                ticketTypeDetail.AgentType = phrase.AgentType;
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in TicketTypeDetails: " + ex);
            }

            return ticketTypeDetail;
        }

        public class TicketTypeInput
        {
            public string title { get; set; }
        }

        public class TicketTypeDetail
        {
            public string ModuleName { get; set; }
            public string StaticModulePagePath { get; set; }
            public bool IsTicketCreated = false;
            public string TicketId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int AgentType { get; set; }
            public string Link { get; set; }
            public long ServiceId { get; set; }
        }


        #region Agent Type
        public TicketTypeDetail AutoAssignAgent(UGITModule Module, Phrases phrase)
        {
            TicketTypeDetail TicketTypeDetail = new TicketTypeDetail();
            TicketRequest = new Ticket(_applicationContext, Module.ModuleName);

            DataRow newTicket = GetTableDataManager.GetTableStructure(Module.ModuleTable).NewRow();
            if (!string.IsNullOrEmpty(phrase.Phrase))
            {
                newTicket[DatabaseObjects.Columns.Title] = phrase.Phrase;
                newTicket[DatabaseObjects.Columns.Description] = phrase.Phrase;
            }
            else
            {
                newTicket[DatabaseObjects.Columns.Title] = string.Empty;
                newTicket[DatabaseObjects.Columns.Description] = string.Empty;
            }
            //Set PRP
            var PRPGroupOpen = GetTableDataManager.autoSetPRP(Module.ModuleName, _applicationContext.TenantID, phrase.Phrase, DatabaseObjects.Columns.PRP, 0);

            var PRPGroupClosed = GetTableDataManager.autoSetPRP(Module.ModuleName, _applicationContext.TenantID, phrase.Phrase, DatabaseObjects.Columns.PRP, 1);

            if (PRPGroupOpen.Rows.Count > 0 || PRPGroupClosed.Rows.Count > 0)
                PRP = _availablePRPAndAssignTo.GetPRPOrAssignTo(PRPGroupOpen, PRPGroupClosed, DatabaseObjects.Columns.PRP);

            //Set Request Type
            // var requestTypeTable = GetTableDataManager.autoSetRequestType(Module.ModuleName, _applicationContext.TenantID, phrase.Phrase);

            //if (requestTypeTable != null && requestTypeTable.Rows.Count != 0)
            //{
            //    int maxRequestType = Convert.ToInt32(requestTypeTable.Compute("max([co])", string.Empty));

            //    requestTypeTable = requestTypeTable.Select($"co = {maxRequestType}").CopyToDataTable();
            //}
            // we are taking request Type from the phrases
            if (phrase.RequestType != null && phrase.RequestType != 0)
            {
                // requestTypeLookUp = requestTypeTable.Rows[0][DatabaseObjects.Columns.TicketRequestTypeLookup].ToString();
                requestTypeLookUp = Convert.ToString(phrase.RequestType);
            }

            newTicket[DatabaseObjects.Columns.PRP] = PRP;

            if (!string.IsNullOrEmpty(requestTypeLookUp))
                newTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] = requestTypeLookUp;

            if (!string.IsNullOrEmpty(PRP) || (!string.IsNullOrEmpty(requestTypeLookUp)))
            {
                agentSummary = new AgentSummary()
                {
                    AgentName = "Auto Assign",
                    AgentType = "AutoAssign",
                    ModuleName = Module.ModuleName,
                    IsAgentActivated = true,
                    //StageSteps = "1;4;5;"
                };
            }

            try
            {
                //bool valid = true;
                TicketRequest.Create(newTicket, _applicationContext.CurrentUser, null, null);
                string error = TicketRequest.CommitChanges(newTicket, agentSummary: agentSummary);
                
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(TicketColumnError.AddError(error));
                    //valid = false;
                }

                /// <summary>
                ///Auto Approve agent 
                /// </summary>
                if (!string.IsNullOrEmpty(PRP) || !newTicket.IsNull(DatabaseObjects.Columns.TicketRequestTypeLookup) && Module.ModuleName != ModuleNames.CMDB)
                {
                    //error = TicketRequest.CommitChanges(newTicket, agentSummary: agentSummary);
                    error = TicketRequest.CommitChanges(newTicket, "createButton", HttpContext.Current.Request.Url, agentSummary: agentSummary);

                    _createTicketManager.ApproveTicketRequest(newTicket, errors, agentSummary, "approvebuttonhidden", true, Module.ModuleName);
                    //error = TicketRequest.CommitChanges(newTicket, agentSummary: agentSummary);
                }

                TicketTypeDetail.StaticModulePagePath = UGITUtility.GetAbsoluteURL(Module.StaticModulePagePath);
                TicketTypeDetail.IsTicketCreated = true;
                TicketTypeDetail.ModuleName = Convert.ToString(UGITUtility.moduleTypeName(Module.ModuleName));
                TicketTypeDetail.TicketId = Convert.ToString(newTicket[DatabaseObjects.Columns.TicketId]);
                TicketTypeDetail.Title = string.Format("{0}: {1}", TicketTypeDetail.TicketId, newTicket[DatabaseObjects.Columns.Title]);

                string newModuleTicketTitle = TicketTypeDetail.Title;
                //newModuleTicketLink = string.Format("<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{1}','ticketId={0}','{2}', 90, 90, 0, '{3}' );  \">{0}</a>", saveTicket[DatabaseObjects.Columns.TicketId], Module.StaticModulePagePath, newModuleTicketTitle, "/Layouts/uGovernIT/DelegateControl.aspx");
                newModuleTicketLink = $"<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{Module.StaticModulePagePath}','ticketId={newTicket[DatabaseObjects.Columns.TicketId]}','{newModuleTicketTitle}', 90, 90, 0, '/Layouts/uGovernIT/DelegateControl.aspx' );  \">{newTicket[DatabaseObjects.Columns.TicketId]}</a>";

                //string typeName = Module.ModuleName;
                string informationMsg = string.Format("<div class='informationMsg'><div class='infoMsgSuccess-title'><div class='cteatedTicketNum'>{0}</div></div></div>", newModuleTicketLink);
                TicketTypeDetail.Link = informationMsg;

                return TicketTypeDetail;
            }

            catch (Exception ex)
            {
                ULog.WriteException(ex);
                throw;
            }
        }
        #endregion
    }
}
