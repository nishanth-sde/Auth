namespace JWT.Auth.Configurations
{
    public class AuthConfig
    {
        public string Issuer  { get; set; } 
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int ExpirationInMin { get; set; }
    }
}
