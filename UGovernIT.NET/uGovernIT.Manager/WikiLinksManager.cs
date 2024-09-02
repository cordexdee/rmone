using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;
using System.IO;

namespace uGovernIT.Manager
{
    public class WikiLinksManager : ManagerBase<WikiLinks>, IWikiLinksManager
    {
        ApplicationContext _context = null;
        public WikiLinksManager( ApplicationContext context): base(context)
        {
            store = new WikiLinksStore(this.dbContext);
            _context = this.dbContext;
        }
        public bool SaveLink(WikiLinks WikiLinks, byte[] file)
        {
            
            bool isSuccess = false;
            if (WikiLinks != null)
            {
                WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(_context);
                this.Insert(WikiLinks);

                List<WikiArticles> wikiArticlesSPList = wikiArticlesManager.Load(x=>x.TicketId== WikiLinks.TicketId).ToList();
                WikiArticles spCollArticles = wikiArticlesSPList.FirstOrDefault();
                spCollArticles.WikiLinksCount = wikiArticlesSPList.Count;
                wikiArticlesManager.Update(spCollArticles);
                
                isSuccess = true;

            }
            return isSuccess;
        }

        public List<WikiLinks> GetWikiLinks(WikiLinks WikiLinks)
        {
            List<WikiLinks> wikiLinksSPList = Load(x => x.TicketId == WikiLinks.TicketId).ToList();
            return wikiLinksSPList;
        }

        public bool DeleteLink(WikiLinks WikiLinks)
        {
            bool isSuccess = false;
            WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(_context);
            if (WikiLinks != null)
            {
                WikiLinks.Deleted=true;
                this.Delete(WikiLinks);
                List<WikiArticles> wikiArticlesSPList = wikiArticlesManager.Load(x => x.TicketId == WikiLinks.TicketId).ToList();
                WikiArticles spCollArticles = wikiArticlesSPList.FirstOrDefault();
                spCollArticles.WikiLinksCount = wikiArticlesSPList.Count;
                wikiArticlesManager.Update(spCollArticles);
                isSuccess = true;

            }
            return isSuccess;
        }

    }
    public interface IWikiLinksManager : IManagerBase<WikiLinks>
    {

    }
}
