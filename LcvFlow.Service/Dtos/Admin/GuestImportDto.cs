namespace LcvFlow.Service.Dtos.Admin;

public class GuestImportDto
{
    // Sabit Alanlar
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    // Dinamik Alanlar (Excel'deki o bilinmeyen kolonlar buraya dolacak)
    public Dictionary<string, string> DynamicFields { get; set; } = new();
}