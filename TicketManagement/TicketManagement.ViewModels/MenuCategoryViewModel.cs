using System;

namespace TicketManagement.ViewModels
{
    public class MenuCategoryViewModel
    {
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public int MenuCategoryId { get; set; }
    }

    public class MenuCategoryCacheViewModel
    {
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
        public int RoleId { get; set; }

    }

    public class RenderCategoriesVM
    {
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
    }
}