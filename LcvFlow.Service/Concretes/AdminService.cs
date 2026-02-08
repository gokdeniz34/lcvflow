using AutoMapper;
using LcvFlow.Domain.Common;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain.Interfaces;
using LcvFlow.Service.Dtos.Admin;
using LcvFlow.Service.Dtos.Guest;
using LcvFlow.Service.Extensions;
using LcvFlow.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Service.Concretes;

public class AdminService : IAdminService
{
    private readonly IMapper _mapper;
    private readonly IGuestRepository _guestRepository;
    private readonly IEventRepository _eventRepository;

    public AdminService(IGuestRepository guestRepository, IEventRepository eventRepository, IMapper mapper)
    {
        _guestRepository = guestRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<Result<AdminDashboardDto>> GetDashboardSummaryAsync(int? eventId = null)
    {
        var query = _guestRepository.Query().AsNoTracking();

        if (eventId.HasValue && eventId > 0)
        {
            query = query.Where(x => x.EventId == eventId.Value);
        }

        var guests = await query.ToListAsync();

        return Result<AdminDashboardDto>.Success(new AdminDashboardDto
        {
            TotalGuests = guests.Count,
            AttendingCount = guests.Count(x => x.IsAttending == true),
            DeclinedCount = guests.Count(x => x.IsAttending == false),
            PendingCount = guests.Count(x => x.IsAttending == null),
            TotalAdults = guests.Where(x => x.IsAttending == true).Sum(g => g.AdultCount),
            TotalChildren = guests.Where(x => x.IsAttending == true).Sum(g => g.ChildCount)
        });
    }

    public async Task<Result<List<GuestListDto>>> GetAllGuestsAsync(int eventId)
    {
        var guests = await _guestRepository.Query()
            .Where(x => x.EventId == eventId)
                .AsNoTracking()
            .ToListAsync();

        return Result<List<GuestListDto>>.Success(_mapper.Map<List<GuestListDto>>(guests));
    }

    public async Task<Result<string>> CreateGuestAsync(CreateGuestDto dto)
    {
        try
        {
            // GÜNCELLEME: Constructor sırası -> (eventId, firstName, lastName, phone)
            // DTO'da phone yoksa boş string gönderiyoruz
            var guest = new Guest(dto.EventId, dto.FirstName, dto.LastName, "");

            await _guestRepository.AddAsync(guest);
            await _guestRepository.SaveChangesAsync();
            return Result<string>.Success(guest.AccessToken);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Misafir oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result> AddBulkGuestsAsync(int eventId, List<CreateGuestDto> guests)
    {
        try
        {
            foreach (var dto in guests)
            {
                // GÜNCELLEME: Constructor sırası
                var guest = new Guest(eventId, dto.FirstName, dto.LastName, "");
                await _guestRepository.AddAsync(guest);
            }
            await _guestRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Toplu ekleme hatası: {ex.Message}");
        }
    }

    public async Task<Result> DeleteGuestAsync(int guestId)
    {
        try
        {
            var guest = await _guestRepository.Query().FirstOrDefaultAsync(x => x.Id == guestId);
            if (guest == null) return Result.Failure("Misafir bulunamadı.");

            _guestRepository.Remove(guest);
            await _guestRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Silme hatası: {ex.Message}");
        }
    }

    public async Task<Result> UpdateGuestNoteAsync(int guestId, string note)
    {
        try
        {
            var guest = await _guestRepository.Query().FirstOrDefaultAsync(x => x.Id == guestId);
            if (guest == null) return Result.Failure("Misafir bulunamadı.");

            // Entity içindeki davranış (metot) üzerinden güncellemek daha temizdir.
            // Eğer yoksa: guest.AdminNote = note; 
            await _guestRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Güncelleme hatası: {ex.Message}");
        }
    }

    public Task<Result<int>> CreateEventAsync(CreateEventDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task SeedDummyDataAsync(int eventId)
    {
        var random = new Random();
        var names = new[] { "Ahmet", "Ayşe", "Mehmet", "Fatma", "Can", "Zeynep", "Burak", "Ece" };
        var lastNames = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Öztürk" };

        var dummyGuests = new List<Guest>();

        for (int i = 0; i < 50; i++) // 50 örnek kişi
        {
            var guest = new Guest(
                eventId,
                names[random.Next(names.Length)],
                lastNames[random.Next(lastNames.Length)],
                "555" + random.Next(1000000, 9999999)
            );

            // Rastgele LCV durumu (Gelen, Gelmeyen, Bekleyen)
            var status = random.Next(0, 3);
            if (status == 1)
                guest.UpdateRsvpStatus(true);
            else if (status == 2)
                guest.UpdateRsvpStatus(false);

            // Dinamik veri simülasyonu
            var additional = new Dictionary<string, string> {
            { "Masa No", random.Next(1, 20).ToString() },
            { "Not", "Sistem tarafından otomatik üretildi." }
        };
            guest.SetImportedAdditionalData(additional);

            dummyGuests.Add(guest);
        }

        await _guestRepository.AddRangeAsync(dummyGuests);
        await _guestRepository.SaveChangesAsync();
    }
}