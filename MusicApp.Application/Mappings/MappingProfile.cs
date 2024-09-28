using System;
using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Domain.Entities;

namespace MusicApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Playlist, PlaylistDTO>().ReverseMap();
        CreateMap<Track, TrackDTO>().ReverseMap();
    }
}
