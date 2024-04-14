using Microsoft.Extensions.Configuration;
using Project.Domain;
using Project.Domain.Files;
using Project.Domain.Files.FileUploading;
using Project.Domain.Songs;
using Project.Generic;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Songs;

public class SongRevisionService(
    ISongRevisionRepository<SongRevision> songRevisionRepository,
    ISongRepository<Song> songRepository,
    IModelMapper<SongRevision, SongRevisionModel> mapper,
    IFileUploadService fileUploadService,
    IFileMetadataRepository<FileMetadatum> fileMetadataRepository,
    IConfiguration configuration) : ISongRevisionService
{
    private readonly ISongRevisionRepository<SongRevision> _songRevisionRepository = songRevisionRepository;
    private readonly ISongRepository<Song> _songRepository = songRepository;
    private readonly IModelMapper<SongRevision, SongRevisionModel> _mapper = mapper;
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IFileMetadataRepository<FileMetadatum> _fileMetadataRepository = fileMetadataRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<SongRevisionModel> CreateSongRevision(Guid songId, CreateSongRevisionRequest request)
    {
        request.SongRevision.SongId = songId;
        var songRevisionEntity = _mapper.MapToDatabaseModel(request.SongRevision);
        var songEntity = _songRepository.GetById(songId);

        var fileUploadModel = new FileUploadModel()
        {
            Content = request.File.Content,
            Extension = request.File.Extension,
            OriginalFileName = request.File.FileName,
        };
        Guid[] directories = [songEntity.ChannelId, songId];
        var file = await _fileUploadService.UploadFile(directories, fileUploadModel);

        var fileMetadata = new FileMetadatum()
        {
            OriginalFileName = file.OriginalFileName,
            OriginalExtension = file.Extension,
            NewFileName = file.NewFileName!,
            Directory = string.Join('/', directories),
            ContentHash = file.ContentHash ?? file.GetContentAsByteArray()!.ComputeMD5Hash().GetBytesAsHexString(),
            ContentLengthBytes = file.GetContentAsByteArray()!.Length,
            FileShare = _configuration.GetConnectionString("Azure:ShareName")!,
        };

        var fileEntity = _fileMetadataRepository.Create(fileMetadata);


        songRevisionEntity.FileMetaDataId = fileEntity.Id;
        songRevisionEntity = _songRevisionRepository.Create(songRevisionEntity);
        return _mapper.MapToDomainModel(songRevisionEntity);
    }

    public List<SongRevisionModel> GetSongRevisions(Guid songId)
    {
        var songRevisions = _songRevisionRepository.GetBySongId(songId).ToList();
        return songRevisions.Select(sr => _mapper.MapToDomainModel(sr)).ToList();
    }
    public List<SongRevisionModel> GetSongRevisionsByChannelId(Guid channelId)
    {
        var songRevisions = _songRevisionRepository.GetByChannelId(channelId).ToList();
        return songRevisions.Select(sr => _mapper.MapToDomainModel(sr)).ToList();
    }

    public SongRevisionModel GetSongRevisionById(Guid id)
    {
        var songRevision = _songRevisionRepository.GetById(id);
        return _mapper.MapToDomainModel(songRevision);
    }
}
