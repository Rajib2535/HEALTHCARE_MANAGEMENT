
using UTILITY;

namespace DATA.Models.RequestReponseModels
{
    public class ExternalAPIRequestModel
    {
        public string request_body { get; set; } = string.Empty;
        public CommonEnum.APIScope api_scope { get; set; }
        public CommonEnum.ExternalAPIRequestType external_api_request_type { get; set; }
        public string url { get; set; }
        public Dictionary<string, string> url_encoded_body { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> request_headers { get; set; } = new Dictionary<string, string>();
        public string certificate_path { get; set; }
        public string certificate_password { get; set; }
        public bool is_get_request_body_available { get; set; } = false;
        public bool is_timeout { get; set; } = false;
        public int timeout_seconds { get; set; } = 0;
        public bool is_rcvc_true { get; set; } = false;
    }
}
