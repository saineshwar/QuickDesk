using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("TicketDeleteLockStatus")]
    public class TicketDeleteLockStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DeleteLockId { get; set; }
        public bool TicketDeleteStatus { get; set; }
        public bool TicketLockStatus { get; set; }
        public long TicketId { get; set; }

    }
}
