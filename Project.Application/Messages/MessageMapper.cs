using Project.Domain;
using Project.Domain.Messages;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Messages;
public class MessageMapper : IModelMapper<Message, MessageModel>
{
    public Message MapToDatabaseModel(MessageModel domainModel, Message? databaseModel = null)
    {
        databaseModel ??= new();
        databaseModel.Content = domainModel.Content;
        return databaseModel;
    }

    public MessageModel MapToDomainModel(Message databaseModel, MessageModel? domainModel = null)
    {
        domainModel ??= new();
        domainModel.Id = databaseModel.Id;
        domainModel.ChannelId = databaseModel.ChannelId;
        domainModel.Content = databaseModel.Content;
        domainModel.CreatedAt = databaseModel.Utc;
        domainModel.User = databaseModel.User.UserName!;
        domainModel.SongRevisionId = databaseModel.SongRevisionId;
        domainModel.SongTimestampMilliseconds = databaseModel.SongTimestampMilliseconds;
        return domainModel;
    }
}
