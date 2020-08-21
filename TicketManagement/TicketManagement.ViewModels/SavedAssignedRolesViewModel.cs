using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class SavedAssignedRolesViewModel
    {
        public long? UserId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
    }
}
