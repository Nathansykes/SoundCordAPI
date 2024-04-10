using Project.Domain.Groups;
using Project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Infrastructure.Model.Entities;
using System.Runtime.CompilerServices;
using Project.Infrastructure.Model;
using Project.Infrastructure.Repositories;

namespace Project.Application.Groups;

public class GroupService(
    IGroupRepository<Group> groupRepository, 
    IModelMapper<Group, GroupModel> modelMapper,
    ICurrentUserAccessor currentUserAccessor) : IGroupService
{
    private readonly IGroupRepository<Group> _groupRepository = groupRepository;
    private readonly IModelMapper<Group, GroupModel> _modelMapper = modelMapper;
    private readonly IApplicationUser _user = currentUserAccessor.User;
    
    public GroupModel CreateGroup(GroupModel group)
    {
        var entity = _modelMapper.MapToDatabaseModel(group);
        _groupRepository.Create(entity);
        AddUserToGroup(entity.Id, _user.UserName);
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
}
