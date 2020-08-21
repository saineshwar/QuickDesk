using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagement.CommonData
{
    public static class CommonMessages
    {
        public const string CategoryAlreadyExistsMessages = "CategoryName Already Exists";
        public const string CategorySuccessMessages = "CategoryName added successfully.";
        public const string TicketSuccessMessages = "Ticket Generated successfully.";
        public const string TicketErrorMessages = "Ticket Not Generated Please Try after Some time successfully.";
        public const string TicketSuccessReplyMessages = "Ticket Replied successfully.";
        public const string TicketErrorReplyMessages = "Ticket Replied Failed.";
        public const string TicketUpdatedSuccessReplyMessages = "Ticket updated successfully.";
        public const string UserDetailsUpdateSuccessMessages = "Userdetails updated successfully.";

        public const string MenuNameAlreadyExistsMessages = "Menu Name Already Exists";
        public const string MenuSuccessMessages = "Menu added successfully.";
        public const string MenuUpdateMessages = "Menu Update successfully.";
    }

    public static class HistoryTicketCommonMessages
    {
        public const string Message1 = "Ticket created by ";
        public const string Message2 = "Submitted by ";
        public const string Message3 = "Automatically assigned by";
        public const string Message4 = "Status changed by ";
        public const string Message5 = "Priority changed by ";
        public const string Message6 = "Assigned to ";
        public const string Message7 = "Closed by ";
        public const string Message8 = "Edited by ";
        public const string Message9 = "Edited reply by ";
        public const string Message10 = "Resolved by ";
        public const string Message11 = "Inprogress by ";
        public const string Message12 = "OnHold by ";
        public const string Message13 = "Replied by ";
    }
}