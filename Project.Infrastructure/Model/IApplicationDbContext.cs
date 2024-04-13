using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project.Domain;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Model;

public interface IApplicationDbContext : IDisposable
{
    DbSet<AspNetRole> AspNetRoles { get; set; }
    DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
    DbSet<AspNetUser> AspNetUsers { get; set; }
    DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    DbSet<Channel> Channels { get; set; }
    DbSet<FileMetadatum> FileMetadata { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<Song> Songs { get; set; }
    DbSet<SongRevision> SongRevisions { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Entry(object entity);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

public interface IUserApplicationDbContext : IApplicationDbContext
{
    public AspNetUser ContextUser { get; }
}

public class UserApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserAccessor userAccessor)
    : ApplicationDbContext(options), IUserApplicationDbContext
{
    private readonly IApplicationUser _user = userAccessor.User;
    private AspNetUser? _contextUser;
    public AspNetUser ContextUser => _contextUser ??= AspNetUsers.First(x => x.Id == _user.Id);
}