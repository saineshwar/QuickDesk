using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("SavedAssignedRoles")]
    public class SavedAssignedRoles
    {
        [Key]
        public int AssignedRoleId { get; set; }
        public long? UserId { get; set; }
        public short? RoleId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
    }
}
