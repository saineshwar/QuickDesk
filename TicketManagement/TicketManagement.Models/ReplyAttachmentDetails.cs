using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("ReplyAttachmentDetails")]
    public class ReplyAttachmentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyAttachmentDetailsId { get; set; }
        public byte[] AttachmentBytes { get; set; }
        public long? TicketId { get; set; }
        public long ReplyAttachmentId { get; set; }
    }

}
