using Project.Domain.Exceptions;
using Project.Domain.Files;
using Project.Infrastructure.Model;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Repositories;
public class FileMetadataRepository(IUserApplicationDbContext dbContext) : IFileMetadataRepository<FileMetadatum>
{
    private readonly IUserApplicationDbContext _context = dbContext;

    public FileMetadatum Create(FileMetadatum entity)
    {
        entity.UploadedByUserId = _context.ContextUser.Id;
        _context.FileMetadata.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void DeleteById(Guid id)
    {
        var entity = GetById(id);
        if (entity.UploadedByUserId != _context.ContextUser.Id)
            throw new UnauthorizedAccessException("You are not allowed to delete this file metadata");
        _context.FileMetadata.Remove(entity);
        _context.SaveChanges();
    }

    public IQueryable<FileMetadatum> GetAll()
    {
        return _context.FileMetadata
            .Where(x => x.SongRevisions
                .Any(y => y.Song.Channel.Group.Users
                    .Any(z => z.Id == _context.ContextUser.Id)));
    }

    public FileMetadatum GetById(Guid id)
    {
        return TryGetById(id, out FileMetadatum? entity) ? entity! : throw new NotFoundException($"No file metadata found with Id: {id}");
    }
    public bool TryGetById(Guid id, out FileMetadatum? entity)
    {
        entity = GetAll().FirstOrDefault(x => x.Id == id);
        return entity != null;
    }
}
