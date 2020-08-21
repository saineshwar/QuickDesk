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
    public class AssignViewUserRoleModel
    {
        [DisplayName("Role")]
        [Required(ErrorMessage = "Choose Role")]

        public short? RoleId { get; set; }

        [DisplayName("User")]
        [Required(ErrorMessage = "Choose Username")]
        public long? UserId { get; set; }

        [Required(ErrorMessage = "Choose Username")]
        public string Username { get; set; }

        public List<SelectListItem> ListRole { get; set; }
    }
}
