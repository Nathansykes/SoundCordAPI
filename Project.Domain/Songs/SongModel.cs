using Project.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace Project.Domain.Songs;
public class SongModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }
    public string SongName { get; set; } = "";

    [SwaggerSchema(ReadOnly = true)]
    public List<IdNameModel> Revisions { get; set; } = [];

    [SwaggerSchema(ReadOnly = true)]
    public Guid ChannelId { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public string CreatedByUser { get; set; } = "";
}
