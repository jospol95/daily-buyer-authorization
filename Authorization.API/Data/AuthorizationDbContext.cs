using Authorization.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.Data
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        //base: access members of the base class from the derived class
        {
            
        }
        
        public DbSet<UserModel> Users { get; set; }
    }
}