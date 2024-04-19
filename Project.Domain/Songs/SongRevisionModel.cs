using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Songs;
public class SongRevisionModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? RevisionName { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public Guid SongId { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public string CreatedByUser { get; set; } = "";


    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedUtc { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public Guid? FileMetadataId { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public Guid ChannelId { get; set; }
}
