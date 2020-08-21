using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class SlaPoliciesShowViewModel
    {
        public short? SlaPoliciesId { get; set; }
        public string PriorityName { get; set; }
        public short? FirstResponseDay { get; set; }
        public short? FirstResponseHour { get; set; }
        public short? FirstResponseMins { get; set; }
        public short? EveryResponseDay { get; set; }
        public short? EveryResponseHour { get; set; }
        public short? EveryResponseMins { get; set; }
        public short? ResolutionResponseDay { get; set; }
        public short? ResolutionResponseHour { get; set; }
        public short? ResolutionResponseMins { get; set; }
        public short? EscalationDay { get; set; }
        public short? EscalationHour { get; set; }
        public short? EscalationMins { get; set; }
    }
}
