using Swashbuckle.AspNetCore.Annotations;
namespace Project.Generic;

[SwaggerSchema(ReadOnly = true)]
public record IdNameModel(Guid Id, string Name);