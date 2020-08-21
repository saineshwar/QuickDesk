using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ChangeStatusRequestModel
    {

        [Required(ErrorMessage = "Required TicketId")]
        public long TicketId { get; set; }
        [Required(ErrorMessage = "Required StatusId")]
        public short StatusId { get; set; }
        public bool FirstResponseStatus { get; set; }
        public bool ResolutionStatus { get; set; }
        public bool EscalationStatus { get; set; }
        public bool EveryResponseStatus { get; set; }
        public int PriorityId { get; set; }
    }
}
