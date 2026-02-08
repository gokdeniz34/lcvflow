using LcvFlow.Domain.Common;
using LcvFlow.Domain.Enums;

namespace LcvFlow.Domain.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public EventType Type { get; set; }

    // Adminin seçtiği form konfigürasyonu (JSON)
    // Örn: {"AskAdultCount": true, "AskChildCount": false, "CustomQuestions": []}
    public string FormConfigJson { get; set; } = "[]";

    // Excel Şablonundaki "Yönerge Sayfası" için özel notlar
    // Admin buraya "Lütfen sadece 18 yaş üstü misafirleri ekleyin" gibi notlar düşebilir.
    public string? InstructionSheetNote { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    private Event() { }

    // Constructor'ı daha net hale getirdik: Tip artık zorunlu bir parametre gibi düşünülebilir.
    public Event(string name, DateTime eventDate, string location, string slug, EventType type)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        EventDate = eventDate;
        Location = location ?? throw new ArgumentNullException(nameof(location));
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
        Type = type;
        IsActive = true;
    }

    public string GenerateSlug(string name)
    {
        return name.ToLower().Replace(" ", "-").Replace("&", "ve"); // Basit bir örnek
    }
}