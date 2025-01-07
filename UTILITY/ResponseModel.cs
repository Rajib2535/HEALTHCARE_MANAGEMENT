namespace UTILITY
{
    public class ResponseModel
    {
        public int response_code { get; set; } = 500;
        public string response_body { get; set; } = string.Empty;
        public bool is_success { get; set; } = false;
    }
}
