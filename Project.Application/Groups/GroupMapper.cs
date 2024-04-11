using Project.Domain;
using Project.Domain.Groups;
using Project.Generic;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Groups;
public class GroupMapper : IModelMapper<Group, GroupModel>
{
    public Group MapToDatabaseModel(GroupModel domainModel, Group? databaseModel = null)
    {
        databaseModel ??= new();
        databaseModel.GroupName = domainModel.GroupName;
        return databaseModel;
    }

    public GroupModel MapToDomainModel(Group databaseModel, GroupModel? domainModel = null)
    {
        domainModel ??= new();
        domainModel.Id = databaseModel.Id;
        domainModel.GroupName = databaseModel.GroupName;
        domainModel.CreatedByUser = databaseModel.CreatedByUser.UserName!;
        domainModel.Channels = databaseModel.Channels.Select(x => new IdNameModel(x.Id, x.ChannelName)).ToList();
        domainModel.Users = databaseModel.Users.Select(x => x.UserName!).ToList();
        return domainModel;
    }
}
