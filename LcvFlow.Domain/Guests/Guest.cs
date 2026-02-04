using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Guests;

public class Guest : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // LCV (RSVP) Mantığı
    public bool? IsAttending { get; set; } // null: Cevap vermedi, true: Geliyor, false: Gelmiyor
    public int AdultCount { get; set; } = 1; // Kaç yetişkin?
    public int ChildCount { get; set; } = 0; // Kaç çocuk?

    // Güvenlik ve Erişim
    public string AccessToken { get; set; } = Guid.NewGuid().ToString("N"); // Linkteki benzersiz kod (örn: ...?token=a1b2c3)

    // Ekstra Bilgiler
    public string? Note { get; set; } // Misafirin iletmek istediği not (Alerji, tebrik vb.)
    public string? TableNumber { get; set; } // Admin tarafından atanacak masa numarası
}