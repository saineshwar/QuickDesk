namespace TicketManagement.ViewModels
{
    public class SubMenuMasterOrderingVm
    {
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
    }

  
    public class RequestSubMenuMasterOrderVm
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }

    public class RequestMenu
    {
        public int RoleId { get; set; }
        public int MenuCategoryId { get; set; }
    }

    public class RequestSubMenu
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }

    public class SubMenuStoringOrder
    {
        public int SubMenuId { get; set; }
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }

    public class RequestMenuCategoryOrderVm
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
    }

}