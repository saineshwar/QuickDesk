using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class BusinessHoursViewModel
    {
        public int BusinessHoursId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessHoursName { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
