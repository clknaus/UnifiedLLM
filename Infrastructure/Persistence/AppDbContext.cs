using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public DbSet<Anonymizer> AnonymizedChatRequest { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional configuration or seed data can be added here
    }
}
