using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketStatus")]
    public class TicketStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TicketStatusId { get; set; }
        public long? TicketId { get; set; }
        public long? UserId { get; set; }
        public DateTime? TicketAssignedDate { get; set; }
        public DateTime? TicketUpdatedDate { get; set; }
        public short? StatusId { get; set; }
        public short? PriorityId { get; set; }
        public bool DeleteStatus { get; set; }
        public bool IsActive { get; set; }
        //Used While restoring Ticket State
        public short? PerviousStatusId { get; set; }
        public short? CategoryId { get; set; }
        public bool EscalationStatus { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool ResolutionStatus { get; set; }
        public bool EveryResponseStatus { get; set; }
        public short? EscalationStage { get; set; }
    }

}
