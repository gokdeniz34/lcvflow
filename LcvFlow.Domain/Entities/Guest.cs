using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Entities;

public class Guest : BaseEntity
{
    public int EventId { get; set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public bool? IsAttending { get; private set; }
    public int AdultCount { get; private set; }
    public int ChildCount { get; private set; }
    public string? Note { get; private set; }
    public string AccessToken { get; private set; } = string.Empty;
    public virtual Event Event { get; set; } = null!;

    // Veritabanında saklanan ham JSON
    public string AdditionalDataJson { get; private set; } = "{}";

    // Veritabanına yazılmaz, kod içinde işlem yapmamızı sağlar
    [NotMapped]
    public Dictionary<string, string> AdditionalProperties { get; private set; } = new();

    private Guest() { }

    public Guest(int eventId, string firstName, string lastName, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        EventId = eventId;
        PhoneNumber = phone;
        AccessToken = Guid.NewGuid().ToString("N");
    }

    // JSON'u Dictionary'ye çevirir (Veritabanından okurken çağrılır)
    public void LoadAdditionalProperties()
    {
        if (!string.IsNullOrEmpty(AdditionalDataJson))
        {
            AdditionalProperties = JsonSerializer.Deserialize<Dictionary<string, string>>(AdditionalDataJson) ?? new();
        }
    }

    // Dictionary'yi JSON'a çevirir (Veritabanına yazmadan önce çağrılır)
    public void UpdateAdditionalDataJson()
    {
        AdditionalDataJson = JsonSerializer.Serialize(AdditionalProperties);
    }

    public Result SubmitRsvp(bool isAttending, int adultCount, int childCount, string? note, Dictionary<string, string>? dynamicFields = null)
    {
        IsAttending = isAttending;
        AdultCount = isAttending ? adultCount : 0;
        ChildCount = isAttending ? childCount : 0;
        Note = note;

        if (dynamicFields != null)
        {
            foreach (var field in dynamicFields)
            {
                AdditionalProperties[field.Key] = field.Value;
            }
            UpdateAdditionalDataJson();
        }

        return Result.Success();
    }

    public void SetImportedAdditionalData(Dictionary<string, string> data)
    {
        if (data == null) return;

        AdditionalProperties = data;
        UpdateAdditionalDataJson();
    }

    //seed işlemi için
    public void UpdateRsvpStatus(bool? isAttending)
    {
        // Burada ileride "Etkinlik tarihi geçtiyse değiştirilemez" gibi kurallar eklenebilir.
        IsAttending = isAttending;
    }
}