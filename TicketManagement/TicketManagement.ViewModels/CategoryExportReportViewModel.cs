using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class CategoryExportReportViewModel
    {
        public string CategoryName { get; set; }
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
}
