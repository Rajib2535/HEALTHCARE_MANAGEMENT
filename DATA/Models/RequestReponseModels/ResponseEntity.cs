namespace DATA.Models.RequestReponseModels
{
    public class ResponseEntity
    {
        public bool is_valid { get; set; } = false;
        public bool session_expired { get; set; } = false;
        public string redirect_url { get; set; }
        public string html { get; set; }
        public string html2 { get; set; }
        public List<string> error_messages { get; set; } = new List<string>();
        public object data { get; set; } = new object();
    }

    public class PaginatedResponseEntity<T>
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<T> Data { get; set; }
    }
}
