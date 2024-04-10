using Project.Domain;
using Project.Domain.Groups;
using Project.Generic;
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
            Channels = databaseModel.Channels.Select(x => new IdNameModel(x.Id, x.ChannelName)).ToList(),
            Users = databaseModel.Users.Select(x => x.UserName!).ToList(),
        };
        return domainModel;
    }
}
