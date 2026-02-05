using System.Text.RegularExpressions;

namespace LcvFlow.Service.Extensions;

public static class StringExtensions
{
    public static string ToSlug(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Küçük harfe çevir ve Türkçe karakterleri değiştir
        string str = text.ToLowerInvariant()
            .Replace('ö', 'o')
            .Replace('ü', 'u')
            .Replace('ı', 'i')
            .Replace('ş', 's')
            .Replace('ç', 'c')
            .Replace('ğ', 'g')
            .Replace("&", "ve");

        // Geçersiz karakterleri temizle (Sadece harf, rakam ve boşluk kalsın)
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // Birden fazla boşluğu tek boşluğa indir ve kenarlardaki boşlukları sil
        str = Regex.Replace(str, @"\s+", " ").Trim();

        // Boşlukları tireye çevir
        str = str.Replace(" ", "-");

        return str;
    }
}