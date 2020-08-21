using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IRole
    {
        IEnumerable<RoleMaster> GetAllRoleMaster();
        RoleMaster GetRoleMasterById(int? roleId);
        int? AddRoleMaster(RoleMaster roleMaster);
        int? UpdateRoleMaster(RoleMaster roleMaster);
        void DeleteRoleMaster(int? roleId);
        bool CheckRoleMasterNameExists(string roleName);
        IQueryable<RoleMaster> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetAllActiveRoles();
        int? UpdateRoleStatus(ViewMenuRoleStatusUpdateModel vmrolemodel);
        int? UpdateSubMenuRoleStatus(ViewSubMenuRoleStatusUpdateModel vmrolemodel);
        List<SelectListItem> GetAllActiveRolesNotAgent();
        List<SelectListItem> GetAllActiveRolesAgent();
    }
}
