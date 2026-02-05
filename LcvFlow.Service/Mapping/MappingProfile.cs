using AutoMapper;
using LcvFlow.Domain.Entities;
using LcvFlow.Service.Dtos.Auth;
using LcvFlow.Service.Dtos.Guest;

namespace LcvFlow.Service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Guest, GuestDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.EventName, o => o.MapFrom(s => s.Event.Name));

        CreateMap<Guest, GuestListDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));

        CreateMap<GuestRsvpDto, Guest>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.AccessToken, o => o.Ignore());
    }
}
