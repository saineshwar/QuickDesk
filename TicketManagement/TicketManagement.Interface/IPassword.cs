using TicketManagement.Models;

namespace TicketManagement.Interface
{
    public interface IPassword
    {
        string GetPasswordbyUserId(long userId);

        bool UpdatePasswordandHistory(long userId, string passwordHash, string passwordSalt, string processType);
    }
}