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

        CreateMap<Character, CharacterDisplayDto>();

        // Add mapping for NewUserDto to User
        CreateMap<NewUserDto, User>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => 0)) // Initialize Balance
            .ForMember(dest => dest.CardPacks, opt => opt.MapFrom(src => new List<CardPack>())) // Initialize CardPacks
            .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => new List<Card>())); // Initialize Cards

        CreateMap<User, ReturnUserDto>()
            .ForMember(dest => dest.CardPacks, opt => opt.MapFrom(src => src.CardPacks));

        // Add mapping for CardPack to CardPackDto
        CreateMap<CardPack, CardPackDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.PackType, opt => opt.MapFrom(src => src.PackType.ToString()))
            .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Rarity.ToString()));
    }
}
