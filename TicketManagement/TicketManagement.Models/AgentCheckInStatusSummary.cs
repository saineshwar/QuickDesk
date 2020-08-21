using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("AgentCheckInStatusSummary")]
    public class AgentCheckInStatusSummary
    {
        [Key]
        public int AgentStatusId { get; set; }
        public bool AgentStatus { get; set; }
        public long UserId { get; set; }      
    }
}
