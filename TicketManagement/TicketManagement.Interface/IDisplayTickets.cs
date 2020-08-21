using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IDisplayTickets
    {
        DisplayTicketViewModel TicketsDetailsbyticketId(string ticketId);
        bool CheckTrackingIdExists(string trackingId);


    }
}