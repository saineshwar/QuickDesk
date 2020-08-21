using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("SavedSubMenuRoles")]
    public class SavedSubMenuRoles
    {
        [Key]
        public int SavedSubMenuRoleId { get; set; }
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int SubMenuId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public int? MenuOrder { get; set; }
    }
}
