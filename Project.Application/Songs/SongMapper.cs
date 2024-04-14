using Project.Domain;
using Project.Domain.Songs;
using Project.Generic;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Songs;
public class SongMapper : IModelMapper<Song, SongModel>
{
    public Song MapToDatabaseModel(SongModel domainModel, Song? databaseModel)
    {
        databaseModel ??= new Song();
        databaseModel.SongName = domainModel.SongName;
        return databaseModel;
    }

    public SongModel MapToDomainModel(Song databaseModel, SongModel? domainModel)
    {
        domainModel ??= new SongModel();
        domainModel.Id = databaseModel.Id;
        domainModel.SongName = databaseModel.SongName;
        domainModel.Revisions = databaseModel.SongRevisions.Select(sr => new IdNameModel(sr.SongId, sr.RevisionName ?? "")).ToList();
        domainModel.ChannelId = databaseModel.ChannelId;
        domainModel.CreatedByUser = databaseModel.CreatedByUser.UserName!;
        return domainModel;
    }
}

public class SongRevisionMapper : IModelMapper<SongRevision, SongRevisionModel>
{
    public SongRevision MapToDatabaseModel(SongRevisionModel domainModel, SongRevision? databaseModel)
    {
        databaseModel ??= new SongRevision();
        databaseModel.RevisionName = domainModel.RevisionName;
        databaseModel.SongId = domainModel.SongId;
        return databaseModel;
    }

    public SongRevisionModel MapToDomainModel(SongRevision databaseModel, SongRevisionModel? domainModel)
    {
        domainModel ??= new SongRevisionModel();
        domainModel.Id = databaseModel.Id;
        domainModel.SongId = databaseModel.SongId!;
        domainModel.RevisionName = databaseModel.RevisionName;
        domainModel.FileMetadataId = databaseModel.FileMetaDataId;
        domainModel.ChannelId = databaseModel.Song.ChannelId;
        domainModel.CreatedByUser = databaseModel.CreatedByUser.UserName!;
        return domainModel;
    }
}
