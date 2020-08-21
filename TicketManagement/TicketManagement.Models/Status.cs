using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public short StatusId { get; set; }
        public string StatusText { get; set; }
        public bool IsInternalStatus { get; set; }
    }
}
