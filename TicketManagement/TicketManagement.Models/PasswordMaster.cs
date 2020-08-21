using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("PasswordMaster")]
    public class PasswordMaster
    {
        [Key]
        public long PasswordId { get; set; }
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UserId { get; set; }
    }
}
