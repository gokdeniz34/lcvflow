using AutoMapper;
using FluentValidation;
using LcvFlow.Domain.Common;
using LcvFlow.Domain.Interfaces;
using LcvFlow.Service.Dtos.Guest;
using LcvFlow.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Service.Concretes;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GuestRsvpDto> _validator;

    public GuestService(IGuestRepository guestRepository, IMapper mapper, IValidator<GuestRsvpDto> validator)
    {
        _guestRepository = guestRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public Task<Result<bool>> AddBulkGuestsAsync(List<GuestRsvpDto> guests, int eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<GuestListDto>>> GetAllByEventIdAsync(int eventId)
    {
        var guests = await _guestRepository.Query()
            .Where(x => x.EventId == eventId)
            .AsNoTracking()
            .ToListAsync();

        return Result<List<GuestListDto>>.Success(_mapper.Map<List<GuestListDto>>(guests));
    }

    public async Task<Result<GuestDto>> GetByTokenAsync(string token)
    {
        var guest = await _guestRepository.Query()
            .Where(x => x.AccessToken == token)
            .Include(x => x.Event)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (guest == null)
            return Result<GuestDto>.Failure("Davetiye bulunamadı.");

        return Result<GuestDto>.Success(_mapper.Map<GuestDto>(guest));
    }

    public async Task<Result> SubmitRsvpAsync(GuestRsvpDto rsvpDto)
    {
        var valResult = await _validator.ValidateAsync(rsvpDto);
        if (!valResult.IsValid)
            return Result.Failure(string.Join(", ", valResult.Errors.Select(e => e.ErrorMessage)));

        var guest = await _guestRepository.Query()
            .Where(x => x.AccessToken == rsvpDto.AccessToken)
            .FirstOrDefaultAsync();

        if (guest == null) return Result.Failure("Davetli bulunamadı.");

        var domainResult = guest.SubmitRsvp(
            rsvpDto.IsAttending ?? false,
            rsvpDto.AdultCount,
            rsvpDto.ChildCount,
            rsvpDto.Note);

        if (domainResult.IsFailure) return domainResult;

        await _guestRepository.SaveChangesAsync();

        return Result.Success();
    }
}