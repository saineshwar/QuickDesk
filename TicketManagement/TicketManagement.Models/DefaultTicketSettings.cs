using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models
{
    [Table("DefaultTicketSettings")]
    public class DefaultTicketSettings
    {
        [Key]
        public int? DefaultTicketId { get; set; }
        public int? TicketsCount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UserId { get; set; }
        public int? AutoTicketsCloseHour { get; set; }
        
    }
}