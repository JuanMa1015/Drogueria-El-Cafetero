using El_Cafetero.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace El_Cafetero.Data
{
    public class El_CafeteroDbContext : IdentityDbContext<Users>
    {
        public El_CafeteroDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
