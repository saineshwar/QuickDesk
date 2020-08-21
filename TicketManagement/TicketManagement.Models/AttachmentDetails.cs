using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("AttachmentDetails")]
    public class AttachmentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AttachmentDetailsId { get; set; }
        [MaxLength]
        public byte[] AttachmentBytes { get; set; }
        public long? TicketId { get; set; }
        public long? UserId { get; set; }
        public long AttachmentId { get; set; }
    }

}
