using Swashbuckle.AspNetCore.Annotations;

namespace Project.Domain.Songs;
public class SongRevisionModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }
    public string? RevisionName { get; set; }

    public Guid SongId { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public string CreateByUser { get; set; } = "";
}
