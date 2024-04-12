using Project.Domain;
using Project.Domain.Channels;
using Project.Domain.Messages;
using Project.Generic;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        domainModel.ChannelId = databaseModel.ChannelMessage!.ChannelId;
        domainModel.Content = databaseModel.Content;
        domainModel.CreatedAt = databaseModel.Utc;
        domainModel.User = databaseModel.User.UserName!;
        return domainModel;
    }
}
