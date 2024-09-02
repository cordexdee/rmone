using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class Node
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string stageStep { get; set; }
        public string picture { get; set; }
        public string predecessor { get; set; }
        public string itemOrder { get; set; }
        public string level { get; set; }
        public string taskType { get; set; }
        public string status { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string ContainerId { get; set; }
        public double leftX { get; set; }
        public double topY { get; set; }
        public string onClickString { get; set; }
        public string publicTicketId { get; set; }








    }
    
    public class Edges
    {
        public string id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string myLocalType { get; set; }
        public int fromShapePoint { get; set; }

    }


    public class TaskNode
    {
        public string Id { get; set; }
        public string Predecessor { get; set; }
        public int step { get; set; }
        public int itemOrder { get; set; }

    }
}
