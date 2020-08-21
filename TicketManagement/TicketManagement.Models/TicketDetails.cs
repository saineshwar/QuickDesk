using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketDetails")]
    public class TicketDetails
    {
        [Key]
        public long TicketDetailsId { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public long? UserId { get; set; }
        public long TicketId { get; set; }
    }

}
