using System.Collections.Generic;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IAllTicketGrid
    {
        int AllAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch);

        List<ViewTicketModel> AllAdminTickets(string search, string searchin, int? prioritysearch,
            int? statussearch, int startIndex, int count, string sorting);

        int AllAgentsTicketsCount(long userId, string search, string searchin, int? prioritysearch,
            int? statussearch);

        List<ViewTicketModel> AllAgentsTickets(long userId, string search, string searchin,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);

        int AllAgentAdminTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch,
            int categoryId);

        List<ViewTicketModel> AllAgentAdminTicketsbyList(string search, string searchin, int categoryId,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);

        int AllHodTicketsCount(string search, string searchin, int? prioritysearch, int? statussearch,
            int categoryId);

        List<ViewTicketModel> AllHodTicketsbyList(string search, string searchin, int categoryId,
            int? prioritysearch, int? statussearch, int startIndex, int count, string sorting);

        int AllUserTicketsCount(long userId, string search, int? statusId, string searchin);

        List<ViewTicketModel> AllUserTickets(string search, string searchin, int? statusId, long userId, int startIndex,
            int count, string sorting);
    }
}