using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Groups;

public interface IGroupService
{
    GroupModel CreateGroup(GroupModel group);
    GroupModel GetGroup(Guid id);
    ICollection<GroupModel> GetGroups();
    void AddUserToGroup(Guid groupId, string userName);
}


