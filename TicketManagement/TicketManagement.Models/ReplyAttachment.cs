using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("ReplyAttachment")]
    public class ReplyAttachment
    {
        [Key]
        public long ReplyAttachmentId { get; set; }
        [MaxLength(50)]
        public string AttachmentName { get; set; }
        [MaxLength(50)]
        public string AttachmentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? TicketUser { get; set; }
        public long? SystemUser { get; set; }
        public long? TicketReplyId { get; set; }
        public long? TicketId { get; set; }
    }

}
