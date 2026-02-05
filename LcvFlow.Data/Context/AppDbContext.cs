using Microsoft.EntityFrameworkCore;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain;

namespace LcvFlow.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Event> Events => Set<Event>();
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
}