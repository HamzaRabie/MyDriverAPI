using Microsoft.AspNetCore.Identity;
using MyDriver.Model.AuthManagment;

namespace MyDriver.Model
{
    public class ApplicationUser : IdentityUser
    {
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public  List<RefreshToken>? refreshtokens { get; set; }
    }
}
