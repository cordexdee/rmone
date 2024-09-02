using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
   public class ServiceResponseTreeNode
    {
        public string MName { get; set; }
        public string TicketID { get; set; }
        public long ID { get; set; }
        public string Text { get; set; }

       /// <summary>
       /// 1 = task, 0 = ticket
       /// </summary>
        public int Type { get; set; }

        public ServiceResponseTreeNode()
        {
            MName = string.Empty;
            TicketID = string.Empty;
            ID = 0;
            Text = string.Empty;
        }
    }

   public class ServiceResponseTreeNodeParent
   {
       public string MName { get; set; }
       public string TicketId { get; set; }
       public int ID { get; set; }
       public string Text { get; set; }

       public List<ServiceResponseTreeNode> ServiceResponseTreeNode { get; set; }

       public ServiceResponseTreeNodeParent()
       {
           MName = string.Empty;
           TicketId = string.Empty;
           ID = 0;
           Text = string.Empty;
           ServiceResponseTreeNode = new List<ServiceResponseTreeNode>();
       }
   }
}
