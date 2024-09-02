using System.Collections.Generic;

namespace uGovernIT.Utility.Entities.Common
{
    public class WikiSubCategory
    {
        public string text { get; set; } 
        public List<WikiRequestType> items { get; set; }
        // public List<string> items { get; set; }
        public WikiSubCategory()
        {
            items = new List<WikiRequestType>();
        }


    }
}
