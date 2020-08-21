using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ViewModels
{
    public class HolidayViewModel
    {
        [Required(ErrorMessage = "HolidayDate")]
        public string HolidayDate { get; set; }
        [Required(ErrorMessage = "HolidayName")]
        public string HolidayName { get; set; }
    }
}