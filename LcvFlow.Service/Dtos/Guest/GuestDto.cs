namespace LcvFlow.Service.Dtos.Guest;

public record GuestDto
{
    public string FullName { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public bool? IsAttending { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
}