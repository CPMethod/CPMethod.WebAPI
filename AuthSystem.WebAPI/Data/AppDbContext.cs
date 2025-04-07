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

            builder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);

            builder.Entity<Note>()
                .HasIndex(n => n.Title);

            builder.Entity<Note>()
                .HasIndex(n => n.UserId);
        }

        public DbSet<Note> Notes { get; set; }
    }
}
