using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketReply")]
    public class TicketReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? TicketReplyId { get; set; }
        public long? UserId { get; set; }
        public long? TicketId { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(20)]
        public string CreatedDateDisplay { get; set; }
        public bool DeleteStatus { get; set; }

    }

}
