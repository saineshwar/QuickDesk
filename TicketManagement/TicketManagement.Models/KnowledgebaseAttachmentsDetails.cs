using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("KnowledgebaseAttachmentsDetails")]
    public class KnowledgebaseAttachmentsDetails
    {
        [Key]
        public long KnowledgebaseAttachmentsDetailsId { get; set; }
        public byte[] AttachmentBytes { get; set; }
        public long KnowledgebaseId { get; set; }
        public long KnowledgebaseAttachmentsId { get; set; }
        
    }
}
