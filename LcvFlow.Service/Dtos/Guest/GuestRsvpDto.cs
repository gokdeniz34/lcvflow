namespace LcvFlow.Service.Dtos.Guest;

public class GuestRsvpDto
{
    public string AccessToken { get; set; } = string.Empty;
    public bool IsAttending { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public string? Note { get; set; }
}