using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class MenuMasterViewModel
    {
        [Required(ErrorMessage = "Enter Controller Name")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "Enter Action Method")]
        public string ActionMethod { get; set; }
        [Required(ErrorMessage = "Enter MenuName")]
        public string MenuName { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
        public List<SelectListItem> AllActiveRoles { get; set; }
        public int MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class EditMenuMasterViewModel
    {
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }
        [Required(ErrorMessage = "Enter MenuName")]
        public string MenuName { get; set; }
        public bool Status { get; set; }
        public int? MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
        public int? RoleId { get; set; }
        public List<SelectListItem> AllActiveRoles { get; set; }
    }

    public class MenuViewModel
    {
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ControllerName { get; set; }
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }
        [Required(ErrorMessage = "Enter MenuName")]
        public string MenuName { get; set; }
        public bool Status { get; set; }
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
    }

    public class MenuMasterCacheViewModel
    {
        public int MenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string Areas { get; set; }
        public string MenuName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsCache { get; set; }
        public int UserId { get; set; }
        public int? RoleId { get; set; }

    }

    public class RenderMenuVM
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string Areas { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
    }
}
