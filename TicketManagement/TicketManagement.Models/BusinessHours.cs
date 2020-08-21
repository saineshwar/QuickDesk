using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("BusinessHours")]
    public class BusinessHours
    {
        [Key]
        public short BusinessHoursId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HelpdeskHoursType { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
