using Project.Domain.Groups;
using Project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Groups;
public class GroupMapper : IModelMapper<Group, GroupModel>
{
    public Group MapToDatabaseModel(GroupModel domainModel, Group? databaseModel = null)
    {
        databaseModel ??= new()
        {
            GroupName = domainModel.GroupName,
        };
        return databaseModel;
    }

    public GroupModel MapToDomainModel(Group databaseModel, GroupModel? domainModel = null)
    {
        domainModel ??= new()
        {
            Id = databaseModel.Id,
            GroupName = databaseModel.GroupName,
            CreatedByUserId = databaseModel.CreatedByUserId,
            Channels = databaseModel.Channels.ToDictionary(x => x.Id, x => x.ChannelName),
            Users = databaseModel.Users.Select(x => x.Id).ToList(),
        };
        return domainModel;
    }
}
