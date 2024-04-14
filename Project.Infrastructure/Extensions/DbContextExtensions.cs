using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure.Model;

public partial class ApplicationDbContext : IApplicationDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {

    }
}