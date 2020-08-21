using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Knowledgebase")]
    public class Knowledgebase
    {
        [Key]
        public long KnowledgebaseId { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        public int? KnowledgebaseTypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool Status { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
    }

}
