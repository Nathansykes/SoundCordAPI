using Project.Generic;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Channels;

public class ChannelModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public Guid GroupId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string ChannelName { get; set; } = "";

    [SwaggerSchema(ReadOnly = true)]
    public IdNameModel? Song { get; set; }
}