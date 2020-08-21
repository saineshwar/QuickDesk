using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IDefaultTicketSettings
    {
        int? AddDefaultTicketCount(DefaultTicketSettings defaultTicket);
        DefaultTicketSettings GetDefaultTicketCount();
    }
}