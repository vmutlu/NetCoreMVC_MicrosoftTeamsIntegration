namespace NetCoreMVC_GraphAPI_Integration.Models
{
    public class Credentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public string Scopes { get; set; }
        public string TenantId { get; set; }
    }
}
