using Project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Groups;
public interface IGroupRepository<TGroupEntity> : IRepository<TGroupEntity, Guid> where TGroupEntity : class, new()
{
    ICollection<TGroupEntity> GetByName(string name);
    void AddUserToGroup(Guid id, string userName);
}
