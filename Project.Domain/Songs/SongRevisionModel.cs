using Swashbuckle.AspNetCore.Annotations;

namespace Project.Domain.Songs;
public class SongRevisionModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }
    public string? RevisionName { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public Guid SongId { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public string CreatedByUser { get; set; } = "";
    [SwaggerSchema(ReadOnly = true)]
    public Guid? FileMetadataId { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public Guid ChannelId { get; set; }
}
