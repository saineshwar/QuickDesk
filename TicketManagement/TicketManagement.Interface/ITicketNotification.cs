using System.Collections.Generic;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ITicketNotification
    {
        List<TicketNotificationViewModel> ListofNotification(long? agentId);
        int? GetNotificationCount(long? agentId);
        void UpdateTicketNotificationasRead(long? agentId, long? notificationId);
    }
}