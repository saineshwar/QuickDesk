using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.Models
{
    [Table("MenuMaster")]
    public class MenuMaster
    {
        [Key]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Enter MenuName")]
        public string MenuName { get; set; }
        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? RoleId { get; set; }
        public int? SortingOrder { get; set; }
    }
}
