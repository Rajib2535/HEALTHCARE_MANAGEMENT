using Newtonsoft.Json;

namespace WEB_APP.Models
{
    public class ToastMessage
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        public ToastType ToastType { get; set; }
        public bool IsSticky { get; set; }
    }
}
