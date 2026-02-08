# LcvFlow Teknik Mimari ve Geliştirici Kılavuzu

Bu döküman, LcvFlow projesinin kod seviyesindeki işleyişini ve katmanlar arası veri akışını detaylandırır.

## 🏛️ Katmanlı Mimari ve Kritik Metodlar

### 1. Domain Katmanı: İş Kuralları (Core Logic)
Veri bütünlüğünü korumak için Property'ler `private set` ile korunur. Değişiklikler sadece metodlar üzerinden yapılır.

#### **Guest Entity**
- `SubmitRsvp(bool isAttending, int adultCount, int childCount, string? note, Dictionary<string, string>? dynamicFields)`
    - **Görevi:** RSVP sürecini yönetir. Katılım `false` ise sayıları otomatik sıfırlar. Dinamik alanları `AdditionalProperties` sözlüğüne işler ve JSON senkronizasyonunu (`UpdateAdditionalDataJson`) tetikler.
- `LoadAdditionalProperties()`: `AdditionalDataJson` kolonundaki ham string'i `Dictionary` formatına mapler.
- `UpdateRsvpStatus(bool? isAttending)`: Sadece katılım durumunu günceller (Seed ve Admin hızlı düzenleme işlemleri için).

---

### 2. Service Katmanı: Uygulama Mantığı (Application)
Katmanlar arası veri taşıma (DTO) ve kompleks iş süreçlerini yönetir.

#### **IExcelService**
- `GenerateTemplateWithInstructionsAsync(Event ev)`: EPPlus kullanarak admin için özel yönergeler içeren dinamik bir Excel şablonu üretir.
- `ParseGuestExcelAsync(Stream fileStream, Event ev)`: Excel'deki kolon başlıklarını `Event.FormConfigJson` ile eşleştirerek `AdditionalProperties` sözlüğüne dinamik veri ataması yapar.

#### **IGuestService**
- `SubmitRsvpAsync(GuestRsvpDto rsvpDto)`: Web katmanından gelen DTO'yu alır, ilgili `Guest` entity'sini bulur ve Domain katmanındaki `SubmitRsvp` metodunu güvenli bir `Result` dönecek şekilde tetikler.

---

### 3. Data Katmanı: Veri Erişimi ve Persistence
Veritabanı seviyesindeki otomasyonları yönetir.

- **AppDbContext.SaveChangesAsync() Override:**
    - `BaseEntity` tipindeki tüm kayıtları `ChangeTracker` ile yakalar. 
    - `Added` durumunda `CreatedAt`, `Modified` durumunda `ModifiedAt` alanlarını `DateTime.UtcNow` olarak otomatik set eder.
- **Global Query Filtering:**
    - `OnModelCreating` içerisinde `HasQueryFilter(g => !g.IsDeleted)` kullanılarak, silinmiş kayıtların sistem genelinde "yok sayılması" sağlanır.

---

### 4. Web Katmanı: Sunum ve Middleware (Blazor Server)

- **Proxy Variable Pattern (Blazor Logic):** - `.razor` sayfalarında Domain nesnelerinin `private set` alanları doğrudan bind edilemez. 
    - Bu yüzden UI tarafında `_status`, `_adultCount` gibi geçici değişkenler tutulur, `HandleSave` anında bu değerler Entity metoduna parametre geçilir.
- **ExceptionMiddleware:**
    - Tüm `unhandled exception`ları yakalar. Status kodlarını (401, 404, 500) mapler.
    - Kritik hataları Serilog üzerinden MySQL'deki `Logs` tablosuna yazar.

## 💾 Veri Saklama Standartları
- **Dinamik Veri:** `AdditionalDataJson` kolonu MySQL üzerinde `json` tipinde saklanarak performanslı sorgulama imkanı sağlar.
- **Güvenlik:** Her davetli için `AccessToken` (Unique Index) üzerinden erişim sağlanır, `Id` tabanlı erişim engellenerek "ID Enumaration" saldırıları önlenir.