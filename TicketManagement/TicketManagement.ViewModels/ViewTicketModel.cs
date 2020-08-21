using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ViewTicketModel
    {
        public long TicketId { get; set; }
        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public short? PriorityId { get; set; }
        public short? StatusId { get; set; }
        public bool StatusAssigned { get; set; }
        public long? UserId { get; set; }
        public DateTime? TicketAssignedDate { get; set; }
        public DateTime? TicketUpdatedDate { get; set; }
        public bool? DeleteStatus { get; set; }
        public short? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public bool? EscalationStatus { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool? FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool? ResolutionStatus { get; set; }
        public bool? EveryResponseStatus { get; set; }
    }
}
