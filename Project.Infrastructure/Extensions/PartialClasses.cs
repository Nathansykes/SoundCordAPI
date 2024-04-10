using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Model;

public partial class ApplicationDbContext : IApplicationDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {

    }
}