using AutoMapper;
using LcvFlow.Domain.Entities;
using LcvFlow.Service.Dtos.Auth;

namespace LcvFlow.Service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO (Veritabanından okurken)
        // DTO -> Entity (Kaydederken - ReverseMap sayesinde)
        CreateMap<AdminUser, UserDto>().ReverseMap();
        CreateMap<Guest, GuestDto>().ReverseMap();
        
        // İleride Event için de ekleyeceğiz:
        // CreateMap<Event, EventDto>().ReverseMap();
    }
}
