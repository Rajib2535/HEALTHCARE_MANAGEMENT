namespace DATA.Models.ViewModels.UserPermissionViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public int? ParentMenu { get; set; }
        public bool? IsActive { get; set; }
        public string? MenuName { get; set; }
        public int? MenuOrder { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ParentMenuName { get; set; }
    }
}
