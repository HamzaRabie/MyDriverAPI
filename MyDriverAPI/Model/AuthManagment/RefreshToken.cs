using Microsoft.EntityFrameworkCore;

namespace MyDriver.Model.AuthManagment
{
    [Owned]
    public class RefreshToken
    {
        public  string Token { get; set; }
        public  DateTime ExpireOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpireOn;
        public  DateTime CreatedOn { get; set; }
        public  DateTime? RevokedON { get; set; }
        public bool IsActive => !IsExpired && RevokedON == null;
    }
}
