using LcvFlow.Domain.Common;
using LcvFlow.Service.Dtos.Guest;

namespace LcvFlow.Service.Interfaces;

public interface IGuestService
{
    Task<Result<GuestDto>> GetByTokenAsync(string token);
    Task<Result<List<GuestListDto>>> GetAllByEventIdAsync(int eventId);
    Task<Result> SubmitRsvpAsync(GuestRsvpDto rsvpDto);
    Task<Result<bool>> AddBulkGuestsAsync(List<GuestRsvpDto> guests, int eventId);
}

