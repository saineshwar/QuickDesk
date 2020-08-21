using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    public class MenuCategoryOrderingVm
    {
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
    }

    public class MenuCategoryStoringOrder
    {
        public int MenuCategoryId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }

    public class MenuMasterOrderingVm
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }

    public class RequestMenuMasterOrderVm
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
    }

    public class MenuStoringOrder
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }
}
