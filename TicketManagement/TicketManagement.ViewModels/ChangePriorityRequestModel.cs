using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ChangePriorityRequestModel
    {
        [Required(ErrorMessage = "Required TicketId")]
        public long TicketId { get; set; }
        [Required(ErrorMessage = "Required PriorityId")]
        public short PriorityId { get; set; }
    }
}
