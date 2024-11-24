using IMemeU.Models;
using Microsoft.EntityFrameworkCore;

namespace IMemeU.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<MessageViewModel> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
    }
}