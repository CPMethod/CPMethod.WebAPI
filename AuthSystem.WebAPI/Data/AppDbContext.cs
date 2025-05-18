using AuthSystem.DataModel;
using AuthSystem.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Data
{
    public class AppDbContext : RefreshTokenDbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
