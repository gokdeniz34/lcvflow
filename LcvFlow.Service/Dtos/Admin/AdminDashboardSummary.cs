namespace LcvFlow.Service.Dtos.Admin;

public class AdminDashboardDto
{
    public int TotalGuests { get; set; }     // Toplam davetli sayısı
    public int AttendingCount { get; set; }   // Gelecekler (True)
    public int DeclinedCount { get; set; }    // Gelemeyecekler (False)
    public int PendingCount { get; set; }     // Henüz cevap vermeyenler (Null)
    public int TotalAdults { get; set; }      // Geleceklerin içindeki toplam yetişkin
    public int TotalChildren { get; set; }    // Geleceklerin içindeki toplam çocuk
}