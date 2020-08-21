using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketHistory")]
    public class TicketHistory
    {
        [Key]
        public long TicketHistoryId { get; set; }
        public string Message { get; set; }
        public DateTime? ProcessDate { get; set; }
        public long? UserId { get; set; }
        public long? TicketId { get; set; }
        public short? StatusId { get; set; }
        public short? PriorityId { get; set; }
        public short? CategoryId { get; set; }
        public long? TicketReplyId { get; set; }
        public short? ActivitiesId { get; set; }
    }
}
