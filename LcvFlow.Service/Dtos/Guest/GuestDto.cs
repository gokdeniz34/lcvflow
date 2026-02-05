namespace LcvFlow.Service;

public class GuestDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsAttending { get; set; }
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string? TableNumber { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}