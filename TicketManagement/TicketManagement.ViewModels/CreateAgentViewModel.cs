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
    public class CreateAgentViewModel
    {
        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }
        public bool? Status { get; set; }

        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in characters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public int? CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

    }

    public class EditAgentViewModel
    {
        public long UserId { get; set; }

        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }
        public bool Status { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Category.")]
        public short CategoryId { get; set; }
        public List<SelectListItem> ListofCategory { get; set; }

    }
}
