using VelesServer.Models;
using Microsoft.EntityFrameworkCore;

namespace VelesServer.Data
{
    public class VelesContext : DbContext
    {
        public VelesContext(DbContextOptions<VelesContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}