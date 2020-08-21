using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Priority")]
    public class Priority
    {
        [Key]
        public short PriorityId { get; set; }
        public string PriorityName { get; set; }
    }
}
