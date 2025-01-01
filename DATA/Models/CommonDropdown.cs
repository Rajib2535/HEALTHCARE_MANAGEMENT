namespace DATA.Models
{
    public class CommonDropdown
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string Identifier { get; set; }
        public bool? Active { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
