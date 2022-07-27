using Microsoft.EntityFrameworkCore;
using VelesLibrary.DbModels;

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

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
