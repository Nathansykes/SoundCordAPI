using Project.Domain;
using Project.Domain.Songs;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Songs;

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
        domainModel.CreatedUtc = databaseModel.CreatedUtc;
        domainModel.LengthMilliseconds = databaseModel.LengthMilliseconds;
        return domainModel;
    }
}
