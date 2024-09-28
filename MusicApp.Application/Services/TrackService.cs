using AutoMapper;
using MusicApp.Application.DTOs;
using MusicApp.Application.Interfaces;
using MusicApp.Domain.Entities;
using MusicApp.Domain.Interfaces;

namespace MusicApp.Application.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IMapper _mapper;

        public TrackService(ITrackRepository trackRepository, IMapper mapper)
        {
            _trackRepository = trackRepository;
            _mapper = mapper;
        }

        public async Task<TrackDTO> GetTrackByIdAsync(Guid trackId)
        {
            var track = await _trackRepository.GetTrackByIdAsync(trackId);
            return _mapper.Map<TrackDTO>(track);
        }

        public async Task<Guid> CreateTrackAsync(TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            await _trackRepository.AddTrackAsync(track);
            return track.Id;
        }

        public async Task<bool> UpdateTrackAsync(TrackDTO trackDTO)
        {
            var track = _mapper.Map<Track>(trackDTO);
            await _trackRepository.UpdateTrackAsync(track);
            return true;
        }

        public async Task<bool> DeleteTrackAsync(Guid trackId)
        {
            await _trackRepository.DeleteTrackAsync(trackId);
            return true;
        }
    }
}
