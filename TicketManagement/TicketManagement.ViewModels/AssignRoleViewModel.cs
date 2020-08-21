using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.ViewModels
{
    public class AssignRoleViewModel
    {
        [Required(ErrorMessage = "Choose Role")]
        [DisplayName("Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> RolesList { get; set; }
        public int[] SelectedMenuId { get; set; }
    }


    public class AssignRoleViewModelSubMenu
    {
        [DisplayName("Menu")]
        [Required(ErrorMessage = "Choose Menu")]

        public int MenuId { get; set; }
        public List<SelectListItem> Menulist { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Choose Role")]

        public int RoleId { get; set; }
        public List<SelectListItem> RolesList { get; set; }

        public int[] SelectedSubMenuId { get; set; }

    }
}
