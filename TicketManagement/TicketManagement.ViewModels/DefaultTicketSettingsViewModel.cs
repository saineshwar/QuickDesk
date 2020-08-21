using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ViewModels
{
    public class DefaultTicketSettingsViewModel
    {
        public int? DefaultTicketId { get; set; }

        [Required(ErrorMessage = "Enter TicketsCount")]
        [DisplayName("TicketsCount")]
        [RegularExpression("^[0-9]*$")]
        public int? TicketsCount { get; set; }

        [Required(ErrorMessage = "Enter Automatically close resolved tickets hours")]
        [DisplayName("Automatically close resolved tickets hours")]
        [RegularExpression("^[0-9]*$")]
        public int? AutoTicketsCloseHour { get; set; }

    }
}