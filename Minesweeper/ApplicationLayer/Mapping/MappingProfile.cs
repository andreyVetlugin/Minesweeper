using AutoMapper;
using Minesweeper.ApplicationLayer.Models;
using Minesweeper.ApplicationLayer.Views;

namespace Minesweeper.ApplicationLayer.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateMap<Game, DataLayer.Models.Game>()
            .ReverseMap();

        CreateMap<Map, DataLayer.Models.Map>()
            .ForMember(x => x.MinesCoordinates, opt => opt.MapFrom(o => o.MinesCoordinates))
            .ReverseMap();

        CreateMap<Point, DataLayer.Models.Point>()
            .ReverseMap();

        CreateMap<Game, GameInfoResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Map.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Map.Height))
            .ForMember(dest => dest.MinesCount, opt => opt.MapFrom(src => src.Map.MinesCount))
            .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.IsEnded))
            .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Map.Field));


        CreateMap<DataLayer.Models.Game, GameInfoResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Map.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Map.Height))
            .ForMember(dest => dest.MinesCount, opt => opt.MapFrom(src => src.Map.MinesCount))
            .ForMember(dest => dest.Field, opt => opt.MapFrom(src => src.Map.Field))
            .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.IsEnded));
    }
}