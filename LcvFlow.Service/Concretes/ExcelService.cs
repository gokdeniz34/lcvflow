using LcvFlow.Domain.Entities;
using LcvFlow.Service.Dtos.Admin;
using LcvFlow.Service.Dtos.Guest;
using LcvFlow.Service.Interfaces;
using OfficeOpenXml;
using System.Drawing;
using System.Text.Json;

namespace LcvFlow.Service.Concretes;

public class ExcelService : IExcelService
{
    public async Task<byte[]> GenerateTemplateWithInstructionsAsync(Event ev)
    {
        using var package = new ExcelPackage();

        // 1. Sayfa: Yönergeler (Instruction Sheet)
        var instrSheet = package.Workbook.Worksheets.Add("Nasil Doldurulur");
        instrSheet.Cells["A1"].Value = $"{ev.Name} Listesi Hazırlama Rehberi";
        instrSheet.Cells["A1"].Style.Font.Bold = true;
        instrSheet.Cells["A1"].Style.Font.Size = 14;

        instrSheet.Cells["A3"].Value = "ÖNEMLİ NOT:";
        instrSheet.Cells["B3"].Value = ev.InstructionSheetNote ?? "Lütfen kolon başlıklarını değiştirmeden verileri giriniz.";
        instrSheet.Cells["B3"].Style.Font.Italic = true;

        instrSheet.Cells["A5"].Value = "Kolon Rehberi:";
        instrSheet.Cells["A6"].Value = "- Mavi Kolonlar: Temel bilgiler (Ad, Soyad, Telefon).";
        instrSheet.Cells["A7"].Value = "- Yeşil Kolonlar: Sizin tanımladığınız özel sorular.";
        instrSheet.Cells["A8"].Value = "- Evet/Hayır tipi sorulara sadece 'Evet' veya 'Hayır' yazınız.";

        instrSheet.Cells.AutoFitColumns();

        // 2. Sayfa: Veri Girişi (Data Sheet)
        var dataSheet = package.Workbook.Worksheets.Add("Davetli Listesi");

        // Sabit başlıklar
        var headers = new List<string> { "Ad", "Soyad", "Telefon" };

        // Event içindeki dinamik soruları al
        var customFields = JsonSerializer.Deserialize<List<FormFieldDefinitionDto>>(ev.FormConfigJson) ?? new();
        headers.AddRange(customFields.Select(f => f.Label));

        // Başlıkları Excel'e yaz ve boya
        for (int i = 0; i < headers.Count; i++)
        {
            var cell = dataSheet.Cells[1, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);

            // İlk 3 kolon koyu mavi, diğerleri koyu yeşil
            var bgColor = (i < 3) ? Color.FromArgb(44, 62, 80) : Color.FromArgb(39, 174, 96);
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(bgColor);
        }

        // Bir örnek satır ekleyelim (Kullanıcıya kopya olsun)
        dataSheet.Cells[2, 1].Value = "Örnek Ad";
        dataSheet.Cells[2, 2].Value = "Örnek Soyad";
        dataSheet.Cells[2, 3].Value = "05550000000";

        // Boolean alanlar için Excel'e Dropdown (Validation) ekleyebiliriz (Senior dokunuşu!)
        for (int i = 0; i < customFields.Count; i++)
        {
            if (customFields[i].FieldType == "Boolean")
            {
                var colLetter = GetExcelColumnName(i + 4); // 4. kolondan başlar (A=1, B=2, C=3, D=4)
                var validation = dataSheet.DataValidations.AddListValidation($"{colLetter}2:{colLetter}1000");
                validation.Formula.Values.Add("Evet");
                validation.Formula.Values.Add("Hayır");
            }
        }

        dataSheet.Cells.AutoFitColumns();
        return await package.GetAsByteArrayAsync();
    }

    // Yardımcı metot: Sayıyı Excel kolon harfine çevirir (1->A, 2->B...)
    private string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }
        return columnName;
    }

    public List<Guest> ParseGuestExcelSync(Stream fileStream, Event ev)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var guests = new List<Guest>();

        try
        {
            if (fileStream.CanSeek) fileStream.Position = 0;

            // LoadAsync yerine doğrudan constructor kullanımı tam senkrondur
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name.Contains("Liste"))
                                ?? package.Workbook.Worksheets[0];

                if (worksheet == null || worksheet.Dimension == null) return guests;

                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                var headerMapping = new Dictionary<int, string>();
                for (int col = 4; col <= colCount; col++)
                {
                    var header = worksheet.Cells[1, col].Text?.Trim();
                    if (!string.IsNullOrEmpty(header)) headerMapping[col] = header;
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var firstName = worksheet.Cells[row, 1].Text?.Trim();
                    var lastName = worksheet.Cells[row, 2].Text?.Trim();
                    var phone = worksheet.Cells[row, 3].Text?.Trim();

                    if (string.IsNullOrWhiteSpace(firstName)) continue;

                    var guest = new Guest(ev.Id, firstName, lastName, phone ?? "");

                    var additionalData = new Dictionary<string, string>();
                    foreach (var mapping in headerMapping)
                    {
                        var cellValue = worksheet.Cells[row, mapping.Key].Text?.Trim();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            additionalData[mapping.Value] = cellValue;
                        }
                    }

                    guest.SetImportedAdditionalData(additionalData);
                    guests.Add(guest);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ParseGuestExcelSync KRİTİK HATA: {ex.Message}");
            throw;
        }

        return guests;
    }
    public async Task<List<Guest>> ParseGuestExcelAsync(Stream fileStream, Event ev)
    {
        // LİSANS BURADA OLMAZSA EPPLUS ÇÖKEBİLİR
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var guests = new List<Guest>();

        try
        {
            // MemoryStream kullanımı zorunlu (Blazor Stream direkt okunamıyor demiştik)
            using (var package = new ExcelPackage())
            {
                await package.LoadAsync(fileStream); // Bu metod daha güvenlidir

                var worksheet = package.Workbook.Worksheets[0]; // 1 yerine 0. index dene (bazen fark eder)
                if (worksheet == null) return guests;

                var rowCount = worksheet.Dimension?.Rows ?? 0;
                var colCount = worksheet.Dimension?.Columns ?? 0;

                var headerMapping = new Dictionary<int, string>();
                for (int col = 4; col <= colCount; col++)
                {
                    var header = worksheet.Cells[1, col].Text;
                    if (!string.IsNullOrEmpty(header)) headerMapping[col] = header;
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var firstName = worksheet.Cells[row, 1].Value?.ToString();
                    var lastName = worksheet.Cells[row, 2].Value?.ToString();
                    var phone = worksheet.Cells[row, 3].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(firstName)) continue;

                    var guest = new Guest(ev.Id, firstName, lastName, phone ?? "");

                    var additionalData = new Dictionary<string, string>();
                    foreach (var mapping in headerMapping)
                    {
                        var cellValue = worksheet.Cells[row, mapping.Key].Value?.ToString();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            additionalData[mapping.Value] = cellValue;
                        }
                    }

                    guest.SetImportedAdditionalData(additionalData);
                    guests.Add(guest);
                }
            }
        }
        catch (Exception ex)
        {
            // Log.Error(ex, "Excel Parse Hatası!");
            throw; // Buradan fırlat ki UI'daki try-catch yakalasın
        }

        return guests;
    }
}