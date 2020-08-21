using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketManagement.Concrete;

namespace TicketManagement.CommonData
{
    public static class AgentCheckInStatusCommon
    {
        public static bool CheckIsalreadyCheckedIn(long userId)
        {
            using (var context = new DatabaseContext())
            {
                var checkStatus = (from a in context.AgentCheckInStatusSummary
                                   where a.UserId == userId
                                   select a.AgentStatus).FirstOrDefault();

                return checkStatus;
            }
        }

        public static bool CheckIsCategoryAssignedtoAgent(long userId)
        {
            using (var context = new DatabaseContext())
            {
                var result = (from agentCategory in context.AgentCategoryAssigned
                              where agentCategory.UserId == userId
                              select agentCategory).Any();

                return result;
            }
        }
    }
}