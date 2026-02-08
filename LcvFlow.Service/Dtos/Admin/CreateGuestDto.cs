namespace LcvFlow.Service.Dtos.Admin;

public class CreateGuestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int EventId { get; set; }
}