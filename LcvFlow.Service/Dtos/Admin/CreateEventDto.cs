namespace LcvFlow.Service.Dtos.Admin;

public class CreateEventDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime EventDate { get; set; } = DateTime.Now.AddDays(30);
    public string Location { get; set; } = string.Empty;
}