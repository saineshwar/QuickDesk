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
    public class BusinessHoursModel
    {
        public IList<string> SelectedDays { get; set; }
        public IList<SelectListItem> ListofDays { get; set; }
        public string MorningHour { get; set; }
        public string EveningHour { get; set; }
        public IList<SelectListItem> ListofHour { get; set; }
        public string MorningPeriod { get; set; }
        public string EveningPeriod { get; set; }
        public IList<SelectListItem> ListofPeriod { get; set; }

        [Required(ErrorMessage = "Enter Selected BusinessHours Type")]
        public string SelectedBusinessHoursType { get; set; }
        public IList<SelectListItem> ListofBusinessHoursType { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }

        public string SelectWeek { get; set; }
        public IList<SelectListItem> Listofdd { get; set; }
    }
}
