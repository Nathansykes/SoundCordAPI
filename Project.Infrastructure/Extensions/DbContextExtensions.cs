using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Infrastructure.Model.Entities;

namespace Project.Infrastructure.Model;

public partial class ApplicationDbContext(ICurrentUserAccessor currentUserAccessor) : IApplicationDbContext
{
    private readonly ICurrentUserAccessor _currentUserAccessor = currentUserAccessor;

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasQueryFilter(group => group.Users.Any(x => x.Id == _currentUserAccessor.User!.Id));
    }
}