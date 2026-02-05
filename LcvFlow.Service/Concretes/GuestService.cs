using AutoMapper;
using FluentValidation;
using LcvFlow.Domain;
using LcvFlow.Domain.Common;
using LcvFlow.Domain.Entities;
using LcvFlow.Service.Dtos.Guest;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Service.Concretes;

public class GuestService : IGuestService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<GuestRsvpDto> _validator; // DI ile geldi

    public GuestService(IUnitOfWork uow, IMapper mapper, IValidator<GuestRsvpDto> validator)
    {
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<GuestDto>> GetByTokenAsync(string token)
    {
        var guest = await _uow.Guests.Query()
            .AsNoTracking()
            .Include(x => x.Event)
            .FirstOrDefaultAsync(x => x.AccessToken == token);

        if (guest == null)
            return Result<GuestDto>.Failure("Davetiye bulunamadı.");

        return Result<GuestDto>.Success(_mapper.Map<GuestDto>(guest));
    }

    public async Task<Result<List<GuestListDto>>> GetAllByEventIdAsync(int eventId)
    {
        // GetAllAsync metodumuzu kullanarak filtreli liste çekiyoruz
        var guests = await _uow.Guests.GetAllAsync(x => x.EventId == eventId);
        return Result<List<GuestListDto>>.Success(_mapper.Map<List<GuestListDto>>(guests));
    }

    public async Task<Result<bool>> SubmitRsvpAsync(GuestRsvpDto rsvpDto)
    {
        var validationResult = await _validator.ValidateAsync(rsvpDto);
        if (!validationResult.IsValid)
        {
            var allErrors = string.Join("|", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<bool>.Failure(allErrors);
        }

        var guest = await _uow.Guests.GetAsync(x => x.AccessToken == rsvpDto.AccessToken);
        if (guest == null)
        {
            return Result<bool>.Failure("Davetli bulunamadı.");
        }

        _mapper.Map(rsvpDto, guest);
        _uow.Guests.Update(guest);

        var res = await _uow.SaveChangesAsync();
        if (res <= 0)
        {
            return Result<bool>.Failure("Güncelleme sırasında bir hata oluştu.");
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> AddBulkGuestsAsync(List<GuestRsvpDto> guestDtos, int eventId)
    {
        var guests = _mapper.Map<List<Guest>>(guestDtos);
        guests.ForEach(g => { g.EventId = eventId; g.AccessToken = Guid.NewGuid().ToString("N"); });

        await _uow.Guests.AddRangeAsync(guests);
        var res = await _uow.SaveChangesAsync();
        return res > 0 ? Result<bool>.Success(true) : Result<bool>.Failure("Toplu kayıt başarısız.");
    }
}