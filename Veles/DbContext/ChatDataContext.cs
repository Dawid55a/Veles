using Microsoft.EntityFrameworkCore;
using VelesAPI.DbModels;

namespace VelesAPI.DbContext
{
    public class ChatDataContext : Microsoft.EntityFrameworkCore.DbContext 
    {
        public ChatDataContext(DbContextOptions<ChatDataContext> options):
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
    }
}
