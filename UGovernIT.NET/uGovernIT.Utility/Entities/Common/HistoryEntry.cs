using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class HistoryEntry
    {
        // Actual history entry text
        public string entry { get; set; }

        // Username that created the entry
        public string createdBy { get; set; }

        public string Picture { get; set; }

        // Timestamp of entry as string in local format
        public string created { get; set; }
        public string ChangedOwner { get; set; }
        //keep track of comment ,is it private or not
        public bool IsPrivate { get; set; }
        //Added by mudassir 10 march 2020
        private string strNotificationId = string.Empty;
        public int Index { get; set; }
        public string NotificationID { get { return this.strNotificationId; } set { this.strNotificationId = value; } }
        //

        public string PrivateCommentImage { get; set; }

    }

    public class ResourceTimeSheetEntry
    {
        // Username that created the entry
        public string createdBy { get; set; }

        // Timestamp of entry as string in UTC or local format
        public string created { get; set; }

        public string status { get; set; }

        // Actual history entry text
        public string entry { get; set; }
    }

    public class ActionHistory
    {
        public static string ActionName = string.Empty;
    }

    public class Comments
    {
       public List<Comment> lstComments { get; set; }
    }

    public class Comment
    {
        public string Created { get; set; }
        public string CreatedBy { get; set; }
        public string Entry { get; set; }
        public string TicketID { get; set; }
    }
}
