using AutoMapper;
using RollBotApi.Models;
using RollBotApi.DTOs;

namespace RollBotApi.Profiles;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateSerieDto, Serie>()
            .ForMember(dest => dest.TagIds, opt => opt.Ignore());

        CreateMap<CreateCharacterDto, Character>();

        CreateMap<Serie, DisplaySerieDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Characters, opt => opt.Ignore()) // Ignore Characters for now
            .ForMember(dest => dest.Tags, opt => opt.Ignore()); // Ignore Tags for now
    }
}
