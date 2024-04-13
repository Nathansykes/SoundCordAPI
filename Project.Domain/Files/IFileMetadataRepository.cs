namespace Project.Domain.Files;
public interface IFileMetadataRepository<TFileMetadataEntity> where TFileMetadataEntity : new()
{
    TFileMetadataEntity Create(TFileMetadataEntity entity);
    void DeleteById(Guid id);
    IQueryable<TFileMetadataEntity> GetAll();
    TFileMetadataEntity GetById(Guid id);
    bool TryGetById(Guid id, out TFileMetadataEntity? entity);
}