namespace DATA.Models.ViewModels.UserPermissionViewModels
{
    public class RoleWiseMenuViewModel
    {
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? MenuURL { get; set; }
        public string? MenuIcon { get; set; }
        public string? ActionName { get; set; }
        public string? ControllerName { get; set; }
        public int? Priority { get; set; }
        public List<ChildMenuViewModel> ChildMenuViewModels { get; set; } = new();
        public class ChildMenuViewModel
        {
            public int MenuId { get; set; }
            public string? MenuURL { get; set; }
            public string? MenuName { get; set; }
            public string? MenuIcon { get; set; }
            public string? ActionName { get; set; }
            public string? ControllerName { get; set; }
            public int? Priority { get; set; }
        }

    }
    
}
