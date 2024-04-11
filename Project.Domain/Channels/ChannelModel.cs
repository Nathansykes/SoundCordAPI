using Project.Generic;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Project.Domain.Channels;

public class ChannelModel
{
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public Guid GroupId { get; set; }

    public string ChannelName { get; set; } = "";

    [SwaggerSchema(ReadOnly = true)]
    public IdNameModel? Song { get; set; }
}