namespace LcvFlow.Service;

public class DashboardStatsDto
{
    public int TotalGuests { get; set; }
    public int AcceptedCount { get; set; }
    public int DeclinedCount { get; set; }
    public int PendingCount { get; set; }
    public int TotalAdults { get; set; }
    public int TotalChildren { get; set; }
}