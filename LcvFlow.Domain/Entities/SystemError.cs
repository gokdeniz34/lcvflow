namespace LcvFlow.Domain.Entities;

public class SystemError
{
    public int Id { get; set; }
    public string? Source { get; set; } // Hangi metoddan geldi?
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}