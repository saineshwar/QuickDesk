using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class SubMenuMasterViewModel
    {
        public int SubMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string Areas { get; set; }
        public string SubMenuName { get; set; }
        public string MenuName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? MenuId { get; set; }
        public int? RoleId { get; set; }
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
    }
    public class RenderSubMenuVM
    {
        public int SubMenuID { get; set; }
        public string SubMenuName { get; set; }
        public string Areas { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
    }
}
