using System.Text.Json.Serialization;

namespace MyDriver.Model.AuthManagment
{
    public class AuthModel
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }
        [JsonIgnore]
        public  string refreshToken { get; set; }
        public  DateTime refreshTokenExpiration { get; set; }
    }
}
