using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IUserMaster
    {
        Usermaster GetUserById(long? userId);
        long? AddUser(Usermaster usermaster, string password, string salt, short? roleId);
        long? UpdateUser(Usermaster usermaster);
        void DeleteUser(int? userId);
        bool CheckUsernameExists(string username);
        Usermaster GetUserByUsername(string username);
        List<SelectListItem> GetListofAdmin();
        List<UserResponseViewModel> GetAutoCompleteUserName(string username, int roleId);
        List<UserResponseViewModel> GetAllAgentsMembers(string username, long userId);
        List<UserResponseViewModel> GetAllUsersExceptSuperAdmin(string username);
        List<UsermasterViewModel> GetUserList(string username, int? roleid, int startIndex, int count, string sorting);
        EditUserViewModel EditUserbyUserId(long? userId);
        bool CheckUserIdExists(long userId);
        int UpdateUserDetails(EditUserViewModel editUserViewModel);
        EditAgentViewModel EditAgentbyUserId(long? userId);
        List<UsermasterViewModel> GetAgentList(string username, int? roleid, int startIndex, int count, string sorting);
        int GetUserCount(string username, int? roleid);
        int GetAgentCount(string username, int? roleid);
        long? AddAgent(Usermaster usermaster, string password, SavedAssignedRoles savedAssignedRoles, int? categoryId, string salt);
        int UpdateAgentDetails(EditAgentViewModel editAgentViewModel);
        long GetUserIdbyEmailId(string emailid);
        List<UserResponseViewModel> GetActiveUsersbyCategoryId(string username, string categoryId);
        List<SelectListItem> GetListofAgentsAdmin();
        List<SelectListItem> GetListofAgents(short categoryId);
        List<SelectListItem> GetListofHod();
        EscalatedUserViewModel GetTicketEscalatedToUserNames(long? ticketId);
        bool CheckEmailIdExists(string emailid);
        bool CheckMobileNoExists(string mobileno);
        List<UsermasterViewModel> GetOnlyUserList(string username, int startIndex, int count, string sorting);
        int GetOnlyUserCount(string username);
        long UpdateChangePasswordFlag(long userId, string newpassword);
        UserTokens GetUserSaltbyUserid(long userId);
        long UpdateIsFirstLogin(long? userId);
        bool CheckIsCategogryAssignedtoHod(long userId);
        bool CheckIsCategogryAssignedtoAgentAdmin(long userId);
    }
}