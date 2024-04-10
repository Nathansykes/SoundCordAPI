namespace Project.Domain.Groups;

public interface IGroupService
{
    GroupModel CreateGroup(GroupModel group);
    GroupModel GetGroup(Guid id);
    ICollection<GroupModel> GetGroups();
    void AddUserToGroup(Guid groupId, string userName);
}


