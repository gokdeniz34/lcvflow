using Microsoft.EntityFrameworkCore;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain;
using LcvFlow.Domain.Common;

namespace LcvFlow.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<ImportLog> ImportLogs { get; set; }
    public DbSet<SystemError> SystemErrors { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }

    //burası oldukça temiz olmalı. Tüm entity configurationları ayrı sınıflarda yapacağız ve burada sadece onları uygulayacağız.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //her sorguda silinmiş kayıtları hariç tutmak için global query filter ekle
        modelBuilder.Entity<Guest>().HasQueryFilter(g => !g.IsDeleted);
        modelBuilder.Entity<AdminUser>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Event>().HasQueryFilter(a => !a.IsDeleted);

        // Tüm Configuration sınıflarını (IEntityTypeConfiguration) otomatik bulur ve uygular
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}