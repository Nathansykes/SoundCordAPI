using Project.Infrastructure.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Project.Infrastructure.Model;

public interface IApplicationDbContext : IDisposable
{
    DbSet<AspNetRole> AspNetRoles { get; set; }
    DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
    DbSet<AspNetUser> AspNetUsers { get; set; }
    DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    DbSet<Link> Links { get; set; }


    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Entry(object entity);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

public partial class ApplicationDbContext : IApplicationDbContext { }