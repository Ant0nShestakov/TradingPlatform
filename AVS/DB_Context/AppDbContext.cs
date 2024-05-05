using AVS.Models;
using Microsoft.EntityFrameworkCore;

namespace AVS.DB_Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(user => user.Id);

            modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        }
    }
}
