using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain;

public record Channel(Guid Id, string Name, string Description, Guid OwnerId, DateTime CreatedAt, DateTime UpdatedAt);