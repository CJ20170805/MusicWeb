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
        CreateMap<User, UserDTO>().ReverseMap();

        CreateMap<UserRoles, RoleDTO>().ReverseMap();

        CreateMap<Track, TrackDTO>()
             .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (TimeSpan)src.Duration))  // Map Duration to TimeSpan
             .ReverseMap()
             .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => new Duration(src.Duration))); // Map TimeSpan back to Duration

        CreateMap<Playlist, PlaylistDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src => src.PlaylistTracks.Select(pt => pt.Track)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap()
            .ForMember(dest => dest.PlaylistTracks, opt => opt.MapFrom(src => src.Tracks));

        CreateMap<PlaylistDTO, Playlist>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.PlaylistTracks, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<TrackDTO, PlaylistTrack>()
         .ConvertUsing(src => CreatePlaylistTrack(src));
    }

    private PlaylistTrack CreatePlaylistTrack(TrackDTO src)
    {
        if (string.IsNullOrEmpty(src.Title))
        {
            throw new ArgumentNullException(nameof(src.Title), "Title cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(src.Artist))
        {
            throw new ArgumentNullException(nameof(src.Artist), "Artist cannot be null or empty.");
        }

        // Create the Track entity
        var track = new Track(src.Title, src.Artist, src.Duration);

        // Create and return PlaylistTrack
        return new PlaylistTrack { Track = track };
    }
}
