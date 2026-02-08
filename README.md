# LcvFlow

LCV (Lütfen Cevap Veriniz) süreçlerini profesyonel, dinamik ve ölçeklenebilir bir şekilde yönetmek için geliştirilmiş etkinlik yönetim sistemidir.

## ✨ Temel Özellikler
- **Dinamik Form Yapısı:** Her etkinliğe özel sorular ekleyebilme.
- **Excel Entegrasyonu:** Şablon üzerinden toplu davetli aktarımı.
- **Token Bazlı Erişim:** Davetlilere özel güvenli linkler.
- **Admin Dashboard:** Anlık katılım istatistikleri ve log takibi.

## 🛠️ Teknoloji Yığını
- **Backend:** .NET 9.0, Entity Framework Core
- **Frontend:** Blazor Server (Interactive Server Mode)
- **Database:** MySQL (JSON Data Type desteği ile)
- **Reporting:** EPPlus (Excel Processing)
- **Logging:** Serilog (MySQL Sink)

## 🏁 Başlangıç
1. `appsettings.json` içerisindeki `DefaultConnection` string'ini güncelleyin.
2. `Update-Database` komutu ile migration'ları uygulayın.
3. Projeyi çalıştırın ve `/admin/login` üzerinden giriş yapın.

---
> **Mimari kararlar, metod tanımları ve geliştirici notları için [ARCHITECTURE.md](ARCHITECTURE.md) dosyasını inceleyin.**