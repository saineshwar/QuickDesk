using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("KnowledgebaseAttachments")]
    public class KnowledgebaseAttachments
    {
        [Key]
        public long KnowledgebaseAttachmentsId { get; set; }
        [MaxLength(100)]
        public string AttachmentsName { get; set; }
        [MaxLength(100)]
        public string AttachmentsType { get; set; }
        public long KnowledgebaseId { get; set; }
    }

}
