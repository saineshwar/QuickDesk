using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
  
    [Table("BusinessHoursType")]
    public class BusinessHoursType
    {
        [Key]
        public short BusinessHoursTypeId { get; set; }
        public string BusinessHoursName { get; set; }
    }
}
