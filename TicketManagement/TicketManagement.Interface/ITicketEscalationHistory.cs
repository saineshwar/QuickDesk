using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ITicketEscalationHistory
    {
        bool CheckIsTicketAlreadyEscalate(RequestEscalationTicket request);
    }
}