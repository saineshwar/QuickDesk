using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class ViewSignatureRequestViewModel
    {
        public string Signature { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Enter ExistingPassword")]
        public string ExistingPassword { get; set; }

        [Required(ErrorMessage = "Enter NewPassword")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
