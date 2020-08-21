using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IDashboardTicketCount
    {
        DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAgent(long userId);
        DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAdmin();
        DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforAgentAdmin(string categoryId);
        DashboardCountViewModel GetAllTicketbyStatusofCurrentDayforUser(long userId);
    }
}
