using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models
{
    [Table("MenuCategory")]
    public class MenuCategory
    {
        [Key]
        public int MenuCategoryId { get; set; }

        [Required(ErrorMessage = "Enter CategoryName")]
        public string MenuCategoryName { get; set; }

        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Required IsActive")]
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public int? SortingOrder { get; set; }

    }
}