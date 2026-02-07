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
            var guest = new Guest(dto.FirstName, dto.LastName, dto.EventId);
            await _guestRepository.AddAsync(guest);
            await _guestRepository.SaveChangesAsync();
            return Result<string>.Success(guest.AccessToken);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Misafir oluşturulamadı: {ex.Message}");
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

    public async Task<Result<int>> CreateEventAsync(CreateEventDto dto)
    {
        try
        {
            var slug = dto.Name.ToSlug();
            var @event = new Event(dto.Name, dto.EventDate, dto.Location, slug);
            await _eventRepository.AddAsync(@event);
            await _eventRepository.SaveChangesAsync();
            return Result<int>.Success(@event.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Etkinlik hatası: {ex.Message}");
        }
    }

    public async Task<Result> AddBulkGuestsAsync(int eventId, List<CreateGuestDto> guests)
    {
        try
        {
            foreach (var dto in guests)
            {
                var guest = new Guest(dto.FirstName, dto.LastName, eventId);
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
}