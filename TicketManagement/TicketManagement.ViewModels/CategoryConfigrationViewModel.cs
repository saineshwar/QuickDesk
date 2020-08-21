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
    public class CategoryConfigrationViewModel
    {
        [DisplayName("Category Admin")]
        [Required(ErrorMessage = "Please Select Admin")]
        public int AgentAdminUserId { get; set; }
        public List<SelectListItem> ListofAdmin { get; set; }

        [DisplayName("HOD")]
        [Required(ErrorMessage = "Please Select HOD")]
        public int HodUserId { get; set; }
        public List<SelectListItem> ListofHod { get; set; }


        [DisplayName("BusinessHours")]
        [Required(ErrorMessage = "Please Select BusinessHours")]
        public short BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public short CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        public bool Status { get; set; }
    }

    public class EditCategoryConfigrationViewModel
    {
        public int CategoryConfigrationId { get; set; }

        [DisplayName("Category Admin")]
        [Required(ErrorMessage = "Please Select Admin")]
        public long UserId { get; set; }
        public List<SelectListItem> ListofAdmin { get; set; }

        [DisplayName("HOD")]
        [Required(ErrorMessage = "Please Select HOD")]
        public long HodUserId { get; set; }
        public List<SelectListItem> ListofHod { get; set; }

        [DisplayName("BusinessHours")]
        [Required(ErrorMessage = "Please Select BusinessHours")]
        public short BusinessHoursId { get; set; }
        public List<SelectListItem> ListofBusinessHours { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public short CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

        public bool Status { get; set; }
    }

    public class ShowCategoryConfigration
    {
        public int CategoryConfigrationId { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string HodName { get; set; }
    }
}
