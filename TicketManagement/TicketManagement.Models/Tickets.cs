using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Tickets")]
    public class Tickets
    {
        [Key]
        public long TicketId { get; set; }
        [MaxLength(20)]
        public string TrackingId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public short? PriorityId { get; set; }
        public short? CategoryId { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public bool StatusAssigned { get; set; }
        public long? InternalUserId { get; set; }
    }

}
