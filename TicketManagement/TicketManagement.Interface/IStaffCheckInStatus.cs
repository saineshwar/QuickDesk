using System.Collections.Generic;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IAgentCheckInStatus
    {
        void StatusCheckInCheckOut(long userId);
        bool CheckIsalreadyCheckedIn(long userId);
        List<AgentDailyActivityModel> AgentDailyActivity(long userId);
        bool CheckIsCategoryAssignedtoAgent(long userId);
    }
}