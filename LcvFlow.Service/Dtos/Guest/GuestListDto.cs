namespace LcvFlow.Service.Dtos.Guest;

public record GuestListDto
{
    public int Id { get; init; }
    public string FirstName { get; set; } = string.Empty; // FullName yerine FirstName lazım olabilir
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool? IsAttending { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public string? Note { get; set; }
}