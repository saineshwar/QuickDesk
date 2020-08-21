using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ViewModels
{
    public class ChangePasswordFirstLoginModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        [Required(ErrorMessage = "Enter Current Password")]
        public string CurrentPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm new password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Required(ErrorMessage = "Enter New Password")]
        public string NewPassword { get; set; }

    }
}