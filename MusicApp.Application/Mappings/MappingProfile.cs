using System;
using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Domain.Entities;
using MusicApp.Domain.ValueObject;

namespace MusicApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Playlist, PlaylistDTO>().ReverseMap();

        CreateMap<Track, TrackDTO>()
             .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (TimeSpan)src.Duration))  // Map Duration to TimeSpan
             .ReverseMap()
             .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => new Duration(src.Duration))); // Map TimeSpan back to Duration

        CreateMap<Playlist, PlaylistDTO>()
     .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src => src.Tracks));

        CreateMap<PlaylistDTO, Playlist>()
            .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src => src.Tracks));

    }
}
