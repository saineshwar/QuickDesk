using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models
{
    [Table("HolidayList")]
    public class HolidayList
    {
        [Key]
        public short HolidayId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}