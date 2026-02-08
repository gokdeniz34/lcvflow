namespace LcvFlow.Domain.Entities;

public class ImportLog
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int TotalRows { get; set; }
    public long ProcessingTimeMs { get; set; } // Stopwatch ile ölçeceğimiz süre
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}