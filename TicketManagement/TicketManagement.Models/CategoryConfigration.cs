using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("CategoryConfigration")]
    public class CategoryConfigration
    {
        [Key]
        public int CategoryConfigrationId { get; set; }
        public short CategoryId { get; set; }
        public long AgentAdminUserId { get; set; }
        public short BusinessHoursId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long HodUserId { get; set; }
        
    }
}
