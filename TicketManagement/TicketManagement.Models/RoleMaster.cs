using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("RoleMaster")]
    public class RoleMaster
    {
        [Key]
        public short RoleId { get; set; }
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public long UserId { get; set; }

    }
}
