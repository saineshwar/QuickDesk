using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("BusinessHoursDetails")]
    public class BusinessHoursDetails
    {
        [Key]
        public int BusinessHoursDetailsId { get; set; }
        public string Day { get; set; }
        public string MorningTime { get; set; }
        public string MorningPeriods { get; set; }
        public string EveningTime { get; set; }
        public string EveningPeriods { get; set; }
        public DateTime? CreateDate { get; set; }
        public long UserId { get; set; }
        public int BusinessHoursId { get; set; }
        public string StartTime { get; set; }
        public string CloseTime { get; set; }

    }
}
