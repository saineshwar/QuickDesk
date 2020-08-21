using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface ISavedAssignedRoles
    {
        long? AddAssignedRoles(SavedAssignedRoles savedAssignedRoles);
        bool CheckAssignedRoles(long? userId);
        SavedAssignedRolesViewModel GetAssignedRolesbyUserId(long? userId);
    }
}