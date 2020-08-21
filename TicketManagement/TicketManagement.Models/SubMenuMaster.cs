using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("SubMenuMaster")]
    public class SubMenuMaster
    {
        [Key]
        public int SubMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string SubMenuName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int MenuId { get; set; }
        public long UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? RoleId { get; set; }
        public int? SortingOrder { get; set; }
    }
}
