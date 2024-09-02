using System.Collections.Generic;
using System.Linq;

namespace uGovernIT.Web
{
    public class MessageResult
    {
        public bool Status { get; set; }
        public int ResultCode { get; set; }
        public List<string> InfoMessages { get; set; }
        public List<string> ErrorMessages { get; set; }

        public MessageResult()
        {
            InfoMessages = new List<string>();
            ErrorMessages = new List<string>();
        }

        public void AddError(string message)
        {
            ErrorMessages.Add(message);
        }

        public void AddInfo(string message)
        {
            InfoMessages.Add(message);
        }

        public string GetMessage()
        {
            var message = "";

            if (ErrorMessages.Any())
            {
                message = ErrorMessages.First();
            }

            if (InfoMessages.Any())
            {
                message = InfoMessages.First();
            }

            return message;
        }
    }

    public static class MessageCategory
    {
        public const string TenantDeletion = "Tenant Deletion";
        public const string ResetPassword = "Reset Password";
        public const string DocumentManagement = "Document Management";
    }
}