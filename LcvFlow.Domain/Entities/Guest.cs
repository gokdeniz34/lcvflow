using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Entities;

public class Guest : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public bool? IsAttending { get; set; }
    public int AdultCount { get; set; } = 1;
    public int ChildCount { get; set; } = 0;

    // Güvenlik ve Erişim
    public string AccessToken { get; set; } = Guid.NewGuid().ToString("N");

    // Ekstra Bilgiler
    public string? Note { get; set; } // Misafirin iletmek istediği not (Alerji, tebrik vb.)
    public string? TableNumber { get; set; } // Admin tarafından atanacak masa numarası

    public int EventId { get; set; }
    public virtual Event Event { get; set; } = null!;
}