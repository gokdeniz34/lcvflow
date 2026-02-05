using LcvFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LcvFlow.Data.Configurations;


public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(g => g.Id);

        // İsim alanları (Eğer FirstName/LastName ayırdıysan)
        builder.Property(g => g.FirstName)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(g => g.LastName)
            .IsRequired()
            .HasMaxLength(75);

        // Erişim Kodu (Token)
        // Uzunluğu 50 ideal, ama GUID "N" formatı 32 karakterdir. 
        builder.Property(g => g.AccessToken)
            .IsRequired()
            .HasMaxLength(50);

        // Performans ve Tekillik için Index Şart!
        builder.HasIndex(g => g.AccessToken)
            .IsUnique();

        // Varsayılan Değerler
        builder.Property(g => g.AdultCount)
            .HasDefaultValue(1);

        builder.Property(g => g.ChildCount)
            .HasDefaultValue(0);

        // Not alanı çok uzun olabilir, veritabanında kısıtlamayalım (TEXT/LONGTEXT)
        builder.Property(g => g.Note)
            .HasMaxLength(500);

        // BaseEntity'den gelen alanları da burada yönetebilirsin (isteğe bağlı)
        builder.Property(g => g.CreatedAt)
            .IsRequired();
    }
}