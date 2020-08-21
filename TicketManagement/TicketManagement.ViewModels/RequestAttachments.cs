using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class RequestAttachments
    {
        public long? TicketId { get; set; }
        public long AttachmentsId { get; set; }
    }

    public class RequestLockTicket
    {
        public long? TicketId { get; set; }
    }

    public class RequestEscalationTicket
    {
        public long? TicketId { get; set; }
    }
}
