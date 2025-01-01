namespace SERVICE.Interface
{
    public interface ITokenHandler
    {
        public void SetToken(string token, string tokenType, int expireIn);
        public string GetToken();
        public string GetTokenType();
        public DateTime GetTokenExpiration();
        public bool IsTokenExpired();
    }
}
