using Microsoft.AspNetCore.Identity;

namespace El_Cafetero.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
