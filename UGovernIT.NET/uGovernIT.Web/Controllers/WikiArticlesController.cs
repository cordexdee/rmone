using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Utility.Entities.Common;
using DevExpress.DataProcessing;
using System.Data;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;
using System.Text.RegularExpressions;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/WikiArticles")]
    public class WikiArticlesController : ApiController
    {
        private ApplicationContext _applicationContext = null;
        private WikiArticlesManager wikiArticlesManager = null;
        ModuleViewManager ModuleManager = null;
        RequestTypeManager requestTypeManager = null;
        UserProfileManager userProfileManager = null;
        WikiContentsManager wikiContentsManager = null;
        WikiDiscussionManager wikiDiscussionManager = null;
        WikiLinksManager wikiLinksManager = null;
        TicketRelationManager ticketRelationManager = null;
        WikiReviewsManager wikiReviewsManager = null;
        List<WikiArticles> WikiArticles = new List<WikiArticles>();
        List<WikiArticlesResponse> wikiArticlesResponse = new List<WikiArticlesResponse>();
        List<WikiDiscussionResponse> wikiDiscussionResponse = new List<WikiDiscussionResponse>();
        List<WikiCategory> WikiMenuLeftNavigation = new List<WikiCategory>();
        TicketRelationshipHelper TicketRelationshipHelper = null;
        ConfigurationVariableManager configurationVariableManager = null;
        UserProfile user = HttpContext.Current.CurrentUser();
        UserProfileManager userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        WikiMenuLeftNavigationManager wikiMenuLeftNavigationManager = null;

        public WikiArticlesController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            wikiArticlesManager = new WikiArticlesManager(_applicationContext);
            ModuleManager = new ModuleViewManager(_applicationContext);
            requestTypeManager = new RequestTypeManager(_applicationContext);
            userProfileManager = new UserProfileManager(_applicationContext);
            wikiContentsManager = new WikiContentsManager(_applicationContext);
            wikiDiscussionManager = new WikiDiscussionManager(_applicationContext);
            wikiLinksManager = new WikiLinksManager(_applicationContext);
            ticketRelationManager = new TicketRelationManager(_applicationContext);
            wikiReviewsManager = new WikiReviewsManager(_applicationContext);
            TicketRelationshipHelper = new TicketRelationshipHelper(_applicationContext);
            configurationVariableManager = new ConfigurationVariableManager(_applicationContext);
            wikiMenuLeftNavigationManager = new WikiMenuLeftNavigationManager(_applicationContext);

        }
        // GET: api/WikiArticles
        [HttpGet]
        [Route("Getwikies")]
        public async Task<IHttpActionResult> GetWikies(string pageIndex)
        {
            await Task.FromResult(0);
            try
            {
                int pageNumber = Convert.ToInt32(pageIndex);
                // 

                var total = WikiArticles.Count();
                var pageSize = 15; // set your page size, which is number of records per page

                //var page = 2; // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

                var skip = pageSize * (pageNumber - 1);

                var canPage = skip < total;

                //if (!canPage) // do what you wish if you can page no further
                //    return NotFound;

                //Ticket obj = new Ticket(_applicationContext, "WIKI");
                //obj.GetNewTicketId();
                WikiArticles = wikiArticlesManager.Load(x => x.Deleted == false, null, skip, pageSize, null);


                //WikiArticles = WikiArticles.Select(p => p)
                // .Skip(skip)
                // .Take(pageSize)
                // .ToList();

                if (WikiArticles.Count > 0)
                {
                    WikiArticles.ForEach(x =>
                    {
                        var user = userProfileManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                        else
                            x.CreatedBy = "--";
                    }
                     );
                    foreach (var article in WikiArticles)
                    {
                        var obj = new WikiArticlesResponse();
                        obj.ID = article.ID;
                        obj.CreatedByUser = article.CreatedBy;
                        obj.Created = Convert.ToDateTime(article.Created).ToString("MMM-dd-yyyy hh:mm tt");
                        obj.ModuleName = article.ModuleNameLookup;
                        obj.Title = article.Title;
                        obj.WikiDislikesCount = article.WikiDislikesCount;
                        obj.WikiLikesCount = article.WikiLikesCount;
                        obj.WikiTicketId = article.TicketId;
                        obj.WikiFavorites = article.WikiFavorites;
                        obj.WikiLinksCount = article.WikiLinksCount;
                        obj.WikiServiceRequestCount = article.WikiServiceRequestCount;
                        obj.WikiDiscussionCount = article.WikiDiscussionCount;
                        obj.WikiViews = article.WikiViews;
                        obj.WikiAverageScore = article.WikiAverageScore;
                        string authorizedToview = article.AuthorizedToView;
                        if (authorizedToview != null)
                        {

                            string[] result = Regex.Split(authorizedToview, ",");
                            foreach (var user in result)
                            {
                                bool isUserInGroup = userManager.CheckUserIsInGroup(user, _applicationContext.CurrentUser);
                                if (result.Contains(_applicationContext.CurrentUser.Id) || isUserInGroup)
                                    wikiArticlesResponse.Add(obj);

                            }

                        }
                        if (authorizedToview == null) /// authorizedToview to everyone
                        {
                            wikiArticlesResponse.Add(obj);
                        }
                    }

                    if (wikiArticlesResponse != null)
                    {
                        string jsonmodules = JsonConvert.SerializeObject(wikiArticlesResponse);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }

                }


                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWikies: " + ex);
                return InternalServerError();
            }

        }

        // GET: api/WikiArticles/5

        [HttpGet]
        [Route("GetwikiesByRequestType")]

        public async Task<IHttpActionResult> GetwikiesByRequestType(string id)
        {
            List<WikiArticles> WikiArticlesByRequestType = new List<WikiArticles>();

            try
            {
                // this item change further have to remove ...implementing logic for all types of filteration e.g catogory etc
                if (id == "undefined" || id == "0")
                {
                    return await GetWikies("1");
                }
                else
                {
                    var module = string.Empty;
                    //long requestType = Convert.ToInt64(id);
                    long requestType = 0;
                    long.TryParse(id, out requestType);

                    var requestTypeObject = requestTypeManager.LoadByID(requestType);
                    module = requestTypeObject.ModuleNameLookup;
                    WikiArticles = wikiArticlesManager.Load();
                    if (WikiArticles.Count > 0)
                    {
                        WikiArticlesByRequestType = WikiArticles.Where(x => x.RequestTypeLookup == requestType && x.ModuleNameLookup == module).ToList();

                        WikiArticlesByRequestType.ForEach(x =>
                        {
                            var user = userProfileManager.LoadById(x.CreatedBy);
                            if (user != null)
                                x.CreatedBy = user.Name;
                            else
                                x.CreatedBy = "--";
                        });

                        foreach (var article in WikiArticlesByRequestType)
                        {
                            var obj = new WikiArticlesResponse();
                            obj.ID = article.ID;
                            obj.CreatedByUser = article.CreatedBy;
                            obj.Created = Convert.ToDateTime(article.Created).ToString("MMM-dd-yyyy hh:mm tt");
                            obj.ModuleName = article.ModuleNameLookup;
                            obj.Title = article.Title;
                            obj.WikiDislikesCount = article.WikiDislikesCount;
                            obj.WikiLikesCount = article.WikiLikesCount;
                            obj.WikiTicketId = article.TicketId;
                            obj.WikiFavorites = article.WikiFavorites;
                            wikiArticlesResponse.Add(obj);
                        }

                        if (wikiArticlesResponse != null)
                        {
                            string jsonmodules = JsonConvert.SerializeObject(wikiArticlesResponse);
                            var response = this.Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                            return ResponseMessage(response);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetwikiesByRequestType: " + ex);
                return InternalServerError();
            }
        }

        // POST: api/WikiArticles
        [HttpPost]
        [Route("CreateWiki")]
        public async Task<IHttpActionResult> CreateWiki([FromBody] AddWikiRequestModel wikiarticle)
        {
            await Task.FromResult(0);
            try
            {
                // wikiticketId
                string ticketId;
                string snapshot = string.Empty;
                Ticket obj = new Ticket(_applicationContext, "WIKI");

                ticketId = obj.GetNewTicketId();
                //txtDescription.Text = UGITUtility.StripHTML(Convert.ToString(item.Body));
                string wikisnapShot = UGITUtility.StripHTML(wikiarticle.HtmlBody);
                // string snapshot = wikisnapShot.Take(150).ToString();
                if (wikisnapShot != null && wikisnapShot.Length > 150)
                    snapshot = wikisnapShot.Substring(0, 150);
                else
                    snapshot = wikisnapShot;

                // module name
                //string[] breakModuleName = wikiarticle.Module.Split('(');
                //string name = breakModuleName[1];
                //string moduleNameLookup = name.Trim(')');

                WikiContents wikiContent = new WikiContents();
                wikiContent.Content = wikiarticle.HtmlBody;
                wikiContent.TicketId = ticketId;
                wikiContent.Title = wikiarticle.WikiTitle;
                wikiContentsManager.Insert(wikiContent);


                WikiArticles article = new WikiArticles();
                article.TicketId = ticketId;
                article.Title = wikiarticle.WikiTitle;
                article.RequestTypeLookup = wikiarticle.RequestType;
                article.ModuleNameLookup = wikiarticle.Module;
                article.WikiContentID = wikiContent.ID;
                article.WikiSnapshot = snapshot;
                article.WikiLikesCount = 0;
                article.WikiLinksCount = 0;
                article.WikiViews = 0;
                article.WikiDislikesCount = 0;
                article.WikiFavorites = false;
                article.WikiServiceRequestCount = 0;
                article.WikiDiscussionCount = 0;
                article.WikiAverageScore = 0;
                wikiArticlesManager.Insert(article);
                Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Created Wiki Article: {article.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateWiki: " + ex);
                return InternalServerError();
            }
        }

        // PUT: api/WikiArticles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WikiArticles/5
        public void Delete(int id)
        {
        }

        #region treeview
        [HttpGet]
        [Route("GetModules")]
        public async Task<IHttpActionResult> GetModules()
        {
            await Task.FromResult(0);
            try
            {
                var WikiTreeList = new List<WikiTree>();
                var requestTypeList = requestTypeManager.Load();

                var pNode = new List<WikiParentNode>();

                requestTypeList = requestTypeList.OrderBy(m => m.ModuleNameLookup).ToList();
                // code for  converting into tree model
                var requestTypeListGroupbyModules = requestTypeList.GroupBy(x => x.ModuleNameLookup).ToList();
                //requestTypeList.ForEach(x =>
                //{
                //    if (string.IsNullOrEmpty(x.Category))
                //    {
                //        x.Category = x.SubCategory;
                //    }
                //    if (string.IsNullOrEmpty(x.SubCategory))
                //    {
                //        x.SubCategory = x.Category;
                //    }
                //});

                var treeView = (from x in requestTypeList
                                group x by new { x.ModuleNameLookup } into grouped
                                select new WikiTree()
                                {
                                    text = grouped.Key.ModuleNameLookup,
                                    //WikiSubCategory = grouped.Select(c => new WikiSubCategory()
                                    //{
                                    //    SubCategory = c.SubCategory,
                                    //    RequestType =
                                    //                            }
                                    //).ToList()
                                    items = (from x in grouped
                                             group x by new { x.Category } into category
                                             select new WikiTree()
                                             {
                                                 text = category.Key.Category,
                                                 items = (from x in category
                                                          group x by new { x.SubCategory } into SubCategory
                                                          select new WikiTree()
                                                          {
                                                              text = SubCategory.Key.SubCategory,
                                                              items = (from x in SubCategory
                                                                       group x by new { x.RequestType, x.ID } into requestType
                                                                       select new WikiTree()
                                                                       {
                                                                           Id = requestType.Key.ID,
                                                                           text = requestType.Key.RequestType,
                                                                       }).ToList()
                                                          }

                                                 ).ToList()
                                             }).ToList()
                                }).ToList();

                WikiTreeList.AddRange(treeView);
                var WikiTreeList1 = new List<WikiTree>();

                foreach (var item in treeView)
                {
                    WikiTree ModuleChild = new WikiTree();
                    WikiTree TempCategoryChild = new WikiTree();

                    if (!string.IsNullOrEmpty(item.text)) // module check
                    {
                        ModuleChild.text = item.text;
                        ModuleChild.Id = item.Id;
                        //tree1.items = item.items;
                        foreach (var category in item.items)
                        {
                            WikiTree categoryChild = new WikiTree();

                            if (!string.IsNullOrEmpty(category.text))
                            {
                                categoryChild.text = category.text;
                                //tree1.items = category.items;
                                foreach (var subcategory in category.items)
                                {
                                    WikiTree subcategoryChild = new WikiTree();

                                    if (!string.IsNullOrEmpty(subcategory.text))
                                    {
                                        subcategoryChild.Id = subcategory.Id;
                                        subcategoryChild.text = subcategory.text;
                                        subcategoryChild.items = subcategory.items;

                                        categoryChild.items.Add(subcategoryChild);
                                    }
                                    else
                                    {
                                        categoryChild.items = subcategory.items;
                                        //ModuleChild.items.Add(categoryChild);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var subcategory in category.items)
                                {
                                    WikiTree subcategoryChild = new WikiTree();

                                    if (!string.IsNullOrEmpty(subcategory.text))
                                    {
                                        subcategoryChild.Id = subcategory.Id;
                                        subcategoryChild.text = subcategory.text;
                                        subcategoryChild.items = subcategory.items;

                                        categoryChild.items.Add(subcategoryChild);
                                    }
                                    else
                                    {
                                        categoryChild.items = subcategory.items;
                                        //ModuleChild.items.Add(categoryChild);
                                    }
                                }
                            }
                            ModuleChild.items.Add(categoryChild);
                        }
                    }
                    //ModuleChild.items.Add(TempCategoryChild);
                    WikiTreeList1.Add(ModuleChild);
                }

                pNode.Add(new WikiParentNode()
                {
                    text = "All",
                    items = WikiTreeList1
                });

                if (pNode != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(pNode);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetModules: " + ex);
                return InternalServerError();
            }

        }

        //[HttpGet]
        //[Route("GetRequestTypes")]
        //public async Task<IHttpActionResult> GetRequestTypes()
        //{


        //}
        #endregion

        #region ddlmodule and ddlRequestype

        [HttpGet]
        [Route("GetDdlModules")]

        public async Task<IHttpActionResult> GetDdlModules()
        {
            await Task.FromResult(0);
            try
            {
                //ModuleManager.get//DataTable modules = uGITCache.GetModuleList(ModuleType.All)
                List<UGITModule> modules = ModuleManager.Load(x => x.EnableModule == true);

                if (modules != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(modules);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDdlModules: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetDdlRequestType")]

        public async Task<IHttpActionResult> GetDdlRequestType(string moduleName)
        {
            await Task.FromResult(0);
            //string[] breakModuleName = moduleName.Split('(');
            //string name = breakModuleName[1];
            //string moduleNameLookup = name.Trim(')');

            try
            {
                string moduleNameLookup = moduleName.TrimStart();

                var requestTypeList = requestTypeManager.Load();
                var requestTypeListByModule = requestTypeList.Where(x => x.ModuleNameLookup == moduleNameLookup).ToList();

                //List<WikiParams> WikiParams = new List<WikiParams>();

                //var obj1 = new WikiParams();
                //obj1.ID = 1;
                //obj1.Name = "wiki";

                //var obj2 = new WikiParams();
                //obj2.ID = 2;
                //obj2.Name = "RMM";
                //var obj3 = new WikiParams();
                //obj3.ID = 3;
                //obj3.Name = "WIKI";

                //WikiParams.Add(obj1);
                //WikiParams.Add(obj2);
                //WikiParams.Add(obj3);


                if (requestTypeListByModule != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(requestTypeListByModule);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDdlRequestType: " + ex);
                return InternalServerError();
            }

        }


        #endregion

        #region Wikidetail Api

        [HttpGet]
        [Route("GetwikiDetails")]
        public async Task<IHttpActionResult> GetwikiDetails(string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                ticketId = ticketId.Trim();
                WikiDetailsResponse wikidetail = new WikiDetailsResponse();
                List<WikiDiscussion> WikiDiscussions = new List<WikiDiscussion>();
                List<WikiLinks> WikiLinks = new List<WikiLinks>();
                List<WikiReviews> WikiReviews = new List<WikiReviews>();
                //WikiArticles article = new WikiArticles();
                //"where ModuleNameLookup='" + ModuleName + "' and IsDeleted = 0"
                //string query = "TicketId ='" + ticketId + "' ";
                string query = $"TicketId ='{ticketId.Trim()}'";

                //string getQuery = "where TicketId = '" + ticketId + "' ";
                string getQuery = $"TicketId ='{ticketId.Trim()}'";


                //article = wikiArticlesManager.Get(getQuery);
                WikiArticles article = wikiArticlesManager.Load(query).FirstOrDefault();
                if (!string.IsNullOrEmpty(article.ModifiedBy))
                {
                    var user = userProfileManager.LoadById(article.ModifiedBy);
                    if (user != null)
                        article.ModifiedBy = user.Name;
                }
                if (!string.IsNullOrEmpty(article.CreatedBy))
                {
                    var user = userProfileManager.LoadById(article.CreatedBy);
                    if (user != null)
                        article.CreatedBy = user.Name;
                }
                WikiDiscussions = wikiDiscussionManager.Load(query);
                WikiLinks = wikiLinksManager.Load(query);
                // WikiContents wikiContent = wikiContentsManager.Get(getQuery);
                WikiContents wikiContent = wikiContentsManager.Load(query).FirstOrDefault();

                WikiReviews = wikiReviewsManager.Load(getQuery);

                if (wikiContent != null)
                {
                    if (!string.IsNullOrEmpty(wikiContent.ModifiedBy))
                    {
                        var user = userProfileManager.LoadById(wikiContent.ModifiedBy);

                        if (user != null)
                            wikidetail.wikiContents.ModifiedBy = user.Name;
                    }

                    wikidetail.WikiContentsModified = Convert.ToDateTime(wikiContent.Modified).ToString("MMM-dd-yyyy hh:mm tt");
                    wikidetail.WikiLinks.AddRange(WikiLinks);
                    wikidetail.WikiDiscussions.AddRange(WikiDiscussions);
                    wikidetail.WikiReviews.AddRange(WikiReviews);
                    wikidetail.TicketID = ticketId;
                }
                if (article != null)
                {
                    //wikidetail.WikiArticles.Deleted = article.Deleted;
                    wikidetail.WikiArticles = article;
                }
                WikiDiscussions.ForEach(x =>
                {
                    var user = userProfileManager.LoadById(x.CreatedBy);
                    if (user != null)
                        x.CreatedBy = user.Name;
                    else
                        x.CreatedBy = "--";
                }
            );

                if (wikiContent != null)
                {
                    wikidetail.wikiContents.Title = wikiContent.Title;
                    wikidetail.wikiContents.Content = wikiContent.Content;
                }
                else
                {
                    wikidetail.wikiContents.Content = string.Empty;
                }

                //wikidetail.wikiContents.Title = "ITIL KPIs Financial Management";

                // checking about permissions on wiki
                //bool IsAllowToView = GetPermissionsForWiki("edit", ticketId);

                wikidetail.IsAuthorizedToView = true;
                if (wikidetail != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikidetail);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    //var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&ticketId=" + ticketId;
                    //response.Headers.Location = new Uri(url);
                    return ResponseMessage(response);
                    //return Redirect(url);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetwikiDetails: " + ex);
                return InternalServerError();
            }

        }



        [HttpPost]
        [Route("CreateWikiDiscussion")]

        public async Task<IHttpActionResult> CreateWikiDiscussion([FromUri] WikiDiscussionRequestModel wikiDiscussion)
        {
            await Task.FromResult(0);
            WikiDiscussion obj = new WikiDiscussion();

            try
            {
                obj.Comment = wikiDiscussion.Comment;
                obj.TicketId = wikiDiscussion.TicketId;
                wikiDiscussionManager.Insert(obj);

                List<WikiDiscussion> listOfwikies = new List<WikiDiscussion>();
                string query = "TicketID ='" + obj.TicketId + "' ";

                listOfwikies = wikiDiscussionManager.Load(query);


                // update wiki discussion count in wikiArticle table
                string getQuery = "where TicketId = '" + obj.TicketId + "' ";
                var article = wikiArticlesManager.Get(getQuery);

                if (article != null)
                {
                    if (listOfwikies.Count > 0)
                        article.WikiLinksCount = listOfwikies.Count;
                    else
                        article.WikiLinksCount = 0;

                    wikiArticlesManager.Update(article);

                }

                listOfwikies.ForEach(x =>
                {
                    var user = userProfileManager.LoadById(x.CreatedBy);
                    if (user != null)
                        x.CreatedBy = user.Name;
                    else
                        x.CreatedBy = "--";
                }
                );


                foreach (var comment in listOfwikies)
                {
                    var responseobj = new WikiDiscussionResponse();

                    responseobj.ID = comment.ID;
                    responseobj.Comment = comment.Comment;
                    responseobj.WikiTicketID = comment.TicketId;
                    responseobj.CreatedByUser = comment.CreatedBy;
                    responseobj.CreatedDate = Convert.ToDateTime(comment.Created).ToString("MMM-dd-yyyy hh:mm tt");
                    wikiDiscussionResponse.Add(responseobj);
                }

                if (wikiDiscussionResponse != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikiDiscussionResponse);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateWikiDiscussion: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("CreateWikiLinks")]
        public async Task<IHttpActionResult> CreateWikiLinks([FromUri] WikiLinksRequestModel wikiLinks)
        {
            await Task.FromResult(0);
            WikiLinks obj = new WikiLinks();
            try
            {
                obj.URL = wikiLinks.URL;
                obj.TicketId = wikiLinks.TicketId;
                //obj.WikiTicketID = "WIKI-19-000002";
                obj.Title = wikiLinks.Title;
                wikiLinksManager.Insert(obj);



                List<WikiLinks> listlinks = new List<WikiLinks>();
                string query = "TicketId ='" + obj.TicketId + "' ";
                listlinks = wikiLinksManager.Load(query);

                // update wiki links count in wikiArticle table
                string getQuery = "where TicketId = '" + obj.TicketId + "' ";
                var article = wikiArticlesManager.Get(getQuery);

                if (article != null)
                {
                    if (listlinks.Count > 0)
                        article.WikiLinksCount = listlinks.Count;
                    else
                        article.WikiLinksCount = 0;

                    wikiArticlesManager.Update(article);

                }

                if (listlinks != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(listlinks);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateWikiLinks: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetLinksToOtherTickets")]
        public async Task<IHttpActionResult> GetLinksToOtherTickets(string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                ticketId = ticketId.Trim();
                string moduleName = string.Empty
                       , viewUrl = string.Empty
                       , sourceURL = HttpContext.Current.Request["source"] != null ? HttpContext.Current.Request["source"] : HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath); ;
                ModuleViewManager ObjModuleViewManager = new ModuleViewManager(_applicationContext);
                List<WikiRelatedTicket> WikiRelatedTickets = new List<WikiRelatedTicket>();

                List<TicketRelation> ListOfOtherLinkedTickets = TicketRelationshipHelper.GetTicketChildList(ticketId);

                foreach (var otherTicket in ListOfOtherLinkedTickets)
                {
                    WikiRelatedTicket wikiRelatedTicket = new WikiRelatedTicket();

                    DataRow row = Ticket.GetCurrentTicket(_applicationContext, otherTicket.ChildModuleName, otherTicket.ChildTicketID);
                    wikiRelatedTicket.ID = otherTicket.ID;
                    wikiRelatedTicket.ChildTicketTitle = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                    wikiRelatedTicket.ChildTicketID = otherTicket.ChildTicketID;

                    moduleName = uHelper.getModuleNameByTicketId(otherTicket.ChildTicketID);
                    viewUrl = ObjModuleViewManager.LoadByName(moduleName).StaticModulePagePath;
                    wikiRelatedTicket.Link = string.Format("window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", otherTicket.ChildTicketID), wikiRelatedTicket.ChildTicketTitle.Replace("'", ""), sourceURL, 90, 90);

                    WikiRelatedTickets.Add(wikiRelatedTicket);
                    // var dataTable = TicketRelationshipHelper.GetTicketDetail(otherTicket.ChildTicketID);

                }

                // update wiki discussion count in wikiArticle table
                string getQuery = "where TicketId = '" + ticketId + "' ";
                var article = wikiArticlesManager.Get(getQuery);

                if (article != null)
                {
                    if (WikiRelatedTickets.Count > 0)
                        article.WikiLinksCount = WikiRelatedTickets.Count;
                    else
                        article.WikiLinksCount = 0;

                    wikiArticlesManager.Update(article);

                }


                if (WikiRelatedTickets != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(WikiRelatedTickets);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetLinksToOtherTickets: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetWikiDiscussions")]
        public async Task<IHttpActionResult> GetWikiDiscussions(string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                ticketId = ticketId.Trim();
                List<WikiDiscussion> listOfwikies = new List<WikiDiscussion>();
                string query = "TicketId ='" + ticketId + "' ";
                //string query = "WikiTicketID ='WIKI-19-000003'";

                listOfwikies = wikiDiscussionManager.Load(query);

                listOfwikies.ForEach(x =>
                {
                    var user = userProfileManager.LoadById(x.CreatedBy);
                    if (user != null)
                        x.CreatedBy = user.Name;
                    else
                        x.CreatedBy = "--";
                }
                );


                foreach (var comment in listOfwikies)
                {
                    var responseobj = new WikiDiscussionResponse();

                    responseobj.ID = comment.ID;
                    responseobj.Comment = comment.Comment;
                    responseobj.WikiTicketID = comment.TicketId;
                    responseobj.CreatedByUser = comment.CreatedBy;
                    responseobj.CreatedDate = Convert.ToDateTime(comment.Created).ToString("MMM-dd-yyyy hh:mm tt");
                    wikiDiscussionResponse.Add(responseobj);
                }

                if (wikiDiscussionResponse != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikiDiscussionResponse);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWikiDiscussions: " + ex);
                return InternalServerError();
            }
        }


        [HttpPost]
        [Route("DeleteWikiDiscussion")]

        public async Task<IHttpActionResult> DeleteWikiDiscussion(long ID)
        {
            await Task.FromResult(0);
            try
            {
                var obj = wikiDiscussionManager.LoadByID(ID);
                bool IsDeleted = wikiDiscussionManager.Delete(obj);

                if (IsDeleted)
                {
                    List<WikiDiscussion> listOfwikies = new List<WikiDiscussion>();
                    string query = "TicketId ='" + obj.TicketId + "' ";

                    listOfwikies = wikiDiscussionManager.Load(query);

                    listOfwikies.ForEach(x =>
                    {
                        var user = userProfileManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                        else
                            x.CreatedBy = "--";
                    }
                  );

                    // update wiki links count in wikiArticle table
                    string getQuery = "where TicketId = '" + obj.TicketId + "' ";
                    var article = wikiArticlesManager.Get(getQuery);

                    if (article != null)
                    {
                        if (listOfwikies.Count > 0)
                            article.WikiLinksCount = listOfwikies.Count;
                        else
                            article.WikiLinksCount = 0;

                        wikiArticlesManager.Update(article);

                    }


                    if (listOfwikies != null)
                    {
                        string jsonmodules = JsonConvert.SerializeObject(listOfwikies);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteWikiDiscussion: " + ex);
                return InternalServerError();
            }

        }



        [HttpPost]
        [Route("DeleteWikiLinks")]

        public async Task<IHttpActionResult> DeleteWikiLinks(long ID)
        {
            await Task.FromResult(0);
            try
            {
                var obj = wikiLinksManager.LoadByID(ID);
                bool IsDeleted = wikiLinksManager.Delete(obj);

                if (IsDeleted)
                {
                    List<WikiLinks> listOfwikies = new List<WikiLinks>();
                    string query = "TicketId ='" + obj.TicketId + "' ";

                    listOfwikies = wikiLinksManager.Load(query);

                    listOfwikies.ForEach(x =>
                    {
                        var user = userProfileManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                        else
                            x.CreatedBy = "--";
                    }
                  );

                    // update wiki links count in wikiArticle table
                    string getQuery = "where TicketId = '" + obj.TicketId + "' ";
                    var article = wikiArticlesManager.Get(getQuery);

                    if (article != null)
                    {
                        if (listOfwikies.Count > 0)
                            article.WikiLinksCount = listOfwikies.Count;
                        else
                            article.WikiLinksCount = 0;

                        wikiArticlesManager.Update(article);

                    }



                    if (listOfwikies != null)
                    {
                        string jsonmodules = JsonConvert.SerializeObject(listOfwikies);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }

                }
                return Ok();
            }
            catch (Exception ex )
            {
                ULog.WriteException($"An Exception Occurred in DeleteWikiLinks: " + ex);
                return InternalServerError();
            }

        }


        [HttpPost]
        [Route("DeleteLinksToOtherTickets")]

        public async Task<IHttpActionResult> DeleteLinksToOtherTickets(long ID, string ticketId)
        {
            await Task.FromResult(0);
            TicketRelationshipHelper TicketRelationshipHelper = new TicketRelationshipHelper(_applicationContext);

            List<TicketRelation> ListOfOtherLinkedTickets = new List<TicketRelation>();
            bool IsDeleted = false;

            try
            {
                var obj = ticketRelationManager.LoadByID(ID);
                IsDeleted = ticketRelationManager.Delete(obj);


                if (IsDeleted)
                {

                    ListOfOtherLinkedTickets = TicketRelationshipHelper.GetTicketChildList(ticketId);

                }

                // update wiki links count in wikiArticle table
                string getQuery = "where TicketId = '" + ticketId + "' ";
                var article = wikiArticlesManager.Get(getQuery);

                if (article != null)
                {
                    if (ListOfOtherLinkedTickets.Count > 0)
                        article.WikiLinksCount = ListOfOtherLinkedTickets.Count;
                    else
                        article.WikiLinksCount = 0;

                    wikiArticlesManager.Update(article);

                }

                if (ListOfOtherLinkedTickets != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(ListOfOtherLinkedTickets);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteLinksToOtherTickets: " + ex);
                return InternalServerError();
            }
        }




        #endregion

        #region Edit Wikiarticle

        [HttpGet]
        [Route("GetWikiArticleByTicketId")]
        public async Task<IHttpActionResult> GetWikiArticleByTicketId(string id)
        {
            await Task.FromResult(0);
            try
            {
                string query = $" ticketid ='{id.Trim()}'";

                // WikiArticles wikiArticle = wikiArticlesManager.Get(query);

                WikiArticles wikiArticle = wikiArticlesManager.Load(query).FirstOrDefault();

                //    var content = wikiContentsManager.LoadByID((long)wikiArticle.WikiContentID);

                //wikiArticle.Content = content.Content; //html body;
                // WikiHtmlEditor.Html = Server.HtmlDecode(Convert.ToString(dtWikiDetails.Rows[0][DatabaseObjects.Columns.WikiDescription]));

                if (wikiArticle != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikiArticle);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWikiArticleByTicketId: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("UpdateWiki")]
        public async Task<IHttpActionResult> UpdateWiki([FromBody] AddWikiRequestModel updatedWikiArticle)
        {
            await Task.FromResult(0);
            // wikiticketId
            //string ticketId = "WIKI-19-000003";
            //Ticket obj = new Ticket(_applicationContext, "WIKI");

            //ticketId = obj.GetNewTicketId();

            // module name
            //string[] breakModuleName = wikiarticle.Module.Split('(');
            //string name = breakModuleName[1];
            //string moduleNameLookup = name.Trim(')');

            ////string query = "where TicketId ='" + updatedWikiArticle.TicketId + "' ";

            try
            {
                string query = $"{DatabaseObjects.Columns.TicketId} ='{updatedWikiArticle.TicketId}'";

                // WikiContents obj = wikiContentsManager.Get(query);
                WikiContents obj = wikiContentsManager.Load(query).FirstOrDefault();

                //WikiArticles wikiArticle = wikiArticlesManager.Get(query);
                WikiArticles wikiArticle = wikiArticlesManager.Load(query).FirstOrDefault();

                WikiContents wikiContent = new WikiContents();
                wikiContent.Content = updatedWikiArticle.HtmlBody;
                wikiContent.TicketId = updatedWikiArticle.TicketId;
                wikiContent.Title = updatedWikiArticle.WikiTitle;
                wikiContent.ID = obj.ID;

                wikiContentsManager.Update(wikiContent);

                WikiArticles article = new WikiArticles();
                article.ID = wikiArticle.ID;
                article.TicketId = updatedWikiArticle.TicketId;
                article.Title = updatedWikiArticle.WikiTitle;
                article.RequestTypeLookup = updatedWikiArticle.RequestType;
                article.ModuleNameLookup = updatedWikiArticle.Module;
                article.WikiContentID = wikiContent.ID;
                article.WikiLikesCount = wikiArticle.WikiLikesCount;
                article.WikiLinksCount = wikiArticle.WikiLinksCount;
                article.WikiViews = wikiArticle.WikiViews;
                article.WikiDislikesCount = wikiArticle.WikiDislikesCount;
                article.WikiFavorites = wikiArticle.WikiFavorites;
                article.WikiServiceRequestCount = wikiArticle.WikiServiceRequestCount;
                article.WikiDiscussionCount = wikiArticle.WikiDiscussionCount;
                article.WikiAverageScore = wikiArticle.WikiAverageScore;
                wikiArticle.ModifiedBy = HttpContext.Current.GetManagerContext().CurrentUser.Id;
                wikiArticlesManager.Update(article);
                Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Updated Wiki Article: {article.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateWiki: " + ex);
                return InternalServerError();
            }
        }

        #endregion

        #region Like and Dislike Wiki

        [HttpPost]
        [Route("LikeWikiArticle")]
        public async Task<IHttpActionResult> LikeWikiArticle([FromUri] string ticketId)
        {
            await Task.FromResult(0);
            try
            {
                //ticketId = "WIKI-19-000003";
                string query = "where TicketId ='" + ticketId + "' ";
                // WikiContents obj = wikiContentsManager.Get(query);
                WikiArticles wikiArticle = wikiArticlesManager.Get(query);

                //WikiArticles article = new WikiArticles();

                long? disLikesCount = wikiArticle.WikiLikesCount;
                disLikesCount++;
                wikiArticle.WikiLikesCount = disLikesCount;
                wikiArticlesManager.Update(wikiArticle);
                //updateWikiReviews(ticketId, ReviewType.Like, ReviewStatus.likeWiki);

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in LikeWikiArticle: " + ex);
                return InternalServerError();
            }

        }

        [HttpPost]
        [Route("DisLikeWikiArticle")]
        public async Task<IHttpActionResult> DisLikeWikiArticle([FromUri] string ticketId)
        {
            await Task.FromResult(0);
            //ticketId = "WIKI-19-000003";

            //updateWikiReviews(ticketId, ReviewType.Dislike, ReviewStatus.likeWiki);

            string query = "where TicketId ='" + ticketId + "' ";

            try
            {
                // WikiContents obj = wikiContentsManager.Get(query);
                WikiArticles wikiArticle = wikiArticlesManager.Get(query);
                long? disLikesCount = wikiArticle.WikiDislikesCount;
                disLikesCount++;
                wikiArticle.WikiDislikesCount = disLikesCount;
                wikiArticlesManager.Update(wikiArticle);

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DisLikeWikiArticle: " + ex);
                return InternalServerError();
            }

        }

        [HttpPost]
        [Route("FavoriteWikiArticle")]
        public async Task<IHttpActionResult> FavoriteWikiArticle([FromUri] string ticketId, ReviewType ReviewType)
        {
            await Task.FromResult(0);
            //ticketId = "WIKI-19-000003";

            updateWikiReviews(ticketId, ReviewType, ReviewStatus.FavoriteWiki);

            //string query = "where WikiTicketID ='" + ticketId + "' ";

            //// WikiContents obj = wikiContentsManager.Get(query);
            //WikiArticles wikiArticle = wikiArticlesManager.Get(query);

            ////WikiArticles article = new WikiArticles();

            //long? disLikesCount = wikiArticle.WikiLikesCount;
            //disLikesCount++;
            //wikiArticle.WikiDislikesCount = disLikesCount;
            //wikiArticlesManager.Update(wikiArticle);

            return Ok();

        }
        [HttpPost]
        [Route("UnFavoriteWikiArticle")]
        public async Task<IHttpActionResult> UnFavoriteWikiArticle([FromUri] string ticketId)
        {
            await Task.FromResult(0);
            ticketId = "WIKI-19-000016";

            updateWikiReviews(ticketId, ReviewType.UnFavorite, ReviewStatus.FavoriteWiki);

            //string query = "where WikiTicketID ='" + ticketId + "' ";

            //// WikiContents obj = wikiContentsManager.Get(query);
            //WikiArticles wikiArticle = wikiArticlesManager.Get(query);

            ////WikiArticles article = new WikiArticles();

            //long? disLikesCount = wikiArticle.WikiLikesCount;
            //disLikesCount++;
            //wikiArticle.WikiDislikesCount = disLikesCount;
            //wikiArticlesManager.Update(wikiArticle);

            return Ok();

        }


        public void updateWikiReviews(string ticketId, ReviewType ReviewType, ReviewStatus ReviewStatus)
        {
            var obj = new WikiReviews();
            string query = "where TicketId ='" + ticketId + "' ";
            bool isConditionExists = false;

            try
            {
                List<WikiReviews> wikiReviews = wikiReviewsManager.Load(query);

                if (wikiReviews != null && wikiReviews.Count > 0)
                {
                    foreach (var item in wikiReviews)
                    {
                        if (((ReviewType != ReviewType.Favorite && ReviewType != ReviewType.UnFavorite) && ReviewStatus != ReviewStatus.FavoriteWiki) && (item.ReviewType == ReviewType.Like && (item.ReviewStatus == ReviewStatus.likeWiki) || (item.ReviewType == ReviewType.Dislike && item.ReviewStatus == ReviewStatus.likeWiki)))
                        {
                            item.ReviewType = ReviewType;
                            item.ReviewStatus = ReviewStatus;
                            wikiReviewsManager.Update(item);
                            isConditionExists = true;
                            break;

                        }
                        if (((ReviewType != ReviewType.Like && ReviewType != ReviewType.Dislike) && ReviewStatus != ReviewStatus.likeWiki) && ((item.ReviewType == ReviewType.Favorite && item.ReviewStatus == ReviewStatus.FavoriteWiki) || (item.ReviewType == ReviewType.UnFavorite && item.ReviewStatus == ReviewStatus.FavoriteWiki)))
                        {
                            item.ReviewType = ReviewType;
                            item.ReviewStatus = ReviewStatus;
                            wikiReviewsManager.Update(item);
                            isConditionExists = true;
                            break;

                        }
                        else
                        {
                            isConditionExists = false;
                        }


                    }
                }

                if (isConditionExists == false)
                {

                    obj.TicketId = ticketId;
                    obj.ReviewType = ReviewType;
                    obj.ReviewStatus = ReviewStatus;
                    wikiReviewsManager.Update(obj);
                }

                UpdateWikiArticleReview(ticketId);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in updateWikiReviews: " + ex);
            }
        }



        private void UpdateWikiArticleReview(string ticketId)
        {
            // ticketId = "WIKI-19-000003";
            List<WikiReviews> wikiReviews = new List<WikiReviews>();
            List<WikiReviews> ListOfLikesArticle = new List<WikiReviews>();
            List<WikiReviews> ListOfDisLikesArticle = new List<WikiReviews>();
            string query = "where TicketId ='" + ticketId + "' ";
            //string likeReviewQuery = string.Format("where WikiTicketID = '{0}' AND ReviewType={1} ", ticketId, 0);  //ReviewType.Like
            //string disLikeReviewQuery = string.Format("where WikiTicketID = '{0}' AND ReviewType={1} ", ticketId, 1); // ReviewType.Dislike

            try
            {
                //var article = wikiArticlesManager.Get(query);
                var article = wikiArticlesManager.Load(query).FirstOrDefault();

                //var wikiReviews = wikiReviewsManager.Get(query);

                wikiReviews = wikiReviewsManager.Load(query);

                ListOfLikesArticle = wikiReviews.Where(x => x.ReviewType == ReviewType.Like && x.ReviewStatus == ReviewStatus.likeWiki).ToList();
                ListOfDisLikesArticle = wikiReviews.Where(x => x.ReviewType == ReviewType.Dislike && x.ReviewStatus == ReviewStatus.likeWiki).ToList();
                var favWiki = wikiReviews.Where(x => x.ReviewType == ReviewType.Favorite && x.ReviewStatus == ReviewStatus.FavoriteWiki).FirstOrDefault();
                var unFavWiki = wikiReviews.Where(x => x.ReviewType == ReviewType.UnFavorite && x.ReviewStatus == ReviewStatus.FavoriteWiki).FirstOrDefault();

                //List<WikiReviews> ListOfLikesArticle = wikiReviewsManager.Load(likeReviewQuery);
                //List<WikiReviews> ListOfDisLikesArticle = wikiReviewsManager.Load(disLikeReviewQuery);



                if (article != null)
                {
                    if (ListOfLikesArticle.Count > 0)
                    {
                        article.WikiLikesCount = ListOfLikesArticle.Count();
                    }

                    else
                    {
                        article.WikiLikesCount = 0;
                    }

                    if (ListOfDisLikesArticle.Count > 0)
                    {
                        article.WikiDislikesCount = ListOfDisLikesArticle.Count;
                    }
                    else
                    {
                        article.WikiDislikesCount = 0;

                    }

                    if (favWiki != null)
                    {
                        if (favWiki.ReviewType == ReviewType.Favorite)
                        {
                            article.WikiFavorites = true;
                        }
                    }
                    else
                    {
                        if (unFavWiki != null)
                        {
                            if (unFavWiki.ReviewType == ReviewType.UnFavorite)
                            {
                                article.WikiFavorites = false;
                            }

                        }


                    }
                    //article.WikiTicketId = "WIKI-19-000003";
                    wikiArticlesManager.Update(article);

                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateWikiArticleReview: " + ex);
            }
        }
        #endregion

        #region Enum WikiReviewType
        public class WikiReviewType
        {
            public long ID { get; set; }
            public string WikiTicketID { get; set; }
            public ReviewType ReviewType { get; set; }
            public double Rating { get; set; }
            public double Score { get; set; }
        }
        #endregion

        #region archive and unarchive and Delete Wiki

        [HttpPost]
        [Route("ArchiveWikiArticle")]

        public async Task<IHttpActionResult> ArchiveWikiArticle(string ticketId)
        {
            await Task.FromResult(0);
            bool isSuccess = ArchiveUnArchiveWiki(ticketId, true);
            if (isSuccess)
            {
                // uHelper.ClosePopUpAndEndResponse(Context);
            }

            return Ok();
        }

        [HttpPost]
        [Route("UnArchiveWikiArticle")]

        public async Task<IHttpActionResult> UnArchiveWikiArticle(string ticketId)
        {
            await Task.FromResult(0);
            bool isSuccess = ArchiveUnArchiveWiki(ticketId, false);
            if (isSuccess)
            {
                // uHelper.ClosePopUpAndEndResponse(Context);
            }

            return Ok();
        }

        public bool ArchiveUnArchiveWiki(String ticketId, Boolean IsArchive)
        {
            ticketId = ticketId.Trim();
            bool isSuccess = false;
            try
            {
                string query = "where TicketId ='" + ticketId + "' ";

                //var article = wikiArticlesManager.Get(query);
                var article = wikiArticlesManager.Load(query).FirstOrDefault();

                if (article != null)
                {
                    article.Deleted = IsArchive;
                    wikiArticlesManager.Update(article);
                    //spListItemArticle.Delete();
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ArchiveUnArchiveWiki: " + ex);
            }
            return isSuccess;
        }


        [HttpPost]
        [Route("DeleteWikiArticle")]

        public async Task<IHttpActionResult> DeleteWikiArticle(string ticketId)
        {
            await Task.FromResult(0);
            ticketId = ticketId.Trim();


            List<WikiDiscussion> WikiDiscussions = new List<WikiDiscussion>();
            List<WikiLinks> WikiLinks = new List<WikiLinks>();
            List<WikiReviews> WikiReviews = new List<WikiReviews>();
            List<TicketRelation> ListOfOtherLinkedTickets = new List<TicketRelation>();

            string query = "TicketId ='" + ticketId + "' ";
            string getQuery = "where TicketId = '" + ticketId + "' ";

            try
            {
                WikiLinks = wikiLinksManager.Load(query);
                WikiDiscussions = wikiDiscussionManager.Load(query);
                WikiReviews = wikiReviewsManager.Load(query);
                ListOfOtherLinkedTickets = TicketRelationshipHelper.GetTicketChildList(ticketId);
                WikiArticles wikiArticle = wikiArticlesManager.Get(getQuery);


                if (WikiLinks.Count > 0)
                {
                    foreach (var item in WikiLinks)
                    {
                        wikiLinksManager.Delete(item);

                    }
                }

                if (WikiDiscussions.Count > 0)
                {
                    foreach (var item in WikiDiscussions)
                    {
                        wikiDiscussionManager.Delete(item);

                    }
                }

                if (WikiReviews.Count > 0)
                {
                    foreach (var item in WikiReviews)
                    {
                        wikiReviewsManager.Delete(item);

                    }

                }

                if (ListOfOtherLinkedTickets.Count > 0)
                {

                    foreach (var item in ListOfOtherLinkedTickets)
                    {
                        TicketRelationshipHelper.DeleteRelation(item.ParentTicketID, item.ChildTicketID);

                    }
                }


                if (wikiArticle != null)
                {
                    Util.Log.ULog.WriteUGITLog(_applicationContext.CurrentUser.Id, $"Deleted Wiki Article: {wikiArticle.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _applicationContext.TenantID);
                    wikiArticlesManager.Delete(wikiArticle);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteWikiArticle: " + ex);
                return InternalServerError();
            }
        }



        #endregion

        #region wikiMenuLeftNavigation
        [HttpGet]
        [Route("GetWikiMenuLeftNavigation")]
        public async Task<IHttpActionResult> GetWikiMenuLeftNavigation()
        {
            await Task.FromResult(0);
            try
            {
                // List<WikiArticles> testlist = new List<WikiArticles>();
                WikiMenuLeftNavigation = wikiMenuLeftNavigationManager.Load();

                foreach (var item in WikiMenuLeftNavigation)
                {
                    var articlelist = BindWikiNaviagtionList(item.ColumnType, item.ID.ToString());
                    item.ItemOrder = articlelist.Count; // just to avoid more foreach iteration used itemorder to hold count of articles
                }
                if (WikiMenuLeftNavigation != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(WikiMenuLeftNavigation);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWikiMenuLeftNavigation: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetWikiNavigationList")]

        public async Task<IHttpActionResult> GetWikiNavigationList(string navigationType, string navigationId)
        {
            await Task.FromResult(0);
            try
            {
                var articlelist = BindWikiNaviagtionList(navigationType, navigationId);
                wikiArticlesResponse.AddRange(articlelist);
                if (wikiArticlesResponse != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikiArticlesResponse);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWikiNavigationList: " + ex);
                return InternalServerError();
            }

            //if (navegationType == "CustomWiki")
            //{
            //    SPListItem spWikiNavigationItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.WikiLeftNavigation, navegationId);

            //    string conditionexpression = Convert.ToString(spWikiNavigationItem[DatabaseObjects.Columns.ConditionalLogic]).Replace("[", "").Replace("]", "").Replace("&&", "AND").Replace("||", "OR").Replace("true", "1").Replace("false", "0");

            //    DataRow[] dtrow = dtWikiArticles.Select(conditionexpression);
            //    if (dtrow != null && dtrow.Length > 0)
            //        dtWikiArticles = dtrow.CopyToDataTable();
            //    else
            //        dtWikiArticles = null;
            //}

            //if (dtWikiArticles != null)
            //{
            //    dtWikiArticles = SetWikiDetailsDefaults(dtWikiArticles);
            //}
            //  return dtWikiArticles;


        }



        private List<WikiArticlesResponse> BindWikiNaviagtionList(string navigationType, string navigationId)
        {

            List<WikiArticlesResponse> articlesResponse = new List<WikiArticlesResponse>();

            //Boolean IsAdminUser = CheckUserIsAdmin();
            string query = string.Empty;
            //List<WikiArticles> articles = new List<WikiArticles>();
            //List<string> expressions = new List<string>();
            //SPQuery query = new SPQuery();
            //if (!IsAdminUser)
            //{
            //    expressions.Add(string.Format(@"<Or><Or><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='User'>{1}</Value></Eq>
            //                                <Membership Type='CurrentUserGroups'><FieldRef Name='{0}'/></Membership></Or>
            //                                <IsNull><FieldRef Name='{0}' /></IsNull></Or>", DatabaseObjects.Columns.AuthorizedToView, SPContext.Current.Web.CurrentUser.ID));
            //}
            try
            {
                if (navigationType == "AllWiki")
                {
                    query = string.Format("IsDeleted={0}", 0);
                }
                else if (navigationType == "PopularWiki")
                {
                    query = string.Format("Deleted={0} and WikiViews= {1}", 0, 10);

                    //expressions.Add(string.Format("<Neq><FieldRef Name='{0}'/><Value Type='Boolean'>{1}</Value></Neq>", DatabaseObjects.Columns.IsDeleted, 1));
                    //expressions.Add(string.Format("<Gt><FieldRef Name='{0}'/><Value Type='Number'>{1}</Value></Gt>", DatabaseObjects.Columns.WikiViewsCount, 10));
                }
                else if (navigationType == "MyWiki")
                {
                    query = string.Format("Deleted={0} and {2} = '{1}'", 0, _applicationContext.CurrentUser.Id, DatabaseObjects.Columns.CreatedByUser);

                    //expressions.Add(string.Format("<Neq><FieldRef Name='{0}'/><Value Type='Boolean'>{1}</Value></Neq>", DatabaseObjects.Columns.IsDeleted, 1));
                    //expressions.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='User'>{1}</Value></Eq>", DatabaseObjects.Columns.Author, SPContext.Current.Web.CurrentUser.ID));
                }
                else if (navigationType == "ArchiveWiki")
                {
                    query = string.Format("Deleted = {0} ", 1);

                    //expressions.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Boolean'>{1}</Value></Eq>", DatabaseObjects.Columns.IsDeleted, 1));
                }

                else if (navigationType == "FavoriteWiki")
                {
                    query = string.Format("Deleted={0} and {3} = '{1}' and WikiFavorites={2}", 0, _applicationContext.CurrentUser.Id, 1, DatabaseObjects.Columns.CreatedByUser);

                }

                else
                {
                    // query.Query = string.Format("<Where><Or><Or><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='User'>{1}</Value></Eq><Membership Type='CurrentUserGroups'><FieldRef Name='{0}'/></Or></Membership><IsNull><FieldRef Name='{0}' /></IsNull></Or></Where>", DatabaseObjects.Columns.AuthorizedToView, SPContext.Current.Web.CurrentUser.ID);
                }
                //query.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(expressions, expressions.Count - 1, true));
                //query.ViewFields = uGITCache.ModuleConfigCache.GetCachedModuleViewFields(SPContext.Current.Web, "WIK");
                //query.ViewFieldsOnly = true;
                //DataTable dtWikiArticles = SPListHelper.GetDataTable(DatabaseObjects.Lists.WikiArticles, query);


                WikiArticles = wikiArticlesManager.Load(query);

                if (WikiArticles != null && WikiArticles.Count > 0)
                {
                    WikiArticles.ForEach(x =>
                    {
                        var user = userProfileManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                        else
                            x.CreatedBy = "--";
                    }
                     );
                    foreach (var article in WikiArticles)
                    {
                        var obj = new WikiArticlesResponse();
                        obj.ID = article.ID;
                        obj.CreatedByUser = article.CreatedBy;
                        obj.Created = Convert.ToDateTime(article.Created).ToString("MMM-dd-yyyy hh:mm tt");
                        obj.ModuleName = article.ModuleNameLookup;
                        obj.Title = article.Title;
                        obj.WikiDislikesCount = article.WikiDislikesCount;
                        obj.WikiLikesCount = article.WikiLikesCount;
                        obj.WikiTicketId = article.TicketId;
                        obj.WikiFavorites = article.WikiFavorites;
                        obj.WikiLinksCount = article.WikiLinksCount;
                        obj.WikiServiceRequestCount = article.WikiServiceRequestCount;
                        obj.WikiDiscussionCount = article.WikiDiscussionCount;
                        obj.WikiViews = article.WikiViews;
                        obj.WikiAverageScore = article.WikiAverageScore;
                        string authorizedToview = article.AuthorizedToView;
                        if (authorizedToview != null)
                        {

                            string[] result = Regex.Split(authorizedToview, ",");
                            foreach (var user in result)
                            {
                                bool isUserInGroup = userManager.CheckUserIsInGroup(user, _applicationContext.CurrentUser);
                                if (result.Contains(_applicationContext.CurrentUser.Id) || isUserInGroup)
                                    articlesResponse.Add(obj);

                            }

                        }
                        if (authorizedToview == null) /// authorizedToview to everyone
                        {
                            articlesResponse.Add(obj);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BindWikiNaviagtionList: " + ex);
            }
            return articlesResponse;

            //if (navegationType == "CustomWiki")
            //{
            //    SPListItem spWikiNavigationItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.WikiLeftNavigation, navegationId);

            //    string conditionexpression = Convert.ToString(spWikiNavigationItem[DatabaseObjects.Columns.ConditionalLogic]).Replace("[", "").Replace("]", "").Replace("&&", "AND").Replace("||", "OR").Replace("true", "1").Replace("false", "0");

            //    DataRow[] dtrow = dtWikiArticles.Select(conditionexpression);
            //    if (dtrow != null && dtrow.Length > 0)
            //        dtWikiArticles = dtrow.CopyToDataTable();
            //    else
            //        dtWikiArticles = null;
            //}

            //if (dtWikiArticles != null)
            //{
            //    dtWikiArticles = SetWikiDetailsDefaults(dtWikiArticles);
            //}
            //  return dtWikiArticles;

        }
        #endregion

        #region WikiPermission
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPermissions")]

        public async Task<IHttpActionResult> GetPermissions(string Action, string TicketId)
        {
            await Task.FromResult(0);
            try
            {
                WikiPermission wikiPermission = new WikiPermission();

                bool IsAllowed = GetPermissionsForWiki(Action, TicketId);

                wikiPermission.AuthorizedToView = IsAllowed;
                if (wikiPermission != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(wikiPermission);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPermissions: " + ex);
                return InternalServerError();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsWikiOwner(UserProfile user)
        {
            bool isWikiOwner = false;

            try
            {
                ConfigurationVariable wikiOwners = configurationVariableManager.GetValue(ConfigConstants.WikiOwners, false);

                if (wikiOwners != null)
                {
                    if (wikiOwners.Type == "Text" && wikiOwners.KeyValue != null)
                    {
                        isWikiOwner = CheckUserIsInGroup(wikiOwners.KeyValue, _applicationContext.CurrentUser);
                    }
                    if (wikiOwners.Type == "User" && wikiOwners.KeyValue != null)
                    {
                        // KeyValue is userId
                        if (wikiOwners.KeyValue == _applicationContext.CurrentUser.Id)
                        {
                            isWikiOwner = true;

                        }
                        //KeyValue is usergroup
                        else
                        {
                            isWikiOwner = userManager.CheckUserInGroup(_applicationContext.CurrentUser.Id, wikiOwners.KeyValue);

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in IsWikiOwner: " + ex);
            }
            return isWikiOwner;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public bool GetPermissionsForWiki(string Action, string TicketId)
        {
            // Didn't find right permissions
            bool IsAuthorizedUser = false;
            bool AuthorizedToView = false;
            string getQuery = string.Empty;
            bool isCreator = false;

            try
            {
                if (!string.IsNullOrEmpty(TicketId))
                {
                    getQuery = "where TicketId = '" + TicketId + "' ";
                }

                // If current user is super admin, just return true

                if (userManager.IsUGITSuperAdmin(_applicationContext.CurrentUser) || userManager.IsAdmin(_applicationContext.CurrentUser))
                    AuthorizedToView = true;

                if (!string.IsNullOrEmpty(Action))
                {
                    if (IsWikiOwner(_applicationContext.CurrentUser))
                        AuthorizedToView = true;

                    if (Action == "add")
                    {
                        ConfigurationVariable wikiCreators = configurationVariableManager.GetValue(ConfigConstants.WikiCreators, false);
                        if (wikiCreators != null)
                        {
                            if (wikiCreators.Type == "Text" && wikiCreators.KeyValue != null)
                            {
                                isCreator = CheckUserIsInGroup(wikiCreators.KeyValue, _applicationContext.CurrentUser);
                            }
                            if (wikiCreators.Type == "User" && wikiCreators.KeyValue != null)
                            {
                                // KeyValue is userId
                                if (wikiCreators.KeyValue == _applicationContext.CurrentUser.Id)
                                {
                                    IsAuthorizedUser = true;

                                }
                                //KeyValue is usergroup
                                else
                                {
                                    isCreator = userManager.CheckUserInGroup(_applicationContext.CurrentUser.Id, wikiCreators.KeyValue);

                                }

                            }
                        }

                        //bool isCreator = userManager.CheckIfUserGroup(_applicationContext.CurrentUser.Id, wikiCreators);

                        if (isCreator == true || IsAuthorizedUser)
                            AuthorizedToView = true;
                    }
                    else // action == "edit-delete"
                    {
                        WikiArticles wikiArticle = new WikiArticles();
                        wikiArticle.TicketId = TicketId;
                        // WikiArticleHelper wikiHelper = new WikiArticleHelper();
                        var article = wikiArticlesManager.Get(getQuery);
                        string WikiUserId = string.Empty;
                        if (article != null)
                        {
                            // SPFieldUserValueCollection spFieldAuthor = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(spColl[0][DatabaseObjects.Columns.Author]));
                            if (article.CreatedBy != null)
                                WikiUserId = article.CreatedBy;
                        }

                        if (WikiUserId == _applicationContext.CurrentUser.Id)
                            AuthorizedToView = true;

                    }
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPermissionsForWiki: " + ex);
            }
            return AuthorizedToView;
        }

        /// <summary>
        /// Checks if user is part of the group(s) passed in
        /// </summary>
        /// <param name="groupNames">One or more groups separated by ","</param>
        /// <param name="user">User whose membership to check</param>
        /// <returns></returns>
        public bool CheckUserIsInGroup(string groupNames, UserProfile user)
        {
            if (string.IsNullOrEmpty(groupNames) || user == null)
                return false;

            string[] groups = UGITUtility.SplitString(groupNames, uGovernIT.Utility.Constants.Separator6);
            if (groups != null && groups.Count() > 0)
                return userProfileManager.IsUserinGroups(groupNames, user.UserName);

            return false;
        }

        #endregion

    }
}
