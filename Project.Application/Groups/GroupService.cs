using Project.Domain;
using Project.Domain.Groups;
using Project.Infrastructure.Model.Entities;

namespace Project.Application.Groups;

public class GroupService(
    IGroupRepository<Group> groupRepository,
    IModelMapper<Group, GroupModel> modelMapper,
    ICurrentUserAccessor currentUserAccessor) : IGroupService
{
    private readonly IGroupRepository<Group> _groupRepository = groupRepository;
    private readonly IModelMapper<Group, GroupModel> _modelMapper = modelMapper;
    private readonly ICurrentUserAccessor _userAccessor = currentUserAccessor;

    public GroupModel CreateGroup(GroupModel group)
    {
        var entity = _modelMapper.MapToDatabaseModel(group);
        _groupRepository.Create(entity);
        AddUserToGroup(entity.Id, _userAccessor.User!.UserName);
        return _modelMapper.MapToDomainModel(entity);
    }

    public GroupModel GetGroup(Guid groupId)
    {
        var entity = _groupRepository.GetById(groupId);
        return _modelMapper.MapToDomainModel(entity);
    }

    public ICollection<GroupModel> GetGroups()
    {
        var list = _groupRepository.GetAll().ToList();
        return list.Select(entity => _modelMapper.MapToDomainModel(entity)).ToList();
    }

    public void AddUserToGroup(Guid groupId, string userName)
    {
        _groupRepository.AddUserToGroup(groupId, userName);
    }

    public void RemoveUserFromGroup(Guid groupId, string userName)
    {
        _groupRepository.RemoveUserFromGroup(groupId, userName);
    }
}
