using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagement.CommonData
{
    public static class StatusMain
    {
        public enum Status
        {
            Open = 1,
            Resolved = 2,
            InProgress = 3,
            OnHold = 4,
            RecentlyEdited = 5,
            Replied = 6,
            Deleted = 7,
            Overdue = 8,
            Escalation = 9,
            Closed = 10
        }

        public enum Roles
        {
            SuperAdmin = 1,
            User = 2,
            Admin = 3,
            Agent = 4,
            AgentAdmin = 5,
            Hod = 6
        }

        public enum Priority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4
        }

        public enum Activities
        {
            Created = 1,
            AutoAgentAssigned,
            Replied,
            Restored,
            EditedTicket,
            EditedTicketReply,
            Resolved,
            AutoAssigned,
            DeleteTicket,
            DeleteTicketReply,
            PriorityChanged,
            CategoryChanged,
            StatusChanged,
            ManuallyAssigedTicket,
            RepliedandChangedStatus,
            DeletedAttachment,
            DeleteReplyAttachment,
            AutoFirstResponseDue,
            AutoEveryresponseDue,
            AutoResolutionDue
        }
    }
}