using Microsoft.EntityFrameworkCore;
using SYSTEMATIC.DB.Entities;

namespace SYSTEMATIC.DB
{
    public class SystematicDbContext : DbContext
    {
        private readonly string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=SystematicDb;Trusted_Connection=True;";
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            _ = modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
