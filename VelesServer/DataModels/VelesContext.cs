using VelesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace VelesServer.DataModels
{
    public class VelesContext : DbContext
    {
        public VelesContext(DbContextOptions<VelesContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Users> User { get; set; } //to change name
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Messages> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}