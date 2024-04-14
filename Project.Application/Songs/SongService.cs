using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Songs;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Songs;

public class SongService(
    ISongRepository<Song> songRepository,
    IModelMapper<Song, SongModel> mapper,
    IChannelService channelService) : ISongService
{
    private readonly ISongRepository<Song> _songRepository = songRepository;
    private readonly IModelMapper<Song, SongModel> _mapper = mapper;
    private readonly IChannelService _channelService = channelService;

    public SongModel GetSongById(Guid id)
    {
        var entity = _songRepository.GetById(id);
        return _mapper.MapToDomainModel(entity);
    }

    public SongModel? GetForChannelId(Guid channelId)
    {
        var entity = _songRepository.GetForChannelId(channelId);
        if (entity is null)
            return null;
        return _mapper.MapToDomainModel(entity);
    }

    public List<SongModel> GetAllSongs()
    {
        var entities = _songRepository.GetAll().ToList();
        return entities.Select(s => _mapper.MapToDomainModel(s)).ToList();
    }

    public SongModel CreateSong(Guid groupId, SongModel song)
    {
        var channel = new ChannelModel()
        {
            ChannelName = song.SongName,
        };
        var createdChannel = _channelService.CreateChannel(groupId, channel);

        var songEntity = _mapper.MapToDatabaseModel(song);
        songEntity.ChannelId = createdChannel.Id;
        _songRepository.Create(songEntity);


        return _mapper.MapToDomainModel(songEntity);
    }
}
