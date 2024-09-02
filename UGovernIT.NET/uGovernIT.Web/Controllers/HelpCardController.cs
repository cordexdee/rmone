using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/HelpCard")]
    public class HelpCardController : ApiController
    {
        private ApplicationContext _applicationContext = null;
        private HelpCardManager helpCardManager = null;
        HelpCardContentManager helpCardContentManager = null;
        List<HelpCard> listHelpCard = new List<HelpCard>();                              
        ConfigurationVariableManager configurationVariableManager = null;
        UserProfile user = HttpContext.Current.CurrentUser();
        UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        List<HelpCardResponse> helpCardResponse = new List<HelpCardResponse>();
        public HelpCardController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            helpCardManager = new HelpCardManager(_applicationContext);            
            configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
            helpCardContentManager = new HelpCardContentManager(_applicationContext);
        }

        [HttpGet]
        [Route("GetHelpCards")]
        public async Task<IHttpActionResult> GetHelpCards(string Category = "All Categories", bool isShowArchive = false)
        {
            await Task.FromResult(0);
            try
            {
                if (string.IsNullOrEmpty(Category))
                {
                    return BadRequest();
                }

                if (Category.EqualsIgnoreCase("All Categories"))
                    listHelpCard = helpCardManager.Load(x => x.Deleted == isShowArchive);
                else
                    listHelpCard = helpCardManager.Load(x => x.Deleted == isShowArchive && x.Category == Category);

                if (listHelpCard.Count > 0)
                {
                    listHelpCard.ForEach(x =>
                    {
                        var user = userManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                    });
                    foreach (var article in listHelpCard)
                    {
                        var obj = new HelpCardResponse();
                        obj.ID = article.ID;
                        obj.CreatedBy = article.CreatedBy;
                        obj.Created = Convert.ToDateTime(article.Created).ToString("MMM-dd-yyyy hh:mm tt");
                        obj.HelpCardTitle = article.Title;
                        obj.HelpCardTicketId = article.TicketId;
                        obj.AgentLookUp = article.AgentLookUp;
                        obj.HelpCardCategory = article.Category;
                        obj.Description = article.Description;
                        var helpCardContent = helpCardContentManager.Load(x => x.TicketId == article.TicketId).FirstOrDefault();
                        if (helpCardContent != null)
                        {
                            obj.AgentContent = helpCardContent.AgentContent;
                            obj.HelpCardContent = helpCardContent.Content;
                        }
                        string authorizedToview = article.AuthorizedToView;
                        if (authorizedToview != null)
                        {

                            string[] result = Regex.Split(authorizedToview, ",");
                            foreach (var user in result)
                            {
                                bool isUserInGroup = userManager.CheckUserIsInGroup(user, _applicationContext.CurrentUser);
                                if (result.Contains(_applicationContext.CurrentUser.Id) || isUserInGroup)
                                    helpCardResponse.Add(obj);

                            }

                        }
                        if (authorizedToview == null) /// authorizedToview to everyone
                        {
                            helpCardResponse.Add(obj);
                        }
                    }

                    if (helpCardResponse != null)
                    {
                        string jsonmodules = JsonConvert.SerializeObject(helpCardResponse);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                }
                //string jsonmodulesEmpty = JsonConvert.SerializeObject("[]");
                //var responseEmpty = this.Request.CreateResponse(HttpStatusCode.OK);
                //responseEmpty.Content = new StringContent(jsonmodulesEmpty, Encoding.UTF8, "application/json");
                //return ResponseMessage(responseEmpty);
                return Ok(helpCardResponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetHelpCards: " + ex);
                return InternalServerError();
            }
        }




        // POST: api/WikiArticles
        [HttpPost]
        [Route("CreateHelpCard")]
        public async Task<IHttpActionResult> CreateHelpCard([FromBody] AddHelpCardModel HelpCardModel)
        {
            await Task.FromResult(0);
            // wikiticketId
            string ticketId;
            try
            {
                Ticket obj = new Ticket(_applicationContext, "HLP");
                ticketId = obj.GetNewTicketId();

                string wikisnapShot = UGITUtility.StripHTML(HelpCardModel.HelpCardContent);
                string snapshot = wikisnapShot.Take(150).ToString();
                if (wikisnapShot != null && wikisnapShot.Length > 150)
                    snapshot = wikisnapShot.Substring(0, 150);
                else
                    snapshot = wikisnapShot;

                HelpCardContent helpCardContent = new HelpCardContent();
                helpCardContent.Content = HelpCardModel.HelpCardContent;
                helpCardContent.TicketId = ticketId;
                helpCardContent.Title = HelpCardModel.HelpCardTitle;
                helpCardContent.AgentContent = HelpCardModel.AgentContent;
                helpCardContentManager.Insert(helpCardContent);


                HelpCard helpCard = new HelpCard();
                helpCard.TicketId = ticketId;
                helpCard.Title = HelpCardModel.HelpCardTitle;
                helpCard.HelpCardContentID = helpCardContent.ID;
                helpCard.Category = HelpCardModel.HelpCardCategory;
                helpCard.Description = HelpCardModel.Description;

                if (HelpCardModel.listAgents != null)
                {
                    helpCard.AgentLookUp = string.Join(Constants.Separator, HelpCardModel.listAgents.Select(x => x.Id));
                }
                helpCardManager.Insert(helpCard);
                Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Created Help Card: {helpCard.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateHelpCard: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("DuplicateHelpCard")]
        public async Task<IHttpActionResult> DuplicateHelpCard(string duplicateTicketId)
        {
            await Task.FromResult(0);
            try
            {
                // wikiticketId
                string query = $" ticketid ='{duplicateTicketId.Trim()}'";

                HelpCard originalHelpCard = helpCardManager.Load(query).FirstOrDefault();
                HelpCardContent originalhelpCardHelpCardContent = helpCardContentManager.Load(query).FirstOrDefault();

                if (originalHelpCard != null && originalhelpCardHelpCardContent != null)
                {

                    string ticketId;
                    List<HelpCard> allHelpCard = helpCardManager.Load();
                    Ticket obj = new Ticket(_applicationContext, "HLP");
                    ticketId = obj.GetNewTicketId();

                    //string wikisnapShot = UGITUtility.StripHTML(originalhelpCardHelpCardContent.Content);
                    //string snapshot = wikisnapShot.Take(150).ToString();
                    //if (wikisnapShot != null && wikisnapShot.Length > 150)
                    //    snapshot = wikisnapShot.Substring(0, 150);
                    //else
                    //    snapshot = wikisnapShot;


                    string newHelpCardTitle = originalHelpCard.Title + "-Copy";
                    int i = 1;
                    while (allHelpCard.Exists(x => x.Title == newHelpCardTitle))
                    {
                        newHelpCardTitle = originalHelpCard.Title + "-Copy" + i;
                        i++;
                    }


                    HelpCardContent helpCardContent = new HelpCardContent();
                    helpCardContent.Content = originalhelpCardHelpCardContent.Content;
                    helpCardContent.TicketId = ticketId;
                    helpCardContent.AgentContent = originalhelpCardHelpCardContent.AgentContent;
                    helpCardContent.Title = newHelpCardTitle;
                    helpCardContentManager.Insert(helpCardContent);


                    HelpCard helpCard = new HelpCard();
                    helpCard.TicketId = ticketId;
                    helpCard.Title = newHelpCardTitle;
                    helpCard.HelpCardContentID = helpCardContent.ID;
                    helpCard.Category = originalHelpCard.Category;
                    helpCard.Description = originalHelpCard.Description;
                    helpCard.AgentLookUp = originalHelpCard.AgentLookUp;
                    helpCardManager.Insert(helpCard);

                }

                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DuplicateHelpCard: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<IHttpActionResult> GetCategories(bool isShowArchive = false)
        {
            await Task.FromResult(0);
            List<string> listCategories = new List<string>();
            try{
                listCategories = helpCardManager.Load(x=>x.Deleted == isShowArchive).Select(x => x.Category).ToList();
                listCategories = listCategories != null && listCategories.Count > 0 ? listCategories.Distinct(StringComparer.CurrentCultureIgnoreCase).OrderBy(x=>x).ToList() : null;
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetCategories: " + ex);
            }
            
            
            return Ok(listCategories);
        }


        [HttpGet]
        [Route("GetHelpCardByTicketId")]
        public async Task<IHttpActionResult> GetHelpCardByTicketId(string id)
        {
            await Task.FromResult(0);
            try
            {
                string query = $" ticketid ='{id.Trim()}'";

                HelpCard helpCard = helpCardManager.Load(query).FirstOrDefault();
                HelpCardContent helpCardContent = helpCardContentManager.Load(query).FirstOrDefault();
                HelpCardResponse helpCardResponse = new HelpCardResponse();
                if (helpCard != null && helpCardContent != null)
                {
                    if (!string.IsNullOrEmpty(helpCard.AgentLookUp))
                    {
                        AgentsManager agentsManager = new AgentsManager(_applicationContext);
                        var agentLookUpId = helpCard.AgentLookUp.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                        List<Agents> lstAgent = agentsManager.Load(x => agentLookUpId.Contains(x.Id.ToString()));
                        foreach (Agents ag in lstAgent)
                        {
                            AgentsDto agentsDto = new AgentsDto();
                            agentsDto.Title = ag.Title;
                            agentsDto.Id = ag.Id;
                            agentsDto.Description = ag.Description;
                            agentsDto.Icon = ag.Icon;
                            helpCardResponse.listAgents.Add(agentsDto);
                        }

                    }
                    helpCardResponse.HelpCardTitle = helpCard.Title;
                    helpCardResponse.HelpCardTicketId = helpCard.TicketId;
                    helpCardResponse.HelpCardContent = helpCardContent.Content;
                    helpCardResponse.Description = helpCard.Description;
                    helpCardResponse.ID = helpCard.ID;
                    helpCardResponse.HelpCardContentID = helpCard.HelpCardContentID;
                    helpCardResponse.AuthorizedToView = helpCard.AuthorizedToView;
                    helpCardResponse.HelpCardCategory = helpCard.Category;
                    helpCardResponse.AgentLookUp = helpCard.Title;
                    helpCardResponse.AgentContent = helpCardContent.AgentContent;
                }


                if (helpCard != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(helpCardResponse);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetHelpCardByTicketId: " + ex);
                return InternalServerError();
            }
        }


        [HttpPost]
        [Route("UpdateHelpCard")]
        public async Task<IHttpActionResult> UpdateHelpCard([FromBody] AddHelpCardModel updatedHelpCard)
        {
            await Task.FromResult(0);
            try
            {
                string query = $"{DatabaseObjects.Columns.TicketId} ='{updatedHelpCard.HelpCardTicketId}'";
                HelpCardContent obj = helpCardContentManager.Load(query).FirstOrDefault();
                HelpCard helpCard = helpCardManager.Load(query).FirstOrDefault();

                obj.Content = updatedHelpCard.HelpCardContent;
                obj.Title = updatedHelpCard.HelpCardTitle;
                obj.AgentContent = updatedHelpCard.AgentContent;
                helpCardContentManager.Update(obj);

                helpCard.Title = updatedHelpCard.HelpCardTitle;
                helpCard.Category = updatedHelpCard.HelpCardCategory;
                helpCard.Description = updatedHelpCard.Description;
                if (updatedHelpCard.listAgents != null)
                {
                    helpCard.AgentLookUp = string.Join(Constants.Separator, updatedHelpCard.listAgents.Select(x => x.Id));
                }
                helpCardManager.Update(helpCard);
                Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Updated Help Card: {helpCard.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateHelpCard: " + ex);
                return InternalServerError();
            }
        }




        #region archive and unarchive and Delete Wiki
        [HttpPost]
        [Route("AsyncUpload")]
        public async Task<IHttpActionResult> AsyncUpload()
        {
            await Task.FromResult(0);
            try
            {
                string targetLocation = HttpContext.Current.Server.MapPath("~/Content/Images/helpcardImage");
                
                if (!Directory.Exists(targetLocation))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(targetLocation);
                }

                var httpRequest = HttpContext.Current.Request;

                // Check if files are available
                if (httpRequest.Files.Count > 0)
                {
                    HttpFileCollection files = httpRequest.Files;
                    var file = files[0];
                    string path = System.IO.Path.Combine(targetLocation, file.FileName);
                    file.SaveAs(path);
                }
               
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in AsyncUpload: " + ex);
                throw new HttpException("Invalid file name");
            }
            return Ok();

        }


        [HttpPost]
        [Route("ArchiveHelpCard")]

        public async Task<IHttpActionResult> ArchiveHelpCard(string ticketId)
        {
            await Task.FromResult(0);
            bool isSuccess = ArchiveUnArchiveHelpCard(ticketId, true);
            if (isSuccess)
            {

            }

            return Ok();
        }

        [HttpPost]
        [Route("UnArchiveHelpCard")]

        public async Task<IHttpActionResult> UnArchiveHelpCard(string ticketId)
        {
            await Task.FromResult(0);
            bool isSuccess = ArchiveUnArchiveHelpCard(ticketId, false);            
            return Ok();
        }

        public bool ArchiveUnArchiveHelpCard(String ticketId, Boolean IsArchive)
        {
            ticketId = ticketId.Trim();
            bool isSuccess = false;
            string query = "where TicketId ='" + ticketId + "' ";            
            var card = helpCardManager.Load(query).FirstOrDefault();

            if (card != null)
            {
                card.Deleted = IsArchive;
                helpCardManager.Update(card);                
            }
            return isSuccess;
        }


        [HttpPost]
        [Route("DeleteHelpCard")]

        public async Task<IHttpActionResult> DeleteHelpCard(string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                if (!string.IsNullOrEmpty(ticketId))
                    ticketId = ticketId.Trim();

                string getQuery = $"{DatabaseObjects.Columns.TicketId} = '{ticketId}'";
                HelpCard helpCard = helpCardManager.Load(getQuery).FirstOrDefault();
                HelpCardContent helpCardContent = helpCardContentManager.Load(getQuery).FirstOrDefault();

                if (helpCardContent != null)
                {
                    helpCardContentManager.Delete(helpCardContent);
                }

                if (helpCard != null)
                {
                    Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Deleted Help Card: {helpCard.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    helpCardManager.Delete(helpCard);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteHelpCard: " + ex);
                return InternalServerError();
            }
        }



        #endregion

        [HttpGet]
        [Route("GetAgents")]
        public async Task<IHttpActionResult> GetAgents()
        {
            await Task.FromResult(0);
            try
            {
                AgentsManager agentsManager = new AgentsManager(_applicationContext);
                List<Agents> lstAgent = agentsManager.Load(x => x.Deleted == false);
                var dtoAgent = lstAgent.Select(x => new
                {
                    Title = x.Title,
                    Id = x.Id,
                    Description = x.Description,
                    Icon = x.Icon
                });
                string jsonmodules = JsonConvert.SerializeObject(dtoAgent);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetAgents: " + ex);
                return InternalServerError();
            }

        }
    }
}