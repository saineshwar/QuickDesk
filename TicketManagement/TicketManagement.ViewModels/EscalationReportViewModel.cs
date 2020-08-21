using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class EscalationReportViewModel
    {
        public string TrackingId { get; set; }
        public string CategoryName { get; set; }
        public string TicketAssignedDate { get; set; }
        public string FirstEscalationDate { get; set; }
        public string SecondEscalationDate { get; set; }
        public string Stage1User { get; set; }
        public string Stage2User { get; set; }

    }

    public class DeletedTicketHistoryReportViewModel
    {
        public string TrackingId { get; set; }
        public string CreatedUser { get; set; }
        public string DeletedUser { get; set; }
        public string CategoryName { get; set; }
        public string AssignedDate { get; set; }
        public string DeletedDate { get; set; }
    }

    public class PriorityWiseTicketStatusReportViewModel
    {
        public string PriorityName { get; set; }
        public string TicketAssignedDate { get; set; }
        public int? Total { get; set; }
        public int? OpenTicket { get; set; }
        public int? ResolvedTicket { get; set; }
        public int? InProgressTicket { get; set; }
        public int? OnHoldTicket { get; set; }
        public int? RecentlyEditedTicket { get; set; }
        public int? RepliedTicket { get; set; }
        public int? DeletedTicket { get; set; }
        public int? OverdueTicket { get; set; }
        public int? EscalationTicket { get; set; }
        public int? ClosedTicket { get; set; }
    }

    public class UserDetailsReportViewModel
    {
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string CreateDate { get; set; }
        public string FirstLoginDate { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }

    }

    public class UserWiseCheckinCheckOutReportViewModel
    {
        public string AgentName { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string TotalHours { get; set; }
    }
}
