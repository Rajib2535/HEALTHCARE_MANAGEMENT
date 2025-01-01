
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class TokenHandler : ITokenHandler
    {
        private string _token;
        private string _tokenType;
        private DateTime _expiration;

        public void SetToken(string token, string tokenType, int expireIn)
        {
            _token = token;
            _tokenType = tokenType;
            _expiration = DateTime.UtcNow.AddSeconds(expireIn);
        }

        public string GetToken() => _token;

        public string GetTokenType() => _tokenType;

        public DateTime GetTokenExpiration() => _expiration;

        public bool IsTokenExpired() => DateTime.UtcNow >= _expiration;
    }
}
