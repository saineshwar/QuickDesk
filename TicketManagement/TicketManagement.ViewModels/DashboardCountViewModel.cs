using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class DashboardCountViewModel
    {
        public long? NewTicketCount { get; set; }
        public long? ResolvedTicketCount { get; set; }
        public long? InProgressTicketCount { get; set; }
        public long? RepiledTicketCount { get; set; }
        public long? DeletedTicketCount { get; set; }
        public long? OnHoldTicketCount { get; set; }
        public long? RecentlyEditedTicketCount { get; set; }
        public long? OverdueTicketCount { get; set; }
        public long? EscalatedTicketCount { get; set; }
        public long? ClosedTicketCount { get; set; }
    }
}
