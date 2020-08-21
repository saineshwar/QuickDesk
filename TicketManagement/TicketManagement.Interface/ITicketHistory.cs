using System.Collections.Generic;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ITicketHistory
    {
        void TicketHistory(TicketHistory ticketHistory);
        List<TicketHistoryResponse> ListofTicketHistorybyTicket(long? ticketId);
    }
}