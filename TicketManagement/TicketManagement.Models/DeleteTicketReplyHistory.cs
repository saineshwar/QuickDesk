using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("DeleteTicketReplyHistory")]
    public class DeleteTicketReplyHistory
    {
        [Key]
        public long DeleteTicketReplyId { get; set; }
        public long TicketReplyId { get; set; }
        public long TicketId { get; set; }
        public long DeletedUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
