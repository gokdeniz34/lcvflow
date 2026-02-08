using LcvFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LcvFlow.Data.Configurations;


public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(x => x.Id);

        // JSON kolonunu MySQL için optimize edelim
        builder.Property(x => x.AdditionalDataJson)
               .HasColumnType("json") // MySQL 5.7+ kullanıyorsan 'json' mükemmel olur
               .IsRequired();

        // AccessToken üzerinde index olsun, hızlı arama için şart
        builder.HasIndex(x => x.AccessToken).IsUnique();
    }
}