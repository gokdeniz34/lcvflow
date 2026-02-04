using Microsoft.EntityFrameworkCore;
using LcvFlow.Domain.Guests;
using System.Reflection;

namespace LcvFlow.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Guest> Guests => Set<Guest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tüm Configuration sınıflarını (IEntityTypeConfiguration) otomatik bulur ve uygular
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}