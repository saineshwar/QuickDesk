using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class SlaPoliciesViewModel
    {
        [DisplayName("Priority")]
        [Required(ErrorMessage = "Select Priority.")]
        public short? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? FirstResponseDay { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? FirstResponseHour { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? FirstResponseMins { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EveryResponseDay { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EveryResponseHour { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EveryResponseMins { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? ResolutionResponseDay { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? ResolutionResponseHour { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? ResolutionResponseMins { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EscalationDay { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EscalationHour { get; set; }

        [RegularExpression("^([1-9][0-9]{0,1})$", ErrorMessage = "Enter Number Between 1 to 99")]
        public short? EscalationMins { get; set; }
    }
}