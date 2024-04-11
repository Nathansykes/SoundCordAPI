using Project.Domain;
using Project.Domain.Channels;
using Project.Generic;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Channels;
public class ChannelMapper : IModelMapper<Channel, ChannelModel>
{
    public Channel MapToDatabaseModel(ChannelModel domainModel, Channel? databaseModel = null)
    {
        databaseModel ??= new();
        databaseModel.ChannelName = domainModel.ChannelName;
        databaseModel.GroupId = domainModel.GroupId;
        return databaseModel;
    }

    public ChannelModel MapToDomainModel(Channel databaseModel, ChannelModel? domainModel = null)
    {
        domainModel ??= new();
        domainModel.Id = databaseModel.Id;
        domainModel.GroupId = databaseModel.GroupId;
        domainModel.ChannelName = databaseModel.ChannelName;
        if (databaseModel.Song != null)
            domainModel.Song = (IdNameModel?)new IdNameModel(databaseModel.Song.Id, databaseModel.Song.SongName);
        else 
            domainModel.Song = null;
        return domainModel;
    }
}
