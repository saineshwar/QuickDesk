using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("AgentCheckInStatusDetails")]
    public class AgentCheckInStatusDetails
    {
        [Key]
        
        public long AgentStatusDetailsId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long UserId { get; set; }
    }
}
