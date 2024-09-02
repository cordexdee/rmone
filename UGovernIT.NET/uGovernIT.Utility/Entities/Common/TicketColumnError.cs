using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class TicketColumnError
    {
        public string InternalFieldName { get; set; }
        public string DisplayName { get; set; }
        public string Message { get; set; }
        public ErrorType Type { get; set; }

        public TicketColumnError()
        {
            Type = ErrorType.Error;
        }

        public static TicketColumnError AddError(string message)
        {
            TicketColumnError msg = new TicketColumnError();
            msg.Type = ErrorType.Error;
            msg.InternalFieldName = string.Empty;
            msg.DisplayName = string.Empty;
            msg.Message = message;
            return msg;
        }

        public static TicketColumnError AddError(string internalFieldName, string displayName, string message)
        {
            TicketColumnError msg = new TicketColumnError();
            msg.Type = ErrorType.Error;
            msg.InternalFieldName = internalFieldName;
            msg.DisplayName = displayName;
            msg.Message = message;
            return msg;
        }

        public static TicketColumnError AddCustomError(string internalFieldName, string displayName, string message, ErrorType type)
        {
            TicketColumnError msg = new TicketColumnError();
            msg.Type = type;
            msg.InternalFieldName = internalFieldName;
            msg.DisplayName = displayName;
            msg.Message = message;
            return msg;
        }

        public static TicketColumnError AddMandatoryError(string internalFieldName, string displayName, string message)
        {
            TicketColumnError msg = new TicketColumnError();
            msg.Type = ErrorType.Mandatory;
            msg.DisplayName = displayName;
            msg.InternalFieldName = internalFieldName;
            msg.Message = message;
            return msg;
        }
    }
}
