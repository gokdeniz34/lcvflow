using LcvFlow.Domain.Common;

namespace LcvFlow.Domain.Entities;

public class Guest : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public bool? IsAttending { get; private set; }
    public int AdultCount { get; private set; }
    public int ChildCount { get; private set; }
    public string AccessToken { get; private set; } = string.Empty;
    public string? Note { get; private set; }
    public int EventId { get; set; }
    public virtual Event Event { get; set; } = null!;
    private Guest() { }

    public Guest(string firstName, string lastName, int eventId)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("İsim boş olamaz.");

        FirstName = firstName;
        LastName = lastName;
        EventId = eventId;
        AccessToken = Guid.NewGuid().ToString("N"); // Token burada üretilir, kimse unutamaz!
        CreatedAt = DateTime.UtcNow;
    }
    public Result SubmitRsvp(bool isAttending, int adultCount, int childCount, string? note)
    {
        if (IsAttending.HasValue)
            return Result.Failure("Bu davetiye için zaten cevap verilmiş.");

        if (isAttending && adultCount <= 0)
            return Result.Failure("Katılıyorsanız yetişkin sayısı 0 olamaz.");

        IsAttending = isAttending;
        AdultCount = isAttending ? adultCount : 0;
        ChildCount = isAttending ? childCount : 0;
        Note = note;

        return Result.Success();
    }
}