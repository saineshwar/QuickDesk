using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class TicketHistoryRequest
    {
        public string Ticketid { get; set; }
    }

    public class TicketHistoryResponse
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string Activities { get; set; }
        public string StatusText { get; set; }
        public string PriorityName { get; set; }
        public string CategoryName { get; set; }
        public string ProcessDate { get; set; }
        public string Viewcolor { get; set; }
        public string Nameinitial { get; set; }
    }
  
}
