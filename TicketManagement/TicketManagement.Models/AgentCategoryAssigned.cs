using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("AgentCategoryAssigned")]
    public class AgentCategoryAssigned
    {
        [Key]
        public short AgentCategoryId { get; set; }
        public short CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
