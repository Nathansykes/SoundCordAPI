namespace Project.Domain;
public interface IModelMapper<TDatabaseModel, TDomainModel>
    where TDatabaseModel : class, new()
    where TDomainModel : class, new()
{
    TDatabaseModel MapToDatabaseModel(TDomainModel domainModel, TDatabaseModel? databaseModel = null);
    TDomainModel MapToDomainModel(TDatabaseModel databaseModel, TDomainModel? domainModel = null);
}
