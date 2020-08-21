using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ChangeCategoryRequestModel
    {
        [Required(ErrorMessage = "Required TicketId")]
        public long TicketId { get; set; }
        [Required(ErrorMessage = "Required CategoryId")]
        public short CategoryId { get; set; }
    }
}
