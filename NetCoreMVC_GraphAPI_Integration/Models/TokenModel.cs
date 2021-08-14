namespace NetCoreMVC_GraphAPI_Integration.Models
{
    public class TokenModel
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
