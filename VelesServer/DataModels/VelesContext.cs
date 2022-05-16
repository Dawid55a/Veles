using VelesServer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace VelesServer.DataModels
{
    public class VelesContext : DbContext
    {
        public VelesContext(DbContextOptions<VelesContext> options) : base(options)
        {
        }

        
        public DbSet<User> Users { get; set; } //to change name
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Group>().ToTable("Group");
        }
    }
}