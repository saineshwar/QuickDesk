using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{

    public class AddCategoriesVM
    {

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter CategoryName")]
        public string MenuCategoryName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

        [DisplayName("IsActive")]
        [Required(ErrorMessage = "Required IsActive")]
        public bool Status { get; set; }

        public List<SelectListItem> ListofRoles { get; set; }
    }

    public class EditCategoriesVM
    {
        public int MenuCategoryId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter CategoryName")]
        public string MenuCategoryName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

        [DisplayName("IsActive")]
        [Required(ErrorMessage = "Required IsActive")]
        public bool Status { get; set; }

        public List<SelectListItem> ListofRoles { get; set; }
    }

    public class RequestDeleteCategory
    {
        public int MenuCategoryId { get; set; }
    }

    public class RequestCategory
    {
        public int? RoleID { get; set; }
    }
}