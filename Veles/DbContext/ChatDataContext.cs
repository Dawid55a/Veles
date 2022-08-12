using Microsoft.EntityFrameworkCore;
using VelesLibrary.DbModels;

namespace VelesAPI.DbContext;

public class ChatDataContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ChatDataContext(DbContextOptions<ChatDataContext> options) :
        base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGroup>()
            .HasKey(ug => new {ug.UserId, ug.GroupId});
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);
        modelBuilder.Entity<Group>()
            .HasMany(g => g.UserGroups)
            .WithOne(ug => ug.Group)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.UseIdentityColumns();
    }
}
