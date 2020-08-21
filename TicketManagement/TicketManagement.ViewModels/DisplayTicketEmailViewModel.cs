using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class DisplayTicketEmailViewModel
    {
        public long TicketId { get; set; }
        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [DisplayName("Priority")]
        public string PriorityName { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public string Contact { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public bool StatusAssigned { get; set; }
        public long TicketDetailsId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        [DisplayName("Change Priority")]
        public int? PriorityId { get; set; }
    }
}
