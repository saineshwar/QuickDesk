using System.Collections.Generic;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ISlaPolicies
    {
        int? AddSlaPolicies(SlaPolicies slaPolicies);
        bool CheckPoliciesExists(int? priorityId);
        int GetSlaPoliciesCount(string priorityName);
        List<SlaPoliciesShowViewModel> GetSlaPoliciesList(string priorityName, int startIndex, int count,
            string sorting);
        int DeleteSlaPolicies(int? slaPoliciesId);
    }
}