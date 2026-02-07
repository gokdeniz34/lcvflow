using LcvFlow.Domain.Common;
using LcvFlow.Service.Dtos.Admin;
using LcvFlow.Service.Dtos.Guest;

namespace LcvFlow.Service.Interfaces;

public interface IAdminService
{
    // Dashboard & İstatistik (eventId null gelebilir, bu durumda global özet döner)
    Task<Result<AdminDashboardDto>> GetDashboardSummaryAsync(int? eventId = null);

    // Misafir Yönetimi
    Task<Result<List<GuestListDto>>> GetAllGuestsAsync(int eventId);
    Task<Result<string>> CreateGuestAsync(CreateGuestDto dto);
    Task<Result> DeleteGuestAsync(int guestId);
    Task<Result> UpdateGuestNoteAsync(int guestId, string note);

    // Event Yönetimi
    Task<Result<int>> CreateEventAsync(CreateEventDto dto);

    // Toplu İşlemler
    Task<Result> AddBulkGuestsAsync(int eventId, List<CreateGuestDto> guests);
}