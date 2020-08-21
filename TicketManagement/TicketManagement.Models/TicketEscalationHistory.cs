using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketEscalationHistory")]
    public class TicketEscalationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TicketEscalationId { get; set; }
        public long? TicketId { get; set; }
        public DateTime? EscalationDate1 { get; set; }
        public DateTime? EscalationDate2 { get; set; }
        public short? EscalationStage1UserId { get; set; }
        public short? EscalationStage2UserId { get; set; }
    }

}
