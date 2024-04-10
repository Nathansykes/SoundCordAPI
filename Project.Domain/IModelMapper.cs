using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain;
public interface IModelMapper<TDatabaseModel, TDomainModel>
    where TDatabaseModel : class, new()
    where TDomainModel : class, new()
{
    TDatabaseModel MapToDatabaseModel(TDomainModel domainModel, TDatabaseModel? databaseModel = null);
    TDomainModel MapToDomainModel(TDatabaseModel databaseModel, TDomainModel? domainModel = null);
}
