using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class WikiTree
    {
        public long Id { get; set; }
        //public string Name { get; set; }
        //public string InternalName { get; set; }
        //public string Type { get; set; }
        //public List<WikiTree> childNode{get;set;}
        public string text { get; set; }
        public List<WikiTree> items { get; set; }
        public WikiTree()
        {
            items = new List<WikiTree>();
        }


        //public List<WikiCategory> items { get; set; }
        //public WikiTree()
        //{
        //    items = new List<WikiCategory>();
        //}

    }
}
